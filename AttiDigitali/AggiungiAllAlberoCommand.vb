Imports Microsoft.Web.UI.WebControls
Public Class AggiungiAllAlberoCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        RedirectingCommand.Log.Debug(System.Reflection.MethodInfo.GetCurrentMethod)
        Dim key As String = context.Request.Params("key")

        context.Session.Add("codDocumento", key)

        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            txtDataInizio = Now.AddMonths(-1)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If


        Dim vr As Object = Nothing

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(key)
        If LCase(statoIstanza.Operatore) = LCase(oOperatore.Codice) Then
            vr = Verifica_Prima_Apertura(key)

            If vr(0) = 0 Then
                aggiorna_Documenti_Sessione(context, "APRI", key, vr(2), vr(1))
            End If
        End If


        Dim vettoredati As Object
        Dim tipoApplic As Integer
        Dim tipoAllegato As Integer
        Dim tipoAllegatoFirmato As Integer
        Dim vettoreDocumento As Array
        Dim vettoreDocumentoFirmato As Array
        Dim vAllegati As Object
        Dim codiceUfficioProponente As String = ""

         vettoredati = Elenco_Allegati(key)
        codiceUfficioProponente = vettoredati(3)
        If vettoredati(0) = 0 Then
            If Not oOperatore.oUfficio.CodUfficio = codiceUfficioProponente Then

                If Not context.Session.Item("tipoApplic") Is Nothing Then
                    tipoApplic = Integer.Parse(context.Session.Item("tipoApplic"))
                Else
                    tipoApplic = Integer.Parse(vettoredati(2))
                End If

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

                vettoreDocumento = estraiUltimaVersione(key, tipoAllegato)
                vettoreDocumentoFirmato = estraiUltimaVersione(key, tipoAllegatoFirmato)
                vAllegati = estraiAllegati(vettoredati(1))
                If vettoreDocumentoFirmato(0) = 0 Then
                    vettoreDocumento(1) = unisciVettori(vettoreDocumento(1), vettoreDocumentoFirmato(1))
                End If
                If vAllegati(0) = 0 Then
                    vettoredati(1) = unisciVettori(vettoreDocumento(1), vAllegati(1))
                Else
                    vettoredati(1) = vettoreDocumento(1)
                End If
            'Else 
            '    If Not vettoredati(1).Equals("Nessun Record Trovato") Then
            '        vettoredati = estraiAllegati(vettoredati(1))
            '    End If 
            End If
        End If



        context.Items.Add("vettoreDati", vettoredati)

        vettoredati = Elenco_Compiti_Documento(key)
        context.Items.Add("vettoreCompiti", vettoredati)
        'modgg 10-06
        If context.Items.Item("vettoreDocumento") Is Nothing Then
            vettoredati = Leggi_Documento(key)
            context.Items.Add("vettoreDocumento", vettoredati)
        End If

        context.Items.Add("tipoApplic", vettoredati(2))
    End Sub
End Class
