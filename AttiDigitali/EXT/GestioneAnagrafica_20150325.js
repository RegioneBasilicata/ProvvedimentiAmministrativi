function fnOnBeforeSearchCommand() {
    if (Ext.getCmp('GridCC') != undefined) {
        Ext.getCmp('tab_cc').remove(Ext.getCmp('GridCC'));
    }

    if (Ext.getCmp('GridSedi') != undefined) {
        Ext.getCmp('tab_sedi').remove(Ext.getCmp('GridSedi'));
    }
}

function fnOnAfterSearchCommand() {
    Ext.getCmp("tab_panel").doLayout();
    Ext.getCmp("tab_panel").hide();

    Ext.getCmp('panelGestioneAnagrafica').doLayout();
}

function fnOnStoreLoad(records) {
}

function fnOnSelect(selectedRow) {
    if (selectedRow != null) {
        var beneficiario = selectedRow.data;     
    
        if (Ext.getCmp("anagrafica") != null) {
            Ext.getCmp("anagrafica").setValue(Ext.util.JSON.encode(beneficiario));
        }

        Ext.get("idAnagrafica").value = beneficiario.ID;

        if (Ext.getCmp('GridSedi') != undefined) {
            Ext.getCmp('tab_sedi').remove(Ext.getCmp('GridSedi'));
        }

        if (Ext.getCmp('GridCC') != undefined) {
            Ext.getCmp('tab_cc').remove(Ext.getCmp('GridCC'));
        }

        var grigliasedi = buildGrigliaSedi(beneficiario.ListaSedi);

        Ext.getCmp("tab_sedi").enable();
        Ext.getCmp("tab_sedi").add(grigliasedi);

        Ext.getCmp("tab_cc").disable();
        Ext.getCmp('tab_panel').hideTabStripItem('tab_cc');
        
        Ext.getCmp('tab_panel').show();        
        setActivePanel('panelGestioneAnagrafica', 'tab_sedi');
        Ext.getCmp('tab_panel').doLayout();
               
    } else {
        Ext.getCmp("tab_sedi").disable();
        Ext.getCmp("tab_cc").disable();
        
        Ext.getCmp('tab_panel').hide();
    }
}

function buildPanelGestioneAnagrafica() {
    var actionAggiungiAnagrafica = new Ext.Action({
        text: 'Nuovo Beneficiario',
        tooltip: 'Aggiungi un nuovo beneficiario',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.anagrafica, insertModes.newObject);
        },
        iconCls: 'add'
    });

    var gestioneAnagrafica = new Ext.FormPanel({
        id: 'panelGestioneAnagrafica',
        url: 'GestioneAnagrafica.aspx' + window.location.search,
        labelAlign: 'top',
        border: false,
        tbar: [actionAggiungiAnagrafica],
        bodyStyle: 'padding:10px',
        width: 800,
        autoScroll: true,
        items: [
            { xtype: 'panel',
                layout: Ext.isIE ? 'anchor' : 'auto',
                border: false,
                anchor: Ext.isIE ? '-18' : '0',
                autoWidth: Ext.isIE ? false : true,
                items: [
            buildPanelSearchBeneficiari(false, false, fnOnBeforeSearchCommand, fnOnAfterSearchCommand, fnOnStoreLoad, fnOnSelect, undefined, undefined, undefined, undefined, undefined, undefined),
            {
                xtype: 'tabpanel',
                plain: true,
                id: 'tab_panel',
                autoHeight: true,
                hidden: true,
                defaults: {
                    bodyStyle: 'padding:10px'
                },
                style: {
                    "margin-top": "10px",
                    "margin-bottom": "10px"
                },
                items: [
                         { xtype: 'panel',
                             title: 'Sedi ',
                             autoHeight: true,
                             defaults: { autoHeight: true },
                             layout: 'auto',
                             id: 'tab_sedi',
                             autoWidth: Ext.isIE ? false : true
                         },
                          { xtype: 'panel',
                              title: 'Dati Pagamento ',
                              autoHeight: true,
                              defaults: { autoHeight: true },
                              layout: 'auto',
                              id: 'tab_cc',
                              autoWidth: Ext.isIE ? false : true
                          }
                    ]
            }]
            }
             , {
                 xtype: 'hidden',
                 id: 'anagrafica'
             }, {
                 xtype: 'hidden',
                 id: 'sede'
             }, {
                 xtype: 'hidden',
                 id: 'conto'
             },
              {
                  xtype: 'hidden',
                  id: 'idAnagrafica'
              }, {
                  xtype: 'hidden',
                  id: 'idSede'
              }, {
                  xtype: 'hidden',
                  id: 'idConto'
}]
    });

          return gestioneAnagrafica;
}


