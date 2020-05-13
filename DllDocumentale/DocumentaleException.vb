Public Class DocumentaleException

    Inherits Exception

    Private _esito As String
    Sub New()

    End Sub
    Sub New(ByVal messaggio As String)
        MyBase.New(messaggio)
    End Sub
    Sub New(ByVal messaggio As String, ByVal esito As String)
        MyBase.New(messaggio)
        _esito = esito
    End Sub
    ReadOnly Property Esito() As String
        Get
            Return _esito
        End Get
    End Property
End Class


