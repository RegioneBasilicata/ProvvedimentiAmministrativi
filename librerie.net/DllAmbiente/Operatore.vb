Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XmlReader
Imports System.Data.SqlClient
Imports System.Configuration



Public Class Operatore

    Private pStato As String
    Private pEmail As String
    Private pCodiceFiscale As String
    Private pCognome As String
    Private pNome As String
    Private pRuoli As Collections.Hashtable
    Private pGruppi As Collections.ArrayList
    Private pAttributi As Collections.Hashtable
    Private pUfficiDipendenti As Collections.ArrayList
    Private pDipartimentoDipendenti As Collections.ArrayList
    Private password As String
    Private passwordPrimoAccesso As String

    Public pCodice As String
    Public oUfficio As DllAmbiente.Ufficio
    Public esito As String
    Public descrEsito As String
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Operatore))

    Private pOpzioniMessaggi As Collections.Hashtable

    Public ReadOnly Property DipartimentoDipendenti(ByVal flussoDocumentale As String) As ArrayList
        Get
            If pDipartimentoDipendenti Is Nothing Then
                Leggi_DipartimentoDipendenti(flussoDocumentale)
            End If
            Return pDipartimentoDipendenti
        End Get
    End Property
    Public ReadOnly Property UfficiDipendenti(ByVal flussoDocumentale As String, Optional ByVal idDipartimento As String = "") As IList
        Get
            If idDipartimento <> "" Then
                Return Leggi_UfficiDipendentiFiltratoPerDip(flussoDocumentale, idDipartimento)
            Else
                Return Leggi_UfficiDipendentiFiltratoPerDip(flussoDocumentale)
            End If
            'If pUfficiDipendenti Is Nothing Then
            '    Leggi_UfficiDipendenti()
            'End If
            Return pUfficiDipendenti
        End Get
    End Property


    Public Property Cognome() As String
        Get
            If pCognome Is Nothing Then
                Leggi_Dati()
            End If
            Return pCognome
        End Get
        Set(ByVal value As String)
            pCognome = value
        End Set
    End Property
    Public Property Email() As String
        Get
            If pEmail Is Nothing Then
                Leggi_Dati()
            End If
            Return pEmail
        End Get
        Set(ByVal value As String)
            pEmail = value
        End Set
    End Property
    Public Property CodiceFiscale() As String
        Get
            If pCodiceFiscale Is Nothing Then
                Leggi_Dati()
            End If
            Return pCodiceFiscale
        End Get
        Set(ByVal value As String)
            pCodiceFiscale = value
        End Set
    End Property
    Public ReadOnly Property Stato() As String
        Get
            If pStato Is Nothing Then
                Leggi_Dati()
            End If
            Return pStato
        End Get
    End Property
    Public ReadOnly Property Nominativo() As String
        Get
            If pNome Is Nothing Or pCognome Is Nothing Then
                Leggi_Dati()
            End If
            Return pCodice & " - " & pCognome & " " & pNome
        End Get
    End Property
    Public ReadOnly Property Nome() As String
        Get
            If pNome Is Nothing Then
                Leggi_Dati()
            End If
            Return pNome
        End Get
    End Property
    Public ReadOnly Property Attributo(ByVal codAttributo As String, Optional ByVal codProcedura As String = "") As String
        Get
            If pAttributi Is Nothing Then
                Call leggiAttributi(codProcedura)
            End If
            If pAttributi Is Nothing Then
                Return ""
            End If
            If Not pAttributi.Contains(codAttributo) Then
                Call leggiAttributi(codProcedura)
            Else
                If pAttributi.Contains(codAttributo) Then
                    Return pAttributi.Item(codAttributo) & ""
                Else
                    Return ""
                End If
            End If
            Return ""
        End Get
    End Property
    Public Property ListaOpzioniMessaggi() As Hashtable
        Get
            Return pOpzioniMessaggi
        End Get
        Set(ByVal value As Hashtable)
            pOpzioniMessaggi = value
        End Set

    End Property


    Public ReadOnly Property OpzioniMessaggi(ByVal CodOpzione As String) As Integer
        Get
            If pOpzioniMessaggi Is Nothing Then
                Call leggiOpzioniMessaggi()
            End If
            If pOpzioniMessaggi Is Nothing Then
                Return 0
            End If
            If Not pOpzioniMessaggi.Contains(CodOpzione) Then
                Call leggiOpzioniMessaggi()
            Else
                If pOpzioniMessaggi.Contains(CodOpzione) Then
                    Return pOpzioniMessaggi.Item(CodOpzione)
                Else
                    Return 0
                End If
            End If
            Return 0
        End Get
    End Property
    Public Property Codice() As String
        Get
            Return pCodice
        End Get
        Set(ByVal Value As String)
            If Trim(Value) <> "" And (Value <> pCodice) Then
                pCodice = Value
                Call Leggi_UfficioOperatore()
            End If
        End Set
    End Property
    Private Sub Leggi_Dati()
        Const SFunzione As String = "Leggi_Dati"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("***INIZIO Leggi_Dati - Operatore: " & pCodice & " " & Now)
            
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")
            Dim conta As Integer = 0

            Sqlq = "SELECT     Op_Cognome, Op_Nome,  Op_Stato , Op_CodiceFiscale, Op_Email " & _
                   " FROM Tab_Operatori " & _
                   " WHERE     (Op_Codice_Operatore = '" & pCodice & "')"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then pCognome = rdr.GetString(0)
                If Not rdr.IsDBNull(1) Then pNome = rdr.GetString(1)
                If Not rdr.IsDBNull(2) Then pStato = rdr.GetByte(2)
                If Not rdr.IsDBNull(3) Then pCodiceFiscale = rdr.GetString(3)
                If Not rdr.IsDBNull(4) Then pEmail = rdr.GetString(4)
            End While
            rdr.Close()
        Catch ex As Exception
            Log.Error(ex.Message)
            If Not rdr.IsClosed Then
                rdr.Close()
            End If
            rdr = Nothing
        Finally
             Log.Debug("***Fine Leggi_Dati - Operatore: " & pCodice & " " & Now)
           
        End Try
    End Sub
    Public Sub Leggi_Dati_CF_Ufficio(ByVal cf As String, ByVal codUfficioPubblico As String)
        Const SFunzione As String = "Leggi_Dati_CF_Ufficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Dim conta As Integer = 0
            'LU 25/05/2009
            'aggiungere nella vista Op_Email ?
            Sqlq = " SELECT     Op_Cognome, Op_Nome,  Op_Stato , Op_CodiceFiscale,Op_Codice_Operatore  " &
                   " FROM Tab_Operatori JOIN Strutture_Operatori on Tab_Operatori.Op_Codice_Operatore = Strutture_Operatori.Sop_Operatore    join Struttura  on ( Sop_Struttura=Str_id and str_codiceUtente= '" & codUfficioPubblico & "' )" &
                   " WHERE     (Op_CodiceFiscale = '" & cf & "') AND (Op_Stato= 1)"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then pCognome = rdr.GetString(0)
                If Not rdr.IsDBNull(1) Then pNome = rdr.GetString(1)
                If Not rdr.IsDBNull(2) Then pStato = rdr.GetByte(2)
                If Not rdr.IsDBNull(3) Then pCodiceFiscale = rdr.GetString(3)
                If Not rdr.IsDBNull(4) Then Codice = rdr.GetString(4)
                'If Not rdr.IsDBNull(4) Then pEmail = rdr.GetString(4)
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
    Private Sub Leggi_DipartimentoDipendenti(ByVal flussoDocumentale As String)
        Const SFunzione As String = "Leggi_DipartimentoDipendenti"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Dim uffcons As New ArrayList
            uffcons = Me.oUfficio.UfficiConsultabili(flussoDocumentale, Me.pCodice)
            If uffcons Is Nothing OrElse uffcons.Count = 0 Then
                'criteri abituali
                Sqlq = "SELECT DISTINCT dip.Str_id, dip.Str_codiceUtente, dip.Str_descrBreve, dip.Str_tipo " & _
                                  "FROM         Struttura INNER JOIN " & _
                                    "Strutture_Operatori ON Struttura.Str_id = Strutture_Operatori.Sop_Struttura INNER JOIN " & _
                                    "Struttura dip ON Struttura.Str_padre = dip.Str_id "

                If Me.oUfficio.bUfficioDirigenzaDipartimento Then
                    Sqlq = Sqlq & " WHERE (Struttura.Str_padre = '" & Me.oUfficio.CodDipartimento & "') "
                Else
                    If Not (Me.oUfficio.bUfficioControlloAmministrativo Or Me.oUfficio.bUfficioSegreteria Or Me.oUfficio.bUfficioSegreteriaPresidenzaLegittimita Or Me.oUfficio.bUfficioSegreteriaPresidenzaSegretario Or Me.oUfficio.bUfficioRagioneria) Then
                        Sqlq = Sqlq & " WHERE (Strutture_Operatori.Sop_Operatore = '" & pCodice & "') "
                    End If
                End If
                Sqlq = Sqlq & IIf(Sqlq.Contains(" WHERE "), " and  Struttura.Str_attivio=1", " WHERE  Struttura.Str_attivio=1")
                Sqlq = Sqlq & " ORDER BY dip.Str_codiceUtente "

            Else
                'esiste l'attributo consultazione
                Dim valoriSelect As String = "'"
                For a As Integer = 0 To uffcons.Count - 2
                    valoriSelect = valoriSelect & uffcons(a) & "','"
                Next
                valoriSelect = valoriSelect & uffcons(uffcons.Count - 1) & "'"

                Sqlq = " SELECT DISTINCT Str_id ,Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                        " FROM   ( " & _
                        " SELECT DISTINCT Str_id, Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                        " FROM Struttura " & _
                        " WHERE      (Str_padre IN (" & valoriSelect & ")) AND (Str_tipo = 'DIP')  and Struttura.Str_attivio=1" & _
                        " UNION " & _
                        " SELECT DISTINCT Str_id ,Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                        " FROM    Struttura" & _
                        " WHERE      (Str_id IN (" & valoriSelect & ")) AND (Str_tipo = 'DIP') and Struttura.Str_attivio=1" & _
                        " UNION " & _
                        " SELECT DISTINCT Str_id ,Str_codiceUtente, Str_descrBreve, Str_tipo " & _
                        " FROM         Struttura AS Struttura_3 " & _
                        " WHERE     (Str_id IN " & _
                        "  (SELECT DISTINCT Struttura_2.Str_padre " & _
                        " FROM          Struttura AS Struttura_2 INNER JOIN " & _
                        "  Struttura AS Struttura_1 ON Struttura_2.Str_padre = Struttura_1.Str_id" & _
                        " WHERE  (Struttura_2.Str_id IN (" & valoriSelect & ")) AND (Struttura_2.Str_tipo = 'UFF') and Struttura_2.Str_attivio=1))) AS derivedtbl_1" & _
                        " ORDER BY Str_codiceUtente "

            End If

            If pDipartimentoDipendenti Is Nothing Then
                pDipartimentoDipendenti = New ArrayList
            End If
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            Dim temp As StrutturaInfo
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    temp = New StrutturaInfo
                    temp.CodiceInterno = rdr.GetString(0)
                    temp.CodicePubblico = rdr.GetString(1)
                    temp.DescrizioneBreve = rdr.GetString(2)
                    temp.Tipologia = rdr.GetString(3)
                    pDipartimentoDipendenti.Add(temp)
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
    Private Function Leggi_UfficiDipendentiFiltratoPerDip(ByVal flussoDocumentale As String, Optional ByVal idDipartimento As String = "") As IList
        Const SFunzione As String = "Leggi_UfficiDipendentiFiltratoPerDip"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim localUfficiDipendentiPerDip As IList = New ArrayList

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Dim uffcons As New ArrayList
            uffcons = Me.oUfficio.UfficiConsultabili(flussoDocumentale, Me.pCodice)
            If uffcons Is Nothing OrElse uffcons.Count = 0 Then
                Sqlq = " SELECT DISTINCT Struttura.Str_id, Struttura.Str_codiceUtente, Struttura.Str_descrBreve,Struttura.Str_padre,  Struttura.Str_tipo " & _
                               " FROM Struttura INNER JOIN " & _
                               "    Strutture_Operatori ON Struttura.Str_id = Strutture_Operatori.Sop_Struttura"

                If Me.oUfficio.bUfficioDirigenzaDipartimento Then
                    Sqlq = Sqlq & " WHERE (Struttura.Str_padre = '" & Me.oUfficio.CodDipartimento & "')"
                Else
                    'modgg 10-06
                    If Not (Me.oUfficio.bUfficioControlloAmministrativo Or Me.oUfficio.bUfficioSegreteria Or Me.oUfficio.bUfficioSegreteriaPresidenzaLegittimita Or Me.oUfficio.bUfficioSegreteriaPresidenzaSegretario Or Me.oUfficio.bUfficioRagioneria) Then
                        Sqlq = Sqlq & " WHERE (Strutture_Operatori.Sop_Operatore = '" & pCodice & "')"
                    End If
                End If
                Sqlq = Sqlq & IIf(Sqlq.Contains(" WHERE "), " and  Struttura.Str_attivio=1", " WHERE  Struttura.Str_attivio=1")
            Else

                'esiste l'attributo consultazione
                Dim valoriSelect As String = "'"
                For a As Integer = 0 To uffcons.Count - 2
                    valoriSelect = valoriSelect & uffcons(a) & "','"
                Next
                valoriSelect = valoriSelect & uffcons(uffcons.Count - 1) & "'"

                Sqlq = "SELECT DISTINCT Str_id, Str_codiceUtente, Str_descrBreve, Str_padre, str_tipo " & _
                " FROM Struttura " & _
                " WHERE     ((Str_tipo = 'UFF') AND (Str_radice IN (" & valoriSelect & ")) OR " & _
                " (Str_tipo = 'UFF') AND (Str_padre IN (" & valoriSelect & ")) OR " & _
                " (Str_tipo = 'UFF') AND (Str_id IN (" & valoriSelect & ")))" & _
                " ORDER BY Str_codiceUtente "
                ' " AND  Struttura.Str_attivio=1 " & _
            End If

            Dim temp As StrutturaInfo

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If idDipartimento = "" Then
                    If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                        temp = New StrutturaInfo
                        temp.CodiceInterno = rdr.GetString(0)
                        temp.CodicePubblico = rdr.GetString(1)
                        temp.DescrizioneBreve = rdr.GetString(2)
                        temp.Padre = rdr.GetString(3)
                        temp.Tipologia = rdr.GetString(4)
                        localUfficiDipendentiPerDip.Add(temp)
                    End If
                Else
                    If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) And rdr.GetString(3) = idDipartimento Then
                        temp = New StrutturaInfo
                        temp.CodiceInterno = rdr.GetString(0)
                        temp.CodicePubblico = rdr.GetString(1)
                        temp.DescrizioneBreve = rdr.GetString(2)
                        temp.Padre = rdr.GetString(3)
                        temp.Tipologia = rdr.GetString(4)
                        localUfficiDipendentiPerDip.Add(temp)
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
        Return localUfficiDipendentiPerDip
    End Function
    Private Sub Leggi_UfficioOperatore()
        Const SFunzione As String = "Leggi_Ufficio"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader
        Try
           Log.Debug("***INIZIO " & SFunzione & " Operatore: " & pCodice & " " & Now)
             Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Sqlq = "SELECT     Sop_Struttura " & _
                   " FROM Strutture_Operatori " & _
                   "WHERE     (Sop_Operatore = '" & pCodice & "')"


            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then
                    oUfficio.CodUfficio = rdr.GetString(0)
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
            Log.Debug("***FINE " & SFunzione & " Operatore: " & pCodice & " " & Now)
            
        End Try
    End Sub
    Private Sub leggiRuoli()
        Const SFunzione As String = "Leggi_Ruoli"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")


            Sqlq = "(SELECT     Tab_Operatori_Ruoli.TOR_Ruolo , Tab_Ruoli.Tru_Descrizione , case when (TOR_Operatore = '*') then 'Tutti' else 'Personale' end  as Origine " &
                   " FROM Tab_Operatori_Ruoli " &
                   " INNER JOIN Tab_Ruoli ON Tab_Operatori_Ruoli.TOR_Ruolo = Tab_Ruoli.Tru_Codice " &
                   " WHERE (TOR_Operatore = '" & pCodice & "' or TOR_Operatore = '*') " &
                   " AND (Tab_Ruoli.Tru_Procedura='ORDINANZE' or Tab_Ruoli.Tru_Procedura='DECRETI' or Tab_Ruoli.Tru_Procedura='DELIBERE' or Tab_Ruoli.Tru_Procedura='DETERMINE' OR Tab_Ruoli.Tru_Procedura='DISPOSIZIONI' or Tab_Ruoli.Tru_Procedura='*'))  " &
                   "UNION " &
                   "( " &
                   "Select Tab_Operatori_Ruoli.TOR_Ruolo , Tab_Ruoli.Tru_Descrizione, 'Gruppo' as Origine " &
                   " FROM         Tab_Operatori_Ruoli " &
                   "  INNER JOIN Tab_Operatori_Gruppi ON Tab_Operatori_Ruoli.TOR_Operatore = Tab_Operatori_Gruppi.TOG_Gruppo " &
                   " INNER JOIN Tab_Ruoli ON Tab_Operatori_Ruoli.TOR_Ruolo = Tab_Ruoli.Tru_Codice " &
                   " WHERE     (Tab_Operatori_Gruppi.TOG_Operatore = '" & pCodice & "') " &
                   " AND (Tab_Ruoli.Tru_Procedura='ORDINANZE' or Tab_Ruoli.Tru_Procedura='DECRETI' or Tab_Ruoli.Tru_Procedura='DELIBERE' or Tab_Ruoli.Tru_Procedura='DETERMINE' OR Tab_Ruoli.Tru_Procedura='DISPOSIZIONI' or Tab_Ruoli.Tru_Procedura='*') ) "

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            If pRuoli Is Nothing Then
                pRuoli = New Hashtable
            End If
            While (rdr.Read())
                If (Not rdr.IsDBNull(0)) And (Not rdr.IsDBNull(1)) And Not (rdr.IsDBNull(2)) Then
                    If Not pRuoli.ContainsKey(rdr.GetString(0)) Then
                        pRuoli.Add(rdr.GetString(0), (New RuoloInfo(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2))))
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
    Private Sub leggiAttributi(Optional ByVal procedura As String = "")
        Const SFunzione As String = "leggiAttributi"
        Dim Sqlq As String
        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("**INIZIO " & SFunzione & " Operatore: " & pCodice & " " & Now)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")

            Sqlq = "(SELECT     TOA_Attributo, Toa_Valore " & _
                   " FROM TAB_Operatori_Attributi " & _
                   " WHERE     (TOA_Operatore = '" & pCodice & "' or TOA_Operatore = '*'  ) "

            If procedura <> "" Then
                Sqlq = Sqlq & "AND (TOA_Procedura = '" & procedura & "' or TOA_Procedura = '*'   or  TOA_Procedura = 'ORDINANZE' or  TOA_Procedura = 'DECRETI' or  TOA_Procedura = 'DISPOSIZIONI'  or TOA_Procedura = 'DETERMINE' or TOA_Procedura = 'DELIBERE') "
            End If

            Sqlq = Sqlq & ")  " & _
                   "UNION " & _
                   "( " & _
                   " SELECT     TAB_Operatori_Attributi.TOA_Attributo, TAB_Operatori_Attributi.Toa_Valore " & _
                   " FROM         TAB_Operatori_Attributi INNER JOIN " & _
                   "           Tab_Operatori_Gruppi ON TAB_Operatori_Attributi.TOA_Operatore = Tab_Operatori_Gruppi.TOG_Gruppo " & _
                   " WHERE     (Tab_Operatori_Gruppi.TOG_Operatore = '" & pCodice & "') " & _
                   ") "

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) And Not (rdr.IsDBNull(1)) Then
                    addAttributi(rdr.GetString(0), rdr.GetString(1))
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
            Log.Debug("**FINE " & SFunzione & " Operatore: " & pCodice & " " & Now)
            
        End Try
    End Sub
    Private Sub leggiGruppi()
        Const SFunzione As String = "leggiGruppi"
        Dim Sqlq As String

        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")

            Sqlq = "SELECT     TOG_Gruppo " & _
                   " FROM Tab_Operatori_Gruppi " & _
                   " WHERE     (TOG_Operatore = '" & Me.Codice & "')"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then
                    addGruppo(rdr.GetString(0))
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

    Private Sub leggiOpzioniMessaggi()
        Const SFunzione As String = "leggiOpzioniMessaggi"
        Dim Sqlq As String

        Dim rdr As SqlClient.SqlDataReader

        Try
            Log.Info("***INIZIO " & SFunzione & " Operatore: " & pCodice & " " & Now)
            
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")


            Sqlq = " Select [id],[Op_Codice_Operatore],[idMessaggio],[abilitato] FROM " & _
            " Tab_Operatori_Messaggi where Op_Codice_Operatore= '" & pCodice & "' "


            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            While (rdr.Read())
                If Not rdr.IsDBNull(0) Then
                    addOpzioneMessaggi(rdr.GetInt32(2), rdr.GetInt16(3))
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
            Log.Info("***FINE " & SFunzione & " Operatore: " & pCodice & " " & Now)
            
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
    Private Function addOpzioneMessaggi(ByVal codOpzione As String, ByVal valOpzione As String) As Boolean
        If pOpzioniMessaggi Is Nothing Then
            pOpzioniMessaggi = New Hashtable
        End If
        If Not pOpzioniMessaggi.Contains(codOpzione) Then
            pOpzioniMessaggi.Add(codOpzione, valOpzione)
        End If
    End Function
    Private Function addGruppo(ByVal codGruppo As String) As Boolean
        If pGruppi Is Nothing Then
            pGruppi = New ArrayList
        End If
        If Not pGruppi.Contains(codGruppo) Then
            pGruppi.Add(codGruppo)
        End If
    End Function
    Public Sub New()
        oUfficio = New DllAmbiente.Ufficio
    End Sub
    Public Sub Controlla_Autenticazione(ByVal passwordDigitata As String)
        Const SFunzione As String = "Controlla_Autenticazione"
        Dim Sqlq As String
        Dim pwdCriptata As String
        esito = 1
        descrEsito = "Autenticazione non riuscita"
        Log.Debug("Inizio" & SFunzione)

        Dim ggScadenzaPwd As Integer = 30
        Dim sGgScadenzaPwd As String = Me.Attributo("SCADENZA_PWD", "ATTIDIGITALI")
        If IsNumeric(Trim(sGgScadenzaPwd)) Then
            ggScadenzaPwd = CType(Trim(sGgScadenzaPwd), Integer)
            If ggScadenzaPwd < 0 Then
                ggScadenzaPwd = 30
            End If
        End If

        Dim ggScadenzaAcc As Integer = 30
        Dim sGgScadenzaAcc As String = Me.Attributo("SCADENZA_ACCOUNT", "ATTIDIGITALI")
        If IsNumeric(Trim(sGgScadenzaAcc)) Then
            ggScadenzaAcc = CType(Trim(sGgScadenzaAcc), Integer)
            If ggScadenzaAcc < 0 Then
                ggScadenzaAcc = 30
            End If
        End If
        Try
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")
            Sqlq = "SELECT     Op_Cognome, Op_Nome, Op_Password,Op_Password_H, Op_Data_Ultimo_Utilizzo, Op_Fl_Password, Op_Stato ,Op_Data " & _
                   " FROM         Tab_Operatori " & _
                   " WHERE (Op_Codice_Operatore = '" & pCodice & "' )"
            Using sqlDataRead As SqlClient.SqlDataReader = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

                If Not sqlDataRead.HasRows Then
                    esito = "1"
                    Throw New LoginException("Codice utente e/o password sbagliati", esito)
                End If
                While sqlDataRead.Read()
                    pwdCriptata = sqlDataRead.Item("Op_Password_H") & ""
                    passwordPrimoAccesso = sqlDataRead.Item("Op_Password") & ""
                    pCognome = sqlDataRead.Item("Op_Cognome") & ""
                    pNome = sqlDataRead.Item("Op_Nome") & ""
                    pStato = sqlDataRead.Item("Op_Stato") & ""
                    If Stato.Equals("0") Then
                        esito = "1"
                        Throw New LoginException("L'operatore non risulta più abilitato alla procedura", esito)
                    End If
                    If pwdCriptata = "" Then
                        If UCase(Trim(passwordDigitata)) <> UCase(Trim(passwordPrimoAccesso)) Then
                            esito = "1"
                            Throw New LoginException("Codice utente e/o password sbagliati", esito)
                        End If
                    Else
                        If UCase(Trim(pwdCriptata)) <> UCase(Trim(Intema.DBUtility.Utility.Cripta(True, "ametni", passwordDigitata, True))) Then
                            esito = "1"
                            Throw New LoginException("Codice utente e/o password sbagliati", esito)
                        End If
                    End If
                    If sqlDataRead.Item("Op_Fl_Password").Equals("1") Then
                        esito = "1"
                        Throw New LoginException("L'operatore è stato disattivato per ripetuti tentativi di accesso", esito)
                    End If
                    If Configuration.ConfigurationSettings.AppSettings("AUTENTICAZIONE") <> "IMS" Then
                        If DateDiff(DateInterval.Day, DirectCast(sqlDataRead.Item("Op_Data"), Date), Now) > ggScadenzaPwd Then
                            esito = "2"
                            Update_Operatore(Me)
                            Throw New LoginException("Attenzione! Password attiva da più di tre mesi. Per maggiore sicurezza ti consigliamo di <a href='ProfiloOperatoreAction.aspx'>modificarla.</a>", esito)
                        End If
                        If DateDiff(DateInterval.Day, DirectCast(sqlDataRead.Item("Op_Data_Ultimo_Utilizzo"), Date), Now) > ggScadenzaAcc Then
                            esito = "1"
                            Throw New LoginException("L'operatore non è più abilitato alla procedura", esito)
                        End If
                    End If
                End While
            End Using
            If Update_Operatore(Me) < 0 Then
                Throw New LoginException("")
            End If
            esito = "0"
            descrEsito = ""
        Catch ex As LoginException
            Log.Error(ex.Message)
            Throw New LoginException(ex.Message, esito)
        Catch ex As Exception
            Log.Error(ex.Message)
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Public Function Ruoli() As Hashtable
        If pRuoli Is Nothing Then
            Call leggiRuoli()
        End If
        Return pRuoli
    End Function
    Public Function Elenco_Attributi(Optional ByVal codProcedura As String = "") As Array
        If pAttributi Is Nothing Then
            pAttributi = New Hashtable
            Call leggiAttributi(codProcedura)
        End If
        Return pAttributi.Keys
    End Function
    Public Function Test_Ruolo(ByVal codRuolo As String) As Boolean
        If pRuoli Is Nothing Then
            Call leggiRuoli()
        End If
        Return pRuoli.Contains(codRuolo)
    End Function
    Public Function Test_Attributo(ByVal codAttributo As String, ByVal valAttributo As String, Optional ByVal codProcedura As String = "") As Boolean
        If pAttributi Is Nothing Then
            Call leggiAttributi(codProcedura)
        End If
        Return (UCase(pAttributi.Item(codAttributo)) = UCase(valAttributo))
    End Function


    Public Function Test_Gruppo(ByVal codGruppo As String) As Boolean
        If pGruppi Is Nothing Then
            Call leggiGruppi()
        End If
        Return pGruppi.Contains(codGruppo)
    End Function
    Public Function cambia_Password(ByVal vecchiaPS As String, ByVal nuovaPS As String, ByVal confNuovaPS As String) As Object
        Const SFunzione As String = "cambia_Password"
        Dim Sqlq As String
        Dim vP(1, 1) As Object
        Dim vR As Object = Nothing
        Dim vRitPar(1) As Object
        Dim rdr As SqlClient.SqlDataReader

        Dim vecchiaPSDecrittata As String

        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")


            If nuovaPS <> confNuovaPS Then
                vRitPar(0) = 1
                vRitPar(1) = "La conferma della password non coincide"
                Throw New LoginException()
            End If

            Sqlq = "SELECT     Op_Password, Op_Password_H ,  Op_Data_Ultimo_Utilizzo, Op_Data " & _
                   " FROM Tab_Operatori " & _
                   " WHERE     (Op_Codice_Operatore = '" & Me.Codice & "')"

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            If Not rdr.HasRows Then
                vRitPar(0) = 1
                vRitPar(1) = "Operatore inesistente"
                Throw New LoginException()
            End If

            While (rdr.Read())
                Dim pwdRegistrata As String
                If rdr.IsDBNull(1) Then
                    pwdRegistrata = ""
                Else
                    pwdRegistrata = rdr.GetString(1)
                End If
                If pwdRegistrata <> "" Then
                    vecchiaPSDecrittata = Intema.DBUtility.Utility.Cripta(False, "ametni", pwdRegistrata, True)
                Else
                    vecchiaPSDecrittata = rdr.GetString(0)
                End If

                If vecchiaPSDecrittata <> vecchiaPS Then
                    vRitPar(0) = 1
                    vRitPar(1) = "La vecchia password non coincide"
                    Throw New LoginException()
                End If
            End While
            rdr.Close()
            Me.password = nuovaPS
            If (Update_Operatore(Me) <= 0) Then
                vRitPar(0) = 1
                vRitPar(1) = "Aggiornamento Password non effettuato"
                Throw New LoginException()
            End If


            vRitPar(0) = 0
            vRitPar(1) = "Password modificata con successo"

        Catch ex As LoginException
            Log.Error(ex.Message)
            Return vRitPar
        Catch ex As Exception
            Log.Error(ex.Message)
            Return vRitPar
        Finally
            Me.password = ""
        End Try
        Return vRitPar
    End Function
    Function Update_Operatore(ByVal item As Operatore) As Integer
        Dim conn As SqlClient.SqlConnection = Nothing
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim returnValue As Integer = -1
        Try
            Dim SQL_Update_Operatore As String = " UPDATE  [Tab_Operatori] " & _
                " SET [Op_Cognome] = '" & item.Cognome.Replace("'", "''") & "'" & _
                "     ,[Op_Nome] =  '" & item.Nome.Replace("'", "''") & "'" & _
                "     ,[Op_Data_Ultimo_Utilizzo] =convert(datetime, '" & Now.Month & "/" & Now.Day & "/" & Now.Year & "',102)" & _
                "     ,[Op_CodiceFiscale] = '" & item.CodiceFiscale & "" & "'" & _
                "     ,[Op_Email] = '" & item.Email & "'"

            '     ,[Op_Stato] = '" & item.Stato & "'" & _
            If Not String.IsNullOrEmpty(item.password) Then
                SQL_Update_Operatore = SQL_Update_Operatore & "     ,[Op_Password_H] =  '" & Intema.DBUtility.Utility.Cripta(True, "ametni", item.password, True) & "'"
                SQL_Update_Operatore = SQL_Update_Operatore & "     ,[Op_Data] =convert(datetime, '" & Now.Month & "/" & Now.Day & "/" & Now.Year & "',102) "
            End If
            SQL_Update_Operatore = SQL_Update_Operatore & " WHERE [Op_Codice_Operatore] = '" & item.Codice & "'"

            conn = New SqlClient.SqlConnection(Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR"))
            conn.Open()
            trans = conn.BeginTransaction

            returnValue = Intema.DBUtility.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL_Update_Operatore, Nothing, -1)
            trans.Commit()

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
            Throw New Exception(ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

        Leggi_Dati()
        Return returnValue
    End Function

    Function Insert_Opzioni_Messaggi(ByVal itemOp As Operatore) As Integer
        Dim conn As SqlClient.SqlConnection = Nothing
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim returnValue As Integer = -1
        Try
            Dim SQL_Delete_Opzioni_Messaggi As String = " delete from [Tab_Operatori_Messaggi] where Op_Codice_Operatore =  " & _
                " '" & itemOp.Codice & "'"

            conn = New SqlClient.SqlConnection(Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR"))
            conn.Open()
            trans = conn.BeginTransaction
            Dim param_codiceOp As String = "@CodiceUtente"
            Dim param_idMessaggio As String = "@idMessaggio"
            Dim param_abilitato As String = "@abilitato"
            Dim SQL_insert_Opzioni_Messaggi As String = " INSERT INTO [Tab_Operatori_Messaggi] " & _
             "([Op_Codice_Operatore]" & _
           ",[idMessaggio]" & _
           ",[abilitato])" & _
           " VALUES " & _
           "(" & param_codiceOp & _
           "," & param_idMessaggio & _
           "," & param_abilitato & " )"

            returnValue = Intema.DBUtility.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL_Delete_Opzioni_Messaggi, Nothing, -1)
            For Each Item As String In itemOp.ListaOpzioniMessaggi.Keys

                Dim parms(2) As SqlParameter
                parms(0) = New SqlParameter(param_codiceOp, SqlDbType.VarChar)
                parms(0).Value = itemOp.Codice
                parms(1) = New SqlParameter(param_idMessaggio, SqlDbType.Int)
                parms(1).Value = Item
                parms(2) = New SqlParameter(param_abilitato, SqlDbType.SmallInt)
                parms(2).Value = itemOp.OpzioniMessaggi(Item)

                returnValue = Intema.DBUtility.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL_insert_Opzioni_Messaggi, parms, -1)
            Next


            trans.Commit()

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
                returnValue = -1
            End If
            Throw New Exception(ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

        Return returnValue
    End Function
    Public Function Leggi_Dati_CF(ByVal cf As String) As Generic.List(Of SceltaOperatoreInfo)
        Const SFunzione As String = "Leggi_Dati_CF"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim listascelteoperatore As New Generic.List(Of SceltaOperatoreInfo)
        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            Dim parms(0) As SqlParameter

            parms(0) = New SqlParameter("@cf", SqlDbType.VarChar)
            parms(0).Value = cf
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.StoredProcedure, "SP_GET_OP_CF", parms, -1)

            While (rdr.Read())
                Dim scelta As New SceltaOperatoreInfo
                If Not rdr.IsDBNull(0) Then scelta.CodiceOperatore = rdr.GetString(0)
                If Not rdr.IsDBNull(1) Then scelta.CodiceUfficio = rdr.GetString(1)
                If Not rdr.IsDBNull(2) Then scelta.DescrizioneUfficio = rdr.GetString(2)
                If Not rdr.IsDBNull(3) Then scelta.CodiceUfficioPubblico = rdr.GetString(3)
                listascelteoperatore.Add(scelta)
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
        Return listascelteoperatore
    End Function
    Function Insert_Attributo_Operatore(ByVal itemOp As Operatore, ByVal codiceAttributo As String, ByVal valoreAttributo As String, Optional ByVal procedura As String = "*") As Integer
        Dim conn As SqlClient.SqlConnection = Nothing
        Dim trans As SqlClient.SqlTransaction = Nothing
        Dim returnValue As Integer = -1
        Try
            If Not String.IsNullOrEmpty(valoreAttributo) Then


                Dim SQL_Delete_Attributo_Operatore As String = " delete FROM TAB_Operatori_Attributi where TOA_Operatore =  " & _
                        " '" & itemOp.Codice & "' and TOA_Attributo = '" & codiceAttributo & "' "

                conn = New SqlClient.SqlConnection(Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR"))
                conn.Open()
                trans = conn.BeginTransaction
                Dim param_codiceOp As String = "@CodiceUtente"
                Dim param_codiceAttributo As String = "@codiceAttributo"
                Dim param_procedura As String = "@procedura"
                Dim param_valoreAttributo As String = "@valoreAttributo"
                Dim SQL_insert_Attributo_Operatore As String = " INSERT INTO TAB_Operatori_Attributi " & _
                 "(TOA_Operatore " & _
               ", TOA_Procedura " & _
               ", TOA_Attributo " & _
               ", Toa_Valore)" & _
               " VALUES " & _
               "(" & param_codiceOp & _
               "," & param_procedura & _
               "," & param_codiceAttributo & _
               "," & param_valoreAttributo & " )"

                returnValue = Intema.DBUtility.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL_delete_Attributo_operatore, Nothing, -1)
            
                Dim parms(3) As SqlParameter
                parms(0) = New SqlParameter(param_codiceOp, SqlDbType.VarChar)
                parms(0).Value = itemOp.Codice
                parms(1) = New SqlParameter(param_procedura, SqlDbType.VarChar)
                parms(1).Value = procedura
                parms(2) = New SqlParameter(param_codiceAttributo, SqlDbType.VarChar)
                parms(2).Value = codiceAttributo
                parms(3) = New SqlParameter(param_valoreAttributo, SqlDbType.VarChar)
                parms(3).Value = valoreAttributo

                returnValue = Intema.DBUtility.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL_insert_Attributo_Operatore, parms, -1)

                trans.Commit()
            End If
        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
                returnValue = -1
            End If
            Throw New Exception(ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        pAttributi = Nothing
        leggiAttributi()
        Return returnValue
    End Function


    Public Function Leggi_NominativoDaAnagrafica(ByVal idAnagrafica As String) As String
        Const SFunzione As String = "Leggi_Nominativo"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim str_nominativo As String = ""
        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")

            Dim SQL_Nominativo As String = "SELECT     isnull(Tab_Anagrafica.Op_Cognome,'') +' ' + isnull( Tab_Anagrafica.Op_Nome,'') " & _
                "  FROM         Tab_Anagrafica  where  id_anagrafica= @idAn"

            Dim parms(0) As SqlParameter

            parms(0) = New SqlParameter("@idAn", SqlDbType.BigInt)
            parms(0).Value = idAnagrafica

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, SQL_Nominativo, parms, -1)
            str_nominativo = ""
            While (rdr.Read())
                Dim scelta As New SceltaOperatoreInfo
                If Not rdr.IsDBNull(0) Then str_nominativo = rdr.GetString(0)

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
        Return str_nominativo
    End Function



    Public Function Leggi_IdAnagrafica(ByVal cod_operatore As String) As Long
        Const SFunzione As String = "Leggi_Nominativo"
        Dim rdr As SqlClient.SqlDataReader = Nothing
        Dim idAnagrafica As Long = 0
        Try
            Log.Debug("Inizio " & SFunzione)
            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("TABCENTR")

            Dim SQL_Nominativo As String = "SELECT    id_anagrafica " & _
                "  FROM         Tab_Operatori_Anagrafica  where  Op_Codice_Operatore = @codOp"

            Dim parms(0) As SqlParameter

            parms(0) = New SqlParameter("@codOp", SqlDbType.VarChar)
            parms(0).Value = cod_operatore

            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, SQL_Nominativo, parms, -1)

            While (rdr.Read())

                If Not rdr.IsDBNull(0) Then idAnagrafica = rdr.GetInt64(0)

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
        Return idAnagrafica
    End Function
End Class
