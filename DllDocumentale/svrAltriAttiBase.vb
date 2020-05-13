Imports System.Configuration

'Imports DllSistemaNet.ClSistema  ' DllSistema.ClSistemaClass
'Imports DAO.DBEngineClass
'Imports DllGestDBNET  'DllGestDB
'Imports System.IO
'Imports System.Text
'Imports System.Xml
'Imports System.Xml.Schema
'Imports System.Xml.XmlReader

Public Class svrAltriAttiBase
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

            Case Dic_FODocumentale.cfo_Crea_AltroAtto '10
                Elabora = FO_Crea_AltroAtto(vparm(1))
            Case Dic_FODocumentale.cfo_Passo_AltroAtto  '15
                Elabora = FO_Passo_AltroAtto(vparm(1), destinatarioInoltro)
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

    Private Function FO_Crea_AltroAtto(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Crea_AltroAtto"
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
        Dim numProvvisorio As String
        Dim dirD As String
        Dim dirU As String
        Dim idDocumentoLocale As String
        Dim tipoAtto As Integer

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        vRitPar(2) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If

        codUfficioProponente = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_cod_ufficio_proponente)
        utenteCreazione = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_utente_creazione) & ""
        tipoAtto = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_tipo_atto) & ""
        If IsDate(vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_data_creazione)) Then
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

        Call DB.ApriDB()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo FineSub
        End If

        'leggo la direzione per la creazione
        'verifico se sono un ufficio proponente

        Sqlq = "SELECT     Sta_id, Sta_attributo, Sta_valore " &
               " FROM         Struttura_Attributi " &
               " WHERE       (Sta_procedura = 'DETERMINE' or Sta_procedura = '*') AND (Sta_attributo = 'UFFICIO_PROPONENTE') " &
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
        Sqlq = "Select  Sta_attributo " &
               " FROM         Struttura_Attributi " &
               "  WHERE (Sta_id = '" & codUfficioProponente & "') " &
               "        AND (Sta_valore = '1' " &
               "              and (Sta_attributo = 'UFFICIO_DIRIGENZA_DIPARTIMENTO' OR  Sta_attributo = 'UFFICIO_RAGIONERIA'  OR  Sta_attributo = 'UFFICIO_CONTROLLO_AMMINISTRATIVO')) "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)
        If vR(0) = 0 Then
            If vR(1)(0, 0) = "UFFICIO_DIRIGENZA_DIPARTIMENTO" Then
                dirD = "UDD"
            End If
        End If

        dirU = "C"

        Sqlq = "(SELECT     TAB_Operatori_Attributi.Toa_Valore " &
               "   FROM TAB_Operatori_Attributi " &
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (TAB_Operatori_Attributi.TOA_Operatore = '" & utenteCreazione & "') " &
               " and ( TOA_Procedura = 'DETERMINE'  or  TOA_Procedura= '*' ) ) " &
               " UNION " &
               "(SELECT     TAB_Operatori_Attributi.Toa_Valore " &
               " FROM         TAB_Operatori_Attributi INNER JOIN " &
               "            Tab_Operatori_Gruppi ON TAB_Operatori_Attributi.TOA_Operatore = Tab_Operatori_Gruppi.TOG_Gruppo " &
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (Tab_Operatori_Gruppi.TOG_Operatore = '" & utenteCreazione & "')    " &
               " and ( TOA_Procedura = 'DETERMINE'  or  TOA_Procedura= '*' ) ) "

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
        vR = Istanzia_Flusso_Documento(dirD, dirU, utenteCreazione, idDocumentoLocale)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = "Problemi con il WorkFlow Engine - " + vR(1)
            GoTo FineSub
        End If

        idIstanzaWFE = Trim(vR(1))

        'registrare i dati su DB
        Call DB.BeginTrans()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        Dim codiceProgressivo As String = ""
        If tipoAtto = 3 Then
            codiceProgressivo = "NPDR"
        ElseIf tipoAtto = 4 Then
            codiceProgressivo = "NPDO"
        End If
        vR = Calcola_Progressivo(DB, codiceProgressivo, Year(Now), False, codUfficioProponente)

        If vR(0) = 0 Then
            numProvvisorio = vR(1)
        Else
            vRitPar(0) = "1"
            vRitPar(1) = "Problemi nella creazione del contatore - " & vR(1)
            GoTo RollTrans
        End If

        Sqlq = " SELECT     Doc_Id,  Doc_numeroProvvisorio, Doc_numero, Doc_Oggetto, Doc_Cod_Uff_Prop, Doc_Data, Doc_Tipo, Doc_Codice_Documento, " &
               "            Doc_Stato, Doc_liquidazione, Doc_Contabile, Doc_Pubblicazione, Doc_Testo, Doc_id_WFE , " &
               "            Doc_utenteCreazione,  Doc_operatore, Doc_dataRegistrazione " &
               " FROM   Documento " &
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
        RS("Doc_id_WFE").Value = idIstanzaWFE
        RS("Doc_Tipo").Value = tipoAtto
        RS("Doc_utenteCreazione").Value = oOperatore.Codice
        RS("Doc_operatore").Value = oOperatore.Codice
        RS("Doc_dataRegistrazione").Value = Now

        Call DB.UpdateRS(RS)
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        'registro la creazione della determina tra le attività dell'utente
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
        FO_Crea_AltroAtto = vRitPar
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


    Private Function FO_Passo_AltroAtto(ByVal vParm As Object, Optional ByVal destinatarioInoltro As Integer = -1) As Object
        Const SFunzione As String = "FO_Passo_AltroAtto"

        Log.Info("***INIZIO FO_Passo_AltroAtto - idDocumento: " & vParm(Dic_FODocumentale.vc_Passo_Determina.c_idDocumento) & " azione: " & vParm(Dic_FODocumentale.vc_Passo_Determina.c_azione) & " " & Now)

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

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If
        '  Dim utente As New DllAmbiente.Operatore
        ' utente.Codice = vParm(Dic_FODocumentale.vc_Passo_Determina.c_utente) & ""

        idDocumento = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_idDocumento) & ""
        azione = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_azione) & ""
        prossimoAttore = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_prossimoAttore) & ""
        xmlDati = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_datiXml) & ""
        note = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_note) & ""

        Dim suggerimento As Integer
        suggerimento = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_suggerimento)


        Dim tipoAtto As Integer
        tipoAtto = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_tipoAtto)
        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

        Call DB.ApriDB()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo FineSub
        End If


        Sqlq = "SELECT      Doc_id_WFE " &
               " FROM         Documento " &
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
        If vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_utente) & "" = Me.getUtenteArchivio() And azione = "INOLTRO" Then
            attore = vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_utente)
        End If
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
            GoTo FineSub
        End If

        If attore <> "" Then
            If LCase(vR(1)(2)) & "" <> LCase(attore) Then
                vRitPar(0) = "1"
                vRitPar(1) = "L'utente non ha più in carico il documento"
                GoTo FineSub
            End If
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

        ' MEV 2.2012: Alcuni uffici (attualmente solo autorità di gestione) possono 
        ' scegliere di saltare la dirigenza del dipartimento. In questo caso la numerazione 
        ' del documento deve avvenire alla firma del dirigente proponente. 
        If oOperatore.oUfficio.Test_Attributo("SCEGLI_DEST_INOLTRO", True) _
            And UCase(nodoFlussoDetermina) = "UFFICIO_PROPONENTE" _
            And UCase(nodoFlussoUfficio) = "RESPONSABILE" _
            And destinatarioInoltro > 0 Then

            nodoFlussoDetermina = "UFFICIO_DIRIGENZA_DIPARTIMENTO"
        End If

        Dim nomeFlusso As String = ""
        If tipoAtto = 3 Then
            nomeFlusso = "DECRETI"
        ElseIf tipoAtto = 4 Then
            nomeFlusso = "ORDINANZE"
        End If


        'leggo le azioni da fare in questo passo
        Sqlq = "SELECT     Awf_attivita, Awf_nomeAttivita, Awf_nomeOggetto, Awf_dataBase , Awf_parametri " &
               " FROM Azioni_Passo_WorkFlow " &
               " WHERE    (Awf_flusso =  '" & nomeFlusso & "' )  " &
               "          AND (Awf_processo = '" & nodoFlussoDetermina & "') " &
               "           AND (Awf_sottoProcesso = '" & nodoFlussoUfficio & "') " &
               "           AND (Awf_azione = '" & azione & "') " &
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

        If (destinatarioInoltro <> -1) Then
            destinInoltro = destinatarioInoltro
        End If

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


        If AggiornaPassoAltriAtti(vRegAtt, azione, idIstanzaWFE, idWorkitem, pNodoFlussoDetermina, pNodoFlussoUfficio, attore, pDirD, pDirU, pProssimoAttore, suggerimento, ) Then

            vRitPar(0) = 0
            vRitPar(1) = ""
        Else

            vRitPar(0) = 9999
            vRitPar(1) = "Impossibile Eseguire il passo"

        End If
        'da verificare

        Log.Info("***FINE FO_Passo_AltroAtto - idDocumento: " & vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_idDocumento) & " azione: " & vParm(Dic_FODocumentale.vc_Passo_AltriAtti.c_azione) & " " & Now)

