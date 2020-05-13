Imports System.Web
Imports System.Web.SessionState

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Codice generato da Progettazione componenti "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione componenti.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Richiesto da Progettazione componenti
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione componenti.
    'Può essere modificata in Progettazione componenti.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    ''modgg 10-06 2 
    'Public hashUtenti As Hashtable
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(17, 17)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"

    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato all'avvio dell'applicazione
        ''modgg 10-06 2
        'hashUtenti = New Hashtable
        'HttpContext.Current.Application.Add("hashUtenti", hashUtenti)
        'modgg 10-06 7
        log4net.Config.XmlConfigurator.Configure()
        Application.Add("NOME_ENTE_INSTALLAZIONE", ConfigurationManager.AppSettings("NOME_ENTE_INSTALLAZIONE"))
        Application.Add("CONTABILITA", ConfigurationManager.AppSettings("CONTABILITA"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato all'apertura della sessione  
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato all'inizio di ogni richiesta
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato durante il tentativo di autenticazione dell'utente
        'modgg 10-03 eliminata prova utente
        'If Not HttpContext.Current.Session Is Nothing Then
        '    Dim ooperatore As DllAmbiente.Operatore = DirectCast(HttpContext.Current.Session.Item("ooperatore"), DllAmbiente.Operatore)
        '    Dim sessionId As String = DirectCast(HttpContext.Current.Application.Item("hashUtenti"), Hashtable).Item(ooperatore.Codice)
        '    If sessionId.Equals(HttpContext.Current.Session.SessionID) Then
        '        HttpContext.Current.Session.Abandon()
        '    End If
        'End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato in caso di errore
        ''modgg 10-06 2
        'HttpContext.Current.Application.Remove("hashUtenti")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato alla fine della sessione
        ''modgg 10-06 2
        ' Dim operatore As DllAmbiente.Operatore = DirectCast(HttpContext.Current.ApplicationInstance.Context.Session.Item("oOperatore"), DllAmbiente.Operatore)
        'If DirectCast(HttpContext.Current.Application.Item("hashUtenti"), Hashtable).Contains(operatore.Codice) Then
        'DirectCast(HttpContext.Current.Application.Item("hashUtenti"), Hashtable).Remove(operatore.Codice)
        'End If
        'HttpContext.Current.Session.Remove("oOperatore")
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato alla chiusura dell'applicazione
        ''modgg 10-06 2
        'HttpContext.Current.Application.Remove("hashUtenti")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Global_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.BeginRequest
        'CType(sender, Global).Response.Cache.SetNoStore()
        'CType(sender, Global).Response.Buffer = False
        'CType(sender, Global).Response.ExpiresAbsolute = Now.AddDays(-1)
    End Sub
End Class
