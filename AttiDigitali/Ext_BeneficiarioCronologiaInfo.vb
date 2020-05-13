<System.Runtime.Serialization.DataContract()> _
Public Class Ext_BeneficiarioCronologiaInfo

    Private _idBeneficiario As String = String.Empty
    Private _idSede As String = String.Empty
    Private _idTipoPagamento As String = String.Empty
    Private _idContoCorrente As String = String.Empty

    Private _flagPersonaFisica As Boolean
    Private _nominativo As String = String.Empty
    Private _codFiscPIva As String = String.Empty
    Private _dataNasc As String = String.Empty
    Private _luogoNasc As String = String.Empty
    Private _legaleRappresentante As String = String.Empty

    Private _descrSede As String = String.Empty
    Private _descrModPagamento As String = String.Empty
    Private _descrDatiBancari As String = String.Empty

    Private _dataUltimoUtilizzo As String = String.Empty
    Private _contatoreFrequenza As Integer

    <System.Runtime.Serialization.DataMember()> _
    Public Property IdBeneficiario() As String
        Get
            Return _idBeneficiario
        End Get
        Set(ByVal value As String)
            _idBeneficiario = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property IdSede() As String
        Get
            Return _idSede
        End Get
        Set(ByVal value As String)
            _idSede = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property IdTipoPagamento() As String
        Get
            Return _idTipoPagamento
        End Get
        Set(ByVal value As String)
            _idTipoPagamento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property IdContoCorrente() As String
        Get
            Return _idContoCorrente
        End Get
        Set(ByVal value As String)
            _idContoCorrente = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property FlagPersonaFisica() As Boolean
        Get
            Return _flagPersonaFisica
        End Get
        Set(ByVal value As Boolean)
            _flagPersonaFisica = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property Nominativo() As String
        Get
            Return _nominativo
        End Get
        Set(ByVal value As String)
            _nominativo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property CodFiscPIva() As String
        Get
            Return _codFiscPIva
        End Get
        Set(ByVal value As String)
            _codFiscPIva = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property DataNasc() As String
        Get
            Return _dataNasc
        End Get
        Set(ByVal value As String)
            _dataNasc = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property LuogoNasc() As String
        Get
            Return _luogoNasc
        End Get
        Set(ByVal value As String)
            _luogoNasc = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property LegaleRappresentante() As String
        Get
            Return _legaleRappresentante
        End Get
        Set(ByVal value As String)
            _legaleRappresentante = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property DescrSede() As String
        Get
            Return _descrSede
        End Get
        Set(ByVal value As String)
            _descrSede = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property DescrDatiBancari() As String
        Get
            Return _descrDatiBancari
        End Get
        Set(ByVal value As String)
            _descrDatiBancari = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property DescrModPagamento() As String
        Get
            Return _descrModPagamento
        End Get
        Set(ByVal value As String)
            _descrModPagamento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property DataUltimoUtilizzo() As String
        Get
            Return _dataUltimoUtilizzo
        End Get
        Set(ByVal value As String)
            _dataUltimoUtilizzo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Public Property ContatoreFrequenza() As String
        Get
            Return _contatoreFrequenza
        End Get
        Set(ByVal value As String)
            _contatoreFrequenza = value
        End Set
    End Property

End Class
