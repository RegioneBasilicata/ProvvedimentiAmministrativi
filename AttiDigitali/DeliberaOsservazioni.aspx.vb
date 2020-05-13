Imports System.Xml.XPath
Imports System.IO
Imports Microsoft.Web.UI.WebControls
Imports System.Xml

Public Class DeliberaOsservazioni
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents pnlOsservazioni As System.Web.UI.WebControls.Panel
    Protected WithEvents txtOsservazioniDirGen As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnRegistraOss As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Osservazioni")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim xmlDatiLetti As New System.Xml.XmlDocument
        Dim sXmlDatiLetti As String
        Dim nodoTemplateTabella As System.Xml.XmlNode
        Dim nomeTabella As String
        Dim attributo As System.Xml.XmlAttribute
        Dim nodoTemplateColonna As System.Xml.XmlNode
        Dim idProgRiga As String
       
        Try
            If Not IsPostBack Then
                Rinomina_Pagina(Me, " Delibera " & context.Items.Item("numeroDoc"))

                sXmlDatiLetti = context.Items.Item("xmlDati")
                If Trim(sXmlDatiLetti) <> "" Then
                    xmlDatiLetti.LoadXml(sXmlDatiLetti)
                End If

                'ciclo sulle tabelle lette
                For Each nodoTemplateTabella In xmlDatiLetti.SelectNodes("/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni']")
                    nomeTabella = ""
                    For Each attributo In nodoTemplateTabella.Attributes
                        Select Case UCase(attributo.Name)
                            Case "NOME_TABELLA"
                                nomeTabella = attributo.Value
                            Case "COL_PROG_REGISTRAZIONE"
                                idProgRiga = attributo.Value
                        End Select
                    Next

                    nodoTemplateColonna = Nothing
                    nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode(".//" & idProgRiga)
                    If Not nodoTemplateColonna Is Nothing Then
                        Select Case nodoTemplateColonna.InnerText
                            Case 1
                                nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode(".//Dno_testo")
                                If Not nodoTemplateColonna Is Nothing Then
                                    txtOsservazioniDirGen.Text = nodoTemplateColonna.InnerText
                                End If

                        End Select
                    End If
                Next

                Contenuto.Controls.Add(pnlOsservazioni)
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnRegistraOss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistraOss.Click
        Dim xmlTemplate As New System.Xml.XmlDocument
        Dim xmlDatiDaRegistrare As New System.Xml.XmlDocument
        Dim sXmlDatiDaRegistrare As String
        Dim nodoTemplateTabella As System.Xml.XmlNode
        Dim nodoTemplateColonna As System.Xml.XmlNode
        Dim attributo As System.Xml.XmlAttribute
        Dim nomeTabella As String
        Dim oInput As String
       Dim idProgRiga As String

        Dim fileXmlTemplate As String = ConfigurationManager.AppSettings("templateDatiRegistraDelibera")

        xmlTemplate.Load(AppDomain.CurrentDomain.BaseDirectory + fileXmlTemplate)

        sXmlDatiDaRegistrare = ""
        For Each nodoTemplateTabella In xmlTemplate.SelectNodes("/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni']")
            nomeTabella = ""
            For Each attributo In nodoTemplateTabella.Attributes
                Select Case UCase(attributo.Name)
                    Case "NOME_TABELLA"
                        nomeTabella = attributo.Value
                    Case "COL_PROG_REGISTRAZIONE"
                        idProgRiga = attributo.Value
                End Select
            Next

            nodoTemplateColonna = Nothing
            nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode(".//" & idProgRiga)
            If Not nodoTemplateColonna Is Nothing Then
                Select Case nodoTemplateColonna.InnerText
                    Case 1
                        oInput = Context.Request.Form.Get("txtOsservazioniDirGen") & ""
                        If Trim(oInput <> "") Then
                            nodoTemplateColonna = nodoTemplateTabella.SelectSingleNode(".//Dno_testo")
                            If Not nodoTemplateColonna Is Nothing Then
                                nodoTemplateColonna.InnerText = oInput
                            End If
                        End If
                End Select

                sXmlDatiDaRegistrare = sXmlDatiDaRegistrare + nodoTemplateTabella.OuterXml

            End If
        Next

        xmlDatiDaRegistrare.LoadXml("<datiDocumento>" & sXmlDatiDaRegistrare & "</datiDocumento>")

        Session.Add("datiXml", xmlDatiDaRegistrare.OuterXml)
        If Trim(context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", context.Request.QueryString.Get("key"))
        End If

        Response.Redirect("RegistraOsservazioniDeliberaAction.aspx")
    End Sub
End Class
