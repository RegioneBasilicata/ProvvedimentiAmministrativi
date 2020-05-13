var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});



//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelMandati = new Ext.FormPanel({
    frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    collapsible: true,
    width: 750,
    xtype: "form",
    title: "Mandati registrati"
});

        //FUNZIONE CHE RIEMPIE LA GRIGLIA "CAPITOLI SELEZIONATI"
        function buildGridMandati() {

            var proxy = new Ext.data.HttpProxy({
                url: 'ProcAmm.svc/GetMandatiRegistrati' + window.location.search,
                method: 'GET'
            });
            var reader = new Ext.data.JsonReader({

                root: 'GetMandatiRegistratiResult',
                fields: [
           { name: 'NLiquidazione' },
           { name: 'Nmandato' },
           { name: 'DataMandato' },
           { name: 'NImporto' },
           { name: 'Nimpegno' },
           { name: 'Doc_id' },
           { name: 'ID' }
           ]
            });

            var store = new Ext.data.Store({
                proxy: proxy
		, reader: reader
            });

            store.load();
            store.on({
                'load': {
                    fn: function(store, records, options) {
                        mask.hide();
                    },
                    scope: this
                }
            });


            var sm = new Ext.grid.CheckboxSelectionModel();
            var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                { header: "Numero Liquidazione", dataIndex: 'NLiquidazione', sortable: true },
		            	{ header: "Numero Mandato", dataIndex: 'Nmandato', sortable: true },
		            	{ header: "Data Mandato", dataIndex: 'DataMandato', sortable: true },
		            	{renderer: eurRend, header: "Importo", dataIndex: 'NImporto', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true }
		    ]);

            var GridMandato = new Ext.grid.GridPanel({
                id: 'GridRiepilogoMandato',
                ds: store,
                colModel: ColumnModel,
                sm: sm,
                title: "",
                autoHeight: true,
                autoWidth: true,
                layout: 'fit',
                loadMask: true,
                viewConfig: { forceFit: true }
            });
            return GridMandato;
        }

     