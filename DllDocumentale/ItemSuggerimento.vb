Public Class ItemSuggerimento
    Private _id As Long
    Private _codOperatore As String = ""
    Private _Doc_Id As String = ""
    Private _Livello_Ufficio As String = ""
    Private _dataRegistrazione As DateTime
    Private _Ruolo As String = ""
    Private _isPubblico As Boolean = True
    Private _id_Suggerimento As Long
    Private _Note As String
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property
    Property CodOperatore() As String
        Get
            Return _codOperatore
        End Get
        Set(ByVal value As String)
            _codOperatore = value
        End Set
    End Property
    Property Doc_Id() As String
        Get
            Return _Doc_Id
        End Get
        Set(ByVal value As String)
            _Doc_Id = value
        End Set
    End Property
    Property Livello_Ufficio() As String
        Get
            Return _Livello_Ufficio
        End Get
        Set(ByVal value As String)
            _Livello_Ufficio = value
        End Set
    End Property
    Property DataRegistrazione() As DateTime
        Get
            Return _dataRegistrazione
        End Get
        Set(ByVal value As DateTime)
            _dataRegistrazione = value
        End Set
    End Property
    Property Ruolo() As String
        Get
            Return _Ruolo
        End Get
        Set(ByVal value As String)
            _Ruolo = value
        End Set
    End Property
    Property isPubblico() As Boolean
        Get
            Return _isPubblico
        End Get
        Set(ByVal value As Boolean)
            _isPubblico = value
        End Set
    End Property
    Property Id_Suggerimento() As Long
        Get
            Return _id_Suggerimento
        End Get
        Set(ByVal value As Long)
            _id_Suggerimento = value
        End Set
    End Property
    Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal value As String)
            _Note = value
        End Set
    End Property
End Class
Public Class ItemSuggerimentoInfo
    Private _id As Long
    Private _descrizioneBreve As String = ""
    Private _descrizione As String = ""
    Private _tipologiaAtto As String = ""
   
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property
    Property DescrizioneBreve() As String
        Get
            Return _descrizioneBreve
        End Get
        Set(ByVal value As String)
            _descrizioneBreve = value
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
    Property TipologiaAtto() As String
        Get
            Return _tipologiaAtto
        End Get
        Set(ByVal value As String)
            _tipologiaAtto = value
        End Set
    End Property
End Class
