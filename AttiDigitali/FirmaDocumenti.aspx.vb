Public Class FirmaDocumenti
    Inherits WebSession
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lnkAnteprima As System.Web.UI.WebControls.HyperLink
    Protected WithEvents btnFirma As System.Web.UI.WebControls.Button
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents pnlFirma As System.Web.UI.WebControls.Panel
    Protected WithEvents TxtTitolareNome As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TxtTitolareCognome As System.Web.UI.HtmlControls.HtmlInputHidden


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Firma Determina")

        Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

        Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
        Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
        Dim messaggi_di_Warning As New ArrayList

        For Each docDaInoltrare As String In vDocumentiDaInoltrare

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(docDaInoltrare)
            If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
                messaggi_di_Warning.Add(Warning(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "UR" Then
                messaggi_di_Warning.Add(Warning_Ragioneria(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "USS" Then
                messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(docDaInoltrare))
            End If
        Next
        Context.Session.Add("warning", messaggi_di_Warning)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Try

        


            If Not IsPostBack Then
                LabelErrore = New Label
                lblInfo = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                Dim vrOper As Object = Nothing
                vr = Context.Items.Item("vettoreDati")
                If Not Context.Session.Item("warning") Is Nothing Then
                    LabelErrore.Text = ""
                    For Each messaggio As String In DirectCast(Context.Session.Item("warning"), ArrayList)
                        LabelErrore.Text = LabelErrore.Text & messaggio
                    Next
                    LabelErrore.Visible = True
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    Context.Session.Remove("warning")
                End If

                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                TxtTitolareCognome.Value = oOperatore.Cognome
                TxtTitolareNome.Value = oOperatore.Nome
                'TxtTitolareCognome.Value = "Gentilesca"
                'TxtTitolareNome.Value = "Giovanna"

                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If (vr(0) <> 0 And vr(0) <> 1) Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1) & " - " & vrOper(1))
                    Contenuto.Controls.Add(LabelErrore)
                    pnlFirma.Visible = False
                ElseIf (vr(0) = 1) Then
                    LabelErrore.Visible = True
                    If vr(0) = 1 Then
                        LabelErrore.Text = LabelErrore.Text & "Problemi nel caricare il file da firmare" + "<br/>"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                    pnlFirma.Visible = False
                    lblInfo.Visible = False
                Else
                    Select Case CInt(vr(2))
                        Case 0
                            Rinomina_Pagina(Me, "Firma Determine")
                        Case 1
                            Rinomina_Pagina(Me, "Firma Delibere")
                        Case 2
                            Rinomina_Pagina(Me, "Firma Disposizioni")
                    End Select

                    Dim tabella As Web.UI.WebControls.Table
                    Dim riga As Web.UI.WebControls.TableRow
                    Dim cella As Web.UI.WebControls.TableCell



                    lblInfo.CssClass = "lbl"
                    lblInfo.Width = Web.UI.WebControls.Unit.Point(250)
                    lblInfo.Attributes.Add("wordwrap", True)
                    lblInfo.Attributes.Add("autosize", True)
                    btnFirma.CssClass = "btn"

                    tabella = New Web.UI.WebControls.Table
                    tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                    tabella.HorizontalAlign = HorizontalAlign.Center

                    riga = New Web.UI.WebControls.TableRow
                    cella = New Web.UI.WebControls.TableCell
                    cella.ColumnSpan = 2
                    lblInfo.Text = "L'utente è a conoscenza chiaramente e senza ambiguità dei dati contenuti nel presente documento " & _
                    " e dichiara espressamente di voler firmare."
                    cella.Controls.Add(lblInfo)
                    riga.Controls.Add(cella)
                    tabella.Rows.Add(riga)

                    riga = New Web.UI.WebControls.TableRow
                    cella = New Web.UI.WebControls.TableCell
                    lnkAnteprima.Target = "_blank"
                    lnkAnteprima.NavigateUrl = "AnteprimaAllegatoAction.aspx?key=" + CStr(vr(1)(0, 0)) & "&pdf=1&prew=1"
                    lnkAnteprima.Visible = True
                    cella.Controls.Add(lnkAnteprima)
                    riga.Controls.Add(cella)

                    cella = New Web.UI.WebControls.TableCell
                    btnFirma.Visible = True
                    btnFirma().Text = "Firma"
                    cella.Controls.Add(btnFirma)
                    riga.Controls.Add(cella)

                    tabella.Rows.Add(riga)

                    Contenuto.Controls.Add(tabella)
                    Session.Add("tipoApplic", vr(2))
                    Session.Add("nomeDocumentoFirma", vr(1)(2, 0))
                    Session.Add("estensioneDocumentoFirma", vr(1)(3, 0))
                End If
            End If
            If Not Context.Session.Item("warning") Is Nothing Then
                LabelErrore.Text = ""
                For Each messaggio As String In DirectCast(Context.Session.Item("warning"), ArrayList)
                    LabelErrore.Text = LabelErrore.Text & messaggio
                Next
                LabelErrore.Visible = True
                Contenuto.Controls.Add(LabelErrore)
            Else
                Context.Session.Remove("warning")
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub


    Private Sub btnFirma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirma.Click

        Dim hContenutoFileFirmato As String = Context.Request.Form.Get("hContenutoFileFirmato")

        Session.Add("hContenutoFileFirmato", hContenutoFileFirmato)
        If Trim(context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", context.Request.QueryString.Get("key"))
        End If

        Dim tipoApplic As String = Session.Item("tipoApplic")
        Session.Remove("tipoApplic")

        Select Case CInt(tipoApplic)
            Case 0
                Response.Redirect("RegistraFirmaDeterminaAction.aspx")
            Case 1
                Response.Redirect("RegistraFirmaDeliberaAction.aspx")
            Case 2
                Response.Redirect("RegistraFirmaDisposizioneAction.aspx")
            Case 3
                Response.Redirect("RegistraFirmaDecretoAction.aspx")
            Case 4
                Response.Redirect("RegistraFirmaOrdinanzaAction.aspx")
        End Select

    End Sub
End Class
