var mask;

var isOpContImpLiq = false;
var isOpContLiq = false;
var isOpContImpLiqPerenti = false;


mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

var maskSalvataggio = new Ext.LoadMask(Ext.getBody(), {
    msg: "Salvataggio in corso..."
});


var storeRettifiche = new Ext.data.SimpleStore({
    fields: ['Id', 'Descrizione'],
    data: [['1', "Annullamento"], ['2', "liq"]]
});


function buildComboSistemazioni(tipo) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaTipoRettifiche',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetListaTipoRettificheResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }
        ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader
    });

    store.setDefaultSort("Descrizione", "ASC");

    store.on({
        'load': {
            fn: function (store, records, options) {
                mask.hide();
                var comboSistemazioni = Ext.getCmp("ComboSistemazioni");
                var codSistemazione = comboSistemazioni.findRecord(comboSistemazioni.valueField || comboSistemazioni.displayField, arrOpContabili[7]);
                comboSistemazioni.setValue(codSistemazione != undefined && codSistemazione != null ? arrOpContabili[7] : "")
            },
            scope: this
        }
    });

    var ComboSistemazioni = new Ext.form.ComboBox({
        fieldLabel: 'Sistemazioni',
        name: 'ComboSistemazioni',
        hiddenName: 'ValueSistemazioni',
        id: 'ComboSistemazioni',
        width: 300,
        itemCls: "titfis",
        listWidth: 300,
        triggerAction: 'all',
        typeAhead: false,
        emptyText: '',
        displayField: 'Descrizione',
        valueField: 'Id',
        mode: 'local',
        readOnly: true,
        store: store

    });
    var parametri = { tipoDocumento: tipo };
    store.load({ params: parametri });

    return ComboSistemazioni;
}

function rispostaVerificaOggetto(btn) {
    if (btn == 'yes') { CreaProvvedimento(); }
}

function verificaEsistenzaOggetto(flag_ChiamaCreaProvvedimento) {

    var stdec = Ext.lib.Ajax.defaultPostHeader
    Ext.lib.Ajax.defaultPostHeader = 'application/json';

    var maskOggetto = new Ext.LoadMask(Ext.getBody(), {
        msg: "Verifica Oggetto..."
    });

    var lstrOggetto = Ext.getCmp('myPanel').getForm().getValues().txtOggetto;
    if (lstrOggetto != '') {
        maskOggetto.show();
        var params = { tipo: tipo, oggetto: lstrOggetto };
        //     var stdec = Ext.lib.Ajax.defaultPostHeader 
        //    Ext.lib.Ajax.defaultPostHeader = 'application/json';
        //    alert(stdec)

        Ext.Ajax.timeout = 100000000;

        Ext.MessageBox.wait('Loading ...');

        var box = Ext.Ajax.request({
            url: 'ProcAmm.svc/VerificaEsistenzaOggetto',
            params: Ext.encode(params),
            method: 'POST',
            success: function (response, options) {
                maskOggetto.hide();

                var data = Ext.decode(response.responseText);
                if (data.VerificaEsistenzaOggettoResult == true) {
                    Ext.MessageBox.buttonText.yes = 'Si';

                    Ext.MessageBox.show({
                        title: 'Attenzione',
                        msg: 'Esiste già un provvedimento con lo stesso oggetto.<br>Continuare?',
                        buttons: Ext.MessageBox.YESNO,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                            if (btn == 'yes') {
                                if (flag_ChiamaCreaProvvedimento == true) {
                                    CreaProvvedimento();
                                }
                            }
                        }
                    });
                } else {
                    if (flag_ChiamaCreaProvvedimento == true) {
                        CreaProvvedimento();
                    }
                }

            },
            failure: function (response, options) {
                maskOggetto.hide();
                var data = Ext.decode(response.responseText);
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: data.FaultMessage,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function (btn) { return }
                });
                CreaProvvedimento();
            }
        });

    } else {
        CreaProvvedimento();
    }

    Ext.lib.Ajax.defaultPostHeader = stdec;

}

var actionHomePageUfficio = new Ext.Action({
    text: 'Annulla',
    tooltip: 'Annulla Provvedimento',
    id: 'btnAnnulla',
    handler: function () {
        CancellaProvvedimento();
    },
    iconCls: 'remove'
});

function CreaProvvedimentoFunction() {
    var result = verificaEsistenzaOggetto(true);
}

var actionCreaProvvedimento = new Ext.Action({
    text: 'Crea',
    id: 'btnCrea',
    tooltip: 'Crea Provvedimento',
    handler: CreaProvvedimentoFunction,
    iconCls: 'save'
});

function CancellaProvvedimento() {
    Ext.getDom('chkSalva').value = '0';
    window.location.href = 'HomePageAction.aspx';
}

