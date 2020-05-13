var ADD_LIST_RESULT = { "ok": 0, "exists": 1,
    "error": 2
};

function isPubblicazioneYesSelected() {
    return Ext.getCmp('yesPubblicazione') != undefined && Ext.getCmp('yesPubblicazione').checked == true;
}

function isDeliberaSelectedAndValid() {
    return Ext.getCmp('DELIBERA') != undefined && Ext.getCmp('DELIBERA').checked &&
           Ext.getCmp('annoDelibera').isValid() && Ext.getCmp('numeroDelibera').isValid();
}

function isDeterminaSelectedAndValid() {
    return Ext.getCmp('DETERMINA') != undefined && Ext.getCmp('DETERMINA').checked &&
           Ext.getCmp('annoDetermina').isValid() && Ext.getCmp('numeroDetermina').isValid() &&
           Ext.getCmp('ufficioDetermina').isValid();
}

function isAltroSelected() {
    return Ext.getCmp('ALTRO') != undefined && Ext.getCmp('ALTRO').checked;
}

function validateSchedaLeggeTrasparenzaFields(panelSchedaId) {
    var retValue = null;

    if (!Ext.getCmp(panelSchedaId).disabled) {
        var isValid = false;
        var errorMsg = '';

        if ((Ext.getCmp('yesPubblicazione') == undefined || Ext.getCmp('yesPubblicazione').checked == false) &&
            (Ext.getCmp('noPubblicazione') == undefined || Ext.getCmp('noPubblicazione').checked == false)) {
            errorMsg += "E' necessario specificare se le informazioni della scheda sono o meno soggette a pubblicazione.<br/>";
        } else if (Ext.getCmp('noPubblicazione') != undefined && Ext.getCmp('noPubblicazione').checked == true) {
            if (Ext.getCmp('noteAutorizzazionePubblicazione') == undefined || !Ext.getCmp('noteAutorizzazionePubblicazione').validate())
                errorMsg += "Verificare che il motivo per il quale non si autorizza la pubblicazione delle informazioni presenti nella scheda sia specificato (max 1024 caratteri).<br/>";
            else
                isValid = true;
        } else if ((Ext.getCmp('DETERMINA') != undefined && Ext.getCmp('DETERMINA').checked && !validateDatiDetermina()) ||
                   (Ext.getCmp('DELIBERA') != undefined && Ext.getCmp('DELIBERA').checked && !validateDatiDelibera()) ||
                   (Ext.getCmp('ALTRO') != undefined && Ext.getCmp('ALTRO').checked &&
                    (Ext.getCmp('normaAttribuzioneAltroDescrizione') == undefined || !Ext.getCmp('normaAttribuzioneAltroDescrizione').validate()))) {
            errorMsg += "Verificare che le informazioni fornite per la norma o l'atto in base al quale viene attribuito il beneficio siano specificate e valide (max 1024 caratteri).<br/>";
        } else if ((Ext.getCmp('ufficioResponsabileDelProcedimento') == undefined || !Ext.getCmp('ufficioResponsabileDelProcedimento').validate())) {
            errorMsg += "E' necessario specificare l'ufficio responsabile del procedimento.";
        } else if ((Ext.getCmp('funzionarioResponsabileDelProcedimento') == undefined || !Ext.getCmp('funzionarioResponsabileDelProcedimento').validate())) {
            errorMsg += "E' necessario specificare il responsabile del procedimento.";
        } else if ((Ext.getCmp('modalitaIndividuazioneBeneficiario') == undefined || !Ext.getCmp('modalitaIndividuazioneBeneficiario').validate())) {
            errorMsg += "E' necessario specificare la modalità di individuazione del beneficiario/destinatario.<br/>";
        } else if ((Ext.getCmp('determinaErrorFlag') != undefined && Ext.getCmp('determinaErrorFlag').text == 'true' && Ext.getCmp('DETERMINA') != undefined && Ext.getCmp('DETERMINA').checked) ||
                   (Ext.getCmp('deliberaErrorFlag') != undefined && Ext.getCmp('deliberaErrorFlag').text == 'true' && Ext.getCmp('DELIBERA') != undefined && Ext.getCmp('DELIBERA').checked)) {
            errorMsg += "Verificare che le informazioni fornite per la norma o l'atto in base al quale viene attribuito il beneficio siano specificate e valide.<br/>";
        } else
            isValid = true;

        if ((Ext.getCmp('contenutoAtto') == undefined || !Ext.getCmp('contenutoAtto').validate())) {
            errorMsg += "Verificare che il contenuto dell'atto sia specificato (max 1000 caratteri).<br/>";
            isValid = false;
        }
        if (!isValid) {
            errorMsg = "Scheda 'Legge Trasparenza' incompleta.<br/>" + errorMsg;
            retValue = { tab_to_activate: panelSchedaId, msg: errorMsg };
        }
    }

    return retValue;
}

function resetSchedaLeggeTrasparenzaFields() {
    if (Ext.getCmp('yesPubblicazione') != undefined) {
        Ext.getCmp('yesPubblicazione').reset();
    }

    if (Ext.getCmp('noPubblicazione') != undefined) {
        Ext.getCmp('noPubblicazione').reset();
    }

    resetPubblicazioneSelectionFields();

    if (Ext.getCmp('DETERMINA') != undefined)
        Ext.getCmp('DETERMINA').reset();

    if (Ext.getCmp('DELIBERA') != undefined)
        Ext.getCmp('DELIBERA').reset();

    if (Ext.getCmp('ALTRO') != undefined)
        Ext.getCmp('ALTRO').reset();

    resetNormaSelectionFields();

    if (Ext.getCmp('ufficioResponsabileDelProcedimento') != undefined)
        Ext.getCmp('ufficioResponsabileDelProcedimento').reset();

    if (Ext.getCmp('funzionarioResponsabileDelProcedimento') != undefined)
        Ext.getCmp('funzionarioResponsabileDelProcedimento').reset();

    if (Ext.getCmp('modalitaIndividuazioneBeneficiario') != undefined)
        Ext.getCmp('modalitaIndividuazioneBeneficiario').reset();

}


function resetPubblicazioneSelectionFields() {
    if (Ext.getCmp('noteAutorizzazionePubblicazione') != undefined)
        Ext.getCmp('noteAutorizzazionePubblicazione').reset();
}

function buildSchedaLeggeTrasparenzaPanel(schedaLeggeTrasparenzaInfo) {
    var yesOption = new Ext.form.Radio({
        boxLabel: 'Si',
        id: 'yesPubblicazione',
        name: 'selezionePubblicazione',
        inputValue: 'SI',
        checked: true
    });

    var noOption = new Ext.form.Radio({
        boxLabel: 'No',
        id: 'noPubblicazione',
        name: 'selezionePubblicazione',
        inputValue: 'NO',
        checked: false
    });

    yesOption.on('check', function() {
        if (Ext.getCmp('yesPubblicazione').checked) {
            enableFieldsForAutorizzazionePubblicazioneToYes(true);
            showFieldsForAutorizzazionePubblicazioneToYes(true);
//            enableDatiContratto(true);
//            showDatiContratto(true);
            enableContenutoAtto(true);
            showContenutoAtto(true);
        } else {
            enableFieldsForAutorizzazionePubblicazioneToYes(false);
            showFieldsForAutorizzazionePubblicazioneToYes(false);
        }
    });

    noOption.on('check', function() {
        if (Ext.getCmp('noPubblicazione').checked) {
            enableFieldsForAutorizzazionePubblicazioneToNo(true);
            showFieldsForAutorizzazionePubblicazioneToNo(true);
//            enableDatiContratto(true);
//            showDatiContratto(true);
            enableContenutoAtto(true);
            showContenutoAtto(true);
        } else {
            enableFieldsForAutorizzazionePubblicazioneToNo(false);
            showFieldsForAutorizzazionePubblicazioneToNo(false);
        }
    });

    var panelSchedaLeggeTrasparenza = new Ext.Panel({
        xtype: "panel",
        id: 'panelSchedaLeggeTrasparenza',
        layout: 'table',
        border: false,
        autoScroll: true,
        layoutConfig: {
            columns: 2
        },
        style: {
            "margin-top": "5px",
            "margin-bottom": "8px",
            "margin-left": "10px"
        },
        items: [{
            xtype: 'label',
            id: 'autorizzazionePubblicazioneLabel',
            name: 'autorizzazionePubblicazioneLabel',
            html: "<div style='width:605px;padding-right:25px;text-align:justify;'><b>Pubblicare le informazioni</b> " +
            "previste dal D.Lgs 33/2013, " +
            "relative al nome dell'impresa o altro soggetto beneficiario ed i suoi dati fiscali, " +
            "l’importo e la norma o il titolo a base dell’attribuzione del beneficio, l’ufficio e il funzionario o dirigente responsabile " +
            "del relativo procedimento amministrativo, " +
            "la modalità seguita per l’individuazione del beneficiario e il contratto e capitolato della prestazione, fornitura o servizio (*)</div>",
            style: 'margin-right:50px'
        },
        {
            xtype: 'panel',
            border: false,
            layout: 'table',
            layoutConfig: {
                columns: 3
            },
            items: [yesOption,
                {
                    xtype: 'label',
                    style: 'padding-right:10px',
                    text: ''
                },
                noOption]
        },
            {
                xtype: 'linkbutton', id: 'ulterioriInformazioniId', text: 'Ulteriori informazioni', style: 'margin-top: 10px',
                tooltip: "Ulteriori informazioni sulla normativa alla base della 'Scheda Trasparenza'",
                listeners: {
                    click: function() {
                        // showInfoSchedaLeggeTrasparenza();                    
                        window.open("./risorse/trasparenza/Allegato_obblighi_di_pubblicazione_05052014.pdf", "_blank")
                    }
                }
            }
        ]
    });

    var panelDataSchedaLeggeTrasparenza = new Ext.Panel({
        xtype: "panel",
        title: "",
        id: 'panelDataSchedaLeggeTrasparenza',
        layout: 'table',
        layoutConfig: {
            columns: 2
        },
        border: false,
        width: 720,
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        items: [{
            xtype: 'label',
            text: 'Note per mancata pubblicazione (*)',
            style: "margin-right:25px",
            id: 'noteAutorizzazionePubblicazioneLabel',
            hidden: true
        }, {
            xtype: 'textarea',
            maxLength: 1024,
            allowBlank: false,
            name: 'noteAutorizzazionePubblicazione',
            id: 'noteAutorizzazionePubblicazione',
            style: 'margin-top:' + (Ext.isIE ? '12px;margin-bottom:4px' : '12px'),
            width: 500,
            maxLengthText: 'La lunghezza massima è di 1024 caratteri',
            hidden: true,
            validator: function(value) {
                return value != null && value != undefined && value.trim().length != 0;
            },
            invalidText: "Campo obbligatorio"
        }, {
            xtype: 'label',
            id: 'normaAttribuzioneBeneficioLabel',
            text: "Norma o atto fonte del provvedimento (*) "
        },
            buildPanelNormaAttribuzioneBeneficio(),
         {
             xtype: 'label',
             id: 'ufficioResponsabileDelProcedimentoLabel',
             text: 'Ufficio responsabile del procedimento (*) '
         }, {
             xtype: 'textfield',
             name: 'ufficioResponsabileDelProcedimento',
             id: 'ufficioResponsabileDelProcedimento',
             style: 'margin-top:0px',
             maxLength: 250,
             width: Ext.isIE ? 502 : 500,
             allowBlank: false,
             blankText: 'Campo Obbligatorio',
             maxLengthText: 'La lunghezza massima è di 250 caratteri',
             validator: function(value) {
                 return value != null && value != undefined && value.trim().length != 0;
             },
             invalidText: "Campo obbligatorio"
         }
        ,
         {
             xtype: 'label',
             id: 'funzionarioResponsabileDelProcedimentoLabel',
             text: 'Responsabile del procedimento (*) '
         }, {
             xtype: 'textfield',
             name: 'funzionarioResponsabileDelProcedimento',
             id: 'funzionarioResponsabileDelProcedimento',
             style: 'margin-top:0px',
             maxLength: 250,
             width: Ext.isIE ? 502 : 500,
             allowBlank: false,
             blankText: 'Campo Obbligatorio',
             maxLengthText: 'La lunghezza massima è di 250 caratteri',
             validator: function(value) {
                 return value != null && value != undefined && value.trim().length != 0;
             },
             invalidText: "Campo obbligatorio"
         }
         ,
         {
             xtype: 'label',
             id: 'modalitaIndividuazioneBeneficiarioLabel',
             text: 'Modalità di individuazione del beneficiario/destinatario (*)'
         }, {
             xtype: 'textfield',
             name: 'modalitaIndividuazioneBeneficiario',
             id: 'modalitaIndividuazioneBeneficiario',
             style: Ext.isIE ? 'margin-top:-1px' : '',
             maxLength: 250,
             width: Ext.isIE ? 502 : 500,
             allowBlank: false,
             blankText: 'Campo Obbligatorio',
             maxLengthText: 'La lunghezza massima è di 250 caratteri',
             validator: function(value) {
                 return value != null && value != undefined && value.trim().length != 0;
             },
             invalidText: "Campo obbligatorio"
         }, {
             xtype: 'label',
             text: 'Sintesi contenuto atto (*)',
             style: "margin-right:25px",
             id: 'contenutoAttoLabel',
             hidden: true
         }, {
             xtype: 'textarea',
             maxLength: 1000,
             allowBlank: false,
             name: 'contenutoAtto',
             id: 'contenutoAtto',
             style: 'margin-top:' + (Ext.isIE ? '12px;margin-bottom:4px' : '12px'),
             width: 500,
             maxLengthText: 'La lunghezza massima è di 1000 caratteri',
             hidden: true,
             validator: function(value) {
                 return value != null && value != undefined && value.trim().length != 0;
             },
             invalidText: "Campo obbligatorio"
         }
//         , {
//             xtype: 'label',
//             id: 'contrattoLabel',
//             text: 'Contratto/i'
//         },
//            buildPanelContratti()
         ]
    });

    var panelContenitore = new Ext.Panel({
        xtype: "panel",
        id: 'panelContenitore',
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        width: 765,
        border: false,
        items: [
                panelSchedaLeggeTrasparenza,
                panelDataSchedaLeggeTrasparenza,
                 {
                     xtype: 'label',
                     html: "<div style='margin-left:10px;margin-top:15px'><i>I campi contrassegnati con (*) sono obbligatori.</i></div>"
                 }
            ]
    });

    Ext.getCmp('ufficioResponsabileDelProcedimento').setValue(Ext.get('descrizioneUffProp').getValue());
    Ext.getCmp('funzionarioResponsabileDelProcedimento').setValue(Ext.get('responsabileUffProp').getValue());

    showFieldsForAutorizzazionePubblicazioneToNo(false);
    showFieldsForAutorizzazionePubblicazioneToYes(true);
//    showDatiContratto(true);
    showContenutoAtto(true);
    enableFieldsForAutorizzazionePubblicazioneToNo(false);
    enableFieldsForAutorizzazionePubblicazioneToYes(true);
//    enableDatiContratto(true);
    enableContenutoAtto(true);

    if (schedaLeggeTrasparenzaInfo != null && schedaLeggeTrasparenzaInfo != undefined)
        setDatiSchedaLeggeTrasparenzaOnLoad(schedaLeggeTrasparenzaInfo);

    return panelContenitore;
}

