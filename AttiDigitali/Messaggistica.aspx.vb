Imports System.Collections.Generic
Partial Public Class Messaggistica
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Messaggistica")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        inviati.Value = IIf(Context.Request.QueryString.Item("inv") = 1, 1, 0)
    End Sub

End Class