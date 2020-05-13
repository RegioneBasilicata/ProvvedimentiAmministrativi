Public Class ItemDocumentoInfo
    Private _idDocumento As String = String.Empty

    Public Property IdDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property
    
End Class
