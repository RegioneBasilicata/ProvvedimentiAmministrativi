var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

var storeSiNo = new Ext.data.SimpleStore({
					    fields: ['value', 'description'],
					    data: [[false, 'No'], [true, 'Si']]
					});
function renderBool(val) {
        var checkedImg = '/Attidigitali/ext/resources/images/default/menu/checked.gif';
        var uncheckedImg = '/Attidigitali/ext/resources/images/default/menu/unchecked.gif';
        var cb = ''
            + '<div style="text-align:center;height:13px;overflow:visible">'
            + '<img style="vertical-align:-3px" src="'
            + (val ? checkedImg : uncheckedImg)
            + '"'
            + ' />'
            + '</div>'
        ;
        return cb;
    }  
//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
    var actionRegistraLiq = new Ext.Action({
        text: 'Registra',
        tooltip: 'Registra la liquidazione selezionato',
        handler: function() {
            if (Ext.getCmp('GridBeneficiariRag') != null && Ext.getCmp('GridBeneficiariRag') != undefined &&
                Ext.getCmp('GridBeneficiariRag').getStore().getRange().length == 0) {
                GeneraLiquidazione();
            } else {
                //Ext.MessageBox.show({
                //    title: "Registra Liquidazione",
                //    msg: "Attenzione: confermando l'operazione non sarà più possibile aggiornare i dati relativi ai beneficiari associati alla liquidazione selezionata.",
                //    buttons: Ext.MessageBox.OKCANCEL,
                //    icon: Ext.MessageBox.WARNING,
                //    fn: function(btn) {
                //        if (btn == 'ok') {
                            GeneraLiquidazione();
                //        }
                //    }
                //});
            }
        },
        iconCls: 'add'
    });

function openContratto(idContratto) {
    var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
    var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

    window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + idContratto, '_blank');
}

