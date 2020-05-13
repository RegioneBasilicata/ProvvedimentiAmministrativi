var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
var myFormPanelUfficiCompetenza = null;
var myFormPanelUfficiCompetenzaRegistrati = null;
var myFormPanelUtentiUfficiCompetenza = null;
var myFormPanelUtentiUfficiCompetenzaRegistrati = null;

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
    if (Ext.getCmp('tab_componenti')!= undefined) {
        if (tipo == 1){
            if (conta >=1){
                alert("Non è possibile assegnare più di un ufficio proponente.")
                flagTrovato = true;
                return flagTrovato;
            }
            var grid = buildGridComponenti(rec.data.CodiceInterno);
            Ext.getCmp('tab_componenti').add(grid);
            Ext.getCmp('myPanel').active = Ext.getCmp("tab_componenti")
            Ext.getCmp('myPanel').active.show();
        }else{
            var ufficioProponente = Ext.getDom('valueUffProp').value;
            var grid = buildGridComponenti(ufficioProponente);
            Ext.getCmp('tab_componenti').add(grid);
            Ext.getCmp('myPanel').active = Ext.getCmp("tab_componenti")
            Ext.getCmp('myPanel').active.show();
        }
    }
    return flagTrovato;   
}

function VerificaEsistenzaRecordUtentiUfficio(rec) {
    var flagTrovato = false;
    var conta = 0;
    Ext.getCmp('GridListaUtentiufficiSelezionati').getStore().each(function(recordStore) {
        conta= conta +1;
        var newAccount = recordStore.data.Account
        if (newAccount == rec.data.Account) {
                flagTrovato = true;
            }
        })
    var tipo = Ext.getDom('tipo').value;
    if (Ext.getCmp('tab_componenti')!= undefined) {
        if (tipo == 1){
            if (conta >=1){
                alert("Non è possibile assegnare più di un ufficio proponente.")
                flagTrovato = true;
                return flagTrovato;
            }
            var grid = buildGridComponenti(rec.data.CodiceInterno);
            Ext.getCmp('tab_componenti').add(grid);
            Ext.getCmp('myPanel').active = Ext.getCmp("tab_componenti")
            Ext.getCmp('myPanel').active.show();
        }else{
            var ufficioProponente = Ext.getDom('valueUffProp').value;
            var grid = buildGridComponenti(ufficioProponente);
            Ext.getCmp('tab_componenti').add(grid);
            Ext.getCmp('myPanel').active = Ext.getCmp("tab_componenti")
            Ext.getCmp('myPanel').active.show();
        }
    }
    return flagTrovato;   
}

var actionAddUfficio = new Ext.Action({
    text: '2. Aggiungi ufficio selezionato',
    tooltip: 'Aggiungi ufficio selezionato per la notifica',
    handler: function() {
        var storeGridLiq = Ext.getCmp('GridListauffici').getStore();
        if (storeGridLiq.data.items.length <= 0) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: "E' necessario selezionare un dipartimento.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    return;
                }
            });
        } else {
            var GridLiquidazione = Ext.getCmp('GridListauffici');
            var rows = GridLiquidazione.getSelectionModel().getSelections();
            if (rows.length != 1) {
                Ext.MessageBox.show({
                title: 'Attenzione',
                    msg: "E' necessario selezionare l'ufficio dalla lista sottostante.",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                        return;
                    }
                });
            } else {
                Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function(rec) {
                    if (!VerificaEsistenzaRecord(rec)) {
                        Ext.getCmp('GridListaufficiSelezionati').getStore().add(rec);
                        Ext.getCmp('GridListaufficiSelezionati').getView().refresh(); 
                        updateListaUtentiUfficiItemSelctor(rec.data.CodiceInterno); 
                    }
                });
            }
        }
    },
    iconCls: 'add'
});


