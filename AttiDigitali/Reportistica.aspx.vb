Imports System.Collections.Generic
Imports DllDocumentale

Partial Class Reportistica
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnInoltra As System.Web.UI.WebControls.Button
    Protected WithEvents btnFirma As System.Web.UI.WebControls.Button
    Protected WithEvents pnlInoltro As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlNote As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSupervisore As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LblErrore As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    Private isPostback

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Invio Provvedimento")

        Dim lstrCalendarPopupDataA As String = ""
        If lstrCalendarPopupDataA <> "" Then
            CalendarPopupDataA.SelectedDate = lstrCalendarPopupDataA
        Else

            If CalendarPopupDataA.SelectedDate.Year = 1 Then
                CalendarPopupDataA.SelectedDate = Date.Now
            End If
        End If

        Dim lstrCalendarPopupDataDa As String = ""
        If lstrCalendarPopupDataDa <> "" Then
            CalendarPopupDataDa.SelectedDate = lstrCalendarPopupDataDa
        Else
            If CalendarPopupDataDa.SelectedDate.Year = 1 Then
                CalendarPopupDataDa.SelectedDate = New Date(Year(Today), 1, 1)
            Else
                CalendarPopupDataDa.SelectedDate = Now.AddMonths(-3)
            End If
        End If
        LinkAttiTotali.Visible = False
    End Sub

#End Region
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Reportistica))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim vR As Object = Nothing
        Try

            If Page.IsPostBack Then
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                LinkAttiTotali.Visible = True
                LinkAttiTotali.NavigateUrl = "GeneraReportAction.aspx"
                Dim var As String = ""

            End If



        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnFiltraRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFiltraRicerca.Click
        Session.Add("txtFiltroDataDa", CalendarPopupDataDa.SelectedDate)
        Session.Add("txtFiltroDataA", CalendarPopupDataA.SelectedDate)

        Dim lstr As String = Context.Request.UrlReferrer.AbsoluteUri
        If lstr.IndexOf("Archivio") = -1 Then
            If lstr.IndexOf("Action") = -1 Then
                lstr = lstr.Insert(lstr.LastIndexOf("."), "Action")
            End If
        Else
            'lstr = "ReportisticaWaiting.aspx?" & Context.Request.QueryString.ToString
        End If
        Response.Redirect("GeneraReportAction.aspx")
        'Response.Redirect("ReportisticaWaiting.aspx?" & Context.Request.QueryString.ToString)
    End Sub


End Class