function showInfoSchedaLeggeTrasparenza() {
    var testoInfo = "<h2 style='padding:5px 5px 5px 5px;font-size:12px;text-align:center;'>Adempimenti connessi all’attuazione del Decreto Legislativo 14 marzo 2013 n. 33 “Riordino della disciplina riguardante gli obblighi di pubblicità, trasparenza e diffusione di informazioni da parte delle pubbliche amministrazioni”</h2><br/>" +
        " <p style='padding:5px 5px 5px 5px;font-size:10px;text-align:justify;'>La norma dell’art. 18, della Legge n.134/2012, rubricata “Amministrazione Aperta” o Open Government, prevede disposizioni che impattano sulla trasparenza e l’apertura che le pubbliche amministrazioni sono tenute a garantire ai propri dati. Questa nuova modalità operativa della P.A. è richiamato altresì anche dagli  artt. 15, 16, 32 e 33 della L. 190/2012." +
        " <br/>Nello specifico si prevede che “la concessione delle sovvenzioni, contributi, sussidi ed ausili finanziari alle imprese e <u>l’attribuzione dei corrispettivi e dei compensi a persone, professionisti, imprese ed enti privati e comunque di vantaggi economici di qualunque genere</u> di cui all’articolo 12 della legge 7 agosto 1990, n. 241 ad enti pubblici e privati, sono soggetti alla pubblicità sulla rete internet, ai sensi del presente articolo e secondo il principio di accessibilità totale di cui all’articolo 11 del decreto legislativo 27 ottobre 2009, n. 150”. " +
        " <br/>La disposizione specifica quali dati, ad esclusione di quelli sensibili e supersensibili, obbligatoriamente devono essere indicati nel sito internet dell’Ente (art. 18, comma 2):<br/><br/> " +
        " a)    il nome dell'impresa o altro soggetto beneficiario ed i suoi dati fiscali;<br/>" +
        " b)    l’importo;<br/>" +
        " c)    la norma o il titolo a base dell’attribuzione;<br/>" +
        " d)    l’ufficio e il funzionario o dirigente responsabile del relativo procedimento amministrativo;<br/>" +
        " e)    la modalità seguita per l’individuazione del beneficiario;<br/>" +
        " f)    il link (collegamento) al progetto selezionato, al curriculum del soggetto incaricato, nonché al contratto e capitolato della prestazione, fornitura o servizio.<br/><br/>" +
        " La norma dettaglia poi anche puntuali modalità di pubblicazione dei dati previsti. La pubblicazione deve avvenire sul sito internet nella sezione <b>“Trasparenza, valutazione e merito”</b> con link ben visibile nella home page e i dati devono essere resi di facile consultazione, accessibili ai motori di ricerca ed in formato tabellare aperto che ne consenta l’esportazione, il trattamento e il riuso: si tratta quindi di open data (dati aperti).<br/><br/>" +
        " <b>Sono previste chiare responsabilità e sanzioni in caso di mancata osservanza di quanto previsto: infatti, dal 1° gennaio 2013 la pubblicazione ai sensi della norma costituirà condizione legale di efficacia del titolo legittimante delle concessioni e attribuzioni di importo complessivo superiore a mille euro nel corso dell’anno solare.</b><br/><br/>" +
        " L’eventuale omissione o incompletezza della pubblicazione è rilevata d’ufficio dagli organi dirigenziali e di controllo sotto la propria diretta responsabilità amministrativa, patrimoniale e contabile per l’indebita concessione o attribuzione del beneficio economico; la mancata, incompleta o ritardata pubblicazione è anche rilevabile dal destinatario della concessione o attribuzione e da chiunque altro abbia interesse, anche ai fini del risarcimento del danno da ritardo da parte dell’Amministrazione.</p>"

    var popup = new Ext.Window({
        title: "Informazioni 'Legge Trasparenza'",
        width: 700,
        height: 470,
        layout: 'fit',
        plain: true,
        resizable: false,
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        buttonAlign: 'center',
        closable: true,
        items: [
            {
                xtype: 'panel',
                border: false,
                layout: 'table',
                autoScroll: true,
                bodyStyle: 'padding:10px;background:#F9FBFC',
                layoutConfig: {
                    columns: 1
                },
                items: [
                {
                    xtype: 'label',
                    id: 'schedaLeggeTrasparenzaInfoLabel',
                    name: 'schedaLeggeTrasparenzaInfoLabel',
                    html: testoInfo,
                    style: 'margin-right:50px'
                }
              ]
            }
            ],
        buttons: [{
            text: 'OK',
            id: 'btnOkInfoSchedaLeggeTrasparenza'
}]
        });

        Ext.getCmp('btnOkInfoSchedaLeggeTrasparenza').on('click', function() {
            popup.close();
        });

        popup.doLayout();
        popup.show();
    }

    function buildPanelContratti() {
        var panelContratto = new Ext.Panel({
            id: 'panelContratto',
            autoHeight: true,
            xtype: 'panel',
            border: false,
            layout: 'table',
            style: 'margin-top:15px',
            layoutConfig: {
                columns: 1
            },
            items: [buildPanelGridContratti(),
            {
                xtype: 'hidden',
                id: 'listaContratti'
            }
         ]
        });

        return panelContratto;
    }

    var GET_DOCUMENT_RESULT_STATUS = { "success": 0, "error": -1, "documentsNotFoundOnNotQueryableOffice": -4, "notQueryableOffice": -5 };
    var MSG_LEVEL = { "info": 0, "warning": 1, "error": 3 };

    function getDeterminaOnLoad(ufficio, anno, numero, errorCode, errorMsg, data, showMessageBox) {
        if (errorCode == GET_DOCUMENT_RESULT_STATUS.success) {
            var msgLevel = MSG_LEVEL.info;
            if (data == null || data == undefined) {
                errorMsg = "Determina '" + buildNumeroDetermina(ufficio, anno, numero) + "' non trovata." +
                     "<br>Sarà comunque possibile utilizzare il numero di determina specificato quale " +
                     "norma per l'attribuzione del beneficio.";
                msgLevel = MSG_LEVEL.warning;
            } else {
                errorMsg = "Determina '" + buildNumeroDetermina(ufficio, anno, numero) + "' presente in archivio.";
                if (!data.IsConsultabile) {
                    errorMsg = errorMsg + "<br>L'operatore non è abilitato alla consultazione delle determine dell'ufficio '" + ufficio + "'. " +
                     "Sarà comunque possibile utilizzare il numero di determina specificato quale " +
                     "norma per l'attribuzione del beneficio.";
                    msgLevel = MSG_LEVEL.warning;
                }
            }

            var oggetto = ((data != null && data != undefined) && (data.Doc_Oggetto != null && data.Doc_Oggetto != undefined)) ? data.Doc_Oggetto : "";
            var tipologiaDocumento = ((data != null && data != undefined) && (data.Doc_TipologiaDocumento != null && data.Doc_TipologiaDocumento != undefined)) ? data.Doc_TipologiaDocumento : "";

            setDatiDetermina(ufficio, anno, numero, oggetto, tipologiaDocumento);
            setDeterminaMsg(errorMsg, msgLevel, false, showMessageBox);
        }
        else {
            var msgLevel = MSG_LEVEL.error;
            if (errorCode == GET_DOCUMENT_RESULT_STATUS.notQueryableOffice) {
                errorMsg = "Non è stato possibile verificare la determina '" + buildNumeroDetermina(ufficio, anno, numero) + "'.<br>" +
                    "L'operatore non è abilitato alla consultazione delle determine dell'ufficio '" + ufficio + "'.";
            } else if (errorCode == GET_DOCUMENT_RESULT_STATUS.documentsNotFoundOnNotQueryableOffice) {
                errorMsg = "Determina '" + buildNumeroDetermina(ufficio, anno, numero) + "' non trovata." +
                       "<br>L'operatore non è abilitato alla consultazione delle determine dell'ufficio '" + ufficio + "'. " +
                       "Sarà comunque possibile utilizzare il numero di determina specificato quale " +
                       "norma per l'attribuzione del beneficio.";
                msgLevel = MSG_LEVEL.warning;
            }

            setDatiDetermina(ufficio, anno, numero, "", "");
            setDeterminaMsg(errorMsg, msgLevel, false, showMessageBox);
        }
    }

    function getDeliberaOnLoad(anno, numero, errorCode, errorMsg, data, showMessageBox) {
        if (errorCode == GET_DOCUMENT_RESULT_STATUS.success) {
            var msgLevel = MSG_LEVEL.info;
            if (data == null || data == undefined) {
                errorMsg = "Delibera '" + buildNumeroDelibera(anno, numero) + "' non trovata." +
                     "<br>Sarà comunque possibile utilizzare il numero di delibera specificato quale " +
                     "norma per l'attribuzione del beneficio.";
                msgLevel = MSG_LEVEL.warning;
            } else {
                errorMsg = "Delibera '" + buildNumeroDelibera(anno, numero) + "' presente in archivio.";
            }

            var oggetto = ((data != null && data != undefined) && (data.Doc_Oggetto != null && data.Doc_Oggetto != undefined)) ? data.Doc_Oggetto : "";
            var tipologiaDocumento = ((data != null && data != undefined) && (data.Doc_TipologiaDocumento != null && data.Doc_TipologiaDocumento != undefined)) ? data.Doc_TipologiaDocumento : "";

            setDatiDelibera(anno, numero, oggetto, tipologiaDocumento);
            setDeliberaMsg(errorMsg, msgLevel, false, showMessageBox);
        }
        else {
            setDatiDelibera(anno, numero, "", "");
            setDeliberaMsg(errorMsg, MSG_LEVEL.error, false, showMessageBox);
        }
    }

    function setDatiSchedaLeggeTrasparenzaOnLoad(schedaLeggeTrasparenzaInfo) {
        if (schedaLeggeTrasparenzaInfo != null) {
            if (!schedaLeggeTrasparenzaInfo.AutorizzazionePubblicazione) {
                Ext.getCmp('noPubblicazione').checked = true;
                Ext.getCmp('noPubblicazione').fireEvent('check');
                Ext.getCmp('yesPubblicazione').checked = false;
                Ext.getCmp('yesPubblicazione').fireEvent('check');
                Ext.getCmp('noteAutorizzazionePubblicazione').setValue(schedaLeggeTrasparenzaInfo.NotePubblicazione);
            } else {
                Ext.getCmp('noPubblicazione').checked = false;
                Ext.getCmp('noPubblicazione').fireEvent('check');
                Ext.getCmp('yesPubblicazione').checked = true;
                Ext.getCmp('yesPubblicazione').fireEvent('check');

                var dati = null;
                var normaAttribuzioneBeneficio = schedaLeggeTrasparenzaInfo.NormaAttribuzioneBeneficio;
                if ((dati = parseNumeroDetermina(normaAttribuzioneBeneficio, "Determina n. ", "")) != null) {
                    Ext.getCmp('DETERMINA').checked = true;
                    Ext.getCmp('DETERMINA').fireEvent('check');
                    Ext.getCmp('DELIBERA').checked = false;
                    Ext.getCmp('DELIBERA').fireEvent('check');
                    Ext.getCmp('ALTRO').checked = false;
                    Ext.getCmp('ALTRO').fireEvent('check');

                    getDetermina(dati.ufficio, dati.anno, dati.numero, getDeterminaOnLoad);
                } else if ((dati = parseNumeroDelibera(normaAttribuzioneBeneficio, "Delibera n. ", "")) != null) {
                    Ext.getCmp('DETERMINA').checked = false;
                    Ext.getCmp('DETERMINA').fireEvent('check');
                    Ext.getCmp('DELIBERA').checked = true;
                    Ext.getCmp('DELIBERA').fireEvent('check');
                    Ext.getCmp('ALTRO').checked = false;
                    Ext.getCmp('ALTRO').fireEvent('check');

                    getDelibera(dati.anno, dati.numero, getDeliberaOnLoad);
                } else {
                    Ext.getCmp('DETERMINA').checked = false;
                    Ext.getCmp('DETERMINA').fireEvent('check');
                    Ext.getCmp('DELIBERA').checked = false;
                    Ext.getCmp('DELIBERA').fireEvent('check');
                    Ext.getCmp('ALTRO').checked = true;
                    Ext.getCmp('ALTRO').fireEvent('check');

                    setDatiAltraNorma(normaAttribuzioneBeneficio);
                }

                if (schedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento == "") {
                    Ext.getCmp('ufficioResponsabileDelProcedimento').setValue(Ext.get('descrizioneUffProp').getValue());
                } else {
                    Ext.getCmp('ufficioResponsabileDelProcedimento').setValue(schedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento);
                }

                if (schedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento == "") {
                    Ext.getCmp('funzionarioResponsabileDelProcedimento').setValue(Ext.get('responsabileUffProp').getValue());
                } else {
                    Ext.getCmp('funzionarioResponsabileDelProcedimento').setValue(schedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento);
                }

                Ext.getCmp('modalitaIndividuazioneBeneficiario').setValue(schedaLeggeTrasparenzaInfo.ModalitaIndividuazioneBeneficiario);
            }

            //init contratti grid
//            enableDatiContratto(true);
//            if (schedaLeggeTrasparenzaInfo.Contratti != undefined && schedaLeggeTrasparenzaInfo.Contratti != null) {
//                var contratti = schedaLeggeTrasparenzaInfo.Contratti;

//                for (var i = 0; i < contratti.length; i++) {
//                    var contratto = contratti[i];
//                    if (contratto != undefined && contratto != null)
//                        addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id);
//                }
//            }
            enableContenutoAtto(true);
            if (schedaLeggeTrasparenzaInfo.ContenutoAtto != undefined && schedaLeggeTrasparenzaInfo.ContenutoAtto != null) {
                Ext.getCmp('contenutoAtto').setValue(schedaLeggeTrasparenzaInfo.ContenutoAtto);
            }
        }
    }

    function resetNormaSelectionFields() {
        if (Ext.getCmp('numeroDelibera') != undefined)
            Ext.getCmp('numeroDelibera').reset();

        if (Ext.getCmp('annoDelibera') != undefined)
            Ext.getCmp('annoDelibera').reset();

        if (Ext.getCmp('numeroDetermina') != undefined)
            Ext.getCmp('numeroDetermina').reset();

        if (Ext.getCmp('ufficioDetermina') != undefined)
            Ext.getCmp('ufficioDetermina').reset();

        if (Ext.getCmp('annoDetermina') != undefined)
            Ext.getCmp('annoDetermina').reset();

        if (Ext.getCmp('normaAttribuzioneAltroDescrizione') != undefined)
            Ext.getCmp('normaAttribuzioneAltroDescrizione').reset();
    }

    function validateDatiDetermina() {
        return ((Ext.getCmp('ufficioDetermina') != undefined && Ext.getCmp('ufficioDetermina').validate()) &&
        (Ext.getCmp('annoDetermina') != undefined && Ext.getCmp('annoDetermina').validate()) &&
        (Ext.getCmp('numeroDetermina') != undefined && Ext.getCmp('numeroDetermina').validate()));
    }

    function validateDatiDelibera() {
        return ((Ext.getCmp('numeroDelibera') != undefined && Ext.getCmp('numeroDelibera').validate()) &&
        (Ext.getCmp('annoDelibera') != undefined && Ext.getCmp('annoDelibera').validate()));
    }

    function isKeyValid(event) {
        return event.keyCode == 8 // backspace
    || event.keyCode == 46 // canc
    || (event.keyCode > 47 && event.keyCode < 91)
    || (event.keyCode > 95 && event.keyCode < 106); // alfanumerici
    }

    var deliberaCheckTimeoutFn = null;

    function autoCompleteDeliberaSearch(event) {
        if (Ext.getCmp('annoDelibera').isValid() && Ext.getCmp('numeroDelibera').isValid()) {
            if (event == null || event == undefined || isKeyValid(event)) {
                if (deliberaCheckTimeoutFn != null && deliberaCheckTimeoutFn != undefined) {
                    clearTimeout(deliberaCheckTimeoutFn);
                    deliberaCheckTimeoutFn = null;
                }
                deliberaCheckTimeoutFn = setTimeout(
                                    function() {
                                        getDelibera(Ext.getCmp('annoDelibera').getValue(),
                                            Ext.getCmp('numeroDelibera').getValue(),
                                            getDeliberaOnLoad,
                                            true, 'panelNormaAttribuzioneDelibera', false);
                                    }, 1000);
            }
        } else {
            if (deliberaCheckTimeoutFn != null && deliberaCheckTimeoutFn != undefined) {
                clearTimeout(deliberaCheckTimeoutFn);
                deliberaCheckTimeoutFn = null;
            }
        }
    }

    var determinaCheckTimeoutFn = null;

    function autoCompleteDeterminaSearch(event) {
        if (Ext.getCmp('ufficioDetermina').isValid() && Ext.getCmp('annoDetermina').isValid() && Ext.getCmp('numeroDetermina').isValid()) {
            if (event == null || event == undefined || isKeyValid(event)) {
                if (determinaCheckTimeoutFn != null && determinaCheckTimeoutFn != undefined) {
                    clearTimeout(determinaCheckTimeoutFn);
                    determinaCheckTimeoutFn = null;
                }
                determinaCheckTimeoutFn = setTimeout(
                                        function() {
                                            getDetermina(Ext.getCmp('ufficioDetermina').getValue(),
                                                Ext.getCmp('annoDetermina').getValue(),
                                                Ext.getCmp('numeroDetermina').getValue(),
                                                getDeterminaOnLoad,
                                                true, 'panelNormaAttribuzioneDetermina', false);
                                        }, 1000);
            }
        } else {
            if (determinaCheckTimeoutFn != null && determinaCheckTimeoutFn != undefined) {
                clearTimeout(determinaCheckTimeoutFn);
                determinaCheckTimeoutFn = null;
            }
        }
    }

    function buildPanelNormaAttribuzioneBeneficio() {

        var panelNormaAttribuzioneDelibera = new Ext.Panel({
            xtype: "panel",
            title: "",
            id: 'panelNormaAttribuzioneDelibera',
            layout: 'table',
            border: true,
            width: 500,
            layoutConfig: {
                columns: 1
            },
            autoHeight: true,
            bodyStyle: 'margin-top:2px;margin-bottom:6px;padding:7px;background: #EBF3FD',
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: 'table',
                    bodyStyle: 'background: #EBF3FD',
                    layoutConfig: {
                        columns: 5
                    },
                    items: [
	        {
	            xtype: 'label',
	            text: 'Numero*',
	            id: 'numeroDeliberaLabel',
	            style: 'padding-right: 9px'
	        }, {
	            xtype: 'textfield',
	            name: 'numeroDelibera',
	            id: 'numeroDelibera',
	            style: 'margin-top:0px',
	            width: 100,
	            allowBlank: false,
	            blankText: 'Campo Obbligatorio',
	            maxLength: 7,
	            maxLengthText: 'La lunghezza massima è 7 caratteri',
	            enableKeyEvents: true,
	            listeners: {
	                keyup: function(textField, event) {
	                    autoCompleteDeliberaSearch(event);
	                }
	            },
	            validator: function(value) {
	                if (!Ext.getCmp('numeroDelibera').allowBlank) {
	                    var pattern = '^[0-9]{1,7}$';
	                    var stringa = value.trim();
	                    var result = stringa.search(pattern);
	                    return result > -1;
	                } else
	                    return true;
	            },
	            invalidText: 'Il numero delibera inserito non è corretto'
	        },
	        {
	            xtype: 'label',
	            text: 'Anno*',
	            id: 'annoDeliberaLabel',
	            style: 'padding-left: 8px;padding-right: 6px'
	        }, {
	            xtype: 'textfield',
	            name: 'annoDelibera',
	            id: 'annoDelibera',
	            width: 50,
	            allowBlank: false,
	            blankText: 'Campo Obbligatorio',
	            maxLength: 4,
	            minLength: 4,
	            minLengthText: 'La lunghezza minima è 4 caratteri',
	            maxLengthText: 'La lunghezza massima è 4 caratteri',
	            enableKeyEvents: true,
	            listeners: {
	                keyup: function(textField, event) {
	                    autoCompleteDeliberaSearch(event);
	                }
	            },
	            validator: function(value) {
	                if (!Ext.getCmp('annoDelibera').allowBlank) {
	                    var pattern = '^[0-9]{4}$';
	                    var stringa = value.trim();
	                    var result = stringa.search(pattern);
	                    return result > -1 && parseInt(value) > 1752;
	                } else
	                    return true;
	            },
	            invalidText: "L'anno inserito non è corretto"

	        }, {
	            xtype: 'linkbutton', id: 'cercaDelibere', text: 'Cerca', style: 'margin-left: 10px',
	            tooltip: 'Cerca Delibere',
	            listeners: {
	                click: function() {
	                    showPopupPanelRicercaDocumenti(2, "Delibere");
	                }
	            }
}]
                },
            {
                xtype: 'panel',
                border: false,
                layout: 'table',
                bodyStyle: 'background: #EBF3FD',
                layoutConfig: {
                    columns: 2
                },
                style: 'margin-top:2px',
                items: [{
                    xtype: 'label',
                    id: 'oggettoDeliberaLabel',
                    text: "Oggetto",
                    disabled: true,
                    style: 'margin-right:11px'
                }, {
                    xtype: 'textarea',
                    maxLength: 1024,
                    name: 'oggettoDelibera',
                    id: 'oggettoDelibera',
                    style: 'margin-bottom:2px',
                    disabled: true,
                    width: 425,
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: "Tipologia",
                    id: 'tipologiaDocumentoDeliberaLabel',
                    disabled: true,
                    style: 'margin-right:8px'
                }, {
                    xtype: 'label',
                    id: 'tipologiaDocumentoDelibera',
                    html: "<div style='height:12px;width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'></div>",
                    disabled: true
                }
                ]
            }, {
                xtype: 'panel',
                id: 'deliberaErrorPanel',
                bodyStyle: 'background: #EBF3FD',
                border: false,
                hidden: true,
                width: 500,
                layout: 'table',
                layoutConfig: {
                    columns: 1
                },
                style: 'margin-top:8px',
                items: [{
                    xtype: 'label',
                    id: 'deliberaErrorLabel',
                    style: 'margin-right:4px'
                }, {
                    xtype: 'label',
                    id: 'deliberaErrorFlag',
                    hidden: true
}]
                }
	    ]
            });

            var panelNormaAttribuzioneDetermina = new Ext.Panel({
                xtype: "panel",
                title: "",
                id: 'panelNormaAttribuzioneDetermina',
                layout: 'table',
                border: true,
                bodyStyle: 'margin-top:2px;margin-bottom:6px;padding:7px;background:#EBF3FD',
                width: 500,
                layoutConfig: {
                    columns: 1
                },
                autoHeight: true,
                items: [
        {
            xtype: 'panel',
            border: false,
            layout: 'table',
            bodyStyle: 'background:#EBF3FD',
            layoutConfig: {
                columns: 7
            },
            items: [
	        {
	            xtype: 'label',
	            text: 'Ufficio*',
	            id: 'ufficioDeterminaLabel',
	            style: 'padding-right: 18px'
	        }, {
	            xtype: 'textfield',
	            name: 'ufficioDetermina',
	            id: 'ufficioDetermina',
	            style: 'margin-top:0px',
	            width: 42,
	            allowBlank: false,
	            blankText: 'Campo Obbligatorio',
	            maxLength: 4,
	            minLength: 4,
	            minLengthText: 'La lunghezza minima è 4 caratteri',
	            maxLengthText: 'La lunghezza massima è 4 caratteri',
	            enableKeyEvents: true,
	            listeners: {
	                keyup: function(textField, event) {
	                    autoCompleteDeterminaSearch(event);
	                }
	            },
	            validator: function(value) {
	                if (!Ext.getCmp('ufficioDetermina').allowBlank) {
	                    var pattern = '^[a-zA-Z0-9]{4}$';
	                    var stringa = value.trim();
	                    var result = stringa.search(pattern);
	                    return result > -1;
	                } else
	                    return true;
	            },
	            invalidText: "Il codice dell'ufficio inserito non è corretto"

	        },
	        {
	            xtype: 'label',
	            text: 'Anno*',
	            id: 'annoDeterminaLabel',
	            style: 'padding-left: 8px;padding-right: 6px'
	        }, {
	            xtype: 'textfield',
	            name: 'annoDetermina',
	            id: 'annoDetermina',
	            width: 42,
	            allowBlank: false,
	            blankText: 'Campo Obbligatorio',
	            maxLength: 4,
	            minLength: 4,
	            minLengthText: 'La lunghezza minima è 4 caratteri',
	            maxLengthText: 'La lunghezza massima è 4 caratteri',
	            enableKeyEvents: true,
	            listeners: {
	                keyup: function(textField, event) {
	                    autoCompleteDeterminaSearch(event);
	                }
	            },
	            validator: function(value) {
	                if (!Ext.getCmp('annoDetermina').allowBlank) {
	                    var pattern = '^[0-9]{4}$';
	                    var stringa = value.trim();
	                    var result = stringa.search(pattern);
	                    return result > -1 && parseInt(value) > 1752;
	                } else
	                    return true;
	            },
	            invalidText: "L'anno inserito non è corretto"

	        }, {
	            xtype: 'label',
	            text: 'Numero*',
	            id: 'numeroDeterminaLabel',
	            style: 'padding-left: 8px;padding-right: 6px'
	        }, {
	            xtype: 'textfield',
	            name: 'numeroDetermina',
	            id: 'numeroDetermina',
	            width: 50,
	            allowBlank: false,
	            blankText: 'Campo Obbligatorio',
	            maxLength: 5,
	            maxLengthText: 'La lunghezza massima è 5 caratteri',
	            enableKeyEvents: true,
	            listeners: {
	                keyup: function(textField, event) {
	                    autoCompleteDeterminaSearch(event);
	                }
	            },
	            validator: function(value) {
	                if (!Ext.getCmp('numeroDetermina').allowBlank) {
	                    var pattern = '^[0-9]{1,5}$';
	                    var stringa = value.trim();
	                    var result = stringa.search(pattern);
	                    return result > -1;
	                } else
	                    return true;
	            },
	            invalidText: 'Il numero determina inserito non è corretto'

	        }
	        ,
	        { xtype: 'linkbutton', id: 'cercaDetermine', text: 'Cerca', style: 'margin-left: 10px',
	            tooltip: 'Cerca Determine',
	            listeners: {
	                click: function() {
	                    showPopupPanelRicercaDocumenti(0, "Determine");
	                }
	            }
	        }
	        ]
        },
            {
                xtype: 'panel',
                border: false,
                layout: 'table',
                bodyStyle: 'background:#EBF3FD',
                layoutConfig: {
                    columns: 2
                },
                style: 'margin-top:2px',
                items: [{
                    xtype: 'label',
                    text: "Oggetto",
                    id: 'oggettoDeterminaLabel',
                    disabled: true,
                    style: 'margin-right:6px'
                }, {
                    xtype: 'textarea',
                    maxLength: 1024,
                    name: 'oggettoDetermina',
                    id: 'oggettoDetermina',
                    style: 'margin-bottom:2px',
                    disabled: true,
                    width: 425,
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: "Tipologia",
                    id: 'tipologiaDocumentoDeterminaLabel',
                    disabled: true,
                    style: 'margin-right:8px'
                }, {
                    xtype: 'label',
                    id: 'tipologiaDocumentoDetermina',
                    html: "<div style='height:12px;width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'></div>",
                    disabled: true
}]
                }, {
                    xtype: 'panel',
                    id: 'determinaErrorPanel',
                    border: false,
                    hidden: true,
                    width: 500,
                    bodyStyle: 'background:#EBF3FD',
                    layout: 'table',
                    layoutConfig: {
                        columns: 1
                    },
                    style: 'margin-top:8px;',
                    items: [{
                        xtype: 'label',
                        id: 'determinaErrorLabel',
                        style: 'margin-right:4px'
                    }, {
                        xtype: 'label',
                        id: 'determinaErrorFlag',
                        hidden: true
}]
                    }
            ]
                });

                var panelNormaAttribuzioneAltro = new Ext.Panel({
                    xtype: "panel",
                    title: "",
                    id: 'panelNormaAttribuzioneAltro',
                    layout: 'table',
                    border: false,
                    layoutConfig: {
                        columns: 1
                    },
                    autoHeight: true,
                    style: 'margin-top:2px;margin-bottom:6px',
                    items: [
	    {
	        xtype: 'textarea',
	        allowBlank: false,
	        blankText: 'Campo Obbligatorio',
	        maxLength: 1024,
	        name: 'normaAttribuzioneAltroDescrizione',
	        id: 'normaAttribuzioneAltroDescrizione',
	        width: 500,
	        maxLengthText: 'La lunghezza massima è di 1024 caratteri',
	        validator: function(value) {
	            return value != null && value != undefined && value.trim().length != 0;
	        },
	        invalidText: "Campo obbligatorio"
	    }
	    ]
                });


                var DETERMINA = new Ext.form.Radio({
                    boxLabel: 'Determina',
                    id: 'DETERMINA',
                    name: 'selezioneNorma',
                    inputValue: 'DETERMINA',
                    checked: false
                });

                DETERMINA.on('check', function() {
                    if (DETERMINA.checked) {
                        enableDatiDetermina(true);
                        enableDatiAltraNorma(false);
                        enableDatiDelibera(false);
                        panelNormaAttribuzioneDetermina.show();
                    } else {
                        panelNormaAttribuzioneDetermina.hide();
                    }
                });

                var DELIBERA = new Ext.form.Radio({
                    boxLabel: 'Delibera',
                    id: 'DELIBERA',
                    name: 'selezioneNorma',
                    inputValue: 'DELIBERA',
                    checked: false
                });

                DELIBERA.on('check', function() {
                    if (DELIBERA.checked) {
                        enableDatiDetermina(false);
                        enableDatiAltraNorma(false);
                        enableDatiDelibera(true);
                        panelNormaAttribuzioneDelibera.show();
                    } else {
                        panelNormaAttribuzioneDelibera.hide();
                    }
                });

                var ALTRO = new Ext.form.Radio({
                    boxLabel: 'Altro',
                    id: 'ALTRO',
                    name: 'selezioneNorma',
                    inputValue: 'ALTRO',
                    checked: true
                });

                ALTRO.on('check', function() {
                    if (ALTRO.checked) {
                        enableDatiDetermina(false);
                        enableDatiAltraNorma(true);
                        enableDatiDelibera(false);
                        panelNormaAttribuzioneAltro.show();
                    } else {
                        panelNormaAttribuzioneAltro.hide();
                    }
                });

                enableDatiAltraNorma(true);
                enableDatiDetermina(false);
                enableDatiDelibera(false);

                panelNormaAttribuzioneAltro.show();
                panelNormaAttribuzioneDelibera.hide();
                panelNormaAttribuzioneDetermina.hide();

                var gestioneNormaAttribuzioneBeneficio = new Ext.Panel({
                    id: 'panelGestioneNormaAttribuzioneBeneficio',
                    labelAlign: 'top',
                    bodyStyle: 'margin:' + (Ext.isIE ? '8px' : '10px') + ' 0px 10px 0px',
                    border: false,
                    items:
            [{
                xtype: 'panel',
                border: false,
                layout: 'table',
                style: 'margin-bottom: 0px',
                layoutConfig: {
                    columns: 5
                },
                items: [DETERMINA,
                    {
                        xtype: 'label',
                        style: 'padding-right:10px',
                        text: ''
                    },
                    DELIBERA,
                    {
                        xtype: 'label',
                        style: 'padding-right:10px',
                        text: ''
                    },
                    ALTRO
                    ]
            },
              panelNormaAttribuzioneDetermina,
              panelNormaAttribuzioneDelibera,
              panelNormaAttribuzioneAltro
            ]
                });

                return gestioneNormaAttribuzioneBeneficio;
            }

