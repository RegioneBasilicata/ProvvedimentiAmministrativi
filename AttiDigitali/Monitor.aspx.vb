Imports eWorld.UI
Public Class Monitor
    Inherits WebSession


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents PannelloRicerca As System.Web.UI.WebControls.PlaceHolder
    'Protected WithEvents ricdoc As campiRicDoc

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Monitor Documenti")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not Page.IsPostBack Then
            Try
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                Dim tipoApplic As String
                tipoApplic = context.Items.Item("tipoApplic")
                context.Session.Add("tipoApplic", tipoApplic)
                vr = context.Items.Item("vettoreDati")
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If vr(0) <> 0 Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1))
                    Contenuto.Controls.Add(LabelErrore)
                    'pnlRicerca.Visible = False
                ElseIf vr(0) = 1 Then
                    LabelErrore.Visible = True
                    If Uso_Filtri() Then
                        LabelErrore.Text() = "Non ci sono documenti che rispondono ai filtri impostati"
                    Else
                        LabelErrore.Text() = "Non ci sono documenti nella tua lista"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    Select Case tipoApplic
                        Case 0
                            Rinomina_Pagina(Me, "Monitor Documenti Determine")
                        Case 1
                            Rinomina_Pagina(Me, "Monitor Documenti Delibere")
                        Case 2
                            Rinomina_Pagina(Me, "Monitor Documenti Disposizioni")
                    End Select
                    Dim objGriglia As New Griglia
                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"Id", "Numero", "Oggetto", "codOper", "Stato", "", "Storico", "", "", "UDD", "UCA", "UR"}
                    'objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, False, False, False, True,False,False}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, False, True, False, True, False, False, False, False, False}
                    objGriglia.ControlloColonna() = New String() {"", "", "", "", "SUBSTRING", "", "LINK", "", "", "", "", ""}
                    objGriglia.idControlloColonna() = New String() {"", "", "", "", "30", "", "Storico", "", "", "", "", ""}
                    objGriglia.PaginaCorrente = IIf(context.Session.Item("pagina") Is Nothing, 1, context.Session.Item("pagina"))
                    objGriglia.VetAzioni = New String() {"", "", "", "", "", "", "StoricoDocumentoAction.aspx"}
                    ' objGriglia.Ordina = True
                    objGriglia.IndiceOrdinamento = IIf(context.Session.Item("indice") Is Nothing, 1, context.Session.Item("indice"))
                    objGriglia.Vettore = vr(1)
                    objGriglia.crea_tabella_daVettore()

                    Contenuto.Controls.Add(tblDati)

                    Dim consentiexport As Boolean = False
                    consentiexport = IIf(System.Configuration.ConfigurationManager.AppSettings("CONSENTI_EXPORT_XLS") Is Nothing, False, IIf(System.Configuration.ConfigurationManager.AppSettings("CONSENTI_EXPORT_XLS") = 0, False, True))
                    If consentiexport Then
                        Dim btnExport As New Button
                        btnExport.ID = "btnExport"
                        btnExport.Text = "Esporta"
                        btnExport.CssClass = "btn"
                        btnExport.Visible = consentiexport
                        btnExport.Attributes.Add("onclick", "javascript:window.open('WebReport.aspx')")

                        If btnExport.Visible Then
                            Session.Add("tblDati", tblDati)
                            Contenuto.Controls.Add(btnExport)
                        Else
                            Session.Remove("tblDati")
                        End If
                    End If
                   

                   
                End If

                If Not (Session("txtDataInizio")) = Nothing Or Not (IsDate(Session("txtDataInizio"))) Then

                    DirectCast(Me.FindControl("ricerca").FindControl("CalendarPopup1"), CalendarPopup).SelectedDate = CDate(Session("txtDataInizio"))
                End If
                If Not (Session("txtDataFine")) = Nothing Or Not (IsDate(Session("txtDataFine"))) Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("CalendarPopup2"), CalendarPopup).SelectedDate = Session("txtDataFine")
                End If
                If Not (Session("txtOggettoRicerca")) = Nothing Or Not (Session("txtOggettoRicerca") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("txtOggettoRicerca"), TextBox).Text = Session("txtOggettoRicerca")
                End If
                If Not (Session("txtCodUfficio")) = Nothing Or Not (Session("txtCodUfficio") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("ddlUffici"), DropDownList).SelectedValue() = Session("txtCodUfficio")
                End If
                If Not (Session("TipoRigetto")) = Nothing Or Not (Session("TipoRigetto") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("dllTipoRigetto"), DropDownList).SelectedValue() = Session("TipoRigetto")
                End If

                If Not (Session("TipologiaRicercaBeneficiario") = Nothing) Or Not (Session("TipologiaRicercaBeneficiario") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("TipologiaRicercaBeneficiario"), DropDownList).SelectedValue() = Session("TipologiaRicercaBeneficiario")
                End If

                If Not (Session("FiltroRicercaBeneficiario") = Nothing) Or Not (Session("FiltroRicercaBeneficiario") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("BeneficiarioTxt"), TextBox).Text = Session("FiltroRicercaBeneficiario")
                End If

                Call Pulisci_Sessione()

            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        Else
            'Response.Redirect(Context.Request.UrlReferrer.AbsoluteUri)

            Try
                If CType(CType(Albero.Controls(1).Controls(1), System.Web.UI.Control), Microsoft.Web.UI.WebControls.TreeView).Nodes.Count > 1 Then
                    CType(CType(Albero.Controls(1).Controls(1), System.Web.UI.Control), Microsoft.Web.UI.WebControls.TreeView).Nodes.RemoveAt(1)
                End If

            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try

        End If

    End Sub

    Private Sub Pulisci_Sessione()
        context.Session.Remove("pagina")
        'context.Session.Add("txtDataInizio", Nothing)
        'context.Session.Add("txtDataFine", Nothing)
        'context.Session.Add("txtOggettoRicerca", "")
        'context.Session.Add("txtCodUfficio", "")
        'context.Session.Add("txtDescUfficio", "")
    End Sub

    Private Function Uso_Filtri() As Boolean
        Return context.Session.Item("txtDataInizio") <> "" Or _
        context.Session.Item("txtDataFine") <> "" Or _
        context.Session.Item("txtOggettoRicerca") <> "" Or _
        context.Session.Item("txtCodUfficio") <> "" Or _
        context.Session.Item("txtDescUfficio") <> ""
    End Function



    Private Sub btnAvviaRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session.Add("txtDataInizio", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataInizio"), CalendarPopup).SelectedDate)
        Session.Add("txtDataFine", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataFine"), CalendarPopup).SelectedDate)
        Session.Add("txtOggettoRicerca", DirectCast((Me.FindControl("ricerca")).FindControl("txtOggettoRicerca"), TextBox).Text)
        Session.Add("txtCodUfficio", DirectCast((Me.FindControl("ricerca")).FindControl("txtCodUfficio"), DropDownList).SelectedItem.Value)

        Response.Redirect("MonitorAction.aspx?tipo=" & TipoApplic(Context))
    End Sub

End Class
