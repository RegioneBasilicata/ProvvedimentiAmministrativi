<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login_CF_IMS.aspx.vb" Inherits="AttiDigitali.Login_CF_IMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
   <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
	<meta name="vs_defaultClientScript" content="JavaScript" />
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	<link href="Determine.css" type="text/css" rel="stylesheet" />
	<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <script src="ext/LoginCFIMS.js" type="text/javascript"></script>
    
    </head>
<body >

			<table id="Table1"  style="height:500px" class="pagina" style cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina" width="1024px">
						<asp:placeholder  id="Testata" runat="server">
						
						
						</asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder Visible=false id="Albero" runat="server">
						</asp:placeholder>
					</td>
					<td class="pagina" style="height:100%"   width="75%"><div id="login"></div>
						<asp:placeholder Visible=false id="Contenuto" runat="server">
						</asp:placeholder>
					</td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td  class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>

<form    runat="server">
<asp:HiddenField ID="hiddenCF" Value="1" runat="server" />
<asp:HiddenField ID="flagNascondiCF" Value="1" runat="server" />
<asp:Label id="lblEsitoAutenticazione"  width="330" runat="server" Height="64px" CssClass="lbl"></asp:Label>
</form>
	</body></html>
