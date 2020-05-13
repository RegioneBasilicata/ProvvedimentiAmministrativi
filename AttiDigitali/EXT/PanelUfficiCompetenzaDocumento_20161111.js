var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
var myFormPanelUfficiCompetenza = null;
var myFormPanelUtentiUfficiCompetenza = null;
var myFormPanelUfficiCompetenzaRegistrati = null;


function VerificaEsistenzaRecord(rec) {
    var flagTrovato = false;
 
  var conta = 0;
    Ext.getCmp('GridListaufficiSelezionati').getStore().each(function(recordStore) {
        conta= conta +1;
        var newCodiceIntero = recordStore.data.CodiceInterno
        if (newCodiceIntero == rec.data.CodiceInterno) {
                flagTrovato = true;
            }
    })
    
    var tipo = Ext.getDom('tipo').value;
    if (tipo == '1'){
        if (conta >=1){
            alert("Non è possibile assegnare più di un ufficio proponente.")
            flagTrovato = true;
            return flagTrovato;
        }
    }
    
    
    return flagTrovato
   
}
var actionAddUfficio = new Ext.Action({
    text: 'Aggiungi',
    tooltip: 'Aggiungi selezionato',
    handler: function() {
        var storeGridLiq = Ext.getCmp('GridListauffici').getStore();
        var GridLiquidazione = Ext.getCmp('GridListauffici');
        Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function(rec) {
            if (!VerificaEsistenzaRecord(rec)) {
                Ext.getCmp('GridListaufficiSelezionati').getStore().add(rec);
            }
        })
    },
    iconCls: 'add'
});


var actionDeleteUfficio = new Ext.Action({
    text: 'Cancella',
    tooltip: 'Cancella selezionato',
    handler: function() {
        var storeGridLiq = Ext.getCmp('GridListaufficiSelezionati').getStore();
        var GridLiquidazione = Ext.getCmp('GridListaufficiSelezionati');
        Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function(rec) {
            storeGridLiq.remove(rec);
        })
    },
    iconCls: 'remove'
});



var actionSalvaLista = new Ext.Action({
    text: 'Salva',
    tooltip: 'Salva tutti ',
    handler: function() {

        var storeGridLiq = Ext.getCmp('GridListaufficiSelezionati').getStore();
        var lstr = '';
        storeGridLiq.each(function(storeGridLiq) {
            lstr += storeGridLiq.data.CodiceInterno + ',';
        });
        lstr = lstr.substring(0, lstr.length - 1);
        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';

        Ext.getDom('itemselector_obj').value = lstr;


        if (myFormPanelUfficiCompetenzaRegistrati.getForm().isValid()) {
            myFormPanelUfficiCompetenzaRegistrati.getForm().timeout = 100000000;
            myFormPanelUfficiCompetenzaRegistrati.getForm().submit({
                url: 'ProcAmm.svc/SetUfficiCompetenza' + window.location.search,
                waitTitle: "Attendere...",
                waitMsg: 'Aggiornamento in corso ......',
                failure:
								function(result, response) {
								    var lstr_messaggio = ''
								    try {
								        lstr_messaggio = response.result.FaultMessage
								    } catch (ex) {
								        lstr_messaggio = 'Errore Generale'
								    }

								    Ext.MessageBox.show({
								        title: 'Gestione Uffici Competenze',
								        msg: lstr_messaggio,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.ERROR,
								        fn: function(btn) {
								            return;
								        }
								    });

								}, // FINE FAILURE
                success:
								function(result, response) {
								    var msg = 'Uffici di Competenza al provvedimento salvati!';
								    Ext.MessageBox.show({
								        title: 'Gestione Uffici Competenze',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {
								            //  Ext.getCmp('GridLiquidazioniContestuali').getStore().reload();
								            return;
								        }
								    });
								} // FINE SUCCESS

            }) // FINE SUBMIT

        }
    },
    iconCls: 'save'
});
	


