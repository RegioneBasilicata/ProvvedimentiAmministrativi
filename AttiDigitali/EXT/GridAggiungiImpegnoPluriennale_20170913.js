var isDocumentoConFatture = false;
var listaFatturaToStartImpegno = [];

var maskImp;
maskImp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


Ext.IframeWindow = Ext.extend(Ext.Window, {
    onRender: function () {
        this.bodyCfg = {
            tag: 'iframe',
            src: this.src,
            cls: this.bodyCls,
            style: {
                border: '0px none'
            }
        };

        Ext.IframeWindow.superclass.onRender.apply(this, arguments);
    }
});



var storePreImpegni = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaPreimp',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaPreimpResult',
        fields: [
           { name: 'NumPreImp' },
           { name: 'ImpDisp' },
           { name: 'TipoAtto' },
           { name: 'DataAtto' },
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' },
           { name: 'PianoDeiContiFinanziario' }
        ]
    }),
    listeners: {
        beforeload: function(store, options) {
            Ext.getCmp('preimpegno').disable();
            Ext.getCmp('annoBilancioPreImp').disable();
        },
        load: function(store, records, options) {
            Ext.getCmp('preimpegno').enable();
            Ext.getCmp('annoBilancioPreImp').enable();
        }
    }
});

storePreImpegni.setDefaultSort("NumPreImp", "DESC");


var storeCog = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaObiettiviGestionali',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaObiettiviGestionaliResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }
        ]
    })
});

storeCog.on({
    'load': {
        fn: function (store, records, options) {
            if (records.length == 1) {                
                Ext.getCmp('cog_impegno1').setValue(records[0].data.Id);                
            }
        },
        scope: this
    }
});


// un piano dei conti finanziario per ogni bilancio
// non è possibile riusare un solo store
var storePCFPreImp = new Ext.data.Store({
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
});

storePCFPreImp.on({
    'load': {
        fn: function (store, records, options) {
            if (records.length == 1) {
                Ext.getCmp('pcf_preimpegno').setValue(records[0].data.Id);
            }
        },
        scope: this
    }
});



var storePCF1 = new Ext.data.Store({
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
});

storePCF1.on({
    'load': {
        fn: function (store, records, options) {
            if (records.length == 1 && Ext.getCmp('pcf_impegno1') != undefined) {
                Ext.getCmp('pcf_impegno1').setValue(records[0].data.Id);                
            }
        },
        scope: this
    }
});

var storePCF2 = new Ext.data.Store({
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
});

var storePCF3 = new Ext.data.Store({
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
})

var actionHelpOnLine = new Ext.Action({
    text: 'Aiuto',
    tooltip: 'Aiuto in linea: Generazione impegno contabile',
    handler: function() {
        new Ext.IframeWindow({
            modal:true,
            layout: 'fit',
		    title: 'Generazione di un impegno',
            width:720,
			height:500,
			closable:true,
			resizable: false,
			maximizable: false,
			plain: false,
			iconCls: 'help',
			bodyStyle: 'overflow:auto',
			src: 'risorse/helpBilancioPluriennale.htm'
        }).show();
    },
    iconCls: 'help'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
var actionAddImpegno = new Ext.Action({
    text: 'Aggiungi',
    tooltip: 'Aggiungi un nuovo impegno',
    handler: function() {
        var mostraAnno = false;
        if (isDocumentoConFatture) {
            Ext.MessageBox.buttonText.yes = 'Si';

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Si intende generare un impegno, con contestuale liquidazione per la fattura indicata precedentemente?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                    if (btn == 'yes') {
                        showPopupPanelFattureLiquidazione(undefined, 0, true);
                    } else {
                        InitFormCapitoliImpegno(mostraAnno, false);
                    }
                }
            });
        } else {
            InitFormCapitoliImpegno(mostraAnno, false);
        }


    },
    iconCls: 'add'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
var actionAddFromFile = new Ext.Action({
    text: 'Carica da file',
    tooltip: 'Aggiungi nuovi impegni e beneficiari da file',
    handler: function () {
        //showPopupPanelUploadFile();

        window.open("BeneficiariFromFile.aspx" + window.location.search, "_self");
    },
    iconCls: 'add'
});

//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaImpegno = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function() {
        var impegniDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridRiepilogo').getSelectionModel().getSelections(), function(rec) {
            //conferma solo gli impegni da confermare, quelli aventi stato 2
            if (rec.data['Stato'] == 2) {
                var impegnoInfo = new Object();

                impegnoInfo.ID = rec.data['ID'];
                impegnoInfo.NumPreImp = rec.data['NumPreImp'];
                impegnoInfo.Stato = rec.data['Stato'];
                impegnoInfo.HashTokenCallSic = rec.data['HashTokenCallSic'];
                impegnoInfo.HashTokenCallSic_Imp = rec.data['HashTokenCallSic_Imp'];
                impegniDaComfermare.push(impegnoInfo);
            }
        });

        ConfermaMultiplaPreImpegno(impegniDaComfermare);
    },
    iconCls: 'save'
});
//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaLiquidazione2 = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function() {
        var liquidazioneDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridLiquidazioniContestuali').getSelectionModel().getSelections(), function(rec) {
            //conferma solo le liquidazioni contestuali da confermare, quelle aventi stato 2
            if (rec.data['Stato'] == 2) {
                var liquidazioneInfo = new Object();

                liquidazioneInfo.ID = rec.data['ID'];
                liquidazioneInfo.Stato = rec.data['Stato'];
                liquidazioneInfo.NumPreImp = rec.data['NumPreImp'];

                liquidazioneDaComfermare.push(liquidazioneInfo);
            }
        });

        ConfermaMultiplaLiquidazioneContestuale(liquidazioneDaComfermare);
    },
    iconCls: 'save'
});
//DEFINISCO L'AZIONE per la gestione dei beneficiari
var actionBeneficiari2 = new Ext.Action({
    text: 'Beneficiari',
    tooltip: 'Aggiunge, Elimina e Modifica i Beneficiari della liquidazione selezionata',
    handler: function() {

        Ext.each(Ext.getCmp('GridLiquidazioniContestuali').getSelectionModel().getSelections(), function(rec) {
            if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            } else {
                var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, IdDocumento: rec.data.IdDocumento, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };
                var grid = BeneficiariLiquidazioneImpegno(liquidazione, undefined, "GridBeneficiariContestuali", "GridLiquidazioniContestuali");

                Ext.getCmp('myPanelLiqContestuali').add(grid);
                
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            }
        }
		)
    },
    iconCls: 'coin'
});

  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA
var actionDeleteImpegno = new Ext.Action({
    text: 'Cancella',
    tooltip: 'Cancella selezionato',
    handler: function() {
        var storeGridImp = Ext.getCmp('GridRiepilogo').getStore();
        var GridImpegni = Ext.getCmp('GridRiepilogo');
        Ext.each(GridImpegni.getSelectionModel().getSelections(), function(rec) {
            if (rec.data.Stato == 2) {
                EliminaImpegnoNonAncoraConfermato(rec.data['NumPreImp'], rec.data['ID']);
                storeGridImp.remove(rec);

                actionAddImpegno.setDisabled(false);
                actionAddFromFile.setDisabled(false);
                actionDeleteImpegno.setDisabled(true);
                actionBeneficiariImpegni.setDisabled(true);
                actionCompilaLiquidazioni.setDisabled(true);
                actionConfermaImpegno.setDisabled(true);
                actionModificaCOGAndPdCFImp.setDisabled(true);

               
                if (Ext.getCmp('GridBeneficiariImpegnoContestuali') != undefined) {
                    Ext.getCmp('myPanel').remove(Ext.getCmp('GridBeneficiariImpegnoContestuali'));
                    Ext.getCmp('myPanel').doLayout();
                }

                try {
                    Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                } catch (ex) { }
            } else {
                EliminaPreImpegno(rec.data['NumPreImp'], rec.data['ID']);
                storeGridImp.remove(rec);

                actionAddImpegno.setDisabled(false);
                actionAddFromFile.setDisabled(false);
                actionDeleteImpegno.setDisabled(true);
                actionBeneficiariImpegni.setDisabled(true);
                actionCompilaLiquidazioni.setDisabled(true);
                actionConfermaImpegno.setDisabled(true);
                actionModificaCOGAndPdCFImp.setDisabled(true);

                try {
                    Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                } catch (ex) { }
            }
        })
    },

    iconCls: 'remove'
});

var actionBeneficiariImpegni = new Ext.Action({
    text: 'Beneficiari',
    tooltip: 'Aggiunge, Elimina e Modifica i Beneficiari dell\'impegno selezionato',
    handler: function () {
        var GridImpegni = Ext.getCmp('GridRiepilogo');
        Ext.each(GridImpegni.getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridBeneficiariImpegnoContestuali') != undefined) {
                Ext.getCmp('myPanel').remove(Ext.getCmp('GridBeneficiariImpegnoContestuali'));
                Ext.getCmp('myPanel').doLayout();
            } else {
                var impegno = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, IdDocumento: rec.data.IdDocumento, ImpPrenotato: rec.data.ImpPrenotato };
                var grid = BeneficiariLiquidazioneImpegno(undefined, impegno, "GridBeneficiariImpegnoContestuali", "GridRiepilogo");

                Ext.getCmp('myPanel').add(grid);

                Ext.getCmp('myPanel').doLayout();
            }
        }
		)
    },
    iconCls: 'coin'
});



var actionCompilaLiquidazioni = new Ext.Action({
    text: 'Crea Liquidazione',
    tooltip: 'Compila una Liquidazione per l\'impegno selezionato',
    handler: function() {

    if (!isDocumentoConFatture) {
            Ext.MessageBox.buttonText.yes = 'Si';

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Se si intende pagare una fattura è necessario aggiungerla nella sezione Contratti/Fatture.<br>Aggiungere la fattura all\'atto?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'yes') {
                        window.open("CreaProvvedimento.aspx" + window.location.search + "&addFatt=1", "_self");
                    } else {
                        addLiquidazioneContestualeDaSelezione();
                    }
                }
            });
        } else {
            addLiquidazioneContestualeDaSelezione();
        }
        



        //        var aggiugniFattura = window.confirm("Se si intende pagare una fattura è necessario aggiungerla nella sezione Contratti/Fatture. Premere OK se si vuole aggiungere una fattura all\'atto, ANNULLA nel caso in cui non si tratta di una liquidazione di una fattura");

        //        if (aggiugniFattura) {
        //            window.open("CreaProvvedimento.aspx" + window.location.search + "&addFatt=1", "_self");
        //        } else {
        //            
        //        }


    },
    iconCls: 'add'
});

    function buildPanelInfoFattImp(impegno, importoFattureImp) {
        var impegnoPrenotato;
        if (impegno.ImpPrenotato == undefined) {
            impegnoPrenotato = impegno.ImportoPrenotato;
        } else {
             impegnoPrenotato = impegno.ImpPrenotato;
        }
        
        var labelImportoImpPrenotatoValore = new Ext.form.Label({
            text: parseFloat(impegnoPrenotato).toFixed(2),
             style: 'font-size:13px; margin-top:15px;background-color: #fffb8a;background-image:none;',
            id: 'impImpPrenotatoValore'
        });
        
        var labelImportoFattureValore = new Ext.form.Label({
            text: parseFloat(importoFattureImp).toFixed(2),
             style: 'font-size:13px; margin-top:15px;background-color: #fffb8a;background-image:none;',
            id: 'impFattureValore'
        });


        var labelImportoDifferenzaFattureImpegnoValore = new Ext.form.Label({
            text: parseFloat(impegnoPrenotato - importoFattureImp).toFixed(2),
            style: 'background-color: #ffffff',
            id: 'impDifferenzaFattereImpegno'
        });
        
 
        var labelImportoImpPrenotato = new Ext.form.Label({
            text: 'Importo Impegno Selezionato: ' + labelImportoImpPrenotatoValore.text + ' € '+' - Importo Fatture Impegno: '+ labelImportoFattureValore.text  + ' €' + ' - Differenza Importi: '+ labelImportoDifferenzaFattureImpegnoValore.text + ' €',
            id: 'impImpPrenotato'
            //, 
//            style: 'background-color: #ffffff'

        });
        
        var panelInfoFatt = new Ext.Panel({
            id: 'panelInfoFatt',
            autoHeight: true,
            xtype: 'panel',
            border: false,
            layout: 'table',
            style: 'font-size:14px; margin-top:13px;background-color: #fffb8a;background-image:none;',
            layoutConfig: {
                columns: 2
            },
            items: [
                labelImportoImpPrenotato,
            {
                xtype: 'hidden',
                id: 'listaFattureImpegno'
            }
         ]
        });

        return panelInfoFatt;
    }

       



////DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
//var panelImpImpPrenotato = new Ext.Panel({
//								xtype : "panel",
//								title : "",
//								width:300,
//								buttonAlign: "center",
//								autoHeight:true,
//								layout: "fit",
//								items: [
//									labelImportoImpPrenotato
//									
//								 ]
//								}); 


    
    function getInfoImpegno(){
      var GridImpegni = Ext.getCmp('GridRiepilogo');
     }
                        
    function selectComboCOG(record,index) {
        Ext.get('DescrCOG').dom.value=index.data['Descrizione']
    }

    //PopUp Modifica COG/PdCF   
    function generaFormCOGAndPdCF(idProg, codValue, codPdCFValue, AnnoRif, CapitoloRif) {

        //Piano Dei Conti Finanziario combobox
        var proxyPdCF = new Ext.data.HttpProxy({
            url: 'ProcAmm.svc/GetPianoDeiContiFinanziari',
            method: 'GET'
        });
        
        var readerPdCF = new Ext.data.JsonReader({
            root: 'GetPianoDeiContiFinanziariResult',
            fields: [
               { name: 'Id' },
               { name: 'Descrizione' }  
           
               ]
        });

        var storePdCF = new Ext.data.Store({
            proxy: proxyPdCF,
            reader: readerPdCF
        });

        storePdCF.setDefaultSort("Descrizione", "ASC");

        var currentYear = new Date().getYear();
        if (currentYear < 1900) currentYear += 1900;

        var parametri = { AnnoRif: currentYear, CapitoloRif: CapitoloRif };        
        storePdCF.load({ params: parametri });
        
        var ComboPdCF = new Ext.form.ComboBox({
            fieldLabel: 'Piano dei Conti Finanziario(*)',
            displayField: 'Id',
            valueField: 'Id',
            listWidth: 300,
            width: 300,
            queryMode: 'local',
            store: storePdCF,
            tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
            readOnly: true,
            disabled: false,
            name: 'ComboPdCF',
            id: 'ComboPdCFImp',
            mode: 'local',
            triggerAction: 'all',
            emptyText: 'Seleziona Piano dei Conti Finanziario...'
        });


    //COG combobox
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaObiettiviGestionali',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetListaObiettiviGestionaliResult',
        fields: [
               { name: 'Id' },
               { name: 'Descrizione' }

               ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
      , reader: reader
    });

    store.setDefaultSort("Descrizione", "ASC");

    var currentYear = new Date().getYear();
    if (currentYear < 1900) currentYear += 1900;

    var parametri = { AnnoRif: currentYear, CapitoloRif: CapitoloRif };
    store.load({ params: parametri });

    var ComboCOGImp=  new Ext.form.ComboBox({
        fieldLabel: 'Codice Obiettivo Gestionale(*)',      
        displayField: 'Id',
        valueField: 'Id',
        listWidth: 300,
        width: 300,
        queryMode: 'local',
        store: store,
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
        readOnly: true,
        disabled: false,
        name: 'ComboCOG',
        id: 'ComboCOGImp',
        mode: 'local',
        triggerAction: 'all',
        emptyText: 'Seleziona Codice Obiettivo Gestionale...'
    });
                   
    var FormDetailCOGAndPdCF = new Ext.FormPanel({
        id: 'FormDetailCOGAndPdCF',
        frame: true,
        labelAlign: 'left',
        border: false,
        collapsible: true,   
        width: 455,
        items: [{
        	xtype: 'fieldset',
            id: 'detCapitolo',
            labelWidth: 100,
            defaultType: 'textfield',
            autoHeight: true,
            border: false,
            items: [          				
           	{
                fieldLabel: 'Bilancio',
                name: 'Bilancio',
                id: 'Bilancio',
                readOnly: true,    
                enable: false,            
                value:AnnoRif
            },{
                fieldLabel: 'Capitolo',
                name: 'Capitolo',
                id: 'Capitolo',
                readOnly: true,
                enabled: false,
                value:CapitoloRif
            },
            ComboCOGImp,
            ComboPdCF,
            {  
                 xtype:'hidden',
                 name:'id_prog',
                 value:idProg 
            }]
           } 
           ]                 
        });
                         
        var popup = new Ext.Window({
				            title: 'Modifica COG/Piano dei Conti Finanziario',
				            width: 455,
				            height: 230,
				            layout: 'fit',
				            plain: true,
				            buttonAlign: 'right',
				            maximizable: false,
				            enableDragDrop:true,
				            collapsible: false,
				            modal:true,
				            closable: true,
				            buttons: [{
			                    text: 'Salva',
			                    id: 'btnSalvaCOGAndPdCF'
                            },
		                    {
		                        text: 'Annulla',
		                        id: 'btnAnnullaCOGAndPdCF'
                            }]
        });

	  popup.add('FormDetailCOGAndPdCF');
      
      Ext.getCmp('ComboCOGImp').setValue(codValue);
      Ext.getCmp('ComboPdCFImp').setValue(codPdCFValue);
     
	  popup.doLayout()

	  Ext.getCmp('btnAnnullaCOGAndPdCF').on('click', function() {
	      popup.close();
	  });

	  Ext.getCmp('btnSalvaCOGAndPdCF').on('click', function() {
		  
	    if ((Ext.get('ComboCOGImp').dom.value.indexOf("Seleziona") != -1 ) || (Ext.get('ComboCOGImp').dom.value == '')){
                Ext.MessageBox.show({  
                    title: 'COG',
                    msg: 'Il campo Codide Obiettivo Gestionale non &egrave; stato valorizzato!',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
        } else if ((Ext.get('ComboPdCFImp').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboPdCFImp').dom.value == '')) {
                Ext.MessageBox.show({
                    title: 'Piano dei Conti Finanziario',
                    msg: 'Il campo Piano dei Conti Finanziario non &egrave; stato valorizzato!',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
        } else {
          Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
                    FormDetailCOGAndPdCF.getForm().timeout = 100000000;
                    FormDetailCOGAndPdCF.getForm().submit({
                        url: 'ProcAmm.svc/AssegnaCOGePdCF',
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
                title: 'Modifica COG/Piano dei Conti Finanziario',
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
	            title: 'Modifica COG/Piano dei Conti Finanziario',
	            msg: msg,
	            buttons: Ext.MessageBox.OK,
	            icon: Ext.MessageBox.INFO,
	            fn: function(btn) {
	              Ext.getCmp('GridRiepilogo').getStore().reload();
	              popup.close();
	              
	              actionDeleteImpegno.setDisabled(true);
	              actionCompilaLiquidazioni.setDisabled(true);
	              actionConfermaImpegno.setDisabled(true);
	              actionModificaCOGAndPdCFImp.setDisabled(true);
	              actionAddImpegno.setDisabled(false);                                                                            
            actionAddFromFile.setDisabled(false);
	            }
	        });
	    } // FINE SUCCESS
                      
         }) // FINE SUBMIT
          }     
					           
	  }) //fine onclick
        popup.show();                
     }
    
    
    var actionModificaCOGAndPdCFImp = new Ext.Action({
        text: 'Modifica COG/Piano dei Conti Finanziario',
        tooltip: 'Modifica COG e/o Piano dei Conti Finanziario',
        iconCls: 'add',
        handler: function() {
          
           var GridImpegni = Ext.getCmp('GridRiepilogo');
           var flag=true;
             Ext.each(GridImpegni.getSelectionModel().getSelections(), function(rec) {
   	             if (flag){
   	                 generaFormCOGAndPdCF(rec.data.ID, rec.data.Codice_Obbiettivo_Gestionale,
   	                               rec.data.PianoDeiContiFinanziario,
   	                               rec.data.Bilancio,rec.data.Capitolo);
   	                flag=false;
   	             }
   	          
              });
          }
    });
    //COG


var actionGeneraImpPluri = new Ext.Action({
    text: 'Crea Impegno',
    tooltip: 'Crea impegno per le fatture presenti',
    handler: function () {
        var mostraAnno = false;
        InitFormCapitoliImpegno(mostraAnno, false);
    },
    iconCls: 'add'
});

    var myPanelFattureLiq = new Ext.FormPanel({
        id: 'myPanelFattureLiq',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        width: 750,
        xtype: "form",
        title: "Fatture Documento",
        tbar: [actionGeneraImpPluri]
    });
    
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanel = new Ext.FormPanel({
    id:'myPanel',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px;',
    collapsible: true,
    width: 750,
    xtype: "form",
    title: "Impegni",
    tbar: [actionAddFromFile, actionAddImpegno, actionDeleteImpegno, actionBeneficiariImpegni, actionCompilaLiquidazioni, actionConfermaImpegno, actionModificaCOGAndPdCFImp]
    //tbar: [actionAddImpegno, actionDeleteImpegno, actionBeneficiariImpegni, actionCompilaLiquidazioni, actionConfermaImpegno, actionModificaCOGAndPdCFImp]
});


//DEFINISCO L'AZIONE Cancella DELLA GRIGLIA LIQUDIAZIONI CONTESTUALI da preimp
var actionCancellaLiq = new Ext.Action({
    text: 'Cancella Liquidazione',
    tooltip: 'Cancella Liquidazione selezionata',
    iconCls: 'remove',
    handler: function() {
        var GridLiq = Ext.getCmp('GridLiquidazioniContestuali');
        var storeGridLiq = Ext.getCmp('GridLiquidazioniContestuali').getStore();
        Ext.each(GridLiq.getSelectionModel().getSelections(), function(rec) {
            if (rec.data['ID'] != 0)
                EliminaLiquidazioneDaPreImpegno(rec.data['NumPreImp'], rec.data['ID']);
            storeGridLiq.remove(rec);
        });


        try {
            if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                Ext.getCmp('GridFattureLiquidazione').getStore().reload();
                Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            }
        } catch (ex) { }
        
        //synchronize 'GRIGLIA LIQUIDAZIONI CONTESTUALI' tool bar buttons to new rows status
        actionBeneficiari2.setDisabled(true);
        if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
            Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
            Ext.getCmp('myPanelLiqContestuali').doLayout();
        }
        actionConfermaLiquidazione2.setDisabled(true);
        actionRegistraLiq.setDisabled(true);
        actionCancellaLiq.setDisabled(true);
        actionShowFattureLiquidazione.setDisabled(true);
    },
    iconCls: 'remove'
});

function registraLiquidazioniContestuali () {

    var storeGridLiq = Ext.getCmp('GridLiquidazioniContestuali').getStore();
    var json = '';
    storeGridLiq.each(function (storeGridLiq) {
        json += Ext.util.JSON.encode(storeGridLiq.data) + ',';
    });
    json = json.substring(0, json.length - 1);
    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
    Ext.getDom('LiquidazioniCapitoli').value = json;
    myPanelLiqContestuali.getForm().timeout = 100000000;
    myPanelLiqContestuali.getForm().submit({
        url: 'ProcAmm.svc/SetLiquidazioni' + window.location.search,
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
                                    title: 'Gestione Liquidazioni',
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
                                var msg = 'Aggiornamento Liquidazioni effettuato con successo!';
                                var liquidazione = undefined;
                                var progLiq = "";
                                try {
                                    progLiq = response.result.progLiquidazione;
                                    Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();

                                } catch (ex) { }

                                Ext.MessageBox.show({
                                    title: 'Gestione Liquidazioni',
                                    msg: msg,
                                    buttons: Ext.MessageBox.OK,
                                    icon: Ext.MessageBox.INFO,
                                    fn: function (btn) {
                                        if (isDocumentoConFatture) {
                                            var storeGridLiq = Ext.getCmp('GridLiquidazioniContestuali').getStore();
                                            storeGridLiq.each(function (rec) {
                                                if (progLiq == rec.data.ID) {
                                                    liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };

                                                    var index = Ext.getCmp('GridLiquidazioniContestuali').getStore().indexOf(rec);
                                                    Ext.getCmp('GridLiquidazioniContestuali').getSelectionModel().selectRow(index);
                                                    Ext.getCmp('GridLiquidazioniContestuali').getView().refresh();
                                                }
                                            });

                                            if (liquidazione != undefined) {
                                                //var grid = buildPanelFattureLiquidazione(liquidazione);
                                                //Ext.getCmp('myPanelLiqContestuali').add(grid);
                                                //Ext.getCmp('myPanelLiqContestuali').doLayout();
                                            }

                                        }

                                        return;
                                    }
                                });

                            } // FINE SUCCESS

    }) // FINE SUBMIT
}

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUDIAZIONI CONTESTUALI
var actionRegistraLiq = new Ext.Action({
    text: 'Registra',
    tooltip: 'Registra la liquidazione selezionata',
    id: 'btnRegistraLiquidazione',
    handler: registraLiquidazioniContestuali,
    iconCls: 'add'
});

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanelLiqContestuali = new Ext.FormPanel({
    id:'myPanelLiqContestuali',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Liquidazioni sugli impegni del presente provvedimento",
	items : [
	  	 {
            id: "LiquidazioniCapitoli",
            xtype: "hidden"
	     }]
});

var labelNumPreImp = new Ext.form.Label({
						  text: 'Numero Prenotazione PreImpegno: ',
						  id: 'labelNumPreImp' 
						 });    


  
var ComboPreimp = new Ext.form.ComboBox({
fieldLabel: 'NumeroPreImpegno',
displayField: 'NumPreImp',
valueField: 'NumPreImp',
id: 'ComboPreimp',
       name: 'ComboPreimp',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Preimpegni ...',
        mode:'local'

});
     

var ComboCOGImp=  new Ext.form.ComboBox({
      fieldLabel: 'Cod.Ob.Gest(*)',
       displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOG',
        id: 'ComboCOGImp',
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona COG ...'

    });
     
     

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
var panelPreImp = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumPreImp,
									ComboPreimp
								 ]
								}); 
								



