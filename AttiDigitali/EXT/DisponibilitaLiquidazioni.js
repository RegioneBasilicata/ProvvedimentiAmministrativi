//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanelRiduzioniLiq = new Ext.FormPanel({
    id:'myPanelRiduzioniLiq',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Riduzioni Liquidazioni registrate"
});

var labelNumLiqRid = new Ext.form.Label({
						  text: 'Numero Liquidazione: ',
						  id: 'labelNumLiqRid' 
						 });    


  
var ComboLiqRid = new Ext.form.ComboBox({
fieldLabel: 'NLiquidazione',
displayField: 'NLiquidazione',
valueField: 'NLiquidazione',
id: 'ComboLiqRid',
       name: 'ComboLiqRid',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Liquidazione ...',
        mode:'local'

});
  
    
//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DELLA LIQUIDAZIONE
var panelLiqRid = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumLiqRid,
									ComboLiqRid
								 ]
								}); 
								
//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliRiduzioneLiq(mostraAnno) {
  var AnnoBilancioRiduzioneLiq= new Ext.ux.YearMenu({
   	  			    format: 'Y',
			        id: 'AnnoBilancio',
			        allowBlank: false,
			        noPastYears: false,
			        noPastMonths: true,
			        minDate: new Date('1990/1/1'),
			        maxDate: new Date()
			       ,
			        handler : function(dp, newValue, oldValue){
			         	Ext.getCmp('AnnoIni').value= newValue.format('Y');
           		 		Ext.get('AnnoIni').dom.value= newValue.format('Y');
           		 		}
   			        });
   			        var btnRicercaRiduzioneLiq = new Ext.Button({
   			            text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
   			            id: 'btnBilancio'
   			        });

   			        btnRicercaRiduzioneLiq.on('click', function() {
   			      
   			            var fNewAnno = Ext.getCmp("AnnoIni").value;
   			            //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
   			            gridCapitoli = buildGridStartRiduzioneLiq(fNewAnno);
   			            capitoloPanelRiduzioneLiq.remove('GridCapitoli')
   			            capitoloPanelRiduzioneLiq.add(gridCapitoli);
   			            capitoloPanelRiduzioneLiq.doLayout();
   			        });

 var tbarDateRiduzioneLiq = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancioRiduzioneLiq},
        			 {
			        xtype: 'textfield',
			       	cls: 'titfis',
			        readOnly: true,
			        id: 'AnnoIni',
			        width: 80,
 			        value: new Date().format('Y')
			    },
			    btnRicercaRiduzioneLiq
			 ]
	});
	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelRiduzioneLiq = new Ext.Panel({
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 	
						
	labelNumLiqRid = new Ext.form.Label({
		text: 'Numero Liquidazione: ',
		id: 'labelNumLiqRid' 
	});    

    ComboLiqRid = new Ext.form.ComboBox({
        fieldLabel: 'NLiquidazione',
        displayField: 'NLiquidazione',
        valueField: 'NLiquidazione',
        id: 'ComboLiqRid',
        name: 'ComboLiqRid',
        mode: 'local',
        listWidth: 150,
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Liquidazione ...'
    });
     
 
    panelLiqRid = new Ext.Panel({
				xtype : "panel",
				title : "",
				width:300,
				buttonAlign: "center",
				autoHeight:true,
				layout: "fit",
				items: [
					labelNumLiqRid,
					ComboLiqRid
				 ]
	}); 

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanelRiduzioneLiq = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{
								    layout : "fit"
								}]
					}); 
 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
 var formCapitoliRiduzioneLiq = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per riduzione liquidazione',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 800,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDateRiduzioneLiq
        		],
            items: [capitoloPanelRiduzioneLiq]
            },
            {
        	columnWidth: 0.4,
            xtype: 'fieldset',
            id: 'detCapitolo',
            labelWidth: 90,
            title:'Dettaglio',
            defaultType: 'textfield',
            autoHeight: true,
            bodyStyle: Ext.isIE ? 'padding:0 0 5px 15px;' : 'padding:10px 15px;',
            border: false,
            style: {
                "margin-left": "10px", // when you add custom margin in IE 6...
                "margin-right": Ext.isIE6 ? (Ext.isStrict ? "-10px" : "-13px") : "0"  // you have to adjust for it somewhere else
            },
            items: [
            	panelLiqRid
            ,
            	{
                fieldLabel: 'Bilancio',
                
                name: 'Bilancio',
                id: 'Bilancio',
                readOnly: true
            },{
                fieldLabel: 'UPB',
                name: 'UPB',
                id: 'UPB',
                readOnly: true
            }, {
                fieldLabel: 'Missione. Programma',
                name: 'MissioneProgramma',
                id: 'MissioneProgramma',
                readOnly: true
            }, {
                fieldLabel: 'Capitolo',
                name: 'Capitolo',
                id: 'Capitolo',
                readOnly: true
            },{
                xtype: 'textarea',
                width: 180,
                 height:50,
                fieldLabel: 'Descrizione',
                name: 'DescrCapitolo',
                id: 'DescrCapitolo',
                readOnly: true
            }, {
                xtype: 'textarea',
                width: 180,
                 height:50,
                fieldLabel: 'Descrizione Liquidazione',
                name: 'Oggetto_Impegno',
                id: 'Oggetto_Impegno',
                readOnly: true
            },{
                fieldLabel: 'Disponibilit&agrave;',
                name: 'ImpDisp',
                id: 'ImpDisp',
                readOnly: true
            },{
                fieldLabel: 'Importo Potenziale',
                name: 'ImpPotenzialePrenotato',
                id: 'ImpPotenzialePrenotato',
                readOnly: true,
                msgTarget  : 'qtip',       
                  listeners: {
                    render: function(c) {
                      Ext.QuickTips.register({
                        target: c.getEl(),
                        text: 'Importo al netto di eventuali operazioni contabili non ancora registrate (Liquidazioni/Riduzioni)'
                      });
                    }
                  }           
            },
            buttonPanelRiduzioneLiq]
        }]        
    });

    if (mostraAnno == false) {
        tbarDateRiduzioneLiq.hide();
    } else { tbarDateRiduzioneLiq.show(); }
    
    
