Ext.override(Ext.form.NumberField, {
    setFieldLabel: function(text) {
        if (this.rendered) {
            this.el.up('.x-form-item', 10, true).child('.x-form-item-label').update(text);
        }
        this.fieldLabel = text;
    }
});


function creaRidEco(btn, evt) {
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
        var impegno1 = parseFloat(Ext.getCmp('impegno1').getValue());
        var impegno2 = parseFloat(Ext.getCmp('impegno2').getValue());
        var impegno3 = parseFloat(Ext.getCmp('impegno3').getValue());

        if (isNaN(impegno1) && isNaN(impegno2) && isNaN(impegno3)) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Selezionare almeno un impegno per effettuare la riduzione',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        } else {
        
        var disponilibitaImp1 = Ext.getCmp('disponilibitaImp1').getValue();
        var importo_riduzione1 = parseFloat(Ext.getCmp('importo_riduzione1').getValue());
        var tipoAtto1 = Ext.getCmp('tipo_atto1').getValue();
        var dataAtto1 = Ext.getCmp('data_atto1').getValue();
        var numeroAtto1 = Ext.getCmp('numero_atto1').getValue();
        var isEconomia1 = Ext.getCmp('isEconomia1').getValue();


        var disponilibitaImp2 = Ext.getCmp('disponilibitaImp2').getValue();
        var importo_riduzione2 = parseFloat(Ext.getCmp('importo_riduzione2').getValue());
        var tipoAtto2 = Ext.getCmp('tipo_atto2').getValue();
        var dataAtto2 = Ext.getCmp('data_atto2').getValue();
        var numeroAtto2 = Ext.getCmp('numero_atto2').getValue();
        var isEconomia2 = Ext.getCmp('isEconomia2').getValue();


        var disponilibitaImp3 = Ext.getCmp('disponilibitaImp3').getValue();
        var importo_riduzione3 = parseFloat(Ext.getCmp('importo_riduzione3').getValue());
        var tipoAtto3 = Ext.getCmp('tipo_atto3').getValue();
        var dataAtto3 = Ext.getCmp('data_atto3').getValue();
        var numeroAtto3 = Ext.getCmp('numero_atto3').getValue();
        var isEconomia3 = Ext.getCmp('isEconomia3').getValue();

        creaRiduzionePluriennale(rows[0].data.Capitolo, rows[0].data.UPB, rows[0].data.MissioneProgramma, rows[0].data.Bilancio, 
        impegno1, disponilibitaImp1, importo_riduzione1, tipoAtto1, dataAtto1, numeroAtto1, isEconomia1,
        impegno2, disponilibitaImp2, importo_riduzione2, tipoAtto2, dataAtto2, numeroAtto2, isEconomia2,
        impegno3, disponilibitaImp3, importo_riduzione3, tipoAtto3, dataAtto3, numeroAtto3, isEconomia3);
        }
   
    }

    function creaRiduzionePluriennale(capitolo, upb, missioneProgramma, bilancio, impegno1, disponilibitaImp1, importo_riduzione1, tipoAtto1, dataAtto1, numeroAtto1, isEconomia1,
        impegno2, disponilibitaImp2, importo_riduzione2, tipoAtto2, dataAtto2, numeroAtto2, isEconomia2,
        impegno3, disponilibitaImp3, importo_riduzione3, tipoAtto3, dataAtto3, numeroAtto3, isEconomia3) {

        Ext.getCmp('btnAggiungi').disable();
        Ext.Ajax.request({
            url: "ProcAmm.svc/GenerazioneRiduzioniMultiple" + window.location.search,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            params: {
                Capitolo: capitolo,
                UPB: upb,
                MissioneProgramma: missioneProgramma,
                Bilancio: bilancio,

                DataAtto1: dataAtto1,
                TipoAtto1: tipoAtto1,
                NumeroAtto1: numeroAtto1,
                NumImpegno1: impegno1,
                ImpDisp1: disponilibitaImp1,
                ImpDaRidurre1: importo_riduzione1,
                IsEconomia1: isEconomia1,
                
                DataAtto2: dataAtto2,
                TipoAtto2: tipoAtto2,
                NumeroAtto2: numeroAtto2,
                NumImpegno2: impegno2,
                ImpDisp2: disponilibitaImp2,
                ImpDaRidurre2: importo_riduzione2,
                IsEconomia2: isEconomia2,
                
                
                DataAtto3: dataAtto3,
                TipoAtto3: tipoAtto3,
                NumeroAtto3: numeroAtto3,
                NumImpegno3: impegno3,
                ImpDisp3: disponilibitaImp3,
                ImpDaRidurre3: importo_riduzione3,
                IsEconomia3: isEconomia3
            },
            method: 'POST',
            success: function(response, opts) {
                var result = Ext.decode(response.responseText);
                if (result.success) {
                    Ext.MessageBox.show({
                        title: 'Riduzione',
                        msg: 'Riduzioni avvenute con succeso',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function(btn) {
                          Ext.getCmp('btnAggiungi').enable();
                          Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
                          Ext.getCmp('popup_riduzione').close();
                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: result.FaultMessage,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) {
                            Ext.getCmp('btnAggiungi').enable();
                            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
                            Ext.getCmp('popup_riduzione').close();
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
                        Ext.getCmp('btnAggiungi').enable();
                    }
                });
            }
        });
    }


    function ConfermaRiduzione(ID) {
        var params = { ID: ID };

        Ext.lib.Ajax.defaultPostHeader = 'application/json';
        Ext.Ajax.request({
            url: 'ProcAmm.svc/ConfermaRiduzione',
            params: Ext.encode(params),
            method: 'POST',
            success: function(result, response, options) {
                mask.hide();
                Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
            },
            failure: function(response, result, options) {
                mask.hide();
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


function abilitaFormBilanci(abilita) {
    if (abilita) {
        Ext.getCmp('button_pulisci1').enable();
        Ext.getCmp('button_pulisci2').enable();
        Ext.getCmp('button_pulisci3').enable();
    } else {
        Ext.getCmp('cog_impegno1').reset();
        Ext.getCmp('cog_impegno1').disable();
        Ext.getCmp('importo_riduzione1').disable();
        Ext.getCmp('importo_riduzione1').reset();
        Ext.getCmp('button_pulisci1').disable();

        Ext.getCmp('cog_impegno2').reset();
        Ext.getCmp('cog_impegno2').disable();
        Ext.getCmp('importo_riduzione2').disable();
        Ext.getCmp('importo_riduzione2').reset();
        Ext.getCmp('button_pulisci2').disable();

        Ext.getCmp('cog_impegno3').reset();
        Ext.getCmp('cog_impegno3').disable();
        Ext.getCmp('importo_riduzione3').disable();
        Ext.getCmp('importo_riduzione3').reset();
        Ext.getCmp('button_pulisci3').disable();
    }
}

var actionHelpOnLineRiduz = new Ext.Action({
    text: 'Aiuto',
    tooltip: 'Aiuto in linea: generazione riduzione e\/o economia',
    handler: function() {
        new Ext.IframeWindow({
            modal: true,
            layout: 'fit',
            title: 'Generazione di una riduzione e\/o economia',
            width: 720,
            height: 500,
            closable: true,
            resizable: false,
            maximizable: false,
            plain: false,
            iconCls: 'help',
            bodyStyle: 'overflow:auto',
            src: 'risorse/helpBilancioPluriennaleRiduz.htm'
        }).show();
    },
    iconCls: 'help'
});

var storeImpegni1 = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImp',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaImpResult',
        fields: [
           { name: 'NumImpegno' },
           { name: 'ImpDisp' },
           { name: 'TipoAtto' },
           { name: 'DataAtto' },
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' },
           { name: 'Codice_Obbiettivo_Gestionale' }
           ]
    }),
    listeners: {
        beforeload: function(store, options) {
            Ext.getCmp('impegno1').disable();
        },
        load: function(store, records, options) {
            Ext.getCmp('impegno1').enable();
        }
    }
});

var storeImpegni2 = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImp',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaImpResult',
        fields: [
           { name: 'NumImpegno' },
           { name: 'ImpDisp' },
           { name: 'TipoAtto' },
           { name: 'DataAtto' },
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' },
           { name: 'Codice_Obbiettivo_Gestionale' }
           ]
    }),
    listeners: {
        beforeload: function(store, options) {
            Ext.getCmp('impegno2').disable();
        },
        load: function(store, records, options) {
            Ext.getCmp('impegno2').enable();
        }
    }
});