function editAnagrafica(objectData) {
    InitFormInsertAnagrafica(objectTypes.anagrafica, insertModes.editObject, objectData);
}

function editSede(objectData) {
    InitFormInsertAnagrafica(objectTypes.sede, insertModes.editObject, objectData);
}

function editDatiBancari(objectData) {
    InitFormInsertAnagrafica(objectTypes.datiBancari, insertModes.editObject, objectData);
}

function editDatiContoCorrente(objectData) {
    InitFormInsertAnagrafica(objectTypes.datiContoCorrente, insertModes.editObject, objectData);
}


function buildGrigliaCC(sede, datiBancari) {
    var store = new Ext.data.Store();

    if (datiBancari != null) {    
        var TipoRecord = Ext.data.Record.create([
    	     {name: 'Abi'}
            ,{name: 'Cab'}
            ,{name: 'ContoCorrente'}
            ,{name: 'Cin'}
            ,{name: 'Iban'}
            ,{name: 'IdAgenzia'}
            ,{name: 'IdContoCorrente'}
            ,{name: 'NomeBanca'}
            ,{name: 'IndirizzoAgenzia'}
            ,{name: 'ModalitaPrincipale'}            
            ,{name: 'SedeAgenzia'}               
        ]);
		
	    for (var i=0; i<datiBancari.length; i ++ ){	
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
            });
            store.insert(i, record);
        }
    }    

    var sm = new Ext.grid.CheckboxSelectionModel({
          singleSelect: true,
          loadMask: true
         });
               		 
    var checkColumn = new Ext.grid.CheckColumn({
       header: "Principale",
       dataIndex: 'ModalitaPrincipale',
       width: 55,
       readOnly:true
    });
    		     
    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Abi", width: 40, dataIndex: 'Abi', id: 'Abi', sortable: true, hidden: true },
        { header: "Cab", width: 40, dataIndex: 'Cab', sortable: true, locked: false, hidden: true },
        { header: "ContoCorrente", width: 100, dataIndex: 'ContoCorrente', sortable: true, hidden: true },
        { header: "Cin", width: 10, dataIndex: 'Cin', sortable: true, locked: false, hidden: true },
     	{ header: "IBAN/Conto Corrente", width: 200, dataIndex: 'Iban', id: 'Iban',
     	    renderer: function(val, p, record) {
     	        return !isNullOrEmpty(record.data.Iban) ? record.data.Iban : record.data.ContoCorrente;
     	    }
     	, sortable: true },
     	{ header: "IdAgenzia", dataIndex: 'IdAgenzia', id: 'IdAgenzia', sortable: true, hidden: true },
        { header: "IdContoCorrente", width: 60, dataIndex: 'IdContoCorrente', sortable: true, hidden: true },
        { header: "Nome Banca", width: 150, dataIndex: 'NomeBanca', sortable: true, locked: false },
     	checkColumn,
     	{ header: "Sede Agenzia", width: 150, dataIndex: 'SedeAgenzia', sortable: true, locked: false },
     	{ header: "Indirizzo Agenzia", width: 150, dataIndex: 'IndirizzoAgenzia', sortable: true, locked: false }
     	]);

   	var actionAggiungiCCBancario = new Ext.Action({
     	text: 'Nuovo C/C Bancario',     	
     	tooltip: 'Aggiungi un nuovo conto corrente bancario',
 	    handler: function() {
 	        InitFormInsertAnagrafica(objectTypes.datiBancari, insertModes.newObject, 
 	            { idAnagrafica: Ext.get('idAnagrafica').value,
 	              idSede: Ext.get('idSede').value,
 	              updateFn: editInsertUpdateFn
 	            });
 	    },
 	    iconCls: 'add'
 	});

    var actionAggiungiContoCorrente = new Ext.Action({
        text: 'Nuovo Conto Corrente',
        tooltip: 'Aggiungi un nuovo conto corrente (es. postale)',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.datiContoCorrente, insertModes.newObject,
                { idAnagrafica: Ext.get('idAnagrafica').value,
                  idSede: Ext.get('idSede').value,
                  istitutoRiferimento: sede.IstitutoRiferimento,
                  updateFn: editInsertUpdateFn
                });
        },
        iconCls: 'add'
    });

    var actionModificaConto = new Ext.Action({
        text: 'Modifica Conto',
        tooltip: 'Modifica il conto selezionato',
        handler: function() {
            Ext.each(Ext.getCmp('GridCC').getSelectionModel().getSelections(), function(rec) {
            if (rec.data.NomeBanca != null && rec.data.NomeBanca != undefined && (rec.data.NomeBanca == "UFFICIO POSTALE" || rec.data.NomeBanca == "BANCA D'ITALIA"))
                editDatiContoCorrente({ value: rec.data,
                    idAnagrafica: Ext.get('idAnagrafica').value,
                    idSede: Ext.get('idSede').value,
                    idConto: Ext.get('idConto').value,
                    updateFn: editInsertUpdateFn
                }); 	            
            else             
                editDatiBancari({ value: rec.data,
                    idAnagrafica: Ext.get('idAnagrafica').value,
                    idSede: Ext.get('idSede').value,
                    idConto: Ext.get('idConto').value,
                    updateFn: editInsertUpdateFn
                }); 	            
            })
        },
        iconCls: 'edit-pen'
    });
 	     	    	              	
    var grid = new Ext.grid.GridPanel({
	      id:'GridCC',
	      autoHeight: true,  
          autoWidth: true,     
          border: true,
          cm: ColumnModel,
          ds: store,
          stripeRows: true,
          tbar: [actionAggiungiCCBancario, actionAggiungiContoCorrente], //, actionModificaConto],
          sm: sm,
          viewConfig: {
              forceFit : true,
              emptyText: "Nessun conto bancario presente.",
              deferEmptyText: false
          },
  	      plugins:checkColumn
          });
                    
          grid.addListener({
              'rowclick': {
                fn: function(grid, rowIndex, event) {
                    actionModificaConto.setDisabled(grid.getSelectionModel().getSelected() == null);
                    if (grid.getSelectionModel().getSelected() != null) {                          
                          var rec = grid.store.getAt(rowIndex);
                          if (Ext.getCmp("conto") != null) {
                              Ext.getCmp("conto").setValue(Ext.util.JSON.encode(rec.data));
                          }
                          Ext.get("idConto").value = rec.data.IdContoCorrente;                          
                      } else {
                          Ext.get("idConto").value = undefined;                          
                      }
                  }
            , scope: this
          }
      });

    actionModificaConto.setDisabled(true);

    actionAggiungiCCBancario.setDisabled(!(sede.HasDatiBancari && isNullOrEmpty(sede.IstitutoRiferimento)));
    actionAggiungiContoCorrente.setDisabled(!(sede.HasDatiBancari && !isNullOrEmpty(sede.IstitutoRiferimento)));
    
	return grid;
}
    
