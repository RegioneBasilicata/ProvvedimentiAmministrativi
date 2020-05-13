function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

var actionAdd = new Ext.Action({
    text: 'Carica il provvedimento',
    tooltip: 'Upload del testo del provvedimento',
    handler: function() {
      
        // window.location = "AggiungiAllAlberoAction.aspx" + window.location.search
        if (gup("tipo") == 2) {
            window.location = "LeggiTestoDisposizioneAction.aspx" + window.location.search
        } else {
            window.location = "LeggiTestoDeterminaAction.aspx" + window.location.search
        }
    },
    iconCls: 'upload'
});

var actionApri = new Ext.Action({
    text: 'Fascicolo',
    tooltip: 'Visualizza fascicolo',
    handler: function() {
        window.location = "AggiungiAllAlberoAction.aspx" + window.location.search
    },
    iconCls: 'open'
});
    
    var tbar = new Ext.Toolbar({
        id: 'headtoolbar',
        cls: 'topMenu',
        style: 'border:1px;',
        items: [actionAdd, actionApri]
    });

    var myPanelToolBar = new Ext.Panel({
        id:'myPanelToolBar',
        frame: true,
        labelAlign: 'left',
        buttonAlign: "center",
        bodyStyle: 'padding:1px',
        collapsible: true,
        width: 750,
        tbar: tbar
        
    });

 
