Imports System.Collections.Generic
Imports Newtonsoft.Json
Partial Public Class SceltaOperatore
    Inherits Page

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Scelta Profilo")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim listascelte As Generic.List(Of Ext_SceltaOperatoreInfo)
        listascelte = Context.Session.Item("sceltaOperatore")
        'DirectCast(JavaScriptConvert.SerializeObject(listascelte), String)
        ' scelte.Value = DirectCast(JavaScriptConvert.SerializeObject(listascelte), String)
       
        hiddenCF.Value = Session("CodiceFiscale")
        Session.Remove("CodiceFiscale")
    End Sub

End Class