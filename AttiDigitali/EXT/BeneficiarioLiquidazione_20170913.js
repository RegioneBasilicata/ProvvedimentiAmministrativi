Ext.IframeWindow = Ext.extend(Ext.Window, {
    onRender: function() {
        this.bodyCfg = {
            tag: 'iframe',
            src: this.src,
            cls: this.bodyCls,
            style: {
                border: '0px none'
            }
        };

        Ext.IframeWindow.superclass.onRender.apply(this, arguments);
    }
});

function fnOnSelectSearchField(selectedField) {
    var btnSelezionaBen = Ext.getCmp('btnSelezionaBen');
    if (btnSelezionaBen != undefined && btnSelezionaBen != null)
        btnSelezionaBen.disable();
}

function fnOnAfterSearchCommand() {
    Ext.getCmp("tab_panel").doLayout();
    Ext.getCmp("tab_panel").hide();
}

function fnOnStoreLoad(records) {
    if(records.length==0) {
       if (Ext.getCmp('btnSelezionaBen') != null && Ext.getCmp('btnSelezionaBen') != undefined)
            Ext.getCmp('btnSelezionaBen').disable();
    }
}

function fnOnSelectBeneficiario(selectedRow) {
    if (selectedRow != null) {
        var beneficiario = selectedRow.data;     
           
        Ext.getCmp('anagrafica').setValue(Ext.util.JSON.encode(beneficiario));
        Ext.get("idAnagrafica").value = beneficiario.ID;
        
        if (Ext.getCmp('tab_datiBeneficiarioLiquidazione') != undefined) 
              Ext.getCmp("tab_datiBeneficiarioLiquidazione").enable();
    
        if (Ext.getCmp('sedeSelectionPanel') != undefined) 
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').remove(Ext.getCmp('sedeSelectionPanel'));        
            
        if (Ext.getCmp('beneficiarioInfoPanel') != undefined) 
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').remove(Ext.getCmp('beneficiarioInfoPanel'));
            
        if (Ext.getCmp('contrattoSelectionPanel') != undefined)
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').remove(Ext.getCmp('contrattoSelectionPanel'));        
            
        if (Ext.getCmp('datiLiquidazionePanel') != undefined)
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').remove(Ext.getCmp('datiLiquidazionePanel'));

        if (Ext.getCmp('beneficiarioQueryPanel') != undefined)
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').remove(Ext.getCmp('beneficiarioQueryPanel'));

        Ext.getCmp('tab_datiBeneficiarioLiquidazione').insert(0, buildBeneficiarioInfoPanel(beneficiario));
        Ext.getCmp('tab_datiBeneficiarioLiquidazione').insert(1, buildBeneficiarioQueryPanel(beneficiario));
        Ext.getCmp('tab_datiBeneficiarioLiquidazione').insert(2, buildContrattoSelectionPanel(beneficiario));
        Ext.getCmp('tab_datiBeneficiarioLiquidazione').insert(3, buildSedeSelectionPanel(beneficiario));
                                       
        Ext.getCmp('tab_panel').show();
        setActivePanel('panelBeneficiarioLiquidazioneImpegno', 'tab_datiBeneficiarioLiquidazione');
        Ext.getCmp('tab_panel').doLayout();
    } else {
        if (Ext.getCmp('tab_datiBeneficiarioLiquidazione') != null && Ext.getCmp('tab_datiBeneficiarioLiquidazione') != undefined) 
              Ext.getCmp("tab_datiBeneficiarioLiquidazione").disable();

        Ext.getCmp('tab_panel').hide();
        
        if (Ext.getCmp('btnSelezionaBen') != null && Ext.getCmp('btnSelezionaBen') != undefined)
                Ext.getCmp('btnSelezionaBen').disable();
    }
}

function validatePanelDatiLiquidazioneImpegnoFields() {
    return (Ext.getCmp('ImpSpettante') != null && Ext.getCmp('ImpSpettante') != undefined && Ext.getCmp('ImpSpettante').validate()) &&
            (Ext.getCmp('CodiceCig') != null && Ext.getCmp('CodiceCig') != undefined && Ext.getCmp('CodiceCig').validate()) &&
            (Ext.getCmp('CodiceCup') != null && Ext.getCmp('CodiceCup') != undefined && Ext.getCmp('CodiceCup').validate());
}

function checkDatiSensibiliBeneficiario() {    
    return !((Ext.getCmp('yesDatoSensibile') == undefined || Ext.getCmp('yesDatoSensibile').checked == false) &&
            (Ext.getCmp('noDatoSensibile') == undefined || Ext.getCmp('noDatoSensibile').checked == false));    
}

function checkContrattoBeneficiario() {
    var retValue = true;

    var combobox = Ext.getCmp('comboContrattiBeneficiarioId');
    if (combobox != undefined) {
        if (combobox.store != undefined ) {
            var count = combobox.store.getCount();
            if (count > 1) {
                var contratto = getContrattoInfo(combobox.getValue(), "comboContrattiBeneficiarioId");
                if (contratto == undefined) 
                    retValue = false;  
            }
        }
    }
    return retValue;
}

