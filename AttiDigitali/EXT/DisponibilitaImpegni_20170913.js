var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni"
var myPanelLiq = new Ext.FormPanel({
    id:'myPanelLiq',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Disponibilita Impegni"
});

var labelNumImp = new Ext.form.Label({
						  text: 'Numero Impegno: ',
						  id: 'labelNumImp' 
						 });   
						  
var ComboImp = new Ext.form.ComboBox({
       fieldLabel: 'NumeroImpegno',
       name: 'ComboImp',
       id: 'ComboImp',
       width: 150,
       itemCls: "titvar",
       listWidth: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Impegni ...',
       displayField: 'NumImpegno',
       valueField:'NumImpegno',
       mode: 'local'
        

});

//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DELL'IMPEGNO
var panelImp =  new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit", 
								items : [
									labelNumImp, 
									ComboImp
								]
								});


function InitFormCapitoli(mostraAnno, codice_Ufficio) {

var AnnoBilancio= new Ext.ux.YearMenu({
   	  			    format: 'Y',
			        id: 'AnnoBilancio',
			        allowBlank: false,
			        noPastYears: false,
			        noFutureYears: false,
			        noPastMonths: true,
			        minDate: new Date('1990/1/1'),
			        maxDate: new Date('2100/1/1')
			       ,
			        handler : function(dp, newValue, oldValue){
			         	Ext.getCmp('AnnoIni').value= newValue.format('Y');
           		 		Ext.get('AnnoIni').dom.value= newValue.format('Y');
           		 		}
   			        });
var btnRicerca = new Ext.Button({
						  text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
						    id: 'btnBilancio'
						});

						btnRicerca.on('click', function() {

						    var fNewAnno = Ext.getCmp("AnnoIni").value;
						    //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
						    gridCapitoli = buildGridStart(fNewAnno,codice_Ufficio);
						    capitoloPanel.remove('GridCapitoli')
						    capitoloPanel.add(gridCapitoli);
						    capitoloPanel.doLayout();
						});

var tbarDate = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancio},
        			 {
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
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 					
	 
 //DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanel = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{layout : "fit"
								
								}]
					});
					
labelNumImp = new Ext.form.Label({
						  text: 'Numero Impegno: ',
						  id: 'labelNumImp' 
						 });   
	ComboImp = new Ext.form.ComboBox({
       fieldLabel: 'NumeroImpegno',
       name: 'ComboImp',
       id: 'ComboImp',
       width: 150,
       itemCls: "titvar",
       listWidth: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Impegni ...',
       displayField: 'NumImpegno',
       valueField:'NumImpegno',
        mode:'local'

});

panelImp =  new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit", 
								items : [
									labelNumImp, 
									ComboImp
								]
								}); 
 
 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
 var formCapitoli = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per liquidazione',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 800,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDate
        		],
            items: [capitoloPanel]
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
            	panelImp
            	            
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
                width: 170,
                 height:50,
                fieldLabel: 'Descrizione',
                name: 'DescrCapitolo',
                id: 'DescrCapitolo',
                readOnly: true
            }
            , {
                xtype: 'textarea',
                width: 170,
                 height:50,
                fieldLabel: 'Descrizione Impegno',
                name: 'Oggetto_Impegno',
                id: 'Oggetto_Impegno',
                readOnly: true
            }
            ,{
                fieldLabel: 'Disponibilit&agrave; Impegno',
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

          
            },{
                fieldLabel: 'Tipo Atto',
                name: 'TipoAtto',
                id: 'TipoAtto',
                readOnly: true
            },
            {
                fieldLabel: 'Numero Atto',
                name: 'NumeroAtto',
                id: 'NumeroAtto',
                readOnly: true
            },
            {
                fieldLabel: 'Data Atto',
                name: 'DataAtto',
                id: 'DataAtto',
                readOnly: true
            }]
        }]        
    });


    if (mostraAnno == false) {
        tbarDate.hide();
    } else { tbarDate.show(); }
    


 //PRENDO L'ANNO DALLA DATA ODIERNA
 		var dtOggi=new Date()
		dtOggi.setDate(dtOggi.getDate());
		var fAnno = dtOggi.getFullYear();
		
		//RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
	 	var gridCapitoli = buildGridStart(fAnno,codice_Ufficio);
	    capitoloPanel.add(gridCapitoli);
		// SETTO panelImp INVISIBILE 
        panelImp.hide();

