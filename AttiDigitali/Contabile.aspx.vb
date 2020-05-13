Imports System.Collections.Generic
Partial Public Class Contabile
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Dettaglio Contabile")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HttpContext.Current.Session.Remove("msgEsito")
        
        Dim key As String = Request.QueryString("key")
        Dim tipoAtto As String = Request.QueryString("tipo")
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        codDocumento.Value = key
        tipo.Value = tipoAtto

        NPreimpReg.Value = "0"
        NimpReg.Value = "0"
        NliqReg.Value = "0"

        'imposto l'ufficio proponente
        Cod_uff_Prop.Value = objDocumento.Doc_Cod_Uff_Prop

        codFiscOperatore.Value = operatore.CodiceFiscale
        uffPubblicoOperatore.Value = operatore.oUfficio.CodUfficioPubblico

        Dim ProcAmm As New ProcAmm
        Dim listaImpegniRegistrati As List(Of Ext_CapitoliAnnoInfo)
        listaImpegniRegistrati = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NimpReg.Value = listaImpegniRegistrati.Count()

        Dim listaPrenotazioniRegistrate As New List(Of Ext_CapitoliAnnoInfo)
        'listaImpegniRegistrati = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NPreimpReg.Value = listaPrenotazioniRegistrate.Count()


        Dim listaLiq As List(Of Ext_LiquidazioneInfo)
        listaLiq = ProcAmm.GetLiquidazioniRegistrate(objDocumento.Doc_Cod_Uff_Prop)
        Dim listaLiqContestualiNonPerenti As List(Of Ext_LiquidazioneInfo)
        listaLiqContestualiNonPerenti = ProcAmm.GetLiquidazioniRegistrateContestualiNonPerenti(objDocumento.Doc_Cod_Uff_Prop)
        NliqReg.Value = listaLiq.Count()
        Dim listaMandati As List(Of Ext_MandatoInfo) = ProcAmm.GetMandatiRegistrati()
        NMandati.Value = listaMandati.Count
        Dim listaRidLiq As List(Of Ext_LiquidazioneInfo) = ProcAmm.GetRiduzioniLiquidazioniRegistrate
        RidLiq.Value = listaRidLiq.Count

        'For Each impegno As Ext_CapitoliAnnoInfo In listaImpegniRegistrati
        '    Dim listaFattureImpegno As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattByImpegno(impegno.ID, objDocumento.Doc_id)
        '    RidLiq.Value = listaRidLiq.Count
        'Next

        Dim listaFattureDocumento As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattureAtto(objDocumento.Doc_id)
        If Not listaFattureDocumento Is Nothing AndAlso listaFattureDocumento.Count > 0 Then
            NFattureDocumento.Value = 1
        Else
            NFattureDocumento.Value = 0
        End If


        chkPreimpegni.Value = "0"
        chkImpegno.Value = "0"
        chkImpegnoSuPerenti.Value = "0"
        chkAccertamento.Value = "0"
        chkRiduzione.Value = "0"
        chkLiquidazione.Value = "0"
        chkRiduzionePreImp.Value = "0"
        chkRiduzioneLiq.Value = "0"

        Dim lstr_numeroDocumento As String = ""
        lstr_numeroDocumento = objDocumento.Doc_numero
        If lstr_numeroDocumento = "" Then
            lstr_numeroDocumento = objDocumento.Doc_numeroProvvisorio
        End If
        Rinomina_Pagina(Me, "Dettaglio Contabile " & lstr_numeroDocumento)
        Dim objSvrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = objSvrDocumenti.Get_StatoIstanzaDocumento(key)
        'verifico che la ragioneria non abbia già rigettato, altrimenti non posso fare operazioni contabili
        Dim Ritorno As Boolean = VerificaAbilitazioneInoltroRigetto(operatore, objDocumento.Doc_id, "INOLTRO")


        If Ritorno And ((operatore.oUfficio.CodUfficio = objDocumento.Doc_Cod_Uff_Prop And statoIstanza.LivelloUfficio = "UP" And LCase(operatore.Codice) = LCase(statoIstanza.Operatore)) _
                        Or (((operatore.oUfficio.CodUfficio = objDocumento.Doc_Cod_Uff_Prop And operatore.oUfficio.bUfficioDirigenzaDipartimento And statoIstanza.LivelloUfficio = "UDD") _
                             Or (operatore.oUfficio.CodUfficio = objDocumento.Doc_Cod_Uff_Prop And operatore.oUfficio.bUfficioSegreteriaPresidenzaLegittimita And statoIstanza.LivelloUfficio = "USL")) _
                        And LCase(operatore.Codice) = LCase(statoIstanza.Operatore))) Then
            isUffProp.Value = 1
            'gestione testo del provvdimento
            Dim vRitTestoCaricato As Object = Nothing
            Select Case objDocumento.Doc_Tipo
                Case 0
                    vRitTestoCaricato = objSvrDocumenti.FO_Leggi_Allegato_Con_Parametri(objDocumento.Doc_id, "", "1")
                Case 1
                    vRitTestoCaricato = objSvrDocumenti.FO_Leggi_Allegato_Con_Parametri(objDocumento.Doc_id, "", "5")
                Case 2
                    vRitTestoCaricato = objSvrDocumenti.FO_Leggi_Allegato_Con_Parametri(objDocumento.Doc_id, "", "8")
            End Select
            If vRitTestoCaricato(0) = 0 Then
                TestoCaricato.Value = 1
            Else
                TestoCaricato.Value = 0
            End If
        Else
            isUffProp.Value = 0
        End If

        Dim count As Integer = 0
        chkPreimpegni.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Preimpegno)
        If chkPreimpegni.Value = "1" And isUffProp.Value = 1 Then
            For Each prenotazione As Ext_CapitoliAnnoInfo In listaPrenotazioniRegistrate
                If prenotazione.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                PrenotazioniDaConfermare.Value = 1
            Else
                PrenotazioniDaConfermare.Value = 0
            End If
            count = 0
        End If


        chkImpegno.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
        If chkImpegno.Value = "1" And isUffProp.Value = 1 Then
            For Each impegno As Ext_CapitoliAnnoInfo In listaImpegniRegistrati
                If impegno.Stato = 2 And impegno.isPerente = False Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                ImpegniDaConfermare.Value = 1
            Else
                ImpegniDaConfermare.Value = 0
            End If
            count = 0
            'verifica liquidazioni contestuali
            For Each liq As Ext_LiquidazioneInfo In listaLiqContestualiNonPerenti
                If liq.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                LiquidazioniContestualiDaConfermare.Value = 1
            Else
                LiquidazioniContestualiDaConfermare.Value = 0
            End If
            count = 0
        End If
        chkImpegnoSuPerenti.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
        If chkImpegnoSuPerenti.Value = "1" And isUffProp.Value = 1 Then
            For Each impegno As Ext_CapitoliAnnoInfo In listaImpegniRegistrati
                If impegno.Stato = 2 And impegno.isPerente = True Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                ImpegniPerentiDaConfermare.Value = 1
            Else
                ImpegniPerentiDaConfermare.Value = 0
            End If
            count = 0
        End If
        chkAccertamento.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
        chkLiquidazione.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)

        If chkLiquidazione.Value = "1" And isUffProp.Value = 1 Then
            For Each liq As Ext_LiquidazioneInfo In listaLiq
                If liq.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                LiquidazioniDaConfermare.Value = 1
            Else
                LiquidazioniDaConfermare.Value = 0
            End If
            count = 0
        End If
        chkRiduzione.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)
        If chkRiduzione.Value = "1" And isUffProp.Value = 1 Then
            Dim listaRiduzioni As List(Of Ext_CapitoliAnnoInfo) = ProcAmm.GetRiduzioniRegistrate()
            For Each item As Ext_CapitoliAnnoInfo In listaRiduzioni
                If item.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                RiduzioniDaConfermare.Value = 1
            Else
                RiduzioniDaConfermare.Value = 0
            End If
            count = 0
        End If
        chkRiduzionePreImp.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzionePreImp)
        If chkRiduzionePreImp.Value = "1" And isUffProp.Value = 1 Then
            Dim listaRiduzioni As List(Of Ext_CapitoliAnnoInfo) = ProcAmm.GetRiduzioniPreImpegniRegistrati()
            For Each item As Ext_CapitoliAnnoInfo In listaRiduzioni
                If item.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                RiduzioniPreImpDaConfermare.Value = 1
            Else
                RiduzioniPreImpDaConfermare.Value = 0
            End If
            count = 0
        End If
        chkRiduzioneLiq.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.RiduzioneLiq)
        If chkRiduzioneLiq.Value = "1" And isUffProp.Value = 1 Then
            Dim listaRiduzioniLiq As List(Of Ext_LiquidazioneInfo) = ProcAmm.GetRiduzioniLiquidazioniRegistrate()
            For Each item As Ext_LiquidazioneInfo In listaRiduzioniLiq
                If item.Stato = 2 Then
                    count = count + 1
                    Exit For
                End If
            Next
            If count > 0 Then
                RiduzioniLiqDaConfermare.Value = 1
            Else
                RiduzioniLiqDaConfermare.Value = 0
            End If
            count = 0
        End If
        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            Dim svrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            TipoRettifiche.Value = svrDocumenti.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
       
    End Sub

End Class