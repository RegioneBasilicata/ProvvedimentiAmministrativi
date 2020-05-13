var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

//DEFINISCO L'AZIONE DETTAGLIO BENEFICIARIO
var actionDetailBeneficiarioImpegno = new Ext.Action({
    text: 'Dettaglio',
    tooltip: 'Dettaglio del beneficiario selezionato',
    handler: function () {
        var storeGridBen = Ext.getCmp('GridBeneficiariImpegnoRag');
        Ext.each(storeGridBen.getSelectionModel().getSelections(),
            function (rec) {
                //per caricare i dati dal SIC usare la prima istruzione, dal DB la seconda
                //loadAndShowDetailBeneficiario(rec.data, 'GridBeneficiariImpegnoRag', false);
                showDetailBeneficiario(rec.data, 'GridBeneficiariImpegnoRag', false);
            }
        )
    },
    iconCls: 'read'
});

//DEFINISCO L'AZIONE per la gestione dei beneficiari
var actionRegistraBeneficiarioImpegno = new Ext.Action({
    text: 'Registra Dati Beneficiari',
    tooltip: 'Registra i dati del Beneficiario dell\'impegno selezionato',
    disabled: true,
    handler: function () {
        var storeGridLiq = Ext.getCmp('GridBeneficiariImpegnoRag').getStore();

        var json = '';
        storeGridLiq.each(function (storeGridLiq) {
            json += Ext.util.JSON.encode(storeGridLiq.data) + ',';
        });
        json = json.substring(0, json.length - 1);

        Ext.getCmp("BeneficiarioImpegno").setValue(json);

        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        myPanelBeneficiariRag.getForm().timeout = 100000000;
        myPanelBeneficiariRag.getForm().submit({
            url: 'ProcAmm.svc/Registra_BeneficiarioLiquidazioneImpegnoUR' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure:
                    function (result, response) {
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
                            fn: function (btn) {
                                return;
                            }
                        });

                    }, // FINE FAILURE
            success:
					function (result, response) {
					    var msg = 'Operazione effettuata con successo!';
					    Ext.MessageBox.show({
					        title: 'Registrazione Dati Beneficiari',
					        msg: msg,
					        buttons: Ext.MessageBox.OK,
					        icon: Ext.MessageBox.INFO,
					        fn: function (btn) {
					            Ext.getCmp('GridBeneficiariImpegnoRag').getStore().reload();
					        }
					    }
					    );
					} // FINE SUCCESS
        }) // FINE SUBMIT      
    },
    iconCls: 'save'
});

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanelBeneficiariImpegnoRag = new Ext.FormPanel({
    id: 'myPanelBeneficiariImpegnoRag',
    frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    width: 750,
    collapsible: true,
    xtype: "form",
    title: "Beneficiari dell\'impegno selezionato",
    tbar: [actionRegistraBeneficiarioImpegno, actionDetailBeneficiarioImpegno],
    items: [{
        id: "BeneficiarioImpegno",
        xtype: "hidden"
    }]
});

function showPanelBeneficiariImpegno(impegno, disableactionRegistraBeneficiarioImpegno) {
    if (Ext.getCmp('GridBeneficiariImpegnoRag') != undefined)
        myPanelBeneficiariImpegnoRag.remove(Ext.getCmp('GridBeneficiariImpegnoRag'));

    myPanelBeneficiariImpegnoRag.add(buildBeneficiariImpegnoRagioneria(impegno));

    actionRegistraBeneficiarioImpegno.setDisabled(disableactionRegistraBeneficiarioImpegno);

    myPanelBeneficiariImpegnoRag.doLayout();
    myPanelBeneficiariImpegnoRag.render("ListaBenImpegno");
    myPanelBeneficiariImpegnoRag.show();
}

function hidePanelBeneficiariImpegno() {
    if (Ext.getCmp('GridBeneficiariImpegnoRag') != undefined)
        myPanelBeneficiariImpegnoRag.remove(Ext.getCmp('GridBeneficiariImpegnoRag'));

    myPanelBeneficiariImpegnoRag.doLayout();
    myPanelBeneficiariImpegnoRag.hide();

    actionRegistraBeneficiarioImpegno.setDisabled(true);
    actionDetailBeneficiarioImpegno.setDisabled(true);
}

