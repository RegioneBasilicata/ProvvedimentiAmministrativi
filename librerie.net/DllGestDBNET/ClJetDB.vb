Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
'UPGRADE_WARNING: L'istanza della classe è stata cambiata in public. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1043"'
<System.Runtime.InteropServices.ProgId("ClJetDB_NET.ClJetDB")> Public Class ClJetDB
	
	Public strPercorso As String
	Public StrConnessione As String
	Public Nome As String
	Public NomeFisico As String
	Public DB As DAO.Database
	Public WSCL As DAO.Workspace
	Public Errore As clDicGDB.DBCostErrori
	Public ErrDescr As String
	Private dbErr As New ClDbErrore
	Public Stato As clDicGDB.dbCostTipoStato
	Public Tipo As clDicGDB.dbCostTipoDB
	Public timeOut As Integer
	Function ImpostaTimeout(ByRef nTimeout As Object) As Object
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto nTimeout. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		timeOut = nTimeout
	End Function
	
	Function ApriDB(Optional ByRef WSDB As DAO.Workspace = Nothing, Optional ByRef Percorso As Object = Nothing, Optional ByRef bEsclusivo As Object = False, Optional ByRef bLettura As Object = False, Optional ByRef Connessione As Object = "", Optional ByRef TimeoutConn As Object = 1000) As Short
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		'UPGRADE_NOTE: IsMissing() è stata cambiata in IsNothing(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1021"'
		If IsNothing(Percorso) Then
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Percorso. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			Percorso = strPercorso
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Percorso. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			strPercorso = Percorso
		End If
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Connessione. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		If Connessione = "" Then
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Connessione. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			Connessione = StrConnessione & ""
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Connessione. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			StrConnessione = Connessione
		End If
		
		If Not WSDB Is Nothing Then
			WSCL = WSDB
		End If
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Percorso. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		DB = WSCL.OpenDatabase(Percorso, bEsclusivo, bLettura, Connessione)
		
		Stato = clDicGDB.dbCostTipoStato.dbAperto
		
FineSub: 
		ApriDB = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Errore = Err.Number
		ErrDescr = Err.Description
		
		Call AccessErrore(dbErr)
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
		ErrDescr = dbErr.Descr_Nativo
		
		GoTo FineSub
		
	End Function
	Function ChiudiDB() As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		Stato = clDicGDB.dbCostTipoStato.dbChiuso
		
		DB.Close()
		
		
FineSub: 
		On Error Resume Next
		ChiudiDB = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		If Err.Number = 3704 Then
			GoTo FineSub
		End If
		Errore = Err.Number
		ErrDescr = Err.Description
		GoTo FineSub
		
		
		'   Call AccessErrore(dbErr)
		'   If dbErr.Azione = dbAzione_Ripeti Then
		'      If Timer < tMaxEdit Then
		'         tm = Timer + 1
		'         While tm > Timer
		'          DoEvents
		'         Wend
		'         Resume
		'      End If
		'   End If
		'   errore = dbErr.Codice
		'   ErrDescr = dbErr.Descrizione
		'   GoTo FineSub
		
	End Function
	Function BeginTrans() As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		
		WSCL.BeginTrans()
		
		Stato = clDicGDB.dbCostTipoStato.dbBeginTrans
		
FineSub: 
		BeginTrans = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function CommitTrans() As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		
		WSCL.CommitTrans()
		
		Stato = clDicGDB.dbCostTipoStato.dbAperto
		
FineSub: 
		CommitTrans = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function RollTrans() As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		
		WSCL.Rollback()
		
		Stato = clDicGDB.dbCostTipoStato.dbAperto
		
