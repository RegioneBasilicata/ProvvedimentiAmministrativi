Public Class svrDelibereRegione
    Inherits svrDelibereBase

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrDelibere" Then
            'chiamata ad una funzione dell'oggetto PassoDetermina
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub
    Overrides Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            Select Case cFunzione
                Case vFunzioniPassoDelibera.cf_DELIBERA_ANNULLO
                    Call DELIBERA_ANNULLO()
                Case vFunzioniPassoDelibera.cf_DELIBERA_ASSEGNA
                    Call DELIBERA_ASSEGNA()
                Case vFunzioniPassoDelibera.cf_DELIBERA_PRELAZIONE
                    Call DELIBERA_PRELAZIONE()
                Case vFunzioniPassoDelibera.cf_DELIBERA_DEPOSITO_PRELIEVO
                    Call DELIBERA_DEPOSITO_PRELIEVO()
                Case vFunzioniPassoDelibera.cf_DELIBERA_COLLABORATORE_INOLTRO
                    Call DELIBERA_COLLABORATORE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_DELIBERA_SUPERVISORE_INOLTRO
                    Call DELIBERA_SUPERVISORE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO
                    Call UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_INOLTRO
                    Call UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_INOLTRO
                    Call UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_PRESIDENZA_RESPONSABILE_INOLTRO
                    Call UFFICIO_PRESIDENZA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_INOLTRO
                    Call UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_POLITICO_RESPONSABILE_INOLTRO
                    Call UFFICIO_POLITICO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_SEGRETERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_RIGETTO
                    Call UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_RIGETTO
                    Call UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_RIGETTO
                    UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_POLITICO_RESPONSABILE_RIGETTO
                    Call UFFICIO_POLITICO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_SEGRETERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_SEGRETERIA_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_PRESIDENZA_RESPONSABILE_RIGETTO
                    Call UFFICIO_PRESIDENZA_RESPONSABILE_RIGETTO()
            End Select
        Catch ex As Exception
            Errore = Err.Number
            ErrDesc = Err.Description
        End Try
    End Sub
    Private Sub DELIBERA_ANNULLO()
        If oOperatore.Test_Ruolo("DL005") Then
            pDirD = "StessoUfficio"
            pDirU = "A"
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può annullare"
        End If
    End Sub
    Private Sub DELIBERA_ASSEGNA()
        If oOperatore.Test_Ruolo("GL002") Then
            pDirD = "StessoUfficio"
            pDirU = "C"
            pProssimoAttore = pProssimoAttore
        Else
            Errore = 1
            ErrDesc = "L'operatore non può assegnare"
        End If
    End Sub
    Private Sub DELIBERA_PRELAZIONE()
        If oOperatore.Test_Ruolo("GL001") Then
            pDirD = "StessoUfficio"
            pDirU = "S"
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                pDirU = "S"
            End If
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
                pDirU = "R"
            End If
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può prelazionare"
        End If
    End Sub
    Private Sub DELIBERA_DEPOSITO_PRELIEVO()
        If oOperatore.Test_Ruolo("DL015") Then
            pDirD = "StessoUfficio"
            pDirU = ""

            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Collaboratore") Then
                pDirU = "C"
            End If
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                pDirU = "S"
            End If
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
                pDirU = "R"
            End If
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può prelevare"
        End If
    End Sub
    Private Sub DELIBERA_COLLABORATORE_INOLTRO()
        Dim vR As Object = Nothing
        Dim supUff As String
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""

        'Mod salto  supervisore e vado in responsabile

        If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DELIBERE") Then
            If Not String.IsNullOrEmpty(pProssimoAttore) Then
                pDirU = "R"
                Exit Sub
            End If
        End If

        If pProssimoAttore <> "" Then
            pDirU = "S"
            'SOLO PER TESTARE LA FUNZIONE
            ''update per Atti e delibere web
            'Call UPDATE_PER_ADW(idDocumento)
            Exit Sub
        End If

        'il successivo attore è il supervisore se l'utente ha un superviziore di default
        supUff = oOperatore.Attributo("SUPERVISORE_DEFAULT")
        If supUff <> "" Then
            pDirU = "S"
            pProssimoAttore = supUff
            Exit Sub
        End If

        'il successivo attore è il supervisore se l'ufficio ha un supervisore
        vR = oOperatore.oUfficio.SupervisoriUfficio("DELIBERE")

        If Not vR Is Nothing Then

            Dim en As IDictionaryEnumerator = vR.GetEnumerator
            While (en.MoveNext())
                supUff = DirectCast(en.Key, String)
                Exit While
            End While

            'supUff = vR.Item(DirectCast(en.Current(), String))
            If supUff <> "" Then
                pDirU = "S"
                pProssimoAttore = supUff
                Exit Sub
            End If
        End If

        respUff = oOperatore.oUfficio.ResponsabileUfficio("DELIBERE")
        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
        MyBase.Passo_Possibile()
    End Sub
    Private Sub DELIBERA_SUPERVISORE_INOLTRO()
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        pProssimoAttore = ""

        respUff = oOperatore.oUfficio.ResponsabileUfficio("DELIBERE")
        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
        Dim oUffDirDip As New DllAmbiente.Ufficio

        'scelta direzione

        pDirD = "UDD"
        oUffDirDip.CodUfficio = oOperatore.oUfficio.CodUfficioDirigenzaDipartimento

        Call INOLTRO_ALTRO_UFFICIO(oUffDirDip)

        oUffDirDip = Nothing
        MyBase.Passo_Possibile()




        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio
        Dim vett_Rit_UltimaOperazione As Object

        Dim ultimaOperazione As String
        Dim destinatario As Integer = destinInoltro
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioDirigenzaDipartimento)
        If vett_Rit_UltimaOperazione(0) = 0 Then
            'controllo l'ultima azione del dirigente generale
            ultimaOperazione = vett_Rit_UltimaOperazione(1)
            If ultimaOperazione = "INOLTRO" Then
                'il dirigente generale ha firmato
                Dim oUffDirigenzaDip As New DllAmbiente.Ufficio
                oUffDirigenzaDip.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
                Dim responsabile As String = oUffDirigenzaDip.ResponsabileUfficio("DELIBERE")
                'verifico firma del dirigente su questa versione del documento
                If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabile) > 0) Then

                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita
                    pDirD = "USL"
                    GoTo INOLTRO_ALTRO_UFFICIO

                End If
                'ci entro nel caso in cui il dirigente non ha firmato                 
            ElseIf ultimaOperazione = Nothing Then
                oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
                pDirD = "UDD"
                GoTo INOLTRO_ALTRO_UFFICIO

            End If
        End If


        If (destinInoltro = 1) Then
            'oUffDestinazione.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
            'pDirD = "UCA"
        Else
            oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
            pDirD = "UDD"
        End If
        GoTo INOLTRO_ALTRO_UFFICIO

