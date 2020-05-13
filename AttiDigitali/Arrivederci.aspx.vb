Public Class Arrivederci
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblArrivederci As System.Web.UI.WebControls.Label
    Protected WithEvents lnkAutenticazione As System.Web.UI.WebControls.HyperLink

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        
        InitializeComponent()
        Inizializza_Pagina(Me,"Logout")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg17
        'ResolveClientUrl(ResolveUrl("login.aspx"))
        Try
            If Not IsPostBack Then
                Dim tabella As Web.UI.WebControls.Table
                Dim riga As Web.UI.WebControls.TableRow
                Dim cella As Web.UI.WebControls.TableCell

                lblArrivederci.Width = Web.UI.WebControls.Unit.Point(250)
                lblArrivederci.Attributes.Add("wordwrap", True)
                lblArrivederci.Attributes.Add("autosize", True)
                lblArrivederci.CssClass = "lbl"
                lblArrivederci.Text = "Hai appena chiuso la tua sessione di lavoro, per riaprirne un'altra, digitare nome utente e password"
                lblArrivederci.CssClass = "lbl"

                lnkAutenticazione.NavigateUrl = "./Login.aspx"

                tabella = New Web.UI.WebControls.Table
                tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                tabella.HorizontalAlign = HorizontalAlign.Center

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(lblArrivederci)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(lnkAutenticazione)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                Contenuto.Controls.Add(tabella)
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

End Class
