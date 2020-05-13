Imports System.Xml.XPath
Imports System.IO
Imports Microsoft.Web.UI.WebControls
Imports System.Xml
Imports DllDocumentale
Imports System.Collections.Generic

Public Class Determina
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents prova As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table1 As System.Web.UI.WebControls.Table
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents formProposta As System.Web.UI.WebControls.DataList
    Protected WithEvents ButtonContinua As System.Web.UI.WebControls.Button
    Protected WithEvents ddlTipoPubblicazione As System.Web.UI.WebControls.DropDownList

    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUpLoadAllegato As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents rqvControllo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents controlloCheck As System.Web.UI.WebControls.TextBox

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
        Inizializza_Pagina(Me, "Determina")

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim xmlDatiLetti As New System.Xml.XmlDocument
            Dim sXmlDatiLetti As String
            Dim oggetto As String
            Dim intContabile, pubIntegrale As String
            Dim TabellaDettaglio As New Web.UI.WebControls.Table
            Dim titoloCelle As Web.UI.WebControls.Literal
            Dim ceckOggetto As New Web.UI.WebControls.RequiredFieldValidator
            Dim strMessaggio As String = ""
            If Not Context.Session("RegistraOggetto") Is Nothing Then
                If Context.Session("RegistraOggetto") = 1 Then
                    strMessaggio = "Modifiche salvate con successo"
                    'salvataggio Avvenuto con successo
                    Context.Session.Remove("RegistraOggetto")

                End If
            End If

            If Not Page.IsPostBack Then
                controlloCheck.Attributes.CssStyle.Add("display", "none")
                controlloCheck.Text = 1
                Dim key As String = Context.Request.QueryString.Get("key")
                Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

                chkImpegno.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
                chkImpegnoSuPerenti.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
                chkAccertamento.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
                chkLiquidazione.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
                chkRiduzione.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)
                If Not objDocumento.haveOpContabile Then
                    chkNessuna.Checked = True
                End If

                Rinomina_Pagina(Me, " Determina " & Context.Items.Item("numeroDoc"))

                sXmlDatiLetti = Context.Items.Item("xmlDati")
                oggetto = Context.Items.Item("oggetto")
                intContabile = Context.Items.Item("intOpContabili")

                pubIntegrale = Context.Items.Item("pubIntegrale")
                'comm per eliminazione xml
                'oggetto = oggetto.Replace("&amp;", Chr(38))
                'oggetto = oggetto.Replace("&aps;", Chr(39))
                'oggetto = oggetto.Replace("&gt;", Chr(62))
                'oggetto = oggetto.Replace("&lt;", Chr(60))
                'oggetto = oggetto.Replace("&quot;", Chr(34))


                Dim tblDetermina As New Web.UI.WebControls.Table
                Dim tblRigaDetermina As Web.UI.WebControls.TableRow
                Dim tblCellaDetermina As Web.UI.WebControls.TableCell

                If strMessaggio <> "" Then
                    tblRigaDetermina = New Web.UI.WebControls.TableRow
                    tblCellaDetermina = New Web.UI.WebControls.TableCell
                    titoloCelle = New Web.UI.WebControls.Literal
                    titoloCelle.Text = strMessaggio
                    tblCellaDetermina.Style.Add("font-size", "80%")
                    tblCellaDetermina.Controls.Add(titoloCelle)
                    tblRigaDetermina.Controls.Add(tblCellaDetermina)
                    tblDetermina.Rows.Add(tblRigaDetermina)

                    tblRigaDetermina = New Web.UI.WebControls.TableRow
                    tblCellaDetermina = New Web.UI.WebControls.TableCell

                    tblCellaDetermina.Style.Add("font-size", "80%")

                    titoloCelle = New Web.UI.WebControls.Literal
                    titoloCelle.Text = ""
                    tblCellaDetermina.Controls.Add(titoloCelle)
                    tblRigaDetermina.Controls.Add(tblCellaDetermina)
                    tblDetermina.Rows.Add(tblRigaDetermina)


                End If

                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                titoloCelle = New Web.UI.WebControls.Literal
                titoloCelle.Text = "Oggetto"
                tblCellaDetermina.Controls.Add(titoloCelle)
                tblRigaDetermina.Controls.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)

                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                Dim txtOggetto As New Web.UI.WebControls.TextBox
                txtOggetto.TextMode = TextBoxMode.MultiLine
                txtOggetto.ID = "txtOggetto"
                txtOggetto.Rows = 5
                txtOggetto.Columns = 65
                txtOggetto.Text = oggetto
                txtOggetto.ID = "txtOggetto"
                ceckOggetto.ID = "txtCeckOggetto"
                ceckOggetto.ControlToValidate = "txtOggetto"
                ceckOggetto.ErrorMessage = "Campo Oggetto Obbligatorio"
                ceckOggetto.CssClass = "lblErrore"
                tblCellaDetermina.Controls.Add(txtOggetto)
                tblRigaDetermina.Controls.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)





                ''LU      If TipoApplic() = 0 Then 'Creo la checkbox solo per Determina
                ''Dim panL As New Panel
                'Dim lbl As New Label
                'lbl.Text = "Comporta operazione contabile"
                ''panL.Controls.Add(lbl)
                ''Dim opSoggettoPor As Boolean = CBool(context.Request.Params.Item("opSoggettoPor"))
                'Dim radioContabili As New RadioButtonList
                'radioContabili.ID = "opContabili"
                'radioContabili.Items.Add(New ListItem("  Si", 1))
                'radioContabili.Items.Add(New ListItem("  No", 0))
                'radioContabili.Attributes.Add("border", "0")
                'radioContabili.TextAlign = TextAlign.Left
                'radioContabili.BorderStyle = BorderStyle.None
                'radioContabili.ControlStyle.BorderStyle = BorderStyle.None
                'radioContabili.RepeatColumns = 3
                'radioContabili.RepeatDirection = RepeatDirection.Horizontal
                'radioContabili.SelectedValue = intContabile
                'Dim rfv As New RequiredFieldValidator
                'rfv.ErrorMessage = "Selezionare Tipologia Operazione"
                'rfv.ControlToValidate = "opContabili"
                'tblCellaDetermina = New Web.UI.WebControls.TableCell
                'tblCellaDetermina.ColumnSpan = 3

                'tblCellaDetermina.Controls.Add(lbl)
                'tblCellaDetermina.Controls.Add(radioContabili)
                'tblCellaDetermina.Controls.Add(rfv)

                'tblRigaDetermina = New Web.UI.WebControls.TableRow
                'tblRigaDetermina.Cells.Add(tblCellaDetermina)
                'tblDetermina.Rows.Add(tblRigaDetermina)



                Dim lblPub As New Label
                lblPub.Text = "Tipo Pubblicazione"
                'panL.Controls.Add(lbl)
                'Dim opSoggettoPor As Boolean = CBool(context.Request.Params.Item("opSoggettoPor"))
                Dim radioPubbl As New RadioButtonList
                radioPubbl.ID = "tipoPubblic"
                'Tipo Pubblicazione
                radioPubbl.Attributes.Add("border", "0")
                radioPubbl.Items.Add(New ListItem("  Integrale", 0))
                radioPubbl.Items.Add(New ListItem("  Per Estratto Ogg+Disp", 1))
                radioPubbl.Items.Add(New ListItem("  Per Estratto Ogg", 2))
                radioPubbl.SelectedValue = pubIntegrale
                radioPubbl.Items(0).Attributes.Add("border", "0")
                radioPubbl.TextAlign = TextAlign.Left
                radioPubbl.BorderStyle = BorderStyle.None
                radioPubbl.ControlStyle.BorderStyle = BorderStyle.None
                radioPubbl.RepeatColumns = 4
                radioPubbl.RepeatDirection = RepeatDirection.Horizontal
                Dim rfvPub As New RequiredFieldValidator
                rfvPub.ErrorMessage = "Selezionare Tipologia Pubblicazione"
                rfvPub.ControlToValidate = "tipoPubblic"
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                tblCellaDetermina.ColumnSpan = 3

                tblCellaDetermina.Controls.Add(lblPub)
                tblCellaDetermina.Controls.Add(radioPubbl)
                tblCellaDetermina.Controls.Add(rfvPub)

                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblRigaDetermina.Cells.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)

                'LU End If



                'comm per eliminazione xml
                'sXmlDatiLetti = sXmlDatiLetti.Replace("&amp;", Chr(38))
                'sXmlDatiLetti = sXmlDatiLetti.Replace("&aps;", Chr(39))
                'sXmlDatiLetti = sXmlDatiLetti.Replace("&gt;", Chr(62))
                'sXmlDatiLetti = sXmlDatiLetti.Replace("&lt;", Chr(60))
                'sXmlDatiLetti = sXmlDatiLetti.Replace("&quot;", Chr(34))
                'If Trim(sXmlDatiLetti) <> "" Then
                '    xmlDatiLetti.LoadXml(sXmlDatiLetti)
                'End If

                ''ciclo sulle tabelle lette
                'precNomeTabella = ""
                'For Each nodoTemplateTabella In xmlDatiLetti.SelectNodes("/datiDocumento/tabella")
                '    nomeTabella = ""
                '    'individuo la tabella
                '    For Each attributo In nodoTemplateTabella.Attributes
                '        Select Case UCase(attributo.Name)
                '            Case "NOME_TABELLA"
                '                nomeTabella = attributo.Value
                '            Case "COL_PROG_REGISTRAZIONE"
                '                colonnaProgRegistrazioneTabella = attributo.Value
                '        End Select
                '    Next

                '    If precNomeTabella <> nomeTabella Then
                '        tblRigaDetermina = New Web.UI.WebControls.TableRow
                '        tblCellaDetermina = New Web.UI.WebControls.TableCell

                '        TabellaDettaglio = New Web.UI.WebControls.Table
                '        TabellaDettaglio.BorderWidth = WebControls.Unit.Pixel(1)
                '        TabellaDettaglio.BorderStyle = BorderStyle.Solid
                '        TabellaDettaglio.Width = WebControls.Unit.Percentage(100)
                '        TabellaDettaglio.Style.Add("display", "none")

                '        'preparo l'intestazione per la tabella
                '        Select Case UCase(nomeTabella)
                '            Case "DOCUMENTO_BILANCIO"
                '                RigaDettaglio = New Web.UI.WebControls.TableRow
                '                CellaDettaglio = New Web.UI.WebControls.TableCell
                '                CellaDettaglio.ColumnSpan = 4
                '                titoloCelle = New Web.UI.WebControls.Literal
                '                titoloCelle.Text = "Impegni contabili"
                '                CellaDettaglio.Controls.Add(titoloCelle)
                '                CellaDettaglio.HorizontalAlign = HorizontalAlign.Center
                '                RigaDettaglio.Controls.Add(CellaDettaglio)
                '                TabellaDettaglio.Rows.Add(RigaDettaglio)
                '            Case "DOCUMENTO_LIQUIDAZIONE"
                '                RigaDettaglio = New Web.UI.WebControls.TableRow
                '                CellaDettaglio = New Web.UI.WebControls.TableCell
                '                CellaDettaglio.ColumnSpan = 4
                '                titoloCelle = New Web.UI.WebControls.Literal
                '                titoloCelle.Text = "Liquidazioni"
                '                CellaDettaglio.Controls.Add(titoloCelle)
                '                CellaDettaglio.HorizontalAlign = HorizontalAlign.Center
                '                RigaDettaglio.Controls.Add(CellaDettaglio)
                '                TabellaDettaglio.Rows.Add(RigaDettaglio)
                '            Case "DOCUMENTO_RAG_ASSUNZIONE"
                '                RigaDettaglio = New Web.UI.WebControls.TableRow
                '                CellaDettaglio = New Web.UI.WebControls.TableCell
                '                CellaDettaglio.ColumnSpan = 4
                '                titoloCelle = New Web.UI.WebControls.Literal
                '                titoloCelle.Text = "Impegni contabili assunti"
                '                CellaDettaglio.Controls.Add(titoloCelle)
                '                CellaDettaglio.HorizontalAlign = HorizontalAlign.Center
                '                RigaDettaglio.Controls.Add(CellaDettaglio)
                '                TabellaDettaglio.Rows.Add(RigaDettaglio)
                '            Case "DOCUMENTO_RAG_LIQUIDAZIONE"
                '                RigaDettaglio = New Web.UI.WebControls.TableRow
                '                CellaDettaglio = New Web.UI.WebControls.TableCell
                '                CellaDettaglio.ColumnSpan = 4
                '                titoloCelle = New Web.UI.WebControls.Literal
                '                titoloCelle.Text = "Liquidazioni assunte"
                '                CellaDettaglio.Controls.Add(titoloCelle)
                '                CellaDettaglio.HorizontalAlign = HorizontalAlign.Center
                '                RigaDettaglio.Controls.Add(CellaDettaglio)
                '                TabellaDettaglio.Rows.Add(RigaDettaglio)
                '        End Select

                '        'ciclo sulle righe lette
                '        i = 0
                '        For Each nodoTemplateTabellaRiga In xmlDatiLetti.SelectNodes("/datiDocumento/tabella[@nome_tabella='" & nomeTabella & "']")
                '            i += 1
                '            Select Case UCase(nomeTabella)
                '                Case "DOCUMENTO_BILANCIO"
                '                    Call caricaRiga_Documento_bilancio(nodoTemplateTabellaRiga, TabellaDettaglio, i)
                '                Case "DOCUMENTO_LIQUIDAZIONE"
                '                    Call caricaRiga_Documento_liquidazione(nodoTemplateTabellaRiga, TabellaDettaglio, i)
                '                Case "DOCUMENTO_RAG_ASSUNZIONE"
                '                    Call caricaRiga_Documento_rag_assunzione(nodoTemplateTabellaRiga, TabellaDettaglio, i)
                '                Case "DOCUMENTO_RAG_LIQUIDAZIONE"
                '                    Call caricaRiga_Documento_rag_liquidazione(nodoTemplateTabellaRiga, TabellaDettaglio, i)
                '            End Select
                '        Next
                '        Session.Add("numRighe" + nomeTabella, CStr(i))

                '        'metto la riga vuota
                '        Select Case UCase(nomeTabella)
                '            Case "DOCUMENTO_BILANCIO"
                '                Call caricaRiga_Documento_bilancio(nodoTemplateTabellaRiga, TabellaDettaglio, 0)
                '            Case "DOCUMENTO_LIQUIDAZIONE"
                '                Call caricaRiga_Documento_liquidazione(nodoTemplateTabellaRiga, TabellaDettaglio, 0)
                '            Case "DOCUMENTO_RAG_ASSUNZIONE"
                '                Call caricaRiga_Documento_rag_assunzione(nodoTemplateTabellaRiga, TabellaDettaglio, 0)
                '            Case "DOCUMENTO_RAG_LIQUIDAZIONE"
                '                Call caricaRiga_Documento_rag_liquidazione(nodoTemplateTabellaRiga, TabellaDettaglio, 0)
                '        End Select
                '        tblCellaDetermina.Controls.Add(TabellaDettaglio)
                '        tblRigaDetermina.Controls.Add(tblCellaDetermina)
                '        tblDetermina.Rows.Add(tblRigaDetermina)
                '    End If

                '    precNomeTabella = nomeTabella


                'Next


                PannelloTipoContabile.Visible = True

                Dim lbl As New Label
                lbl.Text = "Quale operazione contabile comporta?"
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                tblCellaDetermina.ColumnSpan = 3
                tblCellaDetermina.Controls.Add(lbl)
                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblRigaDetermina.Cells.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)

                chkNessuna.ID = "chkNessuna"
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                tblCellaDetermina.ColumnSpan = 3



                tblCellaDetermina.Controls.Add(PannelloTipoContabile)

                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblRigaDetermina.Cells.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)

                If verificaOpContabilInserite() Then

                    chkImpegno.Enabled = False
                    chkImpegnoSuPerenti.Enabled = False
                    chkAccertamento.Enabled = False
                    chkLiquidazione.Enabled = False
                    chkRiduzione.Enabled = False
                    chkNessuna.Enabled = False


                End If

                tblRigaDetermina = New Web.UI.WebControls.TableRow
                tblCellaDetermina = New Web.UI.WebControls.TableCell
                tblCellaDetermina.HorizontalAlign = HorizontalAlign.Center
                tblCellaDetermina.Controls.Add(ButtonContinua)
                tblRigaDetermina.Controls.Add(tblCellaDetermina)
                tblDetermina.Rows.Add(tblRigaDetermina)

                Contenuto.Controls.Add(tblDetermina)

                Call Pulisci_Sessione()

            End If
            gestionePulsanti()
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub
    Private Function verificaOpContabilInserite() As Boolean
        Dim key As String = Context.Request.QueryString.Get("key")
        Dim dllDoc As New DllDocumentale.svrDocumenti(Context.Session.Item("oOperatore"))
        Dim listaImp As List(Of ItemImpegnoInfo)

        listaImp = dllDoc.FO_Get_DatiImpegni(key)
        Dim listaLiq As List(Of ItemLiquidazioneInfo)
        listaLiq = dllDoc.FO_Get_DatiLiquidazione(key)
        Dim listaAcc As List(Of ItemAssunzioneContabileInfo)
        listaAcc = dllDoc.FO_Get_Dati_Assunzione(key)

        If listaImp.Count > 0 Or listaLiq.Count > 0 Or listaAcc.Count > 0 Then
            Return True
        Else
            Return False
        End If


    End Function
    Private Sub Pulisci_Sessione()

    End Sub

    'Private Sub caricaRiga_Documento_bilancio(ByRef nodo As System.Xml.XmlNode, ByRef tabella As Web.UI.WebControls.Table, ByVal numRiga As Integer)
    '    Dim riga As Web.UI.WebControls.TableRow
    '    Dim cella As Web.UI.WebControls.TableCell
    '    Dim InputText As Web.UI.WebControls.TextBox
    '    Dim nodoColonna As System.Xml.XmlNode
    '    Dim attributo As System.Xml.XmlAttribute
    '    Dim visualizzare As Boolean
    '    Dim etic As Web.UI.WebControls.Label

    '    riga = New Web.UI.WebControls.TableRow
    '    riga.CssClass = "determina"
    '    For Each nodoColonna In nodo.ChildNodes
    '        InputText = New Web.UI.WebControls.TextBox
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        InputText.CssClass = "sololinea"
    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DBI_BILANCIO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "Bilancio "
    '            Case "DBI_UPB"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "UPB "
    '            Case "DBI_CAP"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "Cap. "
    '            Case "DBI_COSTO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(70)
    '                etic.Text = "per € "
    '            Case Else
    '                visualizzare = False
    '        End Select

    '        cella.Controls.Add(etic)
    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                InputText.Text = nodoColonna.InnerText
    '            End If
    '            For Each attributo In nodoColonna.Attributes
    '                Select Case UCase(attributo.Name)
    '                    Case "ERRORETIPODATO"
    '                        InputText.ForeColor = System.Drawing.Color.Red
    '                        InputText.ToolTip = attributo.Value
    '                    Case "ERRORECOMPITO"
    '                        InputText.ForeColor = System.Drawing.Color.Red
    '                        InputText.ToolTip = attributo.Value
    '                    Case "DISABILITA"
    '                        InputText.Enabled = True
    '                End Select
    '            Next
    '            cella.Controls.Add(InputText)
    '            riga.Controls.Add(cella)
    '        End If
    '    Next
    '    tabella.Rows.Add(riga)
    'End Sub

    'Private Sub caricaRiga_Documento_liquidazione(ByRef nodo As System.Xml.XmlNode, ByRef tabella As Web.UI.WebControls.Table, ByVal numRiga As Integer)
    '    Dim riga As Web.UI.WebControls.TableRow
    '    Dim cella As Web.UI.WebControls.TableCell
    '    Dim InputText As Web.UI.WebControls.TextBox
    '    Dim InputChk As Web.UI.WebControls.DropDownList
    '    Dim nodoColonna As System.Xml.XmlNode
    '    Dim attributo As System.Xml.XmlAttribute
    '    Dim visualizzare As Boolean
    '    Dim etic As Web.UI.WebControls.Label

    '    riga = New Web.UI.WebControls.TableRow
    '    riga.CssClass = "determina"
    '    For Each nodoColonna In nodo.ChildNodes
    '        InputText = New Web.UI.WebControls.TextBox
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        InputText.CssClass = "sololinea"
    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DLI_COSTO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(70)
    '                etic.Text = "- di €"
    '            Case "DLI_CAP"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "sul Cap."
    '            Case "DLI_UPB"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "UPB"
    '            Case "DLI_ESERCIZIO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "Esercizio"
    '            Case Else
    '                visualizzare = False
    '        End Select
    '        cella.Controls.Add(etic)

    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                InputText.Text = nodoColonna.InnerText
    '                For Each attributo In nodoColonna.Attributes
    '                    Select Case UCase(attributo.Name)
    '                        Case "ERRORETIPODATO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "ERRORECOMPITO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "DISABILITA"
    '                            InputText.Enabled = False
    '                    End Select
    '                Next
    '            End If


    '            cella.Controls.Add(InputText)
    '            riga.Controls.Add(cella)
    '        End If
    '    Next
    '    tabella.Rows.Add(riga)

    '    riga = New Web.UI.WebControls.TableRow
    '    For Each nodoColonna In nodo.ChildNodes
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        If UCase(nodoColonna.Name) = "DLI_TIPOASSUNZIONE" Then
    '            InputChk = New Web.UI.WebControls.DropDownList
    '            InputChk.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        Else
    '            InputText = New Web.UI.WebControls.TextBox
    '            InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '            InputText.CssClass = "sololinea"
    '        End If


    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DLI_NCONTABILE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Impegno contabile"
    '            Case "DLI_TIPOASSUNZIONE"
    '                etic.Text = " assunto con "
    '                Dim item1 As ListItem = New ListItem
    '                Dim item2 As ListItem = New ListItem
    '                item1.Text() = "Determina"
    '                item1.Value = "0"
    '                'modgg 4-10
    '                item2.Text() = "Deliberazione"
    '                item2.Value = "1"
    '                InputChk.Items.Add(item1)
    '                InputChk.Items.Add(item2)
    '                cella.ColumnSpan = 2
    '            Case "DLI_NUM_ASSUNZIONE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "N "
    '            Case "DLI_DATA_ASSUNZIONE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(70)
    '                etic.Text = "del"
    '            Case Else
    '                visualizzare = False
    '        End Select
    '        cella.Controls.Add(etic)

    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                If UCase(nodoColonna.Name) = "DLI_TIPOASSUNZIONE" Then
    '                    If IsNumeric(nodoColonna.InnerText) Then
    '                        If CInt(nodoColonna.InnerText) <= InputChk.Items.Count Then
    '                            InputChk.SelectedIndex = CInt(nodoColonna.InnerText)
    '                        End If
    '                    End If
    '                Else
    '                    InputText.Text = nodoColonna.InnerText
    '                    For Each attributo In nodoColonna.Attributes
    '                        Select Case UCase(attributo.Name)
    '                            Case "ERRORETIPODATO"
    '                                InputText.ForeColor = System.Drawing.Color.Red
    '                                InputText.ToolTip = attributo.Value
    '                            Case "ERRORECOMPITO"
    '                                InputText.ForeColor = System.Drawing.Color.Red
    '                                InputText.ToolTip = attributo.Value
    '                            Case "DISABILITA"
    '                                InputText.Enabled = False
    '                        End Select
    '                    Next
    '                End If
    '            End If
    '            If UCase(nodoColonna.Name) = "DLI_TIPOASSUNZIONE" Then
    '                cella.Controls.Add(InputChk)
    '            Else
    '                cella.Controls.Add(InputText)
    '            End If

    '            riga.Controls.Add(cella)
    '        End If
    '    Next
    '    tabella.Rows.Add(riga)
    'End Sub

    'Private Sub caricaRiga_Documento_rag_assunzione(ByRef nodo As System.Xml.XmlNode, ByRef tabella As Web.UI.WebControls.Table, ByVal numRiga As Integer)
    '    Dim riga As Web.UI.WebControls.TableRow
    '    Dim cella As Web.UI.WebControls.TableCell
    '    Dim InputText As Web.UI.WebControls.TextBox
    '    Dim nodoColonna As System.Xml.XmlNode
    '    Dim attributo As System.Xml.XmlAttribute
    '    Dim visualizzare As Boolean
    '    Dim etic As Web.UI.WebControls.Label


    '    riga = New Web.UI.WebControls.TableRow
    '    riga.CssClass = "determina"
    '    For Each nodoColonna In nodo.ChildNodes
    '        InputText = New Web.UI.WebControls.TextBox
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        InputText.CssClass = "sololinea"
    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DRA_NCONTABILE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Assunto impegno cont."
    '            Case "DRA_UPB"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "UPB"
    '            Case "DRA_CAP"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Cap."
    '            Case "DRA_ESERCIZIO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Esercizio"
    '            Case "DRA_COSTO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "per €"
    '            Case Else
    '                visualizzare = False
    '        End Select
    '        cella.Controls.Add(etic)

    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                InputText.Text = nodoColonna.InnerText
    '                For Each attributo In nodoColonna.Attributes
    '                    Select Case UCase(attributo.Name)
    '                        Case "ERRORETIPODATO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "ERRORECOMPITO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "DISABILITA"
    '                            InputText.Enabled = False
    '                    End Select
    '                Next

    '            End If

    '            cella.Controls.Add(InputText)
    '            riga.Controls.Add(cella)
    '        End If
    '    Next

    '    tabella.Rows.Add(riga)
    'End Sub

    'Private Sub caricaRiga_Documento_rag_liquidazione(ByRef nodo As System.Xml.XmlNode, ByRef tabella As Web.UI.WebControls.Table, ByVal numRiga As Integer)
    '    Dim riga As Web.UI.WebControls.TableRow
    '    Dim cella As Web.UI.WebControls.TableCell
    '    Dim InputText As Web.UI.WebControls.TextBox
    '    Dim InputChk As Web.UI.WebControls.DropDownList
    '    Dim nodoColonna As System.Xml.XmlNode
    '    Dim attributo As System.Xml.XmlAttribute
    '    Dim visualizzare As Boolean
    '    Dim etic As Web.UI.WebControls.Label

    '    riga = New Web.UI.WebControls.TableRow
    '    riga.CssClass = "determina"
    '    For Each nodoColonna In nodo.ChildNodes
    '        InputText = New Web.UI.WebControls.TextBox
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        InputText.CssClass = "sololinea"
    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DRL_NLIQUIDAZIONE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = " - Liquidazione n°"
    '            Case "DRL_UPB"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "UPB"
    '            Case "DRL_CAP"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Cap."
    '            Case "DRL_ESERCIZIO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Esercizio"
    '            Case "DRL_COSTO"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(60)
    '                etic.Text = "per €"
    '                cella.ColumnSpan = 2
    '            Case Else
    '                visualizzare = False
    '        End Select
    '        cella.Controls.Add(etic)

    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                InputText.Text = nodoColonna.InnerText
    '                For Each attributo In nodoColonna.Attributes
    '                    Select Case UCase(attributo.Name)
    '                        Case "ERRORETIPODATO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "ERRORECOMPITO"
    '                            InputText.ForeColor = System.Drawing.Color.Red
    '                            InputText.ToolTip = attributo.Value
    '                        Case "DISABILITA"
    '                            InputText.Enabled = False
    '                    End Select
    '                Next
    '            End If


    '            cella.Controls.Add(InputText)
    '            riga.Controls.Add(cella)
    '        End If
    '    Next
    '    tabella.Rows.Add(riga)

    '    riga = New Web.UI.WebControls.TableRow
    '    For Each nodoColonna In nodo.ChildNodes
    '        InputText = New Web.UI.WebControls.TextBox
    '        cella = New Web.UI.WebControls.TableCell
    '        cella.CssClass = "determina"
    '        visualizzare = True
    '        If UCase(nodoColonna.Name) = "DRL_TIPOASSUNZIONE" Then
    '            InputChk = New Web.UI.WebControls.DropDownList
    '            InputChk.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '        Else
    '            InputText = New Web.UI.WebControls.TextBox
    '            InputText.ID = "txt_" + nodoColonna.Name + CStr(numRiga)
    '            InputText.CssClass = "sololinea"
    '        End If
    '        etic = New Web.UI.WebControls.Label
    '        etic.CssClass = "determina"
    '        Select Case UCase(nodoColonna.Name)
    '            Case "DRL_NCONTABILE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "Impegno cont. n°"
    '            Case "DRL_TIPOASSUNZIONE"
    '                etic.Text = " assunto con "
    '                Dim item1 As ListItem = New ListItem
    '                Dim item2 As ListItem = New ListItem
    '                item1.Text() = "Determina"
    '                item1.Value = "0"
    '                'modgg 4-10
    '                item2.Text() = "Deliberazione"
    '                item2.Value = "1"
    '                InputChk.Items.Add(item1)
    '                InputChk.Items.Add(item2)
    '                cella.ColumnSpan = 2
    '            Case "DRL_NASSUNZIONE"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(50)
    '                etic.Text = "N°"
    '            Case "DRL_DATA"
    '                InputText.Width = Web.UI.WebControls.Unit.Pixel(70)
    '                etic.Text = "del"
    '            Case Else
    '                visualizzare = False
    '        End Select
    '        cella.Controls.Add(etic)

    '        If visualizzare Then
    '            If numRiga > 0 Then
    '                If UCase(nodoColonna.Name) = "DRL_TIPOASSUNZIONE" Then
    '                    If IsNumeric(nodoColonna.InnerText) Then
    '                        If CInt(nodoColonna.InnerText) <= InputChk.Items.Count Then
    '                            InputChk.SelectedIndex = CInt(nodoColonna.InnerText)
    '                        End If
    '                    End If
    '                Else
    '                    InputText.Text = nodoColonna.InnerText
    '                    For Each attributo In nodoColonna.Attributes
    '                        Select Case UCase(attributo.Name)
    '                            Case "ERRORETIPODATO"
    '                                InputText.ForeColor = System.Drawing.Color.Red
    '                                InputText.ToolTip = attributo.Value
    '                            Case "ERRORECOMPITO"
    '                                InputText.ForeColor = System.Drawing.Color.Red
    '                                InputText.ToolTip = attributo.Value
    '                            Case "DISABILITA"
    '                                InputText.Enabled = False
    '                        End Select
    '                    Next
    '                End If
    '            End If
    '            If UCase(nodoColonna.Name) = "DRL_TIPOASSUNZIONE" Then
    '                cella.Controls.Add(InputChk)
    '            Else
    '                cella.Controls.Add(InputText)
    '            End If

    '            riga.Controls.Add(cella)
    '        End If
    '    Next
    '    tabella.Rows.Add(riga)
    'End Sub

    Private Sub ButtonContinua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonContinua.Click
        Dim xmlTemplate As New System.Xml.XmlDocument
        Dim xmlDatiDaRegistrare As New System.Xml.XmlDocument
        Dim sXmlDatiDaRegistrare As String
       
        Dim fileXmlTemplate As String = ConfigurationManager.AppSettings("templateDatiRegistraDetermina")

        xmlTemplate.Load(AppDomain.CurrentDomain.BaseDirectory + fileXmlTemplate)

        'sXmlDatiDaRegistrare = ""
        'For Each nodoTemplateTabella In xmlTemplate.SelectNodes("/datiDocumento/tabella[@nome_tabella!='Documento_noteosservazioni']")
        '    nomeTabella = ""
        '    For Each attributo In nodoTemplateTabella.Attributes
        '        Select Case UCase(attributo.Name)
        '            Case "NOME_TABELLA"
        '                nomeTabella = attributo.Value
        '            Case "COL_PROG_REGISTRAZIONE"
        '                idProgRiga = attributo.Value
        '        End Select
        '    Next
        '    inserisciRiga = False
        '    For i = 0 To CInt(Session.Item("numRighe" + nomeTabella))
        '        For Each nodoTemplateColonna In nodoTemplateTabella.ChildNodes
        '            oInput = Context.Request.Form.Get("txt_" + nodoTemplateColonna.Name + CStr(i)) & ""
        '            If Trim(oInput <> "") Then
        '                nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode("//" & nodoTemplateColonna.Name)
        '                If Not nodoTemplateColonna Is Nothing Then
        '                    nodoTemplateColonna.InnerText = oInput
        '                    If Not (UCase(nodoTemplateColonna.Name) = "DLI_TIPOASSUNZIONE") And Not (UCase(nodoTemplateColonna.Name) = "DRL_TIPOASSUNZIONE") Then
        '                        inserisciRiga = True
        '                    End If
        '                End If
        '            End If
        '        Next
        '        nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode("//" & idProgRiga)
        '        If nodoTemplateColonna Is Nothing Then
        '            inserisciRiga = False
        '        Else
        '            nodoTemplateColonna.InnerText = CStr(i)
        '        End If
        '        If inserisciRiga Or i = 1 Then
        '            sXmlDatiDaRegistrare = sXmlDatiDaRegistrare + nodoTemplateTabella.OuterXml
        '        End If
        '    Next
        'Next

        xmlDatiDaRegistrare.LoadXml("<datiDocumento>" & sXmlDatiDaRegistrare & "</datiDocumento>")

        Dim oggetto As String = Context.Request.Form.Get("txtOggetto")
        'comm per eliminazione xml
        'oggetto = oggetto.Replace(Chr(38), "&amp;")
        'oggetto = oggetto.Replace(Chr(39), "&aps;")
        'oggetto = oggetto.Replace(Chr(62), "&gt;")
        'oggetto = oggetto.Replace(Chr(60), "&lt;")
        'oggetto = oggetto.Replace(Chr(34), "&quot;")

        Dim stringaOpConta As String = ""

        If verificaOpContabilInserite() Then
            Dim key As String = Context.Request.QueryString.Get("key")
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

            chkImpegno.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
            chkImpegnoSuPerenti.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
            chkAccertamento.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
            chkLiquidazione.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
            chkRiduzione.Checked = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)
            If Not objDocumento.haveOpContabile Then
                chkNessuna.Checked = True
            End If

        End If
        
        stringaOpConta = creaStringaOpContabili()


        Dim intOpContabili As Integer = 0

        Dim lstr_StringTempOpContabili As String = Trim(stringaOpConta.Replace("0", "").Replace(";", ""))
        'elminino tutti gli 0 e i punti e vigorla cosi facendo mi rimane il valore delle operazioni selezionate
        'non potrò + fare il controllo se contiene 1 visto che per sistemazioni contabili potrà avere altri valori
        'If stringaOpConta.Contains("1") Then
        If Not String.IsNullOrEmpty(lstr_StringTempOpContabili) Then
            intOpContabili = 1
        Else
            intOpContabili = 0
        End If
        Context.Session.Add("tipoOpContabili", stringaOpConta)



        Dim tipoPubblic As Integer = 0

        tipoPubblic = Context.Request.Form("tipoPubblic")

        'context.Session.Add("intOpContabili", intOpContabili)
        Context.Session.Add("intOpContabili", intOpContabili)
        Context.Session.Add("pubIntegrale", tipoPubblic)

        Session.Add("datiXml", xmlDatiDaRegistrare.OuterXml)
        Session.Add("oggetto", oggetto & "")


        If Trim(Context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", Context.Request.QueryString.Get("key"))
        End If

        Response.Redirect("RegistraDeterminaAction.aspx?key=" & Context.Request.QueryString.Get("key"))

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
    Public Sub New()

    End Sub
    Private Sub gestionePulsanti()
        Dim obj As Object = Leggi_Documento(Context.Request.QueryString.Get("key"))
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        If obj(0) = 0 Then
            If obj(2) <> oOperatore.oUfficio.CodUfficio Then
                ButtonContinua.Enabled = False
            End If
        End If

    End Sub
End Class
