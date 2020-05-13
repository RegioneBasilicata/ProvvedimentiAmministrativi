Public Class ItemTipologiaDocumento
    Private _id As Long = -1
    Private _tipologia As String = ""
    Private _hasDestinatari As Boolean = False
    Private _hasDestinatariObbligatori As Boolean = False
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property
    Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property
    Property HasDestinatari() As Boolean
        Get
            Return _hasDestinatari
        End Get
        Set(ByVal value As Boolean)
            _hasDestinatari = value
        End Set
    End Property
    Property HasDestinatariObbligatori() As Boolean
        Get
            Return _hasDestinatariObbligatori
        End Get
        Set(ByVal value As Boolean)
            _hasDestinatariObbligatori = value
        End Set
    End Property
End Class