function buildGridFattureLiquidazioneRag(liquidazione) {
    
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaFattureByLiquidazione' + window.location.search,
        method: 'GET',
        timeout: 20000000
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaFattureByLiquidazioneResult',
        fields: [
           { name: 'Prog' },
	       { name: 'IdUnivoco' },
	       { name: 'Contratto' },
           { name: 'NumeroFatturaBeneficiario' },
           { name: 'DataFatturaBeneficiario' },
           { name: 'DescrizioneFattura' },
           { name: 'ImportoTotaleFattura' },
           { name: 'ImportoLiquidato' },
           { name: 'AnagraficaInfo' },
           { name: 'IdDcoumentoContabile' }

           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" }
    });

    store.on({ 'load': {
        fn: function(store, records, options) {
            if (records.length == 0) {
                //                if (Ext.getCmp('btnConfermaSelezioneFattura') != null && Ext.getCmp('btnConfermaSelezioneFattura') != undefined)
                //                    Ext.getCmp('btnConfermaSelezioneFattura').disable();
            }
            maskApp.hide();
        },
        scope: this
    }
    });

    maskApp.show();

    var parametri = { idLiquidazione: liquidazione.ID };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
            var totalRows = Ext.getCmp('GridFattureLiquidazioneRag').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridFattureLiquidazioneRag').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                
                var view = Ext.getCmp('GridFattureLiquidazioneRag').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Repertorio", width: 60, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
            renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                var retValue = '';
                if (record.data.Contratto != undefined && record.data.Contratto != null) {

                    if (!isNullOrEmpty(record.data.Contratto.NumeroRepertorio)) {
                        retValue = record.data.Contratto.NumeroRepertorio;
                    }

                }
                return retValue;
            }
        },
         { header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true },
         { header: "Data Fattura", width: 60, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },

         { header: "Beneficiario", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
             renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                 var retValue = '';
                 if (record.data.AnagraficaInfo != undefined && record.data.AnagraficaInfo != null) {

                     if (!isNullOrEmpty(record.data.AnagraficaInfo.Nome) && !isNullOrEmpty(record.data.AnagraficaInfo.Cognome)) {
                         retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome
                     }
                     if (!isNullOrEmpty(record.data.AnagraficaInfo.Denominazione)) {
                         retValue = record.data.AnagraficaInfo.Denominazione;
                     }
                 }
                 return retValue;
             }
         },

            { header: "Sede", width: 85, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede
                        }
                    }
                    return retValue;
                }
            },

            { header: "Metodo Pagamento", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
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
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Descrizione", width: 100, dataIndex: 'DescrizioneFattura', sortable: true, locked: false, hidden: true },
            { header: "Importo Totale", width: 60, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false, renderer: eurRend },
            { header: "Da Liquidare", width: 60, dataIndex: 'ImportoLiquidato', sortable: true, locked: false, renderer: eurRend },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "IdDocumento", width: 100, dataIndex: 'IdDocumento', hidden: true },
            { header: "IdDcoumentoContabile", width: 100, dataIndex: 'IdDcoumentoContabile', hidden: true }

        ]);


    var grid = new Ext.grid.GridPanel({
        id: 'GridFattureLiquidazioneRag',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        xtype: "form",
        autoHeight: true,
        width: 750,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        loadMask: true,
        viewConfig: {
            emptyText: "Nessuna fattura associata alla liquidazione.",
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

//    var gridFatture = new Ext.Panel({
//        autoHeight: true,
//        xtype: 'panel',
//        border: false,
//        layout: 'table',
//        layoutConfig: {
//            columns: 1
//        },
//        items: [grid]
//    }
//    );

    return grid;
}


   
//FUNZIONE CHE visualizza i beneficiari  legati alla liquidazione
function buildBeneficiariLiquidazioneRagioneria(liquidazione) {
    var params = { ID: liquidazione.ID };
    
    var proxy = new Ext.data.HttpProxy({
	    url: 'ProcAmm.svc/GetlistaBeneficiariLiquidazioni',
        method:'GET',
        timeout: 900000
    });
    
    var reader = new Ext.data.JsonReader({
        root: 'GetlistaBeneficiariLiquidazioniResult',
        fields: [
           { name: 'CodiceFiscale' },
           { name: 'Denominazione' },
           { name: 'ImportoSpettante' },
           { name: 'ImportoPagato' },
           { name: 'ID' },
           { name: 'NLiquidazione' },
           { name: 'IdAnagrafica' },
           { name: 'IdDcoumentoContabile' },
           { name: 'IdDocumento' },
           { name: 'CodiceSiope' },
           { name: 'CodiceSiopeAggiuntivo' },
           { name: 'EsenzCommBonifico' },
           { name: 'StampaAvviso' },
           { name: 'Bollo' },
           { name: 'ImponibileIrpef' },
           { name: 'RitenuteIrpef' },
           { name: 'RitenutePrevidenzialiBen' },
           { name: 'AltreRitenute' },
           { name: 'ImponibilePrevidenziale' },
           { name: 'RitenutePrevidenzialiEnte' },
           { name: 'AddizionaleComunale' },
           { name: 'AddizionaleRegionale' },
           //campi aggiungi per la visualizzazione del dettaglio
           { name: 'SedeVia' },
           { name: 'SedeComune' },
           { name: 'SedeProvincia' },
           { name: 'Cig' },
           { name: 'Cup' },
           { name: 'Iban' },
           { name: 'PartitaIva' },
           { name: 'IdConto' },
           { name: 'IdSede' },
           { name: 'FlagPersonaFisica' },
           { name: 'IdModalitaPag' },
           { name: 'DescrizioneModalitaPag' },
           { name: 'HasDatiBancari' },
           { name: 'IdContratto' },
           { name: 'NumeroRepertorioContratto' },
           { name: 'IsDatoSensibile' },
           { name: 'ProgFatturaLiq' }           
           ]
       });
    
    var store = new Ext.data.GroupingStore({
        proxy: proxy,
        reader: reader,
        groupField: 'NLiquidazione',
        sortInfo: {
            field: 'NLiquidazione',
            direction: "ASC"
        },
        listeners: {
            'beforeload': function(store, operation, eOpts) {                
                actionDetailBeneficiario.setDisabled(true);    
            }
        }
    });
    
	store.on({
         'load':{
            fn: function(store, records, options) {
                if(records.length==0)
                    actionRegistraBeneficiario.setDisabled(true);
            },
            scope:this
  	     }
 	}); 
  
    store.load({params:params});
    
    var summary = new Ext.grid.GroupSummary(); 
   
    var sm = new Ext.grid.CheckboxSelectionModel(
 	    {singleSelect: true,
 	    listeners: {
            rowselect: function(sm, row, rec) { 	                
                actionDetailBeneficiario.setDisabled(false); 
             },
             rowdeselect: function(sm, row, rec) {	                 
                actionDetailBeneficiario.setDisabled(true);
            }
 	     } 	                
    });

    function renderSwitchCodFiscPartitaIVA(val, p, record) {
        if (record.data.FlagPersonaFisica) {
            return record.data.CodiceFiscale;
        }
        else {
            return record.data.PartitaIva;
        }
    }      
    
    var ColumnModel = new Ext.grid.ColumnModel([
                   sm,
	               { header: "Cod Fisc/Partita IVA", width: 110, dataIndex: 'CodiceFiscale', renderer: renderSwitchCodFiscPartitaIVA, id: 'CodiceFiscale', sortable: true,
		                 summaryRenderer: function(v, params, data){return '<b> Totale </b>'; }},
	               { header: "Denominazione", width: 120, dataIndex: 'Denominazione', sortable: true, locked: false },
	               { header: "Num. Repertorio Contratto", width: 60, dataIndex: 'NumeroRepertorioContratto', sortable: true, hidden: false,
	                   renderer: function(value, metaData, record, rowIdx, colIdx, store) {
	                       var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
	                       var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

	                       var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.IdContratto;
	                       var link = "<a target='_blank' href='" + href + "' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorioContratto + "</a>";
                    	                   
	                       return link;
	                   }
	               },
                   { renderer: eurRend, header: "Importo Spettante", width: 110,dataIndex: 'ImportoSpettante',   id: 'ImportoSpettante',    sortable: true, summaryType:'sum'  },
	               { renderer: eurRend, header: "Importo Da Pagare", width: 110,dataIndex: 'ImportoPagato',   id: 'ImportoPagato',    sortable: true  ,
	                editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false						        
						    })
				   },
	               { header: "ID",  dataIndex: 'ID',  sortable: true, locked:false, hidden:true},
	               { header: "NLiquidazione",  width: 110,dataIndex: 'NLiquidazione',  sortable: true, locked:false, hidden:true},
	               { header: "IdAnagrafica", width: 110, dataIndex: 'IdAnagrafica',  sortable: true, locked:false, hidden:true},
	               { header: "Codice Siope",  width: 110,dataIndex: 'CodiceSiope',id: 'CodiceSiope',sortable: true, locked:false,
	                editor: new Ext.form.ComboBox({
		            	            id:"isCodiceSiope",
		            	            listWidth:400,
                                    width:400,
                                    displayField: 'Descrizione',
								    valueField: 'Id',
                                    mode: 'remote',
                                    store: new Ext.data.JsonStore({
                                            url: 'ProcAmm.svc/GetListaCodiciSiope?AnnoRif=' + liquidazione.Bilancio + '&CapitoloRif=' + liquidazione.Capitolo,
                                            method: 'GET',
                                            readOnly:false,
                                            root: 'GetListaCodiciSiopeResult',
                                            fields: [{name:'Descrizione'},{name:'Id'}],
                                            autoLoad: true,
                                            forceSelection:true,
                                            valueNotFoundText:'SELEZIONARE UN CODICE SIOPE'
                                    }),
		            	            typeAhead: false,
		            	            mode: 'remote',
		            	            triggerAction: 'all',
		            	            selectOnFocus: true,
                                    lazyRender: true,
                                    forceSelection: true 	            	         
		            	     })
		            },
                   { header: "Codice Siope Aggiuntivo",  width: 110,id: 'CodiceSiopeAggiuntivo', dataIndex: 'CodiceSiopeAggiuntivo' , sortable: true, locked:false,
	                editor: new Ext.form.ComboBox({
		            	            id:"isCodiceSiopeAggiuntivo",
		            	            listWidth:400,
                                    width:200,
                                    displayField: 'Descrizione',
								    valueField: 'Id',
                                    mode: 'remote',
                                    store: new Ext.data.JsonStore({
                                            url: 'ProcAmm.svc/GetListaCodiciSiope?AnnoRif=' + liquidazione.Bilancio + '&CapitoloRif=' + liquidazione.Capitolo,
                                            method: 'GET',
                                            readOnly:false,
                                            root: 'GetListaCodiciSiopeResult',
                                            fields: [{name:'Descrizione'},{name:'Id'}],
                                            autoLoad: true,
                                            forceSelection:true,
                                            valueNotFoundText:'SELEZIONARE UN CODICE SIOPE'
                                    }),
		            	            typeAhead: false,
		            	            mode: 'remote',
		            	            triggerAction: 'all',
		            	            selectOnFocus: true, 
		            	            lazyRender: true,
                                    forceSelection: true 	            	         
		            	     })
		            },
                   { header: "Esenz. Comm. Bon.", dataIndex: 'EsenzCommBonifico', id: 'EsenzCommBonifico', width: 110, sortable: true, locked: false, renderer: renderBool,
                    editor: new Ext.form.ComboBox({
		            	            id:"isEsenzCommBonifico",
		            	            store:storeSiNo,
		            	            displayField: 'description',
								    valueField: 'value',
		            	            typeAhead: false,
		            	            mode: 'local',
		            	            triggerAction: 'all',
		            	            selectOnFocus: true, editable: false,
                                    lazyRender: true,
                                    forceSelection: true 	            	         
		            	     })
		            },
                   { header: "Stampa Avviso", dataIndex: 'StampaAvviso', id: 'StampaAvviso', width: 110, sortable: true, locked: false, renderer: renderBool,
                    editor: new Ext.form.ComboBox({
		            	            id:"isStampaAvviso",
		            	            store:storeSiNo,
		            	            typeAhead: false,
		            	            mode: 'local',
		            	            displayField: 'description',
								    valueField: 'value',
		            	            triggerAction: 'all',
		            	            selectOnFocus: true, editable: false,
                                    lazyRender: true,
                                    forceSelection: true 	            	         
		            	     })
		           },
                   { header: "Bollo", dataIndex: 'Bollo', id: 'Bollo', sortable: true, locked: false, renderer: renderBool,
                    editor: new Ext.form.ComboBox({
		            	            id:"isBollo",
		            	            store:storeSiNo,
		            	            displayField: 'description',
								    valueField: 'value',
		            	            typeAhead: false,
		            	            mode: 'local',
		            	            triggerAction: 'all',
		            	            selectOnFocus: true, editable: false,
                                    lazyRender: true,
                                    forceSelection: true 	            	         
		            	     })
                   },
                   { renderer: eurRend, header: "Imponibile Irpef", dataIndex: 'ImponibileIrpef', id: 'ImponibileIrpef', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
					},
                   { renderer: eurRend, header: "Ritenute Irpef", dataIndex: 'RitenuteIrpef', id: 'RitenuteIrpef', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
					},
                   { renderer: eurRend, header: "Ritenute Prev. Ben", dataIndex: 'RitenutePrevidenzialiBen', id: 'RitenutePrevidenzialiBen', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				   },
                   { renderer: eurRend, header: "Altre Ritenute", dataIndex: 'AltreRitenute', id: 'AltreRitenute', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				   },
                   { renderer: eurRend, header: "Impon. Previdenziale", dataIndex: 'ImponibilePrevidenziale', id: 'ImponibilePrevidenziale', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				   },
                   { renderer: eurRend, header: "Riten. Previdenziali Ente", dataIndex: 'RitenutePrevidenzialiEnte', id: 'RitenutePrevidenzialiEnte', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				   },
                   { renderer: eurRend, header: "Addiz. Comunale", dataIndex: 'AddizionaleComunale', id: 'AddizionaleComunale', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				    },
                   { renderer: eurRend, header: "Addiz. Regionale", dataIndex: 'AddizionaleRegionale', id: 'AddizionaleRegionale', sortable: true, locked: false,
                    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
				    }
	             	] );

				    var GridBeneficiariRag = new Ext.grid.EditorGridPanel({
				    id: 'GridBeneficiariRag',
				    cls: 'row-summary-style-GridBeneficiariRag',
				        ds: store,
				        colModel: ColumnModel,
				        sm: sm,
				        title: "",
				        autoHeight: true,
				        width: 750,
				        plugins: [summary],
				        autoWidth: true,
				        clicksToEdit: 1,
				        layout: 'fit',
				        loadMask: true,
				        view: new Ext.grid.GroupingView({
				            forceFit: true,
				            showGroupName: false,
				            enableNoGroups: true, // REQUIRED!
				            hideGroupedColumn: true,
				            enableGroupingMenu: false,
				            emptyGroupText: '',
				            emptyText: "Nessun beneficiario associato alla liquidazione selezionata."
				        })
				    });

    actionDetailBeneficiario.setDisabled(true);
    actionRegistraBeneficiario.setDisabled(false);
	
    return GridBeneficiariRag;
}

