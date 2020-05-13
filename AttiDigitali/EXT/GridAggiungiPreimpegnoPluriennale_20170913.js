var maskPreimpegni;
maskPreimpegni = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


Ext.IframeWindow = Ext.extend(Ext.Window, {
    onRender: function() {
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





var storeCogPreimpegni = new Ext.data.Store({
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

// un piano dei conti finanziario per ogni bilancio
// non è possibile riusare un solo store 
var storePCF1Preimpegni = new Ext.data.Store({
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

var storePCF2Preimpegni = new Ext.data.Store({
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

var storePCF3Preimpegni = new Ext.data.Store({
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

var actionHelpOnLinePreimpegno = new Ext.Action({
    text: 'Aiuto',
    tooltip: 'Aiuto in linea: Generazione preimpegno contabile',
    handler: function() {
        new Ext.IframeWindow({
            modal:true,
            layout: 'fit',
            title: 'Generazione di un preimpegno ',
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
var actionAddPreimpegno = new Ext.Action({
    text: 'Aggiungi',
    tooltip: 'Aggiungi un nuovo preimpegno',
    handler: function() {
        var mostraAnno = false;
        InitFormCapitoliPreimpegno(mostraAnno);
    },
    iconCls: 'add'
});

//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaPreimpegno = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function() {
        var impegniDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridRiepilogoPreimpegni').getSelectionModel().getSelections(), function(rec) {
            //conferma solo gli impegni da confermare, quelli aventi stato 2
            if (rec.data['Stato'] == 2) {
                var impegnoInfo = new Object();

                impegnoInfo.ID = rec.data['ID'];
                impegnoInfo.NumPreImp = rec.data['NumPreImp'];
                impegnoInfo.Stato = rec.data['Stato'];

                impegniDaComfermare.push(impegnoInfo);
            }
        });

        ConfermaMultiplaPrenotazione(impegniDaComfermare);
    },
    iconCls: 'save'
});



  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA  
    var actionDeletePreimpegno = new Ext.Action({
         text:'Cancella',
         tooltip:'Cancella selezionato',
        handler: function(){
            var storeGridImp=Ext.getCmp('GridRiepilogoPreimpegni').getStore();
			var GridImpegni=Ext.getCmp('GridRiepilogoPreimpegni');
			   Ext.each(GridImpegni.getSelectionModel().getSelections(), function(rec)
						{
						if (rec.data.Stato == 2 ){
						    EliminaPreImpegnoProvvisorio(rec.data['NumPreImp'], rec.data['ID']);
						    storeGridImp.remove(rec);
						    actionConfermaPreimpegno.setDisabled(true);
						    actionDeletePreimpegno.setDisabled(true);
						    actionAddPreimpegno.setDisabled(false);
						    
						}else{
						    EliminaPreImpegnoProvvisorio(rec.data['NumPreImp'], rec.data['ID']);
						    storeGridImp.remove(rec);
						    actionDeletePreimpegno.setDisabled(true);
						    actionAddPreimpegno.setDisabled(false);
						 }
						})            
        },
        iconCls: 'remove'
    });


    
    

    
    function getInfoPrenotazione(){
      var GridImpegni = Ext.getCmp('GridRiepilogoPreimpegni');
     }
                        
    function selectComboCOG_prenotazione(record,index){
        Ext.get('DescrCOG').dom.value=index.data['Descrizione']
    }

    //PopUp Modifica COG/PdCF   
    function generaFormCOGAndPdCF_prenotazione(idProg, codValue, codPdCFValue, AnnoRif, CapitoloRif) {

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
            id: 'ComboPdCFPreimp',
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

        var ComboCOGPreimp_pr=  new Ext.form.ComboBox({
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
            id: 'ComboCOGPreimp_pr',
            mode: 'local',
            triggerAction: 'all',
            emptyText: 'Seleziona Codice Obiettivo Gestionale...'
        });
                   
        var FormDetailCOGAndPdCF_preimp = new Ext.FormPanel({
            id: 'FormDetailCOGAndPdCF_preimp',
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
                ComboCOGPreimp_pr,
                ComboPdCF,
                {  
                     xtype:'hidden',
                     name:'id_prog',
                     value:idProg 
                }]
           }]
       }); // chiude FormDetailCOGAndPdCF_preimp
                         
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
			                    id: 'btnSalvaCOGAndPdCF_preimp'
                            },
		                    {
		                        text: 'Annulla',
		                        id: 'btnAnnullaCOGAndPdCF_preimp'
                            }]
                        }); // chiude popup

	  popup.add('FormDetailCOGAndPdCF_preimp');
      
      Ext.getCmp('ComboCOGPreimp_pr').setValue(codValue);
      Ext.getCmp('ComboPdCFPreimp').setValue(codPdCFValue);
     
	  popup.doLayout()

	  Ext.getCmp('btnAnnullaCOGAndPdCF_preimp').on('click', function() {
	      popup.close();
	  });

	  Ext.getCmp('btnSalvaCOGAndPdCF_preimp').on('click', function() {		  
	    if ((Ext.get('ComboCOGPreimp_pr').dom.value.indexOf("Seleziona") != -1 ) || (Ext.get('ComboCOGPreimp_pr').dom.value == '')){
                Ext.MessageBox.show({  
                    title: 'COG',
                    msg: 'Il campo Codide Obiettivo Gestionale non &egrave; stato valorizzato!',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
        } else if ((Ext.get('ComboPdCFPreimp').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboPdCFPreimp').dom.value == '')) {
                Ext.MessageBox.show({
                    title: 'Piano dei Conti Finanziario',
                    msg: 'Il campo Piano dei Conti Finanziario non &egrave; stato valorizzato!',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
        } else {
            Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
            FormDetailCOGAndPdCF_preimp.getForm().timeout = 100000000;
            FormDetailCOGAndPdCF_preimp.getForm().submit({
                url: 'ProcAmm.svc/AssegnaCOGePdCFPreimpegno',
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
                              Ext.getCmp('GridRiepilogoPreimpegni').getStore().reload();
                              popup.close();
            	              
                              actionDeletePreimpegno.setDisabled(true);
                              actionConfermaPreimpegno.setDisabled(true);
                              actionModificaCOGAndPdCFPrenotazione.setDisabled(true);
                              actionAddPreimpegno.setDisabled(false);                                                                                
                            }
                        });
                } // FINE SUCCESS
               }) // FINE SUBMIT
          }//fine else         
	  }) //fine onclick
        popup.show();
    } // generaFormCOGAndPdCF_prenotazione
    
    
    var actionModificaCOGAndPdCFPrenotazione = new Ext.Action({
        text: 'Modifica COG/Piano dei Conti Finanziario',
        tooltip: 'Modifica COG e/o Piano dei Conti Finanziario',
        iconCls: 'add',
        handler: function() {
          
           var GridImpegni = Ext.getCmp('GridRiepilogoPreimpegni');
           var flag=true;
             Ext.each(GridImpegni.getSelectionModel().getSelections(), function(rec) {
   	             if (flag){
   	                 generaFormCOGAndPdCF_prenotazione(rec.data.ID, rec.data.Codice_Obbiettivo_Gestionale,
   	                               rec.data.PianoDeiContiFinanziario,
   	                               rec.data.Bilancio,rec.data.Capitolo);
   	                flag=false;
   	             }
   	          
              });
          }
    });
    //COG  
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelPrenotazioni = new Ext.FormPanel({
    id:'myPanelPrenotazioni',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Preimpegni",
	tbar: [actionAddPreimpegno, actionDeletePreimpegno, actionConfermaPreimpegno, actionModificaCOGAndPdCFPrenotazione]
});

var labelNumPreImp_pr = new Ext.form.Label({
						  text: 'Numero PreImpegno: ',
						  id: 'labelNumPreImp_pr' 
						 });    


  
var ComboPreimp_pr = new Ext.form.ComboBox({
fieldLabel: 'NumeroPreImpegno',
displayField: 'NumPreImp',
valueField: 'NumPreImp',
id: 'ComboPreimp_pr',
       name: 'ComboPreimp_pr',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Preimpegni ...',
        mode:'local'

});
     

var ComboCOGPreimp_pr=  new Ext.form.ComboBox({
      fieldLabel: 'Cod.Ob.Gest(*)',
       displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOG',
        id: 'ComboCOGPreimp_pr',
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona COG ...'

    });
     
     

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
var panelPreImp_pr = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumPreImp_pr,
									ComboPreimp_pr
								 ]
								}); 
								



//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliPreimpegno(mostraAnno) {
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var capitoloPanelPreimpegno = new Ext.Panel({
								        xtype : "panel",
								        columnWidth: 1,
								        bodyStyle: 'margin-bottom: 10px'
				                    }); 			
    ComboCOGPreimp_pr =  new Ext.form.ComboBox({
        fieldLabel: 'Cod.Ob.Gest(*)',
        displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOGPreimp_pr',
        id: 'ComboCOGPreimp_pr',
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

    //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER PRENOTARE"
    var formCapitoliPreimpegno = new Ext.FormPanel({
              id: 'ElencoCapitoli',
              frame: true,
              title: 'Elenco dei Capitoli',
              width: 800,
              layout: 'absolute',
              monitorValid: true,
              buttons: [{
                  text: 'Salva',
                  id: 'btnCreaPreimpegno',
                  handler: creaPreimpegno,
                  formBind: true
                }, {
                  text: 'Annulla',
                  handler: function(btn, evt) {
                      popupPreimpegno.close();
                  }
                }],
                  items: [
                    capitoloPanelPreimpegno,
                 {
                    xtype: 'panel',
                    title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear) + "</span>",
                    frame: true,
                    layout: 'form',
                    width: 250,
                    x: 0,
                    y: 260,
                    defaults: {
                        width: 132,
                        labelStyle: 'font-size:11px;'
                    },
                    buttons: [{
                        text: 'Pulisci',
                        id: 'button_pulisci1',
                        disabled: true,
                        handler: function() {
                            Ext.getCmp('cog_preimpegno1').reset();
                            Ext.getCmp('pcf_preimpegno1').reset();
                            Ext.getCmp('importo_preimpegno1').reset();
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
                            id: 'cog_preimpegno1',
                            emptyText: 'Seleziona...',
                            name: 'cog_preimpegno1',
                            readOnly: true,
                            disabled: true,
                            allowBlank: true,
                            validateValue: validateBilancio1_pr,
                            blankText: 'Selezionare il codice obiettivo gestionale',
                            mode: 'local',
                            triggerAction: 'all',
                            displayField: 'Id',
                            listWidth: 350,
                            queryMode: 'local',
                            store: storeCogPreimpegni,
                            tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                            style: 'background-color: #fffb8a; background-image:none;'
                        }, {
                            fieldLabel: 'Piano dei conti fin.',
                            xtype: 'combo',
                            id: 'pcf_preimpegno1',
                            emptyText: 'Seleziona...',
                            name: 'pcf_preimpegno1',
                            displayField: 'Id',
                            validateValue: validateBilancio1_pr,
                            listWidth: 350,
                            queryMode: 'local',
                            store: storePCF1Preimpegni,
                            tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                            readOnly: true,
                            disabled: true,
                            mode: 'local',
                            triggerAction: 'all',
                            style: 'background-color: #fffb8a; background-image:none;'
                        }, {
                            fieldLabel: 'Importo preimp. 1',
                            xtype: 'numberfield',
                            decimalSeparator: ',',
                            name: 'importo_preimpegno1',
                            validateValue: validateBilancio1_pr,
                            disabled: true,
                            decimalPrecision: 2,
                            id: 'importo_preimpegno1',
                            style: 'background-color: #fffb8a;background-image:none;'
        }]
                        }, {
                            xtype: 'panel',
                            title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 1) + "</span>",
                            frame: true,
                            layout: 'form',
                            width: 250,
                            x: 251,
                            y: 260,
                            defaults: {
                                width: 132,
                                labelStyle: 'font-size:11px;'
                            },
                            buttons: [{
                                text: 'Pulisci',
                                id: 'button_pulisci2',
                                disabled: true,
                                handler: function() {
                                    Ext.getCmp('cog_preimpegno2').reset();
                                    Ext.getCmp('pcf_preimpegno2').reset();
                                    Ext.getCmp('importo_preimpegno2').reset();
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
                                    id: 'cog_preimpegno2',
                                    emptyText: 'Seleziona...',
                                    validateValue: validateBilancio2_pr,
                                    allowBlank: true,
                                    blankText: 'Selezionare il codice obiettivo gestionale',
                                    name: 'cog_preimpegno2',
                                    readOnly: true,
                                    disabled: true,
                                    mode: 'local',
                                    triggerAction: 'all',
                                    displayField: 'Id',
                                    listWidth: 350,
                                    queryMode: 'local',
                                    store: storeCogPreimpegni,
                                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                    style: 'background-color: #fffb8a; background-image:none;'
                                }, {
                                    fieldLabel: 'Piano dei conti fin.',
                                    xtype: 'combo',
                                    id: 'pcf_preimpegno2',
                                    emptyText: 'Seleziona...',
                                    name: 'pcf_preimpegno2',
                                    validateValue: validateBilancio2_pr,
                                    readOnly: true,
                                    disabled: true,
                                    displayField: 'Id',
                                    listWidth: 350,
                                    queryMode: 'local',
                                    store: storePCF2Preimpegni,
                                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                    mode: 'local',
                                    triggerAction: 'all',
                                    style: 'background-color: #fffb8a; background-image:none;'
                                }, {
                                    fieldLabel: 'Importo preimp. 2',
                                    xtype: 'numberfield',
                                    decimalSeparator: ',',
                                    name: 'importo_preimpegno2',
                                    validateValue: validateBilancio2_pr,
                                    decimalPrecision: 2,
                                    disabled: true,
                                    id: 'importo_preimpegno2',
                                    style: 'background-color: #fffb8a;background-image:none;'
        }]
                                }, {
                                    xtype: 'panel',
                                    title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 2) + "</span>",
                                    frame: true,
                                    layout: 'form',
                                    width: 250,
                                    x: 502,
                                    y: 260,
                                    defaults: {
                                        width: 132,
                                        labelStyle: 'font-size:11px;'
                                    },
                                    buttons: [{
                                        text: 'Pulisci',
                                        id: 'button_pulisci3',
                                        disabled: true,
                                        handler: function() {
                                            Ext.getCmp('cog_preimpegno3').reset();
                                            Ext.getCmp('pcf_preimpegno3').reset();
                                            Ext.getCmp('importo_preimpegno3').reset();
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
                                            id: 'cog_preimpegno3',
                                            emptyText: 'Seleziona...',
                                            name: 'cog_preimpegno3',
                                            validateValue: validateBilancio3_pr,
                                            readOnly: true,
                                            disabled: true,
                                            allowBlank: true,
                                            blankText: 'Selezionare il codice obiettivo gestionale',
                                            mode: 'local',
                                            triggerAction: 'all',
                                            displayField: 'Id',
                                            listWidth: 350,
                                            queryMode: 'local',
                                            store: storeCogPreimpegni,
                                            tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                            style: 'background-color: #fffb8a; background-image:none;'
                                        }, {
                                            fieldLabel: 'Piano dei conti fin.',
                                            xtype: 'combo',
                                            id: 'pcf_preimpegno3',
                                            emptyText: 'Seleziona...',
                                            name: 'pcf_preimpegno3',
                                            validateValue: validateBilancio3_pr,
                                            displayField: 'Id',
                                            listWidth: 350,
                                            queryMode: 'local',
                                            store: storePCF3Preimpegni,
                                            tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                                            readOnly: true,
                                            disabled: true,
                                            mode: 'local',
                                            triggerAction: 'all',
                                            style: 'background-color: #fffb8a; background-image:none;'
                                        }, {
                                            fieldLabel: 'Importo preimp. 3',
                                            xtype: 'numberfield',
                                            decimalSeparator: ',',
                                            validateValue: validateBilancio3_pr,
                                            decimalPrecision: 2,
                                            name: 'importo_preimpegno3',
                                            disabled: true,
                                            id: 'importo_preimpegno3',
                                            style: 'background-color: #fffb8a;background-image:none;'
        }]
        }]
              });

    

     var popupPreimpegno = new Ext.Window({ y:15,
					                title: 'Aggiungi un nuovo preimpegno',
					                id: 'popup_preimpegno',
					                width: 782,
					                height: 590,
					                layout: 'fit',
					                plain: true,
					                buttonAlign: 'center',
					                maximizable: true,
					                tbar: ['->', actionHelpOnLinePreimpegno],
					                enableDragDrop   : false,
					                collapsible: false,
					                modal:true,
					                closable: true  /* * toglie la croce per chiudere la finestra */
					     	       });
					                popupPreimpegno.add(formCapitoliPreimpegno);
					                popupPreimpegno.doLayout(); //forzo ridisegno
					         
	

    //PRENDO L'ANNO DALLA DATA ODIERNA
 		var dtOggi=new Date()
		dtOggi.setDate(dtOggi.getDate());
		var fAnno = dtOggi.getFullYear();
		
		//RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
	 	var GridCapitoliPrenotazione = buildGridStartPrenotazione(fAnno);
	    capitoloPanelPreimpegno.add(GridCapitoliPrenotazione);
		// SETTO panelImp INVISIBILE 
        panelPreImp_pr.hide();
       
    popupPreimpegno.show();

 
}// fine InitFormCapitoli

function VerificaDisponibilitaP() {
    try {
      
        Ext.get('ImpPrenotato').dom.value = 0;
        //var num=Ext.get('NumPreImp').dom.value;
        var num = Ext.get('ComboPreimp_pr').dom.value;
        Ext.getCmp('btnAggiungi').show();
    } catch (ex) { 
    }
}




function buildComboCOG_Pr(AnnoRif, CapitoloRif) {
   var formCapitoliPreimpegno = Ext.getCmp('detCapitolo');
   
   // formCapitoliPreimpegno.remove(ComboCOGPreimp_pr,true);
  ComboCOGPreimp_pr.el.parent().parent().parent().remove()


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
                maskPreimpegni.hide();
            },
            scope: this
        }
    });
    store.load({ params: parametri });


   


 ComboCOGPreimp_pr=  new Ext.form.ComboBox({
      fieldLabel: 'Cod.Ob.Gest(*)',
       displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOGPreimp_pr',
        id: 'ComboCOGPreimp_pr',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona COG ...'

    });
     
  formCapitoliPreimpegno.insert(12,ComboCOGPreimp_pr);  
  formCapitoliPreimpegno.doLayout();
    ComboCOGPreimp_pr.show();

   ComboCOGPreimp_pr.on('select', function(record, index) { selectComboCOG_prenotazione(record, index)});
    return ComboCOGPreimp_pr;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartPrenotazione(annoRif) {
	var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetElencoCapitoliAnno',
    method:'GET'
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
           ]// chiude fields
       }); // chiude reader
	
	var store = new Ext.data.Store({
	    proxy: proxy,
	    reader: reader,
	    listeners: {
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
           maskPreimpegni.hide();
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
                    { header: "UPB", width: 50, dataIndex: 'UPB', sortable: true },
                    { header: "Missione.Programma", width: 80, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width: 300, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	                //,
	            	//{ renderer: eurRend,header: "Importo<br/>Potenziale",width: 60,dataIndex: 'ImpPotenzialePrenotato', sortable: true },
	                //{ renderer: eurRend,header: "Importo<br/>Disponibile",width: 60,dataIndex: 'ImpDisp', sortable: true }

	            	] );

	            	var GridCapitoliPrenotazione = new Ext.grid.GridPanel({
	                    id: 'GridCapitoliPrenotazione',
	                    autoExpandColumn: 'Capitolo',
	                    width: 754,
	                    height: 245,
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
	                                    resetFormPrenotazioni();
	                                    Ext.getCmp('disponibilita1').setValue(eurRend(rows[0].data.ImpDisp));
	                                    Ext.getCmp('disponibilita2').setValue('Attendere...');
	                                    Ext.getCmp('disponibilita3').setValue('Attendere...');

	                                    // setImpDisp_Pr(Ext.getCmp('disponibilita1'), rows[0].data.Capitolo, Ext.getCmp('bilancio1').getValue());
	                                    setImpDisp_Pr(Ext.getCmp('disponibilita2'), rows[0].data.Capitolo, Ext.getCmp('bilancio2').getValue());
	                                    setImpDisp_Pr(Ext.getCmp('disponibilita3'), rows[0].data.Capitolo, Ext.getCmp('bilancio3').getValue());

	                                    var params = {
	                                        AnnoRif: rows[0].data.Bilancio,
	                                        CapitoloRif: rows[0].data.Capitolo
	                                    };

	                                    	                                    
	                                    storeCogPreimpegni.reload({ params: params });
	                                    storePCF1Preimpegni.reload({ params: params });

	                                    var params2 = {
	                                        AnnoRif: Ext.getCmp('bilancio2').getValue(),
	                                        CapitoloRif: rows[0].data.Capitolo
	                                    };

	                                    storePCF2Preimpegni.reload({ params: params2 });

	                                    var params3 = {
	                                        AnnoRif: Ext.getCmp('bilancio3').getValue(),
	                                        CapitoloRif: rows[0].data.Capitolo
	                                    };

	                                    storePCF3Preimpegni.reload({ params: params3 });

	                                    abilitaBilancioPrenotazioni(true);
	                                }
	                            }
	                        }
	                    })
	                });

	                return GridCapitoliPrenotazione;
} //chiude buildGridStartPrenotazione


