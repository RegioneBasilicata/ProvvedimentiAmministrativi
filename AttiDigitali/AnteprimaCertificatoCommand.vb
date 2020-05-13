Public Class AnteprimaCertificatoCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AnteprimaCertificatoCommand))
    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim vettoredati As Object
        Dim codAllegato As String
        Dim binarioFile() As Byte

        codAllegato = context.Request.QueryString.Get("key")
        vettoredati = Anteprima_Allegato(codAllegato)

        Log.Debug("ANTEPRIMA CERT - Id Allegato: " & codAllegato)
        Log.Debug("ANTEPRIMA CERT - Rit. Anteprima_Allegato: " & vettoredati(0))

        If vettoredati(0) = 0 Then
            binarioFile = vettoredati(1)
            context.Response.AddFileDependency("certificato.cer")
            context.Response.ContentType = "application/x-x509-ca-cert"
            context.Response.BinaryWrite(vettoredati(5))
        End If

        context.Response.Flush()
        context.Response.End()
        'context.Response.Close()

    End Sub
End Class
