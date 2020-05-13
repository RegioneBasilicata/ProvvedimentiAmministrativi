<System.Runtime.Serialization.DataContract()> _
Public Class Ext_MandatoInfo

    Private _id As String = String.Empty
    Private _Nmandato As String = String.Empty
    Private _Doc_id As String = String.Empty
    Private _Nimpegno As String = String.Empty
    Private _NLiquidazione As String = String.Empty
    Private _NImporto As String = String.Empty
    Private _DataMandato As String = String.Empty



    <System.Runtime.Serialization.DataMember()> _
 Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Nmandato() As String
        Get
            Return _Nmandato
        End Get
        Set(ByVal value As String)
            _Nmandato = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Doc_id() As String
        Get
            Return _Doc_id
        End Get
        Set(ByVal value As String)
            _Doc_id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property Nimpegno() As String
        Get
            Return _Nimpegno
        End Get
        Set(ByVal value As String)
            _Nimpegno = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property NLiquidazione() As String
        Get
            Return _NLiquidazione
        End Get
        Set(ByVal value As String)
            _NLiquidazione = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()> _
    Property NImporto() As String
        Get
            Return _NImporto
        End Get
        Set(ByVal value As String)
            _NImporto = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()> _
    Property DataMandato() As String
        Get
            Return _DataMandato
        End Get
        Set(ByVal value As String)
            _DataMandato = value
        End Set
    End Property
    Public Sub New()

    End Sub

   
End Class
