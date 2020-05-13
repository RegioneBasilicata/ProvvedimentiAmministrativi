Option Strict Off
Option Explicit On
'UPGRADE_WARNING: L'istanza della classe è stata cambiata in public. Fare clic qui per ulteriori informazioni: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1043"'
<System.Runtime.InteropServices.ProgId("ClSistema_NET.ClSistema")> Public Class ClSistema
	Public bTRACE As Boolean
	Public bLOG As Boolean
	Public sLOGPATH As String
	Public sLOGFILE As String
	Public sTRACEFILE As String
	
	Public Client_Funzione As Integer
	Public Client_Operatore As String
	Public Client_Terminale As String
	Public Client_Procedura As String
	
	Public LOCKTIMEOUT As Integer
	Public TIMEOUTCONNECTION As Integer
	Public TIMEOUTCOMMAND As Integer
	Public PROCDB As New Collection
	Public TimeLock As Short 'tempo di attesa su un lock di pagina o di edit
	
	'$$I----------------------------------------------------
	'Funzione  : Registra_Trace_OLE(TextTrace, Optional RoutName)
	'            Registra il percorso di elaborazione di una funzione
	'
	'Parametri : TextTrace = testo da registrare
	'            RoutName = routine attraversata
	'            FormName = Form attraversata
	'
	'Ritorno  : ()
	'$$F----------------------------------------------------
    Public Sub Registra_Trace(ByRef TextTrace As String, Optional ByRef RoutName As String = "", Optional ByRef FormName As String = "")
        Dim Riga As String = ""
        Dim fTrace As Integer

        '* -----------------------------
        If Not bTRACE Then
            Exit Sub
        End If
        '* -----------------------------

        If Trim(TextTrace) = "" Then
            Exit Sub
        End If


        If Len(TextTrace) > 1 Then
            If Mid(TextTrace, 1, 2) = ">>" Then
                Riga = ">> " & Now & ";" & Client_Terminale & ";" & Client_Funzione & ";" & Client_Operatore & ";" & Trim(RoutName) & ";" & Trim(FormName) & ";" & Trim(Mid(TextTrace, 3))
            Else
                Riga = Now & ";" & Client_Terminale & ";" & Client_Funzione & ";" & Client_Operatore & ";" & Trim(RoutName) & ";" & Trim(FormName) & ";" & Trim(TextTrace)
            End If
        End If
        Try
            fTrace = FreeFile() 'Apre il trace
            FileClose(fTrace)
            FileOpen(fTrace, sTRACEFILE, OpenMode.Append)
            PrintLine(fTrace, Trim(Riga))
        Catch ex As Exception
        Finally
            FileClose(fTrace)
            fTrace = CInt(Nothing)
        End Try
    End Sub

    '$$I----------------------------------------------------
    'Funzione  : Registra_Log(Optional DescrErr, Optional RoutName)
    '            Registra nel file log dell'applicazione errori o log dell'applicazione
    '
    'Parametri : DescrErr=descrizione errore
    '            RoutName = routine che ha generato l'errore
    '
    'Ritorno  : ()
    '$$F----------------------------------------------------
    Public Sub Registra_Log(Optional ByRef DescrErr As String = "", Optional ByRef RoutName As String = "", Optional ByRef FormName As String = "", Optional ByRef Visualizza As Boolean = False)
        '* -------------------------------------------------------
        Dim fNum As Integer
        Dim Riga As String

        If IsNothing(DescrErr) Then
            DescrErr = " >> n. errore=" & Err.Number & ": " & Err.Description
        End If
        Try
            If bLOG Then
                fNum = FreeFile()
                FileOpen(fNum, sLOGFILE, OpenMode.Append)
                PrintLine(fNum, Trim(Now & ";" & Client_Terminale & ";" & Client_Funzione & ";" & Client_Operatore & ";" & RoutName & ";" & FormName & ";" & DescrErr))
            End If
        Catch ex As Exception
        Finally
            FileClose(fNum)
        End Try

        If Visualizza Then
            Call MsgBox(DescrErr, MsgBoxStyle.Critical)
        End If

        If bTRACE Then
            Riga = DescrErr
            Call Registra_Trace(Riga, RoutName)
        End If

    End Sub




    'D03 -Nuova
    Public Function GetFileIni(ByRef ExeName As String, ByRef ProcIni As String) As String
        Dim fName As String
        Dim Linea As String
        Dim fNum As Integer
        Dim Y1 As Short
        Dim Var2 As String = ""
        Dim cPath As String
        

        'Verifico se esiste un file di set file.ini
        fName = "c:\Prodintema\Ambiente\Configurazione\start.ini"
        If Dir(fName) = "" Then
            'leggo il file di impostazione file.ini
            Y1 = InStrRev(fName, "\")
            If Y1 <> 0 Then
                cPath = Trim(Mid(fName, 1, Y1 - 1))
            End If
            Try
                fNum = FreeFile()
                FileOpen(fNum, fName, OpenMode.Input)
                While Not EOF(fNum)
                    Linea = LineInput(fNum)
                    If InStr(1, UCase(Linea), "SET " & UCase(ExeName) & "=") > 0 And Mid(Trim(Linea), 1, 1) <> "'" Then
                        Y1 = InStr(1, Linea, "=")
                        Var2 = Mid(Linea, Y1 + 1, Len(Linea) - Y1)
                        Var2 = PathAssoluto(cPath, Var2)
                    End If
                End While
            Catch ex As Exception
            Finally
                FileClose(fNum)
            End Try

            If Var2 <> "" And Dir(Var2) <> "" Then
                ProcIni = Var2
                GoTo FineSub
            End If
        End If


        'se non esiste cerco il file oleserver.ini
        fName = "c:\Prodintema\Ambiente\configurazione\oleserver.ini"
        If Dir(fName) <> "" Then
            ProcIni = fName
        Else
            'se non esiste ritorno il file chiamato
        End If
FineSub:
        GetFileIni = ProcIni
    End Function

    Public Function PathAssoluto(ByRef cPath As String, ByRef File As String, Optional ByRef sPar As String = "", Optional ByRef ParPath As String = "") As String
        Dim NewPAth As String
        Dim jj As Short

        If sPar <> "" Then
            File = Replace(File, sPar, ParPath)
        End If
        NewPAth = File
        If Left(NewPAth, 2) = ".\" Then
            NewPAth = cPath & Mid(NewPAth, 2)
        End If

        If Left(NewPAth, 3) = "..\" Then
            Do While True
                If Left(NewPAth, 3) = "..\" Then
                    For jj = Len(cPath) To 1 Step -1
                        If Mid(cPath, jj, 1) = "\" Then
                            cPath = Left(cPath, jj - 1)
                            Exit For
                        End If
                    Next jj
                    NewPAth = Mid(NewPAth, 4)
                Else
                    NewPAth = cPath & "\" & NewPAth
                    Exit Do
                End If
            Loop
        End If
        PathAssoluto = NewPAth
    End Function

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
    Function Cripta(ByVal Cript As Boolean, ByVal Pass As String, ByVal Strg As String, Optional ByVal Hx As Boolean = False) As String
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



    '$$I---------------------------------------------
    'Funzione : VerDataFile(Nome As String, Data As Date) As Boolean
    '           Verifica la data di creazione del file
    '
    'Parametri: Nome = Nome File
    '           Data = data da comparare
    '
    'Ritorno : ()
    '$$F---------------------------------------------
    Public Function VerDataFile(ByRef Nome As String, ByRef Data As Date) As Boolean
        Dim dd As Date

        VerDataFile = True
        If Trim(Dir(Nome)) <> "" Then
            dd = CDate(VB6.Format(FileDateTime(Nome), "dd/mm/yyyy"))
            VerDataFile = (DateDiff(Microsoft.VisualBasic.DateInterval.Day, Data, dd) = 0)
        End If

    End Function

    Public Sub New()

    End Sub
End Class