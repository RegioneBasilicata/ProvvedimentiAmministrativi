Public Class SuccessoDeterminaTesto
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnSalvaTesto As System.Web.UI.WebControls.Button
    Protected WithEvents testoEditato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlAggiungiAllegato As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUploadAllegato As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Testo")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Contenuto.Controls.Add(Label1)
        Dim tabella As Web.UI.WebControls.Table
        Dim riga As Web.UI.WebControls.TableRow
        Dim cella As Web.UI.WebControls.TableCell
        tabella = New Web.UI.WebControls.Table
        tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
        tabella.HorizontalAlign = HorizontalAlign.Center
        riga = New Web.UI.WebControls.TableRow
        cella = New Web.UI.WebControls.TableCell
        riga = New Web.UI.WebControls.TableRow
        cella = New Web.UI.WebControls.TableCell
        cella.ColumnSpan = 2

        riga.Controls.Add(cella)
        tabella.Rows.Add(riga)
        Contenuto.Controls.Add(tabella)

    End Sub

   
End Class