Ext.getCmp('ElencoCapitoli').render("Lista")

//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
	 var formPanelDropTargetEl = formCapitoli.body.dom;
	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
	      ddGroup: 'gridDDGroup',
	      notifyEnter: function(ddSource, e, data) {
	      //EFFETTI VISIVI SUL DRAG & DROP
	      formCapitoli.body.stopFx();
	      formCapitoli.body.highlight();
	      },
	      notifyDrop: function(ddSource, e, data) {
	          resetForm();
	          // CATTURO IL RECORD SELEZIONATO
	          var selectedRecord = ddSource.dragData.selections[0];
	          // RENDO VISIBILE panelImp
		      buildComboImp(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
		      // CARICO IL RECORD NELLA FORM
	          formCapitoli.getForm().loadRecord(selectedRecord);
	          Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
	          // RENDO VISIBILE panelImp
	          buildComboImp(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
	          panelImp.doLayout();
		      panelImp.show();
	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
	          // CANCELLO IL RECORD DALLA GRIGLIA.
	          //ddSource.grid.store.remove(selectedRecord);
	          return (true);
	      }
	  }); 

}

function buildComboImp(AnnoRif, CapitoloRif) {

     panelImp.remove(ComboImp);
    
     var proxy = new Ext.data.HttpProxy({
     url: 'ProcAmm.svc/GetListaImp',
        method: 'GET',
        timeout: 900000
     });
     var reader = new Ext.data.JsonReader({

        root: 'GetListaImpResult',
         fields: [
           { name: 'NumImpegno' }   ,
           { name: 'ImpDisp' }   ,
           { name: 'TipoAtto' }   ,
           { name: 'DataAtto' }   ,
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' },
           { name: 'Codice_Obbiettivo_Gestionale' }
           
           ]
     });

     var store = new Ext.data.Store({
         proxy: proxy
  , reader: reader
     });

     //store.setDefaultSort("NumImpegno", "DESC");
     store.setDefaultSort("NumImpegno", "ASC");

  var ufficio = ''+ Ext.getCmp('ComboUffici').value
 
   if (ufficio==''){
     ufficio=Ext.get('Cod_uff_Prop').dom.value;
   }
 
     //var parametri = { CodiceUfficio: ufficio };
  

     store.on({
         'load': {
             fn: function(store, records, options) {
                 mask.hide();
             },
             scope: this
         }
     });

       // mask.show();                           
	var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif, CodiceUfficio: ufficio };

     //var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif};
   try{
     store.load({ params: parametri });
     }
     catch(ex){
      mask.hide();
    }
    // store.sort();
    ComboImp=  new Ext.form.ComboBox({
    fieldLabel: 'NumeroImpegno',
       name: 'ComboImp',
       id: 'ComboImp',
       width: 150,
       itemCls: "titvar",
       triggerAction: 'all',
       emptyText: 'Seleziona Impegni ...',
       displayField: 'NumImpegno',
       valueField:'NumImpegno',
       store: store,
       mode:'local',
       readOnly:true
    });
    
    panelImp.add(ComboImp);
    
    ComboImp.show();
    panelImp.show();
    panelImp.doLayout();
    ComboImp.on('select', function(record, index) { VerificaDisponibilitaLiquidazione() });
    return ComboImp;


}

