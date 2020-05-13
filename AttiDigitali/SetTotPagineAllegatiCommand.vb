
Public Class SetTotPagineAllegatiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim totalePagineAllegati As String = ""
        Dim vettoredati As Object
        Dim tipoApplic As Integer
        Dim tipoAllegato As Integer
        Dim tipoAllegatoFirmato As Integer
        Dim vettoreDocumento As Array
        Dim vettoreDocumentoFirmato As Array
        Dim vAllegati As Object
        Dim codiceUfficioProponente As String = ""


        Dim codDocumento As String = context.Session.Item("codDocumento")

        Dim oDllDocumenti As New DllDocumentale.svrDocumenti(oOperatore)
        Dim resetPagineAllegati As Boolean = context.Session.Item("resetTotalePagineAllegati")
        If resetPagineAllegati Then
            Dim item As New DllDocumentale.Documento_attributo
            item.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
            item.Doc_id = codDocumento
            item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")

            oDllDocumenti.FO_Delete_Documento_Attributi(item)
        Else
            Dim NumAllegati As Integer = oDllDocumenti.Conta_allegatiPerDocumento(codDocumento)
            If NumAllegati > 0 Then
                totalePagineAllegati = context.Session.Item("totalePagineAllegati") & ""
                If totalePagineAllegati <> "" Then
                    Dim totalePagine As Integer
                    If Integer.TryParse(totalePagineAllegati, totalePagine) Then
                        Dim itemPagAllegati As New DllDocumentale.Documento_attributo
                        itemPagAllegati.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
                        itemPagAllegati.Doc_id = codDocumento
                        itemPagAllegati.Valore = totalePagine
                        itemPagAllegati.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")

                        oDllDocumenti.FO_Registra_Attributo(itemPagAllegati, oOperatore)

                        context.Items.Add("lblOK", "Totale delle pagine degli allegati registrato correttamente")
                    Else
                        context.Items.Add("lblErrorNpagAlleg", "Il valore inserito per il totale delle pagine degli allegati deve essere numerico")
                    End If
                Else
                    context.Items.Add("lblErrorNpagAlleg", "Specificare il numero totale delle pagine degli allegati")
                End If
            Else
                context.Items.Add("lblErrorNpagAlleg", "Allegati non presenti: impossibile specificare il numero totale di pagine.")
            End If
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

End Class