function buildGrigliaSedi(rec) {
    var store = new Ext.data.Store();
    
    if (rec != null) {
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
                   , { name: 'IstitutoRiferimento' },
                   , { name: 'HasDatiBancari' }                   
			    ]);
    	
	    for (var i=0; i<rec.length; i ++ ){	
            var record = new TipoRecord({
                     Comune:rec[i].Comune
                     ,CapComune:rec[i].CapComune
                     ,Email:rec[i].Email
                     ,Fax: rec[i].Fax
                     ,Bollo: rec[i].Bollo
                     ,Telefono: rec[i].Telefono
                     ,Indirizzo:rec[i].Indirizzo
                     ,DatiBancari: rec[i].DatiBancari
                     ,ModalitaPagamento: rec[i].ModalitaPagamento
                     ,IdSede:rec[i].IdSede
                     ,IdModalitaPagamento: rec[i].IdModalitaPagamento
                     ,NomeSede: rec[i].NomeSede
                     ,IstitutoRiferimento: rec[i].IstitutoRiferimento
                     ,HasDatiBancari: rec[i].HasDatiBancari
            });
            store.insert(i, record);
        }
    }
         
    var sm = new Ext.grid.CheckboxSelectionModel(
 	    { singleSelect: true,
 	      loadMask: true
 	    });	           		 
	           		 	           		 
    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Nome", width: 115, dataIndex: 'NomeSede', id: 'NomeSede', sortable: true },
        { header: "Indirizzo",width: 164,dataIndex: 'Indirizzo', id: 'Indirizzo',sortable: true },
        { header: "Comune", width: 100,dataIndex: 'Comune',  id: 'Comune',  sortable: true },
        { header: "Cap", width: 42, dataIndex: 'CapComune', sortable: true},
        { header: "Modalità Pagamento", width: 160, dataIndex: 'ModalitaPagamento', sortable: true},        
        { header: "Fax", width: 82, dataIndex: 'Fax', sortable: true},	                
        { header: "IdSede", width: 180, dataIndex: 'IdSede',  sortable: false, locked:false, hidden:true},
        { header: "IdModalitaPagamento", width: 180, dataIndex: 'IdModalitaPagamento', sortable: false, locked: false, hidden: true}
    ]);

    var actionAggiungiSede = new Ext.Action({
        text: 'Nuova Sede',
        tooltip: 'Aggiungi una nuova sede',
        handler: function() {
        InitFormInsertAnagrafica(objectTypes.sede, insertModes.newObject,
            { idAnagrafica: Ext.get('idAnagrafica').value,
              updateFn: editInsertUpdateFn
            });
        },
        iconCls: 'add'
    });
    
    var actionAggiungiCCBancario = new Ext.Action({
        text: 'Nuovo C/C Bancario',
        tooltip: 'Aggiungi un nuovo conto corrente bancario',
        handler: function() {
            InitFormInsertAnagrafica(objectTypes.datiBancari, insertModes.newObject,
            { idAnagrafica: Ext.get('idAnagrafica').value,
              idSede: Ext.get('idSede').value,
              updateFn: editInsertUpdateFn
            });
        },
        iconCls: 'add'
    });

    var actionAggiungiContoCorrente = new Ext.Action({
        text: 'Nuovo Conto Corrente',
        tooltip: 'Aggiungi un nuovo conto corrente (es. postale)',
        handler: function() {
            var sede = Ext.getCmp('GridSedi').getSelectionModel().getSelected();

            InitFormInsertAnagrafica(objectTypes.datiContoCorrente, insertModes.newObject,
            { idAnagrafica: Ext.get('idAnagrafica').value,
                idSede: Ext.get('idSede').value,
                istitutoRiferimento: sede.data.IstitutoRiferimento,
                updateFn: editInsertUpdateFn
            });
        },
        iconCls: 'add'
    });

        
    var actionModificaSede = new Ext.Action({
        text: 'Modifica Sede',
        tooltip: 'Modifica la sede selezionata',
        handler: function() {
        Ext.each(Ext.getCmp('GridSedi').getSelectionModel().getSelections(), function(rec) {
                editSede({ value: rec.data, 
                    idAnagrafica: Ext.get('idAnagrafica').value,
                    idSede: Ext.get('idSede').value,
                    updateFn: editInsertUpdateFn
                    });
            })
        },
        iconCls: 'edit-pen'
    });
	            	
    var grid = new Ext.grid.GridPanel({
		  id:'GridSedi',
		  autoHeight: true,
		  autoWidth: true,
          border: true,	      			  
          ds: store,
          cm: ColumnModel,
          stripeRows: true,
          sm: sm,
          viewConfig: {
              forceFit : true,
              emptyText: "Nessuna sede trovata.",
              deferEmptyText: false
          },
          tbar: [actionAggiungiSede, actionAggiungiCCBancario, actionAggiungiContoCorrente]//, actionModificaSede]
    });

    grid.addListener({
        'rowclick': {
            fn: function(grid, rowIndex, event) {
                actionModificaSede.setDisabled(grid.getSelectionModel().getSelected() == null);
                if (grid.getSelectionModel().getSelected() != null) {
                    var currentSede = grid.store.getAt(rowIndex);
                    if (Ext.getCmp("sede") != null) {
                        Ext.getCmp("sede").setValue(Ext.util.JSON.encode(currentSede.data));
                    }
                    Ext.get("idSede").value = currentSede.data.IdSede;
                    if (Ext.getCmp('GridCC') != undefined) {
                        Ext.getCmp('tab_cc').remove(Ext.getCmp('GridCC'));                       
                    }
                    if (currentSede.data.DatiBancari == null) {
                        Ext.getCmp("tab_cc").disable();
                        Ext.getCmp('tab_panel').hideTabStripItem('tab_cc');
                    } else {
                        var grigliaDatiBancari = buildGrigliaCC(currentSede.data, currentSede.data.DatiBancari);
                                                
                        Ext.getCmp("tab_cc").enable();
                        Ext.getCmp("tab_cc").add(grigliaDatiBancari);

                        Ext.getCmp('tab_panel').unhideTabStripItem('tab_cc');

                        setActivePanel("panelGestioneAnagrafica", "tab_cc");
                        Ext.getCmp('tab_panel').doLayout();
                    }
                    actionAggiungiCCBancario.setDisabled(!(currentSede.data.HasDatiBancari && isNullOrEmpty(currentSede.data.IstitutoRiferimento)));
                    actionAggiungiContoCorrente.setDisabled(!(currentSede.data.HasDatiBancari && !isNullOrEmpty(currentSede.data.IstitutoRiferimento)));
                } else {
                    Ext.get("idSede").value = undefined;

                    Ext.getCmp("tab_cc").disable();
                    Ext.getCmp('tab_panel').hideTabStripItem('tab_cc');

                    actionAggiungiCCBancario.setDisabled(true);
                    actionAggiungiContoCorrente.setDisabled(true);
                }
            }
        , scope: this
        }
    });

    actionAggiungiCCBancario.setDisabled(true);
    actionAggiungiContoCorrente.setDisabled(true);
    actionModificaSede.setDisabled(true);

    return grid;  
}

