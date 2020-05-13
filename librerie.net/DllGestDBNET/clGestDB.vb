Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("clGestDB_NET.clGestDB")> Public Class clGestDB
    Inherits BaseGestDB
    Public Ws As DAO.Workspace
    '  Public SISTEMA As New DllSistemaNet.ClSistema
    Public SISTEMA As New DllSistemaNet.ClSistema

    Public Function DBElabora_Interrogazione(ByVal vParm As Object) As Object
        Const SFunzione As String = "DBElabora_Interrogazione"
        Dim vRitPar(3) As Object
        Dim Sqlq As String
        Dim sQuery As String
        Dim codInt As Long
        Dim vPq As Object
        Dim i As Integer
        Dim vR As Object = Nothing
        Dim sArchivio As String
        Dim s As String
        Dim xParm(2) As Object
        Dim sParm(1) As Object

        '
        On Error GoTo Herr
        vPq = vParm(1)
        Sqlq = "SELECT  Tip_Query, Tip_Archivio" _
             & " From Tab_Procedure_Interrogazioni " _
             & " Where (Tip_Codice = '" & vParm(0) & "')"
        xParm(0) = "TABCENTR"
        xParm(1) = Sqlq
        xParm(2) = 1
        vR = DBQuery(xParm)
        If vR(0) <> 0 Then
            GoTo FineSub
        End If
        sQuery = UCase(vR(1)(0, 0))
        sArchivio = vR(1)(1, 0)

        If IsArray(vPq) Then
            For i = 0 To UBound(vPq, 2)
                sQuery = Replace(sQuery, "#" & vPq(0, i) & "#", vPq(1, i))
            Next i
        End If
        xParm(0) = sArchivio
        xParm(1) = sQuery
        xParm(2) = 1
        vR = DBQuery(xParm)
FineSub:
        DBElabora_Interrogazione = vR
        On Error Resume Next
        If vR(0) <> 0 Then
            Call SISTEMA.Registra_Log(vR(1), SFunzione)
        End If
        Erase vR
        Exit Function

Herr:
        sParm(0) = Err.Number
        sParm(1) = Err.Description
        vR = sParm
        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
        GoTo FineSub
        'Resume
    End Function






    '%%01
    '$$I----------------------------------------------------------------------------
    'Funzione : DBQueryPagina(vParm) -> vDati
    '           Lancia la query e ritorna il risulato
    '
    'Parametri: [DataBase;Query Sql;Tipo Parametro Database;Numero Pagina;Numero Righe]
    'Ritorno  : vRitOle=[codice errore; Messaggio / Matrice Dati]
    '                   MatriceDati = [Valore(Campo,Record)]
    '$$F----------------------------------------------------------------------------
    Public Function DBQueryPagina(ByVal vParm As Object) As Object
        Dim qDB As Object
        Dim sDb, sSql As String
        Dim TiposDB As Short
        Dim i As Short
        Dim Statodb As Short
        Dim vRit(4) As Object
        Dim bStruttura As Boolean
        Dim NRighe As Integer
        Dim nPagina As Integer
        Dim TipoScansione As Byte


        Statodb = 0
        bStruttura = False

        Dim vR As Object = Nothing
        If UBound(vParm) > 1 Then
            TiposDB = vParm(2)
        Else
            TiposDB = 0
        End If

        sSql = vParm(1)
        If UBound(vParm) > 2 Then
            NRighe = vParm(3)
        Else
            NRighe = 0
        End If
        If UBound(vParm) > 3 Then
            nPagina = vParm(4)
        Else
            nPagina = 0
        End If
        If UBound(vParm) > 4 Then
            TipoScansione = vParm(5)
        Else
            TipoScansione = 0
        End If

        '%%02
        Select Case TiposDB
            Case 0
                vRit(0) = 1
                vRit(1) = "tipo apertura non più utilizzata"
                Call SISTEMA.Registra_Log(vRit(1))
                DBQueryPagina = VB6.CopyArray(vRit)
                GoTo FineSub
            Case 1
                On Error Resume Next
                qDB = SISTEMA.PROCDB.Item(vParm(0))
                On Error GoTo 0
                If qDB Is Nothing Then
                    vRit(0) = 1
                    vRit(1) = "Archivio " & sDb & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
                    Call SISTEMA.Registra_Log(vRit(1))
                    DBQueryPagina = VB6.CopyArray(vRit)
                    GoTo FineSub
                End If
                If qDB.Stato = clDicGDB.dbCostTipoStato.dbChiuso Then
                    Statodb = 1
                    Call qDB.ApriDB()
                    If qDB.Errore <> 0 Then
                        vRit(0) = qDB.Errore
                        vRit(1) = qDB.ErrDescr
                        Call SISTEMA.Registra_Log(vRit(1))
                        DBQueryPagina = VB6.CopyArray(vRit)
                        GoTo FineSub
                    End If
                End If
            Case 2
                qDB = vParm(0)
            Case 3
                qDB = New clAdoDB
                qDB.Nome = vParm(0)
                '        qDB.ConnectionString = vParm(6)
                Statodb = 1
                Call qDB.ApriDB(vParm(6))
                If qDB.Errore <> 0 Then
                    vRit(0) = qDB.Errore
                    vRit(1) = qDB.ErrDescr
                    Call SISTEMA.Registra_Log(vRit(1))
                    DBQueryPagina = VB6.CopyArray(vRit)
                    GoTo FineSub
                End If
        End Select

        sSql = vParm(1)
        vR = qDB.Leggi_Pagina(sSql, NRighe, nPagina, TipoScansione)
        If qDB.Errore <> 1 And qDB.Errore <> 0 Then
            Call SISTEMA.Registra_Log(qDB.ErrDescr, "DBQueryPAgina")
            vRit(0) = qDB.Errore
            vRit(1) = qDB.ErrDescr
        End If
        DBQueryPagina = vR
