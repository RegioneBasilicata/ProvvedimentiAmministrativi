

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraRiduzionePreImpDefinitiva = new Ext.Action({
         text:'Registra',
         tooltip:'Registra la riduzione del Preimpegno selezionata',
        handler: function(){
            registraRiduzionePreImpDefinitiva();
     },
        iconCls: 'add'
    });
    
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
    var myPanelRiduzioniPreImp = new Ext.FormPanel({
    id:'myPanelRiduzioniPreImp',
    timeout: 100000000,
   	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni Preimpegni registrate",
	tbar:[actionRegistraRiduzionePreImpDefinitiva],
	items : [
	  	 {
            id: "RiduzioniPreImpRagioneria",
            xtype: "hidden"
        },
	     {
	         id: "DataMovimentoRidPreImp",
	         xtype: "hidden"
	     }
	     
	     ]
});


//FUNZIONE CHE RIEMPIE LA GRIGLIA "RIDUZIONI SELEZIONATI"
function buildGridRiduzioniPreImp() {
    
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetRiduzioniPreImpegniRegistrati'+ window.location.search,
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetRiduzioniPreImpegniRegistratiResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpDisp'},
           {name: 'ImpPrenotato'},
           {name: 'NumPreImp'},
           {name: 'AnnoPrenotazione' },
           {name: 'TipoAtto' },
           {name: 'NumeroAtto' },
           {name: 'DataAtto' },
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
 	        actionRegistraRiduzionePreImpDefinitiva.setDisabled(true);
 	        } else {	
 	            if ((rec.data.RegistratoSic == true )){
 	                actionRegistraRiduzionePreImpDefinitiva.setDisabled(true); 
 	            }else{
 	                actionRegistraRiduzionePreImpDefinitiva.setDisabled(false); 
 	            }
 	        }
 	    },
 	     rowdeselect :function(sm, row, rec) {
 	     actionRegistraRiduzionePreImpDefinitiva.setDisabled(true); }
 	 }
 	});
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Bilancio", dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
		                { header: "UPB", dataIndex: 'UPB', sortable: true },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		                { header: "Capitolo", dataIndex: 'Capitolo',  sortable: true, locked:false},
		            	{renderer: eurRend, header: "Importo Ridotto", dataIndex: 'ImpPrenotato', sortable: true },
		            	{ header: "Numero PreImpegno", dataIndex: 'NumPreImp', sortable: true },
		            	{ header: "Tipo Atto", dataIndex: 'TipoAtto', sortable: true, hidden: true },
		            	{ header: "Num. Atto", dataIndex: 'NumeroAtto', sortable: true, hidden: true },
		            	{ header: "Data Atto", dataIndex: 'DataAtto', sortable: true, hidden: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	checkColumn,
		            	{ header: "Stato", dataIndex: 'Stato', hidden: true }
		    ]);
		            	
		    var GridRiduzionePreImp = new Ext.grid.GridPanel({ 
				    	id: 'GridRiepilogoRiduzionePreImp',
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
	          actionRegistraRiduzionePreImpDefinitiva.setDisabled(true);
 return GridRiduzionePreImp; 
}

//funzione per registrare le liquidazioni contestuali
function registraRiduzionePreImpDefinitiva() {
    var json = '';
    Ext.each(Ext.getCmp('GridRiepilogoRiduzionePreImp').getSelectionModel().getSelections(), function(rec) {
        json += Ext.util.JSON.encode(rec.data);
        if (Ext.getCmp('GridRiepilogoRiduzionePreImp').getSelectionModel().getSelections()[0].data.RegistratoSic == true) {
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
    });
       
    if (json!=''){
        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        Ext.getDom('RiduzioniPreImpRagioneria').value = json;
        Ext.getDom('DataMovimentoRidPreImp').value = getDataMovimentoAggiorna();
        myPanelRiduzioniPreImp.getForm().timeout = 100000000;
        myPanelRiduzioniPreImp.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneRiduzionePreImp' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure:
								    function(result,response) {
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
								                Ext.getCmp('GridRiepilogoRiduzionePreImp').getStore().reload();
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
    								            Ext.getCmp('GridRiepilogoRiduzionePreImp').getStore().reload();
								                var loca = "" + window.location;
								                return;
								            }
								        });
								    } // FINE SUCCESS

        }) // FINE SUBMIT
    }//FINE IF
};      //FINE function

