<System.Runtime.Serialization.DataContract()> _
Public Class Ext_LiquidazioneInfo
    Inherits Ext_CapitoliAnnoInfo

    Private _ImpPrenotatoLiq As Double = 0
    Private _ImportoIva As String = String.Empty
    Private _NLiquidazione As String = String.Empty
    Private _IdImpegno As String = String.Empty
    'Private _listaBeneficiari As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
    <System.Runtime.Serialization.DataMember()> _
    Public Property ImpPrenotatoLiq() As Double
        Get
            Return _ImpPrenotatoLiq
        End Get
        Set(ByVal value As Double)
            _ImpPrenotatoLiq = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Public Property ImportoIva() As String
        Get
            Return _ImportoIva
        End Get
        Set(ByVal value As String)
            _ImportoIva = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Public Property NLiquidazione() As String
        Get
            Return _NLiquidazione
        End Get
        Set(ByVal value As String)
            _NLiquidazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Public Property IdImpegno() As String
        Get
            Return _IdImpegno
        End Get
        Set(ByVal value As String)
            _IdImpegno = value
        End Set
    End Property

    '<System.Runtime.Serialization.DataMember()> _
    'Public Property ListaBeneficiari() As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
    '    Get
    '        Return _listaBeneficiari
    '    End Get
    '    Set(ByVal value As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo))
    '        _listaBeneficiari = value
    '    End Set
    'End Property
    Public Sub New()

    End Sub
    Public Sub New( _
    ByVal BilancioINP As String, _
    ByVal UPBINP As String, _
    ByVal MissioneProgrammaINP As String, _
    ByVal CapitoloINP As String, _
    ByVal ImpPrenotatoLiqINP As String)

        Bilancio = BilancioINP
        UPB = UPBINP
        MissioneProgramma = MissioneProgrammaINP
        Capitolo = CapitoloINP
        ImpPrenotatoLiq = ImpPrenotatoLiqINP
    End Sub
End Class
