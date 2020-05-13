Public Class CambiaOpzioniEmailCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim email As String = context.Session.Item("email")
        Dim cf As String = context.Session.Item("cf")
        Dim htOpzioni As Hashtable = context.Session.Item("opzioni")


        oOperatore.Email = email
        oOperatore.CodiceFiscale = cf
        oOperatore.ListaOpzioniMessaggi = htOpzioni

        oOperatore.Update_Operatore(oOperatore)
        Dim returnValue As Integer = oOperatore.Insert_Opzioni_Messaggi(oOperatore)

        If returnValue <> -1 Then
            context.Session.Item("oOperatore") = oOperatore
            context.Items.Add("esitoCambioPass", "Aggiornamento Effettuato con successo.")
        Else
            context.Items.Add("esitoCambioPass", "Aggiornamento non riuscito riprova.")
        End If
    End Sub
End Class
