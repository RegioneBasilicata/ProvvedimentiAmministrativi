var isDocumentoConFatture = false;
var hiddenBeneficiario;
var comboModalitaPagamento;
var comboListaBeneficiariImpegno;
var listaBeneficiariImpegno;

var maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


var storePdCF = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetPianoDeiContiFinanziari',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetPianoDeiContiFinanziariResult',
        fields: [
            { name: 'Id' },
            { name: 'Descrizione' }
        ]
    })
});
storePdCF.setDefaultSort("Descrizione", "ASC");





var storeImpegni = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImp',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaImpResult',
        fields: [
          { name: 'NumImpegno' },
          { name: 'ImpDisp' },
          { name: 'TipoAtto' },
          { name: 'DataAtto' },
          { name: 'NumeroAtto' },
          { name: 'Oggetto_Impegno' },
          { name: 'ImpPotenzialePrenotato' },
          { name: 'Codice_Obbiettivo_Gestionale' },
          { name: 'PianoDeiContiFinanziario' },
          { name: 'ListaBeneficiari' },
          { name: 'Beneficiario' }
        ]
    }),
    listeners: {
        beforeload: function (store, options) {
            maskApp.show();
        },
        load: function (store, records, options) {
            Ext.each(records, function (rec) {
                if (rec.data.Beneficiario != null && rec.data.Beneficiario.ID == "") {
                    rec.data.Beneficiario.Denominazione = "<div style='color:orange;display:inline'>NON PRESENTE</div>";
                    rec.data.Beneficiario.ListaSedi = [];
                } else {
                }

            });
            maskApp.hide();
        }
    }
});

storeImpegni.setDefaultSort("NumImpegno", "DESC");

function showGridBeneficiari(recLiquidazione) {
    var liquidazione = { ID: recLiquidazione.data.ID, Capitolo: recLiquidazione.data.Capitolo, Bilancio: recLiquidazione.data.Bilancio, ImpPrenotatoLiq: recLiquidazione.data.ImpPrenotatoLiq };
    var grid = BeneficiariLiquidazioneImpegno(liquidazione, undefined, "GridBeneficiari", "GridLiquidazione");

    Ext.getCmp('myPanelLiq').add(grid);
    Ext.getCmp('myPanelLiq').doLayout();
}

function registraFatturaLiquidazioneNonCont(statoOperazione, idLiquidazione, listaFatture) {

    var parametri = { idLiquidazione: idLiquidazione, statoOperazione: statoOperazione, listaFatture: listaFatture };
    var messaggioRisposta;
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/NotificaAttoFattura' + window.location.search,
        params: Ext.encode(parametri),
        method: 'POST',
        success: function (response, options) {
            var msg = "";
            if (response.statusText == "OK") {
                msg = "Operazione completata con successo";
            } else {
                msg = "Errore Generico";
            }
            Ext.MessageBox.show({
                title: 'Fatture liquidazione',
                msg: msg,
                buttons: Ext.MessageBox.OK
            });

            Ext.each(listaFatture, function (fattura) {

                FatturaRecordType = Ext.data.Record.create(['IdUnivoco', 'IdDocumento', 'Contratto', 'NumeroFatturaBeneficiario', 'DataFatturaBeneficiario', 'AnagraficaInfo', 'ImportoTotaleFattura', 'DescrizioneFattura', 'ImportoFattDaLiquidare', 'Prog']);

                var newRecordFattura = new FatturaRecordType({
                    Contratto: fattura.Contratto,
                    IdDocumento: fattura.IdDocumento,
                    NumeroFatturaBeneficiario: fattura.NumeroFatturaBeneficiario,
                    DataFatturaBeneficiario: fattura.DataFatturaBeneficiario,
                    AnagraficaInfo: fattura.AnagraficaInfo,
                    ImportoTotaleFattura: fattura.ImportoTotaleFattura,
                    DescrizioneFattura: fattura.DescrizioneFattura,
                    IdUnivoco: fattura.IdUnivoco,
                    ImportoLiquidato: fattura.ImportoFattDaLiquidare,
                    Prog: fattura.Prog
                });

                Ext.getCmp('GridFattureLiquidazioneNonCont').store.add(newRecordFattura);
            });

        },
        failure: function (response, result) {
            var data = Ext.decode(response.responseText);

            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });

        }
    });
}

function buildPopUpFattureLiquidazioneNonCont(liquidazione, importoTotaleFattureLiq) {

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaFattureNonAssegnateLiquidazioneOrResidue' + window.location.search,
        method: 'POST',
        timeout: 20000000,
        failure: function (response, result) {
            maskApp.hide();
            var data = Ext.decode(response.responseText);

            msg = data.NotificaAttoFatturaResult;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: 'Impossibile recuperare le fatture',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });

        }
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaFattureNonAssegnateLiquidazioneOrResidueResult',
        fields: [
           { name: 'Prog' },
	       { name: 'IdUnivoco' },
	       { name: 'Contratto' },
           { name: 'NumeroFatturaBeneficiario' },
           { name: 'DataFatturaBeneficiario' },
           { name: 'DescrizioneFattura' },
           { name: 'ImportoTotaleFattura' },
           { name: 'ImportoLiquidato' },
           { name: 'ImportoResiduo' },
           { name: 'ImportoFattDaLiquidare' },
           { name: 'AnagraficaInfo' }
        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" }
    });

    store.on({
        'load': {
            fn: function (store, records, options) {
                if (records.length == 0) {
                    if (Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont') != null && Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont') != undefined)
                        Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').disable();
                } else {
                    var grigliaFattureNonContest = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont');
                    if (grigliaFattureNonContest != undefined) {
                        grigliaFattureNonContest.getSelectionModel().selectAll();
                        grigliaFattureNonContest.getView().refresh();
                    }
                }
                maskApp.hide();
            },
            scope: this
        }
    });



    var parametri = {};

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: false,
        listeners: {

            rowselect: function (selectionModel, rowIndex, record) {
                if (record.data.ImportoResiduo == 0) {
                    selectionModel.getSelections().remove(rowIndex);
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Impossibile selezionare la fattura ' + record.data.NumeroFatturaBeneficiario + ' il residuo è ZERO',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) { return }
                    });
                } else {
                    var totalRows = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont').store.getRange().length;
                    var selectedRows = selectionModel.getSelections();
                    if (selectedRows.length > 0) {

                        Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').enable();
                    }
                    if (totalRows == selectedRows.length) {

                        if (Ext.getCmp('GridPopUpFattureLiquidazioneNonCont') != undefined) {
                            var view = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont').getView();
                            var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                            chkdiv.addClass("x-grid3-hd-checker-on");
                        }

                    }
                }
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();

                if (selectedRows.length == 0) {
                    Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').disable();
                }
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
            sm,

            { header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true },
            { header: "Data Fattura", width: 80, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },
            {
                header: "Beneficiario", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo != undefined && record.data.AnagraficaInfo != null) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Nome) && !isNullOrEmpty(record.data.AnagraficaInfo.Cognome)) {
                            retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome;
                        }
                        if (!isNullOrEmpty(record.data.AnagraficaInfo.Denominazione)) {
                            retValue = record.data.AnagraficaInfo.Denominazione;
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "Sede", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede;
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "Metodo Pagamento", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento;
                        }
                    }
                    return retValue;
                }
            },

             {
                 header: "IdConto", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false, hidden: true,
                 renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                     var retValue = '';
                     if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                         if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                             if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente)) {
                                 retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente;
                             }
                         }
                     }
                     return retValue;
                 }
             },

            {
                header: "Iban", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban;
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Descrizione", width: 100, dataIndex: 'DescrizioneFattura', sortable: true, locked: false, hidden: true },
            { header: "Importo Totale", width: 60, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false, renderer: eurRend },
            { header: "Liquidato", width: 60, dataIndex: 'ImportoLiquidato', sortable: true, locked: false, renderer: eurRend },
            { header: "Residuo", width: 60, dataIndex: 'ImportoResiduo', sortable: true, locked: false, renderer: eurRend },
            {
                header: "Da Liquidare", width: 60, dataIndex: 'ImportoFattDaLiquidare', sortable: true, locked: false,
                css: 'background-color: #fffb8a;',
                renderer: eurRend,
                editor: new Ext.form.NumberField({
                    decimalSeparator: ',',
                    allowBlank: true,
                    allowNegative: true
                })
            },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "Prog", width: 100, dataIndex: 'Prog', hidden: true }
    ]);

    var grid = new Ext.grid.EditorGridPanel({
        id: 'GridPopUpFattureLiquidazioneNonCont',
        title: 'Lista Fatture',
        autoHeight: false,
        height: 100,
        width: 750,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        viewConfig: {
            emptyText: "Nessuna fattura corrisponde ai criteri di ricerca inseriti.",
            deferEmptyText: false
        },
        sm: sm
    });

    return grid;
}

