Imports eWorld.UI
Imports System.ServiceModel
Imports DllDocumentale
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class DatiSeduta
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    
    Protected WithEvents btnSalvaDatiTrasparenza As System.Web.UI.WebControls.Button


    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Testo")
    End Sub

#End Region
    Private _numeroDocumento As String = ""
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(DatiSeduta))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        codDocumento.Value = Context.Request.QueryString.Item("key")
        If codDocumento.Value Is Nothing Or codDocumento.Value = "" Then
            codDocumento.Value = Context.Session.Item("codDocumento")
        End If
        Context.Session.Add("codDocumento", codDocumento.Value)

        Try
            If Not IsPostBack Then
                Dim vR As Object = Nothing
                Dim codiceUfficio As String = ""
                If (Context.Items.Item("CodUffProp") Is Nothing) Then
                    vR = Leggi_Documento(codDocumento.Value)
                    If (Not vR Is Nothing) Then
                        codiceUfficio = vR(2)
                    End If
                Else
                    codiceUfficio = Context.Items.Item("CodUffProp")
                End If


                'Qui va testato se deve essere uploadato il file oppure no
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(codDocumento.Value)
                Dim livelloUfficio As String = statoIstanza.LivelloUfficio
                Dim ruolo As String = "" & statoIstanza.Ruolo
                If ruolo <> "A" AndAlso (oOperatore.oUfficio.CodUfficio = codiceUfficio And livelloUfficio = "UP") Or ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloUfficio = "UDD")) Or ((livelloUfficio = "RAG")) Or oOperatore.Test_Ruolo("CT001") Then
                    'btnSalvaDatiTrasparenza.Visible = True
                    'gestionePulsanti(livelloUfficio)
                Else
                    'pnlAggiungiAllegato.Visible = False
                    'btnSalvaDatiTrasparenza.Visible = False
                    'Label2.Text = "Impossibile modificare i dati sulla 'Trasparenza'. Autorizzazione negata"
                End If


                Dim itemDatiSedutaInfoDaDB As DllDocumentale.ItemDatiSedutaInfo = (New DllDocumentale.svrDocumenti(oOperatore)).FO_Get_DatiSedutaRelatoreInfo(codDocumento.Value)
                'If itemDatiSedutaInfoDaDB Is Nothing Then

                '    Dim listaRelatori As Collections.Generic.List(Of ItemRelatore) = (New DllDocumentale.svrDocumenti(oOperatore)).FO_Get_Lista_Relatori_Attivi()
                '    If Not listaRelatori Is Nothing Then
                '        itemDatiSedutaInfoDaDB = New DllDocumentale.ItemDatiSedutaInfo
                '        itemDatiSedutaInfoDaDB.DocId = codDocumento.Value
                '        itemDatiSedutaInfoDaDB.DataSeduta = Now.Date
                '        itemDatiSedutaInfoDaDB.OraSeduta = IIf(Now.Hour.ToString.Length = 1, "0" & Now.Hour, Now.Hour) & ":" & IIf(Now.Minute.ToString.Length = 1, "0" & Now.Minute, Now.Minute)

                '        itemDatiSedutaInfoDaDB.Relatori = listaRelatori
                '    End If
                'End If
                'itemDatiSedutaInfoDaDB.DataSeduta

                itemDatiSedutaInfo.Value = JavaScriptConvert.SerializeObject(itemDatiSedutaInfoDaDB)


                'Dim listaTipologieDocumento As Generic.List(Of DllDocumentale.ItemTipologiaDocumento) = ojbSvrdocumenti.FO_Get_ListaTipologiaDocumento()

                'Dim lItem As WebControls.ListItem
                'lItem = New WebControls.ListItem
                'lItem.Value = -1
                'lItem.Text = "- Selezionare una tipologia -"
                ''lItem.Selected = True

                'ddlTipologiaDocumento.Items.Add(lItem)
                'Dim tipologiaDocumentoSelezionata As String = Session.Item("tipologiaDocumento")

                'For Each tipologia As DllDocumentale.ItemTipologiaDocumento In listaTipologieDocumento
                '    lItem = New WebControls.ListItem
                '    lItem.Value = tipologia.Id
                '    lItem.Text = tipologia.Tipologia

                '    If ((Not tipologiaDocumentoSelezionata Is Nothing AndAlso tipologiaDocumentoSelezionata <> "" Or tipologiaDocumentoSelezionata <> "-1") And tipologiaDocumentoSelezionata = tipologia.Id) Or _
                '        (Not itemDocumentoDatiTrasparenzaDaDB Is Nothing AndAlso itemDocumentoDatiTrasparenzaDaDB.IdTipologiaProvvedimento = tipologia.Id) Then
                '        lItem.Selected = True
                '    End If

                '    ddlTipologiaDocumento.Items.Add(lItem)
                'Next


                'Dim isStraordinario As String = Session.Item("isStraordinario")
                'If (Not isStraordinario Is Nothing AndAlso isStraordinario = "on") Then
                '    checkIsStraordinario.Checked = True
                'Else
                '    checkIsStraordinario.Checked = False
                'End If

                'If (Not itemDocumentoDatiTrasparenzaDaDB Is Nothing AndAlso itemDocumentoDatiTrasparenzaDaDB.IsStraordinario) Then
                '    checkIsStraordinario.Checked = True
                'End If

                'Dim tabella As Web.UI.WebControls.Table
                'Dim riga As Web.UI.WebControls.TableRow
                'Dim cella As Web.UI.WebControls.TableCell
                'tabella = New Web.UI.WebControls.Table
                'tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                'tabella.HorizontalAlign = HorizontalAlign.Center
                'riga = New Web.UI.WebControls.TableRow
                'cella = New Web.UI.WebControls.TableCell
                'riga = New Web.UI.WebControls.TableRow
                'cella = New Web.UI.WebControls.TableCell
                'cella.ColumnSpan = 2
                'riga.Controls.Add(cella)
                'tabella.Rows.Add(riga)

                'Contenuto.Controls.Add(tabella)

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


        If Request.Form("chkSalva") & "" = "1" Then
            Try
                Dim str_return As String = RegistraDatiSeduta()
                Response.Write("{success:true,link:'" & str_return & "'}     ")
                Response.Flush()
                Response.Close()

            Catch ex As Exception
                Response.Clear()
                Response.ClearHeaders()

                Dim exf As New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)

                Response.Write("{success:false,FaultMessage:'" & exf.Message.Replace("'", "''") & "'} ")
                Response.Flush()
                Response.Close()
            End Try
        End If




        ''Inserire qui il codice utente necessario per inizializzare la pagina
        ''modgg 10-06 1
        'Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        'Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)
        ''Dim codDocumento As String = Context.Request.QueryString.Item("key")
        ''Context.Session.Add("key", codDocumento)

        'Try
        '    If Not IsPostBack Then
        '        Dim descrizioneDocumento As String = ""
        '        Dim numeroPubblicoDocumento As String = ""
        '        Select Case Context.Request.QueryString.Item("tipo")

        '            Case 0
        '                descrizioneDocumento = "Determina"
        '            Case 1
        '                descrizioneDocumento = "Delibera"
        '            Case 2
        '                descrizioneDocumento = "Disposizione"
        '            Case Else
        '                descrizioneDocumento = "Documento"
        '        End Select
        '        'Rinomina_Pagina(Me, descrizioneDocumento & " " & Context.Items.Item("numeroDoc"))
        '        'Context.Session.Remove("numeroPubblicoDocumento")
        '        'numeroPubblicoDocumento = Context.Items.Item("numeroDoc")
        '        'Context.Session.Add("numeroPubblicoDocumento", numeroPubblicoDocumento)

        '        'Label1.Text = HttpContext.Current.Session.Item("erroreFile")
        '        'HttpContext.Current.Session.Remove("erroreFile")


        '        'Contenuto.Controls.Add(Label1)
        '        'Contenuto.Controls.Add(Label2)

        '        'Dim tabellaDatiSeduta As New Table()
        '        'tabellaDatiSeduta.CellSpacing = 10
        '        'Dim rigaData As New TableRow()
        '        'Dim cellaLabelData As New TableCell()
        '        'Dim cellaCalendar As New TableCell()

        '        'cellaLabelData.Text = "Seduta del "
        '        'Dim calendarPopup As New CalendarPopup
        '        'calendarPopup.CalendarWidth = 300
        '        'calendarPopup.ImageUrl = "~/risorse/immagini/Calendar.gif"
        '        'calendarPopup.ControlDisplay = DisplayType.TextBoxImage
        '        'calendarPopup.DisableTextBoxEntry = "False"
        '        'cellaCalendar.Controls.Add(calendarPopup)

        '        'rigaData.Controls.Add(cellaLabelData)
        '        'rigaData.Controls.Add(cellaCalendar)

        '        'Dim rigaOra As New TableRow()
        '        'Dim cellaLabel As New TableCell()
        '        'Dim cellaOra As New TableCell()

        '        'cellaLabel.Text = "Alle ore: "
        '        'Dim tbOre As New TextBox
        '        'Dim tbMinuti As New TextBox
        '        'tbOre.Width = 23
        '        'tbOre.CssClass = "sololinea"
        '        'tbMinuti.Width = 23
        '        'tbMinuti.CssClass = "sololinea"

        '        'cellaOra.Controls.Add(tbOre)
        '        'Dim labelPunti As New Label
        '        'labelPunti.Text = " : "
        '        'cellaOra.Controls.Add(tbOre)
        '        'cellaOra.Controls.Add(labelPunti)
        '        'cellaOra.Controls.Add(tbMinuti)

        '        'rigaOra.Controls.Add(cellaLabel)
        '        'rigaOra.Controls.Add(cellaOra)

        '        'tabellaDatiSeduta.Controls.Add(rigaData)
        '        'tabellaDatiSeduta.Controls.Add(rigaOra)


        '        'Dim listaRelatori As Generic.List(Of DllDocumentale.ItemRelatore) = ojbSvrdocumenti.FO_Get_Lista_Relatori_Attivi()


        '        'Dim tabellaRelatori As New Table()

        '        ''tabellaRelatori.CellSpacing = 10

        '        'Dim rigaIntestazione As New TableRow()
        '        'Dim cIntestRelatore As New TableCell()
        '        ''Dim cIntestCarica As New TableHeaderCell()
        '        'Dim cIntestPresente As New TableCell()

        '        'Dim cIntestAssente As New TableCell()

        '        'cIntestRelatore.Text = "RELATORE"
        '        'cIntestRelatore.ColumnSpan = 2
        '        ''cIntestCarica.Text = ""
        '        'cIntestPresente.Text = "PRESENTE"
        '        'cIntestPresente.Width = 50

        '        'cIntestAssente.Text = "ASSENTE"
        '        'cIntestAssente.Width = 30
        '        ''rigaIntestazione.HorizontalAlign = HorizontalAlign.Center

        '        'rigaIntestazione.Controls.Add(cIntestRelatore)
        '        ''rigaIntestazione.Controls.Add(cIntestCarica)
        '        'rigaIntestazione.Controls.Add(cIntestPresente)
        '        'rigaIntestazione.Controls.Add(cIntestAssente)
        '        'tabellaRelatori.Controls.Add(rigaIntestazione)

        '        'For Each relatore As DllDocumentale.ItemRelatore In listaRelatori
        '        '    Dim rigaRelatore As New TableRow()

        '        '    'rigaRelatore.HorizontalAlign = HorizontalAlign.Left
        '        '    Dim cRelatore As New TableCell()
        '        '    Dim cCarica As New TableCell()
        '        '    Dim cPresente As New TableCell()
        '        '    Dim cAssente As New TableCell()

        '        '    cRelatore.Text = relatore.Nome & " " & relatore.Cognome.ToUpper
        '        '    cCarica.Text = relatore.Carica

        '        '    Dim radioButtonPresente As New RadioButton
        '        '    Dim radioButtonAssente As New RadioButton
        '        '    radioButtonPresente.GroupName = relatore.Id
        '        '    radioButtonAssente.GroupName = relatore.Id
        '        '    radioButtonPresente.Visible = True
        '        '    radioButtonAssente.Visible = True
        '        '    cPresente.HorizontalAlign = HorizontalAlign.Center
        '        '    cAssente.HorizontalAlign = HorizontalAlign.Center
        '        '    cPresente.Controls.Add(radioButtonPresente)
        '        '    cAssente.Controls.Add(radioButtonAssente)

        '        '    cPresente.HorizontalAlign = HorizontalAlign.Center
        '        '    cAssente.HorizontalAlign = HorizontalAlign.Center

        '        '    rigaRelatore.Cells.Add(cRelatore)
        '        '    rigaRelatore.Cells.Add(cCarica)
        '        '    rigaRelatore.Cells.Add(cPresente)
        '        '    rigaRelatore.Cells.Add(cAssente)


        '        '    tabellaRelatori.Rows.Add(rigaRelatore)



        '        'Next


        '        ''pnlSeduta.Controls.Add(tabellaDatiSeduta)
        '        'pnlRelatori.Controls.Add(tabellaRelatori)

        '        ''pnlSeduta.Controls.Add(tabellaRelatori)




        '        'Contenuto.Controls.Add(pnlSeduta)
        '    End If

        'Catch ex As Exception
        '    HttpContext.Current.Session.Add("error", ex.Message)
        '    Response.Redirect("MessaggioErrore.aspx")
        'End Try
    End Sub


    Protected Function RegistraDatiSeduta() As String
        Dim codDocumento As String = Request.QueryString("key")
        If codDocumento Is Nothing Then
            codDocumento = Context.Session.Item("codDocumento")
        End If

        Dim itemDatiSeduta As New ItemDatiSedutaInfo


        Dim dataSeduta As String = CStr(Context.Request.Form("DataSeduta"))
        Dim oraSeduta As String = CStr(Context.Request.Form("OraSeduta"))
        'Dim listaRelatori As Generic.List(Of DllDocumentale.ItemRelatore) = CStr(Context.Request.Form("listaRelatori"))

        itemDatiSeduta.DocId = codDocumento
        itemDatiSeduta.DataSeduta = dataSeduta
        itemDatiSeduta.OraSeduta = oraSeduta

        If Not Context.Request.Form("listaRelatori") Is Nothing Then
            Dim listaRelatori As String = htmlDecode(HttpContext.Current.Request.Item("listaRelatori"))
            listaRelatori = "[" & listaRelatori & "]"

            Dim relatori As List(Of Ext_RelatoreInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaRelatori, GetType(List(Of Ext_RelatoreInfo))), List(Of Ext_RelatoreInfo))
            For Each relatore As Ext_RelatoreInfo In relatori
                Dim itemRelatoreInfo As ItemRelatore = New ItemRelatore()
                itemRelatoreInfo.Id = relatore.Tr_id
                itemRelatoreInfo.IsPresente = relatore.IsPresente


                itemDatiSeduta.Relatori.Add(itemRelatoreInfo)
            Next
        End If




        Context.Session.Add("itemDatiSeduta", itemDatiSeduta)
        'RegistraDatiSedutaCommand
        Dim lstr_link As String = "RegistraDatiSedutaAction.aspx"
        'Session.Remove("key")

        'If Trim(Context.Request.QueryString.Get("key")) <> "" Then
        '    Session.Add("key", Context.Request.QueryString.Get("key"))
        '    Select Case tipoApp
        '        Case 0
        '            lstr_link = "CreaODGCommand.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '        Case 1
        '            lstr_link = "RegistraDeliberaAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '        Case 2
        '            lstr_link = "RegistraDisposizioneAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '    End Select



        'End If

        Return lstr_link
    End Function

    Private Function htmlDecode(ByVal value As Object)
        Dim retValue As Object = Nothing
        If Not value Is Nothing Then
            retValue = HttpUtility.HtmlDecode(value)
        End If
        Return retValue
    End Function


    Private Sub gestionePulsanti(ByVal livelloufficio As String)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


        'consentire il salvataggio solo nell'ufficio proponente
        If (oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And livelloufficio = "UP") Or ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloufficio = "UDD")) Or ((livelloufficio = "RAG")) Or oOperatore.Test_Ruolo("CT001") Then
            btnSalvaDatiTrasparenza.Enabled = True
        Else
            btnSalvaDatiTrasparenza.Enabled = False
        End If


        Dim modificaAbilitata As Boolean = VerificaAbilitazioneInoltroRigetto(oOperatore, Context.Request.QueryString.Get("key"), "INOLTRO")
        btnSalvaDatiTrasparenza.Enabled = modificaAbilitata

        'blocco caricamento documento di un atto creato da tool esterno

        Dim obj_doc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(Context.Request.QueryString.Get("key"))
        If Not String.IsNullOrEmpty(obj_doc.Doc_codApp) Then
            btnSalvaDatiTrasparenza.Enabled = False
        End If

    End Sub
    Private Sub btnSalvaDatiTrasparenza_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaDatiTrasparenza.Click
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(Context.Request.QueryString.Item("key"))

        Dim ruolo As String = "" & statoIstanza.Ruolo
        If ruolo <> "A" Then
            Dim tipologiaDocumento As String = ""
            If Not Request.Form.GetValues("ddlTipologiaDocumento") Is Nothing AndAlso Request.Form.GetValues("ddlTipologiaDocumento")(0) <> "" Then
                tipologiaDocumento = Request.Form.GetValues("ddlTipologiaDocumento")(0)
                Session.Add("tipologiaDocumento", tipologiaDocumento)
            End If
            Dim isStraordinario As String = ""
            If Not Request.Form.GetValues("checkIsStraordinario") Is Nothing AndAlso Request.Form.GetValues("checkIsStraordinario")(0) <> "" Then
                isStraordinario = Request.Form.GetValues("checkIsStraordinario")(0)
                Session.Add("isStraordinario", isStraordinario)
            Else
                Session.Add("isStraordinario", "off")
            End If

            HttpContext.Current.Session.Remove("erroreFile")
            If tipologiaDocumento <> "-1" Then
                'Response.Redirect("DatiTrasparenzaAction.aspx")
                Response.Redirect("VerificaSalvaDatiTrasparenza.aspx")
            Else
                HttpContext.Current.Session.Add("erroreFile", "E' necessario selezionare la tipologia del provvedimento")
                Response.Redirect(Request.UrlReferrer.AbsoluteUri)
            End If
        Else
            HttpContext.Current.Session.Add("erroreFile", "Il provvedimento risulta Annullato. ")
            Response.Redirect(Request.UrlReferrer.AbsoluteUri)
        End If
    End Sub

End Class
