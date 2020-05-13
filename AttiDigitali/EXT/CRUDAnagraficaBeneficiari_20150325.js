var objectTypes = { "anagrafica": 0, "sede": 1, "datiBancari": 2, "datiContoCorrente": 3 };
var insertModes = { "editObject": 0, "newObject": 1 };


function buildInfoPanel(msgText, infoPanelId) {
    var infoPanel = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        id: infoPanelId,
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;',
        border: false,
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        layoutConfig: {
            columns: 1
        },
        title: "",
        items: [{
            xtype: 'label',
            id: infoPanelId + 'Msg',
            style: 'font-weight:bold',
            text: msgText
    }]
    });

    return infoPanel;
}

function disableDatiBancariFields(disable) {
    Ext.getCmp("NomeBanca").setDisabled(disable);
    Ext.getCmp("Abi").setDisabled(disable);
    Ext.getCmp("Cab").setDisabled(disable);
    Ext.getCmp("Cin").setDisabled(disable);
    Ext.getCmp("Iban").setDisabled(disable);
    Ext.getCmp("ContoCorrente").setDisabled(disable);
    Ext.getCmp("IndirizzoAgenzia").setDisabled(disable);
    Ext.getCmp("ComuneAgenzia").setDisabled(disable);
    Ext.getCmp("CAPAgenzia").setDisabled(disable);
    Ext.getCmp("ProvinciaAgenzia").setDisabled(disable);
    Ext.getCmp("ModalitaPrincipale").setDisabled(disable);

    if (disable) {
        Ext.getCmp("ContoCorrente").setValue(undefined);
        Ext.getCmp("Iban").setValue(undefined);
    }
}

function fillDatiBancariFields(datiBancari, objectType) {    
    if (objectType == objectTypes.datiBancari)
        Ext.getCmp("Iban").setValue(datiBancari.Iban);
    else if (objectType == objectTypes.datiContoCorrente)
        Ext.getCmp("ContoCorrente").setValue(datiBancari.ContoCorrente);

    Ext.getCmp("ModalitaPrincipale").setValue(datiBancari.ModalitaPrincipale);
}

function getDatiBancariFields() {    
    var datiBancari = new Object;

    datiBancari.Iban = Ext.getCmp("Iban").getValue();
    datiBancari.ContoCorrente = Ext.getCmp("ContoCorrente").getValue();

    return datiBancari;
}

function showDatiPagamentoFields(obbligoIBAN, obbligoCC, istitutoRiferimento) {
    if (obbligoIBAN) {
        Ext.getCmp("ContoCorrente").hide();
        Ext.getCmp("ContoCorrente").allowBlank = true;
        Ext.getCmp("ContoCorrenteLabel").hide();
        Ext.getCmp("ContoCorrente").setValue(undefined);
        Ext.getCmp("Iban").show();
        Ext.getCmp("IbanLabel").show();
        Ext.getCmp("IbanInfo").show();
        Ext.getCmp("Iban").allowBlank = false;
        Ext.getCmp("NomeBanca").setValue(undefined);
    } else if (obbligoCC) {
        Ext.getCmp("ContoCorrente").show();
        Ext.getCmp("ContoCorrente").allowBlank = false;
        Ext.getCmp("ContoCorrenteLabel").show();
        Ext.getCmp("Iban").hide();
        Ext.getCmp("IbanInfo").hide();
        Ext.getCmp("Iban").allowBlank = true;
        Ext.getCmp("IbanLabel").hide();
        Ext.getCmp("Iban").setValue(undefined);
        Ext.getCmp("NomeBanca").setValue(!isNullOrEmpty(istitutoRiferimento) ? istitutoRiferimento : undefined);
    }
}

function enableAndShowDatiBancariTabPanel(insertMode) {
    var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamento');

    var value = comboModalitaPagamento.getValue();
    var record = comboModalitaPagamento.findRecord(comboModalitaPagamento.valueField || comboModalitaPagamento.displayField, value);

    Ext.getCmp("IdModalitaPagamento").setValue(record.data.Id);

    if (insertMode == insertModes.editObject || ((record.data.ObbligoIBAN == false) && (record.data.ObbligoCC == false))) {
        disableDatiBancariFields(true);

        Ext.getCmp("tab_ana_cc").disable();
        Ext.getCmp('tabs').hideTabStripItem('tab_ana_cc');
    } else {
        disableDatiBancariFields(false);

        showDatiPagamentoFields(record.data.ObbligoIBAN, record.data.ObbligoCC, record.data.IstitutoRiferimento);

        Ext.getCmp("tab_ana_cc").enable();
        Ext.getCmp('tabs').unhideTabStripItem('tab_ana_cc');
    }
}

function buildcomboModalitaPagamento(id, insertMode) {
    var maskPag;
    maskPag = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetTipologiePagamentoSic',
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetTipologiePagamentoSicResult',
        fields: [
       { name: 'Id' },
       { name: 'Descrizione' },
       { name: 'ObbligoCC' },
       { name: 'Preferiti' },
       { name: 'OrdineApparizione' },
       { name: 'ObbligoCC' },
       { name: 'ObbligoIBAN' },
       { name: 'IstitutoRiferimento' }
       ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        autoLoad: true
    });

    store.setDefaultSort("OrdineApparizione", "ASC");

    var labelModalitaPagamento = new Ext.form.Label({
        text: 'Modalità di Pagamento* ',
        id: 'labelModalitaPagamento'        
    });

    var comboModalitaPagamento = new Ext.form.ComboBox({
        fieldLabel: '',
        displayField: 'Descrizione',
        valueField: 'Id',
        name: 'comboModalitaPagamento',
        id: 'comboModalitaPagamento',
        listWidth: 520,
        store: store,
        readOnly: true,
        mode: 'local',
        allowBlank: false,
        blankText: 'Campo Obbligatorio',
        queryMode: 'local',
        width: 520,
        triggerAction: 'all',
        emptyText: 'Seleziona Modalità di Pagamento ...'
    });

    store.on({
        'load': {
            fn: function(store, records, options) {
                maskPag.hide();
                if (!isNullOrEmpty(id)) {
                    comboModalitaPagamento.setValue(id);
                    enableAndShowDatiBancariTabPanel(insertMode);
                }
            },
            scope: this
        }
    });


    comboModalitaPagamento.on('select',
    function(record, index) {
        enableAndShowDatiBancariTabPanel(insertMode);
    }
);

    if (Ext.getCmp('panelModalita') != undefined) {
        Ext.getCmp('panelModalita').destroy();
    }

    var panelModalita = new Ext.Panel({
        xtype: "panel",
        title: "",
        id: 'panelModalita',
        width: 700,
        border: false,
        buttonAlign: "center",
        autoHeight: true,
        style: 'margin-left: 22px',
        items: [
	    labelModalitaPagamento,	   
	    comboModalitaPagamento
	 ]
    });

    return panelModalita;
}

function disableCommonGFFields(disable) {
    if (disable)
        Ext.getCmp("Pignoramento").disable();
    else
        Ext.getCmp("Pignoramento").enable();
    Ext.getCmp("NotaPignoramento").setDisabled(disable);
    Ext.getCmp("Commissioni").setDisabled(disable);
    Ext.getCmp("CodiceFiscale").setDisabled(disable);
}

function fillCommonGFFields(anagrafica) {
    Ext.getCmp("Pignoramento").setValue(anagrafica.Pignoramento);
    Ext.getCmp("NotaPignoramento").setValue(anagrafica.NotePignoramento);
    Ext.getCmp("Commissioni").setValue(anagrafica.Commissioni);
    Ext.getCmp("CodiceFiscale").setValue(anagrafica.CodiceFiscale);
}

function getCommonGFFields() {
    var commonData = new Object;

    commonData.Pignoramento = Ext.getCmp("Pignoramento").getValue();
    commonData.NotePignoramento = Ext.getCmp("NotaPignoramento").getValue();
    commonData.Commissioni = Ext.getCmp("Commissioni").getValue();
    commonData.CodiceFiscale = Ext.getCmp("CodiceFiscale").getValue();

    return commonData;
}

