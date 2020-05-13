Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class MessageMaker

    Public Shared Function createProtocolloMessage(ByVal numeroDocumentoDefinitivo As String, ByVal oggetto As String, ByVal codiceRubrica As String, ByVal annotazioni As String, ByVal idComunicazione As String) As String

        Dim messaggio As New Messaggio
        Dim richiesta As New Richiesta
        richiesta.ItemElementName = ItemChoiceType3.ProtocollazioneDocumento

        Dim protocollazioneDocumento As New NuovoProtocollo


        messaggio.Intestazione = CreateHeader(idComunicazione)
        messaggio.Item = richiesta
        With protocollazioneDocumento
            .Ente = "001"
            .TipoProtocollo = TipoProtocollo.E
            .SoggettoPrivacy = Privacy.N
            .DIpartimentoRicezione = "71"
            .UfficioRicezione = "71A"
            .Oggetto = oggetto
            .Tipologia = "PDL"
            'errore ritorno del ws
            '.ProtDocArrivo = numeroDocumentoDefinitivo
            ' .DataProtDocArrivo = Format(Now, "yyyy-MM-dd")
            Dim mittenti As New Mittenti
            Dim mittentiEntrata As New VoceRubrica
            mittentiEntrata.Item = codiceRubrica
            mittenti.Item = mittentiEntrata
            .Mittenti = mittenti
            Dim destinatari As New Destinatari
            Dim destinatarioEntrata(1) As Object
            Dim ufficio As New Ufficio
            With ufficio
                .Ente = "001"
                .Dipartimento = "71"
                .CodiceUfficio = "71A"
                .Componente = "0"
                .TipoAssegnazione = "1"
            End With
            destinatarioEntrata(0) = ufficio
            destinatari.Items = destinatarioEntrata
            .Destinatari = destinatari
            If annotazioni.Length <> 0 Then
                .Annotazioni = annotazioni
            End If
            '.DataProtDocArrivo = Format(Now, "yyyy-MM-dd")
            .ProtDocArrivo = numeroDocumentoDefinitivo
            .DataProtDocArrivoSpecified = True
            .DataProtDocArrivo = CDate(Format(Now, "yyyy-MM-dd"))
        End With
        richiesta.Item = protocollazioneDocumento
        Return SerializeIt(messaggio)
    End Function

    Public Shared Function CreateHeader(ByVal idComunicazione As String) As Intestazione
        Dim intestazione As New Intestazione
        With intestazione
            .IdComunicazione = idComunicazione
            .IdMessaggio = "1"
            .InfoMittDest = "GPAOP"
            Dim applicazione As New Applicazione
            applicazione.CodiceApplicazione = "GPA"
            applicazione.ChiaveAutenticazione = "testitm"
            .Applicazione = applicazione
        End With
        Return intestazione

    End Function

    Public Shared Function SerializeIt(ByVal messaggio As Messaggio) As String


        Dim xDeserializer As XmlSerializer
        'Dim sw As StringWriter
        'Dim xw As XmlTextWriter
        'Dim fs As FileStream
        Dim buffer As New MemoryStream(4096)


        'sw = New StringWriter
        'xw = New XmlTextWriter(sw)
        xDeserializer = New XmlSerializer(GetType(Messaggio))
        'fs = New FileStream("c:\testMessaggio.xml", FileMode.Create, FileAccess.Write, FileShare.None)
        xDeserializer.Serialize(buffer, messaggio)
        buffer.Seek(0, SeekOrigin.Begin)
        'fs.Close()
        Dim strXml As String = New StreamReader(buffer).ReadToEnd()
        ' chiude il MemoryStream
        buffer.Close()



        'Return sw.ToString()
        Return strXml

    End Function

    Private Shared Function DeserializeIt(ByVal xmlMessage As String) As Messaggio

        Dim xDeserializer As XmlSerializer
        Dim sr As StringReader
        Dim xr As XmlTextReader


        sr = New StringReader(xmlMessage)
        xr = New XmlTextReader(sr)
        xDeserializer = New XmlSerializer(GetType(Messaggio))



        Return DirectCast(xDeserializer.Deserialize(xr), Messaggio)

    End Function


    Public Shared Function RitornoNumeroProtocollo(ByVal xmlMessage As String) As String
        Dim messaggioRitorno As Messaggio
        Try
            messaggioRitorno = DeserializeIt(xmlMessage)
            Dim risposta As Risposta = DirectCast(messaggioRitorno.Item, Risposta)
        Catch ex As Exception
            'todo 
            'fare eccezione personalizzata per far risalire l'eccezione
            Dim risposta As Eccezione = DirectCast(messaggioRitorno.Item, Eccezione)
        End Try

        Dim protDoc As RispostaProtocollazioneDocumento = DirectCast(messaggioRitorno.Item.Item, RispostaProtocollazioneDocumento)

        Return protDoc.NumeroProtocollo
    End Function

    

End Class
