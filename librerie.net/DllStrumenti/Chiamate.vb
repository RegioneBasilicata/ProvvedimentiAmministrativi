Imports System.Net
Imports System.IO

Public Class Chiamate
    Public Function ChiamaWes(ByVal urlWes As String, ByVal xmlParam As String) As Object
        'funzione per la chiamata di WebServices
        'aliasWes; alias del web service, deve essere definito nel web.config
        'xmlParam; stringa di parametri da passare al web service
        Dim url As Uri
        Dim httpReq As HttpWebRequest
        Dim httpRes As HttpWebResponse
        Dim stream As Stream
        Dim sr As StreamReader
        Dim html As String
        Dim formData As String
        Dim sw As StreamWriter
      
        Try
            url = New Uri(urlWes)

            formData = xmlParam

            httpReq = DirectCast(WebRequest.Create(url), HttpWebRequest)
            httpReq.Method = "POST"
            httpReq.ContentType = "application/x-www-form-urlencoded"
            httpReq.ContentLength = formData.Length
            'rocco 11-05-2006 gestire l'autenticazione  su zope
            'httpReq.Credentials()

            sw = New StreamWriter(httpReq.GetRequestStream)
            sw.Write(formData)
            sw.Close()

            httpRes = DirectCast(httpReq.GetResponse, HttpWebResponse)

            stream = httpRes.GetResponseStream
            sr = New StreamReader(stream)

            html = sr.ReadToEnd()
            stream.Close()

            ChiamaWes = html

        Catch ex As Exception
            ChiamaWes = "1#" & ex.ToString
        End Try
    End Function

End Class
