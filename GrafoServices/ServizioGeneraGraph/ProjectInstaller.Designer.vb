<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione componenti
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione componenti
    'Può essere modificata in Progettazione componenti.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.process = New System.ServiceProcess.ServiceProcessInstaller
        Me.installer = New System.ServiceProcess.ServiceInstaller
        '
        'process
        '
        Me.process.Account = System.ServiceProcess.ServiceAccount.LocalService
        Me.process.Password = Nothing
        Me.process.Username = Nothing
        '
        'installer
        '
        Me.installer.Description = "GestioneGraph"
        Me.installer.DisplayName = "GestioneGraph"
        Me.installer.ServiceName = "GestioneGraph"
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.process, Me.installer})

    End Sub
    Friend WithEvents process As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents installer As System.ServiceProcess.ServiceInstaller

End Class
