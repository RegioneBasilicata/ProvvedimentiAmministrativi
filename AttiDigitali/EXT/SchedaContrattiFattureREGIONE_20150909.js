function validateSchedaContrattiFattureFields(panelSchedaId) {
    var retValue = null;

    if (!Ext.getCmp(panelSchedaId).disabled) {
        var isValid = false;
        var errorMsg = '';
        
        var gridContratti = Ext.getCmp('GridContratti');
        var storeGridContratti = gridContratti.getStore();

        storeGridContratti.each(function(storeGridContratti) {
            var contratto = storeGridContratti.data;
            var a = 1;
            var contrattoConFattura = false;
            var gridFatture = Ext.getCmp('GridFatture');
            var storeGridFatture = gridFatture.getStore();
            storeGridFatture.each(function(storeGridFatture) {
                var fattura = storeGridFatture.data;
                var a = 1;
                if (contratto.Id == fattura.Contratto.Id) {
                    contrattoConFattura = true;
                }
            });
            if (!contrattoConFattura && (isOpContImpLiq || isOpContLiq || isOpContImpLiqPerenti)) {
                errorMsg += "Al contratto" + contratto.NumeroRepertorio + " non è stata associata nessuna fattura.<br/>";
                isValid = false;
                return;
            } else if (!contrattoConFattura) {
                isValid = false;
            } else {
                isValid = true;
            }
        });

        if (!isValid && (isOpContImpLiq || isOpContLiq || isOpContImpLiqPerenti)) {
            errorMsg = "Scheda 'Contratti-Fatture' incompleta.<br/>" + errorMsg;
            retValue = { tab_to_activate: panelSchedaId, msg: errorMsg };
        }
        
        
        

//        if ((Ext.getCmp('yesPubblicazione') == undefined || Ext.getCmp('yesPubblicazione').checked == false) &&
//            (Ext.getCmp('noPubblicazione') == undefined || Ext.getCmp('noPubblicazione').checked == false)) {
//            errorMsg += "E' necessario specificare se le informazioni della scheda sono o meno soggette a pubblicazione.<br/>";
//        } else if (Ext.getCmp('noPubblicazione') != undefined && Ext.getCmp('noPubblicazione').checked == true) {
//            if (Ext.getCmp('noteAutorizzazionePubblicazione') == undefined || !Ext.getCmp('noteAutorizzazionePubblicazione').validate())
//                errorMsg += "Verificare che il motivo per il quale non si autorizza la pubblicazione delle informazioni presenti nella scheda sia specificato (max 1024 caratteri).<br/>";
//            else
//                isValid = true;
//        } else if ((Ext.getCmp('DETERMINA') != undefined && Ext.getCmp('DETERMINA').checked && !validateDatiDetermina()) ||
//                   (Ext.getCmp('DELIBERA') != undefined && Ext.getCmp('DELIBERA').checked && !validateDatiDelibera()) ||
//                   (Ext.getCmp('ALTRO') != undefined && Ext.getCmp('ALTRO').checked &&
//                    (Ext.getCmp('normaAttribuzioneAltroDescrizione') == undefined || !Ext.getCmp('normaAttribuzioneAltroDescrizione').validate()))) {
//            errorMsg += "Verificare che le informazioni fornite per la norma o l'atto in base al quale viene attribuito il beneficio siano specificate e valide (max 1024 caratteri).<br/>";
//        } else if ((Ext.getCmp('ufficioResponsabileDelProcedimento') == undefined || !Ext.getCmp('ufficioResponsabileDelProcedimento').validate())) {
//            errorMsg += "E' necessario specificare l'ufficio responsabile del procedimento.";
//        } else if ((Ext.getCmp('funzionarioResponsabileDelProcedimento') == undefined || !Ext.getCmp('funzionarioResponsabileDelProcedimento').validate())) {
//            errorMsg += "E' necessario specificare il responsabile del procedimento.";
//        } else if ((Ext.getCmp('modalitaIndividuazioneBeneficiario') == undefined || !Ext.getCmp('modalitaIndividuazioneBeneficiario').validate())) {
//            errorMsg += "E' necessario specificare la modalità di individuazione del beneficiario/destinatario.<br/>";
//        } else if ((Ext.getCmp('determinaErrorFlag') != undefined && Ext.getCmp('determinaErrorFlag').text == 'true' && Ext.getCmp('DETERMINA') != undefined && Ext.getCmp('DETERMINA').checked) ||
//                   (Ext.getCmp('deliberaErrorFlag') != undefined && Ext.getCmp('deliberaErrorFlag').text == 'true' && Ext.getCmp('DELIBERA') != undefined && Ext.getCmp('DELIBERA').checked)) {
//            errorMsg += "Verificare che le informazioni fornite per la norma o l'atto in base al quale viene attribuito il beneficio siano specificate e valide.<br/>";
//        } else
//            isValid = true;

//        if ((Ext.getCmp('contenutoAtto') == undefined || !Ext.getCmp('contenutoAtto').validate())) {
//            errorMsg += "Verificare che il contenuto dell'atto sia specificato (max 1000 caratteri).<br/>";
//            isValid = false;
//        }
//        if (!isValid) {
//            errorMsg = "Scheda 'Legge Trasparenza' incompleta.<br/>" + errorMsg;
//            retValue = { tab_to_activate: panelSchedaId, msg: errorMsg };
//        }
    }

    return retValue;
}



function fnOnSelectContratto(selectedRow) {
    if (selectedRow != null) {
        var contratto = selectedRow.data;

        Ext.get("idContratto").value = contratto.ID;

        if (Ext.getCmp('GridRisultatiRicercaContratti') != undefined) {
            Ext.getCmp('panelRicercaContratti').remove(Ext.getCmp('GridRisultatiRicercaContratti'));
        }

        Ext.getCmp('panelFattura').show();
        setActivePanel('panelRicercaContratti', 'panelFattura');
        Ext.getCmp('panelFattura').doLayout();

    } else {
        Ext.getCmp("panelFattura").disable();
    }
}

