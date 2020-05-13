var mask;
mask = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});


var storeRettifiche = new Ext.data.SimpleStore({
fields: ['Id', 'Descrizione'],
    data:[['1',"Annullamento"],['2',"liq"]]
});





function buildComboSistemazioni(tipo) {
    var proxy = new Ext.data.HttpProxy({
    url: 'ProcAmm.svc/GetListaTipoRettifiche',
        method: 'GET'
    });
    var reader = new Ext.data.JsonReader({

    root: 'GetListaTipoRettificheResult',
        fields: [
           { name: 'Id' },
           { name: 'Descrizione' }
           ]
    });

    var store = new Ext.data.Store({
        proxy: proxy
  , reader: reader
    });

    store.setDefaultSort("Descrizione", "ASC");

  
  

    store.on({
        'load': {
            fn: function(store, records, options) {
            mask.hide();
            Ext.getCmp("ComboSistemazioni").setValue(arrOpContabili[7])
            },
            scope: this
        }
    });

    var ComboSistemazioni = new Ext.form.ComboBox({
        fieldLabel: 'ComboSistemazioni',
        name: 'ComboSistemazioni',
        hiddenName: 'ValueSistemazioni',
        id: 'ComboSistemazioni',
        width: 300,
        itemCls: "titfis",
        listWidth: 300,
        triggerAction: 'all',
        typeAhead: false,
        emptyText: '',
        displayField: 'Descrizione',
        valueField: 'Id',
        mode: 'local',
        readOnly:true,        
        store: store

    });
    var parametri = { tipoDocumento: tipo };
    store.load({params:parametri});
  //  store.load();
    return ComboSistemazioni;

}
function rispostaVerificaOggetto(btn) {
    if (btn == 'yes') {CreaProvvedimento()}

}
function verificaEsistenzaOggetto(flag_ChiamaCreaProvvedimento) {
 
    var stdec = Ext.lib.Ajax.defaultPostHeader
    Ext.lib.Ajax.defaultPostHeader = 'application/json';

   var maskOggetto = new Ext.LoadMask(Ext.getBody(), {
        msg: "Verifica Oggetto..."
   
    });
   
    var lstr_oggetto = Ext.getCmp('myPanel').getForm().getValues().txtOggetto;
    if (lstr_oggetto != '') {
        maskOggetto.show()
        var params = { tipo: tipo, oggetto: lstr_oggetto };
        //     var stdec = Ext.lib.Ajax.defaultPostHeader 
        //    Ext.lib.Ajax.defaultPostHeader = 'application/json';
        //    alert(stdec)
        Ext.Ajax.timeout = 100000000
      
        Ext.MessageBox.wait('Loading ...');
        var box = Ext.Ajax.request({
            async: false,
            url: 'ProcAmm.svc/VerificaEsistenzaOggetto',
            params: Ext.encode(params),
            method: 'POST',
            success: function(response, options) {
                maskOggetto.hide()

                var data = Ext.decode(response.responseText);

                if (data.VerificaEsistenzaOggettoResult == true) {

                    //                    Ext.MessageBox.show({
                    //                        title: 'Avviso',
                    //                        msg: 'Attenzione!! Esiste già un provvedimento con lo stesso oggetto',
                    //                        buttons: Ext.MessageBox.OK,
                    //                        icon: Ext.MessageBox.INFO,
                    //                        fn: function(btn) {
                    //                            return;
                    //                        }
                    //                    })
                    Ext.MessageBox.buttonText.yes = 'Si';

                    Ext.MessageBox.show({
                        title: 'Avviso',
                        msg: 'Attenzione!! Esiste già un provvedimento con lo stesso oggetto',
                        buttons: Ext.MessageBox.YESNO,
                        icon: Ext.MessageBox.INFO,
                        fn: function(btn) {
                        if (btn == 'yes') {
                            if (flag_ChiamaCreaProvvedimento==true)
                                CreaProvvedimento() 
                        }
                        
                        }
                    })


                }
            },
            failure: function(response, options) {
                maskOggetto.hide();
                var data = Ext.decode(response.responseText);
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: data.FaultMessage,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) { return }
                });
                CreaProvvedimento()
            }
        });

    } else {
       CreaProvvedimento()
}
 
 Ext.lib.Ajax.defaultPostHeader   =stdec
 
}