var storeImpegni3 = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImp',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaImpResult',
        fields: [
           { name: 'NumImpegno' },
           { name: 'ImpDisp' },
           { name: 'TipoAtto' },
           { name: 'DataAtto' },
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' },
           { name: 'Codice_Obbiettivo_Gestionale' }
           ]
    }),
    listeners: {
        beforeload: function(store, options) {
            Ext.getCmp('impegno3').disable();
        },
        load: function(store, records, options) {
            Ext.getCmp('impegno3').enable();
        }
    }
});

storeImpegni1.setDefaultSort("NumImpegno", "DESC");
storeImpegni2.setDefaultSort("NumImpegno", "DESC");
storeImpegni3.setDefaultSort("NumImpegno", "DESC");

//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaRiduzione = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function() {
        var riduzioniDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridRiepilogoRiduzione').getSelectionModel().getSelections(), function(rec) {
            //conferma solo le riduzione da confermare, quelle aventi stato 2
            if (rec.data['Stato'] == 2) {
                var riduzioneInfo = new Object();

                riduzioneInfo.ID = rec.data['ID'];
                riduzioneInfo.Stato = rec.data['Stato'];
                riduzioneInfo.NumImpegno = rec.data['NumImpegno'];

                riduzioniDaComfermare.push(riduzioneInfo);
            }
        });

        ConfermaMultiplaRiduzione(riduzioniDaComfermare);
    },
    iconCls: 'save'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
  var actionAddRiduzione = new Ext.Action({
         text:'Aggiungi',
         tooltip:'Aggiungi una nuova riga',
         handler: function() {
         var mostraAnno = false;
         InitFormCapitoliRiduzione(mostraAnno);
        },
        iconCls: 'add'
    });
  
  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA  
    var actionDeleteRiduzione = new Ext.Action({
         text:'Cancella',
         tooltip:'Cancella selezionato',
        handler: function(){
            var storeGridImp=Ext.getCmp('GridRiepilogoRiduzione').getStore();
            Ext.each(Ext.getCmp('GridRiepilogoRiduzione').getSelectionModel().getSelections(), function(rec) {
                EliminaRiduzione(rec.data['NumImpegno'], rec.data['ID']);
                storeGridImp.remove(rec);
            });
        },
        iconCls: 'remove'
    });

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelRiduzioni = new Ext.FormPanel({
    id:'myPanelRiduzioni',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni registrate",
	tbar:[actionAddRiduzione,actionDeleteRiduzione, actionConfermaRiduzione]
});

var labelNumImpRid = new Ext.form.Label({
						  text: 'Numero Impegno da Ridurre: ',
						  id: 'labelNumImpRid' 
						 });    


  
var ComboImpRid = new Ext.form.ComboBox({
fieldLabel: 'NumeroImpegnoRid',
displayField: 'NumImpegno',
valueField: 'NumImpegno',
id: 'ComboImpRid',
       name: 'ComboImpRid',
       mode: 'local',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Impegni ...'

});
     
//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
var panelImpRid = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumImpRid,
									ComboImpRid
								 ]
								}); 
								
//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliRiduzione(mostraAnno) {

  



var AnnoBilancioRiduzione= new Ext.ux.YearMenu({
   	  			    format: 'Y',
			        id: 'AnnoBilancio',
			        allowBlank: false,
			        noPastYears: false,
			        noPastMonths: true,
			        minDate: new Date('1990/1/1'),
			        maxDate: new Date()
			       ,
			        handler : function(dp, newValue, oldValue){
			         	Ext.getCmp('AnnoIni').value= newValue.format('Y');
           		 		Ext.get('AnnoIni').dom.value= newValue.format('Y');
           		 		}
   			        });
var btnRicercaRiduzione = new Ext.Button({
						  text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
						    id: 'btnBilancio'
						});


						btnRicercaRiduzione.on('click', function() {

						    var fNewAnno = Ext.getCmp("AnnoIni").value;
						    //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
						    gridCapitoli = buildGridStartRiduzione(fNewAnno);
						    capitoloPanelRiduzione.remove('GridCapitoli');
						    capitoloPanelRiduzione.add(gridCapitoli);
						     capitoloPanelRiduzione.doLayout();
						});  