function setImpDisp_Pr(disponibilita,capImpDisp, bilancioImpDisp) {
    
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

} // chiude setImpDisp_Pr

function enableConfirmAction_Pr(selectedRows) {
    var retValue = false

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.Stato == 2))
        break;

    return retValue;
}



//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridPreimpegni() {
    
    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetPreImpegniRegistratiProvvisori' + window.location.search,
    method:'GET'
    });

    var reader = new Ext.data.JsonReader({

        root: 'GetPreImpegniRegistratiProvvisoriResult',
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
           { name: 'PianoDeiContiFinanziario' }
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

     store.on({
       'load':{
          fn: function(store, records, options){
           maskPreimpegni.hide();
         },
          scope:this
  	     }
 	    });

        var ufficio =  Ext.get('Cod_uff_Prop').dom.value;
        var parametri = { CodiceUfficio: ufficio };
         store.load({params:parametri});
       
	     var summary = new Ext.grid.GroupSummary();


	     var sm = new Ext.grid.CheckboxSelectionModel(
 	        { singleSelect: false,
 	            listeners: {
 	                rowselect: function(sm, row, rec) {
 	                    var multiSelect = sm.getSelections().length > 1;

 	                    var currentYear = new Date().getYear();
 	                    if (currentYear < 1900) currentYear += 1900;

 	                    actionAddPreimpegno.setDisabled(true);

 	                    if (rec.data.Stato == 2) { 
 	                        actionDeletePreimpegno.setDisabled(multiSelect);
     	                   
 	                        actionConfermaPreimpegno.setDisabled(false);
 	                        actionModificaCOGAndPdCFPrenotazione.setDisabled(multiSelect);
 	                    } else { 
 	                        actionDeletePreimpegno.setDisabled(multiSelect);
     	                   
 	                        actionModificaCOGAndPdCFPrenotazione.setDisabled(multiSelect);
 	                    }
 	                },
 	                rowdeselect: function(sm, row, rec) {
 	                    var selectedRowsCount = sm.getSelections().length;

 	                    var currentYear = new Date().getYear();
 	                    if (currentYear < 1900) currentYear += 1900; 
     	                    
 	                    actionAddPreimpegno.setDisabled(selectedRowsCount == 0 ? false : true);

 	                    if (selectedRowsCount == 1) {
 	                        if (sm.getSelected().data.Stato == 2) {
 	                            actionDeletePreimpegno.setDisabled(false);
 	                            actionModificaCOGAndPdCFPrenotazione.setDisabled(false);
 	                        } else { 	                        
 	                            actionDeletePreimpegno.setDisabled(false);
     	                        
 	                            actionModificaCOGAndPdCFPrenotazione.setDisabled(false);
 	                        }
 	                    } else {
 	                        actionDeletePreimpegno.setDisabled(true);
 	                        actionModificaCOGAndPdCFPrenotazione.setDisabled(true);
 	                    }

 	                    actionConfermaPreimpegno.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction_Pr(sm.getSelections()));
 	                }
 	            }
 	        }
 	    );
         var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Bilancio", dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true ,
		                  summaryRenderer: function(v, params, data){return '<b> Totale </b>';} },
		                { header: "Capitolo", dataIndex: 'Capitolo', sortable: true, locked: false },
	                    { header: "UPB", dataIndex: 'UPB', sortable: true },
	                    { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		            	{ renderer: eurRend, header: "Importo da Prenotare", align: 'right', dataIndex: 'ImpPrenotato', sortable: true
		            	 , summaryType:'sum'},
		            	{ header: "Numero PreImpegno", dataIndex: 'NumPreImp', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true} ,
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true}  ,
		            	{ header: "COG", dataIndex: 'Codice_Obbiettivo_Gestionale', sortable: true },
		            	{ header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario', sortable: true },
		            	{ header: "Stato", dataIndex: 'StatoAsString' }		            			            			            	
		    ]);
		   		            	
		    var GridRiepilogoPreimpegni = new Ext.grid.GridPanel({ 
				    	id: 'GridRiepilogoPreimpegni',
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
	           actionDeletePreimpegno.setDisabled(true); 
 	           actionConfermaPreimpegno.setDisabled(true);
 	           actionModificaCOGAndPdCFPrenotazione.setDisabled(true);

 	           return GridRiepilogoPreimpegni; 
}



//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
// ex ConfermaMultiplaPreImpegno
function ConfermaMultiplaPrenotazione(ImpegniInfo) {
    var CodiceUfficio = Ext.get('Cod_uff_Prop').dom.value;

    var params = { ImpegniInfo: ImpegniInfo, CodiceUfficio: CodiceUfficio };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaPrenotazione',
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            mask.hide();
            Ext.getCmp('GridRiepilogoPreimpegni').getStore().reload();
            try {               
                actionDeletePreimpegno.setDisabled(true);
                actionConfermaPreimpegno.setDisabled(true);
                actionModificaCOGAndPdCFPrenotazione.setDisabled(true);
                actionAddPreimpegno.setDisabled(false);                                                                                
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
                        maskPreimpegni.hide();
                        Ext.getCmp('GridRiepilogoPreimpegni').getStore().reload();
                        try {
                            actionDeletePreimpegno.setDisabled(true);
                            actionConfermaPreimpegno.setDisabled(true);
                            actionModificaCOGAndPdCFPrenotazione.setDisabled(true);
                            actionAddPreimpegno.setDisabled(false);
                        } catch (ex) { }
                    }
                }
            });
        }
    });
}
// ex ConfermaPreImpegno
function ConfermaPrenotazione(ID) {
    var CodiceUfficio =Ext.get('Cod_uff_Prop').dom.value

	var params = {ID:ID,CodiceUfficio:CodiceUfficio};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaPrenotazione',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           maskPreimpegni.hide();
           Ext.getCmp('GridRiepilogoPreimpegni').getStore().reload();          
       },
      failure: function(response,result, options) {
            maskPreimpegni.hide();
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

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function EliminaPreImpegnoProvvisorio(NumPreImpegno, ID) {
    var params = { NumeroPreImpegno: NumPreImpegno, ID: ID };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/EliminaPreImpegnoProvvisorio' + window.location.search,
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            maskImp.hide();
        },
        failure: function(result, response, options) {
            maskImp.hide();
            //   var data = Ext.decode(result.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: Ext.decode(result.responseText).FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) { return }
            });
        }
    });
}



