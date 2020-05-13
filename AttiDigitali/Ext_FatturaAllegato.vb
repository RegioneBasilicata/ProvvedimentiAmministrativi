
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_FatturaAllegato
    Private _prog As String = String.Empty
    Private _progFattura As String = String.Empty
    Private _nome As String = String.Empty
    Private _formato As String = String.Empty
    Private _url As String = String.Empty
    Private _idDocumento As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
 Property Prog() As String
        Get
            Return _prog
        End Get
        Set(ByVal value As String)
            _prog = value
        End Set
    End Property
    Property ProgFattura() As String
        Get
            Return _progFattura
        End Get
        Set(ByVal value As String)
            _progFattura = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Formato() As String
        Get
            Return _formato
        End Get
        Set(ByVal value As String)
            _formato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property
   
    Public Sub New()

    End Sub
End Class
