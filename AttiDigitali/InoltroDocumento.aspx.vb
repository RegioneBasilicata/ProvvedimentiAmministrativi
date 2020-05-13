Imports System.Collections.Generic
Imports DllDocumentale

Partial Class InoltroDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnInoltra As System.Web.UI.WebControls.Button
    Protected WithEvents btnFirma As System.Web.UI.WebControls.Button
    Protected WithEvents pnlInoltro As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlNote As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSupervisore As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LblErrore As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Invio Provvedimento")
    End Sub

#End Region
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(InoltroDocumento))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim vR As Object = Nothing
        Try

            If Not IsPostBack Then
                'modgg 10-06 1

                Dim tipoDoc As String = TipoApplic(Context)
                Dim flusso As String = DefinisciFlusso(tipoDoc)
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                Dim key As String = ""
                If Not Request.QueryString.Get("key") Is Nothing Then
                    key = Request.QueryString.Get("key")
                End If


                If (oOperatore.Test_Ruolo("DL002") And tipoDoc = 1) Or
                    (oOperatore.Test_Ruolo("DT002") And tipoDoc = 0) Or
                    (oOperatore.Test_Ruolo("DS015") And tipoDoc = 2) Or
                    (oOperatore.Test_Ruolo("DR002") And tipoDoc = 3) Or
                    (oOperatore.Test_Ruolo("DO002") And tipoDoc = 4) Then

                    ddlSupervisore.Visible = False

                    If oOperatore.oUfficio.Test_Attributo("UFFICIO_SEGRETERIA", "1") And
                        oOperatore.Test_Attributo("LIVELLO_UFFICIO", "SUPERVISORE") Then

                        vR = hashToArray(oOperatore.oUfficio.SupervisoriUfficio(flusso))
                        'ddlSupervisore.Visible = True
                        lblDestinatarioInoltro.Text = "Seleziona l'assessore o l'ufficio a cui inoltrare"
                        Label3.Text = Label3.Text & " o direttamente all'ufficio segreteria generale di giunta "
                        Label1.Text = Label1.Text & " o direttamente all'ufficio segreteria generale di giunta"
                        If Not UBound(vR, 2) = -1 Then

                            If Not String.IsNullOrEmpty(oOperatore.oUfficio.ResponsabileUfficio(flusso)) Then

                                Dim lItem2 As New WebControls.ListItem

                                lItem2.Value = oOperatore.oUfficio.ResponsabileUfficio(flusso)
                                lItem2.Text = "Assessore"

                                cmbDestinatarioInoltro.Items.Add(lItem2)

                            End If
                        End If

                        Dim lItemSegreteriaGeneraleGiunta As WebControls.ListItem = New WebControls.ListItem
                        lItemSegreteriaGeneraleGiunta.Value = 0
                        lItemSegreteriaGeneraleGiunta.Text = "Segreteria Generale della Giunta"
                        cmbDestinatarioInoltro.Text = "Inoltra al Responsabile o direttamente alla Segreteria Generale di Giunta"
                        cmbDestinatarioInoltro.Items.Add(lItemSegreteriaGeneraleGiunta)
                    End If

                    If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4LIVELLO") Or oOperatore.Test_Attributo("LIVELLO_UFFICIO", "COLLABORATORE") Then
                        Dim supervisoreDefault As String
                        supervisoreDefault = oOperatore.Attributo("SUPERVISORE_DEFAULT")
                        'leggo i supervisori dell'ufficio
                        If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4LIVELLO") Then
                            vR = hashToArray(oOperatore.oUfficio.CollaboratoriUfficio(flusso))
                            If Not UBound(vR, 2) = -1 Then
                                caricaDropDaArray(vR, supervisoreDefault)
                            End If
                        End If
                        vR = hashToArray(oOperatore.oUfficio.SupervisoriUfficio(flusso))


                        If Not UBound(vR, 2) = -1 Then
                            caricaDropDaArray(vR, supervisoreDefault)

                            If Not String.IsNullOrEmpty(oOperatore.oUfficio.ResponsabileUfficio(flusso)) Then

                                Dim lItem2 As New WebControls.ListItem

                                lItem2.Value = oOperatore.oUfficio.ResponsabileUfficio(flusso)
                                lItem2.Text = "Dirigente dell'ufficio"

                                If supervisoreDefault = lItem2.Value Then
                                    lItem2.Selected = True
                                End If

                                ddlSupervisore.Visible = True
                                ddlSupervisore.Items.Add(lItem2)

                            End If
                        Else
                            ddlSupervisore.Visible = False
                        End If

                    End If
                    Contenuto.Controls.Add(pnlInoltro)

                    If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                        ddlSuggerimenti.Visible = False
                        lblSuggerimento.Visible = False
                        If Not oOperatore.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") Then
                            Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)

                            Dim listaSuggerimenti As Generic.List(Of DllDocumentale.ItemSuggerimentoInfo) = ojbSvrdocumenti.FO_Get_Suggerimenti(Nothing)

                            Dim lItem1 As WebControls.ListItem


                            For Each suggerimento As DllDocumentale.ItemSuggerimentoInfo In listaSuggerimenti
                                ddlSuggerimenti.Visible = True
                                lItem1 = New WebControls.ListItem
                                lItem1.Value = suggerimento.Id
                                lItem1.Text = suggerimento.Descrizione

                                ddlSuggerimenti.Items.Add(lItem1)
                            Next
                            lItem1 = New WebControls.ListItem
                            lItem1.Value = -1
                            lItem1.Text = "Nessun Suggerimento"
                            lItem1.Selected = True
                            ddlSuggerimenti.Items.Add(lItem1)

                            lblSuggerimento.Visible = True
                            Contenuto.Controls.Add(pnlSuggerimento)
                        End If
                    End If
                    cmbDestinatarioInoltro.Visible = True

                    If tipoDoc <> 1 Then
                        Dim lItemDirigenza As WebControls.ListItem = New WebControls.ListItem
                        lItemDirigenza.Value = 0
                        lItemDirigenza.Text = "Dirigenza Generale"
                        cmbDestinatarioInoltro.Items.Add(lItemDirigenza)
                    End If


                    Dim lItemSuccessivo As WebControls.ListItem = New WebControls.ListItem
                    If (tipoDoc = 0) Then
                        lItemSuccessivo.Value = 1
                        lItemSuccessivo.Text = "Uff. Controllo Amministrativo"
                    ElseIf (tipoDoc = 2) Then
                        lItemSuccessivo.Value = 2
                        lItemSuccessivo.Text = "Uff. Ragioneria"
                    End If

                    If tipoDoc <> 1 Then
                        cmbDestinatarioInoltro.Items.Add(lItemSuccessivo)
                    End If

                    If (oOperatore.oUfficio.Test_Attributo("SCEGLI_DEST_INOLTRO", True) And oOperatore.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE")) Then
                        cmbDestinatarioInoltro.Visible = True
                        lblDestinatarioInoltro.Visible = True
                    Else
                        cmbDestinatarioInoltro.Visible = False
                        lblDestinatarioInoltro.Visible = False
                    End If

                    If oOperatore.oUfficio.Test_Attributo("UFFICIO_SEGRETERIA", "1") And
                        oOperatore.Test_Attributo("LIVELLO_UFFICIO", "SUPERVISORE") Then
                        cmbDestinatarioInoltro.Visible = True
                        lblDestinatarioInoltro.Visible = True
                    End If


                    If (oOperatore.Test_Attributo("SCEGLI_URGENZA", True)) Then
                        If key <> "" Then
                            Dim listaDocAttributo As Generic.List(Of Documento_attributo) = Elenco_Attributi_Urgente(key)
                            Dim urgente As Integer = 0
                            If Not listaDocAttributo Is Nothing Then
                                For Each item As Documento_attributo In listaDocAttributo
                                    If item.Valore = "True" Then
                                        urgente = urgente + 1
                                    End If
                                Next
                            End If
                            If urgente = 1 Then
                                radioIsUrgent.Checked = True
                            End If
                        End If
                        lblPriorita.Visible = True
                        radioIsNotUrgent.Visible = True
                        radioIsUrgent.Visible = True
                    Else
                        lblPriorita.Visible = False
                        radioIsNotUrgent.Visible = False
                        radioIsUrgent.Visible = False
                    End If
                Else
                    pnlInoltro.Visible = False
                End If

                Contenuto.Controls.Add(pnlNote)

                If Not Context.Request.Params.Item("key") Is Nothing Or Context.Session.Item("key") Then
                    Context.Session.Remove("elencoDocumentiDaInoltrare")
                    gestionePulsanti()
                Else
                    If Not Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Then
                        gestionePulsantiMultiplo()
                    Else
                        Throw New Exception("E' necessario selezionare almeno un provvedimento prima di inoltrare")
                    End If
                End If

                If oOperatore.Test_Attributo("SCEGLI_URGENZA", True) AndAlso HttpContext.Current.ApplicationInstance.Session.Item("prioritaMixed") = True Then
                    Dim csname As [String] = "PopupScript"
                    Dim cstype As Type = Me.[GetType]()
                    ' Get a ClientScriptManager reference from the Page class. 
                    Dim cs As ClientScriptManager = Page.ClientScript
                    ' Check to see if the startup script is already registered. 
                    If Not cs.IsStartupScriptRegistered(cstype, csname) Then
                        Dim cstext As New System.Text.StringBuilder()
                        cstext.Append("<script type=text/javascript>var answer =  confirm(' ")
                        cstext.Append("ATTENZIONE: nella lista degli atti da inoltrare sono stati selezionati alcuni atti etichettati come \'URGENTE\'.\n\nPremendo OK si procederà all\'inoltro, senza poter scegliere la priorità degli atti da inoltrare.\n\nAltrimenti, scegliere ANNULLA per selezionare gli atti da inoltrare con la stessa priorità.")
                        cstext.Append("'); ")
                        cstext.Append("if (answer){}else{window.location = 'WorklistAction.aspx?tipo=" & TipoApplic(Context) & "';} ")
                        cstext.Append("</script>")
                        cs.RegisterStartupScript(cstype, csname, cstext.ToString())
                        radioIsUrgent.Enabled = False
                        radioIsUrgent.Checked = False
                        radioIsNotUrgent.Enabled = False
                        radioIsNotUrgent.Checked = False
                    End If
                ElseIf oOperatore.Test_Attributo("SCEGLI_URGENZA", True) AndAlso HttpContext.Current.ApplicationInstance.Session.Item("prioritaUrgenti") = True Then
                    radioIsUrgent.Checked = True
                    radioIsUrgent.Enabled = True
                    radioIsNotUrgent.Enabled = True
                End If
            End If



            If Not Context.Session.Item("erroreFile") Is Nothing Then
                LblErrore = New Label
                LblErrore.Text = Context.Session.Item("erroreFile")
                LblErrore.Visible = True
                Contenuto.Controls.Add(LblErrore)
            Else
                Context.Session.Remove("erroreFile")
            End If
            If Not Context.Session.Item("warning") Is Nothing Then
                LblErrore = New Label
                LblErrore.Text = ""
                LblErrore.CssClass = "lblWarning"
                For Each messaggio As String In DirectCast(Context.Session.Item("warning"), ArrayList)
                    LblErrore.Text = LblErrore.Text & messaggio
                Next
                LblErrore.Visible = True
                Contenuto.Controls.Add(LblErrore)
            Else
                Context.Session.Remove("warning")
            End If

            HttpContext.Current.ApplicationInstance.Session.Remove("prioritaUrgenti")
            HttpContext.Current.ApplicationInstance.Session.Remove("prioritaMixed")
            
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub caricaDropDaArray(ByRef vR As Object, ByVal elementoDefault As String)
        Dim i As Integer = 0
        For i = 0 To UBound(vR, 2)
            Dim lItem As New WebControls.ListItem
            lItem.Value = vR(0, i)
            lItem.Text = vR(1, i)

            If elementoDefault = lItem.Value Then
                lItem.Selected = True
            End If


            ddlSupervisore.Items.Add(lItem)
        Next
    End Sub
    Private Sub gestionePulsanti()

        Select Case HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                gestionePulsanti_Regione()
            Case "ALSIA"
                gestionePulsanti_Alsia()

        End Select

    End Sub
    Private Sub gestionePulsantiMultiplo()

        Select Case HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                gestionePulsantiMultiplo_Regione()
            Case "ALSIA"
                gestionePulsantiMultiplo_Alsia()


        End Select

    End Sub
    Private Sub btnInoltra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInoltra.Click
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim valoriCmbDestinatarioInoltro() As String = Request.Form.GetValues("cmbDestinatarioInoltro")

         If Not ((valoriCmbDestinatarioInoltro Is Nothing)) Then
            If  oOperatore.Test_Attributo("LIVELLO_UFFICIO", "SUPERVISORE") Then
                Session.Add("prossimoAttore", Request.Form.GetValues("cmbDestinatarioInoltro")(0))
            Else 
                Dim destinatarioInoltro As Integer = valoriCmbDestinatarioInoltro(0)
                If (destinatarioInoltro <> Nothing) Then
                    Session.Add("destinatarioInoltro", destinatarioInoltro)
                End If
            End If
        End If
       
        If radioIsUrgent.Checked Then
            Session.Add("flagUrgente", True)
        ElseIf radioIsNotUrgent.Checked Then
            Session.Add("flagUrgente", False)
        End If
        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
            If Not Request.Form.GetValues("ddlsuggerimenti") Is Nothing Then
                If Request.Form.GetValues("ddlsuggerimenti")(0) < 0 Then
                    HttpContext.Current.Session.Add("erroreFile", "Non è stato specificato alcun suggerimento, si prega di selezionare uno dalla lista<br />")
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                Else
                    Session.Add("suggerimento", Request.Form.GetValues("ddlsuggerimenti")(0))
                End If
            End If
        End If
        Session.Remove("conFirma")
        Select Case HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                btnInoltraClickRegione()
            Case "ALSIA"
                btnInoltraClickAlsia()

        End Select
    End Sub
    Private Sub btnFirma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirma.Click
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Session.Add("note", txtNote.Text & "")

        Dim valoriCmbDestinatarioInoltro() As String = Request.Form.GetValues("cmbDestinatarioInoltro")
        Dim destinatarioInoltro As Integer = -1
        If Not ((valoriCmbDestinatarioInoltro Is Nothing)) Then
            If  oOperatore.Test_Attributo("LIVELLO_UFFICIO", "SUPERVISORE") Then
                Session.Add("prossimoAttore", Request.Form.GetValues("cmbDestinatarioInoltro")(0))
            Else 
                destinatarioInoltro = valoriCmbDestinatarioInoltro(0)
            End If
        End If
        Session.Add("destinatarioInoltro", destinatarioInoltro)
        If radioIsUrgent.Checked Then
            Session.Add("flagUrgente", True)
        ElseIf radioIsNotUrgent.Checked Then
            Session.Add("flagUrgente", False)
        End If
        If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4LIVELLO") Or oOperatore.Test_Attributo("LIVELLO_UFFICIO", "COLLABORATORE") Then
            If Not Request.Form.GetValues("ddlsupervisore") Is Nothing AndAlso Request.Form.GetValues("ddlsupervisore")(0) <> "" Then
                Session.Add("prossimoAttore", Request.Form.GetValues("ddlsupervisore")(0))
            End If
        End If

        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
            If Not Request.Form.GetValues("ddlsuggerimenti") Is Nothing Then
                If Request.Form.GetValues("ddlsuggerimenti")(0) < 0 Then
                    HttpContext.Current.Session.Add("erroreFile", "Non è stato specificato alcun suggerimento, si prega di selezionare uno dalla lista<br />")
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                Else
                    Session.Add("suggerimento", Request.Form.GetValues("ddlsuggerimenti")(0))
                End If
            End If
        End If

        Session.Add("codAzione", "INOLTRO")
        Session.Add("conFirma", "true")


        If Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Or Context.Session.Item("elencoDocumentiDaInoltrare") = "" Then


            If Not Request.QueryString.Get("key") Is Nothing Then
                Session.Add("codDocumento", Request.QueryString.Get("key"))
            Else
                Session.Add("codDocumento", Context.Session.Item("key"))
                Context.Session.Remove("key")
            End If

            Session.Add("note", txtNote.Text & "")
            Response.Redirect("FirmaDocumentoAction.aspx?key=" & Context.Session.Item("codDocumento"))
        Else

            'verifica ni conferma multipla
            Response.Redirect("ConfermaFirmaMultiplaAction.aspx")

        End If

    End Sub

    Private Sub btnFirma_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirma.Init

    End Sub


    Private Sub btnFirma_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirma.Load

    End Sub
End Class
