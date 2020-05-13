<System.Runtime.Serialization.DataContract()> _
Public Class Ext_CapitoliAnnoInfo

    '------------- RIGA ELENCO

    Private _Bilancio As String = String.Empty
    Private _UPB As String = String.Empty
    Private _MissioneProgramma As String = String.Empty
    Private _Capitolo As String = String.Empty
    Private _ImpDisp As String = String.Empty
    Private _ImpPrenotato As Double = 0
    Private _NumPrenotazione As String = String.Empty
    Private _AnnoPrenotazione As String = String.Empty
    Private _ID As String = String.Empty
    Private _contoEconomicaLista As ArrayList = New ArrayList
    Private _contoEconomica As String = String.Empty
    Private _ratei As String = String.Empty
    Private _risconti As String = String.Empty
    Private _impostaIrap As String = String.Empty
    Private _NumImpegno As String = String.Empty
    Private _DescrCapitolo As String = String.Empty
    Private _NumeroAtto As String = String.Empty
    Private _DataAtto As String = String.Empty
    Private _TipoAtto As String = String.Empty
    Private _isPerente As Boolean = False
    Private _RegistratoSic As Boolean = False
    Private _NumImpPrecedente As String = String.Empty
    Private _IsEconomia As String = String.Empty
    Private _Tipo As String = String.Empty
    Private _Stato As Integer = -1
    Private _StatoAsString As String = "Non Valido"
    Private _Codice_Obbiettivo_Gestionale As String = String.Empty
    Private _Oggetto_Impegno As String = String.Empty
    Private _ImpPotenzialePrenotato As Double = 0
    Private _PianoDeiContiFinanziari As String = String.Empty
    Private _CodiceRisposta As String = String.Empty
    Private _DescrizioneRisposta As String = String.Empty
    Private _TipoAssunzioneDescr As String = String.Empty
    Private _TipoAssunzione As Integer = -1
    Private _NumeroAssunzione As String = String.Empty
    Private _DataAssunzione As Date
    Private _listaBeneficiari As Generic.List(Of Ext_AnagraficaInfo)
    
    Private _HashTokenCallSic As String
    Private _IdDocContabileSic As String
    Private _HashTokenCallSic_Imp As String
    Private _IdDocContabileSIC_Imp As String

    'proprietà aggiunta agli impegni, solo per la vista, al front-end si fa arrivare un solo beneficiario
    Private _beneficiario As Ext_AnagraficaInfo



    <System.Runtime.Serialization.DataMember()> _
    Public Property DataAssunzione() As String
        Get
            Return _DataAssunzione
        End Get
        Set(ByVal value As String)
            _DataAssunzione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property NumeroAssunzione() As String
        Get
            Return _NumeroAssunzione
        End Get
        Set(ByVal value As String)
            _NumeroAssunzione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property TipoAssunzione() As Integer
        Get
            Return _TipoAssunzione
        End Get
        Set(ByVal value As Integer)
            _TipoAssunzione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Public Property TipoAssunzioneDescr() As String
        Get
            Return _TipoAssunzioneDescr
        End Get
        Set(ByVal value As String)
            _TipoAssunzioneDescr = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Public Property Bilancio() As String
        Get
            Return _Bilancio
        End Get
        Set(ByVal value As String)
            _Bilancio = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property UPB() As String
        Get
            Return _UPB
        End Get
        Set(ByVal value As String)
            _UPB = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property MissioneProgramma() As String
        Get
            Return _MissioneProgramma
        End Get
        Set(ByVal value As String)
            _MissioneProgramma = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property Capitolo() As String
        Get
            Return _Capitolo
        End Get
        Set(ByVal value As String)
            _Capitolo = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property ImpDisp() As String
        Get
            Return _ImpDisp
        End Get
        Set(ByVal value As String)
            _ImpDisp = value
        End Set
    End Property


    <System.Runtime.Serialization.DataMember()> _
    Public Property ImpPrenotato() As Double
        Get
            Return _ImpPrenotato
        End Get
        Set(ByVal value As Double)
            _ImpPrenotato = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property NumPreImp() As String
        Get
            Return _NumPrenotazione
        End Get
        Set(ByVal value As String)
            _NumPrenotazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property AnnoPrenotazione() As String
        Get
            Return _AnnoPrenotazione
        End Get
        Set(ByVal value As String)
            _AnnoPrenotazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property ContoEconomica() As String
        Get
            Return _contoEconomica
        End Get
        Set(ByVal value As String)
            _contoEconomica = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property ContoEconomicaLista() As ArrayList
        Get
            Return _contoEconomicaLista
        End Get
        Set(ByVal value As ArrayList)
            _contoEconomicaLista = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property Ratei() As String
        Get
            Return _ratei
        End Get
        Set(ByVal value As String)
            _ratei = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property Risconti() As String
        Get
            Return _risconti
        End Get
        Set(ByVal value As String)
            _risconti = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property ImpostaIrap() As String
        Get
            Return _impostaIrap
        End Get
        Set(ByVal value As String)
            _impostaIrap = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property NumImpegno() As String
        Get
            Return _NumImpegno
        End Get
        Set(ByVal value As String)
            _NumImpegno = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property DescrCapitolo() As String
        Get
            Return _DescrCapitolo
        End Get
        Set(ByVal value As String)
            _DescrCapitolo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property NumeroAtto() As String
        Get
            Return _NumeroAtto
        End Get
        Set(ByVal value As String)
            _NumeroAtto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property DataAtto() As String
        Get
            Return _DataAtto
        End Get
        Set(ByVal value As String)
            _DataAtto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property TipoAtto() As String
        Get
            Return _TipoAtto
        End Get
        Set(ByVal value As String)
            _TipoAtto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property isPerente() As Boolean
        Get
            Return _isPerente
        End Get
        Set(ByVal value As Boolean)
            _isPerente = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property RegistratoSic() As Boolean
        Get
            Return _RegistratoSic
        End Get
        Set(ByVal value As Boolean)
            _RegistratoSic = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property NumImpPrecedente() As String
        Get
            Return _NumImpPrecedente
        End Get
        Set(ByVal value As String)
            _NumImpPrecedente = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property IsEconomia() As String
        Get
            Return _IsEconomia
        End Get
        Set(ByVal value As String)
            _IsEconomia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Public Property Stato() As Integer
        Get
            Return _Stato
        End Get
        Set(ByVal value As Integer)
            _Stato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Public Property StatoAsString() As String
        Get
            Select Case _Stato
                Case 0
                    Return "Cancellato Logicamente"
                Case 1
                    Return "Valido"
                Case 2
                    Return "<b>Da Confermare</b>"
                Case Else
                    Return "Non Valido"
            End Select
        End Get
        Set(ByVal value As String)
            _StatoAsString = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Public Property Codice_Obbiettivo_Gestionale() As String
        Get
            Return _Codice_Obbiettivo_Gestionale
        End Get
        Set(ByVal value As String)
            _Codice_Obbiettivo_Gestionale = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
