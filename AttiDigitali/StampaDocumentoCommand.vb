Public Class StampaDocumentoCommand
    Inherits RedirectingCommand
    'modgg16
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim codDocumento As String
        ' vettore Allegati
        codDocumento = context.Request.QueryString.Get("key")
        vettoredati = Elenco_Allegati(codDocumento, , , "1")
        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
