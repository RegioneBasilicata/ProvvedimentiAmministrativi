Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports DllDocumentale

Public Class CreaODGCommand
    Implements ICommand
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(CreaDocumentoCommand))
    Protected Sub Execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim url As String
        Dim suffix As String
        Dim lstr_Key As String = ""
        Dim action As String = context.Request.Params("PATH_INFO")
        action = action.ToLower.Replace(ConfigurationManager.AppSettings("replaceKey").ToLower, "/AttiDigitali/")


        Dim tipoApp As String = ""
        Try


            Dim vR As New Object
            tipoApp = TipoApplic(context)

            
            Dim itemODG As ItemODGInfo = context.Session("itemODG")
            


            
            suffix = ".success"


            
            
            Dim msgEsito As String = ""

            'vR = Crea_ODG()
           



            Dim dlldoc As New DllDocumentale.svrDocumenti(context.Session.Item("oOperatore"))

            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            Dim itemoODGInfo As ItemODGInfo = itemODG
            If Not itemoODGInfo Is Nothing Then
                'dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, ItemSchedaLeggeTrasparenzaInfo)
                dlldoc.FO_Insert_ODG(oOperatore, itemODG)
                'dlldoc()
                'For Each delibera As ItemDocumentoInfo In ItemODGInfo.Delibere
                '    FO_Insert_Delibera_Seduta(delibera, trans)
                'Next
            End If



            HttpContext.Current.Session.Add("msgEsito", msgEsito)

        Catch ex As Exception
            suffix = ".failure"
        End Try

        url = ActionMappings.getInstance.mappings.Get(action + suffix)
        'controllo se c'è un default
        If url Is Nothing Then
            url = ActionMappings.getInstance.mappings.Get("*" + suffix)
        End If

        If lstr_Key <> "" Then
            url = url & "?codODG=" & lstr_Key
            context.Session.Add("codODG", lstr_Key)
        End If
        context.Response.Redirect(url)

    End Sub

    
End Class
