var ADD_LIST_RESULT = { "ok": 0, "exists": 1,
    "error": 2
};

var TIPO_DOCUMENTO = { "delibera": 2, "determina": 0,
    "disposizione": 1
};

function validateSchedaTipologiaProvvedimentoFields(panelSchedaId) {
    var retValue = null;

    if (!Ext.getCmp(panelSchedaId).disabled) {
        var isValid = false;
        var errorMsg = '';

        if ((Ext.getCmp('comboTipologieProvvedimento') == undefined || !Ext.getCmp('comboTipologieProvvedimento').validate())) 
            errorMsg += "E' necessario selezionare una tipologia di provvedimento.";
        else {      
            var selectedTipologia = getSelectedTipologiaProvvedimento();
            if (selectedTipologia != null && selectedTipologia != undefined) {
                if (selectedTipologia.HasDestinatari && selectedTipologia.HasDestinatariObbligatori) {
                    var gridDestinatari = Ext.getCmp('GridDestinatari');

                    if (gridDestinatari != undefined && gridDestinatari != null) {
                        var storeGridDestinatari = gridDestinatari.getStore();

                        if (storeGridDestinatari != undefined && storeGridDestinatari != null) {
                            if (storeGridDestinatari.data.length == 0)
                                errorMsg += "E' necessario indicare almeno un destinatario del provvedimento.";
                            else
                                isValid = true
                        }
                        else
                            errorMsg += "Impossibile ottenere l'elenco dei destinatari del provvedimento.";
                    }
                    else
                        errorMsg += "Impossibile ottenere l'elenco dei destinatari del provvedimento.";
                } 
                else 
                    isValid = true
            }
            else
                errorMsg += "Impossibile ottenere l'elenco dei destinatari del provvedimento.";
        }
        
        if (!isValid) {
            errorMsg = "Scheda 'Tipologia Provvedimento' incompleta.<br/>" + errorMsg;
            retValue = { tab_to_activate: panelSchedaId, msg: errorMsg };
        }
    }

    return retValue;
}

function setDatiSchedaTipologiaProvvedimentoOnLoad(schedaTipologiaProvvedimentoInfo) {
    if (schedaTipologiaProvvedimentoInfo != null) {    
    
        if (!schedaTipologiaProvvedimentoInfo.isSommaAutomatica) {
                Ext.getCmp('noSommaAutomatica').checked = true;
                Ext.getCmp('yesSommaAutomatica').checked = false;
//                //Ext.getCmp('noteAutorizzazionePubblicazione').setValue(schedaLeggeTrasparenzaInfo.NotePubblicazione);
            } else {
                Ext.getCmp('noSommaAutomatica').checked = false;
                Ext.getCmp('yesSommaAutomatica').checked = true;
       }
        //init importo spesa prevista
        if(schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista!=null && 
            schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista!=undefined)            
            Ext.getCmp('importoSpesaPrevista').setValue(schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista);
        // moooood Ext.getCmp('importoSpesaPrevista').setValue(schedaTipologiaProvvedimentoInfo.ImportoSpesaPrevista);
        
        //init destinatari grid
        enableDatiDestinatario(true);
        
        if (schedaTipologiaProvvedimentoInfo.Destinatari != undefined && schedaTipologiaProvvedimentoInfo.Destinatari != null) {
            var destinatari = schedaTipologiaProvvedimentoInfo.Destinatari;

            for (var i = 0; i < destinatari.length; i++) {
                var destinatario = destinatari[i];
                if (destinatario != undefined && destinatario != null)
                    addDestinatario(destinatario.IdSIC, destinatario.isPersonaFisica, destinatario.Denominazione,
                                    destinatario.CodiceFiscale, destinatario.PartitaIva, destinatario.LuogoNascita, destinatario.DataNascita,
                                    destinatario.legaleRappresentante, destinatario.IdContratto, destinatario.NumeroRepertorioContratto, destinatario.isDatoSensibile, destinatario.ImportoSpettante);
            }
        }
    }         
}

function resetSchedaTipologiaProvvedimentoFields() {
    resetDatiDestinatariFields();
}

function resetDatiDestinatariFields() {
    if (Ext.getCmp('listaDestinatari') != undefined)
        Ext.getCmp('listaDestinatari').reset();
}


