Public Class ItemDatiSedutaInfo
    Private _docId As String = String.Empty
    Private _dataSeduta As String = String.Empty
    Private _oraSeduta As String = String.Empty
    Private _relatori As Generic.List(Of ItemRelatore) = New Generic.List(Of ItemRelatore)

    Public Property DocId() As String
        Get
            Return _docId
        End Get
        Set(ByVal value As String)
            _docId = value
        End Set
    End Property

    Public Property DataSeduta() As String
        Get
            Return _dataSeduta
        End Get
        Set(ByVal value As String)
            _dataSeduta = value
        End Set
    End Property

    Public Property OraSeduta() As String
        Get
            Return _oraSeduta
        End Get
        Set(ByVal value As String)
            _oraSeduta = value
        End Set
    End Property

    Property Relatori() As Generic.List(Of ItemRelatore)
        Get
            Return _relatori
        End Get
        Set(ByVal Value As Generic.List(Of ItemRelatore))
            _relatori = Value
        End Set
    End Property

End Class