var tbarDateRiduzione = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancioRiduzione},
        			 {
			        xtype: 'textfield',
			       	cls: 'titfis',
			        readOnly: true,
			        id: 'AnnoIni',
			        width: 80,
 			        value: new Date().format('Y')
			    },
			    btnRicercaRiduzione
			 ]
	});
	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelRiduzione = new Ext.Panel({
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 			
						
labelNumImpRid = new Ext.form.Label({
						  text: 'Numero Impegno da Ridurre: ',
						  id: 'labelNumImpRid' 
						 });    


  
ComboImpRid = new Ext.form.ComboBox({
fieldLabel: 'NumeroImpegnoRid',
displayField: 'NumImpegno',
valueField: 'NumImpegno',
id: 'ComboImpRid',
       name: 'ComboImpRid',
       mode: 'local',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Impegni ...'

});
     
//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
 panelImpRid = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumImpRid,
									ComboImpRid
								 ]
								}); 
 
 //DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanelRiduzione = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{
								    layout : "fit"
								}],
								buttons: [{
									text: 'Aggiungi Riduzione',
									id: 'btnAggiungi'
								}]
					}); 

 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
					var storeEconomia = new Ext.data.SimpleStore({
					    fields: ['value', 'description'],
					    data: [['0', 'Riduzione'], ['1', 'Economia']]
					});
					var comboEconomia = new Ext.form.ComboBox({
					id: 'comboEconomia',
					//name: 'comboEconomia',
					hiddenName: 'hiddenComboEconomia',
					    fieldLabel:  'Operazione(*)',
					    store: storeEconomia,
					    displayField: 'description',
					    valueField: 'value',
					    typeAhead: false,
					    mode: 'local',
					    triggerAction: 'all',
					    emptyText: 'Seleziona Tipo Operazione...',
					    selectOnFocus: true
					  
					});
 var formCapitoliRiduzione = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per riduzione',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 750,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDateRiduzione
        		],
            items: [capitoloPanelRiduzione]
            },
            {
        	columnWidth: 0.4,
            xtype: 'fieldset',
            id: 'detCapitolo',
            labelWidth: 90,
            title:'Dettaglio',
            defaultType: 'textfield',
            autoHeight: true,
            bodyStyle: Ext.isIE ? 'padding:0 0 5px 15px;' : 'padding:10px 15px;',
            border: false,
            style: {
                "margin-left": "10px", // when you add custom margin in IE 6...
                "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
            },
            items: [
            	panelImpRid
            ,  {
                fieldLabel: 'Bilancio',
                name: 'Bilancio',
                id: 'Bilancio',
                readOnly: true
            },{
                fieldLabel: 'UPB',
                name: 'UPB',
                id: 'UPB',
                readOnly: true
            }, {
                fieldLabel: 'Missione. Programma',
                name: 'MissioneProgramma',
                id: 'MissioneProgramma',
                readOnly: true
            }, {
                fieldLabel: 'Capitolo',
                name: 'Capitolo',
                id: 'Capitolo',
                readOnly: true
            },{
                xtype: 'textarea',
                width: 180,
                 height:50,
                fieldLabel: 'Descrizione',
                name: 'DescrCapitolo',
                id: 'DescrCapitolo',
                readOnly: true
            },{
                xtype: 'textarea',
                width: 180,
                 height:50,
                fieldLabel: 'Oggetto Impegno',
                name: 'Oggetto_Impegno',
                id: 'Oggetto_Impegno',
                readOnly: true
            },{
                fieldLabel: 'Importo Impegno',
                name: 'ImpDisp',
                id: 'ImpDisp',
                readOnly: true
            },{
                fieldLabel: 'Importo Potenziale',
                name: 'ImpPotenzialePrenotato',
                id: 'ImpPotenzialePrenotato',
                readOnly: true,
                msgTarget  : 'qtip',       
                  listeners: {
                    render: function(c) {
                      Ext.QuickTips.register({
                        target: c.getEl(),
                        text: 'Importo al netto di eventuali operazioni contabili non ancora registrate (Liquidazioni/Riduzioni)'
                      });
                    }
                  }          
            },
            {
                fieldLabel: 'Tipo Atto',
                name: 'TipoAtto',
                id: 'TipoAtto',
                readOnly: true
            },
            {
                fieldLabel: 'Numero Atto',
                name: 'NumeroAtto',
                id: 'NumeroAtto',
                readOnly: true
            },
            {
                fieldLabel: 'Data Atto',
                name: 'DataAtto',
                id: 'DataAtto',
                readOnly: true
            },
            
            {
                xtype: 'numberfield',
                decimalSeparator: ',',
                allowNegative :false,
                fieldLabel: 'Importo da ridurre (*)',
                name: 'ImpDaRidurre',
                id: 'ImpDaRidurre'
            }, comboEconomia,{
                fieldLabel: 'Cod.Ob.Gest.',
                name: 'COGSemplice',
                id: 'COGSemplice',
                readOnly: true
            }
            
            ,
            buttonPanelRiduzione]
        }]
    });



   

    var capitoloPanelImpegno = new Ext.Panel({
        xtype: "panel",
        columnWidth: 1,
        bodyStyle: 'margin-bottom: 10px'
    });

    
    var currentYear = new Date().getYear();	
    //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
    var formCapitoliRiduzEcon = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        title: 'Elenco dei Capitoli',
        width: 800,
       
        layout: 'absolute',
        monitorValid: true,
        buttons: [{
            text: 'Salva',
            id: 'btnCreaRidEcon',
            handler: creaRidEco,
            formBind: true
        }, {
            text: 'Annulla',
            handler: function(btn, evt) {
                popupRiduzione.close();
            }
}],
            items: [
            capitoloPanelImpegno,
             {
                 xtype: 'panel',
                 title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear) + ":</br>impegni anno " + (currentYear) + " e precedenti</span>",
                 frame: true,
                 layout: 'form',
                 width: 250,
                 x: 0,
                 y: 210,
                 defaults: {
                     width: 132,
                     labelStyle: 'font-size:11px;'
                 },
                 buttons: [{
                     text: 'Pulisci',
                     id: 'button_pulisci1',
                     disabled: true,
                     handler: function() {
                         Ext.getCmp('impegno1').clearValue();
                         Ext.getCmp('disponilibitaImp1').reset();
                         Ext.getCmp('impPotenziale1').reset();
                         Ext.getCmp('cog_impegno1').reset();
                         Ext.getCmp('importo_riduzione1').disable();
                         Ext.getCmp('importo_riduzione1').reset();
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
                         xtype: 'hidden',
                         name: 'isEconomia1',
                         id: 'isEconomia1',
                         hiddenName: 'isEconomia1',
                         value: 0
                     },{
                         xtype: 'hidden',
                         name: 'tipo_atto1',
                         id: 'tipo_atto1',
                         hiddenName: 'tipo_atto1',
                         value: 0
                     }, {
                         xtype: 'hidden',
                         name: 'data_atto1',
                         id: 'data_atto1',
                         hiddenName: 'data_atto1',
                         value: 0
                     }, {
                         xtype: 'hidden',
                         name: 'numero_atto1',
                         id: 'numero_atto1',
                         hiddenName: 'numero_atto1',
                         value: 0
                     }, {
                         xtype: 'combo',
                         id: 'impegno1',
                         emptyText: 'Seleziona...',
                         name: 'impegno',
                         fieldLabel: 'Numero Impegno',
                         displayField: 'NumImpegno',
                         loadingText: 'Attendere...',
                         x: 0,
                         y: 0,
                         width: 132,
                         listWidth: 350,
                         editable: false,
                         mode: 'local',
                         triggerAction: 'all',
                         allowBlank: true,
                         store: storeImpegni1,
                         listeners: {
                             select: function(combo, record) {
                                 Ext.getCmp('disponilibitaImp1').setValue(eurRend(record.data.ImpDisp));
                                 Ext.getCmp('impPotenziale1').setValue(eurRend(record.data.ImpPotenzialePrenotato));
                                 Ext.getCmp('cog_impegno1').setValue(record.data.Codice_Obbiettivo_Gestionale);
                                 Ext.getCmp('importo_riduzione1').enable();
                                 
                                 Ext.getCmp('tipo_atto1').setValue(record.data.TipoAtto);
                                 Ext.getCmp('data_atto1').setValue(record.data.DataAtto);
                                 Ext.getCmp('numero_atto1').setValue(record.data.NumeroAtto);

                                 var annoImp = parseInt(record.data.NumImpegno.substring(0, 4))
                                 if (parseInt(record.data.NumImpegno.substring(0, 4)) < currentYear) {                                     
                                     Ext.getCmp('isEconomia1').setValue('1');
                                 }                                
                             }
                         },
                         tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">Impegno n. <b>{NumImpegno}</b><br/>{Oggetto_Impegno}<br/>da {TipoAtto} n. {NumeroAtto} del {DataAtto}</div></tpl>',
                         style: 'background-color: #fffb8a;background-image:none;',
                         disabled: true
                     }, {
                         fieldLabel: 'Disponibilità',
                         xtype: 'textfield',
                         name: 'disponilibitaImp1',
                         id: 'disponilibitaImp1',
                         style: 'opacity:.9;',
                         readOnly: true,
                         disabled: true
                     }, {
                         fieldLabel: 'Imp. potenziale',
                         xtype: 'textfield',
                         name: '',
                         id: 'impPotenziale1',
                         style: 'opacity:.9;',
                         readOnly: true,
                         disabled: true
                     }, {
                         fieldLabel: 'Ob. Gestionale',
                         xtype: 'textfield',
                         id: 'cog_impegno1',
                         name: 'cog_impegno1',
                         style: 'opacity:.9;',
                         readOnly: true,
                         disabled: true
                     }, {
                         fieldLabel: "Importo riduzione 1",
                         xtype: 'numberfield',
                         decimalSeparator: ',',
                         name: 'importo_riduzione1',
                         allowBlank: false,
                         //                        validateValue: validateBilancio1,
                         disabled: true,
                         decimalPrecision: 2,
                         id: 'importo_riduzione1',
                         style: 'background-color: #fffb8a;background-image:none;'
}]
                     }, {
                         xtype: 'panel',
                         title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 1) + ":</br>impegni anno " + (currentYear + 1) + "</span>",
                         frame: true,
                         layout: 'form',
                         width: 250,
                         x: 251,
                         y: 210,
                         defaults: {
                             width: 132,
                             labelStyle: 'font-size:11px;'
                         },
                         buttons: [{
                             text: 'Pulisci',
                             id: 'button_pulisci2',
                             disabled: true,
                             handler: function() {
                                 Ext.getCmp('impegno2').clearValue();
                                 Ext.getCmp('disponilibitaImp2').reset();
                                 Ext.getCmp('impPotenziale2').reset();
                                 Ext.getCmp('cog_impegno2').reset();
                                 Ext.getCmp('importo_riduzione2').reset();
                                 Ext.getCmp('importo_riduzione2').disable();
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
                                 xtype: 'hidden',
                                 name: 'isEconomia2',
                                 id: 'isEconomia2',
                                 hiddenName: 'isEconomia2',
                                 value: 0
                             }, {
                                 xtype: 'hidden',
                                 name: 'tipo_atto2',
                                 id: 'tipo_atto2',
                                 hiddenName: 'tipo_atto2',
                                 value: 0
                             }, {
                                 xtype: 'hidden',
                                 name: 'data_atto2',
                                 id: 'data_atto2',
                                 hiddenName: 'data_atto2',
                                 value: 0
                             }, {
                                 xtype: 'hidden',
                                 name: 'numero_atto2',
                                 id: 'numero_atto2',
                                 hiddenName: 'numero_atto2',
                                 value: 0
                             }, {
                                 xtype: 'combo',
                                 id: 'impegno2',
                                 emptyText: 'Seleziona...',
                                 name: 'impegno',
                                 fieldLabel: 'Numero Impegno',
                                 displayField: 'NumImpegno',
                                 loadingText: 'Attendere...',
                                 x: 0,
                                 y: 0,
                                 width: 132,
                                 listWidth: 350,
                                 editable: false,
                                 mode: 'local',
                                 triggerAction: 'all',
                                 allowBlank: true,
                                 store: storeImpegni2,
                                 listeners: {
                                     select: function(combo, record) {
                                         Ext.getCmp('disponilibitaImp2').setValue(eurRend(record.data.ImpDisp));
                                         Ext.getCmp('impPotenziale2').setValue(eurRend(record.data.ImpPotenzialePrenotato));
                                         Ext.getCmp('cog_impegno2').setValue(record.data.Codice_Obbiettivo_Gestionale);
                                         Ext.getCmp('tipo_atto2').setValue(record.data.TipoAtto);
                                         Ext.getCmp('data_atto2').setValue(record.data.DataAtto);
                                         Ext.getCmp('numero_atto2').setValue(record.data.NumeroAtto);
                                         Ext.getCmp('importo_riduzione2').enable();
                                     }
                                 },
                                 tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">Impegno n. <b>{NumImpegno}</b><br/>{Oggetto_Impegno}<br/>da {TipoAtto} n. {NumeroAtto} del {DataAtto}</div></tpl>',
                                 style: 'background-color: #fffb8a;background-image:none;',
                                 disabled: true
                             }, {
                                 fieldLabel: 'Disponibilità',
                                 xtype: 'textfield',
                                 name: 'disponilibitaImp2',
                                 id: 'disponilibitaImp2',
                                 style: 'opacity:.9;',
                                 readOnly: true,
                                 disabled: true
                             }, {
                                 fieldLabel: 'Imp. potenziale',
                                 xtype: 'textfield',
                                 name: '',
                                 id: 'impPotenziale2',
                                 style: 'opacity:.9;',
                                 readOnly: true,
                                 disabled: true
                             }, {
                                 fieldLabel: 'Ob. Gestionale',
                                 xtype: 'textfield',
                                 id: 'cog_impegno2',
                                 name: 'cog_impegno2',
                                 style: 'opacity:.9;',
                                 readOnly: true,
                                 disabled: true
                             }, {
                                 fieldLabel: 'Importo riduzione 2',
                                 xtype: 'numberfield',
                                 decimalSeparator: ',',
                                 name: 'importo_riduzione2',
                                 allowBlank: false,
                                 //                                validateValue: validateBilancio2,
                                 decimalPrecision: 2,
                                 disabled: true,
                                 id: 'importo_riduzione2',
                                 style: 'background-color: #fffb8a;background-image:none;'
}]
                             }, {
                                 xtype: 'panel',
                                 title: '<span style="font-size: 0.8em">Bilancio ' + (currentYear + 2) + ":</br>impegni anno " + (currentYear + 2) + "</span>",
                                 frame: true,
                                 layout: 'form',
                                 width: 250,
                                 x: 502,
                                 y: 210,
                                 defaults: {
                                     width: 132,
                                     labelStyle: 'font-size:11px;'
                                 },
                                 buttons: [{
                                     text: 'Pulisci',
                                     id: 'button_pulisci3',
                                     disabled: true,
                                     handler: function() {
                                         Ext.getCmp('impegno3').clearValue();
                                         Ext.getCmp('disponilibitaImp3').reset();
                                         Ext.getCmp('impPotenziale3').reset();
                                         Ext.getCmp('cog_impegno3').reset();
                                         Ext.getCmp('importo_riduzione3').reset();
                                         Ext.getCmp('importo_riduzione3').disable();
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
                                         xtype: 'hidden',
                                         name: 'isEconomia3',
                                         id: 'isEconomia3',
                                         hiddenName: 'isEconomia3',
                                         value: 0
                                     }, {
                                         xtype: 'hidden',
                                         name: 'tipo_atto3',
                                         id: 'tipo_atto3',
                                         hiddenName: 'tipo_atto3',
                                         value: 0
                                     }, {
                                         xtype: 'hidden',
                                         name: 'data_atto3',
                                         id: 'data_atto3',
                                         hiddenName: 'data_atto3',
                                         value: 0
                                     }, {
                                         xtype: 'hidden',
                                         name: 'numero_atto3',
                                         id: 'numero_atto3',
                                         hiddenName: 'numero_atto3',
                                         value: 0
                                     }, {
                                         xtype: 'combo',
                                         id: 'impegno3',
                                         emptyText: 'Seleziona...',
                                         name: 'impegno',
                                         fieldLabel: 'Numero Impegno',
                                         displayField: 'NumImpegno',
                                         loadingText: 'Attendere...',
                                         x: 0,
                                         y: 0,
                                         width: 132,
                                         listWidth: 350,
                                         editable: false,
                                         mode: 'local',
                                         triggerAction: 'all',
                                         allowBlank: true,
                                         store: storeImpegni3,
                                         listeners: {
                                             select: function(combo, record) {
                                                 Ext.getCmp('disponilibitaImp3').setValue(eurRend(record.data.ImpDisp));
                                                 Ext.getCmp('impPotenziale3').setValue(eurRend(record.data.ImpPotenzialePrenotato));
                                                 Ext.getCmp('cog_impegno3').setValue(record.data.Codice_Obbiettivo_Gestionale);
                                                 Ext.getCmp('tipo_atto3').setValue(record.data.TipoAtto);
                                                 Ext.getCmp('data_atto3').setValue(record.data.DataAtto);
                                                 Ext.getCmp('numero_atto3').setValue(record.data.NumeroAtto);
                                                 
                                                 Ext.getCmp('importo_riduzione3').enable();
                                                 
                                                 
                                             }
                                         },
                                         tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">Impegno n. <b>{NumImpegno}</b><br/>{Oggetto_Impegno}<br/>da {TipoAtto} n. {NumeroAtto} del {DataAtto}</div></tpl>',
                                         style: 'background-color: #fffb8a;background-image:none;',
                                         disabled: true
                                     }, {
                                         fieldLabel: 'Disponibilità',
                                         xtype: 'textfield',
                                         name: 'disponilibitaImp3',
                                         id: 'disponilibitaImp3',
                                         style: 'opacity:.9;',
                                         readOnly: true,
                                         disabled: true
                                     }, {
                                         fieldLabel: 'Imp. potenziale',
                                         xtype: 'textfield',
                                         name: '',
                                         id: 'impPotenziale3',
                                         style: 'opacity:.9;',
                                         readOnly: true,
                                         disabled: true
                                     }, {
                                         fieldLabel: 'Ob. Gestionale',
                                         xtype: 'textfield',
                                         id: 'cog_impegno3',
                                         name: 'cog_impegno3',
                                         style: 'opacity:.9;',
                                         readOnly: true,
                                         disabled: true
                                     }, {
                                         fieldLabel: 'Importo riduzione 3',
                                         xtype: 'numberfield',
                                         decimalSeparator: ',',
                                         //                                        validateValue: validateBilancio3,
                                         decimalPrecision: 2,
                                         allowBlank: false,
                                         name: 'importo_riduzione3',
                                         disabled: true,
                                         id: 'importo_riduzione3',
                                         style: 'background-color: #fffb8a;background-image:none;'
}]
}]
        });

    if (mostraAnno == false) {
        tbarDateRiduzione.hide();
    } else { tbarDateRiduzione.show(); }
    
 var popupRiduzione = new Ext.Window({ y:15,
					            title: 'Riduzione e\/o Economia',
					            width: 782,
					            height: 590,
					            id: 'popup_riduzione',
					            tbar: ['->', actionHelpOnLineRiduz],
					            layout: 'fit',
					            plain: true,
					            bodyStyle: 'padding-top:10px',
					            buttonAlign: 'center',
					            maximizable: true,
					            enableDragDrop   : true,
					            collapsible: false,
					            modal:true,
					            closable: true  /* * toglie la croce per chiudere la finestra */
					        });

					        popupRiduzione.add(formCapitoliRiduzEcon);
