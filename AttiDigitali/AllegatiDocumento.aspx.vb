Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports DllDocumentale
Imports DotCMIS
Imports System.Net
Imports System.Web.Services.Protocols
Imports Microsoft.Office.Interop.Word

Public Class AllegatiDocumento
    Inherits WebSession
    'modgg16
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents LabelOK As System.Web.UI.WebControls.Label
    Protected WithEvents LabelErrorNpagAlleg As System.Web.UI.WebControls.Label
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents tblDatiBeneficiari As System.Web.UI.WebControls.Table
    Protected WithEvents tblDatiAllegatiFatture As System.Web.UI.WebControls.Table
    Protected WithEvents tblCompiti As System.Web.UI.WebControls.Table
    Protected WithEvents btnRegistraAllegato As System.Web.UI.WebControls.Button
    Protected WithEvents txtNomeAllegato As System.Web.UI.WebControls.TextBox
    Protected WithEvents fileUpLoadAllegato1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAggiungiDocumenti As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUpLoadDocumento As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents btnCancella As System.Web.UI.WebControls.Button
    Protected WithEvents pnlAggiungiAllegato As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNomeDocumento As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnRegistraDocumento As System.Web.UI.WebControls.Button
    Protected WithEvents txtNomeAllegatoVuoto As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnRegistraAllegatoVuoto As System.Web.UI.WebControls.Button
    Protected WithEvents pnlAggiungiAllegatoVuoto As System.Web.UI.WebControls.Panel
    Protected WithEvents txtModalita As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDestinatari As System.Web.UI.WebControls.TextBox
    Protected WithEvents PnlCancella As System.Web.UI.WebControls.Panel

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Fascicolo")
    End Sub