var actionDeleteUfficio = new Ext.Action({
    text: 'Cancella ufficio selezionato',
    tooltip: 'Cancella ufficio selezionato',
    handler: function() {
        var storeGridLiq = Ext.getCmp('GridListaufficiSelezionati').getStore();
        var GridLiquidazione = Ext.getCmp('GridListaufficiSelezionati');
        var rows = GridLiquidazione.getSelectionModel().getSelections();
        if (rows.length != 1) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: "E' necessario selezionare l'ufficio dalla lista sottostante.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    return;
                }
            });
        } else {

            Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function(rec) {
                storeGridLiq.remove(rec);
                Ext.getCmp('GridListauffici').store.add(rec);
            });
            Ext.getCmp('GridListaUtentiuffici').store.removeAll();
        }
    },
    iconCls: 'remove'
});

var actionDeleteUtenteUfficio = new Ext.Action({
    text: 'Cancella utente selezionato',
    tooltip: 'Cancella utente selezionato',
    handler: function() {
        var storeGridUtenti = Ext.getCmp('GridListaUtentiufficiSelezionati').getStore();
        var GridUtenti = Ext.getCmp('GridListaUtentiufficiSelezionati');
        var rows = GridUtenti.getSelectionModel().getSelections();
        if (rows.length != 1) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: "E' necessario selezionare l'utente dalla lista sottostante.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    return;
                }
            });
        } else {

            Ext.each(GridUtenti.getSelectionModel().getSelections(), function(rec) {
                storeGridUtenti.remove(rec);
                Ext.getCmp('GridListaUtentiuffici').store.add(rec);
                
                 Ext.getCmp('GridListaufficiSelezionati').getView().refresh();
            });
        }
    },
    iconCls: 'remove'
});