FineSub:
        If Statodb = 1 Then
            Call qDB.ChiudiDB()
            qDB.DB = Nothing
        End If
        On Error Resume Next
        System.Array.Clear(vRit, 0, vRit.Length)
        Erase vR

    End Function

    '%%01
    '$$I----------------------------------------------------------------------------
    'Funzione : DBQueryMultipla(vParm) -> vDati
    '           Lancia la query e ritorna il risulato
    '
    'Parametri: [DataBase;Query Sql;Tipo Parametro Database]
    '             DataBase = percorso database/ Nome di procedura del database
    '             Tipo Parametro DataBase = 0 se DataBase comprende il percorso
    '                                     = 1 se è il nome usato nella procedura del database
    '                                     = 2 DataBase
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio / Matrice Dati]
    '                   MatriceDati = [Valore(Campo,Record)]
    '$$F----------------------------------------------------------------------------
    Public Function DBQueryMultipla(ByVal vParm As Object) As Object
        Dim qDB As Object
        Dim sDb, sSql As String
        Dim TiposDB As Short
        Dim i As Short
        Dim NumRec As Single
        Dim Statodb As Short
        Dim vRit As Object
        Dim sConnessione As String
        Dim bStruttura As Boolean

        Statodb = 0
        bStruttura = False
        sConnessione = ""

        Dim vR As Object = Nothing
        TiposDB = vParm(1)
        NumRec = 0
        ReDim vRit(UBound(vParm))
        '%%02
        Select Case TiposDB
            Case 0
                vRit(0) = 1
                vRit(1) = "tipo apertura non più utilizzata"
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            Case 1
                On Error Resume Next
                qDB = SISTEMA.PROCDB.Item(vParm(0))
                On Error GoTo 0
                If qDB Is Nothing Then
                    vRit(0) = 1
                    vRit(1) = "Archivio " & sDb & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
                    Call SISTEMA.Registra_Log(vRit(1))
                    GoTo FineSub
                End If
                If qDB.Stato = clDicGDB.dbCostTipoStato.dbChiuso Then
                    '            If qDB.Tipo = dbjet Then
                    '               Set qDB.WSCL = Ws
                    '            End If
                    Statodb = 1
                    Call qDB.ApriDB()
                    If qDB.Errore <> 0 Then
                        vRit(0) = qDB.Errore
                        vRit(1) = qDB.ErrDescr
                        Call SISTEMA.Registra_Log(vRit(1))
                        GoTo FineSub
                    End If
                End If
            Case 2
                qDB = vParm(0)
        End Select

        For i = 2 To UBound(vParm)
            sSql = vParm(i)
            vRit(i) = qDB.GetRows(sSql, 0)
            If qDB.Errore <> 0 Then
                vRit(i) = Nothing
                If qDB.Errore <> 1 Then
                    vRit(0) = qDB.Errore
                    vRit(1) = qDB.ErrDescr
                    Call SISTEMA.Registra_Log(vRit(1))
                End If
                '                GoTo FineSub
            End If
        Next i
        vRit(0) = 0

FineSub:
        DBQueryMultipla = vRit
        If Statodb = 1 Then
            Call qDB.ChiudiDB()
            qDB.DB = Nothing
        End If
        Erase vRit

    End Function

    '%%01
    '$$I----------------------------------------------------------------------------
    'Funzione : DBQUERY(vParm) -> vDati
    '           Lancia la query e ritorna il risulato
    '
    'Parametri: [DataBase;Query Sql;Tipo Parametro Database]
    '             DataBase = percorso database/ Nome di procedura del database
    '             Tipo Parametro DataBase = 0 se DataBase comprende il percorso
    '                                     = 1 se è il nome usato nella procedura del database
    '                                     = 2 DataBase
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio / Matrice Dati]
    '                   MatriceDati = [Valore(Campo,Record)]
    '$$F----------------------------------------------------------------------------
    Public Function DBQuery(ByVal vParm As Object) As Object
        Dim qDB As Object
        Dim sDb, sSql As String
        Dim TiposDB As Short
        Dim i As Short
        Dim NumRec As Single
        Dim Statodb As Short
        Dim vRit(4) As Object
        Dim sConnessione As String
        Dim bStruttura As Boolean
        Dim Ritorno As Byte
        Dim s As String

        Statodb = 0
        bStruttura = False
        sConnessione = ""

        Dim vR As Object = Nothing
        If UBound(vParm) > 1 Then
            TiposDB = vParm(2)
        Else
            TiposDB = 0
        End If
        NumRec = 10000
        If UBound(vParm) > 2 Then
            If IsNumeric(vParm(3)) Then
                NumRec = vParm(3)
            End If
        End If

        If UBound(vParm) > 3 Then
            If IsNothing(vParm(4)) Then
                bStruttura = False
            Else
                If IsNumeric(vParm(4)) Then
                    If vParm(4) = 1 Then
                        bStruttura = True
                    End If
                End If
            End If
        End If


        If UBound(vParm) > 4 Then
            If Not IsNothing(vParm(5)) Then
                If Not IsArray(vParm(5)) And Not IsNumeric(vParm(5)) Then
                    sConnessione = vParm(5)
                End If
            End If
        End If

        If UBound(vParm) > 5 Then
            If Not IsNothing(vParm(6)) Then
                If IsNumeric(vParm(6)) Then
                    Ritorno = vParm(6)
                End If
            End If
        End If

        sSql = vParm(1)

        '%%02
        Select Case TiposDB
            Case 0
                vRit(0) = 1
                vRit(1) = "tipo apertura non più utilizzata"
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            Case 1
                On Error Resume Next
                qDB = SISTEMA.PROCDB.Item(vParm(0))
                On Error GoTo 0
                If qDB Is Nothing Then
                    vRit(0) = 1
                    vRit(1) = "Archivio " & sDb & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
                    Call SISTEMA.Registra_Log(vRit(1))
                    GoTo FineSub
                End If
                If qDB.Stato = clDicGDB.dbCostTipoStato.dbChiuso Then
                    Statodb = 1
                    If sConnessione <> "" Then
                        Call qDB.ApriDB(, , , , sConnessione)
                    Else
                        Call qDB.ApriDB()
                    End If
                    If qDB.Errore <> 0 Then
                        vRit(0) = qDB.Errore
                        vRit(1) = qDB.ErrDescr
                        Call SISTEMA.Registra_Log(vRit(1))
                        GoTo FineSub
                    End If
                End If
            Case 2
                qDB = vParm(0)
            Case 3
                qDB = New clAdoDB
                qDB.Nome = vParm(0)
                Statodb = 1
                Call qDB.ApriDB(sConnessione)
                If qDB.Errore <> 0 Then
                    vRit(0) = qDB.Errore
                    vRit(1) = qDB.ErrDescr
                    Call SISTEMA.Registra_Log(vRit(1))
                    GoTo FineSub
                End If
        End Select
        sSql = vParm(1)
        If Ritorno = 0 Then
            If bStruttura Then
                vR = qDB.GetRows(sSql, NumRec, 1)
                vRit(1) = vR(0)
                vRit(4) = vR(1)
            Else
                vRit(1) = qDB.GetRows(sSql, NumRec)
            End If
            If qDB.Errore <> 1 And qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                If qDB.Errore <> 1 Then
                    Call SISTEMA.Registra_Log(vRit(1))
                End If
                GoTo FineSub
            End If
            If Not IsNothing(vRit(1)) Then
                vRit(2) = UBound(vRit(1), 2) + 1 'Numero di record letti = Colonne della matrice
                vRit(3) = UBound(vRit(1), 1) + 1 'Numero campi = Righe della matrice
            End If
        ElseIf Ritorno = 1 Then
            s = qDB.GetString(sSql, NumRec)
            vRit(0) = 0
            vRit(1) = s
        ElseIf Ritorno = 2 Then
            s = qDB.GetXml(sSql, NumRec)
            vRit(0) = 0
            vRit(1) = s
        End If
        If qDB.Errore = 1 Then
            vRit(0) = 1
            vRit(1) = "Nessun Record Trovato"
            GoTo FineSub

        End If

        vRit(0) = 0
        'vRit(2) = UBound(vRit(1), 2) + 1 'Numero di record letti = Colonne della matrice
        'vRit(3) = UBound(vRit(1), 1) + 1 'Numero campi = Righe della matrice

