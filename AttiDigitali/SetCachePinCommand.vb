Public Class SetCachePinCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim cachepin As String = context.Session.Item("CACHEPIN")
        context.Session.Remove("CACHEPIN")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim vr As Object = oOperatore.Insert_Attributo_Operatore(oOperatore, "CACHEPIN", cachepin)
    End Sub
End Class
