Imports System.Runtime.Serialization

<DataContract()> _
Public Class ExtJSFault
    Private _faultMessage As String
    Private _faultCode As Integer

    Sub New(ByVal ex As Exception)
        Me.New(ex, -1)
    End Sub

    Sub New(ByVal ex As Exception, ByVal faultCode As Integer)
        Me.FaultMessage = ex.Message
        Me.FaultCode = faultCode
    End Sub

    <DataMember()> _
    Public Property FaultMessage() As String
        Get
            Return _faultMessage
        End Get
        Set(ByVal value As String)
            _faultMessage = value
        End Set
    End Property

    <DataMember()> _
    Public Property FaultCode() As String
        Get
            Return _faultCode
        End Get
        Set(ByVal value As String)
            _faultCode = value
        End Set
    End Property

End Class
