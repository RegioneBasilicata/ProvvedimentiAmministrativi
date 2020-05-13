var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaRiduzionePreImp = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati della riga selezionata',
    handler: function() {
        Ext.each(Ext.getCmp('GridRiduzioniPreImp').getSelectionModel().getSelections(), function(rec)
		    {
		        ConfermaRiduzionePreImp(rec.data['ID']);
		        actionConfermaRiduzionePreImp.setDisabled(true);
		     }
		)
    },
    iconCls: 'save'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
  var actionAddRiduzionePreImp = new Ext.Action({
         text:'Aggiungi',
         tooltip:'Aggiungi una nuova riga',
         handler: function() {
         var mostraAnno = false;
         InitFormCapitoliRiduzionePreImp(mostraAnno);
        },
        iconCls: 'add'
    });
  
  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA  
    var actionDeleteRiduzionePreImp = new Ext.Action({
         text:'Cancella',
         tooltip:'Cancella selezionato',
        handler: function(){
            var storeGridRidPreImp=Ext.getCmp('GridRiduzioniPreImp').getStore();
            Ext.each(Ext.getCmp('GridRiduzioniPreImp').getSelectionModel().getSelections(), function(rec) {
                EliminaRiduzionePreImp(rec.data['ID']);
                storeGridRidPreImp.remove(rec);
            });
            actionAddRiduzionePreImp.setDisabled(false);
        },
        iconCls: 'remove'
    });

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelRiduzioniPreImp = new Ext.FormPanel({
    id:'myPanelRiduzioniPreImp',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni PreImpegni registrate",
	tbar:[actionAddRiduzionePreImp,actionDeleteRiduzionePreImp, actionConfermaRiduzionePreImp]
});

var labelNumPreImpRid = new Ext.form.Label({
						  text: 'Numero Prenotazione Impegno: ',
						  id: 'labelNumPreImpRid' 
						 });    


  
var ComboPreImpRid = new Ext.form.ComboBox({
fieldLabel: 'NumeroPreImpegno',
displayField: 'NumPreImp',
valueField: 'NumPreImp',
id: 'ComboPreImpRid',
       name: 'ComboPreImpRid',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Preimpegni ...',
        mode:'local'

});
  
    
//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
var panelPreImpRid = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumPreImpRid,
									ComboPreImpRid
								 ]
								}); 
								
