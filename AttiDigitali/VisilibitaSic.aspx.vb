Imports System.Collections.Generic
Partial Public Class VisibilitaSic
    Inherits WebSession

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        Inizializza_Pagina(Me, "Disponibilità SIC")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'leggere l'ufficio e il dipartimento
        Dim operatore As DllAmbiente.Operatore = HttpContext.Current.Session.Item("oOperatore")
        
        Dim cosaVisualizzare As String = String.Empty
        cosaVisualizzare = Context.Request.QueryString.Item("vis")
        If Not String.IsNullOrEmpty(cosaVisualizzare) Then
            'distinguo se visualizzare capitoli:0, preimpegni=1, impegni=2, liquidazioni=3
            Visualizza.Value = cosaVisualizzare
        Else
            'visualizzo i capitoli
            Visualizza.Value = "0"
        End If

        'se non è appartenente ad alcun ufficio - caso ROOT- leggo il primo ufficio consultabile
        If String.IsNullOrEmpty(operatore.oUfficio.CodUfficio) Then
            Dim arraylist As ArrayList = operatore.UfficiDipendenti("0")
            If arraylist.Count > 0 Then
                Dipartimento.Value = DirectCast(arraylist.Item(0), DllAmbiente.StrutturaInfo).Padre
                Ufficio.Value = DirectCast(arraylist.Item(0), DllAmbiente.StrutturaInfo).CodiceInterno
            Else
                Throw New Exception("L'operatore non è abilitatato alla consultazione di queste informazioni")
            End If
        Else
            Dipartimento.Value = operatore.oUfficio.CodDipartimento
            Ufficio.Value = operatore.oUfficio.CodUfficio
        End If

        'se esiste la key da query string, devo leggere i dati dell 'ufficio proponente
        Dim key As String = String.Empty
        key = Context.Request.QueryString.Item("key")
        If Not String.IsNullOrEmpty(key) Then
            Dim statoIstanza As DllDocumentale.StatoIstanzaDocumentoInfo = (New DllDocumentale.svrDocumenti(operatore)).Get_StatoIstanzaDocumento(key)
            If statoIstanza.LivelloUfficio = "UR" Then
                Dim uffProponente As New DllAmbiente.Ufficio
                uffProponente.CodUfficio = statoIstanza.CodiceUfficio
                Dipartimento.Value = uffProponente.CodDipartimento
                Ufficio.Value = uffProponente.CodUfficio
            End If
            'imposto l'ufficio proponente
            Cod_uff_Prop.value = statoIstanza.CodiceUfficio
        End If
    End Sub

End Class