Public Class svrDelibereBase
    Inherits svrDocumenti

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Overrides Function Elabora(ByVal cFunzione As Integer, ByVal vparm As Object, Optional ByVal controllo As Integer = 0, Optional ByVal FlagChiudiTutte As Boolean = True, Optional ByVal destinatarioInoltro As Integer = -1) As Object
        MyBase.Elabora(cFunzione, vparm, controllo, FlagChiudiTutte)

        Dim vRo As Object
        Dim Ritorno As Integer

        If UBound(vparm) > 1 Then
            Ritorno = vparm(2)
        Else
            Ritorno = 0
        End If

        Select Case cFunzione

            Case Dic_FODocumentale.cfo_Crea_Delibera
                Elabora = FO_Crea_Delibera(vparm(1))
            Case Dic_FODocumentale.cfo_Passo_Delibera
                Elabora = FO_Passo_Delibera(vparm(1))
            Case Dic_FODocumentale.cfo_Seduta_Giunta
                Elabora = FO_Seduta_Giunta(vparm(1))

            Case Else
                Elabora = New Object() {777777, "Funzione Non Più in Uso, Consultare il Fornitore del Prodotto"}
        End Select

FineSub:
        On Error Resume Next
        If IsArray(vparm) Then
            Erase vparm
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Elabora-Fine")
        End If

    End Function

    Public Function FO_Seduta_Giunta(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Seduta_Giunta"
        Dim vRitPar(3) As Object
        Dim vRitSql As Object = Nothing
        Dim Sqlq As String
        Dim sWhere As String
        Dim DB As Object = Nothing
        Dim vR As Object = Nothing
        Dim vP(2) As Object

        Dim vIstanzaWFE As Object = Nothing
        Dim sInIstanzaDet As New System.Text.StringBuilder
        Dim i As Integer
        Dim j As Integer

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If


        vR = WF_Seduta_Giunta()

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)
            GoTo FineSub
        End If

        sInIstanzaDet.Remove(0, sInIstanzaDet.Length)
        vIstanzaWFE = vR(1)
        For i = 0 To UBound(vIstanzaWFE, 2)
            If Trim(vIstanzaWFE(0, i)) <> "" Then
                If sInIstanzaDet.Length = 0 Then
                    sInIstanzaDet.Remove(0, sInIstanzaDet.Length)
                    sInIstanzaDet.Append("'" & Trim(vIstanzaWFE(0, i)) & "'")
                Else
                    sInIstanzaDet = sInIstanzaDet.Append(",'" & Trim(vIstanzaWFE(0, i)) & "'")
                End If
            End If
        Next
        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
        '  DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

        Call DB.ApriDB()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo FineSub
        End If

        'modgg 12-06 5
        Sqlq = " SELECT DISTINCT Doc_Id ,Doc_numero ,doc_Data , " & _
              "         dbo.fn_ReplaceCaratteriSpeciali(isnull(Doc_Oggetto,'')) as Doc_Oggetto , Doc_id_WFE ,  Doc_Cod_Uff_Prop   " & _
              " FROM fn_ElencoDocumenti('" & sInIstanzaDet.Replace("'", "''").ToString & "',1, '" & Format(Now(), "MM/dd/yyyy") & "') " & _
              " INNER JOIN Azioni_Utente_Documento ON fn_ElencoDocumenti.Doc_Id = Azioni_Utente_Documento.Sto_id_Doc " & _
              " WHERE  (Azioni_Utente_Documento.Sto_Utente = '" & oOperatore.Codice & "')"

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)
            GoTo FineSub
        End If

        Dim vRisultati(8, UBound(vR(1), 2)) As String

        For i = 0 To UBound(vR(1), 2)
            vRisultati(0, i) = vR(1)(0, i) & ""
            vRisultati(1, i) = vR(1)(1, i) & ""
            vRisultati(2, i) = vR(1)(2, i) & ""
            vRisultati(3, i) = vR(1)(3, i) & ""
            vRisultati(4, i) = vR(1)(4, i) & ""
            vRisultati(5, i) = ""
            For j = 0 To UBound(vIstanzaWFE, 2)
                If vIstanzaWFE(0, j) = vRisultati(4, i) Then
                    vRisultati(5, i) = vIstanzaWFE(1, j) & ""
                End If
            Next
            vRisultati(6, i) = vR(1)(5, i) & ""
            vRisultati(8, i) = ""
        Next


        vRitPar(0) = 0
        vRitPar(1) = vRisultati

