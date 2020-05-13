Imports System.Xml
Public Class searchStampa
    Inherits System.Web.UI.UserControl

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnAvviaRicerca As System.Web.UI.WebControls.Button
    Protected WithEvents txtOggettoRicerca As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlRicerca As System.Web.UI.WebControls.Panel
    Protected WithEvents CalendarPopup1 As eWorld.UI.CalendarPopup
    Protected WithEvents CalendarPopup2 As eWorld.UI.CalendarPopup
    Protected WithEvents ddlUffici As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dllDipartimenti As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtNumero As System.Web.UI.WebControls.TextBox
    Protected WithEvents dllStatoStampa As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblTipo As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    Public renderDllStatoStampa As Boolean = True
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

            Dim lstrDip As String = "" & Session("txtCodDipartimenti")

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
            Session.Remove("passato")
            'PulisciSessione()
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

                    If Not Request.Form("ricerca:dllStatoStampa") Is Nothing Then
                        dllStatoStampa.SelectedValue = Request.Form("ricerca:dllStatoStampa")
                    End If
                End If

            Catch ex As Exception

            End Try
            'Else

            '                Session.Remove("passato")

            '        End If

        End If

    End Sub

    Private Sub btnAvviaRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAvviaRicerca.Click
        Session.Add("txtDataInizio", CalendarPopup1.SelectedDate)
        Session.Add("txtDataFine", CalendarPopup2.SelectedDate)
        Session.Add("txtOggettoRicerca", txtOggettoRicerca.Text)
        'Session.Add("txtCodUfficio", ddlUffici.SelectedValue)
        Session.Add("txtCodDipartimenti", Request.Form("ricerca:dllDipartimenti"))
        Session.Add("txtNumero", txtNumero.Text)
        Session.Add("txtCodUfficio", Request.Form("ricerca:ddlUffici"))
        Session.Add("StatoStampa", Request.Form("ricerca:dllStatoStampa"))

        Dim lstr As String = Context.Request.UrlReferrer.AbsoluteUri

        If lstr.IndexOf("Action") = -1 Then
            lstr = lstr.Insert(lstr.LastIndexOf("."), "Action")
        End If

        Response.Redirect(lstr)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        dllStatoStampa.Visible = renderDllStatoStampa
        If Not renderDllStatoStampa Then
            dllStatoStampa.SelectedValue = ""
        End If
        lblTipo.Visible = renderDllStatoStampa
    End Sub

End Class
