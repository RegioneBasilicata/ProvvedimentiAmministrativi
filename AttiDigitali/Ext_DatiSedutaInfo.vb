<System.Runtime.Serialization.DataContract()> _
    Public Class Ext_DatiSedutaInfo

    Private _Doc_Id As String = String.Empty
    Private _Doc_numeroProvvisorio As String = String.Empty
    Private _Doc_numero As String = String.Empty
    Private _Doc_Oggetto As String = String.Empty
    Private _Doc_Cod_Uff_Prop As String = String.Empty
    Private _Doc_Descrizione_ufficio As String = String.Empty
    Private _Doc_Data As String = String.Empty
    Private _Doc_Tipo As Int32
    Private _Doc_IdTipologiaDocumento As Int32
    Private _Doc_TipologiaDocumento As String = String.Empty
    Private _Doc_DestinatariDocumento As Generic.List(Of Ext_DestinatarioInfo) = New Generic.List(Of Ext_DestinatarioInfo)
    Private _Doc_FlagDaPagare As Int32
    Private _TipoAtto As String = String.Empty
    Private _Numero As String = String.Empty
    Private _isConsultabile As Boolean = True

    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Id() As String
        Get
            Return _Doc_Id
        End Get
        Set(ByVal Value As String)
            _Doc_Id = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Property TipoAtto() As String
        Get
            Return _TipoAtto
        End Get
        Set(ByVal Value As String)
            _TipoAtto = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_NumeroProvvisorio() As String
        Get
            Return _Doc_numeroProvvisorio
        End Get
        Set(ByVal Value As String)
            _Doc_numeroProvvisorio = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Numero() As String
        Get
            Return _Doc_numero
        End Get
        Set(ByVal Value As String)
            _Doc_numero = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Oggetto() As String
        Get
            Return _Doc_Oggetto
        End Get
        Set(ByVal Value As String)
            _Doc_Oggetto = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Descrizione_ufficio() As String
        Get
            Return _Doc_Descrizione_ufficio
        End Get
        Set(ByVal Value As String)
            _Doc_Descrizione_ufficio = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Cod_Uff_Prop() As String
        Get
            Return _Doc_Cod_Uff_Prop
        End Get
        Set(ByVal Value As String)
            _Doc_Cod_Uff_Prop = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Data() As String
        Get
            Return _Doc_Data
        End Get
        Set(ByVal Value As String)
            _Doc_Data = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_Tipo() As Int32
        Get
            Return _Doc_Tipo
        End Get
        Set(ByVal Value As Int32)
            _Doc_Tipo = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_FlagDaPagare() As Int32
        Get
            Return _Doc_FlagDaPagare
        End Get
        Set(ByVal Value As Int32)
            _Doc_FlagDaPagare = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property IsConsultabile() As Boolean
        Get
            Return _isConsultabile
        End Get
        Set(ByVal Value As Boolean)
            _isConsultabile = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_IdTipologiaDocumento() As Int32
        Get
            Return _Doc_IdTipologiaDocumento
        End Get
        Set(ByVal Value As Int32)
            _Doc_IdTipologiaDocumento = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_TipologiaDocumento() As String
        Get
            Return _Doc_TipologiaDocumento
        End Get
        Set(ByVal Value As String)
            _Doc_TipologiaDocumento = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_DestinatariDocumento() As Generic.List(Of Ext_DestinatarioInfo)
        Get
            Return _Doc_DestinatariDocumento
        End Get
        Set(ByVal Value As Generic.List(Of Ext_DestinatarioInfo))
            _Doc_DestinatariDocumento = Value
        End Set
    End Property
    Public Sub New()
    End Sub
End Class
