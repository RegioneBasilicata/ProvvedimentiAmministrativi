Public Class OrdinaCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(OrdinaCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim url As String = context.Request.UrlReferrer.PathAndQuery
        Log.Debug("ORDINA - Url Chiamante: " & url)
        Dim indice As String = context.Request.Params.Item("indice")
        context.Session.Add("indice", indice)
        Log.Debug("ORDINA - Indice da ordinare: " & indice)
        context.Items.Add("tipoApplic", context.Items.Item("tipoApplic"))
        Log.Debug("ORDINA - tipo Applicazione: " & context.Items.Item("tipoApplic"))
        context.Response.Redirect(url)
    End Sub
End Class
