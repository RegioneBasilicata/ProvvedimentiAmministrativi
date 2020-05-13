<System.Runtime.Serialization.DataContract()> _
Public Class Ext_DettaglioStorico

    Private _ID As Integer
    Private _IdDocumento As String = String.Empty
    Private _Progressivo As Integer
    Private _IdUfficio As String = String.Empty
    Private _CodiceUfficio As String = String.Empty
    Private _DescrizioneUfficio As String = String.Empty
    Private _DenominazUfficioDaVisualizz As String = String.Empty
    Private _Giorni As Integer
    Private _DataArrivo As String
    Private _DataUscita As String
    Private _Utente As String = String.Empty
    Private _Stato As String = String.Empty


    <System.Runtime.Serialization.DataMember()> _
  Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As String)
            _IdDocumento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property Progressivo() As Integer
        Get
            Return _Progressivo
        End Get
        Set(ByVal value As Integer)
            _Progressivo = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property IdUfficio() As String
        Get
            Return _IdUfficio
        End Get
        Set(ByVal value As String)
            _IdUfficio = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property CodiceUfficio() As String
        Get
            Return _CodiceUfficio
        End Get
        Set(ByVal value As String)
            _CodiceUfficio = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property DescrizioneUfficio() As String
        Get
            Return _DescrizioneUfficio
        End Get
        Set(ByVal value As String)
            _DescrizioneUfficio = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property DenominazUfficioDaVisualizz() As String
        Get
            Return _DenominazUfficioDaVisualizz
        End Get
        Set(ByVal value As String)
            _DenominazUfficioDaVisualizz = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property Giorni() As Integer
        Get
            Return _Giorni
        End Get
        Set(ByVal value As Integer)
            _Giorni = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property DataArrivo() As String
        Get
            Return _DataArrivo
        End Get
        Set(ByVal value As String)
            _DataArrivo = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property DataUscita() As String
        Get
            Return _DataUscita
        End Get
        Set(ByVal value As String)
            _DataUscita = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property Utente() As String
        Get
            Return _Utente
        End Get
        Set(ByVal value As String)
            _Utente = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property Stato() As String
        Get
            Return _Stato
        End Get
        Set(ByVal value As String)
            _Stato = value
        End Set
    End Property

    Public Sub New()
    End Sub
   
End Class