//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliImpegno(mostraAnno, lockPluriennale) {

	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelImpegno = new Ext.Panel({
								xtype : "panel",
								columnWidth: 1,
								bodyStyle: 'margin-bottom: 10px'
				}); 			
						 
	
	
    ComboCOGImp =  new Ext.form.ComboBox({
      fieldLabel: 'Cod.Ob.Gest(*)',
       displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOGImp',
        id: 'ComboCOGImp',
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona COG ...'

    });
     	

  var currentYear = new Date().getYear();
  if (currentYear < 1900) currentYear += 1900;

  var storeAnniBilancio = new Ext.data.SimpleStore({
      fields: ['value', 'description'],
      data: [[currentYear, currentYear],
      [currentYear+1, currentYear+1],
      [currentYear+2, currentYear+2]],
      autoLoad: true
  });			

    var testoBottoneSalva = "Salva";
    if (lockPluriennale) {
        testoBottoneSalva = "Genera impegno e liquidazione";
    } else {
        testoBottoneSalva = "Genera impegno";
    }

    //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
    var formCapitoliImpegno = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        title: 'Elenco dei Capitoli',
        //width: 800,
        width: 900,       
        layout: 'absolute',
        monitorValid: true,
        buttons: [{
            text: testoBottoneSalva,            
            id: 'btnCreaImpegno',
            handler: creaImpegno,
            formBind: true
        }, {
            text: 'Annulla',
            handler: function (btn, evt) {
                listaFatturaToStartImpegno = [];
                popupImpegno.close();
            }
        }],
        items: [
          capitoloPanelImpegno,
      {
          xtype: 'panel',
          title: '<span style="font-size: 0.8em">Seleziona da Preimpegno</span>',
          width: 752,
          //95
          height: 165,
          frame: true,
          layout: 'absolute',
          x: 0,
          y: 160,
          items: [
          {
              xtype: 'label',
              text: 'Bilancio:',
              x: 0,
              y: 0,
              width: 100,
              style: 'padding-top: 5px'
          }, {
              xtype: 'combo',
              displayField: 'description',
              valueField: 'value',
              triggerAction: 'all',
              selectOnFocus: true,
              typeAhead: true,
              mode: 'local',
              store: storeAnniBilancio,
              name: 'annoBilancioPreImp',
              id: 'annoBilancioPreImp',
              queryMode: 'local',
              editable: false,
              disabled: true,
              width: 132,
              listWidth: 132,
              x: 105,
              y: 0,
              listeners: {
                  select: function (combo, record) {
                      var gridCapitoli = Ext.getCmp('GridCapitoli');
                      if (gridCapitoli.getSelectionModel().hasSelection()) {
                          var selectedRow = gridCapitoli.getSelectionModel().getSelections()[0];

                            if (selectedRow != null && selectedRow != undefined) {
                                var params = {
                                    AnnoRif: record.data.value,
                                    CapitoloRif: selectedRow.data.Capitolo
                                };

                                storePreImpegni.reload({ params: params });
                                storePCFPreImp.reload({ params: params });
                                Ext.getCmp('preimpegno').reset();
                                Ext.getCmp('disponibilita_preimp').reset();
                                Ext.getCmp('potenziale_preimp').reset();
                                Ext.getCmp('obgestionale_preimp').reset();
                                Ext.getCmp('pcf_preimpegno').reset();
                                Ext.getCmp('impegno_da_preimp').reset();


                                abilitaBilancio(true);
                                
                            }
                        }
                    }
                }
            }, {
                xtype: 'label',
                text: 'Preimpegno:',
                x: 0,
                y: 30,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'combo',
                id: 'preimpegno',
                emptyText: 'Seleziona...',
                name: 'preimpegno',
                fieldLabel: 'NumeroPreImpegno',
                displayField: 'NumPreImp',
                loadingText: 'Attendere...',
                x: 105,
                y: 30,
                width: 132,
                listWidth: 350,
                editable: false,
                mode: 'local',
                triggerAction: 'all',
                allowBlank: true,
                store: storePreImpegni,
                listeners: {
                    select: function(combo, record) {

                        Ext.getCmp('disponibilita_preimp').setValue(eurRend(record.data.ImpDisp));
                        Ext.getCmp('potenziale_preimp').setValue(eurRend(record.data.ImpPotenzialePrenotato));
                        Ext.getCmp('pcf_preimpegno').setValue(record.data.PianoDeiContiFinanziario);

                        abilitaPreimpegno(true);
                        abilitaBilancio(false);
                    }
                },
                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">Preimpegno n. <b>{NumPreImp}</b><br/>{Oggetto_Impegno}<br/>da {TipoAtto} n. {NumeroAtto} del {DataAtto}</div></tpl>',
                style: 'background-color: #fffb8a;background-image:none;',
                disabled: true
            }, {
                xtype: 'label',
                text: 'Disponbilità:',
                x: 250,
                y: 0,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'textfield',
                y: 0,
                x: 357,
                name: 'disponilibita_preimp',
                id: 'disponibilita_preimp',
                style: 'opacity:.9;',
                width: 132,
                readOnly: true,
                disabled: true
            }, {
                xtype: 'label',
                text: 'Importo potenziale:',
                x: 502,
                y: 0,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'textfield',
                y: 0,
                x: 607,
                name: 'potenziale_preimp',
                id: 'potenziale_preimp',
                style: 'opacity:.9;',
                width: 132,
                readOnly: true,
                disabled: true
            }, {
                xtype: 'label',
                text: 'Piano dei conti fin.:',
                //                x: 0,
                //                y: 60,
                x: 502,
                y: 30,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'combo',
                id: 'pcf_preimpegno',
                emptyText: 'Seleziona...',
                name: 'pcf_preimpegno',
                fieldLabel: 'Piano dei conti fin.',
                displayField: 'Id',
                loadingText: 'Attendere...',
                //                x: 105,
                //                y: 60,
                x: 607,
                y: 30,
                width: 132,
                listWidth: 350,
                queryMode: 'local',
                editable: false,
                mode: 'local',
                triggerAction: 'all',
                store: storePCFPreImp,
                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                readOnly: true,
                disabled: true,
                style: 'background-color: #fffb8a; background-image:none;'
            }, {
                xtype: 'label',
                text: 'Ob. Gestionale:',
                x: 250,
                y: 30,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'combo',
                id: 'obgestionale_preimp',
                emptyText: 'Seleziona...',
                name: 'obgestionale_preimp',
                fieldLabel: 'Id',
                displayField: 'Id',
                x: 357,
                y: 30,
                width: 132,
                listWidth: 350,
                allowBlank: false,
                blankText: 'Indicare codice obiettivo gestionale',
                readOnly: true,
                queryMode: 'local',
                mode: 'local',
                triggerAction: 'all',
                store: storeCog,
                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                style: 'background-color: #fffb8a;background-image:none;',
                disabled: true
            }, {
                xtype: 'label',
                text: 'Importo impegno:',               
                x: 502,
                //                y: 30,
                y: 60,
                width: 100,
                style: 'padding-top: 5px'
            }, {
                xtype: 'numberfield',
                decimalSeparator: ',',
                invalidText: 'Importo non valido',
                x: 607,
                //                y: 30,
                y: 60,
                name: 'impegno_da_preimp',
                id: 'impegno_da_preimp',
                validateValue: validatePreimpegno,
                allowBlank: false,
                blankText: 'Indicare importo del preimpegno',
                style: 'background-color: #fffb8a;background-image:none;',
                width: 132,
                disabled: true
}]
            }, {
                xtype: 'panel',
                title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear) + "</span>",
                frame: true,
                layout: 'form',
                width: 250,
                x: 0,
                //                y: 260,
                y: 280,
                defaults: {
                    width: 132,
                    labelStyle: 'font-size:11px;'
                },
                buttons: [{
                    text: 'Pulisci',
                    id: 'button_pulisci1',
                    disabled: true,
                    handler: function() {
                        Ext.getCmp('cog_impegno1').reset();
                        Ext.getCmp('pcf_impegno1').reset();
                        Ext.getCmp('importo_impegno1').reset();
                    }
}],
                    labelWidth: 100,
                    items: [{
                        xtype: 'hidden',
                        name: 'bilancio1',
                        id: 'bilancio1',
                        value: currentYear,
                        readOnly: true,
                        disabled: true,
                        style: 'opacity:.9;'
                    }, {
                        fieldLabel: 'Disponibilità',
                        xtype: 'textfield',
                        name: 'disponilibita1',
                        id: 'disponibilita1',
                        style: 'opacity:.9;',
                        readOnly: true,
                        disabled: true
                    }, {
                        fieldLabel: 'Ob. Gestionale',
                        xtype: 'combo',
                        id: 'cog_impegno1',
                        emptyText: 'Seleziona...',
                        name: 'cog_impegno1',
                        readOnly: true,
                        disabled: true,
                        allowBlank: true,
                        validateValue: validateBilancio1,
                        blankText: 'Selezionare il codice obiettivo gestionale',
                        mode: 'local',
                        triggerAction: 'all',
                        displayField: 'Id',
                        listWidth: 350,
                        queryMode: 'local',
                        store: storeCog,
                        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                        style: 'background-color: #fffb8a; background-image:none;'
                    }, {
                        fieldLabel: 'Piano dei conti fin.',
                        xtype: 'combo',
                        id: 'pcf_impegno1',
                        emptyText: 'Seleziona...',
                        name: 'pcf_impegno1',
                        displayField: 'Id',
                        validateValue: validateBilancio1,
                        listWidth: 350,
                        queryMode: 'local',
                        store: storePCF1,
                        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                        readOnly: true,
                        disabled: true,
                        mode: 'local',
                        triggerAction: 'all',
                        style: 'background-color: #fffb8a; background-image:none;'
                    }, {
                        fieldLabel: 'Importo impegno 1',
                        xtype: 'numberfield',
                        decimalSeparator: ',',
                        name: 'importo_impegno1',
                        validateValue: validateBilancio1,
                        disabled: true,
                        decimalPrecision: 2,
                        id: 'importo_impegno1',
                        style: 'background-color: #fffb8a;background-image:none;'
}]
                    }, {
                        xtype: 'panel',
                        title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 1) + "</span>",
                        frame: true,
                        layout: 'form',
                        width: 250,
                        x: 251,
                        //                        y: 260,
                        y: 280,
                        defaults: {
                            width: 132,
                            labelStyle: 'font-size:11px;'
                        },
                        buttons: [{
                            text: 'Pulisci',
                            id: 'button_pulisci2',
                            disabled: true,
                            handler: function() {
                                Ext.getCmp('cog_impegno2').reset();
                                Ext.getCmp('pcf_impegno2').reset();
                                Ext.getCmp('importo_impegno2').reset();
                            }
}],
                            labelWidth: 100,
                            items: [{
                                xtype: 'hidden',
                                name: 'bilancio2',
                                id: 'bilancio2',
                                value: currentYear + 1,
                                readOnly: true,
                                disabled: true,
                                style: 'opacity:.9;'
                            }, {
                                fieldLabel: 'Disponibilità',
                                xtype: 'textfield',
                                name: 'disponilibita2',
                                id: 'disponibilita2',
                                style: 'opacity:.9;',
                                readOnly: true,
                                disabled: true
                            }, {
                                fieldLabel: 'Ob. Gestionale',
                                xtype: 'combo',
                                id: 'cog_impegno2',
                                emptyText: 'Seleziona...',
                                validateValue: validateBilancio2,
                                allowBlank: true,
                                blankText: 'Selezionare il codice obiettivo gestionale',
                                name: 'cog_impegno2',
                                readOnly: true,
                                disabled: true,
                                mode: 'local',
                                triggerAction: 'all',
                                displayField: 'Id',
                                listWidth: 350,
                                queryMode: 'local',
                                store: storeCog,
                                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                style: 'background-color: #fffb8a; background-image:none;'
                            }, {
                                fieldLabel: 'Piano dei conti fin.',
                                xtype: 'combo',
                                id: 'pcf_impegno2',
                                emptyText: 'Seleziona...',
                                name: 'pcf_impegno2',
                                validateValue: validateBilancio2,
                                readOnly: true,
                                disabled: true,
                                displayField: 'Id',
                                listWidth: 350,
                                queryMode: 'local',
                                store: storePCF2,
                                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                mode: 'local',
                                triggerAction: 'all',
                                style: 'background-color: #fffb8a; background-image:none;'
                            }, {
                                fieldLabel: 'Importo impegno 2',
                                xtype: 'numberfield',
                                decimalSeparator: ',',
                                name: 'importo_impegno2',
                                validateValue: validateBilancio2,
                                decimalPrecision: 2,
                                disabled: true,
                                id: 'importo_impegno2',
                                style: 'background-color: #fffb8a;background-image:none;'
}]
                            }, {
                                xtype: 'panel',
                                title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 2) + "</span>",
                                frame: true,
                                layout: 'form',
                                width: 250,
                                x: 502,
                                //                                y: 260,
                                y: 280,
                                defaults: {
                                    width: 132,
                                    labelStyle: 'font-size:11px;'
                                },
                                buttons: [{
                                    text: 'Pulisci',
                                    id: 'button_pulisci3',
                                    disabled: true,
                                    handler: function() {
                                        Ext.getCmp('cog_impegno3').reset();
                                        Ext.getCmp('pcf_impegno3').reset();
                                        Ext.getCmp('importo_impegno3').reset();
                                    }
}],
                                    labelWidth: 100,
                                    items: [{
                                        xtype: 'hidden',
                                        name: 'bilancio3',
                                        id: 'bilancio3',
                                        value: currentYear + 2,
                                        readOnly: true,
                                        disabled: true,
                                        style: 'opacity:.9;'
                                    }, {
                                        fieldLabel: 'Disponibilità',
                                        xtype: 'textfield',
                                        name: 'disponilibita3',
                                        id: 'disponibilita3',
                                        style: 'opacity:.9;',
                                        readOnly: true,
                                        disabled: true
                                    }, {
                                        fieldLabel: 'Ob. Gestionale',
                                        xtype: 'combo',
                                        id: 'cog_impegno3',
                                        emptyText: 'Seleziona...',
                                        name: 'cog_impegno3',
                                        validateValue: validateBilancio3,
                                        readOnly: true,
                                        disabled: true,
                                        allowBlank: true,
                                        blankText: 'Selezionare il codice obiettivo gestionale',
                                        mode: 'local',
                                        triggerAction: 'all',
                                        displayField: 'Id',
                                        listWidth: 350,
                                        queryMode: 'local',
                                        store: storeCog,
                                        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                        style: 'background-color: #fffb8a; background-image:none;'
                                    }, {
                                        fieldLabel: 'Piano dei conti fin.',
                                        xtype: 'combo',
                                        id: 'pcf_impegno3',
                                        emptyText: 'Seleziona...',
                                        name: 'pcf_impegno3',
                                        validateValue: validateBilancio3,
                                        displayField: 'Id',
                                        listWidth: 350,
                                        queryMode: 'local',
                                        store: storePCF3,
                                        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                        readOnly: true,
                                        disabled: true,
                                        mode: 'local',
                                        triggerAction: 'all',
                                        style: 'background-color: #fffb8a; background-image:none;'
                                    }, {
                                        fieldLabel: 'Importo impegno 3',
                                        xtype: 'numberfield',
                                        decimalSeparator: ',',
                                        validateValue: validateBilancio3,
                                        decimalPrecision: 2,
                                        name: 'importo_impegno3',
                                        disabled: true,
                                        id: 'importo_impegno3',
                                        style: 'background-color: #fffb8a;background-image:none;'
}]
}]
      });

    

    var popupImpegno = new Ext.Window({
        //y: 15,
        title: 'Aggiungi un nuovo impegno',
        id: 'popup_impegno',
        width: 782,
        //					            height: 590,
        height: 610,
        layout: 'fit',
        plain: true,
        buttonAlign: 'center',
        maximizable: true,
        tbar: ['->', actionHelpOnLine],
        enableDragDrop: false,
        collapsible: false,
        modal: true,
        closable: true  /* * toglie la croce per chiudere la finestra */
    });
    popupImpegno.add(formCapitoliImpegno);
    popupImpegno.doLayout(); //forzo ridisegno



    //PRENDO L'ANNO DALLA DATA ODIERNA
    var dtOggi = new Date();
    dtOggi.setDate(dtOggi.getDate());
    var fAnno = dtOggi.getFullYear();

    //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO


    var gridCapitoli = buildGridStartImpegno(fAnno, lockPluriennale);
    capitoloPanelImpegno.add(gridCapitoli);
    // SETTO panelImp INVISIBILE 
    panelPreImp.hide();

    popupImpegno.show();


}// fine InitFormCapitoli




//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function EliminaPreImpegno(NumPreImpegno, ID) {
    var params = { NumeroPreImpegno: NumPreImpegno, ID: ID };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/EliminaPreImpegno' + window.location.search,
        params: Ext.encode(params),
        method: 'POST',
        success: function (result, response, options) {
            try {
                if (Ext.getCmp('GridFattureImpegno') != undefined) {
                    Ext.getCmp('GridFattureImpegno').getStore().reload();
                }
                if (Ext.getCmp('GridLiquidazioniContestuali') != undefined) {
                    Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                }
                if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                    Ext.getCmp('GridFattureLiquidazione').getStore().reload();
                }
                if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                    Ext.getCmp('GridBeneficiariContestuali').getStore().reload();
                }

                if (Ext.getCmp('myPanel') != undefined) {
                    if (Ext.getCmp('GridFattureImpegno') != undefined) {
                        Ext.getCmp('myPanel').remove(Ext.getCmp('GridFattureImpegno'));
                    }
                    if (Ext.getCmp('panelInfoFatt') != undefined) {
                        Ext.getCmp('myPanel').remove(Ext.getCmp('panelInfoFatt'));
                    }
                    if (Ext.getCmp('GridBeneficiariImpegnoContestuali') != undefined) {
                        Ext.getCmp('myPanel').remove(Ext.getCmp('GridBeneficiariImpegnoContestuali'));
                    }
                    Ext.getCmp('myPanel').doLayout();
                }
                if (Ext.getCmp('myPanelLiqContestuali') != undefined) {
                    //                   if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                    //                       Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                    //                   }
                    if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                        Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                    }
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }
            } catch (ex) { }

            maskImp.hide();
        },
        failure: function (result, response, options) {
            maskImp.hide();
            //   var data = Ext.decode(result.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: Ext.decode(result.responseText).FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
        }
    });



}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LE FATTURE DALL IMPEGNO SELEZIONATO
function EliminaFatturaDaImpegno(listaFatture, statoOperazione, numPreImp) {

    var params = { listaFatture: listaFatture, statoOperazione: statoOperazione, numPreImp: numPreImp };

    Ext.Ajax.request({
    url: 'ProcAmm.svc/NotificaAttoFattura' + window.location.search,
        params: Ext.encode(params),
        headers: { 'Content-Type': 'application/json' },
        method: 'POST',
        success: function(response, options) {
                
            var data = Ext.decode(response.responseText);
            msg = data.NotificaAttoFatturaResult;
            
            Ext.MessageBox.show({
                title: 'Eliminazione Elementi Selezionati',
                msg: msg,
                buttons: Ext.MessageBox.OK
            });

            try {
                Ext.getCmp('GridFattureImpegno').getStore().reload();
            } catch (ex) { }
           

        },
        failure: function(response, result) {
        
           
	        
            var data = Ext.decode(response.responseText);

            msg = data.NotificaAttoFatturaResult;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: lstr_messaggio,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) { return }
            });
            
        }
    });
}