//            function addContratto(numeroRepertorio, oggetto, id) {
//                var retValue = ADD_LIST_RESULT.error;

//                var gridContratti = Ext.getCmp('GridContratti');
//                if (gridContratti != undefined && gridContratti != null) {
//                    var gridContrattiStore = gridContratti.store;
//                    if (gridContrattiStore != undefined && gridContrattiStore != null) {
//                        if (!isNullOrEmpty(id)) {
//                            var recordAlreadyExists = gridContrattiStore.queryBy(
//                                function(record, recordId) {
//                                    return (record.data.Id == id)
//                                });

//                            if (!(recordAlreadyExists != undefined && recordAlreadyExists.length > 0)) {
//                                ContrattoRecordType = Ext.data.Record.create(['Id', 'NumeroRepertorio', 'Oggetto']);
//                                var newRecordContratto = new ContrattoRecordType({
//                                    Id: id
//                        , NumeroRepertorio: numeroRepertorio
//                        , Oggetto: oggetto
//                                });

//                                gridContrattiStore.add(newRecordContratto);
//                                retValue = ADD_LIST_RESULT.ok;
//                            }
//                            else
//                                retValue = ADD_LIST_RESULT.exists;
//                        }
//                    }
//                }

//                return retValue;
//            }


            function setActivePanel(panelName, panelNameToActive) {
                Ext.getCmp(panelName).active = Ext.getCmp(panelNameToActive);
                Ext.getCmp(panelName).active.show();
                Ext.getCmp(panelName).doLayout();
            }

