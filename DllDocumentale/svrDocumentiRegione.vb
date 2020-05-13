Imports System.Configuration

Partial Public Class svrDocumentiRegione
    Inherits svrDocumenti

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Friend Overrides Function FO_Elenco_Documenti_Da_Stampare(ByVal vParm As Object) As Object
        Const SFunzione As String = "FO_Elenco_Documenti_Da_Stampare"
        Dim vRitPar(3) As Object
        Dim Sqlq As String = ""
        Dim sWhere As String = ""
        Dim data_inizio As String = ""
        Dim data_fine As String = ""
        Dim oggetto As String = ""
        Dim cod_ufficio As String = ""
        Dim cod_dip As String = ""
        Dim num_doc As String = ""
        Dim idDocumentoLocale As String = ""
        Dim descr_ufficio As String = ""
        Dim visualizzaLaTipRigetto As String = ""
        Dim visualizzaFlagStatoStampato As String = ""
        Dim tipoDocumento As Integer
        Dim i As Integer
        Dim strConn As String
        Dim rdr As SqlClient.SqlDataReader

        Dim ufficio_competenza As String = ""

        vRitPar(0) = 0
        vRitPar(1) = ""
        Try


            If SISTEMA.bTRACE Then
                Call SISTEMA.Registra_Trace("Inizio", SFunzione)
            End If

            If IsNumeric(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento)) Then
                tipoDocumento = vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_tipoDocumento) & ""
            Else
                tipoDocumento = -1
            End If

            If IsDate(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio)) Then
                data_inizio = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_data_inizio) & "")
            End If
            If IsDate(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine)) Then
                data_fine = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_data_fine) & "")
            End If
            oggetto = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_oggetto) & "")
            cod_ufficio = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_cod_ufficio) & "")
            cod_dip = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_cod_dip) & "")
            num_doc = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_numero_doc) & "")
            visualizzaLaTipRigetto = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_tipo_Rigetto) & "")
            ufficio_competenza = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_competenza) & "")
            idDocumentoLocale = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_idDocumento) & "")

            visualizzaFlagStatoStampato = Trim(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_FlagStampato) & "")

            Dim lstr_listaUtenti As String = ""


            lstr_listaUtenti = "'" & vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_utente) & "'"

            Dim tipologia As String = ""

            Sqlq = " SELECT a.Doc_Id, case when isnull(a.Doc_NumDefinitivo ,'') = '' then isnull(a.Doc_numero,'')+'' else isnull(a.Doc_NumDefinitivo,'')+' - '+isnull(a.Doc_numero,'')+'' end, a.doc_data,a.Doc_Oggetto, a.Doc_isContabile,a.Livello_Ufficio, a.osservazioniLivello  from (" & _
                      " select Documento.Doc_Id , isnull(Doc_numero,Doc_numeroProvvisorio) as Doc_numero ,doc_data  , " & _
                      "         dbo.fn_ReplaceCaratteriSpeciali(isnull(Doc_Oggetto,'')) as Doc_Oggetto , Doc_id_WFE ,'' as doc_tipoScadenza,   Doc_Cod_Uff_Prop , doc_dataRicezione, isnull(Doc_isContabile,0) as Doc_isContabile ,isnull(Doc_FlagStampato,0) as  Doc_FlagStampato " & _
                      " , Stato_Istanza_Documento.[Livello_Ufficio] as Livello_Ufficio,isnull(Dno_testo,'') as osservazioniLivello, Doc_NumDefinitivo as Doc_NumDefinitivo" & _
                      " FROM Documento inner join  Stato_Istanza_Documento on Documento.[Doc_Id] = Stato_Istanza_Documento.[Doc_Id] and Stato_Istanza_Documento.[Ruolo] <> 'A' " & _
                      " left join Documento_noteosservazioni on Stato_Istanza_Documento.[Doc_Id]=Documento_noteosservazioni.[Dno_id_documento] and Stato_Istanza_Documento.[Livello_Ufficio]=Documento_noteosservazioni.[Dno_tipo]" & _
                      " where  Stato_Istanza_Documento.operatore in (" & lstr_listaUtenti & " ) and Documento.Doc_Tipo= " & tipoDocumento & " )a " & _
                           " inner join struttura on struttura.Str_id = a.Doc_Cod_Uff_Prop "



            If Not String.IsNullOrEmpty(ufficio_competenza) Then
                Sqlq = Sqlq & " join Documento_Uff_Competenza on a.Doc_Id=duc_idDocumento and duc_ufficiCompetenza='" & ufficio_competenza & "' "
            End If

            If Not vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione) Is Nothing AndAlso DirectCast(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione), Array).Length <> 0 Then
                Dim ufficio As New DllAmbiente.Ufficio
                tipologia = ufficio.leggiTipologiaUfficio(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione)(0))
                'effettuo controllo su cosa posso vedere
                Dim stringaWhere As String = ""


                Dim e As Integer
                For e = 0 To UBound(DirectCast(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione), Array)) - 1
                    stringaWhere = stringaWhere & DirectCast(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione), Array)(e) & "' , '"
                Next
                stringaWhere = stringaWhere & DirectCast(vParm(Dic_FODocumentale.vc_Elenco_Documenti.c_ufficio_creazione), Array)(e)

                If UCase(tipologia) = "DIP" Then
                    If cod_dip <> "" And stringaWhere.IndexOf(cod_dip) >= 0 Then
                        stringaWhere = cod_dip
                    End If

                    sWhere = sWhere & " WHERE  (struttura.Str_padre in  ('" & stringaWhere & "')) "

                    If cod_ufficio <> "" Then
                        sWhere = " AND ( Doc_Cod_Uff_Prop = '" & cod_ufficio & "') "
                    End If


                ElseIf UCase(tipologia) = "UFF" Then
                    If cod_ufficio <> "" And stringaWhere.IndexOf(cod_ufficio) >= 0 Then
                        stringaWhere = cod_ufficio
                    End If

                    sWhere = sWhere & " WHERE (Doc_Cod_Uff_Prop in ('" & stringaWhere & "')) "
                ElseIf UCase(tipologia) = "ENTE" Then
                    If cod_ufficio <> "" And stringaWhere.IndexOf(cod_ufficio) >= 0 Then
                        stringaWhere = cod_ufficio
                    End If
                    sWhere = sWhere & " WHERE   (Struttura.Str_radice in ('" & stringaWhere & "')) "

                    If cod_ufficio <> "" Then
                        sWhere = " AND ( Doc_Cod_Uff_Prop = '" & cod_ufficio & "') "
                    End If

                End If

            Else

                If cod_dip <> "" Then
                    sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Struttura.Str_padre = '" & cod_dip & "' ) "
                End If

                If cod_ufficio <> "" And cod_ufficio <> "ARCHIVIO" Then
                    sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Struttura.Str_id = '" & cod_ufficio & "' ) "
                End If


                If descr_ufficio <> "" Then
                    sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Struttura.Str_descrizione like '%" & descr_ufficio & "%' ) "
                End If
            End If

            Dim queryoggetto As String = SplitStringOggettoDoc_Oggetto(oggetto)
            If queryoggetto <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & " ( " & queryoggetto & " ) "
            End If

            If num_doc <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  (CAST(RIGHT(Doc_numero, 5) AS int)  = '" & Trim(num_doc) & "'  ) "
            End If

            If data_inizio <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & " ( Doc_Data >= CONVERT(DATETIME, '" & Format(CDate(data_inizio), "MM/dd/yyyy") & " 00:00:00',102) ) "
            End If
            If data_fine <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Doc_Data <= CONVERT(DATETIME, '" & Format(CDate(data_fine), "MM/dd/yyyy") & " 23:59:59',102) ) "
            End If

            If idDocumentoLocale <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Doc_Id = '" & idDocumentoLocale & "' ) "
            End If



            If visualizzaFlagStatoStampato <> "" Then
                sWhere = sWhere & IIf(Trim(sWhere).EndsWith("WHERE"), " ", " AND ") & "  ( Doc_FlagStampato = " & visualizzaFlagStatoStampato & " ) "
            End If

            If sWhere.StartsWith(" AND") Then
                sWhere = " WHERE " & sWhere.Substring(4)
            End If

            If Not Trim(sWhere).EndsWith("WHERE") Then
                Sqlq = Sqlq & sWhere
            End If

            Dim lstrOrderBy As String = "" & oOperatore.Attributo("ORDER_ELENCO_DOCUMENTIUFFICIO")

            If lstrOrderBy = "" Then
                Sqlq = Sqlq & " ORDER BY  CAST(RIGHT(Doc_numero, 5) as int) ASC, Doc_data DESC, LEFT(Doc_numero, 4) ASC"
            Else
                Sqlq = Sqlq & " ORDER BY " & lstrOrderBy
            End If

            strConn = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            Dim flagEliminaUltimo As Boolean = False

            Dim contaRighe As Integer = 0
            Dim arr(7, contaRighe) As Object
            Try

                While (rdr.Read())
                    flagEliminaUltimo = False

                    ReDim Preserve arr(7, contaRighe)

                    For col As Integer = 0 To 4
                        arr(col, contaRighe) = rdr(col)
                    Next
                    Dim livelloCorrente As String = "" & rdr(5)
                    Dim notelivelloCorrente As String = "" & rdr(6)

                    arr(5, contaRighe) = "./risorse/immagini/blank.gif"
                    arr(6, contaRighe) = "./risorse/immagini/blank.gif"
                    arr(7, contaRighe) = "./risorse/immagini/blank.gif"



                    Dim listaInoltriRigettiUffici As IList = UltimeOperazioniInoltroRigetto(arr(0, contaRighe))
                    For Each Item As Object In listaInoltriRigettiUffici
                        Select Case CStr(Item(0))
                            Case "UFFICIO_DIRIGENZA_DIPARTIMENTO"
                                arr(5, contaRighe) = "'./risorse/immagini/" & getImageWorklist(Item(1))
                            Case "UFFICIO_CONTROLLO_AMMINISTRATIVO"
                                arr(6, contaRighe) = "'./risorse/immagini/" & getImageWorklist(Item(1))
                            Case "UFFICIO_RAGIONERIA"
                                arr(7, contaRighe) = "'./risorse/immagini/" & getImageWorklist(Item(1))
                        End Select
                    Next
                    If arr(4, contaRighe) = 0 Then
                        'se il documento non è contabile scrivo NO
                        arr(7, contaRighe) = "'./risorse/immagini/" & getImageWorklist("NO")
                    End If
                    Dim ultimoContaRigheValido As Integer = contaRighe
                    Select Case visualizzaLaTipRigetto
                        Case ""
                            contaRighe += 1
                        Case "UDD"
                            If arr(5, contaRighe).Contains(getImageWorklist("RIGETTO")) Then
                                contaRighe += 1
                            Else
                                flagEliminaUltimo = True
                            End If
                        Case "UCA"
                            If arr(6, contaRighe).Contains(getImageWorklist("RIGETTO")) Then
                                contaRighe += 1
                            Else
                                flagEliminaUltimo = True
                            End If
                        Case "UR"
                            If arr(7, contaRighe).Contains(getImageWorklist("RIGETTO")) Then
                                contaRighe += 1
                            Else
                                flagEliminaUltimo = True
                            End If


                    End Select

                    If "" & ConfigurationManager.AppSettings("NOTE_" & livelloCorrente) = "1" Then

                        Select Case livelloCorrente

                            Case "UDD"
                                If Not String.IsNullOrEmpty(notelivelloCorrente) Then
                                    arr(5, ultimoContaRigheValido) = "'./risorse/immagini/" & getImageWorklist("NOTE")
                                End If
                            Case "UCA"
                                If Not String.IsNullOrEmpty(notelivelloCorrente) Then
                                    arr(6, ultimoContaRigheValido) = "'./risorse/immagini/" & getImageWorklist("NOTE")
                                End If

                            Case "UR"
                                If Not String.IsNullOrEmpty(notelivelloCorrente) Then
                                    arr(7, ultimoContaRigheValido) = "'./risorse/immagini/" & getImageWorklist("NOTE")
                                End If
                        End Select
                    End If
                   
                End While

                rdr.Close()
                rdr = Nothing
            Catch ex As Exception
                contaRighe = 0
                rdr = Nothing
                vRitPar(0) = 9999
                vRitPar(1) = ex.Message
            End Try


            If flagEliminaUltimo Then
                contaRighe -= 1
                ReDim Preserve arr(11, contaRighe)
            End If

            vRitPar(0) = IIf(contaRighe > 0, 0, 1)
            vRitPar(1) = IIf(contaRighe > 0, arr, "Non sono stati trovati documenti che rispondono ai criteri di ricerca impostati")

        Catch ex As Exception
            vRitPar(0) = 9999
            vRitPar(1) = ex.Message
        End Try

        If vRitPar(0) <> 0 Then
            GoTo FineSub
        End If

        'Dim vRisultati(6, UBound(vRitPar(1), 2)) As String
        Dim vRisultati(8, UBound(vRitPar(1), 2)) As String
        Dim str As String = ""
        For i = 0 To UBound(vRitPar(1), 2)
            vRisultati(0, i) = vRitPar(1)(0, i) & "" 'idDocumento
            vRisultati(1, i) = vRitPar(1)(1, i) & "" 'Numero Documento (provvisorio o definitivo)
            vRisultati(2, i) = vRitPar(1)(2, i) & "" 'Data provvedimento

            str = vRitPar(1)(3, i) 'Oggetto
            'comm per eliminazione xml
            'str = str.Replace("&amp;", Chr(38))
            'str = str.Replace("&aps;", Chr(39))
            'str = str.Replace("&gt;", Chr(62))
            'str = str.Replace("&lt;", Chr(60))
            'str = str.Replace("&quot;", Chr(34))
            vRisultati(3, i) = str 'Oggetto Replace caratteri speciali
            vRisultati(4, i) = "" 'utilizzato per link APRI

            Dim IdUltimaVersione As String = IdUltimaVersioneDocumento(vRisultati(0, i))
            'modgg 10-06 4
            Sqlq = "SELECT     Sto_Prog, Sto_idAllegato " & _
                " FROM Azioni_Utente_Documento " & _
                " WHERE     (Sto_id_Doc = '" & vRisultati(0, i) & "') AND (Sto_TipoAttivita = 'firma') " & _
                "  AND (Sto_Info_Attivita like '" & IdUltimaVersione & "') " & _
                " AND (Sto_Utente = '" & oOperatore.Codice & "')"
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)
            Dim contaRighe2 As Integer = 0
            Dim arr2(1, contaRighe2) As Object
            Try
                While (rdr.Read())
                    ReDim Preserve arr2(1, contaRighe2)

                    For col As Integer = 0 To 1
                        arr2(col, contaRighe2) = rdr(col)
                    Next
                    contaRighe2 = contaRighe2 + 1

                End While
                rdr.Close()
                rdr = Nothing
            Catch ex As Exception
                contaRighe2 = 0
                rdr = Nothing
                vRitPar(0) = 9999
                vRitPar(1) = ex.Message
            End Try

            If contaRighe2 > 0 Then
                'documento firmato
                vRisultati(5, i) = "'./risorse/immagini/firma.gif' alt='Il documento risulta correttamente firmato' "
            Else
                'documento non firmato
                vRisultati(5, i) = "'./risorse/immagini/blank.gif'  alt='Il documento non risulta firmato' "
            End If

            vRisultati(6, i) = "" & vRitPar(1)(5, i) 'icona rigetto UDG
            vRisultati(7, i) = "" & vRitPar(1)(6, i) 'icona rigetto UCA
            vRisultati(8, i) = "" & vRitPar(1)(7, i) 'icona rigetto UR

        Next
        vRitPar(1) = vRisultati