function showPopupPanelFattureLiquidazioneNonCont(liquidazione, importoTotaleFattureLiq) {
    var popup = new Ext.Window({
        title: 'Seleziona le fatture da aggiungere alla liquidazione',
        width: 900,
        height: 450,
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        resizable: false,
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        autoScroll: true,
        closable: true,
        buttons: [{
            text: 'Conferma',
            id: 'btnConfermaSelezioneFatturaLiqNonCont'
        }, {
            text: 'Chiudi',
            id: 'btnChiudiSelezioneFatturaLiqNonCont'
        }]
    });

    var gridFattureDocumento = buildPopUpFattureLiquidazioneNonCont(liquidazione, importoTotaleFattureLiq);

    if (gridFattureDocumento != null && gridFattureDocumento != undefined)
        popup.add(gridFattureDocumento);


    popup.doLayout();
    popup.show();

    Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').disable();

    Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').on('click', function () {

        var fatture = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont').getSelectionModel().getSelections();
        var residuoLiquidazione = liquidazione.ImpPrenotatoLiq - importoTotaleFattureLiq;
        residuoLiquidazione = parseFloat(residuoLiquidazione).toFixed(2);
        var listaFatture = [];
        if (fatture.length > 0) {
            var errore = false;
            var messaggioErrore = "";
            var totaleImportiDaLiquidare = 0.0;

            Ext.each(fatture, function (rec) {
                totaleImportiDaLiquidare = totaleImportiDaLiquidare + rec.data.ImportoFattDaLiquidare;
            })
            totaleImportiDaLiquidare = parseFloat(totaleImportiDaLiquidare).toFixed(2);
            if (totaleImportiDaLiquidare > liquidazione.ImpPrenotatoLiq) {
                errore = true;
                messaggioErrore = "Il totale degli importi da liquidare (€ " + totaleImportiDaLiquidare + ") è maggiore dell\'importo prenotato per la liquidazione (€ " + liquidazione.ImpPrenotatoLiq + ")<br/>";
            } else if (totaleImportiDaLiquidare > residuoLiquidazione) {
                //controllare le fatt già presenti sulle liq, calcolare il totale e verificare che l'importo totale che sto aggiungendo (totaleImportiDaLiquidare) non sia
                //maggiore dell'importo residuo sulla liq (importo liq MENO fatt già presenti sulle liq)
                errore = true;
                messaggioErrore = "L'importo residuo sulla liquidazione è di € " + residuoLiquidazione + ". L\'importo totale indicato sulle futture è € " + totaleImportiDaLiquidare + "<br/>";
            } else {
                Ext.each(fatture, function (rec) {
                    if (rec.data.ImportoFattDaLiquidare != undefined) {
                        if ((rec.data.ImportoFattDaLiquidare == 0) || (rec.data.ImportoFattDaLiquidare == '')) {
                            errore = true;
                            messaggioErrore = messaggioErrore + "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare è ZERO<br/>";
                        } else {
                            if ((rec.data.ImportoFattDaLiquidare > rec.data.ImportoResiduo)) {
                                errore = true;
                                messaggioErrore = "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare (€ " + rec.data.ImportoFattDaLiquidare + ") è maggiore dell\' importo residuo della fattura (€ " + rec.data.ImportoResiduo + ")<br/>";
                            } else {
                                listaFatture.push(rec.data);
                            }
                        }
                    } else {
                        errore = true;
                        messaggioErrore = messaggioErrore + "N° Fatt " + rec.data.NumeroFatturaBeneficiario + ": l\'importo da liquidare è ZERO<br/>";
                    }
                });
            }
            if (errore) {
                Ext.MessageBox.show({
                    title: 'Associazione Fattura',
                    msg: messaggioErrore,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function (btn) { return }
                });
            } else {
                if (listaFatture.length != 0) {
                    var statoOperazione = "I";
                    registraFatturaLiquidazioneNonCont(statoOperazione, liquidazione.ID, listaFatture);

                    //                    var gridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont');
                    //                    var storeGridFatture = gridFatture.getStore();
                    //                    storeGridFatture.reload();
                    popup.close();
                } else {
                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Selezionare una fattura e specificare l\'importo da liquidare',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.WARNING
                    });
                    return;
                }
            }
        } else {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Selezionare una fattura e specificare l\'importo da liquidare',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        }
    });


    Ext.getCmp('btnChiudiSelezioneFatturaLiqNonCont').on('click', function () {
        if (Ext.getCmp('GridFattureLiquidazioneNonCont') != undefined) {
            Ext.getCmp('GridFattureLiquidazioneNonCont').getStore().reload();
            var totalRows = Ext.getCmp('GridFattureLiquidazioneNonCont').store.getRange().length;
            if (totalRows == 0) {
                Ext.getCmp('myPanelLiq').remove(Ext.getCmp('panelFatturaLiquidazioneNonCont'));
                Ext.getCmp('myPanelLiq').doLayout();
            }
        }
        popup.close();
    });

    popup.on('close', function () {
        var gridFatture = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont');
        gridFatture.getView().refresh();

    });
}

function buildPanelFattureLiquidazioneNonCont(liquidazione) {

    var panelFatturaLiquidazioneNonCont = new Ext.Panel({
        id: 'panelFatturaLiquidazioneNonCont',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        style: 'margin-top:15px;',

        layoutConfig: {
            columns: 1
        },
        items: [
            buildPanelGridFattureLiquidazioneNonCont(liquidazione),
            {
                xtype: 'hidden',
                id: 'listaFattureLiquidazioneNonCont'
            }
        ]
    });
    return panelFatturaLiquidazioneNonCont;
}

function buildPanelGridFattureLiquidazioneNonCont(liquidazione) {

    var aggiungiFatturaLiquidazioneNonCont = new Ext.Action({
        text: 'Aggiungi',
        id: 'actionAggiungiFatturaLiquidazioneNonCont',
        tooltip: 'Seleziona e aggiunge uno più fatture alla liquidazione selezionata',
        handler: function () {
            //stef
            var importoTotaleFattureLiq = 0;
            var gridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont');
            if (gridFatture != undefined) {
                var storeGridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont').getStore();
                storeGridFatture.each(function (rec) {
                    importoTotaleFattureLiq = importoTotaleFattureLiq + rec.data.ImportoLiquidato;
                });
                showPopupPanelFattureLiquidazioneNonCont(liquidazione, importoTotaleFattureLiq);
            }
        },
        iconCls: 'add'
    });

    var rimuoviFatturaLiquidazioneNonCont = new Ext.Action({
        text: 'Rimuovi',
        id: 'actionRimuoviFatturaLiquidazioneNonCont',
        tooltip: 'Rimuove una o più fatture dalla liquidazione selezionata',

        handler: function () {
            Ext.MessageBox.buttonText.cancel = "Annulla";
            var gridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont');
            var storeGridFatture = gridFatture.getStore();
            var selections = gridFatture.getSelectionModel().getSelections();

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Procedere con la rimozione ' + (selections.length == 1 ? 'della fattura selezionata' : 'delle fatture selezionate') + '?',
                buttons: Ext.MessageBox.OKCANCEL,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                    if (btn == 'ok') {
                        var listaFatture = [];
                        var gridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont');
                        Ext.each(gridFatture.getSelectionModel().getSelections(), function (rec) {
                            if (rec.data['IdUnivoco'] != 0) {
                                listaFatture.push(rec.data);
                            }
                        })

                        EliminaFatturaDaLiquidazioneNonCont(listaFatture, 'C', liquidazione.ID);

                        var gridFatture = Ext.getCmp('GridFattureLiquidazioneNonCont');
                        if (gridFatture != undefined && gridFatture != null) {
                            var storeGridFatture = gridFatture.getStore();
                            storeGridFatture.reload();
                        }
                        //STEEEF
                        var gridBen = Ext.getCmp('GridBeneficiari');
                        if (gridBen != undefined && gridBen != null) {
                            var storeGridBen = gridBen.getStore();
                            storeGridBen.reload();
                        }


                        var selectedRows = gridFatture.getSelectionModel().getSelections();
                        if (selectedRows.length == 0) {
                            rimuoviFatturaLiquidazioneNonCont.disable();
                        }
                    }
                }
            });
        },
        iconCls: 'remove'
    });



    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaFattureByLiquidazione' + window.location.search,
        method: 'GET',
        timeout: 20000000
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetListaFattureByLiquidazioneResult',
        fields: [
           { name: 'Prog' },
	       { name: 'IdUnivoco' },
	       { name: 'Contratto' },
           { name: 'NumeroFatturaBeneficiario' },
           { name: 'DataFatturaBeneficiario' },
           { name: 'DescrizioneFattura' },
           { name: 'ImportoTotaleFattura' },
           { name: 'ImportoLiquidato' },
           { name: 'AnagraficaInfo' },
           { name: 'IdLiquidazione' }

        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        sortInfo: { field: "NumeroFatturaBeneficiario", direction: "ASC" }
    });

    store.on({
        'load': {
            fn: function (store, records, options) {
                if (records.length == 0) {
                    //                if (Ext.getCmp('btnConfermaSelezioneFattura') != null && Ext.getCmp('btnConfermaSelezioneFattura') != undefined)
                    //                    Ext.getCmp('btnConfermaSelezioneFattura').disable();
                    aggiungiFatturaLiquidazioneNonCont.execute();
                }
                maskApp.hide();
            },
            scope: this
        }
    });

    maskApp.show();

    var parametri = { idLiquidazione: liquidazione.ID };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: true,
        listeners: {
            rowselect: function (selectionModel, rowIndex, record) {
                var totalRows = Ext.getCmp('GridFattureLiquidazioneNonCont').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviFatturaLiquidazioneNonCont.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridFattureLiquidazioneNonCont').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviFatturaLiquidazioneNonCont.disable();
                }
                var view = Ext.getCmp('GridFattureLiquidazioneNonCont').getView();
                var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                chkdiv.removeClass('x-grid3-hd-checker-on');
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        {
            header: "Repertorio", width: 60, dataIndex: 'NumeroRepertorio', sortable: true, locked: true,
            renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                var retValue = '';
                if (record.data.Contratto != undefined && record.data.Contratto != null) {

                    if (!isNullOrEmpty(record.data.Contratto.NumeroRepertorio)) {
                        retValue = record.data.Contratto.NumeroRepertorio;
                    }

                }
                return retValue;
            }
        },
         { header: "Numero Fattura", width: 100, dataIndex: 'NumeroFatturaBeneficiario', sortable: true, locked: true },
         { header: "Data Fattura", width: 60, dataIndex: 'DataFatturaBeneficiario', sortable: true, locked: false },

         {
             header: "Beneficiario", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
             renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                 var retValue = '';
                 if (record.data.AnagraficaInfo != undefined && record.data.AnagraficaInfo != null) {

                     if (!isNullOrEmpty(record.data.AnagraficaInfo.Nome) && !isNullOrEmpty(record.data.AnagraficaInfo.Cognome)) {
                         retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome;
                     }
                     if (!isNullOrEmpty(record.data.AnagraficaInfo.Denominazione)) {
                         retValue = record.data.AnagraficaInfo.Denominazione;
                     }
                 }
                 return retValue;
             }
         },

            {
                header: "Sede", width: 85, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].NomeSede)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].NomeSede;
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "Metodo Pagamento", width: 80, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento)) {
                            retValue = record.data.AnagraficaInfo.ListaSedi[0].ModalitaPagamento;
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "IdConto", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false, hidden: true,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].IdContoCorrente;
                            }
                        }
                    }
                    return retValue;
                }
            },

            {
                header: "Iban", width: 100, dataIndex: 'AnagraficaInfo', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.AnagraficaInfo.ListaSedi != undefined && record.data.AnagraficaInfo.ListaSedi.length > 0) {
                        if (record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != undefined && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari != null && record.data.AnagraficaInfo.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban)) {
                                retValue = record.data.AnagraficaInfo.ListaSedi[0].DatiBancari[0].Iban;
                            }
                        }
                    }
                    return retValue;
                }
            },

            { header: "Descrizione", width: 100, dataIndex: 'DescrizioneFattura', sortable: true, locked: false, hidden: true },
            { header: "Importo Totale", width: 60, dataIndex: 'ImportoTotaleFattura', sortable: true, locked: false, renderer: eurRend },
            { header: "Da Liquidare", width: 60, dataIndex: 'ImportoLiquidato', sortable: true, locked: false, renderer: eurRend },
            { header: "IdUnivoco", width: 100, dataIndex: 'IdUnivoco', hidden: true },
            { header: "IdDocumento", width: 100, dataIndex: 'IdDocumento', hidden: true },
            { header: "IdLiquidazione", width: 100, dataIndex: 'IdLiquidazione', hidden: true }

    ]);


    var grid = new Ext.grid.GridPanel({
        id: 'GridFattureLiquidazioneNonCont',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        xtype: "form",
        autoHeight: false,
        height: 230,
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        width: 740,
        stripeRows: true,
        loadMask: true,
        title: "Fatture relative alla liquidazione selezionata",
        tbar: [aggiungiFatturaLiquidazioneNonCont, rimuoviFatturaLiquidazioneNonCont],

        viewConfig: {
            emptyText: "Nessuna fattura associata alla liquidazione.",
            deferEmptyText: false
        },
        sm: sm
    });

    grid.on('render', function () {
        this.getView().mainBody.on('mousedown', function (e, t) {
            if (t.tagName == 'A') {
                e.stopEvent();
                t.click();
            }
        });
    }, grid);

    var gridFatture = new Ext.Panel({
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        layoutConfig: {
            columns: 1
        },
        items: [grid]
    }
    );

    rimuoviFatturaLiquidazioneNonCont.disable();

    return gridFatture;
}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LE FATTURE DELLA LIQUIDAZIONE SELEZIONATA
function EliminaFatturaDaLiquidazioneNonCont(listaFatture, statoOperazione, idLiquidazione) {

    var params = { listaFatture: listaFatture, statoOperazione: statoOperazione, idLiquidazione: idLiquidazione };

    Ext.Ajax.request({
        url: 'ProcAmm.svc/NotificaAttoFattura' + window.location.search,
        params: Ext.encode(params),
        headers: { 'Content-Type': 'application/json' },
        method: 'POST',
        success: function (response, options) {
            var msg = "";
            if (response.statusText == "OK") {
                msg = "Operazione completata con successo";
            } else {
                msg = "Errore Generico";
            }
            Ext.MessageBox.show({
                title: 'Eliminazione Fatture liquidazione',
                msg: msg,
                buttons: Ext.MessageBox.OK
            });

            try {
                Ext.getCmp('GridFattureLiquidazioneNonCont').getStore().reload();
            } catch (ex) { }

        },
        failure: function (response, result) {
            var data = Ext.decode(response.responseText);

            msg = data.FaultMessage;
            Ext.MessageBox.show({
                title: 'Errore',
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });

        }
    });
}

