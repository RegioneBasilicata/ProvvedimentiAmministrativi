Imports System.Collections.Generic
Partial Public Class ContabileDispPerRag
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Dettaglio Contabile Ragioneria")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

        codFiscOperatore.Value = operatore.CodiceFiscale
        uffPubblicoOperatore.Value = operatore.oUfficio.CodUfficioPubblico

        Dim key As String = Request.QueryString("key")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        'imposto l'ufficio proponente
        Cod_uff_Prop.Value = objDocumento.Doc_Cod_Uff_Prop

        NPreimpReg.Value = "0"
        NimpReg.Value = "0"
        NliqReg.Value = "0"

        Dim ProcAmm As New ProcAmm

        Dim listaPreimpegni As List(Of Ext_CapitoliAnnoInfo)
        listaPreimpegni = ProcAmm.GetPreImpegniRegistratiProvvisori(objDocumento.Doc_Cod_Uff_Prop)
        NPreimpReg.Value = listaPreimpegni.Count()

        Dim lista As List(Of Ext_CapitoliAnnoInfo)
        lista = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NimpReg.Value = lista.Count()
        Dim listaLiq As List(Of Ext_LiquidazioneInfo)
        listaLiq = ProcAmm.GetTutteLiquidazioniRegistrate(objDocumento.Doc_Cod_Uff_Prop)
        NliqReg.Value = listaLiq.Count()
        Dim listaMandati As List(Of Ext_MandatoInfo) = ProcAmm.GetMandatiRegistrati()
        NMandati.Value = listaMandati.Count

        Dim lstr_numeroDocumento As String = ""
        lstr_numeroDocumento = objDocumento.Doc_numero
        If lstr_numeroDocumento = "" Then
            lstr_numeroDocumento = objDocumento.Doc_numeroProvvisorio
        End If

        Rinomina_Pagina(Me, "Dettaglio Contabile Ragioneria " & lstr_numeroDocumento)
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(key)

        chkAccertamento.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)

        Dim valoreAltro As String = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Altro)
        If Not String.IsNullOrEmpty(valoreAltro) AndAlso CInt(valoreAltro) > 0 Then
            Dim svrDocumenti As DllDocumentale.svrDocumenti = New DllDocumentale.svrDocumenti(operatore)
            TipoRettifiche.Value = svrDocumenti.DettaglioOperazioneRettifica(CInt(valoreAltro), objDocumento.Doc_Tipo).Descrizione
        End If
        If (statoIstanza.LivelloUfficio = "UR" And operatore.oUfficio.bUfficioRagioneria) Then
            isUffRag.Value = 1
        Else
            isUffRag.Value = 0
        End If
        
    End Sub

End Class