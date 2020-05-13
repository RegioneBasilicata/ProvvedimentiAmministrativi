Public Class SuccessoCaricaDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnSalvaTesto As System.Web.UI.WebControls.Button
    Protected WithEvents testoEditato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlAggiungiAllegato As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUploadAllegato As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents btnOk As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Dim descrizioneDocumento As String = ""

        Select Case CInt(Session.Item("tipoApplic"))
            Case 0
                descrizioneDocumento = "Determina"
            Case 1
                descrizioneDocumento = "Delibera"
            Case 2
                descrizioneDocumento = "Disposizione"
            Case Else
                descrizioneDocumento = "Documento"
        End Select
        Inizializza_Pagina(Me, "Upload " & descrizioneDocumento & " " & context.Session.Item("numeroPubblicoDocumento"))
        context.Session.Remove("numeroPubblicoDocumento")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim msgEsito As String = "" & HttpContext.Current.Session("msgEsito")
        HttpContext.Current.Session.Remove("msgEsito")

        Label1 = New Label
        If msgEsito = "" Then
            Label1.Text = " " & "<br/> Il caricamento del documento è stato correttamente eseguito."
        Else
            Label1.Text = " " & "<br/> " & msgEsito
        End If


        Contenuto.Controls.Add(Label1)
        Dim tabella As Web.UI.WebControls.Table
        Dim riga As Web.UI.WebControls.TableRow
        Dim cella As Web.UI.WebControls.TableCell
        tabella = New Web.UI.WebControls.Table
        tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
        tabella.HorizontalAlign = HorizontalAlign.Center
        riga = New Web.UI.WebControls.TableRow
        cella = New Web.UI.WebControls.TableCell
        riga = New Web.UI.WebControls.TableRow
        cella = New Web.UI.WebControls.TableCell
        cella.ColumnSpan = 2

        riga.Controls.Add(cella)
        tabella.Rows.Add(riga)
        Contenuto.Controls.Add(tabella)
        riga = New Web.UI.WebControls.TableRow
        cella = New Web.UI.WebControls.TableCell
        btnOk.Visible = True
        cella.Controls.Add(btnOk)
        riga.Controls.Add(cella)
        tabella.Rows.Add(riga)
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("AllegatiDocumentoAction.aspx?key=" & context.Session.Item("key"))
    End Sub

End Class
