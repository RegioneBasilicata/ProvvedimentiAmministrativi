Imports System.Configuration

Partial Public Class svrAltriAttiRegione
    Inherits svrAltriAttiBase

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrAltriAtti" Then
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub

    Overrides Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            Select Case cFunzione
                Case vFunzioniPassoAltriAtti.cf_COLLABORATORE_INOLTRO
                    Call DETERMINA_COLLABORATORE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_DEPOSITO_PRELIEVO
                    Call DETERMINA_DEPOSITO_PRELIEVO()
                Case vFunzioniPassoAltriAtti.cf_SUPERVISORE_INOLTRO
                    Call DETERMINA_SUPERVISORE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_PRELAZIONE
                    Call DETERMINA_PRELAZIONE()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO
                    Call UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO
                    Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoAltriAtti.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO
                    Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoAltriAtti.cf_DETERMINA_ANNULLO
                    Call DETERMINA_ANNULLO()
                Case vFunzioniPassoAltriAtti.cf_DETERMINA_ASSEGNA
                    Call DETERMINA_ASSEGNA()
            End Select
        Catch ex As Exception
            Errore = Err.Number
            ErrDesc = Err.Description
        End Try
    End Sub
    Private Sub DETERMINA_DEPOSITO_PRELIEVO()
        If oOperatore.Test_Ruolo("DT015") Then
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
    Private Sub DETERMINA_SUPERVISORE_INOLTRO()
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        pProssimoAttore = ""

        respUff = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE")
        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
    End Sub
    Private Sub UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
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
                Dim responsabile As String = oUffDirigenzaDip.ResponsabileUfficio("DETERMINE")
                'verifico firma del dirigente su questa versione del documento
                If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabile) > 0) Then
                    'forzo l'invio dei messaggi ai destinatari intermedi
                    Call MESSAGGIO_INOLTRO(7, oOperatore.pCodice, responsabile, )
                    'ha già firmato questa versione del documento, l'UP non si è adeguato quindi controllo UCA
                    Dim oUffControlloAmministrativo As New DllAmbiente.Ufficio
                    oUffControlloAmministrativo.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
                    Dim responsabileUCA As String = oUffControlloAmministrativo.ResponsabileUfficio("DETERMINE")
                    If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabileUCA) > 0) Then
                        'forzo l'invio dei messaggi ai destinatari intermedi
                        Call MESSAGGIO_INOLTRO(7, oOperatore.pCodice, responsabileUCA, )
                        'ha già firmato questa versione del documento, l'UP non si è adeguato quindi invio a UR
                        'MEV: tutte le det passano dalla ragioneria, eliminato il controllo sulle op contabili.
                        ' ** VECCHIO:
                        'If objDocumento.Doc_IsContabile = 1 Then
                        '    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioRagioneria
                        '    pDirD = "UR"
                        '    GoTo INOLTRO_ALTRO_UFFICIO
                        'Else
                        '    oUffDestinazione.CodUfficio = oUffProponente.CodArchivio
                        '    pDirD = "UAR"
                        '    Call MESSAGGIO_ARCHIVIO(6, , , )
                        '    GoTo INOLTRO_ALTRO_UFFICIO
                        'End If
                        ' ** NUOVO:
                        oUffDestinazione.CodUfficio = oUffProponente.CodUfficioRagioneria
                        pDirD = "UR"
                        GoTo INOLTRO_ALTRO_UFFICIO

                    Else
                        oUffDestinazione.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
                        pDirD = "UCA"
                        GoTo INOLTRO_ALTRO_UFFICIO
                    End If
                End If
                'ci entro nel caso in cui il dirigente non ha firmato 
                ' (questo caso si verifica sicuramente quando la Minardi sceglie di inoltrare direttamente
                ' all'ufficio del Controllo Amm o in Ragioneria)
            ElseIf ultimaOperazione = Nothing Then
                Dim oUffControlloAmministrativo As New DllAmbiente.Ufficio
                oUffControlloAmministrativo.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
                Dim responsabileUCA As String = oUffControlloAmministrativo.ResponsabileUfficio("DETERMINE")
                If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabileUCA) > 0) Then
                    'forzo l'invio dei messaggi ai destinatari intermedi
                    Call MESSAGGIO_INOLTRO(7, oOperatore.pCodice, responsabileUCA, )
                    'ha già firmato questa versione del documento, l'UP non si è adeguato quindi invio a UR
                    'ha già firmato questa versione del documento, l'UP non si è adeguato quindi invio a UR
                    'MEV: tutte le det passano dalla ragioneria, eliminato il controllo sulle op contabili.
                    ' ** VECCHIO:
                    'If objDocumento.Doc_IsContabile = 1 Then
                    '    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioRagioneria
                    '    pDirD = "UR"
                    '    GoTo INOLTRO_ALTRO_UFFICIO
                    'Else
                    '    oUffDestinazione.CodUfficio = oUffProponente.CodArchivio
                    '    pDirD = "UAR"
                    '    Call MESSAGGIO_ARCHIVIO(6, , , )
                    '    GoTo INOLTRO_ALTRO_UFFICIO
                    'End If
                    ' ** NUOVO:
                    'If objDocumento.Doc_IsContabile = 1 Then
                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioRagioneria
                    pDirD = "UR"
                    GoTo INOLTRO_ALTRO_UFFICIO
                    'Else
                    '    oUffDestinazione.CodUfficio = oUffProponente.CodArchivio
                    '    pDirD = "UAR"
                    '    Call MESSAGGIO_ARCHIVIO(6, , , )
                    '    GoTo INOLTRO_ALTRO_UFFICIO
                    'End If
                End If
            End If
        End If


        If (destinInoltro = 1) Then
            oUffDestinazione.CodUfficio = oUffProponente.CodUfficioControlloAmministrativo
            pDirD = "UCA"
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
    Private Sub DETERMINA_PRELAZIONE()
        If oOperatore.Test_Ruolo("GT001") Then
            pDirD = "StessoUfficio"
            pDirU = "S"
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                pDirU = "S"
            End If
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Responsabile") Then
                pDirU = "R"
            End If
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "Collaboratore") Then
                pDirU = "C"
            End If
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può prelazionare"
        End If
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffContrAmm As New DllAmbiente.Ufficio
        oUffContrAmm.CodUfficio = oOperatore.oUfficio.CodUfficioControlloAmministrativo
        Dim responsabileUCA As String = oUffContrAmm.ResponsabileUfficio("DETERMINE")
        If (VERIFICA_FIRMA_UTENTE(idDocumento, responsabileUCA) > 0) Then
            'forzo l'invio dei messaggi ai destinatari intermedi
            Call MESSAGGIO_INOLTRO(7, oOperatore.pCodice, responsabileUCA, )
            'ha già firmato questa versione del documento, l'UP non si è adeguato quindi invio a UR
            'MEV: tutte le det passano dalla ragioneria, eliminato il controllo sulle op contabili.
            ' ** VECCHIO:
            'If objDocumento.Doc_IsContabile = 1 Then
            '    oUffDestinazione.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria
            '    pDirD = "UR"
            '    Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
            'Else
            '    oUffDestinazione.CodUfficio = oOperatore.oUfficio.CodArchivio
            '    pDirD = "UAR"
            '    Call MESSAGGIO_ARCHIVIO(6, , , )
            '    Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
            'End If
            ' ** NUOVO:
            oUffDestinazione.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria
            pDirD = "UR"
            Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)

        Else
            pDirD = "UCA"
            oUffDestinazione.CodUfficio = oOperatore.oUfficio.CodUfficioControlloAmministrativo
            Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
        End If

        oUffContrAmm = Nothing
        oUffDestinazione = Nothing
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
    Private Sub DETERMINA_COLLABORATORE_INOLTRO()
        Dim vR As Hashtable
        Dim supUff As String
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        'Mod salto  supervisore e vado in responsabile
        If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE") Then
            pDirU = "R"
            Exit Sub
        End If

        If pProssimoAttore <> "" Then
            pDirU = "S"
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
        vR = oOperatore.oUfficio.SupervisoriUfficio("DETERMINE")

        If Not vR Is Nothing Then

            'Dim chiavi As Array = vR
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

        respUff = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE")
        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()

        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        pDirD = "UAR"
        pDirU = "D"
        pProssimoAttore = oUffProponente.CodArchivio
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub

    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()

        Dim oUffPr As New DllAmbiente.Ufficio
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        pDirD = "UP"

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")


        MyBase.Passo_Possibile()

        'forzo l'invio dei messaggi ai destinatari intermedi
        Dim uffDirDip As New DllAmbiente.Ufficio
        uffDirDip.CodUfficio = oUffPr.CodUfficioDirigenzaDipartimento
        Call MESSAGGIO_INOLTRO(8, oOperatore.pCodice, uffDirDip.ResponsabileUfficio("DETERMINE"), )

        Dim uffUCA As New DllAmbiente.Ufficio
        uffUCA.CodUfficio = oUffPr.CodUfficioControlloAmministrativo
        Call MESSAGGIO_INOLTRO(8, oOperatore.pCodice, uffUCA.ResponsabileUfficio("DETERMINE"), )

        oUffPr = Nothing
        uffDirDip = Nothing
        uffUCA = Nothing
    End Sub
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()

        Dim vett_Rit_IsContabile As Object

        'recupero l'ufficio proponente
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        'verifico se ha impegno contabile
        vett_Rit_IsContabile = Is_Contabile()

        'verifica correttezza funzione is_Contabile
        If vett_Rit_IsContabile(0) = 0 Then

            ' se vett_Rit_IsContabile(1) è vera, controllo su ragioneria
            'MEV: tutte le det passano dalla ragioneria, eliminato il controllo sulle op contabili.
            ' ** VECCHIO:
            'If vett_Rit_IsContabile(1)(0) Then
            '    'inioltro in ragioneria
            '    Dim oUffRag As New DllAmbiente.Ufficio
            '    oUffRag.CodUfficio = oUffProponente.CodUfficioRagioneria
            '    pDirD = "UR"
            '    Call INOLTRO_ALTRO_UFFICIO(oUffRag, "DETERMINE")
            '    oUffRag = Nothing
            '    MyBase.Passo_Possibile()
            '    Exit Sub
            'Else
            '    'archivio
            '    Call MESSAGGIO_ARCHIVIO(6, , , )
            '    pDirD = "UAR"
            '    pDirU = "D"
            '    pProssimoAttore = oUffProponente.CodArchivio
            '    Exit Sub
            'End If
            ' ** NUOVO:
            Dim oUffRag As New DllAmbiente.Ufficio
            oUffRag.CodUfficio = oUffProponente.CodUfficioRagioneria
            pDirD = "UR"
            Call INOLTRO_ALTRO_UFFICIO(oUffRag, "DETERMINE")
            oUffRag = Nothing
            MyBase.Passo_Possibile()
            Exit Sub

        End If

    End Sub
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        If oUffPr.bUfficioDirigenzaDipartimento Then
            pDirD = "UDD"
            Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")
        Else
            pDirD = "UP"
            Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")
            MyBase.Passo_Possibile()

            'forzo l'invio dei messaggi ai destinatari intermedi
            Dim uffDirDip As New DllAmbiente.Ufficio
            uffDirDip.CodUfficio = oUffPr.CodUfficioDirigenzaDipartimento
            Call MESSAGGIO_INOLTRO(8, oOperatore.pCodice, uffDirDip.ResponsabileUfficio("DETERMINE"), )
            uffDirDip = Nothing
        End If

        oUffPr = Nothing
    End Sub
    Private Sub DETERMINA_ASSEGNA()
        If oOperatore.Test_Ruolo("GT002") Then
            pDirD = "StessoUfficio"
            Dim destinatario As New DllAmbiente.Operatore
            destinatario.Codice = prossimoAttore
            If destinatario.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                pDirU = "S"
            End If
            If destinatario.Test_Attributo("LIVELLO_UFFICIO", "Collaboratore") Then
                pDirU = "C"
            End If
            destinatario = Nothing
        Else
            Errore = 1
            ErrDesc = "L'operatore non può assegnare"
        End If
    End Sub
    Private Sub DETERMINA_ANNULLO()
        If oOperatore.Test_Ruolo("DT005") Then
            pDirD = "StessoUfficio"
            pDirU = "A"
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può annullare"
        End If
    End Sub
End Class