function buildSchedaContrattiFatturePanel() {

    var panelDataSchedaContrattiFatture = new Ext.Panel({
        xtype: "panel",
        title: "",
        id: 'panelDataSchedaContrattiFatture',
        layout: 'table',
        layoutConfig: {
            columns: 2
        },
        border: false,
        width: 750,
        style: {
            "margin-left": "4px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        
        items: [
        
         {
             xtype: 'label',
             id: 'contrattoLabel',
             style: "margin-right:5px",
             text: 'Contratto/i'
         },
            buildPanelContratti()
         ,
         {
             xtype: 'label',
             id: 'fatturaLabel',
             style: "margin-right:5px",
             text: 'Fattura/e'
         },
            buildPanelFatture()
         
         ]
    });

    var panelContenitore = new Ext.Panel({
        xtype: "panel",
        id: 'panelContenitore',
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        width: 760,
        border: false,
        items: [
                panelDataSchedaContrattiFatture
            ]
    });

    enableDatiContratto(true);
    enableDatiFattura(true);
    if (schedaContrattiFattureInfo != null && schedaContrattiFattureInfo != undefined)
        setDatiSchedaContrattiFattureOnLoad(schedaContrattiFattureInfo);


    return panelContenitore;
}


function setContrattiAssociati() {
    var gridContratti = Ext.getCmp('GridContratti');

    if (gridContratti != undefined && gridContratti != null) {
        var storeGridContratti = gridContratti.getStore();

        if (storeGridContratti != undefined && storeGridContratti != null) {
            var contrattiAsJsonObject = '';

            storeGridContratti.each(function(storeGridContratti) {
                contrattiAsJsonObject += Ext.util.JSON.encode(storeGridContratti.data) + ',';
            });

            contrattiAsJsonObject = contrattiAsJsonObject.substring(0, contrattiAsJsonObject.length - 1);
            Ext.getDom('listaContratti').value = contrattiAsJsonObject;
        }
    }
}

function getContrattiAssociati() {
    var retValue = [];
    var gridContratti = Ext.getCmp('GridContratti');

    if (gridContratti != undefined && gridContratti != null) {
        var storeGridContratti = gridContratti.getStore();

        if (storeGridContratti != undefined && storeGridContratti != null) {
            storeGridContratti.each(function(storeGridContratti) {
                retValue.push(storeGridContratti.data);
            });
        }
    }
    return retValue;
}


function setFattureAssociate() {
    var gridFatture = Ext.getCmp('GridFatture');

    if (gridFatture != undefined && gridFatture != null) {
        var storeGridFatture = gridFatture.getStore();

        if (storeGridFatture != undefined && storeGridFatture != null) {
            var fattureAsJsonObject = '';

            storeGridFatture.each(function(storeGridFatture) {
                fattureAsJsonObject += Ext.util.JSON.encode(storeGridFatture.data) + ',';
            });

            fattureAsJsonObject = fattureAsJsonObject.substring(0, fattureAsJsonObject.length - 1);
            Ext.getDom('listaFatture').value = fattureAsJsonObject;
        }
    }
}


function getFattureAssociate() {
    var retValue = [];
    var gridFatture = Ext.getCmp('GridFatture');

    if (gridFatture != undefined && gridFatture != null) {
        var storeGridFatture = gridFatture.getStore();

        if (storeGridFatture != undefined && storeGridFatture != null) {
            storeGridFatture.each(function(storeGridFatture) {
                retValue.push(storeGridFatture.data);
            });
        }
    }
    return retValue;
}

function getFattureDaAssociare() {
    var retValue = [];
    var gridFatture = Ext.getCmp('GridRisultatiRicercaFatture');

   if (gridFatture != undefined && gridFatture != null) {
//        var storeGridFatture = gridFatture.getSelectionModel().getSelections();

        var selections = gridFatture.getSelectionModel().getSelections();

        var unableToRemove = null;
        var found = false;
        if (selections != undefined && selections != null) {
            Ext.each(selections, function(rec) {
                retValue.push(rec.data);
            });
        }
//        if (storeGridFatture != undefined && storeGridFatture != null) {
//            storeGridFatture.each(function(storeGridFatture) {
//                retValue.push(storeGridFatture.data);
//            });
//        }
    }
    return retValue;
}

function removeContrattiFn(beneficiariContratti, beneficiariLabel) {
    var gridContratti = Ext.getCmp('GridContratti');
    var storeGridContratti = gridContratti.getStore();
    var selections = gridContratti.getSelectionModel().getSelections();

    var unableToRemove = null;
    var found = false;

    Ext.each(selections, function(rec) {
        if (!found) {
            if (beneficiariContratti != null && beneficiariContratti != undefined) {
                for (var i = 0; i < beneficiariContratti.length; i++) {
                    if (beneficiariContratti[i].data.IdContratto == rec.data.Id) {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                storeGridContratti.remove(rec);
            else
                unableToRemove = "'" + rec.data.NumeroRepertorio + "' (" + rec.data.Oggetto + ")";
        }
    });

    if (found) {
        var warning_message = "Non è possibile eliminare il contratto avente numero di repertorio " +
                unableToRemove + " poichè associato ad un " +
                (beneficiariLabel != undefined && beneficiariLabel != null && beneficiariLabel.trim() != '' ? beneficiariLabel : 'beneficiario') + " del provvedimento. Operazione interrotta.";

        Ext.MessageBox.show({
            title: 'Attenzione',
            msg: warning_message,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.WARNING,
            fn: function(btn) {
                Ext.getCmp('actionRimuoviContratto').disable();

                Ext.getCmp('GridContratti').getSelectionModel().clearSelections();

                var view = Ext.getCmp('GridContratti').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        });
    } else {
        Ext.getCmp('actionRimuoviContratto').disable();

        var view = Ext.getCmp('GridContratti').getView();
        var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
        chkdiv.removeClass('x-grid3-hd-checker-on');
    }

    return !found;
}


function removeFattureFn(listaFatture) {
    var gridFatture = Ext.getCmp('GridFatture');
    var storeGridFatture = gridFatture.getStore();
    var selections = gridFatture.getSelectionModel().getSelections();
    
    var unableToRemove = null;
    var found = false;

    Ext.each(selections, function(rec) {
        if (!found) {
            storeGridFatture.remove(rec);
        }
    });
    
    Ext.getCmp('actionRimuoviContratto').disable();

    var view = Ext.getCmp('GridFatture').getView();
    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
    chkdiv.removeClass('x-grid3-hd-checker-on');

    return !found;
}

function getBeneficiariContratti(idDocumento, idContratto, fnOnLoad) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaBeneficiariContratti' + (idDocumento != null && idDocumento != undefined && idDocumento.trim() != '' ? '?idDocumento=' + idDocumento : '') +
            (idContratto != null && idContratto != undefined && idContratto.trim() != '' ? '&idContratto=' + idContratto : ''),
        method: 'GET',
        timeout: 900000
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaBeneficiariContrattiResult',
        fields: [
       { name: 'CodiceFiscale' },
       { name: 'Denominazione' },
       { name: 'ImportoSpettante' },
       { name: 'ID' },
       { name: 'NLiquidazione' },
       { name: 'IdAnagrafica' },
       { name: 'IDLiquidazione' },
       { name: 'IdDocumento' },
       { name: 'SedeVia' },
       { name: 'SedeComune' },
       { name: 'SedeProvincia' },
       { name: 'Cig' },
       { name: 'Cup' },
       { name: 'CodiceSiope' },
       { name: 'Iban' },
       { name: 'PartitaIva' },
       { name: 'FlagPersonaFisica' },
       { name: 'IdModalitaPag' },
       { name: 'DescrizioneModalitaPag' },
       { name: 'HasDatiBancari' },
       { name: 'IdConto' },
       { name: 'IdSede' },
       { name: 'IdContratto' },
       { name: 'NumeroRepertorioContratto' }
       ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        listeners: {
            'loadexception': function(proxy, options, response) {
                maskApp.hide();
                Ext.MessageBox.show({
                    title: 'Elimina Contratti',
                    msg: "Errore durante il caricamento dei contratti associati ai beneficiari:<br>" +
                     "'" + Ext.decode(response.responseText).FaultMessage + "'",
                    buttons: Ext.Msg.OK,
                    closable: false,
                    icon: Ext.MessageBox.ERROR
                });
            }
        }
    });

    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    maskApp.show();

    try {
        store.load();
    } catch (ex) {
        maskApp.hide();
    }

    store.on({
        'load': {
            fn: function(store, records, options) {
                maskApp.hide();
                if (fnOnLoad != null && fnOnLoad != undefined)
                    fnOnLoad(records);
            },
            scope: this
        }
    });
}

function buildPanelGridContratti() {

    var aggiungiContratto = new Ext.Action({
        text: 'Aggiungi Contratti e Fatture',
        id: 'actionAggiungiContratto',
        tooltip: 'Seleziona e aggiunge uno più contratti alla lista',
        handler: function() {
            showPopupPanelRicercaContratti();
        }
    });

    var rimuoviContratto = new Ext.Action({
        text: 'Rimuovi Contratti e Fatture',
        id: 'actionRimuoviContratto',
        tooltip: 'Rimuove uno o più contratti dalla lista',
        handler: function() {
            var gridContratti = Ext.getCmp('GridContratti');

            var storeGridContratti = gridContratti.getStore();
            var selections = gridContratti.getSelectionModel().getSelections();

            Ext.MessageBox.buttonText.cancel = "Annulla";


            var gridFatture = Ext.getCmp('GridFatture');
            var storeGridfatture = gridFatture.getStore();

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'del contratto selezionato' : 'dei contratti selezionati') + '?',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'ok') {
                        if (removeContrattiFn(getDestinatariAssociati(), "destinatario")) {
                            var idDocumento = Ext.getDom('codDocumento').value;
                            if (!(idDocumento == null || idDocumento == undefined || idDocumento.trim() == ''))
                                getBeneficiariContratti(idDocumento, null, removeContrattiFn);
                        }

                        var listaFatture = getFattureAssociate();
                        var numeroFattureGrid = listaFatture.length;
                        Ext.each(selections, function(rec) {

                        for (var i = listaFatture.length - 1; i > -1; i--) {
                                if (listaFatture[i].Contratto.Id == rec.data.Id) {
                                    storeGridfatture.data.keys.remove(storeGridfatture.data.keys[i]);
                                    storeGridfatture.data.items.remove(storeGridfatture.data.items[i]);
                                    numeroFattureGrid = numeroFattureGrid - 1;
                                    storeGridfatture.data.length = (numeroFattureGrid);
                                }
                            }
                        });
                        setFattureAssociate();
                        gridFatture.getView().refresh();

                    }
                }
            });
        }
    });

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridContratti').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviContratto.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridContratti').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviContratto.disable();
                }
                var view = Ext.getCmp('GridContratti').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Repertorio", width: 100, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
            renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
                var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

                var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.Id;
                var link = "<a target='_blank' href='" + href + "' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorio + "</a>";

                return link;
            }
        },
        { header: "Oggetto", width: 350, dataIndex: 'Oggetto', sortable: true, locked: false },
        { header: "CIG", width: 75, dataIndex: 'CodiceCIG', sortable: true, locked: false },
        { header: "CUP", width: 100, dataIndex: 'CodiceCUP', sortable: true, locked: false },
        { header: "Id", dataIndex: 'Id', hidden: true }
        ]);

    var store = new Ext.data.SimpleStore({
        fields: ['Id', 'NumeroRepertorio', 'Oggetto', 'CodiceCIG', 'CodiceCUP']
    });

    var grid = new Ext.grid.GridPanel({
        id: 'GridContratti',
        autoHeight: false,
        height: 130,
        width: 670,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,        
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessun contratto associato.",
            deferEmptyText: false
        },
        sm: sm
    });

    grid.on('render', function() {
        this.getView().mainBody.on('mousedown', function(e, t) {
            if (t.tagName == 'A') {
                e.stopEvent();
                t.click();
            }
        });
    }, grid);

    var gridContratti = new Ext.Panel({
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        items: [grid,
                {
                    xtype: 'panel',
                    plain: true,
                    border: false,
                    layout: 'table',
                    style: 'margin-left:420px;margin-top:4px',
                    layoutConfig: {
                        columns: 3
                    },
                    items: [new Ext.LinkButton(aggiungiContratto),
                            { xtype: 'label', style: 'margin-left:10px' },
                            new Ext.LinkButton(rimuoviContratto)
                    ]
}]
    }
    );

    rimuoviContratto.disable();

    return gridContratti;
}


