//DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "Liquidazioni contestuali"
var myPanel = new Ext.FormPanel({
    id:'myPanel',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:5px',
    collapsible: true,
    width: 800
});
 
//DEFINISCO L'AZIONE AGGIUNGI DELLA GRIGLIA
var actionRegistraOss = new Ext.Action({
    id: 'btnRegistra',
    text: 'Registra',
    tooltip: 'Registra Osservazione',
    handler: function() {

        Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
        myPanel.getForm().timeout = 100000000;
        myPanel.getForm().submit({
            url: 'ProcAmm.svc/RegistraOsservazione' + window.location.search,
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
	                            title: 'Osservazioni',
	                            msg: lstr_messaggio,
	                            buttons: Ext.MessageBox.OK,
	                            icon: Ext.MessageBox.ERROR,
	                            fn: function(btn) {
	                            }
	                        });
	                    }, // FINE FAILURE
            success:
	                    function(result, response) {
	                        var msg = 'Osservazioni registrate con successo!';
	                        Ext.MessageBox.show({
	                            title: 'Osservazioni',
	                            msg: msg,
	                            buttons: Ext.MessageBox.OK,
	                            icon: Ext.MessageBox.INFO,
	                            fn: function(btn) {
	                            }
	                        }
	                        );
	                    } // FINE SUCCESS
        }) // FINE SUBMIT






    },
    iconCls: 'save'
});
var tbar = new Ext.Toolbar({
    id:'toolbar',
    items:[actionRegistraOss]
});

function buildOsservazioniDirSegretarioLegittimita() {
    //DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE GENERALE
    var panelDirSegretarioLegittimita = new Ext.Panel({
    id: 'panelDirSegretarioLegittimita',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        width: 760,
        autoHeight: true,
        title: "Osservazioni Dirigente Segreteria - Controllo di Legittimità",
        items: [
	        new Ext.form.TextArea({
	            id: 'OsservazioneUSL',
	            width: 750,
	            height: 50,
	            readOnly: true
	        })
	    ]
    });

    var testo = Ext.get("DirSegretarioLegittimita").dom.value;
    panelDirSegretarioLegittimita.findById('OsservazioneUSL').value = testo;
    Ext.getCmp('myPanel').add(panelDirSegretarioLegittimita);
}  

function buildOsservazioniDirSegretarioDiPresidenza() {
    //DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE GENERALE
    var panelDirSegretarioDiPresidenza = new Ext.Panel({
        id: 'panelDirSegretarioDiPresidenza',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        width: 760,
        autoHeight: true,
        title: "Osservazioni Dirigente Segreteria di Presidenza",
        items: [
	        new Ext.form.TextArea({
	            id: 'OsservazioneUSS',
	            width: 750,
	            height: 50,
	            readOnly: true
	        })
	    ]
    });

    var testo = Ext.get("DirSegretarioDiPresidenza").dom.value;
    panelDirSegretarioDiPresidenza.findById('OsservazioneUSS').value = testo;
    Ext.getCmp('myPanel').add(panelDirSegretarioDiPresidenza);
}  
  
  
function buildOsservazioniDirGenerale() {
    //DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE GENERALE
    var panelDirGenerale = new Ext.Panel({
	    id:'panelDirGenerale',
	    frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 760,
        autoHeight:true,
	    title : "Osservazioni Dirigente Generale",
	    items:[
	        new Ext.form.TextArea({
			    id: 'OsservazioneUDD' ,
			     width :750,
			     height :50,
			     readOnly:true
		    })
	    ]
    });

    var testo = Ext.get("DirGenerale").dom.value;
    if (panelDirGenerale.findById('OsservazioneUDD') != undefined ) {
        panelDirGenerale.findById('OsservazioneUDD').value = testo;
    }
    
    Ext.getCmp('myPanel').add(panelDirGenerale);
}

function buildOsservazioniDirRagioneria() {
//DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE Ragioneria
var panelDirRagioneria = new Ext.Panel({
	id:'panelDirRagioneria',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 760,
    autoHeight:true,
	title : "Osservazioni Dirigente Ragioneria",
	items:[
	    new Ext.form.TextArea({
			id: 'OsservazioneUR' ,
			 width :750,
			 height :50,
			 readOnly:true
		})
	]
});
    var testo = Ext.get("DirRagioneria").dom.value;
    panelDirRagioneria.findById('OsservazioneUR').value=testo;
    Ext.getCmp('myPanel').add(panelDirRagioneria);
}

function buildOsservazioniDirContrAmministrativo() {
    //DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE Controllo Amministrativo
    var panelDirContrAmministrativo = new Ext.Panel({
	    id:'panelDirContrAmministrativo',
	    frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 760,
        autoHeight:true,
	    title : "Osservazioni Dirigente Controllo Amministrativo",
	    items:[
	        new Ext.form.TextArea({
			    id: 'OsservazioneUCA' ,
			     width :750,
			     height :50,
			     readOnly:true
		    })
	    ]
    	
    });
  var testo = Ext.get("DirContrAmministrativo").dom.value;
    panelDirContrAmministrativo.findById('OsservazioneUCA').value=testo;
    Ext.getCmp('myPanel').add(panelDirContrAmministrativo);
}

