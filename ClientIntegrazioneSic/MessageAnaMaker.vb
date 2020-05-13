Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports ClientIntegrazioneSic
Imports ClientIntegrazioneSic.Intema.WS.Ana.Richiesta
Imports ClientIntegrazioneSic.Intema.WS.Ana.Risposta
Imports ClientIntegrazioneSic.Intema.WS
Imports System.Configuration




Public Class MessageAnaMaker
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(MessageAnaMaker))

    Public Shared Function SerializeIt(ByVal messaggio As MessaggioRichiesta_Types) As String
        Dim xDeserializer As XmlSerializer
        Dim buffer As New MemoryStream(4096)

        xDeserializer = New XmlSerializer(GetType(MessaggioRichiesta_Types))

        xDeserializer.Serialize(buffer, messaggio)
        buffer.Seek(0, SeekOrigin.Begin)

        Dim strXml = New StreamReader(buffer).ReadToEnd()
        ' chiude il MemoryStream
        buffer.Close()

        Return strXml

    End Function
    Private Shared Function DeserializeIt(ByVal xmlMessage As String) As MessaggioRisposta_Types

        Dim xDeserializer As XmlSerializer
        Dim sr As StringReader
        Dim xr As XmlTextReader
        sr = New StringReader(xmlMessage)
        xr = New XmlTextReader(sr)
        xDeserializer = New XmlSerializer(GetType(MessaggioRisposta_Types))
        Return DirectCast(xDeserializer.Deserialize(xr), MessaggioRisposta_Types)
    End Function

    Public Shared Function createInterrogazioneAnagraficaMessage(ByVal operatore As DllAmbiente.Operatore,
                                                                 ByVal denominazione As String,
                                                                 ByVal codiceFiscale As String,
                                                                 ByVal pIva As String,
                                                                 ByVal idAnagrafica As String,
                                                                 ByVal idContratto As String) As Array

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogaAnagrafica As New InterrogaAnagrafica_Types

            If Not String.IsNullOrEmpty(denominazione) Then
                objRichiestaInterrogaAnagrafica.ItemElementName = ItemChoiceType.Denominazione
                objRichiestaInterrogaAnagrafica.Item = denominazione
            End If

            If Not String.IsNullOrEmpty(pIva) Then
                objRichiestaInterrogaAnagrafica.ItemElementName = ItemChoiceType.PartitaIva
                objRichiestaInterrogaAnagrafica.Item = pIva
            End If

            If Not String.IsNullOrEmpty(codiceFiscale) Then
                objRichiestaInterrogaAnagrafica.ItemElementName = ItemChoiceType.CodiceFiscale
                objRichiestaInterrogaAnagrafica.Item = codiceFiscale
            End If

            If Not String.IsNullOrEmpty(idAnagrafica) Then
                objRichiestaInterrogaAnagrafica.ItemElementName = ItemChoiceType.IdAnagrafica
                objRichiestaInterrogaAnagrafica.Item = idAnagrafica
            End If

            If Not String.IsNullOrEmpty(idContratto) Then
                objRichiestaInterrogaAnagrafica.ItemElementName = ItemChoiceType.NumeroContratto
                objRichiestaInterrogaAnagrafica.Item = idContratto
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogaAnagrafica
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            outputQuerySic.return = outputQuerySic.return.Replace("null", "N")
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Log.Debug("Risposta SIC:" & excep.Descrizione)
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayInterrogaAnagrafica As Risposta_InterrogaAnagrafica_Types

            arrayInterrogaAnagrafica = DirectCast(risposta.Item.Item, Risposta_InterrogaAnagrafica_Types)

            Return DirectCast(arrayInterrogaAnagrafica.Anagrafica, Array)
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createUpdateAnagraficaMessage(ByVal operatore As DllAmbiente.Operatore,
                                                         ByVal idAnagrafica As String,
                                                         ByVal objAnagrafica As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_Types) As Risposta_ModificaAnagrafica_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaModificaAnagrafica As New ModificaAnagrafica_Types

            If Not objAnagrafica Is Nothing Then
                objRichiestaModificaAnagrafica.CodiceFiscaleOperatore = operatore.CodiceFiscale
                objRichiestaModificaAnagrafica.IdAnagrafica = idAnagrafica

                objRichiestaModificaAnagrafica.AltriNomi = objAnagrafica.AltriNomi
                objRichiestaModificaAnagrafica.CAPRESLR = objAnagrafica.CAPRESLR
                objRichiestaModificaAnagrafica.CodiceFiscale = objAnagrafica.CodiceFiscale
                objRichiestaModificaAnagrafica.CodiceFiscaleLR = objAnagrafica.CodiceFiscaleLR
                objRichiestaModificaAnagrafica.Cognome = objAnagrafica.Cognome
                objRichiestaModificaAnagrafica.CognomeLR = objAnagrafica.CognomeLR
                objRichiestaModificaAnagrafica.Commissioni = objAnagrafica.Commissioni
                objRichiestaModificaAnagrafica.CommissioniSpecified = objAnagrafica.CommissioniSpecified
                objRichiestaModificaAnagrafica.ComuneNS = objAnagrafica.ComuneNS
                objRichiestaModificaAnagrafica.ComuneNSLR = objAnagrafica.ComuneNSLR
                objRichiestaModificaAnagrafica.ComuneRESLR = objAnagrafica.ComuneRESLR
                objRichiestaModificaAnagrafica.DataNascita = objAnagrafica.DataNascita
                objRichiestaModificaAnagrafica.DataNascitaLR = objAnagrafica.DataNascitaLR
                objRichiestaModificaAnagrafica.DataNascitaLRSpecified = objAnagrafica.DataNascitaLRSpecified
                objRichiestaModificaAnagrafica.DataNascitaSpecified = objAnagrafica.DataNascitaSpecified
                objRichiestaModificaAnagrafica.Denominazione = objAnagrafica.Denominazione
                objRichiestaModificaAnagrafica.Estero = objAnagrafica.Estero
                objRichiestaModificaAnagrafica.EsteroSpecified = objAnagrafica.EsteroSpecified
                objRichiestaModificaAnagrafica.IndirizzoLR = objAnagrafica.IndirizzoLR
                objRichiestaModificaAnagrafica.Nome = objAnagrafica.Nome
                objRichiestaModificaAnagrafica.NomeLR = objAnagrafica.NomeLR
                objRichiestaModificaAnagrafica.NotaPignoramento = objAnagrafica.NotaPignoramento
                objRichiestaModificaAnagrafica.PartitaIva = objAnagrafica.PartitaIva
                objRichiestaModificaAnagrafica.Pignoramento = objAnagrafica.Pignoramento
                objRichiestaModificaAnagrafica.PignoramentoSpecified = objAnagrafica.PignoramentoSpecified
                objRichiestaModificaAnagrafica.Sesso = objAnagrafica.Sesso
                objRichiestaModificaAnagrafica.SessoLR = objAnagrafica.SessoLR
                objRichiestaModificaAnagrafica.SessoLRSpecified = objAnagrafica.SessoLRSpecified
                objRichiestaModificaAnagrafica.SessoSpecified = objAnagrafica.SessoSpecified
                objRichiestaModificaAnagrafica.TipoAnagrafica = objAnagrafica.TipoAnagrafica
            End If
            richiesta.Richiesta.Item = objRichiestaModificaAnagrafica
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arraycreaAnagrafica As Risposta_ModificaAnagrafica_Types

            arraycreaAnagrafica = DirectCast(risposta.Item.Item, Risposta_ModificaAnagrafica_Types)

            Return arraycreaAnagrafica
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createUpdateSedeMessage(ByVal operatore As DllAmbiente.Operatore,
                                                 ByVal idAnagrafica As String,
                                                 ByVal idSede As String,
                                                 ByVal objSede As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_Types) As Risposta_ModificaSede_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaUpdateSede As New ModificaSede_Types

            If Not String.IsNullOrEmpty(idAnagrafica) Then
                objRichiestaUpdateSede = New Ana.Richiesta.ModificaSede_Types

                objRichiestaUpdateSede.CodiceFiscaleOperatore = operatore.CodiceFiscale
                objRichiestaUpdateSede.IdAnagrafica = idAnagrafica
                objRichiestaUpdateSede.IdSede = idSede

                objRichiestaUpdateSede.BolloSpecified = objSede.BolloSpecified
                objRichiestaUpdateSede.Bollo = objSede.Bollo
                objRichiestaUpdateSede.CAP = objSede.CAP
                objRichiestaUpdateSede.Comune = objSede.Comune
                objRichiestaUpdateSede.EMail = objSede.EMail
                objRichiestaUpdateSede.Fax = objSede.Fax
                objRichiestaUpdateSede.Indirizzo = objSede.Indirizzo
                objRichiestaUpdateSede.NomeSede = objSede.NomeSede
                objRichiestaUpdateSede.Telefono = objSede.Telefono
                objRichiestaUpdateSede.TipoPagamento = objSede.TipoPagamento
                'aggiungerere campi sede
            End If
            richiesta.Richiesta.Item = objRichiestaUpdateSede
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arraySedeAnagrafica As Risposta_ModificaSede_Types

            arraySedeAnagrafica = DirectCast(risposta.Item.Item, Risposta_ModificaSede_Types)

            Return arraySedeAnagrafica
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createUpdateContoBancarioMessage(ByVal operatore As DllAmbiente.Operatore,
                                               ByVal idAnagrafica As String,
                                               ByVal idSede As String,
                                               ByVal idConto As String,
                                               ByVal objContoBancario As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types) As Risposta_ModificaContoBancario_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaUpdateCB As New ModificaContoBancario_Types

            If Not String.IsNullOrEmpty(idAnagrafica) And Not String.IsNullOrEmpty(idSede) Then
                objRichiestaUpdateCB.CodiceFiscaleOperatore = operatore.CodiceFiscale

                objRichiestaUpdateCB.IdAnagrafica = idAnagrafica
                objRichiestaUpdateCB.IdSede = idSede
                objRichiestaUpdateCB.IdContoCorrente = idConto
                If Not objContoBancario Is Nothing Then
                    objRichiestaUpdateCB.CIN = objContoBancario.CIN
                    objRichiestaUpdateCB.ContoCorrente = objContoBancario.ContoCorrente
                    objRichiestaUpdateCB.IBAN = objContoBancario.IBAN
                    objRichiestaUpdateCB.ModalitaPrincipale = objContoBancario.ModalitaPrincipale
                End If

            End If
            richiesta.Richiesta.Item = objRichiestaUpdateCB
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayConto As Risposta_ModificaContoBancario_Types

            arrayConto = DirectCast(risposta.Item.Item, Risposta_ModificaContoBancario_Types)

            Return arrayConto
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createInterrogazioneComuniMessage(ByVal operatore As DllAmbiente.Operatore,
                                                                 ByVal denominazione As String) As Array

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaInterrogaComuni As New InterrogaComuni_Types

            If Not String.IsNullOrEmpty(denominazione) Then
                objRichiestaInterrogaComuni.NomeComune = denominazione
            Else
                Log.Error("Tentativo fallito di lettura comuni, il valore denominazione non è stato valorizzato")
                Throw New Exception("Tentativo fallito di lettura comuni, il valore denominazione non è stato valorizzato")
            End If

            richiesta.Richiesta.Item = objRichiestaInterrogaComuni
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Log.Debug("Risposta SIC:" & excep.Descrizione)
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayInterrogaComuni As Risposta_InterrogaComuni_Types

            arrayInterrogaComuni = DirectCast(risposta.Item.Item, Risposta_InterrogaComuni_Types)

            Return DirectCast(arrayInterrogaComuni.Comune, Array)
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createCreaAnagraficaMessage(ByVal operatore As DllAmbiente.Operatore,
                                                       ByVal objAnagrafica As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_Types) As Risposta_CreaAnagrafica_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaCreaAnagrafica As New CreaAnagrafica_Types

            If Not objAnagrafica Is Nothing Then
                objRichiestaCreaAnagrafica.CodiceFiscaleOperatore = operatore.CodiceFiscale
                objRichiestaCreaAnagrafica.Anagrafica = objAnagrafica
            End If

            richiesta.Richiesta.Item = objRichiestaCreaAnagrafica
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arraycreaAnagrafica As Risposta_CreaAnagrafica_Types

            arraycreaAnagrafica = DirectCast(risposta.Item.Item, Risposta_CreaAnagrafica_Types)

            Return arraycreaAnagrafica
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createCreaSedeMessage(ByVal operatore As DllAmbiente.Operatore,
                                                 ByVal idAnagrafica As String,
                                                 ByVal objSede As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Sede_Types,
                                                 ByVal objDatiBancari As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types) As Risposta_CreaSede_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaCreaSede As New CreaSede_Types

            If Not String.IsNullOrEmpty(idAnagrafica) Then
                objRichiestaCreaSede.CodiceFiscaleOperatore = operatore.CodiceFiscale
                objRichiestaCreaSede.Sede = New Ana.Richiesta.Sede_Types
                objRichiestaCreaSede.Sede = objSede
                objRichiestaCreaSede.IdAnagrafica = idAnagrafica

                If Not objDatiBancari Is Nothing Then
                    objRichiestaCreaSede.DatiBancari = New Ana.Richiesta.DatiBancari_Types
                    objRichiestaCreaSede.DatiBancari = objDatiBancari
                End If

            End If
            richiesta.Richiesta.Item = objRichiestaCreaSede
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arraySedeAnagrafica As Risposta_CreaSede_Types

            arraySedeAnagrafica = DirectCast(risposta.Item.Item, Risposta_CreaSede_Types)

            Return arraySedeAnagrafica
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Shared Function createCreaContoBancarioMessage(ByVal operatore As DllAmbiente.Operatore,
                                               ByVal idAnagrafica As String,
                                               ByVal idSede As String,
                                               ByVal objContoBancario As ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_Types) As Risposta_CreaContoBancario_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaCreaCB As New CreaContoBancario_Types

            If Not String.IsNullOrEmpty(idAnagrafica) And Not String.IsNullOrEmpty(idSede) Then
                objRichiestaCreaCB.CodiceFiscaleOperatore = operatore.CodiceFiscale

                objRichiestaCreaCB.IdAnagrafica = idAnagrafica
                objRichiestaCreaCB.IdSede = idSede
                If Not objContoBancario Is Nothing Then
                    objRichiestaCreaCB.DatiBancari = New Ana.Richiesta.DatiBancari_Types
                    objRichiestaCreaCB.DatiBancari = objContoBancario
                End If

            End If
            richiesta.Richiesta.Item = objRichiestaCreaCB
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim arrayConto As Risposta_CreaContoBancario_Types

            arrayConto = DirectCast(risposta.Item.Item, Risposta_CreaContoBancario_Types)

            Return arrayConto
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createInterrogazioneVerificaAnagraficaMessage(ByVal operatore As DllAmbiente.Operatore,
                                                                  ByVal anagrafica As Ana.Richiesta.Anagrafica_Types) As Ana.Risposta.Anagrafica_Types

        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaVerificaAnagrafica As New VerificaAnagrafica_Types

            If Not anagrafica Is Nothing Then
                objRichiestaVerificaAnagrafica.Anagrafica = anagrafica
            End If

            richiesta.Richiesta.Item = objRichiestaVerificaAnagrafica
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            If ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim excep As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                If excep.Codice = 1 Then
                    Throw New ListaVuotaException(excep.Codice)
                Else
                    Log.Debug("Risposta SIC:" & excep.Descrizione)
                    Throw New Exception(excep.Descrizione)
                End If
            End If
            Dim returnVerificaAnagrafica As Risposta_VerificaAnagrafica_Types

            returnVerificaAnagrafica = DirectCast(risposta.Item.Item, Risposta_VerificaAnagrafica_Types)
            Return returnVerificaAnagrafica.Anagrafica
        Catch ex As ListaVuotaException
            Log.Debug("Risposta SIC: Nessun elemento trovato, lista vuota")
            Throw ex
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function createCaricamentoAnagraficheMassivoMessage(ByVal operatore As DllAmbiente.Operatore,
                                                                 ByVal listaAnagraficheToValidateType As Array) As MessaggioRispostaMassiva_TypesRispostaMassiva_Type()
        Dim listaErrori As New List(Of String)
        Try
            Dim richiesta As New Ana.Richiesta.MessaggioRichiesta_Types
            richiesta.Intestazione = New Ana.Richiesta.Intestazione_Types
            richiesta.Intestazione.InfoMittDest = operatore.Cognome & " " & operatore.Nome
            richiesta.Intestazione.Applicazione = "ProvvedimentiAmministrativi"

            richiesta.Richiesta = New Richiesta_Types
            Dim objRichiestaCaricamentoAnagrafiche As New CaricamentoAnagrafiche_Types

            objRichiestaCaricamentoAnagrafiche.CodiceFiscaleOperatore = operatore.CodiceFiscale
            objRichiestaCaricamentoAnagrafiche.AnagraficaMassiva = listaAnagraficheToValidateType

          richiesta.Richiesta.Item = objRichiestaCaricamentoAnagrafiche
            Dim WS As New SICAnagrafeInterface(ConfigurationManager.AppSettings("SicWSAna"))
            WS.AllowAutoRedirect = True
            Dim inputQuerySic As New queryAnagrafica
            inputQuerySic.q = SerializeIt(richiesta)
            Log.Debug("Richiesta SIC:" & inputQuerySic.q)
            Dim outputQuerySic As queryAnagraficaResponse = WS.queryAnagrafica(inputQuerySic)
            outputQuerySic.return = outputQuerySic.return.Replace("null", "N")
            Dim risposta As MessaggioRisposta_Types = DeserializeIt(outputQuerySic.return)
           
            Log.Debug("Risposta SIC:" & outputQuerySic.return)
            
            If ((risposta.Item.GetType)) Is GetType(MessaggioRispostaMassiva_Types) Then
                Dim messaggioRispostaMassiva_Types As MessaggioRispostaMassiva_Types = DirectCast(risposta.Item, MessaggioRispostaMassiva_Types)

                Return messaggioRispostaMassiva_Types.RispostaMassiva_Type
            ElseIf ((risposta.Item.GetType)) Is GetType(Eccezione_Types) Then
                Dim eccezione_Types As Eccezione_Types = DirectCast(risposta.Item, Eccezione_Types)
                Throw New Exception(eccezione_Types.Descrizione)
            End If
        Catch ex As InvalidCastException
            Log.Debug("Risposta SIC con InvalidCastException:" & ex.Message)
            Throw New InvalidCastException("")
        Catch ex As Exception
            Log.Debug("Risposta SIC con Exception:" & ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Function

End Class
