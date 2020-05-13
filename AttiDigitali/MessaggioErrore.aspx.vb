Public Class MessaggioErrore
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me,"Evento Inatteso")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        
        LabelErrore = New Label
        LabelErrore.CssClass = "lblErrore"
        LabelErrore.Text = "Si è verificato un evento inatteso " & "<br/>" & _
                           HttpContext.Current.Session("error")
        Contenuto.Controls.Add(LabelErrore)
        Session.Remove("error")
    End Sub

End Class