//FUNZIONE CHE visualizza i beneficiari  legati all'impegno
function buildBeneficiariImpegnoRagioneria(impegno) {
    var params = { ID: impegno.ID };

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetlistaBeneficiariImpegno' + window.location.search,
        method: 'GET',
        timeout: 900000
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetlistaBeneficiariImpegnoResult',
        fields: [
           { name: 'CodiceFiscale' },
           { name: 'Denominazione' },
           { name: 'ImportoSpettante' },
           { name: 'ImportoPagato' },
           { name: 'ID' },
           { name: 'NDcoumentoContabile' },
           { name: 'IdAnagrafica' },
           { name: 'IDDocumentoContabile' },
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
        groupField: 'NDcoumentoContabile',
        sortInfo: {
            field: 'NDcoumentoContabile',
            direction: "ASC"
        },
        listeners: {
            'beforeload': function (store, operation, eOpts) {
                actionDetailBeneficiarioImpegno.setDisabled(true);
            }
        }
    });

    store.on({
        'load': {
            fn: function (store, records, options) {
                if (records.length == 0)
                    actionRegistraBeneficiarioImpegno.setDisabled(true);
            },
            scope: this
        }
    });

    store.load({ params: params });

    var summary = new Ext.grid.GroupSummary();

    var sm = new Ext.grid.CheckboxSelectionModel(
 	    {
 	        singleSelect: true,
 	        listeners: {
 	            rowselect: function (sm, row, rec) {
 	                actionDetailBeneficiarioImpegno.setDisabled(false);
 	            },
 	            rowdeselect: function (sm, row, rec) {
 	                actionDetailBeneficiarioImpegno.setDisabled(true);
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
	               {
	                   header: "Cod Fisc/Partita IVA", width: 110, dataIndex: 'CodiceFiscale', renderer: renderSwitchCodFiscPartitaIVA, id: 'CodiceFiscale', sortable: true,
	                   summaryRenderer: function (v, params, data) { return '<b> Totale </b>'; }
	               },
	               { header: "Denominazione", width: 120, dataIndex: 'Denominazione', sortable: true, locked: false },
	               {
	                   header: "Num. Repertorio Contratto", width: 60, dataIndex: 'NumeroRepertorioContratto', sortable: true, hidden: false,
	                   renderer: function (value, metaData, record, rowIdx, colIdx, store) {
	                       var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
	                       var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

	                       var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.IdContratto;
	                       var link = "<a target='_blank' href='" + href + "' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorioContratto + "</a>";

	                       return link;
	                   }
	               },
                   { renderer: eurRend, header: "Importo Spettante", width: 110, dataIndex: 'ImportoSpettante', id: 'ImportoSpettante', sortable: true, summaryType: 'sum' },
	               {
	                   renderer: eurRend, header: "Importo Da Pagare", width: 110, dataIndex: 'ImportoPagato', id: 'ImportoPagato', sortable: true,
	                   editor: new Ext.form.NumberField({
	                       decimalSeparator: ',',
	                       allowBlank: true,
	                       allowNegative: false
	                   })
	               },
	               { header: "ID", dataIndex: 'ID', sortable: true, locked: false, hidden: true },
	               { header: "NDcoumentoContabile", width: 110, dataIndex: 'NDcoumentoContabile', sortable: true, locked: false, hidden: true },
	               { header: "IdAnagrafica", width: 110, dataIndex: 'IdAnagrafica', sortable: true, locked: false, hidden: true },
	               {
	                   header: "Codice Siope", width: 110, dataIndex: 'CodiceSiope', id: 'CodiceSiope', sortable: true, locked: false,
	                   editor: new Ext.form.ComboBox({
	                       id: "isCodiceSiope",
	                       listWidth: 400,
	                       width: 400,
	                       displayField: 'Descrizione',
	                       valueField: 'Id',
	                       mode: 'remote',
	                       store: new Ext.data.JsonStore({
	                           url: 'ProcAmm.svc/GetListaCodiciSiope?AnnoRif=' + impegno.Bilancio + '&CapitoloRif=' + impegno.Capitolo,
	                           method: 'GET',
	                           readOnly: false,
	                           root: 'GetListaCodiciSiopeResult',
	                           fields: [{ name: 'Descrizione' }, { name: 'Id' }],
	                           autoLoad: true,
	                           forceSelection: true,
	                           valueNotFoundText: 'SELEZIONARE UN CODICE SIOPE'
	                       }),
	                       typeAhead: false,
	                       mode: 'remote',
	                       triggerAction: 'all',
	                       selectOnFocus: true,
	                       lazyRender: true,
	                       forceSelection: true
	                   })
	               },
                   {
                       header: "Codice Siope Aggiuntivo", width: 110, id: 'CodiceSiopeAggiuntivo', dataIndex: 'CodiceSiopeAggiuntivo', sortable: true, locked: false,
                       editor: new Ext.form.ComboBox({
                           id: "isCodiceSiopeAggiuntivo",
                           listWidth: 400,
                           width: 200,
                           displayField: 'Descrizione',
                           valueField: 'Id',
                           mode: 'remote',
                           store: new Ext.data.JsonStore({
                               url: 'ProcAmm.svc/GetListaCodiciSiope?AnnoRif=' + impegno.Bilancio + '&CapitoloRif=' + impegno.Capitolo,
                               method: 'GET',
                               readOnly: false,
                               root: 'GetListaCodiciSiopeResult',
                               fields: [{ name: 'Descrizione' }, { name: 'Id' }],
                               autoLoad: true,
                               forceSelection: true,
                               valueNotFoundText: 'SELEZIONARE UN CODICE SIOPE'
                           }),
                           typeAhead: false,
                           mode: 'remote',
                           triggerAction: 'all',
                           selectOnFocus: true,
                           lazyRender: true,
                           forceSelection: true
                       })
                   },
                   {
                       header: "Esenz. Comm. Bon.", dataIndex: 'EsenzCommBonifico', id: 'EsenzCommBonifico', width: 110, sortable: true, locked: false, renderer: renderBool,
                       editor: new Ext.form.ComboBox({
                           id: "isEsenzCommBonifico",
                           store: storeSiNo,
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
                   {
                       header: "Stampa Avviso", dataIndex: 'StampaAvviso', id: 'StampaAvviso', width: 110, sortable: true, locked: false, renderer: renderBool,
                       editor: new Ext.form.ComboBox({
                           id: "isStampaAvviso",
                           store: storeSiNo,
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
                   {
                       header: "Bollo", dataIndex: 'Bollo', id: 'Bollo', sortable: true, locked: false, renderer: renderBool,
                       editor: new Ext.form.ComboBox({
                           id: "isBollo",
                           store: storeSiNo,
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
                   {
                       renderer: eurRend, header: "Imponibile Irpef", dataIndex: 'ImponibileIrpef', id: 'ImponibileIrpef', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Ritenute Irpef", dataIndex: 'RitenuteIrpef', id: 'RitenuteIrpef', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Ritenute Prev. Ben", dataIndex: 'RitenutePrevidenzialiBen', id: 'RitenutePrevidenzialiBen', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Altre Ritenute", dataIndex: 'AltreRitenute', id: 'AltreRitenute', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Impon. Previdenziale", dataIndex: 'ImponibilePrevidenziale', id: 'ImponibilePrevidenziale', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Riten. Previdenziali Ente", dataIndex: 'RitenutePrevidenzialiEnte', id: 'RitenutePrevidenzialiEnte', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Addiz. Comunale", dataIndex: 'AddizionaleComunale', id: 'AddizionaleComunale', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   },
                   {
                       renderer: eurRend, header: "Addiz. Regionale", dataIndex: 'AddizionaleRegionale', id: 'AddizionaleRegionale', sortable: true, locked: false,
                       editor: new Ext.form.NumberField({
                           decimalSeparator: ',',
                           allowBlank: true,
                           allowNegative: false
                       })
                   }
    ]);

    var GridBeneficiariImpegnoRag = new Ext.grid.EditorGridPanel({
        id: 'GridBeneficiariImpegnoRag',
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
            emptyText: "Nessun beneficiario associato all\'impegno selezionato."
        })
    });

    actionDetailBeneficiarioImpegno.setDisabled(true);
    actionRegistraBeneficiarioImpegno.setDisabled(false);

    return GridBeneficiariImpegnoRag;
}

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraImpegno = new Ext.Action({
         text:'Registra',
         tooltip:'Registra l\'impegno selezionato',
        handler: function(){
            GenerazioneImpegno();
     },
        iconCls: 'add'
    });
    
//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridRiep() {

var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetImpegniRegistrati'+ window.location.search,
    method:'GET'
    }); 
      
	var reader = new Ext.data.JsonReader({

	root: 'GetImpegniRegistratiResult',
	fields: [
	       {name: 'NumImpegno'},
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpPrenotato'},
           {name: 'NumPreImp'},
           {name: 'ID'},
           {name: 'ContoEconomica'},
           {name: 'Ratei'},
           {name: 'Risconti'},
           {name: 'ImpostaIrap'},
           {name: 'isPerente'},
           {name: 'NumImpPrecedente'},
           {name: 'Stato'},
           {name: 'Codice_Obbiettivo_Gestionale'},
           { name: 'PianoDeiContiFinanziario' },
           { name: 'HashTokenCallSic' },
	       { name: 'IdDocContabileSic' },
           { name: 'HashTokenCallSic_Imp' },
           { name: 'IdDocContabileSic_Imp' }
           ]
	});
	
	 var checkColumnPerente = new Ext.grid.CheckColumn({
       header: "Cap.<br/>Perente",
       dataIndex: 'isPerente',
       readonly:true,
       width:4
    });


    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        listeners: {
            'beforeload': function(store, operation, eOpts) {
                actionRegistraImpegno.setDisabled(true);
            }
        }
    });

    store.on({
   'load':{
      fn: function(store, records, options){
       mask.hide();
                if (tipo == 1) {
                    actionRegistraImpegno.setDisabled(true);
                }
     },
      scope:this
  	 }
 	});

var ufficio =  Ext.get('Cod_uff_Prop').dom.value;

var parametri = { CodiceUfficio: ufficio };
   store.load({params:parametri});

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: true,
        listeners: {
            rowselect: function (sm, row, rec) {

                actionRegistraImpegno.setDisabled(true);

                showPanelBeneficiariImpegno(rec.data, true);
                if (rec.data.Stato != 1) {
                    if (rec.data.Stato == 0) {
                        alert("Non è possibile registrare questo impegno poichè risulta non attivo, contattare l'Amministratore del Sistema.");
                    }
                    if (rec.data.Stato == 2) {
                        alert("Non è possibile registrare questo impegno poichè risulta non confermato dall'ufficio proponente.");
                    }
                    actionRegistraImpegno.setDisabled(true);
                } else {
                    if ((rec.data.NumImpegno == '') || (rec.data.NumImpegno == '0')) {
                        if (tipo == 1) {
                            actionRegistraImpegno.setDisabled(true);
                        } else {
                        actionRegistraImpegno.setDisabled(false);
                        }
                    } else {
                        actionRegistraImpegno.setDisabled(true);
                    }
                }
            }
             ,
            rowdeselect: function (sm, row, rec) {
                actionRegistraImpegno.setDisabled(true);
                hidePanelBeneficiariImpegno();
            }
        }
    });

	         var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Num. <br/>Impegno", dataIndex: 'NumImpegno', id: 'NumImpegno', sortable: true, width: 6 },
		                { header: "Bilancio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true, width: 4 },
		                { header: "UPB", dataIndex: 'UPB', sortable: true, width: 4 },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true, width: 4 },
		                { header: "Capitolo", dataIndex: 'Capitolo', sortable: true, locked: false, width: 5 },
		            	{ renderer: eurRend, header: "Importo da<br/> Prenotare", dataIndex: 'ImpPrenotato', sortable: true, width: 5 },
		            	{ header: "Numero<br/> PreImpegno", dataIndex: 'NumPreImp', sortable: true, width: 6 },
               {
                   header: "Conto<br/> Economica", dataIndex: 'ContoEconomica', sortable: false, width: 6, id: 'ContoEconomico',
		            	    editor: new Ext.form.TextField({
		            	        allowBlank: false
		            	    })
		            	},
               {
                   renderer: eurRend, header: "Ratei", dataIndex: 'Ratei', width: 4, sortable: false, id: 'Ratei',
					        editor: new Ext.form.TextField({
					            allowBlank: false
					        })
					    },
               {
                   renderer: eurRend, header: "Imposta<br/>Irap", width: 4, dataIndex: 'ImpostaIrap', sortable: false, id: 'Irap',
					        editor: new Ext.form.TextField({
					            emptyText: '',
					            allowBlank: false
					        })
					    },
               {
                   renderer: eurRend, header: "Risconti", width: 4, dataIndex: 'Risconti', sortable: false, id: 'Risconti',
					        editor: new Ext.form.TextField({
					            allowBlank: false
					        })
					    },
					   checkColumnPerente,
					    { header: "Num. Imp.<br/>Precedente", width: 7, dataIndex: 'NumImpPrecedente', sortable: true, locked: false },

		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	{ header: "Stato", dataIndex: 'Stato', hidden: true },
		            	{ header: "COG", dataIndex: 'Codice_Obbiettivo_Gestionale', id: 'Codice_Obbiettivo_Gestionale', sortable: true, width: 3 },
               {
                   header: "PCF", dataIndex: 'PianoDeiContiFinanziario', id: 'PianoDeiContiFinanziario', sortable: true, width: 10,
		            	    editor: new Ext.form.ComboBox({
		            	        id: "pcfList",
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
                               url: 'ProcAmm.svc/GetPianoDeiContiFinanziari' + window.location.search,
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
		            	                var rows = Ext.getCmp('GridRiepilogo').getSelectionModel().getSelections();
		            	                var params = {
		            	                    AnnoRif: rows[0].data.Bilancio,
		            	                    CapitoloRif: rows[0].data.Capitolo
		            	                };
		            	                Ext.getCmp('pcfList').store.reload({ params: params });
		            	            }
		            	        }
		            	    })
		            	}

		    ]);
		            	
		    var GridRiep = new Ext.grid.EditorGridPanel({
				    	id: 'GridRiepilogo',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',
				        viewConfig : {  forceFit:true },
				        loadMask: true,
				        cls: 'row-summary-style-GridBeneficiariRag'

	          });
              actionRegistraImpegno.setDisabled(true);
 return GridRiep; 
}

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanel = new Ext.FormPanel({
    id:'myPanel',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	tbar:[actionRegistraImpegno],
	title : "Preimpegni Registrati",
	 items : [
	  	 {
            id: "ImpegniRagioneria",
            xtype: "hidden"
        },
	     {
	         id: "DataMovimento",
	         xtype: "hidden"
	     }
	     ]
	     /*,
	buttons: [{
			text: 'Registra Impegno',
			 id: 'btnRegImp'
			}
		]*/
});



