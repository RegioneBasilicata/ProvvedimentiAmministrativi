<System.Runtime.Serialization.DataContract()> _
Public Class Ext_ContrattoInfo
    Inherits Ext_ContrattoInfoHeader

    Private _oggetto As String
    Private _codiceCUP As String
    Private _codiceCIG As String

    <System.Runtime.Serialization.DataMember()> _
    Property Oggetto() As String
        Get
            Return _oggetto
        End Get
        Set(ByVal value As String)
            _oggetto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceCUP() As String
        Get
            Return _codiceCUP
        End Get
        Set(ByVal value As String)
            _codiceCUP = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Property CodiceCIG() As String
        Get
            Return _codiceCIG
        End Get
        Set(ByVal value As String)
            _codiceCIG = value
        End Set
    End Property
End Class

