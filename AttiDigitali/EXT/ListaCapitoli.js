//var mask;
//mask = new Ext.LoadMask(Ext.getBody(), {
//    msg: "Recupero Dati..."
//});



//DEFINISCO LA FUNZIONE PER INIZIALIZZARE LA POPUP
function InitFormCapitoliLista(codice_Ufficio) {

 var AnnoBilancioImpegno = new Ext.ux.YearMenu({
        format: 'Y',
        id: 'AnnoBilancio',
        allowBlank: false,
        noPastYears: false,
        noPastMonths: true,
        minDate: new Date('1990/1/1'),
        maxDate: new Date('2069/1/1')
			       ,
        handler: function(dp, newValue, oldValue) {
            Ext.getCmp('AnnoIni').value = newValue.format('Y');
            Ext.get('AnnoIni').dom.value = newValue.format('Y');
        }
    });

//
//    var AnnoBilancioImpegno = new Ext.form.DateField({
//                              
//                                id: 'AnnoBilancio',
//                                fieldLabel: 'Choose month',
//                                plugins: 'monthPickerPlugin',
//                                format: 'Y',
//                                editable: true,
//                                readOnly: false,
//                                  
//        handler: function(dp, newValue, oldValue) {
//            Ext.getCmp('AnnoIni').value = newValue.format('Y');
//            Ext.get('AnnoIni').dom.value = newValue.format('Y');
//        }
//                                });

//   

    var btnRicercaImpegno = new Ext.Button({
        text: '<font color="#000066"><b>--->> Visualizza capitoli</b></font>',
        id: 'btnBilancio'
    });

    btnRicercaImpegno.on('click', function() {

        var fNewAnno = Ext.getCmp("AnnoIni").value;
        //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
        gridCapitoli = buildGridStartLista(fNewAnno,codice_Ufficio);
        capitoloPanelImpegno.remove('GridCapitoli')
        capitoloPanelImpegno.add(gridCapitoli);
        capitoloPanelImpegno.doLayout();
    });

    var tbarDateImpegno = new Ext.Toolbar({
        style: 'margin-bottom:-1px;',
        width: 700,
        items: [{ xtype: 'button',
            text: "<font color='#0000A0'><b>Bilancio</b></font>",
            id: 'SelBil',
            menu: AnnoBilancioImpegno
        },
//        {
//        xtype: 'datefield',
//        id: 'AnnoIni',
//        plugins: 'monthPickerPlugin',
//        format: 'Y',
//        editable: true,
//        readOnly: false,
//        value: new Date().format('Y')

//        },
        			 {
        			     xtype: 'textfield',
        			     cls: 'titfis',
        			     readOnly: true,
        			     id: 'AnnoIni',
        			     width: 80,
        			     value: new Date().format('Y')
        			 },
			    btnRicercaImpegno
			 ]
    });

    //DEFINISCO IL PANNELLO CONTENENTE LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
    var capitoloPanelImpegno = new Ext.Panel({
        xtype: "panel",
        title: "",
        autoHeight: true
    });

      //DEFINISCO LA FORM CHE CONTIENE TUTTI GLI ELEMENTI CHE COMPONGONO IL PANNELLO "ELENCO CAPITOLI PER IMPEGNO"
            var formCapitoliImpegno = new Ext.FormPanel({
                id: 'ElencoCapitoli',
                frame: true,
                labelAlign: 'left',
                title: 'Elenco Capitoli',
                bodyStyle: 'padding:5px',
                collapsible: true,   // PERMETTE DI ICONIZZARE LA FORM
                width: 800,
                layout: 'column', // SPECIFICA CHE IL CONTENUTO VIENE MESSO IN COLONNE
                items: [{
                    columnWidth: 0.9,
                    layout: 'fit',
                    tbar: [tbarDateImpegno
        		],
                   items: [capitoloPanelImpegno]
                }]
            });

            
            //PRENDO L'ANNO DALLA DATA ODIERNA
            var dtOggi = new Date()
            dtOggi.setDate(dtOggi.getDate());
            var fAnno = dtOggi.getFullYear();

            //RICHIAMO LA FUNZIONE PER RIEMPIRE LA GRIGLIA CON L'ELENCO CAPITOLI PER IMPEGNO
            var gridCapitoli = buildGridStartLista(fAnno,codice_Ufficio);
            capitoloPanelImpegno.add(gridCapitoli);
            // SETTO panelImp INVISIBILE 
        Ext.getCmp('ElencoCapitoli').render("Lista")

        } // fine InitFormCapitoli

        //FUNZIONE CHE RIEMPIE LO STORE PER LA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
        function buildGridStartLista(annoRif, codice_Ufficio) {
       var maskApp;
maskApp = new Ext.LoadMask(Ext.getBody(), {
    msg: "Recupero Dati..."
});
            var proxy = new Ext.data.HttpProxy({
                url: 'ProcAmm.svc/GetElencoCapitoliAnno',
                method: 'GET'
            });
            var reader = new Ext.data.JsonReader({

                root: 'GetElencoCapitoliAnnoResult',
                fields: [
           { name: 'Bilancio' },
           { name: 'UPB' },
           { name: 'MissioneProgramma' },
           { name: 'Capitolo' },
           { name: 'ImpDisp' },
           { name: 'ImpPrenotato' },
           { name: 'NumPrenotazione' },
           { name: 'AnnoPrenotazione' },
           { name: 'DescrCapitolo' },
           { name: 'CodiceRisposta' },
           { name: 'DescrizioneRisposta' }
           ]
            });

            var store = new Ext.data.Store({
                proxy: proxy
		, reader: reader
                , listeners: {
                    'loadexception': function (proxy, options, response) {
                        maskApp.hide();
                        Ext.MessageBox.show({
                            title: 'ERRORE CAPITOLI',
                            msg: "Errore durante il caricamento dei capitoli:<br>" +
                                 "'" + Ext.decode(response.responseText).FaultMessage + "'",
                            buttons: Ext.Msg.OK,
                            closable: false,
                            icon: Ext.MessageBox.ERROR
                        });
                    }
                }
            });

            var parametri = { AnnoRif: annoRif, tipoCapitolo: '2' , codiceUfficio: codice_Ufficio };
            store.load({ params: parametri });

            store.on({
                'load': {
                    fn: function(store, records, options) {
                        maskApp.hide();
                    },
                    scope: this
                }
            });

            
            function renderColor(val, p, record) {
                var color = '';
                if (record.data.CodiceRisposta == 0) {
                    if (record.data.DescrizioneRisposta.toUpperCase().indexOf("NESSUN") > -1) {
                        color = '#298A08';
                    } else {
                        color = '#FE9A2E';
                    }
                }
                else {
                    color = 'red';
                }
                return '<p style="font-size:12px;color:' + color + ';">' + record.data.DescrizioneRisposta + '</p>';
            }  

            //DEFINISCO LA GRIGLIA ELENCO CAPITOLI PER IMPEGNO
            var ColumnModel = new Ext.grid.ColumnModel([
	                { header: "Bilancio", width: 60, dataIndex: 'Bilancio', id: 'Bilancio', sortable: true },
	               	{ header: "Capitolo", width: 60, dataIndex: 'Capitolo', sortable: true, locked: false },
                    { header: "UPB", width: 60, dataIndex: 'UPB', sortable: true },
                    { header: "Missione.Programma", width: 60, dataIndex: 'MissioneProgramma', sortable: true },
	                { header: "Descrizione", width:230, dataIndex: 'DescrCapitolo', sortable: true, locked: false },
	            	{ renderer: eurRend, header: "Importo<br/>Disponibile", width: 90, dataIndex: 'ImpDisp', sortable: true },
	            	{ header: "Blocco", width: 120, dataIndex: 'DescrizioneRisposta', sortable: true, locked: false, renderer: renderColor }
	            	]);

            var grid = new Ext.grid.GridPanel({
                id: 'GridCapitoli',
               autoExpandColumn: 'Descrizione',
                // da mettere sempre 
                height: 350,
                width: 690,
                title: '',
                border: true,
                 viewConfig: { forceFit: true },
                ds: store,
                cm: ColumnModel,
                stripeRows: true,
                loadMask: true,
                // istruzioni per abilitazione Drag & Drop		          
                enableDragDrop: false,
                ddGroup: 'gridDDGroup'
                // fine istruzioni per abilitazione Drag & Drop		          
                });

            //AGGIUNGO GESTIONE EVENTO DBLCLICK SULLA GRIGLIA
            return grid;
        }
        //FUNZIONE CHE RIEMPIE IL CAMPO "IMPORTO DISPONIBILE" NELLA GRIGLIA "ELENCO CAPITOLI PER IMPEGNO"
        function getImpDisp(rec, capImpDisp, bilancioImpDisp) {
         var ufficio =  Ext.get('Cod_uff_Prop').dom.value;
   
            var params = { Capitolo: capImpDisp, Bilancio: bilancioImpDisp, ufficioProponente: ufficio };

            Ext.lib.Ajax.defaultPostHeader = 'application/json';
            Ext.Ajax.request({
                url: 'ProcAmm.svc/GetImpDispCapitolo_post',
                params: Ext.encode(params),
                method: 'POST',
                success: function(response, options) {
                    maskApp.hide();
                    var data = Ext.decode(response.responseText);
                    var ImpDisponibile = data.GetImpDispCapitolo_postResult.ImportoDisponibile;
                    rec.set('ImpDisp', ImpDisponibile)
                },
                failure: function(response, options) {
                    maskApp.hide();


                    var data = Ext.decode(response.responseText);
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: data.FaultMessage,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) { return }
                    });

                }
            });
        }

