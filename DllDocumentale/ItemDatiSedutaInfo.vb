Public Class ItemAzioneUtenteDocumento
    Private _sto_id_Doc As String = String.Empty
    Private _sto_Prog As Integer
    Private _sto_Utente As String = String.Empty
    Private _sto_Ufficio As String = String.Empty
    Private _sto_Data As DateTime
    Private _sto_TipoAttivita As String = String.Empty
    Private _sto_Info_Attivita As String = String.Empty
    Private _sto_attivita_corrente As Integer
    Private _sto_idAllegato As String = String.Empty
    Private _sto_Nominativo As String = String.Empty
    Private _sto_Livello As String = String.Empty
    Private _sto_Ruolo As String = String.Empty
    Private _sto_note As String = String.Empty

    Public Property Sto_id_Doc() As String
        Get
            Return _sto_id_Doc
        End Get
        Set(ByVal value As String)
            _sto_id_Doc = value
        End Set
    End Property

    Public Property Sto_Prog() As Integer
        Get
            Return _sto_Prog
        End Get
        Set(ByVal value As Integer)
            _sto_Prog = value
        End Set
    End Property

    Public Property Sto_Utente() As String
        Get
            Return _sto_Utente
        End Get
        Set(ByVal value As String)
            _sto_Utente = value
        End Set
    End Property

    Public Property Sto_Ufficio() As String
        Get
            Return _sto_Ufficio
        End Get
        Set(ByVal value As String)
            _sto_Ufficio = value
        End Set
    End Property


    Public Property Sto_Data() As DateTime
        Get
            Return _sto_Data
        End Get
        Set(ByVal value As DateTime)
            _sto_Data = value
        End Set
    End Property

    Public Property Sto_TipoAttivita() As String
        Get
            Return _sto_TipoAttivita
        End Get
        Set(ByVal value As String)
            _sto_TipoAttivita = value
        End Set
    End Property

    Public Property Sto_Info_Attivita() As String
        Get
            Return _sto_Info_Attivita
        End Get
        Set(ByVal value As String)
            _sto_Info_Attivita = value
        End Set
    End Property

    Public Property Sto_attivita_corrente() As Integer
        Get
            Return _sto_attivita_corrente
        End Get
        Set(ByVal value As Integer)
            _sto_attivita_corrente = value
        End Set
    End Property

    Public Property Sto_idAllegato() As String
        Get
            Return _sto_idAllegato
        End Get
        Set(ByVal value As String)
            _sto_idAllegato = value
        End Set
    End Property

    Public Property Sto_Nominativo() As String
        Get
            Return _sto_Nominativo
        End Get
        Set(ByVal value As String)
            _sto_Nominativo = value
        End Set
    End Property

    Public Property Sto_Livello() As String
        Get
            Return _sto_Livello
        End Get
        Set(ByVal value As String)
            _sto_Livello = value
        End Set
    End Property


    Public Property Sto_Ruolo() As String
        Get
            Return _sto_Ruolo
        End Get
        Set(ByVal value As String)
            _sto_Ruolo = value
        End Set
    End Property


    Public Property Sto_note() As String
        Get
            Return _sto_note
        End Get
        Set(ByVal value As String)
            _sto_note = value
        End Set
    End Property
End Class
