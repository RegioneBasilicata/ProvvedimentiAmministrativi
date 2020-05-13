var mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

function buildInfoPanel(msgText, style) {
    var infoPanel = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 50,
        bodyStyle: style,
        border: false,
        layoutConfig: {
            columns: 1
        },
        title: "",
        items: [{
            xtype: 'label',
            style: 'font-weight:bold; font-size:11px',
            text: msgText
        }]
    });

    return infoPanel;
}

function buildGridLiquidazioni(key) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    var qstring = '';

    if (key == undefined || key == "") {
        qstring = window.location.search;
    } else {
        qstring = '?key=' + key;
    }

    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetTutteLiquidazioniRegistrate' + qstring,
        method: 'GET'
    });

    var reader = new Ext.data.JsonReader({

        root: 'GetTutteLiquidazioniRegistrateResult',
        fields: [
           { name: 'Bilancio' },
           { name: 'UPB' },
           { name: 'MissionePrograma' },
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
           { name: 'NLiquidazione' },
           { name: 'Stato' }
          ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader,
        listeners: {
            'beforeload': function(store, operation, eOpts) {                
            }
        }
    });

    store.on({
        'load': {
            fn: function(store, records, options) {
                maskApp.hide();
            },
            scope: this
        }
    });

    maskApp.show();

    var codUfficioProp = Ext.get('valueUffProp').dom.value;    
    var parametri = { CodiceUfficio: codUfficioProp };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: true,
        listeners: {
            rowselect: function(sm, row, rec) {
                if (rec.data.Stato != 1) {
                    showPanelMandati(rec.data.NLiquidazione, rec.data.ImpPrenotatoLiq);
                } else {
                    if (!((rec.data.NLiquidazione == '') || (rec.data.NLiquidazione == '0'))) {
                        showPanelMandati(rec.data.NLiquidazione, rec.data.ImpPrenotatoLiq);
                    }
                }
            }
 	       ,
            rowdeselect: function(sm, row, rec) {
                hidePanelMandati();
            }
        }
    });

    var ColumnModel = new Ext.grid.ColumnModel({
        columns: [
	                    sm,
	                    { header: "N. Liquidazione", dataIndex: 'NLiquidazione', id: 'NLiquidazione', sortable: true },
						{ renderer: eurRend, header: "Importo<br/> da Liquidare", align: 'right', dataIndex: 'ImpPrenotatoLiq', sortable: true },
						{ header: "NumImpegno", dataIndex: 'NumImpegno', sortable: true },
						{ header: "NumPreImp", dataIndex: 'NumPreImp', sortable: true, hidden: true },
						{ header: "Capitolo", dataIndex: 'Capitolo', sortable: true },
						{ header: "UPB", dataIndex: 'UPB', sortable: true },
						{ header: "Missione.Programma", dataIndex: 'MissioneProgramma', sortable: true },
						{ header: "Esercizio", dataIndex: 'Bilancio', id: 'Bilancio', sortable: true },
						{ header: "ID", dataIndex: 'ID', id: 'ID', sortable: false, hidden: true },
						{ header: "Conto<br/> Economica", dataIndex: 'ContoEconomica', sortable: false,
						    editor: new Ext.form.TextField({
						        allowBlank: false
						    })
						},
					    { renderer: eurRend, header: "Importo<br/>Iva", align: 'right', dataIndex: 'ImportoIva', sortable: false,
					        editor: new Ext.form.NumberField({
					            decimalSeparator: ',',
					            allowBlank: true,
					            allowNegative: false
					        })
					    },
						{ header: "Stato", dataIndex: 'Stato', id: 'Stato', hidden: true }
		 ]
    });

    var GridLiq = new Ext.grid.EditorGridPanel({
        id: 'GridLiquidazioni',
        title: 'Liquidazioni per il presente provvedimento',
        ds: store,
        colModel: ColumnModel,
        sm: sm,
        autoHeight: true,
        autoWidth: true,
        layout: 'fit',
        loadMask: true,
        viewConfig: {
            emptyText: "Nessuna liquidazione presente.",
            deferEmptyText: true,
            forceFit: true
        }
    });
   
    return GridLiq;
}

function showPanelMandati(numeroLiquidazione, importoLiquidazione) {
    if (Ext.getCmp('GridMandati') != undefined) {
        Ext.getCmp('tab_mandati').remove(Ext.getCmp('GridMandati'));
    }

    var tipoAtto = undefined; 
    var numeroAtto = undefined;
    var dataAtto = undefined;
    var ufficioAtto = undefined;

    var grigliaMandati = buildGrigliaMandatiAtto(tipoAtto, numeroAtto, dataAtto, ufficioAtto, numeroLiquidazione, importoLiquidazione);

    Ext.getCmp("tab_mandati").setTitle("Mandati liquidazione n. " + numeroLiquidazione);
    Ext.getCmp("tab_mandati").enable();
    Ext.getCmp("tab_mandati").add(grigliaMandati);
    Ext.getCmp("tab_mandati").doLayout();

    Ext.getCmp("tab_beneficiari").disable();
    Ext.getCmp('tab_panel').hideTabStripItem('tab_beneficiari');

    Ext.getCmp('tab_panel').show();
    Ext.getCmp('tab_panel').doLayout();

    setActivePanel("panelGestioneMandati", "tab_mandati");
    scrollBottom("panelGestioneMandati");
}