FineSub:
        FO_Elenco_Documenti_Da_Stampare = vRitPar

        If vRitPar(0) <> 0 Then
            Call SISTEMA.Registra_Log(vRitPar(1), SFunzione)
        End If
        If SISTEMA.bTRACE Then
            Call SISTEMA.Registra_Trace("Fine", SFunzione)
        End If

        Exit Function

    End Function

    Public Overrides Function VerificaImportoLiquidazione(ByVal idDocumento As String) As Boolean
        ' Verifica innnazitutto che il blocco a livello di ufficio proponente sia abilitato 
        If ConfigurationManager.AppSettings.Get("BLOCCA_LIQUIDAZIONI_SENZA_BENEFICIARI") = 1 Then
            Dim vparm(1) As Object

            vparm(0) = idDocumento
            Dim objDoc As DllDocumentale.Model.DocumentoInfo = FO_Leggi_Documento_Object(vparm)
            Dim statoIstanzaDocumento As DllDocumentale.StatoIstanzaDocumentoInfo = Get_StatoIstanzaDocumento(idDocumento)

            ' Blocco nel caso in cui il dirigente dell'ufficio proponente stia cercando di inoltrare
            ' un documento contentente liquidazioni in cui non sono stati caricati i beneficiari 
            If (statoIstanzaDocumento.LivelloUfficio = "UP" And statoIstanzaDocumento.Ruolo = "R") Then
                Dim chkLiquidazione As String = objDoc.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
                Dim listaLiquidazione As IList = FO_Get_DatiLiquidazione(objDoc.Doc_id)
                Dim count As Integer = 0

                For Each liq As DllDocumentale.ItemLiquidazioneInfo In listaLiquidazione
                    Dim totale As Double = 0
                    If liq.Di_Stato <> 0 Then
                        Dim listaben As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = FO_Get_ListaBeneficiariLiquidazione(oOperatore, liq.Dli_Documento, , liq.Dli_prog)
                        For Each ben As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In listaben
                            totale = totale + ben.ImportoSpettante
                        Next
                    End If
                    If liq.Di_Stato = 2 Then
                        count = count + 1
                    End If
                    Dim flagDisabilitaArchivioLiquidazione As Boolean = IIf(ConfigurationManager.AppSettings("DISABILITA_ARCHIVIO_LIQUIDAZIONI") = "1", True, False)

                    If flagDisabilitaArchivioLiquidazione Then
                        If Math.Abs(totale - liq.Dli_Costo) >= 0.01 Then
                            Return False
                        End If
                    End If
                Next

                '' Liquidazioni da confermare?
                'If chkLiquidazione = "1" Then
                '    If listaLiquidazione.Count > 0 Then
                '        If count > 0 Then
                '            Return False
                '        End If
                '    End If
                'Else
                '    'ci sono liquidazioni contestuali
                '    If listaLiquidazione.Count > 0 Then
                '        If count > 0 Then
                '            Return False
                '        End If
                '    End If
                'End If
            End If
        End If

        Return True
    End Function

    Public Overrides Function AbilitaInoltroRigetto(ByVal statoIstanzaDocumento As DllDocumentale.StatoIstanzaDocumentoInfo) As Boolean
        Const SFunzione As String = "AbilitaInoltroRigetto"
        Log.Info(Now & " - " & oOperatore.Codice & " - " & SFunzione)
        Dim result As Boolean = True
        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio
        Dim vett_Rit_UltimaOperazione As Object = Nothing
        Dim ultimaOperazione As String = ""
        Dim vparm(1) As Object
        vparm(0) = statoIstanzaDocumento.Doc_id
        Dim objDoc As DllDocumentale.Model.DocumentoInfo = FO_Leggi_Documento_Object(vparm)

        oUffProponente.CodUfficio = objDoc.Doc_Cod_Uff_Prop
        idDocumento = statoIstanzaDocumento.Doc_id

        Log.Info(Now & " - " & oOperatore.Codice & " - " & statoIstanzaDocumento.LivelloUfficio)
        'controllo se non annullato
        If statoIstanzaDocumento.Ruolo = "A" Then
            result = False
            ErrDescrizione = "<span style='color:red'>L'atto risulta annullato</span><br/>"
            Return result
        Else
            result = True
        End If
        'controllo se ho in carico il documento
        If UCase(statoIstanzaDocumento.Operatore) = UCase(oOperatore.Codice) Then
            result = True
        Else
            result = False
            ErrDescrizione = "<span style='color:red'>L'atto non è più in carico all'utente</span><br/>"
            Return result
        End If

        Select Case statoIstanzaDocumento.LivelloUfficio
            Case "UP"
                'verifico ragioneria
                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioRagioneria)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case "RIGETTO FORMALE"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case ""
                            result = True
                    End Select
                End If
                
                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteriaPresidenzaSegretario)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case "RIGETTO FORMALE"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case ""
                            result = True
                    End Select
                End If

                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza (Controllo di Legittimità) è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case "RIGETTO FORMALE"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza (Controllo di Legittimità) è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case ""
                            result = True
                    End Select
                End If

                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteriaPresidenzaApprovazione)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case "RIGETTO FORMALE"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Segreteria di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case ""
                            result = True
                    End Select
                End If

                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioPresidenza)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case "RIGETTO FORMALE"
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'Ufficio di Presidenza è obbligatorio e vincolante</span><br/>"
                            Return result
                        Case ""
                            result = True
                    End Select
                End If

                'verifico inserimento note in caso di rigetto formale di uca
                vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioControlloAmministrativo)
                If vett_Rit_UltimaOperazione(0) = 0 Then
                    ultimaOperazione = vett_Rit_UltimaOperazione(1)
                    Select Case ultimaOperazione
                        Case "INOLTRO"
                            result = True
                        Case "RIGETTO"
                            result = True
                        Case "RIGETTO FORMALE"
                            'verifico la presenza di osservazioni lato up
                            Dim oss As DllDocumentale.Model.OsservazioneInfo = GetOsservazione(oOperatore.Codice, statoIstanzaDocumento.Doc_id, "UP")
                            If (Not oss Is Nothing) AndAlso (Not String.IsNullOrEmpty(oss.Testo)) Then
                                result = True
                            Else
                                result = False
                                ErrDescrizione = "<span style='color:red'>E' necessario confermare l'atto, digitando le osservazioni</span><br/>"
                                Return result
                            End If
                        Case ""
                            result = True
                    End Select
                    If ultimaOperazione.Contains("RIGETTO FORMALE") Then
                        'verifico la presenza di osservazioni lato up
                        Dim oss As DllDocumentale.Model.OsservazioneInfo = GetOsservazione(oOperatore.Codice, statoIstanzaDocumento.Doc_id, "UP")
                        If (Not oss Is Nothing) AndAlso (Not String.IsNullOrEmpty(oss.Testo)) Then
                            result = True
                        Else
                            result = False
                            ErrDescrizione = "<span style='color:red'>E' necessario confermare l'atto, digitando le osservazioni</span><br/>"
                            Return result
                        End If
                    End If
                End If
                Return result
            Case "UDD"
                'controllo se udd propone
                If statoIstanzaDocumento.CodiceUfficio = oUffProponente.CodUfficioDirigenzaDipartimento Then
                    'verifico ragioneria
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioRagioneria)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                        If ultimaOperazione.Contains("RIGETTO FORMALE") Then
                            result = False
                            ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                            Return result
                        End If
                    End If

                    'verifico inserimento note in caso di rigetto formale di uca
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioControlloAmministrativo)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = True
                            Case "RIGETTO FORMALE"
                                'verifico la presenza di osservazioni lato up
                                Dim oss As DllDocumentale.Model.OsservazioneInfo = GetOsservazione(oOperatore.Codice, statoIstanzaDocumento.Doc_id, "UDD")
                                If (Not oss Is Nothing) AndAlso (Not String.IsNullOrEmpty(oss.Testo)) Then
                                    result = True
                                Else
                                    result = False
                                    ErrDescrizione = "<span style='color:red'>E' necessario confermare l'atto, digitando le osservazioni</span><br/>"
                                    Return result
                                End If
                            Case ""
                                result = True
                        End Select
                        If ultimaOperazione.Contains("RIGETTO FORMALE") Then
                            'verifico la presenza di osservazioni lato up
                            Dim oss As DllDocumentale.Model.OsservazioneInfo = GetOsservazione(oOperatore.Codice, statoIstanzaDocumento.Doc_id, "UDD")
                            If (Not oss Is Nothing) AndAlso (Not String.IsNullOrEmpty(oss.Testo)) Then
                                result = True
                            Else
                                result = False
                                ErrDescrizione = "<span style='color:red'>E' necessario confermare l'atto, digitando le osservazioni</span><br/>"
                                Return result
                            End If
                        End If
                    End If
                    Return result
                Else
                    'verifico ultima mia azione come organo di controllo
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioDirigenzaDipartimento)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = True
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>L'atto è stato già rigettato formalmente. Altro rigetto non consentito</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                    End If
                    Return result
                End If

            Case "USL"
                'controllo se Ufficio di Segr di Presidenza - Legittimità propone
                If statoIstanzaDocumento.CodiceUfficio = oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita Then
                    'verifico ragioneria
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioRagioneria)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio Ragioneria, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                    End If

                    'verifico inserimento note in caso di rigetto formale di Segreteria di presidenza (gestione ODG)
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteriaPresidenzaSegretario)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio di Segreteria di Presidenza, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>Il rigetto dell'ufficio di Segreteria di Presidenza, è obbligatorio e vincolante</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                    End If
                    Return result
                Else
                    'verifico ultima mia azione come organo di controllo
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = True
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>L'atto è stato già rigettato formalmente. Altro rigetto non consentito</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                    End If
                    Return result
                End If

            Case "UCA"
                'controllo solo le determine
                If objDoc.Doc_Tipo = 0 Then
                    'verifico ultima mia azione
                    vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioControlloAmministrativo)
                    If vett_Rit_UltimaOperazione(0) = 0 Then
                        ultimaOperazione = vett_Rit_UltimaOperazione(1)
                        Select Case ultimaOperazione
                            Case "INOLTRO"
                                result = True
                            Case "RIGETTO"
                                result = True
                            Case "RIGETTO FORMALE"
                                result = False
                                ErrDescrizione = "<span style='color:red'>L'atto è stato già rigettato formalmente. Altro rigetto non consentito</span><br/>"
                                Return result
                            Case ""
                                result = True
                        End Select
                        If ultimaOperazione.Contains("RIGETTO FORMALE") Then
                            result = False
                            ErrDescrizione = "<span style='color:red'>L'atto è stato già rigettato formalmente. Altro rigetto non consentito</span><br/>"
                            Return result
                        End If
                    End If
                    Return result
                End If
        End Select
        Return result
    End Function
End Class
