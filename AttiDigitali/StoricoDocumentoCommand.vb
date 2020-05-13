Public Class StoricoDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Request.QueryString.Get("key")
        Dim vettoredati As Object
        vettoredati = Storico_Documento(codDocumento)
        context.Items.Add("vettoreDati", vettoredati)
        'modgg 10-06
        If context.Items.Item("vettoreDocumento") Is Nothing Then
            vettoredati = Leggi_Documento(codDocumento)
            context.Items.Add("vettoreDocumento", vettoredati)
        End If

    End Sub
End Class
