 var tipoRettifica= new Ext.form.TextField({
						 fieldLabel: 'Tipologia',
						     id:'tipoRettifica',
						     allowBlank: false,
						     readOnly:true,
						     width :500
						 });
var myPanelVisualizzaRettifica = new Ext.FormPanel({
id: 'myPanelVisualizzaRettifica',
	frame: true,
    labelAlign: 'left',
    buttonAlign: "center",
    bodyStyle:'padding:1px',
    collapsible: false,
    width: 750,
    autoHeight:true,
    xtype: "form",
      title: " Rettifica Contabile ",
      items: [tipoRettifica]
	});
