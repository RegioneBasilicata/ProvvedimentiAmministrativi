
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_LegaleRappresentanteInfo
    Private _cognome As String = String.Empty
    Private _nome As String = String.Empty
    Private _tipologia As String = String.Empty
    Private _sesso As String = String.Empty
    Private _comuneNascita As String = String.Empty
    Private _dataNascita As String = String.Empty
    Private _indirizzo As String = String.Empty
    Private _comuneResidenza As String = String.Empty
    Private _capResidenza As String = String.Empty
    Private _codiceFiscale As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
 Property Cognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal value As String)
            _cognome = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Sesso() As String
        Get
            Return _sesso
        End Get
        Set(ByVal value As String)
            _sesso = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property ComuneNascita() As String
        Get
            Return _comuneNascita
        End Get
        Set(ByVal value As String)
            _comuneNascita = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property DataNascita() As String
        Get
            Return _dataNascita
        End Get
        Set(ByVal value As String)
            _dataNascita = value
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
Property ComuneResidenza() As String
        Get
            Return _comuneResidenza
        End Get
        Set(ByVal value As String)
            _comuneResidenza = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property CapResidenza() As String
        Get
            Return _capResidenza
        End Get
        Set(ByVal value As String)
            _capResidenza = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property CodiceFiscale() As String
        Get
            Return _codiceFiscale
        End Get
        Set(ByVal value As String)
            _codiceFiscale = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
