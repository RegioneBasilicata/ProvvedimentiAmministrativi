Public Class ItemDocumentoConservazione 

    Private _Id As Integer
    Private _Numero_Definitivo As String
    Private _Id_Documento As String
    Private _Id_Allegato As String
    Private _Data_Archivio As DateTime
    Private _Estensione_Allegato As String
    Private _Codice_Operatore_Allegato As String 
    Private _Nominativo_Operatore_Allegato As String 
    Private _Data_Registrazione_Allegato As DateTime
    Private _Data_Aggiornamento As DateTime
    Private _Id_Alfresco As String = ""
    Private _Path_Alfresco As String = ""
    Private _Id_Stato As Integer
    Private _Stato As String

    Private _Descrizione_Errore As String


    Public Property Descrizione_Errore As String
        Get
            Return _Descrizione_Errore
        End Get
        Set(value As String)
            _Descrizione_Errore = value
        End Set
    End Property

    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property

    Public Property Numero_Definitivo As String
        Get
            Return _Numero_Definitivo
        End Get
        Set(value As String)
            _Numero_Definitivo = value
        End Set
    End Property

    Public Property Id_Documento As String
        Get
            Return _Id_Documento
        End Get
        Set(value As String)
            _Id_Documento = value
        End Set
    End Property

   Public Property Stato As String
        Get
            Return _Stato
        End Get
        Set(value As String)
            _Stato = value
        End Set
    End Property

    Public Property Id_Stato As Integer
        Get
            Return _Id_Stato
        End Get
        Set(value As Integer)
            _Id_Stato = value
        End Set
    End Property

    Public Property Id_Allegato As String
        Get
            Return _Id_Allegato
        End Get
        Set(value As String)
            _Id_Allegato = value
        End Set
    End Property

    Public Property Data_Aggiornamento As Date
        Get
            Return _Data_Aggiornamento
        End Get
        Set(value As Date)
            _Data_Aggiornamento = value
        End Set
    End Property

    Public Property Data_Archivio As Date
        Get
            Return _Data_Archivio
        End Get
        Set(value As Date)
            _Data_Archivio = value
        End Set
    End Property
    Public Property Data_Registrazione_Allegato As Date
        Get
            Return _Data_Registrazione_Allegato
        End Get
        Set(value As Date)
            _Data_Registrazione_Allegato = value
        End Set
    End Property


    Public Property Id_Alfresco As String
        Get
            Return _Id_Alfresco
        End Get
        Set(value As String)
            _Id_Alfresco = value
        End Set
    End Property

    Public Property Path_Alfresco As String
        Get
            Return _Path_Alfresco
        End Get
        Set(value As String)
            _Path_Alfresco = value
        End Set
    End Property

    Public Property Estensione_Allegato As String
        Get
            Return _Estensione_Allegato
        End Get
        Set(value As String)
            _Estensione_Allegato = value
        End Set
    End Property
    Public Property Codice_Operatore_Allegato As String
        Get
            Return _Codice_Operatore_Allegato
        End Get
        Set(value As String)
            _Codice_Operatore_Allegato = value
        End Set
    End Property
    Public Property Nominativo_Operatore_Allegato As String
        Get
            Return _Nominativo_Operatore_Allegato
        End Get
        Set(value As String)
            _Nominativo_Operatore_Allegato = value
        End Set
    End Property


End Class
