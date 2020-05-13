Option Strict Off
Option Explicit On 
<System.Runtime.InteropServices.ProgId("clDicGDB_NET.clDicGDB")> _
Public Class clDicGDB
    Public Enum DBCostErrori
        dbeOperazioneRiuscita = 0
        dbeNessunRecord = 1
        dbeRecordAssente = 2
        dbeRecordBloccato = 3
        dbeTimeOut = 4
        dbeArchivioNonAggiornabile = 5
        dbeDatiModificati = 6
        dbeArchivioChiuso = 7
        dbeChiaveDuplicata = 8
        dbeIgnoto = 9999
    End Enum


    'Codice azione conseguente l'errore
    Public Enum dbCostAzione
        dbAzione_Ripeti = 0
        dbAzione_Continua = 1
        dbAzione_Esci = 2
    End Enum
    Public Enum dbCostTipoDB
        dbADOAccess = 0
        dbADOSQL = 1
        dbjet = 2
    End Enum
    Public Enum dbCostTipoStato
        dbAperto = 0
        dbChiuso = 1
        dbElaborazione = 2
        dbBeginTrans = 3
    End Enum

    Public Enum dbCostLock
        dbLockLettura = 1
        dbLockPessimistico = 2
        dbLockOttimistico = 3
        dbLockBatch = 4
    End Enum

    Public Enum dbCostAperturaRs
        dbAperturaForward = 5
        dbAperturaKeyset = 1
        dbAperturaDinamica = 2
        dbAperturaStatica = 3
        dbAperturaDynaset = 4
    End Enum
End Class