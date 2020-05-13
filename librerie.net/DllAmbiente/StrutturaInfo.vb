Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility



Public Class StrutturaInfo
    Implements IEquatable(Of StrutturaInfo)

    Private _codiceInterno As String = ""
    Private _codicePubblico As String = ""
    Private _descrizione As String = ""
    Private _descrizioneBreve As String = ""
    Private _padre As String = ""
    Private _tipologia As String = ""

    Property CodiceInterno() As String
        Get
            Return _codiceInterno
        End Get
        Set(ByVal Value As String)
            _codiceInterno = Value
        End Set
    End Property
    Property CodicePubblico() As String
        Get
            Return _codicePubblico
        End Get
        Set(ByVal Value As String)
            _codicePubblico = Value
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
    Property DescrizioneBreve() As String
        Get
            Return _descrizioneBreve
        End Get
        Set(ByVal Value As String)
            _descrizioneBreve = Value
        End Set
    End Property
    Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal Value As String)
            _tipologia = Value
        End Set
    End Property
    Property Padre() As String
        Get
            Return _padre
        End Get
        Set(ByVal Value As String)
            _padre = Value
        End Set
    End Property
    Public Sub New()

    End Sub
    Public Sub New(ByVal codiceinterno As String, ByVal codicePubblico As String, ByVal descrizione As String, ByVal descrizioneBreve As String, ByVal tipologia As String)
        _codiceInterno = codiceinterno
        _codicePubblico = codicePubblico
        _descrizione = descrizione
        _descrizioneBreve = descrizioneBreve
        _tipologia = tipologia
    End Sub
    Overloads Function Equals(ByVal obj As StrutturaInfo) As Boolean Implements IEquatable(Of DllAmbiente.StrutturaInfo).Equals
        Return obj.CodiceInterno = Me.CodiceInterno
    End Function

End Class

