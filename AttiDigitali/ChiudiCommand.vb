'modgg 10-06 9
Imports System.Reflection
Public Class ChiudiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        'stessa funzione di logout
        'modgg 10-06 8
        Dim Classe As String = Me.GetType.Name
        Dim funz As String = MethodInfo.GetCurrentMethod().Name
        RedirectingCommand.Log.Debug("INIZIO " & Classe & " Funz: " & funz)
        If Not oOperatore Is Nothing Then

            'modgg 10-06 4
            Dim operatore As DllAmbiente.Operatore = DirectCast(HttpContext.Current.ApplicationInstance.Context.Session.Item("oOperatore"), DllAmbiente.Operatore)

            


            context.Session.Remove("oOperatore")
            oOperatore = Nothing
        End If
        context.Session.RemoveAll()
        context.Session.Clear()
        RedirectingCommand.Log.Debug("RemoveAll da Session")
        context.Session.Abandon()
        RedirectingCommand.Log.Debug("FINE " & Classe & " Funz: " & funz)
        Select Case ConfigurationManager.AppSettings("AUTENTICAZIONE")
            Case "IMS"
                System.Web.HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings("logoutRedirectIMS"))
            Case "LOGIN"
                System.Web.HttpContext.Current.Response.Write("<script language='javascript'> { window.close();}</script>")
            Case "LOGINCF"
                context.Server.Transfer("Login_CF.aspx")
            Case Else
                context = Nothing
        End Select
    End Sub
End Class
