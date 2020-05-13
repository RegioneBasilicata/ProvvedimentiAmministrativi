Imports System.Configuration

Public Class FirmaDocumento
    Inherits WebSession
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lnkAnteprima As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lnkUpload As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents pnlFirma As System.Web.UI.WebControls.Panel
    Protected WithEvents session_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents key As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents numDef As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnProcedi As System.Web.UI.WebControls.Button
    Protected idDocumento As String = ""
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(FirmaDocumento))


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Firma Provvedimento")
    End Sub


#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Try
            btnProcedi.Attributes.Add("onclick", "javascript:" + btnProcedi.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnProcedi, ""))

            If Not IsPostBack Then

                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                If oOperatore.Test_Gruppo("RespUfRg") Then
                    Log.Info("**************************************")
                    Log.Info("**** Operatore " & oOperatore.Codice & " Inizio inoltro singolo di n° 1 atto")
                    Log.Info("**************************************")
                End If
                LabelErrore = New Label
                lblInfo = New Label
                LabelErrore.CssClass = "lblWarning"
                Dim vr As Object = Nothing
                Dim vrOper As Object = Nothing
                vr = Context.Items.Item("vettoreDati")





                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If (vr(0) <> 0 And vr(0) <> 1) Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1) & " - " & vrOper(1))
                    Contenuto.Controls.Add(LabelErrore)
                    pnlFirma.Visible = False
                ElseIf (vr(0) = 1) Then
                    LabelErrore.Visible = True
                    If vr(0) = 1 Then
                        LabelErrore.Text = LabelErrore.Text & "Problemi nel caricare il file da firmare" + "<br/>"
                    End If
                    Contenuto.Controls.Add(LabelErrore)
                    pnlFirma.Visible = False
                    lblInfo.Visible = False
                Else

                    Select Case CInt(vr(2))
                        Case 0
                            Rinomina_Pagina(Me, "Firma Determina")
                        Case 1
                            Rinomina_Pagina(Me, "Firma Delibera")
                        Case 2
                            Rinomina_Pagina(Me, "Firma Disposizione")
                    End Select

                    lblInfo.CssClass = "lbl"
                    lblInfo.Width = Web.UI.WebControls.Unit.Point(500)
                    lblInfo.Attributes.Add("wordwrap", True)
                    lblInfo.Attributes.Add("autosize", True)

                    idDocumento = Request.Params.Item("key")
                    If String.IsNullOrEmpty(idDocumento) Then
                        idDocumento = Session.Item("codDocumento")
                    End If

                    lnkAnteprima.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "AnteprimaAllegatoAction.aspx?key=" + CStr(vr(1)(0, 0)) & "&pdf=1&prew=1")
                    Select Case CInt(vr(2))
                        Case 0
                            lnkUpload.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDeterminaAction.aspx?key=" + idDocumento)
                        Case 1
                            lnkUpload.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDeliberaAction.aspx?key=" + idDocumento)
                        Case 2
                            lnkUpload.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDisposizioneAction.aspx?key=" + idDocumento)
                        Case 3
                            lnkUpload.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaDecretoAction.aspx?key=" + idDocumento)
                        Case 4
                            lnkUpload.Value = Context.Request.Url.AbsoluteUri.Replace(Context.Request.Url.PathAndQuery, Context.Request.Url.Segments(0) & Context.Request.Url.Segments(1) & "RegistraFirmaOrdinanzaAction.aspx?key=" + idDocumento)
                    End Select

                    Session.Add("tipoApplic", vr(2))
                    Session.Add("nomeDocumentoFirma", vr(1)(2, 0))
                    session_id.Value = Session.SessionID
                    Dim objDoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocumento)

                    Dim numerare As Boolean = False
                    numerare = Session.Item("numerare")
                    Session.Remove("numerare")
                    If numerare Then
                        key.Value = objDoc.Doc_numeroProvvisorio
                        numDef.Value = IIf(String.IsNullOrEmpty(objDoc.Doc_numero), "", objDoc.Doc_numero)
                    Else
                        key.Value = IIf(String.IsNullOrEmpty(objDoc.Doc_numero), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero)
                        numDef.Value = ""
                    End If
                End If

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub

    Private Sub btnProcedi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcedi.Click
        Response.Redirect("InoltraDocumentoAction.aspx?tipo=" & TipoApplic(Context))
    End Sub
End Class
