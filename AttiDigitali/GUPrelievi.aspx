<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GUPrelievi.aspx.vb" Inherits="AttiDigitali.GUPrelievi1"%>
<%@ Register TagPrefix="search" TagName="search" src="search.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" id="Table1" cellpadding="0" cellspacing="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
						<asp:placeholder id="Contenuto" runat="server"></asp:placeholder>
						<asp:placeholder id="PannelloRicerca" runat="server"><search:search id="ricerca" runat="server"></search:search></asp:placeholder>
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
			<asp:Button cssclass="btn" id="btnPreleva" runat="server" Text="Preleva"></asp:Button>
		</form>
	</body>
</html>
