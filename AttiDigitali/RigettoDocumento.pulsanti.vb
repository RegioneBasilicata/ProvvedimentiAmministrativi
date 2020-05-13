Imports System.Collections.Generic
Partial Class RigettoDocumento
    Private Sub gestionePulsanti_Regione()

        Dim idDocumento As String = ""
        If Not Request.QueryString.Get("key") Is Nothing Then
            idDocumento = Context.Request.QueryString.Get("key")
        Else
            idDocumento = Context.Session.Item("key")
        End If
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Dim mess As String = ""
        Dim tipoSuggerimento As String = ""
        Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(idDocumento)
        If listaSugg.Count > 0 And String.IsNullOrEmpty(tipoSuggerimento) Then
            'Ci sono suggerimenti
            tipoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
            mess = " ultimo suggerimento attribuito: " & tipoSuggerimento
        End If

        'Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)
       
        Dim rigettoabilitato As Boolean = dllDoc.VerificaAbilitazioneInoltroRigetto(idDocumento, oOperatore, "RIGETTO")
        btnRigetta.Enabled = rigettoabilitato
        btnFirma.Enabled = rigettoabilitato
        Dim docFirmato As Integer = Verifica_Firma_Utente_Documento(idDocumento, "RIGETTO")
        If docFirmato > 0 Then
            btnFirma.Visible = False
            btnRigetta.Text = "RIGETTA"
            Label1.Text = "Premi su RIGETTA per inviare il provvedimento al successivo responsabile"
            Label2.Text = ""
        Else
            btnFirma.Visible = True
        End If
        Dim messaggi_di_Warning As New ArrayList
        If Not String.IsNullOrEmpty(dllDoc.ErrDescrizione) Then
            messaggi_di_Warning.Add(dllDoc.ErrDescrizione)
            dllDoc.ErrDescrizione = ""
        End If
        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(idDocumento)
        Dim messaggioonclick As String = ""
        If statoIstanza.LivelloUfficio = "UDD" And statoIstanza.CodiceUfficio = oOperatore.oUfficio.CodUfficio And statoIstanza.Ruolo <> "R" Then
            'propone la direzione generale
            messaggioonclick = Warning(idDocumento)
            If messaggioonclick.Contains("lblRed") Then
                'btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
            End If
            messaggi_di_Warning.Add(messaggioonclick)
            If rigettoabilitato Then
                'modificare il label del bottone 
            End If
        End If

        If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
            messaggioonclick = Warning(idDocumento)
            If messaggioonclick.Contains("lblRed") Then
                'btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
            End If
            messaggi_di_Warning.Add(messaggioonclick)
            If rigettoabilitato Then
                'modificare il label del bottone 
            End If
        End If
        If statoIstanza.LivelloUfficio = "UR" Then
            messaggi_di_Warning.Add(Warning_Ragioneria(idDocumento))
        End If
        If statoIstanza.LivelloUfficio = "USS" Then
            messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(idDocumento))
        End If


        Context.Session.Add("warning", messaggi_di_Warning)



        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
            If mess.Contains("inoltr") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                ' btnInoltra.BorderColor = Color.Red
            End If
            If mess.Contains("rigett") Then
                ' btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnRigetta.BorderColor = Color.Red
            End If
        End If
    End Sub
    Private Sub gestionePulsantiMultiplo_Regione()

        Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

        Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim dllDoc As DllDocumentale.svrDocumenti = DllDocumentale.AbstractSvrDocumenti.getSvrDocumenti(oOperatore)

     
        If vDocumentiDaInoltrare.Length = 1 Then
            Context.Session.Add("key", vDocumentiDaInoltrare(0))
            CreaWarning(vDocumentiDaInoltrare(0), oOperatore, dllDoc)
            gestionePulsanti()
        Else
            CreaWarning(vDocumentiDaInoltrare, oOperatore, dllDoc)
            
            Dim messaggi_di_Warning As New ArrayList
            messaggi_di_Warning = DirectCast(Context.Session.Item("warning"), ArrayList)

            Dim rigettoabilitato As Boolean = True

            For Each docDaInoltrare As String In vDocumentiDaInoltrare
                Dim abilitaInoltro As Boolean = dllDoc.VerificaAbilitazioneInoltroRigetto(docDaInoltrare, oOperatore, "RIGETTO")
                If abilitaInoltro And rigettoabilitato Then
                    rigettoabilitato = True
                Else
                    rigettoabilitato = False
                End If

                'nel VerificaAbilitazioneInoltroRigetto possono venire aggiunti dei msg di warning, 
                'che non venivano mostrati all'utente, è stato necessaio aggiungere:
                Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(docDaInoltrare)
                If Not String.IsNullOrEmpty(dllDoc.ErrDescrizione) Then
                    If messaggi_di_Warning IsNot Nothing Then
                        For i As Integer = 0 To messaggi_di_Warning.Count - 1
                            Dim messaggio As String = messaggi_di_Warning.Item(i)
                            If messaggio.ToUpper().Contains(objDocumento.Numero.ToUpper()) Then
                                messaggi_di_Warning.Item(i) = messaggio + dllDoc.ErrDescrizione
                            End If
                        Next
                    End If
                End If
            Next

            Context.Session.Add("warning", messaggi_di_Warning)
            
            btnRigetta.Enabled = rigettoabilitato
            btnFirma.Enabled = rigettoabilitato


        End If

    End Sub


    'Private Sub gestionePulsanti_Alsia()

    '    Dim idDocumento As String = ""
    '    If Not Request.QueryString.Get("key") Is Nothing Then
    '        idDocumento = Context.Request.QueryString.Get("key")
    '    Else
    '        idDocumento = Context.Session.Item("key")
    '    End If

    '    Dim obj As Object = Leggi_Documento(idDocumento)
    '    Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
    '    Dim messaggi_di_Warning As New ArrayList
    '    Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

    '    Dim tipoSuggerimento As String = ""
    '    Dim mess As String = ""
    '    Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(idDocumento)
    '    If listaSugg.Count > 0 And String.IsNullOrEmpty(tipoSuggerimento) Then
    '        'Ci sono suggerimenti
    '        tipoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
    '        mess = " ultimo suggerimento attribuito:  " & tipoSuggerimento
    '    End If

    '    Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(idDocumento)

    '    btnRigetta.Enabled = False
    '    btnInoltra.Enabled = True
    '    btnRigetta.Visible = False
    '    Dim messaggioOnClick As String = ""
    '    If statoIstanza.LivelloUfficio = "UDD" And statoIstanza.CodiceUfficio = obj(2) And statoIstanza.Ruolo <> "R" Then
    '        'propone la direzione generale
    '        messaggioOnClick = Warning(idDocumento)
    '        If messaggioOnClick.Contains("lblRed") Then
    '            btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
    '        End If
    '        messaggi_di_Warning.Add(messaggioOnClick)
    '    End If
    '    If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR") Then
    '        messaggioOnClick = Warning(idDocumento)
    '        If messaggioOnClick.Contains("lblRed") Then
    '            btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
    '        End If
    '        messaggi_di_Warning.Add(messaggioOnClick)
    '    End If
    '    If statoIstanza.LivelloUfficio = "UR" Then
    '        messaggi_di_Warning.Add(Warning_Ragioneria(idDocumento))
    '        pnlRigetto.Visible = True
    '        btnRigetta.Enabled = True
    '        btnRigetta.Visible = True


    '    End If
    '    Context.Session.Add("warning", messaggi_di_Warning)



    '    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

    '    'vRit(0) ABILITA RIGETTO
    '    'vRit(1) ABILITA Inoltro



    '    If obj(0) = 0 Then
    '        If obj(2) = oOperatore.oUfficio.CodUfficio And statoIstanza.LivelloUfficio = "UP" Then
    '            Label2.Visible = False
    '            btnRigetta.Visible = False
    '        End If

    '    End If

    '    If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
    '        If mess.Contains("inoltr") Then
    '            btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '            btnInoltra.BorderColor = Color.Red
    '        End If
    '        If mess.Contains("rigett") Then
    '            btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '            btnRigetta.BorderColor = Color.Red
    '        End If
    '    End If
    'End Sub


    'Private Sub gestionePulsantiMultiplo_Alsia()

    '    Dim elencoDocumentiDaInoltrare As Object = Context.Session.Item("elencoDocumentiDaInoltrare")

    '    Dim vDocumentiDaInoltrare As String() = Split(elencoDocumentiDaInoltrare, ",")
    '    Dim dllDoc As New DllDocumentale.svrDocumenti(Session.Item("oOperatore"))
    '    Dim messaggi_di_Warning As New ArrayList
    '    Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

    '    Dim primoSuggerimento As String = ""
    '    Dim mess As String = ""
    '    Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(vDocumentiDaInoltrare(0))
    '    If listaSugg.Count > 0 And String.IsNullOrEmpty(primoSuggerimento) Then
    '        'Ci sono suggerimenti
    '        primoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
    '        mess = " ultimo suggerimento attribuito: " & primoSuggerimento
    '    End If



    '    For Each docDaInoltrare As String In vDocumentiDaInoltrare
    '        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
    '            listaSugg = Elenco_Suggerimenti(docDaInoltrare)

    '            If listaSugg.Count > 0 Then
    '                If listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia <> primoSuggerimento Then
    '                    mess = "Sono stati selezionati documenti che riportano suggerimenti discordanti, prima di procedere verificare le operazione da effettuare"
    '                End If
    '            End If
    '        End If
    '        Dim messaggioOnClick As String = ""
    '        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dllDoc.Get_StatoIstanzaDocumento(docDaInoltrare)
    '        If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR") Then
    '            messaggioOnClick = Warning(docDaInoltrare)
    '            If messaggioOnClick.Contains("lblRed") Then
    '                btnInoltra.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
    '            End If
    '            messaggi_di_Warning.Add(messaggioOnClick)
    '        End If
    '        If statoIstanza.LivelloUfficio = "UR" Then
    '            messaggi_di_Warning.Add(Warning_Ragioneria(docDaInoltrare))
    '        End If

    '    Next
    '    Context.Session.Add("warning", messaggi_di_Warning)
    '    If vDocumentiDaInoltrare.Length = 1 Then
    '        Context.Session.Add("key", vDocumentiDaInoltrare(0))
    '        gestionePulsanti()
    '    Else
    '        btnInoltra.Enabled = True
    '        btnRigetta.Enabled = True
    '        If oOperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
    '            If mess.Contains("inoltr") Then
    '                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '                btnInoltra.BorderColor = Color.Red
    '            End If
    '            If mess.Contains("rigett") Then
    '                btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '                btnRigetta.BorderColor = Color.Red
    '            End If
    '            If mess.Contains("discordanti") Then
    '                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '                btnInoltra.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
    '            End If

    '        End If
    '    End If


    'End Sub
    Sub ModificaPreImpSIC(ByVal codDocumento As String)


        '  ModificaPreImpSIC(codDocumento)
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim respUfficio As String = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE")

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(oOperatore)).Get_StatoIstanzaDocumento(codDocumento)

        If (statoIstanza.LivelloUfficio = "UR" And oOperatore.oUfficio.bUfficioRagioneria And LCase(respUfficio) = LCase(oOperatore.Codice)) Then


            Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(codDocumento)
            Dim tipoAtto As String = ""
            Dim dataAtto As Date = objDocumento.Doc_Data

            Dim numeroAtto As String = Right(objDocumento.Doc_numero, 5)
            Select Case objDocumento.Doc_Tipo
                Case 0
                    tipoAtto = "DETERMINA"
                Case 1
                    tipoAtto = "DELIBERA"
                Case 2
                    tipoAtto = "DISPOSIZIONE"

            End Select

            Dim numeroProvvisOrDefAtto As String = ""
            If objDocumento.Doc_numero = "" then
	            numeroProvvisOrDefAtto = objDocumento.Doc_numeroProvvisorio
            Else
	            numeroProvvisOrDefAtto = objDocumento.Doc_numero
            End If

            VariaPreimpPerRiduzioni(codDocumento, tipoAtto, numeroAtto, dataAtto, objDocumento.Doc_Oggetto, numeroProvvisOrDefAtto)
        End If



    End Sub
    Public Sub VariaPreimpPerRiduzioni(ByVal idDocumento As String, ByVal tipoAtto As String, ByVal numeroAtto As String, ByVal dataAtto As Date, ByVal oggettoAtto As String, ByVal numeroProvvisOrDefAtto As String)

        Dim listaRiduzioni As IList(Of DllDocumentale.ItemRiduzioneInfo)

        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
        listaRiduzioni = dllDoc.FO_Get_DatiImpegniVariazioni(idDocumento)
        Dim flagResult As Boolean = False
        For Each item As DllDocumentale.ItemRiduzioneInfo In listaRiduzioni

            Try

                'GEstione per riportare i soldi sul capitolo
                If Not item.Div_IsEconomia And item.Di_Stato = 1 Then
                    Dim listaImpegni As IList(Of DllDocumentale.ItemImpegnoInfo) = dllDoc.FO_GetImpegno(item.Dli_NumImpegno)
                    If listaImpegni.Count = 1 Then
                        If listaImpegni(0).Di_PreImpDaPrenotazione <> 1 Then
                            'Riduco il Pre Imp
                            ClientIntegrazioneSic.MessageMaker.createGenerazioneVariazionePreIMPMessage(oOperatore, listaImpegni(0).Dli_NPreImpegno, -item.Dli_Costo, tipoAtto, dataAtto, numeroAtto, item.Div_Data_Assunzione, oggettoAtto, idDocumento, numeroProvvisOrDefAtto, item.HashTokenCallSic)

                        End If
                    End If

                End If
            Catch ex As Exception
                If Not ex.Message.Contains("Mancata Disponibilita' Fondi PreImpegno") Then
                    HttpContext.Current.Session.Add("error", "Atto Numero:" & numeroAtto & " " & ex.Message)
                    Response.Redirect("MessaggioErrore.aspx")
                End If


            End Try

        Next
    End Sub
    Private Sub CreaWarning(ByVal vDocumentiDaInoltrare() As String, ByVal ooperatore As DllAmbiente.Operatore, ByRef dlldoc As DllDocumentale.svrDocumenti)
        Dim messaggi_di_Warning As New ArrayList
        Dim primoSuggerimento As String = ""
        Dim mess As String = ""
        Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(vDocumentiDaInoltrare(0))
        If listaSugg.Count > 0 And String.IsNullOrEmpty(primoSuggerimento) Then
            'Ci sono suggerimenti
            primoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
            mess = " ultimo suggerimento attribuito: " & primoSuggerimento
        End If

        If Not String.IsNullOrEmpty(dlldoc.ErrDescrizione) Then
            messaggi_di_Warning.Add(dlldoc.ErrDescrizione)
            dlldoc.ErrDescrizione = ""
        End If

        For Each docDaInoltrare As String In vDocumentiDaInoltrare
            If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) Then
                listaSugg = Elenco_Suggerimenti(docDaInoltrare)

                If listaSugg.Count > 0 Then
                    If listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia <> primoSuggerimento Then
                        mess = "Sono stati selezionati documenti che riportano suggerimenti discordanti, prima di procedere verificare le operazione da effettuare"
                    End If
                End If
            End If
            Dim messaggioOnClick As String = ""
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dlldoc.Get_StatoIstanzaDocumento(docDaInoltrare)
            If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
                messaggioOnClick = Warning(docDaInoltrare)
                If messaggioOnClick.Contains("lblRed") Then
                    btnRigetta.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                    btnFirma.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                End If
                If messaggioOnClick.Contains("lblRFirma") Then
                    btnRigetta.Attributes.Add("onclick", "return confirm('Si sta rigettando senza firma, continuare?')")
                End If
                messaggi_di_Warning.Add(messaggioOnClick)
            End If
            If statoIstanza.LivelloUfficio = "UR" Then
                messaggi_di_Warning.Add(Warning_Ragioneria(docDaInoltrare))
            End If
            If statoIstanza.LivelloUfficio = "USS" Then
                messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(docDaInoltrare))
            End If

        Next
        Context.Session.Add("warning", messaggi_di_Warning)
        If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
            If mess.Contains("rigett") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If
            If mess.Contains("discordanti") Then
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If

        End If
    End Sub
    Private Sub CreaWarning(ByVal iddocumento As String, ByVal ooperatore As DllAmbiente.Operatore, ByRef dlldoc As DllDocumentale.svrDocumenti)
        Dim messaggi_di_Warning As New ArrayList
        Dim primoSuggerimento As String = ""
        Dim mess As String = ""
        Dim tipoSuggerimento As String = ""
        Dim listaSugg As Generic.List(Of Ext_SuggerimentoInfo) = Elenco_Suggerimenti(iddocumento)
        If listaSugg.Count > 0 And String.IsNullOrEmpty(tipoSuggerimento) Then
            'Ci sono suggerimenti
            tipoSuggerimento = listaSugg.Item(listaSugg.Count - 1).DescrizioneTipologia
            mess = " ultimo suggerimento attribuito: " & tipoSuggerimento
        End If

        Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = dlldoc.Get_StatoIstanzaDocumento(iddocumento)
        Dim messaggioonclick As String = ""

        If Not String.IsNullOrEmpty(dlldoc.ErrDescrizione) Then
            messaggi_di_Warning.Add(dlldoc.ErrDescrizione)
            dlldoc.ErrDescrizione = ""
        End If
        If statoIstanza.LivelloUfficio = "UDD" And statoIstanza.CodiceUfficio = ooperatore.oUfficio.CodUfficio And statoIstanza.Ruolo <> "R" Then
            'propone la direzione generale
            messaggioonclick = Warning(iddocumento)
            If messaggioonclick.Contains("lblRed") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
            End If
            If messaggioonclick.Contains("lblRFirma") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie sulla firma, continuare?')")
            End If

            messaggi_di_Warning.Add(messaggioonclick)
        End If

        If statoIstanza.LivelloUfficio = "UP" Or ((statoIstanza.Ruolo = "R") And statoIstanza.LivelloUfficio <> "UR" And statoIstanza.LivelloUfficio <> "USS") Then
            messaggioonclick = Warning(iddocumento)
            If messaggioonclick.Contains("lblRed") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('Il riepilogo contiene anomalie, continuare?')")
            End If
            If messaggioonclick.Contains("lblRFirma") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('Si sta rigettando senza firma, continuare?')")
            End If
            messaggi_di_Warning.Add(messaggioonclick)
        End If
        If statoIstanza.LivelloUfficio = "UR" Then
            messaggi_di_Warning.Add(Warning_Ragioneria(iddocumento))
        End If
        If statoIstanza.LivelloUfficio = "USS" Then
            messaggi_di_Warning.Add(Warning_SegreteriaPresidenza(iddocumento))
        End If

        Context.Session.Add("warning", messaggi_di_Warning)
        If ooperatore.oUfficio.Test_Attributo("ATTIVA_SUGGERIMENTO", True) And Not String.IsNullOrEmpty(mess) Then
            If mess.Contains("rigett") Then
                btnRigetta.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
                btnFirma.Attributes.Add("onclick", "return confirm('" & mess & " , continuare?')")
            End If
        End If

    End Sub
End Class
