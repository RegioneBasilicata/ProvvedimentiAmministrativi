Public Class ItemDettaglioStorico
    
    Private _ID As Integer
    Private _IdDocumento As String = ""
    Private _Progressivo As Integer
    Private _IdUfficio As String = ""
    Private _CodiceUfficio As String = ""
    Private _DescrizioneUfficio As String = ""
    Private _Giorni As Integer
    Private _DataArrivo As Date
    Private _DataUscita As Date
    Private _Utente As String = ""
    Private _Stato As String = ""

    Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As String)
            _IdDocumento = value
        End Set
    End Property
    Property Progressivo() As Integer
        Get
            Return _Progressivo
        End Get
        Set(ByVal value As Integer)
            _Progressivo = value
        End Set
    End Property
    Property IdUfficio() As String
        Get
            Return _IdUfficio
        End Get
        Set(ByVal value As String)
            _IdUfficio = value
        End Set
    End Property
    Property CodiceUfficio() As String
        Get
            Return _CodiceUfficio
        End Get
        Set(ByVal value As String)
            _CodiceUfficio = value
        End Set
    End Property
    Property DescrizioneUfficio() As String
        Get
            Return _DescrizioneUfficio
        End Get
        Set(ByVal value As String)
            _DescrizioneUfficio = value
        End Set
    End Property
    Property Giorni() As Integer
        Get
            Return _Giorni
        End Get
        Set(ByVal value As Integer)
            _Giorni = value
        End Set
    End Property
    Property DataArrivo() As Date
        Get
            Return _DataArrivo
        End Get
        Set(ByVal value As Date)
            _DataArrivo = value
        End Set
    End Property
    Property DataUscita() As Date
        Get
            Return _DataUscita
        End Get
        Set(ByVal value As Date)
            _DataUscita = value
        End Set
    End Property
    Property Utente() As String
        Get
            Return _Utente
        End Get
        Set(ByVal value As String)
            _Utente = value
        End Set
    End Property
    Property Stato() As String
        Get
            Return _Stato
        End Get
        Set(ByVal value As String)
            _Stato = value
        End Set
    End Property
End Class