FineSub:
        DBQuery = VB6.CopyArray(vRit)
        If Statodb = 1 Then
            Call qDB.ChiudiDB()
            qDB.DB = Nothing
        End If
        System.Array.Clear(vRit, 0, vRit.Length)
    End Function


    '%%02
    '$$I----------------------------------------------------------------------------
    'Funzione : DBInsert_SQL(DB As Database, NomeTab, TabPar) As Variant
    '           Costruisce la query di Inserimento e la lancia
    '
    'Parametri: DataBase = Data base su cui operare
    '           NomeTab = tabella su cui operare
    '           TabPar = Tabella Parametri [[Campo;Valore;[TipoDato]]]
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio]
    '$$F----------------------------------------------------------------------------
    Public Function DBInsert_SQL(ByRef DB As Object, ByRef NomeTab As String, ByRef TabPar As Object) As Object
        Dim j As Short
        Dim StrInsert As String
        Dim SS As String
        Dim SV As String
        Dim AppStr As String
        Dim AppStr1 As String
        Dim vRitPar(2) As Object
        Dim StrSQL As String
        Dim TipoDato As Object
        Dim bTipoInTab As Boolean
        Dim s As Single
        Dim L As Integer

        vRitPar(0) = 0
        vRitPar(1) = ""
        On Error GoTo Herr
        StrInsert = "INSERT INTO " & NomeTab & " "

        SS = "( "

        SV = "VALUES ("

        If IsArray(TabPar) Then
        Else
            GoTo FineSub
        End If

        If UBound(TabPar, 2) = 2 Then
            If TabPar(0, 2) <> "" Then
                bTipoInTab = True
            End If
        End If

        For j = 0 To UBound(TabPar)
            If TabPar(j, 0) & "" = "" Or TabPar(j, 1) & "" = "" Then
                GoTo FineLoop
            End If
            If IsDBNull(TabPar(j, 1)) Then
                GoTo FineLoop
            End If


            If j > 0 Then
                SS = SS & ","
                SV = SV & ","
            End If
            SS = SS & TabPar(j, 0)

            If bTipoInTab Then
                TipoDato = TipoDBtoVB(TabPar(j, 2), DB.Tipo)
            Else
                'UPGRADE_WARNING: VarType ha un nuovo comportamento. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
                'Raffaele 30/11/2004 conversione
                If VarType(TabPar(j, 1)) = VariantType.Object Then
                    TipoDato = VariantType.Empty
                Else
                    TipoDato = VarType(TabPar(j, 1))
                End If
            End If
            Select Case TipoDato
                Case VariantType.Integer, VariantType.Short, VariantType.Byte, VariantType.Decimal, VariantType.Single, VariantType.Double '%01Mario eliminare il tipo currency
                    If Int(CDbl(TabPar(j, 1))) <> TabPar(j, 1) Then
                        SV = SV & "'" & TabPar(j, 1) & "'"
                    Else
                        SV = SV & TabPar(j, 1)
                    End If
                Case VariantType.Decimal
                    If Int(CDbl(TabPar(j, 1))) <> TabPar(j, 1) Then
                        If DB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                            SV = SV & "'" & TabPar(j, 1) & "'"
                        Else
                            SV = SV & Replace(TabPar(j, 1), ",", ".")
                        End If
                    Else
                        SV = SV & TabPar(j, 1)
                    End If
                Case VariantType.Date
                    If DB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                        SV = SV & "#" & VB6.Format(TabPar(j, 1), "mm/dd/yyyy") & "#"
                    Else
                        AppStr = VB6.Format(TabPar(j, 1), "mm/dd/yyyy")
                        AppStr = Replace(AppStr, ".", ":")
                        SV = SV & "'" & AppStr & "'"
                    End If
                Case VariantType.String
                    If bTipoInTab Then
                        AppStr1 = TabPar(j, 1)
                        AppStr = Ctrl_Apici(AppStr1)
                        SV = SV & "'" & AppStr & "'"
                    Else
                        If IsNumeric(TabPar(j, 1)) Then
                            SV = SV & "'" & TabPar(j, 1) & "'"
                        ElseIf IsDate(TabPar(j, 1)) Then
                            If DB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                                SV = SV & "#" & VB6.Format(TabPar(j, 1), "mm/dd/yyyy") & "#"
                            Else
                                SV = SV & "'" & VB6.Format(TabPar(j, 1), "mm/dd/yyyy") & "'"
                            End If
                            '                  SV = SV & "#" & Format(TabPar(j, 1), "mm/dd/yyyy") & "#"
                        Else
                            AppStr1 = TabPar(j, 1)
                            AppStr = Ctrl_Apici(AppStr1)
                            SV = SV & "'" & AppStr & "'"
                        End If
                    End If
                Case VariantType.Boolean
                    Select Case TabPar(j, 1)
                        Case "Falso", "False", 0
                            SV = SV & 0
                        Case Else
                            SV = SV & 1
                    End Select
                Case VariantType.Empty, VariantType.Null
                    SV = SV & "Null"
                Case Else
                    'UPGRADE_WARNING: VarType ha un nuovo comportamento. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
                    'Dovrebbe andare bene perchè è un tipo non gestito diverso da object che abbiamo indicato come variant empty
                    SISTEMA.Registra_Log(("Tipo Dato errato " & VarType(TabPar(j, 1)) & " " & TabPar(j, 0)))
                    vRitPar(0) = 1
                    vRitPar(1) = "Insert_Sql:Tipo Dato errato " & VarType(TabPar(j, 1)) & " " & TabPar(j, 0)
                    GoTo FineSub
FineLoop:
            End Select

        Next j
        SS = SS & ") "
        SV = SV & ")"

        StrSQL = StrInsert & SS & SV

        'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto DB.Execute. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
        Call DB.Execute(StrSQL)
        If DB.Errore <> 0 Then
            vRitPar(0) = DB.Errore
            vRitPar(1) = DB.ErrDescr
            Call SISTEMA.Registra_Log(DB.ErrDescr & " Errore ExecuteSQL :" & StrSQL)
            GoTo FineSub
        Else
            StrSQL = ""
        End If

FineSub:
        DBInsert_SQL = VB6.CopyArray(vRitPar)
        Exit Function

