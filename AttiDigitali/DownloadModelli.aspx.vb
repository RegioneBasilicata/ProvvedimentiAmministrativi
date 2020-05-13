Public Partial Class DownloadModelli
    Inherits System.Web.UI.Page

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()

        Inizializza_Pagina(Me, "Scarica Modelli")

        Dim immagine As String = ".\risorse\immagini\Download-Icon1.png"

        'DETERMINE
        lblDetermina.Text = "Determina"
        hyperLinkDetermina.NavigateUrl = ".\risorse\Modelli_20150216\Determina.zip"
        hyperLinkDetermina.ImageUrl = immagine

        lblDetermina_dg.Text = "Determina Dirigente Generale"
        hyperLinkDetermina_dg.NavigateUrl = ".\risorse\Modelli_20150216\Determina_dg.zip"
        hyperLinkDetermina_dg.ImageUrl = immagine

        lblDetermina_CICO.Text = "Determina CICO"
        hyperLinkDetermina_CICO.NavigateUrl = ".\risorse\Modelli_20150216\Determina_CICO.zip"
        hyperLinkDetermina_CICO.ImageUrl = immagine

        lblDetermina_CICO_pres.Text = "Determina Presidente CICO"
        hyperLinkDetermina_CICO_pres.NavigateUrl = ".\risorse\Modelli_20150216\Determina_CICO_pres.zip"
        hyperLinkDetermina_CICO_pres.ImageUrl = immagine

        lblDetermina_StrutturePresidente.Text = "Determina Strutture del Presidente"
        hyperLinkDetermina_StrutturePresidente.NavigateUrl = ".\risorse\Modelli_20150216\Determina_StrutturePresidente.zip"
        hyperLinkDetermina_StrutturePresidente.ImageUrl = immagine

        'DISPOSIZIONI
        lblDisposizione.Text = "Disposizione"
        hyperLinkDisposizione.NavigateUrl = ".\risorse\Modelli_20150216\Disposizione.zip"
        hyperLinkDisposizione.ImageUrl = immagine

        lblDisposizione_dg.Text = "Disposizione Dirigente Generale"
        hyperLinkDisposizione_dg.NavigateUrl = ".\risorse\Modelli_20150216\Disposizione_dg.zip"
        hyperLinkDisposizione_dg.ImageUrl = immagine

        lblDisposizione_CICO.Text = "Disposizione CICO"
        hyperLinkDisposizione_CICO.NavigateUrl = ".\risorse\Modelli_20150216\Disposizione_CICO.zip"
        hyperLinkDisposizione_CICO.ImageUrl = immagine

        lblDisposizione_CICO_pres.Text = "Disposizione Presidente CICO"
        hyperLinkDisposizione_CICO_pres.NavigateUrl = ".\risorse\Modelli_20150216\Disposizione_CICO_pres.zip"
        hyperLinkDisposizione_CICO_pres.ImageUrl = immagine

        lblDisposizione_StrutturePres.Text = "Disposizione Strutture del Presidente"
        hyperLinkDisposizione_StrutturePres.NavigateUrl = ".\risorse\Modelli_20150216\DisposizionePresidente.zip"
        hyperLinkDisposizione_StrutturePres.ImageUrl = immagine


         'DELIBERA
        'lblDelibera.Text = "Delibera"
        'hyperLinkDelibera.NavigateUrl = ".\risorse\Modelli_20150216\Delibera.zip"
        'hyperLinkDelibera.ImageUrl = immagine

        'TRACCIATO BENEFICIARI
        lblTracciatoBeneficiari.Text = "Caricamento massivo beneficiari"
        hyperLinkTracciatoBeneficiari.NavigateUrl = ".\risorse\Tracciati_excel\Tracciato_caricamento_massivo_beneficiari.zip"
        hyperLinkTracciatoBeneficiari.ImageUrl = immagine


        Contenuto.Controls.Add(pnlDownload)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        

    End Sub

End Class