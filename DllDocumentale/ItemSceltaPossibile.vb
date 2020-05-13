Public Class ItemSceltaPossibile
    Private _cod_attributo As String = ""
    Private _valore As String = ""
    Private _id As Integer
    Private _attivo As Boolean

    Property Cod_attributo() As String
        Get
            Return _cod_attributo
        End Get
        Set(ByVal value As String)
            _cod_attributo = value
        End Set
    End Property
    Property Valore() As String
        Get
            Return _valore
        End Get
        Set(ByVal value As String)
            _valore = value
        End Set
    End Property
    Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Property Attivo() As Boolean
        Get
            Return _attivo
        End Get
        Set(ByVal value As Boolean)
            _attivo = value
        End Set
    End Property
End Class
