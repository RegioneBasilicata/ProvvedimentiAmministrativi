<System.Runtime.Serialization.DataContract()> _
Public Class Ext_PCFInfo
    Private _id As String
    Private _descrizione As String

    <System.Runtime.Serialization.DataMember()> _
    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property Descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
End Class
