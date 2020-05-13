Public Partial Class PopupDettaglioStorico
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim key As String = Request.QueryString("key")


    End Sub

End Class