Public Class ItemContaDocumenti
    Private _totale As Long = 0
    Private _descrizioneDipartimento As String = ""
    Private _codiceDipartimento As String = ""

    Private _tipoAtto As Integer = 0
    Property Totale() As Long
        Get
            Return _totale
        End Get
        Set(ByVal value As Long)
            _totale = value
        End Set
    End Property
    Property CodiceDipartimento() As String
        Get
            Return _codiceDipartimento
        End Get
        Set(ByVal value As String)
            _codiceDipartimento = value
        End Set
    End Property
    Property DescrizioneDipartimento() As String
        Get
            Return _descrizioneDipartimento
        End Get
        Set(ByVal value As String)
            _descrizioneDipartimento = value
        End Set
    End Property

    Property TipoAtto() As String
        Get
            Return _tipoAtto
        End Get
        Set(ByVal value As String)
            _tipoAtto = value
        End Set
    End Property

End Class
