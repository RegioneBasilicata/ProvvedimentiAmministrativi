Public Class RegistraFirmaMultiplaCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)

        Dim vCodiciDoc As Object = HttpContext.Current.Session.Item("vCodiciDoc")
        Dim hContenutoFileFirmato() As String = context.Session.Item("hContenutoFileFirmato")

        context.Session.Remove("nomeDocumentoFirma")
        context.Session.Remove("estensioneDocumentoFirma")
        HttpContext.Current.Session.Remove("vCodiciDoc")
        Dim arr As New ArrayList

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
       
        Log.Info("**************************************")
        Log.Info("****** Operatore " & oOperatore.Codice & " Inizio upload di n° " & hContenutoFileFirmato.Length & "file firmati ")
        Log.Info("**************************************")
       

        For i As Integer = 0 To hContenutoFileFirmato.Length - 1

            Dim bFile() As Byte
            Try
                bFile = Convert.FromBase64String(hContenutoFileFirmato(i))

                Dim codDocumento As String = vCodiciDoc(i, 0)

                Log.Info("****** Inizio invocazione Registra_Firma Operatore " & oOperatore.Codice & " Fine upload file firmato idDocumento " + codDocumento )
                Dim vr1 As Object = Registra_Firma(bFile, codDocumento, vCodiciDoc(i, 3), vCodiciDoc(i, 4))
                Log.Info("****** Fine invocazione Registra_Firma Operatore " & oOperatore.Codice & " Fine upload file firmato idDocumento " + codDocumento )
                If vr1(0) <> 0 Then
                    arr.Add(vCodiciDoc(i, 1))
                End If

                Log.Info("Ritorno di Registra Firma, vr(0)= " & vr1(0) & " Vr(1)= " & vr1(1))

                'REGISTRA MARCA TEMPORALE - FIRMA MULTIPLA
                'Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)
                'Log.Info("****** Inizio invocazione RegistraMarcaTemporale Operatore " & oOperatore.Codice & " Fine upload marca idDocumento " + codDocumento )
                'ojbSvrdocumenti.RegistraMarcaTemporale(bFile, codDocumento , oOperatore)
                'Log.Info("****** Fine invocazione RegistraMarcaTemporale Operatore " & oOperatore.Codice & " Fine upload marca idDocumento " + codDocumento )

            Catch ex As Exception
                If ex.Message.Contains("Marca Temporale Non Disponibile")Then
                    Log.Error("Servizio di Marcatura Temporale Non Disponibile - Operatore " & oOperatore.Codice, ex)
                Else 
                    Log.Error("Errore durante il caricamento del file firmato - Operatore " & oOperatore.Codice, ex)
                End If
                Throw New Exception("Errore - " & ex.Message)
            End Try

        Next

        Log.Info("**************************************")
        Log.Info("****** Operatore " & oOperatore.Codice & " Fine upload di n° " & hContenutoFileFirmato.Length & "file firmati ")
        Log.Info("**************************************")
        

        If arr.Count > 0 Then
            Dim lstr_errore As String = ""
            For Each lstr_doc As String In arr
                lstr_errore += lstr_doc & ", "

            Next
            lstr_errore = lstr_errore.Substring(0, lstr_errore.Length - 2)
            Throw New Exception("Errore nella firma dei seguenti documenti: " & lstr_errore)
        End If

    End Sub
End Class
