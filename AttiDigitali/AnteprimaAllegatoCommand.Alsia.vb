Imports Microsoft.Office.Interop
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.IO.File
Imports System.Runtime.Remoting.Contexts
Imports DllDocumentale.Model

Partial Class AnteprimaAllegatoCommand
    Protected Sub Execute_Alsia(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim idTestoDocumento As String = ""
        Dim binarioFile() As Byte

        Dim idDocumento As String = ""
        Dim canMerge As Boolean = False
        Dim paginaFirma As Boolean = False
        If context.Request.QueryString.Item("prn") <> "" Then
            Dim op As New DllAmbiente.Operatore
            If context.Request.QueryString.Item("idop") & "" <> "" Then
                op.pCodice = context.Request.QueryString.Item("idop") & ""
            Else
                op.pCodice = ConfigurationManager.AppSettings("printUser")
            End If

            HttpContext.Current.Session.Add("oOperatore", op)
        End If
        Dim oDllDocumenti As New DllDocumentale.svrDocumenti(HttpContext.Current.Session.Item("oOperatore"))

        codAllegato = context.Request.QueryString.Get("key")
        vettoredati = Anteprima_Allegato(codAllegato)
        idDocFromAllegato = vettoredati(7) & ""


        Log.Debug("ANTEPRIMA ALLEG - Id Allegato: " & codAllegato)
        Log.Debug("ANTEPRIMA ALLEG - Rit. Anteprima_Allegato: " & vettoredati(0))
        Try
            If context.Request.QueryString.Item("idx") <> "" Then
                indexAllegato = context.Request.QueryString.Item("idx")
            End If
            If context.Request.QueryString.Item("pdf") = 1 Then
                ''Stampa pdf
                '                printWord(vettoredati, context)
                '               CreaDocumento(vettoredati, context)
                If vettoredati(3) = "pdf" Then
                    vettoredati(1) = CreaDocumentoAlsia(vettoredati, context, context.Request.QueryString("prew") = "1")
                Else


                    printWord(vettoredati, context)

                    vettoredati(3) = "pdf"
                    vettoredati(4) = "application/pdf"
                End If

            End If

            If vettoredati(0) = 0 Then
                binarioFile = vettoredati(1)
                context.Response.ContentType = vettoredati(4) & ""





                If UCase(vettoredati(6)) & "" = "1" Then
                    context.Response.ClearHeaders()
                    context.Response.Buffer = True
                    context.Response.Expires = -1
                    ' ----- imposta le headers
                    context.Response.Clear()
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" & vettoredati(2) & "." & vettoredati(3))
                    context.Response.AddHeader("Content-Length", binarioFile.Length.ToString())
                    ' leggo dal file e scrivo nello stream di risposta
                    context.Response.BinaryWrite(binarioFile)
                    context.Response.End()
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" & vettoredati(2) & "." & vettoredati(3))
                    context.Response.Flush()
                Else
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" & vettoredati(2) & "." & vettoredati(3))
                    context.Response.AddHeader("Content-Length", binarioFile.Length.ToString())
                    context.Response.AddFileDependency(CStr(vettoredati(2) + "." + vettoredati(3)))
                    context.Response.ContentType = vettoredati(4) & ""
                    context.Response.BinaryWrite(binarioFile)

                    context.Response.Flush()
                    context.Response.End()
                    'context.Response.Close()
                End If
            End If

            context.Session.Remove("idTestoDocumento")
            context.Session.Remove("idDocumento")
        Catch ex As Exception
            Log.Debug("Stampa pdf --> " + ex.Message)
        End Try
    End Sub

    Private Function CreaDocumentoAlsia(ByRef vettoredati As Object, ByRef context As HttpContext, ByVal flagIsPrew As Boolean) As Byte()
        Try
            Dim arrByteReturn() As Byte = Nothing
            Dim nomePDFGenerato As String = vettoredati(2)
            nomePDFGenerato = nomePDFGenerato.Replace("/", "_").Replace("\", "_")

            Dim codufficio As String = ""
            Dim Ufficio As String = ""
            Dim numdoc As String = ""
            Dim numCronologicoDoc As String = ""
            Dim datadoc As String = ""

            Dim stampaNumerodata As Boolean = False
            Dim vettDirGen(3) As Object
            '(0) note
            '(1) Cognome e Nome
            '(2) Data Firma
            '(3) utente
            Dim vettDirReg(3) As Object
            Dim vettContAmm(3) As Object
            Dim vettUfficioProp(3) As Object
            Dim lstr_Oggetto As String = ""
            Dim lstr_TipoPubblicazione As String = ""
            Dim strConfig As String = ""
            Select Case CInt(context.Session("tipoApplic"))
                Case 0 'Determine
                    strConfig = "Determine"
                Case 1 'Delibere
                    strConfig = "Delibere"
                Case 2 'Disposizione
                    strConfig = "Disposizioni"
            End Select


            'Note (0)
            'Cognome nome (1)
            'Data(2)
            'operatore(2)
            Dim op As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Dim objDocumento As New DllDocumentale.svrDocumenti(op)
            Dim vParm(1) As Object


            Dim flagDisattiva As Boolean = True
            Dim flagStampaUffProponente As Boolean = True
            Dim isUDD As Boolean = False

            Dim descrDipartimento As String = ""

            If idDocFromAllegato = "" Then
                idDocFromAllegato = context.Session("idDocumento")
            End If
            Dim objDoc As DocumentoInfo = Leggi_Documento_Object(idDocFromAllegato)



            Try
                codufficio = objDoc.Doc_Cod_Uff_Pubblico
                Ufficio = objDoc.Doc_Descrizione_ufficio
                numdoc = objDoc.Doc_numero
                numCronologicoDoc = objDoc.Doc_NumCronologico
                If IsDate(objDoc.Doc_Data) Then
                    datadoc = objDoc.Doc_Data.Day & "/" & objDoc.Doc_Data.Month & "/" & objDoc.Doc_Data.Year
                End If
                Dim memStream As MemoryStream
                memStream = InserisciValoreInPDF_Alsia(context.Application("NOME_ENTE_INSTALLAZIONE"), vettoredati(1), strConfig, Ufficio, codufficio, descrDipartimento, stampaNumerodata, numCronologicoDoc, numdoc, datadoc, flagDisattiva, vettDirGen, vettDirReg, vettContAmm, vettUfficioProp, flagStampaUffProponente, isUDD, lstr_Oggetto, lstr_TipoPubblicazione, flagIsPrew)
                arrByteReturn = memStream.GetBuffer
                memStream.Close()
            Catch ex As Exception
                Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
            End Try

            If indexAllegato = "" Then
                context.Session.Add("nomeDocumentoFirmato", nomePDFGenerato)
                context.Session.Add("estensioneDocumentoFirmato", "pdf")
            Else
                Dim vCodiciDoc As Object = HttpContext.Current.Session.Item("vCodiciDoc")


                vCodiciDoc(indexAllegato, 3) = nomePDFGenerato
                vCodiciDoc(indexAllegato, 4) = "pdf"

            End If

            Return arrByteReturn
        Catch e As Exception
            Return Nothing
        End Try

    End Function
    Private Function InserisciValoreInPDF_Alsia(ByVal Ente As String, ByVal pdfByte() As Byte, ByVal strConfigTipoDocumento As String, ByVal Ufficio As String, ByVal codufficio As String, ByVal descDipartimento As String, ByVal stampaNumerodata As Boolean, ByVal numCronologicoDoc As String, ByVal numdoc As String, ByVal datadoc As String, ByVal flagdisattiva As Boolean, ByVal vettDirGen() As Object, ByVal vettDirReg() As Object, ByVal vettContAmm() As Object, ByVal vettUfficioProp() As Object, ByVal flagStampaUffProponente As Boolean, ByVal isUDD As Boolean, Optional ByVal lstr_Oggetto As String = "", Optional ByVal lstr_TipoPubblicazione As String = "", Optional ByVal flagIsPrew As Boolean = False, Optional ByVal dataCron As String = "") As MemoryStream
        Try

            Dim memStreamReturn As MemoryStream


            Dim f As Integer = 1
            ' we create a reader for a certain document
            Dim reader As New PdfReader(pdfByte)
            ' we retrieve the total number of pages
            Dim n As Integer = reader.NumberOfPages

            ' step 1: creation of a document-object
            Dim document As New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
            ' step 2: we create a writer that listens to the document

            memStreamReturn = New MemoryStream
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, memStreamReturn)
            ' step 3: we open the document
            document.Open()
            Dim cb As PdfContentByte = writer.DirectContent
            Dim cb1 As PdfContentByte = writer.DirectContent
            Dim page As PdfImportedPage
            Dim rotation As Integer
            ' step 4: we add content

            Dim xNumeroCronologico As Single = 365
            Dim yNumeroCronologico As Single = 782
            Dim xDataCronologico As Single = 365
            Dim yDataCronologico As Single = 902

            Dim xNumero As Single = 365
            Dim yNumero As Single = 682

            Dim xData As Single = 365
            Dim yData As Single = 682

            Dim xNumero2 As Single = 365
            Dim yNumero2 As Single = 682

            Dim xData2 As Single = 365
            Dim yData2 As Single = 682

            


            
            Try
                xNumeroCronologico = ConfigurationManager.AppSettings("xNumeroCronologico" & strConfigTipoDocumento & Ente)
                yNumeroCronologico = ConfigurationManager.AppSettings("yNumeroCronologico" & strConfigTipoDocumento & Ente)
                xDataCronologico = ConfigurationManager.AppSettings("xDataCronologico" & strConfigTipoDocumento & Ente)
                yDataCronologico = ConfigurationManager.AppSettings("yDataCronologico" & strConfigTipoDocumento & Ente)
                xNumero = ConfigurationManager.AppSettings("xNumero" & strConfigTipoDocumento & Ente)
                yNumero = ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento & Ente)
                xData = ConfigurationManager.AppSettings("xData" & strConfigTipoDocumento & Ente)
                yData = ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento & Ente)
                xNumero2 = ConfigurationManager.AppSettings("xNumero2" & strConfigTipoDocumento & Ente)
                yNumero2 = ConfigurationManager.AppSettings("yNumero2" & strConfigTipoDocumento & Ente)
                xData2 = ConfigurationManager.AppSettings("xData2" & strConfigTipoDocumento & Ente)
                yData2 = ConfigurationManager.AppSettings("yData2" & strConfigTipoDocumento & Ente)

            Catch ex As Exception
                Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfigTipoDocumento)
            End Try

            Dim i As Integer = 0


            Dim flagStampaNCont As Boolean = True
            If Not flagIsPrew Then
                If "" & vettDirReg(1) = "" Then
                    flagStampaNCont = False
                End If
            End If

            While i < n
                i += 1

                document.SetPageSize(reader.GetPageSizeWithRotation(i))
                document.NewPage()
               
                page = writer.GetImportedPage(reader, i)
                rotation = reader.GetPageRotation(i)
                If rotation = 90 OrElse rotation = 270 Then
                    cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                Else
                    cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                End If
                If (i = 1) Then



                    If Not String.IsNullOrEmpty(numCronologicoDoc) Then

                        Dim TabellaNumeroCronologico As PdfPTable = New PdfPTable(1)
                        TabellaNumeroCronologico.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                        TabellaNumeroCronologico.AddCell(numCronologicoDoc)
                        TabellaNumeroCronologico.TotalWidth = 225
                        TabellaNumeroCronologico.WriteSelectedRows(0, -1, xNumeroCronologico, yNumeroCronologico, writer.DirectContent)
                        Dim TabellaData As PdfPTable = New PdfPTable(1)
                        TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaData.AddCell(dataCron)
                        TabellaData.TotalWidth = 225
                        TabellaData.WriteSelectedRows(0, -1, xDataCronologico, yDataCronologico, writer.DirectContent)
                    End If

                    If Not String.IsNullOrEmpty(numdoc) Then
                        Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                        TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                        TabellaNumero.AddCell(numdoc)
                        TabellaNumero.TotalWidth = 225
                        TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)
                        Dim TabellaData As PdfPTable = New PdfPTable(1)
                        TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaData.AddCell(datadoc)
                        TabellaData.TotalWidth = 225
                        TabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                    End If


                End If

                If i > 1 Then
                    If Not String.IsNullOrEmpty(numdoc) Then
                        Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                        TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                        TabellaNumero.AddCell(numdoc)
                        TabellaNumero.TotalWidth = 225
                        TabellaNumero.WriteSelectedRows(0, -1, xNumero2, yNumero2, writer.DirectContent)
                        Dim TabellaData As PdfPTable = New PdfPTable(1)
                        TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaData.AddCell(datadoc)
                        TabellaData.TotalWidth = 225
                        TabellaData.WriteSelectedRows(0, -1, xData2, yData2, writer.DirectContent)
                    End If


                End If


            End While



            f += 1


            document.Close()
            Return memStreamReturn


        Catch e As Exception
            Return Nothing
        End Try

    End Function
End Class
