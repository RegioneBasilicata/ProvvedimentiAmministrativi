Imports System.Net

Public Class FATTURAELETTRONICAServiceOverride
    Inherits FATTURAELETTRONICAService

    Protected Overrides Function GetWebRequest(ByVal uri As System.Uri) As WebRequest
        Dim webRequest As HttpWebRequest = DirectCast(MyBase.GetWebRequest(uri), HttpWebRequest)
        webRequest.KeepAlive = False
        webRequest.ProtocolVersion = HttpVersion.Version10
        Return webRequest

    End Function

End Class
