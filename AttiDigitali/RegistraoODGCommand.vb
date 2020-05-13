Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class RegistraoODGCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)


        Dim vR As Object = Nothing
        'Dim itemODG As String = context.Session("itemODG")
        'scheda legge trasparenza
        Dim itemODG As DllDocumentale.ItemODGInfo = context.Session("itemODG")
        If Not itemODG Is Nothing Then
            'If (dlldoc.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, codDocumento) Is Nothing) Then
            '    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
            'Else
            '    dlldoc.FO_Update_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
            'End If
        Else
            'dlldoc.FO_Delete_Info_Scheda_Legge_Trasparenza(oOperatore, codDocumento)
        End If



        'vR = Registra_ODG(itemODG)

        'If vR(0) = 0 Then

        '    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        '    Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)
        '    Dim listaStrutturaUfficio As New List(Of DllAmbiente.StrutturaInfo)
        '    Dim ufficioStruttura As New DllAmbiente.StrutturaInfo


        '    Dim ufficiDiCompetenza As String = context.Session("ufficiDiCompetenza")


        '    For Each codUfficio As String In ufficiDiCompetenza.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        '        ufficioStruttura = New DllAmbiente.StrutturaInfo
        '        ufficioStruttura.CodiceInterno = codUfficio
        '        If Not listaStrutturaUfficio.Contains(ufficioStruttura) Then
        '            listaStrutturaUfficio.Add(ufficioStruttura)
        '        End If

        '    Next

        '    Dim result As String = dlldoc.FO_Insert_Documento_Uffici_Competenza(codDocumento, oOperatore.Codice, listaStrutturaUfficio)

        '    Dim valoriAttributi As String = context.Session("valoriAttributi")
        '    valoriAttributi = "[" & valoriAttributi & "]"
        '    Dim listaAttributi As List(Of Ext_AttributoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(valoriAttributi, GetType(List(Of Ext_AttributoInfo))), List(Of Ext_AttributoInfo))
        '    Dim item As DllDocumentale.Documento_attributo
        '    For Each att As Ext_AttributoInfo In listaAttributi
        '        If Not (String.IsNullOrEmpty(att.Valore)) Then
        '            item = New DllDocumentale.Documento_attributo
        '            item.Cod_attributo = att.ID
        '            item.Doc_id = codDocumento
        '            item.Valore = att.Valore
        '            item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
        '            dlldoc.FO_Registra_Attributo(item, oOperatore)
        '        End If
        '    Next

        '    'scheda legge trasparenza
        '    Dim itemSchedaLeggeTrasparenzaInfo As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = CreaDocumentoCommand.getSchedaLeggeTrasparenzaInfo(codDocumento, context)
        '    If Not itemSchedaLeggeTrasparenzaInfo Is Nothing Then
        '        If (dlldoc.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, codDocumento) Is Nothing) Then
        '            dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
        '        Else
        '            dlldoc.FO_Update_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
        '        End If
        '    Else
        '        dlldoc.FO_Delete_Info_Scheda_Legge_Trasparenza(oOperatore, codDocumento)
        '    End If

        '    'scheda tipologia provvedimento
        '    Dim itemSchedaTipologiaProvvedimentoInfo As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = CreaDocumentoCommand.getSchedaTipologiaProvvedimentoInfo(codDocumento, context)
        '    If Not itemSchedaTipologiaProvvedimentoInfo Is Nothing Then
        '        If (dlldoc.FO_Get_SchedaTipologiaProvvedimentoInfo(oOperatore, codDocumento) Is Nothing) Then
        '            dlldoc.FO_Insert_Info_Scheda_Tipologia_Provvedimento(oOperatore, itemSchedaTipologiaProvvedimentoInfo)
        '        Else
        '            dlldoc.FO_Update_Info_Scheda_Tipologia_Provvedimento(oOperatore, itemSchedaTipologiaProvvedimentoInfo)
        '        End If
        '    Else
        '        dlldoc.FO_Delete_Info_Scheda_Tipologia_Provvedimento(oOperatore, codDocumento)
        '    End If

        '    dlldoc.Messagio_WebServiceNotifica(codDocumento, DllDocumentale.svrDocumenti.Stato_Notifica.Modificato, -1, "Modificati Attributi Provvedimento")


        'End If

        'vR = Leggi_Documento(codDocumento)




        'datiXml = ""
        'oggetto = ""
        'pubIntegrale = 0
        'If vR(0) = 0 Then
        '    'datiXml = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_xmlDatiDocumento)
        '    oggetto = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto)
        '    testo = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_testo)
        '    numeroDoc = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_numUtenteDocumento)
        '    pubIntegrale = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_pubIntegrale)

        '    intOpContabili = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_isContabile)

        'End If




        'context.Items.Add("intOpContabili", intOpContabili)
        'context.Items.Add("xmlDati", datiXml)
        'context.Items.Add("oggetto", oggetto)
        'context.Items.Add("pubIntegrale", pubIntegrale)
        'context.Session.Add("key", codDocumento)
        'context.Items.Add("testoEditato", testo)
        'context.Items.Add("numeroDoc", numeroDoc)
        'context.Items.Add("tipoOpContabili", tipoOpContabili)
    End Sub
End Class