INOLTRO_ALTRO_UFFICIO:
        Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
        oUffDestinazione = Nothing
        oUffProponente = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()

        Dim oUffSegrAss As New DllAmbiente.Ufficio

        'Scelta Segreteria assessorato
        'Se si tratta di un provvedimento creato un dip presente in Struttura_Attributi con Sta_Attributo="INOLTRO_SEGRETERIA_ASS" lo inotro alla
        ' segreteria particolare presidente 11A1 altrimenti alla propria segreteria di assessorato del dipartimento
        pDirD = "US"

        If Not oOperatore.oUfficio.CodUfficioSegreteriaAssessorato.Equals("") Then
            oUffSegrAss.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteriaAssessorato
        Else
            oUffSegrAss.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteria
        End If

        Call INOLTRO_ALTRO_UFFICIO(oUffSegrAss)

        oUffSegrAss = Nothing
        MyBase.Passo_Possibile()
        

'        Dim oUffDestinazione As New DllAmbiente.Ufficio
'        Dim oUffProponente As New DllAmbiente.Ufficio
'        Dim vett_Rit_UltimaOperazione As Object

'        Dim ultimaOperazione As String
'        Dim destinatario As Integer = destinInoltro
'        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

'        vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteria)
'        If vett_Rit_UltimaOperazione(0) = 0 Then
'            'controllo l'ultima azione del dirigente generale
'            ultimaOperazione = vett_Rit_UltimaOperazione(1)
'            If ultimaOperazione = "INOLTRO" Then
'                'il dirigente generale ha firmato
'                Dim oUffDirigenzaDip As New DllAmbiente.Ufficio
'                oUffDirigenzaDip.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'                Dim responsabile As String = oUffDirigenzaDip.ResponsabileUfficio("DELIBERE")
'                'verifico firma del dirigente su questa versione del documento
'                If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabile) > 0) Then

