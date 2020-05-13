Public Class BeneficiarioCronologia
    Private _codOperatore As String
    Private _idBeneficiario As String
    Private _idSede As String
    Private _idTipoPagamento As String
    Private _idContoCorrente As String

    Private _flagPersonaFisica As Boolean
    Private _nominativo As String
    Private _codFiscPIva As String
    Private _dataNasc As DateTime
    Private _luogoNasc As String
    Private _legaleRappresentante As String

    Private _descrSede As String
    Private _descrModPagamento As String
    Private _descrDatiBancari As String

    Private _dataUltimoUtilizzo As DateTime
    Private _contatoreFrequenza As Integer


    Public Property CodOperatore() As String
        Get
            Return _codOperatore
        End Get
        Set(ByVal value As String)
            _codOperatore = value
        End Set
    End Property

    Public Property IdBeneficiario() As String
        Get
            Return _idBeneficiario
        End Get
        Set(ByVal value As String)
            _idBeneficiario = value
        End Set
    End Property
    Public Property IdSede() As String
        Get
            Return _idSede
        End Get
        Set(ByVal value As String)
            _idSede = value
        End Set
    End Property
    Public Property IdTipoPagamento() As String
        Get
            Return _idTipoPagamento
        End Get
        Set(ByVal value As String)
            _idTipoPagamento = value
        End Set
    End Property
    Public Property IdContoCorrente() As String
        Get
            Return _idContoCorrente
        End Get
        Set(ByVal value As String)
            _idContoCorrente = value
        End Set
    End Property


    Public Property FlagPersonaFisica() As Boolean
        Get
            Return _flagPersonaFisica
        End Get
        Set(ByVal value As Boolean)
            _flagPersonaFisica = value
        End Set
    End Property
    Public Property Nominativo() As String
        Get
            Return _nominativo
        End Get
        Set(ByVal value As String)
            _nominativo = value
        End Set
    End Property
    Public Property CodFiscPIva() As String
        Get
            Return _codFiscPIva
        End Get
        Set(ByVal value As String)
            _codFiscPIva = value
        End Set
    End Property
    Public Property DataNasc() As DateTime
        Get
            Return _dataNasc
        End Get
        Set(ByVal value As DateTime)
            _dataNasc = value
        End Set
    End Property
    Public Property LuogoNasc() As String
        Get
            Return _luogoNasc
        End Get
        Set(ByVal value As String)
            _luogoNasc = value
        End Set
    End Property
    Public Property LegaleRappresentante() As String
        Get
            Return _legaleRappresentante
        End Get
        Set(ByVal value As String)
            _legaleRappresentante = value
        End Set
    End Property

    Public Property DescrSede() As String
        Get
            Return _descrSede
        End Get
        Set(ByVal value As String)
            _descrSede = value
        End Set
    End Property
    Public Property DescrDatiBancari() As String
        Get
            Return _descrDatiBancari
        End Get
        Set(ByVal value As String)
            _descrDatiBancari = value
        End Set
    End Property
    Public Property DescrModPagamento() As String
        Get
            Return _descrModPagamento
        End Get
        Set(ByVal value As String)
            _descrModPagamento = value
        End Set
    End Property
    Public Property DataUltimoUtilizzo() As DateTime
        Get
            Return _dataUltimoUtilizzo
        End Get
        Set(ByVal value As DateTime)
            _dataUltimoUtilizzo = value
        End Set
    End Property
    Public Property ContatoreFrequenza() As Integer
        Get
            Return _contatoreFrequenza
        End Get
        Set(ByVal value As Integer)
            _contatoreFrequenza = value
        End Set
    End Property
End Class
