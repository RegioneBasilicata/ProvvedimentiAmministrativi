Public Class formStampa
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents idOp As System.Web.UI.WebControls.TextBox
    Protected WithEvents idLinkToPrint As System.Web.UI.WebControls.TextBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim op As DllAmbiente.Operatore = CType(Session("oOperatore"), DllAmbiente.Operatore)
        idOp.Text = op.Codice
        idLinkToPrint.Text = "AnteprimaAllegatoAction.aspx"
        'AnteprimaAllegatoAction.aspx
        'AnteprimaAllegatoArchivioAction.aspx


        Dim idDoc As String = Request.QueryString("idDoc")
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(op)).Get_StatoIstanzaDocumento(idDoc)
        Dim livelloIstanza As String = statoIstanza.LivelloUfficio

        If livelloIstanza = "UAR" Then
            If op.Test_Gruppo("Politico") Or op.Test_Gruppo("ArchGen") Then
                idLinkToPrint.Text = "AnteprimaAllegatoArchivioAction.aspx"
            End If


        End If

    End Sub

End Class