function CreaProvvedimento() {
    var errore = null;
    errore = verificaForm(tipo);


    maskSalvataggio.show();

    if (errore == null) {
        if (Ext.getCmp('tab_legge_trasparenza').disabled) {
            resetSchedaLeggeTrasparenzaFields();
            enableFieldsForAutorizzazionePubblicazioneToYes(false);
            enableFieldsForAutorizzazionePubblicazioneToNo(false);
        }
        setContrattiAssociati();
        setFattureAssociate();

        if (Ext.getCmp('tab_tipologia_provvedimento').disabled) {
            resetSchedaTipologiaProvvedimentoFields();
            enableFieldsSchedaTipologiaProvvedimento(false);
        } else {
            setDestinatariAssociati();
        }

        Ext.getDom('chkSalva').value = '1';
        setUfficiSelezionati();
        setUtentiUfficiSelezionati();
        setAttributiInseriti();
        Ext.getCmp('myPanel').getForm().timeout = 100000000;

        //html encoding of form fields...
        var params = Ext.getCmp('myPanel').getForm().getValues();

        for (var key in params) {
            if (key != undefined && key != null && params.hasOwnProperty(key)) {
                params[key] = Ext.util.Format.htmlEncode(params[key]);
            }
        }

        Ext.Ajax.request({
            url: 'CreaProvvedimento.aspx' + window.location.search,
            params: params,
            method: 'POST',
            waitTitle: "Attendere...",
            waitMsg: 'Aggiornamento in corso ......',
            success: function (result, response) {
                var decodedResponseText = Ext.decode(result.responseText);
                if (decodedResponseText.success == true) {
                    location.href = decodedResponseText.link;
                }
                else {
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: decodedResponseText.FaultMessage,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) {
                            maskSalvataggio.hide();
                            return;
                        }
                    });
                }
            },
            failure:
                function (result, response) {

                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: 'Errore nel salvataggio. Riprovare',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) {
                            maskSalvataggio.hide();
                            return;
                        }
                    }); //fine messagebox
                } //fine function
        });
    } else {
        if (errore.msg.indexOf("Contratti-Fatture") > -1) {
            maskSalvataggio.hide();
            var msggio = errore.msg;
            errore = null;
            Ext.MessageBox.show({
                title: 'Attenzione',
                msg: msggio + '<br>Continuare?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                    if (btn == 'yes') {
                        maskSalvataggio.show();
                        if (Ext.getCmp('tab_legge_trasparenza').disabled) {
                            resetSchedaLeggeTrasparenzaFields();
                            enableFieldsForAutorizzazionePubblicazioneToYes(false);
                            enableFieldsForAutorizzazionePubblicazioneToNo(false);
                        }
                        setContrattiAssociati();
                        setFattureAssociate();

                        if (Ext.getCmp('tab_tipologia_provvedimento').disabled) {
                            resetSchedaTipologiaProvvedimentoFields();
                            enableFieldsSchedaTipologiaProvvedimento(false);
                        } else {
                            setDestinatariAssociati();
                        }

                        Ext.getDom('chkSalva').value = '1';
                        setUfficiSelezionati();
                        setUtentiUfficiSelezionati();
                        setAttributiInseriti();
                        Ext.getCmp('myPanel').getForm().timeout = 100000000;

                        //html encoding of form fields...
                        var params = Ext.getCmp('myPanel').getForm().getValues();

                        for (var key in params) {
                            if (key != undefined && key != null && params.hasOwnProperty(key)) {
                                params[key] = Ext.util.Format.htmlEncode(params[key]);
                            }
                        }

                        Ext.Ajax.request({
                            url: 'CreaProvvedimento.aspx' + window.location.search,
                            params: params,
                            method: 'POST',
                            waitTitle: "Attendere...",
                            waitMsg: 'Aggiornamento in corso ......',
                            success: function (result, response) {
                                var decodedResponseText = Ext.decode(result.responseText);
                                if (decodedResponseText.success == true) {
                                    location.href = decodedResponseText.link;
                                }
                                else {
                                    Ext.MessageBox.show({
                                        title: 'Errore',
                                        msg: decodedResponseText.FaultMessage,
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.ERROR,
                                        fn: function (btn) {
                                            return;
                                        }
                                    });
                                }
                            },
                            failure:
                function (result, response) {

                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: 'Errore nel salvataggio. Riprovare',
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function (btn) {
                            return;
                        }
                    }); //fine messagebox
                } //fine function
                        });

                    }
                }
            });
        } else {
            maskSalvataggio.hide();
            Ext.MessageBox.show({
                title: 'Errore',
                msg: errore.msg,
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function (btn) {
                    if (errore.tab_to_activate != null && errore.tab_to_activate != '') {
                        setActivePanel("documentInfo_tabPanel", errore.tab_to_activate);
                    }
                    return;
                }
            });
        }
    }
}


function gestioneClickOpContabili(obj, flagAbilita) {
    if (flagAbilita != '1') {
        return false;
    } else {
        obj.setValue(1);
        var result = gestioneCheck(obj);
        manageSchedaLeggeTrasparenzaTabPanel();
        return result;
    }
}