FineSub:
        FO_Seduta_Giunta = vRitPar
        On Error Resume Next
        If Not DB Is Nothing Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        On Error GoTo 0
        Exit Function

Herr:
        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description
        Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
        ' Resume
    End Function

    Private Function FO_Crea_Delibera(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Crea_Delibera"
        Dim vRitPar(2) As Object
        Dim vRitSql As Object = Nothing
        Dim vR As Object = Nothing
        Dim Sqlq As String
        Dim sWhere As String
        Dim DB As Object = Nothing
        Dim RS As Object = Nothing
        Dim vP(2) As Object

        Dim codUfficioProponente As String
        Dim utenteCreazione As String
        Dim dataCreazione As Date
        Dim idIstanzaWEF As String
        Dim numProvvisorio As String
        Dim dirD As String
        Dim dirU As String
        Dim idDocumentoLocale As String

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        vRitPar(2) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If

        codUfficioProponente = vParm(Dic_FODocumentale.vc_Crea_Determina.c_cod_ufficio_proponente)
        utenteCreazione = vParm(Dic_FODocumentale.vc_Crea_Determina.c_utente_creazione) & ""
        If IsDate(vParm(Dic_FODocumentale.vc_Crea_Determina.c_data_creazione)) Then
            dataCreazione = CDate(vParm(Dic_FODocumentale.vc_Crea_Determina.c_data_creazione))
        Else
            dataCreazione = Now.Date
        End If

        If Trim(utenteCreazione) = "" Then
            vRitPar(0) = 1
            vRitPar(1) = "Parametri insufficienti, " + SFunzione
            GoTo FineSub
        End If
        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

        Call DB.ApriDB()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo FineSub
        End If

        'verifico se sono un ufficio proponente

        Sqlq = "SELECT     Sta_id, Sta_attributo, Sta_valore " & _
               " FROM         Struttura_Attributi " & _
               " WHERE       (Sta_procedura = 'DELIBERE'  or Sta_procedura = '*' ) AND (Sta_attributo = 'UFFICIO_PROPONENTE') " & _
               "           AND ((Sta_id = '" & codUfficioProponente & "') or (Sta_id = '*'))"

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2

        vR = GDB.DBQuery(vP)
        If vR(0) <> 0 Then
            vRitPar(0) = 1
            vRitPar(1) = "L'ufficio non è abilitato alla creazione delle determine"
            GoTo FineSub
        End If

        dirD = "UP"

        'verifico se non sono l'ufficio dirigenza o ragioneria
        Sqlq = "Select  Sta_attributo " & _
               " FROM         Struttura_Attributi " & _
               "  WHERE (Sta_id = '" & codUfficioProponente & "') " & _
               "        AND (Sta_valore = '1' " & _
               "              and (Sta_attributo = 'UFFICIO_DIRIGENZA_DIPARTIMENTO' OR  Sta_attributo = 'UFFICIO_RAGIONERIA' OR  Sta_attributo = 'UFFICIO_SEGRETERIA'  OR  Sta_attributo = 'UFFICIO_SEGR_PRESIDENZA_LEGITTIMITA')) "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)
        If vR(0) = 0 Then
            If UCase(vR(1)(0, 0)) = "UFFICIO_DIRIGENZA_DIPARTIMENTO" Then
                dirD = "UDD"
            End If
         
        End If

        dirU = "C"

        Sqlq = "(SELECT     TAB_Operatori_Attributi.Toa_Valore " & _
               "   FROM TAB_Operatori_Attributi " & _
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (TAB_Operatori_Attributi.TOA_Operatore = '" & utenteCreazione & "') " & _
               " and ( TOA_Procedura = 'DELIBERE'  or  TOA_Procedura= '*' ) ) " & _
               " UNION " & _
               "(SELECT     TAB_Operatori_Attributi.Toa_Valore " & _
               " FROM         TAB_Operatori_Attributi INNER JOIN " & _
               "            Tab_Operatori_Gruppi ON TAB_Operatori_Attributi.TOA_Operatore = Tab_Operatori_Gruppi.TOG_Gruppo " & _
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (Tab_Operatori_Gruppi.TOG_Operatore = '" & utenteCreazione & "')    " & _
               " and ( TOA_Procedura = 'DELIBERE'  or  TOA_Procedura= '*' )) "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)
        If vR(0) = 0 Then
            If Trim(vR(1)(0, 0)) <> "" Then
                dirU = UCase(Trim(vR(1)(0, 0)))
            End If
        End If


        'creare il contatore
        vR = Calcola_Progressivo(DB, "CDOC", Year(Now), False)

        If vR(0) = 0 Then
            idDocumentoLocale = vR(1)
        Else
            vRitPar(0) = "1"
            vRitPar(1) = "Problemi nella creazione del contatore - " & vR(1)
            GoTo FineSub
        End If

        'istanziare il processo nel WEF
        tipoDocumento = 1
        vR = Istanzia_Flusso_Documento(dirD, dirU, utenteCreazione, idDocumentoLocale)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = "Problemi con il WorkFlow Engine - " + vR(1)
            GoTo FineSub
        End If

        idIstanzaWEF = Trim(vR(1))

        'registrare i dati su DB
        Call DB.BeginTrans()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        vR = Calcola_Progressivo(DB, "NPDL", Year(Now), False, codUfficioProponente)

        If vR(0) = 0 Then
            numProvvisorio = vR(1)
        Else
            vRitPar(0) = "1"
            vRitPar(1) = "Problemi nella creazione del contatore - " & vR(1)
            GoTo RollTrans
        End If

        Sqlq = " SELECT     Doc_Id,  Doc_numeroProvvisorio, Doc_numero, Doc_Oggetto, Doc_Cod_Uff_Prop, Doc_Data, Doc_Tipo, Doc_Codice_Documento, " & _
               "            Doc_Stato, Doc_liquidazione, Doc_Contabile, Doc_Pubblicazione, Doc_Testo, Doc_id_WFE , " & _
               "            Doc_utenteCreazione,  Doc_operatore, Doc_dataRegistrazione " & _
               " FROM   Documento " & _
               " WHERE Doc_Id = '" & idDocumentoLocale & "' "

        RS = DB.ApriRS(Sqlq, DllGestDBNET.clDicGDB.dbCostAperturaRs.dbAperturaKeyset, DllGestDBNET.clDicGDB.dbCostLock.dbLockPessimistico)
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        If RS.EOF Then
            Call DB.AddRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo RollTrans
            End If
            RS("Doc_Id").Value = idDocumentoLocale
            RS("Doc_numeroProvvisorio").Value = numProvvisorio
        Else
            Call DB.EditRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo RollTrans
            End If
        End If

        RS("Doc_Cod_Uff_Prop").Value = codUfficioProponente
        RS("Doc_Data").Value = dataCreazione
        RS("Doc_id_WFE").Value = idIstanzaWEF
        RS("Doc_Tipo").Value = tipoDocumento
        RS("Doc_utenteCreazione").Value = oOperatore.Codice
        RS("Doc_operatore").Value = oOperatore.Codice
        RS("Doc_dataRegistrazione").Value = Now

        Call DB.UpdateRS(RS)
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        'registro la creazione della delibera tra le attività dell'utente
        Dim vRegAtt(Dic_FODocumentale.dimvc_Registra_Attivita) As Object
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_id_documento) = idDocumentoLocale
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_tipoAttivita) = "CREAZIONE"
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_nominativo) = oOperatore.Cognome & " " & oOperatore.Nome
        vR = FO_Registra_Attivita(vRegAtt, DB, False)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)

            GoTo RollTrans
        End If

        Call DB.CommitTrans()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        vRitPar(0) = 0
        vRitPar(1) = numProvvisorio
        vRitPar(2) = idDocumentoLocale