function buildPanelDatiLiquidazione(bilancio, capitolo) {
    Ext.override(Ext.form.Field, {
        fireKey: function(e) {
            if (((Ext.isIE && e.type == 'keydown') || e.type == 'keypress') && e.isSpecialKey()) {
                this.fireEvent('specialkey', this, e);
            } else {
                this.fireEvent(e.type, this, e);
            }
        }, 
        initEvents: function() {         
              this.el.on("focus", this.onFocus, this);
              this.el.on("blur", this.onBlur, this);
              this.el.on("keydown", this.fireKey, this);
              this.el.on("keypress", this.fireKey, this);
              this.el.on("keyup", this.fireKey, this);
              // reference to original value for reset
              this.originalValue = this.getValue();
        }
    });

    Ext.override(Ext.layout.TableLayout, {
       tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 0, cn: { tag: 'tbody'} },
        onLayout: function(ct, target) {
            var cs = ct.items.items, len = cs.length, c, i;
            if (!this.table) {
                target.addClass('x-table-layout-ct');
                this.table = target.createChild(this.tableCfg, null, true);
                this.renderAll(ct, target);
            }
        }
    });

    var comboCodiciSiope = new Ext.form.ComboBox({
                id: "idCodiceSiope",
                hiddenName: "CodiceSiope",
                listWidth: 340,
                width: 340,
                displayField: 'Descrizione',
                valueField: 'Id',
                queryMode: 'local',
                mode: 'local',
                tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><b>{Id}</b><br/>{Descrizione}</div></tpl>',
                store: new Ext.data.JsonStore({
                    url: 'ProcAmm.svc/GetListaCodiciSiope?AnnoRif=' + bilancio + '&CapitoloRif=' + capitolo,
                    method: 'GET',
                    readOnly: false,
                    root: 'GetListaCodiciSiopeResult',
                    fields: [{ name: 'Descrizione' }, { name: 'Id'}],
                    autoLoad: true
                }),
                hideTrigger: false,                
                emptyText: 'Selezionare un codice Siope...',
                typeAhead: true,
                forceSelection: true,
                triggerAction: 'all',
                selectOnFocus: true                
    });
   
    var datiLiquidazionePanel = new Ext.Panel({
        layout: 'table',
        id: 'datiLiquidazionePanel',
        autoHeight: true,
        labelWidth: 50,
        border: false,
        xtype: "panel",
        layoutConfig: {
            columns: 2
            , tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        style: "margin-top:3px",
        items: [
            {
                xtype: 'label',
                text: 'Codice Cup  '
            },
            { xtype: 'panel',
                bodyStyle: 'padding-top:' + (Ext.isIE ? '1' : '0') + 'px; margin-bottom:' + (Ext.isIE ? '-1' : '0') + 'px',
                border: false,
                items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Codice Cup',
                    name: 'CodiceCup',
                    id: 'CodiceCup',
                    width: 200,
                    listeners: {
                        scope: this,
                        'keyup': function() {
                            if (!checkCodiceCUP() && Ext.getCmp('CodiceCup').isValid())
                                Ext.getCmp('codiceCUPAlert').show();
                            else
                                Ext.getCmp('codiceCUPAlert').hide();
                        }
                    },
                    minLength: 15,
                    minLengthText: 'La lunghezza è di 15 caratteri',
                    maxLength: 15,
                    maxLengthText: 'La lunghezza è di 15 caratteri'
                }, {
                    xtype: 'label',
                    id: 'codiceCUPAlert',
                    html: codiceCUPAlert(),
                    hidden: true
                }]
                },
             {
                 xtype: 'label',
                 text: 'Codice Cig  '
             },
                { xtype: 'panel',
                    border: false,
                    bodyStyle: 'padding-top:' + (Ext.isIE ? '1' : '0') + 'px',
                    items: [{
                        xtype: 'textfield',
                        fieldLabel: 'Codice Cig',
                        name: 'CodiceCig',
                        id: 'CodiceCig',
                        width: 200,
                        listeners: {
                            scope: this,
                            'keyup': function() {
                                if (!checkCodiceCIG() && Ext.getCmp('CodiceCig').isValid())
                                    Ext.getCmp('codiceCIGAlert').show();
                                else
                                    Ext.getCmp('codiceCIGAlert').hide();
                            }
                        },
                        minLength: 10,
                        minLengthText: 'La lunghezza è di 10 caratteri',
                        maxLength: 10,
                        maxLengthText: 'La lunghezza è 10 caratteri'
                    }, {
                        xtype: 'label',
                        id: 'codiceCIGAlert',
                        html: codiceCIGAlert(),
                        hidden: true
}]
                    }, {
                        xtype: 'label',
                        text: 'Codice Siope  '
                    },  { xtype: 'panel',
                    border: false,                    
                    bodyStyle: 'margin-bottom:' + (Ext.isIE ? '1' : '0') + 'px' + ';margin-top:' + (Ext.isIE ? '-1' : '0') + 'px',
                    items: [comboCodiciSiope]},
            {
                xtype: 'label',
                text: 'Importo Spettante(*)',
                style: 'padding-right:8px'
            },           
             {
                xtype: 'textfield',
                name: 'ImpSpettante',
                id: 'ImpSpettante',
                style: 'background-color: ' + (!Ext.isIE ? '#' : '') + 'fffb8a; background-image: none',
                width: 200,
                allowBlank: false,
                blankText: 'Campo Obbligatorio',                
                validator: function(valu) {
                    var pattern = /^-?(0|[1-9]\d*)(\,\d{1,2})?$/
                    var stringa = valu.trim()
                    var result = stringa.search(pattern)
                    return result > -1;
                },
                invalidText: "L'importo inseriro non è valido"
            }
        ]
    });
            
    
    
    var benSelezionato = Ext.getCmp('GridRicerca').getSelectionModel().getSelections();
    if (benSelezionato != undefined) {
        var CIG = benSelezionato[0].data.Contratto.CodiceCIG;
        var CUP = benSelezionato[0].data.Contratto.CodiceCUP;
        if (CIG != undefined && CIG != "") {
            updateCIGField(CIG);
        }
        if (CUP != undefined && CUP != "") {
            updateCUPField(CUP);
        }
        var importoSpettanteDaInializzare = 0;
        if (Ext.getCmp("importoSpettanteBeneficiario") != undefined && Ext.getCmp("importoSpettanteBeneficiario").value != undefined && Ext.getCmp("importoSpettanteBeneficiario").value != "") {
            importoSpettanteDaInializzare = Ext.getCmp("importoSpettanteBeneficiario").value;
        } else {
            importoSpettanteDaInializzare = benSelezionato[0].data.ImportoSpettante;
        }
        updateImportoSpettanteField(importoSpettanteDaInializzare);
        
    }
    if (Ext.getCmp('CodiceCup').value == undefined || Ext.getCmp('CodiceCup').value == '' || Ext.getCmp('codiceCupInAtto').value == undefined || Ext.getCmp('codiceCupInAtto').value == '' ) {
        getAttributoDocumento('CUP', updateCUPField);
    }
    if (Ext.getCmp('CodiceCig').value == undefined || Ext.getCmp('CodiceCig').value == '' || Ext.getCmp('codiceCigInAtto').value == undefined || Ext.getCmp('codiceCigInAtto').value == '') {
        getAttributoDocumento('CIG', updateCIGField);
    }
         
    return datiLiquidazionePanel;
}

function buildPanelBeneficiarioLiquidazioneImpegno(liquidazione, impegno, fatturePresentiSuLiq) {


    

    var actionAggiungiAnagrafica = new Ext.Action({
        text: 'Nuovo Beneficiario',
        tooltip: 'Aggiungi un nuovo beneficiario',
        handler: function() {
                InitFormInsertAnagrafica(objectTypes.anagrafica,
                        insertModes.newObject, {
                            updateFn: editInsertUpdateComboFn
                        });
        },
        iconCls: 'add'
    });

    var actionHelpOnLine = new Ext.Action({
        text: 'Aiuto',
        tooltip: 'Aiuto in linea: Associazione Beneficiario-Liquidazione',
        handler: function() {
            new Ext.IframeWindow({
                modal: true,
                layout: 'fit',
                title: 'Associazione Beneficiario-Liquidazione',
                width: 720,
                height: 500,
                closable: true,
                resizable: false,
                maximizable: false,
                plain: false,
                iconCls: 'help',
                bodyStyle: 'overflow:auto',
                src: 'risorse/helpAggiuntaBeneficiarioLiquidazione.htm'
            }).show();
        },
        iconCls: 'help'
    });

        
    var panelBeneficiarioLiquidazioneImpegno = new Ext.FormPanel({        
        layout: 'form',
        id: 'panelBeneficiarioLiquidazioneImpegno',
        bodyStyle: 'padding:10px',
        height: 450,
        autoScroll: true,
        tbar:[actionAggiungiAnagrafica,'->', actionHelpOnLine],
        items: [
             buildPanelSearchBeneficiari(true, true, undefined, fnOnAfterSearchCommand, fnOnStoreLoad, fnOnSelectBeneficiario, fnOnSelectSearchField, undefined, undefined, liquidazione, impegno, fatturePresentiSuLiq),
             {
                xtype: 'tabpanel',
                plain: true,
                id: 'tab_panel',
                autoHeight: true,
                defaults: { bodyStyle: 'padding:10px' },
                style: {
                    "margin-top": "10px",
                    "margin-bottom": "10px"
                },
                hidden: true,
                deferredRender: false,
                items: [
                         { title: 'Scheda Beneficiario',
                           layout: 'auto',
                           autoWidth: true,
                           autoHeight: true, defaults: { autoHeight: true }, 
                           id: 'tab_datiBeneficiarioLiquidazione' }
                    ]
            },{
                xtype: 'hidden',
                id: 'hidLiquidazione'
            }, {
                xtype: 'hidden',
                id: 'hidImpegno'
            }, {
                xtype: 'hidden',
                id: 'importoSpettanteBeneficiario'
            }, {
                xtype: 'hidden',
                id: 'IdDocumento'
            }, {
                xtype: 'hidden',
                id: 'ID',
                value: 0
            }, {
                xtype: 'hidden',
                id: 'anagrafica'
            }, {
                xtype: 'hidden',
                id: 'sede'
            }, {
                xtype: 'hidden',
                id: 'conto'
            }, {
                xtype: 'hidden',
                id: 'codiceCigInAtto'
            }, {
                xtype: 'hidden',
                id: 'codiceCupInAtto'
            }, {
                xtype: 'hidden',
                id: 'liquidazioneData'
            }, {
                xtype: 'hidden',
                id: 'impegnoData'
            }, {
                xtype: 'hidden',
                id: 'idAnagrafica'
            }, {
                xtype: 'hidden',
                id: 'idSede'
            }, {
                xtype: 'hidden',
                id: 'idConto'
            }
        ]
    });
    
    
    if (liquidazione != undefined) {
        Ext.getCmp('liquidazioneData').setValue(Ext.util.JSON.encode(liquidazione));
        Ext.getCmp('IdDocumento').setValue(liquidazione.IdDocumento);
    }    
    if (impegno != undefined) {
        Ext.getCmp('impegnoData').setValue(Ext.util.JSON.encode(impegno));
        Ext.getCmp('IdDocumento').setValue(impegno.IdDocumento);
    }
                                                    
    return panelBeneficiarioLiquidazioneImpegno;
}

function updateCUPField(CUPValue) {
    Ext.getCmp('CodiceCup').setValue(CUPValue);
    Ext.getCmp('codiceCupInAtto').setValue(CUPValue);
}

function updateCIGField(CIGValue) {
    Ext.getCmp('CodiceCig').setValue(CIGValue);
    Ext.getCmp('codiceCigInAtto').setValue(CIGValue);
}

function updateImportoSpettanteField(ImportoSpettanteValue) {
    ImportoSpettanteValue = ImportoSpettanteValue.toString();
    ImportoSpettanteValue = ImportoSpettanteValue.replace('.', ',');
    if(ImportoSpettanteValue != '0')
        Ext.getCmp('ImpSpettante').setValue(ImportoSpettanteValue);
}