function editInsertUpdateFn(objectType, insertMode, objectData) {
    loadAnagrafica(objectData, objectType == objectTypes.anagrafica ? updateGrigliaRicerca : (objectType == objectTypes.sede ? updateGrigliaSedi : updateGrigliaDatiBancari), insertMode);
}


function updateGridSediStore(record) {
    var records = Ext.getCmp('GridSedi').getStore().getRange();
    for (var i = 0; i < records.length; i++) {
        if (records[i].data.IdSede == record.IdSede) {
            records[i].data = record;
            break;
        }
    }
}

function updateGrigliaSedi(objectData, record, insertMode) {
    var idSedeFound = false;

    updateGridRicercaStore(record);

    if (insertMode == insertModes.editObject && objectData.idSede != undefined) {
        for (var i = 0; i < record.ListaSedi.length; i++) {
            if (record.ListaSedi[i].IdSede == objectData.idSede) {
                updateGridSediStore(record.ListaSedi[i]);

                idSedeFound = true;
                break;
            }
        }
    }

    if(insertMode == insertModes.newObject || !idSedeFound) {
        if (Ext.getCmp('GridSedi') != undefined) {
            Ext.getCmp('tab_sedi').remove(Ext.getCmp('GridSedi'));
        }

        var grigliasedi = buildGrigliaSedi(record.ListaSedi);

        Ext.getCmp("tab_sedi").enable();
        Ext.getCmp("tab_sedi").add(grigliasedi);
        Ext.getCmp("tab_sedi").doLayout();

        Ext.getCmp('tab_panel').show();

        Ext.getCmp("tab_cc").disable();
        Ext.getCmp('tab_panel').hideTabStripItem('tab_cc');

        setActivePanel("panelGestioneAnagrafica", "tab_sedi");
    }

    Ext.getCmp('GridSedi').getView().refresh();      
}

