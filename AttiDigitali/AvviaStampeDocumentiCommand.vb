Imports System.io
Public Class AvviaStampeDocumentiCommand
    Inherits RedirectingCommand
    'modgg16
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)

        ' codDocumento = (context.Request.UrlReferrer.Query).Substring(context.Request.UrlReferrer.Query.IndexOf("=") + 1)
        ' vettoredati = Elenco_Allegati(codDocumento)
      
        Dim shtml As String
        shtml = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"" > " _
        & "<html>" _
        & "<head>" _
        & "<title>Sistema di Gestione Atti Amministrativi</title>"
       
        Dim url As String = context.Request.Url.AbsoluteUri
        Dim indextofSlash As Integer = url.LastIndexOf("/")
        'url = url.Substring(0, indextofSlash) & "/formStampa.htm"
        url = url.Substring(0, indextofSlash) & "/formStampa.aspx"

        Dim rows As String = "50%"
      
        '   Dim vettSeparato As Object
        '  vettSeparato = separaArrayPerTipo(vettoredati(1), 1)
        ' Dim NumRows As Integer = CStr(UBound(vettSeparato, 1))
        'For i = 0 To NumRows
        'rows += ",50%"
        'Next
        rows = 100%
        Dim lstr_QueryString As String = ""
        'For i = 0 To NumRows
        ' 'Dim src As String = CStr(vettoredati(5))
        ' nomeframe = "all" & CStr(i)
        ''  Dim src As String = "Http://localhost/AttiDigitali/AnteprimaAllegatoAction.aspx?key=" & vettSeparato(0)(0, i) 'CStr(vettoredati(1)(0, 0))

        ''non commentare la riga successiva
        '' shtml += ("<P><A href=""" & src & """ name=""" & nomeframe & """ id=""" & nomeframe & """>" & nomeframe & "</A></P>")
        'lstr_QueryString += vettSeparato(i)(0, 0) & "-"
        'Next
      
        lstr_QueryString = context.Session("valoriSelezionati")
        context.Session.Remove("valoriSelezionati")
        If lstr_QueryString <> "" Then
            lstr_QueryString = "?ids=" & lstr_QueryString
        End If

        lstr_QueryString = lstr_QueryString & "&idDoc=" & context.Request.QueryString("idDoc")
        shtml += "<frameset rows=""" & rows & """> "
        '  shtml += ("<frame src=""Http://localhost/AttiDigitali/formStampa.htm" & lstr_QueryString & """ name=""principale"" id=""principale"">")
        shtml += ("<frame src=""" & url & lstr_QueryString & """ name=""principale"" id=""principale"">")

        'For i = 0 To NumRows
        '    'Dim src As String = CStr(vettoredati(5))
        '    nomeframe = "frame" & CStr(i)
        '    Dim src As String = "Http://localhost/AttiDigitali/AnteprimaAllegatoAction.aspx?key=" & vettSeparato(0)(0, i) 'CStr(vettoredati(1)(0, 0))

        '    'non commentare la riga successiva
        '     shtml += ("<frame src=""" & src & """ name=""" & nomeframe & """ id=""" & nomeframe & """>")

        'Next
        'shtml += ("<script language=""javascript""><!--")

        'shtml += " var listaAllegati= "

        'shtml += (" //--></script>")
        'For i = 0 To NumRows
        '    'Dim src As String = CStr(vettoredati(5))
        '    nomeframe = "all" & CStr(i)
        '    Dim src As String = "Http://localhost/AttiDigitali/AnteprimaAllegatoAction.aspx?key=" & vettSeparato(0)(0, i) 'CStr(vettoredati(1)(0, 0))

        '    'non commentare la riga successiva
        '    ' shtml += ("<P><A href=""" & src & """ name=""" & nomeframe & """ id=""" & nomeframe & """>" & nomeframe & "</A></P>")
        '    shtml += vettSeparato(0)(0, i) & ";"
        'Next

        shtml += ("</frameset>")
        shtml += "</html>"

        'context.Response.ContentType = vettoredati(4) & ""
        context.Response.Write(shtml)

        context.Response.Flush()
        context.Response.End()

    End Sub
End Class
