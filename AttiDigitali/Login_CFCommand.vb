Imports System.ServiceModel

Public Class Login_CFCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Login_CFCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim ooperatore As DllAmbiente.Operatore = Nothing
        'Dim codUtente As String = context.Session.Item("codUtente")
        'Dim codPwd As String = context.Session.Item("codPwd")


        Dim codFiscale As String = context.Request.Item("cf")
        Dim codPwd As String = context.Request.Item("codPwd")

        Dim link_result As String = ""
        Dim errore As String = ""


        If ooperatore Is Nothing Then
            ooperatore = New DllAmbiente.Operatore
        End If
        Log.Debug("Autenticazione Utente cf: " & codFiscale)

        Try


            link_result = VerificaCF(codFiscale, codPwd, context)
            context.Session.Add("CodiceFiscale", codFiscale)
            If link_result = "" Then
                errore = "Impossibile autenticare Cod. Fiscale: " & codFiscale
            End If
        Catch ex As Exception

            Log.Error("Esito Controlla_Autenticazione: " & ex.Message)
            'context.Session.Add("esito", ex.Message)
            ' context.Response.Redirect("./login.aspx")
            link_result = ""
            errore = ex.Message

        End Try


        context.Response.Clear()
        context.Response.ClearHeaders()


        Dim str_return As String = ""

        If errore = "" Then
            context.Response.Write("{ success: true, link: '" & link_result & "'}")
        Else
            context.Session.Remove("CodiceFiscale")

            context.Response.Write("{ success: false, FaultMessage: '" & errore.Replace("'", "\'") & "'}")
        End If

        context.Response.Flush()
        context.Response.Close()

    End Sub

    Function VerificaCF(ByVal cf As String, ByVal passDigitata As String, ByRef context As HttpContext) As String
        Dim link_result As String = "HomePageAction.aspx"

        Dim op As New DllAmbiente.Operatore
        If String.IsNullOrEmpty(Trim("" & cf)) Then
            Return link_result
        End If

        Dim listascelte As Generic.List(Of DllAmbiente.SceltaOperatoreInfo) = op.Leggi_Dati_CF(cf)


        Select Case listascelte.Count
            Case 0
                op = Nothing
                link_result = ""
            Case Else
                link_result = "HomePageAction.aspx"
                If listascelte.Count > 1 Then
                    link_result = "SceltaOperatoreAction.aspx"
                End If


                ' context.Session.Add("sceltaOperatore", listaScelte)
                For Each opScela As DllAmbiente.SceltaOperatoreInfo In listascelte
                    op = New DllAmbiente.Operatore
                    op.Codice = opScela.CodiceOperatore
                    Try
                        op.Controlla_Autenticazione(passDigitata)
                        context.Session.Add("oOperatore", op)

                        Exit For
                    Catch ex As DllAmbiente.LoginException
                        If ex.Esito = "2" Then
                            context.Session.Add("oOperatore", op)
                            Exit For
                        Else
                            'pass non valida
                            op = Nothing
                        End If
                    End Try

                Next
                If op Is Nothing Then
                    link_result = ""
                    context.Session.Remove("CodiceFiscale")

                End If
        End Select
        Return link_result
    End Function
End Class
