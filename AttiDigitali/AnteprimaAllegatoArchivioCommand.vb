Imports System.Configuration

Public Class AnteprimaAllegatoArchivioCommand
    Implements ICommand
    'modgg 10-06 7
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AnteprimaAllegatoArchivioCommand))
    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim codAllegato As String = context.Request.QueryString.Get("key")
        Dim ope As DllAmbiente.Operatore = context.Session("oOperatore")

        Dim ws As AttiDigitaliWS.ServiceMergePDF = New AttiDigitaliWS.ServiceMergePDF
        ws.Url = ConfigurationManager.AppSettings("AttiDigitaliWSStampe")


        If context.Request.QueryString.Item("prn") <> "" Then

            If context.Request.QueryString.Item("idop") & "" <> "" Then
                If ope Is Nothing Then
                    ope = New DllAmbiente.Operatore
                    ope.Codice = context.Request.QueryString.Item("idop") & ""
                End If
            End If


        End If


        Dim fraseInTrasparenza As String = ""
        If ope.Test_Gruppo("Politico") Then
            fraseInTrasparenza = "COPIA§§"

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("TRASPARENZA_POLITICO")) Then
                fraseInTrasparenza = ConfigurationManager.AppSettings("TRASPARENZA_POLITICO")
            End If

        ElseIf ope.Test_Gruppo("ArchGen") Then
            fraseInTrasparenza = "COPIA CONFORME§Archivio Generale§"

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("TRASPARENZA_ARCH")) Then
                fraseInTrasparenza = ConfigurationManager.AppSettings("TRASPARENZA_ARCH")
            End If

        Else
            fraseInTrasparenza = "COPIA§§"
        End If

            Dim file As Byte() = ws.GetFileToPrint(ope.Codice, "", fraseInTrasparenza, codAllegato)

            context.Response.AddHeader("Content-Disposition", "attachment; filename=" & codAllegato & ".pdf")
            context.Response.AddHeader("Content-Length", file.Length.ToString())
            context.Response.AddFileDependency(codAllegato & ".pdf")
            context.Response.ContentType = "application/pdf"
            context.Response.BinaryWrite(file)

            context.Response.Flush()
        context.Response.End()
        'context.Response.Close()
    End Sub


End Class
