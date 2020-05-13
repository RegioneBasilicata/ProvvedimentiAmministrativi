Imports System.IO
Imports Microsoft.Office
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Xml
Imports DigitalSignatureUtils
Imports ClientIntegrazioneSic.Intema.WS.Risposta

Module ElencoFunzioni
    Private Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(ElencoFunzioni))
    Public Function Registra_Documento(ByVal codDocumento As String, Optional ByRef DocOggetto As String = Nothing, Optional ByRef DocTesto As String = Nothing, Optional ByRef pubIntegrale As Integer = 0, Optional ByVal fileTestoDetermina As Byte() = Nothing, Optional ByVal isContabile As Integer = 0, Optional ByVal tipoOpContabili As String = "") As Object
        Dim oDll As DllDocumentale.svrDocumenti  '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Documento ' Dll.Dic_FO.cfo_
        Dim dimvc_Funzione As Integer = DllDocumentale.Dic_FODocumentale.dimvc_Registra_Documento
        Dim vFunzione(dimvc_Funzione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            vRit(0) = 0
            vRit(1) = ""

            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDll = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_doc_Oggetto) = DocOggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_testo) = DocTesto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_utente) = oOperatore.Codice
            'vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_xmlDatiDocumento) = datiXml
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_pub_integrale) = pubIntegrale
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_isContabile) = isContabile
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Documento.c_tipoOpContabili) = tipoOpContabili

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDll.Elabora(cFunzione, vParam)

            vRit(0) = vr(0)
            vRit(1) = vr(1)

            oDll = Nothing
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Registra_Documento = vRit
        End Try
    End Function

    Public Function Seduta_Giunta(Optional ByVal dataSeduta As String = "") As Object
        Dim oDll As DllDocumentale.svrDelibereBase  '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Seduta_Giunta ' Dll.Dic_FO.cfo_
        Dim dimvc_Funzione As Integer = DllDocumentale.Dic_FODocumentale.dimvc_Seduta_Giunta
        Dim vFunzione(dimvc_Funzione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            vRit(0) = 0
            vRit(1) = ""

            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            '    oDll = New DllDocumentale.svrDelibere(oOperatore)
            oDll = DllDocumentale.AbstractSvrDocumenti.getSvrDelibere(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Seduta_Giunta.c_attesa) = 1
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Seduta_Giunta.c_data) = dataSeduta

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDll.Elabora(cFunzione, vParam)

            vRit(0) = vr(0)
            vRit(1) = vr(1)

            oDll = Nothing
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Seduta_Giunta = vRit
        End Try
    End Function

    'Questa funzione viene chiamata in Worklist
    Public Function Elenco_Documenti(ByVal tipoDoc As Integer,
                                     Optional ByVal tipoData As Integer = -1,
                                     Optional ByVal dataInizio As String = "",
                                     Optional ByVal dataFine As String = "",
                                     Optional ByVal oggetto As String = "",
                                     Optional ByVal ufficio As String = "",
                                     Optional ByVal dipartimento As String = "",
                                     Optional ByVal numero As String = "",
                                     Optional ByVal tipoRigetto As String = "",
                                     Optional ByVal idDocumento As String = "",
                                     Optional ByVal tipologiaRicercaBeneficiario As String = "",
                                     Optional ByVal beneficiario As String = "",
                                     Optional ByVal codiceCUP As String = "",
                                     Optional ByVal codiceCIG As String = "",
                                     Optional ByVal visualizzaUrgenti As Boolean = False,
                                     Optional ByVal visualizzaNonTrasp As Boolean = False,
                                     Optional ByVal idTipologiaDocumento As Integer = -1,
                                     Optional ByVal autorizzazionePubblicazione As String = "",
                                     Optional ByVal tipologiaRicercaDestinatario As String = "",
                                     Optional ByVal destinatario As String = "") As Object

        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Documenti
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Documenti) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try

            If numero <> "" Then
                dataInizio = ""
                dataFine = ""
                oggetto = ""
            End If

            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente) = oOperatore.Codice  'idoperatore
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento) = tipoDoc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio) = dataInizio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine) = dataFine
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_oggetto) = oggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_ufficio) = ufficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_dip) = dipartimento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_numero_doc) = numero
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_Rigetto) = tipoRigetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_idDocumento) = idDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipologia_ricerca_beneficiario) = tipologiaRicercaBeneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_beneficiario) = beneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_codiceCUP) = codiceCUP
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_codiceCIG) = codiceCIG
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_visualizzaUrgenti) = visualizzaUrgenti
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_visualizzaNonTrasp) = visualizzaNonTrasp
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_id_tipologia_documento) = idTipologiaDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_autorizzazione_pubblicazione) = autorizzazionePubblicazione
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipologia_ricerca_destinatario) = tipologiaRicercaDestinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_destinatario) = destinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_data) = tipoData
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
            Elenco_Documenti = vRit
        End Try


    End Function

    'Questa funzione viene chiamata in Deposito
    Public Function Elenco_Deposito(ByVal tipoDoc As Integer,
                                     Optional ByVal dataInizio As String = "",
                                     Optional ByVal dataFine As String = "",
                                     Optional ByVal oggetto As String = "",
                                     Optional ByVal ufficio As String = "",
                                     Optional ByVal dipartimento As String = "",
                                     Optional ByVal numero As String = "",
                                     Optional ByVal visualizzaDepositoUrgenti As Boolean = False) As Object

        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Documenti
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Documenti) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento) = tipoDoc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio) = dataInizio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine) = dataFine
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_oggetto) = oggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_ufficio) = ufficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_dip) = dipartimento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_numero_doc) = numero
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_visualizzaUrgenti) = visualizzaDepositoUrgenti
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
            Elenco_Deposito = vRit
        End Try


    End Function

    'Questa funzione viene chiamata in Prelazione
    Public Function Elenco_DocumentiUfficio(ByVal tipoApplic As Integer) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_DocumentiUfficio
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_DocumentiUfficio) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try


            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_DocumentiUfficio.c_tipoDocumento) = tipoApplic


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
            Elenco_DocumentiUfficio = vRit
        End Try
    End Function

    Public Function Leggi_Stato_Ufficio(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim vFunzione(1) As Object
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
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(0) = codDocumento
            vFunzione(1) = oOperatore.oUfficio.CodUfficio

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Verifica_Ultima_Azione_Ufficio, vParam)


            vRit(0) = vr(0)
            vRit(1) = vr(1)


            oDllDocumenti = Nothing
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Leggi_Stato_Ufficio = vRit
        End Try
    End Function

    Function VerificaAbilitazioneInoltroRigetto(ByVal ooperatore As DllAmbiente.Operatore, ByVal idDocumento As String, ByVal codAzione As String) As Boolean
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(ooperatore)
        Return dllDoc.VerificaAbilitazioneInoltroRigetto(idDocumento, ooperatore, codAzione)
    End Function


    Public Function Leggi_Osservazioni_Documento(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Osservazioni_Documento ' Dll.Dic_FO.cfo_
        Dim dimvc_Funzione As Integer = DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Osservazioni_Documento
        Dim vFunzione(dimvc_Funzione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object



        Try

            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Osservazioni_Documento.c_idDocumento) = codDocumento


            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)


            vRit(0) = vr(0)
            vRit(1) = vr(1)



            oDllDocumenti = Nothing
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Leggi_Osservazioni_Documento = vRit
        End Try
    End Function

    Public Function Leggi_Documento(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Leggi_Documento ' Dll.Dic_FO.cfo_
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
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_idDocumento) = codDocumento
            'vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_xmlDatiDocumento) = xmlDati

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            vRit(0) = vr(0)
            vRit(1) = vr(1)


            vRit(2) = oDllDocumenti.objDocumento.Doc_Cod_Uff_Prop
            vRit(3) = oDllDocumenti.objDocumento.Doc_Descrizione_ufficio
            
            Dim str As String = vRit(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto)

            str = str.Replace("&amp;", Chr(38))
            str = str.Replace("&aps;", Chr(39))
            str = str.Replace("&gt;", Chr(62))
            str = str.Replace("&lt;", Chr(60))
            str = str.Replace("&quot;", Chr(34))

            vRit(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto) = str

            oDllDocumenti = Nothing
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Leggi_Documento = vRit
        End Try
    End Function

    Public Function Leggi_Documento_Object(ByVal codDocumento As String, Optional ByRef xmlDati As String = Nothing) As DllDocumentale.Model.DocumentoInfo

        Dim docItem As DllDocumentale.Model.DocumentoInfo

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
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Documento.c_xmlDatiDocumento) = xmlDati

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
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
            docItem = Nothing
        Finally
            Leggi_Documento_Object = docItem
        End Try
    End Function

    Public Function Crea_Determina() As Object
        'Dim oDllDetermine As DllDocumentale.svrDetermine
        Dim oDllDetermine As DllDocumentale.svrDetermineBase
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_Determina) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
            'oDllDetermine = New DllDocumentale.svrDetermine(oOperatore)
            'Lu 05/02/2010 test ereditarietà
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

    Public Function Crea_Delibera() As Object
        Dim oDllDelibere As DllDocumentale.svrDelibereBase
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_Delibera) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            'oDllDelibere = New DllDocumentale.svrDelibere(oOperatore)
            oDllDelibere = DllDocumentale.AbstractSvrDocumenti.getSvrDelibere(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Delibera.c_cod_ufficio_proponente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Delibera.c_utente_creazione) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Delibera.c_data_creazione) = Format(Now, "dd/MM/yyyy")

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDelibere.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Crea_Delibera, vParam)

            oDllDelibere = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Crea_Delibera = vRit
        End Try

    End Function

    Public Function Crea_Disposizione() As Object
        Dim oDllDisposizioni As DllDocumentale.svrDisposizioniBase
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_Disposizione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            '  oDllDetermine = New DllDocumentale.svrDisposizioni(oOperatore)
            'Lu 05/02/2010 test ereditarietà
            oDllDisposizioni = DllDocumentale.AbstractSvrDocumenti.getSvrDisposizioni(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_cod_ufficio_proponente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_utente_creazione) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_Disposizione.c_data_creazione) = Format(Now, "dd/MM/yyyy")

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDisposizioni.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Crea_Disposizione, vParam)

            oDllDisposizioni = Nothing
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

    Public Function Crea_AltroAtto(ByVal tipoAtto As Integer) As Object
        Dim oDllDocumenti As DllDocumentale.svrAltriAttiBase
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Crea_AltroAtto) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            '  oDllDetermine = New DllDocumentale.svrDisposizioni(oOperatore)
            'Lu 05/02/2010 test ereditarietà
            oDllDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrAltriAtti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_AltroAtto.c_cod_ufficio_proponente) = oOperatore.oUfficio.CodUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_AltroAtto.c_utente_creazione) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_AltroAtto.c_data_creazione) = Format(Now, "dd/MM/yyyy")
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Crea_AltroAtto.c_tipo_atto) = tipoAtto

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(DllDocumentale.Dic_FODocumentale.cfo_Crea_AltroAtto, vParam)

            oDllDocumenti = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Crea_AltroAtto = vRit
        End Try

    End Function

    Public Function Inoltra_Determina(ByVal codDocumento As String, ByVal codAzione As String, Optional ByVal prossimoAttore As String = "", Optional ByVal note As String = "", Optional ByVal suggerimento As Integer = -1, Optional ByVal destinatarioInoltro As Integer = -1, Optional ByVal flagUrgente As Boolean = False) As Object
        Dim oDllDetermine As DllDocumentale.svrDetermineBase
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Passo_Determina
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Passo_Determina) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing

        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        ' oDllDetermine = New DllDocumentale.svrDetermine(oOperatore)
        oDllDetermine = DllDocumentale.AbstractSvrDocumenti.getSvrDetermine(oOperatore)

        If prossimoAttore = "ARCHIVIO" Then
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_utente) = oDllDetermine.getUtenteArchivio()
            System.Web.HttpContext.Current.Session.Remove("prossimoAttore")
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_prossimoAttore) = Nothing
        Else
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_utente) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_prossimoAttore) = prossimoAttore
        End If

        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_idDocumento) = codDocumento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_ufficioUtente) = oOperatore.oUfficio.CodUfficio
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_azione) = codAzione
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_note) = note
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_suggerimento) = suggerimento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Determina.c_flagUrgente) = flagUrgente


        vClient(0) = ""
        vClient(1) = ""
        vClient(2) = oOperatore.Codice
        vClient(3) = ""

        vParam(0) = vClient
        vParam(1) = vFunzione
        'aggiunto l'ultimo parametro che è dato dall'input dell utente, per stabilire 
        'se da determina o la disp deve andare al (0) Dirigenza Generale o al (1) Controllo Amministrativo
        vr = oDllDetermine.Elabora(cFunzione, vParam, , , destinatarioInoltro)

        oDllDetermine = Nothing

        Return vr
    End Function

    Public Function Inoltra_Delibera(ByVal codDocumento As String, ByVal codAzione As String, Optional ByVal prossimoAttore As String = "", Optional ByVal note As String = "", Optional ByVal oDllDelibere As DllDocumentale.svrDelibereBase = Nothing, Optional ByVal suggerimento As Integer = -1) As Object
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Passo_Delibera
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Passo_Delibera) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim deistanziaOggetto As Boolean = True

        If oDllDelibere Is Nothing Then
            'oDllDelibere = New DllDocumentale.svrDelibere(oOperatore)
            oDllDelibere = DllDocumentale.AbstractSvrDocumenti.getSvrDelibere(oOperatore)

        Else
            deistanziaOggetto = False
        End If

        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_idDocumento) = codDocumento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_utente) = oOperatore.Codice
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_ufficioUtente) = oOperatore.oUfficio.CodUfficio
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_azione) = codAzione
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_prossimoAttore) = prossimoAttore
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_note) = note
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Delibera.c_suggerimento) = suggerimento

        vClient(0) = ""
        vClient(1) = ""
        vClient(2) = oOperatore.Codice
        vClient(3) = ""

        vParam(0) = vClient
        vParam(1) = vFunzione

        vr = oDllDelibere.Elabora(cFunzione, vParam)

        If deistanziaOggetto Then
            oDllDelibere = Nothing
        End If

        Return vr
    End Function

    Public Function Inoltra_AltroAtto(ByVal codDocumento As String, ByVal codAzione As String, Optional ByVal prossimoAttore As String = "", Optional ByVal note As String = "", Optional ByVal oDllAltriAtti As DllDocumentale.svrAltriAttiBase = Nothing, Optional ByVal suggerimento As Integer = -1, Optional ByVal tipoAtto As Integer = -1) As Object
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Passo_AltroAtto
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.cfo_Passo_AltroAtto) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim deistanziaOggetto As Boolean = True

        If oDllAltriAtti Is Nothing Then
            oDllAltriAtti = DllDocumentale.AbstractSvrDocumenti.getSvrAltriAtti(oOperatore)

        Else
            deistanziaOggetto = False
        End If

        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_idDocumento) = codDocumento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_utente) = oOperatore.Codice
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_ufficioUtente) = oOperatore.oUfficio.CodUfficio
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_azione) = codAzione
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_prossimoAttore) = prossimoAttore
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_note) = note
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_suggerimento) = suggerimento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_AltriAtti.c_tipoAtto) = tipoAtto

        vClient(0) = ""
        vClient(1) = ""
        vClient(2) = oOperatore.Codice
        vClient(3) = ""

        vParam(0) = vClient
        vParam(1) = vFunzione

        vr = oDllAltriAtti.Elabora(cFunzione, vParam)

        If deistanziaOggetto Then
            oDllAltriAtti = Nothing
        End If

        Return vr
    End Function

    Public Function Inoltra_Disposizione(ByVal codDocumento As String, ByVal codAzione As String, Optional ByVal prossimoAttore As String = "", Optional ByVal note As String = "", Optional ByVal suggerimento As Integer = -1, Optional ByVal destinatarioInoltro As Integer = -1, Optional ByVal flagUrgente As Boolean = False) As Object
        Dim oDllDisposizioni As DllDocumentale.svrDisposizioniBase
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Passo_Disposizione
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Passo_Disposizione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing

        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        ' oDllDisposizioni = New DllDocumentale.svrDisposizioni(oOperatore)
        'Lu 05/02/2010 test ereditarietà
        oDllDisposizioni = DllDocumentale.AbstractSvrDocumenti.getSvrDisposizioni(oOperatore)

        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_idDocumento) = codDocumento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_utente) = oOperatore.Codice
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_ufficioUtente) = oOperatore.oUfficio.CodUfficio
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_azione) = codAzione
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_prossimoAttore) = prossimoAttore
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_note) = note
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_suggerimento) = suggerimento
        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Passo_Disposizione.c_flagUrgente) = flagUrgente

        vClient(0) = ""
        vClient(1) = ""
        vClient(2) = oOperatore.Codice
        vClient(3) = ""

        vParam(0) = vClient
        vParam(1) = vFunzione

        vr = oDllDisposizioni.Elabora(cFunzione, vParam, , , destinatarioInoltro)

        oDllDisposizioni = Nothing

        Return vr
    End Function

    Public Function Monitors(ByRef context As HttpContext, ByVal lstr_PaginaChiamante As String)
        Dim vettoredati As Object = Nothing
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))

        Dim txtDescUfficio As String = context.Session.Item("txtDescUfficio")
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        Dim idTipologiaDocumento As Integer
        Dim autorizzazionePubblicazione As String


        If Not context.Session.Item("txtTipologiaDocumento") Is Nothing Then
            idTipologiaDocumento = context.Session.Item("txtTipologiaDocumento")
        Else
            idTipologiaDocumento = -1
        End If

        If Not context.Session.Item("txtAutorizPubblicazione") Is Nothing Then
            autorizzazionePubblicazione = context.Session.Item("txtAutorizPubblicazione")
        Else
            autorizzazionePubblicazione = Nothing
        End If

        Dim oOperatore As DllAmbiente.Operatore = context.Session("oOperatore")

        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            ' txtDataInizio = New Date(Year(Today), Now.Month - 3, 1)
            txtDataInizio = Now.AddMonths(-1)

        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If
        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
        Dim txtNumero As String = "" & context.Session("txtNumero")
        Dim tipoRigetto As String = "" & context.Session("TipoRigetto")
        Dim tipologiaRicercaBeneficiario = "" & context.Session("TipologiaRicercaBeneficiario")
        Dim beneficiario = "" & context.Session("FiltroRicercaBeneficiario")
        Dim tipologiaRicercaDestinatario = "" & context.Session("TipologiaRicercaDestinatario")
        Dim destinatario = "" & context.Session("FiltroRicercaDestinatario")
        Dim txtCodiceCUP As String = context.Session.Item("txtCodiceCUP")
        Dim txtCodiceCIG As String = context.Session.Item("txtCodiceCIG")

        If context.Session.Item("txtCodDipartimenti") Is Nothing OrElse String.IsNullOrEmpty(context.Session.Item("txtCodDipartimenti")) Then
            Dim op As DllAmbiente.Operatore = context.Session.Item("oOperatore")
            If Not op.oUfficio.CodDipartimento Is Nothing And Not String.IsNullOrEmpty(op.oUfficio.CodDipartimento) Then
                context.Session.Add("txtCodDipartimenti", op.oUfficio.CodDipartimento)
            Else
                Dim hta As ArrayList = op.DipartimentoDipendenti(tipoApplic)
                If hta.Count > 0 Then
                    context.Session.Add("txtCodDipartimenti", DirectCast(hta.Item(0), DllAmbiente.StrutturaInfo).CodiceInterno)
                Else
                    Throw New Exception("L'operatore non è abilitato alla visualizzazione di alcun dipartimento")
                End If
            End If
        End If

        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")
        Dim ufCons As ArrayList = New ArrayList
        If lstr_PaginaChiamante <> "" Then
            ufCons = oOperatore.oUfficio.UfficiConsultabili(tipoApplic, oOperatore.pCodice)
            'consulto anche il mio ufficio di appartenenza
            If lstr_PaginaChiamante = "UFFICI" Then
                If Not ufCons.Contains(oOperatore.oUfficio.CodUfficio) Then
                    ufCons.Add(oOperatore.oUfficio.CodUfficio)
                End If
            End If
        End If

        Select Case UCase(tipoApplic)
            Case 0
                vettoredati = Elenco_Monitor(0, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, ufCons.ToArray, tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario)
                context.Items.Add("tipoApplic", 0)
            Case 1
                vettoredati = Elenco_Monitor(1, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, ufCons.ToArray, tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario)
                context.Items.Add("tipoApplic", 1)
            Case 2
                vettoredati = Elenco_Monitor(2, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, ufCons.ToArray, tipoRigetto, tipologiaRicercaBeneficiario, beneficiario, txtCodiceCUP, txtCodiceCIG, idTipologiaDocumento, autorizzazionePubblicazione, tipologiaRicercaDestinatario, destinatario)
                context.Items.Add("tipoApplic", 2)
        End Select
        context.Items.Add("vettoreDati", vettoredati)


    End Function

    Public Function Elenco_Monitor(ByVal tipoDoc As Integer, _
                                     Optional ByVal dataInizio As String = "", _
                                     Optional ByVal dataFine As String = "", _
                                     Optional ByVal oggetto As String = "", _
                                     Optional ByVal ufficio As String = "", _
                                     Optional ByVal descrUfficio As String = "", _
                                     Optional ByVal cod_dip As String = "", _
                                     Optional ByVal numero_doc As String = "", _
                                     Optional ByVal v_uffici_consultabili As Object = Nothing, _
                                     Optional ByVal tipoRigetto As String = "", _
                                     Optional ByVal tipologiaRicercaBeneficiario As String = "", _
                                     Optional ByVal beneficiario As String = "", _
                                     Optional ByVal codiceCUP As String = "", _
                                     Optional ByVal codiceCIG As String = "", _
                                     Optional ByVal idTipologiaDocumento As Integer = -1, _
                                     Optional ByVal autorizzazionePubblicazione As String = "", _
                                     Optional ByVal tipologiaRicercaDestinatario As String = "", _
                                     Optional ByVal destinatario As String = "", _
                                     Optional ByVal tipoData As Integer = -1) As Object
        Dim oDllDocumento As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Monitor
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Monitor) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        If numero_doc <> "" Then
            dataInizio = ""
            dataFine = ""
            oggetto = ""
        End If


        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumento = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_utente) = oOperatore.Codice  'idoperatore
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_tipoDocumento) = tipoDoc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_data_inizio) = dataInizio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_data_fine) = dataFine
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_oggetto) = oggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_cod_ufficio) = ufficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_descr_ufficio) = descrUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_cod_dip) = cod_dip
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_numero_doc) = numero_doc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_uffici_consultabili) = v_uffici_consultabili
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_tipo_Rigetto) = tipoRigetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_tipologia_ricerca_beneficiario) = tipologiaRicercaBeneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_beneficiario) = beneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_codiceCUP) = codiceCUP
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_codiceCIG) = codiceCIG
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_id_tipologia_documento) = idTipologiaDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_autorizzazione_pubblicazione) = autorizzazionePubblicazione
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_tipologia_ricerca_destinatario) = tipologiaRicercaDestinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_destinatario) = destinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Monitor.c_tipo_data) = tipoData

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumento.Elabora(cFunzione, vParam)

            oDllDocumento = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Elenco_Monitor = vRit
        End Try


    End Function

    Public Function Elenco_Messaggi(Optional ByVal key As String = "", Optional ByVal boolInviati As Boolean = False) As Generic.List(Of Ext_MessaggioInfo)
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Messaggi
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Messaggi) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim messaggio As Ext_MessaggioInfo

        Dim returnListaMessaggi As New Generic.List(Of Ext_MessaggioInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            If boolInviati Then
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Messaggi.c_inviati) = 1 'idoperatore
            End If
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Messaggi.c_destinatario) = oOperatore.Codice 'idoperatore

            If Not String.IsNullOrEmpty(key) Then
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Messaggi.c_IdDocumento) = key  'chiave del documento
            End If

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumenti.Elabora(cFunzione, vParam)
            If vr(0) = 0 Then
                'ci sono messaggi da leggere
                Dim listaMessaggiDiRitorno As Collections.Generic.List(Of DllDocumentale.Model.MessaggioInfo) = vr(1)
                For Each mess As DllDocumentale.Model.MessaggioInfo In listaMessaggiDiRitorno
                    messaggio = New Ext_MessaggioInfo
                    With messaggio
                        .Id = mess.Id
                        .Img = mess.Img
                        .Mittente = mess.Mittente
                        .Destinatario = mess.Destinatario
                        .Data = mess.Data
                        .Testo = mess.Testo
                        .IdDocumento = mess.IdDocumento
                    End With
                    returnListaMessaggi.Add(messaggio)
                Next
            End If

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Elenco_Messaggi = returnListaMessaggi
        End Try
    End Function

    Public Function Cancella_Allegati(ByVal codAllegato As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Cancella_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Cancella_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

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

    Public Function Cancella_Messaggio(ByVal codMessaggio As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Cancella_Messaggio
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Cancella_Messaggio) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Messaggio.c_idMessaggio) = codMessaggio  'idoperatore
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Messaggio.c_cancella) = True  'idoperatore

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
            Throw New Exception(Err.Description)
        Finally
            Cancella_Messaggio = vRit
        End Try
    End Function

    Public Function Leggi_Messaggio(ByVal idMessaggio As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Leggi_Messaggio
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Leggi_Messaggio) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Messaggio.c_idMessaggio) = idMessaggio 'idoperatore

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
            Leggi_Messaggio = vRit
        End Try


    End Function

    Public Function Storico_Documento(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Storico_Documento
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Storico_Documento) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Storico_Documento.c_idDocumento) = codDocumento  'idoperatore
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Storico_Documento.c_codUfficioProponente) = Nothing

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione


            Dim objDoc As Object = Leggi_Documento(codDocumento)
            If IsArray(objDoc) Then
                If objDoc(0) = 0 Then
                    vFunzione(DllDocumentale.Dic_FODocumentale.vc_Storico_Documento.c_codUfficioProponente) = objDoc(2)
                End If
            End If
            vr = oDllDocumenti.Elabora(cFunzione, vParam)

            oDllDocumenti = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Storico_Documento = vRit
        End Try


    End Function

    Public Function Elenco_Allegati(ByVal codDocumento As String, Optional ByRef tipoAllegato As Integer = -1, Optional ByVal allDocumento As Integer = 0, Optional ByVal daStampare As String = Nothing) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Allegati
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Allegati) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

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

    Public Function Allegato_Da_Firmare(ByVal codDocumento As String) As Object
        Dim arrayPdfTemplate As Object = Nothing
        Dim arrayDoc As Object = Nothing

        Try

            arrayPdfTemplate = Elenco_Allegati(codDocumento, 21)
            If arrayPdfTemplate(0) = 0 Then
                Return arrayPdfTemplate
            Else
                arrayDoc = Elenco_Allegati(codDocumento, , 1)
                Return arrayDoc
            End If

        Catch ex As Exception

        Finally


        End Try
    End Function

    Public Function Elenco_Allegati_Da_Stampare(ByVal codDocumento As String, Optional ByRef tipoAllegato As Integer = -1, Optional ByVal allDocumento As Integer = 0) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Allegati_Da_Stampare
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Allegati) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_tipoAllegati) = tipoAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Allegati.c_allDocumento) = allDocumento


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

            Elenco_Allegati_Da_Stampare = vr
        End Try


    End Function

    Public Function Elenco_Compiti_Documento(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Compiti_Documento
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Compiti_Documento) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Compiti_Documento.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Compiti_Documento.c_NomeCognome) = 0
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
            vr = vRit
        Finally
            Elenco_Compiti_Documento = vr
        End Try


    End Function
    Public Function GetArrContoEconomica(operatore As DllAmbiente.Operatore, Bilancio As String, Capitolo As String, Ufficio As String) As ArrayList

        Dim DettaglioBilancio As Risposta_InterrogazioneBilancio_Types = ClientIntegrazioneSic.MessageMaker.createInterrogazioneBilancioInfoMessage(operatore, Bilancio, Capitolo, Ufficio)
        Dim arrContoEconomica As New ArrayList

        If Not String.IsNullOrEmpty(DettaglioBilancio.ContoEconomica1) Then
            arrContoEconomica.Add(DettaglioBilancio.ContoEconomica1)
        End If
        If Not String.IsNullOrEmpty(DettaglioBilancio.ContoEconomica2) Then
            arrContoEconomica.Add(DettaglioBilancio.ContoEconomica2)
        End If
        If Not String.IsNullOrEmpty(DettaglioBilancio.ContoEconomica3) Then
            arrContoEconomica.Add(DettaglioBilancio.ContoEconomica3)
        End If
        If Not String.IsNullOrEmpty(DettaglioBilancio.ContoEconomica4) Then
            arrContoEconomica.Add(DettaglioBilancio.ContoEconomica4)
        End If
        If Not String.IsNullOrEmpty(DettaglioBilancio.ContoEconomica5) Then
            arrContoEconomica.Add(DettaglioBilancio.ContoEconomica5)
        End If
        Return arrContoEconomica
    End Function
    Public Function Anteprima_Allegato(ByVal idAllegato As String, Optional ByVal docP7m As Boolean = False, Optional ByVal docTsr As Boolean = False) As Object
        Dim oDllDocumento As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Leggi_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Leggi_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(7) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumento = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Allegato.c_idAllegato) = idAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Allegato.c_p7m) = docP7m
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Leggi_Allegato.c_tsr) = docTsr

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumento.Elabora(cFunzione, vParam)

            oDllDocumento = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)
            vRit(2) = vr(2)
            vRit(3) = vr(3)
            vRit(4) = vr(4)
            vRit(5) = vr(5)
            vRit(6) = vr(6)
            'LU 15/05 Correzione codice documento per PDF
            'vRit(7)  idDocumento
            vRit(7) = vr(8)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Anteprima_Allegato = vRit
        End Try


    End Function

    Public Function Aggiorna_PDF_Appendice(ByVal bFile() As Byte, ByVal idallegato As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Aggiorna_Appendice
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Aggiorna_appendice) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Aggiorna_appendice.c_idAllegato) = idallegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Aggiorna_appendice.c_binarioAllegato) = bFile

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
            Aggiorna_PDF_Appendice = vRit
        End Try


    End Function

    Public Function Registra_Allegato(ByVal bFile() As Byte, ByVal nomeFile As String, ByVal estensione As String, ByVal codDocumento As String, ByVal codTipo As Integer, Optional ByVal versioneAllegato As Integer = 1, Optional ByVal destinatari As String = "", Optional ByVal modalita As String = "", Optional ByVal riferimento_Appendice As String = "", Optional ByVal flagControlloIstanza As Boolean = True, Optional ByVal flagRegistraAttivita As Boolean = True, Optional ByVal livelloFirma As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

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

    Public Function Registra_Firma(ByVal bFile() As Byte, ByVal codDocumento As String, Optional ByVal nomeFile As String = "Documento Firmato ", Optional ByVal estensione As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Allegato
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Allegato) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object


        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            If ConfigurationManager.AppSettings("VERIFICA_CERTIFICATI_FIRMA") = 1 Or ConfigurationManager.AppSettings("VERIFICA_FIRMA_OPERATORE") Then
                'verifico Firma

                Dim dsUtils As DigitalSignatureUtils.CAdESUtils = New DigitalSignatureUtils.CAdESUtils(bFile)
                If ConfigurationManager.AppSettings("VERIFICA_CERTIFICATI_FIRMA") = 1 Then
                    dsUtils.CheckSignature()
                End If
                If ConfigurationManager.AppSettings("VERIFICA_FIRMA_OPERATORE") = 1 Then
                    Dim sd() As SignerData = dsUtils.GetFirmatari()
                    Dim i As Integer
                    For i = 0 To sd.Length - 1
                        Log.Info("CF Firmatario " & (i + 1) & ":" & sd(i).CodiceFiscale)
                        Log.Info("Nome Firmatario " & (i + 1) & ":" & sd(i).Nome)
                        Log.Info("Cognome Firmatario " & (i + 1) & ":" & sd(i).Cognome)
                        If String.IsNullOrEmpty(oOperatore.CodiceFiscale) OrElse UCase(oOperatore.CodiceFiscale) <> UCase(sd(i).CodiceFiscale) Then
                            Dim errore As String = "Firma Operatore non valida (Riconoscimento tramite codice fiscale): non ci sono certificati di firma per l'utente " & oOperatore.Cognome & " " & oOperatore.Nome
                            Log.Error(errore)
                            Throw New Exception(errore)
                        End If
                    Next
                End If
            End If

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_binarioAllegato) = bFile
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_descEstensione) = estensione
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_copiaFirmatoDoc) = 1
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_autore) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_firmato) = 1
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Allegato.c_nome) = nomeFile



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
            Log.Error("Errore durante il salvataggio del file firmato: " + ex.Message)
            Throw ex
        Finally
            Registra_Firma = vRit
        End Try

    End Function

    Public Function Verifica_Prima_Apertura(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Verifica_Prima_Apertura
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Verifica_Prima_Apertura) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Verifica_Prima_Apertura.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Verifica_Prima_Apertura.c_utente) = oOperatore.Codice

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
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Verifica_Prima_Apertura = vRit
        End Try


    End Function

    Public Function Registra_Compito(ByVal codDocumento As String, ByVal specifico As Boolean) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Compito
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Compito) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            Dim compito As String
            compito = oDllDocumenti.Definisci_Compito(codDocumento, specifico, oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Compito.c_idDocumento) = codDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Compito.c_utente) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Compito.c_compito) = compito

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
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Registra_Compito = vRit
        End Try


    End Function

    Public Function Modifica_Compiti(ByVal codDocumento As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Update_compiti_Specifici_to_Generici
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Compito) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

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

    Public Function Consulta_documenti_AltriUffici(ByVal tipoDoc As Integer,
                                     Optional ByVal tipoData As Integer = -1, _                             
                                     Optional ByVal dataInizio As String = "",
                                     Optional ByVal dataFine As String = "",
                                     Optional ByVal oggetto As String = "",
                                     Optional ByVal ufficio As String = "",
                                     Optional ByVal descrUfficio As String = "",
                                     Optional ByVal cod_dip As String = "",
                                     Optional ByVal numeroDoc As String = "",
                                     Optional ByVal archivio As Boolean = False,
                                    Optional ByVal tipoRigetto As String = "",
                                    Optional ByVal visualizza_per_competenza As Boolean = False,
                                    Optional ByVal StatoStampato As String = "",
                                    Optional ByVal tipologiaRicercaBeneficiario As String = "",
                                    Optional ByVal beneficiario As String = "",
                                    Optional ByVal codiceCUP As String = "",
                                    Optional ByVal codiceCIG As String = "",
                                    Optional ByVal idTipologiaDocumento As Integer = -1,
                                    Optional ByVal autorizzazionePubblicazione As String = "",
                                    Optional ByVal tipologiaRicercaDestinatario As String = "",
                                    Optional ByVal destinatario As String = "",
                                    Optional ByVal visualizzaAnnullati As Boolean = False) As Object

        Dim oDllDocumento As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Documenti
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Documenti) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        If numeroDoc <> "" Then
            dataInizio = ""
            dataFine = ""
            oggetto = ""
        End If


        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Try
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_competenza) = ""
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente_competenza) = ""

            If visualizza_per_competenza Then
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_competenza) = oOperatore.oUfficio.CodUfficio
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente_competenza) = oOperatore.Codice
            End If

            If archivio Then
                Dim vet As ArrayList = oOperatore.oUfficio.UfficiConsultabili(tipoDoc, oOperatore.pCodice)
                ' vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio) = oOperatore.oUfficio.CodArchivio()
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione) = vet.ToArray

                Dim uff As New DllAmbiente.Ufficio
                uff.CodUfficio = cod_dip
                vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente) = uff.CodArchivio
                If Not String.IsNullOrEmpty(StatoStampato) Then
                    vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_FlagStampato) = StatoStampato
                End If
            Else

                Select Case ufficio
                    Case ""
                        Dim vet As ArrayList = oOperatore.oUfficio.UfficiConsultabili(tipoDoc, oOperatore.oUfficio.CodUfficio)
                        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio) = vet.ToArray
                    Case oOperatore.oUfficio.CodUfficio
                        Dim vet As ArrayList = oOperatore.oUfficio.UfficiConsultabili(tipoDoc, oOperatore.pCodice)
                        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio) = vet.ToArray
                    Case Else
                        vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio) = oOperatore.oUfficio.CodArchivio()
                End Select

            End If

            If (Not String.IsNullOrEmpty(StatoStampato)) And archivio Then
                'la chiamata è relativa alla funzionalità "atti da stampare" 
                oDllDocumento = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)
                cFunzione = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Documenti_Da_Stampare
            Else
                'la chiamata è relativa a "archivio" o "deposito" o "worklist"
                oDllDocumento = New DllDocumentale.svrDocumenti(oOperatore)
            End If

            'vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente) = oOperatore.Codice
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento) = tipoDoc


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio) = dataInizio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine) = dataFine
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_oggetto) = oggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_ufficio) = ufficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_dip) = cod_dip
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_numero_doc) = numeroDoc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_Rigetto) = tipoRigetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipologia_ricerca_beneficiario) = tipologiaRicercaBeneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_beneficiario) = beneficiario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_codiceCUP) = codiceCUP
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_codiceCIG) = codiceCIG
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_id_tipologia_documento) = idTipologiaDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_autorizzazione_pubblicazione) = autorizzazionePubblicazione
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipologia_ricerca_destinatario) = tipologiaRicercaDestinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_destinatario) = destinatario
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_visualizzaAnnullati) = visualizzaAnnullati
	    vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_data) = tipoData
            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""
            vParam(0) = vClient
            vParam(1) = vFunzione
            vr = oDllDocumento.Elabora(cFunzione, vParam)
            oDllDocumento = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)

        Catch ex As Exception

            vRit(0) = Err.Number
            vRit(1) = Err.Description

        Finally

            Consulta_documenti_AltriUffici = vRit

        End Try
    End Function

    Public Sub Inizializza_Pagina(ByVal pagina As Object, Optional ByVal argomentoContenuto As String = "&nbsp")
        Dim tabellaHalbero As New Web.UI.WebControls.Table
        Dim rigaHalbero As New Web.UI.WebControls.TableRow
        Dim cellaHalbero As New Web.UI.WebControls.TableCell
        Dim tabellaHcontenuto As New Web.UI.WebControls.Table
        Dim rigaHcontenuto As New Web.UI.WebControls.TableRow
        Dim cellaHcontenuto As New Web.UI.WebControls.TableCell

        tabellaHalbero.CssClass = "x-tree-header"
        ' rigaHalbero.CssClass = "x-panel-header"
        'cellaHalbero.CssClass = "x-panel-header"

        tabellaHcontenuto.CssClass = "x-contenuto-header"
        'rigaHcontenuto.CssClass = "x-panel-header"
        'cellaHcontenuto.CssClass = "x-panel-header"

        rigaHalbero.Controls.Add(cellaHalbero)
        tabellaHalbero.Rows.Add(rigaHalbero)
        rigaHcontenuto.Controls.Add(cellaHcontenuto)
        tabellaHcontenuto.Rows.Add(rigaHcontenuto)

        Dim header As header = pagina.LoadControl("header.ascx")
        Dim tree As tree = pagina.LoadControl("tree.ascx")

        pagina.FindControl("Testata").Controls.Add(header)

        cellaHalbero.Text = "Menu"
        pagina.FindControl("Albero").Controls.Add(tabellaHalbero)
        pagina.FindControl("Albero").Controls.Add(tree)
        tabellaHcontenuto.ID = "tabellaHcontenuto"
        If Not pagina.FindControl("Contenuto") Is Nothing Then
            cellaHcontenuto.ID = "cellaHcontenuto"
            cellaHcontenuto.Text = argomentoContenuto
            pagina.FindControl("Contenuto").Controls.Add(tabellaHcontenuto)
        End If
    End Sub

    Public Sub Rinomina_Pagina(ByRef pagina As Object, ByVal argomentoContenuto As String)
        pagina.FindControl("Contenuto").findcontrol("tabellaHcontenuto").findcontrol("cellaHcontenuto").text = argomentoContenuto
    End Sub

    Public Function Info_HomePage() As Object
        Dim oDllDocumentale As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Info_HomePage
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Info_HomePage) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumentale = New DllDocumentale.svrDocumenti(oOperatore)

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Info_HomePage.c_utente) = oOperatore.Codice

            vClient(0) = ""
            vClient(1) = ""
            vClient(2) = oOperatore.Codice
            vClient(3) = ""

            vParam(0) = vClient
            vParam(1) = vFunzione

            vr = oDllDocumentale.Elabora(cFunzione, vParam)

            oDllDocumentale = Nothing
            vRit(0) = vr(0)
            vRit(1) = vr(1)

        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Info_HomePage = vRit
        End Try


    End Function

    Function TrovaFirstKey(ByVal tabellaHash As Hashtable, ByVal valueToFind As Object) As Object
        Dim keyResult As Object = Nothing

        If Not tabellaHash Is Nothing Then
            For Each key As Object In tabellaHash.Keys
                If Not tabellaHash(key) Is Nothing AndAlso tabellaHash(key).Equals(valueToFind) Then
                    keyResult = key
                    Return keyResult
                End If

            Next
        End If
        Return keyResult
    End Function

    Public Function hashToArraySorted(ByVal tabellaHash As Hashtable) As Object
        Dim vettoreDati(1, tabellaHash.Count - 1) As Object
        Dim vettoreChiavi(tabellaHash.Count - 1) As Object
        Dim vettoreValori(tabellaHash.Count - 1) As Object
        tabellaHash.Keys.CopyTo(vettoreChiavi, 0)
        tabellaHash.Values.CopyTo(vettoreValori, 0)
        Array.Sort(vettoreValori)
        Dim conta As Integer = 0
        For conta = 0 To UBound(vettoreValori)
            Dim key As Object = Nothing
            If vettoreValori(conta) <> Nothing Then
                vettoreDati(1, conta) = vettoreValori(conta)
                key = TrovaFirstKey(tabellaHash, vettoreValori(conta))

                vettoreDati(0, conta) = key
                tabellaHash.Remove(key)
            End If


        Next


        'Dim j As Integer = 0
        'For j = 0 To UBound(vettoreChiavi)
        '    If vettoreChiavi(j) <> Nothing Then
        '        vettoreDati(0, j) = vettoreChiavi(j)
        '        vettoreDati(1, j) = tabellaHash.Item(vettoreChiavi(j))
        '    End If
        'Next
        'tabellaHash.Value()
        Return vettoreDati
    End Function

    Public Function Aggiorna_Documento(ByVal objDocumento As DllDocumentale.Model.DocumentoInfo, ByVal CampoDaAggiornare As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Aggiorna_Documento
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Aggiorna_Documento) As Object
        Dim vClient(3) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Aggiorna_Documento.c_idDocumento) = objDocumento.Doc_id  'Codice Docuemnto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Aggiorna_Documento.c_ValoreCampoAggiornare) = objDocumento.Doc_Testo
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Aggiorna_Documento.c_CampoDaAggiornare) = CampoDaAggiornare

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
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Aggiorna_Documento = vRit
        End Try


    End Function

    Sub aggiungiValori(ByRef orig As Hashtable, ByRef dest As Hashtable)
        For Each key As String In orig.Keys
            If dest(key) Is Nothing Then
                dest.Add(key, orig(key))
            End If
        Next
    End Sub

    Public Function Cancella_Allegato_Fisicamente(Optional ByVal codAllegato As String = "", Optional ByVal CodTipologia As String = "", Optional ByVal idDocumento As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Cancella_Allegato_Fisicamente
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Cancella_Allegato_Fisicamente) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_idAllegato) = codAllegato
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_idDocumento) = idDocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Cancella_Allegato_Fisicamente.c_tipologiaAllegato) = CodTipologia

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
            Cancella_Allegato_Fisicamente = vRit
        End Try
    End Function

    Public Function Verifica_Firma_Utente_Documento(ByVal codDocumento As String, Optional ByVal codAzione As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim returnInt As Integer = 0
        Dim oOperatore As DllAmbiente.Operatore
        Try

            oOperatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            ' returnInt = oDllDocumenti.VERIFICA_FIRMA_UTENTE(codDocumento, oOperatore.Codice)
            returnInt = oDllDocumenti.VERIFICA_FIRMA_UTENTE(codDocumento, oOperatore.Codice, codAzione)

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Verifica_Firma_Utente_Documento = returnInt
        End Try
    End Function

    Public Function Verifica_Marca_Utente_Documento(ByVal codDocumento As String, Optional ByVal codAzione As String = "") As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti    '"dll.classe"
        Dim returnBool As Boolean = False
        Dim oOperatore As DllAmbiente.Operatore
        Try
            oOperatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            returnBool = oDllDocumenti.VERIFICA_MARCA_UTENTE(codDocumento, oOperatore.Codice)

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Verifica_Marca_Utente_Documento = returnBool
        End Try
    End Function

    'Questa funzione viene chiamata in Worklist Da archiviare
    Public Function Elenco_Documenti_Da_Archiviare(ByVal tipoDoc As Integer,
                                     ByVal utente_fittizio As String,
                                     Optional ByVal dataInizio As String = "",
                                     Optional ByVal dataFine As String = "",
                                     Optional ByVal oggetto As String = "",
                                     Optional ByVal ufficio As String = "",
                                     Optional ByVal dipartimento As String = "",
                                     Optional ByVal numero As String = "",
                                     Optional ByVal tipoRigetto As String = "") As Object

        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Elenco_Documenti
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Elenco_Documenti) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            If numero <> "" Then
                dataInizio = ""
                dataFine = ""
                oggetto = ""
            End If
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_utente) = utente_fittizio  'idoperatore
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento) = tipoDoc
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio) = dataInizio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine) = dataFine
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_oggetto) = oggetto
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_ufficio) = ufficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_cod_dip) = dipartimento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_numero_doc) = numero
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_Rigetto) = tipoRigetto


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
            Elenco_Documenti_Da_Archiviare = vRit
        End Try


    End Function

    Public Function Verifica_Azione_Ufficio(ByVal iddocumento As String, Optional ByVal ufficio As String = "U010017",
                                         Optional ByVal progressivo As Integer = -1) As Object

        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Verifica_Ultima_Azione_Ufficio
        Dim vFunzione2(DllDocumentale.Dic_FODocumentale.dimvc_Verifica_Azione_Ufficio) As Object
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Verifica_Ultima_Azione_Ufficio) As Object

        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(1) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione2(DllDocumentale.Dic_FODocumentale.vc_Verifica_Azione_Ufficio.c_ufficio) = ufficio
            vFunzione2(DllDocumentale.Dic_FODocumentale.vc_Verifica_Azione_Ufficio.c_progressivo) = progressivo

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Verifica_Ultima_Azione_Ufficio.c_idDocumento) = iddocumento
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Verifica_Ultima_Azione_Ufficio.c_parm) = vFunzione2

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
            Verifica_Azione_Ufficio = vRit
        End Try


    End Function

    Public Function Warning(ByVal idDocumento As String, Optional ByRef warnings As WarningSet = Nothing) As String
        Dim messaggio_Di_Ritorno As String = String.Empty

        If Not warnings Is Nothing Then
            warnings.clear()
        Else
            warnings = New WarningSet()
        End If

        'Verifica firma
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocumento)
        Dim numerare As Boolean = dllDoc.Numerare(idDocumento, operatore, objDocumento.Doc_Tipo)
        If numerare Then
            messaggio_Di_Ritorno = "<b>Provv. n° " & objDocumento.Doc_numeroProvvisorio & " --> " & objDocumento.Doc_numero & ":</B>"
        Else
            messaggio_Di_Ritorno = "<b>Provv. n° " & IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero) & ":</B>"
        End If

        'Verifica documento firmato
        Dim objFirma As Integer = Verifica_Firma_Utente_Documento(objDocumento.Doc_id)
        If objFirma = 0 Then
            'Documento non firmato 
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i><span class='lblRFirma'>Il documento risulta non firmato</span>"
            warnings.add(WarningType.SIGN_WARNING)
        Else
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i>Il documento risulta correttamente firmato"
        End If
        'Verifica suggerimenti
        If operatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
            Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(idDocumento)
            If listaSugg.Count > 0 Then
                'Ci sono suggerimenti
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Ultimo Suggerimento:</i><span class='lblSRed'>" & listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia & "</span>"
                warnings.add(WarningType.OTHER_WARNINGS)
            Else
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Suggerimenti:</i>Il documento non riporta suggerimenti"
            End If
        End If
        If objDocumento.Doc_Tipo = 1 Then
            Dim itemRicercato As New DllDocumentale.Documento_attributo
            itemRicercato.Doc_id = idDocumento
            itemRicercato.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
            itemRicercato.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
            Dim lista As Generic.List(Of DllDocumentale.Documento_attributo) = dllDoc.FO_Get_Documento_Attributi(itemRicercato)
            If lista.Count = 0 Then
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Pagine Allegati:</i>non è stato indicato il numero di pagine totali degli allegati presenti"
                warnings.add(WarningType.OTHER_WARNINGS)
            End If
        End If
        If objDocumento.haveOpContabile Then

            Dim chkPreimpegno As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Preimpegno)
            If chkPreimpegno = "1" Then
                Dim listaPreimpegni As IList = dllDoc.FO_Get_DatiPreImpegni(objDocumento.Doc_id)
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Preimpegni: </i>Inseriti " & listaPreimpegni.Count
            End If

            Dim count As Integer = 0
            Dim countImpegniNonRegistrati As Integer = 0
            Dim chkImpegno As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
            Dim listaImpegni As IList = dllDoc.FO_Get_DatiImpegni(objDocumento.Doc_id)
            If chkImpegno = "1" Then
                For Each impegno As DllDocumentale.ItemImpegnoInfo In listaImpegni
                    If impegno.Di_Stato = 2 Then
                        count = count + 1
                    ElseIf impegno.Di_Stato = 1 And impegno.Dli_NumImpegno = 0 Then
                        countImpegniNonRegistrati = countImpegniNonRegistrati + 1

                        Dim totale As Double = 0
                        Dim listaben As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = dllDoc.FO_Get_ListaBeneficiariImpegno(operatore, impegno.Dli_Documento, , impegno.Dli_prog)
                        For Each ben As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In listaben
                            totale = totale + ben.ImportoSpettante
                        Next


                        If Math.Abs(totale - impegno.Dli_Costo) >= 0.01 Then
                            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegno:</i><span class='lblRed'>All'impegno di € " & FormatNumber(impegno.Dli_Costo, 2) & " non sono stati associati beneficiari per un totale di € " & FormatNumber(Math.Abs(impegno.Dli_Costo - totale), 2) & "</span>"
                            warnings.add(WarningType.NO_BENEFICIARI_IMPEGNO)
                        End If


                    End If
                Next
                If listaImpegni.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i><span class='lblRed'>Registrati " & listaImpegni.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i><span class='lblRed'>Da confermare " & count & " su " & listaImpegni.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    ElseIf countImpegniNonRegistrati > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i>Da registrare " & countImpegniNonRegistrati
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i>Registrati " & listaImpegni.Count
                    End If
                End If
            End If

            Dim chkLiquidazione As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
            Dim listaLiquidazione As IList = dllDoc.FO_Get_DatiLiquidazione(objDocumento.Doc_id)

            'Se al documento sono state associate delle fatture, ma nessuna liquidazione, avviso l'utente.
            Dim listaFattureDocumento As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = dllDoc.FO_Get_ListaFatture(objDocumento.Doc_id)
            If listaLiquidazione.Count = 0 AndAlso listaFattureDocumento.Count > 0 Then
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>ATTENZIONE:</i><span class='lblRed'>Al documento risultano assiocate " & listaFattureDocumento.Count.ToString & ", ma non è presente nessuna liquidazione</span>"
            End If
            Dim listaFattureLiquidazioni As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = dllDoc.FO_Get_ListaFattureNonAssegnateLiquidazioneOrResidue(objDocumento.Doc_id)
            For Each fatturaLiquidazione As DllDocumentale.ItemFatturaInfoHeader In listaFattureLiquidazioni
                If fatturaLiquidazione.ImportoResiduo > 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>FATTURA " & fatturaLiquidazione.NumeroFatturaBeneficiario & " (" & fatturaLiquidazione.AnagraficaInfo.Denominazione & "): </i><span class='lblRed'>E' necessario liquidare il 100% del totale della fattura.  Importo totale € " & fatturaLiquidazione.ImportoTotaleFattura & ", importo liquidato € " & fatturaLiquidazione.ImportoLiquidato & ", importo residuo € " & fatturaLiquidazione.ImportoResiduo & ".  </span>"
                End If
            Next


            count = 0
            For Each liq As DllDocumentale.ItemLiquidazioneInfo In listaLiquidazione
                Dim totale As Double = 0
                If liq.Di_Stato <> 0 Then
                    Dim listaben As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = dllDoc.FO_Get_ListaBeneficiariLiquidazione(operatore, liq.Dli_Documento, , liq.Dli_prog)
                    For Each ben As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In listaben
                        totale = totale + ben.ImportoSpettante
                    Next
                End If
                If liq.Di_Stato = 2 Then
                    count = count + 1
                End If
                Dim flagDisabilitaArchivioLiquidazione As Boolean = IIf(ConfigurationManager.AppSettings("DISABILITA_ARCHIVIO_LIQUIDAZIONI") = "1", True, False)

                If flagDisabilitaArchivioLiquidazione Then
                    If Math.Abs(totale - liq.Dli_Costo) >= 0.01 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i><span class='lblRed'>Alla liquidazione di € " & FormatNumber(liq.Dli_Costo, 2) & "non sono stati associati beneficiari per un totale di € " & FormatNumber(Math.Abs(liq.Dli_Costo - totale), 2) & "</span>"
                        warnings.add(WarningType.NO_BENEFICIARI_LIQUIDAZIONE)
                    End If
                End If
            Next
            If chkLiquidazione = "1" Then
                If listaLiquidazione.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i><span class='lblRed'>Inserite " & listaLiquidazione.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i><span class='lblRed'>Da confermare " & count & " su " & listaLiquidazione.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i>Inserite " & listaLiquidazione.Count
                    End If
                End If
            Else
                'ci sono liquidazioni contestuali
                If listaLiquidazione.Count > 0 Then
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni Contestuali:</i><span class='lblRed'>Da confermare " & count & " su " & listaLiquidazione.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni Contestuali:</i>Inserite " & listaLiquidazione.Count
                    End If
                End If
            End If

            Dim chkImpegnoSuPerenti As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
            Dim listaImpegniPerenti As IList = dllDoc.FO_Get_DatiImpegniPerenti(objDocumento.Doc_id)
            count = 0
            If chkImpegnoSuPerenti = "1" Then
                For Each impegno As DllDocumentale.ItemImpegnoInfo In listaImpegniPerenti
                    If impegno.Di_Stato = 2 Then
                        count = count + 1
                    End If
                Next
                If listaImpegniPerenti.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni su Perenti:</i><span class='lblRed'>Inseriti " & listaImpegniPerenti.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni su Perenti:</i><span class='lblRed'>Da confermare " & count & " su " & listaImpegniPerenti.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni su Perenti:</i>Inseriti " & listaImpegniPerenti.Count
                    End If
                End If
            End If

            Dim chkAccertamento As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
            Dim accertamento As IList = dllDoc.FO_Get_Dati_Assunzione(objDocumento.Doc_id)
            If chkAccertamento = "1" Then
                If accertamento.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Accertamento:</i><span class='lblRed'>Inserito " & accertamento.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Accertamento:</i>Inserito " & accertamento.Count
                End If
            End If

            Dim chkRiduzione As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)
            Dim listaImpegniVariazioni As IList = dllDoc.FO_Get_DatiImpegniVariazioni(objDocumento.Doc_id)
            count = 0
            For Each impegnoVar As DllDocumentale.ItemRiduzioneInfo In listaImpegniVariazioni
                If impegnoVar.Di_Stato = 2 Then
                    count = count + 1
                End If
            Next
            If chkRiduzione = "1" Then
                If listaImpegniVariazioni.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni/Economie:</i><span class='lblRed'>Inserite " & listaImpegniVariazioni.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni/Economie:</i><span class='lblRed'>Da confermare " & count & " su " & listaImpegniVariazioni.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni/Economie:</i>Inserite " & listaImpegniVariazioni.Count
                    End If
                End If
            Else
                'ci sono riduzioni contestuali
                If listaImpegniVariazioni.Count > 0 Then
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Contestuali:</i><span class='lblRed'>Da confermare " & count & " su " & listaImpegniVariazioni.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Contestuali:</i>Inserite " & listaImpegniVariazioni.Count
                    End If
                End If
            End If
            Dim chkRiduzioneLiq As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzioneLiq)
            Dim listaLiquidazioniVariazioni As IList = dllDoc.FO_Get_DatiLiquidazioniVariazioni(objDocumento.Doc_id)
            count = 0

            If chkRiduzioneLiq = "1" Then
                For Each liqVar As DllDocumentale.ItemRiduzioneLiqInfo In listaLiquidazioniVariazioni
                    If liqVar.Di_Stato = 2 Then
                        count = count + 1
                    End If
                Next
                If listaLiquidazioniVariazioni.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Liquidazioni:</i><span class='lblRed'>Inserite " & listaLiquidazioniVariazioni.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Liquidazioni:</i><span class='lblRed'>Da confermare " & count & " su " & listaLiquidazioniVariazioni.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Liquidazioni:</i>Inserite " & listaLiquidazioniVariazioni.Count

                    End If
                End If
            End If

            Dim chkRiduzionePreImp As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzionePreImp)
            Dim listaPreImpegniVariazioni As IList = dllDoc.FO_Get_DatiPreImpegniVariazioni(objDocumento.Doc_id)
            count = 0
            If chkRiduzionePreImp = "1" Then
                For Each impegnoVar As DllDocumentale.ItemRiduzioneInfo In listaPreImpegniVariazioni
                    If Not String.IsNullOrEmpty(impegnoVar.Div_NumeroReg) AndAlso Int(impegnoVar.Div_NumeroReg) > 0 Then
                        count = count + 1
                    End If
                Next
                If listaPreImpegniVariazioni.Count = 0 Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i><span class='lblRed'>Riduzioni PreImpegni:</i>" & listaPreImpegniVariazioni.Count & "</span>"
                    warnings.add(WarningType.OTHER_WARNINGS)
                Else
                    If count > 0 Then
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni PreImpegni:</i><span class='lblRed'>Da confermare " & count & " su " & listaPreImpegniVariazioni.Count & "</span>"
                        warnings.add(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING)
                    Else
                        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni PreImpegni:</i>Inserite " & listaPreImpegniVariazioni.Count
                    End If
                End If
            End If
        End If
        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Rettifiche Contabili:</i>" & dllDoc.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/>"

        Return messaggio_Di_Ritorno
    End Function

    Public Function Registra_Osservazione(ByVal codDocumento As String, ByVal testo As String) As Object
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim cFunzione As Integer = DllDocumentale.Dic_FODocumentale.cfo_Registra_Osservazione
        Dim vFunzione(DllDocumentale.Dic_FODocumentale.dimvc_Registra_Osservazione) As Object
        Dim vClient(4) As Object
        Dim vParam(1) As Object
        Dim vr As Object = Nothing
        Dim vRit(2) As Object

        Try
            'modgg 10-06 1
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)


            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Osservazione.c_idDocumento) = codDocumento

            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Osservazione.c_operatore) = oOperatore.Codice
            Dim livelloUfficio As String = oDllDocumenti.Get_StatoIstanzaDocumento(codDocumento).LivelloUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Osservazione.c_Tipologia) = livelloUfficio
            vFunzione(DllDocumentale.Dic_FODocumentale.vc_Registra_Osservazione.c_Testo) = testo

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
            vRit(2) = vr(2)
        Catch ex As Exception
            vRit(0) = Err.Number
            vRit(1) = Err.Description
        Finally
            Registra_Osservazione = vRit
        End Try


    End Function

    Public Function Warning_Ragioneria(ByVal idDocumento As String) As String

        Dim messaggio_Di_Ritorno As String = String.Empty

        'Verifica firma
        Dim dllDoc As New DllDocumentale.svrDocumenti(HttpContext.Current.Session.Item("oOperatore"))
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocumento)
        messaggio_Di_Ritorno = "<b>Provv. n° " & IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero) & ":</b>"

        'Verifica documento firmato
        Dim objFirma As Integer = Verifica_Firma_Utente_Documento(objDocumento.Doc_id)
        If objFirma = 0 Then
            'Documento non firmato 
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i><span class='lblRFirma'>Il documento risulta non firmato</span>"
        Else
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i>Il documento risulta correttamente firmato"
        End If
        'Verifica suggerimenti
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        If operatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
            Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(idDocumento)
            If listaSugg.Count > 0 Then
                'Ci sono suggerimenti
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Ultimo Suggerimento:</i><span class='lblSRed'>" & listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia & "</span>"
            Else
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Suggerimenti:</i>Il documento non riporta suggerimenti"
            End If
        End If

        If objDocumento.haveOpContabile Then
            Dim countPreimp As Integer = 0
            Dim chkPreimpegno As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Preimpegno)
            If chkPreimpegno = "1" Then
                Dim listaPreimpegni As IList = dllDoc.FO_Get_DatiPreImpegni(objDocumento.Doc_id)
                For Each preimpegno As DllDocumentale.ItemImpegnoInfo In listaPreimpegni
                    If (preimpegno.Di_TipoAssunzioneDescr <> "PREIMP-PROVV") Then
                        countPreimp = countPreimp + 1
                    End If
                Next

                If listaPreimpegni.Count <> countPreimp Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Preimpegni:</i><span class='lblRed'>Dichiarati " & listaPreimpegni.Count & " Registrati " & countPreimp & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Preimpegni:</i>Dichiarati " & listaPreimpegni.Count & " Registrati " & countPreimp
                End If
                countPreimp = 0
            End If
            Dim count As Integer = 0
            Dim chkImpegno As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
            Dim listaImpegni As IList = dllDoc.FO_Get_DatiImpegni(objDocumento.Doc_id)
            If chkImpegno = "1" Then
                For Each impegno As DllDocumentale.ItemImpegnoInfo In listaImpegni
                    If Not String.IsNullOrEmpty(impegno.Dli_NumImpegno) AndAlso Int(impegno.Dli_NumImpegno) > 0 Then
                        count = count + 1
                    End If
                Next
                'evidenzio se c'è un errore
                If listaImpegni.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i><span class='lblRed'>Dichiarati " & listaImpegni.Count & " Registrati " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni:</i>Dichiarati   " & listaImpegni.Count & " Registrati " & count
                End If
                count = 0
            End If
            Dim chkLiquidazione As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
            Dim listaLiquidazione As IList = dllDoc.FO_Get_DatiLiquidazione(objDocumento.Doc_id)
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In listaLiquidazione
                If Not String.IsNullOrEmpty(liquidazione.Dli_NLiquidazione) AndAlso Int(liquidazione.Dli_NLiquidazione) > 0 Then
                    count = count + 1
                End If
            Next
            If chkLiquidazione = "1" Then
                If listaLiquidazione.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i><span class='lblRed'>Dichiarate " & listaLiquidazione.Count & " Registrate " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i>Dichiarate " & listaLiquidazione.Count & " Registrate " & count
                End If
                count = 0
            Else
                If listaLiquidazione.Count > 0 And listaLiquidazione.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni Contestuali:</i><span class='lblRed'>Dichiarate " & listaLiquidazione.Count & " Registrate " & count & "</span>"
                ElseIf listaLiquidazione.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Liquidazioni:</i>Dichiarate " & listaLiquidazione.Count & " Registrate " & count
                End If
                count = 0
            End If

            Dim chkImpegnoSuPerenti As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
            Dim listaImpegniPerenti As IList = dllDoc.FO_Get_DatiImpegniPerenti(objDocumento.Doc_id)
            If chkImpegnoSuPerenti = "1" Then
                For Each impegno As DllDocumentale.ItemImpegnoInfo In listaImpegniPerenti
                    If Not String.IsNullOrEmpty(impegno.Dli_NumImpegno) AndAlso Int(impegno.Dli_NumImpegno) > 0 Then
                        count = count + 1
                    End If
                Next
                If listaImpegniPerenti.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni su Perenti:</i><span class='lblRed'>Dichiarati " & listaImpegniPerenti.Count & " Registrati " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Impegni su Perenti:</i>Dichiarati " & listaImpegniPerenti.Count & " Registrati " & count
                End If
                count = 0
            End If

            Dim chkAccertamento As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
            Dim accertamento As IList = dllDoc.FO_Get_Dati_Assunzione(objDocumento.Doc_id)
            If chkAccertamento = "1" Then
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Accertamento:</i>La registrazione deve avvenire solo sul SIC."
            End If

            Dim chkRiduzione As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)
            Dim listaImpegniVariazioni As IList = dllDoc.FO_Get_DatiImpegniVariazioni(objDocumento.Doc_id)
            If chkRiduzione = "1" Then
                For Each impegnoVar As DllDocumentale.ItemRiduzioneInfo In listaImpegniVariazioni
                    If Not String.IsNullOrEmpty(impegnoVar.Div_NumeroReg) AndAlso Int(impegnoVar.Div_NumeroReg) > 0 Then
                        count = count + 1
                    End If
                Next
                If listaImpegniVariazioni.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni/Economie:</i><span class='lblRed'>Dichiarate " & listaImpegniVariazioni.Count & " Registrate " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni/Economie:</i>Dichiarate " & listaImpegniVariazioni.Count & " Registrate " & count
                End If
                count = 0
            End If

            Dim chkRiduzioneLiq As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzioneLiq)
            Dim listaLiquidazioniVariazioni As IList = dllDoc.FO_Get_DatiLiquidazioniVariazioni(objDocumento.Doc_id)
            If chkRiduzioneLiq = "1" Then
                For Each liqVar As DllDocumentale.ItemRiduzioneLiqInfo In listaLiquidazioniVariazioni
                    If Not String.IsNullOrEmpty(liqVar.Div_NumeroReg) AndAlso Int(liqVar.Div_NumeroReg) > 0 Then
                        count = count + 1
                    End If
                Next
                If listaLiquidazioniVariazioni.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Liquidazioni:</i><span class='lblRed'>Dichiarate " & listaLiquidazioniVariazioni.Count & " Registrate " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Liquidazioni:</i>Dichiarate " & listaLiquidazioniVariazioni.Count & " Registrate " & count
                End If
                count = 0
            End If

            Dim chkRiduzionePreImp As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzionePreImp)
            Dim listaPreImpegniVariazioni As IList = dllDoc.FO_Get_DatiPreImpegniVariazioni(objDocumento.Doc_id)
            If chkRiduzionePreImp = "1" Then
                For Each impegnoVar As DllDocumentale.ItemRiduzioneInfo In listaPreImpegniVariazioni
                    If Not String.IsNullOrEmpty(impegnoVar.Div_NumeroReg) AndAlso Int(impegnoVar.Div_NumeroReg) > 0 Then
                        count = count + 1
                    End If
                Next
                If listaPreImpegniVariazioni.Count <> count Then
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Preimpegni:</i><span class='lblRed'>Dichiarate " & listaPreImpegniVariazioni.Count & " Registrate " & count & "</span>"
                Else
                    messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Riduzioni Preimpegni:</i>Dichiarate " & listaPreImpegniVariazioni.Count & " Registrate " & count
                End If
                count = 0
            End If
        End If
        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Rettifiche Contabili:</i>" & dllDoc.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/>"
        Return messaggio_Di_Ritorno
    End Function

    Public Function Warning_SegreteriaPresidenza(ByVal idDocumento As String) As String

        Dim messaggio_Di_Ritorno As String = String.Empty

        'Verifica firma
        Dim dllDoc As New DllDocumentale.svrDocumenti(HttpContext.Current.Session.Item("oOperatore"))
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocumento)
        messaggio_Di_Ritorno = "<b>Provv. n° " & IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero) & ":</b>"

        'Verifica documento firmato
        Dim objFirma As Integer = Verifica_Firma_Utente_Documento(objDocumento.Doc_id)
        If objFirma = 0 Then
            'Documento non firmato 
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i><span class='lblRFirma'>Il documento risulta non firmato</span>"
        Else
            messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Firma:</i>Il documento risulta correttamente firmato"
        End If
        'Verifica suggerimenti
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        ' Se si tratta di una Delibera controllo
        If objDocumento.Doc_Tipo = 1 Then
            Dim datiSedutaRelatoreInfo As DllDocumentale.ItemDatiSedutaInfo = dllDoc.FO_Get_DatiSedutaRelatoreInfo(objDocumento.Doc_id)

            If datiSedutaRelatoreInfo Is Nothing Then
                messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/><i>Dati seduta:</i><span class='lblRed'>Non sono stati inseriti</span>"
            End If
        End If


        messaggio_Di_Ritorno = messaggio_Di_Ritorno & "<br/>"
        Return messaggio_Di_Ritorno
    End Function

    Public Function hashToArray(ByVal tabellaHash As Hashtable) As Object
        Dim vettoreDati(1, tabellaHash.Count - 1) As Object
        Dim vettoreChiavi(tabellaHash.Count - 1) As Object
        tabellaHash.Keys.CopyTo(vettoreChiavi, 0)
        Dim j As Integer = 0
        For j = 0 To UBound(vettoreChiavi)
            If vettoreChiavi(j) <> Nothing Then
                vettoreDati(0, j) = vettoreChiavi(j)
                vettoreDati(1, j) = tabellaHash.Item(vettoreChiavi(j))
            End If
        Next
        Return vettoreDati
    End Function

    Public Function Elenco_Suggerimenti(Optional ByVal key As String = "") As Generic.List(Of Ext_SuggerimentoInfo)
        Dim oDllDocumenti As DllDocumentale.svrDocumenti

        Dim suggerimento As Ext_SuggerimentoInfo

        Dim returnListaSuggerimenti As New Generic.List(Of Ext_SuggerimentoInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            Dim itemsuggerimentoRicercato As New DllDocumentale.ItemSuggerimento
            Dim autore As DllAmbiente.Operatore
            itemsuggerimentoRicercato.Doc_Id = key
            itemsuggerimentoRicercato.isPubblico = True

            Dim listaSuggerimentiDiRitorno As Collections.Generic.List(Of DllDocumentale.ItemSuggerimento) = oDllDocumenti.FO_Get_Suggerimenti_Documento(itemsuggerimentoRicercato)

            For Each sugg As DllDocumentale.ItemSuggerimento In listaSuggerimentiDiRitorno
                suggerimento = New Ext_SuggerimentoInfo
                With suggerimento
                    .Id = sugg.Id
                    autore = New DllAmbiente.Operatore
                    autore.Codice = sugg.CodOperatore
                    .CodOperatore = autore.Codice
                    .Autore = autore.Cognome & " " & autore.Nome
                    .Tipologia = sugg.Id_Suggerimento
                    .DescrizioneTipologia = oDllDocumenti.Get_Suggerimento(sugg.Id_Suggerimento).Descrizione
                    .Data = sugg.DataRegistrazione.Date
                    .Note = sugg.Note
                    .IdDocumento = sugg.Doc_Id
                End With
                returnListaSuggerimenti.Add(suggerimento)
            Next
            autore = Nothing
            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Elenco_Suggerimenti = returnListaSuggerimenti
        End Try
    End Function

    Public Function Elenco_Attributi(Optional ByVal key As String = "") As Generic.List(Of Ext_AttributoInfo)
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim returnAttributo As Ext_AttributoInfo
        Dim returnListaAttributi As New Generic.List(Of Ext_AttributoInfo)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            Dim itemAttributo As New DllDocumentale.ItemAttributoInfo
            itemAttributo.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
            Dim lista As Generic.List(Of DllDocumentale.ItemAttributoInfo) = oDllDocumenti.FO_Get_Attributi(itemAttributo)
            For Each att As DllDocumentale.ItemAttributoInfo In lista
                att = oDllDocumenti.Get_SceltePossibili(att)
                returnAttributo = New Ext_AttributoInfo
                returnAttributo.ID = att.Codice
                returnAttributo.Descrizione = att.Descrizione
                returnAttributo.TipoDato = att.Tipo_Scelta.TipoDato
                If Not String.IsNullOrEmpty(key) Then
                    Dim itemAttributoRicercato As New DllDocumentale.Documento_attributo
                    itemAttributoRicercato.Cod_attributo = att.Codice
                    itemAttributoRicercato.Doc_id = key
                    Dim listaDocAtt As Generic.List(Of DllDocumentale.Documento_attributo) = oDllDocumenti.FO_Get_Documento_Attributi(itemAttributoRicercato)
                    If listaDocAtt.Count > 0 Then
                        Dim item As DllDocumentale.Documento_attributo = listaDocAtt.Item(0)
                        returnAttributo.Valore = item.Valore
                    End If
                End If

                returnListaAttributi.Add(returnAttributo)
            Next

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Elenco_Attributi = returnListaAttributi
        End Try
        Return returnListaAttributi
    End Function

    Public Function GetDatiSchedaLeggeTrasparenza(ByVal oOperatore As DllAmbiente.Operatore, Optional ByVal key As String = "") As Ext_SchedaLeggeTrasparenzaInfo
        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim ext_SchedaLeggeTrasparenzaInfo As Ext_SchedaLeggeTrasparenzaInfo = Nothing
        Dim itemSchedaTrasparenzaInfo As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            itemSchedaTrasparenzaInfo = oDllDocumenti.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, key)
            If Not itemSchedaTrasparenzaInfo Is Nothing Then
                ext_SchedaLeggeTrasparenzaInfo = New Ext_SchedaLeggeTrasparenzaInfo
                ext_SchedaLeggeTrasparenzaInfo.AutorizzazionePubblicazione = itemSchedaTrasparenzaInfo.AutorizzazionePubblicazione
                ext_SchedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento = itemSchedaTrasparenzaInfo.FunzionarioResponsabileProcedimento
                ext_SchedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento = itemSchedaTrasparenzaInfo.UfficioResponsabileProcedimento
                ext_SchedaLeggeTrasparenzaInfo.IdDocumento = itemSchedaTrasparenzaInfo.IdDocumento
                ext_SchedaLeggeTrasparenzaInfo.ModalitaIndividuazioneBeneficiario = itemSchedaTrasparenzaInfo.ModalitaIndividuazioneBeneficiario
                ext_SchedaLeggeTrasparenzaInfo.NormaAttribuzioneBeneficio = itemSchedaTrasparenzaInfo.NormaAttribuzioneBeneficio
                ext_SchedaLeggeTrasparenzaInfo.NotePubblicazione = itemSchedaTrasparenzaInfo.NotePubblicazione
                ext_SchedaLeggeTrasparenzaInfo.ContenutoAtto = itemSchedaTrasparenzaInfo.ContenutoAtto

                'ext_SchedaLeggeTrasparenzaInfo.Contratti = New Generic.List(Of Ext_ContrattoInfo)

                'Dim contrattiInfo As Generic.List(Of DllDocumentale.ItemContrattoInfo) = GetContrattiInfo(oOperatore, itemSchedaTrasparenzaInfo.Contratti)

                'For Each contratto As DllDocumentale.ItemContrattoInfo In contrattiInfo
                '    Dim ext_contratto As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                '    ext_contratto.Id = contratto.IdContratto
                '    ext_contratto.NumeroRepertorio = contratto.NumeroRepertorioContratto
                '    ext_contratto.Oggetto = contratto.OggettoContratto

                '    ext_SchedaLeggeTrasparenzaInfo.Contratti.Add(ext_contratto)
                'Next
            End If

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            GetDatiSchedaLeggeTrasparenza = ext_SchedaLeggeTrasparenzaInfo
        End Try
        Return ext_SchedaLeggeTrasparenzaInfo
    End Function

    Public Function GetDatiSchedaTipologiaProvvedimento(ByVal oOperatore As DllAmbiente.Operatore, Optional ByVal key As String = "") As Ext_SchedaTipologiaProvvedimentoInfo
        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim ext_SchedaTipologiaProvvedimentoInfo As Ext_SchedaTipologiaProvvedimentoInfo = Nothing
        Dim itemSchedaTipologiaProvvedimentoInfo As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            itemSchedaTipologiaProvvedimentoInfo = oDllDocumenti.FO_Get_SchedaTipologiaProvvedimentoInfo(oOperatore, key)
            If Not itemSchedaTipologiaProvvedimentoInfo Is Nothing Then
                ext_SchedaTipologiaProvvedimentoInfo = New Ext_SchedaTipologiaProvvedimentoInfo

                'tipologia provvedimento
                ext_SchedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento = itemSchedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento
                'importo spesa prevista
                ext_SchedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista = itemSchedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista
                ext_SchedaTipologiaProvvedimentoInfo.isSommaAutomatica = itemSchedaTipologiaProvvedimentoInfo.isSommaAutomatica
                'destinatari
                ext_SchedaTipologiaProvvedimentoInfo.Destinatari = New Generic.List(Of Ext_DestinatarioInfo)

                For Each destinatario As DllDocumentale.ItemDestinatarioInfo In itemSchedaTipologiaProvvedimentoInfo.Destinatari
                    Dim ext_destinatario As Ext_DestinatarioInfo = New Ext_DestinatarioInfo()

                    ext_destinatario.Id = destinatario.Id
                    ext_destinatario.IdSIC = destinatario.IdSIC
                    ext_destinatario.isPersonaFisica = destinatario.isPersonaFisica
                    ext_destinatario.IdDocumento = destinatario.IdDocumento
                    ext_destinatario.Denominazione = destinatario.Denominazione
                    ext_destinatario.CodiceFiscale = destinatario.CodiceFiscale
                    ext_destinatario.PartitaIva = destinatario.PartitaIva
                    ext_destinatario.DataNascita = IIf(destinatario.DataNascita Is Nothing, String.Empty, destinatario.DataNascita)
                    ext_destinatario.LuogoNascita = destinatario.LuogoNascita
                    ext_destinatario.LegaleRappresentante = destinatario.LegaleRappresentante
                    ext_destinatario.IdContratto = destinatario.IdContratto
                    ext_destinatario.NumeroRepertorioContratto = destinatario.NumeroRepertorioContratto
                    ext_destinatario.isDatoSensibile = destinatario.isDatoSensibile
                    ext_destinatario.ImportoSpettante = destinatario.ImportoSpettante

                    ext_SchedaTipologiaProvvedimentoInfo.Destinatari.Add(ext_destinatario)
                Next

            End If

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            GetDatiSchedaTipologiaProvvedimento = ext_SchedaTipologiaProvvedimentoInfo
        End Try
        Return ext_SchedaTipologiaProvvedimentoInfo
    End Function




    Public Function GetContrattoInfo(ByVal operatore As DllAmbiente.Operatore, ByVal contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader) As DllDocumentale.ItemContrattoInfo
        Dim retValue As DllDocumentale.ItemContrattoInfo = Nothing
        If Not contrattoInfoHeader Is Nothing Then
            Dim singletonContrattiInfoHeaderList As Generic.List(Of DllDocumentale.ItemContrattoInfoHeader) = New Generic.List(Of DllDocumentale.ItemContrattoInfoHeader)
            singletonContrattiInfoHeaderList.Add(contrattoInfoHeader)

            Dim singletonContrattiInfoList As Generic.List(Of DllDocumentale.ItemContrattoInfo) = GetContrattiInfo(operatore, singletonContrattiInfoHeaderList)
            If singletonContrattiInfoList.Count > 0 Then
                retValue = singletonContrattiInfoList.Item(0)
            End If
        End If
        Return retValue
    End Function

    Public Function GetContrattiInfo(ByVal operatore As DllAmbiente.Operatore, ByVal contrattiInfoHeader As Generic.List(Of DllDocumentale.ItemContrattoInfoHeader)) As Generic.List(Of DllDocumentale.ItemContrattoInfo)
        Dim retValue As Generic.List(Of DllDocumentale.ItemContrattoInfo) = New Generic.List(Of DllDocumentale.ItemContrattoInfo)

        Try
            If Not contrattiInfoHeader Is Nothing AndAlso contrattiInfoHeader.Count > 0 Then
                Dim idContratti As Generic.List(Of String) = New Generic.List(Of String)
                For Each contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader In contrattiInfoHeader
                    If Not contrattoInfoHeader Is Nothing AndAlso Not contrattoInfoHeader.IdContratto Is Nothing AndAlso contrattoInfoHeader.IdContratto.Trim() <> String.Empty Then
                        idContratti.Add(contrattoInfoHeader.IdContratto)
                    End If
                Next

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

                        itemContrattoInfo.CodieCIG = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCig
                        If itemContrattoInfo.CodieCIG Is Nothing Then
                            itemContrattoInfo.CodieCIG = String.Empty
                        End If

                        itemContrattoInfo.CodieCUP = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCup
                        If itemContrattoInfo.CodieCUP Is Nothing Then
                            itemContrattoInfo.CodieCUP = String.Empty
                        End If

                        retValue.Add(itemContrattoInfo)
                    End If
                Next
            End If
        Catch listaVuotaEx As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaEx.Message)
        End Try
        Return retValue
    End Function

    Public Function GetFattureInfo(ByVal operatore As DllAmbiente.Operatore, ByVal idDocumento As String) As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)
        Dim retValue As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = New Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)

        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim itemFattureInfoHeader As DllDocumentale.ItemFatturaInfoHeader = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(operatore)
            retValue = oDllDocumenti.FO_Get_ListaFatture(idDocumento)

            oDllDocumenti = Nothing

        Catch listaVuotaEx As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaEx.Message)
        End Try
        Return retValue
    End Function

    Public Function GetAllegatiFattureInfo(ByVal operatore As DllAmbiente.Operatore, ByVal progFattura As Integer) As Generic.List(Of DllDocumentale.ItemFatturaAllegato)
        Dim retValue As Generic.List(Of DllDocumentale.ItemFatturaAllegato) = New Generic.List(Of DllDocumentale.ItemFatturaAllegato)

        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim itemAllegatiFattura As DllDocumentale.ItemFatturaAllegato = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(operatore)
            retValue = oDllDocumenti.FO_Get_ListaAllegatiFattura(progFattura)

            oDllDocumenti = Nothing

        Catch listaVuotaEx As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaEx.Message)
        End Try
        Return retValue
    End Function

    Public Function GetListaAllegatiFatture(ByVal operatore As DllAmbiente.Operatore, ByVal listaFatture As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)) As Generic.List(Of DllDocumentale.ItemFatturaAllegato)
        Dim retValue As Generic.List(Of DllDocumentale.ItemFatturaAllegato) = New Generic.List(Of DllDocumentale.ItemFatturaAllegato)

        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim itemAllegatiFattura As DllDocumentale.ItemFatturaAllegato = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(operatore)
            retValue = oDllDocumenti.FO_Get_ListaAllegatiFatture(listaFatture)

            oDllDocumenti = Nothing

        Catch listaVuotaEx As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaEx.Message)
        End Try
        Return retValue
    End Function


    Public Function Elenco_Attributi_Urgente(Optional ByVal key As String = "") As Generic.List(Of DllDocumentale.Documento_attributo)
        Dim oDllDocumenti As DllDocumentale.svrDocumenti
        Dim returnAttributo As New DllDocumentale.Documento_attributo
        'Dim returnListaAttributi As New Generic.List(Of Ext_AttributoInfo)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        'Dim lista As Generic.List(Of DllDocumentale.ItemAttributoInfo)
        Dim listaDocAtt As Generic.List(Of DllDocumentale.Documento_attributo) = Nothing
        Try
            'modgg 10-06 1

            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)

            returnAttributo.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
            returnAttributo.Cod_attributo = "URGENTE"
            returnAttributo.Doc_id = key
            listaDocAtt = oDllDocumenti.FO_Get_Documento_Attributi(returnAttributo)
            'For Each att As DllDocumentale.ItemAttributoInfo In lista
            '    'att = oDllDocumenti.Get_SceltePossibili(att)
            '    returnAttributo = New Ext_AttributoInfo
            '    returnAttributo.ID = key
            '    returnAttributo.Descrizione = "URGENTE"
            '    'returnAttributo.TipoDato = att.Tipo_Scelta.TipoDato
            '    If Not String.IsNullOrEmpty(key) Then
            '        Dim itemAttributoRicercato As New DllDocumentale.Documento_attributo
            '        itemAttributoRicercato.Cod_attributo = "URGENTE"
            '        itemAttributoRicercato.Doc_id = key
            '        Dim listaDocAtt As Generic.List(Of DllDocumentale.Documento_attributo) = oDllDocumenti.FO_Get_Documento_Attributi(itemAttributoRicercato)
            '        If listaDocAtt.Count > 0 Then
            '            Dim item As DllDocumentale.Documento_attributo = listaDocAtt.Item(0)
            '            returnAttributo.Valore = item.Valore
            '        End If
            '    End If

            '    returnListaAttributi.Add(returnAttributo)
            'Next

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            Elenco_Attributi_Urgente = listaDocAtt
        End Try
        Return listaDocAtt
    End Function


    Public Function GetDatiSchedaContrattiFatture(ByVal oOperatore As DllAmbiente.Operatore, Optional ByVal key As String = "") As Ext_SchedaContrattiFattureInfo
        Dim oDllDocumenti As DllDocumentale.svrDocumenti = Nothing
        Dim ext_SchedaContrattiFattureInfo As Ext_SchedaContrattiFattureInfo = Nothing
        Dim itemSchedaContrattiFattureInfo As DllDocumentale.ItemSchedaContrattiFattureInfo = Nothing
        Try
            oDllDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
            itemSchedaContrattiFattureInfo = oDllDocumenti.FO_Get_SchedaContrattiFattureInfo(oOperatore, key)
            If Not itemSchedaContrattiFattureInfo Is Nothing Then
                ext_SchedaContrattiFattureInfo = New Ext_SchedaContrattiFattureInfo

                ext_SchedaContrattiFattureInfo.Contratti = New Generic.List(Of Ext_ContrattoInfo)

                ext_SchedaContrattiFattureInfo.Fatture = New Generic.List(Of Ext_FatturaInfo)

                Dim contrattiInfo As Generic.List(Of DllDocumentale.ItemContrattoInfo) = GetContrattiInfo(oOperatore, itemSchedaContrattiFattureInfo.Contratti)
                'Dim contrattiInfo As Generic.List(Of DllDocumentale.ItemContrattoInfo) = GetContrattiInfo(oOperatore, itemSchedaContrattiFattureInfo.Contratti)

                For Each contratto As DllDocumentale.ItemContrattoInfo In contrattiInfo
                    Dim ext_contratto As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                    ext_contratto.Id = contratto.IdContratto
                    ext_contratto.NumeroRepertorio = contratto.NumeroRepertorioContratto
                    ext_contratto.Oggetto = contratto.OggettoContratto
                    ext_contratto.CodiceCIG = contratto.CodieCIG
                    ext_contratto.CodiceCUP = contratto.CodieCUP

                    ext_SchedaContrattiFattureInfo.Contratti.Add(ext_contratto)
                Next

                Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetFattureInfo(oOperatore, key)

                For Each fattura As DllDocumentale.ItemFatturaInfoHeader In fattureInfo

                    Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                    Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                    Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                    Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                    Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                    Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)
                    Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()



                    ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                    ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                    ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                    ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                    ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                    ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                    ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                    ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                    ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                    ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                    ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                    ext_dati_bancari.Add(ext_dato_bancario)

                    ext_sede.DatiBancari = ext_dati_bancari
                    ext_lista_sedi.Add(ext_sede)

                    ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                    ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                    ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                    ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                    ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                    ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                    ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                    ext_fattura.IdUnivoco = fattura.IdUnivoco
                    ext_fattura.IdDocumento = fattura.IdDocumento
                    ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                    ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario
                    ext_fattura.Prog = fattura.Prog
                    ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                    ext_fattura.Contratto = ext_contratto_fattura

                    ext_SchedaContrattiFattureInfo.Fatture.Add(ext_fattura)

                    Dim listaAllegatiFatt As Generic.List(Of DllDocumentale.ItemFatturaAllegato) = GetAllegatiFattureInfo(oOperatore, fattura.Prog)
                    If Not listaAllegatiFatt Is Nothing Then
                        Dim listaAllegatiEXT As Generic.List(Of Ext_FatturaAllegato) = Nothing
                        For Each allegatoItem As DllDocumentale.ItemFatturaAllegato In listaAllegatiFatt
                            Dim allegatoExt As New Ext_FatturaAllegato
                            allegatoExt.Prog = allegatoItem.Prog
                            allegatoExt.ProgFattura = allegatoItem.ProgFattura
                            allegatoExt.Nome = allegatoItem.Nome
                            allegatoExt.Formato = allegatoItem.Formato
                            allegatoExt.Url = allegatoItem.Url
                            allegatoExt.Formato = allegatoItem.IdDocumento

                            listaAllegatiEXT = New Generic.List(Of Ext_FatturaAllegato)
                            listaAllegatiEXT.Add(allegatoExt)
                        Next

                        ext_fattura.ListaAllegati = listaAllegatiEXT
                    End If
                Next
            End If

            oDllDocumenti = Nothing

        Catch ex As Exception
            Log.Error(oOperatore.Codice & ":" & ex.Message)
        Finally
            GetDatiSchedaContrattiFatture = ext_SchedaContrattiFattureInfo
        End Try
        Return ext_SchedaContrattiFattureInfo
    End Function





