
function fillDatiLiquidazioneFields(liquidazione) {
    Ext.getCmp("hidLiquidazione").setValue(liquidazione.IDLiquidazione);
    Ext.getCmp("IdDocumento").setValue(liquidazione.IdDocumento);
    Ext.getCmp("ID").setValue(liquidazione.ID);
    Ext.getCmp("CodiceCup").setValue(liquidazione.Cup);
    Ext.getCmp("CodiceCig").setValue(liquidazione.Cig);
    Ext.getCmp("ImpSpettante").setValue(liquidazione.ImportoSpettante);   
}

function editDatiLiquidazione(liquidazione, currentGridPanelId) {
    var popup = new Ext.Window({
        title: 'Modifica Dati Liquidazione',
        width: 372,       
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding-left:10px;padding-top:10px;padding-right:10px',
        resizable: false,
        buttonAlign: 'center',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        autoScroll: true,
        closable: true
    });

    var panelLiquidazioneBen = new Ext.FormPanel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: Ext.isIE ? 'padding:15px 10px 15px 15px;' : 'padding:15px 10px 15px 15px;',
        border: true,        
        layoutConfig: {
            columns: 2
        },
        items: [
            {
                xtype: 'label',
                text: 'Codice Cup  '
            }, {
                xtype: 'textfield',
                fieldLabel: 'Codice Cup',
                name: 'CodiceCup',
                id: 'CodiceCup',
                width: 200,
                minLength: 15,
                minLengthText: 'La lunghezza è di 15 caratteri',                
                maxLength: 15,
                maxLengthText: 'La lunghezza è di 15 caratteri'
            }, {
                xtype: 'label',
                text: 'Codice Cig  '
            }, {
                 xtype: 'textfield',
                 fieldLabel: 'Codice Cig',
                 name: 'CodiceCig',
                 id: 'CodiceCig',
                 width: 200,
                 minLength: 10,
                 minLengthText: 'La lunghezza è di 10 caratteri',
                 maxLength: 10,
                 maxLengthText: 'La lunghezza è di 10 caratteri'
             }, {
                 xtype: 'label',
                 text: 'Importo Spettante  ',
                 style: 'padding-right:15px'
             }, {
                 xtype: 'numberfield',
                 name: 'ImpSpettante',
                 id: 'ImpSpettante',
                 decimalSeparator: ',',                
                 allowNegative: false,
                 width: 200,
                 allowBlank: false,
                 blankText: 'Campo Obbligatorio'
             },
             {
                 xtype: 'hidden',
                 id: 'hidLiquidazione'
             },
             {
                 xtype: 'hidden',
                 id: 'hidImpegno'
             },
             {
                 xtype: 'hidden',
                 id: 'IdDocumento'
             },                 
             {
                 xtype: 'hidden',
                 id: 'ID'
             }
        ],
        buttonAlign: 'right',
        buttons: [{
            text: 'Salva',
            id: 'btnSalvaLiquidazione'
        }]
    });

    popup.add(panelLiquidazioneBen);
    popup.doLayout();
    popup.show();

    fillDatiLiquidazioneFields(liquidazione);

    //Definisco l'azione del bottone seleziona beneficiario
    Ext.getCmp('btnSalvaLiquidazione').on('click', function() {
        if (panelLiquidazioneBen.getForm().isValid()) {
            Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
            panelLiquidazioneBen.getForm().timeout = 100000000;
            panelLiquidazioneBen.getForm().submit({
                url: 'ProcAmm.svc/Registra_BeneficiarioLiquidazioneImpegno' + window.location.search,
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
                                title: 'Modifica Dati Liquidazione',
                                msg: lstr_messaggio,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.ERROR,
                                fn: function(btn) {
                                    popup.close();
                                    return;
                                }
                            });
                        }, // FINE FAILURE
                success:
					    function(result, response) {
					        var msg = 'Operazione effettuata con successo!';
					        Ext.MessageBox.show({
					            title: 'Modifica Dati Liquidazione',
					            msg: msg,
					            buttons: Ext.MessageBox.OK,
					            icon: Ext.MessageBox.INFO,
					            fn: function(btn) {
					                var liquidazione = Ext.get("hidDatiLiquidazione").value;
					                
					                liquidazione.Cup = Ext.getCmp("CodiceCup").getValue();
					                liquidazione.Cig = Ext.getCmp("CodiceCig").getValue();
					                liquidazione.ImportoSpettante = Ext.getCmp("ImpSpettante").getValue();
					                
					                fillViewLiquidazione(liquidazione);

					                popup.close();
					                return;
					            }
					        });
					    } // FINE SUCCESS
            }) // FINE SUBMIT
        } else {
            Ext.MessageBox.alert('Errore di validazione', 'Verificare che tutti i campi contengano valori validi.');
        }
    });     
}

 
function initFormAssociaBeneficiarioLiquidazioneImpegno(liquidazione, impegno, gridPanelId, gridPanelIdSuper, fatturePresentiSuLiq) {

    var textButton = 'Aggiungi beneficiario';
    if (liquidazione != undefined) {
        textButton = textButton + " alla liquidazione";
    } else if (impegno != undefined) {
        textButton = textButton + " all\'impegno";
    }
    var currentGridPanelId = gridPanelId;
                         	
    var popup = new Ext.Window({
        title: 'Beneficiario',
        width: 850,
        height: 700,
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        resizable: false,
        //buttonAlign: 'center',
        maximizable: false,					            
        enableDragDrop   : true,
        collapsible: false,
        modal:true,
        autoScroll:true,
        closable: true,
        buttons: [{
            text: textButton,
            id: 'btnSelezionaBen'
        },
         {
            text: 'Chiudi',
            id: 'btnChiudiSelezione'
        }]        
    });

    popup.on('close', function() {
        Ext.getCmp(gridPanelId).getStore().reload();
        Ext.getCmp(gridPanelIdSuper).getStore().reload();        
    });

    var panelBeneficiarioLiquidazioneImpegno = buildPanelBeneficiarioLiquidazioneImpegno(liquidazione, impegno, fatturePresentiSuLiq);
    popup.add(panelBeneficiarioLiquidazioneImpegno);
     
    popup.doLayout();
    popup.show();

    if (liquidazione != undefined) {
        Ext.getCmp('hidLiquidazione').setValue(liquidazione.ID);
        Ext.getCmp('IdDocumento').setValue(liquidazione.IdDocumento);
    } else if (impegno != undefined) {
        Ext.getCmp('hidImpegno').setValue(impegno.ID);
        Ext.getCmp('IdDocumento').setValue(impegno.IdDocumento);
        Ext.getCmp("importoSpettanteBeneficiario").setValue(impegno.ImpPrenotato);
    }
    
    
    Ext.getCmp('btnChiudiSelezione').on('click', function() {
        popup.close();
    });
    
    Ext.getCmp('btnSelezionaBen').disable();

    Ext.getCmp('btnSelezionaBen').on('click', function() {
        if (panelBeneficiarioLiquidazioneImpegno.getForm().isValid()) {

            if (!validatePanelDatiLiquidazioneImpegnoFields()) {
                Ext.MessageBox.show({
                    title: 'Errore di validazione',
                    msg: 'Verificare che tutti i campi contengano valori validi.',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                    }
                });
            } else if (!checkDatiSensibiliBeneficiario()) {
                Ext.MessageBox.show({
                    title: 'Errore di validazione',
                    msg: "E' necessario specificare se i dati relativi al beneficiario sono o meno sensibili.",
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                    }
                });

            } else if (!checkContrattoBeneficiario() &&
                       (Ext.getCmp('ID').getValue() == null || Ext.getCmp('ID').getValue() == undefined ||
                        Ext.getCmp('ID').getValue().trim() == '' || Ext.getCmp('ID').getValue().trim() == '0')) {
                var warning_message = "Nessun contratto di quelli indicati nella 'Scheda Trasparenza' è stato associato al beneficiario. Continuare ? ";
                warning_message += "(Premendo 'Sì' nessun contratto sarà associato al beneficiario, premendo 'No' sarà possibile effettuare l'associazione)";

                Ext.MessageBox.buttonText.yes = "Sì";

                Ext.MessageBox.show({
                    title: 'Attenzione',
                    msg: warning_message,
                    buttons: Ext.MessageBox.YESNO,
                    icon: Ext.MessageBox.WARNING,
                    fn: function(btn) {
                        if (btn == 'yes')
                            selezionaBenCallBack(panelBeneficiarioLiquidazioneImpegno, currentGridPanelId, liquidazione, impegno);
                    }
                });
            } else
                selezionaBenCallBack(panelBeneficiarioLiquidazioneImpegno, currentGridPanelId, liquidazione, impegno);
        } else {
            Ext.MessageBox.alert('Errore di validazione', 'Verificare che tutti i campi contengano valori validi.');
        }
    });
}

