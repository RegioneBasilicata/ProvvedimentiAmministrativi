var isDocumentoConFatture = false;
var comboModalitaPagamentoImpegnoPerente;
var comboListaBeneficiariImpegnoPerente;
var listaBeneficiariImpegnoPerente;

var maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


var storeImpegniPer = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaImpegniPerentiAperti',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaImpegniPerentiApertiResult',
        fields: [
          { name: 'NumImpegno' },
          { name: 'ImpDisp' },
          { name: 'ImportoOriginario' },
          { name: 'CapitoloOriginario' },
          { name: 'Oggetto_Impegno' },
          { name: 'ImpPotenzialePrenotato' },
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

storeImpegniPer.setDefaultSort("NumImpegno", "DESC");

var storePdCFImpegniPer = new Ext.data.Store({
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
    }),
    listeners: {
        beforeload: function (store, options) {
            maskApp.show();
        },
        load: function (store, records, options) {
            maskApp.hide();
        }
    }
});

var storeCOGImpegniPer = new Ext.data.Store({
    proxy: new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaObiettiviGestionali',
        method: 'GET'
    }),
    reader: new Ext.data.JsonReader({
        root: 'GetListaObiettiviGestionaliResult',
        fields: [
            { name: 'Id' },
            { name: 'Descrizione' }
        ]
    }),
    listeners: {
        beforeload: function (store, options) {
            maskApp.show();
        },
        load: function (store, records, options) {
            maskApp.hide();
        }
    }
});

function showGridBeneficiariPerenti(recLiquidazione, recImpegno) {
    if (recLiquidazione != undefined) {
        var liquidazione = { ID: recLiquidazione.data.ID, Capitolo: recLiquidazione.data.Capitolo, Bilancio: recLiquidazione.data.Bilancio, ImpPrenotatoLiq: recLiquidazione.data.ImpPrenotatoLiq };
        var gridBen = BeneficiariLiquidazioneImpegno(liquidazione, undefined, "GridBeneficiariPerenti", "GridLiqPerenti");

        Ext.getCmp('myPanelLiqPerentiContestuali').add(gridBen);
        Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
    }
    if (recImpegno != undefined) {
        var impegno = { ID: recImpegno.data.ID, Capitolo: recImpegno.data.Capitolo, Bilancio: recImpegno.data.Bilancio, ImpPrenotatoLiq: recImpegno.data.ImpPrenotatoLiq };
        var grid = BeneficiariLiquidazioneImpegno(undefined, impegno, "GridBeneficiariImpegnoPerenti", "GridPerenti");

        Ext.getCmp('myPanelPerenti').add(grid);
        Ext.getCmp('myPanelPerenti').doLayout();
    }

}


