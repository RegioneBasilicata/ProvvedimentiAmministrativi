Public Class RedirectClearCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(RedirectClearCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim suffix As String = ".success"
        Dim url As String = HttpContext.Current.Request.Params("PATH_INFO")
        Log.Debug("CLEAR - Url Chiamante: " & url)
        Dim Arrayurl As Array = url.Split("/")
        Dim url1 As String = Arrayurl(2)
        url1 = url1.Substring(1)
        Dim url2 As String = Arrayurl(0) & "/" & Arrayurl(1) & "/" & url1
        'ridireziono l'utente sulla url ricavata
        HttpContext.Current.Session.Clear()
        'acquisisco la url da richiamare ricercando nei mappings configurati nel Web.config
        'ridireziono l'utente sulla url ricavata
        Log.Debug("CLEAR - Url Da Redirigere: " & url2)
        context.Response.Redirect(url2)
    End Sub
End Class
