<System.Runtime.Serialization.DataContract()> _
Public Class Ext_FatturaInfoHeader
    Private _idUnivoco As String = String.Empty
    Private _idDocumento As String = String.Empty
    Private _prog As Long
    Private _numeroFatturaBeneficiario As String = String.Empty
    Private _dataFatturaBeneficiario As String = String.Empty
    Private _descrizioneFattura As String = String.Empty
    Private _importoTotaleFattura As Double = 0
    Private _importoLiquidato As Double = 0
    Private _importoResiduo As Double = 0
    Private _importoFattDaLiquidare As Double = 0
    Private _anagraficaInfo As Ext_AnagraficaInfo
    Private _contratto As Ext_ContrattoInfo

    Private _idLiquidazione As Integer = 0
    Private _idImpegno As Integer = 0
    Private _idProgFatturaInLiquidazione As Long

    Private _listaAllegati As Generic.List(Of Ext_FatturaAllegato)


    <System.Runtime.Serialization.DataMember()> _
     Public Property IdProgFatturaInLiquidazione() As Long
        Get
            Return _idProgFatturaInLiquidazione
        End Get
        Set(ByVal value As Long)
            _idProgFatturaInLiquidazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
     Public Property Prog() As Long
        Get
            Return _prog
        End Get
        Set(ByVal value As Long)
            _prog = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
     Public Property AnagraficaInfo() As Ext_AnagraficaInfo
        Get
            Return _anagraficaInfo
        End Get
        Set(ByVal value As Ext_AnagraficaInfo)
            _anagraficaInfo = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property Contratto() As Ext_ContrattoInfo
        Get
            Return _contratto
        End Get
        Set(ByVal value As Ext_ContrattoInfo)
            _contratto = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdUnivoco() As String
        Get
            Return _idUnivoco
        End Get
        Set(ByVal value As String)
            _idUnivoco = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdLiquidazione() As Integer
        Get
            Return _idLiquidazione
        End Get
        Set(ByVal value As Integer)
            _idLiquidazione = value
        End Set
    End Property


    <System.Runtime.Serialization.DataMember()> _
    Property IdImpegno() As Integer
        Get
            Return _idImpegno
        End Get
        Set(ByVal value As Integer)
            _idImpegno = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property NumeroFatturaBeneficiario() As String
        Get
            Return _numeroFatturaBeneficiario
        End Get
        Set(ByVal value As String)
            _numeroFatturaBeneficiario = value
        End Set
    End Property


    <System.Runtime.Serialization.DataMember()> _
    Property DataFatturaBeneficiario() As String
        Get
            Return _dataFatturaBeneficiario
        End Get
        Set(ByVal value As String)
            _dataFatturaBeneficiario = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property DescrizioneFattura() As String
        Get
            Return _descrizioneFattura
        End Get
        Set(ByVal value As String)
            _descrizioneFattura = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property ImportoTotaleFattura() As Double
        Get
            Return _importoTotaleFattura
        End Get
        Set(ByVal value As Double)
            _importoTotaleFattura = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property ImportoLiquidato() As Double
        Get
            Return _importoLiquidato
        End Get
        Set(ByVal value As Double)
            _importoLiquidato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Public Property ImportoFattDaLiquidare() As Double
        Get
            Return _importoFattDaLiquidare
        End Get
        Set(ByVal value As Double)
            _importoFattDaLiquidare = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property ImportoResiduo() As Double
        Get
            Return _importoResiduo
        End Get
        Set(ByVal value As Double)
            _importoResiduo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property ListaAllegati() As Generic.List(Of Ext_FatturaAllegato)
        Get
            Return _listaAllegati
        End Get
        Set(ByVal value As Generic.List(Of Ext_FatturaAllegato))
            _listaAllegati = value
        End Set
    End Property

    Public Sub New()
    End Sub

End Class
