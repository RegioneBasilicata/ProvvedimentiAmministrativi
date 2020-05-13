Public Class RegistraTestoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Session.Item("key")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = context.Session.Item("objDocumento")
        Dim vettoredati As Object
        vettoredati = Aggiorna_Documento(objDocumento, "Doc_Testo")
        If vettoredati(0) = 0 Then
            context.Session.Add("msgEsito", "Il testo è stato registrato con successo")
        Else
            context.Session.Add("msgEsito", "Il testo non è stato registrato con successo. " & vettoredati(1))
        End If
    End Sub
End Class