var actionHomePageUfficio = new Ext.Action({
    text: 'Annulla',
    tooltip:'Annulla Provvedimento',
    id: 'btnAnnulla',
    handler: function() {
    CancellaProvvedimento()
    },
    iconCls: 'remove'
});




function CreaProvvedimentoFunction() {

   // function() {
        var result = verificaEsistenzaOggetto(true);

    
         //   CreaProvvedimento()
        //}
    //}
}

var actionCreaProvvedimento = new Ext.Action({
    text: 'Crea',
    id: 'btnCrea',
    tooltip: 'Crea Provvedimento',
    handler:CreaProvvedimentoFunction,
    iconCls: 'save'
});

function CancellaProvvedimento() {
    Ext.getDom('chkSalva').value = '0'
    window.location.href = 'HomePageAction.aspx'
}
function CreaProvvedimento() {
                            var errore='';
                            errore=verificaForm(tipo);
                                if (errore=='') { 
                                            Ext.getDom('chkSalva').value = '1'
                                            setUfficiSelezionati()
                                            Ext.getCmp('myPanel').getForm().timeout = 100000000;
                                            Ext.getCmp('myPanel').getForm().submit(
                                            {
                                                waitTitle: "Attendere...",
                                                waitMsg: 'Aggiornamento in corso ......',
                                                success: function(result, response) {
                                                    if (response.result.success == true) {
                                                        location.href = response.result.link
                                                    }
                                                    else {

                                                        Ext.MessageBox.show({
                                                            title: 'Errore',
                                                            msg: response.result.FaultMessage,
                                                            buttons: Ext.MessageBox.OK,
                                                            icon: Ext.MessageBox.ERROR,
                                                            fn: function(btn) {
                                                                return;
                                                            }
                                                        })
                                                     
                                                      
                                                    }
                                                },
                                                failure:
	                                                function(result, response) {

	                                                    Ext.MessageBox.show({
	                                                        title: 'Errore',
	                                                        msg: 'Errore nel salvataggio. Riprovare',
	                                                        buttons: Ext.MessageBox.OK,
	                                                        icon: Ext.MessageBox.ERROR,
	                                                        fn: function(btn) {
	                                                            return;
	                                                        }
	                                                    }); //fine messagebox
	                                                } //fine function
	                                            })
	                                        } else {



	                                        Ext.MessageBox.show({
	                                            title: 'Errore',
	                                            msg: errore,
	                                            buttons: Ext.MessageBox.OK,
	                                            icon: Ext.MessageBox.ERROR,
	                                            fn: function(btn) {
	                                                return;
	                                            }
	                                        });
	                                    
	                                        }  
	                               
	                                 
                            }


function gestioneClickOpContabili(obj, flagAbilita) {
    if (flagAbilita != '1') { return false } else {
   // obj.setValue(!obj.getValue())
   obj.setValue(1)
   return  gestioneCheck(obj)
    
    }

}

