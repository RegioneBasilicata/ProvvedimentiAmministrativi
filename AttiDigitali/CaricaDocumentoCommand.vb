Public Class CaricaDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String
        Dim testoDocumentoFile() As Byte
        Dim estensione As String
        Dim nomeFile As String
        Dim tipologiaDocumento As Integer
        Dim vR As Object = Nothing
        'Dim oDllDocumento As New DllDocumentale.svrDocumenti
        Dim versioneAllegato As Integer = 1


        codDocumento = context.Session.Item("key")
        testoDocumentoFile = context.Session.Item("testoDocumento")
        estensione = context.Session.Item("estensione")
        nomeFile = context.Session.Item("nomeFile")



        Select Case CInt(context.Session.Item("tipo"))
            Case 0 'Si tratta di una determina
                tipologiaDocumento = 1
            Case 1 'Si tratta di una delibera
                tipologiaDocumento = 5
            Case 2 'Si tratta di una disposizione
                tipologiaDocumento = 8
            Case 3 'Si tratta di un decreto
                tipologiaDocumento = 10
            Case 4 'Si tratta di una ordinanza
                tipologiaDocumento = 11
        End Select

        vR = Elenco_Allegati(codDocumento, tipologiaDocumento)

        If vR(0) = 0 Then
            versioneAllegato = CInt(vR(4)) + 1
            vR = Nothing

            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(context.Session.Item("oOperatore"))).Get_StatoIstanzaDocumento(codDocumento)
            Dim objdocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
            If (statoIstanza.LivelloUfficio = "UP") or ((statoIstanza.LivelloUfficio = "UDD") and objdocumento.Doc_Cod_Uff_Prop=oOperatore.oUfficio.CodUfficio)Then
                'quando viene uploadato un nuovo documento word, deve invalidare tutte le firme precedenti, cancellando l'elenco certificati di firme
                Cancella_Allegato_Fisicamente(, "16", codDocumento)
                'modifico tutti i ruoli specifici
                Modifica_Compiti(codDocumento)
            End If
        
            nomeFile = nomeFile & "_Vers. " & versioneAllegato.ToString
            vR = Registra_Allegato(testoDocumentoFile, nomeFile, estensione, codDocumento, tipologiaDocumento, versioneAllegato)
        Else
            vR = Nothing
            nomeFile = nomeFile & "_Vers. " & versioneAllegato.ToString
            vR = Registra_Allegato(testoDocumentoFile, nomeFile, estensione, codDocumento, tipologiaDocumento)


        End If
        Dim msgEsito As String = ""

        If vR(0) = 0 Then
            Dim idUltimoDoc As String = vR(1)
            vR = Nothing
            Dim utilita As New UtilityWord()
            Dim arrayTras As Byte() = utilita.TrasformaDocInpdfBynary(testoDocumentoFile)

            '' salvo template
            vR = Registra_Allegato(arrayTras, "Documento_Principale", "pdf", codDocumento, 21, 1, , , , False, False)
            If vR(0) <> 0 Then
                Cancella_Allegato_Fisicamente(idUltimoDoc)
            End If
        Else

            msgEsito = "Impossibile salvare il documento. L'operazione non è andata a buon fine. Riprovare il caricamento." & vbCrLf
            HttpContext.Current.Session.Add("msgEsito", msgEsito)

        End If



        If vR(0) <> 0 Then

            msgEsito = "Impossibile salvare il documento. L'operazione non è andata a buon fine. Riprovare il caricamento." & vbCrLf
            HttpContext.Current.Session.Add("msgEsito", msgEsito)

        End If
        context.Session.Add("tipoApplic", CInt(context.Session.Item("tipo")))
        context.Session.Add("key", codDocumento)
    End Sub


End Class