//            function resetContrattoSearchFields() {
//                Ext.getCmp('numeroRepertorioContrattoSearchField').setValue('');
//                Ext.getCmp('descrizioneContrattoSearchField').setValue('');
//            }

//            function buildPanelRicercaContratti() {
//                var numeroRepertorio = new Ext.form.TextField({
//                    id: 'numeroRepertorioContrattoSearchField',
//                    width: 450,
//                    labelSeparator: '',
//                    blankText: '',
//                    emptyText: '',
//                    listeners: {
//                        scope: this,
//                        specialkey: function(f, e) {
//                            if (e.getKey() == e.ENTER) {
//                                var btnCerca = Ext.getCmp('btnCercaContratti');
//                                btnCerca.fireEvent('click');
//                            }
//                        }
//                    }
//                });

//                var descrizione = new Ext.form.TextField({
//                    id: 'descrizioneContrattoSearchField',
//                    width: 450,
//                    labelSeparator: '',
//                    blankText: '',
//                    emptyText: '',
//                    listeners: {
//                        scope: this,
//                        specialkey: function(f, e) {
//                            if (e.getKey() == e.ENTER) {
//                                var btnCerca = Ext.getCmp('btnCercaContratti');
//                                btnCerca.fireEvent('click');
//                            }
//                        }
//                    }
//                });

//                var NUMERO_REPERTORIO = new Ext.form.Radio({
//                    boxLabel: 'Numero Repertorio',
//                    id: 'NUMERO_REPERTORIO_RADIO_BTN',
//                    name: 'CampoRicercaContratto',
//                    inputValue: 'NUMERO_REPERTORIO_CONTRATTO',
//                    checked: true
//                });

//                NUMERO_REPERTORIO.on('check', function() {
//                    resetContrattoSearchFields();
//                    if (NUMERO_REPERTORIO.checked) {
//                        numeroRepertorio.show();
//                    } else {
//                        numeroRepertorio.hide();
//                    }
//                });

//                var DESCRIZIONE = new Ext.form.Radio({
//                    boxLabel: 'Oggetto',
//                    id: 'DESCRIZIONE_RADIO_BTN',
//                    name: 'CampoRicercaContratto',
//                    inputValue: 'DESCRIZIONE_CONTRATTO'
//                });

//                DESCRIZIONE.on('check', function() {
//                    resetContrattoSearchFields();
//                    if (DESCRIZIONE.checked) {
//                        descrizione.show();
//                    } else {
//                        descrizione.hide();
//                    }
//                });


//                numeroRepertorio.show();
//                descrizione.hide();

//                var risultatiRicercaContratti = new Ext.grid.GridPanel({
//                    id: 'risultatiRicercaContratti',
//                    cm: new Ext.grid.ColumnModel({}),
//                    sm: new Ext.grid.RowSelectionModel({}),
//                    store: new Ext.data.SimpleStore({ fields: ['id'], data: ['G'] })
//                });

//                var actionAggiungiContratto = new Ext.Action({
//                    text: 'Nuovo Contratto',
//                    tooltip: 'Aggiungi un nuovo contratto',
//                    handler: function() {
//                        var codFiscOperatore = Ext.get('codFiscOperatore').getValue()
//                        var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue()

//                        window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO:" + codFiscOperatore + "," + uffPubblicoOperatore, '_blank');
//                    },
//                    iconCls: 'add'
//                });

//                var ricercaContratti = new Ext.Panel({
//                    id: 'panelRicercaContratti',
//                    labelAlign: 'top',
//                    tbar: [actionAggiungiContratto],
//                    bodyStyle: 'padding:10px',
//                    width: 650,
//                    height: 350,
//                    autoScroll: true,
//                    items: [
//        {
//            xtype: 'panel',
//            plain: true,
//            title: 'Criteri di ricerca',
//            border: true,
//            layout: 'table',
//            style: 'margin-top:10px',
//            bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 10px;' : 'padding:10px 10px 10px 10px',
//            layoutConfig: {
//                columns: 3
//            },
//            items: [NUMERO_REPERTORIO,
//                {
//                    xtype: 'label',
//                    style: 'padding-right:40px',
//                    text: ''
//                }, numeroRepertorio,
//                DESCRIZIONE,
//                {
//                    xtype: 'label',
//                    style: 'padding-right:40px',
//                    text: ''
//                }, descrizione
//            ],
//            buttons: [{
//                text: 'Cerca',
//                id: 'btnCercaContratti'
//            }
//        ]
//        },
//        risultatiRicercaContratti
//           ]
//                });

//                Ext.getCmp('btnCercaContratti').on('click', function() {
//                    var numeroRepertorioContratto = Ext.get('numeroRepertorioContrattoSearchField').getValue();
//                    var oggettoContratto = Ext.get('descrizioneContrattoSearchField').getValue();

//                    if (Ext.getCmp('GridRisultatiRicercaContratti') != undefined)
//                        Ext.getCmp('risultatiRicercaContratti').remove(Ext.getCmp('GridRisultatiRicercaContratti'));

//                    var gridRisultatiRicercaContratti = buildPanelRisultatiRicercaContratti(numeroRepertorioContratto, oggettoContratto);

//                    if (gridRisultatiRicercaContratti != null && gridRisultatiRicercaContratti != undefined)
//                        Ext.getCmp('risultatiRicercaContratti').add(gridRisultatiRicercaContratti);

//                    Ext.getCmp('risultatiRicercaContratti').show();
//                    Ext.getCmp("panelRicercaContratti").doLayout();
//                });


//                Ext.getCmp('risultatiRicercaContratti').hide();
//                Ext.getCmp('btnCercaContratti').enable();

//                return ricercaContratti;
//            }

//            function buildPanelRisultatiRicercaContratti(numeroRepertorioContratto, oggettoContratto) {
//                var maskApp = new Ext.LoadMask(Ext.getBody(), {
//                    msg: "Recupero Dati..."
//                });

//                var proxy = new Ext.data.HttpProxy({
//                    url: 'ProcAmm.svc/InterrogaContratti',
//                    method: 'POST',
//                    timeout: 10000000
//                });

//                var reader = new Ext.data.JsonReader({
//                    root: 'InterrogaContrattiResult',
//                    fields: [
//	       { name: 'Id' },
//           { name: 'NumeroRepertorio' },
//           { name: 'Oggetto' }
//           ]
//                });

//                var store = new Ext.data.Store({
//                    proxy: proxy
//		   , reader: reader
//           , sortInfo: { field: "NumeroRepertorio", direction: "ASC" }
//                });

//                store.on({ 'load': {
//                    fn: function(store, records, options) {
//                        if (records.length == 0) {
//                            if (Ext.getCmp('btnSelezionaContratto') != null && Ext.getCmp('btnSelezionaContratto') != undefined)
//                                Ext.getCmp('btnSelezionaContratto').disable();
//                        }
//                        maskApp.hide();
//                    },
//                    scope: this
//                }
//                });

//                maskApp.show();

//                var parametri = { numeroRepertorio: numeroRepertorioContratto, descrizione: oggettoContratto };

//                try {
//                    store.load({ params: parametri });
//                } catch (ex) {
//                    maskApp.hide();
//                }

//                var sm = new Ext.grid.CheckboxSelectionModel({
//                    singleSelect: false,
//                    loadMask: false,
//                    listeners: {
//                        rowselect: function(selectionModel, rowIndex, record) {
//                            var totalRows = Ext.getCmp('GridRisultatiRicercaContratti').store.getRange().length;
//                            var selectedRows = selectionModel.getSelections();
//                            if (selectedRows.length > 0) {
//                                Ext.getCmp('btnSelezionaContratto').enable();
//                            }
//                            if (totalRows == selectedRows.length) {
//                                var view = Ext.getCmp('GridRisultatiRicercaContratti').getView();
//                                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker")
//                                chkdiv.addClass("x-grid3-hd-checker-on");
//                            }
//                        },
//                        rowdeselect: function(selectionModel, rowIndex, record) {
//                            var selectedRows = selectionModel.getSelections();
//                            if (selectedRows.length == 0) {
//                                Ext.getCmp('btnSelezionaContratto').disable();
//                            }
//                            var view = Ext.getCmp('GridRisultatiRicercaContratti').getView();
//                            var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker")
//                            chkdiv.removeClass('x-grid3-hd-checker-on');
//                        }
//                    }
//                });

//                var ColumnModel = new Ext.grid.ColumnModel([
//            sm,
//            { header: "Numero Repertorio", width: 120, dataIndex: 'NumeroRepertorio', sortable: true, locked: true },
//            { header: "Oggetto", width: 470, dataIndex: 'Oggetto', sortable: true, locked: false },
//         	{ header: "Id", dataIndex: 'Id', id: 'Id', sortable: false, hidden: true }
//         	]);

//                var grid = new Ext.grid.GridPanel({
//                    id: 'GridRisultatiRicercaContratti',
//                    title: 'Risultati della ricerca',
//                    autoHeight: false,
//                    height: 180,
//                    border: false,
//                    viewConfig: { forceFit: true },
//                    ds: store,
//                    cm: ColumnModel,
//                    stripeRows: true,
//                    viewConfig: {
//                        emptyText: "Nessun contratto corrisponde ai criteri di ricerca inseriti.",
//                        deferEmptyText: false
//                    },
//                    sm: sm
//                });

//                return grid;
//            }

//            function showPopupPanelRicercaContratti() {
//                var popup = new Ext.Window({
//                    title: 'Ricerca Contratti',
//                    width: 700,
//                    height: 470,
//                    layout: 'fit',
//                    plain: true,
//                    bodyStyle: 'padding:10px',
//                    resizable: false,
//                    maximizable: false,
//                    enableDragDrop: true,
//                    collapsible: false,
//                    modal: true,
//                    autoScroll: true,
//                    closable: true,
//                    buttons: [{
//                        text: 'Inserisci Contratto/i',
//                        id: 'btnSelezionaContratto'
//                    },
//            {
//                text: 'Chiudi',
//                id: 'btnChiudiSelezioneContratto'
//}]
//                });

//                var panelRicercaContratti = buildPanelRicercaContratti();
//                popup.add(panelRicercaContratti);

//                popup.doLayout();
//                popup.show();

//                Ext.getCmp('btnSelezionaContratto').disable();

//                Ext.getCmp('btnSelezionaContratto').on('click', function() {
//                    enableDatiContratto(true);

//                    var contratti = Ext.getCmp('GridRisultatiRicercaContratti').getSelectionModel().getSelections();
//                    if (contratti != undefined && contratti != null) {
//                        var addStatus = ADD_LIST_RESULT.ok;
//                        var contrattiEsistenti = "";
//                        var contrattiSeparator = "'";
//                        for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < contratti.length; i++) {
//                            var contratto = contratti[i].data;
//                            if (contratto != undefined && contratto != null) {
//                                var currentAddStatus = addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id);
//                                if (currentAddStatus == ADD_LIST_RESULT.error)
//                                    addStatus = currentAddStatus;
//                                else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
//                                    addStatus = currentAddStatus;

//                                if (currentAddStatus == ADD_LIST_RESULT.exists) {
//                                    contrattiEsistenti = contrattiEsistenti + contrattiSeparator + contratto.NumeroRepertorio + "'";
//                                    contrattiSeparator = ", '";
//                                }
//                            }
//                        }

//                        var msg = 'Impossibile aggiungere tutti o parte dei contratti selezionati.';
//                        if (addStatus == ADD_LIST_RESULT.exists)
//                            msg = 'Uno o più contratti selezionati sono già presenti nella lista: ' + contrattiEsistenti + '. Sono stati aggiunti solo quelli eventualmente non presenti.';
//                        else if (addStatus == ADD_LIST_RESULT.ok)
//                            msg = 'Operazione completata con successo.';

//                        Ext.MessageBox.show({
//                            title: 'Aggiungi Contratto/i',
//                            msg: msg,
//                            buttons: Ext.MessageBox.OK,
//                            icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
//                            fn: function(btn) {
//                            }
//                        });
//                    }
//                });

