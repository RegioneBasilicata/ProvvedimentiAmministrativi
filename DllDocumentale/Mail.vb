Imports System.Net.Mail
Imports System.Net.Sockets


Public Class Mail
    Dim msg As MailMessage
    Public Sub New(ByVal subject As String, ByVal body As String, ByVal BccAddress As String, ByVal toAddress As String, ByVal html As Boolean)

        msg = New MailMessage()
        msg.Subject = subject
        msg.Body = body
        Dim toCollection As New MailAddressCollection



        msg.IsBodyHtml = html
        Dim mailAdd As MailAddress
        If BccAddress <> "" Then

            mailAdd = New MailAddress(BccAddress)
            msg.Bcc.Add(mailAdd)

        End If

        For Each toAddr As String In toAddress.Split(CChar(";"))
            mailAdd = New MailAddress(toAddr)
            msg.To.Add(mailAdd)
        Next



    End Sub
    Public Function Send() As Boolean

        Dim client As New SmtpClient
        '      msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess
        client.SendAsync(msg, New Object)


    End Function

    Public Sub New()

    End Sub
End Class