function buildSchedaTipologiaProvvedimentoPanel(schedaTipologiaProvvedimentoInfo) {
    
    
    var panelSchedaTipologiaProvvedimento = new Ext.Panel({
        xtype: "panel",
        id: 'panelSchedaTipologiaProvvedimento',
        layout: 'table',
        border: false,
        autoScroll: true,
        layoutConfig: {
            columns: 1
        },
        style: {
            "margin-top": "5px",
            "margin-bottom": "20px",
            "margin-left": "10px"
        },
        items: [
            {
                xtype: 'label',
                html: "<div style='width:720px;padding-right:25px;text-align:justify;'>" + 
                "L'indicazione della tipologia del provvedimento è necessaria per ottemperare alla pubblicazione dell’atto " +
                "nelle diverse sezioni del portale istituzionale così come previsto dal D.Lgs 33/2013.</div>",
                style: 'margin-right:50px'
            }                        
//           ,{
//                xtype: 'linkbutton', id: 'ulterioriInformazioniId', text: "Ulteriori informazioni", style: 'margin-top: 10px',
//                tooltip: "Ulteriori informazioni sulla normativa alla base della scheda 'Tipologia Provvedimento'",
//                listeners: {
//                    click: function() {
//                        showInfoSchedaTipologiaProvvedimento();
//                    }
//                }
//            }
        ]
    });
    
    //la domanda sull'interfaccia "Vuoi disattivare la somma automatica?" è posta al contrario rispetto ai valori gestiti.
    // Se si risponde SI, la somma automatica sarà disattiva, quindi il valore 
    // del flag sarà NO(n) calcolare somma automatica e viceversa.
    var yesOptionSommaAutomatica = new Ext.form.Radio({
        boxLabel: 'No',
        id: 'yesSommaAutomatica',
        name: 'selezioneSommaAutomatica',
        inputValue: 'SI',
        checked: true
    });

    var noOptionSommaAutomatica = new Ext.form.Radio({
        boxLabel: 'Si',
        id: 'noSommaAutomatica',
        name: 'selezioneSommaAutomatica',
        inputValue: 'NO',
        checked: false
    });
    
    yesOptionSommaAutomatica.on('check', function() {
        if (Ext.getCmp('yesSommaAutomatica').checked) {
            
            Ext.getCmp('importoSpesaPrevista').getEl().dom.setAttribute('readOnly', true);
            Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "gray";
            var gridDestinatari = Ext.getCmp('GridDestinatari');
            var storeGridDestinatari = gridDestinatari.getStore();

            if (storeGridDestinatari != undefined && storeGridDestinatari != null) {
                var totImporto = 0;

                storeGridDestinatari.each(function(storeGridDestinatari) {
                    totImporto = totImporto + parseFloat(String(storeGridDestinatari.data.ImportoSpettante).replace(",","."));
                });

                Ext.getCmp('importoSpesaPrevista').setValue(totImporto);
                
            }
        } else {
           Ext.getCmp('importoSpesaPrevista').getEl().dom.removeAttribute('readOnly');
           Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "black";
        }
    });

    noOptionSommaAutomatica.on('check', function() {
        if (Ext.getCmp('noSommaAutomatica').checked) {
            alert('Il calcolo automatico del totale della spesa prevista dei signoli destinatari sarà disattivato. Pertanto, sarà possibile indicare un valore differente.');
            Ext.getCmp('importoSpesaPrevista').getEl().dom.removeAttribute('readOnly');
            Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "black";
        } else {
            Ext.getCmp('importoSpesaPrevista').getEl().dom.setAttribute('readOnly', true);
            Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "gray";
        }
    });

    var panelDataSchedaTipologiaProvvedimento = new Ext.Panel({
        xtype: "panel",
        title: "",
        id: 'panelDataSchedaTipologiaProvvedimento',
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
        items: [
         {
             xtype: 'label',
             id: 'tipologiaProvvedimentoLabel',
             text: 'Tipologia Provvedimento (*)'                    
         },
            buildPanelTipologiaProvvedimento(
                function(store) {
                    //init tipologia provvedimento
                    if (schedaTipologiaProvvedimentoInfo != null && schedaTipologiaProvvedimentoInfo != undefined) {
                        enableTipologiaProvvedimento(true);
                        if (schedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento > -1) {
                            var comboTipologieProvvedimento = Ext.getCmp('comboTipologieProvvedimento');
                            comboTipologieProvvedimento.setValue(schedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento);

                            var tipologiaProvvedimentoSelezionata = comboTipologieProvvedimento.findRecord(comboTipologieProvvedimento.valueField || comboTipologieProvvedimento.displayField, schedaTipologiaProvvedimentoInfo.IdTipologiaProvvedimento);
                            comboTipologieProvvedimento.fireEvent('select', comboTipologieProvvedimento, tipologiaProvvedimentoSelezionata, comboTipologieProvvedimento.store.indexOf(tipologiaProvvedimentoSelezionata));
                        }
                    }
                },
                function(value) {
                    if (value.data.HasDestinatari) {
                        Ext.getCmp('destinatariLabel').setText(value.data.HasDestinatariObbligatori ? 'Destinatari (*)' : 'Destinatari');
                        Ext.getCmp('actionAggiungiDestinatario').enable();
                        Ext.getCmp('actionImportaDestinatari').enable();
                    }
                    else
                        Ext.getCmp('destinatariLabel').setText('Destinatari');

                    enableDatiDestinatario(value.data.HasDestinatari);
                    showDatiDestinatario(value.data.HasDestinatari);
                }
            ),
         {
             xtype: 'label',
             id: 'destinatariLabel',
             text: 'Destinatari (*)'
         },
            buildPanelDestinatari(), 
         {
            xtype: 'label',
            text: 'Vuoi disattivare la somma automatica?',
            id: 'sommaAutomaticaLabel'            
        },
        {
            xtype: 'panel',            
            border: false,
            layout: 'table',
            layoutConfig: {
                columns: 3
            },
            items: [yesOptionSommaAutomatica,
                {
                    xtype: 'label',
                    style: 'padding-right:10px',
                    text: ''
                },
                noOptionSommaAutomatica]
        }
        ,{ 
            xtype: 'label',
            text: ''            
        }
        ,{ 
            xtype: "panel",
            layout: 'table',
            border: false,
            layoutConfig: {
                columns: 1
            },
            items: [{
                xtype: 'label',
                html: "<div style='width:720px;padding-right:25px;text-align:justify;'>" + 
                "</div>",
                style: 'margin-right:50px'
            }]   
        }
        ,{
            xtype: 'label',
            text: 'Totale Spesa Prevista da pubblicare',
            id: 'importoSpesaPrevistaLabel'
        }, {
            xtype: 'numberfield',
            name: 'importoSpesaPrevista',
            id: 'importoSpesaPrevista',
            decimalSeparator: ',',
            allowNegative: false,
            width: 200,
            allowBlank: true,
            value: 0
        }
         ]
    });

    var panelContenitore = new Ext.Panel({
        xtype: "panel",
        id: 'panelContenitoreSchedaTipologiaProvvedimento',
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        width: 765,
        border: false,
        items: [
                panelSchedaTipologiaProvvedimento,
                panelDataSchedaTipologiaProvvedimento,
                 {
                     xtype: 'label',
                     html: "<div style='margin-left:10px;margin-top:15px'><i>I campi contrassegnati con (*) sono obbligatori.</i></div>"
                 }
            ]
    });

    showDatiDestinatario(true);
    enableDatiDestinatario(true);
    enableTipologiaProvvedimento(true);
    showTipologiaProvvedimento(true);
    
    if (schedaTipologiaProvvedimentoInfo != null && schedaTipologiaProvvedimentoInfo != undefined)
        setDatiSchedaTipologiaProvvedimentoOnLoad(schedaTipologiaProvvedimentoInfo);
    else {
        Ext.getCmp('actionAggiungiDestinatario').disable();
        Ext.getCmp('actionImportaDestinatari').disable();
    }

    return panelContenitore;
}

function buildPanelDestinatari(leggeTrasparenzaInfo) {
    var panelDestinatari = new Ext.Panel({
        id: 'panelDestinatari',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        style: 'margin-top:15px',
        layoutConfig: {
            columns: 1
        },
        items: [         
            buildPanelGridDestinatari()
            ,{
                xtype: 'hidden',
                id: 'listaDestinatari'
            }
     ]
    });

    return panelDestinatari;
}

function enableFieldsSchedaTipologiaProvvedimento(enable) {
    enableDatiDestinatario(enable);
    enableTipologiaProvvedimento(enable);
}


function enableTipologiaProvvedimento(enable) {
    if (enable) {
        Ext.getCmp('comboTipologieProvvedimento').enable();
        Ext.getCmp('importoSpesaPrevista').enable();
    } else {
        Ext.getCmp('comboTipologieProvvedimento').disable();
        Ext.getCmp('importoSpesaPrevista').disable();
    }
}

function showTipologiaProvvedimento(show) {
    if (show) {
        Ext.getCmp('tipologiaProvvedimentoLabel').show();
        Ext.getCmp('panelTipologiaProvvedimento').show();
        
        Ext.getCmp('importoSpesaPrevistaLabel').show();
        Ext.getCmp('importoSpesaPrevista').show();
    } else {
        Ext.getCmp('tipologiaProvvedimentoLabel').hide();
        Ext.getCmp('panelTipologiaProvvedimento').hide();
        
        Ext.getCmp('importoSpesaPrevistaLabel').hide();
        Ext.getCmp('importoSpesaPrevista').hide();
    }
}

function enableDatiDestinatario(enable) {
    if (enable) {
        Ext.getCmp('listaDestinatari').enable();
        Ext.getCmp('sommaAutomaticaLabel').enable();
        Ext.getCmp('yesSommaAutomatica').enable();
        Ext.getCmp('noSommaAutomatica').enable();
    } else {
        Ext.getCmp('listaDestinatari').disable();
        Ext.getCmp('sommaAutomaticaLabel').disable();
        Ext.getCmp('yesSommaAutomatica').disable();
        Ext.getCmp('noSommaAutomatica').disable();
    }
}

function showDatiDestinatario(show) {
    if (show) {
        Ext.getCmp('destinatariLabel').show();
        Ext.getCmp('panelDestinatari').show();
        
        Ext.getCmp('sommaAutomaticaLabel').show();
        Ext.getCmp('yesSommaAutomatica').show();
        Ext.getCmp('noSommaAutomatica').show();
        
        if (Ext.getCmp('importoSpesaPrevista').getEl() != undefined) {
            Ext.getCmp('importoSpesaPrevista').getEl().dom.setAttribute('readOnly', true);
            Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "grey";
        }
    } else {
        Ext.getCmp('destinatariLabel').hide();
        Ext.getCmp('panelDestinatari').hide();
        
        Ext.getCmp('sommaAutomaticaLabel').hide();
        Ext.getCmp('yesSommaAutomatica').hide();
        Ext.getCmp('noSommaAutomatica').hide();
        
        if (Ext.getCmp('importoSpesaPrevista').getEl() != undefined) {
            Ext.getCmp('importoSpesaPrevista').getEl().dom.setAttribute('readOnly', false);
            Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "black";
        }
    }
}

