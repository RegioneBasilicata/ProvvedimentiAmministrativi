Ext.onReady(function() {
    Ext.QuickTips.init();
    Ext.Ajax.timeout = 100000000;

    setValoriDatiSeduta();


    var panelPrincipale = buildPanelPrincipale(Ext.getDom('codDocumento').value != "" ? itemDatiSedutaInfo : null);
    panelPrincipale.render("myPanelPrincipale");

    
});

var itemDatiSedutaInfo = '';
var codDocumento = '';

function setValoriDatiSeduta() {

    var codDocumento = Ext.getDom('codDocumento').value;

    var itemDatiSedutaInfoJsonValue = Ext.getDom('itemDatiSedutaInfo').value;
    if (itemDatiSedutaInfoJsonValue != undefined && itemDatiSedutaInfoJsonValue != null && itemDatiSedutaInfoJsonValue.trim().length != 0)
        itemDatiSedutaInfo = Ext.decode(itemDatiSedutaInfoJsonValue);

}