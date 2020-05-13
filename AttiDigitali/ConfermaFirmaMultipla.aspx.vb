Public Class ConfermaFirmaMultipla
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblConferma As System.Web.UI.WebControls.Label
    Protected WithEvents NumeroAllegati As New System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents lnkUpload As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlFirma As System.Web.UI.WebControls.Panel
    Protected WithEvents session_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnProcedi As System.Web.UI.WebControls.Button
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Firma Multipla ")
    End Sub

#End Region
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(ConfermaFirmaMultipla))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            btnProcedi.Attributes.Add("onclick", "javascript:" + btnProcedi.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnProcedi, ""))

            
            If Not IsPostBack Then

                Dim urlDaFirmare() As String = Context.Session.Item("urlDaFirmare")
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                If oOperatore.Test_Gruppo("RespUfRg") Then
                    Log.Info("**************************************")
                    Log.Info("****** Operatore " & oOperatore.Codice & " Inizio firma multipla di n° " & urlDaFirmare.Length & " atto")
                    Log.Info("**************************************")
                End If
                Dim hyperlinkTemp As HtmlInputHidden
                Dim hKey As HtmlInputHidden
                Dim hNumero As HtmlInputHidden
                Dim hNumeroDef As HtmlInputHidden
                Dim vCodiciDoc As Object = HttpContext.Current.Session.Item("vCodiciDoc")

                Rinomina_Pagina(Me, "Conferma Firma Multipla")





                For i As Integer = 0 To urlDaFirmare.Length - 1

                    hyperlinkTemp = New HtmlInputHidden
                    hyperlinkTemp.ID = "link" & i.ToString
                    hyperlinkTemp.Value = urlDaFirmare(i) & "&pdf=1&prew=1"
                    Page.Controls.Add(hyperlinkTemp)
                    hKey = New HtmlInputHidden
                    hKey.ID = "hKey" & i
                    hKey.Value = vCodiciDoc(i, 1)
                    hKey.Name = "hKey" & i

                    hNumero = New HtmlInputHidden
                    hNumero.ID = "hNumero" & i
                    hNumero.Name = "hNumero" & i

                    hNumeroDef = New HtmlInputHidden
                    hNumeroDef.ID = "hNumeroDef" & i
                    hNumeroDef.Name = "hNumeroDef" & i

                    Dim numerare As Boolean = False
                    numerare = Session.Item("numerare")

                    If numerare Then

                        hNumero.Value = vCodiciDoc(i, 1)
                        hNumeroDef.Value = vCodiciDoc(i, 5)
                    Else
                        hNumero.Value = IIf(String.IsNullOrEmpty(vCodiciDoc(i, 5)), vCodiciDoc(i, 1), vCodiciDoc(i, 5))
                        hNumeroDef.Value = ""
                    End If

                    Page.Controls.Add(hNumeroDef)
                    Page.Controls.Add(hNumero)

                    Select Case CInt(Context.Session.Item("tipoApplic"))
                        Case 0
                            hKey.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDeterminaAction.aspx?key=" + vCodiciDoc(i, 0))
                        Case 1
                            hKey.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDeliberaAction.aspx?key=" + vCodiciDoc(i, 0))
                        Case 2
                            hKey.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDisposizioneAction.aspx?key=" + vCodiciDoc(i, 0))
                        Case 3
                            hKey.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDecretoAction.aspx?key=" + vCodiciDoc(i, 0))
                        Case 4
                            hKey.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaOrdinanzaAction.aspx?key=" + vCodiciDoc(i, 0))
                    End Select
                    Page.Controls.Add(hKey)
                Next
                Session.Remove("numerare")
                NumeroAllegati.Value = urlDaFirmare.Length
                session_id.Value = Session.SessionID
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub


    Protected Sub btnProcedi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcedi.Click
        Session.Remove("vCodiciDoc")
        Response.Redirect("InoltraBloccoDocumentiAction.aspx?tipo=" & TipoApplic(Context))
    End Sub
End Class
