Public Class login
    Inherits Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblEsitoAutenticazione As System.Web.UI.WebControls.Label
    Protected WithEvents hiddenCF As Global.System.Web.UI.WebControls.HiddenField
    Protected WithEvents flagNascondiCF As Global.System.Web.UI.WebControls.HiddenField

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me,"Login")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If Not IsPostBack Then
            'Verifico se l'autenticazione è a carico dell'applicazione o tramite servizio di autenticazione esterno
            'Leggo la chiave nel web.config 
            Select Case ConfigurationManager.AppSettings("AUTENTICAZIONE")
                Case "IMS"
                    'nel caso di autenticazione da parte di un servizio esterno, vengo rediretta direttamente sulla pagina che effettua il match
                    ' HttpContext.Current.Session.Add("error", "L'accesso all'applicazione non può avvenire con username e password. E' necessario autenticarsi dall'area riservata")
                    'Response.Redirect("Errore.aspx", False)
                    Response.Redirect("AutenticamiIMSAction.aspx", False)
                Case "LOGIN"
                    flagNascondiCF.Value = "1"
                   
                Case "LOGINCF"
                    Context.Session.RemoveAll()
                    Context.Session.Clear()
                    Context.Session.Abandon()
                    Server.Transfer("Login_CF.aspx")
                Case Else
                    HttpContext.Current.Session.Add("error", "Nessuna configurazione per l'autenticazione. Inserire un valore valido nel file di configurazione.")
                    Response.Redirect("MessaggioErrore.aspx")
            End Select

        End If
        
    End Sub

End Class
