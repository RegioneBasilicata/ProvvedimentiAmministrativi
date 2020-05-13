Public Class CancellaAllegatiCommand
    Inherits RedirectingCommand
    'modgg16
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)
        Dim elencoAllegatiDaCancellare As String
        Dim vAllegatiDaCancellare() As String
        Dim i As Integer
        Dim codDocumento As String

        elencoAllegatiDaCancellare = context.Session.Item("elencoAllegatiDaCancellare")

        vAllegatiDaCancellare = Split(elencoAllegatiDaCancellare, ",")
        For i = 0 To UBound(vAllegatiDaCancellare)
            If Trim(vAllegatiDaCancellare(i)) <> "" Then
                Call Cancella_Allegati(vAllegatiDaCancellare(i))
            End If
        Next

        Dim vettoredati As Object
        If context.Request.QueryString.Get("key") Is Nothing Then
            codDocumento = context.Session.Item("key")
        Else
            codDocumento = context.Request.QueryString.Get("key")
        End If
        vettoredati = Elenco_Allegati(codDocumento)
        context.Items.Add("vettoreDati", vettoredati)
        Dim r As Integer
        Dim countAlletati As Integer = 0
        If vettoredati(0) <> 1 Then

            For r = 0 To UBound(vettoredati(1), 2)
                Dim tipoAllegato As String = vettoredati(1)(1, r)
                If tipoAllegato.ToLower.Contains("allegato") Or tipoAllegato.ToLower.Contains("cartaceo") Or tipoAllegato.ToLower.Contains("documenti") Then
                    countAlletati = countAlletati + 1
                End If
            Next
        End If
        If countAlletati = 0 Then
            Dim itemRicercato As New DllDocumentale.Documento_attributo
            itemRicercato.Doc_id = codDocumento
            itemRicercato.Cod_attributo = DllDocumentale.Dic_FODocumentale.Attributi_Documento.PAGINE_TOTALI_ALLEGATI.ToString
            itemRicercato.Ente = ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
            dlldoc.FO_Delete_Documento_Attributi(itemRicercato)
        End If

        'modgg 10-06
        If context.Items.Item("vettoreDocumento") Is Nothing Then
            vettoredati = Leggi_Documento(codDocumento)
            context.Items.Add("vettoreDocumento", vettoredati)
        End If

        'Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
       ' Dim dlldoc As New DllDocumentale.svrDocumenti(oOperatore)
        dlldoc.Messagio_WebServiceNotifica(codDocumento, DllDocumentale.svrDocumenti.Stato_Notifica.Modificato, -1, "Cancellato Allegato")

    End Sub
End Class