function setDestinatariAssociati() {
    var gridDestinatari = Ext.getCmp('GridDestinatari');

    if (gridDestinatari != undefined && gridDestinatari != null) {
        var storeGridDestinatari = gridDestinatari.getStore();

        if (storeGridDestinatari != undefined && storeGridDestinatari != null) {
            var destinatariAsJsonObject = '';

            storeGridDestinatari.each(function(storeGridDestinatari) {
                destinatariAsJsonObject += Ext.util.JSON.encode(storeGridDestinatari.data) + ',';
            });

            destinatariAsJsonObject = destinatariAsJsonObject.substring(0, destinatariAsJsonObject.length - 1);
            Ext.getDom('listaDestinatari').value = destinatariAsJsonObject;
        }
    }
}

function getDestinatariAssociati() {
    var retValue = [];
    var gridDestinatari = Ext.getCmp('GridDestinatari');

    if (gridDestinatari != undefined && gridDestinatari != null) {
        var storeGridDestinatari = gridDestinatari.getStore();

        if (storeGridDestinatari != undefined && storeGridDestinatari != null) {
            storeGridDestinatari.each(function(storeGridDestinatari) {
                retValue.push(storeGridDestinatari);
            });
        }
    }
    return retValue;
}


function refreshSchedaTipologiaProvvedimentoPanel() {
    refreshPanelGridDestinatari();
}

function refreshPanelGridDestinatari() {
    var gridDestinatari = Ext.getCmp('GridDestinatari');
    gridDestinatari.getView().refresh();
    
    if (Ext.getCmp('yesSommaAutomatica').checked) {
        Ext.getCmp('importoSpesaPrevista').getEl().dom.setAttribute('readOnly', true);
        Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "grey";
    } else {
        Ext.getCmp('importoSpesaPrevista').getEl().dom.removeAttribute('readOnly');
        Ext.getCmp('importoSpesaPrevista').getEl().dom.style.color = "black";
    }
   
    
}

function getTipologieProvvedimentoStore(fnOnLoad) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaTipologieDocumento',
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaTipologieDocumentoResult',
        fields: [
           { name: 'Id' },
           { name: 'Tipologia' },
           { name: 'HasDestinatari' },
           { name: 'HasDestinatariObbligatori' }
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

function getSelectedTipologiaProvvedimento() {
    var retValue = undefined;

    var comboTipologieProvvedimento = Ext.getCmp("comboTipologieProvvedimento");
    if (comboTipologieProvvedimento != undefined && comboTipologieProvvedimento != null) {
        var record = comboTipologieProvvedimento.findRecord(comboTipologieProvvedimento.valueField || comboTipologieProvvedimento.displayField, comboTipologieProvvedimento.getValue());
        if (record != undefined && record != null)
            retValue = record.data
    }

    return retValue;
}

function buildPanelTipologiaProvvedimento(fnOnLoadTipologieProvvedimento, fnOnSelectTipologiaProvvedimento) {
    var comboTipologieProvvedimento = new Ext.form.ComboBox({
        id: 'comboTipologieProvvedimento',
        store: getTipologieProvvedimentoStore(fnOnLoadTipologieProvvedimento),
        displayField: 'Tipologia',
        valueField: 'Id',
        submitValue: true,
        hideTrigger: false,
        hiddenName: 'idTipologiaProvvedimento',
        width: Ext.isIE ? 588 : 587,
        listWidth: Ext.isIE ? 588 : 587,
        queryMode: 'local',
        style: 'margin-top:0px',
        readOnly: true,
        allowBlank: false,
        mode: 'local',
        emptyText: 'Selezionare una tipologia...',
        typeAhead: true,
        forceSelection: true,
        triggerAction: 'all',
        selectOnFocus: true,
        listeners: {
            select: {
                fn: function(combo, value) {
                    if (fnOnSelectTipologiaProvvedimento != null && fnOnSelectTipologiaProvvedimento != undefined)
                        fnOnSelectTipologiaProvvedimento(value);
                }
            }
        }
    });

    var panelTipologiaProvvedimento = new Ext.Panel({
        id: 'panelTipologiaProvvedimento',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        width: 604,
        style: (Ext.isIE ? 'margin-top:-1px;' : 'margin-top:0px;'),
        layoutConfig: {
            columns: 1
        },
        items: [comboTipologieProvvedimento]
    });

    return panelTipologiaProvvedimento;
}

function renderPartitaIva(val, p, record) {
    if (val) {
        return record.data.CodiceFiscale;
    } else {
        return record.data.PartitaIva
    }
}

function renderTipologia(val, p, record) {
    return "Persona " + (val ? "Fisica" : "Giuridica");
}

function buildPanelGridDestinatari() {
    var importaDestinatari = new Ext.Action({
        text: 'Importa Destinatari',
        id: 'actionImportaDestinatari',
        tooltip: "Aggiunge uno o più destinatari alla lista importandoli dall'atto (delibera o determina) alla base dell'attribuzione del beneficio specificato nella 'Scheda Trasparenza'",
        handler: function() {
            importDestinatari();
        }
    });

    var aggiungiDestinatario = new Ext.Action({
        text: 'Aggiungi Destinatario/i',
        id: 'actionAggiungiDestinatario',
        tooltip: 'Seleziona e aggiunge uno più destinatari alla lista',
        handler: function() {
            showPopupPanelRicercaDestinatari();
        }
    });

    var rimuoviDestinatario = new Ext.Action({
        text: 'Rimuovi Destinatario/i',
        id: 'actionRimuoviContratto',
        tooltip: 'Rimuove uno o più destinatari dalla lista',
        handler: function() {
            var gridDestinatari = Ext.getCmp('GridDestinatari');
            var storeGridDestinatari = gridDestinatari.getStore();
            var selections = gridDestinatari.getSelectionModel().getSelections();

            Ext.MessageBox.buttonText.cancel = "Annulla";

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'del destinatario selezionato' : 'dei destinatari selezionati') + '?',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'ok') {
                        Ext.each(selections, function(rec) {
                            storeGridDestinatari.remove(rec);
                            if (Ext.getCmp('yesSommaAutomatica').checked) {
                                removeToImportoTotaleSpettante(rec.data.ImportoSpettante);
                            }
                        });

                        rimuoviDestinatario.disable();

                        var view = Ext.getCmp('GridDestinatari').getView();
                        var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                        chkdiv.removeClass('x-grid3-hd-checker-on');
                    }
                }
            })
        }
    });

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridDestinatari').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviDestinatario.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridDestinatari').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviDestinatario.disable();
                }
                var view = Ext.getCmp('GridDestinatari').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var isDatoSensibileColumn = new Ext.grid.CheckColumn({
        header: "Dato Sensibile",
        dataIndex: 'isDatoSensibile',
        width: 48,
        readOnly: true
    });

    var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            { header: "Tipologia", width: 54, dataIndex: 'isPersonaFisica', renderer: renderTipologia, sortable: true },
            { header: "Nominativo", width: 178, dataIndex: 'Denominazione', sortable: true },
            { header: "Partita Iva/Cod. Fiscale", width: 125, dataIndex: 'isPersonaFisica', renderer: renderPartitaIva, sortable: true, locked: false },
            isDatoSensibileColumn,
            { header: "Num. Repertorio Contratto", width: 100, dataIndex: 'NumeroRepertorioContratto', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
                    var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

                    var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.IdContratto;
                    var link = "<a target='_blank' href='" + href + "' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorioContratto + "</a>";

                    return link;
                }
            },            
            { header: "Spesa Prevista", width: 178, dataIndex: 'ImportoSpettante', sortable: false },
            { header: "Data di Nascita", width: 85, dataIndex: 'DataNascita', sortable: true },
            { header: "Luogo di Nascita", width: 100, dataIndex: 'LuogoNascita', sortable: true, locked: false },
            { header: "Legale Rappresentante", width: 160, dataIndex: 'LegaleRappresentante', sortable: true, locked: false },
         	{ header: "IdSIC", dataIndex: 'IdSIC', id: 'IdSIC', sortable: true, hidden: true }
         	]);

    var store = new Ext.data.SimpleStore({
        fields: ['IdSIC',
                 'isPersonaFisica',
                 'LuogoNascita',
                 'DataNascita',
                 'Denominazione',
                 'CodiceFiscale',
                 'PartitaIva',
                 'LegaleRappresentante',
                 'IdContratto',
                 'NumeroRepertorioContratto',
                 'isDatoSensibile',
                 'ImportoSpettante']
    });

    var grid = new Ext.grid.GridPanel({
        id: 'GridDestinatari',
        autoHeight: false,
        height: 160,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        width: 604,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessun destinatario associato.",
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

    var gridDestinatari = new Ext.Panel({
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
                    style: 'margin-left:208px;margin-top:4px;margin-bottom:10px',
                    layoutConfig: {
                        columns: 5
                    },
                    items: [new Ext.LinkButton(importaDestinatari),
                            { xtype: 'label', style: 'margin-left:10px' },
                            new Ext.LinkButton(aggiungiDestinatario),
                            { xtype: 'label', style: 'margin-left:10px' },
                            new Ext.LinkButton(rimuoviDestinatario)
                    ]
                }
                ]
    }
        );

    rimuoviDestinatario.disable();
   
    return gridDestinatari;
}


