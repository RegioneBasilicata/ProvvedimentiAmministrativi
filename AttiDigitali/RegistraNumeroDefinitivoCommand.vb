Public Class RegistraNumeroDefinitivoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Session.Item("key")
        Dim numeroDefinitivo As String = context.Session.Item("numeroDefinitivo")
        Dim vettoredati As Object
        Dim obj As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
        vettoredati = Aggiorna_Documento(obj, numeroDefinitivo)
        If vettoredati(0) = 0 Then
            context.Session.Add("msgEsito", "Il Documento è stato numerato con successo")
        Else
            context.Session.Add("msgEsito", "Il Documento non è stato numerato con successo. " & vettoredati(1))
        End If
    End Sub
End Class