//					            popupRiduzione.add(formCapitoliRiduzione);
					            popupRiduzione.doLayout(); //forzo ridisegno
					         
	//GESTISCO L'AZIONE DEL BOTTONE "Aggiungi Riduzione"
					            Ext.getCmp('btnAggiungi').on('click', function() {
					                if (Ext.getCmp('comboEconomia').getValue() == '') {
					                    Ext.MessageBox.show({
					                        title: 'Gestione Riduzioni',
					                        msg: 'Selezionare il tipo operazione !',
					                        buttons: Ext.MessageBox.OK,
					                        icon: Ext.MessageBox.ERROR
					                    });
					                    return
					                }
					                if ((Ext.get('ImpDaRidurre').dom.value == 0) || (Ext.get('ImpDaRidurre').dom.value == '')) {
					                    Ext.MessageBox.show({
					                        title: 'Gestione  Riduzioni',
					                        msg: 'Il campo Importo da ridurre non &egrave; stato valorizzato!',
					                        buttons: Ext.MessageBox.OK,
					                        icon: Ext.MessageBox.ERROR,
					                        fn: function(btn) { return }
					                    });
					                } else {
					                    if (Ext.get('ImpDaRidurre').dom.value == 0) {
					                        Ext.MessageBox.show({
					                            title: 'Gestione Riduzioni',
					                            msg: 'Il campo Importo da ridurre non può essere maggiore del residuo!',
					                            buttons: Ext.MessageBox.OK,
					                            icon: Ext.MessageBox.ERROR,
					                            fn: function(btn) { return }
					                        });

					                    } else {

					                        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
					                        formCapitoliRiduzione.getForm().timeout = 100000000;
					                        formCapitoliRiduzione.getForm().submit({
					                            url: 'ProcAmm.svc/GenerazioneRiduzioneUP' + window.location.search,
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
								        title: 'Gestione Riduzione Impegno',
								        msg: lstr_messaggio,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.ERROR,
								        fn: function(btn) {
								            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
								            return;
								        }
								    });

								}, // FINE FAILURE
					                            success:
								function(result, response) {
								    var msg = 'Riduzione Contabile effettuato con successo!';
								    Ext.MessageBox.show({
								        title: 'Gestione Riduzione Impegno',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {
								            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
								            popupRiduzione.close();
								        }
								    }
								    );
								} // FINE SUCCESS
					                        }) // FINE SUBMIT
					                    } //fine else  			 
					                } //fine else
					            });   //FINE ON CLICK