FineSub:
        FO_Crea_Delibera = vRitPar
        On Error Resume Next
        If Not DB Is Nothing Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        On Error GoTo 0
        Exit Function

RollTrans:
        If Not DB Is Nothing Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
            End If
        End If
        GoTo FineSub

Herr:

        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description
        Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
        GoTo RollTrans
        ' Resume
    End Function

    Private Function FO_Passo_Delibera(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Passo_Delibera"
        Dim vRitPar(3) As Object
        Dim vRitSql As Object = Nothing
        Dim Sqlq As String
        Dim sWhere As String
        Dim DB As Object = Nothing
        Dim vR As Object = Nothing
        Dim vP(2) As Object

        Dim idWorkitem As String
        Dim i As Integer
        Dim xmlDati As String
        Dim numeroDefinitivo As String

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If

        '  utente.Codice = vParm(Dic_FODocumentale.vc_Passo_Determina.c_utente) & ""

        idDocumento = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_idDocumento)
        azione = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_azione) & ""
        prossimoAttore = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_prossimoAttore) & ""
        xmlDati = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_datiXml) & ""
        note = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_note) & ""

        Dim suggerimento As Integer
        suggerimento = vParm(Dic_FODocumentale.vc_Passo_Delibera.c_suggerimento)

        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

        Call DB.ApriDB()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo FineSub
        End If


        Sqlq = "SELECT      Doc_id_WFE " & _
               " FROM         Documento " & _
               " WHERE (Doc_Id = '" & idDocumento & "') "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)
            GoTo FineSub
        End If

        idIstanzaWFE = vR(1)(0, 0)

        'leggo lo stato dal WFE
        attore = oOperatore.Codice
        If Trim(UCase(azione)) = "PRELIEVO" Then
            attore = oOperatore.oUfficio.CodUfficio
        End If
        If Trim(UCase(azione)) = "PRELAZIONE" Then
            attore = ""
        End If

        vR = Leggi_StatoIstanza_WFE(idIstanzaWFE, attore)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)
            GoTo RollTrans
        End If

        nodoFlussoDetermina = vR(1)(0) & ""
        nodoFlussoUfficio = vR(1)(1) & ""
        attore = vR(1)(2) & ""
        idWorkitem = vR(1)(3) & ""

        If Not Parametri_Completi() Then
            vRitPar(0) = Errore
            vRitPar(1) = ErrDesc
            GoTo FineSub
        End If

        'leggo le azioni da fare in questo passo
        Sqlq = "SELECT     Awf_attivita, Awf_nomeAttivita, Awf_nomeOggetto, Awf_dataBase, Awf_parametri " & _
               " FROM Azioni_Passo_WorkFlow " & _
               " WHERE    (Awf_flusso = 'DELIBERE' )  " & _
               "          AND (Awf_processo = '" & nodoFlussoDetermina & "') " & _
               "           AND (Awf_sottoProcesso = '" & nodoFlussoUfficio & "') " & _
               "           AND (Awf_azione = '" & azione & "') " & _
               " ORDER BY Awf_ordine "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = vR(1)
            GoTo FineSub
        End If

        Attivita = vR(1)

        'eseguo i passi
        i = 0
        While Errore = 0 And i <= nAttivita
            Esegui_Attivita(i)
            i = i + 1
        End While

        If Errore <> 0 Then
            vRitPar(0) = Errore
            vRitPar(1) = ErrDesc
            GoTo FineSub
        End If

        If Not Passo_Possibile() Then
            vRitPar(0) = Errore
            vRitPar(1) = ErrDesc
            GoTo FineSub
        End If

        If attore <> "" Then
            Dim attoreTEmp As New DllAmbiente.Operatore
            attoreTEmp.pCodice = attore
            pDescrizioneAttore = attoreTEmp.Cognome & " " & attoreTEmp.Nome
        End If

        Dim utenteTEmp As New DllAmbiente.Operatore
        utenteTEmp.pCodice = pProssimoAttore
        pDescrizioneProssimoAttore = utenteTEmp.Cognome & " " & utenteTEmp.Nome


        If Trim(pDescrizioneProssimoAttore) = "" Then
            Dim ufficioTEmp As New DllAmbiente.Ufficio
            ufficioTEmp.CodUfficio = pProssimoAttore
            pDescrizioneProssimoAttore = ufficioTEmp.DescrUfficioBreve
        End If

        If azione = "PRELIEVO" Or azione = "PRELAZIONE" Then
            pDescrizioneAttore = pDescrizioneProssimoAttore
        End If

        Dim vRegAtt(Dic_FODocumentale.dimvc_Registra_Attivita) As Object
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_tipoAttivita) = UCase(azione)
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_info_attivita) = ""
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_id_documento) = idDocumento


        If AggiornaPassoDelibera(vRegAtt, azione, idIstanzaWFE, idWorkitem, pNodoFlussoDetermina, pNodoFlussoUfficio, attore, pDirD, pDirU, pProssimoAttore, suggerimento) Then

            vRitPar(0) = 0
            vRitPar(1) = ""
        Else

            vRitPar(0) = 9999
            vRitPar(1) = "Impossibile Eseguire il passo"

        End If

