Public Class ItemContrattoInfo
    Inherits ItemContrattoInfoHeader

    Private _numeroRepertorioContratto As String = String.Empty
    Private _oggettoContratto As String = String.Empty
    
    Public Property NumeroRepertorioContratto() As String
        Get
            Return _numeroRepertorioContratto
        End Get
        Set(ByVal value As String)
            _numeroRepertorioContratto = value
        End Set
    End Property
    Public Property OggettoContratto() As String
        Get
            Return _oggettoContratto
        End Get
        Set(ByVal value As String)
            _oggettoContratto = value
        End Set
    End Property
End Class
