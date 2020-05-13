var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

var actionScegliOperatore = new Ext.Action({
    text: 'Accedi',
    tooltip: 'Accedi Con Operatore Selezionato',
    id: 'btnLogin',
    handler: function() {

   if ( Ext.getCmp('GridUtente').getSelectionModel().getSelections().length==0) {
       alert('Selezionare un ufficio');
   }
    Ext.each(Ext.getCmp('GridUtente').getSelectionModel().getSelections(), function(rec)
                {
                  
    SettaUtenza(rec.data.CodiceOperatore);
    })
    },
    iconCls: 'add'
});

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function SettaUtenza(ID) {
    Ext.getDom('Selezione').value = ID
    Ext.getCmp('MyPanel').getForm().submit({

                    waitTitle: "Attendere...",
                    waitMsg: 'Aggiornamento in corso ......',
                    success: function(result, response) {
                            if (response.result.success == true) {
                                location.href = response.result.link;
                            }
                        }
    })
}

Ext.onReady(function() {
    Ext.QuickTips.init();

    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetUtenzeCF',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

    root: 'GetUtenzeCFResult',

        fields: [

           { name: 'CodiceUfficio' },
           { name: 'CodiceUfficioPubblico' },
           { name: 'CodiceOperatore' },
           { name: 'DescrizioneUfficio' }
          ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
		, reader: reader
    });
    var codice_fiscale =    Ext.getDom('hiddenCF').value;

    var parametri = { codice_fiscale: codice_fiscale };
    
    store.load({ params: parametri });
    store.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
            },
            scope: this
        }
    });



    var ColumnModel = new Ext.grid.ColumnModel([
      	            { header: "Codice Operatore", width: 60, dataIndex: 'CodiceOperatore', sortable: true, locked: false },
	                { header: "", width: 60, dataIndex: 'CodiceUfficio', id: 'CodiceUfficio', sortable: true , hidden:true},
                    { header: "Codice Ufficio ", width: 60, dataIndex: 'CodiceUfficioPubblico', id: 'CodiceUfficioPubblico', sortable: true },
                    { header: "Descrizione Ufficio", width: 60, dataIndex: 'DescrizioneUfficio', sortable: true } 
	            	]);

var gridUtenze = new Ext.grid.GridPanel({
        id: 'GridUtente',
        autoExpandColumn: 'Capitolo',
        // da mettere sempre 
        height: 350,
        title: '',
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        loadMask: true,
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: true,
        ddGroup: 'gridDDGroup',
        // fine istruzioni per abilitazione Drag & Drop		          
        sm: new Ext.grid.RowSelectionModel({
            singleSelect: true
        })
    });

        // put it in a Panel so it looks pretty
    var panel = new Ext.FormPanel({
        url :'SelezionaUtenzaAction.aspx',
        id: 'MyPanel',
        tbar: [actionScegliOperatore],
            width: 600,
            height: 300,
            collapsible: true,
            layout: 'fit',
            title: 'Sono stati trovati più uffici associati a questo codice fiscale. Selezionare l\'ufficio interessato. <br/> <h1 style="color:#2CAB76; text-align:center">*** ATTENZIONE ***</h1><br/><p style="color:#2CAB76;">A seguito della riorganizzazione di cui alla DGR 227/2014 si comunica che è in corso la ricodifica di tutte le Unità Organizzative della Regione. Pertanto, prima di procedere alla predisposizione del provvedimento si consiglia di attendere l\’attivazione della nuova codifica.<br/>Si confida nella massima collaborazione. </p>',
            items: [gridUtenze, {
            id: "Selezione",
            xtype: "hidden"}]
	     
        });


        //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
        gridUtenze.addListener({
            'rowdblclick':{
                fn: function(grid, rowIndex, event) {
                    var rec = gridUtenze.store.getAt(rowIndex);
                    SettaUtenza(rec.data.CodiceOperatore);
                  }  
                }
        });
        
        
        panel.render('MyPanel');
    });           //FINE Ext.onReady
