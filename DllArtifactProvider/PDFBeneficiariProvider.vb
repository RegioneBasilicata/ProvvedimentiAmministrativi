Imports System.Collections.Generic
Imports DllDocumentale
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class PDFBeneficiariProvider

    Public Function getPDFBeneficiari(ByVal codDocumento As String, ByVal codOperatore As String, ByVal nascondiBeneficiariConDatiSensibili As Boolean) As Byte()
        Dim oOperatore As DllAmbiente.Operatore = New DllAmbiente.Operatore
        oOperatore.Codice = codOperatore

        Return getPDFBeneficiari(codDocumento, oOperatore, nascondiBeneficiariConDatiSensibili)
    End Function

    Public Function getPDFBeneficiari(ByVal codDocumento As String, ByRef oOperatore As DllAmbiente.Operatore, ByVal nascondiBeneficiariConDatiSensibili As Boolean) As Byte()
        Dim ms As MemoryStream = New MemoryStream()

        'Create our PDF document            
        Dim Doc As New Document(PageSize.LETTER)

        'Bind our PDF object to the physical file using a PdfWriter               
        Dim Writer As iTextSharp.text.pdf.PdfWriter = PdfWriter.GetInstance(Doc, ms)

        'Open our document for writing                   
        Doc.Open()

        'Insert a blank page                    
        Doc.NewPage()

        Dim fontIntestazione As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 7.0F, iTextSharp.text.Font.BOLD)
        Dim fontDatiLiquidaz As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9.0F, Font.BOLD)
        Dim fontBody As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6.0F)

        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim listaLiquidazioni As IList(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(codDocumento, 0, "", False)

        If (listaLiquidazioni.Count > 0) Then
            Dim countLiquidazioni As Integer = 1

            Dim listaBeneficiari As IList(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)

            For Each liquidazione As ItemLiquidazioneInfo In listaLiquidazioni

                Dim table As PdfPTable = New PdfPTable(8)
                table.TotalWidth = 226.0F
                Dim widths() As Single = New Single() {31.0F, 35.0F, 27.0F, 38.0F, 50.0F, 14.0F, 14.0F, 17.0F}
                table.SetWidths(widths)

                listaBeneficiari = dllDoc.FO_Get_ListaBeneficiariLiquidazione(oOperatore, liquidazione.Dli_Documento, , liquidazione.Dli_prog)
                liquidazione.ListaBeneficiari = listaBeneficiari

                Dim cellNumLiquidaz As PdfPCell

                If (liquidazione.Dli_NLiquidazione <> 0) Then
                    cellNumLiquidaz = New PdfPCell(New Phrase("Liquidazione n° " + liquidazione.Dli_NLiquidazione.ToString))
                Else
                    cellNumLiquidaz = New PdfPCell(New Phrase("Liquidazione n° " + countLiquidazioni.ToString))
                End If

                cellNumLiquidaz.Colspan = 8
                cellNumLiquidaz.Border = 0
                cellNumLiquidaz.HorizontalAlignment = 0
                table.AddCell(cellNumLiquidaz)
                Dim infoLiquidazione As String = ""
                If (liquidazione.Dli_NumImpegno <> "0") Then
                    infoLiquidazione = "Bilancio: " + liquidazione.Dli_Esercizio + "  -  N° Impegno: " + liquidazione.Dli_NumImpegno + "  -  UPB: " + liquidazione.Dli_UPB + "  -  Capitolo: " + liquidazione.Dli_Cap
                ElseIf (liquidazione.Dli_NPreImpegno <> "0") Then
                    infoLiquidazione = "Bilancio: " + liquidazione.Dli_Esercizio + "  -  N° Preimpegno: " + liquidazione.Dli_NPreImpegno + "  -  UPB: " + liquidazione.Dli_UPB + "  -  Capitolo: " + liquidazione.Dli_Cap
                End If
                Dim cellImpegno As PdfPCell = New PdfPCell(New Phrase(infoLiquidazione, fontDatiLiquidaz))
                cellImpegno.Colspan = 8
                cellImpegno.Border = 0
                cellImpegno.HorizontalAlignment = 1
                table.AddCell(cellImpegno)

                Dim cellDenom As PdfPCell = New PdfPCell(New Phrase("Denominazione", fontIntestazione))
                Dim cellCodFiscPartIva As PdfPCell = New PdfPCell(New Phrase("Cod.Fisc./Partita IVA", fontIntestazione))
                Dim cellSede As PdfPCell = New PdfPCell(New Phrase("Sede", fontIntestazione))

                Dim cellPagamento As PdfPCell = New PdfPCell(New Phrase("Pagamento", fontIntestazione))
                Dim cellIBANCC As PdfPCell = New PdfPCell(New Phrase("IBAN/Conto Corrente", fontIntestazione))

                Dim cellCig As PdfPCell = New PdfPCell(New Phrase("C.I.G.", fontIntestazione))
                Dim cellCup As PdfPCell = New PdfPCell(New Phrase("C.U.P.", fontIntestazione))
                Dim cellImporto As PdfPCell = New PdfPCell(New Phrase("Importo", fontIntestazione))


                cellDenom.BorderWidth = 1.0F
                cellDenom.HorizontalAlignment = 1
                cellCodFiscPartIva.BorderWidth = 1.0F
                cellCodFiscPartIva.HorizontalAlignment = 1

                cellSede.BorderWidth = 1.0F
                cellSede.HorizontalAlignment = 1

                cellPagamento.BorderWidth = 1.0F
                cellPagamento.HorizontalAlignment = 1
                cellIBANCC.BorderWidth = 1.0F
                cellIBANCC.HorizontalAlignment = 1

                cellCig.BorderWidth = 1.0F
                cellCig.HorizontalAlignment = 1
                cellCup.BorderWidth = 1.0F
                cellCup.HorizontalAlignment = 1
                cellImporto.BorderWidth = 1.0F
                cellImporto.HorizontalAlignment = 1

                table.AddCell(cellDenom)
                table.AddCell(cellCodFiscPartIva)
                table.AddCell(cellSede)
                table.AddCell(cellPagamento)
                table.AddCell(cellIBANCC)
                table.AddCell(cellCig)
                table.AddCell(cellCup)
                table.AddCell(cellImporto)

                Dim totaleLiquidazBeneficiari As Decimal = 0
                For Each beneficiario As ItemLiquidazioneImpegnoBeneficiarioInfo In listaBeneficiari
                    table.AddCell(New Phrase(IIf(beneficiario.IsDatoSensibile AndAlso nascondiBeneficiariConDatiSensibili, beneficiario.IdAnagrafica, beneficiario.Denominazione), fontBody))
                    If beneficiario.IsDatoSensibile AndAlso nascondiBeneficiariConDatiSensibili Then
                        table.AddCell(New Phrase(IIf(beneficiario.FlagPersonaFisica, "xxxxxxxxxxxxxxxx", "xxxxxxxxxxx"), fontBody))
                    Else
                        If (beneficiario.FlagPersonaFisica) Then
                            table.AddCell(New Phrase(beneficiario.CodiceFiscale, fontBody))
                        Else
                            table.AddCell(New Phrase(beneficiario.PartitaIva, fontBody))
                        End If

                    End If

                    If beneficiario.IsDatoSensibile AndAlso nascondiBeneficiariConDatiSensibili Then
                        table.AddCell(New Phrase(beneficiario.IdSede, fontBody))
                    Else
                        table.AddCell(New Phrase(beneficiario.SedeVia + Environment.NewLine + beneficiario.SedeProvincia + " " + beneficiario.SedeComune, fontBody))
                    End If

                    table.AddCell(New Phrase(beneficiario.DescrizioneModalitaPag, fontBody))

                    If (beneficiario.IdConto <> Nothing And beneficiario.IdConto <> "") Then
                        If beneficiario.IsDatoSensibile AndAlso nascondiBeneficiariConDatiSensibili Then
                            table.AddCell(New Phrase("xxxxxxxxxxxxxxxxxxxxxxxxxxx", fontBody))
                        Else
                            table.AddCell(New Phrase(beneficiario.Iban, fontBody))
                        End If
                    Else
                        table.AddCell("")
                    End If

                    table.AddCell(New Phrase(beneficiario.Cig, fontBody))
                    table.AddCell(New Phrase(beneficiario.Cup, fontBody))
                    Dim cellImportoSpettante As PdfPCell = New PdfPCell(New Phrase(beneficiario.ImportoSpettante.ToString("0.00"), fontBody))
                    cellImportoSpettante.HorizontalAlignment = 1
                    table.AddCell(cellImportoSpettante)

                    totaleLiquidazBeneficiari = totaleLiquidazBeneficiari + beneficiario.ImportoSpettante
                Next

                Dim cellLabelTotaleImporti As PdfPCell = New PdfPCell(New Phrase("(Totale da liquidare: " + liquidazione.Dli_Costo.ToString + ")                       Totale importi: ", fontIntestazione))
                cellLabelTotaleImporti.Colspan = 7
                cellLabelTotaleImporti.HorizontalAlignment = 2
                table.AddCell(cellLabelTotaleImporti)

                Dim cellTotale As PdfPCell = New PdfPCell(New Phrase(totaleLiquidazBeneficiari.ToString("0.00"), fontIntestazione))

                cellTotale.HorizontalAlignment = 1
                table.AddCell(cellTotale)

                Doc.Add(table)
                Doc.Add(New Phrase(Environment.NewLine))

                countLiquidazioni = countLiquidazioni + 1
            Next
        Else
            Doc.Add(New Phrase(Environment.NewLine))
        End If

        Doc.Close()
        Writer.Close()

        Return ms.GetBuffer()
    End Function
End Class
