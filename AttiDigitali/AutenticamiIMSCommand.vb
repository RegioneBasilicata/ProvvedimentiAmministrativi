Public Class AutenticamiIMSCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AutenticamiIMSCommand))
    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Try

            Log.Warn("codice fiscale prima context.Request.Headers('shib-fiscalNumber')" & context.Request.Headers("shib-fiscalNumber"))
            Log.Warn("codice fiscale prima context.Request.Headers('fiscalNumber')" & context.Request.Headers("fiscalNumber"))

            Dim listaScelte As Generic.List(Of Ext_SceltaOperatoreInfo) = ValidaRichiesta(context)
            Select Case listaScelte.Count
                Case 0
                    context.Response.Clear()

                    context.Server.Transfer("Login_CF_IMS.aspx", True)


                    context.Response.Close()
                    'context.Response.Redirect("Login_CF_IMS.aspx", False)
                Case 1
                    Dim ooperatore As New DllAmbiente.Operatore
                    ooperatore.Codice = listaScelte.Item(0).CodiceOperatore
                    context.Session.Add("oOperatore", ooperatore)
                    context.Response.Redirect("HomePageAction.aspx", False)

                Case Is > 1
                    'Log.Debug("codice fiscale case >1" & context.Request.Headers("fiscalcode"))
                    'context.Session.Add("CodiceFiscale", context.Request.Headers("fiscalcode"))
                    ' per futura integrazione spid, decommentare le seguenti linee di codice e commentare le precendenti
                    If Not String.IsNullOrEmpty(context.Request.Headers("fiscalNumber")) Then
                        context.Session.Add("CodiceFiscale", context.Request.Headers("fiscalNumber"))
                    ElseIf Not String.IsNullOrEmpty(context.Request.Headers("shib-fiscalNumber")) Then
                        context.Session.Add("CodiceFiscale", context.Request.Headers("shib-fiscalNumber"))
                    End If

                    context.Session.Add("sceltaOperatore", listaScelte)
                    context.Response.Redirect("SceltaOperatoreAction.aspx", False)
            End Select
        Catch ex As System.Threading.ThreadAbortException


        Catch ex As Exception
            context.Session.Add("error", ex.Message)
            context.Response.Redirect("Errore.aspx")
        End Try
    End Sub
    Function ValidaRichiesta(ByVal context As HttpContext) As Generic.List(Of Ext_SceltaOperatoreInfo)
        Dim listasceltePresentation As New Generic.List(Of Ext_SceltaOperatoreInfo)
        Try
            Log.Warn("Inizio Chiamata ValidaRichiesta")
            Dim urlAutorizzato As String = System.Configuration.ConfigurationManager.AppSettings("URLAUTORIZZATI")
            Dim isAutorizzato As Boolean = False
            'se non specificato l'url nel web.config non effettua il controllo sull'url chiamante
            If Not String.IsNullOrEmpty(urlAutorizzato) Then

                If urlAutorizzato.Contains(";") Then
                    'il valore della richiesta è multiplo
                    Dim urlAutorizzati As String()
                    Dim separatore As New Char
                    separatore = ";"
                    urlAutorizzati = urlAutorizzato.Split(separatore)
                    For Each urlConsentito As String In urlAutorizzati
                        If Not String.IsNullOrEmpty(urlConsentito) Then
                            If Not String.IsNullOrEmpty(context.Request.ServerVariables("REMOTE_USER")) AndAlso context.Request.ServerVariables("REMOTE_USER") = urlConsentito Then
                                isAutorizzato = True
                            End If
                        End If
                    Next
                Else
                    'il valore è singolo
                    If Not String.IsNullOrEmpty(context.Request.ServerVariables("REMOTE_USER")) AndAlso context.Request.ServerVariables("REMOTE_USER") = urlAutorizzato Then
                        isAutorizzato = True
                    End If
                End If
            Else
                'se .AppSettings("URLAUTORIZZATI") è vuoto non si effettau alcun controllo quindi è sempre autorizzato
                isAutorizzato = True
            End If

            If isAutorizzato Then
                'se l'url è autorizzato, verifico il cf che mi èp stato inviato
                'Dim codiceFiscaleDaVerificare As String = context.Request.Headers("fiscalNumber")
				' per futura integrazione spid, decommentare la seguente linea di codice e commentare la precendente
                Dim codiceFiscaleDaVerificare As String = ""

                If Not String.IsNullOrEmpty(context.Request.Headers("fiscalNumber")) Then
                    codiceFiscaleDaVerificare = context.Request.Headers("fiscalNumber")
                ElseIf Not String.IsNullOrEmpty(context.Request.Headers("shib-fiscalNumber")) Then
                    codiceFiscaleDaVerificare = context.Request.Headers("shib-fiscalNumber")
                End If

                If Not String.IsNullOrEmpty(codiceFiscaleDaVerificare) Then
                    Log.Warn("Richiesta Accesso " & codiceFiscaleDaVerificare)
                    Dim op As New DllAmbiente.Operatore
                    Dim listascelte As Generic.List(Of DllAmbiente.SceltaOperatoreInfo) = op.Leggi_Dati_CF(codiceFiscaleDaVerificare)

                    If Not listascelte Is Nothing Then

                        For Each scelta As DllAmbiente.SceltaOperatoreInfo In listascelte
                            Dim sceltaPresentation As New Ext_SceltaOperatoreInfo
                            sceltaPresentation.CodiceOperatore = scelta.CodiceOperatore
                            sceltaPresentation.CodiceUfficio = scelta.CodiceUfficio
                            sceltaPresentation.DescrizioneUfficio = scelta.DescrizioneUfficio
                            listasceltePresentation.Add(sceltaPresentation)
                        Next
                    Else
                        Log.Error("Impossibile trovare c.f. " & codiceFiscaleDaVerificare)
                        Throw New Exception("Non ci sono operatori abilitati all'accesso con il codice fiscale specificato: " & codiceFiscaleDaVerificare)
                    End If
                End If
            End If

        Catch ex As Exception
            Log.Error("ValidaRichiesta" & ex.Message)
            Throw New Exception(ex.Message)
        End Try
        Return listasceltePresentation
    End Function
End Class
