Public Class svrDisposizioniRegione
    Inherits svrDisposizioniBase

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub

    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrDisposizioni" Then
            'chiamata ad una funzione dell'oggetto PassoDetermina
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub

    Overrides Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            Select Case cFunzione
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_ANNULLO
                    Call DISPOSIZIONE_ANNULLO()
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_ASSEGNA
                    Call DISPOSIZIONE_ASSEGNA()
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_PRELAZIONE
                    Call DISPOSIZIONE_PRELAZIONE()
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_DEPOSITO_PRELIEVO
                    Call DISPOSIZIONE_DEPOSITO_PRELIEVO()
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_COLLABORATORE_INOLTRO
                    Call DISPOSIZIONE_COLLABORATORE_INOLTRO()
                Case vFunzioniPassoDisposizione.cf_DISPOSIZIONE_SUPERVISORE_INOLTRO
                    Call DISPOSIZIONE_SUPERVISORE_INOLTRO()
                Case vFunzioniPassoDisposizione.cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO
                    Call UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDisposizione.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDisposizione.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDisposizione.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDisposizione.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()

            End Select
        Catch ex As Exception
            Errore = Err.Number
            ErrDesc = Err.Description
        End Try
    End Sub
    Private Sub DISPOSIZIONE_SUPERVISORE_INOLTRO()
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        pProssimoAttore = ""
        respUff = oOperatore.oUfficio.ResponsabileUfficio("DISPOSIZIONE")

        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
    End Sub
    Private Sub DISPOSIZIONE_COLLABORATORE_INOLTRO()
        Dim vR As Object = Nothing
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
        vR = oOperatore.oUfficio.SupervisoriUfficio("DISPOSIZIONI")

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

        respUff = oOperatore.oUfficio.ResponsabileUfficio("DISPOSIZIONI")
        If respUff <> "" Then
            pDirU = "R"
            pProssimoAttore = respUff
        End If
        MyBase.Passo_Possibile()
    End Sub
    Private Sub DISPOSIZIONE_ANNULLO()
        If oOperatore.Test_Ruolo("DS016") Then
            pDirD = "StessoUfficio"
            pDirU = "A"
            pProssimoAttore = oOperatore.Codice
        Else
            Errore = 1
            ErrDesc = "L'operatore non può annullare"
        End If
    End Sub
    Private Sub DISPOSIZIONE_PRELAZIONE()
        If oOperatore.Test_Ruolo("GS011") Then
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
    Private Sub DISPOSIZIONE_DEPOSITO_PRELIEVO()
        If oOperatore.Test_Ruolo("GS015") Then
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
    Private Sub DISPOSIZIONE_ASSEGNA()
        If oOperatore.Test_Ruolo("GS012") Then
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
    Private Sub UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()

        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio
        Dim vett_Rit_UltimaOperazione As Object
        Dim ultimaOperazione As String

        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO(oUffProponente.CodUfficioDirigenzaDipartimento)
        If vett_Rit_UltimaOperazione(0) = 0 Then
            'controllo l'ultima azione del dirigente generale
            ultimaOperazione = vett_Rit_UltimaOperazione(1)
            If ultimaOperazione = "RIGETTO" Or ultimaOperazione = "RIGETTO FORMALE" Or ultimaOperazione = "" Then
                If (destinInoltro = 2) Then
                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioRagioneria
                    pDirD = "UR"
                    GoTo INOLTRO_ALTRO_UFFICIO
                Else
                    oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
                    pDirD = "UDD"
                    GoTo INOLTRO_ALTRO_UFFICIO
                End If
            End If
            End If
            'controllo ultima azione della ragioneria
            vett_Rit_UltimaOperazione = VERIFICA_AZIONE_UFFICIO()
            If vett_Rit_UltimaOperazione(0) = 0 Then
                'controllo l'ultima azione della ragioneria
                ultimaOperazione = vett_Rit_UltimaOperazione(1)
                If ultimaOperazione = "RIGETTO" Or ultimaOperazione = "RIGETTO FORMALE" Then
                    'non è possibile inoltrare se la ragioneria ha rigettato
                    Exit Sub
                End If
            End If

INOLTRO_ALTRO_UFFICIO:
            Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
            oUffDestinazione = Nothing
            oUffProponente = Nothing
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
        '      Dim oUffDirDip As New DllAmbiente.Ufficio

        If oUffPr.bUfficioDirigenzaDipartimento Then
            pDirD = "UDD"
        Else
            pDirD = "UP"
        End If

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        Dim uffDirDip As New DllAmbiente.Ufficio
        uffDirDip.CodUfficio = oUffPr.CodUfficioDirigenzaDipartimento
        Call MESSAGGIO_INOLTRO(8, oOperatore.pCodice, uffDirDip.ResponsabileUfficio("DETERMINE"), )

        oUffPr = Nothing
        uffDirDip = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
        Dim oUffRag As New DllAmbiente.Ufficio

        oUffRag.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria
        pDirD = "UR"

        Call INOLTRO_ALTRO_UFFICIO(oUffRag, "DISPOSIZIONI")

        oUffRag = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        'Dim oUffDirDip As New DllAmbiente.Ufficio

        pDirD = "UP"

        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")

        oUffPr = Nothing
        'oUffDirDip = Nothing
        MyBase.Passo_Possibile()
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
        Dim oUffRag As DllAmbiente.Ufficio

        oUffRag = New DllAmbiente.Ufficio
        pDirD = "UR"
        oUffRag.CodUfficio = oOperatore.oUfficio.CodUfficioRagioneria
        Call INOLTRO_ALTRO_UFFICIO(oUffRag, "DISPOSIZIONI")

        oUffRag = Nothing

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
End Class