function buildGridBeneficiariImpegno() {

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: false,
        listeners: {
            rowselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    Ext.getCmp('btnAggiungi').enable();
                } else {
                    Ext.getCmp('btnAggiungi').disable();
                }
                //if (record.data.ImportoResiduo == 0) {
                //    selectionModel.getSelections().remove(rowIndex);
                //    Ext.MessageBox.show({
                //        title: 'Attenzione',
                //        msg: 'Impossibile selezionare la fattura ' + record.data.NumeroFatturaBeneficiario + ' il residuo è ZERO',
                //        buttons: Ext.MessageBox.OK,
                //        icon: Ext.MessageBox.WARNING,
                //        fn: function (btn) { return }
                //    });
                //} else {
                //    var totalRows = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont').store.getRange().length;
                //    var selectedRows = selectionModel.getSelections();
                //    if (selectedRows.length > 0) {

                //        Ext.getCmp('btnConfermaSelezioneFatturaLiqNonCont').enable();
                //    }
                //    if (totalRows == selectedRows.length) {

                //        if (Ext.getCmp('GridPopUpFattureLiquidazioneNonCont') != undefined) {
                //            var view = Ext.getCmp('GridPopUpFattureLiquidazioneNonCont').getView();
                //            var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker")
                //            chkdiv.addClass("x-grid3-hd-checker-on");
                //        }

                //    }
                //}
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    Ext.getCmp('btnAggiungi').enable();
                } else {
                    Ext.getCmp('btnAggiungi').disable();
                }
            }
        }
    });


    var ColumnModel = new Ext.grid.ColumnModel([
            sm,
            {
                header: "P.IVA/C.F.", width: 120, dataIndex: 'PartitaIvaOrCodFiscToView', sortable: false, locked: false
            }, {
                header: "Denominazione", width: 120, dataIndex: 'Denominazione', sortable: false, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (!isNullOrEmpty(record.data.Nome) && !isNullOrEmpty(record.data.Cognome)) {
                        retValue = record.data.Nome + " " + record.data.Cognome;
                    }
                    if (!isNullOrEmpty(record.data.Denominazione)) {
                        retValue = record.data.Denominazione;
                    }

                    return retValue;
                }
            }, {
                header: "Sede", width: 200, dataIndex: 'ListaSedi[0]', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.ListaSedi != undefined && record.data.ListaSedi.length > 0) {

                        if (!isNullOrEmpty(record.data.ListaSedi[0].NomeSede)) {
                            retValue = record.data.ListaSedi[0].NomeSede + ': ' + record.data.ListaSedi[0].Indirizzo;
                        }
                    }
                    return retValue;
                }
            }, {
                header: "Metodo Pagamento", width: 170, dataIndex: 'IdModalitaPagamentoSelected', sortable: true, locked: false,
                css: 'background-color: #fffb8a;',
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var descrizioneModalitaPagamento = "";
                    var comboModalitaPagamentoId = Ext.getCmp('comboModalitaPagamentoId');

                    var idModalitaPagamento = record.data.ListaSedi[0].IdModalitaPagamento;
                    var idModalitaPagamentoNew = comboModalitaPagamentoId.getValue();
                    if (idModalitaPagamentoNew != "") {
                        var modalitaPagamentoNew = comboModalitaPagamentoId.findRecord(comboModalitaPagamentoId.valueField || comboModalitaPagamentoId.displayField, idModalitaPagamentoNew);
                        descrizioneModalitaPagamento = modalitaPagamentoNew.data.Descrizione;
                        record.data.ListaSedi[0].IdModalitaPagamento = idModalitaPagamentoNew;
                    } else {
                        if (idModalitaPagamento != undefined) {
                            var modalitaPagamento = comboModalitaPagamentoId.findRecord(comboModalitaPagamentoId.valueField || comboModalitaPagamentoId.displayField, idModalitaPagamento);
                            if (modalitaPagamento != undefined)
                                descrizioneModalitaPagamento = modalitaPagamento.data.Descrizione;
                        }
                    }
                    return descrizioneModalitaPagamento;

                },
                editor: buildComboModalitaPagamento()
            }, {
                header: "IdConto", width: 20, dataIndex: 'ListaSedi[0]', sortable: true, locked: false, hidden: true,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.ListaSedi != undefined && record.data.ListaSedi.length > 0) {
                        if (record.data.ListaSedi[0].DatiBancari != undefined && record.data.ListaSedi[0].DatiBancari != null && record.data.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.ListaSedi[0].DatiBancari[0].IdContoCorrente)) {
                                retValue = record.data.ListaSedi[0].DatiBancari[0].IdContoCorrente;
                            }
                        }
                    }
                    return retValue;
                }
            }, {
                header: "Iban", width: 170, dataIndex: 'ListaSedi[0]', sortable: true, locked: false,
                renderer: function (value, metaData, record, rowIdx, colIdx, store) {
                    var retValue = '';
                    if (record.data.ListaSedi != undefined && record.data.ListaSedi.length > 0) {
                        if (record.data.ListaSedi[0].DatiBancari != undefined && record.data.ListaSedi[0].DatiBancari != null && record.data.ListaSedi[0].DatiBancari.length > 0) {

                            if (!isNullOrEmpty(record.data.ListaSedi[0].DatiBancari[0].Iban)) {
                                retValue = record.data.ListaSedi[0].DatiBancari[0].Iban;
                            }
                        }
                    }
                    return retValue;
                }
            }, {
                header: "Importo Originario", width: 80, dataIndex: 'ImportoOriginarioSuImpegno', sortable: true, locked: false, renderer: eurRend
            }, {
                header: "Importo Residuo", width: 80, dataIndex: 'ImportoResiduoSuImpegno', sortable: true, locked: false, renderer: eurRend
            },
            {
                header: "Da Liquidare", width: 80, dataIndex: 'ImportoDaLiquidare', sortable: true, locked: false,
                css: 'background-color: #fffb8a;',
                renderer: eurRend,
                editor: new Ext.form.NumberField({
                    decimalSeparator: ',',
                    allowBlank: true,
                    allowNegative: true
                })
            }
            
    ]);

    var grid = new Ext.grid.EditorGridPanel({
        id: 'GridBeneficiariImpegno',
        name: 'GridBeneficiariImpegno',
        stripeRows: true,
        autoHeight: false,
        ds: getListaBeneficiariImpegnoStore(),
        cm: ColumnModel,
        viewConfig: {
            emptyText: "Nessun beneficiario presente. Per impegni non recanti beneficiari, è necessario contattare l’Ufficio Ragioneria Generale e Fiscalità Regionale per aggiornare direttamente sul SIC i dati dell\’impegno, integrandolo con tutte le informazioni relative al beneficiario. In seguito sarà possibile procedere con la liquidazione dell’impegno, che in automatico erediterà il beneficiario.",
            deferEmptyText: false,
            forceFit: true
        },
        sm: sm
    });

    grid.doLayout();

    return grid;
}