//DEFINISCO L'AZIONE DETTAGLIO BENEFICIARIO
var actionDetailBeneficiario = new Ext.Action({
    text: 'Dettaglio',
    tooltip: 'Dettaglio del beneficiario selezionato',
    handler: function() {
    var storeGridBen = Ext.getCmp('GridBeneficiariRag');
        Ext.each(storeGridBen.getSelectionModel().getSelections(),
            function(rec) {
                //per caricare i dati dal SIC usare la prima istruzione, dal DB la seconda
                //loadAndShowDetailBeneficiario(rec.data, 'GridBeneficiariRag', false);
                showDetailBeneficiario(rec.data, 'GridBeneficiariRag', false);
            }
        )
    },
    iconCls: 'read'
});

//DEFINISCO L'AZIONE per la gestione dei beneficiari
var actionRegistraBeneficiario = new Ext.Action({
    text: 'Registra Dati Beneficiari',
    tooltip: 'Registra i dati del Beneficiario della liquidazione selezionata',
    disabled: true,
    handler: function() {
        var storeGridLiq = Ext.getCmp('GridBeneficiariRag').getStore();

        var json = '';
        storeGridLiq.each(function(storeGridLiq) {
            json += Ext.util.JSON.encode(storeGridLiq.data) + ',';
        });
        json = json.substring(0, json.length - 1);

        Ext.getCmp("BeneficiarioLiquidazione").setValue(json);

        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        myPanelBeneficiariRag.getForm().timeout = 100000000;
        myPanelBeneficiariRag.getForm().submit({
            url: 'ProcAmm.svc/Registra_BeneficiarioLiquidazioneImpegnoUR',
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure:
                    function(result, response) {
                        var lstr_messaggio = '';
                        try {
                            lstr_messaggio = response.result.FaultMessage;
                        } catch (ex) {
                            lstr_messaggio = 'Errore Generale';
                        }

                        Ext.MessageBox.show({
                            title: 'Registrazione Dati Beneficiari',
                            msg: lstr_messaggio,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.ERROR,
                            fn: function(btn) {
                                return;
                            }
                        });

                    }, // FINE FAILURE
            success:
					function(result, response) {
					    var msg = 'Operazione effettuata con successo!';
					    Ext.MessageBox.show({
					        title: 'Registrazione Dati Beneficiari',
					        msg: msg,
					        buttons: Ext.MessageBox.OK,
					        icon: Ext.MessageBox.INFO,
					        fn: function(btn) {
					            Ext.getCmp('GridBeneficiariRag').getStore().reload();
					        }
					    }
					    );
					} // FINE SUCCESS
        }) // FINE SUBMIT      
    },
    iconCls: 'save'
});

