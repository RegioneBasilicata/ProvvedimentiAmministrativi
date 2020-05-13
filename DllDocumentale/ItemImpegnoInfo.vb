Public Class ItemImpegnoInfo
    Inherits ItemContabileInfo

    
    Private _hashTokenCallSic_Imp As String
    Private _idDocContabileSIC_Imp As String
    Private _Di_Ratei As Decimal
    Private _Di_Risconti As Decimal
    Private _Di_ImpostaIrap As Decimal
    Private _NDocPrecedente As String
    Private _Di_PreImpDaPrenotazione As Integer
    Private _Dbi_Anno As String
    Private _Codice_obbiettivo_Gestionale As String = ""
    Private _Piano_dei_conti_finanari As String = ""
    Private _Di_TipoAssunzioneDescr As String = ""
    Private _Di_TipoAssunzione As Integer
    Private _Di_Num_assunzione As String
    Private _Di_Data_Assunzione As DateTime
    Private _listaBeneficiari As Generic.List(Of ItemLiquidazioneImpegnoBeneficiarioInfo)

    
   


    Property DBi_Anno() As String
        Get
            Return _Dbi_Anno
        End Get
        Set(ByVal value As String)
            _Dbi_Anno = value
        End Set
    End Property
    Property Di_Ratei() As Decimal
        Get
            Return _Di_Ratei
        End Get
        Set(ByVal value As Decimal)
            _Di_Ratei = value
        End Set
    End Property
    Property Di_Risconti() As Decimal
        Get
            Return _Di_Risconti
        End Get
        Set(ByVal value As Decimal)
            _Di_Risconti = value
        End Set
    End Property
    Property Di_ImpostaIrap() As Decimal
        Get
            Return _Di_ImpostaIrap
        End Get
        Set(ByVal value As Decimal)
            _Di_ImpostaIrap = value
        End Set
    End Property
    Property NDocPrecedente() As String
        Get
            Return _NDocPrecedente
        End Get
        Set(ByVal value As String)
            _NDocPrecedente = value
        End Set
    End Property
    Property Di_PreImpDaPrenotazione() As Integer
        Get
            Return _Di_PreImpDaPrenotazione
        End Get
        Set(ByVal value As Integer)
            _Di_PreImpDaPrenotazione = value
        End Set
    End Property


    Property Codice_Obbiettivo_Gestionale() As String
        Get
            Return _Codice_obbiettivo_Gestionale
        End Get
        Set(ByVal value As String)
            _Codice_obbiettivo_Gestionale = value
        End Set
    End Property

    Property Piano_Dei_Conti_Finanziari() As String
        Get
            Return _Piano_dei_conti_finanari
        End Get
        Set(ByVal value As String)
            _Piano_dei_conti_finanari = value
        End Set
    End Property
    Property Di_TipoAssunzioneDescr() As String
        Get
            Return _Di_TipoAssunzioneDescr
        End Get
        Set(ByVal value As String)
            _Di_TipoAssunzioneDescr = value
        End Set
    End Property
    Property Di_TipoAssunzione() As Integer
        Get
            Return _Di_TipoAssunzione
        End Get
        Set(ByVal value As Integer)
            _Di_TipoAssunzione = value
        End Set
    End Property
    Property Di_Num_assunzione() As String
        Get
            Return _Di_Num_assunzione
        End Get
        Set(ByVal value As String)
            _Di_Num_assunzione = value
        End Set
    End Property
    Property Di_Data_Assunzione() As DateTime
        Get
            Return _Di_Data_Assunzione
        End Get
        Set(ByVal value As DateTime)
            _Di_Data_Assunzione = value
        End Set
    End Property
    Property ListaBeneficiari() As Generic.List(Of ItemLiquidazioneImpegnoBeneficiarioInfo)
        Get
            Return _listaBeneficiari
        End Get
        Set(ByVal Value As Generic.List(Of ItemLiquidazioneImpegnoBeneficiarioInfo))
            _listaBeneficiari = Value
        End Set
    End Property

    

    Public Property HashTokenCallSic_Imp as String
        Get
            return _hashTokenCallSic_Imp
        End Get
        Set
            _hashTokenCallSic_Imp = value
        End Set
    End Property

    Public Property IdDocContabileSic_Imp as String
        Get
            return _idDocContabileSIC_Imp
        End Get
        Set
            _idDocContabileSIC_Imp = value
        End Set
    End Property
End Class
