Imports System.ServiceProcess

' NOTA: se si modifica il nome della classe "Service2" qui, è necessario aggiornare anche il riferimento a "Service2" in App.config.
Public Class Graph

    Implements IGraph


    Public Function CreaGrafo(ByVal stringaDot As String, ByVal pathIMG As String) As Boolean Implements IGraph.CreaGrafo
        Dim wg As New WINGRAPHVIZLib.DOT
        Return wg.ToGIF(stringaDot).Save(pathIMG)
    End Function








End Class