function checkCodiceCUP() {
    var codiceCupAtto = Ext.getCmp('codiceCupInAtto').getValue()
    var codiceCup = Ext.getCmp('CodiceCup').getValue();
    
    return codiceCupAtto.trim()==codiceCup.trim() || codiceCupAtto.trim()=="";
}

function checkCodiceCIG() {
    var codiceCigAtto = Ext.getCmp('codiceCigInAtto').getValue()
    var codiceCig = Ext.getCmp('CodiceCig').getValue();
    
    return codiceCigAtto.trim()==codiceCig.trim() || codiceCigAtto.trim()=="";
}

function codiceCUPAlert() {
    var html = "<div class=\"alertMessage\">"+
               "<img src=\"/Attidigitali/risorse/immagini/exclamationButton.gif\" alt=\"\" />&nbsp;&nbsp;"+
               "Codice CUP differente da quello indicato nell'atto." +
               "&nbsp;&nbsp;<a href='#' style=\"font-size:10px;text-decoration:underline;color:#F85118\" onClick=\"restoreCUPCode(); return false;\">Ripristina Valore</a>" +
               "</div>";

    return html;
}

function codiceCIGAlert() {
    var html = "<div class=\"alertMessage\">" +
               "<img src=\"/Attidigitali/risorse/immagini/exclamationButton.gif\" alt=\"\" />&nbsp;&nbsp;" +
               "Codice CIG differente da quello indicato nell'atto." +
               "&nbsp;&nbsp;<a href='#' style=\"font-size:10px;text-decoration:underline;color:#F85118\" onClick=\"restoreCIGCode(); return false;\">Ripristina Valore</a>" +
               "</div>";

    return html;
}

function restoreCIGCode() {
    Ext.getCmp('CodiceCig').setValue(Ext.getCmp('codiceCigInAtto').getValue());
    Ext.getCmp('codiceCIGAlert').hide();
}

function restoreCUPCode() {
    Ext.getCmp('CodiceCup').setValue(Ext.getCmp('codiceCupInAtto').getValue());
    Ext.getCmp('codiceCUPAlert').hide();
}

function buildContrattoSelectionPanel(beneficiario) {

    var actionVisualizzaContratto = new Ext.Action({
        text: 'Visualizza Contratto',
        id: 'actionApriContrattoId',
        tooltip: 'Visualizza il contratto relativo al numero di repertorio indicato',
        handler: function() {
            var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
            var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();
            var num_contr = comboContratti.getValue();

            window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + num_contr, '_blank');
        }
    });

    var comboContratti = buildComboContrattiDocumento("comboContrattiBeneficiarioId", 515, false,
        function(combo, value) {
            var contratto = getContrattoInfo(value.data.Id, "comboContrattiBeneficiarioId");
            var anagrafica = Ext.util.JSON.decode(Ext.getCmp("anagrafica").getValue());

            anagrafica.Contratto.Id = contratto.Id
            anagrafica.Contratto.NumeroRepertorio = contratto.NumeroRepertorio;

            Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(anagrafica));

            if (comboContratti.getValue() == 'none') {
                comboContratti.setValue('');
                Ext.getCmp("oggettoContratto").setValue('');
                actionVisualizzaContratto.disable();
                Ext.getCmp('oggettoContratto').disable();
            } else {
                actionVisualizzaContratto.enable();
                Ext.getCmp('oggettoContratto').enable();
                Ext.getCmp("oggettoContratto").setValue(contratto.Oggetto);
            }
        },
        function(store, records, options) {
            if (records.length == 0) {
                Ext.getCmp('labelSelectionContrattoId').disable();
                Ext.getCmp('comboContrattiBeneficiarioId').disable();
                Ext.getCmp('actionApriContrattoId').disable();
                Ext.getCmp('labelOggettoContrattoId').disable();
                Ext.getCmp('oggettoContratto').disable();
            } else {
                Ext.getCmp('labelSelectionContrattoId').enable();
                Ext.getCmp('comboContrattiBeneficiarioId').enable();
                Ext.getCmp('actionApriContrattoId').enable();
                Ext.getCmp('labelOggettoContrattoId').enable();
                Ext.getCmp('oggettoContratto').enable();

                if (beneficiario.Contratto != undefined && beneficiario.Contratto != null &&
                     !isNullOrEmpty(beneficiario.Contratto.Id)) 
                {
                    comboContratti.setValue(beneficiario.Contratto.Id);
                    comboContratti.disable();

                    actionVisualizzaContratto.enable();

                    var contratto = getContrattoInfo(comboContratti.getValue(), "comboContrattiBeneficiarioId");
                    
                    Ext.getCmp('oggettoContratto').disable();                    
                    Ext.getCmp("oggettoContratto").setValue(contratto.Oggetto);
                } else {
                    actionVisualizzaContratto.disable();
                    Ext.getCmp('oggettoContratto').disable();
                }
            }
        }
    );

        var panel = new Ext.Panel({
            border: false,
            style: 'margin-left:2px;margin-top:8px;margin-bottom:8px',            
            id: "contrattoSelectionPanel",
            items: [
            { xtype: 'panel',
                layout: 'table',
                border: false,
                layoutConfig: {
                    columns: 2
                },
             items: [
                {
                    xtype: 'label',
                    id: 'labelSelectionContrattoId',
                    text: 'Repertorio Contratto  ',
                    style: Ext.isIE ? 'padding-right:9px' : 'padding-right:12px'
                },
                {
                layout: "column",
                border: false,
                anchor: "0",
                width: 636,
                items: [{
                    border: false,
                    style: 'margin-right: 10px',
                    items: [
					comboContratti
				]
                }, {
                    border: false,
                    items: [
					new Ext.LinkButton(actionVisualizzaContratto)
				]
                }]
                }]
            },
            { xtype: 'panel',
                layout: 'table',
                border: false,
                style: Ext.isIE ? 'margin-top: 1px' : 'margin-top: 2px',                
                layoutConfig: {
                    columns: 2
                },
                items: [
                {
                    xtype: 'label',
                    id: 'labelOggettoContrattoId',
                    text: 'Oggetto  ',
                    style: Ext.isIE ? 'padding-right:55px' : 'padding-right:58px'
                },{
                xtype: 'textarea',
                maxLength: 1024,
                readOnly: true,
                id: 'oggettoContratto',
                style: 'margin-left: 15px',
                width: Ext.isIE ? 516: 515,
                height: 34
            }
                ]
            }                                                            
        ]
        });
            
    return panel;
}

function setIsDatoSensibile(isDatoSensibile) {
    var anagrafica = Ext.util.JSON.decode(Ext.getCmp("anagrafica").getValue());
    anagrafica.IsDatoSensibile = isDatoSensibile;
    Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(anagrafica));
}

function buildBeneficiarioQueryPanel(beneficiario) {
    var yesOption = new Ext.form.Radio({
        boxLabel: 'Si',
        id: 'yesDatoSensibile',
        name: 'selezioneDatoSensibile',
        inputValue: 'SI',
        checked: false
    });

    var noOption = new Ext.form.Radio({
        boxLabel: 'No',
        id: 'noDatoSensibile',
        name: 'selezioneDatoSensibile',
        inputValue: 'NO',
        checked: false
    });

    yesOption.on('check', function() {
        if (Ext.getCmp('yesDatoSensibile').checked) {
            setIsDatoSensibile(true);
        }
    });

    noOption.on('check', function() {
        if (Ext.getCmp('noDatoSensibile').checked) {
            setIsDatoSensibile(false);
        }
    });

    var panel = new Ext.Panel({
        border: false,
        layout: 'table',
        style: 'margin-bottom:2px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        id: "beneficiarioQueryPanel",
        items: [{
            xtype: 'label',
            text: 'Dato sensibile(*)',
            style: Ext.isIE ? 'padding-right:31px' : 'padding-right:31px'
        }, {
            xtype: 'panel',
            border: false,
            layout: 'table',
            layoutConfig: {
                columns: 3
            },
            items: [yesOption,
                {
                    xtype: 'label',
                    style: 'padding-right:10px',
                    text: ''
                },
                noOption]
        }

                    ]
    });

    return panel;
}

