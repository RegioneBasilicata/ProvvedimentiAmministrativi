

Ext.onReady(function(){
    Ext.QuickTips.init();
    if (myPanelToolBar != undefined) {
        myPanelToolBar.render('ToolBarMenu');
    }

    var documentoConFatture = Ext.get('NFattureDocumento').dom.value;
    var isFatturePresenti = false;
    if (documentoConFatture == 0)
        isFatturePresenti = false;
    else
        isFatturePresenti = true;

    var conLiquidazione = Ext.get('chkLiquidazione').dom.value;
    if (conLiquidazione == true) {
        var GridLiq = buildGridLiquidazioni(isFatturePresenti);
        myPanelLiq.add(GridLiq);
        myPanelLiq.render("ListaLiq");
        actionCompilaRiduzioni.hide();
    }
    var accertamento = Ext.get('chkAccertamento').dom.value;
    if (accertamento == true) {
        Ext.getCmp('myPanelAccertamento').render('Accertamento');
    }
    var rettifiche = Ext.get('TipoRettifiche').dom.value;
    if (rettifiche != '') {
        Ext.getCmp('tipoRettifica').value = rettifiche;
        Ext.getCmp('myPanelVisualizzaRettifica').render('Rettifica');
    }
     var LiquidazioniDaConfermare =  Ext.get('LiquidazioniDaConfermare').dom.value;
        if (LiquidazioniDaConfermare == 0){
           NascondiColonneLiq();
        }
		isUffProp = Ext.get('isUffProp').dom.value;
		actionAdd.setDisabled(false);
		if (isUffProp == 0) {
		    if (Ext.getCmp('myPanelLiq')!= undefined ){
		        var tbarMyPanelLiq = Ext.getCmp('myPanelLiq').tbar;
                if (tbarMyPanelLiq != undefined) {
                    tbarMyPanelLiq.remove();
                }
		    }
		}else{
            //caso ufficio proponente: verifica caricatamento il testo del documento
            var testoCaricato;
            testoCaricato = Ext.get('TestoCaricato').dom.value;
            if (testoCaricato == 1) {
                actionAdd.hide();
            }
    }
}); //FINE Ext.onReady