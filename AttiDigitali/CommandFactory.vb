Imports System.Collections.Specialized
Public Class CommandFactory    
    Public Shared Function make(ByVal params As NameValueCollection) As ICommand
        Dim action As String = params("PATH_INFO")
        'in base alla URL di provenienza ottengo il mapping configurato nel Web.config contenente il Command da invocare
        action = action.ToLower.Replace(ConfigurationManager.AppSettings("replaceKey").ToLower, "/AttiDigitali/")


        Dim commandToInvoke As String = ""
        '  commandToInvoke = ActionMappings.getInstance.mappings.Get(action.Replace("/AttiDigitali/", "/AttiDigitali/" & ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")))

        If String.IsNullOrEmpty(commandToInvoke) Then
            commandToInvoke = ActionMappings.getInstance.mappings.Get(action)
        End If


        'creo l'istanza del Command da invocare
        Dim actionType As Type = Type.GetType(commandToInvoke)
        Return CType(Activator.CreateInstance(actionType), ICommand)
    End Function
End Class