//PRENDO L'ANNO DALLA DATA ODIERNA
 		var dtOggi=new Date()
		dtOggi.setDate(dtOggi.getDate());
		var fAnno = dtOggi.getFullYear();
		
		//RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
	 	var gridCapitoli = buildGridStartRiduzioneLiq(fAnno);
	    capitoloPanelRiduzioneLiq.add(gridCapitoli);
		panelLiqRid.hide();
        
Ext.getCmp('ElencoCapitoli').render("Lista")
//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
	 var formPanelDropTargetEl = formCapitoliRiduzioneLiq.body.dom;
	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
	      ddGroup: 'gridDDGroup',
	      notifyEnter: function(ddSource, e, data) {
	      //EFFETTI VISIVI SUL DRAG & DROP
	      formCapitoliRiduzioneLiq.body.stopFx();
	      formCapitoliRiduzioneLiq.body.highlight();
	      },
	      notifyDrop: function(ddSource, e, data) {
	          // CATTURO IL RECORD SELEZIONATO
	          var selectedRecord = ddSource.dragData.selections[0];
	          // CARICO IL RECORD NELLA FORM
	          formCapitoliRiduzioneLiq.getForm().loadRecord(selectedRecord);
	          // RENDO VISIBILE panelImp
	           buildComboLiqRid(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
	          //LU aggiunta formattazione imp disp 
	         if(Ext.get('ImpDisp').dom.value == "€ NaN,00"){
	             Ext.get('ImpDisp').dom.value = eurRend("0,00");
	         }else{
	            Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
	         }    
	          //LU Fine  aggiunta formattazione imp disp 
	          buildComboLiqRid(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
	          panelLiqRid.doLayout();
		      panelLiqRid.show();
	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
	          // CANCELLO IL RECORD DALLA GRIGLIA.
	          //ddSource.grid.store.remove(selectedRecord);
	          return (true);
	      }
	  }); 



 
}// fine InitFormCapitoli


