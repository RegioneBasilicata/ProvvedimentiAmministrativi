Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XmlReader

Public Class Ufficio
    'Lu Aggiunt desc breve
    Private pDescrDipartimentoBreve As String
    Private pDescrEnteBreve As String
    Private pDescrUfficioBreve As String
    Private pDescrArchivioBreve As String


    Public CodUfficio As String
    Private pCodDipartimento As String
    Private pCodEnte As String
    Private pDescrDipartimento As String
    Private pDescrEnte As String
    Private pDescrUfficio As String
    Private pDescrArchivio As String
    Private pCodDipartimentoPubblico As String
    Private pCodEntePubblico As String
    Private pCodUfficioPubblico As String
    Private pAbilitatoCreazioneAtti As Int16
    Private pAbilitatoCreazioneAttiSiOpCont As Int16

    Private pbUfficioProponente As Integer
    Private pbUfficioDirigenzaDipartimento As Integer
    Private pbUfficioPolitico As Integer
    Private pbUfficioSegreteria As Integer
    Private pbUfficioSegreteriaPresidenzaLegittimita As Integer
    Private pbUfficioSegreteriaPresidenzaSegretario As Integer
    Private pbUfficioSegreteriaPresidenzaApprovazione As Integer
    Private pbUfficioPresidenza As Integer
    Private pbUfficioRagioneria As Integer
    Private pbUfficioControlloAmministrativo As Integer
    Private pCodUfficioDirigenzaDipartimento As String
    Private pCodUfficioRagioneria As String
    Private pCodUfficioControlloAmministrativo As String
    Private pCodUfficioPolitico As String
    Private pCodArchivio As String
    Private pCodUfficioSegreteria As String
    Private pCodUfficioSegreteriaPresidenzaLegittimita As String
    Private pCodUfficioSegreteriaPresidenzaSegretario As String
    Private pCodUfficioSegreteriaPresidenzaApprovazione As String
    Private pCodUfficioPresidenza As String
    Private pAssegnazioneArrivoDetermine As String

    Private pAssegnazioneArrivoDelibereSegrAss As String

    Private pSupervisoriUfficio As Collections.Hashtable
    Private pResponsabileUfficio As String
    Private pCollaboratoriUfficio As Collections.Hashtable
    Private p4LivelloUfficio As Collections.Hashtable
    Private pUtentiUfficio As Collections.Hashtable
    Private pAttributi As Collections.Hashtable
    Private pUfficiConsultabili As ArrayList
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Ufficio))
    Public Sub New()
        CodUfficio = ""
        pCodDipartimento = ""
        pCodDipartimentoPubblico = ""
        pbUfficioProponente = -1
        pbUfficioDirigenzaDipartimento = -1
        pbUfficioRagioneria = -1
        pbUfficioControlloAmministrativo = -1
        pbUfficioPolitico = -1
        pbUfficioSegreteria = -1
        pbUfficioSegreteriaPresidenzaLegittimita = -1
        pbUfficioSegreteriaPresidenzaSegretario = -1
        pbUfficioSegreteriaPresidenzaApprovazione = -1
        pbUfficioPresidenza = -1
        pCodUfficioDirigenzaDipartimento = ""
        pCodUfficioRagioneria = ""
        pCodUfficioRagioneria = ""
        pCodUfficioControlloAmministrativo = ""
        pCodUfficioPolitico = ""
        pCodUfficioSegreteria = ""
        pCodUfficioSegreteriaPresidenzaLegittimita = ""
        pCodUfficioSegreteriaPresidenzaSegretario = ""
        pCodUfficioSegreteriaPresidenzaApprovazione = ""
        pCodUfficioPresidenza = ""
        pAssegnazioneArrivoDetermine = ""
        pAssegnazioneArrivoDelibereSegrAss = ""
        pResponsabileUfficio = String.Empty
        pSupervisoriUfficio = Nothing
        pCollaboratoriUfficio = Nothing
        p4LivelloUfficio = Nothing
        pUtentiUfficio = Nothing
        pUfficiConsultabili = Nothing
        pCodArchivio = ""
        pDescrArchivio = ""
        pDescrDipartimento = ""
        pDescrEnte = ""
        pDescrUfficio = ""
        pCodEntePubblico = ""
        CodUfficioPubblico = ""

        pDescrDipartimentoBreve = ""
        pDescrEnteBreve = ""
        pDescrUfficioBreve = ""
        pDescrArchivioBreve = ""
    End Sub
    Public Property Attributo(ByVal codAttributo As String) As String
        Get
            If pAttributi Is Nothing Then
                Call leggiAttributi()
            End If
            If pAttributi.Contains(codAttributo) Then
                Return pAttributi.Item(codAttributo) & ""
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            If pAttributi.Contains(codAttributo) Then
                pAttributi.Item(codAttributo) = Value
            End If
        End Set
    End Property
    Public ReadOnly Property bUfficioProponente() As Boolean
        Get
            If pbUfficioProponente = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioProponente = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioDirigenzaDipartimento() As Boolean
        Get
            If pbUfficioDirigenzaDipartimento = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioDirigenzaDipartimento = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioRagioneria() As Boolean
        Get
            If pbUfficioRagioneria = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioRagioneria = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioPolitico() As Boolean
        Get
            If pbUfficioPolitico = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioPolitico = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioControlloAmministrativo() As Boolean
        Get
            If pbUfficioControlloAmministrativo = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioControlloAmministrativo = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioSegreteria() As Boolean
        Get
            If pbUfficioSegreteria = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioSegreteria = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioSegreteriaPresidenzaLegittimita() As Boolean
        Get
            If pbUfficioSegreteriaPresidenzaLegittimita = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioSegreteriaPresidenzaLegittimita = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioSegreteriaPresidenzaSegretario() As Boolean
        Get
            If pbUfficioSegreteriaPresidenzaSegretario = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioSegreteriaPresidenzaSegretario = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioSegreteriaPresidenzaApprovazione() As Boolean
        Get
            If pbUfficioSegreteriaPresidenzaApprovazione = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioSegreteriaPresidenzaApprovazione = 1)
        End Get
    End Property
    Public ReadOnly Property bUfficioPresidenza() As Boolean
        Get
            If pbUfficioPresidenza = -1 Then
                Call leggiAttributi()
            End If
            Return (pbUfficioPresidenza = 1)
        End Get
    End Property
    Public ReadOnly Property CodUfficioControlloAmministrativo() As String
        Get
            If pbUfficioControlloAmministrativo = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioControlloAmministrativo = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioControlloAmministrativo = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioControlloAmministrativo
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioPolitico() As String
        Get
            If pbUfficioPolitico = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioPolitico = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioPolitico = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioPolitico
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioSegreteria() As String
        Get
            If pbUfficioSegreteria = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioSegreteria = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioSegreteria = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioSegreteria
            End If
        End Get
    End Property


    Public ReadOnly Property CodUfficioSegreteriaAssessorato() As String

         Get
            If pAssegnazioneArrivoDelibereSegrAss Is Nothing Or pAssegnazioneArrivoDelibereSegrAss = "" Then
                leggiAttributi()
            End If
           Return pAssegnazioneArrivoDelibereSegrAss
     
        End Get
    End Property

    Public ReadOnly Property CodUfficioSegreteriaPresidenzaLegittimita() As String
        Get
            If pbUfficioSegreteriaPresidenzaLegittimita = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioSegreteriaPresidenzaLegittimita = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioSegreteriaPresidenzaLegittimita = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioSegreteriaPresidenzaLegittimita
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioSegreteriaPresidenzaSegretario() As String
        Get
            If pbUfficioSegreteriaPresidenzaSegretario = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioSegreteriaPresidenzaSegretario = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioSegreteriaPresidenzaSegretario = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioSegreteriaPresidenzaSegretario
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioSegreteriaPresidenzaApprovazione() As String
        Get
            If pbUfficioSegreteriaPresidenzaApprovazione = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioSegreteriaPresidenzaApprovazione = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioSegreteriaPresidenzaApprovazione = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioSegreteriaPresidenzaApprovazione
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioPresidenza() As String
        Get
            If pbUfficioPresidenza = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioPresidenza = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioPresidenza = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioPresidenza
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioDirigenzaDipartimento() As String
        Get
            If pbUfficioDirigenzaDipartimento = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioDirigenzaDipartimento = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioDirigenzaDipartimento = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioDirigenzaDipartimento
            End If
        End Get
    End Property
    Public ReadOnly Property CodUfficioRagioneria() As String
        Get
            If pbUfficioRagioneria = -1 Then
                Call leggiAttributi()
            End If
            If pbUfficioRagioneria = 1 Then
                Return CodUfficio
            Else
                If pCodUfficioRagioneria = "" Then
                    leggiAttributi()
                End If
                Return pCodUfficioRagioneria
            End If
        End Get
    End Property
    Public ReadOnly Property AssegnazioneArrivoDetermine() As String
        Get
            If pAttributi Is Nothing Then
                Call leggiAttributi()
            End If
            If pAttributi.Contains("ASSEGNAZIONE_ARRIVO_DETERMINE") Then
                Return pAttributi.Item("ASSEGNAZIONE_ARRIVO_DETERMINE") & ""
            Else
                If pAssegnazioneArrivoDetermine = "" Then
                    leggiAttributi()
                End If
                Return pAssegnazioneArrivoDetermine
            End If
        End Get
    End Property
    '    Private Sub Leggi_CollaboratoriUfficio(ByVal flussoDocumentale As String)
    '        Const SFunzione As String = "Leggi_CollaboratoriUfficio"
    '        Dim Sqlq As String
    '        Dim DB As Object
    '        Dim RS As Object
    '        Dim vP(2) As Object
    '        Dim vR As Object = Nothing
    '        Dim i As Integer

    '        On Error GoTo Herr

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
    '        End If

    '            VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
    '            'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

    '            Call DB.ApriDB()
    '            If DB.errore <> 0 Then
    '                GoTo FineSub
    '            End If

    '            Sqlq = "SELECT     Strutture_Operatori.Sop_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
    '                   " FROM      Tab_Operatori_Gruppi INNER JOIN " & _
    '                   "           Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
    '                   "           TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore  INNER JOIN " & _
    '                   "       Tab_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Tab_Operatori.Op_Codice_Operatore" & _
    '                   " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
    '                   "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
    '                   "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'COLLABORATORE' ) " & _
    '                   "          AND (Tab_Operatori.Op_Stato = 1)   "

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '            For i = 0 To vR(2) - 1
    '                pCollaboratoriUfficio.Add(vR(1)(0, i), vR(1)(1, i))
    '            Next
    '            End If

    'FineSub:
    '        On Error Resume Next
    '        If Not DB Is Nothing Then
    '            Call DB.ChiudiDB()
    '            DB = Nothing
    '        End If

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Fine", SFunzione)
    '        End If
    '        On Error GoTo 0
    '        Exit Sub

    'Herr:

    '        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
    '        GoTo FineSub
    '        ' Resume
    '    End Sub

    Private Sub Leggi_4LivelloUfficio(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_4LivelloUfficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader = Nothing

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Sqlq = "SELECT     Strutture_Operatori.Sop_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
             " FROM      Tab_Operatori_Gruppi INNER JOIN " & _
             "           Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
             "           TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore  INNER JOIN " & _
             "       Tab_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Tab_Operatori.Op_Codice_Operatore" & _
             " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
             "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
             "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = '4LIVELLO' ) " & _
             "          AND (Tab_Operatori.Op_Stato = 1) " & _
             " order by Tab_Operatori.Op_Cognome "


           
           

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    If Not p4LivelloUfficio.ContainsKey(rdr.GetString(0)) Then
                        p4LivelloUfficio.Add(rdr.GetString(0), rdr.GetString(1))
                    End If

                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try

    End Sub
    Private Sub Leggi_CollaboratoriUfficio(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_CollaboratoriUfficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Sqlq = "SELECT     Strutture_Operatori.Sop_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
             " FROM      Tab_Operatori_Gruppi INNER JOIN " & _
             "           Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
             "           TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore  INNER JOIN " & _
             "       Tab_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Tab_Operatori.Op_Codice_Operatore" & _
             " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
             "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
             "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'COLLABORATORE' ) " & _
             "          AND (Tab_Operatori.Op_Stato = 1)  " & _
             " order by Tab_Operatori.Op_Cognome "

          
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    If Not pCollaboratoriUfficio.ContainsKey(rdr.GetString(0)) Then
                        pCollaboratoriUfficio.Add(rdr.GetString(0), rdr.GetString(1))
                    End If
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try

    End Sub
    Private Sub Leggi_UtentiUfficio(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_UtentiUfficio"
        Dim Sqlq As String
        Dim strFlussodoc As String = ""
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("***INIZIO " & SFunzione & " - Ufficio: " & Me.CodUfficio & " " & Now)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Sqlq = "SELECT     Strutture_Operatori.Sop_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
                " FROM      Tab_Operatori INNER JOIN " & _
                "           Strutture_Operatori ON Tab_Operatori.Op_Codice_Operatore = Strutture_Operatori.Sop_Operatore  " & _
                " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND (Tab_Operatori.Op_Stato = 1) " & _
                " order by  Tab_Operatori.Op_Cognome "

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            If pUtentiUfficio Is Nothing Then
                pUtentiUfficio = New Hashtable
            End If
            While (rdr.Read)
                If Not rdr.IsDBNull(0) And Not rdr.IsDBNull(1) Then
                    If Not pUtentiUfficio.ContainsKey(rdr.GetString(0)) Then
                        pUtentiUfficio.Add(rdr.GetString(0), rdr.GetString(1))
                    End If
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("***FINE " & SFunzione & " - Ufficio: " & Me.CodUfficio & " " & Now)
            
        End Try
    End Sub
    '    Private Sub Leggi_UfficiConsultabili(Optional ByVal flussoDocumentale As String = "", Optional ByVal ufficio As String = "", Optional ByVal isHt As Boolean = False)
    '        Const SFunzione As String = "Leggi_UfficiConsultabili"
    '        Dim Sqlq As String
    '        Dim DB As Object
    '        Dim RS As Object
    '        Dim vP(2) As Object
    '        Dim vR As Object = Nothing
    '        Dim i As Integer

    '        On Error GoTo Herr

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
    '        End If

    '        'Dim nodoconfronto As XmlNode

    '        If flussoDocumentale = "" Then
    '            ' nodoconfronto = domXmlDati.SelectSingleNode("//ufficiconsultabili")
    '        Else
    '            Dim strFlussodoc As String = ""
    '            Select Case CInt(flussoDocumentale)
    '                Case 0
    '                    strFlussodoc = "DETERMINE"
    '                Case 1
    '                    strFlussodoc = "DELIBERE"
    '                Case 2
    '                    strFlussodoc = "DISPOSIZIONI"
    '            End Select

    '            'nodoconfronto = domXmlDati.SelectSingleNode("//ufficiconsultabili/" & strFlussodoc)
    '        End If

    '        'If Not nodoconfronto Is Nothing Then
    '        '    Dim figlioUfficiConsultabili As XmlNode = nodoconfronto
    '        '    Dim nodo As XmlNode
    '        '    'il primo e l'ultimo nodo sono di tipo Text
    '        '    ReDim vR(2, figlioUfficiConsultabili.ChildNodes.Count - 2)
    '        '    For Each nodo In figlioUfficiConsultabili.ChildNodes
    '        '        If nodo.NodeType = XmlNodeType.Element Then
    '        '            If Not pUfficiConsultabili.Contains(nodo.FirstChild.InnerText) Then
    '        '                pUfficiConsultabili.Add(nodo.FirstChild.InnerText)
    '        '            End If
    '        '        End If
    '        '    Next

    '        'Else
    '        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
    '        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

    '        Call DB.ApriDB()
    '        If DB.errore <> 0 Then
    '            GoTo FineSub
    '        End If

    '        Sqlq = " SELECT Sta_valore, Struttura.Str_descrizione " & _
    '               " FROM Struttura_Attributi " & _
    '               " LEFT OUTER JOIN Struttura ON Struttura_Attributi.Sta_valore = Struttura.Str_id " & _
    '               " WHERE (Sta_attributo = 'CONSULTAZIONE') "

    '        If flussoDocumentale <> "" Then
    '            Select Case CInt(flussoDocumentale)
    '                Case 0
    '                    flussoDocumentale = "DETERMINE"
    '                Case 1
    '                    flussoDocumentale = "DELIBERE"
    '                Case 2
    '                    flussoDocumentale = "DISPOSIZIONI"
    '            End Select

    '            Sqlq = Sqlq & " AND (Sta_procedura = '" & flussoDocumentale & "')"
    '        Else
    '            Sqlq = Sqlq & " AND (Sta_procedura = '*')"
    '        End If

    '        If ufficio <> "" Then
    '            Sqlq = Sqlq & " AND (Sta_id = '" & ufficio & "')"
    '        Else
    '            Sqlq = Sqlq & " AND (Sta_id = '*')"
    '        End If


    '        vP(0) = DB
    '        vP(1) = Sqlq
    '        vP(2) = 2
    '        vR = GDB.DBQuery(vP)

    '        If vR(0) = 0 Then
    '            pUfficiConsultabili = New ArrayList
    '            For i = 0 To vR(2) - 1
    '                pUfficiConsultabili.Add(vR(1)(0, i))
    '            Next
    '        End If

    'FineSub:
    '        On Error Resume Next
    '        If Not DB Is Nothing Then
    '            Call DB.ChiudiDB()
    '            DB = Nothing
    '        End If

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Fine", SFunzione)
    '        End If
    '        On Error GoTo 0
    '        Exit Sub

    'Herr:

    '        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
    '        GoTo FineSub
    '        ' Resume
    '    End Sub
    Private Sub Leggi_UfficiConsultabili(Optional ByVal flussoDocumentale As String = "", Optional ByVal ufficio As String = "", Optional ByVal isHt As Boolean = False)
        Const SFunzione As String = "Leggi_UfficiConsultabili"
        Dim strFlussodoc As String = ""
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        If flussoDocumentale <> "" Then
            Select Case CInt(flussoDocumentale)
                Case 0
                    strFlussodoc = "DETERMINE"
                Case 1
                    strFlussodoc = "DELIBERE"
                Case 2
                    strFlussodoc = "DISPOSIZIONI"
                Case 3
                    strFlussodoc = "DECRETI"
                Case 4
                    strFlussodoc = "ORDINANZE"
            End Select
        End If

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Sqlq = " SELECT Sta_valore, Struttura.Str_descrizione " & _
                   " FROM Struttura_Attributi " & _
                   " LEFT OUTER JOIN Struttura ON Struttura_Attributi.Sta_valore = Struttura.Str_id " & _
                   " WHERE (Sta_attributo = 'CONSULTAZIONE') "

            If strFlussodoc <> "" Then
                Sqlq = Sqlq & " AND (Sta_procedura = '" & strFlussodoc & "')"
            Else
                Sqlq = Sqlq & " AND (Sta_procedura = '*')"
            End If

            If ufficio = "" Then
                Sqlq = Sqlq & " AND (Sta_id = '*')"
            Else

                Dim sqlGruppi As String = ""
                sqlGruppi = " Select TOG_Gruppo from Tab_Operatori_Gruppi  where Tog_Operatore='" & ufficio & "' "


                Dim sqlPerGruppi As String = Sqlq & " AND (Sta_id IN ( " & sqlGruppi & " ))"

                Sqlq = Sqlq & " AND (Sta_id = '" & ufficio & "')"
                Sqlq = Sqlq & " Union " & sqlPerGruppi

            End If



            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            If pUfficiConsultabili Is Nothing Then
                pUfficiConsultabili = New ArrayList
            End If
            While (rdr.Read)
                If Not rdr.IsDBNull(0) Then pUfficiConsultabili.Add(rdr.GetString(0))
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
    End Sub
    Private Sub Leggi_SupervisoriUfficio(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_SupervisoreUfficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            'Sqlq = "SELECT     Tab_Operatori_Gruppi.TOG_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
            '       " FROM         Tab_Operatori_Gruppi INNER JOIN " & _
            '       "      Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
            '       "       TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore INNER JOIN " & _
            '       "       Tab_Operatori ON Strutture_Operatori.Sop_Operatore = Tab_Operatori.Op_Codice_Operatore " & _
            '       " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
            '       "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
            '       "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'SUPERVISORE') " & _
            '       " order by Tab_Operatori.Op_Cognome "
            'Aggiunta verifica su abilitazione 
            Sqlq = "SELECT     Tab_Operatori_Gruppi.TOG_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
                   " FROM         Tab_Operatori_Gruppi INNER JOIN " & _
                   "      Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
                   "       TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore INNER JOIN " & _
                   "       Tab_Operatori ON Strutture_Operatori.Sop_Operatore = Tab_Operatori.Op_Codice_Operatore " & _
                   " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
                   "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
                   "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'SUPERVISORE') " & _
                      "          AND (Tab_Operatori.Op_Stato = 1)  " & _
                   " order by Tab_Operatori.Op_Cognome "

               


            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not rdr.IsDBNull(1) Then
                    If Not pSupervisoriUfficio.ContainsKey(rdr.GetString(0)) Then
                        pSupervisoriUfficio.Add(rdr.GetString(0), rdr.GetString(1))
                    End If
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
    End Sub
    '    Private Sub Leggi_SupervisoriUfficio(ByVal flussoDocumentale As String)
    '        Const SFunzione As String = "Leggi_SupervisoreUfficio"
    '        Dim Sqlq As String
    '        Dim DB As Object = Nothing
    '        Dim RS As Object = Nothing
    '        Dim vP(2) As Object
    '        Dim vR As Object = Nothing
    '        Dim i As Integer

    '        On Error GoTo Herr

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
    '        End If


    '        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
    '        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

    '        Call DB.ApriDB()
    '        If DB.errore <> 0 Then
    '            GoTo FineSub
    '        End If

    '        Sqlq = "SELECT     Tab_Operatori_Gruppi.TOG_Operatore, Tab_Operatori.Op_Cognome + ' ' + Tab_Operatori.Op_Nome as nominativo " & _
    '               " FROM         Tab_Operatori_Gruppi INNER JOIN " & _
    '               "      Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
    '               "       TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore INNER JOIN " & _
    '               "       Tab_Operatori ON Strutture_Operatori.Sop_Operatore = Tab_Operatori.Op_Codice_Operatore " & _
    '               " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
    '               "          (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
    '               "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'SUPERVISORE') "

    '        vP(0) = DB
    '        vP(1) = Sqlq
    '        vP(2) = 2
    '        vR = GDB.DBQuery(vP)

    '        If vR(0) = 0 Then
    '            For i = 0 To vR(2) - 1
    '                pSupervisoriUfficio.Add(vR(1)(0, i), vR(1)(1, i))
    '            Next
    '        End If

    'FineSub:
    '        On Error Resume Next
    '        If Not DB Is Nothing Then
    '            Call DB.ChiudiDB()
    '            DB = Nothing
    '        End If

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Fine", SFunzione)
    '        End If
    '        On Error GoTo 0
    '        Exit Sub

    'Herr:

    '        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
    '        GoTo FineSub
    '        ' Resume
    '    End Sub
    Private Sub Leggi_ResponsabileUfficio(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_ResponsabileUfficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("***INIZIO " & SFunzione & " Ufficio: " & Me.CodUfficio & " " & Now)
            
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            'Sqlq = "SELECT     Tab_Operatori_Gruppi.TOG_Operatore " & _
            '   " FROM      Tab_Operatori_Gruppi INNER JOIN " & _
            '   "           Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
            '   "           TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore " & _
            '   " WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
            '   "        (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
            '   "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'RESPONSABILE') "
            'Aggiungi verifica Abilitato

            Sqlq = "SELECT     Tab_Operatori_Gruppi.TOG_Operatore " & _
               " FROM      Tab_Operatori_Gruppi INNER JOIN " & _
               "           Strutture_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Strutture_Operatori.Sop_Operatore INNER JOIN " & _
               "           TAB_Operatori_Attributi ON Tab_Operatori_Gruppi.TOG_Gruppo = TAB_Operatori_Attributi.TOA_Operatore INNER JOIN " & _
            "       Tab_Operatori ON Tab_Operatori_Gruppi.TOG_Operatore = Tab_Operatori.Op_Codice_Operatore" & _
" WHERE     (Strutture_Operatori.Sop_Struttura = '" & Me.CodUfficio & "') AND " & _
               "        (TAB_Operatori_Attributi.TOA_Procedura = '" & flussoDocumentale & "' or TAB_Operatori_Attributi.TOA_Procedura = '*') AND " & _
               "          (TAB_Operatori_Attributi.TOA_Attributo = 'LIVELLO_UFFICIO') AND (TAB_Operatori_Attributi.Toa_Valore = 'RESPONSABILE') " & _
                 "          AND (Tab_Operatori.Op_Stato = 1)  "



            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then  pResponsabileUfficio = rdr.GetString(0)
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("***FINE " & SFunzione & " Ufficio: " & Me.CodUfficio & " " & Now)
            
        End Try
    End Sub
    Private Function addAttributi(ByVal codAttributo As String, ByVal valAttributo As String) As Boolean
        If pAttributi Is Nothing Then
            pAttributi = New Hashtable
        End If
        If Not pAttributi.Contains(codAttributo) Then
            pAttributi.Add(codAttributo, valAttributo)
        End If
    End Function
    '    Private Sub leggiAttributi()
    '        Const SFunzione As String = "leggiAttributi"
    '        Dim Sqlq As String
    '        Dim DB As Object
    '        Dim RS As Object
    '        Dim vP(2) As Object
    '        Dim vR As Object = Nothing
    '        Dim i As Integer
    '        Dim codAttributo As String
    '        Dim valAttributo As String

    '        On Error GoTo Herr

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
    '        End If

    '        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
    '        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

    '        Call DB.ApriDB()
    '        If DB.errore <> 0 Then
    '            GoTo FineSub
    '        End If

    '        Sqlq = "SELECT     Sta_attributo, Sta_valore " & _
    '               "         FROM Struttura_Attributi " & _
    '               " WHERE     (Sta_id = '" & CodUfficio & "' or Sta_id = '*' )" & _
    '               " AND (Sta_procedura = 'ATTIDIGITALI' OR " & _
    '               "  Sta_procedura = '*')"

    '        vP(0) = DB
    '        vP(1) = Sqlq
    '        vP(2) = 2
    '        vR = GDB.DBQuery(vP)

    '        If vR(0) <> 0 Then
    '            GoTo FineSub
    '        End If

    '        pbUfficioProponente = 0
    '        pbUfficioDirigenzaDipartimento = 0
    '        pbUfficioRagioneria = 0

    '        For i = 0 To vR(2) - 1

    '            addAttributi(Trim(vR(1)(0, i) & ""), Trim(vR(1)(1, i) & ""))

    '            codAttributo = UCase(Trim(vR(1)(0, i)))
    '            valAttributo = UCase(Trim(vR(1)(1, i)))

    '            Select Case codAttributo
    '                Case "UFFICIO_PROPONENTE"
    '                    pbUfficioProponente = IIf(valAttributo = "1", 1, 0)
    '                Case "UFFICIO_DIRIGENZA_DIPARTIMENTO"
    '                    pbUfficioDirigenzaDipartimento = IIf(valAttributo = "1", 1, 0)
    '                Case "UFFICIO_RAGIONERIA"
    '                    pbUfficioRagioneria = IIf(valAttributo = "1", 1, 0)
    '                Case "UFFICIO_CONTROLLO_AMMINISTRATIVO"
    '                    pbUfficioControlloAmministrativo = IIf(valAttributo = "1", 1, 0)
    '                Case "UFFICIO_POLITICO"
    '                    pbUfficioPolitico = IIf(valAttributo = "1", 1, 0)
    '                Case "UFFICIO_SEGRETERIA"
    '                    pbUfficioSegreteria = IIf(valAttributo = "1", 1, 0)
    '                Case "ASSEGNAZIONE_ARRIVO_DETERMINE"
    '                    pAssegnazioneArrivoDetermine = valAttributo
    '            End Select
    '        Next

    '        'fare un'unica query per tutte le operazioni che seguono ...

    '        'se non sono un ufficio Dirigenza Dipartimento leggo il mio
    '        If pbUfficioDirigenzaDipartimento <= 0 Then
    '            Sqlq = "SELECT     dirigenzaDip.Str_id " & _
    '                   " FROM         Struttura dirigenzaDip INNER JOIN " & _
    '                   "              Struttura ufficio ON dirigenzaDip.Str_padre = ufficio.Str_padre INNER JOIN " & _
    '                   "              Struttura_Attributi ON dirigenzaDip.Str_id = Struttura_Attributi.Sta_id " & _
    '                   " WHERE     (Struttura_Attributi.Sta_attributo = 'UFFICIO_DIRIGENZA_DIPARTIMENTO') " & _
    '                   "            AND (ufficio.Str_id = '" & CodUfficio & "')"

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '                pCodUfficioDirigenzaDipartimento = Trim(vR(1)(0, 0))
    '            End If
    '        End If

    '        'se non sono la ragioneria leggo la ragionaria
    '        If pbUfficioRagioneria <= 0 Then
    '            Sqlq = "SELECT     Sta_id " & _
    '                   "   FROM Struttura_Attributi " & _
    '                   "   WHERE     (Sta_attributo = 'UFFICIO_RAGIONERIA') "

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '                pCodUfficioRagioneria = Trim(vR(1)(0, 0))
    '            End If
    '        End If

    '        'se non sono l'ufficio controllo amministrativo leggo il codice
    '        If pbUfficioControlloAmministrativo <= 0 Then
    '            Sqlq = "SELECT     Sta_id " & _
    '                   "   FROM Struttura_Attributi " & _
    '                   "   WHERE     (Sta_attributo = 'UFFICIO_CONTROLLO_AMMINISTRATIVO') "

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '                pCodUfficioControlloAmministrativo = Trim(vR(1)(0, 0))
    '            End If
    '        End If

    '        'se non sono l'ufficio controllo amministrativo leggo il codice
    '        If pbUfficioPolitico <= 0 Then
    '            Sqlq = "SELECT     dirigenzaDip.Str_id " & _
    '                   " FROM         Struttura dirigenzaDip INNER JOIN " & _
    '                   "              Struttura ufficio ON dirigenzaDip.Str_padre = ufficio.Str_padre INNER JOIN " & _
    '                   "              Struttura_Attributi ON dirigenzaDip.Str_id = Struttura_Attributi.Sta_id " & _
    '                   " WHERE     (Struttura_Attributi.Sta_attributo = 'UFFICIO_POLITICO') " & _
    '                   "            AND (ufficio.Str_id = '" & CodUfficio & "')"

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '                pCodUfficioPolitico = Trim(vR(1)(0, 0))
    '            End If
    '        End If

    '        'se non sono l'ufficio controllo amministrativo leggo il codice
    '        If pbUfficioSegreteria <= 0 Then
    '            Sqlq = "SELECT     Sta_id " & _
    '                   "   FROM Struttura_Attributi " & _
    '                   "   WHERE     (Sta_attributo = 'UFFICIO_SEGRETERIA') "

    '            vP(0) = DB
    '            vP(1) = Sqlq
    '            vP(2) = 2
    '            vR = GDB.DBQuery(vP)

    '            If vR(0) = 0 Then
    '                pCodUfficioSegreteria = Trim(vR(1)(0, 0))
    '            End If
    '        End If
    '        'End If

    'FineSub:
    '        On Error Resume Next
    '        If Not DB Is Nothing Then
    '            Call DB.ChiudiDB()
    '            DB = Nothing
    '        End If

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Fine", SFunzione)
    '        End If
    '        On Error GoTo 0
    '        Exit Sub

    'Herr:

    '        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
    '        GoTo FineSub
    '        ' Resume
    '    End Sub

    Public Function livelloUfficioAsSigla(ByVal ufficio As Ufficio) As String

        If ufficio.Test_Attributo("ufficio_dirigenza_dipartimento", 1) Then
            Return "UDD"
        ElseIf ufficio.Test_Attributo("ufficio_ragioneria", 1) Then
            Return "UR"
        ElseIf ufficio.Test_Attributo("ufficio_controllo_amministrativo", 1) Then
            Return "UCA"
        ElseIf ufficio.Test_Attributo("ufficio_segreteria_legittimita", 1) Then
            Return "USL"
        ElseIf ufficio.Test_Attributo("ufficio_segreteria_segretario", 1) Then
            Return "USS"
        ElseIf ufficio.Test_Attributo("ufficio_segreteria_approvazione", 1) Then
            Return "USA"
        ElseIf ufficio.Test_Attributo("ufficio_segreteria", 1) Then
            Return "US"
        ElseIf ufficio.Test_Attributo("ufficio_presidenza", 1) Then
            Return "UPRES"
        ElseIf ufficio.Test_Attributo("ufficio_proponente", 1) Then
            Return "UP"
        Else
            Return ""
        End If

    End Function



    Private Sub leggiAttributi()
        Const SFunzione As String = "leggiAttributi"
        Dim Sqlq As String
        Dim codAttributo As String
        Dim valAttributo As String

        Try

            Log.Debug("***INIZIO " & SFunzione & " Ufficio: " & CodUfficio & " " & Now)
            
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Sqlq = "SELECT     Sta_attributo, Sta_valore " & _
              "         FROM Struttura_Attributi " & _
              " WHERE     (Sta_id = '" & CodUfficio & "' or Sta_id = '*' or Sta_id ='" & CodDipartimento & "')" & _
              " AND (Sta_procedura = 'ATTIDIGITALI' OR " & _
              "  Sta_procedura = '*')"
            Dim rdr As SqlClient.SqlDataReader

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            'inizializzo
            pbUfficioProponente = 0
            pbUfficioDirigenzaDipartimento = 0
            pbUfficioRagioneria = 0

            Try
                While (rdr.Read())
                    If Not (rdr.IsDBNull(0)) And Not (rdr.IsDBNull(1)) Then
                        addAttributi(Trim(rdr.GetString(0) & ""), Trim(rdr.GetString(1) & ""))

                        codAttributo = UCase(Trim(rdr.GetString(0)))
                        valAttributo = UCase(Trim(rdr.GetString(1)))

                        Select Case codAttributo
                            Case "UFFICIO_PROPONENTE"
                                pbUfficioProponente = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_DIRIGENZA_DIPARTIMENTO"
                                pbUfficioDirigenzaDipartimento = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_RAGIONERIA"
                                pbUfficioRagioneria = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_CONTROLLO_AMMINISTRATIVO"
                                pbUfficioControlloAmministrativo = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_POLITICO"
                                pbUfficioPolitico = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_SEGRETERIA"
                                pbUfficioSegreteria = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_SEGR_PRESIDENZA_LEGITTIMITA"
                                pbUfficioSegreteriaPresidenzaLegittimita = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_SEGR_PRESIDENZA_SEGRETARIO"
                                pbUfficioSegreteriaPresidenzaSegretario = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_SEGR_PRESIDENZA_APPROVAZIONE"
                                pbUfficioSegreteriaPresidenzaApprovazione = IIf(valAttributo = "1", 1, 0)
                            Case "UFFICIO_PRESIDENZA"
                                pbUfficioPresidenza = IIf(valAttributo = "1", 1, 0)
                            Case "ASSEGNAZIONE_ARRIVO_DETERMINE"
                                pAssegnazioneArrivoDetermine = valAttributo

                            Case "INOLTRO_SEGRETERIA_ASS"
                                pAssegnazioneArrivoDelibereSegrAss = valAttributo
                                
                        End Select
                    End If
                End While
                rdr.Close()
            Catch ex As Exception
                Log.Error(ex.Message)
                If Not rdr.IsClosed Then
                    rdr.Close()
                End If
                rdr = Nothing
            End Try

            'se non sono un ufficio Dirigenza Dipartimento leggo il mio
            If pbUfficioDirigenzaDipartimento <= 0 Then
                Sqlq = "SELECT     dirigenzaDip.Str_id " & _
                       " FROM         Struttura dirigenzaDip INNER JOIN " & _
                       "              Struttura ufficio ON dirigenzaDip.Str_padre = ufficio.Str_padre INNER JOIN " & _
                       "              Struttura_Attributi ON dirigenzaDip.Str_id = Struttura_Attributi.Sta_id " & _
                       " WHERE     (Struttura_Attributi.Sta_attributo = 'UFFICIO_DIRIGENZA_DIPARTIMENTO') " & _
                       "            AND (ufficio.Str_id = '" & CodUfficio & "')"

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioDirigenzaDipartimento = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono la ragioneria leggo la ragionaria
            If pbUfficioRagioneria <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_RAGIONERIA') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioRagioneria = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioControlloAmministrativo <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_CONTROLLO_AMMINISTRATIVO') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioControlloAmministrativo = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioPolitico <= 0 Then
                Sqlq = "SELECT     dirigenzaDip.Str_id " & _
                       " FROM         Struttura dirigenzaDip INNER JOIN " & _
                       "              Struttura ufficio ON dirigenzaDip.Str_padre = ufficio.Str_padre INNER JOIN " & _
                       "              Struttura_Attributi ON dirigenzaDip.Str_id = Struttura_Attributi.Sta_id " & _
                       " WHERE     (Struttura_Attributi.Sta_attributo = 'UFFICIO_POLITICO') " & _
                       "            AND (ufficio.Str_id = '" & CodUfficio & "')"

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioPolitico = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioSegreteria <= 0 Then
                'Sqlq = "SELECT     Sta_id " & _
                '       "   FROM Struttura_Attributi " & _
                '       "   WHERE     (Sta_attributo = 'UFFICIO_SEGRETERIA') "
                'MODIFICA GIO 231017 - MODIFICA PER SETTARE L'UFFICIO SEGRETERIA ASSESSORATO PER LE DELIBERE 
                 Sqlq = "SELECT     dirigenzaDip.Str_id " & _
                       " FROM         Struttura dirigenzaDip INNER JOIN " & _
                       "              Struttura ufficio ON dirigenzaDip.Str_padre = ufficio.Str_padre INNER JOIN " & _
                       "              Struttura_Attributi ON dirigenzaDip.Str_id = Struttura_Attributi.Sta_id " & _
                       " WHERE     (Struttura_Attributi.Sta_attributo = 'UFFICIO_SEGRETERIA') " & _
                       "            AND (ufficio.Str_id = '" & CodUfficio & "')"
                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioSegreteria = Trim(rdr.GetString(0))
                    End While
                    
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioSegreteriaPresidenzaLegittimita <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_SEGR_PRESIDENZA_LEGITTIMITA') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioSegreteriaPresidenzaLegittimita = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioSegreteriaPresidenzaSegretario <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_SEGR_PRESIDENZA_SEGRETARIO') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioSegreteriaPresidenzaSegretario = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio segr di presidenza - approvazione leggo il codice
            If pbUfficioSegreteriaPresidenzaApprovazione <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_SEGR_PRESIDENZA_APPROVAZIONE') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioSegreteriaPresidenzaApprovazione = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

            'se non sono l'ufficio controllo amministrativo leggo il codice
            If pbUfficioPresidenza <= 0 Then
                Sqlq = "SELECT     Sta_id " & _
                       "   FROM Struttura_Attributi " & _
                       "   WHERE     (Sta_attributo = 'UFFICIO_PRESIDENZA') "

                rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
                Try
                    While (rdr.Read())
                        If Not rdr.IsDBNull(0) Then pCodUfficioPresidenza = Trim(rdr.GetString(0))
                    End While
                    rdr.Close()
                Catch ex As Exception
                    Log.Error(ex.Message)
                    If Not rdr.IsClosed Then
                        rdr.Close()
                    End If
                    rdr = Nothing
                End Try
            End If

        Catch ex As Exception
            Log.Error(ex.Message)
        Finally
            Log.Debug("***FINE " & SFunzione & " Ufficio: " & CodUfficio & " " & Now)
            
        End Try
    End Sub
    '    Private Sub leggiDati()
    '        Const SFunzione As String = "leggiDati"
    '        Dim Sqlq As String
    '        Dim DB As Object
    '        Dim RS As Object
    '        Dim vP(2) As Object
    '        Dim vR As Object = Nothing
    '        Dim i As Integer
    '        Dim codAttributo As String
    '        Dim valAttributo As String

    '        On Error GoTo Herr

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Inizio", SFunzione)
    '        End If




    '        VerificaSistema(SISTEMA, "DOCUMENTALE", DB)
    '        'DB = SISTEMA.PROCDB.Item("DOCUMENTALE")

    '        Call DB.ApriDB()
    '        If DB.errore <> 0 Then
    '            GoTo FineSub
    '        End If

    '        Sqlq = " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve " & _
    '               " FROM Struttura " & _
    '               " WHERE  ((Str_tipo = 'UFF') AND (str_id = '" & CodUfficio & "')) OR ((Str_padre =" & _
    '               " (SELECT     str_padre" & _
    '               " FROM Struttura " & _
    '               " WHERE      (str_id = '" & CodUfficio & "'))) and" & _
    '               " (Str_tipo = 'ARCH'))" & _
    '               " union " & _
    '               " SELECT     Str_descrizione, Str_id, Str_tipo , Str_codiceUtente,Str_descrBreve" & _
    '               " FROM         Struttura" & _
    '               " WHERE     (Str_tipo = 'ARCH') AND (Str_padre ='" & CodUfficio & "')" & _
    '               " union " & _
    '               " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve" & _
    '               " FROM         Struttura" & _
    '               " WHERE     ((Str_tipo = 'DIP') AND (str_id = '" & CodUfficio & "')) " & _
    '               " OR (Str_id = (SELECT     str_padre" & _
    '               " FROM          Struttura" & _
    '               " WHERE      (str_id = '" & CodUfficio & "') and (Str_tipo = 'UFF')))" & _
    '               " union " & _
    '               " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve" & _
    '               " FROM         Struttura " & _
    '               " WHERE     ((Str_tipo = 'ENTE') AND (str_id =  (SELECT  distinct   str_padre" & _
    '               " FROM          Struttura" & _
    '               " WHERE      (Str_tipo = 'DIP'))))"

    '        vP(0) = DB
    '        vP(1) = Sqlq
    '        vP(2) = 2
    '        vR = GDB.DBQuery(vP)

    '        If vR(0) <> 0 Then
    '            GoTo FineSub
    '        End If

    '        For i = 0 To vR(2) - 1
    '            Select Case CStr(vR(1)(2, i)).ToUpper
    '                Case "UFF"
    '                    pDescrUfficio = vR(1)(0, i)
    '                    pDescrUfficioBreve = "" & vR(1)(4, i)
    '                    CodUfficioPubblico = vR(1)(3, i)
    '                Case "ENTE"
    '                    pCodEnte = vR(1)(1, i)
    '                    pCodEntePubblico = vR(1)(3, i)
    '                    pDescrEnte = vR(1)(0, i)
    '                    pDescrEnteBreve = "" & vR(1)(4, i)
    '                Case "DIP"
    '                    pCodDipartimentoPubblico = vR(1)(3, i)
    '                    pCodDipartimento = vR(1)(1, i)
    '                    pDescrDipartimento = vR(1)(0, i)
    '                    pDescrDipartimentoBreve = "" & vR(1)(4, i)
    '                Case "ARCH"
    '                    pCodArchivio = vR(1)(1, i)
    '                    pDescrArchivio = vR(1)(0, i)
    '                    pDescrArchivioBreve = vR(1)(4, i)
    '            End Select
    '        Next

    'FineSub:
    '        On Error Resume Next
    '        If Not DB Is Nothing Then
    '            Call DB.ChiudiDB()
    '            DB = Nothing
    '        End If

    '        If SISTEMA.bTRACE Then
    '            Call SISTEMA.Registra_Trace("Fine", SFunzione)
    '        End If
    '        On Error GoTo 0
    '        Exit Sub

    'Herr:

    '        Call SISTEMA.Registra_Log(Err.Description & " ", SFunzione)
    '        GoTo FineSub
    '        ' Resume
    '    End Sub
    Private Sub leggiDati()
        Const SFunzione As String = "leggiDati"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("***INIZIO " & SFunzione & " Ufficio: " & CodUfficio & " " & Now)
            
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Sqlq = " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve, Str_creazioneAtti, Str_creazioneAttSiOpCon " & _
                   " FROM Struttura " & _
                   " WHERE  ((Str_tipo = 'UFF') AND (str_id = '" & CodUfficio & "')) OR ((Str_padre =" & _
                   " (SELECT     str_padre" & _
                   " FROM Struttura " & _
                   " WHERE      (str_id = '" & CodUfficio & "'))) and" & _
                   " (Str_tipo = 'ARCH'))" & _
                   " union " & _
                   " SELECT     Str_descrizione, Str_id, Str_tipo , Str_codiceUtente,Str_descrBreve, Str_creazioneAtti, Str_creazioneAttSiOpCon " & _
                   " FROM         Struttura" & _
                   " WHERE     (Str_tipo = 'ARCH') AND (Str_padre ='" & CodUfficio & "')" & _
                   " union " & _
                   " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve, Str_creazioneAtti, Str_creazioneAttSiOpCon " & _
                   " FROM         Struttura" & _
                   " WHERE     ((Str_tipo = 'DIP') AND (str_id = '" & CodUfficio & "')) " & _
                   " OR (Str_id = (SELECT     str_padre" & _
                   " FROM          Struttura" & _
                   " WHERE      (str_id = '" & CodUfficio & "') and (Str_tipo = 'UFF')))" & _
                   " union " & _
                   " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente,Str_descrBreve, Str_creazioneAtti, Str_creazioneAttSiOpCon " & _
                   " FROM         Struttura " & _
                   " WHERE     ((Str_tipo = 'ENTE') AND (str_id =  (SELECT  distinct   str_padre" & _
                   " FROM          Struttura" & _
                   " WHERE      (Str_tipo = 'DIP'))))" & _
                   " ORDER BY Str_tipo"
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            While rdr.Read()
                Select Case CStr(rdr.GetString(2).ToUpper)
                    Case "UFF"
                        If Not rdr.IsDBNull(0) Then pDescrUfficio = rdr.GetString(0)
                        If Not rdr.IsDBNull(4) Then pDescrUfficioBreve = rdr.GetString(4)
                        If Not rdr.IsDBNull(3) Then CodUfficioPubblico = rdr.GetString(3)
                        If Not rdr.IsDBNull(5) Then AbilitatoCreazioneAtti = rdr.GetByte(5)
                        If Not rdr.IsDBNull(6) Then AbilitatoCreazioneAttiSiOpCont = rdr.GetByte(6)

                    Case "ENTE"
                        If Not rdr.IsDBNull(1) Then pCodEnte = rdr.GetString(1)
                        If Not rdr.IsDBNull(3) Then pCodEntePubblico = rdr.GetString(3)
                        If Not rdr.IsDBNull(0) Then pDescrEnte = rdr.GetString(0)
                        If Not rdr.IsDBNull(4) Then pDescrEnteBreve = rdr.GetString(4)
                        If Not rdr.IsDBNull(5) Then pAbilitatoCreazioneAtti = rdr.GetByte(5)
                        If Not rdr.IsDBNull(6) Then pAbilitatoCreazioneAttiSiOpCont = rdr.GetByte(6)
                    Case "DIP"
                        If Not rdr.IsDBNull(3) Then pCodDipartimentoPubblico = rdr.GetString(3)
                        If Not rdr.IsDBNull(1) Then pCodDipartimento = rdr.GetString(1)
                        If Not rdr.IsDBNull(0) Then pDescrDipartimento = rdr.GetString(0)
                        If Not rdr.IsDBNull(4) Then pDescrDipartimentoBreve = rdr.GetString(4)
                        If Not rdr.IsDBNull(5) Then pAbilitatoCreazioneAtti = rdr.GetByte(5)
                        If Not rdr.IsDBNull(6) Then pAbilitatoCreazioneAttiSiOpCont = rdr.GetByte(6)
                    Case "ARCH"
                        If Not rdr.IsDBNull(1) Then pCodArchivio = rdr.GetString(1)
                        If Not rdr.IsDBNull(0) Then pDescrArchivio = rdr.GetString(0)
                        If Not rdr.IsDBNull(4) Then pDescrArchivioBreve = rdr.GetString(4)
                        If Not rdr.IsDBNull(5) Then pAbilitatoCreazioneAtti = rdr.GetByte(5)
                        If Not rdr.IsDBNull(6) Then pAbilitatoCreazioneAttiSiOpCont = rdr.GetByte(6)
                End Select
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("***FINE " & SFunzione & " Ufficio: " & CodUfficio & " " & Now)
            
        End Try
    End Sub

    Public Function Test_Attributo(ByVal codAttributo As String, ByVal valAttributo As String) As Boolean
        If pAttributi Is Nothing Then
            Call leggiAttributi()
        End If
        Return (UCase(pAttributi.Item(codAttributo)) = UCase(valAttributo))

    End Function
    Public ReadOnly Property CodDipartimento() As String
        Get
            If pCodDipartimento = "" Then
                leggiDati()
            End If
            Return pCodDipartimento & ""
        End Get
    End Property
    Public Property CodUfficioPubblico() As String
        Get
            If pCodUfficioPubblico = "" Then
                leggiDati()
            End If
            Return pCodUfficioPubblico & ""
        End Get
        Set(ByVal value As String)
            pCodUfficioPubblico = value
        End Set
    End Property
    Public Property AbilitatoCreazioneAtti() As Int16
        Get
            If pAbilitatoCreazioneAtti <> 0 And pAbilitatoCreazioneAtti <> 1 Then
                leggiDati()
            End If
            Return pAbilitatoCreazioneAtti
        End Get
        Set(ByVal value As Int16)
            pAbilitatoCreazioneAtti = value
        End Set
    End Property

     Public Property AbilitatoCreazioneAttiSiOpCont() As Int16
        Get
            If pAbilitatoCreazioneAttiSiOpCont <> 0 And pAbilitatoCreazioneAttiSiOpCont <> 1 Then
                leggiDati()
            End If
            Return pAbilitatoCreazioneAttiSiOpCont
        End Get
        Set(ByVal value As Int16)
            pAbilitatoCreazioneAttiSiOpCont = value
        End Set
    End Property

    Public ReadOnly Property CodDipartimentoPubblico() As String
        Get
            If pCodDipartimentoPubblico = "" Then
                leggiDati()
            End If
            Return pCodDipartimentoPubblico & ""
        End Get
    End Property
    Public ReadOnly Property CodEntePubblico() As String
        Get
            If pCodEntePubblico = "" Then
                leggiDati()
            End If
            Return pCodEntePubblico & ""
        End Get
    End Property
    Public ReadOnly Property CodArchivio() As String
        Get
            If pCodArchivio = "" Then
                leggiDati()
            End If
            Return pCodArchivio & ""
        End Get
    End Property
    Public ReadOnly Property CodEnte() As String
        Get
            If pCodEnte = "" Then
                leggiDati()
            End If
            Return pCodEnte & ""
        End Get
    End Property
    Public ReadOnly Property DescrArchivio() As String
        Get
            If pDescrArchivio = "" Then
                leggiDati()
            End If
            Return pDescrArchivio & ""
        End Get
    End Property
    Public ReadOnly Property DescrDipartimento() As String
        Get
            If pDescrDipartimento = "" Then
                leggiDati()
            End If
            Return pDescrDipartimento & ""
        End Get
    End Property
    Public ReadOnly Property DescrUfficio() As String
        Get
            If pDescrUfficio = "" Then
                leggiDati()
            End If
            Return pDescrUfficio & ""
        End Get
    End Property
    Public ReadOnly Property DescrEnte() As String
        Get
            If pDescrEnte = "" Then
                leggiDati()
            End If
            Return pDescrEnte & ""
        End Get
    End Property
    Public ReadOnly Property DescrArchivioBreve() As String
        Get
            If pDescrArchivioBreve = "" Then
                leggiDati()
            End If
            Return pDescrArchivioBreve & ""
        End Get
    End Property
    Public ReadOnly Property DescrDipartimentoBreve() As String
        Get
            If pDescrDipartimentoBreve = "" Then
                leggiDati()
            End If
            Return pDescrDipartimentoBreve & ""
        End Get
    End Property
    Public ReadOnly Property DescrUfficioBreve() As String
        Get
            If pDescrUfficioBreve = "" Then
                leggiDati()
            End If
            Return pDescrUfficioBreve & ""
        End Get
    End Property
    Public ReadOnly Property DescrEnteBreve() As String
        Get
            If pDescrEnteBreve = "" Then
                leggiDati()
            End If
            Return pDescrEnteBreve & ""
        End Get
    End Property
    Public Function SupervisoriUfficio(ByVal flussoDocumentale As String) As ICollection
        'rivedere in InvioDelibera.aspx
        If pSupervisoriUfficio Is Nothing OrElse pSupervisoriUfficio.Count = 0 Then
            pSupervisoriUfficio = New Hashtable
            Call Leggi_SupervisoriUfficio(flussoDocumentale)
        End If
        Return pSupervisoriUfficio
    End Function
    Public Function ResponsabileUfficio(ByVal flussoDocumentale As String) As String
        If pResponsabileUfficio = System.String.Empty Then
            Call Leggi_ResponsabileUfficio(flussoDocumentale)
        End If
        Return pResponsabileUfficio
    End Function
    Public Function QuartoLivelloUfficio(ByVal flussoDocumentale As String) As ICollection
        If p4LivelloUfficio Is Nothing OrElse p4LivelloUfficio.Count = 0 Then
            p4LivelloUfficio = New Hashtable
            Call Leggi_4LivelloUfficio(flussoDocumentale)
        End If
        Return p4LivelloUfficio
    End Function
    Public Function CollaboratoriUfficio(ByVal flussoDocumentale As String) As ICollection
        If pCollaboratoriUfficio Is Nothing OrElse pCollaboratoriUfficio.Count = 0 Then
            pCollaboratoriUfficio = New Hashtable
            Call Leggi_CollaboratoriUfficio(flussoDocumentale)
        End If
        Return pCollaboratoriUfficio
    End Function
    Public Function UtentiUfficio(ByVal flussoDocumentale As String) As ICollection
        If pUtentiUfficio Is Nothing OrElse pUtentiUfficio.Count = 0 Then
            pUtentiUfficio = New Hashtable
            Call Leggi_UtentiUfficio(flussoDocumentale)
        End If
        Return pUtentiUfficio
    End Function
    Public Function UfficiConsultabili(ByVal flussoDocumentale As String, ByVal ufficio As String) As ICollection
        If pUfficiConsultabili Is Nothing OrElse pUfficiConsultabili.Count = 0 Then
            pUfficiConsultabili = New ArrayList
            Call Leggi_UfficiConsultabili(flussoDocumentale, ufficio)
        End If
        Return pUfficiConsultabili
    End Function
    Public Function leggiUfficioPubblico(ByVal codUffInterno As String) As String
        Const SFunzione As String = "leggiUfficioPubblico"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader
        Dim stringReturn As String = ""
        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Sqlq = " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente " & _
                         " FROM Struttura " & _
                         " WHERE  ((str_id = '" & codUffInterno & "'))"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(3) Then stringReturn = rdr.GetString(3)
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
        Return stringReturn
    End Function

    Public Function leggiTipologiaUfficio(ByVal codUffInterno As String) As Object
        Const SFunzione As String = "leggiTipologiaUfficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader
        Dim stringReturn As String = ""
        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Sqlq = " SELECT     Str_descrizione, Str_id, Str_tipo, Str_codiceUtente " & _
               " FROM Struttura " & _
               " WHERE  ((str_id = '" & codUffInterno & "'))"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(2) Then stringReturn = rdr.GetString(2)
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
        Return stringReturn
    End Function
    Public Function GetDescrizioneDipartimentoPerModuli(ByVal codDipartimento As String) As String
        Dim result As String = ""

       
        Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

        Dim Sqlq As String = ""
        'or All_Tipo=16
        Sqlq = " SELECT  DescrizioneModello FROM DescrizioniStruttureModuli " & _
        " WHERE     idStruttura = @idStruttura  "


        Dim param(0) As SqlClient.SqlParameter

        Dim par As New SqlClient.SqlParameter("idStruttura", SqlDbType.VarChar)
        par.Value = codDipartimento
        param(0) = par


        Dim rdr As SqlClient.SqlDataReader

        Try

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, param)

            While rdr.Read()
                If Not rdr.IsDBNull(0) Then result = rdr.GetString(0)
            End While
            rdr.Close()

        Catch ex As SqlClient.SqlException
            Err.Number = ex.Number
            Err.Description = ex.Message
            rdr.Close()
            rdr = Nothing
        Catch ex As Exception
            Err.Description = ex.Message
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing

        End Try


        Return result

    End Function
    Public Function GetTuttiDipartimenti() As ArrayList
        Dim listaDipartimenti As New ArrayList
        Const SFunzione As String = "GetTuttiDipartimenti"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            'criteri abituali
            Sqlq = "SELECT  Str_id, Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                              "FROM         Struttura   " & _
                                " where Str_tipo='DIP' and Str_attivio= 1 " & _
                                " order by   Str_codiceUtente "

          

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            Dim temp As StrutturaInfo
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    temp = New StrutturaInfo
                    temp.CodiceInterno = rdr.GetString(0)
                    temp.CodicePubblico = rdr.GetString(1)
                    temp.DescrizioneBreve = rdr.GetString(2)
                    temp.Tipologia = rdr.GetString(3)
                    listaDipartimenti.Add(temp)
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
        Return listaDipartimenti
    End Function

    Public Function GetTuttiDipartimentiVisibiliPerNotifica() As ArrayList
        Dim listaDipartimenti As New ArrayList
        Const SFunzione As String = "GetTuttiDipartimenti"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            'criteri abituali
            Sqlq = "SELECT  Str_id, Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                              "FROM         Struttura   " & _
                                " where Str_tipo='DIP' and Str_attivio= 1 and Str_visibilePerNotifica = 1 " & _
                                " order by   Str_codiceUtente "



            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            Dim temp As StrutturaInfo
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    temp = New StrutturaInfo
                    temp.CodiceInterno = rdr.GetString(0)
                    temp.CodicePubblico = rdr.GetString(1)
                    temp.DescrizioneBreve = rdr.GetString(2)
                    temp.Tipologia = rdr.GetString(3)
                    listaDipartimenti.Add(temp)
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
        Return listaDipartimenti
    End Function

    Public Function GetTuttiUfficiDelDipartimento(Optional ByVal str_codiceDipartimento As String = "") As ArrayList
        Dim listaDipartimenti As New ArrayList
        Const SFunzione As String = "GetTuttiUfficiDelDipartimento"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            'criteri abituali
            Sqlq = "SELECT DISTINCT Str_id, Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                              "FROM         Struttura dip  " & _
                                " where Str_tipo='UFF' and Str_attivio=1 "
            If Not String.IsNullOrEmpty(str_codiceDipartimento) Then
                Sqlq += " and  str_padre='" & str_codiceDipartimento & "' "
            End If
            Sqlq += " order by   Str_codiceUtente "



            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            Dim temp As StrutturaInfo
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    temp = New StrutturaInfo
                    temp.CodiceInterno = rdr.GetString(0)
                    temp.CodicePubblico = rdr.GetString(1)
                    temp.DescrizioneBreve = rdr.GetString(2)
                    temp.Tipologia = rdr.GetString(3)
                    listaDipartimenti.Add(temp)
                End If
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
            Log.Debug("Fine " & SFunzione)
        End Try
        Return listaDipartimenti
    End Function


End Class
