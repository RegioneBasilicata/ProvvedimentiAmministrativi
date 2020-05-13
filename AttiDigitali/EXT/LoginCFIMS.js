
function callSubmit() {
    //Ext.getCmp('btnLogin').click();
    //alert('');
    //Ext.getCmp('btnLogin').onClick()

    Ext.getCmp("btnLogin").handler.call(Ext.getCmp("btnLogin").scope);
}
Ext.onReady(function () {
    Ext.QuickTips.init();
    var valueCF = Ext.getDom('hiddenCF').value;
    var valueFlagCF = Ext.getDom('flagNascondiCF').value;
    var labelCF = "";
    var labelSep = ":";
    labelCF = "Cod. Fiscale";
    var nascondiCF = false;

    if (valueFlagCF == "1") {
        labelCF = ""
        nascondiCF = true
        labelSep = ""
    }
    // alert(nascondiCF)


    var login = new Ext.FormPanel({
        labelWidth: 90,
        id: 'myPanel',
        name: 'myPanel',
        //   el: 'myPanel',
        // url: 'Login_CF_IMSAction.aspx',
        frame: true,
        width: 320,
        //autoHeight: true,
        autoHeight: true,
        //  padding: 200,
        defaultType: 'textfield',
        title: "Login",
        items: [{
            xtype: 'box', //create image
            autoEl: {
                tag: 'img',
                src: './risorse/immagini/im48x48.png'
            }
        },
        {
            fieldLabel: 'Username',
            name: 'codUtente',
            id: 'codUtente',            
    width: 190,
    allowBlank: false,
    listeners: {
        specialkey: function (field, e) {
            if (e.getKey() == Ext.EventObject.ENTER) {
                callSubmit();
            }
        }
    }
}, {
    fieldLabel: 'Password',
    name: 'codPwd',
    id: 'codPwd',
    width: 190,    
    inputType: 'password',
    id: 'pass',
    allowBlank: false,
    listeners: {
        specialkey: function (field, e) {
            if (e.getKey() == Ext.EventObject.ENTER) {
                callSubmit();
            }
        }
    }
}, {
    fieldLabel: labelCF,
    name: 'cf',
    width: 190,
    // inputType: 'cf',
    id: 'cf',
    disabled: nascondiCF,
    hidden: nascondiCF,
    invalidText: 'Codice Fiscale Non Valido',
    value: valueCF,
    labelSeparator: labelSep,
    validator: function (valu) {

        var pattern = /[a-zA-Z]{6}\d\d[a-zA-Z]\d\d[a-zA-Z]\d\d\d[a-zA-Z]/;
        var stringa = valu.trim();
        var result = stringa.search(pattern);


        return result > -1;
    },
    validateOnBlur: false,
    allowBlank: false,
    listeners: {
        specialkey: function (field, e) {
            if (e.getKey() == Ext.EventObject.ENTER) {
                callSubmit();
            }
        }
    }
}],

        buttons: [{
            text: 'Login',
            id: 'btnLogin',
            handler: function () {
                var urlAction = 'Login_CF_IMSAction.aspx';
                if (Ext.getDom('flagNascondiCF').value == "1") {
                    urlAction = 'LoginAction.aspx';
                }
                login.getForm().timeout = 100000000;
                login.getForm().submit({
                    url: urlAction,
                    waitTitle: "Attendere...",
                    waitMsg: 'Autenticazione in corso ......',
                    success: function (result, response) {
                        //  if (response.result.success == true) {
                        location.href = response.result.link;
                        //}

                    },
                    failure: function (result, response, options) {

                        var messaggio = 'Verifica i dati inseriti';
                        if (response.result != undefined) {
                            messaggio = response.result.FaultMessage;
                        }
                        Ext.MessageBox.show({
                            title: 'Errore',
                            msg: messaggio,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.ERROR,
                            fn: function (btn) { return }
                        });
                    }



                }) // FINE SUBMIT

            }
        },
{
    text: 'Reset',
    handler: function () {
        login.getForm().reset();
    }

}]
    });



    //       var createwindow = new Ext.Window({
    //           frame: true,
    //           title: 'Login',
    //           width: 330,
    //           height: 200,
    //           closable: false,
    //           items: login
    //       });

    //   createwindow.show();

    // login.getForm().on('keypress', onKeypress, this);

    login.render("login");

});