//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE LE FATTURE DELLA LIQUIDAZIONE SELEZIONATA
function EliminaFatturaDaLiquidazionePerenti(listaFatture, statoOperazione, idLiquidazione) {

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
                Ext.getCmp('GridFattureLiquidazionePerenti').getStore().reload();
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
function registraFatturaLiquidazionePerenti(statoOperazione, idLiquidazione, listaFatture) {

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

                Ext.getCmp('GridFattureLiquidazionePerenti').store.add(newRecordFattura);
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

function buildPopUpFattureLiquidazionePerenti(liquidazione, importoTotaleFattureLiq) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

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
                    if (Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti') != null && Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti') != undefined)
                        Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti').disable();
                } else {
                    var gridFattureLiquidazionePerenti = Ext.getCmp('GridPopUpFattureLiquidazionePerenti');
                    if (gridFattureLiquidazionePerenti != undefined) {
                        gridFattureLiquidazionePerenti.getSelectionModel().selectAll();
                        gridFattureLiquidazionePerenti.getView().refresh();
                    }
                }
                maskApp.hide();
            },
            scope: this
        }
    });

    maskApp.show();

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
                    var totalRows = Ext.getCmp('GridPopUpFattureLiquidazionePerenti').store.getRange().length;
                    var selectedRows = selectionModel.getSelections();
                    if (selectedRows.length > 0) {

                        Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti').enable();
                    }
                    if (totalRows == selectedRows.length) {

                        if (Ext.getCmp('GridPopUpFattureLiquidazionePerenti') != undefined) {
                            var view = Ext.getCmp('GridPopUpFattureLiquidazionePerenti').getView();
                            var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                            chkdiv.addClass("x-grid3-hd-checker-on");
                        }

                    }
                }
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();

                if (selectedRows.length == 0) {
                    Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti').disable();
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
        id: 'GridPopUpFattureLiquidazionePerenti',
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

function showPopupPanelFattureLiquidazionePerenti(liquidazione, importoTotaleFattureLiq) {
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
            id: 'btnConfermaSelezioneFatturaLiqPerenti'
        }, {
            text: 'Chiudi',
            id: 'btnChiudiSelezioneFatturaLiqPerenti'
        }]
    });

    var gridFattureDocumento = buildPopUpFattureLiquidazionePerenti(liquidazione, importoTotaleFattureLiq);

    if (gridFattureDocumento != null && gridFattureDocumento != undefined)
        popup.add(gridFattureDocumento);


    popup.doLayout();
    popup.show();

    Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti').disable();

    Ext.getCmp('btnConfermaSelezioneFatturaLiqPerenti').on('click', function () {

        var fatture = Ext.getCmp('GridPopUpFattureLiquidazionePerenti').getSelectionModel().getSelections();
        var residuoLiquidazione = liquidazione.ImpPrenotatoLiq - importoTotaleFattureLiq;
        residuoLiquidazione = parseFloat(residuoLiquidazione).toFixed(2);
        var listaFatture = [];
        if (fatture.length > 0) {
            var errore = false;
            var messaggioErrore = "";
            var totaleImportiDaLiquidare = 0;
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
                    registraFatturaLiquidazionePerenti(statoOperazione, liquidazione.ID, listaFatture);

                    //                        var gridFatture = Ext.getCmp('GridFattureLiquidazionePerenti');
                    //                        var storeGridFatture = gridFatture.getStore();
                    //                        storeGridFatture.reload();
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


    Ext.getCmp('btnChiudiSelezioneFatturaLiqPerenti').on('click', function () {
        if (Ext.getCmp('GridFattureLiquidazionePerenti') != undefined) {
            Ext.getCmp('GridFattureLiquidazionePerenti').getStore().reload();
            var totalRows = Ext.getCmp('GridFattureLiquidazionePerenti').store.getRange().length;
            if (totalRows == 0) {
                Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('panelFatturaLiquidazionePerenti'));
                Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
            }
        }
        popup.close();
    });

    popup.on('close', function () {
        var gridFatture = Ext.getCmp('GridPopUpFattureLiquidazionePerenti');
        gridFatture.getView().refresh();

    });
}

function buildPanelFattureLiquidazionePerenti(liquidazione) {

    var panelFatturaLiquidazionePerenti = new Ext.Panel({
        id: 'panelFatturaLiquidazionePerenti',
        autoHeight: true,
        xtype: 'panel',
        border: false,
        layout: 'table',
        style: 'margin-top:15px;',

        layoutConfig: {
            columns: 1
        },
        items: [
            buildPanelGridFattureLiquidazionePerenti(liquidazione),
            {
                xtype: 'hidden',
                id: 'listaFattureLiquidazionePerenti'
            }
        ]
    });
    return panelFatturaLiquidazionePerenti;
}

function buildPanelGridFattureLiquidazionePerenti(liquidazione) {

    var aggiungiFatturaLiquidazionePerenti = new Ext.Action({
        text: 'Aggiungi',
        id: 'actionAggiungiFatturaLiquidazionePerenti',
        tooltip: 'Seleziona e aggiunge uno più fatture alla liquidazione selezionata',
        handler: function () {
            //stef
            var importoTotaleFattureLiq = 0;
            var gridFatture = Ext.getCmp('GridFattureLiquidazionePerenti');
            if (gridFatture != undefined) {
                var storeGridFatture = Ext.getCmp('GridFattureLiquidazionePerenti').getStore();
                storeGridFatture.each(function (rec) {
                    importoTotaleFattureLiq = importoTotaleFattureLiq + rec.data.ImportoLiquidato;
                });
                showPopupPanelFattureLiquidazionePerenti(liquidazione, importoTotaleFattureLiq);
            }
        },
        iconCls: 'add'
    });

    var rimuoviFatturaLiquidazionePerenti = new Ext.Action({
        text: 'Rimuovi',
        id: 'actionRimuoviFatturaLiquidazionePerenti',
        tooltip: 'Rimuove una o più fatture dalla liquidazione selezionata',

        handler: function () {
            Ext.MessageBox.buttonText.cancel = "Annulla";
            var gridFatture = Ext.getCmp('GridFattureLiquidazionePerenti');
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
                        var gridFatture = Ext.getCmp('GridFattureLiquidazionePerenti');
                        Ext.each(gridFatture.getSelectionModel().getSelections(), function (rec) {
                            if (rec.data['IdUnivoco'] != 0) {
                                listaFatture.push(rec.data);
                            }
                        })

                        EliminaFatturaDaLiquidazionePerenti(listaFatture, 'C', liquidazione.ID);

                        var gridFatture = Ext.getCmp('GridFattureLiquidazionePerenti');
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
                            rimuoviFatturaLiquidazionePerenti.disable();
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
                    aggiungiFatturaLiquidazionePerenti.execute();
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
                var totalRows = Ext.getCmp('GridFattureLiquidazionePerenti').store.getRange().length;
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    rimuoviFatturaLiquidazionePerenti.enable();
                }
                if (totalRows == selectedRows.length) {
                    var view = Ext.getCmp('GridFattureLiquidazionePerenti').getView();
                    var chkdiv = Ext.fly(view.innerHd).child(".x-grid3-hd-checker");
                    chkdiv.addClass("x-grid3-hd-checker-on");
                }
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length == 0) {
                    rimuoviFatturaLiquidazionePerenti.disable();
                }
                var view = Ext.getCmp('GridFattureLiquidazionePerenti').getView();
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
                         retValue = record.data.AnagraficaInfo.Nome + " " + record.data.AnagraficaInfo.Cognome
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
        id: 'GridFattureLiquidazionePerenti',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        xtype: "form",
        autoHeight: false,
        height: 230,
        border: true,
        ds: store,
        cm: ColumnModel,
        width: 740,
        stripeRows: true,
        loadMask: true,
        title: "Fatture relative alla liquidazione selezionata",
        tbar: [aggiungiFatturaLiquidazionePerenti, rimuoviFatturaLiquidazionePerenti],
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

    rimuoviFatturaLiquidazionePerenti.disable();

    return gridFatture;
}


//DEFINISCO L'AZIONE CONFERMA DELLA GRIGLIA
var actionConfermaImpegnoPerente = new Ext.Action({
    text: 'Conferma',
    tooltip: 'Conferma i dati delle righe selezionate',
    handler: function () {
        var ImpegniPerentiDaComfermare = new Array();

        Ext.each(Ext.getCmp('GridPerenti').getSelectionModel().getSelections(), function (rec) {
            //conferma solo gli impegni perenti da confermare, quelli aventi stato 2
            if (rec.data['Stato'] == 2) {
                var impegnoPerenteInfo = new Object();

                impegnoPerenteInfo.ID = rec.data['ID'];
                impegnoPerenteInfo.Stato = rec.data['Stato'];
                impegnoPerenteInfo.NumPreImp = rec.data['NumPreImp'];

                ImpegniPerentiDaComfermare.push(impegnoPerenteInfo);
            }
        });

        ConfermaMultiplaImpegnoPerente(ImpegniPerentiDaComfermare);
    },
    iconCls: 'save'
});

//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
function buildGridBeneficiariImpegnoPerente() {

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: false,
        loadMask: false,
        listeners: {
            rowselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    Ext.getCmp('btnAggiungiPerente').enable();
                } else {
                    Ext.getCmp('btnAggiungiPerente').disable();
                }
            },
            rowdeselect: function (selectionModel, rowIndex, record) {
                var selectedRows = selectionModel.getSelections();
                if (selectedRows.length > 0) {
                    Ext.getCmp('btnAggiungiPerente').enable();
                } else {
                    Ext.getCmp('btnAggiungiPerente').disable();
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
                    var comboModalitaPagamentoImpegnoPerenteId = Ext.getCmp('comboModalitaPagamentoImpegnoPerenteId');
                    
                    var idModalitaPagamento = record.data.ListaSedi[0].IdModalitaPagamento;
                    var idModalitaPagamentoNew = comboModalitaPagamentoImpegnoPerenteId.getValue();
                    if (idModalitaPagamentoNew != "") {
                        var modalitaPagamentoNew = comboModalitaPagamentoImpegnoPerenteId.findRecord(comboModalitaPagamentoImpegnoPerenteId.valueField || comboModalitaPagamentoImpegnoPerenteId.displayField, idModalitaPagamentoNew);
                        descrizioneModalitaPagamento = modalitaPagamentoNew.data.Descrizione;
                        record.data.ListaSedi[0].IdModalitaPagamento = idModalitaPagamentoNew;
                    } else {
                        if (idModalitaPagamento != undefined) {
                            var modalitaPagamento = comboModalitaPagamentoImpegnoPerenteId.findRecord(comboModalitaPagamentoImpegnoPerenteId.valueField || comboModalitaPagamentoImpegnoPerenteId.displayField, idModalitaPagamento);
                            if (modalitaPagamento != undefined)
                                descrizioneModalitaPagamento = modalitaPagamento.data.Descrizione;
                        }
                    }
                    return descrizioneModalitaPagamento;

                },
                editor: buildComboModalitaPagamentoImpegnoPerente()
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
                header: "Da Impegnare e Liquidare", width: 80, dataIndex: 'ImportoDaLiquidare', sortable: true, locked: false,
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
        id: 'GridBeneficiariImpegnoPerenti',
        name: 'GridBeneficiariImpegnoPerenti',
        stripeRows: true,
        autoHeight: false,
        ds: getListaBeneficiariImpegnoPerenteStore(),
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

var actionAddPerente = new Ext.Action({
    id: 'action-add-perenti',
    text: 'Aggiungi',
    tooltip: 'Aggiungi una nuova riga',
    handler: function () {
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
                        InitFormPerenti();
                    }
                }
            });
        } else {
            InitFormPerenti();
        }
    },
    iconCls: 'add'
});


var actionBeneficiariImpPer = new Ext.Action({
    text: 'Beneficiari',
    tooltip: 'Aggiunge, Elimina e Modifica i Beneficiari dell\'impegno selezionato',
    handler: function () {
        Ext.each(Ext.getCmp('GridPerenti').getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridBeneficiariImpegnoPerenti') != undefined) {
                Ext.getCmp('myPanelPerenti').remove(Ext.getCmp('GridBeneficiariImpegnoPerenti'));
                Ext.getCmp('myPanelPerenti').doLayout();
            } else {
                showGridBeneficiariPerenti(undefined, rec);
            }
        });
    },
    iconCls: 'coin'
});

//DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA
var actionDeletePerente = new Ext.Action({
    text: 'Cancella',
    tooltip: 'Cancella selezionato',
    handler: function () {
        var storeGridImp = Ext.getCmp('GridPerenti').getStore();
        var GridImpegni = Ext.getCmp('GridPerenti');
        Ext.each(GridImpegni.getSelectionModel().getSelections(), function (rec) {
            EliminaPreImpegnoPerente(rec.data['NumPreImp'], rec.data['ID']);
            storeGridImp.remove(rec);
        })
        actionAddPerente.setDisabled(false);
        actionDeletePerente.setDisabled(true);
        actionBeneficiariImpPer.setDisabled(true);
        actionModificaCOGAndPdCFImpPerente.setDisabled(true);
    },
    iconCls: 'remove'
});

