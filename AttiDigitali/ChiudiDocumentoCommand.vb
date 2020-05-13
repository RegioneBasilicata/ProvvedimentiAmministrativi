Public Class ChiudiDocumentoCommand
    Inherits Redirect300Command

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim key As String = context.Request.Params("key")
        Dim tipoApplic As String
        tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", key)
        context.Session.Add("tipoApplic", tipoApplic)
    End Sub
End Class