function renderNominativo(destinatario) {
    if (destinatario.Tipologia == 'F') {
        return destinatario.Cognome + ' ' + destinatario.Nome;
    } else {
        return destinatario.Denominazione;
    }
}

function renderLegaleRappresentante(destinatario) {
    if (destinatario.Tipologia == 'F') {
        return "";
    } else {
        var nominativo = ""
        if (!isNullOrEmpty(destinatario.LegaleRappresentante.Cognome))
            nominativo += destinatario.LegaleRappresentante.Cognome;
        if (!isNullOrEmpty(destinatario.LegaleRappresentante.Nome))
            nominativo += " " + destinatario.LegaleRappresentante.Nome;
        return nominativo;
    }
}

function addDestinatario(idSIC, isPersonaFisica, denominazione,
                         codiceFiscale, partitaIva, luogoNascita, dataNascita,
                         legaleRappresentante, idContratto, numeroRepertorioContratto, isDatoSensibile, importoSpettante, replaceIfExists) {
    var retValue = ADD_LIST_RESULT.error;

    var gridDestinatari = Ext.getCmp('GridDestinatari');
    if (gridDestinatari != undefined && gridDestinatari != null) {
        var gridDestinatariStore = gridDestinatari.store;
        if (gridDestinatariStore != undefined && gridDestinatariStore != null) {
            if (!isNullOrEmpty(idSIC)) {
                var recordAlreadyExists = false;
                var replaceIndex = -1;

                for (var i = 0; !recordAlreadyExists && replaceIndex==-1 && i < gridDestinatariStore.getRange().length; i++) {
                    var record = gridDestinatariStore.getAt(i);
                    recordAlreadyExists = record.data.IdSIC == idSIC;

                    if (recordAlreadyExists && replaceIfExists != undefined && replaceIfExists != null && replaceIfExists) {
                        gridDestinatariStore.remove(record);                        
                        recordAlreadyExists = false;
                        replaceIndex = i;
                    }
                }
                                                   
                if (!recordAlreadyExists) {
                    var recordDestinatario = buildRecordDestinatario(idSIC, isPersonaFisica, denominazione,
                         codiceFiscale, partitaIva, luogoNascita, dataNascita,
                         legaleRappresentante, idContratto, numeroRepertorioContratto, isDatoSensibile, importoSpettante);

                    if (replaceIndex != -1)
                        gridDestinatariStore.insert(replaceIndex, recordDestinatario);
                    else
                        gridDestinatariStore.add(recordDestinatario);
                        
                    retValue = ADD_LIST_RESULT.ok;
                }
                else 
                    retValue = ADD_LIST_RESULT.exists;
            }
        }
    }

    return retValue;
}

function buildRecordDestinatario(idSIC, isPersonaFisica, denominazione,
                         codiceFiscale, partitaIva, luogoNascita, dataNascita,
                         legaleRappresentante, idContratto, numeroRepertorioContratto, isDatoSensibile, ImportoSpettante)
{
    DestinatarioRecordType = Ext.data.Record.create([
                         'IdSIC',
                         'isPersonaFisica',
                         'LuogoNascita',
                         'DataNascita',
                         'Denominazione',
                         'CodiceFiscale',
                         'PartitaIva',
                         'LegaleRappresentante',
                         'IdContratto',
                         'NumeroRepertorioContratto',
                         'isDatoSensibile',
                         'ImportoSpettante'
                    ]);
                    
                    
    var recordDestinatario = new DestinatarioRecordType({
        IdSIC: idSIC,
        isPersonaFisica: isPersonaFisica,
        LuogoNascita: normalizeString(luogoNascita),
        DataNascita: normalizeString(dataNascita),
        Denominazione: normalizeString(denominazione),
        CodiceFiscale: normalizeString(codiceFiscale),
        PartitaIva: normalizeString(partitaIva),
        LegaleRappresentante: normalizeString(legaleRappresentante),
        IdContratto: idContratto,
        NumeroRepertorioContratto: numeroRepertorioContratto,
        isDatoSensibile: isDatoSensibile,
        ImportoSpettante: ImportoSpettante
    });
    
    return recordDestinatario;
}

function normalizeString(stringToNormalize) {
    return isNullOrEmpty(stringToNormalize) ? "" : stringToNormalize;
}

function fnOnSelectSearchField(selectedField) {
    var btnSelezionaBen = Ext.getCmp('btnSelezionaDestinatario');
    if (btnSelezionaBen != undefined && btnSelezionaBen != null)
        btnSelezionaBen.disable();
}

function fnOnAfterSearchCommand() {
    Ext.getCmp("tab_panel").doLayout();
    Ext.getCmp("tab_panel").hide();
}

function fnOnStoreLoad(records) {
    if (records.length == 0) {
        if (Ext.getCmp('btnSelezionaDestinatario') != null && Ext.getCmp('btnSelezionaDestinatario') != undefined)
            Ext.getCmp('btnSelezionaDestinatario').disable();
    }
}

