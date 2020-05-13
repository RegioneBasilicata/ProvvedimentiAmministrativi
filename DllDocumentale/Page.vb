<System.Runtime.Serialization.DataContract()> _
Public Class Page(Of t)

    Private _data As Collections.Generic.IList(Of t) = New Collections.Generic.List(Of t)
    Private _totalCount As Integer = 0

    <System.Runtime.Serialization.DataMember()> _
    Public Property Data() As Collections.Generic.IList(Of t)
        Get
            Return _data
        End Get
        Set(ByVal value As Collections.Generic.IList(Of t))
            _data = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property TotalCount() As Integer
        Get
            Return _totalCount
        End Get
        Set(ByVal value As Integer)
            _totalCount = value
        End Set
    End Property
End Class