function hidePanelMandati() {
    Ext.getCmp("tab_mandati").disable();
    Ext.getCmp("tab_beneficiari").disable();
    Ext.getCmp('tab_panel').hide();
}

function buildPanelGestioneMandati(key) {                             
    var gestioneMandati = new Ext.Panel({
        id: 'panelGestioneMandati',
        labelAlign: 'top',
        bodyStyle: 'padding:10px',
        width: 750,
        height: 450,
        layout: 'fit',
        autoScroll: true,
        items: [
            buildInfoPanel('Selezionare una liquidazione per visualizzazre la lista dei mandati e beneficiari associati.', 'padding:5px 0px 15px 1px'),
            buildGridLiquidazioni(key),
            {
              xtype: 'tabpanel',
              plain: true,
              id: 'tab_panel',                  
              autoHeight: true,
              defaults: { bodyStyle: 'padding:10px' },
              style: { 'margin-top': '15px' },
              items: [
                     { title: 'Mandati', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_mandati' },
                     { title: 'Beneficiari', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_beneficiari' }
              ]
          }
        ]
    });
    
    Ext.getCmp("panelGestioneMandati").doLayout();
    Ext.getCmp('tab_panel').hide();

    return gestioneMandati;
}

function buildGrigliaMandatiAtto(tipoAtto, numeroAtto, dataAtto, ufficioAtto, numeroLiquidazione, importoLiquidazione) {
    var maskApp = new Ext.LoadMask(Ext.getBody(), {
        msg: "Recupero Dati..."
    });

    //DEFINISCO L'AZIONE DETTAGLIO MANDATO
    var actionDetailMandato = new Ext.Action({
        text: 'Dettaglio',
        tooltip: 'Dettaglio del mandato selezionato',
        handler: function() {
            var gridMandati = Ext.getCmp('GridMandati');
            Ext.each(gridMandati.getSelectionModel().getSelections(),
                    function(record) {
                        showDetailMandato(record.data);
                    }
               )
        },
        iconCls: 'read'
    });

    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/InterrogaMandatiBeneficiariAtto',
        method: 'POST',
        timeout: 10000000
    });
    
    var reader = new Ext.data.JsonReader({
        root: 'InterrogaMandatiBeneficiariAttoResult',
        fields: [
            { name: 'DataAttoImp' },
            { name: 'DataAttoLiq' },
            { name: 'DataEmissioneMandato' },
            { name: 'DataIncarico' },
            { name: 'DataQuietanza' },
            { name: 'DataStorno' },
            { name: 'Dipartimento' },
            { name: 'EsercizioImpegno' },
            { name: 'EsercizioImpegnoRifPerEnte' },
            { name: 'EsercizioMandato' },
            { name: 'IdDocumento' },
            { name: 'ImportoImpegno' },
            { name: 'ImportoTotaleMandato' },
            { name: 'Incarico' },
            { name: 'NumeroAttoImp' },
            { name: 'NumeroAttoLiq' },
            { name: 'NumeroDistinta' },
            { name: 'NumeroImpegno' },
            { name: 'NumeroImpegnoRifPerEnte' },
            { name: 'NumeroLiquidazione' },
            { name: 'NumeroMandato' },
            { name: 'NumeroQuietanza' },
            { name: 'NumeroStorno' },
            { name: 'OggettoMandato' },
            { name: 'TipoAttoImp' },
            { name: 'TipoAttoLiq' },
            { name: 'TipoImpegno' },
            { name: 'ListaBeneficiari' },
            { name: 'ValidoAnnullato' }
           ]
    });

    var store = new Ext.data.GroupingStore({
        proxy: proxy,
        reader: reader,
        groupField: 'NumeroLiquidazione',
        sortInfo: {
            field: 'NumeroLiquidazione',
            direction: "ASC"
        }, 
        listeners: {
            'beforeload': function(store, operation, eOpts) {
                actionDetailMandato.setDisabled(true);
            }
        }
    });

    store.on({ 'load': {
        fn: function(store, records, options) {            
            maskApp.hide();
        },
        scope: this
    }
    });

    maskApp.show();

    var parametri = {
        tipoAtto: tipoAtto,
        numeroAtto: numeroAtto,
        dataAtto: dataAtto,
        ufficioAtto: ufficioAtto,
        numeroLiquidazione: numeroLiquidazione
    };

    try {
        store.load({ params: parametri });
    } catch (ex) {
        maskApp.hide();
    }

    var sm = new Ext.grid.CheckboxSelectionModel({
        singleSelect: true,
        loadMask: true
    });

    var summary = new Ext.grid.GroupSummary();

    var incaricoColumn = new Ext.grid.CheckColumn({
        header: "Incarico",
        dataIndex: 'Incarico',
        width: 48,
        readOnly: true
    });
   
    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Numero Mandato", width: 74, dataIndex: 'NumeroMandato', id: 'NumeroMandato', sortable: true,
            summaryRenderer: function(v, params, data) {
                return '<b> Totale </b>';
            } 
        },
        { header: "Data Emissione", width: 91, align: 'center', dataIndex: 'DataEmissioneMandato', id: 'DataEmissioneMandato', sortable: true },
        { header: "Oggetto Mandato", width: 178, dataIndex: 'OggettoMandato', id: 'OggettoMandato', sortable: true },
        { renderer: eurRend, header: "Importo Mandato", align: 'right', width: 102, dataIndex: 'ImportoTotaleMandato', id: 'ImportoTotaleMandato', sortable: true, locked: false, 
            summaryType:'sum',
            summaryRenderer: function(value) {
                if(value!=importoLiquidazione)
                    return '<b style="color:red">' + eurRend(value) + '</b>'
                else
                    return '<b>' + eurRend(value) + '</b>'
            }
        },
        incaricoColumn,
        { header: "Data Quietanza", width: 91, align: 'center', dataIndex: 'DataQuietanza', id: 'DataQuietanza', sortable: true },
        { header: "Numero Liquidazione", width: 104, dataIndex: 'NumeroLiquidazione', sortable: true, locked: false, hidden: true },
        { header: "Data Storno", width: 76, align: 'center', dataIndex: 'DataStorno', id: 'DataStorno', sortable: true }                    	                	                
    ]);
    
    var grid = new Ext.grid.GridPanel({
        id: 'GridMandati',
        autoHeight: false,
        height: 180,
        border: true,
        ds: store,
        cm: ColumnModel,
        plugins: [summary],
        stripeRows: true,
        tbar: [actionDetailMandato],     
        view: new Ext.grid.GroupingView({
            forceFit: true,
            showGroupName: false,
            enableNoGroups: true, 
            hideGroupedColumn: true,
            enableGroupingMenu: false,
            emptyGroupText: '',
            emptyText: "Nessun mandato associato."
        }),
        sm: sm
    });

    //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
    grid.addListener({
        'rowclick': {
            fn: function(grid, rowIndex, event) {
                actionDetailMandato.setDisabled(grid.getSelectionModel().getSelected() == null);
                
                var rec = grid.store.getAt(rowIndex);
                if (grid.getSelectionModel().getSelected() != null) {

                    if (Ext.getCmp('GridBeneficiari') != undefined)
                        Ext.getCmp('tab_beneficiari').remove(Ext.getCmp('GridBeneficiari'));

                    var grigliaBeneficiari = buildGrigliaBeneficiariMandato(rec.data.NumeroMandato, rec.data.ListaBeneficiari);

                    Ext.getCmp("tab_beneficiari").setTitle("Beneficiari mandato n. " + rec.data.NumeroMandato);
                    Ext.getCmp("tab_beneficiari").enable();
                    Ext.getCmp("tab_beneficiari").add(grigliaBeneficiari);
                    Ext.getCmp("tab_beneficiari").doLayout();

                    Ext.getCmp('tab_panel').unhideTabStripItem('tab_beneficiari');
                    Ext.getCmp('tab_panel').doLayout();
                } else {
                    Ext.getCmp("tab_beneficiari").disable();
                    Ext.getCmp('tab_panel').hideTabStripItem('tab_beneficiari');
                }
            }, scope: this
        }
    });

    return grid;
}

