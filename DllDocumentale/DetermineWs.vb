﻿Namespace DetermineWs



'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2032
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'Codice sorgente generato automaticamente da xsd, versione=1.1.4322.2032.
'

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes"), _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes", IsNullable:=False)> _
Public Class Messaggio

    '<remarks/>
    Public Intestazione As Intestazione

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Eccezione", GetType(Eccezione)), _
     System.Xml.Serialization.XmlElementAttribute("Risposta", GetType(Risposta)), _
     System.Xml.Serialization.XmlElementAttribute("Richiesta", GetType(Richiesta))> _
    Public Item As Object
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Intestazione

    '<remarks/>
    Public InfoMittDest As String

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")> _
    Public IdMessaggio As String

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")> _
    Public IdComunicazione As String

    '<remarks/>
    Public Applicazione As Applicazione
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Applicazione

    '<remarks/>
    Public CodiceApplicazione As String

    '<remarks/>
    Public ChiaveAutenticazione As String
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Eccezione

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")> _
    Public Codice As String

    '<remarks/>
    Public Descrizione As String
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class RispostaRegistrazione

    '<remarks/>
    Public NumeroRegistrazione As String
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Risposta

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Registrazione")> _
    Public Item As RispostaRegistrazione
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Registrazione

    '<remarks/>
    Public TipoFunzione As String

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="integer")> _
    Public TipoDocumento As String

    '<remarks/>
    Public QueryString As String
End Class

'<remarks/>
<System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/determinews/datatypes")> _
Public Class Richiesta

    '<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Registrazione")> _
    Public Item As Registrazione
    End Class

End Namespace
