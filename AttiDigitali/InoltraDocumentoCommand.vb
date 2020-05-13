Imports System.Collections.Generic
Imports DllDocumentale

Public Class InoltraDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Session.Item("codDocumento")
        Dim codAzione As String = context.Session.Item("codAzione")
        Dim note As String = context.Session.Item("note")
        'Se si tratta di una determina la scelta può ricadere fra 0 o 1
        ' se invece è una disposizione la scelta può essere fra 0 e 2, 
        ' perchè le disposizioni non passano dal Controllo Amministrativo
        'destinatarioInoltro = 0 --> Dirigenza Dipartimento
        'destinatarioInoltro = 1 --> Controllo Amministrativo
        'destinatarioInoltro = 2 --> Ufficio Ragioneria 

        Dim destinatarioInoltro As Integer = context.Session.Item("destinatarioInoltro")
        Dim flagUrgente As Boolean = context.Session.Item("flagUrgente")
        Dim prossimoAttore As String
        If Not context.Session.Item("prossimoAttore") Is Nothing Then
            prossimoAttore = context.Session.Item("prossimoAttore")
        End If
        Dim vR As Object = Nothing
        Dim msgEsito As String
        Dim tipoApplic As String
        Dim tipologiaDocumento As Integer

        tipoApplic = context.Request.QueryString.Get("tipo")

        Select Case CInt(tipoApplic)
            Case 0
                tipologiaDocumento = 1
            Case 1
                tipologiaDocumento = 5
            Case 2
                tipologiaDocumento = 8
            Case 3
                tipologiaDocumento = 10
            Case 4
                tipologiaDocumento = 11
        End Select

        vR = Leggi_Documento(codDocumento)

        If vR(0) = 0 Then
            Dim oggetto As String = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto)
            If oggetto Is Nothing Or oggetto = "" Then
                msgEsito = "Impossibile effettuare un inoltro senza prima compilare il provvedimento con un oggetto valido"
                HttpContext.Current.Session.Add("msgEsito", msgEsito)
                Dim vDati As Object
                vDati = Elenco_Documenti(tipoApplic)
                context.Items.Add("vettoreDati", vDati)
                context.Items.Add("tipoApplic", tipoApplic)
                Exit Sub
            End If
        ElseIf vR(0) Is Nothing Or vR(0) = 1 Then
            msgEsito = "Impossibile leggere le informazioni relative al provvedimento necessarie all'inoltro. Riprovare"
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
            Dim vDati As Object
            vDati = Elenco_Documenti(tipoApplic)
            context.Items.Add("vettoreDati", vDati)
            context.Items.Add("tipoApplic", tipoApplic)
            Exit Sub
        End If
        vR = Nothing
        context.Session.Remove("prossimoAttore")
        vR = Elenco_Allegati(codDocumento, tipologiaDocumento)
        If vR(0) = 1 Then
            msgEsito = "Impossibile effettuare un inoltro senza prima caricare il provvedimento"
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
            Dim vDati As Object
            vDati = Elenco_Documenti(tipoApplic)
            context.Items.Add("vettoreDati", vDati)
            context.Items.Add("tipoApplic", tipoApplic)
            Exit Sub
        Else
            vR = Nothing
        End If
        'verifico se è stato firmato, e se non è firmato verifico le intenzioni
        Dim obbligato As Boolean = False

        Dim docFirmato As Integer = Verifica_Firma_Utente_Documento(codDocumento)
        'Dim docMarcato As Boolean = Verifica_Marca_Utente_Documento(codDocumento)

        If docFirmato <= 0 Then
            If (Not context.Session.Item("conFirma") Is Nothing) Then
                obbligato = True
            End If
            If obbligato Then
                msgEsito = "Impossibile effettuare un inoltro senza firmare il provvedimento."
                HttpContext.Current.Session.Add("msgEsito", msgEsito)
                Exit Sub
            End If
        End If
        If obbligato Then
            msgEsito = "Impossibile effettuare un inoltro senza prima firmare il provvedimento"
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
            Exit Sub
        End If

        Dim bFile() As Byte
        bFile = context.Session.Item("noteRigetto")

        If Not bFile Is Nothing Then
            Registra_Allegato(bFile, "Note Rigetto", "p7m", codDocumento, 19, , , , , , , "UCA")
        End If

        Dim suggerimento As Integer = -1
        If IsNumeric(context.Session.Item("suggerimento")) Then
            suggerimento = context.Session.Item("suggerimento")
        End If

        
        Select Case CInt(tipoApplic)
            Case 0
                If codAzione = "RIGETTO" Then
                    Dim invDete As New RigettoDocumento
                    invDete.CancellaPrenotazione(codDocumento)
                End If
                vR = Inoltra_Determina(codDocumento, codAzione, prossimoAttore, note, suggerimento, destinatarioInoltro, flagUrgente)
            Case 1
                vR = Inoltra_Delibera(codDocumento, codAzione, prossimoAttore, note, , suggerimento)
            Case 2
                'Gestione rigetto in blocco disposizione
                If codAzione = "RIGETTO" Then
                    Dim invDisp As New RigettoDocumento
                    Dim flusso As String = DefinisciFlusso(tipoApplic)
                    invDisp.EliminaLiquidazioni(codDocumento, flusso)
                End If
                vR = Inoltra_Disposizione(codDocumento, codAzione, prossimoAttore, note, suggerimento, destinatarioInoltro, flagUrgente)
            Case 3, 4
                vR = Inoltra_AltroAtto(codDocumento, codAzione, prossimoAttore, note, , , CInt(tipoApplic))
        End Select


        If vR(0) = 0 Then
            Dim oDllDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            Try
                'se l'inoltro è andato a buon fine, creo il pdf unico
                Dim a As AttiDigitaliWS.ServiceMergePDF = New AttiDigitaliWS.ServiceMergePDF                
                a.BeginMergeFile(oOperatore.Codice, codDocumento, "", New System.AsyncCallback(AddressOf AsyncCallWs), a)


                Select Case CInt(tipoApplic)
                    Case 0
                        context.Session.Add("numDetermineUrgenti", oDllDocumenti.ContaDocumentiUrgentiPerOperatoreTot(oOperatore.Codice, 0).Totale)
                    Case 2
                        context.Session.Add("numDisposizioniUrgenti", oDllDocumenti.ContaDocumentiUrgentiPerOperatoreTot(oOperatore.Codice, 2).Totale)
                End Select
                msgEsito = "L'operazione è andata a buon fine." & vbCrLf & _
                                   "Consultare lo storico del documento per seguirne l'iter." & msgEsito
            Catch ex As Exception
                'SE L'AGGIORNAMENTO DEGLI IMPEGNI NON VA A BUON FINE, BISOGNA RIPOSIZIONARE L'ATTO SULLA SCRIVANIA DI CHI HA APPENA INOLTRATO.                                
                ' RipristinaPassoDocumento in questo caso deve essere chiamato 2 volte, la prima per eliminare il record di "arrivo" all'utente destinatario
                ' la secoda per eliminare l'inoltro dal mittente.
                oDllDocumenti.RipristinaPassoDocumento(codDocumento)
                oDllDocumenti.RipristinaPassoDocumento(codDocumento)
                Log.Error("Errore :" & ex.Message)
                msgEsito = "Errore : " & msgEsito & "<br /> " & ex.Message
            Finally
                HttpContext.Current.Session.Add("msgEsito", msgEsito)
                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", codDocumento, CInt(tipoApplic))


            End Try




        Else
            msgEsito = "Evento inatteso. L'operazione non è andata a buon fine." & vbCrLf & vR(1)
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
        End If


        If oOperatore.Test_Gruppo("RespUfRg") Then
            Log.Info("**************************************")
            Log.Info("**** Operatore " & oOperatore.Codice & " Firmato e inoltrato n° 1 atto")
            Log.Info("**************************************")
        End If

        Dim vettoredati As Object
        vettoredati = Elenco_Documenti(tipoApplic)
        context.Session.Remove("prossimoAttore")
        context.Items.Add("vettoreDati", vettoredati)
        context.Items.Add("tipoApplic", tipoApplic)



    End Sub


    Sub AsyncCallWs(ByVal A As System.IAsyncResult)

    End Sub



End Class