function buildPanelComuneGF() {

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

    var panelComuneGF = new Ext.Panel({
        xtype: "panel",
        title: "",
        id: 'panelComuneGF',
        layout: 'table',
        width: 700,
        border: false,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 10px;' : 'padding:10px 10px 10px 10px;',
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        defaults: {
            width: 700,
            maxLength: 300
        },
        layoutConfig: {
            columns: 2
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        buttonAlign: "center",
        items: [{
            xtype: 'label',
            text: 'Pignoramento  ',
            hidden: true
        }, {
            xtype: 'checkbox',
            fieldLabel: 'Pignoramento',
            name: 'Pignoramento',
            id: 'Pignoramento',
            width: 200,
            hidden: true
        }, {
            xtype: 'label',
            text: 'NotaPignoramento',
            hidden: true
        }, {
            xtype: 'textarea',
            fieldLabel: 'NotaPignoramento',
            name: 'NotaPignoramento',
            id: 'NotaPignoramento',
            width: 200,
            hidden: true
        },
            {
                xtype: 'label',
                style: 'padding-right:12px',
                text: 'Esente Commissioni  '
            }, {
                xtype: 'checkbox',
                fieldLabel: 'Commissioni',
                name: 'Commissioni',
                id: 'Commissioni',
                width: 300
            }, {
                xtype: 'label',
                text: 'Codice Fiscale* '
            }, {
                xtype: 'textfield',
                fieldLabel: 'CodiceFiscale',
                name: 'CodiceFiscale',
                id: 'CodiceFiscale',
                maxLength: 16,
                minLength: 16,
                width: 300,
                allowBlank: false,
                blankText: 'Campo Obbligatorio',
                minLengthText: 'La lunghezza deve essere di 16',
                maxLengthText: 'La lunghezza deve essere di 16',
                validator: function(valu) {
                    var pattern = /[a-zA-Z]{6}\d\d[a-zA-Z]\d\d[a-zA-Z]\d\d\d[a-zA-Z]/
                    var stringa = valu.trim()
                    var result = stringa.search(pattern)
                    return result > -1;
                },
                invalidText: 'Il codice Fiscale inserito non è corretto'
            }
]
    });

    return panelComuneGF;
}

function disablePersonaGiuridicaFields(disable) {
    Ext.getCmp("Denominazione").setDisabled(disable);
    Ext.getCmp("PartitaIva").setDisabled(disable);
    Ext.getCmp("CodiceFiscaleLR").setDisabled(disable);
    Ext.getCmp("CognomeLR").setDisabled(disable);
    Ext.getCmp("NomeLR").setDisabled(disable);
    Ext.getCmp("SessoLR").setDisabled(disable);
    Ext.getCmp("ComuneNSLR").setDisabled(disable);
    Ext.getCmp("DataNascitaLR").setDisabled(disable);
    Ext.getCmp("IndirizzoLR").setDisabled(disable);
    Ext.getCmp("ComuneResLR").setDisabled(disable);
    Ext.getCmp("CAPRESLR").setDisabled(disable);
    Ext.getCmp("Estero").setDisabled(disable);
}

function fillPersonaGiuridicaFields(anagrafica) {
    Ext.getCmp("Denominazione").setValue(anagrafica.Denominazione);
    Ext.getCmp("PartitaIva").setValue(anagrafica.PartitaIva);
    Ext.getCmp("Estero").setValue(anagrafica.Estero);
}

function getPersonaGiuridicaFields() {
    var anagrafica = new Object;

    anagrafica.Tipologia = "G";
    anagrafica.Denominazione = Ext.getCmp("Denominazione").getValue();
    anagrafica.PartitaIva = Ext.getCmp("PartitaIva").getValue();
    anagrafica.Estero = Ext.getCmp("Estero").getValue();
    anagrafica.CodiceFiscaleLR = Ext.getCmp("CodiceFiscaleLR").getValue();
    anagrafica.CognomeLR = Ext.getCmp("CognomeLR").getValue();
    anagrafica.NomeLR = Ext.getCmp("NomeLR").getValue();
    anagrafica.SessoLR = Ext.getCmp("SessoLR").getValue();
    anagrafica.ComuneNSLR = Ext.getCmp("ComuneNSLR").getValue();
    anagrafica.DataNascitaLR = Ext.getCmp("DataNascitaLR").getValue();
    anagrafica.IndirizzoLR = Ext.getCmp("IndirizzoLR").getValue();
    anagrafica.ComuneResLR = Ext.getCmp("ComuneResLR").getValue();
    anagrafica.CAPRESLR = Ext.getCmp("CAPRESLR").getValue();

    return anagrafica;
}

function buildPanelGiuridica() {
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

    var storeSesso = new Ext.data.SimpleStore({
        fields: ['value', 'description'],
        data: [['M', 'Maschio'], ['F', 'Femmina']],
        autoLoad: true
    });

    var panelGiuridica = new Ext.Panel({
        id: 'panelGiuridica',
        width: 700,
        border: false,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:8px 20px 10px 10px;' : 'padding:8px 20px 10px 10px;',
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        defaults: {
            width: 700,
            maxLength: 300
        },
        buttonAlign: "center",
        items: [
            { xtype: 'panel',
                layout: 'table',
                border: false,
                layoutConfig: {
                    columns: 2
                      , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
                },
                items: [
            {
                xtype: 'label',
                text: 'Estero'
            }, {
                xtype: 'checkbox',
                fieldLabel: 'Estero',
                name: 'Estero',
                id: 'Estero',
                width: 200,
                listeners: {
                    check: function(checkbox, checked) {
                        Ext.getCmp('labelPartitaIva').setText(checked ? "Partita Iva/Cod.Fiscale" : "Partita Iva/Cod.Fiscale*");
                        Ext.getCmp('PartitaIva').allowBlank = checked;
                        Ext.getCmp('PartitaIva').minLength = checked ? 0 : 11;
                        if (checked)
                            Ext.getCmp('PartitaIva').validate();
                    }
                }
            },
             {
                 xtype: 'label',
                 style: 'padding-right: 47px',
                 text: 'Denominazione* '
             }, {
                 xtype: 'textfield',
                 fieldLabel: 'Denominazione',
                 name: 'Denominazione',
                 id: 'Denominazione',
                 width: 300,
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio'
             }, {
                 xtype: 'label',
                 id: 'labelPartitaIva',                 
                 text: 'Partita Iva/Cod.Fiscale* '
             }, {
                 xtype: 'textfield',
                 fieldLabel: 'PartitaIva',
                 name: 'PartitaIva',
                 id: 'PartitaIva',
                 width: 300,
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio',
                 maxLength: 11,
                 minLength: 11,
                 minLengthText: 'La lunghezza minima è 11 caratteri',
                 maxLengthText: 'La lunghezza massima è 11 caratteri',
                 validator: function(valu) {
                     if (!Ext.getCmp('PartitaIva').allowBlank) {
                         var pattern = '^[0-9]{11}$'
                         var stringa = valu.trim()
                         var result = stringa.search(pattern)
                         return result > -1;
                     } else
                         return true;
                 },
                 invalidText: 'La partita iva inserita non è corretta'
            }]
            },
            { xtype: 'panel',
              bodyStyle: 'margin-top:5px;margin-bottom:2px',
                width: 175,
                     items: [
	                 {
	                     xtype: 'label',
	                     style: 'font-weight:bold;padding-left:15px;padding-right:15px',
	                     text: 'LEGALE RAPPRESENTANTE'
                    }]
             },
             { xtype: 'panel',
                 layout: 'table',
                 border: false,
                 layoutConfig: {
                     columns: 2
                      , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
                 },
                 items: [                
	          {
	              xtype: 'label',
	              style: 'padding-right: 81px',
	              text: 'Cognome  '       
	          }, {
	              xtype: 'textfield',
	              fieldLabel: 'CognomeLR',
	              name: 'CognomeLR',
	              id: 'CognomeLR',
	              width: 300
	          }, {
	              xtype: 'label',
	              text: 'Nome  '
	          }, {
	              xtype: 'textfield',
	              fieldLabel: 'NomeLR',
	              name: 'NomeLR',
	              id: 'NomeLR',
	              width: 300
	          }, {
	              xtype: 'label',
	              text: 'Sesso  '
	          }, {
	              xtype: 'combo',
	              displayField: 'description',
	              valueField: 'value',
	              triggerAction: 'all',
	              selectOnFocus: true,
	              typeAhead: true,
	              mode: 'local',
	              store: storeSesso,
	              name: 'SessoLR',
	              id: 'SessoLR',
	              queryMode: 'local',
	              width: 100,
	              listWidth: 100
	          }, {
	              xtype: 'label',
	              text: 'Comune di Nascita  '
	          }, {
	              xtype: 'combo',
	              name: 'ComuneNSLR',
	              id: 'ComuneNSLR',
	              displayField: 'Descrizione',
	              triggerAction: 'all',
	              selectOnFocus: true,
	              typeAhead: true,
	              minChars: 3,
	              listWidth: 300,
	              width: 300,
	              mode: 'remote',
	              store: new Ext.data.JsonStore({
	                  url: 'ProcAmm.svc/InterrogaComune',
	                  method: 'GET',
	                  readOnly: false,
	                  root: 'InterrogaComuneResult',
	                  fields: [{ name: 'Cap' }, { name: 'Descrizione' }, { name: 'Provincia' }, { name: 'ID'}],
	                  autoLoad: true,
	                  forceSelection: true,
	                  valueNotFoundText: 'SELEZIONARE UN COMUNE'
	              })
	          }, {
	              xtype: 'label',
	              text: 'Data di Nascita  '
	          }, {
	              xtype: 'datefield',
	              fieldLabel: 'DataNascitaLR',
	              name: 'DataNascitaLR',
	              id: 'DataNascitaLR',
	              format: 'd-m-Y',
	              altFormats: 'd/m/Y',
	              width: 100
	          },
	         {
	             xtype: 'label',
	             text: 'Codice Fiscale  '
	         }, {
	             xtype: 'textfield',
	             fieldLabel: 'CodiceFiscaleLR',
	             name: 'CodiceFiscaleLR',
	             id: 'CodiceFiscaleLR',
	             width: 300,
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
	             invalidText: 'Il codice Fiscale inserito non è corretto'
	         },
	         {
	             xtype: 'label',
	             text: 'Indirizzo Residenza  '
	         }, {
	             xtype: 'textfield',
	             fieldLabel: 'IndirizzoLR',
	             name: 'IndirizzoLR',
	             id: 'IndirizzoLR',
	             width: 300
	         }, {
	             xtype: 'label',
	             text: 'Comune Residenza  '
	         }, {
	             xtype: 'combo',
	             name: 'ComuneResLR',
	             id: 'ComuneResLR',
	             displayField: 'Descrizione',
	             triggerAction: 'all',
	             selectOnFocus: true,
	             typeAhead: true,
	             minChars: 3,
	             listWidth: 300,
	             width: 300,
	             mode: 'remote',
	             store: new Ext.data.JsonStore({
	                 url: 'ProcAmm.svc/InterrogaComune',
	                 method: 'GET',
	                 readOnly: false,
	                 root: 'InterrogaComuneResult',
	                 fields: [{ name: 'Cap' }, { name: 'Descrizione' }, { name: 'Provincia' }, { name: 'ID'}],
	                 autoLoad: true,
	                 forceSelection: true,
	                 valueNotFoundText: 'SELEZIONARE UN COMUNE'
	             })
	         }, {
	             xtype: 'label',
	             text: 'CAP Residenza  '
	         }, {
	             xtype: 'textfield',
	             fieldLabel: 'CAPRESLR',
	             name: 'CAPRESLR',
	             id: 'CAPRESLR',
	             width: 100,
	             readOnly: true
            }]
        }]
    });

    Ext.getCmp('ComuneResLR').on('select', function(record, index) {
        Ext.getCmp('CAPRESLR').setValue(Ext.getCmp('ComuneResLR').store.data.get(Ext.getCmp('ComuneResLR').selectedIndex).data.Cap);
    });

    return panelGiuridica;
}

function disablePersonaFisicaFields(disable) {
    disableCommonGFFields(disable);

    Ext.getCmp("Cognome").setDisabled(disable);
    Ext.getCmp("Nome").setDisabled(disable);
    Ext.getCmp("AltriNomi").setDisabled(disable);
    Ext.getCmp("Sesso").setDisabled(disable);
    Ext.getCmp("ComuneNS").setDisabled(disable);
    Ext.getCmp("DataNascita").setDisabled(disable);
}

function fillPersonaFisicaFields(anagrafica) {
    fillCommonGFFields(anagrafica);

    Ext.getCmp("Cognome").setValue(anagrafica.Cognome);
    Ext.getCmp("Nome").setValue(anagrafica.Nome);
    Ext.getCmp("AltriNomi").setValue(anagrafica.AltriNomi);
    Ext.getCmp("Sesso").setValue(anagrafica.Sesso);
    Ext.getCmp("ComuneNS").setValue(anagrafica.ComuneNascita);
    Ext.getCmp("DataNascita").setValue(anagrafica.DataNascita);
}

function getPersonaFisicaFields() {
    var anagrafica = new Object;

    anagrafica.Tipologia = "F";
    anagrafica.commonFields = getCommonGFFields();
    anagrafica.Cognome = Ext.getCmp("Cognome").getValue();
    anagrafica.Nome = Ext.getCmp("Nome").getValue();
    anagrafica.AltriNomi = Ext.getCmp("AltriNomi").getValue();
    anagrafica.Sesso = Ext.getCmp("Sesso").getValue();
    anagrafica.ComuneNascita = Ext.getCmp("ComuneNS").getValue();
    anagrafica.DataNascita = Ext.getCmp("DataNascita").getValue();

    return anagrafica;
}

function buildPanelFisica() {

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

    var storeSesso = new Ext.data.SimpleStore({
        fields: ['value', 'description'],
        data: [['M', 'Maschio'], ['F', 'Femmina']]
    });

    var panelFisica = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        width: 700,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 20px 10px 10px;' : 'padding:10px 20px 10px 10px;',
        border: false,
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        defaults: {
            width: 700,
            maxLength: 300
        },
        layoutConfig: {
            columns: 2
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        title: "",
        id: 'panelFisica',
        buttonAlign: "center",
        items: [{
            xtype: 'label',
            style: 'padding-right:54px',
            text: 'Cognome* '
        }, {
            xtype: 'textfield',
            fieldLabel: 'Cognome',
            name: 'Cognome',
            id: 'Cognome',
            width: 300,
            allowBlank: false,
            blankText: 'Campo Obbligatorio'
        }, {
            xtype: 'label',
            text: 'Nome* '
        },
             {
                 xtype: 'textfield',
                 fieldLabel: 'Nome',
                 name: 'Nome',
                 id: 'Nome',
                 width: 300,
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio'

             }, {
                 xtype: 'label',
                 text: 'Altri Nomi '
             },
             {
                 xtype: 'textfield',
                 fieldLabel: 'Altri Nomi',
                 name: 'AltriNomi',
                 id: 'AltriNomi',
                 width: 300

             }, {
                 xtype: 'label',
                 text: 'Sesso* '
             }, {
                 xtype: 'combo',
                 displayField: 'description',
                 valueField: 'value',
                 triggerAction: 'all',
                 selectOnFocus: true,
                 typeAhead: true,
                 mode: 'local',
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio',
                 store: storeSesso,
                 queryMode: 'local',
                 name: 'Sesso',
                 id: 'Sesso',
                 listWidth: 150,
                 width: 150
             },
              {
                  xtype: 'label',
                  text: 'Comune di Nascita* '
              }, {
                  xtype: 'combo',
                  name: 'ComuneNS',
                  id: 'ComuneNS',
                  displayField: 'Descrizione',
                  triggerAction: 'all',
                  selectOnFocus: true,
                  typeAhead: true,
                  minChars: 3,
                  listWidth: 300,
                  width: 300,
                  mode: 'remote',
                  store: new Ext.data.JsonStore({
                      url: 'ProcAmm.svc/InterrogaComune',
                      method: 'GET',
                      readOnly: false,
                      root: 'InterrogaComuneResult',
                      fields: [{ name: 'Cap' }, { name: 'Descrizione' }, { name: 'Provincia' }, { name: 'ID'}],
                      autoLoad: true,
                      forceSelection: true,
                      valueNotFoundText: 'SELEZIONARE UN COMUNE'
                  }),
                  allowBlank: false,
                  blankText: 'Campo Obbligatorio'
              },
             {
                 xtype: 'label',
                 text: 'Data di Nascita* '
             }, {
                 xtype: 'datefield',
                 fieldLabel: 'DataNascita',
                 name: 'DataNascita',
                 id: 'DataNascita',
                 format: 'd-m-Y',
                 altFormats: 'd/m/Y',
                 width: 150,
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio'
             }
	        ]
    });

    return panelFisica;
}


