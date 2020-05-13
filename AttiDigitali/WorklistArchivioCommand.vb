Public Class WorklistArchivioCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        context.Session.Add("prossimoattore", "ARCHIVIO")
        Dim vettoredati As Object
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            ' txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-1)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If
        Dim txtOggettoRicerca As String = "" & context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = "" & context.Session.Item("txtCodUfficio")
        Dim txtNumero As String = "" & context.Session("txtNumero")
        Dim tipoRigetto As String = "" & context.Session("TipoRigetto")

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
        'modgg eliminato txtDescUfficio
        Select Case UCase(tipoApplic)

            Case 0
                vettoredati = Elenco_Documenti_Da_Archiviare(0, (New DllDocumentale.svrDocumenti(oOperatore).getUtenteArchivio()), , , , , "", , tipoRigetto)
                context.Items.Add("tipoApplic", 0)
            Case 1
                vettoredati = Elenco_Documenti_Da_Archiviare(1, (New DllDocumentale.svrDocumenti(oOperatore).getUtenteArchivio()), , , txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, tipoRigetto)
                context.Items.Add("tipoApplic", 1)

            Case 2
                vettoredati = Elenco_Documenti(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, tipoRigetto)
                context.Items.Add("tipoApplic", 2)
        End Select
        context.Items.Add("vettoreDati", vettoredati)
    End Sub


End Class