//DEFINISCO L'AZIONE per la gestione delle fatture delle liquidazioni
var actionShowFattureLiquidazionePerenti = new Ext.Action({
    text: 'Fatture',
    tooltip: 'Aggiunge le Fatture della liquidazione selezionata',
    handler: function () {

        Ext.each(Ext.getCmp('GridLiqPerenti').getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridFattureLiquidazionePerenti') != undefined) {
                Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridFattureLiquidazionePerenti'));
                Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
            } else {
                var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };

                var grid = buildPanelFattureLiquidazionePerenti(liquidazione);
                Ext.getCmp('myPanelLiqPerentiContestuali').add(grid);
                Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
            }
        });
    },
    iconCls: 'add'
});

//DEFINISCO L'AZIONE per la gestione dei beneficiari
var actionBeneficiari3 = new Ext.Action({
    text: 'Beneficiari',
    tooltip: 'Aggiunge, Elimina e Modifica i Beneficiari della liquidazione selezionata',
    handler: function () {
        Ext.each(Ext.getCmp('GridLiqPerenti').getSelectionModel().getSelections(), function (rec) {
            if (Ext.getCmp('GridBeneficiariPerenti') != undefined) {
                Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridBeneficiariPerenti'));
                Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
            } else {
                var liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };
                var grid = BeneficiariLiquidazioneImpegno(liquidazione, undefined, "GridBeneficiariPerenti", "GridLiqPerenti");

                Ext.getCmp('myPanelLiqPerentiContestuali').add(grid);
                Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
            }
        });
    },
    iconCls: 'coin'
});




