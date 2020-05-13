<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SchedaTipologiaProvvedimentoInfo
    Private _idDocumento As String
    Private _idTipologiaProvvedimento As Integer
    Private _importoSpesaPrevista As Nullable(Of Double) = Nothing
    Private _isSommaAutomatica As Boolean = False
    Private _destinatari As Generic.List(Of Ext_DestinatarioInfo)

    <System.Runtime.Serialization.DataMember()> _
    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property IdTipologiaProvvedimento() As Integer
        Get
            Return _idTipologiaProvvedimento
        End Get
        Set(ByVal value As Integer)
            _idTipologiaProvvedimento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property ImportoSpesaPrevista() As Nullable(Of Double)
        Get
            Return _importoSpesaPrevista
        End Get
        Set(ByVal value As Nullable(Of Double))
            _importoSpesaPrevista = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property Destinatari() As Generic.List(Of Ext_DestinatarioInfo)
        Get
            Return _destinatari
        End Get
        Set(ByVal value As Generic.List(Of Ext_DestinatarioInfo))
            _destinatari = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
   Property isSommaAutomatica() As Boolean
        Get
            Return _isSommaAutomatica
        End Get
        Set(ByVal value As Boolean)
            _isSommaAutomatica = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

