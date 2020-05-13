<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="BeneficiariFromFile.aspx.vb" Inherits="AttiDigitali.BeneficiariFromFile" ValidateRequest="False" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Sistema di Gestione Atti Amministrativi</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="Determine.css" type="text/css" rel="stylesheet" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />

    <link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
</head>
<body>

    <form id="Form1" method="post" runat="server">
        <table class="pagina" cellspacing="0" cellpadding="0">
            <tr>
                <td class="pagina" colspan="2">
                    <asp:PlaceHolder ID="Testata" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td class="pagina" width="25%">
                    <asp:PlaceHolder ID="Albero" runat="server"></asp:PlaceHolder>
                </td>
                <td class="pagina" width="75%">
                    <asp:PlaceHolder ID="Contenuto" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <!-- 'modgg 10-06 9 INIZIO -->
            <tr>
                <td width="25%">
                    <div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
                </td>
                <td class="x-panel-header" width="75%"></td>
            </tr>
            <!-- 'modgg 10-06 9 FINE -->
        </table>
        <%--<p><asp:Button ID="btnSalvaDocumento" runat="server" Visible="False" Text="Registra" CssClass="btn"></asp:Button></p>--%>
        
        <p><asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></p>
        <p><asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label></p>
        <p>&nbsp;</p>
        <asp:Panel ID="pnlAggiungiAllegato" runat="server" Height="24px" HorizontalAlign="Center" Width="750px">
            <p><asp:Label ID="Label3" Text="Il tracciato excel è possibile scaricarlo dal menù, alla voce 'Scarica Modelli'" runat="server"></asp:Label></p>
            <hr />
            <table style="margin-left: 70px; margin-top: 10px; margin-bottom: 20px">
                <tr>
                    <td colspan="2">
                        <div style="height: 10px"></div>
                    </td>
                </tr>
                <tr align="center" style="margin-bottom: 10px; margin-top: 10px;">
                    <td colspan="2"><b>Seleziona il file</b></td>
                </tr>
                <tr>
                    <td>File: </td>
                    <td>
                        <input id="fileUpLoadAllegato" type="file" runat="server" name="fileUpLoadAllegato" /></td>
                </tr>
                <tr align="center">
                    <td style="align-content: center" colspan="2">
                        <asp:Button ID="btnSalvaDocumento" runat="server" Width="83px" Text="Salva" CssClass="btn"></asp:Button></td>
                </tr>
            </table>

            <hr />
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server" Height="24px" Width="750px">
            <asp:BulletedList ID="BulletedList" runat="Server" BulletStyle="Disc"></asp:BulletedList>
        </asp:Panel>
        
    </form>
</body>
</html>
