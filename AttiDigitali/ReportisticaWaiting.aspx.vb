Partial Public Class ReportisticaWaiting
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        Inizializza_Pagina(Me, "Generazione Report")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        waitingPanelTitle.Value = "Generazione Report"
        waitingPanelMsg.Value = "Attendere. Operazione in corso..."
        If Not Context.Request.QueryString.ToString Is Nothing And Context.Request.QueryString.ToString <> "" Then
            waitingPanelActionUrl.Value = "ReportisticaAction.aspx?" & Context.Request.QueryString.ToString
        Else
            waitingPanelActionUrl.Value = "ReportisticaAction.aspx"
        End If

    End Sub

End Class