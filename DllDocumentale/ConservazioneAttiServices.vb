﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Il codice è stato generato da uno strumento.
'     Versione runtime:2.0.50727.8825
'
'     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
'     il codice viene rigenerato.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



<System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.ServiceContractAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiServices/200911", ConfigurationName:="ConservazioneAttiPortType")>  _
Public Interface ConservazioneAttiPortType
    
    'CODEGEN: Generazione di un contratto di messaggio perché l'operazione AddMetadataDocumentoAmministrativo non è RPC né incapsulata da documenti.
    <System.ServiceModel.OperationContractAttribute(Action:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiServices/200911/"& _ 
        "AddMetadataDocumentoAmministrativo", ReplyAction:="*"),  _
     System.ServiceModel.XmlSerializerFormatAttribute()>  _
    Function AddMetadataDocumentoAmministrativo(ByVal request As AddMetadataDocumentoAmministrativoRequest) As AddMetadataDocumentoAmministrativoResponse
    
    'CODEGEN: Generazione di un contratto di messaggio perché l'operazione GetMarcaTemporale non è RPC né incapsulata da documenti.
    <System.ServiceModel.OperationContractAttribute(Action:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiServices/200911/"& _ 
        "GetMarcaTemporale", ReplyAction:="*"),  _
     System.ServiceModel.XmlSerializerFormatAttribute()>  _
    Function GetMarcaTemporale(ByVal request As GetMarcaTemporaleRequest) As GetMarcaTemporaleResponse
    
    'CODEGEN: Generazione di un contratto di messaggio perché l'operazione SendAllegatiAttoToDocumentoConservazione non è RPC né incapsulata da documenti.
    <System.ServiceModel.OperationContractAttribute(Action:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiServices/200911/"& _ 
        "SendAllegatiAttoToDocumentoConservazione", ReplyAction:="*"),  _
     System.ServiceModel.XmlSerializerFormatAttribute()>  _
    Function SendAllegatiAttoToDocumentoConservazione(ByVal request As SendAllegatiAttoToDocumentoConservazioneRequest) As SendAllegatiAttoToDocumentoConservazioneResponse
End Interface

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class AddMetadataDocumentoAmministrativoRequestType
    
    Private idFoldereComunicazioneAlfrescoField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=0)>  _
    Public Property IdFoldereComunicazioneAlfresco() As String
        Get
            Return Me.idFoldereComunicazioneAlfrescoField
        End Get
        Set
            Me.idFoldereComunicazioneAlfrescoField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class SendAllegatiAttoToDocumentoConservazioneResponseType
    
    Private statoConservazioneField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>  _
    Public Property statoConservazione() As String
        Get
            Return Me.statoConservazioneField
        End Get
        Set
            Me.statoConservazioneField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class SendAllegatiAttoToDocumentoConservazioneRequestType
    
    Private idAttoField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=0)>  _
    Public Property IdAtto() As String
        Get
            Return Me.idAttoField
        End Get
        Set
            Me.idAttoField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class GetMarcaTemporaleResponseType
    
    Private marcaTemporaleFileField() As Byte
    
    Private timeStampCreataField As Date
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary", Order:=0)>  _
    Public Property MarcaTemporaleFile() As Byte()
        Get
            Return Me.marcaTemporaleFileField
        End Get
        Set
            Me.marcaTemporaleFileField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=1)>  _
    Public Property TimeStampCreata() As Date
        Get
            Return Me.timeStampCreataField
        End Get
        Set
            Me.timeStampCreataField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class GetMarcaTemporaleRequestType
    
    Private fileDaMarcareField() As Byte
    
    Private idAttoField As String
    
    Private operatoreField As String
    
    Private applicationIDField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary", Order:=0)>  _
    Public Property FileDaMarcare() As Byte()
        Get
            Return Me.fileDaMarcareField
        End Get
        Set
            Me.fileDaMarcareField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=1)>  _
    Public Property IdAtto() As String
        Get
            Return Me.idAttoField
        End Get
        Set
            Me.idAttoField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=2)>  _
    Public Property Operatore() As String
        Get
            Return Me.operatoreField
        End Get
        Set
            Me.operatoreField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=3)>  _
    Public Property applicationID() As String
        Get
            Return Me.applicationIDField
        End Get
        Set
            Me.applicationIDField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "3.0.4506.2152"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911")>  _