var actionAddLiquidazione = new Ext.Action({
    text: 'Aggiungi',
    tooltip: 'Aggiungi una nuova riga',
    handler: function () {
        var mostraAnno = false;
        if (!isDocumentoConFatture) {
            Ext.MessageBox.buttonText.yes = 'Si';

            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Se si intende pagare una fattura è necessario aggiungerla nella sezione Contratti/Fatture.<br>Aggiungere la fattura all\'atto?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                    if (btn == 'yes') {
                        window.open("CreaProvvedimento.aspx" + window.location.search + "&addFatt=1", "_self");
                    } else {
                        InitFormCapitoli(mostraAnno);
                    }
                }
            });
        } else {
            InitFormCapitoli(mostraAnno);
        }

    },
    iconCls: 'add'
});

var actionDeleteLiquidazione = new Ext.Action({
    text: 'Cancella',
    tooltip: 'Cancella selezionato',
    handler: function () {
        var storeGridLiq = Ext.getCmp('GridLiquidazione').getStore();
        var GridLiquidazione = Ext.getCmp('GridLiquidazione');

        if (GridLiquidazione != undefined) {
            Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function (rec) {
                EliminaLiquidazioni(rec.data['NumPreImp'], rec.data['NumImpegno'], rec.data['ID'])
                storeGridLiq.remove(rec);
                var numRighePresenti = storeGridLiq.data.length;
                actionAddLiquidazione.setDisabled(false);
                if (numRighePresenti >= 0) {
                    actionAddLiquidazione.setDisabled(false);
                    actionBeneficiari.setDisabled(true);
                    actionShowFattureLiquidazioneNonCont.setDisabled(true);
                    actionDeleteLiquidazione.setDisabled(true);

                    Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridBeneficiari'));
                }
            })
            Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridFattureLiquidazioneNonCont'));
        }

    },
    iconCls: 'remove'
});


var actionCompilaRiduzioni = new Ext.Action({
    text: 'Crea Riduzione',
    tooltip: 'Compila una riduzione per la liquidazione selezionata',
    handler: function () {
        addRiduzioneContestualeDaSelezione();
    },
    iconCls: 'add'
});

//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaLiquidazione = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function () {
        var liquidazioniDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridLiquidazione').getSelectionModel().getSelections(), function (rec) {
            //conferma solo le liquidazioni da confermare, quelle aventi stato 2
            if (rec.data['Stato'] == 2) {
                var liquidazioneInfo = new Object();

                liquidazioneInfo.ID = rec.data['ID'];
                liquidazioneInfo.Stato = rec.data['Stato'];
                liquidazioneInfo.NumPreImp = rec.data['NumPreImp'];
                liquidazioneInfo.NumImpegno = rec.data['NumImpegno'];

                liquidazioniDaComfermare.push(liquidazioneInfo);
            }
        });

        ConfermaMultiplaLiquidazione(liquidazioniDaComfermare);
    },
    iconCls: 'save'
});
//DEFINISCO L'AZIONE per la gestione dei beneficiari
var actionBeneficiari = new Ext.Action({
    text: 'Beneficiari',
    tooltip: 'Aggiunge, Elimina e Modifica i Beneficiari della liquidazione selezionata',
    handler: function () {
        Ext.each(Ext.getCmp('GridLiquidazione').getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridBeneficiari') != undefined) {
                Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridBeneficiari'));
                Ext.getCmp('myPanelLiq').doLayout();
            } else {
                //var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio };
                //var grid = BeneficiariLiquidazioneImpegno(liquidazione, undefined, "GridBeneficiari");

                //Ext.getCmp('myPanelLiq').add(grid);
                //Ext.getCmp('myPanelLiq').doLayout();

                showGridBeneficiari(rec);
            }
        });
    },
    iconCls: 'coin'
});


//DEFINISCO L'AZIONE per la gestione delle fatture delle liquidazioni
var actionShowFattureLiquidazioneNonCont = new Ext.Action({
    text: 'Fatture',
    tooltip: 'Aggiunge le Fatture della liquidazione selezionata',
    handler: function () {

        Ext.each(Ext.getCmp('GridLiquidazione').getSelectionModel().getSelections(), function (rec) {


            if (Ext.getCmp('GridFattureLiquidazioneNonCont') != undefined) {

                Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridFattureLiquidazioneNonCont'));
                Ext.getCmp('myPanelLiq').doLayout();

            } else {
                var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };
                var grid = buildPanelFattureLiquidazioneNonCont(liquidazione);

                Ext.getCmp('myPanelLiq').add(grid);
                Ext.getCmp('myPanelLiq').doLayout();
            }
        }
		)
    },
    iconCls: 'add'
});

function enableConfirmAction(selectedRows) {
    var retValue = false

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.Stato == 2))
            break;

    return retValue;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "Liquidazioni"
