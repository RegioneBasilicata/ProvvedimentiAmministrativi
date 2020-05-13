<System.Runtime.Serialization.DataContract()> _
    Public Class Ext_RelatoreInfo

    Private _Tr_id As String = String.Empty
    Private _Tr_Cognome As String = String.Empty
    Private _Tr_Nome As String = String.Empty
    Private _Tr_Ordine_Apparizione As String = String.Empty
    Private _Tr_Carica As String = String.Empty
    Private _Tr_attivo As Boolean
    Private _Tr_dataattivazione As String = String.Empty
    Private _Tr_datadisattivazione As String = String.Empty
    Private _Tr_IdStruttura As String = String.Empty
    Private _IsPresente As Boolean

    <System.Runtime.Serialization.DataMember()> _
    Property Tr_id() As String
        Get
            Return _Tr_id
        End Get
        Set(ByVal Value As String)
            _Tr_id = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Property Tr_IdStruttura() As String
        Get
            Return _Tr_IdStruttura
        End Get
        Set(ByVal Value As String)
            _Tr_IdStruttura = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_Cognome() As String
        Get
            Return _Tr_Cognome
        End Get
        Set(ByVal Value As String)
            _Tr_Cognome = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_Nome() As String
        Get
            Return _Tr_Nome
        End Get
        Set(ByVal Value As String)
            _Tr_Nome = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_Ordine_Apparizione() As String
        Get
            Return _Tr_Ordine_Apparizione
        End Get
        Set(ByVal Value As String)
            _Tr_Ordine_Apparizione = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_attivo() As Boolean
        Get
            Return _Tr_attivo
        End Get
        Set(ByVal Value As Boolean)
            _Tr_attivo = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_Carica() As String
        Get
            Return _Tr_Carica
        End Get
        Set(ByVal Value As String)
            _Tr_Carica = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_dataattivazione() As String
        Get
            Return _Tr_dataattivazione
        End Get
        Set(ByVal Value As String)
            _Tr_dataattivazione = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tr_datadisattivazione() As String
        Get
            Return _Tr_datadisattivazione
        End Get
        Set(ByVal Value As String)
            _Tr_datadisattivazione = Value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property IsPresente() As Boolean
        Get
            Return _IsPresente
        End Get
        Set(ByVal Value As Boolean)
            _IsPresente = Value
        End Set
    End Property
    
    Public Sub New()
    End Sub
End Class
