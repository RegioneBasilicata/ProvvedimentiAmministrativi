Public Class Index
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Index))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Verifico se l'autenticazione è a carico dell'applicazione o tramite servizio di autenticazione esterno
        'Leggo la chiave nel web.config 
        Log.Debug("TIPO AUTENTICAZIONE: " & ConfigurationManager.AppSettings("AUTENTICAZIONE"))

        Select Case ConfigurationManager.AppSettings("AUTENTICAZIONE")
            Case "IMS"
                
                'nel caso di autenticazione da parte di un servizio esterno, vengo rediretta direttamente sulla pagina che effettua il match
                'HttpContext.Current.Session.Add("error", "L'accesso all'applicazione non può avvenire con username e password. E' necessario autenticarsi dall'area riservata")
                'Response.Redirect("Errore.aspx", False)
                Response.Redirect("AutenticamiIMSAction.aspx", False)
            Case "LOGIN"
                Context.Session.RemoveAll()
                Context.Session.Clear()
                Context.Session.Abandon()
                Server.Transfer("Login.aspx")
            Case "LOGINCF"
                Context.Session.RemoveAll()
                Context.Session.Clear()
                Context.Session.Abandon()
                Server.Transfer("Login_CF.aspx")
            Case Else
                HttpContext.Current.Session.Add("error", "Nessuna configurazione per l'autenticazione. Inserire un valore valido nel file di configurazione.")
                Response.Redirect("MessaggioErrore.aspx")
        End Select
     
        

    End Sub

End Class
