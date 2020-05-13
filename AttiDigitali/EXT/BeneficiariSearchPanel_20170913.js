var searchFields = { "codiceSIC": 0, "codiceFiscale": 1, 
                    "partitiIva": 2, "denominazione": 3,
                    "contratti": 4, "beneficiariRecenti": 5,
                    "destinatariAssociati": 6, "fatture": 7};
                
function setSearchFieldToShow(searchFieldCode, setSelector) {
    if (setSelector) {
        var selector = Ext.getCmp('tipoRicerca');
        selector.setValue(searchFieldCode);
    }

    var codiceSIC = Ext.getCmp('codiceSIC');
    var codFiscale = Ext.getCmp('codFiscale');
    var pIva = Ext.getCmp('pIva');
    var denominazione = Ext.getCmp('denominazione');
    var comboBeneficiariCronologia = Ext.getCmp('comboBeneficiariCronologiaId');
    var comboContratti = Ext.getCmp('comboContrattiId');
    var comboFatture = Ext.getCmp('comboFattureId');
    var comboDestinatariAssociati = Ext.getCmp('comboDestinatariAssociatiId');

    codiceSIC.hide();
    codFiscale.hide();
    pIva.hide();
    denominazione.hide();
    if (comboContratti != undefined && comboContratti != null)
        comboContratti.hide();
    if (comboFatture != undefined && comboFatture != null)
        comboFatture.hide();
    if (comboDestinatariAssociati != undefined && comboDestinatariAssociati != null)
        comboDestinatariAssociati.hide();
    comboBeneficiariCronologia.hide();

    Ext.getCmp('risultatiRicerca').hide();
    Ext.getCmp('tab_panel').hide();

    resetSearchFields();

    if (comboContratti != undefined && comboContratti != null)
        comboContratti.setValue('');
    if (comboFatture != undefined && comboFatture != null)
        comboFatture.setValue('');
    if (comboDestinatariAssociati != undefined && comboDestinatariAssociati != null)
        comboDestinatariAssociati.setValue('');
    comboBeneficiariCronologia.setValue('');

    var value = searchFieldCode;

    if (value == searchFields.codiceSIC) {
        codiceSIC.show();
        Ext.getCmp('btnCerca').enable();
    }
    else if (value == searchFields.codiceFiscale) {
        codFiscale.show();
        Ext.getCmp('btnCerca').enable();
    }
    else if (value == searchFields.partitiIva) {
        pIva.show();
        Ext.getCmp('btnCerca').enable();
    }
    else if (value == searchFields.denominazione) {
        denominazione.show();
        Ext.getCmp('btnCerca').enable();
    }
    else if (value == searchFields.contratti && comboContratti != undefined && comboContratti != null) {
        comboContratti.show();
        Ext.getCmp('btnCerca').disable();
    }
    else if (value == searchFields.fatture && comboFatture != undefined && comboFatture != null) {
    comboFatture.show();
        //STEF
        Ext.getCmp('btnCerca').enable();
    }
    else if (value == searchFields.destinatariAssociati && comboDestinatariAssociati != undefined && comboDestinatariAssociati != null) {
        comboDestinatariAssociati.show();
        Ext.getCmp('btnCerca').disable();
    }
    else if (value == searchFields.beneficiariRecenti) {
        comboBeneficiariCronologia.show();
        Ext.getCmp('btnCerca').disable();
    }
}