//                Ext.getCmp('btnChiudiSelezioneContratto').on('click', function() {
//                    popup.close();
//                });
//            }

            function getDipartimentiStore(fnOnLoad) {
                var proxy = new Ext.data.HttpProxy({
                    //url: 'ProcAmm.svc/GetDipartimentiDipendenti',
                    url: 'ProcAmm.svc/GetTuttiDipartimenti',
                    method: 'GET'
                });

                var reader = new Ext.data.JsonReader({

                    //root: 'GetDipartimentiDipendentiResult',
                    root: 'GetTuttiDipartimentiResult',
                    fields: [
   { name: 'CodiceInterno' },
   { name: 'CodicePubblico' },
   { name: 'DescrizioneBreve' }
   ]
                });

                var store = new Ext.data.Store({
                    proxy: proxy,
                    reader: reader,
                    autoLoad: false
                });

                store.load();
                store.on({
                    'load': {
                        fn: function(store, records, options) {
                            if (fnOnLoad != undefined && fnOnLoad != null)
                                fnOnLoad(store);
                        },
                        scope: this
                    }
                });
                return store;
            }

            function initDipartimentiCombo(store) {
                var comboDipartimenti = Ext.getCmp('comboDipartimentiSearchField');

                if (store.getCount() != 0) {
                    var codiceDipartimento = store.getAt(0).data.CodiceInterno;
                    comboDipartimenti.setValue(codiceDipartimento);

                    comboDipartimenti.enable();
                    Ext.getCmp('labelDipartimentiId').enable();

                    var comboUffici = Ext.getCmp('comboUfficiSearchField');

                    comboUffici.clearValue();
                    comboUffici.bindStore(getUfficiStore(codiceDipartimento, initUfficiCombo));
                } else {
                    comboDipartimenti.disable();
                    Ext.getCmp('labelDipartimentiId').disable();
                }
            }

            function getUfficiStore(dipartimento, onLoadFn) {
                var proxy = new Ext.data.HttpProxy({
                    //url: 'ProcAmm.svc/GetUfficiDipendenti',
                    url: 'ProcAmm.svc/GetTuttiUffici',
                    method: 'GET'
                });

                var reader = new Ext.data.JsonReader({
                    //root: 'GetUfficiDipendentiResult',
                    root: 'GetTuttiUfficiResult',
                    fields: [
    { name: 'CodiceInterno' },
    { name: 'CodicePubblico' },
    { name: 'DescrizioneBreve' }
   ]
                });

                var store = new Ext.data.Store({
                    proxy: proxy,
                    reader: reader,
                    autoLoad: false
                });

                var parametri = { dipartimentoScelto: dipartimento };

                store.load({ params: parametri });
                store.on({
                    'load': {
                        fn: function(store, records, options) {
                            if (store != undefined && store != null)
                                onLoadFn(store);
                        },
                        scope: this
                    }
                });

                return store;
            }

            function initUfficiCombo(store) {
                var comboUffici = Ext.getCmp('comboUfficiSearchField');

                if (store.getCount() != 0) {
                    comboUffici.setValue(store.getAt(0).data.CodiceInterno);

                    comboUffici.enable();
                    Ext.getCmp('labelUfficiId').enable();

                    Ext.getCmp('btnCercaDetermine').enable();
                } else {
                    comboUffici.disable();
                    Ext.getCmp('labelUfficiId').disable();

                    Ext.getCmp('btnCercaDetermine').disable();
                }
            }

            function buildPanelRicercaDetermine() {
                var storeUffici = new Ext.data.SimpleStore({
                    fields: ['CodiceInterno', 'DescrizioneBreve'],
                    data: [],
                    autoLoad: true
                });

                var comboUffici = new Ext.form.ComboBox({
                    displayField: 'DescrizioneBreve',
                    valueField: 'CodiceInterno',
                    id: 'comboUfficiSearchField',
                    listWidth: 400,
                    store: storeUffici,
                    readOnly: true,
                    mode: 'local',
                    queryMode: 'local',
                    width: 400,
                    triggerAction: 'all',
                    emptyText: 'Selezionare un ufficio...',
                    lastQuery: '',
                    listeners: {
                        select: {
                            fn: function(combo, value) {
                            }
                        }
                    }
                });

                var comboDipartimenti = new Ext.form.ComboBox({
                    id: 'comboDipartimentiSearchField',
                    store: getDipartimentiStore(initDipartimentiCombo),
                    displayField: 'DescrizioneBreve',
                    valueField: 'CodiceInterno',
                    hideTrigger: false,
                    width: 400,
                    listWidth: 400,
                    queryMode: 'local',
                    readOnly: true,
                    allowBlank: false,
                    mode: 'local',
                    emptyText: 'Selezionare un dipartimento...',
                    typeAhead: true,
                    forceSelection: true,
                    triggerAction: 'all',
                    selectOnFocus: true,
                    listeners: {
                        select: {
                            fn: function(combo, value) {
                                var codiceDipartimento = combo.store.data.get(combo.selectedIndex).data.CodiceInterno;

                                var comboUffici = Ext.getCmp('comboUfficiSearchField');

                                comboUffici.clearValue();
                                comboUffici.bindStore(getUfficiStore(codiceDipartimento, initUfficiCombo));
                            }
                        }
                    }
                });

                var risultatiRicercaDetermine = new Ext.grid.GridPanel({
                    id: 'risultatiRicercaDetermine',
                    cm: new Ext.grid.ColumnModel({}),
                    sm: new Ext.grid.RowSelectionModel({}),
                    store: new Ext.data.SimpleStore({ fields: ['id'], data: ['G'] })
                });

                Ext.override(Ext.layout.TableLayout, {
                    tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 0, cn: { tag: 'tbody'} },
                    onLayout: function(ct, target) {
                        var cs = ct.items.items, len = cs.length, c, i;
                        if (!this.table) {
                            target.addClass('x-table-layout-ct');
                            this.table = target.createChild(this.tableCfg, null, true);
                            this.renderAll(ct, target);
                        }
                    }
                });

                var ricercaDetermine = new Ext.Panel({
                    id: 'panelRicercaDetermine',
                    labelAlign: 'top',
                    bodyStyle: 'padding:10px',
                    width: 650,
                    height: 350,
                    autoScroll: true,
                    items: [
    {
        xtype: 'panel',
        plain: true,
        title: 'Criteri di ricerca',
        border: true,
        layout: 'table',
        style: 'margin-top:10px',
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 10px;' : 'padding:10px 10px 10px 10px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        items: [
            {
                xtype: 'label',
                style: 'padding-right:20px',
                text: 'Data da'
            }, {
                xtype: 'datefield',
                fieldLabel: 'DT_DataDaSearchField',
                name: 'DT_DataDaSearchField',
                id: 'DT_DataDaSearchField',
                format: 'd-m-Y',
                altFormats: 'd/m/Y',
                emptyText: getStartDate(3),
                width: 120
            },
            {
                xtype: 'label',
                style: 'padding-right:20px',
                text: 'Data a'
            }, {
                xtype: 'datefield',
                fieldLabel: 'DT_DataASearchField',
                name: 'DT_DataASearchField',
                id: 'DT_DataASearchField',
                format: 'd-m-Y',
                altFormats: 'd/m/Y',
                emptyText: getEndDate(),
                width: 120
            },
            {
                xtype: 'label',
                style: 'padding-right:20px',
                id: 'labelDipartimentiId',
                text: 'Dipartimento'
            }, comboDipartimenti,
            {
                xtype: 'label',
                style: 'padding-right:20px',
                id: 'labelUfficiId',
                text: 'Ufficio'
            }, comboUffici,
            {
                xtype: 'label',
                text: 'Numero'
            }, {
                xtype: 'textfield',
                name: 'DT_NumeroSearchField',
                id: 'DT_NumeroSearchField',
                width: 120,
                allowBlank: true,
                maxLength: 5,
                maxLengthText: 'La lunghezza massima è 5 caratteri',
                validator: function(valu) {
                    var pattern = '^[0-9]{1,5}$';
                    var stringa = valu.trim();
                    var result = stringa.search(pattern);
                    return result > -1;
                },
                invalidText: 'Il numero determina inserito non è corretto'
            },
            {
                xtype: 'label',
                text: 'Oggetto'
            }, {
                xtype: 'textfield',
                name: 'DT_OggettoSearchField',
                id: 'DT_OggettoSearchField',
                width: 400,
                allowBlank: true
            }
        ],
        buttons: [{
            text: 'Cerca',
            id: 'btnCercaDetermine'
        }
    ]
    },
    risultatiRicercaDetermine,
      {
          xtype: 'hidden',
          id: 'idDocumentoHidden'
      },
      {
          xtype: 'hidden',
          id: 'numeroDocumentoHidden'
      },
      {
          xtype: 'hidden',
          id: 'oggettoDocumentoHidden'
      },
      {
          xtype: 'hidden',
          id: 'dataDocumentoHidden'
      },
      {
          xtype: 'hidden',
          id: 'documentoConsultabileHidden'
      },
      {
          xtype: 'hidden',
          id: 'tipologiaDocumentoHidden'
      }
       ]
                });

                Ext.getCmp('btnCercaDetermine').on('click', function() {
                    var dataDa = Ext.get('DT_DataDaSearchField').getValue();
                    dataDa = (dataDa != null && dataDa != undefined) ? dataDa : "";

                    var dataA = Ext.get('DT_DataASearchField').getValue();
                    dataA = (dataA != null && dataA != undefined) ? dataA : "";

                    var codDip = comboDipartimenti.findRecord(comboDipartimenti.valueField || comboDipartimenti.displayField, comboDipartimenti.getValue());
                    codDip = (codDip != null && codDip != undefined) ? codDip.data.CodicePubblico : "";

                    var codUff = comboUffici.findRecord(comboUffici.valueField || comboUffici.displayField, comboUffici.getValue());
                    codUff = (codUff != null && codUff != undefined) ? codUff.data.CodicePubblico : "";

                    var numDoc = Ext.get('DT_NumeroSearchField').getValue();
                    numDoc = (numDoc != null && numDoc != undefined) ? numDoc : "";

                    var oggettoDoc = Ext.get('DT_OggettoSearchField').getValue();
                    oggettoDoc = (oggettoDoc != null && oggettoDoc != undefined) ? oggettoDoc : "";

                    if (Ext.getCmp('GridRisultatiRicercaDocumenti') != undefined)
                        Ext.getCmp('risultatiRicercaDetermine').remove(Ext.getCmp('GridRisultatiRicercaDocumenti'));

                    var gridRisultatiRicercaDetermine = buildPanelRisultatiRicercaDocumenti(0, dataDa, dataA, undefined,
        codDip, codUff, numDoc, oggettoDoc);

                    if (gridRisultatiRicercaDetermine != null && gridRisultatiRicercaDetermine != undefined)
                        Ext.getCmp('risultatiRicercaDetermine').add(gridRisultatiRicercaDetermine);

                    Ext.getCmp('risultatiRicercaDetermine').show();
                    Ext.getCmp("panelRicercaDetermine").doLayout();
                });

                comboUffici.disable();
                Ext.getCmp('labelUfficiId').disable();

                comboDipartimenti.disable();
                Ext.getCmp('labelDipartimentiId').disable();

                Ext.getCmp('risultatiRicercaDetermine').hide();
                Ext.getCmp('btnCercaDetermine').disable();

                return ricercaDetermine;
            }

            function getDipartimentiDelibereStore(tipoStruttura, fnOnLoad) {
                var proxy = new Ext.data.HttpProxy({
                    url: 'ProcAmm.svc/GetDipartimentiDelibere',
                    method: 'GET'
                });

                var reader = new Ext.data.JsonReader({

                    root: 'GetDipartimentiDelibereResult',
                    fields: [
           { name: 'CodiceInterno' },
           { name: 'DescrizioneBreve' }
           ]
                });

                var store = new Ext.data.Store({
                    proxy: proxy,
                    reader: reader
                });

                var parametri = { tipoStruttura: tipoStruttura };
                store.load({ params: parametri });

                store.on({
                    'load': {
                        fn: function(store, records, options) {
                            if (fnOnLoad != undefined && fnOnLoad != null)
                                fnOnLoad(store);
                        },
                        scope: this
                    }
                });
                return store;
            }


            function buildPanelStrutturaProponente() {
                var emptyDipartimentiStore = new Ext.data.SimpleStore({
                    fields: ['CodiceInterno', 'DescrizioneBreve'],
                    data: [],
                    autoLoad: true
                });

                var comboDipartimenti = new Ext.form.ComboBox({
                    id: 'comboDipartimentiDelibereSearchField',
                    store: getDipartimentiDelibereStore("attuale"),
                    displayField: 'DescrizioneBreve',
                    valueField: 'CodiceInterno',
                    hideTrigger: false,
                    width: 400,
                    listWidth: 400,
                    queryMode: 'local',
                    mode: 'local',
                    emptyText: 'Selezionare un dipartimento...',
                    typeAhead: true,
                    forceSelection: true,
                    triggerAction: 'all',
                    selectOnFocus: true,
                    listeners: {
                        select: {
                            fn: function(combo, value) {
                                var codiceDipartimento = combo.store.data.get(combo.selectedIndex).data.CodiceInterno;
                            }
                        }
                    }
                });

                var STRUTTURA_ATTUALE = new Ext.form.Radio({
                    boxLabel: 'Attuale',
                    id: 'STRUTTURA_ATTUALE',
                    name: 'selezioneTipoStruttura',
                    inputValue: 'STRUTTURA_ATTUALE',
                    checked: true
                });

                STRUTTURA_ATTUALE.on('check', function() {
                    if (STRUTTURA_ATTUALE.checked) {
                        comboDipartimenti.clearValue();
                        comboDipartimenti.bindStore(getDipartimentiDelibereStore("attuale"));
                    }
                });

                var STRUTTURA_PRECEDENTE = new Ext.form.Radio({
                    boxLabel: 'Precedente',
                    id: 'STRUTTURA_PRECEDENTE',
                    name: 'selezioneTipoStruttura',
                    inputValue: 'STRUTTURA_PRECEDENTE',
                    checked: false
                });

                STRUTTURA_PRECEDENTE.on('check', function() {
                    if (STRUTTURA_PRECEDENTE.checked) {
                        comboDipartimenti.clearValue();
                        comboDipartimenti.bindStore(getDipartimentiDelibereStore("vl"));
                    }
                });

                var gestioneStrutturaProponente = new Ext.Panel({
                    id: 'panelGestioneStrutturaProponente',
                    labelAlign: 'top',
                    bodyStyle: 'margin:' + (Ext.isIE ? '8px' : '10px') + ' 0px 10px 0px',
                    border: false,
                    items:
                [{
                    xtype: 'panel',
                    border: false,
                    layout: 'table',
                    layoutConfig: {
                        columns: 3
                    },
                    items: [STRUTTURA_ATTUALE,
                        {
                            xtype: 'label',
                            style: 'padding-right:10px',
                            text: ''
                        },
                        STRUTTURA_PRECEDENTE]
                },
                  comboDipartimenti
                ]
                });

                return gestioneStrutturaProponente;
            }

            function buildPanelRicercaDelibere() {

                var risultatiRicercaDelibere = new Ext.grid.GridPanel({
                    id: 'risultatiRicercaDelibere',
                    cm: new Ext.grid.ColumnModel({}),
                    sm: new Ext.grid.RowSelectionModel({}),
                    store: new Ext.data.SimpleStore({ fields: ['id'], data: ['G'] })
                });

                Ext.override(Ext.layout.TableLayout, {
                    tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 0, cn: { tag: 'tbody'} },
                    onLayout: function(ct, target) {
                        var cs = ct.items.items, len = cs.length, c, i;
                        if (!this.table) {
                            target.addClass('x-table-layout-ct');
                            this.table = target.createChild(this.tableCfg, null, true);
                            this.renderAll(ct, target);
                        }
                    }
                });

                var ricercaDelibere = new Ext.Panel({
                    id: 'panelRicercaDelibere',
                    labelAlign: 'top',
                    bodyStyle: 'padding:10px',
                    width: 650,
                    height: 350,
                    autoScroll: true,
                    items: [
        {
            xtype: 'panel',
            plain: true,
            title: 'Criteri di ricerca',
            border: true,
            layout: 'table',
            style: 'margin-top:10px',
            bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 10px;' : 'padding:10px 10px 10px 10px',
            layoutConfig: {
                columns: 2,
                tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
            },
            items: [
            {
                xtype: 'label',
                style: 'padding-right:20px',
                text: 'Data da'
            }, {
                xtype: 'datefield',
                fieldLabel: 'DT_DataDaSearchField',
                name: 'DT_DataDaSearchField',
                id: 'DT_DataDaSearchField',
                format: 'd-m-Y',
                altFormats: 'd/m/Y',
                emptyText: getStartDate(3),
                width: 120
            },
            {
                xtype: 'label',
                style: 'padding-right:20px',
                text: 'Data a'
            }, {
                xtype: 'datefield',
                fieldLabel: 'DT_DataASearchField',
                name: 'DT_DataASearchField',
                id: 'DT_DataASearchField',
                format: 'd-m-Y',
                altFormats: 'd/m/Y',
                emptyText: getEndDate(),
                width: 120
            },
            {
                xtype: 'label',
                style: 'padding-right:20px',
                text: 'Strutture Proponenti'
            },
                buildPanelStrutturaProponente(),
            {
                xtype: 'label',
                text: 'Numero'
            }, {
                xtype: 'textfield',
                name: 'DT_NumeroSearchField',
                id: 'DT_NumeroSearchField',
                width: 120,
                allowBlank: true,
                maxLength: 7,
                maxLengthText: 'La lunghezza massima è 7 caratteri',
                validator: function(valu) {
                    var pattern = '^[0-9]{1,7}$';
                    var stringa = valu.trim();
                    var result = stringa.search(pattern);
                    return result > -1;
                },
                invalidText: 'Il numero determina inserito non è corretto'
            },
            {
                xtype: 'label',
                text: 'Oggetto'
            }, {
                xtype: 'textfield',
                name: 'DT_OggettoSearchField',
                id: 'DT_OggettoSearchField',
                width: 400,
                allowBlank: true
            }
        ],
            buttons: [{
                text: 'Cerca',
                id: 'btnCercaDelibere'
            }
        ]
        },
        risultatiRicercaDelibere,
          {
              xtype: 'hidden',
              id: 'idDocumentoHidden'
          },
          {
              xtype: 'hidden',
              id: 'numeroDocumentoHidden'
          },
          {
              xtype: 'hidden',
              id: 'oggettoDocumentoHidden'
          },
          {
              xtype: 'hidden',
              id: 'dataDocumentoHidden'
          },
          {
              xtype: 'hidden',
              id: 'documentoConsultabileHidden'
          },
          {
              xtype: 'hidden',
              id: 'tipologiaDocumentoHidden'
          }
           ]
                });

                Ext.getCmp('btnCercaDelibere').on('click', function() {
                    var dataDa = Ext.get('DT_DataDaSearchField').getValue();
                    dataDa = (dataDa != null && dataDa != undefined) ? dataDa : "";

                    var dataA = Ext.get('DT_DataASearchField').getValue();
                    dataA = (dataA != null && dataA != undefined) ? dataA : "";

                    var tipoStruttura = Ext.getCmp('STRUTTURA_ATTUALE').checked ? "attuale" : "vl";

                    var comboDipartimenti = Ext.getCmp('comboDipartimentiDelibereSearchField');
                    var codDip = comboDipartimenti.findRecord(comboDipartimenti.valueField || comboDipartimenti.displayField, comboDipartimenti.getValue());
                    codDip = (codDip != null && codDip != undefined) ? codDip.data.CodiceInterno : "";

                    var codUff = null;

                    var numDoc = Ext.get('DT_NumeroSearchField').getValue();
                    numDoc = (numDoc != null && numDoc != undefined) ? numDoc : "";

                    var oggettoDoc = Ext.get('DT_OggettoSearchField').getValue();
                    oggettoDoc = (oggettoDoc != null && oggettoDoc != undefined) ? oggettoDoc : "";

                    if (Ext.getCmp('GridRisultatiRicercaDocumenti') != undefined)
                        Ext.getCmp('risultatiRicercaDelibere').remove(Ext.getCmp('GridRisultatiRicercaDocumenti'));

                    var gridRisultatiRicercaDelibere = buildPanelRisultatiRicercaDocumenti(2, dataDa, dataA,
            tipoStruttura, codDip, codUff, numDoc, oggettoDoc);

                    if (gridRisultatiRicercaDelibere != null && gridRisultatiRicercaDelibere != undefined)
                        Ext.getCmp('risultatiRicercaDelibere').add(gridRisultatiRicercaDelibere);

                    Ext.getCmp('risultatiRicercaDelibere').show();
                    Ext.getCmp("panelRicercaDelibere").doLayout();
                });

                Ext.getCmp('risultatiRicercaDelibere').hide();
                Ext.getCmp('btnCercaDelibere').enable();

                return ricercaDelibere;
            }

            function showPopupPanelRicercaDocumenti(tipoDocumento, tipoDocumentoDisplayName) {
                var popup = new Ext.Window({
                    title: 'Ricerca ' + tipoDocumentoDisplayName,
                    width: 700,
                    height: tipoDocumento == 0 ? 540 : 555,
                    layout: 'fit',
                    plain: true,
                    bodyStyle: 'padding:10px',
                    resizable: false,
                    maximizable: false,
                    enableDragDrop: true,
                    collapsible: false,
                    modal: true,
                    autoScroll: true,
                    closable: true,
                    buttons: [{
                        text: 'Inserisci',
                        id: 'btnSelezionaDocumento'
}]
                    });

                    var panelRicercaDocumenti = null;

                    if (tipoDocumento == 0)
                        panelRicercaDocumenti = buildPanelRicercaDetermine();
                    else if (tipoDocumento == 2)
                        panelRicercaDocumenti = buildPanelRicercaDelibere();

                    if (panelRicercaDocumenti != null)
                        popup.add(panelRicercaDocumenti);

                    popup.doLayout();
                    popup.show();

                    Ext.getCmp('btnSelezionaDocumento').disable();

                    Ext.getCmp('btnSelezionaDocumento').on('click', function() {
                        var numeroDocumento = Ext.get("numeroDocumentoHidden").value;
                        var oggettoDocumento = Ext.get("oggettoDocumentoHidden").value;
                        var documentoConsultabile = Ext.get("documentoConsultabileHidden").value;
                        var tipologiaDocumento = Ext.get("tipologiaDocumentoHidden").value;

                        var dati = null;
                        if (tipoDocumento == 0) {
                            dati = parseNumeroDetermina(numeroDocumento);
                            if (dati != null && dati != undefined)
                                setDatiDetermina(dati.ufficio, dati.anno, dati.numero, oggettoDocumento, tipologiaDocumento);

                            var msg = "Determina '" + buildNumeroDetermina(dati.ufficio, dati.anno, dati.numero) + "' presente in archivio.";
                            var msgLevel = MSG_LEVEL.info;

                            if (documentoConsultabile != null && documentoConsultabile != undefined && !documentoConsultabile) {
                                msg = msg + "<br>L'operatore non è abilitato alla consultazione delle determine dell'ufficio '" + dati.ufficio + "'. " +
                 "Sarà comunque possibile utilizzare il numero di determina specificato quale " +
                 "norma per l'attribuzione del beneficio.";
                                msgLevel = MSG_LEVEL.warning;
                            }

                            setDeterminaMsg(msg, msgLevel, false, false);
                        }
                        else if (tipoDocumento == 2) {
                            dati = parseNumeroDelibera(numeroDocumento);
                            if (dati != null && dati != undefined)
                                setDatiDelibera(dati.anno, dati.numero, oggettoDocumento, tipologiaDocumento);
                            setDeliberaMsg("Delibera '" + buildNumeroDelibera(dati.anno, dati.numero) + "' presente in archivio.", MSG_LEVEL.info, false, false);
                        }

                        if (dati == null || dati == undefined) {
                            Ext.MessageBox.show({
                                title: 'Errore',
                                msg: 'Il numero del documento selezionato non è valido o non è disponibile.',
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.ERROR,
                                fn: function(btn) { return }
                            });
                        }
                        else
                            popup.close();
                    });
                }

                function setDatiDelibera(anno, numero, oggetto, tipologiaDocumento) {
                    Ext.getCmp('annoDelibera').setValue(anno);
                    Ext.getCmp('numeroDelibera').setValue(numero);

                    if (oggetto == null || oggetto == undefined || oggetto.trim().length == 0) {
                        Ext.getCmp('oggettoDeliberaLabel').disable();
                        Ext.getCmp('oggettoDelibera').disable();
                        Ext.getCmp('oggettoDelibera').setValue("");
                    } else {
                        Ext.getCmp('oggettoDeliberaLabel').enable();
                        Ext.getCmp('oggettoDelibera').enable();
                        Ext.getCmp('oggettoDelibera').setValue(oggetto);
                    }

                    if (tipologiaDocumento == null || tipologiaDocumento == undefined || tipologiaDocumento.trim().length == 0) {
                        Ext.getCmp('tipologiaDocumentoDeliberaLabel').disable();
                        Ext.getCmp('tipologiaDocumentoDelibera').disable();
                        Ext.getCmp('tipologiaDocumentoDelibera').setText("<div style='height:12px;width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'></div>", false);
                    } else {
                        Ext.getCmp('tipologiaDocumentoDeliberaLabel').enable();
                        Ext.getCmp('tipologiaDocumentoDelibera').enable();
                        Ext.getCmp('tipologiaDocumentoDelibera').setText("<div style='width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'>" + tipologiaDocumento + "</div>", false);
                    }
                }

                function setDatiDetermina(ufficio, anno, numero, oggetto, tipologiaDocumento) {
                    Ext.getCmp('ufficioDetermina').setValue(ufficio);
                    Ext.getCmp('annoDetermina').setValue(anno);
                    Ext.getCmp('numeroDetermina').setValue(numero);

                    if (oggetto == null || oggetto == undefined || oggetto.trim().length == 0) {
                        Ext.getCmp('oggettoDeterminaLabel').disable();
                        Ext.getCmp('oggettoDetermina').disable();
                        Ext.getCmp('oggettoDetermina').setValue("");
                    } else {
                        Ext.getCmp('oggettoDeterminaLabel').enable();
                        Ext.getCmp('oggettoDetermina').enable();
                        Ext.getCmp('oggettoDetermina').setValue(oggetto);
                    }

                    if (tipologiaDocumento == null || tipologiaDocumento == undefined || tipologiaDocumento.trim().length == 0) {
                        Ext.getCmp('tipologiaDocumentoDeterminaLabel').disable();
                        Ext.getCmp('tipologiaDocumentoDetermina').disable();
                        Ext.getCmp('tipologiaDocumentoDetermina').setText("<div style='height:12px;width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'></div>", false);
                    } else {
                        Ext.getCmp('tipologiaDocumentoDeterminaLabel').enable();
                        Ext.getCmp('tipologiaDocumentoDetermina').enable();
                        Ext.getCmp('tipologiaDocumentoDetermina').setText("<div style='width:" + (Ext.isIE ? "415px" : "417px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:black;font-size:10px;'>" + tipologiaDocumento + "</div>", false);
                    }
                }

                function setDatiAltraNorma(altraNorma) {
                    Ext.getCmp('normaAttribuzioneAltroDescrizione').setValue(altraNorma);
                }

                function enableDatiDetermina(enable) {
                    if (enable) {
                        Ext.getCmp('ufficioDetermina').enable();
                        Ext.getCmp('annoDetermina').enable();
                        Ext.getCmp('numeroDetermina').enable();
                    } else {
                        Ext.getCmp('ufficioDetermina').disable();
                        Ext.getCmp('annoDetermina').disable();
                        Ext.getCmp('numeroDetermina').disable();
                    }
                }

                function enableDatiDelibera(enable) {
                    if (enable) {
                        Ext.getCmp('annoDelibera').enable();
                        Ext.getCmp('numeroDelibera').enable();
                    } else {
                        Ext.getCmp('annoDelibera').disable();
                        Ext.getCmp('numeroDelibera').disable();
                    }
                }

                function enableDatiAltraNorma(enable) {
                    if (enable)
                        Ext.getCmp('normaAttribuzioneAltroDescrizione').enable();
                    else
                        Ext.getCmp('normaAttribuzioneAltroDescrizione').disable();
                }

                function parseNumeroDetermina(numeroDetermina, prefix, suffix) {
                    var retValue = null;

                    if (numeroDetermina != null && numeroDetermina != undefined && numeroDetermina.trim().length != 0) {
                        var numeroDeterminaRE = "[A-Za-z0-9]{4}\.[0-9]{4}/[d,D]{1}\.[0-9]{5}";
                        var allRE = numeroDeterminaRE;

                        if (prefix != null && prefix != undefined)
                            allRE = "^" + prefix + allRE;

                        if (suffix != null && suffix != undefined)
                            allRE = allRE + suffix + "$";

                        if (allRE != numeroDeterminaRE) {
                            if (numeroDetermina.match(allRE) == null)
                                return retValue;
                        }

                        var matchedExpression = null;
                        if ((matchedExpression = numeroDetermina.match(numeroDeterminaRE)) != null) {
                            var ufficioDetermina = matchedExpression[0].substr(0, 4);
                            var annoDetermina = matchedExpression[0].substr(5, 4);
                            var numeroDetermina = matchedExpression[0].substr(12, 5);

                            retValue = { ufficio: ufficioDetermina,
                                anno: annoDetermina,
                                numero: numeroDetermina
                            };
                        }
                    }

                    return retValue;
                }

                function parseNumeroDelibera(numeroDelibera, prefix, suffix) {
                    var retValue = null;

                    if (numeroDelibera != null && numeroDelibera != undefined && numeroDelibera.trim().length != 0) {
                        var numeroDeliberaRE_a = "[0-9]{1,7}/[0-9]{4}";
                        var numeroDeliberaRE_b = "[0-9]{11}";

                        var allRE = numeroDeliberaRE_a;

                        if (prefix != null && prefix != undefined)
                            allRE = "^" + prefix + allRE;

                        if (suffix != null && suffix != undefined)
                            allRE = allRE + suffix + "$";

                        if (allRE != numeroDeliberaRE_a) {
                            if (numeroDelibera.match(allRE) == null) {
                                allRE = numeroDeliberaRE_b;

                                if (prefix != null && prefix != undefined)
                                    allRE = "^" + prefix + allRE;

                                if (suffix != null && suffix != undefined)
                                    allRE = allRE + suffix + "$";

                                if (allRE != numeroDeliberaRE_a) {
                                    if (numeroDelibera.match(allRE) == null)
                                        return retValue;
                                }
                            }
                        }

                        var matchedExpression = null;
                        if ((matchedExpression = numeroDelibera.match(numeroDeliberaRE_a)) != null) {
                            var index = matchedExpression[0].indexOf("/");

                            var numeroDelibera = matchedExpression[0].substring(0, index);
                            var annoDelibera = matchedExpression[0].substring(index + 1);

                            retValue = { anno: annoDelibera,
                                numero: numeroDelibera
                            };
                        } else if ((matchedExpression = numeroDelibera.match(numeroDeliberaRE_b)) != null) {
                            var annoDelibera = matchedExpression[0].substr(0, 4);
                            var numeroDelibera = matchedExpression[0].substr(4, 7);

                            retValue = { anno: annoDelibera,
                                numero: numeroDelibera
                            };
                        }
                    }
                    return retValue;
                }


                function buildPanelRisultatiRicercaDocumenti(tipoDoc, dataDa, dataA, tipoStruttura, codDip, codUff, numDoc, oggettoDoc) {
                    var maskApp = new Ext.LoadMask(Ext.getBody(), {
                        msg: "Recupero Dati..."
                    });

                    var proxy = new Ext.data.HttpProxy({
                        url: 'ProcAmm.svc/InterrogaDocumenti',
                        method: 'POST',
                        timeout: 10000000
                    });

                    var reader = new Ext.data.JsonReader({
                        root: 'InterrogaDocumentiResult.Data',
                        totalProperty: 'InterrogaDocumentiResult.TotalCount',
                        fields: [
           { name: 'Doc_Id' },
           { name: 'Doc_Numero' },
           { name: 'Doc_Oggetto' },
           { name: 'Doc_Data' },
           { name: 'Doc_IdTipologiaDocumento' },
           { name: 'Doc_TipologiaDocumento' },
           { name: 'IsConsultabile' }
        ]
                    });

                    var store = new Ext.data.Store({
                        proxy: proxy,
                        reader: reader,
                        sortInfo: { field: "Doc_Numero", direction: "ASC" },
                        listeners: {
                            'loadexception': function(proxy, options, response) {
                                maskApp.hide();
                                if (Ext.decode(response.responseText).FaultCode != GET_DOCUMENT_RESULT_STATUS.documentsNotFoundOnNotQueryableOffice) {
                                    Ext.MessageBox.show({
                                        title: '',
                                        msg: Ext.decode(response.responseText).FaultMessage,
                                        buttons: Ext.Msg.OK,
                                        closable: false,
                                        icon: Ext.MessageBox.ERROR
                                    });
                                }
                            }
                        }
                    });

                    store.on({ 'load': {
                        fn: function(store, records, options) {
                            if (records.length == 0) {
                                if (Ext.getCmp('btnSelezionaDocumento') != null && Ext.getCmp('btnSelezionaDocumento') != undefined)
                                    Ext.getCmp('btnSelezionaDocumento').disable();
                            }
                            maskApp.hide();
                        },
                        scope: this
                    }
                    });

                    maskApp.show();

                    var parametri = {
                        start: 0,
                        tipoDoc: tipoDoc,
                        dataDa: dataDa,
                        dataA: dataA,
                        tipoStruttura: tipoStruttura,
                        codDip: codDip,
                        codUff: codUff,
                        numDoc: numDoc,
                        oggettoDoc: oggettoDoc
                    };

                    try {
                        store.load({ params: parametri });
                    } catch (ex) {
                        maskApp.hide();
                    }

                    var sm = new Ext.grid.CheckboxSelectionModel({
                        singleSelect: true,
                        loadMask: true
                    });

                    var pagingToolbar = new Ext.PagingToolbar({
                        id: 'pagingToolbar',
                        pageSize: 10,
                        store: store,
                        displayInfo: true,
                        beforePageText: 'Pagina',
                        afterPageText: 'di {0}',
                        displayMsg: "Risultati {0} - {1} di {2}",
                        emptyMsg: "Nessun documento trovato"
                    });

                    pagingToolbar.on('beforechange', function(pt, params) {
                        params.tipoDoc = tipoDoc,
        params.dataDa = dataDa,
        params.dataA = dataA,
        params.tipoStruttura = tipoStruttura,
        params.codDip = codDip,
        params.codUff = codUff,
        params.numDoc = numDoc,
        params.oggettoDoc = oggettoDoc
                    });

                    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Numero", width: 120, dataIndex: 'Doc_Numero', sortable: true, locked: true },
        { header: "Oggetto", width: 370, dataIndex: 'Doc_Oggetto', sortable: true, locked: false },
        { header: "Data", width: 80, dataIndex: 'Doc_Data', sortable: true, locked: false },
        { header: "Id", width: 100, dataIndex: 'Doc_Id', hidden: true }
        ]);

                    var grid = new Ext.grid.GridPanel({
                        id: 'GridRisultatiRicercaDocumenti',
                        title: 'Risultati della ricerca',
                        autoHeight: false,
                        height: 180,
                        border: false,
                        viewConfig: { forceFit: true },
                        ds: store,
                        cm: ColumnModel,
                        stripeRows: true,
                        viewConfig: {
                            emptyText: "Nessun atto corrisponde ai criteri di ricerca inseriti.",
                            deferEmptyText: false
                        },
                        bbar: pagingToolbar,
                        sm: sm
                    });

                    grid.addListener({
                        'rowclick': {
                            fn: function(grid, rowIndex, event) {
                                var rec = grid.store.getAt(rowIndex);
                                if (grid.getSelectionModel().getSelected() != null &&
            rec.data.Doc_Numero != null && rec.data.Doc_Numero != undefined && rec.data.Doc_Numero.trim().length != 0) {
                                    Ext.get("idDocumentoHidden").value = rec.data.Doc_Id;
                                    Ext.get("numeroDocumentoHidden").value = rec.data.Doc_Numero;
                                    Ext.get("oggettoDocumentoHidden").value = rec.data.Doc_Oggetto;
                                    Ext.get("dataDocumentoHidden").value = rec.data.Doc_Data;
                                    Ext.get("documentoConsultabileHidden").value = rec.data.IsConsultabile;
                                    Ext.get("tipologiaDocumentoHidden").value = rec.data.Doc_TipologiaDocumento;
                                    Ext.getCmp('btnSelezionaDocumento').enable();
                                }
                                else {
                                    Ext.get("idDocumentoHidden").value = "";
                                    Ext.get("numeroDocumentoHidden").value = "";
                                    Ext.get("oggettoDocumentoHidden").value = "";
                                    Ext.get("dataDocumentoHidden").value = "";
                                    Ext.get("documentoConsultabileHidden").value = false;
                                    Ext.get("tipologiaDocumentoHidden").value = "";
                                    Ext.getCmp('btnSelezionaDocumento').disable();
                                }
                            }
    , scope: this
                        }
                    });

                    return grid;
                }

                function getStartDate(monthToSub) {
                    var dt = new Date();
                    dt.setMonth(dt.getMonth() - monthToSub);
                    return getDateAsString(dt);
                }

                function getEndDate() {
                    var date = new Date();
                    return getDateAsString(date);
                }

                function getDateAsString(d) {
                    var curr_date = ("0" + d.getDate()).slice(-2);
                    var curr_month = ("0" + (d.getMonth() + 1)).slice(-2);
                    var curr_year = d.getFullYear();

                    return curr_date + "-" + curr_month + "-" + curr_year;
                }

                function buildNumeroDetermina(ufficio, anno, numero) {
                    var retValue = null;
                    if (ufficio != null && ufficio != undefined && (ufficio.length > 0 && ufficio.length < 5) &&
        anno != null && anno != undefined && (anno.length > 0 && anno.length < 5) &&
        numero != null && numero != undefined && (numero.length > 0 && numero.length < 6)) {
                        retValue = ufficio + "." + anno + "/D." + ("00000" + numero).slice(-5);
                    }
                    return retValue;
                }

                function buildNumeroDelibera(anno, numero) {
                    var retValue = null;
                    if (anno != null && anno != undefined && (anno.length > 0 && anno.length < 5) &&
        numero != null && numero != undefined && (numero.length > 0 && numero.length < 8)) {
                        retValue = ("0000000" + numero).slice(-7) + "/" + anno;
                    }
                    return retValue;
                }

                function getDetermina(ufficio, anno, numero, fnOnLoad, useMask, panelIdForMask, showMessageBox) {
                    if (fnOnLoad != null && fnOnLoad != undefined) {
                        if (parseNumeroDetermina(buildNumeroDetermina(ufficio, anno, numero))) {

                            var mask = null;
                            if ((useMask != null && useMask != undefined && useMask) ||
                (panelIdForMask != null && panelIdForMask != undefined)) {
                                mask = new Ext.LoadMask(
                (panelIdForMask != null && panelIdForMask != undefined
                    && panelIdForMask.trim().length != 0) ? Ext.getCmp(panelIdForMask).el : Ext.getBody(), {
                        msg: "Verifica in corso..."
                    });
                                enableDatiDetermina(false);
                                mask.show();
                            }

                            Ext.Ajax.request({
                                url: 'ProcAmm.svc/InterrogaDocumenti?tipoDoc=0&dataDa=01-01-' + anno + '&dataA=31-12-' + anno + '&codUff=' + ufficio + '&numDoc=' + numero + '&validateUfficio=false',
                                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                                method: 'POST',
                                success: function(response, options) {
                                    if (mask != null && mask != undefined) {
                                        mask.hide();
                                        enableDatiDetermina(true);
                                    }
                                    var data = Ext.decode(response.responseText);
                                    if (data.InterrogaDocumentiResult.TotalCount <= 1) {
                                        fnOnLoad(ufficio, anno, numero, GET_DOCUMENT_RESULT_STATUS.success, "success",
                    data.InterrogaDocumentiResult.TotalCount == 1 ? data.InterrogaDocumentiResult.Data[0] : null, showMessageBox);
                                    } else
                                        fnOnLoad(ufficio, anno, numero, GET_DOCUMENT_RESULT_STATUS.error, "I criteri di ricerca specificati hanno prodotto più di un risultato. Criteri non validi.", null, showMessageBox);
                                },
                                failure: function(response, options) {
                                    if (mask != null && mask != undefined) {
                                        mask.hide();
                                        enableDatiDetermina(true);
                                    }
                                    var data = Ext.decode(response.responseText);
                                    fnOnLoad(ufficio, anno, numero, data.FaultCode, data.FaultMessage, null, showMessageBox);
                                }
                            });
                        } else
                            fnOnLoad(ufficio, anno, numero, GET_DOCUMENT_RESULT_STATUS.error, "Criteri di ricerca specificati non validi.", null, showMessageBox);
                    }
                }

                function getDelibera(anno, numero, fnOnLoad, useMask, panelIdForMask, showMessageBox) {
                    if (fnOnLoad != null && fnOnLoad != undefined) {
                        if (parseNumeroDelibera(buildNumeroDelibera(anno, numero))) {
                            var mask = null;
                            if ((useMask != null && useMask != undefined && useMask) ||
                (panelIdForMask != null && panelIdForMask != undefined)) {
                                mask = new Ext.LoadMask(
                (panelIdForMask != null && panelIdForMask != undefined
                    && panelIdForMask.trim().length != 0) ? Ext.getCmp(panelIdForMask).el : Ext.getBody(), {
                        msg: "Verifica in corso..."
                    });
                                enableDatiDelibera(false);
                                mask.show();
                            }

                            Ext.Ajax.request({
                                url: 'ProcAmm.svc/InterrogaDocumenti?tipoDoc=2&dataDa=01-01-' + anno + '&dataA=31-12-' + anno + '&numDoc=' + numero,
                                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                                method: 'POST',
                                success: function(response, options) {
                                    if (mask != null && mask != undefined) {
                                        mask.hide();
                                        enableDatiDelibera(true);
                                    }
                                    var data = Ext.decode(response.responseText);
                                    if (data.InterrogaDocumentiResult.TotalCount <= 1) {
                                        fnOnLoad(anno, numero, GET_DOCUMENT_RESULT_STATUS.success, "success",
                    data.InterrogaDocumentiResult.TotalCount == 1 ? data.InterrogaDocumentiResult.Data[0] : null, showMessageBox);
                                    } else
                                        fnOnLoad(anno, numero, GET_DOCUMENT_RESULT_STATUS.error, "I criteri di ricerca specificati hanno prodotto più di un risultato. Criteri non validi.", null, showMessageBox);
                                },
                                failure: function(response, options) {
                                    if (mask != null && mask != undefined) {
                                        mask.hide();
                                        enableDatiDelibera(true);
                                    }
                                    var data = Ext.decode(response.responseText);
                                    fnOnLoad(anno, numero, data.FaultCode, data.FaultMessage, null, showMessageBox);
                                }
                            });
                        } else
                            fnOnLoad(anno, numero, GET_DOCUMENT_RESULT_STATUS.error, "Criteri di ricerca specificati non validi.", null, showMessageBox);
                    }
                }

                function setDeterminaMsg(msg, msgLevel, stopOnValidate, showMessageBox) {
                    if (msg != null && msg != undefined && msg.length > 0) {
                        Ext.getCmp('determinaErrorPanel').show();
                        var color = "red";
                        if (msgLevel == MSG_LEVEL.info)
                            color = "green";
                        else if (msgLevel == MSG_LEVEL.warning)
                            color = "orange";
                        else if (msgLevel == MSG_LEVEL.error)
                            color = "red";
                        else
                            msgLevel = MSG_LEVEL.error;

                        Ext.getCmp('determinaErrorLabel').setText("<div style='width:" + (Ext.isIE ? "474px" : "476px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:" + color + ";font-size:10px;'><b>" + msg + "</b></div>", false);
                        Ext.getCmp('determinaErrorFlag').setText((stopOnValidate == true || MSG_LEVEL.error == msgLevel) ? "true" : "false");

                        if (showMessageBox != null && showMessageBox != undefined && showMessageBox == true &&
    (msgLevel == MSG_LEVEL.error || msgLevel == MSG_LEVEL.warning)) {
                            Ext.MessageBox.show({
                                title: (msgLevel == MSG_LEVEL.error) ? 'Errore' : 'Attenzione',
                                msg: msg,
                                buttons: Ext.MessageBox.OK,
                                icon: (msgLevel == MSG_LEVEL.error) ? Ext.MessageBox.ERROR : Ext.MessageBox.WARNING,
                                fn: function(btn) { return }
                            });
                        }
                    } else {
                        Ext.getCmp('determinaErrorPanel').hide();

                        Ext.getCmp('determinaErrorLabel').setText("");
                        Ext.getCmp('determinaErrorFlag').setText("false");
                    }
                }

                function setDeliberaMsg(msg, msgLevel, stopOnValidate, showMessageBox) {
                    if (msg != null && msg != undefined && msg.length > 0) {
                        Ext.getCmp('deliberaErrorPanel').show();
                        var color = "red";
                        if (msgLevel == MSG_LEVEL.info)
                            color = "green";
                        else if (msgLevel == MSG_LEVEL.warning)
                            color = "orange";
                        else if (msgLevel == MSG_LEVEL.error)
                            color = "red";
                        else
                            msgLevel = MSG_LEVEL.error;

                        Ext.getCmp('deliberaErrorLabel').setText("<div style='width:" + (Ext.isIE ? "474px" : "476px") + ";border:1px solid #B5B8C8;padding:3px;background-color:white;color:" + color + ";font-size:10px;'><b>" + msg + "</b></div>", false);
                        Ext.getCmp('deliberaErrorFlag').setText((stopOnValidate == true || MSG_LEVEL.error == msgLevel) ? "true" : "false");

                        if (showMessageBox != null && showMessageBox != undefined && showMessageBox == true &&
            (msgLevel == MSG_LEVEL.error || msgLevel == MSG_LEVEL.warning)) {
                            Ext.MessageBox.show({
                                title: msgLevel == MSG_LEVEL.error ? 'Errore' : 'Attenzione',
                                msg: msg,
                                buttons: Ext.MessageBox.OK,
                                icon: msgLevel == MSG_LEVEL.error ? Ext.MessageBox.ERROR : Ext.MessageBox.WARNING,
                                fn: function(btn) { return }
                            });
                        }
                    } else {
                        Ext.getCmp('deliberaErrorPanel').hide();

                        Ext.getCmp('deliberaErrorLabel').setText("");
                        Ext.getCmp('deliberaErrorFlag').setText("false");
                    }
                }

//                function enableDatiContratto(enable) {
//                    if (enable) {
//                        Ext.getCmp('listaContratti').enable();
//                    } else {
//                        Ext.getCmp('listaContratti').disable();
//                    }
//                }

                function enableContenutoAtto(enable) {
                    if (enable) {
                        Ext.getCmp('contenutoAtto').enable();
                    } else {
                        Ext.getCmp('contenutoAtto').disable();
                    }
                }

                function showDatiContratto(show) {
                    if (show) {
                        Ext.getCmp('contrattoLabel').show();
                        Ext.getCmp('panelContratto').show();
                    } else {
                        Ext.getCmp('contrattoLabel').hide();
                        Ext.getCmp('panelContratto').hide();
                    }
                }

                function showContenutoAtto(show) {
                    if (show) {
                        Ext.getCmp('contenutoAttoLabel').show();
                        Ext.getCmp('contenutoAtto').show();
                    } else {
                        Ext.getCmp('contenutoAttoLabel').hide();
                        Ext.getCmp('contenutoAtto').hide();
                    }
                }

                function enableFieldsForAutorizzazionePubblicazioneToNo(enable) {
                    if (enable) {
                        Ext.getCmp('noteAutorizzazionePubblicazione').enable();
                    } else {
                        Ext.getCmp('noteAutorizzazionePubblicazione').disable();
                    }
                }

                function enableFieldsForAutorizzazionePubblicazioneToYes(enable) {
                    enableDatiDetermina(enable ? Ext.getCmp('DETERMINA').checked : false);
                    enableDatiDelibera(enable ? Ext.getCmp('DELIBERA').checked : false);
                    enableDatiAltraNorma(enable ? Ext.getCmp('ALTRO').checked : false);

                    if (enable) {
                        Ext.getCmp('DETERMINA').enable();
                        Ext.getCmp('DELIBERA').enable();
                        Ext.getCmp('ALTRO').enable();

                        Ext.getCmp('ufficioResponsabileDelProcedimento').enable();
                        Ext.getCmp('funzionarioResponsabileDelProcedimento').enable();
                        Ext.getCmp('modalitaIndividuazioneBeneficiario').enable();
                    } else {
                        Ext.getCmp('DETERMINA').disable();
                        Ext.getCmp('DELIBERA').disable();
                        Ext.getCmp('ALTRO').disable();

                        Ext.getCmp('ufficioResponsabileDelProcedimento').disable();
                        Ext.getCmp('funzionarioResponsabileDelProcedimento').disable();
                        Ext.getCmp('modalitaIndividuazioneBeneficiario').disable();
                    }
                }

                function showFieldsForAutorizzazionePubblicazioneToNo(show) {
                    if (show) {
                        Ext.getCmp('noteAutorizzazionePubblicazione').show();
                        Ext.getCmp('noteAutorizzazionePubblicazioneLabel').show();
                    } else {
                        Ext.getCmp('noteAutorizzazionePubblicazione').hide();
                        Ext.getCmp('noteAutorizzazionePubblicazioneLabel').hide();
                    }
                }

                function showFieldsForAutorizzazionePubblicazioneToYes(show) {
                    if (show) {
                        Ext.getCmp('normaAttribuzioneBeneficioLabel').show();
                        Ext.getCmp('panelGestioneNormaAttribuzioneBeneficio').show();

                        Ext.getCmp('ufficioResponsabileDelProcedimentoLabel').show();
                        Ext.getCmp('ufficioResponsabileDelProcedimento').show();

                        Ext.getCmp('funzionarioResponsabileDelProcedimentoLabel').show();
                        Ext.getCmp('funzionarioResponsabileDelProcedimento').show();

                        Ext.getCmp('modalitaIndividuazioneBeneficiarioLabel').show();
                        Ext.getCmp('modalitaIndividuazioneBeneficiario').show();
                    } else {
                        Ext.getCmp('normaAttribuzioneBeneficioLabel').hide();
                        Ext.getCmp('panelGestioneNormaAttribuzioneBeneficio').hide();

                        Ext.getCmp('ufficioResponsabileDelProcedimentoLabel').hide();
                        Ext.getCmp('ufficioResponsabileDelProcedimento').hide();

                        Ext.getCmp('funzionarioResponsabileDelProcedimentoLabel').hide();
                        Ext.getCmp('funzionarioResponsabileDelProcedimento').hide();

                        Ext.getCmp('modalitaIndividuazioneBeneficiarioLabel').hide();
                        Ext.getCmp('modalitaIndividuazioneBeneficiario').hide();
                    }
                }

                function openContratto(idContratto) {
                    var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
                    var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

                    window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + idContratto, '_blank');
                }