//DEFINISCO L'AZIONE per la generazione dei mandati
var actionGeneraMandato = new Ext.Action({
    text: 'Compila mandato',
    tooltip: 'Compila il mandato della liquidazione selezionata',
    handler: function() {
        var grigliaLiq = Ext.getCmp("GridLiquidazioneRag");
       
        var pannelloLiquidazione = new Ext.Panel({
            id:'pannelloLiquidazione',
	        width: 600,
	        autoHeight:true,
	        title : "Dettaglio Liquidazione Selezionata",
	    items : [grigliaLiq]
        });
        
        popupInfoMandato.add(pannelloLiquidazione);
     
         var grigliaben = Ext.getCmp("GridBeneficiariRag");
         var pannelloBeneficiariLiq = new Ext.Panel({
                id:'pannelloBeneficiariLiq',
	            width: 600,
	            autoHeight:true,
	            title : "Elenco Beneficiari per liquidazione selezionata",
	        items : [grigliaben]
            });
        popupInfoMandato.add(pannelloBeneficiariLiq);
   
        popupInfoMandato.add(formInfoMandato);
        popupInfoMandato.show();
    },
    iconCls: 'save'
});
 
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanelBeneficiariRag = new Ext.FormPanel({
    id:'myPanelBeneficiariRag',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    width: 750,
    collapsible: true,
    xtype: "form",	
	title : "Beneficiari della liquidazione selezionata",
	tbar: [actionRegistraBeneficiario, actionGeneraMandato, actionDetailBeneficiario],
	items : [{
            id: "BeneficiarioLiquidazione",
            xtype: "hidden"
            }]
    });

   var myPanelFattureRag = new Ext.FormPanel({
        id: 'myPanelFattureRag',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        width: 750,
        collapsible: true,
        xtype: "form",
        title: "Fatture della liquidazione selezionata",
        items: [{
            id: "FatturaLiquidazione",
            xtype: "hidden"
      }]
        });


