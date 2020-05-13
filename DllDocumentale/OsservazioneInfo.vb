Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility
Namespace Model
    Public Class OsservazioneInfo

        Private _IdDocumento As String
        Private _Progressivo As Integer = 0
        Private _Testo As String = ""
        Private _Tipologia As String = ""
        Private _DataRegistrazione As DateTime
        Private _Operatore As String = ""

        Property IdDocumento() As String
            Get
                Return _IdDocumento
            End Get
            Set(ByVal Value As String)
                _IdDocumento = Value
            End Set
        End Property
        Property Progressivo() As Integer
            Get
                Return _Progressivo
            End Get
            Set(ByVal Value As Integer)
                _Progressivo = Value
            End Set
        End Property
        Property Testo() As String
            Get
                Return _Testo
            End Get
            Set(ByVal Value As String)
                _Testo = Value
            End Set
        End Property
        Property Tipologia() As String
            Get
                Return _Tipologia
            End Get
            Set(ByVal Value As String)
                _Tipologia = Value
            End Set
        End Property
        Property DataRegistrazione() As DateTime
            Get
                Return _DataRegistrazione
            End Get
            Set(ByVal Value As DateTime)
                _DataRegistrazione = Value
            End Set
        End Property
        Property Operatore() As String
            Get
                Return _Operatore
            End Get
            Set(ByVal Value As String)
                _Operatore = Value
            End Set
        End Property
        Public Sub New()
        End Sub
    End Class
End Namespace
