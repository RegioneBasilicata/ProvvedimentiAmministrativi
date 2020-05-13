function getWaitingPanelResource(resourceName)
{
    return Ext.getDom(resourceName) != null && Ext.getDom(resourceName) != undefined ? (Ext.getDom(resourceName).value!=null && Ext.getDom(resourceName).value!=undefined ? Ext.getDom(resourceName).value : null) : null;    
}

function buildWaitingPanel(title, msg) {
    var tmpTitle = (title == null || title == undefined || title == '') ? "Attendere" : title;
    var tmpMsg = (msg == null || msg == undefined || msg == '') ? "Operazione in corso..." : msg;

    var waitingPanel = {
        'showLoading': function() {
            Ext.MessageBox.show({
                title: tmpTitle,
                msg: tmpMsg,
                width: 300,
                wait: true,
                waitConfig: { interval: 350 }
            });
        },
        'hideLoading': function() {
            Ext.MessageBox.hide();
        }
    };

    return waitingPanel;
}


Ext.onReady(function() {
    var waitingPanelTitle = getWaitingPanelResource("waitingPanelTitle");
    var waitingPanelMsg = getWaitingPanelResource("waitingPanelMsg");
    var waitingPanelActionUrl = getWaitingPanelResource("waitingPanelActionUrl");

    var emptyPanel = new Ext.Panel({
        xtype: 'panel',
        id: 'emptyPanel'
    });

    emptyPanel.hide();
    Ext.getCmp('emptyPanel').render("myPanelPrincipale")

    waitingPanel = buildWaitingPanel(waitingPanelTitle, waitingPanelMsg);
    waitingPanel.showLoading();
    if (waitingPanelActionUrl) {

        var conn = new Ext.data.Connection();

        conn.request({
            url: waitingPanelActionUrl,
            timeout: 360000,
            method: 'post',
            success: function(response) {
                if (Ext.decode(response.responseText).success == true) {
                    var message = Ext.decode(response.responseText).message;
                    if (!(message == null || message == undefined || message == "")) {
                        location.href = Ext.decode(response.responseText).link;
                        Ext.MessageBox.show({
                            title: 'Operazione completata',
                            msg: Ext.decode(response.responseText).message,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.INFO,
                            fn: function(btn) {
                                
                            }
                        });
                    } else
                        location.href = Ext.decode(response.responseText).link;
                }
                else {
                    Ext.MessageBox.show({
                        title: 'Errore',
                        msg: Ext.decode(response.responseText).message,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR,
                        fn: function(btn) {
                            history.back();
                        }
                    })
                }
            },
            failure: function(response) {
                Ext.MessageBox.show({
                    title: 'Errore',
                    msg: 'Operazione interrotta. Riprovare.',
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.ERROR,
                    fn: function(btn) {
                        history.back();
                        return;
                    }
                });
            }
        });
    }
    else
        waitingPanel.hideLoading();

});

