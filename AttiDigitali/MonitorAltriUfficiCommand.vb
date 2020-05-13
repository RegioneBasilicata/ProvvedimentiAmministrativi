''' <summary>
''' Questa funzione viene chiamata in Archivio e Worklist Print User
''' </summary>
''' <remarks></remarks>
Public Class MonitorAltriUfficiCommand
    Implements ICommand

    Public oOperatore As DllAmbiente.Operatore

    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim responseMessage As String = ""
        oOperatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Try
            OnExecute(context)

            'acquisisco la url da richiamare ricercando nei mappings configurati nel Web.config
            Dim action As String = context.Request.Params("PATH_INFO")
            action = action.ToLower.Replace(ConfigurationManager.AppSettings("replaceKey").ToLower, "/AttiDigitali/")

            Dim url = ActionMappings.getInstance.mappings.Get(action + ".success")
            'controllo se c'è un default
            If url Is Nothing Then
                responseMessage = "{success:false,message:""" & "Unable to get action url. Check application web.config file." & """}     "
            Else
                Dim descrizioneDocumento As String = ""
               
                Dim successMsg As String = ""

                If Not context.Request.QueryString.ToString Is Nothing And context.Request.QueryString.ToString <> "" Then
                    url = url & "?" & context.Request.QueryString.ToString
               
                End If
                'url = url & "?key=" & context.Session.Item("key")

                responseMessage = "{success:true,message:""" & successMsg & """, link:""" & url & """}     "

            End If
        Catch ex As Exception
            responseMessage = "{success:false,message:""" & ex.Message & "<br/> " & "Correggere l'errore e ricaricare il documento." & """}     "
        End Try

        context.Response.Clear()
        context.Response.ClearHeaders()

        context.Response.Write(responseMessage)
        context.Response.Flush()
        context.Response.Close()
    End Sub



    Public Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object


        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim tipoData As Integer = context.Session.Item("radioData")
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
            ' txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-1)
            
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If

        Dim flagPrinterUser As Boolean = False
        Dim tipologiaRicercaBeneficiario = "" & context.Session("TipologiaRicercaBeneficiario")
        Dim beneficiario = "" & context.Session("FiltroRicercaBeneficiario")
        Dim tipologiaRicercaDestinatario = "" & context.Session("TipologiaRicercaDestinatario")
        Dim destinatario = "" & context.Session("FiltroRicercaDestinatario")
        Dim txtCodiceCUP As String = context.Session.Item("txtCodiceCUP")
        Dim txtCodiceCIG As String = context.Session.Item("txtCodiceCIG")

        Dim idTipologiaDocumento As Integer
        Dim autorizzazionePubblicazione As String

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
        Dim tipoRigetto As String = "" & context.Session("TipoRigetto")

        Dim visualizzaAnnullati As Boolean
        If context.Request.Item("visualizzaAnnullati") Is Nothing Then
            visualizzaAnnullati = False
        Else
            visualizzaAnnullati = context.Request.Item("visualizzaAnnullati")
        End If
        vettoredati = Consulta_documenti_AltriUffici(CInt(tipoApplic),tipoData, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, True, tipoRigetto, , , tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario, visualizzaAnnullati)
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

        context.Session.Add("vettoreDati", vettoredati)
        context.Session.Add("tipoApplic", tipoApplic)
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
        Dim tipologiaRicercaBeneficiario = "" & context.Session("TipologiaRicercaBeneficiario")
        Dim beneficiario = "" & context.Session("FiltroRicercaBeneficiario")
        Dim tipologiaRicercaDestinatario = "" & context.Session("TipologiaRicercaDestinatario")
        Dim destinatario = "" & context.Session("FiltroRicercaDestinatario")
        Dim idTipologiaDocumento As Integer
        Dim autorizzazionePubblicazione As String
        Dim tipoData As Integer = context.Session.Item("radioData")

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

        Dim tipoRigetto As String = "" & context.Session("TipoRigetto")

        Dim txtCodiceCUP As String = context.Session.Item("txtCodiceCUP")
        Dim txtCodiceCIG As String = context.Session.Item("txtCodiceCIG")

        Select Case UCase(tipoApplic)
            Case 0
                vettoredatiMonitor = Elenco_Monitor(0, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario,)
            Case 1
                vettoredatiMonitor = Elenco_Monitor(1, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario,tipoData)
            Case 2
                vettoredatiMonitor = Elenco_Monitor(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, , tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario,)
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
            txtDataInizio = Now.AddMonths(-1)
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

    Public Shared Function getDocumentiArchiviati(ByVal op As DllAmbiente.Operatore, ByVal tipoDoc As String, ByVal dataDa As String, ByVal dataA As String, ByVal codUff As String, ByVal codDip As String, ByVal oggettoDoc As String, ByVal numDoc As String) As Object
        Dim vettoredati As Object

        Dim _dataDa As Date
        Dim _dataA As Date

        If Not dataDa Is Nothing AndAlso Not dataDa.Trim() = String.Empty Then
            _dataDa = dataDa
        Else
            _dataDa = Now.AddMonths(-1)
        End If

        If Not dataA Is Nothing AndAlso Not dataA.Trim() = String.Empty Then
            _dataA = dataA
        Else
            _dataA = DateTime.Now
        End If

        If codDip Is Nothing OrElse codDip.Trim() = String.Empty Then
            If Not op.oUfficio.CodDipartimento Is Nothing And Not String.IsNullOrEmpty(op.oUfficio.CodDipartimento) Then
                codDip = op.oUfficio.CodDipartimento
            Else
                Dim hta As ArrayList = op.DipartimentoDipendenti(tipoDoc)
                If hta.Count > 0 Then
                    codDip = DirectCast(hta.Item(0), DllAmbiente.StrutturaInfo).CodiceInterno
                Else
                    Throw New Exception("L'operatore non è abilitato alla visualizzazione di alcun dipartimento")
                End If
            End If
        End If

        vettoredati = Consulta_documenti_AltriUffici(CInt(tipoDoc), Format(_dataDa, "dd/MM/yyyy"), Format(_dataA, "dd/MM/yyyy"), oggettoDoc, codUff, , codDip, numDoc, True)

        If vettoredati(0) = 0 Then
            Dim ufCons As ArrayList = op.oUfficio.UfficiConsultabili(tipoDoc, op.pCodice)
            Dim consultazione As Boolean = Not (ufCons Is Nothing OrElse ufCons.Count = 0)

            If Not consultazione Then
                Dim vettoredatiMonitor As Object

                Select Case UCase(tipoDoc)
                    Case 0
                        vettoredatiMonitor = Elenco_Monitor(0, Format(_dataDa, "dd/MM/yyyy"), Format(_dataA, "dd/MM/yyyy"), oggettoDoc, codUff, , codDip, numDoc)
                    Case 1
                        vettoredatiMonitor = Elenco_Monitor(1, Format(_dataDa, "dd/MM/yyyy"), Format(_dataA, "dd/MM/yyyy"), oggettoDoc, codUff, , codDip, numDoc)
                    Case 2
                        vettoredatiMonitor = Elenco_Monitor(2, Format(_dataDa, "dd/MM/yyyy"), Format(_dataA, "dd/MM/yyyy"), oggettoDoc, codUff, , codDip, numDoc)
                End Select

                Dim objTemp(UBound(vettoredati(1), 1), UBound(vettoredati(1), 2)) As Object

                Dim index As Integer = 0
                Dim found As Boolean = False

                If vettoredatiMonitor(0) = 0 Then
                    For i As Integer = 0 To UBound(vettoredati(1), 2)

                        For j As Integer = 0 To UBound(vettoredatiMonitor(1), 2)
                            found = False
                            If vettoredati(1)(0, i) = vettoredatiMonitor(1)(0, j) Then
                                found = True
                                Exit For
                            End If

                        Next

                        If found Then
                            objTemp(0, index) = vettoredati(1)(0, i)
                            objTemp(1, index) = vettoredati(1)(1, i)
                            objTemp(2, index) = vettoredati(1)(2, i)
                            objTemp(3, index) = vettoredati(1)(3, i)
                            objTemp(4, index) = vettoredati(1)(4, i)
                            objTemp(5, index) = vettoredati(1)(5, i)
                            objTemp(6, index) = vettoredati(1)(6, i)

                            index += 1
                        End If
                    Next
                End If

                ReDim Preserve objTemp(UBound(vettoredati(1), 1), index - 1)
                vettoredati(1) = objTemp
            End If
        End If

        Return vettoredati
    End Function
End Class
