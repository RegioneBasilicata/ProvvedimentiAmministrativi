Imports log4net

Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Security.Cryptography
Imports ClientIntegrazioneSic.Intema.WS.Risposta
Imports iTextSharp.text
Imports Org.BouncyCastle.Bcpg

' NOTA: se si modifica il nome della classe "ProvvedimentiService" qui, è necessario aggiornare anche il riferimento a "ProvvedimentiService" in Web.config e nel file .svc associato.
Enum StatoProvvedimento As Integer
    Creato = 0
    Archiviato = 1
    Modificato = 2
    Altro = 3
    Annullato = 9
End Enum
Enum StatoOpContabile As Integer
    Cancellato = 0
    Registrato = 1
    InAttesaDiConferma = 2
End Enum

Enum TipoAllegatiDb As Integer
    AllegatoGeneric = 0
    Determina = 1
    Delibera = 5
    Disposizione = 8
    AllegatoUfficio = 4
    AllegatoNotaCartaceo = 15
    ElencoCertificati = 16
    AllegatoPremessa_Dispositivo = 22
    AllegatoPor = 55

End Enum


Public Class ProvvedimentiService
    Implements ProvvedimentiPortType

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ProvvedimentiPortType))
    'Errori 
    '0001 Richiesta scorretta
    '0002 Operatore Non Abilitato verificare Codice Fiscale
    '0003 Autorizzazione Negata
    '0004 Numero provvisorio e numero definitivo Mancante
    '0005 Documento non trovato
    '0006 Generico con numero provvisorio
    '0007 Verificare Codice Obiettivo Gestionale
    '0008 Verificare Piano dei Conti Finanziario
    '9998 Generico campo mancante
    '9999 Generico

#Region "Gestione Eccezioni"
    Function CreaEccezionePerTipoRichiesta(ByVal tipoRichiesta As Type, ByVal tipoMetodo As Type) As Eccezione_Types
        Dim ecc As New Eccezione_Types
        If Not tipoRichiesta Is tipoMetodo Then
            ecc.Codice = "0001"
            ecc.Descrizione = "Richiesto " & tipoMetodo.ToString & " e non " & tipoRichiesta.ToString
            ecc.Exception = "impossibile analizzare la richiesta per il metodo Richiesto"
        Else
            ecc = Nothing

        End If

        Return ecc
    End Function

    Function CreaEccezioneOperatore(ByVal op As DllAmbiente.Operatore) As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore
        If op Is Nothing Then
            ecc.Codice = "0002"
            ecc.Descrizione = "Operatore Non Abilitato verificare Codice Fiscale"
            ecc.Exception = "Operatore Non Abilitato verificare Codice Fiscale"

        Else
            ecc = Nothing
        End If

        Return ecc
    End Function

    Function CreaEccezioneCampoObbligatorio(ByVal nomecampo As String) As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "9998"
        ecc.Descrizione = "campo " & nomecampo & " mancante "
        ecc.Exception = nomecampo & ": è un campo obbligatorio "



        Return ecc
    End Function

    Function CreaEccezioneCampoObbligatorioBeneficiario(ByVal nomecampo As String) As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "9998"
        ecc.Descrizione = "campo " & nomecampo & " mancante nella definizione del beneficiario "
        ecc.Exception = nomecampo & ": è un campo obbligatorio "

        Return ecc
    End Function

    Function CreaEccezioneAutorizzazione() As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0003"
        ecc.Descrizione = "Autorizzazione Negata"
        ecc.Exception = "Autorizzazione Negata"


        Return ecc
    End Function

    Function CreaEccezioneNumDefeNumProvvMancante() As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0004"
        ecc.Descrizione = "Effettuare la richiesta passando o il numero provvisorio dell atto o il numero definitivo"
        ecc.Exception = "Numero provvisorio e numero definitivo Mancante"


        Return ecc
    End Function

   

    Function CreaEccezioneGenerica(Optional ByVal testoEccezione As String = "Errore Generico", Optional ByVal desc As String = "Errore Generico", Optional ByVal codiceErrore As String = "9999") As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = codiceErrore
        ecc.Descrizione = desc
        ecc.Exception = testoEccezione


        Return ecc
    End Function


    Function CreaEccezioneDocumentoNonTrovato() As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0005"
        ecc.Descrizione = "Documento Non Trovato"
        ecc.Exception = "Documento Non Trovato"


        Return ecc
    End Function


    Function CreaEccezioneCOG() As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0007"
        ecc.Descrizione = "Verificare Codice Obiettivo Gestionale"
        ecc.Exception = "Codice Obiettivo Gestionale non valido"
        Return ecc
    End Function

    Function CreaEccezionePianoDeiContiFinanziario(Optional ByVal messaggio As String = "") As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0008"
        ecc.Descrizione = "Verificare Piano dei Conti Finanziario"
        ecc.Exception = "Piano dei Conti Finanziario non valido " & messaggio
        Return ecc
    End Function

    Function CreaEccezioneLiquidazione(Optional ByVal messaggio As String = "") As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0009"
        ecc.Descrizione = "Verificare la liquidazione"
        ecc.Exception = "Errore su Liquidazione" & messaggio
        Return ecc
    End Function

    Function CreaEccezioneBeneficiarioImpegno(Optional ByVal messaggio As String = "") As Eccezione_Types
        Dim ecc As New Eccezione_Types
        'Errore

        ecc.Codice = "0010"
        ecc.Descrizione = "Verificare il beneficiario sull'impegno"
        ecc.Exception = "Beneficiario sull'impegno non valido " & messaggio
        Return ecc
    End Function


