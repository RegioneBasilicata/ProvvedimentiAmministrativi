Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient

<System.Runtime.InteropServices.ProgId("clAdoDB_NET.clAdoDB")> Public Class clAdoDB
	Public Nome As String
	Public NomeFisico As String
	Public StrConnessione As String
	Public DB As New ADODB.Connection
	Public Provider As String
	Public server As String
	Public Trast As Boolean
	Public User As String
	Public Password As String

    'Public cGDB As New clDicGDB
	Public Errore As clDicGDB.DBCostErrori
	Public ErrDescr As String
	Public Stato As Byte 'dbCostTipoStato
	Public Tipo As Byte 'dbCostTipoDB
	Public timeOut As Integer
	Public nConnessioni As Short
	
	Private dbErr As New ClDbErrore
    Const LockPessimistico As Short = 1

    Function ImpostaTimeout(ByRef nTimeout As Integer) As Object
        On Error Resume Next
        DB.ConnectionTimeout = nTimeout
        DB.CommandTimeout = nTimeout
        On Error GoTo 0
    End Function

    Function ApriDB(Optional ByRef Connessione As String = Nothing, Optional ByRef Apertura As ADODB.ConnectModeEnum = ADODB.ConnectModeEnum.adModeUnknown, Optional ByRef TimeoutConn As Integer = 1000) As Short
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        If IsNothing(Connessione) And StrConnessione = "" Then
            If NomeFisico = "" Then
                NomeFisico = Nome
            End If
            DB.Provider = Provider
            DB.Properties("Data Source").Value = server
            If Provider <> "OraOLEDB.Oracle" Then
                DB.Properties("Initial Catalog").Value = NomeFisico
            End If
            ' Tipo autorizzazione: WinNT o SQL Server.
            If Trast = True Then
                DB.Properties("Integrated Security").Value = "SSPI"
            Else
                DB.Properties("User ID").Value = User
                DB.Properties("Password").Value = Password
            End If
        ElseIf IsNothing(Connessione) And StrConnessione <> "" Then
            DB.ConnectionString = StrConnessione
        Else
            StrConnessione = Connessione
            DB.ConnectionString = Connessione
        End If

        If DB.Provider = "" And Provider <> "" Then
            DB.Provider = Provider
        End If
        ' DB.Mode = Apertura
        If Provider <> "OraOLEDB.Oracle" Then
            DB.ConnectionTimeout = TimeoutConn
            DB.CommandTimeout = timeOut
        Else
            DB.CommandTimeout = TimeoutConn
        End If
        DB.Open()

        Stato = clDicGDB.dbCostTipoStato.dbAperto
        nConnessioni = 1
FineSub:
        ApriDB = Errore
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Errore = Err.Number
        ErrDescr = Err.Description
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        If Errore = 3705 Then
            nConnessioni = nConnessioni + 1
            Errore = 0
        End If
        GoTo FineSub

    End Function


    Function ChiudiDB() As Byte
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        Stato = clDicGDB.dbCostTipoStato.dbChiuso
        If nConnessioni <= 1 Then
            DB.Close()
        End If
        nConnessioni = nConnessioni - 1
FineSub:
        ChiudiDB = Errore
        Exit Function

DBErrorHandler:
        If Err.Number = 3704 Then
            GoTo FineSub
        End If
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Function BeginTrans() As Byte
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler


        DB.BeginTrans()

        Stato = clDicGDB.dbCostTipoStato.dbBeginTrans

FineSub:
        BeginTrans = Errore
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function
    Function CommitTrans() As Byte
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler


        DB.CommitTrans()

        Stato = clDicGDB.dbCostTipoStato.dbAperto

FineSub:
        CommitTrans = Errore
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Function RollTrans() As Byte
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler


        DB.RollbackTrans()

        Stato = clDicGDB.dbCostTipoStato.dbAperto

FineSub:
        RollTrans = Errore
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Function ApriRS(ByRef Tabella As String, Optional ByRef Cursore As clDicGDB.dbCostAperturaRs = clDicGDB.dbCostAperturaRs.dbAperturaDinamica, Optional ByRef TipoLock As clDicGDB.dbCostLock = clDicGDB.dbCostLock.dbLockLettura, Optional ByRef Opzioni As Integer = -1, Optional ByRef CursorLocation As Integer = 2) As ADODB.Recordset
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        RS.CursorType = AperturaRS(Cursore)
        RS.LockType = LockDB(TipoLock)
        RS.CursorLocation = CursorLocation
        Tabella = Controlla_Sintassi_SQL(Tabella)
        RS.Open(Tabella, DB, , , Opzioni)


