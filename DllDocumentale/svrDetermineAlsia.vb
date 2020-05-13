Imports System.Configuration

Partial Public Class svrDetermineAlsia
    Inherits svrDetermineBase

    Public Sub New(ByVal operatore As DllAmbiente.Operatore)
        MyBase.New(operatore)
    End Sub
    Overrides Sub ChiamaDll(ByVal nomeOggetto As String, ByVal nomeAttivita As String)
        If nomeOggetto = "DllDocumentale.svrDetermineAlsia" Then
            Me.ElaboraPasso(CInt(nomeAttivita))
        Else
            MyBase.ChiamaDll(nomeOggetto, nomeAttivita)
        End If
    End Sub

    Overrides Sub ElaboraPasso(ByVal cFunzione As Integer)
        Try
            Select Case cFunzione
                Case vFunzioniPassoDetermina.cf_DETERMINA_4LIVELLO_INOLTRO
                    Call DETERMINA_4LIVELLO_INOLTRO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_COLLABORATORE_INOLTRO
                    Call DETERMINA_COLLABORATORE_INOLTRO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_DEPOSITO_PRELIEVO
                    Call DETERMINA_DEPOSITO_PRELIEVO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_SUPERVISORE_INOLTRO
                    Call DETERMINA_SUPERVISORE_INOLTRO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_PRELAZIONE
                    Call DETERMINA_PRELAZIONE()
                Case vFunzioniPassoDetermina.cf_UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO
                    Call UFFICIO_PROPONENTE_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDetermina.cf_UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDetermina.cf_UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO
                    Call UFFICIO_RAGIONERIA_RESPONSABILE_RIGETTO()
                Case vFunzioniPassoDetermina.cf_UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO
                    Call UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_ANNULLO
                    Call DETERMINA_ANNULLO()
                Case vFunzioniPassoDetermina.cf_DETERMINA_ASSEGNA
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

        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop
        Dim oUffRag As New DllAmbiente.Ufficio
        oUffRag.CodUfficio = oUffProponente.CodUfficioRagioneria
        pDirD = "UR"
        Call INOLTRO_ALTRO_UFFICIO(oUffRag)
        oUffRag = Nothing
        MyBase.Passo_Possibile()
        Exit Sub

    End Sub
    Private Sub DETERMINA_PRELAZIONE()
        If oOperatore.Test_Ruolo("GT001") Then
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
        'modgg 02-08 1
        Dim oUffContrAmm As New DllAmbiente.Ufficio
        oUffContrAmm = New DllAmbiente.Ufficio
        pDirD = "UCA"
        oUffContrAmm.CodUfficio = oOperatore.oUfficio.CodUfficioControlloAmministrativo
        Call INOLTRO_ALTRO_UFFICIO(oUffContrAmm, , "ARCHIVIA")
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
    Private Sub UFFICIO_CONTROLLO_AMMINISTRATIVO_RESPONSABILE_INOLTRO()
        'verifica firma di inoltro
        ' VERIFICA_FIRMA_INOLTRO()

        'recupero l'ufficio proponente
        Dim oUffProponente As New DllAmbiente.Ufficio
        oUffProponente.CodUfficio = objDocumento.Doc_Cod_Uff_Prop

        pDirD = "UAR"
        pDirU = "D"
        pProssimoAttore = oUffProponente.CodArchivio
        MyBase.Passo_Possibile()
        Exit Sub
    End Sub
    Private Sub DETERMINA_ASSEGNA()
        If oOperatore.Test_Ruolo("GT002") Then
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
    Private Sub DETERMINA_4LIVELLO_INOLTRO()
        Dim vR As Hashtable
        Dim supUff As String
        Dim respUff As String

        'il collaboratore non può far uscire la proposta dal suo ufficio
        pDirD = "StessoUfficio"
        pDirU = ""
        Log.Debug("Prossimo Attore:" & pProssimoAttore)
        'Mod salto  supervisore e vado in responsabile
        Log.Debug("Prossimo Attore:" & oOperatore.oUfficio.ResponsabileUfficio("DETERMINE"))
        If pProssimoAttore = oOperatore.oUfficio.ResponsabileUfficio("DETERMINE") Then
            pDirU = "R"
            Log.Debug("pDirU:" & pDirU)

            Exit Sub
        End If

        If pProssimoAttore <> "" Then
            Dim opTEmp As New DllAmbiente.Operatore()
            opTEmp.Codice = pProssimoAttore
            If opTEmp.Test_Attributo("LIVELLO_UFFICIO", "Supervisore") Then
                pDirU = "S"
                Exit Sub
            Else
                pDirU = "C"
                Exit Sub
            End If
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
 
End Class
