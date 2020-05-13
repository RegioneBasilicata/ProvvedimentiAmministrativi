
var dateMovimento = new Ext.form.DateField({
	fieldLabel: 'Data Movimento',
	id: 'dateMovimento',
	allowBlank: false,
	format: 'd/m/Y',
	readOnly: true
});
dateMovimento.setValue(new Date());

function getDataMovimentoAggiorna() {
	return dateMovimento.value;
}
var myPanelDataMovimento = new Ext.FormPanel({
	id: 'myPanelDataMovimento',
	frame: true,
	labelAlign: 'left',
	buttonAlign: "center",
	bodyStyle: 'padding:1px',
	collapsible: true,
	width: 750,
	autoHeight: true,
	xtype: "form",
	title: " Data Movimento",
	items: [dateMovimento]
});