function buildBeneficiarioInfoPanel(beneficiario) {
    var nominativo = beneficiario.Tipologia=='F' ? beneficiario.Cognome + ' ' + beneficiario.Nome : beneficiario.Denominazione; 
    var partitaIvaCod = beneficiario.Tipologia=='F' ? beneficiario.CodiceFiscale : beneficiario.PartitaIva;

    var labelText1 = 'Nominativo <b>' + nominativo + '</b> - Persona <b>' + (beneficiario.Tipologia == 'F' ? 'FISICA' : 'GIURIDICA') + '</b>';
    var labelText2 = ''
    var labelText3 = ''
    
    if (!isNullOrEmpty(partitaIvaCod)) 
        labelText2 += (beneficiario.Tipologia == 'F' ? 'Codice Fiscale ':'Partita IVA ')+'<b>'+partitaIvaCod+'</b>';
    if (beneficiario.Tipologia == 'F')
        labelText3 += 'Data di nascita <b>' + beneficiario.DataNascita + '</b> - Luogo di nascita <b>' + beneficiario.ComuneNascita + '</b>';
    else {
        var legaleRappresentante = "";

        if (!isNullOrEmpty(beneficiario.LegaleRappresentante.Cognome))
            legaleRappresentante += beneficiario.LegaleRappresentante.Cognome;
        if (!isNullOrEmpty(beneficiario.LegaleRappresentante.Nome))
            legaleRappresentante += " " + beneficiario.LegaleRappresentante.Nome;
        
        if(legaleRappresentante!="")   
            labelText3 += 'Legale Rappresentante <b>' + legaleRappresentante + '</b>';
    }

    var panel = new Ext.Panel({
        border: false,
        layout: 'table',
        style: 'margin-bottom:-2px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 2, cn: { tag: 'tbody'} }
        },
        id: "beneficiarioInfoPanel",
        items: [
        {
            xtype: 'label',
            text: 'Beneficiario',
            style: 'padding-right:56px'
        }, {
            xtype: 'panel',
            plain: true,
            border: true,
            layout: 'table',
            bodyStyle: 'padding:4px 4px 4px 5px',
            width: Ext.isIE ? 514 : 515,
            layoutConfig: {
                columns: 1
            },
            items: [
                {
                    xtype: 'label',
                    id: 'labelBeneficiarioInfo1Id',
                    html: labelText1,
                    style: 'padding-right:85px'
                },
                {
                    xtype: 'label',
                    id: 'labelBeneficiarioInfo2Id',
                    html: labelText2,
                    style: 'padding-right:85px'
                }, (labelText3.trim().length != 0 ?
                {
                    xtype: 'label',
                    id: 'labelBeneficiarioInfo3Id',
                    html: labelText3,
                    style: 'padding-right:85px'
                } : {xtype: 'label'})
                
            ]
        }
        ]
    });
    
    return panel;
}

function getContrattoInfo(idContratto, idComboContratti) {
    var retValue = undefined;

    var comboContratti = Ext.getCmp(idComboContratti);
    if (comboContratti != undefined && comboContratti != null) {
        var record = comboContratti.findRecord(comboContratti.valueField || comboContratti.displayField, idContratto);
        if (record != undefined && record != null)
            retValue = record.data;
    }

    return retValue;
}

function displayContrattoInfo(contratto) {
    if (contratto != undefined && contratto != null) {

        var popup = new Ext.Window({
            width: 472,
            title: 'Dettagli Contratto',
            layout: 'fit',
            plain: true,
            bodyStyle: 'padding:10px',
            resizable: false,
            buttonAlign: 'center',
            maximizable: false,
            enableDragDrop: true,
            collapsible: false,
            modal: true,
            autoScroll: true,
            closable: false,
            items: [
               { xtype: 'label',                   
                   html: '<table>'+
                         '<tr>'+
                         '<td style="text-align:right;padding-right:8px;vertical-align:top"><b>Repertorio:</b></td>' +
                         '<td>'+contratto.NumeroRepertorio+'</td>'+
                         '</tr>'+
                         '<tr>'+
                         '<td style="text-align:right;padding-right:8px;vertical-align:top;"><b>Oggetto:</b></td>' +
                         '<td>'+contratto.Oggetto+'</td>'+
                         '</tr>'+
                         '</table>'                              
               }
            ],
            buttons: [{
                text: 'OK',
                handler: function() { popup.close(); }
            }]
        });

        popup.show();        
        } else {
            Ext.MessageBox.show({
                title: 'Dettagli Contratto',
                msg: 'Impossibile recuperare le informazioni per il contratto selezionato.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.ERROR,
                fn: function(btn) {
                    popup.close();
                    return;
                }
            });
        }
}


function getSediStore(listaSedi) {
    var store = new Ext.data.Store();

    if (listaSedi != null) {
        var TipoRecord = Ext.data.Record.create([
    			     { name: 'Comune' }
                   , { name: 'CapComune' }
                   , { name: 'Email' }
                   , { name: 'Fax' }
                   , { name: 'Telefono' }
                   , { name: 'Bollo' }
                   , { name: 'Indirizzo' }
                   , { name: 'ModalitaPagamento' }
                   , { name: 'DatiBancari' }
                   , { name: 'IdSede' }
                   , { name: 'IdModalitaPagamento' }
                   , { name: 'NomeSede' },
                   , { name: 'HasDatiBancari' }
                   , { name: 'DisplayRow' }
			    ]);

        for (var i = 0; i < listaSedi.length; i++) {
            var record = new TipoRecord({
                Comune: listaSedi[i].Comune
                     , CapComune: listaSedi[i].CapComune
                     , Email: listaSedi[i].Email
                     , Fax: listaSedi[i].Fax
                     , Bollo: listaSedi[i].Bollo
                     , Telefono: listaSedi[i].Telefono
                     , Indirizzo: listaSedi[i].Indirizzo
                     , DatiBancari: listaSedi[i].DatiBancari
                     , ModalitaPagamento: listaSedi[i].ModalitaPagamento
                     , IdSede: listaSedi[i].IdSede
                     , IdModalitaPagamento: listaSedi[i].IdModalitaPagamento
                     , NomeSede: listaSedi[i].NomeSede
                     , HasDatiBancari: listaSedi[i].HasDatiBancari
                     , DisplayRow: listaSedi[i].NomeSede + ': ' + listaSedi[i].Indirizzo + ' - ' + (!isNullOrEmpty(listaSedi[i].CapComune) ? listaSedi[i].CapComune + ' ': '') + listaSedi[i].Comune
            });
            store.insert(i, record);
        }
    }

    return store;
}

function getAllContiStore(listaSedi) {
    var store = new Ext.data.Store();

    if (listaSedi != null) {
        var TipoRecord = Ext.data.Record.create([
    			     { name: 'Comune' }
                   , { name: 'CapComune' }
                   , { name: 'Email' }
                   , { name: 'Fax' }
                   , { name: 'Telefono' }
                   , { name: 'Bollo' }
                   , { name: 'Indirizzo' }
                   , { name: 'ModalitaPagamento' }
                   , { name: 'DatiBancari' }
                   , { name: 'IdSede' }
                   , { name: 'IdModalitaPagamento' }
                   , { name: 'NomeSede' },
                   , { name: 'HasDatiBancari' },
                   , { name: 'ContoCorrente' }
                   , { name: 'Iban' }
                   , { name: 'IdContoCorrente' }
                   , { name: 'NomeBanca' }
                   , { name: 'DisplayRow' }
                   , { name: 'ValueConto' }
			    ]);

    	var index = 0;
    	for (var i = 0; i < listaSedi.length; i++) {
    	    if(listaSedi[i].DatiBancari!=null && listaSedi[i].DatiBancari!=undefined) {
    	        var datiBancari = listaSedi[i].DatiBancari;
    	        for (var j = 0; j < datiBancari.length; j++) {    	            
    	            var record = new TipoRecord({
    	                    Comune: listaSedi[i].Comune
                             , CapComune: listaSedi[i].CapComune
                             , Email: listaSedi[i].Email
                             , Fax: listaSedi[i].Fax
                             , Bollo: listaSedi[i].Bollo
                             , Telefono: listaSedi[i].Telefono
                             , Indirizzo: listaSedi[i].Indirizzo
                             , DatiBancari: listaSedi[i].DatiBancari
                             , ModalitaPagamento: listaSedi[i].ModalitaPagamento
                             , IdSede: listaSedi[i].IdSede
                             , IdModalitaPagamento: listaSedi[i].IdModalitaPagamento
                             , NomeSede: listaSedi[i].NomeSede
                             , HasDatiBancari: listaSedi[i].HasDatiBancari
                             , ContoCorrente: datiBancari[j].ContoCorrente
                             , Iban: datiBancari[j].Iban
                             , IdContoCorrente: datiBancari[j].IdContoCorrente
                             , NomeBanca: datiBancari[j].NomeBanca
                             , DisplayRow: (!isNullOrEmpty(datiBancari[j].Iban) ? datiBancari[j].Iban : datiBancari[j].ContoCorrente) + ' - ' + listaSedi[i].NomeSede + ': ' + listaSedi[i].Indirizzo + ' - ' + (!isNullOrEmpty(listaSedi[i].CapComune) ? listaSedi[i].CapComune + ' ' : '') + listaSedi[i].Comune
                             , ValueConto: (!isNullOrEmpty(datiBancari[j].Iban) ? datiBancari[j].Iban : datiBancari[j].ContoCorrente)
    	            });
    	            store.insert(index++, record);
    	        }
            }
        }
    }

    return store;
}