function selezionaBenCallBack(panelBeneficiarioLiquidazioneImpegno, currentGridPanelId, liquidazione, impegno) {
    var onclickCurrentGridPanelId = currentGridPanelId;
    Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
    panelBeneficiarioLiquidazioneImpegno.getForm().timeout = 100000000;
    panelBeneficiarioLiquidazioneImpegno.getForm().submit({
        url: 'ProcAmm.svc/Registra_BeneficiarioLiquidazioneImpegno' + window.location.search,
        waitTitle: "Attendere...",
        waitMsg: 'Aggiornamento in corso ......',
        failure:
            function(result, response) {
                var failureCurrentGridPanelId = onclickCurrentGridPanelId;

                var lstr_messaggio;
                var error_code;
                try {
                    lstr_messaggio = response.result.FaultMessage;
                    error_code = response.result.errorCode;
                } catch (ex) {
                    lstr_messaggio = 'Errore Generale'
                    error_code = 1
                }

                //beneficiario già esistente nella liquidazione
                if (error_code == 5) {
                    var warning_message = lstr_messaggio + " Aggiornare le informazioni con i nuovi valori ?";

                    Ext.MessageBox.buttonText.yes = "Sì";

                    Ext.MessageBox.show({
                        title: 'Aggiunta Beneficiario',
                        msg: warning_message,
                        buttons: Ext.MessageBox.YESNO,
                        icon: Ext.MessageBox.WARNING,
                        fn: function(btn) {
                            if (btn == 'yes') {
                                Ext.getCmp('ID').setValue(response.result.additionalInfo.beneficiario.ID);
                                Ext.getCmp('btnSelezionaBen').fireEvent('click');
                            } else {
                                Ext.MessageBox.show({
                                    title: 'Aggiunta Beneficiario',
                                    msg: 'Operazione annullata.',
                                    buttons: Ext.MessageBox.OK,
                                    icon: Ext.MessageBox.INFO,
                                    fn: function(btn) {
                                        return;
                                    }
                                });
                            }
                        }
                    });
                } else {
                    Ext.MessageBox.show({
                        title: 'Aggiunta Beneficiario',
                        msg: lstr_messaggio,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) {
                            return;
                        }
                    });
                }
            },
        success:
            function(result, response) {
                Ext.getCmp('ID').setValue(0);

                var msg = 'Beneficiario aggiunto';
                var successCurrentGridPanelId = onclickCurrentGridPanelId;
                Ext.MessageBox.show({
                    title: 'Aggiunta Beneficiario',
                    msg: msg,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.INFO,
                    fn: function(btn) {
                        var anagraficaValue = Ext.getCmp('anagrafica').getValue();
                        var sedeValue = Ext.getCmp('sede').getValue();
                        var contoValue = Ext.getCmp('conto').getValue();

                        var anagrafica = anagraficaValue != undefined && anagraficaValue != null && anagraficaValue != "" ?
                                Ext.util.JSON.decode(anagraficaValue) : null;
                        var sede = sedeValue != undefined && sedeValue != null && sedeValue != "" ?
                                Ext.util.JSON.decode(sedeValue) : null;
                        var conto = contoValue != undefined && contoValue != null && contoValue != "" ?
                                Ext.util.JSON.decode(contoValue) : null;

                        //aggiungi benef all'imp o alla liq
                        if (liquidazione != undefined) {

                        } else if (impegno != undefined) {
                            impegno.ListaBeneficiari = [];
                            impegno.ListaBeneficiari.push(anagraficaValue);                            
                            
                        }

                        generaBeneficiarioCronologia(anagrafica, sede, conto);
                        Ext.getCmp('btnChiudiSelezione').fireEvent('click');
                        return;
                    }
                }
                );
            }
    })
}

function openContratto(idContratto) {
    var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
    var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();

    window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + idContratto, '_blank');
}