function gestioneCheck(obj) {

    switch (obj.id) {

        case "chkNessuna":
           Ext.getCmp("chkImpegno").setValue(false);
           Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
           Ext.getCmp("chkAccertamento").setValue(false);
           Ext.getCmp("chkRiduzione").setValue(false);
           Ext.getCmp("chkLiquidazione").setValue(false);
           Ext.getCmp("chkRiduzionePreImp").setValue(false);
           Ext.getCmp("chkRiduzioneLiq").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
            break;

        case "chkImpegno":
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkRiduzione").setValue(false);
           Ext.getCmp("chkRiduzioneLiq").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
            break;
            
         case "chkLiquidazione":
           Ext.getCmp("chkRiduzionePreImp").setValue(false);
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
           break;

        case "chkRiduzione":
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
           Ext.getCmp("chkImpegno").setValue(false);
           Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
            if (Ext.getCmp("chkLiquidazione").checked == false) {
               Ext.getCmp("chkAccertamento").setValue(false);
            }
            break;

        case "chkRiduzionePreImp":
           Ext.getCmp("chkAltro").setValue(false);
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
           break;
          
        case "chkRiduzioneLiq":
           Ext.getCmp("chkAltro").setValue(false);
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkImpegno").setValue(false);
           Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
           break;
            
       

        case "chkImpegnoSuPerenti":
           Ext.getCmp("chkAltro").setValue(false);
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkRiduzione").setValue(false);
           Ext.getCmp("chkRiduzionePreImp").setValue(false);
           Ext.getCmp("chkRiduzioneLiq").setValue(false);
           Ext.getCmp("chkAltro").setValue(false);
            break;



        case "chkAccertamento":
            if (tipo == '0') {
                if (Ext.getCmp("chkImpegno").checked ||
	                Ext.getCmp("chkLiquidazione").checked) {
                    Ext.getCmp("chkNessuna").setValue(false);
                    Ext.getCmp("chkRiduzione").setValue(false);
                    Ext.getCmp("chkRiduzionePreImp").setValue(false);
                    Ext.getCmp("chkRiduzioneLiq").setValue(false);
                } else {
                    Ext.getCmp("chkAccertamento").setValue(false);
                }
            }
            else {
                Ext.getCmp("chkNessuna").setValue(false);
            }
            Ext.getCmp("chkAltro").setValue(false);
            break;

       case "chkAltro":
           Ext.getCmp("chkNessuna").setValue(false);
           Ext.getCmp("chkImpegno").setValue(false);
           Ext.getCmp("chkImpegnoSuPerenti").setValue(false);
           Ext.getCmp("chkAccertamento").setValue(false);
           Ext.getCmp("chkRiduzione").setValue(false);
           Ext.getCmp("chkLiquidazione").setValue(false);
           Ext.getCmp("chkRiduzioneLiq").setValue(false);
           Ext.getCmp("chkRiduzionePreImp").setValue(false);
           break;


    }

    if (Ext.getCmp("chkAltro").checked)
    { Ext.getCmp("ComboSistemazioni").enable() } else { Ext.getCmp("ComboSistemazioni").disable() };

    if ((Ext.getCmp("chkNessuna").checked) || (Ext.getCmp("chkRiduzione").checked)
|| (Ext.getCmp("chkAccertamento").checked) || (Ext.getCmp("chkImpegno").checked) || (Ext.getCmp("chkImpegnoSuPerenti").checked)
|| (Ext.getCmp("chkLiquidazione").checked) || (Ext.getCmp("chkRiduzionePreImp").checked) || (Ext.getCmp("chkRiduzioneLiq").checked)|| (Ext.getCmp("chkAltro").checked)) {
        window.document.getElementById("controlloCheck").value = '1'

    } else {
        window.document.getElementById("controlloCheck").value = ''
    }
    
    
}

var valueOggetto = '';
var chkpubIntegrale = false;
var chkpubOggDisp = false;
var chkpubEstratto = false;
var opContabileVuoto = "0;0;0;0;0;0;0;";
var arrOpContabili = opContabileVuoto.split(";")
var chkNessuno = false;
var tipo = 0;
var codDocumento = '';
var etichetta= 'Carica Provvedimento';

function SetValoriProvvedimenti() {
     valueOggetto = '';
     chkpubIntegrale = false;
     chkpubOggDisp = false;
     chkpubEstratto = false;
   codDocumento=Ext.getDom('codDocumento').value
     switch (Ext.getDom('valuePub').value) {
         case '0': { chkpubIntegrale = true;  break }
         case '1': { chkpubOggDisp = true; break }
         case '2': { chkpubEstratto= true; break }
        
     }

     if (Ext.getDom('flagModificato').value != '') {

         Ext.MessageBox.show({
             title: 'Avviso',
             msg: Ext.getDom('flagModificato').value,
             buttons: Ext.MessageBox.OK,
             icon: Ext.MessageBox.INFO,
             fn: function(btn) {
                 return;
             }
         })
     
         
     }
     


   etichetta=  Ext.getDom('lblEtichetta').value

     tipo = Ext.getDom('tipo').value
     valueOggetto = Ext.getDom('valueOggetto').value
     var valueOpContabile = Ext.getDom('valueOpContabile').value
     if (valueOpContabile != '') {
         arrOpContabili = valueOpContabile.split(";")
         
     }


var tempValueOpContabile = valueOpContabile.replace(/0/g, '').replace(/;/g, '')


    // if (valueOpContabile.indexOf("1") >= 0) {
    if(tempValueOpContabile>=1){
         chkNessuno = false;
     } else {
         if (codDocumento != '') {
             chkNessuno = true;
         }

     }
  

}

