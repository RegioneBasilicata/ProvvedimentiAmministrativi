Public Class PrelevaDocumentoCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim tipoApplic As String = context.Request.Params.Item("tipo")
        Dim elencoDocumentiDaPrelevare As String
        Dim vDocumentoDaPrelevare As Object = Nothing
        Dim vR As Object = Nothing
        Dim i As Integer

        Dim esitoCiclo As Boolean = False


        elencoDocumentiDaPrelevare = context.Session.Item("elencoDocumentiDaPrelevare")
        vDocumentoDaPrelevare = Split(elencoDocumentiDaPrelevare, ",")

        For i = 0 To UBound(vDocumentoDaPrelevare)
            If Trim(vDocumentoDaPrelevare(i)) <> "" Then
                Select Case CInt(tipoApplic)
                    Case 0
                        vR = Inoltra_Determina(vDocumentoDaPrelevare(i), "PRELIEVO")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 1
                        vR = Inoltra_Delibera(vDocumentoDaPrelevare(i), "PRELIEVO")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 2
                        vR = Inoltra_Disposizione(vDocumentoDaPrelevare(i), "PRELIEVO")
                        If vR(0) <> 0 Then
                            esitoCiclo = False
                        Else
                            esitoCiclo = True
                        End If
                    Case 3, 4
                        vR = Inoltra_AltroAtto(vDocumentoDaPrelevare(i), "PRELIEVO", , , , , CInt(tipoApplic))
                End Select

            End If
        Next

        'modgg17
        Dim numDoc As Integer = CInt(UBound(vDocumentoDaPrelevare)) + 1
        Dim msgEsito As String
        Dim numDetUrgentiDeposito As Integer = 0
        Dim numDispoUrgentiDeposito As Integer = 0
        If Not context.Session.Item("numDetermineDepositoUrgenti") Is Nothing Then
            numDetUrgentiDeposito = (context.Session.Item("numDetermineDepositoUrgenti"))
        End If
        If Not context.Session.Item("numDisposizioniDepositoUrgenti") Is Nothing Then
            numDispoUrgentiDeposito = (context.Session.Item("numDisposizioniDepositoUrgenti"))
        End If

        


        If esitoCiclo = True Then
            If numDoc > 1 Then
                msgEsito = "I documenti sono stati prelevati con successo. Adesso sono presenti nella tua lista lavoro"
            Else
                msgEsito = "Il documento è stato prelevato con successo. Adesso è presente nella tua lista lavoro"
            End If

            'context.Session.Add("numDisposizioniUrgenti", totaleDisposizioniUrgenti)
            If context.Session.Item("visualizzaDepositoUrgenti") Then
                Select Case CInt(tipoApplic)
                    Case 0
                        If numDetUrgentiDeposito >= numDoc Then
                            numDetUrgentiDeposito = numDetUrgentiDeposito - numDoc
                            context.Session.Add("numDetermineDepositoUrgenti", numDetUrgentiDeposito)
                            Dim numDetermineUrgenti As Integer = context.Session.Item("numDetermineUrgenti")
                            context.Session.Add("numDetermineUrgenti", numDetermineUrgenti + numDoc)
                        End If
                    Case 2
                        If numDispoUrgentiDeposito >= numDoc Then
                            numDispoUrgentiDeposito = numDispoUrgentiDeposito - numDoc
                            context.Session.Add("numDisposizioniDepositoUrgenti", numDispoUrgentiDeposito)
                            Dim numDisposizioniUrgenti As Integer = context.Session.Item("numDisposizioniUrgenti")
                            context.Session.Add("numDisposizioniUrgenti", numDisposizioniUrgenti + numDoc)


                        End If
                End Select
            End If
            context.Session.Remove("visualizzaDepositoUrgenti")
        Else
            If numDoc >= 1 Then
                msgEsito = "Il prelievo del documento non è avvenuto con successo."
            Else
                msgEsito = "Il prelievo di alcuni documenti non è avvenuto con successo."
            End If
        End If
        HttpContext.Current.Session.Add("msgEsito", msgEsito)

        Dim vettoredati As Object
        Select Case CInt(tipoApplic)
            Case 0
                vettoredati = Elenco_Deposito(0)
            Case 1
                vettoredati = Elenco_Deposito(1)
            Case 2
                vettoredati = Elenco_Deposito(2)
        End Select
        context.Items.Add("tipoApplic", tipoApplic)
        context.Items.Add("vettoreDati", vettoredati)
    End Sub

End Class
