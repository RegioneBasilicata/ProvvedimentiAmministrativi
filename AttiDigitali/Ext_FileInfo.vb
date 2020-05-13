
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_FileInfo
    Private _id As String = String.Empty
    Private _descrizione As String = String.Empty
    Private _link As String = String.Empty
    Private _tipologia As String = String.Empty
    Private _ultimaVersione As Boolean = True
    <System.Runtime.Serialization.DataMember()> _
 Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Link() As String
        Get
            Return _link
        End Get
        Set(ByVal value As String)
            _link = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property UltimaVersione() As Boolean
        Get
            Return _ultimaVersione
        End Get
        Set(ByVal value As Boolean)
            _ultimaVersione = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
