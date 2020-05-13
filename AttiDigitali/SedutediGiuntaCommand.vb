Public Class SedutediGiuntaCommand
    Inherits RedirectingCommand
    'rc22022006
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object

        'vettoredati = Seduta_Giunta()
        vettoredati = Elenco_Documenti(1, "02/05/2013", "02/05/2014", , , , , )
        context.Items.Add("vettoreDati", vettoredati)
    End Sub

End Class