var actionAddUtente = new Ext.Action({
    text: '3. Aggiungi utente selezionato',
    tooltip: 'Aggiungi utente selezionato per la notifica via mail',
    handler: function() {
        var storeGridUtenti = Ext.getCmp('GridListaUtentiuffici').getStore();
        if (storeGridUtenti.data.items.length <= 0) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: "E' necessario selezionare un ufficio.",
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    return;
                }
            });
        } else {
            var GridUtentiUffici = Ext.getCmp('GridListaUtentiuffici');
            var rows = GridUtentiUffici.getSelectionModel().getSelections();
            if (rows.length != 1) {
                Ext.MessageBox.show({
                title: 'Attenzione',
                    msg: "E' necessario selezionare l'utente dalla lista sottostante.",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                        return;
                    }
                });
            } else {
                Ext.each(GridUtentiUffici.getSelectionModel().getSelections(), function(utente) {
                    //if (!VerificaEsistenzaRecordUtentiUfficio(utente)) {                       
                        Ext.MessageBox.show({
                            title: 'Attenzione',
                            msg: "Si vuole notificare solo alla persona selezionata?<br><br>Cliccare NO per inviare una notifica anche al suo ufficio",
                            buttons: Ext.MessageBox.YESNO,
                            icon: Ext.MessageBox.WARNING,
                            fn: function(btn) {
                                    var storeGridUfficiSelezionati = Ext.getCmp('GridListaufficiSelezionati').getStore();
                                    if (btn == 'yes') {     
                                        Ext.each(storeGridUfficiSelezionati.data.items, function(ufficio) {
                                            if (utente.data.CodiceInternoUfficio == ufficio.data.CodiceInterno) {  
                                                Ext.getCmp('GridListaufficiSelezionati').store.remove(ufficio);
                                                Ext.getCmp('GridListauffici').getStore().add(ufficio); 
                                            }
                                        });         
                                    } else if (btn == 'no') {
                                        var storeGridUffici = Ext.getCmp('GridListauffici').getStore();                
                                        Ext.each(storeGridUffici.data.items, function(ufficio) {
                                            if (utente.data.CodiceInternoUfficio == ufficio.data.CodiceInterno) {  
                                                Ext.getCmp('GridListauffici').store.remove(ufficio);
                                                Ext.getCmp('GridListaufficiSelezionati').getStore().add(ufficio); 
                                            }
                                        });
                                    }  
                                    if (!VerificaEsistenzaRecordUtentiUfficio(utente)) {  
                                        Ext.getCmp('GridListaUtentiuffici').store.remove(utente);
                                        Ext.getCmp('GridListaUtentiufficiSelezionati').getStore().add(utente);  
                                        Ext.getCmp('GridListaUtentiufficiSelezionati').getView().refresh();
                                        
                                        // stef
                                        
                                    }                                                             
                                }
                        });
                    //}
                });
            }
        }
    },
    iconCls: 'add'
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

        Ext.getDom('ufficiDiCompetenza').value = lstr;


        if (myFormPanelUfficiCompetenzaRegistrati.getForm().isValid()) {
            myFormPanelUfficiCompetenzaRegistrati.getForm().timeout = 100000000;
            myFormPanelUfficiCompetenzaRegistrati.getForm().submit({
                url: 'ProcAmm.svc/SetUfficiCompetenza' + window.location.search,
                waitTitle: "Attendere...",
                waitMsg: 'Aggiornamento in corso ......',
                failure:
								function(result, response) {
								    var lstr_messaggio = '';
                                    try {
                                        lstr_messaggio = response.result.FaultMessage;
                                    } catch (ex) {
                                        lstr_messaggio = 'Errore Generale';
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


function setUfficiSelezionati() {

    var storeGridLiq = Ext.getCmp('GridListaufficiSelezionati').getStore();
    var lstr = '';
    storeGridLiq.each(function(storeGridLiq) {
        lstr += storeGridLiq.data.CodiceInterno + ',';
    });
    lstr = lstr.substring(0, lstr.length - 1);

    if (Ext.getDom('ufficiDiCompetenza') != null) {
        Ext.getDom('ufficiDiCompetenza').value = lstr;
    }
    

}

function setUtentiUfficiSelezionati() {
    var storeGridUtentiUfficio = Ext.getCmp('GridListaUtentiufficiSelezionati').getStore();
    var lstr = '';
    storeGridUtentiUfficio.each(function(storeGridUtentiUfficio) {
        lstr += storeGridUtentiUfficio.data.Account + ',';
    });
    lstr = lstr.substring(0, lstr.length - 1);

    if (Ext.getDom('utentiDiCompetenza') != null) {
        Ext.getDom('utentiDiCompetenza').value = lstr;
    }

}

    function builFormPanelUfficiCompetenza() {
        var storGrid =    new Ext.data.SimpleStore({
                fields: [],
                data: []
            });

        myFormPanelUfficiCompetenza = new Ext.Panel({
            id: 'myFormPanelUfficiCompetenza',
            bodyStyle: 'background-color:white;',        
            border: false,
            tbar:[actionAddUfficio],
            items: [buildgridUffici(storGrid)],
            layout: 'column',  
            frame: true     
            });
    }


    function builFormPanelUtentiUfficiCompetenza() {
        var storGridUtentiUffici =    new Ext.data.SimpleStore({
            fields: [],
            data: []
        });
        myFormPanelUtentiUfficiCompetenza = new Ext.Panel({
            id: 'myFormPanelUtentiUfficiCompetenza',
            bodyStyle: 'background-color:white;',        
            border: false,    
            tbar:[actionAddUtente],   
            items: [buildgridUtentiUffici(storGridUtentiUffici)],
            layout: 'column',  
            frame: true     
            });
    }

    function builFormPanelUfficiCompetenzaRegistrati(storeCompetenzaRegistrato, enableActions) {
        if (Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati') == undefined) {
            var griglia = buildgridUfficiSelezionati(storeCompetenzaRegistrato);
            myFormPanelUfficiCompetenzaRegistrati = new Ext.Panel({
                border: false,
                id: 'myFormPanelUfficiCompetenzaRegistrati',
                bodyStyle: 'background-color:white;',
                items: [{id: "ufficiDiCompetenza",xtype: "hidden"},
                        griglia],
                layout: 'column',
                tbar: [actionDeleteUfficio],
                frame: true
            });

        }
        if (enableActions != undefined && enableActions != null) {
            if (enableActions)
                actionDeleteUfficio.enable();
            else
                actionDeleteUfficio.disable();
        }
    } // fine builFormPanelUfficiCompetenzaRegistrati
        
      function builFormPanelUtentiUfficiCompetenzaRegistrati(storeUtentiCompetenzaRegistrato, enableActions) {
        if (Ext.getCmp('myFormPanelUtentiUfficiCompetenzaRegistrati') == undefined) {
            var griglia = buildgridUtentiUfficiSelezionati(storeUtentiCompetenzaRegistrato);
            myFormPanelUtentiUfficiCompetenzaRegistrati = new Ext.Panel({
                border: false,
                id: 'myFormPanelUtentiUfficiCompetenzaRegistrati',
                bodyStyle: 'background-color:white;',
                items: [{id: "utentiDiCompetenza",xtype: "hidden"
                    },griglia],
                layout: 'column',
                tbar: [actionDeleteUtenteUfficio],
                frame: true
            });

        }
        if (enableActions != undefined && enableActions != null) {
            if (enableActions)
                actionDeleteUtenteUfficio.enable();
            else
                actionDeleteUtenteUfficio.disable();
        }
    } // fine builFormPanelUtentiUfficiCompetenzaRegistrati    
        



var myPanelFiltroDipartimento = new Ext.Panel({
    //        xtype: "form",
    xtype: "panel",
    border: false,
        id: 'myPanelFiltroDipartimento',
        frame: true,
    labelAlign: 'left',
//    title: "Dipartimento",
    buttonAlign: "center",
    bodyStyle: 'padding:0px;',
    collapsible: false,
    width: 766,
    autoHeight: true,
    layout: 'table',
    layoutConfig: {
        columns: 4
    },
    items: [{xtype: 'label',
                    style: 'padding-right:50px',
                    text: ''
             }, {
                xtype: 'label',
                id: 'dipartimentoLabel',
                name: 'dipartimentoLabel',
                html: "1. Dipartimento: "
            },{
                xtype: 'label',
                style: 'padding-right:15px',
                text: ''
            }, buildComboDipartimenti()
          ]


        });




function buildComboDipartimenti() {
    if(Ext.getCmp('ComboDipartimenti')== undefined ){
        var proxy = new Ext.data.HttpProxy({
            url: 'ProcAmm.svc/GetTuttiDipartimenti',
            method: 'GET'
        });
        var reader = new Ext.data.JsonReader({

        root: 'GetTuttiDipartimentiResult',
            fields: [
               { name: 'CodiceInterno' },
               { name: 'DescrizioneBreve' },
               { name: 'DescrizioneToDisplay' }               
               ]
        });

        var store = new Ext.data.Store({
            proxy: proxy,
            reader: reader
        });

        var isPerNotifica = true;
        var parametri = { isPerNotifica: isPerNotifica };

        store.load({ params: parametri });
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
            displayField: 'DescrizioneToDisplay',
            valueField: 'CodiceInterno',
            id: 'ComboDipartimenti',
            mode: 'local',
            listWidth: 500,
            width: 500,
            triggerAction: 'all',
            store: store,
            emptyText: 'Selezionare un dipartimento...'
        });
//        Ext.getCmp('myPanelFiltroDipartimento').add(Ext.getCmp('ComboDipartimenti'));  
       
        ComboDipartimenti.on('select', function(record, index) {
            var codDip = ComboDipartimenti.store.data.get(ComboDipartimenti.selectedIndex).data.CodiceInterno;
            updateListaUfficiItemSelctor(codDip);
            // stef
            
            Ext.getCmp('myPanelFiltroDipartimento').doLayout();
            Ext.getCmp('GridListaufficiSelezionati').getView().refresh();
        });
        return ComboDipartimenti;
    }
    
}

function updateListaUfficiItemSelctor(dipartimentoScelto) {
 var proxy = new Ext.data.HttpProxy({
 url: 'ProcAmm.svc/GetTuttiUffici',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

    root: 'GetTuttiUfficiResult',
        fields: [
            { name: 'CodiceInterno' },
            { name: 'DescrizioneBreve' },
            { name: 'DescrizioneToDisplay' }            
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

    var griglia = buildgridUffici(store);
//    return buildgridUffici(store)
 
    Ext.getCmp('myFormPanelUfficiCompetenza').add(griglia);
    Ext.getCmp('myFormPanelUfficiCompetenza').doLayout();
}


function updateListaUtentiUfficiItemSelctor(ufficioScelto) {
 var proxy = new Ext.data.HttpProxy({
 url: 'ProcAmm.svc/GetTuttiDipendentiUfficio',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

    root: 'GetTuttiDipendentiUfficioResult',
        fields: [
            { name: 'Account' },
            { name: 'Denominazione' },          
            { name: 'CodicePubblicoUfficio' },
            { name: 'CodiceInternoUfficio' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
        , reader: reader
    });

    store.setDefaultSort("Denominazione", "ASC");

    var parametri = { ufficioScelto: ufficioScelto };
    store.load({ params: parametri });
    store.on({
        'load': {
            fn: function(store, records, options) {
            mask.hide();
            },
            scope: this
        }
    });

    var griglia = buildgridUtentiUffici(store);
//    return buildgridUffici(store)
 
    Ext.getCmp('myFormPanelUtentiUfficiCompetenza').add(griglia);
    Ext.getCmp('myFormPanelUtentiUfficiCompetenza').doLayout();
}

function buildgridUffici(store) {

    
    
    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "CodiceInterno", width: 60, dataIndex: 'CodiceInterno', id: 'Bilancio',  hidden: true },
	                { header: "Uffici Disponibili", width: 340, dataIndex: 'DescrizioneToDisplay', sortable: true, locked: true }
	               ]);
        if (Ext.getCmp('GridListauffici') != undefined) {
            Ext.getCmp('GridListauffici').destroy();
        }
       var gridUffici = new Ext.grid.GridPanel({
        id: 'GridListauffici',        
       // da mettere sempre
        height: 125,
        //width:360,	
        title: '',
        border: false,               
        cm: ColumnModel,
        stripeRows: true,
        ds:store,
        loadMask: true,
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: true,
        ddGroup: 'gridDDGroup',
        // fine istruzioni per abilitazione Drag & Drop	
	    viewConfig: {
                emptyText: "Nessun ufficio disponibile.",
                deferEmptyText: false,
                forceFit: true
        },	          
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
                    updateListaUtentiUfficiItemSelctor(rec.data.CodiceInterno);
                    
                    Ext.getCmp('GridListauffici').store.remove(rec);                    
                    Ext.getCmp('GridListaufficiSelezionati').getView().refresh();
                } 
            }
		, scope: this
        }
    });
    
    
    return gridUffici;
    //Ext.getCmp('myFormPanelUfficiCompetenza').render('FormPanelUfficiCompetenza')

}

function buildgridUtentiUffici(store) {    
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "Account", width: 60, dataIndex: 'Account',  id: 'AccountUtente',  hidden: true },
	                { header: "Utenti disponibili", width: 340, dataIndex: 'Denominazione', id:'DenominazioneUtente', sortable: true, locked: true },
	                { header: "CodicePubblicoUfficio", width: 340, dataIndex: 'CodicePubblicoUfficio', id:'CodicePubblicoUfficio', hidden: true },
	                { header: "CodiceInternoUfficio", width: 340, dataIndex: 'CodiceInternoUfficio', id:'CodiceInternoUfficio', hidden: true }
	               ]);
        if (Ext.getCmp('GridListaUtentiuffici') != undefined) {
            Ext.getCmp('GridListaUtentiuffici').destroy();
        }
       var gridUtentiUffici = new Ext.grid.GridPanel({
        id: 'GridListaUtentiuffici',
       // da mettere sempre
        height: 125,
        //width:360,	
        title: '',
        border: false,        
        viewConfig: { forceFit: true },
        cm: ColumnModel,
        stripeRows: true,
        ds:store,
        loadMask: true,
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: true,
        ddGroup: 'gridDDGroup',
        // fine istruzioni per abilitazione Drag & Drop	
	    viewConfig: {
                emptyText: "Nessun utente disponibile.",
                deferEmptyText: false
        },	          
        sm: new Ext.grid.RowSelectionModel({
            singleSelect: true,
            listeners: {
                rowselect: function(sm, row, rec) {
                 
                }
            }
        })
    });

    //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
    gridUtentiUffici.addListener({
        'rowdblclick': {
            fn: function(gridUtentiUffici, rowIndex, event) {
                var utente = gridUtentiUffici.store.getAt(rowIndex);

                //if (!VerificaEsistenzaRecordUtentiUfficio(utente)) {
                    // CARICO IL RECORD NELLA FORM
                     Ext.MessageBox.show({
                            title: 'Attenzione',
                            msg: "Si vuole notificare solo alla persona selezionata?<br><br>Cliccare NO per inviare una notifica anche al suo ufficio",
                            buttons: Ext.MessageBox.YESNO,
                            icon: Ext.MessageBox.WARNING,
                            fn: function(btn) {
                                    var storeGridUfficiSelezionati = Ext.getCmp('GridListaufficiSelezionati').getStore();
                                    if (btn == 'yes') {     
                                        Ext.each(storeGridUfficiSelezionati.data.items, function(ufficio) {
                                            if (utente.data.CodiceInternoUfficio == ufficio.data.CodiceInterno) {  
                                                Ext.getCmp('GridListaufficiSelezionati').store.remove(ufficio);
                                                Ext.getCmp('GridListauffici').getStore().add(ufficio); 
                                            }
                                        });         
                                    } else if (btn == 'no') {
                                        var storeGridUffici = Ext.getCmp('GridListauffici').getStore();                
                                        Ext.each(storeGridUffici.data.items, function(ufficio) {
                                            if (utente.data.CodiceInternoUfficio == ufficio.data.CodiceInterno) {  
                                                Ext.getCmp('GridListauffici').store.remove(ufficio);
                                                Ext.getCmp('GridListaufficiSelezionati').getStore().add(ufficio); 
                                            }
                                        });
                                    } 
                                    if (!VerificaEsistenzaRecordUtentiUfficio(utente)) { 
                                        Ext.getCmp('GridListaUtentiuffici').store.remove(utente);
                                        Ext.getCmp('GridListaUtentiufficiSelezionati').getStore().add(utente);  
                                        Ext.getCmp('GridListaUtentiufficiSelezionati').getView().refresh();
                                    }                                                             
                                }
                    });
                //} 
            }
		, scope: this
        }
    });
    
    
    return gridUtentiUffici;
}





