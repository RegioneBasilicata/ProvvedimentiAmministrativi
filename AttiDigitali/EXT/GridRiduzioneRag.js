//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraRiduzioneDefinitiva = new Ext.Action({
         text:'Registra',
         tooltip:'Registra la riduzione selezionata',
        handler: function(){
            registraRiduzioneDefinitiva();
     },
        iconCls: 'add'
    });
    
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
    var myPanelRiduzioni = new Ext.FormPanel({
    id:'myPanelRiduzioni',
    timeout: 100000000,
   	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni registrate",
	tbar:[actionRegistraRiduzioneDefinitiva],
	items : [
	  	 {
            id: "RiduzioniRagioneria",
            xtype: "hidden"
        },
	     {
	         id: "DataMovimentoRid",
	         xtype: "hidden"
	     }
	     
	     ]
});


//FUNZIONE CHE RIEMPIE LA GRIGLIA "RIDUZIONI SELEZIONATI"
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
           { name: 'AnnoPrenotazione' },
           { name: 'TipoAtto' },
           { name: 'NumeroAtto' },
           { name: 'DataAtto' },
           { name: 'ID' },
           { name: 'IsEconomia' },
           { name: 'RegistratoSic'},
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
 	         actionRegistraRiduzioneDefinitiva.setDisabled(true);
 	        } else {	
 	            if ((rec.data.RegistratoSic == true )){
 	                actionRegistraRiduzioneDefinitiva.setDisabled(true); 
 	            }else{
 	                actionRegistraRiduzioneDefinitiva.setDisabled(false); 
 	            }
 	        }
 	    },
 	     rowdeselect :function(sm, row, rec) {
 	     actionRegistraRiduzioneDefinitiva.setDisabled(true); }
 	 }
 	});
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Bilancio", dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
		                { header: "UPB", dataIndex: 'UPB', sortable: true },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
		                { header: "Capitolo", dataIndex: 'Capitolo',  sortable: true, locked:false},
		            	{renderer: eurRend, header: "Importo Ridotto", dataIndex: 'ImpPrenotato', sortable: true },
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
		            	checkColumn,
		            	{ header: "Stato", dataIndex: 'Stato', hidden: true }
		    ]);
		            	
		    var GridRiduzione = new Ext.grid.EditorGridPanel({ 
				    	id: 'GridRiepilogoRiduzione',
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
	          actionRegistraRiduzioneDefinitiva.setDisabled(true);
 return GridRiduzione; 
}
function fnEconomia(value) {
    if (value == "")
        return ""
    if (value == "0")
        return "Riduzione";
    if (value == "1")
        return "Economia";
}
//funzione per registrare le liquidazioni contestuali
function registraRiduzioneDefinitiva() {
   // var storeGridRiduzione = Ext.getCmp('GridRiepilogoRiduzione').getStore();
    var json = '';
    Ext.each(Ext.getCmp('GridRiepilogoRiduzione').getSelectionModel().getSelections(), function(rec) {
        json += Ext.util.JSON.encode(rec.data);
        if (Ext.getCmp('GridRiepilogoRiduzione').getSelectionModel().getSelections()[0].data.RegistratoSic == true) {
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
        Ext.getDom('RiduzioniRagioneria').value = json;
        Ext.getDom('DataMovimentoRid').value = getDataMovimentoAggiorna();
        myPanelRiduzioni.getForm().timeout = 100000000;
        myPanelRiduzioni.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneRiduzione' + window.location.search,
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
								                Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
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
    								            Ext.getCmp('GridRiepilogoRiduzione').getStore().reload();
								                var loca = "" + window.location;
								                return;
								            }
								        });
								    } // FINE SUCCESS

        }) // FINE SUBMIT
    }//FINE IF
};      //FINE function

