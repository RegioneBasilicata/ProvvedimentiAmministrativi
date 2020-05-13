Public Class WorklistCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object = Nothing
        Dim tipoApplic As String = getTipoAppl(context)
        Dim tipoDocumento As Integer = Integer.Parse(tipoApplic)

        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        Dim tipoRigetto As String = ""
        Dim idTipologiaDocumento As Integer
        Dim autorizzazionePubblicazione As String = ""

        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            txtDataInizio = Now.AddMonths(-12)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If

        If Not context.Session.Item("txtTipologiaDocumento") Is Nothing Then
            idTipologiaDocumento = context.Session.Item("txtTipologiaDocumento")
        Else
            idTipologiaDocumento = -1
        End If

        If Not context.Session.Item("txtAutorizPubblicazione") Is Nothing Then
            autorizzazionePubblicazione = context.Session.Item("txtAutorizPubblicazione")
        Else
            autorizzazionePubblicazione = Nothing
        End If

        Dim txtOggettoRicerca As String = "" & context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = "" & context.Session.Item("txtCodUfficio")
        Dim txtNumero As String = "" & context.Session("txtNumero")
        tipoRigetto = "" & context.Session("TipoRigetto")
        Dim tipologiaRicercaBeneficiario = "" & context.Session("TipologiaRicercaBeneficiario")
        Dim beneficiario = "" & context.Session("FiltroRicercaBeneficiario")
        Dim tipologiaRicercaDestinatario = "" & context.Session("TipologiaRicercaDestinatario")
        Dim destinatario = "" & context.Session("FiltroRicercaDestinatario")
        Dim txtCodiceCUP As String = "" & context.Session.Item("txtCodiceCUP")
        Dim txtCodiceCIG As String = "" & context.Session.Item("txtCodiceCIG")
        Dim visualizzaUrgenti As Boolean = False
        Dim visualizzaNonTrasp As Boolean = False
        If Not String.IsNullOrEmpty(context.Request.QueryString("visualizzaUrgenti")) Then
            visualizzaUrgenti = context.Request.QueryString("visualizzaUrgenti")
        End If
        If Not String.IsNullOrEmpty(context.Request.QueryString("visualizzaNonTrasp")) Then
            visualizzaNonTrasp = context.Request.QueryString("visualizzaNonTrasp")
        End If
        If context.Session.Item("txtCodDipartimenti") Is Nothing OrElse String.IsNullOrEmpty(context.Session.Item("txtCodDipartimenti")) Then
            If Not String.IsNullOrEmpty(context.Request.QueryString("dip")) Then
                context.Session.Add("txtCodDipartimenti", context.Request.QueryString("dip"))
            Else
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



        Else

        End If

        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")

	 Select Case UCase(tipoApplic)
	   Case 1 ' Delibere
                vettoredati = Elenco_Documenti(1,,Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, tipoRigetto)
                context.Items.Add("tipoApplic", 1)
	Case Else	
        	vettoredati = Elenco_Documenti(tipoDocumento,, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtCodDipartimento, txtNumero, tipoRigetto, , tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, visualizzaUrgenti, visualizzaNonTrasp, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario)
        	context.Items.Add("tipoApplic", tipoDocumento)
	End Select
        context.Items.Add("vettoreDati", vettoredati)
    End Sub


    Private Function getTipoAppl(ByVal context As HttpContext) As String
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        If String.IsNullOrEmpty(tipoApplic) Then
            tipoApplic = context.Session.Item("tipoApplic")
        End If
        Return tipoApplic
    End Function
End Class