//                function buildPanelGridContratti() {

//                    var aggiungiContratto = new Ext.Action({
//                        text: 'Aggiungi Contratto/i',
//                        id: 'actionAggiungiContratto',
//                        tooltip: 'Seleziona e aggiunge uno più contratti alla lista',
//                        handler: function() {
//                            showPopupPanelRicercaContratti();
//                        }
//                    });

//                    var rimuoviContratto = new Ext.Action({
//                        text: 'Rimuovi Contratto/i',
//                        id: 'actionRimuoviContratto',
//                        tooltip: 'Rimuove uno o più contratti dalla lista',
//                        handler: function() {
//                            var gridContratti = Ext.getCmp('GridContratti');
//                            var storeGridContratti = gridContratti.getStore();
//                            var selections = gridContratti.getSelectionModel().getSelections();

//                            Ext.MessageBox.buttonText.cancel = "Annulla";

//                            Ext.MessageBox.show({
//                                title: 'Attenzione',
//                                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'del contratto selezionato' : 'dei contratti selezionati') + '?',
//                                buttons: Ext.MessageBox.OKCANCEL,
//                                icon: Ext.MessageBox.WARNING,
//                                fn: function(btn) {
//                                    if (btn == 'ok') {
//                                        if (removeContrattiFn(getDestinatariAssociati(), "destinatario")) {
//                                            var idDocumento = Ext.getDom('codDocumento').value;
//                                            if (!(idDocumento == null || idDocumento == undefined || idDocumento.trim() == ''))
//                                                getBeneficiariContratti(idDocumento, null, removeContrattiFn);
//                                        }
//                                    }
//                                }
//                            });
//                        }
//                    });

