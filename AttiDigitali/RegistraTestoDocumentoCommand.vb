Public Class RegistraTestoDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String
        Dim testoEditato As String = context.Session.Item("testoEditato")
        Dim vR As Object = Nothing

        codDocumento = context.Session.Item("key")

        vR = Registra_Documento(codDocumento, , testoEditato)

        vR = Leggi_Documento(codDocumento)

        testoEditato = ""
        If vR(0) = 0 Then
            testoEditato = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_testo)
        End If

        context.Items.Add("testoEditato", testoEditato)
        context.Session.Add("key", codDocumento)
    End Sub
End Class
