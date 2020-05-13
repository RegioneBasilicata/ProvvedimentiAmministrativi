Public Class NotificaFatturaOutput

    Private _progrFatturaInLiquidazione As Integer
    Private _esitoSIC As String

    Property ProgrFatturaInLiquidazione() As String
        Get
            Return _progrFatturaInLiquidazione
        End Get
        Set(ByVal value As String)
            _progrFatturaInLiquidazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property EsitoSIC() As String
        Get
            Return _esitoSIC
        End Get
        Set(ByVal value As String)
            _esitoSIC = value
        End Set
    End Property

End Class