FineSub: 
		RollTrans = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	
	'$I------------------------------------------------------------------------------------------------------------
	' Il Parametro CursorLocation non è utilizzato in questo tipo di archivio, ma è necessario per garantire uniformità alla chiamata della funzione
	'$F------------------------------------------------------------------------------------------------------------
	Function ApriRS(ByRef Tabella As String, Optional ByRef Tipo As clDicGDB.dbCostAperturaRs = DAO.RecordsetTypeEnum.dbOpenDynaset, Optional ByRef TipoLock As clDicGDB.dbCostLock = 0, Optional ByRef Opzioni As Object = Nothing, Optional ByRef CursorLocation As Object = 2) As DAO.Recordset
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim r As Object
		
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		'UPGRADE_NOTE: IsMissing() è stata cambiata in IsNothing(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1021"'
		If IsNothing(TipoLock) Or TipoLock = 0 Then
			'     Set r = DB.OpenRecordset(Tabella)
			'     MsgBox (VarType(r))
			'     Set Rs = DB.OpenRecordset("Assistiti")
			'Dim I As Integer
			'     For I = 0 To DB.TableDefs.Count - 1
			'        Debug.Print DB.TableDefs(I).Name
			'     Next I
			RS = DB.OpenRecordset(Tabella, AperturaRS(Tipo))
		Else
			RS = DB.OpenRecordset(Tabella, AperturaRS(Tipo),  , LockDB(TipoLock))
		End If
		
		
FineSub: 
		ApriRS = RS
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function ChiudiRS(ByRef RS As DAO.Recordset) As clDicGDB.DBCostErrori
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
		Call AccessErrore(dbErr)
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
	Function AddRS(ByRef RS As DAO.Recordset) As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		'  Dim RS As New ADODB.Recordset
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		' RS.EditMode = EditMode
		'RS.LockEdits = True
		RS.AddNew()
		
		
FineSub: 
		AddRS = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function EditRS(ByRef RS As DAO.Recordset) As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		RS.LockEdits = True
		RS.Edit()
		
FineSub: 
		EditRS = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function UpdateRS(ByRef RS As DAO.Recordset) As clDicGDB.DBCostErrori
		Dim tm As Integer
		Dim tMaxEdit As Integer
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		RS.Update()
		
		
FineSub: 
		UpdateRS = Errore
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	
	
	Function GetString(ByRef Tabella As String, Optional ByRef NumRighe As Object = -1, Optional ByRef sSepCol As Object = "|", Optional ByRef sSepRiga As Object = "/n") As String
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim vRit(1) As Object
		Dim vR As Object
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		If StrConnessione <> "" Then
			RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NumRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			If NumRighe = 0 Then
				RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetTypeEnum.dbOpenForwardOnly)
			Else
				RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot)
			End If
		End If
		
		If RS.EOF Then
			GetString = ""
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto RS.GetRows(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vR = RS.GetRows(-1)
			GetString = MatToString(vR, "|", "/n")
		End If
		
		
FineSub: 
		Call ChiudiRS(RS)
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	
	
	
	
	Function Leggi_Pagina(ByRef Tabella As String, Optional ByRef NRighe As Object = 50, Optional ByRef nPagina As Object = 1) As Object
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim i As Short
		Dim j As Short
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		If StrConnessione <> "" Then
			RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		Else
			RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot)
		End If
		
		If RS.EOF Then
			Errore = 1
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Leggi_Pagina. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			Leggi_Pagina = Nothing
			GoTo FineSub 'Dino_13_10
		End If
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto nPagina. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		RS.AbsolutePosition = (nPagina - 1) * NRighe
		' RS.AbsolutePage = nPagina
		Dim vR(RS.Fields.Count - 1, NRighe) As Object
		i = -1
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		While i < NRighe Or Not RS.EOF
			i = i + 1
			For j = 0 To RS.Fields.Count - 1
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR(j, i). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				vR(j, i) = RS.Fields(i).Value
			Next j
			RS.MoveNext()
		End While
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Leggi_Pagina. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		Leggi_Pagina = VB6.CopyArray(vR)
		
