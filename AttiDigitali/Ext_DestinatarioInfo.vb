<System.Runtime.Serialization.DataContract()> _
Public Class Ext_DestinatarioInfo
    Private _Id As Long = -1
    Private _IdDocumento As String = String.Empty
    Private _IdSIC As String = String.Empty
    Private _Denominazione As String = String.Empty
    Private _CodiceFiscale As String = String.Empty
    Private _PartitaIva As String = String.Empty
    Private _isPersonaFisica As Boolean = False
    Private _DataNascita As String = String.Empty
    Private _LuogoNascita As String = String.Empty
    Private _LegaleRappresentante As String = String.Empty
    Private _IdContratto As String = String.Empty
    Private _NumeroRepertorioContratto As String = String.Empty
    Private _isDatoSensibile As Boolean = False
    Private _ImportoSpettante As Decimal = 0


    <System.Runtime.Serialization.DataMember()> _
    Property Id() As Long
        Get
            Return _Id
        End Get
        Set(ByVal value As Long)
            _Id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property IdSIC() As String
        Get
            Return _IdSIC
        End Get
        Set(ByVal value As String)
            _IdSIC = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As String)
            _IdDocumento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Denominazione() As String
        Get
            Return _Denominazione
        End Get
        Set(ByVal value As String)
            _Denominazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property PartitaIva() As String
        Get
            Return _PartitaIva
        End Get
        Set(ByVal value As String)
            _PartitaIva = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property isPersonaFisica() As Boolean
        Get
            Return _isPersonaFisica
        End Get
        Set(ByVal value As Boolean)
            _isPersonaFisica = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceFiscale() As String
        Get
            Return _CodiceFiscale
        End Get
        Set(ByVal value As String)
            _CodiceFiscale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property DataNascita() As String
        Get
            Return _DataNascita
        End Get
        Set(ByVal value As String)
            _DataNascita = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property LuogoNascita() As String
        Get
            Return _LuogoNascita
        End Get
        Set(ByVal value As String)
            _LuogoNascita = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property LegaleRappresentante() As String
        Get
            Return _LegaleRappresentante
        End Get
        Set(ByVal value As String)
            _LegaleRappresentante = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdContratto() As String
        Get
            Return _IdContratto
        End Get
        Set(ByVal value As String)
            _IdContratto = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property NumeroRepertorioContratto() As String
        Get
            Return _NumeroRepertorioContratto
        End Get
        Set(ByVal value As String)
            _NumeroRepertorioContratto = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property isDatoSensibile() As Boolean
        Get
            Return _isDatoSensibile
        End Get
        Set(ByVal value As Boolean)
            _isDatoSensibile = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property ImportoSpettante() As Decimal
        Get
            Return _ImportoSpettante
        End Get
        Set(ByVal value As Decimal)
            _ImportoSpettante = value
        End Set
    End Property

End Class

