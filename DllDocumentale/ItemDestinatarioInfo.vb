Public Class ItemDestinatarioInfo
    Private _Id As Long = -1
    Private _IdDocumento As String = String.Empty
    Private _DataOperazione As Nullable(Of DateTime) = Nothing
    Private _Operatore As String = String.Empty
    Private _IdSIC As String = String.Empty
    Private _Denominazione As String = String.Empty
    Private _CodiceFiscale As String = String.Empty
    Private _PartitaIva As String = String.Empty
    Private _isPersonaFisica As Boolean = False
    Private _DataNascita As Nullable(Of DateTime) = Nothing
    Private _LuogoNascita As String = String.Empty
    Private _LegaleRappresentante As String = String.Empty
    Private _IdContratto As String = String.Empty
    Private _NumeroRepertorioContratto As String = String.Empty
    Private _isDatoSensibile As Boolean = False
    Private _ImportoSpettante As Decimal = 0

    Public Property Id() As Long
        Get
            Return _Id
        End Get
        Set(ByVal value As Long)
            _Id = value
        End Set
    End Property

    Public Property DataOperazione() As Nullable(Of DateTime)
        Get
            Return _DataOperazione
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _DataOperazione = value
        End Set
    End Property

    Public Property Operatore() As String
        Get
            Return _Operatore
        End Get
        Set(ByVal value As String)
            _Operatore = value
        End Set
    End Property

    Public Property IdSIC() As String
        Get
            Return _IdSIC
        End Get
        Set(ByVal value As String)
            _IdSIC = value
        End Set
    End Property

    Public Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As String)
            _IdDocumento = value
        End Set
    End Property

    Public Property Denominazione() As String
        Get
            Return _Denominazione
        End Get
        Set(ByVal value As String)
            _Denominazione = value
        End Set
    End Property

    Public Property PartitaIva() As String
        Get
            Return _PartitaIva
        End Get
        Set(ByVal value As String)
            _PartitaIva = value
        End Set
    End Property

    Public Property isPersonaFisica() As Boolean
        Get
            Return _isPersonaFisica
        End Get
        Set(ByVal value As Boolean)
            _isPersonaFisica = value
        End Set
    End Property

    Public Property CodiceFiscale() As String
        Get
            Return _CodiceFiscale
        End Get
        Set(ByVal value As String)
            _CodiceFiscale = value
        End Set
    End Property

    Public Property DataNascita() As Nullable(Of DateTime)
        Get
            Return _DataNascita
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _DataNascita = value
        End Set
    End Property

    Public Property LuogoNascita() As String
        Get
            Return _LuogoNascita
        End Get
        Set(ByVal value As String)
            _LuogoNascita = value
        End Set
    End Property

    Public Property LegaleRappresentante() As String
        Get
            Return _LegaleRappresentante
        End Get
        Set(ByVal value As String)
            _LegaleRappresentante = value
        End Set
    End Property

    Public Property IdContratto() As String
        Get
            Return _IdContratto
        End Get
        Set(ByVal value As String)
            _IdContratto = value
        End Set
    End Property

    Public Property NumeroRepertorioContratto() As String
        Get
            Return _NumeroRepertorioContratto
        End Get
        Set(ByVal value As String)
            _NumeroRepertorioContratto = value
        End Set
    End Property

    Public Property isDatoSensibile() As Boolean
        Get
            Return _isDatoSensibile
        End Get
        Set(ByVal value As Boolean)
            _isDatoSensibile = value
        End Set
    End Property

    Public Property ImportoSpettante() As Decimal
        Get
            Return _ImportoSpettante
        End Get
        Set(ByVal value As Decimal)
            _ImportoSpettante = value
        End Set
    End Property
End Class

