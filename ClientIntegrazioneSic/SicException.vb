Public Class SicException
    Inherits Exception

    Private _codice As String = ""
    Sub New()
        MyBase.New("Errore durante l'invocazione dei servizi del SIC.")
    End Sub
    Sub New(ByVal ex As Exception)

        MyBase.New("Errore durante l'invocazione dei servizi del SIC.: " & ex.Message)
        _codice = codice
    End Sub

    ReadOnly Property Codice() As String
        Get
            Return _codice
        End Get
    End Property



End Class