function builFormPanelUfficiCompetenza() {

var storGrid =    new Ext.data.SimpleStore({
fields: [],
        data: []
    });

    myFormPanelUfficiCompetenza = new Ext.form.FormPanel({
        id: 'myFormPanelUfficiCompetenza',
        title: 'Uffici Disponibili',
        width: 400,
        bodyStyle: 'padding:10px;',
        //renderTo: 'itemselector',
        tbar:[actionAddUfficio],
        items: [buildgridUffici(storGrid)],
        layout: 'column',  
        frame: true     
        });

    }

function builFormPanelUtentiUfficiCompetenza() {

var storGrid =    new Ext.data.SimpleStore({
        fields: [],
        data: []
    });

    myFormPanelUtentiUfficiCompetenza = new Ext.form.FormPanel({
        id: 'myFormPanelUtentiUfficiCompetenza',
        title: 'Utenti Disponibili',
        width: 400,
        bodyStyle: 'padding:10px;',
        //renderTo: 'itemselector',
        tbar:[
        //actionAddUfficio
        ],
        items: [
        //buildgridUffici(storGrid)
        ],
        layout: 'column',  
        frame: true     
        });

    }


    function builFormPanelUfficiCompetenzaRegistrati(storeCompetenzaRegistrato) {

        var griglia = buildgridUfficiSelezionati(storeCompetenzaRegistrato);
        
        myFormPanelUfficiCompetenzaRegistrati = new Ext.form.FormPanel({
        id: 'myFormPanelUfficiCompetenzaRegistrati',
        title: 'Uffici Selezionati',
            width: 400,           
            bodyStyle: 'padding:10px;',
            //renderTo: 'itemselector',
            items: [{id: "itemselector_obj",
                     xtype: "hidden"},
	                griglia],
            layout: 'column',
            tbar: [actionDeleteUfficio, actionSalvaLista],
            frame: true
            });

        }
        
    
    
    
				
var myPanelFiltroDipartimento = new Ext.FormPanel({
id: 'myPanelFiltroDipartimento',
 frame: true,
        labelAlign: 'left',
        title: "Dipartimento",
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 800,
        autoHeight:true,
        xtype: "form"
        
	});
	


function buildComboDipartimenti() {
if(Ext.getCmp('ComboDipartimenti')== undefined ){
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetDipartimentiDipendenti',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetDipartimentiDipendentiResult',
        fields: [
           { name: 'CodiceInterno' },
           { name: 'DescrizioneBreve' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader
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
    var ComboDipartimenti = new Ext.form.ComboBox({
        fieldLabel: 'Dipartimento ',
        displayField: 'DescrizioneBreve',
        valueField: 'CodiceInterno',
        id: 'ComboDipartimenti',
        mode: 'local',
        listWidth: 400,
        width: 400,
        triggerAction: 'all',
        store: store
    });
    Ext.getCmp('myPanelFiltroDipartimento').add(Ext.getCmp('ComboDipartimenti'));
    ComboDipartimenti.on('select', function(record, index) {

  
        var codDip = ComboDipartimenti.store.data.get(ComboDipartimenti.selectedIndex).data.CodiceInterno;
 
        
        updateListaUfficiItemSelctor(codDip);
        //Ext.getCmp('myPanelFiltroStruttura').add(comboUff);
         Ext.getCmp('myPanelFiltroDipartimento').doLayout();
    });
}
}

function updateListaUfficiItemSelctor(dipartimentoScelto) {
 var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetUfficiDipendenti',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetUfficiDipendentiResult',
        fields: [
            { name: 'CodiceInterno' },
            { name: 'DescrizioneBreve' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
        , reader: reader
    });



    var parametri = { dipartimentoScelto: dipartimentoScelto };
    store.load({ params: parametri });
    store.on({
        'load': {
            fn: function(store, records, options) {
            mask.hide();
                
               
            },
            scope: this
        }
    });

    var griglia = buildgridUffici(store)

 
    Ext.getCmp('myFormPanelUfficiCompetenza').add(griglia);
    Ext.getCmp('myFormPanelUfficiCompetenza').doLayout();
}


function buildgridUffici(store) {
  

    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "CodiceInterno", width: 60, dataIndex: 'CodiceInterno', id: 'Bilancio',  hidden: true },
	                { header: "DescrizioneBreve", dataIndex: 'DescrizioneBreve',  locked: true }
	               ]);
        if (Ext.getCmp('GridListauffici') != undefined) {
            Ext.getCmp('GridListauffici').destroy();
        }
       var gridUffici = new Ext.grid.GridPanel({
        id: 'GridListauffici',
       // da mettere sempre
        height: 125,
        width:300,
        title: '',
        border: true,
        viewConfig: { forceFit: true },
        cm: ColumnModel,
        stripeRows: true,
        ds:store,
        loadMask: true,
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: true,
        ddGroup: 'gridDDGroup',
        // fine istruzioni per abilitazione Drag & Drop		          
        sm: new Ext.grid.RowSelectionModel({
            singleSelect: true,
            listeners: {
                rowselect: function(sm, row, rec) {
                 
                }
            }
        })
    });


    //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
    gridUffici.addListener({
        'rowdblclick': {
            fn: function(gridUffici, rowIndex, event) {
                var rec = gridUffici.store.getAt(rowIndex);
                if (!VerificaEsistenzaRecord(rec)) {
                    // CARICO IL RECORD NELLA FORM
                    Ext.getCmp('GridListaufficiSelezionati').store.add(rec);
                    Ext.getCmp('GridListauffici').store.remove(rec)
                }
            }
		, scope: this
        }
    });
    
    
    return gridUffici
    //Ext.getCmp('myFormPanelUfficiCompetenza').render('FormPanelUfficiCompetenza')

}