function buildGridLiquidazioni(isFatturePresenti) {
    isDocumentoConFatture = isFatturePresenti;

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetLiquidazioniRegistrate' + window.location.search,
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetLiquidazioniRegistrateResult',
        fields: [
               { name: 'Bilancio' },
               { name: 'UPB' },
               { name: 'MissioneProgramma' },
               { name: 'Capitolo' },
               { name: 'ImpDisp' },
               { name: 'ImpPrenotato' },
               { name: 'NumPreImp' },
               { name: 'AnnoPrenotazione' },
               { name: 'ID' },
               { name: 'ImpPrenotatoLiq' },
               { name: 'ImportoIva' },
               { name: 'ContoEconomica' },
               { name: 'NumImpegno' },
               { name: 'Tipo' },
               { name: 'Stato' },
               { name: 'StatoAsString' },
               { name: 'PianoDeiContiFinanziario' }
        ]
    });

    var store = new Ext.data.GroupingStore({
        proxy: proxy
            , reader: reader,
        groupField: 'Tipo',
        sortInfo: {
            field: 'Tipo',
            direction: "ASC"
        }
    })

    try {
        var storeGridRiep = Ext.getCmp('GridRiepilogo').getStore();
        if (storeGridRiep.data.length != 0) {
            store = storeGridRiep;
        } else {
            var ufficio = Ext.get('Cod_uff_Prop').dom.value;
            var parametri = { CodiceUfficio: ufficio };
            store.load({ params: parametri });

            store.on({
                'load': {
                    fn: function (store, records, options) {
                        maskApp.hide();
                    },
                    scope: this
                }
            });
        }
    } catch (Err) {

        var ufficio = Ext.get('Cod_uff_Prop').dom.value;
        var parametri = { CodiceUfficio: ufficio };
        store.load({ params: parametri });

        store.on({
            'load': {
                fn: function (store, records, options) {
                    maskApp.hide();
                },
                scope: this
            }
        });
    }

    var summary = new Ext.grid.GroupSummary();

    var sm = new Ext.grid.CheckboxSelectionModel(
            {
                singleSelect: false,
                listeners: {
                    rowselect: function (sm, row, rec) {
                        var multiSelect = sm.getSelections().length > 1;

                        if (Ext.getCmp('GridBeneficiari') != undefined) {
                            Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridBeneficiari'));
                            Ext.getCmp('myPanelLiq').doLayout();
                        }

                        if (Ext.getCmp('GridFattureLiquidazioneNonCont') != undefined) {
                            Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridFattureLiquidazioneNonCont'));
                            Ext.getCmp('myPanelLiq').doLayout();
                        }

                        actionBeneficiari.setDisabled(multiSelect);
                        if (isDocumentoConFatture) {
                            actionShowFattureLiquidazioneNonCont.setDisabled(multiSelect);
                        } else {
                            actionShowFattureLiquidazioneNonCont.setDisabled(true);
                        }

                        actionDeleteLiquidazione.setDisabled(multiSelect);

                        if (rec.data.Stato == 2) {
                            actionCompilaRiduzioni.setDisabled(true);
                            actionConfermaLiquidazione.setDisabled(false);
                            actionAddLiquidazione.setDisabled(true);
                        } else {
                            actionAddLiquidazione.setDisabled(true);
                            actionCompilaRiduzioni.setDisabled(multiSelect);
                        }
                    },
                    rowdeselect: function (sm, row, rec) {
                        var selectedRowsCount = sm.getSelections().length;

                        if (Ext.getCmp('GridBeneficiari') != undefined) {
                            Ext.getCmp('myPanelLiq').remove(Ext.getCmp('GridBeneficiari'));
                            Ext.getCmp('myPanelLiq').doLayout();
                        }

                        if (Ext.getCmp('panelFatturaLiquidazioneNonCont') != undefined) {
                            Ext.getCmp('myPanelLiq').remove(Ext.getCmp('panelFatturaLiquidazioneNonCont'));
                            Ext.getCmp('myPanelLiq').doLayout();
                        }

                        actionBeneficiari.setDisabled(selectedRowsCount == 1 ? false : true);
                        actionShowFattureLiquidazioneNonCont.setDisabled(selectedRowsCount == 1 ? false : true);
                        actionDeleteLiquidazione.setDisabled(selectedRowsCount == 1 ? false : true);

                        if (selectedRowsCount == 1) {
                            if (sm.getSelected().data.Stato == 2) {
                                actionCompilaRiduzioni.setDisabled(true);
                            } else {
                                actionCompilaRiduzioni.setDisabled(false);
                            }
                        } else
                            actionCompilaRiduzioni.setDisabled(true);

                        actionAddLiquidazione.setDisabled(selectedRowsCount == 0 ? false : true);
                        actionConfermaLiquidazione.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));
                    }
                }
            });
    var ColumnModel = new Ext.grid.ColumnModel({
        columns: [
                            sm,
                            {
                                header: "Bilancio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true,
                                summaryRenderer: function (v, params, data) { return '<b> Totale </b>'; }
                            },
                            { header: "Capitolo", dataIndex: 'Capitolo', sortable: true },
                            { header: "UPB", dataIndex: 'UPB', sortable: true },
                            { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
                            { renderer: eurRend, header: "Importo da Liquidare", align: 'right', dataIndex: 'ImpPrenotatoLiq', sortable: true, summaryType: 'sum' },
                            { header: "Numero Impegno", dataIndex: 'NumImpegno', id: 'NumImpegno', sortable: true },
                            { header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario', sortable: true },
                            { header: "Stato", dataIndex: 'StatoAsString' },
                            { header: "ID", dataIndex: 'ID', id: 'ID', sortable: false, hidden: true },
                            { header: "Tipo", dataIndex: 'Tipo', hidden: true }
        ]
    });


    var GridLiq = new Ext.grid.EditorGridPanel({
        id: 'GridLiquidazione',
        ds: store,
        sm: sm,
        colModel: ColumnModel,
        title: "",
        autoHeight: true,
        autoWidth: true,
        layout: 'fit',
        plugins: [summary],
        loadMask: true,
        view: new Ext.grid.GroupingView({
            forceFit: true,
            showGroupName: false,
            enableNoGroups: true, // REQUIRED!
            hideGroupedColumn: true,
            enableGroupingMenu: true
        })
    });

    actionDeleteLiquidazione.setDisabled(true);
    actionCompilaRiduzioni.setDisabled(true);
    actionConfermaLiquidazione.setDisabled(true);
    actionBeneficiari.setDisabled(true);
    actionShowFattureLiquidazioneNonCont.setDisabled(true);
    return GridLiq;
}

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni"
var myPanelLiq = new Ext.FormPanel({
    id: 'myPanelLiq',
    frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    collapsible: true,
    width: 750,
    xtype: "form",
    title: "Liquidazioni su Altri Impegni",
    tbar: [actionAddLiquidazione, actionDeleteLiquidazione, actionConfermaLiquidazione, actionCompilaRiduzioni, actionShowFattureLiquidazioneNonCont, actionBeneficiari],
    items: [
	  	 {
	  	     id: "LiquidazioniCapitoli",
	  	     xtype: "hidden"
	  	 }]
});

function addRiduzioneContestualeDaSelezione() {
    var store = Ext.getCmp('GridRiepilogoRiduzione').getStore();

    var GridLiquidazione = Ext.getCmp('GridLiquidazione');
    //Lu rid
    Ext.each(GridLiquidazione.getSelectionModel().getSelections(), function (rec) {
        var TipoRecord = Ext.data.Record.create([
            { name: 'Bilancio' }, { name: 'UPB' }, { name: 'MissioneProgramma' }, { name: 'Capitolo' }, { name: 'ImpDisp' }, { name: 'ImpPrenotato' }, { name: 'NumImpegno' }, { name: 'ImpPrenotatoLiq' }, { name: 'AnnoPrenotazione' }, { name: 'ID' }, { name: 'IsEconomia' }, { name: 'Tipo' }, { name: 'PianoDeiContiFinanziario' }
        ]);

        /* 
         * Se si riduce un impegno dell'anno corrente si parla di Riduzione, 
         * altrimenti si definisce Economia 
         */
        var _isEconomia = '1';
        if (rec.data.NumImpegno.substring(0, 4) == rec.data.Bilancio) {
            _isEconomia = '0';
        }

        var record = new TipoRecord({
            Bilancio: rec.data.Bilancio,
            UPB: rec.data.UPB,
            MissioneProgramma: rec.data.MissioneProgramma,
            Capitolo: rec.data.Capitolo,
            ImpDisp: rec.data.ImpDisp,
            ImpPrenotato: 0,
            NumImpegno: rec.data.NumImpegno,
            ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq,
            AnnoPrenotazione: rec.data.AnnoPrenotazione,
            ID: 0,
            IsEconomia: _isEconomia,
            Tipo: ' ',
            PianoDeiContiFinanziario: rec.data.PianoDeiContiFinanziario
        });
        var flagTrovato = false;

        store.each(function (recordStore) {

            var fieldValue = recordStore.data.NumImpegno;
            if (fieldValue == record.data.NumImpegno) {
                flagTrovato = true;
            }
        });

        if (!flagTrovato) {
            store.insert(0, record);
        }

    });

}


//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LE LIQUIDAZIONI
function EliminaLiquidazioni(NumPreImpegno, NumImpegno, ID) {
    var ajaxMask;
    ajaxMask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });
    ajaxMask.show();

    var params = { NumeroPreImpegno: NumPreImpegno, NumImpegno: NumImpegno, ID: ID };

    Ext.Ajax.request({
        url: 'ProcAmm.svc/EliminaLiquidazione' + window.location.search,
        headers: { 'Content-Type': 'application/json' },
        params: Ext.encode(params),
        method: 'POST',
        success: function (response, options) {
            ajaxMask.hide();
            //            var data = Ext.decode(response.responseText);
            // var numeroPreImpegnoResult = data.EliminaPreImpegnoResult;
            //            if (data) {
            //            	return true;
            //            }else{
            //            	return false;
            //            }
        },
        failure: function (response, options) {
            ajaxMask.hide();
            var data = Ext.decode(response.responseText);

            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
        }
    });

}