function buildGrigliaBeneficiariMandato(numeroMandato, beneficiari) {
    //DEFINISCO L'AZIONE DETTAGLIO BENEFICIARIO
    var actionDetailBeneficiario = new Ext.Action({
        text: 'Dettaglio',
        tooltip: 'Dettaglio del beneficiario selezionato',
        handler: function() {
            var gridBeneficiari = Ext.getCmp('GridBeneficiari');
            Ext.each(gridBeneficiari.getSelectionModel().getSelections(),
                    function(record) {
                        showDetailBeneficiario(record.data);
                    }
               )
        },
        iconCls: 'read'
    });

    var store = new Ext.data.Store();

    var store = new Ext.data.GroupingStore({
        groupField: 'NumeroMandato',
        sortInfo: {
            field: 'NumeroMandato',
            direction: "ASC"
        },
        listeners: {
            'beforeload': function(store, operation, eOpts) {
                actionDetailBeneficiario.setDisabled(true);
            }
        }
    });
    
    if (beneficiari != null) {
        var TipoRecord = Ext.data.Record.create([
             { name: 'ABI' }
            ,{ name: 'Banca' }
            ,{ name: 'CAB' }
            ,{ name: 'CapBanca' }
            ,{ name: 'CapFornitore' }
            ,{ name: 'CittaBanca' }
            ,{ name: 'CittaFornitore' }
            ,{ name: 'CodiceCig' }
            ,{ name: 'CodiceCup' }
            ,{ name: 'CodiceFiscaleRappresentanteLegale' }
            ,{ name: 'ContoCorrente' }
            ,{ name: 'DataNascitaRappresentanteLegale' }
            ,{ name: 'DescrizionePagamentoTesoriere' }
            ,{ name: 'IBAN' }
            ,{ name: 'ImponibileIrap' }
            ,{ name: 'ImponibileIrpef' }
            ,{ name: 'ImponibilePrevidenziale' }
            ,{ name: 'ImportoLordoBeneficiario' }
            ,{ name: 'ImportoNettoBeneficiario' }
            ,{ name: 'ImpostaIrap' }
            ,{ name: 'IndirizzoBanca' }
            ,{ name: 'IndirizzoFornitore' }
            ,{ name: 'InfoAggiuntive' }
            ,{ name: 'InfoPagamento' }
            ,{ name: 'MetodoPagamento' }
            ,{ name: 'NazioneBanca' }
            ,{ name: 'NazioneFornitore' }
            ,{ name: 'PartitaIvaFornitore' }
            ,{ name: 'ProvinciaBanca' }
            ,{ name: 'ProvinciaFornitore' }
            ,{ name: 'RappresentanteLegale' }
            ,{ name: 'RitenutaIrpef' }
            ,{ name: 'RitenutePrevidenzialiBeneficiario' }
            ,{ name: 'RitenutePrevidenzialiEnte' }
            ,{ name: 'StatoFornitore' }
            ,{ name: 'NomeFornitore' }
            ,{ name: 'NumeroMandato' }
        ]);
              
        for (var i = 0; i < beneficiari.length; i++) {
            var record = new TipoRecord({
                   ABI: beneficiari[i].ABI
                 , AddizionaleComunale: beneficiari[i].AddizionaleComunale
                 , AddizionaleRegionale: beneficiari[i].AddizionaleRegionale
                 , AltreRitenute: beneficiari[i].AltreRitenute
                 , Banca: beneficiari[i].Banca
                 , CAB: beneficiari[i].CAB
                 , CapBanca: beneficiari[i].CapBanca
                 , CapFornitore: beneficiari[i].CapFornitore
                 , CittaBanca: beneficiari[i].CittaBanca
                 , CittaFornitore: beneficiari[i].CittaFornitore
                 , CodiceCig: beneficiari[i].CodiceCig
                 , CodiceCup: beneficiari[i].CodiceCup
                 , CodiceFiscaleRappresentanteLegale: beneficiari[i].CodiceFiscaleRappresentanteLegale
                 , ContoCorrente: beneficiari[i].ContoCorrente
                 , DataNascitaRappresentanteLegale: beneficiari[i].DataNascitaRappresentanteLegale
                 , DescrizionePagamentoTesoriere: beneficiari[i].DescrizionePagamentoTesoriere
                 , IBAN: beneficiari[i].IBAN
                 , ImponibileIrap: beneficiari[i].ImponibileIrap
                 , ImponibileIrpef: beneficiari[i].ImponibileIrpef
                 , ImponibilePrevidenziale: beneficiari[i].ImponibilePrevidenziale
                 , ImportoLordoBeneficiario: beneficiari[i].ImportoLordoBeneficiario
                 , ImportoNettoBeneficiario: beneficiari[i].ImportoNettoBeneficiario
                 , ImpostaIrap: beneficiari[i].ImpostaIrap
                 , IndirizzoBanca: beneficiari[i].IndirizzoBanca
                 , IndirizzoFornitore: beneficiari[i].IndirizzoFornitore
                 , InfoAggiuntive: beneficiari[i].InfoAggiuntive
                 , InfoPagamento: beneficiari[i].InfoPagamento
                 , MetodoPagamento: beneficiari[i].MetodoPagamento
                 , NazioneBanca: beneficiari[i].NazioneBanca
                 , NazioneFornitore: beneficiari[i].NazioneFornitore
                 , PartitaIvaFornitore: beneficiari[i].PartitaIvaFornitore
                 , ProvinciaBanca: beneficiari[i].ProvinciaBanca
                 , ProvinciaFornitore: beneficiari[i].ProvinciaFornitore
                 , RappresentanteLegale: beneficiari[i].RappresentanteLegale
                 , RitenutaIrpef: beneficiari[i].RitenutaIrpef
                 , RitenutePrevidenzialiBeneficiario: beneficiari[i].RitenutePrevidenzialiBeneficiario
                 , RitenutePrevidenzialiEnte: beneficiari[i].RitenutePrevidenzialiEnte
                 , StatoFornitore: beneficiari[i].StatoFornitore
                 , NomeFornitore: beneficiari[i].NomeFornitore
                 , NumeroMandato: numeroMandato
            });
            store.insert(i, record);
        }
    }

    var sm = new Ext.grid.CheckboxSelectionModel(
 	    { singleSelect: true,
 	        loadMask: true
 	    });

    var summary = new Ext.grid.GroupSummary(); 

    //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var ColumnModel = new Ext.grid.ColumnModel([
        sm,
        { header: "Cod.Fisc./Partita Iva", width: 180, dataIndex: 'PartitaIvaFornitore', id: 'PartitaIvaFornitore', sortable: true,
            summaryRenderer: function(v, params, data) {
                return '<b> Totale </b>';
            }
        },
        { header: "Denominazione", width: 150, dataIndex: 'NomeFornitore', id: 'NomeFornitore', sortable: true },
        { renderer: eurRend, header: "Importo Lordo Beneficiario", width: 150, align: 'right', dataIndex: 'ImportoLordoBeneficiario', id: 'ImportoLordoBeneficiario', sortable: true },
        { header: "Numero Mandato", width: 110, dataIndex: 'NumeroMandato', sortable: true, locked: false, hidden: true },
        { renderer: eurRend, header: "Importo Netto Beneficiario", width: 150, align: 'right', dataIndex: 'ImportoNettoBeneficiario', id: 'ImportoNettoBeneficiario', sortable: true,
            summaryType: 'sum',
            summaryRenderer: function(value) {
                return '<b>' + eurRend(value) + '</b>'
            } 
        }
     	]);
    
    var grid = new Ext.grid.GridPanel({
        id: 'GridBeneficiari',
        autoHeight: true,
        title: '',
        border: true,
        viewConfig: { forceFit: true },
        ds: store,
        cm: ColumnModel,
        stripeRows: true,
        sm: sm,
        plugins: [summary],
        tbar: [actionDetailBeneficiario],     
        view: new Ext.grid.GroupingView({
            forceFit: true,
            showGroupName: false,
            enableNoGroups: true, 
            hideGroupedColumn: true,
            enableGroupingMenu: false,
            emptyGroupText: '',
            emptyText: "Nessun beneficiario associato."
        })
    });
    
    //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
    grid.addListener({
        'rowclick': {
            fn: function(grid, rowIndex, event) {
                actionDetailBeneficiario.setDisabled(grid.getSelectionModel().getSelected() == null);                
            }
            , scope: this
        }
    });

    actionDetailBeneficiario.setDisabled(true);
    
    return grid;
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

