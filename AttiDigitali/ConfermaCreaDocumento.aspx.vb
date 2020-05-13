Public Class ConfermaCreaDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblConferma As System.Web.UI.WebControls.Label
    Protected WithEvents btnContinua2 As System.Web.UI.WebControls.Button
    Protected WithEvents btnAnnulla2 As System.Web.UI.WebControls.Button
    Protected WithEvents chkContabile As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rqvCheck As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents rqvControllo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents controlloCheck As System.Web.UI.WebControls.TextBox

    Protected WithEvents chkPreimpegni As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkImpegno As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkImpegnoSuPerenti As System.Web.UI.WebControls.CheckBox

    Protected WithEvents chkLiquidazione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkAccertamento As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRiduzione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkNessuna As System.Web.UI.WebControls.CheckBox
    Protected WithEvents PannelloTipoContabile As System.Web.UI.WebControls.Panel

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Crea ")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            If Not IsPostBack Then
                controlloCheck.Attributes.CssStyle.Add("display", "none")
                Dim tipoApplic As String = Context.Items.Item("tipoApplic")
                Context.Session.Add("tipoApplic", tipoApplic)

                Dim tabella As Web.UI.WebControls.Table
                Dim riga As Web.UI.WebControls.TableRow
                Dim cella As Web.UI.WebControls.TableCell



                lblConferma = New Label
                lblConferma.CssClass = "lbl"
                lblConferma.Width = Web.UI.WebControls.Unit.Point(400)
                lblConferma.Attributes.Add("wordwrap", True)
                lblConferma.Attributes.Add("autosize", True)

                tabella = New Web.UI.WebControls.Table
                tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                tabella.HorizontalAlign = HorizontalAlign.Center

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                Select Case tipoApplic
                    Case 0
                        Rinomina_Pagina(Me, "Crea Determina")
                        lblConferma.Text = "Scegliendo Continua, si proseguirà con la creazione di una nuova determina. " _
                        & "<br/>Nel caso in cui, non si voglia creare la determina, scegliere Annulla"
                    Case 1
                        Rinomina_Pagina(Me, "Crea Delibera")
                        lblConferma.Text = "Scegliendo Continua, si proseguirà con la creazione di una nuova proposta di delibera. " _
                        & "<br/>Nel caso in cui, non si voglia creare la proposta di delibera, scegliere Annulla"
                    Case 2
                        Rinomina_Pagina(Me, "Crea Disposizione")
                        lblConferma.Text = "Scegliendo Continua, si proseguirà con la creazione di una nuova disposizione. " _
                        & "<br/>Nel caso in cui, non si voglia creare la disposizione, scegliere Annulla"
                End Select

                cella.Controls.Add(lblConferma)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                Dim lblOggetto As Label = New Label
                lblOggetto.CssClass = "lbl"
                lblOggetto.Width = Web.UI.WebControls.Unit.Point(250)
                lblOggetto.Attributes.Add("wordwrap", True)
                lblOggetto.Attributes.Add("autosize", True)
                lblOggetto.Text = "Oggetto:"

                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(lblOggetto)
                riga = New Web.UI.WebControls.TableRow
                riga.Cells.Add(cella)
                tabella.Rows.Add(riga)


                Dim rfv As New RequiredFieldValidator
                rfv.ErrorMessage = "Inserire L'Oggetto"
                rfv.ControlToValidate = "txtOggetto"
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(rfv)
                riga = New Web.UI.WebControls.TableRow
                riga.Cells.Add(cella)
                tabella.Rows.Add(riga)

                Dim txtOggetto As New Web.UI.WebControls.TextBox
                txtOggetto.TextMode = TextBoxMode.MultiLine
                txtOggetto.ID = "txtOggetto"
                txtOggetto.Rows = 5
                txtOggetto.Columns = 65
                txtOggetto.Text = ""
                txtOggetto.Attributes.Add("onblur", "javascript:verificaEsistenzaOggetto(this)")
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(txtOggetto)
                riga = New Web.UI.WebControls.TableRow
                riga.Cells.Add(cella)
                tabella.Rows.Add(riga)

                Dim lbl As New Label
                If tipoApplic = 0 Then 'Creo la checkbox solo per Determina
                    Dim lblPub As New Label
                    lblPub.Text = "Tipo Pubblicazione"

                    Dim radioPubbl As New RadioButtonList
                    radioPubbl.ID = "tipoPubblic"
                    'Tipo Pubblicazione
                    radioPubbl.Attributes.Add("border", "0")
                    radioPubbl.CssClass = "lbl"
                    radioPubbl.Items.Add(New ListItem("  Integrale", 0))
                    radioPubbl.Items.Add(New ListItem("  Per Estratto Ogg+Disp", 1))
                    radioPubbl.Items.Add(New ListItem("  Per Estratto Ogg", 2))
                    radioPubbl.Items(0).Attributes.Add("border", "0")
                    radioPubbl.TextAlign = TextAlign.Left
                    radioPubbl.BorderStyle = BorderStyle.None
                    radioPubbl.ControlStyle.BorderStyle = BorderStyle.None
                    radioPubbl.RepeatColumns = 4
                    radioPubbl.RepeatDirection = RepeatDirection.Horizontal
                    Dim rfvPub As New RequiredFieldValidator
                    rfvPub.ErrorMessage = "Selezionare Tipologia Pubblicazione"
                    rfvPub.ControlToValidate = "tipoPubblic"
                    cella = New Web.UI.WebControls.TableCell
                    cella.ColumnSpan = 3

                    cella.Controls.Add(lblPub)
                    cella.Controls.Add(radioPubbl)
                    cella.Controls.Add(rfvPub)

                    riga = New Web.UI.WebControls.TableRow
                    riga.Cells.Add(cella)
                    tabella.Rows.Add(riga)

                    PannelloTipoContabile.Visible = True


                    lbl.Text = "Quale operazione contabile comporta?"
                    cella = New Web.UI.WebControls.TableCell
                    cella.ColumnSpan = 3
                    cella.Controls.Add(lbl)
                    riga = New Web.UI.WebControls.TableRow
                    riga.Cells.Add(cella)
                    tabella.Rows.Add(riga)

                    chkNessuna.ID = "chkNessuna"
                    cella = New Web.UI.WebControls.TableCell
                    cella.ColumnSpan = 3

                Else
                    'in caso di disposizione deve essere visibile solo l'accertamento
                    lbl.Text = "Se la disposizione prevede anche un accertamento, <br/>selezionare la casella sottostante. "
                    cella = New Web.UI.WebControls.TableCell
                    cella.ColumnSpan = 3
                    cella.Controls.Add(lbl)
                    riga = New Web.UI.WebControls.TableRow
                    riga.Cells.Add(cella)
                    tabella.Rows.Add(riga)

                    PannelloTipoContabile.Visible = True
                    chkNessuna.Visible = False
                    chkPreimpegni.Visible = False
                    chkImpegno.Visible = False
                    chkImpegnoSuPerenti.Visible = False
                    chkLiquidazione.Visible = False
                    chkRiduzione.Visible = False
                    rqvControllo.Enabled = False

                End If

                cella.Controls.Add(PannelloTipoContabile)

                riga = New Web.UI.WebControls.TableRow
                riga.Cells.Add(cella)
                tabella.Rows.Add(riga)

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell

                btnContinua2.Visible = True


                cella.Controls.Add(btnContinua2)
                riga.Controls.Add(cella)

                cella = New Web.UI.WebControls.TableCell

                btnAnnulla2.Visible = True
                cella.Controls.Add(btnAnnulla2)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                Contenuto.Controls.Add(tabella)
            Else
                Dim tipoApplic As String = "0"
                'Context.Session.Remove("tipoApplic")

                Dim txtOggetto As String = CStr(Context.Request.Form("txtOggetto"))
                'comm per eliminazione xml
                'txtOggetto = txtOggetto.Replace(Chr(38), "&amp;")
                'txtOggetto = txtOggetto.Replace(Chr(39), "&aps;")
                'txtOggetto = txtOggetto.Replace(Chr(62), "&gt;")
                'txtOggetto = txtOggetto.Replace(Chr(60), "&lt;")
                'txtOggetto = txtOggetto.Replace(Chr(34), "&quot;")
                Dim svrdoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
                Dim esiste As Boolean = svrdoc.EsistenzaOggetto(tipoApplic, txtOggetto)
                If esiste Then
                    Me.ClientScript.RegisterStartupScript(Me.GetType(), "Script2", "<script language='javascript'>return confirm('L'oggetto digitato è già presente in archivio, continuare?')</script>")
                    btnContinua2.Attributes.Add("onclick", "Script2")
                End If
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub


    Private Sub btnContinua2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinua2.Click


        Dim tipoApp As String = TipoApplic(Context)
        Context.Session.Remove("tipoApplic")

        Dim txtOggetto As String = CStr(Context.Request.Form("txtOggetto"))
        Dim OpContabili As Integer = 0
        Dim tipoPubblic As Integer = 0
        'comm per eliminazione xml
        'txtOggetto = txtOggetto.Replace(Chr(38), "&amp;")
        'txtOggetto = txtOggetto.Replace(Chr(39), "&aps;")
        'txtOggetto = txtOggetto.Replace(Chr(62), "&gt;")
        'txtOggetto = txtOggetto.Replace(Chr(60), "&lt;")
        'txtOggetto = txtOggetto.Replace(Chr(34), "&quot;")

        Dim stringaOpConta As String = ""
        Select Case tipoApp
            Case 0, 1
                stringaOpConta = creaStringaOpContabili()
            Case 2
                stringaOpConta = creaStringaOpContabiliDisposizione()
        End Select

        Dim lstr_StringTempOpContabili As String = Trim(stringaOpConta.Replace("0", "").Replace(";", ""))
        'elminino tutti gli 0 e i punti e vigorla cosi facendo mi rimane il valore delle operazioni selezionate
        'non potrò + fare il controllo se contiene 1 visto che per sistemazioni contabili potrà avere altri valori
        'If stringaOpConta.Contains("1") Then
        If Not String.IsNullOrEmpty(lstr_StringTempOpContabili) Then
            OpContabili = 1
        Else
            OpContabili = 0
        End If


        Select Case tipoApp
            Case 0
                tipoPubblic = Context.Request.Form("tipoPubblic")
            Case 1
                OpContabili = 0
            Case 2
                OpContabili = 1
                tipoPubblic = 0
        End Select


        Context.Session.Add("txtOggetto", txtOggetto)
        Context.Session.Add("intOpContabili", OpContabili)
        Context.Session.Add("pubIntegrale", tipoPubblic)

        Context.Session.Add("tipoOpContabili", stringaOpConta)
        Response.Redirect("CreaDocumentoAction.aspx?tipo=" & tipoApp)


    End Sub

    Function creaStringaOpContabili() As String
        Dim result As String = ""
        If chkImpegno.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If chkImpegnoSuPerenti.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If



        If chkLiquidazione.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If chkAccertamento.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If chkRiduzione.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        'riduzione liq
        result = result & "0;"
        'riduzione pre imp
        result = result & "0;"
        'campo in +
        result = result & "0;0;"

        Return result
    End Function

    Function creaStringaOpContabiliDisposizione() As String
        Dim result As String = ""
        'chkImpegno.Checked 
        result = result & "0;"

        'chkImpegnoSuPerenti.Checked 
        result = result & "0;"

        If chkLiquidazione.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If
        
        If chkAccertamento.Checked Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        'chkRiduzione.Checked
        result = result & "0;"

        'riduzione liq
        result = result & "0;"
        'riduzione pre imp
        result = result & "0;"
        'campo in +
        result = result & "0;0;"

        Return result
    End Function

    Private Sub btnAnnulla2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla2.Click
        Response.Redirect("TornaHomeAction.aspx")
    End Sub
End Class
