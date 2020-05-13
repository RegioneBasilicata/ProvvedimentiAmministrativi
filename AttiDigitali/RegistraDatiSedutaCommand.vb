Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports DllDocumentale

Public Class RegistraDatiSedutaCommand
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
            Dim codDocumento As String = context.Session.Item("key")

            Dim itemDatiSeduta As ItemDatiSedutaInfo = context.Session("itemDatiSeduta")




            suffix = ".success"




            Dim msgEsito As String = ""

            'vR = Crea_ODG()




            Dim dlldoc As New DllDocumentale.svrDocumenti(context.Session.Item("oOperatore"))

            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


            Dim itemDatiSedutaInfo As ItemDatiSedutaInfo = itemDatiSeduta
            If Not itemDatiSeduta Is Nothing Then



                Dim itemDatiSedutaInfoDB As ItemDatiSedutaInfo = dlldoc.FO_Get_DatiSedutaRelatoreInfo(itemDatiSeduta.DocId)
                If (Not itemDatiSedutaInfoDB Is Nothing) Then
                    dlldoc.FO_Delete_Info_DatiSedutaRelatoriDocumento(oOperatore, itemDatiSeduta.DocId)
                    dlldoc.FO_Insert_DatiSedutaRelatoriDocumento(oOperatore, itemDatiSedutaInfo)
                Else
                    dlldoc.FO_Insert_DatiSedutaRelatoriDocumento(oOperatore, itemDatiSedutaInfo)
                End If
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
