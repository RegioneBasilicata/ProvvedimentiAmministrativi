Public Class GUAssegnazioniCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object = Nothing
        Dim vrOpBase As Hashtable = Nothing
        Dim vrOper As Hashtable = Nothing
        Dim vrOpSuper As Hashtable

        Dim idDocumento As String = context.Request.QueryString("key")

        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        context.Items.Add("tipoApplic", tipoApplic)
        vettoredati = Elenco_Documenti(CInt(tipoApplic), , , , , , , , , idDocumento)

        Dim tipoAttoDescrizione As String = DefinisciFlusso(tipoApplic)

        vrOpBase = oOperatore.oUfficio.QuartoLivelloUfficio(tipoAttoDescrizione)
        vrOper = oOperatore.oUfficio.CollaboratoriUfficio(tipoAttoDescrizione)

        If Not vrOpBase Is Nothing Then
            aggiungiValori(vrOper, vrOpBase)
        End If

        If LCase(oOperatore.Codice) = LCase(oOperatore.oUfficio.ResponsabileUfficio(tipoAttoDescrizione)) Then
            vrOpSuper = oOperatore.oUfficio.SupervisoriUfficio(tipoAttoDescrizione)
            aggiungiValori(vrOpSuper, vrOpBase)
        End If

        context.Items.Add("vettoreDati", vettoredati)
        context.Items.Add("vettoreDatiOperatori", vrOpBase)
    End Sub
End Class
