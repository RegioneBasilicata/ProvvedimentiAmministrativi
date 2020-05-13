var ADD_LIST_RESULT = { "ok": 0, "exists": 1,
    "error": 2
};

function buildPopupListaDelibere(gridPanelId) {
    var currentGridPanelId = gridPanelId;

    var popup = new Ext.Window({
        title: 'Delibere',
        width: 850,
        height: 550,
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        resizable: false,
        //buttonAlign: 'center',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        autoScroll: true,
        closable: true,
        buttons: [{
            text: 'Aggiungi delibera alla lista',
            id: 'btnSelezionaDel'
        },
         {
             text: 'Chiudi',
             id: 'btnChiudiSelezione'
        }]
    });

    popup.on('close', function() {
        popup.close();
    });

    var panelDel = buildPanelDocumentiWL();
    popup.add(panelDel);

    popup.doLayout();
    popup.show();


    Ext.getCmp('btnChiudiSelezione').on('click', function() {
        popup.close();
    });

    Ext.getCmp('btnSelezionaDel').on('click', function() {

    var delibere = Ext.getCmp('GridDocumentiWL').getSelectionModel().getSelections();
    if (delibere != undefined && delibere != null) {
        var addStatus = ADD_LIST_RESULT.ok;
        var delibereEsistenti = "";
        var delibereSeparator = "'";
        for (var i = 0; addStatus != ADD_LIST_RESULT.error && i < delibere.length; i++) {
            var delibera = delibere[i].data;
            if (delibera != undefined && delibera != null) {
                var currentAddStatus = addDelibera(delibera.Doc_Id, delibera.Doc_NumeroProvvisorio, delibera.Doc_Oggetto, delibera.Doc_Cod_Uff_Prop, delibera.Doc_Descrizione_ufficio, delibera.Doc_Data);
                if (currentAddStatus == ADD_LIST_RESULT.error)
                    addStatus = currentAddStatus;
                else if (addStatus != ADD_LIST_RESULT.exists && currentAddStatus != addStatus)
                    addStatus = currentAddStatus;

                if (currentAddStatus == ADD_LIST_RESULT.exists) {
                    delibereEsistenti = delibereEsistenti + delibereSeparator + delibera.Doc_NumeroProvvisorio + "'";
                    delibereSeparator = ", '";
                }
            }
        }

        var msg = 'Impossibile aggiungere tutti o parte delle delibere selezionate.';
        if (addStatus == ADD_LIST_RESULT.exists)
            msg = 'Una o più delibere selezionate sono già presenti nella lista: ' + delibereEsistenti + '. Sono stati aggiunti solo quelle eventualmente non presenti.';
        else if (addStatus == ADD_LIST_RESULT.ok)
            msg = 'Operazione completata con successo.';

        Ext.MessageBox.show({
            title: 'Aggiungi Delibera/e',
            msg: msg,
            buttons: Ext.MessageBox.OK,
            icon: (addStatus == ADD_LIST_RESULT.ok) ? Ext.MessageBox.INFO : ((addStatus == ADD_LIST_RESULT.exists) ? Ext.MessageBox.WARNING : Ext.MessageBox.ERROR),
            fn: function(btn) {
            }
        });
    }
    });
}

function addDelibera(id, numeroProvvisorio, oggetto, codUff, descizioneUfficio, data) {
    var retValue = ADD_LIST_RESULT.error;

    var gridDelibere = Ext.getCmp('GridDelibere');
    if (gridDelibere != undefined && gridDelibere != null) {
        var gridDelibereStore = gridDelibere.store;
        if (gridDelibereStore != undefined && gridDelibereStore != null) {
            if (!isNullOrEmpty(id)) {
                var recordAlreadyExists = gridDelibereStore.queryBy(
                    function(record, recordId) {
                        return (record.data.Doc_Id == id)
                    });

                if (!(recordAlreadyExists != undefined && recordAlreadyExists.length > 0)) {
                    DeliberaRecordType = Ext.data.Record.create(['Doc_Id', 'Doc_NumeroProvvisorio', 'Doc_Oggetto', 'Doc_Cod_Uff_Prop', 'Doc_Descrizione_ufficio', 'Doc_Data']);
                   
                    var newRecordDelibera = new DeliberaRecordType({
                          Doc_Id: id
                        , Doc_NumeroProvvisorio: numeroProvvisorio
                        , Doc_Oggetto: oggetto
                        , Doc_Cod_Uff_Prop: codUff
                        , Doc_Descrizione_ufficio: descizioneUfficio
                        , Doc_Data: data
                    });

                    gridDelibereStore.add(newRecordDelibera);
                    retValue = ADD_LIST_RESULT.ok;
                }
                else
                    retValue = ADD_LIST_RESULT.exists;
            }
        }
    }

    return retValue;
}