function fnOnSelectDestinatario(selectedRow) {
    if (selectedRow != null) {
        var destinatario = selectedRow.data;

        Ext.getCmp('anagrafica').setValue(Ext.util.JSON.encode(destinatario));
        Ext.get("idAnagrafica").value = destinatario.ID;

        if (Ext.getCmp('tab_datiDestinatario') != undefined)
            Ext.getCmp("tab_datiDestinatario").enable();

        if (Ext.getCmp('destinatarioInfoPanel') != undefined)
            Ext.getCmp('tab_datiDestinatario').remove(Ext.getCmp('destinatarioInfoPanel'));

        if (Ext.getCmp('contrattoSelectionPanel') != undefined)
            Ext.getCmp('tab_datiDestinatario').remove(Ext.getCmp('contrattoSelectionPanel'));

        if (Ext.getCmp('destinatarioQueryPanel') != undefined)
            Ext.getCmp('tab_datiDestinatario').remove(Ext.getCmp('destinatarioQueryPanel'));

        Ext.getCmp('tab_datiDestinatario').insert(0, buildDestinatarioInfoPanel(destinatario));
        Ext.getCmp('tab_datiDestinatario').insert(1, buildDestinatarioQueryPanel(destinatario));
        Ext.getCmp('tab_datiDestinatario').insert(2, buildContrattoSelectionPanel(destinatario));
                
        Ext.getCmp('tab_panel').show();
        setActivePanel('panelRicercaDestinatari', 'tab_datiDestinatario');
        Ext.getCmp('tab_panel').doLayout();

        scrollBottom('panelRicercaDestinatari');

        if (Ext.getCmp('btnSelezionaDestinatario') != null && Ext.getCmp('btnSelezionaDestinatario') != undefined)
            Ext.getCmp('btnSelezionaDestinatario').enable();
    } else {
        if (Ext.getCmp('tab_datiDestinatario') != null && Ext.getCmp('tab_datiDestinatario') != undefined)
            Ext.getCmp("tab_datiDestinatario").disable();

        Ext.getCmp('tab_panel').hide();

        if (Ext.getCmp('btnSelezionaDestinatario') != null && Ext.getCmp('btnSelezionaDestinatario') != undefined)
            Ext.getCmp('btnSelezionaDestinatario').disable();
    }
}

function showPopupPanelRicercaDestinatari() {
    var popup = new Ext.Window({
        title: 'Ricerca Destinatari',
        width: 850,
        height: 550,
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
            text: 'Inserisci Destinatario',
            id: 'btnSelezionaDestinatario'
        },
                {
                    text: 'Chiudi',
                    id: 'btnChiudiSelezioneDestinatario'
}]
    });

    var panelRicercaDestinatari = buildPanelRicercaDestinatari();
    popup.add(panelRicercaDestinatari);

    popup.doLayout();
    popup.show();

    Ext.getCmp('btnSelezionaDestinatario').disable();

    Ext.getCmp('btnSelezionaDestinatario').on('click', function() {        
        if (!checkDatiSensibiliDestinatario()) {
            Ext.MessageBox.show({
                title: 'Errore di validazione',
                msg: "E' necessario specificare se i dati relativi al destinatario sono o meno sensibili.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                }
            });
        } else if (!checkImportoSpesaDestinatario()){
            var warning_message = "Non è stato indicato l'importo di spesa previsto per il signolo destinatario. Continuare ? ";
            warning_message += "(Premendo 'Sì' nessun importo sarà indicato per il destinatario, premendo 'No' sarà possibile indicarlo)";

            Ext.MessageBox.buttonText.yes = "Sì";
            
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: warning_message,
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'yes') {
                        if (!checkContrattoDestinatario()) {
                            var warning_message = "Nessun contratto di quelli indicati nella 'Scheda Trasparenza' è stato associato al destinatario. Continuare ? ";
                            warning_message += "(Premendo 'Sì' nessun contratto sarà associato al destinatario, premendo 'No' sarà possibile effettuare l'associazione)";

                            Ext.MessageBox.buttonText.yes = "Sì";

                            Ext.MessageBox.show({
                                title: 'Attenzione',
                                msg: warning_message,
                                buttons: Ext.MessageBox.YESNO,
                                icon: Ext.MessageBox.WARNING,
                                fn: function(btn) {
                                    if (btn == 'yes') {
                                        selezionaEAggiungiDestinatario(); 
                                    } else if (btn == 'yes') {
                                        Ext.MessageBox.show({
                                            title: 'Errore di validazione',
                                            msg: "E' necessario specificare l'importo",
                                            buttons: Ext.MessageBox.OK,
                                            icon: Ext.MessageBox.ERROR,
                                            fn: function(btn) {
                                            }
                                        });
                                    }
                                }
                            });
                        } else 
                            selezionaEAggiungiDestinatario();
                    } else {
                       
                    }  
                }
            });
            
//            if (!checkContrattoDestinatario()) {
//                var warning_message = "Nessun contratto di quelli indicati nella 'Scheda Trasparenza' è stato associato al destinatario. Continuare ? ";
//                warning_message += "(Premendo 'Sì' nessun contratto sarà associato al destinatario, premendo 'No' sarà possibile effettuare l'associazione)";

//                Ext.MessageBox.buttonText.yes = "Sì";

//                Ext.MessageBox.show({
//                    title: 'Attenzione',
//                    msg: warning_message,
//                    buttons: Ext.MessageBox.YESNO,
//                    icon: Ext.MessageBox.WARNING,
//                    fn: function(btn) {
//                        if (btn == 'yes' && okNoImporto) {
//                            selezionaEAggiungiDestinatario(); 
//                        } else if (btn == 'yes' && !okNoImporto) {
//                            Ext.MessageBox.show({
//                                title: 'Errore di validazione',
//                                msg: "E' necessario specificare l'importo",
//                                buttons: Ext.MessageBox.OK,
//                                icon: Ext.MessageBox.ERROR,
//                                fn: function(btn) {
//                                }
//                            });
//                        }
//                    }
//                });
//            } else 
//                selezionaEAggiungiDestinatario();
        } else 
                selezionaEAggiungiDestinatario();
    });

    Ext.getCmp('btnChiudiSelezioneDestinatario').on('click', function() {
        popup.close();
    });
}

