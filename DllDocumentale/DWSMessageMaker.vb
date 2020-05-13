Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class DWSMessageMaker

    

    Public Shared Function CreateHeader(ByVal idComunicazione As String) As DetermineWs.Intestazione
        Dim intestazione As New DetermineWs.Intestazione
        With intestazione
            .IdComunicazione = idComunicazione
            .IdMessaggio = "1"
            .InfoMittDest = "GPAOP"
            Dim applicazione As New DetermineWs.Applicazione
            applicazione.CodiceApplicazione = "GPA"
            applicazione.ChiaveAutenticazione = "testitm"
            .Applicazione = applicazione
        End With
        Return intestazione

    End Function

    Public Shared Function SerializeIt(ByVal messaggio As DetermineWs.Messaggio) As String


        Dim xDeserializer As XmlSerializer
        'Dim sw As StringWriter
        'Dim xw As XmlTextWriter
        'Dim fs As FileStream
        Dim buffer As New MemoryStream(4096)


        'sw = New StringWriter
        'xw = New XmlTextWriter(sw)
        xDeserializer = New XmlSerializer(GetType(DetermineWs.Messaggio))
        'fs = New FileStream("c:\testMessaggio.xml", FileMode.Create, FileAccess.Write, FileShare.None)
        xDeserializer.Serialize(buffer, messaggio)
        buffer.Seek(0, SeekOrigin.Begin)
        'fs.Close()
        Dim strXml = New StreamReader(buffer).ReadToEnd()
        ' chiude il MemoryStream
        buffer.Close()



        'Return sw.ToString()
        Return strXml

    End Function

    Private Shared Function DeserializeIt(ByVal xmlMessage As String) As DetermineWs.Messaggio

        Dim xDeserializer As XmlSerializer
        Dim sr As StringReader
        Dim xr As XmlTextReader


        sr = New StringReader(xmlMessage)
        xr = New XmlTextReader(sr)
        xDeserializer = New XmlSerializer(GetType(DetermineWs.Messaggio))



        Return DirectCast(xDeserializer.Deserialize(xr), DetermineWs.Messaggio)

    End Function


    Public Shared Function RitornoNumeroRegistrazione(ByVal xmlMessage As String) As String
        Dim messaggioRitorno As DetermineWs.Messaggio
        Try
            messaggioRitorno = DeserializeIt(xmlMessage)
            Dim risposta As DetermineWs.Risposta = DirectCast(messaggioRitorno.Item, DetermineWs.Risposta)
        Catch ex As Exception
            'todo 
            'fare eccezione personalizzata per far risalire l'eccezione
            Dim risposta As DetermineWs.Eccezione = DirectCast(messaggioRitorno.Item, DetermineWs.Eccezione)
        End Try



        Return ""
    End Function

    Public Shared Function createRegistrazioneMessage(ByVal tipoFunzione As String, ByVal tipoDocumento As Integer, ByVal queryString As String) As String

        Dim messaggio As New DetermineWs.Messaggio
        Dim richiesta As New DetermineWs.Richiesta


        Dim registrazione As New DetermineWs.Registrazione



        messaggio.Item = richiesta
        With registrazione
        End With
        richiesta.Item = registrazione
        Return SerializeIt(messaggio)
    End Function

End Class
