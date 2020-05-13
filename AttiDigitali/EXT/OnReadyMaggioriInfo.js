var actionWL = new Ext.Action({
    text: 'Lista Lavoro',
    tooltip: 'Torna alla Lista Lavoro',
    handler: function() {
    window.location = "WorklistAction.aspx?tipo=" +  Ext.get("TipoAtto").dom.value; 
    },
    iconCls: 'open'
});    
    
    var tbarHead = new Ext.Toolbar({
        id: 'headtoolbar',
        cls: 'topMenu',
        style: 'border:1px;',
        items: [ actionWL]
    });

    var myPanelToolBar = new Ext.Panel({
        id:'myPanelToolBar',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        width: 800,
        tbar: tbarHead
        
    });

 


Ext.onReady(function() {
    Ext.QuickTips.init();
    myPanelToolBar.render('ToolBarMenu');
    buildGridMessaggi();
    Ext.getCmp('myPanelMessaggi').render('ListaMessaggi');
    var tipo = Ext.get("TipoAtto").dom.value;
    if (tipo==0) {
        buildOsservazioniDirContrAmministrativo();
    } if (tipo == 1) {
        buildOsservazioniDirSegretarioLegittimita();
        buildOsservazioniDirSegretarioDiPresidenza();
    }
    buildOsservazioniDirRagioneria();
    buildOsservazioniDirGenerale();
    buildOsservazioniDirProponente();
    abilitaScrittura();
    Ext.getCmp('myPanel').render('Osservazioni');
    collapsePanel();
    buildGridSuggerimenti();
    Ext.getCmp('myPanelSuggerimenti').render('ListaSuggerimenti');
    
});          //FINE Ext.onReady
