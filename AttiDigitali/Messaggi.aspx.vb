Public Class Messaggi
    Inherits WebSession
    'modgg16
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblMessaggi As System.Web.UI.WebControls.Table
    Protected WithEvents btnCancella As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.

        InitializeComponent()
        Inizializza_Pagina(Me,"Messaggi")
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            If Not Page.IsPostBack Then
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
            vr = context.Items.Item("vettoreDati")
            If vr Is Nothing Then
                Exit Sub
            End If
            vr = CType(vr, Array)
            If vr(0) <> 0 And vr(0) <> 1 Then
                LabelErrore.Visible = True
                LabelErrore.Text() = CStr(vr(1))
                Contenuto.Controls.Add(LabelErrore)
            ElseIf vr(0) = 1 Then
                LabelErrore.Visible = True
                LabelErrore.Text() = "Non ci sono messaggi per te"
                Contenuto.Controls.Add(LabelErrore)
                Else
                    btnCancella.Visible() = True
                    Dim objGriglia As New Griglia
                    tblMessaggi = New Table
                    btnCancella.Visible() = True
                    objGriglia.Tabella() = tblMessaggi
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "&nbsp;", "Mittente", "", "Del", "Messaggio", "Leggi"}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, False, True, True, True}
                    objGriglia.ControlloColonna() = New String() {"CHECK", "IMAGE", "", "", "DATA", "SUBSTRING", "LINK"}
                    objGriglia.idControlloColonna() = New String() {"ckMessaggi", "", "", "", "", "35", "Leggi"}
                    objGriglia.VetAzioni() = New String() {"", "", "", "", "", "", "LeggiMessaggioAction.aspx"}
                    objGriglia.Vettore = vr(1)
                    objGriglia.PaginaCorrente = IIf(Context.Session.Item("pagina") Is Nothing, 1, Context.Session.Item("pagina"))
                    objGriglia.Ordina = True
                    objGriglia.IndiceOrdinamento = IIf(Context.Session.Item("indice") Is Nothing, 0, Context.Session.Item("indice"))
                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblMessaggi)

                    Contenuto.Controls.Add(btnCancella)
                End If
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub
    Private Sub btnCancella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Session.Add("elencoMessaggiDaCancellare", Request.Item("ckMessaggi"))
        Response.Redirect("CancellaMessaggiAction.aspx")
    End Sub
End Class
