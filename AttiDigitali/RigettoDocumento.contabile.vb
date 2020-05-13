Imports System.Collections.Generic
Partial Class RigettoDocumento
    Inherits WebSession
    Sub cancellaPrenotazioneSIC(ByVal codDocumento As String)


	
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)

        Dim flusso As String = DefinisciFlusso(objDocumento.Doc_Tipo)
        Dim respUfficio As String = oOperatore.oUfficio.ResponsabileUfficio(flusso)

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(codDocumento)

        If (statoIstanza.LivelloUfficio = "UR" And oOperatore.oUfficio.bUfficioRagioneria And LCase(respUfficio) = LCase(oOperatore.Codice)) Then
            Dim tipoAtto As String = ""
            Dim dataAtto As Date = objDocumento.Doc_Data

            Dim numeroAtto As String = Right(objDocumento.Doc_numero, 5)
            Select Case objDocumento.Doc_Tipo
                Case 0
                    tipoAtto = "DETERMINA"
                Case 1
                    tipoAtto = "DELIBERA"
                Case 2
                    tipoAtto = "DISPOSIZIONE"

            End Select

            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            EliminaLiquidazioni(codDocumento, flusso)
            EliminaRiduzioni(codDocumento, tipoAtto, numeroAtto, dataAtto, numeroProvvisOrDefAtto)
            EliminaImpegni(codDocumento, tipoAtto, numeroAtto, dataAtto, numeroProvvisOrDefAtto)
            EliminaRiduzioniLiq(codDocumento, tipoAtto, numeroAtto, dataAtto, objDocumento.Doc_Oggetto, numeroProvvisOrDefAtto)
            EliminaRiduzioniPreImp(codDocumento, tipoAtto, numeroAtto, dataAtto, objDocumento.Doc_Oggetto, numeroProvvisOrDefAtto)
        End If

    End Sub
    Private Sub EliminaImpegni(ByVal key As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal numeroProvvisOrDefAtto As String)
        Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaImpegni = dllDoc.FO_Get_DatiImpegni(key)
        Dim flagResult As Boolean = False

        For Each item As DllDocumentale.ItemImpegnoInfo In listaImpegni
            Try
                If item.Di_Stato = 1 Then
                    If Not (String.IsNullOrEmpty(item.Dli_NumImpegno) Or item.Dli_NumImpegno = "0") Then
                        ClientIntegrazioneSic.MessageMaker.createEliminazioneImpMessage(oOperatore, item.Dli_NumImpegno, tipoAtto, dataAtto, numeroAtto, key, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())
                    End If
                    If String.IsNullOrEmpty(item.NDocPrecedente) Then

                        If item.Di_PreImpDaPrenotazione Then
                            If item.Dli_NumImpegno = "0" Or item.Dli_NumImpegno = "" Then
                                ' prenotazione da delibera da verificare
                                flagResult = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(oOperatore, "C", item.Dli_NPreImpegno, item.Dli_Costo)
                            End If
                        Else
                            flagResult = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(oOperatore, item.Dli_NPreImpegno, tipoAtto, dataAtto, numeroAtto, key, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())
                        End If
                    End If

                    dllDoc.FO_Delete_Logica_Bil(item)

                End If
            Catch ex As Exception

                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        Next









    End Sub

    Public Sub EliminaLiquidazioni(ByVal key As String, ByVal flusso As String)
        Dim tipoAtto As String
        Dim numeroAtto As String
        Dim dataAtto As Date
        Dim ProcAmm As New ProcAmm
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim respUfficio As String = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE")

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(key)

        If (statoIstanza.LivelloUfficio = "UR" And oOperatore.oUfficio.bUfficioRagioneria And LCase(respUfficio) = LCase(oOperatore.Codice)) Then
            Dim listaLiquidazioni As IList(Of DllDocumentale.ItemLiquidazioneInfo)

            Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)
            listaLiquidazioni = dllDoc.FO_Get_DatiLiquidazione(key)


            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
	            numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
	            numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            Dim flagResult As Boolean = False
            For Each item As DllDocumentale.ItemLiquidazioneInfo In listaLiquidazioni
                Try
                    Select Case item.Dli_TipoAssunzione
                        Case 0
                            tipoAtto = "DETERMINA"
                        Case 1
                            tipoAtto = "DELIBERA"
                        Case 2
                            tipoAtto = "DISPOSIZIONE"
                    End Select

                    numeroAtto = item.Dli_Num_assunzione
                    dataAtto = item.Dli_Data_Assunzione

                    'CANCELLA FATTURE SUL SIC CON NumLiquidazione
                    Dim P_ESITO_PROVV As Double = 0
                    Dim P_ESITO_DEF As Double = 0

                    Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattByLiquidazione(item.Dli_prog, objDocumento.Doc_id)
                    If Not listaFattureLiquidazioneEXT Is Nothing Then
                        ' per ogni fattura è necessario chiamare la cancellazione 2 volte:
                        '   1°: numero provvisorio dell'atto con num liq: 0
                        '   2°: numero definitivo dell'atto con num liq: quello definitivo del SIC
                        For Each fattura As Ext_FatturaInfo In listaFattureLiquidazioneEXT
                            Dim messaggioSicCancellazioneProvv As String = ClientIntegrazioneSic.MessageMaker.notificaAttoFatturaSIC(oOperatore, "", _
                                                                                                     objDocumento.Doc_numeroProvvisorio, ClientIntegrazioneSic.Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString, _
                                                                                                     0, 0, 0, fattura.IdUnivoco, _
                                                                                                     "C", Now, fattura.ImportoLiquidato, P_ESITO_PROVV)
                            If P_ESITO_PROVV <> 1 Then
                                Throw New Exception(messaggioSicCancellazioneProvv)
                            Else
                                If item.Dli_NLiquidazione = 0 Then
                                    dllDoc.FO_Delete_Liquidazione_Fattura(objDocumento.Doc_id, item.Dli_prog, fattura.IdUnivoco, )
                                End If
                            End If

                            If item.Dli_NLiquidazione <> 0 Then
                                Dim messaggioSicCancellazioneDEF As String = ClientIntegrazioneSic.MessageMaker.notificaAttoFatturaSIC(oOperatore, objDocumento.Doc_numero, _
                                                                                                         "", ClientIntegrazioneSic.Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString, _
                                                                                                         item.Dli_NLiquidazione, 0, 0, fattura.IdUnivoco, _
                                                                                                         "C", Now, fattura.ImportoLiquidato, P_ESITO_DEF)
                                If P_ESITO_DEF <> 1 Then
                                    Throw New Exception(messaggioSicCancellazioneDEF)
                                Else
                                    dllDoc.FO_Delete_Liquidazione_Fattura(objDocumento.Doc_id, item.Dli_prog, fattura.IdUnivoco, )
                                End If
                            End If
                        Next
                    End If

                    If item.Dli_NLiquidazione <> 0 Then
                        If item.Di_Stato = 1 Then
                            ClientIntegrazioneSic.MessageMaker.createEliminazioneLiqMessage(oOperatore, item.Dli_NLiquidazione, tipoAtto, dataAtto, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())
                            dllDoc.FO_Delete_Logica_Liq(item)
                        End If
                    Else
                        dllDoc.FO_Delete_Logica_Liq(item)
                    End If

                Catch ex As Exception

                    HttpContext.Current.Session.Add("error", ex.Message)
                    Response.Redirect("MessaggioErrore.aspx")

                End Try

            Next

            Dim listaFattureAttoEXT As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattureAtto(objDocumento.Doc_id)
            For Each fatturaAtto As Ext_FatturaInfo In listaFattureAttoEXT
                dllDoc.FO_Delete_FatturaByIdFatturaSIC(fatturaAtto.IdUnivoco, objDocumento.Doc_id)
            Next
        End If

    End Sub
  
    Private Sub EliminaRiduzioni(ByVal key As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal numeroProvvisOrDefAtto As String)
        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaRiduzioni = dllDoc.FO_Get_DatiImpegniVariazioni(key)
        Dim flagResult As Boolean = False
        For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni
            Try
               
                If item.Div_NumeroReg <> "0" And item.Div_NumeroReg <> "" Then
                    If item.Di_Stato = 1 Then
                        If item.Div_IsEconomia Then
                            ClientIntegrazioneSic.MessageMaker.createEliminazioneEcoMessage(oOperatore, item.Div_NumeroReg, tipoAtto, dataAtto, numeroAtto, key, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())
                        Else
                                                                
                            Dim objdoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)
                            Dim dip As String = objdoc.Doc_Cod_Uff_Pubblico()
                            ClientIntegrazioneSic.MessageMaker.createGenerazioneRiduzioneMessage(oOperatore, item.Dli_NumImpegno, item.Dli_Costo, tipoAtto, dataAtto, numeroAtto, item.Div_Data_Assunzione, dip, objdoc.Doc_Oggetto, objdoc.Doc_id, numeroProvvisOrDefAtto, item.HashTokenCallSic)
                        End If
                        dllDoc.FO_Delete_Logica_Impegno_Var(item)
                    End If
                Else
                    dllDoc.FO_Delete_Logica_Impegno_Var(item)
                End If
            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try

        Next

    End Sub

    Private Sub EliminaRiduzioniLiq(ByVal idDocumento As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal oggettoAtto As String,ByVal numeroProvvisOrDefAtto As String)
        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneLiqInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaRiduzioni = dllDoc.FO_Get_DatiLiquidazioniVariazioni(idDocumento)
        Dim flagResult As Boolean = False
        For Each item As DllDocumentale.ItemRiduzioneLiqInfo In listaRiduzioni

            Try
                If item.Div_NumeroReg <> "0" And item.Div_NumeroReg <> "" Then
                    If item.Di_Stato = 1 Then

                        ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazioneLiqMessage(oOperatore, item.Div_NLiquidazione, item.Dli_Costo, Now, oggettoAtto, numeroAtto, idDocumento, numeroProvvisOrDefAtto, item.HashTokenCallSic)
                        dllDoc.FO_Delete_Logica_Liquidazione_Var(item)
                    End If
                Else
                    dllDoc.FO_Delete_Logica_Liquidazione_Var(item)
                End If
            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try

        Next

    End Sub
    Private Sub EliminaRiduzioniPreImp(ByVal idDocumento As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal oggettoAtto As String,
                                       ByVal numeroProvvisOrDefAtto As String)
        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaRiduzioni = dllDoc.FO_Get_DatiPreImpegniVariazioni(idDocumento)
        Dim flagResult As Boolean = False
        For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni

            Try
                If item.Div_NumeroReg <> "0" And item.Div_NumeroReg <> "" Then
                    If item.Di_Stato = 1 Then

                        ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazionePreIMPMessage(oOperatore, item.Dli_NPreImpegno, item.Dli_Costo, tipoAtto, dataAtto, numeroAtto, item.Div_Data_Assunzione, oggettoAtto, idDocumento, numeroProvvisOrDefAtto, item.HashTokenCallSic)
                        dllDoc.FO_Delete_Logica_PreImpegno_Var(item)
                    End If
                Else
                    dllDoc.FO_Delete_Logica_PreImpegno_Var(item)
                End If
            Catch ex As Exception
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try

        Next

    End Sub
End Class
