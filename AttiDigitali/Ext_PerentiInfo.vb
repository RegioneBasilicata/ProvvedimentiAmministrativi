<System.Runtime.Serialization.DataContract()> _
Public Class Ext_PerentiInfo
    Inherits Ext_CapitoliAnnoInfo
    '------------- RIGA ELENCO

    Private _ImportoOriginario As Double = 0
    Private _CapitoloOriginario As String = String.Empty

    <System.Runtime.Serialization.DataMember()> _
    Public Property ImportoOriginario() As Double
        Get
            Return _ImportoOriginario
        End Get
        Set(ByVal value As Double)
            _ImportoOriginario = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Public Property CapitoloOriginario() As String
        Get
            Return _CapitoloOriginario
        End Get
        Set(ByVal value As String)
            _CapitoloOriginario = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