function buildgridUfficiSelezionati(toStore) {

  

    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "CodiceInterno", width: 60, dataIndex: 'CodiceInterno', id: 'Bilancio', sortable: true, hidden: true },
	                { header: "DescrizioneBreve", dataIndex: 'DescrizioneBreve', sortable: true, locked: true }
	               ]);
    if (Ext.getCmp('GridListaufficiSelezionati') != undefined) {
        Ext.getCmp('GridListaufficiSelezionati').destroy();
    }
    var gridUfficiSelezionati = new Ext.grid.GridPanel({
    id: 'GridListaufficiSelezionati',
        // da mettere sempre
        height: 555,
        width: 300,
        title: '',
        border: true,
        viewConfig: { forceFit: true },
        ds: toStore,
        cm: ColumnModel,
        stripeRows: true,
        loadMask: true,
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: false,
        ddGroup: 'gridDDGroup'

    });
    
    return gridUfficiSelezionati;

}

function buildCopyDrop() {

    var formPanelDropTargetEl = Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati').body.dom;
    var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
        ddGroup: 'gridDDGroup',
        notifyEnter: function(ddSource, e, data) {
            //EFFETTI VISIVI SUL DRAG & DROP
            Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati').body.stopFx();
            Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati').body.highlight();
        },
        notifyDrop: function(ddSource, e, data) {
            // CATTURO IL RECORD SELEZIONATO
            var selectedRecord = ddSource.dragData.selections[0];

            // CARICO IL RECORD NELLA FORM
            if (!VerificaEsistenzaRecord(selectedRecord)) {
                Ext.getCmp('GridListaufficiSelezionati').store.add(selectedRecord);
                // RENDO VISIBILE panelImp
                //LU aggiunta formattazione imp disp 

                //LU Fine  aggiunta formattazione imp disp 


                // ISTRUZIONI PER IL DRAG --- ASTERISCATE
                // CANCELLO IL RECORD DALLA GRIGLIA.
                ddSource.grid.store.remove(selectedRecord);
            }
            return (true);
        }
    });

}

var MyPrincipalPanel = null

function buildPanelPrincipale() {

  panelPreImp=  new Ext.Panel({
  xtype: "panel",
  id: 'MyPrincipalPanel',
        title: "",
        width: 800,
        buttonAlign: "center",
        autoHeight: true,
        layout: 'column',
        items: [ { columnWidth: .5,items:[myFormPanelUfficiCompetenza, myFormPanelUtentiUfficiCompetenza]},
				 { columnWidth: .5, items: [myFormPanelUfficiCompetenzaRegistrati]},
				]
    });
}

							