function InitFormCapitoli(mostraAnno) {

    var AnnoBilancio = new Ext.ux.YearMenu({
        format: 'Y',
        id: 'AnnoBilancio',
        allowBlank: false,
        noPastYears: false,
        noPastMonths: true,
        minDate: new Date('1990/1/1'),
        maxDate: new Date()
                       ,
        handler: function (dp, newValue, oldValue) {
            Ext.getCmp('AnnoIni').value = newValue.format('Y');
            Ext.get('AnnoIni').dom.value = newValue.format('Y');
        }
    });
    var btnRicerca = new Ext.Button({
        text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
        id: 'btnBilancio'
    });

    btnRicerca.on('click', function () {

        var fNewAnno = Ext.getCmp("AnnoIni").value;
        //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
        gridCapitoli = buildGridStart(fNewAnno);
        capitoloPanel.remove('GridCapitoli');
        capitoloPanel.add(gridCapitoli);
        capitoloPanel.doLayout();
    });

    var tbarDate = new Ext.Toolbar({
        style: 'margin-bottom:-1px;',
        width: 700,
        items: [{
            xtype: 'button',
            text: "<font color='#0000A0'><b>Bilancio</b></font>",
            id: 'SelBil',
            menu: AnnoBilancio
        }, {
            xtype: 'textfield',
            cls: 'titfis',
            readOnly: true,
            id: 'AnnoIni',
            width: 80,
            value: new Date().format('Y')
        },
            btnRicerca
        ]
    });

    //DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var capitoloPanel = new Ext.Panel({
        xtype: "panel",
        columnWidth: 1,
        bodyStyle: 'margin-bottom: 10px'
    });

    var beneficiariPanel = new Ext.Panel({
        xtype: "panel",
        title: '<span style="font-size: 0.8em">Beneficiari impegno</span>',
        columnWidth: 1,
        layout: 'fit',
        width: 971,
        height: 180,
        frame: true,
        bodyStyle: 'margin-bottom: 10px',
        x: 0,
        y: 490
    });

    buildComboModalitaPagamento();

    //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
    var formCapitoliLiquidazione = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        title: 'Elenco Capitoli per liquidazione',
        width: 1000,
        layout: 'absolute',
        monitorValid: true,
        autoScroll: true,
        buttons: [{
            text: "Aggiungi Liquidazione",
            id: 'btnAggiungi',
            //formBind: true,
            disabled: true
        }, {
            text: 'Annulla',
            handler: function (btn, evt) {
                popupLiquidazione.close();
            }
        }],
        items: [
            {
                id: "ListaBeneficiariDaLiquidare",
                xtype: "hidden"
            },
            {
                id: "Capitolo",
                xtype: "hidden"
            },
            {
                id: "Bilancio",
                xtype: "hidden"
            },
            {
                id: "UPB",
                xtype: "hidden"
            },
            {
                id: "MissioneProgramma",
                xtype: "hidden"
            },
            capitoloPanel,
            {
                xtype: 'panel',
                title: '<span style="font-size: 0.8em">Seleziona Impegno</span>',
                width: 971,
                //95
                height: 180,
                frame: true,
                layout: 'absolute',
                x: 0,
                y: 305,
                items: [{
                    xtype: 'label',
                    text: 'Impegno:',
                    x: 0,
                    y: 0,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'combo',
                    id: 'comboImpegni',
                    emptyText: 'Seleziona...',
                    name: 'comboImpegni',
                    fieldLabel: 'NumeroImpegno',
                    displayField: 'NumImpegno',
                    loadingText: 'Attendere...',
                    x: 105,
                    y: 0,
                    width: 180,
                    listwidth: 180,
                    editable: false,
                    mode: 'local',
                    triggerAction: 'all',
                    allowBlank: true,
                    store: storeImpegni,
                    listeners: {
                        select: function (ele, rec, idx) {

                            var params = { NumImpegno: rec.data.NumImpegno };

                            Ext.lib.Ajax.defaultPostHeader = 'application/json';
                            Ext.Ajax.request({
                                url: 'ProcAmm.svc/GetBeneficiarioImpegno',
                                params: Ext.encode(params),
                                method: 'POST',
                                success: function (response, options) {
                                    maskApp.hide();
                                    var data = Ext.decode(response.responseText);

                                    listaBeneficiariImpegno = data.GetBeneficiarioImpegnoResult;

                                    VerificaDisponibilitaLiquidazione();


                                    if (Ext.getCmp('GridBeneficiariImpegno') != undefined) {
                                        beneficiariPanel.remove(Ext.getCmp('GridBeneficiariImpegno'));
                                    }
                                    var grigliaBeneficiariImpegno = buildGridBeneficiariImpegno();
                                    beneficiariPanel.add(grigliaBeneficiariImpegno);
                                    beneficiariPanel.doLayout();
                                    beneficiariPanel.show();

                                },
                                failure: function (response, result) {
                                    maskApp.hide();
                                    var data = Ext.decode(response.responseText);
                                    Ext.MessageBox.show({
                                        title: 'Errore',
                                        msg: data.FaultMessage,
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.ERROR,
                                        fn: function (btn) { return }
                                    });
                                }
                            });



                            //VerificaDisponibilitaLiquidazione();

                            //listaBeneficiariImpegno = rec.data.ListaBeneficiari;


                            //if (Ext.getCmp('GridBeneficiariImpegno') != undefined) {
                            //    beneficiariPanel.remove(Ext.getCmp('GridBeneficiariImpegno'));
                            //}
                            //var grigliaBeneficiariImpegno = buildGridBeneficiariImpegno();
                            //beneficiariPanel.add(grigliaBeneficiariImpegno);
                            //beneficiariPanel.doLayout();
                            //beneficiariPanel.show();

                        }
                    },
                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><div style="text-decoration:underline;display:inline">N. Impegno</div>: <b>{NumImpegno}</b></div></tpl>',
                    style: 'background-color: #fffb8a;background-image:none;'
                }, {
                    xtype: 'label',
                    text: 'Disponibilità imp:',
                    x: 320,
                    y: 0,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 410,
                    y: 0,
                    name: 'ImpDisp',
                    id: 'ImpDisp',
                    style: 'opacity:.9;',
                    width: 180,
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Importo potenziale:',
                    x: 620,
                    y: 0,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 750,
                    y: 0,
                    width: 180,
                    name: 'ImpPotenzialePrenotato',
                    id: 'ImpPotenzialePrenotato',
                    style: 'opacity:.9;',
                    readOnly: true,
                    msgTarget: 'qtip',
                    listeners: {
                        render: function (c) {
                            Ext.QuickTips.register({
                                target: c.getEl(),
                                text: 'Importo al netto di eventuali operazioni contabili non ancora registrate (Liquidazioni/Riduzioni)'
                            });
                        }
                    }
                }, {
                    xtype: 'label',
                    text: 'Oggetto impegno:',
                    x: 0,
                    y: 30,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textarea',
                    x: 105,
                    y: 30,
                    width: 180,
                    height: 100,
                    fieldLabel: 'Oggetto Impegno',
                    name: 'Oggetto_Impegno',
                    id: 'Oggetto_Impegno',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Cod. Ob. Gest:',
                    x: 620,
                    y: 30,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 750,
                    y: 30,
                    width: 180,
                    name: 'COGSemplice',
                    id: 'COGSemplice',
                    style: 'opacity:.9;',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Data Atto:',
                    x: 320,
                    y: 30,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'datefield',
                    x: 410,
                    y: 30,
                    width: 180,
                    name: 'DataAtto',
                    id: 'DataAtto',
                    style: 'opacity:.9;',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Numero Atto:',
                    x: 320,
                    y: 60,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 410,
                    y: 60,
                    width: 180,
                    name: 'NumeroAtto',
                    id: 'NumeroAtto',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Tipo Atto:',
                    x: 320,
                    y: 90,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 410,
                    y: 90,
                    width: 180,
                    name: 'TipoAtto',
                    id: 'TipoAtto',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'P.C.F. (*):',
                    x: 620,
                    //y: 90,
                    y: 60,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'combo',
                    id: 'ComboPdCFImpegni',
                    name: 'ComboPdCFImpegni',
                    displayField: 'Id',
                    valueField: 'Id',
                    loadingText: 'Attendere...',
                    x: 750,
                    //y: 90,
                    y: 60,
                    width: 180,
                    listwidth: 180,
                    editable: false,
                    mode: 'local',
                    triggerAction: 'all',
                    allowBlank: true,
                    store: storePdCF,
                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                    style: 'background-color: #fffb8a;background-image:none;',
                    disabled: false,
                    emptyText: 'Seleziona PCF...'
                }
                ]
            }
            , beneficiariPanel
        ]
    });


    if (mostraAnno == false) {
        tbarDate.hide();
    } else { tbarDate.show(); }

    var popupLiquidazione = new Ext.Window({
        //y: 15,
        title: 'Aggiungi una nuova liquidazione da impegno esistente',
        id: 'popup_liquidazione',
        width: 1000,
        height: 800,
        layout: 'fit',
        plain: true,
        //bodyStyle: 'padding-top:10px',
        buttonAlign: 'center',
        maximizable: true,
        enableDragDrop: false,
        collapsible: false,
        modal: true,
        autoScroll: true,
        closable: true  /* * toglie la croce per chiudere la finestra */
    });
    popupLiquidazione.add(formCapitoliLiquidazione);
    popupLiquidazione.doLayout(); //forzo ridisegno





    //GESTISCO L'AZIONE DEL BOTTONE "Aggiungi Liquidazione"
    Ext.getCmp('btnAggiungi').on('click', function () {
        var rows = Ext.getCmp('GridCapitoli').getSelectionModel().getSelections();
        if (rows.length != 1) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Selezionare un capitolo',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) { return; }
            });
            return;
        }
        if (rows[0].data.CodiceRisposta > 0) {
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: 'Il capitolo selezionato è bloccato',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.WARNING
            });
            return;
        } else {
            if ((Ext.get('ImpDisp').dom.value == 0) || (Ext.get('ImpDisp').dom.value == '')) {
                Ext.MessageBox.show({
                    title: 'Gestione Liquidazione',
                    msg: 'Selezionare un valore dalla lista degli Impegni',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function (btn) { return; }
                });
            } else {
                if ((Ext.get('ComboPdCFImpegni').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboPdCFImpegni').dom.value == '')) {
                    Ext.MessageBox.show({
                        title: 'Piano dei Conti Finanziario',
                        msg: 'Il campo Piano dei Conti Finanziario non &egrave; stato valorizzato!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) { return; }
                    });
                } else {
                    var listaBeneficiariSelezionati = Ext.getCmp("GridBeneficiariImpegno").getSelectionModel().getSelections();
                    if (listaBeneficiariSelezionati.length <= 0) {
                        Ext.MessageBox.show({
                            title: 'Attenzione',
                            msg: 'E\' necessario selezionare almeno un beneficiario dalla lista',
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.ERROR,
                            fn: function(btn) { return; }
                        });
                    } else {
                        var listaMessaggiErrore = [];
                        for (var i = 0; i < listaBeneficiariSelezionati.length; i++) {
                            var beneficiario = listaBeneficiariSelezionati[i].data;
                            var sede = beneficiario.ListaSedi[0];
                            if (sede != undefined) {
                                var idModalitaPagamento = sede.IdModalitaPagamento;
                                if (idModalitaPagamento == "" || idModalitaPagamento == -1) {
                                    listaMessaggiErrore.push('Beneficiario ' + beneficiario.Denominazione + ': la tipologia di pagamento non &egrave; stata valorizzata!');
                                } else if (idModalitaPagamento == 32) {
                                    listaMessaggiErrore.push('Beneficiario ' + beneficiario.Denominazione + ': la tipologia di pagamento può essere "Nessuna modalità di pagamento"!');
                                } else if (beneficiario.ImportoDaLiquidare <= 0) {
                                        listaMessaggiErrore.push('Beneficiario ' + beneficiario.Denominazione + ': l\'importo da liquidare specificato è inferiore o uguale zero!');
                                } else if (beneficiario.ImportoDaLiquidare > beneficiario.ImportoResiduoSuImpegno) {
                                    listaMessaggiErrore.push('Beneficiario ' + beneficiario.Denominazione + ': l\'importo da liquidare specificato è maggiore dell\'importo originario disponibile!');
                                }
                            }
                        }//fine for listaBeneficiariSelezionati
                        if (listaMessaggiErrore.length > 0) {
                            var messaggioErrore = "<br/>";
                            for (var j = 0; j < listaMessaggiErrore.length; j++) {
                                messaggioErrore = messaggioErrore + "<br/> <br/>" + listaMessaggiErrore[j];
                            }

                            Ext.MessageBox.show({
                                title: 'Errore Dati Beneficiari',
                                msg: messaggioErrore,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.ERROR,
                                fn: function(btn) { return; }
                            });
                        } else {
                            generaLiquidazioneUP(listaBeneficiariSelezionati);
                        }
                    }

                } //fine else  dell'if ComboPdCFImpegni			 
            } //fine else
        } //fine else CodiceRisposta
    });         //FINE ON CLICK


    //function generaLiquidazioneUP(hiddenBeneficiario, idModalitaPagamento, modalitaPagamento) {
    function generaLiquidazioneUP(listaBeneficiari) {
        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        //hiddenBeneficiario = Ext.util.JSON.encode(rec.data.Beneficiario);
        //var beneficiarioOBJ = Ext.util.JSON.decode(hiddenBeneficiario);
        //var beneficiarioOBJ = Ext.util.JSON.decode(listaBeneficiari);
        //if (beneficiarioOBJ != undefined && beneficiarioOBJ.ListaSedi != undefined && beneficiarioOBJ.ListaSedi.length > 0) {
        //    beneficiarioOBJ.ListaSedi[0].IdModalitaPagamento = idModalitaPagamento;
        //    beneficiarioOBJ.ListaSedi[0].ModalitaPagamento = modalitaPagamento.data.Descrizione;
        //}
        //var beneficiarioEncoded = Ext.util.JSON.encode(beneficiarioOBJ);
        //var beneficiarioEncoded = listaBeneficiari;
        //Ext.get('ListaBeneficiariDaLiquidare').dom.value = beneficiarioEncoded;

        var json = '';
        for (var i = 0; i < listaBeneficiari.length; i++) {
            json += Ext.util.JSON.encode(listaBeneficiari[i].data) + ',';
        }
        json = json.substring(0, json.length - 1);
        
        Ext.get('ListaBeneficiariDaLiquidare').dom.value = json;

        formCapitoliLiquidazione.getForm().timeout = 100000000;
        formCapitoliLiquidazione.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneLiquidazioneUP' + window.location.search,
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure: function (result, response) {
                var lstr_messaggio = '';
                try {
                    lstr_messaggio = response.result.FaultMessage;
                } catch (ex) {
                    lstr_messaggio = 'Errore Generale';
                }
                Ext.MessageBox.show({
                    title: 'Gestione Liquidazione',
                    msg: lstr_messaggio,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function (btn) {
                        Ext.getCmp('GridLiquidazione').getStore().reload();
                        popupLiquidazione.close();
                        return;
                    }
                });
            }, // FINE FAILURE
            success: function (result, response) {
                var msg = 'Generazione Liquidazione effettuata con successo!';
                var progLiq = "";
                try {
                    progLiq = response.result.progLiquidazione;
                    Ext.getCmp('GridLiquidazione').getStore().reload();

                } catch (ex) {
                }
                Ext.MessageBox.show({
                    title: 'Gestione Liquidazione',
                    msg: msg,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.INFO,
                    fn: function (btn) {
                        var storeGridLiq = Ext.getCmp('GridLiquidazione').getStore();
                        if (isDocumentoConFatture) {
                            var liquidazione = undefined;
                            storeGridLiq.each(function (rec) {
                                if (progLiq == rec.data.ID) {
                                    liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };
                                    var index = Ext.getCmp('GridLiquidazione').getStore().indexOf(rec);
                                    Ext.getCmp('GridLiquidazione').getSelectionModel().selectRow(index);
                                    Ext.getCmp('GridLiquidazione').getView().refresh();
                                }
                            });
                            if (liquidazione != undefined) {
                                var grid = buildPanelFattureLiquidazioneNonCont(liquidazione);
                                Ext.getCmp('myPanelLiq').add(grid);
                                Ext.getCmp('myPanelLiq').doLayout();
                            }
                        } else {
                            var listaBeneficiariDaLiquidare = Ext.util.JSON.decode("[" + Ext.get('ListaBeneficiariDaLiquidare').dom.value+ "]");
                            
                            if (listaBeneficiariDaLiquidare != undefined && listaBeneficiariDaLiquidare.length > 0) {
                                //storeGridLiq.each(function (rec) {
                                    //if (progLiq == rec.data.ID) {
                                    //    showGridBeneficiari(rec);
                                    //}
                                //});
                            } else {
                                Ext.MessageBox.show({
                                    title: 'Attenzione',
                                    msg: "Ricordati di inserire il beneficiario sulla liquidazione appena generata.",
                                    buttons: Ext.MessageBox.OK,
                                    icon: Ext.MessageBox.WARNING,
                                    fn: function (btn) {
                                        Ext.getCmp('GridLiquidazione').getStore().reload();
                                        return;
                                    }
                                });
                            }

                        } // fine if (isDocumentoConFatture) {
                        popupLiquidazione.close();
                    } //fine fn: function (btn) {
                }); // fine Ext.MessageBox.show({
            } // FINE SUCCESS
        }); // FINE SUBMIT
    }

    //PRENDO L'ANNO DALLA DATA ODIERNA
    var dtOggi = new Date();
    dtOggi.setDate(dtOggi.getDate());
    var fAnno = dtOggi.getFullYear();

    //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
    var gridCapitoli = buildGridStart(fAnno);
    capitoloPanel.add(gridCapitoli);

    beneficiariPanel.add(buildGridBeneficiariImpegno());


    //VISUALIZZO LA FORM RENDERIZZANDOLA NELLA DIV 
    //formCapitoli.render("ScegliCap");

    popupLiquidazione.show();

    //GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
    var formPanelDropTargetEl = formCapitoliLiquidazione.body.dom;
    var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
        ddGroup: 'gridDDGroup',
        notifyEnter: function (ddSource, e, data) {
            //EFFETTI VISIVI SUL DRAG & DROP
            formCapitoliLiquidazione.body.stopFx();
            formCapitoliLiquidazione.body.highlight();
        },
        notifyDrop: function (ddSource, e, data) {
            // CATTURO IL RECORD SELEZIONATO
            var selectedRecord = ddSource.dragData.selections[0];
            // CARICO IL RECORD NELLA FORM
            formCapitoliLiquidazione.getForm().loadRecord(selectedRecord);

            //LU aggiunta formattazione imp disp 

            Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value);


            // ISTRUZIONI PER IL DRAG --- ASTERISCATE
            // CANCELLO IL RECORD DALLA GRIGLIA.
            //ddSource.grid.store.remove(selectedRecord);
            return (true);
        }
    });

} // fine InitFormCapitoli


