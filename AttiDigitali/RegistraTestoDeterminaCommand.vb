Public Class RegistraTestoDeterminaCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String
        Dim testoDeterminaFile() As Byte
        Dim estensione As String
        Dim nomeFile As String
        Dim vR As Object = Nothing

        codDocumento = context.Session.Item("key")
        testoDeterminaFile = context.Session.Item("testoDetermina")
        estensione = context.Session.Item("estensione")
        nomeFile = context.Session.Item("nomeFile")

        vR = Elenco_Allegati(codDocumento, 13)

        If vR(0) = 0 Then

            Cancella_Allegati(vR(1)(0, 0))
        End If

        vR = Nothing
        vR = Registra_Allegato(testoDeterminaFile, nomeFile, estensione, codDocumento, 13)




        context.Session.Add("key", codDocumento)
    End Sub
End Class
