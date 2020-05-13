
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_UtenteInfo
    Private _account As String = String.Empty
    Private _denominazione As String = String.Empty
    Private _codicePubblicoUfficio As String = String.Empty
    Private _codiceInternoUfficio As String = String.Empty
    <System.Runtime.Serialization.DataMember()> _
 Property Account() As String
        Get
            Return _account
        End Get
        Set(ByVal value As String)
            _account = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Denominazione() As String
        Get
            Return _denominazione
        End Get
        Set(ByVal value As String)
            _denominazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Property CodicePubblicoUfficio() As String
        Get
            Return _codicePubblicoUfficio
        End Get
        Set(ByVal value As String)
            _codicePubblicoUfficio = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Property CodiceInternoUfficio() As String
        Get
            Return _codiceInternoUfficio
        End Get
        Set(ByVal value As String)
            _codiceInternoUfficio = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
