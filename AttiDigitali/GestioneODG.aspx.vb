Imports System.ServiceModel
Imports DllDocumentale
Imports System.Collections.Generic
Imports System.Net

Partial Public Class GestioneODG
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Gestione Ordine del Giorno")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form("chkSalva") & "" = "1" Then
            Try
                Dim str_return As String = CreaODG()
                Response.Write("{success:true,link:'" & str_return & "'}     ")
                Response.Flush()
                Response.Close()

            Catch ex As Exception
                Response.Clear()
                Response.ClearHeaders()

                Dim exf As New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)

                Response.Write("{success:false,FaultMessage:'" & exf.Message.Replace("'", "''") & "'} ")
                Response.Flush()
                Response.Close()
            End Try
        End If

    End Sub

    Protected Function CreaODG() As String
        
        
        
        Dim itemODG As New ItemODGInfo
        Dim dataSeduta As String = CStr(Context.Request.Form("DataSeduta"))
        Dim oraSeduta As String = CStr(Context.Request.Form("OraSeduta"))

        itemODG.DataSeduta = dataSeduta
        itemODG.OraSeduta = oraSeduta

        If Not Context.Request.Form("listaDelibere") Is Nothing Then
            Dim listaDelibere As String = htmlDecode(HttpContext.Current.Request.Item("listaDelibere"))
            listaDelibere = "[" & listaDelibere & "]"

            Dim delibere As List(Of Ext_DocumentoInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaDelibere, GetType(List(Of Ext_DocumentoInfo))), List(Of Ext_DocumentoInfo))
            For Each delibera As Ext_DocumentoInfo In delibere
                Dim itemDocumentiInfo As ItemDocumentoInfo = New ItemDocumentoInfo()
                itemDocumentiInfo.IdDocumento = delibera.Doc_Id

                itemODG.Delibere.Add(itemDocumentiInfo)
            Next
        End If


        

        Context.Session.Add("itemODG", itemODG)

        Dim lstr_link As String = "CreaODGAction.aspx"
        'Session.Remove("key")

        'If Trim(Context.Request.QueryString.Get("key")) <> "" Then
        '    Session.Add("key", Context.Request.QueryString.Get("key"))
        '    Select Case tipoApp
        '        Case 0
        '            lstr_link = "CreaODGCommand.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '        Case 1
        '            lstr_link = "RegistraDeliberaAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '        Case 2
        '            lstr_link = "RegistraDisposizioneAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
        '    End Select



        'End If

        Return lstr_link
    End Function

    Private Function htmlDecode(ByVal value As Object)
        Dim retValue As Object = Nothing
        If Not value Is Nothing Then
            retValue = HttpUtility.HtmlDecode(value)
        End If
        Return retValue
    End Function
End Class