function buildgridUfficiSelezionati(toStore) {  
    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "CodiceInterno", width: 60, dataIndex: 'CodiceInterno', id: 'Bilancio', sortable: true, hidden: true },
	                { header: "Uffici Selezionati", width: 340, dataIndex: 'DescrizioneToDisplay', sortable: true, locked: true }
	               ]);
    if (Ext.getCmp('GridListaufficiSelezionati') != undefined) {
        Ext.getCmp('GridListaufficiSelezionati').destroy();
    }
    var gridUfficiSelezionati = new Ext.grid.GridPanel({
    id: 'GridListaufficiSelezionati',
        // da mettere sempre
        height: 125,
        //width: 360,
        title: '',
        layout: 'table',
        border: true,
        viewConfig: { forceFit: true },
        ds: toStore,
        cm: ColumnModel,
        stripeRows: true,
        loadMask: true,
	    viewConfig: {
                emptyText: "Nessun ufficio selezionato.",
                deferEmptyText: false
        },
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: false,
        ddGroup: 'gridDDGroup'

    });
    
    return gridUfficiSelezionati;

}

function buildgridUtentiUfficiSelezionati(toStore) {  
    var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "Account", width: 60, dataIndex: 'Account', id: 'AccountUtente', sortable: true, hidden: true },
	                { header: "Utenti Selezionati", width: 340, dataIndex: 'Denominazione', id:'DenominazioneUtente', sortable: true, locked: true },
	                { header: "CodicePubblicoUfficio", width: 340, dataIndex: 'CodicePubblicoUfficio', id:'CodicePubblicoUfficio', hidden: true },
	                { header: "CodiceInternoUfficio", width: 340, dataIndex: 'CodiceInternoUfficio', id:'CodiceInternoUfficio', hidden: true }
	               ]);
    if (Ext.getCmp('GridListaUtentiufficiSelezionati') != undefined) {
        Ext.getCmp('GridListaUtentiufficiSelezionati').destroy();
    }
    var gridListaUtentiufficiSelezionati = new Ext.grid.GridPanel({
    id: 'GridListaUtentiufficiSelezionati',
        // da mettere sempre
        height: 125,
        //width: 360,
        title: '',
        layout: 'table',
        autoScoll: true,
        border: true,
        viewConfig: { forceFit: false },
        ds: toStore,
        cm: ColumnModel,
        stripeRows: true,
        loadMask: true,
	    viewConfig: {
                emptyText: "Nessun utente selezionato.",
                deferEmptyText: false
        },
        // istruzioni per abilitazione Drag & Drop		          
        enableDragDrop: false,
        ddGroup: 'gridDDGroup'

    });
    
    
     
    return gridListaUtentiufficiSelezionati;

}

