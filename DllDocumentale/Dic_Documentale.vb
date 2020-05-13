Option Strict Off
Option Explicit On
Public Module Dic_FODocumentale

    Public Enum Attributi_Documento
        CIG = 0
        CUP = 1
        PAGINE_TOTALI_ALLEGATI = 3
    End Enum

    'costanti per la chiamata a funzioni
     Public Const param_excel_riga As String = "@ExcelRiga"
    Public Const param_progressivo As String = "@Progressivo"
    Public Const param_id_Documento As String = "@IdDocumento"
    Public Const param_Prog_Contabilita As String = "@ProgContabilita"
    Public Const param_DataRegistrazione As String = "@DataRegistrazione"
    Public Const param_Operatore As String = "@Operatore"
    Public Const param_NContabile As String = "@NContabile"
    Public Const param_UPB As String = "@UPB"
    Public Const param_MissioneProgramma As String = "@MissioneProgramma"
    Public Const param_HashTokenCallSic As String = "@HashTokenCallSic"
    Public Const param_IdDocContabileSic As String = "@IdDocContabileSic"

    Public Const param_HashTokenCallSic_Imp As String = "@HashTokenCallSic_Imp"
    Public Const param_IdDocContabileSic_Imp As String = "@IdDocContabileSic_Imp"


    Public Const param_Cap As String = "@Cap"
    Public Const param_Esercizio As String = "@Esercizio"
    Public Const param_Costo As String = "@Costo"
    Public Const param_NAssunzione As String = "@NAssunzione"
    Public Const param_TipoAtto As String = "@TipoAtto"
    Public Const param_TipoAssunzione As String = "@TipoAssunzione"
    Public Const param_TipoAssunzioneDescr As String = "@TipoAssunzioneDesc"
    Public Const param_DataAss As String = "@Anno"
    Public Const param_NLiquidazione As String = "@NLiquidazione"
    Public Const param_AnnoImp As String = "@AnnoImp"
    Public Const param_NPreImp As String = "@NPreImp"
    Public Const param_ContoEconomica As String = "@ContoEconomica"
    Public Const param_Ratei As String = "@Ratei"
    Public Const param_Risconti As String = "@Risconti"
    Public Const param_ImpostaIrap As String = "@ImpostaIrap"
    Public Const param_isEconomia As String = "@isEconomia"
    Public Const param_Compito As String = "@Compito"
    Public Const param_CompitoSpecifico_Generico As String = "@CompitoSpecificoGenerico"
    Public Const param_unicita As String = "@unicita"
    Public Const param_Oggetto As String = "@Oggetto"
    Public Const param_Doc_FlagStampato As String = "@Doc_FlagStampato"
    Public Const param_Pubblico As String = "@Pubblico"
    Public Const param_Note As String = "@Note"
    Public Const param_IdSuggerimento As String = "@IdSuggerimento"

    Public Const param_Documento_ufficio_Competenza As String = "@ufficio"
    Public Const param_Documento_accountUtenti_Competenza As String = "@ufficio"

    Public Const param_PreImpDaPrenotazione As String = "@DaPrenotazione"
    Public Const param_Stato As String = "@Stato"
    Public Const param_IdImpegno As String = "@IdImpegno"
    Public Const param_NDocPrecedente As String = "@NDocPrecedente"
    Public Const param_ImportoIva As String = "@ImportoIva"
    Public Const param_Por As String = "@Por"
    Public Const param_AllegatoPor As String = "@Dli_AllegatoPor"
    Public Const param_Importo As String = "@Importo"
    Public Const param_NImpegno As String = "@NImpegno"
    Public Const param_NumRegistr As String = "@NumRegistr"
    Public Const param_CodObGest As String = "@CodObGest"
    Public Const param_PCF As String = "@PCF"
    Public Const param_certificatoUtente As String = "@CertificatoUtente"





    Public Const param_id_Allegato As String = "@IdAllegato"
    Public Const param_TipoAttivita As String = "@TipoAttivita"
    Public Const param_Utente As String = "@Utente"
    Public Const param_Versione As String = "@Versione"
    Public Const param_allegatoBinario As String = "@AllegatoBinario"
    Public Const param_nome As String = "@Nome"
    Public Const param_denominazione As String = "@denominazione"
    Public Const param_flag_persona_fisica As String = "@flagPersonaFisica"
    Public Const param_sesso As String = "@sesso"
    Public Const param_comune_nascita As String = "@comuneNascita"
    Public Const param_data_nascita As String = "@dataNascita"
    Public Const param_provincia_nascita As String = "@provinciaNascita"

    Public Const param_indirizzo_residenza As String = "@indirizzoResidenza"
    Public Const param_comune_residenza As String = "@comuneResidenza"
    Public Const param_cap_residenza As String = "@capResidenza"
    Public Const param_provincia_residenza As String = "@provinciaResidenza"
    Public Const param_stato_residenza As String = "@statoResidenza"
    Public Const param_sigla_nazione_residenza As String = "@siglaNazioneResidenza"
    Public Const param_codice_fiscale As String = "@codiceFiscale"
    Public Const param_partita_iva As String = "@partitaIva"
    Public Const param_iban As String = "@iban"
    Public Const param_id_modalita_pagamento As String = "@idModalitaPagamento"
    Public Const param_desrizione_operazione_sic As String = "@descrizioneOperazioneSic"
    Public Const param_tiplogia_pagamento_nome As String = "@tipologiaPagamentoNome"
    Public Const param_id_anagrafica_sic As String = "@idAnagraficaSic"
    Public Const param_genera_liquidazione_contestuale As String = "@generaLiquidazioneContestuale"
    Public Const param_grouped_by As String = "@grouped_by"
    Public Const param_id_sede_sic As String = "@idSedeSic"
    Public Const param_indirizzo_sede As String = "@indirizzoSede"
    Public Const param_comune_sede As String = "@comuneSede"
    Public Const param_cap_sede As String = "@capSede"
    Public Const param_nome_sede As String = "@nomeSede"
    Public Const param_id_conto_corrente_sic As String = "@idContoCorrenteSic"
    Public Const param_is_modalita_principale As String = "@isModalitaPrincipale"
    Public Const param_is_dato_sensibile As String = "@isDatoSensibile"
    Public Const param_cognome_lr As String = "@cognomeLr"
    Public Const param_nome_lr As String = "@nomeLr"
    Public Const param_sesso_lr As String = "@sessoLr"
    Public Const param_comune_nascita_lr As String = "@comuneNascitaLr"
    Public Const param_data_nascita_lr As String = "@dataNascitaLr"
    Public Const param_indirizzo_residenza_lr As String = "@indirizzoResidenzaLr"
    Public Const param_comune_residenza_lr As String = "@comuneResidenzaLr"
    Public Const param_cap_residenza_lr As String = "@capResidenzaLr"
    Public Const param_codice_fiscale_lr As String = "@codiceFiscaleLr"
    Public Const param_estero As String = "@estero"
    Public Const param_cig As String = "@cig"
    Public Const param_cup As String = "@cup"
    Public Const param_data_caricamento As String = "@dataCaricamento"
   
  
        
    Public Const param_estensione As String = "@estensione"
    Public Const param_firmato As String = "@Firmato"

    Public Const param_Struttura As String = "@Struttura"
    Public Const param_NMandato As String = "@Nmandato"
    Public Const param_Ente As String = "@ente"
    Public Const param_flusso As String = "@flusso"
    Public Const param_Valore As String = "@valore"
    Public Const param_codAttributo As String = "@cod_Attributo"

    ' parametri per tabella StatoIstanzaDocumento 
    Public Const param_CodiceUfficio As String = "@CodiceUfficio"
    Public Const param_DataUltimaOperazione As String = "@DataUltimaOperazione"
    Public Const param_Ruolo As String = "@Ruolo"
    Public Const param_LivelloUfficio As String = "@LivelloUfficio"
    Public Const param_TipoDocumento As String = "@TipoDocumento"
    Public Const param_IdRettifica As String = "@IdRettifica"



    Public Const cfo_Crea_Determina As Short = 10
    Public Const cfo_Elenco_Documenti As Short = 11
    Public Const cfo_Elenco_Monitor As Short = 12
    Public Const cfo_Storico_Documento As Short = 13
    Public Const cfo_Registra_Attivita As Short = 14
    Public Const cfo_Passo_Determina As Short = 15
    Public Const cfo_Registra_Documento As Short = 16
    Public Const cfo_Leggi_Documento As Short = 17
    Public Const cfo_Elenco_Messaggi As Short = 18
    Public Const cfo_Leggi_Messaggio As Short = 19
    Public Const cfo_Cancella_Messaggio As Short = 20
    Public Const cfo_Elenco_Allegati As Short = 21
    Public Const cfo_Leggi_Allegato As Short = 22
    Public Const cfo_Registra_Allegato As Short = 23
    Public Const cfo_Elenco_DocumentiUfficio As Short = 24
    Public Const cfo_Verifica_Prima_Apertura As Short = 25
    Public Const cfo_Crea_Delibera As Short = 26
    Public Const cfo_Registra_Compito As Short = 27
    Public Const cfo_Passo_Delibera As Short = 28
    Public Const cfo_Elenco_Lettere As Short = 29
    Public Const cfo_Registra_Lettera As Short = 30
    Public Const cfo_Cancella_Allegato As Short = 31
    Public Const cfo_Seduta_Giunta As Short = 32
    Public Const cfo_Aggiorna_Stato_Lettera As Short = 33
    Public Const cfo_Crea_Disposizione As Short = 34
    Public Const cfo_Passo_Disposizione As Short = 35
    Public Const cfo_Info_HomePage As Short = 36
    Public Const cfo_Elenco_Compiti_Documento As Short = 37
    'modgg 10-06 3
    Public Const cfo_Aggiorna_Documento As Short = 38
    Public Const cfo_Verifica_Ultima_Azione_Ufficio As Short = 40
    Public Const cfo_Abilita_RigettoInoltro As Short = 41

    Public Const cfo_Elenco_Allegati_Da_Stampare As Short = 42

    Public Const cfo_Elenco_Osservazioni_Documento As Short = 43

    Public Const cfo_ListaFirmeDocumento As Short = 44
    Public Const cfo_Cancella_Allegato_Fisicamente As Short = 45

    Public Const cfo_Verifica_Azione_Ufficio As Short = 47

    Public Const cfo_Aggiorna_Appendice As Short = 48
    Public Const cfo_Update_compiti_Specifici_to_Generici As Short = 49
    Public Const cfo_Registra_Osservazione As Short = 50

    Public Const cfo_Verifica_Firma_Utente As Short = 51
    Public Const cfo_Elenco_Documenti_Da_Stampare As Short = 52
    Public Const cfo_Cancella_AllegatiTemporanei As Short = 53

    Public Const cfo_Crea_AltroAtto As Short = 54
    Public Const cfo_Passo_AltroAtto As Short = 55
    Public Const cfo_Registra_Allegato_Ex As Short = 120

    Public Const cfo_Registra_Documento_Object As Short = 1600
    Public Const cfo_Leggi_Documento_Object As Short = 1700
   

    Public Const cfo_Documento_Conservazione_Allegato_Marca_Desc_Tipo As String = "MARCA_TEMPORALE"
    Public Const cfo_Documento_Conservazione_Allegato_Marca_Ext_Tipo As String = "tsr"
    Public Const cfo_Documento_Conservazione_Allegato__Marca_Nome As String = "Marca_Temporale"


    Public Const cfo_Documento_Conservazione_Applicazione As String = "PROVVEDIMENTI"

    Public Const cfo_Conservazione_Stato_LIVE As String = "LIVE" 
    Public Const cfo_Conservazione_Stato_WORK As String = "WORK"
    Public Const cfo_Conservazione_Stato_ERROR As String = "ERROR"
    Public Const cfo_Conservazione_Stato_ERROR_WS As String = "ERROR_WS"
    Public Const cfo_Conservazione_Stato_DOCUMENT_READY As String = "DOCUMENT_READY"
    Public Const cfo_Conservazione_Stato_DA_MARCARE As String = "DA_MARCARE"


    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Documento_Object As Short = 0


    Public Enum vc_Conservazione_Stato
        c_work = 1
        c_live = 2
        c_error = 3
        c_error_ws = 4
    End Enum

    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Documento_Object
        c_Documento_Info = 0
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_Crea_Determina
    Public Const dimvc_Crea_Determina As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Crea_Determina
        c_cod_ufficio_proponente = 0
        c_utente_creazione = 1
        c_data_creazione = 2
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_Crea_Determina
    Public Const dimvc_Crea_Delibera As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Crea_Delibera
        c_cod_ufficio_proponente = 0
        c_utente_creazione = 1
        c_data_creazione = 2
    End Enum

    Public Const dimvc_Crea_Disposizione As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Crea_Disposizione
        c_cod_ufficio_proponente = 0
        c_utente_creazione = 1
        c_data_creazione = 2
    End Enum

    Public Const dimvc_Crea_AltroAtto As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Crea_AltroAtto
        c_cod_ufficio_proponente = 0
        c_utente_creazione = 1
        c_data_creazione = 2
        c_tipo_atto = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Lettere As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Lettere
        c_utente = 0
        c_tipoLettera = 1
        c_statoLettera = 2
        c_tipoDocumento = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Lettera As Short = 3 'rc 08 02 2006
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Lettera
        c_idDocumenti = 0
        c_ufficio = 1
        c_tipolettera = 2
        c_testo = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Attivita As Short = 9
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Attivita
        c_id_documento = 0
        c_utente = 1
        c_data = 2
        c_tipoAttivita = 3
        c_info_attivita = 4
        c_id_allegato = 5
        c_ufficio = 6
        c_nominativo = 7
        c_livelloufficio = 8
        c_livelloRuoloufficio = 9
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Monitor As Short = 20
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Monitor
        c_utente = 0
        c_tipoDocumento = 1
        'rc 08 02 2006
        c_progressivo_utente = 2
        c_data_inizio = 3
        c_data_fine = 4
        c_oggetto = 5
        c_cod_ufficio = 6
        c_descr_ufficio = 7
        c_cod_dip = 8
        c_numero_doc = 9
        c_uffici_consultabili = 10
        c_tipo_Rigetto = 11
        c_tipologia_ricerca_beneficiario = 12
        c_beneficiario = 13
        c_codiceCUP = 14
        c_codiceCIG = 15
        c_id_tipologia_documento = 16
        c_autorizzazione_pubblicazione = 17
        c_tipologia_ricerca_destinatario = 18
        c_destinatario = 19
        c_tipo_data = 20
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Storico_Documento As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Storico_Documento
        c_idDocumento = 0
        c_codUfficioProponente = 1
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Passo_Determina As Short = 8
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Passo_Determina
        c_idDocumento = 0
        c_utente = 1
        c_azione = 2
        c_prossimoAttore = 3
        c_datiXml = 4
        c_ufficioUtente = 5
        c_note = 6
        c_suggerimento = 7
        c_flagUrgente = 8
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Passo_Delibera As Short = 7
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Passo_Delibera
        c_idDocumento = 0
        c_utente = 1
        c_azione = 2
        c_prossimoAttore = 3
        c_datiXml = 4
        c_ufficioUtente = 5
        c_note = 6
        c_suggerimento = 7
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Passo_AltriAtti As Short = 8
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Passo_AltriAtti
        c_idDocumento = 0
        c_utente = 1
        c_azione = 2
        c_prossimoAttore = 3
        c_datiXml = 4
        c_ufficioUtente = 5
        c_note = 6
        c_suggerimento = 7
        c_tipoAtto = 8
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Passo_Disposizione As Short = 8
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Passo_Disposizione
        c_idDocumento = 0
        c_utente = 1
        c_azione = 2
        c_prossimoAttore = 3
        c_datiXml = 4
        c_ufficioUtente = 5
        c_note = 6
        c_suggerimento = 7
        c_flagUrgente = 8
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Documento As Short = 13
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Documento
        c_idDocumento = 0
        c_doc_Oggetto = 1
        c_testo = 2
        c_utente = 3
        c_pub_integrale = 4
        c_xmlDatiDocumento = 5
        c_xslModelloDocumento = 6
        c_isContabile = 7
        c_tipoOpContabili = 8
        c_flagPrivacy = 9
        c_Cod_Cup = 10
        c_Cod_Applicazione = 11
        c_cod_doc_Esterno = 12
        c_cod_Investimento_Pub = 13
        c_cod_CaricamentoTesto = 14
    End Enum




    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Leggi_Documento As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Leggi_Documento
        c_idDocumento = 0
        c_xmlDatiDocumento = 1
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvr_Leggi_Documento As Short = 9
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vr_Leggi_Documento
        c_oggetto = 0
        c_testo = 1
        c_vuoto = 2
        c_numUtenteDocumento = 3
        c_compitoUtente = 4
        c_pubIntegrale = 5
        c_isContabile = 6
        c_Data = 7
        c_CodProponente = 8
        c_DescProponente = 9
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Messaggi As Short = 3 'rc 08 02 2006
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Messaggi
        c_destinatario = 0
        'rc 08 02 2006
        c_letto = 1
        c_IdDocumento = 2
        c_inviati = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Leggi_Messaggio As Short = 0
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Leggi_Messaggio
        c_idMessaggio = 0
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Cancella_Messaggio As Short = 4
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Cancella_Messaggio
        c_idMessaggio = 0
        c_mittente = 1
        c_destinatario = 2
        c_testo = 3
        c_cancella = 4
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Allegati As Short = 4
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Allegati
        c_idDocumento = 0
        c_autore = 1
        c_tipoAllegati = 2
        c_allDocumento = 3
        c_daStampare = 4
    End Enum


    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Osservazioni_Documento As Short = 0
    'enum per le posizioni delle variabili per la chiamata a FO_Elenco_Osservazioni_Documento
    Public Enum vc_Elenco_Osservazioni_Documento
        c_idDocumento = 0
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Leggi_Allegato As Short = 4
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Leggi_Allegato
        c_idDocumento = 0
        c_idAllegato = 1
        c_tipologiaAllegato = 2
        c_p7m = 3
        c_tsr = 4

    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Allegato As Short = 17
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Allegato
        c_idDocumento = 0
        c_idAllegato = 1
        c_codTipo = 2
        c_descTipo = 3
        c_binarioAllegato = 4
        c_nome = 5
        c_codEstensione = 6
        c_descEstensione = 7
        c_firmato = 8
        c_autore = 9
        c_copiaFirmatoDoc = 10
        c_versioneAllegato = 11
        c_destinatari = 12
        c_modalita = 13
        c_controlloIstanza = 14
        c_flagRegistraAttivita = 15
        c_riferimento_Appendice = 16
        c_flag_Trasforma_In_Pdf = 17
    End Enum


    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Aggiorna_appendice As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Aggiorna_appendice
        c_idAllegato = 0
        c_binarioAllegato = 1
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Cancella_Allegato As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Cancella_Allegato
        c_idAllegato = 1
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Documenti As Short = 27
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Documenti
        c_utente = 0
        c_tipoDocumento = 1
        c_ufficio = 2
        c_data_inizio = 3
        c_data_fine = 4
        c_oggetto = 5
        c_cod_ufficio = 6
        c_ufficio_creazione = 7
        c_cod_dip = 8
        c_numero_doc = 9
        c_tipo_Rigetto = 10
        c_ufficio_competenza = 11
        c_idDocumento = 12
        c_FlagStampato = 13
        c_suggerimento = 14
        c_tipologia_ricerca_beneficiario = 15
        c_beneficiario = 16
        c_codiceCUP = 17
        c_codiceCIG = 18
        c_visualizzaUrgenti = 19
        c_visualizzaNonTrasp = 20
        c_id_tipologia_documento = 21
        c_autorizzazione_pubblicazione = 22
        c_tipologia_ricerca_destinatario = 23
        c_destinatario = 24
        c_visualizzaAnnullati = 25
        c_utente_competenza = 26
  	c_tipo_data = 27
    End Enum
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_DocumentiUfficio As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_DocumentiUfficio
        c_tipoDocumento = 0
        c_ufficio = 1
    End Enum

    'dimensione del vettore parametri per il ritorno a fo_
    Public Const dimvr_Elenco_Worklist As Short = 0
    Public Enum vr_Elenco_Worklist
        c_idDocumento = 0
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Verifica_Prima_Apertura As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Verifica_Prima_Apertura
        c_idDocumento = 0
        c_utente = 1
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Compito As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Compito
        c_idDocumento = 0
        c_utente = 1
        c_compito = 2
        c_certificato = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Seduta_Giunta As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Seduta_Giunta
        c_attesa = 0
        c_data = 1
    End Enum

    Public Const dimcv_Controllo_Liquidazione As Short = 4
    Public Enum vc_Controllo_Liquidazione
        c_totale_impegno = 0
        c_totale_liquidazione = 1
        c_totale_rag_assunzione = 2
        c_totale_rag_liquidazione = 3
        c_totale_rag_prenotazione = 4
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Info_HomePage As Short = 0
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Info_HomePage
        c_utente = 0
    End Enum

    'rc 28 02
    Public Const dimvr_Info_HomePage As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vr_Info_HomePage
        c_numDoc_TipoDocumento = 0
        c_numDocDeposito_TipoDocumento = 1
        c_numNuoviMessaggi = 2
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Elenco_Compiti_Documento As Short = 1
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Elenco_Compiti_Documento
        c_idDocumento = 0
        c_NomeCognome = 1
    End Enum

    Public Const dimvr_Elenco_Compiti_Documento As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vr_Elenco_Compiti_Documento
        c_utente = 0
        c_nominativoUtente = 1
        c_compito = 2
        c_descrizioneCompito = 3
    End Enum
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Registra_Osservazione As Short = 3
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Registra_Osservazione
        c_idDocumento = 0
        c_operatore = 1
        c_Testo = 2
        c_Tipologia = 3
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    'Public Const dimvc_ As Short = 0
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    'Public Enum vc_
    '    c_ = 0
    'End Enum

    Public Enum vFunzioniPassoDelibera
        cf_DELIBERA_ANNULLO = 300
        cf_DELIBERA_ASSEGNA = 301
        cf_DELIBERA_PRELAZIONE = 302
        cf_DELIBERA_DEPOSITO_PRELIEVO = 303
        cf_DELIBERA_COLLABORATORE_INOLTRO = 304
        cf_DELIBERA_SUPERVISORE_INOLTRO = 305
        cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO = 306
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO = 307
        cf_UFFICIO_POLITICO_RESPONSABILE_INOLTRO = 308
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO = 309
        cf_UFFICIO_SEGRETERIA_RESPONSABILE_INOLTRO = 310
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO = 311
        cf_UFFICIO_POLITICO_RESPONSABILE_RIGETTO = 312
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO = 313
        cf_UFFICIO_SEGRETERIA_RESPONSABILE_RIGETTO = 314
        cf_UFFICIO_POLITICO_RESPONSABILE_INOLTRO_2 = 315
        cf_DELIBERA_4LIVELLO_INOLTRO = 316
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO = 317
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_INOLTRO = 318
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_RIGETTO = 319
        cf_UFFICIO_PRESIDENZA_RESPONSABILE_INOLTRO = 320
        cf_UFFICIO_PRESIDENZA_RESPONSABILE_RIGETTO = 321
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_INOLTRO = 322
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_RIGETTO = 323
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_INOLTRO = 324
        cf_UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_RIGETTO = 325

    End Enum

    Public Enum vFunzioniPassoDetermina
        cf_DETERMINA_COLLABORATORE_INOLTRO = 200
        cf_DETERMINA_ANNULLO = 201
        cf_DETERMINA_DEPOSITO_PRELIEVO = 202
        cf_DETERMINA_SUPERVISORE_INOLTRO = 203
        cf_DETERMINA_PRELAZIONE = 205
        cf_DETERMINA_ASSEGNA = 206
        cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO = 208
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO = 222
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO = 227
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO = 236
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO = 241
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO = 244
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO = 245
        cf_DETERMINA_4LIVELLO_INOLTRO = 246

    End Enum

    Public Enum vFunzioniPassoAltriAtti
        cf_COLLABORATORE_INOLTRO = 500
        cf_DETERMINA_ANNULLO = 501
        cf_DEPOSITO_PRELIEVO = 502
        cf_SUPERVISORE_INOLTRO = 503
        cf_PRELAZIONE = 505
        cf_DETERMINA_ASSEGNA = 506
        cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO = 508
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO = 522
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO = 527
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO = 536
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO = 541
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO = 544
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO = 545
        cf_DETERMINA_4LIVELLO_INOLTRO = 546

    End Enum

    Public Enum vFunzioniPassoDisposizione
        cf_DISPOSIZIONE_COLLABORATORE_INOLTRO = 400
        cf_DISPOSIZIONE_ANNULLO = 401
        cf_DISPOSIZIONE_DEPOSITO_PRELIEVO = 402
        cf_DISPOSIZIONE_SUPERVISORE_INOLTRO = 403
        cf_DISPOSIZIONE_PRELAZIONE = 405
        cf_DISPOSIZIONE_ASSEGNA = 406
        cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO = 407
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO = 422
        cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO = 427
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO = 436
        cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO = 441
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO = 444
        cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO = 445
    End Enum

    Public Enum vFunzioniPassoDocumento
        cf_MESSAGGIO_INOLTRO = 150
        cf_LETTERA_ACCOMPAGNAMENTO = 151
        cf_VERIFICA_FIRMA_INOLTRO = 152
        cf_VERIFICA_FIRMA_RIGETTO = 153
        cf_ATTIVA_SCADENZA = 154
        cf_DISATTIVA_SCADENZA = 155
        cf_LETTERE_DOCUMENTI = 156
        cf_MESSAGGIO_ARCHIVIO = 157
        cf_SEGNA_MESSAGGIO_COME_VISTO_DOPO_INOLTRO = 158
        cf_ASSEGNA_NUMERAZIONE_DEFINITIVA = 159
        cf_ASSEGNA_NUMERAZIONE_CRONOLOGIA = 160

        cf_MESSAGGIO_ARCHIVIO_COMPETENZA = 161

        cf_UPDATE_DOCUMENTO_CONSERVAZIONE_ATTO = 248
    End Enum
    'modgg 10-06 3
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Aggiorna_Documento As Short = 4
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Aggiorna_Documento
        c_idDocumento = 0
        c_ValoreCampoAggiornare = 1
        c_CampoDaAggiornare = 2
        c_avviaTransazione = 3
        c_Db = 4
    End Enum
    Public Const dimvc_isContabile As Short = 0
    Public Enum vc_isContabile
        c_isContabile = 0
    End Enum
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Cancella_Allegato_Fisicamente As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_Crea_Determina
    Public Enum vc_Cancella_Allegato_Fisicamente
        c_idDocumento = 0
        c_idAllegato = 1
        c_tipologiaAllegato = 2
    End Enum

    Public Const dimvc_ListaFirmeDoc As Short = 2
    Public Enum vc_ListaFirmeDoc
        c_idDocumento = 0
        c_UltimaVersioneAllegato = 1
        c_ListaUtenze = 2
    End Enum
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Cancella_Istanza_WF As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_Cancella_Istanza_WF
    Public Enum vc_Cancella_Istanza_WF
        c_idDocumento = 0
        c_utente = 1
        c_tipologiaDocumento = 2
    End Enum

    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Verifica_Ultima_Azione_Ufficio As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_vc_Verifica_Ultima_Azione_Ufficio
    Public Enum vc_Verifica_Ultima_Azione_Ufficio
        c_idDocumento = 0
        c_parm = 1
    End Enum
    'dimensione del vettore parametri per la chiamata a fo_
    Public Const dimvc_Verifica_Azione_Ufficio As Short = 2
    'enum per le posizioni delle variabili per la chiamata a fo_vc_Verifica_Ultima_Azione_Ufficio
    Public Enum vc_Verifica_Azione_Ufficio
        c_ufficio = 0
        c_progressivo = 1
    End Enum

End Module