function hasBeneficiarioConti(beneficiario) {
    var retValue = false;
    
    var listaSedi = beneficiario.ListaSedi;
    
    if(listaSedi!=null && listaSedi!=undefined) {
	    for (var i = 0; i<listaSedi.length && !retValue; i++) {
	        if (listaSedi[i].DatiBancari != null && listaSedi[i].DatiBancari != undefined && listaSedi[i].DatiBancari.length != 0) {
	            retValue = true;
            }
        }
    }
    
    return retValue;
}


function getContiStore(datiBancari, includeType, istitutoRiferimento) {
    var store = new Ext.data.Store();

    if (datiBancari != null) {
        var TipoRecord = Ext.data.Record.create([
    	     { name: 'Abi' }
            , { name: 'Cab' }
            , { name: 'ContoCorrente' }
            , { name: 'Cin' }
            , { name: 'Iban' }
            , { name: 'IdAgenzia' }
            , { name: 'IdContoCorrente' }
            , { name: 'NomeBanca' }
            , { name: 'IndirizzoAgenzia' }
            , { name: 'ModalitaPrincipale' }
            , { name: 'SedeAgenzia' }
            , { name: 'DisplayRow' }
            , { name: 'ListRow' }
        ]);

    	var includeRow = false;

    	for (var i = 0; i < datiBancari.length; i++) {
    	    if (includeType == 'iban')
    	        includeRow = !isNullOrEmpty(datiBancari[i].Iban);
    	    else if (includeType == 'cc')
    	        includeRow = !isNullOrEmpty(datiBancari[i].ContoCorrente) && isNullOrEmpty(datiBancari[i].Iban);
    	    else if (includeType == 'all')
    	        includeRow = true;
    	    else
    	        break;

    	    if (includeRow) {
	            if(isNullOrEmpty(istitutoRiferimento) || (!isNullOrEmpty(datiBancari[i].NomeBanca) && datiBancari[i].NomeBanca==istitutoRiferimento)) {	        
	                var record = new TipoRecord({
	                    Abi: datiBancari[i].Abi
                        , Cab: datiBancari[i].Cab
                        , ContoCorrente: datiBancari[i].ContoCorrente
                        , Cin: datiBancari[i].Cin
                        , Iban: datiBancari[i].Iban
                        , IdAgenzia: datiBancari[i].IdAgenzia
                        , IdContoCorrente: datiBancari[i].IdContoCorrente
                        , NomeBanca: datiBancari[i].NomeBanca
                        , IndirizzoAgenzia: datiBancari[i].IndirizzoAgenzia
                        , ModalitaPrincipale: datiBancari[i].ModalitaPrincipale
                        , SedeAgenzia: datiBancari[i].SedeAgenzia
                        , DisplayRow: (!isNullOrEmpty(datiBancari[i].Iban) ? datiBancari[i].Iban : datiBancari[i].ContoCorrente) + (!isNullOrEmpty(datiBancari[i].NomeBanca) ? " - " + datiBancari[i].NomeBanca : "") + (datiBancari[i].ModalitaPrincipale ? " (Modalità Principale)" : "")
                        , ListRow: (!isNullOrEmpty(datiBancari[i].Iban) ? datiBancari[i].Iban : datiBancari[i].ContoCorrente) + (!isNullOrEmpty(datiBancari[i].NomeBanca) ? " - " + datiBancari[i].NomeBanca : "") + (datiBancari[i].ModalitaPrincipale ? " <b>(Modalità Principale)</b>" : "")
	                });
	                store.insert(i, record);
                }
    	    }
        }
    }

    return store;
}

function getModalitaPagamentoStore(listaSedi, onLoadFn) {
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
        autoLoad:true
    });

    store.setDefaultSort("OrdineApparizione", "ASC");

    store.on({
        'load': {
            fn: function(store, records, options) {
                if(onLoadFn!=null && onLoadFn!=undefined)
                    onLoadFn(listaSedi);
            },
            scope: this
        }
    });


    return store;
}

function getModalitaPagamentoAllContoStore(includeType, istitutoRiferimento) {
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
            fn: function(store, records, options) {
                store.filterBy(function filter(record) {
                    var retValue = false;
                
                    if (includeType == null || includeType == undefined || includeType == 'all')
                        retValue = record.data.ObbligoCC == true || record.data.ObbligoIBAN == true;
                    else if (includeType == 'iban')
                        retValue = record.data.ObbligoIBAN == true;
                    else if (includeType == 'cc')
                        retValue = record.data.ObbligoIBAN == false && record.data.ObbligoCC == true;

                    retValue &= (isNullOrEmpty(istitutoRiferimento) || (!isNullOrEmpty(record.data.IstitutoRiferimento) && record.data.IstitutoRiferimento == istitutoRiferimento));
                                                                
                    return retValue;
                });
            },
            scope: this
        }
    });


    return store;
}

function resetContoHiddenField() {
    if (Ext.getCmp("conto") != null)
        Ext.getCmp("conto").setValue("");
}

function resetSedeHiddenField() {
    if (Ext.getCmp("sede") != null)
        Ext.getCmp("sede").setValue("");
}

function storeSedeDataIntoSedeHiddenField(sede, modalitaPagamento) {
    if (sede != null && sede != undefined) {
        Ext.get("idSede").value = sede.IdSede;

        if (Ext.getCmp("sede") != null) {
            var TipoRecord = Ext.data.Record.create([
    			                         { name: 'Comune' }
                                       , { name: 'CapComune' }
                                       , { name: 'Email' }
                                       , { name: 'Fax' }
                                       , { name: 'Telefono' }
                                       , { name: 'Bollo' }
                                       , { name: 'Indirizzo' }
                                       , { name: 'ModalitaPagamento' }
                                       , { name: 'DatiBancari' }
                                       , { name: 'IdSede' }
                                       , { name: 'IdModalitaPagamento' }
                                       , { name: 'NomeSede' },
                                       , { name: 'HasDatiBancari' }
			                        ]);

            var sedeValue = new TipoRecord({
                    Comune: sede.Comune
                     , CapComune: sede.CapComune
                     , Email: sede.Email
                     , Fax: sede.Fax
                     , Bollo: sede.Bollo
                     , Telefono: sede.Telefono
                     , Indirizzo: sede.Indirizzo
                     , DatiBancari: sede.DatiBancari
                     , ModalitaPagamento: (modalitaPagamento != null && modalitaPagamento!=undefined) ? modalitaPagamento.Descrizione : sede.ModalitaPagamento
                     , IdSede: sede.IdSede
                     , IdModalitaPagamento: (modalitaPagamento != null && modalitaPagamento!=undefined) ? modalitaPagamento.Id : sede.IdModalitaPagamento
                     , NomeSede: sede.NomeSede
                     , HasDatiBancari: (modalitaPagamento != null && modalitaPagamento!=undefined) ? (modalitaPagamento.ObbligoIBAN || modalitaPagamento.ObbligoCC) : sede.HasDatiBancari
            });

            Ext.getCmp("sede").setValue(Ext.util.JSON.encode(sedeValue.data));
        }
    }
}

function storeContoDataIntoContoHiddenField(conto) {
    if(conto!=null && conto!=undefined) {
        Ext.get("idConto").value = conto.IdContoCorrente;
                        
        if (Ext.getCmp("conto") != null) {
            var TipoRecord = Ext.data.Record.create([
                  { name: 'Abi' }
                , { name: 'Cab' }
                , { name: 'ContoCorrente' }
                , { name: 'Cin' }
                , { name: 'Iban' }
                , { name: 'IdAgenzia' }
                , { name: 'IdContoCorrente' }
                , { name: 'NomeBanca' }
                , { name: 'IndirizzoAgenzia' }
                , { name: 'ModalitaPrincipale' }
                , { name: 'SedeAgenzia' }
            ]);

            var contoValue = new TipoRecord({
                      Abi: conto.Abi
                    , Cab: conto.Cab
                    , ContoCorrente: conto.ContoCorrente
                    , Cin: conto.Cin
                    , Iban: conto.Iban
                    , IdAgenzia: conto.IdAgenzia
                    , IdContoCorrente: conto.IdContoCorrente
                    , NomeBanca: conto.NomeBanca
                    , IndirizzoAgenzia: conto.IndirizzoAgenzia
                    , ModalitaPrincipale: conto.ModalitaPrincipale
                    , SedeAgenzia: conto.SedeAgenzia
            });

            Ext.getCmp('conto').setValue(Ext.util.JSON.encode(contoValue.data));
        }
    }
}