function selezionaEAggiungiDestinatario(replaceIfExists) {
    setImportoSpettante(Ext.getCmp('importoSpesaDestinatario').getValue());
    
    if (Ext.getCmp('yesSommaAutomatica').checked) {
        addToImportoTotaleSpettante(Ext.getCmp('importoSpesaDestinatario').getValue());
    }
    
    var retValue = ADD_LIST_RESULT.error;
    var destinatario = Ext.util.JSON.decode(Ext.getCmp('anagrafica').getValue());

    if (destinatario != null && destinatario != undefined)
        retValue = addDestinatario(destinatario.ID, destinatario.Tipologia == 'F',
                                   renderNominativo(destinatario), destinatario.CodiceFiscale,
                                   destinatario.PartitaIva, destinatario.ComuneNascita, destinatario.DataNascita,
                                   renderLegaleRappresentante(destinatario),
                                   destinatario.Contratto.Id, destinatario.Contratto.NumeroRepertorio, destinatario.isDatoSensibile, destinatario.ImportoSpettante, replaceIfExists);
    
    if (retValue == ADD_LIST_RESULT.exists) {
        var msg = 'Destinatario già incluso nella lista.<br>Sostituirlo ?';

        Ext.MessageBox.buttonText.yes = "Sì";

        Ext.MessageBox.show({
            title: 'Aggiunta Destinatario',
            msg: msg,
            buttons: Ext.MessageBox.YESNO,
            icon: Ext.MessageBox.WARNING,
            fn: function(btn) {
                if (btn == 'yes')
                    selezionaEAggiungiDestinatario(true);
            }
        });
    }
    else {
        var msg = 'Impossibile aggiungere il destinatario.';
        if (retValue == ADD_LIST_RESULT.ok)
            msg = 'Operazione completata con successo.';

        Ext.MessageBox.show({
            title: 'Aggiungi Destinatario',
            msg: msg,
            buttons: Ext.MessageBox.OK,
            icon: (retValue == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((retValue == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
            fn: function(btn) {
            }
        });
    }
}

function buildPanelRicercaDestinatari() {
    var actionAggiungiAnagrafica = new Ext.Action({
        text: 'Nuovo Destinatario',
        tooltip: 'Aggiungi un nuovo destinatario',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.anagrafica,
                        insertModes.newObject, {
                            updateFn: editInsertUpdateComboFn,
                            objectNameLabel: 'Destinatario'
                        });
        },
        iconCls: 'add'
    });

    var actionHelpOnLine = new Ext.Action({
        text: 'Aiuto',
        tooltip: 'Aiuto in linea: Associazione Destinatario',
        handler: function() {
            new Ext.IframeWindow({
                modal: true,
                layout: 'fit',
                title: 'Associazione Destinatario',
                width: 720,
                height: 500,
                closable: true,
                resizable: false,
                maximizable: false,
                plain: false,
                iconCls: 'help',
                bodyStyle: 'overflow:auto',
                src: 'risorse/helpAggiuntaDestinatario.htm'
            }).show();
        },
        iconCls: 'help'
    });

    var panelRicercaDestinatari = new Ext.FormPanel({
        layout: 'form',
        id: 'panelRicercaDestinatari',
        bodyStyle: 'padding:10px',
        height: 450,
        autoScroll: true,
        //tbar: [actionAggiungiAnagrafica, '->', actionHelpOnLine],
        tbar: [actionAggiungiAnagrafica],
        items: [
                    buildPanelSearchBeneficiari(false, false, undefined, fnOnAfterSearchCommand, fnOnStoreLoad, fnOnSelectDestinatario, fnOnSelectSearchField, undefined, "Destinatario Recente", undefined, undefined ), 
                    {
                        xtype: 'tabpanel',
                        plain: true,
                        id: 'tab_panel',
                        autoHeight: true,
                        defaults: { bodyStyle: 'padding:10px' },
                        style: {
                            "margin-top": Ext.isIE ? "9px" : "10px",
                            "margin-bottom": "10px"
                        },
                        hidden: true,
                        deferredRender: false,
                        items: [
                             { title: 'Scheda Destinatario',
                                 layout: 'auto',
                                 autoWidth: true,
                                 autoHeight: true, defaults: { autoHeight: true },
                                 id: 'tab_datiDestinatario'
                             }
                        ]
                    },
              {
                  xtype: 'hidden',
                  id: 'anagrafica'
              }, {
                  xtype: 'hidden',
                  id: 'idAnagrafica'
              }
        ]
    });

    return panelRicercaDestinatari;
}

function buildDestinatarioInfoPanel(destinatario) {
    var nominativo = destinatario.Tipologia == 'F' ? destinatario.Cognome + ' ' + destinatario.Nome : destinatario.Denominazione;
    var partitaIvaCod = destinatario.Tipologia == 'F' ? destinatario.CodiceFiscale : destinatario.PartitaIva;

    var labelText1 = 'Nominativo <b>' + nominativo + '</b> - Persona <b>' + (destinatario.Tipologia == 'F' ? 'FISICA' : 'GIURIDICA') + '</b>';
    var labelText2 = ''
    var labelText3 = ''

    if (!isNullOrEmpty(partitaIvaCod)) {
        labelText2 += (destinatario.Tipologia == 'F' ? 'Codice Fiscale ' : 'Partita IVA ') + '<b>' + partitaIvaCod + '</b>';
        labelText2 += (destinatario.Tipologia == 'F' ? ' - Sesso ' + '<b>' + destinatario.Sesso + '</b>' : '');
    }
    if (destinatario.Tipologia == 'F')
        labelText3 += 'Data di nascita <b>' + destinatario.DataNascita + '</b> - Luogo di nascita <b>' + destinatario.ComuneNascita + '</b>';
    else {
        var legaleRappresentante = "";

        if (!isNullOrEmpty(destinatario.LegaleRappresentante.Cognome))
            legaleRappresentante += destinatario.LegaleRappresentante.Cognome;
        if (!isNullOrEmpty(destinatario.LegaleRappresentante.Nome))
            legaleRappresentante += " " + destinatario.LegaleRappresentante.Nome;
        if (legaleRappresentante != "")
            labelText3 += 'Legale Rappresentante <b>' + legaleRappresentante + '</b>';
    }

    var panel = new Ext.Panel({
        border: false,
        layout: 'table',
        style: 'margin-bottom:2px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        id: "destinatarioInfoPanel",
        items: [{
                    xtype: 'label',
                    text: 'Destinatario',
                    style: Ext.isIE ? 'padding-right:57px' : 'padding-right:57px'
                },
                {
                    xtype: 'panel',
                    plain: true,
                    border: true,
                    layout: 'table',                        
                    bodyStyle: 'padding:4px 4px 4px 5px',
                    width: Ext.isIE ? 514 : 515,
                    layoutConfig: {
                        columns: 1
                    },
                    items: [
                        {
                            xtype: 'label',
                            id: 'labelDestinatarioInfo1Id',
                            html: labelText1,
                            style: 'padding-right:85px'
                        },
                        {
                            xtype: 'label',
                            id: 'labelDestinatarioInfo2Id',
                            html: labelText2,
                            style: 'padding-right:85px'
                        }, (labelText3.trim().length != 0 ?
                        {
                            xtype: 'label',
                            id: 'labelDestinatarioInfo3Id',
                            html: labelText3,
                            style: 'padding-right:85px'
                        } : { xtype: 'label' })

                        ]
                    }
                    ]
    });

    return panel;
}

function editInsertUpdateComboFn(objectType, insertMode, objectData, result) {
    if (objectType == objectTypes.anagrafica)
        preFillAllResultFields(false, result.IdAnagrafica, result.IdSede, result.IdTipoPagamento, result.IdContoCorrente);
}

function buildContrattoSelectionPanel(destinatario) {

    var actionVisualizzaContratto = new Ext.Action({
        text: 'Visualizza Contratto',
        id: 'actionApriContrattoId',
        tooltip: 'Visualizza il contratto relativo al numero di repertorio indicato',
        handler: function() {
            var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
            var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();
            var num_contr = comboContratti.getValue();

            window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + num_contr, '_blank');
        }
    });

    var comboContratti = buildComboContrattiDocumento("comboContrattiDestinatarioId", Ext.isIE ? 515 : 515 , false,
        function(combo, value) {
            var contratto = getContrattoInfo(value.data.Id, "comboContrattiDestinatarioId");
            var anagrafica = Ext.util.JSON.decode(Ext.getCmp("anagrafica").getValue());

            anagrafica.Contratto.Id = contratto.Id
            anagrafica.Contratto.NumeroRepertorio = contratto.NumeroRepertorio;

            Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(anagrafica));

            if (comboContratti.getValue() == 'none') {
                comboContratti.setValue('');
                Ext.getCmp("oggettoContratto").setValue('');
                actionVisualizzaContratto.disable();
                Ext.getCmp('oggettoContratto').disable();
            } else {
                actionVisualizzaContratto.enable();
                Ext.getCmp('oggettoContratto').enable();
                Ext.getCmp("oggettoContratto").setValue(contratto.Oggetto);
            }
        },
        undefined,
        function(addAllContrattiLabel, fnOnLoad) {
            var store = new Ext.data.SimpleStore({
                fields: ['Id',
                     'NumeroRepertorio',
                     'Oggetto']
            });

            var contratti = getContrattiAssociati();

            for (var i = 0; i < contratti.length; i++) {
                var recordAlreadyExists = store.queryBy(
                        function(record, recordId) {
                            return (record.data.Id == contratti[i].Id)
                        });

                if (!(recordAlreadyExists != undefined && recordAlreadyExists.length > 0)) {
                    ContrattoRecordType = Ext.data.Record.create(['Id', 'NumeroRepertorio', 'Oggetto']);
                    var newRecordContratto = new ContrattoRecordType({
                        Id: contratti[i].Id
                                , NumeroRepertorio: contratti[i].NumeroRepertorio
                                , Oggetto: contratti[i].Oggetto
                    });

                    store.add(newRecordContratto);
                }
            }

            ContrattoRecordType = Ext.data.Record.create(['Id', 'NumeroRepertorio', 'Oggetto']);
            if (addAllContrattiLabel != undefined && addAllContrattiLabel != null && addAllContrattiLabel && records.length > 0) {
                var newRecordContratto = new ContrattoRecordType({
                    Id: 'all'
                        , NumeroRepertorio: 'Tutti'
                        , Oggetto: 'Tutti i contratti associati al provvedimento'
                });
            } else {
                var newRecordContratto = new ContrattoRecordType({
                    Id: 'none'
                            , NumeroRepertorio: 'Nessuno'
                            , Oggetto: 'Nessun contratto da associare al beneficiario'
                });
            }

            store.insert(0, newRecordContratto);

            if (fnOnLoad != null && fnOnLoad != undefined)
                fnOnLoad(store, store.getRange());

            return store;
        }
    );

    var panel = new Ext.Panel({
        border: false,
        style: 'margin-bottom:0px',
        id: "contrattoSelectionPanel",
        items: [
            { xtype: 'panel',
                layout: 'table',
                border: false,
                layoutConfig: {
                    columns: 2
                },
                items: [
                {
                    xtype: 'label',
                    id: 'labelSelectionContrattoId',
                    text: 'Repertorio Contratto  ',
                    style: Ext.isIE ? 'padding-right:11px' : 'padding-right:14px'
                },
                {
                    layout: "column",
                    border: false,
                    anchor: "0",
                    width: 636,
                    items: [{
                        border: false,
                        style: 'margin-right: 10px',
                        items: [
					comboContratti
				]
                    }, {
                        border: false,
                        items: [
					new Ext.LinkButton(actionVisualizzaContratto)
				]
            }]
            }]
                    },
            { xtype: 'panel',
                layout: 'table',
                border: false,
                style: Ext.isIE ? 'margin-top: 1px' : 'margin-top: 2px',
                layoutConfig: {
                    columns: 2
                },
                items: [
                {
                    xtype: 'label',
                    id: 'labelOggettoContrattoId',
                    text: 'Oggetto  ',
                    style: Ext.isIE ? 'padding-right:57px' : 'padding-right:60px'
                }, {
                    xtype: 'textarea',
                    maxLength: 1024,
                    readOnly: true,
                    id: 'oggettoContratto',
                    style: 'margin-left: 15px',
                    width: Ext.isIE ? 516 : 515,
                    height: 34
                }
                ]
            }
        ]
    });

    initContrattoSelectionPanel(destinatario);

    return panel;
}

