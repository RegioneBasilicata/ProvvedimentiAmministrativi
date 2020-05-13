Public Class ListaVuotaException
    Inherits Exception

    Private _codice As String = ""
    Sub New()
        MyBase.New("Nessun Risultato Restituito")
    End Sub
    Sub New(ByVal codice As String)

        MyBase.New("Nessun Risultato Restituito")
        _codice = codice
    End Sub
  
    ReadOnly Property Codice() As String
        Get
            Return _codice
        End Get
    End Property



End Class