End Module

Public Class WarningSet
    Dim warnings As New Generic.HashSet(Of String)

    Public Sub add(ByVal warning As WarningType)
        If Not warning Is Nothing Then
            warnings.Add(warning.Code)
        End If
    End Sub

    Public Function contains(ByVal warning As WarningType)
        Dim retValue As Boolean = False
        If Not warning Is Nothing Then
            retValue = warnings.Contains(warning.Code)
        End If
        Return retValue
    End Function

    Public Function containsOnly(ByVal warning As WarningType)
        Dim retValue As Boolean = False
        If Not warning Is Nothing Then
            retValue = warnings.Contains(warning.Code) AndAlso count() = 1
        End If
        Return retValue
    End Function

    Public Sub clear()
        warnings.Clear()
    End Sub

    Public Function count() As Integer
        Return warnings.Count()
    End Function
End Class

Public Class WarningType
    Dim _code As String = String.Empty

    Public Shared NO_WARNING As WarningType = New WarningType("wt_no_w")
    Public Shared NOT_CONFIRMED_OPERATIONS_WARNING As WarningType = New WarningType("wt_ncow")
    Public Shared NO_BENEFICIARI_IMPEGNO As WarningType = New WarningType("wt_no_ben_imp")
    Public Shared NO_BENEFICIARI_LIQUIDAZIONE As WarningType = New WarningType("wt_no_ben_liq")
    Public Shared SIGN_WARNING As WarningType = New WarningType("wt_sw")
    Public Shared OTHER_WARNINGS As WarningType = New WarningType("wt_ow")

    Private Sub New(ByVal code As String)
        Me.Code = code
    End Sub

    Public Property Code() As String
        Get
            Return _code
        End Get
        Private Set(ByVal value As String)
            _code = value
        End Set
    End Property

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return _code.Equals(obj.Code)
    End Function
End Class



