Imports System.Xml
Public Class search
    Inherits System.Web.UI.UserControl

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dllTipoRigetto As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblTipo As System.Web.UI.WebControls.Label
    Protected WithEvents txtCodiceCUP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCodiceCIG As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlTipologiaDocumento As System.Web.UI.WebControls.DropDownList
    
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    Public renderDllTipoRigetto As Boolean = True
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim op As DllAmbiente.Operatore = Context.Session.Item("oOperatore")
        Dim itemList As New ListItem
        If Not Page.IsPostBack Then

            Dim hta As ArrayList = op.DipartimentoDipendenti(TipoApplic(Context))

            dllDipartimenti.Items.Clear()
            ddlUffici.Items.Clear()
            ddlTipologiaDocumento.Items.Clear()


            Dim lstrDip As String = "" & Session("txtCodDipartimenti")

            Dim tipoData As Integer = Session("radioData")
            IF tipoData = 0 Then
                chkDataSeduta.SelectedValue = 1
            ElseIf  tipoData = 1 Then
                chkDataSeduta.SelectedValue = 1
            ElseIf tipoData = 2 Then
                chkDataSeduta.SelectedValue = 2
            End If
            If Not TipoApplic(Context).Equals("1") Then
                chkDataSeduta.Visible = False
                labelTipoData.Visible = False
            Else 
                labelTipoData.Visible = True
                chkDataSeduta.Visible = True
            End If
            If Not HttpContext.Current.Request.Url.ToString.Contains("Archivio") Then
                chkDataSeduta.Visible = False
                labelTipoData.Visible = False
            End If
            For Each dipartimento As DllAmbiente.StrutturaInfo In hta
                itemList = New ListItem
                itemList.Value = dipartimento.CodiceInterno
                itemList.Text = dipartimento.CodicePubblico & "--" & dipartimento.DescrizioneBreve
                If lstrDip = itemList.Value Then
                    itemList.Selected = True
                Else
                    itemList.Selected = False
                End If
                dllDipartimenti.Items.Add(itemList)
            Next
            If hta.Count = 0 And dllDipartimenti.Items.Count = 0 Then
                itemList = New ListItem
                itemList.Value = op.oUfficio.CodDipartimento
                itemList.Text = op.oUfficio.CodDipartimentoPubblico & "--" & op.oUfficio.DescrDipartimentoBreve
                If lstrDip = itemList.Value Then
                    itemList.Selected = True
                Else
                    itemList.Selected = False
                End If
                dllDipartimenti.Items.Add(itemList)
            End If


            Dim AUD As ArrayList
            AUD = DirectCast(op.UfficiDipendenti(TipoApplic(Context), dllDipartimenti.SelectedValue), ArrayList)

            Dim lstrUffici As String = "" & Session("txtCodUfficio")
            For Each ufficio As DllAmbiente.StrutturaInfo In AUD
                itemList = New ListItem
                itemList.Value = ufficio.CodiceInterno
                itemList.Text = ufficio.CodicePubblico & "--" & ufficio.DescrizioneBreve
                If lstrUffici = itemList.Value Then
                    itemList.Selected = True
                Else
                    itemList.Selected = False
                End If
                ddlUffici.Items.Add(itemList)
            Next

            If ddlUffici.Items.Count > 1 Then
                itemList = New ListItem
                itemList.Value = ""
                itemList.Text = "Tutti gli uffici"

                If lstrUffici = "" Then
                    itemList.Selected = True
                End If

                ddlUffici.Items.Add(itemList)
            End If


            'If AUD.Count = 0 And ddlUffici.Items.Count = 0 Then

            If AUD.Count = 0 And dllDipartimenti.Items.Count = 0 Then
                itemList = New ListItem
                itemList.Value = op.oUfficio.CodUfficio
                itemList.Text = op.oUfficio.CodUfficioPubblico & "--" & op.oUfficio.DescrUfficioBreve
                If lstrDip = itemList.Value Then
                    itemList.Selected = True
                Else
                    itemList.Selected = False
                End If
                dllDipartimenti.Items.Add(itemList)
            End If

            Dim lstrNumero As String = "" & Session("txtNumero")
            txtNumero.Text = lstrNumero

            Dim lstrOgg As String = "" & Session("txtOggettoRicerca")
            txtOggettoRicerca.Text = lstrOgg

            Dim lstrCodCUP As String = "" & Session("txtCodiceCUP")
            txtCodiceCUP.Text = lstrCodCUP

            Dim lstrCodCIG As String = "" & Session("txtCodiceCIG")
            txtCodiceCIG.Text = lstrCodCIG

            Dim lstrCalendarPopup2 As String = "" & Session("txtDataFine")
            If lstrCalendarPopup2 <> "" Then
                CalendarPopup2.SelectedDate = lstrCalendarPopup2
            Else

                If CalendarPopup2.SelectedDate.Year = 1 Then
                    CalendarPopup2.SelectedDate = Date.Now
                End If
            End If

            Dim dataInizio As DateTime = DateTime.Now
            'CalendarPopup1.SelectedDate = DateAdd(DateInterval.WeekOfYear, -1, Now)

            Dim lstrCalendarPopup1 As String = "" & Session("txtDataInizio")


            If lstrCalendarPopup1 <> "" Then
                CalendarPopup1.SelectedDate = lstrCalendarPopup1
            Else


                If CalendarPopup1.SelectedDate.Year = 1 Then
                    CalendarPopup1.SelectedDate = New Date(Year(Today), 1, 1)
                Else
                    CalendarPopup1.SelectedDate = Now.AddMonths(-1)
                    
                End If

            End If

            If Not (Session("TipologiaRicercaBeneficiario") = Nothing) Or Not (Session("TipologiaRicercaBeneficiario") = "") Then
                TipologiaRicercaBeneficiario.SelectedValue = Session("TipologiaRicercaBeneficiario")
            End If

            If Not (Session("FiltroRicercaBeneficiario") = Nothing) Or Not (Session("FiltroRicercaBeneficiario") = "") Then
                BeneficiarioTxt.Text = Session("FiltroRicercaBeneficiario")
            End If

            If Not (Session("TipologiaRicercaDestinatario") = Nothing) Or Not (Session("TipologiaRicercaDestinatario") = "") Then
                TipologiaRicercaDestinatario.SelectedValue = Session("TipologiaRicercaDestinatario")
            End If

            If Not (Session("FiltroRicercaDestinatario") = Nothing) Or Not (Session("FiltroRicercaDestinatario") = "") Then
                DestinatarioTxt.Text = Session("FiltroRicercaDestinatario")
            End If

            

            Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(op)
            Dim listaTipologieDocumento As Generic.List(Of DllDocumentale.ItemTipologiaDocumento) = ojbSvrdocumenti.FO_GetListaTipologieDocumento()
            Dim lItem As WebControls.ListItem
            lItem = New WebControls.ListItem
            lItem.Value = -1
            lItem.Text = "- Selezionare una tipologia -"
            ddlTipologiaDocumento.Items.Add(lItem)

            For Each tipologia As DllDocumentale.ItemTipologiaDocumento In listaTipologieDocumento
                lItem = New WebControls.ListItem
                lItem.Value = tipologia.Id
                lItem.Text = tipologia.Tipologia
                ddlTipologiaDocumento.Items.Add(lItem)
            Next

            If Not (Session("txtTipologiaDocumento") = Nothing) Or Not (Session("txtTipologiaDocumento") = "") Then
                ddlTipologiaDocumento.SelectedValue = Session("txtTipologiaDocumento")
            End If

            If Not (Session("txtAutorizPubblicazione") = Nothing) Or Not (Session("txtAutorizPubblicazione") = "") Then
                ddlAutorizPubblicazione.SelectedValue = Session("txtAutorizPubblicazione")
            End If

            Session.Remove("passato")
            If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                dllTipoRigetto.Visible = True
            Else
                dllTipoRigetto.Visible = False
            End If


        Else

            Try

                Dim hUD As IList
                hUD = op.UfficiDipendenti(TipoApplic(Context), dllDipartimenti.SelectedValue)

                If Not hUD Is Nothing OrElse hUD.Count > 0 Then

                    ddlUffici.Items.Clear()
                    Dim lstrUfficio As String = "" & Request.Form("ricerca:ddlUffici") ' Is Nothing

                    If hUD.Count > 1 Then
                        itemList = New ListItem
                        itemList.Value = ""
                        If lstrUfficio = "" Then
                            itemList.Selected = True
                        End If
                        itemList.Text = "Tutti gli uffici"
                        ddlUffici.Items.Add(itemList)
                    End If


                    For Each ufficio As DllAmbiente.StrutturaInfo In hUD
                        itemList = New ListItem
                        itemList.Value = ufficio.CodiceInterno
                        itemList.Text = ufficio.CodicePubblico & "--" & ufficio.DescrizioneBreve
                        If lstrUfficio = itemList.Value Then
                            itemList.Selected = True
                        Else
                            itemList.Selected = False
                        End If
                        ddlUffici.Items.Add(itemList)
                    Next

                    'If Not Request.Form("ricerca:ddlUffici") Is Nothing Then
                    '    ddlUffici.SelectedValue = Request.Form("ricerca:ddlUffici")
                    'End If
                    If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
                        If Not Request.Form("ricerca:dllTipoRigetto") Is Nothing Then
                            dllTipoRigetto.SelectedValue = Request.Form("ricerca:dllTipoRigetto")
                        End If
                    Else
                        dllTipoRigetto.Visible = False
                    End If
                End If

            Catch ex As Exception

            End Try

        End If

    End Sub
    
    Private Sub btnAvviaRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAvviaRicerca.Click
        Session.Add("radioData", Integer.Parse(chkDataSeduta.SelectedItem.Value))
        Session.Add("txtDataInizio", CalendarPopup1.SelectedDate)
        Session.Add("txtDataFine", CalendarPopup2.SelectedDate)
        Session.Add("txtOggettoRicerca", txtOggettoRicerca.Text)
        'Session.Add("txtCodUfficio", ddlUffici.SelectedValue)
        Session.Add("txtCodDipartimenti", Request.Form("ricerca:dllDipartimenti"))
        Session.Add("txtNumero", txtNumero.Text)
        Session.Add("txtCodUfficio", Request.Form("ricerca:ddlUffici"))
        Session.Add("TipoRigetto", Request.Form("ricerca:dllTipoRigetto"))
        Session.Add("TipologiaRicercaBeneficiario", TipologiaRicercaBeneficiario.Text)
        Session.Add("FiltroRicercaBeneficiario", BeneficiarioTxt.Text)
        Session.Add("txtCodiceCUP", txtCodiceCUP.Text)
        Session.Add("txtCodiceCIG", txtCodiceCIG.Text)
        Session.Add("txtTipologiaDocumento", Request.Form("ricerca:ddlTipologiaDocumento"))
        Session.Add("txtAutorizPubblicazione", Request.Form("ricerca:ddlAutorizPubblicazione"))
        Session.Add("TipologiaRicercaDestinatario", TipologiaRicercaDestinatario.Text)
        Session.Add("FiltroRicercaDestinatario", DestinatarioTxt.Text)
       
        Dim lstr As String = Context.Request.UrlReferrer.AbsoluteUri
        If lstr.IndexOf("Archivio") = -1 Then
            If lstr.IndexOf("Action") = -1 Then
                lstr = lstr.Insert(lstr.LastIndexOf("."), "Action")
            End If
        Else
            lstr = "MonitorArchivioWaiting.aspx?" & Context.Request.QueryString.ToString
        End If
        

        Response.Redirect(lstr)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE" Then
            renderDllTipoRigetto = True
        Else
            renderDllTipoRigetto = False
        End If
        dllTipoRigetto.Visible = renderDllTipoRigetto
        If Not renderDllTipoRigetto Then
            dllTipoRigetto.SelectedValue = ""
        End If
        lblTipo.Visible = renderDllTipoRigetto
    End Sub


End Class
