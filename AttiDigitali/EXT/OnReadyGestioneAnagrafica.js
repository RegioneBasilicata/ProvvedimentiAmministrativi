Ext.onReady(function() {
    Ext.QuickTips.init();
    Ext.Ajax.timeout = 100000000;
    var gestioneAnagrafica = buildPanelGestioneAnagrafica();



    gestioneAnagrafica.render("myPanelPrincipale");
});