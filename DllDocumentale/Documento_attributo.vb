Public Class Documento_attributo
    Private _Doc_id As String
    Private _Ente As String
    Private _Cod_attributo As String
    Private _Valore As String
    Private _IdValore As String

    Property Doc_id() As String
        Get
            Return _Doc_id
        End Get
        Set(ByVal value As String)
            _Doc_id = value
        End Set
    End Property
    Property Ente() As String
        Get
            Return _Ente
        End Get
        Set(ByVal value As String)
            _Ente = value
        End Set
    End Property
    Property Cod_attributo() As String
        Get
            Return _Cod_attributo
        End Get
        Set(ByVal value As String)
            _Cod_attributo = value
        End Set
    End Property
    Property Valore() As String
        Get
            Return _Valore
        End Get
        Set(ByVal value As String)
            _Valore = value
        End Set
    End Property
    Property IdValore() As String
        Get
            Return _IdValore
        End Get
        Set(ByVal value As String)
            _IdValore = value
        End Set
    End Property
End Class
