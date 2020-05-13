Public Class GUPrelazioniCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))

        vettoredati = Elenco_DocumentiUfficio(CInt(tipoApplic))
        context.Items.Add("tipoApplic", CInt(tipoApplic))

        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
