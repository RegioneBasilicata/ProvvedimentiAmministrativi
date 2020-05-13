Public Class PrelazionaDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim tipoApplic As String = context.Request.Params.Item("tipo")
        Dim elencoDocDaPrelazionare As String
        Dim vDocDaPrelazionare As Object
        Dim vR As Object = Nothing
        Dim i As Integer

        'modgg28
        Dim esitoCiclo As Boolean = False

        elencoDocDaPrelazionare = context.Session.Item("elencoDocumentiDaPrelazionare")
        vDocDaPrelazionare = Split(elencoDocDaPrelazionare, ",")

        For i = 0 To UBound(vDocDaPrelazionare)
            If Trim(vDocDaPrelazionare(i)) <> "" Then
                Select Case CInt(tipoApplic)
                    Case 0
                        vR = Inoltra_Determina(vDocDaPrelazionare(i), "PRELAZIONE")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 1
                        vR = Inoltra_Delibera(vDocDaPrelazionare(i), "PRELAZIONE")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 2
                        vR = Inoltra_Disposizione(vDocDaPrelazionare(i), "PRELAZIONE")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 3, 4
                        vR = Inoltra_AltroAtto(vDocDaPrelazionare(i), "PRELAZIONE", , , , , CInt(tipoApplic))
                End Select
            End If
        Next

        Dim numDoc As Integer = CInt(UBound(vDocDaPrelazionare))
        Dim msgEsito As String

        If esitoCiclo = True Then

            If numDoc >= 1 Then
                msgEsito = "Il documento è stato prelazionato con successo. Adesso è presente nella tua lista lavoro"
            Else
                msgEsito = "I documenti sono stati prelazionati con successo. Adesso sono presenti nella tua lista lavoro"
            End If
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
        Else
            If numDoc >= 1 Then
                msgEsito = "La prelazione del documento non è avvenuta con successo."
            Else
                msgEsito = "La prelazione dei documenti non è avvenuta con successo."
            End If
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
        End If


        Dim vettoredati As Object
        
        vettoredati = Elenco_Deposito(CInt(tipoApplic))
            
        context.Items.Add("tipoApplic", tipoApplic)
        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