'                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita
'                    pDirD = "USL"
'                    GoTo INOLTRO_ALTRO_UFFICIO

'                End If
'                'ci entro nel caso in cui il dirigente non ha firmato                 
'            ElseIf ultimaOperazione = Nothing Then
'                oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'                pDirD = "UDD"
'                GoTo INOLTRO_ALTRO_UFFICIO

'            End If
'        End If


'        If (destinInoltro = 1) Then
'            'oUffDestinazione.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
'            'pDirD = "UCA"
'        Else
'            oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'            pDirD = "UDD"
'        End If
'        GoTo INOLTRO_ALTRO_UFFICIO

'INOLTRO_ALTRO_UFFICIO:
'        Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
'        oUffDestinazione = Nothing
'        oUffProponente = Nothing
'        MyBase.Passo_Possibile()

    End Sub
    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_INOLTRO()
        Dim vett_Rit_IsContabile As Object

        'recupero l'ufficio proponente
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        'verifico se ha impegno contabile
        vett_Rit_IsContabile = Is_Contabile()

        'verifica correttezza funzione is_Contabile
        If vett_Rit_IsContabile(0) = 0 Then

            ' se vett_Rit_IsContabile(1) è vera, controllo su ragioneria
            If vett_Rit_IsContabile(1)(0) Then
                'inoltro in ragioneria
                Dim oUffRag As New DllAmbiente.Ufficio
                oUffRag.CodUfficio = oUffProponente.CodUfficioRagioneria
                pDirD = "UR"
                Call INOLTRO_ALTRO_UFFICIO(oUffRag, "DELIBERE")
                oUffRag = Nothing
                MyBase.Passo_Possibile()
                Exit Sub
            Else
                'inoltro in segreteria di presidenza
                Dim oUffSegreteriaPresidSegretario As New DllAmbiente.Ufficio
                oUffSegreteriaPresidSegretario.CodUfficio = oUffProponente.CodUfficioSegreteriaPresidenzaSegretario
                pDirD = "USS"
                Call INOLTRO_ALTRO_UFFICIO(oUffSegreteriaPresidSegretario, "DELIBERE")
                oUffSegreteriaPresidSegretario = Nothing
                MyBase.Passo_Possibile()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_INOLTRO()
        Dim oUffPresidenza As New DllAmbiente.Ufficio

        'scelta direzione
        pDirD = "UPRES"
        oUffPresidenza.CodUfficio = oOperatore.oUfficio.CodUfficioPresidenza

        Call INOLTRO_ALTRO_UFFICIO(oUffPresidenza)

        oUffPresidenza = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_PRESIDENZA_RESPONSABILE_INOLTRO()
        'Dim oUffProponente As New DllAmbiente.Ufficio
        'oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        'pDirD = "UAR"
        'pDirU = "D"
        'pProssimoAttore = oUffProponente.CodArchivio
        'MyBase.Passo_Possibile()
        'Exit Sub
        Dim oUffSegrApprovazione As New DllAmbiente.Ufficio
        oUffSegrApprovazione.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteriaPresidenzaApprovazione
        pDirD = "USA"
        Call INOLTRO_ALTRO_UFFICIO(oUffSegrApprovazione)
        oUffSegrApprovazione = Nothing
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub

    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_INOLTRO()
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        pDirD = "UAR"
        pDirU = "D"
        pProssimoAttore = oUffProponente.CodArchivio
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub
    Private Sub UFFICIO_POLITICO_RESPONSABILE_INOLTRO()
        Dim oUffSegreteria As New DllAmbiente.Ufficio

        'scelta direzione
        pDirD = "US"
        oUffSegreteria.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteria

        Call INOLTRO_ALTRO_UFFICIO(oUffSegreteria)

        'modgg 12-06 2
        'update per Atti e delibere web
        'Call UPDATE_PER_ADW(idDocumento)

        oUffSegreteria = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_SEGRETERIA_RESPONSABILE_INOLTRO()
   
        If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DELIBERE") Then
            If Not String.IsNullOrEmpty(pProssimoAttore) Then
                pDirU = "R"
                pDirD = "StessoUfficio"
                Exit Sub
            End If
        Else
            Dim oUffSegrPres As New DllAmbiente.Ufficio
            pDirD = "USL"
            oUffSegrPres.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteriaPresidenzaLegittimita
            Call INOLTRO_ALTRO_UFFICIO(oUffSegrPres)
            oUffSegrPres = Nothing
        End If
        MyBase.Passo_Possibile()
        