FineSub:
        ApriRS = RS
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function
    Function ChiudiRS(ByRef RS As ADODB.Recordset) As clDicGDB.DBCostErrori
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler


        RS.Close()


FineSub:
        ChiudiRS = Errore
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        If Err.Number = 91 Then
            GoTo FineSub
        End If
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function
    Function AddRS(ByRef RS As ADODB.Recordset, Optional ByRef EditMode As ADODB.EditModeEnum = ADOR.LockTypeEnum.adLockOptimistic) As clDicGDB.DBCostErrori
        Dim tm As Integer
        Dim tMaxEdit As Integer
        '  Dim RS As New ADODB.Recordset

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        ' RS.EditMode = EditMode
        RS.AddNew()


FineSub:
        AddRS = Errore
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:

        dbErr.Descrizione = Err.Description
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Function EditRS(ByVal RS As ADODB.Recordset, Optional ByVal EditMode As ADODB.EditModeEnum = ADOR.LockTypeEnum.adLockOptimistic) As clDicGDB.DBCostErrori
        Dim tm As Long
        Dim tMaxEdit As Long


        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        'RS.EditMode = EditMode
        '  RS.Edit
        '  Errore = dbeIgnoto
        '  Err.Description = "non Attivo"

FineSub:
        EditRS = Errore
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer < tMaxEdit Then
                tm = VB.Timer + 1
                While tm > VB.Timer
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function


    Function UpdateRS(ByRef RS As ADODB.Recordset, Optional ByRef EditMode As ADODB.EditModeEnum = ADOR.LockTypeEnum.adLockOptimistic) As clDicGDB.DBCostErrori
        Dim tm As Integer
        Dim tMaxEdit As Integer

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        RS.Update()


FineSub:
        UpdateRS = Errore
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function


    Function Leggi_Pagina(ByRef Tabella As String, Optional ByRef NRighe As Integer = 50, Optional ByRef nPagina As Integer = 1, Optional ByRef TipoScansione As Integer = 0) As Object
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim nRec As Integer
        Dim nCol As Integer
        Dim VettBlob() As Byte
        Dim vRit(3) As Object
        Dim VettRisultati(,) As Object

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
        vRit(0) = 0
        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        RS.CursorType = AperturaRS(clDicGDB.dbCostAperturaRs.dbAperturaKeyset)
        RS.LockType = LockDB(clDicGDB.dbCostLock.dbLockLettura)
        Tabella = Controlla_Sintassi_SQL(Tabella)
        RS.CursorLocation = ADOR.CursorLocationEnum.adUseClient
        RS.Open(Tabella, DB, , , -1)

        If RS.EOF Then
            Errore = 1
            vRit(0) = 1
            vRit(1) = "Nessun Record"
            vRit(2) = 0
            vRit(3) = 0
            GoTo FineSub '
        End If

        If nPagina <= 1 Then
            RS.MoveLast()
            RS.MoveFirst()
            vRit(3) = RS.RecordCount
            '      If NRighe > vRit(3) Then
            '         NRighe = vRit(3)
            '      End If
        End If
        RS.PageSize = NRighe
        If TipoScansione = 0 Then
            If nPagina = 0 Or NRighe >= vRit(3) Then
                vRit(1) = RS.GetRows(NRighe, 0)
            Else
                RS.AbsolutePage = nPagina
                vRit(1) = RS.GetRows(NRighe, (nPagina - 1) * NRighe)
            End If
            vRit(2) = UBound(vRit(1), 2)
        Else
            If nPagina = 0 Then '-- non gestisco la paginazione ---
                ReDim VettRisultati(RS.Fields.Count - 1, RS.RecordCount - 1)
            Else
                ReDim VettRisultati(RS.Fields.Count - 1, NRighe - 1)
                RS.PageSize = NRighe
                RS.AbsolutePage = nPagina
            End If
            nRec = -1
            If RS.EOF Then
                vRit(0) = 1
                vRit(1) = "Nessun Record"
                vRit(2) = 0
                vRit(3) = 0
                GoTo FineSub '
            End If
            Do Until RS.EOF
                nRec = nRec + 1
                If nPagina > 0 Then
                    If nRec = NRighe Then
                        Exit Do
                    End If
                End If
                For nCol = 0 To RS.Fields.Count - 1
                    Select Case RS.Fields.Item(nCol).Type
                        Case ADOR.DataTypeEnum.adDate, ADOR.DataTypeEnum.adDBDate, ADOR.DataTypeEnum.adDBTimeStamp
                            VettRisultati(nCol, nRec) = VB6.Format(RS.Fields(nCol).Value & "", "dd/mm/yyyy hh.nn.ss")
                        Case ADOR.DataTypeEnum.adLongVarBinary, ADOR.DataTypeEnum.adLongVarWChar, ADOR.DataTypeEnum.adLongVarChar
                            If IsDBNull(RS.Fields(nCol).Value) Then
                                VettRisultati(nCol, nRec) = ""
                            Else
                                VettBlob = RS.Fields(nCol).GetChunk(RS.Fields.Item(nCol).ActualSize)
                                VettRisultati(nCol, nRec) = VB6.CopyArray(VettBlob)
                            End If
                        Case Else
                            VettRisultati(nCol, nRec) = RS.Fields(nCol).Value & ""
                    End Select
                Next
                RS.MoveNext()
            Loop
            If nRec < NRighe Then
                ReDim Preserve VettRisultati(RS.Fields.Count - 1, nRec)
            End If
            vRit(1) = VB6.CopyArray(VettRisultati)
            vRit(2) = UBound(vRit(1), 2) + 1

        End If

