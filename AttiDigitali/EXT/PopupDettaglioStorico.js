var maskImp;
maskImp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


function openPopup(idDoc) {
   var popup = new Ext.Window({
        title: 'Dettaglio Storico',
        width: 800,
        autoHeight: true,
        layout: 'fit',
        plain: true,
        buttonAlign: 'right',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        closable: true,
        buttons: [{
            text: 'Chiudi',
            id: 'btnChiudi'
        }]
    });

    var gridRiepilogoStorico = buildGridRiepilogoStorico(idDoc);
    popup.add(gridRiepilogoStorico);
        popup.doLayout();
        Ext.getCmp('btnChiudi').on('click', function() {
            popup.close();
        });
        popup.show();
    }

    function buildGridRiepilogoStorico(IdDoc) {
        var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetInfoStorico' + window.location.search,
            method: 'GET'
        });
        var reader = new Ext.data.JsonReader({
            root: 'GetInfoStoricoResult',
            fields: [
           { name: 'ID' },
           { name: 'IdDocumento' },
           { name: 'Progressivo' },
           { name: 'IdUfficio' },
           { name: 'CodiceUfficio' },
           { name: 'DescrizioneUfficio' },
           { name: 'DenominazUfficioDaVisualizz' },           
           { name: 'Giorni' },
           { name: 'DataArrivo' },
           { name: 'DataUscita' },
           { name: 'Utente' },
           { name: 'Stato' }
           ]
        });

        var store = new Ext.data.GroupingStore({
            proxy: proxy
		, reader: reader
        })

        store.on({
            'load': {
                fn: function(store, records, options) {
                    maskImp.hide();
                },
                scope: this
            }
        });

       
        var parametri = { IdDoc: IdDoc };
        store.load({ params: parametri });

//        var sm = new Ext.grid.CheckboxSelectionModel(
// 	        { singleSelect: false,
// 	            listeners: {
// 	                rowselect: function(sm, row, rec) {
// 	                },
// 	                rowdeselect: function(sm, row, rec) {
// 	                }
// 	            }
// 	        }
// 	    );
        var ColumnModel = new Ext.grid.ColumnModel([
//		                sm,
		                { header: "Ufficio", dataIndex: 'DenominazUfficioDaVisualizz', sortable: true, width: 200},
	                    { header: "Giorni Permanenza", dataIndex: 'Giorni', align: 'center', sortable: true, width: 50 },
	                    { header: "Data Arrivo", dataIndex: 'DataArrivo', sortable: true, width: 50},
		            	{ header: "Data Uscita", dataIndex: 'DataUscita', sortable: true, width: 50},
		            	{ header: "Utente", dataIndex: 'Utente', width: 80},
		            	{ header: "Stato", dataIndex: 'Stato', width: 50}
		    ]);

        var GridRiepStorico = new Ext.grid.GridPanel({
            id: 'GridRiepilogoStorico',
            ds: store,
            colModel: ColumnModel,
//            sm: sm,
            title: "",
            autoHeight: true,
            width: 800,
            loadMask: true
            ,
            view: new Ext.grid.GroupingView({
                forceFit: true,
                showGroupName: false,
                enableNoGroups: true, // REQUIRED!
                hideGroupedColumn: true,
                enableGroupingMenu: true
            })
        });
        return GridRiepStorico;
    }
