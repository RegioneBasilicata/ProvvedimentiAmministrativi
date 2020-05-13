<System.Runtime.Serialization.DataContract()> _
Public Class Ext_ContrattoInfoHeader

    Private _id As String = String.Empty
    Private _numeroRepertorio As String = String.Empty
    Private _fatture As Generic.List(Of Ext_FatturaInfo)
    
    <System.Runtime.Serialization.DataMember()> _
    Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property NumeroRepertorio() As String
        Get
            Return _numeroRepertorio
        End Get
        Set(ByVal value As String)
            _numeroRepertorio = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
     Public Property Fatture() As Generic.List(Of Ext_FatturaInfo)
        Get
            Return _fatture
        End Get
        Set(ByVal value As Generic.List(Of Ext_FatturaInfo))
            _fatture = value
        End Set
    End Property

    Public Sub New()
    End Sub
End Class