function fatturePresentiLiq(liquidazione) {
    var esiste = false;
    var stdec = Ext.lib.Ajax.defaultPostHeader;
    Ext.lib.Ajax.defaultPostHeader = 'application/json';
    if (liquidazione != undefined) {
        Ext.Ajax.timeout = 100000000;
        var box = Ext.Ajax.request({
            async: false,
            url: 'ProcAmm.svc/GetListaFattureByLiquidazione' + window.location.search + '&idLiquidazione=' + liquidazione.ID,
            method: 'GET',
            success: function(response, options) {
                var data = Ext.decode(response.responseText);
                esiste = true ;
            },
            failure: function(response, options) {
                esiste = false ;
            }
        });
    }

    Ext.lib.Ajax.defaultPostHeader = stdec
    return esiste;
}

function BeneficiariLiquidazioneImpegno(liquidazione, impegno, gridPanelId, gridPanelIdSuper) {    
    var IDDocumentoContabile_value;
    var urlProxy;
    var result;
    var NDocumentoContabile;
    var IDDocumentoContabile;
    var titoloGrid = "Beneficiari";
    var isImpegno = false;
    if (liquidazione != undefined) {        
        IDDocumentoContabile_value = liquidazione.ID;
        urlProxy = 'ProcAmm.svc/GetlistaBeneficiariLiquidazioni';
        result = 'GetlistaBeneficiariLiquidazioniResult';
        NDocumentoContabile = 'NLiquidazione';
        IDDocumentoContabile = 'IDLiquidazione';
        titoloGrid = titoloGrid + " Liquidazione";
        isImpegno = false;
    } else if (impegno != undefined) {        
        IDDocumentoContabile_value = impegno.ID;
        urlProxy = 'ProcAmm.svc/GetlistaBeneficiariImpegno';
        result = 'GetlistaBeneficiariImpegnoResult';
        NDocumentoContabile = 'NImpegno';
        IDDocumentoContabile = 'IDImpegno';
        titoloGrid = titoloGrid + " Impegno";
        isImpegno = true;
    }

    var params = { ID: IDDocumentoContabile_value };
    
    var proxy = new Ext.data.HttpProxy({
        url: urlProxy,
        method:'GET',
        timeout: 900000
    });

    var reader = new Ext.data.JsonReader({
        root: result,
        fields: [
           { name: 'CodiceFiscale' },
           { name: 'Denominazione' },
           { name: 'ImportoSpettante' },
           { name: 'ID' },
           { name: NDocumentoContabile },
           { name: 'IdAnagrafica' },
           { name: IDDocumentoContabile },
           { name: 'IdDocumento' },
           { name: 'SedeVia' },
           { name: 'SedeComune' },
           { name: 'SedeProvincia' },
           { name: 'Cig' },
           { name: 'Cup' },
           { name: 'CodiceSiope' },
           { name: 'Iban' },
           { name: 'PartitaIva' },
           { name: 'FlagPersonaFisica' },
           { name: 'IdModalitaPag' },
           { name: 'DescrizioneModalitaPag' },
           { name: 'HasDatiBancari' },
           { name: 'IdConto' },
           { name: 'IdSede' },
           { name: 'IdContratto' },
           { name: 'NumeroRepertorioContratto' },
           { name: 'IsDatoSensibile' }
           ]
       });
    
    

    //DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
       var actionAddBeneficiario = new Ext.Action({
           text: 'Aggiungi',
           tooltip: 'Aggiungi un beneficiario',
           handler: function() {               
               if (liquidazione != undefined) {
                   var totaleImportoBeneficiari = 0;
                   var storeGridBen = Ext.getCmp(gridPanelId);
                   Ext.each(storeGridBen.getStore().data.items, function(rec) {
                       totaleImportoBeneficiari = totaleImportoBeneficiari + rec.data.ImportoSpettante;
                   });
                   if (liquidazione.ImpPrenotatoLiq != undefined && liquidazione.ImpPrenotatoLiq <= totaleImportoBeneficiari) {
                       Ext.MessageBox.show({
                           title: "Attenzione",
                           msg: "Non è possibile aggiungere ulteriori beneficiari alla liquidazione selezionata. Il totale supererebbe l'importo della liquidazione.",
                           buttons: Ext.MessageBox.OK,
                           icon: Ext.MessageBox.WARNING,
                           fn: function(btn) { return; }
                       });
                   } else {
                       var stdec = Ext.lib.Ajax.defaultPostHeader;
                       Ext.lib.Ajax.defaultPostHeader = 'application/json';

                       Ext.Ajax.timeout = 100000000;
                       var box = Ext.Ajax.request({
                           async: false,
                           url: 'ProcAmm.svc/GetListaFattureByLiquidazione' + window.location.search + '&idLiquidazione=' + liquidazione.ID,
                           method: 'GET',
                           success: function (response, options) {
                               var fatturePresentiSuLiq = false;
                               var data = Ext.decode(response.responseText);

                           if (data.GetListaFattureByLiquidazioneResult != '') {
                               if (data.GetListaFattureByLiquidazioneResult.length > 0) {
                                   fatturePresentiSuLiq = true;
                               } else {
                                    fatturePresentiSuLiq = false ;
                               }
                           }

                               initFormAssociaBeneficiarioLiquidazioneImpegno(liquidazione, undefined, gridPanelId, gridPanelIdSuper,fatturePresentiSuLiq);
                       },
                       failure: function(response, options) {

                           }
                       });
                       Ext.lib.Ajax.defaultPostHeader = stdec;
                   }
               } else {
                   initFormAssociaBeneficiarioLiquidazioneImpegno(undefined, impegno, gridPanelId, gridPanelIdSuper, false);
               }
           },
           iconCls: 'add'
       });

    //DEFINISCO L'AZIONE CANCELLA DELLA GRIGLIA
    var actionDeleteBeneficiario = new Ext.Action({
        text: 'Cancella',
        tooltip: 'Cancella selezionato',
        handler: function() {
            var storeGridBen = Ext.getCmp(gridPanelId);
            Ext.each(storeGridBen.getSelectionModel().getSelections(),
                function(rec) {
                    //var params = { IDDocContabile: rec.data[IDDocumentoContabile], IDDocumento: rec.data['IdDocumento'], ID: rec.data['ID'], IsImpegno: isImpegno};
                   
                    var params = { IDDocContabile: IDDocumentoContabile_value, IDDocumento: rec.data['IdDocumento'], ID: rec.data['ID'], IsImpegno: isImpegno };
                    
                    Ext.Ajax.request({
                        url: 'ProcAmm.svc/EliminaBeneficiarioLiquidazioneImpegno',
                        params: Ext.encode(params),
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        success: function(response, options) {
                            var retValue = false;
                            
                            if (retValue = Ext.decode(response.responseText)) {
                                Ext.getCmp(gridPanelId).getStore().reload();
                            }

                            return retValue;                            
                        },
                        failure: function(response, options) {
                            var data = Ext.decode(response.responseText);

                            Ext.MessageBox.show({
                                title: 'Errore',
                                msg: data.FaultMessage,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.ERROR,
                                fn: function(btn) { return }
                            });
                        }
                    });
                }
              )
        },
        iconCls: 'remove'
    });

    //DEFINISCO L'AZIONE DETTAGLIO BENEFICIARIO
    var actionDetailBeneficiario = new Ext.Action({
        text: 'Dettaglio',
        tooltip: 'Dettaglio del beneficiario selezionato',
        handler: function() {
            var storeGridBen = Ext.getCmp(gridPanelId);
            Ext.each(storeGridBen.getSelectionModel().getSelections(),
                    function(rec) {
                        //per caricare i dati dal SIC usare la prima istruzione, dal DB la seconda
                        //loadAndShowDetailBeneficiario(rec.data, gridPanelId, false);
                        showDetailBeneficiario(rec.data, gridPanelId, false);
                    }
               )
        },
        iconCls: 'read'
    });
        
	var store = new Ext.data.GroupingStore({
		proxy:proxy,
		reader:reader,
		groupField: NDocumentoContabile,
		sortInfo: {
		    field: NDocumentoContabile,
            direction: "ASC"
        }, 
        listeners: {
            'beforeload': function (store, operation, eOpts) {
                var isUffProp = Ext.get('isUffProp').dom.value;

                if (isUffProp != undefined && isUffProp == 0) {
                    actionDeleteBeneficiario.hide();
                    actionAddBeneficiario.hide();
                } else {
                    actionDeleteBeneficiario.setDisabled(true);
                    actionDetailBeneficiario.setDisabled(true);
                }
                 
            }
        }
	});


	
	store.on({
	    'load': {
	        fn: function (store, records, options) {
	            if (impegno != undefined) {
	                impegno.ListaBeneficiari
	            }
	            if (liquidazione != undefined) {
	                liquidazione.ListaBeneficiari
	            }

	        },
            scope:this }
 	        }); 

    store.load({params:params});
        
    var sm = new Ext.grid.CheckboxSelectionModel(
 	    {singleSelect: true,
 	    listeners: {
            rowselect: function(sm, row, rec) {
                 actionDeleteBeneficiario.setDisabled(false); 
                 actionDetailBeneficiario.setDisabled(false); 
             },
             rowdeselect :function(sm, row, rec) {
                actionDeleteBeneficiario.setDisabled(true); 
                actionDetailBeneficiario.setDisabled(true); 
            }
 	     }
 	 });

    function renderSwitchCodFiscPartitaIVA(val, p, record) {
        if (record.data.FlagPersonaFisica) {
            return record.data.CodiceFiscale;
        }   else {
            return record.data.PartitaIva;
        }
    }
 	 
    function renderDescrizioneTipologiaPersona(val, p, record) {
        if (val == true) {
            return 'Persona Fisica';
        } else {
            return 'Persona Giuridica';
        }
    }

    function formatBoolean(aValue, aMetadata, aRecord, aRowInderx, aColIndex, aStore) {
        aMetadata.css += ' x-grid3-check-col-td';
        return '<div class="x-grid3-check-col' + (aValue != null && aValue == true ? '-on' : '') + '"> </div>';
    }

    var isDatoSensibileColumn = new Ext.grid.CheckColumn({
        header: "Dato Sensibile",
        dataIndex: 'IsDatoSensibile',
        width: 60
        //,disabled: false
        //, readOnly: false
       , renderer: formatBoolean, 
        editor: { xtype: 'checkbox' }

    });

    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Tipologia", dataIndex: 'FlagPersonaFisica', width: 120, renderer: renderDescrizioneTipologiaPersona, sortable: true, locked: false, hidden: false },
        { header: "Cod. Fisc./Partita IVA", width: 120, dataIndex: 'CodiceFiscale', id: 'CodiceFiscale', renderer: renderSwitchCodFiscPartitaIVA, sortable: true,
            summaryRenderer: function(v, params, data) { return '<b> Totale </b>'; } 
        },
        { header: "Denominazione", width: 150, dataIndex: 'Denominazione', sortable: true, locked: false },
        isDatoSensibileColumn,
        { header: "Num. Repertorio Contratto", width: 150, dataIndex: 'NumeroRepertorioContratto', sortable: true, hidden: false,
            renderer: function(value, metaData, record, rowIdx, colIdx, store) {
                var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
                var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();
                
                var href = "http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + record.data.IdContratto;
                var link = "<a target='_blank' href='"+href+"' style=\"font-size:10px;text-decoration:underline;color:#F85118\" >" + record.data.NumeroRepertorioContratto + "</a>";
                return link;
            }
        },
        { renderer: eurRend, header: "Importo Spettante", width: 100, align: 'right', dataIndex: 'ImportoSpettante', id: 'ImportoSpettante', sortable: true, summaryType: 'sum'
        },        
        { header: "ID", dataIndex: 'ID', sortable: true, locked: false, hidden: true },
        { header: NDocumentoContabile, dataIndex: NDocumentoContabile, sortable: true, locked: false, hidden: true },
        { header: "IdAnagrafica", dataIndex: 'IdAnagrafica', sortable: true, locked: false, hidden: true }
 	    ]);
         
    var GridBeneficiari = new Ext.grid.GridPanel({ 
    	id: gridPanelId,
        ds: store,
 		colModel :ColumnModel,
 		sm:  sm,	
        autoHeight:true,
        //plugins: [new Ext.grid.GroupSummary()],
        autoWidth: true,
        style: 'margin-top:10px',
        border: 10,
        layout: 'fit',
        loadMask: true,
        title: titoloGrid,
        tbar: [actionAddBeneficiario, actionDeleteBeneficiario, actionDetailBeneficiario]
        //,view: new Ext.grid.GroupingView({
        //    forceFit: true,
        //    showGroupName: false,
        //    enableNoGroups:true, 
        //    hideGroupedColumn: false,
        //    enableGroupingMenu: true
        //})

    });

    GridBeneficiari.on('render', function() {
        this.getView().mainBody.on('mousedown', function(e, t) {
            if (t.tagName == 'A') {
                e.stopEvent();
                t.click();
            }
        });
    }, GridBeneficiari);
	          
    actionAddBeneficiario.setDisabled(false); 
    actionDeleteBeneficiario.setDisabled(true); 
    actionDetailBeneficiario.setDisabled(true);

    return GridBeneficiari;
}

