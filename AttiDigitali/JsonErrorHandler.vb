Option Strict On

Imports System
Imports System.Net
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Channels
Imports System.Collections.ObjectModel
Imports System.Runtime.Serialization.Json
Imports System.ServiceModel

Public Class JsonErrorHandler
    Implements IErrorHandler


    Function handleError(ByVal e As Exception) As Boolean Implements IErrorHandler.HandleError
        Return True
    End Function


    Sub ProvideFault(ByVal err As Exception, ByVal ver As MessageVersion, ByRef fault As Message) Implements IErrorHandler.ProvideFault

        If TypeOf err Is FaultException Then
            Dim detail As Object = err.GetType.GetProperty("Detail").GetGetMethod.Invoke(err, Nothing)
            fault = Message.CreateMessage(ver, "", detail, New DataContractJsonSerializer(detail.GetType()))
            Dim wbf As WebBodyFormatMessageProperty = New WebBodyFormatMessageProperty(WebContentFormat.Json)
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf)

            Dim rmp As HttpResponseMessageProperty = New HttpResponseMessageProperty()
            rmp.StatusCode = System.Net.HttpStatusCode.BadRequest
            rmp.StatusDescription = "See fault object for more information."
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp)
            rmp.Headers.Add(HttpResponseHeader.ContentType, "text/x-json")
        Else
            fault = Message.CreateMessage(ver, "", "An non-fault exception is occured.", New DataContractJsonSerializer(GetType(String)))
            Dim wbf As WebBodyFormatMessageProperty = New WebBodyFormatMessageProperty(WebContentFormat.Json)
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf)
            Dim rmp As HttpResponseMessageProperty = New HttpResponseMessageProperty()
            rmp.StatusCode = System.Net.HttpStatusCode.InternalServerError
            rmp.StatusDescription = "Uknown exception..."
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp)
            rmp.Headers.Add(HttpResponseHeader.ContentType, "text/x-json")
        End If
    End Sub
End Class