//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LE FATTURE DELLA LIQUIDAZIONE SELEZIONATA
function EliminaFatturaDaLiquidazione(listaFatture,statoOperazione,idLiquidazione) {

    var params = {listaFatture: listaFatture, statoOperazione: statoOperazione, idLiquidazione: idLiquidazione };

    Ext.Ajax.request({
        url: 'ProcAmm.svc/NotificaAttoFattura' + window.location.search,
        params: Ext.encode(params),
        headers: { 'Content-Type': 'application/json' },
        method: 'POST',
        success: function(response, options) {
            var msg = "";
            if (response.statusText == "OK") {
                msg = "Operazione completata con successo";
            } else {
                msg = "Errore Generico";
            }
            Ext.MessageBox.show({
                title: 'Eliminazione Fatture liquidazione',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                fn: function(btn) {
                    var gridFatture = Ext.getCmp('GridFattureLiquidazione');
                    if (gridFatture != undefined && gridFatture != null) {
                        var storeGridFatture = gridFatture.getStore();
                        storeGridFatture.reload();
                    }

                    var gridBen = Ext.getCmp('GridBeneficiariContestuali');
                    if (gridBen != undefined && gridBen != null) {
                        var storeGridBen = gridBen.getStore();
                        storeGridBen.reload();
                    } 
                }
            });

//            try {
//                Ext.getCmp('GridFattureLiquidazione').getStore().reload();
//            } catch (ex) { }

        },
        failure: function(response, result) {
            var data = Ext.decode(response.responseText);

            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) { return }
            });

        }
    });
}


//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function EliminaLiquidazioneDaPreImpegno(NumPreImpegno, ID) {
    var params = { NumeroPreImpegno: NumPreImpegno, ID: ID };
    
    Ext.Ajax.request({
    url: 'ProcAmm.svc/EliminaLiquidazioneDaPreImpegno' + window.location.search,
        params: Ext.encode(params),
        headers: { 'Content-Type': 'application/json' },
        method: 'POST',
        success: function(response, options) {
            maskImp.hide();
            // var numeroPreImpegnoResult = data.EliminaPreImpegnoResult;

            try {
                if (Ext.getCmp('GridLiquidazioniContestuali') != undefined) {
                    Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                }
                if (Ext.getCmp('myPanelLiqContestuali') != undefined) {
//                    if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
//                        Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
//                    }
                    if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                        Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                    }
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }
            } catch (ex) { }

        },
        failure: function(response, options) {
            maskImp.hide();
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

}

function addLiquidazioneContestualeDaSelezione() {
    
    var store = Ext.getCmp('GridLiquidazioniContestuali').getStore();
    
    var GridImpegni = Ext.getCmp('GridRiepilogo');
    Ext.each(GridImpegni.getSelectionModel().getSelections(), function (rec) {

        if (rec.data.ListaBeneficiari != undefined && rec.data.ListaBeneficiari.length > 0) {
            var TipoRecord = Ext.data.Record.create([
                   { name: 'Bilancio' }
                 , { name: 'UPB' }
                 , { name: 'MissioneProgramma' }
                 , { name: 'Capitolo' }
                 , { name: 'ImpPrenotatoLiq' }
                 , { name: 'NumPreImp' }
                 , { name: 'ID' }
                 , { name: 'Tipo' }
                 , { name: 'Stato' }
                 , { name: 'IdImpegno' }
                 , { name: 'PianoDeiContiFinanziario' }
                 , { name: 'ListaBeneficiari' }
            ]);

            var record = new TipoRecord({
                Bilancio: rec.data.Bilancio
                    , UPB: rec.data.UPB
                    , MissioneProgramma: rec.data.MissioneProgramma
                    , Capitolo: rec.data.Capitolo
                    , NumPreImp: rec.data.NumPreImp
                //, ImpPrenotatoLiq: 0
                    , ImpPrenotatoLiq: rec.data.ImpPrenotato
                    , ID: 0
                    , Tipo: ' '
                    , Stato: rec.data.Stato
                    , IdImpegno: rec.data.ID
                    , PianoDeiContiFinanziario: rec.data.PianoDeiContiFinanziario
                    , ListaBeneficiari : rec.data.ListaBeneficiari
            });
            store.insert(0, record);

            

            registraLiquidazioniContestuali();
        } else {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Per poter creare la liquidazione è necessario specificare il beneficiario dell\'impegno',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
        }
    });
}


//inserisce nello store delle liquidazioni  contestuali il record 
// costruitop a partire dai campi della form
function addLiquidazioneContestuale(fields){
	
	//alert(fields.Bilancio);
	var TipoRecord = Ext.data.Record.create([
    			{name: 'Bilancio'}
               ,{name: 'UPB' }
               ,{name: 'MissioneProgramma' }
               ,{name: 'Capitolo'}
               ,{name: 'ImpDisp'}
               ,{name: 'ImpPrenotato'}
               ,{name: 'NumImpegno' }
               ,{name: 'ImpPrenotatoLiq'}
               ,{name: 'AnnoPrenotazione'}
               ,{name: 'ID'}
               ,{ name: 'Tipo' }
               ,{ name: 'IdImpegno' }
			]);
      var record = new TipoRecord({
      			 Bilancio:fields.Bilancio
      		    , UPB: fields.UPB
      		    , MissioneProgramma: fields.MissioneProgramma
              	,Capitolo:fields.Capitolo
                ,ImpDisp:fields.ImpDisp
                ,ImpPrenotato:fields.ImpPrenotato
                ,NumImpegno:fields.NumImpegno
                ,ImpPrenotatoLiq:fields.ImpPrenotatoLiq
                ,AnnoPrenotazione:fields.AnnoPrenotazione
                ,ID:fields.ID
                 , Tipo: ' '
                , IdImpegno: fields.IdImpegno 
      });
		 var store = Ext.getCmp('GridLiquidazioniContestuali').getStore();
		
		store.insert(0,record); // alza l' evento 'add' che la griglia intercetta 
	}// end addLiquidazioneContestuale
	 function fnBtnVisLiq(fields) {
	     //Ext.getCmp('btnVisLiq').hide();
	     myPanelLiqContestuali.show();
	     addLiquidazioneContestuale(fields);
	 }

function VerificaDisponibilita() {
    try {
      
        Ext.get('ImpPrenotato').dom.value = 0;
        //var num=Ext.get('NumPreImp').dom.value;
        var num = Ext.get('ComboPreimp').dom.value;
        getImpDispPreImpegno(num);
        Ext.getCmp('btnAggiungi').show();
    } catch (ex) { 
    }
}




function buildComboCOGImp(AnnoRif, CapitoloRif) {
   var formCapitoliImpegno = Ext.getCmp('detCapitolo');
   
   // formCapitoliImpegno.remove(ComboCOGImp,true);
  ComboCOGImp.el.parent().parent().parent().remove()


   var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaObiettiviGestionali',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetListaObiettiviGestionaliResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }  
       
           
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("Descrizione", "ASC");


    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
      store.on({
        'load': {
            fn: function(store, records, options) {
                maskImp.hide();
            },
            scope: this
        }
    });
    store.load({ params: parametri });


   


 ComboCOGImp=  new Ext.form.ComboBox({
      fieldLabel: 'Cod.Ob.Gest(*)',
       displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOGImp',
        id: 'ComboCOGImp',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona COG ...'

    });
     
  formCapitoliImpegno.insert(12,ComboCOGImp);  
  formCapitoliImpegno.doLayout();
    ComboCOGImp.show();

   ComboCOGImp.on('select', function(record, index) { selectComboCOG(record, index)});
    return ComboCOGImp;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartImpegno(annoRif, lockPluriennale) {

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetElencoCapitoliAnno',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

	root: 'GetElencoCapitoliAnnoResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpDisp'},
           {name: 'ImpPrenotato'},
           {name: 'NumPrenotazione'},
           {name: 'AnnoPrenotazione'},
           { name: 'DescrCapitolo' },
           { name: 'CodiceRisposta' },
           { name: 'DescrizioneRisposta' }
           ]
	});
	
	var store = new Ext.data.Store({
		proxy:proxy
		,reader:reader
        , listeners: {
            'loadexception': function (proxy, options, response) {
                maskApp.hide();
                Ext.MessageBox.show({
                    title: 'ERRORE CAPITOLI',
                    msg: "Errore durante il caricamento dei capitoli:<br>" +
                         "'" + Ext.decode(response.responseText).FaultMessage + "'",
                    buttons: Ext.Msg.OK,
                    closable: false,
                    icon: Ext.MessageBox.ERROR
                });
            }
        }
	});

	var parametri = { AnnoRif: annoRif, tipoCapitolo:'0' };
    store.load({params:parametri});

    store.on({
   'load':{
      fn: function(store, records, options){
       maskImp.hide();
     },
      scope:this
  	 }
 	});
 	
 	function renderColor(val, p, record) {
 	    var color = '';
 	    if (record.data.CodiceRisposta == 0) {
 	        if (record.data.DescrizioneRisposta.toUpperCase().indexOf("NESSUN") > -1) {
 	            color = '#298A08';
 	        } else {
 	            color = '#FE9A2E';
 	        }
 	    }
 	    else {
 	        color = 'red';
 	    }
 	    return '<p style="font-size:12px;color:' + color + ';">' + record.data.DescrizioneRisposta + '</p>';
 	} 
 
//DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
     var ColumnModel = new Ext.grid.ColumnModel([
	                //{ header: "Bilancio", width: 60,dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
	                { header: "Capitolo", width: 60,dataIndex: 'Capitolo',  sortable: true, locked:false},
                    { header: "UPB", width: 50, dataIndex: 'UPB', sortable: true, hidden: true },
                    { header: "Missione.Programma", width: 80, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width: 300, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	                //,
	            	//{ renderer: eurRend,header: "Importo<br/>Potenziale",width: 60,dataIndex: 'ImpPotenzialePrenotato', sortable: true },
	                //{ renderer: eurRend,header: "Importo<br/>Disponibile",width: 60,dataIndex: 'ImpDisp', sortable: true }

	            	] );

	            	var grid = new Ext.grid.GridPanel({
	            	    id: 'GridCapitoli',
	            	    autoExpandColumn: 'Capitolo',
	            	    width: 754,
	            	    height: 150,
	            	    // title:'',
	            	    border: true,
	            	    viewConfig: { forceFit: true },
	            	    ds: store,
	            	    cm: ColumnModel,
	            	    stripeRows: true,
	            	    // istruzioni per abilitazione Drag & Drop		          
	            	    enableDragDrop: false,
	            	    // ddGroup: 'gridDDGroup',
	            	    // fine istruzioni per abilitazione Drag & Drop		          
	            	    sm: new Ext.grid.RowSelectionModel({
	            	        singleSelect: true,
	            	        loadMask: true,
	            	        listeners: {
	            	            selectionchange: function(selModel) {
	            	                var rows = selModel.getSelections();
	            	                if (rows.length == 1) {
	            	                    resetForm();
	            	                    Ext.getCmp('disponibilita1').setValue(eurRend(rows[0].data.ImpDisp));
	            	                    Ext.getCmp('disponibilita2').setValue('Attendere...');
	            	                    Ext.getCmp('disponibilita3').setValue('Attendere...');

	            	                    // setImpDisp(Ext.getCmp('disponibilita1'), rows[0].data.Capitolo, Ext.getCmp('bilancio1').getValue());
	            	                    setImpDisp(Ext.getCmp('disponibilita2'), rows[0].data.Capitolo, Ext.getCmp('bilancio2').getValue());
	            	                    setImpDisp(Ext.getCmp('disponibilita3'), rows[0].data.Capitolo, Ext.getCmp('bilancio3').getValue());

	            	                    var params = {
	            	                        AnnoRif: rows[0].data.Bilancio,
	            	                        CapitoloRif: rows[0].data.Capitolo
	            	                    };

	            	                    Ext.getCmp('annoBilancioPreImp').setValue(rows[0].data.Bilancio);

	            	                    storePreImpegni.reload({ params: params });
	            	                    storePCFPreImp.reload({ params: params });
	            	                    storeCog.reload({ params: params });
	            	                    storePCF1.reload({ params: params });

	            	                    var params2 = {
	            	                        AnnoRif: Ext.getCmp('bilancio2').getValue(),
	            	                        CapitoloRif: rows[0].data.Capitolo
	            	                    };

	            	                    storePCF2.reload({ params: params2 });

	            	                    var params3 = {
	            	                        AnnoRif: Ext.getCmp('bilancio3').getValue(),
	            	                        CapitoloRif: rows[0].data.Capitolo
	            	                    };

	            	                    storePCF3.reload({ params: params3 });

                        //stef
                        if (lockPluriennale) {
                            abilitaOnlyBilancioAnnoCorrente(lockPluriennale);

                            var totaleImportiDaLiquidare = 0;
                            Ext.each(listaFatturaToStartImpegno, function (fattura) {
                                totaleImportiDaLiquidare = totaleImportiDaLiquidare + fattura.ImportoFattDaLiquidare;
                            });
                            totaleImportiDaLiquidare = parseFloat(totaleImportiDaLiquidare).toFixed(2);

                            Ext.getCmp('impegno_da_preimp').setValue(totaleImportiDaLiquidare);
                            Ext.getCmp('importo_impegno1').setValue(totaleImportiDaLiquidare);
                        } else {
                            abilitaPreimpegno(false);
                            abilitaBilancio(true);
                        }

                    }
                }
            }
        })
    });

	return grid;
}


function setImpDisp(disponibilita,capImpDisp, bilancioImpDisp) {
    
   var ufficio =  Ext.get('Cod_uff_Prop').dom.value;
   var params = { Capitolo: capImpDisp,  Bilancio: bilancioImpDisp, ufficioProponente: ufficio  };

   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/GetImpDispCapitolo_post',
       params: Ext.encode(params),
       method: 'POST',
       success: function(response, options) {
           
           var data = Ext.decode(response.responseText);
           var ImpDisponibile = data.GetImpDispCapitolo_postResult.ImportoDisponibile;
           var potenziale = data.GetImpDispCapitolo_postResult.ImportoPotenziale;
           disponibilita.setValue(eurRend(ImpDisponibile));
           
           //rec.set('ImpDisp', ImpDisponibile)
           //rec.set('ImpPotenzialePrenotato', potenziale)
       },
       failure: function(response, options) {

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

}

function enableConfirmAction(selectedRows) {
    var retValue = false

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.Stato == 2))
        break;

    return retValue;
}

function enableBeneficiariAction(selectedRows) {
    var retValue = selectedRows.length==0 || selectedRows.length > 1;

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.ID == 0))
        break;

    return !retValue;
}

