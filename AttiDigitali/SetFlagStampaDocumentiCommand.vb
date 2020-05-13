Public Class SetFlagStampaDocumentiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)

        Dim elencoDocumentiDaInoltrare As String
        Dim vDocumentiDaInoltrare() As String
        Dim vR As Object = Nothing
        Dim i As Integer
        Dim tipoApplic As String


        tipoApplic = context.Request.QueryString.Get("tipo")

        Dim codAzione As String = context.Session.Item("codAzione")
        Dim flagStampato As String = context.Session.Item("FlagStampato")
        context.Session.Remove("FlagStampato")

        elencoDocumentiDaInoltrare = context.Session.Item("elencoDocumentiDaInoltrare")

        vDocumentiDaInoltrare = Split(elencoDocumentiDaInoltrare, ",")
        Dim numeroDoc = ""
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        For i = 0 To UBound(vDocumentiDaInoltrare)
            If Trim(vDocumentiDaInoltrare(i)) <> "" Then
                numeroDoc = ""
              
                Dim oDllDocumento As New DllDocumentale.svrDocumenti(oOperatore)
                'fine Gestione rigetto in blocco disposizione
                oDllDocumento.FO_Update_StatoStampaDocumento(oOperatore.Codice, vDocumentiDaInoltrare(i), flagStampato)
              
                
            End If

        Next
        context.Session.Remove("elencoDocumentiDaInoltrare")
      context.Items.Add("tipoApplic", tipoApplic)
        context.Response.Redirect("MonitorArchivioStampaAction.aspx?tipo=" & tipoApplic, True)
    End Sub

End Class