function onSelectSedeAction(sede, idModalitaPagamento, idContoCorrente) {
    var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamentoId');

    Ext.getCmp('labelModalitaPagamentoId').show();
    comboModalitaPagamento.show();

    if(!isNullOrEmpty(idModalitaPagamento)) {
        comboModalitaPagamento.setValue(idModalitaPagamento);
        var modalitaPagamento = comboModalitaPagamento.findRecord(comboModalitaPagamento.valueField || comboModalitaPagamento.displayField, idModalitaPagamento);
        if (modalitaPagamento != undefined) {
            onSelectTipologiaPagamentoAction(sede, modalitaPagamento.data, idContoCorrente);
        }        
    }
}

function onSelectTipologiaPagamentoAction(sede, modalitaPagamento, idContoCorrente) {
    
    storeSedeDataIntoSedeHiddenField(sede, modalitaPagamento);
    resetContoHiddenField();
    
    var comboDatiBancari = Ext.getCmp('comboDatiBancariId');

    if (modalitaPagamento.ObbligoCC || modalitaPagamento.ObbligoIBAN) {
        if (modalitaPagamento.ObbligoIBAN) {
            Ext.getCmp('actionAggiungiCCBancarioId').show();
            Ext.getCmp('actionAggiungiCCId').hide();
        } else if (modalitaPagamento.ObbligoCC) {
            Ext.getCmp('actionAggiungiCCBancarioId').hide();
            Ext.getCmp('actionAggiungiCCId').show();
        }

        Ext.getCmp('labelDatiBancariId').show();
        comboDatiBancari.show();

        comboDatiBancari.clearValue();
        
        var contiStore = getContiStore(sede.DatiBancari, modalitaPagamento.ObbligoIBAN ? 'iban' : (modalitaPagamento.ObbligoCC ? 'cc' : 'all'), modalitaPagamento.IstitutoRiferimento);
        comboDatiBancari.bindStore(contiStore);

        if ((idContoCorrente==null || idContoCorrente==undefined || idContoCorrente=="") && contiStore.getCount() == 1) {
            var conto = contiStore.getAt(0).data;

            if ((modalitaPagamento.ObbligoIBAN && !isNullOrEmpty(conto.Iban)) ||
                (modalitaPagamento.ObbligoCC && !isNullOrEmpty(conto.ContoCorrente) && isNullOrEmpty(conto.Iban)))
                idContoCorrente = conto.IdContoCorrente;
        }

        if (!isNullOrEmpty(idContoCorrente)) {               
            comboDatiBancari.setValue(idContoCorrente);

            var contoSelezionato = comboDatiBancari.findRecord(comboDatiBancari.valueField || comboDatiBancari.displayField, idContoCorrente);
            comboDatiBancari.fireEvent('select', comboDatiBancari, contoSelezionato, comboDatiBancari.store.indexOf(contoSelezionato));
        } else {
            if (Ext.getCmp('datiLiquidazionePanel') != undefined && Ext.getCmp('datiLiquidazionePanel') != null)
                Ext.getCmp('datiLiquidazionePanel').hide();
                
            Ext.getCmp('btnSelezionaBen').disable();

            scrollBottom("panelBeneficiarioLiquidazioneImpegno");
        }
    } else {                            
        comboDatiBancari.hide();
        Ext.getCmp('labelDatiBancariId').hide();

        Ext.getCmp('actionAggiungiCCBancarioId').hide();
        Ext.getCmp('actionAggiungiCCId').hide();
                
        if (Ext.getCmp('datiLiquidazionePanel') == undefined || Ext.getCmp('datiLiquidazionePanel') == null) {

            var bilancio = undefined;
            var capitolo = undefined;
            if (Ext.getCmp('liquidazioneData').getValue() != undefined && Ext.getCmp('liquidazioneData').getValue() != "") {
                var liquidazione = Ext.util.JSON.decode(Ext.getCmp('liquidazioneData').getValue());
                bilancio = liquidazione.Bilancio;
                capitolo = liquidazione.Capitolo;
            } else if (Ext.getCmp('impegnoData').getValue() != undefined && Ext.getCmp('impegnoData').getValue() != "") {
                var impegno = Ext.util.JSON.decode(Ext.getCmp('impegnoData').getValue());
                bilancio = impegno.Bilancio;
                capitolo = impegno.Capitolo;
            }            

            Ext.getCmp('tab_datiBeneficiarioLiquidazione').add(buildPanelDatiLiquidazione(bilancio, capitolo));
            Ext.getCmp('tab_datiBeneficiarioLiquidazione').doLayout();
        } else
            Ext.getCmp('datiLiquidazionePanel').show();
            
        Ext.getCmp('btnSelezionaBen').enable();

        scrollBottom("panelBeneficiarioLiquidazioneImpegno");
    }
}

