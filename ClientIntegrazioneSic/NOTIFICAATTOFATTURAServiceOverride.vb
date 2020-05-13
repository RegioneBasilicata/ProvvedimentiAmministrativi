Imports System.Net

Public Class NOTIFICAATTOFATTURAServiceOverride
    Inherits NOTIFICAATTOFATTURAService

    Protected Overrides Function GetWebRequest(ByVal uri As System.Uri) As System.Net.WebRequest
        Dim webRequest As HttpWebRequest = DirectCast(MyBase.GetWebRequest(uri), HttpWebRequest)
        webRequest.KeepAlive = False
        webRequest.ProtocolVersion = HttpVersion.Version10
        Return webRequest

    End Function
End Class
