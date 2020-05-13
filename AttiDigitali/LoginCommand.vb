Imports System.ServiceModel

Public Class LoginCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(LoginCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Log.info("LoginCommand - Execute")

        Dim ooperatore As DllAmbiente.Operatore = Nothing
        'Dim codUtente As String = context.Session.Item("codUtente")
        'Dim codPwd As String = context.Session.Item("codPwd")


        Dim codUtente As String = context.Request.Item("codUtente")
        Dim codPwd As String = context.Request.Item("codPwd")

        Dim link_result As String = ""
        Dim errore As String = ""


        If ooperatore Is Nothing Then
            ooperatore = New DllAmbiente.Operatore
        End If
        Log.Debug("Autenticazione Utente: " & codUtente)
        ooperatore.Codice = Trim(LCase(codUtente))
        Try
            ooperatore.Controlla_Autenticazione(codPwd)
            context.Session.Add("oOperatore", ooperatore)
            Log.Debug("Autenticazione Riuscita con utente: " & ooperatore.Codice)
            'context.Response.Redirect("HomePageAction.aspx")

            link_result = "HomePageAction.aspx"
            If "" & ConfigurationManager.AppSettings("MULTI_UTENZA") = "1" Then
                link_result = VerificaCF(ooperatore, context)
            End If


        Catch ex As DllAmbiente.LoginException
            If ex.Esito = "2" Then
                context.Session.Add("oOperatore", ooperatore)
                ' context.Response.Redirect("ProfiloOperatoreAction.aspx")
                Log.Debug("Autenticazione Riuscita con utente: " & ooperatore.Codice)
                context.Session.Add("esito", ex.Message)
                'context.Response.Redirect("HomePageAction.aspx")
                link_result = "HomePageAction.aspx"

                If "" & ConfigurationManager.AppSettings("MULTI_UTENZA") = "1" Then
                    link_result = VerificaCF(ooperatore, context)
                End If

                '   link_result = VerificaCF(ooperatore, context)
            Else
                Log.Error("Esito Controlla_Autenticazione: " & ex.Message)
                'context.Session.Add("esito", ex.Message)
                ' context.Response.Redirect("./login.aspx")
                link_result = ""
                errore = ex.Message
            End If
        End Try


        context.Response.Clear()
        context.Response.ClearHeaders()


        Dim str_return As String = ""

        If errore = "" Then
            context.Response.Write("{ success: true, link: '" & link_result & "'}")
        Else
            context.Response.Write("{ success: false, FaultMessage: '" & errore.Replace("'", "\'") & "'}")
        End If

        context.Response.Flush()
        'context.Response.Close()
        context.Response.End()

    End Sub

    Function VerificaCF(ByVal op As DllAmbiente.Operatore, ByRef context As HttpContext)
        Dim link_result As String = "HomePageAction.aspx"
        If String.IsNullOrEmpty(Trim("" & op.CodiceFiscale)) Then
            Return link_result
        End If
        Dim listascelte As Generic.List(Of DllAmbiente.SceltaOperatoreInfo) = op.Leggi_Dati_CF(op.CodiceFiscale)


        Select Case listascelte.Count
            Case Is > 1

                context.Session.Add("CodiceFiscale", op.CodiceFiscale)
                ' context.Session.Add("sceltaOperatore", listaScelte)
                link_result = "SceltaOperatoreAction.aspx"
            Case Else
                link_result = "HomePageAction.aspx"

        End Select
        Return link_result
    End Function
End Class