//PRENDO L'ANNO DALLA DATA ODIERNA
 		var dtOggi=new Date()
		dtOggi.setDate(dtOggi.getDate());
		var fAnno = dtOggi.getFullYear();
		
		//RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
	 	var gridCapitoli = buildGridStartRiduzione(fAnno);
//	 	capitoloPanelRiduzione.add(gridCapitoli);	 	
	 	capitoloPanelImpegno.add(gridCapitoli);
	    // SETTO panelImp INVISIBILE 
		
		
        panelImpRid.hide();
       
popupRiduzione.show();
//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
//    var formPanelDropTargetEl = formCapitoliRiduzEcon.body.dom;
//	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
//	      ddGroup: 'gridDDGroup',
//	      notifyEnter: function(ddSource, e, data) {
//	      //EFFETTI VISIVI SUL DRAG & DROP
//	      formCapitoliRiduzione.body.stopFx();
//	      formCapitoliRiduzione.body.highlight();
//	      },
//	      notifyDrop: function(ddSource, e, data) {
//	          // CATTURO IL RECORD SELEZIONATO
//	          var selectedRecord = ddSource.dragData.selections[0];
//	          // CARICO IL RECORD NELLA FORM
//	          formCapitoliRiduzione.getForm().loadRecord(selectedRecord);
//	          // RENDO VISIBILE panelImp
//	          
//	          //LU aggiunta formattazione imp disp 

