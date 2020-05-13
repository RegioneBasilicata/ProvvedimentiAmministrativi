Public Class StoricoDocumento
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

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.

        InitializeComponent()
        Inizializza_Pagina(Me, "Storico Documento")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not IsPostBack Then
                'Inserire qui il codice utente necessario per inizializzare la pagina
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                vr = context.Items.Item("vettoreDati")
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If vr(0) <> 0 Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1))
                    Contenuto.Controls.Add(LabelErrore)
                Else

                    'modgg 10-06
                    Dim vrDoc As Object
                    vrDoc = context.Items.Item("vettoreDocumento")
                    If Not vrDoc Is Nothing Then
                        vrDoc = CType(vrDoc, Array)
                    End If
                   

                    Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(Context.Request.QueryString.Get("key"))

                    'Aggiunto per visibilità sulla notifica
                    Dim pannelloBtn As New Panel
                    pannelloBtn.HorizontalAlign = HorizontalAlign.Right
                    pannelloBtn.Style.Add("padding-top", "5px")

                    Dim linkDettaglio As New HyperLink
                    linkDettaglio.CssClass = "lbl"
                    'linkDettaglio.Text = "Visualizza Dettaglio Atto"
                    linkDettaglio.ImageUrl = "risorse/immagini/btnVisualizza/btnDettaglio.png"
                    linkDettaglio.NavigateUrl = "CreaProvvedimento.aspx?tipo=" & objDocumento.Doc_Tipo & "&key=" & objDocumento.Doc_id

                    'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                    'Contenuto.Controls.Add(linkDettaglio)
                    'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                    pannelloBtn.Controls.Add(linkDettaglio)

                    'FIne Notifica

                    If objDocumento.haveOpContabile Then
                        Dim link As New HyperLink
                        link.CssClass = "lbl"
                        'link.Text = "Riepilogo dati contabili"
                        link.ImageUrl = "risorse/immagini/btnVisualizza/btnDatiContabili.png"
                        If objDocumento.Doc_Tipo = 0 Then
                            'DETERMINA
                            link.NavigateUrl = "ContabilePerRag.aspx?" & Request.QueryString.ToString
                        ElseIf objDocumento.Doc_Tipo = 2 Then
                            link.NavigateUrl = "ContabileDispPerRag.aspx?" & Request.QueryString.ToString
                        End If
                        'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                        'Contenuto.Controls.Add(link)
                        'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                        pannelloBtn.Controls.Add(link)
                    End If


                    Dim linkDettaglioStorico As New HyperLink
                    linkDettaglioStorico.CssClass = "lbl"
                    'linkDettaglioStorico.Text = "Dettaglio Storico"
                    linkDettaglioStorico.ImageUrl = "risorse/immagini/btnVisualizza/btnStorico.png"
                    linkDettaglioStorico.NavigateUrl = "javascript:openPopup(" & Request.QueryString.ToString & ")"

                    'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                    'Contenuto.Controls.Add(linkDettaglioStorico)
                    'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                    pannelloBtn.Controls.Add(linkDettaglioStorico)
                    Contenuto.Controls.Add(pannelloBtn)


                    If vrDoc(0) = 0 Then
                        Rinomina_Pagina(Me, "Storico Documento  " & vrDoc(1)(3))
                        Dim tblInfo As New Web.UI.WebControls.Table
                        Dim rigaInfo As New Web.UI.WebControls.TableRow
                        Dim cellaInfo As New Web.UI.WebControls.TableCell
                        Dim cellaInfo2 As New Web.UI.WebControls.TableCell
                        Dim cellaInfo3 As New Web.UI.WebControls.TableCell
                        Dim cellaInfo4 As New Web.UI.WebControls.TableCell
                        Dim rigaInfo2 As New Web.UI.WebControls.TableRow

                        'Label Oggetto
                        Dim lblOggetto As New Label
                        lblOggetto.Text = "Oggetto: "
                        lblOggetto.CssClass = "lbl"
                        cellaInfo.Controls.Add(lblOggetto)
                        rigaInfo.Controls.Add(cellaInfo)

                        'txtOggetto 
                        Dim txtOggetto As New TextBox
                        txtOggetto.TextMode = TextBoxMode.MultiLine
                        txtOggetto.Text = vrDoc(1)(0)
                        txtOggetto.Rows = 5
                        txtOggetto.Columns = 50
                        txtOggetto.ReadOnly = True
                        cellaInfo2.Controls.Add(txtOggetto)
                        rigaInfo.Controls.Add(cellaInfo2)
                        tblInfo.Rows.Add(rigaInfo)

                        'Label Ufficio Proponente
                        Dim lblUp As New Label
                        lblUp.Text = "Ufficio Proponente: "
                        lblUp.CssClass = "lbl"
                        cellaInfo3.Controls.Add(lblUp)
                        rigaInfo2.Controls.Add(cellaInfo3)

                        Dim txtUpDesc As New TextBox
                        txtUpDesc.Text = vrDoc(3)
                        txtUpDesc.TextMode = TextBoxMode.MultiLine
                        txtUpDesc.Rows = 2
                        txtUpDesc.Columns = 50
                        txtUpDesc.ReadOnly = True
                        cellaInfo4.Controls.Add(txtUpDesc)
                        rigaInfo2.Controls.Add(cellaInfo4)
                        tblInfo.Rows.Add(rigaInfo2)

                        Contenuto.Controls.Add(tblInfo)
                    End If

                    Dim objGriglia As New Griglia
                    tblDati = New Table
                    'modgg 10-06
                    tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                    objGriglia.Tabella() = tblDati
                    objGriglia.TastoDettaglio() = False
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"Data", "", "", "Operatore", "&nbsp;", "Azione", "GG.", "Documento", "Firma", ""}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {True, False, False, True, True, True, False, True, True, False}
                    objGriglia.ControlloColonna() = New String() {"DATA", "", "", "SUBSTRING", "IMAGE", "SUBSTRING", "", "POPUP", "POPUP", ""}
                    objGriglia.idControlloColonna() = New String() {"", "", "", "25", "", "35", "", "Visiona", "Certificato", ""}
                    objGriglia.VetAzioni() = New String() {"", "", "", "", "", "", "", "AnteprimaAllegatoAction.aspx", "AnteprimaCertificatoAction.aspx", ""}
                    objGriglia.PaginaCorrente = IIf(context.Session.Item("pagina") Is Nothing, 1, context.Session.Item("pagina"))
                    objGriglia.Ordina = True
                    objGriglia.IndiceOrdinamento = IIf(context.Session.Item("indice") Is Nothing, 0, context.Session.Item("indice"))
                    objGriglia.Vettore = vr(1)
                    objGriglia.cssClasse = "griglia"
                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblDati)

                    Dim tblImmagine As New Web.UI.WebControls.Table
                    Dim rigaImmagine As New Web.UI.WebControls.TableRow
                    Dim cellaImmagine As New Web.UI.WebControls.TableCell

                    Dim img As New Web.UI.WebControls.Image
                    img.ImageUrl = ".\" & vr(2)
                    cellaImmagine.HorizontalAlign = HorizontalAlign.Center
                    cellaImmagine.Controls.Add(img)
                    rigaImmagine.Controls.Add(cellaImmagine)
                    tblImmagine.Rows.Add(rigaImmagine)
                    tblImmagine.HorizontalAlign = HorizontalAlign.Center

                    Contenuto.Controls.Add(tblImmagine)

                    'modgg 12-06 4
                    Dim tblBottone As New Web.UI.WebControls.Table
                    Dim rigaBottone As New Web.UI.WebControls.TableRow
                    Dim cellaBottone As New Web.UI.WebControls.TableCell

                    Dim torna As HyperLink = New Web.UI.WebControls.HyperLink
                    torna.Visible = True
                    torna.CssClass = "link"
                    torna.Text = "Torna"
                    torna.NavigateUrl() = Request.UrlReferrer.AbsoluteUri
                    cellaBottone.HorizontalAlign = HorizontalAlign.Center
                    cellaBottone.Controls.Add(torna)
                    rigaBottone.Controls.Add(cellaBottone)
                    tblBottone.Rows.Add(rigaBottone)
                    tblBottone.HorizontalAlign = HorizontalAlign.Center
                    Contenuto.Controls.Add(tblBottone)

                    Call Pulisci_Sessione()

                End If
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub Pulisci_Sessione()
        context.Session.Remove("pagina")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

    End Sub
End Class
