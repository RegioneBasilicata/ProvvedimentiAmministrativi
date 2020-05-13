Public Class LeggiDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim codDocumento As String = context.Request.QueryString.Get("key")
        ' Dim datiXml As String = context.Session.Item("datiXml")
        Dim oggetto As String
        Dim isContabile As String
        Dim intPubIntegrale As String
        Dim testo As String = ""
        Dim vR As Object = Nothing
        Dim numeroDoc As String
        'Dim fileXmlTemplate As String
        'Dim xmlTemplate As New System.Xml.XmlDocument

        'Dim urlCorrente As String = context.Request.Path

        'Dim urlChiamante As String = context.Request.UrlReferrer.Segments(context.Request.UrlReferrer.Segments.Length - 1)
        'If InStr(UCase(urlChiamante), "DELIBERA") Or InStr(UCase(urlCorrente), "DELIBERA") Then
        '    fileXmlTemplate = ConfigurationManager.AppSettings("templateDatiRegistraDelibera")
        'End If

        'If InStr(UCase(urlChiamante), "DETERMINA") Or InStr(UCase(urlCorrente), "DETERMINA") Then
        '    fileXmlTemplate = ConfigurationManager.AppSettings("templateDatiRegistraDetermina")
        'End If

        'If InStr(UCase(urlChiamante), "DISPOSIZIONE") Or InStr(UCase(urlCorrente), "DISPOSIZIONE") Then
        '    fileXmlTemplate = ConfigurationManager.AppSettings("templateDatiRegistraDisposizione")
        'End If

        'xmlTemplate.Load(AppDomain.CurrentDomain.BaseDirectory + fileXmlTemplate)

        'vR = Leggi_Documento(codDocumento, xmlTemplate.OuterXml)
        vR = Leggi_Documento(codDocumento)

        oggetto = ""
        isContabile = ""

        If vR(0) = 0 Then

            'lu 181109 Modifica per gestire il redirect e l'apertura albero
            context.Session.Add("codDocumento", codDocumento)
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(codDocumento)
            If LCase(statoIstanza.Operatore) = LCase(oOperatore.Codice) Then
                Dim vRVerifica = Verifica_Prima_Apertura(codDocumento)

                If vRVerifica(0) = 0 Then
                    aggiorna_Documenti_Sessione(context, "APRI", codDocumento, vRVerifica(2), vRVerifica(1))
                End If
            End If
            'lu 181109 fine Modifica per gestire il redirect e l'apertura albero

            'datiXml = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_xmlDatiDocumento)
            'Dim lstrXML1 As String = datiXml.Substring(0, datiXml.IndexOf("<oggetto>"))
            'Dim lstrXML2 As String = datiXml.Substring(datiXml.IndexOf("</oggetto>") + 10)
            'datiXml = lstrXML1 & lstrXML2
            oggetto = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_oggetto)
            testo = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_testo)
            numeroDoc = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_numUtenteDocumento)
            isContabile = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_isContabile)
            intPubIntegrale = vR(1)(DllDocumentale.Dic_FODocumentale.vr_Leggi_Documento.c_pubIntegrale)
        End If
        context.Items.Add("tipo", context.Request.QueryString.Item("tipo"))
        context.Items.Add("CodUffProp", vR(2))
        'context.Items.Add("xmlDati", datiXml)
        context.Items.Add("oggetto", oggetto)
        context.Items.Add("testoEditato", testo)
        context.Items.Add("numeroDoc", numeroDoc)
        context.Items.Add("intOpContabili", isContabile)
        context.Items.Add("pubIntegrale", intPubIntegrale)


    End Sub
End Class
