Imports eWorld.UI
Public Class MonitorArchivioStampa
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
    Protected WithEvents pnlStampaBlocco As System.Web.UI.WebControls.Panel
    Protected WithEvents PannelloRicerca As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnStampaBlocco As System.Web.UI.WebControls.Button
    Protected WithEvents pnlResetStato As System.Web.UI.WebControls.Panel
    Protected WithEvents btnResetStato As System.Web.UI.WebControls.Button
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
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Dim searchCont As search = PannelloRicerca.FindControl("ricerca")
        'If Not searchCont Is Nothing Then
        '    searchCont.renderDllStatoStampa = False
        'End If
        'return VerificaSelezione('chkDocumenti')

        Response.Expires = -1
        Dim visualizzaCheck As Boolean = True
        If Not Session("StatoStampa") Is Nothing AndAlso Session("StatoStampa") = "1" Then
            visualizzaCheck = False
        End If
        btnStampaBlocco.Attributes.Add("onclick", "AsseggnaClasseNonSelezione('chkDocumenti');window.print(); return VerificaSelezione('chkDocumenti') ")

        If Not Page.IsPostBack Then
            Try
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                vr = Context.Items.Item("vettoreDati")
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
                    If Uso_Filtri() Then
                        LabelErrore.Text() = "Non ci sono documenti che rispondono ai filtri impostati"
                    Else
                        LabelErrore.Text() = "Non ci sono documenti nella tua lista"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    Dim objGriglia As New Griglia
                    Dim tipo As Integer = Context.Items.Item("tipoApplic")
                    Context.Session.Add("tipoApplic", tipo)
                    Select Case tipo
                        Case 0
                        
                            Select Case UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE"))
                                Case "REGIONE"
                                    Rinomina_Pagina(Me, "Elenco Determine Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}

                                Case "ALSIA"
                                    Rinomina_Pagina(Me, "Elenco Determine Approvate  Pubblicate il " & Format(Now, "dd-MM-yyyy"))
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, True, True, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "", "DATA", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Num. Cronologico", "Num. Atto", "Data", "Oggetto del documento", "Area Proponente"}

                                Case Else
                                    Rinomina_Pagina(Me, "Elenco Determine Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}
                            End Select

                        Case 1


                            Select Case UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE"))
                                Case "REGIONE"
                                    Rinomina_Pagina(Me, "Elenco Delibere Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}

                                Case "ALSIA"
                                    Rinomina_Pagina(Me, "Elenco Delibere Approvate Pubblicate il " & Format(Now, "dd-MM-yyyy"))
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, True, True, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "", "DATA", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Num. Cronologico", "Num. Atto", "Data", "Oggetto del documento", "Area Proponente"}

                                Case Else
                                    Rinomina_Pagina(Me, "Elenco Delibere Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}
                            End Select

                        Case 2
                            Select Case UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE"))
                                Case "REGIONE"
                                    Rinomina_Pagina(Me, "Elenco Disposizioni Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}

                                Case "ALSIA"

                                Case Else
                                    Rinomina_Pagina(Me, "Elenco Disposizioni Approvate")
                                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, False, False, False, False}
                                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", ""}
                                    objGriglia.VetDatiIntestazione() = New String() {"CHECK&chkDocumenti", "Numero", "Data", "Oggetto del documento"}
                            End Select

 
                    End Select
                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    objGriglia.idControlloColonna = New String() {"chkDocumenti", "", "", "", "", "", "", "", ""}
                    objGriglia.VetAzioni = New String() {"", "", "", "", "", "", "", "", ""}
                    objGriglia.PaginaCorrente = IIf(Context.Session.Item("pagina") Is Nothing, 1, Context.Session.Item("pagina"))
                    objGriglia.Vettore = vr(1)
                    objGriglia.cssClasseRigaDispari = "grigliaRigaDispari"
                    ' objGriglia.Ordina = True
                    If Not Context.Session.Item("indice") Is Nothing Then
                        objGriglia.IndiceOrdinamento = Context.Session.Item("indice")
                    End If

                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblDati)

                    'verifico se l'utente ha il ruolo per inoltrare
                    Dim addPannelloInoltroInBlocco As Boolean = False
                    'modgg 10-06 1
                    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                    Select Case tipo
                        Case 0
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DT002")
                        Case 1
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DL002")
                        Case 2
                            addPannelloInoltroInBlocco = oOperatore.Test_Ruolo("DS015")
                    End Select
                    If addPannelloInoltroInBlocco Then
                        pnlStampaBlocco.Visible = visualizzaCheck
                        pnlResetStato.Visible = Not visualizzaCheck
                        If visualizzaCheck Then
                            Contenuto.Controls.Add(pnlStampaBlocco)
                        Else
                            Contenuto.Controls.Add(pnlResetStato)
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

                If Not (Session("StatoStampa")) = Nothing Or Not (Session("StatoStampa") = "") Then
                    DirectCast((Me.FindControl("ricerca")).FindControl("dllStatoStampa"), DropDownList).SelectedValue() = Session("StatoStampa")
                End If

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
        Response.Redirect("MonitorArchivioStampaAction.aspx?tipo=" & TipoApplic(Context))

    End Sub

    Private Sub btnStampaBlocco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaBlocco.Click
        If Request.Item("chkDocumenti") Is Nothing Then
            Session.Add("error", "Impossibile proseguire se non viene selezionato almeno un documento. Riprovare")
            Response.Redirect("MessaggioErrore.aspx")
        End If
        Session.Add("elencoDocumentiDaInoltrare", Request.Item("chkDocumenti"))
        'Segnare i documenti come stampati
        Session.Add("flagStampato", 1)
        Response.Redirect("SetStatoStampatoAction.aspx?tipo=" & TipoApplic(Context))
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetStato.Click
        If Request.Item("chkDocumenti") Is Nothing Then
            Session.Add("error", "Impossibile proseguire se non viene selezionato almeno un documento. Riprovare")
            Response.Redirect("MessaggioErrore.aspx")
        End If
        Session.Add("elencoDocumentiDaInoltrare", Request.Item("chkDocumenti"))
        'Segnare i documenti come stampati
        Session.Add("flagStampato", 0)
        Response.Redirect("SetStatoStampatoAction.aspx?tipo=" & TipoApplic(Context))
    End Sub

  
End Class
