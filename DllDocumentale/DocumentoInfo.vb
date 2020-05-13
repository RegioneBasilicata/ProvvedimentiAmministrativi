Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Intema.DBUtility
Namespace Model

    <System.Runtime.Serialization.DataContract()> _
    Public Class DocumentoInfo

        Private _Doc_Id As String
        Private _Doc_numeroProvvisorio As String = ""
        Private _Doc_numero As String = ""
        Private _Doc_Oggetto As String = ""
        Private _Doc_Cod_Uff_Prop As String = ""
        Private _Doc_Descrizione_ufficio As String = ""

        Private _Doc_Data As DateTime
        Private _Doc_Tipo As Int32
        Private _Doc_Codice_Documento As Int32
        Private _Doc_Stato As Int32
        Private _Doc_liquidazione As Int16
        Private _Doc_Contabile As Int16
        Private _Doc_Pubblicazione As Int16
        Private _Doc_Testo As String = ""
        Private _Doc_id_WFE As String = ""
        Private _Doc_utenteCreazione As String = ""
        Private _Doc_privacy As Int16
        Private _Doc_AOOprotocollo As String = ""
        Private _Doc_numProtocollo As String = ""
        Private _Doc_NumDefinitivo As String = ""
        Private _Doc_dataRegistrazione As DateTime
        Private _Doc_operatore As String = ""
        Private _Doc_IsContabile As Int16
        Private _Doc_dataRicezione As DateTime
        Private _Doc_Cod_Uff_Pubblico As String = ""

        Private _Lista_Compiti As IList(Of String)
        Private _ListaAllegati As ArrayList
        Private _ListaOpContabil(9) As String

        Private _ListaUfficiDiCompetenza As IList(Of DllAmbiente.StrutturaInfo)

        Private _Doc_codCup As String = ""
        Private _Doc_codApp As String = ""
        Private _Doc_cod_Esterno As String = ""
        Private _Doc_Investimento_Pub As Int16

        'Private _ListaRagAssunzione As List(Of ItemRagAssunzioneInfo)
        'Private _ListaRagLiquidazione As List(Of ItemRagLiquidazioneInfo)
        Private _ListaBilancio As List(Of ItemImpegnoInfo)
        Private _ListaLiquidazione As List(Of ItemLiquidazioneInfo)
        Private _Doc_XML As String
        Private _Numero As String
        Private _Doc_FlagDaPagare As Int16
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_NumCronologico() As String

            Get
                Return _Doc_NumDefinitivo
            End Get
            Set(ByVal Value As String)
                _Doc_NumDefinitivo = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_codCup() As String

            Get
                Return _Doc_codCup
            End Get
            Set(ByVal Value As String)
                _Doc_codCup = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_codApp() As String

            Get
                Return _Doc_codApp
            End Get
            Set(ByVal Value As String)
                _Doc_codApp = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_cod_Esterno() As String

            Get
                Return _Doc_cod_Esterno
            End Get
            Set(ByVal Value As String)
                _Doc_cod_Esterno = Value
            End Set
        End Property

        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Investimento_Pub() As Int16

            Get
                Return _Doc_Investimento_Pub
            End Get
            Set(ByVal Value As Int16)
                _Doc_Investimento_Pub = Value
            End Set
        End Property

        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Cod_Uff_Pubblico() As String

            Get
                Return _Doc_Cod_Uff_Pubblico
            End Get
            Set(ByVal Value As String)
                _Doc_Cod_Uff_Pubblico = Value
            End Set
        End Property

        Property Doc_XML() As String

            Get
                Return _Doc_XML
            End Get
            Set(ByVal Value As String)
                _Doc_XML = Value
            End Set
        End Property

        <System.Runtime.Serialization.DataMember()> _
        Property Doc_id() As String

            Get
                Return _Doc_Id
            End Get
            Set(ByVal Value As String)
                _Doc_Id = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_numeroProvvisorio() As String
            Get
                Return _Doc_numeroProvvisorio
            End Get
            Set(ByVal Value As String)
                _Doc_numeroProvvisorio = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_numero() As String
            Get
                Return _Doc_numero
            End Get
            Set(ByVal Value As String)
                _Doc_numero = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Oggetto() As String
            Get
                Return _Doc_Oggetto
            End Get
            Set(ByVal Value As String)
                _Doc_Oggetto = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Descrizione_ufficio() As String
            Get
                Return _Doc_Descrizione_ufficio
            End Get
            Set(ByVal Value As String)
                _Doc_Descrizione_ufficio = Value
            End Set
        End Property
       
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Cod_Uff_Prop() As String
            Get
                Return _Doc_Cod_Uff_Prop
            End Get
            Set(ByVal Value As String)
                _Doc_Cod_Uff_Prop = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Data() As DateTime
            Get
                Return _Doc_Data
            End Get
            Set(ByVal Value As DateTime)
                _Doc_Data = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Tipo() As Int32
            Get
                Return _Doc_Tipo
            End Get
            Set(ByVal Value As Int32)
                _Doc_Tipo = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Codice_Documento() As Int32
            Get
                Return _Doc_Codice_Documento
            End Get
            Set(ByVal Value As Int32)
                _Doc_Codice_Documento = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Stato() As Int32
            Get
                Return _Doc_Stato
            End Get
            Set(ByVal Value As Int32)
                _Doc_Stato = Value
            End Set

        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_liquidazione() As Int16
            Get
                Return _Doc_liquidazione
            End Get
            Set(ByVal Value As Int16)
                _Doc_liquidazione = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Contabile() As Int16
            Get
                Return _Doc_Contabile
            End Get
            Set(ByVal Value As Int16)
                _Doc_Contabile = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Pubblicazione() As Int16
            Get
                Return _Doc_Pubblicazione
            End Get
            Set(ByVal Value As Int16)
                _Doc_Pubblicazione = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_Testo() As String
            Get
                Return _Doc_Testo

            End Get
            Set(ByVal Value As String)
                _Doc_Testo = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_id_WFE() As String
            Get
                Return _Doc_id_WFE
            End Get
            Set(ByVal Value As String)
                _Doc_id_WFE = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_utenteCreazione() As String
            Get
                Return _Doc_utenteCreazione
            End Get
            Set(ByVal Value As String)
                _Doc_utenteCreazione = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_privacy() As Int16
            Get
                Return _Doc_privacy
            End Get
            Set(ByVal Value As Int16)
                _Doc_privacy = Value
            End Set
        End Property

        Property Doc_AOOprotocollo() As String
            Get
                Return _Doc_AOOprotocollo
            End Get
            Set(ByVal Value As String)
                _Doc_AOOprotocollo = Value
            End Set
        End Property

        Property Doc_numProtocollo() As String
            Get
                Return _Doc_numProtocollo
            End Get
            Set(ByVal Value As String)
                _Doc_numProtocollo = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_dataRegistrazione() As DateTime
            Get
                Return _Doc_dataRegistrazione
            End Get
            Set(ByVal Value As DateTime)
                _Doc_dataRegistrazione = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_operatore() As String
            Get
                Return _Doc_operatore
            End Get
            Set(ByVal Value As String)
                _Doc_operatore = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_IsContabile() As Int16
            Get
                Return _Doc_IsContabile
            End Get
            Set(ByVal Value As Int16)
                _Doc_IsContabile = Value
            End Set
        End Property
        <System.Runtime.Serialization.DataMember()> _
        Property Doc_dataRicezione() As DateTime
            Get
                Return _Doc_dataRicezione
            End Get
            Set(ByVal Value As DateTime)
                _Doc_dataRicezione = Value
            End Set
        End Property

        Property ListaAllegati() As ArrayList
            Get
                Return _ListaAllegati
            End Get
            Set(ByVal Value As ArrayList)
                _ListaAllegati = Value
            End Set
        End Property

        Property Lista_Compiti() As IList(Of String)
            Get
                Return _Lista_Compiti
            End Get
            Set(ByVal Value As IList(Of String))
                _Lista_Compiti = Value
            End Set
        End Property

        'Property ListaRagAssunzione() As List(Of ItemRagAssunzioneInfo)
        '    Get
        '        Return _ListaRagAssunzione
        '    End Get
        '    Set(ByVal Value As List(Of ItemRagAssunzioneInfo))
        '        _ListaRagAssunzione = Value
        '    End Set
        'End Property

        'Property ListaRagLiquidazione() As List(Of ItemRagLiquidazioneInfo)
        '    Get
        '        Return _ListaRagLiquidazione
        '    End Get
        '    Set(ByVal Value As List(Of ItemRagLiquidazioneInfo))
        '        _ListaRagLiquidazione = Value
        '    End Set
        'End Property

        Property ListaBilancio() As List(Of ItemImpegnoInfo)
            Get
                Return _ListaBilancio
            End Get
            Set(ByVal Value As List(Of ItemImpegnoInfo))
                _ListaBilancio = Value
            End Set
        End Property

        Property ListaLiquidazione() As List(Of ItemLiquidazioneInfo)
            Get
                Return _ListaLiquidazione
            End Get
            Set(ByVal Value As List(Of ItemLiquidazioneInfo))
                _ListaLiquidazione = Value
            End Set
        End Property

        Property ListaOpContabil() As Array
            Get
                Return _ListaOpContabil
            End Get
            Set(ByVal Value As Array)
                If Value.Length <= 1 Then
                    _ListaOpContabil = "0;0;0;0;0;0;0;0".Split(";")
                Else
                    _ListaOpContabil = Value
                End If
            End Set
        End Property

        ReadOnly Property Numero() As String
            Get
                If String.IsNullOrEmpty(_Doc_numero) Then
                    Return _Doc_numeroProvvisorio
                Else
                    Return _Doc_numero
                End If
            End Get
        End Property

        Function getOpContabile(ByVal value As Integer) As String
            Dim result As String = "0"
            If _ListaOpContabil.Length <= value Then
                Return result
            End If
            If Not _ListaOpContabil Is Nothing Then
                result = _ListaOpContabil(value)
                If String.IsNullOrEmpty(result) Then
                    result = "0"
                End If
            End If

            Return result
        End Function

        Function haveOpContabile() As Boolean

            If Not ListaOpContabil Is Nothing Then
                For Each value As String In ListaOpContabil
                    If value <> "0" AndAlso value <> "" Then
                        Return True
                    End If
                Next

                Return False
            Else
                Return True
            End If
        End Function

        Property Lista_UfficiDiCompetenza() As IList(Of DllAmbiente.StrutturaInfo)
            Get
                Return _ListaUfficiDiCompetenza
            End Get
            Set(ByVal value As IList(Of DllAmbiente.StrutturaInfo))
                _ListaUfficiDiCompetenza = value
            End Set
        End Property

        <System.Runtime.Serialization.DataMember()> _
        Property Doc_FlagDaPagare() As Int16
            Get
                Return _Doc_FlagDaPagare
            End Get
            Set(ByVal Value As Int16)
                _Doc_FlagDaPagare = Value
            End Set
        End Property

        Public Function TipologiaProvvedimento(ByVal codDocumento As String) As String

            Dim leggiTipologiaDocumento As String = "SELECT Ttd_descrizione " & _
            " FROM Documento inner join Tab_Tipo_Documenti  on Doc_Tipo = Ttd_idTipoDocumento" & _
            " WHERE Doc_Id = " & param_id_Documento

            Dim strConn As String = Intema.DBUtility.SqlHelper.ConnectionString("DOCUMENTALE")

            Dim parms(0) As SqlParameter

            parms(0) = New SqlParameter(param_id_Documento, SqlDbType.VarChar)
            parms(0).Value = codDocumento

            Dim returnValue As String = ""
            Using rdr As SqlDataReader = Intema.DBUtility.SqlHelper.ExecuteReader(strConn, CommandType.Text, leggiTipologiaDocumento, parms, -1)
                While rdr.Read()
                    If Not rdr.IsDBNull(0) Then returnValue = rdr.GetString(0)
                End While
                rdr.Close()
            End Using

            Return returnValue
        End Function
        Public Function getXmlDati(Optional ByVal tipoMessaggio As Integer = 0) As Xml.XmlDocument

            Dim xmlDocument As New Xml.XmlDocument
            xmlDocument.AppendChild(xmlDocument.CreateElement("dati"))
            Try
                Dim nodo As Xml.XmlNode = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "numerodetermina", "")
                If String.IsNullOrEmpty(Me.Doc_numero) Then
                    nodo.InnerText = Me.Doc_numeroProvvisorio
                Else
                    nodo.InnerText = Me.Doc_numero
                End If
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "linkdetermina", "")
                Dim linkDetermina As String = ""
                Dim href As String
                If String.IsNullOrEmpty(Me.Doc_numero) Then
                    href = Me.Doc_numeroProvvisorio
                Else
                    href = Me.Doc_numero
                End If

                Select Case tipoMessaggio
                    Case 0, 2, 3, 5
                        linkDetermina = " <a href=""AggiungiAllAlberoAction.aspx?key=" & Me.Doc_id & """> " & href & " </a>  "
                    Case 4, 7, 8
                        linkDetermina = " <a href=""StoricoDocumentoAction.aspx?key=" & Me.Doc_id & """> " & href & " </a>  "
                    Case 6, 11
                        linkDetermina = " <a href=""AllegatiDocumentoAction.aspx?key=" & Me.Doc_id & """> " & href & " </a>  "
                    Case Else
                        linkDetermina = " <a href=""AggiungiAllAlberoAction.aspx?key=" & Me.Doc_id & """> " & href & " </a>  "
                End Select
                nodo.InnerText = linkDetermina
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "IdDocumento", "")
                nodo.InnerText = Me.Doc_id
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "codufficioproponente", "")
                nodo.InnerText = Me.Doc_Cod_Uff_Prop
                xmlDocument.FirstChild.AppendChild(nodo)
                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "idIstanzaWFE", "")
                nodo.InnerText = Me.Doc_id_WFE
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "datacreazione", "")
                nodo.InnerText = Me.Doc_Data
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "utentecreazione", "")
                nodo.InnerText = Me.Doc_utenteCreazione
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "ufficiodescrizione", "")
                nodo.InnerText = Me.Doc_Descrizione_ufficio
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "numeroprovvisorio", "")
                nodo.InnerText = Me.Doc_numeroProvvisorio
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "TipoDocumento", "")
                nodo.InnerText = Me.Doc_Tipo
                xmlDocument.FirstChild.AppendChild(nodo)

                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "descTipoDocumento", "")
                Select Case Me.Doc_Tipo
                    Case 0
                        nodo.InnerText = " la determina "
                    Case 1
                        nodo.InnerText = " la delibera "
                    Case 2
                        nodo.InnerText = " la disposizione "
                    Case 3
                        nodo.InnerText = " il decreto "
                    Case 4
                        nodo.InnerText = " l'ordinanza "
                End Select
                xmlDocument.FirstChild.AppendChild(nodo)

                
                nodo = xmlDocument.CreateNode(System.Xml.XmlNodeType.Element, "oggetto", "")
                nodo.InnerText = Me.Doc_Oggetto
                xmlDocument.FirstChild.AppendChild(nodo)

            Catch ex As Exception

            End Try
            Return xmlDocument
        End Function

        Public Sub New()
        End Sub

        Public Sub New(ByVal id As String)
            Doc_id = id
        End Sub
    End Class
End Namespace
