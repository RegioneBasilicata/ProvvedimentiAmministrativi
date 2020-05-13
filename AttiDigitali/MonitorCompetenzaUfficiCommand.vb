''' <summary>
''' Questa funzione viene chiamata in Archivio e Worklist Print User
''' </summary>
''' <remarks></remarks>
Public Class MonitorCompetenzaUfficiCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim vettoredati As Object
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))
        Dim txtOggettoRicerca As String = context.Session.Item("txtOggettoRicerca")
        Dim txtCodUfficio As String = context.Session.Item("txtCodUfficio")
        Dim txtDescUfficio As String = context.Session.Item("txtDescUfficio")
        Dim urlCorrente As String = context.Request.Path
        Dim txtDataInizio As Date
        Dim txtDataFine As Date
        If Not context.Session.Item("txtDataInizio") Is Nothing Then
            txtDataInizio = context.Session.Item("txtDataInizio")
        Else
            'txtDataInizio = DateAdd(DateInterval.WeekOfYear, -1, Now)
            ' txtDataInizio = New Date(Year(Today), 1, 1)
            txtDataInizio = Now.AddMonths(-1)
        End If
        If Not context.Session.Item("txtDataFine") Is Nothing Then
            txtDataFine = context.Session.Item("txtDataFine")
        Else
            txtDataFine = DateTime.Now
        End If

        Dim flagPrinterUser As Boolean = False


        If urlCorrente.IndexOf("MonitorUfficioAction") > -1 Then
            Dim oOperatore As DllAmbiente.Operatore = HttpContext.Current.ApplicationInstance.Session.Item("oOperatore")
            txtCodUfficio = oOperatore.oUfficio.CodUfficio
        End If
        Dim txtNumero As String = "" & context.Session("txtNumero")

        If context.Session.Item("txtCodDipartimenti") Is Nothing OrElse String.IsNullOrEmpty(context.Session.Item("txtCodDipartimenti")) Then
            Dim op As DllAmbiente.Operatore = context.Session.Item("oOperatore")
            If Not op.oUfficio.CodDipartimento Is Nothing And Not String.IsNullOrEmpty(op.oUfficio.CodDipartimento) Then
                context.Session.Add("txtCodDipartimenti", op.oUfficio.CodDipartimento)
           
            End If
        End If

        Dim txtCodDipartimento As String = context.Session.Item("txtCodDipartimenti")
        Dim tipoRigetto As String = "" & context.Session("TipoRigetto")

        vettoredati = Consulta_documenti_AltriUffici(CInt(tipoApplic),, Format(txtDataInizio, "dd/MM/yyyy"), Format(txtDataFine, "dd/MM/yyyy"), txtOggettoRicerca, txtCodUfficio, txtDescUfficio, txtCodDipartimento, txtNumero, True, tipoRigetto, True)


        context.Items.Add("vettoreDati", vettoredati)
        context.Items.Add("tipoApplic", tipoApplic)
    End Sub

End Class
