Public Class AnnullaDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Session.Item("codDocumento")
        Dim codAzione As String = context.Session.Item("codAzione")
        Dim note As String = context.Session.Item("note")
        Dim prossimoAttore As String = ""
        If Not context.Session.Item("prossimoAttore") Is Nothing Then
            prossimoAttore = context.Session.Item("prossimoAttore")
        End If
        Dim vR As Object = Nothing
        Dim msgEsito As String
        Dim tipoApplic As String

        tipoApplic = context.Request.QueryString.Get("tipo")

        Select Case CInt(tipoApplic)
            Case 0
                vR = Inoltra_Determina(codDocumento, codAzione, prossimoAttore, note)
            Case 1
                vR = Inoltra_Delibera(codDocumento, codAzione, prossimoAttore, note)
            Case 2
                vR = Inoltra_Disposizione(codDocumento, codAzione, prossimoAttore, note)
            Case 3, 4
                vR = Inoltra_AltroAtto(codDocumento, codAzione, prossimoAttore, note, , , CInt(tipoApplic))
        End Select


        If vR(0) = 0 Then
            If vR(0) = 0 Then
                msgEsito = "L'operazione è andata a buon fine." & vbCrLf & _
                                                    "Consultare lo storico del documento per seguirne l'iter."
                HttpContext.Current.Session.Add("msgEsito", msgEsito)
                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", codDocumento, CInt(tipoApplic))
            Else
                msgEsito = "Evento inatteso. L'operazione non è andata a buon fine." & vbCrLf & vR(1)
                HttpContext.Current.Session.Add("msgEsito", msgEsito)
            End If
        End If
        context.Session.Remove("prossimoAttore")
        context.Items.Add("tipoApplic", tipoApplic)

    End Sub
End Class