//	          Ext.get('ImpDisp').dom.value = eurRend(0)

//	          //LU Fine  aggiunta formattazione imp disp 
//	          buildComboImpRid(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
//	          panelImpRid.doLayout();
//		      panelImpRid.show();
//	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
//	          // CANCELLO IL RECORD DALLA GRIGLIA.
//	          //ddSource.grid.store.remove(selectedRecord);
//	          return (true);
//	      }
//	  }); 


 
}// fine InitformCapitoliRiduzione

function VerificaDisponibilitaRiduzione() {
    try {
         Ext.get('COGSemplice').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.Codice_Obbiettivo_Gestionale
        Ext.get('DataAtto').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.DataAtto;
        Ext.get('TipoAtto').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.TipoAtto;
        Ext.get('NumeroAtto').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.NumeroAtto;
        Ext.get('ImpDisp').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.ImpDisp;
        //LU aggiunta formattazione imp disp 
        Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
        //LU Fine  aggiunta formattazione imp disp 
        Ext.get('Oggetto_Impegno').dom.value = ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(ComboImpRid.store.data.get(ComboImpRid.selectedIndex).data.ImpPotenzialePrenotato);

        Ext.get('ImpDaRidurre').dom.value = 0;
      
      
        Ext.getCmp('btnAggiungi').show();
    } catch (ex) { 
    }
}

function buildComboImpRid(AnnoRif, CapitoloRif) {
//    panelImpRid.remove(ComboImpRid);
  
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImp',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

      root: 'GetListaImpResult',
         fields: [
           { name: 'NumImpegno' }   ,
           { name: 'ImpDisp' }   ,
           { name: 'TipoAtto' }   ,
           { name: 'DataAtto' }   ,
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenziale' },
           { name: 'Codice_Obbiettivo_Gestionale' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("NumImpegno", "ASC");

  var ufficio =Ext.get('Cod_uff_Prop').dom.value;
  
 
    store.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
            },
            scope: this
        }
    });

    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif ,CodiceUfficio: ufficio};
    mask.show();
    store.load({ params: parametri });


  ComboImpRid=  new Ext.form.ComboBox({
        fieldLabel: 'NumeroImpegno',
        name: 'ComboImpRid',
        id: 'ComboImpRid',
        store: store,
        listWidth: 150,
        readOnly:true,
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Impegni ...',
        displayField: 'NumImpegno',
        valueField:'NumImpegno',
         mode:'local'

    });
    
    panelImpRid.add(ComboImpRid);
    
    ComboImpRid.show();

    ComboImpRid.on('select', function(record, index) { VerificaDisponibilitaRiduzione() });
    return ComboImpRid;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER RIDUZIONE O ECONOMIA"
