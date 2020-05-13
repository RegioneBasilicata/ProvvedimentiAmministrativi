Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XmlReader
Imports System.Data.SqlClient
Imports System.Configuration



Public Class Utente

    Private pAccount As String
    Private pDenominazione As String
    
    Public Property Denominazione() As String
        Get
            Return pDenominazione
        End Get
        Set(ByVal value As String)
            pDenominazione = value
        End Set
    End Property
    Public Property Account() As String
        Get
            Return pAccount
        End Get
        Set(ByVal value As String)
            pAccount = value
        End Set
    End Property
End Class
