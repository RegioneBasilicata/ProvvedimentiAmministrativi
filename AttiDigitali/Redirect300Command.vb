Public MustInherit Class Redirect300Command
    Implements ICommand

    Public oOperatore As DllAmbiente.Operatore
    'modgg 10-06 8
    Protected Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(LoginCommand))
    Protected MustOverride Sub OnExecute(ByVal context As HttpContext)

    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim suffix As String = ".success"
        'modgg 10-06 1
        'Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        oOperatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Try
            'eseguo il metodo che esegue la funzionalità del Command
            'e in base all'esito aggiungo il suffisso success o failure per ottenere il mapping associato
            If oOperatore Is Nothing And (Not context.Request.Params("PATH_INFO").EndsWith("LogoutAction.aspx")) Then
                'oOperatore = New DllAmbiente.Operatore
                oOperatore = context.Session.Item("oOperatore")
            End If
            'modgg 10-06 1
            'commento tutto per il momento
            'If (InStr(context.Request.Path, "autenticatiaction.aspx", CompareMethod.Text) <= 0) And (InStr(context.Request.Path, "LogoutAction.aspx", CompareMethod.Text) <= 0) Then

            '    vR = oOperatore.Controlla_Sessione(context.Session.SessionID, context.Request.Url.ToString)

            '    If vR(0) <> 0 Then
            '        suffix = ".login"
            '        HttpContext.Current.Session.Add("error", vR(1))
            '        Exit Try
            '        oOperatore.domXmlStato = IIf(oOperatore.domXmlStato.InnerXml = "", "", oOperatore.domXmlStato.InnerXml)
            '        oOperatore.Registra_Sessione(context.Session.SessionID, oOperatore.domXmlStato.InnerXml)
            '    End If
            '    'oOperatore.Codice = Trim(vR(2))
            'End If
            OnExecute(context)
        Catch ex As Exception
            suffix = ".failure"
            HttpContext.Current.Session.Add("error", ex.Message)
        End Try

        'acquisisco la url da richiamare ricercando nei mappings configurati nel Web.config
        Dim url As String
        'url = ActionMappings.getInstance.mappings.Get(context.Request.Params("PATH_INFO") + suffix)
        Dim action As String = context.Request.Params("PATH_INFO")
        action = action.ToLower.Replace(ConfigurationManager.AppSettings("replaceKey").ToLower, "/AttiDigitali/")

        url = ActionMappings.getInstance.mappings.Get(action + suffix)
        'controllo se c'è un default
        If url Is Nothing Then
            url = ActionMappings.getInstance.mappings.Get("*" + suffix)
        End If
        'ridireziono l'utente sulla url ricavata
        context.Response.Redirect(url)
    End Sub
End Class
