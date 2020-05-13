Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb
Imports System.Collections

Public Class OLEDBHelper
    Public Enum eccezioniAccess
        'credenzialiAccesso :   Impossibile avviare l'applicazione. 
        '                       Il file di informazioni sul gruppo di lavoro è mancante o aperto in modo 
        '                       esclusivo da un altro utente.
        '                       
        '                       Username o psw errati
        credenzialiAccesso = -2147217843

        'formatoDb :            formato di database non riconosciuto
        formatoDb = -2147467259

        'colonneErrate :        colonne errate
        colonneErrate = -2147217865

        'colonneErrate 2 :      interpreta le colonne inesistenti come parametri
        parametriInsufficienti = -2147217904
    End Enum


    'Public Shared ReadOnly ConnectionStringFlussiRegione As String = ConfigurationManager.ConnectionStrings("OLEDBConnFlussiRegione").ConnectionString
    Private Shared ConnectionStringFlussiRegione As String
    Public Shared ReadOnly Property ConnectionString(ByVal nomeFile As String) As String
        Get
            ConnectionStringFlussiRegione = ConfigurationManager.AppSettings("OLEDBConnFlussiRegione")
            Dim userACCESS As String = "intema"
            Dim pswACCESS As String = "tema5018"
            ConnectionStringFlussiRegione = ConnectionStringFlussiRegione.Replace("@nomeFile", nomeFile)
            ConnectionStringFlussiRegione = ConnectionStringFlussiRegione.Replace("@userACCESS", userACCESS)
            ConnectionStringFlussiRegione = ConnectionStringFlussiRegione.Replace("@pswACCESS", pswACCESS)
            Return ConnectionStringFlussiRegione
        End Get
    End Property
    ' Hashtable to store cached parameters
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())
    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As OleDbParameter()) As OleDbDataReader
        Dim cmd As New OleDbCommand()
        Dim conn As New OleDbConnection(connectionString)
        '// we use a try/catch here because if the method throws an exception we want to 
        '// close the connection throw code, because no datareader will exist, hence the 
        '// commandBehaviour.CloseConnection will not work
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            'Dim rdr As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Dim rdr As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Sub PrepareCommand(ByVal cmd As OleDbCommand, ByVal conn As OleDbConnection, ByVal trans As OleDbTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As OleDbParameter())
        If (conn.State <> ConnectionState.Open) Then
            conn.Open()
        End If
        cmd.Connection = conn
        cmd.CommandText = cmdText
        If (Not trans Is Nothing) Then
            cmd.Transaction = trans
        End If
        cmd.CommandType = cmdType
        If (Not cmdParms Is Nothing) Then
            Dim parm As OleDbParameter
            For Each parm In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub
End Class
