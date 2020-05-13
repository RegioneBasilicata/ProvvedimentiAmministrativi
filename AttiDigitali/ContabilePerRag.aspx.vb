Imports System.Collections.Generic
Partial Public Class ContabilePerRag
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Dettaglio Contabile Ragioneria")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim key As String = Request.QueryString("key")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)
        Dim tipologia As String = Request.QueryString("tipo")
        'imposto l'ufficio proponente
        Cod_uff_Prop.Value = objDocumento.Doc_Cod_Uff_Prop

        codFiscOperatore.Value = operatore.CodiceFiscale
        uffPubblicoOperatore.Value = operatore.oUfficio.CodUfficioPubblico

        tipo.Value = tipologia
        NpreimpReg.Value = "0"
        NimpReg.Value = "0"
        NliqReg.Value = "0"
        NridReg.Value = "0"
        Dim ProcAmm As New ProcAmm

        Dim listaPreimpegni As List(Of Ext_CapitoliAnnoInfo)
        listaPreimpegni = ProcAmm.GetPreImpegniRegistratiProvvisori(objDocumento.Doc_Cod_Uff_Prop)
        NpreimpReg.Value = listaPreimpegni.Count()
        Dim lista As List(Of Ext_CapitoliAnnoInfo)
        lista = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NimpReg.Value = lista.Count()
        Dim numLiqReg As Integer
        numLiqReg = ProcAmm.Get_LiquidazioniTutte(operatore).Count
        NliqReg.Value = numLiqReg
        Dim listaRid As List(Of Ext_CapitoliAnnoInfo)
        listaRid = ProcAmm.GetRiduzioniRegistrate()
        NridReg.Value = listaRid.Count()
        Dim listaMandati As List(Of Ext_MandatoInfo) = ProcAmm.GetMandatiRegistrati()
        NMandati.Value = listaMandati.Count
        Dim listaRidPreImp As List(Of Ext_CapitoliAnnoInfo)
        listaRidPreImp = ProcAmm.GetRiduzioniPreImpegniRegistrati()
        NridPreImpReg.Value = listaRidPreImp.Count
        Dim listaRidLiq As List(Of Ext_LiquidazioneInfo)
        listaRidLiq = ProcAmm.GetRiduzioniLiquidazioniRegistrate()
        NridLiqReg.Value = listaRidLiq.Count

        Dim lstr_numeroDocumento As String = ""
        lstr_numeroDocumento = objDocumento.Doc_numero
        If lstr_numeroDocumento = "" Then
            lstr_numeroDocumento = objDocumento.Doc_numeroProvvisorio
        End If
        Rinomina_Pagina(Me, "Dettaglio Contabile Ragioneria " & lstr_numeroDocumento)

        chkAccertamento.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
        Dim svrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)

        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            TipoRettifiche.Value = svrDocumenti.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = svrDocumenti.Get_StatoIstanzaDocumento(key)
        If (statoIstanza.LivelloUfficio = "UR" And operatore.oUfficio.bUfficioRagioneria) Then
            isUffRag.Value = 1
        Else
            isUffRag.Value = 0
        End If
        If statoIstanza.LivelloUfficio = "UR" And statoIstanza.Ruolo = "M" Then
            IsMandato.Value = 1
        Else
            IsMandato.Value = 0
        End If

        'imposto l'ufficio proponente
        Cod_uff_Prop.Value = objDocumento.Doc_Cod_Uff_Prop
    End Sub

End Class