function buildPanelSearchBeneficiari(useDocumentRestrictedSearchFields, useAutoSilentSearch, fnOnBeforeSearchCommand, fnOnAfterSearchCommand, fnOnStoreBeneficiariLoad, fnOnSelectBeneficiario, fnOnSelectSearchField, resultSearchGridHeight, beneficiariRecentiLabel, liquidazione, impegno, fatturePresentiSuLiq) {
    var denominazione = new Ext.form.TextField({
        id: 'denominazione',
        width: 442,
        labelSeparator: '',
        blankText: '',
        emptyText: '',
        style: 'margin-top: 0px',
        value: '',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });
    var codFiscale = new Ext.form.TextField({
        id: 'codFiscale',
        labelSeparator: '',
        blankText: '',
        width: 442,
        style: 'margin-top: 0px',
        emptyText: '',
        value: '',
        maxLength: 16,
        minLength: 16,
        minLengthText: 'La lunghezza deve essere di 16',
        maxLengthText: 'La lunghezza deve essere di 16',
        validator: function(valu) {
            var pattern = /[a-zA-Z]{6}\d\d[a-zA-Z]\d\d[a-zA-Z]\d\d\d[a-zA-Z]/
            var stringa = valu.trim()
            var result = stringa.search(pattern)
            return result > -1;
        },
        invalidText: 'Il codice Fiscale inserito non è corretto',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });
    var pIva = new Ext.form.TextField({
        id: 'pIva',
        labelSeparator: '',
        blankText: '',
        emptyText: '',
        width: 442,
        style: 'margin-top: 0px',
        value: '',
        maxLength: 11,
        minLength: 11,
        minLengthText: 'La lunghezza deve essere di 11',
        maxLengthText: 'La lunghezza deve essere di 11',
        validator: function(valu) {
            var pattern = '^[0-9]{11}$'
            var stringa = valu.trim()
            var result = stringa.search(pattern)
            return result > -1;
        },
        invalidText: 'La partita iva inserita non è corretta',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });

    var codiceSIC = new Ext.form.TextField({
        id: 'codiceSIC',
        labelSeparator: '',
        blankText: '',
        width: 442,
        style: 'margin-top: 0px',
        emptyText: '',
        value: '',
        validator: function(valu) {
            var pattern = '^([0-9]{0,})$'
            var stringa = valu.trim()
            var result = stringa.search(pattern)
            return result > -1;
        },
        invalidText: 'Il codice SIC inserito non è corretto',
        listeners: {
            scope: this,
            specialkey: function(f, e) {
                if (e.getKey() == e.ENTER) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                }
            }
        }
    });

    var comboFatture = undefined;
    if (liquidazione != undefined) {         
        if (useDocumentRestrictedSearchFields) {
            comboFatture = buildComboFattureDocumento("comboFattureId", 442, true,
            function(combo, value) {
            var btnCerca = Ext.getCmp('btnCerca');
            //STEF
    //        btnCerca.enable();
                btnCerca.fireEvent('click');
            },
            function(store, records, options) {
                if (records.length == 0) {
                    Ext.getCmp('comboFattureId').disable();
                } else {
                    Ext.getCmp('comboFattureId').enable();
                    Ext.getCmp('comboFattureId').show();
                    Ext.getCmp('comboBeneficiariCronologiaId').hide();
                    Ext.getCmp('comboBeneficiariCronologiaId').disable();
                    Ext.getCmp('tipoRicerca').setValue('7');
                }
            }, undefined
             , liquidazione        
            );
        }
    }

    var comboBeneficiariCronologia = new Ext.form.ComboBox({
        displayField: 'Nominativo',
        valueField: 'IdBeneficiario',
        id: 'comboBeneficiariCronologiaId',
        style: 'margin-top: 0px',
        listWidth: 442,
        width: 442,
        store: getBeneficiariCronologiaStore(function(records) {
            Ext.getCmp('tipoRicercaLabel').enable();
            Ext.getCmp('tipoRicerca').enable();
            if (records.length > 0) {
                if (fatturePresentiSuLiq == true) {
                    Ext.getCmp('comboBeneficiariCronologiaId').hide();
                    Ext.getCmp('comboBeneficiariCronologiaId').disable();
                    Ext.getCmp('tipoRicerca').setValue('7');

                    if (comboFatture != undefined && comboFatture != null)
                        comboFatture.show();
                    
                } else {
                    Ext.getCmp('tipoRicerca').setValue('5');
                    Ext.getCmp('comboBeneficiariCronologiaId').enable();
                    Ext.getCmp('comboBeneficiariCronologiaId').show();
                }
                
                Ext.getCmp('btnCerca').disable();

            } else {
                if (fatturePresentiSuLiq == false) {
                    Ext.getCmp('comboBeneficiariCronologiaId').hide();
                    Ext.getCmp('comboBeneficiariCronologiaId').disable();

                    Ext.getCmp('tipoRicerca').setValue('3');
                    denominazione.show();

                    Ext.getCmp('btnCerca').enable();
                }
            }
            Ext.getCmp('btnCerca').show();
        }),
        readOnly: true,
        mode: 'local',
        allowBlank: true,
        queryMode: 'local',
        triggerAction: 'all',
        emptyText: 'Selezionare un ' + (beneficiariRecentiLabel != undefined && beneficiariRecentiLabel != null && beneficiariRecentiLabel.trim() != '' ? beneficiariRecentiLabel.toLowerCase() : 'beneficiario recente') + '...',
        tpl: '<tpl for="."><div class="x-combo-list-item"><b>{Nominativo}</b> - {CodFiscPIva}<br/>{DescrSede}</br>{DescrModPagamento} - {DescrDatiBancari}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    preFillAllResultFields(useAutoSilentSearch,
                                value.data.IdBeneficiario,
                                value.data.IdSede,
                                value.data.IdTipoPagamento,
                                value.data.IdContoCorrente);
                }
            }
        }
    });

    var comboContratti = undefined;
    if (useDocumentRestrictedSearchFields) {
        comboContratti = buildComboContrattiDocumento("comboContrattiId", 442, true,
                function(combo, value) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                },
                function(store, records, options) {
                    if (records.length == 0)
                        Ext.getCmp('comboContrattiId').disable();
                    else
                        Ext.getCmp('comboContrattiId').enable();
                }
            );
    }

    

    var comboDestinatariAssociati = undefined;
    if (useDocumentRestrictedSearchFields) {
        comboDestinatariAssociati = buildComboDestinatariAssociatiDocumento(
                function(combo, value) {
                    var btnCerca = Ext.getCmp('btnCerca');
                    btnCerca.fireEvent('click');
                },
                function(store, records, options) {
                    if (records.length == 0)
                        Ext.getCmp('comboDestinatariAssociatiId').disable();
                    else
                        Ext.getCmp('comboBeneficiatiAssociatiId').enable();
                }
            );
    }

    codiceSIC.hide();
    denominazione.hide();
    pIva.hide();
    codFiscale.hide();
    if (comboContratti != undefined && comboContratti != null)
        comboContratti.hide();
    if (comboFatture != undefined && comboFatture != null)
        comboFatture.hide();
    if (comboDestinatariAssociati != undefined && comboDestinatariAssociati != null)
        comboDestinatariAssociati.hide();
    comboBeneficiariCronologia.hide();

    var risultatiRicerca = new Ext.Panel({
        border: false,
        id: 'risultatiRicerca'
    });
        
    storeRicercaData = new Array();
    storeRicercaData.push(new Array(searchFields.codiceSIC, 'Codice SIC'));
    storeRicercaData.push(new Array(searchFields.codiceFiscale, 'Codice Fiscale'));
    storeRicercaData.push(new Array(searchFields.partitiIva, 'Partita IVA'));
    storeRicercaData.push(new Array(searchFields.denominazione, 'Denominazione'));
    if (comboContratti != undefined && comboContratti != null)
        storeRicercaData.push(new Array(searchFields.contratti, 'Contratto'));
    if (comboFatture != undefined && comboFatture != null)
        storeRicercaData.push(new Array(searchFields.fatture, 'Fatture'));
    if (comboDestinatariAssociati != undefined && comboDestinatariAssociati != null)
        storeRicercaData.push(new Array(searchFields.destinatariAssociati, 'Destinatari Atto'));
    storeRicercaData.push(new Array(searchFields.beneficiariRecenti, beneficiariRecentiLabel!=undefined && beneficiariRecentiLabel!=null && beneficiariRecentiLabel.trim()!='' ? beneficiariRecentiLabel : 'Beneficiario Recente'));
    
    var storeRicerca = new Ext.data.SimpleStore({
        fields: ['value', 'description'],
        data: storeRicercaData,
        autoLoad: true
    });

    var searchBeneficiariPanel = new Ext.Panel({
        id: 'panelSearchBeneficiari',
        border: false,
        layout: 'auto',
        plain: true,
        items: [{
            xtype: 'panel',
            plain: true,
            border: true,
            layout: 'table',
            bodyStyle: 'padding:10px; background: #EBF3FD',
            layoutConfig: {
                columns: 1
            },
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: 'table',
                    bodyStyle: 'background: #EBF3FD',
                    layoutConfig: {
                        columns: 3
                    },
                    items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: 'table',
                    bodyStyle: 'padding-top:0px;margin-top:0px;background: #EBF3FD',
                    layoutConfig: {
                        columns: 2
                    },
                    items: [
                                { xtype: 'label',
                                    style: 'margin-right:10px',
                                    text: 'Ricerca per ',
                                    id: 'tipoRicercaLabel'
                                },
                                {
                                    xtype: 'combo',
                                    style: 'margin-top: 0px',
                                    displayField: 'description',
                                    valueField: 'value',
                                    triggerAction: 'all',
                                    selectOnFocus: true,
                                    typeAhead: true,
                                    mode: 'local',
                                    store: storeRicerca,
                                    name: 'tipoRicerca',
                                    id: 'tipoRicerca',
                                    queryMode: 'local',
                                    readOnly: true,
                                    width: 150,
                                    listeners: {
                                        select: {
                                            fn: function(combo, item) {
                                                setSearchFieldToShow(item.data.value);
                                                if (fnOnSelectSearchField != undefined && fnOnSelectSearchField != null)
                                                    fnOnSelectSearchField(item.data.value);
                                            }
                                        }
                                    }
                                }
		                     ]
                }, {
                    xtype: 'panel',
                    border: false,
                    style: 'margin-left: 10px;margin-top:' + (Ext.isIE ? '1' : '0') + 'px',
                    bodyStyle: 'padding-top: 0px;background: #EBF3FD',
                    items: [codiceSIC,
                            codFiscale,
                            pIva,
                            denominazione,
                            comboContratti != undefined && comboContratti != null ? comboContratti : { hidden: true },
                            comboFatture != undefined && comboFatture != null ? comboFatture : { hidden: true },
                            comboDestinatariAssociati != undefined && comboDestinatariAssociati != null ? comboDestinatariAssociati : { hidden: true },
                            comboBeneficiariCronologia
                    ]
                },
                {
                    xtype: 'button', id: 'btnCerca', text: 'Cerca', style: 'margin-left: 10px'
}]
                }
            ]
        },
         risultatiRicerca, 
          {
              xtype: 'hidden',
              id: 'idResultAnagrafica',
              value: null
          }, {
              xtype: 'hidden',
              id: 'idResultSede',
              value: null
          }, {
              xtype: 'hidden',
              id: 'idResultTipoPagamento',
              value: null
          }, {
              xtype: 'hidden',
              id: 'idResultConto',
              value: null
          }, {
              xtype: 'hidden',
              id: 'idSearchModel',
              value: 'onSearch'
}]
    });

    Ext.getCmp('btnCerca').on('click', function() {
        var codiceSIC = '';
        var denominazione = '';
        var codFiscale = '';
        var pIva = '';
        var idContratto = "";
        var idFattura = "";
        var idDestinatariAssociati = "";

        codiceSIC = Ext.get('codiceSIC').getValue();
        if (!(codiceSIC != undefined && codiceSIC != null && codiceSIC.trim().length != 0)) {
            denominazione = Ext.get('denominazione').getValue();
            codFiscale = Ext.get('codFiscale').getValue();
            pIva = Ext.get('pIva').getValue();

            var comboContratti = Ext.getCmp('comboContrattiId');

            if (comboContratti != undefined && comboContratti != null) {
                var value = comboContratti.getValue();
                if (value != undefined && value != null) {
                    var record = comboContratti.findRecord(comboContratti.valueField || comboContratti.displayField, value);

                    if (record != undefined && record != null)
                        idContratto = record.data.Id;
                }
            }

            var comboFatture = Ext.getCmp('comboFattureId');

            if (comboFatture != undefined && comboFatture != null) {
                var value = comboFatture.getValue();
                if (value != undefined && value != null) {
                    var record = comboFatture.findRecord(comboFatture.valueField || comboFatture.displayField, value);

                    if (record != undefined && record != null)
                        idFattura = record.data.IdUnivoco;
                }
            }

            var comboDestinatariAssociati = Ext.getCmp('comboDestinatariAssociatiId');

            if (comboDestinatariAssociati != undefined && comboDestinatariAssociati != null) {
                var value = comboDestinatariAssociati.getValue();
                if (value != undefined && value != null) {
                    var record = comboDestinatariAssociati.findRecord(comboDestinatariAssociati.valueField || comboDestinatariAssociati.displayField, value);

                    if (record != undefined && record != null)
                        idDestinatariAssociati = record.data.Id;
                }
            }
        }

        if ((codiceSIC == undefined || codiceSIC == null || codiceSIC.trim() == '') &&
            (denominazione == undefined || denominazione == null || denominazione.trim() == '') &&
            (codFiscale == undefined || codFiscale == null || codFiscale.trim() == '') &&
            (pIva == undefined || pIva == null || pIva.trim() == '') &&
            (idFattura == undefined || idFattura == null || idFattura.trim() == '') &&
            (idContratto == undefined || idContratto == null || idContratto.trim() == '') &&
            (idDestinatariAssociati == undefined || idDestinatariAssociati == null || idDestinatariAssociati.trim() == '')) {

            Ext.MessageBox.show({
                title: 'Ricerca Beneficiari',
                msg: "E' necessario indicare un valore (o parte di esso) da ricercare.",
                buttons: Ext.Msg.OK,
                closable: false,
                icon: Ext.MessageBox.ERROR
            });

        } else {
            if (Ext.getCmp('GridRicerca') != undefined)
                Ext.getCmp('risultatiRicerca').remove(Ext.getCmp('GridRicerca'));

            if (fnOnBeforeSearchCommand != undefined && fnOnBeforeSearchCommand != null)
                fnOnBeforeSearchCommand();

            var IDliquidazione = 0;
            if (liquidazione != undefined) {
                IDliquidazione = liquidazione.ID;
            }


            var grid = buildInterrogaAnagrafica(codFiscale, pIva, denominazione, codiceSIC, idContratto, idFattura, IDliquidazione, idDestinatariAssociati, fnOnStoreBeneficiariLoad, fnOnSelectBeneficiario, resultSearchGridHeight);
            Ext.getCmp('risultatiRicerca').add(grid);

            var selectionModel = Ext.getCmp('idSearchModel').getValue();
            if (selectionModel != null && selectionModel != undefined && selectionModel == 'onAutoSilentSearch')
                Ext.getCmp('risultatiRicerca').hide();
            else
                Ext.getCmp('risultatiRicerca').show();

            if (fnOnAfterSearchCommand != undefined && fnOnAfterSearchCommand != null)
                fnOnAfterSearchCommand();

            Ext.getCmp('panelSearchBeneficiari').doLayout();
        }
    });

    Ext.getCmp('risultatiRicerca').hide();
    Ext.getCmp('btnCerca').hide();
    
    Ext.getCmp('tipoRicercaLabel').disable();
    Ext.getCmp('tipoRicerca').disable();
    
    return searchBeneficiariPanel;
}

