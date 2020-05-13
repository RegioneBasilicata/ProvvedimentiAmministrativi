
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_AttributoInfo
    Private _id As String = String.Empty
    Private _descrizione As String = String.Empty
    Private _tipoDato As String = String.Empty
    Private _valore As String = String.Empty
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
    Property Descrizione() As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property Valore() As String
        Get
            Return _valore
        End Get
        Set(ByVal value As String)
            _valore = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property TipoDato() As String
        Get
            Return _tipoDato
        End Get
        Set(ByVal value As String)
            _tipoDato = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