//per caricare i dati dal SIC utilizzare la prima signature del metodo, se da DB la seconda
//function showDetailBeneficiario(anagrafica, sede, conto, liquidazione, gridPanelId, modificaAbilitata) {
function showDetailBeneficiario(data, gridPanelId, modificaAbilitata) {
    var windowDetailAnagrafica = new Ext.Window({
        title: 'Dettaglio',
        width: 700,
        autoHeight: true,
        layout: 'fit',
        plain: true,
        bodyStyle: 'padding:10px',
        buttonAlign: 'center',
        maximizable: false,
        enableDragDrop: true,
        collapsible: false,
        modal: true,
        autoScroll: false,
        closable: true,
        resizable: false /*, 
        listeners: {
            'close': function(p) {
                Ext.getCmp(gridPanelId).getStore().reload();
            }
        }
        */
    });

    //DEFINISCO L'AZIONE MODFICA DATI ANAGRAFICI BENEFICIARIO
    var actionModificaDatiAnagrafici = new Ext.Action({
       id: 'btnModifica',
       text: 'Modifica',
       tooltip: 'Modifica dati anagrafici beneficiario',
       handler: function() {
            editAnagrafica({ value: Ext.get("hidAnagrafica").value,
               idAnagrafica: Ext.get("hidAnagrafica").value.ID,
               updateFn: function(objectType, insertMode, objectData) {
                   loadAnagrafica(objectData,
                        function(objectData, anagrafica, insertMode) {
                            fillViewAnagrafica(anagrafica);
                        }, insertMode);
               }
           });           
       },
       iconCls: 'edit-pen'
    });

    //DEFINISCO L'AZIONE MODFICA DATI SEDE
    var actionModificaDatiSede = new Ext.Action({
        text: 'Modifica',
        tooltip: 'Modifica dati sede',
        handler: function() {
            editSede({ value: Ext.get("hidSede").value,
                idAnagrafica: Ext.get("hidAnagrafica").value.ID,
                idSede: Ext.get("hidSede").value.IdSede,
                disableModalitaPagamento: true,
                updateFn: function(objectType, insertMode, objectData) {
                    loadAnagrafica(objectData,
                            function(objectData, anagrafica, insertMode) {
                                var sede = getSede(anagrafica.ListaSedi, objectData.idSede);
                                fillViewSede(sede);
                            }, insertMode);
                }
            });
        },
        iconCls: 'edit-pen'
    });
 
    //DEFINISCO L'AZIONE MODFICA DATI BANCARI
    var actionModificaDatiBancari = new Ext.Action({
        text: 'Modifica',
        tooltip: 'Modifica dati bancari',
        handler: function() {
            if (conto.Iban != null) {
                editDatiBancari({ value: Ext.get("hidConto").value,
                    idAnagrafica: Ext.get("hidAnagrafica").value.ID,
                    idSede: Ext.get("hidSede").value.IdSede,
                    idConto: Ext.get("hidConto").value.IdContoCorrente,
                    updateFn: function(objectType, insertMode, objectData) {
                        loadAnagrafica(objectData,
                            function(objectData, anagrafica, insertMode) {
                                var sede = getSede(anagrafica.ListaSedi, objectData.idSede);
                                var conto = getConto(sede.DatiBancari, objectData.idConto);
                                fillViewDatiBancari(sede, conto);
                            }, insertMode);
                    }
                });
            } else if (conto.ContoCorrente != null) {
                editDatiContoCorrente({ value: Ext.get("hidConto").value,
                    idAnagrafica: Ext.get("hidAnagrafica").value.ID,
                    idSede: Ext.get("hidSede").value.IdSede,
                    idConto: Ext.get("hidConto").value.IdContoCorrente,
                    updateFn: function(objectType, insertMode, objectData) {
                        loadAnagrafica(objectData,
                            function(objectData, anagrafica, insertMode) {
                                var sede = getSede(anagrafica.ListaSedi, objectData.idSede);
                                var conto = getConto(sede.DatiBancari, objectData.idConto);
                                fillViewDatiBancari(sede, conto);
                            }, insertMode);
                    }
                });
            }
        },
        iconCls: 'edit-pen'
    });

    //DEFINISCO L'AZIONE MODFICA DATI LIQUIDAZIONE
    var actionModificaDatiLiquidazione = new Ext.Action({
        text: 'Modifica',
        tooltip: 'Modifica dati liquidazione',
        handler: function() {
            editDatiLiquidazione(Ext.get("hidDatiLiquidazione").value, gridPanelId);
        },
        iconCls: 'edit-pen'
    });

    var panelDetailBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        //tbar: [actionModificaDatiAnagrafici],
        tbar: new Ext.Toolbar({
            id: 'tbarBeneficiario',
            items: [actionModificaDatiAnagrafici]
        }),
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
        "margin-top": "0px",
        "margin-bottom": "10px",
        "margin-left": "10px", // when you add custom margin in IE 6...
        "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
           columns: 2
        },
        title: "Beneficiario",
        id: 'panelDetailBeneficiario',
        cls: 'pannelliDettaglio',
        items: [
            {
                xtype: 'label',
                text: 'Denominazione'
            },  {
                 xtype: 'textfield',
                id: 'denominazioneID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
            },
             {
                 xtype: 'label',
                 text: 'Cod. Fisc./Partita IVA',
                 columnWidth: .9
             },  {
                 xtype: 'textfield',
                id: 'codFiscID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
            },
            {
                xtype: 'label',
                text: 'Dato sensibile',
                columnWidth: .9
            }, {
                xtype: 'textfield',
                id: 'datoSensibileID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
            }
            ]
        });

        var actionVisualizzaContratto = new Ext.Action({
            text: data.NumeroRepertorioContratto,
            id: 'actionApriContrattoId',
            tooltip: 'Visualizza il contratto relativo al numero di repertorio indicato',
            handler: function() {
                var codFiscOperatore = Ext.get('codFiscOperatore').getValue();
                var uffPubblicoOperatore = Ext.get('uffPubblicoOperatore').getValue();
                var num_contr = data.IdContratto;

                window.open("http://oias.rete.basilicata.it/zzhdbzz/f?p=contr_trasp:50:::::PARAMETER_ID,UFFICIO,NUM_CONTR:" + codFiscOperatore + "," + uffPubblicoOperatore + "," + num_contr, '_blank');
            }
        });

    var panelDetailContratto = new Ext.Panel({
            xtype: "panel",
            layout: 'table',
            autoHeight: true,
            labelWidth: 50,
            bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
            border: false,
            style: {
                "margin-top": "10px",
                "margin-bottom": "10px",
                "margin-left": "10px", // when you add custom margin in IE 6...
                "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
            },
            width: 650,

            layoutConfig: {
                columns: 2
            },
            title: "Contratto",
            id: 'panelDetailContratto',
            cls: 'pannelliDettaglio',
            items: [            
            {
            xtype: 'label',
            text: 'Numero Repertorio '
        }, new Ext.LinkButton(actionVisualizzaContratto)
            ]
    });


    var panelDetailSede = new Ext.Panel({
            xtype: "panel",
            layout: 'table',
            autoHeight: true,
            labelWidth: 50,
            tbar: new Ext.Toolbar({
                id: 'tbarSede',
                items: [actionModificaDatiSede]
            }),
            bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
            border: false,
            style: {
                "margin-top": "10px",
                "margin-bottom": "10px",
                "margin-left": "10px", // when you add custom margin in IE 6...
                "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
            },
            width: 650,

            layoutConfig: {
                columns: 2
            },
            title: "Sede",
            id: 'panelDetailSede',
            cls: 'pannelliDettaglio',
            items: [
            // per caricare i dati dal SIC inserire anche il nome della sede nel dettaglio beneficiari
            /*
            {
                xtype: 'label',
                text: 'Nome'
            }, {
                xtype: 'textfield',
                id: 'nomeSedeID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
            },
            */
            {
                xtype: 'label',
                text: 'Via'                 
            }, {
                xtype: 'textfield',
                id: 'viaID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
            },
             {
                 xtype: 'label',
                 text: 'Comune'                     
             }, {
                 xtype: 'textfield',
                id: 'comuneID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
             },
             {
                 xtype: 'label',
                 text: 'Provincia'
             }, {
                 xtype: 'textfield',
                id: 'provinciaID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
             },
             {
                 xtype: 'label',
                 text: 'Modilità di Pagamento'
             }, {
                 xtype: 'textfield',
                id: 'modalitaPagID',
                style: 'opacity:.9',
                disabled: true,
                width: 400
             }
            ]
    });

    var panelDetailContoCorrente = new Ext.Panel({
                xtype: "panel",
                layout: 'table',
                autoHeight: true,
                labelWidth: 50,
                tbar: new Ext.Toolbar({
                    id: 'tbarContoCorrente',
                    items: [actionModificaDatiBancari]
                }),
                //tbar: [actionModificaDatiBancari],
                bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent',
                border: false,
                style: {
                    "margin-top": "10px",
                    "margin-bottom": "10px",
                    "margin-left": "10px", // when you add custom margin in IE 6...
                    "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
                },
                width: 650,
                layoutConfig: {
                    columns: 2                       
                },
                title: "Dati Pagamento",
                id: 'panelDetailContoCorrente',
                cls: 'pannelliDettaglio',
                items: [
                    {
                        xtype: 'label',
                        text: '',
                        id: 'labelDatiBancari'
                    }, {
                        xtype: 'textfield',
                        id: 'ibanID',
                        style: 'opacity:.9',
                        disabled: true,
                        width: 400
                    }
            ]
    });

    var panelDetailLiquidazione = new Ext.Panel({
                xtype: 'panel',
                layout: 'table',
                autoHeight: true,
                labelWidth: 50,
                bodyStyle: Ext.isIE ? 'padding:10px 10px 0px 15px;' : 'padding:10px 10px 10px 15px;background: transparent',
                border: false,
                tbar: new Ext.Toolbar({ 
                        id: 'tbarLiquidazione',
                        items: [actionModificaDatiLiquidazione] 
                    }),
                style: {
                    "margin-top": "10px",
                    "margin-bottom": "0px",
                    "margin-left": "10px", // when you add custom margin in IE 6...
                    "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
                },
                width: 650,
                layoutConfig: {
                    columns: 2
                },
                title: "Liquidazione",
                id: 'panelDetailLiquidazione',
                cls: 'pannelliDettaglio',
                items: [
                        {
                            xtype: 'label',
                            text: 'C.U.P'
                        }, {
                           xtype: 'textfield',
                            id: 'cupID',
                            style: 'opacity:.9',
                            disabled: true,
                            width: 400
                        },
                        {
                            xtype: 'label',
                            text: 'C.I.G'
                        }, {
                           xtype: 'textfield',
                            id: 'cigID',
                            style: 'opacity:.9',
                            disabled: true,
                            width: 400
                        },
                        {
                            xtype: 'label',
                            text: 'Codice Siope'
                        }, {
                            xtype: 'textfield',
                            id: 'codiceSiopeID',
                            style: 'opacity:.9',
                            disabled: true,
                            width: 400
                        },                        
                        {
                            xtype: 'label',
                            text: 'Importo Spettante'
                        }, {
                            xtype: 'textfield',
                            id: 'importoID',
                            style: 'opacity:.9',
                            disabled: true,
                            width: 400
                        }
                        ]
                });
               
    var panelVuoto = new Ext.Panel({
         xtype: 'panel',
        layout: 'table',
        autoHeight: true,
        
        cls: 'pannelliDettaglio',
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,                   
        style: {
            "margin-top": "0px",
            "margin-bottom": "0px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 650,
        layoutConfig: {
            columns: 2
        },
   
        id: 'panelVuoto',
        items: [
            {
                xtype: 'hidden',
                id: 'hidAnagrafica'
            }, {
                xtype: 'hidden',
                id: 'hidSede'
            }
            , {
                xtype: 'hidden',
                id: 'hidConto'
            }
            , {
                xtype: 'hidden',
                id: 'hidDatiLiquidazione'
            }
        ]
    });
   
    Ext.getCmp('panelVuoto').hide();
    
    windowDetailAnagrafica.add(panelVuoto);
    windowDetailAnagrafica.add(panelDetailBeneficiario);
    if(data.IdContratto!=undefined && data.IdContratto!=null && data.IdContratto.trim().length>0)
        windowDetailAnagrafica.add(panelDetailContratto);
    windowDetailAnagrafica.add(panelDetailSede);
        
    // anagrafica, sede, conto, liquidazione, modificaAbilitata
    // per caricare i dati dal SIC utilizzare l'IF seguente piuttosto che l'altro (serve se i dati sono caricati dal DB)
    // if (conto != null && conto.IdContoCorrente != null && conto.IdContoCorrente != undefined && conto.IdContoCorrente != "")
    if (data.IdConto != null && data.IdConto != undefined && data.IdConto != "") 
        windowDetailAnagrafica.add(panelDetailContoCorrente);
    
    windowDetailAnagrafica.add(panelDetailLiquidazione);

    if (!modificaAbilitata) {
        Ext.getCmp('tbarBeneficiario').hide();
        Ext.getCmp('tbarSede').hide();
        Ext.getCmp('tbarContoCorrente').hide();
        Ext.getCmp('tbarLiquidazione').hide();
    }

    windowDetailAnagrafica.doLayout();
    windowDetailAnagrafica.show();


    // per caricare i dati dal SIC utilizzare le 4 funzioni di seguito.              
    /*
        fillViewAnagrafica(anagrafica);
        fillViewSede(sede);
        fillViewDatiBancari(sede, conto);
        fillViewLiquidazione(liquidazione);
    */
    
    // per caricare i dati dal DB utilizzare le 4 funzioni di seguito.
    fillViewAnagrafica(data);
    fillViewSede(data);
    fillViewDatiBancari(data);
    fillViewLiquidazione(data);
}