Public Property Oggetto_Impegno() As String
        Get
            Return _Oggetto_Impegno
        End Get
        Set(ByVal value As String)
            _Oggetto_Impegno = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Public Property ImpPotenzialePrenotato() As Double
        Get
            Return _ImpPotenzialePrenotato
        End Get
        Set(ByVal value As Double)
            _ImpPotenzialePrenotato = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property PianoDeiContiFinanziario() As String
        Get
            Return _PianoDeiContiFinanziari
        End Get
        Set(ByVal value As String)
            _PianoDeiContiFinanziari = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property CodiceRisposta() As String
        Get
            Return _CodiceRisposta
        End Get
        Set(ByVal value As String)
            _CodiceRisposta = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Public Property DescrizioneRisposta() As String
        Get
            Return _DescrizioneRisposta
        End Get
        Set(ByVal value As String)
            _DescrizioneRisposta = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Public Property ListaBeneficiari() As Generic.List(Of Ext_AnagraficaInfo)
        Get
            Return _listaBeneficiari
        End Get
        Set(ByVal value As Generic.List(Of Ext_AnagraficaInfo))
            _listaBeneficiari = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Public Property Beneficiario() As Ext_AnagraficaInfo
        Get
            Return _beneficiario
        End Get
        Set(ByVal value As Ext_AnagraficaInfo)
            _beneficiario = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Public Property HashTokenCallSic as String
        Get
            return _HashTokenCallSic
        End Get
        Set
            _HashTokenCallSic = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property IdDocContabileSic as String
        Get
            return _IdDocContabileSic
        End Get
        Set
            _IdDocContabileSic = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property HashTokenCallSic_Imp as String
        Get
            return _HashTokenCallSic_Imp
        End Get
        Set
            _HashTokenCallSic_Imp = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property IdDocContabileSic_Imp as String
        Get
            return _IdDocContabileSIC_Imp
        End Get
        Set
            _IdDocContabileSIC_Imp = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New( _
    ByVal BilancioINP As String, _
    ByVal UPBINP As String, _
    ByVal MissioneProgrammaINP As String, _
    ByVal CapitoloINP As String, _
    ByVal ImpDispINP As String, _
    ByVal ImpPrenotatoINP As String, _
    Optional ByVal NumPrenotazioneINP As String = "", _
    Optional ByVal AnnoPrenotazioneINP As String = "", _
    Optional ByVal IDINP As String = "")


        Bilancio = BilancioINP
        UPB = UPBINP
        MissioneProgramma = MissioneProgrammaINP
        Capitolo = CapitoloINP
        ImpDisp = ImpDispINP
        ImpPrenotato = ImpPrenotatoINP
        NumPreImp = NumPrenotazioneINP
        AnnoPrenotazione = AnnoPrenotazioneINP
        ID = IDINP
    End Sub
End Class