#End Region


    'Public Function GenerateHashTokenCallSic() As String
    '    Dim g as Guid = Guid.NewGuid()
    '    Return g.ToString()
    'End Function

    Public Function GetMd5Hash(ByVal md5Hash As MD5, ByVal input As String) As String

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()
    End Function

    Function InserisciMandati(ByVal request As InserisciMandatiRequest) As InserisciMandatiResponse Implements ProvvedimentiPortType.InserisciMandati

        Dim richiestaMandato As InterrogazioneMandato_Types = CType(request.Messaggio_Richiesta.Richiesta.Item, InterrogazioneMandato_Types)

        Dim listaMandati() As Mandato_Types = richiestaMandato.ListaMandati
        Dim dllDoc As New DllDocumentale.svrDocumenti(Nothing)
        Dim itemMandato As DllDocumentale.ItemMandatoInfo = Nothing
        Dim itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo

        Dim errore As Eccezione_Types = Nothing
        Dim contaInserimenti As Integer = 0
        Try
            Log.Info("InserisciMandati: " & " Mandati Atto: " & richiestaMandato.NumeroAtto & " tipo atto:  " & richiestaMandato.TipoAtto & " del:  " & richiestaMandato.DataAtto)
            If listaMandati Is Nothing Then
                Log.Info("lista Mandati nothing")
            Else
                Log.Info("lista Mandati " & listaMandati.Length)
            End If



            For Each mandato As Mandato_Types In listaMandati
                Log.Info("InserisciMandati: " & " Mandato : " & mandato.NumeroMandato)
                Log.Info("InserisciMandati: " & " Liq Mandato : " & mandato.NumLiquidazione)
                itemLiquidazione = dllDoc.FO_Get_DatiLiquidazioneByNLiquidazione(mandato.NumLiquidazione)
                Log.Info("Fine chiamata  dllDoc.FO_Get_DatiLiquidazioneByNLiquidazione ")
                If Not itemLiquidazione Is Nothing AndAlso Not itemLiquidazione.Dli_Documento Is Nothing Then
                    Log.Info("Inserisco mandato: " & mandato.NumeroMandato)
                    itemMandato = New DllDocumentale.ItemMandatoInfo
                    itemMandato.Man_DataMandato = mandato.DataMandato
                    itemMandato.Man_NImporto = mandato.ImportoPagato
                    itemMandato.Man_Nimpegno = mandato.NumeroImpegno
                    itemMandato.Man_NLiquidazione = mandato.NumLiquidazione
                    itemMandato.Man_Nmandato = mandato.NumeroMandato
                    itemMandato.Man_Doc_id = itemLiquidazione.Dli_Documento

                    dllDoc.FO_Insert_Mandato(itemMandato)
                    contaInserimenti += 1
                Else

                    ''
                    If errore Is Nothing Then
                        errore = New Eccezione_Types
                        errore.Codice = "1"
                        errore.Descrizione = ""
                        errore.Exception = "Liquidazione " & mandato.NumLiquidazione & " non trovata  per il mandato numero " & mandato.NumeroMandato & ". "
                    End If

                    errore.Descrizione = errore.Descrizione & "Errore nell'inserimento del mandato " & mandato.NumeroMandato & ". "
                    Log.Error(errore.Exception)
                End If

            Next

        Catch ex As Exception
            errore = New Eccezione_Types
            errore.Descrizione = "Errore generico."
            errore.Codice = "9999"
            errore.Exception = ex.Message
            Log.Error(ex.Message)
        End Try

        Dim response As New InserisciMandatiResponse
        Try
            response.Messaggio_Risposta = New MessaggioRisposta_Types
            response.Messaggio_Risposta.Intestazione = CreaIntestazione()

            If errore Is Nothing Then
                Dim successo As Successo_Types = New Successo_Types

                Dim risposta_InterrogazioneMandato = New Risposta_InterrogazioneMandato_Types
                risposta_InterrogazioneMandato.Codice = 0
                risposta_InterrogazioneMandato.Descrizione = "Inseriti " & contaInserimenti & " mandati"
                successo.Item = risposta_InterrogazioneMandato
                response.Messaggio_Risposta.Item = successo
            Else
                response.Messaggio_Risposta.Item = errore
            End If

        Catch ex As Exception
            Log.Error("Errore nell'invio della risposta al SIC. " & ex.Message)
            errore = New Eccezione_Types
            errore.Descrizione = "Errore generico. " & ex.Message
            errore.Codice = "9999"
            errore.Exception = ex.Message
        End Try

        Return response
    End Function

    Function GetOperatoreDaRequest(ByVal cf As String, ByVal codUff As String) As DllAmbiente.Operatore
        Log.Info("Richiesta Accesso " & cf & " con codice ufficio " & codUff)
        If String.IsNullOrEmpty(cf) Or String.IsNullOrEmpty(codUff) Then
            Return Nothing
        End If
        Dim op As New DllAmbiente.Operatore
        op.Leggi_Dati_CF_Ufficio(cf, codUff)
        If op.Codice = "" Then
            Log.Error("Impossibile trovare c.f. " & cf & " con codice ufficio " & codUff)
            Return Nothing
        End If

        If op.Stato <> "1" Then
            Log.Error("Operatore c.f. " & cf & " con codice ufficio " & codUff & " non abilitato alla procedura")
            Return Nothing
        End If

        Return op
    End Function

    Function CreaIntestazione() As Intestazione_Types
        Dim intestaz As New Intestazione_Types
        intestaz.Applicazione = "ProvvedimenteAmministrativi"
        intestaz.InfoMittDest = "ProvvedimenteAmministrativi"
        Return intestaz
    End Function

    Private Function verifica_CodiceObiettivoGestionale(ByVal operatore As DllAmbiente.Operatore, ByVal AnnoRif As String, ByVal CapitoloRif As String, ByVal cog As String) As Boolean
        Try
            Dim rispostaInterrogazioneCog As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneListaCogMessage(operatore, AnnoRif, CapitoloRif)
            For i As Integer = 0 To UBound(rispostaInterrogazioneCog, 1)
                If cog = DirectCast(rispostaInterrogazioneCog(i), ClientIntegrazioneSic.Intema.WS.Risposta.COG_Type).Codice Then
                    Return True
                End If
            Next
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return False
        End Try

        Return False
    End Function

    Private Function verifica_PianoDeiContiFinanziario(ByVal operatore As DllAmbiente.Operatore, ByVal AnnoRif As String, ByVal CapitoloRif As String, ByVal PdCF As String) As Boolean
        Try
            Dim rispostaInterrogazionePdCF As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazionePianoDeiContiFinanziario(operatore, AnnoRif, CapitoloRif)
            For i As Integer = 0 To UBound(rispostaInterrogazionePdCF, 1)
                If PdCF = DirectCast(rispostaInterrogazionePdCF(i), ClientIntegrazioneSic.Intema.WS.Risposta.PCF_Type).Codice Then
                    Return True
                End If
            Next
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return False
        End Try
        Return False
    End Function

    '<System.Web.Services.WebMethodAttribute(), _
    'System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:CreaDocumento", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Bare, Binding:="ProvvedimentiBinding")> _
    'Public Overridable Function CreaDocumento(<System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://regione.basilicata.it/provvedimentiamministrativi/types", ElementName:="Messaggio_Richiesta")> ByVal messaggio_Richiesta As MessaggioRichiesta_Types) As <System.Xml.Serialization.XmlElementAttribute("Messaggio_Risposta", [Namespace]:="http://regione.basilicata.it/provvedimentiamministrativi/types")> MessaggioRisposta_Types Implements IProvvedimentiService.CreaDocumento
    Function CreaDocumento(ByVal request As InserisciMandatiRequest) As InserisciMandatiResponse Implements ProvvedimentiPortType.CreaDocumento

        Log.Debug("Inizio-CreaDocumento")
        Dim response As New InserisciMandatiResponse
        response.Messaggio_Risposta = New MessaggioRisposta_Types
        response.Messaggio_Risposta.Intestazione = CreaIntestazione()


        Log.Info("Applicazione chiamante:" & request.Messaggio_Richiesta.Intestazione.Applicazione & " InfoMittDest chiamante: " & request.Messaggio_Richiesta.Intestazione.InfoMittDest)


        Dim numProvvisorio As String = ""
        Dim CodiceEsterno As String = ""


        'If Not ((request.Messaggio_Richiesta.Richiesta.Item.GetType)) Is GetType(CreazioneDocumento_Types) Then
        '    'Errore
        '    Dim ecc As New Eccezione_Types
        '    ecc.Codice = "9999"
        '    ecc.Descrizione = "Richiesto CreazioneDocumento_Types e non " & request.Messaggio_Richiesta.Richiesta.Item.GetType.ToString
        '    ecc.Exception = "impossibile analizzare la richiesta per il metodo Richiesto"
        '    response.Messaggio_Risposta.Item = ecc


        '    Return response

        'End If

        Try
            Dim eccez As Eccezione_Types = CreaEccezionePerTipoRichiesta(request.Messaggio_Richiesta.Richiesta.Item.GetType, GetType(CreazioneDocumento_Types))

            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Dim objRichiestaCrea As CreazioneDocumento_Types = request.Messaggio_Richiesta.Richiesta.Item

            'Trovare operatore da cf
            Log.Debug("CF : " & objRichiestaCrea.Cod_Fiscale & " UFF : " & objRichiestaCrea.Cod_Ufficio)
            Dim op As DllAmbiente.Operatore = GetOperatoreDaRequest(objRichiestaCrea.Cod_Fiscale, objRichiestaCrea.Cod_Ufficio)
            Dim dllRegistraOp As New DllDocumentale.svrDocumenti(op)
            eccez = CreaEccezioneOperatore(op)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            CodiceEsterno = objRichiestaCrea.DocumentoInfo.Cod_Esterno
            'Verifica Esistenza codice Esterno 
            Dim numDocProv As String = ""
            If Not String.IsNullOrEmpty(CodiceEsterno) Then
                numDocProv = VerificaEsistenzaCodiceEsternoEApplicazione(op, CodiceEsterno, request.Messaggio_Richiesta.Intestazione.Applicazione)
            End If
           
            If Not String.IsNullOrEmpty(numDocProv) Then
                'Documento già esistente.
                Log.Info("Attenzione Codice Esterno: " & CodiceEsterno & " già esistente con il numero provvisorio " & numDocProv)
                Dim succ As New Successo_Types
                Dim rispCrea As New Risposta_CreaProvvedimento_Types
                rispCrea.NumeroProvvisorio = numDocProv
                rispCrea.Codice = 1
                rispCrea.Descrizione = "Attenzione Codice Esterno: " & CodiceEsterno & "  esistente con il numero provvisorio " & numDocProv
                succ.Item = rispCrea
                response.Messaggio_Risposta.Item = succ
                Return response
            End If

            If String.IsNullOrEmpty(objRichiestaCrea.DocumentoInfo.Oggetto) Then
                eccez = CreaEccezioneCampoObbligatorio("Oggetto")
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Dim codDocumento As String = ""
            Dim vR As Object = Nothing

            eccez = validatePrioritàProvvedimento(op, objRichiestaCrea.DocumentoInfo.Urgente)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Dim contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo) = New Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)

            Dim schedaLeggeTrasparenzaToBeEnabled As Boolean = True
            If schedaLeggeTrasparenzaToBeEnabled Then
                ' valida i contratti e contestualmente invoca il SIC per popolarsi gli altri dati dei singoli contratti
                eccez = validateSchedaLeggeTrasparenzaEschedaContrFattInfo(op, objRichiestaCrea.DocumentoInfo.Legge_Trasparenza, contratti)
                If Not eccez Is Nothing Then
                    response.Messaggio_Risposta.Item = eccez
                    Return response
                End If
            End If

            Dim schedaTipologiaProvvedimentoToBeEnabled As Boolean = True
            If schedaTipologiaProvvedimentoToBeEnabled Then
                eccez = validateSchedaTipologiaProvvedimentoInfo(op, objRichiestaCrea.DocumentoInfo.Tipologia_Provvedimento, contratti)
                If Not eccez Is Nothing Then
                    response.Messaggio_Risposta.Item = eccez
                    Return response
                End If
            End If

            eccez = verifica_DatiContabili(op, objRichiestaCrea.DocumentoInfo.Dati_Contabili, contratti)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Select Case objRichiestaCrea.DocumentoInfo.Tipo_Atto
                Case 0
                    vR = Crea_Determina(op)
                    If vR(0) = 0 Then
                        codDocumento = vR(2)
                        numProvvisorio = vR(1)
                    End If
                Case 2
                    vR = Crea_Disposizione(op)
                    If vR(0) = 0 Then
                        codDocumento = vR(2)
                        numProvvisorio = vR(1)
                    End If
            End Select

            'imposta la priorità del provvedimento. se urgente, l'operatore deve essere abilitato all'inoltro di atti urgenti
            impostaPrioritàProvvedimento(op, codDocumento, objRichiestaCrea.DocumentoInfo.Urgente)

            'imposta, se presenti, le info riguardanti la legge sulla trasparenza degli atti nella pubblica amministrazione 
            If schedaLeggeTrasparenzaToBeEnabled Then
                impostaInfoSchedaLeggeTrasparenza(op, codDocumento, objRichiestaCrea.DocumentoInfo.Legge_Trasparenza)
            End If

            If schedaLeggeTrasparenzaToBeEnabled Then
                impostaInfoSchedaContrattiFatture(op, codDocumento, objRichiestaCrea.DocumentoInfo.Legge_Trasparenza, contratti)
            End If

            'imposta, se presenti, le info riguardanti la tipologia e i destinatari del provvedimento
            If schedaTipologiaProvvedimentoToBeEnabled Then
                impostaInfoSchedaTipologiaProvvedimento(op, codDocumento, objRichiestaCrea.DocumentoInfo.Tipologia_Provvedimento, contratti)
            End If

            Dim lstr_opContabile As String = creaStringaDatiContabili(objRichiestaCrea.DocumentoInfo.Dati_Contabili, objRichiestaCrea.DocumentoInfo.Tipo_Atto)
            Dim lstr_StringTempOpContabili As String = Trim(lstr_opContabile.Replace("0", "").Replace(";", ""))
            'elminino tutti gli 0 e i punti e vigorla cosi facendo mi rimane il valore delle operazioni selezionate
            'non potrò + fare il controllo se contiene 1 visto che per sistemazioni contabili potrà avere altri valori
            'If stringaOpConta.Contains("1") Then
            Dim OpContabili As Integer = 0
            If Not String.IsNullOrEmpty(lstr_StringTempOpContabili) Then
                OpContabili = 1
            Else
                OpContabili = 0
            End If

            Dim vrit As Object = RegistraDocumentoProv(op, codDocumento, objRichiestaCrea.DocumentoInfo.Oggetto, , , objRichiestaCrea.DocumentoInfo.Tipo_Pubblicazione, , OpContabili, lstr_opContabile, objRichiestaCrea.DocumentoInfo.Flag_privacy, request.Messaggio_Richiesta.Intestazione.Applicazione, objRichiestaCrea.DocumentoInfo.Codice_Cup, objRichiestaCrea.DocumentoInfo.Cod_Esterno, objRichiestaCrea.DocumentoInfo.Flag_Investimento_Pub)

            'gestione Corpo Atto
            If Not objRichiestaCrea.DocumentoInfo.Corpo_Atto Is Nothing Then
                gestioneCorpo(codDocumento, objRichiestaCrea.DocumentoInfo.Corpo_Atto, op, objRichiestaCrea.DocumentoInfo.Tipo_Atto)
            Else
                Log.Info("Doc id: " & codDocumento & " corpo dell'atto mancante")
            End If

            'gestione  allegato
            If Not objRichiestaCrea.DocumentoInfo.Lista_Allegati Is Nothing Then
                If Not objRichiestaCrea.DocumentoInfo.Lista_Allegati.Allegati Is Nothing Then
                    For Each allegato As Allegato_Types In objRichiestaCrea.DocumentoInfo.Lista_Allegati.Allegati
                        PreparaERegistraAllegato(codDocumento, allegato, op)
                    Next
                End If

                If Not objRichiestaCrea.DocumentoInfo.Lista_Allegati.Nota_Cartacea Is Nothing Then
                    For Each allegato As Nota_Cartacea_Types In objRichiestaCrea.DocumentoInfo.Lista_Allegati.Nota_Cartacea
                        PreparaERegistraNotaCartacea(codDocumento, allegato, op)
                    Next
                End If
            End If
            'inizio gestione dati contabili
            If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili Is Nothing Then

                'Gestione impegni
                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.Impegni Is Nothing Then
                    For Each impegno As Impegno_Types In objRichiestaCrea.DocumentoInfo.Dati_Contabili.Impegni
                        Dim impPA As New DllDocumentale.ItemImpegnoInfo
                        
                        impPA.Dli_Documento = codDocumento
                        impPA.DBi_Anno = Now.Year
                        impPA.Dli_Esercizio = impegno.Bilancio
                        impPA.Dli_Cap = impegno.Capitolo

                        impPA.Piano_Dei_Conti_Finanziari = impegno.Piano_dei_Conti_Finanziario
                        impPA.Codice_Obbiettivo_Gestionale = impegno.Codice_obiettivo_Gestionale

                        impPA.Dli_Costo = impegno.Importo
                        impPA.Dli_NPreImpegno = impegno.NumeroPreImpegno

                        'Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                        'Dim hashTokenCallSic_Imp As String = GenerateHashTokenCallSic()
                        
                        'impPA.HashTokenCallSic = hashTokenCallSic
                        'impPA.HashTokenCallSic_Imp = hashTokenCallSic_Imp

                        If impPA.Dli_NPreImpegno <> "" Then
                            impPA.Di_PreImpDaPrenotazione = 1
                        End If
                        
                        impPA.Dli_UPB = impegno.UPB
                        impPA.Dli_MissioneProgramma = impegno.MissioneProgramma


                        impPA.Dli_Operatore = op.Codice
                        impPA.Di_Stato = StatoOpContabile.InAttesaDiConferma

                        'Beneficiario dell'impegno
                        If Not impegno.Beneficiario Is Nothing Then
                            Dim beneficiarioImpegno As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo
                            associaBeneficiariImpegno(impPA, impegno.Beneficiario, contratti, dllRegistraOp, codDocumento)
                        End If

                        Dim listaLiq As New List(Of DllDocumentale.ItemLiquidazioneInfo)
                        'Liquidazioni contestuali
                        If Not impegno.Liquidazioni Is Nothing Then

                            For Each liquidazioni As Liquidazione_Types In impegno.Liquidazioni
                                Dim liqPA As New DllDocumentale.ItemLiquidazioneInfo
                                'liqPA.HashTokenCallSic = GenerateHashTokenCallSic()

                                liqPA.Dli_Documento = codDocumento
                                liqPA.Dli_NPreImpegno = impPA.Dli_NPreImpegno

                                liqPA.Dli_Esercizio = liquidazioni.Esercizio
                                liqPA.Dli_Costo = liquidazioni.Importo
                                liqPA.Dli_Num_assunzione = liquidazioni.NumeroAssunzione
                                liqPA.Dli_NumImpegno = liquidazioni.NumeroImpegno
                                liqPA.Dli_UPB = liquidazioni.UPB
                                liqPA.Dli_MissioneProgramma = liquidazioni.MissioneProgramma
                                liqPA.Dli_PianoDeiContiFinanziari = liquidazioni.PianoContiFinanziario

                                liqPA.Dli_Cap = liquidazioni.Capitolo
                                If liquidazioni.DataAssunzioneSpecified Then
                                    liqPA.Dli_Data_Assunzione = liquidazioni.DataAssunzione
                                Else
                                    liqPA.Dli_Data_Assunzione = Now
                                End If
                                liqPA.Dli_TipoAssunzione = liquidazioni.TipoAssunzione
                                liqPA.Dli_Operatore = op.Codice
                                liqPA.Di_Stato = StatoOpContabile.InAttesaDiConferma

                                'aggiungo i beneficiari alla liquidazione
                                associaFatturaEBeneficiariLiquidazione(liqPA, liquidazioni.Lista_Beneficiari, contratti, dllRegistraOp, codDocumento)

                                listaLiq.Add(liqPA)
                                liqPA = Nothing
                            Next
                        End If
                        'REgistra
                        dllRegistraOp.FO_Insert_impegno_LiqContestuali_Da_WS(op, impPA, listaLiq)




                        impPA = Nothing
                        listaLiq = Nothing
                    Next
                End If
                'Gestione impegni perenti
                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.ImpegniSuPerenti Is Nothing Then
                    For Each impegnoPerente As Impegno_Su_Perente_Types In objRichiestaCrea.DocumentoInfo.Dati_Contabili.ImpegniSuPerenti
                        'Gestione Liquidazione contestuale sul perente
                        Dim impPerentePA As New DllDocumentale.ItemImpegnoInfo
                        'impPerentePA.HashTokenCallSic = GenerateHashTokenCallSic()
                        'impPerentePA.HashTokenCallSic_Imp = GenerateHashTokenCallSic()

                        impPerentePA.Dli_Documento = codDocumento
                        impPerentePA.DBi_Anno = impegnoPerente.Anno
                        impPerentePA.Dli_Esercizio = impegnoPerente.Bilancio
                        impPerentePA.Dli_Cap = impegnoPerente.Capitolo
                        impPerentePA.Dli_Costo = impegnoPerente.Importo

                        impPerentePA.Dli_UPB = impegnoPerente.UPB
                        impPerentePA.Dli_MissioneProgramma = impegnoPerente.MissioneProgramma
                        impPerentePA.Dli_Operatore = op.Codice
                        impPerentePA.Di_Stato = StatoOpContabile.InAttesaDiConferma
                        impPerentePA.NDocPrecedente = impegnoPerente.NumeroImpegnoPerente
                        impPerentePA.Piano_Dei_Conti_Finanziari = impegnoPerente.Piano_dei_Conti_Finanziario
                        impPerentePA.Codice_Obbiettivo_Gestionale = impegnoPerente.Codice_obiettivo_Gestionale

                        'Beneficiario dell'impegno
                        If Not impegnoPerente.Beneficiario Is Nothing Then
                            Dim beneficiarioImpegno As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo
                            associaBeneficiariImpegno(impPerentePA, impegnoPerente.Beneficiario, contratti, dllRegistraOp, codDocumento)
                        End If

                        Dim listaLiqPerPerenti As New List(Of DllDocumentale.ItemLiquidazioneInfo)

                        If Not impegnoPerente.Liquidazioni Is Nothing Then
                            Dim liquidazioni As Liquidazione_Types = impegnoPerente.Liquidazioni

                            Dim liqPerentiPA As New DllDocumentale.ItemLiquidazioneInfo
                            'liqPerentiPA.HashTokenCallSic = GenerateHashTokenCallSic()
                            liqPerentiPA.Dli_Documento = codDocumento
                            liqPerentiPA.Dli_NPreImpegno = impPerentePA.Dli_NPreImpegno
                            liqPerentiPA.Dli_Esercizio = liquidazioni.Esercizio
                            liqPerentiPA.Dli_Costo = liquidazioni.Importo
                            If liquidazioni.DataAssunzioneSpecified Then
                                liqPerentiPA.Dli_Data_Assunzione = liquidazioni.DataAssunzione
                            Else
                                liqPerentiPA.Dli_Data_Assunzione = Now
                            End If
                            liqPerentiPA.Dli_Num_assunzione = liquidazioni.NumeroAssunzione
                            liqPerentiPA.Dli_NumImpegno = liquidazioni.NumeroImpegno
                            liqPerentiPA.Dli_UPB = liquidazioni.UPB
                            liqPerentiPA.Dli_MissioneProgramma = liquidazioni.MissioneProgramma
                            liqPerentiPA.Dli_PianoDeiContiFinanziari = liquidazioni.PianoContiFinanziario
                            liqPerentiPA.Dli_Cap = liquidazioni.Capitolo
                            liqPerentiPA.Dli_TipoAssunzione = liquidazioni.TipoAssunzione
                            liqPerentiPA.Dli_Operatore = op.Codice
                            liqPerentiPA.Di_Stato = StatoOpContabile.InAttesaDiConferma

                            'aggiungo i beneficiari alla liquidazione
                            associaFatturaEBeneficiariLiquidazione(liqPerentiPA, liquidazioni.Lista_Beneficiari, contratti, dllRegistraOp, codDocumento)

                            listaLiqPerPerenti.Add(liqPerentiPA)
                        End If

                        'REgistra
                        dllRegistraOp.FO_Insert_impegno_LiqContestuali_Da_WS(op, impPerentePA, listaLiqPerPerenti)


                        impPerentePA = Nothing
                        listaLiqPerPerenti = Nothing
                    Next
                End If

                'Gestione Liquidazione
                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.Liquidazioni Is Nothing Then
                    Dim listaLiq As New List(Of DllDocumentale.ItemLiquidazioneInfo)
                    For Each liquidazioneWS As Liquidazione_Types In objRichiestaCrea.DocumentoInfo.Dati_Contabili.Liquidazioni

                        Dim liq As New DllDocumentale.ItemLiquidazioneInfo
                        'liq.HashTokenCallSic = GenerateHashTokenCallSic()
                        liq.Dli_Documento = codDocumento
                        liq.Dli_NPreImpegno = ""
                        liq.Dli_Esercizio = liquidazioneWS.Esercizio
                        liq.Dli_Costo = liquidazioneWS.Importo
                        If liquidazioneWS.DataAssunzioneSpecified Then
                            liq.Dli_Data_Assunzione = liquidazioneWS.DataAssunzione
                        Else
                            liq.Dli_Data_Assunzione = Now
                        End If
                        liq.Dli_Num_assunzione = liquidazioneWS.NumeroAssunzione
                        liq.Dli_NumImpegno = liquidazioneWS.NumeroImpegno
                        liq.Dli_UPB = liquidazioneWS.UPB
                        liq.Dli_MissioneProgramma = liquidazioneWS.MissioneProgramma
                        liq.Dli_PianoDeiContiFinanziari = liquidazioneWS.PianoContiFinanziario
                        liq.Dli_Cap = liquidazioneWS.Capitolo
                        liq.Dli_TipoAssunzione = liquidazioneWS.TipoAssunzione
                        liq.Dli_Operatore = op.Codice
                        liq.Di_Stato = StatoOpContabile.InAttesaDiConferma



                        'aggiungo i beneficiari alla liquidazione
                        associaFatturaEBeneficiariLiquidazione(liq, liquidazioneWS.Lista_Beneficiari, contratti, dllRegistraOp, codDocumento)

                        dllRegistraOp.FO_Insert_Liquidazione_Fatture_E_Beneficiari(op, liq)


                        If Not liquidazioneWS.Item Is Nothing Then
                            If liquidazioneWS.Item.GetType Is GetType(Economia_Types) Then
                                'REgistra economia contestuale
                                Dim ecoWs As Economia_Types = liquidazioneWS.Item
                                Dim itemEconomia As New DllDocumentale.ItemRiduzioneInfo

                                With itemEconomia
                                    '.HashTokenCallSic = GenerateHashTokenCallSic()
                                    .Dli_Documento = codDocumento
                                    .Dli_DataRegistrazione = Now
                                    .Dli_Operatore = op.Codice
                                    .Dli_Esercizio = ecoWs.Bilancio
                                    .Dli_UPB = ecoWs.UPB
                                    .Dli_MissioneProgramma = ecoWs.MissioneProgramma
                                    .Dli_Cap = ecoWs.Capitolo
                                    .Dli_Costo = ecoWs.Importo
                                    .Dli_NumImpegno = ecoWs.NumeroDocumentoContabile
                                    .DBi_Anno = ecoWs.Bilancio
                                    .Dli_NPreImpegno = ""
                                    .Div_TipoAssunzione = ecoWs.TipoAssunzione
                                    .Div_Data_Assunzione = IIf(ecoWs.DataAssunzioneSpecified, ecoWs.DataAssunzione, Now)
                                    .Div_Num_assunzione = ecoWs.NumeroAssunzione
                                    'Lu Rid
                                    .Div_IsEconomia = 1
                                    .Di_Stato = StatoOpContabile.InAttesaDiConferma
                                End With

                                dllRegistraOp.FO_Insert_Impegno_Var(itemEconomia)

                            Else

                                'REgistra riduzione contestuale
                                Dim ridWs As Riduzione_Types = liquidazioneWS.Item
                                Dim ridImpPA As New DllDocumentale.ItemRiduzioneInfo

                                With ridImpPA
                                    '.HashTokenCallSic = GenerateHashTokenCallSic()
                                    .Dli_Documento = codDocumento
                                    .Dli_DataRegistrazione = Now
                                    .Dli_Operatore = op.Codice
                                    .Dli_Esercizio = ridWs.Bilancio
                                    .Dli_UPB = ridWs.UPB
                                    .Dli_MissioneProgramma = ridWs.MissioneProgramma
                                    .Dli_Cap = ridWs.Capitolo
                                    .Dli_Costo = ridWs.Importo
                                    .Dli_NumImpegno = ridWs.NumeroDocumentoContabile
                                    .DBi_Anno = ridWs.Bilancio
                                    .Dli_NPreImpegno = ""
                                    .Div_TipoAssunzione = ridWs.TipoAssunzione
                                    .Div_Data_Assunzione = IIf(ridWs.DataAssunzioneSpecified, ridWs.DataAssunzione, Now)
                                    .Div_Num_assunzione = ridWs.NumeroAssunzione
                                    'Lu Rid
                                    .Div_IsEconomia = 0
                                    .Di_Stato = StatoOpContabile.InAttesaDiConferma
                                End With

                                dllRegistraOp.FO_Insert_Impegno_Var(ridImpPA)
                            End If
                        End If
                    Next
                    'registra ListaLiq
                    listaLiq = Nothing
                End If

                'Gestione Economia
                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.Economie Is Nothing Then
                    For Each economie As Economia_Types In objRichiestaCrea.DocumentoInfo.Dati_Contabili.Economie
                        Dim itemEconomia As New DllDocumentale.ItemRiduzioneInfo

                        With itemEconomia
                            '.HashTokenCallSic = GenerateHashTokenCallSic()
                            .Dli_Documento = codDocumento
                            .Dli_DataRegistrazione = Now
                            .Dli_Operatore = op.Codice
                            .Dli_Esercizio = economie.Bilancio
                            .Dli_UPB = economie.UPB
                            .Dli_MissioneProgramma = economie.MissioneProgramma
                            .Dli_Cap = economie.Capitolo
                            .Dli_Costo = economie.Importo
                            .Dli_NumImpegno = economie.NumeroDocumentoContabile
                            .DBi_Anno = economie.Bilancio
                            .Dli_NPreImpegno = ""
                            .Div_TipoAssunzione = economie.TipoAssunzione
                            .Div_Data_Assunzione = IIf(economie.DataAssunzioneSpecified, economie.DataAssunzione, Now)
                            .Div_Num_assunzione = economie.NumeroAssunzione
                            'Lu Rid
                            .Div_IsEconomia = 1
                            .Di_Stato = StatoOpContabile.InAttesaDiConferma
                        End With

                        dllRegistraOp.FO_Insert_Impegno_Var(itemEconomia)
                    Next
                End If

                'Gestione Riduzioni 
                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.Riduzioni Is Nothing Then
                    For Each riduzioni As Riduzione_Types In objRichiestaCrea.DocumentoInfo.Dati_Contabili.Riduzioni

                        Select Case riduzioni.TipoOperazioneContabile
                            Case "IMP" 'Riduzione impegno
                                Dim ridImpPA As New DllDocumentale.ItemRiduzioneInfo

                                With ridImpPA
                                    '.HashTokenCallSic = GenerateHashTokenCallSic()
                                    .Dli_Documento = codDocumento
                                    .Dli_DataRegistrazione = Now
                                    .Dli_Operatore = op.Codice
                                    .Dli_Esercizio = riduzioni.Bilancio
                                    .Dli_UPB = riduzioni.UPB
                                    .Dli_MissioneProgramma = riduzioni.MissioneProgramma
                                    .Dli_Cap = riduzioni.Capitolo
                                    .Dli_Costo = riduzioni.Importo
                                    .Dli_NumImpegno = riduzioni.NumeroDocumentoContabile
                                    .DBi_Anno = riduzioni.Bilancio
                                    .Dli_NPreImpegno = ""
                                    .Div_TipoAssunzione = riduzioni.TipoAssunzione
                                    .Div_Data_Assunzione = IIf(riduzioni.DataAssunzioneSpecified, riduzioni.DataAssunzione, Now)
                                    .Div_Num_assunzione = riduzioni.NumeroAssunzione
                                    'Lu Rid
                                    .Div_IsEconomia = 0
                                    .Di_Stato = StatoOpContabile.InAttesaDiConferma
                                End With

                                dllRegistraOp.FO_Insert_Impegno_Var(ridImpPA)

                        End Select
                    Next
                End If

                If Not objRichiestaCrea.DocumentoInfo.Dati_Contabili.Accertamento Is Nothing Then
                    Dim acc As Accertamento_Types = objRichiestaCrea.DocumentoInfo.Dati_Contabili.Accertamento
                    Dim accPA As New DllDocumentale.ItemAssunzioneContabileInfo

                    With accPA
                        .Da_Documento = codDocumento
                        .Da_DataRegistrazione = Now
                        .Da_Operatore = op.Codice
                        .Da_Costo = acc.Importo
                        .Da_Stato = StatoOpContabile.Registrato

                    End With
                    dllRegistraOp.FO_Insert_Assunzione(accPA)


                End If

            End If

            ''fine dati contabili
            Dim lstrMessaggio As String = "Provvedimento Creato con successo"

            If Not String.IsNullOrEmpty(numProvvisorio) Then
                Dim succ As New Successo_Types
                Dim rispCrea As New Risposta_CreaProvvedimento_Types
                rispCrea.Codice = 0
                rispCrea.Descrizione = lstrMessaggio
                rispCrea.NumeroProvvisorio = numProvvisorio
                succ.Item = rispCrea
                response.Messaggio_Risposta.Item = succ

            Else

                Dim ecc As New Eccezione_Types
                ecc.Codice = "9999"
                ecc.Descrizione = lstrMessaggio
                ecc.Exception = lstrMessaggio
                response.Messaggio_Risposta.Item = ecc
                Return response

            End If

        Catch ex As Exception
            Log.Error("Error-CreaDocumento: " & ex.Message)
            If String.IsNullOrEmpty(numProvvisorio) Then
                response.Messaggio_Risposta.Item = CreaEccezioneGenerica(ex.Message, ex.Message)
            Else
                response.Messaggio_Risposta.Item = CreaEccezioneGenerica(ex.Message & " Numero provvisorio creato " & numProvvisorio, numProvvisorio, "0006")
            End If

            Return response

        End Try

        Log.Debug("Fine-CreaDocumento")
        Return response

    End Function
    Function VerificaEsistenzaCodiceEsternoEApplicazione(ByVal op As DllAmbiente.Operatore, ByVal codiceEsterno As String, ByVal cod_app As String) As String
        Dim numProv As String = ""
        Dim doc_item As DllDocumentale.Model.DocumentoInfo = Nothing
        doc_item = GetDocumentoFrom_NumProv_NumDef_CodEster(op, codiceEsterno, 2, cod_app)
        If Not doc_item Is Nothing Then
            numProv = doc_item.Doc_numeroProvvisorio
        End If
        Return numProv
    End Function
    ' tipoCodice 
    '0 prov
    '1 def
    '2 esterno
    Function GetDocumentoFrom_NumProv_NumDef_CodEster(ByVal op As DllAmbiente.Operatore, ByVal codice As String, ByVal tipoCodice As String, Optional ByVal cod_app As String = "") As DllDocumentale.Model.DocumentoInfo
        Dim dllDoc As New DllDocumentale.svrDocumenti(op)
        Dim docItem As New DllDocumentale.Model.DocumentoInfo
        Dim docId As String = dllDoc.GetIdDocumentoFrom_NumProv_NumDef_CodEster(codice, tipoCodice, cod_app)
        If Not String.IsNullOrEmpty(docId) Then
            docItem = Leggi_Documento_Object(op, docId)
        End If
        Return docItem
    End Function

    Public Function Leggi_Documento_Object(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, Optional ByRef xmlDati As String = Nothing) As DllDocumentale.Model.DocumentoInfo

        Dim docItem As DllDocumentale.Model.DocumentoInfo = Nothing

        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Leggi_Documento_Object ' Dll.Dic_FO.cfo_
        Dim dimvc_Funzione As Integer = DllDocumentale.Dic_FODocumentale.dimvc_Leggi_Documento
        Dim vFunzione(dimvc_Funzione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(3) As Object

        Try
            vRit(0) = 0
            vRit(1) = ""
            vRit(2) = ""
            vRit(3) = ""
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(op)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_xmlDatiDocumento) = xmlDati

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = op.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            docItem = oDllDocumenti.Elabora(cFunzione, vParam)

            vRit(2) = docItem.Doc_Cod_Uff_Prop
            vRit(3) = docItem.Doc_Descrizione_ufficio

            'comm per eliminazione xml
            'With docItem
            '    .Doc_Oggetto = .Doc_Oggetto.Replace("&amp;", Chr(38))
            '    .Doc_Oggetto = .Doc_Oggetto.Replace("&aps;", Chr(39))
            '    .Doc_Oggetto = .Doc_Oggetto.Replace("&gt;", Chr(62))
            '    .Doc_Oggetto = .Doc_Oggetto.Replace("&lt;", Chr(60))
            '    .Doc_Oggetto = .Doc_Oggetto.Replace("&quot;", Chr(34))

            'End With

            oDllDocumenti = Nothing
        Catch ex As Exception
            'Log.error
            Log.Error("Leggi Documento object: " & ex.Message)
            docItem = Nothing
        Finally
            Leggi_Documento_Object = docItem
        End Try
    End Function

    Public Function Crea_Determina(ByVal op As DllAmbiente.Operatore) As Object
        Dim oDll As New DllDocumentale.svrDocumenti(op)
        Dim oDllDetermine As DllDocumentale.svrDetermineBase = Nothing
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_Determina) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = op

            ' oDllDetermine = New DllDocumentale.svrDetermine(op)
            oDllDetermine = DllDocumentale.AbstractSvrDocumenti.getSvrDetermine(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Determina.c_cod_ufficio_proponente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Determina.c_utente_creazione) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Determina.c_data_creazione) = Format(Now, "dd/MM/yyyy")

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDetermine.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Crea_Determina, vParam)

            oDllDetermine = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Crea_Determina = vRit
        End Try

    End Function

    Public Function Crea_Disposizione(ByVal op As DllAmbiente.Operatore) As Object
        Dim oDllDisposizione As DllDocumentale.svrDisposizioniBase = Nothing
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_Disposizione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = op

            '  oDllDisposizione = New DllDocumentale.svrDisposizioni(op)
            oDllDisposizione = DllDocumentale.AbstractSvrDocumenti.getSvrDisposizioni(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_cod_ufficio_proponente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_utente_creazione) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_data_creazione) = Format(Now, "dd/MM/yyyy")

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDisposizione.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Crea_Disposizione, vParam)

            oDllDisposizione = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Crea_Disposizione = vRit
        End Try

    End Function
    Function RegistraDocumentoProv(ByVal operatore As DllAmbiente.Operatore, ByVal codDocumento As String, Optional ByRef DocOggetto As String = Nothing, Optional ByRef DocTesto As String = Nothing, Optional ByRef datiXml As String = Nothing, Optional ByRef pubIntegrale As Integer = 0, Optional ByVal fileTestoDetermina As Byte() = Nothing, Optional ByVal isContabile As Integer = 0, Optional ByVal tipoOpContabili As String = "", Optional ByVal privacy As String = "0", Optional ByVal codice_Applicazione As String = "", Optional ByVal codice_Cup As String = "", Optional ByVal cod_esterno As String = "", Optional ByVal flag_Invest_Pubb As String = "") As Object

        Dim oDll As New DllDocumentale.svrDocumenti(operatore) '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Documento ' Dll.Dic_FO.cfo_
        Dim dimvc_Funzione As Integer = DllDocumentale.Dic_FODocumentale.dimvc_Registra_Documento
        Dim vFunzione(dimvc_Funzione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object


        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_idDocumento) = codDocumento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_doc_Oggetto) = DocOggetto
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_testo) = DocTesto
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_utente) = operatore.Codice
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_xmlDatiDocumento) = datiXml
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_pub_integrale) = pubIntegrale
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_isContabile) = isContabile
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_tipoOpContabili) = tipoOpContabili
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_flagPrivacy) = privacy
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_Cod_Applicazione) = codice_Applicazione
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_Cod_Cup) = codice_Cup
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_cod_Investimento_Pub) = flag_Invest_Pubb
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_cod_doc_Esterno) = cod_esterno

        vClient(0) = ""
        vClient(1) = ""
        vClient(2) = operatore.Codice
        vClient(3) = ""

        vParam(0) = vClient
        vParam(1) = vFunzione

        vr = oDll.Elabora(cFunzione, vParam)

        vRit(0) = vr(0)
        vRit(1) = vr(1)
        Return vRit
    End Function
    Function creaStringaDatiContabili(ByVal datiContabili As Dati_Contabili_Types, ByVal tipoDocumento As String) As String
        'impegno;perente;liquidazione;Accertamento;Riduzione
        Dim StringaDatiContabili As String = "0;0;0;0;0;0;0;0;"
        If datiContabili Is Nothing Then
            Return StringaDatiContabili
        Else
            Select Case tipoDocumento
                Case 0
                    StringaDatiContabili = creaStringaOpContabili(datiContabili)
                Case 2
                    StringaDatiContabili = creaStringaOpContabiliDisposizione(datiContabili)
            End Select
            Return StringaDatiContabili
        End If
    End Function


    Function creaStringaOpContabili(ByVal datiContabili As Dati_Contabili_Types) As String
        Dim result As String = ""

        Dim StringaDatiCotabili As String = "0;0;"
        If datiContabili Is Nothing Then

        End If


        If Not datiContabili.Impegni Is Nothing AndAlso datiContabili.Impegni.Length > 0 Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not datiContabili.ImpegniSuPerenti Is Nothing AndAlso datiContabili.ImpegniSuPerenti.Length > 0 Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If



        If Not datiContabili.Liquidazioni Is Nothing AndAlso datiContabili.Liquidazioni.Length > 0 Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If
        Dim flagPresenzaRidOEconomiaImpegno As Boolean = False
        Dim flagPresenzaRidPreImpDaDelibera As Boolean = False
        Dim flagPresenzaRidLiq As Boolean = False
        If Not datiContabili.Riduzioni Is Nothing AndAlso datiContabili.Riduzioni.Length > 0 Then
            flagPresenzaRidOEconomiaImpegno = True
        End If

        If Not datiContabili.Riduzioni Is Nothing AndAlso datiContabili.Riduzioni.Length > 0 Then

            For Each itemRid As Riduzione_Types In datiContabili.Riduzioni
                Select Case itemRid.TipoOperazioneContabile
                    Case "IMP"
                        flagPresenzaRidOEconomiaImpegno = True
                    Case "PRE-IMP"
                        flagPresenzaRidPreImpDaDelibera = True
                    Case "LIQ"
                        flagPresenzaRidLiq = True
                End Select
            Next
        End If

        'Accert
        result = result & "0;"

        If flagPresenzaRidOEconomiaImpegno Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        'riduzione pre imp
        'result = result & "0;"
        If flagPresenzaRidPreImpDaDelibera Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        'riduzione liq
        ' result = result & "0;"
        If flagPresenzaRidLiq Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not datiContabili.RichiestaRettificaRagioneria Is Nothing Then
            Dim codRic As String = IIf(String.IsNullOrEmpty(datiContabili.RichiestaRettificaRagioneria.CodiceRichiesta), "0", datiContabili.RichiestaRettificaRagioneria.CodiceRichiesta)
            result = result & codRic & ";"
        Else
            result = result & "0;"
        End If

        'campo in +
        result = result & "0;"



        Return result
    End Function

    Function creaStringaOpContabiliDisposizione(ByVal datiContabili As Dati_Contabili_Types) As String
        Dim result As String = ""
        'chkImpegno.Checked 
        result = result & "0;"

        'chkImpegnoSuPerenti.Checked 
        result = result & "0;"

        'chkLiquidazione.Checked
        result = result & "1;"
        'accer

        result = result & "0;"


        'chkRiduzione.Checked
        result = result & "0;"

        'riduzione pre imp
        result = result & "0;"
        'riduzione liq
        result = result & "0;"

        'campo in +
        result = result & "0;0;"

        Return result
    End Function

    Sub gestioneCorpo(ByVal codDocumento As String, ByVal corpo_atto As Corpo_Atto_Types, ByVal op As DllAmbiente.Operatore, ByVal tipoAtto As String)
        If Not corpo_atto Is Nothing Then
            If Not corpo_atto.Item Is Nothing Then
                Select Case corpo_atto.Item.GetType.ToString
                    Case GetType(Testo_Atto_Types).ToString
                        Dim memStream As MemoryStream = UnisciPremessaEDispositivo(CType(corpo_atto.Item, Testo_Atto_Types).PremessaXML.Item, CType(corpo_atto.Item, Testo_Atto_Types).DispositivoXML.Item, op, tipoAtto)
                        Dim arrayTras As Byte() = memStream.ToArray
                        memStream.Close()
                        memStream = Nothing

                        '' salvo template
                        Dim vR = Registra_Allegato(op, corpo_atto.Item, "PREMESSA_DISPOSITIVO", "pdf", codDocumento, 22, 1, , , , False, False)

                        Dim tipologiaDocumento As Integer
                        Select Case tipoAtto
                            Case 0 'Si tratta di una determina
                                tipologiaDocumento = 1
                            Case 1 'Si tratta di una delibera
                                tipologiaDocumento = 5
                            Case 2 'Si tratta di una disposizione
                                tipologiaDocumento = 8
                            Case Else

                        End Select

                        Dim versioneAllegato As String = 1

                        vR = Elenco_Allegati(op, codDocumento, tipologiaDocumento)

                        If vR(0) = 0 Then
                            versioneAllegato = CInt(vR(4)) + 1
                            vR = Nothing
                            'quando viene uploadato un nuovo documento word, deve invalidare tutte le firme precedenti, cancellando l'elenco certificati di firme
                            Cancella_Allegato_Fisicamente(op, , "16", codDocumento)
                            'modifico tutti i ruoli specifici
                            Modifica_Compiti(op, codDocumento)

                            vR = Registra_Allegato(op, corpo_atto.Item, "CORPO_PROVVEDIMENTO" & "_Vers. " & versioneAllegato, "pdf", codDocumento, tipologiaDocumento, versioneAllegato)
                        Else
                            vR = Registra_Allegato(op, corpo_atto.Item, "CORPO_PROVVEDIMENTO" & "_Vers. 1", "pdf", codDocumento, tipologiaDocumento)
                        End If

                        'Registro il corpo come file principale
                        If vR(0) = 0 Then
                            Dim idUltimoDoc As String = vR(1)
                            vR = Nothing

                            memStream = creaPDFunico(corpo_atto.Item, op, tipoAtto)
                            arrayTras = Nothing
                            arrayTras = memStream.ToArray
                            memStream.Close()
                            '' salvo template
                            vR = Registra_Allegato(op, arrayTras, "Documento_Principale", "pdf", codDocumento, 21, 1, , , , False, False)

                            If vR(0) <> 0 Then
                                Cancella_Allegato_Fisicamente(op, idUltimoDoc)
                            End If
                        End If

                        If vR(0) <> 0 Then
                            Dim errorMsg As String = vR(1)
                            Log.Error("Impossibile salvare il documento. L'operazione non è andata a buon fine. " + IIf(Not errorMsg Is Nothing, errorMsg + ".", "") + "Riprovare il caricamento.")
                        End If
                    Case Else
                        'Modello unico
                        'Registro l'allegato con tipo XX
                        'Creo il modulo concatenando il documento

                        Dim vR As Object = Registra_Allegato(op, corpo_atto.Item, "PREMESSA_DISPOSITIVO", "pdf", codDocumento, 22, 1, , , , False, False)

                        Dim tipologiaDocumento As Integer
                        Select Case tipoAtto
                            Case 0 'Si tratta di una determina
                                tipologiaDocumento = 1
                            Case 1 'Si tratta di una delibera
                                tipologiaDocumento = 5
                            Case 2 'Si tratta di una disposizione
                                tipologiaDocumento = 8
                            Case Else

                        End Select

                        Dim versioneAllegato As String = 1

                        vR = Elenco_Allegati(op, codDocumento, tipologiaDocumento)

                        If vR(0) = 0 Then
                            versioneAllegato = CInt(vR(4)) + 1
                            vR = Nothing
                            'quando viene uploadato un nuovo documento word, deve invalidare tutte le firme precedenti, cancellando l'elenco certificati di firme
                            Cancella_Allegato_Fisicamente(op, , "16", codDocumento)
                            'modifico tutti i ruoli specifici
                            Modifica_Compiti(op, codDocumento)

                            vR = Registra_Allegato(op, corpo_atto.Item, "CORPO_PROVVEDIMENTO" & "_Vers. " & versioneAllegato, "pdf", codDocumento, tipologiaDocumento, versioneAllegato)
                        Else
                            vR = Registra_Allegato(op, corpo_atto.Item, "CORPO_PROVVEDIMENTO" & "_Vers. 1", "pdf", codDocumento, tipologiaDocumento)
                        End If

                        'Registro il corpo come file principale
                        If vR(0) = 0 Then
                            Dim idUltimoDoc As String = vR(1)
                            vR = Nothing

                            Dim memStream As MemoryStream = creaPDFunico(corpo_atto.Item, op, tipoAtto)
                            Dim arrayTras As Byte() = memStream.ToArray
                            memStream.Close()

                            '' salvo template
                            vR = Registra_Allegato(op, arrayTras, "Documento_Principale", "pdf", codDocumento, 21, 1, , , , False, False)

                            If vR(0) <> 0 Then
                                Cancella_Allegato_Fisicamente(op, idUltimoDoc)
                            End If
                        End If

                        If vR(0) <> 0 Then
                            Dim errorMsg As String = vR(1)
                            Log.Error("Impossibile salvare il documento. L'operazione non è andata a buon fine. " + IIf(Not errorMsg Is Nothing, errorMsg + ".", "") + "Riprovare il caricamento.")
                        End If

                End Select
            Else
                Log.Error("Doc id: " & codDocumento & ". Corpo atto non specificato")
            End If
        Else
            Log.Error("Doc id: " & codDocumento & ". Corpo atto non specificato")
        End If
    End Sub


    Public Function Modifica_Compiti(ByVal oOperatore As DllAmbiente.Operatore, ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Update_compiti_Specifici_to_Generici
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Compito) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Compito.c_idDocumento) = codDocumento

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            oDllDocumenti = Nothing

        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Modifica_Compiti = vRit
        End Try


    End Function

    Public Function Elenco_Allegati(ByVal oOperatore As DllAmbiente.Operatore, ByVal codDocumento As String, Optional ByRef tipoAllegato As Integer = -1, Optional ByVal allDocumento As Integer = 0, Optional ByVal daStampare As String = Nothing) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Allegati
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Allegati) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_tipoAllegati) = tipoAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_allDocumento) = allDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_daStampare) = daStampare


            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)
            vr(3) = oDllDocumenti.objDocumento.Doc_Cod_Uff_Prop
            oDllDocumenti = Nothing

        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
            vr = vRit
        Finally

            Elenco_Allegati = vr
        End Try


    End Function

    Protected Sub PreparaERegistraAllegato(ByVal codDocumento As String, ByVal allegato As Allegato_Types, ByVal op As DllAmbiente.Operatore)

        Dim nomeAllegato As String

        Dim estensione As String = ""


        nomeAllegato = allegato.NomeFile
        Dim bFile() As Byte
        bFile = allegato.File_Allegato

        If InStr(nomeAllegato, ".", CompareMethod.Text) > 0 Then
            estensione = Trim(Split("." & nomeAllegato, ".")(UBound(Split("." & nomeAllegato, "."))))
        End If


        Dim tipoAllegatoDb As Integer = TipoAllegatiDb.AllegatoGeneric

        If Not allegato.Flag_Solo_ufficio Is Nothing AndAlso allegato.Flag_Solo_ufficio = "1" Then
            tipoAllegatoDb = TipoAllegatiDb.AllegatoUfficio
        End If

        If allegato.Flag_is_Por = "1" Then
            tipoAllegatoDb = TipoAllegatiDb.AllegatoPor
        End If

        Registra_Allegato(op, bFile, nomeAllegato, estensione, codDocumento, tipoAllegatoDb)



    End Sub

    Protected Sub PreparaERegistraNotaCartacea(ByVal codDocumento As String, ByVal note As Nota_Cartacea_Types, ByVal op As DllAmbiente.Operatore)

        Dim estensione As String = ""

        Dim informazioni_Cartacee As New StringBuilder
        informazioni_Cartacee.Append("<html><body>")
        informazioni_Cartacee.Append("<p>Nome Allegato: " & note.NomeAllegato & "</p>")
        informazioni_Cartacee.Append("<p>Rintracciabilità: " & note.Rintracciabilita & "</p>")
        informazioni_Cartacee.Append("<p>Referente: " & note.Referente & "</p>")
        informazioni_Cartacee.Append("</body></html>")
        Dim content As Byte() = System.Text.UnicodeEncoding.Unicode.GetBytes(informazioni_Cartacee.ToString)

        estensione = "html"

        Registra_Allegato(op, content, note.NomeAllegato, estensione, codDocumento, 15, , note.Referente, note.Rintracciabilita)
    End Sub

    Public Function Registra_Allegato(ByVal oOperatore As DllAmbiente.Operatore, ByVal bFile() As Byte, ByVal nomeFile As String, ByVal estensione As String, ByVal codDocumento As String, ByVal codTipo As Integer, Optional ByVal versioneAllegato As Integer = 1, Optional ByVal destinatari As String = "", Optional ByVal modalita As String = "", Optional ByVal riferimento_Appendice As String = "", Optional ByVal flagControlloIstanza As Boolean = True, Optional ByVal flagRegistraAttivita As Boolean = True) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_autore) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_binarioAllegato) = bFile

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_nome) = nomeFile
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_descEstensione) = estensione
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_codTipo) = codTipo
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_versioneAllegato) = versioneAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_destinatari) = destinatari

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_modalita) = modalita
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_riferimento_Appendice) = riferimento_Appendice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_controlloIstanza) = flagControlloIstanza
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_flagRegistraAttivita) = flagRegistraAttivita
            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            oDllDocumenti = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Registra_Allegato = vRit
        End Try
    End Function

    Function getNomeTemplateDaUtilizzare(ByVal op As DllAmbiente.Operatore, ByVal strConfigTipoDocumento As String) As String
        Dim tipoDoc As String = ""
        Select Case strConfigTipoDocumento
            Case 0
                tipoDoc = "determina"
            Case 1
                tipoDoc = "delibera"
            Case 2
                tipoDoc = "disposizione"

        End Select
        Dim tipoModello As String = ""
        If op.oUfficio.bUfficioDirigenzaDipartimento Then
            'UDD
            tipoModello = "_dg"

        End If

        '    Dim hsu As HttpServerUtility = HttpContext.Current.Server

        Dim uploadFolder As String = ConfigurationManager.AppSettings.Get("template")
        Dim returnValue As String = ""

        'returnValue = IO.Path.Combine(hsu.MapPath(path), uploadFolder)
        Dim lstr_stringTemplate As String = "" & tipoDoc & tipoModello & ".pdf"
        returnValue = Path.Combine(uploadFolder, lstr_stringTemplate)


        'Return "C:\determina.pdf"

        Return returnValue '"C:\determina.pdf"
    End Function
    Function creaPDFunico(ByVal pdfByte() As Byte, ByVal op As DllAmbiente.Operatore, ByVal strConfigTipoDocumento As String) As MemoryStream
        Dim memStreamReturn As MemoryStream

        Dim f As Integer = 1
        ' we create a reader for a certain document
        Dim lstr_modelloDaUtilizzare As String = ""
        lstr_modelloDaUtilizzare = "" & getNomeTemplateDaUtilizzare(op, strConfigTipoDocumento) 'C:\determina.pdf"

        Dim reader As New PdfReader(lstr_modelloDaUtilizzare)

        Dim readerExt As New PdfReader(pdfByte)
        Dim countPageTotali As Integer = readerExt.NumberOfPages
        ' we retrieve the total number of pages
        Dim n As Integer = reader.NumberOfPages

        ' step 1: creation of a document-object
        Dim document As New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
        ' step 2: we create a writer that listens to the document

        memStreamReturn = New MemoryStream
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memStreamReturn)

        ' step 3: we open the document
        document.Open()
        Dim cb As PdfContentByte = writer.DirectContent
        Dim cb1 As PdfContentByte = writer.DirectContent
        Dim page As PdfImportedPage
        Dim rotation As Integer
        ' step 4: we add content

        Dim xUfficio As Single = 355

        Dim flagAppendiceUltimaPagina As String = "0"

        Dim i As Integer = 0

        While (i < n Or i = 1)
            '''''''''''''
            i += 1
            'Gestione Appendice
            If i = 2 Then
                inserisciModelloUnico(cb, pdfByte, document, writer, readerExt)
            End If

            If strConfigTipoDocumento <> 2 OrElse (strConfigTipoDocumento = 2 And i = 1) Then
                document.SetPageSize(reader.GetPageSizeWithRotation(i))
                document.NewPage()



                page = writer.GetImportedPage(reader, i)
                rotation = reader.GetPageRotation(i)

                Dim lstr_xpos As String = ConfigurationManager.AppSettings("xPagina")
                Dim lstr_ypos As String = ConfigurationManager.AppSettings("yPagina")
                If Not String.IsNullOrEmpty(lstr_xpos) And Not String.IsNullOrEmpty(lstr_xpos) Then
                    inserisciDicituraPagina(IIf(i < n, i, countPageTotali + 2), countPageTotali + 2, writer, lstr_xpos, lstr_ypos)
                End If

                If rotation = 90 OrElse rotation = 270 Then
                    cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                Else
                    cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                End If
            End If
        End While

        f += 1

        document.Close()
        '    Return memStreamReturn

        'Dim fileSys As New IO.FileStream("C:\provapdfMege.pdf", IO.FileMode.OpenOrCreate)

        Return memStreamReturn

    End Function
    Function UnisciPremessaEDispositivo(ByVal pdfPremessaByte() As Byte, ByVal pdfDispositivoByte() As Byte, ByVal op As DllAmbiente.Operatore, ByVal strConfigTipoDocumento As String) As MemoryStream
        Dim memStreamReturn As MemoryStream


        Dim f As Integer = 1
        ' we create a reader for a certain document
        Dim reader As New PdfReader(pdfPremessaByte)
        Dim countPageTotali As Integer = reader.NumberOfPages
        Dim readerExt As New PdfReader(pdfDispositivoByte)
        Dim countPageTotali2 As Integer = readerExt.NumberOfPages
        ' we retrieve the total number of pages
        Dim n As Integer = reader.NumberOfPages

        ' step 1: creation of a document-object
        Dim document As New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
        ' step 2: we create a writer that listens to the document

        memStreamReturn = New MemoryStream
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memStreamReturn)

        ' step 3: we open the document
        document.Open()
        Dim cb As PdfContentByte = writer.DirectContent
        Dim cb1 As PdfContentByte = writer.DirectContent
        Dim page As PdfImportedPage
        Dim rotation As Integer
        Dim i As Integer = 0

        While (i < countPageTotali)
            '''''''''''''
            i += 1

            document.SetPageSize(reader.GetPageSizeWithRotation(i))
            document.NewPage()



            page = writer.GetImportedPage(reader, i)
            rotation = reader.GetPageRotation(i)




            If rotation = 90 OrElse rotation = 270 Then
                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
            Else
                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
            End If


        End While


        i = 0
        While (i < countPageTotali2)
            '''''''''''''
            i += 1

            document.SetPageSize(readerExt.GetPageSizeWithRotation(i))
            document.NewPage()



            page = writer.GetImportedPage(readerExt, i)
            rotation = readerExt.GetPageRotation(i)




            If rotation = 90 OrElse rotation = 270 Then
                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, readerExt.GetPageSizeWithRotation(i).Height)
            Else
                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
            End If


        End While







        document.Close()


        Return memStreamReturn

    End Function
    Sub inserisciDicituraPagina(ByVal paginaCorrente As String, ByVal paginaTotale As String, ByRef writer As PdfWriter, ByVal xpos As Single, ByVal ypos As Single)
        Dim ft As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", Single.Parse("7,98"), iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.GRAY) 'New Font(iTextSharp.text.Font., 10, iTextSharp.text.Font.BOLD)

        Dim TabellaApp As PdfPTable = New PdfPTable(1)
        TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
        TabellaApp.AddCell(New Phrase("Pagina " & paginaCorrente & " di " & paginaTotale, ft))
        TabellaApp.TotalWidth = 300
        TabellaApp.WriteSelectedRows(0, -1, xpos, ypos, writer.DirectContent)
    End Sub
    Sub inserisciModelloUnico(ByRef cb As PdfContentByte, ByVal pdfByte() As Byte, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal reader_out As iTextSharp.text.pdf.PdfReader)


        Dim result As Boolean = False
        Dim pdfCount As Integer = 1 'total input pdf file count
        Dim f As Integer = 0 'pointer to current input pdf file
        Dim fileName As String = String.Empty 'current input pdf filename
        Dim reader As iTextSharp.text.pdf.PdfReader = reader_out
        Dim pageCount As Integer = 0 'cureent input pdf page count



        'Declare a variable to hold the imported pages
        Dim page As PdfImportedPage = Nothing
        Dim rotation As Integer = 0
        'Declare a font to used for the bookmarks
        Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA,
        12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE)
        Try

            If pdfCount > 0 Then
                'Open the 1st pad using PdfReader object
                If reader Is Nothing Then
                    reader = New iTextSharp.text.pdf.PdfReader(pdfByte)
                End If

                'Get page count
                pageCount = reader.NumberOfPages



                'Now loop thru the input pdfs
                While f < pdfCount
                    'Declare a page counter variable
                    Dim i As Integer = 0
                    'Loop thru the current input pdf's pages starting at page 1
                    While i < pageCount


                        i += 1
                        'Get the input page size
                        pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                        'Create a new page on the output document
                        pdfDoc.NewPage()
                        'If it is the 1st page, we add bookmarks to the page
                        If i = 1 Then
                            'First create a paragraph using the filename as the heading
                            '                            Dim para As New iTextSharp.text.Paragraph(IO.Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                            'Then create a chapter from the above paragraph
                            para.IndentationLeft = -50000000
                            Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                            'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                            'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            'TabellaNumero.AddCell(para)
                            'TabellaNumero.TotalWidth = 1
                            'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                            'Finally add the chapter to the document

                            chpter.NumberDepth = -1
                            pdfDoc.Add(chpter)

                        End If
                        'Now we get the imported page
                        page = writer.GetImportedPage(reader, i)
                        Dim contaRighe As Integer = 25
                        Dim linea As Integer = 0

                        'Read the imported page's rotation
                        rotation = reader.GetPageRotation(i)
                        'Then add the imported page to the PdfContentByte object as a template based on the page's rotation
                        Dim lstr_xpos As String = ConfigurationManager.AppSettings("xPagina")
                        Dim lstr_ypos As String = ConfigurationManager.AppSettings("yPagina")
                        If Not String.IsNullOrEmpty(lstr_xpos) And Not String.IsNullOrEmpty(lstr_xpos) Then
                            inserisciDicituraPagina(i + 1, pageCount + 2, writer, lstr_xpos, lstr_ypos)
                        End If
                        If rotation = 90 Then
                            cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                        ElseIf rotation = 270 Then
                            cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                        Else
                            cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                        End If


                    End While
                    'Increment f and read the next input pdf file
                    f += 1

                End While
                'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                result = True
            End If
        Catch ex As Exception
            Log.Error("Inserisci Modello: " & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Sub
    'fine Creazione

    Public Function Cancella_Allegati(ByVal op As DllAmbiente.Operatore, ByVal codAllegato As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Cancella_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Cancella_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = op

            oDllDocumenti = New DllDocumentale.svrDocumenti(op)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato.c_idAllegato) = codAllegato  'idoperatore

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            oDllDocumenti = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Cancella_Allegati = vRit
        End Try
    End Function


    Public Function Cancella_Allegato_Fisicamente(ByVal op As DllAmbiente.Operatore, Optional ByVal codAllegato As String = "", Optional ByVal CodTipologia As String = "", Optional ByVal idDocumento As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Cancella_Allegato_Fisicamente
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Cancella_Allegato_Fisicamente) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(op)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_idAllegato) = codAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_idDocumento) = idDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_tipologiaAllegato) = CodTipologia

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = op.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            oDllDocumenti = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Cancella_Allegato_Fisicamente = vRit
        End Try
    End Function


    Function ModificaDocumento(ByVal request As InserisciMandatiRequest) As InserisciMandatiResponse Implements ProvvedimentiPortType.ModificaDocumento
        Dim numProvvisorio As String = ""
        Dim numDefinitivo As String = ""
        Dim codDocumento As String = ""

        Dim response As New InserisciMandatiResponse
        response.Messaggio_Risposta = New MessaggioRisposta_Types
        response.Messaggio_Risposta.Intestazione = CreaIntestazione()

        Try
            Dim eccez As Eccezione_Types = CreaEccezionePerTipoRichiesta(request.Messaggio_Richiesta.Richiesta.Item.GetType, GetType(ModificaDocumento_Types))

            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If
            Dim objRichiestaModifica As ModificaDocumento_Types = request.Messaggio_Richiesta.Richiesta.Item

            'Trovare operatore da cf
            Dim op As DllAmbiente.Operatore = GetOperatoreDaRequest(objRichiestaModifica.Cod_Fiscale, objRichiestaModifica.Cod_Ufficio)

            eccez = CreaEccezioneOperatore(op)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            numDefinitivo = objRichiestaModifica.DocumentoInfo.NumeroDefinitivo()
            numProvvisorio = objRichiestaModifica.DocumentoInfo.NumeroProvvisorio

            If String.IsNullOrEmpty(numDefinitivo) And String.IsNullOrEmpty(numProvvisorio) Then
                response.Messaggio_Risposta.Item = CreaEccezioneNumDefeNumProvvMancante()
                Return response
            End If

            eccez = validatePrioritàProvvedimento(op, objRichiestaModifica.DocumentoInfo.Urgente)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Dim contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo) = New Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)
            eccez = validateSchedaLeggeTrasparenzaEschedaContrFattInfo(op, objRichiestaModifica.DocumentoInfo.Legge_Trasparenza, contratti)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            eccez = validateSchedaTipologiaProvvedimentoInfo(op, objRichiestaModifica.DocumentoInfo.Tipologia_Provvedimento, contratti)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            codDocumento = String.Empty

            Dim str_codApp As String = request.Messaggio_Richiesta.Intestazione.Applicazione
            Dim doc As New DocumentoInfo_Types
            Dim docItem As DllDocumentale.Model.DocumentoInfo = Nothing
            If Not String.IsNullOrEmpty(numDefinitivo) Then
                docItem = GetDocumentoFrom_NumProv_NumDef_CodEster(op, numDefinitivo, 1, str_codApp)
            ElseIf Not String.IsNullOrEmpty(numProvvisorio) Then
                docItem = GetDocumentoFrom_NumProv_NumDef_CodEster(op, numProvvisorio, 0, str_codApp)
            End If

            If Not docItem Is Nothing Then
                codDocumento = docItem.Doc_id

                If String.IsNullOrEmpty(codDocumento) Then
                    Log.Error("Impossibile Trovare provvedimento con Num definitivo: " & numDefinitivo & " e num prov: " & numProvvisorio)
                    response.Messaggio_Risposta.Item = CreaEccezioneDocumentoNonTrovato()
                    Return response
                Else
                    If String.IsNullOrEmpty(objRichiestaModifica.DocumentoInfo.Oggetto) Then
                        eccez = CreaEccezioneCampoObbligatorio("Oggetto")
                        response.Messaggio_Risposta.Item = eccez
                        Return response
                    End If

                    Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = New DllDocumentale.svrDocumenti(op).Get_StatoIstanzaDocumento(codDocumento)
                    If LCase(statoIstanza.Operatore) = LCase(op.Codice) And statoIstanza.LivelloUfficio = "UP" Then
                        Dim vrit As Object = RegistraDocumentoProv(op, codDocumento, objRichiestaModifica.DocumentoInfo.Oggetto, , , objRichiestaModifica.DocumentoInfo.Tipo_Pubblicazione, , -1, "")

                        'modifica la priorità del provvedimento. se urgente, l'operatore deve essere abilitato all'inoltro di atti urgenti
                        impostaPrioritàProvvedimento(op, codDocumento, objRichiestaModifica.DocumentoInfo.Urgente)

                        'modifica le info riguardanti la legge sulla trasparenza degli atti nella pubblica amministrazione
                        impostaInfoSchedaLeggeTrasparenza(op, codDocumento, objRichiestaModifica.DocumentoInfo.Legge_Trasparenza, True)

                        'modifica le info riguardanti la tipologia e i destinatari del provvedimento
                        impostaInfoSchedaTipologiaProvvedimento(op, codDocumento, objRichiestaModifica.DocumentoInfo.Tipologia_Provvedimento, contratti, True)

                        'modifica le info riguardanti i contratti e le fatture
                        impostaInfoSchedaContrattiFatture(op, codDocumento, objRichiestaModifica.DocumentoInfo.Legge_Trasparenza, contratti, True)


                        'Gestione Corpo Atto
                        gestioneCorpo(codDocumento, objRichiestaModifica.DocumentoInfo.Corpo_Atto, op, objRichiestaModifica.DocumentoInfo.Tipo_Atto)

                        If ConfigurationManager.AppSettings("CancellaAllegatiModifica") = "1" Then
                            Cancella_TipoAllegati(op, codDocumento, TipoAllegatiDb.AllegatoGeneric)
                            Cancella_TipoAllegati(op, codDocumento, TipoAllegatiDb.AllegatoUfficio)
                            Cancella_TipoAllegati(op, codDocumento, TipoAllegatiDb.AllegatoNotaCartaceo)
                            Cancella_TipoAllegati(op, codDocumento, TipoAllegatiDb.AllegatoPor)
                        End If
                        If ConfigurationManager.AppSettings("InserisciAllegatiModifica") = "1" Then

                            'gestione  allegato
                            If Not objRichiestaModifica.DocumentoInfo.Lista_Allegati Is Nothing Then
                                If Not objRichiestaModifica.DocumentoInfo.Lista_Allegati.Allegati Is Nothing Then
                                    For Each allegato As Allegato_Types In objRichiestaModifica.DocumentoInfo.Lista_Allegati.Allegati
                                        PreparaERegistraAllegato(codDocumento, allegato, op)
                                    Next
                                End If

                                If Not objRichiestaModifica.DocumentoInfo.Lista_Allegati.Nota_Cartacea Is Nothing Then
                                    For Each allegato As Nota_Cartacea_Types In objRichiestaModifica.DocumentoInfo.Lista_Allegati.Nota_Cartacea
                                        PreparaERegistraNotaCartacea(codDocumento, allegato, op)
                                    Next
                                End If
                            End If
                        End If

                    Else
                        'Istanza non incarico oppure al di fuori dell ufficio proponente
                        response.Messaggio_Risposta.Item = CreaEccezioneAutorizzazione()
                        Return response
                End If

                End If

            Else
                'Documento non trovato
                Log.Error("Impossibile Trovare provvedimento con Num definitivo: " & numDefinitivo & " e num prov: " & numProvvisorio)
                response.Messaggio_Risposta.Item = CreaEccezioneDocumentoNonTrovato()
                Return response
            End If


            Dim lstrMessaggio As String = "Provvedimento Modifica con successo"
            Dim lstrCodiceRitorno As String = "0"

            Dim succ As New Successo_Types
            Dim rispCrea As New Risposta_ModificaProvvedimento_Types
            rispCrea.Codice = 0
            rispCrea.Stato = StatoProvvedimento.Modificato
            succ.Item = rispCrea

            response.Messaggio_Risposta.Item = succ

        Catch ex As Exception
            Log.Error("Modifica Documento" & ex.Message)
            response.Messaggio_Risposta.Item = CreaEccezioneGenerica(ex.Message, ex.Message)
        End Try

        Return response

    End Function


    Function InfoDocumento(ByVal request As InserisciMandatiRequest) As InserisciMandatiResponse Implements ProvvedimentiPortType.InfoDocumento
        Dim numProvvisorio As String = ""
        Dim numDefinitivo As String = ""
        Dim codDocumento As String = ""

        Dim response As New InserisciMandatiResponse
        response.Messaggio_Risposta = New MessaggioRisposta_Types
        response.Messaggio_Risposta.Intestazione = CreaIntestazione()

        Try
            Dim eccez As Eccezione_Types = CreaEccezionePerTipoRichiesta(request.Messaggio_Richiesta.Richiesta.Item.GetType, GetType(InfoDocumento_Types))

            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If
            Dim objRichiestaInfo As InfoDocumento_Types = request.Messaggio_Richiesta.Richiesta.Item

            'request.Messaggio_Richiesta.Intestazione.Applicazione
            'request.Messaggio_Richiesta.Intestazione.InfoMittDest

            'Trovare operatore da cf
            Dim op As DllAmbiente.Operatore = GetOperatoreDaRequest(objRichiestaInfo.Cod_Fiscale, objRichiestaInfo.Cod_Ufficio)

            eccez = CreaEccezioneOperatore(op)
            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            numProvvisorio = objRichiestaInfo.NumeroProvvisorio
            numDefinitivo = objRichiestaInfo.NumeroDefinitivo

            If String.IsNullOrEmpty(numProvvisorio) And String.IsNullOrEmpty(numDefinitivo) Then
                response.Messaggio_Risposta.Item = CreaEccezioneNumDefeNumProvvMancante()
                Return response

            End If
            Dim doc As New DocumentoInfo_Types

            Dim docItem As DllDocumentale.Model.DocumentoInfo = Nothing
            If Not String.IsNullOrEmpty(numDefinitivo) Then
                docItem = GetDocumentoFrom_NumProv_NumDef_CodEster(op, numDefinitivo, 1)
            ElseIf Not String.IsNullOrEmpty(numProvvisorio) Then
                docItem = GetDocumentoFrom_NumProv_NumDef_CodEster(op, numProvvisorio, 0)
            End If

            If Not docItem Is Nothing AndAlso Not docItem.Doc_id Is Nothing Then
                doc.Cod_Esterno = docItem.Doc_cod_Esterno


                doc.Flag_Investimento_Pub = docItem.Doc_Investimento_Pub
                doc.Flag_privacy = docItem.Doc_privacy
                doc.Oggetto = docItem.Doc_Oggetto
                doc.NumeroDefinitivo = docItem.Doc_numero
                doc.Data_AttoSpecified = False
                If Not String.IsNullOrEmpty(docItem.Doc_numero) Then
                    doc.Data_AttoSpecified = True
                    doc.Data_Atto = docItem.Doc_Data
                End If
                doc.Stato = 0
                Dim docDll As New DllDocumentale.svrDocumenti(op)
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = docDll.Get_StatoIstanzaDocumento(docItem.Doc_id)

                CaricaInfoSchedaLeggeTrasparenza(op, doc, docItem.Doc_id)
                CaricaInfoSchedaTipologiaProvvedimento(op, doc, docItem.Doc_id)
                CaricaDatiContabilPerInfo(op, doc, docItem.Doc_id)

                doc.Data_ModificaSpecified = True
                doc.Data_Modifica = docItem.Doc_Data
                If Not statoIstanza Is Nothing Then
                    doc.Data_Modifica = statoIstanza.DataUltimaOperazione
                    doc.Stato = FromLivelloToStato(statoIstanza.LivelloUfficio, statoIstanza.Ruolo)
                End If


                doc.Tipo_Atto = docItem.Doc_Tipo
                doc.Tipo_Pubblicazione = docItem.Doc_Pubblicazione
                If objRichiestaInfo.Flag_RichiestaAllegati = 1 Then


                    Dim item_listaAllegati As List(Of Allegato_Types) = ListaAllegati(docItem.Doc_id, op)
                    If Not item_listaAllegati Is Nothing AndAlso item_listaAllegati.Count > 0 Then
                        doc.Lista_Allegati = New Lista_Allegati_Types
                        doc.Lista_Allegati.Allegati = item_listaAllegati.ToArray()
                    End If
                End If

            Else
                Log.Error("Impossibile Trovare provvedimento con Num definitivo: " & numDefinitivo & " e num prov: " & numProvvisorio)
                'Documento non trovato
                response.Messaggio_Risposta.Item = CreaEccezioneDocumentoNonTrovato()
                Return response
            End If

            Dim info As New Risposta_InfoProvvedimento_Types
            info.DocumentoInfo = doc


            Dim succ As New Successo_Types

            succ.Item = info
            response.Messaggio_Risposta.Item = succ

        Catch ex As Exception
            Log.Error("Info Documento" & ex.Message)
            response.Messaggio_Risposta.Item = CreaEccezioneGenerica(ex.Message, ex.Message)
        End Try

        Return response
    End Function

    Function getInfoSchedaTipologiaProvvedimento(ByVal operatore As DllAmbiente.Operatore, ByVal codDocumento As String) As Tipologia_Provvedimento_Types
        Dim retValue As Tipologia_Provvedimento_Types = Nothing

        Dim docDll As New DllDocumentale.svrDocumenti(operatore)
        Dim schedaTipologiaProvvedimentoInfo As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = docDll.FO_Get_SchedaTipologiaProvvedimentoInfo(operatore, codDocumento)

        If Not schedaTipologiaProvvedimentoInfo Is Nothing Then
            retValue = New Tipologia_Provvedimento_Types()

            'tipologia provvedimento
            retValue.IdTipologiaProvvedimento = schedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento

            'importo spesa prevista
            If Not schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista Is Nothing Then
                retValue.ImportoSpesaPrevistaSpecified = True
                retValue.ImportoSpesaPrevista = schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista
            Else
                retValue.ImportoSpesaPrevistaSpecified = False
                retValue.ImportoSpesaPrevista = Nothing
            End If

            retValue.IsSommaAutomaticaSpecified = True
            retValue.IsSommaAutomatica = schedaTipologiaProvvedimentoInfo.isSommaAutomatica

            'destinatari
            Dim destinatari As Generic.List(Of Destinatario_Types) = New Generic.List(Of Destinatario_Types)

            If Not schedaTipologiaProvvedimentoInfo.Destinatari Is Nothing Then
                For Each itemDestinatarioInfo As DllDocumentale.ItemDestinatarioInfo In schedaTipologiaProvvedimentoInfo.Destinatari
                    Dim destinatario As Destinatario_Types = New Destinatario_Types()

                    destinatario.ID_Anagrafica = itemDestinatarioInfo.IdSIC
                    destinatario.Denominazione = itemDestinatarioInfo.Denominazione
                    destinatario.Flag_Persona_Fisica = itemDestinatarioInfo.isPersonaFisica

                    destinatario.Dati_Fiscali = New Destinatario_TypesDati_Fiscali()
                    destinatario.Dati_Fiscali.ItemElementName = IIf(itemDestinatarioInfo.isPersonaFisica, ItemChoiceType1.Codice_Fiscale, ItemChoiceType1.Partita_Iva)
                    destinatario.Dati_Fiscali.Item = IIf(itemDestinatarioInfo.isPersonaFisica, itemDestinatarioInfo.CodiceFiscale, itemDestinatarioInfo.PartitaIva)

                    destinatario.Luogo_Nascita = IIf(isStringNullOrEmpty(itemDestinatarioInfo.LuogoNascita), Nothing, itemDestinatarioInfo.LuogoNascita)
                    destinatario.Data_Nascita = IIf(isStringNullOrEmpty(itemDestinatarioInfo.DataNascita), Nothing, itemDestinatarioInfo.DataNascita)
                    destinatario.Legale_Rappresentante = IIf(isStringNullOrEmpty(itemDestinatarioInfo.LegaleRappresentante), Nothing, itemDestinatarioInfo.LegaleRappresentante)

                    destinatario.Contratto = New Contratto_Types()
                    destinatario.Contratto.Id_Contratto = itemDestinatarioInfo.IdContratto

                    destinatario.IsDatoSensibile = itemDestinatarioInfo.isDatoSensibile
                    destinatario.ImportoSpettante = itemDestinatarioInfo.ImportoSpettante

                    destinatari.Add(destinatario)
                Next
            End If

            retValue.Lista_Destinatari = IIf(destinatari.Count > 0, destinatari.ToArray, Nothing)
        Else
            If Not Err() Is Nothing AndAlso Not isStringNullOrEmpty(Err.Description) Then
                Throw New Exception("Impossibile recuperare le informazioni sulla scheda 'Tipologia Provvedimento': " + Err.Description)
            End If
        End If

        Return retValue
    End Function

    Function getInfoSchedaLeggeTrasparenza(ByVal operatore As DllAmbiente.Operatore, ByVal codDocumento As String) As Legge_Trasparenza_Types
        Dim retValue As Legge_Trasparenza_Types = Nothing

        Dim docDll As New DllDocumentale.svrDocumenti(operatore)
        Dim schedaLeggeTrasparenzaInfo As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = docDll.FO_Get_SchedaLeggeTrasparenzaInfo(operatore, codDocumento)

        If Not schedaLeggeTrasparenzaInfo Is Nothing Then
            retValue = New Legge_Trasparenza_Types()

            retValue.Autorizzazione_Pubblicazione = schedaLeggeTrasparenzaInfo.AutorizzazionePubblicazione
            retValue.Note_Pubblicazione = schedaLeggeTrasparenzaInfo.NotePubblicazione
            retValue.Norma_Attribuzione_Beneficio = schedaLeggeTrasparenzaInfo.NormaAttribuzioneBeneficio
            retValue.Ufficio_Responsabile_Procedimento = schedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento
            retValue.Funzionario_Responsabile_Procedimento = schedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento
            retValue.Modalita_Individuazione_Beneficiario = schedaLeggeTrasparenzaInfo.ModalitaIndividuazioneBeneficiario
            retValue.Contenuto_Atto = schedaLeggeTrasparenzaInfo.ContenutoAtto

            'contratti
            Dim contratti As Generic.List(Of Contratto_Types) = New Generic.List(Of Contratto_Types)

            If Not schedaLeggeTrasparenzaInfo.Contratti Is Nothing Then
                For Each contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader In schedaLeggeTrasparenzaInfo.Contratti
                    Dim contratto As Contratto_Types = New Contratto_Types()
                    contratto.Id_Contratto = contrattoInfoHeader.IdContratto
                    contratti.Add(contratto)
                Next
            End If

            retValue.Lista_Contratti = IIf(contratti.Count > 0, contratti.ToArray, Nothing)
        Else
            If Not Err() Is Nothing AndAlso Not isStringNullOrEmpty(Err.Description) Then
                Throw New Exception("Impossibile recuperare le informazioni sulla scheda 'Legge Trasparenza': " + Err.Description)
            End If
        End If

        Return retValue
    End Function

    Sub CaricaInfoSchedaLeggeTrasparenza(ByVal operatore As DllAmbiente.Operatore, ByRef doc As DocumentoInfo_Types, ByVal codDocumento As String)
        doc.Legge_Trasparenza = getInfoSchedaLeggeTrasparenza(operatore, codDocumento)
    End Sub

    Sub CaricaInfoSchedaTipologiaProvvedimento(ByVal operatore As DllAmbiente.Operatore, ByRef doc As DocumentoInfo_Types, ByVal codDocumento As String)
        doc.Tipologia_Provvedimento = getInfoSchedaTipologiaProvvedimento(operatore, codDocumento)
    End Sub

    Sub CaricaDatiContabilPerInfo(ByVal operatore As DllAmbiente.Operatore, ByRef doc As DocumentoInfo_Types, ByVal codDocumento As String)
        Dim docDll As New DllDocumentale.svrDocumenti(operatore)
        doc.Dati_Contabili = New Dati_Contabili_Types

        Dim listaImpegni As List(Of DllDocumentale.ItemImpegnoInfo) = docDll.FO_Get_DatiImpegni(codDocumento)
        Dim listaImpegni_WS As New List(Of Impegno_Types)
        Dim listaImpegni_Perenti_WS As New List(Of Impegno_Su_Perente_Types)
        'CArico tutti gli impegni perenti e non 
        If Not listaImpegni Is Nothing Then
            Dim impegnoItem As Impegno_Types = Nothing
            Dim impegnoPerenteItem As Impegno_Su_Perente_Types = Nothing
            For Each impegno In listaImpegni
                If impegno.Di_Stato <> 0 Then
                    If String.IsNullOrEmpty(impegno.NDocPrecedente) Then
                        impegnoItem = New Impegno_Types

                        impegnoItem.Liquidazioni = (New List(Of Liquidazione_Types)).ToArray
                        impegnoItem.UPB = impegno.Dli_UPB
                        impegnoItem.MissioneProgramma = impegno.Dli_MissioneProgramma
                        impegnoItem.Piano_dei_Conti_Finanziario = impegno.Piano_Dei_Conti_Finanziari
                        impegnoItem.Codice_obiettivo_Gestionale = impegno.Codice_Obbiettivo_Gestionale
                        impegnoItem.Anno = impegno.DBi_Anno
                        impegnoItem.Bilancio = impegno.Dli_Esercizio
                        impegnoItem.Capitolo = impegno.Dli_Cap
                        impegnoItem.ImportoSpecified = True
                        impegnoItem.Importo = impegno.Dli_Costo
                        impegnoItem.NumeroImpegno = impegno.Dli_NumImpegno
                        impegnoItem.NumeroPreImpegno = impegno.Dli_NPreImpegno

                        listaImpegni_WS.Add(impegnoItem)
                    Else
                        impegnoPerenteItem = New Impegno_Su_Perente_Types
                        impegnoPerenteItem.UPB = impegno.Dli_UPB
                        impegnoPerenteItem.MissioneProgramma = impegno.Dli_MissioneProgramma
                        impegnoPerenteItem.Piano_dei_Conti_Finanziario = impegno.Piano_Dei_Conti_Finanziari
                        impegnoPerenteItem.Codice_obiettivo_Gestionale = impegno.Codice_Obbiettivo_Gestionale
                        impegnoPerenteItem.Anno = impegno.DBi_Anno
                        impegnoPerenteItem.Bilancio = impegno.Dli_Esercizio
                        impegnoPerenteItem.Capitolo = impegno.Dli_Cap
                        impegnoPerenteItem.ImportoSpecified = True
                        impegnoPerenteItem.Importo = impegno.Dli_Costo
                        impegnoPerenteItem.NumeroImpegno = impegno.Dli_NumImpegno
                        impegnoPerenteItem.NumeroImpegnoPerente = impegno.NDocPrecedente
                        listaImpegni_Perenti_WS.Add(impegnoPerenteItem)
                    End If

                End If

            Next

        End If

        'Carico tutte le liquidazioni e verifico se è contestuale o meno
        Dim listaliquidazioni As List(Of DllDocumentale.ItemLiquidazioneInfo) = docDll.FO_Get_DatiLiquidazione(codDocumento, , , True)
        Dim listaLiq_WS As New List(Of Liquidazione_Types)
        If Not listaliquidazioni Is Nothing Then
            Dim liquidazioneItem As Liquidazione_Types = Nothing

            For Each liq In listaliquidazioni
                If liq.Di_Stato <> 0 Then

                    liquidazioneItem = New Liquidazione_Types
                    liquidazioneItem.UPB = liq.Dli_UPB
                    liquidazioneItem.MissioneProgramma = liq.Dli_MissioneProgramma
                    liquidazioneItem.Esercizio = liq.Dli_Esercizio
                    liquidazioneItem.Capitolo = liq.Dli_Cap
                    liquidazioneItem.ImportoSpecified = True
                    liquidazioneItem.Importo = liq.Dli_Costo
                    liquidazioneItem.NumeroImpegno = liq.Dli_NumImpegno
                    liquidazioneItem.DataAssunzione = liq.Dli_Data_Assunzione
                    liquidazioneItem.NumeroAssunzione = liq.Dli_Num_assunzione
                    liquidazioneItem.NumeroLiquidazione = liq.Dli_NLiquidazione
                    liquidazioneItem.TipoAssunzione = liq.Dli_TipoAssunzione

                    'aggiungo i beneficiari alla liquidazione
                    'FO_Get_ListaFattureLiquidazione
                    Dim listaFatture As List(Of DllDocumentale.ItemFatturaInfoHeader) = docDll.FO_Get_ListaFattureLiquidazione(liq.Dli_prog, codDocumento)

                    associaBeneficiariLiquidazioneWS(liquidazioneItem, liq.ListaBeneficiari, listaFatture, docDll)

                    If Not verificaImpegnoEliquidazioniContestuali(liq.Dli_NPreImpegno, liq.Dli_NumImpegno, liquidazioneItem, listaImpegni_WS, listaImpegni_Perenti_WS) Then
                        'verifico se esiste un impegno
                        'altrimenti aggiungo alla listaLiq
                        listaLiq_WS.Add(liquidazioneItem)
                    End If

                End If
            Next

        End If

        Dim listaRiduzioni As List(Of DllDocumentale.ItemRiduzioneInfo) = docDll.FO_Get_DatiImpegniVariazioni(codDocumento)

        If Not listaImpegni_WS Is Nothing Then
            doc.Dati_Contabili.Impegni = listaImpegni_WS.ToArray
        End If

        If Not listaImpegni_Perenti_WS Is Nothing Then
            doc.Dati_Contabili.ImpegniSuPerenti = listaImpegni_Perenti_WS.ToArray
        End If

        If Not listaLiq_WS Is Nothing Then
            doc.Dati_Contabili.Liquidazioni = listaLiq_WS.ToArray
        End If
    End Sub

    Function verificaImpegnoEliquidazioniContestuali(ByVal numPrePimp As String, ByVal numImpegno As String, ByRef liq As Liquidazione_Types, ByRef listaImpegniWs As List(Of Impegno_Types), ByRef listaImpegniPerentiWs As List(Of Impegno_Su_Perente_Types)) As Boolean
        If Not listaImpegniPerentiWs Is Nothing Then
            For Each imp In listaImpegniPerentiWs
                If imp.NumeroImpegno = numImpegno Then
                    imp.Liquidazioni = liq
                    Return True
                End If

            Next
        End If

        If Not listaImpegniWs Is Nothing Then
            For Each imp In listaImpegniWs
                If imp.NumeroImpegno = numImpegno Or imp.NumeroPreImpegno = numPrePimp Then
                    Dim liquidazioni As Generic.List(Of Liquidazione_Types) = imp.Liquidazioni.ToList
                    liquidazioni.Add(liq)
                    imp.Liquidazioni = liquidazioni.ToArray
                    Return True
                End If
            Next
        End If
        Return False

    End Function

    Function FromLivelloToStato(ByVal LivelloUfficio As String, ByVal ruolo As String) As Integer
        Select Case LivelloUfficio
            Case "UAR"
                Return StatoProvvedimento.Archiviato
            Case "UP"
                If ruolo = "A" Then
                    Return StatoProvvedimento.Archiviato
                Else
                    Return StatoProvvedimento.Modificato
                End If
            Case Else
                Return StatoProvvedimento.Modificato
        End Select
    End Function

    Public Function Elenco_Allegati(ByVal operatore As String, ByVal codDocumento As String, Optional ByRef tipoAllegato As Integer = -1, Optional ByVal allDocumento As Integer = 0, Optional ByVal daStampare As String = Nothing) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Allegati
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Allegati) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = New DllAmbiente.Operatore
            oOperatore.Codice = operatore

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_tipoAllegati) = tipoAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_allDocumento) = allDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_daStampare) = daStampare

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)
            vr(3) = oDllDocumenti.objDocumento.Doc_Cod_Uff_Prop
            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error("elenco Allegati " & ex.Message)
            vRit(0) = Err.Number
            vRit(1) = Err.Description
            vr = vRit
        Finally

            Elenco_Allegati = vr
        End Try
    End Function

    Function ListaAllegati(ByVal codDocumento As String, ByVal ooperatore As DllAmbiente.Operatore) As List(Of Allegato_Types)
        Dim arrListFile As New List(Of Allegato_Types)
        Dim allegato As Allegato_Types = Nothing

        Dim vr As Object = Nothing
        vr = Elenco_Allegati(ooperatore, codDocumento, , , "1")
        Dim idAllegatoFileUnico As String = ""
        If vr(0) = 0 Then

            For i = 0 To UBound(vr(1), 2)
                Dim codAllegato As String = vr(1).getvalue(0, i)
                If vr(1).getvalue(1, i) = "File Unico" Then
                    idAllegatoFileUnico = codAllegato
                Else
                    allegato = New Allegato_Types

                    Dim svrdoc As New DllDocumentale.svrDocumenti(ooperatore)
                    Dim vettoredati
                    Dim nomeFile As String = ""

                    vettoredati = svrdoc.FO_Leggi_Allegato_Con_Parametri(codDocumento, codAllegato)

                    If vr(1).getvalue(1, i).ToString.ToLower.Contains("elenco certificati di firme") Then
                        nomeFile = "Elenco Certificati"
                    Else
                        nomeFile = vettoredati(2)
                    End If
                    allegato.NomeFile = nomeFile & "." & vettoredati(3)
                    allegato.Flag_is_DocumentoPrincipale = 0
                    If (allegato.NomeFile).ToUpper.Contains("Documento_Principale.pdf".ToUpper) Then
                        allegato.Flag_is_DocumentoPrincipale = 1
                    End If
                    allegato.File_Allegato = vettoredati(1)

                    arrListFile.Add(allegato)
                End If
            Next
        End If
        Return arrListFile
    End Function
    Sub Cancella_TipoAllegati(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, ByVal tipologiaAllegato As Integer)
        Log.Debug("Cancella_TipoAllegati op: " & op.Codice & " tipoAllegati " & tipologiaAllegato)
        Dim vrAllegati As Object = Elenco_Allegati(op, codDocumento, tipologiaAllegato)

        If vrAllegati(0) = 0 Then
            For i = 0 To UBound(vrAllegati(1), 2)
                Dim codAllegato As String = vrAllegati(1).getvalue(0, i)
                Log.Debug("codAllegatoDaCancellare " & codAllegato)

                Cancella_Allegati(op, codAllegato)
            Next
        End If
    End Sub
    Private Function verifica_DatiContabili(ByVal operatore As DllAmbiente.Operatore, ByVal dati_contabili As Dati_Contabili_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing
        'inizio gestione dati contabili
        If Not dati_contabili Is Nothing Then
            'Gestione liquidazioni
            If Not dati_contabili.Liquidazioni Is Nothing Then
                For Each liquidazione As Liquidazione_Types In dati_contabili.Liquidazioni

                    'Gestione corrispondenza fra il beneficiario dell'imp e la liquidazione che si sta creando
                    Dim disponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, liquidazione.NumeroImpegno)
                    Dim listaBeneficiariImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario() = disponibilitaImpegno.Beneficiario

                    If Not listaBeneficiariImpegnoSIC Is Nothing AndAlso listaBeneficiariImpegnoSIC.Count() = 1 Then
                        Dim beneficiarioImpegno As Risposta_DisponibilitaImpegno_TypesBeneficiario = listaBeneficiariImpegnoSIC.ElementAt(0)

                        If liquidazione.Lista_Beneficiari.Count() > 1 Then
                            retValue = CreaEccezioneLiquidazione("SU LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " Non è possibile indicare più di un beneficiario per la liqudazione")
                        ElseIf liquidazione.Lista_Beneficiari.Count() = 1 Then
                            Dim beneficiarioLiquidazione As Beneficiario_Types = liquidazione.Lista_Beneficiari.ElementAt(0)
                            If beneficiarioImpegno.IdBeneficiario <> beneficiarioLiquidazione.ID_Anagrafica _
                                           AndAlso beneficiarioImpegno.IdSede <> beneficiarioLiquidazione.ID_Sede _
                                           AndAlso beneficiarioImpegno.InfoAggiuntiveBeneficiario.MetodoPagamento <> beneficiarioLiquidazione.ID_Modalita_Pagamento Then
                                retValue = CreaEccezioneLiquidazione("SU LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " Il beneficiario indicato sulla liquidazione deve corrispondere al beneficairio dell'impegno usato")

                            End If

                            'For Each liquidazione As Liquidazione_Types In impegno.Liquidazioni
                            Dim beneficiariLiquidazione As Beneficiario_Types() = liquidazione.Lista_Beneficiari
                            retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                            If retValue Is Nothing Then
                                If Not String.IsNullOrEmpty(liquidazione.PianoContiFinanziario) Then
                                    If Not verifica_PianoDeiContiFinanziario(operatore, liquidazione.Esercizio, liquidazione.Capitolo, liquidazione.PianoContiFinanziario) Then
                                        retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: " & liquidazione.PianoContiFinanziario)
                                    End If
                                Else
                                    retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: non specificato")
                                End If

                            End If

                        End If
                    Else
                        Dim beneficiariLiquidazione As Beneficiario_Types() = liquidazione.Lista_Beneficiari
                        retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(liquidazione.PianoContiFinanziario) Then
                                If Not verifica_PianoDeiContiFinanziario(operatore, liquidazione.Esercizio, liquidazione.Capitolo, liquidazione.PianoContiFinanziario) Then
                                    retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ - anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: " & liquidazione.PianoContiFinanziario)
                                End If
                            Else
                                retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: non specificato")
                            End If
                        End If
                    End If


                    If Not retValue Is Nothing Then
                        Exit For
                    End If

                Next
            End If

            'Gestione impegni
            If retValue Is Nothing Then
                If Not dati_contabili.Impegni Is Nothing Then
                    For Each impegno As Impegno_Types In dati_contabili.Impegni
                        Dim beneficiarioImpegno As Beneficiario_Types = impegno.Beneficiario
                        If impegno.Beneficiario Is Nothing Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: non specificato")
                        ElseIf String.IsNullOrEmpty(beneficiarioImpegno.ID_Anagrafica) Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: ID_Anagrafica non specificato")
                        ElseIf String.IsNullOrEmpty(beneficiarioImpegno.ID_Sede) Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: ID_Sede non specificato")
                        ElseIf String.IsNullOrEmpty(beneficiarioImpegno.ID_Modalita_Pagamento) Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: ID_Modalita_Pagamento non specificato")
                        ElseIf String.IsNullOrEmpty(beneficiarioImpegno.Importo_Spettante) Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: Importo_Spettante non specificato")
                        ElseIf impegno.Importo <> beneficiarioImpegno.Importo_Spettante Then
                            retValue = CreaEccezioneBeneficiarioImpegno("SU IMP -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Beneficiario: L'importo dell'impegno e l'importo spettante del beneficiario sono differenti")
                        End If

                        If Not impegno.Liquidazioni Is Nothing Then
                            If impegno.Liquidazioni.Count() > 1 Then
                                retValue = CreaEccezioneLiquidazione("SU IMP-LIQ -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Non è possibile effettuare più di una liquidazione per uno stesso impegno")
                            ElseIf impegno.Liquidazioni.Count() = 1 Then
                                Dim liquidazione As Liquidazione_Types = impegno.Liquidazioni.ElementAt(0)
                                If liquidazione.Lista_Beneficiari.Count() > 1 Then
                                    retValue = CreaEccezioneLiquidazione("SU IMP-LIQ -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Non è possibile indicare più di un beneficiario per la liqudazione")
                                ElseIf liquidazione.Lista_Beneficiari.Count() = 1 Then
                                    Dim beneficiarioLiquidazione As Beneficiario_Types = impegno.Liquidazioni.ElementAt(0).Lista_Beneficiari.ElementAt(0)
                                    If beneficiarioImpegno.ID_Anagrafica <> beneficiarioLiquidazione.ID_Anagrafica _
                                       AndAlso beneficiarioImpegno.ID_Sede <> beneficiarioLiquidazione.ID_Sede _
                                       AndAlso beneficiarioImpegno.ID_Modalita_Pagamento <> beneficiarioLiquidazione.ID_Modalita_Pagamento _
                                       AndAlso beneficiarioImpegno.ID_Conto <> beneficiarioLiquidazione.ID_Conto Then
                                        retValue = CreaEccezioneLiquidazione("SU IMP-LIQ -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " Il beneficiario indicato sulla liquidazione e sull'impegno deve corrispondere")
                                    ElseIf beneficiarioImpegno.Importo_Spettante <> liquidazione.Importo AndAlso beneficiarioImpegno.Importo_Spettante <> beneficiarioLiquidazione.Importo_Spettante Then
                                        retValue = CreaEccezioneLiquidazione("SU IMP-LIQ -  anno: " & impegno.Anno & " cap: " & impegno.Capitolo & " L'importo del beneficiario indicato sulla liquidazione e sull'impegno deve corrispondere")
                                    End If

                                    'For Each liquidazione As Liquidazione_Types In impegno.Liquidazioni
                                    Dim beneficiariLiquidazione As Beneficiario_Types() = liquidazione.Lista_Beneficiari
                                    retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                                    If retValue Is Nothing Then
                                        If Not String.IsNullOrEmpty(liquidazione.PianoContiFinanziario) Then
                                            If Not verifica_PianoDeiContiFinanziario(operatore, liquidazione.Esercizio, liquidazione.Capitolo, liquidazione.PianoContiFinanziario) Then
                                                retValue = CreaEccezionePianoDeiContiFinanziario("SU IMP-LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: " & liquidazione.PianoContiFinanziario)
                                            End If
                                        Else
                                            retValue = CreaEccezionePianoDeiContiFinanziario("SU IMP-LIQ -  anno: " & liquidazione.Esercizio & " cap: " & liquidazione.Capitolo & " PCF: non specificato")
                                        End If

                                    End If
                                    If Not retValue Is Nothing Then
                                        Exit For
                                    End If
                                    'Next
                                End If
                            End If
                        End If



                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(impegno.Codice_obiettivo_Gestionale) Then
                                If Not verifica_CodiceObiettivoGestionale(operatore, impegno.Bilancio, impegno.Capitolo, impegno.Codice_obiettivo_Gestionale) Then
                                    retValue = CreaEccezioneCOG()
                                End If
                            End If
                        End If

                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(impegno.Piano_dei_Conti_Finanziario) Then
                                Dim anno As String = ""
                                If impegno.Bilancio Is Nothing OrElse impegno.Bilancio = "" Then
                                    anno = impegno.Anno
                                Else
                                    anno = impegno.Bilancio
                                End If

                                If Not verifica_PianoDeiContiFinanziario(operatore, anno, impegno.Capitolo, impegno.Piano_dei_Conti_Finanziario) Then
                                    retValue = CreaEccezionePianoDeiContiFinanziario("SU IMP -  anno: " & anno & " cap: " & impegno.Capitolo & " PCF: " & impegno.Piano_dei_Conti_Finanziario)
                                End If
                            End If
                        End If

                        If Not retValue Is Nothing Then
                            Exit For
                        End If
                    Next
                End If
            End If

            'Gestione impegni perenti
            If retValue Is Nothing Then
                If Not dati_contabili.ImpegniSuPerenti Is Nothing Then
                    For Each impegnoPerente As Impegno_Su_Perente_Types In dati_contabili.ImpegniSuPerenti

                        'Gestione corrispondenza fra il beneficiario dell'imp perente e la liquidazione che si sta creando
                        Dim disponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, impegnoPerente.NumeroImpegno)
                        Dim listaBeneficiariImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario() = disponibilitaImpegno.Beneficiario

                        If Not listaBeneficiariImpegnoSIC Is Nothing AndAlso listaBeneficiariImpegnoSIC.Count() = 1 Then
                            Dim beneficiarioImpegnoPerente As Risposta_DisponibilitaImpegno_TypesBeneficiario = listaBeneficiariImpegnoSIC.ElementAt(0)

                            If Not impegnoPerente.Liquidazioni Is Nothing Then
                                Dim beneficiariLiquidazione As Beneficiario_Types() = impegnoPerente.Liquidazioni.Lista_Beneficiari

                                If beneficiariLiquidazione.Count() > 1 Then
                                    retValue = CreaEccezioneLiquidazione("Per l'impegno perente " + impegnoPerente.NumeroImpegno + " è necessario indicare una unica liquidazione ")
                                Else
                                    Dim beneficiarioLiquidazione As Beneficiario_Types = beneficiariLiquidazione.ElementAt(0)
                                    If beneficiarioImpegnoPerente.IdBeneficiario <> beneficiarioLiquidazione.ID_Anagrafica _
                                       AndAlso beneficiarioImpegnoPerente.IdSede <> beneficiarioLiquidazione.ID_Sede _
                                        AndAlso beneficiarioImpegnoPerente.InfoAggiuntiveBeneficiario.MetodoPagamento <> beneficiarioLiquidazione.ID_Modalita_Pagamento Then
                                        retValue = CreaEccezioneLiquidazione("Per l'impegno perente " + impegnoPerente.NumeroImpegno + " il beneficiario della liquidazione differisce da quello indicato per l'impegno. ")
                                    Else
                                        retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                                    End If
                                End If

                            End If

                        ElseIf Not impegnoPerente.Liquidazioni Is Nothing Then
                            Dim beneficiariLiquidazione As Beneficiario_Types() = impegnoPerente.Liquidazioni.Lista_Beneficiari
                            retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                        End If
                        'If Not impegnoPerente.Liquidazioni Is Nothing Then
                        '    Dim beneficiariLiquidazione As Beneficiario_Types() = impegnoPerente.Liquidazioni.Lista_Beneficiari
                        '    retValue = validateBeneficiariFattureLiquidazione(beneficiariLiquidazione, contratti)
                        'End If


                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(impegnoPerente.Liquidazioni.PianoContiFinanziario) Then
                                If Not verifica_PianoDeiContiFinanziario(operatore, impegnoPerente.Liquidazioni.Esercizio, impegnoPerente.Liquidazioni.Capitolo, impegnoPerente.Liquidazioni.PianoContiFinanziario) Then
                                    retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ DI IMPPER -  anno: " & impegnoPerente.Liquidazioni.Esercizio & " cap: " & impegnoPerente.Liquidazioni.Capitolo & " PCF: " & impegnoPerente.Liquidazioni.PianoContiFinanziario)
                                End If
                            Else
                                retValue = CreaEccezionePianoDeiContiFinanziario("SU LIQ DI IMPPER -  anno: " & impegnoPerente.Liquidazioni.Esercizio & " cap: " & impegnoPerente.Liquidazioni.Capitolo & " PCF: non specificato")
                            End If
                        End If


                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(impegnoPerente.Codice_obiettivo_Gestionale) Then
                                If Not verifica_CodiceObiettivoGestionale(operatore, impegnoPerente.Anno, impegnoPerente.Capitolo, impegnoPerente.Codice_obiettivo_Gestionale) Then
                                    retValue = CreaEccezioneCOG()
                                End If
                            End If
                        End If

                        If retValue Is Nothing Then
                            If Not String.IsNullOrEmpty(impegnoPerente.Piano_dei_Conti_Finanziario) Then
                                If Not verifica_PianoDeiContiFinanziario(operatore, impegnoPerente.Anno, impegnoPerente.Capitolo, impegnoPerente.Piano_dei_Conti_Finanziario) Then
                                    retValue = CreaEccezionePianoDeiContiFinanziario("SU IMPPER -  anno: " & impegnoPerente.Anno & " cap: " & impegnoPerente.Capitolo & " PCF: " & impegnoPerente.Piano_dei_Conti_Finanziario)
                                End If
                            End If
                        End If

                        If Not retValue Is Nothing Then
                            Exit For
                        End If
                    Next
                End If
            End If
        End If

        Return retValue
    End Function

    Private Function checkContrattiBeneficiari(ByVal beneficiariLiquidazioneWS() As Beneficiario_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)) As Generic.List(Of String)
        Dim notValidIdContratti As Generic.List(Of String) = New Generic.List(Of String)

        If Not beneficiariLiquidazioneWS Is Nothing Then
            For Each beneficiarioLiqWS As Beneficiario_Types In beneficiariLiquidazioneWS
                If Not beneficiarioLiqWS.Contratto.Id_Contratto Is Nothing Then
                    If Not beneficiarioLiqWS.Contratto.Id_Contratto.Trim() = String.Empty Then
                        Dim contratto As DllDocumentale.ItemContrattoInfo = contratti.Item(beneficiarioLiqWS.Contratto.Id_Contratto)
                        If contratto Is Nothing Then
                            notValidIdContratti.Add(beneficiarioLiqWS.Contratto.Id_Contratto)
                        End If
                    Else
                        notValidIdContratti.Add(beneficiarioLiqWS.Contratto.Id_Contratto)
                    End If
                End If
            Next
        End If

        Return notValidIdContratti
    End Function

    Private Sub associaFatturaEBeneficiariLiquidazione(ByVal liquidazione As DllDocumentale.ItemLiquidazioneInfo, ByVal beneficiariLiquidazioneWS() As Beneficiario_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), ByVal dllDoc As DllDocumentale.svrDocumenti, ByVal codDocumento As String)

        If Not liquidazione.ListaBeneficiari Is Nothing Then
            liquidazione.ListaBeneficiari.Clear()
        Else
            liquidazione.ListaBeneficiari = New Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        End If

        If Not beneficiariLiquidazioneWS Is Nothing Then
            For Each beneficiarioLiqWS As Beneficiario_Types In beneficiariLiquidazioneWS
                Dim benLiq As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo

                benLiq.AddizionaleComunale = beneficiarioLiqWS.Addizionale_Comunale
                benLiq.AddizionaleRegionale = beneficiarioLiqWS.Addizionale_Regionale
                benLiq.AltreRitenute = beneficiarioLiqWS.Altre_Ritenute
                benLiq.Bollo = beneficiarioLiqWS.Bollo

                If (beneficiarioLiqWS.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale) Then
                    benLiq.CodiceFiscale = beneficiarioLiqWS.Dati_Fiscali.Item
                    benLiq.PartitaIva = String.Empty
                ElseIf (beneficiarioLiqWS.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva) Then
                    benLiq.PartitaIva = beneficiarioLiqWS.Dati_Fiscali.Item
                    benLiq.CodiceFiscale = String.Empty
                End If
                benLiq.CodiceSiope = beneficiarioLiqWS.Codice_Siope
                benLiq.CodiceSiopeAggiuntivo = beneficiarioLiqWS.Codice_Siope_Aggiuntivo
                benLiq.Cig = beneficiarioLiqWS.Cod_Cig
                benLiq.Cup = beneficiarioLiqWS.Cod_Cup
                benLiq.DataRegistrazione = Now
                benLiq.Denominazione = beneficiarioLiqWS.Denominazione
                benLiq.EsenzCommBonifico = beneficiarioLiqWS.Esenz_Comm_Bonifico
                benLiq.FlagPersonaFisica = beneficiarioLiqWS.Flag_Persona_Fisica
                benLiq.Iban = beneficiarioLiqWS.IBAN
                benLiq.IdAnagrafica = beneficiarioLiqWS.ID_Anagrafica
                benLiq.IdConto = beneficiarioLiqWS.ID_Conto
                benLiq.IdDocumento = liquidazione.Dli_Documento
                benLiq.IdModalitaPag = beneficiarioLiqWS.ID_Modalita_Pagamento
                benLiq.IdSede = beneficiarioLiqWS.ID_Sede
                benLiq.ImponibileIrpef = beneficiarioLiqWS.Imponibile_Irpef
                benLiq.ImponibilePrevidenziale = beneficiarioLiqWS.Imponibile_Previdenziale
                benLiq.ImportoPagato = beneficiarioLiqWS.Importo_Pagato
                benLiq.ImportoSpettante = beneficiarioLiqWS.Importo_Spettante
                'benLiq.NLiquidazione --- viene riempito alla registrazione della liq, in ragioneria.
                'benLiq.NMandato --- viene riempito alla registrazione, in ragioneria.
                benLiq.Operatore = liquidazione.Dli_Operatore
                benLiq.RitenuteIrpef = beneficiarioLiqWS.Ritenute_Irpef
                benLiq.RitenutePrevidenzialiBen = beneficiarioLiqWS.Ritenute_Previdenziali_Ben
                benLiq.RitenutePrevidenzialiEnte = beneficiarioLiqWS.Ritenute_Previdenziali_Ente
                benLiq.SedeComune = beneficiarioLiqWS.Sede_Comune
                benLiq.SedeProvincia = beneficiarioLiqWS.Sede_Provincia
                benLiq.SedeVia = beneficiarioLiqWS.Sede_Via
                benLiq.StampaAvviso = beneficiarioLiqWS.Stampa_Avviso
                benLiq.IsDatoSensibile = beneficiarioLiqWS.IsDatoSensibile


                If Not beneficiarioLiqWS.Contratto Is Nothing Then
                    If Not beneficiarioLiqWS.Contratto.Id_Contratto Is Nothing Then
                        If Not beneficiarioLiqWS.Contratto.Id_Contratto.Trim() = String.Empty Then
                            Dim contratto As DllDocumentale.ItemContrattoInfo = contratti.Item(beneficiarioLiqWS.Contratto.Id_Contratto)
                            If Not contratto Is Nothing Then

                                contratto.IdDocumento = codDocumento
                                benLiq.IdDocumento = contratto.IdDocumento
                                benLiq.IdContratto = contratto.IdContratto
                                benLiq.NumeroRepertorioContratto = contratto.NumeroRepertorioContratto
                                benLiq.Cig = contratto.CodieCIG
                                benLiq.Cup = contratto.CodieCUP

                                'Try
                                '    'dllDoc.FO_Insert_Contratto(contratto)
                                'Catch ex As System.Data.SqlClient.SqlException
                                '    ' ex.Number = 2627 --> codice errore sql chiave duplicata
                                '    If Not ex.Number = 2627 Then
                                '        Throw New Exception(ex.Message)
                                '    End If
                                'End Try
                                If Not beneficiarioLiqWS.Contratto.Lista_Fatture Is Nothing AndAlso beneficiarioLiqWS.Contratto.Lista_Fatture.Count > 0 Then
                                    benLiq.ListaFatture = New Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)
                                    For Each fatturaWS As Fattura_Beneficiario_Types In beneficiarioLiqWS.Contratto.Lista_Fatture
                                        Dim fatturaBenLiq As New DllDocumentale.ItemFatturaInfoHeader
                                        fatturaBenLiq.IdDocumento = codDocumento
                                        fatturaBenLiq.IdUnivoco = fatturaWS.Id_Fatture_SIC
                                        fatturaBenLiq.ImportoFattDaLiquidare = fatturaWS.Importo_Parziale

                                        benLiq.ListaFatture.Add(fatturaBenLiq)
                                    Next
                                End If
                            Else
                                Throw New Exception("Impossibile recuperare le informazioni sul contratto con id '" + beneficiarioLiqWS.Contratto.Id_Contratto + "'")
                            End If
                        Else
                            Throw New Exception("Impossibile recuperare le informazioni sul contratto con id vuoto")
                        End If

                    End If

                End If

                benLiq.IsDatoSensibile = beneficiarioLiqWS.IsDatoSensibile


                liquidazione.ListaBeneficiari.Add(benLiq)

            Next

        End If

    End Sub

    Private Sub associaBeneficiariImpegno(ByVal impegno As DllDocumentale.ItemImpegnoInfo, ByVal beneficiarioImpegnoWS As Beneficiario_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), ByVal dllDoc As DllDocumentale.svrDocumenti, ByVal codDocumento As String)

        If Not impegno.ListaBeneficiari Is Nothing Then
            impegno.ListaBeneficiari.Clear()
        Else
            impegno.ListaBeneficiari = New Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        End If

        If Not beneficiarioImpegnoWS Is Nothing Then

            Dim benLiq As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo

            benLiq.AddizionaleComunale = beneficiarioImpegnoWS.Addizionale_Comunale
            benLiq.AddizionaleRegionale = beneficiarioImpegnoWS.Addizionale_Regionale
            benLiq.AltreRitenute = beneficiarioImpegnoWS.Altre_Ritenute
            benLiq.Bollo = beneficiarioImpegnoWS.Bollo

            If (beneficiarioImpegnoWS.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale) Then
                benLiq.CodiceFiscale = beneficiarioImpegnoWS.Dati_Fiscali.Item
                benLiq.PartitaIva = String.Empty
            ElseIf (beneficiarioImpegnoWS.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva) Then
                benLiq.PartitaIva = beneficiarioImpegnoWS.Dati_Fiscali.Item
                benLiq.CodiceFiscale = String.Empty
            End If
            benLiq.CodiceSiope = beneficiarioImpegnoWS.Codice_Siope
            benLiq.CodiceSiopeAggiuntivo = beneficiarioImpegnoWS.Codice_Siope_Aggiuntivo
            benLiq.Cig = beneficiarioImpegnoWS.Cod_Cig
            benLiq.Cup = beneficiarioImpegnoWS.Cod_Cup
            benLiq.DataRegistrazione = Now
            benLiq.Denominazione = beneficiarioImpegnoWS.Denominazione
            benLiq.EsenzCommBonifico = beneficiarioImpegnoWS.Esenz_Comm_Bonifico
            benLiq.FlagPersonaFisica = beneficiarioImpegnoWS.Flag_Persona_Fisica
            benLiq.Iban = beneficiarioImpegnoWS.IBAN
            benLiq.IdAnagrafica = beneficiarioImpegnoWS.ID_Anagrafica
            benLiq.IdConto = beneficiarioImpegnoWS.ID_Conto
            benLiq.IdDocumento = impegno.Dli_Documento
            benLiq.IdModalitaPag = beneficiarioImpegnoWS.ID_Modalita_Pagamento
            benLiq.IdSede = beneficiarioImpegnoWS.ID_Sede
            benLiq.ImponibileIrpef = beneficiarioImpegnoWS.Imponibile_Irpef
            benLiq.ImponibilePrevidenziale = beneficiarioImpegnoWS.Imponibile_Previdenziale
            benLiq.ImportoPagato = beneficiarioImpegnoWS.Importo_Pagato
            benLiq.ImportoSpettante = beneficiarioImpegnoWS.Importo_Spettante
            'benLiq.NLiquidazione --- viene riempito alla registrazione della liq, in ragioneria.
            'benLiq.NMandato --- viene riempito alla registrazione, in ragioneria.
            benLiq.Operatore = impegno.Dli_Operatore
            benLiq.RitenuteIrpef = beneficiarioImpegnoWS.Ritenute_Irpef
            benLiq.RitenutePrevidenzialiBen = beneficiarioImpegnoWS.Ritenute_Previdenziali_Ben
            benLiq.RitenutePrevidenzialiEnte = beneficiarioImpegnoWS.Ritenute_Previdenziali_Ente
            benLiq.SedeComune = beneficiarioImpegnoWS.Sede_Comune
            benLiq.SedeProvincia = beneficiarioImpegnoWS.Sede_Provincia
            benLiq.SedeVia = beneficiarioImpegnoWS.Sede_Via
            benLiq.StampaAvviso = beneficiarioImpegnoWS.Stampa_Avviso
            benLiq.IsDatoSensibile = beneficiarioImpegnoWS.IsDatoSensibile


            If Not beneficiarioImpegnoWS.Contratto Is Nothing Then
                If Not beneficiarioImpegnoWS.Contratto.Id_Contratto Is Nothing Then
                    If Not beneficiarioImpegnoWS.Contratto.Id_Contratto.Trim() = String.Empty Then
                        Dim contratto As DllDocumentale.ItemContrattoInfo = contratti.Item(beneficiarioImpegnoWS.Contratto.Id_Contratto)
                        If Not contratto Is Nothing Then

                            contratto.IdDocumento = codDocumento
                            benLiq.IdDocumento = contratto.IdDocumento
                            benLiq.IdContratto = contratto.IdContratto
                            benLiq.NumeroRepertorioContratto = contratto.NumeroRepertorioContratto
                            benLiq.Cig = contratto.CodieCIG
                            benLiq.Cup = contratto.CodieCUP

                            'Try
                            '    'dllDoc.FO_Insert_Contratto(contratto)
                            'Catch ex As System.Data.SqlClient.SqlException
                            '    ' ex.Number = 2627 --> codice errore sql chiave duplicata
                            '    If Not ex.Number = 2627 Then
                            '        Throw New Exception(ex.Message)
                            '    End If
                            'End Try
                            If Not beneficiarioImpegnoWS.Contratto.Lista_Fatture Is Nothing AndAlso beneficiarioImpegnoWS.Contratto.Lista_Fatture.Count > 0 Then
                                benLiq.ListaFatture = New Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)
                                For Each fatturaWS As Fattura_Beneficiario_Types In beneficiarioImpegnoWS.Contratto.Lista_Fatture
                                    Dim fatturaBenLiq As New DllDocumentale.ItemFatturaInfoHeader
                                    fatturaBenLiq.IdDocumento = codDocumento
                                    fatturaBenLiq.IdUnivoco = fatturaWS.Id_Fatture_SIC
                                    fatturaBenLiq.ImportoFattDaLiquidare = fatturaWS.Importo_Parziale

                                    benLiq.ListaFatture.Add(fatturaBenLiq)
                                Next
                            End If
                        Else
                            Throw New Exception("Impossibile recuperare le informazioni sul contratto con id '" + beneficiarioImpegnoWS.Contratto.Id_Contratto + "'")
                        End If
                    Else
                        Throw New Exception("Impossibile recuperare le informazioni sul contratto con id vuoto")
                    End If

                End If

            End If

            benLiq.IsDatoSensibile = beneficiarioImpegnoWS.IsDatoSensibile


            impegno.ListaBeneficiari.Add(benLiq)



        End If

    End Sub

    Private Sub associaBeneficiariLiquidazioneWS(ByVal liquidazione As Liquidazione_Types, ByVal beneficiariLiquidazione As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo), ByVal listaFatture As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader), ByVal docDll As DllDocumentale.svrDocumenti)
        Dim listaBeneficiari_WS As New List(Of Beneficiario_Types)


        If Not beneficiariLiquidazione Is Nothing Then
            For Each beneficiarioLiq As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In beneficiariLiquidazione
                Dim benLiqWS As New Beneficiario_Types

                benLiqWS.Addizionale_Comunale = beneficiarioLiq.AddizionaleComunale
                benLiqWS.Addizionale_Regionale = beneficiarioLiq.AddizionaleRegionale
                benLiqWS.Altre_Ritenute = beneficiarioLiq.AltreRitenute
                benLiqWS.Bollo = beneficiarioLiq.Bollo
                benLiqWS.Cod_Cig = beneficiarioLiq.Cig

                Dim datiFiscali As Beneficiario_TypesDati_Fiscali = New Beneficiario_TypesDati_Fiscali()

                If beneficiarioLiq.FlagPersonaFisica Then
                    datiFiscali.ItemElementName = ItemChoiceType.Codice_Fiscale
                    datiFiscali.Item = beneficiarioLiq.CodiceFiscale
                Else
                    datiFiscali.ItemElementName = ItemChoiceType.Partita_Iva
                    datiFiscali.Item = beneficiarioLiq.PartitaIva
                End If

                benLiqWS.Dati_Fiscali = datiFiscali

                benLiqWS.Codice_Siope = beneficiarioLiq.CodiceSiope
                benLiqWS.Codice_Siope_Aggiuntivo = beneficiarioLiq.CodiceSiopeAggiuntivo
                benLiqWS.Cod_Cup = beneficiarioLiq.Cup
                benLiqWS.Denominazione = beneficiarioLiq.Denominazione
                benLiqWS.Esenz_Comm_Bonifico = beneficiarioLiq.EsenzCommBonifico
                benLiqWS.Flag_Persona_Fisica = beneficiarioLiq.FlagPersonaFisica
                benLiqWS.IBAN = beneficiarioLiq.Iban
                benLiqWS.ID_Anagrafica = beneficiarioLiq.IdAnagrafica
                benLiqWS.ID_Conto = beneficiarioLiq.IdConto
                benLiqWS.ID_Modalita_Pagamento = beneficiarioLiq.IdModalitaPag
                benLiqWS.ID_Sede = beneficiarioLiq.IdSede
                benLiqWS.Imponibile_Irpef = beneficiarioLiq.ImponibileIrpef
                benLiqWS.Imponibile_Previdenziale = beneficiarioLiq.ImponibilePrevidenziale
                benLiqWS.Importo_Pagato = beneficiarioLiq.ImportoPagato
                benLiqWS.Importo_Spettante = beneficiarioLiq.ImportoSpettante
                benLiqWS.Ritenute_Irpef = beneficiarioLiq.RitenuteIrpef
                benLiqWS.Ritenute_Previdenziali_Ben = beneficiarioLiq.RitenutePrevidenzialiBen
                benLiqWS.Ritenute_Previdenziali_Ente = beneficiarioLiq.RitenutePrevidenzialiEnte
                benLiqWS.Sede_Comune = beneficiarioLiq.SedeComune
                benLiqWS.Sede_Provincia = beneficiarioLiq.SedeProvincia
                benLiqWS.Sede_Via = beneficiarioLiq.SedeVia
                benLiqWS.Stampa_Avviso = beneficiarioLiq.StampaAvviso

                benLiqWS.Contratto = New Contratto_Beneficiario_Types()
                benLiqWS.Contratto.Id_Contratto = beneficiarioLiq.IdContratto

                benLiqWS.IsDatoSensibile = beneficiarioLiq.IsDatoSensibile

                Dim listaFatture_WS As New List(Of Fattura_Beneficiario_Types)
                For Each fattureLiq As DllDocumentale.ItemFatturaInfoHeader In listaFatture
                    Dim fatturaWS As New Fattura_Beneficiario_Types
                    fatturaWS.Id_Fatture_SIC = fattureLiq.IdUnivoco
                    'fatturaWS.Numero_Fattura_Beneficiario = fattureLiq.NumeroFatturaBeneficiario
                    'fatturaWS.Data_Fattura_Beneficiario = fattureLiq.DataFatturaBeneficiario
                    'fatturaWS.Descrizione_Fattura = fattureLiq.DescrizioneFattura
                    'fatturaWS.Importo_Totale_Fattura = fattureLiq.ImportoTotaleFattura
                    fatturaWS.Importo_Parziale = fattureLiq.ImportoFattDaLiquidare


                    fattureLiq.ListaAllegati = docDll.FO_Get_ListaAllegatiFattura(fattureLiq.Prog)

                    Dim listaAllegati_Fatture_WS As New List(Of Allegato_Fattura_Types)
                    For Each allegatiFattura As DllDocumentale.ItemFatturaAllegato In fattureLiq.ListaAllegati
                        Dim allegatoFattWS As New Allegato_Fattura_Types

                        allegatoFattWS.Nome() = allegatiFattura.Nome
                        allegatoFattWS.Formato() = allegatiFattura.Formato
                        allegatoFattWS.Url() = allegatiFattura.Url

                        listaAllegati_Fatture_WS.Add(allegatoFattWS)
                    Next
                    listaFatture_WS.Add(fatturaWS)
                    'fatturaWS.Lista_Allegati_Fattura = listaAllegati_Fatture_WS.ToArray
                Next

                benLiqWS.Contratto.Lista_Fatture = listaFatture_WS.ToArray
                listaBeneficiari_WS.Add(benLiqWS)
            Next
        End If

        liquidazione.Lista_Beneficiari = listaBeneficiari_WS.ToArray
    End Sub

    Private Function validateBeneficiarioFattureInfo(ByVal beneficiario As Beneficiario_Types, ByVal contratti As Dictionary(Of String, DllDocumentale.ItemContrattoInfo), Optional ByVal enableIDContoCheck As Boolean = False, Optional ByVal oOperatore As DllAmbiente.Operatore = Nothing) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If isStringNullOrEmpty(beneficiario.ID_Anagrafica) Then
            retValue = CreaEccezioneGenerica("Campo ID_Anagrafica non specificato o vuoto", "E' necessario indicare un codice modalità pagamento valido", 9998)
        ElseIf isStringNullOrEmpty(beneficiario.ID_Modalita_Pagamento) Then
            retValue = CreaEccezioneGenerica("Campo ID_Modalita_Pagamento non specificato o vuoto", "E' necessario indicare un codice anagrafica valido", 9998)
        ElseIf isStringNullOrEmpty(beneficiario.ID_Sede) Then
            retValue = CreaEccezioneGenerica("Campo ID_Sede non specificato o vuoto", "E' necessario indicare un codice sede valido", 9998)
        End If

        If retValue Is Nothing AndAlso enableIDContoCheck AndAlso Not oOperatore Is Nothing Then
            Dim oDllDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            Dim modalitaPagamento As Generic.List(Of DllDocumentale.TipoPagamentoInfo) = oDllDocumenti.GetTipologiePagamentoSIC(beneficiario.ID_Modalita_Pagamento)
            If modalitaPagamento.Count > 0 Then
                If modalitaPagamento(0).ObbligoIBAN OrElse modalitaPagamento(0).ObbligoCC Then
                    If String.IsNullOrEmpty(beneficiario.ID_Conto.Trim()) Then
                        retValue = CreaEccezioneGenerica("Campo ID_Conto non specificato o vuoto", "E' necessario indicare un codice conto valido", 9998)
                    End If
                End If
            Else
                retValue = CreaEccezioneGenerica("Errore", "Modalità di pagamento non valida")
            End If
        End If

        If retValue Is Nothing AndAlso Not contratti Is Nothing Then
            If Not beneficiario.Contratto Is Nothing Then
                If Not beneficiario.Contratto.Id_Contratto Is Nothing AndAlso Not beneficiario.Contratto.Id_Contratto.Trim() = String.Empty Then
                    If contratti Is Nothing OrElse contratti.Count = 0 Then
                        retValue = CreaEccezioneGenerica("E' stato specificato un Id_Contratto per il beneficiario mentre nessun contratto è stato associato al provvedimento", "Se previsto, è necessario associare l'Id_Contratto specificato per il beneficiario al provvedimento")
                    Else
                        If Not contratti.ContainsKey(beneficiario.Contratto.Id_Contratto) Then
                            retValue = CreaEccezioneGenerica("Id_Contratto specificato per il beneficiario non presente tra quelli associati al provvedimento", "E' necessario indicare un Id_Contratto tra quelli associati al provvedimento")
                        End If
                    End If
                Else
                    retValue = CreaEccezioneGenerica("Id_Contratto vuoto o non specificato", "E' necessario indicare un Id_Contratto tra quelli associati al provvedimento")
                End If
            End If
        End If
        'If retValue Is Nothing AndAlso Not beneficiario.Lista_Fatture Is Nothing AndAlso beneficiario.Lista_Fatture.Count > 0 Then
        '    If Not beneficiario.Contratto Is Nothing Then
        '        If Not beneficiario.Contratto.Id_Contratto Is Nothing AndAlso Not beneficiario.Contratto.Id_Contratto.Trim() = String.Empty Then
        '            Dim fattureBeneficiario As Fattura_Types() = beneficiario.Lista_Fatture
        '            retValue = validateFatture(fattureBeneficiario, contratti)
        '        End If
        '    Else
        '        retValue = CreaEccezioneGenerica("Contratto vuoto o non specificato", "E' necessario indicare almeno un contratto")
        '    End If
        'End If
        Return retValue
    End Function
    Private Function validateFatturaInfo(ByVal fattura As Fattura_Types, Optional ByVal oOperatore As DllAmbiente.Operatore = Nothing) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If isStringNullOrEmpty(fattura.Id_Fatture_SIC) Then
            retValue = CreaEccezioneGenerica("Campo Id_Fatture_SIC non specificato o vuoto", "E' necessario indicare un codice fatture SIC valido", 9998)
        ElseIf isStringNullOrEmpty(fattura.Numero_Fattura_Beneficiario) Then
            retValue = CreaEccezioneGenerica("Campo Numero_Fattura_Beneficiario non specificato o vuoto", "E' necessario indicare un numero fattura beneficiario valido", 9998)
        ElseIf isStringNullOrEmpty(fattura.Data_Fattura_Beneficiario) Then
            retValue = CreaEccezioneGenerica("Campo Data_Fattura_Beneficiario non specificato o vuoto", "E' necessario indicare una descrizione fattura valido", 9998)
        End If


        Return retValue
    End Function

    Private Function validateBeneficiariFattureLiquidazione(ByVal beneficiariLiquidazioneWS() As Beneficiario_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), Optional ByVal enableIDContoCheck As Boolean = False, Optional ByVal oOperatore As DllAmbiente.Operatore = Nothing) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If Not beneficiariLiquidazioneWS Is Nothing Then
            For Each beneficiarioLiqWS As Beneficiario_Types In beneficiariLiquidazioneWS
                retValue = validateBeneficiarioFattureInfo(beneficiarioLiqWS, contratti, enableIDContoCheck, oOperatore)
                If Not retValue Is Nothing Then
                    Exit For
                End If
            Next
        End If

        Return retValue
    End Function
    Private Function validateFatture(ByVal fattureBeneficiarioWS() As Fattura_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), Optional ByVal oOperatore As DllAmbiente.Operatore = Nothing) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If Not fattureBeneficiarioWS Is Nothing Then
            For Each fatturaBenWS As Fattura_Types In fattureBeneficiarioWS
                retValue = validateFatturaInfo(fatturaBenWS, oOperatore)
                If Not retValue Is Nothing Then
                    Exit For
                End If
            Next
        End If

        Return retValue
    End Function

    Private Sub impostaPrioritàProvvedimento(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, ByVal urgente As Boolean)
        'imposta la priorità del provvedumento
        Dim dllRegistraOp As New DllDocumentale.svrDocumenti(op)
        Dim item As DllDocumentale.Documento_attributo = New DllDocumentale.Documento_attributo
        item.Cod_attributo = "URGENTE"
        item.Doc_id = codDocumento
        item.Valore = urgente
        item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
        dllRegistraOp.FO_Registra_Attributo(item, op)
    End Sub

    Private Function setInfoSchedaTipologiaProvvedimento(ByVal codDocumento As String, ByVal infoTipologiaProvvedimento As Tipologia_Provvedimento_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)) As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo
        Dim retValue As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = Nothing

        If Not infoTipologiaProvvedimento Is Nothing Then
            retValue = New DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo

            'id documento
            retValue.IdDocumento = codDocumento

            'tipologia provvedimento
            Dim idTipologiaProvvedimento As Integer = -1
            If Not isStringNullOrEmpty(infoTipologiaProvvedimento.IdTipologiaProvvedimento) Then
                If Not Integer.TryParse(infoTipologiaProvvedimento.IdTipologiaProvvedimento, idTipologiaProvvedimento) Then
                    idTipologiaProvvedimento = -1
                End If
            End If
            retValue.IdTipologiaProvvedimento = idTipologiaProvvedimento

            'importo spesa prevista
            If (infoTipologiaProvvedimento.ImportoSpesaPrevistaSpecified) Then
                retValue.ImportoSpesaPrevista = infoTipologiaProvvedimento.ImportoSpesaPrevista
            Else
                retValue.ImportoSpesaPrevista = Nothing
            End If

            If (infoTipologiaProvvedimento.IsSommaAutomaticaSpecified) Then
                retValue.isSommaAutomatica = infoTipologiaProvvedimento.IsSommaAutomatica
            Else
                retValue.isSommaAutomatica = False
            End If


            'destinatari
            Dim destinatari As Generic.List(Of DllDocumentale.ItemDestinatarioInfo) = New Generic.List(Of DllDocumentale.ItemDestinatarioInfo)
            If Not infoTipologiaProvvedimento.Lista_Destinatari Is Nothing Then
                For Each destinatario As Destinatario_Types In infoTipologiaProvvedimento.Lista_Destinatari
                    If Not destinatario Is Nothing Then
                        Dim destinatarioInfo As DllDocumentale.ItemDestinatarioInfo = New DllDocumentale.ItemDestinatarioInfo()

                        destinatarioInfo.IdDocumento = codDocumento
                        destinatarioInfo.IdSIC = destinatario.ID_Anagrafica
                        destinatarioInfo.isPersonaFisica = destinatario.Flag_Persona_Fisica
                        destinatarioInfo.Denominazione = destinatario.Denominazione

                        If destinatario.Flag_Persona_Fisica Then
                            If destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale Then
                                destinatarioInfo.CodiceFiscale = destinatario.Dati_Fiscali.Item
                            End If

                            destinatarioInfo.LuogoNascita = destinatario.Luogo_Nascita
                            destinatarioInfo.DataNascita = destinatario.Data_Nascita
                        Else
                            If destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva Then
                                destinatarioInfo.PartitaIva = destinatario.Dati_Fiscali.Item
                            End If

                            destinatarioInfo.LegaleRappresentante = destinatario.Legale_Rappresentante
                        End If

                        If Not destinatario.Contratto Is Nothing Then
                            Dim contratto As DllDocumentale.ItemContrattoInfo = contratti.Item(destinatario.Contratto.Id_Contratto)
                            If contratto Is Nothing Then
                                destinatarioInfo.IdContratto = contratto.IdContratto
                                destinatarioInfo.NumeroRepertorioContratto = contratto.NumeroRepertorioContratto
                            End If
                        End If

                        destinatarioInfo.isDatoSensibile = destinatario.IsDatoSensibile
                        If (destinatario.ImportoSpettanteSpecified) Then
                            destinatarioInfo.ImportoSpettante = destinatario.ImportoSpettante
                        Else
                            destinatarioInfo.ImportoSpettante = 0
                        End If

                        destinatari.Add(destinatarioInfo)
                    End If
                Next
            End If

            retValue.Destinatari = destinatari
        End If

            Return retValue
    End Function

    Private Function setInfoSchedaLeggeTrasparenza(ByVal codDocumento As String, ByVal infoLeggeTrasparenza As Legge_Trasparenza_Types) As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo
        Dim retValue As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = Nothing

        If Not infoLeggeTrasparenza Is Nothing Then
            retValue = New DllDocumentale.ItemSchedaLeggeTrasparenzaInfo

            retValue.IdDocumento = codDocumento
            retValue.AutorizzazionePubblicazione = infoLeggeTrasparenza.Autorizzazione_Pubblicazione
            retValue.NotePubblicazione = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Note_Pubblicazione), "", infoLeggeTrasparenza.Note_Pubblicazione)
            retValue.NormaAttribuzioneBeneficio = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Norma_Attribuzione_Beneficio), "", infoLeggeTrasparenza.Norma_Attribuzione_Beneficio)
            retValue.UfficioResponsabileProcedimento = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Ufficio_Responsabile_Procedimento), "", infoLeggeTrasparenza.Ufficio_Responsabile_Procedimento)
            retValue.FunzionarioResponsabileProcedimento = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Funzionario_Responsabile_Procedimento), "", infoLeggeTrasparenza.Funzionario_Responsabile_Procedimento)
            retValue.ModalitaIndividuazioneBeneficiario = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Modalita_Individuazione_Beneficiario), "", infoLeggeTrasparenza.Modalita_Individuazione_Beneficiario)
            retValue.ContenutoAtto = IIf(isStringNullOrEmpty(infoLeggeTrasparenza.Contenuto_Atto), "", infoLeggeTrasparenza.Contenuto_Atto)

            'contratti
            Dim contratti As Generic.List(Of DllDocumentale.ItemContrattoInfoHeader) = New Generic.List(Of DllDocumentale.ItemContrattoInfoHeader)
            If Not infoLeggeTrasparenza.Lista_Contratti Is Nothing Then
                For Each contratto As Contratto_Types In infoLeggeTrasparenza.Lista_Contratti
                    If Not contratto Is Nothing Then
                        Dim contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader()
                        contrattoInfoHeader.IdContratto = contratto.Id_Contratto
                        contrattoInfoHeader.IdDocumento = codDocumento

                        contratti.Add(contrattoInfoHeader)
                    End If
                Next
            End If

            retValue.Contratti = contratti
        End If

        Return retValue
    End Function

    Private Function setInfoSchedaContrattiFatture(ByVal codDocumento As String, ByVal infoLeggeTrasparenza As Legge_Trasparenza_Types, ByVal dllDoc As DllDocumentale.svrDocumenti) As DllDocumentale.ItemSchedaContrattiFattureInfo
        Dim retValue As DllDocumentale.ItemSchedaContrattiFattureInfo = Nothing
        If Not infoLeggeTrasparenza Is Nothing Then
            retValue = New DllDocumentale.ItemSchedaContrattiFattureInfo

            'contratti
            Dim contratti As Generic.List(Of DllDocumentale.ItemContrattoInfoHeader) = New Generic.List(Of DllDocumentale.ItemContrattoInfoHeader)
            Dim fatture As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = New Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)
            If Not infoLeggeTrasparenza.Lista_Contratti Is Nothing Then
                For Each contratto As Contratto_Types In infoLeggeTrasparenza.Lista_Contratti
                    If Not contratto Is Nothing Then
                        Dim contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader()
                        contrattoInfoHeader.IdContratto = contratto.Id_Contratto
                        contrattoInfoHeader.IdDocumento = codDocumento
                        contrattoInfoHeader.CodieCIG = contratto.CIG
                        contrattoInfoHeader.CodieCUP = contratto.CUP

                        contratti.Add(contrattoInfoHeader)
                    End If

                    If Not contratto.Lista_Fatture Is Nothing AndAlso contratto.Lista_Fatture.Count > 0 Then


                        For Each fattura As Fattura_Types In contratto.Lista_Fatture
                            Dim fatturaInfoHeader As DllDocumentale.ItemFatturaInfoHeader = New DllDocumentale.ItemFatturaInfoHeader

                            fatturaInfoHeader.IdUnivoco = fattura.Id_Fatture_SIC
                            fatturaInfoHeader.IdDocumento = codDocumento
                            fatturaInfoHeader.Contratto.IdContratto = contratto.Id_Contratto
                            fatturaInfoHeader.Contratto.NumeroRepertorioContratto = contratto.Numero_Repertorio

                            fatturaInfoHeader.NumeroFatturaBeneficiario = fattura.Numero_Fattura_Beneficiario
                            fatturaInfoHeader.DataFatturaBeneficiario = fattura.Data_Fattura_Beneficiario
                            fatturaInfoHeader.AnagraficaInfo.IdAnagrafica = fattura.Id_Anagrafica
                            fatturaInfoHeader.AnagraficaInfo.Denominazione = fattura.Denominazione_Beneficiario
                            fatturaInfoHeader.AnagraficaInfo.FlagPersonaFisica = fattura.Flag_Persona_Fisica

                            If (fattura.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale) Then
                                fatturaInfoHeader.AnagraficaInfo.CodiceFiscale = fattura.Dati_Fiscali.Item
                                fatturaInfoHeader.AnagraficaInfo.PartitaIva = String.Empty
                            ElseIf (fattura.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva) Then
                                fatturaInfoHeader.AnagraficaInfo.PartitaIva = fattura.Dati_Fiscali.Item
                                fatturaInfoHeader.AnagraficaInfo.CodiceFiscale = String.Empty
                            End If


                            fatturaInfoHeader.AnagraficaInfo.IdSede = fattura.Id_Sede
                            fatturaInfoHeader.AnagraficaInfo.SedeVia = fattura.Sede

                            fatturaInfoHeader.AnagraficaInfo.IdModalitaPag = fattura.Id_Modalita_Di_Pagamento

                            Dim listaTipoPagamento As New Generic.List(Of DllDocumentale.TipoPagamentoInfo)
                            listaTipoPagamento = dllDoc.GetTipologiePagamentoSIC(fattura.Id_Modalita_Di_Pagamento)

                            If Not listaTipoPagamento Is Nothing AndAlso listaTipoPagamento.Count = 1 Then
                                fatturaInfoHeader.AnagraficaInfo.DescrizioneModalitaPag = listaTipoPagamento(0).Descrizione
                            End If

                            fatturaInfoHeader.AnagraficaInfo.Iban = fattura.IBAN

                            fatturaInfoHeader.DescrizioneFattura = fattura.Descrizione_Fattura
                            fatturaInfoHeader.ImportoTotaleFattura = fattura.Importo_Totale_Fattura

                            If Not fattura.Lista_Allegati_Fattura Is Nothing AndAlso fattura.Lista_Allegati_Fattura.Count > 0 Then
                                fatturaInfoHeader.ListaAllegati = New Generic.List(Of DllDocumentale.ItemFatturaAllegato)
                                For Each allegatoFattura As Allegato_Fattura_Types In fattura.Lista_Allegati_Fattura

                                    Dim itemFatturaAllegato As DllDocumentale.ItemFatturaAllegato = New DllDocumentale.ItemFatturaAllegato

                                    itemFatturaAllegato.IdDocumento = codDocumento
                                    itemFatturaAllegato.Nome = allegatoFattura.Nome
                                    itemFatturaAllegato.Formato = allegatoFattura.Formato
                                    itemFatturaAllegato.Url = allegatoFattura.Url

                                    fatturaInfoHeader.ListaAllegati.Add(itemFatturaAllegato)
                                Next
                            End If

                            fatture.Add(fatturaInfoHeader)
                        Next
                    End If
                Next
            End If

            retValue.IdDocumento = codDocumento
            retValue.Contratti = contratti
            retValue.Fatture = fatture

        End If


        Return retValue
    End Function

    Private Sub impostaInfoSchedaLeggeTrasparenza(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, ByVal infoLeggeTrasparenza As Legge_Trasparenza_Types, Optional ByVal updateOnly As Boolean = False)
        Dim dllRegistraOp As New DllDocumentale.svrDocumenti(op)

        Dim itemSchedaLeggeTrasparenzaInfo As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = setInfoSchedaLeggeTrasparenza(codDocumento, infoLeggeTrasparenza)
        Dim itemSchedaContrattiFattureInfo As New DllDocumentale.ItemSchedaContrattiFattureInfo
        itemSchedaContrattiFattureInfo.IdDocumento = codDocumento
        itemSchedaContrattiFattureInfo.Contratti = itemSchedaLeggeTrasparenzaInfo.Contratti


        If Not itemSchedaLeggeTrasparenzaInfo Is Nothing Then

            Dim itemSchedaLeggeTrasparenzaInfoDB As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = dllRegistraOp.FO_Get_SchedaLeggeTrasparenzaInfo(op, codDocumento)

            ' Nel caso di chiamata a "ModificaDocumento"
            If updateOnly Then
                dllRegistraOp.FO_Update_Info_Scheda_Legge_Trasparenza(op, itemSchedaLeggeTrasparenzaInfo)
                dllRegistraOp.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(op, itemSchedaLeggeTrasparenzaInfo)




                ' se ci sono contratti, aggiorno lo storico con i nuovi
                If itemSchedaLeggeTrasparenzaInfo.Contratti.Count > 0 Then
                    dllRegistraOp.FO_Insert_Contratto_Storico(op, itemSchedaContrattiFattureInfo)
                ElseIf itemSchedaLeggeTrasparenzaInfoDB.Contratti.Count > 0 Then
                    'se, invece, precedentemente c'erano dei contratti nel db,
                    ' registro che sono stati eliminati tutti gli esistenti
                    ' altrimenti, non inserisco nessun record nello storico, 
                    ' perchè i contratti non sono mai stati inseriti
                    Dim contratto As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader
                    contratto.IdDocumento = itemSchedaLeggeTrasparenzaInfo.IdDocumento
                    contratto.IdContratto = "Rimossi tutti i contratti"
                    itemSchedaLeggeTrasparenzaInfo.Contratti.Add(contratto)
                    dllRegistraOp.FO_Insert_Contratto_Storico(op, itemSchedaContrattiFattureInfo)
                End If

                ' Nel caso di chiamata a "CreaDocumento"
            Else
                dllRegistraOp.FO_Insert_Info_Scheda_Legge_Trasparenza(op, itemSchedaLeggeTrasparenzaInfo)

                dllRegistraOp.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(op, itemSchedaLeggeTrasparenzaInfo)

                If itemSchedaLeggeTrasparenzaInfo.Contratti.Count > 0 Then
                    dllRegistraOp.FO_Insert_Contratto_Storico(op, itemSchedaContrattiFattureInfo)
                End If
            End If
        End If
    End Sub

    Private Sub impostaInfoSchedaContrattiFatture(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, ByVal infoLeggeTrasparenza As Legge_Trasparenza_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), Optional ByVal updateOnly As Boolean = False)
        Dim dllRegistraOp As New DllDocumentale.svrDocumenti(op)
        Dim itemSchedaContrattiFattureInfo = setInfoSchedaContrattiFatture(codDocumento, infoLeggeTrasparenza, dllRegistraOp)

        If updateOnly Then
            dllRegistraOp.FO_Update_Info_Scheda_Contratti_Fatture(op, itemSchedaContrattiFattureInfo)
        Else
            dllRegistraOp.FO_Insert_Info_Scheda_Contratti_Fatture(op, itemSchedaContrattiFattureInfo)
        End If


    End Sub


    Private Sub impostaInfoSchedaTipologiaProvvedimento(ByVal op As DllAmbiente.Operatore, ByVal codDocumento As String, ByVal infoTipologiaProvvedimento As Tipologia_Provvedimento_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo), Optional ByVal updateOnly As Boolean = False)
        Dim dllRegistraOp As New DllDocumentale.svrDocumenti(op)

        Dim itemSchedaTipologiaProvvedimentoInfo As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = setInfoSchedaTipologiaProvvedimento(codDocumento, infoTipologiaProvvedimento, contratti)

        If Not itemSchedaTipologiaProvvedimentoInfo Is Nothing Then
            If updateOnly Then
                dllRegistraOp.FO_Update_Info_Scheda_Tipologia_Provvedimento(op, itemSchedaTipologiaProvvedimentoInfo)
            Else
                dllRegistraOp.FO_Insert_Info_Scheda_Tipologia_Provvedimento(op, itemSchedaTipologiaProvvedimentoInfo)
            End If
        End If
    End Sub

    Private Function validatePrioritàProvvedimento(ByVal op As DllAmbiente.Operatore, ByVal urgente As Boolean)
        Dim retValue As Eccezione_Types = Nothing

        'verifica la possibilità che l'utente possa inoltrare un atto urgente
        If urgente Then
            If Not op.Test_Attributo("SCEGLI_URGENZA", True) Then
                retValue = CreaEccezioneGenerica("Errore", "Utente non abilitato all'inoltro di documenti urgenti")
            End If
        End If

        Return retValue
    End Function

    Private Function validateSchedaLeggeTrasparenzaEschedaContrFattInfo(ByVal operatore As DllAmbiente.Operatore, ByVal infoLeggeTrasparenza As Legge_Trasparenza_Types, ByRef contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If Not infoLeggeTrasparenza Is Nothing Then
            If Not infoLeggeTrasparenza.Autorizzazione_Pubblicazione Then
                If infoLeggeTrasparenza.Note_Pubblicazione Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Note_Pubblicazione non specificato", "E' necessario indicare il motivo della non pubblicazione dell'atto", 9998)
                End If
            Else
                If infoLeggeTrasparenza.Norma_Attribuzione_Beneficio Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Norma_Attribuzione_Beneficio non specificato", "E' necessario indicare la norma per l'attribuzione del beneficio", 9998)
                ElseIf infoLeggeTrasparenza.Ufficio_Responsabile_Procedimento Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Ufficio_Responsabile_Procedimento non specificato", "E' necessario indicare il nome dell'ufficio responsabile del procedimento", 9998)
                ElseIf infoLeggeTrasparenza.Funzionario_Responsabile_Procedimento Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Funzionario_Responsabile_Procedimento non specificato", "E' necessario indicare il nome e cognome del funzionario responsabile del procedimento", 9998)
                ElseIf infoLeggeTrasparenza.Modalita_Individuazione_Beneficiario Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Modalita_Individuazione_Beneficiario non specificato", "E' necessario indicare la modalità di individuazione del beneficiario dell'atto", 9998)
                ElseIf infoLeggeTrasparenza.Contenuto_Atto Is Nothing Then
                    retValue = CreaEccezioneGenerica("Campo Contenuto_Atto non specificato", "E' necessario indicare il riepilogo del contenuto dell'atto", 9998)
                End If
            End If

            'contratti
            If retValue Is Nothing AndAlso Not infoLeggeTrasparenza.Lista_Contratti Is Nothing Then
                Dim idContratti As Generic.List(Of String) = New Generic.List(Of String)
                For Each contratto As Contratto_Types In infoLeggeTrasparenza.Lista_Contratti
                    If contratto.Id_Contratto Is Nothing OrElse contratto.Id_Contratto.Trim() = String.Empty Then
                        retValue = CreaEccezioneGenerica("Campo Id_Contratto non specificato o vuoto", "E' necessario indicare un id di contratto al quale si fa riferimento nell'atto", 9998)
                        Exit For
                    Else
                        If Not idContratti.Contains(contratto.Id_Contratto) Then
                            idContratti.Add(contratto.Id_Contratto)

                            'per ogni contratto indicato, verifico le fatture, se presenti
                            If Not contratto.Lista_Fatture Is Nothing Then
                                For Each fattura As Fattura_Types In contratto.Lista_Fatture
                                    validateFatturaInfo(fattura)
                                Next
                            End If
                        End If
                    End If
                Next

                If retValue Is Nothing Then
                    Dim tmpContratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo) = GetContratti(operatore, idContratti)
                    Dim excludedIdContratti As IEnumerable(Of String) = idContratti.ToArray().Except(tmpContratti.Keys.ToArray())

                    If excludedIdContratti.Count <> 0 Then
                        Dim excludedIdContrattiAsString As String = "["
                        For i As Integer = 0 To excludedIdContratti.Count - 1
                            Dim idContratto As String = excludedIdContratti.ElementAt(i)
                            excludedIdContrattiAsString = excludedIdContrattiAsString + "'" + idContratto + "'"
                            If i <> excludedIdContratti.Count - 1 Then
                                excludedIdContrattiAsString = excludedIdContrattiAsString + ", "
                            End If
                        Next
                        excludedIdContrattiAsString = excludedIdContrattiAsString + "]"
                        retValue = CreaEccezioneGenerica("Sono stati specificati degli 'Id_Contratto' non validi: " + excludedIdContrattiAsString, "E' necessario specificare solo 'Id_Contratto' validi", 9998)
                    Else
                        contratti = tmpContratti
                    End If
                End If
            End If
        Else
            retValue = CreaEccezioneGenerica("Sezione 'Legge Trasparenza' non specificata", "E' necessario specificare la sezione 'Legge Trasparenza'", 9998)
        End If

        Return retValue
    End Function

    Private Function validateSchedaTipologiaProvvedimentoInfo(ByVal operatore As DllAmbiente.Operatore, ByVal infoTipologiaProvvedimento As Tipologia_Provvedimento_Types, ByVal contratti As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing

        If Not infoTipologiaProvvedimento Is Nothing Then
            'tipologia provvedimento            
            retValue = validateTipologiaProvvedimento(infoTipologiaProvvedimento)

            'destinatari
            If retValue Is Nothing AndAlso Not infoTipologiaProvvedimento.Lista_Destinatari Is Nothing Then
                For Each destinatario As Destinatario_Types In infoTipologiaProvvedimento.Lista_Destinatari
                    If destinatario.Denominazione Is Nothing OrElse destinatario.Denominazione.Trim() = String.Empty Then
                        retValue = CreaEccezioneGenerica("Campo Denominazione non specificato o vuoto", "E' necessario specificare la denominazione del destinatario", 9998)
                        Exit For
                    ElseIf destinatario.ID_Anagrafica Is Nothing OrElse destinatario.ID_Anagrafica.Trim() = String.Empty Then
                        retValue = CreaEccezioneGenerica("Campo ID_Anagrafica non specificato o vuoto ", "E' necessario specificre un id anagrafica (codice SIC) del destinatario valido", 9998)
                        Exit For
                    Else
                        If destinatario.Dati_Fiscali Is Nothing Then
                            retValue = CreaEccezioneGenerica("Campo relativo ai dati fiscali non specificato", "E' necessario specificare il codice fiscale o la partita iva del destinatario", 9998)
                            Exit For
                        Else
                            If (destinatario.Flag_Persona_Fisica AndAlso _
                                destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva) Then
                                retValue = CreaEccezioneGenerica("Campo Dati_Fiscali.Codice_Fiscale non specificato", "E' necessario specificare il codice fiscale e non la partita iva se il destinatario è una persona fisica", 9998)
                                Exit For
                            End If
                            If (Not destinatario.Flag_Persona_Fisica AndAlso _
                                destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale) Then
                                retValue = CreaEccezioneGenerica("Campo Dati_Fiscali.Partita_Iva non specificato", "E' necessario specificare la partita iva e non il codice fiscale se il destinatario è una persona giuridica", 9998)
                                Exit For
                            End If
                            If (destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Codice_Fiscale AndAlso _
                               (destinatario.Dati_Fiscali.Item Is Nothing OrElse destinatario.Dati_Fiscali.Item.Trim() = String.Empty)) Then
                                retValue = CreaEccezioneGenerica("Campo Dati_Fiscali.Codice_Fiscale non specificato", "E' necessario specificare il codice fiscale del destinatario", 9998)
                                Exit For
                            End If
                            If (destinatario.Dati_Fiscali.ItemElementName = ItemChoiceType.Partita_Iva AndAlso _
                               (destinatario.Dati_Fiscali.Item Is Nothing OrElse destinatario.Dati_Fiscali.Item.Trim() = String.Empty)) Then
                                retValue = CreaEccezioneGenerica("Campo Dati_Fiscali.Partita_Iva non specificato", "E' necessario specificare la partita iva del destinatario", 9998)
                                Exit For
                            End If
                            If Not destinatario.Contratto Is Nothing Then
                                If Not destinatario.Contratto.Id_Contratto Is Nothing AndAlso Not destinatario.Contratto.Id_Contratto.Trim() = String.Empty Then
                                    If contratti Is Nothing OrElse contratti.Count = 0 Then
                                        retValue = CreaEccezioneGenerica("E' stato specificato un Id_Contratto per il destinatario mentre nessun contratto è stato associato al provvedimento", "Se previsto, è necessario associare l'Id_Contratto specificato per il destinatario al provvedimento")
                                    Else
                                        If Not contratti.ContainsKey(destinatario.Contratto.Id_Contratto) Then
                                            retValue = CreaEccezioneGenerica("Id_Contratto specificato per il destinatario non presente tra quelli associati al provvedimento", "E' necessario indicare un Id_Contratto tra quelli associati al provvedimento")
                                        End If
                                    End If
                                Else
                                    retValue = CreaEccezioneGenerica("Id_Contratto vuoto o non specificato", "E' necessario indicare un Id_Contratto tra quelli associati al provvedimento")
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        Else
            retValue = CreaEccezioneGenerica("Sezione 'Tipologia Provvedimento' non specificata", "E' necessario specificare la sezione 'Tipologia Provvedimento'", 9998)
        End If

        Return retValue
    End Function

    Private Function isStringNullOrEmpty(ByVal value As String, Optional ByVal trimValue As Boolean = True) As Boolean
        Dim retValue As Boolean = False

        If (trimValue) Then
            retValue = value Is Nothing OrElse value.Trim() = String.Empty
        Else
            retValue = value Is Nothing OrElse value = String.Empty
        End If

        Return retValue
    End Function

    Private Function isSchedaLeggeTrasparenzaToBeEnabled(ByVal datiContabili As Dati_Contabili_Types)
        Dim retValue As Boolean = False
        If Not datiContabili Is Nothing Then
            retValue = (Not datiContabili.Impegni Is Nothing AndAlso datiContabili.Impegni.Length > 0)
            retValue = retValue OrElse (Not datiContabili.ImpegniSuPerenti Is Nothing AndAlso datiContabili.ImpegniSuPerenti.Length > 0)
            retValue = retValue OrElse (Not datiContabili.Liquidazioni Is Nothing AndAlso datiContabili.Liquidazioni.Length > 0)
        End If
        Return retValue
    End Function

    Private Function GetContratti(ByVal operatore As DllAmbiente.Operatore, ByVal idContratti As Generic.List(Of String)) As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)
        Dim retValue As Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo) = New Generic.Dictionary(Of String, DllDocumentale.ItemContrattoInfo)

        Try
            Dim rispostaInterrogaContratti As Array = ClientIntegrazioneSic.MessageMaker.createInterrogaContrattiMessage(operatore, idContratti)
            For i As Integer = 0 To UBound(rispostaInterrogaContratti, 1)
                If Not rispostaInterrogaContratti(i) Is Nothing Then
                    Dim itemContrattoInfo As DllDocumentale.ItemContrattoInfo = New DllDocumentale.ItemContrattoInfo

                    itemContrattoInfo.IdContratto = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).Chiave
                    If itemContrattoInfo.IdContratto Is Nothing Then
                        itemContrattoInfo.IdContratto = String.Empty
                    End If

                    itemContrattoInfo.NumeroRepertorioContratto = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).NumeroRepertorio
                    If itemContrattoInfo.NumeroRepertorioContratto Is Nothing Then
                        itemContrattoInfo.NumeroRepertorioContratto = String.Empty
                    End If

                    itemContrattoInfo.OggettoContratto = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).Descrizione
                    If itemContrattoInfo.OggettoContratto Is Nothing Then
                        itemContrattoInfo.OggettoContratto = String.Empty
                    End If

                    itemContrattoInfo.CodieCUP = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCup
                    If itemContrattoInfo.CodieCUP Is Nothing Then
                        itemContrattoInfo.CodieCUP = String.Empty
                    End If
                    itemContrattoInfo.CodieCIG = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCig
                    If itemContrattoInfo.CodieCIG Is Nothing Then
                        itemContrattoInfo.CodieCIG = String.Empty
                    End If

                    retValue.Add(itemContrattoInfo.IdContratto, itemContrattoInfo)
                End If
            Next
        Catch listaVuotaException As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaException.ToString)
        End Try

        Return retValue
    End Function

    Private Function validateTipologiaProvvedimento(ByVal infoTipologiaProvvedimento As Tipologia_Provvedimento_Types) As Eccezione_Types
        Dim retValue As Eccezione_Types = Nothing
        If Not infoTipologiaProvvedimento Is Nothing Then
            Dim idTipologiaProvvedimento As String = infoTipologiaProvvedimento.IdTipologiaProvvedimento
            If Not idTipologiaProvvedimento Is Nothing AndAlso idTipologiaProvvedimento.Trim() <> String.Empty Then
                Dim idTipologiaProvvedimentoAsInteger As Integer = -1
                If Integer.TryParse(idTipologiaProvvedimento, idTipologiaProvvedimentoAsInteger) Then
                    If idTipologiaProvvedimentoAsInteger > -1 Then
                        Dim oDllDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(New DllAmbiente.Operatore())
                        Dim listaTipologieDocumento As Generic.List(Of DllDocumentale.ItemTipologiaDocumento) = oDllDocumenti.FO_GetListaTipologieDocumento(idTipologiaProvvedimentoAsInteger)
                        If listaTipologieDocumento.Count = 0 Then
                            retValue = CreaEccezioneGenerica("Tipologia Provvedimento non definita.", "E' necessario indicare una delle tipologie di provvedimento definite.", 9998)
                        Else
                            Dim destinatariCount As Integer = 0
                            If Not infoTipologiaProvvedimento.Lista_Destinatari Is Nothing Then
                                destinatariCount = infoTipologiaProvvedimento.Lista_Destinatari.Count
                            End If

                            Dim tipologiaDocumento As DllDocumentale.ItemTipologiaDocumento = listaTipologieDocumento.Item(0)
                            If tipologiaDocumento.HasDestinatari AndAlso tipologiaDocumento.HasDestinatariObbligatori AndAlso destinatariCount = 0 Then
                                retValue = CreaEccezioneGenerica("Lista destinatari vuota.", "La tipologia di provvedimento specificata non permette l'associazione di una lista di destinatari vuota.", 9998)
                            End If
                        End If
                    End If
                Else
                    retValue = CreaEccezioneGenerica("Tipologia Provvedimento non valida.", "E' necessario indicare una delle tipologie di provvedimento definite.", 9998)
                End If
            Else
                retValue = CreaEccezioneGenerica("Tipologia Provvedimento vuota o non specificata.", "E' necessario indicare una delle tipologie di provvedimento definite.", 9998)
            End If       
        Else
            retValue = CreaEccezioneGenerica("Sezione 'Tipologia Provvedimento' non specificata", "E' necessario specificare la sezione 'Tipologia Provvedimento'", 9998)
        End If

        Return retValue
    End Function

    Public Function GetTipologieProvvedimento(ByVal request As InserisciMandatiRequest) As InserisciMandatiResponse Implements ProvvedimentiPortType.GetTipologieProvvedimento
        Dim response As New InserisciMandatiResponse

        response.Messaggio_Risposta = New MessaggioRisposta_Types
        response.Messaggio_Risposta.Intestazione = CreaIntestazione()

        Try
            Dim eccez As Eccezione_Types = CreaEccezionePerTipoRichiesta(request.Messaggio_Richiesta.Richiesta.Item.GetType, GetType(GetTipologieProvvedimento_Types))

            If Not eccez Is Nothing Then
                response.Messaggio_Risposta.Item = eccez
                Return response
            End If

            Dim objRichiestaInfo As GetTipologieProvvedimento_Types = request.Messaggio_Richiesta.Richiesta.Item

            Dim tipologieProvvedimento As Generic.List(Of TipologiaProvvedimento_Types) = New Generic.List(Of TipologiaProvvedimento_Types)

            Dim oDllDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(New DllAmbiente.Operatore())
            Dim listaTipologieDocumento As Generic.List(Of DllDocumentale.ItemTipologiaDocumento) = oDllDocumenti.FO_GetListaTipologieDocumento()

            For Each itemTipologiaDocumento As DllDocumentale.ItemTipologiaDocumento In listaTipologieDocumento
                Dim tipologiaProvvedimento As TipologiaProvvedimento_Types = New TipologiaProvvedimento_Types()

                tipologiaProvvedimento.id = itemTipologiaDocumento.Id
                tipologiaProvvedimento.descrizione = itemTipologiaDocumento.Tipologia

                tipologieProvvedimento.Add(tipologiaProvvedimento)
            Next

            Dim risposta_GetTipologieProvvedimento As New Risposta_GetTipologieProvvedimento_Types
            risposta_GetTipologieProvvedimento.tipologie = tipologieProvvedimento.ToArray

            Dim succ As New Successo_Types
            succ.Item = risposta_GetTipologieProvvedimento

            response.Messaggio_Risposta.Item = succ
        Catch ex As Exception
            Log.Error("Get Tipologie Provvedimento" & ex.Message)
            response.Messaggio_Risposta.Item = CreaEccezioneGenerica(ex.Message, ex.Message)
        End Try

        Return response
    End Function
End Class