FineSub:
        Call ChiudiRS(RS)
        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Leggi_Pagina. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        Leggi_Pagina = VB6.CopyArray(vRit)
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        vRit(0) = Errore
        vRit(1) = ErrDescr
        GoTo FineSub

    End Function


    Function GetRows(ByRef Tabella As String, Optional ByRef NumRighe As Integer = 100, Optional ByRef FlgStruttura As Boolean = False, Optional ByRef Cursore As clDicGDB.dbCostAperturaRs = clDicGDB.dbCostAperturaRs.dbAperturaKeyset, Optional ByRef TipoLock As clDicGDB.dbCostLock = clDicGDB.dbCostLock.dbLockLettura, Optional ByRef Opzioni As Integer = -1) As Object
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim vRit(1) As Object
        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        RS.CursorType = AperturaRS(clDicGDB.dbCostAperturaRs.dbAperturaForward)
        RS.LockType = LockDB(TipoLock)
        Tabella = Controlla_Sintassi_SQL(Tabella)
        RS.Open(Tabella, DB, , , Opzioni)

        If RS.EOF Then
            Call ChiudiRS(RS)
            Errore = 1
            vRit(0) = Nothing
            GoTo FineSub 'Dino_13_10
        Else
            If NumRighe = 0 Then
                vRit(0) = RS.GetRows
            Else
                vRit(0) = RS.GetRows(NumRighe)
            End If
        End If
        If FlgStruttura Then
            vRit(1) = TabStruttura(RS)
        End If
        Call ChiudiRS(RS)

FineSub:
        If FlgStruttura Then
            GetRows = VB6.CopyArray(vRit)
        Else
            GetRows = vRit(0)
        End If
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function


    Function Execute(ByRef Sqlq As String, Optional ByRef Opzioni As Integer = DAO.RecordsetOptionEnum.dbFailOnError) As ADODB.Recordset
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler
        Sqlq = Controlla_Sintassi_SQL(Sqlq)

        Call DB.Execute(Sqlq, , Opzioni)


FineSub:
        Execute = RS
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Private Sub AdoErrore(ByRef dbE As ClDbErrore)
        Dim strError As String
        Dim vErr(,) As Object
        Dim i As Short
        Dim vRit(1) As Object
        Dim msg As String
        Dim Azione As Short
        Dim errLoop As ADODB.Error

        dbE.Codice = Err.Number
        dbE.Descrizione = Err.Description
        On Error Resume Next
        errLoop = DB.Errors(0)
        On Error GoTo 0
        If errLoop Is Nothing Then
            'non riesco a valutare l'errore, verifico lo stato del database
            If DB.State = ADOR.ObjectStateEnum.adStateClosed Then
                dbE.Codice = clDicGDB.DBCostErrori.dbeArchivioChiuso
                dbE.Descrizione = "Connessione di Accesso ai dati Chiusa" & Chr(13) & "Consultare il Fornitore della Procedura"
            End If
            dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
            Exit Sub
        Else
            dbE.Codice = errLoop.NativeError
        End If

        On Error GoTo Herr
        errLoop = DB.Errors(0)
        dbE.Errore_Nativo = errLoop.NativeError
        dbE.Descr_Nativo = errLoop.Description
        Call Decod_Errore_Sql(dbE)

        If dbE.Azione <> clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            ReDim vErr(DB.Errors.Count, 1)
            i = -1
            For Each errLoop In DB.Errors
                i = i + 1
                vErr(i, 0) = errLoop.NativeError
                vErr(i, 1) = "Errore #" & errLoop.Number & vbCr & "   " & errLoop.Description & vbCr & "   (Source: " & errLoop.Source & ")" & vbCr & "   (SQL State: " & errLoop.SQLState & ")" & vbCr & "   (NativeError: " & errLoop.NativeError & ")" & vbCr
                If DllSistemaClSistema_definst.bLOG Then
                    '            Call Registra_Log(vErr(i, 1))
                End If
            Next errLoop
            dbE.vErr = vErr
        End If
        Exit Sub

