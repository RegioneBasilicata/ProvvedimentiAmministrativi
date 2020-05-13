Imports System.Collections.Generic

Partial Class RigettoDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ddlSupervisore As System.Web.UI.WebControls.DropDownList

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Rigetto Provvedimento")
    End Sub

#End Region
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(RigettoDocumento))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim vR As Object = Nothing
        Try
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            Dim tipoApp As String = TipoApplic(HttpContext.Current)
            Select Case tipoApp
                Case 0
                    Rinomina_Pagina(Me, "Rigetto Determina")
                Case 1
                    Rinomina_Pagina(Me, "Rigetto Delibera")
                Case 2
                    Rinomina_Pagina(Me, "Rigetto Disposizione")
            End Select
            If Not IsPostBack Then
                'test su pannello allegato di rigetto
                If oOperatore.Test_Ruolo("NT001") Then
                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                Else
                    pnlAggiungiAllegato.Visible = False
                End If

                Contenuto.Controls.Add(pnlRigetto)

                If (Not oOperatore.Test_Ruolo("DT008") And tipoApp = 0) Or (Not oOperatore.Test_Ruolo("DS017") And tipoApp = 2) Then
                    pnlRigetto.Visible = False
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
            End If

            'codice eseguito anche nel postback
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
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub gestionePulsanti()

        Select Case HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                gestionePulsanti_Regione()
            Case "ALSIA"
                ' gestionePulsanti_Alsia()
                gestionePulsanti_Regione()

        End Select

    End Sub
    Private Sub gestionePulsantiMultiplo()

        Select Case HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                gestionePulsantiMultiplo_Regione()
            Case "ALSIA"
                ' gestionePulsantiMultiplo_Alsia()
                gestionePulsantiMultiplo_Regione()

        End Select

    End Sub
    Private Sub btnRigetta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRigetta.Click
        Session.Add("destinatarioInoltro", -1)
        Session.Add("codAzione", "RIGETTO")
        Session.Add("note", txtNote.Text & "")
        If Not fileUpLoadAllegato.PostedFile Is Nothing AndAlso fileUpLoadAllegato.PostedFile.ContentLength <> 0 Then
            HttpContext.Current.Session.Add("erroreFile", "")
            If ((fileUpLoadAllegato.PostedFile.ContentType = "application/pkcs7-mime" Or fileUpLoadAllegato.PostedFile.ContentType = "application/x-pkcs7-mime")) Then
                Dim fileStream As System.IO.Stream
                fileStream = fileUpLoadAllegato.PostedFile.InputStream
                Dim bFile(fileStream.Length) As Byte
                fileStream.Read(bFile, 0, CInt(fileStream.Length))
                Session.Add("noteRigetto", bFile)
            ElseIf fileUpLoadAllegato.PostedFile.ContentType = "application/octet-stream" Then
                HttpContext.Current.Session.Add("erroreFile", "Il file è già aperto da un altro programma, chiudere il file e riprovare")
                Response.Redirect(Request.UrlReferrer.AbsoluteUri)
            Else
                HttpContext.Current.Session.Add("erroreFile", "Il file da salvare deve essere firmato")
                Response.Redirect(Request.UrlReferrer.AbsoluteUri)
            End If
        End If
        If Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Or Context.Session.Item("elencoDocumentiDaInoltrare") = "" Then
            Session.Remove("conFirma")
            Dim key As String = ""
            If Not Request.QueryString.Get("key") Is Nothing Then
                Session.Add("codDocumento", Request.QueryString.Get("key"))
            Else
                Session.Add("codDocumento", Context.Session.Item("key"))
                Context.Session.Remove("key")
            End If

            Dim respUfficio As String = ""

            Dim flusso As String = DefinisciFlusso(TipoApplic(Context))
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
            respUfficio = oOperatore.oUfficio.ResponsabileUfficio(flusso)


            If oOperatore.oUfficio.bUfficioRagioneria And LCase(respUfficio) = LCase(oOperatore.Codice) Then

                key = Session("codDocumento")
                CancellaPrenotazione(key)

            End If

            Response.Redirect("InoltraDocumentoAction.aspx?tipo=" & TipoApplic(Context))

        Else

            Response.Redirect("InoltraBloccoDocumentiAction.aspx?tipo=" & TipoApplic(Context))


        End If
    End Sub
   
    Sub CancellaPrenotazione(ByVal codDocumento As String)
        Select Case HttpContext.Current.Application.Item("CONTABILITA")
            Case "SIC"
                cancellaPrenotazioneSIC(codDocumento)
        End Select
    End Sub
    Private Sub btnFirma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirma.Click
        Session.Add("destinatarioInoltro", -1)
        Session.Add("codAzione", "RIGETTO")
        Session.Add("conFirma", "true")
        If Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Or Context.Session.Item("elencoDocumentiDaInoltrare") = "" Then
            '  Dim key As String = ""
            If Not Request.QueryString.Get("key") Is Nothing Then
                Session.Add("codDocumento", Request.QueryString.Get("key"))
            Else
                Session.Add("codDocumento", Context.Session.Item("key"))
                Context.Session.Remove("key")
            End If

            Response.Redirect("FirmaDocumentoAction.aspx?key=" & Request.QueryString.Get("key"))
        Else
            Try
                PreparaDocumentiDaFirmare(HttpContext.Current, Session.Item("codAzione"))
                Response.Redirect("ConfermaFirmaMultiplaAction.aspx")
            Catch ex As Exception
                Log.Error(Now & " - " & HttpContext.Current.ApplicationInstance.Session.Item("oOperatore").Codice & " - " & ex.Message)
            End Try
            Response.Redirect("ConfermaFirmaMultiplaAction.aspx")
        End If

    End Sub
End Class
