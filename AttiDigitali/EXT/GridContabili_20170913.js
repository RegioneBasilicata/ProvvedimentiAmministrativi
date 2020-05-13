

Ext.onReady(function() {

        Ext.Ajax.timeout = 100000000;
        Ext.getCmp('myPanelDataMovimento').render('DataMov');
        var rettifiche = Ext.get('TipoRettifiche').dom.value;
        if (rettifiche != '') {
            Ext.getCmp('tipoRettifica').value = rettifiche;
            Ext.getCmp('myPanelVisualizzaRettifica').render('Rettifica');
        }

    //        //Preimpegni
    if (Ext.get('NPreimpReg') != undefined) {
        var renderGridPreimpegni = Ext.get('NPreimpReg').dom.value;
        if (renderGridPreimpegni > 0) {
            var GridPreimpegni = buildGridPreimpegni();
            myPanelPreimpegni.add(GridPreimpegni);
            myPanelPreimpegni.render("ListaPreimpegni");
        }
    }

    //Impegni
    if (Ext.get('NimpReg') != undefined) {
        var renderGridRiep = Ext.get('NimpReg').dom.value;
        if (renderGridRiep > 0) {

            var GridRiep = buildGridRiep();
            myPanel.add(GridRiep);
            myPanel.render("ListaCap");
        }
    }

    if (Ext.get('NliqReg') != undefined) {
        var renderGridLiq = Ext.get('NliqReg').dom.value;
        if (renderGridLiq > 0) {
            var GridLiq = buildGridLiquidazioni();
            myPanelLiq.add(GridLiq);
            myPanelLiq.render("ListaLiq");
        }
    }

        if (Ext.get('NridReg')!= null){
            var renderGridRid = Ext.get('NridReg').dom.value;
            if (renderGridRid > 0) {
                var GridRid = buildGridRiduzioni();
                myPanelRiduzioni.add(GridRid);
                myPanelRiduzioni.render("ListaRid");
             }
       }
        if (Ext.get('NridPreImpReg')!= null){
            var renderGridRidPreImp = Ext.get('NridPreImpReg').dom.value;
            if (renderGridRidPreImp > 0) {
                var GridRidPreImp = buildGridRiduzioniPreImp();
                myPanelRiduzioniPreImp.add(GridRidPreImp);
                myPanelRiduzioniPreImp.render("ListaRidPreImp");
             }
       }
       if (Ext.get('NridLiqReg')!= null){
            var renderGridRidLiq = Ext.get('NridLiqReg').dom.value;
            if (renderGridRidLiq > 0) {
                var GridRidLiq = buildGridRiduzioniLiq();
                myPanelRiduzioniLiq.add(GridRidLiq);
                myPanelRiduzioniLiq.render("ListaRidLiq");
             }
       }
            if (Ext.get('chkAccertamento')!= null){
            var accertamento = Ext.get('chkAccertamento').dom.value;
            if (accertamento ==true) {
                actionRegAccertamento.setDisabled(true);
                Ext.getCmp('myPanelAccertamento').render('Accertamento');
            }
         }
         
     
    var isUffRag;
    isUffRag = Ext.get('isUffRag').dom.value;
    if (isUffRag == 0) {
    
    //solo di consultazione
         if (Ext.get('NMandati').dom.value > 0) {
            var GridMandati = buildGridMandati();
            myPanelMandati.add(GridMandati);
            myPanelMandati.render("ListaMandati")
        }
      
        Ext.getCmp('myPanelDataMovimento').hide();
                
        var tbarmyPanelToolBar = Ext.getCmp('myPanel').tbar;
        if (tbarmyPanelToolBar != undefined) {
            tbarmyPanelToolBar.remove();
        }
        
        if( Ext.getCmp('myPanelRiduzioni') != undefined){
            var tbarmyPanelRiduzioniToolBar = Ext.getCmp('myPanelRiduzioni').tbar;
            if (tbarmyPanelRiduzioniToolBar != undefined) {
                tbarmyPanelRiduzioniToolBar.remove();
            }
        }
        
        var tbarmyPanelLiqToolBar = Ext.getCmp('myPanelLiq').tbar;
        if (tbarmyPanelLiqToolBar != undefined) {
            tbarmyPanelLiqToolBar.remove();
        }
        
         if( Ext.getCmp('myPanelRiduzioniLiq') != undefined){
            var tbarmyPanelRiduzioniLiqToolBar = Ext.getCmp('myPanelRiduzioniLiq').tbar;
            if (tbarmyPanelRiduzioniLiqToolBar != undefined) {
                tbarmyPanelRiduzioniLiqToolBar.remove();
            }
        }
         if( Ext.getCmp('myPanelRiduzioniPreImp') != undefined){
            var tbarmyPanelRiduzioniPreImpToolBar = Ext.getCmp('myPanelRiduzioniPreImp').tbar;
            if (tbarmyPanelRiduzioniPreImpToolBar != undefined) {
                tbarmyPanelRiduzioniPreImpToolBar.remove();
            }
        }
     }

});  //FINE Ext.onReady