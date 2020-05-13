/**
 * @author samazzeo
 * 
 * 
 */
 function formatEU(v)
 
{
	v = (Math.round((v-0)*100))/100;
	v = (v == Math.floor(v)) ? v + ".00" : ((v*10 == Math.floor(v*10)) ? v + "0" : v);
	
	return ('€ ' + v).replace(/\./, ',');
};


Ext.onReady(function() {
	
	var eurofield= new Ext.form.TextField({
		
		id:'euro',
		emptyText:'numero prego',
		fieldLabel:'euro'
		
	})	;
	
	eurofield.on('blur',function(){
		var val =eurofield.getValue();
		//alert(val);
		 val = formatEU(val);
		
		//Ext.get('euro').dom.value=val;
		
		eurofield.setValue(val);
		//alert(eurofield.value);
	
	},this);
	
	var panel= new Ext.FormPanel({
		width:200,
		height:200
	})
	panel.add(eurofield);
	panel.render(Ext.getBody());
	
})