
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SedeAnagraficaInfo
    Private _idSede As String = String.Empty
    Private _nomeSede As String = String.Empty
    Private _indirizzo As String = String.Empty
    Private _comune As String = String.Empty
    Private _provincia As String = String.Empty
    Private _capComune As String = String.Empty
    Private _telefono As String = String.Empty
    Private _fax As String = String.Empty
    Private _email As String = String.Empty
    Private _bollo As Boolean = False
    Private _idModalitaPagamento As String = String.Empty
    Private _modalitaPagamento As String = String.Empty
    Private _hasDatiBancari As Boolean = False
    Private _istitutoRiferimento As String = String.Empty
    Private _DatiBancari As Generic.List(Of Ext_DatiBancariInfo)
    <System.Runtime.Serialization.DataMember()> _
 Property IdSede() As String
        Get
            Return _idSede
        End Get
        Set(ByVal value As String)
            _idSede = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property NomeSede() As String
        Get
            Return _nomeSede
        End Get
        Set(ByVal value As String)
            _nomeSede = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Indirizzo() As String
        Get
            Return _indirizzo
        End Get
        Set(ByVal value As String)
            _indirizzo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Comune() As String
        Get
            Return _comune
        End Get
        Set(ByVal value As String)
            _comune = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property CapComune() As String
        Get
            Return _capComune
        End Get
        Set(ByVal value As String)
            _capComune = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Telefono() As String
        Get
            Return _telefono
        End Get
        Set(ByVal value As String)
            _telefono = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Fax() As String
        Get
            Return _fax
        End Get
        Set(ByVal value As String)
            _fax = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Bollo() As Boolean
        Get
            Return _bollo
        End Get
        Set(ByVal value As Boolean)
            _bollo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ModalitaPagamento() As String
        Get
            Return _modalitaPagamento
        End Get
        Set(ByVal value As String)
            _modalitaPagamento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Property IdModalitaPagamento() As String
        Get
            Return _idModalitaPagamento
        End Get
        Set(ByVal value As String)
            _idModalitaPagamento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property DatiBancari() As Generic.List(Of Ext_DatiBancariInfo)
        Get
            Return _DatiBancari
        End Get
        Set(ByVal value As Generic.List(Of Ext_DatiBancariInfo))
            _DatiBancari = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property HasDatiBancari() As Boolean
        Get
            Return _hasDatiBancari
        End Get
        Set(ByVal value As Boolean)
            _hasDatiBancari = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property IstitutoRiferimento() As String
        Get
            Return _istitutoRiferimento
        End Get
        Set(ByVal value As String)
            _istitutoRiferimento = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
