Imports System.ServiceModel

Public Class GestioneGraph

    Dim host As ServiceHost
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Inserire qui il codice necessario per avviare il proprio servizio. Il metodo deve effettuare
        ' le impostazioni necessarie per il funzionamento del servizio.
        EventLog.WriteEntry("Provo a partire")
        host = New ServiceHost(GetType(LibreriaGraph.Graph))

        'host.AddServiceEndpoint(GetType(LibreriaGraph.IGraph), New NetTcpBinding(), "net.tcp://localhost:9001/GestioneGraph.vb")
        host.Open()

    End Sub

    Protected Overrides Sub OnStop()
        ' Inserire qui il codice delle procedure di chiusura necessarie per arrestare il proprio servizio.
        host.Close()
    End Sub

End Class