function buildGridStartRiduzione(annoRif) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetElencoCapitoliAnnoLiquidazione',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetElencoCapitoliAnnoLiquidazioneResult',
        fields: [
           { name: 'Bilancio' },
           { name: 'UPB' },
           { name: 'MissioneProgramma' },
           { name: 'Capitolo' },
           { name: 'ImpDisp' },
           { name: 'ImpPrenotato' },
           { name: 'NumPrenotazione' },
           { name: 'AnnoPrenotazione' },
           { name: 'DescrCapitolo' },
           { name: 'CodiceRisposta' },
           { name: 'DescrizioneRisposta' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
		, reader: reader
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

    var parametri = { AnnoRif: annoRif, tipoCapitolo: '0' };
    store.load({ params: parametri });

    store.on({
        'load': {
            fn: function(store, records, options) {
                maskImp.hide();
            },
            scope: this
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
	                {header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true, locked: false },
                    { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                    { header: "Missione.Programma", width: 60, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width: 300, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	            	]);

    var grid = new Ext.grid.GridPanel({
        id: 'GridCapitoli',
        autoExpandColumn: 'Capitolo',
        width: 754,
        height: 200,
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
                        resetFormRidEcon();
//                        Ext.getCmp('disponibilita1').setValue(eurRend(rows[0].data.ImpDisp));
//                        Ext.getCmp('disponibilita2').setValue('Attendere...');
//                        Ext.getCmp('disponibilita3').setValue('Attendere...');

                       
                        var codiceUfficio = Ext.get('Cod_uff_Prop').dom.value; 
                        var params = {
                            AnnoRif: rows[0].data.Bilancio,
                            CapitoloRif: rows[0].data.Capitolo,
                            CodiceUfficio: codiceUfficio  
                        };
                        storeImpegni1.reload({ params: params });
                        
                        var params2 = {
                            AnnoRif: Ext.getCmp('bilancio2').getValue(),
                            CapitoloRif: rows[0].data.Capitolo,
                            CodiceUfficio: codiceUfficio 
                        };
                        storeImpegni2.reload({ params: params2 });
                        
                        var params3 = {
                            AnnoRif: Ext.getCmp('bilancio3').getValue(),
                            CapitoloRif: rows[0].data.Capitolo,
                            CodiceUfficio: codiceUfficio  
                        };
                        storeImpegni3.reload({ params: params3 });                  

                    }
                }
            }
        })
    });

	return grid;
}




function fnEconomia(value){
    if (value=="")
        return ""
    if (value == "0")
        return "Riduzione";
    if (value == "1")
        return "Economia";
}

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraRidContestuale = new Ext.Action({
         text:'Registra',
         tooltip:'Registra la riduzione selezionata',
        handler: function(){
            registraRiduzioneContestuale();
     },
        iconCls: 'add'
    });

function enableConfirmAction(selectedRows) {
    var retValue = false
    
    for (var i = 0; !retValue && i < selectedRows.length; i++) 
        if (retValue = (selectedRows[i].data.Stato == 2))
            break;
            
    return retValue;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridRiduzioni() {
    
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetRiduzioniRegistrate'+ window.location.search,
	
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetRiduzioniRegistrateResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpDisp'},
           {name: 'ImpPrenotato'},
           {name: 'NumImpegno'},
           {name: 'AnnoPrenotazione'},
           {name: 'ID'},
           {name: 'TipoAtto'},
           {name: 'NumeroAtto'},
           {name: 'DataAtto' },
           {name: 'IsEconomia' },
           {name: 'Tipo'},
           {name: 'Stato'},
           { name: 'StatoAsString' },
           { name: 'HashTokenCallSic' },
           { name: 'IdDocContabileSic' }
           ]
	});


	var storeEconomia = new Ext.data.SimpleStore({
	    fields: ['value', 'description'],
	    data: [['0', 'Riduzione'], ['1', 'Economia']]
	});
	
