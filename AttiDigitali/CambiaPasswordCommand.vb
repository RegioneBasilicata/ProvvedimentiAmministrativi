Public Class CambiaPasswordCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vecchiaPass As String = context.Session.Item("oldPwd")
        Dim nuovaPassword As String = context.Session.Item("newPwd")
        Dim confNuovaPassword As String = context.Session.Item("newPwdConfirm")
        
        Dim operatore As String = oOperatore.Codice
        Dim vr As Object = oOperatore.cambia_Password(vecchiaPass, nuovaPassword, confNuovaPassword)

        If vr(0) = 0 Then
            context.Items.Add("esitoCambioPass", "La password è stata cambiata con successo")
        Else
            context.Items.Add("esitoCambioPass", "Il cambio della password è fallito " & vr(1))
        End If
    End Sub
End Class