function disableSedeFields(disable) {
    Ext.getCmp("IndirizzoSede").setDisabled(disable);
    Ext.getCmp("Telefono").setDisabled(disable);
    Ext.getCmp("EMail").setDisabled(disable);
    Ext.getCmp("Fax").setDisabled(disable);
    Ext.getCmp("ComuneSede").setDisabled(disable);
    Ext.getCmp("CAP").setDisabled(disable);
    Ext.getCmp("Bollo").setDisabled(disable);
    Ext.getCmp("IdModalitaPagamento").setDisabled(disable);

    disableModalitaPagamentoField(disable);
}

function fillSedeFields(sede) {
    Ext.getCmp("IndirizzoSede").setValue(sede.Indirizzo);
    Ext.getCmp("Telefono").setValue(sede.Telefono);
    Ext.getCmp("EMail").setValue(sede.Email);
    Ext.getCmp("Fax").setValue(sede.Fax);
    Ext.getCmp("ComuneSede").setValue(sede.Comune);
    Ext.getCmp("CAP").setValue(sede.CapComune);
    Ext.getCmp("Bollo").setValue(sede.Bollo);
    Ext.getCmp("IdModalitaPagamento").setValue(sede.IdModalitaPagamento);

    fillModalitaPagamentoField(sede);
}

