Public Class Login_CF_IMSCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Login_CF_IMSCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim ooperatore As DllAmbiente.Operatore = Nothing

    
        Dim codUtente As String = context.Request.Form("codUtente")
        Dim codPwd As String = context.Request.Form("codPwd")
        Dim cf As String = context.Request.Form("cf")
        If ooperatore Is Nothing Then
            ooperatore = New DllAmbiente.Operatore
        End If
        Dim link_result As String = ""
        Dim Errore As String = ""
        Log.Debug("Autenticazione Utente: " & codUtente)
        ooperatore.Codice = Trim(LCase(codUtente))
        ooperatore.CodiceFiscale = cf
        Try
            ooperatore.Controlla_Autenticazione(codPwd)
            context.Session.Add("oOperatore", ooperatore)

            Log.Debug("Autenticazione Riuscita con utente: " & ooperatore.Codice)
            ooperatore.Update_Operatore(ooperatore)
            ' context.Response.Redirect("HomePageAction.aspx")
            link_result = "HomePageAction.aspx"
        Catch ex As DllAmbiente.LoginException
            If ex.Esito = "2" Then
                context.Session.Add("oOperatore", ooperatore)
                ' context.Response.Redirect("ProfiloOperatoreAction.aspx")
                Log.Debug("Autenticazione Riuscita con utente: " & ooperatore.Codice)
                ooperatore.Update_Operatore(ooperatore)
                context.Session.Add("esito", ex.Message)
                'context.Response.Redirect("HomePageAction.aspx")
                link_result = "HomePageAction.aspx"
            Else
                Log.Error("Esito Controlla_Autenticazione: " & ex.Message)
                'context.Session.Add("esito", ex.Message)
                'context.Response.Redirect("./login.aspx")
                link_result = ""
                Errore = ex.Message
            End If


        End Try



        context.Response.Clear()
        context.Response.ClearHeaders()


        Dim str_return As String = ""

        If Errore = "" Then
            context.Response.Write("{ success: true, link: '" & link_result & "'}")
        Else
            context.Response.Write("{ success: false, FaultMessage: '" & Errore.Replace("'", "\'") & "'}")
        End If
        context.Response.Flush()
        context.Response.Close()


    End Sub

End Class
