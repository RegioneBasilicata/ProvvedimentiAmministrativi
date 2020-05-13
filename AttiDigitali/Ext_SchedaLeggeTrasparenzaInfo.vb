<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SchedaLeggeTrasparenzaInfo
    Private _idDocumento As String
    Private _autorizzazionePubblicazione As Boolean
    Private _notePubblicazione As String
    Private _normaAttribuzioneBeneficio As String
    Private _ufficioResponsabileProcedimento As String
    Private _funzionarioResponsabileProcedimento As String
    Private _modalitaIndividuazioneBeneficiario As String
    Private _contenutoAtto As String
    Private _contratti As Generic.List(Of Ext_ContrattoInfo)

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
    Public Property AutorizzazionePubblicazione() As Boolean
        Get
            Return _autorizzazionePubblicazione
        End Get
        Set(ByVal value As Boolean)
            _autorizzazionePubblicazione = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property ContenutoAtto() As String
        Get
            Return _contenutoAtto
        End Get
        Set(ByVal value As String)
            _contenutoAtto = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
   Public Property NotePubblicazione() As String
        Get
            Return _notePubblicazione
        End Get
        Set(ByVal value As String)
            _notePubblicazione = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property NormaAttribuzioneBeneficio() As String
        Get
            Return _normaAttribuzioneBeneficio
        End Get
        Set(ByVal value As String)
            _normaAttribuzioneBeneficio = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property UfficioResponsabileProcedimento() As String
        Get
            Return _ufficioResponsabileProcedimento
        End Get
        Set(ByVal value As String)
            _ufficioResponsabileProcedimento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property FunzionarioResponsabileProcedimento() As String
        Get
            Return _funzionarioResponsabileProcedimento
        End Get
        Set(ByVal value As String)
            _funzionarioResponsabileProcedimento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property ModalitaIndividuazioneBeneficiario() As String
        Get
            Return _modalitaIndividuazioneBeneficiario
        End Get
        Set(ByVal value As String)
            _modalitaIndividuazioneBeneficiario = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Public Property Contratti() As Generic.List(Of Ext_ContrattoInfo)
        Get
            Return _contratti
        End Get
        Set(ByVal value As Generic.List(Of Ext_ContrattoInfo))
            _contratti = value
        End Set
    End Property
    
    Public Sub New()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

