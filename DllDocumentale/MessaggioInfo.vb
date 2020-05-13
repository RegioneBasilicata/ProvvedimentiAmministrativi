Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility
Namespace Model
    Public Class MessaggioInfo
        Private _id As String = String.Empty
        Private _img As String = String.Empty
        Private _tipologia As String = String.Empty
        Private _IdDocumento As String = String.Empty
        Private _mittente As String = String.Empty
        Private _destinatario As String = String.Empty
        Private _letto As String = String.Empty
        Private _testo As String = String.Empty
        Private _data As String = String.Empty
        Property Id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Property Img() As String
            Get
                Return _img
            End Get
            Set(ByVal value As String)
                _img = value
            End Set
        End Property
        Property Tipologia() As String
            Get
                Return _tipologia
            End Get
            Set(ByVal value As String)
                _tipologia = value
            End Set
        End Property
        Property IdDocumento() As String
            Get
                Return _IdDocumento
            End Get
            Set(ByVal value As String)
                _IdDocumento = value
            End Set
        End Property
        Property Mittente() As String
            Get
                Return _mittente
            End Get
            Set(ByVal value As String)
                _mittente = value
            End Set
        End Property
        Property Destinatario() As String
            Get
                Return _destinatario
            End Get
            Set(ByVal value As String)
                _destinatario = value
            End Set
        End Property

        Property Letto() As Boolean
            Get
                Return _letto
            End Get
            Set(ByVal value As Boolean)
                _letto = value
            End Set
        End Property
        Property Testo() As String
            Get
                Return _testo
            End Get
            Set(ByVal value As String)
                _testo = value
            End Set
        End Property

        Property Data() As DateTime
            Get
                Return _data
            End Get
            Set(ByVal value As DateTime)
                _data = value
            End Set
        End Property
        Public Sub New()

        End Sub
    End Class
End Namespace
