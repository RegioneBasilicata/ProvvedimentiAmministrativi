Public Class ItemSchedaLeggeTrasparenzaInfo
    Private _idDocumento As String = String.Empty
    Private _autorizzazionePubblicazione As Boolean = True
    Private _notePubblicazione As String = String.Empty
    Private _normaAttribuzioneBeneficio As String = String.Empty
    Private _ufficioResponsabileProcedimento As String = String.Empty
    Private _funzionarioResponsabileProcedimento As String = String.Empty
    Private _modalitaIndividuazioneBeneficiario As String = String.Empty
    Private _contenutoAtto As String = String.Empty
    Private _contratti As Generic.List(Of ItemContrattoInfoHeader) = New Generic.List(Of ItemContrattoInfoHeader)

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
            set_idDocument_onContratti()
        End Set
    End Property

    Public Property AutorizzazionePubblicazione() As Boolean
        Get
            Return _autorizzazionePubblicazione
        End Get
        Set(ByVal value As Boolean)
            _autorizzazionePubblicazione = value
        End Set
    End Property
    Public Property NotePubblicazione() As String
        Get
            Return _notePubblicazione
        End Get
        Set(ByVal value As String)
            _notePubblicazione = value
        End Set
    End Property

    Public Property ContenutoAtto() As String
        Get
            Return _contenutoAtto
        End Get
        Set(ByVal value As String)
            _contenutoAtto = value
        End Set
    End Property

    Public Property NormaAttribuzioneBeneficio() As String
        Get
            Return _normaAttribuzioneBeneficio
        End Get
        Set(ByVal value As String)
            _normaAttribuzioneBeneficio = value
        End Set
    End Property
    Public Property UfficioResponsabileProcedimento() As String
        Get
            Return _ufficioResponsabileProcedimento
        End Get
        Set(ByVal value As String)
            _ufficioResponsabileProcedimento = value
        End Set
    End Property
    Public Property FunzionarioResponsabileProcedimento() As String
        Get
            Return _funzionarioResponsabileProcedimento
        End Get
        Set(ByVal value As String)
            _funzionarioResponsabileProcedimento = value
        End Set
    End Property
    Public Property ModalitaIndividuazioneBeneficiario() As String
        Get
            Return _modalitaIndividuazioneBeneficiario
        End Get
        Set(ByVal value As String)
            _modalitaIndividuazioneBeneficiario = value
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

    Private Sub set_idDocument_onContratti()
        If Not Contratti Is Nothing Then
            For Each contratto As ItemContrattoInfoHeader In Contratti
                If Not contratto Is Nothing Then
                    contratto.IdDocumento = IdDocumento
                End If
            Next
        End If
    End Sub
End Class