FineSub:
        FO_Passo_Delibera = vRitPar
        On Error Resume Next
        If Not DB Is Nothing Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        On Error GoTo 0
        Exit Function

RollTrans:
        If Not DB Is Nothing Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
            End If
        End If
        GoTo FineSub

Herr:

        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description
        Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
        GoTo RollTrans
        ' Resume
    End Function


    Private Function WF_Seduta_Giunta() As Object
        Dim xmlParam As String
        Dim xmlRit As String
        Dim vXmlRit As Array
        Dim vRit(1) As Object

        Dim vElencoIstanzaWFE() As String
        Dim sElencoIstanzaWFE As String
        Dim sIstanza() As String
        Dim i As Integer

        Try
            vRit(0) = 0
            vRit(1) = ""

            xmlParam = "xmlParam=seduta_giunta##"

            'xmlParam = Replace(xmlParam, "%utente%", LCase(utente), , , CompareMethod.Text)

            xmlRit = STRUMENTI_Chiamate.ChiamaWes(WESZOPE, xmlParam)

            vXmlRit = Split(xmlRit + "#", "#")

            If vXmlRit(0) <> 0 Then
                vRit(0) = vXmlRit(0)
                vRit(1) = vXmlRit(1)
                Exit Try
            End If

            sElencoIstanzaWFE = vXmlRit(1)
            vElencoIstanzaWFE = Split(sElencoIstanzaWFE, "$")

            Dim vvElencoIstanzaWFE(1, vElencoIstanzaWFE.Length - 1) As String
            For i = 0 To vElencoIstanzaWFE.Length - 1
                sIstanza = Split(vElencoIstanzaWFE(i) + "||", "|")
                vvElencoIstanzaWFE(0, i) = sIstanza(0)
                vvElencoIstanzaWFE(1, i) = sIstanza(1)
            Next

            vRit(1) = vvElencoIstanzaWFE

        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            WF_Seduta_Giunta = vRit
        End Try

    End Function


    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrDelibere" Then
            'chiamata ad una funzione dell'oggetto PassoDetermina
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub



    Overridable Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try

        Catch ex As Exception
            Errore = Err.Number
            ErrDesc = Err.Description
        End Try
    End Sub

    'modgg 24-05-2006
    Friend Sub UPDATE_PER_ADW(ByVal iddocumento As String)
        Const SFunzione As String = "UPDATE_PER_ADW"
        Dim vRitPar(3) As Object
        Dim DB As Object
        Dim vP(2) As Object
        Dim codiceRubrica As String
        Dim numeroProtocolloModificato As String = ""
        Dim generazioneNumero As String = ""
        Try
            vRitPar(0) = 0
            vRitPar(1) = ""
            If SISTEMA.bTRACE Then
                Call SISTEMA.Registra_Trace("Inizio", SFunzione)
            End If
            VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
            'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

            Call DB.ApriDB()
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo FineSub
            End If

            Call DB.BeginTrans()
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo RollTrans
            End If

            Dim ritorno As Object = Leggi_Attributo("Codice Rubrica Protocollo", oOperatore.oUfficio.CodUfficio, "ADW")
            If ritorno(0) = 0 Then
                codiceRubrica = ritorno(1)
            Else
                GoTo RollTrans
            End If
            Salva_Stati_Adw("1", "1", DB, "0")
            Salva_Stati_Adw("2", "1", DB, "1")
            Salva_Relatore_Adw(DB)
            Dim oggetto As String = ""
            Dim numeroDocDef As String = ""
            Dim annotazioni As String = ""


            Dim vettoreDati(1) As Object
            vettoreDati(0) = iddocumento
            vettoreDati(1) = "<datiDocumento>" & _
                             " <tabella nome_tabella=""Documento_noteosservazioni""" & _
                             " chiave_tabella = ""Dno_id_documento""" & _
                             " col_prog_registrazione = ""Dno_prog""" & _
                             " col_data_registrazione = ""Dno_DataRegistrazione""" & _
                             " col_operatore_registrazione=""Dno_Oparatore""> " & _
                             " <Dno_testo></Dno_testo>" & _
                             "</tabella>" & _
                             "</datiDocumento>"


            ritorno = FO_Leggi_Documento(vettoreDati, False, DB)
            If ritorno(0) = 0 Then
                numeroDocDef = ritorno(1)(3)
                oggetto = ritorno(1)(0)
                Dim domDatiRichiesta As Xml.XmlDocument = New Xml.XmlDocument
                If Trim(ritorno(1)(2)) <> "" Then
                    domDatiRichiesta.LoadXml(ritorno(1)(2) & "")
                    Dim nodo As Xml.XmlNode = domDatiRichiesta.SelectSingleNode("//Dno_testo")
                    annotazioni = nodo.InnerText
                End If
                domDatiRichiesta = Nothing
            End If

            ritorno = Calcola_Progressivo(DB, "WSP", Year(Now), False)
            Dim idComunicazione As String
            If ritorno(0) = 0 Then
                idComunicazione = ritorno(1)
            End If

            ritorno = Leggi_Attributo("GENERAZIONE_NUMERO_PROTOCOLLO", "*", "ATTIDIGITALI")
            If ritorno(0) = 0 Then
                generazioneNumero = ritorno(1)
            End If
            Select Case UCase(generazioneNumero)
                Case "WS"
                    Dim sRichiesta As String = MessageMaker.createProtocolloMessage(numeroDocDef, oggetto, codiceRubrica, annotazioni, idComunicazione)
                    Dim servizioProtocollo As New Protocollows.ProtocolloWs
                    Dim sRisposta As String = servizioProtocollo.Elabora(sRichiesta)
                    numeroProtocolloModificato = MessageMaker.RitornoNumeroProtocollo(sRisposta)
                Case "LOCAL"
                    ritorno = Calcola_Progressivo(DB, "ADDL", Year(Now), False)
                    If ritorno(0) = 0 Then
                        numeroProtocolloModificato = ritorno(1)
                    End If
                Case Else
                    numeroProtocolloModificato = ""
            End Select
            'modgg 10-03 3
            'ritorno = FO_Aggiorna_Documento(numeroProtocolloModificato, False, DB)
            Dim vParmObj(4) As Object
            vParmObj(0) = iddocumento
            vParmObj(1) = numeroProtocolloModificato
            vParmObj(2) = "Doc_numProtocollo"
            vParmObj(3) = False
            vParmObj(4) = DB

            ritorno = FO_Aggiorna_Documento(vParmObj)
            If ritorno(0) <> 0 Then
                GoTo Rolltrans
            End If
            Call DB.CommitTrans()
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo RollTrans
            End If
        Catch ex As Exception
            vRitPar(0) = Err.Number
            vRitPar(1) = Err.Description
            Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
            GoTo RollTrans
        End Try

        vRitPar(0) = 0
        vRitPar(1) = ""

