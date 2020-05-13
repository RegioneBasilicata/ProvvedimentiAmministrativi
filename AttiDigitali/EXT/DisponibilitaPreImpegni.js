

//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "CAPITOLI SELEZIONATI"
var myPanel = new Ext.FormPanel({
    id:'myPanel',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
	xtype: "form",
	title : "Disponibilita PreImpegni"
});
var labelNumPreImp = new Ext.form.Label({
						  text: 'Numero Prenotazione PreImpegno: ',
						  id: 'labelNumPreImp' 
						 });    


  
var ComboPreimp = new Ext.form.ComboBox({
fieldLabel: 'NumeroPreImpegno',
displayField: 'NumPreImp',
valueField: 'NumPreImp',
id: 'ComboPreimp',
       name: 'ComboPreimp',
       listWidth: 150,
       width: 150,
       triggerAction: 'all',
       emptyText: 'Seleziona Preimpegni ...',
        mode:'local'

});
     
     


//DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE VISUALIZZA LA DISPONIBILITA' DEL PREIMPEGNO
var panelPreImp = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								autoHeight:true,
								layout: "fit",
								items: [
									labelNumPreImp,
									ComboPreimp
								 ]
								}); 
								

//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliImpegno(mostraAnno, codice_Ufficio ) {

var AnnoBilancioImpegno= new Ext.ux.YearMenu({
   	  			    format: 'Y',
			        id: 'AnnoBilancio',
			        allowBlank: false,
			        noPastYears: false,
			        noPastMonths: true,
			        minDate: new Date('1990/1/1'),
			        maxDate: new Date('2069/1/1')
			       ,
			        handler : function(dp, newValue, oldValue){
			         	Ext.getCmp('AnnoIni').value= newValue.format('Y');
           		 		Ext.get('AnnoIni').dom.value= newValue.format('Y');
           		 		}
   			        });
   			        var btnRicercaImpegno = new Ext.Button({
   			            text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
   			            id: 'btnBilancio'
   			        });

   			        btnRicercaImpegno.on('click', function() {
   			      
   			            var fNewAnno = Ext.getCmp("AnnoIni").value;
   			            //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
   			            gridCapitoli = buildGridStartImpegno(fNewAnno);
   			            capitoloPanelImpegno.remove('GridCapitoli')
   			            capitoloPanelImpegno.add(gridCapitoli);
   			            capitoloPanelImpegno.doLayout();
   			        });

 var tbarDateImpegno = new Ext.Toolbar({
            style:        'margin-bottom:-1px;',
            width:700,
            items: [{xtype: 'button',
       				 text: "<font color='#0000A0'><b>Bilancio</b></font>",
       				 id: 'SelBil',
        			 menu: AnnoBilancioImpegno},
        			 {
			        xtype: 'textfield',
			       	cls: 'titfis',
			        readOnly: true,
			        id: 'AnnoIni',
			        width: 80,
 			        value: new Date().format('Y')
			    },
			    btnRicercaImpegno
			 ]
	});
	
	//DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
 var capitoloPanelImpegno = new Ext.Panel({
								xtype : "panel",
								title : "",
								autoHeight:true
				}); 			
						
	labelNumPreImp = new Ext.form.Label({
		text: 'Numero Prenotazione PreImpegno: ',
		id: 'labelNumPreImp' 
	});    

    ComboPreimp = new Ext.form.ComboBox({
        fieldLabel: 'NumeroPreImpegno',
        displayField: 'NumPreImp',
        valueField: 'NumPreImp',
        id: 'ComboPreimp',
        name: 'ComboPreimp',
        mode: 'local',
        listWidth: 150,
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Preimpegni ...'
    });
     
 
    panelPreImp = new Ext.Panel({
				xtype : "panel",
				title : "",
				width:300,
				buttonAlign: "center",
				autoHeight:true,
				layout: "fit",
				items: [
					labelNumPreImp,
					ComboPreimp
				 ]
	}); 

 
 //DEFINISCO IL PANNELLO CONTENENTE IL BOTTONE CHE AGGIUNGE L'IMPEGNO
 var buttonPanelImpegno = new Ext.Panel({
								xtype : "panel",
								title : "",
								width:300,
								buttonAlign: "center",
								items : [{
								    layout : "fit"
								}]
					}); 
 //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
 var formCapitoliImpegno = new Ext.FormPanel({
        id: 'ElencoCapitoli',
        frame: true,
        labelAlign: 'left',
        title: 'Elenco Capitoli per impegno',
        bodyStyle:'padding:5px',
        collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
        width: 800,
        layout: 'column',	// SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
        items: [{
            columnWidth: 0.6,
            layout: 'fit',
            tbar: [tbarDateImpegno],
            items: [capitoloPanelImpegno]
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
            	panelPreImp
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
                fieldLabel: 'Descrizione PreImpegno',
                name: 'Oggetto_Impegno',
                id: 'Oggetto_Impegno',
                readOnly: true
            }
            ,{
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
            } ]  
         }]
    });

    if (mostraAnno == false) {
        tbarDateImpegno.hide();
    } else { tbarDateImpegno.show(); }

 //PRENDO L'ANNO DALLA DATA ODIERNA
 		var dtOggi=new Date()
		dtOggi.setDate(dtOggi.getDate());
		var fAnno = dtOggi.getFullYear();
		
		//RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
	 	var gridCapitoli = buildGridStartImpegno(fAnno,codice_Ufficio);
	    capitoloPanelImpegno.add(gridCapitoli);
		// SETTO panelImp INVISIBILE 
        panelPreImp.hide();
        
