
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_TipologiaDocumento
    Private _id As String = String.Empty
    Private _tipologia As String = String.Empty
    Private _hasDestinatari As Boolean = False
    Private _hasDestinatariObbligatori As Boolean = False
    <System.Runtime.Serialization.DataMember()> _
 Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property HasDestinatari() As Boolean
        Get
            Return _hasDestinatari
        End Get
        Set(ByVal value As Boolean)
            _hasDestinatari = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property HasDestinatariObbligatori() As Boolean
        Get
            Return _hasDestinatariObbligatori
        End Get
        Set(ByVal value As Boolean)
            _hasDestinatariObbligatori = value
        End Set
    End Property


    Public Sub New()

    End Sub
End Class
