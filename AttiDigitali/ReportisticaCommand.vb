Imports System.Collections.Generic
Imports DllDocumentale
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class ReportisticaCommand
    Implements ICommand

    Public Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim codDocumento As String = context.Session.Item("codDocumento")
        
        Dim responseMessage As String = ""
        Try
           


            'acquisisco la url da richiamare ricercando nei mappings configurati nel Web.config
            Dim action As String = context.Request.Params("PATH_INFO")
            action = action.ToLower.Replace(ConfigurationManager.AppSettings("replaceKey").ToLower, "/AttiDigitali/")

            Dim url = ActionMappings.getInstance.mappings.Get(action + ".success")
            'controllo se c'è un default
            If url Is Nothing Then
                responseMessage = "{success:false,message:""" & "Unable to get action url. Check application web.config file." & """}     "
            Else
                Dim descrizioneDocumento As String = ""

                Dim successMsg As String = ""

                If Not context.Request.QueryString.ToString Is Nothing And context.Request.QueryString.ToString <> "" Then
                    url = url & "?" & context.Request.QueryString.ToString

                End If
                'url = url & "?key=" & context.Session.Item("key")

                responseMessage = "{success:true,message:""" & successMsg & """, link:""" & url & """}     "

            End If
        Catch ex As Exception
            responseMessage = "{success:false,message:""" & ex.Message & "<br/> " & "Correggere l'errore e ricaricare il documento." & """}     "
        End Try

        context.Response.Clear()
        context.Response.ClearHeaders()

        context.Response.Write(responseMessage)
        context.Response.Flush()
        context.Response.Close()
    End Sub
End Class
