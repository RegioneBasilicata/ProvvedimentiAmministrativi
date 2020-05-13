Imports System.Collections.Generic
Partial Public Class Osservazioni
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Osservazioni")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim svrDocumenti As New DllDocumentale.svrDocumenti(operatore)
        Dim key As String = Context.Request.QueryString.Get("key")
        Dim objDoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)


        Dim lstr_numero As String = IIf(String.IsNullOrEmpty(objDoc.Doc_numero & ""), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero)
        Select Case objDoc.Doc_Tipo
            Case 0
                Rinomina_Pagina(Me, " Osservazione Determina  n° " & lstr_numero)
                TipoAtto.Value = objDoc.Doc_Tipo
            Case 1
                Rinomina_Pagina(Me, " Osservazione Delibera  n° " & lstr_numero)
                TipoAtto.Value = objDoc.Doc_Tipo
            Case 2
                Rinomina_Pagina(Me, " Osservazione Disposizione n° " & lstr_numero)
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
        End If
        If (istanza.LivelloUfficio = "UDD" And operatore.oUfficio.bUfficioDirigenzaDipartimento And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DETERMINE"))) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        End If
	If (istanza.LivelloUfficio = "USL" And operatore.oUfficio.bUfficioSegreteriaPresidenzaLegittimita And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DELIBERE"))) Then
            VerificaRuolo.Value = istanza.LivelloUfficio
        End If
        If (istanza.LivelloUfficio = "USS" And operatore.oUfficio.bUfficioSegreteriaPresidenzaSegretario And LCase(operatore.Codice) = LCase(operatore.oUfficio.ResponsabileUfficio("DELIBERE"))) Then
	    VerificaRuolo.Value = istanza.LivelloUfficio
 	End If

        AbilitaOssUP.Value = False
        Dim vett_Rit_UltimaOperazione As Object = Nothing
        Dim ultimaOperazione As String = ""
        vett_Rit_UltimaOperazione = svrDocumenti.VERIFICA_AZIONE_UFFICIO(operatore.oUfficio.CodUfficioControlloAmministrativo, , objDoc.Doc_id)
        If vett_Rit_UltimaOperazione(0) = 0 Then
            ultimaOperazione = vett_Rit_UltimaOperazione(1)
            Select Case ultimaOperazione
                Case "RIGETTO FORMALE"
                    AbilitaOssUP.Value = True
                Case ""

            End Select
        End If

    End Sub

End Class