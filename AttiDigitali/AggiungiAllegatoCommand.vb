
Public Class AggiungiAllegatoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim nomeAllegato As String
        Dim nomeDocumento As String
        Dim nomeFile As String
        Dim estensione As String = ""
        Dim tipoApplic As Integer
        Dim tipoAllegato As Integer
        Dim tipoAllegatoFirmato As Integer
        Dim vettoreDocumento As Array
        Dim vettoreDocumentoFirmato As Array
        Dim vAllegati As Object
        Dim codiceUfficioProponente As String = ""


        Dim codDocumento As String = context.Session.Item("codDocumento")
        nomeAllegato = context.Session.Item("nomeAllegato") & ""
        nomeFile = context.Session.Item("nomeFileAllegato") & ""
        Dim isCorretto As Boolean = False

        If nomeAllegato <> "" Or nomeFile <> "" Then
            If InStr(nomeFile, ".", CompareMethod.Text) > 0 Then
                estensione = Trim(Split("." & nomeFile, ".")(UBound(Split("." & nomeFile, "."))))
                isCorretto = VerificaEstensione(context, estensione)

            Else
                isCorretto = False
                context.Items.Add("lblError", "Impossibile caricare un file senza estensione.")
            End If
            If isCorretto Then


                If nomeAllegato = "" Then
                    nomeAllegato = Trim(Split(nomeFile, "\")((UBound(Split(nomeFile, "\")))))
                End If

                codDocumento = context.Session.Item("codDocumento")
                Dim bFile() As Byte
                bFile = context.Session.Item("bFileAllegato")

                Registra_Allegato(bFile, nomeAllegato, estensione, codDocumento, 0)

            End If
        End If

        nomeDocumento = context.Session.Item("nomeDocumento") & ""
        nomeFile = context.Session.Item("nomeFileDocumento") & ""

        If nomeDocumento <> "" Or nomeFile <> "" Then
            If InStr(nomeFile, ".", CompareMethod.Text) > 0 Then
                estensione = Trim(Split("." & nomeFile, ".")(UBound(Split("." & nomeFile, "."))))
                isCorretto = VerificaEstensione(context, estensione)
            Else
                isCorretto = False
                context.Items.Add("lblError", "Impossibile caricare un file senza estensione.")
            End If
            If isCorretto Then


                If nomeDocumento = "" Then
                    nomeDocumento = Trim(Split(nomeFile, "\")((UBound(Split(nomeFile, "\")))))
                End If

                codDocumento = context.Session.Item("codDocumento")

                Dim bFile() As Byte = context.Session.Item("bFileDocumento")

                Registra_Allegato(bFile, nomeDocumento, estensione, codDocumento, 4)
            End If
        End If
        tipoApplic = context.Session.Item("tipoApplic")

        If tipoApplic = 1 Then
            Dim oDllDocumenti As New DllDocumentale.svrDocumenti(oOperatore)
            Dim NumAllegati As Integer = oDllDocumenti.Conta_allegatiPerDocumento(codDocumento)
            If NumAllegati > 0 Then

                Dim itemRicercato As New DllDocumentale.Documento_attributo
                itemRicercato.Doc_id = codDocumento
                itemRicercato.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
                itemRicercato.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                Dim lista As Generic.List(Of DllDocumentale.Documento_attributo) = oDllDocumenti.FO_Get_Documento_Attributi(itemRicercato)
                Dim totalePagineAllegato As Integer

                For Each item As DllDocumentale.Documento_attributo In lista
                    If Integer.TryParse(item.Valore, totalePagineAllegato) Then
                    Else
                        context.Items.Add("lblErrorNpagAlleg", "Il valore inserito per il totale delle pagine degli allegati deve essere numerico")
                    End If
                Next

                If totalePagineAllegato = 0 Then
                    context.Items.Add("lblErrorNpagAlleg", "Specificare il numero totale delle pagine degli allegati")
                End If
            Else
                context.Items.Add("lblErrorNpagAlleg", "Allegati non presenti: impossibile specificare il numero totale di pagine.")
            End If
        End If

        nomeDocumento = context.Session.Item("nomeAllegatoVuoto") & ""
        Dim modalita As String = ""
        Dim destinatari As String = ""
        modalita = context.Session.Item("modalitaTrasmissioneAllegatoVuoto")
        destinatari = context.Session.Item("destinatariAllegatoVuoto")

        estensione = ""
        If nomeDocumento <> "" Then


            estensione = "html"

            codDocumento = context.Session.Item("codDocumento")

            Dim arrByte As Byte()
            arrByte = context.Session.Item("bFileCartaceo")
            Registra_Allegato(arrByte, nomeDocumento, estensione, codDocumento, 15, , destinatari, modalita)
        End If






        vettoredati = Elenco_Allegati(codDocumento)
        codiceUfficioProponente = vettoredati(3)

        If vettoredati(0) = 0 Then
            If Not oOperatore.oUfficio.CodUfficio = codiceUfficioProponente Then

                tipoApplic = DirectCast(context.Session.Item("tipoApplic"), Integer)
                Select Case tipoApplic
                    Case 0 'Determina
                        tipoAllegato = 1
                        tipoAllegatoFirmato = 2
                    Case 1 'Delibera
                        tipoAllegato = 5
                        tipoAllegatoFirmato = 6
                    Case 2 'Disposizione
                        tipoAllegato = 8
                        tipoAllegatoFirmato = 9
                End Select

                vettoreDocumento = estraiUltimaVersione(codDocumento, tipoAllegato)
                vettoreDocumentoFirmato = estraiUltimaVersione(codDocumento, tipoAllegatoFirmato)
                vAllegati = estraiAllegati(vettoredati(1))
                If vettoreDocumentoFirmato(0) = 0 Then
                    vettoreDocumento(1) = unisciVettori(vettoreDocumento(1), vettoreDocumentoFirmato(1))
                End If
                If vAllegati(0) = 0 Then
                    vettoredati(1) = unisciVettori(vettoreDocumento(1), vAllegati(1))
                Else
                    vettoredati(1) = vettoreDocumento(1)
                End If
            End If
        End If




        context.Items.Add("vettoreDati", vettoredati)

        vettoredati = Elenco_Compiti_Documento(codDocumento)
        context.Items.Add("vettoreCompiti", vettoredati)

        'modgg 10-06
        If context.Items.Item("vettoreDocumento") Is Nothing Then
            vettoredati = Leggi_Documento(codDocumento)
            context.Items.Add("vettoreDocumento", vettoredati)
        End If

        Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)
        dlldoc.Messagio_WebServiceNotifica(codDocumento, DllDocumentale.svrDocumenti.Stato_Notifica.Modificato, -1, "Aggiunto Allegato")

        context.Response.AppendHeader("key", codDocumento)
    End Sub
    Public Function VerificaEstensione(ByVal context As HttpContext, ByVal estensione As String) As Boolean
        context.Items.Remove("lblError")
        Dim estensioniValide As Collections.Generic.List(Of DllDocumentale.ItemEstensione) = (New DllDocumentale.svrDocumenti(HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")).FO_GetEstensioniValide)
        Dim objEstensione As New DllDocumentale.ItemEstensione
        objEstensione.Nome = estensione
        If Not estensioniValide.Contains(objEstensione) Then
            context.Items.Add("lblError", "Impossibile caricare un file con estensione sconosciuta.")
            Return False
        End If
        Return True

    End Function

End Class
