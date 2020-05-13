Public Class AssegnaDocumentiCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim tipoApplic As String = context.Request.Params.Item("tipo")
        Dim elencoDocDaAssegnare As String
        Dim operatoreAssegnatario As String
        Dim vDocDaAssegnare As Object
        Dim note As String
        Dim vR As Object = Nothing
        Dim i As Integer
        Dim esitoCiclo As Boolean = False

        elencoDocDaAssegnare = context.Session.Item("elencoDocumentiDaAssegnare")
        operatoreAssegnatario = context.Session.Item("operatoreAssegnatario")
        note = context.Session.Item("note")
       vDocDaAssegnare = Split(elencoDocDaAssegnare, ",")

        If Trim(operatoreAssegnatario) <> "" Then
            For i = 0 To UBound(vDocDaAssegnare)
                If Trim(vDocDaAssegnare(i)) <> "" Then
                    Select Case CInt(tipoApplic)
                        Case 0
                            vR = Inoltra_Determina(vDocDaAssegnare(i), "Assegna", operatoreAssegnatario, note)
                            If vR(0) <> 0 Then
                                esitoCiclo = False
                            Else
                                esitoCiclo = True
                                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", vDocDaAssegnare(i), 0)
                            End If
                        Case 1
                            vR = Inoltra_Delibera(vDocDaAssegnare(i), "Assegna", operatoreAssegnatario, note)

                            If vR(0) <> 0 Then
                                esitoCiclo = False
                            Else
                                esitoCiclo = True
                                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", vDocDaAssegnare(i), 1)
                            End If
                        Case 2
                            vR = Inoltra_Disposizione(vDocDaAssegnare(i), "Assegna", operatoreAssegnatario, note)
                            If vR(0) <> 0 Then
                                esitoCiclo = False
                            Else
                                esitoCiclo = True
                                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", vDocDaAssegnare(i), 2)
                            End If
                        Case 3, 4
                            vR = Inoltra_AltroAtto(vDocDaAssegnare(i), "Assegna", operatoreAssegnatario, note, , , CInt(tipoApplic))
                            If vR(0) <> 0 Then
                                esitoCiclo = False
                            Else
                                esitoCiclo = True
                                tipoApplic = aggiorna_Documenti_Sessione(context, "CHIUDI", vDocDaAssegnare(i), 1)
                            End If
                    End Select
                End If
            Next
        End If


        Dim numDoc As Integer = CInt(UBound(vDocDaAssegnare))
        Dim msgEsito As String

        If esitoCiclo = True Then
            If numDoc >= 1 Then
                msgEsito = "Il documento è stato assegnato con successo. Adesso non è più presente nella tua lista lavoro"
            Else
                msgEsito = "I documenti sono stati assegnati con successo. Adesso non sono più presenti nella tua lista lavoro"
            End If
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
        Else
            If numDoc >= 1 Then
                msgEsito = "L'assegnazione del documento non è avvenuta con successo."
            Else
                msgEsito = "L'assegnazione di almeno uno dei documenti non è avvenuta con successo, si prega di verificare."
            End If
            HttpContext.Current.Session.Add("msgEsito", msgEsito)
        End If
        context.Items.Add("tipoApplic", tipoApplic)

        'Select Case CInt(tipoApplic)
        '    Case 0
        '        vR = Elenco_Documenti(tipoApplic)
        '        context.Items.Add("vettoreDati", vR)
        '        vR = oOperatore.oUfficio.CollaboratoriUfficio("DETERMINE")
        '        context.Items.Add("vettoreDatiOperatori", vR)
        '    Case 1
        '        vR = Elenco_Documenti(tipoApplic)
        '        context.Items.Add("vettoreDati", vR)
        '        vR = oOperatore.oUfficio.CollaboratoriUfficio("DELIBERE")
        '        context.Items.Add("vettoreDatiOperatori", vR)
        '    Case 2
        '        vR = Elenco_Documenti(tipoApplic)
        '        context.Items.Add("vettoreDati", vR)
        '        vR = oOperatore.oUfficio.CollaboratoriUfficio("DETERMINE")
        '        context.Items.Add("vettoreDatiOperatori", vR)
        'End Select
    End Sub
End Class