////GESTISCO L'AZIONE DEL BOTTONE "Registra Impegno"
//Ext.getCmp('btnRegImp').on('click', function() {
//    var storeGridRiep = Ext.getCmp('GridRiepilogo').getStore();
//    var GridRiepilogo = Ext.getCmp('GridRiepilogo');
//    Ext.each(Ext.getCmp('GridRiepilogo').getSelectionModel().getSelections(), function(rec) {
//        GenerazioneImpegno(rec);
//    });
//});

//FUNZIONE CHE REGISTRA GLI IMPEGNI (anche perenti)
function GenerazioneImpegno() {
    
    var json = '';
    Ext.each(Ext.getCmp('GridRiepilogo').getSelectionModel().getSelections(), function(rec) {
        json +=Ext.util.JSON.encode(rec.data);
    });
    
    
    if (json!=''){
	    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        Ext.getDom('ImpegniRagioneria').value =json;
        Ext.getDom('DataMovimento').value = getDataMovimentoAggiorna();
        myPanel.getForm().timeout = 100000000;
        myPanel.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneImpegno' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            success: function(response, result) {
                mask.hide();
                //  var data = Ext.decode(response.responseText);
                //  var numeroImpegnoResult = data.GeneraImpegnoResult;
                //rec.set('NumImpegno', result.result.NumImp);
                Ext.getCmp('GridRiepilogo').getStore().reload();
                if (Ext.getCmp('GridLiquidazioneRag')) {
                    Ext.getCmp('GridLiquidazioneRag').getStore().reload();
                }
            },
            failure:
	            function(result, response) {
	                Ext.getCmp('GridRiepilogo').getStore().reload();
	                var lstr_messaggio = '';
	                try {
	                    lstr_messaggio = response.response.responseText;
	                } catch (ex) {
	                    lstr_messaggio = 'Errore Generale';
	                }
	                Ext.MessageBox.show({
	                    title: 'Errore',
	                    msg: lstr_messaggio,
	                    buttons: Ext.MessageBox.OK,
	                    icon: Ext.MessageBox.ERROR,
	                    fn: function(btn) {
	                        return;
	                    }
	                }); //fine messagebox
	            } //fine function
        });   //fine submit
    }//Fine if
}//fine function GenerazioneImpegno

