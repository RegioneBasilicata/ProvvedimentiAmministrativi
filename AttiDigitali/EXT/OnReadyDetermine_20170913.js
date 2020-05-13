
Ext.onReady(function() {
    var tipo = 0;
    tipo = Ext.getDom('tipo').value;

    Ext.QuickTips.init();
    myPanelToolBar.render('ToolBarMenu');
    var conRiduzionePreImp = Ext.get('chkRiduzionePreImp').dom.value;
    if (conRiduzionePreImp == true) {
        var GridRidPreImp = buildGridRiduzioniPreImp();
        myPanelRiduzioniPreImp.add(GridRidPreImp);
        myPanelRiduzioniPreImp.render("ListaRidPreImp");

        var RiduzioniPreImpDaConfermare = Ext.get('RiduzioniPreImpDaConfermare').dom.value;
        if (RiduzioniPreImpDaConfermare == 0) {
            NascondiColonneRidPreImp();
        }
    }

    //conserve l'informazione se è stata scelta la tipologia IMP
    var chkPreimpegni = Ext.get('chkPreimpegni').dom.value;
    //var conImpegno = true;
    if (chkPreimpegni == true) {
        var GridPren = buildGridPreimpegni();
        myPanelPrenotazioni.add(GridPren);
        myPanelPrenotazioni.render("ListaPrenotazioni");
        var PrenotazioniDaConfermare = Ext.get('PrenotazioniDaConfermare').dom.value;
        if (PrenotazioniDaConfermare == 0) {
            NascondiColonnePrenotazioni();
        }
    }

    //conserve l'informazione se è stata scelta la tipologia IMP
    var conImpegno = Ext.get('chkImpegno').dom.value;
    //var conImpegno = true;

    var documentoConFatture = Ext.get('NFattureDocumento').dom.value;
    var isFatturePresenti = false;
    if (documentoConFatture == 0)
        isFatturePresenti = false;
    else
        isFatturePresenti = true;

    if (conImpegno == true) {
        var GridRiep = buildGridRiep(isFatturePresenti);
        myPanel.add(GridRiep);
        myPanel.render("ListaCap");
        // Se è una determina:
        if (tipo == 0) {
        var GridLiqContestuali = buildGridLiquidazioniContestuali(true, isFatturePresenti);

        myPanelLiqContestuali.add(GridLiqContestuali);
        myPanelLiqContestuali.render("ListaLiqContestuali");
        myPanelLiqContestuali.hide();

            if (Ext.get('NimpReg').dom.value > 0) {
                myPanelLiqContestuali.show();
            }
        }
        var ImpegniDaConfermare = Ext.get('ImpegniDaConfermare').dom.value;
        if (ImpegniDaConfermare == 0) {
            NascondiColonne(tipo);
        }
        // Se è una determina:
        if (tipo == 0) {
            var LiquidazioniContestualiDaConfermare = Ext.get('LiquidazioniContestualiDaConfermare').dom.value;
            if (LiquidazioniContestualiDaConfermare == 0) {
                NascondiColonneLiqContestuali();
            }
        }
    }

    //conserve l'informazione se è stata scelta la tipologia LIQ
    var conLiquidazione = Ext.get('chkLiquidazione').dom.value;
    //  var conLiquidazione = true;
    if (conLiquidazione == true) {
        //codice commentato: vecchia implementazione dove si costruisce 
        //la prima griglia che mostra tutte le fatture del documento
        //        if (documentoConFatture == 1) {
//            var GridFattDoc = buildGridFattureDocumento();

//            myPanelFatture.add(GridFattDoc);
//            myPanelFatture.render("ListaFatt");
//        }
        var GridLiq = buildGridLiquidazioni(isFatturePresenti);
        myPanelLiq.add(GridLiq);
        myPanelLiq.render("ListaLiq");

        var LiquidazioniDaConfermare = Ext.get('LiquidazioniDaConfermare').dom.value;
        if (LiquidazioniDaConfermare == 0) {
            NascondiColonneLiq();
        }

    }
    var rettifiche = Ext.get('TipoRettifiche').dom.value;
    if (rettifiche != '') {
        Ext.getCmp('tipoRettifica').value = rettifiche;
        Ext.getCmp('myPanelVisualizzaRettifica').render('Rettifica');
    }
    //conserve l'informazione se è stata scelta la tipologia RID
    var conRiduzione = Ext.get('chkRiduzione').dom.value;
    if ((conRiduzione == true) & (conLiquidazione == true)) {
        var GridRiduzioniContestuali = buildGridRiduzioni();
        actionCompilaRiduzioni.show();
        actionRegistraRidContestuale.setDisabled(true);
        myPanelRiduzioniContestuali.add(GridRiduzioniContestuali);
        myPanelRiduzioniContestuali.render("ListaCapRiduzioneContestuali");
        var RiduzioniDaConfermare = Ext.get('RiduzioniDaConfermare').dom.value;
        if (RiduzioniDaConfermare == 0) {
            NascondiColonneRid();
        }

    }
    if ((conRiduzione == true) && (conLiquidazione == false)) {
        var GridRiduzioni = buildGridRiduzioni();
        myPanelRiduzioni.add(GridRiduzioni);
        myPanelRiduzioni.render("ListaCapRiduzione");
        //Lu Rid
        DisabilitaColonneRiduzione();

        var RiduzioniDaConfermare = Ext.get('RiduzioniDaConfermare').dom.value;
        if (RiduzioniDaConfermare == 0) {
            NascondiColonneRid();
        }
    }


    if ((conRiduzione == false) && (conLiquidazione == true)) {
        actionCompilaRiduzioni.hide();
    }


    var impSuPerenti = Ext.get('chkImpegnoSuPerenti').dom.value;
    if (impSuPerenti == true) {
        var gridPerenti = buildGridPerenti();
        var gridLiqPerenti = buildGridLiquidazioniPerenti(isFatturePresenti);
        myPanelPerenti.add(gridPerenti);
        //  myPanelPerenti.add(gridLiqPerenti);
        myPanelPerenti.render('ListaCapPerenti');

        myPanelPerenti.show();

        myPanelLiqPerentiContestuali.add(gridLiqPerenti)
        myPanelLiqPerentiContestuali.render('ListaLiqContestualiPerenti');
        myPanelLiqPerentiContestuali.show();


        var impegniPerentiDaConfermare = Ext.get('ImpegniPerentiDaConfermare').dom.value;
        if (impegniPerentiDaConfermare == 0) {
            NascondiColonneImpPer();
            NascondiColonneLiqPer();
        }


    }

    var accertamento = Ext.get('chkAccertamento').dom.value;
    if (accertamento == true) {
        Ext.getCmp('myPanelAccertamento').render('Accertamento');
    }

    if (Ext.get('NMandati').dom.value > 0) {
        var GridMandati = buildGridMandati();
        myPanelMandati.add(GridMandati);
        myPanelMandati.render("ListaMandati");

    }


    myPanelLiqContestuali.show();

    var conRiduzioneLiq = Ext.get('chkRiduzioneLiq').dom.value;
    if (conRiduzioneLiq == true) {
        var GridRidLiq = buildGridRiduzioniLiq();
        myPanelRiduzioniLiq.add(GridRidLiq);
        myPanelRiduzioniLiq.render("ListaRidLiq");

        var RiduzioniLiqDaConfermare = Ext.get('RiduzioniLiqDaConfermare').dom.value;
        if (RiduzioniLiqDaConfermare == 0) {
            NascondiColonneRidLiq();
        }
    }

    var isUffProp;
    isUffProp = Ext.get('isUffProp').dom.value;
    if (isUffProp == 0) {
		if (Ext.getCmp('myPanelToolBar') != undefined) {
			var tbarMyPanelToolBar = Ext.getCmp('myPanelToolBar').tbar;
			if (tbarMyPanelToolBar != undefined) {
				tbarMyPanelToolBar.remove();
			}
		}
        if (Ext.getCmp('myPanel') != undefined) {
            var tbarMyPanel = Ext.getCmp('myPanel').tbar;
            if (tbarMyPanel != undefined) {
                actionAddImpegno.hide();
                actionAddFromFile.hide();
                actionDeleteImpegno.hide();
                actionCompilaLiquidazioni.hide();
                actionConfermaImpegno.hide();
                actionModificaCOGAndPdCFImp.hide();
                //tbarMyPanel.remove();
            }
        }
		if (Ext.getCmp('myPanelPrenotazioni') != undefined) {
            var tbarmyPanelPrenotazioni = Ext.getCmp('myPanelPrenotazioni').tbar;
            if (tbarmyPanelPrenotazioni != undefined) {
                tbarmyPanelPrenotazioni.remove();
            }
        }  
        if (Ext.getCmp('myPanelLiq') != undefined) {
            var tbarMyPanelLiq = Ext.getCmp('myPanelLiq').tbar;
            if (tbarMyPanelLiq != undefined) {

                //actionAddLiquidazione.hide();
                //actionDeleteLiquidazione.hide();
                //actionConfermaLiquidazione.hide();
                //actionCompilaRiduzioni.hide();
                //actionShowFattureLiquidazioneNonCont.hide();
                //actionBeneficiari.hide();

                tbarMyPanelLiq.remove();
            }
        }
        if (Ext.getCmp('myPanelRiduzioniContestuali') != undefined) {
            var tbarMyPanelRiduzioniContestuali = Ext.getCmp('myPanelRiduzioniContestuali').tbar;
            if (tbarMyPanelRiduzioniContestuali != undefined) {
                tbarMyPanelRiduzioniContestuali.remove();
            }
        }
        if (Ext.getCmp('myPanelLiqContestuali') != undefined) {
            var tbarMyPanelLiqContestuali = Ext.getCmp('myPanelLiqContestuali').tbar;
            if (tbarMyPanelLiqContestuali != undefined) {
                tbarMyPanelLiqContestuali.remove();
            }

            actionRegistraLiq.hide();
            actionCancellaLiq.hide();
            actionConfermaLiquidazione2.hide();
            //actionShowFattureLiquidazione.hide();
            //actionBeneficiari2.hide();
        }
        if (Ext.getCmp('myPanelPerenti') != undefined) {
            var tbarMyPanelPerenti = Ext.getCmp('myPanelPerenti').tbar;
            if (tbarMyPanelPerenti != undefined) {
                tbarMyPanelPerenti.remove();
            }
        }
        if (Ext.getCmp('myPanelRiduzioniContestuali') != undefined) {
            var tbarMyPanelRiduzioniContestuali = Ext.getCmp('myPanelRiduzioniContestuali').tbar;
            if (tbarMyPanelRiduzioniContestuali != undefined) {
                tbarMyPanelRiduzioniContestuali.remove();
            }
        }
        if (Ext.getCmp('myPanelRiduzioni') != undefined) {
            var tbarMyPanelRiduzioni = Ext.getCmp('myPanelRiduzioni').tbar;
            if (tbarMyPanelRiduzioni != undefined) {
                tbarMyPanelRiduzioni.remove();
            }
        }
        if (Ext.getCmp('myPanelAccertamento') != undefined) {
            var tbarMyPanelAccertamento = Ext.getCmp('myPanelAccertamento').tbar;
            if (tbarMyPanelAccertamento != undefined) {
                tbarMyPanelAccertamento.remove();
            }
        }
        if (Ext.getCmp('myPanelRiduzioniPreImp') != undefined) {
            var tbarmyPanelRiduzioniPreImp = Ext.getCmp('myPanelRiduzioniPreImp').tbar;
            if (tbarmyPanelRiduzioniPreImp != undefined) {
                tbarmyPanelRiduzioniPreImp.remove();
            }
        }
        if (Ext.getCmp('myPanelRiduzioniLiq') != undefined) {
            var tbarmyPanelRiduzioniLiq = Ext.getCmp('myPanelRiduzioniLiq').tbar;
            if (tbarmyPanelRiduzioniLiq != undefined) {
                tbarmyPanelRiduzioniLiq.remove();
            }
        }
    } else {
        //caso ufficio proponente: verifica caricatamento il testo del documento
        var testoCaricato;
        testoCaricato = Ext.get('TestoCaricato').dom.value;
        if (testoCaricato == 1) {
            actionAdd.hide();
        }
    }

});                  //FINE Ext.onReady