FineSub:

        Errore = vRitPar(0)
        ErrDesc = vRitPar(1)
        If Not DB Is Nothing Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        Exit Sub

RollTrans:
        If Not DB Is Nothing Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
            End If
        End If
        GoTo FineSub

Herr:

        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description
        Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
        GoTo RollTrans
        ' Resume
    End Sub
    Private Function Salva_Stati_Adw(ByVal idStato As String, ByVal idEvento As String, Optional ByVal DB As Object = Nothing, Optional ByVal statoCorrente As String = "1") As Object
        Const SFunzione As String = "Salva_Stati_Adw"
        Dim vRitPar(3) As Object
        Dim Sqlq As String
        Dim RS As Object
        Dim vP(2) As Object
        Dim chiudereTrans As Boolean = False

        Try
            vRitPar(0) = 0
            vRitPar(1) = ""
            If SISTEMA.bTRACE Then
                Call SISTEMA.Registra_Trace("Inizio", SFunzione)
            End If

            If DB Is Nothing Then
                chiudereTrans = True
                VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
                'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

                Call DB.ApriDB()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo FineSub
                End If

                Call DB.BeginTrans()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo RollTrans
                End If
            End If

            Sqlq = " SELECT     Dsa_Id_Documento, Dsa_Id_Stato, Dsa_Prog, Dsa_IdEvento, Dsa_StatoCorrente, Dsa_Utente, Dsa_Operatore, Dsa_DataRegistrazione " & _
                   " FROM Documento_StatiAdw " & _
                   " WHERE     (Dsa_Id_Documento = '" & idDocumento & "')"

            RS = DB.ApriRS(Sqlq, DllGestDBNET.clDicGDB.dbCostAperturaRs.dbAperturaKeyset, DllGestDBNET.clDicGDB.dbCostLock.dbLockPessimistico)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
                'If DB.stato = 3 Then
                '    GoTo RollTrans
                'Else
                '    GoTo FineSub
                'End If
            End If

            Call DB.AddRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
                'If DB.stato = 3 Then
                '    GoTo RollTrans
                'Else
                '    GoTo FineSub
                'End If
            End If
            RS("Dsa_Id_Documento") = idDocumento
            RS("Dsa_Id_Stato") = idStato
            RS("Dsa_Prog") = IIf(RS("Dsa_Prog").value Is Nothing, 1, RS("Dsa_Prog").value + 1)
            RS("Dsa_IdEvento") = idEvento
            RS("Dsa_StatoCorrente") = statoCorrente
            RS("Dsa_Utente") = oOperatore.Codice
            RS("Dsa_Operatore") = oOperatore.Codice
            RS("Dsa_DataRegistrazione") = Now

            Call DB.UpdateRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
                'If DB.stato = 3 Then
                '    GoTo RollTrans
                'Else
                '    GoTo FineSub
                'End If
            End If

            If DB.stato = 3 And chiudereTrans Then
                Call DB.CommitTrans()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo RollTrans
                End If
            End If

            vRitPar(0) = 0
            vRitPar(1) = ""

        Catch ex As Exception
            vRitPar(0) = Err.Number
            vRitPar(1) = Err.Description
            Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
            If DB.stato = 3 And chiudereTrans Then
                Call DB.RollTrans()
                If DB.errore <> 0 Then
                    Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
                End If
            Else
                If Not DB Is Nothing Then
                    Call DB.ChiudiDB()
                    DB = Nothing
                End If
            End If
            Throw New Exception(ex.Message)
        End Try

