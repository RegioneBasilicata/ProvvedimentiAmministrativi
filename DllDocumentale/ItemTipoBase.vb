Public Class ItemTipoBase
    Private _id As Long = 0
    Private _descrizione As String = ""
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
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

End Class