function buildInterrogaAnagrafica(CF, PI, DE, CS, IC, IF, progLiquidazione, IB, fnOnStoreBeneficiariLoad, fnOnSelectBeneficiario, resultSearchGridHeight) {

    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/InterrogaAnagrafica',
        method: 'POST',
        timeout: 10000000,
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
    });

    var reader = new Ext.data.JsonReader({
        root: 'InterrogaAnagraficaResult',
        fields: [
	       { name: 'Tipologia' },
           { name: 'DescrizioneTipologia' },
           { name: 'CodiceFiscale' },
           { name: 'Denominazione' },
           { name: 'Cognome' },
           { name: 'Nome' },
           { name: 'DataNascita' },
           { name: 'PartitaIva' },
           { name: 'ID' },
           { name: 'AltriNomi' },
           { name: 'Commissioni' },
           { name: 'ComuneNascita' },
           { name: 'NotePignoramento' },
           { name: 'Pignoramento' },
           { name: 'Sesso' },
           { name: 'Estero' },
           { name: 'ListaSedi' },
           { name: 'LegaleRappresentante' },
           { name: 'Contratto' },
           { name: 'ImportoSpettante' },
           { name: 'Fattura' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
		, reader: reader
		, sortInfo: { field: "DescrizioneTipologia", direction: "ASC" }
		, listeners: {
            'loadexception': function(proxy, options, response) {
                maskApp.hide();
                Ext.MessageBox.show({
                    title: 'Ricerca Beneficiari',
                    msg: "Errore durante l'interrogazione dell'anagrafica dei beneficiari:<br>" +
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
            var gridRicerca = Ext.getCmp('GridRicerca');
           
            if (records.length != 0) {
                var selectionModel = Ext.getCmp('idSearchModel').getValue();
                if (selectionModel != null && selectionModel != undefined && (selectionModel == 'onAutoSearch' || selectionModel == 'onAutoSilentSearch')) {
                    gridRicerca.getSelectionModel().selectRow(0);
                    gridRicerca.fireEvent('rowclick', gridRicerca, 0);
                }
            }
            if (records.length == 1) {
                gridRicerca.getSelectionModel().selectRow(0);
                gridRicerca.fireEvent('rowclick', gridRicerca, 0);
            }

            if (fnOnStoreBeneficiariLoad != undefined && fnOnStoreBeneficiariLoad != null)
                fnOnStoreBeneficiariLoad(records);

            maskApp.hide();
            gridRicerca.getView().refresh();
            },
        scope: this
    }
    });

    maskApp.show();

    var parametri = { codFiscale: CF, pIva: PI, denominazione: DE, idAnagrafica: CS, idContratto: IC, idFattura: IF, progLiq: progLiquidazione, idBeneficiari: IB };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: true,
        loadMask: true
    });

    function renderNominativo(val, p, record) {
        if (val == "F") {
            return record.data.Cognome + ' ' + record.data.Nome;
        } else {
            return record.data.Denominazione;
        }
    }

    function renderPartitaIva(val, p, record) {
        if (val == "F") {
            return record.data.CodiceFiscale;
        } else {
            return record.data.PartitaIva
        }
    }

    function renderLegaleRappresentante(val, p, record) {
        if (val == "F") {
            return "";
        } else {
            var nominativo = ""
            if (!isNullOrEmpty(record.data.LegaleRappresentante.Cognome))
                nominativo += record.data.LegaleRappresentante.Cognome;
            if (!isNullOrEmpty(record.data.LegaleRappresentante.Nome))
                nominativo += " " + record.data.LegaleRappresentante.Nome;
            return nominativo;
        }
    }
    
    var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            { header: "Importo Spettante", width: 60, dataIndex: 'ImportoSpettante', id: 'ImportoSpettante', hidden: true },
            { header: "Codice SIC", width: 60, dataIndex: 'ID', id: 'ID', sortable: true, hidden: false },
            { header: "Tipologia", width: 54, dataIndex: 'DescrizioneTipologia', id: 'DescrizioneTipologia', sortable: true },
            { header: "Nominativo", width: 178, dataIndex: 'Tipologia', renderer: renderNominativo, sortable: true },
            { header: "Partita Iva/Cod. Fiscale", width: 125, dataIndex: 'Tipologia', renderer: renderPartitaIva, sortable: true, locked: false },
            { header: "Data di Nascita", width: 85, dataIndex: 'DataNascita', sortable: true },
            { header: "Luogo di Nascita", width: 100, dataIndex: 'ComuneNascita', sortable: true, locked: false },
            { header: "Legale Rappresentante", width: 160, dataIndex: 'Tipologia', renderer: renderLegaleRappresentante, sortable: true, locked: false },         	
         	{ header: "Num. Repertorio Contratto", dataIndex: 'ID', sortable: true, hidden: false,
         	    renderer: function(value, metaData, record, rowIdx, colIdx, store) {
         	        var retValue = '';
         	        if (record.data.Contratto != undefined && record.data.Contratto != null) {
         	            if (!isNullOrEmpty(record.data.Contratto.Id)) {
         	                var contratto = getContrattoInfo(record.data.Contratto.Id, "comboContrattiId");

         	                if (contratto != undefined && contratto != null &&
         	                    contratto.Oggetto != undefined && contratto.Oggetto != null) {
         	                    //metaData.attr = 'ext:qtitle="Oggetto"';
         	                    metaData.attr += ' ext:qtip="' + (contratto.Oggetto.trim().length == 0, "<<vuoto>>", contratto.Oggetto) + '"';
         	                } else {
         	                    //metaData.attr = 'ext:qtitle="Errore"';
         	                    metaData.attr += ' ext:qtip="Oggetto del contratto non disponibile."';
         	                }

         	                retValue = record.data.Contratto.NumeroRepertorio;
         	            }
         	        }

         	        return retValue;
         	    }
         	}
         	]);

    var actionModificaAnagrafica = new Ext.Action({
        text: 'Modifica',
        tooltip: 'Modifica l\'anagrafica del beneficiario selezionato',
        handler: function() {
            Ext.each(Ext.getCmp('GridRicerca').getSelectionModel().getSelections(), function(rec) {
                editAnagrafica({ value: rec.data,
                    idAnagrafica: Ext.get('idAnagrafica').value,
                    updateFn: editInsertUpdateFn
                });
            })
        },
        iconCls: 'edit-pen'
    });

    var grid = new Ext.grid.GridPanel({
        id: 'GridRicerca',
        autoHeight: false,
        height: resultSearchGridHeight == undefined || resultSearchGridHeight == null ? 180 : resultSearchGridHeight,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        bodyStyle: 'background: #EBF3FD',
        style: 'margin-top: 10px',
        viewConfig: {
            emptyText: "Nessun beneficiario corrisponde ai criteri di ricerca inseriti.",
            deferEmptyText: false
        },
        sm: sm
    });

    grid.addListener({
        'rowclick': {
            fn: function(grid, rowIndex, event) {
                actionModificaAnagrafica.setDisabled(grid.getSelectionModel().getSelected() == null);
                
                if(fnOnSelectBeneficiario!=undefined && fnOnSelectBeneficiario!=null)
                    fnOnSelectBeneficiario(grid.getSelectionModel().getSelected());

                Ext.getCmp('idSearchModel').setValue('onSearch');
                
            }, 
            scope: this
        }
    });

    grid.on('render', function() {
        this.getView().mainBody.on('mousedown', function(e, t) {
            if (t.tagName == 'A') {
                e.stopEvent();
                t.click();
            }
        });
    }, grid);

    actionModificaAnagrafica.setDisabled(true);

    return grid;
}

