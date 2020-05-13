Public Class ItemEstensione
    Private _id As Long = 0
    Private _nome As String = ""
    Private _descrizione As String = ""
    Private _contentType As String = ""
    Private _scarica As Boolean = False
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property
    Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property
    Property Descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
    Property ContentType() As String
        Get
            Return _contentType
        End Get
        Set(ByVal value As String)
            _contentType = value
        End Set
    End Property
    Property Scarica() As Boolean
        Get
            Return _scarica
        End Get
        Set(ByVal value As Boolean)
            _scarica = value
        End Set
    End Property
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return UCase(DirectCast(obj, ItemEstensione).Nome) = UCase(Nome)
    End Function
End Class
