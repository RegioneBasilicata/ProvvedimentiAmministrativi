		
var myPanelComponentiUfficio = new Ext.Panel({
id: 'myPanelComponentiUfficio',
 frame: true,
        labelAlign: 'left',
        title: "Dipartimento",
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 770,
        height:100,
        xtype: "form" 
	});
	


function buildGridComponenti(ufficio) {
var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetComponentiUfficio',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

    root: 'GetComponentiUfficioResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader
    });


    var parametri = { ufficioScelto: ufficio };
    store.load({params:parametri});
    store.on({
        'load': {
            fn: function(store, records, options) {
            mask.hide();
             },
            scope: this
        }
    });
      mask.show();
    var sm= new Ext.grid.CheckboxSelectionModel({
            singleSelect: true,
            listeners: {
 	                rowselect: function(sm, row, rec) {
 	                   Ext.getDom('responsabileProcedimento').value = rec.data.Id;     
 	                },
 	                 rowdeselect :function(sm, row, rec) {
 	                    Ext.getDom('responsabileProcedimento').value = ''; 
 	                }
        }
        })
     var ColumnModel = new Ext.grid.ColumnModel([
     sm,
	                { header: "Id", dataIndex: 'Id',  hidden: true },
	                { header: "Responsabile del Procedimento", dataIndex: 'Descrizione',  sortable: true,locked: true }
	               ]);
       
        
       var GridComponenti = new Ext.grid.GridPanel({
        id: 'GridComponenti',
       // da mettere sempre
        height: 250,
        width:360,
        title: '',
        border: true,
        viewConfig: { forceFit: true },
        cm: ColumnModel,
        stripeRows: true,
        ds:store,
        loadMask: true,
        sm: sm
        
    });
return GridComponenti;
}

							