function buildSedeSelectionPanel(beneficiario) {
    var listaSedi = beneficiario.ListaSedi;

    var actionAggiungiSede = new Ext.Action({
        text: 'Nuova Sede',
        tooltip: 'Aggiungi una nuova sede',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.sede, insertModes.newObject,
            { idAnagrafica: Ext.get('idAnagrafica').value,
                updateFn: editInsertUpdateComboFn
            });
        }
    });

    var actionCercaConto = new Ext.Action({
        text: 'Cerca Dati Bancari',
        id: 'actionCercaConto',
        tooltip: 'Cerca una sede a partire da un Iban/Conto Corrente',
        handler: function() {
            var beneficiarioJSON = Ext.getCmp('beneficiario').getValue();
            buildSearchContoCorrente(Ext.util.JSON.decode(beneficiarioJSON));
        }
    });

    var actionAggiungiCCBancario = new Ext.Action({
        text: 'Nuovo IBAN',
        id: 'actionAggiungiCCBancarioId',        
        tooltip: 'Aggiungi un nuovo conto corrente bancario',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.datiBancari, insertModes.newObject,
 	            { idAnagrafica: Ext.get('idAnagrafica').value,
 	                idSede: Ext.get('idSede').value,
 	                updateFn: editInsertUpdateComboFn
 	            });
        }
    });

    var actionAggiungiCC = new Ext.Action({
        text: 'Nuovo C/C',
        id: 'actionAggiungiCCId',
        tooltip: 'Aggiungi un nuovo conto corrente',
        handler: function() {
            var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamentoId');
            
            var idModalitaPagamento = comboModalitaPagamento.getValue();
            var modalitaPagamento = comboModalitaPagamento.findRecord(comboModalitaPagamento.valueField || comboModalitaPagamento.displayField, idModalitaPagamento);
        
            InitFormInsertAnagrafica(objectTypes.datiContoCorrente, insertModes.newObject,
                { idAnagrafica: Ext.get('idAnagrafica').value,
                    idSede: Ext.get('idSede').value,
                    istitutoRiferimento: modalitaPagamento.data.IstitutoRiferimento,
                    updateFn: editInsertUpdateComboFn
                });
        }
    });

    var comboSedi = new Ext.form.ComboBox({
        id: 'comboSediId',
        store: getSediStore(listaSedi),
        displayField: 'DisplayRow',
        valueField: 'IdSede',
        readOnly: true,
        width: 515,
        listWidth: 515,
        queryMode: 'local',
        mode: 'local',
        emptyText: 'Selezionare una sede...',
        blankText: 'Campo Obbligatorio',
        allowBlank: false,
        triggerAction: 'all',
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><b>{NomeSede}</b><br/>{Indirizzo} - {CapComune} {Comune}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    onSelectSedeAction(value.data, value.data.IdModalitaPagamento);
                }
            }
        }
    });

    var comboModalitaPagamento = new Ext.form.ComboBox({
        displayField: 'Descrizione',
        valueField: 'Id',
        id: 'comboModalitaPagamentoId',
        listWidth: 515,
        store: getModalitaPagamentoStore(listaSedi, initDatiPagamentoAllFieldsDefaultValues),
        readOnly: true,
        mode: 'local',
        allowBlank: false,
        blankText: 'Campo Obbligatorio',
        queryMode: 'local',
        width: 515,
        triggerAction: 'all',
        emptyText: 'Selezionare una modalità di pagamento ...',
        listeners: {
            select: {
            fn: function(combo, value) {                    
                    var comboSede = Ext.getCmp('comboSediId');

                    var idSede = comboSede.getValue();
                    var sede = comboSede.findRecord(comboSede.valueField || comboSede.displayField, idSede);

                    var modalitaPagamento = value.data;
                    onSelectTipologiaPagamentoAction(sede.data, modalitaPagamento);
                }
            }
        }
    });

    var storeDatiBancari = new Ext.data.SimpleStore({
        fields: ['IdContoCorrente', 'DisplayRow'],
        data: [],
        autoLoad: true
    });

    var comboDatiBancari = new Ext.form.ComboBox({
        id: 'comboDatiBancariId',
        store: storeDatiBancari,
        displayField: 'DisplayRow',
        valueField: 'IdContoCorrente',
        readOnly: true,
        width: 515,
        listWidth: 515,
        queryMode: 'local',
        mode: 'local',
        emptyText: 'Selezionare un conto...',
        blankText: 'Campo Obbligatorio',
        allowBlank: false,
        triggerAction: 'all',
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;">{ListRow}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    storeContoDataIntoContoHiddenField(value.data);

                    if (Ext.getCmp('datiLiquidazionePanel') == undefined || Ext.getCmp('datiLiquidazionePanel') == null) {

                        var bilancio = undefined;
                        var capitolo = undefined;
                        if (Ext.getCmp('liquidazioneData').getValue() != undefined && Ext.getCmp('liquidazioneData').getValue() != "") {
                            var liquidazione = Ext.util.JSON.decode(Ext.getCmp('liquidazioneData').getValue());
                            bilancio = liquidazione.Bilancio;
                            capitolo = liquidazione.Capitolo;
                        } else if (Ext.getCmp('impegnoData').getValue() != undefined && Ext.getCmp('impegnoData').getValue() != "") {
                            var impegno = Ext.util.JSON.decode(Ext.getCmp('impegnoData').getValue());
                            bilancio = impegno.Bilancio;
                            capitolo = impegno.Capitolo;
                        }


                        Ext.getCmp('tab_datiBeneficiarioLiquidazione').add(buildPanelDatiLiquidazione(bilancio, capitolo));
                        Ext.getCmp('tab_datiBeneficiarioLiquidazione').doLayout();
                    }
                    else
                        Ext.getCmp('datiLiquidazionePanel').show();

                    Ext.getCmp('btnSelezionaBen').enable();

                    scrollBottom("panelBeneficiarioLiquidazioneImpegno");
                }
            }
        }
    });
      
    Ext.override(Ext.layout.TableLayout, {
	    tableCfg: {tag:'table', cls:'x-table-layout', cellspacing: 0, cn: {tag: 'tbody'}},
	    onLayout : function(ct, target){
		    var cs = ct.items.items, len = cs.length, c, i;
		    if(!this.table) {
			    target.addClass('x-table-layout-ct');
			    this.table = target.createChild(this.tableCfg, null, true);
			    this.renderAll(ct, target);
		    }
	    }
    });
 	       	 	      
    var panel = new Ext.Panel({
        border: false,
        autoHeight: true,
        layout: 'table',
        layoutConfig: {
            columns: 2,
            tableCfg: {tag:'table', cls:'x-table-layout', cellspacing: 2, cn: {tag: 'tbody'}}
        },        
        xtype: "panel",
        id: "sedeSelectionPanel",
        items: [                    
        {
            xtype: 'label',
            id: 'labelSedeId',
            text: 'Sede',
            style: 'padding-right:87px'
        },

        {
            layout: "column",
            width: 650,
            border: false,   
            anchor: "0",
            items: [{
                border: false,
                style: 'margin-right: 10px',
                items: [
					comboSedi
				]
            }, {
                border: false,              
                items: [
					new Ext.LinkButton(actionAggiungiSede),
					new Ext.LinkButton(actionCercaConto)
				]
            }]
            },         
        {
            xtype: 'label',
            id: 'labelModalitaPagamentoId',
            text: 'Modalità Pagamento'
        },
        {
            layout: "column",
            style: 'margin-top:-2px',         
            border: false,
            anchor: "0",
            items: [{
                border: false,
                style: 'margin-right: 10px',
                items: [
					comboModalitaPagamento
				]
            }]
            },                            
        {
            xtype: 'label',
            id: 'labelDatiBancariId',
            text: 'Dati Bancari'
        },              
        {
            layout: "column",
			border: false,
			anchor: "0",
			items: [{
			    border: false,			
				style: 'margin-right: 10px',
				items: [
					comboDatiBancari
				]
			},{
			    border: false,
				items: [ 
					new Ext.LinkButton(actionAggiungiCCBancario)
				]
			},{
			    border: false,
				items: [
					new Ext.LinkButton(actionAggiungiCC)
				]
			}]
		},               
        {
            xtype: 'hidden',
            id: 'beneficiario',
            value: Ext.util.JSON.encode(beneficiario)
        }
        ]
    });

    Ext.getCmp('labelModalitaPagamentoId').hide();
    comboModalitaPagamento.hide();
    
    Ext.getCmp('labelDatiBancariId').hide();
    comboDatiBancari.hide();

    if (Ext.getCmp('datiLiquidazionePanel') != undefined && Ext.getCmp('datiLiquidazionePanel') != null) {
        Ext.getCmp('datiLiquidazionePanel').hide();
    }
    
    Ext.getCmp('btnSelezionaBen').disable();

    actionAggiungiSede.enable();
    actionAggiungiCCBancario.hide();
    actionAggiungiCC.hide();

    if (hasBeneficiarioConti(beneficiario))
        actionCercaConto.enable();
    else
        actionCercaConto.disable();
    
    return panel;
}

function enableContoSearch(beneficiario) {
    if (hasBeneficiarioConti(beneficiario))
        Ext.getCmp('actionCercaConto').enable();
    else
        Ext.getCmp('actionCercaConto').disable();
}

function initDatiPagamentoAllFieldsDefaultValues(listaSedi) {
    var idAnagrafica = null;
    var idSede = null;
    var idTipoPagamento = null;
    var idContoCorrente = null;

    if(Ext.getCmp('idSearchModel').getValue()!=null && Ext.getCmp('idSearchModel').getValue()!=undefined && (Ext.getCmp('idSearchModel').getValue() == 'onAutoSearch' || Ext.getCmp('idSearchModel').getValue() == 'onAutoSilentSearch')) {
        if (!isNullOrEmpty(Ext.getCmp('idResultSede').getValue())) {
            idSede = Ext.getCmp('idResultSede').getValue();
            if (!isNullOrEmpty(Ext.getCmp('idResultTipoPagamento').getValue()))
                idTipoPagamento = Ext.getCmp('idResultTipoPagamento').getValue();
        }
        if (!isNullOrEmpty(Ext.getCmp('idResultConto').getValue()))
            idContoCorrente = Ext.getCmp('idResultConto').getValue();

        Ext.getCmp('idSearchModel').setValue('onSearch');            
    } else if (listaSedi.length == 1) {        
        idSede = listaSedi[0].IdSede;
        idTipoPagamento = listaSedi[0].IdModalitaPagamento;        
    }

    setDatiPagamentoAllFields(idAnagrafica, idSede, idTipoPagamento, idContoCorrente);
}

function setDatiPagamentoAllFields(idAnagrafica, idSede, idTipoPagamento, idContoCorrente) {            
    if (!isNullOrEmpty(idSede)) {
        var comboSedi = Ext.getCmp('comboSediId');
        comboSedi.setValue(idSede);
        
        var sede = comboSedi.findRecord(comboSedi.valueField || comboSedi.displayField, idSede);        
        onSelectSedeAction(sede.data, idTipoPagamento, idContoCorrente);
    } else
        scrollBottom("panelBeneficiarioLiquidazioneImpegno");    
}

function editInsertUpdateComboFn(objectType, insertMode, objectData, result) {   
    if(objectType == objectTypes.anagrafica)
        updateDatiPagamentoAllFields(result);
    else
        loadAnagrafica(objectData, objectType == objectTypes.sede ? updateDatiPagamentoSedeFields : updateDatiPagamentoContoFields, insertMode, result);
}

function updateDatiPagamentoAllFields(result) {
    preFillAllResultFields(false,
            result.IdAnagrafica,
            result.IdSede,
            result.IdTipoPagamento,
            result.IdContoCorrente);            
}

