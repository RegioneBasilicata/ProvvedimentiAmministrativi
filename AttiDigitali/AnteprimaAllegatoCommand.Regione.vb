Imports System.Collections.Generic
Imports Microsoft.Office.Interop
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.IO.File
Imports System.Runtime.Remoting.Contexts
Imports DllAmbiente
Imports DllDocumentale.Model


Partial Class AnteprimaAllegatoCommand

    Protected Sub Execute_Regione(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim idTestoDocumento As String = ""
        Dim binarioFile() As Byte
        Dim isDocP7m As String = "False"
        Dim isDocTsr As String = "False"

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

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        If oOperatore.Test_Gruppo("RespUfRg") Then
            Log.Info("**************************************")
            Log.Info("****** Operatore " & oOperatore.Codice & " Inizio download del file ")
            Log.Info("**************************************")
        End If

        Dim oDllDocumenti As New DllDocumentale.svrDocumenti(HttpContext.Current.Session.Item("oOperatore"))

        codAllegato = context.Request.QueryString.Get("key")

        If context.Request.QueryString.Get("docP7m") <> "" Then
            isDocP7m = context.Request.QueryString.Get("docP7m")
        End If

        If context.Request.QueryString.Get("docTsr") <> "" Then
            isDocTsr = context.Request.QueryString.Get("docTsr")
        End If


        vettoredati = Anteprima_Allegato(codAllegato, isDocP7m, isDocTsr)
        
        idDocFromAllegato = vettoredati(7) & ""

        Log.Debug("ANTEPRIMA ALLEG - Id Allegato: " & codAllegato)
        Log.Debug("ANTEPRIMA ALLEG - Rit. Anteprima_Allegato: " & vettoredati(0))
        Try
            If context.Request.QueryString.Item("idx") <> "" Then
                indexAllegato = context.Request.QueryString.Item("idx")
            End If
            If context.Request.QueryString.Item("pdf") = 1 Then
                ''Stampa pdf
                If vettoredati(3) = "pdf" Then
                    vettoredati(1) = CreaDocumento(vettoredati, context, context.Request.QueryString("prew") = "1")
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

            If oOperatore.Test_Gruppo("RespUfRg") Then
                Log.Info("**************************************")
                Log.Info("****** Operatore " & oOperatore.Codice & " Fine download del file ")
                Log.Info("**************************************")
            End If

            context.Session.Remove("idTestoDocumento")
            context.Session.Remove("idDocumento")
        Catch ex As Exception
            Log.Debug("Stampa pdf --> " + ex.Message)
        End Try



    End Sub



    
    Sub addInfoRagAssLiquidazionePrmaPaginaDetermina_new_model(ByRef writer As PdfWriter, ByRef listaPreImp As IList, ByRef listaImp As IList, ByRef listaLiq As IList, ByRef listaRid As IList, ByRef listaAccert As IList, ByVal strConfigTipoDocumento As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal dataCreazioneDocumento As Date = Nothing, Optional ByVal dataCambioModello As Date = Nothing, Optional ByVal costanteVersioneModello As String = "")
        Dim ft As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD) 'New Font(iTextSharp.text.Font., 10, iTextSharp.text.Font.BOLD)

        'Dim impegno As Single = 548

        Dim contaRighe As Integer = 3
        Dim linea As Integer = 0
        Dim ContinuaInAppendice As String = ""

        Dim xContinuaAppendice As Single = 0
        Dim yContinuaAppendice As Single = 0

        'PREIMPEGNI
        Dim Ypreimpegno As Single = 460
        Dim Xpreimpegno As Single = 36
        Dim XpreimpegnoEsercizio As Single = 103
        Dim XUPBpreimpegno As Single = 145
        Dim XMissioneProgrammaPreimpegno As Single = XUPBpreimpegno
        Dim XCapitoloPreimpegno As Single = 202
        Dim XCostoPreimpegno As Single = 255


        Try

            Ypreimpegno = ConfigurationManager.AppSettings("Ypreimpegno")
            Xpreimpegno = ConfigurationManager.AppSettings("Xpreimpegno")
            XpreimpegnoEsercizio = ConfigurationManager.AppSettings("XpreimpegnoEsercizio")
            XUPBpreimpegno = ConfigurationManager.AppSettings("XUPBpreimpegno")
            XMissioneProgrammaPreimpegno = ConfigurationManager.AppSettings("XMissioneProgrammaPreimpegno")
            XCapitoloPreimpegno = ConfigurationManager.AppSettings("XCapitoloPreimpegno")
            XCostoPreimpegno = ConfigurationManager.AppSettings("XCostoPreimpegno")

            ContinuaInAppendice = ConfigurationManager.AppSettings("MSGCONTINUAPPENDICE")
        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina")
        End Try

        If ContinuaInAppendice = "1" Then
            xContinuaAppendice = Xpreimpegno + 60
            yContinuaAppendice = Ypreimpegno + 35


            If listaPreImp.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If


        Dim ragPreimpegno As DllDocumentale.ItemImpegnoInfo
        While listaPreImp.Count > 0 And linea < contaRighe
            ragPreimpegno = listaPreImp.Item(0)
            linea += 1
            If Not ragPreimpegno Is Nothing Then

                Dim TabellaNum As PdfPTable = New PdfPTable(1)
                TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaNum.AddCell(New Phrase(ragPreimpegno.Dli_NPreImpegno, ft))
                TabellaNum.TotalWidth = 100
                TabellaNum.WriteSelectedRows(0, -1, Xpreimpegno, Ypreimpegno, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragPreimpegno.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XpreimpegnoEsercizio, Ypreimpegno, writer.DirectContent)

                If Not ragPreimpegno.Dli_MissioneProgramma Is Nothing AndAlso Not ragPreimpegno.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragPreimpegno.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaPreimpegno, Ypreimpegno, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragPreimpegno.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPBpreimpegno, Ypreimpegno, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragPreimpegno.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitoloPreimpegno, Ypreimpegno, writer.DirectContent)


                If ragPreimpegno.Dli_Costo > 0 Then

                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragPreimpegno.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCostoPreimpegno, Ypreimpegno, writer.DirectContent)
                End If

                Ypreimpegno -= 16

                listaPreImp.Remove(ragPreimpegno)
            End If
        End While

        'IMPEGNO
        Dim XimpegnoAss As Single = 36
        Dim YimpegnoAss As Single = 460
        Dim XEsercizio As Single = 103
        Dim XUPB As Single = 145
        Dim XMissioneProgramma As Single = XUPB
        Dim XCapitolo As Single = 202
        Dim XCosto As Single = 255
        Dim XTipo As Single = 352
        Dim XNPren As Single = 390
        Dim XAnno As Single = 462
        Dim XPerente As Single = 500
        Dim XCoGes As Single = 520


        Try

            XimpegnoAss = ConfigurationManager.AppSettings("XimpegnoAss")
            If (dataCreazioneDocumento <> Nothing) Then
                If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                    YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAss" & costanteVersioneModello)
                Else
                    YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAss")
                End If
            End If

            XEsercizio = ConfigurationManager.AppSettings("XEsercizio")
            XUPB = ConfigurationManager.AppSettings("XUPB")
            XMissioneProgramma = ConfigurationManager.AppSettings("XMissioneProgramma")
            XCapitolo = ConfigurationManager.AppSettings("XCapitolo")
            XCosto = ConfigurationManager.AppSettings("XCosto")
            XTipo = ConfigurationManager.AppSettings("XTipo")
            XNPren = ConfigurationManager.AppSettings("XNPren")
            XAnno = ConfigurationManager.AppSettings("XAnno")
            XPerente = ConfigurationManager.AppSettings("XPerente")

            ContinuaInAppendice = ConfigurationManager.AppSettings("MSGCONTINUAPPENDICE")
        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina")
        End Try

        If ContinuaInAppendice = "1" Then
            xContinuaAppendice = XimpegnoAss + 40
            yContinuaAppendice = YimpegnoAss + 40


            If listaImp.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If


        Dim ragAss As DllDocumentale.ItemImpegnoInfo
        While listaImp.Count > 0 And linea < contaRighe
            ragAss = listaImp.Item(0)
            linea += 1
            If Not ragAss Is Nothing Then
                If flagInserisciNImpegni Then
                    If "" & ragAss.Dli_NumImpegno <> "0" Then
                        Dim TabellaNum As PdfPTable = New PdfPTable(1)
                        TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNum.AddCell(New Phrase(ragAss.Dli_NumImpegno, ft))
                        TabellaNum.TotalWidth = 100
                        TabellaNum.WriteSelectedRows(0, -1, XimpegnoAss, YimpegnoAss, writer.DirectContent)

                    End If

                End If

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragAss.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizio, YimpegnoAss, writer.DirectContent)

                If Not ragAss.Dli_MissioneProgramma Is Nothing AndAlso Not ragAss.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragAss.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgramma, YimpegnoAss, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragAss.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPB, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragAss.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitolo, YimpegnoAss, writer.DirectContent)


                If ragAss.Dli_Costo > 0 Then

                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragAss.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCosto, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaTipo As PdfPTable = New PdfPTable(1)
                TabellaTipo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                'DA Verificare

                ' L'impegno può essere preso a partire da un PREIMP fatto con un atto o creato generando automaticamente un preimp. 
                ' (ragAss.Di_PreImpDaPrenotazione: 0->preimp generato automaticamente; 1->c'è un preimpegno fatto con un atto)
                ' Verifico se si tratta di un preimp preso con un atto specifico (tipoAssunz: 0->DET o 1->DEL) opp no.
                ' Se il PREIMP è stato assunto appositamente con un atto lo trovo nella tabella Documento_preimpegno,
                ' se è stato invece generato automaticam, non lo trovo, quindi sono certa che il tuo tipo assunzione del preimp sia DET
                Dim objDocLib As New DllDocumentale.svrDocumenti(HttpContext.Current.Session("oOperatore"))
                If ragAss.Di_PreImpDaPrenotazione Then
                    Dim listaPreImpegni As IList = objDocLib.FO_Get_DatiPreImpegni(, , ragAss.Dli_NPreImpegno)
                    If Not listaPreImpegni Is Nothing AndAlso listaPreImpegni.Count > 0 Then
                        Dim preimp As DllDocumentale.ItemImpegnoInfo = listaPreImpegni.Item(0)
                        If Not preimp Is Nothing Then
                            If preimp.Di_TipoAssunzione = 1 Then
                                TabellaTipo.AddCell(New Phrase("DEL", ft))
                            ElseIf preimp.Di_TipoAssunzione = 0 Then
                                TabellaTipo.AddCell(New Phrase("DET", ft))
                            End If
                        End If
                    End If
                Else
                    TabellaTipo.AddCell(New Phrase("DET", ft))
                End If
                TabellaTipo.TotalWidth = 100
                TabellaTipo.WriteSelectedRows(0, -1, XTipo, YimpegnoAss, writer.DirectContent)


                Dim TabellaAnno As PdfPTable = New PdfPTable(1)
                TabellaAnno.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaAnno.AddCell(New Phrase(ragAss.DBi_Anno, ft))
                TabellaAnno.TotalWidth = 100
                TabellaAnno.WriteSelectedRows(0, -1, XAnno, YimpegnoAss, writer.DirectContent)


                'If flagInserisciNImpegni Or (ragAss.Di_PreImpDaPrenotazione) Then
                Dim TabellaPrenot As PdfPTable = New PdfPTable(1)
                TabellaPrenot.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaPrenot.AddCell(New Phrase(ragAss.Dli_NPreImpegno, ft))
                TabellaPrenot.TotalWidth = 100
                TabellaPrenot.WriteSelectedRows(0, -1, XNPren, YimpegnoAss, writer.DirectContent)
                'End If

                Dim TabellaPeren As PdfPTable = New PdfPTable(1)
                TabellaPeren.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaPeren.AddCell(New Phrase(ragAss.NDocPrecedente, ft))
                TabellaPeren.TotalWidth = 100
                TabellaPeren.WriteSelectedRows(0, -1, XPerente, YimpegnoAss, writer.DirectContent)

                YimpegnoAss -= 16

                listaImp.Remove(ragAss)
            End If
        End While


        'LIQUIDAZIONI
        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        Dim XliquidazioneLiq As Single = 36 'da introdurre
        Dim YliquidazioneLiq As Single = 360 'da introdurre
        Dim XEsercizioLiq As Single = 103 'da introdurre
        Dim XUPBLiq As Single = 149 'da introdurre
        Dim XMissioneProgrammaLiq As Single = XUPBLiq 'da introdurre
        Dim XCapitoloLiq As Single = 204 'da introdurre
        Dim XCostoLiq As Single = 256 'da introdurre
        Dim XNImpegnoLiq As Single = 352 'da introdurre
        Dim XTipoLiq As Single = 425 'da introdurre
        Dim XAnnoLiq As Single = 460 'da introdurre
        Dim XNumAttoLiq As Single = 460 'da introdurre
        Dim XDataAttoLiq As Single = 510 'da introdurre
        Dim XCoGesLiq As Single = 530 'da introdurre


        Try
            XliquidazioneLiq = ConfigurationManager.AppSettings("XliquidazioneLiq")

            If (dataCreazioneDocumento <> Nothing) Then
                If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                    YliquidazioneLiq = ConfigurationManager.AppSettings("YliquidazioneLiq" & costanteVersioneModello)
                Else
                    YliquidazioneLiq = ConfigurationManager.AppSettings("YliquidazioneLiq")
                End If
            End If

            XEsercizioLiq = ConfigurationManager.AppSettings("XEsercizioLiq")
            XUPBLiq = ConfigurationManager.AppSettings("XUPBLiq")
            XMissioneProgrammaLiq = ConfigurationManager.AppSettings("XMissioneProgrammaLiq")
            XCapitoloLiq = ConfigurationManager.AppSettings("XCapitoloLiq")
            XCostoLiq = ConfigurationManager.AppSettings("XCostoLiq")
            XNImpegnoLiq = ConfigurationManager.AppSettings("XNImpegnoLiq")
            XTipoLiq = ConfigurationManager.AppSettings("XTipoLiq")
            XAnnoLiq = ConfigurationManager.AppSettings("XAnnoLiq")
            XNumAttoLiq = ConfigurationManager.AppSettings("XNumAttoLiq")
            XDataAttoLiq = ConfigurationManager.AppSettings("XDataAttoLiq")
            '   XCoGesLiq = ConfigurationManager.AppSettings("XCoGesLiq")


        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina per Liquidazione")
        End Try




        linea = 0
        If ContinuaInAppendice Then
            xContinuaAppendice = XliquidazioneLiq + 67
            yContinuaAppendice = YliquidazioneLiq + 41

            If listaLiq.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If


        While listaLiq.Count > 0 And linea < contaRighe
            ragLiq = listaLiq.Item(0)
            linea += 1
            If Not ragLiq Is Nothing Then
                If flagInserisciNImpegni Then

                    If "" & ragLiq.Dli_NLiquidazione <> "0" Then
                        Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                        TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                        TabellaNumLiq.TotalWidth = 100
                        TabellaNumLiq.WriteSelectedRows(0, -1, XliquidazioneLiq, YliquidazioneLiq, writer.DirectContent)

                    End If

                End If

                If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaLiq, YliquidazioneLiq, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPBLiq, YliquidazioneLiq, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitoloLiq, YliquidazioneLiq, writer.DirectContent)


                'Dim TabellaCoGes As PdfPTable = New PdfPTable(1)
                'TabellaCoGes.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                'TabellaCoGes.AddCell(New Phrase(ragAss.Codice_Obbiettivo_Gestionale, ft))
                'TabellaCoGes.TotalWidth = 100
                'TabellaCoGes.WriteSelectedRows(0, -1, XCoGesLiq, YimpegnoAss, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioLiq, YliquidazioneLiq, writer.DirectContent)

                If ragLiq.Dli_Costo > 0 Then
                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragLiq.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCostoLiq, YliquidazioneLiq, writer.DirectContent)
                End If



                If "" & ragLiq.Dli_NumImpegno <> "0" Then
                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoLiq, YliquidazioneLiq, writer.DirectContent)


                    If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                        Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                        TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                        TabellaNumAs.TotalWidth = 100
                        TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoLiq, YliquidazioneLiq, writer.DirectContent)


                        Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                        TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                        TabellaDataAs.TotalWidth = 100
                        TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoLiq, YliquidazioneLiq, writer.DirectContent)


                    End If


                End If


                If ragLiq.Dli_TipoAssunzione = 1 Then

                    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDel.AddCell(New Phrase("DEL", ft))
                    TabellaDel.TotalWidth = 100
                    TabellaDel.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiq, writer.DirectContent)
                End If

                If ragLiq.Dli_TipoAssunzione = 0 Then
                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("DET", ft))
                    TabellaDet.TotalWidth = 100
                    TabellaDet.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiq, writer.DirectContent)
                End If
                YliquidazioneLiq -= 16

                listaLiq.Remove(ragLiq)
            End If



        End While

        ' RIDUZIONI
        Dim ragRid As DllDocumentale.ItemRiduzioneInfo
        Dim ragRidLiq As DllDocumentale.ItemRiduzioneLiqInfo

        Dim XriduzioneRid As Single = 36
        Dim YriduzioneRid As Single = 255
        Dim XEsercizioRid As Single = 103
        Dim XUPBRid As Single = 149
        Dim XMissioneProgrammaRid As Single = XUPBRid
        Dim XCapitoloRid As Single = 204
        Dim XCostoRid As Single = 255
        Dim XNImpegnoRid As Single = 352
        Dim XTipoRid As Single = 425
        Dim XAnnoRid As Single = 460
        Dim XNumAttoRid As Single = 460
        Dim XDataAttoRid As Single = 510



        Try
            XriduzioneRid = ConfigurationManager.AppSettings("XriduzioneRid")

            If (dataCreazioneDocumento <> Nothing) Then
                If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                    YriduzioneRid = ConfigurationManager.AppSettings("YriduzioneRid" & costanteVersioneModello)
                Else
                    YriduzioneRid = ConfigurationManager.AppSettings("YriduzioneRid")
                End If
            End If

            XEsercizioRid = ConfigurationManager.AppSettings("XEsercizioRid")
            XUPBRid = ConfigurationManager.AppSettings("XUPBRid")
            XMissioneProgrammaRid = ConfigurationManager.AppSettings("XMissioneProgrammaRid")
            XCapitoloRid = ConfigurationManager.AppSettings("XCapitoloRid")
            XCostoRid = ConfigurationManager.AppSettings("XCostoRid")
            XNImpegnoRid = ConfigurationManager.AppSettings("XNImpegnoRid")
            XTipoRid = ConfigurationManager.AppSettings("XTipoRid")
            XAnnoRid = ConfigurationManager.AppSettings("XAnnoRid")
            XNumAttoRid = ConfigurationManager.AppSettings("XNumAttoRid")
            XDataAttoRid = ConfigurationManager.AppSettings("XDataAttoRid")


        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina per Riduzioni")
        End Try




        If ContinuaInAppendice Then
            xContinuaAppendice = XriduzioneRid + 180
            yContinuaAppendice = YriduzioneRid + 47
            If listaRid.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If
        linea = 0

        While listaRid.Count > 0 And linea < contaRighe

            If (listaRid.Item(0).GetType) Is GetType(DllDocumentale.ItemRiduzioneInfo) Then
                ragRid = listaRid.Item(0)
                linea += 1
                If Not ragRid Is Nothing Then
                    If flagInserisciNImpegni Then
                        If "" & ragRid.Div_NumeroReg <> "0" Then
                            Dim TabellaNumRid As PdfPTable = New PdfPTable(1)
                            TabellaNumRid.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            TabellaNumRid.AddCell(New Phrase(ragRid.Div_NumeroReg, ft))
                            TabellaNumRid.TotalWidth = 100
                            TabellaNumRid.WriteSelectedRows(0, -1, XriduzioneRid, YriduzioneRid, writer.DirectContent)

                        End If
                    End If

                    If Not ragRid.Dli_MissioneProgramma Is Nothing AndAlso Not ragRid.Dli_MissioneProgramma.Trim() = String.Empty Then
                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaMissioneProgramma.AddCell(New Phrase(ragRid.Dli_MissioneProgramma, ft))
                        TabellaMissioneProgramma.TotalWidth = 100
                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaRid, YriduzioneRid, writer.DirectContent)
                    Else
                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaUPB.AddCell(New Phrase(ragRid.Dli_UPB, ft))
                        TabellaUPB.TotalWidth = 100
                        TabellaUPB.WriteSelectedRows(0, -1, XUPBRid, YriduzioneRid, writer.DirectContent)
                    End If

                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCap.AddCell(New Phrase(ragRid.Dli_Cap, ft))
                    TabellaCap.TotalWidth = 100
                    TabellaCap.WriteSelectedRows(0, -1, XCapitoloRid, YriduzioneRid, writer.DirectContent)

                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaEsercizio.AddCell(New Phrase(ragRid.Dli_Esercizio, ft))
                    TabellaEsercizio.TotalWidth = 100
                    TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioRid, YriduzioneRid, writer.DirectContent)

                    If ragRid.Dli_Costo > 0 Then
                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragRid.Dli_Costo), ft))
                        TabellaCosto.TotalWidth = 100
                        TabellaCosto.WriteSelectedRows(0, -1, XCostoRid, YriduzioneRid, writer.DirectContent)
                    End If



                    If "" & ragRid.Dli_NumImpegno <> "0" Then
                        Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                        TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        Dim lstr_impl As String = ""
                        If ragRid.IsPreImp Then
                            lstr_impl = ragRid.Dli_NPreImpegno & "-PRE"
                        Else
                            lstr_impl = ragRid.Dli_NumImpegno & "-IMP"
                        End If

                        'TabellaNumImpcon.AddCell(New Phrase(ragRid.Dli_NumImpegno, ft))
                        TabellaNumImpcon.AddCell(New Phrase(lstr_impl, ft))
                        TabellaNumImpcon.TotalWidth = 100
                        TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoRid, YriduzioneRid, writer.DirectContent)


                        If "" & ragRid.Div_Num_assunzione <> "0" Then
                            Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                            TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            TabellaNumAs.AddCell(New Phrase(ragRid.Div_Num_assunzione, ft))
                            TabellaNumAs.TotalWidth = 100
                            TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoRid, YriduzioneRid, writer.DirectContent)


                            Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                            TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            TabellaDataAs.AddCell(New Phrase(ragRid.Div_Data_Assunzione.Date, ft))
                            TabellaDataAs.TotalWidth = 100
                            TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoRid, YriduzioneRid, writer.DirectContent)


                        End If


                    End If


                    If ragRid.Div_TipoAssunzione = 1 Then

                        Dim TabellaDel As PdfPTable = New PdfPTable(1)
                        TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaDel.AddCell(New Phrase("DEL", ft))
                        TabellaDel.TotalWidth = 100
                        TabellaDel.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                    End If

                    If ragRid.Div_TipoAssunzione = 0 Then
                        Dim TabellaDet As PdfPTable = New PdfPTable(1)
                        TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaDet.AddCell(New Phrase("DET", ft))
                        TabellaDet.TotalWidth = 100
                        TabellaDet.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                    End If

                    YriduzioneRid -= 16

                    listaRid.Remove(ragRid)
                End If



            Else
                'Gestione Rig Liq
                ragRidLiq = listaRid.Item(0)
                linea += 1
                If Not ragRidLiq Is Nothing Then
                    If flagInserisciNImpegni Then
                        If "" & ragRidLiq.Div_NumeroReg <> "0" Then
                            Dim TabellaNumRid As PdfPTable = New PdfPTable(1)
                            TabellaNumRid.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            TabellaNumRid.AddCell(New Phrase(ragRidLiq.Div_NumeroReg, ft))
                            TabellaNumRid.TotalWidth = 100
                            TabellaNumRid.WriteSelectedRows(0, -1, XriduzioneRid, YriduzioneRid, writer.DirectContent)

                        End If
                    End If

                    If Not ragRidLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragRidLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaMissioneProgramma.AddCell(New Phrase(ragRidLiq.Dli_MissioneProgramma, ft))
                        TabellaMissioneProgramma.TotalWidth = 100
                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaRid, YriduzioneRid, writer.DirectContent)
                    Else
                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaUPB.AddCell(New Phrase(ragRidLiq.Dli_UPB, ft))
                        TabellaUPB.TotalWidth = 100
                        TabellaUPB.WriteSelectedRows(0, -1, XUPBRid, YriduzioneRid, writer.DirectContent)
                    End If

                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCap.AddCell(New Phrase(ragRidLiq.Dli_Cap, ft))
                    TabellaCap.TotalWidth = 100
                    TabellaCap.WriteSelectedRows(0, -1, XCapitoloRid, YriduzioneRid, writer.DirectContent)

                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaEsercizio.AddCell(New Phrase(ragRidLiq.Dli_Esercizio, ft))
                    TabellaEsercizio.TotalWidth = 100
                    TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioRid, YriduzioneRid, writer.DirectContent)

                    If ragRidLiq.Dli_Costo > 0 Then
                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragRidLiq.Dli_Costo), ft))
                        TabellaCosto.TotalWidth = 100
                        TabellaCosto.WriteSelectedRows(0, -1, XCostoRid, YriduzioneRid, writer.DirectContent)
                    End If



                    If "" & ragRidLiq.Div_NLiquidazione <> "0" Then
                        Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                        TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNumImpcon.AddCell(New Phrase(ragRidLiq.Div_NLiquidazione & "-LIQ", ft))
                        TabellaNumImpcon.TotalWidth = 100
                        TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoRid, YriduzioneRid, writer.DirectContent)


                        'If "" & ragRidLiq. <> "0" Then
                        '    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                        '    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        '    TabellaNumAs.AddCell(New Phrase(ragRidLiq.Div_Num_assunzione, ft))
                        '    TabellaNumAs.TotalWidth = 100
                        '    TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoRid, YriduzioneRid, writer.DirectContent)


                        '    Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                        '    TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        '    TabellaDataAs.AddCell(New Phrase(ragRidLiq.Div_Data_Assunzione.Date, ft))
                        '    TabellaDataAs.TotalWidth = 100
                        '    TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoRid, YriduzioneRid, writer.DirectContent)


                        'End If


                    End If


                    'If ragRidLiq.Div_TipoAssunzione = 1 Then

                    '    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    '    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    '    TabellaDel.AddCell(New Phrase("DEL", ft))
                    '    TabellaDel.TotalWidth = 100
                    '    TabellaDel.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                    'End If

                    'If ragRidLiq.Div_TipoAssunzione = 0 Then
                    '    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    '    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    '    TabellaDet.AddCell(New Phrase("DET", ft))
                    '    TabellaDet.TotalWidth = 100
                    '    TabellaDet.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                    'End If

                    YriduzioneRid -= 16

                    listaRid.Remove(ragRidLiq)
                End If





            End If


        End While

        Dim XAccertamento As Single = 130
        Dim YAccertamento As Single = 190


        Try
            XAccertamento = ConfigurationManager.AppSettings("XAccertamento" & strConfigTipoDocumento)
            YAccertamento = ConfigurationManager.AppSettings("YAccertamento" & strConfigTipoDocumento)


        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina per Accertamento")
        End Try

        If listaAccert.Count > 0 Then
            Dim accerta As DllDocumentale.ItemAssunzioneContabileInfo = listaAccert(listaAccert.Count - 1)

            If accerta.Da_Costo > 0 And accerta.Da_Stato = 1 Then
                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", accerta.Da_Costo), ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, XAccertamento, YAccertamento, writer.DirectContent)
            End If

        End If



    End Sub
		
    Sub addInfoContabiliPrimaPaginaDelibera_new_model(ByRef writer As PdfWriter, ByRef listaPreImp As IList, ByRef listaImp As IList, ByVal strConfigTipoDocumento As String, Optional ByVal flagInserisciNImpegni As Boolean = True)
        Dim ft As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)
        Dim contaRighe As Integer = 3
        Dim linea As Integer = 0
        Dim ContinuaInAppendice As String = ""

        Dim xContinuaAppendice As Single = 0
        Dim yContinuaAppendice As Single = 0

        'PREIMPEGNI
        Dim Ypreimpegno As Single = 460
        Dim Xpreimpegno As Single = 36
        Dim XpreimpegnoEsercizio As Single = 103
        Dim XUPBpreimpegno As Single = 145
        Dim XMissioneProgrammaPreimpegno As Single = XUPBpreimpegno
        Dim XCapitoloPreimpegno As Single = 202
        Dim XCostoPreimpegno As Single = 255

        Try
            Ypreimpegno = ConfigurationManager.AppSettings("Ypreimpegno" & strConfigTipoDocumento)
            Xpreimpegno = ConfigurationManager.AppSettings("Xpreimpegno" & strConfigTipoDocumento)
            XpreimpegnoEsercizio = ConfigurationManager.AppSettings("XpreimpegnoEsercizio" & strConfigTipoDocumento)
            XUPBpreimpegno = ConfigurationManager.AppSettings("XUPBpreimpegno" & strConfigTipoDocumento)
            XMissioneProgrammaPreimpegno = ConfigurationManager.AppSettings("XMissioneProgrammaPreimpegno" & strConfigTipoDocumento)
            XCapitoloPreimpegno = ConfigurationManager.AppSettings("XCapitoloPreimpegno" & strConfigTipoDocumento)
            XCostoPreimpegno = ConfigurationManager.AppSettings("XCostoPreimpegno" & strConfigTipoDocumento)

            ContinuaInAppendice = ConfigurationManager.AppSettings("MSGCONTINUAPPENDICE")
        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina")
        End Try

        If ContinuaInAppendice = "1" Then
            xContinuaAppendice = Xpreimpegno + 90
            yContinuaAppendice = Ypreimpegno + 33


            If listaPreImp.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If

        Dim ragPreimpegno As DllDocumentale.ItemImpegnoInfo
        While listaPreImp.Count > 0 And linea < contaRighe
            ragPreimpegno = listaPreImp.Item(0)
            linea += 1
            If Not ragPreimpegno Is Nothing Then

                Dim TabellaNum As PdfPTable = New PdfPTable(1)
                TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaNum.AddCell(New Phrase(ragPreimpegno.Dli_NPreImpegno, ft))
                TabellaNum.TotalWidth = 100
                TabellaNum.WriteSelectedRows(0, -1, Xpreimpegno, Ypreimpegno, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragPreimpegno.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XpreimpegnoEsercizio, Ypreimpegno, writer.DirectContent)

                If Not ragPreimpegno.Dli_MissioneProgramma Is Nothing AndAlso Not ragPreimpegno.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragPreimpegno.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaPreimpegno, Ypreimpegno, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragPreimpegno.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPBpreimpegno, Ypreimpegno, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragPreimpegno.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitoloPreimpegno, Ypreimpegno, writer.DirectContent)

                If ragPreimpegno.Dli_Costo > 0 Then

                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragPreimpegno.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCostoPreimpegno, Ypreimpegno, writer.DirectContent)
                End If

                Ypreimpegno -= 16

                listaPreImp.Remove(ragPreimpegno)
            End If
        End While

        'IMPEGNO
        Dim XimpegnoAss As Single = 36
        Dim YimpegnoAss As Single = 460
        Dim XEsercizio As Single = 103
        Dim XUPB As Single = 145
        Dim XMissioneProgramma As Single = XUPB
        Dim XCapitolo As Single = 202
        Dim XCosto As Single = 255
        Dim XTipo As Single = 352
        Dim XNPren As Single = 390
        Dim XAnno As Single = 462
        Dim XPerente As Single = 500
        Dim XCoGes As Single = 520


        Try

            XimpegnoAss = IIf(ConfigurationManager.AppSettings("XimpegnoAss" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XimpegnoAss" & strConfigTipoDocumento) & "", XimpegnoAss)
            YimpegnoAss = IIf(ConfigurationManager.AppSettings("YimpegnoAss" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("YimpegnoAss" & strConfigTipoDocumento) & "", YimpegnoAss)
            XEsercizio = IIf(ConfigurationManager.AppSettings("XEsercizio" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XEsercizio" & strConfigTipoDocumento) & "", XEsercizio)
            XUPB = IIf(ConfigurationManager.AppSettings("XUPB" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XUPB" & strConfigTipoDocumento) & "", XUPB)
            XMissioneProgramma = IIf(ConfigurationManager.AppSettings("XMissioneProgramma" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XMissioneProgramma" & strConfigTipoDocumento) & "", XMissioneProgramma)
            XCapitolo = IIf(ConfigurationManager.AppSettings("XCapitolo" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XCapitolo" & strConfigTipoDocumento) & "", XCapitolo)
            XCosto = IIf(ConfigurationManager.AppSettings("XCosto" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XCosto" & strConfigTipoDocumento) & "", XCosto)
            XTipo = IIf(ConfigurationManager.AppSettings("XTipo" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XTipo" & strConfigTipoDocumento) & "", XTipo)
            XNPren = IIf(ConfigurationManager.AppSettings("XNPren" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XNPren" & strConfigTipoDocumento) & "", XNPren)
            XAnno = IIf(ConfigurationManager.AppSettings("XAnno" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XAnno" & strConfigTipoDocumento) & "", XAnno)
            XPerente = IIf(ConfigurationManager.AppSettings("XPerente" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("XPerente" & strConfigTipoDocumento) & "", XPerente)

            ContinuaInAppendice = ConfigurationManager.AppSettings("MSGCONTINUAPPENDICE")
        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina")
        End Try

        If ContinuaInAppendice = "1" Then
            xContinuaAppendice = XimpegnoAss + 40
            yContinuaAppendice = YimpegnoAss + 43


            If listaImp.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If

        linea = 0
        Dim ragAss As DllDocumentale.ItemImpegnoInfo
        While listaImp.Count > 0 And linea < contaRighe
            ragAss = listaImp.Item(0)
            linea += 1
            If Not ragAss Is Nothing Then
                If flagInserisciNImpegni Then
                    If "" & ragAss.Dli_NumImpegno <> "0" Then
                        Dim TabellaNum As PdfPTable = New PdfPTable(1)
                        TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNum.AddCell(New Phrase(ragAss.Dli_NumImpegno, ft))
                        TabellaNum.TotalWidth = 100
                        TabellaNum.WriteSelectedRows(0, -1, XimpegnoAss, YimpegnoAss, writer.DirectContent)

                    End If

                End If

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragAss.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizio, YimpegnoAss, writer.DirectContent)

                If Not ragAss.Dli_MissioneProgramma Is Nothing AndAlso Not ragAss.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragAss.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgramma, YimpegnoAss, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragAss.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPB, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragAss.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitolo, YimpegnoAss, writer.DirectContent)


                If ragAss.Dli_Costo > 0 Then

                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragAss.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCosto, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaTipo As PdfPTable = New PdfPTable(1)
                TabellaTipo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                'DA Verificare

                ' L'impegno può essere preso a partire da un PREIMP fatto con un atto o creato generando automaticamente un preimp. 
                ' (ragAss.Di_PreImpDaPrenotazione: 0->preimp generato automaticamente; 1->c'è un preimpegno fatto con un atto)
                ' Verifico se si tratta di un preimp preso con un atto specifico (tipoAssunz: 0->DET o 1->DEL) opp no.
                ' Se il PREIMP è stato assunto appositamente con un atto lo trovo nella tabella Documento_preimpegno,
                ' se è stato invece generato automaticam, non lo trovo, quindi sono certa che il tuo tipo assunzione del preimp sia DET
                Dim objDocLib As New DllDocumentale.svrDocumenti(HttpContext.Current.Session("oOperatore"))
                If ragAss.Di_PreImpDaPrenotazione Then
                    Dim listaPreImpegni As IList = objDocLib.FO_Get_DatiPreImpegni(, , ragAss.Dli_NPreImpegno)
                    If Not listaPreImpegni Is Nothing AndAlso listaPreImpegni.Count > 0 Then
                        Dim preimp As DllDocumentale.ItemImpegnoInfo = listaPreImpegni.Item(0)
                        If Not preimp Is Nothing Then
                            If preimp.Di_TipoAssunzione = 1 Then
                                TabellaTipo.AddCell(New Phrase("DEL", ft))
                            ElseIf preimp.Di_TipoAssunzione = 0 Then
                                TabellaTipo.AddCell(New Phrase("DET", ft))
                            End If
                        End If
                    Else 
                        TabellaTipo.AddCell(New Phrase("DEL", ft))
                    End If
                Else
                    'TabellaTipo.AddCell(New Phrase("DET", ft))
                    'Modifica GIO - 311017
                    TabellaTipo.AddCell(New Phrase("DEL", ft))

                End If
                TabellaTipo.TotalWidth = 100
                TabellaTipo.WriteSelectedRows(0, -1, XTipo, YimpegnoAss, writer.DirectContent)


                Dim TabellaAnno As PdfPTable = New PdfPTable(1)
                TabellaAnno.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaAnno.AddCell(New Phrase(ragAss.DBi_Anno, ft))
                TabellaAnno.TotalWidth = 100
                TabellaAnno.WriteSelectedRows(0, -1, XAnno, YimpegnoAss, writer.DirectContent)


                'If flagInserisciNImpegni Or (ragAss.Di_PreImpDaPrenotazione) Then
                Dim TabellaPrenot As PdfPTable = New PdfPTable(1)
                TabellaPrenot.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaPrenot.AddCell(New Phrase(ragAss.Dli_NPreImpegno, ft))
                TabellaPrenot.TotalWidth = 100
                TabellaPrenot.WriteSelectedRows(0, -1, XNPren, YimpegnoAss, writer.DirectContent)
                'End If

                Dim TabellaPeren As PdfPTable = New PdfPTable(1)
                TabellaPeren.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaPeren.AddCell(New Phrase(ragAss.NDocPrecedente, ft))
                TabellaPeren.TotalWidth = 100
                TabellaPeren.WriteSelectedRows(0, -1, XPerente, YimpegnoAss, writer.DirectContent)

                YimpegnoAss -= 16

                listaImp.Remove(ragAss)
            End If
        End While

    End Sub

    Sub addInfoAssLiquidazionePrmaPaginaDisposizione_new_model(ByRef writer As PdfWriter, ByRef listaLiq As IList, ByRef listaAccert As IList, ByVal strConfigTipoDocumento As String, Optional ByVal flagInserisciNImpegni As Boolean = True)
        Dim ft As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD) 'New Font(iTextSharp.text.Font., 10, iTextSharp.text.Font.BOLD)

        'Dim impegno As Single = 548

        Dim contaRighe As Integer = 4
        Dim linea As Integer = 0
        Dim XimpegnoAss As Single = 36
        Dim YimpegnoAss As Single = 460
        Dim XEsercizio As Single = 103
        Dim XUPB As Single = 145
        Dim XCapitolo As Single = 202
        Dim XCosto As Single = 255
        Dim XTipo As Single = 352
        Dim XNPren As Single = 390
        Dim XAnno As Single = 462
        Dim XPerente As Single = 500


        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        Dim XliquidazioneLiqDisposizioni As Single = 36 'da introdurre
        Dim YliquidazioneLiqDisposizioni As Single = 430 'da introdurre
        Dim XEsercizioLiq As Single = 103 'da introdurre
        Dim XUPBLiq As Single = 149 'da introdurre
        Dim XMissioneProgrammaLiq As Single = XUPBLiq 'da introdurre
        Dim XCapitoloLiq As Single = 204 'da introdurre
        Dim XCostoLiq As Single = 256 'da introdurre
        Dim XNImpegnoLiq As Single = 352 'da introdurre
        Dim XTipoLiq As Single = 425 'da introdurre
        Dim XAnnoLiq As Single = 460 'da introdurre
        Dim XNumAttoLiq As Single = 460 'da introdurre
        Dim XDataAttoLiq As Single = 510 'da introdurre
        Dim xContinuaAppendice As Single = 0
        Dim yContinuaAppendice As Single = 0

        Dim ContinuaInAppendice As String = ""





        Try


            XliquidazioneLiqDisposizioni = ConfigurationManager.AppSettings("XliquidazioneLiqDisposizioni")
            YliquidazioneLiqDisposizioni = ConfigurationManager.AppSettings("YliquidazioneLiqDisposizioni")


            XEsercizioLiq = ConfigurationManager.AppSettings("XEsercizioLiq")
            XUPBLiq = ConfigurationManager.AppSettings("XUPBLiq")
            XMissioneProgrammaLiq = ConfigurationManager.AppSettings("XMissioneProgrammaLiq")
            XCapitoloLiq = ConfigurationManager.AppSettings("XCapitoloLiq")
            XCostoLiq = ConfigurationManager.AppSettings("XCostoLiq")
            XNImpegnoLiq = ConfigurationManager.AppSettings("XNImpegnoLiq")
            XTipoLiq = ConfigurationManager.AppSettings("XTipoLiq")
            XAnnoLiq = ConfigurationManager.AppSettings("XAnnoLiq")
            XNumAttoLiq = ConfigurationManager.AppSettings("XNumAttoLiq")
            XDataAttoLiq = ConfigurationManager.AppSettings("XDataAttoLiq")

            ContinuaInAppendice = ConfigurationManager.AppSettings("MSGCONTINUAPPENDICE")

        Catch ex As Exception
            ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
        End Try





        If ContinuaInAppendice = "1" Then
            xContinuaAppendice = XliquidazioneLiqDisposizioni + 65
            yContinuaAppendice = YliquidazioneLiqDisposizioni + 46


            If listaLiq.Count > contaRighe Then
                Dim TabellaApp As PdfPTable = New PdfPTable(1)
                TabellaApp.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaApp.AddCell(New Phrase("Continua in Appendice", ft))
                TabellaApp.TotalWidth = 300
                TabellaApp.WriteSelectedRows(0, -1, xContinuaAppendice, yContinuaAppendice, writer.DirectContent)
            End If
        End If


        linea = 0

        While listaLiq.Count > 0 And linea < contaRighe
            ragLiq = listaLiq.Item(0)
            linea += 1
            If Not ragLiq Is Nothing Then
                If flagInserisciNImpegni Then

                    If "" & ragLiq.Dli_NLiquidazione <> "0" Then
                        Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                        TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                        TabellaNumLiq.TotalWidth = 100
                        TabellaNumLiq.WriteSelectedRows(0, -1, XliquidazioneLiqDisposizioni, YliquidazioneLiqDisposizioni, writer.DirectContent)

                    End If

                End If

                If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, XUPBLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, XCapitoloLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)

                If ragLiq.Dli_Costo > 0 Then
                    Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                    TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragLiq.Dli_Costo), ft))
                    TabellaCosto.TotalWidth = 100
                    TabellaCosto.WriteSelectedRows(0, -1, XCostoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)
                End If



                If "" & ragLiq.Dli_NumImpegno <> "0" Then
                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)


                    If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                        Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                        TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                        TabellaNumAs.TotalWidth = 100
                        TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)


                        Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                        TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                        TabellaDataAs.TotalWidth = 100
                        TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)


                    End If


                End If


                If ragLiq.Dli_TipoAssunzione = 1 Then

                    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDel.AddCell(New Phrase("DEL", ft))
                    TabellaDel.TotalWidth = 100
                    TabellaDel.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)
                End If

                If ragLiq.Dli_TipoAssunzione = 0 Then
                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("DET", ft))
                    TabellaDet.TotalWidth = 100
                    TabellaDet.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiqDisposizioni, writer.DirectContent)
                End If








                YliquidazioneLiqDisposizioni -= 16

                listaLiq.Remove(ragLiq)
            End If



        End While





        Dim XAccertamento As Single = 130
        Dim YAccertamento As Single = 190


        Try
            XAccertamento = ConfigurationManager.AppSettings("XAccertamento" & strConfigTipoDocumento)
            YAccertamento = ConfigurationManager.AppSettings("YAccertamento" & strConfigTipoDocumento)


        Catch ex As Exception
            Log.Error("Attenzione mancano le posizioni per la addInfoRagAssLiquidazionePrmaPaginaDetermina per Accertamento")
        End Try

        If listaAccert.Count > 0 Then
            Dim accerta As DllDocumentale.ItemAssunzioneContabileInfo = listaAccert(listaAccert.Count - 1)

            If accerta.Da_Costo > 0 And accerta.Da_Stato = 1 Then
                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", accerta.Da_Costo), ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, XAccertamento, YAccertamento, writer.DirectContent)
            End If

        End If





    End Sub
    Sub addInfoRagAssLiquidazionePrmaPaginaDetermina(ByRef writer As PdfWriter, ByRef listaImp As IList, ByRef listaLiq As IList)


        Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
        Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

        'Dim impegno As Single = 548

        Dim contaRighe As Integer = 3
        Dim linea As Integer = 0
        Dim XimpegnoAss As Single = 175
        Dim YimpegnoAss As Single = 396

        Dim ragAss As DllDocumentale.ItemImpegnoInfo
        While listaImp.Count > 0 And linea < contaRighe
            ragAss = listaImp.Item(0)
            linea += 1
            If Not ragAss Is Nothing Then

                Dim TabellaAssunto As PdfPTable = New PdfPTable(1)
                TabellaAssunto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                TabellaAssunto.AddCell(New Phrase("X", ft))
                TabellaAssunto.TotalWidth = 10
                TabellaAssunto.WriteSelectedRows(0, -1, 36, YimpegnoAss, writer.DirectContent)

                If "" & ragAss.Dli_NumImpegno <> "0" Then
                    Dim TabellaNum As PdfPTable = New PdfPTable(1)
                    TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNum.AddCell(New Phrase(ragAss.Dli_NumImpegno, ft))
                    TabellaNum.TotalWidth = 100
                    TabellaNum.WriteSelectedRows(0, -1, 175, YimpegnoAss, writer.DirectContent)

                End If

                If Not ragAss.Dli_MissioneProgramma Is Nothing AndAlso Not ragAss.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragAss.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, 250, YimpegnoAss, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragAss.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, 250, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragAss.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, 310, YimpegnoAss, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragAss.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, 390, YimpegnoAss, writer.DirectContent)

                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase(ragAss.Dli_Costo, ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, 450, YimpegnoAss, writer.DirectContent)



                If linea Mod 3 = 0 Then
                    YimpegnoAss -= 22
                Else
                    YimpegnoAss -= 23.5
                End If
                listaImp.Remove(ragAss)
            End If
        End While



        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        Dim XliquidazioneLiq As Single = 140
        Dim YliquidazioneLiq As Single = 327



        linea = 0

        While listaLiq.Count > 0 And linea < contaRighe
            ragLiq = listaLiq.Item(0)
            linea += 1
            If Not ragLiq Is Nothing Then


                Dim TabellaLiquidazione As PdfPTable = New PdfPTable(1)
                TabellaLiquidazione.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                TabellaLiquidazione.AddCell(New Phrase("X", ft))
                TabellaLiquidazione.TotalWidth = 10
                TabellaLiquidazione.WriteSelectedRows(0, -1, 36, YliquidazioneLiq, writer.DirectContent)


                If "" & ragLiq.Dli_NLiquidazione <> "0" Then
                    Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                    TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                    TabellaNumLiq.TotalWidth = 100
                    TabellaNumLiq.WriteSelectedRows(0, -1, 115, YliquidazioneLiq, writer.DirectContent)

                End If

                If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, 190, YliquidazioneLiq, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, 190, YliquidazioneLiq, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, 260, YliquidazioneLiq, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, 340, YliquidazioneLiq, writer.DirectContent)

                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase(ragLiq.Dli_Costo, ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, 400, YliquidazioneLiq, writer.DirectContent)







                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 21
                Else
                    YliquidazioneLiq -= 22.5
                End If


                If "" & ragLiq.Dli_NumImpegno <> "0" Then
                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, XliquidazioneLiq, YliquidazioneLiq, writer.DirectContent)

                End If


                If ragLiq.Dli_TipoAssunzione = 1 Then


                    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDel.AddCell(New Phrase("X", ft))
                    TabellaDel.TotalWidth = 10
                    TabellaDel.WriteSelectedRows(0, -1, 252, YliquidazioneLiq, writer.DirectContent)
                End If

                If ragLiq.Dli_TipoAssunzione = 0 Then
                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("X", ft))
                    TabellaDet.TotalWidth = 10
                    TabellaDet.WriteSelectedRows(0, -1, 320, YliquidazioneLiq, writer.DirectContent)
                End If

                If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                    TabellaNumAs.TotalWidth = 100
                    TabellaNumAs.WriteSelectedRows(0, -1, 415, YliquidazioneLiq, writer.DirectContent)


                    Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                    TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                    TabellaDataAs.TotalWidth = 100
                    TabellaDataAs.WriteSelectedRows(0, -1, 510, YliquidazioneLiq, writer.DirectContent)


                End If





                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 21
                Else
                    YliquidazioneLiq -= 22.5
                End If

                listaLiq.Remove(ragLiq)
            End If



        End While





    End Sub
    Sub addInfoAssLiquidazionePrmaPaginaDisposizione(ByRef writer As PdfWriter, ByRef listaLiq As IList, ByRef listaRAgLiq As IList)
        Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
        Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

        'Dim impegno As Single = 548

        Dim contaRighe As Integer = 3
        Dim linea As Integer = 0
        Dim XimpegnoAss As Single = 175
        Dim YimpegnoAss As Single = 530
        Dim xEsercizio As Single = 500

        Dim liq As DllDocumentale.ItemLiquidazioneInfo
        While listaLiq.Count > 0 And linea < contaRighe
            liq = listaLiq.Item(0)
            linea += 1
            If Not liq Is Nothing Then


                Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                TabellaNumLiq.AddCell(New Phrase(liq.Dli_Costo, ft))
                TabellaNumLiq.TotalWidth = 100
                TabellaNumLiq.WriteSelectedRows(0, -1, 60, YimpegnoAss, writer.DirectContent)

                If Not liq.Dli_MissioneProgramma Is Nothing AndAlso Not liq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(liq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, 170, YimpegnoAss, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(liq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, 170, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(liq.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, 340, YimpegnoAss, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(liq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, xEsercizio, YimpegnoAss, writer.DirectContent)



                If linea Mod 3 = 0 Then
                    YimpegnoAss -= 22
                Else
                    YimpegnoAss -= 23.5
                End If



                If "" & liq.Dli_NumImpegno <> "0" Then
                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(liq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, 185, YimpegnoAss, writer.DirectContent)

                End If







                Dim TabellaAnno As PdfPTable = New PdfPTable(1)
                TabellaAnno.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                TabellaAnno.AddCell(New Phrase(liq.Dli_Anno, ft))
                TabellaAnno.TotalWidth = 100
                TabellaAnno.WriteSelectedRows(0, -1, 410, YimpegnoAss, writer.DirectContent)


                If linea Mod 3 = 0 Then
                    YimpegnoAss -= 21
                Else
                    YimpegnoAss -= 22.5
                End If


                If liq.Dli_TipoAssunzione = 1 Then


                    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDel.AddCell(New Phrase("X", ft))
                    TabellaDel.TotalWidth = 10
                    TabellaDel.WriteSelectedRows(0, -1, 95, YimpegnoAss, writer.DirectContent)
                End If

                If liq.Dli_TipoAssunzione = 0 Then
                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("X", ft))
                    TabellaDet.TotalWidth = 10
                    TabellaDet.WriteSelectedRows(0, -1, 170, YimpegnoAss, writer.DirectContent)
                End If

                If "" & liq.Dli_Num_assunzione <> "0" And "" & liq.Dli_Num_assunzione <> "" Then

                    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumAs.AddCell(New Phrase(liq.Dli_Num_assunzione, ft))
                    TabellaNumAs.TotalWidth = 100
                    TabellaNumAs.WriteSelectedRows(0, -1, 280, YimpegnoAss, writer.DirectContent)


                    Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                    TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDataAs.AddCell(New Phrase(liq.Dli_Data_Assunzione.Date, ft))
                    TabellaDataAs.TotalWidth = 100
                    TabellaDataAs.WriteSelectedRows(0, -1, 380, YimpegnoAss, writer.DirectContent)


                End If


                If linea Mod 3 = 0 Then
                    YimpegnoAss -= 21
                Else
                    YimpegnoAss -= 22.5
                End If

                listaLiq.Remove(liq)
            End If
        End While



        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        Dim XliquidazioneLiq As Single = 85
        Dim YliquidazioneLiq As Single = 227



        linea = 0

        While listaRAgLiq.Count > 0 And linea < contaRighe
            ragLiq = listaRAgLiq.Item(0)
            linea += 1
            If Not ragLiq Is Nothing Then


                Dim TabellaLiquidazione As PdfPTable = New PdfPTable(1)
                TabellaLiquidazione.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                TabellaLiquidazione.AddCell(New Phrase("X", ft))
                TabellaLiquidazione.TotalWidth = 10
                TabellaLiquidazione.WriteSelectedRows(0, -1, 33, YliquidazioneLiq, writer.DirectContent)


                If "" & ragLiq.Dli_NLiquidazione <> "0" And "" & ragLiq.Dli_NLiquidazione <> "" Then
                    Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                    TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                    TabellaNumLiq.TotalWidth = 100
                    TabellaNumLiq.WriteSelectedRows(0, -1, 115, YliquidazioneLiq, writer.DirectContent)

                End If

                If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, 180, YliquidazioneLiq, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, 180, YliquidazioneLiq, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, 245, YliquidazioneLiq, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, 330, YliquidazioneLiq, writer.DirectContent)

                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase(ragLiq.Dli_Costo, ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, 390, YliquidazioneLiq, writer.DirectContent)







                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 21
                Else
                    YliquidazioneLiq -= 22.5
                End If


                If "" & ragLiq.Dli_NumImpegno <> "0" Then
                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, XliquidazioneLiq, YliquidazioneLiq, writer.DirectContent)

                End If

                If ragLiq.Dli_TipoAssunzione = 1 Then


                    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDel.AddCell(New Phrase("X", ft))
                    TabellaDel.TotalWidth = 10
                    TabellaDel.WriteSelectedRows(0, -1, 270, YliquidazioneLiq, writer.DirectContent)
                End If

                If ragLiq.Dli_TipoAssunzione = 0 Then
                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("X", ft))
                    TabellaDet.TotalWidth = 10
                    TabellaDet.WriteSelectedRows(0, -1, 200, YliquidazioneLiq, writer.DirectContent)
                End If


                If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                    TabellaNumAs.TotalWidth = 100
                    TabellaNumAs.WriteSelectedRows(0, -1, 360, YliquidazioneLiq, writer.DirectContent)

                    Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                    TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                    TabellaDataAs.TotalWidth = 100
                    TabellaDataAs.WriteSelectedRows(0, -1, 490, YliquidazioneLiq, writer.DirectContent)

                End If




                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 22
                Else
                    YliquidazioneLiq -= 23
                End If

                listaRAgLiq.Remove(ragLiq)
            End If



        End While





    End Sub
    Sub addInfoLiquidazioneUltimaPagina(ByRef writer As PdfWriter, ByRef listabil As IList, ByRef listaLiq As IList)
        Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
        Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

        'Dim impegno As Single = 548

        Dim contaRighe As Integer = 3
        Dim linea As Integer = 0
        Dim XimpegnoAss As Single = 87
        Dim YimpegnoAss As Single = 542


        Dim ragbil As DllDocumentale.ItemImpegnoInfo
        Dim anno As String = ""
        Dim nImpegno As String = ""
        Dim nPrenotazione As String = ""



        Dim xChkPresenteBilDetermine As String = "35"
        Dim xBilancioBilDetermine As String = "87"
        Dim xUPBBilDetermine As String = "217"
        Dim xMissioneProgrammaBilDetermine As String = xUPBBilDetermine
        Dim xCapitoliBilDetermine As String = "347"
        Dim xCostoBilDetermine As String = "432"
        Dim xNumImpegnoBilDetermine As String = "207"
        Dim xAnnoImpegnoBilDetermine As String = "357"


        xChkPresenteBilDetermine = ConfigurationManager.AppSettings("xChkPresenteBilDetermine")
        xBilancioBilDetermine = ConfigurationManager.AppSettings("xBilancioBilDetermine")
        xUPBBilDetermine = ConfigurationManager.AppSettings("xUPBBilDetermine")
        xMissioneProgrammaBilDetermine = ConfigurationManager.AppSettings("xMissioneProgrammaBilDetermine")
        xCapitoliBilDetermine = ConfigurationManager.AppSettings("xCapitoliBilDetermine")
        xCostoBilDetermine = ConfigurationManager.AppSettings("xCostoBilDetermine")
        xNumImpegnoBilDetermine = ConfigurationManager.AppSettings("xNumImpegnoBilDetermine")
        xAnnoImpegnoBilDetermine = ConfigurationManager.AppSettings("xAnnoImpegnoBilDetermine")

        While listabil.Count > 0 And linea < contaRighe

            ragbil = listabil.Item(0)
            linea += 1
            If Not ragbil Is Nothing Then
                If "" & ragbil.Dli_NPreImpegno <> "0" Then
                    If nPrenotazione = "" Then
                        nPrenotazione = Right(ragbil.Dli_NPreImpegno, 5)
                    Else
                        nPrenotazione = nPrenotazione & " - " & Right(ragbil.Dli_NPreImpegno, 5)
                    End If

                End If
                If linea = 1 Then
                    Dim TabellaAssunto As PdfPTable = New PdfPTable(1)
                    TabellaAssunto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaAssunto.AddCell(New Phrase("X", ft))
                    TabellaAssunto.TotalWidth = 10
                    TabellaAssunto.WriteSelectedRows(0, -1, xChkPresenteBilDetermine, 563, writer.DirectContent)

                End If


                Dim TabellaNum As PdfPTable = New PdfPTable(1)
                TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                If anno = "" Then
                    nImpegno = ragbil.Dli_NumImpegno
                    anno = ragbil.DBi_Anno
                End If

                TabellaNum.AddCell(New Phrase(ragbil.Dli_Esercizio, ft))
                TabellaNum.TotalWidth = 100
                TabellaNum.WriteSelectedRows(0, -1, xBilancioBilDetermine, YimpegnoAss, writer.DirectContent)

                If Not ragbil.Dli_MissioneProgramma Is Nothing AndAlso Not ragbil.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(ragbil.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, xMissioneProgrammaBilDetermine, YimpegnoAss, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(ragbil.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, xUPBBilDetermine, YimpegnoAss, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(ragbil.Dli_Cap, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, xCapitoliBilDetermine, YimpegnoAss, writer.DirectContent)


                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase(ragbil.Dli_Costo, ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, xCostoBilDetermine, YimpegnoAss, writer.DirectContent)



                If linea Mod 3 = 0 Then
                    YimpegnoAss -= 20.0
                Else
                    YimpegnoAss -= 20.0
                End If
                listabil.Remove(ragbil)
            End If
        End While

        If "" & nPrenotazione <> "" Then

            Dim TabellaPren As PdfPTable = New PdfPTable(1)
            TabellaPren.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            TabellaPren.AddCell(New Phrase(nPrenotazione, ft))
            TabellaPren.TotalWidth = 100
            TabellaPren.WriteSelectedRows(0, -1, xNumImpegnoBilDetermine, 485, writer.DirectContent)

            Dim TabellaAnno As PdfPTable = New PdfPTable(1)
            TabellaAnno.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            TabellaAnno.AddCell(New Phrase(Left(ragbil.Dli_NPreImpegno, 4), ft))
            TabellaAnno.TotalWidth = 100
            TabellaAnno.WriteSelectedRows(0, -1, xAnnoImpegnoBilDetermine, 485, writer.DirectContent)


        End If






        Dim Liq As DllDocumentale.ItemLiquidazioneInfo

        '    Dim XliquidazioneLiq As Single = 100
        Dim YliquidazioneLiq As Single = 444


        Dim xChkPresenteLiqDetermine As String = "33"
        Dim xCostoLiqDetermine As String = "70"
        Dim xCapitoliLiqDetermine As String = "241"
        Dim xUPBLiqDetermine As String = "310"
        Dim xMissioneProgrammaLiqDetermine As String = xUPBLiqDetermine
        Dim xEsercizioLiqDetermine As String = "420"
        Dim xNContabileLiqDetermine As String = "100"
        Dim xTipoAssunzioneLiqDetermine1 As String = "241"
        Dim xTipoAssunzioneLiqDetermine0 As String = "309"
        Dim xNumAssunzioneLiqDetermine As String = "400"
        Dim xDataAssunzioneLiqDetermine As String = "510"

        linea = 0

        While listaLiq.Count > 0 And linea < contaRighe
            Liq = listaLiq.Item(0)
            linea += 1
            If Not Liq Is Nothing Then
                If linea = 1 Then
                    Dim TabellaLiquidazione As PdfPTable = New PdfPTable(1)
                    TabellaLiquidazione.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaLiquidazione.AddCell(New Phrase("X", ft))
                    TabellaLiquidazione.TotalWidth = 10
                    TabellaLiquidazione.WriteSelectedRows(0, -1, xChkPresenteLiqDetermine, YliquidazioneLiq + 23, writer.DirectContent)



                End If


                Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCosto.AddCell(New Phrase(Liq.Dli_Costo, ft))
                TabellaCosto.TotalWidth = 100
                TabellaCosto.WriteSelectedRows(0, -1, xCostoLiqDetermine, YliquidazioneLiq, writer.DirectContent)

                If Not Liq.Dli_MissioneProgramma Is Nothing AndAlso Not Liq.Dli_MissioneProgramma.Trim() = String.Empty Then
                    Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                    TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaMissioneProgramma.AddCell(New Phrase(Liq.Dli_MissioneProgramma, ft))
                    TabellaMissioneProgramma.TotalWidth = 100
                    TabellaMissioneProgramma.WriteSelectedRows(0, -1, xMissioneProgrammaLiqDetermine, YliquidazioneLiq, writer.DirectContent)
                Else
                    Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                    TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    TabellaUPB.AddCell(New Phrase(Liq.Dli_UPB, ft))
                    TabellaUPB.TotalWidth = 100
                    TabellaUPB.WriteSelectedRows(0, -1, xUPBLiqDetermine, YliquidazioneLiq, writer.DirectContent)
                End If

                Dim TabellaCap As PdfPTable = New PdfPTable(1)
                TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaCap.AddCell(New Phrase(Liq.Dli_UPB, ft))
                TabellaCap.TotalWidth = 100
                TabellaCap.WriteSelectedRows(0, -1, xUPBLiqDetermine, YliquidazioneLiq, writer.DirectContent)

                Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                TabellaEsercizio.AddCell(New Phrase(Liq.Dli_Esercizio, ft))
                TabellaEsercizio.TotalWidth = 100
                TabellaEsercizio.WriteSelectedRows(0, -1, xEsercizioLiqDetermine, YliquidazioneLiq, writer.DirectContent)








                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 19.0
                Else
                    YliquidazioneLiq -= 19.0
                End If


                If "" & Liq.Dli_NumImpegno <> "0" Then


                    Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                    TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumImpcon.AddCell(New Phrase(Liq.Dli_NumImpegno, ft))
                    TabellaNumImpcon.TotalWidth = 100
                    TabellaNumImpcon.WriteSelectedRows(0, -1, xNContabileLiqDetermine, YliquidazioneLiq, writer.DirectContent)
                End If





                Dim TabellaDel As PdfPTable = New PdfPTable(1)
                TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                If Liq.Dli_TipoAssunzione = 1 Then
                    TabellaDel.AddCell(New Phrase("X", ft))
                    TabellaDel.TotalWidth = 10
                    TabellaDel.WriteSelectedRows(0, -1, xTipoAssunzioneLiqDetermine1, YliquidazioneLiq, writer.DirectContent)
                End If

                If Liq.Dli_TipoAssunzione = 0 Then


                    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaDet.AddCell(New Phrase("X", ft))
                    TabellaDet.TotalWidth = 10
                    TabellaDet.WriteSelectedRows(0, -1, xTipoAssunzioneLiqDetermine0, YliquidazioneLiq, writer.DirectContent)

                End If

                If "" & Liq.Dli_Num_assunzione <> "0" Then
                    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                    TabellaNumAs.AddCell(New Phrase(Liq.Dli_Num_assunzione, ft))
                    TabellaNumAs.TotalWidth = 100
                    TabellaNumAs.WriteSelectedRows(0, -1, 400, YliquidazioneLiq, writer.DirectContent)

                    If Liq.Dli_Data_Assunzione.Year > 1 Then
                        Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                        TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        TabellaDataAs.AddCell(New Phrase(Liq.Dli_Data_Assunzione.Date, ft))
                        TabellaDataAs.TotalWidth = 100
                        TabellaDataAs.WriteSelectedRows(0, -1, xDataAssunzioneLiqDetermine, YliquidazioneLiq, writer.DirectContent)
                    End If

                End If

                If linea Mod 3 = 0 Then
                    YliquidazioneLiq -= 20.0
                Else
                    YliquidazioneLiq -= 20.0
                End If

                listaLiq.Remove(Liq)
            End If



        End While





    End Sub
    Sub importAppendiceDeterminaDelibera_Impegno(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal listaRagAssunz As IList, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettDirReg As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal valoreCUP As String = "")
        Dim ragAss As DllDocumentale.ItemImpegnoInfo

        While listaRagAssunz.Count > 0

            Dim result As Boolean = False
            Dim pdfCount As Integer = 0 'total input pdf file count
            Dim f As Integer = 0 'pointer to current input pdf file
            Dim fileName As String = String.Empty 'current input pdf filename
            Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
            Dim pageCount As Integer = 0 'cureent input pdf page count



            'Declare a variable to hold the imported pages
            Dim page As PdfImportedPage = Nothing
            Dim rotation As Integer = 0
            'Declare a font to used for the bookmarks
            Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
            12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)

            Try
                pdfCount = pdfFilesAppendice.Length
                If pdfCount > 0 Then
                    'Open the 1st pad using PdfReader object
                    fileName = pdfFilesAppendice(f)
                    reader = New iTextSharp.text.pdf.PdfReader(fileName)
                    'Get page count
                    pageCount = reader.NumberOfPages


                    

                    'Now loop thru the input pdfs
                    While f < pdfCount
                        'Declare a page counter variable
                        Dim i As Integer = 0
                        'Loop thru the current input pdf's pages starting at page 1
                        While i < pageCount


                            i += 1


                            Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                            Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)


                            'Get the input page size
                            pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                            'Create a new page on the output document
                            pdfDoc.NewPage()
                            'If it is the 1st page, we add bookmarks to the page
                            If i = 1 Then
                                'First create a paragraph using the filename as the heading
                                '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                'Then create a chapter from the above paragraph
                                para.IndentationLeft = -50000000
                                Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                                'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                'TabellaNumero.AddCell(para)
                                'TabellaNumero.TotalWidth = 1
                                'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                                'Finally add the chapter to the document

                                chpter.NumberDepth = -1
                                pdfDoc.Add(chpter)

                            End If
                            'Now we get the imported page
                            page = writer.GetImportedPage(reader, i)
                            Dim contaRighe As Integer = 25
                            Dim linea As Integer = 0



                            'Stampo intestazioni ufficio firma


                            Dim xUfficio As Single = 355
                            Dim yUfficio As Single = 750

                            Dim xNumero As Single = 365
                            Dim yNumero As Single = 682

                            Dim xData As Single = 365
                            Dim yData As Single = 682

                            Dim xCup As Single = 355
                            Dim yCup As Single = 650

                            Dim xDirigenteFirma As Single = 290
                            Dim yDirigenteFirma As Single = 68
                            Dim xDataFirma As Single = 455
                            Dim yDataFirma As Single = 68

                            Dim xOggetto As Single = 290
                            Dim yOggetto As Single = 68
                            Dim xIntegrale As Single = 290
                            Dim yIntegrale As Single = 68
                            Dim xEstratto As Single = 455
                            Dim yEstratto As Single = 68


                            Dim XimpegnoAss As Single = 36 'da introdurre
                            Dim YimpegnoAss As Single = 460 'da introdurre
                            Dim XEsercizio As Single = 103 'da introdurre
                            Dim XUPB As Single = 145 'da introdurre
                            Dim XMissioneProgramma As Single = XUPB
                            Dim XCapitolo As Single = 202 'da introdurre
                            Dim XCosto As Single = 255 'da introdurre
                            Dim XTipo As Single = 352 'da introdurre
                            Dim XNPren As Single = 390 'da introdurre
                            Dim XAnno As Single = 462 'da introdurre
                            Dim XPerente As Single = 500 'da introdurre
                            Dim XCoGes As Single = 500 'da introdurre


                            Dim strConfig As String = tipo

                            Try
                                xUfficio = ConfigurationManager.AppSettings("xUfficioAppendice" & strConfig)
                                yUfficio = ConfigurationManager.AppSettings("yUfficioAppendice" & strConfig)
                                xNumero = ConfigurationManager.AppSettings("xNumeroAppendice" & strConfig)
                                yNumero = ConfigurationManager.AppSettings("yNumeroAppendice" & strConfig)
                                xData = ConfigurationManager.AppSettings("xDataAppendice" & strConfig)
                                yData = ConfigurationManager.AppSettings("yDataAppendice" & strConfig)


                                xCup = IIf(ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "", xCup)
                                yCup = IIf(ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "", yCup)

                                '  xUfficio = xUfficio - 100

                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteFirma" & strConfig)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteFirma" & strConfig)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirma" & strConfig)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirma" & strConfig)

                                'Info Impegno

                                XimpegnoAss = ConfigurationManager.AppSettings("XimpegnoAss")
                                'YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAss")                                
                                YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAppendice" & strConfig)

                                XEsercizio = ConfigurationManager.AppSettings("XEsercizio")
                                XUPB = ConfigurationManager.AppSettings("XUPB")
                                XMissioneProgramma = ConfigurationManager.AppSettings("XMissioneProgramma")
                                XCapitolo = ConfigurationManager.AppSettings("XCapitolo")
                                XCosto = ConfigurationManager.AppSettings("XCosto")
                                XTipo = ConfigurationManager.AppSettings("XTipo")
                                XNPren = ConfigurationManager.AppSettings("XNPren")
                                XAnno = ConfigurationManager.AppSettings("XAnno")
                                XPerente = ConfigurationManager.AppSettings("XPerente")
                                XCoGes = ConfigurationManager.AppSettings("XCoGes")





                            Catch ex As Exception
                                ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                            End Try

                            Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                            TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                            TabellaUfficio.AddCell(New Phrase("" & Ufficio, ft))
                            TabellaUfficio.AddCell(New Phrase("" & codUfficio, ft))
                            TabellaUfficio.TotalWidth = 200
                            TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                            If lstr_oggetto <> "" Then
                                xOggetto = ConfigurationManager.AppSettings("xOggettoAppendice" & strConfig)
                                yOggetto = ConfigurationManager.AppSettings("yOggettoAppendice" & strConfig)
                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)

                            End If



                            If numDoc <> "" Then
                                Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaNumero.AddCell(numDoc)
                                TabellaNumero.TotalWidth = 225
                                TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)

                                If strConfig <> "Delibere" Then
                                    Dim tabellaData As PdfPTable = New PdfPTable(1)
                                    tabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                    tabellaData.AddCell(datadoc)
                                    tabellaData.TotalWidth = 225
                                    tabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                                ElseIf strConfig = "Delibere" Then

                                    Dim bfTimesa As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                                    Dim ftDip As iTextSharp.text.Font = New Font(bfTimesa, 9, iTextSharp.text.Font.BOLD)
                                    Dim tabellaDataSeduta As PdfPTable = New PdfPTable(1)
                                    tabellaDataSeduta.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    tabellaDataSeduta.AddCell(New Phrase(datadoc, ftDip))
                                    tabellaDataSeduta.TotalWidth = 170
                                    tabellaDataSeduta.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                                End If
                            End If

                            'CUP
                            Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                            TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaCUP.AddCell(New Phrase("" & valoreCUP, ft))
                            TabellaCUP.TotalWidth = 200
                            TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)





                            While listaRagAssunz.Count > 0 And linea < contaRighe
                                ragAss = listaRagAssunz.Item(0)
                                linea += 1
                                If Not ragAss Is Nothing Then
                                    If flagInserisciNImpegni Then
                                        If "" & ragAss.Dli_NumImpegno <> "0" Then
                                            Dim TabellaNum As PdfPTable = New PdfPTable(1)
                                            TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNum.AddCell(New Phrase(ragAss.Dli_NumImpegno, ft))
                                            TabellaNum.TotalWidth = 100
                                            TabellaNum.WriteSelectedRows(0, -1, XimpegnoAss, YimpegnoAss, writer.DirectContent)

                                        End If
                                    End If

                                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaEsercizio.AddCell(New Phrase(ragAss.Dli_Esercizio, ft))
                                    TabellaEsercizio.TotalWidth = 100
                                    TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizio, YimpegnoAss, writer.DirectContent)

                                    If Not ragAss.Dli_MissioneProgramma Is Nothing AndAlso Not ragAss.Dli_MissioneProgramma.Trim() = String.Empty Then
                                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaMissioneProgramma.AddCell(New Phrase(ragAss.Dli_MissioneProgramma, ft))
                                        TabellaMissioneProgramma.TotalWidth = 100
                                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgramma, YimpegnoAss, writer.DirectContent)
                                    Else
                                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaUPB.AddCell(New Phrase(ragAss.Dli_UPB, ft))
                                        TabellaUPB.TotalWidth = 100
                                        TabellaUPB.WriteSelectedRows(0, -1, XUPB, YimpegnoAss, writer.DirectContent)
                                    End If

                                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaCap.AddCell(New Phrase(ragAss.Dli_Cap, ft))
                                    TabellaCap.TotalWidth = 100
                                    TabellaCap.WriteSelectedRows(0, -1, XCapitolo, YimpegnoAss, writer.DirectContent)




                                    'Dim TabellaCoGes As PdfPTable = New PdfPTable(1)
                                    'TabellaCoGes.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    'TabellaCoGes.AddCell(New Phrase(ragAss.Codice_Obbiettivo_Gestionale, ft))
                                    'TabellaCoGes.TotalWidth = 100
                                    'TabellaCoGes.WriteSelectedRows(0, -1, XCoGes, YimpegnoAss, writer.DirectContent)




                                    If ragAss.Dli_Costo > 0 Then

                                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragAss.Dli_Costo), ft))
                                        TabellaCosto.TotalWidth = 100
                                        TabellaCosto.WriteSelectedRows(0, -1, XCosto, YimpegnoAss, writer.DirectContent)
                                    End If

                                    Dim TabellaTipo As PdfPTable = New PdfPTable(1)
                                    TabellaTipo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    'DA Verificare
                                    'If ragAss.Di_PreImpDaPrenotazione Then
                                    '    If ragAss.Di_TipoAssunzione = 1 Then
                                    '        TabellaTipo.AddCell(New Phrase("DEL", ft))
                                    '    ElseIf ragAss.Di_TipoAssunzione = 0 Then
                                    '        TabellaTipo.AddCell(New Phrase("DET", ft))
                                    '    End If
                                    'Else
                                    '    TabellaTipo.AddCell(New Phrase("DET", ft))
                                    'End If

                                    ' L'impegno può essere preso a partire da un PREIMP fatto con un atto o creato generando automaticamente un preimp. 
                                    ' (ragAss.Di_PreImpDaPrenotazione: 0->preimp generato automaticamente; 1->c'è un preimpegno fatto con un atto)
                                    ' Verifico se si tratta di un preimp preso con un atto specifico (tipoAssunz: 0->DET o 1->DEL) opp no.
                                    ' Se il PREIMP è stato assunto appositamente con un atto lo trovo nella tabella Documento_preimpegno,
                                    ' se è stato invece generato automaticam, non lo trovo, quindi sono certa che il tuo tipo assunzione del preimp sia DET
                                    Dim objDocLib As New DllDocumentale.svrDocumenti(HttpContext.Current.Session("oOperatore"))
                                    If ragAss.Di_PreImpDaPrenotazione Then
                                        Dim listaPreImpegni As IList = objDocLib.FO_Get_DatiPreImpegni(, , ragAss.Dli_NPreImpegno)
                                        If Not listaPreImpegni Is Nothing AndAlso listaPreImpegni.Count > 0 Then
                                            Dim preimp As DllDocumentale.ItemImpegnoInfo = listaPreImpegni.Item(0)
                                            If Not preimp Is Nothing Then
                                                If preimp.Di_TipoAssunzione = 1 Then
                                                    TabellaTipo.AddCell(New Phrase("DEL", ft))
                                                ElseIf preimp.Di_TipoAssunzione = 0 Then
                                                    TabellaTipo.AddCell(New Phrase("DET", ft))
                                                End If
                                            End If
                                        End If
                                    Else
                                        If strConfig <> "Delibere" Then
                                            TabellaTipo.AddCell(New Phrase("DET", ft))
                                        Else 
                                            TabellaTipo.AddCell(New Phrase("DEL", ft))
                                        End If
                                    End If
                                    TabellaTipo.TotalWidth = 100
                                    TabellaTipo.WriteSelectedRows(0, -1, XTipo, YimpegnoAss, writer.DirectContent)


                                    Dim TabellaAnno As PdfPTable = New PdfPTable(1)
                                    TabellaAnno.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaAnno.AddCell(New Phrase(ragAss.DBi_Anno, ft))
                                    TabellaAnno.TotalWidth = 100
                                    TabellaAnno.WriteSelectedRows(0, -1, XAnno, YimpegnoAss, writer.DirectContent)

                                    If flagInserisciNImpegni Or (ragAss.Di_PreImpDaPrenotazione) Then
                                        Dim TabellaPrenot As PdfPTable = New PdfPTable(1)
                                        TabellaPrenot.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaPrenot.AddCell(New Phrase(ragAss.Dli_NPreImpegno, ft))
                                        TabellaPrenot.TotalWidth = 100
                                        TabellaPrenot.WriteSelectedRows(0, -1, XNPren, YimpegnoAss, writer.DirectContent)
                                    End If

                                    Dim TabellaPeren As PdfPTable = New PdfPTable(1)
                                    TabellaPeren.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaPeren.AddCell(New Phrase(ragAss.NDocPrecedente, ft))
                                    TabellaPeren.TotalWidth = 100
                                    TabellaPeren.WriteSelectedRows(0, -1, XPerente, YimpegnoAss, writer.DirectContent)




                                    YimpegnoAss -= 16

                                    listaRagAssunz.Remove(ragAss)
                                End If
                            End While










                            'Read the imported page's rotation
                            rotation = reader.GetPageRotation(i)
                            'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                            If rotation = 90 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            ElseIf rotation = 270 Then
                                cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                            End If


                        End While
                        'Increment f and read the next input pdf file
                        f += 1
                        If f < pdfCount Then
                            fileName = pdfFilesAppendice(f)
                            reader = New iTextSharp.text.pdf.PdfReader(fileName)
                            pageCount = reader.NumberOfPages
                        End If
                    End While
                    'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                    result = True
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End While
    End Sub
    Sub importAppendiceDeterminaDelibera_PreImpegno(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal listaRagPreimpegni As IList, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettDirReg As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal valoreCUP As String = "")
        Dim ragPreimpegno As DllDocumentale.ItemImpegnoInfo

        While listaRagPreimpegni.Count > 0

            Dim result As Boolean = False
            Dim pdfCount As Integer = 0 'total input pdf file count
            Dim f As Integer = 0 'pointer to current input pdf file
            Dim fileName As String = String.Empty 'current input pdf filename
            Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
            Dim pageCount As Integer = 0 'cureent input pdf page count



            'Declare a variable to hold the imported pages
            Dim page As PdfImportedPage = Nothing
            Dim rotation As Integer = 0
            'Declare a font to used for the bookmarks
            Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
            12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)
            Try
                pdfCount = pdfFilesAppendice.Length
                If pdfCount > 0 Then
                    'Open the 1st pad using PdfReader object
                    fileName = pdfFilesAppendice(f)
                    reader = New iTextSharp.text.pdf.PdfReader(fileName)
                    'Get page count
                    pageCount = reader.NumberOfPages

                    Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                    Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)
                    'Now loop thru the input pdfs
                    While f < pdfCount
                        'Declare a page counter variable
                        Dim i As Integer = 0
                        'Loop thru the current input pdf's pages starting at page 1
                        While i < pageCount



                            i += 1
                            'Get the input page size
                            pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                            'Create a new page on the output document
                            pdfDoc.NewPage()
                            'If it is the 1st page, we add bookmarks to the page
                            If i = 1 Then
                                'First create a paragraph using the filename as the heading
                                '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                'Then create a chapter from the above paragraph
                                para.IndentationLeft = -50000000
                                Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                                'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                'TabellaNumero.AddCell(para)
                                'TabellaNumero.TotalWidth = 1
                                'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                                'Finally add the chapter to the document

                                chpter.NumberDepth = -1
                                pdfDoc.Add(chpter)

                            End If
                            'Now we get the imported page
                            page = writer.GetImportedPage(reader, i)
                            Dim contaRighe As Integer = 25
                            Dim linea As Integer = 0



                            'Stampo intestazioni ufficio firma


                            Dim xUfficio As Single = 355
                            Dim yUfficio As Single = 750

                            Dim xNumero As Single = 365
                            Dim yNumero As Single = 682

                            Dim xData As Single = 365
                            Dim yData As Single = 682

                            Dim xCup As Single = 355
                            Dim yCup As Single = 650

                            Dim xDirigenteFirma As Single = 290
                            Dim yDirigenteFirma As Single = 68
                            Dim xDataFirma As Single = 455
                            Dim yDataFirma As Single = 68

                            Dim xOggetto As Single = 290
                            Dim yOggetto As Single = 68
                            Dim xIntegrale As Single = 290
                            Dim yIntegrale As Single = 68
                            Dim xEstratto As Single = 455
                            Dim yEstratto As Single = 68


                            Dim Xpreimpegno As Single = 36 'da introdurre
                            Dim YpreimpegnoAppendice As Single = 460 'da introdurre
                            Dim XpreimpegnoEsercizio As Single = 103 'da introdurre
                            Dim XUPBpreimpegno As Single = 145 'da introdurre
                            Dim XMissioneProgrammaPreimpegno As Single = XUPBpreimpegno
                            Dim XCapitoloPreimpegno As Single = 202 'da introdurre
                            Dim XCostoPreimpegno As Single = 255 'da introdurre

                            Dim strConfig As String = tipo

                            Try
                                xUfficio = ConfigurationManager.AppSettings("xUfficioAppendice" & strConfig)
                                yUfficio = ConfigurationManager.AppSettings("yUfficioAppendice" & strConfig)
                                xNumero = ConfigurationManager.AppSettings("xNumeroAppendice" & strConfig)
                                yNumero = ConfigurationManager.AppSettings("yNumeroAppendice" & strConfig)
                                xData = ConfigurationManager.AppSettings("xDataAppendice" & strConfig)
                                yData = ConfigurationManager.AppSettings("yDataAppendice" & strConfig)


                                xCup = IIf(ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "", xCup)
                                yCup = IIf(ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "", yCup)
                                '  xUfficio = xUfficio - 100

                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteFirma" & strConfig)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteFirma" & strConfig)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirma" & strConfig)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirma" & strConfig)

                                'Info Impegno

                                Xpreimpegno = ConfigurationManager.AppSettings("Xpreimpegno")
                                'Ypreimpegno = ConfigurationManager.AppSettings("Ypreimpegno")                                
                                YpreimpegnoAppendice = ConfigurationManager.AppSettings("YpreimpegnoAppendice" & strConfig)

                                XpreimpegnoEsercizio = ConfigurationManager.AppSettings("XpreimpegnoEsercizio")
                                XUPBpreimpegno = ConfigurationManager.AppSettings("XUPBpreimpegno")
                                XMissioneProgrammaPreimpegno = ConfigurationManager.AppSettings("XMissioneProgrammaPreimpegno")
                                XCapitoloPreimpegno = ConfigurationManager.AppSettings("XCapitoloPreimpegno")
                                XCostoPreimpegno = ConfigurationManager.AppSettings("XCostoPreimpegno")


                            Catch ex As Exception
                                ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                            End Try

                            Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                            TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                            TabellaUfficio.AddCell(New Phrase("" & Ufficio, ft))
                            TabellaUfficio.AddCell(New Phrase("" & codUfficio, ft))
                            TabellaUfficio.TotalWidth = 200
                            TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                            If lstr_oggetto <> "" Then
                                xOggetto = ConfigurationManager.AppSettings("xOggettoAppendice" & strConfig)
                                yOggetto = ConfigurationManager.AppSettings("yOggettoAppendice" & strConfig)
                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)

                            End If



                            If numDoc <> "" Then
                                Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaNumero.AddCell(numDoc)
                                TabellaNumero.TotalWidth = 225
                                TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)

                                If strConfig <> "Delibere" Then
                                    Dim TabellaData As PdfPTable = New PdfPTable(1)
                                    TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaData.AddCell(datadoc)
                                    TabellaData.TotalWidth = 225
                                    TabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                                ElseIf strConfig = "Delibere" Then
                                      Dim bfTimes2 As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                                         Dim ftDip As iTextSharp.text.Font = New Font(bfTimes, 9, Font.BOLD)
                                    'Dim ftDip As iTextSharp.text.Font = New Font(BaseFont.TIMES_ROMAN, 9, Font.BOLD)
                                    Dim TabellaDataSeduta As PdfPTable = New PdfPTable(1)
                                    TabellaDataSeduta.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaDataSeduta.AddCell(New Phrase(datadoc, ftDip))
                                    TabellaDataSeduta.TotalWidth = 170
                                    TabellaDataSeduta.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)

                                End If
                            End If

                            'CUP
                            Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                            TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaCUP.AddCell(New Phrase("" & valoreCUP, ft))
                            TabellaCUP.TotalWidth = 200
                            TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)

                            While listaRagPreimpegni.Count > 0 And linea < contaRighe
                                ragPreimpegno = listaRagPreimpegni.Item(0)
                                linea += 1
                                If Not ragPreimpegno Is Nothing Then
                                    Dim TabellaNum As PdfPTable = New PdfPTable(1)
                                    TabellaNum.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaNum.AddCell(New Phrase(ragPreimpegno.Dli_NPreImpegno, ft))
                                    TabellaNum.TotalWidth = 100
                                    TabellaNum.WriteSelectedRows(0, -1, Xpreimpegno, YpreimpegnoAppendice, writer.DirectContent)

                                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaEsercizio.AddCell(New Phrase(ragPreimpegno.Dli_Esercizio, ft))
                                    TabellaEsercizio.TotalWidth = 100
                                    TabellaEsercizio.WriteSelectedRows(0, -1, XpreimpegnoEsercizio, YpreimpegnoAppendice, writer.DirectContent)

                                    If Not ragPreimpegno.Dli_MissioneProgramma Is Nothing AndAlso Not ragPreimpegno.Dli_MissioneProgramma.Trim() = String.Empty Then
                                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaMissioneProgramma.AddCell(New Phrase(ragPreimpegno.Dli_MissioneProgramma, ft))
                                        TabellaMissioneProgramma.TotalWidth = 100
                                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaPreimpegno, YpreimpegnoAppendice, writer.DirectContent)
                                    Else
                                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaUPB.AddCell(New Phrase(ragPreimpegno.Dli_UPB, ft))
                                        TabellaUPB.TotalWidth = 100
                                        TabellaUPB.WriteSelectedRows(0, -1, XUPBpreimpegno, YpreimpegnoAppendice, writer.DirectContent)
                                    End If

                                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaCap.AddCell(New Phrase(ragPreimpegno.Dli_Cap, ft))
                                    TabellaCap.TotalWidth = 100
                                    TabellaCap.WriteSelectedRows(0, -1, XCapitoloPreimpegno, YpreimpegnoAppendice, writer.DirectContent)


                                    If ragPreimpegno.Dli_Costo > 0 Then

                                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragPreimpegno.Dli_Costo), ft))
                                        TabellaCosto.TotalWidth = 100
                                        TabellaCosto.WriteSelectedRows(0, -1, XCostoPreimpegno, YpreimpegnoAppendice, writer.DirectContent)
                                    End If


                                    YpreimpegnoAppendice -= 16

                                    listaRagPreimpegni.Remove(ragPreimpegno)
                                End If
                            End While
                            'Read the imported page's rotation
                            rotation = reader.GetPageRotation(i)
                            'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                            If rotation = 90 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            ElseIf rotation = 270 Then
                                cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                            End If


                        End While
                        'Increment f and read the next input pdf file
                        f += 1
                        If f < pdfCount Then
                            fileName = pdfFilesAppendice(f)
                            reader = New iTextSharp.text.pdf.PdfReader(fileName)
                            pageCount = reader.NumberOfPages
                        End If
                    End While
                    'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                    result = True
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End While
    End Sub
    Sub importAppendiceDetermina_Liquidazione(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal listaRagLiq As IList, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettDirReg As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal valoreCUP As String = "")
        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        While listaRagLiq.Count > 0

            Dim result As Boolean = False
            Dim pdfCount As Integer = 0 'total input pdf file count
            Dim f As Integer = 0 'pointer to current input pdf file
            Dim fileName As String = String.Empty 'current input pdf filename
            Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
            Dim pageCount As Integer = 0 'cureent input pdf page count



            'Declare a variable to hold the imported pages
            Dim page As PdfImportedPage = Nothing
            Dim rotation As Integer = 0
            'Declare a font to used for the bookmarks
            Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
            12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)
            Try
                pdfCount = pdfFilesAppendice.Length
                If pdfCount > 0 Then
                    'Open the 1st pad using PdfReader object
                    fileName = pdfFilesAppendice(f)
                    reader = New iTextSharp.text.pdf.PdfReader(fileName)
                    'Get page count
                    pageCount = reader.NumberOfPages

                    Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                    Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

                    'Now loop thru the input pdfs
                    While f < pdfCount
                        'Declare a page counter variable
                        Dim i As Integer = 0
                        'Loop thru the current input pdf's pages starting at page 1
                        While i < pageCount



                            i += 1
                            'Get the input page size
                            pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                            'Create a new page on the output document
                            pdfDoc.NewPage()
                            'If it is the 1st page, we add bookmarks to the page
                            If i = 1 Then
                                'First create a paragraph using the filename as the heading
                                '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                'Then create a chapter from the above paragraph
                                para.IndentationLeft = -50000000
                                Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                                'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                'TabellaNumero.AddCell(para)
                                'TabellaNumero.TotalWidth = 1
                                'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                                'Finally add the chapter to the document

                                chpter.NumberDepth = -1
                                pdfDoc.Add(chpter)

                            End If
                            'Now we get the imported page
                            page = writer.GetImportedPage(reader, i)
                            Dim contaRighe As Integer = 25
                            Dim linea As Integer = 0



                            'Stampo intestazioni ufficio firma


                            Dim xUfficio As Single = 355
                            Dim yUfficio As Single = 750

                            Dim xNumero As Single = 365
                            Dim yNumero As Single = 682


                            Dim xData As Single = 365
                            Dim yData As Single = 682

                            Dim xCup As Single = 355
                            Dim yCup As Single = 650

                            Dim xDirigenteFirma As Single = 290
                            Dim yDirigenteFirma As Single = 68
                            Dim xDataFirma As Single = 455
                            Dim yDataFirma As Single = 68

                            Dim xOggetto As Single = 290
                            Dim yOggetto As Single = 68
                            Dim xIntegrale As Single = 290
                            Dim yIntegrale As Single = 68
                            Dim xEstratto As Single = 455
                            Dim yEstratto As Single = 68


                            Dim XimpegnoAss As Single = 36 'da introdurre
                            Dim YimpegnoAss As Single = 460 'da introdurre
                            'da introdurre


                            Dim XliquidazioneLiq As Single = XimpegnoAss
                            Dim YliquidazioneLiq As Single = YimpegnoAss
                            Dim XEsercizioLiq As Single = 103 'da introdurre
                            Dim XUPBLiq As Single = 149 'da introdurre
                            Dim XMissioneProgrammaLiq As Single = XUPBLiq
                            Dim XCapitoloLiq As Single = 204 'da introdurre
                            Dim XCostoLiq As Single = 256 'da introdurre
                            Dim XNImpegnoLiq As Single = 352 'da introdurre
                            Dim XTipoLiq As Single = 425 'da introdurre
                            Dim XAnnoLiq As Single = 460 'da introdurre
                            Dim XNumAttoLiq As Single = 460 'da introdurre
                            Dim XDataAttoLiq As Single = 510 'da introdurre

                            Dim strConfig As String = tipo

                            Try
                                xUfficio = ConfigurationManager.AppSettings("xUfficioAppendice" & strConfig)
                                yUfficio = ConfigurationManager.AppSettings("yUfficioAppendice" & strConfig)
                                xNumero = ConfigurationManager.AppSettings("xNumeroAppendice" & strConfig)
                                yNumero = ConfigurationManager.AppSettings("yNumeroAppendice" & strConfig)
                                xData = ConfigurationManager.AppSettings("xDataAppendice" & strConfig)
                                yData = ConfigurationManager.AppSettings("yDataAppendice" & strConfig)

                                xCup = IIf(ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "", xCup)
                                yCup = IIf(ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "", yCup)

                                '  xUfficio = xUfficio - 100

                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteFirma" & strConfig)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteFirma" & strConfig)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirma" & strConfig)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirma" & strConfig)




                                XliquidazioneLiq = ConfigurationManager.AppSettings("XliquidazioneLiq")
                                YliquidazioneLiq = ConfigurationManager.AppSettings("YliquidazioneLiq")
                                XimpegnoAss = ConfigurationManager.AppSettings("XimpegnoAss")
                                YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAss")

                                XliquidazioneLiq = XimpegnoAss
                                YliquidazioneLiq = YimpegnoAss
                                YliquidazioneLiq = ConfigurationManager.AppSettings("YliquidazioneLiqAppendice" & strConfig)

                                XEsercizioLiq = ConfigurationManager.AppSettings("XEsercizioLiq")
                                XUPBLiq = ConfigurationManager.AppSettings("XUPBLiq")
                                XMissioneProgrammaLiq = ConfigurationManager.AppSettings("XMissioneProgrammaLiq")
                                XCapitoloLiq = ConfigurationManager.AppSettings("XCapitoloLiq")
                                XCostoLiq = ConfigurationManager.AppSettings("XCostoLiq")
                                XNImpegnoLiq = ConfigurationManager.AppSettings("XNImpegnoLiq")
                                XTipoLiq = ConfigurationManager.AppSettings("XTipoLiq")
                                XAnnoLiq = ConfigurationManager.AppSettings("XAnnoLiq")
                                XNumAttoLiq = ConfigurationManager.AppSettings("XNumAttoLiq")
                                XDataAttoLiq = ConfigurationManager.AppSettings("XDataAttoLiq")


                            Catch ex As Exception
                                ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                            End Try

                            Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                            TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                            TabellaUfficio.AddCell(New Phrase("" & Ufficio, ft))
                            TabellaUfficio.AddCell(New Phrase("" & codUfficio, ft))
                            TabellaUfficio.TotalWidth = 200
                            TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                            If lstr_oggetto <> "" Then
                                xOggetto = ConfigurationManager.AppSettings("xOggettoAppendice" & strConfig)
                                yOggetto = ConfigurationManager.AppSettings("yOggettoAppendice" & strConfig)
                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)

                            End If



                            If numDoc <> "" Then
                                Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaNumero.AddCell(numDoc)
                                TabellaNumero.TotalWidth = 225
                                TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)
                                Dim TabellaData As PdfPTable = New PdfPTable(1)
                                TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaData.AddCell(datadoc)
                                TabellaData.TotalWidth = 225
                                TabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                            End If



                            'CUP
                            Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                            TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaCUP.AddCell(New Phrase("" & valoreCUP, ft))
                            TabellaCUP.TotalWidth = 200
                            TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)




                            While listaRagLiq.Count > 0 And linea < contaRighe
                                ragLiq = listaRagLiq.Item(0)
                                linea += 1
                                If Not ragLiq Is Nothing Then


                                    If flagInserisciNImpegni Then
                                        If "" & ragLiq.Dli_NLiquidazione <> "0" Then
                                            Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                                            TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                                            TabellaNumLiq.TotalWidth = 100
                                            TabellaNumLiq.WriteSelectedRows(0, -1, XliquidazioneLiq, YliquidazioneLiq, writer.DirectContent)

                                        End If

                                    End If

                                    If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                                        TabellaMissioneProgramma.TotalWidth = 100
                                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaLiq, YliquidazioneLiq, writer.DirectContent)
                                    Else
                                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                                        TabellaUPB.TotalWidth = 100
                                        TabellaUPB.WriteSelectedRows(0, -1, XUPBLiq, YliquidazioneLiq, writer.DirectContent)
                                    End If

                                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                                    TabellaCap.TotalWidth = 100
                                    TabellaCap.WriteSelectedRows(0, -1, XCapitoloLiq, YliquidazioneLiq, writer.DirectContent)

                                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                                    TabellaEsercizio.TotalWidth = 100
                                    TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioLiq, YliquidazioneLiq, writer.DirectContent)

                                    If ragLiq.Dli_Costo > 0 Then
                                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragLiq.Dli_Costo), ft))
                                        TabellaCosto.TotalWidth = 100
                                        TabellaCosto.WriteSelectedRows(0, -1, XCostoLiq, YliquidazioneLiq, writer.DirectContent)
                                    End If



                                    If "" & ragLiq.Dli_NumImpegno <> "0" Then
                                        Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                                        TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                                        TabellaNumImpcon.TotalWidth = 100
                                        TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoLiq, YliquidazioneLiq, writer.DirectContent)


                                        If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                                            Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                                            TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                                            TabellaNumAs.TotalWidth = 100
                                            TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoLiq, YliquidazioneLiq, writer.DirectContent)


                                            Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                                            TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                                            TabellaDataAs.TotalWidth = 100
                                            TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoLiq, YliquidazioneLiq, writer.DirectContent)


                                        End If


                                    End If


                                    If ragLiq.Dli_TipoAssunzione = 1 Then

                                        Dim TabellaDel As PdfPTable = New PdfPTable(1)
                                        TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaDel.AddCell(New Phrase("DEL", ft))
                                        TabellaDel.TotalWidth = 100
                                        TabellaDel.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiq, writer.DirectContent)
                                    End If

                                    If ragLiq.Dli_TipoAssunzione = 0 Then
                                        Dim TabellaDet As PdfPTable = New PdfPTable(1)
                                        TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaDet.AddCell(New Phrase("DET", ft))
                                        TabellaDet.TotalWidth = 100
                                        TabellaDet.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiq, writer.DirectContent)
                                    End If








                                    YliquidazioneLiq -= 16

                                    listaRagLiq.Remove(ragLiq)
                                End If
                            End While










                            'Read the imported page's rotation
                            rotation = reader.GetPageRotation(i)
                            'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                            If rotation = 90 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            ElseIf rotation = 270 Then
                                cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                            End If


                        End While
                        'Increment f and read the next input pdf file
                        f += 1
                        If f < pdfCount Then
                            fileName = pdfFilesAppendice(f)
                            reader = New iTextSharp.text.pdf.PdfReader(fileName)
                            pageCount = reader.NumberOfPages
                        End If
                    End While
                    'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                    result = True
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End While
    End Sub
    Sub importAppendiceDetermina_Riduzione(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal listaRid As IList, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettDirReg As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal valoreCUP As String = "")

        Dim ragRid As DllDocumentale.ItemRiduzioneInfo
        Dim ragRidLiq As DllDocumentale.ItemRiduzioneLiqInfo

        While listaRid.Count > 0

            Dim result As Boolean = False
            Dim pdfCount As Integer = 0 'total input pdf file count
            Dim f As Integer = 0 'pointer to current input pdf file
            Dim fileName As String = String.Empty 'current input pdf filename
            Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
            Dim pageCount As Integer = 0 'cureent input pdf page count



            'Declare a variable to hold the imported pages
            Dim page As PdfImportedPage = Nothing
            Dim rotation As Integer = 0
            'Declare a font to used for the bookmarks
            Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
            12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)
            Try
                pdfCount = pdfFilesAppendice.Length
                If pdfCount > 0 Then
                    'Open the 1st pad using PdfReader object
                    fileName = pdfFilesAppendice(f)
                    reader = New iTextSharp.text.pdf.PdfReader(fileName)
                    'Get page count
                    pageCount = reader.NumberOfPages

                    Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                    Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

                    'Now loop thru the input pdfs
                    While f < pdfCount
                        'Declare a page counter variable
                        Dim i As Integer = 0
                        'Loop thru the current input pdf's pages starting at page 1
                        While i < pageCount



                            i += 1
                            'Get the input page size
                            pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                            'Create a new page on the output document
                            pdfDoc.NewPage()
                            'If it is the 1st page, we add bookmarks to the page
                            If i = 1 Then
                                'First create a paragraph using the filename as the heading
                                '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                'Then create a chapter from the above paragraph
                                para.IndentationLeft = -50000000
                                Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                                'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                'TabellaNumero.AddCell(para)
                                'TabellaNumero.TotalWidth = 1
                                'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                                'Finally add the chapter to the document

                                chpter.NumberDepth = -1
                                pdfDoc.Add(chpter)

                            End If
                            'Now we get the imported page
                            page = writer.GetImportedPage(reader, i)
                            Dim contaRighe As Integer = 25
                            Dim linea As Integer = 0



                            'Stampo intestazioni ufficio firma


                            Dim xUfficio As Single = 355
                            Dim yUfficio As Single = 750

                            Dim xNumero As Single = 365
                            Dim yNumero As Single = 682

                            Dim xData As Single = 365
                            Dim yData As Single = 682

                            Dim xCup As Single = 355
                            Dim yCup As Single = 650

                            Dim xDirigenteFirma As Single = 290
                            Dim yDirigenteFirma As Single = 68
                            Dim xDataFirma As Single = 455
                            Dim yDataFirma As Single = 68

                            Dim xOggetto As Single = 290
                            Dim yOggetto As Single = 68
                            Dim xIntegrale As Single = 290
                            Dim yIntegrale As Single = 68
                            Dim xEstratto As Single = 455
                            Dim yEstratto As Single = 68


                            Dim XimpegnoAss As Single = 36 'da introdurre
                            Dim YimpegnoAss As Single = 460 'da introdurre
                            'da introdurre


                            Dim XriduzioneRid As Single = XimpegnoAss
                            Dim YriduzioneRid As Single = YimpegnoAss


                            Dim XEsercizioRid As Single = 103 'da introdurre
                            Dim XUPBRid As Single = 149 'da introdurre
                            Dim XMissioneProgrammaRid As Single = XUPBRid 'da introdurre
                            Dim XCapitoloRid As Single = 204 'da introdurre
                            Dim XCostoRid As Single = 255 'da introdurre
                            Dim XNImpegnoRid As Single = 352 'da introdurre
                            Dim XTipoRid As Single = 425 'da introdurre
                            Dim XAnnoRid As Single = 460 'da introdurre
                            Dim XNumAttoRid As Single = 460 'da introdurre
                            Dim XDataAttoRid As Single = 510 'da introdurre

                            Dim strConfig As String = tipo

                            Try
                                xUfficio = ConfigurationManager.AppSettings("xUfficioAppendice" & strConfig)
                                yUfficio = ConfigurationManager.AppSettings("yUfficioAppendice" & strConfig)
                                xNumero = ConfigurationManager.AppSettings("xNumeroAppendice" & strConfig)
                                yNumero = ConfigurationManager.AppSettings("yNumeroAppendice" & strConfig)
                                xData = ConfigurationManager.AppSettings("xDataAppendice" & strConfig)
                                yData = ConfigurationManager.AppSettings("yDataAppendice" & strConfig)


                                xCup = IIf(ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "", xCup)
                                yCup = IIf(ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "", yCup)
                                '  xUfficio = xUfficio - 100

                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteFirma" & strConfig)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteFirma" & strConfig)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirma" & strConfig)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirma" & strConfig)

                                XimpegnoAss = ConfigurationManager.AppSettings("XimpegnoAss")
                                'YimpegnoAss = ConfigurationManager.AppSettings("YimpegnoAss")
                                YriduzioneRid = ConfigurationManager.AppSettings("YriduzioneRidAppendice")

                            Catch ex As Exception
                                ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                            End Try

                            Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                            TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                            TabellaUfficio.AddCell(New Phrase("" & Ufficio, ft))
                            TabellaUfficio.AddCell(New Phrase("" & codUfficio, ft))
                            TabellaUfficio.TotalWidth = 200
                            TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                            If lstr_oggetto <> "" Then
                                xOggetto = ConfigurationManager.AppSettings("xOggettoAppendice" & strConfig)
                                yOggetto = ConfigurationManager.AppSettings("yOggettoAppendice" & strConfig)
                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)

                            End If



                            If numDoc <> "" Then
                                Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaNumero.AddCell(numDoc)
                                TabellaNumero.TotalWidth = 225
                                TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)
                                Dim TabellaData As PdfPTable = New PdfPTable(1)
                                TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaData.AddCell(datadoc)
                                TabellaData.TotalWidth = 225
                                TabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                            End If



                            'CUP
                            Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                            TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaCUP.AddCell(New Phrase("" & valoreCUP, ft))
                            TabellaCUP.TotalWidth = 200
                            TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)




                            While listaRid.Count > 0 And linea < contaRighe
                                If (listaRid.Item(0).GetType) Is GetType(DllDocumentale.ItemRiduzioneInfo) Then


                                    ragRid = listaRid.Item(0)
                                    linea += 1
                                    If Not ragRid Is Nothing Then
                                        If flagInserisciNImpegni Then
                                            If "" & ragRid.Div_NumeroReg <> "0" Then
                                                Dim TabellaNumRid As PdfPTable = New PdfPTable(1)
                                                TabellaNumRid.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                                TabellaNumRid.AddCell(New Phrase(ragRid.Div_NumeroReg, ft))
                                                TabellaNumRid.TotalWidth = 100
                                                TabellaNumRid.WriteSelectedRows(0, -1, XriduzioneRid, YriduzioneRid, writer.DirectContent)

                                            End If

                                        End If

                                        If Not ragRid.Dli_MissioneProgramma Is Nothing AndAlso Not ragRid.Dli_MissioneProgramma.Trim() = String.Empty Then
                                            Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                            TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaMissioneProgramma.AddCell(New Phrase(ragRid.Dli_MissioneProgramma, ft))
                                            TabellaMissioneProgramma.TotalWidth = 100
                                            TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaRid, YriduzioneRid, writer.DirectContent)
                                        Else
                                            Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                            TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaUPB.AddCell(New Phrase(ragRid.Dli_UPB, ft))
                                            TabellaUPB.TotalWidth = 100
                                            TabellaUPB.WriteSelectedRows(0, -1, XUPBRid, YriduzioneRid, writer.DirectContent)
                                        End If

                                        Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                        TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCap.AddCell(New Phrase(ragRid.Dli_Cap, ft))
                                        TabellaCap.TotalWidth = 100
                                        TabellaCap.WriteSelectedRows(0, -1, XCapitoloRid, YriduzioneRid, writer.DirectContent)

                                        Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                        TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaEsercizio.AddCell(New Phrase(ragRid.Dli_Esercizio, ft))
                                        TabellaEsercizio.TotalWidth = 100
                                        TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioRid, YriduzioneRid, writer.DirectContent)

                                        If ragRid.Dli_Costo > 0 Then
                                            Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                            TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragRid.Dli_Costo), ft))
                                            TabellaCosto.TotalWidth = 100
                                            TabellaCosto.WriteSelectedRows(0, -1, XCostoRid, YriduzioneRid, writer.DirectContent)
                                        End If



                                        If "" & ragRid.Dli_NumImpegno <> "0" Then
                                            Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                                            TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            Dim lstr_impl As String = ""
                                            If ragRid.IsPreImp Then
                                                lstr_impl = ragRid.Dli_NPreImpegno & "-PREIMP"
                                            Else
                                                lstr_impl = ragRid.Dli_NumImpegno & "-IMP"
                                            End If

                                            'TabellaNumImpcon.AddCell(New Phrase(ragRid.Dli_NumImpegno, ft))
                                            TabellaNumImpcon.AddCell(New Phrase(lstr_impl, ft))
                                            TabellaNumImpcon.TotalWidth = 100
                                            TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoRid, YriduzioneRid, writer.DirectContent)


                                            If "" & ragRid.Div_Num_assunzione <> "0" Then
                                                Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                                                TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                                TabellaNumAs.AddCell(New Phrase(ragRid.Div_Num_assunzione, ft))
                                                TabellaNumAs.TotalWidth = 100
                                                TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoRid, YriduzioneRid, writer.DirectContent)


                                                Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                                                TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                                TabellaDataAs.AddCell(New Phrase(ragRid.Div_Data_Assunzione.Date, ft))
                                                TabellaDataAs.TotalWidth = 100
                                                TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoRid, YriduzioneRid, writer.DirectContent)


                                            End If


                                        End If


                                        If ragRid.Div_TipoAssunzione = 1 Then

                                            Dim TabellaDel As PdfPTable = New PdfPTable(1)
                                            TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaDel.AddCell(New Phrase("DEL", ft))
                                            TabellaDel.TotalWidth = 100
                                            TabellaDel.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                                        End If

                                        If ragRid.Div_TipoAssunzione = 0 Then
                                            Dim TabellaDet As PdfPTable = New PdfPTable(1)
                                            TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaDet.AddCell(New Phrase("DET", ft))
                                            TabellaDet.TotalWidth = 100
                                            TabellaDet.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                                        End If

                                        YriduzioneRid -= 16

                                        listaRid.Remove(ragRid)
                                    End If

                                Else





                                    'Gestione Rig Liq
                                    ragRidLiq = listaRid.Item(0)
                                    linea += 1
                                    If Not ragRidLiq Is Nothing Then
                                        If flagInserisciNImpegni Then
                                            If "" & ragRidLiq.Div_NumeroReg <> "0" Then
                                                Dim TabellaNumRid As PdfPTable = New PdfPTable(1)
                                                TabellaNumRid.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                                TabellaNumRid.AddCell(New Phrase(ragRidLiq.Div_NumeroReg, ft))
                                                TabellaNumRid.TotalWidth = 100
                                                TabellaNumRid.WriteSelectedRows(0, -1, XriduzioneRid, YriduzioneRid, writer.DirectContent)

                                            End If
                                        End If

                                        If Not ragRidLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragRidLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                                            Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                            TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaMissioneProgramma.AddCell(New Phrase(ragRidLiq.Dli_MissioneProgramma, ft))
                                            TabellaMissioneProgramma.TotalWidth = 100
                                            TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaRid, YriduzioneRid, writer.DirectContent)
                                        Else
                                            Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                            TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaUPB.AddCell(New Phrase(ragRidLiq.Dli_UPB, ft))
                                            TabellaUPB.TotalWidth = 100
                                            TabellaUPB.WriteSelectedRows(0, -1, XUPBRid, YriduzioneRid, writer.DirectContent)
                                        End If

                                        Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                        TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCap.AddCell(New Phrase(ragRidLiq.Dli_Cap, ft))
                                        TabellaCap.TotalWidth = 100
                                        TabellaCap.WriteSelectedRows(0, -1, XCapitoloRid, YriduzioneRid, writer.DirectContent)

                                        Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                        TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaEsercizio.AddCell(New Phrase(ragRidLiq.Dli_Esercizio, ft))
                                        TabellaEsercizio.TotalWidth = 100
                                        TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioRid, YriduzioneRid, writer.DirectContent)

                                        If ragRid.Dli_Costo > 0 Then
                                            Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                            TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                            TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragRidLiq.Dli_Costo), ft))
                                            TabellaCosto.TotalWidth = 100
                                            TabellaCosto.WriteSelectedRows(0, -1, XCostoRid, YriduzioneRid, writer.DirectContent)
                                        End If



                                        If "" & ragRidLiq.Div_NLiquidazione <> "0" Then
                                            Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                                            TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNumImpcon.AddCell(New Phrase(ragRidLiq.Div_NLiquidazione & "-LIQ", ft))
                                            TabellaNumImpcon.TotalWidth = 100
                                            TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoRid, YriduzioneRid, writer.DirectContent)


                                            'If "" & ragRidLiq. <> "0" Then
                                            '    Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                                            '    TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            '    TabellaNumAs.AddCell(New Phrase(ragRidLiq.Div_Num_assunzione, ft))
                                            '    TabellaNumAs.TotalWidth = 100
                                            '    TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoRid, YriduzioneRid, writer.DirectContent)


                                            '    Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                                            '    TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            '    TabellaDataAs.AddCell(New Phrase(ragRidLiq.Div_Data_Assunzione.Date, ft))
                                            '    TabellaDataAs.TotalWidth = 100
                                            '    TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoRid, YriduzioneRid, writer.DirectContent)


                                            'End If


                                        End If


                                        'If ragRidLiq.Div_TipoAssunzione = 1 Then

                                        '    Dim TabellaDel As PdfPTable = New PdfPTable(1)
                                        '    TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        '    TabellaDel.AddCell(New Phrase("DEL", ft))
                                        '    TabellaDel.TotalWidth = 100
                                        '    TabellaDel.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                                        'End If

                                        'If ragRidLiq.Div_TipoAssunzione = 0 Then
                                        '    Dim TabellaDet As PdfPTable = New PdfPTable(1)
                                        '    TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        '    TabellaDet.AddCell(New Phrase("DET", ft))
                                        '    TabellaDet.TotalWidth = 100
                                        '    TabellaDet.WriteSelectedRows(0, -1, XTipoRid, YriduzioneRid, writer.DirectContent)
                                        'End If

                                        YriduzioneRid -= 16

                                        listaRid.Remove(ragRidLiq)
                                    End If





                                End If


                            End While











                            'Read the imported page's rotation
                            rotation = reader.GetPageRotation(i)
                            'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                            If rotation = 90 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            ElseIf rotation = 270 Then
                                cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                            End If


                        End While
                        'Increment f and read the next input pdf file
                        f += 1
                        If f < pdfCount Then
                            fileName = pdfFilesAppendice(f)
                            reader = New iTextSharp.text.pdf.PdfReader(fileName)
                            pageCount = reader.NumberOfPages
                        End If
                    End While
                    'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                    result = True
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End While
    End Sub
    Sub importAppendiceDisposizione_Liquidazione(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal listaRagLiq As IList, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettDirReg As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String, Optional ByVal flagInserisciNImpegni As Boolean = True, Optional ByVal valoreCUP As String = "")
        Dim ragLiq As DllDocumentale.ItemLiquidazioneInfo

        While listaRagLiq.Count > 0

            Dim result As Boolean = False
            Dim pdfCount As Integer = 0 'total input pdf file count
            Dim f As Integer = 0 'pointer to current input pdf file
            Dim fileName As String = String.Empty 'current input pdf filename
            Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
            Dim pageCount As Integer = 0 'cureent input pdf page count



            'Declare a variable to hold the imported pages
            Dim page As PdfImportedPage = Nothing
            Dim rotation As Integer = 0
            'Declare a font to used for the bookmarks
            Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
            12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)
            Try
                pdfCount = pdfFilesAppendice.Length
                If pdfCount > 0 Then
                    'Open the 1st pad using PdfReader object
                    fileName = pdfFilesAppendice(f)
                    reader = New iTextSharp.text.pdf.PdfReader(fileName)
                    'Get page count
                    pageCount = reader.NumberOfPages

                    Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                    Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)


                    'Now loop thru the input pdfs
                    While f < pdfCount
                        'Declare a page counter variable
                        Dim i As Integer = 0
                        'Loop thru the current input pdf's pages starting at page 1
                        While i < pageCount



                            i += 1
                            'Get the input page size
                            pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                            'Create a new page on the output document
                            pdfDoc.NewPage()
                            'If it is the 1st page, we add bookmarks to the page
                            If i = 1 Then
                                'First create a paragraph using the filename as the heading
                                '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                                'Then create a chapter from the above paragraph
                                para.IndentationLeft = -50000000
                                Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                                'Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                'TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                'TabellaNumero.AddCell(para)
                                'TabellaNumero.TotalWidth = 1
                                'TabellaNumero.WriteSelectedRows(0, -1, -1, -1, writer.DirectContent)

                                'Finally add the chapter to the document

                                chpter.NumberDepth = -1
                                pdfDoc.Add(chpter)

                            End If
                            'Now we get the imported page
                            page = writer.GetImportedPage(reader, i)
                            Dim contaRighe As Integer = 25
                            Dim linea As Integer = 0



                            'Stampo intestazioni ufficio firma


                            Dim xUfficio As Single = 355
                            Dim yUfficio As Single = 750
                            Dim yUfficioAppendice As Single = 750

                            Dim yNumeroAppendice As Single = 682
                            Dim xNumero As Single = 365
                            Dim yNumero As Single = 682

                            Dim xData As Single = 365
                            Dim yData As Single = 682
                            Dim yDataAppendice As Single = 682

                            Dim xCup As Single = 355
                            Dim yCup As Single = 650

                            Dim xDirigenteFirma As Single = 290
                            Dim yDirigenteFirma As Single = 68
                            Dim xDataFirma As Single = 455
                            Dim yDataFirma As Single = 68

                            Dim xOggettoAppendice As Single = 37
                            Dim yOggettoAppendice As Single = 700



                            Dim XimpegnoAss As Single = 36 'da introdurre

                            'da introdurre


                            Dim XliquidazioneLiq As Single = XimpegnoAss
                            Dim YliquidazioneLiqAppendice As Single = 462 'da introdurre

                            Dim XEsercizioLiq As Single = 103 'da introdurre
                            Dim XUPBLiq As Single = 149 'da introdurre
                            Dim XMissioneProgrammaLiq As Single = XUPBLiq
                            Dim XCapitoloLiq As Single = 204 'da introdurre
                            Dim XCostoLiq As Single = 256 'da introdurre
                            Dim XNImpegnoLiq As Single = 352 'da introdurre
                            Dim XTipoLiq As Single = 425 'da introdurre
                            Dim XAnnoLiq As Single = 460 'da introdurre
                            Dim XNumAttoLiq As Single = 460 'da introdurre
                            Dim XDataAttoLiq As Single = 510 'da introdurre

                            Dim strConfig As String = tipo

                            Try
                                yUfficioAppendice = ConfigurationManager.AppSettings("xUfficioAppendice" & strConfig)
                                yUfficioAppendice = ConfigurationManager.AppSettings("yUfficioAppendice" & strConfig)

                                xNumero = ConfigurationManager.AppSettings("xNumeroAppendice" & strConfig)
                                yNumeroAppendice = ConfigurationManager.AppSettings("yNumeroAppendice" & strConfig)
                                xData = ConfigurationManager.AppSettings("xDataAppendice" & strConfig)
                                yDataAppendice = ConfigurationManager.AppSettings("yDataAppendice" & strConfig)

                                xCup = IIf(ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("xCupAppendice" & strConfig) & "", xCup)
                                yCup = IIf(ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "" <> "", ConfigurationManager.AppSettings("yCupAppendice" & strConfig) & "", yCup)
                                '  xUfficio = xUfficio - 100

                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteFirma" & strConfig)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteFirma" & strConfig)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirma" & strConfig)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirma" & strConfig)



                            Catch ex As Exception
                                ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                            End Try

                            Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                            TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER


                            TabellaUfficio.AddCell(New Phrase("" & Ufficio, ft))
                            TabellaUfficio.AddCell(New Phrase("" & codUfficio, ft))
                            TabellaUfficio.TotalWidth = 200
                            TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                            If lstr_oggetto <> "" Then
                                xOggettoAppendice = ConfigurationManager.AppSettings("xOggettoAppendice" & strConfig)
                                yOggettoAppendice = ConfigurationManager.AppSettings("yOggettoAppendice" & strConfig)


                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggettoAppendice, yOggettoAppendice, writer.DirectContent)

                            End If



                            If numDoc <> "" Then
                                Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                                TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaNumero.AddCell(numDoc)
                                TabellaNumero.TotalWidth = 225
                                TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumeroAppendice, writer.DirectContent)
                                Dim TabellaData As PdfPTable = New PdfPTable(1)
                                TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                TabellaData.AddCell(datadoc)
                                TabellaData.TotalWidth = 225
                                TabellaData.WriteSelectedRows(0, -1, xData, yDataAppendice, writer.DirectContent)
                            End If

                            'CUP
                            Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                            TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaCUP.AddCell(New Phrase("" & valoreCUP, ft))
                            TabellaCUP.TotalWidth = 200
                            TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)

                            XEsercizioLiq = ConfigurationManager.AppSettings("XEsercizioLiq")
                            XUPBLiq = ConfigurationManager.AppSettings("XUPBLiq")
                            XMissioneProgrammaLiq = ConfigurationManager.AppSettings("XMissioneProgrammaLiq")
                            XCapitoloLiq = ConfigurationManager.AppSettings("XCapitoloLiq")
                            XCostoLiq = ConfigurationManager.AppSettings("XCostoLiq")
                            XNImpegnoLiq = ConfigurationManager.AppSettings("XNImpegnoLiq")
                            XTipoLiq = ConfigurationManager.AppSettings("XTipoLiq")
                            XAnnoLiq = ConfigurationManager.AppSettings("XAnnoLiq")
                            XNumAttoLiq = ConfigurationManager.AppSettings("XNumAttoLiq")
                            XDataAttoLiq = ConfigurationManager.AppSettings("XDataAttoLiq")




                            While listaRagLiq.Count > 0 And linea < contaRighe
                                ragLiq = listaRagLiq.Item(0)
                                linea += 1
                                If Not ragLiq Is Nothing Then


                                    If flagInserisciNImpegni Then
                                        If "" & ragLiq.Dli_NLiquidazione <> "0" Then
                                            Dim TabellaNumLiq As PdfPTable = New PdfPTable(1)
                                            TabellaNumLiq.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNumLiq.AddCell(New Phrase(ragLiq.Dli_NLiquidazione, ft))
                                            TabellaNumLiq.TotalWidth = 100
                                            TabellaNumLiq.WriteSelectedRows(0, -1, XliquidazioneLiq, YliquidazioneLiqAppendice, writer.DirectContent)

                                        End If

                                    End If

                                    If Not ragLiq.Dli_MissioneProgramma Is Nothing AndAlso Not ragLiq.Dli_MissioneProgramma.Trim() = String.Empty Then
                                        Dim TabellaMissioneProgramma As PdfPTable = New PdfPTable(1)
                                        TabellaMissioneProgramma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaMissioneProgramma.AddCell(New Phrase(ragLiq.Dli_MissioneProgramma, ft))
                                        TabellaMissioneProgramma.TotalWidth = 100
                                        TabellaMissioneProgramma.WriteSelectedRows(0, -1, XMissioneProgrammaLiq, YliquidazioneLiqAppendice, writer.DirectContent)
                                    Else
                                        Dim TabellaUPB As PdfPTable = New PdfPTable(1)
                                        TabellaUPB.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaUPB.AddCell(New Phrase(ragLiq.Dli_UPB, ft))
                                        TabellaUPB.TotalWidth = 100
                                        TabellaUPB.WriteSelectedRows(0, -1, XUPBLiq, YliquidazioneLiqAppendice, writer.DirectContent)
                                    End If

                                    Dim TabellaCap As PdfPTable = New PdfPTable(1)
                                    TabellaCap.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaCap.AddCell(New Phrase(ragLiq.Dli_Cap, ft))
                                    TabellaCap.TotalWidth = 100
                                    TabellaCap.WriteSelectedRows(0, -1, XCapitoloLiq, YliquidazioneLiqAppendice, writer.DirectContent)

                                    Dim TabellaEsercizio As PdfPTable = New PdfPTable(1)
                                    TabellaEsercizio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                    TabellaEsercizio.AddCell(New Phrase(ragLiq.Dli_Esercizio, ft))
                                    TabellaEsercizio.TotalWidth = 100
                                    TabellaEsercizio.WriteSelectedRows(0, -1, XEsercizioLiq, YliquidazioneLiqAppendice, writer.DirectContent)

                                    If ragLiq.Dli_Costo > 0 Then
                                        Dim TabellaCosto As PdfPTable = New PdfPTable(1)
                                        TabellaCosto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaCosto.AddCell(New Phrase("€ " & String.Format("{0:n}", ragLiq.Dli_Costo), ft))
                                        TabellaCosto.TotalWidth = 100
                                        TabellaCosto.WriteSelectedRows(0, -1, XCostoLiq, YliquidazioneLiqAppendice, writer.DirectContent)
                                    End If



                                    If "" & ragLiq.Dli_NumImpegno <> "0" Then
                                        Dim TabellaNumImpcon As PdfPTable = New PdfPTable(1)
                                        TabellaNumImpcon.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaNumImpcon.AddCell(New Phrase(ragLiq.Dli_NumImpegno, ft))
                                        TabellaNumImpcon.TotalWidth = 100
                                        TabellaNumImpcon.WriteSelectedRows(0, -1, XNImpegnoLiq, YliquidazioneLiqAppendice, writer.DirectContent)


                                        If "" & ragLiq.Dli_Num_assunzione <> "0" Then
                                            Dim TabellaNumAs As PdfPTable = New PdfPTable(1)
                                            TabellaNumAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaNumAs.AddCell(New Phrase(ragLiq.Dli_Num_assunzione, ft))
                                            TabellaNumAs.TotalWidth = 100
                                            TabellaNumAs.WriteSelectedRows(0, -1, XNumAttoLiq, YliquidazioneLiqAppendice, writer.DirectContent)


                                            Dim TabellaDataAs As PdfPTable = New PdfPTable(1)
                                            TabellaDataAs.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                            TabellaDataAs.AddCell(New Phrase(ragLiq.Dli_Data_Assunzione.Date, ft))
                                            TabellaDataAs.TotalWidth = 100
                                            TabellaDataAs.WriteSelectedRows(0, -1, XDataAttoLiq, YliquidazioneLiqAppendice, writer.DirectContent)


                                        End If


                                    End If


                                    If ragLiq.Dli_TipoAssunzione = 1 Then

                                        Dim TabellaDel As PdfPTable = New PdfPTable(1)
                                        TabellaDel.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaDel.AddCell(New Phrase("DEL", ft))
                                        TabellaDel.TotalWidth = 100
                                        TabellaDel.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiqAppendice, writer.DirectContent)
                                    End If

                                    If ragLiq.Dli_TipoAssunzione = 0 Then
                                        Dim TabellaDet As PdfPTable = New PdfPTable(1)
                                        TabellaDet.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                        TabellaDet.AddCell(New Phrase("DET", ft))
                                        TabellaDet.TotalWidth = 100
                                        TabellaDet.WriteSelectedRows(0, -1, XTipoLiq, YliquidazioneLiqAppendice, writer.DirectContent)
                                    End If








                                    YliquidazioneLiqAppendice -= 16

                                    listaRagLiq.Remove(ragLiq)
                                End If
                            End While










                            'Read the imported page's rotation
                            rotation = reader.GetPageRotation(i)
                            'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                            If rotation = 90 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            ElseIf rotation = 270 Then
                                cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                            End If


                        End While
                        'Increment f and read the next input pdf file
                        f += 1
                        If f < pdfCount Then
                            fileName = pdfFilesAppendice(f)
                            reader = New iTextSharp.text.pdf.PdfReader(fileName)
                            pageCount = reader.NumberOfPages
                        End If
                    End While
                    'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                    result = True
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End While
    End Sub
    Sub importAppendiceNoteUP(ByRef cb As PdfContentByte, ByVal pdfFilesAppendice() As String, ByVal pdfDoc As iTextSharp.text.Document, ByVal writer As PdfWriter, ByVal Ufficio As String, ByVal codUfficio As String, ByVal numDoc As String, ByVal datadoc As String, ByVal vettUfficioProp As Object, ByVal tipo As String, ByVal lstr_oggetto As String, ByVal lstr_TipoPubblicazione As String)


        Dim result As Boolean = False
        Dim pdfCount As Integer = 0 'total input pdf file count
        Dim f As Integer = 0 'pointer to current input pdf file
        Dim fileName As String = String.Empty 'current input pdf filename
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim pageCount As Integer = 0 'cureent input pdf page count



        'Declare a variable to hold the imported pages
        Dim page As PdfImportedPage = Nothing
        Dim rotation As Integer = 0
        'Declare a font to used for the bookmarks
        Dim bookmarkFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, _
        12, iTextSharp.text.Font.BOLD, BaseColor.BLUE)
        Try
            pdfCount = pdfFilesAppendice.Length
            If pdfCount > 0 Then
                'Open the 1st pad using PdfReader object
                fileName = pdfFilesAppendice(f)
                reader = New iTextSharp.text.pdf.PdfReader(fileName)
                'Get page count
                pageCount = reader.NumberOfPages



                'Now loop thru the input pdfs
                'f conta il numero di appendici da allegare
                Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
                Dim ft As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)

                While f < pdfCount
                    'Declare a page counter variable
                    Dim i As Integer = 0
                    'Loop thru the current input pdf's pages starting at page 1
                    While i < pageCount



                        i += 1
                        'Get the input page size
                        pdfDoc.SetPageSize(reader.GetPageSizeWithRotation(i))
                        'Create a new page on the output document
                        pdfDoc.NewPage()
                        'If it is the 1st page, we add bookmarks to the page
                        If i = 1 Then
                            'First create a paragraph using the filename as the heading
                            '                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                            Dim para As New iTextSharp.text.Paragraph(Path.GetFileName(fileName).ToUpper(), bookmarkFont)
                            'Then create a chapter from the above paragraph
                            para.IndentationLeft = -50000000
                            Dim chpter As New iTextSharp.text.Chapter(para, f + 1)

                            'Finally add the chapter to the document

                            chpter.NumberDepth = -1
                            pdfDoc.Add(chpter)

                        End If
                        'Now we get the imported page
                        page = writer.GetImportedPage(reader, i)
                        Dim contaRighe As Integer = 25
                        Dim linea As Integer = 0

                        'Stampo intestazioni ufficio firma
                        'Dim xUfficio As Single = 355
                        'Dim yUfficio As Single = 750
                        'Dim yUfficioAppendice As Single = 750

                        'Dim yNumeroAppendice As Single = 682
                        'Dim xNumero As Single = 365
                        'Dim yNumero As Single = 682

                        'Dim xData As Single = 365
                        'Dim yData As Single = 682
                        'Dim yDataAppendice As Single = 682

                        'Dim xDirigenteFirma As Single = 290
                        'Dim yDirigenteFirma As Single = 68
                        'Dim xDataFirma As Single = 455
                        'Dim yDataFirma As Single = 68

                        'Dim xOggettoAppendice As Single = 37
                        'Dim yOggettoAppendice As Single = 700



                        Dim xDirigenteOsservazioniUP As Single = 0
                        Dim yDirigenteOsservazioniUP As Single = 0

                        Dim strConfig As String = tipo

                        Try


                            xDirigenteOsservazioniUP = ConfigurationManager.AppSettings("xDirigenteOsservazioniUP" & strConfig)
                            yDirigenteOsservazioniUP = ConfigurationManager.AppSettings("yDirigenteOsservazioniUP" & strConfig)

                            'inizializzo a valori di default
                            If xDirigenteOsservazioniUP = 0 Then
                                xDirigenteOsservazioniUP = 37
                            End If
                            If yDirigenteOsservazioniUP = 0 Then
                                yDirigenteOsservazioniUP = 600
                            End If

                        Catch ex As Exception
                            ' Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfig)
                        End Try



                        Dim TabellaOssDir As PdfPTable = New PdfPTable(1)
                        TabellaOssDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        Dim OssDir As Phrase = New Phrase("" & vettUfficioProp(3), ft)
                        TabellaOssDir.TotalWidth = 500
                        TabellaOssDir.AddCell(OssDir)
                        TabellaOssDir.WriteSelectedRows(0, -1, xDirigenteOsservazioniUP, yDirigenteOsservazioniUP, writer.DirectContent)


                    End While

                    'Read the imported page's rotation
                    rotation = reader.GetPageRotation(i)
                    'Then add the imported page to the PdfContentByte object as a template based on the page's rotation


                    If rotation = 90 Then
                        cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                    ElseIf rotation = 270 Then
                        cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)
                    Else
                        cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 2, 2)
                    End If

                    'Increment f and read the next input pdf file
                    f += 1
                End While
                'When all done, we close the documwent so that the pdfwriter object can write it to the output file

                result = True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Function CreaDocumento(ByRef vettoredati As Object, ByRef context As HttpContext, ByVal flagIsPrew As Boolean) As Byte()
        Try
            Dim arrByteReturn() As Byte = Nothing
            Dim nomePDFGenerato As String = vettoredati(2)
            nomePDFGenerato = nomePDFGenerato.Replace("/", "_").Replace("\", "_")

            Dim codufficio As String = ""
            Dim Ufficio As String = ""
            Dim numdoc As String = ""
            Dim datadoc As String = ""
            Dim vett As Object
            Dim isRigettoFormaleByCA As Boolean = False
            Dim stampaNumerodata As Boolean = False
            Dim vettDirGen(4) As Object
            '(0) note
            '(1) Cognome e Nome
            '(2) Data Firma
            '(3) utente
            Dim vettDirReg(4) As Object
            Dim vettContAmm(4) As Object
            Dim vettContSegLegittimita(4) As Object
            Dim vettSegretarioPresidenza(4) As Object

            Dim segreteriaAssessorato As String = ""

            Dim vettPresidente(4) As Object
            Dim vettUfficioProp(4) As Object
            Dim lstr_Oggetto As String = ""
            Dim lstr_TipoPubblicazione As String = ""
            Dim strConfig As String = ""

            Dim codAzione As String = context.Session.Item("codAzione")
            Dim tipoAtto As Integer = CInt(context.Session("tipoApplic"))
            Select Case tipoAtto
                Case 0 'Determine
                    strConfig = "Determine"
                Case 1 'Delibere
                    strConfig = "Delibere"
                Case 2 'Disposizione
                    strConfig = "Disposizioni"
            End Select

            Dim infoDatiSeduta As DllDocumentale.ItemDatiSedutaInfo = Nothing

            'Note (0)
            'Cognome nome (1)
            'Data(2)
            'operatore(2)
            Dim ope As DllAmbiente.Operatore = context.Session("oOperatore")
            Dim objDocumento As New DllDocumentale.svrDocumenti(ope)
            Dim vParm(1) As Object
            Dim arr_elenco_compiti_documento As Object

            Dim flagDisattiva As Boolean = True
            Dim flagStampaUffProponente As Boolean = True
            Dim istruttore As String = ""
            Dim poc As String = ""
            Dim dirUP As String = ""
            Dim dirGen As String = ""
            Dim dirSegrPresLegittimita As String = ""
            Dim isUDD As Boolean = False
            Dim isUSL As Boolean = False

            Dim descrDipartimento As String = ""
            Dim barraDirettoreGenerale As Boolean = False
            Dim msgEsito As String = ""

            Dim isNuovaVersioneDocumentoCaricato As Boolean = False

            Dim docDll As New DllDocumentale.svrDocumenti(ope)

            Try
                If idDocFromAllegato = "" Then
                    idDocFromAllegato = context.Session("idDocumento")
                End If

                vett = Leggi_Documento(idDocFromAllegato)
                codufficio = "" & vett(1)(8)
                Ufficio = "" & vett(1)(9)

                ' Numerazione Delibere da parte del Dirigente dell'uff di Segreteria di Pres - Approvazione
                Dim numera As Boolean = docDll.Numerare(idDocFromAllegato, ope, tipoAtto)
                If Not codAzione.Contains("RIGETTO") Then
                    If (tipoAtto = 1) Then
                        docDll.Numera(idDocFromAllegato, ope, tipoAtto)


                        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(ope)).Get_StatoIstanzaDocumento(idDocFromAllegato)

                        'rileggo le info del DOC per ottenere il definitivo appena assegnato
                        Dim objDocumentoAggiornato As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocFromAllegato)
                        If objDocumentoAggiornato.Doc_numero <> "" Then
                            Dim tipoAttoStr As String = "DELIBERA"
                            Dim dataAtto As Date = objDocumentoAggiornato.Doc_Data
                            Dim numeroAtto As String = Right(objDocumentoAggiornato.Doc_numero, 5)

                            Dim dipartimento As String = objDocumentoAggiornato.Doc_Cod_Uff_Pubblico

                            Dim rispostaGenerazionePreimpegno As String()
                            Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
                            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

                            Dim listaPreimpegni As IList(Of DllDocumentale.ItemImpegnoInfo)
                            listaPreimpegni = docDll.FO_Get_DatiPreImpegni(idDocFromAllegato)

                            For Each item As DllDocumentale.ItemImpegnoInfo In listaPreimpegni
                                Try
                                    'rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(operatore, NumPreImpegno, importo, tipoAttoStr, objDocumento.Doc_Data, numeroAtto, dipartimento, contoEconomico, ratei, importoIrap, risconti, datamovimento, Codice_Obbiettivo_Gestionale, pcf)
                                    Dim pcf As String
                                    If Not item.Piano_Dei_Conti_Finanziari Is Nothing Or item.Piano_Dei_Conti_Finanziari <> "" Then
                                        pcf = item.Piano_Dei_Conti_Finanziari
                                    Else
                                        pcf = item.Dli_PianoDeiContiFinanziari
                                    End If

                                    If Not item.Di_TipoAssunzioneDescr.Equals("DELIBERA") Then
                                        ' in questa chiamata passo il numero di PREIMPEGNO da aggiornare, il numero definitivo dell'atto e tutti i dati relativi.
                                        'VERIFICARE SE COME DATAMOVIMENTO VA BENE QUELLA DEL DOCUMENTO!!!!!
                                        Dim numeroProvvisOrDefAtto As String = ""
                                        If objDocumentoAggiornato.Doc_numero Is Nothing then
                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numeroProvvisorio
                                        Else
                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numero
                                        End If

                                        'La registrazione in ragioneria avviene uno alla volta.
                                        'anche se il SIC risponde con un preimpegni multipli, io leggerò sempre e solo il primo numero preimp e il primo docid
                                        'si sta trasformando il preimp-provv in preimp-def, per cui devo generare un nuovo token per la nuova chiamata al SIC
                                        Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                                        rispostaGenerazionePreimpegno = ClientIntegrazioneSic.MessageMaker.createGenerazionePreImpegnoRagMessage(ope, item.Dli_NPreImpegno, objDocumentoAggiornato.Doc_Oggetto, tipoAttoStr, objDocumentoAggiornato.Doc_Data, Right(objDocumentoAggiornato.Doc_numero, 5), dipartimento, objDocumentoAggiornato.Doc_Data, idDocFromAllegato, numeroProvvisOrDefAtto, hashTokenCallSic)

                                        Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)

                                        Dim itemRagPreimp As DllDocumentale.ItemImpegnoInfo = item
                                        With itemRagPreimp
                                            .HashTokenCallSic = hashTokenCallSic
                                            .IdDocContabileSic = idDocSIC1
                                            .Dli_NPreImpegno = NumPreImpegno1
                                            .Dli_DataRegistrazione = Now
                                            .Dli_Operatore = ope.Codice
                                            .Dli_prog = item.Dli_prog
                                            .Di_TipoAssunzione = 1
                                            .Di_Data_Assunzione = objDocumentoAggiornato.Doc_Data
                                            .Di_Num_assunzione = objDocumentoAggiornato.Doc_numero
                                            .Di_TipoAssunzioneDescr = tipoAttoStr
                                        End With
                                        ' evito la registrazione dell'azione utente sul documento, altrimenti dal monitor, viene visualizzazata
                                        ' una informazione errata.
                                        docDll.FO_Update_Preimpegno(itemRagPreimp, False)
                                    End If

                                    

                                Catch ex As Exception
                                    HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                                    'Response.Redirect("MessaggioErrore.aspx")
                                    msgEsito = msgEsito & "<p style='color:red'>     ***   ATTENZIONE   *** <br />Errore sull'atto " & objDocumentoAggiornato.Doc_numero & " durante la registrazione del preimpegno " & item.Dli_NumImpegno & ": contattare l'amministratore di sistema.</p>"



                                    Throw New Exception(ex.Message)
                                End Try
                            Next


                            Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo)
                            listaImpegni = docDll.FO_Get_DatiImpegni(idDocFromAllegato)

                            Dim flagResult As Boolean = False
                            Dim rispostaGenerazioneImpegno As String()

                            For Each item As DllDocumentale.ItemImpegnoInfo In listaImpegni
                                Try
                                    Dim pcf As String
                                    If Not item.Piano_Dei_Conti_Finanziari Is Nothing Or item.Piano_Dei_Conti_Finanziari <> "" Then
                                        pcf = item.Piano_Dei_Conti_Finanziari
                                    Else
                                        pcf = item.Dli_PianoDeiContiFinanziari
                                    End If

                                    Dim contoEconomicaLista As ArrayList = GetArrContoEconomica(ope, item.DBi_Anno, item.Dli_Cap, codufficio)
                                    If item.Di_ContoEconomica = "" Then
                                        If contoEconomicaLista.Count = 1 Then
                                            item.Di_ContoEconomica = contoEconomicaLista(0)
                                        End If

                                    End If
                                    ' in questa chiamata passo il numero di IMPEGNO da aggiornare, il numero definitivo dell'atto e tutti i dati relativi.

                                    If item.Dli_NumImpegno = 0 Then
                                        Dim listaBenReturn As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = docDll.FO_Get_ListaBeneficiariImpegno(ope,,, item.Dli_prog)
                                        Dim arrayBen As Array = ProcAmm.TrasformaBeneficiariInterniInSIC(listaBenReturn)
                                        Dim numeroProvvisOrDefAtto As String = ""
                                        If objDocumentoAggiornato.Doc_numero Is Nothing then
                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numeroProvvisorio
                                        Else
                                            numeroProvvisOrDefAtto = objDocumentoAggiornato.Doc_numero
                                        End If

                                        'Sto registrando gli impegni definitivi in ragioneria, devo inviare un nuovo token al SIC
                                        Dim hashTokenCallSic As String = GenerateHashTokenCallSic()

                                        rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(ope, item.Dli_NPreImpegno, item.Dli_Costo, tipoAttoStr, objDocumentoAggiornato.Doc_Data, numeroAtto, dipartimento, item.Di_ContoEconomica, item.Di_Ratei, item.Di_ImpostaIrap, item.Di_Risconti, Now.Date, item.Codice_Obbiettivo_Gestionale, pcf, objDocumentoAggiornato.Doc_Oggetto, idDocFromAllegato, numeroProvvisOrDefAtto, hashTokenCallSic, arrayBen)
                                        
                                        Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                                        Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)
                                        
                                         Dim itemRagImp As DllDocumentale.ItemImpegnoInfo = item
                                        With itemRagImp
                                            itemRagImp.HashTokenCallSic_Imp = hashTokenCallSic
                                            itemRagImp.IdDocContabileSic_Imp = idDocSIC1
                                            itemRagImp.Dli_NumImpegno = NumPreImpegno1
                                        End With
                                        docDll.FO_Update_Bilancio(item)
                                    End If
                                   
                                Catch ex As Exception
                                    HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                                    'Response.Redirect("MessaggioErrore.aspx")
                                    msgEsito = msgEsito & "<p style='color:red'>     ***   ATTENZIONE   *** <br />Errore sull'atto " & objDocumentoAggiornato.Doc_numero & " durante la registrazione dell'impegno " & item.Dli_NumImpegno & ": contattare l'amministratore di sistema.</p>"
                                    Throw New Exception(ex.Message)

                                End Try
                            Next
                        End If           
                    End If
                End If

                isNuovaVersioneDocumentoCaricato = objDocumento.IsNuovaVersioneDocumento(idDocFromAllegato)


                Dim dataCreazioneDocumento As Date = vett(1)(7)
                lstr_Oggetto = "" & vett(1)(0)
                lstr_TipoPubblicazione = "" & vett(1)(5)

                vParm(0) = idDocFromAllegato
                'IMPOSTA IL NOMINATIVO CON NOME E COGNOME (ANZICHè COGNOME E NOME)
                vParm(1) = "1"
                arr_elenco_compiti_documento = objDocumento.FO_Elenco_Compiti_Documento(vParm)

                Dim resUffRagio As String = ""
                Dim resUffGen As String = ""
                Dim resUffcontrolloAmm As String = ""
                Dim resUffSegreteriaLegittimita As String = ""
                Dim resUffSegreteriaDiPresidenza As String = ""
                Dim resUffPresidenza As String = ""

                If IsArray(arr_elenco_compiti_documento(1)) Then
                    For contaElencoCompiti As Integer = 0 To UBound(arr_elenco_compiti_documento(1), 2)

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("ISTRUTTORE") And istruttore = "" Then
                            istruttore = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            vettUfficioProp(0) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                        End If
                        'If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("REV_PROPONENTE") And istruttore = "" Then
                        '    istruttore = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                        '    vettUfficioProp(0) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                        'End If


                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("POC_ISTRUTTORE") And poc = "" Then
                            poc = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            vettUfficioProp(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIRIGENTEUP") And dirUP = "" Then
                            dirUP = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            vettUfficioProp(2) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                        End If


                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIRIGENTEGEN") Then
                            vettDirGen(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettDirGen(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettDirGen(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffGen = vettDirGen(3)
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_RAGIONERIA") Then
                            vettDirReg(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettDirReg(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettDirReg(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffRagio = vettDirReg(3)
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_CONTROLLOAMM") Then
                            vettContAmm(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettContAmm(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettContAmm(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffcontrolloAmm = vettContAmm(3)
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_CONTRLEGITT") Then
                            vettContSegLegittimita(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettContSegLegittimita(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettContSegLegittimita(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffSegreteriaLegittimita = vettContSegLegittimita(3)
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_SEGRETPRES") Then
                            vettSegretarioPresidenza(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettSegretarioPresidenza(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettSegretarioPresidenza(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffSegreteriaDiPresidenza = vettSegretarioPresidenza(3)
                        End If

                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_PRESIDENTE") Then
                            vettPresidente(1) = arr_elenco_compiti_documento(1)(1, contaElencoCompiti).ToString
                            vettPresidente(2) = DateValue(arr_elenco_compiti_documento(1)(4, contaElencoCompiti).ToString)
                            vettPresidente(3) = arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            resUffPresidenza = vettPresidente(3)
                        End If

                        
                        If arr_elenco_compiti_documento(1)(2, contaElencoCompiti).ToString.ToUpper = ("DIR_SEGRETERIA") Then
                            Dim operatore As New DllAmbiente.Operatore
                            operatore.Codice =  arr_elenco_compiti_documento(1)(0, contaElencoCompiti).ToString
                            segreteriaAssessorato = operatore.oUfficio.DescrUfficio
                        End If


                    Next
                End If

                Try

                    If flagDisattiva Then

                        Dim uffPro As New DllAmbiente.Ufficio
                        uffPro.CodUfficio = vett(2)
                        descrDipartimento = uffPro.GetDescrizioneDipartimentoPerModuli(Left(vett(1)(8), 2))
                        Dim lnewdataInizio As New DateTime(2008, 6, 1, 0, 0, 0)


                        If uffPro.CodUfficio.StartsWith("U01") Or uffPro.CodUfficio.StartsWith("U11") Then
                            If Date.Compare(CDate(vett(1)(7)), lnewdataInizio) < 0 Then
                                flagStampaUffProponente = False
                            End If
                        End If

                        'If strConfig <> "Delibere" Then
                        'se non è una delibera verifica se è stato direttamente il DG a proporre l'atto
                        If dirUP = "" Then
                            Dim dirifPoc As String = uffPro.ResponsabileUfficio(UCase(strConfig))
                            isUDD = uffPro.bUfficioDirigenzaDipartimento
                            If istruttore = dirifPoc Then
                                istruttore = ""
                                poc = ""
                            End If
                            dirUP = dirifPoc

                            If Not vettDirGen(1) Is Nothing And vettUfficioProp(2) Is Nothing Then
                                vettUfficioProp(2) = vettDirGen(1)
                            End If


                        End If

                        Dim compito As String = docDll.Definisci_Compito(idDocFromAllegato, True, ope)
                        Select Case compito.ToUpper
                            Case "REV_PROPONENTE"
                                vettUfficioProp(0) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                            Case "ISTRUTTORE"
                                vettUfficioProp(0) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                            Case "POC_ISTRUTTORE"
                                vettUfficioProp(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            Case "DIRIGENTEUP"
                                vettUfficioProp(2) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            Case "DIRIGENTEGEN"
                                resUffGen = ope.Codice
                                If isUDD Then
                                    vettUfficioProp(2) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                                End If
                            Case "DIR_RAGIONERIA"
                                resUffRagio = ope.Codice
                            Case "DIR_CONTROLLOAMM"
                                resUffcontrolloAmm = ope.Codice
                            Case "DIR_CONTRLEGITT"
                                resUffSegreteriaLegittimita = ope.Codice
                                If isUSL Then
                                    vettUfficioProp(2) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                                End If
                            Case "DIR_SEGRETPRES"
                                resUffSegreteriaDiPresidenza = ope.Codice
                            Case "DIR_PRESIDENTE"
                                resUffPresidenza = ope.Codice


                        End Select







                        Dim htOsservazioni As Collections.Generic.Dictionary(Of String, OsservazioneInfo) = objDocumento.GetOsservazioniPerDocumento(ope.Codice, idDocFromAllegato)

                        If Not vettDirGen(2) Is Nothing Then
                            Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioDirigenzaDipartimento)(1)
                            If ultimaOperazione = "RIGETTO FORMALE" Then
                                vettDirGen(4) = ultimaOperazione
                            End If
                            If String.IsNullOrEmpty(vettDirGen(4)) And LCase(ope.Codice) = LCase(resUffGen) And context.Session.Item("codAzione") = "RIGETTO" Then
                                vettDirGen(4) = "RIGETTO FORMALE"
                            End If


                        End If

                        If Not vettDirGen(2) Is Nothing Then
                            Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioControlloAmministrativo)(1)
                            If ultimaOperazione = "RIGETTO FORMALE" Then
                                vettContAmm(4) = ultimaOperazione
                            End If
                            If String.IsNullOrEmpty(vettContAmm(4)) And LCase(ope.Codice) = LCase(resUffcontrolloAmm) And context.Session.Item("codAzione") = "RIGETTO" Then
                                vettContAmm(4) = "RIGETTO FORMALE"
                            End If

                            ' Modifica 15.06.15
                            If strConfig = "Delibere" Then

                                Dim ultimaOperazione2 As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioSegreteriaPresidenzaLegittimita)(1)
                                If ultimaOperazione2 = "RIGETTO FORMALE" Then
                                    vettContSegLegittimita(4) = ultimaOperazione2
                                End If
                                If String.IsNullOrEmpty(vettContSegLegittimita(4)) And LCase(ope.Codice) = LCase(resUffSegreteriaLegittimita) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettContSegLegittimita(4) = "RIGETTO FORMALE"
                                End If
                            End If

                            ' Mi trovo nel caso in cui l'ufficio proponente ha saltato (MEV 3.2012) la dirigenza 
                            ' infatti vettDirGen è Nothing
                        ElseIf uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioControlloAmministrativo)(1)
                            'se sono la Minardi e l'ultima operazione del CA è stato un rigetto formale, 
                            'devo controllare se il documento è stato ricaricato o meno:
                            ' se NON si ricarica il provvedimento, il visto della Roberti deve rimanere "depennato"
                            ' altrimenti, se RICARICA il provv. non deve esserci la barra sul visto.
                            ' La variabile che stabilisce se mettere o meno la barra sul visto è vettContAmm(4) = "RIGETTO FORMALE"
                            If ultimaOperazione = "RIGETTO FORMALE" And (ope.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And (ope.oUfficio.CodUfficio <> uffPro.CodUfficioControlloAmministrativo)) Then
                                If (isNuovaVersioneDocumentoCaricato) Then
                                    isRigettoFormaleByCA = True
                                Else
                                    vettContAmm(4) = ultimaOperazione
                                    isRigettoFormaleByCA = True
                                End If
                            End If
                            If String.IsNullOrEmpty(vettContAmm(4)) And LCase(ope.Codice) = LCase(resUffcontrolloAmm) And context.Session.Item("codAzione") = "RIGETTO" Then
                                If ope.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And (ope.oUfficio.CodUfficio <> uffPro.CodUfficioControlloAmministrativo) Then
                                    isRigettoFormaleByCA = True
                                Else
                                    vettContAmm(4) = "RIGETTO FORMALE"
                                    isRigettoFormaleByCA = True
                                End If
                            End If
                        End If
                        'Propone controllo amministrativo e il DG non ha firmato non scrivo il visto
                        If dirUP = resUffcontrolloAmm And ((vettDirGen(1) Is Nothing) Or (LCase(ope.Codice) <> LCase(resUffcontrolloAmm))) Then
                            vettContAmm(0) = Nothing
                            vettContAmm(1) = Nothing
                            vettContAmm(2) = Nothing
                        End If

                        If Not vettDirGen(2) Is Nothing Then
                            Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioRagioneria)(1)
                            If ultimaOperazione = "RIGETTO FORMALE" Then
                                vettDirReg(4) = ultimaOperazione
                            End If
                            If String.IsNullOrEmpty(vettDirReg(4)) And LCase(ope.Codice) = LCase(resUffRagio) And context.Session.Item("codAzione") = "RIGETTO" Then
                                vettDirReg(4) = "RIGETTO FORMALE"
                            End If

                        ElseIf uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            ' Mi trovo nel caso in cui l'ufficio proponente ha saltato (MEV 3.2012) la dirigenza 
                            ' infatti vettDirGen è Nothing
                            Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioRagioneria)(1)
                            If ultimaOperazione = "RIGETTO FORMALE" Then
                                vettDirReg(4) = ultimaOperazione
                            End If
                            If String.IsNullOrEmpty(vettDirReg(4)) And LCase(ope.Codice) = LCase(resUffRagio) And context.Session.Item("codAzione") = "RIGETTO" Then
                                vettDirReg(4) = "RIGETTO FORMALE"
                            End If
                        End If

                        If strConfig = "Delibere" Then
                            ' per le delibere, se il DIR RAGIONERIA sta rigettando, devo cmq assicurarmi che ci sia la firma 
                            ' del segretario per la legittimità
                            If Not vettContSegLegittimita(2) Is Nothing Then
                                Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioRagioneria)(1)
                                If ultimaOperazione = "RIGETTO FORMALE" Then
                                    vettDirReg(4) = ultimaOperazione
                                End If
                                If String.IsNullOrEmpty(vettDirReg(4)) And LCase(ope.Codice) = LCase(resUffRagio) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettDirReg(4) = "RIGETTO FORMALE"
                                End If

                            ElseIf uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                                ' Mi trovo nel caso in cui l'ufficio proponente ha saltato (MEV 3.2012) la dirigenza 
                                ' infatti vettDirGen è Nothing
                                Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioRagioneria)(1)
                                If ultimaOperazione = "RIGETTO FORMALE" Then
                                    vettDirReg(4) = ultimaOperazione
                                End If
                                If String.IsNullOrEmpty(vettDirReg(4)) And LCase(ope.Codice) = LCase(resUffRagio) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettDirReg(4) = "RIGETTO FORMALE"
                                End If
                            End If
                            ' se il DIR SEGRETERIA DI PRESIDENZA sta rigettando, devo cmq assicurarmi che ci sia la firma 
                            ' del segretario per la legittimità (non devo controllare se c'è la frima del Dir Rag, perchè
                            ' l'atto potrebbe non essere passato dalla ragioneria essendo senza op contabili.
                            If Not vettContSegLegittimita(2) Is Nothing Then
                                Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioSegreteriaPresidenzaSegretario)(1)
                                If ultimaOperazione = "RIGETTO FORMALE" Then
                                    vettSegretarioPresidenza(4) = ultimaOperazione
                                End If
                                If String.IsNullOrEmpty(vettContSegLegittimita(4)) And LCase(ope.Codice) = LCase(resUffSegreteriaDiPresidenza) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettSegretarioPresidenza(4) = "RIGETTO FORMALE"
                                End If
                            End If


                            If Not vettContSegLegittimita(2) Is Nothing Then
                                Dim ultimaOperazione As String = Verifica_Azione_Ufficio(idDocFromAllegato, uffPro.CodUfficioPresidenza)(1)
                                If ultimaOperazione = "RIGETTO FORMALE" Then
                                    vettPresidente(4) = ultimaOperazione
                                End If
                                If String.IsNullOrEmpty(vettPresidente(4)) And LCase(ope.Codice) = LCase(resUffPresidenza) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettPresidente(4) = "RIGETTO FORMALE"
                                End If
                            End If
                        End If


                        'Propone ufficio ragione e il DG  non hanno firmato non scrivo il visto
                        If dirUP = resUffRagio And ((vettDirGen(1) Is Nothing) Or (LCase(ope.Codice) <> LCase(resUffRagio))) Then
                            vettDirReg(0) = Nothing
                            vettDirReg(1) = Nothing
                            vettDirReg(2) = Nothing
                        End If


                        If Not vettDirReg(2) Is Nothing _
                        And Not vettDirGen(2) Is Nothing And Not vettContAmm(2) Is Nothing And
                        Not vettUfficioProp(0) Is Nothing And Not vettUfficioProp(1) Is Nothing And
                        Not vettUfficioProp(2) Is Nothing Then

                        End If

                        If LCase(resUffSegreteriaLegittimita) = LCase(ope.Codice) Then
                            vettContSegLegittimita(0) = Nothing
                            vettContSegLegittimita(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettContSegLegittimita(2) = Now.Date
                        ElseIf LCase(resUffSegreteriaLegittimita) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            vettContSegLegittimita(0) = Nothing
                            vettContSegLegittimita(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettContSegLegittimita(2) = Now.Date
                        End If

                        If LCase(resUffSegreteriaDiPresidenza) = LCase(ope.Codice) Then
                            vettSegretarioPresidenza(0) = Nothing
                            vettSegretarioPresidenza(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettSegretarioPresidenza(2) = Now.Date
                        ElseIf LCase(resUffSegreteriaDiPresidenza) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            vettSegretarioPresidenza(0) = Nothing
                            vettSegretarioPresidenza(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettSegretarioPresidenza(2) = Now.Date
                        End If

                        If LCase(resUffPresidenza) = LCase(ope.Codice) Then
                            vettPresidente(0) = Nothing
                            vettPresidente(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettPresidente(2) = Now.Date
                        ElseIf LCase(resUffPresidenza) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            vettPresidente(0) = Nothing
                            vettPresidente(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettPresidente(2) = Now.Date
                        End If

                        If LCase(resUffcontrolloAmm) = LCase(ope.Codice) And (Not vettDirGen(1) Is Nothing) Then
                            vettContAmm(0) = Nothing
                            vettContAmm(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettContAmm(2) = Now.Date
                        ElseIf LCase(resUffcontrolloAmm) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            vettContAmm(0) = Nothing
                            vettContAmm(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettContAmm(2) = Now.Date
                        End If


                        If LCase(resUffGen) = LCase(ope.Codice) Then
                            vettDirGen(0) = Nothing
                            vettDirGen(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) 'ope.Cognome & " " & ope.Nome
                            vettDirGen(2) = Now.Date
                        End If
                        If strConfig = "Delibere" Then
                            If Not vettDirGen(2) Is Nothing Then
                                If String.IsNullOrEmpty(vettDirGen(4)) And LCase(ope.Codice) = LCase(resUffGen) And context.Session.Item("codAzione") = "RIGETTO" Then
                                    vettDirGen(4) = "RIGETTO FORMALE"
                                End If
                            End If
                        End If


                        If LCase(resUffRagio) = LCase(ope.Codice) And (Not vettDirGen(1) Is Nothing) Then
                            vettDirReg(0) = Nothing
                            vettDirReg(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                            vettDirReg(2) = Now.Date
                        ElseIf LCase(resUffRagio) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                            vettDirReg(0) = Nothing
                            vettDirReg(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                            vettDirReg(2) = Now.Date
                        End If

                        If strConfig = "Delibere" Then
                            If LCase(resUffRagio) = LCase(ope.Codice) And (Not vettContSegLegittimita(1) Is Nothing) Then
                                vettDirReg(0) = Nothing
                                vettDirReg(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                                vettDirReg(2) = Now.Date
                            ElseIf LCase(resUffRagio) = LCase(ope.Codice) And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                                vettDirReg(0) = Nothing
                                vettDirReg(1) = Crea_NomeCognomeFirma(ope.Cognome, ope.Nome) ' ope.Cognome & " " & ope.Nome
                                vettDirReg(2) = Now.Date
                            End If
                        End If

                        ' MEV 2.2012 
                        ' Nel caso in cui l'ufficio autorità di gestione decida di saltare la dirigenza generale, viene barrato 
                        ' il box del dg ed aggiunta una dicitura "Autorità di Gestione PO FESR 2007-2013"
                        ' che è una stringa presa dal db, nella tabella Struttura_Attributi codificata con Sta_Attributo = TESTO_NO_FIRMA_DG
                        Dim destinatarioInoltro = context.Session.Item("destinatarioInoltro")
                        If ope.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) _
                            And destinatarioInoltro > 0 And Not (vettContAmm(4) = "RIGETTO FORMALE") Then
                            vettDirGen(0) = Nothing
                            vettDirGen(1) = uffPro.Attributo("TESTO_NO_FIRMA_DG")
                            vettDirGen(2) = Nothing
                            barraDirettoreGenerale = True
                            ' esclude ufficio proponente e aggiunge la barra nei casi in cui il dirigente generale non ha firmato
                        ElseIf ope.oUfficio.CodUfficio <> uffPro.CodUfficio And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) _
                                And vettDirGen(1) Is Nothing Then
                            vettDirGen(1) = uffPro.Attributo("TESTO_NO_FIRMA_DG")
                            vettDirGen(2) = Nothing
                            barraDirettoreGenerale = True
                            'prendo il caso in cui l'atto ha subito un rigetto dal CA e la Minardi
                            ' riinoltra scegliendo DG o CA, tanto cmq sia andrà direttamente in ARCHIVIO
                        ElseIf destinatarioInoltro > -1 _
                            And ope.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) _
                            And vettDirGen(1) Is Nothing And (isRigettoFormaleByCA) Then
                            ' in questo Else ci entro: se c'è stato  un rigetto formale da parte del CA e se l'atto non era passato
                            ' dal DG, quindi nel primo passaggio (prima del rigetto) al posto della firma del DG era stato apposto
                            ' il testo 'Autorità di Gestione' invece della firma del DG. Ora al secondo passaggio, a prescindere
                            ' dalla selezione che la Minardi ha fatto (DG o CA) ci metto cmq il testo e mando in archivio!!


                            ' nel momento il cui la Minardi riinoltra il documento, può decidere di forzare l'inoltro del doc così come
                            ' l'aveva creato, oppure di ricaricare la determina. Se ricarica la determina questo perderà tutte le firme
                            ' E' necessario quindi verificare di essere nel secondo caso
                            If Not (isNuovaVersioneDocumentoCaricato) Then
                                vettDirGen(1) = uffPro.Attributo("TESTO_NO_FIRMA_DG")
                                vettDirGen(2) = Nothing
                                barraDirettoreGenerale = True
                            Else
                                ' qui ci arrivo se dopo il rigetto formale subito, ricarico la determina.
                                ' si deve tener conto della scelta dalla combo della Minardi: DG (non scrivo nulla) - CA (barroDG e scrivo "Autorià di getione")
                                If (destinatarioInoltro > 0) Then
                                    vettDirGen(1) = uffPro.Attributo("TESTO_NO_FIRMA_DG")
                                    vettDirGen(2) = Nothing
                                    barraDirettoreGenerale = True
                                End If
                            End If


                        End If


                        If htOsservazioni.Count > 0 Then
                            If vettDirGen(0) Is Nothing Then
                                If htOsservazioni.ContainsKey("UDD") Then
                                    vettDirGen(0) = "" & htOsservazioni("UDD").Testo
                                End If


                            End If
                            If vettContAmm(0) Is Nothing Then
                                If htOsservazioni.ContainsKey("UCA") Then
                                    vettContAmm(0) = "" & htOsservazioni("UCA").Testo
                                End If
                            End If
                            If htOsservazioni.ContainsKey("UP") Then
                                vettUfficioProp(3) = "" & htOsservazioni("UP").Testo
                            End If

                            If vettDirReg(0) Is Nothing Then
                                If htOsservazioni.ContainsKey("UR") Then
                                    vettDirReg(0) = "" & htOsservazioni("UR").Testo
                                End If

                            End If

                        End If

                    End If

                    infoDatiSeduta = objDocumento.FO_Get_DatiSedutaRelatoreInfo(idDocFromAllegato)


                    If strConfig <> "Delibere" Then
                        If (CStr(vett(1)(3)).IndexOf("/") <> -1) And Not CStr(vett(1)(3)).StartsWith("P" & Now.Year) And Not CStr(vett(1)(3)).StartsWith("P" & Now.Year - 1) Then ' Or CStr(vett(1)(3)).IndexOf("D") <> -1) Then
                            stampaNumerodata = True
                            numdoc = vett(1)(3)
                            datadoc = vett(1)(7)

                        Dim uffPro As New DllAmbiente.Ufficio
                        uffPro.CodUfficio = vett(2)

                        If Not vettDirGen(1) Is Nothing Then
                            If datadoc = "" Then
                                datadoc = vettDirGen(2)
                            End If

                            Else
                                If Not uffPro.Test_Attributo("SCEGLI_DEST_INOLTRO", True) Then
                                    vettDirReg(0) = Nothing
                                    vettDirReg(1) = Nothing
                                    vettDirReg(2) = Nothing
                                End If
                            End If
                        End If
                    Else
                        If Not CStr(vett(1)(3)).StartsWith("P" & Now.Year) And Not CStr(vett(1)(3)).StartsWith("P" & Now.Year - 1) Then ' Or CStr(vett(1)(3)).IndexOf("D") <> -1) Then
                            stampaNumerodata = True
                            numdoc = vett(1)(3)
                            If Not infoDatiSeduta Is Nothing Then
                                datadoc = infoDatiSeduta.DataSeduta
                            End If

                        End If
                End If

                Catch ex As Exception
                    Log.Error(ex.Message)
                End Try

                Dim memStream As MemoryStream


                memStream = InserisciValoreInPDF_new_Model(vettoredati(1), strConfig, Ufficio, codufficio, descrDipartimento, stampaNumerodata, numdoc, datadoc, flagDisattiva, vettDirGen, vettDirReg, vettContAmm, vettContSegLegittimita, vettSegretarioPresidenza, vettPresidente, vettUfficioProp, flagStampaUffProponente, isUDD, lstr_Oggetto, lstr_TipoPubblicazione, flagIsPrew, barraDirettoreGenerale, dataCreazioneDocumento,infoDatiSeduta,segreteriaAssessorato)
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
    Function Crea_NomeCognomeFirma(Optional ByVal cognome As String = "", Optional ByVal nome As String = "") As String
        Return nome & " " & cognome

    End Function
    Private Function InserisciValoreInPDF_new_Model(ByVal pdfByte() As Byte, ByVal strConfigTipoDocumento As String, ByVal Ufficio As String, ByVal codufficio As String, ByVal descDipartimento As String, ByVal stampaNumerodata As Boolean, ByVal numdoc As String, ByVal datadoc As String, ByVal flagdisattiva As Boolean, ByVal vettDirGen() As Object, ByVal vettDirReg() As Object, ByVal vettContAmm() As Object, ByVal vettContSegLegittimmita() As Object, ByVal vettSegretarioDiPresidenza() As Object, ByVal vettPresidenza() As Object,  ByVal vettUfficioProp() As Object, ByVal flagStampaUffProponente As Boolean, ByVal isUDD As Boolean, Optional ByVal lstr_Oggetto As String = "", Optional ByVal lstr_TipoPubblicazione As String = "", Optional ByVal flagIsPrew As Boolean = False, Optional ByVal barraDirettoreGenerale As Boolean = False, Optional ByVal dataCreazioneDocumento As Date = Nothing,Optional ByVal itemDatiSedutaInfo As DllDocumentale.ItemDatiSedutaInfo = Nothing,Optional ByVal segreteriaAssessorato As String = "") As MemoryStream
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

            Dim xUfficio As Single = 355
            Dim yUfficio As Single = 750
            Dim xNumero As Single = 365
            Dim yNumero As Single = 682

            Dim xData As Single = 365
            Dim yData As Single = 682

            Dim xDirigenteOsservazioni As Single = 290
            Dim yDirigenteOsservazioni As Single = 68
            Dim xDirigenteFirma As Single = 290
            Dim yDirigenteFirma As Single = 68
            Dim xDataFirma As Single = 455
            Dim yDataFirma As Single = 68

            Dim xDirigControLegittFirma As Single = 290
            Dim yDirigControLegittFirma As Single = 68
            Dim xDataFirmaDirigControLegitt As Single = 455
            Dim yDataFirmaDirigControLegitt As Single = 68

            Dim xIstruttoreFirma As Single = 290
            Dim yIstruttoreFirma As Single = 68
            Dim xPocFirma As Single = 290
            Dim yPocFirma As Single = 68
            Dim xDirigenteUPFirma As Single = 455
            Dim yDirigenteUPFirma As Single = 68

            Dim xOggetto As Single = 290
            Dim yOggetto As Single = 68
            Dim xIntegrale As Single = 290
            Dim yIntegrale As Single = 68
            Dim xEstratto As Single = 455
            Dim yEstratto As Single = 68
            Dim xDipartimento As Single = 130 'daintrodurre
            Dim yDipartimento As Single = 740 'daintrodurre
            Dim xCup As Single = 355
            Dim yCup As Single = 650
            Dim xNAllegati As Single = 70 'daintrodurre
            Dim yNAllegati As Single = 70 'daintrodurre

            Dim xNPagTotAllegati As Single = 102 
            Dim xSistemazioni As Single = 155
            Dim ySistemazioni As Single = 510
            Dim descrizioneSistemazioni As String = ""
            Dim testoSistemazioni As String = ""


            Dim annoCambioModello As Integer = ConfigurationManager.AppSettings("annoCambioModello")
            Dim meseCambioModello As Integer = ConfigurationManager.AppSettings("meseCambioModello")
            Dim giornoCambioModello As Integer = ConfigurationManager.AppSettings("giornoCambioModello")
            Dim costanteVersioneModello As String = ConfigurationManager.AppSettings("costanteVersioneModello")

            Dim dataCambioModello As New Date(annoCambioModello, meseCambioModello, giornoCambioModello)
	    
	    
            'Dati Seduta Delibere
            Dim yDataSeduta As Single = 1
            Dim xDataSeduta As Single = 1
            Dim yDatiSedutaDataEOra As Single = 1
            Dim xDatiSedutaOra As Single = 1
            Dim xDatiSedutaData As Single = 1
            'Dati Relatori Delibere
            Dim yRigaRelatore As Single = 1
            Dim xDenominazioneRelatore As Single = 1
            Dim xCaricaRelatore As Single = 1
            Dim xPresenzaRelatore As Single = 1
            Dim xAssenzaRelatore As Single = 1

            'Dati Denominazione Segretario Delibere
            Dim yDenominazioneSegretario As Single = 1
            Dim xDenominazioneSegretario As Single = 1

            'Dati Denominazione Segretario Delibere
            Dim ySegreteriaAssessoratoDelibere As Single = 1
            Dim xSegreteriaAssessoratoDelibere As Single = 1
	    
            Try
                xUfficio = IIf(ConfigurationManager.AppSettings("xUfficio" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xUfficio" & strConfigTipoDocumento) & "", xUfficio)
                yUfficio = IIf(ConfigurationManager.AppSettings("yUfficio" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yUfficio" & strConfigTipoDocumento) & "", yUfficio)

                xNumero = IIf(ConfigurationManager.AppSettings("xNumero" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xNumero" & strConfigTipoDocumento) & "", xNumero)
                If (dataCreazioneDocumento <> Nothing) Then
                    If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                        yNumero = IIf(ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento & costanteVersioneModello) & "" <> "", ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento & costanteVersioneModello) & "", yNumero)
                    Else
                        yNumero = IIf(ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento) & "", yNumero)
                    End If
                End If

                xData = IIf(ConfigurationManager.AppSettings("xData" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xData" & strConfigTipoDocumento) & "", xData)
                If (dataCreazioneDocumento <> Nothing) Then
                    If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                        yData = IIf(ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento & costanteVersioneModello) & "" <> "", ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento & costanteVersioneModello) & "", yData)
                    Else
                        yData = IIf(ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento) & "", yData)
                    End If
                End If


                xDipartimento = IIf(ConfigurationManager.AppSettings("xDipartimento" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDipartimento" & strConfigTipoDocumento) & "", xDipartimento)
                yDipartimento = IIf(ConfigurationManager.AppSettings("yDipartimento" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yDipartimento" & strConfigTipoDocumento) & "", yDipartimento)
                'xCup = IIf(ConfigurationManager.AppSettings("xCup") & "" <> "", ConfigurationManager.AppSettings("xCup") & "", xCup)
                'yCup = IIf(ConfigurationManager.AppSettings("yCup") & "" <> "", ConfigurationManager.AppSettings("yCup") & "", yCup)

                xCup = IIf(ConfigurationManager.AppSettings("xCup" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xCup" & strConfigTipoDocumento) & "", xCup)
                If (dataCreazioneDocumento <> Nothing) Then
                    If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                        yCup = IIf(ConfigurationManager.AppSettings("yCup" & strConfigTipoDocumento & costanteVersioneModello) & "" <> "", ConfigurationManager.AppSettings("yCup" & strConfigTipoDocumento & costanteVersioneModello) & "", yCup)
                    Else
                        yCup = IIf(ConfigurationManager.AppSettings("yCup" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yCup" & strConfigTipoDocumento) & "", yCup)
                    End If
                End If



                xNAllegati = IIf(ConfigurationManager.AppSettings("xNAllegati" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xNAllegati" & strConfigTipoDocumento) & "", xNAllegati)
                yNAllegati = IIf(ConfigurationManager.AppSettings("yNAllegati" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yNAllegati" & strConfigTipoDocumento) & "", yNAllegati)
                xSistemazioni = IIf(ConfigurationManager.AppSettings("xSistemazioni" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xSistemazioni" & strConfigTipoDocumento) & "", xSistemazioni)
                ySistemazioni = IIf(ConfigurationManager.AppSettings("ySistemazioni" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("ySistemazioni" & strConfigTipoDocumento) & "", ySistemazioni)
                descrizioneSistemazioni = IIf(ConfigurationManager.AppSettings("descrizioneSistemazioni") & "" <> "", ConfigurationManager.AppSettings("descrizioneSistemazioni") & "", descrizioneSistemazioni)

                yDataSeduta = IIf(ConfigurationManager.AppSettings("yDataSeduta" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yDataSeduta" & strConfigTipoDocumento) & "", yDataSeduta)
                xDataSeduta = IIf(ConfigurationManager.AppSettings("xDataSeduta" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDataSeduta" & strConfigTipoDocumento) & "", xDataSeduta)
                yDatiSedutaDataEOra = IIf(ConfigurationManager.AppSettings("yDatiSedutaDataEOra" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yDatiSedutaDataEOra" & strConfigTipoDocumento) & "", yDatiSedutaDataEOra)
                xDatiSedutaOra = IIf(ConfigurationManager.AppSettings("xDatiSedutaOra" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDatiSedutaOra" & strConfigTipoDocumento) & "", xDatiSedutaOra)
                xDatiSedutaData = IIf(ConfigurationManager.AppSettings("xDatiSedutaData" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDatiSedutaData" & strConfigTipoDocumento) & "", xDatiSedutaData)
                yRigaRelatore = IIf(ConfigurationManager.AppSettings("yRigaRelatore" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yRigaRelatore" & strConfigTipoDocumento) & "", yRigaRelatore)
                xDenominazioneRelatore = IIf(ConfigurationManager.AppSettings("xDenominazioneRelatore" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDenominazioneRelatore" & strConfigTipoDocumento) & "", xDenominazioneRelatore)
                xCaricaRelatore = IIf(ConfigurationManager.AppSettings("xCaricaRelatore" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xCaricaRelatore" & strConfigTipoDocumento) & "", xDenominazioneRelatore)
                xPresenzaRelatore = IIf(ConfigurationManager.AppSettings("xPresenzaRelatore" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xPresenzaRelatore" & strConfigTipoDocumento) & "", xPresenzaRelatore)
                xAssenzaRelatore = IIf(ConfigurationManager.AppSettings("xAssenzaRelatore" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xAssenzaRelatore" & strConfigTipoDocumento) & "", xAssenzaRelatore)

                yDenominazioneSegretario = IIf(ConfigurationManager.AppSettings("yDenominazioneSegretario" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("yDenominazioneSegretario" & strConfigTipoDocumento) & "", yDenominazioneSegretario)
                xDenominazioneSegretario = IIf(ConfigurationManager.AppSettings("xDenominazioneSegretario" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xDenominazioneSegretario" & strConfigTipoDocumento) & "", xDenominazioneSegretario)

                ySegreteriaAssessoratoDelibere = IIf(ConfigurationManager.AppSettings("ySegreteriaAssessorato" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("ySegreteriaAssessorato" & strConfigTipoDocumento) & "", ySegreteriaAssessoratoDelibere)
                xSegreteriaAssessoratoDelibere = IIf(ConfigurationManager.AppSettings("xSegreteriaAssessorato" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xSegreteriaAssessorato" & strConfigTipoDocumento) & "", xSegreteriaAssessoratoDelibere)

            Catch ex As Exception
                Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfigTipoDocumento)
            End Try
            Dim bfTimes As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, False)
            Dim ft As iTextSharp.text.Font = New Font(bfTimes, 7, Font.BOLD)
            Dim ftDip As iTextSharp.text.Font = New Font(bfTimes, 9, Font.BOLD)
            Dim ftFirme As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)
            Dim ftBarra As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial Black", 10, iTextSharp.text.Font.BOLD)


            Dim ftUfficio As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)
            Dim ftCodiceUfficio As iTextSharp.text.Font = New Font(bfTimes, 10, iTextSharp.text.Font.BOLD)
            Dim ftSistemazioni As iTextSharp.text.Font = New Font(bfTimes, 11, iTextSharp.text.Font.BOLD)


            Dim i As Integer = 0
            Dim objDocLib As New DllDocumentale.svrDocumenti(HttpContext.Current.Session("oOperatore"))
            Dim idPerCont As String = idDocFromAllegato

            Dim objdoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(idDocFromAllegato)
            Dim str_sistemazioni As String = objdoc.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
            Dim itemSistemazione As DllDocumentale.ItemTipoBase = objDocLib.DettaglioOperazioneRettifica(str_sistemazioni, objdoc.Doc_Tipo)
            If Not itemSistemazione Is Nothing Then
                testoSistemazioni = "" & itemSistemazione.Descrizione & " " & descrizioneSistemazioni
            End If
            Dim DocAttributo As New DllDocumentale.Documento_attributo
            DocAttributo.Doc_id = idPerCont
            DocAttributo.Cod_attributo = "CUP"
            Dim lista As Generic.List(Of DllDocumentale.Documento_attributo) = objDocLib.FO_Get_Documento_Attributi(DocAttributo)
            Dim valoreCUP As String = ""
            If lista.Count > 0 Then
                valoreCUP = lista.Item(0).Valore
            End If


            Dim listaLiq As IList = objDocLib.FO_Get_DatiLiquidazione(idPerCont)
            Dim listaPreImpegni As IList = objDocLib.FO_Get_DatiPreImpegni(idPerCont)
            Dim listaBil As IList = objDocLib.FO_Get_DatiImpegni(idPerCont)
            Dim listaVarazioni As IList = objDocLib.FO_Get_DatiImpegniVariazioni(idPerCont)
            Dim listaVarazioniPreimp As IList = objDocLib.FO_Get_DatiPreImpegniVariazioni(idPerCont)
            Dim listaVarazioniLiq As IList = objDocLib.FO_Get_DatiLiquidazioniVariazioni(idPerCont)


            Dim listaAccertamenti As IList = objDocLib.FO_Get_Dati_Assunzione(idPerCont)
            Dim NumAllegati As Integer = objDocLib.Conta_allegatiPerDocumento(idPerCont)


            Dim listaRagLiq As IList = New ArrayList '= objDocLib.FO_Get_DatiRagLiquidazione(idPerCont)
            Dim listaRagImpegni As IList = New ArrayList '= objDocLib.FO_Get_DatiRagAssunzioni(idPerCont)
            Dim listaRagPreImpegni As IList = New ArrayList '= objDocLib.FO_Get_DatiRagAssunzioni(idPerCont)
            Dim listaRagRiduzioni As IList = New ArrayList

            Dim flagStampaNCont As Boolean = True
            If Not flagIsPrew Then
                If "" & vettDirReg(1) = "" And vettDirReg(4) = "RIGETTO FORMALE" Then
                    flagStampaNCont = False
                End If
            End If

            DirectCast(listaRagLiq, ArrayList).AddRange(listaLiq)
            DirectCast(listaRagPreImpegni, ArrayList).AddRange(listaPreImpegni)
            DirectCast(listaRagImpegni, ArrayList).AddRange(listaBil)
            DirectCast(listaRagRiduzioni, ArrayList).AddRange(listaVarazioni)

            DirectCast(listaRagRiduzioni, ArrayList).AddRange(listaVarazioniPreimp)
            DirectCast(listaRagRiduzioni, ArrayList).AddRange(listaVarazioniLiq)

            Dim flagAttachAppendice As Boolean = False
            If strConfigTipoDocumento = "Determine" Then
                If (listaRagPreImpegni.Count > 3 Or listaRagLiq.Count > 3 Or listaRagImpegni.Count > 3 Or listaRagRiduzioni.Count > 3) Then
                    flagAttachAppendice = True
                End If

            End If

            If strConfigTipoDocumento = "Disposizioni" Then
                If (listaLiq.Count > 3 Or listaRagLiq.Count > 3) Then
                    flagAttachAppendice = True
                End If

            End If

            If strConfigTipoDocumento = "Delibere" Then
                If (listaRagPreImpegni.Count > 3 Or listaRagImpegni.Count > 3) Then
                    flagAttachAppendice = True
                End If

            End If

            Dim flagAttachNoteUP As Boolean = False
            If Not String.IsNullOrEmpty(vettUfficioProp(3)) Then
                flagAttachNoteUP = True
            End If


            ' Perchè le note ufficio dirigente generale si trovano all'indice 0 del vettore 
            ' e non all'indice 3 come del caso di vettUfficioProp?  
            If isUDD And Not String.IsNullOrEmpty(vettDirGen(0)) Then
                flagAttachNoteUP = True
            End If

            While i < n
                '''''''''''''
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
                    'NELLA PRIMA PAGINA VA INSERITO:

                    'DESCRIZIONE DIPARTIMENTO
                    If strConfigTipoDocumento <> "Delibere" Then
                        Dim TabellaDipartimento As PdfPTable = New PdfPTable(1)
                        TabellaDipartimento.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaDipartimento.AddCell(New Phrase(descDipartimento, ftDip))
                        TabellaDipartimento.TotalWidth = 170
                        TabellaDipartimento.WriteSelectedRows(0, -1, xDipartimento, yDipartimento, writer.DirectContent)
                    End If

                    If strConfigTipoDocumento = "Delibere" Then
                        If Not String.IsNullOrEmpty(segreteriaAssessorato) Then
                            Dim TabellaSegreteriaAssessoratoDelibere As PdfPTable = New PdfPTable(1)
                            TabellaSegreteriaAssessoratoDelibere.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaSegreteriaAssessoratoDelibere.AddCell(New Phrase(segreteriaAssessorato, ftDip))
                            TabellaSegreteriaAssessoratoDelibere.TotalWidth = 510
                            TabellaSegreteriaAssessoratoDelibere.WriteSelectedRows(0, -1, xSegreteriaAssessoratoDelibere, ySegreteriaAssessoratoDelibere, writer.DirectContent)
                        End If

                        If Not itemDatiSedutaInfo Is Nothing Then
                            Dim TabellaDataSeduta As PdfPTable = New PdfPTable(1)
                            TabellaDataSeduta.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaDataSeduta.AddCell(New Phrase(itemDatiSedutaInfo.DataSeduta, ftDip))
                            TabellaDataSeduta.TotalWidth = 170
                            TabellaDataSeduta.WriteSelectedRows(0, -1, xDataSeduta, yDataSeduta, writer.DirectContent)

                            Dim TabellaDatiSedutaData As PdfPTable = New PdfPTable(1)
                            TabellaDatiSedutaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaDatiSedutaData.AddCell(New Phrase(itemDatiSedutaInfo.DataSeduta, ftDip))
                            TabellaDatiSedutaData.TotalWidth = 170
                            TabellaDatiSedutaData.WriteSelectedRows(0, -1, xDatiSedutaData, yDatiSedutaDataEOra, writer.DirectContent)

                            Dim TabellaDatiSedutaOra As PdfPTable = New PdfPTable(1)
                            TabellaDatiSedutaOra.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaDatiSedutaOra.AddCell(New Phrase(itemDatiSedutaInfo.OraSeduta, ftDip))
                            TabellaDatiSedutaOra.TotalWidth = 170
                            TabellaDatiSedutaOra.WriteSelectedRows(0, -1, xDatiSedutaOra, yDatiSedutaDataEOra, writer.DirectContent)

                            For Each relatore As DllDocumentale.ItemRelatore In itemDatiSedutaInfo.Relatori
                                Dim TabellaDenominazioneRelatori As PdfPTable = New PdfPTable(1)
                                TabellaDenominazioneRelatori.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaDenominazioneRelatori.AddCell(New Phrase(relatore.Cognome & " " & relatore.Nome, ftDip))
                                TabellaDenominazioneRelatori.TotalWidth = 170
                                TabellaDenominazioneRelatori.WriteSelectedRows(0, -1, xDenominazioneRelatore, yRigaRelatore, writer.DirectContent)

                                Dim TabellaCaricaRelatori As PdfPTable = New PdfPTable(1)
                                TabellaCaricaRelatori.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaCaricaRelatori.AddCell(New Phrase(relatore.Carica, ftDip))
                                TabellaCaricaRelatori.TotalWidth = 170
                                TabellaCaricaRelatori.WriteSelectedRows(0, -1, xCaricaRelatore, yRigaRelatore, writer.DirectContent)

                                Dim TabellaPresAssRelatori As PdfPTable = New PdfPTable(1)
                                TabellaPresAssRelatori.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaPresAssRelatori.AddCell(New Phrase("X", ftDip))
                                TabellaPresAssRelatori.TotalWidth = 170
                                If relatore.IsPresente Then
                                    TabellaPresAssRelatori.WriteSelectedRows(0, -1, xPresenzaRelatore, yRigaRelatore, writer.DirectContent)
                                Else
                                    TabellaPresAssRelatori.WriteSelectedRows(0, -1, xAssenzaRelatore, yRigaRelatore, writer.DirectContent)
                                End If

                                yRigaRelatore = yRigaRelatore - 17
                            Next

                            Dim TabellaDenominazioneSegretario As PdfPTable = New PdfPTable(1)
                            TabellaDenominazioneSegretario.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaDenominazioneSegretario.AddCell(New Phrase(vettSegretarioDiPresidenza(1), ftDip))
                            TabellaDenominazioneSegretario.TotalWidth = 170
                            TabellaDenominazioneSegretario.WriteSelectedRows(0, -1, xDenominazioneSegretario, yDenominazioneSegretario, writer.DirectContent)


                            
                        End If
                    End If


                    'UFFICIO PROPONENTE
                    Dim TabellaUfficio As PdfPTable = New PdfPTable(1)
                    TabellaUfficio.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                    If Ufficio.Length > 120 Then
                        ftUfficio.Size = 8
                    End If
                    TabellaUfficio.AddCell(New Phrase("" & Ufficio, ftUfficio))
                    TabellaUfficio.AddCell(New Phrase("" & codufficio, ftCodiceUfficio))
                    TabellaUfficio.TotalWidth = 200
                    TabellaUfficio.WriteSelectedRows(0, -1, xUfficio, yUfficio, writer.DirectContent)

                    'CODICE CUP
                    If strConfigTipoDocumento <> "Delibere" Then
                        Dim TabellaCUP As PdfPTable = New PdfPTable(1)
                        TabellaCUP.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaCUP.AddCell(New Phrase("" & valoreCUP, ftUfficio))
                        TabellaCUP.TotalWidth = 200
                        TabellaCUP.WriteSelectedRows(0, -1, xCup, yCup, writer.DirectContent)
                    End If

                    'DATA E NUMERO
                    If stampaNumerodata Then
                        Dim TabellaNumero As PdfPTable = New PdfPTable(1)
                        TabellaNumero.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaNumero.AddCell(numdoc)
                        TabellaNumero.TotalWidth = 225
                        TabellaNumero.WriteSelectedRows(0, -1, xNumero, yNumero, writer.DirectContent)

                        If strConfigTipoDocumento <> "Delibere" Then
                            Dim TabellaData As PdfPTable = New PdfPTable(1)
                            TabellaData.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            If Not String.IsNullOrEmpty(datadoc) Then
                                Dim data As DateTime = DateTime.Parse(datadoc)
                                datadoc = data.Day & "/" & data.Month & "/" & data.Year
                            End If
                            TabellaData.AddCell(datadoc)
                            TabellaData.TotalWidth = 225
                            TabellaData.WriteSelectedRows(0, -1, xData, yData, writer.DirectContent)
                        End If
                    End If

                    'RETTIFICHE CONTABILI
                    If strConfigTipoDocumento <> "Delibere" Then
                        Dim Tabellasistemazioni As PdfPTable = New PdfPTable(1)
                        Tabellasistemazioni.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        Tabellasistemazioni.AddCell(New Phrase("" & testoSistemazioni, ftSistemazioni))
                        Tabellasistemazioni.TotalWidth = 450
                        Tabellasistemazioni.WriteSelectedRows(0, -1, xSistemazioni, ySistemazioni, writer.DirectContent)
                    End If

                    If strConfigTipoDocumento = "Determine" Then
                        addInfoRagAssLiquidazionePrmaPaginaDetermina_new_model(writer, listaRagPreImpegni, listaRagImpegni, listaRagLiq, listaRagRiduzioni, listaAccertamenti, strConfigTipoDocumento, flagStampaNCont, dataCreazioneDocumento, dataCambioModello, costanteVersioneModello)
                    End If

                    If strConfigTipoDocumento = "Disposizioni" Then
                        addInfoAssLiquidazionePrmaPaginaDisposizione_new_model(writer, listaRagLiq, listaAccertamenti, strConfigTipoDocumento, flagStampaNCont)
                    End If

                    If strConfigTipoDocumento = "Delibere" Then
                        addInfoContabiliPrimaPaginaDelibera_new_model(writer, listaRagPreImpegni, listaRagImpegni, strConfigTipoDocumento, flagStampaNCont)
                    End If

                    'NUMERO ALLEGATI
                    If NumAllegati > 0 Then
                        Dim TabellaNumAllegati As PdfPTable = New PdfPTable(1)
                        TabellaNumAllegati.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        TabellaNumAllegati.AddCell(New Phrase(NumAllegati, ftDip))
                        TabellaNumAllegati.TotalWidth = 100
                        TabellaNumAllegati.WriteSelectedRows(0, -1, xNAllegati, yNAllegati, writer.DirectContent)
                    End If

                    If strConfigTipoDocumento = "Delibere" Then
                        xNPagTotAllegati = IIf(ConfigurationManager.AppSettings("xNPagTotAllegati" & strConfigTipoDocumento) & "" <> "", ConfigurationManager.AppSettings("xNPagTotAllegati" & strConfigTipoDocumento) & "", xNPagTotAllegati)

                        Dim itemRicercato As New DllDocumentale.Documento_attributo
                        itemRicercato.Doc_id = idDocFromAllegato
                        itemRicercato.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
                        itemRicercato.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                        Dim listaAttributi As Generic.List(Of DllDocumentale.Documento_attributo) = objDocLib.FO_Get_Documento_Attributi(itemRicercato)
                        If Not listaAttributi Is Nothing AndAlso listaAttributi.Count > 0 Then
                            Dim nTotPagineAllegati As Integer
                            For Each attributo As DllDocumentale.Documento_attributo In listaAttributi
                                Integer.TryParse(attributo.Valore, nTotPagineAllegati)
                            Next
                            If nTotPagineAllegati <> 0 Then
                                Dim TabellaNumPagTotAllegati As PdfPTable = New PdfPTable(1)
                                TabellaNumPagTotAllegati.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaNumPagTotAllegati.AddCell(New Phrase(nTotPagineAllegati.ToString, ftDip))
                                TabellaNumPagTotAllegati.TotalWidth = 100
                                TabellaNumPagTotAllegati.WriteSelectedRows(0, -1, xNPagTotAllegati, yNAllegati, writer.DirectContent)
                            End If
                        End If
                    End If



                    If flagdisattiva Then
                        'OSSERVAZIONI RAGIONERIA
                        If Not vettDirReg(0) Is Nothing Then
                            xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteRagOsservazioni" & strConfigTipoDocumento)
                            yDirigenteOsservazioni = ConfigurationManager.AppSettings("yDirigenteRagOsservazioni" & strConfigTipoDocumento)
                            Dim TabellaOssDir As PdfPTable = New PdfPTable(1)
                            TabellaOssDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            Dim OssDir As Phrase = New Phrase("" & vettDirReg(0), ft)
                            TabellaOssDir.TotalWidth = 500
                            TabellaOssDir.AddCell(OssDir)
                            TabellaOssDir.WriteSelectedRows(0, -1, xDirigenteOsservazioni, yDirigenteOsservazioni, writer.DirectContent)

                        End If
                    End If

                    'FIRMA RAGIONERIA
                    If Not vettDirReg(1) Is Nothing Then

                        xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteRagFirma" & strConfigTipoDocumento)
                        yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteRagFirma" & strConfigTipoDocumento)
                        xDataFirma = ConfigurationManager.AppSettings("xDataFirmaRag" & strConfigTipoDocumento)
                        yDataFirma = ConfigurationManager.AppSettings("yDataFirmaRag" & strConfigTipoDocumento)
                        Dim TabellaFirmaDir As PdfPTable = New PdfPTable(1)
                        TabellaFirmaDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        Dim FirmaDir As Phrase = New Phrase(vettDirReg(1), ftFirme)
                        TabellaFirmaDir.TotalWidth = 200
                        TabellaFirmaDir.AddCell(FirmaDir)
                        TabellaFirmaDir.WriteSelectedRows(0, -1, xDirigenteFirma, yDirigenteFirma, writer.DirectContent)

                        Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                        TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                        Dim dataFirma As Phrase = New Phrase(vettDirReg(2), ftFirme)
                        TabellaDataFirma.TotalWidth = 200
                        TabellaDataFirma.AddCell(dataFirma)
                        TabellaDataFirma.WriteSelectedRows(0, -1, xDataFirma, yDataFirma, writer.DirectContent)

                        If (vettDirReg(4) = "RIGETTO FORMALE") Then
                            'devo barrare la scritta VISTO
                            xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteRagOsservazioni" & strConfigTipoDocumento)
                            Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                            TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            TabellaBarraFirma.TotalWidth = 200
                            Dim barraFirma As Phrase = Nothing
                            If strConfigTipoDocumento <> "Delibere" Then
                                barraFirma = New Phrase("____________________________", ftBarra)
                            Else
                                barraFirma = New Phrase("___________", ftBarra)
                            End If
                            TabellaBarraFirma.AddCell(barraFirma)
                            If strConfigTipoDocumento <> "Delibere" Then
                                ' la x della firma e la y delle osservazioni
                                TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigenteOsservazioni - 5), (yDataFirma + 5), writer.DirectContent)
                            Else
                                ' la x della firma e la y delle osservazioni
                                TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigenteFirma - 68), (yDataFirma + 5), writer.DirectContent)
                            End If

                        End If
                    End If
                    If strConfigTipoDocumento = "Delibere" Then
                        xDirigControLegittFirma = ConfigurationManager.AppSettings("xDirigControLegittFirma" & strConfigTipoDocumento)
                        yDirigControLegittFirma = ConfigurationManager.AppSettings("yDirigControLegittFirma" & strConfigTipoDocumento)
                        xDataFirmaDirigControLegitt = ConfigurationManager.AppSettings("xDataFirmaDirigControLegitt" & strConfigTipoDocumento)
                        yDataFirmaDirigControLegitt = ConfigurationManager.AppSettings("yDataFirmaDirigControLegitt" & strConfigTipoDocumento)

                        If Not vettContSegLegittimmita(1) Is Nothing Then
                            Dim TabellaFirmaControlloLegittimita As PdfPTable = New PdfPTable(1)
                            TabellaFirmaControlloLegittimita.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            Dim FirmaLegitt As Phrase = New Phrase(vettContSegLegittimmita(1), ftFirme)
                            TabellaFirmaControlloLegittimita.TotalWidth = 200
                            TabellaFirmaControlloLegittimita.AddCell(FirmaLegitt)
                            TabellaFirmaControlloLegittimita.WriteSelectedRows(0, -1, xDirigControLegittFirma, yDirigControLegittFirma, writer.DirectContent)


                            'Modifica 12.06.15
                            If (vettContSegLegittimmita(4) = "RIGETTO FORMALE") Then
                             
                                'devo barrare la scritta VISTO
                                Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                                TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim barraFirma As Phrase = New Phrase("___________", ftBarra)
                                TabellaBarraFirma.TotalWidth = 200
                                TabellaBarraFirma.AddCell(barraFirma)
                                ' la x della firma e la y delle osservazioni
                                TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigControLegittFirma - 135), (yDirigControLegittFirma + 5), writer.DirectContent)
                            End If
                            '
                        End If

                        If Not vettContSegLegittimmita(2) Is Nothing Then
                            Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                            TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                            Dim dataFirma As Phrase = New Phrase(vettContSegLegittimmita(2), ftFirme)
                            TabellaDataFirma.TotalWidth = 200
                            TabellaDataFirma.AddCell(dataFirma)
                            TabellaDataFirma.WriteSelectedRows(0, -1, xDataFirmaDirigControLegitt, yDataFirmaDirigControLegitt, writer.DirectContent)

                        End If
                    End If

                    'OGGETTO
                    If lstr_Oggetto <> "" Then
                        xOggetto = ConfigurationManager.AppSettings("xOggetto" & strConfigTipoDocumento)

                        If (dataCreazioneDocumento <> Nothing) Then
                            If strConfigTipoDocumento = "Determine" And dataCreazioneDocumento < dataCambioModello Then
                                yOggetto = ConfigurationManager.AppSettings("yOggetto" & strConfigTipoDocumento & costanteVersioneModello)
                            Else
                                yOggetto = ConfigurationManager.AppSettings("yOggetto" & strConfigTipoDocumento)
                            End If
                        End If

                        'oggetto
                        Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                        TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                        Dim ogg As Phrase = New Phrase("" & lstr_Oggetto, ft)
                        ogg.Font.Size = 9
                        TabellaOggeto.TotalWidth = 500
                        TabellaOggeto.AddCell(ogg)
                        TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)
                    End If
		    If strConfigTipoDocumento = "Delibere" Then
                        'FIRMA DG NELLA PRIMA PAGINA
                        If Not vettDirGen(1) Is Nothing Then

                            xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteGenFirma" & strConfigTipoDocumento)
                            yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteGenFirma" & strConfigTipoDocumento)
                            xDataFirma = ConfigurationManager.AppSettings("xDataFirmaGen" & strConfigTipoDocumento)
                            yDataFirma = ConfigurationManager.AppSettings("yDataFirmaGen" & strConfigTipoDocumento)

                            If barraDirettoreGenerale Then
                                Dim TabellaBarraDirGen = New PdfPTable(1)
                                TabellaBarraDirGen.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaBarraDirGen.TotalWidth = 100
                                Dim barra As Phrase = New Phrase("________________", ftBarra)

                                TabellaBarraDirGen.AddCell(barra)
                                TabellaBarraDirGen.WriteSelectedRows(0, -1, xDirigenteFirma - 100, yDirigenteFirma + 4, writer.DirectContent)
                            End If

                            If (vettDirGen(4) = "RIGETTO FORMALE") Then

                                'devo barrare la scritta VISTO
                                Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                                TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim barraFirma As Phrase = New Phrase("____________________", ftBarra)
                                TabellaBarraFirma.TotalWidth = 200
                                TabellaBarraFirma.AddCell(barraFirma)
                                ' la x della firma e la y delle osservazioni
                                'TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigenteFirma - 135), (yDirigenteFirma + 5), writer.DirectContent)
                            End If


                            Dim TabellaFirmaDir As PdfPTable = New PdfPTable(1)
                            TabellaFirmaDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            Dim FirmaDir As Phrase = New Phrase(vettDirGen(1), ftFirme)
                            TabellaFirmaDir.TotalWidth = 200
                            TabellaFirmaDir.AddCell(FirmaDir)
                            TabellaFirmaDir.WriteSelectedRows(0, -1, xDirigenteFirma, yDirigenteFirma, writer.DirectContent)
                        End If
                    End If

                    If strConfigTipoDocumento = "Disposizioni" Then
                        If Not vettDirGen(1) Is Nothing Then

                            xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteGenFirma" & strConfigTipoDocumento)
                            yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteGenFirma" & strConfigTipoDocumento)
                            xDataFirma = ConfigurationManager.AppSettings("xDataFirmaGen" & strConfigTipoDocumento)
                            yDataFirma = ConfigurationManager.AppSettings("yDataFirmaGen" & strConfigTipoDocumento)

                            If barraDirettoreGenerale Then
                                Dim TabellaBarraDirGen = New PdfPTable(1)
                                TabellaBarraDirGen.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaBarraDirGen.TotalWidth = 100
                                Dim barra As Phrase = New Phrase("________________", ftBarra)

                                TabellaBarraDirGen.AddCell(barra)
                                TabellaBarraDirGen.WriteSelectedRows(0, -1, xDirigenteFirma - 100, yDirigenteFirma + 4, writer.DirectContent)
                            End If


                            Dim TabellaFirmaDir As PdfPTable = New PdfPTable(1)
                            TabellaFirmaDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                            Dim FirmaDir As Phrase = New Phrase(vettDirGen(1), ftFirme)
                            TabellaFirmaDir.TotalWidth = 200
                            TabellaFirmaDir.AddCell(FirmaDir)
                            TabellaFirmaDir.WriteSelectedRows(0, -1, xDirigenteFirma, yDirigenteFirma, writer.DirectContent)
                        End If
                    End If
                    If strConfigTipoDocumento = "Determine" Then


                        xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteContAmmOsservazioni" & strConfigTipoDocumento)
                        yDirigenteOsservazioni = ConfigurationManager.AppSettings("yDirigenteContAmmOsservazioni" & strConfigTipoDocumento)
                        xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteContAmmFirma" & strConfigTipoDocumento)
                        yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteContAmmFirma" & strConfigTipoDocumento)
                        xDataFirma = ConfigurationManager.AppSettings("xDataFirmaContAmm" & strConfigTipoDocumento)
                        yDataFirma = ConfigurationManager.AppSettings("yDataFirmaContAmm" & strConfigTipoDocumento)

                        Select Case lstr_TipoPubblicazione

                            Case "0"
                                xIntegrale = ConfigurationManager.AppSettings("xIntegrale" & strConfigTipoDocumento)
                                yIntegrale = ConfigurationManager.AppSettings("yIntegrale" & strConfigTipoDocumento)

                                Dim TabellaPubb As PdfPTable = New PdfPTable(1)
                                TabellaPubb.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim pub As Phrase = New Phrase("X", ft)
                                pub.Font.Size = 8
                                TabellaPubb.TotalWidth = 5
                                TabellaPubb.AddCell(pub)
                                TabellaPubb.WriteSelectedRows(0, -1, xIntegrale, yIntegrale, writer.DirectContent)

                            Case "1"

                                xEstratto = ConfigurationManager.AppSettings("xEstrattoOggDisp" & strConfigTipoDocumento)
                                yEstratto = ConfigurationManager.AppSettings("yEstrattoOggDisp" & strConfigTipoDocumento)

                                Dim TabellaPubb1 As PdfPTable = New PdfPTable(1)
                                TabellaPubb1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim pub1 As Phrase = New Phrase("X", ft)
                                pub1.Font.Size = 8
                                TabellaPubb1.TotalWidth = 5
                                TabellaPubb1.AddCell(pub1)
                                TabellaPubb1.WriteSelectedRows(0, -1, xEstratto, yEstratto, writer.DirectContent)

                            Case "2"

                                xEstratto = ConfigurationManager.AppSettings("xEstratto" & strConfigTipoDocumento)
                                yEstratto = ConfigurationManager.AppSettings("yEstratto" & strConfigTipoDocumento)

                                Dim TabellaPubb1 As PdfPTable = New PdfPTable(1)
                                TabellaPubb1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim pub1 As Phrase = New Phrase("X", ft)
                                pub1.Font.Size = 8
                                TabellaPubb1.TotalWidth = 5
                                TabellaPubb1.AddCell(pub1)
                                TabellaPubb1.WriteSelectedRows(0, -1, xEstratto, yEstratto, writer.DirectContent)

                        End Select
                    End If

                    If strConfigTipoDocumento = "Delibere" Then
                        Select Case lstr_TipoPubblicazione
                            Case "0"
                                xIntegrale = ConfigurationManager.AppSettings("xIntegrale" & strConfigTipoDocumento)
                                yIntegrale = ConfigurationManager.AppSettings("yIntegrale" & strConfigTipoDocumento)

                                Dim TabellaPubb As PdfPTable = New PdfPTable(1)
                                TabellaPubb.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim pub As Phrase = New Phrase("X", ft)
                                pub.Font.Size = 8
                                TabellaPubb.TotalWidth = 5
                                TabellaPubb.AddCell(pub)
                                TabellaPubb.WriteSelectedRows(0, -1, xIntegrale, yIntegrale, writer.DirectContent)

                            Case "2"
                                xEstratto = ConfigurationManager.AppSettings("xEstratto" & strConfigTipoDocumento)
                                yEstratto = ConfigurationManager.AppSettings("yEstratto" & strConfigTipoDocumento)

                                Dim TabellaPubb1 As PdfPTable = New PdfPTable(1)
                                TabellaPubb1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim pub1 As Phrase = New Phrase("X", ft)
                                pub1.Font.Size = 8
                                TabellaPubb1.TotalWidth = 5
                                TabellaPubb1.AddCell(pub1)
                                TabellaPubb1.WriteSelectedRows(0, -1, xEstratto, yEstratto, writer.DirectContent)
                        End Select
                    End If

                End If

                'Gestione osservazioni 

                If flagdisattiva Then
                    If i = n - 1 And flagStampaUffProponente Then
                        'NELLA PENULTIMA PAGINA
                        If strConfigTipoDocumento = "Determine" Or strConfigTipoDocumento = "Delibere" Then

                            'FIRME UP
                            xIstruttoreFirma = ConfigurationManager.AppSettings("xIstruttoreFirma" & strConfigTipoDocumento)
                            yIstruttoreFirma = ConfigurationManager.AppSettings("yIstruttoreFirma" & strConfigTipoDocumento)

                            If Not vettUfficioProp(0) Is Nothing Then
                                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(0), ftFirme)
                                TabellaDataFirma.TotalWidth = 200
                                TabellaDataFirma.AddCell(dataFirma)
                                TabellaDataFirma.WriteSelectedRows(0, -1, xIstruttoreFirma, yIstruttoreFirma, writer.DirectContent)
                            End If

                            xPocFirma = ConfigurationManager.AppSettings("xPocFirma" & strConfigTipoDocumento)
                            yPocFirma = ConfigurationManager.AppSettings("yPocFirma" & strConfigTipoDocumento)

                            If Not vettUfficioProp(1) Is Nothing Then
                                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(1), ftFirme)
                                TabellaDataFirma.TotalWidth = 200
                                TabellaDataFirma.AddCell(dataFirma)
                                TabellaDataFirma.WriteSelectedRows(0, -1, xPocFirma, yPocFirma, writer.DirectContent)
                            End If

                            If isUDD Then
                                xDirigenteUPFirma = ConfigurationManager.AppSettings("xDirigenteUDDFirma" & strConfigTipoDocumento)
                                yDirigenteUPFirma = ConfigurationManager.AppSettings("yDirigenteUDDFirma" & strConfigTipoDocumento)
                            Else
                                xDirigenteUPFirma = ConfigurationManager.AppSettings("xDirigenteUPFirma" & strConfigTipoDocumento)
                                yDirigenteUPFirma = ConfigurationManager.AppSettings("yDirigenteUPFirma" & strConfigTipoDocumento)

                            End If

                            If Not vettUfficioProp(2) Is Nothing Then
                                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(2), ftFirme)
                                TabellaDataFirma.TotalWidth = 200
                                TabellaDataFirma.AddCell(dataFirma)
                                TabellaDataFirma.WriteSelectedRows(0, -1, xDirigenteUPFirma, yDirigenteUPFirma, writer.DirectContent)

                            End If
                            'ElseIf strConfigTipoDocumento = "Disposizioni" Then

                            '    InserisciValoreInUltimaPaginaDisposizione(ftFirme, writer, strConfigTipoDocumento, Ufficio, codufficio, descDipartimento, stampaNumerodata, numdoc, datadoc, flagdisattiva, vettDirGen, vettDirReg, vettContAmm, vettUfficioProp, flagStampaUffProponente, isUDD, lstr_Oggetto, lstr_TipoPubblicazione)

                        End If
                    End If


                    If i = n Then
                        If strConfigTipoDocumento = "Determine" Then
                            'ULTIMA PAGINA

                            'OGGETTO
                            If lstr_Oggetto <> "" Then
                                xOggetto = ConfigurationManager.AppSettings("xOggettoUltimaPagina" & strConfigTipoDocumento)
                                yOggetto = ConfigurationManager.AppSettings("yOggettoUltimaPagina" & strConfigTipoDocumento)
                                'oggetto
                                Dim TabellaOggeto As PdfPTable = New PdfPTable(1)
                                TabellaOggeto.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                Dim ogg As Phrase = New Phrase("" & lstr_Oggetto, ft)
                                ogg.Font.Size = 9
                                TabellaOggeto.TotalWidth = 500
                                TabellaOggeto.AddCell(ogg)
                                TabellaOggeto.WriteSelectedRows(0, -1, xOggetto, yOggetto, writer.DirectContent)

                            End If


                            If Not isUDD Then
                                xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteContAmmOsservazioni" & strConfigTipoDocumento)
                                yDirigenteOsservazioni = ConfigurationManager.AppSettings("yDirigenteContAmmOsservazioni" & strConfigTipoDocumento)
                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteContAmmFirma" & strConfigTipoDocumento)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteContAmmFirma" & strConfigTipoDocumento)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirmaContAmm" & strConfigTipoDocumento)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirmaContAmm" & strConfigTipoDocumento)

                            Else
                                'sposto la firma visto che non è presente il frma dirigente generale
                                xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteContAmmUDDOsservazioni" & strConfigTipoDocumento)
                                yDirigenteOsservazioni = ConfigurationManager.AppSettings("yDirigenteContAmmUDDOsservazioni" & strConfigTipoDocumento)
                                xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteContAmmUDDFirma" & strConfigTipoDocumento)
                                yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteContAmmUDDFirma" & strConfigTipoDocumento)
                                xDataFirma = ConfigurationManager.AppSettings("xDataFirmaContAmmUDD" & strConfigTipoDocumento)
                                yDataFirma = ConfigurationManager.AppSettings("yDataFirmaContAmmUDD" & strConfigTipoDocumento)

                            End If

                            If Not vettContAmm(0) Is Nothing Then
                                Dim TabellaOssDir As PdfPTable = New PdfPTable(1)
                                TabellaOssDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim OssDir As Phrase = New Phrase("" & vettContAmm(0), ft)
                                TabellaOssDir.TotalWidth = 500
                                TabellaOssDir.AddCell(OssDir)
                                TabellaOssDir.WriteSelectedRows(0, -1, xDirigenteOsservazioni, yDirigenteOsservazioni, writer.DirectContent)
                            End If


                            If Not vettContAmm(1) Is Nothing Then
                                Dim TabellaFirmaDir As PdfPTable = New PdfPTable(1)
                                TabellaFirmaDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim FirmaDir As Phrase = New Phrase(vettContAmm(1), ftFirme)
                                TabellaFirmaDir.TotalWidth = 200
                                TabellaFirmaDir.AddCell(FirmaDir)
                                TabellaFirmaDir.WriteSelectedRows(0, -1, xDirigenteFirma, yDirigenteFirma, writer.DirectContent)
                            End If

                            If Not vettContAmm(2) Is Nothing Then
                                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim dataFirma As Phrase = New Phrase(vettContAmm(2), ft)
                                TabellaDataFirma.TotalWidth = 200
                                TabellaDataFirma.AddCell(dataFirma)
                                TabellaDataFirma.WriteSelectedRows(0, -1, xDataFirma, yDataFirma, writer.DirectContent)

                            End If
                            If vettContAmm(4) = "RIGETTO FORMALE" Then
                                'devo barrare la scritta VISTO
                                Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                                TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim barraFirma As Phrase = New Phrase("____________________________", ftBarra)
                                TabellaBarraFirma.TotalWidth = 200
                                TabellaBarraFirma.AddCell(barraFirma)
                                ' la x della firma e la y delle osservazioni
                                TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigenteOsservazioni - 5), (yDataFirma + 5), writer.DirectContent)
                            End If


                            If Not isUDD Then

                                If Not vettDirGen(1) Is Nothing Then

                                    xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteGenOsservazioni" & strConfigTipoDocumento)
                                    yDirigenteOsservazioni = ConfigurationManager.AppSettings("yDirigenteGenOsservazioni" & strConfigTipoDocumento)
                                    xDirigenteFirma = ConfigurationManager.AppSettings("xDirigenteGenFirma" & strConfigTipoDocumento)
                                    yDirigenteFirma = ConfigurationManager.AppSettings("yDirigenteGenFirma" & strConfigTipoDocumento)
                                    xDataFirma = ConfigurationManager.AppSettings("xDataFirmaGen" & strConfigTipoDocumento)
                                    yDataFirma = ConfigurationManager.AppSettings("yDataFirmaGen" & strConfigTipoDocumento)

                                    Dim TabellaOssDir As PdfPTable = New PdfPTable(1)
                                    TabellaOssDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                    Dim OssDir As Phrase = New Phrase("" & vettDirGen(0), ft)
                                    TabellaOssDir.TotalWidth = 500
                                    TabellaOssDir.AddCell(OssDir)
                                    TabellaOssDir.WriteSelectedRows(0, -1, xDirigenteOsservazioni, yDirigenteOsservazioni, writer.DirectContent)


                                    If barraDirettoreGenerale Then
                                        Dim TabellaBarraDirGen = New PdfPTable(1)
                                        TabellaBarraDirGen.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                        TabellaBarraDirGen.TotalWidth = 106
                                        Dim barra As Phrase = New Phrase("_________________", ftBarra)

                                        TabellaBarraDirGen.AddCell(barra)
                                        TabellaBarraDirGen.WriteSelectedRows(0, -1, xDirigenteFirma - 106, yDirigenteFirma + 6, writer.DirectContent)
                                    End If


                                    Dim TabellaFirmaDir As PdfPTable = New PdfPTable(1)
                                    TabellaFirmaDir.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER

                                    Dim FirmaDir As Phrase = New Phrase(vettDirGen(1), ftFirme)
                                    TabellaFirmaDir.TotalWidth = 200


                                    TabellaFirmaDir.AddCell(FirmaDir)
                                    TabellaFirmaDir.WriteSelectedRows(0, -1, xDirigenteFirma, yDirigenteFirma, writer.DirectContent)

                                End If

                            End If
                        End If
                        If strConfigTipoDocumento = "Disposizioni" Then
                            InserisciValoreInUltimaPaginaDisposizione(ftFirme, writer, strConfigTipoDocumento, Ufficio, codufficio, descDipartimento, stampaNumerodata, numdoc, datadoc, flagdisattiva, vettDirGen, vettDirReg, vettContAmm, vettUfficioProp, flagStampaUffProponente, isUDD, lstr_Oggetto, lstr_TipoPubblicazione)
                        End If
                    	If strConfigTipoDocumento = "Delibere" Then
                            Dim xDirigSegretarioDiPresidenzaFirma As String = ConfigurationManager.AppSettings("xDirigSegretarioPresidenzaFirma" & strConfigTipoDocumento)
                            Dim yDirigSegretarioDiPresidenzaFirma As String = ConfigurationManager.AppSettings("yDirigSegretarioPresidenzaFirma" & strConfigTipoDocumento)
                            ' FIRMA DEL SEGRETARIO DI PRESIDENZA
                            If Not vettSegretarioDiPresidenza(1) Is Nothing Then
                                Dim TabellaFirmaSegreteraioDiPresidenza As PdfPTable = New PdfPTable(1)
                                TabellaFirmaSegreteraioDiPresidenza.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim FirmaLegitt As Phrase = New Phrase(vettSegretarioDiPresidenza(1), ftFirme)
                                TabellaFirmaSegreteraioDiPresidenza.TotalWidth = 200
                                TabellaFirmaSegreteraioDiPresidenza.AddCell(FirmaLegitt)
                                TabellaFirmaSegreteraioDiPresidenza.WriteSelectedRows(0, -1, xDirigSegretarioDiPresidenzaFirma, yDirigSegretarioDiPresidenzaFirma, writer.DirectContent)
                            End If

                            If (vettSegretarioDiPresidenza(4) = "RIGETTO FORMALE") Then
                                'devo barrare la scritta VISTO
                                'xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteRagOsservazioni" & strConfigTipoDocumento)
                                Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                                TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaBarraFirma.TotalWidth = 200
                                Dim barraFirma As Phrase = New Phrase("___________", ftBarra)
                                TabellaBarraFirma.AddCell(barraFirma)

                                ' la x della firma e la y delle osservazioni
                                TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigSegretarioDiPresidenzaFirma - 68), (yDirigSegretarioDiPresidenzaFirma + 5), writer.DirectContent)


                            End If

                            Dim xPresidenteFirma As String = ConfigurationManager.AppSettings("xPresidenteFirma" & strConfigTipoDocumento)
                            Dim yPresidenteFirma As String = ConfigurationManager.AppSettings("yPresidenteFirma" & strConfigTipoDocumento)

                            ' FIRMA PRESIDENTE
                            If Not vettPresidenza(1) Is Nothing Then
                                Dim TabellaFirmaPresidente As PdfPTable = New PdfPTable(1)
                                TabellaFirmaPresidente.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                Dim FirmaLegitt As Phrase = New Phrase(vettPresidenza(1), ftFirme)
                                TabellaFirmaPresidente.TotalWidth = 200
                                TabellaFirmaPresidente.AddCell(FirmaLegitt)
                                TabellaFirmaPresidente.WriteSelectedRows(0, -1, xPresidenteFirma, yPresidenteFirma, writer.DirectContent)
                            End If

                            If (vettPresidenza(4) = "RIGETTO FORMALE") Then
                                'devo barrare la scritta VISTO
                                xDirigenteOsservazioni = ConfigurationManager.AppSettings("xDirigenteRagOsservazioni" & strConfigTipoDocumento)
                                Dim TabellaBarraFirma As PdfPTable = New PdfPTable(1)
                                TabellaBarraFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                                TabellaBarraFirma.TotalWidth = 200
                                Dim barraFirma As Phrase = Nothing
                                If strConfigTipoDocumento <> "Delibere" Then
                                    barraFirma = New Phrase("____________________________", ftBarra)
                                Else
                                    barraFirma = New Phrase("___________", ftBarra)
                                End If
                                TabellaBarraFirma.AddCell(barraFirma)
                                If strConfigTipoDocumento <> "Delibere" Then
                                    ' la x della firma e la y delle osservazioni
                                    TabellaBarraFirma.WriteSelectedRows(0, -1, (xDirigenteOsservazioni - 5), (yDataFirma + 5), writer.DirectContent)
                                Else
                                    ' la x della firma e la y delle osservazioni
                                    TabellaBarraFirma.WriteSelectedRows(0, -1, (xPresidenteFirma - 88), (yPresidenteFirma + 5), writer.DirectContent)
                                End If

                            End If

                        End If
		    End If

                End If

            End While


            'Gestione Appendice
            If flagAttachAppendice Then

                Dim pdfFilesAppendice(0) As String
                Dim path As String = ""
                If strConfigTipoDocumento = "Determine" Then
                    path = "" & AppDomain.CurrentDomain.BaseDirectory & "risorse\" & "AppendiceDetermina"
                ElseIf strConfigTipoDocumento = "Disposizioni" Then
                    path = "" & AppDomain.CurrentDomain.BaseDirectory & "risorse\" & "AppendiceDisposizione"
                ElseIf strConfigTipoDocumento = "Delibere" Then
                    path = "" & AppDomain.CurrentDomain.BaseDirectory & "risorse\" & "AppendiceDelibera"
                End If
                path = path.Replace("/", "\")

                pdfFilesAppendice(0) = path
                If strConfigTipoDocumento = "Determine" Then
                    pdfFilesAppendice(0) = path & "_Preimpegni.pdf"
                    importAppendiceDeterminaDelibera_PreImpegno(cb, pdfFilesAppendice, document, writer, listaRagPreImpegni, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont, valoreCUP)

                    pdfFilesAppendice(0) = path & "_Impegni.pdf"
                    importAppendiceDeterminaDelibera_Impegno(cb, pdfFilesAppendice, document, writer, listaRagImpegni, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont, valoreCUP)

                    pdfFilesAppendice(0) = path & "_Liquidazioni.pdf"
                    importAppendiceDetermina_Liquidazione(cb, pdfFilesAppendice, document, writer, listaRagLiq, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont, valoreCUP)

                    pdfFilesAppendice(0) = path & "_Variazioni.pdf"
                    importAppendiceDetermina_Riduzione(cb, pdfFilesAppendice, document, writer, listaRagRiduzioni, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont, valoreCUP)

                End If

                If strConfigTipoDocumento = "Disposizioni" Then
                    pdfFilesAppendice(0) = path & "_Liquidazioni.pdf"
                    importAppendiceDisposizione_Liquidazione(cb, pdfFilesAppendice, document, writer, listaRagLiq, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont, valoreCUP)
                End If

                If strConfigTipoDocumento = "Delibere" Then
                    pdfFilesAppendice(0) = path & "_Preimpegni.pdf"

                    importAppendiceDeterminaDelibera_PreImpegno(cb, pdfFilesAppendice, document, writer, listaRagPreImpegni, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont)

                     pdfFilesAppendice(0) = path & "_Impegni.pdf"
                    importAppendiceDeterminaDelibera_Impegno(cb, pdfFilesAppendice, document, writer, listaRagImpegni, Ufficio, codufficio, numdoc, datadoc, vettDirReg, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione, flagStampaNCont)
                End If

            End If

            If flagAttachNoteUP AndAlso strConfigTipoDocumento = "Determine" Then
                If Not vettUfficioProp(3) Is Nothing Then
                    Dim pdfFilesAppendice(0) As String
                    Dim path As String = ""
                    path = "" & AppDomain.CurrentDomain.BaseDirectory & "risorse\" & "AppendiceNoteUP.pdf"
                    path = path.Replace("/", "\")
                    pdfFilesAppendice(0) = path
                    importAppendiceNoteUP(cb, pdfFilesAppendice, document, writer, Ufficio, codufficio, numdoc, datadoc, vettUfficioProp, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione)
                End If

                ' Il proponente è l'ufficio del direttore generale 
                ' e ha inserito annotazioni (contenute in vettDirGen(0)???) 
                If isUDD And Not vettDirGen(0) Is Nothing Then
                    Dim pdfFilesAppendice(0) As String
                    Dim path As String = ""
                    path = "" & AppDomain.CurrentDomain.BaseDirectory & "risorse\" & "AppendiceNoteUP.pdf"
                    path = path.Replace("/", "\")
                    pdfFilesAppendice(0) = path

                    ' Le note del dirigente generale si trovano in vettDirGen(0)
                    vettUfficioProp(3) = vettDirGen(0)
                    importAppendiceNoteUP(cb, pdfFilesAppendice, document, writer, Ufficio, codufficio, numdoc, datadoc, vettUfficioProp, strConfigTipoDocumento, lstr_Oggetto, lstr_TipoPubblicazione)
                End If
            End If
            f += 1


            document.Close()
            Return memStreamReturn


        Catch e As Exception
            Return Nothing
        End Try

    End Function
    Private Sub InserisciValoreInUltimaPaginaDisposizione(ByVal ftFirme As iTextSharp.text.Font, ByRef writer As PdfWriter, ByVal strConfigTipoDocumento As String, ByVal Ufficio As String, ByVal codufficio As String, ByVal descDipartimento As String, ByVal stampaNumerodata As Boolean, ByVal numdoc As String, ByVal datadoc As String, ByVal flagdisattiva As Boolean, ByVal vettDirGen() As Object, ByVal vettDirReg() As Object, ByVal vettContAmm() As Object, ByVal vettUfficioProp() As Object, ByVal flagStampaUffProponente As Boolean, ByVal isUDD As Boolean, Optional ByVal lstr_Oggetto As String = "", Optional ByVal lstr_TipoPubblicazione As String = "")
        Try



            Dim xUfficio As Single = 355
            Dim yUfficio As Single = 750
            Dim xNumero As Single = 365
            Dim yNumero As Single = 682

            Dim xData As Single = 365
            Dim yData As Single = 682

            Dim xDirigenteOsservazioni As Single = 290
            Dim yDirigenteOsservazioni As Single = 68
            Dim xDirigenteFirma As Single = 290
            Dim yDirigenteFirma As Single = 68
            Dim xDataFirma As Single = 455
            Dim yDataFirma As Single = 68


            Dim xIstruttoreFirma As Single = 290
            Dim yIstruttoreFirma As Single = 68
            Dim xPocFirma As Single = 290
            Dim yPocFirma As Single = 68
            Dim xDirigenteUPFirma As Single = 455
            Dim yDirigenteUPFirma As Single = 68

            Dim xOggetto As Single = 290
            Dim yOggetto As Single = 68
            Dim xIntegrale As Single = 290
            Dim yIntegrale As Single = 68
            Dim xEstratto As Single = 455
            Dim yEstratto As Single = 68
            Dim xDipartimento As Single = 130 'daintrodurre
            Dim yDipartimento As Single = 740 'daintrodurre
            Dim xNAllegati As Single = 70 'daintrodurre
            Dim yNAllegati As Single = 70 'daintrodurre

            Try
                xUfficio = ConfigurationManager.AppSettings("xUfficio" & strConfigTipoDocumento)
                yUfficio = ConfigurationManager.AppSettings("yUfficio" & strConfigTipoDocumento)
                xNumero = ConfigurationManager.AppSettings("xNumero" & strConfigTipoDocumento)
                yNumero = ConfigurationManager.AppSettings("yNumero" & strConfigTipoDocumento)
                xData = ConfigurationManager.AppSettings("xData" & strConfigTipoDocumento)
                yData = ConfigurationManager.AppSettings("yData" & strConfigTipoDocumento)
                xDipartimento = ConfigurationManager.AppSettings("xDipartimento" & strConfigTipoDocumento)
                yDipartimento = ConfigurationManager.AppSettings("yDipartimento" & strConfigTipoDocumento)
                xNAllegati = ConfigurationManager.AppSettings("xNAllegati" & strConfigTipoDocumento)
                yNAllegati = ConfigurationManager.AppSettings("yNAllegati" & strConfigTipoDocumento)


            Catch ex As Exception
                Log.Error("Attenzione mancano le posizioni per la scrittura nel file di configurazione per " & strConfigTipoDocumento)
            End Try




            xIstruttoreFirma = ConfigurationManager.AppSettings("xIstruttoreFirma" & strConfigTipoDocumento)
            yIstruttoreFirma = ConfigurationManager.AppSettings("yIstruttoreFirma" & strConfigTipoDocumento)

            If Not vettUfficioProp(0) Is Nothing Then
                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(0), ftFirme)
                TabellaDataFirma.TotalWidth = 200
                TabellaDataFirma.AddCell(dataFirma)
                TabellaDataFirma.WriteSelectedRows(0, -1, xIstruttoreFirma, yIstruttoreFirma, writer.DirectContent)

            End If
            xPocFirma = ConfigurationManager.AppSettings("xPocFirma" & strConfigTipoDocumento)
            yPocFirma = ConfigurationManager.AppSettings("yPocFirma" & strConfigTipoDocumento)

            If Not vettUfficioProp(1) Is Nothing Then
                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(1), ftFirme)
                TabellaDataFirma.TotalWidth = 200
                TabellaDataFirma.AddCell(dataFirma)
                TabellaDataFirma.WriteSelectedRows(0, -1, xPocFirma, yPocFirma, writer.DirectContent)

            End If

            If isUDD Then
                xDirigenteUPFirma = ConfigurationManager.AppSettings("xDirigenteUDDFirma" & strConfigTipoDocumento)
                yDirigenteUPFirma = ConfigurationManager.AppSettings("yDirigenteUDDFirma" & strConfigTipoDocumento)

            Else
                xDirigenteUPFirma = ConfigurationManager.AppSettings("xDirigenteUPFirma" & strConfigTipoDocumento)
                yDirigenteUPFirma = ConfigurationManager.AppSettings("yDirigenteUPFirma" & strConfigTipoDocumento)

            End If

            If Not vettUfficioProp(2) Is Nothing Then
                Dim TabellaDataFirma As PdfPTable = New PdfPTable(1)
                TabellaDataFirma.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                Dim dataFirma As Phrase = New Phrase(vettUfficioProp(2), ftFirme)
                TabellaDataFirma.TotalWidth = 200
                TabellaDataFirma.AddCell(dataFirma)
                TabellaDataFirma.WriteSelectedRows(0, -1, xDirigenteUPFirma, yDirigenteUPFirma, writer.DirectContent)

            End If






        Catch e As Exception
            Log.Error(e.Message)
        End Try

    End Sub
End Class