Herr:
        dbE.Codice = clDicGDB.DBCostErrori.dbeIgnoto
        dbE.Descrizione = Err.Description
        dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
    End Sub

    Private Sub Class_Initialize_Renamed()
        If timeOut = 0 Then
            timeOut = 20
        End If
        Stato = 0
    End Sub

    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub


    Function TabStruttura(ByRef Tabella As Object) As Object
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim vR(,) As Object
        Dim FlgAperto As Boolean
        Dim i As Short
        Dim sTab As String

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        If TypeOf Tabella Is ADODB.Recordset Then 'Dino_L_19_12
            RS = Tabella
            FlgAperto = True
        Else
            '    Set RS = DB.OpenRecordset(Tabella, adOpenDynamic)
            sTab = Tabella
            Tabella = Controlla_Sintassi_SQL(sTab)
            RS.Open(Tabella, DB)
            FlgAperto = False
        End If

        ReDim vR(3, RS.Fields.Count - 1)
        For i = 0 To RS.Fields.Count - 1
            vR(0, i) = RS.Fields(i).Name
            vR(1, i) = RS.Fields(i).Type
            vR(2, i) = IIf(RS.Fields(i).Type = DAO.DataTypeEnum.dbDate, 10, RS.Fields(i).DefinedSize)
            vR(3, i) = RS.Fields(i).Name 'RS.Fields(i).Properties("Description")
        Next i


FineSub:
        If FlgAperto = False Then
            RS.Close()
        End If
        TabStruttura = vR
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    '$$I------------------------------------------------------------------------
    'Function Controlla_Sintassi_SQL(QuerySQL As String, TipDb As dbCostTipoDB) As String
    '         Controlla la compatibilità tra SQL access e SQL Standard
    'Parametri: QuerySQL= stringa SQL
    '           TipoDb = tipo archivio
    '
    'Ritorno: Nuova Stringa SQL
    '$$I------------------------------------------------------------------------

    Public Function Controlla_Sintassi_SQL(ByRef QuerySQL As String) As String
        Dim Data As Object
        Dim QueLike As String
        Dim QueP2 As String
        Dim QueP1 As String
        Dim Pos2 As Integer
        Dim Pos As Integer
        Dim s As String
        Dim N As Short
        Dim n1 As Short
        'Ct Like *
        Pos = InStr(1, UCase(QuerySQL), " LIKE ")
        If Pos > 0 Then
            Pos2 = InStr(Pos, UCase(QuerySQL), " GROUP BY ")
            If Pos2 = 0 Then
                Pos2 = InStr(Pos, UCase(QuerySQL), " HAVING ")
            End If

            If Pos2 = 0 Then
                Pos2 = InStr(Pos, UCase(QuerySQL), " ORDER BY ")
            End If

            If Pos2 = 0 Then
                s = Replace(QuerySQL, "*", "%", Pos)
                QuerySQL = Mid(QuerySQL, 1, Pos - 1) & s
            Else
                QueP1 = Left(QuerySQL, Pos)
                QueP2 = Mid(QuerySQL, Pos2)
                QueLike = Mid(QuerySQL, Pos, Pos2 - Pos)
                QueLike = Replace(QueLike, "*", "%")
                QuerySQL = QueP1 & QueLike & QueP2
            End If
        End If
        'Ct Data
        '      QuerySQL = Replace(QuerySQL, "#", "'")
        N = InStr(QuerySQL, "#")
        While N > 0
            n1 = InStr(N + 1, QuerySQL, "#")
            Data = Mid(QuerySQL, N + 1, n1 - (N + 1))
            Data = Replace(Data, ".", ":")
            QuerySQL = Mid(QuerySQL, 1, N - 1) & "'" & Data & "'" & Mid(QuerySQL, n1 + 1)
            N = InStr(QuerySQL, "#")
        End While
        'Ct DISTINCTROW
        Pos = InStr(1, UCase(QuerySQL), " DISTINCTROW ")
        If Pos > 0 Then
            QuerySQL = Replace(QuerySQL, "DISTINCTROW", "")
        End If

        'Ct &
        If InStr(1, QuerySQL, "&") > 0 Then
            QuerySQL = Replace(QuerySQL, "&", "+")
        End If

        Controlla_Sintassi_SQL = QuerySQL
    End Function
    Function Registra_RS(ByRef Operazione As Object, ByRef Sqlq As Object, ByRef vDati() As Object, Optional ByRef vCampi As Object = Nothing, Optional ByRef Att_Log As Boolean = False) As Object  'M40
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim i As Short
        Dim M As Short
        Dim vLog As Object = Nothing
        Dim Log_Dim As Byte 'M40
        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        RS.LockType = ADOR.LockTypeEnum.adLockOptimistic
        RS.CursorType = ADOR.CursorTypeEnum.adOpenDynamic
        RS.Open(Sqlq, DB)

        Select Case Operazione
            Case 0
                RS.AddNew()
            Case 1
                If RS.EOF Then
                    Errore = clDicGDB.DBCostErrori.dbeNessunRecord
                    ErrDescr = "Dati non trovati per l'ag  giornamento"
                    GoTo FineSub
                End If
            Case Else
                If RS.EOF Then
                    RS.AddNew()
                Else
                    ' RS.Edit
                End If
        End Select

        If vCampi Is Nothing Then
            If UBound(vDati) >= RS.Fields.Count Then
                M = RS.Fields.Count - 1
            Else
                M = UBound(vDati)
            End If

            ReDim vLog(3, 0)
            For i = 0 To M
                If Att_Log Then
                    If RS.Fields(i).Type = ADOR.DataTypeEnum.adBoolean Then
                        If RS.Fields(i).Value <> vDati(i) Then 'M40
                            ReDim Preserve vLog(3, Log_Dim)
                            vLog(0, Log_Dim) = RS.Fields(i).Name
                            vLog(1, Log_Dim) = RS.Fields(i).Value
                            vLog(2, Log_Dim) = vDati(i)
                            vLog(3, Log_Dim) = "UPDATE"
                            Log_Dim = Log_Dim + 1
                        End If
                    Else
                        If StrComp(RS.Fields(i).Value, vDati(i), CompareMethod.Binary) <> 0 Then 'M40
                            ReDim Preserve vLog(3, Log_Dim)
                            vLog(0, Log_Dim) = RS.Fields(i).Name
                            vLog(1, Log_Dim) = RS.Fields(i).Value
                            vLog(2, Log_Dim) = vDati(i)
                            vLog(3, Log_Dim) = "UPDATE"
                            Log_Dim = Log_Dim + 1
                        End If
                    End If
                End If
                If Not IsNothing(vDati(i)) Then
                    RS.Fields(i).Value = vDati(i)
                End If
                '''''         If Not IsEmpty(vDati(i)) And vDati(i) <> "" And vDati(i) <> "0.00.00" Then
                '''''           'Debug.Print i & ") " & RS.Fields(i).Name & "= " & IIf(vDati(i) = "", Null, vDati(i))
                '''''            If RS.Fields(i).Type = adDBTimeStamp And vDati(i) <> "0.00.00" Then
                '''''               RS.Fields(i) = Replace(vDati(i), ".", ":")
                '''''            Else
                '''''               If Asc(vDati(i)) <> 0 Then
                '''''                  RS.Fields(i) = vDati(i)
                '''''               End If
                '''''            End If
                '''''         Else
                '''''            'Debug.Print i & ") " & RS.Fields(i).Name
                '''''         End If
            Next i
        Else
            For i = 0 To UBound(vCampi)
                RS.Fields(vCampi(i)).Value = vDati(i)
            Next i
        End If
        RS.Update()