function loadAndShowDetailBeneficiario(beneficiarioInfo, gridPanelId, modificaAbilitata) {
    loadAnagrafica({ idAnagrafica: beneficiarioInfo.IdAnagrafica,
                    modificaAbilitata: modificaAbilitata,
                    beneficiarioInfo: beneficiarioInfo,
                    gridPanelId: gridPanelId },
        function(objectData, record, insertMode) {
            var anagrafica = record;
            var sede = getSede(anagrafica.ListaSedi, objectData.beneficiarioInfo.IdSede);
            var conto = getConto(sede.DatiBancari, objectData.beneficiarioInfo.IdConto);
            var liquidazione = getLiquidazione(objectData.beneficiarioInfo);
            // per caricare i dati dal SIC decommentare l'istruzione seguente
            showDetailBeneficiario(anagrafica, sede, conto, liquidazione, objectData.gridPanelId, objectData.modificaAbilitata);
        }
    );
}

function getLiquidazione(beneficiarioInfo) {
    var TipoRecord = Ext.data.Record.create([
         { name: 'CodiceFiscale' }
        , { name: 'PartitaIva' }
        , { name: 'Denominazione' }
       
    ]);

    var record = new TipoRecord({
        HasDatiBancari: beneficiarioInfo.HasDatiBancari
        , IDLiquidazione: beneficiarioInfo.IDLiquidazione
        , IdDocumento: beneficiarioInfo.IdDocumento
        , ID: beneficiarioInfo.ID
        , Cig: beneficiarioInfo.Cig
        , Cup: beneficiarioInfo.Cup
        , ImportoSpettante: beneficiarioInfo.ImportoSpettante
    });

    return record.data;
}

