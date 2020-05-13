Imports ClientIntegrazioneSic.Intema.WS.Ana.Richiesta

<System.Runtime.Serialization.DataContract()>
Public Class ItemAnagrafica
    Private _idInterno As String
    Private _idAnagraficaSic As String
    Private _denominazione As String
    Private _flagPersonaFisica As Boolean
    Private _partitaIva As String
    Private _cognomeLR As String
    Private _nomeLR As String
    Private _sessoLR As String
    Private _comuneNascitaLR As String
    Private _dataNascitaLR As Date
    Private _indirizzoResidenzaLR As String
    Private _comuneResidenzaLR As String
    Private _capResidenzaLR As String
    Private _codiceFiscaleLR As String
    Private _codiceFiscale As String
    Private _sesso As String
    Private _isEstero As Boolean
    Private _comuneNascita As String
    Private _dataNascita As Date
    Private _idSedeSic As String
    Private _indirizzoSede As String
    Private _comuneSede As String
    Private _capSede As String
    Private _nomeSede As String

    Private _idTipologiaPagamentoSic As String
    Private _tipologiaPagamentoNome As String

    Private _idContoCorrenteSic As String
    Private _isModalitaPrincipale As Boolean
    Private _iban As String

    Private _isDatoSensibile As Boolean
    Private _cup As String
    Private _cig As String

    Public Property Denominazione As String
        Get
            Return _denominazione
        End Get
        Set(value As String)
            _denominazione = value
        End Set
    End Property

    Public Property FlagPersonaFisica As Boolean
        Get
            Return _flagPersonaFisica
        End Get
        Set(value As Boolean)
            _flagPersonaFisica = value
        End Set
    End Property

    Public Property Sesso As String
        Get
            Return _sesso
        End Get
        Set(value As String)
            _sesso = value
        End Set
    End Property

    Public Property ComuneNascita As String
        Get
            Return _comuneNascita
        End Get
        Set(value As String)
            _comuneNascita = value
        End Set
    End Property

    Public Property DataNascita As Date
        Get
            Return _dataNascita
        End Get
        Set(value As Date)
            _dataNascita = value
        End Set
    End Property


    Public Property CodiceFiscale As String
        Get
            Return _codiceFiscale
        End Get
        Set(value As String)
            _codiceFiscale = value
        End Set
    End Property

    Public Property PartitaIva As String
        Get
            Return _partitaIva
        End Get
        Set(value As String)
            _partitaIva = value
        End Set
    End Property

    Public Property Iban As String
        Get
            Return _iban
        End Get
        Set(value As String)
            _iban = value
        End Set
    End Property


    Public Property IdInterno As String
        Get
            Return _idInterno
        End Get
        Set(value As String)
            _idInterno = value
        End Set
    End Property

    Public Property CognomeLR As String
        Get
            Return _cognomeLR
        End Get
        Set(value As String)
            _cognomeLR = value
        End Set
    End Property

    Public Property NomeLR As String
        Get
            Return _nomeLR
        End Get
        Set(value As String)
            _nomeLR = value
        End Set
    End Property

    Public Property SessoLR As String
        Get
            Return _sessoLR
        End Get
        Set(value As String)
            _sessoLR = value
        End Set
    End Property

    Public Property ComuneNascitaLR As String
        Get
            Return _comuneNascitaLR
        End Get
        Set(value As String)
            _comuneNascitaLR = value
        End Set
    End Property

    Public Property DataNascitaLR As Date
        Get
            Return _dataNascitaLR
        End Get
        Set(value As Date)
            _dataNascitaLR = value
        End Set
    End Property



    Public Property ComuneResidenzaLR As String
        Get
            Return _comuneResidenzaLR
        End Get
        Set(value As String)
            _comuneResidenzaLR = value
        End Set
    End Property



    Public Property CodiceFiscaleLR As String
        Get
            Return _codiceFiscaleLR
        End Get
        Set(value As String)
            _codiceFiscaleLR = value
        End Set
    End Property

    Public Property IndirizzoResidenzaLR As String
        Get
            Return _indirizzoResidenzaLR
        End Get
        Set(value As String)
            _indirizzoResidenzaLR = value
        End Set
    End Property

    Public Property CapResidenzaLR As String
        Get
            Return _capResidenzaLR
        End Get
        Set(value As String)
            _capResidenzaLR = value
        End Set
    End Property

    Public Property IsEstero As Boolean
        Get
            Return _isEstero
        End Get
        Set(value As Boolean)
            _isEstero = value
        End Set
    End Property

    Public Property IndirizzoSede As String
        Get
            Return _indirizzoSede
        End Get
        Set(value As String)
            _indirizzoSede = value
        End Set
    End Property

    Public Property ComuneSede As String
        Get
            Return _comuneSede
        End Get
        Set(value As String)
            _comuneSede = value
        End Set
    End Property

    Public Property CapSede As String
        Get
            Return _capSede
        End Get
        Set(value As String)
            _capSede = value
        End Set
    End Property

    Public Property NomeSede As String
        Get
            Return _nomeSede
        End Get
        Set(value As String)
            _nomeSede = value
        End Set
    End Property

    Public Property TipologiaPagamentoNome As String
        Get
            Return _tipologiaPagamentoNome
        End Get
        Set(value As String)
            _tipologiaPagamentoNome = value
        End Set
    End Property

    Public Property IdAnagraficaSic As String
        Get
            Return _idAnagraficaSic
        End Get
        Set(value As String)
            _idAnagraficaSic = value
        End Set
    End Property

    Public Property Cup As String
        Get
            Return _cup
        End Get
        Set(value As String)
            _cup = value
        End Set
    End Property

    Public Property Cig As String
        Get
            Return _cig
        End Get
        Set(value As String)
            _cig = value
        End Set
    End Property

    Public Property IdSedeSic As String
        Get
            Return _idSedeSic
        End Get
        Set(value As String)
            _idSedeSic = value
        End Set
    End Property

    Public Property IdTipologiaPagamentoSic As String
        Get
            Return _idTipologiaPagamentoSic
        End Get
        Set(value As String)
            _idTipologiaPagamentoSic = value
        End Set
    End Property

    Public Property IdContoCorrenteSic As String
        Get
            Return _idContoCorrenteSic
        End Get
        Set(value As String)
            _idContoCorrenteSic = value
        End Set
    End Property

    Public Property IsModalitaPrincipale As Boolean
        Get
            Return _isModalitaPrincipale
        End Get
        Set(value As Boolean)
            _isModalitaPrincipale = value
        End Set
    End Property

    Public Property IsDatoSensibile As Boolean
        Get
            Return _isDatoSensibile
        End Get
        Set(value As Boolean)
            _isDatoSensibile = value
        End Set
    End Property

    Public Shared Function TransformItemAnagraficaToAnagraficaTypeSIC(ByVal itemAnagrafica As ItemAnagrafica) As CaricamentoAnagrafiche_TypesElementoAnagraficaMassiva
        Dim caricamentoAnagrafiche_TypesElementoAnagraficaMassiva As New CaricamentoAnagrafiche_TypesElementoAnagraficaMassiva
        Dim anagrafica_Types As New Anagrafica_Types
        If itemAnagrafica.FlagPersonaFisica Then
            anagrafica_Types.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.F

            Dim listaStringhe As String() = itemAnagrafica.Denominazione.Split("#")
            If not listaStringhe is Nothing AndAlso listaStringhe.Count() > 2 Then
                Throw New Exception("Errore: " + itemAnagrafica.Denominazione + " presenti più di un separatore #")
            End If
            anagrafica_Types.Cognome = listaStringhe(0)
            anagrafica_Types.Nome = listaStringhe(1)

            anagrafica_Types.CodiceFiscale = itemAnagrafica.CodiceFiscale

            anagrafica_Types.SessoSpecified = True

            If not itemAnagrafica.Sesso Is Nothing AndAlso itemAnagrafica.Sesso.Equals("F") Then
                anagrafica_Types.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSesso.F
            Else
                anagrafica_Types.Sesso = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSesso.M
            End If

            anagrafica_Types.EsteroSpecified = True
            If itemAnagrafica.IsEstero Then
                anagrafica_Types.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.S
            Else
                anagrafica_Types.Estero = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesEstero.N
            End If

            anagrafica_Types.ComuneNS = itemAnagrafica.ComuneNascita
            anagrafica_Types.DataNascitaSpecified = True
            anagrafica_Types.DataNascita = itemAnagrafica.DataNascita

        Else
            anagrafica_Types.TipoAnagrafica = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesTipoAnagrafica.G
            anagrafica_Types.PartitaIva = itemAnagrafica.PartitaIva
            anagrafica_Types.Denominazione = itemAnagrafica.Denominazione

            'Legale Rappresentate
            anagrafica_Types.CognomeLR = itemAnagrafica.CognomeLR
            anagrafica_Types.NomeLR = itemAnagrafica.NomeLR
            If Not itemAnagrafica.SessoLR Is Nothing AndAlso itemAnagrafica.SessoLR.Equals("F") Then
                anagrafica_Types.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSessoLR.F
            Else
                anagrafica_Types.SessoLR = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.Anagrafica_TypesSessoLR.M
            End If
            anagrafica_Types.ComuneNSLR = itemAnagrafica.ComuneNascitaLR
            anagrafica_Types.DataNascitaLRSpecified = True
            anagrafica_Types.DataNascitaLR = itemAnagrafica.DataNascitaLR
            anagrafica_Types.IndirizzoLR = itemAnagrafica.IndirizzoResidenzaLR
            anagrafica_Types.ComuneRESLR = itemAnagrafica.ComuneResidenzaLR
            anagrafica_Types.CAPRESLR = itemAnagrafica.CapResidenzaLR
            anagrafica_Types.CodiceFiscaleLR = itemAnagrafica.CodiceFiscaleLR
        End If

        Dim sede As New Sede_Types
        sede.Indirizzo = itemAnagrafica.IndirizzoSede
        sede.Comune = itemAnagrafica.ComuneSede
        sede.CAP = itemAnagrafica.CapSede
        sede.NomeSede = itemAnagrafica.NomeSede
        sede.TipoPagamento = itemAnagrafica.TipologiaPagamentoNome

        anagrafica_Types.Sede = sede


        Dim datiBancari As New DatiBancari_Types
        datiBancari.IBAN = itemAnagrafica.Iban
        datiBancari.ModalitaPrincipaleSpecified = True
        If itemAnagrafica.IsModalitaPrincipale Then
            datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.S
        Else
            datiBancari.ModalitaPrincipale = ClientIntegrazioneSic.Intema.WS.Ana.Richiesta.DatiBancari_TypesModalitaPrincipale.N
        End If

        anagrafica_Types.DatiBancari = datiBancari

        caricamentoAnagrafiche_TypesElementoAnagraficaMassiva.Anagrafica = anagrafica_Types
        caricamentoAnagrafiche_TypesElementoAnagraficaMassiva.IdAnagraficaMittente = itemAnagrafica.IdInterno
        Return caricamentoAnagrafiche_TypesElementoAnagraficaMassiva
    End Function



End Class
