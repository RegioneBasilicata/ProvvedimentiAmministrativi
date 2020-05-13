Ext.onReady(function() {
    Ext.QuickTips.init();
    var tipo = Ext.get("TipoAtto").dom.value;
    if (tipo == 0) {
        buildOsservazioniDirContrAmministrativo();
    }
    if (tipo != 1) {
        buildOsservazioniDirGenerale();
    }
    if (tipo == 1) {
        buildOsservazioniDirSegretarioLegittimita();
        buildOsservazioniDirSegretarioDiPresidenza();
    }
    buildOsservazioniDirRagioneria();
    buildOsservazioniDirProponente();
    var abilitaOssUP = Ext.get("AbilitaOssUP").dom.value;
    abilitaScrittura(abilitaOssUP);
    Ext.getCmp('myPanel').render('Osservazioni');
    collapsePanel();
    
});          //FINE Ext.onReady