Ext.onReady(function() {

    Ext.QuickTips.init();
    Ext.Ajax.timeout = 100000000;
    // turn on validation errors beside the field globally
    Ext.form.Field.prototype.msgTarget = 'side';
   
   
  //  buildComboSistemazioni()
    SetValoriProvvedimenti();
     buildComboSistemazioni(tipo);
    //    var comboDip = buildComboDipartimenti();

    var visibiTipoPub = true;
    var visibiOpConta = true;
    var abilitaRegistra = true;

    if (tipo == 2 ) {
        chkpubIntegrale = true;
        chkpubOggDisp = false;
        chkpubEstratto = false;
        visibiTipoPub = false;
    }
    if (tipo == 1 ) {
        chkpubIntegrale = true;
        chkpubOggDisp = false;
        chkpubEstratto = false;
        visibiTipoPub = false;
    }
    var pubIntegrale = new Ext.form.Radio({
        boxLabel: 'Integrale',
        checked: chkpubIntegrale,
        id: 'Integrale',
        // name: 'TipoPubblicazione',
        name: 'TipoPubblicazione',
        inputValue: '0'



    });

    var pubOggDisp = new Ext.form.Radio({
        boxLabel: 'Per Oggetto + Dispositivo',
        id: 'oggDisp',
        checked: chkpubOggDisp,
        //  name: 'TipoPubblicazione',
        name: 'TipoPubblicazione',
        inputValue: '1'

    });

    var pubEstratto = new Ext.form.Radio({

        boxLabel: 'Estratto Oggetto',
        id: 'estratto',
        checked: chkpubEstratto,
        //  name: 'TipoPubblicazione',
        name: 'TipoPubblicazione',
        inputValue: '2'

    });

    if (codDocumento != '') {
        //    Ext.getCmp('btnCrea').destroy()
        
        actionCreaProvvedimento = new Ext.Action({
            text: 'Salva',
            id: 'btnCrea',
            tooltip: 'Salva Provvedimento',
            handler: function() {
                CreaProvvedimento()
            },
            iconCls: 'save'
        });



        actionCreaProvvedimento.setText('Salva')
        Ext.getDom('controlloCheck').value = '1'
    }

    if  ((codDocumento == '') && (tipo == '2')) {
        arrOpContabili[2] = '1';
        chkNessuno = false;
        Ext.getDom('controlloCheck').value = '1';

    }
    if  ((codDocumento == '') && (tipo == '1')) {
        Ext.getDom('controlloCheck').value = '0';
    }

    var myPanel = new Ext.FormPanel({
        id: 'myPanel',
        url: 'CreaProvvedimento.aspx' + window.location.search,
        tbar: [actionCreaProvvedimento, actionHomePageUfficio],
        labelAlign: 'top',
        title: etichetta,
        bodyStyle: 'padding:5px',
        width: 800,
        items: [{
            layout: 'column',
            border: false,
            items: [{
                columnWidth: 1,
                layout: 'form',
                height: 155,
                border: false,
                items: [{ id: "chkSalva",
                    xtype: "hidden"
                }, {
                    height: 130,
                    xtype: 'textarea',
                    fieldLabel: 'Oggetto',
                    name: 'txtOggetto',
                    id: 'txtOggetto',
                    anchor: '100%',
                    value: valueOggetto,
                    onBlur: function() {
                        if (codDocumento == '') {
                          //  verificaEsistenzaOggetto()
                            //           } else {
                            //            return true
                        }
                    }

}]
}]
                }, {
                    xtype: 'tabpanel',
                    plain: true,
                    activeTab: 0,
                    autoHeight: true,
                    defaults: { bodyStyle: 'padding:10px' },
                    items: [{
                        title: 'Tipo Pubblicazione',
                        id: 'tab_pub',
                        autoHeight: true,
                        defaults: { labelSeparator: '' },
                        layout: 'form',
                        items: [pubIntegrale, pubOggDisp, pubEstratto]
                    }, {
                        title: 'Operazioni Contabili',
                        id: 'tab_cont',
                        layout: 'form',
                        autoHeight: true,
                        width: 500,
                        defaultType: 'checkbox',
                        defaults: { labelSeparator: '' },
                        items: [{
                            labelAlign: 'right',
                            boxLabel: 'Nessuna',
                            name: 'chkNessuna',
                            id: 'chkNessuna',
                            labelSeparator: '',
                            checked: chkNessuno,
                            onClick: function() {
                           return  gestioneClickOpContabili(this, visibiOpConta);
                            
                            }
                        }, {
                            boxLabel: 'Impegno ed eventuale liquidazione contestuale',
                            name: 'chkImpegno',
                            id: 'chkImpegno',
                            checked: arrOpContabili[0] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Impegno Su Perenti e liquidazione contestuale',
                            name: 'chkImpegnoSuPerenti',
                            id: 'chkImpegnoSuPerenti',
                            checked: arrOpContabili[1] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Liquidazione su Impegno precedentemente assunto con altro provvedimento',
                            name: 'chkLiquidazione',
                            id: 'chkLiquidazione',
                            checked: arrOpContabili[2] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Accertamento',
                            name: 'chkAccertamento',
                            id: 'chkAccertamento',
                            checked: arrOpContabili[3] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Riduzione ( Impegno Anno Corrente ) / Economia ( Impegno Anni Precedenti)',
                            name: 'chkRiduzione',
                            id: 'chkRiduzione',
                            checked: arrOpContabili[4] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Riduzione Preimpegno da Delibera( Preimpegno Anno Corrente )',
                            name: 'chkRiduzionePreImp',
                            id: 'chkRiduzionePreImp',
                            checked: arrOpContabili[5] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        }, {
                            boxLabel: 'Riduzione Liquidazioni',
                            name: 'chkRiduzioneLiq',
                            id: 'chkRiduzioneLiq',
                            checked: arrOpContabili[6] == '1',
                            onClick: function() {
                                return gestioneClickOpContabili(this, visibiOpConta);
                            }
                        },
                          {
                              boxLabel: 'Rettifiche Contabili',
                              name: 'chkAltro',
                              id: 'chkAltro',
                              checked: (arrOpContabili[7] != '0' && arrOpContabili[7] != ''),
                              onClick: function() {
                                  return gestioneClickOpContabili(this, visibiOpConta);
                              }
}]

                    }, { title: 'Notifica ad altri Uffici', autoHeight: true, defaults: { autoHeight: true }, layout: 'form', id: 'tab_ufficio' }
                            ]
}]
                    //,
                    //                        buttons: [{
                    //                            text: 'Save',
                    //                            onClick: CreaProvvedimento
                    //                        }, {
                    //                            text: 'Cancel',
                    //                            onClick: CancellaProvvedimento
                    //}]
                });

                Ext.getCmp('tab_ufficio').on('activate', function() {
                    Ext.getCmp('tab_ufficio').doLayout();
                    buildCopyDrop()
                })

                // ComboSistemazioni.doLayout();
                //ComboSistemazioni.show();


                if (Ext.getDom('flagAbilitaOpContabili').value != '1') {
                    visibiOpConta = false
                }





                buildOnReadyUffici()
                Ext.getCmp('myPanel').render("myPanelPrincipale")

                if (Ext.getDom('flagRegistra').value != '1') {
                    Ext.getCmp("btnCrea").disable();

                }

                if ((tipo == '2') || (tipo == '1')) {
              
                    if (tipo == '2'){
                    //CASO DISPOSIZIONE
                        Ext.getCmp("tab_cont").insert(0, Ext.getCmp("chkLiquidazione"))
                        Ext.getCmp("tab_cont").insert(1, Ext.getCmp("chkAccertamento"))
                        Ext.getCmp("tab_cont").insert(2,Ext.getCmp("chkAltro"))
                        Ext.getCmp("tab_cont").insert(3, Ext.getCmp("ComboSistemazioni"))
                    }
                    
                     if (tipo == '1'){
                     //CASO DELIBERA
                      visibiOpConta = false;
                    }
                    Ext.getCmp("chkImpegno").hide();
                    Ext.getCmp("chkImpegnoSuPerenti").hide();
                    Ext.getCmp("chkRiduzione").hide();
                    Ext.getCmp("chkNessuna").hide();
                    Ext.getCmp("chkRiduzioneLiq").hide();
                    Ext.getCmp("chkRiduzionePreImp").hide();
                }
                else{
                
                    //Caso DETERMINA
                    Ext.getCmp("tab_cont").insert(Ext.getCmp("tab_cont").items.length, Ext.getCmp("ComboSistemazioni"))
                }
          //      var combo = buildComboSistemazioni();
                
                Ext.getCmp("ComboSistemazioni").disable();
                if (arrOpContabili[7] != '0' && arrOpContabili[7] != '') {
                    Ext.getCmp("ComboSistemazioni").enable();
                }
 




                if (!visibiTipoPub) {
                    Ext.getCmp("tab_pub").disable();
                    //Ext.getCmp("tab_pub").destroy();
                    Ext.getCmp('myPanel').active = Ext.getCmp("tab_cont")
                    Ext.getCmp('myPanel').active.show();

                }
                if (!visibiOpConta) {
                    Ext.getCmp("tab_cont").disable();
                    //Ext.getCmp("tab_pub").destroy();
                    // Ext.getCmp('myPanel').active = Ext.getCmp("tab_cont")
                    //Ext.getCmp('myPanel').active.show();

                }
                if ((!visibiTipoPub) && (!visibiOpConta)) {
                    Ext.getCmp('myPanel').active = Ext.getCmp("tab_ufficio")
                    Ext.getCmp('myPanel').active.show();
                }





            });











