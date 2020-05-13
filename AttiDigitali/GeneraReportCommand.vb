Imports System.Collections.Generic
Imports DllDocumentale
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Web.UI.DataVisualization.Charting

Public Class GeneraReportCommand
    Implements ICommand

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(DatiContabiliPDFCommand))

    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim siglaUfficio As String = ""
        Dim nomeUfficio As String = ""
        If oOperatore.oUfficio.Test_Attributo("UFFICIO_RAGIONERIA", 1) OrElse oOperatore.oUfficio.Test_Attributo("UFFICIO_RAGIONERIA_OLD", 1) Then
            siglaUfficio = "UR"
            nomeUfficio = "Ufficio Ragioneria"
        ElseIf oOperatore.oUfficio.Test_Attributo("UFFICIO_CONTROLLO_AMMINISTRATIVO", 1) Then
            siglaUfficio = "UCA"
            nomeUfficio = "Ufficio Controllo Amministrativo"
        ElseIf oOperatore.oUfficio.Test_Attributo("UFFICIO_DIRIGENZA_DIPARTIMENTO", 1) Then
            siglaUfficio = "UDD"
            nomeUfficio = "Dirigenza Dipartimento"
        End If


        Dim dataDa As String = Format(CDate(context.Session.Item("txtFiltroDataDa")), "yyyy-MM-dd")
        Dim dataA As String = Format(CDate(context.Session.Item("txtFiltroDataA")), "yyyy-MM-dd")

        Dim annoDa As String = Format(CDate(context.Session.Item("txtFiltroDataDa")), "yyyy")
        Dim annoA As String = Format(CDate(context.Session.Item("txtFiltroDataA")), "yyyy")

        Dim annoDaInt As Integer
        Dim annoAInt As Integer
        Dim numAnni As Integer = 0
        Dim anno As String = ""
        If annoA <> annoDa Then
            annoDaInt = Integer.Parse(annoDa)
            annoAInt = Integer.Parse(annoA)
            numAnni = annoA - annoDa
        End If
        anno = annoDa

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
        Dim fontTitolo As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 9.0F, iTextSharp.text.Font.BOLD)
        Dim fontBold As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6.0F, iTextSharp.text.Font.BOLD)
        Dim fontDati As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6.0F)
        Dim fontDatiCenter As Font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA)


        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim listaReportTotaleAtti As IList(Of DllDocumentale.ItemReportAtti) = dllDoc.FO_Get_ReportAttiTotaliEnte(anno, numAnni, dataDa, dataA)

        Dim listaReportTransitatiIn As IList(Of DllDocumentale.ItemReportAtti) = dllDoc.FO_Get_ReportTransitatiIn(siglaUfficio, anno, numAnni, dataDa, dataA)
        Dim listaReportConOsservazioni As IList(Of DllDocumentale.ItemReportAtti)
        If siglaUfficio <> "UCA" Then
            listaReportConOsservazioni = dllDoc.FO_Get_ReportConOsservazioni(siglaUfficio, anno, numAnni, dataDa, dataA)
        End If
        Dim listaReportRigettati As IList(Of DllDocumentale.ItemReportAtti) = dllDoc.FO_Get_ReportRigettati(siglaUfficio, anno, numAnni, dataDa, dataA)

        Doc.Add(New Phrase("Report periodo dal " & Format(CDate(context.Session.Item("txtFiltroDataDa")), "dd-MM-yyyy") & " al " & Format(CDate(context.Session.Item("txtFiltroDataA")), "dd-MM-yyyy"), fontTitolo))





        'TABELLA TOTALE ATTI
        Dim tableTotale As PdfPTable = New PdfPTable(5)
        tableTotale.TotalWidth = 210.0F
        Dim widths() As Single = New Single() {25.0F, 25.0F, 25.0F, 15.0F, 120.0F}
        tableTotale.SetWidths(widths)


        Dim cellTitolo As New PdfPCell(New Phrase("Totale atti", fontIntestazione))
        cellTitolo.Colspan = 8
        cellTitolo.Border = 0
        cellTitolo.HorizontalAlignment = 0
        tableTotale.AddCell(cellTitolo)



        Dim cellTotaleAtti As PdfPCell = New PdfPCell(New Phrase("Totale", fontIntestazione))
        Dim cellTotDet As PdfPCell = New PdfPCell(New Phrase("DETERMINE", fontIntestazione))
        Dim cellTotDisp As PdfPCell = New PdfPCell(New Phrase("DISPOSIZIONI", fontIntestazione))
        Dim cellCodDip As PdfPCell = New PdfPCell(New Phrase("Cod. Dip.", fontIntestazione))
        Dim cellDescDip As PdfPCell = New PdfPCell(New Phrase("Descrizione Dipartimento", fontIntestazione))


        cellTotaleAtti.BorderWidth = 1.0F
        cellTotaleAtti.HorizontalAlignment = 1
        cellTotDet.BorderWidth = 1.0F
        cellTotDet.HorizontalAlignment = 1
        cellTotDisp.BorderWidth = 1.0F
        cellTotDisp.HorizontalAlignment = 1

        cellCodDip.BorderWidth = 1.0F
        cellCodDip.HorizontalAlignment = 1
        cellDescDip.BorderWidth = 1.0F
        cellDescDip.HorizontalAlignment = 1

        tableTotale.AddCell(cellTotaleAtti)
        tableTotale.AddCell(cellTotDet)
        tableTotale.AddCell(cellTotDisp)
        tableTotale.AddCell(cellCodDip)
        tableTotale.AddCell(cellDescDip)


        Dim totPrimaCol As Integer = 0
        Dim totSecondaCol As Integer = 0
        Dim totTerzaCol As Integer = 0
        For Each recordDetermine As DllDocumentale.ItemReportAtti In listaReportTotaleAtti

            tableTotale.AddCell(New Phrase((recordDetermine.TotaleAtti), fontDatiCenter))
            tableTotale.AddCell(New Phrase((recordDetermine.TotaleDetermine), fontDatiCenter))
            tableTotale.AddCell(New Phrase((recordDetermine.TotaleDisposizioni), fontDatiCenter))

            tableTotale.AddCell(New Phrase((recordDetermine.CodDipartimento), fontDatiCenter))
            tableTotale.AddCell(New Phrase((recordDetermine.DescDipartimento), fontDati))

            totPrimaCol = totPrimaCol + recordDetermine.TotaleAtti
            totSecondaCol = totSecondaCol + recordDetermine.TotaleDetermine
            totTerzaCol = totTerzaCol + recordDetermine.TotaleDisposizioni
        Next

        tableTotale.AddCell(New Phrase(totPrimaCol, fontDatiCenter))
        tableTotale.AddCell(New Phrase(totSecondaCol, fontDatiCenter))
        tableTotale.AddCell(New Phrase(totTerzaCol, fontDatiCenter))

        Dim cellColSpan2 As PdfPCell = New PdfPCell(New Phrase(" TOTALE ", fontIntestazione))
        cellColSpan2.Colspan = 2
        cellColSpan2.HorizontalAlignment = 0
        tableTotale.AddCell(cellColSpan2)




        'TABELLA ATTI TRANSITATI
        Dim tableTransitatiIn As PdfPTable = New PdfPTable(5)
        tableTransitatiIn.TotalWidth = 135.0F
        tableTransitatiIn.SetWidths(widths)

        Dim cellTitoloTransitatiIn As New PdfPCell(New Phrase("Atti transitati in " & nomeUfficio & Environment.NewLine, fontIntestazione))
        cellTitoloTransitatiIn.Colspan = 8
        cellTitoloTransitatiIn.Border = 0
        cellTitoloTransitatiIn.HorizontalAlignment = 0
        tableTransitatiIn.AddCell(cellTitoloTransitatiIn)

        tableTransitatiIn.AddCell(cellTotaleAtti)
        tableTransitatiIn.AddCell(cellTotDet)
        tableTransitatiIn.AddCell(cellTotDisp)
        tableTransitatiIn.AddCell(cellCodDip)
        tableTransitatiIn.AddCell(cellDescDip)

        Dim totPrimaColTransitatiIn As Integer = 0
        Dim totSecondaColTransitatiIn As Integer = 0
        Dim totTerzaColTransitatiIn As Integer = 0
        For Each recordDetermine As DllDocumentale.ItemReportAtti In listaReportTransitatiIn

            tableTransitatiIn.AddCell(New Phrase((recordDetermine.TotaleAtti), fontDatiCenter))
            tableTransitatiIn.AddCell(New Phrase((recordDetermine.TotaleDetermine), fontDatiCenter))
            tableTransitatiIn.AddCell(New Phrase((recordDetermine.TotaleDisposizioni), fontDatiCenter))

            tableTransitatiIn.AddCell(New Phrase((recordDetermine.CodDipartimento), fontDatiCenter))
            tableTransitatiIn.AddCell(New Phrase((recordDetermine.DescDipartimento), fontDati))

            totPrimaColTransitatiIn = totPrimaColTransitatiIn + recordDetermine.TotaleAtti
            totSecondaColTransitatiIn = totSecondaColTransitatiIn + recordDetermine.TotaleDetermine
            totTerzaColTransitatiIn = totTerzaColTransitatiIn + recordDetermine.TotaleDisposizioni
        Next

        tableTransitatiIn.AddCell(New Phrase(totPrimaColTransitatiIn, fontDatiCenter))
        tableTransitatiIn.AddCell(New Phrase(totSecondaColTransitatiIn, fontDatiCenter))
        tableTransitatiIn.AddCell(New Phrase(totTerzaColTransitatiIn, fontDatiCenter))

        tableTransitatiIn.AddCell(cellColSpan2)


        'TABELLA ATTI CON OSSERVAZIONI
        Dim tableConOsservazioni As PdfPTable = New PdfPTable(5)
        Dim totPrimaColConOsservazioni As Integer = 0
        Dim totSecondaColConOsservazioni As Integer = 0
        Dim totTerzaColConOsservazioni As Integer = 0

        If siglaUfficio <> "UCA" Then
            tableConOsservazioni.TotalWidth = 135.0F
            tableConOsservazioni.SetWidths(widths)

            Dim cellTitoloConOsservazioni As New PdfPCell(New Phrase("Atti con osservazioni" & Environment.NewLine, fontIntestazione))
            cellTitoloConOsservazioni.Colspan = 8
            cellTitoloConOsservazioni.Border = 0
            cellTitoloConOsservazioni.HorizontalAlignment = 0
            tableConOsservazioni.AddCell(cellTitoloConOsservazioni)

            tableConOsservazioni.AddCell(cellTotaleAtti)
            tableConOsservazioni.AddCell(cellTotDet)
            tableConOsservazioni.AddCell(cellTotDisp)
            tableConOsservazioni.AddCell(cellCodDip)
            tableConOsservazioni.AddCell(cellDescDip)


            For Each recordDetermine As DllDocumentale.ItemReportAtti In listaReportConOsservazioni

                tableConOsservazioni.AddCell(New Phrase((recordDetermine.TotaleAtti), fontDatiCenter))
                tableConOsservazioni.AddCell(New Phrase((recordDetermine.TotaleDetermine), fontDatiCenter))
                tableConOsservazioni.AddCell(New Phrase((recordDetermine.TotaleDisposizioni), fontDatiCenter))

                tableConOsservazioni.AddCell(New Phrase((recordDetermine.CodDipartimento), fontDatiCenter))
                tableConOsservazioni.AddCell(New Phrase((recordDetermine.DescDipartimento), fontDati))

                totPrimaColConOsservazioni = totPrimaColConOsservazioni + recordDetermine.TotaleAtti
                totSecondaColConOsservazioni = totSecondaColConOsservazioni + recordDetermine.TotaleDetermine
                totTerzaColConOsservazioni = totTerzaColConOsservazioni + recordDetermine.TotaleDisposizioni
            Next

            tableConOsservazioni.AddCell(New Phrase(totPrimaColConOsservazioni, fontDatiCenter))
            tableConOsservazioni.AddCell(New Phrase(totSecondaColConOsservazioni, fontDatiCenter))
            tableConOsservazioni.AddCell(New Phrase(totTerzaColConOsservazioni, fontDatiCenter))

            tableConOsservazioni.AddCell(cellColSpan2)


        End If

        'TABELLA ATTI RIGETTATI CON OSSERVAZIONI
        Dim tableRigettati As PdfPTable = New PdfPTable(5)
        tableRigettati.TotalWidth = 135.0F
        tableRigettati.SetWidths(widths)

        Dim cellTitoloRigettati As New PdfPCell(New Phrase("Atti rigettati" & Environment.NewLine, fontIntestazione))
        cellTitoloRigettati.Colspan = 8
        cellTitoloRigettati.Border = 0
        cellTitoloRigettati.HorizontalAlignment = 0
        tableRigettati.AddCell(cellTitoloRigettati)

        tableRigettati.AddCell(cellTotaleAtti)
        tableRigettati.AddCell(cellTotDet)
        tableRigettati.AddCell(cellTotDisp)
        tableRigettati.AddCell(cellCodDip)
        tableRigettati.AddCell(cellDescDip)

        Dim totPrimaColRigettati As Integer = 0
        Dim totSecondaColRigettati As Integer = 0
        Dim totTerzaColRigettati As Integer = 0

        For Each recordDetermine As DllDocumentale.ItemReportAtti In listaReportRigettati

            tableRigettati.AddCell(New Phrase((recordDetermine.TotaleAtti), fontDatiCenter))
            tableRigettati.AddCell(New Phrase((recordDetermine.TotaleDetermine), fontDatiCenter))
            tableRigettati.AddCell(New Phrase((recordDetermine.TotaleDisposizioni), fontDatiCenter))

            tableRigettati.AddCell(New Phrase((recordDetermine.CodDipartimento), fontDatiCenter))
            tableRigettati.AddCell(New Phrase((recordDetermine.DescDipartimento), fontDati))

            totPrimaColRigettati = totPrimaColRigettati + recordDetermine.TotaleAtti
            totSecondaColRigettati = totSecondaColRigettati + recordDetermine.TotaleDetermine
            totTerzaColRigettati = totTerzaColRigettati + recordDetermine.TotaleDisposizioni
        Next

        Dim listaRigettati As New List(Of Integer)
        listaRigettati.Add(totSecondaColRigettati)
        listaRigettati.Add(totTerzaColRigettati)



        tableRigettati.AddCell(New Phrase(totPrimaColRigettati, fontDatiCenter))
        tableRigettati.AddCell(New Phrase(totSecondaColRigettati, fontDatiCenter))
        tableRigettati.AddCell(New Phrase(totTerzaColRigettati, fontDatiCenter))

        tableRigettati.AddCell(cellColSpan2)



        Dim tableRiepilogo As PdfPTable = New PdfPTable(4)
        tableRiepilogo.TotalWidth = 175.0F
        Dim widthsRiep() As Single = New Single() {100.0F, 25.0F, 25.0F, 25.0F}
        tableRiepilogo.SetWidths(widthsRiep)


        Dim cellTitoloRiepilogo As New PdfPCell(New Phrase("Riepilogo", fontIntestazione))
        cellTitoloRiepilogo.Colspan = 8
        cellTitoloRiepilogo.Border = 0
        cellTitoloRiepilogo.HorizontalAlignment = 0
        tableRiepilogo.AddCell(cellTitoloRiepilogo)



        Dim cellDescrizione As PdfPCell = New PdfPCell(New Phrase("", fontIntestazione))
        Dim cellTotaleRiepilogo As PdfPCell = New PdfPCell(New Phrase("Totale", fontIntestazione))
        Dim cellTotDetRiepilogo As PdfPCell = New PdfPCell(New Phrase("Determine", fontIntestazione))
        Dim cellTotDispRiepilogo As PdfPCell = New PdfPCell(New Phrase("Disposizioni", fontIntestazione))



        cellDescrizione.BorderWidth = 1.0F
        cellDescrizione.HorizontalAlignment = 1
        cellTotaleRiepilogo.BorderWidth = 1.0F
        cellTotaleRiepilogo.HorizontalAlignment = 1

        cellTotDetRiepilogo.BorderWidth = 1.0F
        cellTotDetRiepilogo.HorizontalAlignment = 1
        cellTotDispRiepilogo.BorderWidth = 1.0F
        cellTotDispRiepilogo.HorizontalAlignment = 1

        tableRiepilogo.AddCell(cellDescrizione)
        tableRiepilogo.AddCell(cellTotaleRiepilogo)
        tableRiepilogo.AddCell(cellTotDetRiepilogo)
        tableRiepilogo.AddCell(cellTotDispRiepilogo)

        tableRiepilogo.AddCell(New Phrase("Totale Atti", fontIntestazione))
        tableRiepilogo.AddCell(New Phrase(totPrimaCol, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totSecondaCol, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totTerzaCol, fontDatiCenter))

        tableRiepilogo.AddCell(New Phrase("Atti transitati", fontIntestazione))
        tableRiepilogo.AddCell(New Phrase(totPrimaColTransitatiIn, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totSecondaColTransitatiIn, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totTerzaColTransitatiIn, fontDatiCenter))

        If siglaUfficio <> "UCA" Then
            tableRiepilogo.AddCell(New Phrase("Atti con osservazioni (rigettati e non)", fontIntestazione))
            tableRiepilogo.AddCell(New Phrase(totPrimaColConOsservazioni, fontDatiCenter))
            tableRiepilogo.AddCell(New Phrase(totSecondaColConOsservazioni, fontDatiCenter))
            tableRiepilogo.AddCell(New Phrase(totTerzaColConOsservazioni, fontDatiCenter))
        End If
        tableRiepilogo.AddCell(New Phrase("Atti rigettati", fontIntestazione))
        tableRiepilogo.AddCell(New Phrase(totPrimaColRigettati, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totSecondaColRigettati, fontDatiCenter))
        tableRiepilogo.AddCell(New Phrase(totTerzaColRigettati, fontDatiCenter))




        Doc.Add(tableRiepilogo)
        Doc.Add(New Phrase(Environment.NewLine))

        Doc.Add(tableTotale)
        Doc.Add(New Phrase(Environment.NewLine))

        Doc.Add(tableTransitatiIn)
        Doc.Add(New Phrase(Environment.NewLine))

        If siglaUfficio <> "UCA" Then
            Doc.Add(tableConOsservazioni)
            Doc.Add(New Phrase(Environment.NewLine))
        End If

        Doc.Add(tableRigettati)



        'Dim memoryStream As New MemoryStream()
        'Dim grafico As New Chart()

        'Dim serie As New Series("prova")
        'Dim p1 As New DataPoint(3, 4)
        'Dim p2 As New DataPoint(6, 7)
        'Dim p3 As New DataPoint(20, 33)
        'serie.Points.Add(p1)
        'serie.Points.Add(p2)
        'serie.Points.Add(p3)
        'grafico.Series.Add("prova")


        'Dim Series1 As Series = grafico.Series("prova")

        'grafico.Series(Series1.Name).ChartType = SeriesChartType.Pie

        'grafico.SaveImage(memoryStream, ChartImageFormat.Png)

        'Dim img As Image = Image.GetInstance(memoryStream.GetBuffer)


        'Doc.Add(img)
        Dim areaGrafico As New ChartArea()
        areaGrafico.Name = "Area1"

        Dim Chart1 As New Chart()
        Chart1.Width = 450
        Chart1.Height = 300
        Chart1.Titles.Add("Riepilogo")
        Chart1.ChartAreas.Add(areaGrafico)



        'Chart1.ChartAreas(areaGrafico.Name).AxisX.Title = "Employee"
        Chart1.ChartAreas(areaGrafico.Name).AxisY.Title = "N° Atti"

        'Chart1.ChartAreas(areaGrafico.Name).BackColor = System.Drawing.Color.Azure

        Dim datiPerIlGrafico = New List(Of ItemReportGraficoAtti)


        Dim itemTotale As New ItemReportGraficoAtti
        itemTotale.Descrizione = "Totale Atti"
        itemTotale.TotaleAtti = totPrimaCol
        itemTotale.TotaleDetermine = totSecondaCol
        itemTotale.TotaleDisposizioni = totTerzaCol


        Dim itemTransitati As New ItemReportGraficoAtti
        itemTransitati.Descrizione = "Atti transitati"
        itemTransitati.TotaleAtti = totPrimaColTransitatiIn
        itemTransitati.TotaleDetermine = totSecondaColTransitatiIn
        itemTransitati.TotaleDisposizioni = totTerzaColTransitatiIn

        Dim itemOsservazioni As New ItemReportGraficoAtti
        If siglaUfficio <> "UCA" Then
            itemOsservazioni.Descrizione = "Atti con osservazioni"
            itemOsservazioni.TotaleAtti = totPrimaColConOsservazioni
            itemOsservazioni.TotaleDetermine = totSecondaColConOsservazioni
            itemOsservazioni.TotaleDisposizioni = totTerzaColConOsservazioni
        End If
        Dim itemRigettai As New ItemReportGraficoAtti
        itemRigettai.Descrizione = "Atti rigettati"
        itemRigettai.TotaleAtti = totPrimaColRigettati
        itemRigettai.TotaleDetermine = totSecondaColRigettati
        itemRigettai.TotaleDisposizioni = totTerzaColRigettati


        datiPerIlGrafico.Add(itemTotale)
        datiPerIlGrafico.Add(itemTransitati)

        If siglaUfficio <> "UCA" Then
            datiPerIlGrafico.Add(itemOsservazioni)
        End If
        datiPerIlGrafico.Add(itemRigettai)


        Chart1.DataBindTable(datiPerIlGrafico, "Descrizione")

        Chart1.Legends.Add(New Legend("Legenda1"))
        Chart1.Legends("Legenda1").Title = "Legenda"
        Chart1.Series("TotaleAtti").LegendText = "Totale"
        Chart1.Series("TotaleDetermine").LegendText = "Determine"
        Chart1.Series("TotaleDisposizioni").LegendText = "Disposizioni"

        Dim memoryStream As New MemoryStream()
        Chart1.SaveImage(memoryStream, ChartImageFormat.Png)

        Dim img As Image = Image.GetInstance(memoryStream.GetBuffer)
        Doc.Add(img)


        Doc.Close()
        Writer.Close()

        Dim pdfContent As Byte() = ms.GetBuffer()

        context.Response.ContentType = "application/pdf"

        context.Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf")
        context.Response.OutputStream.Write(pdfContent, 0, pdfContent.Length)
    End Sub

End Class
