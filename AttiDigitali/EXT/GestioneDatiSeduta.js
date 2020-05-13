var ADD_LIST_RESULT = { "ok": 0, "exists": 1,
    "error": 2
};

//function my_function() {
//    var totaleRelatori = Ext.getCmp('GridRelatori').store.getRange().length;
//    var selections = Ext.getCmp('GridRelatori').getSelectionModel().getSelections();
//    if (selections.length == 1) {
//        var nomeGruppo = "gruppo" + selections[0].data.Tr_id;
//        var totaleRelatori = Ext.getCmp('GridRelatori').store.getRange().length;
//        var indiceRelatore = -1;
//         for (var i = 0; i < totaleRelatori; i++) {
//            if (Ext.getCmp('GridRelatori').store.data.items[i].data.Tr_id == selections[0].data.Tr_id) {
//                indiceRelatore = i;
//            }
//        }
//        var radios = document.getElementsByName(nomeGruppo);
//        if (radios[0].checked == true) {
//            Ext.getCmp('GridRelatori').store.data.items[indiceRelatore].data.IsPresente = true; 
//        } else if (radios[1].checked == true){
//            Ext.getCmp('GridRelatori').store.data.items[indiceRelatore].data.IsPresente = false;
//        }
//    }
// };

function onChecked(cb) {
    var ok = cb.checked;
    var totaleRelatori = Ext.getCmp('GridRelatori').store.getRange().length;
    var selections = Ext.getCmp('GridRelatori').getSelectionModel().getSelections();
    if (selections.length == 1) {
        selections[0].data.IsPresente = cb.checked;
    }
};

      
    