function MostraColonne(mandato)
{
    var GridBeneficiariRag = Ext.getCmp('GridBeneficiariRag');
    if (mandato == 1){
        actionRegistraBeneficiario.hide();
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('CodiceSiope'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('CodiceSiopeAggiuntivo'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('ImponibileIrpef'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('RitenuteIrpef'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('RitenutePrevidenzialiBen'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('AltreRitenute'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('ImponibilePrevidenziale'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('RitenutePrevidenzialiEnte'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('AddizionaleComunale'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('AddizionaleRegionale'), true);
    }else{
        actionGeneraMandato.hide();
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('ImportoPagato'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('EsenzCommBonifico'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('StampaAvviso'), true);
        GridBeneficiariRag.colModel.setHidden(GridBeneficiariRag.colModel.getIndexById('Bollo'), true); 
    }    
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "Liquidazioni"
function buildGridLiquidazioni(key) {
    var qstring = '';

    if(key == undefined || key == ""){
        qstring = window.location.search;
    }else{
        qstring='?key='+key;
    }
    
    var proxy = new Ext.data.HttpProxy({
	    url: 'ProcAmm.svc/GetTutteLiquidazioniRegistrate' + qstring,
        method:'GET'
    });

    var reader = new Ext.data.JsonReader({

        root: 'GetTutteLiquidazioniRegistrateResult',
        fields: [
           { name: 'Bilancio' },
           { name: 'UPB' },
           { name: 'MissioneProgramma' },
           { name: 'Capitolo' },
           { name: 'ImpDisp' },
           { name: 'ImpPrenotato' },
           { name: 'NumPreImp' },
           { name: 'AnnoPrenotazione' },
           { name: 'ID' },
           { name: 'ImpPrenotatoLiq' },
           { name: 'ImportoIva' },
           { name: 'ContoEconomica' },
           { name: 'NumImpegno' },
           { name: 'NLiquidazione' },
           { name: 'Stato' },
           { name: 'IdImpegno' },
           { name: 'PianoDeiContiFinanziario' },
           { name: 'HashTokenCallSic' },
           { name: 'IdDocContabileSic' }
          ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        listeners: {
            'beforeload': function(store, operation, eOpts) {
                actionRegistraLiq.setDisabled(true);
                hidePanelBeneficiari();
                hidePanelFatture();
            }
        }
    });
	
    store.on({
        'load':{
            fn: function(store, records, options){
                mask.hide(); 
            },
            scope:this
  	           }
 	});

   var CodiceUfficio =  Ext.get('Cod_uff_Prop').dom.value;
   
   var parametri = { CodiceUfficio: CodiceUfficio };
   store.load({params:parametri});

   var sm = new Ext.grid.CheckboxSelectionModel({
       singleSelect: true,
       listeners: {
           rowselect: function(sm, row, rec) {
               var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio };

               if (rec.data.Stato != 1) {
                   if (rec.data.Stato == 0) {
                       alert("Non è possibile registrare questa liquidazione poichè risulta non attiva, contattare l'Amministratore del Sistema.");
                   } else if (rec.data.Stato == 2) {
                       alert("Non è possibile registrare questa liquidazione poichè risulta non confermata dall'ufficio proponente.");
                   }

                   actionRegistraLiq.setDisabled(true);
                   showPanelBeneficiari(liquidazione, true);
                   showPanelFatture(liquidazione, true);
               } else {
                   if ((rec.data.NLiquidazione == '') || (rec.data.NLiquidazione == '0')) {
                       if ((rec.data.NumImpegno == '') || (rec.data.NumImpegno == '0')) {
                           actionRegistraLiq.setDisabled(true);
                           showPanelBeneficiari(liquidazione, true);
                           showPanelFatture(liquidazione, true);
                       } else {
                           actionRegistraLiq.setDisabled(false);
                           showPanelBeneficiari(liquidazione, false);
                           showPanelFatture(liquidazione, false);
                       }
                   } else {
                       actionRegistraLiq.setDisabled(true);
                       showPanelBeneficiari(liquidazione, true);
                       showPanelFatture(liquidazione, false);
                   }
               }
           }
 	       ,
           rowdeselect: function(sm, row, rec) {
               actionRegistraLiq.setDisabled(true);
               hidePanelBeneficiari();
               hidePanelFatture();
           }
       }
   }); 
   
	 var ColumnModel = new Ext.grid.ColumnModel({
            columns: [
	                    sm,
	                    { header: "N. Liquidazione", dataIndex: 'NLiquidazione',  id: 'NLiquidazione',  sortable: true },
						{renderer: eurRend, header: "Importo<br/>da Liquidare", dataIndex: 'ImpPrenotatoLiq', sortable: true },
						{ header: "Num.<br/>Impegno", dataIndex: 'NumImpegno', sortable: true },
						{ header: "NumPreImp", dataIndex: 'NumPreImp', sortable: true, hidden: true },
						{ header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true },
						{ header: "UPB", width: 65, dataIndex: 'UPB', sortable: true },
						{ header: "Missione.<br/>Programma", dataIndex: 'MissioneProgramma', sortable: true },
						{ header: "Esercizio", width: 60, dataIndex: 'Bilancio', id: 'Bilancio', sortable: true },
						{ header: "ID", dataIndex: 'ID',  id: 'ID',  sortable: false , hidden: true},
						{ header: "Conto<br/>Economica", dataIndex: 'ContoEconomica', sortable: false, id: 'ContoEconomico',
		            	editor:new Ext.form.TextField({
		            	           allowBlank: false}) 
					    },
					    { renderer: eurRend, header: "Importo<br/>Iva", dataIndex: 'ImportoIva', sortable: false, id: 'ImportoIva',
						    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
						},
						{ header: "PCF", dataIndex: 'PianoDeiContiFinanziario', id: 'PianoDeiContiFinanziario', sortable: true, 
		            	    editor: new Ext.form.ComboBox({
		            	        id: "pcfListLIQ",
		            	        width: 400,
		            	        displayField: 'Id',
		            	        valueField: 'Id',
		            	        listWidth: 350,
		            	        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
		            	        style: 'background-color: #fffb8a; background-image:none;',
		            	        mode: 'local',
		            	        store: new Ext.data.Store({
		            	            id: 'pcfStore',
		            	            proxy: new Ext.data.HttpProxy({
		            	                url: 'ProcAmm.svc/GetPianoDeiContiFinanziari',
		            	                method: 'GET'
		            	            }),
		            	            reader: new Ext.data.JsonReader({
		            	                root: 'GetPianoDeiContiFinanziariResult',
		            	                fields: [
                                           { name: 'Id' },
                                           { name: 'Descrizione' }
                                        ]
		            	            })
		            	        }),
		            	        lazyRender: false,
		            	        triggerAction: 'all',
		            	        listeners: {
		            	            focus: function(combo) {
		            	                var rows = Ext.getCmp('GridLiquidazioneRag').getSelectionModel().getSelections();
		            	                var params = {
		            	                    AnnoRif: rows[0].data.Bilancio,
		            	                    CapitoloRif: rows[0].data.Capitolo
		            	                };
		            	                Ext.getCmp('pcfListLIQ').store.reload({ params: params });
		            	            }
		            	        }
		            	    })
		            	},
						{ header: "Stato", dataIndex: 'Stato',  id: 'Stato',  hidden: true},
						{ header: "IdImpegno", dataIndex: 'IdImpegno', id: 'IdImpegno', hidden: true }
		 ]
		});
     
     var GridLiq = new Ext.grid.EditorGridPanel({
                    id: 'GridLiquidazioneRag',
			        ds: store,
			        colModel: ColumnModel,
			        sm: sm,
					title : "",
			        autoHeight:true,
			        autoWidth:true,
			        layout: 'fit',
			        loadMask: true,
			        viewConfig : { forceFit : true },
			        cls: 'row-summary-style-GridLiquidazioneRag'
			    });
          
      actionRegistraLiq.setDisabled(true);
	          
      return GridLiq;
  }

  function showPanelBeneficiari(liquidazione, disableActionRegistraBeneficiario) {
      if(Ext.getCmp('GridBeneficiariRag')!=undefined)
          myPanelBeneficiariRag.remove(Ext.getCmp('GridBeneficiariRag'));
        
      myPanelBeneficiariRag.add(buildBeneficiariLiquidazioneRagioneria(liquidazione));

      var val = Ext.get('IsMandato').dom.value;
      MostraColonne(val);

      actionRegistraBeneficiario.setDisabled(disableActionRegistraBeneficiario);

      myPanelBeneficiariRag.doLayout();
      myPanelBeneficiariRag.render("ListaBen"); 
      myPanelBeneficiariRag.show();
  }
  function hidePanelBeneficiari() {
      if (Ext.getCmp('GridBeneficiariRag') != undefined)
          myPanelBeneficiariRag.remove(Ext.getCmp('GridBeneficiariRag'));

      myPanelBeneficiariRag.doLayout();
      myPanelBeneficiariRag.hide();

      actionRegistraBeneficiario.setDisabled(true);
      actionDetailBeneficiario.setDisabled(true);
  }

  function showPanelFatture(liquidazione, disableActionRegistraFattura) {
      if (Ext.getCmp('GridFattureLiquidazioneRag') != undefined)
          myPanelFattureRag.remove(Ext.getCmp('GridFattureLiquidazioneRag'));

      myPanelFattureRag.add(buildGridFattureLiquidazioneRag(liquidazione));

      myPanelFattureRag.doLayout();
      myPanelFattureRag.render("ListaFatt");
      myPanelFattureRag.show();
  }

  function hidePanelFatture() {
      if (Ext.getCmp('GridFattureLiquidazioneRag') != undefined)
          myPanelFattureRag.remove(Ext.getCmp('GridFattureLiquidazioneRag'));

      myPanelFattureRag.doLayout();
      myPanelFattureRag.hide();
      }
  

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni"
var myPanelLiq = new Ext.FormPanel({
    id:'myPanelLiq',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: false,
    width: 750,
	xtype: "form",
	tbar: [actionRegistraLiq], 
	title : "Liquidazioni per il presente provvedimento",
	items : [
	  	 {
            id: "LiquidazioniRagioneria",
            xtype: "hidden"
        },
	     {
	         id: "DataMovimento",
	         xtype: "hidden"
	     }
	     ]

	 });
	 
//FUNZIONE CHE REGISTRA LE LIQUIDAZIONI
function GeneraLiquidazione() {
    var json = '';
    
    Ext.each(Ext.getCmp('GridLiquidazioneRag').getSelectionModel().getSelections(), function(rec) {
        json +=Ext.util.JSON.encode(rec.data);
    });
        
    if (json!=''){
        Ext.getDom('LiquidazioniRagioneria').value = json;
        Ext.getDom('DataMovimento').value = getDataMovimentoAggiorna();
        var dataMovimento=Ext.getDom('DataMovimento').value;
        var params = { LiquidazioniRagioneria: json, DataMovimento: dataMovimento };

        var ajaxMask = new Ext.LoadMask(Ext.getBody(), { msg: "Operazione in corso..." });
        ajaxMask.show();
     
        Ext.Ajax.request({
            url: 'ProcAmm.svc/GenerazioneLiquidazione' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            headers: { 'Content-Type': 'application/json' },
            params: Ext.encode(params),
            method: 'POST',
            success: function(response, result) {
                ajaxMask.hide();
                Ext.getCmp('GridLiquidazioneRag').getStore().reload();                
            },
            failure: function(response, options) {
                ajaxMask.hide();
                var data = Ext.decode(response.responseText);
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: data.FaultMessage,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
            }
        });
    }// fine if
}