FineSub:
        Errore = vRitPar(0)
        ErrDesc = vRitPar(1)
        If Not DB Is Nothing And chiudereTrans Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        Exit Function

RollTrans:
        If Not DB Is Nothing And chiudereTrans Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
            End If
        End If
        GoTo FineSub

    End Function
    Private Function Salva_Relatore_Adw(Optional ByVal DB As Object = Nothing) As Object
        Const SFunzione As String = "Salva_Relatore_Adw"
        Dim vRitPar(3) As Object
        Dim Sqlq As String
        Dim RS As Object
        Dim vP(2) As Object
        Dim chiudereTrans As Boolean = False

        Try
            vRitPar(0) = 0
            vRitPar(1) = ""
            If SISTEMA.bTRACE Then
                Call SISTEMA.Registra_Trace("Inizio", SFunzione)
            End If

            If DB Is Nothing Then
                chiudereTrans = True
                VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
                'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

                Call DB.ApriDB()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo FineSub
                End If

                Call DB.BeginTrans()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo RollTrans
                End If
            End If

            Sqlq = " SELECT     DAD_Id_Documento, DAD_Prog, DAD_DataRegistrazione, DAD_Relatore, DAD_Operatore " & _
                   " FROM Documento_accessori_delibere " & _
                   " WHERE     ( DAD_Id_Documento = '" & idDocumento & "')"

            RS = DB.ApriRS(Sqlq, DllGestDBNET.clDicGDB.dbCostAperturaRs.dbAperturaKeyset, DllGestDBNET.clDicGDB.dbCostLock.dbLockPessimistico)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
                'If DB.stato = 3 Then
                '    GoTo RollTrans
                'Else
                '    GoTo FineSub
                'End If
            End If

            Call DB.AddRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
            End If
            RS("DAD_Id_Documento") = idDocumento
            RS("DAD_Relatore") = "ATTIDIGITALI"
            RS("DAD_Prog") = IIf(RS("DAD_Prog").value Is Nothing, 1, RS("DAD_Prog").value + 1)
            RS("DAD_DataRegistrazione") = Now
            RS("DAD_Operatore") = oOperatore.Codice

            Call DB.UpdateRS(RS)
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                Throw New Exception("Update non riuscito" & vRitPar(0) & "-" & vRitPar(1))
            End If

            If DB.stato = 3 And chiudereTrans Then
                Call DB.CommitTrans()
                If DB.errore <> 0 Then
                    vRitPar(0) = DB.errore
                    vRitPar(1) = DB.ErrDescr
                    GoTo RollTrans
                End If
            End If

            vRitPar(0) = 0
            vRitPar(1) = ""

        Catch ex As Exception
            vRitPar(0) = Err.Number
            vRitPar(1) = Err.Description
            Call SISTEMA.Registra_Log(vRitPar(1) & " ", SFunzione)
            If DB.stato = 3 And chiudereTrans Then
                Call DB.RollTrans()
                If DB.errore <> 0 Then
                    Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
                End If
            Else
                If Not DB Is Nothing Then
                    Call DB.ChiudiDB()
                    DB = Nothing
                End If
            End If
            Throw New Exception(ex.Message)
        End Try