var store = new Ext.data.GroupingStore({
        proxy:proxy
		,reader:reader,
		groupField: 'Tipo',
		sortInfo: {
           field: 'Tipo',
            direction: "ASC"
        }
    })

      store.load();
    
    store.on({
   'load':{
      fn: function(store, records, options){
       mask.hide();
     },
      scope:this
  	 }
 	});

 	var summary = new Ext.grid.GroupSummary();

 	var sm = new Ext.grid.CheckboxSelectionModel(
 	     { singleSelect: false,
 	         listeners: {
 	             rowselect: function(sm, row, rec) {
 	                 var multiSelect = sm.getSelections().length > 1;

 	                 if (rec.data.Stato == 2) {
 	                     actionDeleteRiduzione.setDisabled(multiSelect);
 	                     actionConfermaRiduzione.setDisabled(false);
 	                     actionRegistraRidContestuale.setDisabled(true);
 	                 } else {
 	                     actionDeleteRiduzione.setDisabled(multiSelect);
 	                     actionRegistraRidContestuale.setDisabled(multiSelect);
 	                 }
 	             },
 	             rowdeselect: function(sm, row, rec) {
 	                 var selectedRowsCount = sm.getSelections().length;

 	                 if (selectedRowsCount == 1) {
 	                     if (sm.getSelected().data.Stato == 2) {
 	                         actionDeleteRiduzione.setDisabled(false);
 	                         actionRegistraRidContestuale.setDisabled(true);
 	                     } else {
 	                         actionDeleteRiduzione.setDisabled(false);
 	                         actionRegistraRidContestuale.setDisabled(false);
 	                     }
 	                 } else {
 	                    actionDeleteRiduzione.setDisabled(true);
 	                    actionRegistraRidContestuale.setDisabled(true);
 	                 }

 	                 actionConfermaRiduzione.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));
 	             }
 	         }
 	     }
 	);
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                 { header: "Bilancio", dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true,
		                  summaryRenderer: function(v, params, data){return '<b> Totale </b>';} },
		                { header: "Capitolo", dataIndex: 'Capitolo',  sortable: true, locked:false},
		                { header: "UPB", dataIndex: 'UPB', sortable: true },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		                { renderer: eurRend, header: "Importo Ridotto", align: 'right', dataIndex: 'ImpPrenotato', sortable: true ,summaryType:'sum' ,
		            	    editor: new Ext.form.NumberField({
		            	           decimalSeparator: ',',
					               allowBlank: true,
					               allowNegative: false
					           })
		            	},
		               	{ header: "Numero Impegno", dataIndex: 'NumImpegno', sortable: true },
		            	{ header: "Tipo Atto", dataIndex: 'TipoAtto', sortable: true, hidden: true },
		            	{ header: "Num. Atto", dataIndex: 'NumeroAtto', sortable: true, hidden: true },
		            	{ header: "Data Atto", dataIndex: 'DataAtto', sortable: true, hidden: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	 { renderer: fnEconomia,header: "Tipologia", dataIndex: 'IsEconomia', sortable: true,
		            	     editor: new Ext.form.ComboBox({
		            	            id:"IsEconomia",
		            	           store:[ ['0','Riduzione']
                                    ,['1','Economia']],
		            	          typeAhead: false,
		            	         mode: 'local',
		            	         triggerAction: 'all',
		            	         selectOnFocus: true, editable: false
                                , lazyRender: true
                                , forceSelection: true
                                		            	         
		            	     })
		            	 },
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true}  ,
		            	{ header: "Stato", dataIndex: 'StatoAsString'} 
		    ]);

		            	/* ColumnModel.setEditable(2, false);
		            	 ColumnModel.setEditable(3, false);
		            	 ColumnModel.setEditable(4, false);
		            	 ColumnModel.setEditable(5, false);*/
		            	 
		    var GridRiduzione = new Ext.grid.EditorGridPanel({ 
				    	id: 'GridRiepilogoRiduzione',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',
				        loadMask: true,
				        
                        plugins: summary,

				        view: new Ext.grid.GroupingView({
				            forceFit:true,
                            showGroupName: false,
                            enableNoGroups:true, // REQUIRED!
                            hideGroupedColumn: true,
                            enableGroupingMenu: true
                        })

	          });
	          actionDeleteRiduzione.setDisabled(true);
	          actionConfermaRiduzione.setDisabled(true);
	         
 return GridRiduzione; 
}
function DisabilitaColonneRiduzione() {


          	Ext.getCmp('GridRiepilogoRiduzione').colModel.setEditable(4, false);
          	Ext.getCmp('GridRiepilogoRiduzione').colModel.setEditable(10, false);
          	/*
          	Ext.getCmp('GridRiepilogoRiduzione').colModel.setEditable(4, false);
          	Ext.getCmp('GridRiepilogoRiduzione').colModel.setEditable(5, false);*/
		            	 
		    
  
}
//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LA RIDUZIONE
function EliminaRiduzione(NumImpegno,ID) {
	var params = { NumImpegno: NumImpegno, ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/EliminaRiduzione',
       params: Ext.encode(params),
       method: 'POST',
       success: function(response, options) {
           mask.hide();
           var data = Ext.decode(response.responseText);
           // var numeroPreImpegnoResult = data.EliminaPreImpegnoResult;
       },
       failure: function(response, options) {
            mask.hide();
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
    
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanelRiduzioniContestuali = new Ext.FormPanel({
    id:'myPanelRiduzioniContestuali',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni contestuali alle liquidazioni del presente provvedimento",
	tbar: [actionRegistraRidContestuale, actionDeleteRiduzione, actionConfermaRiduzione]
	,
	items : [
	  	 {
            id: "RiduzioniContestuali",
            xtype: "hidden"
	     }]
	 });

function resetFormRidEcon() {
    Ext.getCmp('impegno1').clearValue();
    Ext.getCmp('disponilibitaImp1').reset();
    Ext.getCmp('impPotenziale1').reset();
    Ext.getCmp('cog_impegno1').reset();
    Ext.getCmp('importo_riduzione1').reset();

    Ext.getCmp('impegno2').clearValue();
    Ext.getCmp('disponilibitaImp2').reset();
    Ext.getCmp('impPotenziale2').reset();
    Ext.getCmp('cog_impegno2').reset();
    Ext.getCmp('importo_riduzione2').reset();

    Ext.getCmp('impegno3').clearValue();
    Ext.getCmp('disponilibitaImp3').reset();
    Ext.getCmp('impPotenziale3').reset();
    Ext.getCmp('cog_impegno3').reset();
    Ext.getCmp('importo_riduzione3').reset();
//    abilitaPreimpegno(false);
    abilitaFormBilanci(true);
}

//funzione per registrare le riduzioni contestuali alle liquidazioni
function registraRiduzioneContestuale() {
    var storeGridRidContestuali = Ext.getCmp('GridRiepilogoRiduzione').getStore();
    var json = '';
    storeGridRidContestuali.each(function(storeGridRidContestuali) {
        json += Ext.util.JSON.encode(storeGridRidContestuali.data) + ',';
    });
    json = json.substring(0, json.length - 1);
    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
    Ext.getDom('RiduzioniContestuali').value = json;
    myPanelRiduzioniContestuali.getForm().timeout = 100000000;
    myPanelRiduzioniContestuali.getForm().submit({
        url: 'ProcAmm.svc/SetRiduzioni' + window.location.search,
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
								        title: 'Gestione Riduzioni',
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
								    var msg = 'Aggiornamento Riduzione effettuato con successo!';
								    Ext.MessageBox.show({
								        title: 'Gestione Riduzioni',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {
								            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
								            return;
								        }
								    });
								} // FINE SUCCESS

    });// FINE SUBMIT
};      //FINE function

//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaRiduzione(RiduzioniInfo) {
    var params = { RiduzioniInfo: RiduzioniInfo };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();
    
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaRiduzione',
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            mask.hide();
            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
            actionConfermaRiduzione.setDisabled(true);
            actionDeleteRiduzione.setDisabled(true);
            actionRegistraRidContestuale.setDisabled(true);             
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
                        Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
                        actionConfermaRiduzione.setDisabled(true);
                        actionDeleteRiduzione.setDisabled(true);
                        actionRegistraRidContestuale.setDisabled(true);
                    }
                }
            });            
        }
    });
}

function ConfermaRiduzione(ID) {
	var params = {ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaRiduzione',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           mask.hide();
           Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
       },
      failure: function(response,result, options) {
            mask.hide();
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
               title: 'Errore',
               msg: data.FaultMessage,
               buttons: Ext.MessageBox.OK,
               icon: Ext.MessageBox.ERROR,
               fn: function(btn) { return }
           });
       }
   });}function NascondiColonneRid() {    var index = Ext.getCmp('GridRiepilogoRiduzione').colModel.getColumnCount(false);    index = index - 1;    Ext.getCmp('GridRiepilogoRiduzione').colModel.setHidden(index, true);    actionConfermaRiduzione.hide();}