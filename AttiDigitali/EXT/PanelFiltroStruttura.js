var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});

 				
var myPanelFiltroStruttura = new Ext.FormPanel({
id: 'myPanelFiltroStruttura',
 frame: true,
        labelAlign: 'left',
        title: "Dipartimento/Ufficio",
        buttonAlign: "center",
        bodyStyle:'padding:1px',
        collapsible: true,
        width: 800,
        autoHeight:true,
        xtype: "form"
        
	});
	

function buildComboUffici(dipartimentoScelto) {
    var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetUfficiDipendenti',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetUfficiDipendentiResult',
        fields: [
            { name: 'CodiceInterno' },
            { name: 'DescrizioneBreve' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
        , reader: reader
    });



    var parametri = { dipartimentoScelto: dipartimentoScelto };
    store.load({ params: parametri });
   store.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
                var ufficioDefault = Ext.get('Ufficio').dom.value;
                    if (ufficioDefault!=''){
                        ComboUffici.setValue(ufficioDefault);
                        Ext.get('Ufficio').dom.value = '';
                        var cosaVisualizzare = Ext.get('Visualizza').dom.value;
                        if (cosaVisualizzare == ''){
                            cosaVisualizzare =0;
                        }
                        if (cosaVisualizzare == 0){
                            InitFormCapitoliLista(ufficioDefault) ;
                        }
                        if (cosaVisualizzare == 1){
                            InitFormCapitoli(true, ufficioDefault) ;
                        }
                        if (cosaVisualizzare == 2){
                            InitFormCapitoliImpegno(true, ufficioDefault) ;
                        }
                        if (cosaVisualizzare == 3){
                            InitFormCapitoliRiduzioneLiq(false, ufficioDefault) ;
                        }
                }            

            },
            scope: this
        }
    });
    
      Ext.getCmp('myPanelFiltroStruttura').remove('ComboUffici');
      
     var ComboUffici = new Ext.form.ComboBox({
    fieldLabel: 'Ufficio ',
    displayField: 'DescrizioneBreve',
    valueField: 'CodiceInterno',
    id: 'ComboUffici',
     name: 'ComboUffici',
    mode: 'local',
    listWidth: 400,
    width: 400,
    triggerAction: 'all',
    store:store
 });

      
ComboUffici.on('select', function(record, index) { 

        if (Ext.getCmp('ElencoCapitoli')!= undefined) {
            Ext.getCmp('ElencoCapitoli').destroy();
        }
        var cosaVisualizzare = Ext.get('Visualizza').dom.value;
        if (cosaVisualizzare == ''){
            cosaVisualizzare=0;
        }
        if (cosaVisualizzare == 0){
            InitFormCapitoliLista(ComboUffici.store.data.get(ComboUffici.selectedIndex).data.CodiceInterno) ;
        }
        if (cosaVisualizzare == 1){
            InitFormCapitoli(true,  ComboUffici.store.data.get(ComboUffici.selectedIndex).data.CodiceInterno) ;
        }
        if (cosaVisualizzare == 2){
            InitFormCapitoliImpegno(true,  ComboUffici.store.data.get(ComboUffici.selectedIndex).data.CodiceInterno) ;
        }
         if (cosaVisualizzare == 3 ){
            InitFormCapitoliRiduzioneLiq(false,  ComboUffici.store.data.get(ComboUffici.selectedIndex).data.CodiceInterno) ;
        }
     });
     
     return ComboUffici;
 }

function buildComboDipartimenti() {

     var proxy = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetDipartimentiDipendenti',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

        root: 'GetDipartimentiDipendentiResult',
        fields: [
           { name: 'CodiceInterno' },
           { name: 'DescrizioneBreve' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy,
        reader: reader
    });

    store.load();
    store.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
                var dipartimentoDefault = Ext.get('Dipartimento').dom.value;
                if (dipartimentoDefault!=''){
                    ComboDipartimenti.setValue(dipartimentoDefault);
                    var comboUff = buildComboUffici(dipartimentoDefault);
                    Ext.getCmp('myPanelFiltroStruttura').add(comboUff);
                    myPanelFiltroStruttura.doLayout();
                    
                }else{
                Ext.get('Dipartimento').dom.value = '';
                }
                   
            },
            scope: this
        }
    });
var ComboDipartimenti =  new Ext.form.ComboBox({
    fieldLabel: 'Dipartimento ',
    displayField: 'DescrizioneBreve',
    valueField: 'CodiceInterno',
    id: 'ComboDipartimenti',
    mode: 'local',
    listWidth: 400,
    width: 400,
    triggerAction: 'all'   ,
    store:store
});
Ext.getCmp('myPanelFiltroStruttura').add(Ext.getCmp('ComboDipartimenti'));
ComboDipartimenti.on('select', function(record, index) { 

    if (Ext.getCmp('ElencoCapitoli')!= undefined) {
        Ext.getCmp('ElencoCapitoli').destroy();
     }
    var codDip = ComboDipartimenti.store.data.get(ComboDipartimenti.selectedIndex).data.CodiceInterno;
    var comboUff =  buildComboUffici(codDip);
    Ext.getCmp('myPanelFiltroStruttura').add(comboUff);
    myPanelFiltroStruttura.doLayout();
});

}