function buildPanelDataSeduta() {
    
    var dataSedutaPanel = new Ext.Panel({
        id: 'panelDataSeduta',
        layout: 'table',
        layoutConfig: {
            columns: 4
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cn: { tag: 'tbody'} }
        },
        
        plain: true,
        border: true,
        bodyStyle: 'padding:10px; background: #EBF3FD; text-align: center',
        items: [{ xtype: 'label',
                    text: 'Seduta del: ',
                    id: 'DataSedutalbl',
                    style: "margin-right: 5px"
                }, { xtype: 'datefield',
                    fieldLabel: 'Seduta del',
                    name: 'DataSeduta',
                    id: 'DataSeduta',
                    format: 'd-m-Y',
                    altFormats: 'd/m/Y',
                    width: 120,
                    value: new Date()                    
                }, { xtype: 'label',
                    text: 'Alle ore: ',
                    id: 'OraSedutalbl',
                    style: "margin-left: 15px; margin-right: 5px"
                }, {
                    fieldLabel: 'Alle ore',
                    name: 'OraSeduta',
                    xtype: 'timefield',
                    id: 'OraSeduta',
                    format: 'H:i',
                    altFormats: 'H:i',
                    increment: 5,
                    minValue: '09:00',
                    maxValue: '18:00',
                    width: 120
                }]
    });

    return dataSedutaPanel;
}



    function buildPanelDocumentiSeduta(CF, PI, DE, CS, IC, IB, fnOnStoreBeneficiariLoad, fnOnSelectBeneficiario, resultSearchGridHeight) {

        var sm = new Ext.grid.CheckboxSelectionModel({
            singleSelect: false,
            loadMask: true,
            listeners: {
                rowselect: function(selectionModel, rowIndex, record) {
                    var totalRows = Ext.getCmp('GridDelibere').store.getRange().length;
                    var selectedRows = selectionModel.getSelections();
                    if (selectedRows.length > 0) {
//                        rimuoviContratto.enable();
                    }
                    if (totalRows == selectedRows.length) {
                        var view = Ext.getCmp('GridDelibere').getView();
                        var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                        chkdiv.addClass("x-grid3-hd-checker-on");
                    }
                },
                rowdeselect: function(selectionModel, rowIndex, record) {
                    var selectedRows = selectionModel.getSelections();
                    if (selectedRows.length == 0) {
//                        rimuoviContratto.disable();
                    }
                    var view = Ext.getCmp('GridDelibere').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.removeClass('x-grid3-hd-checker-on');
                }
            }
        });

 
        var store = new Ext.data.SimpleStore({
            fields: ['Doc_Id', 'Doc_NumeroProvvisorio', 'Doc_Oggetto', 'Doc_Data']
        });


        function renderNumeroProvvisorio(val, p, record) {
            return "<a href='AggiungiAllAlberoAction.aspx?key=" + record.data.Doc_Id + "'>" + record.data.Doc_NumeroProvvisorio + "</a>";
        }

        var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            { header: "Numero Provvisorio", width: 80, dataIndex: 'Doc_NumeroProvvisorio', renderer: renderNumeroProvvisorio, id: 'NumeroProvvisorio', sortable: true, hidden: false },
            { header: "Data", width: 85, dataIndex: 'Doc_Data', sortable: true },
            { header: "Oggetto", width: 580, dataIndex: 'Doc_Oggetto', id: 'Oggetto', sortable: true }
        	]);

        var actionAggiungiDocSeduta = new Ext.Action({
            text: 'Aggiungi delibera',
            tooltip: 'Aggiungi delibera',
            handler: function() {
                buildPopupListaDelibere("GridDelibere")
            },
            iconCls: 'add'

        });

        var actionEliminaDocSeduta = new Ext.Action({
            text: 'Elimina delibera',
            tooltip: 'Elimina delibera',
            handler: function() {
                var gridDelibere = Ext.getCmp('GridDelibere');
                var storeGridDelibere = gridDelibere.getStore();
                var selections = gridDelibere.getSelectionModel().getSelections();

                Ext.MessageBox.buttonText.cancel = "Annulla";

                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'della delibera selezionata' : 'delle delibere selezionate') + '?',
                    buttons: Ext.MessageBox.OKCANCEL,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(btn) {
                        if (btn == 'ok') {
                                Ext.each(selections, function(rec) {
                                    storeGridDelibere.remove(rec);
                                }); 
                        }
                    }
                });
            },
            iconCls: 'remove'
        });

    
        var grid = new Ext.grid.GridPanel({
            id: 'GridDelibere',
            autoHeight: true,
            border: true,
            tbar: [actionAggiungiDocSeduta, actionEliminaDocSeduta],
            viewConfig: { forceFit: true },
            ds: store,
            cm: ColumnModel,
            stripeRows: true,
            bodyStyle: 'background: #EBF3FD',
            style: 'margin-top: 10px',
            viewConfig: {
                emptyText: "Nessun documento nella propria lista lavoro.",
                deferEmptyText: false
            },
            sm: sm
        });


        grid.on('render', function() {
            this.getView().mainBody.on('mousedown', function(e, t) {
                if (t.tagName == 'A') {
                    e.stopEvent();
                    t.click();
                }
            });
        }, grid);

        

        return grid;
    }


    function buildPanelDocumentiWL(CF, PI, DE, CS, IC, IB, fnOnStoreBeneficiariLoad, fnOnSelectBeneficiario, resultSearchGridHeight) {

        var maskApp = new Ext.LoadMask(Ext.getBody(), {
            msg: "Recupero Dati..."
        });

        var proxy = new Ext.data.HttpProxy({
            url: 'ProcAmm.svc/GetListaDelibereWL' + window.location.search,
            method: 'POST',
            timeout: 10000000
        });

        var reader = new Ext.data.JsonReader({
            root: 'GetListaDelibereWLResult',
            fields: [
           { name: 'Doc_Id' },
	       { name: 'Doc_NumeroProvvisorio' },
           { name: 'Doc_Oggetto' },
           { name: 'Doc_Cod_Uff_Prop' },
           { name: 'Doc_Descrizione_ufficio' },
           { name: 'Doc_Data' }
           ]
        });

        var store = new Ext.data.Store({
            proxy: proxy
		, reader: reader
		, sortInfo: { field: "Doc_NumeroProvvisorio", direction: "ASC" }
		, listeners: {
		    'loadexception': function(proxy, options, response) {
		        maskApp.hide();
		        Ext.MessageBox.show({
		            title: 'Elenco Delibere',
		            msg: "Errore durante il recupero dell'elenco delle delibere:<br>" +
                         "'" + Ext.decode(response.responseText).FaultMessage + "'",
		            buttons: Ext.Msg.OK,
		            closable: false,
		            icon: Ext.MessageBox.ERROR
		        });
		    }
		}
        });

        store.on({ 'load': {
            fn: function(store, records, options) {
                var gridDelibere = Ext.getCmp('GridDocumentiWL');
                if (records.length != 0) {
                  
                }

                maskApp.hide();
                gridDelibere.getView().refresh();
            },
            scope: this
        }
        });

        maskApp.show();

        var parametri = { codFiscale: CF, pIva: PI, denominazione: DE, idAnagrafica: CS, idContratto: IC, idBeneficiari: IB };

        try {
            store.load({});
        } catch (ex) {
            maskApp.hide();
        }

        var sm = new Ext.grid.CheckboxSelectionModel({
            singleSelect: false,
            loadMask: true
        });

        var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            { header: "Numero Provvisorio", width: 80, dataIndex: 'Doc_NumeroProvvisorio', id: 'NumeroProvvisorio', sortable: true, hidden: false },
            { header: "Data", width: 85, dataIndex: 'Doc_Data', sortable: true },
            { header: "Oggetto", width: 580, dataIndex: 'Doc_Oggetto', id: 'Oggetto', sortable: true }
        	]);

        

        
        var grid = new Ext.grid.GridPanel({
        id: 'GridDocumentiWL',
            autoHeight: true,
            border: true,
            viewConfig: { forceFit: true },
            ds: store,
            cm: ColumnModel,
            stripeRows: true,
            bodyStyle: 'background: #EBF3FD',
            style: 'margin-top: 10px',
            viewConfig: {
                emptyText: "Nessun documento nella propria lista lavoro.",
                deferEmptyText: false
            },
            sm: sm
        });

        grid.on('render', function() {
            this.getView().mainBody.on('mousedown', function(e, t) {
                if (t.tagName == 'A') {
                    e.stopEvent();
                    t.click();
                }
            });
        }, grid);



        return grid;
    }    