#End Region
    Private stessoOperatoreProponente As Boolean = True
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AllegatiDocumento))



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


        Dim sSoloConsultazione As Boolean
        Dim p As Integer = 0
        Dim idTestoDocumento As String = ""
        Dim codUffProp As String = ""
        Dim tipoDocumento As String = ""

        Dim flagDocVisibileOperatore As Boolean = False
        flagDocVisibileOperatore = DocVisibileOperatore()
        Dim livelloIstanza As String = ""
        Dim vettoreAllegato As Object = Nothing
        Dim key As String = ""
        Dim codDocumento As String = ""
        Dim codAllegato As String
        Dim descrizione As String
        Dim pannelloBtn As New Panel
        pannelloBtn.HorizontalAlign = HorizontalAlign.Right
        pannelloBtn.Style.Add("padding-top", "5px")


        If Request.QueryString.Get("key") Is Nothing Then
            key = Session("codDocumento")
        Else
            key = Context.Request.QueryString.Get("key")
        End If
        Dim docDll As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
        Dim objdoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        txtTotalePagineAllegati.Visible = False
        btnRegistraTotPagineAllegati.Visible = False
        btnResetTotPagineAllegati.Visible = False
        lblTotalePagineAllegati.Visible = False

        Dim totalePagineAllegato As Integer

        If objdoc.Doc_Tipo = 1 Then
            txtTotalePagineAllegati.Visible = True
            btnRegistraTotPagineAllegati.Visible = True
            btnResetTotPagineAllegati.Visible = True
            lblTotalePagineAllegati.Visible = True

            Dim itemRicercato As New DllDocumentale.Documento_attributo
            itemRicercato.Doc_id = key
            itemRicercato.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
            itemRicercato.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
            Dim lista As Generic.List(Of DllDocumentale.Documento_attributo) = docDll.FO_Get_Documento_Attributi(itemRicercato)

            For Each item As DllDocumentale.Documento_attributo In lista
                Integer.TryParse(item.Valore, totalePagineAllegato)
            Next
        End If

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = docDll.Get_StatoIstanzaDocumento(key)
        livelloIstanza = statoIstanza.LivelloUfficio
        Dim paginaAnteprima As String = "AnteprimaAllegatoAction.aspx"
      

        If Not Context.Session.Item("soloConsultazione") Is Nothing Then
            sSoloConsultazione = Context.Session.Item("soloConsultazione")
        Else
            sSoloConsultazione = False
        End If



        Dim isUfficioCompetenzaBool As Boolean = False
        If Not objdoc.Lista_UfficiDiCompetenza Is Nothing Then
            If Not (oOperatore.oUfficio) Is Nothing Then
                Dim codUffop As String = oOperatore.oUfficio.CodUfficio
                For Each uf As DllAmbiente.StrutturaInfo In objdoc.Lista_UfficiDiCompetenza()
                    If codUffop = uf.CodiceInterno Then
                        isUfficioCompetenzaBool = True
                        Exit For
                    End If
                Next

            End If
        End If

        Context.Session.Remove("idTestoDocumento")
        Dim vr As Object = Nothing
        Dim vrMarche As Object = Nothing

        Try
            If Not Page.IsPostBack Then
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErroreRed"

                LabelOK = New Label
                LabelOK.CssClass = "lblErroreGreen"

                LabelErrorNpagAlleg = New Label
                LabelErrorNpagAlleg.CssClass = "lblErroreRed"

                
                vr = Context.Items.Item("vettoreDati")
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If vr(0) <> 0 Then

                    'modgg 10-06
                    Dim vrDoc As Object
                    vrDoc = Context.Items.Item("vettoreDocumento")
                    Dim a As String = oOperatore.oUfficio.CodUfficio
                    If Not vrDoc Is Nothing Then
                        vrDoc = CType(vrDoc, Array)
                    End If
                    If Not vrDoc Is Nothing AndAlso vrDoc(0) = 0 Then
                        Rinomina_Pagina(Me, "Fascicolo Documento  " & vrDoc(1)(3))
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

                        'label InfoUfficio
                        Dim txtUpDesc As New TextBox
                        txtUpDesc.Text = vrDoc(3)
                        txtUpDesc.TextMode = TextBoxMode.MultiLine
                        txtUpDesc.Rows = 2
                        txtUpDesc.Columns = 50
                        txtUpDesc.ReadOnly = True
                        cellaInfo4.Controls.Add(txtUpDesc)
                        rigaInfo2.Controls.Add(cellaInfo4)
                        tblInfo.Rows.Add(rigaInfo2)
                        tblInfo.HorizontalAlign = HorizontalAlign.Center

                        Contenuto.Controls.Add(tblInfo)
                    Else
                        LabelErrore.Visible = True
                        LabelErrore.Text() = CStr(vr(1))
                        Contenuto.Controls.Add(LabelErrore)
                    End If

                    PnlCancella.Visible = False


                    Dim listaFattureAtto As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)

                    listaFattureAtto = docDll.FO_Get_ListaFatture(objdoc.Doc_id)
                    If Not listaFattureAtto Is Nothing AndAlso listaFattureAtto.Count > 0 Then
                        addTableAllegatiFattura(oOperatore, listaFattureAtto)
                    End If

                Else
                    Context.Session.Remove("tipoDocumento")

                    'modgg 10-06
                    Dim vrDoc As Object
                    vrDoc = Context.Items.Item("vettoreDocumento")
                    Dim a As String = oOperatore.oUfficio.CodUfficio
                    If Not vrDoc Is Nothing Then
                        vrDoc = CType(vrDoc, Array)
                    End If


                    If (sSoloConsultazione = True) Then

                        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(Context.Request.QueryString.Get("key"))

                        Dim linkDettaglio As New WebControls.HyperLink
                        linkDettaglio.CssClass = "lbl"
                        'linkDettaglio.Text = "Visualizza Dettaglio Atto"
                        linkDettaglio.NavigateUrl = "CreaProvvedimento.aspx?tipo=" & objDocumento.Doc_Tipo & "&key=" & objDocumento.Doc_id
                        linkDettaglio.ImageUrl = "risorse/immagini/btnVisualizza/btnDettaglio.png"

                        'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                        'Contenuto.Controls.Add(linkDettaglio)
                        'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                        pannelloBtn.Controls.Add(linkDettaglio)

                        If objDocumento.haveOpContabile Then
                            Dim link As New WebControls.HyperLink
                            link.CssClass = "lbl"
                            'link.Text = "Riepilogo dati contabili"
                            link.ImageUrl = "risorse/immagini/btnVisualizza/btnDatiContabili.png"
                            If objDocumento.Doc_Tipo = 0 Or objDocumento.Doc_Tipo = 1 Then
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


                        If objDocumento.haveOpContabile Then
                            Dim link As New WebControls.HyperLink
                            link.CssClass = "lbl"
                            'link.Text = "Riepilogo mandati emessi"
                            link.ImageUrl = "risorse/immagini/btnVisualizza/btnMandati.png"
                            link.NavigateUrl = "GestioneMandati.aspx?" & Request.QueryString.ToString


                            'Contenuto.Controls.Add(New LiteralControl("<br/>"))
                            'Contenuto.Controls.Add(link)
                            'Contenuto.Controls.Add(New LiteralControl("<br/>"))

                            pannelloBtn.Controls.Add(link)
                        End If


                    End If

                    Dim linkDettaglioStorico As New WebControls.HyperLink
                    linkDettaglioStorico.CssClass = "lbl"
                    linkDettaglioStorico.ImageUrl = "risorse/immagini/btnVisualizza/btnStorico.png"
                    linkDettaglioStorico.NavigateUrl = "javascript:openPopup(" & Request.QueryString.ToString & ")"
                    pannelloBtn.Controls.Add(linkDettaglioStorico)

                    'modifica covasta download P7m 04/0/2016
                    If livelloIstanza = "UAR" Then
                        If oOperatore.Test_Gruppo("Politico") Or oOperatore.Test_Gruppo("ArchGen") Then
                            paginaAnteprima = "AnteprimaAllegatoArchivioAction.aspx"
                        End If

                        vr = Context.Items.Item("vettoreDati")
                        vrMarche = Context.Items.Item("vettoreDatiMarche")

                        If Not vr Is Nothing Then
                            vr = CType(vr, Array)
                        End If

                        If Not vrMarche Is Nothing Then
                            vrMarche = CType(vrMarche, Array)
                        End If

                        Dim i As Integer
                        Dim j1 As Integer
                        Dim index As Integer
                        Dim trovato As Boolean = False
                        Dim trovataMarca As Boolean = False

                        'prelievo indice associato alla descrizione          
                        For i = 0 To UBound(vr(1), 1)
                            For j1 = 0 To UBound(vr(1), 2)
                                descrizione = vr(1).GetValue(1, j1)
                                Dim confronto As String = "Copia firmata"

                                If (descrizione.IndexOf(confronto, 0, StringComparison.CurrentCultureIgnoreCase)) > -1 Then

                                    'If (descrizione.Contains("Copia firmata")) Then
                                    index = j1
                                    trovato = True
                                End If
                            Next
                        Next

                        If (trovato = True) Then
                            codAllegato = vr(1).getvalue(0, index)
                            Dim docP7m As Boolean = True
                            Dim imgDownloadp7m As New WebControls.HyperLink
                            imgDownloadp7m.CssClass = "lbl"
                            imgDownloadp7m.ImageUrl = "risorse/immagini/btnVisualizza/btnDownload.png"
                            imgDownloadp7m.NavigateUrl = "AnteprimaAllegatoAction.aspx?&key=" & codAllegato & "&docP7m=" & docP7m
                            pannelloBtn.Controls.Add(imgDownloadp7m)
                        End If

                       If Not vrMarche Is Nothing Then
                            'prelievo indice associato alla descrizione          
                           For i = 0 To UBound(vrMarche, 1)
                                    For j1 = 0 To UBound(vrMarche, 2)
                                        descrizione = vrMarche(i,j1)
                                        Dim confronto As String = "Marca_Temporale"

                                        If (descrizione.IndexOf(confronto, 0, StringComparison.CurrentCultureIgnoreCase)) > -1 Then

                                            'If (descrizione.Contains("Copia firmata")) Then
                                            index = j1
                                            trovataMarca = True
                                        End If
                                    Next
                           Next
                       End If

                        If (trovataMarca = True) Then
                            codAllegato = vrMarche(0, index)
                            Dim docTsr As Boolean = True
                            Dim imgDownloadMarca As New WebControls.HyperLink
                            imgDownloadMarca.CssClass = "lbl"
                            imgDownloadMarca.ImageUrl = "risorse/immagini/btnVisualizza/btnMarca.png"
                            imgDownloadMarca.NavigateUrl = "AnteprimaAllegatoAction.aspx?&key=" & codAllegato & "&docTsr=" & docTsr
                            pannelloBtn.Controls.Add(imgDownloadMarca)
                        End If



                    End If
                    Contenuto.Controls.Add(pannelloBtn)
                    If vrDoc(0) = 0 Then
                        Rinomina_Pagina(Me, "Fascicolo Documento  " & vrDoc(1)(3))
                        Dim tblInfo As New Web.UI.WebControls.Table
                        Dim rigaInfo As New Web.UI.WebControls.TableRow
                        Dim cellaInfo As New Web.UI.WebControls.TableCell
                        Dim cellaInfo2 As New Web.UI.WebControls.TableCell
                        Dim cellaInfo3 As New Web.UI.WebControls.TableCell
                        Dim cellaInfo4 As New Web.UI.WebControls.TableCell

                        Dim rigaInfo2 As New Web.UI.WebControls.TableRow

                        Dim rigaInfoConservazione As New Web.UI.WebControls.TableRow
                        Dim cellaConservazione1 As New Web.UI.WebControls.TableCell
                        Dim cellaConservazione2 As New Web.UI.WebControls.TableCell

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

                        'label InfoUfficio
                        Dim txtUpDesc As New TextBox
                        txtUpDesc.Text = vrDoc(3)
                        txtUpDesc.TextMode = TextBoxMode.MultiLine
                        txtUpDesc.Rows = 2
                        txtUpDesc.Columns = 50
                        txtUpDesc.ReadOnly = True
                        cellaInfo4.Controls.Add(txtUpDesc)
                        rigaInfo2.Controls.Add(cellaInfo4)
                        tblInfo.Rows.Add(rigaInfo2)
                        tblInfo.HorizontalAlign = HorizontalAlign.Center

                        Contenuto.Controls.Add(tblInfo)
                    'End If
                    If totalePagineAllegato = 0 Then
                        txtTotalePagineAllegati.Text = ""
                    Else
                        txtTotalePagineAllegati.Text = totalePagineAllegato
                    End If
                        If (statoIstanza.LivelloUfficio = "UAR") Then
                            'Label Ufficio Proponente
                            Dim lblConservazione As New Label
                            lblConservazione.Text = "Stato Conservazione: "
                            lblConservazione.CssClass = "lbl"
                            cellaConservazione1.Controls.Add(lblConservazione)
                            rigaInfoConservazione.Controls.Add(cellaConservazione1)
                            'label InfoUfficio
                            Dim lblConservazioneStato As New Label

                            'NB. Gli allegati dello storico dei provvedimenti (01/01/2008-22/02/2018) potrebbero ancora non essere inseriti in Documento_Conservazione (Scheduler StoricoAttiScheduler del batch conservazioneAttiScheduler) 
                            Dim listaDocumentiConservazioneAtto As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"))
                            lblConservazioneStato.Text = "IN ATTESA"
                            lblConservazioneStato.ToolTip = "Il provvedimento non risulta essere inviato al sistema di conservazione"
                            lblConservazioneStato.ForeColor = Color.LightSlateGray

                            If Not listaDocumentiConservazioneAtto Is Nothing And listaDocumentiConservazioneAtto.Count > 0 Then

                                Dim listaDocumentiConservazioneAttoReady As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"), Dic_FODocumentale.cfo_Conservazione_Stato_DOCUMENT_READY)
                                Dim listaDocumentiConservazioneAttoLive As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"), Dic_FODocumentale.cfo_Conservazione_Stato_LIVE)
                                Dim listaDocumentiConservazioneAttoWork As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"), Dic_FODocumentale.cfo_Conservazione_Stato_WORK)
                                Dim listaDocumentiConservazioneAttoError As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"), Dic_FODocumentale.cfo_Conservazione_Stato_ERROR)
                                Dim listaDocumentiConservazioneAttoErrorWS As List(Of ItemDocumentoConservazione) = docDll.FO_GetDocumentoConservazioneAllegati(Request.QueryString.Get("key"), Dic_FODocumentale.cfo_Conservazione_Stato_ERROR_WS)


                                If listaDocumentiConservazioneAttoReady.Count = listaDocumentiConservazioneAtto.Count Then
                                    lblConservazioneStato.Text = "IN ATTESA"
                                    lblConservazioneStato.ToolTip = "Il provvedimento non risulta essere inviato al sistema di conservazione"
                                    lblConservazioneStato.ForeColor = Color.LightSlateGray

                                End If

                                If listaDocumentiConservazioneAttoWork.Count = listaDocumentiConservazioneAtto.Count Then
                                    lblConservazioneStato.Text = "IN LAVORAZIONE"
                                    lblConservazioneStato.ToolTip = "Il provvedimento risulta essere inviato al repository e in attesa di essere mandato in conservazione da Aruba"
                                    lblConservazioneStato.ForeColor = Color.Orange

                                End If

                                If (listaDocumentiConservazioneAttoWork.Count <> listaDocumentiConservazioneAtto.Count And listaDocumentiConservazioneAttoWork.Count > 0) Or
                                    (listaDocumentiConservazioneAttoLive.Count <> listaDocumentiConservazioneAtto.Count And listaDocumentiConservazioneAttoLive.Count > 0) Or
                                    (listaDocumentiConservazioneAttoError.Count <> listaDocumentiConservazioneAtto.Count And listaDocumentiConservazioneAttoError.Count > 0) Or
                                    (listaDocumentiConservazioneAttoErrorWS.Count <> listaDocumentiConservazioneAtto.Count And listaDocumentiConservazioneAttoErrorWS.Count > 0) Then
                                    lblConservazioneStato.Text = "PARZIALE"
                                    lblConservazioneStato.ToolTip = "Il provvedimento risulta essere inviato parzialmente al sistema di conservazione"
                                    lblConservazioneStato.ForeColor = Color.Red
                                End If

                                'Tutti i file (PDM, PDF) del provvedimento in Documento_Conservazione hanno stato LIVE
                                If listaDocumentiConservazioneAttoLive.Count = listaDocumentiConservazioneAtto.Count Then
                                    lblConservazioneStato.Text = "LIVE"
                                    lblConservazioneStato.ToolTip = "Il provvedimento risulta essere inviato correttamente al sistema di conservazione"
                                    lblConservazioneStato.ForeColor = Color.Green
                                End If

                            End If

                            lblConservazioneStato.Font.Bold = True
                            lblConservazioneStato.CssClass = "lbl"
                            cellaConservazione2.Controls.Add(lblConservazioneStato)
                            rigaInfoConservazione.Controls.Add(cellaConservazione2)

                            txtUpDesc.Rows = 2

                            tblInfo.Rows.Add(rigaInfoConservazione)
                        End If

                        txtUpDesc.Columns = 50
                        txtUpDesc.ReadOnly = True
                        cellaInfo4.Controls.Add(txtUpDesc)
                        rigaInfo2.Controls.Add(cellaInfo4)
                        
                        tblInfo.HorizontalAlign = HorizontalAlign.Center

                        Contenuto.Controls.Add(tblInfo)
                        '   End If
                    End If
                    

                    codUffProp = vr(3)
                    Dim collaboratori As Hashtable

                    collaboratori = oOperatore.oUfficio.UtentiUfficio("")

                    Dim j As Integer
                    Dim objGriglia As New Griglia
                    Dim vettSeparato As Object
                    vettSeparato = separaArrayPerTipo(vr(1), 1)
                    For j = 0 To UBound(vettSeparato, 1)

                        tblDati = New WebControls.Table
                        'modgg 10-06
                        tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                        objGriglia.Tabella() = tblDati
                        objGriglia.TastoDettaglio() = True
                        objGriglia.Trasposta() = True
                        If UCase(vettSeparato(j).getvalue(1, 0)) = "RIFERIMENTO ALLEGATO CARTACEO" Then
                            If vettSeparato(j).getvalue(6, vettSeparato(j).getlowerBound(1)) = "0" Or (sSoloConsultazione = True) Then
                                objGriglia.VetDatiIntestazione() = New String() {"", "Tipo", "Nome", "", "Autore", "", "", "Rintracciabilità", "Referente"}
                                objGriglia.ControlloColonna = New String() {"", "", "", "", "", "", "", "", ""}
                                objGriglia.idControlloColonna = New String() {"", "", "", "", "", "", "", "", ""}
                                objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, False, True, False, False, True, True}
                            Else
                                objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Tipo", "Nome", "", "Autore", "", "", "Rintracciabilità", "Referente"}
                                objGriglia.ControlloColonna = New String() {"CHECK", "", "", "", "", "", "", "", ""}
                                objGriglia.idControlloColonna = New String() {"chkCancella", "", "", "", "", "", "", "", ""}
                                objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, False, True, False, False, True, True}
                                PnlCancella.Visible = True
                            End If
                        Else
                            If vettSeparato(j).getvalue(6, vettSeparato(j).getlowerBound(1)) = "0" Or (sSoloConsultazione = True) Then
                                objGriglia.VetDatiIntestazione() = New String() {"", "Tipo", "Nome", "", "Autore", "Anteprima", "", ""}
                                objGriglia.ControlloColonna = New String() {"", "", "", "", "", "POPUP", "", "", ""}
                                objGriglia.idControlloColonna = New String() {"", "", "", "", "", "Anteprima", "", "", ""}
                                objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, True, False, True, True, False, True, True}

                            Else

                                Dim flagVisualizzacheck As Boolean = True
                                If UCase(vettSeparato(j).getvalue(1, 0)).ToString().IndexOf("DETERMINA") >= 0 Or UCase(vettSeparato(j).getvalue(1, 0)).ToString().IndexOf("DISPOSIZIONE") >= 0 Then
                                    flagVisualizzacheck = False
                                End If
                                objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Tipo", "Nome", "", "Autore", "Anteprima", "", ""}
                                objGriglia.ControlloColonna = New String() {"CHECK", "", "", "", "", "POPUP", "", "", ""}
                                objGriglia.idControlloColonna = New String() {"chkCancella", "", "", "", "", "Anteprima", "", "", ""}
                                objGriglia.VetDatiNonVisibili() = New Boolean() {flagVisualizzacheck, True, True, False, True, True, False, True, True}
                                PnlCancella.Visible = True
                            End If

                        End If


                        objGriglia.VetAzioni = New String() {"", "", "", "", "", paginaAnteprima, "", ""}
                        objGriglia.Vettore = vettSeparato(j)
                        objGriglia.crea_tabella_daVettore()
                        If (sSoloConsultazione = False) Then
                            Select Case CStr(UCase(vettSeparato(j).getvalue(1, 0)))
                                Case "ALLEGATO"
                                    tblDati.ID = "ALLEGATO"
                                    pnlAggiungiAllegato.Controls.Add(tblDati)
                                    pnlAggiungiAllegato.Visible = True
                                    'Contenuto.Controls.Add(pnlAggiungiAllegato)
                                    pnlAggiungiAllegato.Visible = True
                                    'Contenuto.Controls.Add(pnlAggiungiAllegato)
                                Case "DOCUMENTI"
                                    tblDati.ID = "DOCUMENTI"
                                    If collaboratori.ContainsValue(vettSeparato(j).getvalue(4, p)) Then
                                        If p < UBound(vettSeparato(j), 2) Then
                                            p = p + 1
                                        End If
                                        pnlAggiungiDocumenti.Controls.Add(tblDati)
                                        pnlAggiungiDocumenti.Visible = True
                                        'Contenuto.Controls.Add(pnlAggiungiDocumenti)
                                        pnlAggiungiDocumenti.Visible = True
                                        'Contenuto.Controls.Add(pnlAggiungiDocumenti)
                                    End If

                                Case "RIFERIMENTO ALLEGATO CARTACEO"
                                    tblDati.ID = "CARTACEO"
                                    pnlAggiungiAllegatoVuoto.Controls.Add(tblDati)
                                    pnlAggiungiAllegatoVuoto.Visible = True
                                    'Contenuto.Controls.Add(pnlAggiungiAllegatoVuoto)
                                    pnlAggiungiAllegatoVuoto.Visible = True
                                    'Contenuto.Controls.Add(pnlAggiungiAllegatoVuoto)

                                Case "TESTO DEL DOCUMENTO"
                                    Context.Session.Add("idTestoDocumento", vettSeparato(j).getvalue(0, 0))
                                    If codUffProp = oOperatore.oUfficio.CodUfficio Then
                                        Contenuto.Controls.Add(tblDati)
                                    End If
                                Case "DETERMINA"

                                    If flagDocVisibileOperatore Then
                                        tipoDocumento = "DETERMINA"
                                        Context.Session.Add("tipoDocumento", tipoDocumento)
                                        Context.Session.Add("idDocumento", vettSeparato(j).getvalue(0, 0))
                                        Contenuto.Controls.Add(tblDati)
                                    End If

                                Case "DELIBERA"
                                    If flagDocVisibileOperatore Then
                                        tipoDocumento = "DELIBERA"
                                        Context.Session.Add("tipoDocumento", tipoDocumento)
                                        Context.Session.Add("idDocumento", vettSeparato(j).getvalue(0, 0))
                                        Contenuto.Controls.Add(tblDati)
                                    End If
                                Case "DISPOSIZIONE"

                                    If flagDocVisibileOperatore Then
                                        tipoDocumento = "DISPOSIZIONE"
                                        Context.Session.Add("tipoDocumento", tipoDocumento)
                                        Context.Session.Add("idDocumento", vettSeparato(j).getvalue(0, 0))
                                        Contenuto.Controls.Add(tblDati)
                                    End If
                                Case "PDFDOCUMENTO"


                                Case Else
                                    Contenuto.Controls.Add(tblDati)
                            End Select
                        Else
                            'Modifica per visualizzare doc word in archivio - Gio 240517
                            If UCase(vettSeparato(j).getvalue(1, 0)) <> "PDFDOCUMENTO" And UCase(vettSeparato(j).getvalue(1, 0)) <> "DETERMINA_" And UCase(vettSeparato(j).getvalue(1, 0)) <> "DISPOSIZIONE_" Then
                                Contenuto.Controls.Add(tblDati)
                            End If
                        End If
                    Next
                    'E' stata inserita la visualizzazione del pdf con il riepilogo dei beneficiari




                    Dim listaLiquidazioni As IList(Of DllDocumentale.ItemLiquidazioneInfo)
                    Dim listaBeneficiari As IList(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
                    Dim hasBeneficiari As Boolean = False
                    listaLiquidazioni = docDll.FO_Get_DatiLiquidazione(key, 0, "", False)

                    Dim listaFattureAtto As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)

                    listaFattureAtto = docDll.FO_Get_ListaFatture(objdoc.Doc_id)
                    If Not listaFattureAtto Is Nothing AndAlso listaFattureAtto.Count > 0 Then
                        addTableAllegatiFattura(oOperatore, listaFattureAtto)
                    End If


                    If (listaLiquidazioni.Count > 0) Then
                        For Each liquidazione As ItemLiquidazioneInfo In listaLiquidazioni
                            listaBeneficiari = docDll.FO_Get_ListaBeneficiariLiquidazione(oOperatore, liquidazione.Dli_Documento, , liquidazione.Dli_prog)
                            If (listaBeneficiari.Count > 0) Then
                                hasBeneficiari = True
                            End If
                        Next
                    End If

                    If (hasBeneficiari) Then
                        Dim paginaAnteprimaBeneficiari As String = "ApriDatiContabiliPDFAction.aspx"
                        Dim grigliaFileBeneficiari As New Griglia
                        Dim vettoreDatiGrigliaBeneficiari As Object
                        Dim contenutoGrigliaBeneficiari(,) As Object = New Array(,) {}
                        ReDim contenutoGrigliaBeneficiari(8, 0)
                        'Nella posizione contenutoGrigliaBeneficiari(0,0) viene passato l'idDocumento (key), che viene concatenato poi 
                        ' nella queryString per l'azione indicata nel VetAzioni.
                        contenutoGrigliaBeneficiari.SetValue(key, 0, 0)
                        contenutoGrigliaBeneficiari.SetValue("Riepilogo Beneficiari in PDF", 1, 0)
                        contenutoGrigliaBeneficiari.SetValue("Beneficiari", 2, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("pdf", Object), 3, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("System", Object), 4, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("", Object), 5, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("", Object), 6, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("", Object), 7, 0)
                        contenutoGrigliaBeneficiari.SetValue(CType("", Object), 8, 0)
                        'arrayTemp: vettore di appoggio, necessario per rispettare la struttura dati preesitente
                        '   per la costruzione della griglia!!!
                        Dim arrayTemp(0) As Array
                        arrayTemp.SetValue(contenutoGrigliaBeneficiari, 0)
                        vettoreDatiGrigliaBeneficiari = arrayTemp
                        grigliaFileBeneficiari.Vettore = vettoreDatiGrigliaBeneficiari(0)
                        tblDatiBeneficiari = New WebControls.Table
                        tblDatiBeneficiari.Width = Web.UI.WebControls.Unit.Pixel(750)
                        grigliaFileBeneficiari.Tabella() = tblDatiBeneficiari
                        grigliaFileBeneficiari.TastoDettaglio() = True
                        grigliaFileBeneficiari.Trasposta() = True
                        grigliaFileBeneficiari.VetDatiIntestazione() = New String() {"", "Tipo", "Nome", "", "Autore", "Anteprima", "", "", ""}
                        grigliaFileBeneficiari.ControlloColonna = New String() {"", "", "", "", "", "POPUP", "", "", ""}
                        grigliaFileBeneficiari.idControlloColonna = New String() {"", "", "", "", "", "Anteprima", "", "", ""}
                        grigliaFileBeneficiari.VetDatiNonVisibili() = New Boolean() {False, True, True, False, False, True, False, False, False}
                        grigliaFileBeneficiari.VetAzioni = New String() {"", "", "", "", "", paginaAnteprimaBeneficiari, "", ""}
                        Contenuto.Controls.Add(tblDatiBeneficiari)

                        grigliaFileBeneficiari.crea_tabella_daVettore()
                    End If
                End If

                If Not Context.Items.Item("lblError") Is Nothing Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(Context.Items.Item("lblError"))
                    Contenuto.Controls.Add(LabelErrore)
                End If

                If Not Context.Items.Item("lblOK") Is Nothing Then
                    LabelOK.Visible = True
                    LabelOK.Text() = CStr(Context.Items.Item("lblOK"))
                    Contenuto.Controls.Add(LabelOK)
                End If

                If Not Context.Items.Item("lblErrorNpagAlleg") Is Nothing Then
                    LabelErrorNpagAlleg.Visible = True
                    LabelErrorNpagAlleg.Text() = CStr(Context.Items.Item("lblErrorNpagAlleg"))
                    Contenuto.Controls.Add(LabelErrorNpagAlleg)
                End If

                Contenuto.Controls.Add(pnlTotalePagineAllegati)

                If (sSoloConsultazione = True And isUfficioCompetenzaBool = False) Then
                    txtTotalePagineAllegati.Enabled = False
                    btnRegistraTotPagineAllegati.Visible = False
                    btnResetTotPagineAllegati.Visible = False

                    pnlAggiungiAllegato.Visible = False
                    pnlAggiungiDocumenti.Visible = False
                    pnlAggiungiAllegatoVuoto.Visible = False
                Else

                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                    Contenuto.Controls.Add(pnlAggiungiDocumenti)
                    Contenuto.Controls.Add(pnlAggiungiAllegatoVuoto)
                    Contenuto.Controls.Add(PnlCancella)
                End If

                vr = Context.Items.Item("vettoreCompiti")
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                Else
                    Exit Sub
                End If
                If vr(0) = 0 Then
                    Dim lblCompitiDoc As New WebControls.Label
                    lblCompitiDoc.ID = "lblCompitiDoc"
                    lblCompitiDoc.Text = "Sul documento hanno lavorato :"
                    Contenuto.Controls.Add(lblCompitiDoc)

                    Dim objGriglia As New Griglia
                    tblCompiti = New WebControls.Table
                    objGriglia.Tabella() = tblCompiti
                    objGriglia.TastoDettaglio() = True
                    objGriglia.Trasposta() = True
                    objGriglia.VetDatiIntestazione() = New String() {"", "Utente", "", "Compito", ""}
                    objGriglia.ControlloColonna = New String() {"", "", "", "", ""}
                    objGriglia.idControlloColonna = New String() {"", "", "", "", ""}
                    objGriglia.VetDatiNonVisibili() = New Boolean() {False, True, False, True, False}
                    objGriglia.VetAzioni = New String() {"", "", "", "", ""}
                    objGriglia.FlagPaginazione = False
                    objGriglia.Vettore = vr(1)
                    objGriglia.crea_tabella_daVettore()
                    Contenuto.Controls.Add(tblCompiti)
                End If


                Call Pulisci_Sessione()
            End If
            gestionePulsanti(flagDocVisibileOperatore, isUfficioCompetenzaBool)

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub
    Function DocVisibileOperatore() As Boolean
        Dim flagDocVisibile As Boolean = True
        Dim obj As Object = Leggi_Documento(Context.Request.QueryString.Get("key"))
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


        If obj(0) = 0 Then
            If obj(2) <> oOperatore.oUfficio.CodUfficio Then
                stessoOperatoreProponente = False
                If oOperatore.Test_Ruolo("CT001") Or oOperatore.Test_Ruolo("CL001") Then
                    flagDocVisibile = True
                Else
                    flagDocVisibile = False
                End If
            End If
        End If

        Return flagDocVisibile

    End Function



    Private Sub gestionePulsanti(ByVal flagDocVisibileOperatore As Boolean, ByVal isUfficoCompetenza As Boolean)

        Dim controlEtichetta As Control = Contenuto.FindControl("lblCompitiDoc")
        Dim indexEtichetta As Integer = Contenuto.Controls.IndexOf(controlEtichetta)


        If stessoOperatoreProponente = False Then
            pnlAggiungiAllegato.Visible = False
            pnlAggiungiAllegatoVuoto.Visible = False
            pnlAggiungiDocumenti.Visible = False
            btnCancella.Enabled = False
            btnRegistraAllegato.Enabled = False
            btnRegistraAllegatoVuoto.Enabled = False
            btnRegistraDocumento.Enabled = False
            PnlCancella.Visible = False
            txtTotalePagineAllegati.Enabled = False
            btnRegistraTotPagineAllegati.Visible = False
            btnResetTotPagineAllegati.Visible = False
            txtTotalePagineAllegati.Enabled = False
            
            If flagDocVisibileOperatore Then
                txtTotalePagineAllegati.Enabled = True
                btnRegistraTotPagineAllegati.Visible = True
                btnResetTotPagineAllegati.Visible = True

                pnlAggiungiAllegato.Visible = True
                btnRegistraAllegato.Enabled = False
                btnCancella.Enabled = True
                PnlCancella.Visible = True
                btnRegistraAllegato.Enabled = True



                If Not Contenuto.Controls.Contains(pnlAggiungiDocumenti) Then


                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                    Contenuto.Controls.AddAt(indexEtichetta, pnlAggiungiAllegato)
                    indexEtichetta += 1
                End If
                If Not Contenuto.Controls.Contains(pnlAggiungiDocumenti) Then
                    Contenuto.Controls.AddAt(indexEtichetta, PnlCancella)
                    indexEtichetta += 1
                End If


            End If

        End If




        Dim obj As Object
        'contenuto.Controls.i
        If pnlAggiungiAllegato.Visible = False Then
            obj = pnlAggiungiAllegato.FindControl("ALLEGATO")
            If Not obj Is Nothing Then

                Contenuto.Controls.AddAt(indexEtichetta, New LiteralControl("<br/>"))
                Contenuto.Controls.AddAt(indexEtichetta + 1, obj)
            End If


        End If

        indexEtichetta = Contenuto.Controls.IndexOf(controlEtichetta)

        If flagDocVisibileOperatore = True And pnlAggiungiDocumenti.Visible = False Then
            obj = pnlAggiungiDocumenti.FindControl("DOCUMENTI")
            If Not obj Is Nothing Then
                Contenuto.Controls.AddAt(indexEtichetta, New LiteralControl("<br/>"))
                Contenuto.Controls.AddAt(indexEtichetta + 1, obj)
            End If
        End If

        indexEtichetta = Contenuto.Controls.IndexOf(controlEtichetta)

        If pnlAggiungiAllegatoVuoto.Visible = False Then
            obj = pnlAggiungiAllegatoVuoto.FindControl("CARTACEO")
            If Not obj Is Nothing Then
                Contenuto.Controls.AddAt(indexEtichetta, New LiteralControl("<br/>"))
                Contenuto.Controls.AddAt(indexEtichetta + 1, obj)
            End If

        End If


        '            End If
        '       End If

        If isUfficoCompetenza Then
            pnlAggiungiAllegato.Visible = True
            pnlAggiungiAllegatoVuoto.Visible = True
            pnlAggiungiDocumenti.Visible = True

            btnRegistraAllegato.Enabled = True
            btnRegistraAllegatoVuoto.Enabled = True
            btnRegistraDocumento.Enabled = True

            txtTotalePagineAllegati.Enabled = True
            btnRegistraTotPagineAllegati.Visible = True
            btnResetTotPagineAllegati.Visible = True
        End If

    End Sub
    Private Sub Pulisci_Sessione()
        Session.Remove("bFileAllegato")
        Session.Remove("nomeAllegato")
        Session.Remove("bFileDocumento")
        Session.Remove("nomeDocumento")
        Session.Remove("elencoAllegatiDaCancellare")
        Session.Remove("soloConsultazione")


        Session.Remove("nomeAllegatoVuoto")
        Session.Remove("modalitaTrasmissioneAllegatoVuoto")
        Session.Remove("destinatariAllegatoVuoto")

    End Sub

    Private Sub btnRegistraAllegato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistraAllegato.Click

        If Not Request.QueryString.Get("key") Is Nothing Then
            Session.Add("codDocumento", Request.QueryString.Get("key"))
        End If

        Session.Remove("bFileAllegato")
        Session.Remove("nomeAllegato")

        If fileUpLoadAllegato1.PostedFile.ContentLength() > 0 Then

            Dim fileStream As IO.Stream
            fileStream = fileUpLoadAllegato1.PostedFile.InputStream
            Dim bFile(fileStream.Length) As Byte
            fileStream.Read(bFile, 0, CInt(fileStream.Length))
            Context.Session.Add("nomeFileAllegato", fileUpLoadAllegato1.PostedFile.FileName)
            Session.Add("bFileAllegato", bFile)

            Session.Add("nomeAllegato", txtNomeAllegato.Text)
            If Trim(Context.Request.QueryString.Get("key")) <> "" Then
                Session.Add("key", Context.Request.QueryString.Get("key"))
            End If
        End If
        Response.Redirect("AggiungiAllegatoAction.aspx")

    End Sub
    Private Sub btnRegistraTotPagineAllegati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistraTotPagineAllegati.Click
        If Not Request.QueryString.Get("key") Is Nothing Then
            Session.Add("codDocumento", Request.QueryString.Get("key"))
        End If
        Session.Add("resetTotalePagineAllegati", 0)
        Session.Add("totalePagineAllegati", txtTotalePagineAllegati.Text)
        If Trim(Context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", Context.Request.QueryString.Get("key"))
        End If

        Response.Redirect("SetTotPagineAllegatiAction.aspx")

    End Sub
    Private Sub btnResetTotPagineAllegati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetTotPagineAllegati.Click
        If Not Request.QueryString.Get("key") Is Nothing Then
            Session.Add("codDocumento", Request.QueryString.Get("key"))
        End If

        Session.Add("resetTotalePagineAllegati", 1)
        If Trim(Context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", Context.Request.QueryString.Get("key"))
        End If

        Response.Redirect("SetTotPagineAllegatiAction.aspx")

    End Sub
    Private Sub btnRegistraDocumento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistraDocumento.Click
        If Not Request.QueryString.Get("key") Is Nothing Then
            Session.Add("codDocumento", Request.QueryString.Get("key"))
        End If
        If fileUpLoadDocumento.PostedFile.ContentLength() > 0 Then

            Dim fileStreamDoc As IO.Stream
            fileStreamDoc = fileUpLoadDocumento.PostedFile.InputStream
            Dim bFileDoc(fileStreamDoc.Length) As Byte
            fileStreamDoc.Read(bFileDoc, 0, CInt(fileStreamDoc.Length))
            Context.Session.Add("nomeFileDocumento", fileUpLoadDocumento.PostedFile.FileName)
            Session.Add("bFileDocumento", bFileDoc)
            Session.Add("nomeDocumento", txtNomeDocumento.Text)
            If Trim(Context.Request.QueryString.Get("key")) <> "" Then
                Session.Add("key", Context.Request.QueryString.Get("key"))
            End If
        End If

        Response.Redirect("AggiungiAllegatoAction.aspx")

    End Sub




    Private Sub btnCancella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Session.Add("elencoAllegatiDaCancellare", Request.Item("chkCancella"))
        If Trim(Context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", Context.Request.QueryString.Get("key"))
        End If
        Response.Redirect("CancellaAllegatiAction.aspx")
    End Sub

    Private Sub OleDbConnection1_InfoMessage(ByVal sender As System.Object, ByVal e As System.Data.OleDb.OleDbInfoMessageEventArgs)

    End Sub

    Private Sub btnRegistraAllegatoVuoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistraAllegatoVuoto.Click
        If Not Request.QueryString.Get("key") Is Nothing Then
            Session.Add("codDocumento", Request.QueryString.Get("key"))
        End If
        'If txtNomeAllegatoVuoto.Text <> "" Then
        '    Session.Add("nomeAllegatoVuoto", txtNomeAllegatoVuoto.Text)
        '    Session.Add("modalitaTrasmissioneAllegatoVuoto", txtModalita.Text)
        '    Session.Add("destinatariAllegatoVuoto", txtDestinatari.Text)




        '    If Trim(context.Request.QueryString.Get("key")) <> "" Then
        '        Session.Add("key", context.Request.QueryString.Get("key"))
        '    End If
        'End If

        If txtNomeAllegatoVuoto.Text <> "" Then

            Dim informazioni_Cartacee As New StringBuilder
            informazioni_Cartacee.Append("<html><body>")
            informazioni_Cartacee.Append("<p>Nome Allegato: " & txtNomeAllegatoVuoto.Text & "</p>")
            informazioni_Cartacee.Append("<p>Rintracciabilità: " & txtModalita.Text & "</p>")
            informazioni_Cartacee.Append("<p>Referente: " & txtDestinatari.Text & "</p>")
            informazioni_Cartacee.Append("</body></html>")
            Dim content As Byte() = System.Text.UnicodeEncoding.Unicode.GetBytes(informazioni_Cartacee.ToString)
            'Dim wr As System.IO.FileStream = New System.IO.FileStream("C:\\myhtml.html", System.IO.FileMode.CreateNew)
            'wr.Write(content, 0, content.Length)
            'wr.Close()
            'Dim stream As System.IO.FileStream
            'stream.Write(
            'stream.Write()
            'stream.Close()
            Session.Add("bFileCartaceo", content)


            Session.Add("nomeAllegatoVuoto", txtNomeAllegatoVuoto.Text)
            Session.Add("modalitaTrasmissioneAllegatoVuoto", txtModalita.Text)
            Session.Add("destinatariAllegatoVuoto", txtDestinatari.Text)
            If Trim(Context.Request.QueryString.Get("key")) <> "" Then
                Session.Add("key", Context.Request.QueryString.Get("key"))
            End If
        End If

        Response.Redirect("AggiungiAllegatoAction.aspx")
    End Sub

    Private Sub addTableAllegatiFattura(ByVal oOperatore As DllAmbiente.Operatore, ByVal listafattureComplessive As Generic.List(Of ItemFatturaInfoHeader))
        tblDatiAllegatiFatture = New System.Web.UI.WebControls.Table

        Dim primaryStyle As New WebControls.Style()
        primaryStyle.CssClass = "tabellaAllegatiFattura"
        tblDatiAllegatiFatture.ApplyStyle(primaryStyle)

        Dim rigaIntestazione As New TableHeaderRow

        Dim numFatturaI As New TableHeaderCell()
        numFatturaI.Controls.Add(New LiteralControl("N°. Fattura"))
        numFatturaI.HorizontalAlign = HorizontalAlign.Left
        'numFatturaI.Width = 170
        numFatturaI.Width = 135
        rigaIntestazione.Cells.Add(numFatturaI)

        Dim importoFatturaI As New TableHeaderCell()
        importoFatturaI.Controls.Add(New LiteralControl("Importo"))
        importoFatturaI.HorizontalAlign = HorizontalAlign.Left
        importoFatturaI.Width = 40
        rigaIntestazione.Cells.Add(importoFatturaI)


        Dim denominazioneBenI As New TableHeaderCell()
        denominazioneBenI.Controls.Add(New LiteralControl("Beneficiario"))
        denominazioneBenI.HorizontalAlign = HorizontalAlign.Left
        denominazioneBenI.Width = 160
        rigaIntestazione.Cells.Add(denominazioneBenI)

        Dim dataFatturaI As New TableHeaderCell()
        dataFatturaI.Controls.Add(New LiteralControl("Data"))
        dataFatturaI.HorizontalAlign = HorizontalAlign.Left
        dataFatturaI.Width = 65
        rigaIntestazione.Cells.Add(dataFatturaI)

        Dim nomeAllegatoIntest As New TableHeaderCell()
        nomeAllegatoIntest.Controls.Add(New LiteralControl("Nome File"))
        nomeAllegatoIntest.HorizontalAlign = HorizontalAlign.Left
        nomeAllegatoIntest.Width = 310
        rigaIntestazione.Cells.Add(nomeAllegatoIntest)

        Dim visualizzaIntest As New TableHeaderCell()
        visualizzaIntest.Controls.Add(New LiteralControl("Visualizza"))
        visualizzaIntest.HorizontalAlign = HorizontalAlign.Center
        visualizzaIntest.Width = 70
        rigaIntestazione.Cells.Add(visualizzaIntest)

        tblDatiAllegatiFatture.Rows.Add(rigaIntestazione)

        Dim allegatiPresenti As Boolean = False
        Dim listaAllegatiFatt As Generic.List(Of ItemFatturaAllegato) = GetListaAllegatiFatture(oOperatore, listafattureComplessive)
        If Not listaAllegatiFatt Is Nothing AndAlso listaAllegatiFatt.Count > 0 Then
            allegatiPresenti = True

            For Each allegato As ItemFatturaAllegato In listaAllegatiFatt
                If allegato.Url.ToLower.Contains("protocollato") Then
                    Dim rigaAllegato As New TableRow()

                    Dim cFatt As New TableCell()
                    cFatt.Controls.Add(New LiteralControl("" & allegato.NumeroFatturaBeneficiario))
                    rigaAllegato.Cells.Add(cFatt)

                    Dim importo As Double
                    For Each fattura As DllDocumentale.ItemFatturaInfoHeader In listafattureComplessive
                        If fattura.Prog = allegato.ProgFattura Then
                            importo = fattura.ImportoTotaleFattura
                        End If
                    Next
                    Dim impFatt As New TableCell()
                    impFatt.Controls.Add(New LiteralControl("" & importo & " €"))
                    rigaAllegato.Cells.Add(impFatt)

                    Dim denominazioneBen As New TableCell()
                    denominazioneBen.Controls.Add(New LiteralControl("" & allegato.DenominazioneBeneficiario))
                    rigaAllegato.Cells.Add(denominazioneBen)

                    Dim cDataFattura As New TableCell()
                    cDataFattura.Controls.Add(New LiteralControl(allegato.DataFatturaBeneficiario))
                    rigaAllegato.Cells.Add(cDataFattura)

                    Dim cNomeAllegato As New TableCell()
                    cNomeAllegato.Controls.Add(New LiteralControl(allegato.Nome))
                    rigaAllegato.Cells.Add(cNomeAllegato)

                    Dim linkAllegato As New WebControls.HyperLink

                    linkAllegato.NavigateUrl = "DownloadAllegatoFatturaAction.aspx?nome=" & allegato.Nome & "&urlAllegatoFattura=" & allegato.Url
                    linkAllegato.Target = "_new"
                    linkAllegato.ImageUrl = "risorse/immagini/Download-Blu.png"

                    Dim cUrlAllegato As New TableCell()
                    cUrlAllegato.Controls.Add(linkAllegato)
                    cUrlAllegato.HorizontalAlign = HorizontalAlign.Center
                    rigaAllegato.Cells.Add(cUrlAllegato)

                    tblDatiAllegatiFatture.Rows.Add(rigaAllegato)
                End If
            Next
        Else
            allegatiPresenti = False
        End If
        If Not tblDatiAllegatiFatture Is Nothing AndAlso allegatiPresenti Then
            Contenuto.Controls.Add(tblDatiAllegatiFatture)
        End If
    End Sub
End Class
