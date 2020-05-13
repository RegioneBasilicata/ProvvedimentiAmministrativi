Public Class ItemContrattoInfoHeader    
    Private _idContratto As String = String.Empty
    Private _idDocumento As String = String.Empty
    Private _codieCIG As String = String.Empty
    Private _codieCUP As String = String.Empty

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property
    Public Property IdContratto() As String
        Get
            Return _idContratto
        End Get
        Set(ByVal value As String)
            _idContratto = value
        End Set
    End Property
    Public Property CodieCIG() As String
        Get
            Return _codieCIG
        End Get
        Set(ByVal value As String)
            _codieCIG = value
        End Set
    End Property
    Public Property CodieCUP() As String
        Get
            Return _codieCUP
        End Get
        Set(ByVal value As String)
            _codieCUP = value
        End Set
    End Property
End Class
