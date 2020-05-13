function callSubmit() {
   
    Ext.getCmp("btnLogin").handler.call(Ext.getCmp("btnLogin").scope);
}
Ext.onReady(function() {
    Ext.QuickTips.init();
    var labelCF = "";
    var labelSep = ":";
    labelCF = "Cod. Fiscale";
    var nascondiCF = false;

    var login = new Ext.FormPanel({
        labelWidth: 90,
        id: 'LoginCF',
        frame: true,
        width: 320,
        autoHeight: true,
       // padding: 200,
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
    fieldLabel: labelCF,
    name: 'cf',
    width: 190,
    id: 'cf',
    invalidText: 'Codice Fiscale Non Valido',
    labelSeparator: labelSep,
    validator: function(valu) {

        var pattern = /[a-zA-Z]{6}\d\d[a-zA-Z]\d\d[a-zA-Z]\d\d\d[a-zA-Z]/
        var stringa = valu.trim()
        var result = stringa.search(pattern)


        return result > -1;
    },
    validateOnBlur: false,
    allowBlank: false,
    autocomplete: "on",
    listeners: {
        specialkey: function(field, e) {
            if (e.getKey() == Ext.EventObject.ENTER) {
                callSubmit()
            }
        }
    }

}, {
    fieldLabel: 'Password',
    name: 'codPwd',
    width: 190,
    inputType: 'password',
    id: 'pass',
    allowBlank: false,
    listeners: {
        specialkey: function(field, e) {
        if (e.getKey() == Ext.EventObject.ENTER) {
            callSubmit()
           }
        }
    }
}
],

        buttons: [{
            text: 'Login',
            id:'btnLogin',
            handler: function() {
                var urlAction = 'Login_CFAction.aspx';
                login.getForm().timeout = 100000000;
                login.getForm().submit({
                    url: urlAction,
                    waitTitle: "Attendere...",
                    waitMsg: 'Autenticazione in corso ......',
                    success: function(result, response) {

                        location.href = response.result.link;

                    },
                    failure: function(result, response, options) {
                        var messaggio = 'Verifica i dati inseriti';
if (response.result != undefined) {
    messaggio = response.result.FaultMessage   
                        }
                        Ext.MessageBox.show({
                            title: 'Errore',
                            msg: messaggio,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.ERROR,
                            fn: function(btn) { return }
                        });
                    }



                }) // FINE SUBMIT

            }
        },
{
    text: 'Reset',
    handler: function() {
        login.getForm().reset();
    }

}]
    });


    login.render("login");
});