function resetSearchFields() {
    Ext.getCmp('denominazione').setValue('');
    Ext.getCmp('codFiscale').setValue('');
    Ext.getCmp('pIva').setValue('');
    Ext.getCmp('codiceSIC').setValue('');
}

function getContrattiDocumentoStore(addAllContrattiLabel, fnOnLoad) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaContratti' + window.location.search,
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaContrattiResult',
        fields: [
           { name: 'Id' },
           { name: 'NumeroRepertorio' },
           { name: 'Oggetto' },
           { name: 'CodieCIG' },
           { name: 'CodieCUP' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        autoLoad: true
    });

    store.on({
        'load': {
            fn: function(store, records, options) {
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

                if (fnOnLoad != undefined && fnOnLoad != null) {
                    fnOnLoad(store, records, options);
                }
            },
            scope: this
        }
    });


    return store;
}

function buildComboContrattiDocumento(comboId, width, addAllContrattiLabel, fnOnSelect, fnOnLoad, fnGetStore) {
    var comboContratti = new Ext.form.ComboBox({
        displayField: 'NumeroRepertorio',
        valueField: 'Id',
        id: comboId,
        listWidth: width,
        width: width,
        style: 'margin-top: 0px',
        store: (fnGetStore != null && fnGetStore!=undefined) ? fnGetStore(addAllContrattiLabel, fnOnLoad) : getContrattiDocumentoStore(addAllContrattiLabel, fnOnLoad),
        forceSelection: true,
        mode: 'local',
        allowBlank: true,
        queryMode: 'local',
        triggerAction: 'all',
        emptyText: 'Selezionare un contratto...',
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><div style="text-decoration:underline;display:inline">Repertorio</div>: <b>{NumeroRepertorio}</b></br><div style="text-decoration:underline;display:inline">Oggetto</div>: {Oggetto}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    if (fnOnSelect != null && fnOnSelect != undefined) {
                        fnOnSelect(combo, value);
                    }
                }
            }
        }
    });

    return comboContratti;
}

