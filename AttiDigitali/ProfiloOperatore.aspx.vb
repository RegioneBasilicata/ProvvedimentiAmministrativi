Public Class ProfiloOperatore
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblRuoli As System.Web.UI.WebControls.Table
    Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents pnlConfEmail As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlUtilityFirmaMultipla As System.Web.UI.WebControls.Panel
    Protected WithEvents chkOpzioni As CheckBoxList

    Protected WithEvents CheckBoxPINCACHE As CheckBox
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCodiceFiscale As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpzioniMessaggi As System.Web.UI.WebControls.Button
    Protected WithEvents btnSaveCachePin As System.Web.UI.WebControls.Button
  
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me,"Profilo Utente")
    End Sub

#End Region

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(ProfiloOperatore))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            If Not IsPostBack Then
                
                'esito cambio password
                If Not Context.Items.Item("esitoCambioPass") Is Nothing Then
                    Dim LabelesitoCambioPass As New Label
                    LabelesitoCambioPass.CssClass = "lblErrore"
                    LabelesitoCambioPass.Text = Context.Items("esitoCambioPass")
                    Contenuto.Controls.Add(LabelesitoCambioPass)
                End If

                Contenuto.Controls.Add(pnlConfEmail)


                If ConfigurationManager.AppSettings("AUTENTICAZIONE") <> "IMS" Then
                    Contenuto.Controls.Add(pnlModificaPassword)
                    txtCodiceFiscale.ReadOnly = True
                    txtEmail.ReadOnly = True
                Else
                    pnlModificaPassword.Visible = False
                    txtEmail.ReadOnly = True
                    txtCodiceFiscale.ReadOnly = True
                End If

                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"

                If Not Context.Session.Item("esito") Is Nothing Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(Context.Session.Item("esito"))
                    Contenuto.Controls.Add(LabelErrore)
                    Context.Session.Remove("esito")
                End If
              
                txtEmail.Text = oOperatore.Email
                txtCodiceFiscale.Text = oOperatore.CodiceFiscale
                For Each item As ListItem In chkOpzioni.Items
                    If oOperatore.OpzioniMessaggi(item.Value) = 1 Then
                        item.Selected = True
                    Else
                        item.Selected = False
                    End If

                Next


                pnlUtilityFirmaMultipla.Visible = True
                CheckBoxPINCACHE.Visible = True
                CheckBoxPINCACHE.Checked = IIf(UCase(oOperatore.Attributo("CACHEPIN")) = "TRUE", 1, 0)
                Contenuto.Controls.Add(pnlUtilityFirmaMultipla)
            End If


            
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnModificaPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificaPassword.Click
        context.Session.Add("oldPwd", oldPwd.Text)
        context.Session.Add("newPwd", newPwd.Text)
        Context.Session.Add("newPwdConfirm", newPwdConfirm.Text)
        
        Response.Redirect("CambiaPasswordAction.aspx")


    End Sub
    Private Sub btnSaveCachePin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCachePin.Click
        Context.Session.Add("CACHEPIN", CheckBoxPINCACHE.Checked)
        Response.Redirect("SetCachePinAction.aspx")
    End Sub

   
    
    Protected Sub btnOpzioniMessaggi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpzioniMessaggi.Click
        Context.Session.Add("email", txtEmail.Text)
        Context.Session.Add("cf", txtCodiceFiscale.Text)


        Dim htOpzioni As Hashtable = New Hashtable
        For Each item As ListItem In chkOpzioni.Items

            If item.Selected Then
                htOpzioni.Add(item.Value, 1)
            Else
                htOpzioni.Add(item.Value, 0)
            End If

        Next
        Context.Session.Add("opzioni", htOpzioni)
        Response.Redirect("CambiaOpzioniEmailAction.aspx")

    End Sub
End Class