//PopUp Modifica COG/PdCF   
function generaFormCOGAndPdCFImpPerente(idProg, codValue, codPdCFValue, AnnoRif, CapitoloRif) {

    //Piano Dei Conti Finanziario combobox
    var proxyPdCF = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetPianoDeiContiFinanziari',
        method: 'GET'
    });

    var readerPdCF = new Ext.data.JsonReader({
        root: 'GetPianoDeiContiFinanziariResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }

        ]
    });

    var storePdCF = new Ext.data.Store({
        proxy: proxyPdCF,
        reader: readerPdCF
    });

    storePdCF.setDefaultSort("Descrizione", "ASC");

    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
    storePdCF.load({ params: parametri });

    var ComboPdCF = new Ext.form.ComboBox({
        fieldLabel: 'Piano dei Conti Finanziario(*)',
        displayField: 'Id',
        valueField: 'Id',
        listWidth: 300,
        width: 300,
        queryMode: 'local',
        store: storePdCF,
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
        readOnly: true,
        disabled: false,
        name: 'ComboPdCF',
        id: 'ComboPdCFImp',
        mode: 'local',
        triggerAction: 'all',
        emptyText: 'Seleziona Piano dei Conti Finanziario...'
    });


    //COG combobox
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaObiettiviGestionali',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetListaObiettiviGestionaliResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }

        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("Descrizione", "ASC");

    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
    store.load({ params: parametri });

    var ComboCOGImp = new Ext.form.ComboBox({
        fieldLabel: 'Codice Obiettivo Gestionale(*)',
        displayField: 'Id',
        valueField: 'Id',
        listWidth: 300,
        width: 300,
        queryMode: 'local',
        store: store,
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
        readOnly: true,
        disabled: false,
        name: 'ComboCOG',
        id: 'ComboCOGImp',
        mode: 'local',
        triggerAction: 'all',
        emptyText: 'Seleziona Codice Obiettivo Gestionale...'
    });

    var FormDetailCOGAndPdCF = new Ext.FormPanel({
        id: 'FormDetailCOGAndPdCF',
        frame: true,
        labelAlign: 'left',
        border: false,
        collapsible: true,
        width: 455,
        items: [{
            xtype: 'fieldset',
            id: 'detCapitolo',
            labelWidth: 100,
            defaultType: 'textfield',
            autoHeight: true,
            border: false,
            items: [
        {
            fieldLabel: 'Bilancio',
            name: 'Bilancio',
            id: 'Bilancio',
            readOnly: true,
            enable: false,
            value: AnnoRif
        }, {
            fieldLabel: 'Capitolo',
            name: 'Capitolo',
            id: 'Capitolo',
            readOnly: true,
            enabled: false,
            value: CapitoloRif
        },
        ComboCOGImp,
        ComboPdCF,
        {
            xtype: 'hidden',
            name: 'id_prog',
            value: idProg
        }]
        }
        ]
    });

    var popup = new Ext.Window({
        title: 'Modifica COG/Piano dei Conti Finanziario',
        width: 455,
        height: 230,
        layout: 'fit',
        plain: true,
        buttonAlign: 'right',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        closable: true,
        buttons: [{
            text: 'Salva',
            id: 'btnSalvaCOGAndPdCF'
        },
                        {
                            text: 'Annulla',
                            id: 'btnAnnullaCOGAndPdCF'
                        }]
    });

    popup.add('FormDetailCOGAndPdCF');

    Ext.getCmp('ComboCOGImp').setValue(codValue);
    Ext.getCmp('ComboPdCFImp').setValue(codPdCFValue);

    popup.doLayout();

    Ext.getCmp('btnAnnullaCOGAndPdCF').on('click', function () {
        popup.close();
    });

    Ext.getCmp('btnSalvaCOGAndPdCF').on('click', function () {

        if ((Ext.get('ComboCOGImp').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboCOGImp').dom.value == '')) {
            Ext.MessageBox.show({
                title: 'COG',
                msg: 'Il campo Codide Obiettivo Gestionale non &egrave; stato valorizzato!',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
        } else if ((Ext.get('ComboPdCFImp').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboPdCFImp').dom.value == '')) {
            Ext.MessageBox.show({
                title: 'Piano dei Conti Finanziario',
                msg: 'Il campo Piano dei Conti Finanziario non &egrave; stato valorizzato!',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) { return }
            });
        } else {
            Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
            FormDetailCOGAndPdCF.getForm().timeout = 100000000;
            FormDetailCOGAndPdCF.getForm().submit({
                url: 'ProcAmm.svc/AssegnaCOGePdCF',
                waitTitle: "Attendere...",
                waitMsg: 'Aggiornamento in corso ......',
                failure:
    function (result, response) {
        var lstr_messaggio = ''
        try {
            lstr_messaggio = response.result.FaultMessage
        } catch (ex) {
            lstr_messaggio = 'Errore Generale';
        }
        Ext.MessageBox.show({
            title: 'Modifica COG/Piano dei Conti Finanziario',
            msg: lstr_messaggio,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.ERROR,
            fn: function (btn) {
                return;
            }
        });
    }, // FINE FAILURE
                success:
    function (result, response) {
        var msg = 'Operazione effettuata con successo!';
        Ext.MessageBox.show({
            title: 'Modifica COG/Piano dei Conti Finanziario',
            msg: msg,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.INFO,
            fn: function (btn) {
                Ext.getCmp('GridPerenti').getStore().reload();
                popup.close();

                actionDeletePerente.setDisabled(true);
                actionBeneficiariImpPer.setDisabled(true);
                actionConfermaImpegnoPerente.setDisabled(true);
                actionModificaCOGAndPdCFImpPerente.setDisabled(true);
                actionAddPerente.setDisabled(false);
            }
        });
    } // FINE SUCCESS

            }) // FINE SUBMIT
        }

    }) //fine onclick
    popup.show();
}


var actionModificaCOGAndPdCFImpPerente = new Ext.Action({
    text: 'Modifica COG/Piano dei Conti Finanziario',
    tooltip: 'Modifica COG e/o Piano dei Conti Finanziario',
    handler: function () {

        var GridImpegni = Ext.getCmp('GridPerenti');
        var flag = true;
        Ext.each(GridImpegni.getSelectionModel().getSelections(), function (rec) {
            if (flag) {
                generaFormCOGAndPdCFImpPerente(rec.data.ID, rec.data.Codice_Obbiettivo_Gestionale,
                                     rec.data.PianoDeiContiFinanziario,
                                     rec.data.Bilancio, rec.data.Capitolo);
                flag = false;
            }

        });
    },
    iconCls: 'add'
});
//COG

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelPerenti = new Ext.FormPanel({
    id: 'myPanelPerenti',
    frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    collapsible: true,
    width: 750,
    autoHeight: true,
    xtype: "form",
    title: "Impegni su Perente",
    tbar: [actionAddPerente, actionDeletePerente, actionBeneficiariImpPer, actionConfermaImpegnoPerente, actionModificaCOGAndPdCFImpPerente]

});

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanelLiqPerentiContestuali = new Ext.FormPanel({
    id: 'myPanelLiqPerentiContestuali',
    frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle: 'padding:1px',
    collapsible: true,
    width: 750,
    tbar: [actionBeneficiari3, actionShowFattureLiquidazionePerenti],
    title: "Liquidazioni contestuali agli impegni su perenti del presente provvedimento"
});


var labelNumImpPer = new Ext.form.Label({
    text: 'Numero Impegno da Ridurre Perente: ',
    id: 'labelNumImpPer'
});

var ComboCOGImpPer = null;
var ComboPdCFImpPer = null;



//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormPerenti() {

    var AnnoBilancioImpegnoPerente = new Ext.ux.YearMenu({
        format: 'Y',
        id: 'AnnoBilancioPerenti',
        allowBlank: false,
        noPastYears: false,
        noPastMonths: true,
        minDate: new Date('1990/1/1'),
        maxDate: new Date()
                       ,
        handler: function (dp, newValue, oldValue) {
            Ext.getCmp('AnnoIniPerente').value = newValue.format('Y');
            Ext.get('AnnoIniPerente').dom.value = newValue.format('Y');
        }
    });
    var btnRicercaImpegno = new Ext.Button({
        text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
        id: 'btnBilancioPerenti'
    });

    //DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var capitoloPanelImpegnoPerente = new Ext.Panel({
        id: 'panel-griglia-perente',
        xtype: "panel",
        columnWidth: 1,
        bodyStyle: 'margin-bottom: 10px'
    });

    btnRicercaImpegno.on('click', function () {

        var fNewAnno = Ext.getCmp("AnnoIniPerente").value;

        //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
        gridCapitoli = buildGridStartImpegnoPerente(fNewAnno);
        capitoloPanelImpegnoPerente.remove('GridCapitoliPerenti');
        capitoloPanelImpegnoPerente.add(gridCapitoli);
        capitoloPanelImpegnoPerente.doLayout();
    });

    var tbarDateImpegno = new Ext.Toolbar({
        style: 'margin-bottom:-1px;',
        width: 700,
        items: [{
            xtype: 'button',
            text: "<font color='#0000A0'><b>Bilancio</b></font>",
            id: 'SelBilPerente',
            menu: AnnoBilancioImpegnoPerente
        },
                 {
                     xtype: 'textfield',
                     cls: 'titfis',
                     readOnly: true,
                     id: 'AnnoIniPerente',
                     width: 80,
                     value: new Date().format('Y')
                 },
            btnRicercaImpegno
        ]
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

    buildComboModalitaPagamentoImpegnoPerente();

    //DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
    var buttonPanelImpegno = new Ext.Panel({
        xtype: "panel",
        title: "",
        width: 300,
        buttonAlign: "center",
        items: [{
            layout: "fit"
        }],
        buttons: [{
            text: 'Aggiungi Perente',
            id: 'btnAggiungiPerente'
        }]
    });

    ComboCOGImpPer = new Ext.form.ComboBox({
        fieldLabel: 'Cod.Ob.Gestionale(*)',
        displayField: 'Id',
        valueField: 'Id',
        name: 'ComboCOGImpPer',
        id: 'ComboCOGImpPer',
        listWidth: 200,
        readOnly: true,
        mode: 'local',
        width: 170,
        triggerAction: 'all',
        emptyText: 'Seleziona COG...',
        style: 'background-color: #fffb8a;background-image:none;'

    });

    ComboPdCFImpPer = new Ext.form.ComboBox({
        fieldLabel: 'P.C.F. (*)',
        displayField: 'Id',
        valueField: 'Id',
        name: 'ComboPdCFImpPer',
        id: 'ComboPdCFImpPer',
        listWidth: 200,
        width: 170,
        readOnly: true,
        mode: 'local',
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">PCF: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
        triggerAction: 'all',
        style: 'background-color: #fffb8a;background-image:none;',
        emptyText: 'Seleziona PCF...'
    });


    //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
    var formCapitoliImpegnoPerente = new Ext.FormPanel({
        id: 'ElencoCapitoliPerente',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per impegno',
        bodyStyle: 'padding:5px',
        collapsible: true, // PERMETTE DI ICONIZZARE LA FORM
        width: 1000,
        autoScroll: true,
        layout: 'absolute',
        monitorValid: true,
        buttons: [
            {
                text: 'Aggiungi Perente',
                id: 'btnAggiungiPerente',
                disabled: true
            }, {
                text: 'Annulla',
                handler: function (btn, evt) {
                    popupImpegnoPerente.close();
                }
            }
        ],
        items: [
            {
                id: "ListaBeneficiariPerentiDaLiquidare",
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
            capitoloPanelImpegnoPerente,
            {
                xtype: 'panel',
                title: '<span style="font-size: 0.8em">Seleziona Impegno Perente</span>',
                width: 971,
                height: 180,
                frame: true,
                layout: 'absolute',
                x: 0,
                y: 305,
                items: [
                {
                    xtype: 'label',
                    text: 'Impegno Perente:',
                    x: 0,
                    y: 0,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'combo',
                    id: 'ComboImpPer',
                    emptyText: 'Seleziona...',
                    name: 'ComboImpPer',
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
                    store: storeImpegniPer,
                    listeners: {
                        select: function (ele, rec, idx) {

                            var params = { NumImpegno: rec.data.NumImpegno };

                            Ext.lib.Ajax.defaultPostHeader = 'application/json';
                            Ext.Ajax.request({
                                url: 'ProcAmm.svc/GetBeneficiarioImpegno',
                                params: Ext.encode(params),
                                method: 'POST',
                                success: function (response, options) {
                                    mask.hide();
                                    var data = Ext.decode(response.responseText);


                                    VerificaDisponibilitaPerente();
                                    listaBeneficiariImpegnoPerente = data.GetBeneficiarioImpegnoResult;

                                    if (Ext.getCmp('GridBeneficiariImpegnoPerenti') != undefined) {
                                        beneficiariPanel.remove(Ext.getCmp('GridBeneficiariImpegnoPerenti'));
                                    }
                                    //if (listaBeneficiariImpegnoPerente != undefined && listaBeneficiariImpegnoPerente.length == 1) {
                                        var grigliaBeneficiariImpegno = buildGridBeneficiariImpegnoPerente();
                                        beneficiariPanel.add(grigliaBeneficiariImpegno);
                                        beneficiariPanel.doLayout();
                                        beneficiariPanel.show();
                                    //}


                                },
                                failure: function (response, result) {
                                    mask.hide();
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
                            /* VerificaDisponibilitaPerente();
                               listaBeneficiariImpegnoPerente = rec.data.ListaBeneficiari;
                              if (Ext.getCmp('GridBeneficiariImpegnoPerenti') != undefined) {
                                 beneficiariPanel.remove(Ext.getCmp('GridBeneficiariImpegnoPerenti'));
                             }
                             var grigliaBeneficiariImpegno = buildGridBeneficiariImpegnoPerente();
                             beneficiariPanel.add(grigliaBeneficiariImpegno);
                             beneficiariPanel.doLayout();
                             beneficiariPanel.show();*/

                        }
                    },
                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><div style="text-decoration:underline;display:inline">N. Impegno</div>: <b>{NumImpegno}</b></div></tpl>',
                    style: 'background-color: #fffb8a;background-image:none;'
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
                }
                , {
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
                    xtype: 'combo',
                    id: 'ComboCOGImpPer',
                    name: 'ComboCOGImpPer',
                    displayField: 'Id',
                    valueField: 'Id',
                    loadingText: 'Attendere...',
                    x: 750,
                    y: 30,
                    width: 180,
                    listwidth: 180,
                    editable: false,
                    mode: 'local',
                    triggerAction: 'all',
                    allowBlank: true,
                    store: storeCOGImpegniPer,
                    tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">COG: <b>{Id}</b><br/>{Descrizione}</div></tpl>',
                    style: 'background-color: #fffb8a;background-image:none;',
                    disabled: false,
                    emptyText: 'Seleziona COG...'
                }, {
                    xtype: 'label',
                    text: 'Importo Originario:',
                    x: 320,
                    y: 30,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 420,
                    y: 30,
                    width: 180,
                    name: 'ImportoOriginario',
                    id: 'ImportoOriginario',
                    style: 'opacity:.9;',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Importo Disponibile:',
                    x: 320,
                    y: 60,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 420,
                    y: 60,
                    width: 180,
                    name: 'ImpDispPerente',
                    id: 'ImpDispPerente',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'Capitolo Originario:',
                    x: 320,
                    y: 0,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'textfield',
                    x: 420,
                    y: 0,
                    width: 180,
                    name: 'CapitoloOriginario',
                    id: 'CapitoloOriginario',
                    readOnly: true
                }, {
                    xtype: 'label',
                    text: 'P.C.F. (*):',
                    x: 620,
                    y: 60,
                    width: 100,
                    style: 'padding-top: 5px'
                }, {
                    xtype: 'combo',
                    id: 'ComboPdCFImpPer',
                    name: 'ComboPdCFImpPer',
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
                    store: storePdCFImpegniPer,
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

    var popupImpegnoPerente = new Ext.Window({
        // y: 15,
        title: 'Aggiungi un nuovo impegno perente',
        id: 'popupImpegnoPerente',
        width: 1000,
        height: 800,
        layout: 'fit',
        plain: true,
        //bodyStyle: 'padding-top:10px',
        buttonAlign: 'center',
        maximizable: true,
        enableDragDrop: false,
        collapsible: false,
        autoScroll: true,
        modal: true,
        closable: true
    });
    popupImpegnoPerente.add(formCapitoliImpegnoPerente);
    popupImpegnoPerente.doLayout(); //forzo ridisegno

    //GESTISCO L'AZIONE DEL BOTTONE "Aggiungi Perente"
    Ext.getCmp('btnAggiungiPerente').on('click', function () {
        var rows = Ext.getCmp('GridCapitoliPerenti').getSelectionModel().getSelections();
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
            if ((Ext.get('ImpDispPerente').dom.value == 0) || (Ext.get('ImpDispPerente').dom.value == '')) {
                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: 'Selezionare un valore dalla lista degli Impegni Perenti',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function (btn) { return; }
                });
            } else {
                if ((Ext.get('ComboCOGImpPer').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboCOGImpPer').dom.value == '')) {
                    Ext.MessageBox.show({
                        title: 'COG',
                        msg: 'Il campo Codide Obiettivo Gestionale non &egrave; stato valorizzato!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) { return }
                    });
                } else if ((Ext.get('ComboPdCFImpPer').dom.value.indexOf("Seleziona") != -1) || (Ext.get('ComboPdCFImpPer').dom.value == '')) {
                    Ext.MessageBox.show({
                        title: 'Piano dei Conti Finanziario',
                        msg: 'Il campo Piano dei Conti Finanziario non &egrave; stato valorizzato!',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) { return }
                    });
                } else {


                    var listaBeneficiariSelezionati = Ext.getCmp("GridBeneficiariImpegnoPerenti").getSelectionModel().getSelections();
                    if (listaBeneficiariSelezionati.length <= 0) {
                        Ext.MessageBox.show({
                            title: 'Attenzione',
                            msg: 'E\' necessario selezionare almeno un beneficiario dalla lista',
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.ERROR,
                            fn: function (btn) { return; }
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
                                fn: function (btn) { return; }
                            });
                        } else {
                            generaImpegnoELiquidazioneDaPerente(listaBeneficiariSelezionati);
                        }
                    }

                }
            }

        } //fine else (rows[0].data.CodiceRisposta > 0)
    });       //FINE ON CLICK


    function generaImpegnoELiquidazioneDaPerente(listaBeneficiari) {
        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';

        var json = '';
        for (var i = 0; i < listaBeneficiari.length; i++) {
            json += Ext.util.JSON.encode(listaBeneficiari[i].data) + ',';
        }
        json = json.substring(0, json.length - 1);

        Ext.get('ListaBeneficiariPerentiDaLiquidare').dom.value = json;


        formCapitoliImpegnoPerente.getForm().timeout = 100000000;
        formCapitoliImpegnoPerente.getForm().submit({
            url: 'ProcAmm.svc/GenerazioneImpegnoSuPerenteELiquidazione',
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            failure:
                function (result, response) {
                    var lstr_messaggio = '';
                    try {
                        lstr_messaggio = response.result.FaultMessage;
                    } catch (ex) {
                        lstr_messaggio = 'Errore Generale';
                    }

                    Ext.MessageBox.show({
                        title: 'Gestione Impegno su perente Contabile',
                        msg: lstr_messaggio,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) {
                            Ext.getCmp('GridPerenti').getStore().reload();
                            Ext.getCmp('popupImpegnoPerente').close();
                            return;
                        }
                    });

                }, // FINE FAILURE
            success:
                function (result, response) {
                    var msg = 'Generazione Impegno e Liquidazione su Perente effettuato con successo!';
                    var progLiq = "";
                    try {
                        progLiq = response.result.progLiquidazione;
                        Ext.getCmp('GridPerenti').getStore().reload();
                        fnBtnVisLiqPerente(formCapitoliImpegnoPerente.getForm().getValues());

                    } catch (ex) {
                    }
                    Ext.MessageBox.show({
                        title: 'Gestione Preimpegno Contabile',
                        msg: msg,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO,
                        fn: function (btn) {
                            var storeGridLiq = Ext.getCmp('GridLiqPerenti').getStore();
                            var storeGridImp = Ext.getCmp('GridPerenti').getStore();
                            if (isDocumentoConFatture) {
                                var liquidazione = undefined;
                                storeGridLiq.each(function (rec) {
                                    if (progLiq == rec.data.ID) {
                                        liquidazione = { ID: rec.data.ID, Capitolo: rec.data.Capitolo, Bilancio: rec.data.Bilancio, ImpPrenotatoLiq: rec.data.ImpPrenotatoLiq };

                                        var index = storeGridLiq.indexOf(rec);
                                        Ext.getCmp('GridLiqPerenti').getSelectionModel().selectRow(index);
                                        Ext.getCmp('GridLiqPerenti').getView().refresh();
                                    }
                                });

                                if (liquidazione != undefined) {
                                    var grid = buildPanelFattureLiquidazionePerenti(liquidazione);
                                    Ext.getCmp('myPanelLiqPerentiContestuali').add(grid);
                                    Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
                                }
                            } else {

                                var listaBeneficiariDaLiquidare = Ext.util.JSON.decode("[" + Ext.get('ListaBeneficiariPerentiDaLiquidare').dom.value + "]");

                                if (listaBeneficiariDaLiquidare != undefined && listaBeneficiariDaLiquidare.length > 0) {
                                    
                                } else {
                                    Ext.MessageBox.show({
                                        title: 'Attenzione',
                                        msg: "Ricordati di inserire il beneficiario sulla liquidazione appena generata.",
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.WARNING,
                                        fn: function (btn) {
                                            Ext.getCmp('GridLiqPerenti').getStore().reload();
                                            return;
                                        }
                                    });
                                }

                              
                            }


                            Ext.getCmp('popupImpegnoPerente').close();
                        }
                    }); // FINE Ext.MessageBox.show
                } // FINE SUCCESS
        }); // FINE SUBMIT
    }


    //PRENDO L'ANNO DALLA DATA ODIERNA
    var dtOggi = new Date();
    dtOggi.setDate(dtOggi.getDate());
    var fAnno = dtOggi.getFullYear();

    //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
    var gridCapitoli = buildGridStartImpegnoPerente(fAnno);
    capitoloPanelImpegnoPerente.add(gridCapitoli);

    beneficiariPanel.add(buildGridBeneficiariImpegnoPerente());

    popupImpegnoPerente.show();

    //GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
    var formPanelDropTargetEl = formCapitoliImpegnoPerente.body.dom;
    var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
        ddGroup: 'gridDDGroup',
        notifyEnter: function (ddSource, e, data) {
            //EFFETTI VISIVI SUL DRAG & DROP
            formCapitoliImpegnoPerente.body.stopFx();
            formCapitoliImpegnoPerente.body.highlight();
        },
        notifyDrop: function (ddSource, e, data) {
            // CATTURO IL RECORD SELEZIONATO
            var selectedRecord = ddSource.dragData.selections[0];
            // CARICO IL RECORD NELLA FORM
            formCapitoliImpegnoPerente.getForm().loadRecord(selectedRecord);
            Ext.get('ImpDispPerente').dom.value = eurRend(Ext.get('ImpDispPerente').dom.value);
            // buildComboImpPer(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
            //    buildComboCOGImpPer(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
            //  panelImpPer.doLayout();
            //  panelImpPer.show();
            // ISTRUZIONI PER IL DRAG --- ASTERISCATE
            // CANCELLO IL RECORD DALLA GRIGLIA.
            //ddSource.grid.store.remove(selectedRecord);
            return (true);
        }
    });

}// fine InitFormPerenti


function VerificaDisponibilitaPerente() {
    try {
        var comboImpPer = Ext.getCmp('ComboImpPer');
        var dataComboImpPer = comboImpPer.store.data;
        var selectedIndex = comboImpPer.selectedIndex;

        Ext.get('ImpDispPerente').dom.value = dataComboImpPer.get(selectedIndex).data.ImpDisp;
        Ext.get('ImpDispPerente').dom.value = eurRend(Ext.get('ImpDispPerente').dom.value);
        Ext.get('CapitoloOriginario').dom.value = dataComboImpPer.get(selectedIndex).data.CapitoloOriginario;
        Ext.get('ImportoOriginario').dom.value = dataComboImpPer.get(selectedIndex).data.ImportoOriginario;
        Ext.get('ImportoOriginario').dom.value = eurRend(Ext.get('ImportoOriginario').dom.value);

        //if (dataComboImpPer.get(selectedIndex).data.Beneficiario != undefined) {
        //    Ext.get('ImpPrenotatoPerente').dom.value = dataComboImpPer.get(selectedIndex).data.ImpDisp;
        //} else {
        //    Ext.get('ImpPrenotatoPerente').dom.value = 0;
        //}

        Ext.get('Oggetto_Impegno').dom.value = dataComboImpPer.get(selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(dataComboImpPer.get(selectedIndex).data.ImpPotenzialePrenotato);
        Ext.get('ComboPdCFImpPer').dom.value = dataComboImpPer.get(selectedIndex).data.PianoDeiContiFinanziario;
        Ext.getCmp('btnAggiungiPerente').show();
    } catch (ex) {
    }
}


function getListaBeneficiariImpegnoPerenteStore() {
    var store = new Ext.data.Store();

    if (listaBeneficiariImpegnoPerente != null) {

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

        for (var i = 0; i < listaBeneficiariImpegnoPerente.length; i++) {
            var record = new TipoRecord({
                ID: listaBeneficiariImpegnoPerente[i].ID,
                Tipologia: listaBeneficiariImpegnoPerente[i].Tipologia,
                CodiceFiscale: listaBeneficiariImpegnoPerente[i].CodiceFiscale,
                PartitaIva: listaBeneficiariImpegnoPerente[i].PartitaIva,
                Denominazione: listaBeneficiariImpegnoPerente[i].Denominazione,
                Cognome: listaBeneficiariImpegnoPerente[i].Cognome,
                Nome: listaBeneficiariImpegnoPerente[i].Nome,
                DataNascita: listaBeneficiariImpegnoPerente[i].DataNascita,
                ListaSedi: listaBeneficiariImpegnoPerente[i].ListaSedi,
                Contratto: listaBeneficiariImpegnoPerente[i].Contratto,
                PartitaIvaOrCodFiscToView: listaBeneficiariImpegnoPerente[i].PartitaIvaOrCodFiscToView,
                ImportoOriginarioSuImpegno: listaBeneficiariImpegnoPerente[i].ImportoOriginarioSuImpegno,
                ImportoResiduoSuImpegno: listaBeneficiariImpegnoPerente[i].ImportoResiduoSuImpegno,
                ImportoDaLiquidare: listaBeneficiariImpegnoPerente[i].ImportoResiduoSuImpegno,
                IdModalitaPagamentoSelected: listaBeneficiariImpegnoPerente[i].IdModalitaPagamentoSelected
            });
            store.insert(i, record);
        }
    }

    return store;
}


function buildComboModalitaPagamentoImpegnoPerente() {



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

    comboModalitaPagamentoImpegnoPerente = new Ext.form.ComboBox({
        displayField: 'Descrizione',
        valueField: 'Id',
        fieldLabel: 'Tipologia pagamento (*)',
        id: 'comboModalitaPagamentoImpegnoPerenteId',
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

    return comboModalitaPagamentoImpegnoPerente;
}

//FUNZIONE CHE CANCELLA LOGICAMENTE E FISICAMENTE I PREIMPEGNI
function EliminaPreImpegnoPerente(NumPreImpegno, ID) {
    var params = { NumeroPreImpegno: NumPreImpegno, ID: ID };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/EliminaPreImpegno',
        params: Ext.encode(params),
        method: 'POST',
        success: function (response, options) {
            mask.hide();
            Ext.getCmp('GridLiqPerenti').store.reload();
            Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridFattureLiquidazionePerenti'));
            Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridBeneficiariPerenti'));
            actionBeneficiari3.setDisabled(true);
            actionShowFattureLiquidazionePerenti.setDisabled(true);
        },
        failure: function (response, options) {
            mask.hide();
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) {
                    Ext.getCmp('GridLiqPerenti').store.reload();
                    return;
                }
            });
        }
    });

}

//inserisce nello store delle liquidazioni  contestuali il record 
// costruitop a partire dai campi della form
function addLiquidazioneContestualePerente(fields) {

    var TipoRecord = Ext.data.Record.create([
    			{ name: 'Bilancio' }
               , { name: 'UPB' }
               , { name: 'MissioneProgramma' }
               , { name: 'Capitolo' }
               , { name: 'ImpDisp' }
               , { name: 'ImpPrenotato' }
               , { name: 'NumImpegno' }
               , { name: 'ImpPrenotatoLiq' }
               , { name: 'AnnoPrenotazione' }
               , { name: 'ID' }
    ]);
    var record = new TipoRecord({
        Bilancio: fields.Bilancio
              , UPB: fields.UPB
              , MissioneProgramma: fields.MissioneProgramma
              , Capitolo: fields.Capitolo
              , ImpDisp: fields.ImpDisp
              , ImpPrenotato: fields.ImpPrenotato
              , NumImpegno: fields.NumImpegno
              , ImpPrenotatoLiq: fields.ImpPrenotatoLiq
              , AnnoPrenotazione: fields.AnnoPrenotazione
              , ID: fields.ID
    });
    var store = Ext.getCmp('GridLiqPerenti').getStore();

    store.insert(0, record); // alza l' evento 'add' che la griglia intercetta 
}
function fnBtnVisLiqPerente(fields) {

    myPanelLiqPerentiContestuali.show();

    var ufficio = Ext.get('Cod_uff_Prop').dom.value;
    var parametri = { CodiceUfficio: ufficio };

    // Ext.getCmp('GridLiqPerenti').store.load();
    Ext.getCmp('GridLiqPerenti').store.load({ params: parametri });

    //Ext.getCmp('GridLiqPerenti').store.load();
}




//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartImpegnoPerente(annoRif) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetElencoCapitoliAnno',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetElencoCapitoliAnnoResult',
        fields: [
               { name: 'Bilancio' },
               { name: 'UPB' },
               { name: 'MissioneProgramma' },
               { name: 'Capitolo' },
               { name: 'ImpDisp' },
               { name: 'ImpPrenotato' },
               { name: 'NumPrenotazione' },
               { name: 'AnnoPrenotazione' },
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

    var parametri = { AnnoRif: annoRif, tipoCapitolo: 1 };
    store.load({ params: parametri });

    store.on({
        'load': {
            fn: function (store, records, options) {
                return;
                mask.hide();
            },
            scope: this
        }
    });

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
                   { header: "Missione.Programma", width: 80, dataIndex: 'MissioneProgramma', sortable: true },
                   { header: "Descrizione", width: 180, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
                   //{ renderer: eurRend, header: "Importo<br/>Disponibile", width: 60, dataIndex: 'ImpDisp', sortable: true },
                   { header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
    ]);

    var grid = new Ext.grid.GridPanel({
        id: 'GridCapitoliPerenti',
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
                Ext.getCmp("ElencoCapitoliPerente").getForm().loadRecord(rec);

                var ufficio = Ext.get('Cod_uff_Prop').dom.value;
                var annoRif = rec.data['Bilancio'];
                var capitoloRif = rec.data['Capitolo'];

                Ext.get('Capitolo').dom.value = capitoloRif;
                Ext.get('Bilancio').dom.value = annoRif;
                Ext.get('UPB').dom.value = rec.data['UPB'];
                Ext.get('MissioneProgramma').dom.value = rec.data['MissioneProgramma'];

                var paramsStoreImpegniPer = {
                    AnnoRif: annoRif,
                    CapitoloRif: capitoloRif,
                    CodiceUfficio: ufficio
                };

                storeImpegniPer.reload({ params: paramsStoreImpegniPer });

                var parametriStorePdCF = { AnnoRif: annoRif, CapitoloRif: capitoloRif };
                storePdCFImpegniPer.load({ params: parametriStorePdCF });

                var parametriStoreCOG = { AnnoRif: annoRif, CapitoloRif: capitoloRif };
                storeCOGImpegniPer.load({ params: parametriStoreCOG });

                //resettare i dati nella gridBeneficiariImpegno
                var gridBeneficiariImpegnoPerenti = Ext.getCmp("GridBeneficiariImpegnoPerenti");
                if (gridBeneficiariImpegnoPerenti != undefined) {
                    gridBeneficiariImpegnoPerenti.getStore().removeAll();
                }


                //buildComboImpPer(rec.data['Bilancio'], rec.data['Capitolo']);

                //setBeneficiarioImpegnoPerenteSelected(undefined);
                //Ext.get('ImpPrenotatoPerente').dom.value = '0';
                Ext.get('ImportoOriginario').dom.value = '0';
                Ext.get('CapitoloOriginario').dom.value = '';
                Ext.get('ImpDispPerente').dom.value = '0';
                // panelImpPer.doLayout();
                // panelImpPer.show();
                //buildComboCOGImpPer(rec.data['Bilancio'], rec.data['Capitolo']);
                //buildComboPdCFImpPer(rec.data['Bilancio'], rec.data['Capitolo']);
            }, scope: this
        }
    });
    return grid;
}