//                    var sm = new Ext.grid.CheckboxSelectionModel({
//                        singleSelect: false,
//                        loadMask: true,
//                        listeners: {
//                            rowselect: function(selectionModel, rowIndex, record) {
//                                var totalRows = Ext.getCmp('GridContratti').store.getRange().length;
//                                var selectedRows = selectionModel.getSelections();
//                                if (selectedRows.length > 0) {
//                                    rimuoviContratto.enable();
//                                }
//                                if (totalRows == selectedRows.length) {
//                                    var view = Ext.getCmp('GridContratti').getView();
//                                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
//                                    chkdiv.addClass("x-grid3-hd-checker-on");
//                                }
//                            },
//                            rowdeselect: function(selectionModel, rowIndex, record) {
//                                var selectedRows = selectionModel.getSelections();
//                                if (selectedRows.length == 0) {
//                                    rimuoviContratto.disable();
//                                }
//                                var view = Ext.getCmp('GridContratti').getView();
//                                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
//                                chkdiv.removeClass('x-grid3-hd-checker-on');
//                            }
//                        }
//                    });

//                    var ColumnModel = new Ext.grid.ColumnModel([
//        sm,
//        { header: "Repertorio", width: 100, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
//            renderer: function(value, metaData, record, rowIdx, colIdx, store) {
//                var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
//                var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

