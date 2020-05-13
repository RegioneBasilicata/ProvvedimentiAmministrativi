Imports Microsoft.Office.Interop
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.IO.File
Imports System.Runtime.Remoting.Contexts
Imports DllDocumentale.Model


Partial Class AnteprimaAllegatoCommand
    Implements ICommand

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AnteprimaAllegatoCommand))
    Private indexAllegato As String = ""
    Dim codAllegato As String
    Dim docP7m As String
    Dim docTsr As String

    Dim idDocFromAllegato As String = ""



    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute



        Select Case context.Application.Item("NOME_ENTE_INSTALLAZIONE")
            Case "REGIONE"
                Execute_Regione(context)
            Case "ALSIA"
                Execute_Alsia(context)

            Case Else
                Execute_Generale(context)
        End Select



    End Sub
    Protected Sub Execute_Generale(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim idTestoDocumento As String = ""
        Dim binarioFile() As Byte
        Dim docP7m As String
        Dim docTsr As String

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
        docP7m = context.Request.QueryString.Get("docP7m")

        docTsr = context.Request.QueryString.Get("docTsr")


        vettoredati = Anteprima_Allegato(codAllegato, docP7m, docTsr)
        idDocFromAllegato = vettoredati(7) & ""


        Log.Debug("ANTEPRIMA ALLEG - Id Allegato: " & codAllegato)
        Log.Debug("ANTEPRIMA ALLEG - Rit. Anteprima_Allegato: " & vettoredati(0))
        Try
            If context.Request.QueryString.Item("idx") <> "" Then
                indexAllegato = context.Request.QueryString.Item("idx")
            End If
            If context.Request.QueryString.Item("pdf") = 1 Then
               
                If vettoredati(3) <> "pdf" Then

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
    
    Private Sub printWord(ByRef vett As Object, ByVal context As HttpContext)

        Dim nome As String = vett(2)
        nome = nome.Replace("/", "_").Replace("\", "_")
        Dim estensione As String = vett(3)
        Dim path As String = "" & AppDomain.CurrentDomain.BaseDirectory & "temp\" & nome & "." & estensione
        path = path.Replace("/", "\")
        Dim coduff As String = ""
        Dim desuff As String = ""
        Dim pathPdfMod As String = ""
        Dim lstrIdDoc As String = "idAll" & vett(7)
        Try
            Dim fileOriginale As New FileInfo(path)
            If fileOriginale.Exists Then
                Try
                    fileOriginale.Delete()
                    fileOriginale = Nothing
                Catch ex As Exception
                    Log.Error("Impossibile cancellare " & path)
                End Try
            End If

            Dim fileSys As New FileStream(path, FileMode.OpenOrCreate)

            Dim arrByte As Byte() = DirectCast(vett(1), Byte())
            fileSys.Write(arrByte, 0, arrByte.Length - 1)
            fileSys.Close()
            Dim ope As DllAmbiente.Operatore = context.Session("oOperatore")

            If estensione <> "pdf" Then

                Dim pathpdf As String = path & ".pdf"

                pathPdfMod = pathpdf.Substring(0, pathpdf.Length - 4) & ope.Codice & lstrIdDoc & "mod.pdf"

                If context.Request.QueryString("prew") <> "1" Then
                    'LU 11
                    'Commentato per forzare la gnerazione del PDF
                    'Dim fileInf As New FileInfo(pathPdfMod)

                    'If fileInf.Exists Then
                    '    vett(2) = fileInf.Name.Replace(ope.Codice & lstrIdDoc & "mod.pdf", ".pdf")
                    '    vett(3) = "pdf"
                    '    vett(4) = "application/pdf"

                    '    Dim fileStr As FileStream = fileInf.OpenRead
                    '    ReDim arrByte(fileStr.Length)
                    '    fileStr.Read(arrByte, 0, fileInf.Length - 1)
                    '    fileStr.Close()
                    '    vett(1) = arrByte
                    '    Exit Sub
                    'End If

                End If


                'creo l'oggetto con il metodo CreateObject
                Dim myword As New Word.Application
                'Non rendo visibile Word
                myword.Visible = False

                'Apro il documento che intendo stampare
                myword.Documents.Open(path)

                Dim filePDFvuoto As New FileInfo(pathpdf)
                If filePDFvuoto.Exists Then
                    Try
                        filePDFvuoto.Delete()
                        filePDFvuoto = Nothing
                    Catch ex As Exception
                        Log.Error("Impossibile cancellare " & pathpdf)
                    End Try
                End If

                myword.ActiveDocument.SaveAs(pathpdf, Word.WdSaveFormat.wdFormatPDF)
                'Esco da Word
                myword.ActiveDocument.Close()

                myword.Quit(0)
                myword = Nothing
                GC.Collect()
                path = pathpdf



                Dim fil As New FileInfo(path)

                Dim nomefile As String = fil.Name.Substring(0, fil.Name.Length - 4)


                ' vett(2) = CStr(vett(2)) & "." & vett(3)
                vett(2) = nomefile
                vett(3) = "pdf"
                vett(4) = "application/pdf"
                'vett(1) = InserisciValore(context, path, pathPdfMod)

                vett(1) = ReadAllBytes(path)
                vett(1) = CreaDocumento(vett, context, True)

            Else
                'pathPdfMod = path.Substring(0, path.Length - 4) & ope.Codice & lstrIdDoc & "mod.pdf"

                vett(1) = ReadAllBytes(path)
                vett(1) = CreaDocumento(vett, context, True)


            End If





        Catch ex As Exception
            Log.Debug("printWord --> " + ex.Message)
            Throw ex
        End Try


    End Sub
End Class