FineSub:
        Registra_RS = vLog

        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If

        Exit Function

DBErrorHandler:
        Errore = Err.Number
        ErrDescr = Err.Description
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function



    Public Function Isrt_Campo(ByRef Tbx As Object, ByRef cx As Object, ByRef Tx As Object, ByRef Sx As Object, ByRef dx As Object) As Object
        '   ' Dim FLD As ADODB.Field
        '    Dim Prp As Property
        '    Dim FldDescr As String
        Dim vRitFun(2) As Object
        '    Dim TD As ADOx.Table
        '    Dim FLD As ADOx.Column
        '    Dim Tr As Boolean
        '

        vRitFun(0) = 1
        vRitFun(1) = "funzione non attiva"
        '
        '   Set TD = DB.Tables(Tbx)
        '   Tr = False
        '   For Each FLD In TD.Columns
        '      If UCase$(FLD.Name) = UCase$(cx) Then
        '         On Error GoTo CreaDescr
        '         FldDescr = FLD.Properties("Description")
        '         On Error GoTo 0
        '         Tr = True
        '         Exit For
        '      End If
        '   Next FLD
        '
        '   If Tr = False Then
        '      Set FLD = TD.CreateField(cx, Tx, Sx)
        '      TD.Fields.Append FLD
        '      If Tx = dbByte Or Tx = dbInteger Or Tx = dbLong Or Tx = dbDouble Or Tx = dbDecimal Then
        '         Set Prp = FLD.CreateProperty("DecimalPlaces", _
        ''                  Tx, Sx)
        '         FLD.Properties.Append Prp
        '      End If
        '      Set Prp = FLD.CreateProperty("Description", _
        ''               dbText, dx)
        '      FLD.Properties.Append Prp
        '   End If
        'FineSub:
        '   Isrt_Campo = vRitFun
        '   Exit Function
        '
        'CreaDescr:
        '      Set Prp = FLD.CreateProperty("Description", _
        ''               dbText, dx)
        '      FLD.Properties.Append Prp
        '      Resume Next
    End Function




    '$$I----------------------------------------------------------------------------
    'Funzione : Execute_QueryDef(VParm) As Variant
    '           Crea le query in Input e Ritorna il risultato dellultima
    '
    'Parametri:
    '           VParm (0) = database sul quale operare
    '           vparm(1) = VQDEF  'vettore query
    '           vQdef=[Nome,Sqlq]
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio/Vdati]
    '$$F----------------------------------------------------------------------------
    Public Function Execute_QueryDef(ByRef vQdef As Object) As Object
        Dim Num, j, i As Short
        Dim vRit(2) As Object
        Dim Cat1 As New ADOX.Catalog
        Dim QDef As ADOX.View
        Dim RS As New ADODB.Recordset
        Dim vField As Object
        Dim Tabella As String

        Try
            Num = UBound(vQdef, 2)

            Cat1.ActiveConnection = DB


            '--- esecuzione dell'elenco dei comandi ---------------
            For i = 0 To Num - 1
                If vQdef(0, i) <> "" Then
                    QDef = Nothing
                    QDef = Cat1.Views(vQdef(0, i))
                    If QDef Is Nothing Then
                        Call Cat1.Views.Append(vQdef(0, i), vQdef(1, i))
                    Else
                        QDef.let_Command(vQdef(1, i))
                    End If
                End If
            Next i

            Tabella = vQdef(1, i)
            RS.CursorType = ADOR.CursorTypeEnum.adOpenKeyset
            RS.LockType = ADOR.LockTypeEnum.adLockOptimistic
            Tabella = Controlla_Sintassi_SQL(Tabella)
            RS.Open(Tabella, DB, , , -1)

            If RS.EOF Then
                Errore = 1
                vRit(0) = Nothing
                Exit Try
            Else
                vRit(1) = RS.GetRows
            End If

            ReDim vField(2, RS.Fields.Count)
            For i = 0 To RS.Fields.Count - 1
                vField(0, i) = RS.Fields(i).Name
                vField(1, i) = RS.Fields(i).Type
                vField(2, i) = IIf(RS.Fields(j).Type = ADOR.DataTypeEnum.adDate, 10, RS.Fields(j).DefinedSize)
            Next i
            vRit(2) = vField
            vRit(0) = 0
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Execute_QueryDef = vRit
        End Try
    End Function


    Private Sub Class_Terminate_Renamed()
        'D_MAR_16_2003  Call Fine_Proc(0)

    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub


    '    Function GetXml(ByRef Tabella As String, Optional ByRef NumRighe As Integer = -1, Optional ByRef sSepCol As String = "|", Optional ByRef sSepRiga As String = "/n") As String
    '        Dim tm As Long
    '        Dim tMaxEdit As Long
    '        Dim RS As New ADODB.Recordset
    '        Dim vRit(1) As Object




    '        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

    '        tMaxEdit = VB.Timer + timeOut 'inizio il controllo di attesa
    '        On Error GoTo DBErrorHandler


    '        RS.CursorType = AperturaRS(clDicGDB.dbCostAperturaRs.dbAperturaForward)
    '        '   RS.LockType = LockDB(TipoLock, dbADOSQL)
    '        Tabella = Controlla_Sintassi_SQL(Tabella)
    '        Tabella = Tabella & " FOR XML AUTO, ELEMENTS"
    '        RS.Open(Tabella, DB, , )

    '        If RS.EOF Then
    '            GetXml = ""
    '        Else
    '            'GetXml = CType(RS.Fields(0).Value, String)
    '            'GetXml = RS.GetString
    '            Dim vv As Byte()
    '            'vv = RS.Fields(0).GetChunk(10000)
    '            'vv = RS.Fields(0).Value
    '            Dim st1 As Stream = File.Open("E:\ProvaXml.xml", FileMode.OpenOrCreate Or FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
    '            Dim bw1 As New BinaryWriter(st1)
    '            bw1.Write(RS.Fields(0).Value)
    '            bw1.Close()
    '            st1.Close()
    '            bw1 = Nothing
    '            'GetXml = RS.GetRows
    '            'GetXml = RS.g
    '            '    GetXml = RS.GetString(adClipString, , sSepCol, sSepRiga)
    '        End If



    'FineSub:
    '        Call ChiudiRS(RS)
    '        'If bTRACE Then
    '        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
    '        'End If
    '        Exit Function

    'DBErrorHandler:
    '        Call AdoErrore(dbErr)
    '        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
    '            If VB.Timer < tMaxEdit Then
    '                tm = VB.Timer + 1
    '                While tm > VB.Timer
    '                    System.Windows.Forms.Application.DoEvents()
    '                End While
    '                Resume
    '            End If
    '        End If
    '        Errore = dbErr.Codice
    '        ErrDescr = dbErr.Descrizione
    '        GetXml = ""
    '        GoTo FineSub

    '    End Function

    Function GetXml(ByRef Tabella As String, Optional ByRef NumRighe As Integer = -1, Optional ByRef sSepCol As String = "|", Optional ByRef sSepRiga As String = "/n") As String
        'Dim tm As Long
        'Dim tMaxEdit As Long
        'Dim RS As New ADODB.Recordset
        Dim vRit(1) As Object
        Dim risultato As String
        'Dim xmldoc As New Xml.XmlDocument

        Try

       

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

            'tMaxEdit = VB.Timer + timeOut 'inizio il controllo di attesa
        'On Error GoTo DBErrorHandler


            'RS.CursorType = AperturaRS(clDicGDB.dbCostAperturaRs.dbAperturaForward)
        '   RS.LockType = LockDB(TipoLock, dbADOSQL)
        Tabella = Controlla_Sintassi_SQL(Tabella)
        Tabella = Tabella & " FOR XML AUTO, ELEMENTS"
        'RS.Open(Tabella, DB, , )
        Dim cmd As New ADODB.Command
        cmd.ActiveConnection = DB
        cmd.CommandText = Tabella
        Dim str As ADODB.Stream
        str = New ADODB.Stream
        'Dim str1 As System.IO.Stream
        str.Open()
        cmd.Properties("Output Stream").Value = str
        cmd.Execute(, , ADODB.ExecuteOptionEnum.adExecuteStream)
        'str1 = CType(str, System.IO.Stream)
            risultato = str.ReadText()
            str.Close()

        'If RS.EOF Then
        '    GetXml = ""
        'Else
        '    'GetXml = CType(RS.Fields(0).Value, String)
        '    'GetXml = RS.GetString
        '    Dim vv As Byte()
        '    'vv = RS.Fields(0).GetChunk(10000)
        '    'vv = RS.Fields(0).Value
        '    Dim st1 As Stream = File.Open("E:\ProvaXml.xml", FileMode.OpenOrCreate Or FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
        '    Dim bw1 As New BinaryWriter(st1)
        '    bw1.Write(RS.Fields(0).Value)
        '    bw1.Close()
        '    st1.Close()
        '    bw1 = Nothing
        '    'GetXml = RS.GetRows
        '    'GetXml = RS.g
        '    '    GetXml = RS.GetString(adClipString, , sSepCol, sSepRiga)
        'End If



