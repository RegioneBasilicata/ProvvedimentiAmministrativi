Public Class EsitoOperazione
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelMsg As System.Web.UI.WebControls.Label
    Protected WithEvents btnWorklist As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Esito")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim tipoApplic As String
                tipoApplic = context.Items.Item("tipoApplic")
                If Not tipoApplic Is Nothing Then
                    context.Session.Add("tipoApplic", tipoApplic)
                ElseIf Not String.IsNullOrEmpty(Request.QueryString("tipo")) Then
                    Context.Session.Add("tipoApplic", Request.QueryString("tipo"))

                End If

                LabelMsg = New Label
                LabelMsg.CssClass = "lblErrore"
                LabelMsg.Text = " " & "<br/>" & _
                                   HttpContext.Current.Session("msgEsito")

                Dim tabella As Web.UI.WebControls.Table
                Dim riga As Web.UI.WebControls.TableRow
                Dim cella As Web.UI.WebControls.TableCell


                LabelMsg.Width = Web.UI.WebControls.Unit.Point(250)
                LabelMsg.Attributes.Add("wordwrap", True)
                LabelMsg.Attributes.Add("autosize", True)
                LabelMsg.CssClass = "lbl"

                tabella = New Web.UI.WebControls.Table
                tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                tabella.HorizontalAlign = HorizontalAlign.Center

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(LabelMsg)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                btnWorklist.Visible = True
                btnWorklist().Text = "OK"
                cella.Controls.Add(btnWorklist)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)

                Contenuto.Controls.Add(tabella)
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnWorklist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWorklist.Click
        Dim tipoApplic As String = context.Session.Item("tipoApplic")
        context.Session.Remove("tipoApplic")
        context.Session.Remove("msgEsito")
        'Dim urlChiamante As String = context.Request.UrlReferrer.Segments(context.Request.UrlReferrer.Segments.Length - 1)
        'If UCase(urlChiamante.Substring(0, 13)) = "CREADOCUMENTO" Then
        '    Response.Redirect("LeggiTestoDeterminaAction.aspx?key=" & context.Session.Item("key"))
        '    context.Session.Remove("key")
        'End If
        Response.Redirect("WorklistAction.aspx?tipo=" & tipoApplic)
    End Sub
End Class
