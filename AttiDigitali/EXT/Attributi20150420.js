var cigAtto = "";
var cupAtto = "";
//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO attributi
    var myPanelAttributi = new Ext.Panel({
    id:'myPanelAttributi',
    timeout: 100000000,
   	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	title : "Informazioni Aggiuntive"
});


//FUNZIONE CHE RIEMPIE LA GRIGLIA "ATTRIBUTI"
function buildGridAttributi(cig, cup) {
    cigAtto = cig;
    cupAtto = cup;
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetAttributi'+ window.location.search,
    method:'GET'
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetAttributiResult',
	fields: [
           {name: 'ID'},
           {name: 'Descrizione'},
           {name: 'TipoDato' },
           {name: 'Valore' }
           ]
	});
	
	var store = new Ext.data.Store({
		proxy:proxy
		,reader:reader
	});

    store.load();
    store.on({
        'load': {
            fn: function(store, records, options) {
                for (var i = 0; i < records.length; i++) {
                    if (records[i].data.ID == 'CIG') {
                        if (records[i].data.Valore == '') {
                            records[i].data.Valore = cigAtto;
                        }
                    }
                    if (records[i].data.ID == 'CUP') {
                        if (records[i].data.Valore == '') {
                            records[i].data.Valore = cupAtto;
                        }
                    }
                }
                var griglia = Ext.getCmp('GridAttributi');
                griglia.getView().refresh();
            },
            scope: this
        }
    });

	
 	var sm = new Ext.grid.CheckboxSelectionModel({singleSelect: true});
 
    var ColumnModel = new Ext.grid.ColumnModel({
    columns: [
		                sm,
		                 { header: "ID", dataIndex: 'ID',hidden: true},
		                 { header: "Descrizione", dataIndex: 'Descrizione', hidden: false },
		                 { header: "TipoDato", dataIndex: 'TipoDato', sortable: true,editable: false, hidden: true},
		                 { header: "Valore", dataIndex: 'Valore', sortable: true,editable: true}
		       
		    ],
	editors: {
		'string': new Ext.grid.GridEditor(new Ext.form.TextField({})),
		'lista': new Ext.grid.GridEditor(new Ext.form.ComboBox({
		
		})),
		'boolean': new Ext.grid.GridEditor(new Ext.form.ComboBox({
		    
		})),
		'datetime': new Ext.grid.GridEditor(new Ext.form.DateField({})),
		'number': new Ext.grid.GridEditor(new Ext.form.NumberField({}))
	},
	getCellEditor: function(colIndex, rowIndex) {
		var field = this.getDataIndex(colIndex);
		if (field == 'Valore') {
			var rec = store.getAt(rowIndex);
			return this.editors[rec.get('TipoDato')];
		}

		return Ext.grid.ColumnModel.prototype.getCellEditor.call(this, colIndex, rowIndex);
	}
	
});
		            	
		    var GridAttributi = new Ext.grid.EditorGridPanel({ 
				    	id: 'GridAttributi',
				        ds: store,
				 		colModel :ColumnModel,
				 		sm:  sm,
						title : "",
				        autoHeight:true,
				        clicksToEdit: 1,
				        autoWidth:true,
				        layout: 'fit',
				        loadMask: true,
				        viewConfig : { forceFit : true }
	          });
	          
      return GridAttributi;
}
function setAttributiInseriti() {
    if (Ext.getCmp('GridAttributi') != undefined) {
            var lstr = '';
         var records = Ext.getCmp('GridAttributi').getStore().getRange();
         for (var i = 0; i < records.length; i++) {
             records[i].data.Descrizione = '';
             if (lstr != '')
                 lstr += ','
             lstr += (Ext.util.JSON.encode(records[i].data));

         }
         if (Ext.getDom('AttributiRegistrati') != null) {
                Ext.getDom('AttributiRegistrati').value = lstr;
         }
     }
 }

 function getValoreAttributo(idAttributo) {
     var retValue = null;
     if (idAttributo != null && idAttributo != undefined) {
         if (Ext.getCmp('GridAttributi') != undefined) {
             var records = Ext.getCmp('GridAttributi').getStore().getRange();
             if (records != null && records != undefined) {
                 for (var i = 0; i < records.length; i++) {
                     if (idAttributo == records[i].data.ID) {
                         retValue = records[i].data.Valore;
                         break;
                     }
                 }
             }
         }
     }
     return retValue;
 }

