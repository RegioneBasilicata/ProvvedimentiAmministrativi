Imports Microsoft.web.UI.WebControls

Public Class tree
    Inherits System.Web.UI.UserControl


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TreeView1 As Microsoft.Web.UI.WebControls.TreeView

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(tree))
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Dim alberoCorrente As TreeView

        alberoCorrente = context.Session.Item("tree")

        If alberoCorrente Is Nothing Then

            Context.Session.Add("TreeView1", TreeView1)

            ' il codice precedente è stato tutto fattorizzato in questa funzione pubblica (refreshAlbero)
            FunzWeb.refreshAlbero(Context)

        Else
            alberoCorrente.ExpandLevel() = 0
            alberoCorrente.SelectedNodeIndex() = 0
            alberoCorrente.SelectExpands() = True
            TreeView1 = alberoCorrente
        End If

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Public Sub refreshTree(ByRef root As TreeNode)
        TreeView1.Nodes.Clear()
    End Sub

End Class
