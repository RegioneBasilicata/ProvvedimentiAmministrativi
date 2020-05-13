<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SchedaContrattiFattureInfo
    Private _contratti As Generic.List(Of Ext_ContrattoInfo)

    Private _fatture As Generic.List(Of Ext_FatturaInfo)


    <System.Runtime.Serialization.DataMember()> _
    Public Property Contratti() As Generic.List(Of Ext_ContrattoInfo)
        Get
            Return _contratti
        End Get
        Set(ByVal value As Generic.List(Of Ext_ContrattoInfo))
            _contratti = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property Fatture() As Generic.List(Of Ext_FatturaInfo)
        Get
            Return _fatture
        End Get
        Set(ByVal value As Generic.List(Of Ext_FatturaInfo))
            _fatture = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

