Public Class ItemAttivita
    Private _Sto_id_Doc As String = ""
    Private _Sto_Utente As String = ""
    Private _Sto_Prog As Long
    Private _Sto_Ufficio As String = ""
    Private _Sto_Data As DateTime
    Private _Sto_TipoAttivita As String = ""
    Private _Sto_attivita_corrente As Boolean = True
    Private _Sto_Ruolo As String = ""
    Private _Sto_Livello As String = ""
    Private _Sto_Info_Attivita As String = ""
    Private _Sto_note As String = ""
    Private _Sto_Nominativo As String = ""
    Private _Sto_idAllegato As String = ""
    Property Sto_id_Doc() As String
        Get
            Return _Sto_id_Doc
        End Get
        Set(ByVal value As String)
            _Sto_id_Doc = value
        End Set
    End Property
    Property Sto_Utente() As String
        Get
            Return _Sto_Utente
        End Get
        Set(ByVal value As String)
            _Sto_Utente = value
        End Set
    End Property
    Property Sto_Prog() As Long
        Get
            Return _Sto_Prog
        End Get
        Set(ByVal value As Long)
            _Sto_Prog = value
        End Set
    End Property
    Property Sto_Ufficio() As String
        Get
            Return _Sto_Ufficio
        End Get
        Set(ByVal value As String)
            _Sto_Ufficio = value
        End Set
    End Property
    Property Sto_Data() As DateTime
        Get
            Return _Sto_Data
        End Get
        Set(ByVal value As DateTime)
            _Sto_Data = value
        End Set
    End Property
    Property Sto_TipoAttivita() As String
        Get
            Return _Sto_TipoAttivita
        End Get
        Set(ByVal value As String)
            _Sto_TipoAttivita = value
        End Set
    End Property
    Property Sto_attivita_corrente() As Boolean
        Get
            Return _Sto_attivita_corrente
        End Get
        Set(ByVal value As Boolean)
            _Sto_attivita_corrente = value
        End Set
    End Property
    Property Sto_Ruolo() As String
        Get
            Return _Sto_Ruolo
        End Get
        Set(ByVal value As String)
            _Sto_Ruolo = value
        End Set
    End Property
    Property Sto_Livello() As String
        Get
            Return _Sto_Livello
        End Get
        Set(ByVal value As String)
            _Sto_Livello = value
        End Set
    End Property
    Property Sto_Info_Attivita() As String
        Get
            Return _Sto_Info_Attivita
        End Get
        Set(ByVal value As String)
            _Sto_Info_Attivita = value
        End Set
    End Property
    Property Sto_idAllegato() As String
        Get
            Return _Sto_idAllegato
        End Get
        Set(ByVal value As String)
            _Sto_idAllegato = value
        End Set
    End Property
    Property Sto_note() As String
        Get
            Return _Sto_note
        End Get
        Set(ByVal value As String)
            _Sto_note = value
        End Set
    End Property
    Property Sto_Nominativo() As String
        Get
            Return _Sto_Nominativo
        End Get
        Set(ByVal value As String)
            _Sto_Nominativo = value
        End Set
    End Property
End Class