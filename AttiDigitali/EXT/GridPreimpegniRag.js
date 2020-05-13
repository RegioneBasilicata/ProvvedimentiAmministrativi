var parseQueryString = function(queryString) {
    var params = {}, queries, temp, i, l;
    var queryString = queryString.replace('?', '');
    // Split into key/value pairs
    queries = queryString.split("&");

    // Convert the array of strings into an object
    for (i = 0, l = queries.length; i < l; i++) {
        temp = queries[i].split('=');
        params[temp[0]] = temp[1];
    }

    return params;
};

var params;
params = parseQueryString(window.location.search);
var tipo = params["tipo"];

var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

//DEFINISCO L'AZIONE REGISTRA DELLA GRIGLIA LIQUIDAZIONI CONTESTUALI
  var actionRegistraPreimpegno = new Ext.Action({
         text:'Registra',
         tooltip:'Registra il preimpegno selezionato',
        handler: function(){
            ResgistraPreimpegno();
     },
        iconCls: 'add'
    });
    
//FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
function buildGridPreimpegni() {

var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetPreImpegniRegistratiProvvisori' + window.location.search,
    method:'GET'
    }); 
      
	var reader = new Ext.data.JsonReader({

	root: 'GetPreImpegniRegistratiProvvisoriResult',
	fields: [
	        { name: 'ID' },
	        { name: 'NumPreImp' },
           {name: 'Bilancio'},
           { name: 'UPB' },
            { name: 'Capitolo' },
           { name: 'ImpPrenotato' },
           { name: 'Stato' },
           {name: 'Codice_Obbiettivo_Gestionale'},
           { name: 'PianoDeiContiFinanziario' },
           {name: 'MissioneProgramma' },
          { name: 'TipoAssunzioneDescr' },
          { name: 'TipoAssunzione' },
          { name: 'DataAssunzione' },
          { name: 'NumeroAssunzione' },
          { name: 'RegistratoSic' },
	        { name: 'HashTokenCallSic' }]
	});
    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        listeners: {
            'beforeload': function(store, operation, eOpts) {
                actionRegistraPreimpegno.setDisabled(true);
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

var ufficio =  Ext.get('Cod_uff_Prop').dom.value;

var parametri = { CodiceUfficio: ufficio };
   store.load({params:parametri});
   var sm = new Ext.grid.CheckboxSelectionModel({
       singleSelect: true,
       listeners: {
           rowselect: function(sm, row, rec) {
               if (rec.data.Stato == 0) {
                   alert("Non è possibile registrare questo impegno poichè risulta non attivo, contattare l'Amministratore del Sistema.");
                   actionRegistraPreimpegno.setDisabled(true);
               } else if (rec.data.Stato == 1 && rec.data.TipoAssunzioneDescr == "PREIMP-PROVV") {
                   if (tipo == 1) {
                        actionRegistraPreimpegno.setDisabled(true);
                   } else {
                        actionRegistraPreimpegno.setDisabled(false);
                   }
               } else {
                   actionRegistraPreimpegno.setDisabled(true);
               }
           }
 	                 ,
           rowdeselect: function(sm, row, rec) {
               actionRegistraPreimpegno.setDisabled(true);
           }
       }
   });
   var checkColumnRegistratoSIC = new Ext.grid.CheckColumn({
       header: "Registrato",
       dataIndex: 'RegistratoSic',
       width: 55,
       readonly: true
   });
	         var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Numero<br/> PreImpegno", dataIndex: 'NumPreImp', sortable: true},
		                { header: "Bilancio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true},
		                { header: "UPB", dataIndex: 'UPB', sortable: true, width: 50},
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true},
		                { header: "Capitolo", dataIndex: 'Capitolo', sortable: true, locked: false, width: 60 },
		            	{ renderer: eurRend, header: "Importo", dataIndex: 'ImpPrenotato', sortable: true, width: 80 },
		            	{ header: "COG", dataIndex: 'Codice_Obbiettivo_Gestionale', id: 'Codice_Obbiettivo_Gestionale', sortable: true},
		            	{ header: "PCF", dataIndex: 'PianoDeiContiFinanziario', id: 'PianoDeiContiFinanziario', sortable: true, 
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
		            	                var rows = Ext.getCmp('GridRiepilogoPreimpegno').getSelectionModel().getSelections();
		            	                var params = {
		            	                    AnnoRif: rows[0].data.Bilancio,
		            	                    CapitoloRif: rows[0].data.Capitolo
		            	                };
		            	                Ext.getCmp('pcfList').store.reload({ params: params });
		            	            }
		            	        }
		            	    })
		            	}, checkColumnRegistratoSIC,
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	{ header: "Stato", dataIndex: 'Stato', hidden: true } 

		    ]);
		            	
		    var GridRiepPreimpegni = new Ext.grid.EditorGridPanel({
				    	id: 'GridRiepilogoPreimpegno',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        autoWidth:true,
				        layout: 'fit',
				        viewConfig : {  forceFit:true },
				        loadMask: true,
				        plugins: checkColumnRegistratoSIC,
				        cls: 'row-summary-style-GridBeneficiariRag'

	          });
              actionRegistraPreimpegno.setDisabled(true);
 return GridRiepPreimpegni; 
}

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelPreimpegni = new Ext.FormPanel({
    id:'myPanelPreimpegni',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	tbar:[actionRegistraPreimpegno],
	title : "Preimpegni Registrati",
	 items : [
	  	 {
            id: "PreimpegniRagioneria",
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



//FUNZIONE CHE REGISTRA GLI IMPEGNI (anche perenti)
function ResgistraPreimpegno() {
    
    var json = '';
    Ext.each(Ext.getCmp('GridRiepilogoPreimpegno').getSelectionModel().getSelections(), function(rec) {
        json +=Ext.util.JSON.encode(rec.data);
    });
    
    
    if (json!=''){
	    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        Ext.getDom('PreimpegniRagioneria').value =json;
        Ext.getDom('DataMovimento').value = getDataMovimentoAggiorna();
        myPanelPreimpegni.getForm().timeout = 100000000;
        myPanelPreimpegni.getForm().submit({
            url: 'ProcAmm.svc/GenerazionePreimpegnoRag' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            success: function(response, result) {
                mask.hide();
                Ext.getCmp('GridRiepilogoPreimpegno').getStore().reload();
            },
            failure:
	            function(result, response) {
                    Ext.getCmp('GridRiepilogoPreimpegno').getStore().reload();
	                var lstr_messaggio = '';
                    try {
                        lstr_messaggio = response.result.FaultMessage;
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
	                });//fine messagebox
	            }//fine function
        });//fine submit
    }//Fine if
}//fine function GenerazioneImpegno