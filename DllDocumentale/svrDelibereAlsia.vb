Public Class svrDelibereAlsia
    Inherits svrDelibereBase

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrDelibereAlsia" Then
            'chiamata ad una funzione dell'oggetto PassoDetermina
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub
    Overrides Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            Select Case cFunzione
                Case vFunzioniPassoDelibera.cf_DELIBERA_4LIVELLO_INOLTRO
                    Call DELIBERA_4LIVELLO_INOLTRO()
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
                Case vFunzioniPassoDelibera.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO
                    Call UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO
                    Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDelibera.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
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
            Dim destinatario As New DllAmbiente.Operatore
            destinatario.Codice = prossimoAttore
            If destinatario.Test_Attributo("LIVELLO_UFFICIO", "4Livello") Then
                pDirU = "4"
            End If
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
    Private Sub DELIBERA_PRELAZIONE()
        If oOperatore.Test_Ruolo("GL001") Then
            pDirD = "StessoUfficio"
            pDirU = "S"
            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4Livello") Then
                pDirU = "4"
            End If
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
            ErrDesc = "L'operatore non può prelazionare"
        End If
    End Sub
    Private Sub DELIBERA_DEPOSITO_PRELIEVO()
        If oOperatore.Test_Ruolo("DL015") Then
            pDirD = "StessoUfficio"
            pDirU = ""

            If oOperatore.Test_Attributo("LIVELLO_UFFICIO", "4LIVELLO") Then
                pDirU = "4"
            End If
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
        Dim vR As Hashtable
        Dim supUff As String
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        'Mod salto  supervisore e vado in responsabile
        If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DELIBERE") Then
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
        vR = oOperatore.oUfficio.SupervisoriUfficio("DELIBERE")

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
        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        Dim soggettoaControllo As Boolean = True
        Dim attributoDaRicercare As New Documento_attributo
        attributoDaRicercare.Cod_attributo = "Controllo_Esterno"
        attributoDaRicercare.Doc_id = objDocumento.Doc_id
        attributoDaRicercare.Ente = Configuration.ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE")
        Dim listaAttributo As Generic.List(Of Documento_attributo) = FO_Get_Documento_Attributi(attributoDaRicercare)
        If listaAttributo.Count > 0 Then
            soggettoaControllo = listaAttributo.Item(0).IdValore
        End If

        If soggettoaControllo Then
            Dim oUffContrAmm As New DllAmbiente.Ufficio
            oUffContrAmm = New DllAmbiente.Ufficio
            pDirD = "UCA"
            oUffContrAmm.CodUfficio = oOperatore.oUfficio.CodUfficioControlloAmministrativo
            Call INOLTRO_ALTRO_UFFICIO(oUffContrAmm, , "ARCHIVIA")
            MyBase.Passo_Possibile()
        Else
            pDirD = "UAR"
            pDirU = "D"
            pProssimoAttore = oUffProponente.CodArchivio
            MyBase.Passo_Possibile()
        End If
    End Sub
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
        'recupero l'ufficio proponente
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        pDirD = "UAR"
        pDirU = "D"
        pProssimoAttore = oUffProponente.CodArchivio
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_INOLTRO()
        Dim oUffArchivio As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        pDirD = "UAR"
        pDirU = "D"
        If tipoDocumento = "1" Then
            Dim vr As Object = Assegna_Numerazione_Definitiva()
            If vr(0) <> 0 Then
                Errore = vr(0)
                ErrDesc = vr(1)
                Exit Sub
            End If
        End If

        pProssimoAttore = oUffProponente.CodArchivio
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub
    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio

        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        oUffDestinazione.CodUfficio = oUffProponente.CodUfficioDirigenzaDipartimento
        pDirD = "UDD"
        Call INOLTRO_ALTRO_UFFICIO(oUffDestinazione)
        MyBase.Passo_Possibile()
        oUffDestinazione = Nothing
        oUffProponente = Nothing
    End Sub
    Private Sub UFFICIO_DIRIGENZA_DIPARTIMENTO_RESPONSABILE_RIGETTO()
        Dim oUffDestinazione As New DllAmbiente.Ufficio
        Dim oUffProponente As New DllAmbiente.Ufficio

        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        Dim oUffRag As New DllAmbiente.Ufficio
        oUffRag.CodUfficio = oUffProponente.CodUfficioRagioneria
        pDirD = "UR"
        Call INOLTRO_ALTRO_UFFICIO(oUffRag)
        oUffRag = Nothing
        MyBase.Passo_Possibile()
    End Sub
   
    Private Sub UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
        Dim oUffPr As New DllAmbiente.Ufficio
        oUffPr.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        pDirD = "UP"
        Call INOLTRO_ALTRO_UFFICIO(oUffPr, , "RIGETTO")
        MyBase.Passo_Possibile()
        oUffPr = Nothing
    End Sub
    Private Sub DELIBERA_4LIVELLO_INOLTRO()
        'Dim vR As Hashtable
        'Dim supUff As String
        'Dim respUff As String

        ''il collaboratore non può far uscire la proposta dal suo ufficio
        'pDirD = "StessoUfficio"
        'pDirU = ""
        ''Mod salto  supervisore e vado in responsabile
        'If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE") Then
        '    pDirU = "R"
        '    Exit Sub
        'End If

        'If pProssimoAttore <> "" Then
        '    pDirU = "S"
        '    Exit Sub
        'End If

        ''il successivo attore è il supervisore se l'utente ha un superviziore di default
        'supUff = oOperatore.Attributo("SUPERVISORE_DEFAULT")
        'If supUff <> "" Then
        '    pDirU = "S"
        '    pProssimoAttore = supUff
        '    Exit Sub
        'End If

        ''il successivo attore è il supervisore se l'ufficio ha un supervisore
        'vR = oOperatore.oUfficio.SupervisoriUfficio("DETERMINE")

        'If Not vR Is Nothing Then

        '    'Dim chiavi As Array = vR
        '    Dim en As IDictionaryEnumerator = vR.GetEnumerator
        '    While (en.MoveNext())
        '        supUff = DirectCast(en.Key, String)
        '        Exit While
        '    End While

        '    'supUff = vR.Item(DirectCast(en.Current(), String))
        '    If supUff <> "" Then
        '        pDirU = "S"
        '        pProssimoAttore = supUff
        '        Exit Sub
        '    End If
        'End If

        'respUff = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE")
        'If respUff <> "" Then
        '    pDirU = "R"
        '    pProssimoAttore = respUff
        'End If
        'MyBase.Passo_Possibile()
    End Sub
End Class