function buildComboLiqRid(AnnoRif, CapitoloRif) {
    panelLiqRid.remove(ComboLiqRid);
  
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaLiquidazioniAperte',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

      root: 'GetListaLiquidazioniAperteResult',
         fields: [
           { name: 'NLiquidazione' }   ,
           { name: 'ImpDisp' }  ,
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("NLiquidazione", "ASC");

    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
    store.load({ params: parametri });

    store.on({
        'load': {
            fn: function(store, records, options) {
                //maskApp.hide();
            },
            scope: this
        }
    });


  ComboLiqRid=  new Ext.form.ComboBox({
        fieldLabel: 'NLiquidazione',
        displayField: 'NLiquidazione',
        name: 'ComboLiqRid',
        id: 'ComboLiqRid',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Liquidazioni ...'

    });
    
    panelLiqRid.add(ComboLiqRid);
    
    ComboLiqRid.show();

    ComboLiqRid.on('select', function(record, index) { VerificaDisponibilitaRiduzioneLiq() });
    return ComboLiqRid;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartRiduzioneLiq(annoRif) {
 var maskApp;
maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
    var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetElencoCapitoliAnnoLiquidazione',
    method:'GET',
    timeout: 900000
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetElencoCapitoliAnnoLiquidazioneResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'ImpDisp'},
           {name: 'NumPrenotazione'},
           {name: 'AnnoPrenotazione'},
           { name: 'DescrCapitolo' },
           { name: 'CodiceRisposta' },
           { name: 'DescrizioneRisposta' }           
           ]
	});
	
	var store = new Ext.data.Store({
		proxy:proxy
		,reader:reader
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
   'load':{
      fn: function(store, records, options){
       maskApp.hide();
     },
      scope:this
  	 }
 	});
 	store.on("loadexception",maskApp.hide());
 
   var ufficio = ''+ Ext.getCmp('ComboUffici').value

   if (ufficio==''){
     ufficio=Ext.get('Cod_uff_Prop').dom.value;
   }
 
     //var parametri = { CodiceUfficio: ufficio };
                                     
	var parametri = { AnnoRif: annoRif, tipoCapitolo: '2', CodiceUfficio: ufficio };
	
	
	maskApp.show();
    store.load({params:parametri});

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
	                { header: "Bilancio", width: 60,dataIndex: 'Bilancio',  id: 'Bilancio',  sortable: true },
	                { header: "Capitolo", width: 60,dataIndex: 'Capitolo',  sortable: true, locked:false},
                    { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                    { header: "Missione.Programma", width: 60, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width: 180, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	            	] );
 	  var grid = new Ext.grid.GridPanel({
	  			  id:'GridCapitoliRid',
	              autoExpandColumn: 'Capitolo',
		          // da mettere sempre 
		          height: 350,
	              title:'',
	              border: true,
			      viewConfig : { forceFit : true },	  			  
		          ds: store,
		          cm: ColumnModel,
		          stripeRows: true,
		          loadMask: true,
               // istruzioni per abilitazione Drag & Drop		          
		          enableDragDrop: true,
		          ddGroup: 'gridDDGroup',
		       // fine istruzioni per abilitazione Drag & Drop		          
	          	  sm: new Ext.grid.RowSelectionModel({
	              singleSelect: true
	           		 })
		          });
	    
//AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
		          grid.addListener({
		              'rowdblclick': {
		                  fn: function(grid, rowIndex, event) {
		                      var rec = grid.store.getAt(rowIndex);
		                      Ext.getCmp("ElencoCapitoli").getForm().loadRecord(rec);
		                      //LU aggiunta formattazione imp disp
		                      Ext.get('ImpDisp').dom.value = eurRend(0)
		                      //LU Fine  aggiunta formattazione imp disp 
		                      buildComboLiqRid(rec.data['Bilancio'], rec.data['Capitolo']);
		                      panelLiqRid.doLayout();
		                      panelLiqRid.show();
		                 }
		, scope: this
		              }
		          });
	return grid;
}

function VerificaDisponibilitaRiduzioneLiq() {
    try {
        Ext.get('ImpDisp').dom.value = ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.ImpDisp;
        //LU aggiunta formattazione imp disp 
        Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
        //LU Fine  aggiunta formattazione imp disp 
        Ext.get('Oggetto_Impegno').dom.value = ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(ComboLiqRid.store.data.get(ComboLiqRid.selectedIndex).data.ImpPotenzialePrenotato);


    } catch (ex) { 
    }
}
