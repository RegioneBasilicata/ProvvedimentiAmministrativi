
Imports System.Collections.Generic
Imports ClientIntegrazioneSic.Intema.WS.Ana.Richiesta
Imports ClientIntegrazioneSic.Intema.WS.Risposta
Imports DllDocumentale
Imports Microsoft.Office.Interop.Excel

<System.Runtime.Serialization.DataContract()>
Public Class Ext_AnagraficaInfo
    Private _id As String = String.Empty
    Private _descrizioneTipologia As String = String.Empty
    Private _partitaIvaOrCodFiscToView As String = String.Empty
    Private _tipologia As String = String.Empty
    Private _partitaIva As String = String.Empty
    Private _codiceFiscale As String = String.Empty
    Private _denominazione As String = String.Empty
    Private _sedi As Generic.List(Of Ext_SedeAnagraficaInfo)
    Private _legaleRappresentante As Ext_LegaleRappresentanteInfo
    Private _cognome As String = String.Empty
    Private _nome As String = String.Empty
    Private _comuneNascita As String = String.Empty
    Private _dataNascita As String = String.Empty
    Private _altriNomi As String = String.Empty
    Private _sesso As String = String.Empty
    Private _pignoramento As Boolean
    Private _notePignoramento As String = String.Empty
    Private _estero As Boolean
    Private _commissioni As Boolean
    Private _contrattoInfo As Ext_ContrattoInfo
    Private _fatturaInfoHeader As Ext_FatturaInfoHeader
    Private _isDatoSensibile As Boolean
    Private _importoSpettante As Double
    Private _idDocumento As String
    Private _importoOriginarioSuImpegno As Double
    Private _importoResiduoSuImpegno As Double
    Private _importoDaLiquidare As Double
    Private _idModalitaPagamentoSelected As String = String.Empty
    
    

    <System.Runtime.Serialization.DataMember()>
    Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property DescrizioneTipologia() As String
        Get
            Return _descrizioneTipologia
        End Get
        Set(ByVal value As String)
            _descrizioneTipologia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Tipologia() As String
        Get
            Return _tipologia
        End Get
        Set(ByVal value As String)
            _tipologia = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Denominazione() As String
        Get
            Return _denominazione
        End Get
        Set(ByVal value As String)
            _denominazione = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property PartitaIva() As String
        Get
            Return _partitaIva
        End Get
        Set(ByVal value As String)
            _partitaIva = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property CodiceFiscale() As String
        Get
            Return _codiceFiscale
        End Get
        Set(ByVal value As String)
            _codiceFiscale = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ListaSedi() As Generic.List(Of Ext_SedeAnagraficaInfo)
        Get
            Return _sedi
        End Get
        Set(ByVal value As Generic.List(Of Ext_SedeAnagraficaInfo))
            _sedi = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Cognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal value As String)
            _cognome = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ComuneNascita() As String
        Get
            Return _comuneNascita
        End Get
        Set(ByVal value As String)
            _comuneNascita = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property DataNascita() As String
        Get
            Return _dataNascita
        End Get
        Set(ByVal value As String)
            _dataNascita = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property AltriNomi() As String
        Get
            Return _altriNomi
        End Get
        Set(ByVal value As String)
            _altriNomi = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Sesso() As String
        Get
            Return _sesso
        End Get
        Set(ByVal value As String)
            _sesso = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Property Pignoramento() As Boolean
        Get
            Return _pignoramento
        End Get
        Set(ByVal value As Boolean)
            _pignoramento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property NotePignoramento() As String
        Get
            Return _notePignoramento
        End Get
        Set(ByVal value As String)
            _notePignoramento = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Property Estero() As Boolean
        Get
            Return _estero
        End Get
        Set(ByVal value As Boolean)
            _estero = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Property LegaleRappresentante() As Ext_LegaleRappresentanteInfo
        Get
            Return _legaleRappresentante
        End Get
        Set(ByVal value As Ext_LegaleRappresentanteInfo)
            _legaleRappresentante = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Commissioni() As Boolean
        Get
            Return _commissioni
        End Get
        Set(ByVal value As Boolean)
            _commissioni = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Contratto() As Ext_ContrattoInfo
        Get
            Return _contrattoInfo
        End Get
        Set(ByVal value As Ext_ContrattoInfo)
            _contrattoInfo = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property Fattura() As Ext_FatturaInfoHeader
        Get
            Return _fatturaInfoHeader
        End Get
        Set(ByVal value As Ext_FatturaInfoHeader)
            _fatturaInfoHeader = value
        End Set
    End Property

    <System.Runtime.Serialization.DataMember()>
    Property IsDatoSensibile() As Boolean
        Get
            Return _isDatoSensibile
        End Get
        Set(ByVal value As Boolean)
            _isDatoSensibile = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImportoSpettante() As Double
        Get
            Return _importoSpettante
        End Get
        Set(ByVal value As Double)
            _importoSpettante = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Property ImportoOriginarioSuImpegno() As Double
        Get
            Return _importoOriginarioSuImpegno
        End Get
        Set(ByVal value As Double)
            _importoOriginarioSuImpegno = value
        End Set
    End Property
     <System.Runtime.Serialization.DataMember()>
    Property ImportoResiduoSuImpegno() As Double
        Get
            Return _importoResiduoSuImpegno
        End Get
        Set(ByVal value As Double)
            _importoResiduoSuImpegno = value
        End Set
    End Property
     <System.Runtime.Serialization.DataMember()>
    Property ImportoDaLiquidare() As Double
        Get
            Return _importoDaLiquidare
        End Get
        Set(ByVal value As Double)
            _importoDaLiquidare = value
        End Set
    End Property
     <System.Runtime.Serialization.DataMember()>
    Property IdModalitaPagamentoSelected() As String
        Get
            Return _idModalitaPagamentoSelected
        End Get
        Set(ByVal value As String)
            _idModalitaPagamentoSelected = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property IdDocumento As String
        Get
            Return _idDocumento
        End Get
        Set(value As String)
            _idDocumento = value
        End Set
    End Property
    <System.Runtime.Serialization.DataMember()>
    Public Property PartitaIvaOrCodFiscToView As String
        Get
            Return _partitaIvaOrCodFiscToView
        End Get
        Set(value As String)
            _partitaIvaOrCodFiscToView = value
        End Set
    End Property

    Public Sub New()

    End Sub


    Public Function TransformItemInExtObj(ByVal beneficiarioItem As DllDocumentale.ItemLiquidazioneImpegnoBeneficiarioInfo) As Ext_AnagraficaInfo

        Dim beneficiarioExt As New Ext_AnagraficaInfo
        beneficiarioExt.IdDocumento = beneficiarioItem.IdDocumento
        beneficiarioExt.Denominazione = beneficiarioItem.Denominazione
        beneficiarioExt.CodiceFiscale = beneficiarioItem.CodiceFiscale
        beneficiarioExt.Commissioni = beneficiarioItem.EsenzCommBonifico
        beneficiarioExt.ComuneNascita = beneficiarioItem.SedeComune
        beneficiarioExt.ImportoSpettante = beneficiarioItem.ImportoSpettante
        beneficiarioExt.IsDatoSensibile = beneficiarioItem.IsDatoSensibile
        beneficiarioExt.ID = beneficiarioItem.IdAnagrafica
        beneficiarioExt.PartitaIva = beneficiarioItem.PartitaIva
        
        If beneficiarioItem.FlagPersonaFisica Then
            beneficiarioExt.Tipologia = "F"
            beneficiarioExt.DescrizioneTipologia = "Persone Fisica"
        Else
            beneficiarioExt.Tipologia = "G"
            beneficiarioExt.DescrizioneTipologia = "Persone Giuridica"
        End If

        Dim contrattoItem As New Ext_ContrattoInfo
        contrattoItem.CodiceCUP = beneficiarioItem.Cup
        contrattoItem.CodiceCIG = beneficiarioItem.Cig
        contrattoItem.Id = beneficiarioItem.IdContratto
        contrattoItem.NumeroRepertorio = beneficiarioItem.NumeroRepertorioContratto

        beneficiarioExt.Contratto = contrattoItem

        Dim extListaSedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
        Dim extSede As New Ext_SedeAnagraficaInfo
        extSede.IdSede = beneficiarioItem.IdSede
        extSede.Bollo = beneficiarioItem.Bollo
        extSede.Comune = beneficiarioItem.SedeComune
        extSede.IdModalitaPagamento = beneficiarioItem.IdModalitaPag
        extSede.Indirizzo = beneficiarioItem.SedeVia
        extSede.ModalitaPagamento = beneficiarioItem.DescrizioneModalitaPag
        
        extSede.HasDatiBancari = beneficiarioItem.HasDatiBancari
        If beneficiarioItem.HasDatiBancari Then
            Dim extDatoBancario As Ext_DatiBancariInfo = New Ext_DatiBancariInfo

            extDatoBancario.Iban = beneficiarioItem.Iban
            extDatoBancario.IdContoCorrente = beneficiarioItem.IdConto
            
            extSede.DatiBancari = New Generic.List(Of Ext_DatiBancariInfo)
            extSede.DatiBancari.Add(extDatoBancario)
        End If

        extListaSedi.Add(extSede)
        beneficiarioExt.ListaSedi = extListaSedi

        Return beneficiarioExt
    End Function


    Public Function TransformDisponibilitaImpegnoBeneficiarioInExtObj(ByVal beneficiarioItem As Risposta_DisponibilitaImpegno_TypesBeneficiario) As Ext_AnagraficaInfo

        Dim beneficiarioExt As New Ext_AnagraficaInfo
        
        beneficiarioExt.ID = beneficiarioItem.IdBeneficiario
        beneficiarioExt.Denominazione = beneficiarioItem.DescrizioneBeneficiario
        beneficiarioExt.ImportoOriginarioSuImpegno = beneficiarioItem.ImportoOriginario
        beneficiarioExt.ImportoResiduoSuImpegno = beneficiarioItem.ImportoResiduo
        If Not String.IsNullOrEmpty(beneficiarioItem.InfoAggiuntiveBeneficiario.TipoAnagrafica) Then
            beneficiarioExt.Tipologia = beneficiarioItem.InfoAggiuntiveBeneficiario.TipoAnagrafica
            If beneficiarioItem.InfoAggiuntiveBeneficiario.TipoAnagrafica = "G" Then
                beneficiarioExt.DescrizioneTipologia = "Persone Giuridica"
                beneficiarioExt.PartitaIva = beneficiarioItem.InfoAggiuntiveBeneficiario.PartitaIva
                beneficiarioExt.PartitaIvaOrCodFiscToView = "(P.IVA) " & beneficiarioExt.PartitaIva
                
            ElseIf beneficiarioItem.InfoAggiuntiveBeneficiario.TipoAnagrafica = "F" Then
                beneficiarioExt.DescrizioneTipologia = "Persone Fisica"
                beneficiarioExt.CodiceFiscale = beneficiarioItem.InfoAggiuntiveBeneficiario.CodiceFiscale
                beneficiarioExt.PartitaIvaOrCodFiscToView = "(C.F.) " & beneficiarioExt.CodiceFiscale
                beneficiarioExt.Cognome = beneficiarioItem.DescrizioneBeneficiario
            End If
        End If

        If beneficiarioItem.InfoAggiuntiveBeneficiario.Commissioni = "N" Then
            beneficiarioExt.Commissioni = False
        ElseIf beneficiarioItem.InfoAggiuntiveBeneficiario.Commissioni = "S" Then
            beneficiarioExt.Commissioni = True
        End If

        If beneficiarioItem.InfoAggiuntiveBeneficiario.DatoSensibile = "N" Then
            beneficiarioExt.IsDatoSensibile = False
        ElseIf beneficiarioItem.InfoAggiuntiveBeneficiario.DatoSensibile = "S" Then
            beneficiarioExt.IsDatoSensibile = True
        End If

        Dim extContratto As New Ext_ContrattoInfo
        extContratto.CodiceCIG = beneficiarioItem.InfoAggiuntiveBeneficiario.CodiceCig
        extContratto.CodiceCUP = beneficiarioItem.InfoAggiuntiveBeneficiario.CodiceCup
        beneficiarioExt.Contratto = extContratto



        Dim extListaSedi As Generic.List(Of Ext_SedeAnagraficaInfo) = New Generic.List(Of Ext_SedeAnagraficaInfo)
        Dim extSede As New Ext_SedeAnagraficaInfo
        extSede.IdSede = beneficiarioItem.IdSede
        extSede.IdModalitaPagamento = beneficiarioItem.InfoAggiuntiveBeneficiario.MetodoPagamento
        extSede.ModalitaPagamento = beneficiarioItem.InfoAggiuntiveBeneficiario.NomeMetodoPagamento
        extSede.NomeSede = beneficiarioItem.InfoAggiuntiveBeneficiario.NomeSede
        extSede.Indirizzo = beneficiarioItem.InfoAggiuntiveBeneficiario.IndirizzoSede



        If beneficiarioItem.InfoAggiuntiveBeneficiario.Bollo = "N" Then
            extSede.Bollo = False
        ElseIf beneficiarioItem.InfoAggiuntiveBeneficiario.Bollo = "S" Then
            extSede.Bollo = True
        End If

        Dim extDatiBancari As Generic.List(Of Ext_DatiBancariInfo) = New Generic.List(Of Ext_DatiBancariInfo)
        Dim extDatoBancario As New Ext_DatiBancariInfo
        extDatoBancario.Iban = beneficiarioItem.InfoAggiuntiveBeneficiario.Iban
        extDatoBancario.IdContoCorrente = beneficiarioItem.InfoAggiuntiveBeneficiario.ContoBancario
        extDatiBancari.Add(extDatoBancario)
        extSede.DatiBancari = extDatiBancari

        extListaSedi.Add(extSede)
        beneficiarioExt.ListaSedi = extListaSedi
        Return beneficiarioExt
    End Function

    Public Shared Function TransformItemAnagraficaToExtAnagrafica(ByVal itemAnagrafica As ItemAnagrafica) As Ext_AnagraficaInfo
        
        Dim extAnagraficaInfo As New Ext_AnagraficaInfo
        extAnagraficaInfo.ID = itemAnagrafica.IdAnagraficaSic

        If itemAnagrafica.FlagPersonaFisica Then
            extAnagraficaInfo.Tipologia = "F"

            Dim listaStringhe As String() = itemAnagrafica.Denominazione.Split("#")
            If listaStringhe.Count() > 2 Then
                Throw New Exception("Errore: " + itemAnagrafica.Denominazione + " presenti più di un separatore #")
            End If
            extAnagraficaInfo.Cognome = listaStringhe(0)
            extAnagraficaInfo.Nome = listaStringhe(1)

            extAnagraficaInfo.CodiceFiscale = itemAnagrafica.CodiceFiscale
            extAnagraficaInfo.Sesso = itemAnagrafica.Sesso
            
            extAnagraficaInfo.Estero = itemAnagrafica.IsEstero

            extAnagraficaInfo.ComuneNascita = itemAnagrafica.ComuneNascita
            extAnagraficaInfo.DataNascita = itemAnagrafica.DataNascita

        Else
            extAnagraficaInfo.Tipologia = "G"
            extAnagraficaInfo.PartitaIva = itemAnagrafica.PartitaIva
            extAnagraficaInfo.Denominazione = itemAnagrafica.Denominazione

            'Legale Rappresentate
            Dim extLegaleRappresentate As New Ext_LegaleRappresentanteInfo
            extLegaleRappresentate.Cognome = itemAnagrafica.CognomeLR
            extLegaleRappresentate.Nome = itemAnagrafica.NomeLR
            extLegaleRappresentate.Sesso = itemAnagrafica.SessoLR
            extLegaleRappresentate.ComuneNascita = itemAnagrafica.ComuneNascitaLR
            extLegaleRappresentate.DataNascita = itemAnagrafica.DataNascitaLR
            extLegaleRappresentate.Indirizzo = itemAnagrafica.IndirizzoResidenzaLR
            extLegaleRappresentate.ComuneResidenza = itemAnagrafica.ComuneResidenzaLR
            extLegaleRappresentate.CapResidenza = itemAnagrafica.CapResidenzaLR
            extLegaleRappresentate.CodiceFiscale = itemAnagrafica.CodiceFiscaleLR

            extAnagraficaInfo.LegaleRappresentante = extLegaleRappresentate
        End If

        Dim listaSedi As New List(Of Ext_SedeAnagraficaInfo)
        Dim sede As New Ext_SedeAnagraficaInfo
        sede.IdSede = itemAnagrafica.IdSedeSic
        sede.Indirizzo = itemAnagrafica.IndirizzoSede
        sede.Comune = itemAnagrafica.ComuneSede
        sede.CapComune = itemAnagrafica.CapSede
        sede.NomeSede = itemAnagrafica.NomeSede
        sede.IdModalitaPagamento = itemAnagrafica.IdTipologiaPagamentoSic

        Dim listaDatiBancari As New List(Of Ext_DatiBancariInfo)
        Dim datiBancari As New Ext_DatiBancariInfo
        datiBancari.IBAN = itemAnagrafica.Iban
        datiBancari.ModalitaPrincipale = itemAnagrafica.IsModalitaPrincipale
        datiBancari.IdContoCorrente = itemAnagrafica.IdContoCorrenteSic

        listaDatiBancari.Add(datiBancari)
        sede.DatiBancari = listaDatiBancari

        listaSedi.Add(sede)
        extAnagraficaInfo.ListaSedi = listaSedi

        Dim contratto As New Ext_ContrattoInfo

        contratto.CodiceCIG = itemAnagrafica.Cig
        contratto.CodiceCUP = itemAnagrafica.Cup
        extAnagraficaInfo.Contratto = contratto

        extAnagraficaInfo.IsDatoSensibile = itemAnagrafica.IsDatoSensibile
        
        Return extAnagraficaInfo
    End Function



    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