function enableDatiContratto(enable) {
    if (enable) {
        Ext.getCmp('listaContratti').enable();
    } else {
        Ext.getCmp('listaContratti').disable();
    }
}

function enableDatiFattura(enable) {
    if (enable) {
        Ext.getCmp('listaFatture').enable();
    } else {
    Ext.getCmp('listaFatture').disable();
    }
}

function verificaEsistenzaFatture() {
    var esiste = false;
    var stdec = Ext.lib.Ajax.defaultPostHeader
    Ext.lib.Ajax.defaultPostHeader = 'application/json';

    var maskFatture = new Ext.LoadMask(Ext.getBody(), {
        msg: "Verifica fatture da aggiungere..."

    });
    var fatture = getFattureDaAssociare();
    if (fatture != undefined) {
        maskFatture.show()
        
        var params = { fattureDaAggiungere: fatture };
//        var params = { };
        Ext.Ajax.timeout = 100000000

        Ext.MessageBox.wait('Loading ...');
        var box = Ext.Ajax.request({
            async: false,
            url: 'ProcAmm.svc/VerificaFattureDaAggiungere',
            params: Ext.encode(params),
            method: 'POST',
            success: function(response, options) {
                maskFatture.hide();

                var data = Ext.decode(response.responseText);

                if (data.VerificaFattureDaAggiungereResult != '') {

                    Ext.MessageBox.show({
                        title: 'Avviso',
                        msg: 'Attenzione!! La fattura n° ' + data.VerificaFattureDaAggiungereResult + ' è già presente su un altro atto',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function(btn) {
                            esiste = true;
                        }
                    });
                } 
            },
            failure: function(response, options) {
                maskFatture.hide();
                var data = Ext.decode(response.responseText);
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: data.FaultMessage,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {}
                });
            }
        });
    }

    Ext.lib.Ajax.defaultPostHeader = stdec
    return esiste;
}


