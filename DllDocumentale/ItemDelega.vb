Public Class ItemDelega
    Private _id As Long
    Private _codDelegato As String = ""
    Private _codDelegante As String = ""
    Private _codOpAttivazione As String = ""
    Private _dataAttivazione As Nullable(Of DateTime)
    Private _codOpDisattivazione As String = ""
    Private _dataDisattivazione As Nullable(Of DateTime)
    Private _opDelegante As DllAmbiente.Operatore = Nothing
    Private _opDelegato As DllAmbiente.Operatore = Nothing
    Private _tipoDelega As Integer = 0 '0 delega '1 interimi
    Private _Del_Id_Anag_Delegato As Long
    Private _Del_Id_Anag_Delegante As Long

    Private _Del_ChiusuraAtomatica As Integer
    Private _Del_DataChiusuraAutomatica As Nullable(Of DateTime)

    Property Del_ChiusuraAtomatica() As Integer
        Get
            Return _Del_ChiusuraAtomatica
        End Get
        Set(ByVal value As Integer)
            _Del_ChiusuraAtomatica = value
        End Set
    End Property

    Property Del_DataChiusuraAutomatica() As Nullable(Of DateTime)
        Get
            Return _Del_DataChiusuraAutomatica
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _Del_DataChiusuraAutomatica = value
        End Set
    End Property
    Property Id() As Long
        Get
            Return _id
        End Get
        Set(ByVal value As Long)
            _id = value
        End Set
    End Property
    Property CodDelegato() As String
        Get
            Return _codDelegato
        End Get
        Set(ByVal value As String)
            _codDelegato = value
        End Set
    End Property

    ReadOnly Property NominativoDelegato() As String
        Get
            If _opDelegato Is Nothing AndAlso Not String.IsNullOrEmpty(_codDelegato) Then
                _opDelegato = New DllAmbiente.Operatore
                _opDelegato.Codice = _codDelegato

            End If
            Return _opDelegato.Nominativo

        End Get

    End Property

    Property CodDelegante() As String
        Get
            Return _codDelegante
        End Get
        Set(ByVal value As String)
            _codDelegante = value
        End Set
    End Property

    ReadOnly Property NominativoDelegante() As String
        Get
            If _opDelegante Is Nothing AndAlso Not String.IsNullOrEmpty(_codDelegante) Then
                _opDelegante = New DllAmbiente.Operatore
                _opDelegante.Codice = _codDelegante

            End If
            Return _opDelegante.Nominativo

        End Get

    End Property

    Property CodOpAttivazione() As String
        Get
            Return _codOpAttivazione
        End Get
        Set(ByVal value As String)
            _codOpAttivazione = value
        End Set
    End Property


    Property DataAttivazione() As Nullable(Of DateTime)
        Get
            Return _dataAttivazione
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dataAttivazione = value
        End Set
    End Property

    Property CodOpDisattivazione() As String
        Get
            Return _codOpDisattivazione
        End Get
        Set(ByVal value As String)
            _codOpDisattivazione = value
        End Set
    End Property


    Property DataDisattivazione() As Nullable(Of DateTime)
        Get
            Return _dataDisattivazione
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dataDisattivazione = value
        End Set
    End Property


    Property TipoDelega() As Integer
        Get
            Return _tipoDelega
        End Get
        Set(ByVal value As Integer)
            _tipoDelega = value
        End Set
    End Property

    ReadOnly Property DescTipoDelega() As String
        Get
            Dim desc As String = ""
            Select Case _tipoDelega
                Case 0
                    desc = "Delega"
                Case 1
                    desc = "Interim"
            End Select

            Return desc
        End Get


    End Property


    Property Del_Id_Anag_Delegato() As Long
        Get
            Return _Del_Id_Anag_Delegato
        End Get
        Set(ByVal value As Long)
            _Del_Id_Anag_Delegato = value
        End Set
    End Property
    Property Del_Id_Anag_Delegante() As Long
        Get
            Return _Del_Id_Anag_Delegante
        End Get
        Set(ByVal value As Long)
            _Del_Id_Anag_Delegante = value
        End Set
    End Property


        


End Class
