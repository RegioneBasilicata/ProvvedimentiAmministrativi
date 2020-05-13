Imports System.Collections.Generic
Imports DllDocumentale
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class DatiContabiliPDFCommand
    Implements ICommand

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(DatiContabiliPDFCommand))

    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim codDocumento As String = context.Request.Params("key")

        Dim hideBeneficiariWithDatiSensibiliInReportBeneficiari As Boolean = ConfigurationManager.AppSettings("HideBeneficiariWithDatiSensibiliInReportBeneficiari")

        Dim pdfBeneficiariProvider As DllArtifactProvider.PDFBeneficiariProvider = New DllArtifactProvider.PDFBeneficiariProvider()
        Dim pdfContent As Byte() = pdfBeneficiariProvider.getPDFBeneficiari(codDocumento, oOperatore, hideBeneficiariWithDatiSensibiliInReportBeneficiari)

        context.Response.ContentType = "application/pdf"

        context.Response.AddHeader("Content-Disposition", "attachment; filename=Beneficiari.pdf")
        context.Response.OutputStream.Write(pdfContent, 0, pdfContent.Length)
    End Sub

End Class