//POPUP
function showPopupPanelRicercaContratti() {
    var popup = new Ext.Window({
        title: 'Ricerca Contratti',
        width: 700,
        height: 700,
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
            text: 'Conferma',
            id: 'btnConfermaSelezione'
            },
            {
                text: 'Chiudi',
                id: 'btnChiudiSelezione'
            }]
    });

    var panelRicercaContratti = buildPanelRicercaContratti();
    
    popup.add(panelRicercaContratti);
    
    popup.doLayout();
    popup.show();
    Ext.getCmp('risultatiRicercaFatture').hide();
    Ext.getCmp('btnConfermaSelezione').disable();


    Ext.getCmp('btnConfermaSelezione').on('click', function() {

        enableDatiContratto(true);
        var fatture = Ext.getCmp('GridRisultatiRicercaFatture').getSelectionModel().getSelections();
        //        fatturaEsistente = verificaEsistenzaFatture();


        var esiste = false;
        var stdec = Ext.lib.Ajax.defaultPostHeader;
        Ext.lib.Ajax.defaultPostHeader = 'application/json';

        var maskFatture = new Ext.LoadMask(Ext.getBody(), {
            msg: "Verifica fatture da aggiungere..."

        });
        var fattureDaAssociare = getFattureDaAssociare();
        if (fattureDaAssociare != undefined) {
            maskFatture.show();

            var params = { fattureDaAggiungere: fattureDaAssociare };
            
            Ext.Ajax.timeout = 100000000;

            Ext.MessageBox.wait('Loading ...');
            var box = Ext.Ajax.request({
                async: false,
                url: 'ProcAmm.svc/VerificaFattureDaAggiungere',
                params: Ext.encode(params),
                method: 'POST',
                success: function(response, options) {
                    maskFatture.hide();

                    var data = Ext.decode(response.responseText);

                    if (data.VerificaFattureDaAggiungereResult != '') {

                        Ext.MessageBox.show({
                            title: 'Avviso',
                            msg: 'Attenzione!! La fattura n° ' + data.VerificaFattureDaAggiungereResult + ' è già presente su un altro atto',
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.INFO,
                            fn: function(btn) {
                                esiste = true;
                            }
                        });
                    } else {
                        var contratti = Ext.getCmp('GridRisultatiRicercaContratti').getSelectionModel().getSelections();
                        if (contratti != undefined && contratti != null) {
                            var addStatus = ADD_LIST_RESULT.ok;
                            var contrattiEsistenti = "";
                            var contrattiSeparator = "'";
                            var fatturaEsistente = false;
                            for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < contratti.length; i++) {

                                var contratto = contratti[i].data;

                                if (contratto != undefined && contratto != null) {
                                    var currentAddStatus = addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id, contratto.CodiceCIG, contratto.CodiceCUP);

                                    var grigliaAttributi = Ext.getCmp('GridAttributi');
                                    if (grigliaAttributi != undefined) {
                                        grigliaAttributi.getView().refresh();
                                    } else {
                                        grigliaAttributi = buildGridAttributi(contratto.CodiceCIG, contratto.CodiceCUP);
                                        Ext.getCmp('tab_attributi').add(grigliaAttributi);
                                        Ext.getCmp('tab_attributi').doLayout();
                                        var griglia = Ext.getCmp('GridAttributi');
                                        grigliaAttributi.getView().refresh();
                                    }
                                   
                                    if (currentAddStatus == ADD_LIST_RESULT.error)
                                        addStatus = currentAddStatus;
                                    else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
                                        addStatus = currentAddStatus;

                                    if (currentAddStatus == ADD_LIST_RESULT.exists) {
                                        contrattiEsistenti = contrattiEsistenti + contrattiSeparator + contratto.NumeroRepertorio + "'";
                                        contrattiSeparator = ", '";
                                    }

                                    if (fatture != undefined && fatture != null) {
                                        var addStatus = ADD_LIST_RESULT.ok;
                                        var fattureEsistenti = "";
                                        var fattureSeparator = "'";
                                        for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < fatture.length; i++) {

                                            var fattura = fatture[i].data;

                                            if (fattura != undefined && fattura != null) {
                                                var idDocumento = Ext.getDom('codDocumento').value;
                                                if (!(idDocumento == null || idDocumento == undefined || idDocumento.trim() == ''))
                                                    var idDoc = idDocumento
                                                else
                                                    var idDoc = ""

                                                var currentAddStatus = addFattura(true, fattura.IdUnivoco, idDoc, fattura.Contratto, fattura.NumeroFatturaBeneficiario, fattura.AnagraficaInfo, fattura.DataFatturaBeneficiario, fattura.ImportoTotaleFattura, fattura.DescrizioneFattura, fattura.ListaAllegati);
                                                if (currentAddStatus == ADD_LIST_RESULT.error)
                                                    addStatus = currentAddStatus;
                                                else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
                                                    addStatus = currentAddStatus;

                                                if (currentAddStatus == ADD_LIST_RESULT.exists) {
                                                    fattureEsistenti = fattureEsistenti + fattureSeparator + fattura.NumeroFatturaBeneficiario + "'";
                                                    fattureSeparator = ", '";
                                                }
                                            }
                                        }

                                        var msg = 'Impossibile aggiungere tutti o parte delle fatture selezionate.';
                                        if (addStatus == ADD_LIST_RESULT.exists)
                                            msg = 'Uno o più fatture selezionate sono già presenti nella lista: ' + fattureEsistenti + '. Sono state aggiunte solo quelle eventualmente non presenti.';
                                        else if (addStatus == ADD_LIST_RESULT.ok)
                                            msg = 'Operazione completata con successo.';

                                        popup.close();

                                        Ext.MessageBox.show({
                                            title: 'Aggiungi Contratto con le relative Fatture',
                                            msg: msg,
                                            buttons: Ext.MessageBox.OK,
                                            icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
                                            fn: function(btn) {
                                            }
                                        });
                                    }
                                }

                            } //chiude for
                            var msg = 'Impossibile aggiungere tutti o parte dei contratti selezionati.';
                            if (addStatus == ADD_LIST_RESULT.exists)
                                msg = 'Uno o più contratti selezionati sono già presenti nella lista: ' + contrattiEsistenti + '. Sono stati aggiunti solo quelli eventualmente non presenti.';
                            else if (addStatus == ADD_LIST_RESULT.ok)
                                msg = 'Operazione completata con successo.';

                            popup.close();

                            Ext.MessageBox.show({
                                title: 'Aggiungi Elementi Selezionati',
                                msg: msg,
                                buttons: Ext.MessageBox.OK,
                                icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
                                fn: function(btn) {
                                }
                            });
                        } //chiude if
                    }
                },
                failure: function(response, options) {
                    maskFatture.hide();
                    var data = Ext.decode(response.responseText);
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: data.FaultMessage,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) { }
                    });
                }
            });
        }

        Ext.lib.Ajax.defaultPostHeader = stdec




        //        if (fatturaEsistente == false) {
        //            var contratti = Ext.getCmp('GridRisultatiRicercaContratti').getSelectionModel().getSelections();
        //            if (contratti != undefined && contratti != null) {
        //                var addStatus = ADD_LIST_RESULT.ok;
        //                var contrattiEsistenti = "";
        //                var contrattiSeparator = "'";
        //                var fatturaEsistente = false;
        //                for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < contratti.length; i++) {

        //                    var contratto = contratti[i].data;

        //                    if (contratto != undefined && contratto != null) {
        //                        var currentAddStatus = addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id);
        //                        if (currentAddStatus == ADD_LIST_RESULT.error)
        //                            addStatus = currentAddStatus;
        //                        else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
        //                            addStatus = currentAddStatus;

        //                        if (currentAddStatus == ADD_LIST_RESULT.exists) {
        //                            contrattiEsistenti = contrattiEsistenti + contrattiSeparator + contratto.NumeroRepertorio + "'";
        //                            contrattiSeparator = ", '";
        //                        }
        //                        
        //                        if (fatture != undefined && fatture != null) {
        //                            var addStatus = ADD_LIST_RESULT.ok;
        //                            var fattureEsistenti = "";
        //                            var fattureSeparator = "'";
        //                            for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < fatture.length; i++) {

        //                                var fattura = fatture[i].data;

        //                                if (fattura != undefined && fattura != null) {
        //                                    var idDocumento = Ext.getDom('codDocumento').value;
        //                                    if (!(idDocumento == null || idDocumento == undefined || idDocumento.trim() == ''))
        //                                        var idDoc = idDocumento
        //                                    else
        //                                        var idDoc = ""

        //                                    var currentAddStatus = addFattura(fattura.IdUnivoco, idDoc, fattura.Contratto, fattura.NumeroFatturaBeneficiario, fattura.AnagraficaInfo, fattura.DataFatturaBeneficiario, fattura.ImportoTotaleFattura, fattura.DescrizioneFattura);
        //                                    if (currentAddStatus == ADD_LIST_RESULT.error)
        //                                        addStatus = currentAddStatus;
        //                                    else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
        //                                        addStatus = currentAddStatus;

        //                                    if (currentAddStatus == ADD_LIST_RESULT.exists) {
        //                                        fattureEsistenti = fattureEsistenti + fattureSeparator + fattura.NumeroFatturaBeneficiario + "'";
        //                                        fattureSeparator = ", '";
        //                                    }
        //                                }
        //                            }

        //                            var msg = 'Impossibile aggiungere tutti o parte delle fatture selezionate.';
        //                            if (addStatus == ADD_LIST_RESULT.exists)
        //                                msg = 'Uno o più fatture selezionate sono già presenti nella lista: ' + fattureEsistenti + '. Sono state aggiunte solo quelle eventualmente non presenti.';
        //                            else if (addStatus == ADD_LIST_RESULT.ok)
        //                                msg = 'Operazione completata con successo.';

        //                            popup.close();

        //                            Ext.MessageBox.show({
        //                                title: 'Aggiungi Contratto con le relative Fatture',
        //                                msg: msg,
        //                                buttons: Ext.MessageBox.OK,
        //                                icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
        //                                fn: function(btn) {
        //                                }
        //                            });
        //                        }
        //                    }

        //                } //chiude for
        //                var msg = 'Impossibile aggiungere tutti o parte dei contratti selezionati.';
        //                if (addStatus == ADD_LIST_RESULT.exists)
        //                    msg = 'Uno o più contratti selezionati sono già presenti nella lista: ' + contrattiEsistenti + '. Sono stati aggiunti solo quelli eventualmente non presenti.';
        //                else if (addStatus == ADD_LIST_RESULT.ok)
        //                    msg = 'Operazione completata con successo.';

        //                popup.close();

        //                Ext.MessageBox.show({
        //                    title: 'Aggiungi Elementi Selezionati',
        //                    msg: msg,
        //                    buttons: Ext.MessageBox.OK,
        //                    icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
        //                    fn: function(btn) {
        //                    }
        //                });
        //            }//chiude if

        //        } else {

        //        }

    });
    
    Ext.getCmp('btnChiudiSelezione').on('click', function() {
        popup.close();
    });
}


