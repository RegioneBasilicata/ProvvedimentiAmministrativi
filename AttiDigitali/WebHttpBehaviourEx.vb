Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Channels
Imports System.Runtime.Serialization.Json


Public Class WebHttpBehaviorEx
    Inherits WebHttpBehavior

    Protected Overrides Sub AddServerErrorHandlers(ByVal endpoint As ServiceEndpoint, ByVal dispatcher As EndpointDispatcher)
        dispatcher.ChannelDispatcher.ErrorHandlers.Clear()

        dispatcher.ChannelDispatcher.ErrorHandlers.Add(New JsonErrorHandler())
    End Sub


End Class

