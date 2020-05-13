Public Class ItemRelatore
    Private _id As String = String.Empty
    Private _cognome As String = ""
    Private _nome As String = ""
    Private _ordineApparizione As Integer = -1
    Private _carica As String = ""
    Private _attivo As Boolean
    Private _dataAttivazione As Date
    Private _dataDisttivazione As Date
    Private _idStruttura As String = ""
    Private _isPresente As Boolean


    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property Cognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal value As String)
            _cognome = value
        End Set
    End Property

    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property OrdineApparizione() As Integer
        Get
            Return _ordineApparizione
        End Get
        Set(ByVal value As Integer)
            _ordineApparizione = value
        End Set
    End Property

    Public Property Carica() As String
        Get
            Return _carica
        End Get
        Set(ByVal value As String)
            _carica = value
        End Set
    End Property
    Public Property Attivo() As Boolean
        Get
            Return _attivo
        End Get
        Set(ByVal value As Boolean)
            _attivo = value
        End Set
    End Property

    Public Property DataAttivazione() As Date
        Get
            Return _dataAttivazione
        End Get
        Set(ByVal value As Date)
            _dataAttivazione = value
        End Set
    End Property

    Public Property DataDisttivazione() As Date
        Get
            Return _dataDisttivazione
        End Get
        Set(ByVal value As Date)
            _dataDisttivazione = value
        End Set
    End Property

    Public Property IdStruttura() As String
        Get
            Return _idStruttura
        End Get
        Set(ByVal value As String)
            _idStruttura = value
        End Set
    End Property
    Public Property IsPresente() As Boolean
        Get
            Return _isPresente
        End Get
        Set(ByVal value As Boolean)
            _isPresente = value
        End Set
    End Property

End Class
