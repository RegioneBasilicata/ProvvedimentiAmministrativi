' NOTA: se si modifica il nome dell'interfaccia "IService1" qui, è necessario aggiornare anche il riferimento a "IService1" in App.config.
<ServiceContract()> _
Public Interface IGraph

    <OperationContract()> _
    Function CreaGrafo(ByVal stringaDot As String, ByVal pathIMG As String) As Boolean


    ' DA FARE: aggiungere qui le operazioni del servizio

End Interface

' Per aggiungere tipi compositi alle operazioni del servizio utilizzare un contratto di dati come descritto nell'esempio seguente
<DataContract()> _
Public Class CompositeType

    Private boolValueField As Boolean
    Private stringValueField As String

    <DataMember()> _
    Public Property BoolValue() As Boolean
        Get
            Return Me.boolValueField
        End Get
        Set(ByVal value As Boolean)
            Me.boolValueField = value
        End Set
    End Property

    <DataMember()> _
    Public Property StringValue() As String
        Get
            Return Me.stringValueField
        End Get
        Set(ByVal value As String)
            Me.stringValueField = value
        End Set
    End Property

End Class