//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliRiduzionePreImp(mostraAnno) {
  var AnnoBilancioRiduzionePreImp= new Ext.ux.YearMenu({
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
   			        var btnRicercaRiduzionePreImp = new Ext.Button({
   			            text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
   			            id: 'btnBilancio'
   			        });

   			        btnRicercaRiduzionePreImp.on('click', function() {
   			      
   			            var fNewAnno = Ext.getCmp("AnnoIni").value;
   			            //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
   			            gridCapitoli = buildGridStartRiduzionePreImp(fNewAnno);
   			            capitoloPanelRiduzionePreImp.remove('GridCapitoli')
   			            capitoloPanelRiduzionePreImp.add(gridCapitoli);
   			            capitoloPanelRiduzionePreImp.doLayout();
   			        });

 var tbarDateRiduzionePreImp = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancioRiduzionePreImp},
        			 {
			        xtype: 'textfield',
			       	cls: 'titfis',
			        readOnly: true,
			        id: 'AnnoIni',
			        width: 80,
 			        value: new Date().format('Y')
			    },
			    btnRicercaRiduzionePreImp
			 ]
	});
	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelRiduzionePreImp = new Ext.Panel({
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 	
						
	labelNumPreImpRid = new Ext.form.Label({
		text: 'Numero Prenotazione Impegno: ',
		id: 'labelNumPreImpRid' 
	});    

    ComboPreImpRid = new Ext.form.ComboBox({
        fieldLabel: 'NumeroPreImpegno',
        displayField: 'NumPreImp',
        valueField: 'NumPreImp',
        id: 'ComboPreImpRid',
        name: 'ComboPreImpRid',
        mode: 'local',
        listWidth: 150,
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Preimpegni ...'
    });
     
 
    panelPreImpRid = new Ext.Panel({
				xtype : "panel",
				title : "",
				width:300,
				buttonAlign: "center",
				autoHeight:true,
				layout: "fit",
				items: [
					labelNumPreImpRid,
					ComboPreImpRid
				 ]
	}); 

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanelRiduzionePreImp = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{
								    layout : "fit"
								}],
								buttons: [{
									text: 'Aggiungi Riduzione',
									id: 'btnRiduciPreImp'
								}]
					}); 
 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
 var formCapitoliRiduzionePreImp = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per riduzione preimpegno',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 800,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDateRiduzionePreImp
        		],
            items: [capitoloPanelRiduzionePreImp]
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
            	panelPreImpRid
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
            },{
                xtype: 'numberfield',
                decimalSeparator: ',',
                allowNegative :false,
                fieldLabel: 'Importo da ridurre (*)',
                name: 'ImpDaRidurrePreImp',
                id: 'ImpDaRidurrePreImp'
            },
            buttonPanelRiduzionePreImp]
        }]        
    });

    if (mostraAnno == false) {
        tbarDateRiduzionePreImp.hide();
    } else { tbarDateRiduzionePreImp.show(); }

 var popupRiduzionePreImp = new Ext.Window({ y:15,
					            title: 'Aggiungi una riduzione di un preimpegno',
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
					            popupRiduzionePreImp.add(formCapitoliRiduzionePreImp);
					            popupRiduzionePreImp.doLayout(); //forzo ridisegno
					         
	//GESTISCO L'AZIONE DEL BOTTONE "Aggiungi Riduzione"
					            Ext.getCmp('btnRiduciPreImp').on('click', function() {
					                if ((Ext.get('ImpDaRidurrePreImp').dom.value == 0) || (Ext.get('ImpDaRidurrePreImp').dom.value == '')) {
					                    Ext.MessageBox.show({
					                        title: 'Gestione Riduzione Preimpegno',
					                        msg: 'Il campo Importo da ridurre non &egrave; stato valorizzato!',
					                        buttons: Ext.MessageBox.OK,
					                        icon: Ext.MessageBox.ERROR,
					                        fn: function(btn) { return }
					                    });
					                } else {
					                    if (Ext.get('ImpDaRidurrePreImp').dom.value == 0) {
					                        Ext.MessageBox.show({
					                            title: 'Gestione Riduzione Preimpegno',
					                            msg: 'Il campo Importo da ridurre non può essere maggiore della disponibilità!',
					                            buttons: Ext.MessageBox.OK,
					                            icon: Ext.MessageBox.ERROR,
					                            fn: function(btn) { return }
					                        });

					                    } else {

					                        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
					                        formCapitoliRiduzionePreImp.getForm().timeout = 100000000;
					                        formCapitoliRiduzionePreImp.getForm().submit({
					                            url: 'ProcAmm.svc/GenerazioneRiduzionePreImpUP',
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
								        title: 'Gestione Riduzione Preimpegno',
								        msg: lstr_messaggio,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.ERROR,
								        fn: function(btn) {
								            Ext.getCmp('GridRiduzioniPreImp').getStore().reload();
								            return;
								        }
								    });

								}, // FINE FAILURE
					                            success:
								function(result, response) {
								    var msg = 'Riduzione Preimpegno effettuato con successo!';
								    Ext.MessageBox.show({
								        title: 'Gestione Riduzione Preimpegno',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {
								            Ext.getCmp('GridRiduzioniPreImp').getStore().reload();
								            popupRiduzionePreImp.close();
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
	 	var gridCapitoli = buildGridStartRiduzionePreImp(fAnno);
	    capitoloPanelRiduzionePreImp.add(gridCapitoli);
		panelPreImpRid.hide();
       
popupRiduzionePreImp.show();

//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
	 var formPanelDropTargetEl = formCapitoliRiduzionePreImp.body.dom;
	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
	      ddGroup: 'gridDDGroup',
	      notifyEnter: function(ddSource, e, data) {
	      //EFFETTI VISIVI SUL DRAG & DROP
	      formCapitoliRiduzionePreImp.body.stopFx();
	      formCapitoliRiduzionePreImp.body.highlight();
	      },
	      notifyDrop: function(ddSource, e, data) {
	          // CATTURO IL RECORD SELEZIONATO
	          var selectedRecord = ddSource.dragData.selections[0];
	          // CARICO IL RECORD NELLA FORM
	          formCapitoliRiduzionePreImp.getForm().loadRecord(selectedRecord);
	          // RENDO VISIBILE panelImp
	           buildComboPreImpRid(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
	          //LU aggiunta formattazione imp disp 
	 
	          Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)

	          //LU Fine  aggiunta formattazione imp disp 
	          buildComboPreImpRid(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
	          panelPreImpRid.doLayout();
		      panelPreImpRid.show();
	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
	          // CANCELLO IL RECORD DALLA GRIGLIA.
	          //ddSource.grid.store.remove(selectedRecord);
	          return (true);
	      }
	  }); 


 
}// fine InitFormCapitoli


function VerificaDisponibilitaRiduzionePreImp() {
    try {
        Ext.get('DataAtto').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.DataAtto;
        Ext.get('TipoAtto').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.TipoAtto;
        Ext.get('NumeroAtto').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.NumeroAtto;
        Ext.get('ImpDisp').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.ImpDisp;
        //LU aggiunta formattazione imp disp 
        Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
        //LU Fine  aggiunta formattazione imp disp 
        Ext.get('Oggetto_Impegno').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.ImpPotenzialePrenotato);
  //       Ext.get('COGSemplice').dom.value = ComboPreImpRid.store.data.get(ComboPreImpRid.selectedIndex).data.Codice_Obbiettivo_Gestionale
        Ext.get('ImpDaRidurrePreImp').dom.value = 0;
        Ext.getCmp('btnRiduciPreImp').show();
    } catch (ex) { 
    }
}

function buildComboPreImpRid(AnnoRif, CapitoloRif) {
    panelPreImpRid.remove(ComboPreImpRid);
  
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaPreimp',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

      root: 'GetListaPreimpResult',
         fields: [
           { name: 'NumPreImp' }   ,
           { name: 'ImpDisp' }   ,
           { name: 'TipoAtto' }   ,
           { name: 'DataAtto' }   ,
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("NumPreImp", "ASC");

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


  ComboPreImpRid=  new Ext.form.ComboBox({
        fieldLabel: 'NumeroPreImpegno',
        displayField: 'NumPreImp',
        name: 'ComboPreImpRid',
        id: 'ComboPreImpRid',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Preimpegni ...'

    });
    
    panelPreImpRid.add(ComboPreImpRid);
    
    ComboPreImpRid.show();

    ComboPreImpRid.on('select', function(record, index) { VerificaDisponibilitaRiduzionePreImp() });
    return ComboPreImpRid;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartRiduzionePreImp(annoRif) {
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
	              loadMask: true,
			      viewConfig : { forceFit : true },	  			  
		          ds: store,
		          cm: ColumnModel,
		          stripeRows: true,
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

		                          Ext.get('ImpDaRidurrePreImp').dom.value = '';
		                          Ext.get('TipoAtto').dom.value = '';
		                          Ext.get('NumeroAtto').dom.value = '';
		                          Ext.get('DataAtto').dom.value = '';

		                          // RENDO VISIBILE panelPreImpRid
                            
		                          //LU aggiunta formattazione imp disp
		                        
		                          
		                              Ext.get('ImpDisp').dom.value = eurRend(0)
		                          	                          

		                          //LU Fine  aggiunta formattazione imp disp 
		                       
		                            buildComboPreImpRid(rec.data['Bilancio'], rec.data['Capitolo']);

		                          panelPreImpRid.doLayout();
		                          panelPreImpRid.show();
		                          //Ext.get('NumPreImp').dom.value = "";
		                      }
		                  }
		, scope: this
		              }
		          });
	return grid;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridRiduzioniPreImp() {
    
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetRiduzioniPreImpegniRegistrati'+ window.location.search,
    method:'GET'
    });
    var reader = new Ext.data.JsonReader({
        root: 'GetRiduzioniPreImpegniRegistratiResult',
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
        { name: 'TipoAtto' },
        { name: 'NumeroAtto' },
        { name: 'DataAtto' },
        { name: 'Tipo' },
        { name: 'Stato' },
        { name: 'StatoAsString' },
        { name: 'HashTokenCallSic' },
        { name: 'IdDocContabileSic' }
	]
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
 	     {singleSelect: true,
		        listeners: {
 	                rowselect: function(sm, row, rec) {
 	                    actionAddRiduzionePreImp.setDisabled(true);
 	                    if (rec.data.Stato == 2) {
 	                        actionDeleteRiduzionePreImp.setDisabled(false); 
 	                        actionConfermaRiduzionePreImp.setDisabled(false);
 	                     }else{
 	                        actionDeleteRiduzionePreImp.setDisabled(false); 
 	                        actionConfermaRiduzionePreImp.setDisabled(true);
 	                    }
 	                  
 	                },
 	                 rowdeselect :function(sm, row, rec) {
 	                    actionDeleteRiduzionePreImp.setDisabled(true);
 	                    actionConfermaRiduzionePreImp.setDisabled(true);
 	                    actionAddRiduzionePreImp.setDisabled(false);
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
		               	{ header: "Numero Preimpegno", dataIndex: 'NumPreImp', sortable: true },
		            	{ header: "Tipo Atto", dataIndex: 'TipoAtto', sortable: true, hidden: true },
		            	{ header: "Num. Atto", dataIndex: 'NumeroAtto', sortable: true, hidden: true },
		            	{ header: "Data Atto", dataIndex: 'DataAtto', sortable: true, hidden: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true}  ,
		            	{ header: "Stato", dataIndex: 'StatoAsString'} 
		    ]);
		            	 
		    var GridRiduzioniPreImp = new Ext.grid.EditorGridPanel({ 
				    	id: 'GridRiduzioniPreImp',
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
	          actionDeleteRiduzionePreImp.setDisabled(true);
	         
 return GridRiduzioniPreImp; 
}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LA RIDUZIONE
function EliminaRiduzionePreImp(ID) {
	var params = { ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/EliminaRiduzionePreImp',
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

 

//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
function ConfermaRiduzionePreImp(ID) {
	var params = {ID:ID};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
       url: 'ProcAmm.svc/ConfermaRiduzionePreImp',
       params: Ext.encode(params),
       method: 'POST',
       success: function(result,response, options) {
           mask.hide();
           Ext.getCmp('GridRiduzioniPreImp').getStore().reload();
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
function NascondiColonneRidPreImp(){
    var index = Ext.getCmp('GridRiduzioniPreImp').colModel.getColumnCount(false);
    index = index-1;
    Ext.getCmp('GridRiduzioniPreImp').colModel.setHidden(index,true) ;
    actionConfermaRiduzionePreImp.hide();
}