FineSub:
        Errore = vRitPar(0)
        ErrDesc = vRitPar(1)
        If Not DB Is Nothing And chiudereTrans Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        Exit Function

RollTrans:
        If Not DB Is Nothing And chiudereTrans Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Call SISTEMA.Registra_Log(DB.ErrDescr, SFunzione)
            End If
        End If
        GoTo FineSub

    End Function
    Private Function Leggi_Attributo(ByVal attributo As String, Optional ByVal struttura As String = "*", Optional ByVal procedura As String = "*") As Object
        Const SFunzione As String = "Leggi_Attributo"
        Dim vRitPar(1) As Object
        Dim vR As Object = Nothing
        Dim Sqlq As String
        Dim DB As Object = Nothing
        Dim vP(2) As Object

        vRitPar(0) = 0
        vRitPar(1) = ""
        Try
            If SISTEMA.bTRACE Then
                Call SISTEMA.Registra_Trace("Inizio", SFunzione)
            End If

            VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
            'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

            Call DB.ApriDB()
            If DB.errore <> 0 Then
                vRitPar(0) = DB.errore
                vRitPar(1) = DB.ErrDescr
                GoTo FineSub
            End If

            Sqlq = "SELECT     Sta_id, Sta_attributo, Sta_valore " & _
                   " FROM         Struttura_Attributi " & _
                   " WHERE       (Sta_attributo = '" & attributo & "') "

            If struttura = "*" Then
                Sqlq = Sqlq & " AND (Sta_id = '*') "
            Else
                Sqlq = Sqlq & "AND ((Sta_id = '" & struttura & "') or (Sta_id = '*')) "
            End If

            If procedura = "*" Then
                Sqlq = Sqlq & " AND (Sta_procedura= '*') "
            Else
                Sqlq = Sqlq & "AND ((Sta_procedura = '" & procedura & "') or (Sta_procedura = '*')) "
            End If

            vP(0) = DB
            vP(1) = Sqlq
            vP(2) = 2

            vR = GDB.DBQuery(vP)
            If vR(0) <> 0 Then
                vRitPar(0) = 1
                vRitPar(1) = "La struttura non dispone dell'attributo richiesto"
                GoTo FineSub
            End If
            vRitPar(0) = 0
            vRitPar(1) = vR(1)(2, 0)
        Catch ex As Exception
            vRitPar(0) = Err.Number
            vRitPar(1) = Err.Description
            GoTo FineSub
        End Try
