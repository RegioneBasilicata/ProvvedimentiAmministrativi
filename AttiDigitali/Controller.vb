Public Class Controller
    Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'acquisisco il Command da eseguire in base ai mappings configurati nel Web.config
        Dim command As ICommand = CommandFactory.make(context.Request.Params)
        'invoco il Command
        command.execute(context)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property
End Class