function showDetailMandato(mandato) {
    var windowDetailMandato = new Ext.Window({
        title: 'Dettaglio Mandato n. '+mandato.NumeroMandato,
        width: 750,
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
        resizable: false
    });

    var textFieldWidth = 250;

    var panelDetailDatiMandato = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 100,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 700,
        layoutConfig: {
            columns: 2
        },
        title: "Mandato n. " + mandato.NumeroMandato,
        id: 'panelDetailMandato',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
            {
                xtype: 'label',
                text: 'Data Emissione',
                columnWidth: .9
            }, 
            {
                xtype: 'textfield',
                id: 'dataEmissioneMandatoID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, 
            {
                xtype: 'label',
                text: 'Importo'
            },
            {
                xtype: 'textfield',
                id: 'importoTotaleMandatoID',
                style: 'opacity:.9;font-weight:bold',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, 
            {
                xtype: 'label',
                text: 'Oggetto'
            }, 
            {
                xtype: 'textarea',
                id: 'oggettoMandatoID',
                style: 'opacity:.9;height:40px',
                disabled: false,   
                readOnly: true,            
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Numero Distinta'
            }, 
            {
                xtype: 'textfield',
                id: 'numeroDistintaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },  
            {
                xtype: 'label',
                text: 'In Carico Tesoreria'
            },
            {
                xtype: 'textfield',
                id: 'dataIncaricoID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Data Quietanza'
            },
            {
                xtype: 'textfield',
                id: 'dataQuietanzaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Data Storno'
            }, 
            {
                xtype: 'textfield',
                id: 'dataStornoID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }
        ]
    });

    var panelDetailDatiLiquidazioneMandato = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 100,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 700,
        layoutConfig: {
            columns: 4
        },
        title: "Liquidazione n. "+mandato.NumeroLiquidazione,
        id: 'panelDetailLiquidazioneMandato',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
                {
                    xtype: 'label',
                    text: 'Numero Atto'
                },
                {
                    xtype: 'textfield',
                    id: 'numeroAttoLiquidazioneID',
                    style: 'opacity:.9',
                    colspan: 3,
                    disabled: false, readOnly: true,
                    width: textFieldWidth
                },
                {
                    xtype: 'label',
                    text: 'Data Atto'
                },
                {
                    xtype: 'textfield',
                    id: 'dataAttoLiquidazioneID',
                    style: 'opacity:.9',
                    disabled: false, readOnly: true,
                    colspan: 3,
                    width: textFieldWidth
                },
                {
                    xtype: 'label',
                    text: 'Tipo Atto'
                },
                {
                    xtype: 'textfield',
                    id: 'tipoAttoLiquidazioneID',
                    style: 'opacity:.9',
                    disabled: false, readOnly: true,
                    colspan: 3,
                    width: textFieldWidth
                }
            ]
    });


    var panelDetailDatiImpegnoMandato = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 100,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 700,
        layoutConfig: {
            columns: 4
        },
        title: "Impegno n. " + mandato.NumeroImpegno,
        id: 'panelDetailImpegnoMandato',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
                {
                    xtype: 'label',
                    text: 'Numero Atto'
                }, 
                {
                    xtype: 'textfield',
                    id: 'numeroAttoImpegnoID',
                    style: 'opacity:.9',
                    disabled: false, readOnly: true,
                    colspan: 3,
                    width: textFieldWidth
                },
                {
                    xtype: 'label',
                    text: 'Data Atto'
                }, 
                {
                    xtype: 'textfield',
                    id: 'dataAttoImpegnoID',
                    style: 'opacity:.9',
                    disabled: false, readOnly: true,
                    colspan: 3,
                    width: textFieldWidth
                }, 
                {
                    xtype: 'label',
                    text: 'Tipo Atto'
                }, 
                {
                    xtype: 'textfield',
                    id: 'tipoAttoImpegnoID',
                    style: 'opacity:.9',
                    disabled: false, readOnly: true,
                    colspan: 3,
                    width: textFieldWidth
                }
            ]
    });
   
    var tabPanel = new Ext.TabPanel({
        width: 700,
        plain: true,
        height: 280,
        activeTab: 2,
        layoutOnTabChange: true,
        deferredRender: false
    });

    tabPanel.add(panelDetailDatiImpegnoMandato);
    tabPanel.add(panelDetailDatiLiquidazioneMandato);
    tabPanel.add(panelDetailDatiMandato);

    windowDetailMandato.add(tabPanel);

    windowDetailMandato.doLayout();
    windowDetailMandato.show();

    fillViewDatiImpegnoMandato(mandato);
    fillViewDatiLiquidazioneMandato(mandato);
    fillViewDatiMandato(mandato);
}

