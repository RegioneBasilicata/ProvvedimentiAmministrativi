
Ext.onReady(function() {

    Ext.QuickTips.init();
    //  Ext.form.Field.prototype.msgTarget = 'side';
    //builFormPanelUtentiUfficiCompetenza();    
    builFormPanelUfficiCompetenza();    
    buildComboDipartimenti();


    var proxy2 = new Ext.data.HttpProxy({
        url: 'ProcAmm.svc/GetUfficiCompetenzaDocumento' + window.location.search,
        method: 'GET'
    });

    var reader2 = new Ext.data.JsonReader({
        root: 'GetUfficiCompetenzaDocumentoResult',
        fields: [
               { name: 'CodiceInterno' },
               { name: 'DescrizioneBreve' },
               { name: 'DescrizioneToDisplay' }
          ]
    });

    var storeRegistrato = new Ext.data.Store({
        proxy: proxy2,
        reader: reader2
    });



    storeRegistrato.on({
        'load': {
            fn: function(store, records, options) {
                mask.hide();
             //   buildgridUfficiSelezionati(storeRegistrato)
            },
            scope: this
        }
    });
    storeRegistrato.load();
    //    buildItemSelector(null, storeRegistrato,true);


    builFormPanelUfficiCompetenzaRegistrati(storeRegistrato);

    Ext.getCmp('myPanelFiltroDipartimento').render('FiltroDipartimento');

    //Ext.getCmp('myFormPanelUfficiCompetenza').render('FormPanelUfficiCompetenza');
    
    //Ext.getCmp('myFormPanelUfficiCompetenzaRegistrati').render('FormPanelUfficiCompetenzaRegistrati');

    buildPanelPrincipale();
    Ext.getCmp('MyPrincipalPanel').render('FormPanelUfficiCompetenza');
    buildCopyDrop();
    

});