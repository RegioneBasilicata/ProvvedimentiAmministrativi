Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility

Public Class RuoloInfo

    Private _codice As String = ""
    Private _descrizione As String = ""
    Private _origine As String = ""
    Property Codice() As String

        Get
            Return _codice
        End Get
        Set(ByVal Value As String)
            _codice = Value
        End Set
    End Property

    Property Descrizione() As String

        Get
            Return _descrizione
        End Get
        Set(ByVal Value As String)
            _descrizione = Value
        End Set
    End Property


    Property Origine() As String

        Get
            Return _origine
        End Get
        Set(ByVal Value As String)
            _origine = Value
        End Set
    End Property
    Public Sub New()

    End Sub
    Public Sub New(ByVal codice As String, ByVal descrizione As String, ByVal origine As String)
        _codice = codice
        _descrizione = descrizione
        _origine = origine
    End Sub
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return DirectCast(obj, RuoloInfo).Codice = Codice
    End Function
End Class

