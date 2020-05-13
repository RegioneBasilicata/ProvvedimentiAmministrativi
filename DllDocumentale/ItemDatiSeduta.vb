Public Class ItemDatiSeduta
    Private _idDocumento As String = String.Empty
    Private _idTipologiaProvvedimento As Integer = -1
    Private _isStraordinario As Boolean = False
    Private _tipologia As String = ""

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    Public Property IdTipologiaProvvedimento() As Integer
        Get
            Return _idTipologiaProvvedimento
        End Get
        Set(ByVal value As Integer)
            _idTipologiaProvvedimento = value
        End Set
    End Property

    Public Property IsStraordinario() As Boolean
        Get
            Return _isStraordinario
        End Get
        Set(ByVal value As Boolean)
            _isStraordinario = value
        End Set
    End Property

    Public Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property
End Class
