Public Class AssegnaNumerazione
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnRegistra As System.Web.UI.WebControls.Button
    Protected WithEvents txtNumDef2 As System.Web.UI.WebControls.TextBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Assegna Numerazione Definitiva")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg 10-06 3
        Try

            If Not IsPostBack Then
                Dim tblInfo As New Web.UI.WebControls.Table
                Dim rigaInfo As New Web.UI.WebControls.TableRow
                Dim cellaInfo As New Web.UI.WebControls.TableCell
                Dim cellaInfo2 As New Web.UI.WebControls.TableCell
                Dim cellaInfo3 As New Web.UI.WebControls.TableCell
                Dim rigaInfo2 As New Web.UI.WebControls.TableRow

                'Label Numero definitivo
                Dim lblOggetto As New Label
                lblOggetto.Text = "Inserisci numero definitivo: "
                lblOggetto.CssClass = "lbl"
                cellaInfo.Controls.Add(lblOggetto)
                rigaInfo.Controls.Add(cellaInfo)

                txtNumDef2.Visible = True

                cellaInfo2.Controls.Add(txtNumDef2)
                rigaInfo.Controls.Add(cellaInfo2)
                tblInfo.Rows.Add(rigaInfo)

                btnRegistra.Visible = True
                cellaInfo3.HorizontalAlign = HorizontalAlign.Center
                cellaInfo3.ColumnSpan = "2"
                cellaInfo3.Controls.Add(btnRegistra)
                rigaInfo2.Controls.Add(cellaInfo3)
                tblInfo.Rows.Add(rigaInfo2)
                Contenuto.Controls.Add(tblInfo)
            Else

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnRegistra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistra.Click
        If (Not (txtNumDef2 Is Nothing)) OrElse txtNumDef2.Text <> "" Then
            context.Session.Add("key", context.Request.QueryString.Get("key"))
            Session.Add("numeroDefinitivo", txtNumDef2.Text)
            Response.Redirect("RegistraNumeroDefinitivoAction.aspx")
        End If
    End Sub
End Class
