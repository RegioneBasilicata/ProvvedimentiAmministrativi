var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


function clock() {
   Ext.getCmp('GridMessaggi').getStore().reload();   
}


  //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA  
    var actionDeleteMessaggi = new Ext.Action({
         text:'Cancella',
         tooltip:'Cancella selezionato',
        handler: function(){
            var storeGridMessaggi=Ext.getCmp('GridMessaggi').getStore();
			var GridMessaggi=Ext.getCmp('GridMessaggi');
			   Ext.each(GridMessaggi.getSelectionModel().getSelections(), function(rec)
						{
						    EliminaMessaggio(rec.data['Id']);   
						    storeGridMessaggi.remove(rec); 
						
						})  						       
        },
        iconCls: 'remove'
    });

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "messaggi"
var myPanelMessaggi = new Ext.FormPanel({
id:'myPanelMessaggi',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 800,
	xtype: "form",
	title : "Messaggi",
	tbar: [actionDeleteMessaggi]
});

 function formatDate(value){
    return value ? value.dateFormat('F d, Y') : '';  
 };
 function renderTestoBr(value) {
     if (value.toString().length <= 180) { 
         return value + '<br/>'
        }
 return value 
 }
 
 
//FUNZIONE CHE RIEMPIE LA GRIGLIA "Messaggi"
function buildGridMessaggi() {


var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetMessaggi' + window.location.search,
    method:'GET',
    timeout: 10000000
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetMessaggiResult',

	fields: [
	
           {name: 'Id'},
           {name: 'Img'},
           {name: 'Mittente'},
           {name: 'Destinatario'},
           { name: 'Data', type: 'date',dateFormat: 'd/m/Y' },
           {name: 'Testo'},
           {name: 'IdDocumento'}
          ]
	});
	
	var store = new Ext.data.Store({
		proxy:proxy
		,reader:reader
	});

	
    store.load();
    store.on({
   'load':{
   fn: function(store, records, options) {
       mask.hide();
     },
      scope:this
  	 }
 	});

     
       var sm = new Ext.grid.CheckboxSelectionModel({
         listeners: {
                    rowselect: function(sm, row, rec) {
                    actionDeleteMessaggi.setDisabled(false);
 	                 }
 	             }
       }); 
 	          function renderIcon(val) {
                return '<img src="' + val + '">';
              }

              var ColumnModel = new Ext.grid.ColumnModel([
	                    sm,
	                    { header: "Id", width: 10, dataIndex: 'Id', id: 'Id', sortable: false, hidden: true },
						{ header: "", width: 10, dataIndex: 'Img', sortable: true, renderer: renderIcon },
						{ header: "Data", width: 60, dataIndex: 'Data', sortable: true, renderer: Ext.util.Format.dateRenderer('d/m/Y') },
						{ header: "Testo", width: 540, dataIndex: 'Testo', id: 'Testo', sortable: true,renderer:renderTestoBr,
						    editor: new Ext.form.TextArea({
						        emptyText: '',
						        allowBlank: true,
						        readOnly: true,
						        height:70
						    })
						}
					
		]);
						
		
	    var GridMessaggi = new Ext.grid.EditorGridPanel({
	                    id: 'GridMessaggi',
				        ds: store,
				        colModel: ColumnModel,
				        sm: sm,
				        frame:true,
						title : "",
				        autoHeight:true,
				        autoWidth: true,
				        clicksToEdit: 1,
				        footer: true,
				        layout: 'fit',
				        viewConfig : {forceFit:true },
				        loadMask: true,
				        stripeRows:true

	          });
            //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
		          GridMessaggi.addListener({
		              'rowdblclick': {
		              fn: function(grid, rowIndex, event) {
		                  Ext.each(Ext.getCmp('GridMessaggi').getSelectionModel().getSelections(), function(rec) {
                          ContrassegnaComeLetto(rec.data.Id) ;
                          });
                      }
		, scope: this
		              }
		          });
		          
    Ext.getCmp('myPanelMessaggi').add(GridMessaggi); 
}


//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function EliminaMessaggio(ID) {
    var params = { ID: ID };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/EliminaMessaggio',
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

}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function ContrassegnaComeLetto(ID) {
 
    var params = { ID: ID };
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ContrassegnaComeLetto',
        params: Ext.encode(params),
        method: 'POST',
        success: function(response, options) {
            mask.hide();
               Ext.getCmp('GridMessaggi').getStore().reload();
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
