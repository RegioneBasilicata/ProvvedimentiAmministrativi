Public Class AvvisoRedirect
    Inherits Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione � richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo � richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Evento Inatteso")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Session.Remove("oOperatore")
        '" Verrai reindirizzato tra <input type="text" size="2" name="counter"> secondi"


        'LabelErrore = New Label
        'LabelErrore.CssClass = "lblErrore"
        'LabelErrore.Text = "ATTENZIONE! " & "<br/>" & _
        '                   HttpContext.Current.Session("error")
        'Contenuto.Controls.Add(LabelErrore)

        'Dim input As New HtmlInputText

        'input.ID = "counter"
        'input.Name = "counter"
        'input.Value = 5
        'Contenuto.Controls.Add(input)

    End Sub

End Class
