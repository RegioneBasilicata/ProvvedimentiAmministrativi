var label= new Ext.form.Label({
						  text: 'Si chiede di accertare euro: ',
						  id: 'lblAccert' 
						 });

var field= new Ext.form.TextField({
fieldLabel: 'Somma da accertare',
id: 'ImpDaAccertare',
	labelSeparator:':',
	blankText:'',
	allowBlank:false,
	emptyText:''

});

field.on('change',function(){actionRegAccertamento.setDisabled(false);})

function registraAccertamento() {
    if (Ext.getCmp('ImpDaAccertare').value == '') {
           Ext.getCmp('ImpDaAccertare').value = '0'
    }
    
    //Ext.lib.Ajax.defaultPostHeader = 'application/x-www-form-urlencoded';
    Ext.getCmp('myPanelAccertamento').getForm().timeout = 100000000;
    Ext.getCmp('myPanelAccertamento').getForm().submit({
        url: 'ProcAmm.svc/GenerazioneAccertamentoUP',
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
								        title: 'Salvataggio Accertamento Fallito',
								        msg: lstr_messaggio,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.ERROR,
								        fn: function(btn) {
								            return;
								        }
								    });

								}, // FINE FAILURE
        success:
								function(result, response) {
								    var msg = 'Salvataggio Accertamento avvenuto successo!';
                                    GetAccertamentoRegistrato();
								    Ext.MessageBox.show({
								        title: 'Salvataggio Accertamento',
								        msg: msg,
								        buttons: Ext.MessageBox.OK,
								        icon: Ext.MessageBox.INFO,
								        fn: function(btn) {

								        }
								    }
								    );
								} // FINE SUCCESS
    }) // FINE SUBMIT
}
var actionRegAccertamento = new Ext.Action({
    id: 'action-add-accertamento',
    text: 'Registra Accertamento',
    tooltip: 'Registra Accertamento',
    handler: function() {
        registraAccertamento();
    },
    iconCls: 'add'
});


var myPanelAccertamento = new Ext.FormPanel({
id: 'myPanelAccertamento',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: true,
    width: 750,
    autoHeight:true,
    xtype: "form",
    tbar: [actionRegAccertamento],
	title : "Accertamento",
	items:[field,{
            id: "idAccertamento",
            xtype: "hidden"
}]
	     


	 });
	 
	 function GetAccertamentoRegistrato(){
	 
	 
 			// metodo get    GetAccertamentoRegistrato
     		// il value per ImpDaAccertare si chiama ImpPrenotato = 0
    		// il value per idAccertamento si chiama ID = 0
      		// var params={ImpDaAccertare:0,idAccertamento:0};
		
  		 Ext.Ajax.request({
  		 url: 'ProcAmm.svc/GetAccertamentoRegistrato' + window.location.search,
      			// params: Ext.encode(params),
  		        method: 'GET',
  		        headers: { 'Content-Type': 'application/json' },
		       success: function(response, options) {
		       
		           var data = Ext.decode(response.responseText);
		           Ext.getCmp('ImpDaAccertare').setValue(data.GetAccertamentoRegistratoResult.ImpPrenotato);
		           Ext.getCmp('idAccertamento').setValue(data.GetAccertamentoRegistratoResult.ID);
		          actionRegAccertamento.setDisabled(true);
		           
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
	myPanelAccertamento.on('render',function() {
		GetAccertamentoRegistrato();
   });
