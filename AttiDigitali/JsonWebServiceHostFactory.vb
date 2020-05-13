Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Channels
Imports System.Runtime.Serialization.Json

Public Class JsonWebServiceHostFactory
    Inherits WebServiceHostFactory

    Public Overrides Function CreateServiceHost(ByVal constructorString As String, ByVal baseAddresses As Uri()) As ServiceHostBase
        Dim sh As ServiceHost = New ServiceHost(GetType(ProcAmm), baseAddresses)
        sh.Description.Endpoints(0).Behaviors.Add(New WebHttpBehaviorEx())
        Return sh
    End Function

    Protected Overrides Function CreateServiceHost(ByVal serviceType As Type, ByVal baseAddresses As Uri()) As ServiceHost
        Return MyBase.CreateServiceHost(serviceType, baseAddresses)
    End Function
End Class