function buildPanelRicercaContratti(fnOnSelectContratto) {
    var numeroRepertorio = new Ext.form.TextField({
        id: 'numeroRepertorioContrattoSearchField',
        width: 450,
        labelSeparator: '',
        blankText: '',
        emptyText: '',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCercaContratti');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });

    var descrizione = new Ext.form.TextField({
        id: 'descrizioneContrattoSearchField',
        width: 450,
        labelSeparator: '',
        blankText: '',
        emptyText: '',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCercaContratti');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });

    var NUMERO_REPERTORIO = new Ext.form.Radio({
        boxLabel: 'Numero Repertorio',
        id: 'NUMERO_REPERTORIO_RADIO_BTN',
        name: 'CampoRicercaContratto',
        inputValue: 'NUMERO_REPERTORIO_CONTRATTO',
        checked: true
    });

    NUMERO_REPERTORIO.on('check', function() {
        resetContrattoSearchFields();
        if (NUMERO_REPERTORIO.checked) {
            numeroRepertorio.show();
        } else {
            numeroRepertorio.hide();
        }
    });

    var DESCRIZIONE = new Ext.form.Radio({
        boxLabel: 'Oggetto',
        id: 'DESCRIZIONE_RADIO_BTN',
        name: 'CampoRicercaContratto',
        inputValue: 'DESCRIZIONE_CONTRATTO'
    });

    DESCRIZIONE.on('check', function() {
        resetContrattoSearchFields();
        if (DESCRIZIONE.checked) {
            descrizione.show();
        } else {
            descrizione.hide();
        }
    });


    numeroRepertorio.show();
    descrizione.hide();

    var risultatiRicercaContratti = new Ext.grid.GridPanel({
        id: 'risultatiRicercaContratti',
        cm: new Ext.grid.ColumnModel({}),
        sm: new Ext.grid.RowSelectionModel({}),
        store: new Ext.data.SimpleStore({ fields: ['id'], data: ['G'] })
    });

    var risultatiRicercaFatture = new Ext.grid.GridPanel({
        id: 'risultatiRicercaFatture',
        cm: new Ext.grid.ColumnModel({}),
        sm: new Ext.grid.RowSelectionModel({}),
        store: new Ext.data.SimpleStore({ fields: ['id'], data: ['G'] })
    });
    

    risultatiRicercaContratti.addListener({
        'rowclick': {
            fn: function(grid, rowIndex, event) {

            if (fnOnSelectContratto != undefined && fnOnSelectContratto != null)
                 fnOnSelectContratto(grid.getSelectionModel().getSelected());

              Ext.getCmp('idSearchModel').setValue('onSearch');
            },
            scope: this
        }
    });
    
    var actionAggiungiContratto = new Ext.Action({
        text: 'Nuovo Contratto',
        tooltip: 'Aggiungi un nuovo contratto',
        handler: function() {
            var codFiscOperatore = Ext.get('codFiscOperatore').getValue()
            var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue()

            window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO:" + codFiscOperatore + "," + uffPubblicoOperatore, '_blank');
        },
        iconCls: 'add'
    });

    var ricercaContratti = new Ext.Panel({
        id: 'panelRicercaContratti',
        labelAlign: 'top',
        tbar: [actionAggiungiContratto],
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
                columns: 3
            },
            items: [NUMERO_REPERTORIO,
                {
                    xtype: 'label',
                    style: 'padding-right:40px',
                    text: ''
                }, numeroRepertorio,
                DESCRIZIONE,
                {
                    xtype: 'label',
                    style: 'padding-right:40px',
                    text: ''
                }, descrizione
            ],
            buttons: [{
                text: 'Cerca',
                id: 'btnCercaContratti'
            }
        ]
        },
        risultatiRicercaContratti,
        risultatiRicercaFatture,
        
         {
              xtype: 'hidden',
              id: 'idResultContratti',
              value: null
          },
           {
              xtype: 'hidden',
              id: 'idResultFatture',
              value: null
          }, 
          
          {
              xtype: 'hidden',
              id: 'idSearchModel',
              value: 'onSearch'
}]
          });

    Ext.getCmp('btnCercaContratti').on('click', function() {
    
        var numeroRepertorioContratto = Ext.get('numeroRepertorioContrattoSearchField').getValue();
        var oggettoContratto = Ext.get('descrizioneContrattoSearchField').getValue();

        if (Ext.getCmp('GridRisultatiRicercaContratti') != undefined)
            Ext.getCmp('risultatiRicercaContratti').remove(Ext.getCmp('GridRisultatiRicercaContratti'));
       
        if (Ext.getCmp('GridRisultatiRicercaFatture') != undefined) {
            Ext.getCmp('panelRicercaContratti').remove(Ext.getCmp('GridRisultatiRicercaFatture'));
        }

        var gridRisultatiRicercaContratti = buildPanelRisultatiRicercaContratti(numeroRepertorioContratto, oggettoContratto);

        if (gridRisultatiRicercaContratti != null && gridRisultatiRicercaContratti != undefined)
            Ext.getCmp('risultatiRicercaContratti').add(gridRisultatiRicercaContratti);

        Ext.getCmp('risultatiRicercaContratti').show();
        
        Ext.getCmp("panelRicercaContratti").doLayout();
       
        
    });


    Ext.getCmp('risultatiRicercaContratti').hide();
    Ext.getCmp('btnCercaContratti').enable();

    return ricercaContratti;
}

function buildPanelRisultatiRicercaContratti(numeroRepertorioContratto, oggettoContratto) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/InterrogaContratti',
        method: 'POST',
        timeout: 20000000
    });

    var reader = new Ext.data.JsonReader({
        root: 'InterrogaContrattiResult',
        fields: [
	       { name: 'Id' },
           { name: 'NumeroRepertorio' },
           { name: 'Oggetto' },
           { name: 'CodiceCIG' },
           { name: 'CodiceCUP' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroRepertorio", direction: "ASC" }
    });

    store.on({ 'load': {
        fn: function(store, records, options) {
            if (records.length == 0) {
                if (Ext.getCmp('btnConfermaSelezione') != null && Ext.getCmp('btnConfermaSelezione') != undefined)
                    Ext.getCmp('btnConfermaSelezione').disable();
            }
            maskApp.hide();
        },
        scope: this
    }
    });

    maskApp.show();

    var parametri = { numeroRepertorio: numeroRepertorioContratto, descrizione: oggettoContratto };

    try {
        if (parametri.numeroRepertorio == '0') {
            maskApp.hide();
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Non è possibile inserire sull\'atto le fatture del contratto con numero repertorio ZERO. E\' necessario correggere l\'associazione contratto-fatture dal "Modulo contratti/convenzioni/altro" della Intranet.',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {return;}
            });
        } else {
            store.load({ params: parametri });
        }
        
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: true,
        loadMask: false,
        listeners: {

            rowselect: function(selectionModel, rowIndex, record) {

                var totalRows = Ext.getCmp('GridRisultatiRicercaContratti').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 1) {

                    if (Ext.getCmp('GridRisultatiRicercaFatture') != undefined) {
                        Ext.getCmp('panelRicercaContratti').remove(Ext.getCmp('GridRisultatiRicercaFatture'));
                    }
                    var grigliafatture = buildPanelRisultatiRicercaFatture(record.data.Id, record.data.NumeroRepertorio,record.data.Oggetto);

                    Ext.getCmp("risultatiRicercaFatture").add(grigliafatture);
                    Ext.getCmp('risultatiRicercaFatture').show();

                    Ext.getCmp('btnConfermaSelezione').enable();
                    
                }
//                if (totalRows == selectedRows.length) {
//                    if (Ext.getCmp('GridRisultatiRicercaFatture') != undefined)
//                        Ext.getCmp('risultatiRicercaFatture').remove(Ext.getCmp('GridRisultatiRicercaFatture'));

//                    if (Ext.getCmp('GridRisultatiRicercaFatture') != undefined) {
//                        var view = Ext.getCmp('GridRisultatiRicercaFatture').getView();
//                        var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker")
//                        chkdiv.addClass("x-grid3-hd-checker-on");
//                    }

//                }
                Ext.getCmp("panelRicercaContratti").doLayout();
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    Ext.getCmp('risultatiRicercaFatture').hide();
                    Ext.getCmp("panelRicercaContratti").doLayout();
                }
                             
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            { header: "Numero Repertorio", width: 90, dataIndex: 'NumeroRepertorio', sortable: true, locked: true },
            { header: "Oggetto", width: 320, dataIndex: 'Oggetto', sortable: true, locked: false },
            { header: "CIG", width: 80, dataIndex: 'CodiceCIG', sortable: true, locked: false },
            { header: "CUP", width: 100, dataIndex: 'CodiceCUP', sortable: true, locked: false },
         	{ header: "Id", dataIndex: 'Id', id: 'Id', sortable: false, hidden: true }
         	]);

    var grid = new Ext.grid.GridPanel({
        id: 'GridRisultatiRicercaContratti',
        title: 'Contratti Trovati',
        autoHeight: false,
        height: 180,
        border: false,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessun contratto corrisponde ai criteri di ricerca inseriti.",
            deferEmptyText: false
        },
        sm: sm
    });

    return grid;
}