function gestioneCheck(obj) {

    switch (obj.id) {
        case "chkPreimpegni":
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkImpegno").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            Ext.getCmp("chkAccertamento").setValue(false);
            Ext.getCmp("chkRiduzione").setValue(false);
            Ext.getCmp("chkLiquidazione").setValue(false);
            Ext.getCmp("chkRiduzionePreImp").setValue(false);
            Ext.getCmp("chkRiduzioneLiq").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);

            isOpContImpLiq = false;
            isOpContLiq = false;
            isOpContImpLiqPerenti = false;

            break;

        case "chkNessuna":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkImpegno").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            Ext.getCmp("chkAccertamento").setValue(false);
            Ext.getCmp("chkRiduzione").setValue(false);
            Ext.getCmp("chkLiquidazione").setValue(false);
            Ext.getCmp("chkRiduzionePreImp").setValue(false);
            Ext.getCmp("chkRiduzioneLiq").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);

            isOpContImpLiq = false;
            isOpContLiq = false;
            isOpContImpLiqPerenti = false;
            break;

        case "chkImpegno":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkRiduzione").setValue(false);
            Ext.getCmp("chkRiduzioneLiq").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);


            isOpContImpLiq = true;
            //            Ext.MessageBox.buttonText.yes = 'Si';
            //            Ext.MessageBox.show({
            //                title: 'Attenzione',
            //                msg: 'Si intende liquidare una fattura?',
            //                buttons: Ext.MessageBox.YESNO,
            //                icon: Ext.MessageBox.WARNING,
            //                fn: function(btn) {
            //                    if (btn == 'yes') {
            //                        isOpContLiquidazione = true;
            //                    } else {
            //                        isOpContLiquidazione = false;
            //                    }
            //                }
            //            });

            break;

        case "chkLiquidazione":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkRiduzionePreImp").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);

            isOpContLiq = true;

            //           Ext.MessageBox.buttonText.yes = 'Si';
            //           Ext.MessageBox.show({
            //               title: 'Attenzione',
            //               msg: 'Si intende liquidare una fattura?',
            //               buttons: Ext.MessageBox.YESNO,
            //               icon: Ext.MessageBox.WARNING,
            //               fn: function(btn) {
            //                   if (btn == 'yes') {
            //                       isOpContLiquidazione = true;
            //                   } else {
            //                       isOpContLiquidazione = false;
            //                   }
            //               }
            //           });
            //           break;

        case "chkRiduzione":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);
            Ext.getCmp("chkImpegno").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            if (Ext.getCmp("chkLiquidazione").checked == false) {
                Ext.getCmp("chkAccertamento").setValue(false);
                isOpContLiq = false;
            } else {
                isOpContLiq = true;
            }

            isOpContImpLiq = false;
            isOpContImpLiqPerenti = false;
            break;

        case "chkRiduzionePreImp":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            Ext.getCmp("chkLiquidazione").setValue(false);

            if (Ext.getCmp("chkImpegno").checked == false) {
                isOpContImpLiq = false;
            } else {
                isOpContImpLiq = true;
            }
            isOpContLiq = false;
            isOpContImpLiqPerenti = false;
            break;

        case "chkRiduzioneLiq":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkImpegno").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);

            if (Ext.getCmp("chkLiquidazione").checked == false) {
                isOpContLiq = false;
            } else {
                isOpContLiq = true;
            }
            isOpContImpLiq = false;
            isOpContImpLiqPerenti = false;
            break;

        case "chkImpegnoSuPerenti":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkRiduzione").setValue(false);
            Ext.getCmp("chkRiduzionePreImp").setValue(false);
            Ext.getCmp("chkRiduzioneLiq").setValue(false);
            Ext.getCmp("chkAltro").setValue(false);

            isOpContImpLiqPerenti = true;

            //           Ext.MessageBox.buttonText.yes = 'Si';
            //           Ext.MessageBox.show({
            //               title: 'Attenzione',
            //               msg: 'Si intende liquidare una fattura?',
            //               buttons: Ext.MessageBox.YESNO,
            //               icon: Ext.MessageBox.WARNING,
            //               fn: function(btn) {
            //                   if (btn == 'yes') {
            //                       isOpContLiquidazione = true;
            //                   } else {
            //                       isOpContLiquidazione = false;
            //                   }
            //               }
            //           });

            break;

        case "chkAccertamento":
            Ext.getCmp("chkPreimpegni").setValue(false);
            if (tipo == '0') {
                if (Ext.getCmp("chkImpegno").checked ||
                     Ext.getCmp("chkLiquidazione").checked) {

                    Ext.getCmp("chkNessuna").setValue(false);
                    Ext.getCmp("chkRiduzione").setValue(false);
                    Ext.getCmp("chkRiduzionePreImp").setValue(false);
                    Ext.getCmp("chkRiduzioneLiq").setValue(false);
                } else {
                    Ext.getCmp("chkAccertamento").setValue(false);
                }
            }
            else {
                Ext.getCmp("chkNessuna").setValue(false);
            }
            Ext.getCmp("chkAltro").setValue(false);
            if (Ext.getCmp("chkImpegno").checked) {
                isOpContImpLiq = true;
            } else { isOpContImpLiq = false; }
            if (Ext.getCmp("chkLiquidazione").checked) {
                isOpContLiq = true;
            } else { isOpContLiq = false; }
            if (Ext.getCmp("chkImpegnoSuPerenti").checked) {
                isOpContImpLiqPerenti = true;
            } else { isOpContImpLiqPerenti = false; }


            break;

        case "chkAltro":
            Ext.getCmp("chkPreimpegni").setValue(false);
            Ext.getCmp("chkNessuna").setValue(false);
            Ext.getCmp("chkImpegno").setValue(false);
            Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            Ext.getCmp("chkAccertamento").setValue(false);
            Ext.getCmp("chkRiduzione").setValue(false);
            Ext.getCmp("chkLiquidazione").setValue(false);
            Ext.getCmp("chkRiduzioneLiq").setValue(false);
            Ext.getCmp("chkRiduzionePreImp").setValue(false);

            isOpContImpLiq = false;
            isOpContLiq = false;
            isOpContImpLiqPerenti = false;
            break;


    }

    if (Ext.getCmp("chkAltro").checked) {
        Ext.getCmp("ComboSistemazioni").enable()
    } else {
        Ext.getCmp("ComboSistemazioni").disable()
    };

    if ((Ext.getCmp("chkNessuna").checked) || (Ext.getCmp("chkRiduzione").checked)
        || (Ext.getCmp("chkAccertamento").checked) || (Ext.getCmp("chkPreimpegni").checked) || (Ext.getCmp("chkImpegno").checked) || (Ext.getCmp("chkImpegnoSuPerenti").checked)
        || (Ext.getCmp("chkLiquidazione").checked) || (Ext.getCmp("chkRiduzionePreImp").checked) || (Ext.getCmp("chkRiduzioneLiq").checked) || (Ext.getCmp("chkAltro").checked)) {
        window.document.getElementById("controlloCheck").value = '1';
    } else {
        window.document.getElementById("controlloCheck").value = '';
    }


}