function buildCopyDrop() {

    var formPanelDropTargetEl = Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati').body;
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
                ddSource.grid.store.remove(selectedRecord);
            }
            return (true);
        }
    });

}

function buildCopyDropUtentiUffici() {
    var formPanelDropTargetEl = Ext.getCmp('myFormPanelUtentiUfficiCompetenzaRegistrati').body;
    
    var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
        ddGroup: 'gridDDGroup',
        notifyEnter: function(ddSource, e, data) {
            //EFFETTI VISIVI SUL DRAG & DROP
            Ext.getCmp('myFormPanelUtentiUfficiCompetenzaRegistrati').body.stopFx();
            Ext.getCmp('myFormPanelUtentiUfficiCompetenzaRegistrati').body.highlight();
        },
        notifyDrop: function(ddSource, e, data) {
            // CATTURO IL RECORD SELEZIONATO
            var selectedRecord = ddSource.dragData.selections[0];

            // CARICO IL RECORD NELLA FORM
            if (!VerificaEsistenzaRecordUtentiUfficio(selectedRecord)) {
                Ext.getCmp('GridListaUtentiufficiSelezionati').store.add(selectedRecord);                
                ddSource.grid.store.remove(selectedRecord);
                
                
            }
            return (true);
        }
    });
    
   

}

var MyPrincipalPanel = null

function buildPanelPrincipale() {
    if (Ext.getCmp('MyPrincipalPanel') == undefined) {
    
  panelPreImp=  new Ext.Panel({
      id: 'MyPrincipalPanel',
      border: false,
      bodyStyle: 'background-color:white;',
        width: 766,
        
        buttonAlign: "center",
        autoHeight: true,
        layout: 'column',
        items: [ { columnWidth:.5,items:[myFormPanelUfficiCompetenza, myFormPanelUtentiUfficiCompetenza]},
				 { columnWidth: .5, items: [myFormPanelUfficiCompetenzaRegistrati, myFormPanelUtentiUfficiCompetenzaRegistrati] }
				]
    });
}

return Ext.getCmp('MyPrincipalPanel')
}

							