function VerificaDisponibilitaLiquidazione() {
    try {

        var comboImpegni = Ext.getCmp('comboImpegni');
        var selectedIndex = comboImpegni.selectedIndex;
        var storeDataSelected = comboImpegni.store.data.get(selectedIndex).data;

        var beneficiario = storeDataSelected.Beneficiario;

        Ext.get('COGSemplice').dom.value = storeDataSelected.Codice_Obbiettivo_Gestionale;
        Ext.get('ComboPdCFImpegni').dom.value = storeDataSelected.PianoDeiContiFinanziario;

        //if (beneficiario != undefined) {
        //    Ext.get('ImpPrenotatoLiq').dom.value = storeDataSelected.ImpDisp;
        //} else {
        //    //Ext.get('ImpPrenotatoLiq').dom.value = 0;
        //}
        
        Ext.get('DataAtto').dom.value = storeDataSelected.DataAtto;
        Ext.get('TipoAtto').dom.value = storeDataSelected.TipoAtto;
        Ext.get('NumeroAtto').dom.value = storeDataSelected.NumeroAtto;
        Ext.get('ImpDisp').dom.value = eurRend(storeDataSelected.ImpDisp);

        Ext.get('Oggetto_Impegno').dom.value = storeDataSelected.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(storeDataSelected.ImpPotenzialePrenotato);

    } catch (ex) {
    }
}

function getListaBeneficiariImpegnoStore() {
    var store = new Ext.data.Store();

    if (listaBeneficiariImpegno != null) {

        var TipoRecord = Ext.data.Record.create([
    			       { name: 'Tipologia' },
                       { name: 'DescrizioneTipologia' },
                       { name: 'CodiceFiscale' },
                       { name: 'Denominazione' },
                       { name: 'Cognome' },
                       { name: 'Nome' },
                       { name: 'DataNascita' },
                       { name: 'PartitaIva' },
                       { name: 'ID' },
                       { name: 'AltriNomi' },
                       { name: 'Commissioni' },
                       { name: 'ComuneNascita' },
                       { name: 'Sesso' },
                       { name: 'ListaSedi' },
                       { name: 'Contratto' },
                       { name: 'PartitaIvaOrCodFiscToView' },
                       { name: 'ImportoOriginarioSuImpegno' },
                       { name: 'ImportoResiduoSuImpegno' },
                       { name: 'ImportoDaLiquidare' },
                       { name: 'IdModalitaPagamentoSelected' }
        ]);

        for (var i = 0; i < listaBeneficiariImpegno.length; i++) {
            var a = 0;
            var record = new TipoRecord({
                ID: listaBeneficiariImpegno[i].ID,
                Tipologia: listaBeneficiariImpegno[i].Tipologia,
                CodiceFiscale: listaBeneficiariImpegno[i].CodiceFiscale,
                PartitaIva: listaBeneficiariImpegno[i].PartitaIva,
                Denominazione: listaBeneficiariImpegno[i].Denominazione,
                Cognome: listaBeneficiariImpegno[i].Cognome,
                Nome: listaBeneficiariImpegno[i].Nome,
                DataNascita: listaBeneficiariImpegno[i].DataNascita,
                ListaSedi: listaBeneficiariImpegno[i].ListaSedi,
                Contratto: listaBeneficiariImpegno[i].Contratto,
                PartitaIvaOrCodFiscToView: listaBeneficiariImpegno[i].PartitaIvaOrCodFiscToView,
                ImportoOriginarioSuImpegno: listaBeneficiariImpegno[i].ImportoOriginarioSuImpegno,
                ImportoResiduoSuImpegno: listaBeneficiariImpegno[i].ImportoResiduoSuImpegno,
                ImportoDaLiquidare: listaBeneficiariImpegno[i].ImportoResiduoSuImpegno,
                IdModalitaPagamentoSelected: listaBeneficiariImpegno[i].IdModalitaPagamentoSelected
        });
            store.insert(i, record);
        }
    }

    return store;
}

function setTestoDatiBeneficiario(beneficiario) {
    if (beneficiario != undefined) {
        //Ext.get('testoBeneficiario').dom.value = beneficiario.Denominazione + ' ' + beneficiario.PartitaIvaOrCodFiscToView + ' Sede: ' + beneficiario.ListaSedi[0].NomeSede + ' - ' + beneficiario.ListaSedi[0].Indirizzo;
    } else {
        //Ext.get('testoBeneficiario').dom.value = '';
    }

}

function buildComboModalitaPagamento() {



    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetTipologiePagamentoSic',
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({
        root: 'GetTipologiePagamentoSicResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' },
           { name: 'Preferiti' },
           { name: 'OrdineApparizione' },
           { name: 'ObbligoCC' },
           { name: 'ObbligoIBAN' },
           { name: 'IstitutoRiferimento' }
        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        autoLoad: true
    });

    store.setDefaultSort("OrdineApparizione", "ASC");

    store.on({
        'load': {
            fn: function (store, records, options) {
                maskApp.hide();
            }
            , scope: this
        }
    });

    store.load({});

    comboModalitaPagamento = new Ext.form.ComboBox({
        displayField: 'Descrizione',
        valueField: 'Id',
        fieldLabel: 'Tipologia pagamento (*)',
        id: 'comboModalitaPagamentoId',
        width: 170,
        listWidth: 600,
        listHeight: 300,
        store: store,
        readOnly: true,
        mode: 'local',
        allowBlank: false,
        blankText: 'Campo Obbligatorio',
        queryMode: 'local',
        triggerAction: 'all',
        emptyText: 'Selezionare una modalità di pagamento ...',
        listeners: {
            select: {
                fn: function (combo, value) {
                }
            }
        },
        style: 'background-color: #fffb8a;background-image:none;'
    });

    return comboModalitaPagamento;
}