function enableConfirmAction(selectedRows) {
    var retValue = false

    for (var i = 0; !retValue && i < selectedRows.length; i++)
        if (retValue = (selectedRows[i].data.Stato == 2))
            break;

    return retValue;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "PERENTI"
function buildGridPerenti() {

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetImpegniRegistratiPerenti' + window.location.search,
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetImpegniRegistratiPerentiResult',
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
               { name: 'Tipo' },
               { name: 'Stato' },
               { name: 'StatoAsString' },
               { name: 'Codice_Obbiettivo_Gestionale' },
               { name: 'PianoDeiContiFinanziario' },
               { name: 'HashTokenCallSic_Imp' },
               { name: 'IdDocContabileSic_Imp' }

        ]
    });

    var store = new Ext.data.GroupingStore({
        proxy: proxy,
        reader: reader,
        groupField: 'Tipo',
        sortInfo: {
            field: 'Tipo',
            direction: "ASC"
        }
    });



    store.on({
        'load': {
            fn: function (store, records, options) {
                return;
                mask.hide();
            },
            scope: this
        }
    });
    var ufficio = Ext.get('Cod_uff_Prop').dom.value;
    var parametri = { CodiceUfficio: ufficio };
    store.load({ params: parametri });


    var summary = new Ext.grid.GroupSummary();
    var sm = new Ext.grid.CheckboxSelectionModel(
       {
           singleSelect: false,
           listeners: {
               rowselect: function (sm, row, rec) {
                   var multiSelect = sm.getSelections().length > 1;

                   if (rec.data.Stato == 2) {
                       actionConfermaImpegnoPerente.setDisabled(false);
                   }

                   actionDeletePerente.setDisabled(multiSelect);
                   actionBeneficiariImpPer.setDisabled(multiSelect);
                   actionAddPerente.setDisabled(true);
                   actionModificaCOGAndPdCFImpPerente.setDisabled(multiSelect);
               },
               rowdeselect: function (sm, row, rec) {
                   var selectedRowsCount = sm.getSelections().length;

                   if (selectedRowsCount == 1) {
                       actionDeletePerente.setDisabled(false);
                       actionBeneficiariImpPer.setDisabled(false);
                       actionModificaCOGAndPdCFImpPerente.setDisabled(false);
                   } else {
                       actionDeletePerente.setDisabled(true);
                       actionBeneficiariImpPer.setDisabled(true);
                       actionModificaCOGAndPdCFImpPerente.setDisabled(true);
                   }

                   Ext.getCmp('myPanelPerenti').remove(Ext.getCmp('GridBeneficiariImpegnoPerenti'));
                   Ext.getCmp('myPanelPerenti').doLayout();

                   actionAddPerente.setDisabled(selectedRowsCount == 0 ? false : true);
                   actionConfermaImpegnoPerente.setDisabled(selectedRowsCount == 0 ? true : !enableConfirmAction(sm.getSelections()));
               }
           }
       }
       );
    var ColumnModel = new Ext.grid.ColumnModel([
		                sm,
		                {
		                    header: "Bilancio", width: 60, dataIndex: 'Bilancio', id: 'Bilancio', sortable: true,
		                    summaryRenderer: function (v, params, data) { return '<b> Totale </b>'; }
		                },
                        { header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true, locked: false },
		                { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
		                { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },

		            	{ renderer: eurRend, header: "Importo da Prenotare", dataIndex: 'ImpPrenotato', sortable: true, summaryType: 'sum' },
		            	{ header: "Numero Impegno Perente", dataIndex: 'NumPreImp', sortable: true },
		            	{ header: "ID", dataIndex: 'ID', hidden: true },
		            	{ header: "Tipo", dataIndex: 'Tipo', hidden: true },
		            	{ header: "COG", dataIndex: 'Codice_Obbiettivo_Gestionale', sortable: true },
		            	{ header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario', sortable: true },
		            	{ header: "Stato", dataIndex: 'StatoAsString' }
    ]);

    var GridPerenti = new Ext.grid.GridPanel({
        id: 'GridPerenti',
        ds: store,
        sm: sm,
        colModel: ColumnModel,
        autoHeight: true,
        autoWidth: true,
        layout: 'fit',
        loadMask: true
    });

    actionDeletePerente.setDisabled(true);
    actionBeneficiariImpPer.setDisabled(true);
    actionConfermaImpegnoPerente.setDisabled(true);
    actionModificaCOGAndPdCFImpPerente.setDisabled(true);
    return GridPerenti;
}

//FUNZIONE CHE RIEMPIE LA GRIGLIA "Liquidazioni Perenti"
function buildGridLiquidazioniPerenti(isFatturePresenti) {
    isDocumentoConFatture = isFatturePresenti;
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetLiquidazionidaPerentiRegistrateContestuali' + window.location.search,
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetLiquidazionidaPerentiRegistrateContestualiResult',
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
               { name: 'PianoDeiContiFinanziario' },
               { name: 'HashTokenCallSic' },
               { name: 'IdDocContabileSic' }
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

    //  store.load();
    store.on({
        'load': {
            fn: function (store, records, options) {
                return;
                mask.hide();
            },
            scope: this
        }
    });

    var ufficio = Ext.get('Cod_uff_Prop').dom.value;
    var parametri = { CodiceUfficio: ufficio };
    store.load({ params: parametri });

    var sm = new Ext.grid.CheckboxSelectionModel(
          {
              singleSelect: true,
              listeners: {
                  rowselect: function (sm, row, rec) {
                      if (Ext.getCmp('GridBeneficiariPerenti') != undefined) {
                          Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridBeneficiariPerenti'));
                          Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
                      }
                      actionBeneficiari3.setDisabled(false);
                      actionShowFattureLiquidazionePerenti.setDisabled(false);
                  },
                  rowdeselect: function (sm, row, rec) {
                      if (Ext.getCmp('GridBeneficiariPerenti') != undefined) {
                          Ext.getCmp('myPanelLiqPerentiContestuali').remove(Ext.getCmp('GridBeneficiariPerenti'));
                          Ext.getCmp('myPanelLiqPerentiContestuali').doLayout();
                      }
                      actionBeneficiari3.setDisabled(true);
                      actionShowFattureLiquidazionePerenti.setDisabled(true);
                  }
              }
          });



    var summary = new Ext.grid.GroupSummary();

    var ColumnModel = new Ext.grid.ColumnModel({
        columns: [
                          sm,
                          {
                              header: "Bilancio", width: 60, dataIndex: 'Bilancio', id: 'Bilancio', sortable: true,
                              summaryRenderer: function (v, params, data) { return '<b> Totale </b>'; }
                          },
                          { header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true },
                          { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                          { header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
                          { renderer: eurRend, header: "Importo da Liquidare", dataIndex: 'ImpPrenotatoLiq', sortable: true, summaryType: 'sum' },
                          { header: "Numero Impegno", dataIndex: 'NumImpegno', id: 'NumImpegno', sortable: true },
                          { header: "Stato", dataIndex: 'StatoAsString' },
                          { header: "Piano dei Conti Finanziario", dataIndex: 'PianoDeiContiFinanziario' },
                          { header: "ID", dataIndex: 'ID', id: 'ID', sortable: false, hidden: true },
                          { header: "Tipo", dataIndex: 'Tipo', hidden: true }

        ]
    });

    var GridLiqPerenti = new Ext.grid.GridPanel({
        id: 'GridLiqPerenti',
        title: '',
        hidden: false,
        ds: store,
        sm: sm,
        colModel: ColumnModel,
        autoHeight: true,
        autoWidth: true,
        layout: 'fit',
        loadMask: true
    });
    actionBeneficiari3.setDisabled(true);
    actionShowFattureLiquidazionePerenti.setDisabled(true);
    //	          if (isDocumentoConFatture) {
    //actionShowFattureLiquidazionePerenti.setDisabled(!enableFattureAction(sm.getSelections()));
    //	          } else {
    //actionShowFattureLiquidazionePerenti.setDisabled(true);
    //	          }
    return GridLiqPerenti;

}

//FUNZIONE CHE CONFERMA I DATI INSERITI CON L'IMPORTAZIONE
function ConfermaMultiplaImpegnoPerente(ImpegniPerentiInfo) {
    var params = { ImpegniPerentiInfo: ImpegniPerentiInfo };

    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: "Operazione in corso..."
    });

    mask.show();

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaMultiplaImpegnoPerente',
        params: Ext.encode(params),
        method: 'POST',
        success: function (result, response, options) {
            mask.hide();
            Ext.getCmp('GridPerenti').getStore().reload();
            try {
                Ext.getCmp('GridLiqPerenti').getStore().reload();
            } catch (ex) { }
            actionDeletePerente.setDisabled(true);
            actionBeneficiariImpPer.setDisabled(true);
            actionConfermaImpegnoPerente.setDisabled(true);
            actionModificaCOGAndPdCFImpPerente.setDisabled(true);
            actionAddPerente.setDisabled(false);
        },
        failure: function (response, result, options) {
            mask.hide();
            var data = Ext.decode(response.responseText);
            Ext.MessageBox.show({
                title: 'Errore',
                msg: data.FaultMessage,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) {
                    if (data.FaultCode == 2) { //this fault code means that at least a confirmation is succeded
                        Ext.getCmp('GridPerenti').getStore().reload();
                        try {
                            Ext.getCmp('GridLiqPerenti').getStore().reload();
                        } catch (ex) { }
                        actionDeletePerente.setDisabled(true);
                        actionBeneficiariImpPer.setDisabled(true);
                        actionConfermaImpegnoPerente.setDisabled(true);
                        actionModificaCOGAndPdCFImpPerente.setDisabled(true);
                        actionAddPerente.setDisabled(false);
                    }
                }
            });
        }
    });
}

function ConfermaImpegnoPerente(ID) {
    var params = { ID: ID };

    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    Ext.Ajax.request({
        url: 'ProcAmm.svc/ConfermaImpegnoPerente',
        params: Ext.encode(params),
        method: 'POST',
        success: function (result, response, options) {
            mask.hide();
            Ext.getCmp('GridPerenti').getStore().reload();
            try {
                Ext.getCmp('GridLiqPerenti').getStore().reload();
            } catch (ex) { }
        },
        failure: function (response, result, options) {
            mask.hide();
            //Ext.decode(result.responseText).FaultMessage
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

function NascondiColonneImpPer() {
    var index = Ext.getCmp('GridPerenti').colModel.getColumnCount(false);
    index = index - 1;
    Ext.getCmp('GridPerenti').colModel.setHidden(index, true);
    actionConfermaImpegnoPerente.hide();
}

function NascondiColonneLiqPer() {
    var index = Ext.getCmp('GridLiqPerenti').colModel.getColumnCount(false);
    index = index - 1;
    Ext.getCmp('GridLiqPerenti').colModel.setHidden(index, true);
}
