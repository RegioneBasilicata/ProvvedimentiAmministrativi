Imports DllDocumentale
Imports System.Xml

Public Class Griglia
    Public cssClasse As String
    Public cssClasseRigaDispari As String
    Private m_tabella As Web.UI.WebControls.Table
    Private m_vettore As Object
    Private m_trasposta As Boolean = False
    Private m_vetDatiIntestazione As Array = Nothing
    Private m_vetDatiNonVisibili As Array = Nothing
    Private m_tastoDettaglio As Boolean = False
    Private m_vetAzioni As Array = Nothing
    Private m_controlloColonna As Array = Nothing
    Private m_idcontrolloColonna As Array = Nothing
    Private m_paginazione As Integer = 10
    Private m_paginaInizio As Integer
    Private m_paginaFine As Integer
    Private m_paginaCorrente As Integer = 0
    Dim i As Integer = 0
    Private m_ordina As Boolean = False
    Private m_indiceOrdinamento As Integer = -1
    Private _flagPaginazione As Boolean = True

    Property Tabella() As Table
        Get
            Return m_tabella
        End Get
        Set(ByVal Value As Table)
            Me.m_tabella = Value
        End Set
    End Property
    Property Vettore() As Object
        Get
            Return m_vettore
        End Get
        Set(ByVal Value As Object)
            Me.m_vettore = Value
        End Set
    End Property
    Property Trasposta() As Boolean
        Get
            Return m_trasposta
        End Get
        Set(ByVal Value As Boolean)
            Me.m_trasposta = Value
        End Set
    End Property
    Property VetDatiNonVisibili() As Array
        Get
            If Not m_vettore Is Nothing AndAlso Not m_vetDatiNonVisibili Is Nothing AndAlso UBound(m_vetDatiNonVisibili) < UBound(m_vettore, 1) Then
                Dim appArray As Array
                appArray = Array.CreateInstance(GetType(String), UBound(m_vettore, 1) + 1)
                For i = 0 To UBound(m_vettore, 1)
                    If i <= UBound(m_vetDatiNonVisibili) Then
                        appArray(i) = m_vetDatiNonVisibili(i)
                    Else
                        appArray(i) = False
                    End If
                Next
                m_vetDatiNonVisibili = appArray
            End If
            Return m_vetDatiNonVisibili
        End Get
        Set(ByVal Value As Array)
            Me.m_vetDatiNonVisibili = Value
        End Set
    End Property
    Property VetDatiIntestazione() As Array
        Get
            If Not m_vettore Is Nothing AndAlso Not m_vetDatiIntestazione Is Nothing AndAlso UBound(m_vetDatiIntestazione) < UBound(m_vettore, 1) Then
                Dim appArray As Array
                appArray = Array.CreateInstance(GetType(String), UBound(m_vettore, 1) + 1)
                For i = 0 To UBound(m_vettore, 1)
                    If i <= UBound(m_vetDatiIntestazione) Then
                        appArray(i) = m_vetDatiIntestazione(i)
                    Else
                        appArray(i) = ""
                    End If
                Next
                m_vetDatiIntestazione = appArray
            End If
            Return m_vetDatiIntestazione
        End Get
        Set(ByVal Value As Array)
            Me.m_vetDatiIntestazione = Value
        End Set
    End Property
    Property VetAzioni() As Array
        Get
            If Not m_vettore Is Nothing And UBound(m_vetAzioni) < UBound(m_vettore, 1) Then
                Dim appArray As Array
                appArray = Array.CreateInstance(GetType(String), UBound(m_vettore, 1) + 1)
                For i = 0 To UBound(m_vettore, 1)
                    If i <= UBound(m_vetAzioni) Then
                        appArray(i) = m_vetAzioni(i)
                    Else
                        appArray(i) = ""
                    End If
                Next
                m_vetAzioni = appArray
            End If
            Return m_vetAzioni
        End Get
        Set(ByVal Value As Array)
            Me.m_vetAzioni = Value
        End Set
    End Property
    Property TastoDettaglio() As Boolean
        Get
            Return m_tastoDettaglio
        End Get
        Set(ByVal Value As Boolean)
            Me.m_tastoDettaglio = Value
        End Set
    End Property
    Property ControlloColonna() As Array
        Get
            If Not m_vettore Is Nothing And UBound(m_controlloColonna) < UBound(m_vettore, 1) Then
                Dim appArray As Array
                appArray = Array.CreateInstance(GetType(String), UBound(m_vettore, 1) + 1)
                For i = 0 To UBound(m_vettore, 1)
                    If i <= UBound(m_controlloColonna) Then
                        appArray(i) = m_controlloColonna(i)
                    Else
                        appArray(i) = ""
                    End If
                Next
                m_controlloColonna = appArray
            End If
            Return m_controlloColonna
        End Get
        Set(ByVal Value As Array)
            Me.m_controlloColonna = Value
        End Set
    End Property
    Property idControlloColonna() As Array
        Get
            If Not m_vettore Is Nothing And UBound(m_idcontrolloColonna) < UBound(m_vettore, 1) Then
                Dim appArray As Array
                appArray = Array.CreateInstance(GetType(String), UBound(m_vettore, 1) + 1)
                For i = 0 To UBound(m_vettore, 1)
                    If i <= UBound(m_idcontrolloColonna) Then
                        appArray(i) = m_idcontrolloColonna(i)
                    Else
                        appArray(i) = ""
                    End If
                Next
                m_idcontrolloColonna = appArray
            End If
            Return m_idcontrolloColonna
        End Get
        Set(ByVal Value As Array)
            Me.m_idcontrolloColonna = Value
        End Set
    End Property
    Property Paginazione() As Integer
        Get
            Return m_paginazione
        End Get
        Set(ByVal Value As Integer)
            Me.m_paginazione = Value
        End Set
    End Property
    Property PaginaInizio() As Integer
        Get
            Return m_paginaInizio
        End Get
        Set(ByVal Value As Integer)
            Me.m_paginaInizio = Value
        End Set
    End Property
    Property PaginaFine() As Integer
        Get
            Return m_paginaFine
        End Get
        Set(ByVal Value As Integer)
            Me.m_paginaFine = Value
        End Set
    End Property
    Property PaginaCorrente() As Integer
        Get
            Return m_paginaCorrente
        End Get
        Set(ByVal Value As Integer)
            Me.m_paginaCorrente = Value
        End Set
    End Property
    Property FlagPaginazione() As Boolean
        Get
            Return _flagPaginazione
        End Get
        Set(ByVal Value As Boolean)
            Me._flagPaginazione = Value
        End Set
    End Property
    Property Ordina() As Boolean
        Get
            Return m_ordina
        End Get
        Set(ByVal Value As Boolean)
            Me.m_ordina = Value
        End Set
    End Property
    Property IndiceOrdinamento() As Integer
        Get
            Return m_indiceOrdinamento
        End Get
        Set(ByVal Value As Integer)
            Me.m_indiceOrdinamento = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub crea_tabella_daVettore()
        Dim cellaCorrente As Web.UI.WebControls.TableCell
        Dim rigaCorrente As Web.UI.WebControls.TableRow
        Dim rigaPaginazione As New TableRow
        Dim cellaPaginazione As New TableCell
        Dim r As Integer
        Dim c As Integer

        Dim rTh As New TableRow

        If cssClasse = "" Then
            cssClasse = "griglia"
        End If
        If cssClasseRigaDispari = "" Then
            cssClasseRigaDispari = "grigliaRigaDispari"
        End If
        'aggiunge la riga di intestazione alla tabella
        If Not VetDatiIntestazione Is Nothing Then
            For r = 0 To VetDatiIntestazione.Length - 1
                Dim cTh As New TableCell
                cTh.CssClass() = "h" + cssClasse
                If Ordina And CStr(VetDatiIntestazione(r)) <> "&nbsp;" Then
                    If CStr(VetDatiIntestazione(r)).StartsWith("CHECK&") Then
                        Dim lstr_idCheck As String = CStr(VetDatiIntestazione(r)).Replace("CHECK&", "")
                        cTh.Controls.Add(New LiteralControl("<input class=""check PrintClass"" id=""" & "ALL_" & lstr_idCheck & """ name=""" & "ALL_" & lstr_idCheck & """ type=""checkbox""  onclick=""javascript:SelezionaTutti('" & lstr_idCheck & "')"">"))

                    Else
                        Dim stringa As String = "<a href=""OrdinaAction.aspx?indice=" & r & """ class=""hgriglia""> "
                        cTh.Controls.Add(New LiteralControl(stringa & CStr(VetDatiIntestazione(r)) & "</a>"))
                    End If
                    'Dim stringa As String = "<a href=""OrdinaAction.aspx?indice=" & r & """ class=""hgriglia""> "
                    'cTh.Controls.Add(New LiteralControl(stringa & CStr(VetDatiIntestazione(r)) & "</a>"))
                Else
                    If CStr(VetDatiIntestazione(r)).StartsWith("CHECK&") Then
                        Dim lstr_idCheck As String = CStr(VetDatiIntestazione(r)).Replace("CHECK&", "")
                        cTh.Controls.Add(New LiteralControl("<input class=""check PrintClass"" id=""" & "ALL_" & lstr_idCheck & """ name=""" & "ALL_" & lstr_idCheck & """ type=""checkbox""  onclick=""javascript:SelezionaTutti('" & lstr_idCheck & "')"">"))
                    Else
                        cTh.Controls.Add(New LiteralControl(CStr(VetDatiIntestazione(r))))
                    End If
                    End If

                    If Not VetDatiNonVisibili Is Nothing Then
                        If VetDatiNonVisibili(r) Then
                            rTh.Cells.Add(cTh)
                        End If
                    Else
                        rTh.Cells.Add(cTh)
                    End If
            Next
            rTh.CssClass() = "thTable"
            Tabella.Rows.Add(rTh)
        End If
        
        Dim dimRigaVettore As Integer
        Dim dimColonnaVettore As Integer

        If Not Trasposta Then
            dimRigaVettore = 1
            dimColonnaVettore = 2
        Else
            dimRigaVettore = 2
            dimColonnaVettore = 1
        End If

        If Ordina And IndiceOrdinamento >= 0 Then
            ordina_Vettore(Vettore, IndiceOrdinamento, dimRigaVettore, dimColonnaVettore)
        End If

        calcola_Paginazione(dimRigaVettore)

        For r = PaginaInizio To PaginaFine
            rigaCorrente = New Web.UI.WebControls.TableRow
            'Dim numeroDef As String
            Dim isAnnullato As Boolean = False
            For c = 0 To UBound(Vettore, dimColonnaVettore)
                cellaCorrente = New Web.UI.WebControls.TableCell
                Dim confronto As String = m_controlloColonna(c) & ""
                Dim valueCella As String

                If Not Trasposta Then
                    valueCella = IIf(Trim(Vettore(r, c) & "") <> "", Vettore(r, c), "&nbsp")

                    If UBound(Vettore, dimColonnaVettore) >= 2 Then
                        If Vettore(r, 2).StartsWith("<ANN>") Then
                            isAnnullato = True
                            If c = 2 Then
                                valueCella = valueCella.Replace("<ANN>", "")
                            End If
                        Else
                            isAnnullato = False
                        End If
                        
                    End If
                    
                    carica_cella(rigaCorrente, cellaCorrente, c, confronto, valueCella, m_idcontrolloColonna(c) & "", Vettore(r, 0), isAnnullato)
                Else
                    If UBound(Vettore, dimColonnaVettore) >= 2 Then
                        If Vettore(2, r).StartsWith("<ANN>") Then
                            isAnnullato = True
                        Else
                            isAnnullato = False
                        End If
                    End If


                    valueCella = IIf(Trim(Vettore(c, r) & "") <> "", Vettore(c, r), "&nbsp")
                    If c = 2 Then
                        valueCella = valueCella.Replace("<ANN>", "")
                    End If
                    carica_cella(rigaCorrente, cellaCorrente, c, confronto, valueCella, m_idcontrolloColonna(c) & "", Vettore(0, r), isAnnullato)
                    End If

                    If Not VetDatiNonVisibili Is Nothing Then
                        If VetDatiNonVisibili(c) Then
                            rigaCorrente.Controls.Add(cellaCorrente)
                        End If
                    Else
                        rigaCorrente.Controls.Add(cellaCorrente)
                    End If
                    If r Mod 2 = 0 Then
                        cellaCorrente.CssClass = cssClasse
                    Else
                        cellaCorrente.CssClass = cssClasseRigaDispari
                    End If
            Next
            'If Not Trasposta Then
            '    Vettore(r, 2) = Vettore(r, 2).Replace("ANN", "")
            'Else
            '    Vettore(2, r) = Vettore(2, r).Replace("ANN", "")
            'End If
            Tabella.Controls.Add(rigaCorrente)
        Next
        If FlagPaginazione = True Then
            If Not Paginazione >= UBound(Vettore, dimRigaVettore) Then
                For i = 1 To Math.Ceiling((UBound(Vettore, dimRigaVettore) / Paginazione))
                    cellaPaginazione.Controls.Add(New LiteralControl("<a href=""PaginazioneAction.aspx?pag=" & i & """ > " & i & "</a>"))
                    cellaPaginazione.HorizontalAlign = HorizontalAlign.Center
                    ' If Trasposta Then
                    'cellaPaginazione.ColumnSpan = UBound(Vettore, dimRigaVettore)
                    ' Else
                    cellaPaginazione.ColumnSpan = UBound(Vettore, dimColonnaVettore)
                    ' End If
                    'cellaPaginazione.ColumnSpan = UBound(Vettore, dimColonnaVettore)
                    rigaPaginazione.Controls.Add(cellaPaginazione)
                Next
                cellaPaginazione.Controls.Add(New LiteralControl("<a href=""PaginazioneAction.aspx?pag=0"" >&nbsp;&nbsp;Mostra tutti</a>"))
                rigaPaginazione.Controls.Add(cellaPaginazione)
                cellaPaginazione.CssClass = "cellaPaginazione"
                Tabella.Controls.Add(rigaPaginazione)
            End If
        End If

        If cssClasse <> "" Then
            Tabella.CssClass = cssClasse
        End If

    End Sub

    Private Sub carica_cella(ByRef riga As Web.UI.WebControls.TableRow, ByRef cella As Web.UI.WebControls.TableCell, ByVal colonna As Integer, ByVal tipo As String, ByVal valore As String, ByVal idControllo As String, ByVal valorePrimaColonna As String, Optional ByVal isAnnullato As Boolean = False)
        'modgg21
        Select Case UCase(tipo)
            Case "TESTO"
                If Not valore Is Nothing AndAlso Trim(valore) <> "&nbsp" Then
                    valore = System.Web.HttpUtility.HtmlEncode(valore)
                End If
                If isAnnullato Then
                    valore = "<strike>" & valore & "</strike>"
                End If
                cella.Text = valore
            Case "IMAGE"
                cella.Style("text-align") = "center"
                If valore <> "&nbsp" Then
                    If idControllo <> "" Then
                        cella.Controls.Add(New LiteralControl("<img style='text-align:center' src=" & valore & " class=""cssIcona"" id=""icona"" />"))
                    Else
                        cella.Controls.Add(New LiteralControl("<img src=" & valore & " class=""cssIcona"" id=""icona"" />"))
                    End If
                Else
                    cella.Text = valore
                End If
            Case "CHECK"
                cella.Controls.Add(New LiteralControl("<input class=""check PrintClass"" id=""" & idControllo & """ name=""" & idControllo & """ type=""checkbox""  value=""" & valore & """>"))
            Case "CHECKREADONLY"
                cella.Controls.Add(New LiteralControl("<input DISABLED class=""check"" id=""" & idControllo & """ name=""" & idControllo & """ type=""checkbox"" " & IIf(CBool(valore), "CHECKED", "") & "  value=""" & valore & """>"))
            Case "SUBSTRING"
                If valore.Length > CInt(idControllo) Then
                    Dim subValore = valore.Substring(0, CInt(idControllo))

                    If Not valore Is Nothing AndAlso Trim(valore) <> "&nbsp" Then
                        valore = System.Web.HttpUtility.HtmlEncode(valore)
                        subValore = System.Web.HttpUtility.HtmlEncode(subValore)
                    End If

                    cella.Text = subValore & "..."
                    cella.ToolTip = valore
                Else
                    If Not valore Is Nothing AndAlso Trim(valore) <> "&nbsp" Then
                        valore = System.Web.HttpUtility.HtmlEncode(valore)
                    End If

                    cella.Text = valore
                End If
            Case "DATA"
                Dim text As String = ""
                If IsDate(valore) Then
                    text = Format(CDate(valore), "dd/MM/yy")
                    'cella.Text = Format(CDate(valore), "dd/MM/yy")
                Else
                    text = valore
                End If
                If isAnnullato Then
                    text = "<strike>" & text & "</strike>"
                End If
                cella.Text = text
            Case "OPTION"
                cella.Controls.Add(New LiteralControl("<input class =""optio PrintClass"" id=""" & idControllo & """  name=""" & idControllo & """  type=""radio""  value=""" & valore & """>"))
            Case "LINK"
                If TastoDettaglio = True Then
                    If Trim(Vettore(vr_Elenco_Worklist.c_idDocumento, 0)) <> "&nbsp" Then
                        If isAnnullato Then
                            cella.Text = "<p style='color:red'>ANNULLATO</p>"
                        Else
                            cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valorePrimaColonna & ">" & IIf(idControllo = "", valore, idControllo) & "</a>"))
                        End If

                    Else
                        cella.Text = valore
                    End If
                Else
                    If Trim(valore) <> "&nbsp" Then
                        If isAnnullato Then
                            cella.Text = "<p style='color:red'>ANNULLATO</p>"
                        Else
                            cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valore & ">" & idControllo & "</a>"))
                        End If

                    Else
                        cella.Text = valore
                    End If
                End If

                riga.Controls.AddAt(0, cella)
            Case "POPUP"
                If TastoDettaglio = True Then
                    If Trim(Vettore(vr_Elenco_Worklist.c_idDocumento, 0)) <> "&nbsp" Then
                        cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valorePrimaColonna & "  target=""_blank"">" & IIf(idControllo = "", valore, idControllo) & "</a>"))
                    Else
                        cella.Text = valore
                    End If
                Else
                    If Trim(valore) <> "&nbsp" Then
                        cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valore & "  target=""_blank"">" & IIf(idControllo = "", valore, idControllo) & "</a>"))
                    Else
                        cella.Text = valore
                    End If
                End If
                riga.Controls.AddAt(0, cella)
            Case "POPUP_IMAGE"
                cella.Style("text-align") = "center"
                If TastoDettaglio = True Then
                    If Trim(Vettore(vr_Elenco_Worklist.c_idDocumento, 0)) <> "&nbsp" Then
                        cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valorePrimaColonna & " > <img src=" & valore & " class=""cssIconaIMG"" id=""icona"" /> </a>"))
                    Else
                        cella.Text = valore
                    End If
                Else
                    If Trim(valore) <> "&nbsp" Then
                        cella.Controls.Add(New LiteralControl("<a href=" & m_vetAzioni(colonna) & "?key=" & valore & "><img src=" & valore & " class=""cssIconaIMG"" id=""icona"" /></a>"))
                    Else
                        cella.Text = valore
                    End If
                End If
                riga.Controls.AddAt(0, cella)
            Case Else
                If Not valore Is Nothing AndAlso Trim(valore) <> "&nbsp" Then
                    valore = System.Web.HttpUtility.HtmlEncode(valore)
                End If

                cella.Text = valore
        End Select
    End Sub
    Public Sub calcola_Paginazione(ByVal dimRigaVettore As Integer)
        'modgg 13-02sera
        If Not FlagPaginazione Then
            PaginaInizio = 0
            PaginaFine = UBound(Vettore, dimRigaVettore)
            Exit Sub
        End If
        Select Case PaginaCorrente
            Case 0
                PaginaInizio = 0
                PaginaFine = UBound(Vettore, dimRigaVettore)
                Exit Sub
            Case 1
                PaginaInizio = 0
            Case Else
                PaginaInizio = Paginazione * (PaginaCorrente - 1)
        End Select

        If Paginazione >= UBound(Vettore, dimRigaVettore) Then
            PaginaFine = UBound(Vettore, dimRigaVettore)
        End If

        If (UBound(Vettore, dimRigaVettore) - PaginaInizio) <= Paginazione Then
            PaginaFine = UBound(Vettore, dimRigaVettore)
        Else
            PaginaFine = PaginaInizio + Paginazione - 1
        End If
    End Sub
    Private Sub ordina_Vettore(ByRef vettore As Array, ByVal indiceOrdinamento As Integer, ByVal dimRigaVettore As Integer, ByVal dimColonnaVettore As Integer)
        Dim tipo As String = "STRING"

        If UBound(vettore, dimRigaVettore) < 1 Then
            Exit Sub
        End If
        vettore = ordinare(vettore, indiceOrdinamento)
    End Sub
    Private Function ordinare(ByVal arrayInput(,) As Object, ByVal indiceOrdinamento As Integer) As Object(,)
        Dim listaRighe As New ArrayList

        Dim stringaRiga As String = ""
        Dim primoIndice As Integer
        Dim secondoIndice As Integer
        Dim riga As rigaArray
        For primoIndice = 0 To UBound(arrayInput, 2)
            For secondoIndice = 0 To UBound(arrayInput, 1)
                stringaRiga &= arrayInput(secondoIndice, primoIndice)
                If Not secondoIndice = UBound(arrayInput, 1) Then
                    stringaRiga &= "#intema#"
                End If
            Next
            riga = New rigaArray(stringaRiga)
            listaRighe.Add(riga)

            stringaRiga = ""
        Next

        Dim arrayTemp(listaRighe.Count - 1, 1) As Object
        Dim indiceArrayTemp As Integer
        For indiceArrayTemp = 0 To listaRighe.Count - 1
            ' If (VetDatiNonVisibili(indiceArrayTemp)) Then
            riga = listaRighe.Item(indiceArrayTemp)
            Dim campoDaOrdinare As Object = (riga.getListaElementi).Item(indiceOrdinamento)

            arrayTemp(indiceArrayTemp, 0) = campoDaOrdinare
            arrayTemp(indiceArrayTemp, 1) = riga
            'End If
        Next

        'gestire l'ordinamento
        Dim campo1 As Object
        Dim campo2 As Object
        Dim riga1 As Object
        Dim riga2 As Object
        Dim arraySwap(0, 1) As Object

        For primoIndice = 0 To UBound(arrayTemp, 1) - 1
            For secondoIndice = (primoIndice + 1) To UBound(arrayTemp, 1)
                campo1 = arrayTemp(primoIndice, 0)
                campo2 = arrayTemp(secondoIndice, 0)
                Try
                    If IsDate(arrayTemp(primoIndice, 0)) Then
                        campo1 = CDate(arrayTemp(primoIndice, 0))
                        campo2 = CDate(arrayTemp(secondoIndice, 0))
                    Else
                        If IsNumeric(arrayTemp(primoIndice, 0)) Then
                            campo1 = CInt(arrayTemp(primoIndice, 0))
                            campo2 = CInt(arrayTemp(secondoIndice, 0))
                        End If
                    End If
                Catch ex As Exception
                    'Se viene scatenata, l'ordinamento non avviene
                    Return arrayInput
                End Try
                If campo2 > campo1 Then
                    riga1 = arrayTemp(primoIndice, 1)
                    riga2 = arrayTemp(secondoIndice, 1)

                    arraySwap(0, 0) = campo1
                    arraySwap(0, 1) = riga1

                    arrayTemp(primoIndice, 0) = campo2
                    arrayTemp(primoIndice, 1) = riga2
                    arrayTemp(secondoIndice, 0) = arraySwap(0, 0)
                    arrayTemp(secondoIndice, 1) = arraySwap(0, 1)
                End If
            Next
        Next

        'a questo punto arrayTemp è ordinato
        Dim arrayReturn(UBound(arrayInput, 1), UBound(arrayInput, 2)) As Object
        Dim indice As Integer
        For indice = 0 To UBound(arrayTemp, 1)
            riga = arrayTemp(indice, 1)
            Dim listaElementi As ArrayList = riga.getListaElementi
            For secondoIndice = 0 To listaElementi.Count - 1
                arrayReturn(secondoIndice, indice) = listaElementi.Item(secondoIndice)
            Next
        Next

        Return arrayReturn
    End Function

End Class
Public Class rigaArray
    Private listaElementi As New ArrayList

    Public Sub New(ByVal rigaArray As String)
        Dim rigaSplittata() As String = Split(rigaArray, "#intema#")

        Dim indice As Integer
        For indice = 0 To rigaSplittata.Length - 1
            listaElementi.Add(rigaSplittata(indice))
        Next
    End Sub

    Public Function getListaElementi() As ArrayList
        Return listaElementi
    End Function

End Class