FineSub:
        'Call ChiudiRS(RS)
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
            'End If
            cmd = Nothing
            str = Nothing
            Return risultato
            Exit Function

        'DBErrorHandler:
        '        Call AdoErrore(dbErr)
        '        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
        '            If VB.Timer < tMaxEdit Then
        '                tm = VB.Timer + 1
        '                While tm > VB.Timer
        '                    System.Windows.Forms.Application.DoEvents()
        '                End While
        '                Resume
        '            End If
        '        End If
        '        Errore = dbErr.Codice
        '        ErrDescr = dbErr.Descrizione
        '        GetXml = ""
        '        GoTo FineSub
        Catch ex As Exception
            risultato = ex.Message
            GoTo FineSub
        End Try

    End Function




    'D31-10_02
    Function GetString(ByRef Tabella As String, Optional ByRef NumRighe As Integer = -1, Optional ByRef sSepCol As String = "|", Optional ByRef sSepRiga As String = "/n") As String
        Dim Opzioni As Object
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim vRit(1) As Object
        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        '   RS.CursorType = AperturaRS(Cursore, dbADOSQL)
        '   RS.LockType = LockDB(TipoLock, dbADOSQL)
        Tabella = Controlla_Sintassi_SQL(Tabella)
        RS.Open(Tabella, DB, , )

        If RS.EOF Then
            GetString = ""
        Else
            GetString = RS.GetString(ADOR.StringFormatEnum.adClipString, , sSepCol, sSepRiga)
        End If

