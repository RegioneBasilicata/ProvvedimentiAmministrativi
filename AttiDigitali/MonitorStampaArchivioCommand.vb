''' <summary>
''' Questa funzione viene chiamata in Archivio e Worklist Print User
''' </summary>
''' <remarks></remarks>
Public Class MonitorStampaArchivioCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
        Dim txtDescUfficio As String = context.Session.Item("txtDescUfficio")
        Dim urlCorrente As String = context.Request.Path
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        Dim tipoData As Integer = context.Session.Item("radioData")
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            ' txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-3)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If

        Dim flagPrinterUser As Boolean = False


        If urlCorrente.IndexOf("MonitorUfficioAction") > -1 Then
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            txtCodUfficio = oOperatore.oUfficio.CodUfficio
        End If
        Dim txtNumero As String = "" & context.Session("txtNumero")

        If context.Session.Item("txtCodDipartimenti") Is Nothing OrElse String.IsNullOrEmpty(context.Session.Item("txtCodDipartimenti")) Then
            Dim op As DllAmbiente.Operatore = context.Session.Item("oOperatore")
            If Not op.oUfficio.CodDipartimento Is Nothing And Not String.IsNullOrEmpty(op.oUfficio.CodDipartimento) Then
                context.Session.Add("txtCodDipartimenti", op.oUfficio.CodDipartimento)
            Else
                Dim hta As ArrayList = op.DipartimentoDipendenti(tipoApplic)
                If hta.Count > 0 Then
                    context.Session.Add("txtCodDipartimenti", DirectCast(hta.Item(0), DllAmbiente.StrutturaInfo).CodiceInterno)
                Else
                    Throw New Exception("L'operatore non è abilitato alla visualizzazione di alcun dipartimento")
                End If
            End If
        End If

        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")
        Dim tipoStato As String = "" & context.Session("StatoStampa")
        If String.IsNullOrEmpty(tipoStato) Then
            tipoStato = "0"
            context.Session("StatoStampa") = tipoStato
        End If
        vettoredati = Consulta_documenti_AltriUffici(CInt(tipoApplic), tipoData, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, True, , , tipoStato)
        If vettoredati(0) = 0 Then

            Dim consultazione As Boolean = False
            Dim ufCons As ArrayList = oOperatore.oUfficio.UfficiConsultabili(tipoApplic, oOperatore.pCodice)
            If ufCons Is Nothing OrElse ufCons.Count = 0 Then
                consultazione = False
            Else
                consultazione = True
            End If
            If (Not consultazione) Then
                mergeVector(context, vettoredati)
            End If
        End If

        context.Items.Add("vettoreDati", vettoredati)
        context.Items.Add("tipoApplic", tipoApplic)
    End Sub
    Private Sub mergeVector(ByRef context As HttpContext, ByRef vettoredati As Object)
        Dim vettoredatiMonitor As Object
        Dim txtDescUfficio As String = context.Session.Item("txtDescUfficio")
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            'txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-3)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If
        Dim txtOggettoRicerca As String = "" & context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = "" & context.Session.Item("txtCodUfficio")
        Dim txtNumero As String = "" & context.Session("txtNumero")

        If context.Session.Item("txtCodDipartimenti") Is Nothing OrElse String.IsNullOrEmpty(context.Session.Item("txtCodDipartimenti")) Then
            Dim op As DllAmbiente.Operatore = context.Session.Item("oOperatore")
            If Not op.oUfficio.CodDipartimento Is Nothing And Not String.IsNullOrEmpty(op.oUfficio.CodDipartimento) Then
                context.Session.Add("txtCodDipartimenti", op.oUfficio.CodDipartimento)
            Else
                Dim hta As ArrayList = op.DipartimentoDipendenti(tipoApplic)
                If hta.Count > 0 Then
                    context.Session.Add("txtCodDipartimenti", DirectCast(hta.Item(0), DllAmbiente.StrutturaInfo).CodiceInterno)
                Else
                    Throw New Exception("L'operatore non è abilitato alla visualizzazione di alcun dipartimento")
                End If
            End If
        End If

        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")

        Dim StatoStampa As String = "" & context.Session("StatoStampa")


        Select Case UCase(tipoApplic)
            Case 0
                vettoredatiMonitor = Elenco_Monitor(0, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , )

            Case 1
                vettoredatiMonitor = Elenco_Monitor(1, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , )

            Case 2
                vettoredatiMonitor = Elenco_Monitor(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , )

        End Select

        Dim objTemp(UBound(vettoredati(1), 1), UBound(vettoredati(1), 2)) As Object
        Dim counter As Integer = 0
        Dim trovato As Boolean = False
        If vettoredatiMonitor(0) = 0 Then
            For i As Integer = 0 To UBound(vettoredati(1), 2)

                For j As Integer = 0 To UBound(vettoredatiMonitor(1), 2)
                    trovato = False
                    If vettoredati(1)(0, i) = vettoredatiMonitor(1)(0, j) Then
                        trovato = True
                        Exit For
                    End If

                Next
                If trovato Then
                    objTemp(0, counter) = vettoredati(1)(0, i)
                    objTemp(1, counter) = vettoredati(1)(1, i)
                    objTemp(2, counter) = vettoredati(1)(2, i)
                    objTemp(3, counter) = vettoredati(1)(3, i)
                    objTemp(4, counter) = vettoredati(1)(4, i)
                    objTemp(5, counter) = vettoredati(1)(5, i)
                    objTemp(6, counter) = vettoredati(1)(6, i)

                    counter += 1
                End If
            Next
        End If


        ReDim Preserve objTemp(UBound(vettoredati(1), 1), counter - 1)

        vettoredati(1) = objTemp

    End Sub
    Private Sub mergeVectorOLD(ByRef context As HttpContext, ByRef vettoredati As Object)
        Dim vettoredatiMonitor As Object
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
        Dim txtDescUfficio As String = context.Session.Item("txtDescUfficio")

        Dim urlCorrente As String = context.Request.Path
        Dim txtDataInizio As Date
        Dim txtDataFine As Date

        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            'txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-3)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If





        Select Case UCase(tipoApplic)
            Case 0
                vettoredatiMonitor = Elenco_Monitor(0, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio)

            Case 1
                vettoredatiMonitor = Elenco_Monitor(1, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio)

            Case 2
                vettoredatiMonitor = Elenco_Monitor(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio)

        End Select

        Dim objTemp(UBound(vettoredati(1), 1), UBound(vettoredati(1), 2)) As Object
        Dim counter As Integer = 0
        Dim trovato As Boolean = False
        If vettoredatiMonitor(0) = 0 Then
            For i As Integer = 0 To UBound(vettoredati(1), 2)

                For j As Integer = 0 To UBound(vettoredatiMonitor(1), 2)
                    trovato = False
                    If vettoredati(1)(0, i) = vettoredatiMonitor(1)(0, j) Then
                        trovato = True
                        Exit For
                    End If

                Next
                If trovato Then
                    objTemp(0, counter) = vettoredati(1)(0, i)
                    objTemp(1, counter) = vettoredati(1)(1, i)
                    objTemp(2, counter) = vettoredati(1)(2, i)
                    objTemp(3, counter) = vettoredati(1)(3, i)
                    objTemp(4, counter) = vettoredati(1)(4, i)
                    objTemp(5, counter) = vettoredati(1)(5, i)
                    objTemp(6, counter) = vettoredati(1)(6, i)

                    counter += 1
                End If
            Next
        End If


        ReDim Preserve objTemp(UBound(vettoredati(1), 1), counter - 1)

        vettoredati(1) = objTemp

    End Sub
End Class
