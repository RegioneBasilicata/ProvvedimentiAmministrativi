<System.Runtime.Serialization.DataContract()> _
Public Class Ext_MandatoBeneficiarioInfo

    Private _partitaIvaFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property PartitaIvaFornitore() As String
        Get
            Return _partitaIvaFornitore
        End Get
        Set(ByVal value As String)
            _partitaIvaFornitore = value
        End Set
    End Property

    Private _rappresentanteLegale As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property RappresentanteLegale() As String
        Get
            Return _rappresentanteLegale
        End Get
        Set(ByVal value As String)
            _rappresentanteLegale = value
        End Set
    End Property

    Private _codiceFiscaleRappresentanteLegale As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceFiscaleRappresentanteLegale() As String
        Get
            Return _codiceFiscaleRappresentanteLegale
        End Get
        Set(ByVal value As String)
            _codiceFiscaleRappresentanteLegale = value
        End Set
    End Property

    Private _dataNascitaRappresentanteLegale As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property DataNascitaRappresentanteLegale() As String
        Get
            Return _dataNascitaRappresentanteLegale
        End Get
        Set(ByVal value As String)
            _dataNascitaRappresentanteLegale = value
        End Set
    End Property

    Private _indirizzoFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property IndirizzoFornitore() As String
        Get
            Return _indirizzoFornitore
        End Get
        Set(ByVal value As String)
            _indirizzoFornitore = value
        End Set
    End Property

    Private _cittaFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CittaFornitore() As String
        Get
            Return _cittaFornitore
        End Get
        Set(ByVal value As String)
            _cittaFornitore = value
        End Set
    End Property

    Private _provinciaFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property ProvinciaFornitore() As String
        Get
            Return _provinciaFornitore
        End Get
        Set(ByVal value As String)
            _provinciaFornitore = value
        End Set
    End Property

    Private _capFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CapFornitore() As String
        Get
            Return _capFornitore
        End Get
        Set(ByVal value As String)
            _capFornitore = value
        End Set
    End Property

    Private _nazioneFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NazioneFornitore() As String
        Get
            Return _nazioneFornitore
        End Get
        Set(ByVal value As String)
            _nazioneFornitore = value
        End Set
    End Property

    Private _statoFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property StatoFornitore() As String
        Get
            Return _statoFornitore
        End Get
        Set(ByVal value As String)
            _statoFornitore = value
        End Set
    End Property

    Private _importoLordoBeneficiario As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImportoLordoBeneficiario() As Double
        Get
            Return _importoLordoBeneficiario
        End Get
        Set(ByVal value As Double)
            _importoLordoBeneficiario = value
        End Set
    End Property

    Private _importoNettoBeneficiario As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImportoNettoBeneficiario() As Double
        Get
            Return _importoNettoBeneficiario
        End Get
        Set(ByVal value As Double)
            _importoNettoBeneficiario = value
        End Set
    End Property

    Private _imponibileIrpef As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImponibileIrpef() As Double
        Get
            Return _imponibileIrpef
        End Get
        Set(ByVal value As Double)
            _imponibileIrpef = value
        End Set
    End Property

    Private _ritenutaIrpef As Double
    <System.Runtime.Serialization.DataMember()> _
    Property RitenutaIrpef() As Double
        Get
            Return _ritenutaIrpef
        End Get
        Set(ByVal value As Double)
            _ritenutaIrpef = value
        End Set
    End Property

    Private _addizionaleComunale As Double
    <System.Runtime.Serialization.DataMember()> _
    Property AddizionaleComunale() As Double
        Get
            Return _addizionaleComunale
        End Get
        Set(ByVal value As Double)
            _addizionaleComunale = value
        End Set
    End Property

    Private _addizionaleRegionale As Double
    <System.Runtime.Serialization.DataMember()> _
    Property AddizionaleRegionale() As Double
        Get
            Return _addizionaleRegionale
        End Get
        Set(ByVal value As Double)
            _addizionaleRegionale = value
        End Set
    End Property

    Private _imponibilePrevidenziale As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImponibilePrevidenziale() As Double
        Get
            Return _imponibilePrevidenziale
        End Get
        Set(ByVal value As Double)
            _imponibilePrevidenziale = value
        End Set
    End Property

    Private _ritenutePrevidenzialiEnte As Double
    <System.Runtime.Serialization.DataMember()> _
    Property RitenutePrevidenzialiEnte() As Double
        Get
            Return _ritenutePrevidenzialiEnte
        End Get
        Set(ByVal value As Double)
            _ritenutePrevidenzialiEnte = value
        End Set
    End Property

    Private _ritenutePrevidenzialiBeneficiario As Double
    <System.Runtime.Serialization.DataMember()> _
    Property RitenutePrevidenzialiBeneficiario() As Double
        Get
            Return _ritenutePrevidenzialiBeneficiario
        End Get
        Set(ByVal value As Double)
            _ritenutePrevidenzialiBeneficiario = value
        End Set
    End Property

    Private _altreRitenute As Double
    <System.Runtime.Serialization.DataMember()> _
    Property AltreRitenute() As Double
        Get
            Return _altreRitenute
        End Get
        Set(ByVal value As Double)
            _altreRitenute = value
        End Set
    End Property

    Private _imponibileIrap As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImponibileIrap() As Double
        Get
            Return _imponibileIrap
        End Get
        Set(ByVal value As Double)
            _imponibileIrap = value
        End Set
    End Property

    Private _impostaIrap As Double
    <System.Runtime.Serialization.DataMember()> _
    Property ImpostaIrap() As Double
        Get
            Return _impostaIrap
        End Get
        Set(ByVal value As Double)
            _impostaIrap = value
        End Set
    End Property

    Private _metodoPagamento As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property MetodoPagamento() As String
        Get
            Return _metodoPagamento
        End Get
        Set(ByVal value As String)
            _metodoPagamento = value
        End Set
    End Property

    Private _infoPagamento As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property InfoPagamento() As String
        Get
            Return _infoPagamento
        End Get
        Set(ByVal value As String)
            _infoPagamento = value
        End Set
    End Property

    Private _infoAggiuntive As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property InfoAggiuntive() As String
        Get
            Return _infoAggiuntive
        End Get
        Set(ByVal value As String)
            _infoAggiuntive = value
        End Set
    End Property

    Private _contoCorrente As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property ContoCorrente() As String
        Get
            Return _contoCorrente
        End Get
        Set(ByVal value As String)
            _contoCorrente = value
        End Set
    End Property

    Private _iBAN As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property IBAN() As String
        Get
            Return _iBAN
        End Get
        Set(ByVal value As String)
            _iBAN = value
        End Set
    End Property

    Private _aBI As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property ABI() As String
        Get
            Return _aBI
        End Get
        Set(ByVal value As String)
            _aBI = value
        End Set
    End Property

    Private _cAB As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CAB() As String
        Get
            Return _cAB
        End Get
        Set(ByVal value As String)
            _cAB = value
        End Set
    End Property

    Private _banca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property Banca() As String
        Get
            Return _banca
        End Get
        Set(ByVal value As String)
            _banca = value
        End Set
    End Property

    Private _indirizzoBanca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property IndirizzoBanca() As String
        Get
            Return _indirizzoBanca
        End Get
        Set(ByVal value As String)
            _indirizzoBanca = value
        End Set
    End Property

    Private _cittaBanca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CittaBanca() As String
        Get
            Return _cittaBanca
        End Get
        Set(ByVal value As String)
            _cittaBanca = value
        End Set
    End Property

    Private _nazioneBanca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NazioneBanca() As String
        Get
            Return _nazioneBanca
        End Get
        Set(ByVal value As String)
            _nazioneBanca = value
        End Set
    End Property

    Private _capBanca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CapBanca() As String
        Get
            Return _capBanca
        End Get
        Set(ByVal value As String)
            _capBanca = value
        End Set
    End Property

    Private _provinciaBanca As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property ProvinciaBanca() As String
        Get
            Return _provinciaBanca
        End Get
        Set(ByVal value As String)
            _provinciaBanca = value
        End Set
    End Property

    Private _descrizionePagamentoTesoriere As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property DescrizionePagamentoTesoriere() As String
        Get
            Return _descrizionePagamentoTesoriere
        End Get
        Set(ByVal value As String)
            _descrizionePagamentoTesoriere = value
        End Set
    End Property

    Private _codiceCup As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceCup() As String
        Get
            Return _codiceCup
        End Get
        Set(ByVal value As String)
            _codiceCup = value
        End Set
    End Property

    Private _codiceCig As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceCig() As String
        Get
            Return _codiceCig
        End Get
        Set(ByVal value As String)
            _codiceCig = value
        End Set
    End Property

    Private _nomeFornitore As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property NomeFornitore() As String
        Get
            Return _nomeFornitore
        End Get
        Set(ByVal value As String)
            _nomeFornitore = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