function manageSchedaLeggeTrasparenzaTabPanel() {
    var opContabiliSelected = (Ext.getCmp("chkPreimpegni").checked ||
     Ext.getCmp("chkImpegno").checked ||
        Ext.getCmp("chkImpegnoSuPerenti").checked ||
        Ext.getCmp("chkLiquidazione").checked);

    if (1 == 1 || opContabiliSelected) {
        Ext.getCmp("tab_legge_trasparenza").enable();
    } else {
        Ext.getCmp("tab_legge_trasparenza").disable();
    }
}

var valueOggetto = '';
var chkpubIntegrale = false;
var chkpubOggDisp = false;
var chkpubEstratto = false;
var opContabileVuoto = "0;0;0;0;0;0;0;0;0;";
var arrOpContabili = opContabileVuoto.split(";");
var chkNessuno = false;
var tipo = 0;
var codDocumento = '';
var etichetta = 'Carica Provvedimento';
var schedaLeggeTrasparenzaInfo = '';
var schedaContrattiFattureInfo = '';
var schedaTipologiaProvvedimentoInfo = '';

function SetValoriProvvedimenti() {
    valueOggetto = '';
    chkpubIntegrale = false;
    chkpubOggDisp = false;
    chkpubEstratto = false;
    codDocumento = Ext.getDom('codDocumento').value;

    //dati per la scheda 'legge trasparenza'
    var schedaLeggeTrasparenzaInfoJsonValue = Ext.getDom('schedaLeggeTrasparenzaInfo').value;
    if (schedaLeggeTrasparenzaInfoJsonValue != undefined && schedaLeggeTrasparenzaInfoJsonValue != null && schedaLeggeTrasparenzaInfoJsonValue.trim().length != 0)
        schedaLeggeTrasparenzaInfo = Ext.decode(schedaLeggeTrasparenzaInfoJsonValue);

    //dati per la scheda 'tipologia provvedimento'
    var schedaTipologiaProvvedimentoInfoJsonValue = Ext.getDom('schedaTipologiaProvvedimentoInfo').value;
    if (schedaTipologiaProvvedimentoInfoJsonValue != undefined && schedaTipologiaProvvedimentoInfoJsonValue != null && schedaTipologiaProvvedimentoInfoJsonValue.trim().length != 0)
        schedaTipologiaProvvedimentoInfo = Ext.decode(schedaTipologiaProvvedimentoInfoJsonValue);

    //dati per la scheda 'contratti fatture'
    var schedaContrattiFattureInfoJsonValue = Ext.getDom('schedaContrattiFattureInfo').value;
    if (schedaContrattiFattureInfoJsonValue != undefined && schedaContrattiFattureInfoJsonValue != null && schedaContrattiFattureInfoJsonValue.trim().length != 0)
        schedaContrattiFattureInfo = Ext.decode(schedaContrattiFattureInfoJsonValue);



    switch (Ext.getDom('valuePub').value) {
        case '0': { chkpubIntegrale = true; break }
        case '1': { chkpubOggDisp = true; break }
        case '2': { chkpubEstratto = true; break }
    }

    if (Ext.getDom('flagModificato').value != '') {
        Ext.MessageBox.show({
            title: 'Avviso',
            msg: Ext.getDom('flagModificato').value,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.INFO,
            fn: function (btn) {
                maskSalvataggio.hide();
                return;
            }
        })
    }

    etichetta = Ext.getDom('lblEtichetta').value;
    tipo = Ext.getDom('tipo').value;
    valueOggetto = Ext.getDom('valueOggetto').value;

    var valueOpContabile = Ext.getDom('valueOpContabile').value;
    if (valueOpContabile != '') {
        arrOpContabili = valueOpContabile.split(";");
    }

    var tempValueOpContabile = valueOpContabile.replace(/0/g, '').replace(/;/g, '');
    if (tempValueOpContabile >= 1) {
        chkNessuno = false;
    } else {
        if (codDocumento != '') {
            chkNessuno = true;
        }
    }
}

function disableElement(e) {
    if (e.items != undefined && e.items != null) {
        e.items.each(function (c) {
            if (c.getXType() == 'panel') {
                disableElement(c);
            } else if (c.getXType() == 'grid') {
                c.getSelectionModel().lock();
                if (c.getView().dragZone != undefined || c.getView().dragZone != null) {
                    c.getView().dragZone.lock();
                }
                disableElement(c);
            } else if (c.getXType() == 'editorgrid') {
                c.on('beforeedit', function (event) {
                    event.cancel = true;
                }, c);
                c.getSelectionModel().lock();
                disableElement(c);
            }
            else
                c.disable();
        })
    }
}