function buildOnReadyUffici() {

  
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
               { name: 'DescrizioneBreve' }
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
            },
            scope: this
        }
    });
    storeRegistrato.load()
  
    builFormPanelUfficiCompetenzaRegistrati(storeRegistrato);

    Ext.getCmp('tab_ufficio').add(Ext.getCmp('myPanelFiltroDipartimento'));

    Ext.getCmp('tab_ufficio').add(buildPanelPrincipale())


}

function verificaForm(tipo) {
    var lstr_errore = ''

    if (Ext.getCmp('myPanel').getForm().getValues().txtOggetto == '') {
        lstr_errore += '- Inserire Oggetto \n'
    }
    if (tipo !=1 ){
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_cont")
        Ext.getCmp('myPanel').active.show();
        Ext.getCmp('myPanel').active = Ext.getCmp("tab_ufficio")
        Ext.getCmp('myPanel').active.show();

        Ext.getCmp('myPanel').active = Ext.getCmp("tab_pub")
        Ext.getCmp('myPanel').active.show();
      
        if (Ext.getCmp('myPanel').getForm().getValues().TipoPubblicazione == undefined) {
            lstr_errore += '- Selezionare Tipo Pubblicazione \n'
        }
        
        if (Ext.getDom('controlloCheck').value != '1') {
            lstr_errore +='- Selezionare Operazione Contabile \n'
        }

        if (Ext.getCmp("chkAltro")!=undefined){
            if (Ext.getCmp("chkAltro").checked) {
                if (Ext.getDom('ValueSistemazioni').value==''){
                   lstr_errore += 'Selezionare Rettifiche'
                }
            }
        }
    }
    return lstr_errore
}                       