function setIsDatoSensibile(isDatoSensibile) {
    var anagrafica = Ext.util.JSON.decode(Ext.getCmp("anagrafica").getValue());
    anagrafica.isDatoSensibile = isDatoSensibile;
    Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(anagrafica));
}

function setImportoSpettante(importoSpettante) {
    var anagrafica = Ext.util.JSON.decode(Ext.getCmp("anagrafica").getValue());
    anagrafica.ImportoSpettante = importoSpettante;
    Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(anagrafica));
}

function addToImportoTotaleSpettante(importoSpettanteDest) {        
    var importoTot = parseFloat(String(Ext.getCmp("importoSpesaPrevista").getValue()).replace(',','.')) + parseFloat(String(importoSpettanteDest).replace(',','.'));
    Ext.getCmp("importoSpesaPrevista").setValue(importoTot.toFixed(2));
}

function removeToImportoTotaleSpettante(importoSpettanteDest) {        
    var importoTot = parseFloat(String(Ext.getCmp("importoSpesaPrevista").getValue()).replace(',','.')) - parseFloat(String(importoSpettanteDest).replace(',','.'));
    Ext.getCmp("importoSpesaPrevista").setValue(importoTot.toFixed(2));
}

function buildDestinatarioQueryPanel(destinatario) {
    var yesOption = new Ext.form.Radio({
        boxLabel: 'Si',
        id: 'yesDatoSensibile',
        name: 'selezioneDatoSensibile',
        inputValue: 'SI',
        checked: false
    });

    var noOption = new Ext.form.Radio({
        boxLabel: 'No',
        id: 'noDatoSensibile',
        name: 'selezioneDatoSensibile',
        inputValue: 'NO',
        checked: false
    });

    yesOption.on('check', function() {
        if (Ext.getCmp('yesDatoSensibile').checked) {
            setIsDatoSensibile(true);
        } 
    });

    noOption.on('check', function() {
        if (Ext.getCmp('noDatoSensibile').checked) {
            setIsDatoSensibile(false);
        } 
    });
            
    var panel = new Ext.Panel({
        border: false,
        layout: 'table',
        style: 'margin-bottom:10px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        id: "destinatarioQueryPanel",
        items: [{
            xtype: 'label',
            text: 'Dato sensibile(*)',
            style: Ext.isIE ? 'padding-right:35px' : 'padding-right:35px'
        }, {
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
        },{
            xtype: 'label',
            text: 'Spesa Prevista',
            style: Ext.isIE ? 'padding-right:35px' : 'padding-right:35px'
        }, {
            xtype: 'numberfield',
            name: 'importoSpesaDestinatario',
            id: 'importoSpesaDestinatario',
            decimalSeparator: ',',
            allowNegative: false,
            width: 200,
            allowBlank: true,
			value: 0
        }
                
                    ]
    });

    return panel;
}

function initContrattoSelectionPanel(destinatario) {
    var comboContratti = Ext.getCmp('comboContrattiDestinatarioId');

    if (comboContratti.store.getRange().length == 1) {
        Ext.getCmp('labelSelectionContrattoId').disable();
        Ext.getCmp('comboContrattiDestinatarioId').disable();
        Ext.getCmp('actionApriContrattoId').disable();
        Ext.getCmp('labelOggettoContrattoId').disable();
        Ext.getCmp('oggettoContratto').disable();
    } else {
        Ext.getCmp('labelSelectionContrattoId').enable();
        Ext.getCmp('comboContrattiDestinatarioId').enable();
        Ext.getCmp('actionApriContrattoId').enable();
        Ext.getCmp('labelOggettoContrattoId').enable();
        Ext.getCmp('oggettoContratto').enable();

        if (destinatario.Contratto != undefined && destinatario.Contratto != null && !isNullOrEmpty(destinatario.Contratto.Id)) {
            comboContratti.setValue(destinatario.Contratto.Id);
            comboContratti.disable();

            Ext.getCmp('actionApriContrattoId').enable();

            var contratto = getContrattoInfo(comboContratti.getValue(), "comboContrattiDestinatarioId");

            Ext.getCmp('oggettoContratto').disable();
            Ext.getCmp("oggettoContratto").setValue(contratto.Oggetto);
        } else {
            Ext.getCmp('actionApriContrattoId').disable();
            Ext.getCmp('oggettoContratto').disable();
        }
    }
}

function getContrattoInfo(idContratto, idComboContratti) {
    var retValue = undefined;

    var comboContratti = Ext.getCmp(idComboContratti);
    if (comboContratti != undefined && comboContratti != null) {
        var record = comboContratti.findRecord(comboContratti.valueField || comboContratti.displayField, idContratto);
        if (record != undefined && record != null)
            retValue = record.data
    }

    return retValue;
}

