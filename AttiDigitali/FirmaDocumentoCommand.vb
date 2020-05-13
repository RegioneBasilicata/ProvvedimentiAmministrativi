Public Class FirmaDocumentoCommand
    Inherits RedirectingCommand
    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim codDocumento As String
        Dim allegatoPerFirma As Object

        context.Session.Remove("nomeDocumentoFirma")
        context.Session.Remove("estensioneDocumentoFirma")
        codDocumento = context.Request.QueryString.Get("key")

        If context.Session.Item("codAzione") = "INOLTRO" Then
            Dim objdocumento As New DllDocumentale.svrDocumenti(oOperatore)
            If context.Session.Item("destinatarioInoltro") <> Nothing Then
                objdocumento.Numera(codDocumento, oOperatore, context.Request.Item("tipo"), context.Session.Item("destinatarioInoltro"))
            Else
                objdocumento.Numera(codDocumento, oOperatore, context.Request.Item("tipo"))
            End If
        End If

            ''imposto il ventuno per utilizzare il filepdfTemplate
            vettoredati = Allegato_Da_Firmare(codDocumento)
            Try
                ReDim allegatoPerFirma(UBound(vettoredati(1), 1), 0)
                For i As Integer = 0 To UBound(vettoredati(1), 1)
                    allegatoPerFirma(i, 0) = vettoredati(1)(i, UBound(vettoredati(1), 2))
                Next
                vettoredati(1) = allegatoPerFirma
                context.Items.Add("vettoreDati", vettoredati)
                context.Session.Add("idDocumento", codDocumento)
            Catch ex As InvalidCastException When vettoredati(0) = Nothing
                Throw New Exception("Impossibile leggere documenti allegati da firmare. Riprovare", ex)
            Catch ex As InvalidCastException When vettoredati(0) = 1
                Throw New Exception("Impossibile firmare in quanto non ci sono documenti allegati. Riprovare", ex)
            Catch ex As Exception
                Throw New Exception("Impossibile completare l'operazione. Riprovare", ex)
            End Try

    End Sub
End Class
