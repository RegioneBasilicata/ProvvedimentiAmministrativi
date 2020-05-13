Public Class ConfermaFirmaMultiplaAX
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblConferma As System.Web.UI.WebControls.Label
    Protected WithEvents btnContinua2 As System.Web.UI.WebControls.Button
    Protected WithEvents btnAnnulla2 As System.Web.UI.WebControls.Button
    Protected WithEvents pnlFirma As System.Web.UI.WebControls.Panel
    'Protected WithEvents NumeroAllegati As System.Web.UI.WebControls.Label
    Protected WithEvents NumeroAllegati As New System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlBottoni As System.Web.UI.WebControls.Panel
    Protected WithEvents TxtTitolareNome As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TxtTitolareCognome As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Firma Multipla ")
        Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

        Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
        Dim dllDoc As New DllDocumentale.svrDocumenti(Context.Session.Item("oOperatore"))
        Dim messaggi_di_Warning As New ArrayList

        For Each docDaInoltrare As String In vDocumentiDaInoltrare

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(docDaInoltrare)
            If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR") Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "USS") Then
                messaggi_di_Warning.Add(Warning(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "UR" Then
                messaggi_di_Warning.Add(Warning_Ragioneria(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "USS" Then
                messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(docDaInoltrare))
            End If
        Next
        Context.Session.Add("warning", messaggi_di_Warning)
    End Sub

#End Region
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(ConfermaFirmaMultipla))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            TxtTitolareCognome.Value = oOperatore.Cognome
            TxtTitolareNome.Value = oOperatore.Nome

            If Not IsPostBack Then
                Dim hyperlinkTemp As HyperLink
                Dim hContenutoFileFirmato As System.Web.UI.HtmlControls.HtmlInputHidden
                Dim vCodiciDoc As Object = HttpContext.Current.Session.Item("vCodiciDoc")

                Rinomina_Pagina(Me, "Conferma Firma Multipla")
                Dim urlDaFirmare() As String = Context.Session.Item("urlDaFirmare")
                For i As Integer = 0 To urlDaFirmare.Length - 1

                    hyperlinkTemp = New HyperLink
                    hyperlinkTemp.ID = "link" & i.ToString
                    hyperlinkTemp.Target = "_blank"
                    hyperlinkTemp.NavigateUrl = urlDaFirmare(i) & "&pdf=1&prew=1"
                    hyperlinkTemp.Visible = True
                    hyperlinkTemp.Text = "   Anteprima " & vCodiciDoc(i, 1) & "<br/>"
                    pnlFirma.Controls.Add(hyperlinkTemp)
                    hContenutoFileFirmato = New System.Web.UI.HtmlControls.HtmlInputHidden
                    hContenutoFileFirmato.ID = "hContenutoFileFirmato" & i
                    hContenutoFileFirmato.Value = "-"
                    hContenutoFileFirmato.Name = "hContenutoFileFirmato" & i
                    pnlFirma.Controls.Add(hContenutoFileFirmato)
                Next

                NumeroAllegati.ID = "NumeroAllegati"
                NumeroAllegati.Value = urlDaFirmare.Length
                pnlFirma.Controls.Add(NumeroAllegati)
                pnlFirma.Visible = True
                pnlBottoni.Visible = True
                btnAnnulla2.Visible = False
                Contenuto.Controls.Add(pnlFirma)
                Contenuto.Controls.Add(pnlBottoni)
            End If
            If Not Context.Session.Item("warning") Is Nothing Then
                LabelErrore.Text = ""
                For Each messaggio As String In DirectCast(Context.Session.Item("warning"), ArrayList)
                    LabelErrore.Text = LabelErrore.Text & messaggio
                Next
                LabelErrore.Visible = True
                Contenuto.Controls.Add(LabelErrore)
            Else
                Context.Session.Remove("warning")
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub


    Private Sub btnContinua2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinua2.Click

        Dim numAllegati As Integer = context.Request.Form.Get("NumeroAllegati")
        Dim hContenutoFileFirmato(numAllegati - 1) As String
        For i As Integer = 0 To numAllegati - 1
            hContenutoFileFirmato(i) = Context.Request.Form.Get("hContenutoFileFirmato" & i)
        Next


        Session.Add("hContenutoFileFirmato", hContenutoFileFirmato)
        If Trim(context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", context.Request.QueryString.Get("key"))
        End If

        Dim tipoApplic As String = Session.Item("tipoApplic")

        Session.Remove("tipoApplic")
        Session.Remove("urlDaFirmare")

        Select Case CInt(tipoApplic)
            Case 0
                Response.Redirect("RegistraFirmaMultiplaDeterminaAction.aspx")
            Case 1
                Response.Redirect("RegistraFirmaMultiplaDeliberaAction.aspx")
            Case 2
                Response.Redirect("RegistraFirmaMultiplaDisposizioneAction.aspx")
        End Select

    End Sub

    Private Sub btnAnnulla2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla2.Click
        Session.Remove("urlDaFirmare")
        Session.Remove("vCodiciDoc")
        Response.Redirect("TornaHomeAction.aspx")

    End Sub
End Class
