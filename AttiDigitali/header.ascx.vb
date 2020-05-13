Public Class header
    Inherits System.Web.UI.UserControl
    Protected WithEvents labelTr As System.Web.UI.WebControls.Label


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblOperatore As System.Web.UI.WebControls.Label
    Protected WithEvents logo As System.Web.UI.WebControls.Image
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Dim urlLogo As String = "./risorse/immagini/logoRegionePic.gif"
            logo.ImageUrl = urlLogo
            logo.Attributes.Add("vspace", "2")
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            If Not oOperatore Is Nothing Then
                If oOperatore.Codice <> "" Then
                    lblOperatore.Text = "Utente : " + oOperatore.Cognome + " " + oOperatore.Nome
                    If Not oOperatore.oUfficio Is Nothing AndAlso Not String.IsNullOrEmpty(oOperatore.oUfficio.CodUfficio) Then
                        lblOperatore.Text = lblOperatore.Text & " - (" & oOperatore.oUfficio.CodUfficioPubblico & ") " & oOperatore.oUfficio.DescrUfficioBreve
                    End If
                End If

            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub

End Class