//                var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.Id;
//                var link = "<a target='_blank' href='" + href + "' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorio + "</a>";

//                return link;
//            }
//        },
//        { header: "Oggetto", width: 360, dataIndex: 'Oggetto', sortable: true, locked: false },
//        { header: "Id", width: 100, dataIndex: 'Id', hidden: true }
//        ]);

//                    var store = new Ext.data.SimpleStore({
//                        fields: ['Id', 'NumeroRepertorio', 'Oggetto']
//                    });

//                    var grid = new Ext.grid.GridPanel({
//                        id: 'GridContratti',
//                        autoHeight: false,
//                        height: 130,
//                        border: true,
//                        viewConfig: { forceFit: true },
//                        ds: store,
//                        cm: ColumnModel,
//                        width: 500,
//                        stripeRows: true,
//                        viewConfig: {
//                            emptyText: "Nessun contratto associato.",
//                            deferEmptyText: false
//                        },
//                        sm: sm
//                    });

//                    grid.on('render', function() {
//                        this.getView().mainBody.on('mousedown', function(e, t) {
//                            if (t.tagName == 'A') {
//                                e.stopEvent();
//                                t.click();
//                            }
//                        });
//                    }, grid);

//                    var gridContratti = new Ext.Panel({
//                        autoHeight: true,
//                        xtype: 'panel',
//                        border: false,
//                        layout: 'table',
//                        layoutConfig: {
//                            columns: 1
//                        },
//                        items: [grid,
//                {
//                    xtype: 'panel',
//                    plain: true,
//                    border: false,
//                    layout: 'table',
//                    style: 'margin-left:268px;margin-top:4px',
//                    layoutConfig: {
//                        columns: 3
//                    },
//                    items: [new Ext.LinkButton(aggiungiContratto),
//                            { xtype: 'label', style: 'margin-left:10px' },
//                            new Ext.LinkButton(rimuoviContratto)
//                    ]
//}]
//                    }
//    );

//                    rimuoviContratto.disable();

//                    return gridContratti;
//                }

//                function isNullOrEmpty(value) {
//                    return (value == null || value == undefined || value == "")
//                }

//                function setContrattiAssociati() {
//                    var gridContratti = Ext.getCmp('GridContratti');

//                    if (gridContratti != undefined && gridContratti != null) {
//                        var storeGridContratti = gridContratti.getStore();

//                        if (storeGridContratti != undefined && storeGridContratti != null) {
//                            var contrattiAsJsonObject = '';

//                            storeGridContratti.each(function(storeGridContratti) {
//                                contrattiAsJsonObject += Ext.util.JSON.encode(storeGridContratti.data) + ',';
//                            });

//                            contrattiAsJsonObject = contrattiAsJsonObject.substring(0, contrattiAsJsonObject.length - 1);
//                            Ext.getDom('listaContratti').value = contrattiAsJsonObject;
//                        }
//                    }
//                }

//                function getContrattiAssociati() {
//                    var retValue = [];
//                    var gridContratti = Ext.getCmp('GridContratti');

//                    if (gridContratti != undefined && gridContratti != null) {
//                        var storeGridContratti = gridContratti.getStore();

//                        if (storeGridContratti != undefined && storeGridContratti != null) {
//                            storeGridContratti.each(function(storeGridContratti) {
//                                retValue.push(storeGridContratti.data);
//                            });
//                        }
//                    }
//                    return retValue;
//                }

//                function refreshSchedaLeggeTrasparenzaPanel() {
//                    refreshPanelGridContratti();
//                }

//                function refreshPanelGridContratti() {
//                    var gridContratti = Ext.getCmp('GridContratti');
//                    gridContratti.getView().refresh();
//                }


//                function getBeneficiariContratti(idDocumento, idContratto, fnOnLoad) {
//                    var proxy = new Ext.data.HttpProxy({
//                        url: 'ProcAmm.svc/GetListaBeneficiariContratti' + (idDocumento != null && idDocumento != undefined && idDocumento.trim() != '' ? '?idDocumento=' + idDocumento : '') +
//            (idContratto != null && idContratto != undefined && idContratto.trim() != '' ? '&idContratto=' + idContratto : ''),
//                        method: 'GET',
//                        timeout: 900000
//                    });

//                    var reader = new Ext.data.JsonReader({
//                        root: 'GetListaBeneficiariContrattiResult',
//                        fields: [
//       { name: 'CodiceFiscale' },
//       { name: 'Denominazione' },
//       { name: 'ImportoSpettante' },
//       { name: 'ID' },
//       { name: 'NLiquidazione' },
//       { name: 'IdAnagrafica' },
//       { name: 'IDLiquidazione' },
//       { name: 'IdDocumento' },
//       { name: 'SedeVia' },
//       { name: 'SedeComune' },
//       { name: 'SedeProvincia' },
//       { name: 'Cig' },
//       { name: 'Cup' },
//       { name: 'CodiceSiope' },
//       { name: 'Iban' },
//       { name: 'PartitaIva' },
//       { name: 'FlagPersonaFisica' },
//       { name: 'IdModalitaPag' },
//       { name: 'DescrizioneModalitaPag' },
//       { name: 'HasDatiBancari' },
//       { name: 'IdConto' },
//       { name: 'IdSede' },
//       { name: 'IdContratto' },
//       { name: 'NumeroRepertorioContratto' }
//       ]
//                    });

//                    var store = new Ext.data.Store({
//                        proxy: proxy,
//                        reader: reader,
//                        listeners: {
//                            'loadexception': function(proxy, options, response) {
//                                maskApp.hide();
//                                Ext.MessageBox.show({
//                                    title: 'Elimina Contratti',
//                                    msg: "Errore durante il caricamento dei contratti associati ai beneficiari:<br>" +
//                     "'" + Ext.decode(response.responseText).FaultMessage + "'",
//                                    buttons: Ext.Msg.OK,
//                                    closable: false,
//                                    icon: Ext.MessageBox.ERROR
//                                });
//                            }
//                        }
//                    });

//                    var maskApp = new Ext.LoadMask(Ext.getBody(), {
//                        msg: "Recupero Dati..."
//                    });

//                    maskApp.show();

//                    try {
//                        store.load();
//                    } catch (ex) {
//                        maskApp.hide();
//                    }

//                    store.on({
//                        'load': {
//                            fn: function(store, records, options) {
//                                maskApp.hide();
//                                if (fnOnLoad != null && fnOnLoad != undefined)
//                                    fnOnLoad(records);
//                            },
//                            scope: this
//                        }
//                    });
//                }

//                function removeContrattiFn(beneficiariContratti, beneficiariLabel) {
//                    var gridContratti = Ext.getCmp('GridContratti');
//                    var storeGridContratti = gridContratti.getStore();
//                    var selections = gridContratti.getSelectionModel().getSelections();

//                    var unableToRemove = null;
//                    var found = false;

//                    Ext.each(selections, function(rec) {
//                        if (!found) {
//                            if (beneficiariContratti != null && beneficiariContratti != undefined) {
//                                for (var i = 0; i < beneficiariContratti.length; i++) {
//                                    if (beneficiariContratti[i].data.IdContratto == rec.data.Id) {
//                                        found = true;
//                                        break;
//                                    }
//                                }
//                            }

//                            if (!found)
//                                storeGridContratti.remove(rec);
//                            else
//                                unableToRemove = "'" + rec.data.NumeroRepertorio + "' (" + rec.data.Oggetto + ")";
//                        }
//                    });

//                    if (found) {
//                        var warning_message = "Non è possibile eliminare il contratto avente numero di repertorio " +
//                unableToRemove + " poichè associato ad un " +
//                (beneficiariLabel != undefined && beneficiariLabel != null && beneficiariLabel.trim() != '' ? beneficiariLabel : 'beneficiario') + " del provvedimento. Operazione interrotta.";

//                        Ext.MessageBox.show({
//                            title: 'Attenzione',
//                            msg: warning_message,
//                            buttons: Ext.MessageBox.OK,
//                            icon: Ext.MessageBox.WARNING,
//                            fn: function(btn) {
//                                Ext.getCmp('actionRimuoviContratto').disable();

//                                Ext.getCmp('GridContratti').getSelectionModel().clearSelections();

//                                var view = Ext.getCmp('GridContratti').getView();
//                                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
//                                chkdiv.removeClass('x-grid3-hd-checker-on');
//                            }
//                        });
//                    } else {
//                        Ext.getCmp('actionRimuoviContratto').disable();

//                        var view = Ext.getCmp('GridContratti').getView();
//                        var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
//                        chkdiv.removeClass('x-grid3-hd-checker-on');
//                    }

//                    return !found;
//                }