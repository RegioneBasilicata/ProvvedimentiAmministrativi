''' <summary>
''' Questa classe rappresenta lo stato corrente di un particolare documento 
''' e dell'operatore che lo ha attualmente in carico. 
''' </summary>
''' <remarks></remarks>
Public Class StatoIstanzaDocumentoInfo

    ' Id del documento o rifermimento ad un oggetto DocumentoInfo? 
    Private _doc_Id As String
    Private _operatore As String
    Private _codiceUfficio As String
    Private _dataUltimaOperazione As Nullable(Of Date)
    Private _ruolo As Char
    Private _livelloUfficio As String

    ''' <summary>
    ''' Id del documento
    ''' </summary>
    Property Doc_id() As String
        Get
            Return _doc_Id
        End Get
        Set(ByVal Value As String)
            _doc_Id = Value
        End Set
    End Property

    ''' <summary>
    ''' Operatore che ha attualmente in carico l'istanza del documento 
    ''' </summary>
    Property Operatore() As String
        Get
            Return _operatore
        End Get
        Set(ByVal value As String)
            _operatore = value
        End Set
    End Property

    ''' <summary>
    ''' Ufficio che ha attualmente in carico il documento 
    ''' </summary>
    Property CodiceUfficio() As String
        Get
            Return _codiceUfficio
        End Get
        Set(ByVal value As String)
            _codiceUfficio = value
        End Set
    End Property

    ''' <summary>
    ''' Data dell'ultima operazione di modifica stato effettuata
    ''' sull'istanza. 
    ''' </summary>
    Property DataUltimaOperazione() As Nullable(Of Date)
        Get
            Return _dataUltimaOperazione
        End Get
        Set(ByVal value As Nullable(Of Date))
            _dataUltimaOperazione = value
        End Set
    End Property

    ''' <summary>
    ''' FIXME: Ruolo ? 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Ruolo() As Char
        Get
            Return _ruolo
        End Get
        Set(ByVal value As Char)
            _ruolo = value
        End Set
    End Property

    ''' <summary>
    ''' FIXME: Livello Ufficio ? 
    ''' </summary>
    Property LivelloUfficio() As String
        Get
            Return _livelloUfficio
        End Get
        Set(ByVal value As String)
            _livelloUfficio = value
        End Set
    End Property

    ReadOnly Property RuoloAsString() As String
        Get
            Select Case _ruolo
                Case "R"
                    Return "responsabile"
                Case "C"
                    Return "collaboratore"
                Case "S"
                    Return "supervisore"
                Case "D"
                    Return "deposito"
                Case "4"
                    Return "4LIVELLO"
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    ReadOnly Property LivelloUfficioAsString() As String
        Get
            Select Case _livelloUfficio
                Case "UP"
                    Return "ufficio_proponente"
                Case "UR"
                    Return "ufficio_ragioneria"
                Case "UDD"
                    Return "ufficio_dirigenza_dipartimento"
                Case "UCA"
                    Return "ufficio_controllo_amministrativo"
                Case "USL"
                    Return "ufficio_segreteria_legittimita"
                Case "USS"
                    Return "ufficio_segreteria_segretario"
                Case "US"
                    Return "ufficio_segreteria"
                Case "USA"
                    Return "ufficio_segreteria_approvazione"
                Case "UPRES"
                    Return "ufficio_presidenza"
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property
End Class
