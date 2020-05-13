var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaRiduzioneLiq = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function() {
        var riduzioniLiqDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridRiduzioniLiq').getSelectionModel().getSelections(), function(rec) {
            //conferma solo le riduzioni liquidazioni da confermare, quelle aventi stato 2
            if (rec.data['Stato'] == 2) {
                var riduzioneLiqInfo = new Object();

                riduzioneLiqInfo.ID = rec.data['ID'];
                riduzioneLiqInfo.Stato = rec.data['Stato'];
                riduzioneLiqInfo.NLiquidazione = rec.data['NLiquidazione'];

                riduzioniLiqDaComfermare.push(riduzioneLiqInfo);
            }
        });

        ConfermaMultiplaRiduzioneLiq(riduzioniLiqDaComfermare);
    },
    iconCls: 'save'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
  var actionAddRiduzioneLiq = new Ext.Action({
         text:'Aggiungi',
         tooltip:'Aggiungi una nuova riga',
         handler: function() {
         var mostraAnno = false;
         InitFormCapitoliRiduzioneLiq(mostraAnno);
        },
        iconCls: 'add'
    });
  
  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA  
    var actionDeleteRiduzioneLiq = new Ext.Action({
         text:'Cancella',
         tooltip:'Cancella selezionato',
        handler: function(){
            var storeGridRidLiq=Ext.getCmp('GridRiduzioniLiq').getStore();
			   Ext.each(Ext.getCmp('GridRiduzioniLiq').getSelectionModel().getSelections(), function(rec)
						{
						    EliminaRiduzioneLiq(rec.data['ID']);
						    storeGridRidLiq.remove(rec);
						})            
        },
        iconCls: 'remove'
    });

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelRiduzioniLiq = new Ext.FormPanel({
    id:'myPanelRiduzioniLiq',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni Liquidazioni registrate",
	tbar:[actionAddRiduzioneLiq,actionDeleteRiduzioneLiq, actionConfermaRiduzioneLiq]
});

var labelNumLiqRid = new Ext.form.Label({
						  text: 'Numero Liquidazione: ',
						  id: 'labelNumLiqRid' 
						 });    


  
var ComboLiqRid = new Ext.form.ComboBox({
fieldLabel: 'NLiquidazione',
displayField: 'NLiquidazione',
valueField: 'NLiquidazione',
id: 'ComboLiqRid',
       name: 'ComboLiqRid',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Liquidazione ...',
        mode:'local'

});
  
    
//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DELLA LIQUIDAZIONE
var panelLiqRid = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumLiqRid,
									ComboLiqRid
								 ]
								}); 
								