Ext.getCmp('ElencoCapitoli').render("Lista")

//GESTISCO IL DRAG & DROP (CHE DIVENTA COPY & DROP) DALLA GRIGLIA ALLA FORM EDITABILE
	 var formPanelDropTargetEl = formCapitoliImpegno.body.dom;
	 var formPanelDropTarget = new Ext.dd.DropTarget(formPanelDropTargetEl, {
	      ddGroup: 'gridDDGroup',
	      notifyEnter: function(ddSource, e, data) {
	      //EFFETTI VISIVI SUL DRAG & DROP
	      formCapitoliImpegno.body.stopFx();
	      formCapitoliImpegno.body.highlight();
	      },
	      notifyDrop: function(ddSource, e, data) {
	         resetForm();
	          // CATTURO IL RECORD SELEZIONATO
	          var selectedRecord = ddSource.dragData.selections[0];
	           buildComboPreImp(selectedRecord.data['Bilancio'], selectedRecord.data['Capitolo']);
	          // CARICO IL RECORD NELLA FORM
	          formCapitoliImpegno.getForm().loadRecord(selectedRecord);
	          // RENDO VISIBILE panelImp
	            Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)
	          buildComboPreImp(selectedRecord.data.Bilancio,selectedRecord.data.Capitolo);
	          panelPreImp.doLayout();
		      panelPreImp.show();
	          // ISTRUZIONI PER IL DRAG --- ASTERISCATE
	          // CANCELLO IL RECORD DALLA GRIGLIA.
	          //ddSource.grid.store.remove(selectedRecord);
	          return (true);
	      }
	  }); 


 
}// fine InitFormCapitoli


function buildComboPreImp(AnnoRif, CapitoloRif) {
    panelPreImp.remove(ComboPreimp);
    var maskApp;
maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
  


    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetListaPreimp',
        method: 'GET',
    timeout: 900000
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetListaPreimpResult',
        fields: [
           { name: 'NumPreImp' },
           { name: 'ImpDisp' }  ,
           { name: 'TipoAtto' } ,
           { name: 'DataAtto' } ,
           { name: 'NumeroAtto' },
           { name: 'Oggetto_Impegno' },
           { name: 'ImpPotenzialePrenotato' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });
    
    store.setDefaultSort("NumPreImp", "ASC");


    var parametri = { AnnoRif: AnnoRif, CapitoloRif: CapitoloRif };
    store.load({ params: parametri });

    store.on({
        'load': {
            fn: function(store, records, options) {
                maskApp.hide();
            },
            scope: this
        }
    });


  ComboPreimp=  new Ext.form.ComboBox({
        fieldLabel: 'NumeroPreImpegno',
        displayField: 'NumPreImp',
        name: 'ComboPreimp',
        id: 'ComboPreimp',
        store: store,
        listWidth: 150,
        readOnly:true,
        mode: 'local',
        width: 150,
        triggerAction: 'all',
        emptyText: 'Seleziona Preimpegni ...'

    });
    
    panelPreImp.add(ComboPreimp);
    
    ComboPreimp.show();

    ComboPreimp.on('select', function(record, index) { VerificaDisponibilita() });
    return ComboPreimp;

}
//FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
function buildGridStartImpegno(annoRif, codice_Ufficio) {

var maskApp;
maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
	var proxy = new Ext.data.HttpProxy({
	url: 'ProcAmm.svc/GetElencoCapitoliAnno',
    method:'GET'
    ,
    timeout: 900000
    }); 
	var reader = new Ext.data.JsonReader({

	root: 'GetElencoCapitoliAnnoResult',
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


	var parametri = { AnnoRif: annoRif, tipoCapitolo:'0' , codiceUfficio: codice_Ufficio};
    store.load({params:parametri});

    store.on({
   'load':{
      fn: function(store, records, options){
       maskApp.hide();
     },
      scope:this
  	 }
 	});
 	maskApp.show();

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
		          loadMask: true,
		          ddGroup: 'gridDDGroup',
		       // fine istruzioni per abilitazione Drag & Drop		          
	          	  sm: new Ext.grid.RowSelectionModel({
	              singleSelect: true,
	              listeners: {
	                    rowselect: function(sm, row, rec) {
	                    // commentare if
	                    	 //	if (rec.data['ImpDisp']=="0") 
	                    		//	{
	                    			var capImpDisp=rec.data['Capitolo'];
	                    			//var upbImpDisp=rec.data['UPB'];
	                    			var bilancioImpDisp=rec.data['Bilancio'];
 							  //      var ImportoDisp = getImpDispPreImpegno(rec,capImpDisp, bilancioImpDisp);
	                    		//	}
	                   	 		}	
	                		  }
	           		 })
		          });
	    
//AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
		          grid.addListener({
		              'rowdblclick': {
		              fn: function(grid, rowIndex, event) {
		              resetForm()
		                      var rec = grid.store.getAt(rowIndex);
		                 //     if (rec.data['ImpDisp'] != "0") {
		                          Ext.getCmp("ElencoCapitoli").getForm().loadRecord(rec);

		                          Ext.get('TipoAtto').dom.value = '';
		                          Ext.get('NumeroAtto').dom.value = '';
		                          Ext.get('DataAtto').dom.value = '';
		                          Ext.get('ImpDisp').dom.value = eurRend(Ext.get('ImpDisp').dom.value)              
		                          buildComboPreImp(rec.data['Bilancio'], rec.data['Capitolo']);

		                          panelPreImp.doLayout();
		                          panelPreImp.show();
		        
		                   //   }
		                  }
		, scope: this
		              }
		          });
	return grid;
}