function getSede(listaSedi, idSede) {
    var sede;
    if (listaSedi != null) {
        for (var i = 0; i < listaSedi.length; i++) {
            if (listaSedi[i].IdSede == idSede) {
                sede = listaSedi[i];
                break;
            }
        }
    }
    return sede;
}

function getConto(listaConti, idConto) {
    var conto;
    if (listaConti != null) {
        for (var i = 0; i < listaConti.length; i++) {
            if (listaConti[i].IdContoCorrente == idConto) {
                conto = listaConti[i];
                break;
            }
        }
    }
    return conto;
}

// per caricare i dati dal SIC
/*
function fillViewAnagrafica(anagrafica) {
    if (anagrafica.Tipologia == "F") {
        Ext.getCmp("codFiscID").setValue(anagrafica.CodiceFiscale);
        Ext.getCmp("denominazioneID").setValue(anagrafica.Cognome + " " + anagrafica.Nome);
    } else {
        Ext.getCmp("codFiscID").setValue(anagrafica.PartitaIva);
        Ext.getCmp("denominazioneID").setValue(anagrafica.Denominazione);
    }

    Ext.get("hidAnagrafica").value = anagrafica;
}

function fillViewSede(sede) {
    Ext.getCmp("nomeSedeID").setValue(sede.NomeSede);
    Ext.getCmp("viaID").setValue(sede.Indirizzo);
    Ext.getCmp("comuneID").setValue(sede.Comune);
    Ext.getCmp("provinciaID").setValue(sede.CapComune);

    Ext.get("hidSede").value = sede;
}

function fillViewDatiBancari(sede, conto) {
    if (sede != null && sede.ModalitaPagamento != null)
        Ext.getCmp("modalitaPagID").setValue(sede.ModalitaPagamento);

    if (conto != null) {
        if (conto.Iban != null) {
            Ext.getCmp("ibanID").setValue(conto.Iban);
            Ext.getCmp("labelDatiBancari").setText("IBAN");
        } else if (conto.ContoCorrente != null) {
            Ext.getCmp("ibanID").setValue(conto.ContoCorrente);
            Ext.getCmp("labelDatiBancari").setText("Conto Corrente");
        }
    }

    Ext.get("hidConto").value = conto;
}

function fillViewLiquidazione(liquidazione) {
    Ext.getCmp("cigID").setValue(liquidazione.Cig);
    Ext.getCmp("cupID").setValue(liquidazione.Cup);
    Ext.getCmp("importoID").setValue("€ " + liquidazione.ImportoSpettante);

    Ext.get("hidDatiLiquidazione").value = liquidazione;
}
*/