//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliRiduzioneLiq(mostraAnno) {
  var AnnoBilancioRiduzioneLiq= new Ext.ux.YearMenu({
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
   			        var btnRicercaRiduzioneLiq = new Ext.Button({
   			            text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
   			            id: 'btnBilancio'
   			        });

   			        btnRicercaRiduzioneLiq.on('click', function() {
   			      
   			            var fNewAnno = Ext.getCmp("AnnoIni").value;
   			            //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
   			            gridCapitoli = buildGridStartRiduzioneLiq(fNewAnno);
   			            capitoloPanelRiduzioneLiq.remove('GridCapitoli')
   			            capitoloPanelRiduzioneLiq.add(gridCapitoli);
   			            capitoloPanelRiduzioneLiq.doLayout();
   			        });

 var tbarDateRiduzioneLiq = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancioRiduzioneLiq},
        			 {
			        xtype: 'textfield',
			       	cls: 'titfis',
			        readOnly: true,
			        id: 'AnnoIni',
			        width: 80,
 			        value: new Date().format('Y')
			    },
			    btnRicercaRiduzioneLiq
			 ]
	});
	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelRiduzioneLiq = new Ext.Panel({
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 	
						
	labelNumLiqRid = new Ext.form.Label({
		text: 'Numero Liquidazione: ',
		id: 'labelNumLiqRid' 
	});    

    ComboLiqRid = new Ext.form.ComboBox({
        fieldLabel: 'NLiquidazione',
        displayField: 'NLiquidazione',
        valueField: 'NLiquidazione',
        id: 'ComboLiqRid',
        name: 'ComboLiqRid',
        mode: 'local',
        listWidth: 150,
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Liquidazione ...'
    });
     
 
    panelLiqRid = new Ext.Panel({
				xtype : "panel",
				title : "",
				width:300,
				buttonAlign: "center",
				autoHeight:true,
				layout: "fit",
				items: [
					labelNumLiqRid,
					ComboLiqRid
				 ]
	}); 

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanelRiduzioneLiq = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{
								    layout : "fit"
								}],
								buttons: [{
									text: 'Aggiungi Riduzione',
									id: 'btnRiduciLiq'
								}]
					}); 
 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
 var formCapitoliRiduzioneLiq = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per riduzione liquidazione',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 800,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDateRiduzioneLiq
        		],
            items: [capitoloPanelRiduzioneLiq]
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
            	panelLiqRid
            ,
            	{
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
                fieldLabel: 'Oggetto Atto',
                name: 'Oggetto_Impegno',
                id: 'Oggetto_Impegno',
                readOnly: true
            },{
                fieldLabel: 'Disponibilit&agrave;',
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
            },{
                xtype: 'numberfield',
                decimalSeparator: ',',
                allowNegative :false,
                fieldLabel: 'Importo da ridurre (*)',
                name: 'ImpDaRidurreLiq',
                id: 'ImpDaRidurreLiq'
            },
            buttonPanelRiduzioneLiq]
        }]        
    });

    if (mostraAnno == false) {
        tbarDateRiduzioneLiq.hide();
    } else { tbarDateRiduzioneLiq.show(); }

 var popupRiduzioneLiq = new Ext.Window({ y:15,
					            title: 'Aggiungi una riduzione di una liquidazione',
					            width: 830,
					           height: 600,
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
					            popupRiduzioneLiq.add(formCapitoliRiduzioneLiq);
					            popupRiduzioneLiq.doLayout(); //forzo ridisegno
					         
	//GESTISCO L'AZIONE DEL BOTTONE "Aggiungi Riduzione"
					            Ext.getCmp('btnRiduciLiq').on('click', function() {
					                if ((Ext.get('ImpDaRidurreLiq').dom.value == 0) || (Ext.get('ImpDaRidurreLiq').dom.value == '')) {
					                    Ext.MessageBox.show({
					                        title: 'Gestione Riduzione Liquidazione',
					                        msg: 'Il campo Importo da ridurre non &egrave; stato valorizzato!',
					                        buttons: Ext.MessageBox.OK,
					                        icon: Ext.MessageBox.ERROR,
					                        fn: function(btn) { return }
					                    });
					                } else {
					                    if (Ext.get('ImpDaRidurreLiq').dom.value == 0) {
					                        Ext.MessageBox.show({
					                            title: 'Gestione Riduzione Liquidazione',
					                            msg: 'Il campo Importo da ridurre non può essere maggiore della disponibilità!',
					                            buttons: Ext.MessageBox.OK,
					                            icon: Ext.MessageBox.ERROR,
					                            fn: function(btn) { return }
					                        });

					                    } else {

					                        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
					                        formCapitoliRiduzioneLiq.getForm().timeout = 100000000;
					                        formCapitoliRiduzioneLiq.getForm().submit({
					                            url: 'ProcAmm.svc/GenerazioneRiduzioneLiqUP',
					                            waitTitle: "Attendere...",
					                            waitMsg: 'Aggiornamento in corso ......',
					                            failure:
								function(result, response) {
								    var lstr_messaggio = ''
								    try {
								        lstr_messaggio = response.result.FaultMessage;
								    } catch (ex) {
								        lstr_messaggio = 'Errore Generale';
								    }

								    Ext.MessageBox.show({
								        title: 'Gestione Riduzione Liquidazione',
								        msg: lstr_messaggio,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.ERROR,
								        fn: function(btn) {
								            Ext.getCmp('GridRiduzioniLiq').getStore().reload();
								            return;
								        }
								    });

								}, // FINE FAILURE
					                            success:
								function(result, response) {
								    var msg = 'Riduzione Liquidazione effettuata con successo!';
								    Ext.MessageBox.show({
								        title: 'Gestione Riduzione Liquidazione',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {
								            Ext.getCmp('GridRiduzioniLiq').getStore().reload();
								            popupRiduzioneLiq.close();
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
	 	var gridCapitoli = buildGridStartRiduzioneLiq(fAnno);
	    capitoloPanelRiduzioneLiq.add(gridCapitoli);
		panelLiqRid.hide();
       
popupRiduzioneLiq.show();

//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
	 var formPanelDropTargetEl = formCapitoliRiduzioneLiq.body.dom;
	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
	      ddGroup: 'gridDDGroup',
	      notifyEnter: function(ddSource, e, data) {
	      //EFFETTI VISIVI SUL DRAG & DROP
	      formCapitoliRiduzioneLiq.body.stopFx();
	      formCapitoliRiduzioneLiq.body.highlight();
	      },
	      notifyDrop: function(ddSource, e, data) {
	          // CATTURO IL RECORD SELEZIONATO
	          var selectedRecord = ddSource.dragData.selections[0];
	          // CARICO IL RECORD NELLA FORM
	          formCapitoliRiduzioneLiq.getForm().loadRecord(selectedRecord);
	          // RENDO VISIBILE panelImp
	           buildComboLiqRid(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
	          //LU aggiunta formattazione imp disp 
	 
	          Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)

	          //LU Fine  aggiunta formattazione imp disp 
	          buildComboLiqRid(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
	          panelLiqRid.doLayout();
		      panelLiqRid.show();
	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
	          // CANCELLO IL RECORD DALLA GRIGLIA.
	          //ddSource.grid.store.remove(selectedRecord);
	          return (true);
	      }
	  }); 


 
}// fine InitFormCapitoli


function VerificaDisponibilitaRiduzioneLiq() {
    try {
        Ext.get('ImpDisp').dom.value = ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.ImpDisp;
        //LU aggiunta formattazione imp disp 
        Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
        //LU Fine  aggiunta formattazione imp disp 
        Ext.get('Oggetto_Impegno').dom.value = ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.ImpPotenzialePrenotato);

        Ext.get('ImpDaRidurreLiq').dom.value = 0;
        Ext.getCmp('btnRiduciLiq').show();
    } catch (ex) { 
    }
}

function buildComboLiqRid(AnnoRif, CapitoloRif) {
    panelLiqRid.remove(ComboLiqRid);
  
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaLiquidazioniAperte',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

      root: 'GetListaLiquidazioniAperteResult',
         fields: [
           { name: 'NLiquidazione' }   ,
           { name: 'ImpDisp' }  ,
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("NLiquidazione", "ASC");

    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
    store.load({ params: parametri });

    store.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
            },
            scope: this
        }
    });


  ComboLiqRid=  new Ext.form.ComboBox({
        fieldLabel: 'NLiquidazione',
        displayField: 'NLiquidazione',
        name: 'ComboLiqRid',
        id: 'ComboLiqRid',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Liquidazioni ...'

    });
    
    panelLiqRid.add(ComboLiqRid);
    
    ComboLiqRid.show();

    ComboLiqRid.on('select', function(record, index) { VerificaDisponibilitaRiduzioneLiq() });
    return ComboLiqRid;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartRiduzioneLiq(annoRif) {
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetElencoCapitoliAnnoLiquidazione',
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetElencoCapitoliAnnoLiquidazioneResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
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

	var parametri = { AnnoRif: annoRif, tipoCapitolo: '2' };
    store.load({params:parametri});

    store.on({
   'load':{
      fn: function(store, records, options){
       mask.hide();
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
	                { header: "Bilancio", width: 60,dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
	                { header: "Capitolo", width: 60,dataIndex: 'Capitolo',  sortable: true, locked:false},
                    { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                    { header: "Missione.Programma", width: 60, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width: 180, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	                { header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	            	] );
 	  var grid = new Ext.grid.GridPanel({
	  			  id:'GridCapitoliRid',
	              autoExpandColumn: 'Capitolo',
		          // da mettere sempre 
		          height: 350,
	              title:'',
	              border: true,
			      viewConfig : { forceFit : true },	  			  
		          ds: store,
		          cm: ColumnModel,
		          stripeRows: true,
		          loadMask: true,
               // istruzioni per abilitazione Drag & Drop		          
		          enableDragDrop: true,
		          ddGroup: 'gridDDGroup',
		       // fine istruzioni per abilitazione Drag & Drop		          
	          	  sm: new Ext.grid.RowSelectionModel({
	              singleSelect: true
	           		 })
		          });
	    
//AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
		          grid.addListener({
		              'rowdblclick': {
		                  fn: function(grid, rowIndex, event) {
		                      var rec = grid.store.getAt(rowIndex);
		                      if (rec.data['ImpDisp'] != "0") {
		                          Ext.getCmp("ElencoCapitoli").getForm().loadRecord(rec);

		                          Ext.get('ImpDaRidurreLiq').dom.value = '';
		                       
		                          //LU aggiunta formattazione imp disp
		                          Ext.get('ImpDisp').dom.value = eurRend(0)
		                          //LU Fine  aggiunta formattazione imp disp 
		                       
		                          buildComboLiqRid(rec.data['Bilancio'], rec.data['Capitolo']);
		                          panelLiqRid.doLayout();
		                          panelLiqRid.show();
		                      }
		                  }
		, scope: this
		              }
		          });
	return grid;
}


function enableConfirmAction(selectedRows) {
    var retValue = false;

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.Stato == 2))
        break;

    return retValue;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridRiduzioniLiq() {
    
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetRiduzioniLiquidazioniRegistrate'+ window.location.search,
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetRiduzioniLiquidazioniRegistrateResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpDisp'},
           {name: 'ImpPrenotato'},
           {name: 'NLiquidazione'},
           {name: 'ID'},
           {name: 'Tipo'},
           {name: 'Stato'},
           { name: 'StatoAsString' },
           { name: 'HashTokenCallSic' },
	       { name: 'IdDocContabileSic' }
           ]
	});


    var store = new Ext.data.GroupingStore({
        proxy: proxy,
        reader: reader,
        groupField: 'Tipo',
        sortInfo: {
            field: 'Tipo',
            direction: "ASC"
        }
    });

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
 	     {singleSelect: false,
		        listeners: {
		        rowselect: function(sm, row, rec) {
	                var multiSelect = sm.getSelections().length > 1;

	                actionAddRiduzioneLiq.setDisabled(true);
	                actionDeleteRiduzioneLiq.setDisabled(multiSelect); 
                    
                    if (rec.data.Stato == 2)  	                        
                        actionConfermaRiduzioneLiq.setDisabled(false); 	                     	                  
                },
                rowdeselect: function(sm, row, rec) {
                    var selectedRowsCount = sm.getSelections().length;

                    actionDeleteRiduzioneLiq.setDisabled(selectedRowsCount != 1);
                    actionAddRiduzioneLiq.setDisabled(selectedRowsCount == 0 ? false : true);               
                    actionConfermaRiduzioneLiq.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));
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
		                { renderer: eurRend, header: "Importo Ridotto", dataIndex: 'ImpPrenotato', sortable: true ,summaryType:'sum' },
		               	{ header: "Numero Liquidazione", dataIndex: 'NLiquidazione', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true}  ,
		            	{ header: "Stato", dataIndex: 'StatoAsString'} 
		    ]);
		            	 
		    var GridRiduzioniLiq = new Ext.grid.EditorGridPanel({ 
				    	id: 'GridRiduzioniLiq',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        loadMask: true,
				        autoWidth:true,
				        layout: 'fit',
				        plugins: summary,
                        view: new Ext.grid.GroupingView({
				            forceFit:true,
                            showGroupName: false,
                            enableNoGroups:true, // REQUIRED!
                            hideGroupedColumn: true,
                            enableGroupingMenu: true
                        })

                    });
              actionAddRiduzioneLiq.setDisabled(false);
	          actionDeleteRiduzioneLiq.setDisabled(true);
	          actionConfermaRiduzioneLiq.setDisabled(true);
	         
 return GridRiduzioniLiq; 
}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LA RIDUZIONE
function EliminaRiduzioneLiq(ID) {
	var params = { ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/EliminaRiduzioneLiq',
       params: Ext.encode(params),
       method: 'POST',
       success: function(response, options) {
           mask.hide();
           var data = Ext.decode(response.responseText);
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
   actionAddRiduzioneLiq.setDisabled(false);
   actionDeleteRiduzioneLiq.setDisabled(true);
   actionConfermaRiduzioneLiq.setDisabled(true);
}

//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaRiduzioneLiq(RiduzioniLiqInfo) {
    var params = { RiduzioniLiqInfo: RiduzioniLiqInfo };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaRiduzioneLiq',
        params: Ext.encode(params),
        method: 'POST',
        success: function(result, response, options) {
            mask.hide();
            Ext.getCmp('GridRiduzioniLiq').getStore().reload();
            actionDeleteRiduzioneLiq.setDisabled(true);
            actionAddRiduzioneLiq.setDisabled(false);
            actionConfermaRiduzioneLiq.setDisabled(true);
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
                        Ext.getCmp('GridRiduzioniLiq').getStore().reload();
                        actionDeleteRiduzioneLiq.setDisabled(true);
                        actionAddRiduzioneLiq.setDisabled(false);
                        actionConfermaRiduzioneLiq.setDisabled(true);
                    }
                }
            });
        }
    });
}

function ConfermaRiduzioneLiq(ID) {
	var params = {ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaRiduzioneLiq',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           mask.hide();
           Ext.getCmp('GridRiduzioniLiq').getStore().reload();
       },
      failure: function(response,result, options) {
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
function NascondiColonneRidLiq()
{    if(Ext.getCmp('GridRiduzioniLiq') != undefined ){
        var index = Ext.getCmp('GridRiduzioniLiq').colModel.getColumnCount(false);
        index = index-1;
        Ext.getCmp('GridRiduzioniLiq').colModel.setHidden(index,true) ;
        actionConfermaRiduzioneLiq.hide();
    }
}


