Public Class SedutediGiunta
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ddlElencoSedute As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnAggiungiAllaSeduta As System.Web.UI.WebControls.Button
    Protected WithEvents btnRimuoviDallaSeduta As System.Web.UI.WebControls.Button
    Protected WithEvents txtNuovaSeduta As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnNuovaSeduta As System.Web.UI.WebControls.Button
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents pnlSeduta As System.Web.UI.WebControls.Panel

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Call Inizializza_Pagina(Me, "Seduta di Giunta")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina


        If Not Page.IsPostBack Then
            Try
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
                    'If Uso_Filtri() Then
                    '    LabelErrore.Text() = "Non ci sono documenti che rispondono ai filtri impostati"
                    'Else
                        LabelErrore.Text() = "Non ci sono documenti nella tua lista"
                    'End If
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    
                    Dim objGriglia As New Griglia
                    tblDati = New Table
                    objGriglia.Tabella() = tblDati
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Numero", "Data", "Oggetto della delibera", "", "", "", "", "Apri", ""}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, True, True, True}
                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", "LINK", ""}
                    objGriglia.idControlloColonna = New String() {"chkSeduta", "", "", "", "", "", "", "", "Apri", ""}
                    objGriglia.VetAzioni = New String() {"", "", "", "", "", "", "", "", "AllegatiDocumentoAction.aspx", ""}
                    objGriglia.PaginaCorrente = IIf(Context.Session.Item("pagina") Is Nothing, 1, Context.Session.Item("pagina"))
                    objGriglia.Vettore = vr(1)
                    objGriglia.cssClasseRigaDispari = "grigliaRigaDispari"
                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblDati)



                End If
                Contenuto.Controls.Add(pnlSeduta)

                pnlSeduta.Visible = True

                context.Session.Add("soloConsultazione", True)
                Call Pulisci_Sessione()
            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        End If

    End Sub

    Private Sub Pulisci_Sessione()

    End Sub

End Class