function fillViewDatiImpegnoMandato(mandato) {
    Ext.getCmp('numeroAttoImpegnoID').setValue(mandato.NumeroAttoImp);
    Ext.getCmp('dataAttoImpegnoID').setValue(mandato.DataAttoImp);
    Ext.getCmp('tipoAttoImpegnoID').setValue(mandato.TipoAttoImp);
}

function fillViewDatiLiquidazioneMandato(mandato) {
    Ext.getCmp('numeroAttoLiquidazioneID').setValue(mandato.NumeroAttoLiq);
    Ext.getCmp('dataAttoLiquidazioneID').setValue(mandato.DataAttoLiq);
    Ext.getCmp('tipoAttoLiquidazioneID').setValue(mandato.TipoAttoLiq);
}

function fillViewDatiMandato(mandato) {
    Ext.getCmp('dataEmissioneMandatoID').setValue(mandato.DataEmissioneMandato);
    Ext.getCmp('importoTotaleMandatoID').setValue(eurRend(mandato.ImportoTotaleMandato));
    Ext.getCmp('oggettoMandatoID').setValue(mandato.OggettoMandato);
    Ext.getCmp('numeroDistintaID').setValue(mandato.NumeroDistinta);
    Ext.getCmp('dataIncaricoID').setValue(mandato.DataIncarico);
    Ext.getCmp('dataQuietanzaID').setValue(mandato.DataQuietanza);
    Ext.getCmp('dataStornoID').setValue(mandato.DataStorno);
}

