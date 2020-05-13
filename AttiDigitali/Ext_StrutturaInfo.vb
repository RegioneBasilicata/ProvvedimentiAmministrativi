
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_StrutturaInfo


    Private _codiceInterno As String = String.Empty
    Private _codicePubblico As String = String.Empty
    Private _descrizione As String = String.Empty
    Private _descrizioneBreve As String = String.Empty
    Private _descrizioneToDisplay As String = String.Empty
    Private _padre As String = String.Empty
    Private _tipologia As String = String.Empty


    <System.Runtime.Serialization.DataMember()> _
 Property CodiceInterno() As String
        Get
            Return _codiceInterno
        End Get
        Set(ByVal value As String)
            _codiceInterno = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodicePubblico() As String
        Get
            Return _codicePubblico
        End Get
        Set(ByVal value As String)
            _codicePubblico = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property DescrizioneBreve() As String
        Get
            Return _descrizioneBreve
        End Get
        Set(ByVal value As String)
            _descrizioneBreve = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property DescrizioneToDisplay() As String
        Get
            Return _descrizioneToDisplay
        End Get
        Set(ByVal value As String)
            _descrizioneToDisplay = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Padre() As String
        Get
            Return _padre
        End Get
        Set(ByVal value As String)
            _padre = value
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
    Public Sub New()

    End Sub
End Class