function getFattureDocumentoStore(addAllFattureLabel, fnOnLoad, liquidazione) {    
    var store;
    if (liquidazione != undefined) {
        var proxy = new Ext.data.HttpProxy({
            url: 'ProcAmm.svc/GetListaFattureByLiquidazione' + window.location.search + '&idLiquidazione=' + liquidazione.ID,
            method: 'GET'
        });

        var reader = new Ext.data.JsonReader({
            root: 'GetListaFattureByLiquidazioneResult',
            fields: [
                 { name: 'AnagraficaInfo' },
                 { name: 'Contratto' },
                 { name: 'DataFatturaBeneficiario' },
                 { name: 'DescrizioneFattura' },
                 { name: 'IdDocumento' },
                 { name: 'IdImpegno' },
                 { name: 'IdLiquidazione' },
                 { name: 'IdUnivoco' },
                 { name: 'ImportoTotaleFattura' },
                 { name: 'NumeroFatturaBeneficiario' },
                 { name: 'Prog' }
            ]
        });

        var store = new Ext.data.Store({

            proxy: proxy,
            reader: reader,
            autoLoad: true
        });

        store.on({
            'load': {
                fn: function (store, records, options) {
                    FatturaRecordType = Ext.data.Record.create(['AnagraficaInfo', 'Contratto', 'DataFatturaBeneficiario', 'DescrizioneFattura', 'IdDocumento', 'IdImpegno', 'IdLiquidazione', 'IdUnivoco', 'ImportoTotaleFattura', 'NumeroFatturaBeneficiario']);
                    if (addAllFattureLabel != undefined && addAllFattureLabel != null && addAllFattureLabel && records.length > 0) {
                        var newRecordFattura = new FatturaRecordType({
                            IdUnivoco: 'all'
                            , NumeroFatturaBeneficiario: 'Tutti'
                            , DescrizioneFattura: 'Tutti le fatture associate alla liquidazione'
                        });
                    } else {
                        var newRecordFattura = new FatturaRecordType({
                            IdUnivoco: 'none'
                                , NumeroFatturaBeneficiario: 'Nessuno'
                                , DescrizioneFattura: 'Nessuna fattura da associare'
                        });
                    }

                    store.insert(0, newRecordFattura);

                    if (fnOnLoad != undefined && fnOnLoad != null) {
                        fnOnLoad(store, records, options);
                    }
                },
                scope: this
            }
        });
    }
    
    return store;
}

