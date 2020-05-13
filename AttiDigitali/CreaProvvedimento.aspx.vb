Imports System.ServiceModel
Imports DllDocumentale
Imports System.Collections.Generic
Imports System.Net
Imports Newtonsoft.Json

Partial Public Class CreaProvvedimento
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Crea Provvedimento")
        ClientScript.RegisterClientScriptInclude("SchedaLeggeTrasparenza", ResolveClientUrl("~/ext/SchedaLeggeTrasparenza" & HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE") & "_20150325.js"))
        ClientScript.RegisterClientScriptInclude("SchedaTipologia", ResolveClientUrl("~/ext/SchedaTipologia" & HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE") & "_20170913.js"))
        ClientScript.RegisterClientScriptInclude("CreaProvvedimento", ResolveClientUrl("~/ext/CreaProvvedimento" & HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE") & "_20170913.js?scriptversion=20181217"))
        ClientScript.RegisterClientScriptInclude("SchedaContrattiFatture", ResolveClientUrl("~/ext/SchedaContrattiFatture" & HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE") & "_20150909.js?scriptversion=20180411"))

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        
        msgBloccoCreazioneAtti.Value = ConfigurationManager.AppSettings("msgBloccoCreazioneAtti")
        ' Viene inibita la creazione degli atti con gli uffici vecchi esistenti fino al 30 Giugno 2014

        'If oOperatore.oUfficio.CodUfficio.StartsWith("U0") Or oOperatore.oUfficio.CodUfficio.StartsWith("U1") Or oOperatore.oUfficio.CodUfficio.StartsWith("U2") Then
        'If oOperatore.oUfficio.AbilitatoCreazioneAtti = 0 Then
        '    Response.Clear()
        '    Response.ClearHeaders()
        '    Response.Write("<script>")
        '    Response.Write("alert('\t\tATTENZIONE!\nA seguito dell’approvazione delle DGR n. 689-691 e 771/2015 - Nuova Organizzazione Uffici Regionali – l’Ufficio " + oOperatore.oUfficio.CodUfficioPubblico + " è stato disattivato, pertanto, non sarà possibile creare nuovi provvedimenti.');window.open('HomePage.aspx','_self');")
        '    Response.Write("</script>")
        '    Response.Flush()
        '    Response.Close()
        'Else


        Dim lblCreaModifica As String = "Crea"
        Dim flussoDocumentale As String = String.Empty
        Select Case Context.Request.QueryString("tipo")
            Case 0
                Rinomina_Pagina(Me, lblCreaModifica & " Determina")
                lblEtichetta.Value = "Scegliendo Crea, si proseguirà con la creazione di una nuova determina. " _
                & "<br/>Nel caso in cui, non si voglia creare la determina, scegliere Annulla"
                flussoDocumentale = "DETERMINE"
            Case 1
                Rinomina_Pagina(Me, lblCreaModifica & " Delibera")
                lblEtichetta.Value = "Scegliendo Crea, si proseguirà con la creazione di una nuova proposta di delibera. " _
                & "<br/>Nel caso in cui, non si voglia creare la proposta di delibera, scegliere Annulla"
                flussoDocumentale = "DELIBERE"
            Case 2
                Rinomina_Pagina(Me, lblCreaModifica & " Disposizione")
                lblEtichetta.Value = "Scegliendo Crea, si proseguirà con la creazione di una nuova disposizione. " _
                 & "<br/>Nel caso in cui, non si voglia creare la disposizione, scegliere Annulla"
                flussoDocumentale = "DISPOSIZIONI"
            Case 3
                Rinomina_Pagina(Me, lblCreaModifica & " Decreto")
                lblEtichetta.Value = "Scegliendo Crea, si proseguirà con la creazione di un nuovo decreto. " _
                 & "<br/>Nel caso in cui, non si voglia creare il decreto, scegliere Annulla"
                flussoDocumentale = "DECRETI"
            Case 4
                Rinomina_Pagina(Me, lblCreaModifica & " Ordinanza")
                lblEtichetta.Value = "Scegliendo Crea, si proseguirà con la creazione di una nuova ordinanza. " _
                 & "<br/>Nel caso in cui, non si voglia creare l'ordinanza, scegliere Annulla"
                flussoDocumentale = "ORDINANZE"
        End Select

        Context.Session.Remove("codDocumento")
        Context.Session.Remove("key")
        tipo.Value = Context.Request.QueryString("tipo")
        valueDataCreazione.Value = Now

        valueUffProp.Value = oOperatore.oUfficio.CodUfficio
        descrizioneUffProp.Value = oOperatore.oUfficio.DescrUfficio + " [" + oOperatore.oUfficio.CodUfficioPubblico + "]"

        Dim responsabileUfficio As DllAmbiente.Operatore = New DllAmbiente.Operatore()
        Dim idAnagraficaResponsabileUfficio As Long = responsabileUfficio.Leggi_IdAnagrafica(oOperatore.oUfficio.ResponsabileUfficio(flussoDocumentale))

        responsabileUffProp.Value = responsabileUfficio.Leggi_NominativoDaAnagrafica(idAnagraficaResponsabileUfficio)

        codFiscOperatore.Value = oOperatore.CodiceFiscale
        uffPubblicoOperatore.Value = oOperatore.oUfficio.CodUfficioPubblico

        If Context.Session("RegistraOggetto") = 1 Then
            flagModificato.Value = "Modifiche salvate con successo"
            'salvataggio Avvenuto con successo
            Context.Session.Remove("RegistraOggetto")
        End If

        If Not IsPostBack Then
            Dim key As String = ""
            key = Request.QueryString("key")

            If Not String.IsNullOrEmpty(key) Then
                lblCreaModifica = "Modifica"
            End If

            abilitatoCreazioneAttiSiOpCont.Value = oOperatore.oUfficio.AbilitatoCreazioneAttiSiOpCont

            If Not String.IsNullOrEmpty(key) Then
                lblEtichetta.Value = ""
                codDocumento.Value = key
                schedaLeggeTrasparenzaInfo.Value = JavaScriptConvert.SerializeObject(GetDatiSchedaLeggeTrasparenza(oOperatore, key))
                schedaTipologiaProvvedimentoInfo.Value = JavaScriptConvert.SerializeObject(GetDatiSchedaTipologiaProvvedimento(oOperatore, key))

                schedaContrattiFattureInfo.Value = JavaScriptConvert.SerializeObject(GetDatiSchedaContrattiFatture(oOperatore, key))

                Dim objDoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(Context.Session.Item("oOperatore"))).Get_StatoIstanzaDocumento(key)

                Dim oOp As DllAmbiente.Operatore = Context.Session.Item("oOperatore")

                valueUffProp.Value = objDoc.Doc_Cod_Uff_Prop
                'serve per disabilitare gli altri panel di creazione ed abilitare quelli per le info della archiviazione
                If ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE") = "ALSIA" And objDoc.Doc_Tipo = 1 Then
                    If statoIstanza.LivelloUfficio = "UAR" Then
                        flagLivello.Value = "UAR"
                    End If
                    If statoIstanza.LivelloUfficio = "UCA" And statoIstanza.Ruolo = "R" Then
                        flagLivello.Value = "UCA"
                    End If
                    If flagRegistra.Value = 1 Then
                        flagLivello.Value = "UP"
                    End If

                End If
                If verificaOpContabilInserite() Then
                    flagAbilitaOpContabili.Value = 0
                Else
                    flagAbilitaOpContabili.Value = 1
                End If

                valueDataCreazione.Value = objDoc.Doc_Data
                valuePub.Value = objDoc.Doc_Pubblicazione


                valueOpContabile.Value = String.Join(";", objDoc.ListaOpContabil)
                valueOggetto.Value = objDoc.Doc_Oggetto

                If (statoIstanza.Ruolo <> "A") And (statoIstanza.LivelloUfficio = "UP" Or (statoIstanza.LivelloUfficio = "UDD") Or (statoIstanza.LivelloUfficio = "USL") And oOp.oUfficio.CodUfficio = objDoc.Doc_Cod_Uff_Prop) And LCase(statoIstanza.Operatore) = LCase(oOp.Codice) Then
                    flagRegistra.Value = 1
                Else

                    'Modifica 26/03/2014
                    If (statoIstanza.LivelloUfficio = "UAR") And ((oOp.Test_Ruolo("TT001") And flussoDocumentale = "DETERMINE") Or (oOp.Test_Ruolo("TS001") And flussoDocumentale = "DISPOSIZIONI") Or (oOp.Test_Ruolo("TL001") And flussoDocumentale = "DELIBERE")) Then
                        flagRegistra.Value = 1
                        flagAbilitaOggettoDoc.Value = 0
                        flagAbilitaPubblBur.Value = 0
                        flagAbilitaOpContabili.Value = 0
                        flagCodificaAltriUff.Value = 0
                        flagCodiciCupCig.Value = 0
                        flagAbilitaTipologiaProvvedimento.Value = 0
                    Else
                        flagRegistra.Value = 0

                    End If
                End If


                Dim lstr_numero As String = IIf(String.IsNullOrEmpty(objDoc.Doc_numero & ""), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero)
                Select Case Context.Request.QueryString("tipo")
                    Case 0
                        Rinomina_Pagina(Me, " Determina " & lstr_numero)
                    Case 1
                        Rinomina_Pagina(Me, " Delibera " & lstr_numero)
                    Case 2
                        Rinomina_Pagina(Me, " Disposizione " & lstr_numero)
                    Case 3
                        Rinomina_Pagina(Me, " Decreto " & lstr_numero)
                    Case 4
                        Rinomina_Pagina(Me, " Ordinanza " & lstr_numero)
                End Select

                abilitatoCreazioneAttiSiOpCont.Value = oOperatore.oUfficio.AbilitatoCreazioneAttiSiOpCont

            ElseIf oOperatore.oUfficio.AbilitatoCreazioneAtti = 0 Then
                'l'ufficio non è abilitato a creare nessun tipo di atto.
                Response.Clear()
                Response.ClearHeaders()
                Response.Write("<script>")
                'Response.Write("alert('\t\tATTENZIONE!\nA seguito dell’approvazione delle DGR n. 689-691 e 771/2015 - Nuova Organizzazione Uffici Regionali – l’Ufficio " + oOperatore.oUfficio.CodUfficioPubblico + " è stato disattivato, pertanto, non sarà possibile creare nuovi provvedimenti.');window.open('HomePage.aspx','_self');")
                Response.Write("alert('\t\tATTENZIONE!\n "& msgBloccoCreazioneAtti.Value &"');window.open('HomePage.aspx','_self');")

                Response.Write("</script>")
                Response.Flush()
                Response.Close()

            'Else

            '     abilitatoCreazioneAttiSiOpCont.Value = oOperatore.oUfficio.AbilitatoCreazioneAttiSiOpCont
                'se può creare atti, controllo che possa creare atti con o senza op contabili
                
            End If
        End If

        If htmlDecode(Request.Form("chkSalva")) & "" = "1" Then
            Try
                Dim key As String = ""
                key = Request.QueryString("key")

                If Not String.IsNullOrEmpty(key) Then

                    Dim oOp As DllAmbiente.Operatore = Context.Session.Item("oOperatore")

                    Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOp)).Get_StatoIstanzaDocumento(key)

                    Dim ruolo As String = "" & statoIstanza.Ruolo
                    If ruolo = "A" Then
                        flagRegistra.Value = 0
                        Throw New Exception("Il provvedimento risulta annullato.")
                    End If

                    Dim objdocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)
                    If ((statoIstanza.LivelloUfficio = "UP") Or (statoIstanza.LivelloUfficio = "UDD")) And objdocumento.Doc_Cod_Uff_Prop = oOp.oUfficio.CodUfficio And LCase(statoIstanza.Operatore) = LCase(oOp.Codice) Then
                        'quando viene salvato il documento, deve invalidare tutte le firme precedenti, cancellando l'elenco certificati di firme
                        Cancella_Allegato_Fisicamente(, "16", key)
                        'modifico tutti i ruoli specifici
                        Modifica_Compiti(key)
                    End If
                End If

                Response.Clear()
                Response.ClearHeaders()
                '***********************    CREO IL PROVVEDIMENTO   *******************
                Dim str_return As String = CreaProvvedimento()
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
        'End If
    End Sub

    Private Function verificaOpContabilInserite() As Boolean
        Dim key As String = Context.Request.QueryString.Get("key")
        Dim dllDoc As New DllDocumentale.svrDocumenti(Context.Session.Item("oOperatore"))

        Dim listaPreimp As List(Of ItemImpegnoInfo)
        listaPreimp = dllDoc.FO_Get_DatiPreImpegni(key)

        Dim listaImp As List(Of ItemImpegnoInfo)

        listaImp = dllDoc.FO_Get_DatiImpegni(key)
        Dim listaLiq As List(Of ItemLiquidazioneInfo)
        listaLiq = dllDoc.FO_Get_DatiLiquidazione(key)
        Dim listaAcc As List(Of ItemAssunzioneContabileInfo)
        listaAcc = dllDoc.FO_Get_Dati_Assunzione(key)

        Dim listaRid As List(Of ItemRiduzioneInfo)
        listaRid = dllDoc.FO_Get_DatiImpegniVariazioni(key)

        Dim listaRidPreImp As List(Of ItemRiduzioneInfo)
        listaRidPreImp = dllDoc.FO_Get_DatiPreImpegniVariazioni(key)

        Dim listaRidLiq As List(Of ItemRiduzioneLiqInfo)
        listaRidLiq = dllDoc.FO_Get_DatiLiquidazioniVariazioni(key)

        If listaPreimp.Count > 0 Or listaImp.Count > 0 Or listaLiq.Count > 0 Or listaAcc.Count > 0 Or listaRid.Count > 0 Or listaRidPreImp.Count > 0 Or listaRidLiq.Count > 0 Then
            'If listaImp.Count > 0 Or listaLiq.Count > 0 Or listaAcc.Count > 0 Or listaRid.Count > 0 Or listaRidPreImp.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Protected Function CreaProvvedimento() As String
        Dim tipoApp As String = TipoApplic(Context)
        Context.Session.Remove("tipoApplic")

        Dim txtOggetto As String = CStr(htmlDecode(Context.Request.Form("txtOggetto")))
        Dim dataCreazione As String = CStr(htmlDecode(Context.Request.Form("dataCreazione")))
        Dim OpContabili As Integer = 0
        Dim tipoPubblic As Integer = 0

        Dim stringaOpConta As String = ""
        Select Case tipoApp
            Case 0, 1, 3, 4
                stringaOpConta = creaStringaOpContabili()
            Case 2
                stringaOpConta = creaStringaOpContabiliDisposizione()
        End Select

        Dim lstr_StringTempOpContabili As String = Trim(stringaOpConta.Replace("0", "").Replace(";", ""))
        'elminino tutti gli 0 e i punti e vigorla cosi facendo mi rimane il valore delle operazioni selezionate
        'non potrò + fare il controllo se contiene 1 visto che per sistemazioni contabili potrà avere altri valori
        'If stringaOpConta.Contains("1") Then
        If Not String.IsNullOrEmpty(lstr_StringTempOpContabili) Then
            OpContabili = 1
        Else
            OpContabili = 0
        End If

        Dim responsabileProcedimento As String = CStr(htmlDecode(Context.Request.Form("responsabileProcedimento")))
        If Not String.IsNullOrEmpty(responsabileProcedimento) Then
            Context.Session.Add("responsabileProcedimento", "" & responsabileProcedimento)
        End If



        tipoPubblic = htmlDecode(Context.Request.Form("TipoPubblicazione"))
        Select Case tipoApp
            Case 1
                OpContabili = 0
                Dim ufficioProponente As String = htmlDecode(Context.Request.Form("ufficiDiCompetenza"))
                Context.Session.Add("ufficioProponente", "" & ufficioProponente)
                Context.Session.Add("dataCreazione", "" & dataCreazione)
            Case 2
                OpContabili = 1
                tipoPubblic = 0
        End Select

        Dim ufficiDiCompetenza As String = htmlDecode(Context.Request.Form("ufficiDiCompetenza"))
        Context.Session.Add("ufficiDiCompetenza", "" & ufficiDiCompetenza)

        Dim utentiDiCompetenza As String = htmlDecode(Context.Request.Form("utentiDiCompetenza"))
        Context.Session.Add("utentiDiCompetenza", "" & utentiDiCompetenza)

        Dim valoriAttributi As String = htmlDecode(Context.Request.Form("AttributiRegistrati"))

        Context.Session.Add("txtOggetto", txtOggetto)
        Context.Session.Add("intOpContabili", OpContabili)
        Context.Session.Add("pubIntegrale", tipoPubblic)
        Context.Session.Add("valoriAttributi", "" & valoriAttributi)
        Context.Session.Add("leggeTrasparenzaInfo", getLeggeTrasparenzaInfo())
        Context.Session.Add("tipologiaProvvedimentoInfo", getTipologiaProvvedimentoInfo())

        Context.Session.Add("tipoOpContabili", stringaOpConta)

        Context.Session.Add("contrattiFattureInfo", getContrattiFattureInfo())




        Dim lstr_link As String = "CreaDocumentoAction.aspx?tipo=" & tipoApp
        Session.Remove("key")

        If Trim(Context.Request.QueryString.Get("key")) <> "" Then
            Session.Add("key", Context.Request.QueryString.Get("key"))
            Select Case tipoApp
                Case 0
                    lstr_link = "RegistraDeterminaAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
                Case 1
                    lstr_link = "RegistraDeliberaAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
                Case 2
                    lstr_link = "RegistraDisposizioneAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
                Case 3
                    lstr_link = "RegistraDecretoAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
                Case 4
                    lstr_link = "RegistraOrdinanzaAction.aspx?key=" & Context.Request.QueryString.Get("key") & "&tipo=" & tipoApp
            End Select



        End If

        Return lstr_link
    End Function

    Function creaStringaOpContabili() As String
        Dim result As String = ""
        If Not Request.Form("chkImpegno") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkImpegnoSuPerenti") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkLiquidazione") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkAccertamento") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkRiduzione") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkRiduzionePreImp") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkRiduzioneLiq") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkAltro") Is Nothing Then
            result = result & htmlDecode(Request.Form("ValueSistemazioni")) & ";"
        Else
            result = result & "0;"
        End If
        If Not Request.Form("chkPreimpegni") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        Return result
    End Function

    Function creaStringaOpContabiliDisposizione() As String
        Dim result As String = ""
        'chkImpegno is nothing 
        result = result & "0;"

        'chkImpegnoSuPerenti is nothing 
        result = result & "0;"

        If Not Request.Form("chkLiquidazione") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        If Not Request.Form("chkAccertamento") Is Nothing Then
            result = result & "1;"
        Else
            result = result & "0;"
        End If

        'chkRiduzione is nothing
        result = result & "0;"

        'chkRiduzionePreImp is nothing
        result = result & "0;"

        'chkRiduzioneLiq is nothing
        result = result & "0;"

        If Not Request.Form("chkAltro") Is Nothing Then

            result = result & htmlDecode(Request.Form("ValueSistemazioni")) & ";"
        Else
            result = result & "0;"
        End If

        Return result
    End Function

    Private Function getLeggeTrasparenzaInfo() As ItemSchedaLeggeTrasparenzaInfo
        Dim retValue As ItemSchedaLeggeTrasparenzaInfo = Nothing

        If Not Context.Request.Form("selezionePubblicazione") Is Nothing Then
            retValue = New ItemSchedaLeggeTrasparenzaInfo()

            Dim selezionePubblicazione As String = htmlDecode(Context.Request.Form("selezionePubblicazione"))
            If Not String.IsNullOrEmpty(selezionePubblicazione) Then
                retValue.AutorizzazionePubblicazione = IIf(selezionePubblicazione = "SI", True, False)
            Else
                retValue.AutorizzazionePubblicazione = False
            End If

            If retValue.AutorizzazionePubblicazione = False Then
                If Not Context.Request.Form("noteAutorizzazionePubblicazione") Is Nothing Then
                    retValue.NotePubblicazione = htmlDecode(Context.Request.Form("noteAutorizzazionePubblicazione")).Trim()
                End If
            Else
                retValue.NotePubblicazione = String.Empty

                Dim normaAttribuzioneBeneficio As String = String.Empty

                If Not Context.Request.Form("selezioneNorma") Is Nothing Then
                    Dim selezioneNorma As String = htmlDecode(Context.Request.Form("selezioneNorma"))
                    If Not String.IsNullOrEmpty(selezioneNorma) Then
                        If selezioneNorma = "DELIBERA" Then
                            Dim numero As String = htmlDecode(Context.Request.Form("numeroDelibera"))
                            Dim anno As String = htmlDecode(Context.Request.Form("annoDelibera"))
                            If Not isStringNullOrEmpty(numero) AndAlso Not isStringNullOrEmpty(anno) Then
                                normaAttribuzioneBeneficio = "Delibera n. " + numero + "/" + anno
                            End If
                        ElseIf selezioneNorma = "DETERMINA" Then
                            Dim ufficio As String = htmlDecode(Context.Request.Form("ufficioDetermina"))
                            Dim numero As String = htmlDecode(Context.Request.Form("numeroDetermina"))
                            Dim anno As String = htmlDecode(Context.Request.Form("annoDetermina"))
                            If Not isStringNullOrEmpty(numero) AndAlso Not isStringNullOrEmpty(anno) AndAlso Not isStringNullOrEmpty(ufficio) Then

                                Dim dllDoc As New DllDocumentale.svrDocumenti(Context.Session.Item("oOperatore"))
                                Dim formatoNumeroDetermina As String = dllDoc.FO_GetFormatoNumeroDocumento("NUDT")

                                If isStringNullOrEmpty(formatoNumeroDetermina) Then
                                    formatoNumeroDetermina = "%UFFICIO%.%ANNO%/D.00000"
                                End If

                                normaAttribuzioneBeneficio = Replace(formatoNumeroDetermina, "%ANNO%", anno)
                                normaAttribuzioneBeneficio = Replace(normaAttribuzioneBeneficio, "%UFFICIO%", ufficio)

                                Dim tmpNormaAttribuzioneBeneficio As String = normaAttribuzioneBeneficio.Replace(".", "_")
                                normaAttribuzioneBeneficio = Format(CType(numero, Long), tmpNormaAttribuzioneBeneficio)
                                If normaAttribuzioneBeneficio.IndexOf("_") > 0 Then
                                    normaAttribuzioneBeneficio = normaAttribuzioneBeneficio.Replace("_", ".")
                                End If

                                normaAttribuzioneBeneficio = "Determina n. " + normaAttribuzioneBeneficio
                            End If
                        ElseIf selezioneNorma = "ALTRO" Then
                            If Not Context.Request.Form("normaAttribuzioneAltroDescrizione") Is Nothing Then
                                normaAttribuzioneBeneficio = htmlDecode(Context.Request.Form("normaAttribuzioneAltroDescrizione")).Trim()
                            End If
                        End If
                    End If
                End If

                retValue.NormaAttribuzioneBeneficio = normaAttribuzioneBeneficio

                If Not Context.Request.Form("ufficioResponsabileDelProcedimento") Is Nothing Then
                    retValue.UfficioResponsabileProcedimento = htmlDecode(Context.Request.Form("ufficioResponsabileDelProcedimento")).Trim()
                End If

                If Not Context.Request.Form("funzionarioResponsabileDelProcedimento") Is Nothing Then
                    retValue.FunzionarioResponsabileProcedimento = htmlDecode(Context.Request.Form("funzionarioResponsabileDelProcedimento")).Trim()
                End If

                If Not Context.Request.Form("modalitaIndividuazioneBeneficiario") Is Nothing Then
                    retValue.ModalitaIndividuazioneBeneficiario = htmlDecode(Context.Request.Form("modalitaIndividuazioneBeneficiario")).Trim()
                End If
            End If

            If Not Context.Request.Form("contenutoAtto") Is Nothing Then
                retValue.ContenutoAtto = htmlDecode(Context.Request.Form("contenutoAtto")).Trim()
            End If

            'retValue.Contratti = New Generic.List(Of ItemContrattoInfoHeader)

            'If Not Context.Request.Form("listaContratti") Is Nothing Then
            '    Dim listaContratti As String = htmlDecode(HttpContext.Current.Request.Item("listaContratti"))
            '    listaContratti = "[" & listaContratti & "]"

            '    Dim contratti As List(Of Ext_ContrattoInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaContratti, GetType(List(Of Ext_ContrattoInfo))), List(Of Ext_ContrattoInfo))
            '    For Each contratto As Ext_ContrattoInfo In contratti
            '        Dim itemContrattoinfo As ItemContrattoInfoHeader = New ItemContrattoInfoHeader()
            '        itemContrattoinfo.IdContratto = contratto.Id

            '        retValue.Contratti.Add(itemContrattoinfo)
            '    Next
            'End If
        End If

        Return retValue
    End Function

    Private Function getTipologiaProvvedimentoInfo() As ItemSchedaTipologiaProvvedimentoInfo
        Dim retValue As ItemSchedaTipologiaProvvedimentoInfo = Nothing

        'tipologia provvedimento
        If Not Context.Request.Form("idTipologiaProvvedimento") Is Nothing Then
            Dim idTipologiaProvvedimento As Integer = -1
            If Integer.TryParse(Context.Request.Form("idTipologiaProvvedimento"), idTipologiaProvvedimento) Then
                retValue = New ItemSchedaTipologiaProvvedimentoInfo()
                retValue.IdTipologiaProvvedimento = IIf(idTipologiaProvvedimento > -1, idTipologiaProvvedimento, -1)

                Dim selezioneSommaAutomatica As String = htmlDecode(Context.Request.Form("selezioneSommaAutomatica"))
                If Not String.IsNullOrEmpty(selezioneSommaAutomatica) Then
                    retValue.isSommaAutomatica = IIf(selezioneSommaAutomatica = "SI", True, False)
                Else
                    retValue.isSommaAutomatica = False
                End If


                'importo spesa prevista
                If Not Context.Request.Form("importoSpesaPrevista") Is Nothing Then
                    Dim importoSpesaPrevista As Decimal = Nothing
                    If Decimal.TryParse(Context.Request.Form("importoSpesaPrevista"), importoSpesaPrevista) Then
                        retValue.ImportoSpesaPrevista = importoSpesaPrevista
                    End If
                End If

                'destinatari
                retValue.Destinatari = New Generic.List(Of ItemDestinatarioInfo)

                If Not Context.Request.Form("listaDestinatari") Is Nothing Then
                    Dim listaDestinatari As String = htmlDecode(HttpContext.Current.Request.Item("listaDestinatari"))
                    listaDestinatari = "[" & listaDestinatari & "]"

                    Dim destinatari As List(Of Ext_DestinatarioInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaDestinatari, GetType(List(Of Ext_DestinatarioInfo))), List(Of Ext_DestinatarioInfo))
                    For Each destinatario As Ext_DestinatarioInfo In destinatari
                        Dim itemDestinatarioInfo As ItemDestinatarioInfo = New ItemDestinatarioInfo()

                        Dim tmpDate As DateTime

                        itemDestinatarioInfo.Id = destinatario.Id
                        itemDestinatarioInfo.IdSIC = destinatario.IdSIC
                        itemDestinatarioInfo.IdDocumento = destinatario.IdDocumento
                        itemDestinatarioInfo.Denominazione = destinatario.Denominazione
                        itemDestinatarioInfo.CodiceFiscale = destinatario.CodiceFiscale
                        itemDestinatarioInfo.PartitaIva = destinatario.PartitaIva
                        itemDestinatarioInfo.isPersonaFisica = destinatario.isPersonaFisica
                        itemDestinatarioInfo.isDatoSensibile = destinatario.isDatoSensibile
                        itemDestinatarioInfo.ImportoSpettante = destinatario.ImportoSpettante

                        If DateTime.TryParse(destinatario.DataNascita, tmpDate) Then
                            itemDestinatarioInfo.DataNascita = tmpDate
                        End If

                        itemDestinatarioInfo.LuogoNascita = destinatario.LuogoNascita
                        itemDestinatarioInfo.LegaleRappresentante = destinatario.LegaleRappresentante
                        itemDestinatarioInfo.IdContratto = destinatario.IdContratto
                        itemDestinatarioInfo.NumeroRepertorioContratto = destinatario.NumeroRepertorioContratto

                        retValue.Destinatari.Add(itemDestinatarioInfo)
                    Next
                End If
            End If
        End If

        Return retValue
    End Function

    Private Function getContrattiFattureInfo() As ItemSchedaContrattiFattureInfo
        Dim retValue As ItemSchedaContrattiFattureInfo = Nothing

        retValue = New ItemSchedaContrattiFattureInfo()

        retValue.Contratti = New Generic.List(Of ItemContrattoInfoHeader)
        retValue.Fatture = New Generic.List(Of ItemFatturaInfoHeader)

        If Not Context.Request.Form("listaContratti") Is Nothing Then
            Dim listaContratti As String = htmlDecode(HttpContext.Current.Request.Item("listaContratti"))

            listaContratti = "[" & listaContratti & "]"

            Dim contratti As List(Of Ext_ContrattoInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaContratti, GetType(List(Of Ext_ContrattoInfo))), List(Of Ext_ContrattoInfo))
            For Each contratto As Ext_ContrattoInfo In contratti
                Dim itemContrattoinfo As ItemContrattoInfoHeader = New ItemContrattoInfoHeader()
                itemContrattoinfo.IdContratto = contratto.Id
                itemContrattoinfo.CodieCIG = contratto.CodiceCIG
                itemContrattoinfo.CodieCUP = contratto.CodiceCUP

                retValue.Contratti.Add(itemContrattoinfo)
            Next
        End If

        If Not Context.Request.Form("listaFatture") Is Nothing Then
            Dim listaFatture As String = htmlDecode(HttpContext.Current.Request.Item("listaFatture"))
            listaFatture = "[" & listaFatture & "]"

            Dim fatture As List(Of Ext_FatturaInfo) = DirectCast(Newtonsoft.Json.JavaScriptConvert.DeserializeObject(listaFatture, GetType(List(Of Ext_FatturaInfo))), List(Of Ext_FatturaInfo))
            For Each fatturaExt As Ext_FatturaInfo In fatture
                Dim itemFatturainfo As ItemFatturaInfoHeader = New ItemFatturaInfoHeader()
                itemFatturainfo.IdUnivoco = fatturaExt.IdUnivoco
                itemFatturainfo.IdDocumento = fatturaExt.IdDocumento

                Dim contrattoItem As ItemContrattoInfo = New ItemContrattoInfo
                Dim anagraficaInfo As ItemLiquidazioneImpegnoBeneficiarioInfo = New ItemLiquidazioneImpegnoBeneficiarioInfo

                contrattoItem.IdContratto = fatturaExt.Contratto.Id
                contrattoItem.NumeroRepertorioContratto = fatturaExt.Contratto.NumeroRepertorio
                contrattoItem.OggettoContratto = fatturaExt.Contratto.Oggetto

                itemFatturainfo.Contratto = contrattoItem

                itemFatturainfo.NumeroFatturaBeneficiario = fatturaExt.NumeroFatturaBeneficiario
                Dim tmpDate As DateTime
                If DateTime.TryParse(fatturaExt.DataFatturaBeneficiario, tmpDate) Then
                    itemFatturainfo.DataFatturaBeneficiario = tmpDate
                End If

                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.Denominazione) Then
                    anagraficaInfo.Denominazione = fatturaExt.AnagraficaInfo.Denominazione
                    anagraficaInfo.FlagPersonaFisica = False
                Else
                    anagraficaInfo.Denominazione = fatturaExt.AnagraficaInfo.Nome + fatturaExt.AnagraficaInfo.Cognome
                    anagraficaInfo.FlagPersonaFisica = True
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.CodiceFiscale) Then
                    anagraficaInfo.CodiceFiscale = fatturaExt.AnagraficaInfo.CodiceFiscale
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.PartitaIva) Then
                    anagraficaInfo.PartitaIva = fatturaExt.AnagraficaInfo.PartitaIva
                End If

                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).IdSede) Then
                    anagraficaInfo.IdSede = fatturaExt.AnagraficaInfo.ListaSedi(0).IdSede
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).NomeSede) Then
                    anagraficaInfo.SedeVia = fatturaExt.AnagraficaInfo.ListaSedi(0).NomeSede
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).IdModalitaPagamento) Then
                    anagraficaInfo.IdModalitaPag = fatturaExt.AnagraficaInfo.ListaSedi(0).IdModalitaPagamento
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).ModalitaPagamento) Then
                    anagraficaInfo.DescrizioneModalitaPag = fatturaExt.AnagraficaInfo.ListaSedi(0).ModalitaPagamento
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).DatiBancari(0).IdContoCorrente) Then
                    anagraficaInfo.IdConto = fatturaExt.AnagraficaInfo.ListaSedi(0).DatiBancari(0).IdContoCorrente
                End If
                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ListaSedi(0).DatiBancari(0).Iban) Then
                    anagraficaInfo.Iban = fatturaExt.AnagraficaInfo.ListaSedi(0).DatiBancari(0).Iban
                End If

                itemFatturainfo.ImportoTotaleFattura = fatturaExt.ImportoTotaleFattura
                If Not String.IsNullOrEmpty(fatturaExt.DescrizioneFattura) Then
                    itemFatturainfo.DescrizioneFattura = fatturaExt.DescrizioneFattura
                End If
                If Not String.IsNullOrEmpty(fatturaExt.IdDocumento) Then
                    itemFatturainfo.IdDocumento = fatturaExt.IdDocumento
                End If

                If Not String.IsNullOrEmpty(fatturaExt.AnagraficaInfo.ID) Then
                    anagraficaInfo.IdAnagrafica = fatturaExt.AnagraficaInfo.ID
                End If

                itemFatturainfo.AnagraficaInfo = anagraficaInfo


                If Not fatturaExt.ListaAllegati Is Nothing Then
                    Dim listaAllegatiFattura As Generic.List(Of ItemFatturaAllegato) = New Generic.List(Of ItemFatturaAllegato)
                    For Each allegato As Ext_FatturaAllegato In fatturaExt.ListaAllegati
                        Dim allegatoItem As New ItemFatturaAllegato
                        allegatoItem.Nome = allegato.Nome
                        allegatoItem.Formato = allegato.Formato
                        allegatoItem.Url = allegato.Url
                        allegatoItem.IdDocumento = fatturaExt.IdDocumento

                        listaAllegatiFattura.Add(allegatoItem)
                    Next

                    If Not listaAllegatiFattura Is Nothing Then
                        itemFatturainfo.ListaAllegati = listaAllegatiFattura
                    End If
                End If


                retValue.Fatture.Add(itemFatturainfo)
            Next
        End If

        Return retValue
    End Function

    Private Function isStringNullOrEmpty(ByVal value As String, Optional ByVal trimValue As Boolean = True) As Boolean
        Dim retValue As Boolean = False

        If (trimValue) Then
            retValue = value Is Nothing OrElse value.Trim() = String.Empty
        Else
            retValue = value Is Nothing OrElse value = String.Empty
        End If

        Return retValue
    End Function

    Private Function htmlDecode(ByVal value As Object)
        Dim retValue As Object = Nothing
        If Not value Is Nothing Then
            retValue = HttpUtility.HtmlDecode(value)
        End If
        Return retValue
    End Function
End Class