function updateGridCCStore(record) {
    var records = Ext.getCmp('GridCC').getStore().getRange();
    for (var i = 0; i < records.length; i++) {
        if (records[i].data.IdContoCorrente == record.IdContoCorrente) {
            records[i].data = record;
            break;
        }
    }
}

function updateGrigliaDatiBancari(objectData, record, insertMode) {
    var idContoCorrenteFound = false;

    updateGridRicercaStore(record);

    for (var i = 0; i < record.ListaSedi.length; i++) {
        if (record.ListaSedi[i].IdSede == objectData.idSede) {
            updateGridSediStore(record.ListaSedi[i]);

            if (insertMode == insertModes.editObject && objectData.idConto != undefined) {
                for (var j = 0; j < record.ListaSedi[i].DatiBancari.length; j++) {
                    if (record.ListaSedi[i].DatiBancari[j].IdContoCorrente == objectData.idConto) {
                        updateGridCCStore(record.ListaSedi[i].DatiBancari[j])

                        idContoCorrenteFound = true;
                        break;
                    }
                }
            }

            if (insertMode == insertModes.newObject || !idContoCorrenteFound) {
                if (Ext.getCmp('GridCC') != undefined) {
                    Ext.getCmp('tab_cc').remove(Ext.getCmp('GridCC'));
                }

                var grigliaDatiBancari = buildGrigliaCC(record.ListaSedi[i], record.ListaSedi[i].DatiBancari);

                Ext.getCmp("tab_cc").enable();
                Ext.getCmp('tab_panel').unhideTabStripItem('tab_cc');

                Ext.getCmp("tab_cc").add(grigliaDatiBancari);
                Ext.getCmp("tab_cc").doLayout();

                setActivePanel("panelGestioneAnagrafica", "tab_cc");
            }
            
            break;
        }
    }
               
    Ext.getCmp('GridCC').getView().refresh();   
}

function setActivePanel(panelName, panelNameToActive) {
    Ext.getCmp(panelName).active = Ext.getCmp(panelNameToActive);
    Ext.getCmp(panelName).active.show();
    Ext.getCmp(panelName).doLayout();
}

function scrollBottom(panelName) {
    var panelGestioneAnagraficaDom = Ext.getCmp(panelName).body.dom;
    var scrollValue = (panelGestioneAnagraficaDom.scrollHeight - panelGestioneAnagraficaDom.offsetHeight);

    Ext.getCmp(panelName).body.scrollTo('top', scrollValue, true);
}

function isNullOrEmpty(value) {
    return (value == null || value == undefined || value == "")
}