function resetContrattoSearchFields() {
    Ext.getCmp('numeroRepertorioContrattoSearchField').setValue('');
    Ext.getCmp('descrizioneContrattoSearchField').setValue('');
}

function buildPanelFatture() {
    var panelFattura = new Ext.Panel({
        id: 'panelFattura',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        style: 'margin-top:15px;',
        
        layoutConfig: {
            columns: 1
        },
        items: [
        
            buildPanelGridFatture(),
            {
                xtype: 'hidden',
                id: 'listaFatture'
            }
         ]
    });

    return panelFattura;
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

function resetDatiContrattiFields() {
    if (Ext.getCmp('listaContratti') != undefined)
        Ext.getCmp('listaContratti').reset();
}

function buildPanelGridFatture() {

    var aggiungiFattura = new Ext.Action({
        text: 'Aggiungi Fattura/e',
        id: 'actionAggiungiFattura',
        tooltip: 'Seleziona e aggiunge uno più fatture alla lista',
        handler: function() {
            showPopupPanelRicercaContratti();
        }
    });

    var rimuoviFattura = new Ext.Action({
        text: 'Rimuovi Fattura/e',
        id: 'actionRimuoviFattura',
        tooltip: 'Rimuove una o più fatture dalla lista',
        handler: function() {
            var gridFatture = Ext.getCmp('GridFatture');
            var storeGridFatture = gridFatture.getStore();
            var selections = gridFatture.getSelectionModel().getSelections();

            Ext.MessageBox.buttonText.cancel = "Annulla";

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'della fattura selezionata' : 'delle fatture selezionate') + '?',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'ok') {
                        removeFattureFn(selections);
                    }
                }
            });
        }
    });

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridFatture').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviFattura.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridFatture').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviFattura.disable();
                }
                var view = Ext.getCmp('GridFatture').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Contratto", width: 60, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
            renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.Contratto!= undefined && record.data.Contratto != null) {

                        if (!isNullOrEmpty(record.data.Contratto.NumeroRepertorio)) {
                            retValue = record.data.Contratto.NumeroRepertorio;
                        }
                      
                    }
                    return retValue;
                }
         },
         { header: "N° Fatt.", width: 60, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true},
         { header: "Data", width: 65, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },
        
         { header: "Beneficiario", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo != undefined && record.data.AnagraficaInfo != null) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Nome) && !isNullOrEmpty(record.data.AnagraficaInfo.Cognome)) {
                            retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome;
                        }
                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Denominazione)) {
                            retValue = record.data.AnagraficaInfo.Denominazione;
                        }
                    }
                    return retValue;
                }
            },

            { header: "Sede", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0 ) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede;
                        }
                    }
                    return retValue;
                }
            },

            { header: "Metodo Pagamento", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento)) {

                            retValue = record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento
                        }
                    }
                    return retValue;
                }
            },

             {
                 header: "IdConto", width: 105, dataIndex: 'AnagraficaInfo', sortable: true, locked: false, hidden: true,
                 renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                     var retValue = '';
                     if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                         if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                             if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente)) {
                                 retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente;
                             }
                         }
                     }
                     return retValue;
                 }
             },

            { header: "Iban", width: 105, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban;
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Descrizione", width: 100, dataIndex: 'DescrizioneFattura', sortable: true, locked: false, hidden: true },
            { header: "Importo", width: 70, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "IdDocumento", width: 100, dataIndex: 'IdDocumento', hidden: true },
            { header: "Prog", width: 100, dataIndex: 'Prog', hidden: true }
            
        ]);
        
        

    var store = new Ext.data.SimpleStore({
    fields: ['NumeroRepertorio', 'IdDocumento', 'NumeroFatturaBeneficiario', 'DataFatturaBeneficiario', 'AnagraficaInfo', 'DescrizioneFattura', 'ImportoTotaleFattura', 'IdUnivoco', 'ListaAllegati', 'Prog']
    });

    var grid = new Ext.grid.GridPanel({
        id: 'GridFatture',
        autoHeight: false,
        height: 150,
        width: 670,
        border: true,
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessuna fattura associata.",
            deferEmptyText: false
        },
        sm: sm
    });

    grid.on('render', function() {
        this.getView().mainBody.on('mousedown', function(e, t) {
            if (t.tagName == 'A') {
                e.stopEvent();
                t.click();
            }
        });
    }, grid);

    var gridFatture = new Ext.Panel({
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        items: [grid,
                {
                    xtype: 'panel',
                    plain: true,
                    border: false,
                    layout: 'table',
                    style: 'margin-left:420px;margin-top:4px',
                    layoutConfig: {
                        columns: 3
                    },
                    items: [
                    //                    new Ext.LinkButton(aggiungiFattura),
                            {xtype: 'label', style: 'margin-left:10px' },
                            { xtype: 'label', style: 'margin-left:10px' },
                            new Ext.LinkButton(rimuoviFattura)
                    ]
}]
    }
    );

    rimuoviFattura.disable();

    return gridFatture;
}



