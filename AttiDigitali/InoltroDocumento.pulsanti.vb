Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports DllDocumentale

Partial Class InoltroDocumento
    Private Sub gestionePulsanti_Regione()
        Dim docFirmato As Integer
        Dim idDocumento As String = ""
        If Not Request.QueryString.Get("key") Is Nothing Then
            idDocumento = Context.Request.QueryString.Get("key")
        Else
            idDocumento = Context.Session.Item("key")
        End If
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        'Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)

        Dim inoltroabilitato As Boolean = dllDoc.VerificaAbilitazioneInoltroRigetto(idDocumento, oOperatore, "INOLTRO") _
            And dllDoc.VerificaImportoLiquidazione(idDocumento)

        CreaWarning(idDocumento, oOperatore, dllDoc)

        btnInoltra.Enabled = inoltroabilitato
        Dim numera As Boolean = dllDoc.Numerare(idDocumento, oOperatore, Context.Request.Item("tipo"))
        Session.Add("numerare", numera)
        If (numera) Then
            Dim msgAlert As String = "return confirm('\t\tATTENZIONE:\n\nPremendo \'OK\' verrà assegnata la numerazione progressiva agli atti da firmare in modo definitivo.\n\nPer numerare e successivamente firmare premere \'OK\', altrimenti \'Annulla\'.')"
            'se si tratta di delibere, numera solo chi è abilitato da db
            If (Context.Request.Item("tipo") = 1) Then
                btnFirma.Attributes.Add("onclick", msgAlert)
            End If
        End If
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(idDocumento)
        'se si sta firmando una delibera che si strova in USL e sono il responsabile dell'ufficio segr di pres 
        '(per il visto di legittimita) per forza! Anche se l'atto è stato proposto e già firmato dallo stesso.
        '
        If Context.Request.Item("tipo") = 1 And statoIstanza.LivelloUfficio = "USL" And statoIstanza.Ruolo = "R" And
        oOperatore.oUfficio.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteriaPresidenzaLegittimita And oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
            docFirmato = 0
        ElseIf Context.Request.Item("tipo") = 1 And statoIstanza.LivelloUfficio = "UDD" And statoIstanza.Ruolo = "R" And
        oOperatore.oUfficio.CodUfficio = oOperatore.oUfficio.CodUfficioDirigenzaDipartimento And oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
            'se si sta firmando una delibera che si strova in UDD e sono il dirigente generale
            'Anche se l'atto è stato proposto e già firmato dallo stesso.
            docFirmato = 0
        Else
            docFirmato = Verifica_Firma_Utente_Documento(idDocumento, "INOLTRO")
        End If

        If docFirmato > 0 Then
            btnFirma.Visible = False
            btnInoltra.Text = "INOLTRA"
            Label1.Text = "Premi su INOLTRA per inviare il provvedimento al successivo responsabile"
            Label3.Text = ""
        Else
            ' il pulsante firma è visibile e quello di inoltro viene abilitato solo se non è previsto l'obbligo di firma
            If (oOperatore.Test_Ruolo("DT003") And Context.Request.Item("tipo") = 0) Or (oOperatore.Test_Ruolo("DL003") And Context.Request.Item("tipo") = 1) Or (oOperatore.Test_Ruolo("DS014") And Context.Request.Item("tipo") = 2) Then

                btnFirma.Visible = True
                btnFirma.Enabled = inoltroabilitato
                If (oOperatore.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And oOperatore.oUfficio.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria) And (docFirmato = 0) Then
                    btnInoltra.Enabled = False
                Else
                    If inoltroabilitato Then
                        ' dllDoc.OBBLIGO_FIRMA_INOLTRO(oOperatore) obbligo di firmare
                        'se c'Ã¨ obbligo, lo nego e lo disabilito

                        btnInoltra.Enabled = Not dllDoc.OBBLIGO_FIRMA_INOLTRO(oOperatore)
                    End If
                End If
            End If

            Dim numera1 As Boolean = dllDoc.Numerare(idDocumento, oOperatore, Context.Request.Item("tipo"))
            Session.Add("numerare", numera)
            If numera1 Then
                Label3.Text = "Premi su FIRMA e INOLTRA per numerare ed inviare il provvedimento al successivo responsabile"
            End If
        End If
        Dim warningMsg As ArrayList = Context.Session.Item("warning")
        For i As Integer = 0 To warningMsg.Count - 1
            If warningMsg.Item(i).ToString().Contains("seduta") Then
                btnFirma.Enabled = False
                btnInoltra.Enabled = False
            End If

            'If warningMsg.Item(i).ToString().Contains("Conservazione") Then
            '    btnFirma.Enabled = False
            '    'btnInoltra.Enabled = False
            'End If
        Next

    End Sub
    Private Sub gestionePulsantiMultiplo_Regione()

        Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

        Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)
        Dim docFirmato As Integer


        If vDocumentiDaInoltrare.Length = 1 Then
            Context.Session.Add("key", vDocumentiDaInoltrare(0))
            CreaWarning(vDocumentiDaInoltrare(0), oOperatore, dllDoc)
            gestionePulsanti()
            Dim numera As Boolean = dllDoc.Numerare(vDocumentiDaInoltrare(0), oOperatore, Context.Request.Item("tipo"))
            Session.Add("numerare", numera)
            If numera Then
                Label3.Text = "Premi su FIRMA e INOLTRA per numerare, marcare temporalmente ed inviare il provvedimento al successivo responsabile"
            End If
        Else
            Dim inoltroabilitato As Boolean = True
            Dim firmaabilitata As Boolean = True

            For Each docDaInoltrare As String In vDocumentiDaInoltrare
                Dim abilitaInoltro As Boolean = dllDoc.VerificaAbilitazioneInoltroRigetto(docDaInoltrare, oOperatore, "INOLTRO") _
                    And dllDoc.VerificaImportoLiquidazione(docDaInoltrare)

                If abilitaInoltro And inoltroabilitato Then
                    inoltroabilitato = True
                Else
                    inoltroabilitato = False
                End If

                docFirmato = Verifica_Firma_Utente_Documento(docDaInoltrare, "INOLTRO")
                If docFirmato And firmaabilitata And inoltroabilitato Then
                    firmaabilitata = True
                Else
                    firmaabilitata = False
                End If

                Dim numera As Boolean = dllDoc.Numerare(docDaInoltrare, oOperatore, Context.Request.Item("tipo"))
                Session.Add("numerare", numera)
                If numera Then
                    btnFirma.Visible = False
                    Label3.Text = "Premi su FIRMA e INOLTRA per numerare ed inviare il provvedimento al successivo responsabile"
                End If

            Next
            CreaWarning(vDocumentiDaInoltrare, oOperatore, dllDoc)
            btnInoltra.Enabled = inoltroabilitato
            If (oOperatore.Test_Ruolo("DT003") And Context.Request.Item("tipo") = 0) Or (oOperatore.Test_Ruolo("DL003") And Context.Request.Item("tipo") = 1) Or (oOperatore.Test_Ruolo("DS014") And Context.Request.Item("tipo") = 2) Then
                If firmaabilitata Then
                    btnFirma.Visible = False
                    btnInoltra.Text = "INOLTRA"
                    Label1.Text = "Premi su INOLTRA per inviare il provvedimento al successivo responsabile"
                    Label3.Text = ""
                Else
                    ' il pulsante firma è visibile e quello di inoltro viene abilitato solo se non è previsto l'obbligo di firma
                    btnFirma.Visible = True
                    btnFirma.Enabled = inoltroabilitato
                    If inoltroabilitato Then
                        btnInoltra.Enabled = Not dllDoc.OBBLIGO_FIRMA_INOLTRO(oOperatore)
                    End If
                    If (oOperatore.Test_Attributo("LIVELLO_UFFICIO", "RESPONSABILE") And oOperatore.oUfficio.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria) And (docFirmato = 0) Then
                        btnInoltra.Enabled = False
                    End If
                End If
            End If
        End If

        Dim warningMsg As ArrayList = Context.Session.Item("warning")
        For i As Integer = 0 To warningMsg.Count - 1
            If warningMsg.Item(i).ToString().Contains("seduta") Then
                btnFirma.Enabled = False
                btnInoltra.Enabled = False
            End If
            If warningMsg.Item(i).ToString().Contains("Conservazione") Then
                btnFirma.Enabled = False
                btnInoltra.Enabled = False
            End If
        Next


    End Sub
    Private Sub gestionePulsanti_Alsia()

        Dim idDocumento As String = ""
        If Not Request.QueryString.Get("key") Is Nothing Then
            idDocumento = Context.Request.QueryString.Get("key")
        Else
            idDocumento = Context.Session.Item("key")
        End If
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
        'elimino il bottone di firma 
        btnFirma.Visible = False
        Label3.Text = ""

        btnInoltra.Enabled = True
        If (oOperatore.Test_Ruolo("DT003") And Context.Request.Item("tipo") = 0) Or (oOperatore.Test_Ruolo("DL003") And Context.Request.Item("tipo") = 1) Or (oOperatore.Test_Ruolo("DS014") And Context.Request.Item("tipo") = 2) Then

            Dim docFirmato As Integer = Verifica_Firma_Utente_Documento(idDocumento, "INOLTRO")
            If docFirmato > 0 Then
                btnFirma.Visible = False
                btnInoltra.Text = "INOLTRA"
                Label1.Text = "Premi su INOLTRA per inviare il provvedimento al successivo responsabile"
                Label3.Text = ""
            Else
                ' il pulsante firma è visibile e quello di inoltro viene abilitato solo se no è previsto l'obbligo di firma
                btnFirma.Visible = True
                btnInoltra.Enabled = Not dllDoc.OBBLIGO_FIRMA_INOLTRO(oOperatore)
            End If

            Dim numera As Boolean = dllDoc.Numerare(idDocumento, oOperatore, Context.Request.Item("tipo"))
            Session.Add("numerare", numera)
            If numera Then
                Label3.Text = "Premi su FIRMA e INOLTRA per numerare ed inviare il provvedimento al successivo responsabile"
            End If
            CreaWarning(idDocumento, oOperatore, dllDoc)
        End If

    End Sub
    Private Sub gestionePulsantiMultiplo_Alsia()

        Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

        Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
        Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        'nascondo il bottone di firma 
        btnFirma.Visible = False
        Label3.Text = ""
        btnInoltra.Enabled = True
        If (oOperatore.Test_Ruolo("DT003") And Context.Request.Item("tipo") = 0) Or (oOperatore.Test_Ruolo("DL003") And Context.Request.Item("tipo") = 1) Or (oOperatore.Test_Ruolo("DS014") And Context.Request.Item("tipo") = 2) Then
            If vDocumentiDaInoltrare.Length = 1 Then
                Context.Session.Add("key", vDocumentiDaInoltrare(0))
                gestionePulsanti()
            Else
                For Each docDaInoltrare As String In vDocumentiDaInoltrare
                    Dim numera As Boolean = dllDoc.Numerare(docDaInoltrare, oOperatore, Context.Request.Item("tipo"))
                    Session.Add("numerare", numera)
                    If numera Then
                        btnFirma.Visible = True
                        Label3.Text = "Premi su FIRMA e INOLTRA per numerare ed inviare il provvedimento al successivo responsabile"
                    End If
                Next
                'inoltro sempre abilitato perchè il rigetto di UR non è vincolante
                btnInoltra.Enabled = True
            End If
            CreaWarning(vDocumentiDaInoltrare, oOperatore, dllDoc)
        End If

    End Sub
    Private Sub btnInoltraClickRegione()
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Session.Add("note", txtNote.Text & "")
        If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "COLLABORATORE") Then
            If Not Request.Form.GetValues("ddlsupervisore") Is Nothing AndAlso Request.Form.GetValues("ddlsupervisore")(0) <> "" Then
                Session.Add("prossimoAttore", Request.Form.GetValues("ddlsupervisore")(0))
            End If
        End If
        Session.Add("codAzione", "INOLTRO")
        If Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Or Context.Session.Item("elencoDocumentiDaInoltrare") = "" Then
            If Not Request.QueryString.Get("key") Is Nothing Then
                Session.Add("codDocumento", Request.QueryString.Get("key"))
            Else
                Session.Add("codDocumento", Context.Session.Item("key"))
                Context.Session.Remove("key")
            End If
            ''Modificarfe per SIC
            ModificaPreImp(Session("codDocumento"))

            Response.Redirect("InoltraDocumentoAction.aspx?tipo=" & TipoApplic(Context))


        Else
            Dim elencoDocumentiDaInoltrare As String
            Dim vDocumentiDaInoltrare() As String
            elencoDocumentiDaInoltrare = Context.Session.Item("elencoDocumentiDaInoltrare")

            vDocumentiDaInoltrare = Split(elencoDocumentiDaInoltrare, ",")


            ''Modificarfe per SIC

            'Controllo sull'oggetto
            For x As Integer = 0 To vDocumentiDaInoltrare.Length - 1
                ModificaPreImp(vDocumentiDaInoltrare(x))
            Next

            Response.Redirect("InoltraBloccoDocumentiAction.aspx?tipo=" & TipoApplic(Context))
        End If
    End Sub
    Private Sub btnInoltraClickAlsia()
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Session.Add("note", txtNote.Text & "")
        If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4LIVELLO") Or oOperatore.Test_Attributo("LIVELLO_UFFICIO", "COLLABORATORE") Then
            If Not Request.Form.GetValues("ddlsupervisore") Is Nothing AndAlso Request.Form.GetValues("ddlsupervisore")(0) <> "" Then
                Session.Add("prossimoAttore", Request.Form.GetValues("ddlsupervisore")(0))
            End If
        End If
        Session.Add("codAzione", "INOLTRO")
        If Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Or Context.Session.Item("elencoDocumentiDaInoltrare") = "" Then
            If Not Request.QueryString.Get("key") Is Nothing Then
                Session.Add("codDocumento", Request.QueryString.Get("key"))
            Else
                Session.Add("codDocumento", Context.Session.Item("key"))
                Context.Session.Remove("key")
            End If
            ''Modificarfe per SIC
            ModificaPreImp(Session("codDocumento"))

            Response.Redirect("InoltraDocumentoAction.aspx?tipo=" & TipoApplic(Context))


        Else
            Dim elencoDocumentiDaInoltrare As String
            Dim vDocumentiDaInoltrare() As String
            elencoDocumentiDaInoltrare = Context.Session.Item("elencoDocumentiDaInoltrare")

            vDocumentiDaInoltrare = Split(elencoDocumentiDaInoltrare, ",")


            ''Modificarfe per SIC

            'Controllo sull'oggetto
            For x As Integer = 0 To vDocumentiDaInoltrare.Length - 1
                ModificaPreImp(vDocumentiDaInoltrare(x))
            Next



            Response.Redirect("InoltraBloccoDocumentiAction.aspx?tipo=" & TipoApplic(Context))
        End If
    End Sub

    Sub ModificaPreImpSIC(ByVal codDocumento As String)

        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
        Dim flusso As String = DefinisciFlusso(objDocumento.Doc_Tipo)
        '  ModificaPreImpSIC(codDocumento)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
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

            VariaPreimpPerRiduzioni(codDocumento, tipoAtto, numeroAtto, dataAtto, objDocumento.Doc_Oggetto, numeroProvvisOrDefAtto)
        End If



    End Sub
    Public Sub VariaPreimpPerRiduzioni(ByVal idDocumento As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal oggettoAtto As String, ByVal numeroProvvisOrDefAtto As String)

        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaRiduzioni = dllDoc.FO_Get_DatiImpegniVariazioni(idDocumento)
        Dim flagResult As Boolean = False
        
        For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni

            Try
                'Gestione per riportare i soldi sul capitolo
                If Not item.Div_IsEconomia And item.Di_Stato = 1 Then
                    Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_GetImpegno(item.Dli_NumImpegno)
                    If listaImpegni.Count = 1 Then
                        'Fix Riduzione Preimpegno solo nel caso in cui è stata eseguita una riduzione di un impegno dell'anno contabile corrente 
                        If listaImpegni(0).Dli_Esercizio = item.DBi_Anno Then
                            If listaImpegni(0).Di_PreImpDaPrenotazione <> 1 Then
                                'Riduco il Pre Imp
                                'stefffffff da verificare se il token lo devo ricalcolare ex-novo o va bene passare quello dell'imp padre
                                ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazionePreIMPMessage(oOperatore, listaImpegni(0).Dli_NPreImpegno, -item.Dli_Costo, tipoAtto, dataAtto, numeroAtto, dataAtto, oggettoAtto, idDocumento, numeroProvvisOrDefAtto, item.HashTokenCallSic)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                If Not ex.Message.Contains("Mancata Disponibilita' Fondi PreImpegno") Then
                    HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                    Response.Redirect("MessaggioErrore.aspx")
                End If


            End Try

        Next
    End Sub
    Public Sub ModificaPreImp(ByVal codDocumento As String)

        Select Case HttpContext.Current.Application.Item("CONTABILITA")
            Case "SIC"
                ModificaPreImpSIC(codDocumento)
        End Select

    End Sub

    Private Sub CreaWarning(ByVal iddocumento As String, ByVal ooperatore As DllAmbiente.Operatore, ByRef dlldoc As DllDocumentale.svrDocumenti)
        Dim messaggi_di_Warning As New ArrayList
        Dim primoSuggerimento As String = ""
        Dim mess As String = ""
        Dim tipoSuggerimento As String = ""
        Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(iddocumento)
        If listaSugg.Count > 0 And String.IsNullOrEmpty(tipoSuggerimento) Then
            'Ci sono suggerimenti
            tipoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
            mess = " ultimo suggerimento attribuito: " & tipoSuggerimento
        End If

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dlldoc.Get_StatoIstanzaDocumento(iddocumento)
        Dim messaggioonclick As String = ""

        If Not String.IsNullOrEmpty(dlldoc.ErrDescrizione) Then
            messaggi_di_Warning.Add(dlldoc.ErrDescrizione)
            dlldoc.ErrDescrizione = ""
        End If
        If statoIstanza.LivelloUfficio = "UDD" And statoIstanza.CodiceUfficio = ooperatore.oUfficio.CodUfficio And statoIstanza.Ruolo <> "R" Then
            'propone la direzione generale            
            Dim warnings As WarningSet = Nothing
            messaggioonclick = Warning(iddocumento, warnings)

            If messaggioonclick.Contains("lblRed") Then
                Dim popupPanel = ""
                Dim confirmMessage As String = "Il riepilogo contiene anomalie."
                If warnings.containsOnly(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) OrElse
                   (warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) AndAlso
                    warnings.contains(WarningType.SIGN_WARNING) AndAlso warnings.count() = 2) Then
                    confirmMessage = "Vi sono delle operazioni non confermate."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) Then
                    confirmMessage = "Il riepilogo contiene anomalie tra le quali operazioni non confermate."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_IMPEGNO) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello dell\'impegno."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_LIQUIDAZIONE) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello della liquidazione."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                End If

                btnInoltra.Attributes.Add("onclick", popupPanel)
                btnFirma.Attributes.Add("onclick", popupPanel)
            End If
            If messaggioonclick.Contains("lblRFirma") Then
                Dim popupPanel = ""
                Dim confirmMessage As String = "Il riepilogo contiene anomalie sulla firma."
                If warnings.containsOnly(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) OrElse
                   (warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) AndAlso
                    warnings.contains(WarningType.SIGN_WARNING) AndAlso warnings.count() = 2) Then
                    confirmMessage = "Vi sono delle operazioni non confermate e anomalie sulla firma"
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) Then
                    confirmMessage = "Il riepilogo contiene anomalie tra le quali operazioni non confermate e problemi sulla firma"
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_IMPEGNO) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello dell\'impegno."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_LIQUIDAZIONE) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello della liquidazione."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                End If

                btnInoltra.Attributes.Add("onclick", popupPanel)
            End If

            messaggi_di_Warning.Add(messaggioonclick)
        End If

        If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
            Dim warnings As WarningSet = Nothing
            messaggioonclick = Warning(iddocumento, warnings)

            If messaggioonclick.Contains("lblRed") Then
                Dim popupPanel = ""
                Dim confirmMessage As String = "Il riepilogo contiene anomalie."
                If warnings.containsOnly(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) OrElse
                   (warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) AndAlso
                    warnings.contains(WarningType.SIGN_WARNING) AndAlso warnings.count() = 2) Then
                    confirmMessage = "Vi sono delle operazioni non confermate."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) Then
                    confirmMessage = "Il riepilogo contiene anomalie tra le quali operazioni non confermate."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_IMPEGNO) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello dell\'impegno."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_LIQUIDAZIONE) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello della liquidazione."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                End If

                btnInoltra.Attributes.Add("onclick", popupPanel)
                btnFirma.Attributes.Add("onclick", popupPanel)
            End If
            If messaggioonclick.Contains("lblRFirma") Then
                Dim popupPanel = ""
                Dim confirmMessage As String = "Il riepilogo contiene anomalie e si sta inoltrando senza firma."
                If warnings.containsOnly(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) OrElse
                   (warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) AndAlso
                    warnings.contains(WarningType.SIGN_WARNING) AndAlso warnings.count() = 2) Then
                    confirmMessage = "Vi sono delle operazioni non confermate e si sta inoltrando senza firma."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NOT_CONFIRMED_OPERATIONS_WARNING) Then
                    confirmMessage = "Il riepilogo contiene anomalie tra le quali operazioni non confermate e si sta inoltrando senza firma."
                    popupPanel = "return confirm('" + confirmMessage + "\nContinuare?')"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_IMPEGNO) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello dell\'impegno."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                ElseIf warnings.contains(WarningType.NO_BENEFICIARI_LIQUIDAZIONE) Then
                    confirmMessage = "Importo assegnato ai beneficiari non corrispondente a quello della liquidazione."
                    popupPanel = "alert('" + confirmMessage + "'); return false"
                End If

                btnInoltra.Attributes.Add("onclick", popupPanel)
            End If

            messaggi_di_Warning.Add(messaggioonclick)
        End If
        If statoIstanza.LivelloUfficio = "UR" Then
            messaggi_di_Warning.Add(Warning_Ragioneria(iddocumento))
        End If
        If statoIstanza.LivelloUfficio = "USS" Then
            messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(iddocumento))
        End If

        'Try
        '    Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)
        '    Dim client As ConservazioneAttiPortTypeClient = New ConservazioneAttiPortTypeClient()

        '     Dim requestMarca As GetMarcaTemporaleRequestType = New GetMarcaTemporaleRequestType()
        '    Dim example As String = "text"

        '    requestMarca.FileDaMarcare = Encoding.UTF8.GetBytes(example) 'colonna bAllegato  
        '    requestMarca.applicationID = Dic_FODocumentale.cfo_Documento_Conservazione_Test_WS_Marca

        '    Dim responseMarca As GetMarcaTemporaleResponseType = client.GetMarcaTemporale(requestMarca)
        '    If responseMarca.MarcaTemporaleFile Is Nothing
        '        Throw New Exception("Errore Servizio Marca Temporale")
        '    End If

        'Catch ex As Exception
        '    messaggi_di_Warning.Add("<br/><i>ATTENZIONE: </i><span class='lblRed'>Il servizio di Marcatura Temporale per la Conservazione a Norma non risulta disponibile</span><br/>")

        'End Try

        Context.Session.Add("warning", messaggi_di_Warning)
        If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
            If mess.Contains("rigett") Then
                btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If
        End If



    End Sub
    Private Sub CreaWarning(ByVal vDocumentiDaInoltrare() As String, ByVal ooperatore As DllAmbiente.Operatore, ByRef dlldoc As DllDocumentale.svrDocumenti)
        Dim messaggi_di_Warning As New ArrayList
        Dim primoSuggerimento As String = ""
        Dim mess As String = ""
        Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(vDocumentiDaInoltrare(0))
        If listaSugg.Count > 0 And String.IsNullOrEmpty(primoSuggerimento) Then
            'Ci sono suggerimenti
            primoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
            mess = " ultimo suggerimento attribuito: " & primoSuggerimento
        End If

        If Not String.IsNullOrEmpty(dlldoc.ErrDescrizione) Then
            messaggi_di_Warning.Add(dlldoc.ErrDescrizione)
            dlldoc.ErrDescrizione = ""
        End If

        For Each docDaInoltrare As String In vDocumentiDaInoltrare
            If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                listaSugg = Elenco_Suggerimenti(docDaInoltrare)

                If listaSugg.Count > 0 Then
                    If listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia <> primoSuggerimento Then
                        mess = "Sono stati selezionati documenti che riportano suggerimenti discordanti, prima di procedere verificare le operazione da effettuare"
                    End If
                End If
            End If
            Dim messaggioOnClick As String = ""
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dlldoc.Get_StatoIstanzaDocumento(docDaInoltrare)
            If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
                messaggioOnClick = Warning(docDaInoltrare)
                If messaggioOnClick.Contains("lblRed") Then
                    btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                    btnFirma.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                End If
                If messaggioOnClick.Contains("lblRFirma") Then
                    btnInoltra.Attributes.Add("onclick", "return confirm('Si sta inoltrando senza firma, continuare?')")
                End If
                messaggi_di_Warning.Add(messaggioOnClick)
            End If
            If statoIstanza.LivelloUfficio = "UR" Then
                messaggi_di_Warning.Add(Warning_Ragioneria(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "USS" Then
                messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(docDaInoltrare))
            End If

        Next
        Context.Session.Add("warning", messaggi_di_Warning)
        If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
            If mess.Contains("rigett") Then
                btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If
            If mess.Contains("discordanti") Then
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If

        End If
    End Sub
End Class
