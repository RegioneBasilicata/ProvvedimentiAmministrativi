Public Class ItemContabileInfo

    Private _Dli_Documento As String
    Private _Dli_prog As Long
    Private _Dli_DataRegistrazione As DateTime
    Private _Dli_Operatore As String
    Private _Dli_Esercizio As String
    Private _Dli_UPB As String
    Private _Dli_MissioneProgramma As String
    Private _Dli_PianoDeiContiFinanziari As String
    Private _Dli_Cap As String
    Private _Dli_Costo As Decimal
    Private _Dli_NumImpegno As String
    Private _Dli_NPreImpegno As String
    Private _Di_ContoEconomica As String
    Private _Di_Stato As Integer = 1
    Private _hashTokenCallSic As String
    Private _idDocContabileSIC As String
    
     
    Property Dli_Documento() As String
        Get
            Return _Dli_Documento
        End Get
        Set(ByVal value As String)
            _Dli_Documento = value
        End Set
    End Property
    Property Dli_prog() As Long
        Get
            Return _Dli_prog
        End Get
        Set(ByVal value As Long)
            _Dli_prog = value
        End Set
    End Property
    Property Dli_DataRegistrazione() As DateTime
        Get
            Return _Dli_DataRegistrazione
        End Get
        Set(ByVal value As DateTime)
            _Dli_DataRegistrazione = value
        End Set
    End Property
    Property Dli_Operatore() As String
        Get
            Return _Dli_Operatore
        End Get
        Set(ByVal value As String)
            _Dli_Operatore = value
        End Set
    End Property
    Property Dli_Esercizio() As String
        Get
            Return _Dli_Esercizio
        End Get
        Set(ByVal value As String)
            _Dli_Esercizio = value
        End Set
    End Property
    Property Dli_UPB() As String
        Get
            Return _Dli_UPB
        End Get
        Set(ByVal value As String)
            _Dli_UPB = value
        End Set
    End Property
    Property Dli_MissioneProgramma() As String
        Get
            Return _Dli_MissioneProgramma
        End Get
        Set(ByVal value As String)
            _Dli_MissioneProgramma = value
        End Set
    End Property
    Property Dli_PianoDeiContiFinanziari() As String
        Get
            Return _Dli_PianoDeiContiFinanziari
        End Get
        Set(ByVal value As String)
            _Dli_PianoDeiContiFinanziari = value
        End Set
    End Property
    Property Dli_Cap() As String
        Get
            Return _Dli_Cap
        End Get
        Set(ByVal value As String)
            _Dli_Cap = value
        End Set
    End Property
    Property Dli_Costo() As Decimal
        Get
            Return _Dli_Costo
        End Get
        Set(ByVal value As Decimal)
            _Dli_Costo = value
        End Set
    End Property
    Property Dli_NumImpegno() As String
        Get
            Return _Dli_NumImpegno
        End Get
        Set(ByVal value As String)
            _Dli_NumImpegno = value
        End Set
    End Property
    Property Dli_NPreImpegno() As String
        Get
            Return _Dli_NPreImpegno
        End Get
        Set(ByVal value As String)
            _Dli_NPreImpegno = value
        End Set
    End Property
    Property Di_ContoEconomica() As String
        Get
            Return _Di_ContoEconomica
        End Get
        Set(ByVal value As String)
            _Di_ContoEconomica = value
        End Set
    End Property
    Property Di_Stato() As Integer
        Get
            Return _Di_Stato
        End Get
        Set(ByVal value As Integer)
            _Di_Stato = value
        End Set
    End Property
    Property HashTokenCallSic() As String
        Get
            Return _hashTokenCallSic
        End Get
        Set(ByVal value As String)
            _hashTokenCallSic = value
        End Set
    End Property

    Public Property IdDocContabileSic as String
        Get
            return _idDocContabileSIC
        End Get
        Set
            _idDocContabileSIC = value
        End Set
    End Property
End Class


