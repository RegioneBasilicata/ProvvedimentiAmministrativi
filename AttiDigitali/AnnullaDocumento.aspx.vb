Imports System.Collections.Generic

Public Class AnnullaDocumento
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnInoltra As System.Web.UI.WebControls.Button
    Protected WithEvents btnRigetta As System.Web.UI.WebControls.Button
    Protected WithEvents btnAnnulla As System.Web.UI.WebControls.Button
    Protected WithEvents pnlInoltro As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlRigetto As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlAnnullo As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlNote As System.Web.UI.WebControls.Panel
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox

    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSupervisore As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblAnnulla As System.Web.UI.WebControls.Label
    Protected WithEvents pnlAggiungiAllegato As System.Web.UI.WebControls.Panel
    Protected WithEvents fileUploadAllegato As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents LblErrore As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Inizializza_Pagina(Me, "Annulla Documento")
    End Sub

#End Region

    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AnnullaDocumento))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Try
            If Not IsPostBack Then
                'modgg 10-06 1
                Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")


                If Not Context.Session.Item("erroreFile") Is Nothing Then
                    LblErrore = New Label
                    LblErrore.Text = Context.Session.Item("erroreFile")
                    LblErrore.Visible = True
                Else
                    Context.Session.Remove("erroreFile")
                End If

                Dim tipoDoc As String = Context.Request.QueryString.Get("tipo")
                Dim lstr_desc_Documento As String = ""
                Select Case tipoDoc
                    Case 0
                        lstr_desc_Documento = "determina"
                    Case 1
                        lstr_desc_Documento = "delibera"
                    Case 2
                        lstr_desc_Documento = "disposizione"
                End Select
                lblAnnulla.Text = lblAnnulla.Text & lstr_desc_Documento
                Dim flagAbilitaAnnulla As Boolean = False
                If (oOperatore.Test_Ruolo("DT005") And tipoDoc = 0) Or (oOperatore.Test_Ruolo("DL005") And tipoDoc = 1) Or (oOperatore.Test_Ruolo("DS016") And tipoDoc = 2) Then
                    flagAbilitaAnnulla = True
                End If

                If flagAbilitaAnnulla Then
                    btnAnnulla.Attributes.Add("onclick", "return confirm('Premendo OK la " & lstr_desc_Documento & " non sarà più presente nella tua lista lavoro, ma solo visibile nel monitor, continuare?')")
                    Contenuto.Controls.Add(pnlAnnullo)
                Else
                    pnlAnnullo.Visible = False
                End If
                Contenuto.Controls.Add(pnlNote)
            End If

            If Not Context.Request.Params.Item("key") Is Nothing Or Context.Session.Item("key") Then
                Context.Session.Remove("elencoDocumentiDaInoltrare")
                gestionePulsanti()
            Else
                If Not Context.Session.Item("elencoDocumentiDaInoltrare") Is Nothing Then

                Else
                    Throw New Exception("E' necessario selezionare almeno un provvedimento prima di inoltrare")
                End If
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub
    Private Sub gestionePulsanti()
        Dim idDocumento As String = ""
        If Not Request.QueryString.Get("key") Is Nothing Then
            idDocumento = Context.Request.QueryString.Get("key")
        Else
            idDocumento = Context.Session.Item("key")
        End If
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)

        Dim annulloabilitato As Boolean = dllDoc.VerificaAbilitazioneInoltroRigetto(idDocumento, oOperatore, "ANNULLO")
        btnAnnulla.Enabled = annulloabilitato

        Dim ultimaOpUR As String = dllDoc.VERIFICA_AZIONE_UFFICIO(oOperatore.oUfficio.CodUfficioRagioneria, , idDocumento)(1)

        If ultimaOpUR = "RIGETTO" Then
            lblAnnulla.Text = "Premi Ritira per ritirare la determina"
            btnAnnulla.Text = "Ritira"
        End If
    End Sub




    Private Sub btnAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
        Session.Add("codAzione", "ANNULLO")

        Session.Add("codDocumento", Request.QueryString.Get("key"))

        Dim tipo As String = TipoApplic(Context)

        Dim key As String = Session("codDocumento")


        EliminaLiquidazioni(key)
        EliminaImpegni(key)
        EliminaPreimpegni(key)


        EliminaRiduzioni(key)
        EliminaRiduzioniLiq(key)
        EliminaRiduzioniPreImp(key)

        Session.Add("note", "")
        Response.Redirect("AnnullaDocumentoAction.aspx?tipo=" & tipo)
    End Sub

    Private Sub EliminaPreimpegni(ByVal key As String)
        Dim listaPreimpegni As IList(Of DllDocumentale.ItemImpegnoInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        Dim numeroProvvisOrDefAtto As String = ""
        If objDocumento.Doc_numero = "" then
	        numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
        Else
	        numeroProvvisOrDefAtto = objDocumento.Doc_numero
        End If

        listaPreimpegni = dllDoc.FO_Get_DatiPreImpegni(key)
        Dim flagResult As Boolean = False
        Dim lstr_idPreimpegnoDACancellare As String = ""
        For Each preimpegno As DllDocumentale.ItemImpegnoInfo In listaPreimpegni
            Try
                If preimpegno.Di_Stato <> 0 Then

                    lstr_idPreimpegnoDACancellare = preimpegno.Dli_NPreImpegno

                   
                    Dim tipoAtto As String = ""
                    Select Case objDocumento.Doc_Tipo
                        Case 0
                            tipoAtto = "DETERMINA"
                        Case 1
                            tipoAtto = "DELIBERA"
                        Case 2
                            tipoAtto = "DISPOSIZIONE"
                    End Select
                    Dim numeroAtto As String
                    numeroAtto = IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero)
                    numeroAtto = Right(numeroAtto, 5)
                    Dim dataAtto As Date
                    dataAtto = objDocumento.Doc_Data

                   


                    flagResult = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(oOperatore, preimpegno.Dli_NPreImpegno, tipoAtto, dataAtto, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())

                    dllDoc.FO_Delete_Logica_Preimpegno(preimpegno)
                    lstr_idPreimpegnoDACancellare = ""
                End If


            Catch ex As Exception
                Log.Error("EliminaPreimpegni iddoc: " & key & " Dli_NPreImpegno" & lstr_idPreimpegnoDACancellare)
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        Next

    End Sub
    Private Sub EliminaImpegni(ByVal key As String)
        Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo)
       
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        Dim numeroProvvisOrDefAtto As String = ""
        If objDocumento.Doc_numero = "" then
	        numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
        Else
	        numeroProvvisOrDefAtto = objDocumento.Doc_numero
        End If

        listaImpegni = dllDoc.FO_Get_DatiImpegni(key)
        Dim flagResult As Boolean = False
        Dim lstr_idImpegnoDACancellare As String = ""
        For Each item As DllDocumentale.ItemImpegnoInfo In listaImpegni
            Try
                If item.Di_Stato <> 0 Then

                    lstr_idImpegnoDACancellare = item.Dli_NPreImpegno
                    If item.Di_Stato = 1 Then


                        If item.Di_PreImpDaPrenotazione Then
                            flagResult = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(oOperatore, "C", item.Dli_NPreImpegno, item.Dli_Costo)
                        Else
                            If String.IsNullOrEmpty(item.NDocPrecedente) Then
                                
                                Dim tipoAtto As String
                                Select Case objDocumento.Doc_Tipo
                                    Case 0
                                        tipoAtto = "DETERMINA"
                                    Case 1
                                        tipoAtto = "DELIBERA"
                                    Case 2
                                        tipoAtto = "DISPOSIZIONE"
                                End Select
                                Dim numeroAtto As String
                                numeroAtto = IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero)
                                numeroAtto = Right(numeroAtto, 5)
                                Dim dataAtto As Date
                                dataAtto = objDocumento.Doc_Data
                                flagResult = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(oOperatore, item.Dli_NPreImpegno, tipoAtto, dataAtto, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())

                                'SE è IMPEGNO PERENTE NON DEVO CANCELLARE NULLA
                            End If
                        End If
                    End If

                    dllDoc.FO_Delete_Logica_Bil(item)
                    lstr_idImpegnoDACancellare = ""
                End If


            Catch ex As Exception
                Log.Error("EliminaImpegni iddoc: " & key & " Dli_NPreImpegno" & lstr_idImpegnoDACancellare)
                HttpContext.Current.Session.Add("error", ex.Message)
                Response.Redirect("MessaggioErrore.aspx")
            End Try
        Next

    End Sub


    Private Sub EliminaLiquidazioni(ByVal key As String)
     
        Dim listaLiquidazioni As IList(Of DllDocumentale.ItemLiquidazioneInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim ProcAmm As New ProcAmm
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)


        listaLiquidazioni = dllDoc.FO_Get_DatiLiquidazione(key)
        Dim lstr_idLiquidazione As String = ""
        Try

            For Each item As DllDocumentale.ItemLiquidazioneInfo In listaLiquidazioni

                'CANCELLA FATTURE SUL SIC 
                Dim P_ESITO_PROVV As Double = 0
                Dim P_ESITO_DEF As Double = 0

                Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattByLiquidazione(item.Dli_prog, objDocumento.Doc_id)
                If Not listaFattureLiquidazioneEXT Is Nothing Then
                    ' per ogni fattura è necessario chiamare la cancellazione al SIC E SUL NS DB:
                    '   1°: numero provvisorio dell'atto con num liq: 0
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


                If item.Di_Stato <> 0 Then
                    lstr_idLiquidazione = item.Dli_prog
                    dllDoc.FO_Delete_Logica_Liq(item)
                End If
                lstr_idLiquidazione = ""
            Next

            Dim listaFattureAttoEXT As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattureAtto(objDocumento.Doc_id)
            For Each fatturaAtto As Ext_FatturaInfo In listaFattureAttoEXT
                dllDoc.FO_Delete_FatturaByIdFatturaSIC(fatturaAtto.IdUnivoco, objDocumento.Doc_id)
            Next

        Catch ex As Exception
            Log.Error("EliminaLiquidazioni iddoc: " & key & " Dli_prog liquidazione " & lstr_idLiquidazione)
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub
    Private Sub EliminaRiduzioni(ByVal key As String)
       Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
      
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)

        listaRiduzioni = dllDoc.FO_Get_DatiImpegniVariazioni(key)

        Dim lstr_idRiduzioni As String = ""

        Try
            For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni
                If item.Di_Stato <> 0 Then
                    lstr_idRiduzioni = item.Dli_prog
                    dllDoc.FO_Delete_Logica_Impegno_Var(item)
                End If
                lstr_idRiduzioni = ""
            Next
        Catch ex As Exception
            Log.Error("EliminaRiduzioni iddoc: " & key & " Dli_prog impengo_var " & lstr_idRiduzioni)
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub
    Private Sub EliminaRiduzioniLiq(ByVal key As String)
        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneLiqInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
       
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)

        listaRiduzioni = dllDoc.FO_Get_DatiLiquidazioniVariazioni(key)

        Dim lstr_idRiduzioni As String = ""

        Try
            For Each item As DllDocumentale.ItemRiduzioneLiqInfo In listaRiduzioni
                If item.Di_Stato <> 0 Then
                    lstr_idRiduzioni = item.Dli_prog
                    dllDoc.FO_Delete_Logica_Liquidazione_Var(item)
                End If
                lstr_idRiduzioni = ""
            Next
        Catch ex As Exception
            Log.Error("EliminaRiduzioniLiq iddoc: " & key & " Dli_prog liquidazione_var " & lstr_idRiduzioni)
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub
    Private Sub EliminaRiduzioniPreImp(ByVal key As String)
        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)


        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
       
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)

        listaRiduzioni = dllDoc.FO_Get_DatiPreImpegniVariazioni(key)

        Dim lstr_idRiduzioni As String = ""

        Try
            For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni
                If item.Di_Stato <> 0 Then
                    lstr_idRiduzioni = item.Dli_prog
                    dllDoc.FO_Delete_Logica_PreImpegno_Var(item)
                End If
                lstr_idRiduzioni = ""
            Next
        Catch ex As Exception
            Log.Error("EliminaRiduzioniPreImp iddoc: " & key & " Dli_prog preimpegno_var " & lstr_idRiduzioni)
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try


    End Sub
End Class
