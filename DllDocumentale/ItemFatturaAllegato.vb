Public Class ItemFatturaAllegato
    Private _prog As Long
    Private _progFattura As Long
    Private _nome As String = String.Empty
    Private _formato As String = String.Empty
    Private _url As String = String.Empty
    Private _idDocumento As String = String.Empty
    Private _numeroFatturaBeneficiario As String = String.Empty
    Private _dataFatturaBeneficiario As Date
    Private _denominazioneBeneficiario As String

    Property Prog() As Long
        Get
            Return _prog
        End Get
        Set(ByVal value As Long)
            _prog = value
        End Set
    End Property
    Public Property ProgFattura() As Long
        Get
            Return _progFattura
        End Get
        Set(ByVal value As Long)
            _progFattura = value
        End Set
    End Property
    Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Property Formato() As String
        Get
            Return _formato
        End Get
        Set(ByVal value As String)
            _formato = value
        End Set
    End Property
    Property Url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property
    Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    Property NumeroFatturaBeneficiario() As String
        Get
            Return _numeroFatturaBeneficiario
        End Get
        Set(ByVal value As String)
            _numeroFatturaBeneficiario = value
        End Set
    End Property
    Property DataFatturaBeneficiario() As Date
        Get
            Return _dataFatturaBeneficiario
        End Get
        Set(ByVal value As Date)
            _dataFatturaBeneficiario = value
        End Set
    End Property
    Property DenominazioneBeneficiario() As String
        Get
            Return _denominazioneBeneficiario
        End Get
        Set(ByVal value As String)
            _denominazioneBeneficiario = value
        End Set
    End Property


End Class