function getSedeFields() {
    var sede = new Object;

    sede.Indirizzo = Ext.getCmp("IndirizzoSede").getValue();
    sede.Telefono = Ext.getCmp("Telefono").getValue();
    sede.Email = Ext.getCmp("EMail").getValue();
    sede.Fax = Ext.getCmp("Fax").getValue();
    sede.Comune = Ext.getCmp("ComuneSede").getValue();
    sede.CapComune = Ext.getCmp("CAP").getValue();
    sede.Bollo = Ext.getCmp("Bollo").getValue();

    var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamento');

    var value = comboModalitaPagamento.getValue();
    var record = comboModalitaPagamento.findRecord(comboModalitaPagamento.valueField || comboModalitaPagamento.displayField, value);

    sede.ModalitaPagamento = (record != null && record != undefined) ? record.data.Descrizione : "";

    return sede;
}

function disableModalitaPagamentoField(disable) {
    Ext.getCmp("labelModalitaPagamento").setDisabled(disable);
    Ext.getCmp("comboModalitaPagamento").setDisabled(disable);
}

function fillModalitaPagamentoField(sede) {
    //Ext.getCmp("comboModalitaPagamento").setValue(sede.IdModalitaPagamento);
}

function buildPanelAddSede() {
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

    var panelAddSede = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        border: false,
        width: 700,
        autoHeight: true,
        bodyStyle: Ext.isIE ? 'padding:17px 20px 10px 10px;' : 'padding:17px 20px 10px 10px;',
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        defaults: {
            width: 350,
            maxLength: 300
        },
        layoutConfig: {
            columns: 2
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        title: "",
        id: 'panelAddSede',
        buttonAlign: "center",
        items: [
					    {
					        xtype: 'label',
					        style: 'padding-right: 10px',
					        text: 'Indirizzo* '
					    }, {
					        xtype: 'textfield',
					        fieldLabel: 'IndirizzoSede',
					        name: 'IndirizzoSede',
					        id: 'IndirizzoSede',
					        width: 300,
					        allowBlank: false,
					        blankText: 'Campo Obbligatorio'
					    }, 
					    {
                              xtype: 'label',
                              text: 'Comune* '
                          },
                        {
                            xtype: 'combo',
                            name: 'ComuneSede',
                            id: 'ComuneSede',
                            fieldLabel: 'Comune',
                            displayField: 'Descrizione',
                            triggerAction: 'all',
                            selectOnFocus: true,
                            typeAhead: true,
                            allowBlank: false,
                            blankText: 'Campo Obbligatorio',
                            minChars: 3,
                            listWidth: 300,
                            width: 300,
                            mode: 'remote',
                            store: new Ext.data.JsonStore({
                                url: 'ProcAmm.svc/InterrogaComune',
                                method: 'GET',
                                readOnly: false,
                                root: 'InterrogaComuneResult',
                                fields: [{ name: 'Cap' }, { name: 'Descrizione' }, { name: 'Provincia' }, { name: 'ID'}],
                                autoLoad: true,
                                forceSelection: true,
                                valueNotFoundText: 'SELEZIONARE UN COMUNE'
                            })
                        },
                          {
                              xtype: 'label',
                              text: 'CAP  '
                          },
                         {
                             xtype: 'textfield',
                             fieldLabel: 'CAP',
                             name: 'CAP',
                             id: 'CAP',
                             width: 100,
                             readOnly: true
                         },					    
					    {
					        xtype: 'label',
					        text: 'Telefono  '
					    },
                         {
                             xtype: 'textfield',
                             fieldLabel: 'Telefono',
                             name: 'Telefono',
                             id: 'Telefono',
                             width: 200

                         },
                          {
                              xtype: 'label',
                              text: 'Fax  '
                          },
                         {
                             xtype: 'textfield',
                             fieldLabel: 'Fax',
                             name: 'Fax',
                             id: 'Fax',
                             width: 200

                         }, {
                             xtype: 'label',
                             text: 'Email  '
                         },
                         {
                             xtype: 'textfield',
                             fieldLabel: 'Email',
                             name: 'EMail',
                             id: 'EMail',
                             width: 300
                         }
                          , {
                             xtype: 'label',
                             text: 'Bollo  '
                         }, {
                             xtype: 'checkbox',
                             fieldLabel: 'Bollo',
                             name: 'Bollo',
                             id: 'Bollo',
                             width: 200

                         },
					    {
					        xtype: 'hidden',
					        id: 'IdModalitaPagamento'
					    }
				    ]

    });
    Ext.getCmp('ComuneSede').on('select', function(record, index) {
        Ext.getCmp('CAP').setValue(Ext.getCmp('ComuneSede').store.data.get(Ext.getCmp('ComuneSede').selectedIndex).data.Cap);
    });

    return panelAddSede;
}