function NascondiColonnePrenotazioni() {
    var index = Ext.getCmp('GridRiepilogoPreimpegni').colModel.getColumnCount(false);
    index = index-1;
    Ext.getCmp('GridRiepilogoPreimpegni').colModel.setHidden(index,true) 
    actionConfermaPreimpegno.hide();
}

/*
 * Rende editabile solo la parte dell'impegno pluriennale
 */
function abilitaBilancioPrenotazioni(abilita) {
    if (abilita) {
       
        Ext.getCmp('cog_preimpegno1').enable();
        Ext.getCmp('pcf_preimpegno1').enable();
        Ext.getCmp('importo_preimpegno1').enable();
        Ext.getCmp('button_pulisci1').enable();
        
        Ext.getCmp('cog_preimpegno2').enable();
        Ext.getCmp('pcf_preimpegno2').enable();
        Ext.getCmp('importo_preimpegno2').enable();
        Ext.getCmp('button_pulisci2').enable();

        Ext.getCmp('cog_preimpegno3').enable();
        Ext.getCmp('pcf_preimpegno3').enable();
        Ext.getCmp('importo_preimpegno3').enable();
        Ext.getCmp('button_pulisci3').enable();
    } else {
  
        Ext.getCmp('cog_preimpegno1').clearValue();
        Ext.getCmp('cog_preimpegno1').disable();
        Ext.getCmp('pcf_preimpegno1').clearValue();
        Ext.getCmp('pcf_preimpegno1').disable();
        Ext.getCmp('importo_preimpegno1').disable();
        Ext.getCmp('importo_preimpegno1').reset();
        Ext.getCmp('button_pulisci1').disable();

        Ext.getCmp('cog_preimpegno2').clearValue();
        Ext.getCmp('cog_preimpegno2').disable();
        Ext.getCmp('pcf_preimpegno2').clearValue();
        Ext.getCmp('pcf_preimpegno2').disable();
        Ext.getCmp('importo_preimpegno2').disable();
        Ext.getCmp('importo_preimpegno2').reset();
        Ext.getCmp('button_pulisci2').disable();

        Ext.getCmp('cog_preimpegno3').clearValue();
        Ext.getCmp('cog_preimpegno3').disable();
        Ext.getCmp('pcf_preimpegno3').clearValue();
        Ext.getCmp('pcf_preimpegno3').disable();
        Ext.getCmp('importo_preimpegno3').disable();
        Ext.getCmp('importo_preimpegno3').reset();
        Ext.getCmp('button_pulisci3').disable();
    }
}

