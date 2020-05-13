
<System.Runtime.Serialization.DataContract()>
Public Class ItemLiquidazioneImpegnoBeneficiarioInfo
    Private _IdDocumento As String
    Private _ID As Long
    Private _IDDocumentoContabile As Long
    Private _NDcoumentoContabile As String
    Private _DataRegistrazione As DateTime
    Private _Operatore As String
    Private _IdAnagrafica As String
    Private _Denominazione As String
    Private _codiceFiscale As String
    Private _PartitaIva As String
    Private _flagPersonaFisica As Boolean
    Private _idSede As String
    Private _sedeVia As String
    Private _sedeComune As String
    Private _sedeProvincia As String
    Private _idModalitaPag As Integer
    Private _descrizioneModalitaPag As String
    Private _idConto As String
    Private _codiceSiope As String
    Private _codiceSiopeAggiuntivo As String
    Private _cig As String
    Private _cup As String
    Private _iban As String
    Private _importoSpettante As Decimal
    Private _importoPagato As Decimal
    Private _flagEsenzCommBonifico As Boolean
    Private _flagStampaAvviso As Boolean
    Private _flagBollo As Boolean
    Private _nMandato As String
    Private _RitenuteIrpef As Decimal
    Private _ImponibileIrpef As Decimal
    Private _RitenutePrevidenzialiBen As Decimal
    Private _AltreRitenute As Decimal
    Private _ImponibilePrevidenziale As Decimal
    Private _RitenutePrevidenzialiEnte As Decimal
    Private _AddizionaleComunale As Decimal
    Private _AddizionaleRegionale As Decimal
    Private _hasDatiBancari As Boolean
    Private _idContratto As String = String.Empty
    Private _numeroRepertorioContratto As String = String.Empty
    Private _isDatoSensibile As Boolean = False
    Private _listaFatture As Generic.List(Of ItemFatturaInfoHeader)
    Private _ProgFattura As Long
    Private _IsImpegno As Boolean



    <System.Runtime.Serialization.DataMember()>
    Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal Value As String)
            _IdDocumento = Value

        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ID() As Long
        Get
            Return _ID
        End Get
        Set(ByVal value As Long)
            _ID = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IDDocumentoContabile() As Long
        Get
            Return _IDDocumentoContabile
        End Get
        Set(ByVal value As Long)
            _IDDocumentoContabile = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property DataRegistrazione() As DateTime
        Get
            Return _DataRegistrazione
        End Get
        Set(ByVal value As DateTime)
            _DataRegistrazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Operatore() As String
        Get
            Return _Operatore
        End Get
        Set(ByVal value As String)
            _Operatore = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IdAnagrafica() As String
        Get
            Return _IdAnagrafica
        End Get
        Set(ByVal value As String)
            _IdAnagrafica = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Denominazione() As String
        Get
            Return _Denominazione
        End Get
        Set(ByVal value As String)
            _Denominazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property PartitaIva() As String
        Get
            Return _PartitaIva
        End Get
        Set(ByVal value As String)
            _PartitaIva = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property FlagPersonaFisica() As Boolean
        Get
            Return _flagPersonaFisica
        End Get
        Set(ByVal value As Boolean)
            _flagPersonaFisica = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property CodiceFiscale() As String
        Get
            Return _codiceFiscale
        End Get
        Set(ByVal value As String)
            _codiceFiscale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IdSede() As String
        Get
            Return _idSede
        End Get
        Set(ByVal value As String)
            _idSede = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property SedeVia() As String
        Get
            Return _sedeVia
        End Get
        Set(ByVal value As String)
            _sedeVia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property SedeComune() As String
        Get
            Return _sedeComune
        End Get
        Set(ByVal value As String)
            _sedeComune = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property SedeProvincia() As String
        Get
            Return _sedeProvincia
        End Get
        Set(ByVal value As String)
            _sedeProvincia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IdModalitaPag() As Integer
        Get
            Return _idModalitaPag
        End Get
        Set(ByVal value As Integer)
            _idModalitaPag = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property DescrizioneModalitaPag() As String
        Get
            Return _descrizioneModalitaPag
        End Get
        Set(ByVal value As String)
            _descrizioneModalitaPag = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property NDocumentoContabile() As String
        Get
            Return _NDcoumentoContabile
        End Get
        Set(ByVal value As String)
            _NDcoumentoContabile = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IdConto() As String
        Get
            Return _idConto
        End Get
        Set(ByVal value As String)
            _idConto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Iban() As String
        Get
            Return _iban
        End Get
        Set(ByVal value As String)
            _iban = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Cig() As String
        Get
            Return _cig
        End Get
        Set(ByVal value As String)
            _cig = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Cup() As String
        Get
            Return _cup
        End Get
        Set(ByVal value As String)
            _cup = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImportoPagato() As Decimal
        Get
            Return _importoPagato
        End Get
        Set(ByVal value As Decimal)
            _importoPagato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImportoSpettante() As Decimal
        Get
            Return _importoSpettante
        End Get
        Set(ByVal value As Decimal)
            _importoSpettante = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property NMandato() As String
        Get
            Return _nMandato
        End Get
        Set(ByVal value As String)
            _nMandato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property EsenzCommBonifico() As Boolean
        Get
            Return _flagEsenzCommBonifico
        End Get
        Set(ByVal value As Boolean)
            _flagEsenzCommBonifico = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property StampaAvviso() As Boolean
        Get
            Return _flagStampaAvviso
        End Get
        Set(ByVal value As Boolean)
            _flagStampaAvviso = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Bollo() As Boolean
        Get
            Return _flagBollo
        End Get
        Set(ByVal value As Boolean)
            _flagBollo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImponibileIrpef() As Decimal
        Get
            Return _ImponibileIrpef
        End Get
        Set(ByVal value As Decimal)
            _ImponibileIrpef = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property RitenuteIrpef() As Decimal
        Get
            Return _RitenuteIrpef
        End Get
        Set(ByVal value As Decimal)
            _RitenuteIrpef = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property RitenutePrevidenzialiBen() As Decimal
        Get
            Return _RitenutePrevidenzialiBen
        End Get
        Set(ByVal value As Decimal)
            _RitenutePrevidenzialiBen = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property AltreRitenute() As Decimal
        Get
            Return _AltreRitenute
        End Get
        Set(ByVal value As Decimal)
            _AltreRitenute = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImponibilePrevidenziale() As Decimal
        Get
            Return _ImponibilePrevidenziale
        End Get
        Set(ByVal value As Decimal)
            _ImponibilePrevidenziale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property RitenutePrevidenzialiEnte() As Decimal
        Get
            Return _RitenutePrevidenzialiEnte
        End Get
        Set(ByVal value As Decimal)
            _RitenutePrevidenzialiEnte = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property AddizionaleComunale() As Decimal
        Get
            Return _AddizionaleComunale
        End Get
        Set(ByVal value As Decimal)
            _AddizionaleComunale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property AddizionaleRegionale() As Decimal
        Get
            Return _AddizionaleRegionale
        End Get
        Set(ByVal value As Decimal)
            _AddizionaleRegionale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property CodiceSiope() As String
        Get
            Return _codiceSiope
        End Get
        Set(ByVal value As String)
            _codiceSiope = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property CodiceSiopeAggiuntivo() As String
        Get
            Return _codiceSiopeAggiuntivo
        End Get
        Set(ByVal value As String)
            _codiceSiopeAggiuntivo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property HasDatiBancari() As Boolean
        Get
            Return _hasDatiBancari
        End Get
        Set(ByVal value As Boolean)
            _hasDatiBancari = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IdContratto() As String
        Get
            Return _idContratto
        End Get
        Set(ByVal value As String)
            _idContratto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property NumeroRepertorioContratto() As String
        Get
            Return _numeroRepertorioContratto
        End Get
        Set(ByVal value As String)
            _numeroRepertorioContratto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IsDatoSensibile() As Boolean
        Get
            Return _isDatoSensibile
        End Get
        Set(ByVal value As Boolean)
            _isDatoSensibile = value
        End Set
    End Property

    Public Property ListaFatture() As Generic.List(Of ItemFatturaInfoHeader)
        Get
            Return _listaFatture
        End Get
        Set(ByVal value As Generic.List(Of ItemFatturaInfoHeader))
            _listaFatture = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ProgFatturaLiq() As Long
        Get
            Return _ProgFattura
        End Get
        Set(ByVal value As Long)
            _ProgFattura = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property IsImpegno() As Long
        Get
            Return _IsImpegno
        End Get
        Set(ByVal value As Long)
            _IsImpegno = value
        End Set
    End Property

End Class