function checkContrattoDestinatario() {
    var retValue = true;

    var combobox = Ext.getCmp('comboContrattiDestinatarioId');
    var count = combobox.store.getCount();
    if (count > 1) {
        var contratto = getContrattoInfo(combobox.getValue(), "comboContrattiDestinatarioId");
        if (contratto == undefined)
            retValue = false;
    }

    return retValue;
}

function scrollBottom(panelName) {
    var panelDom = Ext.getCmp(panelName).body.dom;
    var scrollValue = (panelDom.scrollHeight - panelDom.offsetHeight);

    Ext.getCmp(panelName).body.scrollTo('top', scrollValue, true);
}

function checkDatiSensibiliDestinatario() {
    return !((Ext.getCmp('yesDatoSensibile') == undefined || Ext.getCmp('yesDatoSensibile').checked == false) &&
            (Ext.getCmp('noDatoSensibile') == undefined || Ext.getCmp('noDatoSensibile').checked == false));
}

function checkImportoSpesaDestinatario() {
    return !(Ext.getCmp('importoSpesaDestinatario').value == undefined || Ext.getCmp('importoSpesaDestinatario').value == "");
}


function importDestinatari() {
    if (isPubblicazioneYesSelected()) {
        if (isDeliberaSelectedAndValid() || isDeterminaSelectedAndValid()) {
            var tipoDocumento = isDeliberaSelectedAndValid() ? TIPO_DOCUMENTO.delibera : TIPO_DOCUMENTO.determina;

            Ext.MessageBox.buttonText.yes = "Sì";
            Ext.MessageBox.show({
                title: 'Importa Destinatari',
                msg: "Procedere con l'importazione dei destinatari della <b>" +
                     (tipoDocumento == TIPO_DOCUMENTO.delibera ?
                     "Delibera '" + buildNumeroDelibera(Ext.getCmp('annoDelibera').getValue(),
                                Ext.getCmp('numeroDelibera').getValue()) + "'" :
                     "Determina '" + buildNumeroDetermina(Ext.getCmp('ufficioDetermina').getValue(),
                                Ext.getCmp('annoDetermina').getValue(),
                                Ext.getCmp('numeroDetermina').getValue()) + "'") +
                     "</b> indicata quale norma per l'attribuzione del beneficio nella 'Scheda Trasparenza' ?",
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.INFO,
                fn: function(btn) {
                    if (btn == 'yes') {
                        if (tipoDocumento == TIPO_DOCUMENTO.delibera)
                            importaDestinatariDocumento(tipoDocumento, Ext.getCmp('annoDelibera').getValue(),
                                                Ext.getCmp('numeroDelibera').getValue());
                        else
                            importaDestinatariDocumento(tipoDocumento, Ext.getCmp('annoDetermina').getValue(),
                                                Ext.getCmp('numeroDetermina').getValue(), Ext.getCmp('ufficioDetermina').getValue());
                    }
                }
            });
        } else {
            var warningMsg = "Per poter procedere con l'importazione è necessario specificare un atto (delibera o determina) alla base dell'attribuzione del beneficio nella 'Scheda Trasparenza'."

            Ext.MessageBox.show({
                title: 'Importa Destinatari',
                msg: 'Importazione non disponibile.<br>' + warningMsg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                }
            });
        }
    } else {
    var warningMsg = "Per poter procedere è necessario autorizzare la pubblicazione delle informazioni nella 'Scheda Trasparenza' " +
                     "e specificare un atto (delibera o determina) alla base dell'attribuzione del beneficio.";
                     
        Ext.MessageBox.show({
            title: 'Importa Destinatari',
            msg: 'Importazione non disponibile. ' + warningMsg,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.WARNING,
            fn: function(btn) {
            }
        });
    }
}

function importaDestinatariDocumento(tipoDocumento, anno, numero, ufficio) {             
    var mask = new Ext.LoadMask(Ext.getBody(), {
                         msg: "Importazione destinatari in corso..."
                   });
    mask.show();

    Ext.Ajax.request({
        url: 'ProcAmm.svc/InterrogaDocumenti?tipoDoc='+tipoDocumento+'&dataDa=01-01-' + anno + '&dataA=31-12-' + anno + '&numDoc=' + numero + 
                (tipoDocumento==TIPO_DOCUMENTO.determina ? '&codUff=' + ufficio + '&validateUfficio=false' : ''),                
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        method: 'POST',
        success: function(response, options) {
            if (mask != null && mask != undefined)
                mask.hide();
            var data = Ext.decode(response.responseText);
            if (data.InterrogaDocumentiResult.TotalCount <= 1) {
                importaDestinatariDocumentoOnLoad(tipoDocumento, anno, numero, ufficio, GET_DOCUMENT_RESULT_STATUS.success, "success",
                    data.InterrogaDocumentiResult.TotalCount == 1 ? data.InterrogaDocumentiResult.Data[0] : null);
            } else
                importaDestinatariDocumentoOnLoad(tipoDocumento, anno, numero, ufficio, GET_DOCUMENT_RESULT_STATUS.error, "I criteri di ricerca specificati hanno prodotto più di un risultato. Criteri non validi.", null);
        },
        failure: function(response, options) {
            if (mask != null && mask != undefined)
                mask.hide();
            var data = Ext.decode(response.responseText);
            importaDestinatariDocumentoOnLoad(tipoDocumento, anno, numero, ufficio, data.FaultCode, data.FaultMessage, null);
        }
    });
}

function importaDestinatariDocumentoOnLoad(tipoDocumento, anno, numero, ufficio, errorCode, errorMsg, data) {
    if (errorCode == GET_DOCUMENT_RESULT_STATUS.success) {
    
        errorMsg = 'Operazione completata con successo';
        var msgLevel = MSG_LEVEL.info;

        if (data == null || data == undefined) {
            if(tipoDocumento==TIPO_DOCUMENTO.delibera)
                errorMsg = "Delibera '" + buildNumeroDelibera(anno, numero) + "' non trovata.";
            else
                errorMsg = "Determina '" + buildNumeroDetermina(ufficio, anno, numero) + "' non trovata.";
            msgLevel = MSG_LEVEL.warning;
        } else {
            var destinatari = ((data != null && data != undefined) && (data.Doc_DestinatariDocumento != null && data.Doc_DestinatariDocumento != undefined)) ? data.Doc_DestinatariDocumento : null;

            if (destinatari!=null && destinatari.length > 1) {
                for (var i = 0; i < destinatari.length; i++) {
                    var destinatario = destinatari[i];
                    if (destinatario != undefined && destinatario != null)
                        addDestinatario(destinatario.IdSIC, destinatario.isPersonaFisica, destinatario.Denominazione,
                                       destinatario.CodiceFiscale, destinatario.PartitaIva, destinatario.LuogoNascita, destinatario.DataNascita,
                                       destinatario.legaleRappresentante, destinatario.IdContratto, destinatario.NumeroRepertorioContratto, destinatario.isDatoSensibile, destinatario.ImportoSpettante,  true);
                }                
            } else {
                errorMsg = 'Nessun destinatario trovato';
                msgLevel = MSG_LEVEL.warning;
            }            
        }                
    }

    Ext.MessageBox.show({
        title: 'Importa Destinatari',
        msg: errorMsg,
        buttons: Ext.MessageBox.OK,
        icon: msgLevel == MSG_LEVEL.info ? Ext.MessageBox.INFO : (msgLevel == MSG_LEVEL.warning ? Ext.MessageBox.WARNING: Ext.MessageBox.ERROR),
        fn: function(btn) {
        }
    });        
}