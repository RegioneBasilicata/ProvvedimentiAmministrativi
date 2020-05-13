Public Class ItemReportGraficoAtti

    Private _TotaleAtti As Integer
    Private _TotaleDetermine As Integer
    Private _TotaleDisposizioni As Integer
    Private _Descrizione As String

    Property TotaleAtti() As Integer
        Get
            Return _TotaleAtti
        End Get
        Set(ByVal Value As Integer)
            _TotaleAtti = Value

        End Set
    End Property
    Property TotaleDetermine() As Integer
        Get
            Return _TotaleDetermine
        End Get
        Set(ByVal Value As Integer)
            _TotaleDetermine = Value

        End Set
    End Property
    Property TotaleDisposizioni() As Integer
        Get
            Return _TotaleDisposizioni
        End Get
        Set(ByVal Value As Integer)
            _TotaleDisposizioni = Value

        End Set
    End Property
    Property Descrizione() As String
        Get
            Return _Descrizione
        End Get
        Set(ByVal value As String)
            _Descrizione = value
        End Set
    End Property

End Class
