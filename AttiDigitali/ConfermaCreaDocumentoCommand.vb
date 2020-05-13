Public Class ConfermaCreaDocumentoCommand
    Inherits RedirectingCommand

    Protected Overrides Sub OnExecute(ByVal context As HttpContext)
        Dim tipoApplic As String = CStr(context.Request.Params.Item("tipo"))

        context.Items.Add("tipoApplic", tipoApplic)
       
        '  context.Items.Add("opSoggettoPor", opSoggettoPor)
    End Sub
End Class

