
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SceltaOperatoreInfo


    Private _codiceOperatore As String = String.Empty
    Private _codiceUfficio As String = String.Empty
    Private _codiceUfficioPubblico As String = String.Empty
    Private _descrizioneUfficio As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceUfficioPubblico() As String
        Get
            Return _codiceUfficioPubblico
        End Get
        Set(ByVal value As String)
            _codiceUfficioPubblico = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceUfficio() As String
        Get
            Return _codiceUfficio
        End Get
        Set(ByVal value As String)
            _codiceUfficio = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property DescrizioneUfficio() As String
        Get
            Return _descrizioneUfficio
        End Get
        Set(ByVal value As String)
            _descrizioneUfficio = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodiceOperatore() As String
        Get
            Return _codiceOperatore
        End Get
        Set(ByVal value As String)
            _codiceOperatore = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