function buildPanelRisultatiRicercaFatture(idContratto,numeroRepertorio,oggetto) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetElencoFattureByContratto',
        method: 'POST',
        timeout: 20000000
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetElencoFattureByContrattoResult',
        fields: [
	       { name: 'IdUnivoco' },
	       { name: 'Contratto' },
           { name: 'NumeroFatturaBeneficiario' },
           { name: 'DataFatturaBeneficiario' },
           { name: 'DescrizioneFattura' },
           { name: 'ImportoTotaleFattura' },
           { name: 'AnagraficaInfo' },
           { name: 'ListaAllegati' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" },
        listeners: {
            'loadexception': function(proxy, options, response) {
                maskApp.hide();
                Ext.MessageBox.show({
                    title: 'Ricerca Fatture',
                    msg: "Errore durante la ricerca delle fatture:<br>" +
                         "'" + Ext.decode(response.responseText).FaultMessage + "'",
                    buttons: Ext.Msg.OK,
                    closable: false,
                    icon: Ext.MessageBox.ERROR
                });
            }
        }
    });

    store.on({ 'load': {
    fn: function(store, records, options) {
            var grigliafatture = Ext.getCmp('GridRisultatiRicercaFatture');
            Ext.getCmp("risultatiRicercaFatture").add(grigliafatture);
            Ext.getCmp('risultatiRicercaFatture').show();
            maskApp.hide();
        },
        scope: this
    }
    });

    maskApp.show();

    var parametri = { contratto: idContratto, numeroRepertorio: numeroRepertorio, oggetto:oggetto };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var smFatture = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: false,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridRisultatiRicercaFatture').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {

                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
            smFatture,

            { header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true },
            { header: "Data Fattura", width: 80, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },
            { header: "Beneficiario", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo != undefined && record.data.AnagraficaInfo != null) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Nome) && !isNullOrEmpty(record.data.AnagraficaInfo.Cognome)) {
                            retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome;
                        }
                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Denominazione)) {
                            retValue = record.data.AnagraficaInfo.Denominazione;
                        }
                    }
                    return retValue;
                }
            },

            { header: "Sede", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0 ) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede;
                        }
                    }
                    return retValue;
                }
            },

            { header: "Metodo Pagamento", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento)) {

                            retValue = record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento;
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "IdConto", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false, hidden: true,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente;
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Iban", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban;
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Descrizione", width: 100, dataIndex: 'DescrizioneFattura', sortable: true, locked: false, hidden: true },
            { header: "Importo Totale", width: 100, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true }
         	]);

    var grid = new Ext.grid.GridPanel({
        id: 'GridRisultatiRicercaFatture',
        title: 'Fatture Contratto',
        autoHeight: false,
        height: 220,
        border: false,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessuna fattura corrisponde ai criteri di ricerca inseriti.",
            deferEmptyText: false
        },
        sm: smFatture
    });

    return grid;
}


