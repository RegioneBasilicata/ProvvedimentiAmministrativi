<System.Runtime.Serialization.DataContract()> _
Public Class Ext_MandatoAttoInfo

    Private _idDocumento As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    Private _numeroMandato As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroMandato() As String
        Get
            Return _numeroMandato
        End Get
        Set(ByVal value As String)
            _numeroMandato = value
        End Set
    End Property

    Private _dataEmissioneMandato As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property DataEmissioneMandato() As String
        Get
            Return _dataEmissioneMandato
        End Get
        Set(ByVal value As String)
            _dataEmissioneMandato = value
        End Set
    End Property

    Private _validoAnnullato As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property ValidoAnnullato() As String
        Get
            Return _validoAnnullato
        End Get
        Set(ByVal value As String)
            _validoAnnullato = value
        End Set
    End Property

    Private _numeroStorno As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroStorno() As Long
        Get
            Return _numeroStorno
        End Get
        Set(ByVal value As Long)
            _numeroStorno = value
        End Set
    End Property

    Private _dataStorno As String
    <System.Runtime.Serialization.DataMember()> _
    Property DataStorno() As String
        Get
            Return _dataStorno
        End Get
        Set(ByVal value As String)
            _dataStorno = value
        End Set
    End Property

    Private _importoTotaleMandato As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImportoTotaleMandato() As Double
        Get
            Return _importoTotaleMandato
        End Get
        Set(ByVal value As Double)
            _importoTotaleMandato = value
        End Set
    End Property

    Private _oggettoMandato As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property OggettoMandato() As String
        Get
            Return _oggettoMandato
        End Get
        Set(ByVal value As String)
            _oggettoMandato = value
        End Set
    End Property

    Private _numeroLiquidazione As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroLiquidazione() As Long
        Get
            Return _numeroLiquidazione
        End Get
        Set(ByVal value As Long)
            _numeroLiquidazione = value
        End Set
    End Property

    Private _numeroImpegno As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroImpegno() As Long
        Get
            Return _numeroImpegno
        End Get
        Set(ByVal value As Long)
            _numeroImpegno = value
        End Set
    End Property

    Private _numeroImpegnoRifPerEnte As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroImpegnoRifPerEnte() As Long
        Get
            Return _numeroImpegnoRifPerEnte
        End Get
        Set(ByVal value As Long)
            _numeroImpegnoRifPerEnte = value
        End Set
    End Property

    Private _importoImpegno As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImportoImpegno() As Double
        Get
            Return _importoImpegno
        End Get
        Set(ByVal value As Double)
            _importoImpegno = value
        End Set
    End Property

    Private _tipoImpegno As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property TipoImpegno() As String
        Get
            Return _tipoImpegno
        End Get
        Set(ByVal value As String)
            _tipoImpegno = value
        End Set
    End Property

    Private _esercizioMandato As Integer
    <System.Runtime.Serialization.DataMember()> _
    Property EsercizioMandato() As Integer
        Get
            Return _esercizioMandato
        End Get
        Set(ByVal value As Integer)
            _esercizioMandato = value
        End Set
    End Property

    Private _esercizioImpegno As Integer
    <System.Runtime.Serialization.DataMember()> _
    Property EsercizioImpegno() As Integer
        Get
            Return _esercizioImpegno
        End Get
        Set(ByVal value As Integer)
            _esercizioImpegno = value
        End Set
    End Property

    Private _esercizioImpegnoRifPerEnte As Integer
    <System.Runtime.Serialization.DataMember()> _
    Property EsercizioImpegnoRifPerEnte() As Integer
        Get
            Return _esercizioImpegnoRifPerEnte
        End Get
        Set(ByVal value As Integer)
            _esercizioImpegnoRifPerEnte = value
        End Set
    End Property

    Private _dipartimento As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property Dipartimento() As String
        Get
            Return _dipartimento
        End Get
        Set(ByVal value As String)
            _dipartimento = value
        End Set
    End Property

    Private _tipoAttoLiq As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property TipoAttoLiq() As String
        Get
            Return _tipoAttoLiq
        End Get
        Set(ByVal value As String)
            _tipoAttoLiq = value
        End Set
    End Property

    Private _numeroAttoLiq As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroAttoLiq() As String
        Get
            Return _numeroAttoLiq
        End Get
        Set(ByVal value As String)
            _numeroAttoLiq = value
        End Set
    End Property

    Private _dataAttoLiq As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property DataAttoLiq() As String
        Get
            Return _dataAttoLiq
        End Get
        Set(ByVal value As String)
            _dataAttoLiq = value
        End Set
    End Property

    Private _tipoAttoImp As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property TipoAttoImp() As String
        Get
            Return _tipoAttoImp
        End Get
        Set(ByVal value As String)
            _tipoAttoImp = value
        End Set
    End Property

    Private _numeroAttoImp As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroAttoImp() As String
        Get
            Return _numeroAttoImp
        End Get
        Set(ByVal value As String)
            _numeroAttoImp = value
        End Set
    End Property

    Private _dataAttoImp As String
    <System.Runtime.Serialization.DataMember()> _
    Property DataAttoImp() As String
        Get
            Return _dataAttoImp
        End Get
        Set(ByVal value As String)
            _dataAttoImp = value
        End Set
    End Property

    Private _numeroDistinta As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroDistinta() As Long
        Get
            Return _numeroDistinta
        End Get
        Set(ByVal value As Long)
            _numeroDistinta = value
        End Set
    End Property

    Private _incarico As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property Incarico() As String
        Get
            Return _incarico
        End Get
        Set(ByVal value As String)
            _incarico = value
        End Set
    End Property

    Private _dataIncarico As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property DataIncarico() As String
        Get
            Return _dataIncarico
        End Get
        Set(ByVal value As String)
            _dataIncarico = value
        End Set
    End Property

    Private _dataQuietanza As String
    <System.Runtime.Serialization.DataMember()> _
    Property DataQuietanza() As String
        Get
            Return _dataQuietanza
        End Get
        Set(ByVal value As String)
            _dataQuietanza = value
        End Set
    End Property

    Private _numeroQuietanza As Long
    <System.Runtime.Serialization.DataMember()> _
    Property NumeroQuietanza() As Long
        Get
            Return _numeroQuietanza
        End Get
        Set(ByVal value As Long)
            _numeroQuietanza = value
        End Set
    End Property

    Private _beneficiari As Generic.List(Of Ext_MandatoBeneficiarioInfo)
    <System.Runtime.Serialization.DataMember()> _
    Property ListaBeneficiari() As Generic.List(Of Ext_MandatoBeneficiarioInfo)
        Get
            Return _beneficiari
        End Get
        Set(ByVal value As Generic.List(Of Ext_MandatoBeneficiarioInfo))
            _beneficiari = value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class