function buildPanelDataSeduta() {
    var dateTimeNow = new Date();
    var ora = "";
    if (dateTimeNow.getHours().toString().length == 1) {
        ora = "0 " + dateTimeNow.getHours().toString();
    } else {
        ora = dateTimeNow.getHours().toString();
    }
    if (dateTimeNow.getMinutes().toString().length == 1) {
        ora = ora + ":0" + dateTimeNow.getMinutes().toString();
    } else {
    ora = ora + ":" + dateTimeNow.getMinutes().toString();
    }
    
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
                    value: ora,
                    format: 'H:i',
                    altFormats: 'H:i',
                    increment: 5,
                    minValue: '06:00',
                    maxValue: '23:00',
                    width: 120
                }]
    });




    return dataSedutaPanel;
}


    function buildGridRelatori() {
        var maskApp = new Ext.LoadMask(Ext.getBody(), {
            msg: "Recupero Dati..."
        });

        var proxy = new Ext.data.HttpProxy({
            url: 'ProcAmm.svc/GetListaRelatori' + window.location.search,
            method: 'POST',
            timeout: 10000000
        });

        var reader = new Ext.data.JsonReader({
            root: 'GetListaRelatoriResult',
            fields: [
           { name: 'Tr_id' },
	       { name: 'Tr_Cognome' },
           { name: 'Tr_Nome' },
           { name: 'Tr_Ordine_Apparizione' },
           { name: 'Tr_Carica' },
           { name: 'Tr_attivo' },
           { name: 'Tr_dataattivazione' },
           { name: 'Tr_datadisattivazione' },
           { name: 'Tr_IdStruttura' },
           { name: 'IsPresente' }
           ]
        });


        var store = new Ext.data.Store({
            proxy: proxy
		, reader: reader
		, sortInfo: { field: "Tr_Ordine_Apparizione", direction: "ASC" }
		, listeners: {
		    'loadexception': function(proxy, options, response) {
		        maskApp.hide();
		        Ext.MessageBox.show({
		            title: 'Elenco Relatori',
		            msg: "Errore durante il recupero dei relatori:<br>" +
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
                var gridDelibere = Ext.getCmp('GridRelatori');
                if (records.length != 0) {

                }

                maskApp.hide();
//                gridDelibere.getView().refresh();
            },
            scope: this
        }
        });

        maskApp.show();
        var codDocumento = Ext.getDom('codDocumento').value;
        var parametri = { IdDocumento: codDocumento };

        try {
            store.load({ params: parametri});
        } catch (ex) {
            maskApp.hide();
        }

       

        
//        function renderRadioBox(val, meta, record, rowIndex, colIndex, store) {

//            var gruppo = 'gruppo' + record.data.Tr_id;
//            var a = '<input onclick="my_function()" type= "radio" name="' + gruppo + '" ' + (colIndex == 4 ? " checked " : "") + ' />';
//           
//           return a;
//       };

        var checkColumnPresente = new Ext.grid.CheckColumn({
//            header: "Presente",
//            dataIndex: 'IsPresente',
//            align: 'center'
//            //           , readonly: false
           editor: new Ext.form.Checkbox({
//                      style: {
//                   textAlign: 'center',
//               }

        //              ,listeners: {
//                  check: function(checkbox, checked) {
//                  } 
//              }

          })
,          header: 'Indoor?',
            dataIndex: 'IsPresente',
            width: 55,
            locked: true
        });
        
        function renderCheckIsPresente(val, p, record) {
            if (val) {
                return '<input type="checkbox"  checked="checked" onclick="onChecked(this)"/>';
            } else {
            return '<input type="checkbox"  onclick="onChecked(this)"/>'; 
            }
        }
       var ColumnModel = new Ext.grid.ColumnModel([
            { header: "Cognome", width: 120, dataIndex: 'Tr_Cognome', id: 'Cognome', sortable: true, hidden: false },
            { header: "Nome", width: 120, dataIndex: 'Tr_Nome', id: 'Nome', sortable: true },
            { header: "Carica", width: 340, dataIndex: 'Tr_Carica', id: 'Carica', sortable: true },
//                        checkColumnPresente,
                        {header: "Presente", width: 160, dataIndex: 'IsPresente', renderer: renderCheckIsPresente, sortable: true, locked: false }
//            ,{
//            xtype: 'checkcolumn',
//            header: 'Indoor?',
//            dataIndex: 'IsPresente',
//            width: 55
//            }
//            ,
//             { header: "PCF",  sortable: true, width: 40, dataIndex: 'IsPresente',
//                 editor: new Ext.form.Checkbox({
//                     header: "Presente",
//                     dataIndex: 'IsPresente'
//                 })
//             }

       //            checkColumnPresente
       //            ,
//       { header: "PCF",  id: 'IsPresente', sortable: true, width: 40,
//                       editor: new Ext.form.ComboBox({
//                       id: "pcfList",
//                       displayField: 'Id',
//		               valueField: 'Id',
//		                store: new Ext.data.SimpleStore({
//                            fields: ['value', 'description'],
//                            data: storeRicercaData,
//                            autoLoad: true
//                        })
//       		                    header: "Presente",
//                                   dataIndex: 'IsPresente'})
//       		     }
//       		     ,
//                  { header: 'Active',
//                       width: 40,
//                        sortable: true,
//                        dataIndex: 'IsPresente',
//                        editable: true,
//                        field: {
//                            xtype: 'checkcolumn',
//                            editor: new Ext.form.Checkbox({
//                                listeners: {
//                                    check: function(checkbox, checked) {
//                                    }
//                                }

//                            })
//                        }
//                        }
//       ,
       //    {
       //            xtype: 'checkcolumn',
       //            header: "Checked?",
       //            align: 'center',
       //            dataIndex: 'IsPresente',
       //            trueText: '✓',
       //            falseText: ''
       //}
       //            { header: "Presente", width: 80, id: "IsPresente", renderer: renderRadioBox, editor: { xtype: 'radio'} }
       //            ,
       //            { header: "Assente", width: 80, id: "IsAssente", renderer: renderRadioBox, editor: { xtype: 'radio'} }
        	]);

var sm = new Ext.grid.CheckboxSelectionModel({ 
		        singleSelect: true
		        })
var grid = new Ext.grid.EditorGridPanel({
            id: 'GridRelatori',
            autoHeight: true,
            border: true,
            ds: store,
            cm: ColumnModel,
            stripeRows: true,
            bodyStyle: 'background: #EBF3FD',
            style: 'margin-top: 10px',
            clicksToEdit: 1,
            viewConfig: {
                emptyText: "Nessun relatore trovato.",
                deferEmptyText: false
            }
            ,
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
    var messaggioErrore = "";
    if (dataSelezionata.getTime() > now.getTime()) {
        messaggioErrore = "Data selezionata maggiore della data odierna</br>";
        
    }
    
    var gridRelatori = Ext.getCmp('GridRelatori');
    var countPresenti = 0;
    if (gridRelatori != undefined && gridRelatori != null) {
        var storeGridRelatori = gridRelatori.getStore();
        if (storeGridRelatori != undefined && storeGridRelatori != null) {
            storeGridRelatori.each(function(storeGridRelatori) {
                if (storeGridRelatori.data.IsPresente) {
                    countPresenti = countPresenti + 1; 
                }
            });
        }
    }
    if (countPresenti < 3) {
    messaggioErrore = messaggioErrore + "E' necessario che siano presenti almeno 3 relatori</br>";
    }
    if (messaggioErrore != "")
        lstr_errore = { tab_to_activate: null, msg: messaggioErrore };
    
//    if (dataSelezionata.getFullYear() < now.getFullYear()) {
//        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
//    } else if (dataSelezionata.getMonth()< now.getMonth()){        
//        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
//    } else if (dataSelezionata.getDate() < now.getDate()) {        
//        lstr_errore = { tab_to_activate: null, msg: "Data selezionata inferiore alla data odierna" };
//    } else {
//        var oraSeduta = Ext.getCmp('OraSeduta').value;
//        if (oraSeduta != undefined && oraSeduta != null) {
//            var partsOra = oraSeduta.split(':');
//            var oraSeduta = partsOra[0];
//            var minSeduta = partsOra[1];
//            // se è oggi controllo che sia in un orario successivo ad ora
//            if (dataSelezionata.getFullYear() == now.getFullYear() && dataSelezionata.getMonth() == now.getMonth() && dataSelezionata.getDate() == now.getDate()) {
//                if (oraSeduta < now.getHours()) {
//                    lstr_errore = { tab_to_activate: null, msg: "Ora selezionata inferiore alla data odierna" };
//                } else if (oraSeduta < now.getHours() && minSeduta < now.getMinutes()) {
//                    lstr_errore = { tab_to_activate: null, msg: "Ora selezionata inferiore alla data odierna" };
//                } 
//            } 
//        }
//    }

    return lstr_errore;
}

function setRelatori() {
    var gridRelatori = Ext.getCmp('GridRelatori');

    if (gridRelatori != undefined && gridRelatori != null) {
        var storeGridRelatori = gridRelatori.getStore();

        if (storeGridRelatori != undefined && storeGridRelatori != null) {
                var relatoriAsJsonObject = '';

                storeGridRelatori.each(function(storeGridRelatori) {
                relatoriAsJsonObject += Ext.util.JSON.encode(storeGridRelatori.data) + ',';
            });

            relatoriAsJsonObject = relatoriAsJsonObject.substring(0, relatoriAsJsonObject.length - 1);
            Ext.getDom('listaRelatori').value = relatoriAsJsonObject;
        }
    }
}

function registraDatiSedutaFunction() {
    var errore = null;
    errore = verificaForm();
    if (errore == null) {

//        var listaRelatori1 = Ext.getCmp('GridRelatori').store.data.items
        setRelatori();

        Ext.getDom('chkSalva').value = 1;
//        Ext.getCmp('myPanel').getForm().timeout = 100000000;
        var params = { chkSalva: Ext.getDom('chkSalva').value, dataSeduta: Ext.getCmp('DataSeduta').value, oraSeduta: Ext.getCmp('OraSeduta').value, listaRelatori: Ext.getDom('listaRelatori').value };

        Ext.Ajax.request({
            url: 'DatiSeduta.aspx' + window.location.search,
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

}



function buildPanelPrincipale(itemDatiSedutaInfo) {
 
    var actionRegistraODG = new Ext.Action({
        text: 'Registra Dati Seduta',
        tooltip: 'Registra Dati Seduta',
        handler: function() {
            registraDatiSedutaFunction();
        },
        iconCls: 'save'
    });

    
    var gestioneODG = new Ext.FormPanel({
        id: 'panelGestioneODG',
        url: 'GestioneODG.aspx' + window.location.search,
        labelAlign: 'top',
        border: false,
        tbar: [actionRegistraODG],
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
                buildPanelDataSeduta()
                , buildGridRelatori()
                ]
            }
            ,{ id: "chkSalva",
                xtype: "hidden"
            }, { id: "listaRelatori",
                xtype: "hidden"
            }]           
    });

    if (itemDatiSedutaInfo != null && itemDatiSedutaInfo != undefined)
        setDatiSedutaRelatoriOnLoad(itemDatiSedutaInfo);

    return gestioneODG;
}


function setDatiSedutaRelatoriOnLoad(itemDatiSedutaInfo) {
    if (itemDatiSedutaInfo != null) {
        Ext.getCmp('DataSeduta').value = itemDatiSedutaInfo.DataSeduta;
        Ext.getCmp('OraSeduta').value = itemDatiSedutaInfo.OraSeduta;
        
        var codDocumento = Ext.getDom('codDocumento').value;
        var parametri = { IdDocumento: codDocumento };
        Ext.getCmp('GridRelatori').getStore().load({ params: parametri});
//        Ext.getCmp('OraSeduta').value = "15:2";

//        for (var i in itemDatiSedutaInfo.Relatori) {
//            var relatore = itemDatiSedutaInfo.Relatori[i];
//            var nomeGruppo = "gruppo" + relatore.Id;
//            var radios = document.getElementsByName(nomeGruppo);
//            if (relatore.IsPresente) {
//                radios[0].checked = true;
//                radios[1].checked = false;
//            } else {
//                radios[0].checked = false;
//                radios[1].checked = true;
//            }
           
//            var radios = document.getElementsByName(nomeGruppo);
//            if (radios[0].checked == true) {
//                Ext.getCmp('GridRelatori').store.data.items[indiceRelatore].data.IsPresente = true;
//            } else if (radios[1].checked == true) {
//                Ext.getCmp('GridRelatori').store.data.items[indiceRelatore].data.IsPresente = false;
//            }
//        }
//        { 
//            var relatore = itemDatiSedutaInfo.Relatori[i];
//         }
         //        for (var i = 0; i < itemDatiSedutaInfo.Relatori.length; i++) {
//            if (Ext.getCmp('GridRelatori').store.data.items[i].data.Tr_id == selections[0].data.Tr_id) {
//                indiceRelatore = i;
//            }
//        }
//        if (!schedaLeggeTrasparenzaInfo.AutorizzazionePubblicazione) {
//            Ext.getCmp('noPubblicazione').checked = true;
//            Ext.getCmp('noPubblicazione').fireEvent('check');
//            Ext.getCmp('yesPubblicazione').checked = false;
//            Ext.getCmp('yesPubblicazione').fireEvent('check');
//            Ext.getCmp('noteAutorizzazionePubblicazione').setValue(schedaLeggeTrasparenzaInfo.NotePubblicazione);
//        } else {
//            Ext.getCmp('noPubblicazione').checked = false;
//            Ext.getCmp('noPubblicazione').fireEvent('check');
//            Ext.getCmp('yesPubblicazione').checked = true;
//            Ext.getCmp('yesPubblicazione').fireEvent('check');

//            var dati = null;
//            var normaAttribuzioneBeneficio = schedaLeggeTrasparenzaInfo.NormaAttribuzioneBeneficio;
//            if ((dati = parseNumeroDetermina(normaAttribuzioneBeneficio, "Determina n. ", "")) != null) {
//                Ext.getCmp('DETERMINA').checked = true;
//                Ext.getCmp('DETERMINA').fireEvent('check');
//                Ext.getCmp('DELIBERA').checked = false;
//                Ext.getCmp('DELIBERA').fireEvent('check');
//                Ext.getCmp('ALTRO').checked = false;
//                Ext.getCmp('ALTRO').fireEvent('check');

//                getDetermina(dati.ufficio, dati.anno, dati.numero, getDeterminaOnLoad);
//            } else if ((dati = parseNumeroDelibera(normaAttribuzioneBeneficio, "Delibera n. ", "")) != null) {
//                Ext.getCmp('DETERMINA').checked = false;
//                Ext.getCmp('DETERMINA').fireEvent('check');
//                Ext.getCmp('DELIBERA').checked = true;
//                Ext.getCmp('DELIBERA').fireEvent('check');
//                Ext.getCmp('ALTRO').checked = false;
//                Ext.getCmp('ALTRO').fireEvent('check');

//                getDelibera(dati.anno, dati.numero, getDeliberaOnLoad);
//            } else {
//                Ext.getCmp('DETERMINA').checked = false;
//                Ext.getCmp('DETERMINA').fireEvent('check');
//                Ext.getCmp('DELIBERA').checked = false;
//                Ext.getCmp('DELIBERA').fireEvent('check');
//                Ext.getCmp('ALTRO').checked = true;
//                Ext.getCmp('ALTRO').fireEvent('check');

//                setDatiAltraNorma(normaAttribuzioneBeneficio);
//            }

//            if (schedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento == "") {
//                Ext.getCmp('ufficioResponsabileDelProcedimento').setValue(Ext.get('descrizioneUffProp').getValue());
//            } else {
//                Ext.getCmp('ufficioResponsabileDelProcedimento').setValue(schedaLeggeTrasparenzaInfo.UfficioResponsabileProcedimento);
//            }

//            if (schedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento == "") {
//                Ext.getCmp('funzionarioResponsabileDelProcedimento').setValue(Ext.get('responsabileUffProp').getValue());
//            } else {
//                Ext.getCmp('funzionarioResponsabileDelProcedimento').setValue(schedaLeggeTrasparenzaInfo.FunzionarioResponsabileProcedimento);
//            }

//            Ext.getCmp('modalitaIndividuazioneBeneficiario').setValue(schedaLeggeTrasparenzaInfo.ModalitaIndividuazioneBeneficiario);
//        }

//        //init contratti grid
//        enableDatiContratto(true);
//        if (schedaLeggeTrasparenzaInfo.Contratti != undefined && schedaLeggeTrasparenzaInfo.Contratti != null) {
//            var contratti = schedaLeggeTrasparenzaInfo.Contratti;

//            for (var i = 0; i < contratti.length; i++) {
//                var contratto = contratti[i];
//                if (contratto != undefined && contratto != null)
//                    addContratto(contratto.NumeroRepertorio, contratto.Oggetto, contratto.Id);
//            }
//        }
//        enableContenutoAtto(true);
//        if (schedaLeggeTrasparenzaInfo.ContenutoAtto != undefined && schedaLeggeTrasparenzaInfo.ContenutoAtto != null) {
//            Ext.getCmp('contenutoAtto').setValue(schedaLeggeTrasparenzaInfo.ContenutoAtto);
//        }
   }
}



function isNullOrEmpty(value) {
    return (value == null || value == undefined || value == "")
}