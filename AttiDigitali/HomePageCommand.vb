Public Class HomePageCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        

        'unire tutte queste interrogazioni in un'unica funzione info_Homepage
        vettoredati = Info_HomePage()
        If vettoredati(0) = 0 Then
            Dim i As Integer

            For i = 0 To UBound(vettoredati(1)(0))
                Select Case CStr(UCase(vettoredati(1)(0)(i, 0)))
                    Case "DETERMINE"
                        If oOperatore.Test_Ruolo("WT001") Then
                            context.Items.Add("numDetermine", vettoredati(1)(0)(i, 1))
                        End If
                    Case "DELIBERE"
                        If oOperatore.Test_Ruolo("WL001") Then
                            context.Items.Add("numDelibere", vettoredati(1)(0)(i, 1))
                        End If
                    Case "DISPOSIZIONI"
                        If oOperatore.Test_Ruolo("WS011") Then
                            context.Items.Add("numDisposizioni", vettoredati(1)(0)(i, 1))
                        End If
                End Select
            Next

            For i = 0 To UBound(vettoredati(1)(1))
                Select Case CStr(UCase(vettoredati(1)(1)(i, 0)))
                    Case "DETERMINE"
                        If oOperatore.Test_Ruolo("WT001") Then
                            context.Items.Add("numDetermineDepositoUfficio", vettoredati(1)(1)(i, 1))
                        End If
                    Case "DELIBERE"
                        If oOperatore.Test_Ruolo("WL001") Then
                            context.Items.Add("numDelibereDepositoUfficio", vettoredati(1)(1)(i, 1))
                        End If
                    Case "DISPOSIZIONI"
                        If oOperatore.Test_Ruolo("WS011") Then
                            context.Items.Add("numDisposizioniDepositoUfficio", vettoredati(1)(1)(i, 1))
                        End If
                End Select
            Next

            context.Items.Add("numMessaggi", vettoredati(1)(2))
        End If

        'se l'operatore è abilitato all'archiviazione leggo le determine pronte per l'archiviazione
        If oOperatore.Test_Ruolo("DT017") Then
            Dim vettoredatiArchiviare As Object = Elenco_Documenti_Da_Archiviare(0, (New DllDocumentale.svrDocumenti(oOperatore).getUtenteArchivio()))
            If IsArray(vettoredatiArchiviare(1)) Then
                context.Items.Add("numDaArchiviare", (UBound(vettoredatiArchiviare(1), 2) + 1))
            End If
        End If
        'se l'operatore è abilitato all'archiviazione leggo le delibere pronte per l'archiviazione
        If oOperatore.Test_Ruolo("DL017") Then
            Dim vettoredatiArchiviare As Object = Elenco_Documenti_Da_Archiviare(1, (New DllDocumentale.svrDocumenti(oOperatore).getUtenteArchivio()))
            If IsArray(vettoredatiArchiviare(1)) Then
                context.Items.Add("numDelDaArchiviare", (UBound(vettoredatiArchiviare(1), 2) + 1))
            End If
        End If
       

       
    End Sub
End Class
