Public Class ItemDaImpegnare

    Private _anno As Integer
    Private _capitolo As String
    Private _totale As Double
    Private _preimpegno As String
    Private _missioneProgramma As String

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

    Public Property Totale As Double
        Get
            Return _totale
        End Get
        Set(value As Double)
            _totale = value
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

    Public Property MissioneProgramma As String
        Get
            Return _missioneProgramma
        End Get
        Set(value As String)
            _missioneProgramma = value
        End Set
    End Property
End Class