function addContratto(numeroRepertorio, oggetto, id, codiceCIG, codiceCUP) {
    var retValue = ADD_LIST_RESULT.error;

    var gridContratti = Ext.getCmp('GridContratti');
    if (gridContratti != undefined && gridContratti != null) {
        var gridContrattiStore = gridContratti.store;
        if (gridContrattiStore != undefined && gridContrattiStore != null) {
            if (!isNullOrEmpty(id)) {
                var recordAlreadyExists = gridContrattiStore.queryBy(
                                function(record, recordId) {
                                    return (record.data.Id == id)
                                });

                if (!(recordAlreadyExists != undefined && recordAlreadyExists.length > 0)) {
                    ContrattoRecordType = Ext.data.Record.create(['Id', 'NumeroRepertorio', 'Oggetto', 'CodiceCIG', 'CodiceCUP']);
                    var newRecordContratto = new ContrattoRecordType({
                        Id: id
                        , NumeroRepertorio: numeroRepertorio
                        , Oggetto: oggetto
                        , CodiceCIG: codiceCIG
                        , CodiceCUP: codiceCUP
                    });

                    gridContrattiStore.add(newRecordContratto);
                    retValue = ADD_LIST_RESULT.ok;
                }
                else
                    retValue = ADD_LIST_RESULT.exists;
            }
        }
    }

    return retValue;
}

    function isNullOrEmpty(value) {
        return (value == null || value == undefined || value == "")
    }

    function addFattura(checkIfAlreadyExists, idUnivoco, idDoc, contratto, numeroFatturaBeneficiario, anagraficaInfo, dataFatturaBeneficiario, importoTotaleFattura, descrizioneFattura, listaAllegati, prog) {
        var retValue = ADD_LIST_RESULT.error;

        var gridFatture = Ext.getCmp('GridFatture');
        if (gridFatture != undefined && gridFatture != null) {
            var gridFattureStore = gridFatture.store;
            if (gridFattureStore != undefined && gridFattureStore != null) {
                if (!isNullOrEmpty(numeroFatturaBeneficiario)) {

                    var recordAlreadyExists = false;
                    if (checkIfAlreadyExists) {
                       recordAlreadyExists =  gridFattureStore.queryBy(
                                function(record, recordId) {
                                    return (record.data.IdUnivoco == idUnivoco);
                                });
                    }
                    

                    if (!(recordAlreadyExists != undefined && recordAlreadyExists.length > 0)) {
                        FatturaRecordType = Ext.data.Record.create(['IdUnivoco', 'IdDocumento', 'Contratto', 'NumeroFatturaBeneficiario', 'DataFatturaBeneficiario', 'AnagraficaInfo', 'ImportoTotaleFattura', 'DescrizioneFattura', 'ListaAllegati', 'Prog']);
                        
                        var newRecordFattura = new FatturaRecordType({
                            Contratto: contratto,
                            IdDocumento: idDoc,
                            NumeroFatturaBeneficiario: numeroFatturaBeneficiario,
                            DataFatturaBeneficiario: dataFatturaBeneficiario,
                            AnagraficaInfo: anagraficaInfo,
                            ImportoTotaleFattura: importoTotaleFattura,
                            DescrizioneFattura: descrizioneFattura,
                            IdUnivoco: idUnivoco,
                            ListaAllegati: listaAllegati,
                            Prog: prog
                        });

                        gridFattureStore.add(newRecordFattura);
                        retValue = ADD_LIST_RESULT.ok;
                    }
                    else
                        retValue = ADD_LIST_RESULT.exists;
                }
            }
        }

        return retValue;
    }

    function refreshPanelGridContrattiFatture() {
        var gridContratti = Ext.getCmp('GridContratti');
        gridContratti.getView().refresh();
        var gridFatture = Ext.getCmp('GridFatture');
        gridFatture.getView().refresh();

    }

    function setDatiSchedaContrattiFattureOnLoad(schedaContrattiFattureInfo) {
        if (schedaContrattiFattureInfo != null) {
            //init contratti grid
              enableDatiContratto(true);
              if (schedaContrattiFattureInfo.Contratti != undefined && schedaContrattiFattureInfo.Contratti != null) {
                    var contratti = schedaContrattiFattureInfo.Contratti;

                    for (var i = 0; i < contratti.length; i++) {
                         var contratto = contratti[i];
                          if (contratto != undefined && contratto != null)
                              addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id, contratto.CodiceCIG, contratto.CodiceCUP);
                    }
                }

                //init fatture grid
                enableDatiFattura(true);
                if (schedaContrattiFattureInfo.Fatture != undefined && schedaContrattiFattureInfo.Fatture != null) {
                    var fatture = schedaContrattiFattureInfo.Fatture;

                    for (var i = 0; i < fatture.length; i++) {
                        var fattura = fatture[i];
                        if (fattura != undefined && fattura != null)
                            var idDocumento = Ext.getDom('codDocumento').value;
                        if (!(idDocumento == null || idDocumento == undefined || idDocumento.trim() == '')) {
                            var idDoc = idDocumento;
                            addFattura(false, fattura.IdUnivoco, idDoc, fattura.Contratto, fattura.NumeroFatturaBeneficiario, fattura.AnagraficaInfo, fattura.DataFatturaBeneficiario, fattura.ImportoTotaleFattura, fattura.DescrizioneFattura, fattura.ListaAllegati, fattura.Prog);
                        }
                        else {
                            var idDoc = "";
                        }
                    }
                }
        }
    }
