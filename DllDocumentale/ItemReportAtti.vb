Public Class ItemReportAtti
   
    Private _TotaleAtti As Integer
    Private _TotaleDetermine As Integer
    Private _TotaleDisposizioni As Integer
    Private _CodDipartimento As String
    Private _DescDipartimento As String

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
    Property CodDipartimento() As String
        Get
            Return _CodDipartimento
        End Get
        Set(ByVal value As String)
            _CodDipartimento = value
        End Set
    End Property
    Property DescDipartimento() As String
        Get
            Return _DescDipartimento
        End Get
        Set(ByVal value As String)
            _DescDipartimento = value
        End Set
    End Property
    
End Class
