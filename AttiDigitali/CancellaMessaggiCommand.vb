Public Class CancellaMessaggiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim elencoMessaggiDaCancellare As String
        Dim vMessaggiDaCancellare() As String
        Dim i As Integer

        elencoMessaggiDaCancellare = context.Session.Item("elencoMessaggiDaCancellare")

        vMessaggiDaCancellare = Split(elencoMessaggiDaCancellare, ",")
        For i = 0 To UBound(vMessaggiDaCancellare)
            If Trim(vMessaggiDaCancellare(i)) <> "" Then
                Call Cancella_Messaggio(vMessaggiDaCancellare(i))
            End If
        Next

        Dim vettoredati As Object
        vettoredati = Elenco_Messaggi()
        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
