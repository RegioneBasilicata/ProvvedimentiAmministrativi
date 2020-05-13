Imports System.Collections.Generic
Partial Public Class ContabileDisp
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Dettaglio Contabile")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        NPreimpReg.Value = "0"
        NimpReg.Value = "0"
        NliqReg.Value = "0"
        chkAccertamento.Value = "0"
        chkLiquidazione.Value = "0"
        Dim key As String = Request.QueryString("key")

        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        codFiscOperatore.Value = operatore.CodiceFiscale
        uffPubblicoOperatore.Value = operatore.oUfficio.CodUfficioPubblico

        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        Dim ProcAmm As New ProcAmm

        Dim listaFattureDocumento As List(Of Ext_FatturaInfo) = ProcAmm.GetListaFattureAtto(objDocumento.Doc_id)
        If Not listaFattureDocumento Is Nothing AndAlso listaFattureDocumento.Count > 0 Then
            NFattureDocumento.Value = 1
        Else
            NFattureDocumento.Value = 0
        End If


        Dim listaPreimpegni As List(Of Ext_CapitoliAnnoInfo)
        listaPreimpegni = ProcAmm.GetPreImpegniRegistratiProvvisori(objDocumento.Doc_Cod_Uff_Prop)
        NPreimpReg.Value = listaPreimpegni.Count()

        Dim lista As List(Of Ext_CapitoliAnnoInfo)
        lista = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NimpReg.Value = lista.Count()
        Dim listaLiq As List(Of Ext_LiquidazioneInfo)
        listaLiq = ProcAmm.GetLiquidazioniRegistrate(objDocumento.Doc_Cod_Uff_Prop)
        NliqReg.Value = listaLiq.Count()
             chkLiquidazione.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            Dim svrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            TipoRettifiche.Value = svrDocumenti.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
        
        Dim lstr_numeroDocumento As String = ""
        lstr_numeroDocumento = objDocumento.Doc_numero
        If lstr_numeroDocumento = "" Then
            lstr_numeroDocumento = objDocumento.Doc_numeroProvvisorio
        End If
        Rinomina_Pagina(Me, "Dettaglio Contabile " & lstr_numeroDocumento)
        Dim objSvrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
        Dim livelloUfficio As String = objSvrDocumenti.Get_StatoIstanzaDocumento(key).LivelloUfficio

        chkAccertamento.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)

        'verifico che sia abilitato alla modifica , altrimenti non posso fare operazioni contabili
        Dim Ritorno As Boolean = VerificaAbilitazioneInoltroRigetto(operatore, objDocumento.Doc_id, "INOLTRO")

        If Ritorno And ((operatore.oUfficio.CodUfficio = objDocumento.Doc_Cod_Uff_Prop And livelloUfficio = "UP") Or ((operatore.oUfficio.CodUfficio = objDocumento.Doc_Cod_Uff_Prop And operatore.oUfficio.bUfficioDirigenzaDipartimento) And livelloUfficio = "UDD")) Then
            isUffProp.Value = 1

            Dim count As Integer = 0
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
        'imposto l'ufficio proponente
        Cod_uff_Prop.Value = objDocumento.Doc_Cod_Uff_Prop
    End Sub

End Class