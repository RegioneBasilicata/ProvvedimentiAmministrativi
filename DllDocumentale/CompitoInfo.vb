Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility
Namespace Model
    Public Class CompitoInfo

        Private _IdDocumento As String
        Private _Tipologia As String = ""
        Private _Descrizione As String = ""
        Private _Nominativo As String = ""
        Private _Data As DateTime
        Private _Operatore As String = ""
        Private _CertificatoUtente As String = ""
        Private _Specifico As Boolean
        Private _UltimaAzione As String = ""

        Property IdDocumento() As String
            Get
                Return _IdDocumento
            End Get
            Set(ByVal Value As String)
                _IdDocumento = Value
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
        Property Descrizione() As String
            Get
                Return _Descrizione
            End Get
            Set(ByVal Value As String)
                _Descrizione = Value
            End Set
        End Property
        Property Nominativo() As String
            Get
                Return _Nominativo
            End Get
            Set(ByVal Value As String)
                _Nominativo = Value
            End Set
        End Property
        Property Data() As DateTime
            Get
                Return _Data
            End Get
            Set(ByVal Value As DateTime)
                _Data = Value
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
        Property Specifico() As Boolean
            Get
                Return _Specifico
            End Get
            Set(ByVal Value As Boolean)
                _Specifico = Value
            End Set
        End Property
        Property CertificatoUtente() As String
            Get
                Return _CertificatoUtente
            End Get
            Set(ByVal Value As String)
                _CertificatoUtente = Value
            End Set
        End Property
        Property UltimaAzione() As String
            Get
                Return _UltimaAzione
            End Get
            Set(ByVal Value As String)
                _UltimaAzione = Value
            End Set
        End Property

        Public Sub New()
        End Sub
    End Class
End Namespace
