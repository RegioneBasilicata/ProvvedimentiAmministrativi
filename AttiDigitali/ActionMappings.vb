Imports System.Collections.Specialized

Public Class ActionMappings
    Private Shared singletonActionMappings As ActionMappings = Nothing
    Public mappings As NameValueCollection = Nothing

    Private Sub New()
        mappings = CType(ConfigurationManager.GetSection("actionMappings"), NameValueCollection)
    End Sub

    Public Shared Function getInstance() As ActionMappings
        If (singletonActionMappings Is Nothing) Then
            singletonActionMappings = New ActionMappings
        End If
        Return singletonActionMappings
    End Function
End Class