// per caricare i dati dal DB
function fillViewAnagrafica(data) {
    if (data.FlagPersonaFisica) {
        Ext.getCmp("codFiscID").setValue(data.CodiceFiscale);
    } else {
        Ext.getCmp("codFiscID").setValue(data.PartitaIva);
    }
    Ext.getCmp("denominazioneID").setValue(data.Denominazione);
    Ext.getCmp("datoSensibileID").setValue(data.IsDatoSensibile ? "Si" : "No");

    Ext.get("hidAnagrafica").value = data;
}

function fillViewSede(data) {
    //Ext.getCmp("nomeSedeID").setValue(data.NomeSede);
    Ext.getCmp("viaID").setValue(data.SedeVia);
    Ext.getCmp("comuneID").setValue(data.SedeComune);
    Ext.getCmp("provinciaID").setValue(data.SedeProvincia);

    Ext.get("hidSede").value = data;
}

function fillViewDatiBancari(data) {
    Ext.getCmp("modalitaPagID").setValue(data.DescrizioneModalitaPag);

    if (data.IdConto != null && data.IdConto != undefined && data.IdConto != "") {
        if (data.Iban != null && data.Iban != "") {
            Ext.getCmp("ibanID").setValue(data.Iban);
            if (isNaN(data.Iban)) 
                Ext.getCmp("labelDatiBancari").setText("IBAN");
            else 
                Ext.getCmp("labelDatiBancari").setText("Conto Corrente");
        }
    }
            
    Ext.get("hidConto").value = data;
}

