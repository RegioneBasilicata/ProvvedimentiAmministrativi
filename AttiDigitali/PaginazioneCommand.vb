Public Class PaginazioneCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(PaginazioneCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim url As String = context.Request.UrlReferrer.PathAndQuery
        Dim pagina As String = context.Request.Params.Item("pag")
        context.Session.Add("pagina", pagina)
        Dim tipoApp As String = TipoApplic(context)
        
        context.Items.Add("tipoApplic", tipoApp)
        If url.IndexOf("ChiudiDocumento") > -1 Then
            context.Response.Redirect("WorklistAction.aspx?tipo=" & tipoApp)
        Else
            context.Response.Redirect(url)
        End If

    End Sub
End Class
