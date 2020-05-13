Public Class ItemSchedaTipologiaProvvedimentoInfo
    Private _idDocumento As String = String.Empty
    Private _idTipologiaProvvedimento As Integer = -1
    Private _importoSpesaPrevista As Nullable(Of Decimal) = Nothing
    Private _isSommaAutomatica As Boolean = False
    Private _destinatari As Generic.List(Of ItemDestinatarioInfo) = New Generic.List(Of ItemDestinatarioInfo)

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
            set_idDocument_onDestinatari()
        End Set
    End Property

    Public Property IdTipologiaProvvedimento() As Integer
        Get
            Return _idTipologiaProvvedimento
        End Get
        Set(ByVal value As Integer)
            _idTipologiaProvvedimento = value
        End Set
    End Property

    Public Property ImportoSpesaPrevista() As Nullable(Of Decimal)
        Get
            Return _importoSpesaPrevista
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _importoSpesaPrevista = value
        End Set
    End Property

    Property isSommaAutomatica() As Boolean
        Get
            Return _isSommaAutomatica
        End Get
        Set(ByVal value As Boolean)
            _isSommaAutomatica = value
        End Set
    End Property

    Property Destinatari() As Generic.List(Of ItemDestinatarioInfo)
        Get
            Return _destinatari
        End Get
        Set(ByVal Value As Generic.List(Of ItemDestinatarioInfo))
            _destinatari = Value
            set_idDocument_onDestinatari()
        End Set
    End Property

    Private Sub set_idDocument_onDestinatari()
        If Not Destinatari Is Nothing Then
            For Each destinatario As ItemDestinatarioInfo In Destinatari
                If Not destinatario Is Nothing Then
                    destinatario.IdDocumento = IdDocumento
                End If
            Next
        End If
    End Sub
End Class
