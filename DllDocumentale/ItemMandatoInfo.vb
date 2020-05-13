Public Class ItemMandatoInfo
    Private _Man_id As Integer
    Private _Man_Nmandato As String
    Private _Man_Doc_id As String
    Private _Man_Nimpegno As String
    Private _Man_NLiquidazione As Integer
    Private _Man_NImporto As Decimal
    Private _Man_DataMandato As DateTime

    Property Man_id() As Integer
        Get
            Return _Man_id
        End Get
        Set(ByVal value As Integer)
            _Man_id = value
        End Set
    End Property
    Property Man_Nmandato() As String
        Get
            Return _Man_Nmandato
        End Get
        Set(ByVal value As String)
            _Man_Nmandato = value
        End Set
    End Property
    Property Man_Doc_id() As String
        Get
            Return _Man_Doc_id
        End Get
        Set(ByVal value As String)
            _Man_Doc_id = value
        End Set
    End Property
    Property Man_Nimpegno() As String
        Get
            Return _Man_Nimpegno
        End Get
        Set(ByVal value As String)
            _Man_Nimpegno = value
        End Set
    End Property
    Property Man_NLiquidazione() As Integer
        Get
            Return _Man_NLiquidazione
        End Get
        Set(ByVal value As Integer)
            _Man_NLiquidazione = value
        End Set
    End Property


    Property Man_NImporto() As Decimal
        Get
            Return _Man_NImporto
        End Get
        Set(ByVal value As Decimal)
            _Man_NImporto = value
        End Set
    End Property

    Property Man_DataMandato() As DateTime
        Get
            Return _Man_DataMandato
        End Get
        Set(ByVal value As DateTime)
            _Man_DataMandato = value
        End Set
    End Property
End Class
