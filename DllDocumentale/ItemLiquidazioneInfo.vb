Public Class ItemLiquidazioneInfo
    Inherits ItemContabileInfo

    Private _Dli_TipoAssunzione As Integer
    Private _Dli_Num_assunzione As String
    Private _Dli_Data_Assunzione As DateTime
    Private _Dli_Anno As String
    Private _Dli_ImportoIva As Decimal
    Private _Dli_Por As Integer
    Private _Dli_AllegatoPor As Object
    Private _Dli_NLiquidazione As Integer
    Private _Dli_IdImpegno As Integer
    Private _listaBeneficiari As Generic.List(Of ItemLiquidazioneImpegnoBeneficiarioInfo)


    Property Dli_Allegato() As Object
        Get
            Return _Dli_AllegatoPor
        End Get
        Set(ByVal Value As Object)
            _Dli_AllegatoPor = Value

        End Set
    End Property

    Property Dli_Por() As Integer
        Get
            Return _Dli_Por
        End Get
        Set(ByVal value As Integer)
            _Dli_Por = value
        End Set
    End Property

    Property Dli_NLiquidazione() As Long
        Get
            Return _Dli_NLiquidazione
        End Get
        Set(ByVal value As Long)
            _Dli_NLiquidazione = value
        End Set
    End Property

    Property Dli_ImportoIva() As Integer
        Get
            Return _Dli_ImportoIva
        End Get
        Set(ByVal value As Integer)
            _Dli_ImportoIva = value
        End Set
    End Property

    Property Dli_Anno() As String
        Get
            Return _Dli_Anno
        End Get
        Set(ByVal value As String)
            _Dli_Anno = value
        End Set
    End Property
    Property Dli_TipoAssunzione() As Integer
        Get
            Return _Dli_TipoAssunzione
        End Get
        Set(ByVal value As Integer)
            _Dli_TipoAssunzione = value
        End Set
    End Property
    Property Dli_Num_assunzione() As String
        Get
            Return _Dli_Num_assunzione
        End Get
        Set(ByVal value As String)
            _Dli_Num_assunzione = value
        End Set
    End Property
    Property Dli_Data_Assunzione() As DateTime
        Get
            Return _Dli_Data_Assunzione
        End Get
        Set(ByVal value As DateTime)
            _Dli_Data_Assunzione = value
        End Set
    End Property

    Property Dli_IdImpegno() As Integer
        Get
            Return _Dli_IdImpegno
        End Get
        Set(ByVal value As Integer)
            _Dli_IdImpegno = value
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


End Class
