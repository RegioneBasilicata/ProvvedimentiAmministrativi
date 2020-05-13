Public Partial Class WebReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tabella As WebControls.Table
        tabella = Session("tblDati")

        If Not tabella Is Nothing Then
            Dim a As New UtilityWord
            a.exportRicercaToCSV(Response, tabella, Me) ' Context.Items.Item("vettoreDati"))

        End If
        
    End Sub

End Class