
Imports DllDocumentale

<System.Runtime.Serialization.DataContract()> _
Public Class ItemDocumentoBeneficiarioExcel
    Private _prog As Long
    Private _groupedBy As Integer
    Private _excelRiga As Integer
    Private _idDocumento As String
    Private _anno As Integer
    Private _capitolo As String
    Private _preimpegno As String
    Private _importo As Decimal
    Private _codObGestionale As String
    Private _pcf As String
    Private _missioneProgramma As String
    Private _generaLiquidazioneContestuale As Boolean
    Private _dataCaricamento As DateTime
    Private _operatoreCaricamento As String
    Private _anagrafica As ItemAnagrafica
    Private _descrizioneOperazioneSIC as String

    Public Property Prog As Long
        Get
            Return _prog
        End Get
        Set(value As Long)
            _prog = value
        End Set
    End Property

    Public Property IdDocumento As String
        Get
            Return _idDocumento
        End Get
        Set(value As String)
            _idDocumento = value
        End Set
    End Property

    Public Property Anno As Integer
        Get
            Return _anno
        End Get
        Set(value As Integer)
            _anno = value
        End Set
    End Property

    Public Property Capitolo As String
        Get
            Return _capitolo
        End Get
        Set(value As String)
            _capitolo = value
        End Set
    End Property

    Public Property Preimpegno As String
        Get
            Return _preimpegno
        End Get
        Set(value As String)
            _preimpegno = value
        End Set
    End Property

    Public Property Importo As Decimal
        Get
            Return _importo
        End Get
        Set(value As Decimal)
            _importo = value
        End Set
    End Property

    Public Property CodObGestionale As String
        Get
            Return _codObGestionale
        End Get
        Set(value As String)
            _codObGestionale = value
        End Set
    End Property

    Public Property Pcf As String
        Get
            Return _pcf
        End Get
        Set(value As String)
            _pcf = value
        End Set
    End Property


    Public Property DataCaricamento As Date
        Get
            Return _dataCaricamento
        End Get
        Set(value As Date)
            _dataCaricamento = value
        End Set
    End Property

    Public Property OperatoreCaricamento As String
        Get
            Return _operatoreCaricamento
        End Get
        Set(value As String)
            _operatoreCaricamento = value
        End Set
    End Property

    Public Property Anagrafica As ItemAnagrafica
        Get
            Return _anagrafica
        End Get
        Set(value As ItemAnagrafica)
            _anagrafica = value
        End Set
    End Property

    Public Property ExcelRiga As Integer
        Get
            Return _excelRiga
        End Get
        Set(value As Integer)
            _excelRiga = value
        End Set
    End Property

    Public Property GeneraLiquidazioneContestuale As Boolean
        Get
            Return _generaLiquidazioneContestuale
        End Get
        Set(value As Boolean)
            _generaLiquidazioneContestuale = value
        End Set
    End Property

    Public Property GroupedBy As Integer
        Get
            Return _groupedBy
        End Get
        Set(value As Integer)
            _groupedBy = value
        End Set
    End Property

    Public Property DescrizioneOperazioneSIC As String
        Get
            Return _descrizioneOperazioneSIC
        End Get
        Set(value As String)
            _descrizioneOperazioneSIC = value
        End Set
    End Property

    Public Property MissioneProgramma As String
        Get
            Return _missioneProgramma
        End Get
        Set(value As String)
            _missioneProgramma = value
        End Set
    End Property
End Class
