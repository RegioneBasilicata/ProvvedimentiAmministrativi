Public Class ItemRiduzioneLiqInfo
    Inherits ItemContabileInfo

    Private _Div_NLiquidazione As Integer
    Private _Div_NumeroReg As String

    Property Div_NumeroReg() As String
        Get
            Return _Div_NumeroReg
        End Get
        Set(ByVal value As String)
            _Div_NumeroReg = value
        End Set
    End Property
    Property Div_NLiquidazione() As String
        Get
            Return _Div_NLiquidazione
        End Get
        Set(ByVal value As String)
            _Div_NLiquidazione = value
        End Set
    End Property
End Class
