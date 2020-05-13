Public Class DettaglioitemContabile
    Inherits System.Web.UI.Page

    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm

    Protected WithEvents Testata As Global.System.Web.UI.WebControls.PlaceHolder

    Protected WithEvents Albero As Global.System.Web.UI.WebControls.PlaceHolder

    Protected WithEvents Contenuto As Global.System.Web.UI.WebControls.PlaceHolder

    Protected WithEvents DetailsView1 As Global.System.Web.UI.WebControls.DetailsView

    Protected WithEvents ObjectDetail As Global.System.Web.UI.WebControls.ObjectDataSource


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Dettaglio Item Contabile")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        Else
            CType(CType(Albero.Controls(1).Controls(1), System.Web.UI.Control), Microsoft.Web.UI.WebControls.TreeView).Nodes.RemoveAt(1)

        End If
    End Sub

End Class