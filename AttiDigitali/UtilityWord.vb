Imports Microsoft.Office.Interop
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO


Public Class UtilityWord
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(UtilityWord))

    Function TrasformaDocInpdfBynary(ByRef byteDocFile As Byte()) As Byte()
        Dim temFileName As String = System.IO.Path.GetTempFileName()
        Dim arrBytepdfReturn As Byte() = Nothing

        Dim coduff As String = ""
        Dim desuff As String = ""
        Dim pathPdfMod As String = ""

        Try


            Dim fileSys As New FileStream(temFileName, FileMode.OpenOrCreate)


            fileSys.Write(byteDocFile, 0, byteDocFile.Length - 1)
            fileSys.Close()
            fileSys = Nothing


            Dim pathpdf As String = System.IO.Path.GetTempFileName()




            'creo l'oggetto con il metodo CreateObject
            Dim myword As New Word.Application
            'Non rendo visibile Word
            myword.Visible = False

            'Apro il documento che intendo stampare
            myword.Documents.Open(temFileName)


            myword.ActiveDocument.SaveAs(pathpdf, Word.WdSaveFormat.wdFormatPDF)
            'Esco da Word
            myword.ActiveDocument.Close()

            myword.Quit(0)
            myword = Nothing
            Dim fileSysPdf As New FileStream(pathpdf, FileMode.Open)
            Dim arraylength As Integer = fileSysPdf.Length
            ReDim arrBytepdfReturn(arraylength)
            fileSysPdf.Read(arrBytepdfReturn, 0, arraylength - 1)
            fileSysPdf.Close()
            fileSysPdf = Nothing
        Catch ex As Exception
            Log.Error("Impossibile trasformare il doc in pdf " & ex.Message)
        End Try


        GC.Collect()

        Return arrBytepdfReturn


    End Function
    Function exportRicercaToExcel_old(ByRef Response As Web.HttpResponse, ByRef dati As Object)
        Response.Clear()
        Dim FileName

        FileName = "ricerca.xsl"

        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"

        Response.AddHeader("content-disposition", "inline; filename=" & FileName)
        Response.ContentEncoding = System.Text.Encoding.Unicode
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        Response.Write("<html>")
        Response.Write("<body>")
        Response.Write("<table>")

        For ii As Integer = 0 To UBound(dati(1), 2)


            Response.Write("<tr>")
            For i As Integer = 1 To 4
                Response.Write("<td  valign=top >")
                Response.Write(dati(1)(i, ii))
                Response.Write("</td>")
            Next
            Response.Write("</tr>")
        Next

        Response.Write("</table>")
        Response.Write("</body>")
        Response.Write("</html>")
        Response.Flush()
        Response.End()

    End Function

    'Function exportRicercaToExcel(ByRef Response As Web.HttpResponse, ByVal dati As System.Web.UI.WebControls.Table, ByRef pagina As Page)
    '    Response.Clear()
    '    Dim FileName

    '    FileName = "ricerca.xsl"

    '    Response.Buffer = True
    '    Response.ContentType = "application/vnd.ms-excel"

    '    Response.AddHeader("content-disposition", "attachment; filename=" & FileName)
    '    Response.ContentEncoding = System.Text.Encoding.Unicode
    '    Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())


    '    'Dim stwWriter As New System.IO.StringWriter
    '    'Dim htwHtmlTextWriter As New System.Web.UI.HtmlTextWriter(stwWriter)


    '    'dati.RenderControl(htwHtmlTextWriter)
    '    'Response.Write(stwWriter.ToString())




    '    Response.Write("<html>")
    '    Response.Write("<body>")
    '    Response.Write("<table>")

    '    For riga As Integer = 0 To dati.Rows.Count - 1


    '        Response.Write("<tr>")
    '        For cella As Integer = 0 To dati.Rows(riga).Cells.Count - 1
    '            Response.Write("<td  valign=top >")
    '            If dati.Rows(riga).Cells(cella).Controls.Count = 1 AndAlso dati.Rows(riga).Cells(cella).Controls(0).GetType Is GetType(LiteralControl) Then
    '                Dim ctrl As LiteralControl = CType(dati.Rows(riga).Cells(cella).Controls(0), LiteralControl)
    '                If ctrl.Text.Contains("blank.gif") Then
    '                    Response.Write("<td></td>")
    '                ElseIf ctrl.Text.Contains("<img") Then
    '                    Dim lstra As String = ctrl.Text.Substring(ctrl.Text.ToLower.IndexOf("<img"))
    '                    'lstra = lstra.Substring(0, lstra.LastIndexOf("</img"))
    '                    If lstra.LastIndexOf("title=") <> -1 Then
    '                        lstra = lstra.Substring(lstra.IndexOf("alt="), lstra.LastIndexOf("title=") - lstra.LastIndexOf("alt="))
    '                    ElseIf lstra.LastIndexOf("onclick=") <> -1 Then
    '                        lstra = lstra.Substring(lstra.IndexOf("alt="), lstra.LastIndexOf("onclick=") - lstra.LastIndexOf("alt="))
    '                    End If
    '                    lstra = lstra.Replace("alt=", "")
    '                    Response.Write(lstra)
    '                Else
    '                    Response.Write(ctrl.Text)
    '                End If

    '            Else
    '                Response.Write(dati.Rows(riga).Cells(cella).Text)

    '            End If
    '            Response.Write("</td>")
    '        Next
    '        Response.Write("</tr>")
    '    Next

    '    Response.Write("</table>")
    '    Response.Write("</body>")
    '    Response.Write("</html>")
    '    Response.Flush()
    '    Response.End()

    'End Function
    Sub exportRicercaToCSV(ByRef Response As Web.HttpResponse, ByVal dati As System.Web.UI.WebControls.Table, ByRef pagina As Page)
        Dim lstra As String = ""
        Dim FileName
        FileName = "ricerca.csv"

        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" & FileName)
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252")
        HttpContext.Current.Response.ContentType = "text/csv"
        HttpContext.Current.Response.AddHeader("Pragma", "public")

        For riga As Integer = 0 To dati.Rows.Count - 1

            For cella As Integer = 0 To dati.Rows(riga).Cells.Count - 1
                If dati.Rows(riga).Cells(cella).Controls.Count = 1 AndAlso dati.Rows(riga).Cells(cella).Controls(0).GetType Is GetType(LiteralControl) Then
                    Dim ctrl As LiteralControl = CType(dati.Rows(riga).Cells(cella).Controls(0), LiteralControl)
                    If ctrl.Text.Contains("blank.gif") Then
                        Response.Write("")
                    ElseIf ctrl.Text.Contains("<a href") Then
                        Dim indiceFinale As Integer = ctrl.Text.ToLower.LastIndexOf("</a>")
                        Dim stringa As String = ctrl.Text.Substring(ctrl.Text.ToLower.IndexOf("<a href"), (ctrl.Text.Length - (ctrl.Text.Length - indiceFinale)))
                        Dim indiceIniziale As Integer = stringa.LastIndexOf(">")
                        lstra = stringa.Substring((indiceIniziale + 1))
                        Response.Write(lstra)
                    ElseIf ctrl.Text.Contains("<input") Then
                        Response.Write(";")
                    ElseIf ctrl.Text.Contains("<img") Then
                        lstra = ctrl.Text.Substring(ctrl.Text.ToLower.IndexOf("<img"))
                        If lstra.LastIndexOf("title=") <> -1 Then
                            lstra = lstra.Substring(lstra.IndexOf("alt="), lstra.LastIndexOf("title=") - lstra.LastIndexOf("alt="))
                        ElseIf lstra.LastIndexOf("onclick=") <> -1 Then
                            lstra = lstra.Substring(lstra.IndexOf("alt="), lstra.LastIndexOf("onclick=") - lstra.LastIndexOf("alt="))
                        End If
                        lstra = lstra.Replace("alt=", "")
                        Response.Write(lstra)
                    Else
                        Response.Write(ctrl.Text)
                    End If

                Else
                    Response.Write(dati.Rows(riga).Cells(cella).Text)

                End If

                Response.Write(";")
            Next

            Response.Write(Environment.NewLine)
        Next

        Response.Flush()
        Response.End()

    End Sub
End Class
