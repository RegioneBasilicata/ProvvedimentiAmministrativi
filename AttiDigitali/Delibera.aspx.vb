Public Class Delibera
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents pnlDatiDelibera As System.Web.UI.WebControls.Panel
    Protected WithEvents TextBox10 As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnRegistra As System.Web.UI.WebControls.Button
    Protected WithEvents txtOggetto As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRP_NImpegno1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRP_UPB1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRP_Cap1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRP_Costo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRA_NContabile1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRA_UPB1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRA_Cap1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRA_Esercizio1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRA_Costo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_Costo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_UPB1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_Cap1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_Esercizio1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_NContabile1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_NAssunzione1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DRL_Data1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Dbi_Bilancio1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Dbi_UPB1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Dbi_Cap1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Dbi_Costo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlTipoPubblicazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents pnlPubblicazione As System.Web.UI.WebControls.Panel
    Protected WithEvents ReqTxtOggetto As System.Web.UI.WebControls.RequiredFieldValidator

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Delibera")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim xmlDatiLetti As New System.Xml.XmlDocument
        Dim sXmlDatiLetti As String
        Dim oggetto As String
        Dim pubIntegrale As Integer
        Dim nodoTemplateTabella As System.Xml.XmlNode
        Dim nodoTemplateTabellaRiga As System.Xml.XmlNode
        Dim nomeTabella As String
        Dim attributo As System.Xml.XmlAttribute

        If Not Page.IsPostBack Then
            Rinomina_Pagina(Me, " Delibera " & context.Items.Item("numeroDoc"))

            sXmlDatiLetti = context.Items.Item("xmlDati")
            oggetto = context.Items.Item("oggetto")
            If IsNumeric(context.Items.Item("pubIntegrale")) Then
                pubIntegrale = context.Items.Item("pubIntegrale")
            Else
                pubIntegrale = 0
            End If


            txtOggetto.Text = oggetto & ""
            Dim item1 As ListItem = New ListItem
            Dim item2 As ListItem = New ListItem
            item1.Text() = "Integrale"
            item1.Value = "0"
            item2.Text() = "Per estratto"
            item2.Value = "1"
            ddlTipoPubblicazione.Items.Add(item1)
            ddlTipoPubblicazione.Items.Add(item2)
            ddlTipoPubblicazione.SelectedIndex = pubIntegrale

            If Trim(sXmlDatiLetti) <> "" Then
                xmlDatiLetti.LoadXml(sXmlDatiLetti)
            End If
            For Each nodoTemplateTabella In xmlDatiLetti.SelectNodes("/datiDocumento/tabella")
                nomeTabella = ""
                'individuo la tabella
                For Each attributo In nodoTemplateTabella.Attributes
                    Select Case UCase(attributo.Name)
                        Case "NOME_TABELLA"
                            nomeTabella = attributo.Value
                    End Select
                Next

                For Each nodoTemplateTabellaRiga In xmlDatiLetti.SelectNodes("/datiDocumento/tabella[@nome_tabella='" & nomeTabella & "']")
                    Select Case UCase(nomeTabella)
                        Case "DOCUMENTO_BILANCIO"
                            Call caricaRiga_Documento_Bilancio(nodoTemplateTabellaRiga)
                        Case "DOCUMENTO_RAG_PRENOTAZIONE"
                            Call caricaRiga_Documento_Rag_Prentazione(nodoTemplateTabellaRiga)
                        Case "DOCUMENTO_RAG_ASSUNZIONE"
                            Call caricaRiga_Documento_rag_assunzione(nodoTemplateTabellaRiga)
                        Case "DOCUMENTO_RAG_LIQUIDAZIONE"
                            Call caricaRiga_Documento_rag_liquidazione(nodoTemplateTabellaRiga)
                    End Select
                Next
            Next
            Contenuto.Controls.Add(pnlDatiDelibera)
        End If
    End Sub



    Private Sub caricaRiga_Documento_Bilancio(ByRef nodo As System.Xml.XmlNode)
        Dim InputText As Web.UI.WebControls.TextBox
        Dim nodoColonna As System.Xml.XmlNode
        Dim attributo As System.Xml.XmlAttribute

        For Each nodoColonna In nodo.ChildNodes
            InputText = Page.FindControl("txt_" & nodoColonna.Name & "1")
            If Not InputText Is Nothing Then
                'rocco 11-05-2006 : creare funzione <<
                InputText.Text = nodoColonna.InnerText
                For Each attributo In nodoColonna.Attributes
                    Select Case UCase(attributo.Name)
                        Case "ERRORETIPODATO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "ERRORECOMPITO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "DISABILITA"
                            InputText.Enabled = False
                    End Select
                Next
                'rocco 11-05-2006 : creare funzione >>
            End If


        Next
    End Sub

    Private Sub caricaRiga_Documento_Rag_Prentazione(ByRef nodo As System.Xml.XmlNode)
        Dim InputText As Web.UI.WebControls.TextBox
        Dim nodoColonna As System.Xml.XmlNode
        Dim attributo As System.Xml.XmlAttribute

        For Each nodoColonna In nodo.ChildNodes
            InputText = Page.FindControl("txt_" & nodoColonna.Name & "1")
            If Not InputText Is Nothing Then
                InputText.Text = nodoColonna.InnerText
                For Each attributo In nodoColonna.Attributes
                    Select Case UCase(attributo.Name)
                        Case "ERRORETIPODATO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "ERRORECOMPITO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "DISABILITA"
                            InputText.Enabled = False
                    End Select
                Next
            End If
        Next
    End Sub

    Private Sub caricaRiga_Documento_rag_liquidazione(ByRef nodo As System.Xml.XmlNode)
        Dim InputText As Web.UI.WebControls.TextBox
        Dim nodoColonna As System.Xml.XmlNode
        Dim attributo As System.Xml.XmlAttribute

        For Each nodoColonna In nodo.ChildNodes
            InputText = Page.FindControl("txt_" & nodoColonna.Name & "1")
            If Not InputText Is Nothing Then
                InputText.Text = nodoColonna.InnerText
                For Each attributo In nodoColonna.Attributes
                    Select Case UCase(attributo.Name)
                        Case "ERRORETIPODATO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "ERRORECOMPITO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "DISABILITA"
                            InputText.Enabled = False
                    End Select
                Next
            End If
        Next
    End Sub

    Private Sub caricaRiga_Documento_rag_assunzione(ByRef nodo As System.Xml.XmlNode)
        Dim InputText As Web.UI.WebControls.TextBox
        Dim nodoColonna As System.Xml.XmlNode
        Dim attributo As System.Xml.XmlAttribute

        For Each nodoColonna In nodo.ChildNodes
            InputText = Page.FindControl("txt_" & nodoColonna.Name & "1")
            If Not InputText Is Nothing Then
                InputText.Text = nodoColonna.InnerText
                For Each attributo In nodoColonna.Attributes
                    Select Case UCase(attributo.Name)
                        Case "ERRORETIPODATO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "ERRORECOMPITO"
                            InputText.ForeColor = System.Drawing.Color.Red
                            InputText.ToolTip = attributo.Value
                        Case "DISABILITA"
                            InputText.Enabled = False
                    End Select
                Next
            End If
           

        Next
    End Sub

    Private Sub btnRegistra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistra.Click
        Dim xmlTemplate As New System.Xml.XmlDocument
        Dim xmlDatiDaRegistrare As New System.Xml.XmlDocument
        Dim sXmlDatiDaRegistrare As String
        Dim nodoTemplateTabella As System.Xml.XmlNode
        Dim nodoTemplateColonna As System.Xml.XmlNode
        Dim nomeTabella As String
        Dim attributo As System.Xml.XmlAttribute
        Dim idProgRiga As String
        Dim oInput As String

        Dim fileXmlTemplate As String = ConfigurationManager.AppSettings("templateDatiRegistraDelibera")

        xmlTemplate.Load(AppDomain.CurrentDomain.BaseDirectory + fileXmlTemplate)

        sXmlDatiDaRegistrare = ""
        For Each nodoTemplateTabella In xmlTemplate.SelectNodes("/datiDocumento/tabella")
            nomeTabella = ""
            For Each attributo In nodoTemplateTabella.Attributes
                Select Case UCase(attributo.Name)
                    Case "NOME_TABELLA"
                        nomeTabella = attributo.Value
                    Case "COL_PROG_REGISTRAZIONE"
                        idProgRiga = attributo.Value
                End Select
            Next


            For Each nodoTemplateColonna In nodoTemplateTabella.ChildNodes
                oInput = Context.Request.Form.Get("txt_" + nodoTemplateColonna.Name + "1") & ""
                If Trim(oInput <> "") Then
                    nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode("//" & nodoTemplateColonna.Name)
                    If Not nodoTemplateColonna Is Nothing Then
                        nodoTemplateColonna.InnerText = oInput
                    End If
                End If
            Next

            nodoTemplateColonna.InnerText = 1
            sXmlDatiDaRegistrare = sXmlDatiDaRegistrare + nodoTemplateTabella.OuterXml

        Next

        xmlDatiDaRegistrare.LoadXml("<datiDocumento>" & sXmlDatiDaRegistrare & "</datiDocumento>")

        Session.Add("datiXml", xmlDatiDaRegistrare.OuterXml)
        Session.Add("oggetto", Context.Request.Form.Get("txtOggetto") & "")
        Session.Add("pubIntegrale", Context.Request.Form.Get("ddlTipoPubblicazione") & "")
        If Trim(context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", context.Request.QueryString.Get("key"))
        End If
        Response.Redirect("RegistraDeliberaAction.aspx")
    End Sub

    Private Sub txtOggetto_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOggetto.PreRender
        'comm per eliminazione xml
        'txtOggetto.Text = txtOggetto.Text.Replace("&amp;", Chr(38))
        'txtOggetto.Text = txtOggetto.Text.Replace("&aps;", Chr(39))
        'txtOggetto.Text = txtOggetto.Text.Replace("&gt;", Chr(62))
        'txtOggetto.Text = txtOggetto.Text.Replace("&lt;", Chr(60))
        'txtOggetto.Text = txtOggetto.Text.Replace("&quot;", Chr(34))
    End Sub
End Class
