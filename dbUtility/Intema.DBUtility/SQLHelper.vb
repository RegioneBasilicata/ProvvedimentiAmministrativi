Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Specialized

Public MustInherit Class SqlHelper

    Private Shared _ConnectionString As Collection

    Shared Sub New()
        _ConnectionString = New Collection


        For Each a As String In ConfigurationManager.AppSettings()
            Dim key As String = ""
            If a.StartsWith("ConnectionString") Then
                key = a.Replace("ConnectionString", "")
                _ConnectionString.Add(ConfigurationManager.AppSettings(a), key)
            End If



        Next
    End Sub

    ''' <summary>
    ''' Restituisce la connection string
    ''' </summary>
    ''' <param name="Nome">Nome della connessione nella sezione .config </param>
    ''' <param name="timeout">Timeout della connessione da impostare nella connection string</param>
    ''' <value></value>
    ''' <returns>Stringa di connessione</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ConnectionString(ByVal Nome As String, Optional ByVal timeout As Integer = -1) As String
        Get

            'Dim connStrBuilder As New SqlConnectionStringBuilder
            'connStrBuilder.ConnectionString = _ConnectionString.Item(Nome).ToString
            'If timeout <> -1 Then
            'connStrBuilder.ConnectTimeout = timeout
            'End If
            Return _ConnectionString.Item(Nome).ToString
        End Get
    End Property
    ' Hashtable to store cached parameters
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    ''' <summary>
    ''' Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
    ''' <param name="cmdType ">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText ">the stored procedure name or T-SQL command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <returns>an int representing the number of rows affected by the command</returns>
    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Integer
        Dim cmd As New SqlCommand()
        Dim conn As New SqlConnection(connectionString)

            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            If timeout <> -1 Then
                cmd.CommandTimeout = timeout
            End If

            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()

            conn.Close()
        conn = Nothing
        GC.Collect()
            Return val

    End Function

    ''' <summary>
    '''  Execute a SqlCommand (that returns no resultset) against an existing database connection 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="connection ">an existing database connection</param>
    ''' <param name="cmdType ">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText ">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">timeout della comando</param>
    ''' <returns>an int representing the number of rows affected by the command</returns>
    Public Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Integer
        Dim cmd As New SqlCommand()

        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        If timeout <> -1 Then
            cmd.CommandTimeout = timeout
        End If

        Dim val As Integer = cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        Return val
    End Function

    ''' <summary>
    ''' Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="trans">an existing sql transaction</param>
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <returns>an int representing the number of rows affected by the command</returns>
    Public Shared Function ExecuteNonQuery(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Integer
        Dim cmd As New SqlCommand()
        Dim val As Integer = 0
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        If timeout <> -1 Then
            cmd.CommandTimeout = timeout
        End If

        val = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    ''' <summary>
    ''' Execute a SqlCommand that returns a resultset against the database specified in the connection string 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
    ''' <param name="cmdType ">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText ">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <returns>A SqlDataReader containing the results</returns>
    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As SqlDataReader
        Dim cmd As New SqlCommand()
        Dim conn As New SqlConnection(connectionString)

        '// we use a try/catch here because if the method throws an exception we want to 
        '// close the connection throw code, because no datareader will exist, hence the 
        '// commandBehaviour.CloseConnection will not work
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            If timeout <> -1 Then
                cmd.CommandTimeout = timeout
            End If

            Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch ex As Exception
            conn.Close()
            Throw ex
        End Try
    End Function

    Public Shared Function ExecuteReader(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As SqlDataReader
        Dim cmd As New SqlCommand()
        Dim conn As SqlConnection = trans.Connection

        '// we use a try/catch here because if the method throws an exception we want to 
        '// close the connection throw code, because no datareader will exist, hence the 
        '// commandBehaviour.CloseConnection will not work
        Try
            PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters)
            If timeout <> -1 Then
                cmd.CommandTimeout = timeout
            End If

            Dim rdr As SqlDataReader = cmd.ExecuteReader()
            cmd.Parameters.Clear()
            Return rdr
        Catch ex As Exception
            conn.Close()
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Object
        Dim cmd As New SqlCommand()

        Dim connection As New SqlConnection(connectionString)

            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            If timeout <> -1 Then
                cmd.CommandTimeout = timeout
            End If
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
        connection.Close()
        connection = Nothing
        GC.Collect()
            Return val

    End Function

    ''' <summary>
    ''' Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="trans">a valid Transaction</param>
    ''' <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    Public Shared Function ExecuteScalar(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Object
        Dim cmd As New SqlCommand()

        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        If timeout <> -1 Then
            cmd.CommandTimeout = timeout
        End If

        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()

        Return val
    End Function


    ''' <summary>
    ''' Execute a SqlCommand that returns the first column of the first record against an existing database connection 
    ''' using the provided parameters.
    ''' </summary>
    ''' <remarks>
    ''' e.g.:  
    '''  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    ''' </remarks>
    ''' <param name="connection ">an existing database connection</param>
    ''' <param name="cmdType ">the CommandType (stored procedure, text, etc.)</param>
    ''' <param name="cmdText ">the stored procedure name or T-SQL command</param>
    ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    ''' <param name="timeout">Timeout del comando</param>
    ''' <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    Public Shared Function ExecuteScalar(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal commandParameters As SqlParameter(), Optional ByVal timeout As Integer = -1) As Object
        Dim cmd As New SqlCommand()

        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        If timeout <> -1 Then
            cmd.CommandTimeout = timeout
        End If

        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()
        Return val
    End Function


    ''' <summary>
    ''' Retrieve cached parameters
    ''' </summary>
    ''' <param name="cacheKey">key used to lookup parameters</param>
    ''' <returns>Cached SqlParamters array</returns>
    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As SqlParameter()
        Dim cachedParms As SqlParameter() = DirectCast(parmCache(cacheKey), SqlParameter())

        If (cachedParms Is Nothing) Then
            Return Nothing
        End If

        Dim clonedParms As SqlParameter()
        clonedParms = New SqlParameter(cachedParms.Length) {}

        Dim i As Integer
        Dim j As Integer = cachedParms.Length
        For i = 0 To j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), SqlParameter)
        Next

        Return clonedParms
    End Function

    ''' <summary>
    ''' Prepare a command for execution
    ''' </summary>
    ''' <param name="cmd">SqlCommand object</param>
    ''' <param name="conn">SqlConnection object</param>
    ''' <param name="trans">SqlTransaction object</param>
    ''' <param name="cmdType">Cmd type e.g. stored procedure or text</param>
    ''' <param name="cmdText">Command text, e.g. Select * from Products</param>
    ''' <param name="cmdParms">SqlParameters to use in the command</param>
    Private Shared Sub PrepareCommand(ByVal cmd As SqlCommand, ByVal conn As SqlConnection, ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As SqlParameter())

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
            Dim parm As SqlParameter
            For Each parm In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub

    ''' <summary>
    ''' sostituisce a nothing dbnull.value
    ''' </summary>
    ''' <param name="data">dato da verificare</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DbValue(ByVal data As Object) As Object
        Dim dataTmp As DateTime
        If data Is Nothing Then
            Return DBNull.Value
        Else

        End If
        If System.Type.GetTypeCode(data.GetType) = System.TypeCode.DateTime Then
            dataTmp = CDate(data)
            If dataTmp = Nothing Then
                Return DBNull.Value
            Else
                Return dataTmp
            End If
        Else
            If IsNothing(data) Then
                Return DBNull.Value
            Else
                Return data
            End If
        End If
    End Function



End Class