Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports DllDocumentale

Public Class CreaDocumentoCommand
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
            
            Dim vR As Object = Nothing
            tipoApp = TipoApplic(context)

            Dim txtOggetto As String = CStr(context.Session("txtOggetto"))
            Dim intOpContabili As Integer = CInt(context.Session("intOpContabili"))
            Dim tipoOpContabili As String = context.Session("tipoOpContabili")
            Dim ufficiDiCompetenza As String = context.Session("ufficiDiCompetenza")
            Dim utentiDiCompetenza As String = context.Session("utentiDiCompetenza")
            Dim valoriAttributi As String = context.Session("valoriAttributi")
            Dim dataCreazione As String = context.Session.Item("dataCreazione")
            Dim responsabileProcedimento As String = context.Session("responsabileProcedimento")


            If intOpContabili And (tipoApp = 0 Or tipoApp = 1) Then
                suffix = ".contabile"
            ElseIf Not intOpContabili And tipoApp = 0 Then
                suffix = ".success.determina"
            ElseIf Not intOpContabili And tipoApp = 1 Then
                suffix = ".success.delibera"
            End If

            If tipoApp = 2 Then
                suffix = ".disposizione"
            End If

            If tipoApp = 3 Then
                suffix = ".decreto"
            End If

            If tipoApp = 4 Then
                suffix = ".ordinanza"
            End If

            Dim pubIntegrale As Integer = CInt(context.Session("pubIntegrale"))
            ' Dim opSoggettoPor As Boolean = CBool(context.Request.Params.Item("opSoggettoPor"))
            context.Session.Remove("txtOggetto")
            context.Session.Remove("intOpContabili")
            context.Session.Remove("tipoPub")
            context.Session.Remove("tipoOpContabili")
            context.Session.Remove("ufficiDiCompetenza")
            context.Session.Remove("utentiDiCompetenza")
            context.Session.Remove("valoriAttributi")
            context.Session.Remove("dataCreazione")

            ' Dim urlChiamante As String = context.Request.UrlReferrer.Segments(context.Request.UrlReferrer.Segments.Length - 1)
            'If UCase(urlChiamante.Substring(0, 8)) = "CONFERMA" Or UCase(urlChiamante.Substring(0, 4)) = "CREA" Then
            Dim msgEsito As String = ""
            Select Case tipoApp
                Case 0
                    vR = Crea_Determina()
                    If vR(0) = 0 Then
                        lstr_Key = vR(2)
                        Registra_Documento(vR(2), txtOggetto, , pubIntegrale, , intOpContabili, tipoOpContabili)
                        msgEsito = "La determina n. " & vR(1) & " è stata creata con successo, adesso è a disposizione nella tua lista lavoro"
                    Else
                        msgEsito = "La determina non è stata creata."
                    End If
                Case 1
                    'vR = Crea_Delibera(ufficiDiCompetenza, dataCreazione)
                    vR = Crea_Delibera()
                    If vR(0) = 0 Then
                        lstr_Key = vR(2)
                        Registra_Documento(vR(2), txtOggetto, , pubIntegrale, , intOpContabili, tipoOpContabili)
                        msgEsito = "La delibera n. " & vR(1) & " è stata creata con successo, adesso è a disposizione nella tua lista lavoro"
                    Else
                        msgEsito = "La delibera non è stata creata."
                    End If
                Case 2
                    vR = Crea_Disposizione()
                    If vR(0) = 0 Then
                        lstr_Key = vR(2)
                        Registra_Documento(vR(2), txtOggetto, , , , intOpContabili, tipoOpContabili)
                        msgEsito = "La disposizione n. " & vR(1) & " è stata creata con successo, adesso è a disposizione nella tua lista lavoro"
                    Else
                        msgEsito = "La disposizione non è stata creata."
                    End If
                Case 3
                    vR = Crea_AltroAtto(3)
                    If vR(0) = 0 Then
                        lstr_Key = vR(2)
                        Registra_Documento(vR(2), txtOggetto, , , , intOpContabili, tipoOpContabili)
                        msgEsito = "Il decreto n. " & vR(1) & " è stata creata con successo, adesso è a disposizione nella tua lista lavoro"
                    Else
                        msgEsito = "Il decreto non è stata creata."
                    End If
                Case 4
                    vR = Crea_AltroAtto(4)
                    If vR(0) = 0 Then
                        lstr_Key = vR(2)
                        Registra_Documento(vR(2), txtOggetto, , , , intOpContabili, tipoOpContabili)
                        msgEsito = "L'ordinanza n. " & vR(1) & " è stata creata con successo, adesso è a disposizione nella tua lista lavoro"
                    Else
                        msgEsito = "L'ordinanza non è stata creata."
                    End If
            End Select


            'Lu 04/05/09  mod reg uffici
            If vR(0) = 0 Then

                Dim dlldoc As New DllDocumentale.svrDocumenti(context.Session.Item("oOperatore"))

                Dim listaStrutturaUfficio As New List(Of DllAmbiente.StrutturaInfo)
                Dim ufficioStruttura As New DllAmbiente.StrutturaInfo

                For Each codUfficio As String In ufficiDiCompetenza.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                    ufficioStruttura = New DllAmbiente.StrutturaInfo
                    ufficioStruttura.CodiceInterno = codUfficio
                    If Not listaStrutturaUfficio.Contains(ufficioStruttura) Then
                        listaStrutturaUfficio.Add(ufficioStruttura)
                    End If
                Next
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

                Dim result As String = dlldoc.FO_Insert_Documento_Uffici_Competenza(lstr_Key, oOperatore.Codice, listaStrutturaUfficio)


                'inizio salvataggio Utenti per Notifica via e-mail
                Dim listaUtentiUfficio As New List(Of String)
                Dim utenteUfficio As String
                For Each utenteUfficio In utentiDiCompetenza.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                    listaUtentiUfficio.Add(utenteUfficio)
                Next
                Dim resultInsertUtenteUfficio As String = dlldoc.FO_Insert_Documento_Utente_Uffici_Competenza(lstr_Key, oOperatore.Codice, listaUtentiUfficio)
                'fine salvataggio Utenti per Notifica via e-mail

                valoriAttributi = "[" & valoriAttributi & "]"
                Dim listaAttributi As List(Of Ext_AttributoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(valoriAttributi, GetType(List(Of Ext_AttributoInfo))), List(Of Ext_AttributoInfo))
                Dim item As DllDocumentale.Documento_attributo
                For Each att As Ext_AttributoInfo In listaAttributi
                    If Not att Is Nothing AndAlso Not (String.IsNullOrEmpty(att.Valore)) Then
                        item = New DllDocumentale.Documento_attributo
                        item.Cod_attributo = att.ID
                        item.Doc_id = lstr_Key
                        If att.TipoDato = "datetime" Then
                            item.Valore = att.Valore
                        Else
                            item.Valore = att.Valore
                        End If
                        item.Valore = att.Valore
                        item.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
                        dlldoc.FO_Registra_Attributo(item, oOperatore)
                    End If
                Next

                Dim itemSchedaLeggeTrasparenzaInfo As ItemSchedaLeggeTrasparenzaInfo = getSchedaLeggeTrasparenzaInfo(lstr_Key, context, True)
                If Not itemSchedaLeggeTrasparenzaInfo Is Nothing Then
                    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                    dlldoc.FO_Insert_Info_Scheda_Legge_Trasparenza_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                    'Modifica 01/04/2014
                    'If itemSchedaLeggeTrasparenzaInfo.Contratti.Count > 0 Then
                    '    dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaLeggeTrasparenzaInfo)
                    'End If
                End If

                Dim itemSchedaContrattiFattureInfo As ItemSchedaContrattiFattureInfo = getSchedaContrattiFattureInfo(lstr_Key, context, True)
                If Not itemSchedaContrattiFattureInfo Is Nothing Then

                    dlldoc.FO_Insert_Info_Scheda_Contratti_Fatture(oOperatore, itemSchedaContrattiFattureInfo)
                    If itemSchedaContrattiFattureInfo.Contratti.Count > 0 Then
                        dlldoc.FO_Insert_Contratto_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                    End If
                    If itemSchedaContrattiFattureInfo.Fatture.Count > 0 Then
                        dlldoc.FO_Insert_Fattura_Storico(oOperatore, itemSchedaContrattiFattureInfo)
                    End If

                End If

                Dim itemSchedaTipologiaProvvedimentoInfo As ItemSchedaTipologiaProvvedimentoInfo = getSchedaTipologiaProvvedimentoInfo(lstr_Key, context, True)
                If Not itemSchedaTipologiaProvvedimentoInfo Is Nothing Then
                    dlldoc.FO_Insert_Info_Scheda_Tipologia_Provvedimento(oOperatore, itemSchedaTipologiaProvvedimentoInfo)
                End If

                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(lstr_Key)
                If LCase(statoIstanza.Operatore) = LCase(oOperatore.Codice) Then
                    Dim vRVerifica = Verifica_Prima_Apertura(lstr_Key)

                    If vRVerifica(0) = 0 Then
                        aggiorna_Documenti_Sessione(context, "APRI", lstr_Key, vRVerifica(2), vRVerifica(1))
                    End If
                End If
            End If
            'Fine Lu 04/05/09  mod reg uffici


            HttpContext.Current.Session.Add("msgEsito", msgEsito)
            ' End If

            Dim vettoredati As Object
            vettoredati = Elenco_Documenti(CInt(tipoApp))
            context.Items.Add("tipoApplic", tipoApp)
            context.Items.Add("vettoreDati", vettoredati)

        Catch ex As Exception
            suffix = ".failure"
        End Try

        url = ActionMappings.getInstance.mappings.Get(action + suffix)
        'controllo se c'è un default
        If url Is Nothing Then
            url = ActionMappings.getInstance.mappings.Get("*" + suffix)
        End If
        url = url & "?tipo=" & tipoApp
        If lstr_Key <> "" Then
            url = url & "&key=" & lstr_Key
            context.Session.Add("codDocumento", lstr_Key)
        End If
        context.Response.Redirect(url)

        'context.Server.Transfer(url)
    End Sub

    Shared Function getSchedaLeggeTrasparenzaInfo(ByVal idDocumento As String, ByVal context As HttpContext, Optional ByVal removeFromSession As Boolean = False) As ItemSchedaLeggeTrasparenzaInfo
        Dim retValue As ItemSchedaLeggeTrasparenzaInfo = Nothing

        If Not isStringNullOrEmpty(idDocumento) Then
            retValue = context.Session("leggeTrasparenzaInfo")

            If Not retValue Is Nothing Then
                retValue.IdDocumento = idDocumento
            End If
        End If

        If removeFromSession Then
            context.Session.Remove("leggeTrasparenzaInfo")
        End If
        Return retValue
    End Function

    Shared Function getSchedaContrattiFattureInfo(ByVal idDocumento As String, ByVal context As HttpContext, Optional ByVal removeFromSession As Boolean = False) As ItemSchedaContrattiFattureInfo
        Dim retValue As ItemSchedaContrattiFattureInfo = Nothing

        If Not isStringNullOrEmpty(idDocumento) Then
            retValue = context.Session("contrattiFattureInfo")

            If Not retValue Is Nothing Then
                retValue.IdDocumento = idDocumento
            End If
        End If

        If removeFromSession Then
            context.Session.Remove("contrattiFattureInfo")
        End If
        Return retValue
    End Function


    Shared Function getSchedaTipologiaProvvedimentoInfo(ByVal idDocumento As String, ByVal context As HttpContext, Optional ByVal removeFromSession As Boolean = False) As ItemSchedaTipologiaProvvedimentoInfo
        Dim retValue As ItemSchedaTipologiaProvvedimentoInfo = Nothing

        If Not isStringNullOrEmpty(idDocumento) Then
            retValue = context.Session("tipologiaProvvedimentoInfo")

            If Not retValue Is Nothing Then
                retValue.IdDocumento = idDocumento
            End If
        End If

        If removeFromSession Then
            context.Session.Remove("tipologiaProvvedimentoInfo")
        End If
        Return retValue
    End Function

    Private Shared Function isStringNullOrEmpty(ByVal value As String, Optional ByVal trimValue As Boolean = True) As Boolean
        Dim retValue As Boolean = False

        If (trimValue) Then
            retValue = value Is Nothing OrElse value.Trim() = String.Empty
        Else
            retValue = value Is Nothing OrElse value = String.Empty
        End If

        Return retValue
    End Function
End Class
