Imports System.Net
Imports System.IO

Public Class DownloadAllegatoFatturaCommand
    Implements ICommand

    Public Sub execute(ByVal context As System.Web.HttpContext) Implements ICommand.execute

        Dim urlAllegatoFattura As String = context.Request.QueryString.Get("urlAllegatoFattura")
        Dim nomeAllegatoFattura As String = context.Request.QueryString.Get("nome")
        Dim PWD As String = ConfigurationManager.AppSettings("PWD_ALFRESCO")
        Dim USER As String = ConfigurationManager.AppSettings("USER_ALFRESCO")

        Dim myCache As New CredentialCache()
        myCache.Add(New Uri(urlAllegatoFattura), "Basic", New NetworkCredential(USER, PWD))

        Dim wr As WebRequest = WebRequest.Create(urlAllegatoFattura)
        wr.Credentials = myCache
        wr.Method = WebRequestMethods.File.DownloadFile
        Dim response As WebResponse = wr.GetResponse()
        Dim responseStream As Stream = response.GetResponseStream()

        Dim memoryStream As New MemoryStream
        Dim count As Integer = 0
        Dim buffer(1024) As Byte
        Dim result() As Byte

        count = responseStream.Read(buffer, 0, buffer.Length)
        memoryStream.Write(buffer, 0, count)
        While (count <> 0)
            count = responseStream.Read(buffer, 0, buffer.Length)
            memoryStream.Write(buffer, 0, count)
        End While

        result = memoryStream.ToArray()

        context.Response.ContentType = response.ContentType
        context.Response.AddHeader("Content-Disposition", "attachment; filename=" & nomeAllegatoFattura)
        context.Response.AddHeader("Content-Length", result.Length)
        context.Response.BinaryWrite(result)
        context.Response.Flush()
        context.Response.End()
    End Sub
End Class