function resetFormPrenotazioni() {
    storeCogPreimpegni.removeAll();
 
    Ext.getCmp('cog_preimpegno1').clearValue();
    Ext.getCmp('pcf_preimpegno1').clearValue();
    Ext.getCmp('importo_preimpegno1').reset();

    Ext.getCmp('cog_preimpegno2').clearValue();
    Ext.getCmp('pcf_preimpegno2').clearValue();
    Ext.getCmp('importo_preimpegno2').reset();

    Ext.getCmp('cog_preimpegno3').clearValue();
    Ext.getCmp('pcf_preimpegno3').clearValue();
    Ext.getCmp('importo_preimpegno3').reset();
    
    abilitaBilancioPrenotazioni(true);
}

function creaPreimpegno(btn, evt) {
    var rows = Ext.getCmp('GridCapitoliPrenotazione').getSelectionModel().getSelections();

    if (rows.length != 1) {
        Ext.MessageBox.show({
            title: 'Attenzione',
            msg: 'Selezionare un capitolo',
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.WARNING
        });  
        return;
    }
    
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
        var preimpegno1 = 0;
        preimpegno1 = parseFloat(Ext.getCmp('importo_preimpegno1').getValue());
        var disponibilita1 = 0;
        disponibilita1 = parseFloat(
                    replaceAll_pr(Ext.getCmp('disponibilita1').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
        var obgestionale1 = Ext.getCmp('cog_preimpegno1').getValue();
        var pcf1 = Ext.getCmp('pcf_preimpegno1').getValue();


        if (!isNaN(preimpegno1)) {
            if (disponibilita1 <= 0 || preimpegno1 > disponibilita1) {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Preimpegno 1: L\'importo indicato (' + eurRend(preimpegno1) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita1) + ")",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return;
            }
        }


        var preimpegno2 = 0;
        preimpegno2 = parseFloat(Ext.getCmp('importo_preimpegno2').getValue());
        var disponibilita2 = 0;
        disponibilita2 = parseFloat(
                    replaceAll_pr(Ext.getCmp('disponibilita2').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
        var obgestionale2 = Ext.getCmp('cog_preimpegno2').getValue();
        var pcf2 = Ext.getCmp('pcf_preimpegno2').getValue();

        if (!isNaN(preimpegno2)) {
            if (disponibilita2 <= 0 || preimpegno2 > disponibilita2) {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Preimpegno 2: L\'importo indicato (' + eurRend(preimpegno2) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita2) + ")",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return;
            }
        }

        var preimpegno3 = 0;
        preimpegno3 = parseFloat(Ext.getCmp('importo_preimpegno3').getValue());
        var disponibilita3 = 0;
        disponibilita3 = parseFloat(
                    replaceAll_pr(Ext.getCmp('disponibilita3').getValue().replace('€', ''), '[.]', '').replace(',', '.'));
        var obgestionale3 = Ext.getCmp('cog_preimpegno3').getValue();
        var pcf3 = Ext.getCmp('pcf_preimpegno3').getValue();

        if (!isNaN(preimpegno3)) {
            if (disponibilita3 <= 0 || preimpegno3 > disponibilita3) {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Preimpegno 3: L\'importo indicato (' + eurRend(preimpegno3) + ') supera la disponibilità sul capitolo (' + eurRend(disponibilita3) + ")",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return;
            }
        }

        creaPreimpegnoPluriennale(rows[0].data.Capitolo, rows[0].data.UPB, rows[0].data.MissioneProgramma, rows[0].data.Bilancio, preimpegno1, obgestionale1, pcf1,
                    preimpegno2, obgestionale2, pcf2, preimpegno3, obgestionale3, pcf3)
    }
}

function creaPreimpegnoPluriennale(capitolo, upb, missioneProgramma, bilancio, importo1, obgestionale1, pcf1,
    importo2, obgestionale2, pcf2, importo3, obgestionale3, pcf3) {
    
    Ext.getCmp('btnCreaPreimpegno').disable();
    Ext.Ajax.request({
        url: "ProcAmm.svc/GenerazionePreImpegnoProvvisorio" + window.location.search,
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        params: {
            Capitolo: capitolo,
            ComboPreimp_pr: 0,
            Importo1 : importo1,
            ObGestionale1: obgestionale1,
            PCF1: pcf1,
            Importo2: importo2,
            ObGestionale2: obgestionale2,
            PCF2: pcf2,
            Importo3: importo3,
            ObGestionale3: obgestionale3,
            PCF3: pcf3,
            UPB: upb,
            MissioneProgramma: missioneProgramma,
            Bilancio: bilancio
        },
        method: 'POST',
        success: function(response, opts) {
            var result = Ext.decode(response.responseText);
            if (result.success) {
                Ext.MessageBox.show({
                    title: 'Creazione preimpegno',
                    msg: 'Generazione preimpegno avvenuta con succeso',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.INFO,
                    fn: function(btn) {
                        Ext.getCmp('btnCreaPreimpegno').enable();      
                        Ext.getCmp('GridRiepilogoPreimpegni').getStore().reload();
                        Ext.getCmp('popup_preimpegno').close();
                    }
                });
            } else {
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: result.FaultMessage,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                        Ext.getCmp('btnCreaPreimpegno').enable();      
                    }
                });
            }
        },
        failure: function(response, opts) {
            Ext.MessageBox.show({
                title: 'Errore',
                msg: 'Impossibile effettuare l\'operazione',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                        Ext.getCmp('btnCreaPreimpegno').enable();      
                }
            });
        }
    });
}