function buildOsservazioniDirProponente() {

    //DEFINISCO LA FORM CHE CONTIENE LE OSSERVAZIONI DEL DIRIGENTE dell'ufficio Proponente
    var panelDirProponente = new Ext.Panel({
	    id:'panelDirProponente',
	    frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 760,
        autoHeight:true,
	    title : "Osservazioni Dirigente Proponente",
	    items:[
	        new Ext.form.TextArea({
			    id: 'OsservazioneUP' ,
			     width :750,
			     height :50,
			     readOnly:true
		    })
	    ]
    });
     var testo = Ext.get("DirProponente").dom.value;
    panelDirProponente.findById('OsservazioneUP').value=testo;
    Ext.getCmp('myPanel').add(panelDirProponente);
}
function collapsePanel(){
    var livello = Ext.get("VerificaRuolo").dom.value;
    if (Ext.getCmp('panelDirProponente')!= undefined){
        if ((Ext.getCmp('panelDirProponente').findById('OsservazioneUP').value =='') && (livello !='UP')) {
            Ext.getCmp('panelDirProponente').collapse();
        }
    }
     if (Ext.getCmp('panelDirGenerale')!= undefined){
       if ((Ext.getCmp('panelDirGenerale').findById('OsservazioneUDD').value =='')&& (livello !='UDD')){
            Ext.getCmp('panelDirGenerale').collapse();
        }
     }
     if (Ext.getCmp('panelDirRagioneria')!= undefined){
        if ((Ext.getCmp('panelDirRagioneria').findById('OsservazioneUR').value =='')&& (livello !='UR')){
            Ext.getCmp('panelDirRagioneria').collapse();
        }
     }
     if (Ext.getCmp('panelDirContrAmministrativo')!= undefined){
        if ((Ext.getCmp('panelDirContrAmministrativo').findById('OsservazioneUCA').value =='')&& (livello !='UCA')){
            Ext.getCmp('panelDirContrAmministrativo').collapse();
        }
    }
    if (Ext.getCmp('panelDirSegretarioLegittimita') != undefined) {
        if ((Ext.getCmp('panelDirSegretarioLegittimita').findById('OsservazioneUSL').value == '') && (livello != 'USL')) {
            Ext.getCmp('panelDirSegretarioLegittimita').collapse();
        }
    }
    if (Ext.getCmp('panelDirSegretarioDiPresidenza') != undefined) {
        if ((Ext.getCmp('panelDirSegretarioDiPresidenza').findById('OsservazioneUSS').value == '') && (livello != 'USS')) {
            Ext.getCmp('panelDirSegretarioDiPresidenza').collapse();
        }
    }

    
}
function abilitaScrittura(abilitaOssUP) {
    var livello = Ext.get("VerificaRuolo").dom.value;
    if (livello == 'UP' && abilitaOssUP == "True") {
        if (Ext.getCmp('panelDirProponente')!= undefined){
            Ext.getCmp('panelDirProponente').findById('OsservazioneUP').readOnly =false;
            Ext.getCmp('panelDirProponente').insert(0,tbar);
        }
   }
   if (livello =='UDD'){
        if (Ext.getCmp('panelDirGenerale')!= undefined){
            Ext.getCmp('panelDirGenerale').findById('OsservazioneUDD').readOnly =false;
            Ext.getCmp('panelDirGenerale').insert(0,tbar);
        }
    }
    if (livello =='UR'){
        if (Ext.getCmp('panelDirRagioneria')!= undefined){
            Ext.getCmp('panelDirRagioneria').findById('OsservazioneUR').readOnly =false;
            Ext.getCmp('panelDirRagioneria').insert(0,tbar);
        }
    }
    if (livello =='UCA'){
        if (Ext.getCmp('panelDirContrAmministrativo')!= undefined){
            Ext.getCmp('panelDirContrAmministrativo').findById('OsservazioneUCA').readOnly =false;
            Ext.getCmp('panelDirContrAmministrativo').insert(0,tbar);
        }
    }
    if (livello == 'USL') {
        if (Ext.getCmp('panelDirSegretarioLegittimita') != undefined) {
            Ext.getCmp('panelDirSegretarioLegittimita').findById('OsservazioneUSL').readOnly = false;
            Ext.getCmp('panelDirSegretarioLegittimita').insert(0, tbar);
        }
    }
    if (livello == 'USS') {
        if (Ext.getCmp('panelDirSegretarioDiPresidenza') != undefined) {
            Ext.getCmp('panelDirSegretarioDiPresidenza').findById('OsservazioneUSS').readOnly = false;
            Ext.getCmp('panelDirSegretarioDiPresidenza').insert(0, tbar);
        }
    }
}

