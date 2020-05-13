Imports System.Configuration
Partial Public Class svrDocumentiAlsia
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
            '  Sqlq = " SELECT a.Doc_Id, case when isnull(a.Doc_NumDefinitivo ,'') = '' then isnull(a.Doc_numero,'')+'' else isnull(a.Doc_NumDefinitivo,'')+' - '+isnull(a.Doc_numero,'')+'' end, a.doc_data,a.Doc_Oggetto, a.Doc_isContabile,a.Livello_Ufficio, a.osservazioniLivello  from (" & _

            Sqlq = " SELECT a.Doc_Id, a.Doc_NumDefinitivo, a.Doc_Numero,a.doc_data,a.Doc_Oggetto, struttura.str_descrBreve as ufficio,'',''  from (" & _
                      " select Documento.Doc_Id , Doc_numero ,doc_data  , " & _
                      "         dbo.fn_ReplaceCaratteriSpeciali(isnull(Doc_Oggetto,'')) as Doc_Oggetto ,Doc_Cod_Uff_Prop, isnull(Doc_FlagStampato,0) as  Doc_FlagStampato " & _
                      " , Doc_NumDefinitivo as Doc_NumDefinitivo" & _
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
                Sqlq = Sqlq & " ORDER BY  CAST(RIGHT(Doc_NumDefinitivo, 5) as int) ASC, Doc_data DESC, LEFT(Doc_numero, 4) ASC"
            Else
                Sqlq = Sqlq & " ORDER BY " & lstrOrderBy
            End If

            strConn = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")
            rdr = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, Sqlq, Nothing)

            Dim contaRighe As Integer = 0
            Dim arr(7, contaRighe) As Object
            Try

                While (rdr.Read())

                    ReDim Preserve arr(7, contaRighe)

                    For col As Integer = 0 To 6
                        arr(col, contaRighe) = rdr(col)
                    Next
                    contaRighe += 1
                End While

                rdr.Close()
                rdr = Nothing
            Catch ex As Exception
                contaRighe = 0
                rdr = Nothing
                vRitPar(0) = 9999
                vRitPar(1) = ex.Message
            End Try

            vRitPar(0) = IIf(contaRighe > 0, 0, 1)
            vRitPar(1) = IIf(contaRighe > 0, arr, "Non sono stati trovati documenti che rispondono ai criteri di ricerca impostati")

        Catch ex As Exception
            vRitPar(0) = 9999
            vRitPar(1) = ex.Message
        End Try

        If vRitPar(0) <> 0 Then
            GoTo FineSub
        End If

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
        ' Non fa nulla. In Alsia non c'è integrazione contabile
        Return True
    End Function

    Public Overrides Function AbilitaInoltroRigetto(ByVal statoIstanzaDocumento As DllDocumentale.StatoIstanzaDocumentoInfo) As Boolean
        Log.Info(Now & " - " & oOperatore.Codice & " - " & statoIstanzaDocumento.LivelloUfficio)
        idDocumento = statoIstanzaDocumento.Doc_id
        Dim result As Boolean = True
        'controllo se non annullato
        If statoIstanzaDocumento.Ruolo = "A" Then
            result = False
            ErrDescrizione = "L'atto risulta annullato"
            Return result
        Else
            result = True
        End If
        'controllo se ho in carico il documento
        If LCase(statoIstanzaDocumento.Operatore) = LCase(oOperatore.Codice) Or LCase(statoIstanzaDocumento.Operatore) = LCase(getUtenteArchivio()) Then
            result = True
        Else
            result = False
            ErrDescrizione = "L'atto non è più in carico all'utente"
            Return result
        End If

        Return result
    End Function

End Class