/* Per ogni bilancio o tutti o nessun campo compilato */
function validateBilancio1_pr(value) {
    var result = false;

    result = Ext.getCmp('cog_preimpegno1').getValue() != ""
            && Ext.getCmp('pcf_preimpegno1').getValue() != ""
            && Ext.getCmp('importo_preimpegno1').getValue() != ""
            && parseFloat(Ext.getCmp('importo_preimpegno1').getValue()) > 0;

    result = result || (Ext.getCmp('cog_preimpegno1').getValue() == ""
            && Ext.getCmp('pcf_preimpegno1').getValue() == ""
            && Ext.getCmp('importo_preimpegno1').getValue() == ""
            && (Ext.getCmp('importo_preimpegno3').getValue() != ""
                || Ext.getCmp('importo_preimpegno2').getValue() != ""));

    return result;
}

function validateBilancio2_pr(value) {
    var result = false;

    result = Ext.getCmp('cog_preimpegno2').getValue() != ""
            && Ext.getCmp('pcf_preimpegno2').getValue() != ""
            && Ext.getCmp('importo_preimpegno2').getValue() != ""
             && parseFloat(Ext.getCmp('importo_preimpegno2').getValue()) > 0;

    result = result || (Ext.getCmp('cog_preimpegno2').getValue() == ""
            && Ext.getCmp('pcf_preimpegno2').getValue() == ""
            && Ext.getCmp('importo_preimpegno2').getValue() == ""
            && (Ext.getCmp('importo_preimpegno3').getValue() != ""
                || Ext.getCmp('importo_preimpegno1').getValue() != ""));

    return result;
}

function validateBilancio3_pr(value) {
    var result = false;

    result = Ext.getCmp('cog_preimpegno3').getValue() != ""
            && Ext.getCmp('pcf_preimpegno3').getValue() != ""
            && Ext.getCmp('importo_preimpegno3').getValue() != ""
             && parseFloat(Ext.getCmp('importo_preimpegno3').getValue()) > 0;

    result = result || (Ext.getCmp('cog_preimpegno3').getValue() == ""
            && Ext.getCmp('pcf_preimpegno3').getValue() == ""
            && Ext.getCmp('importo_preimpegno3').getValue() == "" 
            && (Ext.getCmp('importo_preimpegno2').getValue() != ""
                || Ext.getCmp('importo_preimpegno1').getValue() != ""));

    return result;
}

function replaceAll_pr(txt, replace, with_this) {
    return txt.replace(new RegExp(replace, 'g'), with_this);
}