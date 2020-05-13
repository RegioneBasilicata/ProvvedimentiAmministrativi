Public Class MessaggiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        vettoredati = Elenco_Messaggi()
        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