function buildComboFattureDocumento(comboId, width, addAllFattureLabel, fnOnSelect, fnOnLoad, fnGetStore, liquidazione) {
    var comboFatture = new Ext.form.ComboBox({
        displayField: 'NumeroFatturaBeneficiario',
        valueField: 'IdUnivoco',
        id: comboId,
        listWidth: width,
        width: width,
        style: 'margin-top: 0px',
        store: (fnGetStore != null && fnGetStore != undefined && liquidazione == undefined) ? fnGetStore(addAllFattureLabel, fnOnLoad) : getFattureDocumentoStore(addAllFattureLabel, fnOnLoad, liquidazione),
        forceSelection: true,
        mode: 'local',
        allowBlank: true,
        queryMode: 'local',
        triggerAction: 'all',
        emptyText: 'Selezionare una fattura...',
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><div style="text-decoration:underline;display:inline">N. Fattura</div>: <b>{NumeroFatturaBeneficiario}</b></br><div style="display:inline">Data</div>: {DataFatturaBeneficiario}    Importo: € {ImportoTotaleFattura}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    if (fnOnSelect != null && fnOnSelect != undefined) {
                        fnOnSelect(combo, value);
                    }
                }
            }
        }
    });

    return comboFatture;
}

function getDestinatariAssociatiDocumentoStore(fnOnLoad) {
    var store = new Ext.data.SimpleStore({
        fields: ['Descrizione', 'Id'],
        data: [['Tutti', 'all'] 
               //,['Solo quelli con operazioni contabili associate', 'onlyOp']
               //,['Solo quelli senza operazioni contabili associate', 'onlyNotOp']
               ],
        autoLoad: true
    });

    return store;
}