FineSub:
        Call ChiudiRS(RS)
        'If bTRACE Then
        '   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
        'End If
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GetString = ""
        GoTo FineSub

    End Function


    Public Function ELenco_Tabelle() As Object
        Dim tm As Integer
        Dim tMaxEdit As Integer
        Dim RS As New ADODB.Recordset
        Dim vRit(1) As Object
        Dim Sqlq As String
        Dim vTab As Object
        Dim i As Short

        Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita

        tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
        On Error GoTo DBErrorHandler

        Sqlq = "SELECT name AS Tabella From sysobjects WHERE (xtype = 'U') ORDER BY name"
        '   RS.CursorType = AperturaRS(Cursore, dbADOSQL)
        '   RS.LockType = LockDB(TipoLock, dbADOSQL)
        RS.Open(Sqlq, DB) ', , , Opzioni

        If RS.EOF Then
            Call ChiudiRS(RS)
            Errore = 1
            vRit(0) = 0
            vRit(1) = Nothing
            GoTo FineSub
        Else
            Dim vTba(0) As Object
            i = -1
            While Not RS.EOF
                i = i + 1
                ReDim Preserve vTab(i)
                vTab(i) = RS.Fields("Tabella").Value
                RS.MoveNext()
            End While
            vRit(1) = RS.GetRows
        End If

