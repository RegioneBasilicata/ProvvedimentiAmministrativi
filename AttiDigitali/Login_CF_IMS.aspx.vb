﻿Public Class Login_CF_IMS
    Inherits System.Web.UI.Page
    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblEsitoAutenticazione As System.Web.UI.WebControls.Label
    Protected WithEvents hiddenCF As Global.System.Web.UI.WebControls.HiddenField
    Protected WithEvents flagNascondiCF As Global.System.Web.UI.WebControls.HiddenField

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Login")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '   Inizializza_Pagina(Me, "Login")
        hiddenCF.Value = Request("CF")
        flagNascondiCF.Value = "0"
    End Sub

End Class