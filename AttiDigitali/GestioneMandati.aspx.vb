Imports System.ServiceModel
Imports DllDocumentale
Imports System.Collections.Generic
Imports System.Net

Partial Public Class GestioneMandati
    Inherits WebSession
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Dettaglio Mandati Emessi")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim key As String = Request.QueryString("key")
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        Dim objDocumento As DllDocumentale.Model.DocumentoInfo = Leggi_Documento_Object(key)

        Dim lstr_numeroDocumento As String = ""
        lstr_numeroDocumento = objDocumento.Doc_numero
        If lstr_numeroDocumento = "" Then
            lstr_numeroDocumento = objDocumento.Doc_numeroProvvisorio
        End If
        Rinomina_Pagina(Me, "Dettaglio Mandati Emessi " & lstr_numeroDocumento)

        'imposto l'ufficio proponente
        valueUffProp.Value = objDocumento.Doc_Cod_Uff_Prop
    End Sub

End Class