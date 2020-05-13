Imports eWorld.UI
Public Class MonitorConsultazione
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

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Archivio Documenti")
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
                Else
                    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                    Select Case tipoApplic
                        Case 0
                            Rinomina_Pagina(Me, "Archivio Determine")
                        Case 1
                            Rinomina_Pagina(Me, "Archivio Delibere")
                        Case 2
                            Rinomina_Pagina(Me, "Archivio Disposizioni")
                    End Select
                    Dim objGriglia As New Griglia
                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True

                    If oOperatore.Test_Ruolo("PU002") Then
                        'se l'operatore è un print user, può stampare da archivio, altrimenti no
                        objGriglia.VetDatiIntestazione() = New String() {"", "Numero", "Data", "Oggetto", "In carico", "Fascicolo", "Stampa", "UDD", "UCA", "UR"}
                        objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, True, False, True, True, False, False, False}
                        objGriglia.ControlloColonna() = New String() {"", "", "DATA", "", "", "LINK", "LINK", "IMAGE", "IMAGE", "IMAGE"}
                        objGriglia.idControlloColonna() = New String() {"", "", "", "", "", "Fascicolo", "Stampa", "", "", ""}
                        objGriglia.VetAzioni = New String() {"", "", "", "", "", "AllegatiDocumentoAction.aspx", "StampaDocumentoAction.aspx", "", "", ""}
                    Else
                        'se non è un print user non visualizza in link stampa
                        objGriglia.VetDatiIntestazione() = New String() {"", "Numero", "Data", "Oggetto", "In carico", "Fascicolo", "", "UDD", "UCA", "UR"}
                        objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, True, False, True, False, False, False, False}
                        objGriglia.ControlloColonna() = New String() {"", "", "DATA", "", "", "LINK", "", "IMAGE", "IMAGE", "IMAGE"}
                        objGriglia.idControlloColonna() = New String() {"", "", "", "", "", "Fascicolo", "", "", "", ""}
                        objGriglia.VetAzioni = New String() {"", "", "", "", "", "AllegatiDocumentoAction.aspx", "", "", "", ""}
                    End If

                    objGriglia.PaginaCorrente = IIf(context.Session.Item("pagina") Is Nothing, 1, context.Session.Item("pagina"))
                    objGriglia.Vettore = vr(1)
                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblDati)
                    context.Session.Add("soloConsultazione", True)
                    context.Session.Remove("pagina")
                End If

            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        Else
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
    Private Sub btnAvviaRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
     

        Session.Add("txtDataInizio", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataInizio"), CalendarPopup).SelectedDate)
        Session.Add("txtDataFine", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataFine"), CalendarPopup).SelectedDate)
        Session.Add("txtOggettoRicerca", DirectCast((Me.FindControl("ricerca")).FindControl("txtOggettoRicerca"), TextBox).Text)
        Session.Add("txtCodUfficio", DirectCast((Me.FindControl("ricerca")).FindControl("txtCodUfficio"), DropDownList).SelectedItem.Value)

        Response.Redirect("MonitorConsultazioneAction.aspx?tipo=" & TipoApplic(Context))
    End Sub

End Class
