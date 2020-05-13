
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_DatiBancariInfo
    Private _idContoCorrente As String = String.Empty
    Private _nomeBanca As String = String.Empty
    Private _contoCorrente As String = String.Empty
    Private _modalitaPrincipale As Boolean = False
    Private _sedeAgenzia As String = String.Empty
    Private _indirizzoAgenzia As String = String.Empty
    Private _cittaAgenzia As String = String.Empty
    Private _capCittaAgenzia As String = String.Empty
    Private _provinciaAgenzia As String = String.Empty
    Private _abi As String = String.Empty
    Private _cab As String = String.Empty
    Private _cin As String = String.Empty
    Private _iban As String = String.Empty
    Private _idAgenzia As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
 Property IdContoCorrente() As String
        Get
            Return _idContoCorrente
        End Get
        Set(ByVal value As String)
            _idContoCorrente = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property NomeBanca() As String
        Get
            Return _nomeBanca
        End Get
        Set(ByVal value As String)
            _nomeBanca = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ContoCorrente() As String
        Get
            Return _contoCorrente
        End Get
        Set(ByVal value As String)
            _contoCorrente = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ModalitaPrincipale() As Boolean
        Get
            Return _modalitaPrincipale
        End Get
        Set(ByVal value As Boolean)
            _modalitaPrincipale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Property SedeAgenzia() As String
        Get
            Return _sedeAgenzia
        End Get
        Set(ByVal value As String)
            _sedeAgenzia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property IndirizzoAgenzia() As String
        Get
            Return _indirizzoAgenzia
        End Get
        Set(ByVal value As String)
            _indirizzoAgenzia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property CittaAgenzia() As String
        Get
            Return _cittaAgenzia
        End Get
        Set(ByVal value As String)
            _cittaAgenzia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property CapCittaAgenzia() As String
        Get
            Return _capCittaAgenzia
        End Get
        Set(ByVal value As String)
            _capCittaAgenzia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ProvinciaAgenzia() As String
        Get
            Return _provinciaAgenzia
        End Get
        Set(ByVal value As String)
            _provinciaAgenzia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Abi() As String
        Get
            Return _abi
        End Get
        Set(ByVal value As String)
            _abi = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Cab() As String
        Get
            Return _cab
        End Get
        Set(ByVal value As String)
            _cab = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Cin() As String
        Get
            Return _cin
        End Get
        Set(ByVal value As String)
            _cin = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Property Iban() As String
        Get
            Return _iban
        End Get
        Set(ByVal value As String)
            _iban = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property IdAgenzia() As String
        Get
            Return _idAgenzia
        End Get
        Set(ByVal value As String)
            _idAgenzia = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