function buildComboDestinatariAssociatiDocumento(fnOnSelect, fnOnLoad) {   
    var comboDestinatariAssociati = new Ext.form.ComboBox({
        displayField: 'Descrizione',
        valueField: 'Id',
        id: 'comboDestinatariAssociatiId',
        listWidth: 442,
        width: 442,
        style: 'margin-top: 0px',
        store: getDestinatariAssociatiDocumentoStore(fnOnLoad),
        forceSelection: true,
        mode: 'local',
        allowBlank: true,
        queryMode: 'local',
        triggerAction: 'all',
        emptyText: "Selezionare una tipologia di destinatari dell'atto...",
        listeners: {
            select: {
                fn: function(combo, value) {
                    if (fnOnSelect != null && fnOnSelect != undefined) {
                        fnOnSelect(combo, value);
                    }
                }
            }
        }
    });

    return comboDestinatariAssociati;
}


function getBeneficiariCronologiaStore(fnOnLoad) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaBeneficiariCronologia',
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaBeneficiariCronologiaResult',
        fields: [
           { name: 'IdBeneficiario' },
           { name: 'IdSede' },
           { name: 'IdTipoPagamento' },
           { name: 'IdContoCorrente' },
           { name: 'FlagPersonaFisica' },
           { name: 'Nominativo' },
           { name: 'CodFiscPIva' },
           { name: 'DataNasc' },
           { name: 'LuogoNasc' },
           { name: 'LegaleRappresentante' },
           { name: 'DescrSede' },
           { name: 'DescrModPagamento' },
           { name: 'DescrDatiBancari' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        autoLoad: true
    });

    store.on({
        'load': {
            fn: function(store, records, options) {
                fnOnLoad(records);
            },
            scope: this
        }
    });


    return store;
}

