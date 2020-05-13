Imports System.Xml.XPath
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Web.UI.WebControls
Imports System.Xml
Imports DllDocumentale



Module FunzWeb
    Dim o As ConfigXmlDocument
    Public Function verificaAbilitazione(ByVal ruoliXml As String, ByVal ruoliUtente As Hashtable) As Boolean
        Dim abilitato As Boolean = False
        Dim i As Integer
        If (ruoliXml.IndexOf(";") > 0) Then
            Dim ruoliSplittati As Array = Split(ruoliXml, ";")
            For i = 0 To (ruoliSplittati.Length - 1)
                If ruoliUtente.Contains(ruoliSplittati(i)) Or ruoliXml = "*" Then
                    abilitato = True
                    Exit For
                End If
            Next i
        Else
            If ruoliUtente.Contains(ruoliXml) Or ruoliXml = "*" Then   'rocco
                abilitato = True
            End If
        End If
        Return abilitato
    End Function
   
    Public Sub carica_xmlAttivita(ByRef context As HttpContext, ByRef root As TreeNode, ByVal ruoli As Hashtable, ByVal urlBase As String)
        '*************************************************
        'Scrive nel TreeNode passato per riferimento     *
        'il contenuto del file XML                       *
        'il cui percorso è caricato dal WebConfig        *
        '*************************************************

        Dim domXmlTree As New XmlDocument
        Dim nodoAttivita As XmlNode

        Dim fileSchede As String = ConfigurationManager.AppSettings("albero")

        domXmlTree.Load(urlBase + fileSchede)

        root.Expanded() = True
        nodoAttivita = domXmlTree.SelectSingleNode("root/attivita")
        If nodoAttivita.HasChildNodes Then
            riempiAlbero(context, nodoAttivita, root, ruoli)
        End If
    End Sub

    Public Sub carica_xmlDocumentiAperti(ByRef context As HttpContext, ByRef root As TreeNode, ByVal ruoli As Hashtable, ByVal urlBase As String, ByVal documentiAperti As String, ByVal numDocumentiAperti As String, ByVal tipoDoc As Integer)
        '*************************************************
        'Scrive nel TreeNode passato per riferimento     *
        'il contenuto del file XML                       *
        'il cui percorso è caricato dal WebConfig        *
        '*************************************************

        Dim domXmlTree As New XmlDocument
        Dim nodoElenco As XmlNode
        Dim fileSchede As String
        Select Case (tipoDoc)
            Case 0
                fileSchede = ConfigurationManager.AppSettings("albero_determina")
            Case 1
                fileSchede = ConfigurationManager.AppSettings("albero_delibera")
            Case 2
                fileSchede = ConfigurationManager.AppSettings("albero_disposizione")
            Case 3
                fileSchede = ConfigurationManager.AppSettings("albero_decreto")
            Case 4
                fileSchede = ConfigurationManager.AppSettings("albero_ordinanza")

        End Select
        Dim snodoIstanza As String
        Dim vCodDocumento() As String
        Dim vNumDocumento() As String
        Dim snodoIstanze As String
        Dim nodoIstanza As XmlNode
        Dim i As Integer


        domXmlTree.Load(urlBase + fileSchede)

        nodoIstanza = domXmlTree.SelectSingleNode("//istanza")
        snodoIstanza = nodoIstanza.OuterXml
        nodoIstanza.ParentNode.RemoveChild(nodoIstanza)

        snodoIstanze = ""
        nodoElenco = domXmlTree.SelectSingleNode("root/elenco")
        vCodDocumento = Split(documentiAperti, ";")
        vNumDocumento = Split(numDocumentiAperti, ";")
        For i = 0 To UBound(vCodDocumento) - 1
            snodoIstanza = nodoIstanza.OuterXml
            If Trim(vCodDocumento(i)) <> "" Then
                snodoIstanza = Replace(snodoIstanza, "#codDocumento#", vCodDocumento(i))
                snodoIstanze = snodoIstanze & Replace(snodoIstanza, "#numDocumento#", vNumDocumento(i))
            End If
        Next
        nodoElenco.InnerXml = snodoIstanze
        root.Expanded() = True
        If nodoElenco.HasChildNodes Then
            riempiAlbero(context, nodoElenco, root, ruoli)
        End If
    End Sub

    Public Sub aggiornaAlbero(ByRef root As TreeNode, ByVal ruolo As Hashtable)
        '*************************************************
        'Scrive nel TreeNode passato per riferimento     *
        'il contenuto del file XML                       *
        'il cui percorso è caricato dal WebConfig        *
        '*************************************************
        root.Expanded() = True
        Dim x As New XmlDocument
        Dim nodoAttivita As XmlNode
        Dim nodo2 As XmlElement
        Dim fileSchede As String = ConfigurationManager.AppSettings("albero")
        x.Load(fileSchede)
        root.Expanded() = True
        nodoAttivita = x.SelectSingleNode("root/elenco/istanza")
        Dim nodoDetermina As New TreeNode
        nodoDetermina.Text = "Determina n°"
        root.Nodes.Add(nodoDetermina)
        'nodoAttivita = nodoAttivita.FirstChild
        For Each nodo2 In nodoAttivita
            Dim tvnode As New TreeNode
            If Not nodo2.Attributes("ruoli") Is Nothing And Not nodo2.Attributes("NavigateUrl") Is Nothing Then
                If (verificaAbilitazione(nodo2.Attributes("ruoli").Value, ruolo)) Then
                    tvnode.Text = nodo2.LocalName
                    tvnode.NavigateUrl() = nodo2.Attributes("NavigateUrl").Value
                    root.Nodes.Add(tvnode)
                End If
            End If
        Next
    End Sub
    Public Sub refreshAlbero(ByRef context As HttpContext)

        Dim determineAperte As String = context.Session.Item("determineAperte")
        Dim numDetermineAperte As String = context.Session.Item("numDetermineAperte")

        Dim delibereAperte As String = context.Session.Item("delibereAperte")
        Dim numDelibereAperte As String = context.Session.Item("numDelibereAperte")

        Dim disposizioniAperte As String = context.Session.Item("disposizioniAperte")
        Dim numDisposizioniAperte As String = context.Session.Item("numDisposizioniAperte")

        Dim decretiAperte As String = context.Session.Item("decretiAperte")
        Dim numDecretiAperte As String = context.Session.Item("numDecretiAperte")

        Dim ordinanzeAperte As String = context.Session.Item("ordinanzeAperte")
        Dim numOrdinanzeAperte As String = context.Session.Item("numOrdinanzeAperte")

        Dim vetRuoli As Hashtable
        Dim ruoloAnonimo As DllAmbiente.RuoloInfo = New DllAmbiente.RuoloInfo("ANONIM", "", "")
        Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        If Not oOperatore Is Nothing Then
            If oOperatore.esito = 0 Or oOperatore.esito = 2 Then

                vetRuoli = oOperatore.Ruoli
            Else
                vetRuoli = New Hashtable
                If Not vetRuoli.ContainsKey(ruoloAnonimo.Codice) Then
                    vetRuoli.Add(ruoloAnonimo.Codice, ruoloAnonimo)
                End If
            End If
        Else
            vetRuoli = New Hashtable
            If Not vetRuoli.ContainsKey(ruoloAnonimo.Codice) Then
                vetRuoli.Add(ruoloAnonimo.Codice, ruoloAnonimo)
            End If
        End If
        Dim treeView1 As Microsoft.Web.UI.WebControls.TreeView = context.Session.Item("TreeView1")
        treeView1.Nodes.Clear()
        'FunzWeb.carica_xmlAttivita(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory)
        'treeView1.Nodes.Add(root)


        Dim root As TreeNode
        root = New TreeNode
        root.Expanded() = True
        treeView1.SelectExpands() = True
        FunzWeb.carica_xmlAttivita(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory)
        If Not determineAperte Is Nothing Then
            FunzWeb.carica_xmlDocumentiAperti(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory, determineAperte, numDetermineAperte, 0)
        End If
        If Not delibereAperte Is Nothing Then
            FunzWeb.carica_xmlDocumentiAperti(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory, delibereAperte, numDelibereAperte, 1)
        End If
        If Not disposizioniAperte Is Nothing Then
            FunzWeb.carica_xmlDocumentiAperti(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory, disposizioniAperte, numDisposizioniAperte, 2)
        End If
        If Not decretiAperte Is Nothing Then
            FunzWeb.carica_xmlDocumentiAperti(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory, decretiAperte, numDecretiAperte, 3)
        End If
        If Not ordinanzeAperte Is Nothing Then
            FunzWeb.carica_xmlDocumentiAperti(context, root, vetRuoli, AppDomain.CurrentDomain.BaseDirectory, ordinanzeAperte, numOrdinanzeAperte, 4)
        End If

        treeView1.Nodes.Add(root)

    End Sub
    Public Sub riempiAlbero(ByRef context As HttpContext, ByVal nodoXml As XmlNode, ByRef nodes As TreeNode, ByVal ruolo As Hashtable, Optional ByVal valoreAttributo As String = "")
        nodes.Expanded() = True

        If Not nodoXml.HasChildNodes Then
            Exit Sub
        End If

        Dim tvNode As New TreeNode
        Select Case nodoXml.FirstChild.NodeType
            Case XmlNodeType.Text
                If Not nodoXml.Attributes("NavigateUrl") Is Nothing Then
                    If Not nodoXml.Attributes("ruoli") Is Nothing Then

                        If (verificaAbilitazione(nodoXml.Attributes("ruoli").Value & "", ruolo)) Then

                            If Not nodoXml.Attributes("Target") Is Nothing Then
                                tvNode.Target = nodoXml.Attributes("Target").Value() & ""
                            End If

                            ' Anagrafica Contratti
                            If nodoXml.FirstChild.ParentNode.Name = "sicContratti" Then
                                Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")

                                Dim codFiscOperatore As String = operatore.CodiceFiscale
                                Dim uffPubblicoOperatore As String = operatore.oUfficio.CodUfficioPubblico

                                tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value()
                                tvNode.NavigateUrl = Replace(tvNode.NavigateUrl, "#codFiscOperatore#", codFiscOperatore)
                                tvNode.NavigateUrl = Replace(tvNode.NavigateUrl, "#uffPubblicoOperatore#", uffPubblicoOperatore)
                                tvNode.Text = nodoXml.FirstChild.Value
                                nodes.Nodes.Add(tvNode)
                                ' Determine Urgenti
                            ElseIf nodoXml.FirstChild.Value.Contains("#totDetUrgenti#") _
                                    AndAlso Not context.Session.Item("numDetermineUrgenti") Is Nothing Then
                                If context.Session.Item("numDetermineUrgenti") <> "0" Then
                                    tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value() & "&visualizzaUrgenti=true"
                                    tvNode.Text = Replace(nodoXml.FirstChild.Value, "#totDetUrgenti#", "<i style='color: red; font-size: 8px'><b>(" + context.Session.Item("numDetermineUrgenti").ToString & ")</b></i>")
                                    nodes.Nodes.Add(tvNode)
                                Else
                                End If
                                ' Disposizioni Urgenti
                            ElseIf nodoXml.FirstChild.Value.Contains("#totDispUrgenti#") _
                                AndAlso Not context.Session.Item("numDisposizioniUrgenti") Is Nothing Then
                                If context.Session.Item("numDisposizioniUrgenti") <> "0" Then
                                    tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value() & "&visualizzaUrgenti=true"
                                    tvNode.Text = Replace(nodoXml.FirstChild.Value, "#totDispUrgenti#", "<i style='color: red; font-size: 8px'><b>(" + context.Session.Item("numDisposizioniUrgenti").ToString & ")</b></i>")
                                    nodes.Nodes.Add(tvNode)
                                Else
                                End If
                                ' Determine Urgenti Prelevare da Deposito
                            ElseIf nodoXml.FirstChild.Value.Contains("#totDetUrgentiPrel#") _
                                AndAlso Not context.Session.Item("numDetermineDepositoUrgenti") Is Nothing Then
                                If context.Session.Item("numDetermineDepositoUrgenti") <> "0" Then
                                    tvNode.Text = Replace(nodoXml.FirstChild.Value, "#totDetUrgentiPrel#", "<i style='color: red; font-size: 8px'><b>(" + context.Session.Item("numDetermineDepositoUrgenti").ToString & ")</b></i>")
                                    tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value() & "&visualizzaDepositoUrgenti=true"
                                    nodes.Nodes.Add(tvNode)
                                Else
                                End If
                                ' Disposizioni Urgenti Prelevare da Deposito
                            ElseIf nodoXml.FirstChild.Value.Contains("#totDispUrgentiPrel#") _
                                AndAlso Not context.Session.Item("numDisposizioniDepositoUrgenti") Is Nothing Then
                                If context.Session.Item("numDisposizioniDepositoUrgenti") <> "0" Then
                                    tvNode.Text = Replace(nodoXml.FirstChild.Value, "#totDispUrgentiPrel#", "<i style='color: red; font-size: 8px'><b>(" + context.Session.Item("numDisposizioniDepositoUrgenti").ToString & ")</b></i>")
                                    tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value() & "&visualizzaDepositoUrgenti=true"
                                    nodes.Nodes.Add(tvNode)
                                Else
                                End If
                            Else
                                tvNode.Text = nodoXml.FirstChild.Value
                                tvNode.NavigateUrl = nodoXml.Attributes("NavigateUrl").Value() & ""
                                nodes.Nodes.Add(tvNode)
                            End If
                            tvNode.NodeData = "onclick=alert('')"                        
                        End If
                    End If
                End If
            Case XmlNodeType.Element
                If Not nodoXml.Attributes("ruoli") Is Nothing Then
                    If (verificaAbilitazione(nodoXml.Attributes("ruoli").Value & "", ruolo)) Then
                        If Not nodoXml.Attributes("text") Is Nothing Then
                            tvNode.Text = nodoXml.Attributes("text").Value
                        Else
                            tvNode.Text = nodoXml.LocalName
                        End If
                        'modgg 11-06 3
                        tvNode.DefaultStyle().AppendCssText("font-weight:bold")
                        nodes.Nodes.Add(tvNode)
                        Dim xmlchild As XmlNode = nodoXml.FirstChild
                        Do Until xmlchild Is Nothing
                            riempiAlbero(context, xmlchild, tvNode, ruolo)
                            xmlchild = xmlchild.NextSibling
                        Loop
                    End If
                End If
        End Select
    End Sub
    Public Function creaXmlDaForm(ByVal controllo As TextBox, ByRef xdomdati As XmlDocument) As String
        Dim nomeComposto As Array
        Dim nometabella, nomecampo As String
        Dim elemento As XmlElement
        nomeComposto = (controllo.ID).Split("0")
        nometabella = nomeComposto(0)
        nomecampo = nomeComposto(1)
        If xdomdati Is Nothing Then
            xdomdati = New XmlDocument
            elemento = xdomdati.CreateElement("datidocumento")
            xdomdati.AppendChild(elemento)
        End If
        elemento = xdomdati.SelectSingleNode("datidocumento/tabella[@nome_tabella=""" & nometabella & """]")
        If elemento Is Nothing Then
            elemento = xdomdati.CreateElement("tabella")
            elemento.SetAttribute("nome_tabella", nometabella)
            xdomdati.DocumentElement.AppendChild(elemento)
        End If
        Dim newchild As XmlElement = xdomdati.CreateElement(nomecampo)
        Dim newchildText As XmlText = xdomdati.CreateTextNode(controllo.Text)
        newchild.AppendChild(newchildText)
        elemento.AppendChild(newchild)
        'xdomdati.DocumentElement.AppendChild(elemento)
        Return xdomdati.OuterXml

    End Function
    Public Function separaArrayPerTipo(ByVal arrayUnico As Object, ByVal indiceConfronto As Integer) As Array
        Dim r, j As Integer
        Dim tipiAllegati As New ArrayList
        Dim arrayTipo(,) As Object = New Array(,) {}
        If indiceConfronto = Nothing Then
            indiceConfronto = 1
        End If
        'censisco i tipi
        For r = 0 To UBound(arrayUnico, 2)
            If Not tipiAllegati.Contains(arrayUnico.GetValue(indiceConfronto, r)) Then
                tipiAllegati.Add(arrayUnico.GetValue(indiceConfronto, r))
            End If
        Next
        Dim arrayRitorno(tipiAllegati.Count - 1) As Array
        j = 0
        For r = 0 To tipiAllegati.Count - 1
            arrayTipo = arrayPerTipo(arrayUnico, indiceConfronto, tipiAllegati(r))
            arrayRitorno.SetValue(arrayTipo, j)
            j = j + 1
        Next
        separaArrayPerTipo = arrayRitorno
    End Function
    Public Function arrayPerTipo(ByVal arrayUnico As Object, ByVal indiceconfronto As Integer, ByVal tipo As String) As Array
        Dim count As Integer
        Dim r As Integer
        Dim c As Integer
        Dim i As Integer

        count = 0
        For r = 0 To UBound(arrayUnico, 2)
            If arrayUnico.GetValue(indiceconfronto, r) = tipo Then
                count = count + 1
            End If
        Next

        If count = 0 Then
            Return Nothing
        End If

        i = 0

        Dim arrayTipo(,) As Object = New Array(,) {}
        ReDim arrayTipo(UBound(arrayUnico, 1), count - 1)
        For r = 0 To UBound(arrayUnico, 2)
            If arrayUnico.GetValue(indiceconfronto, r) = tipo Then
                For c = 0 To UBound(arrayUnico, 1)
                    arrayTipo.SetValue(CType(arrayUnico.GetValue(c, r), Object), c, i)
                Next
                i = i + 1
            End If
        Next
        arrayPerTipo = arrayTipo
    End Function
    Public Function aggiorna_Documenti_Sessione(ByRef context As HttpContext, ByVal azioneDaCompiere As String, ByVal key As String, Optional ByVal tipoDocumento As String = "", Optional ByVal valoreDaAggiungere As String = "") As String
        Dim documentiAperti As String
        Dim numDocumentiAperti As String
        Dim vDocumentiAperti() As String
        Dim vnumDocumentiAperti() As String

        Select Case UCase(azioneDaCompiere)

            Case "APRI"
                Select Case tipoDocumento
                    Case 0
                        documentiAperti = context.Session.Item("determineAperte") & ""
                        numDocumentiAperti = context.Session.Item("numDetermineAperte") & ""
                        If Trim(documentiAperti) = "" Then
                            context.Session.Add("determineAperte", key + ";")
                            context.Session.Add("numDetermineAperte", valoreDaAggiungere + ";")
                        Else
                            If InStr(documentiAperti, key, CompareMethod.Text) = 0 Then
                                context.Session.Add("determineAperte", documentiAperti + key + ";")
                                context.Session.Add("numDetermineAperte", numDocumentiAperti + valoreDaAggiungere + ";")
                            End If
                        End If
                    Case 1
                        documentiAperti = context.Session.Item("delibereAperte") & ""
                        numDocumentiAperti = context.Session.Item("numDelibereAperte") & ""
                        If Trim(documentiAperti) = "" Then
                            context.Session.Add("delibereAperte", key + ";")
                            context.Session.Add("numDelibereAperte", valoreDaAggiungere + ";")
                        Else
                            If InStr(documentiAperti, key, CompareMethod.Text) = 0 Then
                                context.Session.Add("delibereAperte", documentiAperti + key + ";")
                                context.Session.Add("numDelibereAperte", numDocumentiAperti + valoreDaAggiungere + ";")
                            End If
                        End If
                    Case 2
                        documentiAperti = context.Session.Item("disposizioniAperte") & ""
                        numDocumentiAperti = context.Session.Item("numDisposizioniAperte") & ""
                        If Trim(documentiAperti) = "" Then
                            context.Session.Add("disposizioniAperte", key + ";")
                            context.Session.Add("numDisposizioniAperte", valoreDaAggiungere + ";")
                        Else
                            If InStr(documentiAperti, key, CompareMethod.Text) = 0 Then
                                context.Session.Add("disposizioniAperte", documentiAperti + key + ";")
                                context.Session.Add("numDisposizioniAperte", numDocumentiAperti + valoreDaAggiungere + ";")
                            End If
                        End If
                    Case 3
                        documentiAperti = context.Session.Item("decretiAperte") & ""
                        numDocumentiAperti = context.Session.Item("numDecretiAperte") & ""
                        If Trim(documentiAperti) = "" Then
                            context.Session.Add("decretiAperte", key + ";")
                            context.Session.Add("numDecretiAperte", valoreDaAggiungere + ";")
                        Else
                            If InStr(documentiAperti, key, CompareMethod.Text) = 0 Then
                                context.Session.Add("decretiAperte", documentiAperti + key + ";")
                                context.Session.Add("numDecretiAperte", numDocumentiAperti + valoreDaAggiungere + ";")
                            End If
                        End If
                    Case 4
                        documentiAperti = context.Session.Item("ordinanzeAperte") & ""
                        numDocumentiAperti = context.Session.Item("numOrdinanzeAperte") & ""
                        If Trim(documentiAperti) = "" Then
                            context.Session.Add("ordinanzeAperte", key + ";")
                            context.Session.Add("numOrdinanzeAperte", valoreDaAggiungere + ";")
                        Else
                            If InStr(documentiAperti, key, CompareMethod.Text) = 0 Then
                                context.Session.Add("ordinanzeAperte", documentiAperti + key + ";")
                                context.Session.Add("numOrdinanzeAperte", numDocumentiAperti + valoreDaAggiungere + ";")
                            End If
                        End If
                End Select

                'Case "REPLACE"

                '    Select Case tipoDocumento
                '        Case 0
                '            documentiAperti = context.Session.Item("determineAperte") & ""
                '            documentiAperti = Replace(documentiAperti, key + ";", "")
                '            context.Session.Add("determineAperte", documentiAperti)

                '        Case 1
                '            documentiAperti = context.Session.Item("delibereAperte") & ""
                '            documentiAperti = Replace(documentiAperti, key + ";", "")
                '            context.Session.Add("delibereAperte", documentiAperti)

                '        Case 2
                '            documentiAperti = context.Session.Item("disposizioniAperte") & ""
                '            documentiAperti = Replace(documentiAperti, key + ";", "")
                '            context.Session.Add("disposizioniAperte", documentiAperti)

                '    End Select

            Case "CHIUDI"


                'determine
                'documentiAperti = context.Session.Item("determineAperte") & ""
                'numDocumentiAperti = context.Session.Item("numDetermineAperte") & ""
                'If documentiAperti <> "" Then
                '    tipoDocumento = 0
                'End If
                ''delibere
                'documentiAperti = context.Session.Item("delibereAperte") & ""
                'numDocumentiAperti = context.Session.Item("numDelibereAperte") & ""
                'If documentiAperti <> "" Then
                '    tipoDocumento = 1
                'End If
                ''disposizioni
                'documentiAperti = context.Session.Item("disposizioniAperte") & ""
                'numDocumentiAperti = context.Session.Item("numDisposizioniAperte") & ""
                'If documentiAperti <> "" Then
                '    tipoDocumento = 2
                'End If
                Dim i As Integer
                Dim td As Integer

                For td = 0 To 4
                    Select Case td
                        Case 0
                            documentiAperti = context.Session.Item("determineAperte") & ""
                            numDocumentiAperti = context.Session.Item("numDetermineAperte") & ""
                        Case 1
                            documentiAperti = context.Session.Item("delibereAperte") & ""
                            numDocumentiAperti = context.Session.Item("numDelibereAperte") & ""
                        Case 2
                            documentiAperti = context.Session.Item("disposizioniAperte") & ""
                            numDocumentiAperti = context.Session.Item("numDisposizioniAperte") & ""
                        Case 3
                            documentiAperti = context.Session.Item("decretiAperte") & ""
                            numDocumentiAperti = context.Session.Item("numDecretiAperte") & ""
                        Case 4
                            documentiAperti = context.Session.Item("ordinanzeAperte") & ""
                            numDocumentiAperti = context.Session.Item("numOrdinanzeAperte") & ""

                    End Select

                    vDocumentiAperti = Split(documentiAperti, ";")
                    vnumDocumentiAperti = Split(numDocumentiAperti, ";")

                    For i = 0 To UBound(vDocumentiAperti) - 1
                        If InStr(documentiAperti, key, CompareMethod.Text) > 0 Then
                            tipoDocumento = td
                        End If
                        If Trim(vDocumentiAperti(i)) = Trim(key) And Trim(vDocumentiAperti(i)) <> "" Then
                            documentiAperti = Replace(documentiAperti, vDocumentiAperti(i) + ";", "")
                            numDocumentiAperti = Replace(numDocumentiAperti, vnumDocumentiAperti(i) + ";", "")
                        End If
                    Next

                    Select Case td
                        Case 0
                            context.Session.Add("determineAperte", documentiAperti)
                            context.Session.Add("numdetermineAperte", numDocumentiAperti)
                        Case 1
                            context.Session.Add("delibereAperte", documentiAperti)
                            context.Session.Add("numdelibereAperte", numDocumentiAperti)
                        Case 2
                            context.Session.Add("disposizioniAperte", documentiAperti)
                            context.Session.Add("numdisposizioniAperte", numDocumentiAperti)
                        Case 3
                            context.Session.Add("decretiAperte", documentiAperti)
                            context.Session.Add("numDecretiAperte", numDocumentiAperti)
                        Case 4
                            context.Session.Add("ordinanzeAperte", documentiAperti)
                            context.Session.Add("numOrdinanzeAperte", numDocumentiAperti)
                    End Select
                Next
        End Select

        Return tipoDocumento

    End Function

    Public Function estraiUltimaVersione(ByVal codDoc As String, ByVal tipoDoc As Integer) As Object

        Dim vr As Object = Nothing

        vr = Elenco_Allegati(codDoc, tipoDoc)
        If vr(0) = 0 Then

            Dim indice As Integer = UBound(vr(1), 2)
            If UBound(vr(1), 2) > 0 Then
                Dim vTemp(UBound(vr(1), 1), 0) As Object
                For i As Integer = 0 To UBound(vr(1), 1)
                    vTemp(i, 0) = vr(1)(i, UBound(vr(1), 2))
                Next
                vr(1) = vTemp
                Return vr
            ElseIf UBound(vr(1), 2) = 0 Then
                Return vr
            End If
        Else
            Return vr
        End If
        Return vr


    End Function


    Public Function estraiAllegati(ByVal vettoreDati As Object) As Object
        Dim numeroAllegati As Integer = 0
        Dim numeroAllegatiMarche As Integer = 0

        For i As Integer = 0 To UBound(vettoreDati, 2)
            If DirectCast(vettoreDati, Object(,))(2, i) <> "Marca_Temporale" And UCase(vettoreDati(1, i)) <> "DETERMINA (FILE WORD)" And UCase(vettoreDati(1, i)) <> "DELIBERA (FILE WORD)" And UCase(vettoreDati(1, i)) <> "DISPOSIZIONE (FILE WORD)" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DETERMINA" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DELIBERA" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DISPOSIZIONE"  Then
           
                numeroAllegati = numeroAllegati + 1
            End If
            If  DirectCast(vettoreDati, Object(,))(2, i) = "Marca_Temporale" Then
           
                numeroAllegatiMarche = numeroAllegatiMarche + 1
            End If
            
        Next
        If numeroAllegati <> 0 Then
            Dim vettoreAllegati(UBound(vettoreDati, 1), numeroAllegati - 1) As Object
            
            Dim vettoreMarcheTemporali(UBound(vettoreDati, 1), numeroAllegatiMarche - 1) As Object

            Dim x As Integer = 0
            Dim t As Integer = 0

            For i As Integer = 0 To UBound(vettoreDati, 2)

                 If DirectCast(vettoreDati, Object(,))(2, i) <> "Marca_Temporale" And UCase(vettoreDati(1, i)) <> "DETERMINA (FILE WORD)" And UCase(vettoreDati(1, i)) <> "DELIBERA (FILE WORD)" And UCase(vettoreDati(1, i)) <> "DISPOSIZIONE (FILE WORD)" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DETERMINA" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DELIBERA" And UCase(vettoreDati(1, i)) <> "COPIA FIRMATA DI DISPOSIZIONE"  Then
             
                    For j As Integer = 0 To UBound(vettoreDati, 1)

                        vettoreAllegati(j, x) = vettoreDati(j, i)

                    Next
                    x = x + 1
                End If

                If DirectCast(vettoreDati, Object(,))(2, i) = "Marca_Temporale"Then
             
                    For j As Integer = 0 To UBound(vettoreDati, 1)

                        vettoreMarcheTemporali(j, t) = vettoreDati(j, i)

                    Next
                    t = t + 1
                End If
            Next

            Dim vTemp(3) As Object
            vTemp(0) = 0
            vTemp(1) = vettoreAllegati
            vTemp(2) = vettoreMarcheTemporali

            Return vTemp
        Else
            Dim vettoreAllegati(1) As Object
            vettoreAllegati(0) = 1
            vettoreAllegati(1) = "Nessun Record Trovato"
            Return vettoreAllegati
        End If


    End Function

    Public Function unisciVettori(ByVal v1 As Array, ByVal v2 As Array) As Object
        Dim vr(UBound(v1, 1), ((UBound(v1, 2) + UBound(v2, 2) + 1))) As Object

        For i As Integer = 0 To UBound(vr, 1)
            Dim x As Integer = 0
            For j As Integer = 0 To UBound(v1, 2)
                vr(i, j) = v1(i, j)
                'vr(i, j) = v1(j, i)
                x = x + 1
            Next

            For j As Integer = 0 To UBound(v2, 2)
                vr(i, x) = v2(i, j)
                'vr(i, x) = v2(j, i)
                x = x + 1
            Next
        Next
        Return vr
    End Function

    Public Function TipoApplic(ByVal context As HttpContext) As String
        Dim tipo As String = ""
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Request.Item("tipoApplic") Is Nothing OrElse context.Request.Item("tipoApplic") <> "" Then
                tipo = context.Request.Item("tipoApplic")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Request.QueryString.Item("tipoApplic") Is Nothing OrElse context.Request.QueryString.Item("tipoApplic") <> "" Then
                tipo = context.Request.QueryString.Item("tipoApplic")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Request.Item("tipo") Is Nothing OrElse context.Request.Item("tipo") <> "" Then
                tipo = context.Request.Item("tipo")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Request.QueryString.Item("tipo") Is Nothing OrElse context.Request.QueryString.Item("tipo") <> "" Then
                tipo = context.Request.QueryString.Item("tipo")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Items.Item("tipoApplic") Is Nothing OrElse context.Items.Item("tipoApplic") <> "" Then
                tipo = context.Items.Item("tipoApplic")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Items.Item("tipo") Is Nothing OrElse context.Items.Item("tipo") <> "" Then
                tipo = context.Items.Item("tipo")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Session.Item("tipoApplic") Is Nothing OrElse context.Session.Item("tipoApplic") <> "" Then
                tipo = context.Session.Item("tipoApplic")
            End If
        End If
        If String.IsNullOrEmpty(tipo) Then
            If Not context.Session.Item("tipo") Is Nothing OrElse context.Session.Item("tipo") <> "" Then
                tipo = context.Session.Item("tipo")
            End If
        End If

        Return tipo
    End Function
    Public Sub PreparaDocumentiDaFirmare(ByRef context As HttpContext, ByVal azione As String)
        Dim listaStringaKey As String = context.Request.Item("chkDocumenti")
        Dim vListaKey As Array
        vListaKey = listaStringaKey.Split(",")
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
        If ((operatore.Attributo("CACHEPIN") = "" Or UCase(operatore.Attributo("CACHEPIN")) = "FALSE") And vListaKey.Length > 1 And UCase(HttpContext.Current.Application.Item("NOME_ENTE_INSTALLAZIONE")) = "REGIONE") Then
            operatore.Insert_Attributo_Operatore(operatore, "CACHEPIN", True)
        End If
        Dim arrayUrl() As String = (context.Request.Url.AbsoluteUri).Split("/")
        Dim stringaUrl As String = "http://"
        Dim allegatoPerFirma As Object
        Dim vettoreDati As Object

        context.Session.Remove("urlDaFirmare")
        context.Session.Remove("vCodiciDoc")
        context.Session.Remove("elencoDocumentiDaInoltrare")

        If context.Request.Item("chkDocumenti") Is Nothing Then
            context.Session.Add("error", "Impossibile proseguire se non viene selezionato almeno un documento. Riprovare")
            context.Response.Redirect("MessaggioErrore.aspx")
        End If

        For j As Integer = 2 To arrayUrl.Length - 2
            stringaUrl = stringaUrl & arrayUrl(j) & "/"
        Next

        stringaUrl = stringaUrl & "AnteprimaAllegatoAction.aspx?key="


        Dim vCodiciDoc(vListaKey.Length - 1, 5) As String
        Dim urlStringhe(vListaKey.Length - 1) As String

        'Controllo sugli allegati
        For i As Integer = 0 To vListaKey.Length - 1
            'Firma per prendere i file template da firmare...
            vettoreDati = Allegato_Da_Firmare(vListaKey(i))

            If context.Session.Item("codAzione") = "INOLTRO" Then
                Dim objdocumento As New DllDocumentale.svrDocumenti(operatore)
                
                If context.Session.Item("destinatarioInoltro") <> Nothing Then
                    objdocumento.Numera(vListaKey(i), operatore, context.Request.Item("tipo"), context.Session.Item("destinatarioInoltro"))
                Else
                    objdocumento.Numera(vListaKey(i), operatore, context.Request.Item("tipo"))
                End If
            End If

            Dim objDoc As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(vListaKey(i))

            Try
                ReDim allegatoPerFirma(UBound(vettoreDati(1), 1), 0)

            Catch ex As InvalidCastException When vettoreDati(0) = Nothing
                'Throw New Exception("Impossibile leggere documenti allegati da firmare. Riprovare", ex)
                context.Session.Add("error", "Impossibile leggere documenti allegati da firmare. Riprovare")
                context.Response.Redirect("MessaggioErrore.aspx")
            Catch ex As InvalidCastException When vettoreDati(0) = 1
                'Throw New Exception("Impossibile firmare in quanto non ci sono documenti allegati. Riprovare", ex)
                context.Session.Add("error", "Impossibile firmare in quanto non è presente un allegato per il documento " & IIf(String.IsNullOrEmpty(objDoc.Doc_numero), objDoc.Doc_numeroProvvisorio, objDoc.Doc_numero) & ". Riprovare")
                context.Response.Redirect("MessaggioErrore.aspx")
            Catch ex As Exception
                'Throw New Exception("Impossibile completare l'operazione. Riprovare", ex)
                context.Session.Add("error", "Impossibile completare l'operazione. Riprovare")
                context.Response.Redirect("MessaggioErrore.aspx")
            End Try

            For j As Integer = 0 To UBound(vettoreDati(1), 1)
                allegatoPerFirma(j, 0) = vettoreDati(1)(j, UBound(vettoreDati(1), 2))
            Next

            vettoreDati(1) = allegatoPerFirma
            urlStringhe(i) = stringaUrl & allegatoPerFirma(0, 0)


            vCodiciDoc(i, 0) = vListaKey(i)
            vCodiciDoc(i, 1) = objDoc.Doc_numeroProvvisorio
            vCodiciDoc(i, 2) = allegatoPerFirma(0, 0)
            vCodiciDoc(i, 3) = allegatoPerFirma(2, 0)
            vCodiciDoc(i, 4) = allegatoPerFirma(3, 0)
            vCodiciDoc(i, 5) = IIf(String.IsNullOrEmpty(objDoc.Doc_numero), "", objDoc.Doc_numero)

        Next
        context.Session.Add("urlDaFirmare", urlStringhe)
        context.Session.Add("vCodiciDoc", vCodiciDoc)
        context.Session.Add("elencoDocumentiDaInoltrare", context.Request.Item("chkDocumenti"))
    End Sub
    Function DefinisciFlusso(ByVal tipoApplic As String) As String
        Dim flusso As String = ""
        Select Case tipoApplic
            Case "0"
                flusso = "DETERMINE"
            Case "1"
                flusso = "DELIBERE"
            Case "2"
                flusso = "DISPOSIZIONI"
            Case "3"
                flusso = "DECRETI"
            Case "4"
                flusso = "ORDINANZE"
        End Select
        Return flusso
    End Function


    Public Function GetMd5Hash(ByVal md5Hash As MD5, ByVal input As String) As String

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()
    End Function


    Public Function GenerateHashTokenCallSic() As String
        Dim g as Guid = Guid.NewGuid()
        Return g.ToString()
    End Function
            
End Module
