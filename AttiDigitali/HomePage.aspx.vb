Imports System.Collections.Generic

Public Class HomePage
    Inherits WebSession

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Contenuto As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Albero As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Testata As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents PlaceTotaleAtti As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblBenvenuto As System.Web.UI.WebControls.Label
    Protected WithEvents lblAlert As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.

        InitializeComponent()
        Inizializza_Pagina(Me, "Benvenuto")
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'modgg 10-06 1
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")

        Try
            Dim objDll As New DllDocumentale.svrDocumenti(oOperatore)

            Dim numDetermineDepositoUfficio As Integer

            Dim numDelibereDepositoUfficio As Integer
            Dim numDisposizioniDepositoUfficio As Integer
            Dim numDecretiDepositoUfficio As Integer
            Dim numOrdinanzeDepositoUfficio As Integer

            Dim numDaArchiviare As Integer
            Dim numDelDaArchiviare As Integer

            Dim numDetermineDepositoUrgenti As Integer = 0
            Dim numDisposizioniDepositoUrgenti As Integer = 0

            Dim numMessaggi As Integer

            numDetermineDepositoUfficio = IIf(IsNumeric(Context.Items("numDetermineDepositoUfficio")), CInt(Context.Items("numDetermineDepositoUfficio")), -1)
            numDelibereDepositoUfficio = IIf(IsNumeric(Context.Items("numDelibereDepositoUfficio")), CInt(Context.Items("numDelibereDepositoUfficio")), -1)
            numDisposizioniDepositoUfficio = IIf(IsNumeric(Context.Items("numDisposizioniDepositoUfficio")), CInt(Context.Items("numDisposizioniDepositoUfficio")), -1)
            numDecretiDepositoUfficio = IIf(IsNumeric(Context.Items("numDecretiDepositoUfficio")), CInt(Context.Items("numDecretiDepositoUfficio")), -1)
            numOrdinanzeDepositoUfficio = IIf(IsNumeric(Context.Items("numOrdinanzeDepositoUfficio")), CInt(Context.Items("numOrdinanzeDepositoUfficio")), -1)

            numDaArchiviare = IIf(IsNumeric(Context.Items("numDaArchiviare")), CInt(Context.Items("numDaArchiviare")), -1)
            numDelDaArchiviare = IIf(IsNumeric(Context.Items("numDelDaArchiviare")), CInt(Context.Items("numDelDaArchiviare")), -1)

            numMessaggi = IIf(IsNumeric(Context.Items("numMessaggi")), CInt(Context.Items("numMessaggi")), -1)

            If Not Context.Session.Item("esito") Is Nothing Then
                lblAlert.Text = "<br />" + Context.Session.Item("esito") + "<br />"
                Contenuto.Controls.Add(lblAlert)
                Context.Session.Remove("esito")
            End If

            lblBenvenuto.Text = "<br/>" + lblBenvenuto.Text + " " + oOperatore.Attributo("TITOLO") + " " + oOperatore.Cognome + " " + oOperatore.Nome + "<br/>" + "<br/><br/>"
            Dim delega As DllDocumentale.ItemDelega = objDll.FO_GetDelega(oOperatore, True)
            If Not delega Is Nothing Then
                lblBenvenuto.Text += " Nel sistema è attivo " & IIf(delega.TipoDelega = 0, " una delega ", "un interim ") & " all'utente  " & delega.NominativoDelegato & "<br/>" + "<br/>"
                If delega.Del_ChiusuraAtomatica = 1 Then
                    If delega.Del_DataChiusuraAutomatica.HasValue Then
                        If Date.Compare(Now, delega.Del_DataChiusuraAutomatica) > 0 Then
                            objDll.Rimuovi_Delega(oOperatore, delega, 0)
                            Session.Remove("oOperatore")
                            'Server.Transfer("~/HomePage.aspx")
                            Server.Transfer("~/AvvisoRedirect.aspx")

                        End If


                    End If

                End If
            Else
                ''Verifica se sono deletato

                Dim delega_subita As DllDocumentale.ItemDelega = objDll.FO_GetDelegaDaDelegato(oOperatore, True)
                If Not delega_subita Is Nothing Then
                    If delega_subita.Del_ChiusuraAtomatica = 1 Then
                        If delega_subita.Del_DataChiusuraAutomatica.HasValue Then
                            If Date.Compare(Now, delega_subita.Del_DataChiusuraAutomatica) > 0 Then
                                objDll.Rimuovi_Delega(oOperatore, delega_subita, 0)
                                Session.Remove("oOperatore")
                                'Server.Transfer("~/HomePage.aspx")
                                Server.Transfer("~/AvvisoRedirect.aspx")


                            End If


                        End If
                    End If
                End If

            End If


            'If oOperatore.Ruoli.Contains("WS011") Then
            '    If numDisposizioni > 0 Then
            '        lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(numDisposizioni) + _
            '                                                CStr(IIf(numDisposizioni > 1, " disposizioni ", " disposizione ")) + _
            '                                                " nella tua <a href=""WorklistAction.aspx?tipo=2"" >lista lavoro </a> " + "<br/>" + "<br/>"
            '    End If
            'End If



            'If oOperatore.Ruoli.Contains("WT001") Then
            '    If numDetermine > 0 Then
            '        lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(numDetermine) + _
            '                                                CStr(IIf(numDetermine > 1, " determine ", " determina ")) + _
            '                                                " nella tua <a href=""WorklistAction.aspx?tipo=0"" >lista lavoro </a> " + "<br/>" + "<br/>"
            '    End If
            'End If


            Dim dllDoc As New DllDocumentale.svrDocumenti(oOperatore)
            Dim listaAtti As New Generic.List(Of DllDocumentale.ItemContaDocumenti)
            Dim listaAttiUrgenti As New Dictionary(Of String, DllDocumentale.ItemContaDocumenti)
            Dim totaleDetermineUrgenti As Integer = 0
            Dim totaleDisposizioniUrgenti As Integer = 0




            ' PROPRIA LISTA DETERMINE
            If oOperatore.Ruoli.Contains("WT001") Then
                listaAtti = dllDoc.ContaDocumentiPerDipartimento(oOperatore.Codice, 0)
                listaAttiUrgenti = dllDoc.ContaDocumentiUrgentiPerDipartimento(oOperatore.Codice, 0)
                For Each item As DllDocumentale.ItemContaDocumenti In listaAtti

                    lblBenvenuto.Text = lblBenvenuto.Text & " - Hai " & CStr(item.Totale) &
                                CStr(IIf(item.Totale > 1, " determine ", " determina ")) &
                                                                " nella tua <a href=""WorklistAction.aspx?tipo=0&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"
                    If listaAttiUrgenti.Count > 0 Then
                        Dim itemUrgente As DllDocumentale.ItemContaDocumenti = Nothing
                        Try
                            itemUrgente = listaAttiUrgenti.Item(item.CodiceDipartimento + "#" + item.TipoAtto)
                            If Not itemUrgente Is Nothing AndAlso itemUrgente.Totale > 0 Then
                                lblBenvenuto.Text = lblBenvenuto.Text + " - <a href=""WorklistAction.aspx?tipo=0&visualizzaUrgenti=true&dip=" & item.CodiceDipartimento.Trim & """ ><b style='color:red'>" & CStr(itemUrgente.Totale) + " " + CStr(IIf(itemUrgente.Totale > 1, " urgenti ", " urgente ")) + "</b></a> - "
                                totaleDetermineUrgenti = totaleDetermineUrgenti + itemUrgente.Totale
                            End If
                        Catch ex As KeyNotFoundException
                            'se sale l'eccezione non fa nulla, perchè è possibile che non esista il record con quella chiave!
                        End Try
                    End If

                    lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
                    '  Contenuto.Controls.Add(lblTotali)
                Next
            End If

            ' PROPRIA LISTA DISPOSIZIONI
            listaAtti = Nothing
            listaAttiUrgenti = Nothing
            If oOperatore.Ruoli.Contains("WS011") Then
                listaAtti = dllDoc.ContaDocumentiPerDipartimento(oOperatore.Codice, 2)
                listaAttiUrgenti = dllDoc.ContaDocumentiUrgentiPerDipartimento(oOperatore.Codice, 2)
                For Each item As DllDocumentale.ItemContaDocumenti In listaAtti
                    lblBenvenuto.Text = lblBenvenuto.Text & " - Hai " & CStr(item.Totale) & _
                                CStr(IIf(item.Totale > 1, " disposizioni ", " disposizione ")) & _
                                                                " nella tua <a href=""WorklistAction.aspx?tipo=2&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"

                    If listaAttiUrgenti.Count > 0 Then
                        Dim itemUrgente As DllDocumentale.ItemContaDocumenti = Nothing
                        Try
                            itemUrgente = listaAttiUrgenti.Item(item.CodiceDipartimento + "#" + item.TipoAtto)
                            If Not itemUrgente Is Nothing AndAlso itemUrgente.Totale > 0 Then
                                lblBenvenuto.Text = lblBenvenuto.Text + " - <a href=""WorklistAction.aspx?tipo=2&visualizzaUrgenti=true&dip=" & item.CodiceDipartimento.Trim & """ ><b style='color:red'>" & CStr(itemUrgente.Totale) + " " + CStr(IIf(itemUrgente.Totale > 1, " urgenti ", " urgente ")) + "</b></a> - "
                                totaleDisposizioniUrgenti = totaleDisposizioniUrgenti + itemUrgente.Totale
                            End If
                        Catch ex As KeyNotFoundException
                            'se sale l'eccezione non fa nulla, perchè è possibile che non esista il record con quella chiave!
                        End Try

                    End If

                    lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
                Next
            End If

            ' PROPRIA LISTA DELIBERE
            listaAtti = Nothing
            If oOperatore.Ruoli.Contains("WL001") Then
                listaAtti = dllDoc.ContaDocumentiPerDipartimento(oOperatore.Codice, 1)
                For Each item As DllDocumentale.ItemContaDocumenti In listaAtti
                    lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(item.Totale) +
                                                                          CStr(IIf(item.Totale > 1, " delibere ", " delibera ")) +
                                                                          " nella tua <a href=""WorklistAction.aspx?tipo=1&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"
                    lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
                Next
                'If numDelibere > 0 Then
                '    lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(numDelibere) + _
                '                                            CStr(IIf(numDelibere > 1, " delibere ", " delibera ")) + _
                '                                            " nella tua <a href=""WorklistAction.aspx?tipo=1&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"
                'End If
            End If

            ' PROPRIA LISTA DECRETI
            listaAtti = Nothing
            If oOperatore.Ruoli.Contains("WR001") Then
                listaAtti = dllDoc.ContaDocumentiPerDipartimento(oOperatore.Codice, 3)
                For Each item As DllDocumentale.ItemContaDocumenti In listaAtti
                    lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(item.Totale) +
                                                                          CStr(IIf(item.Totale > 1, " decreti ", " decreto ")) +
                                                                          " nella tua <a href=""WorklistAction.aspx?tipo=3&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"
                    lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
                Next
            End If

            ' PROPRIA LISTA ORDINANZE
            listaAtti = Nothing
            If oOperatore.Ruoli.Contains("WO001") Then
                listaAtti = dllDoc.ContaDocumentiPerDipartimento(oOperatore.Codice, 4)
                For Each item As DllDocumentale.ItemContaDocumenti In listaAtti
                    lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(item.Totale) +
                                                                          CStr(IIf(item.Totale > 1, " ordinanze ", " ordinanza ")) +
                                                                          " nella tua <a href=""WorklistAction.aspx?tipo=4&dip=" & item.CodiceDipartimento.Trim & """ >lista lavoro del " & item.DescrizioneDipartimento & "</a>"
                    lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
                Next
            End If


            'PRELIEVO DETERMINE
            If oOperatore.Ruoli.Contains("DT015") Then
                If numDetermineDepositoUfficio > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDetermineDepositoUfficio > 1, " - Ci sono ", " - C'è ")) + CStr(numDetermineDepositoUfficio) +
                                                            CStr(IIf(numDetermineDepositoUfficio > 1, " determine ", " determina ")) +
                                                            " in arrivo in ufficio, possono essere <a href=""GUPrelieviAction.aspx?tipo=0"" >prelevate</a> "
                    Dim detDepositoUfficioUrgenti As DllDocumentale.ItemContaDocumenti = dllDoc.ContaDocumentiUrgentiDepositoUfficio(oOperatore.oUfficio.CodUfficio, 0)
                    If Not detDepositoUfficioUrgenti Is Nothing Then
                        numDetermineDepositoUrgenti = detDepositoUfficioUrgenti.Totale
                        If numDetermineDepositoUrgenti > 0 Then
                            lblBenvenuto.Text = lblBenvenuto.Text + " - <a href=""GUPrelieviAction.aspx?tipo=0&visualizzaDepositoUrgenti=true""><b style='color:red'>" & CStr(numDetermineDepositoUrgenti) + " " + CStr(IIf(numDetermineDepositoUrgenti > 1, " urgenti ", " urgente ")) + "</b></a> - "
                        End If
                    End If
                End If
            End If
            lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"

            'PRELIEVO DELIBERE
            If oOperatore.Ruoli.Contains("DL015") Then
                If numDelibereDepositoUfficio > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDelibereDepositoUfficio > 1, " - Ci sono ", " - C'è ")) + CStr(numDelibereDepositoUfficio) + _
                                                            CStr(IIf(numDelibereDepositoUfficio > 1, " delibere ", " delibera ")) + _
                                                            " in arrivo in ufficio, possono essere <a href=""GUPrelieviAction.aspx?tipo=1"" >prelevate</a>  " + "<br/>" + "<br/>"
                End If
            End If

            'PRELIEVO DISPOSIZIONI
            If oOperatore.Ruoli.Contains("GS015") Then
                If numDisposizioniDepositoUfficio > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDisposizioniDepositoUfficio > 1, " - Ci sono ", " - C'è ")) + CStr(numDisposizioniDepositoUfficio) +
                                                            CStr(IIf(numDisposizioniDepositoUfficio > 1, " disposizioni ", " disposizione ")) +
                                                            " in arrivo in ufficio, possono essere <a href=""GUPrelieviAction.aspx?tipo=2"" >prelevate</a> "
                    Dim dispDepositoUfficioUrgenti As DllDocumentale.ItemContaDocumenti = dllDoc.ContaDocumentiUrgentiDepositoUfficio(oOperatore.oUfficio.CodUfficio, 2)
                    If Not dispDepositoUfficioUrgenti Is Nothing Then
                        numDisposizioniDepositoUrgenti = dispDepositoUfficioUrgenti.Totale
                        If numDisposizioniDepositoUrgenti > 0 Then
                            lblBenvenuto.Text = lblBenvenuto.Text + " - <a href=""GUPrelieviAction.aspx?tipo=2&visualizzaDepositoUrgenti=true""><b style='color:red'>" & CStr(numDisposizioniDepositoUrgenti) + " " + CStr(IIf(numDisposizioniDepositoUrgenti > 1, " urgenti ", " urgente ")) + "</b></a> - "
                        End If
                    End If
                End If
            End If

            'PRELIEVO DECRETI
            If oOperatore.Ruoli.Contains("DR015") Then
                If numDecretiDepositoUfficio > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDecretiDepositoUfficio > 1, " - Ci sono ", " - C'è ")) + CStr(numDecretiDepositoUfficio) +
                                                            CStr(IIf(numDecretiDepositoUfficio > 1, " decreti ", " decreto ")) +
                                                            " in arrivo in ufficio, possono essere <a href=""GUPrelieviAction.aspx?tipo=3"" >prelevati</a>  " + "<br/>" + "<br/>"
                End If
            End If

            'PRELIEVO ORDINANZE
            If oOperatore.Ruoli.Contains("DO015") Then
                If numOrdinanzeDepositoUfficio > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numOrdinanzeDepositoUfficio > 1, " - Ci sono ", " - C'è ")) + CStr(numOrdinanzeDepositoUfficio) +
                                                            CStr(IIf(numOrdinanzeDepositoUfficio > 1, " ordinanze ", " ordinanza ")) +
                                                            " in arrivo in ufficio, possono essere <a href=""GUPrelieviAction.aspx?tipo=4"" >prelevate</a>  " + "<br/>" + "<br/>"
                End If
            End If

            'DA ARCHIVIARE
            If oOperatore.Ruoli.Contains("DT017") Then
                If numDaArchiviare > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDaArchiviare > 1, " - Ci sono ", " - C'è ")) + CStr(numDaArchiviare) + _
                                                            CStr(IIf(numDaArchiviare > 1, " determine ", " determina ")) + _
                                                            " da archiviare, possono essere <a href=""WorklistArchivioAction.aspx?tipo=0"" >archiviate</a> " + "<br/>" + "<br/>"
                End If

            End If
            If oOperatore.Ruoli.Contains("DL017") Then
                If numDelDaArchiviare > 0 Then
                    lblBenvenuto.Text = lblBenvenuto.Text + CStr(IIf(numDelDaArchiviare > 1, " - Ci sono ", " - C'è ")) + CStr(numDelDaArchiviare) + _
                                                            CStr(IIf(numDelDaArchiviare > 1, " delibere ", " delibera ")) + _
                                                            " da archiviare, possono essere <a href=""WorklistArchivioAction.aspx?tipo=1"" >archiviate</a> " + "<br/>" + "<br/>"
                End If

            End If




            If numMessaggi > 0 Then
                lblBenvenuto.Text = lblBenvenuto.Text + " - Hai " + CStr(numMessaggi) + _
                                                        CStr(IIf(numMessaggi > 1, " messaggi ", " messaggio ")) + _
                                                        " nella tua casella dei <a href=""ElencoMessaggiAction.aspx"" >messaggi </a>  " + "<br/>" + "<br/>"
            End If

            Dim itemDetermineNonConformiTrasparenza As New DllDocumentale.ItemContaDocumenti
            itemDetermineNonConformiTrasparenza = dllDoc.ContaDocumentiNonConformiTrasparenzaUfficio(oOperatore.Codice, 0)
            Dim itemDisposizioniNonConformiTrasparenza As New DllDocumentale.ItemContaDocumenti
            itemDisposizioniNonConformiTrasparenza = dllDoc.ContaDocumentiNonConformiTrasparenzaUfficio(oOperatore.Codice, 2)
            lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
            'lblBenvenuto.Text = lblBenvenuto.Text & "<div style='text-align: center; border:1px solid black;'>"
            lblBenvenuto.Text = lblBenvenuto.Text & "<hr/>"
            If itemDisposizioniNonConformiTrasparenza.Totale <> 0 Then
                lblBenvenuto.Text = lblBenvenuto.Text & "<p style='font-size:11px'>* I dati relativi alla legge sulla trasparenza di <a href=""WorklistAction.aspx?tipo=2&visualizzaNonTrasp=true"">" & CStr(itemDisposizioniNonConformiTrasparenza.Totale) & _
                                    CStr(IIf(itemDisposizioniNonConformiTrasparenza.Totale > 1, " disposizioni", " disposizione")) & _
                                                                    "</a> nella tua lista lavoro non sono completi.</p>"
            End If
            If itemDetermineNonConformiTrasparenza.Totale <> 0 Then
                lblBenvenuto.Text = lblBenvenuto.Text & "<p style='font-size:11px'>* I dati relativi alla legge sulla trasparenza di <a href=""WorklistAction.aspx?tipo=0&visualizzaNonTrasp=true"">" & CStr(itemDetermineNonConformiTrasparenza.Totale) & _
                                    CStr(IIf(itemDetermineNonConformiTrasparenza.Totale > 1, " determine", " determina")) & _
                                                                    "</a> nella tua lista lavoro non sono completi.</p>"
                lblBenvenuto.Text = lblBenvenuto.Text & "<br/><br/>"
            End If
            'lblBenvenuto.Text = lblBenvenuto.Text & "</div>"

            Contenuto.Controls.Add(lblBenvenuto)

            Context.Session.Remove("txtCodDipartimenti")

            'Dim vetRuoli As Hashtable
            'Dim ruoloAnonimo As DllAmbiente.RuoloInfo = New DllAmbiente.RuoloInfo("ANONIM", "", "")

            'If Not oOperatore Is Nothing Then
            '    If oOperatore.esito = 0 Or oOperatore.esito = 2 Then

            '        vetRuoli = oOperatore.Ruoli
            '    Else
            '        vetRuoli = New Hashtable
            '        If Not vetRuoli.ContainsKey(ruoloAnonimo.Codice) Then
            '            vetRuoli.Add(ruoloAnonimo.Codice, ruoloAnonimo)
            '        End If
            '    End If
            'Else
            '    vetRuoli = New Hashtable
            '    If Not vetRuoli.ContainsKey(ruoloAnonimo.Codice) Then
            '        vetRuoli.Add(ruoloAnonimo.Codice, ruoloAnonimo)
            '    End If
            'End If
            'Dim root As Microsoft.Web.UI.WebControls.TreeNode
            'root = New Microsoft.Web.UI.WebControls.TreeNode
            'root.Expanded() = True
            Context.Session.Add("numDetermineUrgenti", totaleDetermineUrgenti)
            Context.Session.Add("numDisposizioniUrgenti", totaleDisposizioniUrgenti)

            Context.Session.Add("numDetermineDepositoUrgenti", numDetermineDepositoUrgenti)
            Context.Session.Add("numDisposizioniDepositoUrgenti", numDisposizioniDepositoUrgenti)
            'Dim treeView1 As Microsoft.Web.UI.WebControls.TreeView = Context.Session.Item("TreeView1")
            'treeView1.Nodes.Clear()
            FunzWeb.refreshAlbero(Context)
            'FunzWeb.carica_xmlAttivita(Context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory)
            'treeView1.Nodes.Add(root)




        Catch ex As Exception
            HttpContext.Current.Session.Add("error", ex.Message)
            Response.Redirect("MessaggioErrore.aspx")
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Private Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload

    End Sub
End Class