function verificaForm() {
    var lstr_errore = null;

    var parts = (Ext.getCmp('DataSeduta').value).split('-');
    var dataSelezionata = new Date(parts[2], parts[1] - 1, parts[0]);
    var now = new Date();
    if (dataSelezionata.getFullYear() < now.getFullYear()) {
        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
    } else if (dataSelezionata.getMonth()< now.getMonth()){        
        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
    } else if (dataSelezionata.getDate() < now.getDate()) {        
        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
    } else {
        var oraSeduta = Ext.getCmp('OraSeduta').value;
        if (oraSeduta != undefined && oraSeduta != null) {
            var partsOra = oraSeduta.split(':');
            var oraSeduta = partsOra[0];
            var minSeduta = partsOra[1];
            // se è oggi controllo che sia in un orario successivo ad ora
            if (dataSelezionata.getFullYear() == now.getFullYear() && dataSelezionata.getMonth() == now.getMonth() && dataSelezionata.getDate() == now.getDate()) {
                if (oraSeduta < now.getHours()) {
                    lstr_errore = { tab_to_activate: null, msg: "Ora selezionata inferiore alla data odierna" };
                } else if (oraSeduta < now.getHours() && minSeduta < now.getMinutes()) {
                    lstr_errore = { tab_to_activate: null, msg: "Ora selezionata inferiore alla data odierna" };
                } else {
                    var totaleDelibere = Ext.getCmp('GridDelibere').store.getRange().length;
                    if (totaleDelibere < 1) {
                        lstr_errore = { tab_to_activate: null, msg: "All'ordine del giorno non è stata associata nessuna delibera" };
                    } 
                }
            } else {
                var totaleDelibere = Ext.getCmp('GridDelibere').store.getRange().length;
                if (totaleDelibere < 1) {
                    lstr_errore = { tab_to_activate: null, msg: "All'ordine del giorno non è stata associata nessuna delibera" };
                }
            }
        }
    }
    
   

    return lstr_errore;
}