FineSub: 
		Call ChiudiRS(RS)
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function GetRows(ByRef Tabella As String, Optional ByRef NumRighe As Object = 100, Optional ByRef FlgStruttura As Object = False) As Object
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim vRit(1) As Object
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		If StrConnessione <> "" Then
			RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NumRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			If NumRighe = 0 Then
				RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetTypeEnum.dbOpenForwardOnly)
			Else
				RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot)
			End If
		End If
		If RS.EOF Then
			Call ChiudiRS(RS)
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NumRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			NumRighe = 0
			Errore = clDicGDB.DBCostErrori.dbeNessunRecord
			'UPGRADE_WARNING: Array ha un nuovo comportamento. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
			GetRows = New Object(){"1", "Nessun Record Trovato"}
			Exit Function
		End If
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NumRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		If NumRighe = 0 Then
			RS.MoveLast()
			RS.MoveFirst()
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto NumRighe. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			NumRighe = RS.RecordCount
		End If
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto RS.GetRows(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(0) = RS.GetRows(NumRighe)
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto FlgStruttura. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		If FlgStruttura Then
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto TabStruttura(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vRit(1) = TabStruttura(RS)
		End If
		Call ChiudiRS(RS)
		
		'  Dim i As Integer
		'  For i = 0 To RS.Fields.Count - 1
		'     Debug.Print i & " - " & RS.Fields(i).Name
		'  Next i
		
FineSub: 
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto FlgStruttura. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		If FlgStruttura Then
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto GetRows. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			GetRows = VB6.CopyArray(vRit)
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			GetRows = vRit(0)
		End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(0) = Errore
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(1) = ErrDescr
		GoTo FineSub
		
	End Function
	
	Function Execute(ByRef Sqlq As String, Optional ByRef Opzioni As Object = DAO.RecordsetOptionEnum.dbFailOnError) As DAO.Recordset
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		
		If StrConnessione <> "" Then
			DB.Execute(Sqlq, DAO.RecordsetOptionEnum.dbSQLPassThrough)
		Else
			DB.Execute(Sqlq, Opzioni)
		End If
		
FineSub: 
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Private Sub AccessErrore(ByRef dbE As ClDbErrore)
		Dim strError As String
		Dim vErr As Object
		Dim i As Short
		Dim vRit(1) As Object
		Dim msg As String
		Dim Azione As Short
		
		If DB Is Nothing Then
			dbE.Codice = clDicGDB.DBCostErrori.dbeArchivioChiuso
			dbE.Descrizione = "Archivio dati Chiuso" & Chr(13) & "Consultare il Fornitore della Procedura"
			dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Esci
			If Not Err Is Nothing Then
				dbE.Errore_Nativo = Err.Number
				dbE.Descr_Nativo = Err.Description
			End If
			Exit Sub
		End If
		
		If Not Err Is Nothing Then
			dbE.Errore_Nativo = Err.Number
			dbE.Descr_Nativo = Err.Description
			Call Decod_Errore_Access(dbE)
		End If
		On Error GoTo Herr
		
		If dbE.Azione <> clDicGDB.dbCostAzione.dbAzione_Ripeti Then
			ReDim vErr(0, 1)
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vErr(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vErr(0, 0) = Err.Number
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vErr(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vErr(i, 1) = Err.Description
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vErr. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto dbE.vErr. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			dbE.vErr = vErr
		End If
		Exit Sub
		
Herr: 
		dbE.Codice = clDicGDB.DBCostErrori.dbeIgnoto
		dbE.Descrizione = Err.Description
	End Sub
	
	
	Function TabStruttura(ByRef Tabella As Object) As Object
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim vR As Object
		Dim FlgAperto As Boolean
		Dim i As Short
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		If TypeOf Tabella Is DAO.Recordset Then
			RS = Tabella
			FlgAperto = True
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Tabella. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			RS = DB.OpenRecordset(Tabella, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetTypeEnum.dbOpenForwardOnly)
			FlgAperto = False
		End If
		On Error Resume Next
		ReDim vR(3, RS.Fields.Count - 1)
		For i = 0 To RS.Fields.Count - 1
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vR(0, i) = RS.Fields(i).Name
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vR(1, i) = RS.Fields(i).Type
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vR(2, i) = RS.Fields(i).Size
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vR(3, i) = RS.Fields(i).Properties("Description").Value
		Next i
		
		
FineSub: 
		If FlgAperto = False And Not RS Is Nothing Then
			RS.Close()
		End If
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vR. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto TabStruttura. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		TabStruttura = vR
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	Function Registra_RS(ByRef Operazione As Object, ByRef Sqlq As Object, ByRef vDati As Object, Optional ByRef vCampi As Object = Nothing, Optional ByRef Att_Log As Object = False) As Object 'M40
		Dim tm As Integer
		Dim tMaxEdit As Integer
		Dim RS As DAO.Recordset
		Dim i As Short
		Dim M As Short
        Dim vLog As Object = Nothing
		Dim Log_Dim As Byte 'M40
		
		Errore = clDicGDB.DBCostErrori.dbeOperazioneRiuscita
		
		tMaxEdit = VB.Timer() + timeOut 'inizio il controllo di attesa
		On Error GoTo DBErrorHandler
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Sqlq. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		RS = DB.OpenRecordset(Sqlq, DAO.RecordsetTypeEnum.dbOpenDynaset)
		
		Select Case Operazione
			Case 0
				RS.AddNew()
			Case 1
				If RS.EOF Then
					Errore = clDicGDB.DBCostErrori.dbeNessunRecord
					ErrDescr = "Dati non trovati per l'aggiornamento"
					GoTo FineSub
				Else
					RS.Edit()
				End If
			Case Else
				If RS.EOF Then
					RS.AddNew()
				Else
					RS.Edit()
				End If
		End Select
		
		'UPGRADE_NOTE: IsMissing() è stata cambiata in IsNothing(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1021"'
		If IsNothing(vCampi) Then
			If UBound(vDati) >= RS.Fields.Count Then
				M = RS.Fields.Count - 1
			Else
				M = UBound(vDati)
			End If
			For i = 0 To M
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Att_Log. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				If Att_Log Then
					If RS.Fields(i).Type = ADOR.DataTypeEnum.adBoolean Then
						If RS.Fields(i).Value <> vDati(i) Then 'M40
							ReDim Preserve vLog(3, Log_Dim)
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(0, Log_Dim) = RS.Fields(i).Name
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(1, Log_Dim) = RS.Fields(i).Value
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vDati(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(2, Log_Dim) = vDati(i)
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(3, Log_Dim) = "UPDATE"
							Log_Dim = Log_Dim + 1
						End If
					Else
						'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vDati(i). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
						If StrComp(RS.Fields(i).Value, vDati(i), CompareMethod.Binary) <> 0 Then 'M40
							ReDim Preserve vLog(3, Log_Dim)
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(0, Log_Dim) = RS.Fields(i).Name
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(1, Log_Dim) = RS.Fields(i).Value
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vDati(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(2, Log_Dim) = vDati(i)
							'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
							vLog(3, Log_Dim) = "UPDATE"
							Log_Dim = Log_Dim + 1
						End If
					End If
				End If
				'UPGRADE_WARNING: IsEmpty è stato aggiornato a IsNothing e ha un nuovo comportamento. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
				If Not IsNothing(vDati(i)) Then
					'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vDati(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
					RS.Fields(i).Value = vDati(i)
				End If
			Next i
		Else
			For i = 0 To UBound(vCampi)
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vDati(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				RS.Fields(vCampi(i)).Value = vDati(i)
			Next i
		End If
		RS.Update()
		
FineSub: 
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vLog. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Registra_RS. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		Registra_RS = vLog
		'If bTRACE Then
		'   Call Registra_Trace("Fine - ApriDB(" & strconnessione",..)")
		'End If
		Exit Function
		
DBErrorHandler: 
		Call AccessErrore(dbErr)
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
	
	'UPGRADE_NOTE: Class_Initialize è stato aggiornato a Class_Initialize_Renamed. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1061"'
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
	
	
	Public Function Isrt_Campo(ByRef Tbx As Object, ByRef cx As Object, ByRef Tx As Object, ByRef Sx As Object, ByRef dx As Object) As Object
		Dim FLD As DAO.Field
		Dim Prp As DAO.Property
		Dim FldDescr As String
		Dim vRitFun(2) As Object
		Dim TD As DAO.TableDef
		Dim Tr As Boolean
		
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRitFun(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRitFun(0) = 0
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRitFun(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRitFun(1) = ""
		
		TD = DB.TableDefs(Tbx)
		Tr = False
		For	Each FLD In TD.Fields
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto cx. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			If UCase(FLD.Name) = UCase(cx) Then
				On Error GoTo CreaDescr
				FldDescr = FLD.Properties("Description").Value
				On Error GoTo 0
				Tr = True
				Exit For
			End If
		Next FLD
		
		If Tr = False Then
			FLD = TD.CreateField(cx, Tx, Sx)
			TD.Fields.Append(FLD)
			If Tx = DAO.DataTypeEnum.dbByte Or Tx = DAO.DataTypeEnum.dbInteger Or Tx = DAO.DataTypeEnum.dbLong Or Tx = DAO.DataTypeEnum.dbDouble Or Tx = DAO.DataTypeEnum.dbDecimal Then
				Prp = FLD.CreateProperty("DecimalPlaces", Tx, Sx)
				FLD.Properties.Append(Prp)
			End If
			Prp = FLD.CreateProperty("Description", DAO.DataTypeEnum.dbText, dx)
			FLD.Properties.Append(Prp)
		End If
FineSub: 
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Isrt_Campo. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		Isrt_Campo = VB6.CopyArray(vRitFun)
		Exit Function
		
CreaDescr: 
		Prp = FLD.CreateProperty("Description", DAO.DataTypeEnum.dbText, dx)
		FLD.Properties.Append(Prp)
		Resume Next
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
		Dim Num, j, i, k As Short
		Dim vRit(2) As Object
		Dim QDef As DAO.QueryDef
		Dim RS As DAO.Recordset
		Dim vField As Object
		Dim sDb As String
		Dim Statodb As Short
		Dim Sqlq As String
		
		Num = UBound(vQdef, 2)
		sDb = ""
		'On Error GoTo Herr
		On Error Resume Next
		'--- esecuzione dell'elenco dei comandi ---------------
		For i = 0 To Num - 1
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vQdef(0, i). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			If vQdef(0, i) <> "" Then
				'UPGRADE_NOTE: È possibile che l'oggetto QDef non venga eliminato finché non venga raccolto nel Garbage Collector. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
				QDef = Nothing
				'On Error Resume Next
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vQdef(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				Call DB.Execute("Drop Table " & vQdef(0, i))
				QDef = DB.CreateQueryDef(vQdef(0, i), vQdef(1, i))
				QDef = DB.QueryDefs(vQdef(0, i))
				'        On Error GoTo Herr
				'        If QDef Is Nothing Then
				'        Else
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vQdef(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				QDef.SQL = vQdef(1, i)
				'        End If
			End If
		Next i
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vQdef(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		Sqlq = vQdef(1, i)
		On Error GoTo Herr
		RS = DB.OpenRecordset(Sqlq)
		If Not RS.EOF Then
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto RS.GetRows(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vRit(1) = RS.GetRows(10000)
		Else
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vRit(1) = Nothing
		End If
		'D_MAGGIO_11
		ReDim vField(2, RS.Fields.Count)
		For i = 0 To RS.Fields.Count - 1
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vField(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vField(0, i) = RS.Fields(i).Name
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vField(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vField(1, i) = RS.Fields(i).Type
			'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vField(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
			vField(2, i) = RS.Fields(i).Size
		Next i
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vField. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(2). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(2) = vField
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(0) = 0
FineSub: 
		On Error Resume Next
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Execute_QueryDef. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		Execute_QueryDef = VB6.CopyArray(vRit)
		'UPGRADE_NOTE: Erase è stato aggiornato a System.Array.Clear. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1061"'
		System.Array.Clear(vRit, 0, vRit.Length)
		Exit Function
		
Herr: 
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(0). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(0) = Err.Number
		'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto vRit(1). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
		vRit(1) = Err.Description
		' Resume Next
		GoTo FineSub
		
	End Function
	
	
	
	'$$I------------------------------------------------------------
	'Funzione  : VetToString(Vet As Variant, sChar) As String
	'            trasforma vettore in una stringa
	'
	'Parametri : Vet     = vettore  da trasformare
	'            sChar   = carattere separatore
	'Ritorno   : vettore
	'$$F------------------------------------------------------------
	Private Function MatToString(ByRef Vet As Object, ByRef sCol As Object, ByRef sRiga As Object) As String
		Dim nLen As Short
		Dim i As Short
		Dim r As Short
        Dim Stringa As String = ""
		Dim strRiga As String
		
		nLen = UBound(Vet)
		For r = 0 To UBound(Vet, 2)
			strRiga = ""
			For i = 0 To nLen
				If i = 0 Then
					'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Vet(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
					strRiga = Vet(i, r)
				Else
					'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto Vet(). Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
					'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto sCol. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
					strRiga = strRiga & sCol & Vet(i, r)
				End If
			Next i
			If r = 0 Then
				Stringa = strRiga
			Else
				'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto sRiga. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
				Stringa = Stringa & sRiga & strRiga
			End If
		Next r
		MatToString = Stringa
		
	End Function
	
	Private Sub Decod_Errore_Access(ByRef dbE As ClDbErrore)
		
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
				dbE.Codice = dbE.Errore_Nativo
				dbE.Azione = clDicGDB.dbCostAzione.dbAzione_Ripeti
			Case 3376
				dbE.Descrizione = dbE.Descr_Nativo
				dbE.Codice = dbE.Errore_Nativo
				'dbE.Azione = dbAzione_Ripeti
				
			Case Else 'errori non noti
				dbE.Descrizione = "Errore Nella Gestione Archivi" & Chr(13) & Chr(13) & dbE.Descr_Nativo & Chr(13) & Chr(13) & "       Consultare il Sistemista"
				dbE.Codice = dbE.Errore_Nativo
		End Select
		
		
	End Sub
	
	Private Function AperturaRS(ByRef TipoApertura As clDicGDB.dbCostAperturaRs) As Short
		Select Case TipoApertura
			Case 5
				AperturaRS = DAO.RecordsetTypeEnum.dbOpenForwardOnly
			Case 1
				AperturaRS = DAO.RecordsetTypeEnum.dbOpenDynaset
			Case 2
				AperturaRS = DAO.RecordsetTypeEnum.dbOpenDynaset
			Case 3
				AperturaRS = DAO.RecordsetTypeEnum.dbOpenSnapshot
			Case 4
				AperturaRS = DAO.RecordsetTypeEnum.dbOpenDynaset
			Case Else
				AperturaRS = TipoApertura
		End Select
	End Function
	
	Public Function LockDB(ByRef blocco As clDicGDB.dbCostLock) As Short
		Select Case blocco
			Case 1
				LockDB = 4
			Case 2, 3
				LockDB = blocco
			Case 4
				LockDB = DAO.LockTypeEnum.dbOptimistic
			Case Else
				LockDB = blocco
		End Select
	End Function
End Class