Herr:
        vRitPar(0) = Err.Number
        vRitPar(1) = Err.Description

        GoTo FineSub
    End Function

    '%%02
    '$$I----------------------------------------------------------------------------
    'Funzione : DBUpdate_SQL(DB As Database, NomeTab, sWhere) As Variant
    '           Costruisce la query di Aggiornamento e la lancia
    '
    'Parametri: DataBase = Data base su cui operare
    '           NomeTab = tabella su cui aggiornare
    '           TabPar = Tabella Parametri [[Campo;Valore;[Tipo]]]
    '           sWhere = condizione di selezione
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio]
    '$$F----------------------------------------------------------------------------
    Public Function DBUpdate_SQL(ByRef DB As Object, ByRef NomeTab As String, ByRef TabPar As Object, ByRef sWhere As String) As Object
        Dim j As Short
        Dim StrUpdate As String
        Dim SS As String
        Dim SV As String
        Dim AppStr As String
        Dim AppStr1 As String
        Dim vRitPar(2) As Object
        Dim StrSQL As String
        Dim bTipoInTab As Boolean
        Dim TipoDato As Object

        vRitPar(0) = 0
        vRitPar(1) = ""

        StrUpdate = "UPDATE " & NomeTab & " "

        SV = "SET "

        If sWhere <> "" Then
            SS = " WHERE " & sWhere
        Else
            SS = ""
        End If
        If IsArray(TabPar) Then
        Else
            GoTo FineSub
        End If

        If UBound(TabPar, 2) = 2 Then
            If TabPar(0, 2) <> "" Then
                bTipoInTab = True
            End If
        End If

        For j = 0 To UBound(TabPar)
            If TabPar(j, 0) & "" = "" Then
                GoTo FineLoop
            End If
            If IsDBNull(TabPar(j, 1)) Then
                GoTo FineLoop
            End If
            If j > 0 Then
                SV = SV & ","
            End If
            If bTipoInTab Then
                TipoDato = TipoDBtoVB(TabPar(j, 2))
            Else
                'Raffaele 30/11/2004 conversione
                If VarType(TabPar(j, 1)) = VariantType.Object Then
                    TipoDato = VariantType.Empty
                Else
                    TipoDato = VarType(TabPar(j, 1))
                End If
            End If
            Select Case TipoDato
                Case VariantType.Integer, VariantType.Short, VariantType.Byte, VariantType.Decimal, VariantType.Single, VariantType.Double
                    If Int(CDbl(TabPar(j, 1))) <> TabPar(j, 1) Then
                        SV = SV & TabPar(j, 0) & " = '" & TabPar(j, 1) & "'"
                    Else
                        SV = SV & TabPar(j, 0) & " = " & TabPar(j, 1)
                    End If
                Case VariantType.Decimal
                    If Int(CDbl(TabPar(j, 1))) <> TabPar(j, 1) Then
                        If DB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                            SV = SV & TabPar(j, 0) & " = '" & TabPar(j, 1) & "'"
                        Else
                            SV = SV & TabPar(j, 0) & " = " & Replace(TabPar(j, 1), ",", ".")
                        End If
                    Else
                        SV = SV & TabPar(j, 0) & " = " & TabPar(j, 1)
                    End If
                Case VariantType.Date
                    SV = SV & TabPar(j, 0) & " = " & "#" & VB6.Format(TabPar(j, 1), "mm/dd/yyyy") & "#"
                Case VariantType.String
                    If bTipoInTab Then
                        AppStr1 = TabPar(j, 1)
                        AppStr = Ctrl_Apici(AppStr1)
                        SV = SV & TabPar(j, 0) & " = " & "'" & AppStr & "'"
                    Else
                        If IsNumeric(TabPar(j, 1)) Then
                            SV = SV & TabPar(j, 0) & " = '" & TabPar(j, 1) & "'"
                        ElseIf IsDate(TabPar(j, 1)) Then
                            SV = SV & TabPar(j, 0) & " = " & "#" & VB6.Format(TabPar(j, 1), "mm/dd/yyyy") & "#"
                        Else
                            AppStr1 = TabPar(j, 1)
                            AppStr = Ctrl_Apici(AppStr1)
                            SV = SV & TabPar(j, 0) & " = " & "'" & AppStr & "'"
                        End If
                    End If
                Case VariantType.Boolean
                    Select Case TabPar(j, 1)
                        Case "Falso", "False", 0
                            SV = SV & TabPar(j, 0) & " = " & 0
                        Case Else
                            SV = SV & TabPar(j, 0) & " = " & 1
                    End Select
                Case VariantType.Empty, VariantType.Null
                    SV = SV & TabPar(j, 0) & " = " & "Null"
                Case Else
                    SISTEMA.Registra_Log(("Tipo Dato errato " & VarType(TabPar(j, 1)) & " " & TabPar(j, 0)))
                    vRitPar(0) = 1
                    vRitPar(1) = "Update_SQL:Tipo Dato errato " & VarType(TabPar(j, 1)) & " " & TabPar(j, 0)
                    GoTo FineSub
FineLoop:
            End Select

        Next j

        StrSQL = StrUpdate & SV & SS

        Call DB.Execute(StrSQL, DAO.RecordsetOptionEnum.dbFailOnError)
        If DB.Errore <> 0 Then
            vRitPar(0) = DB.Errore
            vRitPar(1) = DB.ErrDescr
            Call SISTEMA.Registra_Log(DB.ErrDescr & " Errore ExecuteSQL :" & StrSQL)
            GoTo FineSub
        Else
            StrSQL = ""
        End If

FineSub:
        DBUpdate_SQL = VB6.CopyArray(vRitPar)
    End Function



    '%%02
    '$$I----------------------------------------------------------------------------
    'Funzione : DBExecute_Transazione(Vett_Agg) As Variant
    '           esegue i comandi contenuti nel vettore Vett_Agg
    '
    'Parametri:
    '           Vett_agg (0) = database sul quale operare
    '           Vett_agg (.) = stringa contenente il comando da eseguire
    '           Vett_agg (y) = stringa contenente il comando da eseguire
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio]
    '$$F----------------------------------------------------------------------------
    Public Function DBExecute_Transazione(ByRef Vett_Agg As Object) As Object
        Dim Num, k As Short
        Dim vRitOle(2) As Object
        Dim DBase As Object
        Dim dbStato As Short
        Dim s As String

        Num = UBound(Vett_Agg)
        vRitOle(0) = 0
        vRitOle(1) = ""

        '--- esecuzione dell'elenco dei comandi ---------------
        DBase = SISTEMA.PROCDB.Item(UCase(Vett_Agg(0)))
        If DBase Is Nothing Then
            Call SISTEMA.Registra_Log("archivio non trovato: " & Vett_Agg(0), "DBExecute_Transazione")
            vRitOle(0) = 999100
            vRitOle(1) = "archivio non trovato: " & Vett_Agg(0)
            GoTo FineSub
        End If

        dbStato = 0
        If DBase.Stato = 1 Then 'MA301
            dbStato = 1
            Call DBase.ApriDB()
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "DBExecute_Transazione")
                GoTo FineSub
            End If
        End If

        Call DBase.BeginTrans()
        If DBase.Errore <> 0 Then
            vRitOle(0) = DBase.Errore
            vRitOle(1) = DBase.ErrDescr
            Call SISTEMA.Registra_Log(vRitOle(1), "DBExecute_Transazione")
            GoTo FineSub
        End If

        For k = 1 To Num
            If IsNothing(Vett_Agg(k)) Then Exit For
            s = Vett_Agg(k)
            Call DBase.Execute(s)
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "DBExecute_Transazione")
                GoTo RollTrans
            End If
        Next k

        Call DBase.CommitTrans()
        If DBase.Errore <> 0 Then
            vRitOle(0) = DBase.Errore
            vRitOle(1) = DBase.ErrDescr
            Call SISTEMA.Registra_Log(vRitOle(1), "DBExecute_Transazione")
            GoTo FineSub
        End If

        If dbStato = 1 Then
            DBase.ChiudiDB()
        End If

