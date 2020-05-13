Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Web.SessionState.HttpSessionState
Imports Newtonsoft.Json
Imports ClientIntegrazioneSic.Intema.WS.Risposta
Imports DllAmbiente
Imports ClientIntegrazioneSic
Imports ClientIntegrazioneSic.Intema.WS.Ana.Richiesta
Imports DllDocumentale




<ServiceContract([Namespace]:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class ProcAmm
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(ProcAmm))
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetImpDispCapitolo_post")>
    Public Function GetImpDispCapitolo_post(ByVal Capitolo As String, ByVal Bilancio As String, ByVal ufficioProponente As String) As ImportoDisponibile

        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneBilancio As Double = ClientIntegrazioneSic.MessageMaker.createInterrogazioneBilancioMessage(operatore, Bilancio, Capitolo, ufficioProponente)
            'Dim rispostaInterrogazioneBilancio As Double = ClientIntegrazioneSic.MessageMaker.createInterrogazioneBilancioMessage(operatore, "2014", Capitolo, ufficioProponente)

            Return New ImportoDisponibile(rispostaInterrogazioneBilancio)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetInterrogazioneBilancioContoEconomica_post")>
    Public Function GetInterrogazioneBilancioContoEconomica_post(ByVal Capitolo As String, ByVal Bilancio As String, ByVal Ufficio As String) As ArrayList

        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim arrContoEconomica As ArrayList = GetArrContoEconomica(operatore, Bilancio, Capitolo, Ufficio)

            Return arrContoEconomica
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New ArrayList
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function



    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetElencoCapitoliAnno?AnnoRif={AnnoRif}&tipoCapitolo={tipoCapitolo}&codiceUfficio={codiceUfficio}")>
    Public Function GetElencoCapitoliAnno(ByVal AnnoRif As String, ByVal tipoCapitolo As DllDocumentale.EnumDocumenti.TipoCapitolo, ByVal codiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Try            
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim capitoli As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
            '-1
            Dim rispostaInterrogazioneCapitoli As Array

            If String.IsNullOrEmpty(codiceUfficio) Then
                rispostaInterrogazioneCapitoli = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCapitoliMessage(operatore, AnnoRif)
                'rispostaInterrogazioneCapitoli = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCapitoliMessage(operatore, "2014")

            Else
                rispostaInterrogazioneCapitoli = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCapitoliMessage(operatore, AnnoRif, codiceUfficio)
                'rispostaInterrogazioneCapitoli = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCapitoliMessage(operatore, "2014", codiceUfficio)
            End If
            For i As Integer = 0 To UBound(rispostaInterrogazioneCapitoli, 1)
                Dim capitoloRestituito As New Ext_CapitoliAnnoInfo
                capitoloRestituito.AnnoPrenotazione = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).AnnoBilancio
                capitoloRestituito.Bilancio = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).AnnoBilancio
                capitoloRestituito.Capitolo = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).CodiceCapitolo
                capitoloRestituito.UPB = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).UPB
                capitoloRestituito.MissioneProgramma = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).MissioneProgramma
                capitoloRestituito.ImpDisp = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DisponibilitaCompetenza
                capitoloRestituito.DescrCapitolo = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DescrizioneCapitolo
                capitoloRestituito.ImpPrenotato = 0
                capitoloRestituito.NumPreImp = 0
                If (DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).Perenti) = "N" Then
                    capitoloRestituito.isPerente = False
                ElseIf (DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).Perenti) = "S" Then
                    capitoloRestituito.isPerente = True
                End If
                capitoloRestituito.CodiceRisposta = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).CodiceRisposta
                capitoloRestituito.DescrizioneRisposta = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DescrizioneRisposta

                ' Non esite più il concetto di capitolo perente 
                'If (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.Perente AndAlso capitoloRestituito.isPerente = True) Or (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.Tutti) Or (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.NonPerente AndAlso capitoloRestituito.isPerente = False) Then
                capitoli.Add(capitoloRestituito)
                'End If
                'capitoli.Add(capitoloRestituito)
            Next
            Return capitoli
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetImpDispPreImpegno")>
    Public Function GetImpDispPreImpegno(ByVal NumeroPreImpegno As String, Optional ByVal codUfficioProponente As String = "") As Ext_CapitoliAnnoInfo
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            If String.IsNullOrEmpty(codUfficioProponente) Then
                codUfficioProponente = operatore.oUfficio.CodUfficioPubblico
            End If

            Dim rispostaDisponibilitaPreImpegno As Risposta_DisponibilitaPreImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaPreImpegnoMessage(operatore, NumeroPreImpegno, codUfficioProponente)

            'Dim rispostaDisponibilitaPreImpegno As String = "111"
            'Dim str_DataAtto As String="DataAtto:'"  rispostaDisponibilitaPreImpegno.DataAtto & '
            Dim capitoloRestituito As New Ext_CapitoliAnnoInfo

            capitoloRestituito.Bilancio = Left(NumeroPreImpegno, 4)
            capitoloRestituito.Capitolo = rispostaDisponibilitaPreImpegno.CodiceCapitolo
            capitoloRestituito.UPB = rispostaDisponibilitaPreImpegno.UPB
            capitoloRestituito.MissioneProgramma = rispostaDisponibilitaPreImpegno.MissioneProgramma
            capitoloRestituito.ImpDisp = rispostaDisponibilitaPreImpegno.Disponibilita
            capitoloRestituito.TipoAtto = rispostaDisponibilitaPreImpegno.TipoAtto
            capitoloRestituito.NumeroAtto = rispostaDisponibilitaPreImpegno.NumeroAtto
            capitoloRestituito.DataAtto = rispostaDisponibilitaPreImpegno.DataAtto

            capitoloRestituito.DescrCapitolo = ""
            capitoloRestituito.ImpPrenotato = 0
            capitoloRestituito.NumPreImp = NumeroPreImpegno

            capitoloRestituito.Oggetto_Impegno = rispostaDisponibilitaPreImpegno.OggettoPreimpegno
            capitoloRestituito.ImpPotenzialePrenotato = residuaDisponibilitaPreimpegno(operatore, NumeroPreImpegno, capitoloRestituito.ImpDisp)

            Return capitoloRestituito
            'HttpContext.Current.Response.Write("{  success: true, DispImpPreImp: '" + CStr(rispostaDisponibilitaPreImpegno) + "' }")
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazionePreImpegno")>
    Public Function GenerazionePreImpegno(ByVal ListaFatture As System.Collections.Generic.List(Of Ext_FatturaInfo),
                                                     ByVal Capitolo As String,
                                                     ByVal ComboPreimp As String,
                                                    ByVal UPB As String,
                                                    ByVal MissioneProgramma As String,
                                                    ByVal Bilancio As String,
                                                    ByVal ImpPrenotato As String,
                                                    ByVal ComboCOGImp As String,
                                                    ByVal PcfPreImp As String,
                                                    ByVal ImpDisp As String,
                                                    ByVal Importo1 As String,
                                                    ByVal ObGestionale1 As String,
                                                    ByVal PCF1 As String,
                                                    ByVal Importo2 As String,
                                                    ByVal ObGestionale2 As String,
                                                    ByVal PCF2 As String,
                                                    ByVal Importo3 As String,
                                                    ByVal ObGestionale3 As String,
                                                    ByVal PCF3 As String)





        'Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
        'Dim ComboPreimp As String = HttpContext.Current.Request.Item("ComboPreimp")
        'Dim UPB As String = HttpContext.Current.Request.Item("UPB")
        'Dim MissioneProgramma As String = HttpContext.Current.Request.Item("MissioneProgramma")
        'Dim Bilancio As String = HttpContext.Current.Request.Item("Bilancio")
        'Dim ImpPrenotato As String = HttpContext.Current.Request.Item("ImpPrenotato")
        'Dim ComboCOGImp As String = "" & HttpContext.Current.Request.Item("ComboCOGImp")
        'Dim PcfPreImp As String = "" & HttpContext.Current.Request.Item("PcfPreImp")
        'Dim ImpDisp As String = HttpContext.Current.Request.Item("ImpDisp")
        'Dim ObGestionale1 As String = HttpContext.Current.Request.Item("ObGestionale1")
        'Dim ObGestionale2 As String = HttpContext.Current.Request.Item("ObGestionale2")
        'Dim ObGestionale3 As String = HttpContext.Current.Request.Item("ObGestionale3")
        'Dim PCF1 As String = HttpContext.Current.Request.Item("PCF1")
        'Dim PCF2 As String = HttpContext.Current.Request.Item("PCF2")
        'Dim PCF3 As String = HttpContext.Current.Request.Item("PCF3")
        'Dim Importo1 As String = HttpContext.Current.Request.Item("Importo1")
        'Dim Importo2 As String = HttpContext.Current.Request.Item("Importo2")
        'Dim Importo3 As String = HttpContext.Current.Request.Item("Importo3")



        Try
            'Capitolo:   capitolo,
            'ComboPreimp:  0,
            '		UPB: upb,
            '        MissioneProgramma: missioneProgramma,
            '        Bilancio: bilancio,
            '        listaFatture: listaFatturaToStartImpegno


            'ImpPrenotato: importo,
            '            ComboCOGImp: obgestionale,
            '            pcfPreImp: pcfPreImp,
            '            ImpDisp: disponibilita,


            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)



            'documento di lavoro
            Dim ddlDocumentale As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = ddlDocumentale.Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)


            'Prima di registrare la liquidazione, se al documento sono associate delle fatture e sto generando la liq
            ' a partire da queste, devo assicurarmi che le fatture non siano già state associate ad altre liquidazioni attive, 
            ' su altri atti in itinere
            'Dim fattureExtList As List(Of Ext_FatturaInfo) = GetListaFattureAtto(CodDocumento())
            'Dim listaFatturaVecchiImp As New List(Of DllDocumentale.ItemFatturaInfoHeader)
            'Dim listaFattureAsString As String = ""
            'For Each fatturaDaAssociare As Ext_FatturaInfo In fattureExtList
            '    Dim fatturaVecchieLiq As Collections.Generic.List(Of ItemFatturaInfoHeader) = ddlDocumentale.FO_Get_ListaFattureImpegniAttiveByIdFattura(fatturaDaAssociare.IdUnivoco)
            '    If Not fatturaVecchieLiq Is Nothing AndAlso fatturaVecchieLiq.Count > 0 Then
            '        listaFatturaVecchiImp.Add(fatturaVecchieLiq(0))
            '        listaFattureAsString = listaFattureAsString & " - " & fatturaVecchieLiq(0).NumeroFatturaBeneficiario
            '    End If
            'Next
            'If listaFatturaVecchiImp.Count <> 0 Then
            '    Throw New Exception("Non è possibile creare liquidazioni per queste fatture, in quanto già presenti in altre liquidazioni attive: " & listaFattureAsString)
            'End If

            'valori della richiesta
            'Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            Dim importo As String = HttpContext.Current.Request.Item("ImpPrenotato")

            Dim NumPreImpegno As Long = 0
            Long.TryParse(ComboPreimp, NumPreImpegno)

            'HttpContext.Current.Request.Item("ComboCOGImp")

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            ElseIf UCase(tipoAtto) = "DELIBERE" Then
                tipoAtto = "DELIBERA"
            Else
                tipoAtto = Nothing
            End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            Dim importoDisponibilePreimpegno As Double
            Dim importoPrenotato As Double = Nothing
            If NumPreImpegno <> 0 Then

                ImpDisp = Trim(("" + ImpDisp).Replace("€", "").Replace(".", ""))
                Double.TryParse(ImpDisp, importoDisponibilePreimpegno)
                ImpPrenotato = ("" + ImpPrenotato).Replace(".", ",")
                Double.TryParse(ImpPrenotato, importoPrenotato)


                If importoPrenotato <= 0 Then
                    Throw New Exception("Inserire un importo valido")
                End If
                If importoPrenotato > importoDisponibilePreimpegno Then
                    'log.info 
                    Throw New Exception("Importo Disponibile insufficiente")
                End If
            End If

            Dim numPreDaPrenotazione As Integer = 0
            Dim cog As String = "" & HttpContext.Current.Request.Item("ComboCOGImp")
            If cog.ToLower.Contains("selez") Then
                cog = ""
            End If

            If String.IsNullOrEmpty(PcfPreImp) Then
                PcfPreImp = "" & HttpContext.Current.Request.Item("PcfPreImp")
                If PcfPreImp.ToLower.Contains("selez") Then
                    PcfPreImp = ""
                End If
            End If
            Dim result As String = ""

            Dim impegno1 As New DllDocumentale.ItemImpegnoInfo
            Dim impegno2 As New DllDocumentale.ItemImpegnoInfo
            Dim impegno3 As New DllDocumentale.ItemImpegnoInfo

            'Controllo se sto generano gli impegni pluriennali,
            ' oppure un impegno da dal preimpegno
            If NumPreImpegno = 0 Then
                'devo generare un nuovo preimpegno da determina
                'modifica per inserire numero provvisorio 

                Dim Importo1Doub As Double, Importo2Doub As Double, Importo3Doub As Double
                'Dim Importo1 As Double, Importo2 As Double, Importo3 As Double
                'Dim ObGestionale1 As String, ObGestionale2 As String, ObGestionale3 As String
                'Dim PCF1 As String, PCF2 As String, PCF3 As String

                'If (Double.TryParse(HttpContext.Current.Request.Item("Importo1").Replace(".", ","), Importo1) AndAlso Importo1 > 0) Then
                If (Double.TryParse(("" + Importo1).Replace(".", ","), Importo1Doub) AndAlso Importo1Doub > 0) Then
                    'ObGestionale1 = HttpContext.Current.Request.Item("ObGestionale1")
                    If (ObGestionale1 = "") Then
                        Throw New Exception("Obiettivo gestionale 1 non valido")
                    End If

                    'PCF1 = HttpContext.Current.Request.Item("PCF1")
                    If (PCF1 = "") Then
                        Throw New Exception("Piano dei conti finanziario 1 non valido")
                    End If
                End If

                If (Double.TryParse(("" + Importo2).Replace(".", ","), Importo2Doub) AndAlso Importo2Doub > 0) Then
                    'If (Double.TryParse(HttpContext.Current.Request.Item("Importo2").Replace(".", ","), Importo2) AndAlso Importo2 > 0) Then

                    'ObGestionale2 = HttpContext.Current.Request.Item("ObGestionale2")
                    If (ObGestionale2 = "") Then
                        Throw New Exception("Obiettivo gestionale 2 non valido")
                    End If

                    'PCF2 = HttpContext.Current.Request.Item("PCF2")
                    If (PCF2 = "") Then
                        Throw New Exception("Piano dei conti finanziario 2 non valido")
                    End If
                End If

                If (Double.TryParse(("" + Importo3).Replace(".", ","), Importo3Doub) AndAlso Importo3Doub > 0) Then
                    'If (Double.TryParse(HttpContext.Current.Request.Item("Importo3").Replace(".", ","), Importo3) AndAlso Importo3 > 0) Then

                    'ObGestionale3 = HttpContext.Current.Request.Item("ObGestionale3")
                    If (ObGestionale3 = "") Then
                        Throw New Exception("Obiettivo gestionale 3 non valido")
                    End If

                    'PCF3 = HttpContext.Current.Request.Item("PCF3")
                    If (PCF3 = "") Then
                        Throw New Exception("Piano dei conti finanziario 3 non valido")
                    End If
                End If
                Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
                Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

                
                'Dim rispostaGenerazionePreImpegno As String() = _
                'ClientIntegrazioneSic.MessageMaker.createGenerazionePreimpegnoPerPluriennaleMessage( _
                'operatore, objDocumento.Doc_Oggetto, capitolo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numeroProvvisorio, 5), _
                'Importo1, ObGestionale1, PCF1, Importo2, ObGestionale2, PCF2, Importo3, ObGestionale3, PCF3)
                Dim hashTokenCallSic_Preimp As String = GenerateHashTokenCallSic()
                Dim numeroProvvisOrDefAtto As String = ""
                If objDocumento.Doc_numero = "" then
                    numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                Else
                    numeroProvvisOrDefAtto = objDocumento.Doc_numero
                End If
                Dim rispostaGenerazionePreImpegno As String() =
                     ClientIntegrazioneSic.MessageMaker.createGenerazionePreimpegnoPerPluriennaleMessage(
                        operatore, objDocumento.Doc_Oggetto, Capitolo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numeroProvvisorio, 5),
                        Importo1Doub, ObGestionale1, PCF1, Importo2Doub, ObGestionale2, PCF2, Importo3Doub, ObGestionale3, PCF3, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic_Preimp)

                Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)

                result = "{  success: true, Preimpegni: ['" & NumPreImpegno1 & "','" & NumPreImpegno2 & "','" & NumPreImpegno3 & "'] }"

                Dim bilancioCorrente As Integer
                Integer.TryParse(Bilancio, bilancioCorrente)
                'Integer.TryParse(HttpContext.Current.Request.Item("Bilancio"), bilancioCorrente)

                'inserisco i dati nei nostri archivi
                Dim itemBilancio1 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno1 > 0 And idDocSIC1 > 0 Then
                    With itemBilancio1
                        .HashTokenCallSic = hashTokenCallSic_Preimp
                        .IdDocContabileSic = idDocSIC1
                        'sono in UP: preparo già il tocken per la chiamata il SIC fatta dalla ragioneria per 
                        ' la registrazione di questi impegno
                        .HashTokenCallSic_Imp = GenerateHashTokenCallSic()
                        .DBi_Anno = bilancioCorrente.ToString
                        .Dli_Cap = Capitolo
                        .Dli_Costo = Importo1Doub
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = bilancioCorrente.ToString
                        .Dli_NumImpegno = "0"
                        .Dli_Operatore = operatore.Codice
                        .Dli_UPB = UPB
                        '.Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_NPreImpegno = NumPreImpegno1
                        .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                        .Codice_Obbiettivo_Gestionale = ObGestionale1
                        .Piano_Dei_Conti_Finanziari = PCF1
                        .Dli_MissioneProgramma = MissioneProgramma
                        '.Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                    End With


                    impegno1 = Registra_Bilancio(operatore, itemBilancio1)
                End If


                Dim itemBilancio2 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno2 > 0 And idDocSIC2 > 0Then
                    With itemBilancio2
                        .HashTokenCallSic = hashTokenCallSic_Preimp
                        .IdDocContabileSic = idDocSIC2
                        'sono in UP: preparo già il tocken per la chiamata il SIC fatta dalla ragioneria per 
                        ' la registrazione di questi impegno
                        .HashTokenCallSic_Imp = GenerateHashTokenCallSic()
                        .DBi_Anno = (bilancioCorrente + 1).ToString
                        .Dli_Cap = Capitolo
                        .Dli_Costo = Importo2Doub
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = (bilancioCorrente + 1).ToString
                        .Dli_NumImpegno = "0"
                        .Dli_Operatore = operatore.Codice
                        '.Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_UPB = UPB
                        .Dli_MissioneProgramma = MissioneProgramma
                        '.Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                        .Dli_NPreImpegno = NumPreImpegno2
                        .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                        .Codice_Obbiettivo_Gestionale = ObGestionale2
                        .Piano_Dei_Conti_Finanziari = PCF2
                    End With

                    impegno2 = Registra_Bilancio(operatore, itemBilancio2)
                End If

                Dim itemBilancio3 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno3 > 0 And idDocSIC3 > 0 Then
                    With itemBilancio3
                        .HashTokenCallSic = hashTokenCallSic_Preimp
                        .IdDocContabileSic = idDocSIC3
                        'sono in UP: preparo già il tocken per la chiamata il SIC fatta dalla ragioneria per 
                        ' la registrazione di questi impegno
                        .HashTokenCallSic_Imp = GenerateHashTokenCallSic()
                        .DBi_Anno = (bilancioCorrente + 2).ToString
                        .Dli_Cap = Capitolo
                        .Dli_Costo = Importo3Doub
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = (bilancioCorrente + 2).ToString
                        .Dli_NumImpegno = "0"
                        .Dli_Operatore = operatore.Codice
                        '.Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_UPB = UPB
                        .Dli_MissioneProgramma = MissioneProgramma
                        '.Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                        .Dli_NPreImpegno = NumPreImpegno3
                        .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                        .Codice_Obbiettivo_Gestionale = ObGestionale3
                        .Piano_Dei_Conti_Finanziari = PCF3
                    End With

                    impegno3 = Registra_Bilancio(operatore, itemBilancio3)
                End If
            Else
                '*** 
                '*** ATTENZIONE: il conrollo che segue, sull'importo residuo è stato eliminato,
                '***  perchè il controllo IMPORTO DISPON (SIC) - RIDUZIONI IN ITINIRE viene già fatto
                '***  alla selezione del capitolo, quando si calcola l'importo potenziale del preimpegno selezionato
                '*** 
                'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
                'Dim residuo As Decimal = residuaDisponibilitaPreimpegno(operatore, NumPreImpegno, importoDisponibilePreimpegno)
                'Dim totResiduo As Decimal = residuo - CDbl(importo)
                'If (totResiduo) < 0 Then
                '    Throw New Exception("Disponibilita insufficiente per la generazione di impegno di € " & CStr(importo) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
                'End If

                'non deve essere generato un nuovo preimpegno ma è necessario accontonare su un preimpegno esistente.
                numPreDaPrenotazione = 1
                'Dim rispostaPrenotazionePreImpegno As String = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(operatore, "P", NumPreImpegno, importo)
                Dim rispostaPrenotazionePreImpegno As String = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(operatore, "P", NumPreImpegno, ImpPrenotato)

                result = "{  success: true, NumPreImp: '" + CStr(NumPreImpegno) + "' }"

                'Dal momento che si tratta di un impegno, a partire da un preimpegno già preso in precedenza,
                ' nel db dovrò preparare solo il tocken per la conferma in reagioneria di questo.
                Dim hashTokenCallSic_Imp As String = GenerateHashTokenCallSic()
                'inserisco i dati nei nostri archivi
                Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo
                With itemBilancio
                    .HashTokenCallSic_Imp = hashTokenCallSic_Imp
                    .DBi_Anno = Bilancio
                    '.DBi_Anno = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_Cap = Capitolo
                    .Dli_Costo = importoPrenotato
                    .Dli_DataRegistrazione = Now
                    .Dli_Documento = objDocumento.Doc_id
                    .Dli_Esercizio = Bilancio
                    '.Dli_Esercizio = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_NumImpegno = "0"
                    .Dli_Operatore = operatore.Codice
                    .Dli_UPB = UPB
                    .Dli_MissioneProgramma = MissioneProgramma
                    '.Dli_UPB = HttpContext.Current.Request.Item("UPB")
                    '.Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                    .Dli_NPreImpegno = NumPreImpegno
                    .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                    If Not String.IsNullOrEmpty(ComboCOGImp) Then
                        .Codice_Obbiettivo_Gestionale = ComboCOGImp
                    Else
                        .Codice_Obbiettivo_Gestionale = cog
                    End If

                    .Dli_PianoDeiContiFinanziari = PcfPreImp
                    .Piano_Dei_Conti_Finanziari = PcfPreImp
                    ' lu 08/11/2010  .Codice_Obbiettivo_Gestionale = "" & HttpContext.Current.Request.Item("CodObGestionale")
                End With

                impegno1 = Registra_Bilancio(operatore, itemBilancio)
            End If

            ' se l'inserimento dell'impegno (che sia da preimpegno o sul bilancio anno corrente) 
            ' è andata a buon fine aggiungo tutte le fatture dell'atto all'impegno appena creato
            'If Not fattureExtList Is Nothing AndAlso fattureExtList.Count > 0 Then
            '    If Not impegno1 Is Nothing AndAlso impegno1.Dli_prog <> 0 Then
            '        NotificaAttoFattura(fattureExtList, "I", impegno1.Dli_NPreImpegno, )
            '    End If
            'End If
            Dim unicoBeneficiario As New Ext_AnagraficaInfo
            Dim sedeUnicoBeneficiario As New Ext_SedeAnagraficaInfo
            Dim contoUnicoBeneficiario As New Ext_DatiBancariInfo
            Dim cigFattura As String = ""
            Dim cupFattura As String = ""


            For Each fattura As Ext_FatturaInfo In ListaFatture
                Dim anagraficaFattura As Ext_AnagraficaInfo = fattura.AnagraficaInfo
                Dim sede As New Ext_SedeAnagraficaInfo
                If Not anagraficaFattura.ListaSedi Is Nothing AndAlso anagraficaFattura.ListaSedi.Count > 0 Then
                    sede = anagraficaFattura.ListaSedi.ElementAt(0)
                End If

                Dim listaDaConfront As New System.Collections.Generic.List(Of Ext_FatturaInfo)
                listaDaConfront.AddRange(ListaFatture)
                listaDaConfront.Remove(fattura)

                If listaDaConfront.Count > 1 Then
                    For Each fattura2 As Ext_FatturaInfo In listaDaConfront
                        Dim anagraficaFattura2 As Ext_AnagraficaInfo = fattura2.AnagraficaInfo
                        If anagraficaFattura.ID <> anagraficaFattura2.ID Then
                            Throw New Exception("Non è possibile generare una liquidazione contestuale per 2 fatture con beneficiari differenti")
                        Else
                            Dim sede2 As New Ext_SedeAnagraficaInfo
                            If Not anagraficaFattura2.ListaSedi Is Nothing AndAlso anagraficaFattura2.ListaSedi.Count > 0 Then
                                sede2 = anagraficaFattura2.ListaSedi.ElementAt(0)
                                If sede.IdSede <> sede2.IdSede AndAlso sede.IdModalitaPagamento <> sede2.IdModalitaPagamento Then
                                    Throw New Exception("Non è possibile generare una liquidazione contestuale per 2 fatture con beneficiari differenti")
                                Else
                                    unicoBeneficiario = fattura.AnagraficaInfo
                                    If unicoBeneficiario.Contratto Is Nothing And Not fattura.Contratto Is Nothing Then
                                        unicoBeneficiario.Contratto = fattura.Contratto
                                    End If
                                    unicoBeneficiario.Fattura = fattura
                                    cigFattura = fattura.Contratto.CodiceCIG
                                    cupFattura = fattura.Contratto.CodiceCUP
                                    sedeUnicoBeneficiario = unicoBeneficiario.ListaSedi.ElementAt(0)
                                    contoUnicoBeneficiario = sedeUnicoBeneficiario.DatiBancari.ElementAt(0)
                                End If
                            End If
                        End If
                    Next
                Else
                    unicoBeneficiario = fattura.AnagraficaInfo
                    If unicoBeneficiario.Contratto Is Nothing And Not fattura.Contratto Is Nothing Then
                        unicoBeneficiario.Contratto = fattura.Contratto
                    End If
                    unicoBeneficiario.Fattura = fattura
                    cigFattura = fattura.Contratto.CodiceCIG
                    cupFattura = fattura.Contratto.CodiceCUP
                    sedeUnicoBeneficiario = unicoBeneficiario.ListaSedi.ElementAt(0)
                    contoUnicoBeneficiario = sedeUnicoBeneficiario.DatiBancari.ElementAt(0)
                End If
            Next

            ' se si sta generando un impegno, e insieme arriva la lista delle fatture, allora la liquidazione contestuale viene generata automaticamente 
            ' aggangiando lo stesso beneficiario sia all'impegno che alla liquidazione ed in più, a quest'ultima, la fattura.
            If Not ListaFatture Is Nothing AndAlso ListaFatture.Count > 0 Then
                If Not unicoBeneficiario Is Nothing Then
                    result = Registra_BeneficiarioImpegnoLiquidazione(operatore, unicoBeneficiario, sedeUnicoBeneficiario, contoUnicoBeneficiario, impegno1.Dli_Costo, cigFattura, cupFattura, "", "", impegno1.Dli_prog, 0)
                End If

                Dim hashTokenCallSic As String = GenerateHashTokenCallSic()

                Dim itemLiquidazione As New DllDocumentale.ItemLiquidazioneInfo
                With itemLiquidazione
                    Long.TryParse(0, .Dli_prog)
                    .HashTokenCallSic = hashTokenCallSic
                    .Dli_Anno = Bilancio
                    .Dli_Cap = Capitolo
                    .Dli_Costo = impegno1.Dli_Costo
                    .Dli_DataRegistrazione = Now
                    .Dli_Documento = CodDocumento()
                    .Dli_Esercizio = Bilancio
                    .Dli_NumImpegno = 0
                    .Dli_Operatore = operatore.Codice
                    .Dli_UPB = UPB
                    .Dli_MissioneProgramma = MissioneProgramma
                    'determina corrente, quindi non c'è ancora il num definitivo
                    .Dli_TipoAssunzione = 0
                    .Dli_Data_Assunzione = Now
                    .Dli_Num_assunzione = 0
                    .Dli_NPreImpegno = impegno1.Dli_NPreImpegno
                    If Not impegno1.Dli_PianoDeiContiFinanziari Is Nothing AndAlso impegno1.Dli_PianoDeiContiFinanziari <> "" Then
                        .Dli_PianoDeiContiFinanziari = impegno1.Dli_PianoDeiContiFinanziari
                    ElseIf Not impegno1.Piano_Dei_Conti_Finanziari Is Nothing AndAlso impegno1.Piano_Dei_Conti_Finanziari <> "" Then
                        .Dli_PianoDeiContiFinanziari = impegno1.Piano_Dei_Conti_Finanziari
                    End If


                    If Not String.IsNullOrEmpty(impegno1.Dli_prog) Then
                        .Dli_IdImpegno = impegno1.Dli_prog
                    End If
                End With

                If itemLiquidazione.Dli_prog = 0 Then
                    itemLiquidazione = Registra_Liquidazione(operatore, itemLiquidazione)
                End If

                If itemLiquidazione.Dli_prog <> 0 Then
                    Dim listaNotificaFatturaOutput As New Generic.List(Of NotificaFatturaOutput)
                    If Not ListaFatture Is Nothing AndAlso ListaFatture.Count > 0 Then
                        listaNotificaFatturaOutput = NotificaAttoFattura1(ListaFatture, "I", , itemLiquidazione.Dli_prog)
                        result = Registra_BeneficiarioImpegnoLiquidazione(operatore, unicoBeneficiario, sedeUnicoBeneficiario, contoUnicoBeneficiario, itemLiquidazione.Dli_Costo, cigFattura, cupFattura, "", itemLiquidazione.Dli_prog, "", 0)
                    End If

                End If

            End If

            'For Each fattura As Ext_FatturaInfo In ListaFatture
            '    Dim anagraficaFattura As Ext_AnagraficaInfo = fattura.AnagraficaInfo
            '    If anagraficaFattura.Contratto Is Nothing And Not fattura.Contratto Is Nothing Then
            '        anagraficaFattura.Contratto = fattura.Contratto
            '    End If
            '    anagraficaFattura.Fattura = fattura
            '    Dim sedeFattura As Ext_SedeAnagraficaInfo = anagraficaFattura.ListaSedi.ElementAt(0)
            '    Dim contoFattura As Ext_DatiBancariInfo = sedeFattura.DatiBancari.ElementAt(0)

            '    'result = Registra_BeneficiarioImpegnoLiquidazione(operatore, anagraficaFattura, sedeFattura, contoFattura, impegno1.Dli_Costo, fattura.Contratto.CodiceCIG, fattura.Contratto.CodiceCUP, "", "", impegno1.Dli_prog, 0)

            '    Dim itemLiquidazione As New DllDocumentale.ItemLiquidazioneInfo
            '    With itemLiquidazione
            '        Long.TryParse(0, .Dli_prog)
            '        .Dli_Anno = Bilancio
            '        .Dli_Cap = Capitolo
            '        .Dli_Costo = impegno1.Dli_Costo
            '        .Dli_DataRegistrazione = Now
            '        .Dli_Documento = CodDocumento()
            '        .Dli_Esercizio = Bilancio
            '        .Dli_NumImpegno = 0
            '        .Dli_Operatore = operatore.Codice
            '        .Dli_UPB = UPB
            '        .Dli_MissioneProgramma = MissioneProgramma
            '        'determina corrente, quindi non c'è ancora il num definitivo
            '        .Dli_TipoAssunzione = 0
            '        .Dli_Data_Assunzione = Now
            '        .Dli_Num_assunzione = 0
            '        .Dli_NPreImpegno = impegno1.Dli_NPreImpegno
            '        .Dli_PianoDeiContiFinanziari = impegno1.Dli_PianoDeiContiFinanziari
            '        If Not String.IsNullOrEmpty(impegno1.Dli_prog) Then
            '            .Dli_IdImpegno = impegno1.Dli_prog
            '        End If
            '    End With

            '    If itemLiquidazione.Dli_prog = 0 Then
            '        itemLiquidazione = Registra_Liquidazione(operatore, itemLiquidazione)
            '    End If

            '    If itemLiquidazione.Dli_prog <> 0 Then
            '        Dim listaNotificaFatturaOutput As New Generic.List(Of NotificaFatturaOutput)
            '        Dim listaFattureLiquidazioneEXT As New List(Of Ext_FatturaInfo)
            '        listaFattureLiquidazioneEXT.Add(fattura)
            '        If Not listaFattureLiquidazioneEXT Is Nothing AndAlso listaFattureLiquidazioneEXT.Count > 0 Then
            '            listaNotificaFatturaOutput = NotificaAttoFattura1(ListaFatture, "I", , itemLiquidazione.Dli_prog)

            '            If listaNotificaFatturaOutput.Count = 1 Then
            '                anagraficaFattura.Fattura.IdProgFatturaInLiquidazione = listaNotificaFatturaOutput.ElementAt(0).ProgrFatturaInLiquidazione
            '                result = Registra_BeneficiarioImpegnoLiquidazione(operatore, anagraficaFattura, sedeFattura, contoFattura, itemLiquidazione.Dli_Costo, fattura.Contratto.CodiceCIG, fattura.Contratto.CodiceCUP, "", itemLiquidazione.Dli_prog, "", 0)
            '            End If
            '        End If

            '    End If
            'Next



            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazionePreImpegnoProvvisorio")>
    Public Sub GenerazionePreImpegnoProvvisorio()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            'documento di lavoro
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            'valori della richiesta
            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            Dim importo As String = HttpContext.Current.Request.Item("ImpPrenotato")

            Dim NumPreImpegno As Long = 0
            Long.TryParse(HttpContext.Current.Request.Item("ComboPreimp"), NumPreImpegno)
            Dim tipoAssunzione As Integer = 0
            'HttpContext.Current.Request.Item("ComboCOGImp")

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                'tipoAtto = "DETERMINA"
                tipoAssunzione = 0
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                'tipoAtto = "DISPOSIZIONE"
                tipoAssunzione = 2
            ElseIf UCase(tipoAtto) = "DELIBERE" Then
                'tipoAtto = Nothing
                tipoAssunzione = 1
            End If

            tipoAtto = "PREIMP-PROVV"


            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            Dim importoDisponibilePreimpegno As Double
            Dim importoPrenotato As Double = Nothing
            If NumPreImpegno <> 0 Then
                Dim lstr_impDisp As String = HttpContext.Current.Request.Item("ImpDisp")
                lstr_impDisp = Trim(lstr_impDisp.Replace("€", "").Replace(".", ""))
                Double.TryParse(lstr_impDisp, importoDisponibilePreimpegno)
                importo = importo.Replace(".", ",")
                Double.TryParse(importo, importoPrenotato)


                If importoPrenotato <= 0 Then
                    Throw New Exception("Inserire un importo valido")
                End If
                If importoPrenotato > importoDisponibilePreimpegno Then
                    'log.info 
                    Throw New Exception("Importo Disponibile insufficiente")
                End If
            End If

            Dim cog As String = "" & HttpContext.Current.Request.Item("ComboCOGImp")
            If cog.ToLower.Contains("selez") Then
                cog = ""
            End If

            Dim result As String = ""
            'la lista dei preimpegno contiene solo preimpegni da delibera
            If NumPreImpegno = 0 Then
                'devo generare un nuovo preimpegno da determina
                'modifica per inserire numero provvisorio 

                Dim Importo1 As Double, Importo2 As Double, Importo3 As Double
                Dim ObGestionale1 As String, ObGestionale2 As String, ObGestionale3 As String
                Dim PCF1 As String, PCF2 As String, PCF3 As String

                If (Double.TryParse(HttpContext.Current.Request.Item("Importo1").Replace(".", ","), Importo1) AndAlso Importo1 > 0) Then

                    ObGestionale1 = HttpContext.Current.Request.Item("ObGestionale1")
                    If (ObGestionale1 = "") Then
                        Throw New Exception("Obiettivo gestionale 1 non valido")
                    End If

                    PCF1 = HttpContext.Current.Request.Item("PCF1")
                    If (PCF1 = "") Then
                        Throw New Exception("Piano dei conti finanziario 1 non valido")
                    End If
                End If

                If (Double.TryParse(HttpContext.Current.Request.Item("Importo2").Replace(".", ","), Importo2) AndAlso Importo2 > 0) Then

                    ObGestionale2 = HttpContext.Current.Request.Item("ObGestionale2")
                    If (ObGestionale2 = "") Then
                        Throw New Exception("Obiettivo gestionale 2 non valido")
                    End If

                    PCF2 = HttpContext.Current.Request.Item("PCF2")
                    If (PCF2 = "") Then
                        Throw New Exception("Piano dei conti finanziario 2 non valido")
                    End If
                End If

                If (Double.TryParse(HttpContext.Current.Request.Item("Importo3").Replace(".", ","), Importo3) AndAlso Importo3 > 0) Then

                    ObGestionale3 = HttpContext.Current.Request.Item("ObGestionale3")
                    If (ObGestionale3 = "") Then
                        Throw New Exception("Obiettivo gestionale 3 non valido")
                    End If

                    PCF3 = HttpContext.Current.Request.Item("PCF3")
                    If (PCF3 = "") Then
                        Throw New Exception("Piano dei conti finanziario 3 non valido")
                    End If
                End If
                Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
                Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

                Dim numeroProvvisOrDefAtto As String = ""
                If objDocumento.Doc_numero = "" then
                    numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                Else
                    numeroProvvisOrDefAtto = objDocumento.Doc_numero
                End If

                Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                Dim rispostaGenerazionePreImpegno As String() =
                     ClientIntegrazioneSic.MessageMaker.createGenerazionePreimpegnoPerPluriennaleMessage(
                        operatore, objDocumento.Doc_Oggetto, capitolo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numeroProvvisorio, 5),
                        Importo1, ObGestionale1, PCF1, Importo2, ObGestionale2, PCF2, Importo3, ObGestionale3, PCF3, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)

                Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)
                result = "{  success: true, Preimpegni: ['" & NumPreImpegno1 & "','" & NumPreImpegno2 & "','" & NumPreImpegno3 & "'] }"

                Dim bilancioCorrente As Integer
                Integer.TryParse(HttpContext.Current.Request.Item("Bilancio"), bilancioCorrente)

                'inserisco i dati nei nostri archivi
                Dim itemBilancio1 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno1 > 0 And idDocSIC1 > 0 Then
                    With itemBilancio1
                        .HashTokenCallSic = hashTokenCallSic
                        .IdDocContabileSic = idDocSIC1
                        .DBi_Anno = bilancioCorrente.ToString
                        .Dli_Cap = capitolo
                        .Dli_Costo = Importo1
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = bilancioCorrente.ToString
                        .Dli_Operatore = operatore.Codice
                        .Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_NPreImpegno = NumPreImpegno1
                        .Codice_Obbiettivo_Gestionale = ObGestionale1
                        .Piano_Dei_Conti_Finanziari = PCF1
                        .Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                        .Di_TipoAssunzioneDescr = tipoAtto
                        .Di_TipoAssunzione = tipoAssunzione
                        If Not objDocumento.Doc_numero Is Nothing And objDocumento.Doc_numero <> "" Then
                            .Di_Num_assunzione = objDocumento.Doc_numero
                        Else
                            .Di_Num_assunzione = objDocumento.Doc_numeroProvvisorio
                        End If
                        .Di_Data_Assunzione = Now()

                    End With

                    Registra_PreImpegniProvvisori(operatore, itemBilancio1)
                End If


                Dim itemBilancio2 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno2 > 0 And idDocSIC2 > 0 Then
                    With itemBilancio2
                        .HashTokenCallSic = hashTokenCallSic
                        .IdDocContabileSic = idDocSIC2
                        .DBi_Anno = (bilancioCorrente + 1).ToString
                        .Dli_Cap = capitolo
                        .Dli_Costo = Importo2
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = (bilancioCorrente + 1).ToString
                        .Dli_Operatore = operatore.Codice
                        .Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                        .Dli_NPreImpegno = NumPreImpegno2
                        .Codice_Obbiettivo_Gestionale = ObGestionale2
                        .Piano_Dei_Conti_Finanziari = PCF2
                        .Di_TipoAssunzioneDescr = tipoAtto
                        .Di_TipoAssunzione = tipoAssunzione
                        If Not objDocumento.Doc_numero Is Nothing And objDocumento.Doc_numero <> "" Then
                            .Di_Num_assunzione = objDocumento.Doc_numero
                        Else
                            .Di_Num_assunzione = objDocumento.Doc_numeroProvvisorio
                        End If
                        .Di_Data_Assunzione = Now()
                    End With

                    Registra_PreImpegniProvvisori(operatore, itemBilancio2)
                End If

                Dim itemBilancio3 As New DllDocumentale.ItemImpegnoInfo
                If NumPreImpegno3 > 0 And idDocSIC3 > 0 Then
                    With itemBilancio3
                        .HashTokenCallSic = hashTokenCallSic
                        .IdDocContabileSic = idDocSIC3
                        .DBi_Anno = (bilancioCorrente + 2).ToString
                        .Dli_Cap = capitolo
                        .Dli_Costo = Importo3
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = objDocumento.Doc_id
                        .Dli_Esercizio = (bilancioCorrente + 2).ToString
                        .Dli_Operatore = operatore.Codice
                        .Dli_UPB = HttpContext.Current.Request.Item("UPB")
                        .Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                        .Dli_NPreImpegno = NumPreImpegno3
                        .Codice_Obbiettivo_Gestionale = ObGestionale3
                        .Piano_Dei_Conti_Finanziari = PCF3
                        .Di_TipoAssunzioneDescr = tipoAtto
                        .Di_TipoAssunzione = tipoAssunzione
                        If Not objDocumento.Doc_numero Is Nothing And objDocumento.Doc_numero <> "" Then
                            .Di_Num_assunzione = objDocumento.Doc_numero
                        Else
                            .Di_Num_assunzione = objDocumento.Doc_numeroProvvisorio
                        End If
                        .Di_Data_Assunzione = Now()
                    End With

                    Registra_PreImpegniProvvisori(operatore, itemBilancio3)
                End If
            Else

                'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
                Dim residuo As Decimal = residuaDisponibilitaPreimpegno(operatore, NumPreImpegno, importoDisponibilePreimpegno)
                Dim totResiduo As Decimal = residuo - CDbl(importo)
                If (totResiduo) < 0 Then
                    Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(importo) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
                End If

                'non deve essere generato un nuovo preimpegno ma è necessario accontonare su un preimpegno esistente.
                Dim rispostaPrenotazionePreImpegno As String = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(operatore, "P", NumPreImpegno, importo)
                result = "{  success: true, NumPreImp: '" + CStr(NumPreImpegno) + "' }"

                'inserisco i dati nei nostri archivi
                Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo
                With itemBilancio
                    'steffffffff sulla prenotazione cosa inserisco nell'hash e nel idDocSIC?????'
                    .DBi_Anno = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_Cap = capitolo
                    .Dli_Costo = importoPrenotato
                    .Dli_DataRegistrazione = Now
                    .Dli_Documento = objDocumento.Doc_id
                    .Dli_Esercizio = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_Operatore = operatore.Codice
                    .Dli_UPB = HttpContext.Current.Request.Item("UPB")
                    .Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                    .Dli_NPreImpegno = NumPreImpegno
                    .Codice_Obbiettivo_Gestionale = cog
                    .Di_TipoAssunzioneDescr = tipoAtto
                    .Di_TipoAssunzione = tipoAssunzione
                    If Not objDocumento.Doc_numero Is Nothing And objDocumento.Doc_numero <> "" Then
                        .Di_Num_assunzione = objDocumento.Doc_numero
                    Else
                        .Di_Num_assunzione = objDocumento.Doc_numeroProvvisorio
                    End If
                    .Di_Data_Assunzione = Now()
                    ' lu 08/11/2010  .Codice_Obbiettivo_Gestionale = "" & HttpContext.Current.Request.Item("CodObGestionale")
                End With

                Registra_PreImpegniProvvisori(operatore, itemBilancio)
            End If

            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazionePreimpegnoRag")>
    Public Sub GenerazionePreimpegnoRag()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            Dim struttura As String = objDocumento.Doc_Cod_Uff_Pubblico
            Dim datamovimento As String = HttpContext.Current.Request.Item("DataMovimento")


            Dim preimpegniDaRegistrare As String = HttpContext.Current.Request.Item("PreimpegniRagioneria")
            preimpegniDaRegistrare = "[" & preimpegniDaRegistrare & "]"
            Dim listaPreimpegni As List(Of Ext_CapitoliAnnoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(preimpegniDaRegistrare, GetType(List(Of Ext_CapitoliAnnoInfo))), List(Of Ext_CapitoliAnnoInfo))

            'valori dela richiesta
            'Dim importo As String = listaPreimpegni.Item(0).ImpPrenotato
            'Dim contoEconomico As String = listaPreimpegni.Item(0).ContoEconomica
            'Dim capitolo As String = listaPreimpegni.Item(0).Capitolo
            'Dim upb As String = listaPreimpegni.Item(0).UPB
            'Dim missioneProgramma As String = listaPreimpegni.Item(0).MissioneProgramma
            'Dim esercizio As String = listaPreimpegni.Item(0).Bilancio
            'Dim Codice_Obbiettivo_Gestionale As String = listaPreimpegni.Item(0).Codice_Obbiettivo_Gestionale
            'Dim pcf As String = listaPreimpegni.Item(0).PianoDeiContiFinanziario



            Dim NumPreImpegno As String = listaPreimpegni.Item(0).NumPreImp

            
            Dim tipoAssunzione As String = ""
            Dim tipoAssunzioneDescr As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAssunzioneDescr) = "DETERMINE" Then
                tipoAssunzioneDescr = "DETERMINA"
                tipoAssunzione = 0
            ElseIf UCase(tipoAssunzioneDescr) = "DISPOSIZIONI" Then
                tipoAssunzioneDescr = "DISPOSIZIONE"
                tipoAssunzione = 2
            ElseIf UCase(tipoAssunzioneDescr) = "DELIBERE" Then
                tipoAssunzioneDescr = "DELIBERA"
                tipoAssunzione = 1
            End If

            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            'La registrazione in ragioneria avviene uno alla volta.
            'anche se il SIC risponde con un preimpegni multipli, io leggerò sempre e solo il primo numero preimp e il primo docid
            'si sta trasformando il preimp-provv in preimp-def, per cui devo generare un nuovo token per la nuova chiamata al SIC
            Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
            Dim rispostaGenerazionePreimpegno As String()
            rispostaGenerazionePreimpegno = ClientIntegrazioneSic.MessageMaker.createGenerazionePreImpegnoRagMessage(operatore, NumPreImpegno, objDocumento.Doc_Oggetto, tipoAssunzioneDescr, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), struttura, datamovimento, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)

            'Dim NumPreImpegnoRegistrato As Long = 0
            'If IsNumeric(rispostaGenerazionePreimpegno) Then
            '    Long.TryParse(rispostaGenerazionePreimpegno, NumPreImpegnoRegistrato)
            '    HttpContext.Current.Response.Write("{  success: true, NumPreimp: '" + rispostaGenerazionePreimpegno + "' }")
            'End If
            Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

            Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
            Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
            Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
            Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)

            
            ''chiamata ad insert_item_bilancio
            Dim itemRagPreimp As New DllDocumentale.ItemImpegnoInfo
            With itemRagPreimp
                .HashTokenCallSic = hashTokenCallSic
                .IdDocContabileSic = idDocSIC1
                .Dli_NPreImpegno = NumPreImpegno1
                .Dli_DataRegistrazione = Now
                .Dli_Operatore = operatore.Codice
                .Dli_prog = listaPreimpegni.Item(0).ID
                .Di_TipoAssunzione = tipoAssunzione
                .Di_Data_Assunzione = objDocumento.Doc_Data
                .Di_Num_assunzione = objDocumento.Doc_numero
                .Di_TipoAssunzioneDescr = tipoAssunzioneDescr
            End With

            Registra_RagPreimpegno(operatore, NumPreImpegno, itemRagPreimp)

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneImpegnoSuPerenteELiquidazione")>
    Public Sub GenerazioneImpegnoSuPerenteELiquidazione()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)


            Dim NumPreImpegno As String = ""
            Dim numPreDaPrenotazione As Integer = 0
            Dim result As String = ""

            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            'Dim importo As String = HttpContext.Current.Request.Item("ImpPrenotato")
            Dim NumImpegnoPerente As String = CLng(HttpContext.Current.Request.Item("ComboImpPer"))
            'Dim disponibilitaImpegnoPerente As String = HttpContext.Current.Request.Item("ImpDisp")

            'Dim BeneficiarioString As String = "" & HttpContext.Current.Request.Item("Beneficiario")
            'Dim Beneficiario As Ext_AnagraficaInfo = DirectCast(JavaScriptConvert.DeserializeObject(BeneficiarioString, GetType(Ext_AnagraficaInfo)), Ext_AnagraficaInfo)


            Dim listaBeneficiariStr As String = HttpContext.Current.Request.Item("ListaBeneficiariPerentiDaLiquidare")
            listaBeneficiariStr = "[" & listaBeneficiariStr & "]"
            Dim listaBeneficiari As List(Of Ext_AnagraficaInfo) = DirectCast(JavaScriptConvert.DeserializeObject(listaBeneficiariStr, GetType(List(Of Ext_AnagraficaInfo))), List(Of Ext_AnagraficaInfo))
            If Not listaBeneficiari Is Nothing AndAlso listaBeneficiari.Count < 1 Then
                Throw New Exception("Impossibile registrare, nessuna liquidazione presente")
            End If


            Dim cog As String = "" & HttpContext.Current.Request.Item("ComboCOGImpPer")
            If cog.ToLower.Contains("selez") Then
                cog = ""
                Throw New Exception("Codice Obiettivo Gestionale non valido")
            End If

            Dim pianoDeiContiFinanziario As String = "" & HttpContext.Current.Request.Item("ComboPdCFImpPer")
            If pianoDeiContiFinanziario.ToLower.Contains("selez") Then
                pianoDeiContiFinanziario = ""
                Throw New Exception("Piano dei Conti Finanziario non valido")
            End If

            

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            'Dim importoPrenotato As Double = Nothing
            'Double.TryParse(importo, importoPrenotato)

            'If importoPrenotato <= 0 Then
            '    Throw New Exception("Inserire un importo valido")
            'End If

            'Dim importoDisponibileImpegnoPerente As Double = Nothing
            'disponibilitaImpegnoPerente = Trim(disponibilitaImpegnoPerente.Replace("€", "").Replace(".", ""))
            'Double.TryParse(disponibilitaImpegnoPerente, importoDisponibileImpegnoPerente)

            'If importoDisponibileImpegnoPerente <= 0 Then
            '    Throw New Exception("L'impegno perente selezionato non ha più disponibilità!")
            'End If
            

            'Verifica dell'esistena dell'impegno in perenzione

            'Dim residuo As Decimal = residuaDisponibilitaDaPerente(operatore, "P" & NumImpegnoPerente, importoDisponibileImpegnoPerente)
            'Dim totResiduo As Decimal = residuo - importoPrenotato
            'If totResiduo < 0 Then
            '    Throw New Exception("Importo sull'impegno Perente  insufficiente, residuo " & CStr(residuo))
            'End If
            'If importoPrenotato > importoDisponibileImpegnoPerente Then
            '    'log.info 
            '    Throw New Exception("Importo sull'impegno Perente  insufficiente, residuo " & CStr(importoDisponibileImpegnoPerente))
            'End If
            result = "{  success: true }"

            NumPreImpegno = "P" & NumImpegnoPerente
            
           Dim messaggioErrore As String = ""
            For Each beneficiarioDaLiquidare As Ext_AnagraficaInfo In listaBeneficiari
                'chiamata ad insert_item_bilancio
                Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo
                With itemBilancio
                    .HashTokenCallSic_Imp = GenerateHashTokenCallSic()
                    .DBi_Anno = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_Cap = capitolo
                    .Dli_Costo = beneficiarioDaLiquidare.ImportoDaLiquidare
                    .Dli_DataRegistrazione = Now
                    .Dli_Documento = objDocumento.Doc_id
                    .Dli_Esercizio = HttpContext.Current.Request.Item("Bilancio")
                    .Dli_NumImpegno = "0"
                    .Dli_Operatore = operatore.Codice
                    .Dli_UPB = HttpContext.Current.Request.Item("UPB")
                    .Dli_MissioneProgramma = HttpContext.Current.Request.Item("MissioneProgramma")
                    .Dli_NPreImpegno = NumPreImpegno
                    .Di_PreImpDaPrenotazione = numPreDaPrenotazione
                    .NDocPrecedente = NumImpegnoPerente
                    .Codice_Obbiettivo_Gestionale = cog
                    .Piano_Dei_Conti_Finanziari = pianoDeiContiFinanziario
                End With

                Dim itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo = Registra_ImpegnoSuPerenteEliquidazione(operatore, itemBilancio)
                Dim idImpegno As Long = itemBilancio.Dli_prog
                Dim idLiquidazione As String = ""
                
                If Not itemLiquidazione Is Nothing Then
                    idLiquidazione = itemLiquidazione.Dli_prog.ToString

                    If Not beneficiarioDaLiquidare Is Nothing AndAlso beneficiarioDaLiquidare.ID <> "" AndAlso beneficiarioDaLiquidare.ID <> "0" Then
                        Dim listaSedi As List(Of Ext_SedeAnagraficaInfo) = beneficiarioDaLiquidare.ListaSedi
                        Dim sede As New Ext_SedeAnagraficaInfo
                        Dim datiBancari As New Ext_DatiBancariInfo
                        If Not listaSedi Is Nothing AndAlso listaSedi.Count > 0 Then
                            sede = listaSedi.ElementAt(0)

                            Dim listaDatiBancari As List(Of Ext_DatiBancariInfo) = sede.DatiBancari
                            If Not listaDatiBancari Is Nothing AndAlso listaDatiBancari.Count > 0 Then
                                datiBancari = listaDatiBancari.ElementAt(0)
                            End If

                            Dim codiceCig As String = ""
                            Dim codiceCUP As String = ""
                            If Not beneficiarioDaLiquidare.Contratto Is Nothing Then
                                codiceCig = beneficiarioDaLiquidare.Contratto.CodiceCIG
                                codiceCUP = beneficiarioDaLiquidare.Contratto.CodiceCUP
                            End If
                        
                            messaggioErrore = Registra_BeneficiarioImpegnoLiquidazione(operatore, beneficiarioDaLiquidare, sede, datiBancari, beneficiarioDaLiquidare.ImportoDaLiquidare, codiceCig, codiceCUP, "", "", idImpegno, "")

                            messaggioErrore = Registra_BeneficiarioImpegnoLiquidazione(operatore, beneficiarioDaLiquidare, sede, datiBancari, beneficiarioDaLiquidare.ImportoDaLiquidare, codiceCig, codiceCUP, "", idLiquidazione, "", "")

                            If messaggioErrore <> "" AndAlso messaggioErrore.Contains("success: false") Then
                                Elimina_LiquidazioneConId(operatore, itemLiquidazione)
                                Elimina_ImpegnoByID(operatore, itemBilancio)

                                Log.Error(messaggioErrore)
                            Else
                                messaggioErrore = "{  success: true, progLiquidazione: '" + idLiquidazione + "' }"
                            End If
                        End If
                    End If
                End If
             Next

            
            HttpContext.Current.Response.Write(messaggioErrore)

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Public Shared Function Registra_Bilancio(ByVal operatore As DllAmbiente.Operatore, ByRef itemBilancio As DllDocumentale.ItemImpegnoInfo) As DllDocumentale.ItemImpegnoInfo
        Try
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Log.Debug(operatore.Codice)
            itemBilancio = dllDoc.FO_Insert_Bilancio(itemBilancio)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return itemBilancio
    End Function
    Private Sub Registra_PreImpegniProvvisori(ByVal operatore As DllAmbiente.Operatore, ByRef itemBilancio As DllDocumentale.ItemImpegnoInfo)
        Try
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Log.Debug(operatore.Codice)
            itemBilancio = dllDoc.FO_Insert_PreimpegniProvvisori(itemBilancio)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Function Registra_ImpegnoSuPerenteEliquidazione(ByVal operatore As DllAmbiente.Operatore, ByRef itemBilancio As DllDocumentale.ItemImpegnoInfo) As DllDocumentale.ItemLiquidazioneInfo
        Dim itemLiqui As DllDocumentale.ItemLiquidazioneInfo
        Try
            Log.Debug(operatore.Codice)
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            If itemBilancio.Dli_prog > 0 Then
                itemBilancio = dllDoc.FO_Update_Bilancio(itemBilancio)
                Dim liq As List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(itemBilancio.Dli_Documento, , itemBilancio.Dli_NPreImpegno)
                If liq.Count > 0 Then
                    itemLiqui = liq.Item(0)
                    itemLiqui.Di_Stato = 1
                    itemLiqui.HashTokenCallSic = GenerateHashTokenCallSic()
                    itemLiqui = dllDoc.FO_Update_Liquidazione(itemLiqui)
                End If

            Else
                itemBilancio = dllDoc.FO_Insert_Bilancio(itemBilancio)

                itemLiqui = New DllDocumentale.ItemLiquidazioneInfo
                itemLiqui.HashTokenCallSic = GenerateHashTokenCallSic()
                itemLiqui.Dli_IdImpegno = itemBilancio.Dli_prog
                itemLiqui.Di_ContoEconomica = itemBilancio.Di_ContoEconomica
                itemLiqui.Di_Stato = itemBilancio.Di_Stato
                itemLiqui.Dli_Cap = itemBilancio.Dli_Cap
                itemLiqui.Dli_Costo = itemBilancio.Dli_Costo
                itemLiqui.Dli_DataRegistrazione = itemBilancio.Dli_DataRegistrazione
                itemLiqui.Dli_Documento = itemBilancio.Dli_Documento
                itemLiqui.Dli_Esercizio = itemBilancio.Dli_Esercizio
                itemLiqui.Dli_NPreImpegno = itemBilancio.Dli_NPreImpegno
                itemLiqui.Dli_NumImpegno = itemBilancio.Dli_NumImpegno
                itemLiqui.Dli_Operatore = itemBilancio.Dli_Operatore
                itemLiqui.Dli_UPB = itemBilancio.Dli_UPB
                itemLiqui.Dli_MissioneProgramma = itemBilancio.Dli_MissioneProgramma
                itemLiqui.Dli_Data_Assunzione = Now
                itemLiqui.Dli_Anno = itemBilancio.DBi_Anno
                If Not itemBilancio.Piano_Dei_Conti_Finanziari Is Nothing AndAlso itemBilancio.Piano_Dei_Conti_Finanziari <> "" Then
                    itemLiqui.Dli_PianoDeiContiFinanziari = itemBilancio.Piano_Dei_Conti_Finanziari
                Else
                    itemLiqui.Dli_PianoDeiContiFinanziari = itemBilancio.Dli_PianoDeiContiFinanziari
                End If

                itemLiqui = dllDoc.FO_Insert_Liquidazione(itemLiqui)

            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return itemLiqui
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaPreImpegno")>
    Public Function EliminaPreImpegno(ByVal NumeroPreImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo


            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If

            If String.IsNullOrEmpty(NumeroPreImpegno) Then
                Throw New Exception("impossibile trovare il preimpegno da eliminare")
            End If

            If String.IsNullOrEmpty(ID) Then
                Throw New Exception("Impossibile trovare il preimpegno da eliminare")
            End If

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            itemBilancio = dllDoc.FO_Get_DatiImpegni(CodDocumento(), ID).Item(0)
            If NumeroPreImpegno = itemBilancio.Dli_NPreImpegno Then

                Dim rispostaEliminazionePreImpegno As Boolean = True
                If String.IsNullOrEmpty(itemBilancio.NDocPrecedente) Then

                    'non è un impegno su perente
                    If itemBilancio.Di_PreImpDaPrenotazione = 1 Then
                        'Prenotazione da delibera, cancello l'accontanamento
                        ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(operatore, "C", itemBilancio.Dli_NPreImpegno, itemBilancio.Dli_Costo)
                    Else
                        'Prenotazione da determina, cancello l'intero preimpegno
                        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(CodDocumento)
                        
                        Dim numeroProvvisOrDefAtto As String = ""
                        If objDocumento.Doc_numero = "" then
	                        numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                        Else
	                        numeroProvvisOrDefAtto = objDocumento.Doc_numero
                        End If

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
                        Dim dataAtto As Date
                        dataAtto = objDocumento.Doc_Data

                        rispostaEliminazionePreImpegno = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(operatore, NumeroPreImpegno, tipoAtto, dataAtto, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())


                    End If

                End If


                'Inizio Modifica 15.02.2015
                'ELIMINAZIONE FATTURE_IMPEGNO CON NOTIFICA AL SIC

                'Dim listaFattureImpegnoEXT As List(Of Ext_FatturaInfo) = GetListaFattByImpegno(itemBilancio.Dli_prog, CodDocumento)
                'If Not listaFattureImpegnoEXT Is Nothing AndAlso listaFattureImpegnoEXT.Count > 0 Then
                '    NotificaAttoFattura(listaFattureImpegnoEXT, "C", itemBilancio.Dli_NPreImpegno, )
                'End If


                'Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureByLiquidazione(itemliquidazione.Dli_prog, CodDocumento())
                'Fine Modifica 15.02.2015

                'Inizio Modifica 15.02.2015
                'ELIMINAZIONE FATTURE_LIQUIDAZIONE CON NOTIFICA AL SIC
                Dim datiLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_DatiLiquidazione(CodDocumento, , NumeroPreImpegno)
                'Prendo il primo elemento della lista perche si invia la notifica al sic per ogni liquidazione presente per
                If (datiLiquidazione.Count > 0) Then
                    Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = GetListaFattByLiquidazione(datiLiquidazione(0).Dli_prog)
                    If Not listaFattureLiquidazioneEXT Is Nothing AndAlso listaFattureLiquidazioneEXT.Count > 0 Then
                        NotificaAttoFattura(listaFattureLiquidazioneEXT, "C", , datiLiquidazione(0).Dli_prog)
                    End If

                    'Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureByLiquidazione(itemliquidazione.Dli_prog, CodDocumento())

                End If



                'ELIMINA IMPEGNO
                Elimina_ImpegnoByID(operatore, itemBilancio)


                'Fine Modifica 15.02.2015

                Dim itemliquidazione As New DllDocumentale.ItemLiquidazioneInfo

                itemliquidazione.Dli_Documento = CodDocumento()
                itemliquidazione.Dli_NPreImpegno = NumeroPreImpegno
                If String.IsNullOrEmpty(itemBilancio.NDocPrecedente) Then
                    'Cancello le liquidazioni con quel numero di impegno, nel caso di liquidazioni con lo stesso preimpegno
                    'da delibera, cancello tutte le liq.
                    'è possibile aggiungerne altre
                    Elimina_Liquidazione(operatore, itemliquidazione)
                Else
                    'cancello le liquidazioni da impegno perente con quell'importo
                    itemliquidazione.Dli_Costo = itemBilancio.Dli_Costo
                    itemliquidazione.Dli_NPreImpegno = NumeroPreImpegno
                    Elimina_LiquidazioneConImporto(operatore, itemliquidazione)
                End If


                If Not rispostaEliminazionePreImpegno Then
                    Throw New Exception("")
                Else
                    Return True
                End If
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaPreImpegnoProvvisorio")>
    Public Function EliminaPreImpegnoProvvisorio(ByVal NumeroPreImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemImpegno As New DllDocumentale.ItemImpegnoInfo


            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If

            If String.IsNullOrEmpty(NumeroPreImpegno) Then
                Throw New Exception("impossibile trovare il preimpegno da eliminare")
            End If

            If String.IsNullOrEmpty(ID) Then
                Throw New Exception("Impossibile trovare il preimpegno da eliminare")
            End If

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            itemImpegno = dllDoc.FO_Get_DatiPreImpegni(CodDocumento(), ID).Item(0)
            If NumeroPreImpegno = itemImpegno.Dli_NPreImpegno Then

                Dim rispostaEliminazionePreImpegno As Boolean = True
                If String.IsNullOrEmpty(itemImpegno.NDocPrecedente) Then


                    'Prenotazione da determina, cancello l'intero preimpegno
                    Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(CodDocumento)

                    Dim numeroProvvisOrDefAtto As String = ""
                    If objDocumento.Doc_numero = "" then
	                    numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                    Else
	                    numeroProvvisOrDefAtto = objDocumento.Doc_numero
                    End If


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
                    Dim dataAtto As Date
                    dataAtto = objDocumento.Doc_Data

                    rispostaEliminazionePreImpegno = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(operatore, NumeroPreImpegno, tipoAtto, dataAtto, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())



                End If
                If rispostaEliminazionePreImpegno Then
                    Elimina_PreimpegnoByID(operatore, itemImpegno)
                Else
                    Throw New Exception("")
                End If

            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaLiquidazioneDaPreImpegno")>
    Public Function EliminaLiquidazioneDaPreImpegno(ByVal NumeroPreImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            'Non cancelliamo + ?  Dim rispostaEliminazionePreImpegno As Boolean = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpegnoMessage(operatore, NumeroPreImpegno)

            'Non cancelliamo + ?Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo

            'Non cancelliamo + ? If String.IsNullOrEmpty(CodDocumento) Then
            'Non cancelliamo + ?Throw New Exception("impossibile trovare documento associato la preimpegno")
            'Non cancelliamo + ? End If

            'Non cancelliamo + ? If String.IsNullOrEmpty(NumeroPreImpegno) Then
            'Non cancelliamo + ?Throw New Exception("impossibile trovare il preimpegno da eliminare")
            'Non cancelliamo + ? End If

            'Non cancelliamo + ? itemBilancio.Dli_Documento = CodDocumento()
            'Non cancelliamo + ?  itemBilancio.Dli_NPreImpegno = NumeroPreImpegno

            'Non cancelliamo + ? Elimina_Bilancio(itemBilancio)

            Dim itemliquidazione As New DllDocumentale.ItemLiquidazioneInfo

            itemliquidazione.Dli_Documento = CodDocumento()
            itemliquidazione.Dli_NPreImpegno = NumeroPreImpegno
            itemliquidazione.Dli_prog = ID

            Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = GetListaFattByLiquidazione(itemliquidazione.Dli_prog)
            If Not listaFattureLiquidazioneEXT Is Nothing AndAlso listaFattureLiquidazioneEXT.Count > 0 Then
                NotificaAttoFattura(listaFattureLiquidazioneEXT, "C", , itemliquidazione.Dli_prog)
            End If

            Elimina_Liquidazione(operatore, itemliquidazione)

            'Non cancelliamo + ?  If Not rispostaEliminazionePreImpegno Then
            'Non cancelliamo + ?Throw New Exception("")
            'Non cancelliamo + ?  Else


            'Dim listaFattureLiquidazione As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureByLiquidazione(itemliquidazione.Dli_prog, CodDocumento)





            Return True
            'Non cancelliamo + ?End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaRiduzione")>
    Public Function EliminaRiduzione(ByVal NumImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            '   Dim rispostaEliminazionePreImpegno As Boolean = ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpegnoMessage(operatore, NumeroPreImpegno)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemRiduzione As New DllDocumentale.ItemRiduzioneInfo

            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If

            If String.IsNullOrEmpty(NumImpegno) Then
                Throw New Exception("impossibile trovare l'impegno da eliminare")
            End If

            itemRiduzione.Dli_Documento = CodDocumento()
            itemRiduzione.Dli_NumImpegno = NumImpegno
            itemRiduzione.Dli_prog = ID

            Elimina_Riduzione(operatore, itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/SetLiquidazioni")>
    Public Sub SetLiquidazioni()
        Dim itemLiq As DllDocumentale.ItemLiquidazioneInfo = New DllDocumentale.ItemLiquidazioneInfo
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim liquidazioniCapitoli As String = HttpContext.Current.Request.Item("LiquidazioniCapitoli")
            liquidazioniCapitoli = "[" & liquidazioniCapitoli & "]"
            Dim listaLiquidazioni As List(Of Ext_LiquidazioneInfo) = DirectCast(JavaScriptConvert.DeserializeObject(liquidazioniCapitoli, GetType(List(Of Ext_LiquidazioneInfo))), List(Of Ext_LiquidazioneInfo))
            If Not listaLiquidazioni Is Nothing AndAlso listaLiquidazioni.Count < 1 Then
                Throw New Exception("Impossibile registrare, nessuna liquidazione presente")
            End If

            Dim itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo
            For Each liquidazione As Ext_LiquidazioneInfo In listaLiquidazioni

                Dim registraLiquidazione As Boolean = False
                'registro solo se è valorizzato l'importo da liquidare

                If Not String.IsNullOrEmpty(liquidazione.ImpPrenotatoLiq) AndAlso liquidazione.ImpPrenotatoLiq <> 0 Then
                    Dim prog_Lig As Long = 0
                    Long.TryParse(liquidazione.ID, prog_Lig)
                    Dim residuo As Decimal = calcolaResiduaDisponibilitaDelPreimpegno(operatore, liquidazione.NumPreImp, prog_Lig)
                    Dim totResiduo As Decimal = residuo - liquidazione.ImpPrenotatoLiq
                    If totResiduo < 0 Then
                        Throw New Exception("Disponibilita insufficiente per la liquidazione di € " & CStr(liquidazione.ImpPrenotatoLiq))
                    End If

                    

                    itemLiquidazione = New DllDocumentale.ItemLiquidazioneInfo
                    With itemLiquidazione
                        Long.TryParse(liquidazione.ID, .Dli_prog)

                        If liquidazione.HashTokenCallSic Is Nothing Then
                            .HashTokenCallSic = GenerateHashTokenCallSic()
                        Else
                            .HashTokenCallSic = liquidazione.HashTokenCallSic
                        End If
                        
                        .IdDocContabileSic = liquidazione.IdDocContabileSic
                        .Dli_Anno = liquidazione.AnnoPrenotazione
                        .Dli_Cap = liquidazione.Capitolo
                        .Dli_Costo = liquidazione.ImpPrenotatoLiq
                        .Dli_DataRegistrazione = Now
                        .Dli_Documento = CodDocumento()
                        .Dli_Esercizio = liquidazione.Bilancio
                        .Dli_NumImpegno = 0
                        .Dli_Operatore = operatore.Codice
                        .Dli_UPB = liquidazione.UPB
                        .Dli_MissioneProgramma = liquidazione.MissioneProgramma
                        'determina corrente, quindi non c'è ancora il num definitivo
                        .Dli_TipoAssunzione = 0
                        .Dli_Data_Assunzione = Now
                        .Dli_Num_assunzione = 0
                        .Dli_NPreImpegno = liquidazione.NumPreImp
                        .Dli_PianoDeiContiFinanziari = liquidazione.PianoDeiContiFinanziario

                        Dim extListBeneficiari As System.Collections.Generic.List(Of Ext_AnagraficaInfo) = liquidazione.ListaBeneficiari
                        Dim itemListBeneficiari As New System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
                        If Not extListBeneficiari Is Nothing Then
                            For Each anagraficaScelta As Ext_AnagraficaInfo In extListBeneficiari
                                Dim oggettoDaInserire As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo

                                oggettoDaInserire.IdAnagrafica = anagraficaScelta.ID
                                oggettoDaInserire.EsenzCommBonifico = anagraficaScelta.Commissioni
                                oggettoDaInserire.Cig = anagraficaScelta.Contratto.CodiceCIG
                                oggettoDaInserire.Cup = anagraficaScelta.Contratto.CodiceCUP
                                If (anagraficaScelta.Tipologia = "F") Then
                                    oggettoDaInserire.FlagPersonaFisica = True
                                    oggettoDaInserire.CodiceFiscale = anagraficaScelta.CodiceFiscale
                                    If String.IsNullOrEmpty(anagraficaScelta.Cognome) then
                                        oggettoDaInserire.Denominazione = anagraficaScelta.Denominazione
                                    Else
                                        oggettoDaInserire.Denominazione = anagraficaScelta.Cognome & " " & anagraficaScelta.Nome
                                    End If
                                Else
                                    oggettoDaInserire.PartitaIva = anagraficaScelta.PartitaIva
                                    oggettoDaInserire.FlagPersonaFisica = False
                                    oggettoDaInserire.Denominazione = anagraficaScelta.Denominazione
                                End If

                                If Not anagraficaScelta.Contratto Is Nothing Then
                                    oggettoDaInserire.IdContratto = anagraficaScelta.Contratto.Id
                                    oggettoDaInserire.NumeroRepertorioContratto = anagraficaScelta.Contratto.NumeroRepertorio
                                    oggettoDaInserire.IsDatoSensibile = anagraficaScelta.IsDatoSensibile
                                    If Not anagraficaScelta.Fattura Is Nothing Then
                                        oggettoDaInserire.ProgFatturaLiq = anagraficaScelta.Fattura.IdProgFatturaInLiquidazione
                                    End If
                                End If

                                Dim listaSedi As List(Of Ext_SedeAnagraficaInfo) = anagraficaScelta.ListaSedi
                                If Not listaSedi Is Nothing AndAlso listaSedi.Count = 1 Then
                                    Dim sedeScelta As Ext_SedeAnagraficaInfo = listaSedi.ElementAt(0)
                                    If Not sedeScelta Is Nothing Then
                                        oggettoDaInserire.IdSede = sedeScelta.IdSede
                                        oggettoDaInserire.IdModalitaPag = sedeScelta.IdModalitaPagamento
                                        oggettoDaInserire.SedeComune = sedeScelta.Comune
                                        oggettoDaInserire.SedeVia = sedeScelta.Indirizzo
                                        oggettoDaInserire.SedeProvincia = sedeScelta.CapComune

                                        Dim datiBancari As List(Of Ext_DatiBancariInfo) = sedeScelta.DatiBancari
                                        If Not datiBancari Is Nothing AndAlso datiBancari.Count = 1 Then
                                            Dim contoScelto As Ext_DatiBancariInfo = datiBancari.ElementAt(0)
                                            If Not contoScelto Is Nothing Then
                                                oggettoDaInserire.IdConto = contoScelto.IdContoCorrente
                                                If Not String.IsNullOrEmpty(contoScelto.Iban) Then
                                                    oggettoDaInserire.Iban = contoScelto.Iban
                                                Else
                                                    oggettoDaInserire.Iban = contoScelto.ContoCorrente
                                                End If
                                            End If
                                        End If


                                    End If
                                End If


                                oggettoDaInserire.ImportoSpettante = liquidazione.ImpPrenotatoLiq
                                oggettoDaInserire.IdDocumento = CodDocumento()
                                oggettoDaInserire.Operatore = operatore.Codice


                                itemListBeneficiari.Add(oggettoDaInserire)
                            Next
                        End If
                        .ListaBeneficiari = itemListBeneficiari

                        If Not String.IsNullOrEmpty(liquidazione.IdImpegno) Then
                            .Dli_IdImpegno = liquidazione.IdImpegno
                        End If
                    End With

                    If liquidazione.ID = 0 Then

                        itemLiq = Registra_Liquidazione(operatore, itemLiquidazione)
                    Else
                        Aggiorna_Liquidazione(operatore, itemLiquidazione)
                    End If

                    Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
                    If Not itemLiquidazione.ListaBeneficiari Is Nothing AndAlso itemLiq.Dli_prog <> 0 Then
                        For Each beneficiario As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In itemLiquidazione.ListaBeneficiari
                            beneficiario.IDDocumentoContabile = itemLiq.Dli_prog
                            beneficiario.ID = 0
                            dllDoc.FO_Registra_LiquidazioneBeneficiario(beneficiario, operatore)
                        Next
                    Else
                        Dim listaBenRegistrati As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = dllDoc.FO_Get_ListaBeneficiariLiquidazione(operatore, , , itemLiquidazione.Dli_prog)
                        If Not listaBenRegistrati Is Nothing AndAlso listaBenRegistrati.Count = 1 Then
                            For Each beneficiario As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In listaBenRegistrati
                                beneficiario.ImportoSpettante = itemLiquidazione.Dli_Costo
                                dllDoc.FO_Registra_LiquidazioneBeneficiario(beneficiario, operatore)
                            Next
                        End If

                    End If
                Else
                    Throw New Exception("Importo da liquidare obbligatorio")
                End If
            Next

            HttpContext.Current.Response.Write("{ success: true, progLiquidazione: '" + itemLiq.Dli_prog.ToString + "" + "'" + "}")
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/SetRiduzioni")>
    Public Sub SetRiduzioni()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim riduzioniContestuali As String = HttpContext.Current.Request.Item("RiduzioniContestuali")
            riduzioniContestuali = "[" & riduzioniContestuali & "]"
            Dim listaRiduzioni As List(Of Ext_LiquidazioneInfo) = DirectCast(JavaScriptConvert.DeserializeObject(riduzioniContestuali, GetType(List(Of Ext_LiquidazioneInfo))), List(Of Ext_LiquidazioneInfo))

            Dim itemRiduzione As DllDocumentale.ItemRiduzioneInfo
            For Each riduzione As Ext_LiquidazioneInfo In listaRiduzioni
                If Not String.IsNullOrEmpty(riduzione.ImpPrenotato) AndAlso riduzione.ImpPrenotato <> 0 Then
                    itemRiduzione = New DllDocumentale.ItemRiduzioneInfo
                    With itemRiduzione
                        Long.TryParse(riduzione.ID, .Dli_prog)
                        .HashTokenCallSic = riduzione.HashTokenCallSic
                        .IdDocContabileSic = riduzione.IdDocContabileSic
                        .Dli_Documento = CodDocumento()
                        .Dli_DataRegistrazione = Now
                        .Dli_Operatore = operatore.Codice
                        .Dli_Esercizio = riduzione.Bilancio
                        .Dli_UPB = riduzione.UPB
                        .Dli_MissioneProgramma = riduzione.MissioneProgramma
                        .Dli_Cap = riduzione.Capitolo
                        .Dli_Costo = riduzione.ImpPrenotato
                        .Dli_NumImpegno = riduzione.NumImpegno
                        'LU Rid
                        If riduzione.IsEconomia = "" Then
                            Throw New Exception("E' necessario specificare un valore tra Economia/Riduzione (Riduzione per impegni dell'anno corrente, Economia per impegni  degli anni precedenti)")
                        End If
                        .Div_IsEconomia = riduzione.IsEconomia
                        'condizione da testare
                        If Not riduzione.NumImpegno Is Nothing AndAlso riduzione.NumImpegno <> "" Then
                            Dim capLista As List(Of Ext_CapitoliAnnoInfo) = GetDettaglioImpegno(riduzione.Bilancio, .Dli_Cap, riduzione.NumImpegno, operatore.oUfficio.CodUfficio)
                            Dim cap As Ext_CapitoliAnnoInfo = Nothing
                            If capLista.Count = 1 Then
                                cap = capLista(0)
                            End If
                            Select Case UCase(cap.TipoAtto)
                                Case "DETERMINA"
                                    .Div_TipoAssunzione = 0
                                Case "DELIBERA"
                                    .Div_TipoAssunzione = 1
                            End Select
                            .Div_Data_Assunzione = cap.DataAtto
                            .Div_Num_assunzione = cap.NumeroAtto

                            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
                            Dim residuo As Decimal = residuaDisponibilitaDaImpegno(operatore, cap.NumImpegno, cap.ImpDisp)
                            Dim totResiduo As Decimal = residuo - riduzione.ImpPrenotato
                            If (totResiduo) < 0 Then
                                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(riduzione.ImpPrenotato) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
                            End If
                        End If

                        'fine modifica

                        .DBi_Anno = riduzione.Bilancio
                        .Di_Stato = 1
                    End With
                    Registra_Riduzione(operatore, itemRiduzione)
                Else
                    Throw New Exception("Importo da ridurre obbligatorio")
                End If
            Next

            HttpContext.Current.Response.Write("{ success: true }")
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Public Shared Function calcolaResiduaDisponibilitaDelPreimpegno(ByVal operatore As DllAmbiente.Operatore, ByVal numPreim As String, ByVal proLiq As Long) As Decimal
        Log.Debug(operatore.Codice)

        Dim idoc As String = CodDocumento()
        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista_Liq As IList = dllDoc.FO_Get_ListaLiquidazioniByPreImpegno(numPreim, False, idoc)
        Dim totale_Liq As Decimal = 0
        Dim totale_PreImp As Decimal = 0
        For Each itemLig As DllDocumentale.ItemLiquidazioneInfo In lista_Liq
            If itemLig.Dli_prog <> proLiq Then
                totale_Liq = totale_Liq + itemLig.Dli_Costo
            End If

        Next

        Dim lista_imp As IList = dllDoc.FO_Get_DatiImpegniByNPreImp(numPreim, idoc)

        For Each itemImp As DllDocumentale.ItemImpegnoInfo In lista_imp
            totale_PreImp = totale_PreImp + itemImp.Dli_Costo
        Next

        Return totale_PreImp - totale_Liq

    End Function
    Function residuaDisponibilitaDaImpegno(ByVal operatore As DllAmbiente.Operatore, ByVal NumImpegno As String, ByVal importoDisponibileImpegno As Decimal) As Double
        Log.Debug(operatore.Codice)

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista_Liq As IList = dllDoc.FO_Get_ListaLiquidazioniByPreImpegno(NumImpegno, True)
        Dim totale_Liq As Decimal = 0

        For Each itemLiq As DllDocumentale.ItemLiquidazioneInfo In lista_Liq
            If itemLiq.Dli_NLiquidazione = 0 And itemLiq.Di_Stato = 1 Then
                totale_Liq = totale_Liq + itemLiq.Dli_Costo
            End If
        Next

        'controllo anche su eventuali riduzioni
        Dim lista_var As IList = dllDoc.FO_Get_DatiImpegniVariazioniConNumImpegno(NumImpegno)
        For Each itemVar As DllDocumentale.ItemRiduzioneInfo In lista_var
            If itemVar.Div_NumeroReg = 0 And itemVar.Di_Stato = 1 Then
                totale_Liq = totale_Liq + itemVar.Dli_Costo
            End If
        Next

        Return importoDisponibileImpegno - totale_Liq

    End Function
    Function residuaDisponibilitaDaPerente(ByVal operatore As DllAmbiente.Operatore, ByVal NumPreImp As String, ByVal importoDisponibileImpegno As Decimal) As Double
        Log.Debug(operatore.Codice)

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista_Liq As IList = dllDoc.FO_Get_ListaLiquidazioniByPreImpegno(NumPreImp, False)
        Dim totale_Liq As Decimal = 0

        For Each itemLiq As DllDocumentale.ItemLiquidazioneInfo In lista_Liq
            If itemLiq.Dli_NLiquidazione = 0 And itemLiq.Di_Stato = 1 Then
                totale_Liq = totale_Liq + itemLiq.Dli_Costo
            End If
        Next

        Return importoDisponibileImpegno - totale_Liq

    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaLiquidazione")>
    Public Function EliminaLiquidazione(ByVal NumeroPreImpegno As String, ByVal NumImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemliquidazione As New DllDocumentale.ItemLiquidazioneInfo

            itemliquidazione.Dli_Documento = CodDocumento()
            itemliquidazione.Dli_prog = ID

            Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = GetListaFattByLiquidazione(itemliquidazione.Dli_prog)
            If Not listaFattureLiquidazioneEXT Is Nothing AndAlso listaFattureLiquidazioneEXT.Count > 0 Then
                NotificaAttoFattura(listaFattureLiquidazioneEXT, "C", , itemliquidazione.Dli_prog)
            End If

            Elimina_LiquidazioneConId(operatore, itemliquidazione)



        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    Private Sub Aggiorna_Liquidazione(ByVal operatore As DllAmbiente.Operatore, ByRef itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            itemLiquidazione = dllDoc.FO_Update_Liquidazione(itemLiquidazione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Aggiorna_Riduzione(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            itemRiduzione = dllDoc.FO_Update_Impegno_Var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Aggiorna_RiduzionePreImp(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            itemRiduzione = dllDoc.FO_Update_Preimpegno_Var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub

    Private Sub Aggiorna_RiduzioneLiq(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneLiqInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            itemRiduzione = dllDoc.FO_Update_Liquidazione_Var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Public Shared Function Registra_Liquidazione(ByVal operatore As DllAmbiente.Operatore, ByRef itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo) As DllDocumentale.ItemLiquidazioneInfo
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            itemLiquidazione = dllDoc.FO_Insert_Liquidazione(itemLiquidazione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return itemLiquidazione
    End Function
    Private Sub Registra_Riduzione(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            If itemRiduzione.Dli_prog > 0 Then
                itemRiduzione = dllDoc.FO_Update_Impegno_Var(itemRiduzione)
            Else
                itemRiduzione = dllDoc.FO_Insert_Impegno_Var(itemRiduzione)
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Registra_Accertamento(ByVal operatore As DllAmbiente.Operatore, ByRef itemAccertamento As DllDocumentale.ItemAssunzioneContabileInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            If itemAccertamento.Da_prog > 0 Then
                itemAccertamento = dllDoc.FO_Update_Assunzione(itemAccertamento)
            Else
                itemAccertamento = dllDoc.FO_Insert_Assunzione(itemAccertamento)
            End If


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetImpegniRegistrati?CodiceUfficio={CodiceUfficio}")>
    Public Function GetImpegniRegistrati(ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaImpegniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim impegniRegistrati As System.Collections.Generic.List(Of DllDocumentale.ItemImpegnoInfo) = Get_Impegni(operatore)

            Dim itemExt_Impegno As Ext_CapitoliAnnoInfo
            For Each impegno As DllDocumentale.ItemImpegnoInfo In impegniRegistrati
                itemExt_Impegno = New Ext_CapitoliAnnoInfo
                With itemExt_Impegno
                    '.AnnoPrenotazione = impegno.DBi_Anno
                    .HashTokenCallSic = impegno.HashTokenCallSic
                    .IdDocContabileSic = impegno.IdDocContabileSic
                    .HashTokenCallSic_Imp = impegno.HashTokenCallSic_Imp
                    .IdDocContabileSic_Imp = impegno.IdDocContabileSic_Imp
                    .Tipo = " "
                    .ID = impegno.Dli_prog
                    .Bilancio = impegno.Dli_Esercizio
                    .Capitolo = impegno.Dli_Cap
                    .ImpPrenotato = impegno.Dli_Costo
                    .NumPreImp = impegno.Dli_NPreImpegno
                    .UPB = impegno.Dli_UPB
                    .MissioneProgramma = impegno.Dli_MissioneProgramma
                    .ID = impegno.Dli_prog
                    .ContoEconomica = impegno.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(impegno, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If

                    End If
                    .ImpostaIrap = impegno.Di_ImpostaIrap
                    .Ratei = impegno.Di_Ratei
                    .Risconti = impegno.Di_Ratei
                    .NumImpegno = IIf(impegno.Dli_NumImpegno = "0", "", impegno.Dli_NumImpegno)
                    .NumImpPrecedente = "" & impegno.NDocPrecedente
                    If (String.IsNullOrEmpty(.NumImpPrecedente)) Then
                        .isPerente = False
                    Else
                        .isPerente = True
                    End If
                    .Stato = impegno.Di_Stato
                    .Codice_Obbiettivo_Gestionale = impegno.Codice_Obbiettivo_Gestionale
                    .PianoDeiContiFinanziario = impegno.Piano_Dei_Conti_Finanziari
                End With

                listaImpegniExtDaRestituire.Add(itemExt_Impegno)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaImpegniExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetImpegniRegistratiNonPerenti?CodiceUfficio={CodiceUfficio}")>
    Public Function GetImpegniRegistratiNonPerenti(ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaImpegniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim impegniRegistrati As System.Collections.Generic.List(Of DllDocumentale.ItemImpegnoInfo) = Get_ImpegniNonPerenti(operatore)

            Dim itemExt_Impegno As Ext_CapitoliAnnoInfo
            For Each impegno As DllDocumentale.ItemImpegnoInfo In impegniRegistrati
                itemExt_Impegno = New Ext_CapitoliAnnoInfo
                With itemExt_Impegno
                    '.AnnoPrenotazione = impegno.DBi_Anno
                    .HashTokenCallSic_Imp = impegno.HashTokenCallSic_Imp
                    .HashTokenCallSic = impegno.HashTokenCallSic
                    .Tipo = " "
                    .ID = impegno.Dli_prog
                    .Bilancio = impegno.Dli_Esercizio
                    .Capitolo = impegno.Dli_Cap
                    .ImpPrenotato = impegno.Dli_Costo
                    .NumPreImp = impegno.Dli_NPreImpegno
                    .UPB = impegno.Dli_UPB
                    .MissioneProgramma = impegno.Dli_MissioneProgramma
                    .ID = impegno.Dli_prog
                    .ContoEconomica = impegno.Di_ContoEconomica
                    ' .ContoEconomicaLista = GetArrayValoriContoEconomica(impegno, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If

                    End If
                    .ImpostaIrap = impegno.Di_ImpostaIrap
                    .Ratei = impegno.Di_Ratei
                    .Risconti = impegno.Di_Ratei
                    .NumImpegno = IIf(impegno.Dli_NumImpegno = "0", "", impegno.Dli_NumImpegno)
                    .Stato = impegno.Di_Stato
                    .Codice_Obbiettivo_Gestionale = impegno.Codice_Obbiettivo_Gestionale
                    .PianoDeiContiFinanziario = impegno.Piano_Dei_Conti_Finanziari

                    Dim extListBeneficiari As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
                    For Each beneficiario As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In impegno.ListaBeneficiari
                        Dim extBeneficiario As New Ext_AnagraficaInfo
                        extBeneficiario = extBeneficiario.TransformItemInExtObj(beneficiario)
                        extListBeneficiari.Add(extBeneficiario)
                    Next
                    .ListaBeneficiari = extListBeneficiari
                End With

                listaImpegniExtDaRestituire.Add(itemExt_Impegno)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaImpegniExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetInfoStorico?IdDoc={IdDoc}")>
    Public Function GetInfoStorico(ByVal IdDoc As String) As IList(Of Ext_DettaglioStorico)
        Dim listaItemExtDaRestituire As New List(Of Ext_DettaglioStorico)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim listaItemDettaglioStorico As System.Collections.Generic.List(Of DllDocumentale.ItemDettaglioStorico) = Get_DettaglioStorico(operatore, IdDoc)

            Dim itemExt_DettaglioStorico As Ext_DettaglioStorico
            For Each itemDettaglioStorico As DllDocumentale.ItemDettaglioStorico In listaItemDettaglioStorico
                itemExt_DettaglioStorico = New Ext_DettaglioStorico
                With itemExt_DettaglioStorico
                    .ID = itemDettaglioStorico.ID
                    .IdDocumento = itemDettaglioStorico.IdDocumento
                    .Progressivo = itemDettaglioStorico.Progressivo
                    .IdUfficio = itemDettaglioStorico.IdUfficio
                    .CodiceUfficio = itemDettaglioStorico.CodiceUfficio
                    .DescrizioneUfficio = itemDettaglioStorico.DescrizioneUfficio
                    .DenominazUfficioDaVisualizz = "<b>" & itemDettaglioStorico.CodiceUfficio & "</b> - " & itemDettaglioStorico.DescrizioneUfficio
                    .Giorni = itemDettaglioStorico.Giorni
                    .DataArrivo = itemDettaglioStorico.DataArrivo
                    .DataUscita = itemDettaglioStorico.DataUscita
                    .Utente = itemDettaglioStorico.Utente

                    If itemDettaglioStorico.Stato = "RIGETTO" Or itemDettaglioStorico.Stato = "ANNULLATO" Then
                        .Stato = "<p style='color:red; font-family:arial,tahoma,helvetica,sans-serif; font-size:11px'>" & itemDettaglioStorico.Stato & "</p>"
                        'ElseIf itemDettaglioStorico.Stato = "ANNULLATO" Then
                        '    .Stato = "<p style='color:orange; font-family:arial,tahoma,helvetica,sans-serif; font-size:11px'>" & itemDettaglioStorico.Stato & "</p>"
                    Else
                        .Stato = "<p style='color:green; font-family:arial,tahoma,helvetica,sans-serif; font-size:11px'>" & itemDettaglioStorico.Stato & "</p>"
                    End If
                End With

                listaItemExtDaRestituire.Add(itemExt_DettaglioStorico)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetPreImpegniRegistratiProvvisori?CodiceUfficio={CodiceUfficio}")>
    Public Function GetPreImpegniRegistratiProvvisori(ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaPreImpegniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim preimpegniRegistrati As System.Collections.Generic.List(Of DllDocumentale.ItemImpegnoInfo) = Get_PreImpegniProvvisori(operatore)

            Dim itemExt_Preimpegno As Ext_CapitoliAnnoInfo
            For Each preimpegno As DllDocumentale.ItemImpegnoInfo In preimpegniRegistrati
                itemExt_Preimpegno = New Ext_CapitoliAnnoInfo
                With itemExt_Preimpegno
                    .ID = preimpegno.Dli_prog
                    .NumPreImp = preimpegno.Dli_NPreImpegno
                    .Bilancio = preimpegno.Dli_Esercizio
                    .UPB = preimpegno.Dli_UPB
                    .Capitolo = preimpegno.Dli_Cap
                    .ImpPrenotato = preimpegno.Dli_Costo
                    .Stato = preimpegno.Di_Stato
                    .Codice_Obbiettivo_Gestionale = preimpegno.Codice_Obbiettivo_Gestionale
                    .PianoDeiContiFinanziario = preimpegno.Piano_Dei_Conti_Finanziari
                    .MissioneProgramma = preimpegno.Dli_MissioneProgramma
                    .TipoAssunzioneDescr = preimpegno.Di_TipoAssunzioneDescr
                    .TipoAssunzione = preimpegno.Di_TipoAssunzione
                    .DataAssunzione = preimpegno.Di_Data_Assunzione
                    .NumeroAssunzione = preimpegno.Di_Num_assunzione
                    .RegistratoSic = IIf(preimpegno.Di_TipoAssunzioneDescr = "PREIMP-PROVV", 0, 1)
                    .HashTokenCallSic = preimpegno.HashTokenCallSic
                End With

                listaPreImpegniExtDaRestituire.Add(itemExt_Preimpegno)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaPreImpegniExtDaRestituire
    End Function
    Private Function Get_ImpegniNonPerenti(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemImpegnoInfo)
        Dim listaItemImpegni As List(Of DllDocumentale.ItemImpegnoInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim idDocumento As String = CodDocumento()
            listaItemImpegni = dllDoc.FO_Get_DatiImpegniNonPerenti(idDocumento)

            For Each impegno As DllDocumentale.ItemImpegnoInfo In listaItemImpegni
                Dim beneficiariImpegni As List(Of ItemLiquidazioneImpegnoBeneficiarioInfo) = dllDoc.FO_Get_ListaBeneficiariImpegno(operatore, idDocumento, "", impegno.Dli_prog, "", "", False)
                impegno.ListaBeneficiari = beneficiariImpegni
            Next


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemImpegni
    End Function
    Private Function Get_DettaglioStorico(ByVal operatore As DllAmbiente.Operatore, ByVal idDoc As String) As List(Of DllDocumentale.ItemDettaglioStorico)
        Dim listaItemStorico As List(Of DllDocumentale.ItemDettaglioStorico)
        Try
            Log.Debug(operatore)
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemStorico = dllDoc.FO_Get_DettaglioStorico(CodDocumento())

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemStorico
    End Function
    Private Function Get_PreImpegniProvvisori(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemImpegnoInfo)
        Dim listaItemImpegni As List(Of DllDocumentale.ItemImpegnoInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemImpegni = dllDoc.FO_Get_DatiPreImpegni(CodDocumento())


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemImpegni
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetImpegniRegistratiPerenti?CodiceUfficio={CodiceUfficio}")>
    Public Function GetImpegniRegistratiPerenti(ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaImpegniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")


            Log.Debug(operatore.Codice)

            Dim impegniRegistrati As System.Collections.Generic.List(Of DllDocumentale.ItemImpegnoInfo) = Get_ImpegniPerenti(operatore)

            Dim itemExt_Impegno As Ext_CapitoliAnnoInfo
            For Each impegno As DllDocumentale.ItemImpegnoInfo In impegniRegistrati
                itemExt_Impegno = New Ext_CapitoliAnnoInfo
                With itemExt_Impegno
                    .HashTokenCallSic_Imp = impegno.HashTokenCallSic_Imp
                    .IdDocContabileSic_Imp = impegno.IdDocContabileSic_Imp
                    '.AnnoPrenotazione = impegno.DBi_Anno
                    .Tipo = " "
                    .ID = impegno.Dli_prog
                    .Bilancio = impegno.Dli_Esercizio
                    .Capitolo = impegno.Dli_Cap
                    .ImpPrenotato = impegno.Dli_Costo
                    .NumPreImp = impegno.Dli_NPreImpegno
                    .UPB = impegno.Dli_UPB
                    .MissioneProgramma = impegno.Dli_MissioneProgramma
                    .ID = impegno.Dli_prog
                    .ContoEconomica = impegno.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(impegno, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If

                    End If
                    .ImpostaIrap = impegno.Di_ImpostaIrap
                    .Ratei = impegno.Di_Ratei
                    .Risconti = impegno.Di_Ratei
                    .NumImpegno = IIf(impegno.Dli_NumImpegno = "0", "", impegno.Dli_NumImpegno)
                    .Stato = impegno.Di_Stato
                    .Codice_Obbiettivo_Gestionale = "" & impegno.Codice_Obbiettivo_Gestionale
                    .PianoDeiContiFinanziario = "" & impegno.Piano_Dei_Conti_Finanziari
                End With

                listaImpegniExtDaRestituire.Add(itemExt_Impegno)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaImpegniExtDaRestituire
    End Function
    Private Function Get_ImpegniPerenti(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemImpegnoInfo)

        Dim listaItemImpegni As List(Of DllDocumentale.ItemImpegnoInfo)
        Try

            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemImpegni = dllDoc.FO_Get_DatiImpegniPerenti(CodDocumento())


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemImpegni
    End Function
    Private Function Get_Impegni(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemImpegnoInfo)

        Dim listaItemImpegni As List(Of DllDocumentale.ItemImpegnoInfo)
        Try

            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemImpegni = dllDoc.FO_Get_DatiImpegni(CodDocumento())


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemImpegni
    End Function

    Function GetLiquidazione(ByVal operatore As DllAmbiente.Operatore, ByVal idprog As Long, Optional ByVal numPreImp As String = "") As DllDocumentale.ItemLiquidazioneInfo


        Log.Debug(operatore.Codice)

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista As List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(, idprog, numPreImp)
        If lista.Count = 1 Then
            Return lista(0)
        Else
            Throw New Exception("Liquidazione non Trovata")
        End If

    End Function
    Private Function Get_Riduzioni(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemRiduzioneInfo)

        Dim listaItemRiduzioni As List(Of DllDocumentale.ItemRiduzioneInfo)
        Try

            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            listaItemRiduzioni = dllDoc.FO_Get_DatiImpegniVariazioni(CodDocumento())

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemRiduzioni
    End Function
    Private Function Get_RiduzioniPreImpegni(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemRiduzioneInfo)

        Dim listaItemRiduzioni As List(Of DllDocumentale.ItemRiduzioneInfo)
        Try

            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            listaItemRiduzioni = dllDoc.FO_Get_DatiPreImpegniVariazioni(CodDocumento())
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemRiduzioni
    End Function
    Private Function Get_RiduzioniLiquidazioni(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemRiduzioneLiqInfo)

        Dim listaItemRiduzioni As List(Of DllDocumentale.ItemRiduzioneLiqInfo)
        Try

            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            listaItemRiduzioni = dllDoc.FO_Get_DatiLiquidazioniVariazioni(CodDocumento())
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemRiduzioni
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetLiquidazioniRegistrate?CodiceUfficio={CodiceUfficio}")>
    Public Function GetLiquidazioniRegistrate(ByVal CodiceUfficio As String) As IList(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")


            Log.Debug(operatore.Codice)

            Dim liquidazioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = Get_Liquidazioni(operatore)
            Dim listaLiquidazioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)

            Dim itemExt_Liquidazione As Ext_LiquidazioneInfo
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In liquidazioniRegistrate
                itemExt_Liquidazione = New Ext_LiquidazioneInfo
                With itemExt_Liquidazione
                    .Tipo = " "
                    .NumImpegno = liquidazione.Dli_NumImpegno
                    .Bilancio = liquidazione.Dli_Esercizio
                    .Capitolo = liquidazione.Dli_Cap
                    .ImpPrenotatoLiq = liquidazione.Dli_Costo
                    .UPB = liquidazione.Dli_UPB
                    .MissioneProgramma = liquidazione.Dli_MissioneProgramma
                    .ID = liquidazione.Dli_prog
                    .ImportoIva = liquidazione.Dli_ImportoIva
                    .ContoEconomica = liquidazione.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(liquidazione, CodiceUfficio)

                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If
                    End If
                    .NLiquidazione = IIf(liquidazione.Dli_NLiquidazione = 0, "", liquidazione.Dli_NLiquidazione)
                    .NumPreImp = liquidazione.Dli_NPreImpegno
                    .Stato = liquidazione.Di_Stato
                    .IdImpegno = liquidazione.Dli_IdImpegno

                    Dim extListBeneficiari As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
                    If Not liquidazione.ListaBeneficiari Is Nothing Then
                        For Each beneficiario As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In liquidazione.ListaBeneficiari
                            Dim extBeneficiario As New Ext_AnagraficaInfo
                            extBeneficiario = extBeneficiario.TransformItemInExtObj(beneficiario)
                            extListBeneficiari.Add(extBeneficiario)
                        Next
                    End If

                    .ListaBeneficiari = extListBeneficiari
                    .PianoDeiContiFinanziario = liquidazione.Dli_PianoDeiContiFinanziari
                End With
                listaLiquidazioniExtDaRestituire.Add(itemExt_Liquidazione)
            Next
            Return listaLiquidazioniExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetLiquidazionidaPerentiRegistrateContestuali?CodiceUfficio={CodiceUfficio}")>
    Public Function GetLiquidazionidaPerentiRegistrateContestuali(ByVal CodiceUfficio As String) As IList(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")


            Log.Debug(operatore.Codice)

            Dim liquidazioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = Get_LiquidazioniDaPerentiContestuali(operatore)
            Dim listaLiquidazioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)

            Dim itemExt_Liquidazione As Ext_LiquidazioneInfo
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In liquidazioniRegistrate
                itemExt_Liquidazione = New Ext_LiquidazioneInfo
                With itemExt_Liquidazione
                    .Tipo = " "
                    .NumImpegno = liquidazione.Dli_NumImpegno
                    .Bilancio = liquidazione.Dli_Esercizio
                    .Capitolo = liquidazione.Dli_Cap
                    .ImpPrenotatoLiq = liquidazione.Dli_Costo
                    .UPB = liquidazione.Dli_UPB
                    .MissioneProgramma = liquidazione.Dli_MissioneProgramma
                    .ID = liquidazione.Dli_prog
                    .ImportoIva = liquidazione.Dli_ImportoIva
                    .ContoEconomica = liquidazione.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(liquidazione, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If
                    End If
                    .NLiquidazione = IIf(liquidazione.Dli_NLiquidazione = 0, "", liquidazione.Dli_NLiquidazione)
                    .NumPreImp = liquidazione.Dli_NPreImpegno
                    .Stato = liquidazione.Di_Stato
                    .IdImpegno = liquidazione.Dli_IdImpegno
                    .PianoDeiContiFinanziario = liquidazione.Dli_PianoDeiContiFinanziari
                End With
                listaLiquidazioniExtDaRestituire.Add(itemExt_Liquidazione)
            Next
            Return listaLiquidazioniExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetLiquidazioniRegistrateContestuali?CodiceUfficio={CodiceUfficio}")>
    Public Function GetLiquidazioniRegistrateContestuali(ByVal CodiceUfficio As String) As IList(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim liquidazioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = Get_LiquidazioniContestuali(operatore)
            Dim listaLiquidazioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)

            Dim itemExt_Liquidazione As Ext_LiquidazioneInfo
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In liquidazioniRegistrate
                itemExt_Liquidazione = New Ext_LiquidazioneInfo
                With itemExt_Liquidazione
                    .Tipo = " "
                    .NumImpegno = liquidazione.Dli_NumImpegno
                    .Bilancio = liquidazione.Dli_Esercizio
                    .Capitolo = liquidazione.Dli_Cap
                    .ImpPrenotatoLiq = liquidazione.Dli_Costo
                    .UPB = liquidazione.Dli_UPB
                    .MissioneProgramma = liquidazione.Dli_MissioneProgramma
                    .ID = liquidazione.Dli_prog
                    .ImportoIva = liquidazione.Dli_ImportoIva
                    .ContoEconomica = liquidazione.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(liquidazione, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If
                    End If
                    .NLiquidazione = IIf(liquidazione.Dli_NLiquidazione = 0, "", liquidazione.Dli_NLiquidazione)
                    .NumPreImp = liquidazione.Dli_NPreImpegno
                End With
                listaLiquidazioniExtDaRestituire.Add(itemExt_Liquidazione)
            Next
            Return listaLiquidazioniExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetLiquidazioniRegistrateContestualiNonPerenti?CodiceUfficio={CodiceUfficio}")>
    Public Function GetLiquidazioniRegistrateContestualiNonPerenti(ByVal CodiceUfficio As String) As IList(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim liquidazioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = Get_LiquidazioniContestualiNonPerenti(operatore)
            Dim listaLiquidazioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)

            Dim itemExt_Liquidazione As Ext_LiquidazioneInfo
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In liquidazioniRegistrate
                itemExt_Liquidazione = New Ext_LiquidazioneInfo
                With itemExt_Liquidazione
                    .HashTokenCallSic = liquidazione.HashTokenCallSic
                    .IdDocContabileSic = liquidazione.IdDocContabileSic
                    .Tipo = " "
                    .NumImpegno = liquidazione.Dli_NumImpegno
                    .Bilancio = liquidazione.Dli_Esercizio
                    .Capitolo = liquidazione.Dli_Cap
                    .ImpPrenotatoLiq = liquidazione.Dli_Costo
                    .UPB = liquidazione.Dli_UPB
                    .MissioneProgramma = liquidazione.Dli_MissioneProgramma
                    .ID = liquidazione.Dli_prog
                    .ImportoIva = liquidazione.Dli_ImportoIva
                    .ContoEconomica = liquidazione.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(liquidazione, CodiceUfficio)
                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If
                    End If
                    .NLiquidazione = IIf(liquidazione.Dli_NLiquidazione = 0, "", liquidazione.Dli_NLiquidazione)
                    .NumPreImp = liquidazione.Dli_NPreImpegno
                    .Stato = liquidazione.Di_Stato
                    .IdImpegno = liquidazione.Dli_IdImpegno
                    .PianoDeiContiFinanziario = liquidazione.Dli_PianoDeiContiFinanziari
                End With
                listaLiquidazioniExtDaRestituire.Add(itemExt_Liquidazione)
            Next
            Return listaLiquidazioniExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    Private Function Get_Liquidazioni(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Dim listaItemLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemLiquidazione = dllDoc.FO_Get_DatiLiquidazioneConNumImp(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemLiquidazione
    End Function
    Public Function Get_LiquidazioniTutte(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Dim listaItemLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemLiquidazione = dllDoc.FO_Get_DatiLiquidazione(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
        Return listaItemLiquidazione
    End Function
    Private Function Get_LiquidazioniContestuali(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Dim listaItemLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemLiquidazione = dllDoc.FO_Get_DatiLiquidazioneConNumPreimp(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemLiquidazione
    End Function
    Private Function Get_LiquidazioniContestualiNonPerenti(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Dim listaItemLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemLiquidazione = dllDoc.FO_Get_DatiLiquidazioneConNumPreimpNonPerenti(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemLiquidazione
    End Function
    Private Function Get_LiquidazioniDaPerentiContestuali(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Dim listaItemLiquidazione As List(Of DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemLiquidazione = dllDoc.FO_Get_DatiLiquidazioneDaPerentiConNumPreimp(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemLiquidazione
    End Function
    Private Sub Elimina_BilancioConPreImpegno(ByVal operatore As DllAmbiente.Operatore, ByRef itemImpegno As DllDocumentale.ItemImpegnoInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_BilDaPreimpegno(itemImpegno)


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Private Sub Elimina_PreimpegnoByID(ByVal operatore As DllAmbiente.Operatore, ByRef itemImpegno As DllDocumentale.ItemImpegnoInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_PreimpegnoByDocID(itemImpegno)


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Private Sub Elimina_ImpegnoByID(ByVal operatore As DllAmbiente.Operatore, ByRef itemImpegno As DllDocumentale.ItemImpegnoInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_ImpegnoByDocID(itemImpegno)


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Private Sub Elimina_Liquidazione(ByVal operatore As DllAmbiente.Operatore, ByRef itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_LiqDaimpegno(itemLiquidazione)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Elimina_LiquidazioneConImporto(ByVal operatore As DllAmbiente.Operatore, ByRef itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_LiqConImporto(itemLiquidazione)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Elimina_LiquidazioneConId(ByVal operatore As DllAmbiente.Operatore, ByRef itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_Liq(itemLiquidazione)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Sub Elimina_Riduzione(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_Impegno_var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Private Sub Elimina_RiduzionePreImp(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_PreImpegno_var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub    'LU aggiunta  gestione transazione 23/04/09
    Private Sub Elimina_RiduzioneLiq(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneLiqInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_Liquidazione_var(itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Private Sub Elimina_Fatture_Impegno(ByVal operatore As DllAmbiente.Operatore, ByRef itemFattura As DllDocumentale.ItemFatturaInfoHeader, ByRef impegno As String)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esito As Integer = dllDoc.FO_Delete_Impegno_Fattura(itemFattura.IdDocumento, impegno)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Function Registra_RagImpegno(ByVal operatore As DllAmbiente.Operatore, ByRef itemImpegno As DllDocumentale.ItemImpegnoInfo) As Boolean
        Try

            Log.Debug(operatore.Codice)
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(itemImpegno.Dli_Documento), DllDocumentale.Model.DocumentoInfo)
            Dim result As Boolean = False
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim listaImpegno As List(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_Get_DatiImpegni(itemImpegno.Dli_Documento, itemImpegno.Dli_prog)
            Dim impegno As DllDocumentale.ItemImpegnoInfo = Nothing
            If Not listaImpegno Is Nothing And listaImpegno.Count = 1 Then
                impegno = listaImpegno.Item(0)
            End If

            Dim itemLiq As New DllDocumentale.ItemLiquidazioneInfo

            itemLiq.Dli_NumImpegno = itemImpegno.Dli_NumImpegno
            itemLiq.Dli_NPreImpegno = itemImpegno.Dli_NPreImpegno
            itemLiq.Dli_Documento = itemImpegno.Dli_Documento
            itemLiq.Dli_IdImpegno = itemImpegno.Dli_prog
            If Not impegno Is Nothing AndAlso impegno.Di_PreImpDaPrenotazione Then


                Dim cap As Ext_CapitoliAnnoInfo = GetImpDispPreImpegno(itemLiq.Dli_NPreImpegno, objDocumento.Doc_Cod_Uff_Pubblico)
                Select Case UCase(cap.TipoAtto)
                    Case "DETERMINA"
                        itemLiq.Dli_TipoAssunzione = 0
                    Case "DELIBERA"
                        itemLiq.Dli_TipoAssunzione = 1
                End Select
                itemLiq.Dli_Data_Assunzione = cap.DataAtto
                itemLiq.Dli_Num_assunzione = cap.NumeroAtto

            Else

                itemLiq.Dli_Data_Assunzione = objDocumento.Doc_Data
                itemLiq.Dli_Num_assunzione = Right(objDocumento.Doc_numero, 5)
                itemLiq.Dli_TipoAssunzione = objDocumento.Doc_Tipo

            End If


            'aggiorno il numero di impegno sulle liquidazione collegate
            result = (dllDoc.FO_Update_Bilancio_E_Liq_NumImpegnoEDatiAssunzione(itemImpegno, itemLiq))
            Return result
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Private Sub Registra_RagPreimpegno(ByVal operatore As DllAmbiente.Operatore, ByVal numPreimpegno As String, ByVal preimpegno As DllDocumentale.ItemImpegnoInfo)
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim lista_preimpegni As List(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_Get_DatiPreImpegni(, preimpegno.Dli_prog)
            ''chiamata ad insert_item_bilancio
            Dim itemRagPreimpegno As DllDocumentale.ItemImpegnoInfo = lista_preimpegni.Item(0)

            With itemRagPreimpegno
                .HashTokenCallSic = preimpegno.HashTokenCallSic
                .IdDocContabileSic = preimpegno.IdDocContabileSic
                .Dli_NPreImpegno = preimpegno.Dli_NPreImpegno
                .Di_TipoAssunzioneDescr = preimpegno.Di_TipoAssunzioneDescr
                .Di_TipoAssunzione = preimpegno.Di_TipoAssunzione
                .Di_Data_Assunzione = preimpegno.Di_Data_Assunzione
                .Di_Num_assunzione = preimpegno.Di_Num_assunzione
                .Dli_DataRegistrazione = Now()
                .Dli_Operatore = operatore.Codice
            End With

            dllDoc.FO_Update_Preimpegno(itemRagPreimpegno)
        Catch ex As Exception
            Log.Error(operatore.Codice & " - " & ex.Message)
            Throw ex
        End Try
    End Sub
    Private Sub Registra_RagLiquidazione(ByVal operatore As DllAmbiente.Operatore, ByVal numLiquidazione As String, ByVal importoIva As Double, ByVal progId As Long, ByVal idDocContabileSic As String, Optional ByVal pcf As String = "")
        Try
            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim lista_liq As List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(, progId)
            ''chiamata ad insert_item_bilancio
            Dim itemRagLiquidazione As DllDocumentale.ItemLiquidazioneInfo = lista_liq.Item(0)

            With itemRagLiquidazione
                .Dli_Operatore = operatore.Codice
                .Dli_PianoDeiContiFinanziari = pcf
                .Dli_NLiquidazione = numLiquidazione
                .Dli_ImportoIva = importoIva
                .IdDocContabileSic = idDocContabileSic 
            End With

            dllDoc.FO_Update_Liquidazione(itemRagLiquidazione)
        Catch ex As Exception
            Log.Error(operatore.Codice & " - " & ex.Message)
            Throw ex
        End Try



    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneImpegno")>
    Public Sub GenerazioneImpegno()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim objDllDocumentale As DllDocumentale.svrDocumenti = (New DllDocumentale.svrDocumenti(operatore))
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = objDllDocumentale.Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            Dim dipartimento As String = objDocumento.Doc_Cod_Uff_Pubblico
            Dim datamovimento As String = HttpContext.Current.Request.Item("DataMovimento")


            Dim impegniCapitoli As String = HttpContext.Current.Request.Item("ImpegniRagioneria")
            impegniCapitoli = "[" & impegniCapitoli & "]"
            Dim listaImpegni As List(Of Ext_CapitoliAnnoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(impegniCapitoli, GetType(List(Of Ext_CapitoliAnnoInfo))), List(Of Ext_CapitoliAnnoInfo))

            'valori dela richiesta
            Dim importo As String = listaImpegni.Item(0).ImpPrenotato
            Dim contoEconomico As String = listaImpegni.Item(0).ContoEconomica
            Dim ratei As String = listaImpegni.Item(0).Ratei
            Dim importoIrap As String = listaImpegni.Item(0).ImpostaIrap
            Dim risconti As String = listaImpegni.Item(0).Risconti
            Dim capitolo As String = listaImpegni.Item(0).Capitolo
            Dim upb As String = listaImpegni.Item(0).UPB
            Dim missioneProgramma As String = listaImpegni.Item(0).MissioneProgramma
            Dim esercizio As String = listaImpegni.Item(0).Bilancio
            Dim isPerente As Boolean = listaImpegni.Item(0).isPerente
            Dim numImpPrecedente As String = ""
            Dim Codice_Obbiettivo_Gestionale As String = listaImpegni.Item(0).Codice_Obbiettivo_Gestionale
            Dim pcf As String = listaImpegni.Item(0).PianoDeiContiFinanziario


            Dim hashTokenCallSic As String = listaImpegni.Item(0).HashTokenCallSic
            Dim idDocContabileSic As String = listaImpegni.Item(0).IdDocContabileSic
            'Sto registrando gli impegni definitivi in ragioneria, devo inviare al SIC il token "nuovo",
            ' preparato già nell'ufficio proponente alla generazione del preimpegno "fittizio" - ovvero quando ho salvato il record nel db
            Dim hashTokenCallSic_Imp As String = listaImpegni.Item(0).HashTokenCallSic_Imp
            Dim idDocContabileSic_Imp As String = listaImpegni.Item(0).IdDocContabileSic_Imp

            If isPerente Then
                numImpPrecedente = listaImpegni.Item(0).NumImpPrecedente
                If String.IsNullOrEmpty(numImpPrecedente) Then
                    Throw New Exception("E' necessario specificare il numero dell\'impegno caduto in perenzione")
                End If
            End If

            If Not String.IsNullOrEmpty(listaImpegni.Item(0).NumImpegno) AndAlso (listaImpegni.Item(0).NumImpegno <> 0) Then
                Throw New Exception("Impegno già registrato")
            End If



            Dim NumPreImpegno As String
            NumPreImpegno = listaImpegni.Item(0).NumPreImp

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            ElseIf UCase(tipoAtto) = "DELIBERE" Then
                tipoAtto = "DELIBERA"
            Else
                tipoAtto = Nothing
            End If

            Dim oggetto As String = objDocumento.Doc_Oggetto

            Dim listaBenReturn As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = objDllDocumentale.FO_Get_ListaBeneficiariImpegno(operatore,,, listaImpegni.Item(0).ID)
            Dim arrayBen As Array = TrasformaBeneficiariInterniInSIC(listaBenReturn)


            Dim rispostaGenerazioneImpegno As String()
            Dim numeroAtto As String
            If tipoAtto = "DELIBERA" And objDocumento.Doc_numero = "" Then
                'se il tipo di atto con cui sto creando l'impegno in ragioneria è una delibera
                ' che non ha ancora il numero definitivo, devo passare al SIC il numero provvisorio
                ' il TipoDocumento: IMP-DEF e il TipoAtto: IMP-PROVV.
                ' nel momento in cui si ha il numero def della delibera (dopo l'inoltro da
                ' parte del Segreterario di Presidenza si dovrà richiamare il SIC 
                ' con il TipoDocumento: IMP-PROVV e il TipoAtto: DELIBERA per aggiornare il dati dell'atto
                ' (il numero definitivo) con il quale è stato generato questo impegno
                tipoAtto = "IMP-PROVV"
                numeroAtto = Right(objDocumento.Doc_numeroProvvisorio, 5)
            Else
                numeroAtto = Right(objDocumento.Doc_numero, 5)
            End If
            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If
            
            If String.IsNullOrEmpty(numImpPrecedente) Then
                rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoMessage(operatore, NumPreImpegno, importo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), dipartimento, contoEconomico, ratei, importoIrap, risconti, datamovimento, Codice_Obbiettivo_Gestionale, pcf, oggetto, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic_Imp, arrayBen)
                ''cancellare la prenotazione fatta sul preimpegno da delibera
                EliminaPrenotazione(CodDocumento, listaImpegni.Item(0).ID)
            Else
                rispostaGenerazioneImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazioneImpegnoPerenteMessage(operatore, objDocumento.Doc_Oggetto, importo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), dipartimento, contoEconomico, ratei, importoIrap, risconti, numImpPrecedente, capitolo, datamovimento, Codice_Obbiettivo_Gestionale, pcf, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic_Imp, arrayBen)
            End If

            Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

            Integer.TryParse(rispostaGenerazioneImpegno(0), NumPreImpegno1)
            Integer.TryParse(rispostaGenerazioneImpegno(1), NumPreImpegno2)
            Integer.TryParse(rispostaGenerazioneImpegno(2), NumPreImpegno3)
            Integer.TryParse(rispostaGenerazioneImpegno(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazioneImpegno(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazioneImpegno(5), idDocSIC3)
            Dim NumImpegno As Long = 0
            If IsNumeric(NumPreImpegno1) Then
                Long.TryParse(NumPreImpegno1, NumImpegno)
                HttpContext.Current.Response.Write("{  success: true, NumImp: '" + NumImpegno.ToString() + "' }")
            End If

            ''chiamata ad insert_item_bilancio
            Dim itemRagAssunzione As New DllDocumentale.ItemImpegnoInfo
            With itemRagAssunzione


                
                'tutti i dati già registrati sul record, dall'ufficio proponente, 
                ' devo solo registrare l'idDocSIC1 che mi è stato appena restituito dalla chiamata
                .HashTokenCallSic = hashTokenCallSic
                .IdDocContabileSic = idDocContabileSic
                .IdDocContabileSic_Imp = idDocSIC1
                .HashTokenCallSic_Imp = hashTokenCallSic_Imp

                .Dli_Cap = capitolo
                .Dli_Costo = importo
                .Dli_DataRegistrazione = Now
                .Dli_Documento = objDocumento.Doc_id
                .Dli_Esercizio = esercizio
                .DBi_Anno = esercizio
                .Dli_NumImpegno = NumImpegno
                .Dli_Operatore = operatore.Codice
                .Dli_UPB = upb
                .Dli_MissioneProgramma = missioneProgramma
                .Di_ContoEconomica = contoEconomico
                .Di_ImpostaIrap = importoIrap
                .Di_Ratei = ratei
                .Di_Risconti = risconti
                .Dli_NPreImpegno = listaImpegni.Item(0).NumPreImp
                .Dli_prog = listaImpegni.Item(0).ID
                .Dli_Documento = objDocumento.Doc_id
                .NDocPrecedente = numImpPrecedente
                .Piano_Dei_Conti_Finanziari = pcf
            End With
            If Not Registra_RagImpegno(operatore, itemRagAssunzione) Then
                Throw New Exception("Impossibile registrare in provvedimenti impegno " & NumImpegno & " per item bilandio id:" & itemRagAssunzione.Dli_prog)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Dim messaggio As String = Replace(ex.Message, "'", "\'")
            messaggio = Replace(messaggio, ControlChars.Quote, "")
            messaggio = Replace(messaggio, ",", "")
            'HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & messaggio & "' }")
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), messaggio)
        End Try
    End Sub
    Private Sub EliminaPrenotazione(ByVal key As String, Optional ByVal progressivoImpegno As String = "")
        Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Log.Debug(oOperatore.Codice)


        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        If String.IsNullOrEmpty(progressivoImpegno) Then
            listaImpegni = dllDoc.FO_Get_DatiImpegni(key)
        Else 
            listaImpegni = dllDoc.FO_Get_DatiImpegni(key, progressivoImpegno)
        End If
        
        Dim flagResult As Boolean = False
        For Each item As DllDocumentale.ItemImpegnoInfo In listaImpegni
            Try
                If item.Di_PreImpDaPrenotazione = 1 Then
                    flagResult = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(oOperatore, "C", item.Dli_NPreImpegno, item.Dli_Costo)
                End If
            Catch ex As Exception
                Log.Error(ex.ToString)
                Throw New Exception(ex.Message)
            End Try
        Next
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneLiquidazione")>
    Public Sub GenerazioneLiquidazione(ByVal LiquidazioniRagioneria As String, ByVal DataMovimento As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim objDllDocumentale As DllDocumentale.svrDocumenti = (New DllDocumentale.svrDocumenti(operatore))
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = objDllDocumentale.Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            Dim struttura As String = objDocumento.Doc_Cod_Uff_Pubblico

            Dim liquidazioniCapitoli As String = LiquidazioniRagioneria
            liquidazioniCapitoli = "[" & liquidazioniCapitoli & "]"
            Dim listaLiquidazioni As List(Of Ext_LiquidazioneInfo) = DirectCast(JavaScriptConvert.DeserializeObject(liquidazioniCapitoli, GetType(List(Of Ext_LiquidazioneInfo))), List(Of Ext_LiquidazioneInfo))

            If (Not String.IsNullOrEmpty(listaLiquidazioni.Item(0).NLiquidazione)) AndAlso (listaLiquidazioni.Item(0).NLiquidazione <> 0) Then
                Throw New Exception("Liquidazione già registrata")
            End If

            'valori dela richiesta
            Dim importo As String = listaLiquidazioni.Item(0).ImpPrenotatoLiq
            Dim contoEconomico As String = listaLiquidazioni.Item(0).ContoEconomica

            Dim importoIva As String = listaLiquidazioni.Item(0).ImportoIva
            Dim capitolo As String = listaLiquidazioni.Item(0).Capitolo
            Dim bilancio As String = listaLiquidazioni.Item(0).Bilancio
            Dim annoPrenotazione As String = Left(objDocumento.Doc_id, 4)
            Dim upb As String = listaLiquidazioni.Item(0).UPB
            Dim pcf As String = listaLiquidazioni.Item(0).PianoDeiContiFinanziario
            Dim hashTokenCallSic As String = listaLiquidazioni.Item(0).HashTokenCallSic

            Dim NumImpegno As Long = 0
            Long.TryParse(listaLiquidazioni.Item(0).NumImpegno, NumImpegno)

            If NumImpegno = 0 Then
                Dim lstr_NumImp As String = ""
                lstr_NumImp = GetNumeroImpegno(operatore, listaLiquidazioni.Item(0).NumPreImp, objDocumento.Doc_id)
                Long.TryParse(lstr_NumImp, NumImpegno)
                If NumImpegno = 0 Then
                    Throw New Exception("Non esistono Impegni per questa Liquidazione")
                End If
            End If

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            Else
                tipoAtto = Nothing
            End If

            Dim oggetto As String = objDocumento.Doc_Oggetto

            Dim flagDisabilitaArchivioLiquidazione As Boolean
            flagDisabilitaArchivioLiquidazione = IIf(ConfigurationManager.AppSettings("DISABILITA_ARCHIVIO_LIQUIDAZIONI") = "1", True, False)
            Dim listaBenReturn As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
            Dim arrayBen As Array
            If flagDisabilitaArchivioLiquidazione Then
                listaBenReturn = objDllDocumentale.FO_Get_ListaBeneficiariLiquidazione(operatore, , , listaLiquidazioni.Item(0).ID, )
                arrayBen = TrasformaBeneficiariInterniInSIC(listaBenReturn)
            Else
                listaBenReturn = Nothing
            End If


            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If
            Dim rispostaGenerazioneLiquidazione As String() = ClientIntegrazioneSic.MessageMaker.createGenerazioneLiquidazioneMessage(operatore, NumImpegno, importo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), struttura, contoEconomico, importoIva, DataMovimento, pcf, oggetto, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic, arrayBen)

            Dim NumLiq1 As Integer, NumLiq2 As Integer, NumLiq3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

            Integer.TryParse(rispostaGenerazioneLiquidazione(0), NumLiq1)
            Integer.TryParse(rispostaGenerazioneLiquidazione(1), NumLiq2)
            Integer.TryParse(rispostaGenerazioneLiquidazione(2), NumLiq3)
            Integer.TryParse(rispostaGenerazioneLiquidazione(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazioneLiquidazione(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazioneLiquidazione(5), idDocSIC3)

            Dim NumLiquidazione As Long = 0
            If IsNumeric(NumLiq1) Then
                Long.TryParse(NumLiq1, NumLiquidazione)
                HttpContext.Current.Response.Write("{  success: true, NumLiq: '" + NumLiq1.ToString() + "' }")
            End If

            Registra_RagLiquidazione(operatore, NumLiquidazione, importoIva, listaLiquidazioni.Item(0).ID, idDocSIC1,pcf)

            'REGISTRA FATTURA SUL SIC CON NumLiquidazione
            Dim P_ESITO As Double = 0
            Dim listaFattureLiquidazioneEXT As List(Of Ext_FatturaInfo) = GetListaFattByLiquidazione(listaLiquidazioni.Item(0).ID)

            For Each fattura As Ext_FatturaInfo In listaFattureLiquidazioneEXT
                Dim messaggioSic As String = ClientIntegrazioneSic.MessageMaker.notificaAttoFatturaSIC(operatore, objDocumento.Doc_numero,
                                                                                         objDocumento.Doc_numeroProvvisorio, Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString,
                                                                                         NumLiquidazione, 0, 0, fattura.IdUnivoco,
                                                                                         "I", DataMovimento, fattura.ImportoLiquidato, P_ESITO)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzione")>
    Public Sub GenerazioneRiduzione()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If
            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            Dim dipartimento As String = objDocumento.Doc_Cod_Uff_Pubblico

            Dim riduzioniRagioneria As String = HttpContext.Current.Request.Item("RiduzioniRagioneria")
            riduzioniRagioneria = "[" & riduzioniRagioneria & "]"
            Dim riduzioni As List(Of Ext_CapitoliAnnoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(riduzioniRagioneria, GetType(List(Of Ext_CapitoliAnnoInfo))), List(Of Ext_CapitoliAnnoInfo))
            Dim datamovimento As String = HttpContext.Current.Request.Item("DataMovimentoRid")

            'valori dela richiesta
            Dim hashTokenCallSic As String = riduzioni.Item(0).HashTokenCallSic
            Dim importo As String = riduzioni.Item(0).ImpPrenotato
            Dim capitolo As String = riduzioni.Item(0).Capitolo
            Dim upb As String = riduzioni.Item(0).UPB
            Dim missioneProgramma As String = riduzioni.Item(0).MissioneProgramma
            Dim esercizio As String = riduzioni.Item(0).Bilancio
            Dim isEconomia As Integer = 0
            'Lu Rid
            Integer.TryParse(riduzioni.Item(0).IsEconomia, isEconomia)

            Dim id As Long
            Long.TryParse(riduzioni.Item(0).ID, id)

            If String.IsNullOrEmpty(riduzioni.Item(0).NumImpegno) AndAlso (riduzioni.Item(0).NumImpegno = "0") Then
                Throw New Exception("Specificare il numero impegno da ridurre")
            End If

            Dim dataAttoAss As String = riduzioni.Item(0).DataAtto
            Dim tipoAttoAss As String = riduzioni.Item(0).TipoAtto
            Dim numAttoAss As String = riduzioni.Item(0).NumeroAtto


            Dim NumImp As Integer
            NumImp = CInt(riduzioni.Item(0).NumImpegno)

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            Else
                tipoAtto = Nothing
            End If

            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            'Lu Rid inviare un importo Negativo se isEconomia = 0
            Dim rispostaGenerazioneRiduzione As String()
            If isEconomia Then
                rispostaGenerazioneRiduzione = ClientIntegrazioneSic.MessageMaker.createGenerazioneRiduzioneMessage(operatore, NumImp, importo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), datamovimento, dipartimento, objDocumento.Doc_Oggetto, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)
            Else
                Dim importoDouble As Double = 0
                importoDouble = -Double.Parse(importo)
                rispostaGenerazioneRiduzione = ClientIntegrazioneSic.MessageMaker.createGenerazioneRiduzioneMessage(operatore, NumImp, importoDouble, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), datamovimento, dipartimento, objDocumento.Doc_Oggetto, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)

            End If

            Dim NumDoc1 As Integer, NumDoc2 As Integer, NumDoc3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer
				
				
            Integer.TryParse(rispostaGenerazioneRiduzione(0), NumDoc1)
            Integer.TryParse(rispostaGenerazioneRiduzione(1), NumDoc2)
            Integer.TryParse(rispostaGenerazioneRiduzione(2), NumDoc3)
            Integer.TryParse(rispostaGenerazioneRiduzione(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazioneRiduzione(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazioneRiduzione(5), idDocSIC3)

            Dim NumRegistrazione As Long = 0
            If IsNumeric(rispostaGenerazioneRiduzione(0)) Then
                Long.TryParse(rispostaGenerazioneRiduzione(0), NumRegistrazione)
                HttpContext.Current.Response.Write("{  success: true }")
            End If

            ''chiamata ad insert_item_bilancio
            Dim itemRagRiduzione As New DllDocumentale.ItemRiduzioneInfo
            With itemRagRiduzione
                .HashTokenCallSic = hashTokenCallSic
                .IdDocContabileSic = idDocSIC1
                .DBi_Anno = esercizio
                .Dli_Cap = capitolo
                .Dli_Costo = importo
                .Dli_DataRegistrazione = datamovimento
                .Dli_Documento = objDocumento.Doc_id
                .Dli_Esercizio = esercizio
                .Dli_NumImpegno = NumImp
                .Dli_Operatore = operatore.Codice
                .Dli_UPB = upb
                .Dli_MissioneProgramma = missioneProgramma
                .Dli_Documento = objDocumento.Doc_id
                .Div_NumeroReg = NumRegistrazione
                'Lu Rid

                .Div_TipoAssunzione = IIf(tipoAttoAss = "DETERMINA", 0, 1)
                .Div_Num_assunzione = numAttoAss
                .Div_Data_Assunzione = dataAttoAss
                .Div_IsEconomia = isEconomia
                .Dli_prog = id
            End With

            Aggiorna_Riduzione(operatore, itemRagRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Dim errorMessage As String = Replace(Replace(Replace(ex.Message, "'", "\'"), """", ""), ",", "")
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & errorMessage & "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    Function GetNumeroImpegno(ByVal operatore As DllAmbiente.Operatore, ByVal NPreImp As String, ByVal idDoc As String) As String

        Log.Debug(operatore.Codice)

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim listaImp As List(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_Get_DatiImpegniByNPreImp(NPreImp, idDoc)
        For Each Item As DllDocumentale.ItemImpegnoInfo In listaImp
            If Item.Dli_NPreImpegno = NPreImp And Item.Dli_NumImpegno Then
                Return Item.Dli_NumImpegno
            End If

        Next
        Return ""
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaImpAperti?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}&CodiceUfficio={CodiceUfficio}")>
    Public Function GetListaImpAperti(ByVal AnnoRif As String, ByVal CapitoloRif As String, ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)

        Try
            Dim impegni As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneImpegni As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneImpegniApertiMessage(operatore, AnnoRif, CapitoloRif, CodiceUfficio)
            '-1
            For i As Integer = 0 To UBound(rispostaInterrogazioneImpegni, 1)
                Dim impRestituito As New Ext_CapitoliAnnoInfo
                impRestituito.NumPreImp = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroImpegno
                impRestituito.ImpDisp = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).ImportoDisponibile
                impRestituito.Oggetto_Impegno = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).OggettoImpegno
                impRestituito.Codice_Obbiettivo_Gestionale = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).COG_Semplice

                'verifica il potenziale importo, cioè tenendo conto di eventuali riduzioni sul preimpegno (non registrate sul sic) si atti in itinere
                impRestituito.ImpPotenzialePrenotato = calcolaResiduaDisponibilitaDelPreimpegno(operatore, impRestituito.NumPreImp, impRestituito.ImpDisp)
                impegni.Add(impRestituito)
            Next
            Return impegni
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaPreimp?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetListaPreimp(ByVal AnnoRif As String, ByVal CapitoloRif As String) As IList(Of Ext_CapitoliAnnoInfo)

        Try
            Return getListPreimpByCapAnno(AnnoRif, CapitoloRif)
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    Public Shared Function getListPreimpByCapAnno(AnnoRif As String, CapitoloRif As String) As IList(Of Ext_CapitoliAnnoInfo)

        Dim preimpegni As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        Log.Debug(operatore.Codice)

        Dim rispostaInterrogazionePreImpegni As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazionePreImpegniApertiMessage(operatore, AnnoRif, CapitoloRif)
        '-1
        For i As Integer = 0 To UBound(rispostaInterrogazionePreImpegni, 1)
            Dim PreimpRestituito As New Ext_CapitoliAnnoInfo
            PreimpRestituito.NumPreImp = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).NumeroPreimpegno
            PreimpRestituito.ImpDisp = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).ImportoDisponibile
            'vengono caricate nella lista preimpegni sono quelli effetivamente fatti da delibera, non quelli "privati" fatti dalle determine
            If UCase(DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto) = "DELIBERA" Or
               UCase(DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto) = "DELPRES" Or
               UCase(DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto) = "DETERMINA" Or
               UCase(DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto) = "RENDICONTO" Or
               UCase(DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto) = "D.G.R." Then

                PreimpRestituito.NumeroAtto = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).NumeroAtto
                PreimpRestituito.TipoAtto = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).TipoAtto
                PreimpRestituito.DataAtto = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).DataAtto
                PreimpRestituito.Oggetto_Impegno = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).OggettoPreimpegno

                'verifica il potenziale importo, cioè tenendo conto di eventuali riduzioni sul preimpegno (non registrate sul sic) si atti in itinere
                PreimpRestituito.ImpPotenzialePrenotato = residuaDisponibilitaPreimpegno(operatore, PreimpRestituito.NumPreImp, PreimpRestituito.ImpDisp)
                'PreimpRestituito.PianoDeiContiFinanziario = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).PianoDeiContiFinanziario
                PreimpRestituito.PianoDeiContiFinanziario = DirectCast(rispostaInterrogazionePreImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazionePreimpegniAperti_TypesPreimpegno).PCF.Codice
                preimpegni.Add(PreimpRestituito)
            End If
        Next
        Return preimpegni
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneLiquidazioneUP")>
    Public Sub GenerazioneLiquidazioneUP()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            'valori dela richiesta
            Dim bilancio As String = HttpContext.Current.Request.Item("Bilancio")
            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            'Dim importo As String = HttpContext.Current.Request.Item("ImpPrenotatoLiq")
            Dim upb As String = HttpContext.Current.Request.Item("UPB")
            Dim missioneProgramma As String = HttpContext.Current.Request.Item("MissioneProgramma")
            Dim NumPreImp As String = HttpContext.Current.Request.Item("NumPreImp")
            Dim dataAtto As String = HttpContext.Current.Request.Item("DataAtto")
            Dim tipoAtto As String = HttpContext.Current.Request.Item("TipoAtto")
            Dim numAtto As String = HttpContext.Current.Request.Item("NumeroAtto")
            Dim NumImpegno As Long = CLng(HttpContext.Current.Request.Item("comboImpegni"))
            Dim PianoDeiContiFinanziario As String = "" & HttpContext.Current.Request.Item("ComboPdCFImpegni")
            'Dim BeneficiarioString As String = "" & HttpContext.Current.Request.Item("ListaBeneficiariDaLiquidare")


            Dim listaBeneficiariStr As String = HttpContext.Current.Request.Item("ListaBeneficiariDaLiquidare")
            listaBeneficiariStr = "[" & listaBeneficiariStr & "]"
            Dim listaBeneficiari As List(Of Ext_AnagraficaInfo) = DirectCast(JavaScriptConvert.DeserializeObject(listaBeneficiariStr, GetType(List(Of Ext_AnagraficaInfo))), List(Of Ext_AnagraficaInfo))
            If Not listaBeneficiari Is Nothing AndAlso listaBeneficiari.Count < 1 Then
                Throw New Exception("Impossibile registrare, nessuna liquidazione presente")
            End If


            'Dim Beneficiario As Ext_AnagraficaInfo = DirectCast(JavaScriptConvert.DeserializeObject(BeneficiarioString, GetType(Ext_AnagraficaInfo)), Ext_AnagraficaInfo)
            If PianoDeiContiFinanziario.ToLower.Contains("selez") Then
                PianoDeiContiFinanziario = ""
                Throw New Exception("Piano dei Conti Finanziario non valido")
            End If
            If UCase(tipoAtto) = "DETERMINA" Then
                tipoAtto = "0"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONE" Then
                tipoAtto = "2"
            Else
                tipoAtto = "1"
            End If

            'Dim importoDisponibileimpegno As Decimal

            'Dim lstr_impDisp As String = HttpContext.Current.Request.Item("ImpDisp")
            'lstr_impDisp = Trim(lstr_impDisp.Replace("€", "").Replace(".", ""))
            'Double.TryParse(lstr_impDisp, importoDisponibileimpegno)

            'Dim importoPrenotato As Decimal
            'Double.TryParse(importo, importoPrenotato)

            'Verifico se l'importo di tutte le liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            'Dim residuo As Decimal = residuaDisponibilitaDaImpegno(operatore, NumImpegno, importoDisponibileimpegno)
            'Dim totResiduo As Decimal = residuo - importoPrenotato
            'If (totResiduo) < 0 Then
            '    Throw New Exception("Disponibilita insufficiente per la liquidazione di € " & CStr(importoPrenotato) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di €" & CStr(residuo))
            'End If
            
            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            'If Not String.IsNullOrEmpty(importo) Then
            '    itemLiquidazione = New DllDocumentale.ItemLiquidazioneInfo
            '    With itemLiquidazione
            '        .Dli_Anno = bilancio
            '        .Dli_Cap = capitolo
            '        .Dli_Costo = importo
            '        .Dli_DataRegistrazione = Now
            '        .Dli_Documento = CodDocumento()
            '        .Dli_Esercizio = bilancio
            '        .Dli_NumImpegno = CStr(NumImpegno)
            '        .Dli_Operatore = operatore.Codice
            '        .Dli_UPB = upb
            '        .Dli_MissioneProgramma = missioneProgramma
            '        .Dli_TipoAssunzione = tipoAtto
            '        .Dli_Data_Assunzione = dataAtto
            '        .Dli_Num_assunzione = numAtto
            '        .Dli_NPreImpegno = NumPreImp
            '        .Dli_PianoDeiContiFinanziari = PianoDeiContiFinanziario
            '    End With
            '    itemLiquidazione = Registra_Liquidazione(operatore, itemLiquidazione)
            'End If
            Dim messaggioErrore As String = ""
            For Each beneficiarioDaLiquidare As Ext_AnagraficaInfo In listaBeneficiari
                Dim itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo  = New DllDocumentale.ItemLiquidazioneInfo

                Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                With itemLiquidazione
                    .HashTokenCallSic = hashTokenCallSic
                    .Dli_Anno = bilancio
                    .Dli_Cap = capitolo
                    .Dli_Costo = beneficiarioDaLiquidare.ImportoDaLiquidare
                    .Dli_DataRegistrazione = Now
                    .Dli_Documento = CodDocumento()
                    .Dli_Esercizio = bilancio
                    .Dli_NumImpegno = CStr(NumImpegno)
                    .Dli_Operatore = operatore.Codice
                    .Dli_UPB = upb
                    .Dli_MissioneProgramma = missioneProgramma
                    .Dli_TipoAssunzione = tipoAtto
                    .Dli_Data_Assunzione = dataAtto
                    .Dli_Num_assunzione = numAtto
                    .Dli_NPreImpegno = NumPreImp
                    .Dli_PianoDeiContiFinanziari = PianoDeiContiFinanziario
                End With

                itemLiquidazione = Registra_Liquidazione(operatore, itemLiquidazione)

                Dim prog As String = itemLiquidazione.Dli_prog.ToString
                If  beneficiarioDaLiquidare.ID <> "" AndAlso beneficiarioDaLiquidare.ID <> "0" Then
                    Dim listaSedi As List(Of Ext_SedeAnagraficaInfo) = beneficiarioDaLiquidare.ListaSedi
                    Dim sede As New Ext_SedeAnagraficaInfo
                    Dim datiBancari As New Ext_DatiBancariInfo
                    If Not listaSedi Is Nothing AndAlso listaSedi.Count > 0 Then
                        sede = listaSedi.ElementAt(0)

                        Dim listaDatiBancari As List(Of Ext_DatiBancariInfo) = sede.DatiBancari
                        If Not listaDatiBancari Is Nothing AndAlso listaDatiBancari.Count > 0 Then
                            datiBancari = listaDatiBancari.ElementAt(0)
                        End If

                        Dim codiceCig As String = ""
                        Dim codiceCUP As String = ""
                        If Not beneficiarioDaLiquidare.Contratto Is Nothing Then
                            codiceCig = beneficiarioDaLiquidare.Contratto.CodiceCIG
                            codiceCUP = beneficiarioDaLiquidare.Contratto.CodiceCUP
                        End If

                        messaggioErrore = Registra_BeneficiarioImpegnoLiquidazione(operatore, beneficiarioDaLiquidare, sede, datiBancari, beneficiarioDaLiquidare.ImportoDaLiquidare, codiceCig, codiceCUP, "", prog, "", "")

                        If messaggioErrore <> "" AndAlso messaggioErrore.Contains("success: false") Then
                            Elimina_LiquidazioneConId(operatore, itemLiquidazione)
                            Log.Error(messaggioErrore)
                        Else
                            messaggioErrore = "{  success: true, progLiquidazione: '" + prog + "' }"
                        End If
                    End If
                End If
            Next
            'Dim prog As String = ""
            'Dim messaggioErrore As String =  ""
            'If Not itemLiquidazione Is Nothing Then
            '    prog = itemLiquidazione.Dli_prog.ToString

            '    If Not Beneficiario Is Nothing AndAlso Beneficiario.ID <> "" AndAlso Beneficiario.ID <> "0" Then
            '        Dim listaSedi As List(Of Ext_SedeAnagraficaInfo) = Beneficiario.ListaSedi
            '        Dim sede As New Ext_SedeAnagraficaInfo
            '        Dim datiBancari As New Ext_DatiBancariInfo
            '        If Not listaSedi Is Nothing AndAlso listaSedi.Count > 0 Then
            '            sede = listaSedi.ElementAt(0)

            '            Dim listaDatiBancari As List(Of Ext_DatiBancariInfo) = sede.DatiBancari
            '            If Not listaDatiBancari Is Nothing AndAlso listaDatiBancari.Count > 0 Then
            '                datiBancari = listaDatiBancari.ElementAt(0)
            '            End If

            '            Dim codiceCig As String = ""
            '            Dim codiceCUP As String = ""
            '            If Not Beneficiario.Contratto Is Nothing Then
            '                codiceCig = Beneficiario.Contratto.CodiceCIG
            '                codiceCUP = Beneficiario.Contratto.CodiceCUP
            '            End If

            '            messaggioErrore = Registra_BeneficiarioImpegnoLiquidazione(operatore, Beneficiario, sede, datiBancari, importo, codiceCig, codiceCUP, "", prog, "", "")
            '        End If
            '    End If
            'End If
         
           HttpContext.Current.Response.Write(messaggioErrore)
         

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub

    Public Function GenerazioneRiduzioneUP(ByRef bilancio As String, ByRef capitolo As String, ByRef upb As String, ByRef dataAtto As String, ByRef tipoAtto As String, ByRef numAtto As String, ByRef NumImpegno As Long, ByRef lstr_impDisp As String, ByRef importo As String, ByRef NumPreImp As String, ByRef isEconomia As String, ByRef missioneProgramma As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)
            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If
            'valori dela richiesta
            'Dim bilancio As String = HttpContext.Current.Request.Item("Bilancio")
            'Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            'Dim importo As String = HttpContext.Current.Request.Item("ImpDaRidurre")
            'Dim upb As String = HttpContext.Current.Request.Item("UPB")
            'Dim NumPreImp As String = HttpContext.Current.Request.Item("NumPreImp")
            'Dim dataAtto As String = HttpContext.Current.Request.Item("DataAtto")
            'Dim tipoAtto As String = HttpContext.Current.Request.Item("TipoAtto")
            'Dim numAtto As String = HttpContext.Current.Request.Item("NumeroAtto")
            'Dim NumImpegno As Long = CLng(HttpContext.Current.Request.Item("ComboImpRid"))
            'Dim isEconomia As Integer = 0
            'LU Rid
            'Integer.TryParse(HttpContext.Current.Request.Item("hiddenComboEconomia"), isEconomia)

            If UCase(tipoAtto) = "DETERMINA" Then
                tipoAtto = "0"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONE" Then
                tipoAtto = "2"
            Else
                tipoAtto = "1"
            End If

            Dim importoDisponibileimpegno As Decimal
            'Dim lstr_impDisp As String = HttpContext.Current.Request.Item("ImpDisp")
            lstr_impDisp = Trim(lstr_impDisp.Replace("€", "").Replace(".", ""))
            Decimal.TryParse(lstr_impDisp, importoDisponibileimpegno)

            Dim importoDaRidurre As Decimal
            importo = importo.Replace(".", ",")
            Decimal.TryParse(importo, importoDaRidurre)

            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaDaImpegno(operatore, NumImpegno, importoDisponibileimpegno)
            Dim totResiduo As Decimal = residuo - importoDaRidurre
            If (totResiduo) < 0 Then
                Throw New Exception("la disponibilità effettiva potrebbe essere bloccata, è possibile effettuare riduzioni al massimo di € " & CStr(residuo))
            End If

            ' Controllo eliminato a seguito della modifica effettuata da Lobefaro sul SIC.
            ' In UP si possono generare riduzioni di impegni a zero, in ragioneria sarà possibile registrarle
            ' che sul sic corrisponderanno ad una cancellazione (trasparente all'utente)
            'If (totResiduo) = 0 Then
            '    Throw New Exception("Non è possibile ridurre a ZERO l'impegno. Per una riduzione totale è necessario scegliere dalle Operazioni Contabili la Rettifica Contabile e selezionare la voce 'Cancellazione Documento Contabile'.")
            'End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If Not String.IsNullOrEmpty(importo) Then
                Dim itemRiduzione As DllDocumentale.ItemRiduzioneInfo = New DllDocumentale.ItemRiduzioneInfo
                With itemRiduzione
                    .HashTokenCallSic = GenerateHashTokenCallSic()
                    .Dli_Documento = CodDocumento()
                    .Dli_DataRegistrazione = Now
                    .Dli_Operatore = operatore.Codice
                    .Dli_Esercizio = bilancio
                    .Dli_UPB = upb
                    .Dli_MissioneProgramma = missioneProgramma
                    .Dli_Cap = capitolo
                    .Dli_Costo = importo
                    .Dli_NumImpegno = CStr(NumImpegno)
                    .DBi_Anno = bilancio
                    .Dli_NPreImpegno = NumPreImp
                    .Div_TipoAssunzione = tipoAtto
                    .Div_Data_Assunzione = dataAtto
                    .Div_Num_assunzione = numAtto
                    'Lu Rid
                    .Div_IsEconomia = isEconomia
                    .Di_Stato = 1
                End With
                Registra_Riduzione(operatore, itemRiduzione)
            End If
            Return "ok"
        Catch ex As Exception
            Log.Error(ex.ToString)
            'Throw New Exception(ex.Message)
            Return ex.Message
            'HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzioniMultiple")>
    Public Sub GenerazioneRiduzioniMultiple()
        Dim errStr As String = "<h1 style='text-align:center'>Errore sulle seguenti riduzioni:</h1> "
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If
            'valori dela richiesta
            Dim isEconomia1 As Integer = 0
            Dim isEconomia2 As Integer = 0
            Dim isEconomia3 As Integer = 0


            Dim bilancioCorrente As Integer
            Integer.TryParse(HttpContext.Current.Request.Item("Bilancio"), bilancioCorrente)
            'Dim bilancio As String = HttpContext.Current.Request.Item("Bilancio")
            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            Dim upb As String = HttpContext.Current.Request.Item("UPB")
            Dim missioneProgramma As String = HttpContext.Current.Request.Item("MissioneProgramma")

            Dim dataAtto1 As String = HttpContext.Current.Request.Item("DataAtto1")
            Dim tipoAtto1 As String = HttpContext.Current.Request.Item("TipoAtto1")
            Dim numAtto1 As String = HttpContext.Current.Request.Item("NumeroAtto1")
            Dim NumImpegno1 As Long
            Long.TryParse(HttpContext.Current.Request.Item("NumImpegno1"), NumImpegno1)
            Dim lstr_impDisp1 As String = HttpContext.Current.Request.Item("ImpDisp1")
            Dim importo1 As String = HttpContext.Current.Request.Item("ImpDaRidurre1")
            Dim NumPreImp As String = HttpContext.Current.Request.Item("NumPreImp")
            Integer.TryParse(HttpContext.Current.Request.Item("IsEconomia1"), isEconomia1)

            Dim dataAtto2 As String = HttpContext.Current.Request.Item("DataAtto2")
            Dim tipoAtto2 As String = HttpContext.Current.Request.Item("TipoAtto2")
            Dim numAtto2 As String = HttpContext.Current.Request.Item("NumeroAtto2")
            Dim NumImpegno2 As Long
            Long.TryParse(HttpContext.Current.Request.Item("NumImpegno2"), NumImpegno2)
            Dim lstr_impDisp2 As String = HttpContext.Current.Request.Item("ImpDisp2")
            Dim importo2 As String = HttpContext.Current.Request.Item("ImpDaRidurre2")
            Integer.TryParse(HttpContext.Current.Request.Item("IsEconomia2"), isEconomia2)

            Dim dataAtto3 As String = HttpContext.Current.Request.Item("DataAtto3")
            Dim tipoAtto3 As String = HttpContext.Current.Request.Item("TipoAtto3")
            Dim numAtto3 As String = HttpContext.Current.Request.Item("NumeroAtto3")
            Dim NumImpegno3 As Long
            Long.TryParse(HttpContext.Current.Request.Item("NumImpegno3"), NumImpegno3)
            Dim lstr_impDisp3 As String = HttpContext.Current.Request.Item("ImpDisp3")
            Dim importo3 As String = HttpContext.Current.Request.Item("ImpDaRidurre3")
            Integer.TryParse(HttpContext.Current.Request.Item("IsEconomia3"), isEconomia3)
            Dim esito1 As String = ""
            Dim esito2 As String = ""
            Dim esito3 As String = ""



            If (NumImpegno1 <> 0) Then
                esito1 = GenerazioneRiduzioneUP(bilancioCorrente.ToString, capitolo, upb, dataAtto1, tipoAtto1, numAtto1, NumImpegno1, lstr_impDisp1, importo1, NumPreImp, isEconomia1, missioneProgramma)
                If (esito1 = "ok") Then
                Else
                    errStr = errStr & "</br></br><b>Riduzione 1</b>: </br>bilancio <b>" & (Year(Now)) & "</b> - impegno <b>" & NumImpegno1 & "</b> ->" & esito1
                    'HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(errStr, "'", "\'") + "' }")
                End If
            End If
            If (NumImpegno2 <> 0) Then
                esito2 = GenerazioneRiduzioneUP((bilancioCorrente + 1).ToString, capitolo, upb, dataAtto2, tipoAtto2, numAtto2, NumImpegno2, lstr_impDisp2, importo2, NumPreImp, isEconomia2, missioneProgramma)
                If (esito2 = "ok") Then
                Else
                    errStr = errStr & "</br></br><b>Riduzione 2</b>: </br>bilancio <b>" & (Year(Now) + 1) & "</b> - impegno <b>" & NumImpegno2 & "</b> -> " & esito2
                End If
            End If
            If (NumImpegno3 <> 0) Then
                esito3 = GenerazioneRiduzioneUP((bilancioCorrente + 2).ToString, capitolo, upb, dataAtto3, tipoAtto3, numAtto3, NumImpegno3, lstr_impDisp3, importo3, NumPreImp, isEconomia3, missioneProgramma)
                If (esito3 = "ok") Then
                Else
                    errStr = errStr & "</br></br><b>Riduzione 3</b>: </br>bilancio <b>" & (Year(Now) + 2) & "</b> - impegno <b>" & NumImpegno3 & "</b> -> " & esito3
                    'HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(errStr, "'", "\'") + "' }")
                End If
            End If
            'If (esito1 = "ok" And esito2 = "ok" And esito3 = "ok") Or _
            '       (esito1 = "ok" And esito2 = "" And esito3 = "") Or _
            '       (esito1 = "" And esito2 = "ok" And esito3 = "") Or _
            '       (esito1 = "" And esito2 = "" And esito3 = "ok") Then
            '    HttpContext.Current.Response.Write("{  success: true }")
            'ElseIf ((esito1 <> "ok" And esito1 <> "") Or (esito2 <> "ok" And esito2 <> "") Or (esito3 <> "ok" And esito3 <> "")) Then
            '    HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(errStr, "'", "\'") + "' }")
            'End If

            If ((esito1 <> "ok" And esito1 <> "") Or (esito2 <> "ok" And esito2 <> "") Or (esito3 <> "ok" And esito3 <> "")) Then
                HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(errStr, "'", "\'") + "' }")
            Else
                HttpContext.Current.Response.Write("{  success: true }")
            End If


        Catch ex As Exception
            Log.Error(ex.ToString)

            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(errStr, "'", "\'") + "' }")
        End Try
    End Sub
    Public Shared Function CodDocumento() As String
        Dim _codDocumento As String = HttpContext.Current.Request.QueryString.Item("key")
        If String.IsNullOrEmpty(_codDocumento) Then
            _codDocumento = HttpContext.Current.Items.Item("key")
        End If
        If String.IsNullOrEmpty(_codDocumento) Then
            _codDocumento = HttpContext.Current.Session.Item("codDocumento")
        End If
        If String.IsNullOrEmpty(_codDocumento) Then
            _codDocumento = ""
        End If
        If String.IsNullOrEmpty(_codDocumento) Then
            _codDocumento = HttpContext.Current.Session.Item("key")
        End If
        Return _codDocumento
    End Function
    Private Function GetArrayValoriContoEconomica(ByVal item As DllDocumentale.ItemContabileInfo, ByVal ufficioProp As String) As ArrayList
        Dim arrEconomica As New ArrayList
        If String.IsNullOrEmpty(item.Di_ContoEconomica) Then
            Try
                arrEconomica = GetInterrogazioneBilancioContoEconomica_post(item.Dli_Cap, item.Dli_Esercizio, ufficioProp)
            Catch ex As Exception
                Log.Error(ex.ToString)
                Throw New Exception(ex.Message)
            End Try

        Else
            arrEconomica.Add(item.Di_ContoEconomica)
        End If
        Return arrEconomica
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaImp?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}&CodiceUfficio={CodiceUfficio}")>
    Public Function GetListaImp(ByVal AnnoRif As String, ByVal CapitoloRif As String, ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Try

            Dim impegni As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneImpegni As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneImpegniApertiMessage(operatore, AnnoRif, CapitoloRif, CodiceUfficio)

            For i As Integer = 0 To UBound(rispostaInterrogazioneImpegni, 1) 'Verifica count
                
                Dim ImpRestituito As New Ext_CapitoliAnnoInfo
                ImpRestituito.TipoAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).TipoAtto
                ImpRestituito.DataAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).DataAtto
                ImpRestituito.NumeroAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroAtto
                ImpRestituito.NumImpegno = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroImpegno
                ImpRestituito.ImpDisp = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).ImportoDisponibile
                ImpRestituito.Oggetto_Impegno = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).OggettoImpegno
                ImpRestituito.Codice_Obbiettivo_Gestionale = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).COG_Semplice
                'verifica il potenziale importo, cioè tenendo conto di eventuali riduzioni/liquidazioni sull' impegno (non registrate sul sic) di atti in itinere
                ImpRestituito.ImpPotenzialePrenotato = residuaDisponibilitaDaImpegno(operatore, ImpRestituito.NumImpegno, ImpRestituito.ImpDisp)

                ImpRestituito.PianoDeiContiFinanziario = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).PCF.Codice

                '****** Il caricamento del relativo beneficiario è stato spostato alla selezione dell'impegno da interfaccia ****** 
                'Dim DisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, ImpRestituito.NumImpegno)
                '    Dim listaBeneficiariImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario() = DisponibilitaImpegno.Beneficiario

                '    ImpRestituito.ListaBeneficiari = New List(Of Ext_AnagraficaInfo)
                '    ImpRestituito.Beneficiario = New Ext_AnagraficaInfo
                '    If Not listaBeneficiariImpegnoSIC Is Nothing Then
                '        For Each beneficiarioImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario In listaBeneficiariImpegnoSIC
                '            Dim beneficiarioImpegno As New Ext_AnagraficaInfo
                '            beneficiarioImpegno = beneficiarioImpegno.TransformDisponibilitaImpegnoBeneficiarioInExtObj(beneficiarioImpegnoSIC)

                '            ImpRestituito.Beneficiario = beneficiarioImpegno
                '            ImpRestituito.ListaBeneficiari.Add(beneficiarioImpegno)
                '        Next
                '    End If

                '            ImpRestituito.Beneficiario = beneficiarioImpegno
                '            ImpRestituito.ListaBeneficiari.Add(beneficiarioImpegno)
                '        Next
                '    End If
                
                impegni.Add(ImpRestituito)    
                
                
            Next
            Return impegni
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetDettaglioImpegno?CodiceUfficio={CodiceUfficio}")>
    Public Function GetDettaglioImpegno(ByVal AnnoRif As String, ByVal CapitoloRif As String, ByVal NumeroImpegno As String, ByVal CodiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)

        Try
            Dim impegni As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneImpegni As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneImpegniApertiMessage(operatore, AnnoRif, CapitoloRif, CodiceUfficio)
            '-1
            For i As Integer = 0 To UBound(rispostaInterrogazioneImpegni, 1)
                If NumeroImpegno = "" OrElse NumeroImpegno = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroImpegno Then
                    Dim ImpRestituito As New Ext_CapitoliAnnoInfo
                    ImpRestituito.Tipo = " "
                    ImpRestituito.TipoAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).TipoAtto
                    ImpRestituito.DataAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).DataAtto
                    ImpRestituito.NumeroAtto = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroAtto
                    ImpRestituito.NumImpegno = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).NumeroImpegno
                    ImpRestituito.ImpDisp = DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).ImportoDisponibile
                    ImpRestituito.ImpPotenzialePrenotato = residuaDisponibilitaDaImpegno(operatore, ImpRestituito.NumImpegno, ImpRestituito.ImpDisp)
                    ImpRestituito.Codice_Obbiettivo_Gestionale = "" & DirectCast(rispostaInterrogazioneImpegni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniAperti_TypesImpegno).COG_Semplice

                    impegni.Add(ImpRestituito)
                End If

            Next

            Return impegni
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetBeneficiarioImpegno")>
    Public Function GetBeneficiarioImpegno(ByVal NumImpegno As String) As IList(Of Ext_AnagraficaInfo)
        Try
            Dim impegni As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim DisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, NumImpegno)
            Dim listaBeneficiariImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario() = DisponibilitaImpegno.Beneficiario
            Dim listaBeneficiari = New List(Of Ext_AnagraficaInfo)
            If Not listaBeneficiariImpegnoSIC Is Nothing Then
                For Each beneficiarioImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario In listaBeneficiariImpegnoSIC
                    Dim beneficiarioImpegno As New Ext_AnagraficaInfo
                    beneficiarioImpegno = beneficiarioImpegno.TransformDisponibilitaImpegnoBeneficiarioInExtObj(beneficiarioImpegnoSIC)
                    listaBeneficiari.Add(beneficiarioImpegno)
                Next
            End If
            Return listaBeneficiari
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetRiduzioniRegistrate")>
    Public Function GetRiduzioniRegistrate() As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaRiduzioniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim riduzioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemRiduzioneInfo)
            riduzioniRegistrate = Get_Riduzioni(operatore)
            listaRiduzioniExtDaRestituire = trasformaListaRidInfoInExt(riduzioniRegistrate, listaRiduzioniExtDaRestituire)


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaRiduzioniExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetRiduzioniPreImpegniRegistrati")>
    Public Function GetRiduzioniPreImpegniRegistrati() As IList(Of Ext_CapitoliAnnoInfo)
        Dim listaRiduzioniExtDaRestituire As New List(Of Ext_CapitoliAnnoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim riduzioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemRiduzioneInfo)
            riduzioniRegistrate = Get_RiduzioniPreImpegni(operatore)
            listaRiduzioniExtDaRestituire = trasformaListaRidInfoInExt(riduzioniRegistrate, listaRiduzioniExtDaRestituire)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaRiduzioniExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetMandatiRegistrati")>
    Public Function GetMandatiRegistrati() As IList(Of Ext_MandatoInfo)
        Dim listaMandatiExtDaRestituire As New List(Of Ext_MandatoInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim mandatiRegistrati As System.Collections.Generic.List(Of DllDocumentale.ItemMandatoInfo) = Get_Mandati(operatore)

            Dim itemExt_Mandato As Ext_MandatoInfo
            For Each mandato As DllDocumentale.ItemMandatoInfo In mandatiRegistrati
                itemExt_Mandato = New Ext_MandatoInfo
                With itemExt_Mandato
                    '.AnnoPrenotazione = impegno.DBi_Anno
                    .ID = mandato.Man_id
                    .DataMandato = mandato.Man_DataMandato
                    .Doc_id = mandato.Man_Doc_id
                    .Nimpegno = mandato.Man_Nimpegno
                    .NImporto = mandato.Man_NImporto
                    .Nmandato = mandato.Man_Nmandato
                    .NLiquidazione = mandato.Man_NLiquidazione

                End With

                listaMandatiExtDaRestituire.Add(itemExt_Mandato)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaMandatiExtDaRestituire
    End Function
    Private Function Get_Mandati(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemMandatoInfo)

        Dim listaItemMandati As List(Of DllDocumentale.ItemMandatoInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemMandati = dllDoc.FO_Get_DatiMandati(CodDocumento())
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemMandati
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneAccertamentoUP")>
    Public Sub GenerazioneAccertamentoUP()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            'valori dela richiesta
            Dim importo As String = HttpContext.Current.Request.Item("ImpDaAccertare")
            Dim id As String = "" & HttpContext.Current.Request.Item("idAccertamento")
            If id = "" Then
                id = 0
            End If

            Dim itemAccertamento As DllDocumentale.ItemAssunzioneContabileInfo = New DllDocumentale.ItemAssunzioneContabileInfo

            If Not String.IsNullOrEmpty(importo) Then
                With itemAccertamento
                    .Da_Documento = CodDocumento()
                    .Da_DataRegistrazione = Now
                    .Da_Operatore = operatore.Codice
                    .Da_Costo = importo
                    .Da_prog = id
                    .Da_Stato = 1
                End With
                Registra_Accertamento(operatore, itemAccertamento)
            End If

            Dim result As String = "{  success: true, idAccertamento: '" + CStr(itemAccertamento.Da_prog) + "' }"

            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
            '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetAccertamentoRegistrato")>
    Public Function GetAccertamentoRegistrato() As Ext_CapitoliAnnoInfo
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim accertamentoRegistrato As System.Collections.Generic.List(Of DllDocumentale.ItemAssunzioneContabileInfo) = Get_Accertamento(operatore)
            Dim accertamentoRegistratoExtDaRestituire As New Ext_CapitoliAnnoInfo


            With accertamentoRegistratoExtDaRestituire
                .Tipo = " "
                .ImpPrenotato = 0
                .NumeroAtto = CodDocumento()
                .ID = 0
            End With


            If accertamentoRegistrato.Count > 0 Then
                Dim ultimo As Integer = accertamentoRegistrato.Count - 1
                accertamentoRegistratoExtDaRestituire.ImpPrenotato = accertamentoRegistrato(ultimo).Da_Costo
                accertamentoRegistratoExtDaRestituire.ID = accertamentoRegistrato(ultimo).Da_prog
                accertamentoRegistratoExtDaRestituire.NumeroAtto = accertamentoRegistrato(ultimo).Da_Documento
            End If


            Return accertamentoRegistratoExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    Private Function Get_Accertamento(ByVal operatore As DllAmbiente.Operatore) As List(Of DllDocumentale.ItemAssunzioneContabileInfo)
        Dim listaItemAccertamento As List(Of DllDocumentale.ItemAssunzioneContabileInfo)
        Try

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            listaItemAccertamento = dllDoc.FO_Get_Dati_Assunzione(CodDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaItemAccertamento
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetTutteLiquidazioniRegistrate?CodiceUfficio={CodiceUfficio}")>
    Public Function GetTutteLiquidazioniRegistrate(ByVal CodiceUfficio As String) As IList(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim liquidazioniRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = Get_LiquidazioniTutte(operatore)
            Dim listaLiquidazioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)
            Dim itemExt_Liquidazione As Ext_LiquidazioneInfo
            For Each liquidazione As DllDocumentale.ItemLiquidazioneInfo In liquidazioniRegistrate
                itemExt_Liquidazione = New Ext_LiquidazioneInfo
                With itemExt_Liquidazione
                    .HashTokenCallSic = liquidazione.HashTokenCallSic
                    .IdDocContabileSic = liquidazione.IdDocContabileSic
                    .Tipo = " "
                    .NumImpegno = liquidazione.Dli_NumImpegno
                    .Bilancio = liquidazione.Dli_Esercizio
                    .Capitolo = liquidazione.Dli_Cap
                    .ImpPrenotatoLiq = liquidazione.Dli_Costo
                    .UPB = liquidazione.Dli_UPB
                    .MissioneProgramma = liquidazione.Dli_MissioneProgramma
                    .ID = liquidazione.Dli_prog
                    .ImportoIva = liquidazione.Dli_ImportoIva
                    .ContoEconomica = liquidazione.Di_ContoEconomica
                    '.ContoEconomicaLista = GetArrayValoriContoEconomica(liquidazione, CodiceUfficio)

                    If .ContoEconomica = "" Then
                        If .ContoEconomicaLista.Count = 1 Then
                            .ContoEconomica = .ContoEconomicaLista(0)
                        End If
                    End If
                    .NLiquidazione = IIf(liquidazione.Dli_NLiquidazione = 0, "", liquidazione.Dli_NLiquidazione)
                    .NumPreImp = liquidazione.Dli_NPreImpegno
                    .Stato = liquidazione.Di_Stato
                    .IdImpegno = liquidazione.Dli_IdImpegno
                    .PianoDeiContiFinanziario = liquidazione.Dli_PianoDeiContiFinanziari
                End With
                listaLiquidazioniExtDaRestituire.Add(itemExt_Liquidazione)
            Next
            Return listaLiquidazioniExtDaRestituire

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetDipartimentiDipendenti")>
    Public Function GetDipartimentiDipendenti() As IList(Of Ext_StrutturaInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            'I capitoli di competenza dell'ufficio, sono gli stessi sia per le determine che per le disposizioni,
            ' quindi viene passato 0 di default (DETERMINE) perchè sortiscono lo stesso effetto
            Dim dipartimenti As ArrayList = operatore.DipartimentoDipendenti("0")

            Dim listaDipartimenti_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            Dim dipartimentoExt As Ext_StrutturaInfo = Nothing
            For Each dipartimento As DllAmbiente.StrutturaInfo In dipartimenti
                dipartimentoExt = New Ext_StrutturaInfo
                dipartimentoExt.CodiceInterno = dipartimento.CodiceInterno
                dipartimentoExt.DescrizioneBreve = dipartimento.DescrizioneBreve
                dipartimentoExt.Padre = dipartimento.Padre
                dipartimentoExt.Tipologia = dipartimento.Tipologia
                dipartimentoExt.CodicePubblico = dipartimento.CodicePubblico
                listaDipartimenti_Ext_DaRestituire.Add(dipartimentoExt)
            Next

            Return listaDipartimenti_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetUfficiDipendenti?dipartimentoScelto={dipartimentoScelto}")>
    Public Function GetUfficiDipendenti(ByVal dipartimentoScelto As String) As IList(Of Ext_StrutturaInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            'I capitoli di competenza dell'ufficio, sono gli stessi sia per le determine che per le disposizioni,
            ' quindi viene passato 0 di default (DETERMINE) perchè sortiscono lo stesso effetto
            Dim uffici As ArrayList = operatore.UfficiDipendenti("0", dipartimentoScelto)

            Dim listaUffici_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            Dim UfficioExt As Ext_StrutturaInfo = Nothing
            For Each ufficio As DllAmbiente.StrutturaInfo In uffici
                UfficioExt = New Ext_StrutturaInfo
                UfficioExt.CodiceInterno = ufficio.CodiceInterno
                UfficioExt.DescrizioneBreve = ufficio.DescrizioneBreve
                UfficioExt.Padre = ufficio.Padre
                UfficioExt.Tipologia = ufficio.Tipologia
                UfficioExt.CodicePubblico = ufficio.CodicePubblico
                listaUffici_Ext_DaRestituire.Add(UfficioExt)
            Next

            Return listaUffici_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetUtenzeCF?codice_fiscale={codice_fiscale}")>
    Public Function GetUtenzeCF(ByVal codice_fiscale As String) As IList(Of Ext_SceltaOperatoreInfo)
        Try
            Dim ooperatore As New DllAmbiente.Operatore
            Dim listasceltePresentation As Generic.List(Of Ext_SceltaOperatoreInfo) = Nothing
            If String.IsNullOrEmpty(codice_fiscale) And String.IsNullOrEmpty(HttpContext.Current.Session.Item("CodiceFiscale")) Then
                listasceltePresentation = New Generic.List(Of Ext_SceltaOperatoreInfo)
                Return listasceltePresentation
            Else

                Dim listascelte As Generic.List(Of DllAmbiente.SceltaOperatoreInfo) = Nothing
                If Not String.IsNullOrEmpty(codice_fiscale) Then
                    listascelte = ooperatore.Leggi_Dati_CF(codice_fiscale)
                ElseIf Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("CodiceFiscale")) Then
                    listascelte = ooperatore.Leggi_Dati_CF(HttpContext.Current.Session.Item("CodiceFiscale"))
                End If

                If Not listascelte Is Nothing Then
                    listasceltePresentation = New Generic.List(Of Ext_SceltaOperatoreInfo)
                    For Each scelta As DllAmbiente.SceltaOperatoreInfo In listascelte
                        Dim sceltaPresentation As New Ext_SceltaOperatoreInfo
                        sceltaPresentation.CodiceOperatore = scelta.CodiceOperatore
                        sceltaPresentation.CodiceUfficio = scelta.CodiceUfficio
                        sceltaPresentation.DescrizioneUfficio = scelta.DescrizioneUfficio
                        sceltaPresentation.CodiceUfficioPubblico = scelta.CodiceUfficioPubblico
                        listasceltePresentation.Add(sceltaPresentation)
                    Next
                End If

                Return listasceltePresentation

            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetTuttiDipartimenti?isPerNotifica={isPerNotifica}")>
    Public Function GetTuttiDipartimenti(Optional ByVal isPerNotifica As Boolean = False) As IList(Of Ext_StrutturaInfo)
        Try

            Dim objufficio As DllAmbiente.Ufficio = New DllAmbiente.Ufficio
            'I capitoli di competenza dell'ufficio, sono gli stessi sia per le determine che per le disposizioni,
            ' quindi viene passato 0 di default (DETERMINE) perchè sortiscono lo stesso effetto
            Dim dipartimenti As ArrayList
            If isPerNotifica Then
                dipartimenti = objufficio.GetTuttiDipartimentiVisibiliPerNotifica
            Else
                dipartimenti = objufficio.GetTuttiDipartimenti
            End If


            Dim listaDipartimenti_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            Dim dipartimentoExt As Ext_StrutturaInfo = Nothing
            For Each dipartimento As DllAmbiente.StrutturaInfo In dipartimenti
                dipartimentoExt = New Ext_StrutturaInfo
                dipartimentoExt.CodiceInterno = dipartimento.CodiceInterno
                dipartimentoExt.DescrizioneBreve = dipartimento.DescrizioneBreve
                dipartimentoExt.Padre = dipartimento.Padre
                dipartimentoExt.Tipologia = dipartimento.Tipologia
                dipartimentoExt.CodicePubblico = dipartimento.CodicePubblico
                dipartimentoExt.DescrizioneToDisplay = dipartimento.CodicePubblico & " - " & dipartimento.DescrizioneBreve
                listaDipartimenti_Ext_DaRestituire.Add(dipartimentoExt)
            Next

            Return listaDipartimenti_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetTuttiUffici?dipartimentoScelto={dipartimentoScelto}")>
    Public Function GetTuttiUffici(ByVal dipartimentoScelto As String) As IList(Of Ext_StrutturaInfo)
        Try

            Dim objufficio As DllAmbiente.Ufficio = New DllAmbiente.Ufficio
            'I capitoli di competenza dell'ufficio, sono gli stessi sia per le determine che per le disposizioni,
            ' quindi viene passato 0 di default (DETERMINE) perchè sortiscono lo stesso effetto
            Dim uffici As ArrayList = objufficio.GetTuttiUfficiDelDipartimento(dipartimentoScelto)

            Dim listaUffici_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            Dim UfficioExt As Ext_StrutturaInfo = Nothing
            For Each ufficio As DllAmbiente.StrutturaInfo In uffici
                UfficioExt = New Ext_StrutturaInfo
                UfficioExt.CodiceInterno = ufficio.CodiceInterno
                UfficioExt.DescrizioneBreve = ufficio.DescrizioneBreve
                UfficioExt.Padre = ufficio.Padre
                UfficioExt.Tipologia = ufficio.Tipologia
                UfficioExt.CodicePubblico = ufficio.CodicePubblico
                UfficioExt.DescrizioneToDisplay = ufficio.CodicePubblico & " - " & ufficio.DescrizioneBreve
                listaUffici_Ext_DaRestituire.Add(UfficioExt)
            Next

            Return listaUffici_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetTuttiDipendentiUfficio?ufficioScelto={ufficioScelto}")>
    Public Function GetTuttiDipendentiUfficio(ByVal ufficioScelto As String) As IList(Of Ext_UtenteInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim dipendenti As New Hashtable
            Dim ufficio1 As New DllAmbiente.Ufficio
            ufficio1.CodUfficio = ufficioScelto
            Dim dipendentiUfficio As Hashtable = ufficio1.UtentiUfficio("")

            Dim listaUtentiUfficio_Ext_DaRestituire As List(Of Ext_UtenteInfo) = New List(Of Ext_UtenteInfo)

            Dim utenteInfo As Ext_UtenteInfo = Nothing
            For Each key As String In dipendentiUfficio.Keys
                utenteInfo = New Ext_UtenteInfo

                utenteInfo.Account = key
                utenteInfo.Denominazione = dipendentiUfficio(key)
                utenteInfo.CodiceInternoUfficio = ufficio1.CodUfficio
                utenteInfo.CodicePubblicoUfficio = ufficio1.CodUfficioPubblico

                listaUtentiUfficio_Ext_DaRestituire.Add(utenteInfo)
            Next



            'Dim objufficio As DllAmbiente.Ufficio = New DllAmbiente.Ufficio
            ''I capitoli di competenza dell'ufficio, sono gli stessi sia per le determine che per le disposizioni,
            '' quindi viene passato 0 di default (DETERMINE) perchè sortiscono lo stesso effetto
            'Dim uffici As ArrayList = objufficio.GetTuttiUfficiDelDipartimento("D01")



            'Dim listaUffici_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            'Dim UfficioExt As Ext_StrutturaInfo = Nothing
            'For Each ufficio As DllAmbiente.StrutturaInfo In uffici
            '    UfficioExt = New Ext_StrutturaInfo
            '    UfficioExt.CodiceInterno = ufficio.CodiceInterno
            '    UfficioExt.DescrizioneBreve = ufficio.DescrizioneBreve
            '    UfficioExt.Padre = ufficio.Padre
            '    UfficioExt.Tipologia = ufficio.Tipologia
            '    UfficioExt.CodicePubblico = ufficio.CodicePubblico
            '    UfficioExt.DescrizioneToDisplay = ufficio.CodicePubblico & " - " & ufficio.DescrizioneBreve
            '    listaUffici_Ext_DaRestituire.Add(UfficioExt)
            'Next

            Return listaUtentiUfficio_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function





    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetUfficiCompetenzaDocumento")>
    Public Function GetUfficiCompetenzaDocumento() As IList(Of Ext_StrutturaInfo)
        Try


            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim localCodiceDocumento As String = ""
            Try
                localCodiceDocumento = CodDocumento()
            Catch ex As Exception
                Log.Error(operatore.Codice & " GetUfficiCompetenzaDocumento Codice Documento non trovato")
                localCodiceDocumento = ""
            End Try



            Dim listaUffici_Ext_DaRestituire As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
            If localCodiceDocumento = "" Then
                Return listaUffici_Ext_DaRestituire
            End If

            Dim uffici As Generic.IList(Of DllAmbiente.StrutturaInfo) = dllDoc.Get_Lista_Documento_Uffici_Competenza(CodDocumento, operatore.Codice)
            Dim ufficioExt As Ext_StrutturaInfo = Nothing
            For Each ufficioItem As DllAmbiente.StrutturaInfo In uffici
                ufficioExt = New Ext_StrutturaInfo
                ufficioExt.CodiceInterno = ufficioItem.CodiceInterno
                ufficioExt.DescrizioneBreve = ufficioItem.DescrizioneBreve
                ufficioExt.Padre = ufficioItem.Padre
                ufficioExt.Tipologia = ufficioItem.Tipologia
                ufficioExt.CodicePubblico = ufficioItem.CodicePubblico
                ufficioExt.DescrizioneToDisplay = ufficioItem.CodicePubblico & " - " & ufficioItem.DescrizioneBreve
                listaUffici_Ext_DaRestituire.Add(ufficioExt)
            Next

            Return listaUffici_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetUtentiCompetenzaDocumento")>
    Public Function GetUtentiCompetenzaDocumento() As IList(Of Ext_UtenteInfo)
        Try


            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim localCodiceDocumento As String = ""
            Try
                localCodiceDocumento = CodDocumento()
            Catch ex As Exception
                Log.Error(operatore.Codice & " GetUtentiCompetenzaDocumento Codice Documento non trovato")
                localCodiceDocumento = ""
            End Try



            Dim listaUtenti_Ext_DaRestituire As List(Of Ext_UtenteInfo) = New List(Of Ext_UtenteInfo)
            If localCodiceDocumento = "" Then
                Return listaUtenti_Ext_DaRestituire
            End If

            Dim utenti As Generic.IList(Of DllAmbiente.Utente) = dllDoc.Get_Lista_Documento_Utenti_Uffici_Competenza(CodDocumento, operatore.Codice)
            Dim utenteExt As Ext_UtenteInfo = Nothing
            For Each utenteItem As DllAmbiente.Utente In utenti
                utenteExt = New Ext_UtenteInfo
                utenteExt.Account = utenteItem.Account
                utenteExt.Denominazione = utenteItem.Denominazione
                utenteExt.CodicePubblicoUfficio = ""
                utenteExt.CodiceInternoUfficio = ""

                listaUtenti_Ext_DaRestituire.Add(utenteExt)
            Next

            Return listaUtenti_Ext_DaRestituire
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/SetUfficiCompetenza")>
    Public Sub SetUfficiCompetenza()
        Try
            Dim listaUffici As String = HttpContext.Current.Request.Form("itemselector_obj")

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim dlldoc As New DllDocumentale.svrDocumenti(operatore)
            Dim listaStrutturaUfficio As New List(Of DllAmbiente.StrutturaInfo)
            Dim ufficioStruttura As New DllAmbiente.StrutturaInfo

            For Each codUfficio As String In listaUffici.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                ufficioStruttura = New DllAmbiente.StrutturaInfo
                ufficioStruttura.CodiceInterno = codUfficio
                If Not listaStrutturaUfficio.Contains(ufficioStruttura) Then
                    listaStrutturaUfficio.Add(ufficioStruttura)
                End If

            Next
            dlldoc.FO_Insert_Documento_Uffici_Competenza(CodDocumento, operatore.Codice, listaStrutturaUfficio)

            HttpContext.Current.Response.Write("{ success: true }")
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetMessaggi?key={key}")>
    Public Function GetMessaggi(ByVal key As String) As List(Of Ext_MessaggioInfo)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim inviati As Integer = IIf(HttpContext.Current.Request.QueryString.Item("inv") = 1, 1, 0)
            Dim messaggi As List(Of Ext_MessaggioInfo)
            If key = "" Then
                'prendo tutti i mess dell'utente
                If inviati = 1 Then
                    messaggi = Elenco_Messaggi(, True)
                Else
                    messaggi = Elenco_Messaggi()
                End If


            Else
                'prendo i mess dell'utente relativi ad un documento specifico
                If inviati = 1 Then
                    messaggi = Elenco_Messaggi(key, True)
                Else
                    messaggi = Elenco_Messaggi(key)
                End If

            End If
            Return messaggi
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaMessaggio")>
    Public Function EliminaMessaggio(ByVal ID As String) As Boolean
        Try
            If String.IsNullOrEmpty(ID) Then
                Throw New Exception("Impossibile trovare il messaggio da eliminare")
            End If
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Cancella_Messaggio(ID)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ContrassegnaComeLetto")>
    Public Function ContrassegnaComeLetto(ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            If String.IsNullOrEmpty(ID) Then
                Throw New Exception("Impossibile trovare il messaggio da eliminare")
            End If

            Dim svrdocumenti As New DllDocumentale.svrDocumenti(operatore)
            svrdocumenti.SEGNA_MESSAGGIO_COME_LETTO(ID)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/RegistraOsservazione?key={key}")>
    Public Sub RegistraOsservazione(ByVal key As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim osservazione As String = ""
            If key = "" Then
                key = CodDocumento()
            End If
            Dim objStato As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(key)

            Select Case objStato.LivelloUfficio
                Case "UP"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUP")
                Case "UR"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUR")
                Case "UCA"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUCA")
                Case "UDD"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUDD")
                Case "USL"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUSL")
                Case "USS"
                    osservazione = HttpContext.Current.Request.Item("OsservazioneUSS")
            End Select

            If Not String.IsNullOrEmpty(osservazione.Trim) OrElse osservazione.Trim.Length < 10 Then
                Registra_Osservazione(key, osservazione)
                HttpContext.Current.Response.Write("{ success: true }")
            Else
                HttpContext.Current.Response.Write("{  success: false, FaultMessage: 'Il testo non può contenere solo spazi bianchi e deve essere di almeno 10 caratteri' }")
            End If



        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/VerificaFattureDaAggiungere")>
    Public Function VerificaFattureDaAggiungere(ByVal fattureDaAggiungere As Generic.List(Of Ext_FatturaInfoHeader)) As String
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim svrdoc As New DllDocumentale.svrDocumenti(operatore)
            'Dim esiste As Boolean = svrdoc.EsistenzaOggetto(tipo, oggetto)
            Dim esiste As Boolean = False
            Dim numeroFatturaEsistente As String = ""
            For Each fatturaExt As Ext_FatturaInfoHeader In fattureDaAggiungere
                esiste = svrdoc.EsistenzaFatture(fatturaExt.IdUnivoco)
                If esiste Then
                    numeroFatturaEsistente = fatturaExt.NumeroFatturaBeneficiario
                    Exit For
                End If
            Next



            Return numeroFatturaEsistente
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/VerificaEsistenzaOggetto")>
    Public Function VerificaEsistenzaOggetto(ByVal tipo As String, ByVal oggetto As String) As Boolean
        Try
            'Dim oggetto As String = HttpContext.Current.Request.Form("txtOggetto")
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim svrdoc As New DllDocumentale.svrDocumenti(operatore)
            Dim esiste As Boolean = svrdoc.EsistenzaOggetto(tipo, oggetto)

            Return esiste
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaImpegnoNonAncoraConfermato")>
    Public Function EliminaImpegnoNonAncoraConfermato(ByVal NumeroPreImpegno As String, ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo

            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If

            If String.IsNullOrEmpty(NumeroPreImpegno) Then
                Throw New Exception("impossibile trovare il preimpegno da eliminare")
            End If

            If String.IsNullOrEmpty(ID) Then
                Throw New Exception("Impossibile trovare il preimpegno da eliminare")
            End If


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            itemBilancio = dllDoc.FO_Get_DatiImpegni(CodDocumento(), ID).Item(0)
            If NumeroPreImpegno = itemBilancio.Dli_NPreImpegno Then

                Dim rispostaEliminazionePreImpegno As Boolean = True

                Elimina_ImpegnoByID(operatore, itemBilancio)

                Dim itemliquidazione As New DllDocumentale.ItemLiquidazioneInfo

                itemliquidazione.Dli_Documento = CodDocumento()
                itemliquidazione.Dli_NPreImpegno = NumeroPreImpegno
                'Cancello le liquidazioni con quel numero di impegno, nel caso di liquidazioni con lo stesso preimpegno
                'da delibera, cancello tutte le liq.
                'è possibile aggiungerne altre
                Elimina_Liquidazione(operatore, itemliquidazione)

            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function


    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaMultiplaPreImpegno")>
    Public Sub ConfermaMultiplaPreImpegno(ByVal ImpegniInfo As Ext_CapitoliAnnoInfo(), ByVal CodiceUfficio As String)
        Dim confirmed As Integer = 0
        Try
            Dim idDocumento As String = CodDocumento()

            If Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty Then
                Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(idDocumento)

                If statoIstanza Is Nothing Then
                    Dim errorMsg As String = "Impossibile leggere lo stato del documento"
                    Log.Error(errorMsg)
                    Throw New Exception(errorMsg)
                Else
                    Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(idDocumento), DllDocumentale.Model.DocumentoInfo)

                    Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
                    If UCase(tipoAtto) = "DETERMINE" Then
                        tipoAtto = "DETERMINA"
                    ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                        tipoAtto = "DISPOSIZIONE"
                    Else
                        tipoAtto = Nothing
                    End If

                    For Each impegnoInfo As Ext_CapitoliAnnoInfo In ImpegniInfo
                        'conferma solo gli impegni da confermare, quelli aventi stato 2
                        If impegnoInfo.Stato = 2 Then
                            ConfermaPreImpegno(impegnoInfo.ID, CodiceUfficio, impegnoInfo.NumPreImp,
                                               statoIstanza, objDocumento, tipoAtto, False)
                            confirmed = confirmed + 1
                        End If
                    Next
                End If
            Else
                Dim errorMsg As String = "Riferimento al documento non specificato o non valido"
                Log.Error(errorMsg)
                Throw New Exception(errorMsg)
            End If
        Catch ex As Exception
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex, IIf(confirmed = 0, -1, -2)), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaPreImpegno")>
    Public Sub ConfermaPreImpegno(ByVal ID As Integer, ByVal CodiceUfficio As String, ByVal numPreImpegno As String, Optional ByVal statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = Nothing, Optional ByVal objDocumento As DllDocumentale.Model.DocumentoInfo = Nothing, Optional ByVal tipoAtto As String = Nothing, Optional ByVal writeResponse As Boolean = True)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim idDocumento As String = CodDocumento()
            If statoIstanza Is Nothing Then
                If Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty Then
                    statoIstanza = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(idDocumento)
                Else
                    Throw New Exception("Riferimento al documento non specificato o non valido")
                End If
            End If

            If Not statoIstanza Is Nothing Then
                Dim ruolo As String = "" & statoIstanza.Ruolo
                If ruolo = "A" Then
                    Throw New Exception("Il provvedimento risulta annullato.")
                End If

                Dim dlldoc As New DllDocumentale.svrDocumenti(operatore)
                'la lista contiene un solo elemento perchè la ricerca è fatta per ID che è unico
                Dim impegni As IList(Of DllDocumentale.ItemImpegnoInfo) = dlldoc.FO_Get_DatiImpegni(, ID)
                'documento di lavoro
                If impegni.Count > 0 Then
                    If objDocumento Is Nothing Then
                        objDocumento = DirectCast(Leggi_Documento_Object(impegni.Item(0).Dli_Documento), DllDocumentale.Model.DocumentoInfo)
                    End If

                    Dim numeroProvvisOrDefAtto As String = ""
                    If objDocumento.Doc_numero = "" then
	                    numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
                    Else
	                    numeroProvvisOrDefAtto = objDocumento.Doc_numero
                    End If

                    If tipoAtto Is Nothing Then
                        tipoAtto = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
                        If UCase(tipoAtto) = "DETERMINE" Then
                            tipoAtto = "DETERMINA"
                        ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                            tipoAtto = "DISPOSIZIONE"
                        Else
                            tipoAtto = Nothing
                        End If
                    End If

                    Dim rispostaGenerazionePreImpegno As String()

                    Dim NumPreImpegno1 As Integer, NumPreImpegno2 As Integer, NumPreImpegno3 As Integer
                    Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer


                    Dim result As String = ""

                    If impegni.Item(0).Dli_Costo <= 0 Then
                        Throw New Exception("Inserire un importo valido")
                    End If
                    
                    Dim hashTokenCallSic As String = GenerateHashTokenCallSic()
                    Dim hashTokenCallSic_Imp As String = GenerateHashTokenCallSic()

                    'Dim hashTokenCallSic As String = impegni.Item(0).HashTokenCallSic
                    'Dim hashTokenCallSic_Imp As String = impegni.Item(0).HashTokenCallSic_Imp

                    If impegni.Item(0).Di_PreImpDaPrenotazione = 0 Then
                        'deve essere generato un nuovo impegno
                        Dim DettaglioCapitolo As Risposta_InterrogazioneBilancio_Types = ClientIntegrazioneSic.MessageMaker.createInterrogazioneBilancioInfoMessage(operatore, impegni.Item(0).Dli_Esercizio, impegni.Item(0).Dli_Cap, CodiceUfficio)

                        If impegni.Item(0).Dli_Costo > DettaglioCapitolo.DispCompetenza Then
                            Throw New Exception("Importo Disponibile insufficiente")
                        End If
                        
                        rispostaGenerazionePreImpegno = ClientIntegrazioneSic.MessageMaker.createGenerazionePreImpegnoMessage(operatore, objDocumento.Doc_Oggetto, impegni.Item(0).Dli_Cap, impegni.Item(0).Dli_Costo, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numeroProvvisorio, 5), impegni.Item(0).Codice_Obbiettivo_Gestionale, idDocumento, numeroProvvisOrDefAtto, hashTokenCallSic)


                        Integer.TryParse(rispostaGenerazionePreImpegno(0), NumPreImpegno1)
                        Integer.TryParse(rispostaGenerazionePreImpegno(1), NumPreImpegno2)
                        Integer.TryParse(rispostaGenerazionePreImpegno(2), NumPreImpegno3)
                        Integer.TryParse(rispostaGenerazionePreImpegno(3), idDocSIC1)
                        Integer.TryParse(rispostaGenerazionePreImpegno(4), idDocSIC2)
                        Integer.TryParse(rispostaGenerazionePreImpegno(5), idDocSIC3)

                        If NumPreImpegno1 > 0 Then
                            result = "{  success: true, NumPreImp: '" + NumPreImpegno1.ToString() + "' }"
                        End If
                    Else
                        'non deve essere generato un nuovo preimpegno ma è necessario accontonare su un preimpegno esistente.
                        Dim DisponibilitaPreImpegno As Risposta_DisponibilitaPreImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaPreImpegnoMessage(operatore, impegni.Item(0).Dli_NPreImpegno)
                        If impegni.Item(0).Dli_Costo > DisponibilitaPreImpegno.Disponibilita Then
                            Throw New Exception("Importo Disponibile insufficiente")
                        End If
                        Dim esito As String = ClientIntegrazioneSic.MessageMaker.createPrenotazionePreImpegnoMessage(operatore, "P", impegni.Item(0).Dli_NPreImpegno, impegni.Item(0).Dli_Costo)
                        If Int(esito) = 0 Then
                            NumPreImpegno1 = impegni.Item(0).Dli_NPreImpegno
                        Else
                            Throw New Exception("Prenotazione Preimpegno non effettuata!")
                        End If
                        result = "{  success: true, NumPreImp: '" + CStr(NumPreImpegno1) + "' }"
                    End If

                    'inserisco i dati nei nostri archivi
                    Try
                        Dim itemBilancio As New DllDocumentale.ItemImpegnoInfo
                        With itemBilancio
                            .HashTokenCallSic = hashTokenCallSic
                            .IdDocContabileSic = idDocSIC1
                            .hashTokenCallSic_Imp = hashTokenCallSic_Imp

                            .Codice_Obbiettivo_Gestionale = impegni.Item(0).Codice_Obbiettivo_Gestionale
                            .Piano_Dei_Conti_Finanziari = impegni.Item(0).Piano_Dei_Conti_Finanziari
                            .DBi_Anno = impegni.Item(0).DBi_Anno
                            .Di_ContoEconomica = impegni.Item(0).Di_ContoEconomica
                            .Di_ImpostaIrap = impegni.Item(0).Di_ImpostaIrap
                            .Di_PreImpDaPrenotazione = impegni.Item(0).Di_PreImpDaPrenotazione
                            .Di_Ratei = impegni.Item(0).Di_Ratei
                            .Di_Risconti = impegni.Item(0).Di_Risconti
                            .Dli_Cap = impegni.Item(0).Dli_Cap
                            .Dli_Esercizio = impegni.Item(0).Dli_Esercizio
                            .Dli_prog = impegni.Item(0).Dli_prog
                            .Dli_UPB = impegni.Item(0).Dli_UPB
                            .Dli_MissioneProgramma = impegni.Item(0).Dli_MissioneProgramma
                            .NDocPrecedente = impegni.Item(0).NDocPrecedente
                            .Dli_Costo = impegni.Item(0).Dli_Costo
                            .Dli_DataRegistrazione = Now
                            .Dli_Documento = objDocumento.Doc_id
                            .Dli_NumImpegno = "0"
                            .Dli_Operatore = operatore.Codice
                            .Dli_NPreImpegno = NumPreImpegno1
                            .Di_Stato = 1
                        End With

                        If (Not dlldoc.FO_Update_Bilancio_E_Liq_PreImpegno(impegni.Item(0).Dli_NPreImpegno, itemBilancio)) Then
                            Throw New Exception("Errore durante l'inserimento dei dati in archivio")
                        End If
                    Catch ex As Exception
                        If impegni.Item(0).Di_PreImpDaPrenotazione = 0 Then
                            Dim numeroAtto As String = IIf(String.IsNullOrEmpty(objDocumento.Doc_numero), objDocumento.Doc_numeroProvvisorio, objDocumento.Doc_numero)

                            ClientIntegrazioneSic.MessageMaker.createEliminazionePreImpMessage(operatore, NumPreImpegno1, tipoAtto, objDocumento.Doc_Data, numeroAtto, objDocumento.Doc_id, numeroProvvisOrDefAtto, GenerateHashTokenCallSic())
                        End If
                        Throw ex
                    End Try

                    If writeResponse Then
                        HttpContext.Current.Response.Write(result)
                    End If
                End If
            Else
                Throw New Exception("Impossibile leggere lo stato del documento")
            End If
        Catch ex As Exception
            Dim extendedEx As Exception = New Exception("Impossibile confermare l'impegno avente pre-impegno n. '" + numPreImpegno + "': '" + ex.Message + "'", ex)
            Log.Error(extendedEx.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(extendedEx), extendedEx.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaMultiplaLiquidazione")>
    Public Sub ConfermaMultiplaLiquidazione(ByVal LiquidazioniInfo As Ext_LiquidazioneInfo())
        Dim confirmed As Integer = 0
        Try
            Dim idDocumento As String = CodDocumento()

            If Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty Then
                Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
                Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(idDocumento)

                If statoIstanza Is Nothing Then
                    Dim errorMsg As String = "Impossibile leggere lo stato del documento"
                    Log.Error(errorMsg)
                    Throw New Exception(errorMsg)
                Else
                    For Each liquidazioneInfo As Ext_LiquidazioneInfo In LiquidazioniInfo
                        'conferma solo le liquidazione da confermare, quelle aventi stato 2
                        If liquidazioneInfo.Stato = 2 Then
                            ConfermaLiquidazione(liquidazioneInfo.ID, liquidazioneInfo.NumPreImp, liquidazioneInfo.NumImpegno)
                            confirmed = confirmed + 1
                        End If
                    Next
                End If
            Else
                Dim errorMsg As String = "Riferimento al documento non specificato o non valido"
                Log.Error(errorMsg)
                Throw New Exception(errorMsg)
            End If
        Catch ex As Exception
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex, IIf(confirmed = 0, -1, -2)), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaLiquidazione")>
    Public Sub ConfermaLiquidazione(ByVal ID As Integer, ByVal numPreImpegno As String, ByVal numImpegno As String, Optional ByVal statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = Nothing)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            If statoIstanza Is Nothing Then
                Dim idDocumento As String = CodDocumento()
                If Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty Then
                    statoIstanza = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(idDocumento)
                Else
                    Throw New Exception("Riferimento al documento non specificato o non valido")
                End If
            End If

            If Not statoIstanza Is Nothing Then
                Dim ruolo As String = "" & statoIstanza.Ruolo
                If ruolo = "A" Then
                    Throw New Exception("Il provvedimento risulta annullato.")
                End If

                Dim dlldoc As New DllDocumentale.svrDocumenti(operatore)
                If ID <> 0 Then

                    Dim numPreImpegnoAsInteger As Integer = -1
                    If Not String.IsNullOrEmpty(numPreImpegno) AndAlso Not Integer.TryParse(numPreImpegno, numPreImpegnoAsInteger) Then
                        Throw New Exception("Numero di preimpegno non valido. Verificare che l'impegno corrispondente sia stato confermato.")
                    End If

                    Dim liquidazioni As List(Of DllDocumentale.ItemLiquidazioneInfo) = dlldoc.FO_Get_DatiLiquidazione(, ID)

                    Dim importoDisponibile As Decimal
                    Dim importoPrenotato As Decimal

                    Double.TryParse(liquidazioni.Item(0).Dli_Costo, importoPrenotato)

                    If String.IsNullOrEmpty(liquidazioni.Item(0).Dli_NumImpegno) OrElse CInt(liquidazioni.Item(0).Dli_NumImpegno) = 0 Then
                        Dim DisponibilitaPreImpegno As Risposta_DisponibilitaPreImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaPreImpegnoMessage(operatore, liquidazioni.Item(0).Dli_NPreImpegno)
                        importoDisponibile = DisponibilitaPreImpegno.Disponibilita

                        'Verifico se tutte l'importo di tutte le liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
                        Dim residuo As Decimal = calcolaResiduaDisponibilitaDelPreimpegno(operatore, liquidazioni.Item(0).Dli_NPreImpegno, liquidazioni.Item(0).Dli_prog)
                        Dim totResiduo As Decimal = residuo - importoPrenotato
                        If (totResiduo) < 0 Then
                            Throw New Exception("Disponibilita insufficiente per la liquidazione di € " & CStr(importoPrenotato) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di €" & CStr(residuo))
                        End If

                    Else
                        Dim DisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, liquidazioni.Item(0).Dli_NumImpegno)
                        importoDisponibile = DisponibilitaImpegno.Disponibilita
                        Dim beneficiarioImpegno As Risposta_DisponibilitaImpegno_TypesBeneficiario() = DisponibilitaImpegno.Beneficiario

                        'Verifico se tutte l'importo di tutte le liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
                        Dim residuo As Decimal = residuaDisponibilitaDaImpegno(operatore, liquidazioni.Item(0).Dli_NumImpegno, importoDisponibile)
                        Dim totResiduo As Decimal = residuo - importoPrenotato
                        If (totResiduo) < 0 Then
                            Throw New Exception("Disponibilita insufficiente per la liquidazione di € " & CStr(importoPrenotato) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di €" & CStr(residuo))
                        End If

                    End If

                    'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
                    If importoPrenotato > 0 Then
                        Dim itemLiquidazione As DllDocumentale.ItemLiquidazioneInfo = liquidazioni.Item(0)

                        With itemLiquidazione
                            .HashTokenCallSic = GenerateHashTokenCallSic()
                            .Di_Stato = 1
                        End With
                        dlldoc.FO_Update_Liquidazione(itemLiquidazione)
                    End If
                Else
                    Throw New Exception("Nessuna Liquidazione selezionata")
                End If
            Else
                Throw New Exception("Impossibile leggere lo stato del documento")
            End If
        Catch ex As Exception
            Dim extendedEx As Exception = Nothing
            If Not numPreImpegno Is Nothing AndAlso numPreImpegno.Trim() <> String.Empty Then
                extendedEx = New Exception("Impossibile confermare la liquidazione avente pre-impegno n. '" + numPreImpegno + "': '" + ex.Message + "'", ex)
            ElseIf Not numImpegno Is Nothing AndAlso numImpegno.Trim() <> String.Empty Then
                extendedEx = New Exception("Impossibile confermare la liquidazione avente impegno n. '" + numImpegno + "': '" + ex.Message + "'", ex)
            Else
                extendedEx = New Exception("Impossibile confermare la liquidazione: '" + ex.Message + "'", ex)
            End If
            Log.Error(extendedEx.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(extendedEx), extendedEx.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaMultiplaRiduzione")>
    Public Sub ConfermaMultiplaRiduzione(ByVal RiduzioniInfo As Ext_CapitoliAnnoInfo())
        Dim confirmed As Integer = 0
        Try
            For Each riduzioneInfo As Ext_CapitoliAnnoInfo In RiduzioniInfo
                'conferma solo le riduzioni da confermare, quelle aventi stato 2
                If riduzioneInfo.Stato = 2 Then
                    ConfermaRiduzione(riduzioneInfo.ID, riduzioneInfo.NumImpegno)
                    confirmed = confirmed + 1
                End If
            Next
        Catch ex As Exception
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex, IIf(confirmed = 0, -1, -2)), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaRiduzione")>
    Public Sub ConfermaRiduzione(ByVal ID As Integer, ByVal numImpegno As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim itemRiduzione As List(Of DllDocumentale.ItemRiduzioneInfo) = dllDoc.FO_Get_DatiImpegniVariazioni(, ID)
            'valori dela richiesta
            Dim tipoAtto As String

            If UCase(itemRiduzione.Item(0).Div_TipoAssunzione) = "DETERMINA" Then
                tipoAtto = "0"
            ElseIf UCase(itemRiduzione.Item(0).Div_TipoAssunzione) = "DISPOSIZIONE" Then
                tipoAtto = "2"
            Else
                tipoAtto = "1"
            End If


            Dim DisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, itemRiduzione.Item(0).Dli_NumImpegno)
            Dim ImportoDisponibile As Long = DisponibilitaImpegno.Disponibilita

            Dim beneficiarioImpegno As Risposta_DisponibilitaImpegno_TypesBeneficiario() = DisponibilitaImpegno.Beneficiario

            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaDaImpegno(operatore, itemRiduzione.Item(0).Dli_NumImpegno, ImportoDisponibile)
            Dim totResiduo As Decimal = residuo - itemRiduzione.Item(0).Dli_Costo
            If (totResiduo) < 0 Then
                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(itemRiduzione.Item(0).Dli_Costo) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
            End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If itemRiduzione.Item(0).Dli_Costo > 0 Then
                With itemRiduzione.Item(0)
                    .HashTokenCallSic = GenerateHashTokenCallSic()
                    .Dli_DataRegistrazione = Now
                    .Di_Stato = 1
                End With
                Registra_Riduzione(operatore, itemRiduzione.Item(0))
            End If

        Catch ex As Exception
            Dim extendedEx As Exception = Nothing
            If Not numImpegno Is Nothing AndAlso numImpegno.Trim() <> String.Empty Then
                extendedEx = New Exception("Impossibile confermare la riduzione avente impegno n. '" + numImpegno + "': '" + ex.Message + "'", ex)
            Else
                extendedEx = New Exception("Impossibile confermare la riduzione: '" + ex.Message + "'", ex)
            End If
            Log.Error(extendedEx.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(extendedEx), extendedEx.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzionePreImpUP")>
    Public Sub GenerazioneRiduzionePreImpUP()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'valori dela richiesta
            Dim bilancio As String = HttpContext.Current.Request.Item("Bilancio")
            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            Dim importo As String = HttpContext.Current.Request.Item("ImpDaRidurrePreImp")
            Dim upb As String = HttpContext.Current.Request.Item("UPB")
            Dim missioneProgramma As String = HttpContext.Current.Request.Item("MissioneProgramma")
            Dim NumPreImp As String = CLng(HttpContext.Current.Request.Item("ComboPreImpRid"))
            Dim dataAtto As String = HttpContext.Current.Request.Item("DataAtto")
            Dim tipoAtto As String = HttpContext.Current.Request.Item("TipoAtto")
            Dim numAtto As String = HttpContext.Current.Request.Item("NumeroAtto")
            

            If UCase(tipoAtto) = "DETERMINA" Then
                tipoAtto = "0"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONE" Then
                tipoAtto = "2"
            Else
                tipoAtto = "1"
            End If

            Dim importoDisponibilePreImp As Decimal
            Dim lstr_impDisp As String = HttpContext.Current.Request.Item("ImpDisp")
            lstr_impDisp = Trim(lstr_impDisp.Replace("€", "").Replace(".", ""))
            Decimal.TryParse(lstr_impDisp, importoDisponibilePreImp)

            Dim importoDaRidurre As Decimal
            Decimal.TryParse(importo, importoDaRidurre)

            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaPreimpegno(operatore, NumPreImp, importoDisponibilePreImp)
            Dim totResiduo As Decimal = residuo - importoDaRidurre
            If (totResiduo) < 0 Then
                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(importoDaRidurre) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
            End If

            'If (totResiduo) = 0 Then
            '    Throw New Exception("Non è possibile ridurre a ZERO. Per una riduzione totale è necessario scegliere dalle Operazioni Contabili la Rettifica Contabile e selezionare la voce 'Cancellazione Documento Contabile'.")
            'End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If Not String.IsNullOrEmpty(importo) Then
                Dim itemRiduzione As DllDocumentale.ItemRiduzioneInfo = New DllDocumentale.ItemRiduzioneInfo
                With itemRiduzione
                    .HashTokenCallSic = GenerateHashTokenCallSic()
                    .Dli_Documento = CodDocumento()
                    .Dli_DataRegistrazione = Now
                    .Dli_Operatore = operatore.Codice
                    .Dli_Esercizio = bilancio
                    .Dli_UPB = upb
                    .Dli_MissioneProgramma = missioneProgramma
                    .Dli_Cap = capitolo
                    .Dli_Costo = importo
                    .Dli_NumImpegno = CStr(NumPreImp)
                    .DBi_Anno = bilancio
                    .Dli_NPreImpegno = NumPreImp
                    .Div_TipoAssunzione = tipoAtto
                    .Div_Data_Assunzione = dataAtto
                    .Div_Num_assunzione = numAtto
                    'Lu Rid
                    .Di_Stato = 1
                End With
                Registra_RiduzionePreImp(operatore, itemRiduzione)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    Public Shared Function residuaDisponibilitaPreimpegno(ByVal operatore As DllAmbiente.Operatore, ByVal numPreim As String, ByVal ImportoPreImpegno As Double, Optional ByVal prog As Long = 0) As Decimal

        Log.Debug(operatore.Codice)

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista_RidPreImp As IList = dllDoc.FO_Get_ListaRiduzioniDaPreImpegno(numPreim)
        Dim totale_PreImp As Decimal = 0
        For Each itemPreImpVar As DllDocumentale.ItemRiduzioneInfo In lista_RidPreImp
            If prog <> 0 Then
                If itemPreImpVar.Dli_prog <> prog Then
                    totale_PreImp = totale_PreImp + itemPreImpVar.Dli_Costo
                End If
            Else
                totale_PreImp = totale_PreImp + itemPreImpVar.Dli_Costo
            End If
        Next

        Return ImportoPreImpegno - totale_PreImp

    End Function
    Private Sub Registra_RiduzionePreImp(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneInfo)
        Try
            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            If itemRiduzione.Dli_prog > 0 Then
                itemRiduzione = dllDoc.FO_Update_Preimpegno_Var(itemRiduzione)
            Else
                itemRiduzione = dllDoc.FO_Insert_Preimpegno_Var(itemRiduzione)
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    Private Function trasformaListaRidInfoInExt(ByVal riduzioniRegistrate As List(Of DllDocumentale.ItemRiduzioneInfo), ByRef listaRiduzioniExtDaRestituire As List(Of Ext_CapitoliAnnoInfo))

        Dim itemExt_Riduzione As Ext_CapitoliAnnoInfo
        For Each riduzione As DllDocumentale.ItemRiduzioneInfo In riduzioniRegistrate
            itemExt_Riduzione = New Ext_CapitoliAnnoInfo
            With itemExt_Riduzione
                '.AnnoPrenotazione = impegno.DBi_Anno
                .Tipo = " "
                .ID = riduzione.Dli_prog
                .Bilancio = riduzione.Dli_Esercizio
                .Capitolo = riduzione.Dli_Cap
                .ImpPrenotato = riduzione.Dli_Costo
                .NumPreImp = riduzione.Dli_NPreImpegno
                .UPB = riduzione.Dli_UPB
                .MissioneProgramma = riduzione.Dli_MissioneProgramma
                .ID = riduzione.Dli_prog
                .NumImpegno = IIf(riduzione.Dli_NumImpegno = "0", "", riduzione.Dli_NumImpegno)
                .RegistratoSic = IIf(("" & riduzione.Div_NumeroReg = "") Or (String.IsNullOrEmpty(riduzione.Div_NumeroReg)), False, True)
                .TipoAtto = IIf(riduzione.Div_TipoAssunzione = 0, "DETERMINA", "DELIBERA")
                .NumeroAtto = riduzione.Div_Num_assunzione
                .DataAtto = riduzione.Div_Data_Assunzione
                
                .IsEconomia = riduzione.Div_IsEconomia
                .Stato = riduzione.Di_Stato
                .HashTokenCallSic = riduzione.HashTokenCallSic
                .IdDocContabileSic = riduzione.IdDocContabileSic
            End With

            listaRiduzioniExtDaRestituire.Add(itemExt_Riduzione)
        Next
        Return listaRiduzioniExtDaRestituire
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaRiduzionePreImp")>
    Public Function EliminaRiduzionePreImp(ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemRiduzione As New DllDocumentale.ItemRiduzioneInfo

            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If
            itemRiduzione.Dli_prog = ID
            itemRiduzione.Dli_Documento = CodDocumento()

            Elimina_RiduzionePreImp(operatore, itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaRiduzionePreImp")>
    Public Sub ConfermaRiduzionePreImp(ByVal ID As Integer)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim itemRiduzione As List(Of DllDocumentale.ItemRiduzioneInfo) = dllDoc.FO_Get_DatiPreImpegniVariazioni(, ID)
            'valori dela richiesta
            Dim tipoAtto As String

            If UCase(itemRiduzione.Item(0).Div_TipoAssunzione) = "DETERMINA" Then
                tipoAtto = "0"
            ElseIf UCase(itemRiduzione.Item(0).Div_TipoAssunzione) = "DISPOSIZIONE" Then
                tipoAtto = "2"
            Else
                tipoAtto = "1"
            End If


            Dim DisponibilitaPreImpegno As Risposta_DisponibilitaPreImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaPreImpegnoMessage(operatore, itemRiduzione.Item(0).Dli_NPreImpegno)
            Dim ImportoDisponibile As Long = DisponibilitaPreImpegno.Disponibilita

            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaPreimpegno(operatore, itemRiduzione.Item(0).Dli_NPreImpegno, ImportoDisponibile)
            Dim totResiduo As Decimal = residuo - itemRiduzione.Item(0).Dli_Costo
            If (totResiduo) < 0 Then
                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(itemRiduzione.Item(0).Dli_Costo) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
            End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If itemRiduzione.Item(0).Dli_Costo > 0 Then
                With itemRiduzione.Item(0)
                    .HashTokenCallSic = GenerateHashTokenCallSic()
                    .Dli_DataRegistrazione = Now
                    .Di_Stato = 1
                End With
                Registra_RiduzionePreImp(operatore, itemRiduzione.Item(0))
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzionePreImp")>
    Public Sub GenerazioneRiduzionePreImp()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)

            Dim dipartimento As String = objDocumento.Doc_Cod_Uff_Pubblico

            Dim riduzioniRagioneria As String = HttpContext.Current.Request.Item("RiduzioniPreImpRagioneria")
            riduzioniRagioneria = "[" & riduzioniRagioneria & "]"
            Dim riduzioni As List(Of Ext_CapitoliAnnoInfo) = DirectCast(JavaScriptConvert.DeserializeObject(riduzioniRagioneria, GetType(List(Of Ext_CapitoliAnnoInfo))), List(Of Ext_CapitoliAnnoInfo))
            Dim datamovimento As String = HttpContext.Current.Request.Item("DataMovimentoRidPreImp")

            'valori dela richiesta
            Dim hashTokenCallSic As String = riduzioni.Item(0).HashTokenCallSic
            Dim importo As String = riduzioni.Item(0).ImpPrenotato
            Dim capitolo As String = riduzioni.Item(0).Capitolo
            Dim upb As String = riduzioni.Item(0).UPB
            Dim missioneProgramma As String = riduzioni.Item(0).MissioneProgramma
            Dim esercizio As String = riduzioni.Item(0).Bilancio


            Dim id As Long
            Long.TryParse(riduzioni.Item(0).ID, id)

            If String.IsNullOrEmpty(riduzioni.Item(0).NumPreImp) AndAlso (riduzioni.Item(0).NumPreImp = "0") Then
                Throw New Exception("Specificare il numero di preimpegno da ridurre")
            End If

            Dim dataAttoAss As String = riduzioni.Item(0).DataAtto
            Dim tipoAttoAss As String = riduzioni.Item(0).TipoAtto
            Dim numAttoAss As String = riduzioni.Item(0).NumeroAtto


            Dim NumPreImp As Integer
            NumPreImp = CInt(riduzioni.Item(0).NumPreImp)

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            Else
                tipoAtto = Nothing
            End If
            'Lu Rid inviare un importo Negativo se isEconomia = 0
            

            Dim importoDouble As Double = 0
            importoDouble = -Double.Parse(importo)

            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
                numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
                numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            Dim rispostaGenerazioneRiduzione As String () = ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazionePreIMPMessage(operatore, NumPreImp, importoDouble, tipoAtto, objDocumento.Doc_Data, Right(objDocumento.Doc_numero, 5), datamovimento, objDocumento.Doc_Oggetto, objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)

            Dim varPreImp1 As Integer, varPreImp2 As Integer, varPreImp3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer
				
				
            Integer.TryParse(rispostaGenerazioneRiduzione(0), varPreImp1)
            Integer.TryParse(rispostaGenerazioneRiduzione(1), varPreImp2)
            Integer.TryParse(rispostaGenerazioneRiduzione(2), varPreImp3)
            Integer.TryParse(rispostaGenerazioneRiduzione(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazioneRiduzione(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazioneRiduzione(5), idDocSIC3)

            Dim NumRegistrazione As Long = 0
            If IsNumeric(rispostaGenerazioneRiduzione) Then
                Long.TryParse(varPreImp1, NumRegistrazione)
                HttpContext.Current.Response.Write("{  success: true }")
            End If

            ''chiamata ad insert_item_bilancio
            Dim itemRagRiduzione As New DllDocumentale.ItemRiduzioneInfo
            With itemRagRiduzione
                .HashTokenCallSic = hashTokenCallSic
                .IdDocContabileSic = idDocSIC1
                .DBi_Anno = esercizio
                .Dli_Cap = capitolo
                .Dli_Costo = importo
                .Dli_DataRegistrazione = datamovimento
                .Dli_Documento = objDocumento.Doc_id
                .Dli_Esercizio = esercizio
                .Dli_NPreImpegno = NumPreImp
                .Dli_Operatore = operatore.Codice
                .Dli_UPB = upb
                .Dli_MissioneProgramma = missioneProgramma
                .Dli_Documento = objDocumento.Doc_id
                .Div_NumeroReg = varPreImp1
                .Div_TipoAssunzione = IIf(tipoAttoAss = "DETERMINA", 0, 2)
                .Div_Num_assunzione = numAttoAss
                .Div_Data_Assunzione = datamovimento
                .Dli_prog = id
                .Di_Stato = 1
            End With

            Aggiorna_RiduzionePreImp(operatore, itemRagRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaLiquidazioniAperte?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetListaLiquidazioniAperte(ByVal AnnoRif As String, ByVal CapitoloRif As String) As IList(Of Ext_LiquidazioneInfo)

        Try
            Dim liquidazioni As New System.Collections.Generic.List(Of Ext_LiquidazioneInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneLiquidazioni As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneLiquidazioniAperteMessage(operatore, AnnoRif, CapitoloRif)
            '-1
            For i As Integer = 0 To UBound(rispostaInterrogazioneLiquidazioni, 1)
                Dim liqRestituito As New Ext_LiquidazioneInfo

                liqRestituito.NLiquidazione = DirectCast(rispostaInterrogazioneLiquidazioni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneLiquidazioniAperte_TypesLiquidazione).Numero
                liqRestituito.ImpDisp = DirectCast(rispostaInterrogazioneLiquidazioni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneLiquidazioniAperte_TypesLiquidazione).ImportoResiduo
                liqRestituito.Oggetto_Impegno = DirectCast(rispostaInterrogazioneLiquidazioni(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneLiquidazioniAperte_TypesLiquidazione).Oggetto
                'verifica il potenziale importo, cioè tenendo conto di eventuali riduzioni/liquidazioni sull' impegno (non registrate sul sic) di atti in itinere
                liqRestituito.ImpPotenzialePrenotato = residuaDisponibilitaLiquidazione(operatore, liqRestituito.NLiquidazione, liqRestituito.ImpDisp)

                liquidazioni.Add(liqRestituito)
            Next
            Return liquidazioni
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_LiquidazioneInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaImpegniPerentiAperti?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetListaImpegniPerentiAperti(ByVal AnnoRif As String, ByVal CapitoloRif As String) As IList(Of Ext_PerentiInfo)

        Try
            Dim impegniPerenti As New System.Collections.Generic.List(Of Ext_PerentiInfo)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneImpegniPerenti As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneImpegniPerentiApertiMessage(operatore, AnnoRif, CapitoloRif)
            '-1
            For i As Integer = 0 To UBound(rispostaInterrogazioneImpegniPerenti, 1)
                Dim impRestituito As New Ext_PerentiInfo
                impRestituito.NumImpegno = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).Numero
                impRestituito.ImpDisp = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).ImportoResiduo
                impRestituito.ImportoOriginario = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).ImportoOriginario
                impRestituito.CapitoloOriginario = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).CapitoloOriginario
                impRestituito.Oggetto_Impegno = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).Oggetto
                'verifica il potenziale importo, cioè tenendo conto di eventuali riduzioni/liquidazioni sull' impegno (non registrate sul sic) di atti in itinere
                impRestituito.ImpPotenzialePrenotato = residuaDisponibilitaDaPerente(operatore, impRestituito.NumImpegno, impRestituito.ImpDisp)
                impRestituito.PianoDeiContiFinanziario = DirectCast(rispostaInterrogazioneImpegniPerenti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneImpegniPerenti_TypesImpegnoPerente).PCF.Codice
                impegniPerenti.Add(impRestituito)


                '****** Il caricamento del relativo beneficiario è stato spostato alla selezione dell'impegno da interfaccia ****** 
                ' Dim DisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, impRestituito.NumImpegno)
                ' Dim listaBeneficiariImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario() = DisponibilitaImpegno.Beneficiario

                'impRestituito.ListaBeneficiari = New List(Of Ext_AnagraficaInfo)
                'impRestituito.Beneficiario = New Ext_AnagraficaInfo

                'If Not listaBeneficiariImpegnoSIC Is Nothing Then
                'For Each beneficiarioImpegnoSIC As Risposta_DisponibilitaImpegno_TypesBeneficiario In listaBeneficiariImpegnoSIC
                'Dim beneficiarioImpegno As New Ext_AnagraficaInfo
                'beneficiarioImpegno = beneficiarioImpegno.TransformDisponibilitaImpegnoBeneficiarioInExtObj(beneficiarioImpegnoSIC)

                'impRestituito.Beneficiario = beneficiarioImpegno
                'impRestituito.ListaBeneficiari.Add(beneficiarioImpegno)
                'Next
                ' End If

            Next
            Return impegniPerenti
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_PerentiInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaMultiplaImpegnoPerente")>
    Public Sub ConfermaMultiplaImpegnoPerente(ByVal ImpegniPerentiInfo As Ext_CapitoliAnnoInfo())
        Dim confirmed As Integer = 0
        Try
            For Each impegnoPerenteInfo As Ext_CapitoliAnnoInfo In ImpegniPerentiInfo
                'conferma gli impegni perenti da confermare, quelle aventi stato 2
                If impegnoPerenteInfo.Stato = 2 Then
                    ConfermaImpegnoPerente(impegnoPerenteInfo.ID, impegnoPerenteInfo.NumPreImp)
                    confirmed = confirmed + 1
                End If
            Next
        Catch ex As Exception
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex, IIf(confirmed = 0, -1, -2)), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaImpegnoPerente")>
    Public Sub ConfermaImpegnoPerente(ByVal ID As Integer, ByVal numPreImp As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim dlldoc As New DllDocumentale.svrDocumenti(operatore)
            If ID <> 0 Then

                Dim perente As List(Of DllDocumentale.ItemImpegnoInfo) = dlldoc.FO_Get_DatiImpegniPerenti(, ID)

                'documento di lavoro
                Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(perente.Item(0).Dli_Documento), DllDocumentale.Model.DocumentoInfo)

                Dim importoPrenotato As Decimal
                Double.TryParse(perente.Item(0).Dli_Costo, importoPrenotato)

                'Verifica dell'esistena dell'impegno in perenzione
                Dim rispostaDisponibilitaImpegno As Risposta_DisponibilitaImpegno_Types = ClientIntegrazioneSic.MessageMaker.createDisponibilitaImpegnoMessage(operatore, perente.Item(0).NDocPrecedente)
                Dim importoDisponibileImpegnoPerente As Decimal
                Decimal.TryParse(rispostaDisponibilitaImpegno.Disponibilita, importoDisponibileImpegnoPerente)

                Dim beneficiarioImpegno As Risposta_DisponibilitaImpegno_TypesBeneficiario() = rispostaDisponibilitaImpegno.Beneficiario

                Dim residuo As Decimal = residuaDisponibilitaDaPerente(operatore, "P" & perente.Item(0).NDocPrecedente, importoDisponibileImpegnoPerente)
                Dim totResiduo As Decimal = residuo - importoPrenotato
                If totResiduo < 0 Then
                    Throw New Exception("Importo sull'impegno Perente  insufficiente, residuo " & CStr(residuo))
                End If
                Dim itemBilancio As DllDocumentale.ItemImpegnoInfo = perente.Item(0)
                With itemBilancio
                    .HashTokenCallSic_Imp = GenerateHashTokenCallSic()
                    .Di_Stato = 1
                End With
                Dim item As DllDocumentale.ItemLiquidazioneInfo = Registra_ImpegnoSuPerenteEliquidazione(operatore, itemBilancio)

            End If

        Catch ex As Exception
            Dim extendedEx As Exception = Nothing
            If Not numPreImp Is Nothing AndAlso numPreImp.Trim() <> String.Empty Then
                extendedEx = New Exception("Impossibile confermare l'impegno perente avente prenotazione n. '" + numPreImp + "': '" + ex.Message + "'", ex)
            Else
                extendedEx = New Exception("Impossibile confermare l'impegno perente: '" + ex.Message + "'", ex)
            End If
            Log.Error(extendedEx.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(extendedEx), extendedEx.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaTipoRettifiche?tipoDocumento={tipoDocumento}")>
    Public Function GetListaTipoRettifiche(ByVal tipoDocumento As Integer) As IList(Of Ext_TipoBase)

        Try
            Dim rettif As New System.Collections.Generic.List(Of Ext_TipoBase)
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim objDll As New DllDocumentale.svrDocumenti(operatore)
            Dim listaItem As IList(Of DllDocumentale.ItemTipoBase)
            listaItem = objDll.ListaTipoOperazioniRettifiche(tipoDocumento)
            Dim itemRetti As Ext_TipoBase
            If Not listaItem Is Nothing Then
                For Each itemBase As DllDocumentale.ItemTipoBase In listaItem
                    itemRetti = New Ext_TipoBase
                    itemRetti.Descrizione = itemBase.Descrizione
                    itemRetti.Id = itemBase.Id
                    rettif.Add(itemRetti)
                Next
            End If

            Return rettif

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetRiduzioniLiquidazioniRegistrate")>
    Public Function GetRiduzioniLiquidazioniRegistrate() As IList(Of Ext_LiquidazioneInfo)
        Dim listaRiduzioniExtDaRestituire As New List(Of Ext_LiquidazioneInfo)
        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim riduzioniLiqRegistrate As System.Collections.Generic.List(Of DllDocumentale.ItemRiduzioneLiqInfo)
            riduzioniLiqRegistrate = Get_RiduzioniLiquidazioni(operatore)
            Dim itemExt_Riduzione As Ext_LiquidazioneInfo
            For Each riduzione As DllDocumentale.ItemRiduzioneLiqInfo In riduzioniLiqRegistrate
                itemExt_Riduzione = New Ext_LiquidazioneInfo
                With itemExt_Riduzione
                    '.AnnoPrenotazione = impegno.DBi_Anno
                    .HashTokenCallSic = riduzione.HashTokenCallSic
                    .IdDocContabileSic = riduzione.IdDocContabileSic
                    .Tipo = " "
                    .ID = riduzione.Dli_prog
                    .Bilancio = riduzione.Dli_Esercizio
                    .Capitolo = riduzione.Dli_Cap
                    .ImpPrenotato = riduzione.Dli_Costo
                    .NumPreImp = riduzione.Dli_NPreImpegno
                    .UPB = riduzione.Dli_UPB
                    .MissioneProgramma = riduzione.Dli_MissioneProgramma
                    .ID = riduzione.Dli_prog
                    .NumImpegno = IIf(riduzione.Dli_NumImpegno = "0", "", riduzione.Dli_NumImpegno)
                    .RegistratoSic = IIf(("" & riduzione.Div_NumeroReg = "") Or (String.IsNullOrEmpty(riduzione.Div_NumeroReg)), False, True)
                    .Stato = riduzione.Di_Stato
                    .NLiquidazione = riduzione.Div_NLiquidazione
                End With

                listaRiduzioniExtDaRestituire.Add(itemExt_Riduzione)
            Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaRiduzioniExtDaRestituire
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaRiduzioneLiq")>
    Public Function EliminaRiduzioneLiq(ByVal ID As String) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim itemRiduzione As New DllDocumentale.ItemRiduzioneLiqInfo

            If String.IsNullOrEmpty(CodDocumento) Then
                Throw New Exception("impossibile trovare documento associato la preimpegno")
            End If
            itemRiduzione.Dli_prog = ID
            itemRiduzione.Dli_Documento = CodDocumento()
            Elimina_RiduzioneLiq(operatore, itemRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaMultiplaRiduzioneLiq")>
    Public Sub ConfermaMultiplaRiduzioneLiq(ByVal RiduzioniLiqInfo As Ext_LiquidazioneInfo())
        Dim confirmed As Integer = 0
        Try
            For Each liquidazioneInfo As Ext_LiquidazioneInfo In RiduzioniLiqInfo
                'conferma le riduzioni liquidazioni da confermare, quelle aventi stato 2
                If liquidazioneInfo.Stato = 2 Then
                    ConfermaRiduzioneLiq(liquidazioneInfo.ID, liquidazioneInfo.NLiquidazione)
                    confirmed = confirmed + 1
                End If
            Next
        Catch ex As Exception
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex, IIf(confirmed = 0, -1, -2)), ex.Message)
        End Try
    End Sub

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/ConfermaRiduzioneLiq")>
    Public Sub ConfermaRiduzioneLiq(ByVal ID As Integer, ByVal NLiquidazione As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim itemRiduzione As List(Of DllDocumentale.ItemRiduzioneLiqInfo) = dllDoc.FO_Get_DatiLiquidazioniVariazioni(, ID)
            'valori dela richiesta

            Dim ImportoDisponibile As Long = -1
            Dim ListaLiq As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneLiquidazioniAperteMessage(operatore, itemRiduzione.Item(0).Dli_Esercizio, itemRiduzione.Item(0).Dli_Cap)
            For Each liq As Risposta_InterrogazioneLiquidazioniAperte_TypesLiquidazione In ListaLiq
                If liq.Numero = itemRiduzione.Item(0).Div_NLiquidazione Then
                    ImportoDisponibile = liq.ImportoResiduo
                End If
            Next
            If ImportoDisponibile < 0 Then
                Throw New Exception("Liquidazione n." & itemRiduzione.Item(0).Div_NLiquidazione & " non è stata trovata, controllare i dati inseriti ")
            End If
            If ImportoDisponibile = 0 Then
                Throw New Exception("Disponibilità insufficiente per la liquidazione n." & itemRiduzione.Item(0).Div_NLiquidazione & ", residuo " & ImportoDisponibile)
            End If
            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaLiquidazione(operatore, itemRiduzione.Item(0).Div_NLiquidazione, ImportoDisponibile)
            Dim totResiduo As Decimal = residuo - itemRiduzione.Item(0).Dli_Costo
            If (totResiduo) < 0 Then
                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(itemRiduzione.Item(0).Dli_Costo) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
            End If

            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If itemRiduzione.Item(0).Dli_Costo > 0 Then
                With itemRiduzione.Item(0)
                    .HashTokenCallSic = GenerateHashTokenCallSic()
                    .Dli_DataRegistrazione = Now
                    .Di_Stato = 1
                End With
                Registra_RiduzioneLiq(operatore, itemRiduzione.Item(0))
            End If

        Catch ex As Exception
            Dim extendedEx As Exception = Nothing
            If Not NLiquidazione Is Nothing AndAlso NLiquidazione.Trim() <> String.Empty Then
                extendedEx = New Exception("Impossibile confermare la riduzione della liquidazione n. '" + NLiquidazione + "': '" + ex.Message + "'", ex)
            Else
                extendedEx = New Exception("Impossibile confermare la riduzione della liquidazione: '" + ex.Message + "'", ex)
            End If
            Log.Error(extendedEx.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(extendedEx), extendedEx.Message)
        End Try
    End Sub

    Function residuaDisponibilitaLiquidazione(ByVal operatore As DllAmbiente.Operatore, ByVal numLiq As String, ByVal ImportoLiquidazione As Double, Optional ByVal prog As Long = 0) As Decimal

        Log.Debug(operatore.Codice)

        Dim idoc As String = CodDocumento()
        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        Dim lista_RidLiq As IList = dllDoc.FO_Get_ListaRiduzioniLiquidazioniByNLiquidazione(numLiq)
        Dim totale_RidLiq As Decimal = 0
        For Each itemLiqVar As DllDocumentale.ItemRiduzioneLiqInfo In lista_RidLiq
            If prog <> 0 Then
                If itemLiqVar.Dli_prog <> prog Then
                    totale_RidLiq = totale_RidLiq + itemLiqVar.Dli_Costo
                End If
            Else
                totale_RidLiq = totale_RidLiq + itemLiqVar.Dli_Costo
            End If
        Next

        Return ImportoLiquidazione - totale_RidLiq

    End Function
    Private Sub Registra_RiduzioneLiq(ByVal operatore As DllAmbiente.Operatore, ByRef itemRiduzione As DllDocumentale.ItemRiduzioneLiqInfo)
        Try
            Log.Debug(operatore.Codice)


            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            If itemRiduzione.Dli_prog > 0 Then
                itemRiduzione = dllDoc.FO_Update_Liquidazione_Var(itemRiduzione)
            Else
                itemRiduzione = dllDoc.FO_Insert_Liquidazione_Var(itemRiduzione)
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzioneLiqUP")>
    Public Sub GenerazioneRiduzioneLiqUP()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'valori dela richiesta
            Dim bilancio As String = HttpContext.Current.Request.Item("Bilancio")
            Dim capitolo As String = HttpContext.Current.Request.Item("Capitolo")
            Dim importo As String = HttpContext.Current.Request.Item("ImpDaRidurreLiq")
            Dim upb As String = HttpContext.Current.Request.Item("UPB")
            Dim missioneProgramma As String = HttpContext.Current.Request.Item("MissioneProgramma")
            Dim NLiquidazione As String = CLng(HttpContext.Current.Request.Item("ComboLiqRid"))

            Dim importoDisponibileLiq As Decimal
            Dim lstr_impDisp As String = HttpContext.Current.Request.Item("ImpDisp")
            lstr_impDisp = Trim(lstr_impDisp.Replace("€", "").Replace(".", ""))
            Decimal.TryParse(lstr_impDisp, importoDisponibileLiq)

            Dim importoDaRidurre As Decimal
            Decimal.TryParse(importo, importoDaRidurre)

            'Verifico se l'importo di tutte le riduzioni e liquidazioni che non sono arrivate in ragioneria non supera l'importo disponibile dell'impegno
            Dim residuo As Decimal = residuaDisponibilitaLiquidazione(operatore, NLiquidazione, importoDisponibileLiq)
            Dim totResiduo As Decimal = residuo - importoDaRidurre
            If (totResiduo) < 0 Then
                Throw New Exception("Disponibilita insufficiente per la riduzione di € " & CStr(importoDaRidurre) & ", ci sono altri atti non ancora esecutivi che riducono la disponibilità effettiva, il residuo è di € " & CStr(residuo))
            End If

            If (totResiduo) = 0 Then
                Throw New Exception("Non è possibile ridurre a ZERO. Per una riduzione totale è necessario scegliere dalle Operazioni Contabili la Rettifica Contabile e selezionare la voce 'Cancellazione Documento Contabile'.")
            End If
            Dim hashTokenCallSic_Preimp As String = GenerateHashTokenCallSic()
            'STEFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
            'il preimpegno non viene registrato sul SIC, ma viene controllato che l'importo non sia maggiore della disponibilità
            If Not String.IsNullOrEmpty(importo) Then
                Dim itemRiduzione As DllDocumentale.ItemRiduzioneLiqInfo = New DllDocumentale.ItemRiduzioneLiqInfo
                With itemRiduzione
                    .HashTokenCallSic = hashTokenCallSic_Preimp
                    .Dli_Documento = CodDocumento()
                    .Dli_DataRegistrazione = Now
                    .Dli_Operatore = operatore.Codice
                    .Dli_Esercizio = bilancio
                    .Dli_UPB = upb
                    .Dli_MissioneProgramma = missioneProgramma
                    .Dli_Cap = capitolo
                    .Dli_Costo = importo
                    .Div_NLiquidazione = CStr(NLiquidazione)
                    .Di_Stato = 1
                End With
                Registra_RiduzioneLiq(operatore, itemRiduzione)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GenerazioneRiduzioneLiq")>
    Public Sub GenerazioneRiduzioneLiq()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            'documento di lavoro
            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = DirectCast(Leggi_Documento_Object(CodDocumento), DllDocumentale.Model.DocumentoInfo)
            
            Dim dipartimento As String = objDocumento.Doc_Cod_Uff_Pubblico

            Dim riduzioniRagioneria As String = HttpContext.Current.Request.Item("RiduzioniLiqRagioneria")
            riduzioniRagioneria = "[" & riduzioniRagioneria & "]"
            Dim riduzioni As List(Of Ext_LiquidazioneInfo) = DirectCast(JavaScriptConvert.DeserializeObject(riduzioniRagioneria, GetType(List(Of Ext_LiquidazioneInfo))), List(Of Ext_LiquidazioneInfo))
            Dim datamovimento As String = HttpContext.Current.Request.Item("DataMovimentoRidLiq")

            'valori dela richiesta
            Dim hashTokenCallSic As String = riduzioni.Item(0).HashTokenCallSic
            Dim importo As String = riduzioni.Item(0).ImpPrenotato
            Dim capitolo As String = riduzioni.Item(0).Capitolo
            Dim upb As String = riduzioni.Item(0).UPB
            Dim missioneProgramma As String = riduzioni.Item(0).MissioneProgramma
            Dim esercizio As String = riduzioni.Item(0).Bilancio


            Dim id As Long
            Long.TryParse(riduzioni.Item(0).ID, id)

            If String.IsNullOrEmpty(riduzioni.Item(0).NLiquidazione) AndAlso (riduzioni.Item(0).NLiquidazione = "0") Then
                Throw New Exception("Specificare il numero di liquidazione da ridurre")
            End If


            Dim NLiquidazione As Integer
            NLiquidazione = CInt(riduzioni.Item(0).NLiquidazione)

            Dim tipoAtto As String = objDocumento.TipologiaProvvedimento(objDocumento.Doc_id)
            If UCase(tipoAtto) = "DETERMINE" Then
                tipoAtto = "DETERMINA"
            ElseIf UCase(tipoAtto) = "DISPOSIZIONI" Then
                tipoAtto = "DISPOSIZIONE"
            Else
                tipoAtto = Nothing
            End If


            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
	            numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
	            numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            

            Dim importoDouble As Double = 0
            importoDouble = -Double.Parse(importo)
            Dim rispostaGenerazioneRiduzione As String() = ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazioneLiqMessage(operatore, NLiquidazione, importoDouble, datamovimento, objDocumento.Doc_Oggetto, Right(objDocumento.Doc_numero, 5), objDocumento.Doc_id, numeroProvvisOrDefAtto, hashTokenCallSic)

            Dim numVar1 As Integer, numVar2 As Integer, numVar3 As Integer
            Dim idDocSIC1 As Integer, idDocSIC2 As Integer, idDocSIC3 As Integer

            Integer.TryParse(rispostaGenerazioneRiduzione(0), numVar1)
            Integer.TryParse(rispostaGenerazioneRiduzione(1), numVar2)
            Integer.TryParse(rispostaGenerazioneRiduzione(2), numVar3)
            Integer.TryParse(rispostaGenerazioneRiduzione(3), idDocSIC1)
            Integer.TryParse(rispostaGenerazioneRiduzione(4), idDocSIC2)
            Integer.TryParse(rispostaGenerazioneRiduzione(5), idDocSIC3)
            HttpContext.Current.Response.Write("{  success: true }")
            

            ''chiamata ad insert_item_bilancio
            Dim itemRagRiduzione As New DllDocumentale.ItemRiduzioneLiqInfo
            With itemRagRiduzione
                .HashTokenCallSic = hashTokenCallSic
                .IdDocContabileSic = idDocSIC1
                .Dli_Cap = capitolo
                .Dli_Costo = importo
                .Dli_DataRegistrazione = datamovimento
                .Dli_Documento = objDocumento.Doc_id
                .Dli_Esercizio = esercizio
                .Div_NLiquidazione = NLiquidazione
                .Dli_Operatore = operatore.Codice
                .Dli_UPB = upb
                .Dli_MissioneProgramma = missioneProgramma
                .Dli_Documento = objDocumento.Doc_id
                .Div_NumeroReg = numVar1
                .Dli_prog = id
                .Di_Stato = 1
            End With

            Aggiorna_RiduzioneLiq(operatore, itemRagRiduzione)

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetElencoCapitoliAnnoLiquidazione?AnnoRif={AnnoRif}&tipoCapitolo={tipoCapitolo}&CodiceUfficio={codiceUfficio}")>
    Public Function GetElencoCapitoliAnnoLiquidazione(ByVal AnnoRif As String, ByVal tipoCapitolo As DllDocumentale.EnumDocumenti.TipoCapitolo, ByVal codiceUfficio As String) As IList(Of Ext_CapitoliAnnoInfo)
        Try
            'AnnoRif = "2017"
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)


            Dim capitoli As New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)

            Dim rispostaInterrogazioneCapitoli As Array
            rispostaInterrogazioneCapitoli = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCapitoliLiquidazioniMessage(operatore, AnnoRif, codiceUfficio)
            For i As Integer = 0 To UBound(rispostaInterrogazioneCapitoli, 1)
                Dim capitoloRestituito As New Ext_CapitoliAnnoInfo
                capitoloRestituito.AnnoPrenotazione = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).AnnoBilancio
                capitoloRestituito.Bilancio = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).AnnoBilancio
                capitoloRestituito.Capitolo = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).CodiceCapitolo
                capitoloRestituito.UPB = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).UPB
                capitoloRestituito.MissioneProgramma = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).MissioneProgramma
                capitoloRestituito.ImpDisp = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DisponibilitaCompetenza
                capitoloRestituito.DescrCapitolo = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DescrizioneCapitolo
                capitoloRestituito.ImpPrenotato = 0
                capitoloRestituito.NumPreImp = 0
                capitoloRestituito.CodiceRisposta = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).CodiceRisposta
                capitoloRestituito.DescrizioneRisposta = DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).DescrizioneRisposta
                If capitoloRestituito.CodiceRisposta <> 0 Then
                    Dim a As String = "eccolo"
                End If
                If (DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).Perenti) = "N" Then
                    capitoloRestituito.isPerente = False
                ElseIf (DirectCast(rispostaInterrogazioneCapitoli(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneCapitoli_TypesCapitolo).Perenti) = "S" Then
                    capitoloRestituito.isPerente = True
                End If

                If (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.Perente AndAlso capitoloRestituito.isPerente = True) Or (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.Tutti) Or (tipoCapitolo = DllDocumentale.EnumDocumenti.TipoCapitolo.NonPerente AndAlso capitoloRestituito.isPerente = False) Then
                    capitoli.Add(capitoloRestituito)
                End If
                'capitoli.Add(capitoloRestituito)
            Next
            Return capitoli
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_CapitoliAnnoInfo)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetSuggerimenti?key={key}")>
    Public Function GetSuggerimenti(ByVal key As String) As List(Of Ext_SuggerimentoInfo)
        Try
            Dim suggerimenti As List(Of Ext_SuggerimentoInfo) = Nothing
            If key <> "" Then
                'prendo i suggerimenti relativi ad un atto
                suggerimenti = Elenco_Suggerimenti(key)
            End If
            Return suggerimenti
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaObiettiviGestionali?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetListaObiettiviGestionali(ByVal AnnoRif As String, ByVal CapitoloRif As String) As IList(Of Ext_TipoBase)
        Dim listaCog As New System.Collections.Generic.List(Of Ext_TipoBase)

        Try

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneCog As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneListaCogMessage(operatore, AnnoRif, CapitoloRif)
            'Dim rispostaInterrogazioneCog As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneListaCogMessage(operatore, "2014", CapitoloRif)
            For i As Integer = 0 To UBound(rispostaInterrogazioneCog, 1)
                Dim codiceObiettivo As New Ext_TipoBase
                codiceObiettivo.Id = DirectCast(rispostaInterrogazioneCog(i), ClientIntegrazioneSic.Intema.WS.Risposta.COG_Type).Codice
                codiceObiettivo.Descrizione = DirectCast(rispostaInterrogazioneCog(i), ClientIntegrazioneSic.Intema.WS.Risposta.COG_Type).Descrizione
                listaCog.Add(codiceObiettivo)
            Next





            Return listaCog
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Return New System.Collections.Generic.List(Of Ext_TipoBase)
        Catch ex As Exception
            Log.Error(ex.ToString)
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
            Return listaCog
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/AssegnaCOGePdCF")>
    Public Sub AssegnaCOGePdCF()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            ''valori dela richiesta
            Dim id_prog As String = HttpContext.Current.Request.Item("id_prog")
            Dim valueComboCOG As String = HttpContext.Current.Request.Item("ComboCOG")
            If valueComboCOG = "" Or valueComboCOG.ToLower.Contains("selez") Then
                Throw New Exception("Codice Obiettivo Gestionale non valido.")
            End If
            Dim valueComboPdCF As String = HttpContext.Current.Request.Item("ComboPdCF")
            If valueComboPdCF = "" Or valueComboPdCF.ToLower.Contains("selez") Then
                Throw New Exception("Piano dei Conti Finanziario non valido.")
            End If

            dllDoc.FO_Update_COG_And_PdCF_Impegno(id_prog, valueComboCOG, valueComboPdCF, CodDocumento, operatore.Codice)


        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/AssegnaCOGePdCFPreimpegno")>
    Public Sub AssegnaCOGePdCFPreimpegno()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(CodDocumento)

            Dim ruolo As String = "" & statoIstanza.Ruolo
            If ruolo = "A" Then
                Throw New Exception("Il provvedimento risulta annullato.")
            End If

            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            ''valori dela richiesta
            Dim id_prog As String = HttpContext.Current.Request.Item("id_prog")
            Dim valueComboCOG As String = HttpContext.Current.Request.Item("ComboCOG")
            If valueComboCOG = "" Or valueComboCOG.ToLower.Contains("selez") Then
                Throw New Exception("Codice Obiettivo Gestionale non valido.")
            End If
            Dim valueComboPdCF As String = HttpContext.Current.Request.Item("ComboPdCF")
            If valueComboPdCF = "" Or valueComboPdCF.ToLower.Contains("selez") Then
                Throw New Exception("Piano dei Conti Finanziario non valido.")
            End If

            dllDoc.FO_Update_COG_And_PdCF_Preimpegno(id_prog, valueComboCOG, valueComboPdCF, CodDocumento, operatore.Codice)

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetAttributi?key={key}")>
    Public Function GetAttributi(ByVal key As String) As List(Of Ext_AttributoInfo)
        Try

            Dim attributi As List(Of Ext_AttributoInfo) = Nothing
            If key <> "" Then
                'prendo gli attributi relativi ad un atto
                attributi = Elenco_Attributi(key)
            Else
                attributi = Elenco_Attributi()
            End If
            Return attributi
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetInfoLeggeTrasparenza")>
    Public Function GetInfoLeggeTrasparenza(ByRef IdDocumento As String) As Ext_SchedaLeggeTrasparenzaInfo
        Try
            Dim result As String = ""
            If IdDocumento Is Nothing Then
                IdDocumento = HttpContext.Current.Request.Item("IdDocumento")
            End If

            Dim infoTrasparenza As Ext_SchedaLeggeTrasparenzaInfo = Nothing
            If IdDocumento <> "" Then
                Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
                If Not operatore Is Nothing Then
                    infoTrasparenza = GetDatiSchedaLeggeTrasparenza(operatore, IdDocumento)
                Else
                    Throw New Exception("Impossibile recuperare le informazioni sull'operatore loggato")
                End If
            End If
            Return infoTrasparenza
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/InterrogaMandatiBeneficiariAtto")>
    Public Function InterrogaMandatiBeneficiariAtto() As System.Collections.Generic.List(Of Ext_MandatoAttoInfo)
        Dim mandati As New System.Collections.Generic.List(Of Ext_MandatoAttoInfo)
        Try
            Dim tipoAtto As String = HttpContext.Current.Request.Item("tipoAtto")
            If Not String.IsNullOrEmpty(tipoAtto) Then
                tipoAtto = HttpContext.Current.Request.Params("tipoAtto")
            Else
                tipoAtto = ""
            End If
            Dim numeroAtto As String = HttpContext.Current.Request.Item("numeroAtto")
            If Not String.IsNullOrEmpty(numeroAtto) Then
                numeroAtto = HttpContext.Current.Request.Params("numeroAtto")
            Else
                numeroAtto = ""
            End If
            Dim dataAtto As Date
            If IsDate(HttpContext.Current.Request.Item("dataAtto")) Then
                dataAtto = CDate(HttpContext.Current.Request.Item("dataAtto"))
            Else
                dataAtto = Nothing
            End If
            Dim ufficioAtto As String = HttpContext.Current.Request.Item("ufficioAtto")
            If Not String.IsNullOrEmpty(ufficioAtto) Then
                ufficioAtto = HttpContext.Current.Request.Params("ufficioAtto")
            Else
                ufficioAtto = ""
            End If
            Dim numeroLiquidazione As String = HttpContext.Current.Request.Item("numeroLiquidazione")
            If Not String.IsNullOrEmpty(numeroLiquidazione) Then
                numeroLiquidazione = HttpContext.Current.Request.Params("numeroLiquidazione")
            Else
                numeroLiquidazione = ""
            End If
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Dim rispostaInterrogaMandati As Array = ClientIntegrazioneSic.MessageMaker.createInterrogaMandatiBeneficiarioAttoMessage(operatore, tipoAtto, numeroAtto, dataAtto, ufficioAtto, numeroLiquidazione)
            Dim objMandato As Ext_MandatoAttoInfo
            For i As Integer = 0 To UBound(rispostaInterrogaMandati, 1)
                objMandato = New Ext_MandatoAttoInfo

                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataAttoImpSpecified) Then
                    objMandato.DataAttoImp = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataAttoImp
                Else
                    objMandato.DataAttoImp = ""
                End If
                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataAttoLiqSpecified) Then
                    objMandato.DataAttoLiq = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataAttoLiq
                Else
                    objMandato.DataAttoLiq = ""
                End If
                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataEmissioneMandatoSpecified) Then
                    objMandato.DataEmissioneMandato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataEmissioneMandato
                Else
                    objMandato.DataEmissioneMandato = ""
                End If
                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataIncaricoSpecified) Then
                    objMandato.DataIncarico = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataIncarico
                Else
                    objMandato.DataIncarico = ""
                End If
                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataQuietanzaSpecified) Then
                    objMandato.DataQuietanza = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataQuietanza
                Else
                    objMandato.DataQuietanza = ""
                End If
                If (DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataStornoSpecified) Then
                    objMandato.DataStorno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).DataStorno
                Else
                    objMandato.DataStorno = ""
                End If

                objMandato.Dipartimento = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).Dipartimento
                objMandato.EsercizioImpegno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).EsercizioImpegno
                objMandato.EsercizioImpegnoRifPerEnte = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).EsercizioImpegnoRifPerEnte
                objMandato.EsercizioMandato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).EsercizioMandato
                objMandato.IdDocumento = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).IdDocumento
                objMandato.ImportoImpegno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).ImportoImpegno
                objMandato.ImportoTotaleMandato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).ImportoTotaleMandato
                objMandato.Incarico = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).Incarico
                objMandato.NumeroAttoImp = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroAttoImp
                objMandato.NumeroAttoLiq = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroAttoLiq
                objMandato.NumeroDistinta = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroDistinta
                objMandato.NumeroImpegno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroImpegno
                objMandato.NumeroImpegnoRifPerEnte = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroImpegnoRifPerEnte
                objMandato.NumeroLiquidazione = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroLiquidazione
                objMandato.NumeroMandato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroMandato
                objMandato.NumeroQuietanza = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroQuietanza
                objMandato.NumeroStorno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).NumeroStorno
                objMandato.OggettoMandato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).OggettoMandato
                objMandato.TipoAttoImp = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).TipoAttoImp
                objMandato.TipoAttoLiq = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).TipoAttoLiq
                objMandato.TipoImpegno = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).TipoImpegno
                objMandato.ValidoAnnullato = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).ValidoAnnullato

                If objMandato.ListaBeneficiari Is Nothing Then
                    objMandato.ListaBeneficiari = New Generic.List(Of Ext_MandatoBeneficiarioInfo)
                End If

                Dim listaBeneficiari As Array = DirectCast(rispostaInterrogaMandati(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandato).Beneficiario

                If Not listaBeneficiari Is Nothing Then
                    For j As Integer = 0 To UBound(listaBeneficiari, 1)
                        Dim beneficiario As ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandatoBeneficiario
                        beneficiario = DirectCast(listaBeneficiari(j), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaMandatiBeneficiariAtto_TypesMandatoBeneficiario)

                        If Not beneficiario Is Nothing Then
                            Dim objBeneficiario As Ext_MandatoBeneficiarioInfo
                            objBeneficiario = New Ext_MandatoBeneficiarioInfo

                            objBeneficiario.NomeFornitore = beneficiario.NomeFornitore
                            objBeneficiario.ABI = beneficiario.ABI
                            objBeneficiario.AddizionaleComunale = beneficiario.AddizionaleComunale
                            objBeneficiario.AddizionaleRegionale = beneficiario.AddizionaleRegionale
                            objBeneficiario.AltreRitenute = beneficiario.AltreRitenute
                            objBeneficiario.Banca = beneficiario.Banca
                            objBeneficiario.CAB = beneficiario.CAB
                            objBeneficiario.CapBanca = beneficiario.CapBanca
                            objBeneficiario.CapFornitore = beneficiario.CapFornitore
                            objBeneficiario.CittaBanca = beneficiario.CittaBanca
                            objBeneficiario.CittaFornitore = beneficiario.CittaFornitore
                            objBeneficiario.CodiceCig = beneficiario.CodiceCig
                            objBeneficiario.CodiceCup = beneficiario.CodiceCup
                            objBeneficiario.CodiceFiscaleRappresentanteLegale = beneficiario.CodiceFiscaleRappresentanteLegale
                            objBeneficiario.ContoCorrente = beneficiario.ContoCorrente
                            objBeneficiario.DataNascitaRappresentanteLegale = beneficiario.DataNascitaRappresentanteLegale
                            objBeneficiario.DescrizionePagamentoTesoriere = beneficiario.DescrizionePagamentoTesoriere
                            objBeneficiario.IBAN = beneficiario.IBAN
                            objBeneficiario.ImponibileIrap = beneficiario.ImponibileIrap
                            objBeneficiario.ImponibileIrpef = beneficiario.ImponibileIrpef
                            objBeneficiario.ImponibilePrevidenziale = beneficiario.ImponibilePrevidenziale
                            objBeneficiario.ImportoLordoBeneficiario = beneficiario.ImportoLordoBeneficiario
                            objBeneficiario.ImportoNettoBeneficiario = beneficiario.ImportoNettoBeneficiario
                            objBeneficiario.ImpostaIrap = beneficiario.ImpostaIrap
                            objBeneficiario.IndirizzoBanca = beneficiario.IndirizzoBanca
                            objBeneficiario.IndirizzoFornitore = beneficiario.IndirizzoFornitore
                            objBeneficiario.InfoAggiuntive = beneficiario.InfoAggiuntive
                            objBeneficiario.InfoPagamento = beneficiario.InfoPagamento
                            objBeneficiario.MetodoPagamento = beneficiario.MetodoPagamento
                            objBeneficiario.NazioneBanca = beneficiario.NazioneBanca
                            objBeneficiario.NazioneFornitore = beneficiario.NazioneFornitore
                            objBeneficiario.PartitaIvaFornitore = beneficiario.PartitaIvaFornitore
                            objBeneficiario.ProvinciaBanca = beneficiario.ProvinciaBanca
                            objBeneficiario.ProvinciaFornitore = beneficiario.ProvinciaFornitore
                            objBeneficiario.RappresentanteLegale = beneficiario.RappresentanteLegale
                            objBeneficiario.RitenutaIrpef = beneficiario.RitenutaIrpef
                            objBeneficiario.RitenutePrevidenzialiBeneficiario = beneficiario.RitenutePrevidenzialiBeneficiario
                            objBeneficiario.RitenutePrevidenzialiEnte = beneficiario.RitenutePrevidenzialiEnte
                            objBeneficiario.StatoFornitore = beneficiario.StatoFornitore

                            objMandato.ListaBeneficiari.Add(objBeneficiario)
                        End If
                    Next
                End If
                mandati.Add(objMandato)

            Next
            Return mandati

        Catch ex As Exception
            Log.Error(ex.ToString)
            Return mandati
        End Try

    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/InterrogaContratti")>
    Public Function InterrogaContratti() As System.Collections.Generic.List(Of Ext_ContrattoInfo)
        Dim contratti As New System.Collections.Generic.List(Of Ext_ContrattoInfo)
        Try
            Dim numeroRepertorio As String = HttpContext.Current.Request.Item("numeroRepertorio")
            If Not String.IsNullOrEmpty(numeroRepertorio) Then
                numeroRepertorio = HttpContext.Current.Request.Params("numeroRepertorio")
            Else
                numeroRepertorio = ""
            End If
            Dim descrizione As String = HttpContext.Current.Request.Item("descrizione")
            If Not String.IsNullOrEmpty(descrizione) Then
                descrizione = HttpContext.Current.Request.Params("descrizione")
            Else
                descrizione = ""
            End If

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim rispostaInterrogaContratti As Array = ClientIntegrazioneSic.MessageMaker.createInterrogaContrattiMessage(operatore, numeroRepertorio, descrizione)

            For i As Integer = 0 To UBound(rispostaInterrogaContratti, 1)
                If Not rispostaInterrogaContratti(i) Is Nothing Then
                    Dim objContratto As Ext_ContrattoInfo = New Ext_ContrattoInfo

                    objContratto.Id = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).Chiave
                    If objContratto.Id Is Nothing Then
                        objContratto.Id = String.Empty
                    End If

                    objContratto.NumeroRepertorio = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).NumeroRepertorio
                    If objContratto.NumeroRepertorio Is Nothing Then
                        objContratto.NumeroRepertorio = String.Empty
                    End If

                    objContratto.Oggetto = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).Descrizione
                    If objContratto.Oggetto Is Nothing Then
                        objContratto.Oggetto = String.Empty
                    End If

                    objContratto.CodiceCIG = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCig
                    If objContratto.CodiceCIG Is Nothing Then
                        objContratto.CodiceCIG = String.Empty
                    End If

                    objContratto.CodiceCUP = DirectCast(rispostaInterrogaContratti(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogazioneContratti_TypesContratto).CodiceCup
                    If objContratto.CodiceCUP Is Nothing Then
                        objContratto.CodiceCUP = String.Empty
                    End If

                    If objContratto.NumeroRepertorio <> "0" Then
                        contratti.Add(objContratto)
                    End If

                End If
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
        End Try

        Return contratti
    End Function

    Enum TipoStruttura
        attuale = 0
        vl = 1
    End Enum

    Enum TipoDocumento
        determina = 0
        disposizione = 1
        delibera = 2
    End Enum

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/InterrogaDocumenti")>
    Public Function InterrogaDocumenti() As DllDocumentale.Page(Of Ext_DocumentoInfo)
        Dim documentPage As New DllDocumentale.Page(Of Ext_DocumentoInfo)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

            Dim start As Integer = 0, limit As Integer = 20
            If Not HttpContext.Current.Request.Form.Item("start") Is Nothing Then
                start = HttpContext.Current.Request.Form.Item("start")
            End If

            If Not HttpContext.Current.Request.Form.Item("limit") Is Nothing Then
                limit = HttpContext.Current.Request.Form.Item("limit")
            End If

            Dim validateUfficio As String = HttpContext.Current.Request.Item("validateUfficio")
            If String.IsNullOrEmpty(validateUfficio) Then
                validateUfficio = HttpContext.Current.Request.Form.Item("validateUfficio")
            End If

            If validateUfficio Is Nothing OrElse (validateUfficio.ToLower().Trim() <> "true" AndAlso validateUfficio.ToLower().Trim() <> "false") Then
                validateUfficio = "false"
            End If

            Dim tipoDoc As String = HttpContext.Current.Request.Item("tipoDoc")
            If String.IsNullOrEmpty(tipoDoc) Then
                tipoDoc = HttpContext.Current.Request.Params("tipoDoc")
            End If

            Dim dataDa As String = HttpContext.Current.Request.Item("dataDa")
            If String.IsNullOrEmpty(dataDa) Then
                dataDa = HttpContext.Current.Request.Params("dataDa")
            End If

            Dim dataA As String = HttpContext.Current.Request.Item("dataA")
            If String.IsNullOrEmpty(dataA) Then
                dataA = HttpContext.Current.Request.Params("dataA")
            End If

            Dim tipoStrutturaAsString As String = HttpContext.Current.Request.Item("tipoStruttura")
            If String.IsNullOrEmpty(tipoStrutturaAsString) Then
                tipoStrutturaAsString = HttpContext.Current.Request.Params("tipoStruttura")
            End If

            Dim codDip As String = HttpContext.Current.Request.Item("codDip")
            If String.IsNullOrEmpty(codDip) Then
                codDip = HttpContext.Current.Request.Params("codDip")
            End If

            Dim codUff As String = HttpContext.Current.Request.Item("codUff")
            If String.IsNullOrEmpty(codUff) Then
                codUff = HttpContext.Current.Request.Params("codUff")
            End If

            Dim numDoc As String = HttpContext.Current.Request.Item("numDoc")
            If String.IsNullOrEmpty(numDoc) Then
                numDoc = HttpContext.Current.Request.Params("numDoc")
            End If

            Dim oggettoDoc As String = HttpContext.Current.Request.Item("oggettoDoc")
            If String.IsNullOrEmpty(oggettoDoc) Then
                oggettoDoc = HttpContext.Current.Request.Params("oggettoDoc")
            End If

            If Not tipoDoc Is Nothing Then
                If tipoDoc = "2" Then 'Delibere
                    Dim tipoStrutturaValue As TipoStruttura = TipoStruttura.attuale
                    If Not tipoStrutturaAsString Is Nothing Then
                        If tipoStrutturaAsString.Trim().ToLower() = "vl" Then
                            tipoStrutturaValue = TipoStruttura.vl
                        ElseIf tipoStrutturaAsString.Trim().ToLower() = "attuale" Then
                            tipoStrutturaValue = TipoStruttura.attuale
                        Else
                            Throw New Exception("La tipologia di struttura specificata non è valida. Indicare 'attuale' per richiedere la struttura corrente o 'vl' per quella precedente.")
                        End If
                    End If
                    documentPage = InterrogaDelibere(operatore, dataDa, dataA, tipoStrutturaValue, codDip, oggettoDoc, numDoc, start, limit)
                ElseIf tipoDoc = "0" Or tipoDoc = "1" Then 'Determine o Disposizioni                                        
                    Dim tipoDocValue As TipoDocumento = Nothing
                    If tipoDoc.Trim() = "0" Then
                        tipoDocValue = TipoDocumento.determina
                    ElseIf tipoDoc.Trim() = "1" Then
                        tipoDocValue = TipoDocumento.disposizione
                    End If

                    If (codUff Is Nothing OrElse codUff.Trim() = String.Empty) AndAlso (Not codDip Is Nothing AndAlso codDip.Trim() <> String.Empty) Then
                        codUff = codDip
                    End If

                    documentPage = InterrogaDocumenti(operatore, validateUfficio, tipoDocValue, dataDa, dataA, codUff, oggettoDoc, numDoc, start, limit)
                End If
            End If

        Catch ex1 As InterrogaDocumentiException
            Log.Error(ex1.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex1, ex1.Code))
        Catch ex2 As Exception
            Log.Error(ex2.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex2))
        End Try

        Return documentPage
    End Function

    Private Function InterrogaDocumenti(ByVal op As DllAmbiente.Operatore, ByVal validateUfficio As Boolean, ByVal tipoDoc As TipoDocumento, ByVal dataDa As String, ByVal dataA As String, ByVal codUff As String, ByVal oggettoDoc As String, ByVal numDoc As String, ByVal startRow As Integer, ByVal rowsForPage As Integer) As DllDocumentale.Page(Of Ext_DocumentoInfo)
        Dim retValue As New DllDocumentale.Page(Of Ext_DocumentoInfo)

        Dim wsProvvedimentiFinder As ProvvedimentiFinderWS.ServizioRicerca = New ProvvedimentiFinderWS.ServizioRicerca()

        Dim autenticazione As ProvvedimentiFinderWS.Messaggio_AutenticazioneOperatore = Nothing

        If Not op Is Nothing Then
            If validateUfficio AndAlso Not codUff Is Nothing AndAlso Not codUff.Trim() = String.Empty Then
                Dim ufficioFound As Boolean = False

                Dim ufficiDipendenti = op.UfficiDipendenti(IIf(tipoDoc = TipoDocumento.determina, "0", IIf(tipoDoc = TipoDocumento.disposizione, "1", "2")))
                For Each ufficio As StrutturaInfo In ufficiDipendenti
                    If ufficio.CodicePubblico.ToLower().Trim() = codUff.ToLower().Trim() Then
                        ufficioFound = True
                        Exit For
                    End If
                Next
                If Not ufficioFound Then
                    Throw New InterrogaDocumentiException("Ufficio specificato non consultabile. Interrogazione non consentita.", -5)
                Else
                    autenticazione = New ProvvedimentiFinderWS.Messaggio_AutenticazioneOperatore()

                    autenticazione.Cod_Fiscale = op.CodiceFiscale
                    autenticazione.Cod_Ufficio = op.oUfficio.CodUfficioPubblico
                End If
            End If
        End If

        Dim richiesta As ProvvedimentiFinderWS.Messaggio_RichiestaRicerca = New ProvvedimentiFinderWS.Messaggio_RichiestaRicerca()

        richiesta.TipoAtto = New ProvvedimentiFinderWS.Messaggio_RichiestaTipoAtto

        Select Case tipoDoc
            Case TipoDocumento.determina
                richiesta.TipoAtto.Tipo = ProvvedimentiFinderWS.Tipo_Atto_Types.Item0
            Case TipoDocumento.disposizione
                richiesta.TipoAtto.Tipo = ProvvedimentiFinderWS.Tipo_Atto_Types.Item1
            Case TipoDocumento.delibera
                richiesta.TipoAtto.Tipo = ProvvedimentiFinderWS.Tipo_Atto_Types.Item2
        End Select

        If Not oggettoDoc Is Nothing AndAlso oggettoDoc.Trim() <> String.Empty Then
            richiesta.Oggetto = oggettoDoc
        Else
            richiesta.Oggetto = Nothing
        End If

        If (dataDa Is Nothing OrElse dataDa.Trim() = String.Empty) AndAlso (dataA Is Nothing OrElse dataA.Trim() = String.Empty) Then
            richiesta.Data = Nothing
        Else
            richiesta.Data = New ProvvedimentiFinderWS.Messaggio_RichiestaRicercaData

            If Not dataA Is Nothing AndAlso Not dataA.Trim() = String.Empty Then
                richiesta.Data.DataA = dataA
                richiesta.Data.DataASpecified = True
            Else
                richiesta.Data.DataASpecified = False
            End If

            If Not dataDa Is Nothing AndAlso Not dataDa.Trim() = String.Empty Then
                richiesta.Data.DataDa = dataDa
            Else
                richiesta.Data.DataDa = richiesta.Data.DataA
            End If
        End If

        If Not codUff Is Nothing AndAlso Not codUff.Trim() = String.Empty Then
            richiesta.UfficioProponente = New ProvvedimentiFinderWS.Messaggio_RichiestaRicercaUfficioProponente
            richiesta.UfficioProponente.Codice = codUff
        Else
            richiesta.UfficioProponente = Nothing
        End If

        If Not numDoc Is Nothing AndAlso Not numDoc.Trim() = String.Empty Then
            richiesta.NumeroAtto = New ProvvedimentiFinderWS.Messaggio_RichiestaRicercaNumeroAtto
            richiesta.NumeroAtto.NumeroDa = numDoc
            richiesta.NumeroAtto.NumeroA = numDoc
        Else
            richiesta.NumeroAtto = Nothing
        End If

        Dim totalePerPagina As Integer = rowsForPage
        Dim paginaRichiesta As Integer = (startRow / totalePerPagina) + 1

        richiesta.Paginazione = New ProvvedimentiFinderWS.Messaggio_RichiestaRicercaPaginazione
        richiesta.Paginazione.PaginaRichiesta = paginaRichiesta
        richiesta.Paginazione.TotalePerPagina = totalePerPagina

        Dim risposta As ProvvedimentiFinderWS.Messaggio_Risposta = wsProvvedimentiFinder.RicercaAttiApprovati(richiesta, autenticazione)

        If Not risposta Is Nothing AndAlso Not risposta.Item Is Nothing Then
            If TypeOf risposta.Item Is ProvvedimentiFinderWS.Messaggio_RispostaRicercaAtti Then
                Dim documenti As New System.Collections.Generic.List(Of Ext_DocumentoInfo)

                Dim atti As ProvvedimentiFinderWS.Messaggio_RispostaRicercaAtti = risposta.Item
                If Not atti Is Nothing AndAlso Not atti.Atto Is Nothing AndAlso atti.Atto.Length > 0 Then

                    Dim ufficiDipendentiHashSet As HashSet(Of String) = New HashSet(Of String)

                    Dim ufficiDipendenti = op.UfficiDipendenti(IIf(tipoDoc = TipoDocumento.determina, "0", IIf(tipoDoc = TipoDocumento.disposizione, "1", "2")))
                    If Not ufficiDipendenti Is Nothing Then
                        For Each ufficio As StrutturaInfo In ufficiDipendenti
                            ufficiDipendentiHashSet.Add(ufficio.CodicePubblico.ToLower())
                        Next
                    End If

                    For Each atto As ProvvedimentiFinderWS.Messaggio_RispostaRicercaAttiAtto In atti.Atto
                        If Not atto Is Nothing Then
                            Dim documento As Ext_DocumentoInfo = New Ext_DocumentoInfo
                            documento.Doc_Numero = atto.NumeroDefinitivo
                            documento.Doc_Data = atto.DataAtto
                            documento.Doc_Oggetto = atto.Oggetto
                            documento.Doc_Tipo = atto.TipoAtto
                            If Not atto.TipologiaProvvedimento Is Nothing AndAlso
                                Not atto.TipologiaProvvedimento.IdTipologiaProvvedimento Is Nothing AndAlso
                                atto.TipologiaProvvedimento.IdTipologiaProvvedimento.Trim() <> String.Empty Then
                                Dim idTipologiaDocumento As Integer
                                If Integer.TryParse(atto.TipologiaProvvedimento.IdTipologiaProvvedimento, idTipologiaDocumento) Then
                                    documento.Doc_IdTipologiaDocumento = idTipologiaDocumento
                                    documento.Doc_TipologiaDocumento = IIf(Not String.IsNullOrEmpty(atto.TipologiaProvvedimento.DescrizioneTipologiaProvvedimento),
                                        atto.TipologiaProvvedimento.DescrizioneTipologiaProvvedimento, String.Empty)
                                End If

                                If Not atto.TipologiaProvvedimento.Lista_Destinatari() Is Nothing AndAlso
                                    atto.TipologiaProvvedimento.Lista_Destinatari().Count() > 0 Then
                                    documento.Doc_DestinatariDocumento = New Generic.List(Of Ext_DestinatarioInfo)
                                    For Each destinatario As ProvvedimentiFinderWS.Destinatario_Types In atto.TipologiaProvvedimento.Lista_Destinatari
                                        Dim ext_destinatario As Ext_DestinatarioInfo = New Ext_DestinatarioInfo()


                                        If destinatario.DatiFiscali.ItemElementName = ProvvedimentiFinderWS.ItemChoiceType.CodiceFiscale Then
                                            ext_destinatario.CodiceFiscale = destinatario.DatiFiscali.Item
                                        ElseIf destinatario.DatiFiscali.ItemElementName = ProvvedimentiFinderWS.ItemChoiceType.PartitaIva Then
                                            ext_destinatario.PartitaIva = destinatario.DatiFiscali.Item
                                        End If

                                        ext_destinatario.LuogoNascita = destinatario.LuogoNascita
                                        ext_destinatario.DataNascita = destinatario.DataNascita
                                        ext_destinatario.Denominazione = destinatario.Denominazione
                                        ext_destinatario.IdSIC = destinatario.IdDestinatario

                                        If Not destinatario.Contratto Is Nothing Then
                                            ext_destinatario.IdContratto = destinatario.Contratto.Id_Contratto
                                            ext_destinatario.NumeroRepertorioContratto = destinatario.Contratto.Numero_Repertorio_Contratto
                                        End If

                                        'ext_destinatario.IdDocumento = destinatario.
                                        ext_destinatario.isDatoSensibile = destinatario.IsDatoSensibile
                                        ext_destinatario.isPersonaFisica = destinatario.IsPersonaFisica
                                        If Not destinatario.IsPersonaFisica Then
                                            ext_destinatario.LegaleRappresentante = destinatario.LegaleRappresentante
                                        End If
                                        documento.Doc_DestinatariDocumento.Add(ext_destinatario)
                                    Next
                                End If
                            End If
                            If Not atto.UfficioProponente Is Nothing Then
                                documento.IsConsultabile = ufficiDipendentiHashSet.Contains(atto.UfficioProponente.CodiceUfficio.ToLower())

                                documento.Doc_Cod_Uff_Prop = atto.UfficioProponente.CodiceUfficio
                                documento.Doc_Descrizione_ufficio = atto.UfficioProponente.DescrizioneUfficio
                            End If
                            documenti.Add(documento)
                        End If
                    Next
                End If

                If documenti.Count = 0 AndAlso Not codUff Is Nothing AndAlso Not codUff.Trim() = String.Empty Then
                    If Not op Is Nothing Then
                        Dim ufficioFound As Boolean = False

                        Dim ufficiDipendenti = op.UfficiDipendenti(IIf(tipoDoc = TipoDocumento.determina, "0", IIf(tipoDoc = TipoDocumento.disposizione, "1", "2")))
                        For Each ufficio As StrutturaInfo In ufficiDipendenti
                            If ufficio.CodicePubblico.ToLower().Trim() = codUff.ToLower().Trim() Then
                                ufficioFound = True
                                Exit For
                            End If
                        Next
                        If Not ufficioFound Then
                            Throw New InterrogaDocumentiException("Ufficio specificato non consultabile. Nessun documento trovato.", -4)
                        End If
                    End If
                End If

                retValue.Data = documenti
                retValue.TotalCount = atti.Paginazione.TotaleAtti
            Else
                Dim exception As ProvvedimentiFinderWS.Messaggio_RispostaException = risposta.Item
                Throw New Exception(exception.Descrizione)
            End If
        End If

        Return retValue
    End Function

    Private Function InterrogaDelibere(ByVal op As DllAmbiente.Operatore, ByVal dataDa As String, ByVal dataA As String, ByVal tipoStruttura As TipoStruttura, ByVal codStruttura As String, ByVal oggettoDoc As String, ByVal numDoc As String, ByVal startRow As Integer, ByVal rowsForPage As Integer) As DllDocumentale.Page(Of Ext_DocumentoInfo)
        Dim retValue As New DllDocumentale.Page(Of Ext_DocumentoInfo)

        Dim wsDelibereFinder As AttiFinderWS.ServizioRicerca = New AttiFinderWS.ServizioRicerca()
        Dim richiesta As AttiFinderWS.Messaggio_RichiestaRicerca = New AttiFinderWS.Messaggio_RichiestaRicerca()

        richiesta.TipoAtto = New AttiFinderWS.Messaggio_RichiestaTipoAtto
        richiesta.TipoAtto.Tipo = AttiFinderWS.Tipo_Atto_Types.Item1 'Delibere

        If Not oggettoDoc Is Nothing Then
            richiesta.Oggetto = oggettoDoc
        End If

        richiesta.Data = New AttiFinderWS.Messaggio_RichiestaRicercaData

        If Not dataDa Is Nothing AndAlso Not dataDa.Trim() = String.Empty Then
            richiesta.Data.DataDa = dataDa
        End If

        If Not dataA Is Nothing AndAlso Not dataA.Trim() = String.Empty Then
            richiesta.Data.DataA = dataA
            richiesta.Data.DataASpecified = True
        Else
            richiesta.Data.DataASpecified = False
        End If

        richiesta.StrutturaProponente = New AttiFinderWS.Messaggio_RichiestaRicercaStrutturaProponente

        If Not codStruttura Is Nothing AndAlso Not codStruttura.Trim() = String.Empty Then
            richiesta.StrutturaProponente.Codice = codStruttura
        End If

        If tipoStruttura = ProcAmm.TipoStruttura.vl Then
            richiesta.StrutturaProponente.Tipo = AttiFinderWS.Tipo_Struttura_Types.vl
        Else
            richiesta.StrutturaProponente.Tipo = AttiFinderWS.Tipo_Struttura_Types.attuale
        End If

        richiesta.NumeroAtto = New AttiFinderWS.Messaggio_RichiestaRicercaNumeroAtto

        If Not numDoc Is Nothing AndAlso Not numDoc.Trim() = String.Empty Then
            richiesta.NumeroAtto.NumeroDa = numDoc
            richiesta.NumeroAtto.NumeroA = numDoc
        End If

        Dim totalePerPagina As Integer = rowsForPage
        Dim paginaRichiesta As Integer = (startRow / totalePerPagina) + 1

        richiesta.Paginazione = New AttiFinderWS.Messaggio_RichiestaRicercaPaginazione
        richiesta.Paginazione.PaginaRichiesta = paginaRichiesta
        richiesta.Paginazione.TotalePerPagina = totalePerPagina

        Dim risposta As AttiFinderWS.Messaggio_Risposta = wsDelibereFinder.RicercaAttiApprovati(richiesta)

        If Not risposta Is Nothing AndAlso Not risposta.Item Is Nothing Then
            If TypeOf risposta.Item Is AttiFinderWS.Messaggio_RispostaRicercaAtti Then
                Dim documenti As New System.Collections.Generic.List(Of Ext_DocumentoInfo)

                Dim atti As AttiFinderWS.Messaggio_RispostaRicercaAtti = risposta.Item
                If Not atti Is Nothing AndAlso Not atti.Atto Is Nothing Then
                    For Each atto As AttiFinderWS.Messaggio_RispostaRicercaAttiAtto In atti.Atto
                        If Not atto Is Nothing Then
                            Dim documento As Ext_DocumentoInfo = New Ext_DocumentoInfo
                            documento.Doc_Numero = atto.Numero
                            documento.Doc_Data = atto.Data
                            documento.Doc_Oggetto = atto.Oggetto
                            documento.Doc_Tipo = atto.Tipo

                            If Not atto.TipologiaProvvedimento Is Nothing Then
                                documento.Doc_IdTipologiaDocumento = atto.TipologiaProvvedimento.IdTipologiaProvvedimento
                                documento.Doc_TipologiaDocumento = IIf(Not String.IsNullOrEmpty(atto.TipologiaProvvedimento.DescrizioneTipologiaProvvedimento),
                                        atto.TipologiaProvvedimento.DescrizioneTipologiaProvvedimento, String.Empty)

                                If Not atto.TipologiaProvvedimento.Lista_Destinatari() Is Nothing AndAlso
                                    atto.TipologiaProvvedimento.Lista_Destinatari().Count() > 0 Then

                                    documento.Doc_DestinatariDocumento = New Generic.List(Of Ext_DestinatarioInfo)

                                    For Each destinatario As AttiFinderWS.Destinatario_Types In atto.TipologiaProvvedimento.Lista_Destinatari()
                                        Dim ext_destinatario As Ext_DestinatarioInfo = New Ext_DestinatarioInfo()

                                        ext_destinatario.CodiceFiscale = destinatario.CodiceFiscale
                                        ext_destinatario.DataNascita = destinatario.DataNascita
                                        ext_destinatario.Denominazione = destinatario.Denominazione
                                        ext_destinatario.IdSIC = destinatario.IdSIC
                                        ext_destinatario.IdContratto = destinatario.IdContratto
                                        ext_destinatario.IdDocumento = destinatario.IdDocumento
                                        ext_destinatario.isDatoSensibile = destinatario.IsDatoSensibile
                                        ext_destinatario.isPersonaFisica = destinatario.IsPersonaFisica
                                        ext_destinatario.LegaleRappresentante = destinatario.LegaleRappresentante
                                        ext_destinatario.LuogoNascita = destinatario.LuogoNascita
                                        ext_destinatario.NumeroRepertorioContratto = destinatario.NumeroRepertorioContratto
                                        ext_destinatario.PartitaIva = destinatario.PartitaIva

                                        documento.Doc_DestinatariDocumento.Add(ext_destinatario)
                                    Next
                                End If
                            End If
                            If Not atto.StrutturaProponente Is Nothing Then
                                documento.Doc_Cod_Uff_Prop = atto.StrutturaProponente.CodiceUfficio
                                documento.Doc_Descrizione_ufficio = atto.StrutturaProponente.DescrizioneUfficio
                            End If
                            documenti.Add(documento)
                        End If
                    Next
                End If

                retValue.Data = documenti
                retValue.TotalCount = atti.Paginazione.TotaleAtti
            Else
                Dim exception As AttiFinderWS.Messaggio_RispostaException = risposta.Item
                Throw New Exception(exception.Descrizione)
            End If
        End If

        Return retValue
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetDipartimentiDelibere?tipoStruttura={tipoStruttura}")>
    Public Function GetDipartimentiDelibere(ByVal tipoStruttura As String) As IList(Of Ext_StrutturaInfo)
        Dim retValue As List(Of Ext_StrutturaInfo) = New List(Of Ext_StrutturaInfo)
        Try
            Dim wsDelibereFinder As AttiFinderWS.ServizioRicerca = New AttiFinderWS.ServizioRicerca()
            Dim richiesta As AttiFinderWS.Messaggio_RichiestaGetListaStrutture = New AttiFinderWS.Messaggio_RichiestaGetListaStrutture()

            If tipoStruttura Is Nothing OrElse tipoStruttura = "attuale" Then
                richiesta.TipoSpecified = True
                richiesta.Tipo = AttiFinderWS.Tipo_Struttura_Types.attuale
            ElseIf tipoStruttura = "vl" Then
                richiesta.TipoSpecified = True
                richiesta.Tipo = AttiFinderWS.Tipo_Struttura_Types.vl
            Else
                Throw New Exception("La tipologia di struttura specificata non è valida. Indicare 'attuale' per richiedere la struttura corrente o 'vl' per quella precedente.")
            End If

            Dim risposta As AttiFinderWS.Messaggio_Risposta = wsDelibereFinder.GetListaStrutture(richiesta)

            If Not risposta Is Nothing Then
                If TypeOf risposta.Item Is AttiFinderWS.Messaggio_RispostaGetListaStrutture Then
                    Dim strutture As AttiFinderWS.Messaggio_RispostaGetListaStrutture = risposta.Item

                    If Not strutture Is Nothing AndAlso Not strutture.Struttura Is Nothing Then
                        For Each struttura As AttiFinderWS.Struttura_Types In strutture.Struttura
                            If Not struttura Is Nothing Then
                                Dim dipartimentoExt As Ext_StrutturaInfo = Nothing

                                dipartimentoExt = New Ext_StrutturaInfo
                                dipartimentoExt.CodiceInterno = struttura.IdStruttura
                                dipartimentoExt.DescrizioneBreve = struttura.DescrizioneBreve
                                dipartimentoExt.Padre = struttura.Padre
                                dipartimentoExt.Tipologia = struttura.Tipo

                                retValue.Add(dipartimentoExt)
                            End If
                        Next
                    End If
                Else
                    Dim exception As AttiFinderWS.Messaggio_RispostaException = risposta.Item
                    Throw New Exception(exception.Descrizione)
                End If
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try

        Return retValue
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/InterrogaAnagrafica")>
    Public Function InterrogaAnagrafica() As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        'ByVal codFiscale As String, ByVal pIva As String, ByVal denominazione As String, ByVal idAnagrafica As String, ByVal idContratto As String, ByVal idFattura As String, ByVal progLiq As String, ByVal idBeneficiari As String

        'codFiscale: CF, pIva: PI, denominazione: DE, idAnagrafica: CS, idContratto: IC, idFattura: IF, progLiq: progLiquidazione, idBeneficiari: IB };

        Dim anagrafiche As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Try
            Dim denominazione As String = HttpContext.Current.Request.Item("denominazione")
            If String.IsNullOrEmpty(denominazione) Then
                denominazione = HttpContext.Current.Request.Params("denominazione")
            End If
            Dim codFiscale As String = HttpContext.Current.Request.Item("codFiscale")
            If String.IsNullOrEmpty(codFiscale) Then
                codFiscale = HttpContext.Current.Request.Params("codFiscale")
            End If
            Dim pIva As String = HttpContext.Current.Request.Item("pIva")
            If String.IsNullOrEmpty(pIva) Then
                pIva = HttpContext.Current.Request.Params("pIva")
            End If
            Dim idAnagrafica As String = HttpContext.Current.Request.Item("idAnagrafica")
            If String.IsNullOrEmpty(idAnagrafica) Then
                idAnagrafica = HttpContext.Current.Request.Params("idAnagrafica")
            End If
            Dim idContratto As String = HttpContext.Current.Request.Item("idContratto")
            If String.IsNullOrEmpty(idContratto) Then
                idContratto = HttpContext.Current.Request.Params("idContratto")
            End If

            Dim idFattura As String = HttpContext.Current.Request.Item("idFattura")
            If String.IsNullOrEmpty(idFattura) Then
                idFattura = HttpContext.Current.Request.Params("idFattura")
            End If
            Dim progLiq As String = HttpContext.Current.Request.Item("progLiq")
            If String.IsNullOrEmpty(progLiq) Then
                progLiq = HttpContext.Current.Request.Params("progLiq")
            End If


            Dim idDestinatari As String = HttpContext.Current.Request.Item("idBeneficiari")
            If String.IsNullOrEmpty(idDestinatari) Then
                idDestinatari = HttpContext.Current.Request.Params("idBeneficiari")
            End If
            'Dim idDestinatari As String = idBeneficiari

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            If Not idDestinatari Is Nothing AndAlso idDestinatari.Trim() <> String.Empty AndAlso idDestinatari.ToLower = "all" Then
                anagrafiche = ListaDestinatariProvvedimento(operatore, denominazione, codFiscale, pIva, idContratto, CodDocumento)
            ElseIf Not idContratto Is Nothing AndAlso idContratto.Trim() <> String.Empty AndAlso idContratto.ToLower = "all" Then
                anagrafiche = ListaBeneficiariTuttiContratti(operatore, denominazione, codFiscale, pIva, idAnagrafica, CodDocumento)
            ElseIf Not idFattura Is Nothing AndAlso idFattura.Trim() <> String.Empty Then
                anagrafiche = ListaBeneficiariTutteFatture(operatore, CodDocumento(), idFattura, progLiq)

                'anagrafiche = ListaBeneficiariTutteFatture(operatore, denominazione, codFiscale, pIva, idAnagrafica, CodDocumento)
            Else
                anagrafiche = InterrogaAnagraficaSIC(operatore, denominazione, codFiscale, pIva, idAnagrafica, idContratto)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return anagrafiche
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaDelibereWL")>
    Public Function GetListaDelibereWL() As System.Collections.Generic.List(Of Ext_DocumentoInfo)
        Dim listaDelibereWL As New System.Collections.Generic.List(Of Ext_DocumentoInfo)
        Dim vettoredati As Object = Nothing
        Try
            vettoredati = Elenco_Documenti(1, Format("01/01/2013", "dd/MM/yyyy"), Format(Now, "dd/MM/yyyy"))
            If vettoredati(0) = 0 Then
                Dim indice As Integer = UBound(vettoredati(1), 2)
                For i As Integer = 0 To indice
                    Dim objDelibera As Ext_DocumentoInfo = New Ext_DocumentoInfo

                    objDelibera.Doc_Id = vettoredati(1)(0, i)
                    objDelibera.Doc_NumeroProvvisorio = vettoredati(1)(2, i)
                    objDelibera.Doc_Data = vettoredati(1)(3, i)
                    objDelibera.Doc_Oggetto = vettoredati(1)(4, i)
                    listaDelibereWL.Add(objDelibera)
                Next
            End If
            'Dim denominazione As String = HttpContext.Current.Request.Item("denominazione")
            'If String.IsNullOrEmpty(denominazione) Then
            '    denominazione = HttpContext.Current.Request.Params("denominazione")
            'End If
            'Dim codFiscale As String = HttpContext.Current.Request.Item("codFiscale")
            'If String.IsNullOrEmpty(codFiscale) Then
            '    codFiscale = HttpContext.Current.Request.Params("codFiscale")
            'End If
            'Dim pIva As String = HttpContext.Current.Request.Item("pIva")
            'If String.IsNullOrEmpty(pIva) Then
            '    pIva = HttpContext.Current.Request.Params("pIva")
            'End If
            'Dim idAnagrafica As String = HttpContext.Current.Request.Item("idAnagrafica")
            'If String.IsNullOrEmpty(idAnagrafica) Then
            '    idAnagrafica = HttpContext.Current.Request.Params("idAnagrafica")
            'End If
            'Dim idContratto As String = HttpContext.Current.Request.Item("idContratto")
            'If String.IsNullOrEmpty(idContratto) Then
            '    idContratto = HttpContext.Current.Request.Params("idContratto")
            'End If
            'Dim idDestinatari As String = HttpContext.Current.Request.Item("idBeneficiari")
            'If String.IsNullOrEmpty(idDestinatari) Then
            '    idDestinatari = HttpContext.Current.Request.Params("idBeneficiari")
            'End If
            'Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            'If Not idDestinatari Is Nothing AndAlso idDestinatari.Trim() <> String.Empty AndAlso idDestinatari.ToLower = "all" Then
            '    anagrafiche = ListaDestinatariProvvedimento(operatore, denominazione, codFiscale, pIva, idContratto, CodDocumento)
            'ElseIf Not idContratto Is Nothing AndAlso idContratto.Trim() <> String.Empty AndAlso idContratto.ToLower = "all" Then
            '    anagrafiche = ListaBeneficiariTuttiContratti(operatore, denominazione, codFiscale, pIva, idAnagrafica, CodDocumento)
            'Else
            '    anagrafiche = InterrogaAnagraficaSIC(operatore, denominazione, codFiscale, pIva, idAnagrafica, idContratto)
            'End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaDelibereWL
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaRelatori")>
    Public Function GetListaRelatori() As System.Collections.Generic.List(Of Ext_RelatoreInfo)
        Dim listaRelatori As New System.Collections.Generic.List(Of Ext_RelatoreInfo)
        Dim vettoredati As Object = Nothing
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim IdDocumento As String = HttpContext.Current.Request.Item("IdDocumento")
            Dim listaRelatoriDllDoc As Generic.List(Of DllDocumentale.ItemRelatore) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_Lista_Relatori_Documento(IdDocumento)
            If listaRelatoriDllDoc Is Nothing Or listaRelatoriDllDoc.Count = 0 Then
                listaRelatoriDllDoc = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_Lista_Relatori_Attivi()
            End If


            For Each relatore As DllDocumentale.ItemRelatore In listaRelatoriDllDoc
                Dim objRelatore As Ext_RelatoreInfo = New Ext_RelatoreInfo
                objRelatore.Tr_id = relatore.Id
                objRelatore.Tr_Cognome = relatore.Cognome
                objRelatore.Tr_Nome = relatore.Nome
                objRelatore.Tr_Ordine_Apparizione = relatore.OrdineApparizione
                objRelatore.Tr_Carica = relatore.Carica
                objRelatore.Tr_attivo = relatore.Attivo
                objRelatore.Tr_dataattivazione = relatore.DataAttivazione
                objRelatore.Tr_datadisattivazione = relatore.DataDisttivazione
                objRelatore.Tr_IdStruttura = relatore.IdStruttura
                objRelatore.IsPresente = relatore.IsPresente
                'objRelatore. = relatore.IsPresente
                listaRelatori.Add(objRelatore)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaRelatori
    End Function
    Private Function ListaDestinatariProvvedimento(ByVal operatore As DllAmbiente.Operatore, ByVal denominazione As String, ByVal codFiscale As String, ByVal pIva As String, ByVal idContratto As String, ByVal idDocumento As String) As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Dim retValue As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)

        Try
            Dim listaDestinatari As List(Of DllDocumentale.ItemDestinatarioInfo) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaDestinatari(operatore, idDocumento)

            For Each destinatario As DllDocumentale.ItemDestinatarioInfo In listaDestinatari
                Dim anagrafiche As System.Collections.Generic.List(Of Ext_AnagraficaInfo) = InterrogaAnagraficaSIC(operatore, denominazione, codFiscale, pIva, destinatario.IdSIC, idContratto)
                retValue.AddRange(anagrafiche)
            Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw ex
        End Try

        Return retValue
    End Function


    Private Function ListaBeneficiariTuttiContratti(ByVal operatore As DllAmbiente.Operatore, ByVal denominazione As String, ByVal codFiscale As String, ByVal pIva As String, ByVal idAnagrafica As String, ByVal idDocumento As String) As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Dim retValue As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)

        Try
            Dim listaContratti As List(Of DllDocumentale.ItemContrattoInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaContratti(idDocumento)

            For Each contratto As DllDocumentale.ItemContrattoInfoHeader In listaContratti
                Dim anagrafiche As System.Collections.Generic.List(Of Ext_AnagraficaInfo) = InterrogaAnagraficaSIC(operatore, denominazione, codFiscale, pIva, idAnagrafica, contratto.IdContratto)
                retValue.AddRange(anagrafiche)
            Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw ex
        End Try

        Return retValue
    End Function

    Private Function ListaBeneficiariTutteFatture(ByVal operatore As DllAmbiente.Operatore, ByVal idDocumento As String, ByVal idFattura As String, Optional ByVal progLiq As String = "") As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Dim retValue As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Dim listaFatture As New List(Of DllDocumentale.ItemFatturaInfoHeader)
        Dim progLiqInt As Integer = 0
        Try
            If progLiq <> "" Then
                Integer.TryParse(progLiq, progLiqInt)
                If idFattura.ToLower = "all" Then
                    listaFatture = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(progLiqInt, idDocumento)
                Else
                    listaFatture = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(progLiqInt, idDocumento, idFattura)
                End If
            Else
                If idFattura.ToLower = "all" Then
                    listaFatture = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(, idDocumento)
                Else
                    listaFatture = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(, idDocumento, idFattura)
                End If
            End If
            Dim listaAnagraficheResult As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
            Dim anagraficaTrovataDaRestituire As New Ext_AnagraficaInfo
            For Each fattura As DllDocumentale.ItemFatturaInfoHeader In listaFatture
                Dim anagrafiche As System.Collections.Generic.List(Of Ext_AnagraficaInfo) = InterrogaAnagraficaSIC(operatore, Nothing, Nothing, Nothing, fattura.AnagraficaInfo.IdAnagrafica, "")

                For Each anagrafica As Ext_AnagraficaInfo In anagrafiche
                   
                    anagraficaTrovataDaRestituire = anagrafica

                    For Each sede As Ext_SedeAnagraficaInfo In anagrafica.ListaSedi
                        If sede.IdSede = fattura.AnagraficaInfo.IdSede Then
                            'anagraficaTrovataDaRestituire.ListaSedi.Clear()
                            anagraficaTrovataDaRestituire.ListaSedi = new List(Of Ext_SedeAnagraficaInfo)
                            anagraficaTrovataDaRestituire.ListaSedi.Add(sede)
                            If sede.HasDatiBancari Then

                                For Each datoBancario As Ext_DatiBancariInfo In sede.DatiBancari
                                    If datoBancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto Then
                                        anagraficaTrovataDaRestituire.ListaSedi(0).DatiBancari = new List(Of Ext_DatiBancariInfo)
                                        'anagraficaTrovataDaRestituire.ListaSedi(0).DatiBancari.Clear()
                                        anagraficaTrovataDaRestituire.ListaSedi(0).DatiBancari.Add(datoBancario)
                                    End If

                                Next
                            End If

                            Exit For
                        End If
                    Next
                Next

                anagraficaTrovataDaRestituire.Fattura = New Ext_FatturaInfoHeader
                anagraficaTrovataDaRestituire.Fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario
                anagraficaTrovataDaRestituire.Fattura.DescrizioneFattura = fattura.DescrizioneFattura
                anagraficaTrovataDaRestituire.Fattura.IdDocumento = fattura.IdDocumento
                anagraficaTrovataDaRestituire.Fattura.Prog = fattura.Prog
                anagraficaTrovataDaRestituire.Fattura.IdUnivoco = fattura.IdUnivoco
                anagraficaTrovataDaRestituire.Fattura.IdProgFatturaInLiquidazione = fattura.IdProgFatturaInLiquidazione



                anagraficaTrovataDaRestituire.Contratto.Id = fattura.Contratto.IdContratto
                anagraficaTrovataDaRestituire.Contratto.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                anagraficaTrovataDaRestituire.Contratto.CodiceCIG = fattura.Contratto.CodieCIG
                anagraficaTrovataDaRestituire.Contratto.CodiceCUP = fattura.Contratto.CodieCUP

                anagraficaTrovataDaRestituire.ImportoSpettante = fattura.ImportoLiquidato

                listaAnagraficheResult.Add(anagraficaTrovataDaRestituire)

            Next
            retValue.AddRange(listaAnagraficheResult)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw ex
        End Try

        Return retValue
    End Function

    Public Function InterrogaAnagraficaSIC(ByVal operatore As DllAmbiente.Operatore, ByVal denominazione As String, ByVal codFiscale As String, ByVal pIva As String, ByVal idAnagrafica As String, ByVal idContratto As String) As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Dim anagrafiche As New System.Collections.Generic.List(Of Ext_AnagraficaInfo)
        Try
            Dim contratto As DllDocumentale.ItemContrattoInfo = Nothing

            If Not idContratto Is Nothing AndAlso idContratto.Trim() <> String.Empty Then
                Dim idDocumento As String = CodDocumento()
                If Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty Then
                    Dim contrattoInfoHeader As DllDocumentale.ItemContrattoInfoHeader = New DllDocumentale.ItemContrattoInfoHeader()
                    contrattoInfoHeader.IdContratto = idContratto
                    contrattoInfoHeader.IdDocumento = idDocumento

                    contratto = GetContrattoInfo(operatore, contrattoInfoHeader)
                End If
            End If

            Dim rispostaInterrogaAnagrafica As Array = ClientIntegrazioneSic.MessageAnaMaker.createInterrogazioneAnagraficaMessage(operatore, denominazione, codFiscale, pIva, idAnagrafica, idContratto)
            Dim objAnagrafica As Ext_AnagraficaInfo
            For i As Integer = 0 To UBound(rispostaInterrogaAnagrafica, 1)
                objAnagrafica = New Ext_AnagraficaInfo
                objAnagrafica.ID = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).IdAnagrafica
                objAnagrafica.CodiceFiscale = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).CodiceFiscale

                If (DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Commissioni = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesCommissioni.N) Then
                    objAnagrafica.Commissioni = False
                Else
                    objAnagrafica.Commissioni = True
                End If

                If DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesTipoAnagrafica.F Then
                    objAnagrafica.Tipologia = "F"
                    objAnagrafica.DescrizioneTipologia = "Persona Fisica"

                    Dim dataNascita As Date = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).DataNascita
                    objAnagrafica.DataNascita = dataNascita.ToString("dd/MM/yyyy")

                    If DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.M Then
                        objAnagrafica.Sesso = "M"
                    Else
                        objAnagrafica.Sesso = "F"
                    End If
                    objAnagrafica.Cognome = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Cognome
                    objAnagrafica.Nome = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Nome
                    objAnagrafica.ComuneNascita = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).ComuneNS
                    objAnagrafica.Denominazione = objAnagrafica.Cognome + " " + objAnagrafica.Nome
                Else
                    objAnagrafica.Tipologia = "G"
                    objAnagrafica.DescrizioneTipologia = "Persona Giuridica"
                    objAnagrafica.PartitaIva = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).PartitaIva
                    objAnagrafica.Denominazione = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Denominazione

                    Dim legaleRappresentanteInfo As Ext_LegaleRappresentanteInfo = New Ext_LegaleRappresentanteInfo()

                    legaleRappresentanteInfo.CapResidenza = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).CAPRESLR
                    legaleRappresentanteInfo.CodiceFiscale = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).CodiceFiscaleLR
                    legaleRappresentanteInfo.Cognome = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).CognomeLR
                    legaleRappresentanteInfo.Nome = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).NomeLR
                    legaleRappresentanteInfo.ComuneNascita = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).ComuneNSLR
                    legaleRappresentanteInfo.ComuneResidenza = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).ComuneRESLR
                    If DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).DataNascitaLR <> Nothing Then
                        legaleRappresentanteInfo.DataNascita = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).DataNascitaLR
                    End If
                    legaleRappresentanteInfo.Indirizzo = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).IndirizzoLR
                    If DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).SessoLR <> Nothing Then
                        legaleRappresentanteInfo.Sesso = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).SessoLR
                    End If
                    objAnagrafica.LegaleRappresentante = legaleRappresentanteInfo

                    If (DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).Estero = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Sede_TypesBollo.N) Then
                        objAnagrafica.Estero = False
                    Else
                        objAnagrafica.Estero = True
                    End If
                End If

                Dim listaSedi As Array = DirectCast(rispostaInterrogaAnagrafica(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_Types).ListaSedi
                Dim sede As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Sede_Types
                For j As Integer = 0 To UBound(listaSedi, 1)
                    Dim sedeAnagrafica As Ext_SedeAnagraficaInfo

                    sede = DirectCast(listaSedi(j), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Sede_Types)
                    If Not sede Is Nothing Then
                        sedeAnagrafica = New Ext_SedeAnagraficaInfo

                        If (sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Sede_TypesBollo.N) Then
                            sedeAnagrafica.Bollo = False
                        Else
                            sedeAnagrafica.Bollo = True
                        End If

                        'CT- corretto avvaloramento Comune
                        sedeAnagrafica.Comune = sede.Comune
                        sedeAnagrafica.Indirizzo = sede.Indirizzo
                        sedeAnagrafica.IdSede = sede.IdSede
                        sedeAnagrafica.NomeSede = sede.NomeSede
                        sedeAnagrafica.Telefono = sede.Telefono
                        sedeAnagrafica.CapComune = sede.CAP
                        sedeAnagrafica.Email = sede.EMail
                        sedeAnagrafica.Fax = sede.Fax
                        'CT- avvalorati i campi modalità pagamento (descrizione ed id)
                        sedeAnagrafica.ModalitaPagamento = sede.DescTipoPagamento
                        sedeAnagrafica.IdModalitaPagamento = sede.IdTipoPagamento

                        Dim listatipologiaPagamento As Generic.List(Of DllDocumentale.TipoPagamentoInfo) = (New DllDocumentale.svrDocumenti(operatore)).GetTipologiePagamentoSIC(sede.IdTipoPagamento)

                        If listatipologiaPagamento.Count <= 0 Then
                            Log.Error("Modalità di pagamento non corretta.")
                            Throw New Exception("Modalità di pagamento non corretta.")
                        Else
                            If listatipologiaPagamento.Count = 1 Then
                                If listatipologiaPagamento.Item(0).ObbligoIBAN Or listatipologiaPagamento.Item(0).ObbligoCC Then
                                    sedeAnagrafica.HasDatiBancari = True
                                    sedeAnagrafica.IstitutoRiferimento = IIf(Not String.IsNullOrEmpty(listatipologiaPagamento.Item(0).IstitutoRiferimento), listatipologiaPagamento.Item(0).IstitutoRiferimento, "")
                                End If
                            Else
                                Log.Error("Modalità di pagamento non corretta.")
                                Throw New Exception("Modalità di pagamento non corretta.")
                            End If
                        End If

                        Dim listaCC As Array = sede.ListaDatiBancari
                        Dim datibancari As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.DatiBancari_Types

                        For y As Integer = 0 To UBound(listaCC, 1)
                            datibancari = DirectCast(listaCC(y), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.DatiBancari_Types)
                            Dim datiCCSede As Ext_DatiBancariInfo
                            If Not datibancari Is Nothing Then
                                If sedeAnagrafica.DatiBancari Is Nothing Then
                                    sedeAnagrafica.DatiBancari = New Generic.List(Of Ext_DatiBancariInfo)
                                End If
                                datiCCSede = New Ext_DatiBancariInfo
                                datiCCSede.Abi = datibancari.ABI
                                datiCCSede.Cab = datibancari.CAB
                                datiCCSede.ContoCorrente = datibancari.ContoCorrente
                                datiCCSede.Cin = datibancari.CIN
                                datiCCSede.Iban = datibancari.IBAN
                                datiCCSede.IdAgenzia = datibancari.IdAgenzia
                                datiCCSede.IdContoCorrente = datibancari.IdContoCorrente
                                datiCCSede.IndirizzoAgenzia = datibancari.Indirizzo
                                If datibancari.ModalitaPrincipale.HasValue Then
                                    If (datibancari.ModalitaPrincipale.Value = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.DatiBancari_TypesModalitaPrincipale.N) Then
                                        datiCCSede.ModalitaPrincipale = False
                                    Else
                                        datiCCSede.ModalitaPrincipale = True
                                    End If
                                Else
                                    datibancari.ModalitaPrincipale = False
                                End If
                                datiCCSede.NomeBanca = datibancari.NomeBanca
                                datiCCSede.SedeAgenzia = datibancari.SedeAgenzia
                                'CT- A T T E N Z I O N E - Provincia agenzia e cap agenzia non presenti nella risposta da SIC
                                'objAnagrafica.DatiBancari.ProvinciaAgenzia = 
                                'objAnagrafica.DatiBancari.CapCittaAgenzia
                                sedeAnagrafica.DatiBancari.Add(datiCCSede)
                            End If
                        Next

                        If objAnagrafica.ListaSedi Is Nothing Then
                            objAnagrafica.ListaSedi = New Generic.List(Of Ext_SedeAnagraficaInfo)
                        End If
                        objAnagrafica.ListaSedi.Add(sedeAnagrafica)
                    End If

                Next

                objAnagrafica.Contratto = New Ext_ContrattoInfo()

                If Not idContratto Is Nothing AndAlso idContratto.Trim() <> String.Empty Then
                    objAnagrafica.Contratto.Id = idContratto
                    If Not contratto Is Nothing Then
                        objAnagrafica.Contratto.NumeroRepertorio = contratto.NumeroRepertorioContratto
                        objAnagrafica.Contratto.CodiceCIG = contratto.CodieCIG
                        objAnagrafica.Contratto.CodiceCUP = contratto.CodieCUP
                    End If
                End If

                anagrafiche.Add(objAnagrafica)

            Next
        Catch listaVuotaEx As ClientIntegrazioneSic.ListaVuotaException
            Log.Warn(listaVuotaEx.ToString)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw ex
        End Try

        Return anagrafiche
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/CreaAnagrafica")>
    Public Sub CreaAnagrafica()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            'valori della richiesta

            Dim objAnagrafica As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_Types
            Dim valoreLetto As String = ""
            valoreLetto = HttpContext.Current.Request.Item("TipoAnagrafica")

            If UCase(valoreLetto) = "FISICA" Then
                objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.F
            Else
                objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.G
            End If
            valoreLetto = HttpContext.Current.Request.Item("CodiceFiscale")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.CodiceFiscale = HttpContext.Current.Request.Item("CodiceFiscale")
            End If
            valoreLetto = HttpContext.Current.Request.Item("Pignoramento")
            objAnagrafica.PignoramentoSpecified = True
            If Not String.IsNullOrEmpty(valoreLetto) And valoreLetto = "on" Then
                objAnagrafica.Pignoramento = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesPignoramento.S
            Else
                objAnagrafica.Pignoramento = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesPignoramento.N
            End If
            valoreLetto = HttpContext.Current.Request.Item("Commissioni")
            objAnagrafica.CommissioniSpecified = True
            If Not String.IsNullOrEmpty(valoreLetto) And valoreLetto = "on" Then
                objAnagrafica.Commissioni = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesCommissioni.S
            Else
                objAnagrafica.Commissioni = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesCommissioni.N
            End If
            valoreLetto = HttpContext.Current.Request.Item("NotaPignoramento")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.NotaPignoramento = HttpContext.Current.Request.Item("NotaPignoramento")
            Else
                objAnagrafica.NotaPignoramento = ""
            End If

            'Persona Fisica
            If objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesTipoAnagrafica.F Then
                valoreLetto = HttpContext.Current.Request.Item("Cognome")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Cognome = HttpContext.Current.Request.Item("Cognome")
                Else
                    objAnagrafica.Cognome = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("Nome")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Nome = HttpContext.Current.Request.Item("Nome")
                Else
                    objAnagrafica.Nome = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("AltriNomi")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.AltriNomi = HttpContext.Current.Request.Item("AltriNomi")
                Else
                    objAnagrafica.AltriNomi = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneNS")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneNS = HttpContext.Current.Request.Item("ComuneNS")
                Else
                    objAnagrafica.ComuneNS = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("Sesso")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.SessoSpecified = True
                    If UCase(valoreLetto) = "FEMMINA" Then
                        objAnagrafica.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.F
                    Else
                        objAnagrafica.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.M
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("DataNascita")

                If IsDate(valoreLetto) Then
                    objAnagrafica.DataNascitaSpecified = True
                    objAnagrafica.DataNascita = CDate(HttpContext.Current.Request.Item("DataNascita"))
                Else
                    objAnagrafica.DataNascita = Nothing
                End If
            End If
            'Persona Giuridica
            If objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesTipoAnagrafica.G Then
                valoreLetto = HttpContext.Current.Request.Item("Estero")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.EsteroSpecified = True
                    If valoreLetto = "on" Then
                        objAnagrafica.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.S
                    Else
                        objAnagrafica.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.N
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("Denominazione")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Denominazione = HttpContext.Current.Request.Item("Denominazione")
                Else
                    objAnagrafica.Denominazione = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("PartitaIva")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.PartitaIva = HttpContext.Current.Request.Item("PartitaIva")
                Else
                    objAnagrafica.PartitaIva = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CodiceFiscaleLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CodiceFiscaleLR = HttpContext.Current.Request.Item("CodiceFiscaleLR")
                Else
                    objAnagrafica.CodiceFiscaleLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CognomeLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CognomeLR = HttpContext.Current.Request.Item("CognomeLR")
                Else
                    objAnagrafica.CognomeLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("NomeLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.NomeLR = HttpContext.Current.Request.Item("NomeLR")
                Else
                    objAnagrafica.NomeLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("SessoLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.SessoLRSpecified = True
                    If UCase(valoreLetto) = "FEMMINA" Then
                        objAnagrafica.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.F
                    Else
                        objAnagrafica.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.M
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneNSLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneNSLR = HttpContext.Current.Request.Item("ComuneNSLR")
                Else
                    objAnagrafica.ComuneNSLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneRESLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneRESLR = HttpContext.Current.Request.Item("ComuneRESLR")
                Else
                    objAnagrafica.ComuneRESLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("DataNascitaLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DataNascitaLRSpecified = True
                    If IsDate(valoreLetto) Then
                        objAnagrafica.DataNascitaLR = CDate(HttpContext.Current.Request.Item("DataNascitaLR"))
                    Else
                        objAnagrafica.DataNascitaLR = Nothing
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("IndirizzoLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.IndirizzoLR = HttpContext.Current.Request.Item("IndirizzoLR")
                Else
                    objAnagrafica.IndirizzoLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CAPRESLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CAPRESLR = HttpContext.Current.Request.Item("CAPRESLR")
                Else
                    objAnagrafica.CAPRESLR = ""
                End If
            End If
            objAnagrafica.Sede = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_Types
            valoreLetto = HttpContext.Current.Request.Item("Bollo")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.BolloSpecified = True
                If valoreLetto = "on" Then
                    objAnagrafica.Sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.S
                Else
                    objAnagrafica.Sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.N
                End If
            End If
            valoreLetto = HttpContext.Current.Request.Item("ComuneSede")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.Comune = HttpContext.Current.Request.Item("ComuneSede")
            Else
                objAnagrafica.Sede.Comune = ""
            End If
            valoreLetto = HttpContext.Current.Request.Item("CAP")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.CAP = HttpContext.Current.Request.Item("CAP")
            Else
                objAnagrafica.Sede.CAP = ""
            End If

            valoreLetto = HttpContext.Current.Request.Item("IndirizzoSede")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.Indirizzo = HttpContext.Current.Request.Item("IndirizzoSede")
            Else
                objAnagrafica.Sede.Indirizzo = ""
            End If
            valoreLetto = HttpContext.Current.Request.Item("NomeSede")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.NomeSede = HttpContext.Current.Request.Item("NomeSede")
            Else
                objAnagrafica.Sede.NomeSede = "--"
            End If
            valoreLetto = HttpContext.Current.Request.Item("Telefono")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.Telefono = HttpContext.Current.Request.Item("Telefono")
            Else
                objAnagrafica.Sede.Telefono = ""
            End If

            valoreLetto = HttpContext.Current.Request.Item("EMail")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.EMail = HttpContext.Current.Request.Item("EMail")
            Else
                objAnagrafica.Sede.EMail = ""
            End If
            valoreLetto = HttpContext.Current.Request.Item("Fax")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.Sede.Fax = HttpContext.Current.Request.Item("Fax")
            Else
                objAnagrafica.Sede.Fax = ""
            End If

            objAnagrafica.Sede.TipoPagamento = HttpContext.Current.Request.Item("comboModalitaPagamento")

            objAnagrafica.DatiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types

            If (Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("IBAN"))) Or (Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("ContoCorrente"))) Then
                valoreLetto = HttpContext.Current.Request.Item("IBAN")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.IBAN = HttpContext.Current.Request.Item("IBAN")
                Else
                    objAnagrafica.DatiBancari.IBAN = ""
                End If

                valoreLetto = HttpContext.Current.Request.Item("ABI")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.ABI = HttpContext.Current.Request.Item("ABI")
                Else
                    objAnagrafica.DatiBancari.ABI = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CAB")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.CAB = HttpContext.Current.Request.Item("CAB")
                Else
                    objAnagrafica.DatiBancari.CAB = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ContoCorrente")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.ContoCorrente = HttpContext.Current.Request.Item("ContoCorrente")
                Else
                    objAnagrafica.DatiBancari.ContoCorrente = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CIN")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.CIN = HttpContext.Current.Request.Item("CIN")
                Else
                    objAnagrafica.DatiBancari.CIN = ""
                End If


                valoreLetto = HttpContext.Current.Request.Item("IndirizzoAgenzia")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.Indirizzo = HttpContext.Current.Request.Item("IndirizzoAgenzia")
                Else
                    objAnagrafica.DatiBancari.Indirizzo = ""
                End If

                valoreLetto = HttpContext.Current.Request.Item("ModalitaPrincipale")
                objAnagrafica.DatiBancari.ModalitaPrincipaleSpecified = True
                If valoreLetto = "on" Then
                    objAnagrafica.DatiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.S
                Else
                    objAnagrafica.DatiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.N
                End If
                valoreLetto = HttpContext.Current.Request.Item("NomeBanca")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.NomeBanca = HttpContext.Current.Request.Item("NomeBanca")
                Else
                    objAnagrafica.DatiBancari.NomeBanca = ""
                End If

                valoreLetto = HttpContext.Current.Request.Item("ComuneAgenzia")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.SedeAgenzia = HttpContext.Current.Request.Item("ComuneAgenzia")
                    objAnagrafica.DatiBancari.Citta = HttpContext.Current.Request.Item("ComuneAgenzia")
                Else
                    objAnagrafica.DatiBancari.Citta = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ProvinciaAgenzia")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.Provincia = HttpContext.Current.Request.Item("ProvinciaAgenzia")
                Else
                    objAnagrafica.DatiBancari.Provincia = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CAPAgenzia")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DatiBancari.CAP = HttpContext.Current.Request.Item("CAPAgenzia")
                Else
                    objAnagrafica.DatiBancari.CAP = ""
                End If
            End If
            Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_CreaAnagrafica_Types = ClientIntegrazioneSic.MessageAnaMaker.createCreaAnagraficaMessage(operatore, objAnagrafica)


            Dim result As String = ""

            result = "{  success: true, id: '" + CStr(returnOggetto.IdAnagrafica) + "', IdAnagrafica: " + returnOggetto.IdAnagrafica + ", IdSede: " + returnOggetto.IdSede + ", IdTipoPagamento: " + returnOggetto.IdTipoPagamento + ", IdContoCorrente: " + returnOggetto.IdContoCorrente + " }"

            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" + Replace(ex.Message, "'", "\'") + "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/CreaSede?idAnagrafica={idAnagrafica}")>
    Public Sub CreaSede(ByVal idAnagrafica As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            'valori della richiesta

            If Not String.IsNullOrEmpty(idAnagrafica) Then

                If String.IsNullOrEmpty(HttpContext.Current.Request.Item("IdModalitaPagamento")) Then
                    Log.Error("Modalità di pagamento non corretta.")
                    Throw New Exception("Modalità di pagamento non corretta.")
                End If

                Dim sede As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_Types


                sede.BolloSpecified = True
                If HttpContext.Current.Request.Item("Bollo") = "on" Then
                    sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.S
                Else
                    sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.N
                End If

                sede.Comune = HttpContext.Current.Request.Item("ComuneSede") & ""
                sede.Indirizzo = HttpContext.Current.Request.Item("IndirizzoSede") & ""
                sede.NomeSede = HttpContext.Current.Request.Item("NomeSede") & ""
                sede.Telefono = HttpContext.Current.Request.Item("Telefono") & ""
                sede.CAP = HttpContext.Current.Request.Item("CAP") & ""
                sede.EMail = HttpContext.Current.Request.Item("EMail") & ""
                sede.Fax = HttpContext.Current.Request.Item("Fax") & ""


                Dim listatipologiaPagamento As Generic.List(Of DllDocumentale.TipoPagamentoInfo) = (New DllDocumentale.svrDocumenti(operatore)).GetTipologiePagamentoSIC(HttpContext.Current.Request.Item("IdModalitaPagamento"))
                Dim datiBancari As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types

                If listatipologiaPagamento.Count <= 0 Then
                    Log.Error("Modalità di pagamento non corretta.")
                    Throw New Exception("Modalità di pagamento non corretta.")
                Else
                    If listatipologiaPagamento.Count = 1 Then
                        sede.TipoPagamento = DirectCast(listatipologiaPagamento.Item(0), DllDocumentale.TipoPagamentoInfo).Descrizione
                        If listatipologiaPagamento.Item(0).ObbligoIBAN Or listatipologiaPagamento.Item(0).ObbligoCC Then
                            datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                            datiBancari.ABI = HttpContext.Current.Request.Item("ABI")
                            datiBancari.CAB = HttpContext.Current.Request.Item("CAB")
                            datiBancari.ContoCorrente = HttpContext.Current.Request.Item("ContoCorrente")
                            datiBancari.CIN = HttpContext.Current.Request.Item("CIN")
                            datiBancari.IBAN = HttpContext.Current.Request.Item("IBAN")
                            datiBancari.Indirizzo = HttpContext.Current.Request.Item("IndirizzoAgenzia")
                            datiBancari.ModalitaPrincipaleSpecified = True
                            If HttpContext.Current.Request.Item("ModalitaPrincipale") = "on" Then
                                datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.S
                            Else
                                datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.N
                            End If
                            datiBancari.NomeBanca = HttpContext.Current.Request.Item("NomeBanca")
                            datiBancari.SedeAgenzia = HttpContext.Current.Request.Item("ComuneAgenzia")
                            datiBancari.Citta = HttpContext.Current.Request.Item("ComuneAgenzia")
                            datiBancari.Provincia = HttpContext.Current.Request.Item("ProvinciaAgenzia")
                            datiBancari.CAP = HttpContext.Current.Request.Item("CAPAgenzia")
                        Else
                            datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                        End If
                    Else
                        Log.Error("Modalità di pagamento non corretta.")
                        Throw New Exception("Modalità di pagamento non corretta.")
                    End If

                End If

                Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_CreaSede_Types = ClientIntegrazioneSic.MessageAnaMaker.createCreaSedeMessage(operatore, idAnagrafica, sede, datiBancari)

                Dim result As String = ""
                result = "{  ""success"": true, IdSede: " + returnOggetto.IdSede + ", IdTipoPagamento: " + returnOggetto.IdTipoPagamento + ", IdContoCorrente: " + returnOggetto.IdContoCorrente + " }"

                HttpContext.Current.Response.Write(result)
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/CreaContoBancario?idSede={idSede}&idAnagrafica={idAnagrafica}")>
    Public Sub CreaContoBancario(ByVal idSede As String, ByVal idAnagrafica As String)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            'valori della richiesta


            If Not String.IsNullOrEmpty(idAnagrafica) Or Not String.IsNullOrEmpty(idSede) Then
                Dim datiBancari As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                datiBancari.ABI = HttpContext.Current.Request.Item("ABI")
                datiBancari.CAB = HttpContext.Current.Request.Item("CAB")
                datiBancari.ContoCorrente = HttpContext.Current.Request.Item("ContoCorrente")
                datiBancari.CIN = HttpContext.Current.Request.Item("CIN")
                datiBancari.IBAN = HttpContext.Current.Request.Item("IBAN")
                datiBancari.Indirizzo = HttpContext.Current.Request.Item("IndirizzoAgenzia")

                datiBancari.ModalitaPrincipaleSpecified = True
                If HttpContext.Current.Request.Item("ModalitaPrincipale") = "on" Then
                    datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.S
                Else
                    datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.N
                End If
                datiBancari.NomeBanca = HttpContext.Current.Request.Item("NomeBanca")
                datiBancari.SedeAgenzia = HttpContext.Current.Request.Item("ComuneAgenzia")
                datiBancari.Citta = HttpContext.Current.Request.Item("ComuneAgenzia")
                datiBancari.Provincia = HttpContext.Current.Request.Item("ProvinciaAgenzia")
                datiBancari.CAP = HttpContext.Current.Request.Item("CAPAgenzia")

                Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_CreaContoBancario_Types = ClientIntegrazioneSic.MessageAnaMaker.createCreaContoBancarioMessage(operatore, idAnagrafica, idSede, datiBancari)

                Dim result As String = ""

                result = "{  ""success"": true, IdContoCorrente: " + returnOggetto.IdContoCorrente + " }"
                HttpContext.Current.Response.Write(result)
            Else
                Dim result = "{  ""success"": false }"
                HttpContext.Current.Response.Write(result)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetTipologiePagamentoSic?id={id}")>
    Public Function GetTipologiePagamentoSic(ByVal id As String) As Generic.List(Of DllDocumentale.TipoPagamentoInfo)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim listaTipologiePagamenti As New Generic.List(Of DllDocumentale.TipoPagamentoInfo)
            Dim svrDoc As New DllDocumentale.svrDocumenti(operatore)
            If String.IsNullOrEmpty(id) Then
                listaTipologiePagamenti = svrDoc.GetTipologiePagamentoSIC()
            Else
                listaTipologiePagamenti = svrDoc.GetTipologiePagamentoSIC(id)
            End If


            Return listaTipologiePagamenti
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/InterrogaComune?descrizioneComune={descrizioneComune}")>
    Public Function InterrogaComune(ByVal descrizioneComune As String) As List(Of Ext_ComuneInfo)
        Try
            If String.IsNullOrEmpty(descrizioneComune) Then
                descrizioneComune = HttpContext.Current.Request.Params("query")
            End If
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim svrDoc As New DllDocumentale.svrDocumenti(operatore)
            Dim listaComune_Return As New List(Of Ext_ComuneInfo)
            Dim ext_ComuneInfo As Ext_ComuneInfo
            Dim listacomuni As Array
            If Not String.IsNullOrEmpty(descrizioneComune) Then
                descrizioneComune = descrizioneComune.Replace("'", "''")
                listacomuni = ClientIntegrazioneSic.MessageAnaMaker.createInterrogazioneComuniMessage(operatore, descrizioneComune)
                For i As Integer = 0 To UBound(listacomuni, 1)
                    ext_ComuneInfo = New Ext_ComuneInfo
                    ext_ComuneInfo.CodiceIstat = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).CodiceIstat
                    ext_ComuneInfo.Cap = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).CAP
                    ext_ComuneInfo.Descrizione = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).Descrizione
                    ext_ComuneInfo.Provincia = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).Provincia
                    ext_ComuneInfo.CodProvincia = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).CodProvincia
                    ext_ComuneInfo.Regione = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).Regione
                    ext_ComuneInfo.ID = DirectCast(listacomuni(i), ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Comune_Types).IdComune
                    listaComune_Return.Add(ext_ComuneInfo)
                Next

                Return listaComune_Return
            End If

        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), "La ricerca non ha prodotto risultati!")

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex))
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/UpdateAnagrafica")>
    Public Sub UpdateAnagrafica()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            'valori della richiesta
            Dim idAnagrafica As String = HttpContext.Current.Request.Item("idAnagrafica")

            Dim objAnagrafica As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_Types
            Dim valoreLetto As String = ""
            valoreLetto = HttpContext.Current.Request.Item("TipoAnagrafica")
            If UCase(valoreLetto) = "FISICA" Then
                objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.F
            Else
                objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.G
            End If
            valoreLetto = HttpContext.Current.Request.Item("CodiceFiscale")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.CodiceFiscale = HttpContext.Current.Request.Item("CodiceFiscale")
            End If
            valoreLetto = HttpContext.Current.Request.Item("Pignoramento")
            objAnagrafica.PignoramentoSpecified = True
            If Not String.IsNullOrEmpty(valoreLetto) And valoreLetto = "on" Then
                objAnagrafica.Pignoramento = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesPignoramento.S
            Else
                objAnagrafica.Pignoramento = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesPignoramento.N
            End If
            valoreLetto = HttpContext.Current.Request.Item("Commissioni")
            objAnagrafica.CommissioniSpecified = True
            If Not String.IsNullOrEmpty(valoreLetto) And valoreLetto = "on" Then
                objAnagrafica.Commissioni = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesCommissioni.S
            Else
                objAnagrafica.Commissioni = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesCommissioni.N
            End If
            valoreLetto = HttpContext.Current.Request.Item("NotaPignoramento")
            If Not String.IsNullOrEmpty(valoreLetto) Then
                objAnagrafica.NotaPignoramento = HttpContext.Current.Request.Item("NotaPignoramento")
            Else
                objAnagrafica.NotaPignoramento = ""
            End If

            'Persona Fisica
            If objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesTipoAnagrafica.F Then
                valoreLetto = HttpContext.Current.Request.Item("Cognome")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Cognome = HttpContext.Current.Request.Item("Cognome")
                Else
                    objAnagrafica.Cognome = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("Nome")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Nome = HttpContext.Current.Request.Item("Nome")
                Else
                    objAnagrafica.Nome = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("AltriNomi")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.AltriNomi = HttpContext.Current.Request.Item("AltriNomi")
                Else
                    objAnagrafica.AltriNomi = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneNS")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneNS = HttpContext.Current.Request.Item("ComuneNS")
                Else
                    objAnagrafica.ComuneNS = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("Sesso")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.SessoSpecified = True
                    If UCase(valoreLetto) = "FEMMINA" Then
                        objAnagrafica.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.F
                    Else
                        objAnagrafica.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesSesso.M
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("DataNascita")
                If IsDate(valoreLetto) Then
                    objAnagrafica.DataNascitaSpecified = True
                    objAnagrafica.DataNascita = CDate(HttpContext.Current.Request.Item("DataNascita"))
                Else
                    objAnagrafica.DataNascita = Nothing
                End If
            End If
            'Persona Giuridica
            If objAnagrafica.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Anagrafica_TypesTipoAnagrafica.G Then
                valoreLetto = HttpContext.Current.Request.Item("Estero")
                objAnagrafica.EsteroSpecified = True
                If Not String.IsNullOrEmpty(valoreLetto) And valoreLetto = "on" Then
                    objAnagrafica.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.S
                Else
                    objAnagrafica.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.N
                End If
                valoreLetto = HttpContext.Current.Request.Item("Denominazione")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.Denominazione = HttpContext.Current.Request.Item("Denominazione")
                Else
                    objAnagrafica.Denominazione = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("PartitaIva")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.PartitaIva = HttpContext.Current.Request.Item("PartitaIva")
                Else
                    objAnagrafica.PartitaIva = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CodiceFiscaleLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CodiceFiscaleLR = HttpContext.Current.Request.Item("CodiceFiscaleLR")
                Else
                    objAnagrafica.CodiceFiscaleLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CognomeLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CognomeLR = HttpContext.Current.Request.Item("CognomeLR")
                Else
                    objAnagrafica.CognomeLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("NomeLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.NomeLR = HttpContext.Current.Request.Item("NomeLR")
                Else
                    objAnagrafica.NomeLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneNSLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneNSLR = HttpContext.Current.Request.Item("ComuneNSLR")
                Else
                    objAnagrafica.ComuneNSLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("ComuneRESLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.ComuneRESLR = HttpContext.Current.Request.Item("ComuneRESLR")
                Else
                    objAnagrafica.ComuneRESLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("DataNascitaLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.DataNascitaLRSpecified = True
                    If IsDate(valoreLetto) Then
                        objAnagrafica.DataNascitaLR = CDate(HttpContext.Current.Request.Item("DataNascitaLR"))
                    Else
                        objAnagrafica.DataNascitaLR = Nothing
                    End If
                End If
                valoreLetto = HttpContext.Current.Request.Item("IndirizzoLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.IndirizzoLR = HttpContext.Current.Request.Item("IndirizzoLR")
                Else
                    objAnagrafica.IndirizzoLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("CAPRESLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.CAPRESLR = HttpContext.Current.Request.Item("CAPRESLR")
                Else
                    objAnagrafica.CAPRESLR = ""
                End If
                valoreLetto = HttpContext.Current.Request.Item("SessoLR")
                If Not String.IsNullOrEmpty(valoreLetto) Then
                    objAnagrafica.SessoLRSpecified = True
                    If UCase(valoreLetto) = "FEMMINA" Then
                        objAnagrafica.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSessoLR.F
                    Else
                        objAnagrafica.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSessoLR.M
                    End If
                End If

            End If

            Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ModificaAnagrafica_Types = ClientIntegrazioneSic.MessageAnaMaker.createUpdateAnagraficaMessage(operatore, idAnagrafica, objAnagrafica)

            Dim result As String = ""

            result = "{  success: true, id: '" + CStr(returnOggetto.IdAnagrafica) + "' }"

            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
            '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/UpdateSede")>
    Public Sub UpdateSede()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            'valori della richiesta

            Dim idAnagrafica As String = HttpContext.Current.Request.Item("idAnagrafica")
            Dim idSede As String = HttpContext.Current.Request.Item("idSede")

            Dim sede As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_Types

            sede.BolloSpecified = True
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("Bollo")) And HttpContext.Current.Request.Item("Bollo") = "on" Then
                sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.S
            Else
                sede.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.N
            End If

            sede.Comune = HttpContext.Current.Request.Item("ComuneSede")
            sede.Indirizzo = HttpContext.Current.Request.Item("IndirizzoSede")
            sede.NomeSede = HttpContext.Current.Request.Item("NomeSede")
            sede.Telefono = HttpContext.Current.Request.Item("Telefono")
            sede.CAP = HttpContext.Current.Request.Item("CAP")
            sede.EMail = HttpContext.Current.Request.Item("EMail")
            sede.Fax = HttpContext.Current.Request.Item("Fax")

            Dim listatipologiaPagamento As Generic.List(Of DllDocumentale.TipoPagamentoInfo) = (New DllDocumentale.svrDocumenti(operatore)).GetTipologiePagamentoSIC(HttpContext.Current.Request.Item("IdModalitaPagamento"))
            Dim datiBancari As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types

            If listatipologiaPagamento.Count <= 0 Then
                Log.Error("Modalità di pagamento non corretta.")
                Throw New Exception("Modalità di pagamento non corretta.")
            Else
                If listatipologiaPagamento.Count = 1 Then
                    sede.TipoPagamento = DirectCast(listatipologiaPagamento.Item(0), DllDocumentale.TipoPagamentoInfo).Descrizione
                    If listatipologiaPagamento.Item(0).ObbligoIBAN Or listatipologiaPagamento.Item(0).ObbligoCC Then
                        datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                        datiBancari.ABI = HttpContext.Current.Request.Item("ABI")
                        datiBancari.CAB = HttpContext.Current.Request.Item("CAB")
                        datiBancari.ContoCorrente = HttpContext.Current.Request.Item("ContoCorrente")
                        datiBancari.CIN = HttpContext.Current.Request.Item("CIN")
                        datiBancari.IBAN = HttpContext.Current.Request.Item("IBAN")
                        datiBancari.Indirizzo = HttpContext.Current.Request.Item("Indirizzo")
                        datiBancari.ModalitaPrincipale = HttpContext.Current.Request.Item("ModalitaPrincipale")
                        datiBancari.NomeBanca = HttpContext.Current.Request.Item("NomeBanca")
                        datiBancari.SedeAgenzia = HttpContext.Current.Request.Item("SedeAgenzia")
                    Else
                        datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
                    End If
                Else
                    Log.Error("Modalità di pagamento non corretta.")
                    Throw New Exception("Modalità di pagamento non corretta.")
                End If

            End If

            Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ModificaSede_Types = ClientIntegrazioneSic.MessageAnaMaker.createUpdateSedeMessage(operatore, idAnagrafica, idSede, sede)

            Dim result As String = "{  ""success"": true }"
            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/UpdateContoBancario")>
    Public Sub UpdateContoBancario()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            'valori della richiesta

            Dim idAnagrafica As String = HttpContext.Current.Request.Item("idAnagrafica")
            Dim idSede As String = HttpContext.Current.Request.Item("idSede")
            Dim idConto As String = HttpContext.Current.Request.Item("idConto")

            Dim datiBancari As New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
            datiBancari = New ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types
            datiBancari.ABI = HttpContext.Current.Request.Item("ABI")
            datiBancari.CAB = HttpContext.Current.Request.Item("CAB")
            datiBancari.ContoCorrente = HttpContext.Current.Request.Item("ContoCorrente")
            datiBancari.CIN = HttpContext.Current.Request.Item("CIN")
            datiBancari.IBAN = HttpContext.Current.Request.Item("IBAN")
            datiBancari.Indirizzo = HttpContext.Current.Request.Item("Indirizzo")

            If HttpContext.Current.Request.Item("ModalitaPrincipale") Then
                datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.S
            Else
                datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.N
            End If

            datiBancari.NomeBanca = HttpContext.Current.Request.Item("NomeBanca")
            datiBancari.SedeAgenzia = HttpContext.Current.Request.Item("SedeAgenzia")
            Dim returnOggetto As ClientIntegrazioneSic.Intema.WS.Ana.Risposta.Risposta_ModificaContoBancario_Types = ClientIntegrazioneSic.MessageAnaMaker.createUpdateContoBancarioMessage(operatore, idAnagrafica, idSede, idConto, datiBancari)

            Dim result As String = ""

            result = "{  ""success"": true }"
            HttpContext.Current.Response.Write(result)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Dim msgErr = Replace(ex.Message, """", "'")
            HttpContext.Current.Response.Write("{  success: false, ""FaultMessage"": """ + msgErr + """ }")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetlistaBeneficiariLiquidazioni?ID={ID}")>
    Public Function GetlistaBeneficiariLiquidazioni(ByVal ID As Integer) As IList(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Dim listaLiquidazioneBeneficiarioInfo As List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Try

            Log.Info("Inizio metodo " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            listaLiquidazioneBeneficiarioInfo = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaBeneficiariLiquidazione(operatore, , , ID)


            '******** Questa parte di codice serve per caricare i dati dell'anagrafica del beneficiario
            '******** direttamente dal SIC, per avere i dati del dettaglio corretti. 
            '******** E' stata commentata perchè la chiama al SIC impiega troppo tempo. 
            '******** Nel caso il web service venga migliorato, sarebbe opportuno decommentare.
            'Dim listaAnagraficaSIC As System.Collections.Generic.List(Of Ext_AnagraficaInfo)
            'For Each beneficiarioLiq As DllDocumentale.ItemLiquidazioneBeneficiarioInfo In listaLiquidazioneBeneficiarioInfo
            '    listaAnagraficaSIC = InterrogaAnagraficaSIC(operatore, "", "", "", beneficiarioLiq.IdAnagrafica)
            '    For Each anagraficaSIC As Ext_AnagraficaInfo In listaAnagraficaSIC
            '        If (anagraficaSIC.Tipologia = "F") Then
            '            beneficiarioLiq.Denominazione = anagraficaSIC.Cognome + " " + anagraficaSIC.Nome
            '            beneficiarioLiq.CodiceFiscale = anagraficaSIC.CodiceFiscale
            '        Else
            '            beneficiarioLiq.Denominazione = anagraficaSIC.Denominazione
            '            beneficiarioLiq.PartitaIva = anagraficaSIC.PartitaIva
            '        End If
            '    Next
            'Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaLiquidazioneBeneficiarioInfo
    End Function



    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetlistaBeneficiariImpegno?ID={ID}")>
    Public Function GetlistaBeneficiariImpegno(ByVal ID As Integer) As IList(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Dim listaImpegnoBeneficiarioInfo As New List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Try

            Log.Info("Inizio metodo " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim listaBeneficiariImpegni As List(Of ItemLiquidazioneImpegnoBeneficiarioInfo) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaBeneficiariImpegno(operatore, , , ID)
            For Each impegnoBen As ItemLiquidazioneImpegnoBeneficiarioInfo In listaBeneficiariImpegni
                listaImpegnoBeneficiarioInfo.Add(impegnoBen)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaImpegnoBeneficiarioInfo
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaDestinatariDocumento?IdDocumento={IdDocumento}")>
    Public Function GetListaDestinatariDocumento(ByVal IdDocumento As Integer) As IList(Of Ext_DestinatarioInfo)
        Dim retValue As New List(Of Ext_DestinatarioInfo)
        Try

            Log.Info("Inizio metodo " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim destinatari As Generic.List(Of DllDocumentale.ItemDestinatarioInfo) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaDestinatari(operatore, IdDocumento)

            For Each destinatario As DllDocumentale.ItemDestinatarioInfo In destinatari
                Dim ext_destinatario As Ext_DestinatarioInfo = New Ext_DestinatarioInfo()

                ext_destinatario.Id = destinatario.Id
                ext_destinatario.isPersonaFisica = destinatario.isPersonaFisica
                ext_destinatario.IdSIC = destinatario.IdSIC
                ext_destinatario.IdDocumento = destinatario.IdDocumento
                ext_destinatario.Denominazione = destinatario.Denominazione
                ext_destinatario.CodiceFiscale = destinatario.CodiceFiscale
                ext_destinatario.PartitaIva = destinatario.PartitaIva
                ext_destinatario.DataNascita = IIf(destinatario.DataNascita Is Nothing, String.Empty, destinatario.DataNascita)
                ext_destinatario.LuogoNascita = destinatario.LuogoNascita
                ext_destinatario.LegaleRappresentante = destinatario.LegaleRappresentante
                ext_destinatario.IdContratto = destinatario.IdContratto
                ext_destinatario.NumeroRepertorioContratto = destinatario.NumeroRepertorioContratto
                ext_destinatario.isDatoSensibile = destinatario.isDatoSensibile

                retValue.Add(ext_destinatario)
            Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return retValue
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaBeneficiariContratti?idDocumento={idDocumento}&idContratto={idContratto}")>
    Public Function GetListaBeneficiariContratti(ByVal idDocumento As String, ByVal idContratto As String) As IList(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Dim listaBeneficiariContratti As List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)
        Try
            Log.Info("Inizio metodo " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            listaBeneficiariContratti = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_Contratti_Beneficiari(operatore, idDocumento, idContratto, True)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaBeneficiariContratti
    End Function


    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/EliminaBeneficiarioLiquidazioneImpegno")>
    Public Function EliminaBeneficiarioLiquidazioneImpegno(ByVal ID As Integer, ByVal IDDocumento As String, ByVal IDDocContabile As Integer, ByVal IsImpegno As Boolean) As Boolean
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim ddlDocumentale As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            Dim oggettoDaCancellare As New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo()
            oggettoDaCancellare.ID = ID
            oggettoDaCancellare.IDDocumentoContabile = IDDocContabile
            oggettoDaCancellare.IdDocumento = IDDocumento
            If Not IsImpegno Then
                ddlDocumentale.FO_Delete_DocumentoLiquidazioneBeneficiario(operatore, oggettoDaCancellare)
            Else
                ddlDocumentale.FO_Delete_DocumentoImpegnoBeneficiario(operatore, oggettoDaCancellare)
            End If


        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try

        Return True
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaBeneficiariCronologia")>
    Public Function GetListaBeneficiariCronologia() As IList(Of Ext_BeneficiarioCronologiaInfo)
        Dim listaBeneficiariCronologiaExt As List(Of Ext_BeneficiarioCronologiaInfo) = New List(Of Ext_BeneficiarioCronologiaInfo)
        Try
            Log.Info("Inizio metodo GetListaBeneficiariCronologia " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim listaBeneficiariCronologia As List(Of DllDocumentale.BeneficiarioCronologia) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaBeneficiariCronologiaByCodOpe(operatore.Codice)

            For i As Integer = 0 To listaBeneficiariCronologia.Count - 1
                Dim beneficiario As DllDocumentale.BeneficiarioCronologia = listaBeneficiariCronologia.ElementAt(i)

                Dim beneficiarioExt As Ext_BeneficiarioCronologiaInfo = New Ext_BeneficiarioCronologiaInfo()

                beneficiarioExt.CodFiscPIva = beneficiario.CodFiscPIva
                beneficiarioExt.ContatoreFrequenza = beneficiario.ContatoreFrequenza
                beneficiarioExt.IdBeneficiario = beneficiario.IdBeneficiario
                beneficiarioExt.IdSede = beneficiario.IdSede
                beneficiarioExt.IdTipoPagamento = beneficiario.IdTipoPagamento
                beneficiarioExt.IdContoCorrente = beneficiario.IdContoCorrente
                beneficiarioExt.FlagPersonaFisica = beneficiario.FlagPersonaFisica
                beneficiarioExt.Nominativo = beneficiario.Nominativo
                beneficiarioExt.DataNasc = beneficiario.DataNasc
                beneficiarioExt.LuogoNasc = beneficiario.LuogoNasc
                beneficiarioExt.LegaleRappresentante = beneficiario.LegaleRappresentante
                beneficiarioExt.DescrSede = beneficiario.DescrSede
                beneficiarioExt.DescrModPagamento = beneficiario.DescrModPagamento
                beneficiarioExt.DescrDatiBancari = beneficiario.DescrDatiBancari
                beneficiarioExt.DataUltimoUtilizzo = beneficiario.DataUltimoUtilizzo

                listaBeneficiariCronologiaExt.Add(beneficiarioExt)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return listaBeneficiariCronologiaExt
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaContratti?idDocumento={idDocumento}&idContratto={idContratto}")>
    Public Function GetListaContratti(ByVal idDocumento As String, ByVal idContratto As String) As IList(Of Ext_ContrattoInfo)
        Dim contrattiExt As List(Of Ext_ContrattoInfo) = New List(Of Ext_ContrattoInfo)
        Try
            Log.Info("Inizio metodo GetListaContratti " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim contrattiInfoHeader As List(Of DllDocumentale.ItemContrattoInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaContratti(IIf(Not idDocumento Is Nothing AndAlso idDocumento.Trim() <> String.Empty, idDocumento, CodDocumento), idContratto)
            Dim contrattiInfo As Generic.List(Of DllDocumentale.ItemContrattoInfo) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To contrattiInfo.Count - 1
                Dim contratto As DllDocumentale.ItemContrattoInfo = contrattiInfo.ElementAt(i)

                Dim contrattoExt As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                contrattoExt.Id = contratto.IdContratto
                contrattoExt.NumeroRepertorio = contratto.NumeroRepertorioContratto
                contrattoExt.Oggetto = contratto.OggettoContratto
                contrattoExt.CodiceCIG = contratto.CodieCIG
                contrattoExt.CodiceCUP = contratto.CodieCUP

                contrattiExt.Add(contrattoExt)
            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return contrattiExt
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFattureByLiquidazione")>
    Public Function GetListaFattureByLiquidazione() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFatture " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()

            Dim idLiquidazione = HttpContext.Current.Request.Item("idLiquidazione")




            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(idLiquidazione, idDocumento)
            'Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.ImportoLiquidato = fattura.ImportoLiquidato
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.Prog = fattura.Prog
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                ext_fattura.IdLiquidazione = idLiquidazione
                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFattureNonAssegnate")>
    Public Function GetListaFattureNonAssegnate() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFattureNonAssegnate " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()

            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureNonAssegnate(idDocumento)
            'Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFattureNonAssegnateLiquidazione")>
    Public Function GetListaFattureNonAssegnateLiquidazione() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFattureNonAssegnate " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()

            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureNonAssegnateLiquidazione(idDocumento)
            'Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFattureNonAssegnateLiquidazioneOrResidue")>
    Public Function GetListaFattureNonAssegnateLiquidazioneOrResidue() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFattureNonAssegnate " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()




            Dim fattureNonLiquidateOrResudue As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureNonAssegnateLiquidazioneOrResidue(CodDocumento())



            For i As Integer = 0 To fattureNonLiquidateOrResudue.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureNonLiquidateOrResudue.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()


                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione


                ext_fattura.Prog = fattura.Prog
                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura

                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.ImportoLiquidato = fattura.ImportoLiquidato
                ext_fattura.ImportoResiduo = fattura.ImportoResiduo
                ext_fattura.ImportoFattDaLiquidare = fattura.ImportoResiduo


                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFattureNonAssegnateImpegno")>
    Public Function GetListaFattureNonAssegnateImpegno() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFattureNonAssegnate " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()

            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureNonAssegnateImpegno(idDocumento)
            'Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function


    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFatture?idDocumento={idDocumento}&idFattura={idFattura}")>
    Public Function GetListaFatture1(ByVal idDocumento As String, ByVal idFattura As String) As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFatture " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            fattureExt = GetListaFattureAtto(idDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaFatture")>
    Public Function GetListaFatture() As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFatture " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = CodDocumento()

            fattureExt = GetListaFattureAtto(idDocumento)

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function


    Public Function GetListaFattureAtto(ByVal idDoc As String) As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFatture " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)


            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFatture(idDoc)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetAttributoDocumento?codAttributo={codAttributo}")>
    Public Function GetAttributoDocumento(ByVal codAttributo As String) As Ext_AttributoInfo
        Dim attributo As Ext_AttributoInfo = New Ext_AttributoInfo
        Try
            Dim docAttributo As New DllDocumentale.Documento_attributo
            docAttributo.Doc_id = CodDocumento()
            docAttributo.Cod_attributo = codAttributo
            docAttributo.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim listaDocAttributo As List(Of DllDocumentale.Documento_attributo) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_Documento_Attributi(docAttributo)

            If (listaDocAttributo.Count > 0) Then
                attributo.ID = listaDocAttributo.ElementAt(0).Cod_attributo
                attributo.Valore = listaDocAttributo.ElementAt(0).Valore
                attributo.Descrizione = ""
                attributo.TipoDato = "String"
            Else
                attributo = Nothing
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return attributo
    End Function


    Public Function GetListaFattByLiquidazione(ByVal idLiquidazione As Integer, Optional ByVal idDoc As String = "") As IList(Of Ext_FatturaInfo)
        Dim fattureExt As List(Of Ext_FatturaInfo) = New List(Of Ext_FatturaInfo)
        Try
            Log.Info("Inizio metodo GetListaFatture " + TimeValue(Now))
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)
            Dim idDocumento As String = ""
            If idDoc <> "" Then
                idDocumento = idDoc
            Else
                idDocumento = CodDocumento()
            End If


            'Dim idLiquidazione As String = HttpContext.Current.Request.Item("idLiquidazione")


            Dim fattureInfoHeader As List(Of DllDocumentale.ItemFatturaInfoHeader) = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaFattureLiquidazione(idLiquidazione, idDocumento)
            'Dim fattureInfo As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = GetContrattiInfo(operatore, contrattiInfoHeader)

            For i As Integer = 0 To fattureInfoHeader.Count - 1
                Dim fattura As DllDocumentale.ItemFatturaInfoHeader = fattureInfoHeader.ElementAt(i)

                Dim ext_fattura As Ext_FatturaInfo = New Ext_FatturaInfo()
                Dim ext_anagraficaInfo As Ext_AnagraficaInfo = New Ext_AnagraficaInfo()
                Dim ext_lista_sedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
                Dim ext_sede As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                Dim ext_dato_bancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                Dim ext_dati_bancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)

                Dim ext_contratto_fattura As Ext_ContrattoInfo = New Ext_ContrattoInfo()

                ext_contratto_fattura.Id = fattura.Contratto.IdContratto
                ext_contratto_fattura.NumeroRepertorio = fattura.Contratto.NumeroRepertorioContratto
                ext_contratto_fattura.Oggetto = fattura.Contratto.OggettoContratto
                ext_contratto_fattura.CodiceCIG = fattura.Contratto.CodieCIG
                ext_contratto_fattura.CodiceCUP = fattura.Contratto.CodieCUP

                ext_sede.IdModalitaPagamento = fattura.AnagraficaInfo.IdModalitaPag
                ext_sede.ModalitaPagamento = fattura.AnagraficaInfo.DescrizioneModalitaPag
                ext_sede.NomeSede = fattura.AnagraficaInfo.SedeVia
                ext_sede.IdSede = fattura.AnagraficaInfo.IdSede

                ext_dato_bancario.Iban = fattura.AnagraficaInfo.Iban
                ext_dato_bancario.IdContoCorrente = fattura.AnagraficaInfo.IdConto

                ext_dati_bancari.Add(ext_dato_bancario)

                ext_sede.DatiBancari = ext_dati_bancari
                ext_lista_sedi.Add(ext_sede)

                ext_anagraficaInfo.ListaSedi = ext_lista_sedi

                ext_anagraficaInfo.ID = fattura.AnagraficaInfo.IdAnagrafica
                ext_anagraficaInfo.PartitaIva = fattura.AnagraficaInfo.PartitaIva
                ext_anagraficaInfo.CodiceFiscale = fattura.AnagraficaInfo.CodiceFiscale
                ext_anagraficaInfo.Denominazione = fattura.AnagraficaInfo.Denominazione

                ext_fattura.DescrizioneFattura = fattura.DescrizioneFattura
                ext_fattura.ImportoTotaleFattura = fattura.ImportoTotaleFattura
                ext_fattura.ImportoLiquidato = fattura.ImportoLiquidato
                ext_fattura.IdUnivoco = fattura.IdUnivoco
                ext_fattura.IdDocumento = fattura.IdDocumento
                ext_fattura.NumeroFatturaBeneficiario = fattura.NumeroFatturaBeneficiario
                ext_fattura.DataFatturaBeneficiario = fattura.DataFatturaBeneficiario

                ext_fattura.AnagraficaInfo = ext_anagraficaInfo
                ext_fattura.Contratto = ext_contratto_fattura

                ext_fattura.IdLiquidazione = idLiquidazione
                fattureExt.Add(ext_fattura)

            Next

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        Return fattureExt
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GestisciBeneficiarioCronologia")>
    Public Sub GestisciBeneficiarioCronologia(ByVal beneficiarioCronologia As String)
        Dim result As Boolean
        Dim benefCronologia As DllDocumentale.BeneficiarioCronologia = Nothing
        Try
            Log.Info("Inizio metodo GetListaBeneficiariCronologia " + TimeValue(Now))

            Dim beneficiarioCronologiaExt As Ext_BeneficiarioCronologiaInfo = DirectCast(JavaScriptConvert.DeserializeObject(beneficiarioCronologia, GetType(Ext_BeneficiarioCronologiaInfo)), Ext_BeneficiarioCronologiaInfo)
            Dim beneficiarioRicevuto As New DllDocumentale.BeneficiarioCronologia

            beneficiarioRicevuto.CodFiscPIva = beneficiarioCronologiaExt.CodFiscPIva
            beneficiarioRicevuto.IdBeneficiario = beneficiarioCronologiaExt.IdBeneficiario
            beneficiarioRicevuto.IdSede = beneficiarioCronologiaExt.IdSede
            beneficiarioRicevuto.IdTipoPagamento = beneficiarioCronologiaExt.IdTipoPagamento
            beneficiarioRicevuto.IdContoCorrente = beneficiarioCronologiaExt.IdContoCorrente
            beneficiarioRicevuto.FlagPersonaFisica = beneficiarioCronologiaExt.FlagPersonaFisica
            beneficiarioRicevuto.Nominativo = beneficiarioCronologiaExt.Nominativo
            If (Not String.IsNullOrEmpty(beneficiarioCronologiaExt.DataNasc)) Then
                beneficiarioRicevuto.DataNasc = beneficiarioCronologiaExt.DataNasc
            Else
                beneficiarioRicevuto.DataNasc = Nothing
            End If
            beneficiarioRicevuto.LuogoNasc = beneficiarioCronologiaExt.LuogoNasc
            beneficiarioRicevuto.LegaleRappresentante = beneficiarioCronologiaExt.LegaleRappresentante
            beneficiarioRicevuto.DescrSede = beneficiarioCronologiaExt.DescrSede
            beneficiarioRicevuto.DescrModPagamento = beneficiarioCronologiaExt.DescrModPagamento
            beneficiarioRicevuto.DescrDatiBancari = beneficiarioCronologiaExt.DescrDatiBancari

            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Dim listaBeneficiariCronologia As List(Of DllDocumentale.BeneficiarioCronologia) = Nothing

            Log.Debug(operatore.Codice)

            'Necessario sia per l'inserimento che per l'update!
            beneficiarioRicevuto.CodOperatore = operatore.Codice
            beneficiarioRicevuto.DataUltimoUtilizzo = Now

            benefCronologia = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_BeneficiarioCronologiaByPK(operatore.Codice, beneficiarioRicevuto.IdBeneficiario, beneficiarioRicevuto.IdSede, beneficiarioRicevuto.IdTipoPagamento, beneficiarioRicevuto.IdContoCorrente)

            If (benefCronologia Is Nothing) Then
                ' se non trovo il beneficiario che l'utente ha appena scelto, devo inserirlo, 
                ' ma prima devo eliminarne uno dalla cronologia che viene usato da meno tempo
                ' andando a controllare che la lista in cronologia abbia almeno già 10 benef, altrimenti lo iserisco direttamente
                listaBeneficiariCronologia = (New DllDocumentale.svrDocumenti(operatore)).FO_Get_ListaBeneficiariCronologiaByCodOpe(operatore.Codice)
                If listaBeneficiariCronologia.Count < 10 Then
                    beneficiarioRicevuto.ContatoreFrequenza = 1
                    result = IIf((New DllDocumentale.svrDocumenti(operatore)).FO_Insert_Beneficiario_Cronologia(beneficiarioRicevuto) = 0, True, False)
                Else
                    beneficiarioRicevuto.ContatoreFrequenza = 1
                    If ((New DllDocumentale.svrDocumenti(operatore)).FO_Delete_BeneficiarioCronologiaMenoUsato(operatore.Codice) = 0) Then
                        result = IIf((New DllDocumentale.svrDocumenti(operatore)).FO_Insert_Beneficiario_Cronologia(beneficiarioRicevuto) = 0, True, False)
                    Else
                        result = False
                    End If
                End If
            Else
                result = IIf((New DllDocumentale.svrDocumenti(operatore)).FO_Update_BeneficiarioCronologia(beneficiarioRicevuto) = 0, True, False)
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        Finally
            HttpContext.Current.Response.Write("{ success: " + IIf(result, "true", "false") + "}")
        End Try
    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaCodiciSiope?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetListaCodiciSiope(ByVal CapitoloRif As String, ByVal AnnoRif As String) As IList(Of Ext_TipoBase)
        Dim listaCodiciSiope As New System.Collections.Generic.List(Of Ext_TipoBase)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneCS As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneCodiciSiopeMessage(operatore, AnnoRif, CapitoloRif)
            For i As Integer = 0 To UBound(rispostaInterrogazioneCS, 1)
                Dim codiceSiope As New Ext_TipoBase
                codiceSiope.Id = DirectCast(rispostaInterrogazioneCS(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaCodiceSiope_TypesSiope).Codice
                codiceSiope.Descrizione = DirectCast(rispostaInterrogazioneCS(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaCodiceSiope_TypesSiope).Descrizione
                listaCodiciSiope.Add(codiceSiope)
            Next
            Return listaCodiciSiope
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Log.Error(ex.ToString)
            Return New System.Collections.Generic.List(Of Ext_TipoBase)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
            Return listaCodiciSiope
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaTipologieDocumento")>
    Public Function GetListaTipologieDocumento() As IList(Of Ext_TipologiaDocumento)
        Dim retValue As New System.Collections.Generic.List(Of Ext_TipologiaDocumento)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim dllDoc As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            Dim listaTipologieDocumento As Generic.List(Of DllDocumentale.ItemTipologiaDocumento) = dllDoc.FO_GetListaTipologieDocumento()

            For Each tipologiaDocumento As DllDocumentale.ItemTipologiaDocumento In listaTipologieDocumento
                Dim tipologiaDocumento_ext As New Ext_TipologiaDocumento

                tipologiaDocumento_ext.Id = tipologiaDocumento.Id
                tipologiaDocumento_ext.Tipologia = tipologiaDocumento.Tipologia
                tipologiaDocumento_ext.HasDestinatari = tipologiaDocumento.HasDestinatari
                tipologiaDocumento_ext.HasDestinatariObbligatori = tipologiaDocumento.HasDestinatariObbligatori

                retValue.Add(tipologiaDocumento_ext)
            Next
            Return retValue
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Log.Error(ex.ToString)
            Return New System.Collections.Generic.List(Of Ext_TipoBase)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
            Return retValue
        End Try
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/Registra_BeneficiarioLiquidazioneImpegno")>
    Public Sub Registra_BeneficiarioLiquidazioneImpegno()

        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim oggettoSerializzato As String = HttpContext.Current.Request.Form.Item("anagrafica")
        Dim sedeStr As String = HttpContext.Current.Request.Form.Item("sede")
        Dim conto As String = HttpContext.Current.Request.Form.Item("conto")
        Dim ImpSpettante As String = HttpContext.Current.Request.Form.Item("ImpSpettante")
        Dim CodiceCig As String = HttpContext.Current.Request.Form.Item("CodiceCig")
        Dim CodiceCup As String = HttpContext.Current.Request.Form.Item("CodiceCup")
        Dim CodiceSiope As String = HttpContext.Current.Request.Form.Item("CodiceSiope")
        Dim hidLiquidazione As String = HttpContext.Current.Request.Form.Item("hidLiquidazione")
        Dim hidImpegno As String = HttpContext.Current.Request.Form.Item("hidImpegno")
        Dim ID As String = HttpContext.Current.Request.Form.Item("ID")

        Log.Debug(operatore.Codice)
        Dim anagraficaScelta As Ext_AnagraficaInfo = Nothing
        If Not String.IsNullOrEmpty(oggettoSerializzato) Then
            oggettoSerializzato = "[" & oggettoSerializzato & "]"
            Dim anagrafica_Selezionata As List(Of Ext_AnagraficaInfo) = DirectCast(JavaScriptConvert.DeserializeObject(oggettoSerializzato, GetType(List(Of Ext_AnagraficaInfo))), List(Of Ext_AnagraficaInfo))
            anagraficaScelta = anagrafica_Selezionata.Item(0)
        End If

        oggettoSerializzato = sedeStr + ""
        Dim sedeScelta As Ext_SedeAnagraficaInfo = Nothing
        If Not String.IsNullOrEmpty(oggettoSerializzato) Then
            oggettoSerializzato = "[" & oggettoSerializzato & "]"
            Dim sede_Scelta As List(Of Ext_SedeAnagraficaInfo) = DirectCast(JavaScriptConvert.DeserializeObject(oggettoSerializzato, GetType(List(Of Ext_SedeAnagraficaInfo))), List(Of Ext_SedeAnagraficaInfo))
            sedeScelta = sede_Scelta.Item(0)
        End If

        oggettoSerializzato = conto + ""
        Dim contoScelto As Ext_DatiBancariInfo = Nothing
        If Not String.IsNullOrEmpty(oggettoSerializzato) Then
            oggettoSerializzato = "[" & oggettoSerializzato & "]"
            Dim conto_Scelto As List(Of Ext_DatiBancariInfo) = DirectCast(JavaScriptConvert.DeserializeObject(oggettoSerializzato, GetType(List(Of Ext_DatiBancariInfo))), List(Of Ext_DatiBancariInfo))
            contoScelto = conto_Scelto.Item(0)
        End If

        Dim messaggio As String = Registra_BeneficiarioImpegnoLiquidazione(operatore, anagraficaScelta, sedeScelta, contoScelto, ImpSpettante, CodiceCig, CodiceCup, CodiceSiope, hidLiquidazione, hidImpegno, ID)
        HttpContext.Current.Response.Write(messaggio)

    End Sub
    Public Shared Function Registra_BeneficiarioImpegnoLiquidazione(ByVal Operatore As DllAmbiente.Operatore,
                                                            ByVal anagraficaScelta As Ext_AnagraficaInfo,
                                                            ByVal sedeScelta As Ext_SedeAnagraficaInfo,
                                                            ByVal contoScelto As Ext_DatiBancariInfo,
                                                            ByVal ImpSpettante As String,
                                                            ByVal CodiceCig As String,
                                                            ByVal CodiceCup As String,
                                                            ByVal CodiceSiope As String,
                                                            ByVal hidLiquidazione As String,
                                                            ByVal hidImpegno As String,
                                                            ByVal idInternoDocContabileBeneficiario As String) As String
        Dim messaggio As String = ""
        Try
            Dim ddlDocumentale As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(Operatore)
            Dim oggettoDaInserire As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo = New DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo


            If Not anagraficaScelta Is Nothing AndAlso anagraficaScelta.ID <> "" AndAlso anagraficaScelta.ID <> "0" Then
                oggettoDaInserire.IdAnagrafica = anagraficaScelta.ID
                oggettoDaInserire.EsenzCommBonifico = anagraficaScelta.Commissioni
                oggettoDaInserire.Denominazione = anagraficaScelta.Denominazione
                If (anagraficaScelta.Tipologia = "F") Then
                    oggettoDaInserire.FlagPersonaFisica = True
                    oggettoDaInserire.CodiceFiscale = anagraficaScelta.CodiceFiscale
                    oggettoDaInserire.Denominazione = anagraficaScelta.Cognome & " " & anagraficaScelta.Nome
                Else
                    oggettoDaInserire.PartitaIva = anagraficaScelta.PartitaIva
                    oggettoDaInserire.FlagPersonaFisica = False
                End If

                If Not anagraficaScelta.Contratto Is Nothing Then
                    oggettoDaInserire.IdContratto = anagraficaScelta.Contratto.Id
                    oggettoDaInserire.NumeroRepertorioContratto = anagraficaScelta.Contratto.NumeroRepertorio
                    oggettoDaInserire.IsDatoSensibile = anagraficaScelta.IsDatoSensibile
                    If Not anagraficaScelta.Fattura Is Nothing Then
                        oggettoDaInserire.ProgFatturaLiq = anagraficaScelta.Fattura.IdProgFatturaInLiquidazione
                    End If
                End If
            Else
                Throw New Exception("Il beneficiario non risulta essere selezionato")
            End If

            If Not sedeScelta Is Nothing Then
                oggettoDaInserire.IdSede = sedeScelta.IdSede

                If Not String.IsNullOrEmpty(sedeScelta.IdModalitaPagamento) Then
                    oggettoDaInserire.IdModalitaPag = sedeScelta.IdModalitaPagamento
                End If

                oggettoDaInserire.SedeComune = sedeScelta.Comune
                oggettoDaInserire.SedeVia = sedeScelta.Indirizzo
                oggettoDaInserire.SedeProvincia = sedeScelta.CapComune
            End If

            If Not contoScelto Is Nothing Then
                If Not String.IsNullOrEmpty(contoScelto.IdContoCorrente) AndAlso contoScelto.IdContoCorrente <> "0"  Then
                    oggettoDaInserire.IdConto = contoScelto.IdContoCorrente
                Else
                     oggettoDaInserire.IdConto = ""
                End If

                If Not String.IsNullOrEmpty(contoScelto.Iban) Then
                    oggettoDaInserire.Iban = contoScelto.Iban
                Else
                    oggettoDaInserire.Iban = contoScelto.ContoCorrente
                End If
            End If


            oggettoDaInserire.ImportoSpettante = ImpSpettante + ""
            oggettoDaInserire.Cig = CodiceCig + ""
            oggettoDaInserire.Cup = CodiceCup + ""
            oggettoDaInserire.CodiceSiope = CodiceSiope + ""

            If Not String.IsNullOrEmpty(hidLiquidazione + "") Then
                oggettoDaInserire.IDDocumentoContabile = hidLiquidazione + ""
                oggettoDaInserire.IsImpegno = False
            ElseIf Not String.IsNullOrEmpty(hidImpegno + "") Then
                oggettoDaInserire.IDDocumentoContabile = hidImpegno + ""
                oggettoDaInserire.IsImpegno = True
            End If

            Dim currentLiquidazioneBeneficiarioId As Long = 0
            If Not String.IsNullOrEmpty(idInternoDocContabileBeneficiario) Then
                oggettoDaInserire.ID = idInternoDocContabileBeneficiario
                currentLiquidazioneBeneficiarioId = idInternoDocContabileBeneficiario
            End If

            oggettoDaInserire.IdDocumento = CodDocumento()
            oggettoDaInserire.Operatore = Operatore.Codice

            If currentLiquidazioneBeneficiarioId <> 0 Then
                If Not oggettoDaInserire.IsImpegno Then
                    ddlDocumentale.FO_Registra_LiquidazioneBeneficiario(oggettoDaInserire, Operatore)
                    messaggio = "{  success: true}"
                    'HttpContext.Current.Response.Write("{  success: true}")
                Else
                    Dim messaggioErrore As String = ddlDocumentale.FO_Registra_ImpegnoBeneficiario(oggettoDaInserire, Operatore)
                    If messaggioErrore = "" Then
                        messaggio = "{  success: true}"
                    Else
                        messaggio = "{  success: false, errorCode: 0, FaultMessage: '" & messaggio & ".'}}"
                    End If

                    'HttpContext.Current.Response.Write("{  success: true}")
                End If
            Else
                If Not oggettoDaInserire.IsImpegno Then
                    Dim liquidazioneBeneficiarioId As String = ddlDocumentale.FO_IsBeneficiarioInLiquidazione(Operatore, oggettoDaInserire.IdDocumento, oggettoDaInserire.IDDocumentoContabile, oggettoDaInserire.IdAnagrafica, oggettoDaInserire.IdSede)
                    If liquidazioneBeneficiarioId Is Nothing Then
                        ddlDocumentale.FO_Registra_LiquidazioneBeneficiario(oggettoDaInserire, Operatore)
                        messaggio = "{  success: true}"
                        'HttpContext.Current.Response.Write("{  success: true}")
                    Else
                        Dim beneficiario As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = ddlDocumentale.FO_Get_ListaBeneficiariLiquidazione(Operatore, , , , liquidazioneBeneficiarioId)
                        If Not beneficiario Is Nothing AndAlso beneficiario.Count > 0 Then
                            Dim beneficiarioAsJsonObject As String = JavaScriptConvert.SerializeObject(beneficiario.Item(0))

                            Dim sede As String = Nothing
                            If Not sedeScelta Is Nothing Then
                                sede = sedeScelta.NomeSede & " (" & sedeScelta.Indirizzo & " - " & sedeScelta.CapComune & " " & sedeScelta.Comune & ") "
                            End If
                            messaggio = "{  success: false, errorCode: 5, additionalInfo: { beneficiario: " & beneficiarioAsJsonObject & "} " &
                                                               ", FaultMessage: '" & "Per il beneficiario \'" & oggettoDaInserire.Denominazione & "\'" &
                                                               IIf(Not sede Is Nothing, ", sede \'" & sede.Replace("'", "\'") & "\',", String.Empty) &
                                                               " esiste già un importo da liquidare del valore di € " & beneficiario.Item(0).ImportoSpettante.ToString() & ".'}"
                            'HttpContext.Current.Response.Write("{  success: false, errorCode: 5, additionalInfo: { beneficiario: " & beneficiarioAsJsonObject & "} " &
                            '                                   ", FaultMessage: '" & "Per il beneficiario \'" & oggettoDaInserire.Denominazione & "\'" &
                            '                                   IIf(Not sede Is Nothing, ", sede \'" & sede.Replace("'", "\'") & "\',", String.Empty) &
                            '                                   " esiste già un importo da liquidare del valore di € " & beneficiario.Item(0).ImportoSpettante.ToString() & ".'}")
                        Else
                            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(New Exception()), "Impossibile ottenere le informazioni relative al beneficiario da includere nella liquidazione.")
                        End If
                    End If
                Else
                    Dim listaBeneficiariImpegni As List(Of ItemLiquidazioneImpegnoBeneficiarioInfo) = ddlDocumentale.FO_Get_ListaBeneficiariImpegno(Operatore, oggettoDaInserire.IdDocumento,, oggettoDaInserire.IDDocumentoContabile)

                    If listaBeneficiariImpegni.Count > 0 Then
                        For Each beneficiarioImpegno As ItemLiquidazioneImpegnoBeneficiarioInfo In listaBeneficiariImpegni
                            Dim beneficiarioAsJsonObject As String = JavaScriptConvert.SerializeObject(beneficiarioImpegno)
                            messaggio = "{  success: false, errorCode: 5, additionalInfo: { beneficiario: " & beneficiarioAsJsonObject & "} " &
                                                                   ", FaultMessage: '" & "Per il presente impegno è già presente un beneficiario.'}"
                            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(New Exception()), "Per il presente impegno è già presente un beneficiario.")
                        Next
                    Else
                        Dim messaggioErrore As String = ddlDocumentale.FO_Registra_ImpegnoBeneficiario(oggettoDaInserire, Operatore)
                        If messaggioErrore = "" Then
                            messaggio = "{  success: true}"
                        Else
                            messaggio = "{  success: false, errorCode: 0, FaultMessage: '" & messaggioErrore & ".'}"
                        End If
                        'HttpContext.Current.Response.Write("{  success: true}")

                    End If
                    'End If
                End If
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            messaggio = "{  success: false, errorCode: 1, additionalInfo: {}, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }"
            'Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
        'HttpContext.Current.Response.Write(messaggio)
        Return messaggio
    End Function
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.WrappedRequest, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/Registra_BeneficiarioLiquidazioneImpegnoUR")>
    Public Sub Registra_BeneficiarioLiquidazioneImpegnoUR()
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim ddlDocumentale As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            Dim oggettoSerializzato As String = HttpContext.Current.Request.Form.Item("BeneficiarioLiquidazione")
            oggettoSerializzato = "[" & oggettoSerializzato & "]"

            Dim beneficiariUR As List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) = DirectCast(JavaScriptConvert.DeserializeObject(oggettoSerializzato, GetType(List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo))), List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo))

            For Each beneficiarioUR As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In beneficiariUR
                If beneficiarioUR.IsImpegno Then
                    ddlDocumentale.FO_Registra_ImpegnoBeneficiario(beneficiarioUR, operatore)
                Else
                    ddlDocumentale.FO_Registra_LiquidazioneBeneficiario(beneficiarioUR, operatore)
                End If

            Next
            HttpContext.Current.Response.Write("{  success: true}")
        Catch ex As Exception
            Log.Error(ex.ToString)
            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
        End Try

    End Sub
    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetListaTipologiaReddito")>
    Public Function GetListaTipologiaReddito() As IList(Of Ext_TipoBase)
        Dim listatipoReddito As New System.Collections.Generic.List(Of Ext_TipoBase)
        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

            Log.Debug(operatore.Codice)

            Dim rispostaInterrogazioneTR As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazioneListaTipologiaReddito(operatore)
            For i As Integer = 0 To UBound(rispostaInterrogazioneTR, 1)
                Dim tipoReddito As New Ext_TipoBase
                tipoReddito.Id = DirectCast(rispostaInterrogazioneTR(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaTipologiaReddito_TypesTipologia).Codice
                tipoReddito.Descrizione = DirectCast(rispostaInterrogazioneTR(i), ClientIntegrazioneSic.Intema.WS.Risposta.Risposta_InterrogaTipologiaReddito_TypesTipologia).Descrizione
                listatipoReddito.Add(tipoReddito)
            Next
            Return listatipoReddito
        Catch ex As ClientIntegrazioneSic.ListaVuotaException
            Log.Error(ex.ToString)
            Return New System.Collections.Generic.List(Of Ext_TipoBase)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
            Return listatipoReddito
        End Try
    End Function



    Public Shared Function TrasformaBeneficiariInterniInSIC(ByVal listaBenReturn As Generic.List(Of DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo)) As Array
        Dim arrayBen As Array = Array.CreateInstance(GetType(ClientIntegrazioneSic.Intema.WS.Richiesta.Beneficiario_Types), listaBenReturn.Count)
        Dim benefSic As ClientIntegrazioneSic.Intema.WS.Richiesta.Beneficiario_Types
        Dim i As Integer = 0
        For Each beneficiario As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo In listaBenReturn
            benefSic = New ClientIntegrazioneSic.Intema.WS.Richiesta.Beneficiario_Types
            If beneficiario.AddizionaleComunale > 0 Then
                benefSic.AddizionaleComunale = beneficiario.AddizionaleComunale
                benefSic.AddizionaleComunaleSpecified = True
            End If
            If beneficiario.AddizionaleRegionale > 0 Then
                benefSic.AddizionaleRegionale = beneficiario.AddizionaleRegionale
                benefSic.AddizionaleRegionaleSpecified = True
            End If
            If beneficiario.AltreRitenute > 0 Then
                benefSic.AltreRitenute = beneficiario.AltreRitenute
                benefSic.AltreRitenuteSpecified = True
            End If

            If beneficiario.Bollo Then
                benefSic.Bollo = "S"
                'benefSic.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.S
            Else
                benefSic.Bollo = "N"
                'benefSic.Bollo = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_TypesBollo.N
            End If

            benefSic.CodiceBeneficiario = beneficiario.IdAnagrafica
            benefSic.CodiceCIG = beneficiario.Cig
            benefSic.CodiceContoCorrente = beneficiario.IdConto
            benefSic.CodiceCUP = beneficiario.Cup
            benefSic.CodiceSede = beneficiario.IdSede
            benefSic.CodiceSIOPE = beneficiario.CodiceSiope
            benefSic.CodiceSIOPEAggiuntivo = beneficiario.CodiceSiopeAggiuntivo
            benefSic.CodiceTipoPagamento = beneficiario.IdModalitaPag
            If beneficiario.EsenzCommBonifico Then
                benefSic.EsenzCommBonifico = "S"
            Else
                benefSic.EsenzCommBonifico = "N"
            End If
            If beneficiario.ImponibileIrpef > 0 Then
                benefSic.ImponibileIrpef = beneficiario.ImponibileIrpef
                benefSic.ImponibileIrpefSpecified = True
            End If
            If beneficiario.ImponibilePrevidenziale > 0 Then
                benefSic.ImponibilePrevidenziale = beneficiario.ImponibilePrevidenziale
                benefSic.ImponibilePrevidenzialeSpecified = True
            End If
            If beneficiario.ImportoSpettante > 0 Then
                benefSic.ImportoLordo = beneficiario.ImportoSpettante
                benefSic.ImportoLordoSpecified = True
            End If
            If beneficiario.RitenuteIrpef > 0 Then
                benefSic.RitenuteIrpef = beneficiario.RitenuteIrpef
                benefSic.RitenuteIrpefSpecified = True
            End If
            If beneficiario.RitenutePrevidenzialiBen > 0 Then
                benefSic.RitenutePrevidenzialiBen = beneficiario.RitenutePrevidenzialiBen
                benefSic.RitenutePrevidenzialiBenSpecified = True
            End If
            If beneficiario.RitenutePrevidenzialiEnte > 0 Then
                benefSic.RitenutePrevidenzialiEnte = beneficiario.RitenutePrevidenzialiEnte
                benefSic.RitenutePrevidenzialiEnteSpecified = True
            End If
            If beneficiario.StampaAvviso Then
                benefSic.StampaAvviso = "S"
            Else
                benefSic.StampaAvviso = "N"
            End If

            benefSic.DatoSensibileSpecified = True
            benefSic.DatoSensibile = beneficiario.IsDatoSensibile

            arrayBen.SetValue(benefSic, i)
            i = i + 1
        Next
        Return arrayBen
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, Method:="*", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetPianoDeiContiFinanziari?AnnoRif={AnnoRif}&CapitoloRif={CapitoloRif}")>
    Public Function GetPianoDeiContiFinanziari(ByVal CapitoloRif As String, ByVal AnnoRif As String) As List(Of Ext_PCFInfo)
        Dim result As New List(Of Ext_PCFInfo)

        Try
            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
            Log.Debug(operatore.Codice)

            Dim pcfArray As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazionePianoDeiContiFinanziario(operatore, AnnoRif, CapitoloRif)
            'Dim pcfArray As Array = ClientIntegrazioneSic.MessageMaker.createInterrogazionePianoDeiContiFinanziario(operatore, "2014", CapitoloRif)

            For Each pcf As PCF_Type In pcfArray
                Dim pcfInfo As New Ext_PCFInfo
                pcfInfo.Id = pcf.Codice
                pcfInfo.Descrizione = pcf.Descrizione
                result.Add(pcfInfo)
            Next
            Return result
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

    '    <OperationContract()> _
    '<WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, requestFormat:=WebMessageFormat.Json, responseFormat:=WebMessageFormat.Json, UriTemplate:="/NotificaAttoFattura")> _
    'Public Function NotificaAttoFattura(ByVal fattura As Ext_FatturaInfo, ByVal statoOperazione As String, Optional ByVal numPreImp As String = "", Optional ByVal idLiquidazione As Integer = 0) As String
    '        Dim esito As String = ""
    '        Try
    '            Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

    '            Dim liquidazione As DllDocumentale.ItemLiquidazioneInfo = New DllDocumentale.ItemLiquidazioneInfo
    '            Dim preImpegno As DllDocumentale.ItemImpegnoInfo = New DllDocumentale.ItemImpegnoInfo

    '            Log.Debug(operatore.Codice)

    '            Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)

    '            Dim documento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(CodDocumento)

    '            If idLiquidazione > 0 Then
    '                Dim listaLiquidazioni As Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(CodDocumento, idLiquidazione)

    '                If listaLiquidazioni.Count > 0 Then
    '                    For j As Integer = 0 To listaLiquidazioni.Count - 1
    '                        liquidazione = listaLiquidazioni(j)
    '                    Next
    '                    If Not liquidazione Is Nothing And statoOperazione = "I" Then

    '                        dllDoc.FO_Insert_Liquidazione_Fattura(documento.Doc_id, liquidazione.Dli_prog, fattura.IdUnivoco)
    '                    End If

    '                    If Not liquidazione Is Nothing And statoOperazione = "C" Then
    '                        dllDoc.FO_Delete_Liquidazione_Fattura(documento.Doc_id, liquidazione.Dli_prog, fattura.IdUnivoco)
    '                    End If
    '                End If
    '            End If





    '            If (Not numPreImp Is Nothing) Then
    '                Dim listaPreImpegni As Generic.List(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_Get_DatiImpegniByNPreImp(numPreImp, documento.Doc_id)

    '                If listaPreImpegni.Count > 0 Then
    '                    For i As Integer = 0 To listaPreImpegni.Count - 1
    '                        preImpegno = listaPreImpegni(i)
    '                    Next
    '                    If Not preImpegno Is Nothing And statoOperazione = "I" Then

    '                        dllDoc.FO_Insert_Impegno_Fattura(documento.Doc_id, preImpegno.Dli_prog, fattura.IdUnivoco)
    '                    End If

    '                    If Not preImpegno Is Nothing And statoOperazione = "C" Then

    '                        dllDoc.FO_Delete_Impegno_Fattura(documento.Doc_id, preImpegno.Dli_prog, fattura.IdUnivoco)
    '                    End If
    '                End If
    '            End If



    '            'Servizio SIC - Notifica Fattura-Liquidazione
    '            Dim notificaAttoFatturaService As New NOTIFICAATTOFATTURAService()
    '            notificaAttoFatturaService.AllowAutoRedirect = True

    '            Dim usernameSicWSFatturazione As String = ConfigurationManager.AppSettings("UsernameSicWSFatturazione")
    '            Dim passwordSicWSFatturazione As String = ConfigurationManager.AppSettings("PasswordSicWSFatturazione")

    '            Dim cr As New System.Net.NetworkCredential(usernameSicWSFatturazione, passwordSicWSFatturazione)
    '            notificaAttoFatturaService.Credentials = cr

    '            Dim numeroDefinitivoAtto As String = ""
    '            Dim numeroProvvisorioAtto As String = ""

    '            If Not documento.Doc_numero Is Nothing Then
    '                numeroDefinitivoAtto = documento.Doc_numero
    '            End If
    '            If Not documento.Doc_numeroProvvisorio Is Nothing Then
    '                numeroProvvisorioAtto = documento.Doc_numeroProvvisorio
    '            End If

    '            Dim notificaProvvedimentiSIC As New ClientIntegrazioneSic.Intema.WS.Richiesta.NOTIFICAPROVVEDIMENTISICInput

    '            notificaProvvedimentiSIC.P_TIPODOCSICVARCHAR2IN = ""
    '            If documento.Doc_IsContabile = 1 And idLiquidazione > 0 Then
    '                notificaProvvedimentiSIC.P_TIPODOCSICVARCHAR2IN = Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ
    '            End If
    '            If (Not numPreImp Is Nothing) Then
    '                If documento.Doc_IsContabile = 1 And (Not preImpegno Is Nothing) And Not numPreImp.Equals("") Then
    '                    notificaProvvedimentiSIC.P_TIPODOCSICVARCHAR2IN = Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.PREIMP
    '                End If

    '            End If
    '            'If obj.Doc_IsContabile = 1 And idLiquidazione > 0 Then
    '            '    Dim tipoDocumento As ClientIntegrazioneSic.Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento
    '            '    tipoDocumento = 3
    '            '    notificaProvvedimentiSIC.P_TIPODOCSICVARCHAR2IN = tipoDocumento.ToString
    '            'End If

    '            If statoOperazione = "I" Then
    '                'Inserimento Fattura-(Liquidazione/Impegno) Sic 
    '                notificaProvvedimentiSIC.P_STATONOTIFICAVARCHAR2IN = "I"
    '            Else
    '                'Cancellazione Fattura-(Liquidazione/Impegno) Sic 
    '                notificaProvvedimentiSIC.P_STATONOTIFICAVARCHAR2IN = "C"
    '            End If

    '            If idLiquidazione > 0 And (liquidazione.Dli_NLiquidazione = 0 Or liquidazione.Dli_NLiquidazione = Nothing) Then
    '                notificaProvvedimentiSIC.P_NUMERODOCSICNUMBERIN = 0
    '            End If

    '            If (Not numPreImp Is Nothing) Then
    '                If ((Not preImpegno Is Nothing) And preImpegno.Dli_NumImpegno = 0) Then
    '                    notificaProvvedimentiSIC.P_NUMERODOCSICNUMBERIN = 0
    '                End If
    '            End If

    '            Dim idUnivocoFattura As Double
    '            If Integer.TryParse(fattura.IdUnivoco, idUnivocoFattura) Then
    '                notificaProvvedimentiSIC.P_IDFATTURANUMBERIN = idUnivocoFattura
    '            End If

    '            If Not numeroProvvisorioAtto Is Nothing Then
    '                notificaProvvedimentiSIC.P_IDATTOVARCHAR2IN = numeroProvvisorioAtto
    '            End If
    '            If Not numeroDefinitivoAtto Is Nothing Then
    '                notificaProvvedimentiSIC.P_IDATTOVARCHAR2IN = numeroProvvisorioAtto
    '            End If

    '            'DATA DOCUMENTO CONTABILE NEL CASO DI INVIO NOTIFICA QUANDO IL DOCUMENTO SI TROVA IN RAGIONERIA : Campo Dli_Data_Assunzione del documento
    '            notificaProvvedimentiSIC.P_DATADOCSICVARCHAR2IN = ""

    '            Dim P_ESITO As Double
    '            Dim messaggioVarchar2out As New ClientIntegrazioneSic.NOTIFICAPROVVEDIMENTISICInputP_MESSAGGIOVARCHAR2OUT
    '            Dim messaggioEsitoNumberOut As New ClientIntegrazioneSic.NOTIFICAPROVVEDIMENTISICInputP_ESITONUMBEROUT

    '            Dim messaggioSic As String = notificaAttoFatturaService.NOTIFICAPROVVEDIMENTISIC(notificaProvvedimentiSIC.P_TIPODOCSICVARCHAR2IN, notificaProvvedimentiSIC.P_STATONOTIFICAVARCHAR2IN, notificaProvvedimentiSIC.P_NUMERODOCSICNUMBERIN, messaggioVarchar2out, notificaProvvedimentiSIC.P_IDFATTURANUMBERIN, notificaProvvedimentiSIC.P_IDATTOVARCHAR2IN, messaggioEsitoNumberOut, notificaProvvedimentiSIC.P_DATADOCSICVARCHAR2IN, P_ESITO)
    '            'P_ESITO = 1.0
    '            'P_ESITO = 0.0

    '            If P_ESITO = 1.0 Then
    '                esito = messaggioSic.ToString
    '                'esito = "OK TEST"
    '            ElseIf P_ESITO = 0.0 Then

    '                If Not preImpegno Is Nothing And statoOperazione = "I" Then

    '                    dllDoc.FO_Delete_Impegno_Fattura(documento.Doc_id, preImpegno.Dli_prog, fattura.IdUnivoco)
    '                End If

    '                If Not liquidazione Is Nothing And statoOperazione = "I" Then
    '                    dllDoc.FO_Delete_Liquidazione_Fattura(documento.Doc_id, liquidazione.Dli_prog, fattura.IdUnivoco)
    '                End If
    '                'esito = "NO TEST - ROLLBACK"

    '                esito = messaggioSic.ToString
    '            End If

    '            Return esito
    '        Catch ex As Exception
    '            Log.Error(ex.ToString)
    '            HttpContext.Current.Response.Write("{  success: false, FaultMessage: '" & Replace(ex.Message, "'", "\'") & "' }")
    '        End Try
    '        Return ""
    '    End Function


    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/NotificaAttoFattura")>
    Public Function NotificaAttoFattura(ByVal listaFatture As System.Collections.Generic.List(Of Ext_FatturaInfo), ByVal statoOperazione As String, Optional ByVal numPreImp As String = "", Optional ByVal idLiquidazione As Integer = 0)
        'Dim progrFatturaInLiquidazione As Integer = 0
        'Dim numLiquidazione As Integer = 0
        'Dim numPreimpegno As Integer = 0
        'Dim numImpegno As Integer = 0
        'Dim dataMovimentoContabile As Date = Nothing

        'Dim esito As String = ""
        'Dim liquidazione As DllDocumentale.ItemLiquidazioneInfo = New DllDocumentale.ItemLiquidazioneInfo
        'Dim preImpegno As DllDocumentale.ItemImpegnoInfo = New DllDocumentale.ItemImpegnoInfo
        'Dim documento As DllDocumentale.Model.DocumentoInfo = New DllDocumentale.Model.DocumentoInfo
        'Dim operatore As DllAmbiente.Operatore = New DllAmbiente.Operatore

        'Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        ''Dim listaIdFatture As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
        'Dim messaggioSic As String = ""
        'Dim listaFattureItem As New System.Collections.Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)
        'Try
        '    operatore = HttpContext.Current.Session.Item("oOperatore")

        '    ' prima di registrare una fattura sulla liquidazione controllo che non sia già stata indicata sulla stessa liquidazione
        '    Dim listaFatturePresentiSuLiq As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = dllDoc.FO_Get_ListaFattureLiquidazione(idLiquidazione)

        '    For Each fatturaExt As Ext_FatturaInfoHeader In listaFatture
        '        If statoOperazione = "I" Then

        '            For Each fatturaPresente As DllDocumentale.ItemFatturaInfoHeader In listaFatturePresentiSuLiq
        '                If fatturaPresente.IdUnivoco = fatturaExt.IdUnivoco Then
        '                    Throw New Exception("Non è possibile aggiungere una fattura due volte alla stessa liquidazione")
        '                End If
        '            Next
        '        End If
        '        Dim fatturaItem As New DllDocumentale.ItemFatturaInfoHeader
        '        fatturaItem.Prog = fatturaExt.Prog
        '        fatturaItem.IdUnivoco = fatturaExt.IdUnivoco
        '        fatturaItem.ImportoLiquidato = fatturaExt.ImportoLiquidato
        '        fatturaItem.ImportoFattDaLiquidare = fatturaExt.ImportoFattDaLiquidare
        '        listaFattureItem.Add(fatturaItem)
        '    Next

        '    Log.Debug(operatore.Codice)

        '    documento = Leggi_Documento_Object(CodDocumento)

        '    If idLiquidazione > 0 Then
        '        Dim listaLiquidazioni As Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(CodDocumento, idLiquidazione)
        '        If listaLiquidazioni.Count > 0 Then
        '            For j As Integer = 0 To listaLiquidazioni.Count - 1
        '                liquidazione = listaLiquidazioni(j)
        '            Next
        '            If Not liquidazione Is Nothing Then
        '                numLiquidazione = liquidazione.Dli_NLiquidazione
        '                numPreImp = 0
        '                numImpegno = 0
        '                dataMovimentoContabile = liquidazione.Dli_DataRegistrazione
        '                progrFatturaInLiquidazione = dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, statoOperazione, documento.Doc_id, listaFattureItem)
        '            End If
        '        End If
        '    End If

        '    For Each fattura As Ext_FatturaInfoHeader In listaFatture

        '        'leggere la data di registrazione del doc contabile

        '        Dim P_ESITO As Double = 0
        '        Dim tipoDocumentoContabile As String = ""

        '        numImpegno = 0
        '        numPreimpegno = 0
        '        Dim importoDaPasareAlSIC As Double = 0
        '        If statoOperazione = "C" Then
        '            importoDaPasareAlSIC = fattura.ImportoLiquidato
        '        Else
        '            importoDaPasareAlSIC = fattura.ImportoFattDaLiquidare
        '        End If



        '        tipoDocumentoContabile = Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString
        '        Try
        '            'messaggioSic = ClientIntegrazioneSic.MessageMaker.notificaAttoFatturaSIC(operatore, documento.Doc_numero,
        '            '                                                                     documento.Doc_numeroProvvisorio, tipoDocumentoContabile,
        '            '                                                                     numLiquidazione, numPreimpegno, numImpegno, fattura.IdUnivoco,
        '            '                                                                     statoOperazione, dataMovimentoContabile, importoDaPasareAlSIC, P_ESITO)
        '        Catch ex As Exception
        '            Throw New SicException()
        '        End Try


        '        P_ESITO = 1.0
        '        If P_ESITO = 1.0 Then
        '            esito = messaggioSic.ToString
        '        ElseIf P_ESITO = 0.0 Then
        '            If Not liquidazione Is Nothing And statoOperazione = "I" Then
        '                dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
        '            End If

        '            esito = messaggioSic.ToString
        '            Exit For
        '        End If
        '    Next

        'Catch exSic As SicException
        '    'se il sic va in eccezione durante la notifica di inserimento e ho già inserito sul mio db, devo cancellare.
        '    If Not liquidazione Is Nothing And statoOperazione = "I" Then
        '        dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
        '    End If
        '    'se il sic va in eccezione durante la notifica di cancellazione e ho già inserito sul mio db, devo inserire.
        '    If Not liquidazione Is Nothing And statoOperazione = "C" Then
        '        dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "I", documento.Doc_id, listaFattureItem)
        '    End If
        '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(exSic), exSic.Message)
        'Catch ex As Exception
        '    Log.Error(ex.ToString)
        '    'se il sic va in eccezione durante la notifica di inserimento e ho già inserito sul mio db, devo cancellare.
        '    If Not liquidazione Is Nothing And statoOperazione = "I" Then
        '        dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
        '    End If
        '    'se il sic va in eccezione durante la notifica di cancellazione e ho già inserito sul mio db, devo inserire.
        '    If Not liquidazione Is Nothing And statoOperazione = "C" Then
        '        dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "I", documento.Doc_id, listaFattureItem)
        '    End If

        '    Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)

        'End Try
        Dim listaNotificaFatturaOutput As Generic.List(Of NotificaFatturaOutput) = NotificaAttoFattura1(listaFatture, statoOperazione, numPreImp, idLiquidazione)


        HttpContext.Current.Response.Write("{success: true, NotificaAttoResult: '" & listaNotificaFatturaOutput.ElementAt(0).EsitoSIC & "'}")
    End Function

    Public Function NotificaAttoFattura1(ByVal listaFatture As System.Collections.Generic.List(Of Ext_FatturaInfo), ByVal statoOperazione As String, Optional ByVal numPreImp As String = "", Optional ByVal idLiquidazione As Integer = 0) As Generic.List(Of NotificaFatturaOutput)
        Dim progrFatturaInLiquidazione As Integer = 0
        Dim numLiquidazione As Integer = 0
        Dim numPreimpegno As Integer = 0
        Dim numImpegno As Integer = 0
        Dim dataMovimentoContabile As Date = Nothing

        Dim esito As String = ""
        Dim liquidazione As DllDocumentale.ItemLiquidazioneInfo = New DllDocumentale.ItemLiquidazioneInfo
        Dim preImpegno As DllDocumentale.ItemImpegnoInfo = New DllDocumentale.ItemImpegnoInfo
        Dim documento As DllDocumentale.Model.DocumentoInfo = New DllDocumentale.Model.DocumentoInfo
        Dim operatore As DllAmbiente.Operatore = New DllAmbiente.Operatore

        Dim dllDoc As New DllDocumentale.svrDocumenti(operatore)
        'Dim listaIdFatture As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
        Dim messaggioSic As String = ""
        Dim listaFattureItem As New System.Collections.Generic.List(Of DllDocumentale.ItemFatturaInfoHeader)

        Dim listaNotificaFatturaOutput As New Generic.List(Of NotificaFatturaOutput)
        Try
            operatore = HttpContext.Current.Session.Item("oOperatore")

            ' prima di registrare una fattura sulla liquidazione controllo che non sia già stata indicata sulla stessa liquidazione
            Dim listaFatturePresentiSuLiq As Generic.List(Of DllDocumentale.ItemFatturaInfoHeader) = dllDoc.FO_Get_ListaFattureLiquidazione(idLiquidazione)

            For Each fatturaExt As Ext_FatturaInfoHeader In listaFatture
                If statoOperazione = "I" Then

                    For Each fatturaPresente As DllDocumentale.ItemFatturaInfoHeader In listaFatturePresentiSuLiq
                        If fatturaPresente.IdUnivoco = fatturaExt.IdUnivoco Then
                            Throw New Exception("Non è possibile aggiungere una fattura due volte alla stessa liquidazione")
                        End If
                    Next
                End If
                Dim fatturaItem As New DllDocumentale.ItemFatturaInfoHeader
                fatturaItem.Prog = fatturaExt.Prog
                fatturaItem.IdUnivoco = fatturaExt.IdUnivoco
                fatturaItem.ImportoLiquidato = fatturaExt.ImportoLiquidato
                fatturaItem.ImportoFattDaLiquidare = fatturaExt.ImportoFattDaLiquidare
                listaFattureItem.Add(fatturaItem)
            Next

            Log.Debug(operatore.Codice)

            documento = Leggi_Documento_Object(CodDocumento)

            If idLiquidazione > 0 Then
                Dim listaLiquidazioni As Generic.List(Of DllDocumentale.ItemLiquidazioneInfo) = dllDoc.FO_Get_DatiLiquidazione(CodDocumento, idLiquidazione)
                If listaLiquidazioni.Count > 0 Then
                    For j As Integer = 0 To listaLiquidazioni.Count - 1
                        liquidazione = listaLiquidazioni(j)
                    Next
                    If Not liquidazione Is Nothing Then
                        numLiquidazione = liquidazione.Dli_NLiquidazione
                        numPreImp = 0
                        numImpegno = 0
                        dataMovimentoContabile = liquidazione.Dli_DataRegistrazione
                        progrFatturaInLiquidazione = dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, statoOperazione, documento.Doc_id, listaFattureItem)
                    End If
                End If
            End If

            For Each fattura As Ext_FatturaInfoHeader In listaFatture

                'leggere la data di registrazione del doc contabile

                Dim P_ESITO As Double = 0
                Dim tipoDocumentoContabile As String = ""

                numImpegno = 0
                numPreimpegno = 0
                Dim importoDaPasareAlSIC As Double = 0
                If statoOperazione = "C" Then
                    importoDaPasareAlSIC = fattura.ImportoLiquidato
                Else
                    importoDaPasareAlSIC = fattura.ImportoFattDaLiquidare
                End If



                tipoDocumentoContabile = Intema.WS.Richiesta.CreateDelDocumento_TypesTipoDocumento.LIQ.ToString
                Try
                    messaggioSic = ClientIntegrazioneSic.MessageMaker.notificaAttoFatturaSIC(operatore, documento.Doc_numero,
                                                                                         documento.Doc_numeroProvvisorio, tipoDocumentoContabile,
                                                                                         numLiquidazione, numPreimpegno, numImpegno, fattura.IdUnivoco,
                                                                                         statoOperazione, dataMovimentoContabile, importoDaPasareAlSIC, P_ESITO)
                Catch ex As Exception
                    Throw New SicException()
                End Try


                P_ESITO = 1.0
                If P_ESITO = 1.0 Then
                    esito = messaggioSic.ToString
                ElseIf P_ESITO = 0.0 Then
                    If Not liquidazione Is Nothing And statoOperazione = "I" Then
                        dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
                    End If

                    esito = messaggioSic.ToString
                    Exit For
                End If

                Dim notificaFatturaOutput As New NotificaFatturaOutput
                notificaFatturaOutput.EsitoSIC = esito
                notificaFatturaOutput.ProgrFatturaInLiquidazione = progrFatturaInLiquidazione

                listaNotificaFatturaOutput.Add(notificaFatturaOutput)

            Next

        Catch exSic As SicException
            'se il sic va in eccezione durante la notifica di inserimento e ho già inserito sul mio db, devo cancellare.
            If Not liquidazione Is Nothing And statoOperazione = "I" Then
                dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
            End If
            'se il sic va in eccezione durante la notifica di cancellazione e ho già inserito sul mio db, devo inserire.
            If Not liquidazione Is Nothing And statoOperazione = "C" Then
                dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "I", documento.Doc_id, listaFattureItem)
            End If
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(exSic), exSic.Message)
        Catch ex As Exception
            Log.Error(ex.ToString)
            'se il sic va in eccezione durante la notifica di inserimento e ho già inserito sul mio db, devo cancellare.
            If Not liquidazione Is Nothing And statoOperazione = "I" Then
                dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "C", documento.Doc_id, listaFattureItem)
            End If
            'se il sic va in eccezione durante la notifica di cancellazione e ho già inserito sul mio db, devo inserire.
            If Not liquidazione Is Nothing And statoOperazione = "C" Then
                dllDoc.FO_Update_Scheda_Liquidazione_Fatture(liquidazione, "I", documento.Doc_id, listaFattureItem)
            End If

            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)

        End Try
        Return listaNotificaFatturaOutput
    End Function

    <OperationContract()>
    <WebInvoke(BodyStyle:=WebMessageBodyStyle.Wrapped, RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, UriTemplate:="/GetElencoFattureByContratto")>
    Public Function GetElencoFattureByContratto() As System.Collections.Generic.List(Of Ext_FatturaInfo)
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim fatture As New System.Collections.Generic.List(Of Ext_FatturaInfo)
        Try
            Dim idContratto As String = HttpContext.Current.Request.Item("contratto")
            Dim numeroRepertorioContratto As String = HttpContext.Current.Request.Item("numeroRepertorio")
            Dim oggettoContratto As String = HttpContext.Current.Request.Item("oggetto")

            Dim rispostaElencoFatture As ELENCOFATTUREOutputP_RETURN = Nothing
            Try
                rispostaElencoFatture = ClientIntegrazioneSic.MessageMaker.getElencoFattureByContratto(idContratto)

            Catch ex As Exception
                Throw ex
            End Try
            If Not rispostaElencoFatture.elenco_fatture Is Nothing Then
                If Not rispostaElencoFatture.elenco_fatture.fattura Is Nothing Then
                    For Each fatturaSIC As ClientIntegrazioneSic.elenco_fattureFattura In rispostaElencoFatture.elenco_fatture.fattura
                        If Not fatturaSIC Is Nothing Then
                            Dim objFattura As Ext_FatturaInfo = New Ext_FatturaInfo


                            If fatturaSIC.id_univoco Is Nothing Then
                                objFattura.IdUnivoco = String.Empty
                            Else
                                objFattura.IdUnivoco = fatturaSIC.id_univoco
                            End If

                            Dim contratto As Ext_ContrattoInfo = New Ext_ContrattoInfo
                            contratto.Oggetto = oggettoContratto
                            contratto.NumeroRepertorio = numeroRepertorioContratto
                            contratto.Id = idContratto

                            objFattura.Contratto = contratto

                            If fatturaSIC.numero_fattura_beneficiario Is Nothing Then
                                objFattura.NumeroFatturaBeneficiario = String.Empty
                            Else
                                objFattura.NumeroFatturaBeneficiario = fatturaSIC.numero_fattura_beneficiario
                            End If


                            If fatturaSIC.data_fattura_beneficiario Is Nothing Then
                                objFattura.DataFatturaBeneficiario = String.Empty
                            Else
                                objFattura.DataFatturaBeneficiario = fatturaSIC.data_fattura_beneficiario
                            End If


                            If fatturaSIC.descrizione_fattura Is Nothing Then
                                objFattura.DescrizioneFattura = String.Empty
                            Else
                                objFattura.DescrizioneFattura = fatturaSIC.descrizione_fattura
                            End If

                            objFattura.ImportoTotaleFattura = fatturaSIC.importo_totale_fattura


                            If Not fatturaSIC.allegato Is Nothing AndAlso fatturaSIC.allegato.Length > 0 Then
                                Dim listaAllegatiFattura As Generic.List(Of Ext_FatturaAllegato) = New Generic.List(Of Ext_FatturaAllegato)
                                For Each allegatoFattSIC As ClientIntegrazioneSic.elenco_fattureFatturaAllegato In fatturaSIC.allegato
                                    Dim allegatoFattEXT As New Ext_FatturaAllegato
                                    allegatoFattEXT.Nome = allegatoFattSIC.nome_allegato
                                    allegatoFattEXT.Formato = allegatoFattSIC.formato_allegato
                                    allegatoFattEXT.Url = allegatoFattSIC.url_allegato


                                    listaAllegatiFattura.Add(allegatoFattEXT)
                                Next

                                If Not listaAllegatiFattura Is Nothing AndAlso listaAllegatiFattura.Count > 0 Then
                                    objFattura.ListaAllegati = listaAllegatiFattura
                                End If
                            End If

                            Dim listaAnagrafiche As System.Collections.Generic.List(Of Ext_AnagraficaInfo) = InterrogaAnagraficaSIC(operatore, "", "", "", fatturaSIC.id_anagrafica, "")

                            If (listaAnagrafiche.Count > 0) Then
                                Dim anagraficaSIC As Ext_AnagraficaInfo = listaAnagrafiche(0)

                                Dim listaTipologiaPagamentoDB As Generic.List(Of DllDocumentale.TipoPagamentoInfo) = (New DllDocumentale.svrDocumenti(operatore)).GetTipologiePagamentoSIC(fatturaSIC.id_metodo_di_pagamento)
                                Dim sedeAnagrafica As Ext_SedeAnagraficaInfo = New Ext_SedeAnagraficaInfo
                                For j As Integer = 0 To anagraficaSIC.ListaSedi.Count - 1
                                    If Not String.IsNullOrEmpty(fatturaSIC.id_sede) Then
                                        If (anagraficaSIC.ListaSedi(j).IdSede = fatturaSIC.id_sede) Then
                                            sedeAnagrafica = anagraficaSIC.ListaSedi(j)
                                            Exit For
                                        End If
                                    End If
                                Next

                                anagraficaSIC.ListaSedi.Clear()
                                If Not sedeAnagrafica Is Nothing Then
                                    anagraficaSIC.ListaSedi.Add(sedeAnagrafica)
                                    anagraficaSIC.ListaSedi(0).IdModalitaPagamento = fatturaSIC.id_metodo_di_pagamento
                                    If Not listaTipologiaPagamentoDB Is Nothing AndAlso listaTipologiaPagamentoDB.Count > 0 Then
                                        anagraficaSIC.ListaSedi(0).ModalitaPagamento = listaTipologiaPagamentoDB(0).Descrizione
                                    End If

                                End If


                                Dim listaDatiBancari As New Generic.List(Of Ext_DatiBancariInfo)
                                Dim datiBancari As Ext_DatiBancariInfo = New Ext_DatiBancariInfo
                                datiBancari.Iban = fatturaSIC.iban
                                datiBancari.IdContoCorrente = fatturaSIC.id_iban
                                listaDatiBancari.Add(datiBancari)

                                anagraficaSIC.ListaSedi(0).DatiBancari = listaDatiBancari

                                objFattura.AnagraficaInfo = anagraficaSIC
                            End If

                            fatture.Add(objFattura)
                        End If
                    Next
                End If
            End If

            Return fatture
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New FaultException(Of ExtJSFault)(New ExtJSFault(ex), ex.Message)
        End Try
    End Function

End Class

<System.Runtime.Serialization.DataContract()>
Public Class ImportoDisponibile
    Public Sub New(ByVal ImportoDisponibileInp As Double)
        ImportoDisponibile = ImportoDisponibileInp
    End Sub
    Private _ImportoDisponibile As String
    Private _ImportoPotenziale As String
    <System.Runtime.Serialization.DataMember()>
    Public Property ImportoDisponibile() As String
        Get
            Return _ImportoDisponibile
        End Get
        Set(ByVal value As String)
            _ImportoDisponibile = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property ImportoPotenziale() As String
        Get
            Return _ImportoPotenziale
        End Get
        Set(ByVal value As String)
            _ImportoPotenziale = value
        End Set
    End Property
End Class

Class InterrogaDocumentiException
    Inherits Exception

    Private _code As Integer

    Sub New(ByVal message As String, ByVal code As Integer)
        MyBase.New(message)
        _code = code
    End Sub

    Public ReadOnly Property Code() As Integer
        Get
            Return _code
        End Get
    End Property
End Class