Public Class DocumentoTesto
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnSalvaTesto As System.Web.UI.WebControls.Button
    Protected WithEvents testoEditato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlAggiungiAllegato As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUploadAllegato As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label

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



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Label1.Text = ""
        Try
            If Not IsPostBack Then
                Rinomina_Pagina(Me, " Documento " & context.Items.Item("numeroDoc"))
                testoEditato = New System.Web.UI.HtmlControls.HtmlInputHidden
                testoEditato.Name = "testoEditato"
                testoEditato.ID = "testoEditato"
                testoEditato.Value = context.Items.Item("testoEditato")
                Contenuto.Controls.Add(testoEditato)
                fileUploadAllegato.Accept = "application/msword"
                Label1.Text = HttpContext.Current.Session.Item("erroreFile")
                HttpContext.Current.Session.Remove("erroreFile")
                'Label1.Text = context.Items.Item("erroreFile")

                Contenuto.Controls.Add(Label1)
                Contenuto.Controls.Add(Label2)
                'modgg 10-06 1
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                'Qui va testato se deve essere uploadato il file oppure no
                If oOperatore.oUfficio.CodUfficio = Context.Items.Item("CodUffProp") Then
                    pnlAggiungiAllegato.Visible = True
                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                    btnSalvaTesto.Visible = True
                Else
                    pnlAggiungiAllegato.Visible = False
                    btnSalvaTesto.Visible = False
                    Label2.Text = "Impossibile editare il testo del documento."
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
                cella.Controls.Add(btnSalvaTesto)
                riga.Controls.Add(cella)
                tabella.Rows.Add(riga)



                Contenuto.Controls.Add(tabella)
                'Else
                '    If fileUploadAllegato.PostedFile.ContentLength <> 0 Then

                '        If fileUploadAllegato.PostedFile.ContentType = fileUploadAllegato.Accept Then

                '            If Trim(context.Request.QueryString.Get("key")) <> "" Then
                '                Session.Add("key", context.Request.QueryString.Get("key"))
                '            End If
                '            Response.Redirect("RegistraTestoDeterminaAction.aspx")
                '            ' Display information about posted file

                '            'fileUpLoadAllegato.PostedFile.SaveAs("c:\Uploadedfiles\uploadfile.rtf")
                '        Else
                '            Label1.Text = "Il file da salvare deve essere in formato RTF."
                '            Label1.Visible = True
                '        End If
                '    Else

                '        Label1.Text = "E' necessario selezionare un file RTF prima di procedere alla registrazione"
                '        Label1.Visible = True
                '        Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                '    End If
            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnSalvaTesto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaTesto.Click

        HttpContext.Current.Session.Remove("erroreFile")
        If fileUploadAllegato.PostedFile.ContentLength <> 0 Then
            Dim estensione() As String = fileUploadAllegato.PostedFile.FileName.Split(".")
            HttpContext.Current.Session.Add("erroreFile", "")
            If fileUploadAllegato.PostedFile.ContentType = "application/msword" And UCase(estensione(estensione.Length - 1)) = "RTF" Then

                Dim fileStream As System.IO.Stream
                fileStream = fileUploadAllegato.PostedFile.InputStream
                Dim bFile(fileStream.Length) As Byte
                fileStream.Read(bFile, 0, CInt(fileStream.Length))
                Dim nomeFile() As String = estensione(estensione.Length - 2).Split("\")
                Session.Add("testoDocumento", bFile)
                Session.Add("nomeFile", nomeFile(nomeFile.Length - 1))
                Session.Add("estensione", estensione(estensione.Length - 1))
                If Trim(Context.Request.QueryString.Get("key")) <> "" Then
                    Session.Add("key", Context.Request.QueryString.Get("key"))
                End If
                Response.Redirect("RegistraTestoDeterminaAction.aspx")
            Else
                'context.Items.Add("erroreFile", Label1.Text)
                HttpContext.Current.Session.Add("erroreFile", "Il file da salvare deve essere in formato RTF")
                Response.Redirect(Request.UrlReferrer.AbsoluteUri)
            End If
        Else


            HttpContext.Current.Session.Add("erroreFile", "E' necessario selezionare un file RTF prima di procedere alla registrazione")
            'context.Items.Add("erroreFile", Label1.Text)
            Response.Redirect(Request.UrlReferrer.AbsoluteUri)

        End If




    End Sub

    
End Class