function buildPanelAddDatoBancario() {
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

    var panelAddDatoBancario = new Ext.Panel({
        border: false,
        width: 700,
        bodyStyle: Ext.isIE ? 'padding:18px 20px 10px 10px;' : 'padding:18px 20px 10px 10px;',
        style: {
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        defaults: {
            width: 350,
            maxLength: 300
        },
        layoutConfig: {
            columns: 2
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        id: 'panelAddDatoBancario',
        buttonAlign: "center",
        items: [
                  {
                      xtype: 'label',
                      id: 'ContoCorrenteLabel',
                      text: 'Conto Corrente* ',
                      style: 'padding-right: 12px',
                      hidden: false
                  }, {
                      xtype: 'textfield',
                      fieldLabel: 'ContoCorrente',
                      name: 'ContoCorrente',
                      id: 'ContoCorrente',
                      width: 200,
                      allowBlank: true,
                      blankText: 'Campo Obbligatorio',
                      maxLength: 12,
                      minLength: 1,
                      minLengthText: 'La lunghezza minima è 1 caratteri',
                      maxLengthText: 'La lunghezza massima è 12 caratteri',
                      validator: function(value) {
                          var stringa = value.trim();
                          return stringa.search(/^[0-9]{1,12}$/) > -1 && !(stringa.search(/^[0]+$/) > -1);
                      },
                      invalidText: 'Il Conto Corrente inserito non è corretto',
                      hidden: false
                  }, {
                      xtype: 'label',
                      text: 'Iban* ',
                      id: 'IbanLabel',
                      style: 'padding-right: 12px',
                      hidden: false
                  }, {
                      xtype: 'textfield',
                      fieldLabel: 'Iban',
                      name: 'Iban',
                      id: 'Iban',
                      width: 200,
                      allowBlank: false,
                      blankText: 'Campo Obbligatorio',
                      maxLength: 34,
                      minLength: 5,
                      minLengthText: 'La lunghezza minima è 5 caratteri',
                      maxLengthText: 'La lunghezza massima è 34 caratteri',
                      hidden: false,
                      validator: function(value) {
                          var pattern = /[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}/
                          var iban = value.trim()
                          return (iban.search(pattern) > -1) && checkiban(iban);
                      },
                      invalidText: 'Il codice IBAN inserito non è corretto'
                  }, {
                      xtype: 'label',
                      id: 'IbanInfo',
                      html: buildInfoIBANHtml(),
                      hidden: true
                  }, {
                      xtype: 'hidden',
                      id: 'Cin'
                  }, {
                      xtype: 'hidden',
                      id: 'IndirizzoAgenzia'
                  }, {
                      xtype: 'hidden',
                      id: 'ComuneAgenzia'
                  }, {
                      xtype: 'hidden',
                      id: 'CAPAgenzia'
                  }, {
                      xtype: 'hidden',
                      id: 'ProvinciaAgenzia'
                  }, {
                      xtype: 'hidden',
                      id: 'ModalitaPrincipale'
                  }, {
                      xtype: 'hidden',
                      id: 'NomeBanca'
                  }, {
                      xtype: 'hidden',
                      id: 'Abi'
                  }, {
                      xtype: 'hidden',
                      id: 'Cab'
                  }
            ]
    });

    Ext.getCmp('ComuneAgenzia').on('select', function(record, index) {
        Ext.getCmp('CAPAgenzia').setValue(Ext.getCmp('ComuneAgenzia').store.data.get(Ext.getCmp('ComuneAgenzia').selectedIndex).data.Cap);
        Ext.getCmp('ProvinciaAgenzia').setValue(Ext.getCmp('ComuneAgenzia').store.data.get(Ext.getCmp('ComuneAgenzia').selectedIndex).data.Provincia);
    });

    return panelAddDatoBancario;
}

function buildInfoIBANHtml() {
    var infoIbanHtml = "<div style=\"margin-top:" + (Ext.isIE ? "8px" : "8px") + "\"><font color=\"#99cc00\">Come si calcola il codice IBAN Italiano</font><br/>" +
                "Il codice IBAN Italiano &egrave; composto da 27 caratteri che sono il risultato della somma delle 6 parti che servono per il suo calcolo: " +
                "<div style=\"margin-left:30px;margin-top:5px\"><ul style=\"list-style-type:disc;\">" +
                "<li>Codice Nazionale (2 lettere)</li>" +
                "<li>CIN IBAN (2 cifre) detto anche Codice di Controllo IBAN</li>" +
                "<li>CIN BBAN (1 lettera)</li>" +
                "<li>Codice ABI (5 cifre)</li>" +
                "<li>Codice CAB (5 cifre)</li>" +
                "<li>Conto Corrente (12 caratteri alfanumerici)<br /></li>" +
                "</ul></div>" +
                "<div align=\"center\" style=\"margin-left:50px;margin-top:2px\"><img border=\"0\" align=\"middle\" src=\"/Attidigitali/risorse/immagini/iban.jpg\" alt=\"calcolo del codice IBAN\" /><br />" +
                "</div></div>";

    return infoIbanHtml;
}    


function mostraPanelAnagrafica(tipoPersona, insertMode) {
    Ext.getCmp('formInsertAnagrafica').show();

    var anagraficaToShow = (tipoPersona == 'G') ? 'tab_ana_giuridica' : 'tab_ana_fisica';
    var anagraficaToHide = (tipoPersona == 'G') ? 'tab_ana_fisica' : 'tab_ana_giuridica';

    disablePersonaGiuridicaFields(tipoPersona != 'G');
    disablePersonaFisicaFields(tipoPersona == 'G');
    disableSedeFields(insertMode == insertModes.editObject);
    disableDatiBancariFields(true);

    Ext.getCmp(anagraficaToHide).disable();
    Ext.getCmp('tabs').hideTabStripItem(anagraficaToHide);

    Ext.getCmp(anagraficaToShow).enable();
    Ext.getCmp('tabs').unhideTabStripItem(anagraficaToShow);

    if (insertMode == insertModes.newObject) {
        Ext.getCmp("tab_ana_sedi").enable();
        Ext.getCmp('tabs').unhideTabStripItem('tab_ana_sedi');
    } else {
        Ext.getCmp("tab_ana_sedi").disable();
        Ext.getCmp('tabs').hideTabStripItem('tab_ana_sedi');

        Ext.getCmp("tab_ana_cc").disable();
        Ext.getCmp('tabs').hideTabStripItem('tab_ana_cc');
    }

    Ext.getCmp('formInsertAnagrafica').active = Ext.getCmp(anagraficaToShow);
    Ext.getCmp('formInsertAnagrafica').active.show();

    if (insertMode == insertModes.newObject)
        resetFormInsertAnagrafica(tipoPersona, objectTypes.anagrafica);
}

function resetFormInsertAnagrafica(tipoPersona, objectType) {
    if (objectType == objectTypes.anagrafica || objectType == objectTypes.sede) {
        Ext.getCmp("tab_ana_cc").disable();
        Ext.getCmp('tabs').hideTabStripItem('tab_ana_cc');

        Ext.getCmp('tabs').setActiveTab(objectType == objectTypes.anagrafica ? (tipoPersona == 'F' ? 0 : 1) : 2);
    } else if (objectType == objectTypes.datiBancari || objectType == objectTypes.datiContoCorrente) {
        Ext.getCmp('tabs').setActiveTab(3);
    }

    Ext.getCmp('formInsertAnagrafica').getForm().reset();

    if (objectType == objectTypes.anagrafica) {
        Ext.getCmp("TipoAnagrafica").setValue(tipoPersona == 'G' ? 'GIURIDICA' : 'FISICA');
    }
}

function InitFormInsertAnagrafica(objectType, insertMode, objectData) {
    //where objectType is
    //objectTypes.anagrafica  - anagrafica
    //objectTypes.sede  - sede
    //objectTypes.datiBancari  - contocorrente bancario
    //objectTypes.datiContoCorrente  - contocorrente (es. postale)

    // definisco la combobox per il tipo di anagrafica (persona Fisica/Giuridica)   
    var storeTipologiaPersona = new Ext.data.SimpleStore({
        fields: ['value', 'description'],
        data: [['F', 'Fisica'], ['G', 'Giuridica']]
    });

    var comboTipologiaPersona = new Ext.form.ComboBox({
        id: 'TipoAnagraficaCombo',
        fieldLabel: 'Tipologia Persona',
        store: storeTipologiaPersona,
        displayField: 'description',
        valueField: 'value',
        typeAhead: true,
        mode: 'local',
        readOnly: true,
        width: 180,
        triggerAction: 'all',
        emptyText: 'Seleziona la Tipologia di Persona...',
        selectOnFocus: true
    });

    comboTipologiaPersona.on('select', function(record, index) { mostraPanelAnagrafica(comboTipologiaPersona.store.data.get(comboTipologiaPersona.selectedIndex).data.value, insertMode); });

    var formInsertAnagrafica = new Ext.FormPanel({
        id: 'formInsertAnagrafica',
        frame: false,
        labelAlign: 'center',
        title: '',
        bodyStyle: 'margin-top:10px;background: transparent',
        border: false,
        collapsible: false,
        autoHeight: true,
        layout: 'fit',
        items: [
            {
                xtype: 'hidden',
                id: 'TipoAnagrafica'
            },
   	        {
   	            id: 'tabs',
   	            xtype: 'tabpanel',
   	            plain: true,
   	            activeTab: 0,
   	            layoutOnTabChange: true,
   	            deferredRender: false,
   	            height: 360,
   	            items: [
                { title: 'Persona Fisica', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ana_fisica' },
                { title: 'Persona Giuridica', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ana_giuridica' },
                { title: 'Sede', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ana_sedi' },
                { title: 'Dati Pagamento', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ana_cc' }
            ]
   	        }
        ]
    });

    var title = (objectType == objectTypes.anagrafica ?
                (insertMode == insertModes.newObject ? 'Nuovo ' : 'Modifica ') + 
                    (objectData != null && objectData!=undefined && objectData.objectNameLabel != null && objectData.objectNameLabel != undefined ? objectData.objectNameLabel : 'Beneficiario') : (objectType == objectTypes.sede ? (insertMode == insertModes.newObject ? 'Nuova' : 'Modifica') + ' Sede' : (insertMode == insertModes.newObject ? 'Nuovo' : 'Modifica') + (objectType == objectTypes.datiBancari ? ' Conto Bancario' : ' Conto Corrente')))

    var popupAna = new Ext.Window({
        title: title,
        width: 750,
        id: 'anaWindow',
        layout: 'fit',
        plain: true,
        autoHeight: true,
        bodyStyle: 'padding:10px',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        closable: true,
        resizable: false,
        items: [
          {
              columnWidth: 1,
              xtype: 'panel',
              layout: 'table',
              layoutConfig: {
                  columns: 2
              },
              id: 'panelTipologiaPersona',
              autoHeight: true,
              plain: true,
              bodyStyle: 'margin-top:4px; margin-bottom:4px;background:transparent',
              border: false,
              items: [{
                  xtype: 'label',
                  id: 'LabelTipoAnagrafica',
                  text: 'Persona',
                  style: 'margin-left:2px;padding-right:6px'
              },
                comboTipologiaPersona
            ]
          }
        ]
        ,
        buttons: [{
            text: 'Pulisci',
            id: 'btnResetForm',
            handler: function() {
                resetFormInsertAnagrafica(Ext.getCmp("TipoAnagrafica").getValue() == 'FISICA' ? 'F' : 'G', objectType);
            }
        }, {
            text: 'Salva',
            id: 'btnSalvaTutto',
            handler: function() {
                if (Ext.getCmp('formInsertAnagrafica').getForm().isValid()) {
                    var tipologia = Ext.getCmp("TipoAnagrafica").getValue() == 'FISICA' ? 'F' : 'G';
                    var anagrafica = tipologia == 'G' ? getPersonaGiuridicaFields() : getPersonaFisicaFields();
                    var sede = getSedeFields();
                    var conto = !Ext.getCmp("tab_ana_cc").disabled ? getDatiBancariFields() : null;
                    showSummaryBeneficiario(objectType, insertMode, objectData, anagrafica, sede, conto);
                }
                else {
                    firstInvalidField = Ext.getCmp('formInsertAnagrafica').getForm().items.find(function(f) { return !f.isValid(); });

                    if (firstInvalidField) {
                        var parentTab = firstInvalidField.findParentByType('tabpanel');
                        var parentPanel = firstInvalidField.findParentByType('panel');
                        var tabPanelIndex = mapFieldPanelToTabPanelIndex(parentPanel.getId(), Ext.getCmp("TipoAnagrafica").getValue());
                        if (tabPanelIndex != -1) {
                            Ext.getCmp(parentTab.getId()).setActiveTab(tabPanelIndex);
                        }
                    }
                    Ext.MessageBox.alert('Errore di validazione', 'Verificare che tutti i campi contengano valori validi.');
                }
            }
    }]
    });

    popupAna.add(formInsertAnagrafica);

    Ext.getCmp("tab_ana_fisica").add(buildPanelComuneGF());
    Ext.getCmp("tab_ana_fisica").add(buildPanelFisica());
    Ext.getCmp("tab_ana_giuridica").add(buildPanelGiuridica());
    Ext.getCmp("tab_ana_sedi").add(buildPanelAddSede());
    Ext.getCmp("tab_ana_sedi").add(buildcomboModalitaPagamento(objectType == objectTypes.sede && insertMode == insertModes.editObject ? objectData.value.IdModalitaPagamento : undefined, insertMode));
    Ext.getCmp("tab_ana_cc").add(buildPanelAddDatoBancario());

    popupAna.doLayout();
    popupAna.show();

    switch (objectType) {
        case objectTypes.anagrafica:
            // inserimento anagrafica intera
            showTipoAnagraficaCombo(insertMode == insertModes.newObject);

            var tipologia = storeTipologiaPersona.getAt(0).get('value');

            if (insertMode == insertModes.editObject) {
                if ((tipologia = objectData.value.Tipologia) == 'G')
                    fillPersonaGiuridicaFields(objectData.value);
                else
                    fillPersonaFisicaFields(objectData.value);

                Ext.getCmp("TipoAnagrafica").setValue(tipologia == 'G' ? 'GIURIDICA' : 'FISICA');
                Ext.getCmp('btnResetForm').hide();
            } else {
                comboTipologiaPersona.setValue(tipologia);
            }

            mostraPanelAnagrafica(tipologia, insertMode);
            break;
        case objectTypes.sede:
            //Pressione tasto aggiungi sede
            disablePersonaFisicaFields(true);
            disablePersonaGiuridicaFields(true);
            disableSedeFields(false);
            disableDatiBancariFields(true);

            showTipoAnagraficaCombo(false);

            Ext.getCmp("tab_ana_fisica").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_fisica');
            Ext.getCmp("tab_ana_giuridica").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_giuridica');
            Ext.getCmp("tab_ana_cc").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_cc');
            Ext.getCmp("tab_ana_sedi").enable();
            Ext.getCmp('tabs').unhideTabStripItem('tab_ana_sedi');

            if (insertMode == insertModes.editObject) {
                fillSedeFields(objectData.value);
                if (objectData.disableModalitaPagamento != undefined && objectData.disableModalitaPagamento != null)
                    disableModalitaPagamentoField(objectData.disableModalitaPagamento);
                Ext.getCmp('btnResetForm').hide();
            }

            Ext.getCmp('tabs').show();
            setActivePanel('tabs', 'tab_ana_sedi');
            Ext.getCmp('tabs').doLayout();
            break;
        case objectTypes.datiBancari:
        case objectTypes.datiContoCorrente:
            //Pressione tasto aggiungi conto
            disablePersonaFisicaFields(true);
            disablePersonaGiuridicaFields(true);
            disableSedeFields(true);
            disableDatiBancariFields(false);

            showTipoAnagraficaCombo(false);
            showDatiPagamentoFields(objectType == objectTypes.datiBancari, objectType == objectTypes.datiContoCorrente, objectData.istitutoRiferimento);

            Ext.getCmp("tab_ana_fisica").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_fisica');
            Ext.getCmp("tab_ana_giuridica").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_giuridica');
            Ext.getCmp("tab_ana_sedi").disable();
            Ext.getCmp('tabs').hideTabStripItem('tab_ana_sedi');
            Ext.getCmp("tab_ana_cc").enable();
            Ext.getCmp('tabs').unhideTabStripItem('tab_ana_cc');

            if (insertMode == insertModes.editObject) {
                fillDatiBancariFields(objectData.value, objectType);
                Ext.getCmp('btnResetForm').hide();
            }

            Ext.getCmp('tabs').show();
            setActivePanel('tabs', 'tab_ana_cc');
            Ext.getCmp('tabs').doLayout();
            break;
        default:
            alert("Unknown specified object type!");
    }

    popupAna.syncShadow();
}

function showTipoAnagraficaCombo(show) {
    if (show) {
        Ext.getCmp("panelTipologiaPersona").show();
    } else {
        Ext.getCmp("panelTipologiaPersona").hide();
    }
}

function mapFieldPanelToTabPanelIndex(panelName, tipoAnagrafica) {
    // 0 is tab_ana_fisica
    // 1 is tab_ana_giuridica
    // 2 is tab_ana_sede
    // 3 is tab_ana_cc

    var retValue = -1;

    if (panelName == 'panelComuneGF')
        retValue = (tipoAnagrafica == 'FISICA') ? 0 : 1;
    else if (panelName == 'panelAddSede' || panelName == 'panelModalita')
        retValue = 2;
    else if (panelName == 'panelAddDatoBancario')
        retValue = 3;
    else if (panelName == 'panelFisica')
        retValue = 0;
    else if (panelName == 'panelGiuridica')
        retValue = 1;

    return retValue;
}

//definisco l'azione del bottone salva beneficiario/sede/dati bancari
function btnSalvaTuttoFn(objectType, insertMode, objectData) {
    var msgTitle = "";
    var url = "";

    if (objectType == objectTypes.anagrafica) {
        //sto aggiungento/editando un beneficiario
        msgTitle = (insertMode == insertModes.newObject ? "Inserimento" : "Modifica") + " Anagrafica";
        url = (insertMode == insertModes.newObject) ? 'ProcAmm.svc/CreaAnagrafica' : 'ProcAmm.svc/UpdateAnagrafica';
        if (insertMode == insertModes.editObject)
            url += '?idAnagrafica=' + objectData.idAnagrafica;
    } else if (objectType == objectTypes.datiBancari || objectType == objectTypes.datiContoCorrente) {
        //sto aggiungendo/editando un conto corrente
        msgTitle = (insertMode == insertModes.newObject ? "Inserimento" : "Modifica") + " Dati Pagamento";
        url = ((insertMode == insertModes.newObject) ? 'ProcAmm.svc/CreaContoBancario' : 'ProcAmm.svc/UpdateContoBancario') + '?idSede=' + objectData.idSede + '&idAnagrafica=' + objectData.idAnagrafica;
        if (insertMode == insertModes.editObject)
            url += '&idConto=' + objectData.idConto;
    } else if (objectType == objectTypes.sede) {
        //sto aggiungendo/editando una sede
        msgTitle = (insertMode == insertModes.newObject ? "Inserimento" : "Modifica") + " Sede";
        url = ((insertMode == insertModes.newObject) ? 'ProcAmm.svc/CreaSede' : 'ProcAmm.svc/UpdateSede') + '?idAnagrafica=' + objectData.idAnagrafica;
        if (insertMode == insertModes.editObject)
            url += '&idSede=' + objectData.idSede;
    } else {
        alert("Unknown specified object type!");
        return
    }

    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
    Ext.getCmp('formInsertAnagrafica').getForm().timeout = 100000000;
    Ext.getCmp('formInsertAnagrafica').getForm().submit({
        url: url,
        waitTitle: "Attendere...",
        waitMsg: 'Aggiornamento in corso ......',
        failure:
            function(result, response) {
                var lstr_messaggio = ''
                try {
                    lstr_messaggio = response.result.FaultMessage
                } catch (ex) {
                    lstr_messaggio = 'Errore Generale'
                }

                Ext.MessageBox.show({
                    title: msgTitle,
                    msg: lstr_messaggio,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                        return;
                    }
                });
            }, 
        success:
			function(result, response) {
			    var msg = 'Operazione effettuata con successo!';
			    Ext.MessageBox.show({
			        title: msgTitle,
			        msg: msg,
			        buttons: Ext.MessageBox.OK,
			        icon: Ext.MessageBox.INFO,
			        fn: function(btn) {
			            if (objectData != null && objectData != undefined && objectData.updateFn != null && objectData.updateFn != undefined)
			                objectData.updateFn(objectType, insertMode, objectData, response.result);
			            Ext.getCmp('anaWindow').close();
			        }
			    });
			} 
    });   
}

function showSummaryBeneficiario(objectType, insertMode, objectData, anagrafica, sede, conto) {
    var windowSummaryAnagrafica = new Ext.Window({
        title: 'Riepilogo ' + (objectData != null && objectData != undefined && objectData.objectNameLabel != null && objectData.objectNameLabel != undefined ? objectData.objectNameLabel : 'Beneficiario'),
        width: 700,
        autoHeight: true,
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        autoScroll: false,
        closable: true,
        resizable: false,
        buttons: [{
            text: 'Conferma',
            id: 'summary_btnConfirmForm',
            handler: function() {
                windowSummaryAnagrafica.close();
                btnSalvaTuttoFn(objectType, insertMode, objectData);
            }
        }, {
            text: 'Annulla',
            id: 'summary_btnCancelForm',
            handler: function() {
                windowSummaryAnagrafica.close();
            }
        }]
    });

    var panelSummaryBeneficiarioPersonaFisica = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "0px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
        title: (objectData != null && objectData != undefined && objectData.objectNameLabel != null && objectData.objectNameLabel != undefined ? objectData.objectNameLabel : 'Beneficiario'),
        id: 'panelSummaryBeneficiarioPersonaFisica',
        cls: 'pannelliDettaglio',
        items: [
    {
        xtype: 'label',
        text: 'Nominativo'
    }, {
        xtype: 'textfield',
        id: 'summary_nominativoId',
        style: 'opacity:.9',
        disabled: true,
        width: 400
    },
    {
        xtype: 'label',
        text: 'Altri Nomi'
    }, {
        xtype: 'textfield',
        id: 'summary_altriNomiId',
        style: 'opacity:.9',
        disabled: true,
        width: 400
    },
     {
         xtype: 'label',
         text: 'Codice Fiscale',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_codiceFiscaleId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Sesso',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_sessoId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Data di nascita',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_dataNascitaId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Comune di nascita',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_comuneNascitaId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     }, {
         xtype: 'label',
         text: 'Esente Commissioni'
     }, {
         xtype: 'textfield',
         id: 'summary_commissioniId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     }
    ]
    });

    var panelSummaryBeneficiarioPersonaGiuridica = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "0px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
        title: (objectData != null && objectData != undefined && objectData.objectNameLabel != null && objectData.objectNameLabel != undefined ? objectData.objectNameLabel : 'Beneficiario'),
        id: 'panelSummaryBeneficiarioPersonaGiuridica',
        cls: 'pannelliDettaglio',
        items: [
     {
         xtype: 'label',
         text: 'Estero',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_esteroId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
    {
        xtype: 'label',
        text: 'Denominazione'
    }, {
        xtype: 'textfield',
        id: 'summary_denominazioneId',
        style: 'opacity:.9',
        disabled: true,
        width: 400
    },
     {
         xtype: 'label',
         text: 'Partita IVA',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_partitaIvaId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Legale Rappresentante',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_legaleRappresentanteId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Codice Fiscale',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_codiceFiscaleLRId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Sesso',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_sessoLRId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     }
     ,
     {
         xtype: 'label',
         text: 'Data di nascita',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_dataNascitaLRId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Comune di nascita',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_comuneNascitaLRId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Indirizzo',
         columnWidth: .9
     }, {
         xtype: 'textfield',
         id: 'summary_indirizzoLRId',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     }
    ]
    });

    var panelSummarySede = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "10px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
        title: "Sede",
        id: 'panelSummarySede',
        cls: 'pannelliDettaglio',
        items: [
    {
        xtype: 'label',
        text: 'Indirizzo'
    }, {
        xtype: 'textfield',
        id: 'summary_viaID',
        style: 'opacity:.9',
        disabled: true,
        width: 400
    },
     {
         xtype: 'label',
         text: 'Recapiti'
     }, {
         xtype: 'textfield',
         id: 'summary_recapitiID',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'eMail'
     }, {
         xtype: 'textfield',
         id: 'summary_emailID',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Bollo'
     }, {
         xtype: 'textfield',
         id: 'summary_bolloID',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     },
     {
         xtype: 'label',
         text: 'Modilità di Pagamento'
     }, {
         xtype: 'textfield',
         id: 'summary_modalitaPagID',
         style: 'opacity:.9',
         disabled: true,
         width: 400
     }
    ]
    });

    var panelSummaryContoCorrente = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent',
        border: false,
        style: {
            "margin-top": "10px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
        title: "Dati Pagamento",
        id: 'panelSummaryContoCorrente',
        cls: 'pannelliDettaglio',
        items: [
        {
            xtype: 'label',
            text: '',
            id: 'summary_labelDatiBancari'
        }, {
            xtype: 'textfield',
            id: 'summary_ibanID',
            style: 'opacity:.9',
            disabled: true,
            width: 400
        }
    ]
    });

    var panelVuoto = new Ext.Panel({
        xtype: 'panel',
        layout: 'table',
        autoHeight: true,
        cls: 'pannelliDettaglio',
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "0px",
            "margin-bottom": "0px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
        id: 'summary_panelVuoto',
        items: [
    {
        xtype: 'hidden',
        id: 'summary_hidden1Id'
    }, {
        xtype: 'hidden',
        id: 'summary_hidden2Id'
    }]
    });

    Ext.getCmp('summary_panelVuoto').hide();

    windowSummaryAnagrafica.add(panelVuoto);

    if (objectType == objectTypes.anagrafica) {
        if (anagrafica.Tipologia == "F")
            windowSummaryAnagrafica.add(panelSummaryBeneficiarioPersonaFisica);
        else
            windowSummaryAnagrafica.add(panelSummaryBeneficiarioPersonaGiuridica);
    }

    if (objectType == objectTypes.anagrafica || objectType == objectTypes.sede)
        windowSummaryAnagrafica.add(panelSummarySede);

    if (conto != null && (!isNullOrEmpty(conto.Iban) || !isNullOrEmpty(conto.ContoCorrente)))
        windowSummaryAnagrafica.add(panelSummaryContoCorrente);

    windowSummaryAnagrafica.doLayout();
    windowSummaryAnagrafica.show();

    if (objectType == objectTypes.anagrafica)
        fillSummaryViewAnagrafica(anagrafica);

    if (objectType == objectTypes.anagrafica || objectType == objectTypes.sede)
        fillSummaryViewSede(sede);

    if (conto != null && (!isNullOrEmpty(conto.Iban) || !isNullOrEmpty(conto.ContoCorrente)))
        fillSummaryViewDatiBancari(conto);
}

function dateToDMY(date) {
    var retValue = ""
    if (!isNullOrEmpty(date)) {
        var d = date.getDate();
        var m = date.getMonth() + 1;
        var y = date.getFullYear();
        retValue = '' + (d <= 9 ? '0' + d : d) + '-' + (m <= 9 ? '0' + m : m) + '-' + y;
    }
    return retValue;
}

function fillSummaryViewAnagrafica(anagrafica) {
    if (anagrafica.Tipologia == "F") {
        Ext.getCmp("summary_codiceFiscaleId").setValue(anagrafica.commonFields.CodiceFiscale);
        Ext.getCmp("summary_sessoId").setValue(anagrafica.Sesso == "M" ? "Maschio" : (anagrafica.Sesso == "F" ? "Femmina" : ""));
        Ext.getCmp("summary_dataNascitaId").setValue(dateToDMY(anagrafica.DataNascita));
        Ext.getCmp("summary_comuneNascitaId").setValue(anagrafica.ComuneNascita);
        Ext.getCmp("summary_nominativoId").setValue(anagrafica.Cognome + " " + anagrafica.Nome);
        Ext.getCmp("summary_altriNomiId").setValue(anagrafica.AltriNomi);
        Ext.getCmp("summary_commissioniId").setValue(anagrafica.commonFields.Commissioni ? "Si" : "No");
    } else {
        Ext.getCmp("summary_denominazioneId").setValue(anagrafica.Denominazione);
        Ext.getCmp("summary_partitaIvaId").setValue(anagrafica.PartitaIva);
        Ext.getCmp("summary_esteroId").setValue(anagrafica.Estero ? "Si" : "No");
        Ext.getCmp("summary_codiceFiscaleLRId").setValue(anagrafica.CodiceFiscaleLR);
        Ext.getCmp("summary_legaleRappresentanteId").setValue(anagrafica.CognomeLR + " " + anagrafica.NomeLR);
        Ext.getCmp("summary_sessoLRId").setValue(anagrafica.SessoLR == "M" ? "Maschio" : (anagrafica.SessoLR == "F" ? "Femmina" : ""));
        Ext.getCmp("summary_comuneNascitaLRId").setValue(anagrafica.ComuneNSLR);
        Ext.getCmp("summary_dataNascitaLRId").setValue(dateToDMY(anagrafica.DataNascitaLR));

        var indirizzoLR = "";
        if (!isNullOrEmpty(anagrafica.IndirizzoLR))
            indirizzoLR += anagrafica.IndirizzoLR;
        if (!isNullOrEmpty(anagrafica.ComuneResLR))
            indirizzoLR += (indirizzoLR != "" ? " - " : "") + anagrafica.CAPRESLR + " " + anagrafica.ComuneResLR;

        Ext.getCmp("summary_indirizzoLRId").setValue(indirizzoLR);
    }
}

function fillSummaryViewSede(sede) {
    var indirizzo = "";
    if (!isNullOrEmpty(sede.Indirizzo))
        indirizzo += sede.Indirizzo;
    if (!isNullOrEmpty(sede.Comune))
        indirizzo += (indirizzo != "" ? " - " : "") + sede.CapComune + " " + sede.Comune;

    Ext.getCmp("summary_viaID").setValue(indirizzo);

    var recapiti = ""
    if (!isNullOrEmpty(sede.Telefono))
        recapiti += "Telefono " + sede.Telefono;
    if (!isNullOrEmpty(sede.Fax))
        recapiti += (recapiti != "" ? " - " : "") + "Fax " + sede.Fax;

    Ext.getCmp("summary_recapitiID").setValue(recapiti);
    Ext.getCmp("summary_emailID").setValue(sede.Email);
    Ext.getCmp("summary_bolloID").setValue(sede.Bollo ? "Si" : "No");
    Ext.getCmp("summary_modalitaPagID").setValue(sede.ModalitaPagamento);
}

function fillSummaryViewDatiBancari(conto) {
    if (conto != null) {
        if (!isNullOrEmpty(conto.Iban)) {
            Ext.getCmp("summary_ibanID").setValue(conto.Iban);
            Ext.getCmp("summary_labelDatiBancari").setText("IBAN");
        } else if (!isNullOrEmpty(conto.ContoCorrente)) {
            Ext.getCmp("summary_ibanID").setValue(conto.ContoCorrente);
            Ext.getCmp("summary_labelDatiBancari").setText("Conto Corrente");
        }
    }
}

function loadAnagrafica(objectData, fnUpgrade, insertMode, result) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
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
   { name: 'Estero' },
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
        proxy: proxy,
        reader: reader
    });

    maskApp.show();

    store.on({
        'load': {
            fn: function(store, records, options) {
                if (fnUpgrade != null && fnUpgrade != undefined)
                    fnUpgrade(objectData, records[0].data, insertMode, result);
                maskApp.hide();
            },
            scope: this
        }
    });

    var CF = "";
    var PI = "";
    var DE = "";
    var ID = objectData.idAnagrafica;   
    var IC = "";
    var IF = "";
    var progLiquidazione = 0; 
    var IB = "";
    


    var parametri = { codFiscale: CF, pIva: PI, denominazione: DE, idAnagrafica: ID, idContratto: IC, idFattura: IF, progLiq: progLiquidazione, idBeneficiari: IB };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        //how to do when exeception occurs ?!?!?
    }
}