'        Dim oUffDestinazione As New DllAmbiente.Ufficio
'        Dim oUffProponente As New DllAmbiente.Ufficio
'        Dim vett_Rit_UltimaOperazione As Object

'        Dim ultimaOperazione As String
'        Dim destinatario As Integer = destinInoltro
'        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

'        vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioSegreteria)
'        If vett_Rit_UltimaOperazione(0) = 0 Then
'            'controllo l'ultima azione del dirigente generale
'            ultimaOperazione = vett_Rit_UltimaOperazione(1)
'            If ultimaOperazione = "INOLTRO" Then
'                'il dirigente generale ha firmato
'                Dim oUffDirigenzaDip As New DllAmbiente.Ufficio
'                oUffDirigenzaDip.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'                Dim responsabile As String = oUffDirigenzaDip.ResponsabileUfficio("DELIBERE")
'                'verifico firma del dirigente su questa versione del documento
'                If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabile) > 0) Then

'                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioSegreteriaPresidenzaLegittimita
'                    pDirD = "USL"
'                    GoTo INOLTRO_ALTRO_UFFICIO

'                End If
'                'ci entro nel caso in cui il dirigente non ha firmato                 
'            ElseIf ultimaOperazione = Nothing Then
'                oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'                pDirD = "UDD"
'                GoTo INOLTRO_ALTRO_UFFICIO

'            End If
'        End If


'        If (destinInoltro = 1) Then
'            'oUffDestinazione.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
'            'pDirD = "UCA"
'        Else
'            oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
'            pDirD = "UDD"
'        End If
'        GoTo INOLTRO_ALTRO_UFFICIO

'INOLTRO_ALTRO_UFFICIO:
'        Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
'        oUffDestinazione = Nothing
'        oUffProponente = Nothing
'        MyBase.Passo_Possibile()


    End Sub
    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
        'inoltro in segreteria di presidenza
        Dim oUffSegreteriaPresidSegretario As New DllAmbiente.Ufficio
        oUffSegreteriaPresidSegretario.CodUfficio = oOperatore.oUfficio.CodUfficioSegreteriaPresidenzaSegretario
        pDirD = "USS"
        Call INOLTRO_ALTRO_UFFICIO(oUffSegreteriaPresidSegretario, "DELIBERE")
        oUffSegreteriaPresidSegretario = Nothing

        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio

        pDirD = "UP"
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_LEGITTIMITA_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio

        pDirD = "UP"
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_SEGRETARIO_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio

        pDirD = "UP"
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_SEGRETERIA_PRESIDENZA_APPROVAZIONE_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio

        pDirD = "UP"
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_POLITICO_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        'Dim oUffDirDip As New DllAmbiente.Ufficio

        pDirD = "UP"

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        'oUffDirDip = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        pDirD = "UP"

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_PRESIDENZA_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        pDirD = "UP"

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_SEGRETERIA_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        'Dim oUffDirDip As New DllAmbiente.Ufficio

        pDirD = "UP"

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        'oUffDirDip = Nothing
        MyBase.Passo_Possibile()
    End Sub
End Class
