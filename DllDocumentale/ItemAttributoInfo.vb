Public Class ItemAttributoInfo

    Private _Oggetto_Riferimento As String
    Private _ente As String
    Private _Codice As String
    Private _Descrizione As String
    Private _Tipo_Scelta As ItemTipoSceltaAttributo
    Private _SoggettoARicerca As Boolean
    Private _VisibileInDocumento As Boolean
    Private _OrdineApparizione As Integer
    Private _Obbligatorio As Boolean
    Private _listaSceltePossibili As Generic.List(Of ItemSceltaPossibile)

    Property Oggetto_Riferimento() As String
        Get
            Return _Oggetto_Riferimento
        End Get
        Set(ByVal value As String)
            _Oggetto_Riferimento = value
        End Set
    End Property
    Property Ente() As String
        Get
            Return _ente
        End Get
        Set(ByVal value As String)
            _ente = value
        End Set
    End Property
    Property Codice() As String
        Get
            Return _Codice
        End Get
        Set(ByVal value As String)
            _Codice = value
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
    Property Tipo_Scelta() As ItemTipoSceltaAttributo
        Get
            Return _Tipo_Scelta
        End Get
        Set(ByVal value As ItemTipoSceltaAttributo)
            _Tipo_Scelta = value
        End Set
    End Property
    Property SoggettoARicerca() As Boolean
        Get
            Return _SoggettoARicerca
        End Get
        Set(ByVal value As Boolean)
            _SoggettoARicerca = value
        End Set
    End Property
    Property VisibileInDocumento() As Boolean
        Get
            Return _VisibileInDocumento
        End Get
        Set(ByVal value As Boolean)
            _VisibileInDocumento = value
        End Set
    End Property
    Property OrdineApparizione() As Integer
        Get
            Return _OrdineApparizione
        End Get
        Set(ByVal value As Integer)
            _OrdineApparizione = value
        End Set
    End Property
    Property Obbligatorio() As Boolean
        Get
            Return _Obbligatorio
        End Get
        Set(ByVal value As Boolean)
            _Obbligatorio = value
        End Set
    End Property
    Property ListaSceltePossibili() As Generic.List(Of ItemSceltaPossibile)
        Get
            Return _listaSceltePossibili
        End Get
        Set(ByVal value As Generic.List(Of ItemSceltaPossibile))
            _listaSceltePossibili = value
        End Set
    End Property

End Class