function VerificaDisponibilita() {
    try {
        var num = Ext.get('ComboPreimp').dom.value; 
        getImpDispPreImpegno(num);
    } catch (ex) { 
    }
}
function resetForm() {
    Ext.get('ImpDisp').dom.value = ''
    Ext.get('Bilancio').dom.value = ''
    Ext.get('UPB').dom.value = ''
    Ext.get('MissioneProgramma').dom.value = ''
    Ext.get('Capitolo').dom.value = ''
    Ext.get('TipoAtto').dom.value = ''
    Ext.get('NumeroAtto').dom.value = ''
    Ext.get('DataAtto').dom.value = ''
    Ext.get('Oggetto_Impegno').dom.value = ''
    Ext.get('ImpPotenzialePrenotato').dom.value = ''

}
//FUNZIONE CHE RIEMPIE IL CAMPO "DISPONIBILITA' PREIMPEGNO" NEL PANEL "NUMERO PREIMPEGNO"
function getImpDispPreImpegno(NumPreImpegno) {
	var params = { NumeroPreImpegno: NumPreImpegno};
 
   Ext.lib.Ajax.defaultPostHeader = 'application/json';
   Ext.Ajax.request({
         url: 'ProcAmm.svc/GetImpDispPreImpegno',
        params: Ext.encode(params),
        method: 'POST',
        success: function(response, options) {
        
            var data = Ext.decode(response.responseText);
            //data.GetImpDispPreImpegnoResult.ImportoDisponibile;
            // nonostante impDisp sia lo stesso nome dato alla colonna della griglia, viene modificato solo nel Dettaglio Capitolo perchè nella griglia c'è lo store
            Ext.get('ImpDisp').dom.value = eurRend(data.ImpDisp);
            Ext.get('Bilancio').dom.value = data.Bilancio;
            Ext.get('UPB').dom.value = data.UPB;
            Ext.get('MissioneProgramma').dom.value = data.MissioneProgramma;
            Ext.get('Capitolo').dom.value = data.Capitolo;
            Ext.get('TipoAtto').dom.value = data.TipoAtto;
            Ext.get('NumeroAtto').dom.value = data.NumeroAtto;
            Ext.get('DataAtto').dom.value = data.DataAtto;
            Ext.get('Oggetto_Impegno').dom.value = data.Oggetto_Impegno;
 
                    
           },
	        failure: function(response, result) {
	            maskApp.hide();
//	            	Ext.MessageBox.show({
//	                            title: 'Errore',
//	  				            msg: 'Numero impegno non trovato',
//	                            buttons: Ext.MessageBox.OK,
//	                            icon: Ext.MessageBox.ERROR,
//	                            fn: function(btn) { 			return }
	            //	  					 });
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

