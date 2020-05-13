Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class RegistraDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String
        Dim datiXml As String = context.Session.Item("datiXml")
        Dim oggetto As String = context.Session.Item("txtOggetto")
        Dim pubIntegrale As Integer = context.Session.Item("pubIntegrale")
        Dim vR As Object = Nothing
        Dim testo As String = ""
        Dim numeroDoc As String = ""        

        Dim tipoOpContabili As String = context.Session("tipoOpContabili")

        Dim intOpContabili As Integer = False

        If Not context.Session("intOpContabili") Is Nothing Then
            intOpContabili = CInt(context.Session("intOpContabili"))

        End If

        codDocumento = context.Session.Item("key")
        If Not IsNumeric(pubIntegrale) Then
            pubIntegrale = 0
        End If

        'comm per eliminazione xml
        'oggetto = oggetto.Replace(Chr(38), "&amp;")
        'oggetto = oggetto.Replace(Chr(39), "&aps;")
        'oggetto = oggetto.Replace(Chr(62), "&gt;")
        'oggetto = oggetto.Replace(Chr(60), "&lt;")
        'oggetto = oggetto.Replace(Chr(34), "&quot;")

        Dim key As String = ""
        key = context.Request.QueryString("key")
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(context.Session.Item("oOperatore"))).Get_StatoIstanzaDocumento(key)
        Dim oOp As DllAmbiente.Operatore = context.Session.Item("oOperatore")

        Dim flussoDocumentale As String = String.Empty
        Select Case context.Request.QueryString("tipo")
            Case 0
                flussoDocumentale = "DETERMINE"
            Case 1
                flussoDocumentale = "DELIBERE"
            Case 2
                flussoDocumentale = "DISPOSIZIONI"
        End Select

        'Modifica 28/03/2014
        If (statoIstanza.LivelloUfficio = "UAR") And ((oOp.Test_Ruolo("TT001") And flussoDocumentale = "DETERMINE") Or (oOp.Test_Ruolo("TS001") And flussoDocumentale = "DISPOSIZIONI") Or (oOp.Test_Ruolo("TL001") And flussoDocumentale = "DELIBERE")) Then
            'UPDATE scheda legge trasparenza
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
            Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)

            Dim itemSchedaLeggeTrasparenzaOLD As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = dlldoc.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, codDocumento)

            Dim itemSchedaLeggeTrasparenzaNEW As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = CreaDocumentoCommand.getSchedaLeggeTrasparenzaInfo(codDocumento, context)
            If Not itemSchedaLeggeTrasparenzaNEW Is Nothing Then
                If (dlldoc.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, codDocumento) Is Nothing) Then
                    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    'If itemSchedaLeggeTrasparenzaNEW.Contratti.Count > 0 Then
                    '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    'End If
                Else
                    dlldoc.FO_Update_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(oOperatore, itemSchedaLeggeTrasparenzaNEW)

                    ' se ci sono contratti, aggiorno lo storico con i nuovi
                    'If itemSchedaLeggeTrasparenzaNEW.Contratti.Count > 0 Then
                    '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    'ElseIf itemSchedaLeggeTrasparenzaOLD.Contratti.Count > 0 Then
                    '    'se, invece, precedentemente c'erano dei contratti nel db,
                    '    ' registro che sono stati eliminati tutti gli esistenti
                    '    ' altrimenti, non inserisco nessun record nello storico, 
                    '    ' perchè i contratti non sono mai stati inseriti
                    '    Dim contratto As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader
                    '    contratto.IdDocumento = itemSchedaLeggeTrasparenzaNEW.IdDocumento
                    '    contratto.IdContratto = "Rimossi tutti i contratti"
                    '    itemSchedaLeggeTrasparenzaNEW.Contratti.Add(contratto)
                    '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaNEW)
                    'End If


                End If
            Else
                dlldoc.FO_Delete_Info_Scheda_Legge_Trasparenza(oOperatore, codDocumento)
            End If

            Dim itemSchedaContrattiFattureOLD As DllDocumentale.ItemSchedaContrattiFattureInfo = dlldoc.FO_Get_SchedaContrattiFattureInfo(oOperatore, codDocumento)
            Dim itemSchedaContrattiFattureNEW As DllDocumentale.ItemSchedaContrattiFattureInfo = CreaDocumentoCommand.getSchedaContrattiFattureInfo(codDocumento, context)

            If Not itemSchedaContrattiFattureNEW Is Nothing Then

                If (dlldoc.FO_Get_SchedaContrattiFattureInfo(oOperatore, codDocumento) Is Nothing) Then

                    dlldoc.FO_Insert_Info_Scheda_Contratti_Fatture(oOperatore, itemSchedaContrattiFattureNEW)

                    If itemSchedaContrattiFattureNEW.Contratti.Count > 0 Then
                        dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureNEW)
                    End If

                Else
                    dlldoc.FO_Update_Info_Scheda_Contratti_Fatture(oOperatore, itemSchedaContrattiFattureNEW)
                    'dlldoc.FO_Insert_Info_Scheda_Contratti_Fatture_Storico(oOperatore, itemSchedaContrattiFattureNEW)

                    ' se ci sono contratti, aggiorno lo storico con i nuovi
                    If itemSchedaContrattiFattureNEW.Contratti.Count > 0 Then
                        dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureNEW)

                    ElseIf itemSchedaContrattiFattureOLD.Contratti.Count > 0 Then
                        'se, invece, precedentemente c'erano dei contratti nel db,
                        ' registro che sono stati eliminati tutti gli esistenti
                        ' altrimenti, non inserisco nessun record nello storico, 
                        ' perchè i contratti non sono mai stati inseriti
                        Dim contratto As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader
                        contratto.IdDocumento = itemSchedaContrattiFattureNEW.IdDocumento
                        contratto.IdContratto = "Rimossi tutti i contratti"
                        itemSchedaContrattiFattureNEW.Contratti.Add(contratto)
                        dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureNEW)
                    End If

                    ' se ci sono fatture, aggiorno lo storico con i nuovi
                    If itemSchedaContrattiFattureNEW.Fatture.Count > 0 Then
                        dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureNEW)

                    ElseIf itemSchedaContrattiFattureOLD.Fatture.Count > 0 Then
                        
                        Dim fattura As DllDocumentale.ItemFatturaInfoHeader = New DllDocumentale.ItemFatturaInfoHeader
                        fattura.IdDocumento = itemSchedaContrattiFattureNEW.IdDocumento
                        fattura.IdUnivoco = "Rimosse tutte le fatture"
                        itemSchedaContrattiFattureNEW.Fatture.Add(fattura)
                        dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureNEW)
                    End If
                End If
            End If
        Else

            vR = Registra_Documento(codDocumento, oggetto, , pubIntegrale, , intOpContabili, tipoOpContabili)

            If vR(0) = 0 Then
                context.Session.Add("RegistraOggetto", 1)
                'Lu 04/05/09  mod reg uffici
                If vR(0) = 0 Then
                    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
                    Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)

                    'inizio salvataggio Uffici per Notifica
                    Dim listaStrutturaUfficio As New List(Of DllAmbiente.StrutturaInfo)
                    Dim ufficioStruttura As New DllAmbiente.StrutturaInfo

                    Dim ufficiDiCompetenza As String = context.Session("ufficiDiCompetenza")
                    For Each codUfficio As String In ufficiDiCompetenza.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        ufficioStruttura = New DllAmbiente.StrutturaInfo
                        ufficioStruttura.CodiceInterno = codUfficio
                        If Not listaStrutturaUfficio.Contains(ufficioStruttura) Then
                            listaStrutturaUfficio.Add(ufficioStruttura)
                        End If
                    Next
                    Dim result As String = dlldoc.FO_Insert_Documento_Uffici_Competenza(codDocumento, oOperatore.Codice, listaStrutturaUfficio)
                    'fine salvataggio Uffici per Notifica

                    'inizio salvataggio Utenti per Notifica via e-mail
                    Dim listaUtentiUfficio As New List(Of String)
                    Dim utenteUfficio As String
                    Dim utentiDiCompetenza As String = context.Session("utentiDiCompetenza")
                    For Each utenteUfficio In utentiDiCompetenza.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        listaUtentiUfficio.Add(utenteUfficio)
                    Next
                    Dim resultInsertUtenteUfficio As String = dlldoc.FO_Insert_Documento_Utente_Uffici_Competenza(codDocumento, oOperatore.Codice, listaUtentiUfficio)
                    'fine salvataggio Utenti per Notifica via e-mail

                    Dim valoriAttributi As String = context.Session("valoriAttributi")
                    valoriAttributi = "[" & valoriAttributi & "]"
                    Dim listaAttributi As List(Of Ext_AttributoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(valoriAttributi, GetType(List(Of Ext_AttributoInfo))), List(Of Ext_AttributoInfo))
                    Dim item As DllDocumentale.Documento_attributo
                    If listaAttributi.Count > 0 Then
                        For Each att As Ext_AttributoInfo In listaAttributi
                            If Not (String.IsNullOrEmpty(att.Valore)) Then
                                item = New DllDocumentale.Documento_attributo
                                item.Cod_attributo = att.ID
                                item.Doc_id = codDocumento
                                item.Valore = att.Valore
                                item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                                dlldoc.FO_Registra_Attributo(item, oOperatore)
                            Else
                                item = New DllDocumentale.Documento_attributo
                                item.Cod_attributo = att.ID
                                item.Doc_id = codDocumento
                                item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                                dlldoc.FO_Delete_Documento_Attributi(item)
                            End If
                        Next
                    Else
                        item = New DllDocumentale.Documento_attributo
                        item.Cod_attributo = "CUP"
                        item.Doc_id = codDocumento
                        item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                        dlldoc.FO_Delete_Documento_Attributi(item)
                        item.Cod_attributo = "CIG"
                        dlldoc.FO_Delete_Documento_Attributi(item)
                    End If

                    'scheda legge trasparenza
                    Dim itemSchedaLeggeTrasparenzaInfo As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = CreaDocumentoCommand.getSchedaLeggeTrasparenzaInfo(codDocumento, context)
                    If Not itemSchedaLeggeTrasparenzaInfo Is Nothing Then
                        Dim itemSchedaLeggeTrasparenzaInfoDB As DllDocumentale.ItemSchedaLeggeTrasparenzaInfo = dlldoc.FO_Get_SchedaLeggeTrasparenzaInfo(oOperatore, codDocumento)
                        If (itemSchedaLeggeTrasparenzaInfoDB Is Nothing) Then
                            dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            'If itemSchedaLeggeTrasparenzaInfo.Contratti.Count > 0 Then
                            '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            'End If
                        Else
                            dlldoc.FO_Update_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            ' se ci sono contratti, aggiorno lo storico con i nuovi
                            'If itemSchedaLeggeTrasparenzaInfo.Contratti.Count > 0 Then
                            '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            'ElseIf itemSchedaLeggeTrasparenzaInfoDB.Contratti.Count > 0 Then
                            '    'se, invece, precedentemente c'erano dei contratti nel db,
                            '    ' registro che sono stati eliminati tutti gli esistenti
                            '    ' altrimenti, non inserisco nessun record nello storico, 
                            '    ' perchè i contratti non sono mai stati inseriti
                            '    Dim contratto As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader
                            '    contratto.IdDocumento = itemSchedaLeggeTrasparenzaInfo.IdDocumento
                            '    contratto.IdContratto = "Rimossi tutti i contratti"
                            '    itemSchedaLeggeTrasparenzaInfo.Contratti.Add(contratto)
                            '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                            'End If
                        End If
                    Else
                        dlldoc.FO_Delete_Info_Scheda_Legge_Trasparenza(oOperatore, codDocumento)
                    End If

                    'scheda contratti fatture
                    Dim itemSchedaContrattiFattureInfo As DllDocumentale.ItemSchedaContrattiFattureInfo = CreaDocumentoCommand.getSchedaContrattiFattureInfo(codDocumento, context)
                    If Not itemSchedaContrattiFattureInfo Is Nothing Then
                        Dim itemSchedaContrattiFattureInfoDB As DllDocumentale.ItemSchedaContrattiFattureInfo = dlldoc.FO_Get_SchedaContrattiFattureInfo(oOperatore, codDocumento)

                        If (itemSchedaContrattiFattureInfoDB Is Nothing) Then
                            dlldoc.FO_Insert_Info_Scheda_Contratti_Fatture(oOperatore, itemSchedaContrattiFattureInfo)
                            If itemSchedaContrattiFattureInfo.Contratti.Count > 0 Then
                                dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            End If

                            If itemSchedaContrattiFattureInfo.Fatture.Count > 0 Then
                                dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            End If

                        Else

                            dlldoc.FO_Update_Info_Scheda_Contratti_Fatture(oOperatore, itemSchedaContrattiFattureInfo)

                            If (itemSchedaContrattiFattureInfo.Contratti.Count > 0 And itemSchedaContrattiFattureInfoDB.Contratti.Count = 0) Then
                                dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            ElseIf (itemSchedaContrattiFattureInfoDB.Contratti.Count > 0 And itemSchedaContrattiFattureInfoDB.Contratti.Count > 0) Then
                                'STORICO CONTRATTI
                                Dim contratto As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader
                                contratto.IdDocumento = itemSchedaContrattiFattureInfo.IdDocumento
                                contratto.IdContratto = "Rimossi tutti i contratti"
                                itemSchedaContrattiFattureInfo.Contratti.Add(contratto)
                                dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            End If
                            If (itemSchedaContrattiFattureInfo.Fatture.Count > 0 And itemSchedaContrattiFattureInfoDB.Fatture.Count = 0) Then
                                dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            ElseIf (itemSchedaContrattiFattureInfo.Fatture.Count > 0 And itemSchedaContrattiFattureInfoDB.Fatture.Count > 0) Then
                                'STORICO FATTURE
                                'Dim fattura As DllDocumentale.ItemFatturaInfoHeader = New DllDocumentale.ItemFatturaInfoHeader
                                'fattura.IdDocumento = itemSchedaContrattiFattureInfo.IdDocumento
                                'fattura.IdUnivoco = "Rimossi tutte le fatture"
                                'itemSchedaContrattiFattureInfo.Fatture.Add(fattura)
                                'dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                            End If
                        End If
                    End If

                    'scheda tipologia provvedimento
                    Dim itemSchedaTipologiaProvvedimentoInfo As DllDocumentale.ItemSchedaTipologiaProvvedimentoInfo = CreaDocumentoCommand.getSchedaTipologiaProvvedimentoInfo(codDocumento, context)
                    If Not itemSchedaTipologiaProvvedimentoInfo Is Nothing Then
                        If (dlldoc.FO_Get_SchedaTipologiaProvvedimentoInfo(oOperatore, codDocumento) Is Nothing) Then
                            dlldoc.FO_Insert_Info_Scheda_Tipologia_Provvedimento(oOperatore, itemSchedaTipologiaProvvedimentoInfo)
                        Else
                            dlldoc.FO_Update_Info_Scheda_Tipologia_Provvedimento(oOperatore, itemSchedaTipologiaProvvedimentoInfo)
                        End If
                    Else
                        dlldoc.FO_Delete_Info_Scheda_Tipologia_Provvedimento(oOperatore, codDocumento)
                    End If

                    dlldoc.Messagio_WebServiceNotifica(codDocumento, DllDocumentale.svrDocumenti.Stato_Notifica.Modificato, -1, "Modificati Attributi Provvedimento")
                End If
                'Fine Lu 04/05/09  mod reg uffici

                'End If
                ' Modifica 28/03/2014    
            End If
            End If


            vR = Leggi_Documento(codDocumento)

            datiXml = ""
            oggetto = ""
            pubIntegrale = 0
            If vR(0) = 0 Then
                'datiXml = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_xmlDatiDocumento)
                oggetto = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto)
                testo = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_testo)
                numeroDoc = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_numUtenteDocumento)
                pubIntegrale = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_pubIntegrale)

                intOpContabili = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_isContabile)

            End If


            context.Items.Add("intOpContabili", intOpContabili)
            context.Items.Add("xmlDati", datiXml)
            context.Items.Add("oggetto", oggetto)
            context.Items.Add("pubIntegrale", pubIntegrale)
            context.Session.Add("key", codDocumento)
            context.Items.Add("testoEditato", testo)
            context.Items.Add("numeroDoc", numeroDoc)
            context.Items.Add("tipoOpContabili", tipoOpContabili)
    End Sub
End Class