function enableFattureAction(selectedRows) {
    var retValue = selectedRows.length == 0 || selectedRows.length > 1;

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.ID == 0))
        break;

    return !retValue;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridRiep(isFatturePresenti) {
    isDocumentoConFatture = isFatturePresenti;
    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetImpegniRegistratiNonPerenti' + window.location.search,
    method:'GET'
    });

    var reader = new Ext.data.JsonReader({

        root: 'GetImpegniRegistratiNonPerentiResult',
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
           { name: 'Tipo' },
           { name: 'Stato' },
           { name: 'StatoAsString' },
           { name: 'Codice_Obbiettivo_Gestionale' },
           { name: 'PianoDeiContiFinanziario' },
           { name: 'ListaBeneficiari' },
           { name: 'HashTokenCallSic' },
           { name: 'HashTokenCallSic_Imp' }
        ]
    });


    var store = new Ext.data.GroupingStore({
        proxy: proxy,
        reader: reader,
        groupField: 'Capitolo',
        sortInfo: {
            field: 'Capitolo',
            direction: "ASC"
        }
    });

    store.setDefaultSort("Capitolo", "ASC");

 store.on({
   'load':{
      fn: function(store, records, options){
       maskImp.hide();
     },
      scope:this
  	 }
 	});

    var ufficio =  Ext.get('Cod_uff_Prop').dom.value;
    var parametri = { CodiceUfficio: ufficio };
     store.load({params:parametri});
   
	 var summary = new Ext.grid.GroupSummary();


	 var sm = new Ext.grid.CheckboxSelectionModel(
       {
           singleSelect: false,
 	        listeners: {
 	            rowselect: function(sm, row, rec) {
 	                var multiSelect = sm.getSelections().length > 1;

                   if (Ext.getCmp('GridBeneficiariImpegnoContestuali') != undefined) {
                       Ext.getCmp('myPanel').remove(Ext.getCmp('GridBeneficiariImpegnoContestuali'));
                       Ext.getCmp('myPanel').doLayout();
                   }


                   var currentYear = new Date().getYear();
                   if (currentYear < 1900) currentYear += 1900;

 	                actionAddImpegno.setDisabled(true);
                   actionAddFromFile.setDisabled(true);

                   if (rec.data.Stato == 2) {
                       actionDeleteImpegno.setDisabled(multiSelect);
                       actionBeneficiariImpegni.setDisabled(true);
                       actionCompilaLiquidazioni.setDisabled(true);
                       actionConfermaImpegno.setDisabled(false);
                       actionModificaCOGAndPdCFImp.setDisabled(multiSelect);
                   } else {
                       actionDeleteImpegno.setDisabled(multiSelect);
                       if (rec.data.Bilancio == currentYear) {
                           actionBeneficiariImpegni.setDisabled(multiSelect);
                           actionCompilaLiquidazioni.setDisabled(multiSelect);
                           actionBeneficiariImpegni.setDisabled(multiSelect);
                       } else {
                           actionCompilaLiquidazioni.setDisabled(true);
                           actionBeneficiariImpegni.setDisabled(multiSelect);
                       }
                       actionModificaCOGAndPdCFImp.setDisabled(multiSelect);
                   }
               },
               rowdeselect: function (sm, row, rec) {
                   var selectedRowsCount = sm.getSelections().length;

                   if (Ext.getCmp('GridBeneficiariImpegnoContestuali') != undefined) {
                       Ext.getCmp('myPanel').remove(Ext.getCmp('GridBeneficiariImpegnoContestuali'));
                       Ext.getCmp('myPanel').doLayout();
                   }

                   var currentYear = new Date().getYear();
                   if (currentYear < 1900) currentYear += 1900;

 	                actionAddImpegno.setDisabled(selectedRowsCount == 0 ? false : true);
                   actionAddFromFile.setDisabled(selectedRowsCount == 0 ? false : true);

                   if (selectedRowsCount == 1) {
                       if (sm.getSelected().data.Stato == 2) {
                           actionDeleteImpegno.setDisabled(false);
                           actionBeneficiariImpegni.setDisabled(true);
                           actionCompilaLiquidazioni.setDisabled(true);
                           actionModificaCOGAndPdCFImp.setDisabled(false);
                       } else {
                           actionDeleteImpegno.setDisabled(false);
                           if (sm.getSelected().data.Bilancio == currentYear) {
                               actionBeneficiariImpegni.setDisabled(false);
                               actionCompilaLiquidazioni.setDisabled(false);
                           } else {
                               actionBeneficiariImpegni.setDisabled(false);
                               actionCompilaLiquidazioni.setDisabled(true);
                           }

                           actionModificaCOGAndPdCFImp.setDisabled(false);
                       }
                   } else {
                       actionDeleteImpegno.setDisabled(true);
                       actionBeneficiariImpegni.setDisabled(true);
                       actionCompilaLiquidazioni.setDisabled(true);
                       actionModificaCOGAndPdCFImp.setDisabled(true);
                   }

 	                actionConfermaImpegno.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));


 	            }
 	        }
 	    }
 	);
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                {
		                    header: "Bilancio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true,
		                    summaryRenderer: function (v, params, data) { return '<b> Totale </b>'; }
		                },
		                { header: "Capitolo", dataIndex: 'Capitolo', sortable: true, locked: false },
	                    { header: "UPB", dataIndex: 'UPB', sortable: true, hidden: true },
	                    { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		            	{
		            	    renderer: eurRend, header: "Importo da Prenotare", align: 'right', dataIndex: 'ImpPrenotato', sortable: true
		            	 , summaryType: 'sum'
		            	},
		            	{ header: "Numero PreImpegno", dataIndex: 'NumPreImp', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true} ,
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true}  ,
		            	{ header: "COG", dataIndex: 'Codice_Obbiettivo_Gestionale', sortable: true },
		            	{ header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario', sortable: true },
		            	{ header: "Stato", dataIndex: 'StatoAsString' }		            			            			            	
		    ]);
		   		            	
		    var GridRiep = new Ext.grid.GridPanel({ 
				    	id: 'GridRiepilogo',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',
				       plugins: summary,
                        loadMask: true,
				        view: new Ext.grid.GroupingView({
				            forceFit:true,
                            showGroupName: false,
                            enableNoGroups:true, // REQUIRED!
                            hideGroupedColumn: true,
                            enableGroupingMenu: true
                        })
	          });
	           actionDeleteImpegno.setDisabled(true); 
    actionBeneficiariImpegni.setDisabled(true);
 	           actionCompilaLiquidazioni.setDisabled(true);
 	           actionConfermaImpegno.setDisabled(true);
 	           actionModificaCOGAndPdCFImp.setDisabled(true);
 return GridRiep; 
}

//FUNZIONE CHE RIEMPIE IL CAMPO "DISPONIBILITA' PREIMPEGNO" NEL PANEL "NUMERO PREIMPEGNO"
function getImpDispPreImpegno(NumPreImpegno) {
	var params = { NumeroPreImpegno: NumPreImpegno};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
         url: 'ProcAmm.svc/GetImpDispPreImpegno',
        params: Ext.encode(params),
        method: 'POST',
        success: function(response, options) {
            maskImp.hide();
            var data = Ext.decode(response.responseText);
            //data.GetImpDispPreImpegnoResult.ImportoDisponibile;
            // nonostante impDisp sia lo stesso nome dato alla colonna della griglia, viene modificato solo nel Dettaglio Capitolo perchè nella griglia c'è lo store
            Ext.get('ImpDisp').dom.value = data.ImpDisp;
            Ext.get('Bilancio').dom.value = data.Bilancio;
            Ext.get('UPB').dom.value = data.UPB;
            Ext.get('MissioneProgramma').dom.value = data.MissioneProgramma;
            Ext.get('Capitolo').dom.value = data.Capitolo;
            Ext.get('ImpPrenotato').dom.value = data.ImpPrenotato;
            Ext.get('TipoAtto').dom.value = data.TipoAtto;
            Ext.get('NumeroAtto').dom.value = data.NumeroAtto;
            Ext.get('DataAtto').dom.value = data.DataAtto;
            Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)  ;
            Ext.get('Oggetto_Impegno').dom.value = data.Oggetto_Impegno;
 
           },
	        failure: function(response, result) {
	            maskImp.hide();
//	            	Ext.MessageBox.show({
//	                            title: 'Errore',
//	  				            msg: 'Numero impegno non trovato',
//	                            buttons: Ext.MessageBox.OK,
//	                            icon: Ext.MessageBox.ERROR,
//	                            fn: function(btn) { 			return }
	            //	  					 });
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
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "Liquidazioni Contestuali"
function buildGridLiquidazioniContestuali(LeggoRegistrate, isFatturePresenti) {
    isDocumentoConFatture = isFatturePresenti;
    var store;
    if (LeggoRegistrate == true) {
        
        //url: 'ProcAmm.svc/GetLiquidazioniRegistrateContestuali'+ window.location.search,
      var proxy = new Ext.data.HttpProxy({
	    url: 'ProcAmm.svc/GetLiquidazioniRegistrateContestualiNonPerenti'+ window.location.search,
        method:'GET'
        }); 
	    var reader = new Ext.data.JsonReader({

	    root: 'GetLiquidazioniRegistrateContestualiNonPerentiResult',
	    fields: [
               {name: 'Bilancio'},
               { name: 'UPB' },
               { name: 'MissioneProgramma' },
               {name: 'Capitolo'},
               {name: 'ImpDisp'},
               {name: 'ImpPrenotato'},
               { name: 'NumImpegno' },
               { name: 'ImpPrenotatoLiq' },
               { name: 'AnnoPrenotazione' },
               { name: 'NumPreImp' },
               {name: 'ID'},
               {name: 'Tipo'},
               {name: 'Stato'},
               { name: 'StatoAsString' },
                { name: 'IdImpegno' },
                { name: 'PianoDeiContiFinanziario' },
                { name: 'HashTokenCallSic' },
                { name: 'IdDocContabileSic' }
          ]
	    });

        var store = new Ext.data.GroupingStore({
            proxy: proxy,
            reader: reader,
            groupField: 'NumPreImp',
            sortInfo: {
                field: 'NumPreImp',
                direction: "ASC"
            }
        });

        var ufficio =  Ext.get('Cod_uff_Prop').dom.value;
        var parametri = { CodiceUfficio: ufficio };
        store.load({params:parametri});
            store.on({
           'load':{
              fn: function(store, records, options){
               maskImp.hide();
             },
              scope:this
  	         }
 	        });
        
    }else{
        store=Ext.getCmp('GridRiepilogo').getStore(); 
    }
         
	var summary = new Ext.grid.GroupSummary();
	var sm = new Ext.grid.CheckboxSelectionModel(
    {
        singleSelect: false,

        listeners: {
            rowselect: function(sm, row, rec) {
                var multiSelect = sm.getSelections().length > 1;

                if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                    Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }

                if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                    Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }

                //abilita il tasto beneficiario se e solo se esiste una liquidazione registrata nel db e non ho una selezione multipla
                actionBeneficiari2.setDisabled(!enableBeneficiariAction(sm.getSelections()));

                if (isDocumentoConFatture) {
                    actionShowFattureLiquidazione.setDisabled(!enableFattureAction(sm.getSelections()));
                } else {
                    actionShowFattureLiquidazione.setDisabled(true);
                }

                if (rec.data.Stato == 2) {
                    actionRegistraLiq.setDisabled(true);
                    actionCancellaLiq.setDisabled(multiSelect);
                    actionConfermaLiquidazione2.setDisabled(false);
                } else {
                    actionRegistraLiq.setDisabled(multiSelect);
                    actionCancellaLiq.setDisabled(multiSelect);
                }
            },
            rowdeselect: function(sm, row, rec) {
                var selectedRowsCount = sm.getSelections().length;

                if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                    Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }
                if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                    Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                    Ext.getCmp('myPanelLiqContestuali').doLayout();
                }
                actionBeneficiari2.setDisabled(!enableBeneficiariAction(sm.getSelections()));
                if (isDocumentoConFatture) {
                    actionShowFattureLiquidazione.setDisabled(!enableFattureAction(sm.getSelections()));
                } else {
                    actionShowFattureLiquidazione.setDisabled(true);
                }

                if (selectedRowsCount == 1) {
                    if (sm.getSelected().data.Stato == 2) {
                        actionRegistraLiq.setDisabled(true);
                        actionCancellaLiq.setDisabled(false);
                    } else {
                        actionRegistraLiq.setDisabled(false);
                        actionCancellaLiq.setDisabled(false);
                    }
                } else {
                    actionRegistraLiq.setDisabled(true);
                    actionCancellaLiq.setDisabled(true);
                }
                actionConfermaLiquidazione2.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));
            }
        }
    }
    );
 	    var ColumnModel = new Ext.grid.ColumnModel({
    
    columns: [
	                    sm,
                            {
                                header: "Bilancio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true,
	                        summaryRenderer: function(v, params, data) { return '<b> Totale </b>'; } 
	                    },
		                { header: "Capitolo", dataIndex: 'Capitolo', sortable: true },
						{ header: "UPB", dataIndex: 'UPB', sortable: true, hidden: true },
						{ header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
                            {
                                header: "Importo da Liquidare", align: 'right', dataIndex: 'ImpPrenotatoLiq', id: 'importoDaLiquidare',
						    css: 'background-color: #fffb8a;',
						    sortable: true, summaryType: 'sum',
						    renderer: eurRend,
						    editor: new Ext.form.NumberField({
						        decimalSeparator: ',',
						        allowBlank: true,
						        allowNegative: false
						    })
						},						
						{ header: "Numero PreImpegno", dataIndex: 'NumPreImp', id: 'NumPreImp', sortable: false },
						{ header: "ID", dataIndex: 'ID', id: 'ID', sortable: false, hidden: true },
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true} ,
		            	{ header: "Stato", dataIndex: 'StatoAsString' },		            	
		            	{ header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario', sortable: true },
		            	{ header: "IdImpegno", dataIndex: 'IdImpegno', hidden: true}
						
		 ]
     });
		
		
	    var GridLiq = new Ext.grid.EditorGridPanel({
	                    //title:'Liquidazioni Contestuali agli Impegni',
                	    id: 'GridLiquidazioniContestuali',
				    	cls: 'row-summary-style',
				        ds: store,
				        sm:sm,
				 		colModel :ColumnModel,						
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',				       
				        plugins:[ summary],
				        loadMask: true,
				        tbar: [actionRegistraLiq, actionCancellaLiq, actionConfermaLiquidazione2, actionShowFattureLiquidazione, actionBeneficiari2],
				        view: new Ext.grid.GroupingView({
				            forceFit:true,
                            showGroupName: false,
                            enableNoGroups:true, // REQUIRED!
                            hideGroupedColumn: true,
                            enableGroupingMenu: true
                        })
	          });
	   		  actionRegistraLiq.setDisabled(true); 
 	          actionCancellaLiq.setDisabled(true);
 	          actionConfermaLiquidazione2.setDisabled(true);
 	          actionBeneficiari2.setDisabled(true);
 	          actionShowFattureLiquidazione.setDisabled(true);
 	          
 	          
 	          
 return GridLiq; 
}