function setDelibereAssociate() {
    var gridDelibere = Ext.getCmp('GridDelibere');

    if (gridDelibere != undefined && gridDelibere != null) {
        var storeGridDelibere = gridDelibere.getStore();

        if (storeGridDelibere != undefined && storeGridDelibere != null) {
                var delibereAsJsonObject = '';

                storeGridDelibere.each(function(storeGridDelibere) {
                delibereAsJsonObject += Ext.util.JSON.encode(storeGridDelibere.data) + ',';
            });

            delibereAsJsonObject = delibereAsJsonObject.substring(0, delibereAsJsonObject.length - 1);
            Ext.getDom('listaDelibere').value = delibereAsJsonObject;
        }
    }
}

function creaODGFunction() {
    var errore = null;
    errore = verificaForm();
    if (errore == null) {
    
        
        setDelibereAssociate();

        Ext.getDom('chkSalva').value = 1;
//        Ext.getCmp('myPanel').getForm().timeout = 100000000;
        var params = { chkSalva: Ext.getDom('chkSalva').value, dataSeduta: Ext.getCmp('DataSeduta').value, oraSeduta: Ext.getCmp('OraSeduta').value, listaDelibere: Ext.getDom('listaDelibere').value };

        Ext.Ajax.request({
            url: 'GestioneODG.aspx' + window.location.search,
            params: params,
            method: 'POST',
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            success: function(result, response) {
                var decodedResponseText = Ext.decode(result.responseText);
                if (decodedResponseText.success == true) {
                    location.href = decodedResponseText.link
                } else {
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: decodedResponseText.FaultMessage,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) {
                            return;
                        }
                    })
                }
            },
            failure:
                function(result, response) {

                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: 'Errore nel salvataggio. Riprovare',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) {
                            return;
                        }
                    }); //fine messagebox
                } //fine function
        });
    
    } else {
        Ext.MessageBox.show({
            title: 'Errore',
            msg: errore.msg,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.ERROR
        });   
    }
//    var stdec = Ext.lib.Ajax.defaultPostHeader
//    Ext.lib.Ajax.defaultPostHeader = 'application/json';

//        Ext.lib.Ajax.defaultPostHeader = stdec
}



function buildPanelGestioneODG() {
   
    var actionRegistraODG = new Ext.Action({
        text: 'Registra O.d.G.',
        tooltip: 'Registra O.g.G',
        handler: function() {
            creaODGFunction();
        },
        iconCls: 'save'
    });
    
    var gestioneODG = new Ext.FormPanel({
        id: 'panelGestioneODG',
        url: 'GestioneODG.aspx' + window.location.search,
        labelAlign: 'top',
        border: false,
        tbar: [actionRegistraODG],
//        tbar: [actionAggiungiAnagrafica],
        bodyStyle: 'padding:10px',
        height: 450,
        width: 800,
        autoScroll: true,
        items: [
            { xtype: 'panel',
                layout: Ext.isIE ? 'anchor' : 'auto',
                border: false,
                anchor: Ext.isIE ? '-18' : '0',
                autoWidth: Ext.isIE ? false : true,
                items: [
                buildPanelDataSeduta(),
                buildPanelDocumentiSeduta()
                ,{ id: "chkSalva",
                    xtype: "hidden"
                }, { id: "listaDelibere",
                    xtype: "hidden"
                }]
            }
        ]
    });

    return gestioneODG;
}




function isNullOrEmpty(value) {
    return (value == null || value == undefined || value == "")
}