FineSub:
        On Error Resume Next
        ELenco_Tabelle = vRit
        System.Array.Clear(vRit, 0, vRit.Length)
        Exit Function

DBErrorHandler:
        Call AdoErrore(dbErr)
        If dbErr.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti Then
            If VB.Timer() < tMaxEdit Then
                tm = VB.Timer() + 1
                While tm > VB.Timer()
                    System.Windows.Forms.Application.DoEvents()
                End While
                Resume
            End If
        End If
        Errore = dbErr.Codice
        ErrDescr = dbErr.Descrizione
        GoTo FineSub

    End Function

    Private Sub Decod_Errore_Sql(ByRef dbE As ClDbErrore)

        dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
        Select Case dbE.Errore_Nativo
            Case 3022 ' chiave già presente
                dbE.Descrizione = "Informazione Già Presente In Archivio (Chiave Duplicata) "
                dbE.Codice = clDicGDB.DBCostErrori.dbeChiaveDuplicata

            Case 3326, 3027 ' archivio a sola lettura
                dbE.Descrizione = "Archivio Non Aggiornabile, Consultare Sistemista"
                dbE.Codice = clDicGDB.DBCostErrori.dbeArchivioNonAggiornabile

            Case 3197 'dati cambiati
                dbE.Descrizione = "L'Informazione E' Stata Variata da Altri Utenti." & Chr(13) & "Ripetere L'Intera operazione"
                dbE.Codice = clDicGDB.DBCostErrori.dbeDatiModificati

            Case 3202, 3045, 3189, 3261, 3262, 3356, 3218, 3186, 3187, 3260, 3006, 3008, 3009, 3046, 3188, 3189, 3422 'errori di lock su tabella o archivio
                dbE.Descrizione = "Archivio o Informazione usata da Altro Utente, Rileggere e Riprovare"
                dbE.Codice = clDicGDB.DBCostErrori.dbeRecordBloccato
                dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti
            Case -2147217871
                If InStr(dbE.Descr_Nativo, "Timeout") > 0 Then
                    dbE.Descrizione = "TimeOut di Collegamento ODBC"
                    dbE.Codice = clDicGDB.DBCostErrori.dbeTimeOut
                    dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti
                End If
            Case 2627
                dbE.Descrizione = "Chiave Duplicata"
                dbE.Codice = clDicGDB.DBCostErrori.dbeChiaveDuplicata
                dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
            Case Else 'errori non noti
                dbE.Descrizione = "Errore Nella Gestione Archivi" & Chr(13) & Chr(13) & dbE.Descr_Nativo & Chr(13) & Chr(13) & "       Consultare il Sistemista"
                dbE.Codice = clDicGDB.DBCostErrori.dbeIgnoto
                dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
        End Select

    End Sub

    Private Function AperturaRS(ByRef TipoApertura As clDicGDB.dbCostAperturaRs) As Short
        Select Case TipoApertura
            Case 5
                AperturaRS = ADOR.CursorTypeEnum.adOpenForwardOnly
            Case 1, 4
                AperturaRS = ADOR.CursorTypeEnum.adOpenKeyset
            Case 2
                AperturaRS = ADOR.CursorTypeEnum.adOpenDynamic
            Case 3
                AperturaRS = ADOR.CursorTypeEnum.adOpenStatic
        End Select
    End Function

    Public Function LockDB(ByRef blocco As clDicGDB.dbCostLock) As Short
        Dim TipoDb As Object
        Select Case TipoDb
            Case clDicGDB.dbCostTipoDB.dbADOSQL, clDicGDB.dbCostTipoDB.dbADOAccess
                LockDB = blocco
            Case Else
                LockDB = blocco
        End Select
    End Function
End Class