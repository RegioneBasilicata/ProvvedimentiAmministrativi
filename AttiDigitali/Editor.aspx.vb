Imports iTextSharp.text.pdf
Imports System.IO
Imports iTextSharp.tool.xml
Imports iTextSharp.text

Partial Public Class editor
    Inherits WebSession
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(CaricaDocumento))

    Private Sub editor_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Inizializza_Pagina(Me, "Testo Provvedimento")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Try
            Dim descrizioneDocumento As String = ""
            Dim numeroPubblicoDocumento As String = ""
            Select Case Context.Request.QueryString.Item("tipo")
                Case 0
                    descrizioneDocumento = "Determina"
                Case 1
                    descrizioneDocumento = "Delibera"
                Case 2
                    descrizioneDocumento = "Disposizione"
                Case 3
                    descrizioneDocumento = "Decreti"
                Case 4
                    descrizioneDocumento = "Ordinanze"
                Case Else
                    descrizioneDocumento = "Documento"
            End Select
            Dim idDoc As String = Context.Request.QueryString.Item("key")
            If idDoc = "" Then
                idDoc = "2017000009"
            End If
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDoc)
            Rinomina_Pagina(Me, descrizioneDocumento & " " & IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero))
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(idDoc)
            Dim livelloUfficio As String = statoIstanza.LivelloUfficio
            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo <> "A" AndAlso 
                (oOperatore.oUfficio.CodUfficio = statoIstanza.CodiceUfficio And livelloUfficio = "UP") Or ((oOperatore.oUfficio.CodUfficio = statoIstanza.CodiceUfficio And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloUfficio = "UDD")) Or ((livelloUfficio = "UR") And HttpContext.Current.Application.Item("CONTABILITA") <> "SIC") Or oOperatore.Test_Ruolo("CT001") Then
                hiddentoolbarId.Value = "Full_Provvedimenti"
            Else
                hiddentoolbarId.Value = "Basic"
            End If

            If Not IsPostBack Then
                ' caricamento(testo)

                'objDocumento.Doc_Testo = objDocumento.Doc_Testo.Replace("&lt;", "<").Replace("&gt;", ">")
                If (Not String.IsNullOrEmpty(objDocumento.Doc_Testo)) Then
                    editor1.InnerText = objDocumento.Doc_Testo
                Else
                    editor1.InnerText = ""
                End If
            Else
                If LCase(editor1.InnerText) <> LCase(objDocumento.Doc_Testo) Then
                    objDocumento.Doc_Testo = editor1.InnerText

                    'Dim ms As MemoryStream = New MemoryStream()

                    Dim filestream As FileStream = New FileStream("d:\\file.pdf", FileMode.Create)
                    'Create our PDF document            
                    Dim Doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER)

                    'Bind our PDF object to the physical file using a PdfWriter               
                    Dim Writer As iTextSharp.text.pdf.PdfWriter = PdfWriter.GetInstance(Doc, filestream)

                    'Open our document for writing                   
                    Doc.Open()

                    'Insert a blank page                    
                    Doc.NewPage()

                    Dim fontIntestazione As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 7.0F, iTextSharp.text.Font.BOLD)
                    Dim fontDatiLiquidaz As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9.0F, iTextSharp.text.Font.BOLD)
                    Dim fontBody As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6.0F)

                    'Dim table As PdfPTable = New PdfPTable(1)
                    'table.TotalWidth = 226.0F
                    'Dim cellTesto As PdfPCell
                    'cellTesto = New PdfPCell(New iTextSharp.text.Phrase(objDocumento.Doc_Testo))
                    'table.AddCell(cellTesto)
                    'Doc.Add(table)
                    'Doc.Add(New iTextSharp.text.Phrase(Environment.NewLine))

                    'Dim htmlWorker As iTextSharp.text.html.simpleparser.HTMLWorker = New iTextSharp.text.html.simpleparser.HTMLWorker(Doc)
                    Dim sr As StringReader = New StringReader(objDocumento.Doc_Testo)

                    ' Dim htmlarraylist As ArrayList = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, Nothing)

                    'For k As Integer = 0 To htmlarraylist.Count - 1
                    '    Dim mypara As Paragraph = New Paragraph()
                    '    mypara.IndentationLeft = 36
                    '    mypara.InsertRange(0, htmlarraylist.Item(k))
                    '    Doc.Add(mypara)
                    '    '    Dim a As Object = htmlarraylist.Item(k)
                    '    '    Doc.Add(New iTextSharp.text.Phrase(a))
                    'Next



                    'Dim parser As iTextSharp.text.html.simpleparser.HTMLWorker = New iTextSharp.text.html.simpleparser.HTMLWorker(Doc)

                    'Try
                    '    parser.Parse(sr)
                    'Catch ex As Exception

                    'End Try








                    'ArrayList htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
                    '                 //add the collection to the document
                    ' for (int k = 0; k < htmlarraylist.Count; k++)
                    ' {
                    '     document.Add((IElement)htmlarraylist[k]);
                    ' }


                    'htmlWorker.Parse(sr)

                    XMLWorkerHelper.GetInstance().ParseXHtml(Writer, Doc, sr)
                    'iTextSharp.text.html.HtmlParser.Parse(Doc, )

                    Doc.Close()
                    Writer.Close()
                    Registra_Documento(objDocumento.Doc_id, , objDocumento.Doc_Testo)

                    
                End If

            End If
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try

    End Sub

End Class