function showDetailBeneficiario(beneficiario) {
    var windowDetailBeneficiario = new Ext.Window({
        title: "Dettaglio Beneficiario '"+beneficiario.NomeFornitore+"'",
        width: 800,
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
        resizable: false
    });

    var textFieldWidth = 280;
    var reducedTextFieldWidth = 200;

    var panelDetailDatiEconomiciBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 200,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Dati Economici",
        id: 'panelDetailDatiEconomiciBeneficiario',
        cls: 'pannelliDettaglio',
        items: [
            {
                xtype: 'label',
                text: 'Importo Lordo Beneficiario'
            }, {
                xtype: 'textfield',
                id: 'importoLordoBeneficiarioID',
                style: 'opacity:.9;text-align:right',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            }, {
                xtype: 'label',
                text: 'Importo Netto Beneficiario'
            }, {
                xtype: 'textfield',
                id: 'importoNettoBeneficiarioID',
                style: 'opacity:.9;text-align:right;font-weight:bold',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            },
            {
                xtype: 'label',
                text: 'Imponibile Irap'
            }, {
                xtype: 'textfield',
                id: 'imponibileIrapID',
                style: 'opacity:.9;text-align:right',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            }, {
                 xtype: 'label',
                 text: 'Imposta Irap'
             }, {
                 xtype: 'textfield',
                 id: 'impostaIrapID',
                 style: 'opacity:.9;text-align:right',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             }, {
                xtype: 'label',
                text: 'Imponibile Irpef'
            }, {
                xtype: 'textfield',
                id: 'imponibileIrpefID',
                style: 'opacity:.9;text-align:right',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            }, {
                xtype: 'label',
                text: 'Ritenuta Irpef'
            }, {
                xtype: 'textfield',
                id: 'ritenutaIrpefID',
                style: 'opacity:.9;text-align:right',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            }, {
                xtype: 'label',
                text: 'Imponibile Previdenziale'
            }, {
                xtype: 'textfield',
                id: 'imponibilePrevidenzialeID',
                style: 'opacity:.9;text-align:right',
                disabled: false, readOnly: true,
                width: reducedTextFieldWidth
            },  {
                 xtype: 'label',
                 text: 'Ritenute Previdenziali Beneficiario'
             }, {
                 xtype: 'textfield',
                 id: 'ritenutePrevidenzialiBeneficiarioID',
                 style: 'opacity:.9;text-align:right',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             }, {
                 xtype: 'label'
             },
             {
                 xtype: 'label'
             },
              {
                 xtype: 'label',
                 text: 'Ritenute Previdenziali Ente'
             }, {
                 xtype: 'textfield',
                 id: 'ritenutePrevidenzialiEnteID',
                 style: 'opacity:.9;text-align:right',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             }
        ]
    });
    
    var panelDetailAltriDatiBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 200,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Altri Dati",
        id: 'panelDetailAltriDatiBeneficiario',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
             {
                 xtype: 'label',
                 text: 'Info Aggiuntive'
             }, {
                 xtype: 'textfield',
                 id: 'infoAggiuntiveID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             },
             {
                 xtype: 'label',
                 text: 'Codice CIG'
             }, {
                 xtype: 'textfield',
                 id: 'codiceCigID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             }, {
                 xtype: 'label',
                 text: 'Codice CUP'
             }, {
                 xtype: 'textfield',
                 id: 'codiceCupID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 width: reducedTextFieldWidth
             }
        ]
    });
    
    var panelDetailDatiPagamentoBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 200,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Dati Pagamento",
        id: 'panelDetailDatiPagamentoBeneficiario',
        cls: 'pannelliDettaglio',
        items: [
             {
                 xtype: 'label',
                 text: 'Metodo Pagamento'
             }, {
                 xtype: 'textfield',
                 id: 'metodoPagamentoID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 colspan: 3,
                 width: textFieldWidth
             }, {
                 xtype: 'label',
                 text: 'Descrizione Pagamento Tesoriere'
             }, {
                 xtype: 'textfield',
                 id: 'descrizionePagamentoTesoriereID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 colspan: 3,
                 width: textFieldWidth
             }, {
                 xtype: 'label',
                 text: 'Info Pagamento'
             }, {
                 xtype: 'textfield',
                 id: 'infoPagamentoID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 colspan: 3,
                 width: textFieldWidth
             }
        ]
    });

    var panelDetailDatiBancaBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 100,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Dati Bancari",
        id: 'panelDetailDatiBancaBeneficiario',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
            {
                xtype: 'label',
                text: 'Banca'
            }, {
                xtype: 'textarea',
                id: 'bancaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label',
                text: 'Indirizzo'
            }, {
                xtype: 'textfield',
                id: 'indirizzoBancaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label'
            }, {
                xtype: 'textfield',
                id: 'cittaBancaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label'
            }, {
                xtype: 'textfield',
                id: 'nazioneBancaID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label',
                text: 'ABI'
            }, {
                xtype: 'textfield',
                id: 'abiID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label',
                text: 'CAB'
            }, {
                xtype: 'textfield',
                id: 'cabID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                 xtype: 'label',
                 text: 'Conto Corrente'
            }, {
                xtype: 'textfield',
                id: 'contoCorrenteID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label',
                text: 'IBAN'
            }, {
                xtype: 'textfield',
                id: 'ibanID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }
        ]
    });

    var panelDetailBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 200,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Beneficiario",
        id: 'panelDetailBeneficiario',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
            {
                xtype: 'label',
                text: 'Persona'
            }, {
                xtype: 'textfield',
                id: 'statoFornitoreID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Denominazione'
            }, {
                xtype: 'textfield',
                id: 'denominazioneID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Cod.Fisc./Partita Iva'
            }, {
                xtype: 'textfield',
                id: 'partitaIvaFornitoreID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label',
                text: 'Indirizzo'
            }, {
                xtype: 'textfield',
                id: 'indirizzoFornitoreID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            },
            {
                xtype: 'label'
            }, {
                xtype: 'textfield',
                id: 'cittaFornitoreID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }, {
                xtype: 'label'
            }, {
                xtype: 'textfield',
                id: 'nazioneFornitoreID',
                style: 'opacity:.9',
                disabled: false, readOnly: true,
                width: textFieldWidth
            }
        ]
    });


    var panelDetailRappresentanteLegaleBeneficiario = new Ext.Panel({
        xtype: "panel",
        layout: 'table',
        autoHeight: true,
        labelWidth: 200,
        bodyStyle: Ext.isIE ? 'padding:10px 10px 10px 15px;' : 'padding:10px 10px 10px 15px;background: transparent;',
        border: false,
        style: {
            "margin-top": "5px",
            "margin-bottom": "10px",
            "margin-left": "10px", // when you add custom margin in IE 6...
            "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
        },
        width: 750,
        layoutConfig: {
            columns: 2
        },
        title: "Rappresentante Legale",
        id: 'panelDetailRappresentanteLegaleBeneficiario',
        cls: 'pannelliMultiColumnsDettaglio',
        items: [
             {
                 xtype: 'label',
                 text: 'Denominazione'
             }, {
                 xtype: 'textfield',
                 id: 'rappresentanteLegaleID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 width: textFieldWidth
             },
             {
                 xtype: 'label',
                 text: 'Codice Fiscale'
             }, {
                 xtype: 'textfield',
                 id: 'codiceFiscaleRappresentanteLegaleID',
                 style: 'opacity:.9',
                 disabled: false, readOnly: true,
                 width: textFieldWidth
             }
            ]
    });
    
    var tabPanel = new Ext.TabPanel({
        width: 700,
        plain: true,
        height: 300,
        activeTab: 0,
        layoutOnTabChange: true,
       	deferredRender: false
    });

    tabPanel.add(panelDetailBeneficiario);
    if(beneficiario.StatoFornitore=='G')
        tabPanel.add(panelDetailRappresentanteLegaleBeneficiario);
    tabPanel.add(panelDetailDatiPagamentoBeneficiario);
    tabPanel.add(panelDetailDatiBancaBeneficiario);
    tabPanel.add(panelDetailDatiEconomiciBeneficiario);    
    tabPanel.add(panelDetailAltriDatiBeneficiario);

    windowDetailBeneficiario.add(tabPanel);

    windowDetailBeneficiario.doLayout();
    windowDetailBeneficiario.show();

    fillViewDatiBeneficiario(beneficiario);
    fillViewDatiBancaBeneficiario(beneficiario);
    if (beneficiario.StatoFornitore == 'G')
        fillViewRappresentanteLegaleBeneficiario(beneficiario);
    fillViewOtherInfoBeneficiario(beneficiario);    
}

