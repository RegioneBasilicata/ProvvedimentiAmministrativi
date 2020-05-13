Public Class GUPrelazioni
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblPrelazioni As System.Web.UI.WebControls.Table
    Protected WithEvents rigaPrelazioni As System.Web.UI.WebControls.TableRow
    Protected WithEvents cellaDetPrelazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents cellaBtnPrelazioni As System.Web.UI.WebControls.TableCell
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents btnPrelaziona As System.Web.UI.WebControls.Button
    Protected WithEvents pnlNote As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Prelazione")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            If Not IsPostBack Then
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                vr = context.Items.Item("vettoreDati")

                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If (vr(0) <> 0 And vr(0) <> 1) Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1))
                    Contenuto.Controls.Add(LabelErrore)
                    btnPrelaziona.Visible = False
                ElseIf (vr(0) = 1) Then
                    LabelErrore.Visible = True
                    If vr(0) = 1 Then
                        LabelErrore.Text = LabelErrore.Text & "Non ci sono documenti in lavorazione nel tuo ufficio" + "<br/>"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                    btnPrelaziona.Visible = False
                    pnlNote.Visible = False
                Else
                    Dim tipoApplic As Integer = CInt(Context.Items.Item("tipoApplic"))
                    context.Session.Add("tipoApplic", tipoApplic)
                    Select Case tipoApplic
                        Case 0
                            Rinomina_Pagina(Me, "Prelazione Determine")
                        Case 1
                            Rinomina_Pagina(Me, "Prelazione Delibere")
                        Case 2
                            Rinomina_Pagina(Me, "Prelazione Disposizioni")
                    End Select
                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    Dim codDocumento As String = ""
                    Dim oOperatore As DllAmbiente.Operatore = Session("oOperatore")
                    'tipoApllic deve essere trasformata in stringa es o = Determina
                    Dim responsabile As DllAmbiente.Operatore = New DllAmbiente.Operatore

                    Dim strDocumenti As String = ""
                    Select Case tipoApplic
                        Case 0

                            strDocumenti = "DETERMINE"
                        Case 1
                            strDocumenti = "DELIBERE"
                        Case 2
                            strDocumenti = "DISPOSIZIONI"

                    End Select

                    'If oOperatore.oUfficio.ResponsabileUfficio(strDocumenti) <> oOperatore.Codice Then
                    '    responsabile.Codice = oOperatore.oUfficio.ResponsabileUfficio(strDocumenti)
                    '    Dim stringCognomeNome As String = ""
                    '    stringCognomeNome = responsabile.Cognome & " " & responsabile.Nome
                    '    Dim obj(UBound(vr(1), 1), UBound(vr(1), 2)) As Object
                    '    Dim counter As Integer = 0
                    '    For i As Integer = 0 To UBound(vr(1), 2)

                    '        If stringCognomeNome <> vr(1)(4, i) Then
                    '            obj(0, counter) = vr(1)(0, i)
                    '            obj(1, counter) = vr(1)(1, i)
                    '            obj(2, counter) = vr(1)(2, i)
                    '            obj(3, counter) = vr(1)(3, i)
                    '            obj(4, counter) = vr(1)(4, i)
                    '            obj(5, counter) = vr(1)(5, i)
                    '            obj(6, counter) = vr(1)(6, i)
                    '            counter += 1
                    '        End If
                    '    Next
                    '    ReDim Preserve obj(UBound(vr(1), 1), counter - 1)
                    '    vr(1) = obj
                    'End If
                    
                    Dim objGriglia As New Griglia
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = False
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Numero", "Data", "Oggetto", "Utente", "", "","UDD", "UCA", "UR"}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, True, False, False, False, False, False}
                    objGriglia.ControlloColonna = New String() {"CHECK", "", "DATA", "", "", "", "", "", "", ""}
                    objGriglia.idControlloColonna = New String() {"ckcDocumenti", "", "", "", "", "", "", "", "", ""}
                    objGriglia.Vettore = vr(1)
                    objGriglia.Ordina = True
                    objGriglia.PaginaCorrente = IIf(context.Session.Item("pagina") Is Nothing, 1, context.Session.Item("pagina"))
                    objGriglia.IndiceOrdinamento = IIf(context.Session.Item("indice") Is Nothing, 1, context.Session.Item("indice"))

                    objGriglia.crea_tabella_daVettore()

                    tblPrelazioni = New Table
                    rigaPrelazioni = New TableRow
                    cellaDetPrelazioni = New TableCell
                    cellaBtnPrelazioni = New TableCell

                    cellaDetPrelazioni.Controls.Add(tblDati)
                    rigaPrelazioni.Cells.Add(cellaDetPrelazioni)
                    rigaPrelazioni.VerticalAlign = VerticalAlign.Top
                    tblPrelazioni.Rows.Add(rigaPrelazioni)

                    rigaPrelazioni = New TableRow
                    cellaBtnPrelazioni.Controls.Add(btnPrelaziona)
                    cellaBtnPrelazioni.HorizontalAlign = HorizontalAlign.Center
                    rigaPrelazioni.Cells.Add(cellaBtnPrelazioni)
                    tblPrelazioni.Rows.Add(rigaPrelazioni)
                    Contenuto.Controls.Add(tblPrelazioni)

                    Contenuto.Controls.Add(pnlNote)
                End If
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub
    Private Sub btnPrelaziona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrelaziona.Click
        Session.Add("elencoDocumentiDaPrelazionare", Request.Item("ckcDocumenti"))
        Session.Add("note", Request.Item("txtNote"))
        Response.Redirect("PrelazionaDocumentoAction.aspx?tipo=" & TipoApplic(Context))
    End Sub
End Class