//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaPreImpegno(ImpegniInfo) {
    var CodiceUfficio = Ext.get('Cod_uff_Prop').dom.value;

    var params = { ImpegniInfo: ImpegniInfo, CodiceUfficio: CodiceUfficio };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaPreImpegno',
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            mask.hide();
            Ext.getCmp('GridRiepilogo').getStore().reload();
            try {
                Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                actionDeleteImpegno.setDisabled(true);
                actionBeneficiariImpegni.setDisabled(true);
                actionCompilaLiquidazioni.setDisabled(true);
                actionConfermaImpegno.setDisabled(true);
                actionModificaCOGAndPdCFImp.setDisabled(true);
                actionAddImpegno.setDisabled(false);                                                                                
                actionAddFromFile.setDisabled(false);
            } catch (ex) { }
        },
        failure: function(response, result, options) {
            mask.hide();
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    if (data.FaultCode == -2) { //this fault code means that at least a confirmation is succeded
                        maskImp.hide();
                        Ext.getCmp('GridRiepilogo').getStore().reload();
                        try {
                            Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                            actionDeleteImpegno.setDisabled(true);
                            actionBeneficiariImpegni.setDisabled(true);
                            actionCompilaLiquidazioni.setDisabled(true);
                            actionConfermaImpegno.setDisabled(true);
                            actionModificaCOGAndPdCFImp.setDisabled(true);
                            actionAddImpegno.setDisabled(false);   
                            actionAddFromFile.setDisabled(false);
                        } catch (ex) { }
                    }
                }
            });
        }
    });
}

function ConfermaPreImpegno(ID) {
    var CodiceUfficio = Ext.get('Cod_uff_Prop').dom.value;

	var params = {ID:ID,CodiceUfficio:CodiceUfficio};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaPreImpegno',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           maskImp.hide();
           Ext.getCmp('GridRiepilogo').getStore().reload();
           try{
				 Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
		    }catch(ex){}
       },
      failure: function(response,result, options) {
            maskImp.hide();
            //Ext.decode(result.responseText).FaultMessage
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
}

//FUNZIONE CHE CANCELLA FISICAMENTE GLI IMPEGNI NON CONFERMATI
function EliminaImpegnoNonAncoraConfermato(NumPreImpegno,ID) {
	var params = { NumeroPreImpegno: NumPreImpegno, ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/EliminaImpegnoNonAncoraConfermato',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
               try {
                   if (Ext.getCmp('GridFattureImpegno') != undefined) {
                       Ext.getCmp('GridFattureImpegno').getStore().reload();
                   }
                   if (Ext.getCmp('GridLiquidazioniContestuali') != undefined) {
                       Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                   }
                   if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                       Ext.getCmp('GridFattureLiquidazione').getStore().reload();
                   }
                   if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                       Ext.getCmp('GridBeneficiariContestuali').getStore().reload();
                   }
                   if (Ext.getCmp('myPanel') != undefined) {
                       if (Ext.getCmp('GridFattureImpegno') != undefined) {
                           Ext.getCmp('myPanel').remove(Ext.getCmp('GridFattureImpegno'));
                           Ext.getCmp('myPanel').doLayout();
                       }
                   }
                   if (Ext.getCmp('myPanelLiqContestuali') != undefined) {
                       if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                           Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                       }
                       if (Ext.getCmp('GridBeneficiariContestuali') != undefined) {
                           Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridBeneficiariContestuali'));
                       }
                       Ext.getCmp('myPanelLiqContestuali').doLayout();
                   }
               } catch (ex) { }

               maskImp.hide();
       },
       failure: function(result, response, options) {
            maskImp.hide();
            Ext.MessageBox.show({
               title: 'Errore',
               msg: response.result.FaultMessage,
               buttons: Ext.MessageBox.OK,
               icon: Ext.MessageBox.ERROR,
               fn: function(btn) { return }
           });
       }
   });
}
//FUNZIONE CHE CONFERMA I DATI di LIQUIDAZIONE INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaLiquidazioneContestuale(LiquidazioniInfo) {
    var params = { LiquidazioniInfo: LiquidazioniInfo };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaLiquidazione',
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            mask.hide();
            Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
            actionRegistraLiq.setDisabled(true);
            actionCancellaLiq.setDisabled(true);
            actionConfermaLiquidazione2.setDisabled(true);
            actionBeneficiari2.setDisabled(true);
            actionShowFattureLiquidazione.setDisabled(true);
 	          
        },
        failure: function(response, result, options) {
            mask.hide();            
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    if (data.FaultCode == 2) { //this fault code means that at least a confirmation is succeded
                        Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                        actionRegistraLiq.setDisabled(true);
                        actionCancellaLiq.setDisabled(true);
                        actionConfermaLiquidazione2.setDisabled(true);
                        actionBeneficiari2.setDisabled(true);
                        actionShowFattureLiquidazione.setDisabled(true);
 	          
                    }
                }
            });
        }
    });
}

function ConfermaLiquidazioneContestuale(ID) {
	var params = {ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaLiquidazione',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           maskImp.hide();
           Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
       },
      failure: function(response,result, options) {
            maskImp.hide();
            //Ext.decode(result.responseText).FaultMessage
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
}

function NascondiColonne(tipo){
    var index = Ext.getCmp('GridRiepilogo').colModel.getColumnCount(false);
    index = index-1;
    Ext.getCmp('GridRiepilogo').colModel.setHidden(index, true);
    actionConfermaImpegno.hide();
    if (tipo == 1) {
        actionCompilaLiquidazioni.hide();
    }
}

function NascondiColonneLiqContestuali(){
    var index = Ext.getCmp('GridLiquidazioniContestuali').colModel.getColumnCount(false);
    index = index-1;
    Ext.getCmp('GridLiquidazioniContestuali').colModel.setHidden(index,true) ;
    actionConfermaLiquidazione2.hide();
}

/*
 * Rende editabile solo la parte del preimpegno 
 */
function abilitaPreimpegno(abilita) {
    if (abilita) {
        Ext.getCmp('pcf_preimpegno').enable();
        Ext.getCmp('obgestionale_preimp').enable();
        Ext.getCmp('impegno_da_preimp').enable();
    } else {
        Ext.getCmp('preimpegno').clearValue();
        Ext.getCmp('obgestionale_preimp').clearValue();
        Ext.getCmp('obgestionale_preimp').disable();
        Ext.getCmp('pcf_preimpegno').clearValue();
        Ext.getCmp('pcf_preimpegno').disable();
        Ext.getCmp('impegno_da_preimp').disable();
    }
}

/*
 * Rende editabile solo la parte dell'impegno pluriennale
 */
function abilitaBilancio(abilita) {
    if (abilita) {

        Ext.getCmp('cog_impegno1').enable();
        Ext.getCmp('pcf_impegno1').enable();
        Ext.getCmp('importo_impegno1').enable();
        Ext.getCmp('button_pulisci1').enable();

        Ext.getCmp('cog_impegno2').enable();
        Ext.getCmp('pcf_impegno2').enable();
        Ext.getCmp('importo_impegno2').enable();
        Ext.getCmp('button_pulisci2').enable();

        Ext.getCmp('cog_impegno3').enable();
        Ext.getCmp('pcf_impegno3').enable();
        Ext.getCmp('importo_impegno3').enable();
        Ext.getCmp('button_pulisci3').enable();
    } else {

        Ext.getCmp('cog_impegno1').clearValue();
        Ext.getCmp('cog_impegno1').disable();
        Ext.getCmp('pcf_impegno1').clearValue();
        Ext.getCmp('pcf_impegno1').disable();
        Ext.getCmp('importo_impegno1').disable();
        Ext.getCmp('importo_impegno1').reset();
        Ext.getCmp('button_pulisci1').disable();

        Ext.getCmp('cog_impegno2').clearValue();
        Ext.getCmp('cog_impegno2').disable();
        Ext.getCmp('pcf_impegno2').clearValue();
        Ext.getCmp('pcf_impegno2').disable();
        Ext.getCmp('importo_impegno2').disable();
        Ext.getCmp('importo_impegno2').reset();
        Ext.getCmp('button_pulisci2').disable();

        Ext.getCmp('cog_impegno3').clearValue();
        Ext.getCmp('cog_impegno3').disable();
        Ext.getCmp('pcf_impegno3').clearValue();
        Ext.getCmp('pcf_impegno3').disable();
        Ext.getCmp('importo_impegno3').disable();
        Ext.getCmp('importo_impegno3').reset();
        Ext.getCmp('button_pulisci3').disable();
    }
}

function abilitaOnlyBilancioAnnoCorrente(abilita) {
    if (abilita) {
        Ext.getCmp('cog_impegno1').enable();
        Ext.getCmp('pcf_impegno1').enable();
        Ext.getCmp('importo_impegno1').enable();
        Ext.getCmp('button_pulisci1').enable();

        Ext.getCmp('cog_impegno2').disable();
        Ext.getCmp('pcf_impegno2').disable();
        Ext.getCmp('importo_impegno2').disable();
        Ext.getCmp('importo_impegno2').reset();
        Ext.getCmp('button_pulisci2').disable();

        Ext.getCmp('cog_impegno3').disable();
        Ext.getCmp('pcf_impegno3').disable();
        Ext.getCmp('importo_impegno3').disable();
        Ext.getCmp('importo_impegno3').reset();
        Ext.getCmp('button_pulisci3').disable();
    }
}


function resetForm() {
    storePreImpegni.removeAll();
    storeCog.removeAll();
    storePCFPreImp.removeAll();
    Ext.getCmp('preimpegno').clearValue();
    Ext.getCmp('preimpegno').enable();

    Ext.getCmp('disponibilita_preimp').reset();
    Ext.getCmp('potenziale_preimp').reset();
    Ext.getCmp('obgestionale_preimp').clearValue();
    Ext.getCmp('pcf_preimpegno').clearValue();
    Ext.getCmp('impegno_da_preimp').reset();

    Ext.getCmp('cog_impegno1').clearValue();
    Ext.getCmp('pcf_impegno1').clearValue();
    Ext.getCmp('importo_impegno1').reset();

    Ext.getCmp('cog_impegno2').clearValue();
    Ext.getCmp('pcf_impegno2').clearValue();
    Ext.getCmp('importo_impegno2').reset();

    Ext.getCmp('cog_impegno3').clearValue();
    Ext.getCmp('pcf_impegno3').clearValue();
    Ext.getCmp('importo_impegno3').reset();

    abilitaPreimpegno(false);
    abilitaBilancio(true);
}

function creaImpegno(btn, evt) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Salvataggio Dati..."
    });

   

    var rows = Ext.getCmp('GridCapitoli').getSelectionModel().getSelections();

    if (rows.length != 1) {
        Ext.MessageBox.show({
            title: 'Attenzione',
            msg: 'Selezionare un capitolo',
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.WARNING
        });  
        return;
    }

    var preimpegno = parseFloat(Ext.getCmp('preimpegno').getValue());
    
    if (preimpegno) {
        var disponibilitaPreImp = parseFloat(
            replaceAll(Ext.getCmp('potenziale_preimp').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
        var importoImpegno = parseFloat(Ext.getCmp('impegno_da_preimp').getValue());

        if (!disponibilitaPreImp || importoImpegno > disponibilitaPreImp) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Disponibilità insufficiente sul preimpegno',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        }

        var annoBilancioPreImp = Ext.getCmp('annoBilancioPreImp').getValue();
        creaImpegnoDaPreimp(rows[0].data.Capitolo, preimpegno, Ext.getCmp('pcf_preimpegno').getValue(), Ext.getCmp('obgestionale_preimp').getValue(), importoImpegno, Ext.getCmp('potenziale_preimp').getValue(), annoBilancioPreImp, rows[0].data.UPB, rows[0].data.MissioneProgramma, maskApp);
    } else {

        //controllo che il capitolo non sia bloccato, prima ancora di controllare la disponibilità.
        if (rows[0].data.CodiceRisposta > 0) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Il capitolo selezionato è bloccato',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        } else {
            var impegno1 = 0;
            impegno1 = parseFloat(Ext.getCmp('importo_impegno1').getValue());
            var disponibilita1 = 0;
            disponibilita1 = parseFloat(
                        replaceAll(Ext.getCmp('disponibilita1').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
            var obgestionale1 = Ext.getCmp('cog_impegno1').getValue();
            var pcf1 = Ext.getCmp('pcf_impegno1').getValue();


            if (!isNaN(impegno1)) {
                if (disponibilita1 <= 0 || impegno1 > disponibilita1) {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Impegno 1: L\'importo indicato (' + eurRend(impegno1) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita1) + ")",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    return;
                }
            }


            var impegno2 = 0;
            impegno2 = parseFloat(Ext.getCmp('importo_impegno2').getValue());
            var disponibilita2 = 0;
            disponibilita2 = parseFloat(
                        replaceAll(Ext.getCmp('disponibilita2').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
            var obgestionale2 = Ext.getCmp('cog_impegno2').getValue();
            var pcf2 = Ext.getCmp('pcf_impegno2').getValue();

            if (!isNaN(impegno2)) {
                if (disponibilita2 <= 0 || impegno2 > disponibilita2) {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Impegno 2: L\'importo indicato (' + eurRend(impegno2) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita2) + ")",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    return;
                }
            }

            var impegno3 = 0;
            impegno3 = parseFloat(Ext.getCmp('importo_impegno3').getValue());
            var disponibilita3 = 0;
            disponibilita3 = parseFloat(
                        replaceAll(Ext.getCmp('disponibilita3').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
            var obgestionale3 = Ext.getCmp('cog_impegno3').getValue();
            var pcf3 = Ext.getCmp('pcf_impegno3').getValue();

            if (!isNaN(impegno3)) {
                if (disponibilita3 <= 0 || impegno3 > disponibilita3) {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Impegno 3: L\'importo indicato (' + eurRend(impegno3) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita3) + ")",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    return;
                }
            }

            creaImpegnoPluriennale(rows[0].data.Capitolo, rows[0].data.UPB, rows[0].data.MissioneProgramma, rows[0].data.Bilancio, impegno1, obgestionale1, pcf1,
                impegno2, obgestionale2, pcf2, impegno3, obgestionale3, pcf3, maskApp);
        }
    }
}

function creaImpegnoDaPreimp(capitolo, preimpegno, pcfPreImp, obgestionale, importo, disponibilita, bilancio, upb, missioneProgramma, maskApp) {
    maskApp.show();
    Ext.getCmp('btnCreaImpegno').hide();

    var parametri = {
        Capitolo: capitolo,
        ComboPreimp: preimpegno,
        UPB: upb,
        MissioneProgramma: missioneProgramma,
        Bilancio: bilancio,
        ImpPrenotato: importo,
        ComboCOGImp: obgestionale,
        PcfPreImp: pcfPreImp,
        ImpDisp: disponibilita,
        Importo1: "",
        ObGestionale1: "",
        PCF1: "",
        Importo2: "",
        ObGestionale2: "",
        PCF2: "",
        Importo3: "",
        ObGestionale3: "",
        PCF3: "",
        ListaFatture: listaFatturaToStartImpegno
    };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: "ProcAmm.svc/GenerazionePreImpegno" + window.location.search,
        //headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        params: Ext.encode(parametri),
        method: 'POST',
        success: function (response, opts) {
            maskApp.hide();

            if (response.responseText.toLowerCase().indexOf("success: false") !== -1) {
                listaFatturaToStartImpegno = [];
                var msg = response.responseText;
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: msg,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { Ext.getCmp('btnCreaImpegno').show(); }
                });
            } else {
            

                Ext.getCmp('btnCreaImpegno').show();

                var titolo = "Creazione impegno";
                var messaggio = "Generazione impegno avvenuta con successo";
                if (listaFatturaToStartImpegno.length > 0) {
                    titolo = "Creazione impegno e liquidazione contestuale";
                    messaggio = "Generazione dell'impegno e della liquidazione contestuale avvenuta con successo";
                }
            
                if (response.statusText == "OK") {
                    Ext.MessageBox.show({
                        title: titolo,
                        msg: messaggio,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function (btn) {
                            Ext.getCmp('btnCreaImpegno').show();
                            Ext.getCmp('GridRiepilogo').getStore().reload();
                            if (listaFatturaToStartImpegno.length > 0) {
                                Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                            }
                            Ext.getCmp('popup_impegno').close();
                            listaFatturaToStartImpegno = [];
                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Creazione impegno',
                        msg: "Errore generico",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function (btn) {
                            Ext.getCmp('btnCreaImpegno').show();
                            listaFatturaToStartImpegno = [];
                        }
                    });
                }
            }
        },
        failure: function (response, opts) {
            maskApp.hide();
            Ext.getCmp('btnCreaImpegno').show();

            listaFatturaToStartImpegno = [];
            var data = Ext.decode(response.responseText);
            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { Ext.getCmp('btnCreaImpegno').show(); }
            });
        }
    });
}

