

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraRiduzioneLiqDefinitiva = new Ext.Action({
         text:'Registra',
         tooltip:'Registra la riduzione dela liquidazione selezionata',
        handler: function(){
            registraRiduzioneLiqDefinitiva();
     },
        iconCls: 'add'
    });
    
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
    var myPanelRiduzioniLiq = new Ext.FormPanel({
    id:'myPanelRiduzioniLiq',
    timeout: 100000000,
   	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni Liquidazioni registrate",
	tbar:[actionRegistraRiduzioneLiqDefinitiva],
	items : [
	  	 {
            id: "RiduzioniLiqRagioneria",
            xtype: "hidden"
        },
	     {
	         id: "DataMovimentoRidLiq",
	         xtype: "hidden"
	     }
	     
	     ]
});


//FUNZIONE CHE RIEMPIE LA GRIGLIA "RIDUZIONI SELEZIONATI"
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
           {name: 'ID' },
           {name: 'RegistratoSic'},
           { name: 'Stato' },
           { name: 'HashTokenCallSic' },
	       { name: 'IdDocContabileSic' }
           ]
	});
	
	var store = new Ext.data.Store({
		proxy:proxy
		,reader:reader
	});

    store.load();
    store.on({
   'load':{
      fn: function(store, records, options){
      // mask.hide();
     },
      scope:this
  	 }
 	});

 var checkColumn = new Ext.grid.CheckColumn({
       header: "Registrato",
       dataIndex: 'RegistratoSic',
       width: 55,
       readonly:true
    });

	
 	var sm = new Ext.grid.CheckboxSelectionModel({singleSelect: true,
 	 listeners: {
 	    rowselect: function(sm, row, rec) {
 	        if (rec.data.Stato  != 1){
                if (rec.data.Stato  == 0){
                  alert("Non è possibile registrare questa riduzione poichè risulta non attiva, contattare l'Amministratore del Sistema.");
                }
                if (rec.data.Stato  == 2){
                  alert("Non è possibile registrare questa riduzione poichè risulta non confermata dall'ufficio proponente.");
                }
 	             actionRegistraRiduzioneLiqDefinitiva.setDisabled(true);
 	        } else {	
 	            if ((rec.data.RegistratoSic == true )){
 	                actionRegistraRiduzioneLiqDefinitiva.setDisabled(true); 
 	            }else{
 	                actionRegistraRiduzioneLiqDefinitiva.setDisabled(false); 
 	            }
 	        }
 	    },
 	     rowdeselect :function(sm, row, rec) {
 	     actionRegistraRiduzioneLiqDefinitiva.setDisabled(true); }
 	 }
 	});
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Bilancio", dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
		                { header: "UPB", dataIndex: 'UPB', sortable: true },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		                { header: "Capitolo", dataIndex: 'Capitolo',  sortable: true, locked:false},
		            	{renderer: eurRend, header: "Importo Ridotto", dataIndex: 'ImpPrenotato', sortable: true },
		            	{ header: "NLiquidazione", dataIndex: 'NLiquidazione', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	checkColumn,
		            	{ header: "Stato", dataIndex: 'Stato', hidden: true }
		    ]);
		            	
		    var GridRiduzioneLiq = new Ext.grid.GridPanel({ 
				    	id: 'GridRiepilogoRiduzioneLiq',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',
				        plugins:checkColumn,
				        loadMask: true,
				        viewConfig : { forceFit : true }
	          });
	          actionRegistraRiduzioneLiqDefinitiva.setDisabled(true);
 return GridRiduzioneLiq; 
}

//funzione per registrare le liquidazioni contestuali
function registraRiduzioneLiqDefinitiva() {
    var json = '';
    Ext.each(Ext.getCmp('GridRiepilogoRiduzioneLiq').getSelectionModel().getSelections(), function(rec) {
       json +=Ext.util.JSON.encode(rec.data);
       if ( Ext.getCmp('GridRiepilogoRiduzioneLiq').getSelectionModel().getSelections()[0].data.RegistratoSic == true)
       {
            Ext.MessageBox.show({
			        title: 'Gestione Riduzioni',
					msg: 'Riduzione già registrata sul SIC',
					buttons: Ext.MessageBox.OK,
					icon: Ext.MessageBox.ERROR,
					fn: function(btn) {
						return;
					}
			});
		  return;
     }
    })
       
    if (json!=''){
        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        Ext.getDom('RiduzioniLiqRagioneria').value = json;
        Ext.getDom('DataMovimentoRidLiq').value = getDataMovimentoAggiorna();
        myPanelRiduzioniLiq.getForm().timeout = 100000000;
        myPanelRiduzioniLiq.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneRiduzioneLiq' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure:
								    function(result,response) {
                                        var lstr_messaggio = ''
                                        try {
                                            lstr_messaggio = response.result.FaultMessage
                                        } catch (ex) {
                                            lstr_messaggio = 'Errore Generale'
                                        }
								        Ext.MessageBox.show({
								            title: 'Gestione Riduzioni',
								            msg: lstr_messaggio,
								            buttons: Ext.MessageBox.OK,
								            icon: Ext.MessageBox.ERROR,
								            fn: function(btn) {
								                Ext.getCmp('GridRiepilogoRiduzioneLiq').getStore().reload();
								                return;
								            }
								        });

								    }, // FINE FAILURE
            success:
								    function(result, response) {
								        var msg = 'Aggiornamento Riduzioni effettuato con successo!';
								        Ext.MessageBox.show({
								            title: 'Gestione Riduzioni',
								            msg: msg,
								            buttons: Ext.MessageBox.OK,
								            icon: Ext.MessageBox.INFO,
								            fn: function(btn) {
    								            Ext.getCmp('GridRiepilogoRiduzioneLiq').getStore().reload();
								                var loca = "" + window.location;
								                return;
								            }
								        });
								    } // FINE SUCCESS

        }) // FINE SUBMIT
    }//FINE IF
};      //FINE function

