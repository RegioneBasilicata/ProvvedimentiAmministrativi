Public Class RegistraOsservazioniDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Try
            Dim codDocumento As String = context.Session.Item("key")
            Dim testo As String = context.Session.Item("testo_osservazione")

            context.Session.Remove("testo_osservazione")
            Registra_Osservazione(codDocumento, testo)

            context.Session.Add("key", codDocumento)
        Catch ex As Exception
            Log.Error(ex.Message)
            Throw New Exception(ex.Message)
        End Try
       
        ' context.Items.Add("numeroDoc", numeroDoc)
    End Sub
End Class