function fillViewDatiBancaBeneficiario(beneficiario) {
    Ext.getCmp('bancaID').setValue(beneficiario.Banca);
    Ext.getCmp('abiID').setValue(beneficiario.ABI);
    Ext.getCmp('cabID').setValue(beneficiario.CAB);
    Ext.getCmp('contoCorrenteID').setValue(beneficiario.ContoCorrente);
    Ext.getCmp('ibanID').setValue(beneficiario.IBAN);
    Ext.getCmp('indirizzoBancaID').setValue(beneficiario.IndirizzoBanca);
    Ext.getCmp('cittaBancaID').setValue(beneficiario.CapBanca + ' - ' + beneficiario.CittaBanca + ' (' + beneficiario.ProvinciaBanca + ')');
    Ext.getCmp('nazioneBancaID').setValue(beneficiario.NazioneBanca);        
}

function fillViewDatiBeneficiario(beneficiario) {
    Ext.getCmp('denominazioneID').setValue(beneficiario.NomeFornitore);
    Ext.getCmp('partitaIvaFornitoreID').setValue(beneficiario.PartitaIvaFornitore);
    Ext.getCmp('indirizzoFornitoreID').setValue(beneficiario.IndirizzoFornitore);
    Ext.getCmp('cittaFornitoreID').setValue(beneficiario.CapFornitore + ' - ' + beneficiario.CittaFornitore + ' (' + beneficiario.ProvinciaFornitore + ')');
    Ext.getCmp('nazioneFornitoreID').setValue(beneficiario.NazioneFornitore);    
    Ext.getCmp('statoFornitoreID').setValue(beneficiario.StatoFornitore=='G' ? 'Giuridica' : 'Fisica');
}

