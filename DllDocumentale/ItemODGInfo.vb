Public Class ItemODGInfo
    Private _dataSeduta As String = String.Empty
    Private _oraSeduta As String = String.Empty
    Private _idStatoSeduta As Integer
    Private _delibere As Generic.List(Of ItemDocumentoInfo) = New Generic.List(Of ItemDocumentoInfo)

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

    Public Property IdStatoSeduta() As String
        Get
            Return _idStatoSeduta
        End Get
        Set(ByVal value As String)
            _idStatoSeduta = value
        End Set
    End Property

    Property Delibere() As Generic.List(Of ItemDocumentoInfo)
        Get
            Return _delibere
        End Get
        Set(ByVal Value As Generic.List(Of ItemDocumentoInfo))
            _delibere = Value
        End Set
    End Property

End Class
