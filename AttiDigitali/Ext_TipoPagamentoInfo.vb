
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_TipoPagamentoInfo
    Private _Id As Integer
    Private _Descrizione As String
    Private _ObbligoCC As Boolean = False
    Private _OrdineApparizione As Integer
    Private _Preferiti As Boolean = False
    Private _ObbligoIBAN As Boolean = False
    <System.Runtime.Serialization.DataMember()> _
 Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Descrizione() As String
        Get
            Return _Descrizione
        End Get
        Set(ByVal value As String)
            _Descrizione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ObbligoCC() As Boolean
        Get
            Return _ObbligoCC
        End Get
        Set(ByVal value As Boolean)
            _ObbligoCC = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Preferiti() As Boolean
        Get
            Return _Preferiti
        End Get
        Set(ByVal value As Boolean)
            _Preferiti = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property OrdineApparizione() As Integer
        Get
            Return _OrdineApparizione
        End Get
        Set(ByVal value As Integer)
            _OrdineApparizione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Property ObbligoIBAN() As Boolean
        Get
            Return _ObbligoIBAN
        End Get
        Set(ByVal value As Boolean)
            _ObbligoIBAN = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
