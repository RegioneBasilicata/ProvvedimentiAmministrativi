Imports DllDocumentale
Imports DllDocumentale.Model

Public Class RegistraFirmaDocumentoCommand
    Implements ICommand
    Protected Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(RegistraFirmaDocumentoCommand))

    Protected Sub execute(ByVal context As HttpContext) Implements ICommand.execute
        Dim codDocumento As String = context.Request.Item("key")

        Try
            Dim nomeDocumentoFirma As String = "Documento_Principale"
            Dim estensioneDocumentoFirma As String = "pdf" 'DirectCast(context.Request.Item("estensioneDocumentoFirmato"), String)

            Dim MyStream As System.IO.Stream
            Dim uploadedFile As HttpPostedFile = context.Request.Files.Item("uploadedfile")
            Dim fileLength As Long = uploadedFile.ContentLength

            Dim bFile(fileLength) As Byte
            MyStream = uploadedFile.InputStream
            MyStream.Read(bFile, 0, fileLength)

            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

            Log.Info("**************************************")
            Log.Info("****** Operatore " & oOperatore.Codice & " Inizio upload file firmato idDocumento" + codDocumento)
            Log.Info("**************************************")

            Log.Info("****** Inizio invocazione Registra_Firma Operatore " & oOperatore.Codice & " Fine upload file firmato idDocumento " + codDocumento)
            Dim vr1 As Object = Registra_Firma(bFile, codDocumento, nomeDocumentoFirma, estensioneDocumentoFirma)
            Log.Info("****** Fine invocazione Registra_Firma Operatore " & oOperatore.Codice & " Fine upload file firmato idDocumento " + codDocumento)

            Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)
            Log.Info("****** Inizio Registrazione Documento_Conservazione " & oOperatore.Codice & " idDocumento " + codDocumento)

            Dim objdoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
            Dim allegatoInfo As AllegatoInfo = ojbSvrdocumenti.FO_Get_Allegato_Firma(vr1(1))

            Dim numeroProvvedimento As String = ""

            If Not String.IsNullOrEmpty(objdoc.Doc_numero) Then
                numeroProvvedimento = objdoc.Doc_numero
            Else
                numeroProvvedimento = objdoc.Doc_numeroProvvisorio
            End If
            ojbSvrdocumenti.Registra_Allegato_Documento_Conservazione(codDocumento, numeroProvvedimento, vr1(1), oOperatore.Codice, "P7M", allegatoInfo)

            Log.Info("****** Fine Registrazione Documento_Conservazione " & oOperatore.Codice & " idDocumento " + codDocumento)

            ''REGISTRA MARCA TEMPORALE - FIRMA SINGOLO PROVVEDIMENTO
            'Dim ojbSvrdocumenti As New DllDocumentale.svrDocumenti(oOperatore)
            'Log.Info("****** Inizio invocazione RegistraMarcaTemporale Operatore " & oOperatore.Codice & " Fine upload marca idDocumento " + codDocumento )
            'ojbSvrdocumenti.RegistraMarcaTemporale(bFile, codDocumento, oOperatore)
            'Log.Info("****** Fine invocazione RegistraMarcaTemporale Operatore " & oOperatore.Codice & " Fine upload marca idDocumento " + codDocumento )


            Log.Info("**************************************")
            Log.Info("****** Operatore " & oOperatore.Codice & " Fine upload file firmato idDocumento " + codDocumento)
            Log.Info("**************************************")

        Catch ex As Exception
            If ex.Message.Contains("Marca Temporale Non Disponibile") Then
                Log.Error("Servizio di Marcatura Temporale Non Disponibile", ex)
            Else
                Log.Error("Errore durante il caricamento del file firmato", ex)
            End If
            Throw ex
        End Try
    End Sub

End Class