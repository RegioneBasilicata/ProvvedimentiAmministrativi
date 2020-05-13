Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports ClientIntegrazioneSic
Imports ClientIntegrazioneSic.Intema.WS.Richiesta
Imports ClientIntegrazioneSic.Intema.WS.Risposta
Imports System.Configuration



Public Class MessageMaker
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(MessageAnaMaker))

    'Public Shared Function createProtocolloMessage(ByVal numeroDocumentoDefinitivo As String, ByVal oggetto As String, ByVal codiceRubrica As String, ByVal annotazioni As String, ByVal idComunicazione As String) As String

    '    Dim messaggio As New Messaggio
    '    Dim richiesta As New Richiesta
    '    richiesta.ItemElementName = ItemChoiceType3.ProtocollazioneDocumento

    '    Dim protocollazioneDocumento As New NuovoProtocollo


    '    messaggio.Intestazione = CreateHeader(idComunicazione)
    '    messaggio.Item = richiesta
    '    With protocollazioneDocumento
    '        .Ente = "001"
    '        .TipoProtocollo = TipoProtocollo.E
    '        .SoggettoPrivacy = Privacy.N
    '        .DIpartimentoRicezione = "71"
    '        .UfficioRicezione = "71A"
    '        .Oggetto = oggetto
    '        .Tipologia = "PDL"
    '        'errore ritorno del ws
    '        '.ProtDocArrivo = numeroDocumentoDefinitivo
    '        ' .DataProtDocArrivo = Format(Now, "yyyy-MM-dd")
    '        Dim mittenti As New Mittenti
    '        Dim mittentiEntrata As New VoceRubrica
    '        mittentiEntrata.Item = codiceRubrica
    '        mittenti.Item = mittentiEntrata
    '        .Mittenti = mittenti
    '        Dim destinatari As New Destinatari
    '        Dim destinatarioEntrata(1) As Object
    '        Dim ufficio As New Ufficio
    '        With ufficio
    '            .Ente = "001"
    '            .Dipartimento = "71"
    '            .CodiceUfficio = "71A"
    '            .Componente = "0"
    '            .TipoAssegnazione = "1"
    '        End With
    '        destinatarioEntrata(0) = ufficio
    '        destinatari.Items = destinatarioEntrata
    '        .Destinatari = destinatari
    '        If annotazioni.Length <> 0 Then
    '            .Annotazioni = annotazioni
    '        End If
    '        '.DataProtDocArrivo = Format(Now, "yyyy-MM-dd")
    '        .ProtDocArrivo = numeroDocumentoDefinitivo
    '        .DataProtDocArrivoSpecified = True
    '        .DataProtDocArrivo = CDate(Format(Now, "yyyy-MM-dd"))
    '    End With
    '    richiesta.Item = protocollazioneDocumento
    '    Return SerializeIt(messaggio)
    'End Function

    'Public Shared Function CreateHeader(ByVal idComunicazione As String) As Intestazione
    '    Dim intestazione As New Intestazione
    '    With intestazione
    '        .IdComunicazione = idComunicazione
    '        .IdMessaggio = "1"
    '        .InfoMittDest = "GPAOP"
    '        Dim applicazione As New Applicazione
    '        applicazione.CodiceApplicazione = "GPA"
    '        applicazione.ChiaveAutenticazione = "testitm"
    '        .Applicazione = applicazione
    '    End With
    '    Return intestazione

    'End Function

    Public Shared Function SerializeIt(ByVal messaggio As MessaggioRichiesta_Types) As String


        Dim xDeserializer As XmlSerializer
        Dim buffer As New MemoryStream(4096)
      
        xDeserializer = New XmlSerializer(GetType(MessaggioRichiesta_Types))

        xDeserializer.Serialize(buffer, messaggio)
        buffer.Seek(0, SeekOrigin.Begin)

        Dim strXml = New StreamReader(buffer).ReadToEnd()
        ' chiude il MemoryStream
        buffer.Close()

        Return strXml

    End Function
    Private Shared Function DeserializeIt(ByVal xmlMessage As String) As MessaggioRisposta_Types

        Dim xDeserializer As XmlSerializer
        Dim sr As StringReader
        Dim xr As XmlTextReader
        sr = New StringReader(xmlMessage)
        xr = New XmlTextReader(sr)
        xDeserializer = New XmlSerializer(GetType(MessaggioRisposta_Types))
        Return DirectCast(xDeserializer.Deserialize(xr), MessaggioRisposta_Types)

    End Function

    Public Shared Function createInterrogaMandatiBeneficiarioAttoMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                                 ByVal tipoAtto As String, _
                                                                 ByVal numeroAtto As String, _
                                                                 ByVal dataAtto As Date, _
                                                                 ByVal ufficioAtto As String, _
                                                                 ByVal numeroLiquidazione As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogaMandatiBeneficiarioAtto As New InterrogaMandatiBeneficiariAtto_Types

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaInterrogaMandatiBeneficiarioAtto.TipoAtto = tipoAtto
            Else
                objRichiestaInterrogaMandatiBeneficiarioAtto.TipoAtto = ""
            End If
            If Not String.IsNullOrEmpty(numeroAtto) Then
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroAtto = numeroAtto
            Else
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroAtto = ""
            End If
            If Not (dataAtto = Nothing) And IsDate(dataAtto) Then
                objRichiestaInterrogaMandatiBeneficiarioAtto.DataAtto = dataAtto
            Else
                objRichiestaInterrogaMandatiBeneficiarioAtto.DataAtto = Nothing
            End If
            If Not String.IsNullOrEmpty(ufficioAtto) Then
                objRichiestaInterrogaMandatiBeneficiarioAtto.Ufficio = ufficioAtto
            Else
                objRichiestaInterrogaMandatiBeneficiarioAtto.Ufficio = ""
            End If
            If Not String.IsNullOrEmpty(numeroLiquidazione) And IsNumeric(numeroLiquidazione) Then
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroLiquidazioneSpecified = True
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroLiquidazione = Long.Parse(numeroLiquidazione)
            Else
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroLiquidazioneSpecified = False
                objRichiestaInterrogaMandatiBeneficiarioAtto.NumeroLiquidazione = 0
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogaMandatiBeneficiarioAtto
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogaMandatiBeneficiariAtto

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            Dim arrayMandatiBeneficiariAtto As Risposta_InterrogaMandatiBeneficiariAtto_Types
            arrayMandatiBeneficiariAtto = DirectCast(risposta.Item.Item, Risposta_InterrogaMandatiBeneficiariAtto_Types)

            Return DirectCast(arrayMandatiBeneficiariAtto.Mandato, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createInterrogaContrattiMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                       ByVal idContratti As Generic.List(Of String)) As Array
        Try
            If idContratti Is Nothing OrElse idContratti.Count = 0 Then
                Throw New ListaVuotaException()
            End If

            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types

            Dim objRichiestaInterrogazioneContratti As New InterrogazioneListaContratti_Types
            objRichiestaInterrogazioneContratti.Chiave = idContratti.ToArray

            richiesta.Richiesta.Item = objRichiestaInterrogazioneContratti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneListaContratti

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            Dim arrayContratti As Risposta_InterrogazioneContratti_Types
            arrayContratti = DirectCast(risposta.Item.Item, Risposta_InterrogazioneContratti_Types)

            Return DirectCast(arrayContratti.Contratto, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


    Public Shared Function createInterrogaContrattiMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                           ByVal numeroRepertorio As String, _
                                                           ByVal descrizione As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneContratti As New InterrogazioneContratti_Types

            If Not String.IsNullOrEmpty(numeroRepertorio) Then
                objRichiestaInterrogazioneContratti.NumeroContratto = numeroRepertorio
            End If
            If Not String.IsNullOrEmpty(descrizione) Then
                objRichiestaInterrogazioneContratti.OggettoContratto = descrizione
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneContratti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneContratti

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            Dim arrayContratti As Risposta_InterrogazioneContratti_Types
            arrayContratti = DirectCast(risposta.Item.Item, Risposta_InterrogazioneContratti_Types)

            Return DirectCast(arrayContratti.Contratto, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function


    Public Shared Function createInterrogazioneCapitoliMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, Optional ByVal codice_ufficio As String = "") As Array
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            'richiesta.Intestazione.InfoMittDest = "Giovanna Gentilesca"
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneCapitoli As New InterrogazioneCapitoli_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneCapitoli.AnnoBilancio = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneCapitoli.AnnoBilancio = Year(Now)
            End If

            If String.IsNullOrEmpty(codice_ufficio) Then
                objRichiestaInterrogazioneCapitoli.Struttura = operatore.oUfficio.leggiUfficioPubblico(operatore.oUfficio.CodUfficio)
            Else
                Dim uffTemp As New DllAmbiente.Ufficio
                uffTemp.CodUfficio = codice_ufficio
                objRichiestaInterrogazioneCapitoli.Struttura = uffTemp.leggiUfficioPubblico(uffTemp.CodUfficio)
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneCapitoli
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneCapitoli
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            Dim arrayCapitolo As Risposta_InterrogazioneCapitoli_Types

            arrayCapitolo = DirectCast(risposta.Item.Item, Risposta_InterrogazioneCapitoli_Types)

            Return DirectCast(arrayCapitolo.Capitolo, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function

    Public Shared Function createInterrogazioneBilancioInfoMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal codiceCapitolo As String, ByVal codiceUfficioInterno As String) As Risposta_InterrogazioneBilancio_Types
        Try

            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneBilancio As New InterrogazioneBilancio_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneBilancio.AnnoBilancio = CInt(annoBilancio)
            Else
                Throw New Exception("Anno  non valido")
            End If
            If Not String.IsNullOrEmpty(codiceCapitolo) Then
                objRichiestaInterrogazioneBilancio.CodiceCapitolo = codiceCapitolo
            Else
                Throw New Exception("Codice Capitolo non valido")
            End If

            If Not String.IsNullOrEmpty(codiceUfficioInterno) Then
                objRichiestaInterrogazioneBilancio.Struttura = operatore.oUfficio.leggiUfficioPubblico(codiceUfficioInterno)
            Else
                Throw New Exception("Codice Ufficio non valido")
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneBilancio
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneBilancio
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Dim DettaglioCapitolo As Risposta_InterrogazioneBilancio_Types
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            DettaglioCapitolo = DirectCast(risposta.Item.Item, Risposta_InterrogazioneBilancio_Types)

            Return DettaglioCapitolo
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try



    End Function



    Public Shared Function createInterrogazioneBilancioMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal codiceCapitolo As String, ByVal ufficioProponente As String) As Double
        Try

            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneBilancio As New InterrogazioneBilancio_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneBilancio.AnnoBilancio = CInt(annoBilancio)
            Else
                Throw New Exception("Anno non valido")
            End If
            If Not String.IsNullOrEmpty(codiceCapitolo) Then
                objRichiestaInterrogazioneBilancio.CodiceCapitolo = codiceCapitolo
            Else
                Throw New Exception("Codice Capitolo non valido")
            End If
            If Not String.IsNullOrEmpty(ufficioProponente) Then
                objRichiestaInterrogazioneBilancio.Struttura = operatore.oUfficio.leggiUfficioPubblico(ufficioProponente)
            Else
                objRichiestaInterrogazioneBilancio.Struttura = operatore.oUfficio.leggiUfficioPubblico(operatore.oUfficio.CodUfficio)
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneBilancio
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneBilancio
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Dim DettaglioCapitolo As Risposta_InterrogazioneBilancio_Types
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If

            DettaglioCapitolo = DirectCast(risposta.Item.Item, Risposta_InterrogazioneBilancio_Types)
            Return DettaglioCapitolo.DispCompetenza
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try



    End Function
    Public Shared Function createDisponibilitaPreImpegnoMessage(ByVal operatore As DllAmbiente.Operatore, ByVal numPreImpegno As String, Optional ByVal codUfficioProponente As String = "") As Risposta_DisponibilitaPreImpegno_Types

        Dim risposta As New MessaggioRisposta_Types
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaDisponibilitaPreImpegno As New DisponibilitaPreImpegno_Types

            If Not String.IsNullOrEmpty(numPreImpegno) Then
                objRichiestaDisponibilitaPreImpegno.NumeroPreimpegno = CInt(numPreImpegno)
            Else
                Throw New Exception("Numero Pre impegno non valido")
            End If

            If Not String.IsNullOrEmpty(codUfficioProponente) Then
                objRichiestaDisponibilitaPreImpegno.Struttura = codUfficioProponente
            Else
                objRichiestaDisponibilitaPreImpegno.Struttura = operatore.oUfficio.CodUfficioPubblico
            End If

            richiesta.Richiesta.Item = objRichiestaDisponibilitaPreImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.DisponibilitaPreImpegno

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))

            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            risposta = DeserializeIt(outputQuerySic.return)

            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim importoDisponibilePreImpegno As Risposta_DisponibilitaPreImpegno_Types

            Dim successo As Object = DirectCast(risposta.Item, ClientIntegrazioneSic.Intema.WS.Risposta.Successo_Types)
            importoDisponibilePreImpegno = DirectCast(successo.Item, Risposta_DisponibilitaPreImpegno_Types)

            Return importoDisponibilePreImpegno
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException(DirectCast(risposta.Item, ClientIntegrazioneSic.Intema.WS.Risposta.Eccezione_Types).Descrizione)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function

    Public Shared Function createGenerazionePreimpegnoPerPluriennaleMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                                            ByVal oggettoAtto As String, _
                                                                            ByVal CodiceCapitolo As String, _
                                                                            ByVal tipoAtto As String, _
                                                                            ByVal dataAtto As Date, _
                                                                            ByVal numAtto As String, _
                                                                            ByVal Importo1 As Double, _
                                                                            ByVal ObGestionale1 As String, _
                                                                            ByVal PCF1 As String, _
                                                                            ByVal Importo2 As Double, _
                                                                            ByVal ObGestionale2 As String, _
                                                                            ByVal PCF2 As String, _
                                                                            ByVal Importo3 As Double, _
                                                                            ByVal ObGestionale3 As String, _
                                                                            ByVal PCF3 As String, _
                                                                            ByVal idDocumento As String, _
                                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                                            ByVal tokenSic As String _
                                                                 ) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazionePreImpegno As New Intema.WS.Richiesta.CreateDelDocumento_Types
            
            'GESTIONE TOKEN
            objRichiestaGenerazionePreImpegno.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazionePreImpegno.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazionePreImpegno.IdUnivocoSistemaChiamante= idDocumento


            objRichiestaGenerazionePreImpegno.CodiceFiscale = operatore.CodiceFiscale

            If Not String.IsNullOrEmpty(oggettoAtto) Then
                objRichiestaGenerazionePreImpegno.Oggetto = oggettoAtto
            Else
                Throw New Exception("oggetto atto per il pre-impegno non valido")
            End If
            If Not String.IsNullOrEmpty(CodiceCapitolo) Then
                objRichiestaGenerazionePreImpegno.CodiceCapitolo = CodiceCapitolo
            Else
                Throw New Exception("Codice Capitolo non valido")
            End If

            objRichiestaGenerazionePreImpegno.DataMovimentoSpecified = True
            objRichiestaGenerazionePreImpegno.DataMovimento = Now

            If Not Double.IsNegativeInfinity(Importo1) AndAlso Importo1 > 0 Then
                objRichiestaGenerazionePreImpegno.ImportoSpecified = True
                objRichiestaGenerazionePreImpegno.Importo = Importo1

                If Not String.IsNullOrEmpty(ObGestionale1) Then
                    objRichiestaGenerazionePreImpegno.COG = ObGestionale1
                Else
                    Throw New Exception("COG 1 non valido")
                End If

                If Not String.IsNullOrEmpty(PCF1) Then
                    objRichiestaGenerazionePreImpegno.PianoContiFina = PCF1
                Else
                    Throw New Exception("PCF 1 non valido")
                End If
            End If

            If Not Double.IsNegativeInfinity(Importo2) AndAlso Importo2 > 0 Then
                objRichiestaGenerazionePreImpegno.Importo_Plur1Specified = True
                objRichiestaGenerazionePreImpegno.Importo_Plur1 = Importo2

                If Not String.IsNullOrEmpty(ObGestionale2) Then
                    objRichiestaGenerazionePreImpegno.COG_Plur1 = ObGestionale2
                Else
                    Throw New Exception("COG 2 non valido")
                End If

                If Not String.IsNullOrEmpty(PCF2) Then
                    objRichiestaGenerazionePreImpegno.PianoContiFina_Plur1 = PCF2
                Else
                    Throw New Exception("PCF 2 non valido")
                End If
            End If

            If Not Double.IsNegativeInfinity(Importo3) AndAlso Importo3 > 0 Then
                objRichiestaGenerazionePreImpegno.Importo_Plur2Specified = True
                objRichiestaGenerazionePreImpegno.Importo_Plur2 = Importo3

                If Not String.IsNullOrEmpty(ObGestionale3) Then
                    objRichiestaGenerazionePreImpegno.COG_Plur2 = ObGestionale3
                Else
                    Throw New Exception("COG 3 non valido")
                End If

                If Not String.IsNullOrEmpty(PCF3) Then
                    objRichiestaGenerazionePreImpegno.PianoContiFina_Plur2 = PCF3
                Else
                    Throw New Exception("PCF 3 non valido")
                End If
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazionePreImpegno.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazionePreImpegno.DataAttoSpecified = True
                objRichiestaGenerazionePreImpegno.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazionePreImpegno.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If


            objRichiestaGenerazionePreImpegno.Struttura = operatore.oUfficio.CodUfficioPubblico

            objRichiestaGenerazionePreImpegno.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.PREIMP

            

            richiesta.Richiesta.Item = objRichiestaGenerazionePreImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Info("createGenerazionePreimpegnoPerPluriennaleMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazionePreimpegnoPerPluriennaleMessage OUTPUT SIC: " + outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            'Catch ex As FileValidatorException
            '    Throw New FileValidatorException("")

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(CodiceCapitolo + ": " + ex.Message)
        End Try
    End Function


    Public Shared Function createGenerazionePreImpegnoMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                                 ByVal oggettoAtto As String, _
                                                                 ByVal CodiceCapitolo As String, _
                                                                 ByVal Importo As Double, _
                                                                 ByVal tipoAtto As String, _
                                                                 ByVal dataAtto As Date, _
                                                                 ByVal numAtto As String, _
                                                                 ByVal Codice_obiettivo_gestionale As String, _
                                                                 ByVal idDocumento As String, _
                                                                 ByVal numeroProvvisOrDefAtto As String, _
                                                                 ByVal tokenSic As String
                                                              ) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazionePreImpegno As New Intema.WS.Richiesta.CreateDelDocumento_Types
            
            'GESTIONE TOKEN
            objRichiestaGenerazionePreImpegno.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazionePreImpegno.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazionePreImpegno.IdUnivocoSistemaChiamante= idDocumento

            objRichiestaGenerazionePreImpegno.CodiceFiscale = operatore.CodiceFiscale

            If Not String.IsNullOrEmpty(oggettoAtto) Then
                objRichiestaGenerazionePreImpegno.Oggetto = oggettoAtto
            Else
                Throw New Exception("oggetto atto per il pre-impegno non valido")
            End If
            If Not String.IsNullOrEmpty(CodiceCapitolo) Then
                objRichiestaGenerazionePreImpegno.CodiceCapitolo = CodiceCapitolo
            Else
                Throw New Exception("Codice Capitolo non valido")
            End If

            objRichiestaGenerazionePreImpegno.DataMovimentoSpecified = True
            objRichiestaGenerazionePreImpegno.DataMovimento = Now

            If Not Double.IsNegativeInfinity(Importo) Then
                objRichiestaGenerazionePreImpegno.ImportoSpecified = True
                objRichiestaGenerazionePreImpegno.Importo = Importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazionePreImpegno.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazionePreImpegno.DataAttoSpecified = True
                objRichiestaGenerazionePreImpegno.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazionePreImpegno.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If

            If Not String.IsNullOrEmpty(Codice_obiettivo_gestionale) Then
                objRichiestaGenerazionePreImpegno.COG = Codice_obiettivo_gestionale
            Else
                Throw New Exception("COG non valido")
            End If

            objRichiestaGenerazionePreImpegno.Struttura = operatore.oUfficio.CodUfficioPubblico

            objRichiestaGenerazionePreImpegno.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.PREIMP

            richiesta.Richiesta.Item = objRichiestaGenerazionePreImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Info("createGenerazionePreImpegnoMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazionePreImpegnoMessage OUTPUT SIC: " + outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            'Dim numRegistrazione As Risposta_CreateDelDocumento_Types
            'numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)
            'Return numRegistrazione.NumeroDocumento

            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            'Catch ex As FileValidatorException
            '    Throw New FileValidatorException("")

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni


        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createGenerazionePreImpegnoRagMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                             ByVal numPreimpegno As String, _
                                                             ByVal oggettoAttoPreImpegno As String, _
                                                             ByVal tipoAtto As String, _
                                                             ByVal dataAtto As Date, _
                                                             ByVal numAtto As String, _
                                                             ByVal struttura As String, _
                                                             ByVal dataMovimento As DateTime, _
                                                             ByVal idDocumento As String, _
                                                             ByVal numeroProvvisOrDefAtto As String, _
                                                             ByVal tokenSic As String 
                                                            ) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazionePreImpegno As New Intema.WS.Richiesta.CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazionePreImpegno.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazionePreImpegno.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazionePreImpegno.IdUnivocoSistemaChiamante= idDocumento

            objRichiestaGenerazionePreImpegno.CodiceFiscale = operatore.CodiceFiscale

           


            If Not String.IsNullOrEmpty(oggettoAttoPreImpegno) Then
                objRichiestaGenerazionePreImpegno.Oggetto = oggettoAttoPreImpegno
            Else
                Throw New Exception("Oggetto Atto pre-impegno non valido")
            End If

            objRichiestaGenerazionePreImpegno.DataMovimentoSpecified = True
            objRichiestaGenerazionePreImpegno.DataMovimento = dataMovimento

            If Not String.IsNullOrEmpty(numPreimpegno) Then
                objRichiestaGenerazionePreImpegno.NumeroDocumentoPC = numPreimpegno
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazionePreImpegno.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazionePreImpegno.DataAttoSpecified = True
                objRichiestaGenerazionePreImpegno.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazionePreImpegno.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If

            objRichiestaGenerazionePreImpegno.Struttura = struttura
            objRichiestaGenerazionePreImpegno.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.PREIMPDELIBERA

            richiesta.Richiesta.Item = objRichiestaGenerazionePreImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazionePreImpegnoRagMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazionePreImpegnoRagMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni
            'Catch ex As FileValidatorException
            '    Throw New FileValidatorException("")

            'Return numRegistrazione.NumeroDocumento

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createPrenotazionePreImpegnoMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                          ByVal tipoOperazione As String, _
                                                          ByVal numPreImp As String, _
                                                          ByVal Importo As Double) As String
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaPrenotazionePreImpegno As New PrenotazionePreimpegni_Types

            If Not String.IsNullOrEmpty(numPreImp) Then
                objRichiestaPrenotazionePreImpegno.NumeroPreimpegno = numPreImp
            Else
                Throw New Exception("Numero pre-impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(Importo) Then
                objRichiestaPrenotazionePreImpegno.Importo = Importo
            Else
                Throw New Exception("Importo Capitolo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoOperazione) Then
                objRichiestaPrenotazionePreImpegno.TipoOperazione = tipoOperazione
            Else
                Throw New Exception("Tipo Operazione  non valido")
            End If


            richiesta.Richiesta.Item = objRichiestaPrenotazionePreImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.PrenotazionePreimpegni

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)

            Log.Info("createPrenotazionePreImpegnoMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createPrenotazionePreImpegnoMessage OUTPUT SIC: " + outputQuerySic.return)

            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If

            Dim numPreImpegno As Risposta_PrenotazionePreimpegni_Types


            numPreImpegno = DirectCast(risposta.Item.Item, Risposta_PrenotazionePreimpegni_Types)

            'Catch ex As FileValidatorException
            '    Throw New FileValidatorException("")

            Return numPreImpegno.Codice

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function createGenerazioneImpegnoMessage(ByVal operatore As DllAmbiente.Operatore,
                                                            ByVal numPreimp As Integer,
                                                            ByVal importo As String,
                                                            ByVal tipoAtto As String,
                                                            ByVal dataAtto As Date,
                                                            ByVal numAtto As String,
                                                            ByVal dipartimento As String,
                                                            ByVal contoEconomico As String,
                                                            ByVal ratei As String,
                                                            ByVal impostaIrap As String,
                                                            ByVal risconti As String,
                                                            ByVal dataMovimento As DateTime,
                                                            ByVal Codice_Obbiettivo_Gestionale As String,
                                                            ByVal pcf As String,
                                                            ByVal oggetto As String,
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                            ByVal tokenSic As String, _
                                                            Optional ByVal listaben As Array = Nothing
                                                            ) As String()

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneImpegno As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneImpegno.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneImpegno.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazioneImpegno.IdUnivocoSistemaChiamante= idDocumento

            objRichiestaGenerazioneImpegno.CodiceFiscale = operatore.CodiceFiscale

            objRichiestaGenerazioneImpegno.DataMovimentoSpecified = True
            objRichiestaGenerazioneImpegno.DataMovimento = dataMovimento

            If Not Long.Parse(numPreimp) Then
                objRichiestaGenerazioneImpegno.NumeroDocumentoPC = numPreimp
            Else
                Throw New Exception("Numero pre-impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneImpegno.ImportoSpecified = True
                objRichiestaGenerazioneImpegno.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazioneImpegno.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazioneImpegno.DataAttoSpecified = True
                objRichiestaGenerazioneImpegno.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazioneImpegno.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If

            objRichiestaGenerazioneImpegno.Struttura = dipartimento

            If Not String.IsNullOrEmpty(contoEconomico) Then
                objRichiestaGenerazioneImpegno.ContoEconomica = contoEconomico
            Else
                'Conto Economica non è + obbligatorio
                'Throw New Exception("Conto Economica non valido")
            End If

            If Not String.IsNullOrEmpty(Codice_Obbiettivo_Gestionale) Then
                objRichiestaGenerazioneImpegno.COG = Codice_Obbiettivo_Gestionale
            Else
                Throw New Exception("Codice Obiettivo Gestionale non valido")
            End If

            If Not String.IsNullOrEmpty(oggetto) Then
                objRichiestaGenerazioneImpegno.Oggetto = oggetto
            Else
                Throw New Exception("Oggetto non valido")
            End If

            If Not String.IsNullOrEmpty(ratei) Then
                objRichiestaGenerazioneImpegno.RateiSpecified = True
                objRichiestaGenerazioneImpegno.Ratei = ratei
            Else
                objRichiestaGenerazioneImpegno.Ratei = 0
            End If

            If Not String.IsNullOrEmpty(impostaIrap) Then
                objRichiestaGenerazioneImpegno.ImpostaIrapSpecified = True
                objRichiestaGenerazioneImpegno.ImpostaIrap = impostaIrap
            Else
                objRichiestaGenerazioneImpegno.ImpostaIrap = 0
            End If

            If Not String.IsNullOrEmpty(risconti) Then
                objRichiestaGenerazioneImpegno.RiscontiSpecified = True
                objRichiestaGenerazioneImpegno.Risconti = risconti
            Else
                objRichiestaGenerazioneImpegno.Risconti = 0
            End If

            Dim tot As Double = 0
            tot = tot + objRichiestaGenerazioneImpegno.ImpostaIrap
            tot = tot + objRichiestaGenerazioneImpegno.Ratei
            tot = tot + objRichiestaGenerazioneImpegno.Risconti

            If tot > objRichiestaGenerazioneImpegno.Importo Then
                Throw New Exception("I valori immessi superano il valore dell'importo")
            End If

            If Not String.IsNullOrEmpty(pcf) Then
                objRichiestaGenerazioneImpegno.PianoContiFina = pcf
            End If

            If Not listaben Is Nothing Then
                objRichiestaGenerazioneImpegno.Beneficiari = listaben
            End If

            objRichiestaGenerazioneImpegno.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.IMPDEF

            
            richiesta.Richiesta.Item = objRichiestaGenerazioneImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazioneImpegnoMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneImpegnoMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni
            'numImpegno = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)
            'Return numImpegno.NumeroDocumento
            

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function createGenerazioneLiquidazioneMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                        ByVal numimp As Integer, _
                                                        ByVal importo As String, _
                                                        ByVal tipoAtto As String, _
                                                        ByVal dataAtto As Date, _
                                                        ByVal numAtto As String, _
                                                        ByVal struttura As String, _
                                                        ByVal contoEconomico As String, _
                                                        ByVal importoIva As String, _
                                                        ByVal dataMovimento As DateTime, _
                                                        ByVal pcf As String, _
                                                        ByVal oggetto As String, _
                                                        ByVal idDocumento As String, _
                                                        ByVal numeroProvvisOrDefAtto As String, _
                                                        ByVal tokenSic As String, _
                                                        Optional ByVal listaben As Array = Nothing) As String()

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneLiquidazione As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneLiquidazione.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneLiquidazione.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazioneLiquidazione.IdUnivocoSistemaChiamante= idDocumento

            objRichiestaGenerazioneLiquidazione.DataMovimentoSpecified = True
            objRichiestaGenerazioneLiquidazione.DataMovimento = dataMovimento

            If Not Long.Parse(numimp) Then
                objRichiestaGenerazioneLiquidazione.NumeroDocumentoPC = numimp
            Else
                Throw New Exception("Numero impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneLiquidazione.ImportoSpecified = True
                objRichiestaGenerazioneLiquidazione.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazioneLiquidazione.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazioneLiquidazione.DataAttoSpecified = True
                objRichiestaGenerazioneLiquidazione.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazioneLiquidazione.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero atto non valido")
            End If

            If Not String.IsNullOrEmpty(oggetto) Then
                objRichiestaGenerazioneLiquidazione.Oggetto = oggetto
            Else
                Throw New Exception("Oggetto atto non valido")
            End If

            objRichiestaGenerazioneLiquidazione.Struttura = struttura

            If Not String.IsNullOrEmpty(contoEconomico) Then
                objRichiestaGenerazioneLiquidazione.ContoEconomica = contoEconomico
            Else
                ' Conto Economica non più
                'Throw New Exception("Conto Economica non valido")
            End If

            If Not String.IsNullOrEmpty(pcf) Then
                objRichiestaGenerazioneLiquidazione.PianoContiFina = pcf
            End If

            If Not String.IsNullOrEmpty(importoIva) Then
                objRichiestaGenerazioneLiquidazione.ImportoIVASpecified = True
                objRichiestaGenerazioneLiquidazione.ImportoIVA = importoIva
            Else
                objRichiestaGenerazioneLiquidazione.ImportoIVA = 0
            End If

            If Not listaben Is Nothing Then
                objRichiestaGenerazioneLiquidazione.Beneficiari = listaben
            End If


            Dim tot As Double = 0
            If objRichiestaGenerazioneLiquidazione.ImportoIVA > objRichiestaGenerazioneLiquidazione.Importo Then
                Throw New Exception("L'Iva non può superare l'importo")
            End If

            objRichiestaGenerazioneLiquidazione.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.LIQ

            richiesta.Richiesta.Item = objRichiestaGenerazioneLiquidazione
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazioneLiquidazioneMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneLiquidazioneMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            'Dim numLiquidazione As Risposta_CreateDelDocumento_Types


            'numLiquidazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

           
            'Return numLiquidazione.NumeroDocumento


            
            Dim numLiquidazione As Risposta_CreateDelDocumento_Types
            numLiquidazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            If numLiquidazione.NumeroDocumento = 0 And numLiquidazione.NumeroDocumento_Plur1 = 0 And numLiquidazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numLiquidazione.NumeroDocumento, numLiquidazione.NumeroDocumento_Plur1, numLiquidazione.NumeroDocumento_Plur2, numLiquidazione.DocId, numLiquidazione.DocId1, numLiquidazione.DocId2}

            Return registrazioni

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createInterrogazionePreImpegniApertiMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazionePreimpegniAperti As New InterrogazionePreimpegniAperti_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazionePreimpegniAperti.AnnoFinanziario = CInt(annoBilancio)
            Else
                objRichiestaInterrogazionePreimpegniAperti.AnnoFinanziario = Year(Now)
            End If

            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazionePreimpegniAperti.CodiceCapitolo = capitolo
            End If

            If Not String.IsNullOrEmpty(operatore.oUfficio.CodUfficioPubblico) Then
                objRichiestaInterrogazionePreimpegniAperti.Struttura = operatore.oUfficio.CodUfficioPubblico
            End If


            richiesta.Richiesta.Item = objRichiestaInterrogazionePreimpegniAperti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazionePreimpegniAperti
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayPreImpCapitolo As Risposta_InterrogazionePreimpegniAperti_Types

            arrayPreImpCapitolo = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_Types)

            Return DirectCast(arrayPreImpCapitolo.Preimpegno, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogazioneImpegniApertiMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String, ByVal codice_ufficio As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneImpegniAperti As New InterrogazioneImpegniAperti_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneImpegniAperti.AnnoFinanziario = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneImpegniAperti.AnnoFinanziario = Year(Now)
            End If
            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazioneImpegniAperti.CodiceCapitolo = capitolo
            Else
                Throw New Exception("Capitolo non valido")
            End If
            If Not String.IsNullOrEmpty(codice_ufficio) Then
                objRichiestaInterrogazioneImpegniAperti.Struttura = operatore.oUfficio.leggiUfficioPubblico(codice_ufficio)
            Else
                Throw New Exception("Struttura non valido")
            End If


            richiesta.Richiesta.Item = objRichiestaInterrogazioneImpegniAperti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneImpegniAperti
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayImpCapitolo As Risposta_InterrogazioneImpegniAperti_Types

            arrayImpCapitolo = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_Types)

            Return DirectCast(arrayImpCapitolo.Impegno, Array)

        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createGenerazioneRiduzioneMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                                ByVal numImp As Integer, _
                                                                ByVal importo As String, _
                                                                ByVal tipoAtto As String, _
                                                                ByVal dataAtto As Date, _
                                                                ByVal numAtto As String, _
                                                                ByVal dataMovimento As DateTime, _
                                                                ByVal struttura As String, _
                                                                ByVal oggetto As String, _
                                                                ByVal idDocumento As String, _
                                                                ByVal numeroProvvisOrDefAtto As String, _
                                                                ByVal tokenSic As String
                                                             ) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneRiduzione As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneRiduzione.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneRiduzione.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazioneRiduzione.IdUnivocoSistemaChiamante= idDocumento


            objRichiestaGenerazioneRiduzione.CodiceFiscale = operatore.CodiceFiscale

            objRichiestaGenerazioneRiduzione.DataMovimentoSpecified = True
            objRichiestaGenerazioneRiduzione.DataMovimento = dataMovimento

            If Not Long.Parse(numImp) Then
                objRichiestaGenerazioneRiduzione.NumeroDocumentoPC = numImp
            Else
                Throw New Exception("Numero impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneRiduzione.ImportoSpecified = True
                objRichiestaGenerazioneRiduzione.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazioneRiduzione.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazioneRiduzione.DataAttoSpecified = True
                objRichiestaGenerazioneRiduzione.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazioneRiduzione.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If
            If Not String.IsNullOrEmpty(oggetto) Then
                objRichiestaGenerazioneRiduzione.Oggetto = oggetto
            Else
                Throw New Exception("Oggetto non valido")
            End If

            objRichiestaGenerazioneRiduzione.Struttura = struttura

            objRichiestaGenerazioneRiduzione.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.VARIMP

            richiesta.Richiesta.Item = objRichiestaGenerazioneRiduzione
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazioneRiduzioneMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneRiduzioneMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Descrizione.Contains("mese non valido") Then
                    excep.Descrizione = "Errore SIC: mese non valido"
                End If
                Throw New Exception(excep.Descrizione)

            End If

            'Dim numRegistrazione As Risposta_CreateDelDocumento_Types
            'numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)
            'Return numRegistrazione.NumeroDocumento

            Dim numRegistrazione As Risposta_CreateDelDocumento_Types
            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createGenerazioneVariazionePreIMPMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                            ByVal numImp As Integer, _
                                                            ByVal importo As String, _
                                                            ByVal tipoAtto As String, _
                                                            ByVal dataAtto As Date, _
                                                            ByVal numAtto As String, _
                                                            ByVal dataMovimento As DateTime, _
                                                            ByVal oggettoAtto As String, _
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                            ByVal tokenSic As String
                                                            ) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneRiduzione As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneRiduzione.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneRiduzione.IdUnivocoChiamata = tokenSic 'stefffff verifica la funziona cosa fa e dove viene utilizzata (se dovrebbe essere aggiuta questa chiamata in altri punti) e il token che viene inviato: lo genero ex-novo o che si fa?
            objRichiestaGenerazioneRiduzione.IdUnivocoSistemaChiamante= idDocumento


            objRichiestaGenerazioneRiduzione.CodiceFiscale = operatore.CodiceFiscale

            objRichiestaGenerazioneRiduzione.DataMovimentoSpecified = True
            objRichiestaGenerazioneRiduzione.DataMovimento = dataMovimento

            If Not Long.Parse(numImp) Then
                objRichiestaGenerazioneRiduzione.NumeroDocumentoPC = numImp
            Else
                Throw New Exception("Numero impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneRiduzione.ImportoSpecified = True
                objRichiestaGenerazioneRiduzione.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazioneRiduzione.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazioneRiduzione.DataAttoSpecified = True
                objRichiestaGenerazioneRiduzione.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazioneRiduzione.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If

            If Not String.IsNullOrEmpty(oggettoAtto) Then
                objRichiestaGenerazioneRiduzione.Oggetto = oggettoAtto
            Else
                Throw New Exception("Oggetto Atto non valido")
            End If


            objRichiestaGenerazioneRiduzione.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.VARPREIMP

            richiesta.Richiesta.Item = objRichiestaGenerazioneRiduzione
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazioneVariazionePreIMPMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneVariazionePreIMPMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If

            Dim numRegistrazione As Risposta_CreateDelDocumento_Types
            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createEliminazioneDocumentoMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                             ByVal tipoDocumento As String, _
                                                            ByVal numImp As Integer, _
                                                            ByVal tipoAtto As String, _
                                                            ByVal dataAtto As Date, _
                                                            ByVal numAtto As String, _
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                            ByVal tokenSic As String
                                                              ) As String
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaEliminazioneDocumento As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaEliminazioneDocumento.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaEliminazioneDocumento.IdUnivocoChiamata = tokenSic
            objRichiestaEliminazioneDocumento.IdUnivocoSistemaChiamante = idDocumento

            objRichiestaEliminazioneDocumento.CodiceFiscale = operatore.CodiceFiscale

            objRichiestaEliminazioneDocumento.DataMovimentoSpecified = True
            Dim annoDocumentoContabile As String = numImp.ToString
            If (CInt(annoDocumentoContabile.Substring(0, 4)) < Year(Now)) Then
                objRichiestaEliminazioneDocumento.DataMovimento = New Date(CInt(annoDocumentoContabile.Substring(0, 4)), 12, 31)
            Else
                objRichiestaEliminazioneDocumento.DataMovimento = Now
            End If

            If Not Long.Parse(numImp) Then
                objRichiestaEliminazioneDocumento.NumeroDocumentoPC = numImp
            Else
                Throw New Exception("Numero impegno non valido")
            End If



            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaEliminazioneDocumento.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaEliminazioneDocumento.DataAttoSpecified = True
                objRichiestaEliminazioneDocumento.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaEliminazioneDocumento.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If
            objRichiestaEliminazioneDocumento.TipoDocumento = tipoDocumento

            richiesta.Richiesta.Item = objRichiestaEliminazioneDocumento
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createEliminazioneDocumentoMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createEliminazioneDocumentoMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)



            Return numRegistrazione.NumeroDocumento

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function
    Public Shared Function createEliminazioneImpMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                             ByVal numImp As String, _
                                                             ByVal tipoAtto As String, _
                                                            ByVal dataAtto As Date, _
                                                            ByVal numAtto As String, _
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                                ByVal tokenSic As String) As String
        Try
            Return createEliminazioneDocumentoMessage(operatore, CreateDelDocumento_TypesTipoDocumento.DELIMP, numImp, tipoAtto, dataAtto, numAtto, idDocumento, numeroProvvisOrDefAtto, tokenSic)

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function
    Public Shared Function createEliminazioneLiqMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                          ByVal numLiq As String, _
                                                          ByVal tipoAtto As String, _
                                                         ByVal dataAtto As Date, _
                                                         ByVal numAtto As String, _
                                                        ByVal idDocumento As String, _
                                                        ByVal numeroProvvisOrDefAtto As String, _
                                                                ByVal tokenSic As String) As Boolean
        Try
            createEliminazioneDocumentoMessage(operatore, CreateDelDocumento_TypesTipoDocumento.DELLIQ, numLiq, tipoAtto, dataAtto, numAtto,  idDocumento, numeroProvvisOrDefAtto, tokenSic)
            Return True
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return False

    End Function
    Public Shared Function createEliminazionePreImpMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                        ByVal numPreImp As String, _
                                                        ByVal tipoAtto As String, _
                                                        ByVal dataAtto As Date, _
                                                        ByVal numAtto As String, _
                                                        ByVal idDocumento As String, _
                                                        ByVal numeroProvvisOrDefAtto As String, _
                                                        ByVal tokenSic As String) As String
        Try
            If numAtto.Length > 5 Then
                numAtto = Right(numAtto, 5)
            End If
            Return createEliminazioneDocumentoMessage(operatore, CreateDelDocumento_TypesTipoDocumento.DELPREIMP, numPreImp, tipoAtto, dataAtto, numAtto, idDocumento, numeroProvvisOrDefAtto, tokenSic)

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function
    Public Shared Function createEliminazioneEcoMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                        ByVal numEco As Integer, _
                                                        ByVal tipoAtto As String, _
                                                        ByVal dataAtto As Date, _
                                                        ByVal numAtto As String, _
                                                        ByVal idDocumento As String, _
                                                        ByVal numeroProvvisOrDefAtto As String, _
                                                                ByVal tokenSic As String) As String
        Try
            Return createEliminazioneDocumentoMessage(operatore, CreateDelDocumento_TypesTipoDocumento.DELECO, numEco, tipoAtto, dataAtto, numAtto, idDocumento, numeroProvvisOrDefAtto, tokenSic)

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function
    Public Shared Function createGenerazioneImpegnoPerenteMessage(ByVal operatore As DllAmbiente.Operatore,
                                                            ByVal oggetto As String,
                                                            ByVal importo As String,
                                                            ByVal tipoAtto As String,
                                                            ByVal dataAtto As Date,
                                                            ByVal numAtto As String,
                                                            ByVal dipartimento As String,
                                                            ByVal contoEconomico As String,
                                                            ByVal ratei As String,
                                                            ByVal impostaIrap As String,
                                                            ByVal risconti As String,
                                                            ByVal numImpPrecedente As String,
                                                            ByVal capitolo As String,
                                                            ByVal dataMovimento As DateTime,
                                                            ByVal Codice_Obbiettivo_Gestionale As String,
                                                            ByVal Piano_dei_Conti_Finanziario As String, 
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                            ByVal tokenSic As String,
                                                            Optional ByVal listaben As Array = Nothing) As String()

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneImpegnoPerente As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneImpegnoPerente.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneImpegnoPerente.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazioneImpegnoPerente.IdUnivocoSistemaChiamante = idDocumento

            objRichiestaGenerazioneImpegnoPerente.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.IMPPER
            objRichiestaGenerazioneImpegnoPerente.DataMovimentoSpecified = True
            objRichiestaGenerazioneImpegnoPerente.DataMovimento = dataMovimento
            objRichiestaGenerazioneImpegnoPerente.NumeroDocumentoPC = numImpPrecedente
            objRichiestaGenerazioneImpegnoPerente.Oggetto = oggetto
            objRichiestaGenerazioneImpegnoPerente.CodiceCapitolo = capitolo

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneImpegnoPerente.ImportoSpecified = True
                objRichiestaGenerazioneImpegnoPerente.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(tipoAtto) Then
                objRichiestaGenerazioneImpegnoPerente.TipoAtto = tipoAtto
            Else
                Throw New Exception("Tipo Atto non valido")
            End If

            If IsDate(dataAtto) Then
                objRichiestaGenerazioneImpegnoPerente.DataAttoSpecified = True
                objRichiestaGenerazioneImpegnoPerente.DataAtto = dataAtto
            Else
                Throw New Exception("Data atto non valido")
            End If
            If Not String.IsNullOrEmpty(numAtto) Then
                objRichiestaGenerazioneImpegnoPerente.NumeroAtto = numAtto
            Else
                Throw New Exception("Numero Atto non valido")
            End If

            objRichiestaGenerazioneImpegnoPerente.Struttura = dipartimento

            If Not String.IsNullOrEmpty(contoEconomico) Then
                objRichiestaGenerazioneImpegnoPerente.ContoEconomica = contoEconomico
            Else
                'Conto Economica non è + obbligatorio
                'Throw New Exception("Conto Economica non valido")
            End If

            If Not String.IsNullOrEmpty(Codice_Obbiettivo_Gestionale) Then
                objRichiestaGenerazioneImpegnoPerente.COG = Codice_Obbiettivo_Gestionale
            Else
                Throw New Exception("Codice Obiettivo Gestionale non valido")
            End If

            If Not String.IsNullOrEmpty(Piano_dei_Conti_Finanziario) Then
                objRichiestaGenerazioneImpegnoPerente.PianoContiFina = Piano_dei_Conti_Finanziario
            Else
                Throw New Exception("Piano dei Conti Finanziario non valido")
            End If

            If Not String.IsNullOrEmpty(ratei) Then
                objRichiestaGenerazioneImpegnoPerente.RateiSpecified = True
                objRichiestaGenerazioneImpegnoPerente.Ratei = ratei
            Else
                objRichiestaGenerazioneImpegnoPerente.Ratei = 0
            End If

            If Not String.IsNullOrEmpty(impostaIrap) Then
                objRichiestaGenerazioneImpegnoPerente.ImpostaIrapSpecified = True
                objRichiestaGenerazioneImpegnoPerente.ImpostaIrap = impostaIrap
            Else
                objRichiestaGenerazioneImpegnoPerente.ImpostaIrap = 0
            End If

            If Not String.IsNullOrEmpty(risconti) Then
                objRichiestaGenerazioneImpegnoPerente.RiscontiSpecified = True
                objRichiestaGenerazioneImpegnoPerente.Risconti = risconti
            Else
                objRichiestaGenerazioneImpegnoPerente.Risconti = 0
            End If

            Dim tot As Double = 0
            tot = tot + objRichiestaGenerazioneImpegnoPerente.ImpostaIrap
            tot = tot + objRichiestaGenerazioneImpegnoPerente.Ratei
            tot = tot + objRichiestaGenerazioneImpegnoPerente.Risconti

            If tot > objRichiestaGenerazioneImpegnoPerente.Importo Then
                Throw New Exception("I valori immessi superano il valore dell'importo")
            End If

            If Not listaben Is Nothing Then
                objRichiestaGenerazioneImpegnoPerente.Beneficiari = listaben
            End If

            richiesta.Richiesta.Item = objRichiestaGenerazioneImpegnoPerente
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Log.Info("createGenerazioneImpegnoPerenteMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneImpegnoPerenteMessage OUTPUT SIC: " + outputQuerySic.return)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)

                Throw New Exception(excep.Descrizione)

            End If
            'Dim numImpegnoPer As Risposta_CreateDelDocumento_Types

            'numImpegnoPer = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            'Return numImpegnoPer.NumeroDocumento

            Dim numRegistrazione As Risposta_CreateDelDocumento_Types
            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)

            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni

        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createDisponibilitaImpegnoMessage(ByVal operatore As DllAmbiente.Operatore, ByVal numeroImpegno As String) As Risposta_DisponibilitaImpegno_Types

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaDisponibilitaImpegno As New DisponibilitaImpegno_Types
            If IsNumeric(numeroImpegno) AndAlso CInt(numeroImpegno) <> 0 Then
                objRichiestaDisponibilitaImpegno.NumeroImpegno = numeroImpegno
            Else
                Throw New Exception("Numero Impegno Perente non valorizzato")
            End If

            richiesta.Richiesta.Item = objRichiestaDisponibilitaImpegno
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.DisponibilitaImpegno

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim dettaglioImpegno As Risposta_DisponibilitaImpegno_Types

            dettaglioImpegno = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_DisponibilitaImpegno_Types)

            Return dettaglioImpegno
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogazioneLiquidazioniAperteMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneLiquidazioniAperti As New InterrogazioneLiquidazioniAperte_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneLiquidazioniAperti.AnnoFinanziario = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneLiquidazioniAperti.AnnoFinanziario = Year(Now)
            End If
            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazioneLiquidazioniAperti.CodiceCapitolo = capitolo
            Else

            End If


            richiesta.Richiesta.Item = objRichiestaInterrogazioneLiquidazioniAperti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneLiquidazioniAperte

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayLiquidazioniCapitolo As Risposta_InterrogazioneLiquidazioniAperte_Types

            arrayLiquidazioniCapitolo = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazioneLiquidazioniAperte_Types)
            Return DirectCast(arrayLiquidazioniCapitolo.Liquidazione, Array)

        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogazioneImpegniPerentiApertiMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneImpegniPerentiAperti As New InterrogazioneImpegniPerenti_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneImpegniPerentiAperti.AnnoFinanziario = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneImpegniPerentiAperti.AnnoFinanziario = Year(Now)
            End If
            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazioneImpegniPerentiAperti.CodiceCapitolo = capitolo
            Else

            End If


            richiesta.Richiesta.Item = objRichiestaInterrogazioneImpegniPerentiAperti
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneImpegniPerenti
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayImpegniPerenti As Risposta_InterrogazioneImpegniPerenti_Types

            arrayImpegniPerenti = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_Types)

            Return DirectCast(arrayImpegniPerenti.ImpegnoPerente, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createGenerazioneVariazioneLiqMessage(ByVal operatore As DllAmbiente.Operatore, _
                                                            ByVal numliq As Integer, _
                                                            ByVal importo As String, _
                                                            ByVal dataMovimento As DateTime, _
                                                            ByVal oggettoAtto As String,
                                                            ByVal numAtto As String,
                                                            ByVal idDocumento As String, _
                                                            ByVal numeroProvvisOrDefAtto As String, _
                                                            ByVal tokenSic As String) As String()
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaGenerazioneRiduzione As New CreateDelDocumento_Types

            'GESTIONE TOKEN
            objRichiestaGenerazioneRiduzione.IdentificativoAtto = numeroProvvisOrDefAtto
            objRichiestaGenerazioneRiduzione.IdUnivocoChiamata = tokenSic
            objRichiestaGenerazioneRiduzione.IdUnivocoSistemaChiamante = idDocumento


            objRichiestaGenerazioneRiduzione.CodiceFiscale = operatore.CodiceFiscale

            objRichiestaGenerazioneRiduzione.DataMovimentoSpecified = True
            objRichiestaGenerazioneRiduzione.DataMovimento = dataMovimento

            If Not Long.Parse(numliq) Then
                objRichiestaGenerazioneRiduzione.NumeroDocumentoPC = numliq
            Else
                Throw New Exception("Numero impegno non valido")
            End If

            If Not Double.IsNegativeInfinity(importo) Then
                objRichiestaGenerazioneRiduzione.ImportoSpecified = True
                objRichiestaGenerazioneRiduzione.Importo = importo
            Else
                Throw New Exception("Importo non valido")
            End If

            If Not String.IsNullOrEmpty(oggettoAtto) Then
                objRichiestaGenerazioneRiduzione.Oggetto = oggettoAtto
            Else
                Throw New Exception("Oggetto Atto non valido")
            End If


            objRichiestaGenerazioneRiduzione.TipoDocumento = CreateDelDocumento_TypesTipoDocumento.VARLIQ

            richiesta.Richiesta.Item = objRichiestaGenerazioneRiduzione
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.CreateDelDocumento

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)
            Log.Info("createGenerazioneVariazioneLiqMessage INPUT SIC: " + inputQuerySic.q)
            Log.Info("createGenerazioneVariazioneLiqMessage OUTPUT SIC: " + outputQuerySic.return)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim numRegistrazione As Risposta_CreateDelDocumento_Types

            numRegistrazione = DirectCast(risposta.Item.Item, Risposta_CreateDelDocumento_Types)
            
            
            If numRegistrazione.NumeroDocumento = 0 And numRegistrazione.NumeroDocumento_Plur1 = 0 And numRegistrazione.NumeroDocumento_Plur2 = 0 Then
                Throw New Exception("Registrazione sul SIC non avvenuta")
            End If

            Dim registrazioni As String() = _
                {numRegistrazione.NumeroDocumento, numRegistrazione.NumeroDocumento_Plur1, numRegistrazione.NumeroDocumento_Plur2, numRegistrazione.DocId, numRegistrazione.DocId1, numRegistrazione.DocId2}

            Return registrazioni
            
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Shared Function createInterrogazioneCapitoliLiquidazioniMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal codice_ufficio As String) As Array
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneCapitoli As New InterrogazioneCapitoli_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneCapitoli.AnnoBilancio = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneCapitoli.AnnoBilancio = Year(Now)
            End If

            If String.IsNullOrEmpty(codice_ufficio) Then
                objRichiestaInterrogazioneCapitoli.Struttura = operatore.oUfficio.leggiUfficioPubblico(operatore.oUfficio.CodUfficio)
            Else
                Dim uffTemp As New DllAmbiente.Ufficio
                uffTemp.CodUfficio = codice_ufficio
                objRichiestaInterrogazioneCapitoli.Struttura = uffTemp.leggiUfficioPubblico(uffTemp.CodUfficio)
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneCapitoli
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneCapitoliLiquidazioni

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayCapitolo As Risposta_InterrogazioneCapitoli_Types

            arrayCapitolo = DirectCast(risposta.Item.Item, Risposta_InterrogazioneCapitoli_Types)

            Return DirectCast(arrayCapitolo.Capitolo, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try


    End Function
    Public Shared Function createInterrogazioneListaCogMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneObiettiviGestionali As New InterrogazioneObiettiviGestionali_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneObiettiviGestionali.Anno = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneObiettiviGestionali.Anno = Year(Now)
            End If
            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazioneObiettiviGestionali.CodiceCapitolo = capitolo
            Else
                Throw New Exception("Capitolo non valido")
            End If
            If Not String.IsNullOrEmpty(operatore.oUfficio.CodUfficioPubblico) Then
                objRichiestaInterrogazioneObiettiviGestionali.Ufficio = operatore.oUfficio.CodUfficioPubblico
            Else
                objRichiestaInterrogazioneObiettiviGestionali.Ufficio = operatore.oUfficio.leggiUfficioPubblico(operatore.oUfficio.CodUfficio)
            End If


            richiesta.Richiesta.Item = objRichiestaInterrogazioneObiettiviGestionali
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneObiettiviGestionali
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayObiettiviGestionali As Risposta_InterrogazioneObiettiviGestionali_Types

            arrayObiettiviGestionali = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazioneObiettiviGestionali_Types)

            Return DirectCast(arrayObiettiviGestionali.ObiettivoGestionale, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogaBeneficiariLiqMessage(ByVal operatore As DllAmbiente.Operatore, ByVal NumeroLiquidazione As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogaBeneficiariLiq As New InterrogaBeneficiariLiq_Types
            If IsNumeric(NumeroLiquidazione) AndAlso CInt(NumeroLiquidazione) <> 0 Then
                objRichiestaInterrogaBeneficiariLiq.NumeroLiquidazione = CInt(NumeroLiquidazione)
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogaBeneficiariLiq

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Log.Debug("Risposta SIC:" & excep.Descrizione)
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayBeneficiariLiq As Risposta_InterrogaBeneficiariLiq_Types

            arrayBeneficiariLiq = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogaBeneficiariLiq_Types)
            Return DirectCast(arrayBeneficiariLiq.Beneficiario, Array)

        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogazioneCodiciSiopeMessage(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogazioneCodiceSiope As New InterrogaCodiceSiope_Types
            If IsNumeric(annoBilancio) AndAlso CInt(annoBilancio) <> 0 Then
                objRichiestaInterrogazioneCodiceSiope.AnnoBilancio = CInt(annoBilancio)
            Else
                objRichiestaInterrogazioneCodiceSiope.AnnoBilancio = Year(Now)
            End If
            If Not String.IsNullOrEmpty(capitolo) Then
                objRichiestaInterrogazioneCodiceSiope.CodiceCapitolo = capitolo
            Else
                Throw New Exception("Capitolo non valido")
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogazioneCodiceSiope
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogaCodiceSiope
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                Log.Debug("Risposta SIC:" & excep.Descrizione)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayCodiceSiope As Risposta_InterrogaCodiceSiope_Types

            arrayCodiceSiope = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogaCodiceSiope_Types)

            Return DirectCast(arrayCodiceSiope.Siope, Array)
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createInterrogazioneListaTipologiaReddito(ByVal operatore As DllAmbiente.Operatore) As Array

        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            richiesta.Richiesta.Item = New Object 'Intema.WS.Richiesta.Richiesta_TypesInterrogazioneTipologiaReddito            
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazioneTipologiaReddito
            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayTipologiaReddito As Risposta_InterrogaTipologiaReddito_Types

            arrayTipologiaReddito = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogaTipologiaReddito_Types)

            Return DirectCast(arrayTipologiaReddito.Tipologia, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createInterrogazionePianoDeiContiFinanziario(ByVal operatore As DllAmbiente.Operatore, ByVal annoBilancio As String, ByVal capitolo As String) As Array
        Try
            Dim richiesta As New MessaggioRichiesta_Types
            richiesta.Intestazione = New Intema.WS.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim r As New Intema.WS.Richiesta.InterrogazionePCF_Types()
            richiesta.Richiesta.Item = r
            richiesta.Richiesta.ItemElementName = Intema.WS.Richiesta.ItemChoiceType.InterrogazionePCF
            r.Anno = annoBilancio
            r.CodiceCapitolo = capitolo

            Dim WS As New SICInterface(ConfigurationManager.AppSettings("SicWS"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New querySIC
            inputQuerySic.q = SerializeIt(richiesta)
            Dim outputQuerySic As querySICResponse = WS.querySIC(inputQuerySic)

            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayPCF As Risposta_InterrogazionePCF_Types

            arrayPCF = DirectCast(risposta.Item.Item, Intema.WS.Risposta.Risposta_InterrogazionePCF_Types)

            Return DirectCast(arrayPCF.PianoDeiContiFina, Array)
        Catch ex As ListaVuotaException
            Throw ex
        Catch ex As InvalidCastException
            Throw New InvalidCastException("")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


    Public Shared Function notificaAttoFatturaSIC(ByVal operatore As DllAmbiente.Operatore, ByVal numDefAtto As String, ByVal numProvAtto As String, ByVal tipoDocContabile As String, ByVal numLiquidazione As Integer, ByVal numPreimpegno As Integer, ByVal numImpegno As Integer, ByVal idFatturaSIC As String, ByVal statoOperazione As String, ByVal dataMovimentoOpContabile As Date, ByRef importo As Double, ByRef P_ESITO As Double) As String
        Dim messaggioSic As String = ""
        Try
            System.Net.ServicePointManager.Expect100Continue = False

            Dim notificaAttoFatturaService As New NOTIFICAATTOFATTURAServiceOverride()

            notificaAttoFatturaService.AllowAutoRedirect = True
            notificaAttoFatturaService.PreAuthenticate = True

            Dim usernameSicWSFatturazione As String = ConfigurationManager.AppSettings("UsernameSicWSFatturazione")
            Dim passwordSicWSFatturazione As String = ConfigurationManager.AppSettings("PasswordSicWSFatturazione")

            Dim cr As New System.Net.NetworkCredential(usernameSicWSFatturazione, passwordSicWSFatturazione)
            Dim credentialCache As New Net.CredentialCache()
            credentialCache.Add(New Uri(notificaAttoFatturaService.Url), "Basic", cr)

            notificaAttoFatturaService.Credentials = credentialCache


            Dim notificaProvvedimentiSIC_Input As New ClientIntegrazioneSic.Intema.WS.Richiesta.NOTIFICAPROVVEDIMENTISICInput

            notificaProvvedimentiSIC_Input.P_TIPODOCSICVARCHAR2IN = tipoDocContabile

            Select Case tipoDocContabile
                Case Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString
                    notificaProvvedimentiSIC_Input.P_NUMERODOCSICNUMBERIN = numLiquidazione
                Case Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.IMPDEF.ToString
                    notificaProvvedimentiSIC_Input.P_NUMERODOCSICNUMBERIN = numImpegno
                Case Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.IMPPER.ToString
                    notificaProvvedimentiSIC_Input.P_NUMERODOCSICNUMBERIN = numImpegno
                Case Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.PREIMP.ToString
                    notificaProvvedimentiSIC_Input.P_NUMERODOCSICNUMBERIN = numPreimpegno
            End Select

            notificaProvvedimentiSIC_Input.P_STATONOTIFICAVARCHAR2IN = statoOperazione
            Dim idFatturaSIC_Integer As Integer
            Integer.TryParse(idFatturaSIC, idFatturaSIC_Integer)
            notificaProvvedimentiSIC_Input.P_IDFATTURANUMBERIN = idFatturaSIC_Integer

            If Not numDefAtto Is Nothing And numDefAtto <> "" Then
                notificaProvvedimentiSIC_Input.P_IDATTOVARCHAR2IN = numDefAtto
            Else
                notificaProvvedimentiSIC_Input.P_IDATTOVARCHAR2IN = numProvAtto
            End If

            Dim MM As String = ""
            If dataMovimentoOpContabile.Month < 10 Then
                MM = "0" & dataMovimentoOpContabile.Month
            Else
                MM = dataMovimentoOpContabile.Month
            End If
            Dim gg As String = ""
            If dataMovimentoOpContabile.Day < 10 Then
                gg = "0" & dataMovimentoOpContabile.Day
            Else
                gg = dataMovimentoOpContabile.Day
            End If

            notificaProvvedimentiSIC_Input.P_DATADOCSICVARCHAR2IN = gg & MM & dataMovimentoOpContabile.Year
            notificaProvvedimentiSIC_Input.P_IMPORTONUMBERIN = importo
            Dim messaggioVarchar2out As New ClientIntegrazioneSic.NOTIFICAPROVVEDIMENTISICInputP_MESSAGGIOVARCHAR2OUT
            Dim messaggioEsitoNumberOut As New ClientIntegrazioneSic.NOTIFICAPROVVEDIMENTISICInputP_ESITONUMBEROUT

            messaggioSic = notificaAttoFatturaService.NOTIFICAPROVVEDIMENTISIC(notificaProvvedimentiSIC_Input.P_TIPODOCSICVARCHAR2IN, _
                                                                               notificaProvvedimentiSIC_Input.P_STATONOTIFICAVARCHAR2IN, _
                                                                               notificaProvvedimentiSIC_Input.P_NUMERODOCSICNUMBERIN, _
                                                                               messaggioVarchar2out, _
                                                                               notificaProvvedimentiSIC_Input.P_IDFATTURANUMBERIN, _
                                                                               notificaProvvedimentiSIC_Input.P_IDATTOVARCHAR2IN, _
                                                                               messaggioEsitoNumberOut, _
                                                                               notificaProvvedimentiSIC_Input.P_DATADOCSICVARCHAR2IN, _
                                                                               notificaProvvedimentiSIC_Input.P_IMPORTONUMBERIN, _
                                                                               P_ESITO)
            If messaggioSic.ToLower().Contains("errore") Then
                Throw New Exception(messaggioSic)
            End If
            Return messaggioSic
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getElencoFattureByContratto(ByVal idContratto As String) As ClientIntegrazioneSic.ELENCOFATTUREOutputP_RETURN
        Dim messaggioSic As String = ""
        Try
            System.Net.ServicePointManager.Expect100Continue = False

            Dim fatturaElettronicaService As New FATTURAELETTRONICAServiceOverride()

            fatturaElettronicaService.AllowAutoRedirect = True
            fatturaElettronicaService.PreAuthenticate = True

            Dim cr As New System.Net.NetworkCredential(ConfigurationManager.AppSettings("UsernameSicWSFatturazione"), ConfigurationManager.AppSettings("PasswordSicWSFatturazione"))
            Dim credentialCache As New Net.CredentialCache()
            credentialCache.Add(New Uri(fatturaElettronicaService.Url), "Basic", cr)

            fatturaElettronicaService.Credentials = credentialCache
            Dim rispostaElencoFatture As ELENCOFATTUREOutputP_RETURN = Nothing
            Try
                rispostaElencoFatture = fatturaElettronicaService.ELENCOFATTURE(New ELENCOFATTUREInputP_RETURNXMLTYPEOUT, idContratto)
            Catch ex As Exception
                Throw New SicException(ex)
            End Try

            If rispostaElencoFatture Is Nothing Then
                Throw New ListaVuotaException("Risposta vuota")
            End If
            If rispostaElencoFatture.elenco_fatture Is Nothing Then
                Throw New ListaVuotaException("Lista vuota")
            End If

            Return rispostaElencoFatture
        Catch sicex As SicException
            Throw sicex
        Catch lse As ListaVuotaException
            Throw lse
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