FineSub:
        If dbStato = 1 Then
            DBase.ChiudiDB()
        End If
        DBExecute_Transazione = VB6.CopyArray(vRitOle)
        Exit Function

RollTrans:
        Call DBase.RollTrans()
        If DBase.Errore <> 0 Then
            Call SISTEMA.Registra_Log(DBase.ErrDescr, "DBExecute_Transazione")
        End If
        GoTo FineSub

    End Function



    '%%02
    '$$I----------------------------------------------------------------------------
    'Funzione : DB_Execute(Vett_Agg) As Variant
    '           esegue i comandi contenuti nel vettore Vett_Agg
    '
    'Parametri:
    '           Vett_agg (x,0) = database sul quale operare)
    '           Vett_agg (x,1) = stringa contenente il comando da eseguire
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio]
    '$$F----------------------------------------------------------------------------
    Public Function DB_Execute(ByRef Vett_Agg As Object) As Object
        Dim Num, k As Short
        Dim vRitOle(2) As Object
        Dim DBase As Object
        Dim dbStato As Short
        Dim Sqlq As String

        Num = UBound(Vett_Agg)
        vRitOle(0) = 0
        vRitOle(1) = ""

        '--- esecuzione dell'elenco dei comandi ---------------
        For k = 0 To Num
            If IsNothing(Vett_Agg(k, 0)) Then Exit For
            If Trim(Vett_Agg(k, 0) & "") = "" Then Exit For
            DBase = Nothing
            DBase = SISTEMA.PROCDB.Item(UCase(Vett_Agg(k, 0)))
            If DBase Is Nothing Then
                Call SISTEMA.Registra_Log("archivio non trovato: " & Vett_Agg(k, 0), "DB_Execute")
                Exit For
            End If
            dbStato = 0
            If DBase.DB Is Nothing Or DBase.Stato = clDicGDB.dbCostTipoStato.dbChiuso Then
                dbStato = 1
                Call DBase.ApriDB()
                If DBase.Errore <> 0 Then
                    vRitOle(0) = DBase.Errore
                    vRitOle(1) = DBase.ErrDescr
                    GoTo RollTrans
                End If
            End If

            Call DBase.BeginTrans()
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "Db_Execute")
                GoTo FineSub
            End If

            Sqlq = Vett_Agg(k, 1)
            Call DBase.Execute(Sqlq)
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "Db_Execute")
                GoTo RollTrans
            End If

            Call DBase.CommitTrans()
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "Db_Execute")
                GoTo FineSub
            End If
            If dbStato = 1 Then
                DBase.ChiudiDB()
            End If
        Next k


FineSub:

        DBase.ChiudiDB()
        DB_Execute = VB6.CopyArray(vRitOle)
        Exit Function