function setModalitaPagamento(beneficiario) {
    if (beneficiario != undefined) {
        if (beneficiario.Denominazione.indexOf("NON PRESENTE") == -1) {
            var listaSedi = beneficiario.ListaSedi;
            if (listaSedi != undefined && beneficiario.ListaSedi.length > 0) {
                var idTipoPagamento = listaSedi[0].IdModalitaPagamento;
                if (idTipoPagamento != 32) {
                    comboModalitaPagamento.setValue(idTipoPagamento);
                }
            }
        } else {
            comboModalitaPagamento.setValue("");
        }
    } else {
        comboModalitaPagamento.setValue("");
    }
}

function abilitaLiquidazione(abilita) {
    if (abilita) {
        Ext.getCmp('comboImpegni').enable();
        //Ext.getCmp('ImpPrenotatoLiq').enable();
    } else {
        Ext.getCmp('comboImpegni').clearValue();
        //Ext.getCmp('ImpPrenotatoLiq').clearValue();
        Ext.getCmp('comboImpegni').disable();
        //Ext.getCmp('ImpPrenotatoLiq').disable();
    }
}

//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER Liquidazioni"
function buildGridStart(annoRif) {

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetElencoCapitoliAnnoLiquidazione',
        method: 'GET',
        timeout: 900000

    });
    var reader = new Ext.data.JsonReader({

        root: 'GetElencoCapitoliAnnoLiquidazioneResult',
        fields: [
               { name: 'Bilancio' },
               { name: 'UPB' },
               { name: 'MissioneProgramma' },
               { name: 'Capitolo' },
               { name: 'DescrCapitolo' },
               { name: 'CodiceRisposta' },
               { name: 'DescrizioneRisposta' }
        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
		, reader: reader
        , listeners: {
            'loadexception': function (proxy, options, response) {
                maskApp.hide();
                Ext.MessageBox.show({
                    title: 'ERRORE CAPITOLI',
                    msg: "Errore durante il caricamento dei capitoli:<br>" +
                         "'" + Ext.decode(response.responseText).FaultMessage + "'",
                    buttons: Ext.Msg.OK,
                    closable: false,
                    icon: Ext.MessageBox.ERROR
                });
            }
        }
    });



    store.on({
        'load': {
            fn: function (store, records, options) {
                maskApp.hide();
            },
            scope: this
        }
    });
    var ufficio = Ext.get('Cod_uff_Prop').dom.value;
    //var parametri = { CodiceUfficio: ufficio };

    var parametri = { AnnoRif: annoRif, tipoCapitolo: '2', CodiceUfficio: ufficio };
    store.load({ params: parametri });


    function renderColor(val, p, record) {
        var color = '';
        if (record.data.CodiceRisposta == 0) {
            if (record.data.DescrizioneRisposta.toUpperCase().indexOf("NESSUN") > -1) {
                color = '#298A08';
            } else {
                color = '#FE9A2E';
            }
        }
        else {
            color = 'red';
        }
        return '<p style="font-size:12px;color:' + color + ';">' + record.data.DescrizioneRisposta + '</p>';
    }

    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
                   { header: "Bilancio", width: 60, dataIndex: 'Bilancio', id: 'Bilancio', sortable: true },
                   { header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true, locked: false },
                   { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                  { header: "Missione Programma", width: 80, dataIndex: 'MissioneProgramma', sortable: true },
                   { header: "Descrizione", width: 180, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
                   { header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
    ]);

    var grid = new Ext.grid.GridPanel({
        id: 'GridCapitoli',
        autoExpandColumn: 'Capitolo',
        // da mettere sempre 
        height: 300,
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
            singleSelect: true,
            listeners: {
                rowselect: function (sm, row, rec) { }
            }
        })
    });

    //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
    grid.addListener({
        'rowdblclick': {
            fn: function (grid, rowIndex, event) {

                maskApp.show();

                var rec = grid.store.getAt(rowIndex);
                Ext.getCmp("ElencoCapitoli").getForm().loadRecord(rec);

                var ufficio = Ext.get('Cod_uff_Prop').dom.value;
                var annoRif = rec.data['Bilancio'];
                var capitoloRif = rec.data['Capitolo'];

                Ext.get('Capitolo').dom.value = capitoloRif;
                Ext.get('Bilancio').dom.value = annoRif;
                Ext.get('UPB').dom.value = rec.data['UPB'];
                Ext.get('MissioneProgramma').dom.value = rec.data['MissioneProgramma'];

                var paramsStoreImpegni = {
                    AnnoRif: annoRif,
                    CapitoloRif: capitoloRif,
                    CodiceUfficio: ufficio
                };

                storeImpegni.reload({ params: paramsStoreImpegni });

                var parametriStorePCF = { AnnoRif: annoRif, CapitoloRif: capitoloRif };
                storePdCF.load({ params: parametriStorePCF });

                //resettare i dati nella gridBeneficiariImpegno
                var gridBeneficiariImpegno = Ext.getCmp("GridBeneficiariImpegno");
                if (gridBeneficiariImpegno != undefined) {
                    gridBeneficiariImpegno.getStore().removeAll();
                }

                Ext.get('comboImpegni').dom.value = '';
                Ext.get('Oggetto_Impegno').dom.value = '';

                Ext.get('ImpDisp').dom.value = '';
                Ext.get('DataAtto').dom.value = '';
                Ext.get('NumeroAtto').dom.value = '';
                Ext.get('TipoAtto').dom.value = '';

                Ext.get('ImpPotenzialePrenotato').dom.value = '';
                //Ext.get('ImpPrenotatoLiq').dom.value = '';
                Ext.get('ComboPdCFImpegni').dom.value = '';
                Ext.get('COGSemplice').dom.value = '';

                

                if (rec.data['ImpDisp'] != "0") {
                    Ext.get('ImpDisp').dom.value = eurRend("0");
                } else {
                    Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value);
                }
                abilitaLiquidazione(true);
            }, scope: this
        }
    });

    return grid;
}

//FUNZIONE CHE RIEMPIE IL CAMPO "DISPONIBILITA' IMPEGNO" NEL PANEL "NUMERO IMPEGNO"
//function getDettaglioImpegno(AnnoRif, CapitoloRif, NumeroImpegno) {

//    var ufficio = Ext.get('Cod_uff_Prop').dom.value;
//    //var parametri = { CodiceUfficio: ufficio };


//    var params = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif, NumeroImpegno: NumeroImpegno, CodiceUfficio: ufficio };

//    Ext.lib.Ajax.defaultPostHeader = 'application/json';
//    Ext.Ajax.request({
//        url: 'ProcAmm.svc/GetDettaglioImpegno',
//        params: Ext.encode(params),
//        method: 'POST',
//        success: function (response, options) {
//            maskApp.hide();
//            var data = Ext.decode(response.responseText);
//            Ext.get('COGSemplice').dom.value = data.Codice_Obbiettivo_Gestionale;
//            Ext.get('ImpDisp').dom.value = data.ImpDisp;
//            Ext.get('Bilancio').dom.value = data.Bilancio;
//            Ext.get('UPB').dom.value = data.UPB;
//            Ext.get('MissioneProgramma').dom.value = data.MissioneProgramma;
//            Ext.get('Capitolo').dom.value = data.Capitolo;
//            Ext.get('ImpPrenotatoLiq').dom.value = data.ImpPrenotato;
//            Ext.get('TipoAtto').dom.value = data.TipoAtto;
//            Ext.get('NumeroAtto').dom.value = data.NumeroAtto;
//            Ext.get('DataAtto').dom.value = data.DataAtto;
//            Ext.get('Oggetto_Impegno').dom.value = data.Oggetto_Impegno;
//            Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value);
//            Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(Ext.get('ImpPotenzialePrenotato').dom.value);

//        },
//        failure: function (response, result) {
//            maskApp.hide();
//            var data = Ext.decode(response.responseText);
//            Ext.MessageBox.show({
//                title: 'Errore',
//                msg: data.FaultMessage,
//                buttons: Ext.MessageBox.OK,
//                icon: Ext.MessageBox.ERROR,
//                fn: function (btn) { return }
//            });
//        }
//    });
//}
//FUNZIONE CHE CONFERMA I DATI di LIQUIDAZIONE INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaLiquidazione(LiquidazioniInfo) {
    var params = { LiquidazioniInfo: LiquidazioniInfo };

    maskApp.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaLiquidazione',
        params: Ext.encode(params),
        method: 'POST',
        success: function (result, response, options) {
            maskApp.hide();
            Ext.getCmp('GridLiquidazione').getStore().reload();
            actionDeleteLiquidazione.setDisabled(true);
            actionCompilaRiduzioni.setDisabled(true);
            actionConfermaLiquidazione.setDisabled(true);
            actionBeneficiari.setDisabled(true);
            actionShowFattureLiquidazioneNonCont.setDisabled(true);
            actionAddLiquidazione.setDisabled(false);
        },
        failure: function (response, result, options) {
            maskApp.hide();
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) {
                    if (data.FaultCode == 2) { //this fault code means that at least a confirmation is succeded
                        Ext.getCmp('GridLiquidazione').getStore().reload();
                        actionDeleteLiquidazione.setDisabled(true);
                        actionCompilaRiduzioni.setDisabled(true);
                        actionConfermaLiquidazione.setDisabled(true);
                        actionBeneficiari.setDisabled(true);
                        actionShowFattureLiquidazioneNonCont.setDisabled(true);
                        actionAddLiquidazione.setDisabled(false);
                    }
                }
            });
        }
    });
}

function ConfermaLiquidazione(ID) {
    var params = { ID: ID };
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaLiquidazione',
        params: Ext.encode(params),
        method: 'POST',
        success: function (result, response, options) {
            //maskApp.hide();
            Ext.getCmp('GridLiquidazione').getStore().reload();
        },
        failure: function (response, result, options) {
            //mask.hide();
            //Ext.decode(result.responseText).FaultMessage
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
            actionDeleteLiquidazione.setDisabled(false);
            actionConfermaLiquidazione.setDisabled(true);
        }
    });
}

function NascondiColonneLiq() {
    if (Ext.getCmp('GridLiquidazione') != undefined) {
        var index = Ext.getCmp('GridLiquidazione').colModel.getColumnCount(false);
        index = index - 1;
        Ext.getCmp('GridLiquidazione').colModel.setHidden(index, true);
        actionConfermaLiquidazione.hide();
    }
}

