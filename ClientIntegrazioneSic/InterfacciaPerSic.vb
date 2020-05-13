Imports Intema.XmlValidator

Public Class InterfacciaPerSic
    Public Sub controlloValiditaXml(ByVal xmlMessage As String)
        Dim esito As Boolean = False
        'Carico la configurazione per lo schema xsd
        Dim schemas As String = System.Configuration.ConfigurationManager.AppSettings("schemas")
        Try
            'Valido il messaggio
            Dim fv As New FileValidator(xmlMessage, schemas)
            fv.Validate()
        Catch ex As FileValidatorException
            'Log.Error("Messaggio non valido: " & ex.Message & ". Controllare la conformità con l'xsd. ")
            Throw New FileValidatorException("Messaggio non valido: " & ex.Message & ". Controllare la conformità con l'xsd. ")
        Catch ex As Exception
            'Log.Error("Messaggio non valido: " & ex.Message)
            Throw New Exception(ex.Message)
        End Try

        'Log.Debug("Fine Chiamata a controlloValiditaXml")
    End Sub

End Class
