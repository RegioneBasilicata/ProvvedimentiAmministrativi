
<System.Runtime.Serialization.DataContract()> _
Public Class Ext_SuggerimentoInfo
    Private _id As String = String.Empty
    Private _IdDocumento As String = String.Empty
    Private _tipologia As String = String.Empty
    Private _descrizioneTipologia As String = String.Empty
    Private _autore As String = String.Empty
    Private _codOperatore As String = String.Empty
    Private _note As String = String.Empty
    Private _data As String
    Private _pubblico As String
    <System.Runtime.Serialization.DataMember()> _
  Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
  Property IdDocumento() As String
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As String)
            _IdDocumento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
     Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
Property DescrizioneTipologia() As String
        Get
            Return _descrizioneTipologia
        End Get
        Set(ByVal value As String)
            _descrizioneTipologia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Autore() As String
        Get
            Return _autore
        End Get
        Set(ByVal value As String)
            _autore = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property CodOperatore() As String
        Get
            Return _codOperatore
        End Get
        Set(ByVal value As String)
            _codOperatore = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Property Note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
   Property Data() As String
        Get
            Return _data
        End Get
        Set(ByVal value As String)
            _data = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
 Property Pubblico() As String
        Get
            Return _pubblico
        End Get
        Set(ByVal value As String)
            _pubblico = value
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
