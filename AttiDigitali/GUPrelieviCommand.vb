Public Class GUPrelieviCommand
    Inherits RedirectingCommand


    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object = Nothing
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        Dim visualizzaDepositoUrgenti As Boolean = False
        If Not String.IsNullOrEmpty(context.Request.QueryString("visualizzaDepositoUrgenti")) Then
            visualizzaDepositoUrgenti = context.Request.QueryString("visualizzaDepositoUrgenti")
            context.Session.Add("visualizzaDepositoUrgenti", visualizzaDepositoUrgenti)
        End If
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            'txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-1)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If
        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
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
                    Throw New Exception("L'operatore non � abilitato alla visualizzazione di alcun dipartimento")
                End If
            End If
        End If

        


        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")
        Select Case UCase(tipoApplic)

            Case 0
                vettoredati = Elenco_Deposito(0, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, visualizzaDepositoUrgenti)
                context.Items.Add("tipoApplic", 0)
            Case 1
                vettoredati = Elenco_Deposito(1, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero)
                context.Items.Add("tipoApplic", 1)
            Case 2
                vettoredati = Elenco_Deposito(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, visualizzaDepositoUrgenti)
                context.Items.Add("tipoApplic", 2)
        End Select
        context.Items.Add("vettoreDati", vettoredati)
    End Sub
End Class