RollTrans:
        Call DBase.RollTrans()
        If DBase.Errore <> 0 Then
            Call SISTEMA.Registra_Log(DBase.ErrDescr, "Db_Execute")
        End If
        GoTo FineSub

    End Function


    Private Function TipoDBtoVB(ByRef Tipo As Object, Optional ByRef TipoDb As clDicGDB.dbCostTipoDB = clDicGDB.dbCostTipoDB.dbjet) As Short

        Select Case TipoDb
            Case clDicGDB.dbCostTipoDB.dbjet
                Select Case Tipo
                    Case DAO.DataTypeEnum.dbBigInt, DAO.DataTypeEnum.dbLong
                        TipoDBtoVB = VariantType.Integer
                    Case DAO.DataTypeEnum.dbBinary, DAO.DataTypeEnum.dbGUID, DAO.DataTypeEnum.dbChar, DAO.DataTypeEnum.dbLongBinary, DAO.DataTypeEnum.dbText, DAO.DataTypeEnum.dbMemo
                        TipoDBtoVB = VariantType.String
                    Case DAO.DataTypeEnum.dbBoolean
                        TipoDBtoVB = VariantType.Boolean
                    Case DAO.DataTypeEnum.dbByte
                        TipoDBtoVB = VariantType.Byte
                    Case DAO.DataTypeEnum.dbCurrency
                        TipoDBtoVB = VariantType.Decimal
                    Case DAO.DataTypeEnum.dbDate, DAO.DataTypeEnum.dbTime, DAO.DataTypeEnum.dbTimeStamp
                        TipoDBtoVB = VariantType.Date
                    Case DAO.DataTypeEnum.dbDecimal
                        TipoDBtoVB = VariantType.Decimal
                    Case DAO.DataTypeEnum.dbDouble, DAO.DataTypeEnum.dbFloat
                        TipoDBtoVB = VariantType.Double
                    Case DAO.DataTypeEnum.dbInteger
                        TipoDBtoVB = VariantType.Short
                    Case DAO.DataTypeEnum.dbSingle
                        TipoDBtoVB = VariantType.Single
                    Case Else
                        TipoDBtoVB = VariantType.String
                End Select
            Case clDicGDB.dbCostTipoDB.dbADOSQL
                Select Case Tipo
                    Case ADOR.DataTypeEnum.adBigInt, ADOR.DataTypeEnum.adTinyInt, ADOR.DataTypeEnum.adNumeric
                        TipoDBtoVB = VariantType.Integer
                    Case ADOR.DataTypeEnum.adBinary, ADOR.DataTypeEnum.adChar, DAO.DataTypeEnum.dbText, ADOR.DataTypeEnum.adLongVarChar, ADOR.DataTypeEnum.adLongVarWChar
                        TipoDBtoVB = VariantType.String
                    Case ADOR.DataTypeEnum.adBoolean
                        TipoDBtoVB = VariantType.Boolean
                    Case ADOR.DataTypeEnum.adCurrency
                        TipoDBtoVB = VariantType.Decimal
                    Case ADOR.DataTypeEnum.adDate, ADOR.DataTypeEnum.adDBDate, ADOR.DataTypeEnum.adDBTime, ADOR.DataTypeEnum.adDBTime, ADOR.DataTypeEnum.adDBTimeStamp
                        TipoDBtoVB = VariantType.Date
                    Case ADOR.DataTypeEnum.adDecimal
                        TipoDBtoVB = VariantType.Decimal
                    Case ADOR.DataTypeEnum.adDouble
                        TipoDBtoVB = VariantType.Double
                    Case ADOR.DataTypeEnum.adGUID
                        TipoDBtoVB = VariantType.String
                    Case ADOR.DataTypeEnum.adInteger, ADOR.DataTypeEnum.adTinyInt, ADOR.DataTypeEnum.adSmallInt
                        TipoDBtoVB = VariantType.Short
                    Case DAO.DataTypeEnum.dbLongBinary
                        TipoDBtoVB = VariantType.String
                    Case DAO.DataTypeEnum.dbSingle, ADOR.DataTypeEnum.adSingle
                        TipoDBtoVB = VariantType.Single
                    Case DAO.DataTypeEnum.dbVarBinary, ADOR.DataTypeEnum.adLongVarBinary
                        TipoDBtoVB = VariantType.String
                    Case Else
                        TipoDBtoVB = VariantType.String
                End Select
        End Select
    End Function


    '$$I------------------------------------------------------------
    'Funzione : Ctrl_Apici(ArgRice As String) As Boolean
    '          Raddoppia gli apici singola sulla stringa
    '
    'Parametri: ArgRice = stringa da controllare
    '
    'Ritorno  : Stringa ricostruita con apici raddoppiati
    '$$F------------------------------------------------------------
    Private Function Ctrl_Apici(ByRef ArgRice As String) As String
        Dim AppRicerca As String
        Dim Y1 As Short

        AppRicerca = ""
        For Y1 = 1 To Len(ArgRice)
            If Mid(ArgRice, Y1, 1) = "'" Then
                If Len(AppRicerca) > 0 Then
                    AppRicerca = AppRicerca & "''"
                End If
            Else
                AppRicerca = AppRicerca & Mid(ArgRice, Y1, 1)
            End If
        Next Y1
        Ctrl_Apici = AppRicerca

    End Function


    '$$I----------------------------------------------------
    'Funzione : DB_GetQueryDef(ByVal vParm) As Variant
    '           Legge le query secondo il filtro di lettura
    'Parametri: Filtro= "" tutte le predefinite
    '                   "*" tutte
    'Rirotno   [Esito;messaggio/[Vettore Query]
    '$$F----------------------------------------------------
    Public Function DB_GetQueryDef(ByVal vParm As Object) As Object
        Dim vRitOle(3) As Object
        Dim vRit(2) As Object
        Dim QDef, QD As DAO.QueryDef
        Dim QPar As DAO.Parameter
        Dim Qfield As DAO.Field
        Dim i, j As Short
        Dim vQuery As Object
        Dim vQPar(,) As Object
        Dim vQField(,) As Object
        Dim sSql As String
        Dim N As Short
        Dim qDB As Object
        Dim DB As DAO.Database
        Dim Statodb As Short

        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Leggi_QueryDef-init")
        End If

        Statodb = 0


        On Error Resume Next
        qDB = SISTEMA.PROCDB.Item(vParm(0))
        On Error GoTo 0
        If qDB Is Nothing Then
            vRit(0) = 1
            vRit(1) = "Archivio " & vParm(0) & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
            Call SISTEMA.Registra_Log(vRit(1))
            GoTo FineSub
        End If
        If qDB.DB Is Nothing Then
            Statodb = 1
            Call qDB.ApriDB()
            If qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            End If
        End If
        On Error GoTo ErrLeggi
        If qDB.Tipo <> clDicGDB.dbCostTipoDB.dbjet Then
            vRit(0) = 1
            vRit(1) = "Funzione non disponibile per archivi ADO"
            Call SISTEMA.Registra_Log(vRit(1))
            GoTo FineSub
        End If
        DB = qDB.DB
        ReDim vQuery(DB.QueryDefs.Count - 1, 2)
        i = -1
        N = Len(vParm(0))
        DB.QueryDefs.Refresh()
        For Each QDef In DB.QueryDefs
            If N = 0 Then
                '--- Scarto le query applicative e le Viste
                If UCase(Mid(QDef.Name, 1, 2)) <> "QR_" And UCase(Mid(QDef.Name, 1, 2)) <> "V_" Then
                    GoTo Prossimo
                End If
            Else
                '--- se c'è un filtro scarto quelle con non lo soddisfano
                If vParm(0) <> "*" And (StrComp(Mid(QDef.Name, 1, N), vParm(0), CompareMethod.Text) <> 0) Then
                    GoTo Prossimo
                End If
            End If
            '--- carico i campi della query
            i = i + 1
            sSql = QDef.SQL
            vQuery(i, 0) = QDef.Name
            ReDim vQField(QDef.Fields.Count - 1, 1)
            j = -1
            For Each Qfield In QDef.Fields
                j = j + 1
                vQField(j, 0) = Qfield.Name
                vQField(j, 1) = Qfield.Type
            Next Qfield
            vQuery(i, 1) = VB6.CopyArray(vQField)

            '---Verifico se ci sono parametri
            ReDim vQPar(QDef.Parameters.Count - 1, 1)
            For Each QPar In QDef.Parameters
                j = j + 1
                vQPar(j, 0) = QPar.Name
                vQPar(j, 1) = QPar.Type
            Next QPar
            vQuery(i, 2) = VB6.CopyArray(vQPar)

Prossimo:
        Next QDef

        vRitOle(0) = 0
        vRitOle(1) = vQuery

FineSub:
        DB_GetQueryDef = VB6.CopyArray(vRitOle)
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Leggi_QueryDef-fine")
        End If

        Exit Function

