Public Class RegistraFirmaDocumentoAXCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Session.Item("key")
        Dim hContenutoFileFirmato As String = context.Session.Item("hContenutoFileFirmato")

        Dim nomeDocumentoFirma As String = DirectCast(context.Session.Item("nomeDocumentoFirmato"), String)
        Dim estensioneDocumentoFirma As String = DirectCast(context.Session.Item("estensioneDocumentoFirmato"), String)


        context.Session.Remove("nomeDocumentoFirmato")
        context.Session.Remove("estensioneDocumentoFirmato")

        Dim bFile() As Byte
        Dim lstr_errore As String = ""
        Try
            bFile = Convert.FromBase64String(hContenutoFileFirmato)
            If nomeDocumentoFirma = "" Then
                nomeDocumentoFirma = "Documento_Firmato_" & oOperatore.Codice & ".doc"
            End If
            If estensioneDocumentoFirma = "" OrElse estensioneDocumentoFirma.EndsWith("doc") Then
                estensioneDocumentoFirma = "pdf"
            End If
            Dim vr1 As Object
            Try
                vr1 = Registra_Firma(bFile, codDocumento, nomeDocumentoFirma, estensioneDocumentoFirma)


            Catch ex As Exception
                Log.Error("Definizione compito per l'utente non riuscita" & ex.Message)
                Throw New Exception("Definizione compito per l'utente non riuscita - " & ex.Message)
            End Try

            Log.Debug("Ritorno di Registra Firma, vr(0)= " & vr1(0) & " Vr(1)= " & vr1(1))

            If vr1(0) <> 0 Then
                lstr_errore = "Errore nella firma del documento."
            End If
        Catch ex As Exception
            Throw New Exception("Formato di file non valido - " & ex.Message)
        End Try

        If lstr_errore <> "" Then
            Throw New Exception(lstr_errore)
        End If

    End Sub

End Class