function fillViewLiquidazione(data) {
    Ext.getCmp("cigID").setValue(data.Cig);
    Ext.getCmp("cupID").setValue(data.Cup);
    Ext.getCmp("codiceSiopeID").setValue(data.CodiceSiope);
    Ext.getCmp("importoID").setValue("€ " + data.ImportoSpettante);

    Ext.get("hidDatiLiquidazione").value = data;
}

function buildBeneficarioCronologiaObject(beneficiario, sede, conto) { 
    var TipoRecord = Ext.data.Record.create([
                  { name: 'CodFiscPIva' }
                , { name: 'ContatoreFrequenza' }
                , { name: 'IdBeneficiario' }
                , { name: 'IdSede' }
                , { name: 'IdTipoPagamento' }
                , { name: 'IdContoCorrente' }
                , { name: 'FlagPersonaFisica' }
                , { name: 'Nominativo' }
                , { name: 'DataNasc' }
                , { name: 'LuogoNasc' }
                , { name: 'LegaleRappresentante' }
                , { name: 'DescrSede' }
                , { name: 'DescrModPagamento' }
                , { name: 'DescrDatiBancari' }
                , { name: 'DataUltimoUtilizzo' }
    		 ]);

     var currentTime = new Date()
     var month = currentTime.getMonth() + 1
     var day = currentTime.getDate()
     var year = currentTime.getFullYear()

     var legaleRappresentante = "";
     
     if (beneficiario.LegaleRappresentante != null && beneficiario.LegaleRappresentante != undefined) {
         if (beneficiario.LegaleRappresentante.Cognome != null && beneficiario.LegaleRappresentante.Cognome != undefined)
             legaleRappresentante += beneficiario.LegaleRappresentante.Cognome;
         if (beneficiario.LegaleRappresentante.Nome != null && beneficiario.LegaleRappresentante.Nome != undefined)
             legaleRappresentante += " " + beneficiario.LegaleRappresentante.Nome;
     }

     var record = new TipoRecord({
        CodFiscPIva: beneficiario.Tipologia == 'F' ? beneficiario.CodiceFiscale : (beneficiario.PartitaIva != null && beneficiario.PartitaIva != undefined ? beneficiario.PartitaIva : "")         
        , ContatoreFrequenza: 0
        , IdBeneficiario: beneficiario.ID
        , IdSede: sede.IdSede
        , IdTipoPagamento: sede.IdModalitaPagamento
        , IdContoCorrente: (conto!=null && conto!=undefined) ? conto.IdContoCorrente : ''
        , FlagPersonaFisica: beneficiario.Tipologia=='F'
        , Nominativo: beneficiario.Tipologia == 'F' ? beneficiario.Cognome + ' ' + beneficiario.Nome : beneficiario.Denominazione
        , DataNasc: beneficiario.Tipologia == 'F' ? beneficiario.DataNascita : ''
        , LuogoNasc: beneficiario.Tipologia == 'F' ? beneficiario.ComuneNascita : ''
        , LegaleRappresentante: legaleRappresentante
        , DescrSede: sede.NomeSede + ': ' + sede.Indirizzo + ' - ' + (sede.CapComune != null && sede.CapComune != undefined && sede.CapComune != "" ? sede.CapComune + ' ' : '') + sede.Comune
        , DescrModPagamento: sede.ModalitaPagamento
        , DescrDatiBancari: (conto!=null && conto!=undefined) ? ((conto.Iban != null && conto.Iban != undefined && conto.Iban != "" ? conto.Iban : conto.ContoCorrente) + (conto.NomeBanca != null && conto.NomeBanca != undefined && conto.NomeBanca != "" ? " - " + conto.NomeBanca : "") + (conto.ModalitaPrincipale ? " (Modalità Principale)" : "")) : ''
        , DataUltimoUtilizzo: day+'/'+month+'/'+year
    });
        
    return record;
}

function generaBeneficiarioCronologia(beneficiario, sede, conto) {
    var jsonBeneficiarioCronologiaObject = '';

    var beneficarioCronologiaObject = buildBeneficarioCronologiaObject(beneficiario, sede, conto);
    jsonBeneficiarioCronologiaObject = Ext.util.JSON.encode(beneficarioCronologiaObject.data);

    if (jsonBeneficiarioCronologiaObject != '') {
        var params = { beneficiarioCronologia: jsonBeneficiarioCronologiaObject };

         Ext.Ajax.request({
            url: 'ProcAmm.svc/GestisciBeneficiarioCronologia' + window.location.search,            
            headers: { 'Content-Type': 'application/json' },
            params: Ext.encode(params),
            method: 'POST',
            success: function(response, result) {
            },
            failure: function(response, options) {
            }
        });
    }
}

