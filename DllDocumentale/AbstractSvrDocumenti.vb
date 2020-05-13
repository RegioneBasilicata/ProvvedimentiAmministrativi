Public Module AbstractSvrDocumenti
    Friend Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(AbstractSvrDocumenti))

    Function getSvrDetermine(ByVal op As DllAmbiente.Operatore) As svrDetermineBase
        Dim ente As String = NOME_ENTE_INSTALLAZIONE

        Select Case ente
            Case "REGIONE"
                Return New DllDocumentale.svrDetermineRegione(op)
            Case "ALSIA"
                Return New DllDocumentale.svrDetermineAlsia(op)
            Case Else
                Log.Error("ENTE non definito in  getSvrDetermine")
                Return Nothing
        End Select

    End Function


    Function getSvrDisposizioni(ByVal op As DllAmbiente.Operatore) As svrDisposizioniBase
        Dim ente As String = NOME_ENTE_INSTALLAZIONE

        Select Case ente
            Case "REGIONE"
                Return New DllDocumentale.svrDisposizioniRegione(op)
            Case Else
                Return New DllDocumentale.svrDisposizioniRegione(op)
                Log.Error("ENTE non definito in  getSvrDisposizioni")
                Throw New DocumentaleException("ENTE non definito in  getSvrDisposizioni")
                Return Nothing
        End Select

    End Function

    Function getSvrDelibere(ByVal op As DllAmbiente.Operatore) As svrDelibereBase

        Dim ente As String = NOME_ENTE_INSTALLAZIONE
        Select Case ente
            Case "REGIONE"
                Return New DllDocumentale.svrDelibereRegione(op)
            Case "ALSIA"
                Return New DllDocumentale.svrDelibereAlsia(op)
            Case Else
                Log.Error("ENTE non definito in  getSvrDelibere")
                Return Nothing
        End Select

    End Function
    Function getSvrDocumenti(ByVal op As DllAmbiente.Operatore) As svrDocumenti
        Dim ente As String = NOME_ENTE_INSTALLAZIONE

        Select Case ente
            Case "REGIONE"
                Return New DllDocumentale.svrDocumentiRegione(op)
            Case "ALSIA"
                Return New DllDocumentale.svrDocumentiAlsia(op)
            Case Else
                Log.Error("ENTE non definito in  getSvrDetermine")
                Return Nothing
        End Select

    End Function

    Function getSvrAltriAtti(ByVal op As DllAmbiente.Operatore) As svrAltriAttiBase

        Dim ente As String = NOME_ENTE_INSTALLAZIONE
        Select Case ente
            Case "REGIONE"
                Return New DllDocumentale.svrAltriAttiRegione(op)
            Case "ALSIA"

            Case Else
                Log.Error("ENTE non definito in  getSvrDelibere")
                Return Nothing
        End Select

    End Function
End Module


