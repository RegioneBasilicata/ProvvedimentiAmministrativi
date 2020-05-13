Public Class AllegatiDocumentoCommand
    Inherits RedirectingCommand

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AllegatiDocumentoCommand))
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Log.Info("AllegatiDocumentoCommand => started at " + Now)
        Dim vettoredati As Object
        Dim codDocumento As String
        Dim tipoApplic As Integer
        Dim tipoAllegato As Integer
        Dim tipoAllegatoFirmato As Integer
        Dim vettoreDocumento As Array
        Dim vettoreDocumentoFirmato As Array
        Dim vAllegati As Object
        Dim vAllegatiMarche As Object

        Dim codiceUfficioProponente As String = ""

        codDocumento = context.Request.QueryString.Get("key")
        vettoredati = Elenco_Allegati(codDocumento)
        codiceUfficioProponente = vettoredati(3)
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

        If vettoredati(0) = 0 Then
            If Not oOperatore.oUfficio.CodUfficio = codiceUfficioProponente Then
                
                vettoreDocumento = estraiUltimaVersione(codDocumento, tipoAllegato)
                vettoreDocumentoFirmato = estraiUltimaVersione(codDocumento, tipoAllegatoFirmato)
                vAllegati = estraiAllegati(vettoredati(1))

                vAllegatiMarche = vAllegati(2)

                If vettoreDocumentoFirmato(0) = 0 Then
                    vettoreDocumento(1) = unisciVettori(vettoreDocumento(1), vettoreDocumentoFirmato(1))
                End If
                If vAllegati(0) = 0 Then
                    vettoredati(1) = unisciVettori(vettoreDocumento(1), vAllegati(1))
                Else
                    vettoredati(1) = vettoreDocumento(1)
                End If
            Else
                vettoreDocumento = estraiUltimaVersione(codDocumento, tipoAllegato)
                vettoreDocumentoFirmato = estraiUltimaVersione(codDocumento, tipoAllegatoFirmato)
                If vettoreDocumentoFirmato(0) = 0 Then
                    vettoreDocumento(1) = unisciVettori(vettoreDocumento(1), vettoreDocumentoFirmato(1))
                End If
                If Not vettoreDocumento(1).Equals("Nessun Record Trovato") Then
                    vAllegati = estraiAllegati(vettoredati(1))

                    If Not vAllegati(1).Equals("Nessun Record Trovato") Then
                        vettoredati(1) = unisciVettori(vettoreDocumento(1), vAllegati(1))
                    End If
                End If
            End If
        End If
        
        
        context.Items.Add("vettoreDati", vettoredati)
        context.Items.Add("vettoreDatiMarche", vAllegatiMarche)

        vettoredati = Elenco_Compiti_Documento(codDocumento)
        context.Items.Add("vettoreCompiti", vettoredati)
        'modgg 10-06
        If context.Items.Item("vettoreDocumento") Is Nothing OrElse context.Items.Item("vettoreDocumento")(1).ToString.ToLower = "nessun record trovato" Then
            vettoredati = Leggi_Documento(codDocumento)
            context.Items.Add("vettoreDocumento", vettoredati)
        End If
        Log.Info("AllegatiDocumentoCommand => ended at " + Now)
    End Sub
    
End Class