function creaImpegnoPluriennale(capitolo, upb, missioneProgramma, bilancio, importo1, obgestionale1, pcf1,
    importo2, obgestionale2, pcf2, importo3, obgestionale3, pcf3, maskApp) {

    maskApp.show();
    Ext.getCmp('btnCreaImpegno').hide();

    var parametri = {
        Capitolo: capitolo,
        ComboPreimp: 0,
        UPB: upb,
        MissioneProgramma: missioneProgramma,
        Bilancio: bilancio,
        ImpPrenotato: "",
        ComboCOGImp: "",
        PcfPreImp: "",
        ImpDisp: "",
        Importo1: importo1,
        ObGestionale1: obgestionale1,
        PCF1: pcf1,
        Importo2: importo2,
        ObGestionale2: obgestionale2,
        PCF2: pcf2,
        Importo3: importo3,
        ObGestionale3: obgestionale3,
        PCF3: pcf3,
        ListaFatture: listaFatturaToStartImpegno
    };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: "ProcAmm.svc/GenerazionePreImpegno" + window.location.search,
        params: Ext.encode(parametri),
        method: 'POST',
        success: function (response, opts) {
            maskApp.hide();
            Ext.getCmp('btnCreaImpegno').show();

            var titolo = "Creazione impegno";
            //qui
            if (response.responseText.toLowerCase().indexOf("success: false") !== -1) {
                listaFatturaToStartImpegno = [];
                var msg = response.responseText;
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: msg,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { Ext.getCmp('btnCreaImpegno').show(); }
                });
            } else {
                var messaggio = "Generazione impegno avvenuta con successo";
                if (listaFatturaToStartImpegno.length > 0) {
                    titolo = "Creazione impegno e liquidazione contestuale";
                    messaggio = "Generazione dell'impegno e della liquidazione contestuale avvenuta con succeso";
                }

                if (response.statusText == "OK") {
                    Ext.MessageBox.show({
                        title: titolo,
                        msg: messaggio,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function (btn) {
                            Ext.getCmp('btnCreaImpegno').show();
                            Ext.getCmp('GridRiepilogo').getStore().reload();
                            if (listaFatturaToStartImpegno.length > 0) {
                                Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
                                listaFatturaToStartImpegno = [];
                            }
                            Ext.getCmp('popup_impegno').close();
                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Creazione impegno',
                        msg: "Errore generico",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function (btn) {
                            Ext.getCmp('btnCreaImpegno').show();
                            listaFatturaToStartImpegno = [];
                        }
                    });
                }
            }
        },
        failure: function (response, opts) {
            maskApp.hide();
            Ext.getCmp('btnCreaImpegno').show();

            listaFatturaToStartImpegno = [];
            var data = Ext.decode(response.responseText);
            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { Ext.getCmp('btnCreaImpegno').show(); }
            });
        }
    });
}


function validatePreimpegno(value) {
    return parseFloat(Ext.getCmp('impegno_da_preimp').getValue()) > 0;
}

/* Per ogni bilancio o tutti o nessun campo compilato */
function validateBilancio1(value) {
    var result = false;

    result = Ext.getCmp('cog_impegno1') != undefined
            && Ext.getCmp('cog_impegno1').getValue() != ""

            && Ext.getCmp('pcf_impegno1') != undefined
            && Ext.getCmp('pcf_impegno1').getValue() != ""

            && Ext.getCmp('importo_impegno1') != undefined
            && Ext.getCmp('importo_impegno1').getValue() != ""

            && parseFloat(Ext.getCmp('importo_impegno1').getValue()) > 0;

    result = result || (Ext.getCmp('cog_impegno1').getValue() == ""
            && Ext.getCmp('pcf_impegno1').getValue() == ""
            && Ext.getCmp('importo_impegno1').getValue() == ""
            && (Ext.getCmp('importo_impegno3').getValue() != ""
                || Ext.getCmp('importo_impegno2').getValue() != ""));

    return result;
}

function validateBilancio2(value) {
    var result = false;

    result = Ext.getCmp('cog_impegno2').getValue() != ""
            && Ext.getCmp('pcf_impegno2').getValue() != ""
            && Ext.getCmp('importo_impegno2').getValue() != ""
             && parseFloat(Ext.getCmp('importo_impegno2').getValue()) > 0;

    result = result || (Ext.getCmp('cog_impegno2').getValue() == ""
            && Ext.getCmp('pcf_impegno2').getValue() == ""
            && Ext.getCmp('importo_impegno2').getValue() == ""
            && (Ext.getCmp('importo_impegno3').getValue() != ""
                || Ext.getCmp('importo_impegno1').getValue() != ""));

    return result;
}

function validateBilancio3(value) {
    var result = false;

    result = Ext.getCmp('cog_impegno3').getValue() != ""
            && Ext.getCmp('pcf_impegno3').getValue() != ""
            && Ext.getCmp('importo_impegno3').getValue() != ""
             && parseFloat(Ext.getCmp('importo_impegno3').getValue()) > 0;

    result = result || (Ext.getCmp('cog_impegno3').getValue() == ""
            && Ext.getCmp('pcf_impegno3').getValue() == ""
            && Ext.getCmp('importo_impegno3').getValue() == ""
            && (Ext.getCmp('importo_impegno2').getValue() != ""
                || Ext.getCmp('importo_impegno1').getValue() != ""));

    return result;
}

function replaceAll(txt, replace, with_this) {
    return txt.replace(new RegExp(replace, 'g'), with_this);
}



//DEFINISCO L'AZIONE per la gestione delle fatture delle liquidazioni
var actionShowFattureLiquidazione = new Ext.Action({
    text: 'Fatture',
    tooltip: 'Aggiunge le Fatture della liquidazione selezionata',
    handler: function () {
        Ext.each(Ext.getCmp('GridLiquidazioniContestuali').getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
                Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            } else {
                var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };
                var grid = buildPanelFattureLiquidazione(liquidazione);
                Ext.getCmp('myPanelLiqContestuali').add(grid);
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            }
        })
    },
    iconCls: 'add'
});

function validaImportoDaLiquidareFattura(rec) {
    var messaggioErrore = ""
    if (rec.data.ImportoFattDaLiquidare != undefined) {
        if ((rec.data.ImportoFattDaLiquidare == 0) || (rec.data.ImportoFattDaLiquidare == '')) {
            errore = true;
            messaggioErrore = messaggioErrore + "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare è ZERO<br/>";
            return messaggioErrore;
        } else {
            if ((rec.data.ImportoFattDaLiquidare > rec.data.ImportoResiduo)) {
                errore = true;
                messaggioErrore = "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare (€ " + rec.data.ImportoFattDaLiquidare + ") è maggiore dell\' importo residuo della fattura (€ " + rec.data.ImportoResiduo + ")<br/>";
                return messaggioErrore;
            }
        }
    } else {
        errore = true;
        messaggioErrore = messaggioErrore + "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare è ZERO<br/>";
        return messaggioErrore;
    }
    return messaggioErrore;
}

function showPopupPanelFattureLiquidazione(liquidazione, importoTotaleFattureLiq, showToGenerateImpegno) {
    var titoloFinestra = "Seleziona la fattura";
    if (showToGenerateImpegno) {
        titoloFinestra = titoloFinestra + " per cui generare impegno e liquidazione";
    } else {
        titoloFinestra = titoloFinestra + " da aggiungere alla liquidazione";
    }
    var popup = new Ext.Window({
        title: titoloFinestra,
        width: 900,
        height: 610,
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
            id: 'btnConfermaSelezioneFatturaLiqCont'

        }, {
            //text: 'Genera Impegno e Liquidazione',
            text: 'Avanti',
            id: 'btnGeneraImpegnoELiquidazione',
            iconCls: 'arrow-right'
        }, {
            text: 'Chiudi',
            id: 'btnChiudiSelezioneFatturaLiqCont'
        }]
    });

    var gridFattureDocumento = buildPopUpFattureLiquidazioneContest();

    if (gridFattureDocumento != null && gridFattureDocumento != undefined) {
        popup.add(gridFattureDocumento);
    }

    popup.doLayout();
    popup.show();

    Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').disable();
    if (liquidazione != undefined && !showToGenerateImpegno) {
        Ext.getCmp('btnGeneraImpegnoELiquidazione').hide();

        Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').on('click', function () {

            var fatture = Ext.getCmp('GridPopUpFattureLiquidazioneContest').getSelectionModel().getSelections();
            var residuoLiquidazione = liquidazione.ImpPrenotatoLiq - importoTotaleFattureLiq;
            residuoLiquidazione = parseFloat(residuoLiquidazione).toFixed(2);
            var listaFatture = [];
            if (fatture.length > 0) {
                var errore = false;
                var messaggioErrore = "";
                var totaleImportiDaLiquidare = 0;
                Ext.each(fatture, function (rec) {
                    totaleImportiDaLiquidare = totaleImportiDaLiquidare + rec.data.ImportoFattDaLiquidare;
                });
                totaleImportiDaLiquidare = parseFloat(totaleImportiDaLiquidare).toFixed(2);
                if (totaleImportiDaLiquidare > liquidazione.ImpPrenotatoLiq) {
                    errore = true;
                    messaggioErrore = messaggioErrore + "Il totale degli importi da liquidare delle fatture selezionate (€" + totaleImportiDaLiquidare + ") è maggiore dell\'importo prenotato per la liquidazione (€ " + liquidazione.ImpPrenotatoLiq + ")<br/>";
                } else if (totaleImportiDaLiquidare > residuoLiquidazione) {
                    //controllare le fatt già presenti sulle liq, calcolare il totale e verificare che l'importo totale che sto aggiungendo (totaleImportiDaLiquidare) non sia
                    //maggiore dell'importo residuo sulla liq (importo liq MENO fatt già presenti sulle liq)
                    errore = true;
                    messaggioErrore = "L'importo residuo sulla liquidazione è di € " + residuoLiquidazione + ". L\'importo totale indicato sulle futture è € " + totaleImportiDaLiquidare + "<br/>";
                } else {

                    var messaggioErroreValidazione = "";
                    Ext.each(fatture, function (rec) {
                        messaggioErroreValidazione = validaImportoDaLiquidareFattura(rec);
                        if (messaggioErroreValidazione == "") {
                            listaFatture.push(rec.data);
                        }
                    });
                    if (messaggioErroreValidazione != "") {
                        errore = true;
                        messaggioErrore = messaggioErrore + messaggioErroreValidazione;
                    }
                }
                if (errore) {
                    Ext.MessageBox.show({
                        title: 'Associazione Fattura',
                        msg: messaggioErrore,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) { }
                    });
                } else {
                    if (listaFatture.length != 0) {
                        var statoOperazione = "I";
                        registraFatturaLiquidazione(statoOperazione, liquidazione.ID, listaFatture);

                        //                    var gridFatture = Ext.getCmp('GridFattureLiquidazione');
                        //                    var storeGridFatture = gridFatture.getStore();
                        //                    storeGridFatture.reload();
                        popup.close();
                    } else {
                        Ext.MessageBox.show({
                            title: 'Attenzione',
                            msg: 'Selezionare una fattura e specificare l\'importo da liquidare',
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.WARNING
                        });
                        return;
                    }
                }
            } else {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Selezionare una fattura e specificare l\'importo da liquidare',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return;
            }
        });
    } else {
        Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').hide();

        Ext.getCmp('btnGeneraImpegnoELiquidazione').on('click', function () {
            var fatture = Ext.getCmp('GridPopUpFattureLiquidazioneContest').getSelectionModel().getSelections();
            if (fatture.length == 0) {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: "Selezionare almeno una fattura",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
            } else if (fatture.length > 1) {
                //se è stata selezionata + di una fattura, devo confrontare i loro beneficiari e assicurarmi che riportino tutte lo stesso benficiario (ID, idSede, idModalitaPagamento)
                var retValue = true;
                var msgErrore = "";
                Ext.each(fatture, function (fatturaToCheckBen1) {
                    if (fatturaToCheckBen1 != undefined) {
                        var infoAnagrafeToCheck1 = fatturaToCheckBen1.data.AnagraficaInfo;
                        var sedeToCheck1;
                        if (infoAnagrafeToCheck1.ListaSedi != undefined && infoAnagrafeToCheck1.ListaSedi.length > 0) {
                            sedeToCheck1 = infoAnagrafeToCheck1.ListaSedi[0];
                        }

                        var indiceFatturaDaEscludere = fatture.indexOf(fatturaToCheckBen1);
                        var listaFattureFiltrata = [];
                        listaFattureFiltrata = fatture.slice();
                        listaFattureFiltrata.splice(indiceFatturaDaEscludere, 1);
                        Ext.each(listaFattureFiltrata, function (fattura) {
                            var infoAnagrafe = fattura.data.AnagraficaInfo;
                            var sede;
                            if (infoAnagrafe.ListaSedi != undefined && infoAnagrafe.ListaSedi.length > 0) {
                                sede = infoAnagrafe.ListaSedi[0];
                            }
                            if (infoAnagrafeToCheck1.ID != infoAnagrafe.ID) {
                                msgErrore = 'Non è possibile selezionare fatture con beneficiari differenti';
                                retValue = false;
                            } else if (sedeToCheck1 != undefined && sede != undefined && sedeToCheck1.IdSede != sede.IdSede && sedeToCheck1.IdModalitaPagamento != sede.IdModalitaPagamento) {
                                msgErrore = 'Non è possibile selezionare fatture con beneficiari differenti';
                                retValue = false;
                            } else {
                                retValue = true;
                            }
                            
                        });

                        var messaggioErroreValidazione = "";
                        Ext.each(fatture, function (rec) {
                            messaggioErroreValidazione = validaImportoDaLiquidareFattura(rec);
                        });

                        if (retValue == false && messaggioErroreValidazione != "") {
                            msgErrore = msgErrore + " " + messaggioErroreValidazione
                            return false;
                        }
                    }

                });

                // se non ci sono problemi sulle fatture selezionate, verifico gli importi!
                if (retValue == true) {
                    Ext.MessageBox.buttonText.yes = 'Si';
                    var isSensibile = false;
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Ai fini della trasparenza è obbligatorio indicare se il dato del beneficiario è sensibile o no?',
                        buttons: Ext.MessageBox.YESNO,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                            if (btn == 'yes') {
                                isSensibile = true;
                            } else {
                                isSensibile = false;
                            }
                            Ext.each(fatture, function (fattura) {
                                fattura.data.AnagraficaInfo.IsDatoSensibile = isSensibile;
                                listaFatturaToStartImpegno.push(fattura.data);
                            });

                            popup.close();
                            InitFormCapitoliImpegno(false, true);
                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: msgErrore,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                }

            } else {
                var messaggioErroreValidazione = "";
                Ext.each(fatture, function (fattura) {
                    messaggioErroreValidazione = validaImportoDaLiquidareFattura(fattura);
                });

                if (messaggioErroreValidazione == "") {
                    //stefs chiedi dato sensibile
                    Ext.MessageBox.buttonText.yes = 'Si';
                    var isSensibile = false;
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Ai fini della trasparenza è obbligatorio indicare se il dato del beneficiario è sensibile o no?',
                        buttons: Ext.MessageBox.YESNO,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                            if (btn == 'yes') {
                                isSensibile = true;
                            } else {
                                isSensibile = false;
                            }


                            popup.close();
                            Ext.each(fatture, function (fattura) {
                                fattura.data.AnagraficaInfo.IsDatoSensibile = isSensibile;
                                listaFatturaToStartImpegno.push(fattura.data);
                            });

                            InitFormCapitoliImpegno(false, true);

                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: messaggioErroreValidazione,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                }

            }


        });
    }

    Ext.getCmp('btnChiudiSelezioneFatturaLiqCont').on('click', function () {
        listaFatturaToStartImpegno = [];
        if (Ext.getCmp('GridFattureLiquidazione') != undefined) {
            Ext.getCmp('GridFattureLiquidazione').getStore().reload();
            var totalRows = Ext.getCmp('GridFattureLiquidazione').store.getRange().length;
            if (totalRows == 0) {
                Ext.getCmp('myPanelLiqContestuali').remove(Ext.getCmp('GridFattureLiquidazione'));
                Ext.getCmp('myPanelLiqContestuali').doLayout();
            }
        }
        popup.close();
    });

    popup.on('close', function () {
        var gridFatture = Ext.getCmp('GridPopUpFattureLiquidazioneContest');
        gridFatture.getView().refresh();

    });
}


