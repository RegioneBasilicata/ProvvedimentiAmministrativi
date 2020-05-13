Public Class Utility

    '$$I------------------------------------------------
    'Funzione : Cripta(Cript As Boolean, pass$, strg$, Optional Hx) As String
    '           Cripta/Descripta la stringa in input
    'Parametri: Cript = true :cripta, false descripta
    '           pass = password per il criptaggio
    '           strg = stringa da criptare
    '           hx   = indica se criptare in esadecimale
    '
    'Ritorna  : Stringa Criptata
    '$$F------------------------------------------------
    '$$I------------------------------------------------
    'Funzione : Cripta(Cript As Boolean, pass$, strg$, Optional Hx) As String
    '           Cripta/Descripta la stringa in input
    'Parametri: Cript = true :cripta, false descripta
    '           pass = password per il criptaggio
    '           strg = stringa da criptare
    '           hx   = indica se criptare in esadecimale
    '
    'Ritorna  : Stringa Criptata
    '$$F------------------------------------------------
    Shared Function Cripta(ByVal Cript As Boolean, ByVal Pass As String, ByVal Strg As String, Optional ByVal Hx As Boolean = False) As String
        Dim sCryptA As String
        Dim sCryptH As String
        Dim a As Integer
        Dim b As Object
        Dim ii As Integer
        Dim jj As String

        If Cript Then
            sCryptA = Strg
            ' cripta la stringa in input
            a = 1
            For ii = 1 To Len(sCryptA)
                b = Asc(Mid(Pass, a, 1)) : a = a + 1 : If a > Len(Pass) Then a = 1
                Mid(sCryptA, ii, 1) = Chr(Asc(Mid(sCryptA, ii, 1)) Xor b)
            Next
            Cripta = sCryptA

            'se richiesto ritorno il valore esadecimale
            If Hx Then
                sCryptH = ""
                For ii = 1 To Len(sCryptA)
                    jj = Hex(Asc(Mid(sCryptA, ii, 1)))
                    If Len(jj) = 1 Then jj = "0" + jj
                    sCryptH = sCryptH + jj
                Next
                Cripta = sCryptH
            End If
        Else
            sCryptA = Strg
            If Hx Then
                sCryptA = ""
                For ii = 1 To Len(Strg) Step 2
                    jj = Mid(Strg, ii, 2)
                    sCryptA = sCryptA + Chr(Val("&H" + jj))
                Next
            End If

            'decripta la stringa in input
            a = 1
            For ii = 1 To Len(sCryptA)
                b = Asc(Mid(Pass, a, 1)) : a = a + 1 : If a > Len(Pass) Then a = 1
                Mid(sCryptA, ii, 1) = Chr(Asc(Mid(sCryptA, ii, 1)) Xor b)
            Next
            Cripta = sCryptA
        End If
    End Function

End Class