Partial Public Class AddMetadataDocumentoAmministrativoResponseType
    
    Private esitoField As String
    
    Private statoField As String
    
    Private descrizioneErroreField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=0)>  _
    Public Property Esito() As String
        Get
            Return Me.esitoField
        End Get
        Set
            Me.esitoField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=1)>  _
    Public Property Stato() As String
        Get
            Return Me.statoField
        End Get
        Set
            Me.statoField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(Order:=2)>  _
    Public Property DescrizioneErrore() As String
        Get
            Return Me.descrizioneErroreField
        End Get
        Set
            Me.descrizioneErroreField = value
        End Set
    End Property
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class AddMetadataDocumentoAmministrativoRequest
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="AddMetadataDocumentoAmministrativoRequest", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public AddMetadataDocumentoAmministrativoRequest1 As AddMetadataDocumentoAmministrativoRequestType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal AddMetadataDocumentoAmministrativoRequest1 As AddMetadataDocumentoAmministrativoRequestType)
        MyBase.New
        Me.AddMetadataDocumentoAmministrativoRequest1 = AddMetadataDocumentoAmministrativoRequest1
    End Sub
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class AddMetadataDocumentoAmministrativoResponse
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="AddMetadataDocumentoAmministrativoResponse", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public AddMetadataDocumentoAmministrativoResponse1 As AddMetadataDocumentoAmministrativoResponseType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal AddMetadataDocumentoAmministrativoResponse1 As AddMetadataDocumentoAmministrativoResponseType)
        MyBase.New
        Me.AddMetadataDocumentoAmministrativoResponse1 = AddMetadataDocumentoAmministrativoResponse1
    End Sub
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class GetMarcaTemporaleRequest
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="GetMarcaTemporaleRequest", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public GetMarcaTemporaleRequest1 As GetMarcaTemporaleRequestType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal GetMarcaTemporaleRequest1 As GetMarcaTemporaleRequestType)
        MyBase.New
        Me.GetMarcaTemporaleRequest1 = GetMarcaTemporaleRequest1
    End Sub
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class GetMarcaTemporaleResponse
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="GetMarcaTemporaleResponse", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public GetMarcaTemporaleResponse1 As GetMarcaTemporaleResponseType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal GetMarcaTemporaleResponse1 As GetMarcaTemporaleResponseType)
        MyBase.New
        Me.GetMarcaTemporaleResponse1 = GetMarcaTemporaleResponse1
    End Sub
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class SendAllegatiAttoToDocumentoConservazioneRequest
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="SendAllegatiAttoToDocumentoConservazioneRequest", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public SendAllegatiAttoToDocumentoConservazioneRequest1 As SendAllegatiAttoToDocumentoConservazioneRequestType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal SendAllegatiAttoToDocumentoConservazioneRequest1 As SendAllegatiAttoToDocumentoConservazioneRequestType)
        MyBase.New
        Me.SendAllegatiAttoToDocumentoConservazioneRequest1 = SendAllegatiAttoToDocumentoConservazioneRequest1
    End Sub
End Class

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0"),  _
 System.ServiceModel.MessageContractAttribute(IsWrapped:=false)>  _
Partial Public Class SendAllegatiAttoToDocumentoConservazioneResponse
    
    <System.ServiceModel.MessageBodyMemberAttribute(Name:="SendAllegatiAttoToDocumentoConservazioneResponse", [Namespace]:="http://www.intemaweb.com/egov/conservazioneatti/ConservazioneAttiTypes/200911", Order:=0)>  _
    Public SendAllegatiAttoToDocumentoConservazioneResponse1 As SendAllegatiAttoToDocumentoConservazioneResponseType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal SendAllegatiAttoToDocumentoConservazioneResponse1 As SendAllegatiAttoToDocumentoConservazioneResponseType)
        MyBase.New
        Me.SendAllegatiAttoToDocumentoConservazioneResponse1 = SendAllegatiAttoToDocumentoConservazioneResponse1
    End Sub
End Class