ErrLeggi:
        Call SISTEMA.Registra_Log("Leggi_QueryDef, errore generico " & Err.Number & " - " & Err.Description)
        vRitOle(0) = Err.Number
        vRitOle(1) = "Errore in Lettura record: " & Err.Description
        Resume Next
        GoTo FineSub
    End Function


    '$$I----------------------------------------------------------------------------
    'Funzione : DB_GetTableDef(vParm) -> vDati
    '           Richiede le strutture dell'Archivio in Input
    '
    'Parametri: [DataBase;Tipo Archivio]
    '
    'Ritorno  : vRitOle=[Codice errore; Messaggio / Matrice Dati]
    '                   MatriceDati = [Tabella,[Campo,tipo]]
    '$$F----------------------------------------------------------------------------
    Public Function DB_GetTableDef(ByVal vParm As Object) As Object
        Dim qDB As Object
        Dim i As Short
        Dim Statodb As Short
        Dim vRit(1) As Object
        Dim vTabStrut As Object = Nothing
        Dim vR As Object = Nothing
        Dim NT As Short
        Dim sTab As String
        Dim k As Short

        Statodb = 0


        On Error Resume Next
        qDB = SISTEMA.PROCDB.Item(vParm(0))
        On Error GoTo 0
        If qDB Is Nothing Then
            vRit(0) = 1
            vRit(1) = "Archivio " & vParm(0) & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
            Call SISTEMA.Registra_Log(vRit(1))
            GoTo FineSub
        End If
        If qDB.DB Is Nothing Then
            Statodb = 1
            Call qDB.ApriDB()
            If qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            End If
        End If

        With qDB.DB
            If qDB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                NT = .TableDefs.Count
            Else
                NT = .Tables.Count
            End If
            ReDim vTabStrut(NT, 1)
            k = -1
            For i = 0 To NT - 1
                If qDB.Tipo = clDicGDB.dbCostTipoDB.dbjet Then
                    sTab = .TableDefs(i).Name
                Else
                    sTab = .Tables(i)
                End If
                If Left(sTab, 4) <> "MSys" Then
                    vR = qDB.TabStruttura(sTab)
                    If qDB.Errore <> 1 And qDB.Errore <> 0 Then
                        vRit(0) = qDB.Errore
                        vRit(1) = qDB.ErrDescr
                        If qDB.Errore <> 1 Then
                            Call SISTEMA.Registra_Log(vRit(1))
                        End If
                    Else
                        k = k + 1
                        vTabStrut(k, 0) = sTab
                        vTabStrut(k, 1) = vR
                    End If
                End If
            Next i
        End With
        vRit(0) = 0
        vRit(1) = vTabStrut

FineSub:
        DB_GetTableDef = VB6.CopyArray(vRit)
        If Statodb = 1 Then
            Call qDB.ChiudiDB()
            qDB.DB = Nothing
        End If
    End Function



    '$$I----------------------------------------------------------------------------
    'Funzione : DB_Strutture(vParm) -> vDati
    '           Richiede le strutture delle Tabelle In Input
    '
    'Parametri: [DataBase;[TABELLE]]
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio / Matrice Dati]
    '                   MatriceDati = [Tabella,[Campo,tipo]]
    '$$F----------------------------------------------------------------------------
    Public Function DB_Strutture(ByVal vParm As Object) As Object
        Dim qDB As Object = Nothing
        Dim i As Short
        Dim Statodb As Short
        Dim vRit(1) As Object
        Dim vTabStrut As Object = Nothing
        Dim vR As Object = Nothing

        Statodb = 0


        On Error Resume Next
        qDB = SISTEMA.PROCDB.Item(vParm(0))
        On Error GoTo 0
        If qDB Is Nothing Then
            vRit(0) = 1
            vRit(1) = "Archivio " & vParm(0) & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
            Call SISTEMA.Registra_Log(vRit(1))
            GoTo FineSub
        End If
        If qDB.DB Is Nothing Then
            Statodb = 1
            Call qDB.ApriDB()
            If qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            End If
        End If

        ReDim vTabStrut(1, UBound(vParm(1)))
        For i = 0 To UBound(vParm(1))
            vR = qDB.TabStruttura(vParm(1)(i))
            If qDB.Errore <> 1 And qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                If qDB.Errore <> 1 Then
                    Call SISTEMA.Registra_Log(vRit(1))
                End If
                GoTo FineSub
            End If
            vTabStrut(0, i) = vParm(1)(i)
            vTabStrut(1, i) = vR
        Next i
        vRit(0) = 0
        vRit(1) = vTabStrut

FineSub:
        DB_Strutture = VB6.CopyArray(vRit)
        If Statodb = 1 Then
            Call qDB.ChiudiDB()
            qDB.DB = Nothing
        End If
    End Function
    '$$I----------------------------------------------------------------------------
    'Funzione : DB_Exec_QueryDef(VParm) As Variant
    '           Crea le query in Input e Ritorna il risultato dellultima
    '
    'Parametri:
    '           VParm (0) = database sul quale operare
    '           vparm(1) = VQDEF  'vettore query
    '           vQdef=[Nome,Sqlq]
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio/Vdati]
    '$$F----------------------------------------------------------------------------
    Public Function DB_Exec_QueryDef(ByRef vParm As Object) As Object
        Dim Num, j, i, k As Short
        Dim vRit(2) As Object
        Dim DBase As DAO.Database
        Dim QDef As DAO.QueryDef
        Dim RS As DAO.Recordset
        Dim vField As Object
        Dim vQdef As Object
        Dim sDb As String
        Dim qDB As Object
        Dim Statodb As Short

        Num = UBound(vParm(1), 2)
        vQdef = vParm(1)
        sDb = ""

        On Error Resume Next
        qDB = SISTEMA.PROCDB.Item(vParm(0))
        On Error GoTo 0
        If qDB Is Nothing Then
            vRit(0) = 1
            vRit(1) = "Archivio " & vParm(0) & " non trovato " & Chr(13) & "Consultare l'esperto della Procedura"
            Call SISTEMA.Registra_Log(vRit(1))
            GoTo FineSub
        End If

        If qDB.Stato = 1 Then
            Statodb = 1
            Call qDB.ApriDB()
            If qDB.Errore <> 0 Then
                vRit(0) = qDB.Errore
                vRit(1) = qDB.ErrDescr
                Call SISTEMA.Registra_Log(vRit(1))
                GoTo FineSub
            End If
        End If
        DBase = qDB.DB


        '--- esecuzione dell'elenco dei comandi ---------------
        For i = 0 To Num - 1
            If vQdef(0, i) <> "" Then
                QDef = Nothing
                On Error Resume Next
                Call DBase.Execute("Drop Table " & vQdef(0, i))

                QDef = DBase.QueryDefs(vQdef(0, i))
                On Error GoTo Herr
                If QDef Is Nothing Then
                    QDef = DBase.CreateQueryDef(vQdef(0, i), vQdef(1, i))
                Else
                    QDef.SQL = vQdef(1, i)
                End If
            End If
        Next i
        RS = DBase.OpenRecordset(vQdef(1, i))
        If RS.EOF Then
            vRit(0) = 1
            vRit(1) = "non ci sono record per le condizioni impostate"
            GoTo FineSub
        End If
        vRit(1) = RS.GetRows(10000)

        ReDim vField(RS.Fields.Count, 2)
        For i = 0 To RS.Fields.Count - 1
            vField(i, 0) = RS.Fields(i).Name
            vField(i, 1) = RS.Fields(i).Type
            vField(i, 2) = RS.Fields(i).Size
        Next i
        vRit(2) = vField
        vRit(0) = 0
