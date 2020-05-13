Public Class FirmaBloccoDocumenti
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)


        Dim vettoredati As Object
        Dim codDocumento As String
        Dim allegatoPerFirma As Object

        Dim elencoDocumentiDaInoltrare As String
        Dim vDocumentiDaInoltrare() As String

        

        Dim codAzione As String = context.Session.Item("codAzione")
        Dim note As String = context.Session.Item("note")


        elencoDocumentiDaInoltrare = context.Session.Item("elencoDocumentiDaInoltrare")
        vDocumentiDaInoltrare = Split(elencoDocumentiDaInoltrare, ",")

        codDocumento = context.Request.QueryString.Get("key")
        vettoredati = Elenco_Allegati(codDocumento, , 1)

        Try
            ReDim allegatoPerFirma(UBound(vettoredati(1), 1), 0)
            For i As Integer = 0 To UBound(vettoredati(1), 1)
                allegatoPerFirma(i, 0) = vettoredati(1)(i, UBound(vettoredati(1), 2))
            Next
            vettoredati(1) = allegatoPerFirma
            context.Items.Add("vettoreDati", vettoredati)
            context.Session.Add("idDocumento", codDocumento)
        Catch ex As InvalidCastException When vettoredati(0) = Nothing
            Throw New Exception("Impossibile leggere documenti allegati da firmare. Riprovare", ex)
        Catch ex As InvalidCastException When vettoredati(0) = 1
            Throw New Exception("Impossibile firmare in quanto non ci sono documenti allegati. Riprovare", ex)
        Catch ex As Exception
            Throw New Exception("Impossibile completare l'operazione. Riprovare", ex)
        End Try



    End Sub
End Class