Ext.onReady(function () {

    var abilitatoCreazioneAttiSiOpCont = Ext.get('abilitatoCreazioneAttiSiOpCont').dom.value;

    Ext.QuickTips.init();
    Ext.Ajax.timeout = 100000000;
    // turn on validation errors beside the field globally
    Ext.form.Field.prototype.msgTarget = 'side';

    //  buildComboSistemazioni()
    SetValoriProvvedimenti();
    buildComboSistemazioni(tipo);
    //  var comboDip = buildComboDipartimenti();

    var visibiTipoPub = true;
    var visibiOpConta = true;

    //Modifica 26/03/2014
    var visibiNotificaAltriUff = true;
    var visibiCodiciCupCig = true;
    var visibiOggettoDoc = true;
    var visibiSchedaLeggeTrasparenza = true;
    var visibiTipologiaProvvedimento = true;

    //Modifica 13/01/2015
    var visibiSchedaContrattiFatture = true;


    var abilitaRegistra = true;

    if (tipo == 2) {
        chkpubIntegrale = true;
        chkpubOggDisp = false;
        chkpubEstratto = false;
        visibiTipoPub = false;
    }

    if (tipo == 3 || tipo == 4) {
        visibiCodiciCupCig = false;
        visibiTipologiaProvvedimento = false;
    }

    //if (tipo == 1) {
    //    chkpubIntegrale = true;
    //    chkpubOggDisp = false;
    //    chkpubEstratto = false;
    //    visibiTipoPub = true;
    //}

    var pubIntegrale = new Ext.form.Radio({
        boxLabel: 'Integrale',
        checked: chkpubIntegrale,
        id: 'Integrale',
        // name: 'TipoPubblicazione',
        name: 'TipoPubblicazione',
        inputValue: '0'
    });

    var pubOggDisp = new Ext.form.Radio({
        boxLabel: 'Per Oggetto + Dispositivo',
        id: 'oggDisp',
        checked: chkpubOggDisp,
        //  name: 'TipoPubblicazione',
        name: 'TipoPubblicazione',
        inputValue: '1'
    });

    var pubEstratto = new Ext.form.Radio({
        boxLabel: 'Estratto Oggetto',
        id: 'estratto',
        checked: chkpubEstratto,
        name: 'TipoPubblicazione',
        inputValue: '2'
    });

    if (codDocumento != '') {
        actionCreaProvvedimento = new Ext.Action({
            text: 'Salva',
            id: 'btnCrea',
            tooltip: 'Salva Provvedimento',
            handler: function () {
                CreaProvvedimento();
            },
            iconCls: 'save'
        });

        actionCreaProvvedimento.setText('Salva');
        Ext.getDom('controlloCheck').value = '1';
    }

    if ((codDocumento == '') && (tipo == '2')) {
        arrOpContabili[2] = '1';
        chkNessuno = false;
        Ext.getDom('controlloCheck').value = '1';
    }

    if ((codDocumento == '') && (tipo == '3' || tipo == '4')) {
        chkNessuno = true;
        Ext.getDom('controlloCheck').value = '1';
    }

    var myPanel = new Ext.FormPanel({
        id: 'myPanel',
        url: 'CreaProvvedimento.aspx' + window.location.search,
        tbar: [actionCreaProvvedimento, actionHomePageUfficio],
        labelAlign: 'top',
        loadMask: true,
        title: etichetta,
        bodyStyle: 'padding:5px',
        width: 800,
        items: [{
            layout: 'column',
            border: false,
            items: [{
                columnWidth: 1,
                layout: 'form',
                height: 155,
                border: false,
                items: [{
                    id: "chkSalva",
                    xtype: "hidden"
                }, {
                    height: 130,
                    xtype: 'textarea',
                    fieldLabel: 'Oggetto',
                    name: 'txtOggetto',
                    id: 'txtOggetto',
                    anchor: '100%',
                    value: valueOggetto,
                    onBlur: function () {
                        if (codDocumento == '') {
                            //  verificaEsistenzaOggetto()
                            //           } else {
                            //            return true
                        }
                    }
                }]
            }]
        }, {
            xtype: 'tabpanel',
            id: 'documentInfo_tabPanel',
            deferredRender: false,
            layoutOnTabChange: true,
            plain: true,
            activeTab: 0,
            autoHeight: true,
            defaults: { bodyStyle: 'padding:10px' },
            items: [{
                title: 'Pubblicazione BUR',
                id: 'tab_pub',
                autoHeight: true,
                defaults: { labelSeparator: '' },
                layout: 'form',
                items: [pubIntegrale, pubOggDisp, pubEstratto]
            },
            {
                title: 'Operazioni Contabili',
                id: 'tab_cont',
                layout: 'form',
                autoHeight: true,
                width: 500,
                defaultType: 'checkbox',
                defaults: { labelSeparator: '' },
                items: [{
                    labelAlign: 'right',
                    boxLabel: 'Nessuna. L\'atto <b>non</b> sarà sottoposto al visto di regolarità contabile.',
                    name: 'chkNessuna',
                    id: 'chkNessuna',
                    labelSeparator: '',
                    checked: chkNessuno,
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Preimpegno',
                    name: 'chkPreimpegni',
                    id: 'chkPreimpegni',
                    checked: arrOpContabili[8] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Impegno ed eventuale liquidazione contestuale',
                    name: 'chkImpegno',
                    id: 'chkImpegno',
                    checked: arrOpContabili[0] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }

                }, {
                    boxLabel: 'Impegno Su Perenti e liquidazione contestuale',
                    name: 'chkImpegnoSuPerenti',
                    id: 'chkImpegnoSuPerenti',
                    checked: arrOpContabili[1] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Liquidazione su Impegno precedentemente assunto con altro provvedimento',
                    name: 'chkLiquidazione',
                    id: 'chkLiquidazione',
                    checked: arrOpContabili[2] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Accertamento',
                    name: 'chkAccertamento',
                    id: 'chkAccertamento',
                    checked: arrOpContabili[3] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Economia (su impegni di anni precedenti) - Riduzione (su impegni dell\'anno corrente e successivi)',
                    name: 'chkRiduzione',
                    id: 'chkRiduzione',
                    checked: arrOpContabili[4] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Riduzione Preimpegno (anno corrente)',
                    name: 'chkRiduzionePreImp',
                    id: 'chkRiduzionePreImp',
                    checked: arrOpContabili[5] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                }, {
                    boxLabel: 'Riduzione Liquidazioni',
                    name: 'chkRiduzioneLiq',
                    id: 'chkRiduzioneLiq',
                    checked: arrOpContabili[6] == '1',
                    onClick: function () {
                        return gestioneClickOpContabili(this, visibiOpConta);
                    }
                },
                  {
                      boxLabel: 'Rettifiche Contabili',
                      name: 'chkAltro',
                      id: 'chkAltro',
                      checked: (arrOpContabili[7] != '0' && arrOpContabili[7] != ''),
                      onClick: function () {
                          return gestioneClickOpContabili(this, visibiOpConta);
                      }
                  }]
            },
    {
        title: 'Legge Trasparenza',
        autoHeight: true,
        defaults: { autoHeight: true },
        layout: 'form',
        id: 'tab_legge_trasparenza',
        items: [
         buildSchedaLeggeTrasparenzaPanel(Ext.getDom('codDocumento').value != "" ? schedaLeggeTrasparenzaInfo : null)
        ]
    },
    {
        title: 'Contratti/Fatture',
        autoHeight: true,
        defaults: { autoHeight: true },
        layout: 'form',
        id: 'tab_contratti_fatture',
        items: [
         buildSchedaContrattiFatturePanel(Ext.getDom('codDocumento').value != "" ? schedaContrattiFattureInfo : null)
        ]
    },
    {
        title: 'Tipologia/Destinatari',
        autoHeight: true,
        defaults: { autoHeight: true },
        layout: 'form',
        id: 'tab_tipologia_provvedimento',
        items: [
         buildSchedaTipologiaProvvedimentoPanel(Ext.getDom('codDocumento').value != "" ? schedaTipologiaProvvedimentoInfo : null)
        ]
    },
            { title: 'Notifica', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ufficio' }
            , { title: 'Codici CUP/CIG', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_attributi', items: [{ id: "AttributiRegistrati", xtype: "hidden" }] }
            ]
        }]
    });

    Ext.getCmp('tab_ufficio').on('activate', function () {
        Ext.getCmp('tab_ufficio').doLayout();
        buildCopyDrop();
        buildCopyDropUtentiUffici();
    });

    Ext.getCmp('tab_ufficio').on('render', function () {
        Ext.getCmp('tab_ufficio').add(buildOnReadyUffici());
    });

    Ext.getCmp('tab_attributi').on('activate', function () {
        refreshSchedaContrattiFatturePanel();
        var cig = "";
        var cup = "";
        if (Ext.getCmp('GridContratti') != undefined) {
            var storeContratti = Ext.getCmp('GridContratti').getStore();
            if (storeContratti.data.length >= 1) {
                cig = storeContratti.data.items[0].data.CodiceCIG;
                cup = storeContratti.data.items[0].data.CodiceCUP;
            }
        }
        if (Ext.getCmp('GridAttributi') == undefined) {
            Ext.getCmp('tab_attributi').add(buildGridAttributi(cig, cup));
        } else {

        }
        Ext.getCmp('tab_attributi').doLayout();
    });


    Ext.getCmp('tab_contratti_fatture').on('activate', function () {
        refreshSchedaContrattiFatturePanel();
    });

    Ext.getCmp('tab_tipologia_provvedimento').on('activate', function () {
        refreshSchedaTipologiaProvvedimentoPanel();
    });

    if (Ext.getDom('flagAbilitaOpContabili').value != '1') {
        visibiOpConta = false;
    }
    // Modifica 26/03/2014
    if (Ext.getDom('flagAbilitaPubblBur').value != '1') {
        visibiTipoPub = false;
    }
    if (Ext.getDom('flagCodificaAltriUff').value != '1') {
        visibiNotificaAltriUff = false;
    }
    if (Ext.getDom('flagCodiciCupCig').value != '1') {
        visibiCodiciCupCig = false;
    }
    if (Ext.getDom('flagAbilitaOggettoDoc').value != '1') {
        visibiOggettoDoc = false;
    }
    if (Ext.getDom('flagAbilitaSchedaLeggeTrasparenza').value != '1') {
        visibiSchedaLeggeTrasparenza = false
    }

    if (Ext.getDom('flagAbilitaTipologiaProvvedimento').value != '1') {
        visibiTipologiaProvvedimento = false
    }

    Ext.getCmp('myPanel').render("myPanelPrincipale");

    if (Ext.getDom('flagRegistra').value != '1') {
        Ext.getCmp("btnCrea").disable();
    }

    if ((tipo == '1') || (tipo == '2') || (tipo == '3') || (tipo == '4')) {

        if (tipo == '2') {
            //CASO DISPOSIZIONE                      
            //Ext.getCmp("tab_cont").insert(0, Ext.getCmp("chkLiquidazione"))                        
            //Ext.getCmp("tab_cont").insert(1, Ext.getCmp("chkAccertamento"))
            //Ext.getCmp("tab_cont").insert(2, Ext.getCmp("chkAltro"))
            //Ext.getCmp("tab_cont").insert(3, Ext.getCmp("ComboSistemazioni"))
            Ext.getCmp("tab_cont").insert(Ext.getCmp("tab_cont").items.length, Ext.getCmp("ComboSistemazioni"));

            Ext.getCmp("chkPreimpegni").disable();
            Ext.getCmp("chkImpegno").disable();
            Ext.getCmp("chkImpegnoSuPerenti").disable();
            Ext.getCmp("chkRiduzione").disable();
            Ext.getCmp("chkNessuna").disable();
            Ext.getCmp("chkRiduzioneLiq").disable();
            Ext.getCmp("chkRiduzionePreImp").disable();

        }
        if (tipo == '3' || tipo == '4') {
            visibiOpConta = false;
            Ext.getCmp('tab_contratti_fatture').disable();

            Ext.getCmp("chkNessuna").disable();
            Ext.getCmp("chkPreimpegni").disable();
            Ext.getCmp("chkImpegno").disable();
            Ext.getCmp("chkImpegnoSuPerenti").disable();
            Ext.getCmp("chkLiquidazione").disable();
            Ext.getCmp("chkAccertamento").disable();
            Ext.getCmp("chkRiduzione").disable();
            Ext.getCmp("chkRiduzionePreImp").disable();
            Ext.getCmp("chkRiduzioneLiq").disable();
            Ext.getCmp("chkAltro").disable();

        }

        //CASO DELIBERA
        // tipoPubblicazBUR: visibile solo Integrale e estratto
        // operazContabili: solo nessuna, preimp e impegno.
        if (tipo == '1') {
            Ext.getCmp('tab_contratti_fatture').disable();

            Ext.getCmp("oggDisp").disable();
            Ext.getCmp("chkImpegno").labelEl.update('Impegno');

            Ext.getCmp("chkNessuna").enable();
            Ext.getCmp("chkPreimpegni").enable();
            Ext.getCmp("chkImpegno").enable();

            Ext.getCmp("chkImpegnoSuPerenti").disable();
            Ext.getCmp("chkLiquidazione").disable();
            Ext.getCmp("chkAccertamento").disable();
            Ext.getCmp("chkRiduzione").disable();
            Ext.getCmp("chkRiduzionePreImp").disable();
            Ext.getCmp("chkRiduzioneLiq").disable();
            Ext.getCmp("chkAltro").disable();
        }

        
    } else {
        //Caso DETERMINA
        Ext.getCmp("tab_cont").insert(Ext.getCmp("tab_cont").items.length, Ext.getCmp("ComboSistemazioni"));
    }


    



    Ext.getCmp("ComboSistemazioni").disable();
    if (arrOpContabili[7] != '0' && arrOpContabili[7] != '') {
        Ext.getCmp("ComboSistemazioni").enable();
    }

    manageSchedaLeggeTrasparenzaTabPanel();



    //Modifica 26/03/2014
    if (!visibiNotificaAltriUff) {
        Ext.getCmp("tab_ufficio").disable();
    }

    if (!visibiCodiciCupCig) {
        Ext.getCmp("tab_attributi").disable();
    }
    if (!visibiCodiciCupCig) {
        Ext.getCmp("tab_attributi").disable();
    }
    if (!visibiOggettoDoc) {
        Ext.getCmp("txtOggetto").disable();
    }

    if (!visibiSchedaLeggeTrasparenza) {
        Ext.getCmp('tab_legge_trasparenza').disable();
    }
    if (!visibiTipologiaProvvedimento) {
        Ext.getCmp('tab_tipologia_provvedimento').disable();
    }

    if (!visibiTipoPub) {
        Ext.getCmp("tab_pub").disable();
        //Ext.getCmp("tab_pub").destroy();
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_cont");
        Ext.getCmp('myPanel').active.show();
    }

    if (!visibiOpConta) {
        Ext.getCmp("tab_cont").disable();
        //Ext.getCmp("tab_pub").destroy();
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_legge_trasparenza");
        Ext.getCmp('myPanel').active.show();
    }
    var queyString = window.location.search;
    if (queyString.indexOf("addFatt=1") != -1) {
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_contratti_fatture");
        Ext.getCmp('myPanel').active.show();
    }

    if (visibiTipoPub) {
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_pub");
        Ext.getCmp('myPanel').active.show();
    }


    if (tipo == '0' && abilitatoCreazioneAttiSiOpCont == 0) {
        Ext.getCmp('tab_contratti_fatture').disable();

        Ext.getCmp("chkNessuna").enable();
        gestioneClickOpContabili(Ext.getCmp("chkNessuna"), 1);
        
        Ext.getCmp("chkPreimpegni").disable();
        Ext.getCmp("chkImpegno").disable();
        Ext.getCmp("chkImpegnoSuPerenti").disable();
        Ext.getCmp("chkLiquidazione").disable();
        Ext.getCmp("chkAccertamento").disable();
        Ext.getCmp("chkRiduzione").disable();
        Ext.getCmp("chkRiduzionePreImp").disable();
        Ext.getCmp("chkRiduzioneLiq").disable();
        Ext.getCmp("chkAltro").disable();
    } else if (abilitatoCreazioneAttiSiOpCont == 0) {
        actionCreaProvvedimento.disable();
        
        Ext.getCmp('tab_pub').disable();
        Ext.getCmp('tab_cont').disable();
        Ext.getCmp("tab_legge_trasparenza").disable();
        Ext.getCmp('tab_contratti_fatture').disable();
        Ext.getCmp('tab_tipologia_provvedimento').disable();
        Ext.getCmp('tab_ufficio').disable();
        Ext.getCmp('tab_attributi').disable();

        Ext.MessageBox.show({
            title: 'Attenzione',
            msg: Ext.get('msgBloccoCreazioneAtti').dom.value,
            buttons: Ext.MessageBox.OK,
            icon: Ext.MessageBox.WARNING,
            fn: function (btn) { return; }
        });
    } 

});


function refreshSchedaContrattiFatturePanel() {
    refreshPanelGridContrattiFatture();
}



function buildOnReadyUffici(enableActions) {
    builFormPanelUtentiUfficiCompetenza();
    builFormPanelUfficiCompetenza();
    buildComboDipartimenti();

    //gestione del caricamento degli uffici notificati salvati, per il documento
    var proxy2 = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetUfficiCompetenzaDocumento' + window.location.search,
        method: 'GET'
    });

    var reader2 = new Ext.data.JsonReader({
        root: 'GetUfficiCompetenzaDocumentoResult',
        fields: [
   { name: 'CodiceInterno' },
   { name: 'DescrizioneBreve' },
   { name: 'DescrizioneToDisplay' }
        ]
    });

    var storeRegistrato = new Ext.data.Store({
        proxy: proxy2,
        reader: reader2
    });

    storeRegistrato.on({
        'load': {
            fn: function (store, records, options) {
                mask.hide();
            },
            scope: this
        }
    });
    storeRegistrato.load();

    builFormPanelUfficiCompetenzaRegistrati(storeRegistrato, enableActions);
    //fine gestione del caricamento degli uffici notificati salvati, per il documento

    //inizio gestione del caricamento degli utenti notificati salvati, per il documento
    var proxyUtentiUfficio = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetUtentiCompetenzaDocumento' + window.location.search,
        method: 'GET'
    });

    var readerUtentiUfficio = new Ext.data.JsonReader({
        root: 'GetUtentiCompetenzaDocumentoResult',
        fields: [{ name: 'Account' }, { name: 'Denominazione' }, { name: 'CodicePubblicoUfficio' }, { name: 'CodiceInternoUfficio' }]
    });

    var storeRegistratoUtentiUfficio = new Ext.data.Store({
        proxy: proxyUtentiUfficio,
        reader: readerUtentiUfficio
    });

    storeRegistratoUtentiUfficio.on({
        'load': {
            fn: function (store, records, options) {
                mask.hide();
            },
            scope: this
        }
    });
    storeRegistratoUtentiUfficio.load();

    builFormPanelUtentiUfficiCompetenzaRegistrati(storeRegistratoUtentiUfficio, enableActions);
    //fine gestione del caricamento degli utenti notificati salvati, per il documento

    var selezionaUfficiNotifica = new Ext.Panel({
        id: 'panelSelezionaUfficiNotifica',
        border: false,
        autoHeight: true,
        items: [Ext.getCmp('myPanelFiltroDipartimento'),
                buildPanelPrincipale()
        ]
    });

    return selezionaUfficiNotifica;
}

