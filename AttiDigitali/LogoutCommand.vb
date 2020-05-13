Imports System.Reflection
Public Class LogoutCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        'modgg 10-06 8
        Dim Classe As String = Me.GetType.Name
        Dim funz As String = MethodInfo.GetCurrentMethod().Name
        RedirectingCommand.Log.Debug("INIZIO " & Classe & " Funz: " & funz)
        If Not oOperatore Is Nothing Then
            'modgg 10-06 1
            'oOperatore.Annulla_Sessione()
            oOperatore = Nothing
            'modgg 10-06 4
            Dim operatore As DllAmbiente.Operatore = DirectCast(HttpContext.Current.ApplicationInstance.Context.Session.Item("oOperatore"), DllAmbiente.Operatore)
            operatore.oUfficio = Nothing
            context.Session.Remove("oOperatore")
        End If
        context.Session.RemoveAll()
        context.Session.Clear()
        RedirectingCommand.Log.Debug("RemoveAll da Session")
        context.Session.Abandon()

        RedirectingCommand.Log.Debug("FINE " & Classe & " Funz: " & funz)

        Select Case ConfigurationManager.AppSettings("AUTENTICAZIONE")
            Case "IMS"
                context.Response.Redirect(ConfigurationManager.AppSettings("logoutRedirectIMS"))
            Case "LOGIN"
                context = Nothing
            Case "LOGINCF"
                context.Server.Transfer("Login_CF.aspx")
            Case Else
                context = Nothing
        End Select

        
    End Sub
End Class
