Imports System.Collections.Generic
Imports DllDocumentale

Public Class InoltraBloccoDocumentiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)

        Dim elencoDocumentiDaInoltrare As String
        Dim vDocumentiDaInoltrare() As String
        Dim vR As Object = Nothing
        Dim vRAllegati As Object
        Dim i As Integer
        Dim msgEsito As String
        Dim tipoApp As String

        Dim objDoc As DllDocumentale.Model.DocumentoInfo
        Dim numeroDoc As String = ""
        Dim messaggio As String = ""

        Dim codAzione As String = UCase(context.Session.Item("codAzione"))
        Dim verbo As String = "inoltrare"
        If codAzione = "RIGETTO" Then
            verbo = "rigettare"
        Else
            verbo = "inoltrare"
        End If
        Dim note As String = context.Session.Item("note")
        Dim prossimoAttore As String

        'Se si tratta di una determina la scelta può ricadere fra 0 o 1
        ' se invece è una disposizione la scelta può essere fra 0 e 2, 
        ' perchè le disposizioni non passano dal Controllo Amministrativo
        'destinatarioInoltro = 0 --> Dirigenza Dipartimento
        'destinatarioInoltro = 1 --> Controllo Amministrativo
        'destinatarioInoltro = 2 --> Ufficio Ragioneria 

        Dim destinatarioInoltro As Integer = context.Session.Item("destinatarioInoltro")
        Dim flagUrgente As Boolean = context.Session.Item("flagUrgente")

        elencoDocumentiDaInoltrare = context.Session.Item("elencoDocumentiDaInoltrare")

        vDocumentiDaInoltrare = Split(elencoDocumentiDaInoltrare, ",")
        Dim vettoreDati(UBound(vDocumentiDaInoltrare, 1)) As Object
        If Not context.Session.Item("prossimoAttore") Is Nothing Then
            prossimoAttore = context.Session.Item("prossimoAttore")
        End If

        tipoApp = TipoApplic(context)
        Dim numDetermineUrgenti As Integer = context.Session.Item("numDetermineUrgenti")
        Dim numDisposizioniUrgenti As Integer = context.Session.Item("numDisposizioniUrgenti")


        Dim listaErrori As New ArrayList

        If(vDocumentiDaInoltrare.Length > 0) Then
            If oOperatore.Test_Gruppo("RespUfRg") Then
                Log.Info("**************************************")
                Log.Info("****INIZIO InoltraBloccoDocumentiCommand - Operatore " & oOperatore.Codice & " n° " & vDocumentiDaInoltrare.Length & " atti -" & Now)
                Log.Info("**************************************")
           End If
        End If

        For i = 0 To UBound(vDocumentiDaInoltrare)
            If Trim(vDocumentiDaInoltrare(i)) <> "" Then
                
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                objDoc = Leggi_Documento_Object(vDocumentiDaInoltrare(i))
                numeroDoc = IIf(String.IsNullOrEmpty(objDoc.Doc_numero), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero)
                'controllo oggetto
                If String.IsNullOrEmpty(objDoc.Doc_Oggetto) Then
                    messaggio = "<br/>Impossibile " & verbo & " il provvedimento " & numeroDoc & ": oggetto non valorizzato"
                Else

                    'controllo possibilita di inoltro
                    If Not VerificaAbilitazioneInoltroRigetto(oOperatore, vDocumentiDaInoltrare(i), codAzione) Then
                        messaggio = "<br />Impossibile " & verbo & "  il provvedimento " & numeroDoc & ": inoltro non consentito"
                    Else

                        Dim tipologiaDocumento As Integer
                        Select Case CInt(tipoApp)
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

                        Dim suggerimento As Integer = -1
                        If IsNumeric(context.Session.Item("suggerimento")) Then
                            suggerimento = context.Session.Item("suggerimento")
                        End If

                        vRAllegati = Elenco_Allegati(vDocumentiDaInoltrare(i), tipologiaDocumento)

                        'verifico se è stato allegato il provvedimento
                        If vRAllegati(0) = 1 Then
                            messaggio = "<br/>Impossibile " & verbo & "  il provvedimento " & numeroDoc & ":non è stato caricato alcun documento"
                        Else
                            'verifico se è stato firmato, e se non è firmato ne verifico le intenzioni
                            Dim obbligato As Boolean = False
                            Dim docFirmato As Integer = Verifica_Firma_Utente_Documento(vDocumentiDaInoltrare(i))

                            'Dim docMarcato As Boolean = Verifica_Marca_Utente_Documento(vDocumentiDaInoltrare(i))

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
                                messaggio = "<br/>Impossibile " & verbo & "  il provvedimento " & numeroDoc & ":non è stato firmato il documento"
                            Else


                                Select Case CInt(tipoApp)
                                    Case 0
                                        'Gestione rigetto in blocco determina
                                        If codAzione = "RIGETTO" Then
                                            Dim invDete As New RigettoDocumento
                                            invDete.CancellaPrenotazione(vDocumentiDaInoltrare(i))
                                        End If
                                        'Fine Gestione rigetto in blocco determina
                                        vR = Inoltra_Determina(vDocumentiDaInoltrare(i), codAzione, prossimoAttore, note, suggerimento, destinatarioInoltro, flagUrgente)
                                        Log.Info("***FINE INOLTRO PROVVEDIMENTO - idDocumento " & vDocumentiDaInoltrare(i)  &"  DA PARTE DI - Operatore " & oOperatore.Codice & "  - " & Now)
           
                                    Case 1
                                        vR = Inoltra_Delibera(vDocumentiDaInoltrare(i), codAzione, prossimoAttore, note, , suggerimento)
                                    Case 2
                                        'Gestione rigetto in blocco disposizione
                                        If codAzione = "RIGETTO" Then
                                            Dim invDisp As New RigettoDocumento
                                            Dim flusso As String = DefinisciFlusso(tipoApp)
                                            invDisp.EliminaLiquidazioni(vDocumentiDaInoltrare(i), flusso)
                                        End If
                                        'fine Gestione rigetto in blocco disposizione
                                        vR = Inoltra_Disposizione(vDocumentiDaInoltrare(i), codAzione, prossimoAttore, note, suggerimento, destinatarioInoltro, flagUrgente)
                                    Case 3, 4
                                        vR = Inoltra_AltroAtto(vDocumentiDaInoltrare(i), codAzione, prossimoAttore, note, , suggerimento, CInt(tipoApp))
                                End Select

                                

                                ''Iserire creazione file Unico
                                Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
                                Dim codDocumento As String = vDocumentiDaInoltrare(i)
                                Try
                                    If vR(0) = 0 Then
                                        messaggio = "<br /> E' stato possibile " & verbo & " il provvedimento " & numeroDoc & " con successo."
                                        'se l'inoltro è andato a buon fine, creo il pdf unico
                                        Dim a As AttiDigitaliWS.ServiceMergePDF = New AttiDigitaliWS.ServiceMergePDF
                                        a.BeginMergeFile(oOperatore.Codice, vDocumentiDaInoltrare(i), "", New System.AsyncCallback(AddressOf AsyncCallWs), a)


                                        'se l'inoltro della delibera è andato a buon fine, inoltrata dal Segretario di Presidenza al Presidente
                                        ' devo chiamare il SIC per aggiornare i dati relativamente all'impegno, con il numero definitivo dell'atto con il quale è stato preso.
                                        
                                        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(codDocumento)
                                        If (tipoApp = 1 And statoIstanza.LivelloUfficio = "USA") Then
                                            'rileggo le info del DOC per ottenere il definitivo appena assegnato
                                            Dim objDocumentoAggiornato As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
                                            If objDocumentoAggiornato.Doc_numero <> "" Then
                                                Dim tipoAtto As String = "DELIBERA"
                                                Dim dataAtto As Date = objDoc.Doc_Data
                                                Dim numeroAtto As String = Right(objDocumentoAggiornato.Doc_numero, 5)

                                                Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo)
                                                listaImpegni = dllDoc.FO_Get_DatiImpegni(codDocumento)

                                                Dim flagResult As Boolean = False
                                                Dim dipartimento As String = objDocumentoAggiornato.Doc_Cod_Uff_Pubblico
                                                Dim rispostaGenerazionePreimpegno As String()
                                                Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
                                                Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer
                                                Dim listaPreimpegni As IList(Of DllDocumentale.ItemImpegnoInfo)
                                                listaPreimpegni = dllDoc.FO_Get_DatiPreImpegni(codDocumento)

                                                For Each item As DllDocumentale.ItemImpegnoInfo In listaPreimpegni
                                                    Try
                                                        'rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(operatore, NumPreImpegno, importo, tipoAtto, objDocumento.Doc_Data, numeroAtto, dipartimento, contoEconomico, ratei, importoIrap, risconti, datamovimento, Codice_Obbiettivo_Gestionale, pcf)
                                                        Dim pcf As String
                                                        If Not item.Piano_Dei_Conti_Finanziari Is Nothing Or item.Piano_Dei_Conti_Finanziari <> "" Then
                                                            pcf = item.Piano_Dei_Conti_Finanziari
                                                        Else
                                                            pcf = item.Dli_PianoDeiContiFinanziari
                                                        End If

                                                        Dim numeroProvvisOrDefAtto As String = ""
                                                        If objDocumentoAggiornato.Doc_numero Is Nothing then
                                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numeroProvvisorio
                                                        Else
                                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numero
                                                        End If

                                                        ' in questa chiamata passo il numero di PREIMPEGNO da aggiornare, il numero definitivo dell'atto e tutti i dati relativi.
                                                        'VERIFICARE SE COME DATAMOVIMENTO VA BENE QUELLA DEL DOCUMENTO!!!!!

                                                        'La registrazione in ragioneria avviene uno alla volta.
                                                        'anche se il SIC risponde con un preimpegni multipli, io leggerò sempre e solo il primo numero preimp e il primo docid
                                                        'si sta trasformando il preimp-provv in preimp-def, per cui devo generare un nuovo token per la nuova chiamata al SIC
                                                        Dim hashTokenCallSic As String = GenerateHashTokenCallSic()

                                                        rispostaGenerazionePreimpegno = ClientIntegrazioneSic.MessageMaker.createGenerazionePreImpegnoRagMessage(oOperatore, item.Dli_NPreImpegno, objDocumentoAggiornato.Doc_Oggetto, tipoAtto, objDocumentoAggiornato.Doc_Data, Right(objDocumentoAggiornato.Doc_numero, 5), dipartimento, objDocumentoAggiornato.Doc_Data, codDocumento, numeroProvvisOrDefAtto, hashTokenCallSic)

                                                        Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                                                        Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                                                        Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                                                        Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                                                        Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                                                        Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)

                                                        Dim itemRagPreimp As DllDocumentale.ItemImpegnoInfo = item
                                                        With itemRagPreimp
                                                            .HashTokenCallSic = hashTokenCallSic
                                                            .IdDocContabileSic = idDocSIC1
                                                            .Dli_NPreImpegno = NumPreImpegno1
                                                            .Dli_DataRegistrazione = Now
                                                            .Dli_Operatore = oOperatore.Codice
                                                            .Dli_prog = item.Dli_prog
                                                            .Di_TipoAssunzione = 1
                                                            .Di_Data_Assunzione = objDocumentoAggiornato.Doc_Data
                                                            .Di_Num_assunzione = objDocumentoAggiornato.Doc_numero
                                                            .Di_TipoAssunzioneDescr = tipoAtto
                                                        End With
                                                        dllDoc.FO_Update_Preimpegno(itemRagPreimp)
                                                        'Registra_RagPreimpegno(oOperatore, item.Dli_NPreImpegno, itemRagPreimp)
                                                    Catch ex As Exception
                                                        HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                                                        'Response.Redirect("MessaggioErrore.aspx")
                                                        messaggio = messaggio & "<p style='color:red'>     ***   ATTENZIONE   *** <br />Errore sull'atto " & objDocumentoAggiornato.Doc_numero & " durante la registrazione del preimpegno " & item.Dli_NumImpegno & ": contattare l'amministratore di sistema.</p>"

                                                        Throw ex

                                                    End Try
                                                Next


                                                Dim rispostaGenerazioneImpegno As String()
                                                For Each item As DllDocumentale.ItemImpegnoInfo In listaImpegni
                                                    Try
                                                        'rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(operatore, NumPreImpegno, importo, tipoAtto, objDocumento.Doc_Data, numeroAtto, dipartimento, contoEconomico, ratei, importoIrap, risconti, datamovimento, Codice_Obbiettivo_Gestionale, pcf)
                                                        Dim pcf As String
                                                        If Not item.Piano_Dei_Conti_Finanziari Is Nothing Or item.Piano_Dei_Conti_Finanziari <> "" Then
                                                            pcf = item.Piano_Dei_Conti_Finanziari
                                                        Else
                                                            pcf = item.Dli_PianoDeiContiFinanziari
                                                        End If
                                                        ' in questa chiamata passo il numero di IMPEGNO da aggiornare, il numero definitivo dell'atto e tutti i dati relativi.
                                                        Dim numeroProvvisOrDefAtto As String = ""
                                                        If objDocumentoAggiornato.Doc_numero Is Nothing then
                                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numeroProvvisorio
                                                        Else
                                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numero
                                                        End If
                                                        Dim listaBenReturn As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = dllDoc.FO_Get_ListaBeneficiariImpegno(oOperatore,,, item.Dli_prog)
                                                        Dim arrayBen As Array = ProcAmm.TrasformaBeneficiariInterniInSIC(listaBenReturn)

                                                        'Sto registrando gli impegni definitivi in ragioneria, devo inviare un nuovo token al SIC
                                                        Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                                                        rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(oOperatore, item.Dli_NumImpegno, item.Dli_Costo, tipoAtto, objDocumentoAggiornato.Doc_Data, numeroAtto, dipartimento, item.Di_ContoEconomica, item.Di_Ratei, item.Di_ImpostaIrap, item.Di_Risconti, Now.Date, item.Codice_Obbiettivo_Gestionale, pcf, True, codDocumento, numeroProvvisOrDefAtto, hashTokenCallSic, arrayBen)
                                                    Catch ex As Exception
                                                        HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                                                        'Response.Redirect("MessaggioErrore.aspx")
                                                        messaggio = messaggio & "<p style='color:red'>     ***   ATTENZIONE   *** <br />Errore sull'atto " & objDocumentoAggiornato.Doc_numero & " durante la registrazione dell'impegno " & item.Dli_NumImpegno & ": contattare l'amministratore di sistema.</p>"

                                                        Throw ex
                                                    End Try
                                                Next
                                            End If
                                        End If
                                        messaggio = "<br /> E' stato possibile " & verbo & " il provvedimento " & numeroDoc & " con successo."

                                    Else
                                        messaggio = "<br /><b style='color:red'>Impossibile " & verbo & "</b>  il provvedimento " & numeroDoc & ": inoltro non andato a buon fine."
                                        'If vR(1).ToString().Contains("CONSERVAZIONE_ATTO") Then
                                        '    messaggio += "<br/><br/><b style='color:red'>Problemi per la Conservazione a Norma del provvedimento<br />"
                                        'End If
                                    End If
                                Catch ex As Exception
                                    'SE L'AGGIORNAMENTO DEGLI IMPEGNI NON VA A BUON FINE, BISOGNA RIPOSIZIONARE L'ATTO SULLA SCRIVANIA DI CHI HA APPENA INOLTRATO.                                
                                    ' RipristinaPassoDocumento in questo caso deve essere chiamato 2 volte, la prima per eliminare il record di "arrivo" all'utente destinatario
                                    ' la secoda per eliminare l'inoltro dal mittente.
                                    dllDoc.RipristinaPassoDocumento(codDocumento)
                                    dllDoc.RipristinaPassoDocumento(codDocumento)
                                    messaggio = "<br /><br /><b style='color:red'>Impossibile " & verbo & "</b>  il provvedimento " & numeroDoc & ": inoltro non andato a buon fine.<br />" & messaggio
                                    'If vR(1).ToString().Contains("CONSERVAZIONE_ATTO") Then
                                    '    messaggio += "<br/><br/><b style='color:red'>Problemi per la Conservazione a Norma del provvedimento<br />"
                                    'End If
                                    Log.Error("errore:" & vDocumentiDaInoltrare(i), ex)
                                End Try
                            End If
                        End If
                    End If
                End If
            End If
            listaErrori.Add(messaggio)
        Next

        msgEsito = "ESITO: "
        For Each messaggio In listaErrori
            msgEsito = msgEsito & messaggio
        Next

        Dim oDllDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
        Select Case CInt(tipoApp)
            Case 0
                context.Session.Add("numDetermineUrgenti", oDllDocumenti.ContaDocumentiUrgentiPerOperatoreTot(oOperatore.Codice, 0).Totale)
            Case 2
                context.Session.Add("numDisposizioniUrgenti", oDllDocumenti.ContaDocumentiUrgentiPerOperatoreTot(oOperatore.Codice, 2).Totale)
        End Select

        If oOperatore.Test_Gruppo("RespUfRg") Then
            Log.Info("**************************************")
            Log.Info("****FINE InoltraBloccoDocumentiCommand - Operatore " & oOperatore.Codice & " Firmati e inoltrati n° " & vDocumentiDaInoltrare.Length & " atti con esito: " & msgEsito)
            Log.Info("**************************************")
        End If

        context.Session.Remove("codAzione")
        context.Session.Remove("conFirma")
        context.Session.Remove("prossimoAttore")
        HttpContext.Current.Session.Add("msgEsito", msgEsito)
        context.Items.Add("tipoApplic", tipoApp)

    End Sub


    Sub AsyncCallWs(ByVal A As System.IAsyncResult)

    End Sub
End Class
