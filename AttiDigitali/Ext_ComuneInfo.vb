
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_ComuneInfo

    Private _codiceIstat As String = String.Empty
    Private _descrizione As String = String.Empty
    Private _provincia As String = String.Empty
    Private _CodProvincia As String = String.Empty
    Private _regione As String = String.Empty
    Private _id As String = String.Empty
    Private _cap As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
 Property CodiceIstat() As String
        Get
            Return _codiceIstat
        End Get
        Set(ByVal value As String)
            _codiceIstat = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Provincia() As String
        Get
            Return _provincia
        End Get
        Set(ByVal value As String)
            _provincia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Property CodProvincia() As String
        Get
            Return _CodProvincia
        End Get
        Set(ByVal value As String)
            _CodProvincia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Property Regione() As String
        Get
            Return _regione
        End Get
        Set(ByVal value As String)
            _regione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Cap() As String
        Get
            Return _cap
        End Get
        Set(ByVal value As String)
            _cap = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
