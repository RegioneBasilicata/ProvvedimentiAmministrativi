Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms.VisualStyles
Imports ClientIntegrazioneSic.Intema.WS.Ana.Richiesta
Imports ClientIntegrazioneSic.Intema.WS.Ana.Risposta
Imports ClientIntegrazioneSic.Intema.WS.Risposta
Imports DllAmbiente
Imports DllDocumentale
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

Public Class BeneficiariFromFile
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()

        Inizializza_Pagina(Me, "Testo")
    End Sub

#End Region
    Private _numeroDocumento As String = ""
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(BeneficiariFromFile))
    Private _svrDocumenti As svrDocumenti

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg 10-06 1
         Log.Info("*********** BeneficiariFromFile Page_Load")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim codDocumento As String = Context.Request.QueryString.Get("key")
        Dim vR As Object = Leggi_Documento(codDocumento)
        Dim codiceUfficioProponente As String = vR(2)
        Try
            If Not IsPostBack Then
                Dim descrizioneDocumento As String = "Caricamento massivo beneficiari"
                Dim numeroPubblicoDocumento As String = Context.Items.Item("numeroDoc")
                Rinomina_Pagina(Me, descrizioneDocumento & " " & numeroPubblicoDocumento)

                Dim listaErrori As Collections.Generic.List(Of String) = Context.Session.Item("listaErrori")
                Dim _isUploadedFile As Boolean = Context.Session.Item("_isUploadedFile")
                If _isUploadedFile Then
                    If Not listaErrori Is Nothing AndAlso listaErrori.Count > 0 Then
                        BulletedList.Items.Add("Errori: ")
                        For Each errore As String In listaErrori
                            BulletedList.Items.Add(errore)
                            Context.Session.Remove("listaErrori")
                        Next
                    Else
                        BulletedList.Items.Add("Esito: ")
                        BulletedList.Items.Add("Caricamento avvenuto con successo")
                    End If

                    Context.Session.Remove("_isUploadedFile")
                End If


                Context.Session.Remove("numeroPubblicoDocumento")
                Context.Session.Add("numeroPubblicoDocumento", numeroPubblicoDocumento)

                fileUpLoadAllegato.Accept = "application/vnd.ms-excel"
                Label1.Text = HttpContext.Current.Session.Item("erroreFile")
                HttpContext.Current.Session.Remove("erroreFile")

                Contenuto.Controls.Add(Label1)
                Contenuto.Controls.Add(Label2)
                'Qui va testato se deve essere uploadato il file oppure no
                _svrDocumenti = New DllDocumentale.svrDocumenti(oOperatore)
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (_svrDocumenti).Get_StatoIstanzaDocumento(Context.Request.QueryString.Item("key"))
                Dim livelloUfficio As String = statoIstanza.LivelloUfficio
                Dim ruolo As String = "" & statoIstanza.Ruolo
                If ruolo <> "A" AndAlso (oOperatore.oUfficio.CodUfficio = codiceUfficioProponente And livelloUfficio = "UP") Or
                ((oOperatore.oUfficio.CodUfficio = codiceUfficioProponente And oOperatore.oUfficio.bUfficioDirigenzaDipartimento And livelloUfficio = "UDD")) Or
                ((oOperatore.oUfficio.CodUfficio = codiceUfficioProponente And oOperatore.oUfficio.bUfficioSegreteriaPresidenzaLegittimita And livelloUfficio = "USL")) Then
                    pnlAggiungiAllegato.Visible = True
                    Contenuto.Controls.Add(pnlAggiungiAllegato)
                    Contenuto.Controls.Add(Panel1)
                    btnSalvaDocumento.Visible = True
                Else
                    pnlAggiungiAllegato.Visible = False
                    btnSalvaDocumento.Visible = False
                    Label2.Text = "Impossibile caricare il file. Autorizzazione negata"
                End If
            Else
                Dim a As String = ""


            End If
            Log.Info("*********** BeneficiariFromFile Page_Load - Fine")
        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Private Sub btnSalvaDocumento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaDocumento.Click

        Dim idDocumento As String = Context.Request.QueryString.Get("key")

        Log.Info("*********** beneficiariFromFile btnSalvaDocumento_Click idDocumento: " & idDocumento)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim _svrDocumenti As New DllDocumentale.svrDocumenti(oOperatore)
        
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (_svrDocumenti).Get_StatoIstanzaDocumento(Context.Request.QueryString.Item("key"))

        Dim ruolo As String = "" & statoIstanza.Ruolo
        If ruolo <> "A" Then


            HttpContext.Current.Session.Remove("erroreFile")
            Dim listaErrori As New Collections.Generic.List(Of String)

            If fileUpLoadAllegato.PostedFile.ContentLength <> 0 Then
                Dim nomeFileCompleto As String = fileUpLoadAllegato.PostedFile.FileName

                Dim arrayNomeFileCompleto() As String = nomeFileCompleto.Split(".")
                Dim nomeFile = arrayNomeFileCompleto(arrayNomeFileCompleto.Length-2)
                Dim estensione = arrayNomeFileCompleto(arrayNomeFileCompleto.Length-1)
                HttpContext.Current.Session.Add("erroreFile", "")
                If (fileUpLoadAllegato.PostedFile.ContentType = "application/vnd.ms-excel" Or _
                    fileUpLoadAllegato.PostedFile.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") And _
                    (UCase(estensione) = "XLS" Or UCase(estensione) = "XLSX") Then
                    Log.Info("File content-type" & fileUpLoadAllegato.PostedFile.ContentType)
                    
                    Dim xcApp As Excel.Application
                    
                    Dim xcWB As Excel.Workbook = Nothing
                    Dim xcWS As Excel.Worksheet = Nothing


                    Dim listaImpegniBeneficiariExcelInseriti As New Collections.Generic.List(Of ItemDocumentoBeneficiarioExcel)
                    Dim listaImpegniBeneficiariExcelToInsert As New Collections.Generic.List(Of ItemDocumentoBeneficiarioExcel)
                    Try
                        Dim maxGroupedBy As Integer = _svrDocumenti.getDocumentoBeneficiariExcelMaxGroupedBy(idDocumento)
                        Dim newGroupedBy As Integer = maxGroupedBy + 1
                        Dim pathstr As String = "" & AppDomain.CurrentDomain.BaseDirectory & "temp\" & Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) & "." & estensione
                        Dim arrByte As Byte() = Nothing
                        Try 
                            Dim fileStreamExcel As System.IO.Stream = fileUpLoadAllegato.PostedFile.InputStream
                            Dim bFile(fileStreamExcel.Length) As Byte
                            fileStreamExcel.Read(bFile, 0, CInt(fileStreamExcel.Length))
                            arrByte = DirectCast(bFile, Byte())
                           
                            Dim newfileStream As FileStream = File.Create(pathstr)
                           
                            newfileStream.Write(arrByte, 0, arrByte.Length - 1)
                            newfileStream.Dispose()

                            Log.Info("Apro Excel.ApplicationClass idDocumento: " & idDocumento)
                            xcApp = New Excel.ApplicationClass
                           
                            listaImpegniBeneficiariExcelToInsert = elaboraExcel(oOperatore, idDocumento, xcApp, pathstr, xcWB, xcWS, listaErrori, newGroupedBy)
                            File.Delete(pathstr)
                        
                        Catch ex As Exception
                            Log.Error("*********** idDocumento: " & idDocumento & " errore durante l'apertura di excel " + ex.Message)
                            If File.Exists(pathstr) Then
                                File.Delete(pathstr)
                            End If
                            listaErrori.Add(ex.Message)
                            Throw new Exception()
                        End Try

                        listaImpegniBeneficiariExcelInseriti = _svrDocumenti.FO_Insert_ListDocumentoBeneficiarioExcel(listaImpegniBeneficiariExcelToInsert)

                        Dim listaAnagraficheToValidate As New Collections.Generic.List(Of ItemAnagrafica)
                        For Each itemDocumentoBeneficiarioExcel As ItemDocumentoBeneficiarioExcel In listaImpegniBeneficiariExcelInseriti
                            itemDocumentoBeneficiarioExcel.Anagrafica.IdInterno = itemDocumentoBeneficiarioExcel.Prog
                            listaAnagraficheToValidate.Add(itemDocumentoBeneficiarioExcel.Anagrafica)
                        Next

                        'Controllo disponibilità per tutti i capitoli indicati, per l'anno e se c'è il preimpegno su cui si vuole impegnare la somma totale dei beneficiari
                        'TODO oltre a controllare la disponibilità del SIC, devo calcolare e controllare l'importo potenziale usabile (cioè il netto della disponibilità meno gli atti che sono in giro attualmente
                        Log.Info("Controllo disponibilità per tutti i capitoli indicati - INIZIO idDocumento: " & idDocumento)
                        Dim listaCapitoliToCheck As Collections.Generic.List(Of ItemDaImpegnare) = _svrDocumenti.FO_GetTotaleImportoByAnnoCapitolo(idDocumento, newGroupedBy)
                        For Each item As ItemDaImpegnare In listaCapitoliToCheck
                            If item.Anno.Equals(Now.Year) OrElse item.Anno.Equals(Now.Year + 1) OrElse item.Anno.Equals(Now.Year + 2) Then
                                Dim DettaglioCapitolo As Risposta_InterrogazioneBilancio_Types = ClientIntegrazioneSic.MessageMaker.createInterrogazioneBilancioInfoMessage(oOperatore, item.Anno, item.Capitolo, oOperatore.oUfficio.CodUfficio)

                                item.MissioneProgramma = DettaglioCapitolo.MissioneProgramma
                                ' TODO prima di verificare la disponibilità e qualunque altra cosa, per ciascun capitolo indicato nel file
                                ' verifico che non ci siano blocchi
                                If DettaglioCapitolo.CodiceRisposta > 0 Then
                                    listaErrori.Add("Il capitolo " + item.Capitolo + " è bloccato: " + DettaglioCapitolo.DescrizioneRisposta)
                                End If
                                If String.IsNullOrEmpty(item.Preimpegno) Then
                                    If item.Totale > DettaglioCapitolo.DispCompetenza Then
                                        listaErrori.Add("Capitolo: " + item.Capitolo + " Importo richiesto: " + CType(item.Totale, String) + " Disponibilità: " + CType(DettaglioCapitolo.DispCompetenza, String))
                                    End If
                                Else
                                    Dim DisponibilitaPreImpegno As Risposta_DisponibilitaPreImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaPreImpegnoMessage(oOperatore, item.Preimpegno)
                                    If item.Totale > DisponibilitaPreImpegno.Disponibilita Then
                                        listaErrori.Add("Importo Disponibile sul preimpegno " + item.Preimpegno + " insufficiente")
                                    End If
                                End If
                            Else
                                listaErrori.Add("Anno contabile non valido: " + item.Anno)
                            End If
                        Next
                        Log.Info("Controllo disponibilità per tutti i capitoli indicati - FINE idDocumento: " & idDocumento)
                        If listaErrori.Count = 0 Then
                            'Costruisco lista anagrafiche da inviare al SIC per la validazione/creazione
                            Dim listaAnagraficeTypeToValidate As Array = Array.CreateInstance(GetType(CaricamentoAnagrafiche_TypesElementoAnagraficaMassiva), listaAnagraficheToValidate.Count)
                            For Each itemAnagrafica As ItemAnagrafica In listaAnagraficheToValidate
                                Dim anagraficaTypeSIC As CaricamentoAnagrafiche_TypesElementoAnagraficaMassiva = ItemAnagrafica.TransformItemAnagraficaToAnagraficaTypeSIC(itemAnagrafica)
                                listaAnagraficeTypeToValidate.SetValue(anagraficaTypeSIC, listaAnagraficheToValidate.IndexOf(itemAnagrafica))
                            Next

                            'Invoco la validazione SIC: gestisco eventuali messaggi di errore sulle anagrafiche e eventuali successi.
                            Try
                                Dim messaggioRisposta As MessaggioRispostaMassiva_TypesRispostaMassiva_Type() = ClientIntegrazioneSic.MessageAnaMaker.createCaricamentoAnagraficheMassivoMessage(oOperatore, listaAnagraficeTypeToValidate)
                                For Each messaggioRispostaMassivaTypesRispostaMassivaType As MessaggioRispostaMassiva_TypesRispostaMassiva_Type In messaggioRisposta
                                    Dim itemAnagrafica As ItemDocumentoBeneficiarioExcel = listaImpegniBeneficiariExcelInseriti.Find(Function(anagrafica) anagrafica.Anagrafica.IdInterno.Equals(messaggioRispostaMassivaTypesRispostaMassivaType.IdAnagraficaMittente))
                                    'rimuovo dalla lista l'elemento appena trovo, perchè se va a buon fine la ricerca sul sic, 
                                    ' dovrò aggiornare questo elemento con i nuovi dati ottenuti dal sic e lo rimetto in lista.
                                    listaImpegniBeneficiariExcelInseriti.Remove(itemAnagrafica)
                                    If ((messaggioRispostaMassivaTypesRispostaMassivaType.Item.GetType)) Is GetType(ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Eccezione_Types) Then
                                        Dim excep As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Eccezione_Types = DirectCast(messaggioRispostaMassivaTypesRispostaMassivaType.Item, ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Eccezione_Types)

                                        Log.Debug("Risposta SIC:" & excep.Descrizione)
                                        listaErrori.Add("Riga file: " + itemAnagrafica.ExcelRiga.ToString() + " " + IIf(itemAnagrafica.Anagrafica.FlagPersonaFisica, " - C.F.: " + itemAnagrafica.Anagrafica.CodiceFiscale, " - P.IVA: " + itemAnagrafica.Anagrafica.PartitaIva) + " - " + itemAnagrafica.Anagrafica.Denominazione + "   #   " + excep.Descrizione)
                                   ElseIf ((messaggioRispostaMassivaTypesRispostaMassivaType.Item.GetType)) Is GetType(ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ProcessaAnagrafica_Types) Then
                                        Dim rispostaProcessaAnagrafica As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ProcessaAnagrafica_Types = DirectCast(messaggioRispostaMassivaTypesRispostaMassivaType.Item, ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ProcessaAnagrafica_Types)

                                        If rispostaProcessaAnagrafica.IdAnagrafica > 0 Then
                                            itemAnagrafica.Anagrafica.IdAnagraficaSic = rispostaProcessaAnagrafica.IdAnagrafica
                                            itemAnagrafica.Anagrafica.IdSedeSic = rispostaProcessaAnagrafica.IdSede
                                            itemAnagrafica.Anagrafica.IdTipologiaPagamentoSic = rispostaProcessaAnagrafica.IdTipoPagamento
                                            itemAnagrafica.Anagrafica.IdContoCorrenteSic = rispostaProcessaAnagrafica.IdContoCorrente
                                            itemAnagrafica.OperatoreCaricamento = rispostaProcessaAnagrafica.Operazione

                                            Dim itemDaImpegnare As ItemDaImpegnare = listaCapitoliToCheck.Find(Function(capitolo) capitolo.Capitolo.Equals(itemAnagrafica.Capitolo))
                                            itemAnagrafica.MissioneProgramma = itemDaImpegnare.MissioneProgramma
                                            _svrDocumenti.FO_Update_DocumentoBeneficiarioExcel(itemAnagrafica.Prog, itemAnagrafica.IdDocumento, rispostaProcessaAnagrafica.IdAnagrafica,
                                                                                               rispostaProcessaAnagrafica.IdSede, rispostaProcessaAnagrafica.IdTipoPagamento, rispostaProcessaAnagrafica.IdContoCorrente,
                                                                                               rispostaProcessaAnagrafica.Operazione, itemDaImpegnare.MissioneProgramma)
                                            listaImpegniBeneficiariExcelInseriti.Add(itemAnagrafica)
                                        Else 
                                            Dim msgErrore As String = "Riga file: " + itemAnagrafica.ExcelRiga.ToString() + " " + IIf(itemAnagrafica.Anagrafica.FlagPersonaFisica, " - C.F.: " + itemAnagrafica.Anagrafica.CodiceFiscale, " - P.IVA: " + itemAnagrafica.Anagrafica.PartitaIva) + " - " + itemAnagrafica.Anagrafica.Denominazione + " # " + "Errore dati nella risposta SIC: IdAnagrafica non valorizzato da SIC" 
                                            listaErrori.Add(msgErrore)
                                            Throw New Exception()
                                        End If
                                        
                                    End If
                                Next
                            Catch ex As Exception
                                listaErrori.Add(ex.Message)
                                Context.Session.Add("listaErrori", listaErrori)
                                Throw New Exception()
                            End Try
                        End If

                        If listaErrori.Count = 0 Then
                            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(idDocumento), DllDocumentale.Model.DocumentoInfo)

                            Log.Info("Per ciascun beneficiario indicato nel file idDocumento: " & idDocumento)
                            'Per ciascun beneficiario indicato nel file:
                            For Each beneficiarioExcel As ItemDocumentoBeneficiarioExcel In listaImpegniBeneficiariExcelInseriti
                                Dim excelAnagrafica As ItemAnagrafica = beneficiarioExcel.Anagrafica

                                Dim importo1 As Double = Nothing
                                Dim cog1 As String = Nothing
                                Dim pcf1 As String = Nothing
                                Dim importo2 As Double = Nothing
                                Dim cog2 As String = Nothing
                                Dim pcf2 As String = Nothing
                                Dim importo3 As Double = Nothing
                                Dim cog3 As String = Nothing
                                Dim pcf3 As String = Nothing

                                If beneficiarioExcel.Anno.Equals(Now.Year) Then
                                    importo1 = beneficiarioExcel.Importo
                                    cog1 = beneficiarioExcel.CodObGestionale
                                    pcf1 = beneficiarioExcel.Pcf
                                ElseIf beneficiarioExcel.Anno.Equals(Now.Year + 1) Then
                                    importo2 = beneficiarioExcel.Importo
                                    cog2 = beneficiarioExcel.CodObGestionale
                                    pcf2 = beneficiarioExcel.Pcf
                                ElseIf beneficiarioExcel.Anno.Equals(Now.Year + 2) Then
                                    importo3 = beneficiarioExcel.Importo
                                    cog3 = beneficiarioExcel.CodObGestionale
                                    pcf3 = beneficiarioExcel.Pcf
                                End If

                                
                                 

                                ' 1.  genero l'impegno sul SIC
                                Dim numPreImpegno As Integer = 0
                                Dim idDocContabileSIC As Integer = 0
                                Dim numPreDaPrenotazione As Integer = 0
                                
                                Dim hashTokenCallSicPreImpegno As String = GenerateHashTokenCallSic()


                                Dim hashTokenCallSicImpegno As String = GenerateHashTokenCallSic()


                                If Not String.IsNullOrEmpty(beneficiarioExcel.Preimpegno) Then
                                    Log.Info("idDocumento: " & idDocumento & " se è stato indicato il preimpegno su cui impegnare effettuo una prenotazione di impegno")
                                    'chiamata per generazione prenotazione se nel file viene indicato un preimpegno esistente su cui impegnare

                                    ' se nel file è indicato il preimpegno, allora devo creare solo la prenotazione sul SIC
                                    ' e qui, in UP, non sto generando alcun doc contabile, sfrutterò hashTokenCallSicImpegno per la futura registrazione dell'impegno in UR,
                                    ' salvando il dato nella colonna HashTokenCallSic_Imp
                                    Dim esito As String = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(oOperatore, "P", beneficiarioExcel.Preimpegno, beneficiarioExcel.Importo)
                                    If Int(esito) = 0 Then
                                        numPreDaPrenotazione = 1
                                        numPreImpegno = beneficiarioExcel.Preimpegno
                                    Else
                                        listaErrori.Add("Prenotazione sul preimpegno " + beneficiarioExcel.Preimpegno + " non effettuata.")
                                    End If
                                Else

                                    ' se nel file NON è indicato il preimpegno, 
                                    ' in UP sto generando il preimpegno: genero il token per la chiamata sic della registrazione preimpegno, salvando in HashTokenCallSic e 
                                    ' mi preparo il token per la futura registrazione dell'impegno in UR,
                                    ' salvando il dato nella colonna HashTokenCallSic_Imp
                                    Log.Info("idDocumento: " & idDocumento & " se NON è stato indicato il preimpegno lo genero")
 

                                    Dim numeroProvvisOrDefAtto As String = ""
                                    If objDocumento.Doc_numero = "" then
                                        numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                                    Else
                                        numeroProvvisOrDefAtto = objDocumento.Doc_numero
                                    End If
                                    Dim rispostaGenerazionePreImpegno As String() = ClientIntegrazioneSic.MessageMaker.createGenerazionePreimpegnoPerPluriennaleMessage(
                                    oOperatore, objDocumento.Doc_Oggetto, beneficiarioExcel.Capitolo, objDocumento.Doc_Tipo, objDocumento.Doc_Data,
                                    Right(objDocumento.Doc_numeroProvvisorio, 5), importo1, cog1, pcf1, importo2, cog2, pcf2, importo3, cog3, pcf3, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSicPreImpegno)

                                    'Numero doc contabile 
                                    If Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(0)) AndAlso rispostaGenerazionePreImpegno(0) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(0), numPreImpegno)
                                    ElseIf Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(1)) AndAlso rispostaGenerazionePreImpegno(1) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(1), numPreImpegno)
                                    ElseIf Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(2)) AndAlso rispostaGenerazionePreImpegno(2) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(2), numPreImpegno)
                                    End If

                                    'Id del doc contabile, interno al SIC
                                     If Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(3)) AndAlso rispostaGenerazionePreImpegno(3) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocContabileSIC)
                                    ElseIf Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(4)) AndAlso rispostaGenerazionePreImpegno(4) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocContabileSIC)
                                    ElseIf Not String.IsNullOrEmpty(rispostaGenerazionePreImpegno(5)) AndAlso rispostaGenerazionePreImpegno(5) <> 0 Then
                                        Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocContabileSIC)
                                    End If

                                    Log.Info("idDocumento: " & idDocumento & " numPreImpegno: " & numPreImpegno & " idDocContabileSIC: " & idDocContabileSIC )

                                End If



                                Dim itemImpegnoInfo As New DllDocumentale.ItemImpegnoInfo
                                If numPreImpegno > 0 Then
                                    With itemImpegnoInfo
                                        If numPreDaPrenotazione = 1 Then 
                                            Log.Info("idDocumento: " & idDocumento & " numPreDaPrenotazione: 1 - HashTokenCallSic_Imp: " & hashTokenCallSicImpegno )
                                            .HashTokenCallSic_Imp = hashTokenCallSicImpegno
                                            .IdDocContabileSic_Imp = idDocContabileSIC
                                        Else
                                            Log.Info("idDocumento: " & idDocumento & " numPreDaPrenotazione: 0 - HashTokenCallSic: " & hashTokenCallSicPreImpegno & " idDocContabileSIC " & idDocContabileSIC & " HashTokenCallSic: " & hashTokenCallSicImpegno) 
                                            .HashTokenCallSic = hashTokenCallSicPreImpegno
                                            .IdDocContabileSic = idDocContabileSIC
                                            .HashTokenCallSic_Imp = hashTokenCallSicImpegno
                                        End If
                                        .DBi_Anno = beneficiarioExcel.Anno.ToString
                                        .Dli_Cap = beneficiarioExcel.Capitolo
                                        .Dli_Costo = beneficiarioExcel.Importo
                                        .Dli_DataRegistrazione = Now
                                        .Dli_Documento = objDocumento.Doc_id
                                        .Dli_Esercizio = beneficiarioExcel.Anno.ToString
                                        .Dli_NumImpegno = "0"
                                        .Dli_Operatore = oOperatore.Codice
                                        .Dli_NPreImpegno = numPreImpegno
                                        .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                                        .Codice_Obbiettivo_Gestionale = beneficiarioExcel.CodObGestionale
                                        .Piano_Dei_Conti_Finanziari = beneficiarioExcel.Pcf
                                        .Dli_MissioneProgramma = beneficiarioExcel.MissioneProgramma
                                    End With
                                    ' 2.  regitro l'impegno sul ns db
                                    itemImpegnoInfo = ProcAmm.Registra_Bilancio(oOperatore, itemImpegnoInfo)

                                    ' 3.  registro il beneficiario sul ns db
                                    Dim datoBancario As Ext_DatiBancariInfo
                                    Dim anagrafica As Ext_AnagraficaInfo = Ext_AnagraficaInfo.TransformItemAnagraficaToExtAnagrafica(excelAnagrafica)
                                    Dim sede As Ext_SedeAnagraficaInfo = IIf((Not anagrafica.ListaSedi Is Nothing AndAlso anagrafica.ListaSedi.Count = 1), anagrafica.ListaSedi(0), Nothing)
                                    If Not sede Is Nothing Then
                                        datoBancario = IIf((Not sede.DatiBancari Is Nothing AndAlso sede.DatiBancari.Count = 1), sede.DatiBancari(0), Nothing)
                                    End If

                                    ProcAmm.Registra_BeneficiarioImpegnoLiquidazione(oOperatore, anagrafica, sede, datoBancario, beneficiarioExcel.Importo, excelAnagrafica.Cig, excelAnagrafica.Cup,
                                                                                     "", "", itemImpegnoInfo.Dli_prog, "")

                                    ' 4. in caso di generazione liq contestuale devo assicurarmi che l'anno contabile sia il corrente!
                                    If beneficiarioExcel.GeneraLiquidazioneContestuale AndAlso beneficiarioExcel.Anno.Equals(Now.Year) Then
                                        'chiamate per registrazione liq su db e inserimento beneficiario sulla liquidazione

                                        Dim itemLiquidazione As New DllDocumentale.ItemLiquidazioneInfo

                                        Dim hashTokenCallSicLiquidazione As String = GenerateHashTokenCallSic()

                                        With itemLiquidazione
                                            .HashTokenCallSic = hashTokenCallSicLiquidazione
                                            .Dli_Anno = beneficiarioExcel.Anno
                                            .Dli_Cap = beneficiarioExcel.Capitolo
                                            .Dli_Costo = beneficiarioExcel.Importo
                                            .Dli_NumImpegno = 0
                                            .Dli_NPreImpegno = itemImpegnoInfo.Dli_NPreImpegno
                                            .Dli_IdImpegno = itemImpegnoInfo.Dli_prog
                                            .Dli_DataRegistrazione = Now
                                            .Dli_Documento = idDocumento
                                            .Dli_Esercizio = beneficiarioExcel.Anno
                                            .Dli_Operatore = oOperatore.Codice

                                            ' di questi due codici viene usato ancora SOLO missione programma!
                                            .Dli_UPB = ""
                                            .Dli_MissioneProgramma = beneficiarioExcel.MissioneProgramma

                                            .Dli_TipoAssunzione = 0
                                            .Dli_Data_Assunzione = Now
                                            .Dli_Num_assunzione = 0
                                            .Dli_PianoDeiContiFinanziari = beneficiarioExcel.Pcf
                                        End With

                                        ' 5. registro i dati della liquidazione nel ns db
                                        itemLiquidazione = ProcAmm.Registra_Liquidazione(oOperatore, itemLiquidazione)


                                        ' 6. registro lo stesso beneficiario di prima, anche sulla liqudiazione
                                        If itemLiquidazione.Dli_prog <> 0 Then
                                            ProcAmm.Registra_BeneficiarioImpegnoLiquidazione(oOperatore, anagrafica, sede, datoBancario, beneficiarioExcel.Importo, excelAnagrafica.Cig, excelAnagrafica.Cup, "",
                                                                                             itemLiquidazione.Dli_prog, "", "")
                                        End If

                                    End If
                                End If

                            Next
                        End If


                        Context.Session.Add("listaErrori", listaErrori)
                        If listaErrori.Count > 0 Then
                            For Each itemDocumentoBeneficiarioExcel As ItemDocumentoBeneficiarioExcel In listaImpegniBeneficiariExcelInseriti
                                _svrDocumenti.FO_DeleteDocumentoBeneficiarioExcel(itemDocumentoBeneficiarioExcel.Prog)
                            Next
                        Else
                           
                            Registra_Allegato(arrByte, "Elenco_impegni_benef_" + newGroupedBy.ToString(), estensione, idDocumento, 27, newGroupedBy)
                        End If

                    Catch ex As Exception
                        listaErrori.Add(ex.Message)
                        Log.Error("Errore durante il caricamento massivo dei beneficiari: ")
                        If Not listaErrori Is Nothing Then
                            For Each errore As String In listaErrori
                                Log.Error("       - " & errore)
                            Next
                        End If

                        If Not xcWB Is Nothing Then
                            xcWB.Close()
                            releaseObject(xcWB)
                        End If
                        Log.Info("  -- xcWB chiuso ")

                        If Not xcApp Is Nothing Then
                            Try
                                xcApp.Quit()
                                releaseObject(xcApp)
                            Catch nex As Exception
                                Log.Error("  -- Errore durante xcApp.Quit() " + nex.Message)
                            End Try
                        End If
                        Log.Info("  -- xcApp.Quit() ")
                        If Not xcWS Is Nothing Then
                            releaseObject(xcWS)
                        End If
                        Log.Info("  -- xcWS released")

                        If Not ex.Message Is Nothing Then
                            If ex.Message.ToUpper().Contains("NESSUN") Then
                                listaErrori.Add("Possibili problemi nel contattare il SIC.")
                                Context.Session.Add("listaErrori", listaErrori)
                            ElseIf Not ex.Message.Equals("000") Then
                                listaErrori.Add("Errore durante la lettura del file.")
                                Context.Session.Add("listaErrori", listaErrori)
                            End If
                        End If


                        If Not  listaErrori Is Nothing AndAlso listaErrori.Count > 0 Then
                            For Each itemDocumentoBeneficiarioExcel As ItemDocumentoBeneficiarioExcel In listaImpegniBeneficiariExcelInseriti
                                _svrDocumenti.FO_DeleteDocumentoBeneficiarioExcel(itemDocumentoBeneficiarioExcel.Prog)
                            Next
                        End If

                    End Try
                End If

                If Trim(idDocumento) <> "" Then
                    Session.Add("key", idDocumento)
                End If
                If Trim(Context.Request.QueryString.Get("tipo")) <> "" Then
                    Context.Session.Add("tipo", Context.Request.QueryString.Item("tipo"))
                End If
                If listaErrori.Count > 0 Then
                    Context.Session.Add("_isUploadedFile", True)
                    Response.Redirect("BeneficiariFromFile.aspx?" + Context.Request.QueryString.ToString())
                Else
                    Context.Session.Add("_isUploadedFile", False)
                    Response.Redirect("Contabile.aspx?" + Context.Request.QueryString.ToString())
                End If

            Else
                'modgg 11-06 2
                If fileUpLoadAllegato.PostedFile.ContentType = "application/octet-stream" Then
                    HttpContext.Current.Session.Add("erroreFile", "Il file è già aperto da un altro programma, chiudere il file e riprovare")
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                Else
                    HttpContext.Current.Session.Add("erroreFile", "Il file da salvare deve essere in formato .xls o .xlsx")
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri)
                End If
            End If
        Else
            HttpContext.Current.Session.Add("erroreFile", "E' necessario selezionare un file Excel prima di procedere alla registrazione")
        Response.Redirect(Request.UrlReferrer.AbsoluteUri)

        End If


    End Sub

    Private Function elaboraExcel(oOperatore As Operatore, idDocumento As String, xcApp As Application, pathNomeFile As String, ByRef xcWB As Workbook, ByRef xcWS As Worksheet, listaErrori As List(Of String), newGroupedBy As Integer) As  List(Of ItemDocumentoBeneficiarioExcel)
        Dim listaImpegniBeneficiariExcelToInsert as new List(Of ItemDocumentoBeneficiarioExcel)

        Log.Info("Inizio elaboraExcel")

        xcWB = xcApp.Workbooks.Open(pathNomeFile)
        xcWS = xcWB.ActiveSheet
        Dim range As Excel.Range = xcWS.UsedRange

        Dim Obj As Object
        Dim rowIndex As Integer
        Dim countRow As Integer = range.Rows.Count
        For rowIndex = 2 To countRow




            Dim columnIndex As Integer = 1

            Dim itemDocumentoBeneficiarioExcel As New ItemDocumentoBeneficiarioExcel
            Dim itemAnagrafica As New ItemAnagrafica

            Dim excelContainsDatiLR As Boolean = False
            Try
                itemDocumentoBeneficiarioExcel.IdDocumento = idDocumento
                itemDocumentoBeneficiarioExcel.ExcelRiga = rowIndex

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.Anno = CType(Obj.value, Integer)
                If itemDocumentoBeneficiarioExcel.Anno = 0 Then
                    Exit For
                End If
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.Capitolo = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.Preimpegno = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.Importo = CType(Obj.value, Double)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.CodObGestionale = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemDocumentoBeneficiarioExcel.Pcf = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                Dim flagGeneraLiquidazioneContestuale As String = CType(Obj.value, String)
                itemDocumentoBeneficiarioExcel.GeneraLiquidazioneContestuale = IIf(flagGeneraLiquidazioneContestuale.Equals("S"), 1, 0)
                columnIndex = columnIndex + 1

                itemDocumentoBeneficiarioExcel.DataCaricamento = Now
                itemDocumentoBeneficiarioExcel.OperatoreCaricamento = oOperatore.Codice

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.Denominazione = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                Dim flagPersonaFisicaString As String = CType(Obj.value, String)
                itemAnagrafica.FlagPersonaFisica = IIf(flagPersonaFisicaString.Equals("F"), 1, 0)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.PartitaIva = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                If excelContainsDatiLR Then
                    'Dati Legale Rappresentante (LR)
                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.CognomeLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.NomeLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.SessoLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.ComuneNascitaLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.DataNascitaLR = CType(Obj.value, Date)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.IndirizzoResidenzaLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.ComuneResidenzaLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.CapResidenzaLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1

                    Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                    itemAnagrafica.CodiceFiscaleLR = CType(Obj.value, String)
                    columnIndex = columnIndex + 1
                    'Fine dati LR
                End If

                'Inizio Persona Fisica
                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.CodiceFiscale = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.Sesso = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                If Not Obj.value Is Nothing Then
                    itemAnagrafica.IsEstero = CType(IIf(Obj.value.Equals("S"), True, False), Boolean)
                End If
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.ComuneNascita = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.DataNascita = CType(Obj.value, Date)
                columnIndex = columnIndex + 1
                'Fine Persona Fisica

                'Inizio dati sede
                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.IndirizzoSede = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.ComuneSede = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.CapSede = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.NomeSede = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.TipologiaPagamentoNome = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                If Not Obj.value Is Nothing Then
                    itemAnagrafica.IsModalitaPrincipale = CType(IIf(Obj.value.Equals("S"), True, False), Boolean)
                End If
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.Iban = CType(Obj.value, String)
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                If Not Obj.value Is Nothing Then
                    itemAnagrafica.IsDatoSensibile = CType(IIf(Obj.value.Equals("S"), True, False), Boolean)
                End If
                columnIndex = columnIndex + 1

                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.Cup = CType(Obj.value, String)
                columnIndex = columnIndex + 1


                Obj = CType(range.Cells(rowIndex, columnIndex), Excel.Range)
                itemAnagrafica.Cig = CType(Obj.value, String)
                columnIndex = columnIndex + 1



            Catch ex As Exception
                listaErrori.Add("Errore durante la lettura dei dati")
                Context.Session.Add("listaErrori", listaErrori)
                Throw New Exception("000")
            End Try

            itemDocumentoBeneficiarioExcel.Anagrafica = itemAnagrafica
            itemDocumentoBeneficiarioExcel.GroupedBy = newGroupedBy

            listaImpegniBeneficiariExcelToInsert.Add(itemDocumentoBeneficiarioExcel)
        Next

        xcWB.Close()
        xcApp.Quit()

        xcApp = releaseObject(xcApp)
        xcWB = releaseObject(xcWB)
        xcWS = releaseObject(xcWS)

         Log.Info("Fine elaboraExcel")

        Return listaImpegniBeneficiariExcelToInsert
    End Function


    Private Function releaseObject(ByVal obj As Object) As Object
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try

        Return obj
    End Function

End Class

