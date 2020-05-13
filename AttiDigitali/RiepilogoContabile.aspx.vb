Imports System.Collections.Generic
Partial Public Class RiepilogoContabile
    Inherits System.Web.UI.Page

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Riepilogo Informazioni Contabili")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim key As String = Request.QueryString("key")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        NimpReg.Value = "0"
        NliqReg.Value = "0"
        NridReg.Value = "0"
        Dim ProcAmm As New ProcAmm
        Dim lista As List(Of Ext_CapitoliAnnoInfo)
        lista = ProcAmm.GetImpegniRegistrati(objDocumento.Doc_Cod_Uff_Prop)
        NimpReg.Value = lista.Count()
        Dim listaLiq As List(Of Ext_LiquidazioneInfo)
        listaLiq = ProcAmm.GetLiquidazioniRegistrate(objDocumento.Doc_Cod_Uff_Prop)
        NliqReg.Value = listaLiq.Count()
        Dim listaRid As List(Of Ext_CapitoliAnnoInfo)
        listaRid = ProcAmm.GetRiduzioniRegistrate()
        NridReg.Value = listaRid.Count()

        Dim listaMandati As List(Of Ext_MandatoInfo) = ProcAmm.GetMandatiRegistrati()
        NMandati.Value = listaMandati.Count



       Dim strSourceOnReady As String = ""

        Select Case objDocumento.Doc_Tipo
            Case 0
                strSourceOnReady = "ext/onReadyDetermineDisabil.js"

                chkImpegno.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Impegno)
                chkImpegnoSuPerenti.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.ImpegniSuPerenti)
                chkAccertamento.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Accertamento)
                chkLiquidazione.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Liquidazione)
                chkRiduzione.Value = objDocumento.getOpContabile(DllDocumentale.EnumDocumenti.TipoOperazioniContabili.Riduzione)

            Case 1
                strSourceOnReady = ""
            Case 2
                strSourceOnReady = "ext/onReadyDisposizioneDisabil.js"
                chkLiquidazione.Value = "1"
        End Select

        Me.ClientScript.RegisterClientScriptInclude("onready", strSourceOnReady)





    End Sub

End Class