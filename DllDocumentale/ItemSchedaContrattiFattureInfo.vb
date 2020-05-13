Public Class ItemSchedaContrattiFattureInfo
    Private _idDocumento As String = String.Empty
    Private _contratti As Generic.List(Of ItemContrattoInfoHeader) = New Generic.List(Of ItemContrattoInfoHeader)
    Private _fatture As Generic.List(Of ItemFatturaInfoHeader) = New Generic.List(Of ItemFatturaInfoHeader)

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
            set_idDocument_onContratti()
        End Set
    End Property

    Property Contratti() As Generic.List(Of ItemContrattoInfoHeader)
        Get
            Return _contratti
        End Get
        Set(ByVal Value As Generic.List(Of ItemContrattoInfoHeader))
            _contratti = Value
            set_idDocument_onContratti()
        End Set
    End Property

    Property Fatture() As Generic.List(Of ItemFatturaInfoHeader)
        Get
            Return _fatture
        End Get
        Set(ByVal Value As Generic.List(Of ItemFatturaInfoHeader))
            _fatture = Value
            set_idDocument_onContratti()
        End Set
    End Property

    Private Sub set_idDocument_onContratti()
        If Not Contratti Is Nothing Then
            For Each contratto As ItemContrattoInfoHeader In Contratti
                If Not contratto Is Nothing Then
                    contratto.IdDocumento = IdDocumento
                End If
            Next
        End If
        If Not Fatture Is Nothing Then
            For Each fattura As ItemFatturaInfoHeader In Fatture
                If Not fattura Is Nothing Then
                    fattura.IdDocumento = IdDocumento
                End If
            Next
        End If
    End Sub

End Class
