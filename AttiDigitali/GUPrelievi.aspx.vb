Public Class GUPrelievi1
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents tblPrelievi As System.Web.UI.WebControls.Table
    Protected WithEvents rigaPrelievi As System.Web.UI.WebControls.TableRow
    Protected WithEvents cellaPrelievi As System.Web.UI.WebControls.TableCell
    Protected WithEvents cellaBtnPrelievi As System.Web.UI.WebControls.TableCell
    Protected WithEvents btnPreleva As System.Web.UI.WebControls.Button
    Protected WithEvents PannelloRicerca As System.Web.UI.WebControls.PlaceHolder

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me,"Documenti in arrivo")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not IsPostBack Then
            Try
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                vr = Context.Items.Item("vettoreDati")

                If vr(0) <> 0 And vr(0) <> 1 Then
                    LabelErrore.Text = vr(1)
                    LabelErrore.Visible = True
                    Contenuto.Controls.Add(LabelErrore)
                    btnPreleva.Visible = False
                ElseIf vr(0) = 1 Then
                    LabelErrore.Text = "Non ci sono documenti in arrivo in ufficio "
                    LabelErrore.Visible = True
                    Contenuto.Controls.Add(LabelErrore)
                    btnPreleva.Visible = False
                Else
                    Dim tipoApplic As String = Context.Items.Item("tipoApplic")
                    Context.Session.Add("tipoApplic", tipoApplic)

                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    Dim objGriglia As New Griglia
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = False
                    objGriglia.Trasposta() = True


                    If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                        If tipoApplic = 1 Then
                            objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True, False, False, False, True, False, False}
                            objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data","Data", "Oggetto", "", "", "USL", "UR", "USS", "UPRE"}
                        Else
                            objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True,False, True,False, False, False, False, True, False}
                            objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data","Data", "Oggetto", "", "", "UDG", "UCA", "UR", ""}
                        End If
                      
                    Else
                        objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "", "Numero", "Data", "Oggetto", "", "", "", "", ""}
                        objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, True, True, False, False, False, False, False}

                    End If
                    objGriglia.ControlloColonna = New String() {"CHECK", "IMAGE", "", "DATA", "DATA","", "", "", "IMAGE", "IMAGE", "IMAGE", ""}
                    objGriglia.idControlloColonna = New String() {"ckcDocumento", "", "", "","", "", "", "", "", "", "", ""}
                    objGriglia.Vettore = vr(1)
                    objGriglia.Ordina = True
                    objGriglia.PaginaCorrente = IIf(Context.Session.Item("pagina") Is Nothing, 1, Context.Session.Item("pagina"))
                    objGriglia.IndiceOrdinamento = IIf(Context.Session.Item("indice") Is Nothing, 1, Context.Session.Item("indice"))

                    objGriglia.crea_tabella_daVettore()

                    tblPrelievi = New Table
                    rigaPrelievi = New TableRow
                    cellaPrelievi = New TableCell
                    cellaBtnPrelievi = New TableCell

                    cellaPrelievi.Controls.Add(tblDati)
                    rigaPrelievi.Cells.Add(cellaPrelievi)
                    tblPrelievi.Rows.Add(rigaPrelievi)

                    rigaPrelievi = New TableRow
                    cellaBtnPrelievi.Controls.Add(btnPreleva)
                    cellaBtnPrelievi.HorizontalAlign = HorizontalAlign.Center
                    rigaPrelievi.Cells.Add(cellaBtnPrelievi)
                    tblPrelievi.Rows.Add(rigaPrelievi)

                    Contenuto.Controls.Add(tblPrelievi)
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

    Private Sub btnPreleva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreleva.Click
        Session.Add("elencoDocumentiDaPrelevare", Request.Item("ckcDocumento"))
        Dim tipoApp As String = TipoApplic(HttpContext.Current)

        Response.Redirect("PrelevaDocumentoAction.aspx?tipo=" & tipoApp)

    End Sub
End Class