function initIdResultFields(selectionModel, idAnagrafica, idSede, idTipoPagamento, idConto) {
    Ext.getCmp('idSearchModel').setValue(selectionModel);
    Ext.getCmp('idResultAnagrafica').setValue(idAnagrafica);
    Ext.getCmp('idResultSede').setValue(idSede);
    Ext.getCmp('idResultTipoPagamento').setValue(idTipoPagamento);
    Ext.getCmp('idResultConto').setValue(idConto);
}

function fireSearchBySICCode(SICCodeToSearch, synchronizeSearchField) {
    if (synchronizeSearchField)
        setSearchFieldToShow(searchFields.codiceSIC, true);
    
    Ext.getCmp('codiceSIC').setValue(SICCodeToSearch);
    Ext.getCmp('btnCerca').fireEvent('click');
}

function preFillAllResultFields(silentSearch, idAnagrafica, idSede, idTipoPagamento, idContoCorrente) {
    var retValue = false;

    if (!isNullOrEmpty(idAnagrafica)) {
        initIdResultFields(silentSearch ? 'onAutoSilentSearch' : 'onAutoSearch', idAnagrafica, idSede, idTipoPagamento, idContoCorrente);
        fireSearchBySICCode(idAnagrafica, !silentSearch);

        retValue = true;
    }

    return retValue;
}

function updateGridRicercaStore(record) {
    var records = Ext.getCmp('GridRicerca').getStore().getRange();
    for (var i = 0; i < records.length; i++) {
        if (records[i].data.ID == record.ID) {
            records[i].data = record;
            break;
        }
    }
}

function updateGrigliaRicerca(objectData, record, insertMode) {
    updateGridRicercaStore(record);
    Ext.getCmp('GridRicerca').getView().refresh();
}