function showPopupPanelUploadFile() {
    var popup = new Ext.Window({
        title: "Seleziona il file",
        width: 900,
        height: 610,
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
            text: 'Chiudi',
            id: 'btnChiudiSelezioneFile'
        }]
    });

    var labelImportoImpPrenotato = new Ext.form.Label({
        text: 'Importo Impegno Selezionato',
        id: 'impImpPrenotato'
    });

    //var panelImpImpPrenotato = new Ext.Panel({
    //								xtype : "panel",
    //								title : "",
    //								width:300,
    //								buttonAlign: "center",
    //								autoHeight:true,
    //								layout: "fit",
    //								items: [
    //									//labelImportoImpPrenotato,
    //								    xtype: "form",
    //                                    border: false,
    //                                    bodyStyle: {padding: '10px'},
    //                                    items: {
    //                                        xtype: 'multifilefield',
    //                                        labelWidth: 80,
    //                                        fieldLabel: 'Choose file(s)',
    //                                        anchor: '100%',
    //                                        allowBlank: false,
    //                                        margin: 0
    //                                    }
    //]
    //								}); 

    //popup.add(panelImpImpPrenotato);

    popup.doLayout();
    popup.show();

    Ext.getCmp('btnChiudiSelezioneFile').on('click', function () {
        popup.close();
    });

    popup.on('close', function() {});
}

function registraFatturaLiquidazione( statoOperazione, idLiquidazione, listaFatture) {

    var parametri = { idLiquidazione: idLiquidazione, statoOperazione: statoOperazione, listaFatture: listaFatture };
    var messaggioRisposta;
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/NotificaAttoFattura' + window.location.search,
        params: Ext.encode(parametri),
        method: 'POST',
        success: function(response, options) {
            var msg = "";
            if (response.statusText == "OK") {
                msg = "Operazione completata con successo";
            } else {
                msg = "Errore Generico";
            }
            Ext.MessageBox.show({
                title: 'Fatture liquidazione',
                msg: msg,
                buttons: Ext.MessageBox.OK
            });

            Ext.each(listaFatture, function(fattura) {

                FatturaRecordType = Ext.data.Record.create(['IdUnivoco', 'IdDocumento', 'Contratto', 'NumeroFatturaBeneficiario', 'DataFatturaBeneficiario', 'AnagraficaInfo', 'ImportoTotaleFattura', 'DescrizioneFattura', 'ImportoFattDaLiquidare', 'Prog']);

                var newRecordFattura = new FatturaRecordType({
                    Contratto: fattura.Contratto,
                    IdDocumento: fattura.IdDocumento,
                    NumeroFatturaBeneficiario: fattura.NumeroFatturaBeneficiario,
                    DataFatturaBeneficiario: fattura.DataFatturaBeneficiario,
                    AnagraficaInfo: fattura.AnagraficaInfo,
                    ImportoTotaleFattura: fattura.ImportoTotaleFattura,
                    DescrizioneFattura: fattura.DescrizioneFattura,
                    IdUnivoco: fattura.IdUnivoco,
                    ImportoLiquidato: fattura.ImportoFattDaLiquidare,
                    Prog: fattura.Prog
                });

                Ext.getCmp('GridFattureLiquidazione').store.add(newRecordFattura);
            });

        },
        failure: function(response, result) {
            var data = Ext.decode(response.responseText);

            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) { return }
            });

        }
    });
}



function buildPopUpFattureLiquidazioneContest() {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetListaFattureNonAssegnateLiquidazioneOrResidue' + window.location.search,
        method: 'POST',
        timeout: 20000000,
        failure: function(response, result) {
            maskApp.hide();
            var data = Ext.decode(response.responseText);

            msg = data.NotificaAttoFatturaResult;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: 'Impossibile recuperare le fatture',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) { return }
            });

        }
    });

    var reader = new Ext.data.JsonReader({
    root: 'GetListaFattureNonAssegnateLiquidazioneOrResidueResult',
    fields: [
           { name: 'Prog' },
	       { name: 'IdUnivoco' },
	       { name: 'Contratto' },
           { name: 'NumeroFatturaBeneficiario' },
           { name: 'DataFatturaBeneficiario' },
           { name: 'DescrizioneFattura' },
           { name: 'ImportoTotaleFattura' },
           { name: 'ImportoLiquidato' },
           { name: 'ImportoResiduo' },
           { name: 'ImportoFattDaLiquidare' },           
           { name: 'AnagraficaInfo' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" }
    });

    store.on({
        'load': {
            fn: function (store, records, options) {
                if (records.length == 0) {
                    if (Ext.getCmp('btnConfermaSelezioneFatturaLiqCont') != null && Ext.getCmp('btnConfermaSelezioneFatturaLiqCont') != undefined)
                        Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').disable();
                    if (Ext.getCmp('btnGeneraImpegnoELiquidazione') != null && Ext.getCmp('btnGeneraImpegnoELiquidazione') != undefined)
                        Ext.getCmp('btnGeneraImpegnoELiquidazione').disable();
                } else {
                    var grigliaFattureContest = Ext.getCmp('GridPopUpFattureLiquidazioneContest');
                    if (grigliaFattureContest != undefined) {
                        //grigliaFattureContest.getSelectionModel().selectAll();
                        grigliaFattureContest.getView().refresh();
                    }
                }
                maskApp.hide();
            },
            scope: this
        }
    });

    maskApp.show();

    var parametri = { };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: false,
        listeners: {

            rowselect: function(selectionModel, rowIndex, record) {
                if (record.data.ImportoResiduo == 0) {
                    selectionModel.getSelections().remove(rowIndex);
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Impossibile selezionare la fattura ' + record.data.NumeroFatturaBeneficiario + ' il residuo è ZERO',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(btn) { return }
                    });
                } else {
                    var totalRows = Ext.getCmp('GridPopUpFattureLiquidazioneContest').store.getRange().length;
                    var selectedRows = selectionModel.getSelections();
                    if (selectedRows.length > 0) {

                        Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').enable();
                        Ext.getCmp('btnGeneraImpegnoELiquidazione').enable();
                    }
                    if (totalRows == selectedRows.length) {

                        if (Ext.getCmp('GridPopUpFattureLiquidazioneContest') != undefined) {
                            var view = Ext.getCmp('GridPopUpFattureLiquidazioneContest').getView();
                            var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker")
                            chkdiv.addClass("x-grid3-hd-checker-on");
                        }

                    }
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();

                if (selectedRows.length == 0) {
                    Ext.getCmp('btnConfermaSelezioneFatturaLiqCont').disable();
                    Ext.getCmp('btnGeneraImpegnoELiquidazione').disable();
                }
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            
            { header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true },
            { header: "Data Fattura", width: 80, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },
            {
                header: "Beneficiario", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
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
            }, {
                header: "Sede", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0 ) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede;
                        }
                    }
                    return retValue;
                }
            }, {
                header: "Metodo Pagamento", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
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
            {
                header: "Iban", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
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
            { header: "Importo Totale", width: 60, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false, renderer: eurRend },
            { header: "Liquidato", width: 60, dataIndex: 'ImportoLiquidato', sortable: true, locked: false, renderer: eurRend},
            { header: "Residuo", width: 60, dataIndex: 'ImportoResiduo', sortable: true, locked: false,renderer: eurRend},
            {
                header: "Da Liquidare", width: 60, dataIndex: 'ImportoFattDaLiquidare', sortable: true, locked: false,
                css: 'background-color: #fffb8a;',
                renderer: eurRend,
                editor: new Ext.form.NumberField({
                    decimalSeparator: ',',
                    allowBlank: true,
                    allowNegative: true
                })
            },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "Prog", width: 100, dataIndex: 'Prog', hidden: true }
         	]);

      var grid = new Ext.grid.EditorGridPanel({
        id: 'GridPopUpFattureLiquidazioneContest',
        title: 'Lista Fatture',
        autoHeight: false,
        height: 100,
        width: 750,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessuna fattura corrisponde ai criteri di ricerca inseriti.",
            deferEmptyText: false
        },
        sm: sm
    });

    return grid;
}


function buildPanelGridFattureLiquidazione(liquidazione) {

    var aggiungiFatturaLiquidazione = new Ext.Action({
        text: 'Aggiungi',
        id: 'actionAggiungiFatturaLiquidazione',
        tooltip: 'Seleziona e aggiunge uno più fatture alla liquidazione selezionata',
        handler: function() {
            //stef
            var importoTotaleFattureLiq = 0;
            var gridFatture = Ext.getCmp('GridFattureLiquidazione');
            if (gridFatture != undefined) {
                var storeGridFatture = Ext.getCmp('GridFattureLiquidazione').getStore();
                storeGridFatture.each(function(rec) {
                    importoTotaleFattureLiq = importoTotaleFattureLiq + rec.data.ImportoLiquidato;
                });
                showPopupPanelFattureLiquidazione(liquidazione, importoTotaleFattureLiq, false);
            }
        },
        iconCls: 'add'
    });

    var rimuoviFatturaLiquidazione = new Ext.Action({
        text: 'Rimuovi',
        id: 'actionRimuoviFatturaLiquidazione',
        tooltip: 'Rimuove una o più fatture dalla liquidazione selezionata',

        handler: function() {
            Ext.MessageBox.buttonText.cancel = "Annulla";
            var gridFatture = Ext.getCmp('GridFattureLiquidazione');
            var storeGridFatture = gridFatture.getStore();
            var selections = gridFatture.getSelectionModel().getSelections();

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'della fattura selezionata' : 'delle fatture selezionate') + '?',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function(btn) {
                    if (btn == 'ok') {
                        var listaFatture = [];
                        var gridFatture = Ext.getCmp('GridFattureLiquidazione');
                        Ext.each(gridFatture.getSelectionModel().getSelections(), function(rec) {
                            if (rec.data['IdUnivoco'] != 0) {
                                listaFatture.push(rec.data);
                            }
                        })

                        EliminaFatturaDaLiquidazione(listaFatture, 'C', liquidazione.ID);

//                        var gridFatture = Ext.getCmp('GridFattureLiquidazione');
//                        if (gridFatture != undefined && gridFatture != null) {
//                            var storeGridFatture = gridFatture.getStore();
//                            storeGridFatture.reload();
//                        }
//                        
//                        var gridBen = Ext.getCmp('GridBeneficiariContestuali');
//                        if (gridBen != undefined && gridBen != null) {
//                            var storeGridBen = gridBen.getStore();
//                            storeGridBen.reload();
//                        }


                        var selectedRows = gridFatture.getSelectionModel().getSelections();
                        if (selectedRows.length == 0) {
                            rimuoviFatturaLiquidazione.disable();
                        }
                    }
                }
            });
        },
        iconCls: 'remove'
    });



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
           { name: 'IdLiquidazione' }
           
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" }
    });

    store.on({
        'load': {
            fn: function (store, records, options) {
                if (records.length == 0) {
                    //aggiungiFatturaLiquidazione.execute();

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

    var summary = new Ext.grid.GroupSummary();
    
    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function(selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridFattureLiquidazione').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviFatturaLiquidazione.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridFattureLiquidazione').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function(selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviFatturaLiquidazione.disable();
                }
                var view = Ext.getCmp('GridFattureLiquidazione').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        {
            header: "Repertorio", width: 60, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
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
         {
             header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true,
             summaryRenderer: function(v, params, data) { return '<b> Totale </b>'; } 
         },
         { header: "Data Fattura", width: 60, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },

         {
             header: "Beneficiario", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
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

            {
                header: "Sede", width: 85, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
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

            {
                header: "Metodo Pagamento", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
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

            {
                header: "Iban", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
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
            { header: "Importo Totale", width: 60, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false, renderer: eurRend },
            { header: "Da Liquidare", width: 60, dataIndex: 'ImportoLiquidato', sortable: true, locked: false, renderer: eurRend, summaryType: 'sum' },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "IdDocumento", width: 100, dataIndex: 'IdDocumento', hidden: true },
            { header: "IdLiquidazione", width: 100, dataIndex: 'IdLiquidazione', hidden: true }

        ]);
    
    

    var grid = new Ext.grid.GridPanel({
        id: 'GridFattureLiquidazione',
        cls: 'row-summary-style',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        xtype: "form",
        autoHeight: true,
        border: true,
//        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        plugins: [summary],
        width: 740,
        stripeRows: true,
        loadMask: true,
        title: "Fatture relative alla liquidazione selezionata",
        tbar: [aggiungiFatturaLiquidazione, rimuoviFatturaLiquidazione],
				        
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

    var gridFatture = new Ext.Panel({
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        items: [grid]
    }
    );

    rimuoviFatturaLiquidazione.disable();

    return gridFatture;
}

function buildPanelFattureLiquidazione(liquidazione) {

    var panelFattureLiquidazione = new Ext.Panel({
    id: 'panelFatturaLiquidazione ',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        style: 'margin-top:15px;',

        layoutConfig: {
            columns: 1
        },
        items: [

            buildPanelGridFattureLiquidazione(liquidazione),
            {
                xtype: 'hidden',
                id: 'listaFattureLiquidazione'
            }
         ]
        });
    return panelFattureLiquidazione;
}