function updateDatiPagamentoSedeFields(objectData, beneficiario, insertMode, result) {    
    updateGridRicercaStore(beneficiario);
    
    var comboSedi = Ext.getCmp('comboSediId');
    comboSedi.bindStore(getSediStore(beneficiario.ListaSedi));

    preFillDettagliLiquidazioneDatiPagamentoFields(beneficiario,
        result.IdSede == undefined ? comboSedi.getValue() : result.IdSede,
        result.IdTipoPagamento == undefined ? Ext.getCmp('comboModalitaPagamentoId').getValue() : result.IdTipoPagamento,
        result.IdContoCorrente == undefined ? Ext.getCmp('comboDatiBancariId').getValue() : result.IdContoCorrente);

    enableContoSearch(beneficiario);
    Ext.getCmp('beneficiario').setValue(Ext.util.JSON.encode(beneficiario));    
}

function updateDatiPagamentoContoFields(objectData, beneficiario, insertMode, result) {
    updateDatiPagamentoSedeFields(objectData, beneficiario, insertMode, result);
}


function preFillDettagliLiquidazioneDatiPagamentoFields(beneficiario, idSede, idTipoPagamento, idContoCorrente) {
    var retValue = false;

    if (beneficiario != null && beneficiario != undefined && !isNullOrEmpty(beneficiario.ID)) {
        initIdResultFields('onAutoSearch', beneficiario.ID, idSede, idTipoPagamento, idContoCorrente);

        initDatiPagamentoAllFieldsDefaultValues(beneficiario.ListaSedi);
        retValue = true;
    }

    return retValue;
}

 function enableSearchContoCorrenteOKButton() {
    var comboAllContiValue = Ext.getCmp('comboAllContiId').getValue();
    var comboModalitaPagamentoValue = Ext.getCmp('comboModalitaPagamentoContoCorrenteId').getValue();
    
    if(!isNullOrEmpty(comboAllContiValue) && !isNullOrEmpty(comboModalitaPagamentoValue))
        Ext.getCmp('btnOKConto').enable();
    else
        Ext.getCmp('btnOKConto').disable();
}

function buildSearchContoCorrente(beneficiario) {
    var comboAllConti = new Ext.form.ComboBox({
        id: 'comboAllContiId',
        store: getAllContiStore(beneficiario.ListaSedi),
        displayField: 'DisplayRow',
        valueField: 'IdContoCorrente',
        hideTrigger: false,
        width: 600,
        listWidth: 600,
        queryMode: 'local',
        mode: 'local',
        emptyText: 'Selezionare una conto...',
        typeAhead: true,
        forceSelection: true,
        triggerAction: 'all',
        selectOnFocus: true,
        tpl: '<tpl for="."><div class="x-combo-list-item" style="overflow: visible; text-wrap: normal; text-overflow: normal; white-space: normal;"><b>{ValueConto}</b><br/>{NomeSede}: {Indirizzo} - {CapComune} {Comune}</div></tpl>',
        listeners: {
            select: {
                fn: function(combo, value) {
                    Ext.getCmp('comboModalitaPagamentoContoCorrenteId').enable();
                    Ext.getCmp('labelModalitaPagamentoContoCorrenteId').enable();

                    var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamentoContoCorrenteId');
                    comboModalitaPagamento.clearValue();
                    
                    var includeType = !isNullOrEmpty(value.data.Iban) ? 'iban' : 'cc';
                    comboModalitaPagamento.bindStore(getModalitaPagamentoAllContoStore(includeType, includeType=='cc' ? value.data.NomeBanca : null));

                    enableSearchContoCorrenteOKButton();
                }
            }
        }
    });
    
    var comboModalitaPagamento = new Ext.form.ComboBox({
        displayField: 'Descrizione',
        valueField: 'Id',
        id: 'comboModalitaPagamentoContoCorrenteId',
        listWidth: 600,
        store: getModalitaPagamentoAllContoStore('all'),
        readOnly: true,
        mode: 'local',       
        queryMode: 'local',
        width: 600,
        triggerAction: 'all',
        emptyText: 'Selezionare una modalità di pagamento...',
        lastQuery: '',
        listeners: {
            select: {
                fn: function(combo, value) {
                    enableSearchContoCorrenteOKButton();                    
                }
            }
        }
    });

    Ext.override(Ext.layout.TableLayout, {
        tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 0, cn: { tag: 'tbody'} },
        onLayout: function(ct, target) {
            var cs = ct.items.items, len = cs.length, c, i;
            if (!this.table) {
                target.addClass('x-table-layout-ct');
                this.table = target.createChild(this.tableCfg, null, true);
                this.renderAll(ct, target);
            }
        }
    });

    var panel = new Ext.Panel({
        bodyPadding: 10,
        border: true,
        layout: 'table',
        bodyStyle: Ext.isIE ? 'padding:15px 10px 10px 15px' : 'padding:15px 10px 10px 15px',
        layoutConfig: {
            columns: 2,
            tableCfg: { tag: 'table', cls: 'x-table-layout', cellspacing: 5, cn: { tag: 'tbody'} }
        },
        tableStyle: 'cellspacing:5px',   
        autoHeight: true,    
        xtype: "panel",
        id: "contoSelectionPanel",
        items: [
         {
             xtype: 'label',
             id: 'labelAllContiId',
             text: 'Iban/Conto Corrente',  
             style: 'padding-right:5px'           
         }, comboAllConti,
         {
             xtype: 'label',
             id: 'labelModalitaPagamentoContoCorrenteId',
             text: 'Modalità Pagamento'
         },
         comboModalitaPagamento
        ]
    });

    var popupAna = new Ext.Window({
        title: 'Cerca Iban/Conto Corrente',
        width: 800,
        id: 'searchContoCorrenteWindow',
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        closable: true,
        resizable: false,
        items: [
            panel
        ],
        buttons: [{
            text: 'Cancella',
            id: 'btnCancelConto',
            handler: function() {
                Ext.getCmp('searchContoCorrenteWindow').close();
            }
        }, {
            text: 'OK',
            id: 'btnOKConto',
            handler: function() {
                var comboAllConti = Ext.getCmp('comboAllContiId');
                var contoSelezionato = comboAllConti.findRecord(comboAllConti.valueField || comboAllConti.displayField, comboAllConti.getValue());

                var comboModalitaPagamento = Ext.getCmp('comboModalitaPagamentoContoCorrenteId');
                var modalitaSelezionata = comboModalitaPagamento.findRecord(comboModalitaPagamento.valueField || comboModalitaPagamento.displayField, comboModalitaPagamento.getValue());

                preFillDettagliLiquidazioneDatiPagamentoFields(beneficiario, contoSelezionato.data.IdSede, modalitaSelezionata.data.Id, contoSelezionato.data.IdContoCorrente);
                Ext.getCmp('searchContoCorrenteWindow').close();
            }
        }]
    });

    Ext.getCmp('labelModalitaPagamentoContoCorrenteId').disable();
    Ext.getCmp('comboModalitaPagamentoContoCorrenteId').disable();
    
    enableSearchContoCorrenteOKButton();
     
    popupAna.doLayout();
    popupAna.show();
}

function isNullOrEmpty(value) { 
    return (value==null || value==undefined || value=="")
}

function scrollBottom(panelName) {
    var panelGestioneAnagraficaDom = Ext.getCmp(panelName).body.dom;
    var scrollValue = (panelGestioneAnagraficaDom.scrollHeight - panelGestioneAnagraficaDom.offsetHeight);

    Ext.getCmp(panelName).body.scrollTo('top', scrollValue, true);
}

function getAttributoDocumento(attribCode, callBackFn) {
    if (attribCode != null && attribCode!=undefined && attribCode != '') {
        var params = { codAttributo: attribCode };

        Ext.Ajax.request({
            url: 'ProcAmm.svc/GetAttributoDocumento' + window.location.search,
            headers: { 'Content-Type': 'application/json' },
            params: params,
            method: 'GET',
            success: function(response, result) {
                if (response != null && response != undefined && response.responseText != null && response.responseText != undefined) {
                    var responseObject = Ext.decode(response.responseText);
                    if (responseObject != null && responseObject != undefined && responseObject.GetAttributoDocumentoResult != null && responseObject.GetAttributoDocumentoResult != undefined)
                        callBackFn(responseObject.GetAttributoDocumentoResult.Valore);
                }
            },
            failure: function(response, options) {
            }
        });
    }
}

function setActivePanel(panelName, panelNameToActive) {
    Ext.getCmp(panelName).active = Ext.getCmp(panelNameToActive);
    Ext.getCmp(panelName).active.show();
    Ext.getCmp(panelName).doLayout();
}