function resetForm() {
    Ext.get('ImpDisp').dom.value = '';
    Ext.get('Bilancio').dom.value = '';
    Ext.get('UPB').dom.value = '';
    Ext.get('MissioneProgramma').dom.value = '';
    Ext.get('Capitolo').dom.value = '';
    Ext.get('TipoAtto').dom.value = '';
    Ext.get('NumeroAtto').dom.value = '';
    Ext.get('DataAtto').dom.value = '';
    Ext.get('Oggetto_Impegno').dom.value = '';
    Ext.get('ImpPotenzialePrenotato').dom.value = '';
    


}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER Liquidazioni"
function buildGridStart(annoRif, codice_Ufficio) {
 var maskApp;
maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
	var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetElencoCapitoliAnno',
    method:'GET',
    timeout: 900000
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetElencoCapitoliAnnoResult',
	fields: [
           {name: 'Bilancio'},
           {name: 'UPB' },
           {name: 'MissioneProgramma' },
           {name: 'Capitolo'},
           {name: 'DescrCapitolo'},
           { name: 'ImpDisp' },
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
 
 var parametri = { AnnoRif: annoRif, tipoCapitolo: '2', codiceUfficio: codice_Ufficio};
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
	                { header: "Descrizione", width: 180, dataIndex: 'DescrCapitolo',  sortable: true, locked:false},
	             	{ renderer: eurRend, header: "Importo<br/>Disponibile", width: 60, dataIndex: 'ImpDisp', id: 'ImpDisp', sortable: true, hidden: true },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	             	] );
	            	
 	  var grid = new Ext.grid.GridPanel({
	  			  id:'GridCapitoli',
	              autoExpandColumn: 'Capitolo',
		          // da mettere sempre 
		          height: 350,
	              title:'',
	              border: true,
			      viewConfig : { forceFit : true },	  			  
		          ds: store,
		          cm: ColumnModel,
		          stripeRows: true,
               // istruzioni per abilitazione Drag & Drop		          
		          enableDragDrop: true,
		          ddGroup: 'gridDDGroup',
		       // fine istruzioni per abilitazione Drag & Drop		          
	          	  sm: new Ext.grid.RowSelectionModel({
	              singleSelect: true,
	              loadMask: false,
	              listeners: {
	                    rowselect: function(sm, row, rec) {
	                    // commentare if
	                    	 //	if (rec.data['ImpDisp']=="0") 
	                    		//	{
	                    			//var capImpDisp=rec.data['Capitolo'];
	                    			//var upbImpDisp=rec.data['UPB'];
	                    			//var bilancioImpDisp=rec.data['Bilancio'];
 							        //ImportoDisp = getImpDisp(rec,capImpDisp, bilancioImpDisp);
 							       // var valorediRif = rec.data['ImpDisp'];
 							       // Ext.getCmp('ImpPrenotato').maxValue=parseFloat(valorediRif.replace(",","."));
	                    		    
	                    		//	}
	                   	 		}	
	                		  }
	           		 })
		          });
	    
//AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
		          grid.addListener({
		              'rowdblclick': {
		                  fn: function(grid, rowIndex, event) {
		                      resetForm();
		                      var rec = grid.store.getAt(rowIndex);
		                      Ext.getCmp("ElencoCapitoli").getForm().loadRecord(rec);
		                      // RENDO VISIBILE panelImp
		                      Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
		                      buildComboImp(rec.data['Bilancio'], rec.data['Capitolo']);

		                      panelImp.doLayout();
		                      panelImp.show();
		                  }
		, scope: this
		              }
		          });
 
	return grid;
}

function VerificaDisponibilitaLiquidazione() {
    try {
        Ext.get('DataAtto').dom.value = ComboImp.store.data.get(ComboImp.selectedIndex).data.DataAtto;
        Ext.get('TipoAtto').dom.value = ComboImp.store.data.get(ComboImp.selectedIndex).data.TipoAtto;
        Ext.get('NumeroAtto').dom.value = ComboImp.store.data.get(ComboImp.selectedIndex).data.NumeroAtto;
        Ext.get('ImpDisp').dom.value = eurRend(ComboImp.store.data.get(ComboImp.selectedIndex).data.ImpDisp);
        Ext.get('Oggetto_Impegno').dom.value = ComboImp.store.data.get(ComboImp.selectedIndex).data.Oggetto_Impegno;
        Ext.get('ImpPotenzialePrenotato').dom.value = eurRend(ComboImp.store.data.get(ComboImp.selectedIndex).data.ImpPotenzialePrenotato);
 
   
    } catch (ex) { 
    }
}