function verificaForm(tipo) {
    var lstrErrore = null;

    if (Ext.getCmp('myPanel').getForm().getValues().txtOggetto == '') {
        lstrErrore = { tab_to_activate: null, msg: "E' necessario specificare l'oggetto dell'atto." };
    } else {
        //if (tipo != 1) {
        if (Ext.getCmp('myPanel').getForm().getValues().TipoPubblicazione == undefined) {
            lstrErrore = { tab_to_activate: 'tab_pub', msg: "Scheda 'Pubblicazione BUR' incompleta.<br/>E' necessario specificare un tipo di pubblicazione sul BUR." };
        } else if ((lstrErrore = validateSchedaLeggeTrasparenzaFields('tab_legge_trasparenza')) == null) {
            if (Ext.getDom('controlloCheck').value != '1') {
                lstrErrore = { tab_to_activate: 'tab_cont', msg: "Scheda 'Operazioni Contabili' incompleta.<br/>E' necessario selezionare una operazione contabile." };
            } else if (Ext.getCmp("chkAltro") != undefined && Ext.getCmp("chkAltro").checked && Ext.getDom('ValueSistemazioni').value == '') {
                lstrErrore = { tab_to_activate: 'tab_cont', msg: "Scheda 'Operazioni Contabili' incompleta.<br/>E' necessario selezionare una sistemazione." };
            } else if ((lstrErrore = validateValoriAttributi()) == null) {
                if ((lstrErrore = validateSchedaTipologiaProvvedimentoFields('tab_tipologia_provvedimento')) == null) {
                    if ((lstrErrore = validateSchedaContrattiFattureFields('tab_contratti_fatture')) == null) {
                        //                                        lstr_errore = { tab_to_activate: 'tab_contratti_fatture', msg: "Scheda 'Contratti-Fatture' incompleta.<br/>" };
                    }
                    //other checks
                }
            }
        }
        //}
    }

    return lstrErrore;
}

function validateValoriAttributi() {
    var retValue = null;

    var codiceCIG = getValoreAttributo('CIG');
    var codiceCUP = getValoreAttributo('CUP');

    if (codiceCUP != null && codiceCUP != undefined && codiceCUP != "" && (codiceCUP.length > 15 || codiceCUP.length < 15)) {
        retValue = { tab_to_activate: 'tab_attributi', msg: "La lunghezza del codice CUP non è valida: sono obbligatori 15 caratteri." };
    }

    if (retValue == null && codiceCIG != null && codiceCIG != undefined && codiceCIG != "" && (codiceCIG.length > 10 || codiceCIG.length < 10)) {
        retValue = { tab_to_activate: 'tab_attributi', msg: "La lunghezza del codice CIG non è valida: sono obbligatori 10 caratteri." };
    }

    return retValue;
}
