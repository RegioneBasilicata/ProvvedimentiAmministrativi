Public Class CaricaDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents testoEditato As System.Web.UI.HtmlControls.HtmlInputHidden

    

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Testo")
    End Sub

#End Region
    Private _numeroDocumento As String = ""
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(CaricaDocumento))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

      
        Try
            If Not IsPostBack Then
                Dim descrizioneDocumento As String = ""
                Dim numeroPubblicoDocumento As String = ""
                Select Case Context.Request.QueryString.Item("tipo")

                    Case 0
                        descrizioneDocumento = "Determina"
                    Case 1
                        descrizioneDocumento = "Delibera"
                    Case 2
                        descrizioneDocumento = "Disposizione"
                    Case Else
                        descrizioneDocumento = "Documento"
                End Select
                Rinomina_Pagina(Me, descrizioneDocumento & " " & Context.Items.Item("numeroDoc"))
                Context.Session.Remove("numeroPubblicoDocumento")
                numeroPubblicoDocumento = Context.Items.Item("numeroDoc")
                Context.Session.Add("numeroPubblicoDocumento", numeroPubblicoDocumento)
                testoEditato = New System.Web.UI.HtmlControls.HtmlInputHidden
                testoEditato.Name = "testoEditato"
                testoEditato.ID = "testoEditato"
                testoEditato.Value = Context.Items.Item("testoEditato")
                Contenuto.Controls.Add(testoEditato)
                fileUploadAllegato.Accept = "application/msword"
                Label1.Text = HttpContext.Current.Session.Item("erroreFile")
                HttpContext.Current.Session.Remove("erroreFile")


                Contenuto.Controls.Add(Label1)
                Contenuto.Controls.Add(Label2)
                'Qui va testato se deve essere uploadato il file oppure no
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(Context.Request.QueryString.Item("key"))
                Dim livelloUfficio As String = statoIstanza.LivelloUfficio
                Dim ruolo As String = "" & statoIstanza.Ruolo
                If ruolo <> "A" AndAlso (oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And livelloUfficio = "UP") Or _
                ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloUfficio = "UDD")) Or _
                ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioSegreteriaPresidenzaLegittimita And livelloUfficio = "USL")) Or _
                ((livelloUfficio = "UR") And _
                 HttpContext.Current.Application.Item("CONTABILITA") <> "SIC") Or _
                 oOperatore.Test_Ruolo("CT001") Or oOperatore.Test_Ruolo("CL001") Then
                    pnlAggiungiAllegato.Visible = True
                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                    btnSalvaDocumento.Visible = True
                    gestionePulsanti(livelloUfficio)
                Else
                    pnlAggiungiAllegato.Visible = False
                    btnSalvaDocumento.Visible = False
                    Label2.Text = "Impossibile caricare la " & descrizioneDocumento & ". Autorizzazione negata"
                End If






                Dim tabella As Web.UI.WebControls.Table
                Dim riga As Web.UI.WebControls.TableRow
                Dim cella As Web.UI.WebControls.TableCell
                tabella = New Web.UI.WebControls.Table
                tabella.BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
                tabella.HorizontalAlign = HorizontalAlign.Center
                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                riga = New Web.UI.WebControls.TableRow
                cella = New Web.UI.WebControls.TableCell
                cella.ColumnSpan = 2
                cella.Controls.Add(btnSalvaDocumento)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)



                Contenuto.Controls.Add(tabella)
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub
    Private Sub gestionePulsanti(ByVal livelloufficio As String)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

      
        'consentire il salvataggio solo nell'ufficio proponente



        If (oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And livelloufficio = "UP") Or _
        ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloufficio = "UDD")) Or _
        ((oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") And oOperatore.oUfficio.bUfficioSegreteriaPresidenzaLegittimita And livelloufficio = "USL")) Or _
        ((livelloufficio = "UR") And _
        HttpContext.Current.Application.Item("CONTABILITA") <> "SIC") Or _
        oOperatore.Test_Ruolo("CT001") Or oOperatore.Test_Ruolo("CL001") Then
            btnSalvaDocumento.Enabled = True
        Else
            btnSalvaDocumento.Enabled = False
        End If

        ''310809 mod blocco caricamento documento di un atto rigettato dalla ragioneria 
        'Dim vR As Object = Leggi_Abilita_RigettoInoltro(Context.Request.QueryString.Get("key"), Context.Items.Item("CodUffProp"))
        'Dim vRit As Object = Nothing
        'If vR(0) = 0 Then
        '    vRit = vR(1)
        'End If

        'If vRit(1) <> 1 Then
        '    btnSalvaDocumento.Enabled = False
        'End If
        ''fine modifica
        Dim modificaAbilitata As Boolean = VerificaAbilitazioneInoltroRigetto(oOperatore, Context.Request.QueryString.Get("key"), "INOLTRO")
        btnSalvaDocumento.Enabled = modificaAbilitata

        'blocco caricamento documento di un atto creato da tool esterno

        Dim obj_doc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(Context.Request.QueryString.Get("key"))
        If Not String.IsNullOrEmpty(obj_doc.Doc_codApp) Then
            btnSalvaDocumento.Enabled = False
        End If

    End Sub
    Private Sub btnSalvaDocumento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaDocumento.Click
        'modgg 10-06 1
        Log.Info("*********** caricaDocumento btnSalvaDocumento_Click")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

      
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(Context.Request.QueryString.Item("key"))

        Dim ruolo As String = "" & statoIstanza.Ruolo
        If ruolo <> "A" Then


            HttpContext.Current.Session.Remove("erroreFile")
            If fileUploadAllegato.PostedFile.ContentLength <> 0 Then
                Dim estensione() As String = fileUploadAllegato.PostedFile.FileName.Split(".")
                HttpContext.Current.Session.Add("erroreFile", "")
                If ((fileUploadAllegato.PostedFile.ContentType = "application/msword" Or 
                    fileUploadAllegato.PostedFile.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document") And
                    (UCase(estensione(estensione.Length - 1)) = "DOC" Or UCase(estensione(estensione.Length - 1)) = "DOCX" Or UCase(estensione(estensione.Length - 1)) = "RTF")) Or 
                    (fileUploadAllegato.PostedFile.ContentType = "application/rtf" And (UCase(estensione(estensione.Length - 1)) = "RTF")) Then
                    Log.Debug("content-type" & fileUploadAllegato.PostedFile.ContentType)
                    Dim fileStream As System.IO.Stream
                    fileStream = fileUploadAllegato.PostedFile.InputStream
                    Dim bFile(fileStream.Length) As Byte
                    fileStream.Read(bFile, 0, CInt(fileStream.Length))
                    GC.SuppressFinalize(fileStream)
                    fileStream.Dispose()
                    fileStream.Close()
                    'Dim nomeFile() As String = estensione(estensione.Length - 2).Split("\")
                    Session.Add("testoDocumento", bFile)
                    Session.Add("nomeFile", Context.Session.Item("numeroPubblicoDocumento") & "_" & oOperatore.pCodice)
                    Session.Add("estensione", estensione(estensione.Length - 1))
                    If Trim(Context.Request.QueryString.Get("key")) <> "" Then
                        Session.Add("key", Context.Request.QueryString.Get("key"))
                    End If
                    If Trim(Context.Request.QueryString.Get("tipo")) <> "" Then
                        Context.Session.Add("tipo", Context.Request.QueryString.Item("tipo"))
                    End If
                    Response.Redirect("CaricaDocumentoAction.aspx")
                Else
                    'modgg 11-06 2
                    If fileUploadAllegato.PostedFile.ContentType = "application/octet-stream" Then
                        HttpContext.Current.Session.Add("erroreFile", "Il file è già aperto da un altro programma, chiudere il file e riprovare")
                        Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                    Else
                        HttpContext.Current.Session.Add("erroreFile", "Il file da salvare deve essere in formato .Doc")
                        Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                    End If
                End If
            Else
                HttpContext.Current.Session.Add("erroreFile", "E' necessario selezionare un file Doc prima di procedere alla registrazione")
                Response.Redirect(Request.UrlReferrer.AbsoluteUri)

            End If
        Else
            HttpContext.Current.Session.Add("erroreFile", "Il provvedimento risulta Annullato. ")
            Response.Redirect(Request.UrlReferrer.AbsoluteUri)


        End If



    End Sub

End Class