<System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")>  _
Public Interface ConservazioneAttiPortTypeChannel
    Inherits ConservazioneAttiPortType, System.ServiceModel.IClientChannel
End Interface

<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")>  _
Partial Public Class ConservazioneAttiPortTypeClient
    Inherits System.ServiceModel.ClientBase(Of ConservazioneAttiPortType)
    Implements ConservazioneAttiPortType
    
    Public Sub New()
        MyBase.New
    End Sub
    
    Public Sub New(ByVal endpointConfigurationName As String)
        MyBase.New(endpointConfigurationName)
    End Sub
    
    Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
        MyBase.New(endpointConfigurationName, remoteAddress)
    End Sub
    
    Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
        MyBase.New(endpointConfigurationName, remoteAddress)
    End Sub
    
    Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
        MyBase.New(binding, remoteAddress)
    End Sub
    
    <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Function ConservazioneAttiPortType_AddMetadataDocumentoAmministrativo(ByVal request As AddMetadataDocumentoAmministrativoRequest) As AddMetadataDocumentoAmministrativoResponse Implements ConservazioneAttiPortType.AddMetadataDocumentoAmministrativo
        Return MyBase.Channel.AddMetadataDocumentoAmministrativo(request)
    End Function
    
    Public Function AddMetadataDocumentoAmministrativo(ByVal AddMetadataDocumentoAmministrativoRequest1 As AddMetadataDocumentoAmministrativoRequestType) As AddMetadataDocumentoAmministrativoResponseType
        Dim inValue As AddMetadataDocumentoAmministrativoRequest = New AddMetadataDocumentoAmministrativoRequest
        inValue.AddMetadataDocumentoAmministrativoRequest1 = AddMetadataDocumentoAmministrativoRequest1
        Dim retVal As AddMetadataDocumentoAmministrativoResponse = CType(Me,ConservazioneAttiPortType).AddMetadataDocumentoAmministrativo(inValue)
        Return retVal.AddMetadataDocumentoAmministrativoResponse1
    End Function
    
    <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Function ConservazioneAttiPortType_GetMarcaTemporale(ByVal request As GetMarcaTemporaleRequest) As GetMarcaTemporaleResponse Implements ConservazioneAttiPortType.GetMarcaTemporale
        Return MyBase.Channel.GetMarcaTemporale(request)
    End Function
    
    Public Function GetMarcaTemporale(ByVal GetMarcaTemporaleRequest1 As GetMarcaTemporaleRequestType) As GetMarcaTemporaleResponseType
        Dim inValue As GetMarcaTemporaleRequest = New GetMarcaTemporaleRequest
        inValue.GetMarcaTemporaleRequest1 = GetMarcaTemporaleRequest1
        Dim retVal As GetMarcaTemporaleResponse = CType(Me,ConservazioneAttiPortType).GetMarcaTemporale(inValue)
        Return retVal.GetMarcaTemporaleResponse1
    End Function
    
    <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Function ConservazioneAttiPortType_SendAllegatiAttoToDocumentoConservazione(ByVal request As SendAllegatiAttoToDocumentoConservazioneRequest) As SendAllegatiAttoToDocumentoConservazioneResponse Implements ConservazioneAttiPortType.SendAllegatiAttoToDocumentoConservazione
        Return MyBase.Channel.SendAllegatiAttoToDocumentoConservazione(request)
    End Function
    
    Public Function SendAllegatiAttoToDocumentoConservazione(ByVal SendAllegatiAttoToDocumentoConservazioneRequest1 As SendAllegatiAttoToDocumentoConservazioneRequestType) As SendAllegatiAttoToDocumentoConservazioneResponseType
        Dim inValue As SendAllegatiAttoToDocumentoConservazioneRequest = New SendAllegatiAttoToDocumentoConservazioneRequest
        inValue.SendAllegatiAttoToDocumentoConservazioneRequest1 = SendAllegatiAttoToDocumentoConservazioneRequest1
        Dim retVal As SendAllegatiAttoToDocumentoConservazioneResponse = CType(Me,ConservazioneAttiPortType).SendAllegatiAttoToDocumentoConservazione(inValue)
        Return retVal.SendAllegatiAttoToDocumentoConservazioneResponse1
    End Function
End Class