FineSub:
        On Error Resume Next
        DBase.Close()
        Call qDB.ChiudiDB()
        DB_Exec_QueryDef = VB6.CopyArray(vRit)
        System.Array.Clear(vRit, 0, vRit.Length)
        Exit Function

Herr:
        vRit(0) = Err.Number
        vRit(1) = Err.Description
        'Resume
        Call SISTEMA.Registra_Log(Err.Description, "DB_Exec_Query")
        GoTo FineSub

    End Function


    Public Function DBCtrl_Campo(ByRef DB As Object, ByRef Tbx As Object, ByRef cx As String) As Object
        Dim vRitFun(3) As Object
        Dim TD As Object
        Dim i As Short
        vRitFun(0) = 0
        vRitFun(1) = ""
        vRitFun(2) = False

        TD = DB.ApriRS(Tbx)
        For i = 0 To TD.Fields.Count
            If UCase(TD.Fields(i)) = UCase(cx) Then
                vRitFun(2) = True
                Exit For
            End If
        Next i

FineSub:
        DBCtrl_Campo = VB6.CopyArray(vRitFun)
    End Function




    '%%02
    '$$I----------------------------------------------------------------------------
    'Funzione : DBExecute_Query(Vett_Agg) As Variant
    '           esegue i comandi contenuti nel vettore Vett_Agg
    '
    'Parametri:
    '           Vett_agg (0) = database sul quale operare
    '           Vett_agg (.) = stringa contenente il comando da eseguire
    '           Vett_agg (y) = stringa contenente il comando da eseguire
    '
    'Ritorno  : vRitOle=[codice errore; Messaggio]
    '$$F----------------------------------------------------------------------------
    Public Function DB_Execute_Query(ByRef vParm As Object) As Object
        Dim Num, k As Short
        Dim vRitOle(3) As Object
        Dim DBase As Object
        Dim dbStato As Short
        Dim s As String
        Dim sDb As String
        Dim sConnessione As String
        Dim vQuery As Object
        Dim vRis As Object
        vQuery = vParm(2)
        sDb = vParm(0)
        sConnessione = vParm(1)

        Num = UBound(vQuery, 2)
        vRitOle(0) = 0
        vRitOle(1) = ""

        If sConnessione <> "" Then
            DBase = New ClJetDB
            DBase.StrConnessione = sConnessione
            DBase.strPercorso = sDb
            DBase.Nome = sDb
            DBase.Tipo = clDicGDB.dbCostTipoDB.dbjet
            DBase.Stato = clDicGDB.dbCostTipoStato.dbChiuso
            DBase.NomeFisico = DBase.Nome
            DBase.WSCL = Ws
            '    sistema.procdb.Add DBase, DBase.Nome
        Else
            DBase = SISTEMA.PROCDB.Item(UCase(sDb))
        End If

        '--- esecuzione dell'elenco dei comandi ---------------
        If DBase Is Nothing Then
            Call SISTEMA.Registra_Log("archivio non trovato: " & sDb, "DBExecute_Query")
            vRitOle(0) = 999100
            vRitOle(1) = "archivio non trovato: " & sDb
            GoTo FineSub
        End If

        dbStato = 0
        If DBase.Stato = 1 Then 'MA301
            dbStato = 1
            Call DBase.ApriDB()
            If DBase.Errore <> 0 Then
                vRitOle(0) = DBase.Errore
                vRitOle(1) = DBase.ErrDescr
                Call SISTEMA.Registra_Log(vRitOle(1), "DB_Execute_Query")
                GoTo FineSub
            End If
        End If

        ReDim vRis(1, Num)
        For k = 0 To Num
            If vQuery(2, k) = 5 Then
                s = vQuery(1, k)
                Call DBase.Execute(s)
                vRis(0, k) = DBase.Errore
                vRis(1, k) = DBase.ErrDescr

                If DBase.Errore <> 0 Then
                    Call SISTEMA.Registra_Log(DBase.ErrDescr, "DB_Execute_Query")
                End If
            ElseIf vQuery(2, k) = 0 Then
                s = vQuery(1, k)
                vRis(1, k) = DBase.GetRows(s, 10000)
                vRis(0, k) = DBase.Errore
                If DBase.Errore <> 1 And DBase.Errore <> 0 Then
                    vRitOle(0) = DBase.Errore
                    vRitOle(1) = DBase.ErrDescr
                    Call SISTEMA.Registra_Log(vRitOle(1), "DB_Execute_Query")
                    GoTo FineSub
                ElseIf DBase.Errore = 1 Then
                    vRitOle(0) = 1
                    vRitOle(1) = "Nessun Record Trovato"
                    GoTo FineSub
                End If
            End If
        Next k


        If dbStato = 1 Then
            DBase.ChiudiDB()
        End If
        vRitOle(0) = 0
        vRitOle(1) = vRis
        vRitOle(2) = ""
FineSub:
        If dbStato = 1 Then
            DBase.ChiudiDB()
        End If
        DBase = Nothing
        DB_Execute_Query = VB6.CopyArray(vRitOle)
        Exit Function

RollTrans:
        Call DBase.RollTrans()
        If DBase.Errore <> 0 Then
            Call SISTEMA.Registra_Log(DBase.ErrDescr, "DB_Execute_Query")
        End If
        GoTo FineSub

    End Function


    Public Function Escludi_Condizione_SQL(ByRef Sqlq As String, ByRef sLike As String) As String
        Dim Trovato As Boolean
        Dim PosStart As Integer
        Dim PosIniz As Integer
        Dim Stringa As String
        Dim N As Short

        N = InStr(Sqlq, "WHERE")
        If N > 0 Then
            PosStart = InStr(N, Sqlq, sLike)
            If PosStart > 0 Then
                PosIniz = PosStart
                Stringa = ""
                Trovato = False
                Do Until Trovato = True
                    Stringa = Mid(Sqlq, PosStart - 1, 1) & Stringa
                    If UCase(Mid(Stringa, 1, 4)) = "AND " Or UCase(Mid(Stringa, 1, 3)) = "OR " Or UCase(Mid(Stringa, 1, 4)) = "WHERE " Then
                        Trovato = True
                        Exit Do
                    End If
                    PosStart = PosStart - 1
                Loop
                Stringa = Mid(Sqlq, PosStart - 1, InStr(PosIniz + 1, Sqlq, sLike) - PosStart + 2)
                Sqlq = Replace(Sqlq, Stringa, "")
            End If
        End If
        Escludi_Condizione_SQL = Sqlq
    End Function

    Private Sub Class_Initialize_Renamed()
        ' DllSistemaClSistema_definst.TimeLock = 50
        'Set sistema.procdb = New Collection
    End Sub

    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    'Private Sub Class_Terminate_Renamed()
    '    'Set sistema.procdb = Nothing
    'End Sub

    Protected Overrides Sub Finalize()
        'Class_Terminate_Renamed()
        SISTEMA = Nothing
        MyBase.Finalize()
    End Sub
End Class