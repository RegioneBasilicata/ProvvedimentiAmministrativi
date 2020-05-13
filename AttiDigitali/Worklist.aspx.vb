Imports eWorld.UI
Imports Microsoft.Office.Interop
Imports DllDocumentale

Public Class Worklist
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
    Protected WithEvents tblElencoDetermine As System.Web.UI.WebControls.Table
    Protected WithEvents btnRigettaInBlocco As System.Web.UI.WebControls.Button
    Protected WithEvents pnlRigettaInBlocco As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlInoltroInBlocco As System.Web.UI.WebControls.Panel
    Protected WithEvents PannelloRicerca As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnInoltroInBlocco As System.Web.UI.WebControls.Button
    Protected WithEvents consensoPin As System.Web.UI.HtmlControls.HtmlInputHidden
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.

    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Lista Lavoro")
    End Sub

#End Region

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Worklist))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
            btnRigettaInBlocco.Attributes.Add("onclick", "return VerificaSelezioneEConsenso('chkDocumenti')")
        Else
            btnRigettaInBlocco.Attributes.Add("onclick", "return VerificaSelezione('chkDocumenti')")
        End If

        btnInoltroInBlocco.Attributes.Add("onclick", "return VerificaSelezione('chkDocumenti')")

        If Not Page.IsPostBack Then
            Try
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vettoreDocumenti As Object = Nothing
                vettoreDocumenti = Context.Items.Item("vettoreDati")
                If vettoreDocumenti Is Nothing Then
                    Exit Sub
                End If
                vettoreDocumenti = CType(vettoreDocumenti, Array)
                If vettoreDocumenti(0) <> 0 And vettoreDocumenti(0) <> 1 Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vettoreDocumenti(1))
                    Contenuto.Controls.Add(LabelErrore)
                ElseIf vettoreDocumenti(0) = 1 Then
                    LabelErrore.Visible = True
                    If Uso_Filtri() Then
                        LabelErrore.Text() = "Non ci sono documenti che rispondono ai filtri impostati"
                    Else
                        LabelErrore.Text() = "Non ci sono documenti nella tua lista"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                    Dim objGriglia As New Griglia
                    Dim tipo As Integer = Context.Items.Item("tipoApplic")
                    Context.Session.Add("tipoApplic", tipo)
                    Select Case tipo
                        Case 0
                            Rinomina_Pagina(Me, "Lista Lavoro Determine")
                            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True, False, True, True, True, True, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "LINK", "DATA","DATA", "", "", "IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", ""}
                            Else
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "LINK", "DATA", "", "", "IMAGE", "", "", ""}
                            End If

                        Case 1
                            Rinomina_Pagina(Me, "Lista Lavoro Delibere")
                            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True, False, True, True, True, True, True}
                                objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "LINK", "DATA","DATA", "", "", "IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE"}
                            Else
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "LINK", "DATA", "", "", "IMAGE", "", "", ""}
                            End If
                        Case 2
                            Rinomina_Pagina(Me, "Lista Lavoro Disposizioni")
                            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True, False, True, True, False, True, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "LINK", "DATA","DATA", "", "", "IMAGE", "POPUP_IMAGE", "", "POPUP_IMAGE", ""}
                            Else
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "LINK", "DATA", "", "", "IMAGE", "", "", ""}
                            End If
                        Case 3
                            Rinomina_Pagina(Me, "Lista Lavoro Decreti")
                            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, True, True, True, True, True}
                                objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "LINK", "DATA","DATA", "", "", "IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE"}
                            Else
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "LINK", "DATA", "", "", "IMAGE", "", "", ""}
                            End If
                        Case 4
                            Rinomina_Pagina(Me, "Lista Lavoro Ordinanze")
                            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True, False, True, True, True, True, True}
                                objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "LINK", "DATA","DATA", "", "", "IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE", "POPUP_IMAGE"}
                            Else
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False}
                                objGriglia.ControlloColonna = New String() {"CHECK", "LINK", "DATA", "", "", "IMAGE", "", "", ""}
                            End If
                    End Select
                    tblDati = New Table
                    tblDati.ID = "tblDati"
                    tblDati.EnableViewState = True
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                        If tipo = 0 Or tipo = 2 Then
                            If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                                objGriglia.VetDatiIntestazione() = New String() {"", "", "Numero", "Data", "Data", "Oggetto del documento", "", "Firma", "UDG", "UCA", "UR", ""}
                            Else
                                objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "", "Numero", "Data", "Data", "Oggetto del documento", "", "Firma", "UDG", "UCA", "UR", ""}
                            End If
                            objGriglia.idControlloColonna = New String() {"chkDocumenti", "", "", "", "", "", "", "", "", "", "", ""}
                            objGriglia.VetAzioni = New String() {"", "", "AggiungiAllAlberoAction.aspx", "", "", "", "", "", "MaggioriInfo.aspx", "MaggioriInfo.aspx", "MaggioriInfo.aspx", ""}
                        ElseIf tipo = 1 Then
                            ' nel caso delle delibere le colonne sono differenti
                            If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                                objGriglia.VetDatiIntestazione() = New String() {"", "", "Numero", "Data", "Data", "Oggetto del documento", "", "Firma", "USL", "UR", "USP", "UPRES"}
                            Else
                                objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "", "Numero", "Data", "Data", "Oggetto del documento", "", "Firma", "USL", "UR", "USP", "UPRES"}
                            End If
                            objGriglia.idControlloColonna = New String() {"chkDocumenti", "", "", "", "", "", "", "", "", "", "", ""}
                            objGriglia.VetAzioni = New String() {"", "", "AggiungiAllAlberoAction.aspx", "", "", "", "", "", "MaggioriInfo.aspx", "MaggioriInfo.aspx", "MaggioriInfo.aspx", "MaggioriInfo.aspx"}
                        Else
                            If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                                objGriglia.VetDatiIntestazione() = New String() {"", "", "Numero", "Data", "Data",  "Oggetto del documento", "", "Firma", "USL", "UR", "USP", "UPRES"}
                            Else
                                objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "", "Numero", "Data", "Data", "Oggetto del documento", "", "Firma", "USL", "UR", "USP", "UPRES"}
                            End If
                            objGriglia.idControlloColonna = New String() {"chkDocumenti", "", "", "", "", "", "", "", "", "",  "", ""}
                            objGriglia.VetAzioni = New String() {"", "", "AggiungiAllAlberoAction.aspx", "", "", "", "", "", "MaggioriInfo.aspx", "MaggioriInfo.aspx", "MaggioriInfo.aspx", "MaggioriInfo.aspx"}
                        End If
                    Else
                        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                            objGriglia.VetDatiIntestazione() = New String() {"", "", "Numero", "Data", "Oggetto del documento", "", "Firma", "UDG", "UCA", "UR"}
                        Else
                            objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "", "Numero", "Data", "Oggetto del documento", "", "Firma", "UDG", "UCA", "UR"}
                        End If
                        objGriglia.idControlloColonna = New String() {"chkDocumenti", "", "", "", "", "", "", "", "", ""}
                        objGriglia.VetAzioni = New String() {"", "", "AggiungiAllAlberoAction.aspx", "", "", "", "", "", "", ""}
                    End If
                    objGriglia.PaginaCorrente = IIf(Context.Session.Item("pagina") Is Nothing, 1, Context.Session.Item("pagina"))
                    objGriglia.Vettore = vettoreDocumenti(1)
                    objGriglia.cssClasseRigaDispari = "grigliaRigaDispari"
                    ' objGriglia.Ordina = True
                    If Not Context.Session.Item("indice") Is Nothing Then
                        objGriglia.IndiceOrdinamento = Context.Session.Item("indice")
                    End If

                    objGriglia.crea_tabella_daVettore()


                    If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                        'lu 07/09 aggiungo bottone gestione selezione note
                        Dim btnSelezioneNoteNegative As New Button
                        btnSelezioneNoteNegative.ID = "btnSelezioneNoteNegative"
                        btnSelezioneNoteNegative.CssClass = "btn"
                        btnSelezioneNoteNegative.Attributes.Add("onclick", "javascript:SelezionaConNoteNegative()")
                        btnSelezioneNoteNegative.Text = "Selezione per RIGETTO"

                        Contenuto.Controls.Add(btnSelezioneNoteNegative)
                        Dim btnSelezioneNotePositive As New Button
                        btnSelezioneNotePositive.ID = "btnSelezioneNotePositive "
                        btnSelezioneNotePositive.Text = "Selezione per INOLTRO"
                        btnSelezioneNotePositive.CssClass = "btn"
                        btnSelezioneNotePositive.Attributes.Add("onclick", "javascript:SelezionaConNotePositive()")

                        Contenuto.Controls.Add(btnSelezioneNotePositive)
                    End If
                    'fine lu 07/09 aggiungo bottone gestione selezione note

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



                    'verifico se l'utente ha il ruolo per inoltrare
                    Dim addPannelloInoltroInBlocco As Boolean = False
                    Dim addPannelloRigettoInBlocco As Boolean = False
                    'modgg 10-06 1

                    consensoPin.Value = IIf(oOperatore.Attributo("CACHEPIN") = "" Or oOperatore.Attributo("CACHEPIN") = "False", False, True)
                    Select Case tipo
                        Case 0
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DT002")
                            addPannelloRigettoInBlocco = oOperatore.Test_Ruolo("DT008")
                        Case 1
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DL002")
                            addPannelloRigettoInBlocco = oOperatore.Test_Ruolo("DL008")
                        Case 2
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DS015")
                            addPannelloRigettoInBlocco = oOperatore.Test_Ruolo("DS017")
                        Case 3
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DR002")
                            addPannelloRigettoInBlocco = oOperatore.Test_Ruolo("DR008")
                        Case 4
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DO002")
                            addPannelloRigettoInBlocco = oOperatore.Test_Ruolo("DO008")
                    End Select


                    If addPannelloInoltroInBlocco Then
                        pnlInoltroInBlocco.Visible = True
                        Contenuto.Controls.Add(pnlInoltroInBlocco)
                    End If

                    If addPannelloRigettoInBlocco Then
                        pnlRigettaInBlocco.Visible = True
                        btnRigettaInBlocco.Visible = True
                        Contenuto.Controls.Add(pnlRigettaInBlocco)
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

                Session.Add("tblDati", tblDati)
                Call Pulisci_Sessione()
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

    Private Sub Pulisci_Sessione()

        Context.Session.Remove("key")
        Context.Session.Remove("pagina")

    End Sub

    Private Function Uso_Filtri() As Boolean
        Return IsDate(Context.Session.Item("txtDataInizio")) Or _
        IsDate(Context.Session.Item("txtDataFine")) Or _
        Context.Session.Item("txtOggettoRicerca") <> "" Or _
        Context.Session.Item("txtCodUfficio") <> ""
    End Function

    Private Sub btnAvviaRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Session.Add("txtDataInizio", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataInizio"), CalendarPopup).SelectedDate)
        Session.Add("txtDataFine", DirectCast((Me.FindControl("ricerca")).FindControl("txtDataFine"), CalendarPopup).SelectedDate)
        Session.Add("txtOggettoRicerca", DirectCast((Me.FindControl("ricerca")).FindControl("txtOggettoRicerca"), TextBox).Text)
        Session.Add("txtCodUfficio", DirectCast((Me.FindControl("ricerca")).FindControl("txtCodUfficio"), DropDownList).SelectedItem.Value)
        Response.Redirect("WorklistAction.aspx?tipo=" & TipoApplic(Context))

    End Sub

    Private Sub btnInoltroInBlocco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInoltroInBlocco.Click
        If Request.Item("chkDocumenti") Is Nothing Then
            Session.Add("error", "Impossibile proseguire se non viene selezionato almeno un documento. Riprovare")
            Response.Redirect("MessaggioErrore.aspx")
        End If
        Dim totUrgenti As String = 0
        Dim documenti() As Object = Request.Item("chkDocumenti").Split(",")
        For i As Integer = 0 To documenti.Length - 1
            Dim listaDocAttributo As Generic.List(Of Documento_attributo) = Elenco_Attributi_Urgente(documenti(i))
            If Not listaDocAttributo Is Nothing Then
                For Each item As Documento_attributo In listaDocAttributo
                    If item.Valore = "True" Then
                        totUrgenti = totUrgenti + 1
                    End If
                Next
            End If

        Next
        If totUrgenti > 0 AndAlso totUrgenti <> documenti.Length Then
            Session.Add("prioritaMixed", "True")
        ElseIf totUrgenti = documenti.Length Then
            Session.Add("prioritaUrgenti", "True")
        Else
            Session.Add("prioritaMixed", "False")
            Session.Add("prioritaUrgenti", "False")
        End If
        Session.Add("elencoDocumentiDaInoltrare", Request.Item("chkDocumenti"))
        Session.Remove("key")
        Session.Add("codAzione", "INOLTRO")
        PreparaDocumentiDaFirmare(HttpContext.Current, Session.Item("codAzione"))
        Response.Redirect("InoltroDocumentoAction.aspx?tipo=" & TipoApplic(Context))
    End Sub

    Private Sub btnRigettaInBlocco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRigettaInBlocco.Click


        If Request.Item("chkDocumenti") Is Nothing Then
            Session.Add("error", "Impossibile proseguire se non viene selezionato almeno un documento. Riprovare")
            Response.Redirect("MessaggioErrore.aspx")
        End If
        Session.Add("elencoDocumentiDaInoltrare", Request.Item("chkDocumenti"))
        Session.Remove("key")
        Session.Add("codAzione", "RIGETTO")
        PreparaDocumentiDaFirmare(HttpContext.Current, Session.Item("codAzione"))
        Response.Redirect("RigettoDocumentoAction.aspx?tipo=" & TipoApplic(Context))

    End Sub
End Class
