Imports System.Collections.Generic
Partial Public Class MaggioriInfo
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Messaggistica")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim svrDocumenti As New DllDocumentale.svrDocumenti(operatore)
        Dim key As String = Context.Request.QueryString.Get("key")
        Dim objDoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)


        Dim lstr_numero As String = IIf(String.IsNullOrEmpty(objDoc.Doc_numero & ""), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero)
        Select Case objDoc.Doc_Tipo
            Case 0
                Rinomina_Pagina(Me, " Messaggi, Osservazioni e Suggerimenti Determina  n° " & lstr_numero)
                TipoAtto.Value = objDoc.Doc_Tipo
            Case 1
                Rinomina_Pagina(Me, " Messaggi, Osservazioni e Suggerimenti Delibera  n° " & lstr_numero)
                TipoAtto.Value = objDoc.Doc_Tipo
            Case 2
                Rinomina_Pagina(Me, " Messaggi, Osservazioni e Suggerimenti Disposizione n° " & lstr_numero)
                TipoAtto.Value = objDoc.Doc_Tipo
        End Select

        Dim dictionaryOsservazioni As Collections.Generic.Dictionary(Of String, DllDocumentale.Model.OsservazioneInfo) = svrDocumenti.GetOsservazioniPerDocumento(operatore.Codice, key)
        If dictionaryOsservazioni.ContainsKey("UCA") Then
            DirContrAmministrativo.Value = dictionaryOsservazioni.Item("UCA").Testo
        Else
            DirContrAmministrativo.Value = ""
        End If
        If dictionaryOsservazioni.ContainsKey("UP") Then
            DirProponente.Value = dictionaryOsservazioni.Item("UP").Testo
        Else
            DirProponente.Value = ""
        End If
        If dictionaryOsservazioni.ContainsKey("UR") Then
            DirRagioneria.Value = dictionaryOsservazioni.Item("UR").Testo
        Else
            DirRagioneria.Value = ""
        End If
        If dictionaryOsservazioni.ContainsKey("UDD") Then
            DirGenerale.Value = dictionaryOsservazioni.Item("UDD").Testo
        Else
            DirGenerale.Value = ""
        End If

        If dictionaryOsservazioni.ContainsKey("USL") Then
            DirSegretarioLegittimita.Value = dictionaryOsservazioni.Item("USL").Testo
        Else
            DirSegretarioLegittimita.Value = ""
        End If
        If dictionaryOsservazioni.ContainsKey("USS") Then
            DirSegretarioDiPresidenza.Value = dictionaryOsservazioni.Item("USS").Testo
        Else
            DirSegretarioDiPresidenza.Value = ""
        End If

        VerificaRuolo.Value = ""
        Dim istanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(key)

        If istanza.LivelloUfficio = "UCA" And operatore.oUfficio.bUfficioControlloAmministrativo Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        ElseIf istanza.LivelloUfficio = "UR" And operatore.oUfficio.bUfficioRagioneria Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        ElseIf istanza.LivelloUfficio = "UP" And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DETERMINE")) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        ElseIf istanza.LivelloUfficio = "USL" And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DELIBERE")) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        ElseIf istanza.LivelloUfficio = "USS" And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DELIBERE")) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        End If
        If istanza.LivelloUfficio = "UDD" And operatore.oUfficio.bUfficioDirigenzaDipartimento And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DETERMINE")) And objDoc.Doc_Cod_Uff_Prop = operatore.oUfficio.CodUfficio Then
            VerificaRuolo.Value = "UP"
        ElseIf (istanza.LivelloUfficio = "UDD" And operatore.oUfficio.bUfficioDirigenzaDipartimento And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DETERMINE")) And objDoc.Doc_Cod_Uff_Prop <> operatore.oUfficio.CodUfficio) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        End If
    End Sub

End Class