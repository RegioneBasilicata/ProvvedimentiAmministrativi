Public Class StampaDocumento
    Inherits WebSession
    'modgg16
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelErrore As System.Web.UI.WebControls.Label
    Protected WithEvents tblDati As System.Web.UI.WebControls.Table
    Protected WithEvents ButtonContinua As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonContinua2 As System.Web.UI.WebControls.Button
    Protected WithEvents TipiLettera As System.Web.UI.WebControls.Label
    Protected WithEvents TipiLetteraList As System.Web.UI.WebControls.DropDownList
    Protected WithEvents StatoLettera As System.Web.UI.WebControls.Label
    Protected WithEvents StatoLetteraList As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnFirma As System.Web.UI.WebControls.Button
    Protected WithEvents linkContinua As System.Web.UI.WebControls.HyperLink

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me,"Allegati Documento")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                LabelErrore = New Label
                LabelErrore.CssClass = "lblErrore"
                Dim vr As Object = Nothing
                vr = context.Items.Item("vettoreDati")
                If Not vr Is Nothing Then
                    vr = CType(vr, Array)
                End If
                If vr(0) <> 0 Then
                    LabelErrore.Visible = True
                    LabelErrore.Text() = CStr(vr(1))
                    Contenuto.Controls.Add(LabelErrore)
                Else
                    Dim i As Integer
                    Dim objGriglia As New Griglia
                    Dim vettSeparato As Object
                    vettSeparato = separaArrayPerTipo(vr(1), 1)
                    For i = 0 To UBound(vettSeparato, 1)
                        tblDati = New Table
                        'modgg 10-06
                        tblDati.Width = Web.UI.WebControls.Unit.Pixel(750)
                        objGriglia.Tabella() = tblDati
                        objGriglia.TastoDettaglio() = False
                        objGriglia.Trasposta() = True
                        'objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Tipo", "Nome", "", "", ""}
                        'objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, False, False, False}
                        'objGriglia.ControlloColonna = New String() {"CHECK", "", "", "", "", "", ""}
                        'objGriglia.idControlloColonna = New String() {"chkStampa", "", "", "", "", "", ""}

                        objGriglia.VetDatiIntestazione() = New String() {"&nbsp;", "Tipo", "Nome", "", "Autore", "", "", "Rintracciabilità", "Referente"}
                        objGriglia.ControlloColonna = New String() {"CHECK", "", "", "", "", "", "", "", ""}
                        objGriglia.idControlloColonna = New String() {"chkStampa", "", "", "", "", "", "", "", ""}
                        objGriglia.VetDatiNonVisibili() = New Boolean() {True, True, True, False, True, False, False, False, False}

                        objGriglia.Vettore = vettSeparato(i)
                        objGriglia.crea_tabella_daVettore()
                        Contenuto.Controls.Add(tblDati)
                    Next
                    ButtonContinua.Visible = True
                    ButtonContinua.Enabled = True
                    'linkContinua.Visible = True
                    'linkContinua.NavigateUrl() = "AvviaStampeDocumentiAction.aspx"
                    Contenuto.Controls.Add(ButtonContinua)
                    context.Session.Remove("valoriSelezionati")
                End If
            Else
                '                Albero.Controls(1).Controls(1).Controls.RemoveAt(1)
                CType(CType(Albero.Controls(1).Controls(1), System.Web.UI.Control), Microsoft.Web.UI.WebControls.TreeView).Nodes.RemoveAt(1)

                '    Dim valoriSelezionati As String = Request.Params.Item("chkStampa")
                '    valoriSelezionati = valoriSelezionati.Replace(",", "")
                '    context.Items.Add("valoriSelezionati", valoriSelezionati)
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub ButtonContinua_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonContinua.Click
        ApriPopUp()
    End Sub

    Private Sub linkContinua_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkContinua.PreRender
        Dim valoriSelezionati As Object = Request.Params.Item("chkStampa")
        context.Items.Add("valoriSelezionati", valoriSelezionati)
    End Sub
    Private Sub ApriPopUp()
        context.Response.Clear()
        Dim valoriSelezionati As String = Request.Params.Item("chkStampa")
        valoriSelezionati = valoriSelezionati.Replace(",", "-")

        Dim idDoc As String = "" & Request.QueryString("key")
        context.Session.Add("valoriSelezionati", valoriSelezionati)

        Dim url As String = "<script language='javascript'>window.open('AvviaStampeDocumentoAction.aspx?idDoc=" & idDoc & "','avvia')</script>"
        Response.Write(url)

    End Sub
    Private Sub AvviaStampa()
        Dim vettoredati As Object
        Dim codDocumento As String

        Dim i As Integer

        codDocumento = (context.Request.UrlReferrer.Query).Substring(context.Request.UrlReferrer.Query.IndexOf("=") + 1)
        vettoredati = Elenco_Allegati(codDocumento)

        Dim valoriSelezionati As String = Request.Params.Item("chkStampa")
        valoriSelezionati = valoriSelezionati.Replace(",", "-")


        Dim shtml As String
        shtml = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"" > " _
        & "<html>" _
        & "<head>" _
        & "<title>Sistema di Gestione Atti Amministrativi</title>"

        Dim url As String = HttpContext.Current.Request.UrlReferrer.ToString
        Dim indextofSlash As Integer = url.LastIndexOf("/")
        url = url.Substring(0, indextofSlash) & "/formStampa.htm"

        Dim rows As String = "50%"
        Dim nomeframe As String

        Dim vettSeparato As Object
        vettSeparato = separaArrayPerTipo(vettoredati(1), 1)
        Dim NumRows As Integer = CStr(UBound(vettSeparato, 1))
        For i = 0 To NumRows
            rows += ",50%"
        Next
        rows = 100%
        Dim lstr_QueryString As String = ""
        For i = 0 To NumRows

            nomeframe = "all" & CStr(i)

            lstr_QueryString += vettSeparato(i)(0, 0) & "-"
        Next

        If lstr_QueryString <> "" Then
            lstr_QueryString = "?ids=" & lstr_QueryString
        End If
        shtml += "<frameset rows=""" & rows & """> "

        shtml += ("<frame src=""" & url & lstr_QueryString & """ name=""principale"" id=""principale"">")



        shtml += ("</frameset>")
        shtml += "</html>"


        context.Response.Write(shtml)

    End Sub
End Class