function fillViewRappresentanteLegaleBeneficiario(beneficiario) {
    Ext.getCmp('rappresentanteLegaleID').setValue(beneficiario.RappresentanteLegale);
    Ext.getCmp('codiceFiscaleRappresentanteLegaleID').setValue(beneficiario.CodiceFiscaleRappresentanteLegale);
}

function fillViewOtherInfoBeneficiario(beneficiario) {
    Ext.getCmp('codiceCigID').setValue(beneficiario.CodiceCig);
    Ext.getCmp('codiceCupID').setValue(beneficiario.CodiceCup);
    Ext.getCmp('descrizionePagamentoTesoriereID').setValue(beneficiario.DescrizionePagamentoTesoriere);
    Ext.getCmp('imponibileIrapID').setValue(eurRend(beneficiario.ImponibileIrap));
    Ext.getCmp('imponibileIrpefID').setValue(eurRend(beneficiario.ImponibileIrpef));
    Ext.getCmp('imponibilePrevidenzialeID').setValue(eurRend(beneficiario.ImponibilePrevidenziale));
    Ext.getCmp('importoLordoBeneficiarioID').setValue(eurRend(beneficiario.ImportoLordoBeneficiario));
    Ext.getCmp('importoNettoBeneficiarioID').setValue(eurRend(beneficiario.ImportoNettoBeneficiario));
    Ext.getCmp('impostaIrapID').setValue(eurRend(beneficiario.ImpostaIrap));
    Ext.getCmp('infoAggiuntiveID').setValue(beneficiario.InfoAggiuntive);
    Ext.getCmp('infoPagamentoID').setValue(beneficiario.InfoPagamento);
    Ext.getCmp('metodoPagamentoID').setValue(beneficiario.MetodoPagamento); 
    Ext.getCmp('ritenutaIrpefID').setValue(eurRend(beneficiario.RitenutaIrpef));
    Ext.getCmp('ritenutePrevidenzialiBeneficiarioID').setValue(eurRend(beneficiario.RitenutePrevidenzialiBeneficiario));
    Ext.getCmp('ritenutePrevidenzialiEnteID').setValue(eurRend(beneficiario.RitenutePrevidenzialiEnte)); 
}