FineSub:
        Leggi_Attributo = vRitPar
        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If
        Exit Function

    End Function

    Function AggiornaPassoDelibera(ByVal vRegAtt As Object, ByVal azione As String, ByVal idIstanzaWFE As String, ByVal idWorkitem As String, ByVal pNodoFlussoDelibera As String, ByVal pNodoFlussoUfficio As String, ByVal attore As String, ByVal pDirD As String, ByVal pDirU As String, ByVal pProssimoAttore As String, ByVal suggerimento As Integer) As Boolean
        Dim result As Boolean = False

        Dim conn As SqlClient.SqlConnection = Nothing
        Dim trans As SqlClient.SqlTransaction = Nothing
        Try

            conn = New SqlClient.SqlConnection(Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE"))
            conn.Open()
            trans = conn.BeginTransaction

            If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And suggerimento > 0 Then
                Registra_Suggerimento_Documento(suggerimento, idIstanzaWFE, note, trans)
            End If

            FO_AGGIORNA_DATA_RICEZIONE_new(vRegAtt, trans)
            FO_Registra_Attivita_new(vRegAtt, trans)

            If UCase(azione) = "ASSEGNA" Then
                If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
                    Dim utenteDestinatario As New DllAmbiente.Operatore
                    utenteDestinatario.pCodice = pProssimoAttore
                    If utenteDestinatario.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                        pDirU = "S"
                    End If
                    utenteDestinatario = Nothing
                End If
            End If
            
            Passo_Documento(idIstanzaWFE, idWorkitem, pNodoFlussoDelibera, pNodoFlussoUfficio, attore, pDirD, pDirU, pProssimoAttore)

            trans.Commit()
            result = True
        Catch ex As Exception
            result = False
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

        Return result
    End Function
End Class
