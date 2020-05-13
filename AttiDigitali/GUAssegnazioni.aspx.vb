Public Class GUAssegnazioni
    Inherits WebSession


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblAssegnazioni As System.Web.UI.WebControls.Table
    Protected WithEvents rigaAssegnazioni As System.Web.UI.WebControls.TableRow
    Protected WithEvents cellaDetAssegnazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents cellaOperAssegnazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents cellaBtnAssegnazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents cellaMesAssegnazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents tblDatiOper As System.Web.UI.WebControls.Table
    Protected WithEvents btnAssegna As System.Web.UI.WebControls.Button
    Protected WithEvents pnlNote As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Assegnazione")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            If Not IsPostBack Then
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                Dim vrOper As Object = Nothing
                vr = context.Items.Item("vettoreDati")
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                Dim Operatori As New Hashtable
                If Not Context.Items.Item("vettoreDatiOperatori") Is Nothing Then
                    DirectCast(Context.Items.Item("vettoreDatiOperatori"), Hashtable).Remove(oOperatore.pCodice)

                    Operatori = DirectCast(Context.Items.Item("vettoreDatiOperatori"), Hashtable)
                End If



                If Operatori.Count > 20 Then
                    'se gli utenti sono molti, li ordino per cognome
                    vrOper = hashToArraySorted(Operatori)
                Else
                    vrOper = hashToArray(Operatori)
                End If
               
               
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If Not vrOper Is Nothing Then
                    vrOper = CType(vrOper, Array)
                End If

                If vrOper Is Nothing Or (Not vr Is Nothing AndAlso (vr(0) <> 0 And vr(0) <> 1)) Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1) & " - " & vrOper(1))
                    Contenuto.Controls.Add(LabelErrore)
                    btnAssegna.Visible = False
                ElseIf vrOper Is Nothing Or (Not vr Is Nothing AndAlso vr(0) = 1) Then
                    LabelErrore.Visible = True
                    If vr(0) = 1 Then
                        LabelErrore.Text = LabelErrore.Text & "Non ci sono documenti nella tua lista lavoro" + "<br/>"
                    End If
                    If vrOper Is Nothing Then
                        LabelErrore.Text = LabelErrore.Text & "Non ci sono collaboratori nel tuo ufficio" + "<br/>"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                    btnAssegna.Visible = False
                    pnlNote.Visible = False
                Else
                    Dim tipoApplic As String = Context.Items.Item("tipoApplic")
                    Context.Session.Add("tipoApplic", tipoApplic)
                    Select Case CInt(tipoApplic)
                        Case 0
                            Rinomina_Pagina(Me, "Assegnazione Determine")
                        Case 1
                            Rinomina_Pagina(Me, "Assegnazione Delibere")
                        Case 2
                            Rinomina_Pagina(Me, "Assegnazione Disposizione")
                        Case 3, 4
                            Rinomina_Pagina(Me, "Assegnazione Atti")
                    End Select

                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(500)
                    tblDatiOper = New Table
                    'modgg 10-06
                    tblDatiOper.Width = Web.UI.WebControls.Unit.Pixel(250)


                    'rigaAssegnazioni = New TableRow
                    'Dim lblMes As New WebControls.Label
                    'lblMes.Text = ""
                    'cellaMesAssegnazioni.Controls.Add(lblMes)
                    'cellaMesAssegnazioni.HorizontalAlign = HorizontalAlign.Center
                    'rigaAssegnazioni.Cells.Add(cellaMesAssegnazioni)
                    'tblAssegnazioni.Rows.Add(rigaAssegnazioni)

                    Dim objGriglia As New Griglia
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = False
                    objGriglia.Trasposta() = True
                    objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "", "DATA","DATA", "", "", "", "IMAGE", "IMAGE", "IMAGE", ""}

                    If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                        If tipoApplic = 1 Then
                            objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False,True, False, False, False, True, False, False}
                            objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data", "Data", "Oggetto", "", "", "USL", "UR", "USS", "UPRE"}
                        Else
                            objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, False, True, False, False, False, False, True, False}
                            objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data","Data", "Oggetto", "", "", "UDG", "UCA", "UR", ""}
                        End If
                        

                    Else
                        objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, True, False, False, False, False, False}
                        objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data", "Oggetto", "", "", "", "", ""}

                    End If
                    objGriglia.idControlloColonna = New String() {"ckcDocumenti", "", "", "", "", "", "", "", "", "", "", ""}
                    objGriglia.Vettore = vr(1)
                    objGriglia.Ordina = True
                    objGriglia.crea_tabella_daVettore()

                    objGriglia = New Griglia
                    objGriglia.Tabella() = tblDatiOper
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Collaboratore"}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True}
                    objGriglia.ControlloColonna = New String() {"OPTION", ""}
                    objGriglia.idControlloColonna = New String() {"optOperatore", ""}
                    objGriglia.Vettore = vrOper
                    objGriglia.FlagPaginazione = False
                    objGriglia.crea_tabella_daVettore()

                    tblAssegnazioni = New Table
                    rigaAssegnazioni = New TableRow
                    cellaDetAssegnazioni = New TableCell
                    cellaOperAssegnazioni = New TableCell

                    cellaDetAssegnazioni.Text = "Documenti"
                    cellaOperAssegnazioni.Text = "Assegnatari"
                    rigaAssegnazioni.Cells.Add(cellaDetAssegnazioni)
                    rigaAssegnazioni.Cells.Add(cellaOperAssegnazioni)
                    rigaAssegnazioni.VerticalAlign = VerticalAlign.Top
                    tblAssegnazioni.Rows.Add(rigaAssegnazioni)

                    rigaAssegnazioni = New TableRow
                    cellaDetAssegnazioni = New TableCell
                    cellaOperAssegnazioni = New TableCell


                    cellaDetAssegnazioni.Controls.Add(tblDati)
                    cellaOperAssegnazioni.Controls.Add(tblDatiOper)
                    rigaAssegnazioni.Cells.Add(cellaDetAssegnazioni)
                    rigaAssegnazioni.Cells.Add(cellaOperAssegnazioni)
                    rigaAssegnazioni.VerticalAlign = VerticalAlign.Top
                    tblAssegnazioni.Rows.Add(rigaAssegnazioni)

                    cellaBtnAssegnazioni = New TableCell
                    rigaAssegnazioni = New TableRow
                    cellaBtnAssegnazioni.Text = "Selezionare i documenti da assegnare all'utente"
                    cellaBtnAssegnazioni.HorizontalAlign = HorizontalAlign.Center
                    cellaBtnAssegnazioni.ColumnSpan = 2
                    rigaAssegnazioni.Cells.Add(cellaBtnAssegnazioni)
                    rigaAssegnazioni.VerticalAlign = VerticalAlign.Top
                    cellaBtnAssegnazioni.Controls.Add(btnAssegna)
                    rigaAssegnazioni.Cells.Add(cellaBtnAssegnazioni)
                    tblAssegnazioni.Rows.Add(rigaAssegnazioni)

                    Contenuto.Controls.Add(tblAssegnazioni)

                End If
                Contenuto.Controls.Add(pnlNote)
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnAssegna_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssegna.Click
        Session.Add("elencoDocumentiDaAssegnare", Request.Item("ckcDocumenti"))
        Session.Add("operatoreAssegnatario", Request.Item("optOperatore"))
        Session.Add("note", Request.Item("txtNote"))
        Response.Redirect("AssegnaDocumentiAction.aspx?tipo=" & TipoApplic(Context))
    End Sub
End Class
