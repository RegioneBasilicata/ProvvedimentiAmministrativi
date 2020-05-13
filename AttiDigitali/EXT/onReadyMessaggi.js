
Ext.onReady(function() {
    Ext.QuickTips.init();
    buildGridMessaggi();
    Ext.getCmp('myPanelMessaggi').render('ListaMessaggi');
    var isinviati;
    isinviati = Ext.get('inviati').dom.value;
    if (isinviati == 1) {
        var tbarmyPanelToolBar = Ext.getCmp('myPanelMessaggi').tbar;
        if (tbarmyPanelToolBar != undefined) {
            tbarmyPanelToolBar.remove();
        }
    }
    

});          //FINE Ext.onReady
