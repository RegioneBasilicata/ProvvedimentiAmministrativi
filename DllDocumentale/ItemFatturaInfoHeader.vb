Public Class ItemFatturaInfoHeader
    Private _prog As Long
    Private _idUnivoco As String = String.Empty
    Private _idDocumento As String = String.Empty
    Private _numeroFatturaBeneficiario As String = String.Empty
    Private _dataFatturaBeneficiario As Date
    Private _descrizioneFattura As String = String.Empty
    Private _importoTotaleFattura As Double = 0

    Private _anagraficaInfo As ItemLiquidazioneImpegnoBeneficiarioInfo = New ItemLiquidazioneImpegnoBeneficiarioInfo
    Private _contratto As ItemContrattoInfo = New ItemContrattoInfo
    Private _importoLiquidato As Decimal
    Private _importoResiduo As Decimal
    Private _importoFattDaLiquidare As Decimal
    Private _idProgFatturaInLiquidazione As Long

    Private _listaAllegati As Generic.List(Of ItemFatturaAllegato)



    Property IdProgFatturaInLiquidazione() As Long
        Get
            Return _idProgFatturaInLiquidazione
        End Get
        Set(ByVal value As Long)
            _idProgFatturaInLiquidazione = value
        End Set
    End Property
    Property Prog() As Long
        Get
            Return _prog
        End Get
        Set(ByVal value As Long)
            _prog = value
        End Set
    End Property
    Property AnagraficaInfo() As ItemLiquidazioneImpegnoBeneficiarioInfo
        Get
            Return _anagraficaInfo
        End Get
        Set(ByVal value As ItemLiquidazioneImpegnoBeneficiarioInfo)
            _anagraficaInfo = value
        End Set
    End Property

    Property Contratto() As ItemContrattoInfo
        Get
            Return _contratto
        End Get
        Set(ByVal value As ItemContrattoInfo)
            _contratto = value
        End Set
    End Property


    Public Property IdUnivoco() As String
        Get
            Return _idUnivoco
        End Get
        Set(ByVal value As String)
            _idUnivoco = value
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

    Property DescrizioneFattura() As String
        Get
            Return _descrizioneFattura
        End Get
        Set(ByVal value As String)
            _descrizioneFattura = value
        End Set
    End Property

    Public Property ImportoTotaleFattura() As Double
        Get
            Return _importoTotaleFattura
        End Get
        Set(ByVal value As Double)
            _importoTotaleFattura = value
        End Set
    End Property

    Public Property ImportoLiquidato() As Decimal
        Get
            Return _importoLiquidato
        End Get
        Set(ByVal value As Decimal)
            _importoLiquidato = value
        End Set
    End Property
    Public Property ImportoResiduo() As Decimal
        Get
            Return _importoResiduo
        End Get
        Set(ByVal value As Decimal)
            _importoResiduo = value
        End Set
    End Property
    Public Property ImportoFattDaLiquidare() As Decimal
        Get
            Return _importoFattDaLiquidare
        End Get
        Set(ByVal value As Decimal)
            _importoFattDaLiquidare = value
        End Set
    End Property
    Public Property ListaAllegati() As Generic.List(Of ItemFatturaAllegato)
        Get
            Return _listaAllegati
        End Get
        Set(ByVal value As Generic.List(Of ItemFatturaAllegato))
            _listaAllegati = value
        End Set
    End Property

End Class