FineSub:
        FO_Passo_AltroAtto = vRitPar
        On Error Resume Next
        If Not DB Is Nothing Then
            Call DB.ChiudiDB()
            DB = Nothing
        End If

        If vRitPar(0) <> 0 Then
            Log.Info(vRitPar(1) & SFunzione)
        End If
        Log.Info("***FINE " & SFunzione)

        On Error GoTo 0
        Exit Function

RollTrans:
        If Not DB Is Nothing Then
            Call DB.RollTrans()
            If DB.errore <> 0 Then
                Log.Error(DB.ErrDescr & " " & SFunzione)
            End If
        End If
        GoTo FineSub

Herr:

        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description
        Log.Error(vRitPar(1) & " " & SFunzione)
        GoTo RollTrans
        ' Resume
    End Function
    Function AggiornaPassoAltriAtti(ByVal vRegAtt As Object, ByVal azione As String, ByVal idIstanzaWFE As String, ByVal idWorkitem As String, ByVal pNodoFlussoDetermina As String, ByVal pNodoFlussoUfficio As String, ByVal attore As String, ByVal pDirD As String, ByVal pDirU As String, ByVal pProssimoAttore As String, ByVal suggerimento As Integer, Optional ByVal flagUrgente As Boolean = False) As Boolean
        Dim result As Boolean = False
        Dim conn As SqlClient.SqlConnection = Nothing
        Dim trans As SqlClient.SqlTransaction = Nothing
        Try
            Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)
            conn = New SqlClient.SqlConnection(Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE"))
            conn.Open()
            trans = conn.BeginTransaction

            If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And suggerimento > 0 Then
                Registra_Suggerimento_Documento(suggerimento, idIstanzaWFE, note, trans)
            End If


            If UCase(azione) = "PRELIEVO" Then
            Else
                If oOperatore.Test_Attributo("SCEGLI_URGENZA", True) Then
                    Dim item As DllDocumentale.Documento_attributo = New DllDocumentale.Documento_attributo
                    item.Cod_attributo = "URGENTE"
                    item.Doc_id = idIstanzaWFE
                    item.Valore = flagUrgente
                    item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                    dlldoc.FO_Registra_Attributo(item, oOperatore, trans)
                End If
            End If

            'Aggiorno il campo Doc_dataRicezione in Documento
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

            Passo_Documento(idIstanzaWFE, idWorkitem, pNodoFlussoDetermina, pNodoFlussoUfficio, attore, pDirD, pDirU, pProssimoAttore)

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
    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrAltriAtti" Then
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub

    Overridable Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            'Select Case cFunzione
            '    Case vFunzioniPassoDetermina.cf_COLLABORATORE_INOLTRO
            '        Call DETERMINA_COLLABORATORE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_DEPOSITO_PRELIEVO
            '        Call DETERMINA_DEPOSITO_PRELIEVO()
            '    Case vFunzioniPassoDetermina.cf_SUPERVISORE_INOLTRO
            '        Call DETERMINA_SUPERVISORE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_PRELAZIONE
            '        Call DETERMINA_PRELAZIONE()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO
            '        Call UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO
            '        Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO
            '        Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
            '        Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
            '        Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO
            '        Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
            '    Case vFunzioniPassoDetermina.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO
            '        Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO()
            '    Case vFunzioniPassoDetermina.cf_DETERMINA_ANNULLO
            '        Call DETERMINA_ANNULLO()
            '    Case vFunzioniPassoDetermina.cf_DETERMINA_ASSEGNA
            '        Call DETERMINA_ASSEGNA()
            'End Select
        Catch ex As Exception
            Errore = Err.Number
            ErrDesc = Err.Description
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        objDocumento = Nothing
        MyBase.Finalize()
    End Sub
    '-------------------------- funzioni ad oggetti
    Private Function FO_Crea_AltroAtto_Object(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Crea_AltroAtto"
        Dim vRitPar(2) As Object
        Dim vRitSql As Object
        Dim vR As Object = Nothing
        Dim Sqlq As String
        Dim sWhere As String
        Dim DB As Object = Nothing
        Dim RS As Object = Nothing
        Dim vP(2) As Object

        Dim codUfficioProponente As String
        Dim utenteCreazione As String
        Dim dataCreazione As Date
        Dim numProvvisorio As String
        Dim dirD As String
        Dim dirU As String
        Dim idDocumentoLocale As String
        Dim tipoAtto As Integer

        On Error GoTo Herr

        vRitPar(0) = 0
        vRitPar(1) = ""
        vRitPar(2) = ""
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
        End If

        codUfficioProponente = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_cod_ufficio_proponente)
        utenteCreazione = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_utente_creazione) & ""
        tipoAtto = vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_tipo_atto) & ""

        If IsDate(vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_data_creazione)) Then
            dataCreazione = CDate(vParm(Dic_FODocumentale.vc_Crea_AltroAtto.c_data_creazione))
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

        'leggo la direzione per la creazione
        'verifico se sono un ufficio proponente

        Sqlq = "SELECT     Sta_id, Sta_attributo, Sta_valore " &
               " FROM         Struttura_Attributi " &
               " WHERE       (Sta_procedura = 'DETERMINE' or Sta_procedura = '*') AND (Sta_attributo = 'UFFICIO_PROPONENTE') " &
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
        Sqlq = "Select  Sta_attributo " &
               " FROM         Struttura_Attributi " &
               "  WHERE (Sta_id = '" & codUfficioProponente & "') " &
               "        AND (Sta_valore = '1' " &
               "              and (Sta_attributo = 'UFFICIO_DIRIGENZA_DIPARTIMENTO' OR  Sta_attributo = 'UFFICIO_RAGIONERIA'  OR  Sta_attributo = 'UFFICIO_CONTROLLO_AMMINISTRATIVO')) "

        vP(0) = DB
        vP(1) = Sqlq
        vP(2) = 2
        vR = GDB.DBQuery(vP)
        If vR(0) = 0 Then
            If vR(1)(0, 0) = "UFFICIO_DIRIGENZA_DIPARTIMENTO" Then
                dirD = "UDD"
            End If
        End If

        dirU = "C"

        Sqlq = "(SELECT     TAB_Operatori_Attributi.Toa_Valore " &
               "   FROM TAB_Operatori_Attributi " &
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (TAB_Operatori_Attributi.TOA_Operatore = '" & utenteCreazione & "') " &
               " ) " &
               " UNION " &
               "(SELECT     TAB_Operatori_Attributi.Toa_Valore " &
               " FROM         TAB_Operatori_Attributi INNER JOIN " &
               "            Tab_Operatori_Gruppi ON TAB_Operatori_Attributi.TOA_Operatore = Tab_Operatori_Gruppi.TOG_Gruppo " &
               " WHERE     (TAB_Operatori_Attributi.TOA_Attributo = 'DIRU_CREAZIONE') AND (Tab_Operatori_Gruppi.TOG_Operatore = '" & utenteCreazione & "')    " &
               " ) "

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

        vR = Istanzia_Flusso_Documento(dirD, dirU, utenteCreazione, idDocumentoLocale)

        If vR(0) <> 0 Then
            vRitPar(0) = vR(0)
            vRitPar(1) = "Problemi con il WorkFlow Engine - " + vR(1)
            GoTo FineSub
        End If

        idIstanzaWFE = Trim(vR(1))

        'registrare i dati su DB
        Call DB.BeginTrans()
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        vR = Calcola_Progressivo(DB, "NPDT", Year(Now), False, codUfficioProponente)

        If vR(0) = 0 Then
            numProvvisorio = vR(1)
        Else
            vRitPar(0) = "1"
            vRitPar(1) = "Problemi nella creazione del contatore - " & vR(1)
            GoTo RollTrans
        End If

        Sqlq = " SELECT     Doc_Id,  Doc_numeroProvvisorio, Doc_numero, Doc_Oggetto, Doc_Cod_Uff_Prop, Doc_Data, Doc_Tipo, Doc_Codice_Documento, " &
               "            Doc_Stato, Doc_liquidazione, Doc_Contabile, Doc_Pubblicazione, Doc_Testo, Doc_id_WFE , " &
               "            Doc_utenteCreazione,  Doc_operatore, Doc_dataRegistrazione " &
               " FROM   Documento " &
               " WHERE Doc_Id = '" & idDocumento & "' "

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
        RS("Doc_id_WFE").Value = idIstanzaWFE
        RS("Doc_Tipo").Value = tipoAtto
        RS("Doc_utenteCreazione").Value = oOperatore.Codice
        RS("Doc_operatore").Value = oOperatore.Codice
        RS("Doc_dataRegistrazione").Value = Now

        Call DB.UpdateRS(RS)
        If DB.errore <> 0 Then
            vRitPar(0) = DB.errore
            vRitPar(1) = DB.ErrDescr
            GoTo RollTrans
        End If

        'registro la creazione della determina tra le attività dell'utente
        Dim vRegAtt(Dic_FODocumentale.dimvc_Registra_Attivita) As Object
        vRegAtt(Dic_FODocumentale.vc_Registra_Attivita.c_tipoAttivita) = "CREAZIONE"

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
        FO_Crea_AltroAtto_Object = vRitPar
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
End Class
