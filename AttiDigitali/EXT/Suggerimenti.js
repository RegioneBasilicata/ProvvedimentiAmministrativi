var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


function clock() {
   Ext.getCmp('GridSuggerimenti').getStore().reload();   
}




//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "suggerimenti"
var myPanelSuggerimenti = new Ext.FormPanel({
id:'myPanelSuggerimenti',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 800,
	xtype: "form",
	title : "Suggerimenti"
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
 
 
//FUNZIONE CHE RIEMPIE LA GRIGLIA "Suggerimenti"
function buildGridSuggerimenti() {


var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetSuggerimenti' + window.location.search,
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetSuggerimentiResult',

	fields: [
	
           {name: 'Id'},
           { name: 'Data', type: 'date',dateFormat: 'd/m/Y' },
           {name: 'Autore'},
           {name: 'DescrizioneTipologia'},
           {name: 'Note'},
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

     
       var sm = new Ext.grid.RowSelectionModel(); 

              var ColumnModel = new Ext.grid.ColumnModel([
              	{ header: "", width:0, dataIndex: 'Id', id: 'Id',sortable: false, hidden:true },
	                 	{ header: "Data", width: 60, dataIndex: 'Data', sortable: true, renderer: Ext.util.Format.dateRenderer('d/m/Y') },
						{ header: "Autore",   width: 60,dataIndex: 'Autore', id: 'Autore', sortable: true	 }		,	
						{ header: "DescrizioneTipologia", width: 140, dataIndex: 'DescrizioneTipologia', id: 'DescrizioneTipologia', sortable: true	 }		,			
						{ header: "Note", width: 300, dataIndex: 'Note', id: 'Note', sortable: true,renderer:renderTestoBr}
						
					
		]);
						
		
	    var GridSuggerimenti = new Ext.grid.EditorGridPanel({
	                    id: 'GridSuggerimenti',
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
    Ext.getCmp('myPanelSuggerimenti').add(GridSuggerimenti); 
}
