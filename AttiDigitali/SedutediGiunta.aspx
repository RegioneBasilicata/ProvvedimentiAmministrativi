<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SedutediGiunta.aspx.vb" Inherits="AttiDigitali.SedutediGiunta"%>
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
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:PlaceHolder id="Testata" runat="server"></asp:PlaceHolder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:PlaceHolder id="Albero" runat="server"></asp:PlaceHolder></td>
					<td class="pagina" width="75%">
						<asp:PlaceHolder id="Contenuto" runat="server"></asp:PlaceHolder></td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<asp:Panel id="pnlSeduta" runat="server">
				<table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
					<tr>
						<td>Selezione Seduta&nbsp;
							<asp:DropDownList id="ddlElencoSedute" runat="server"></asp:DropDownList></td>
						<td>
							<asp:Button id="btnAggiungiAllaSeduta" runat="server" Text="Aggiungi alla seduta"></asp:Button><br />
							<asp:Button id="btnRimuoviDallaSeduta" runat="server" Text="Rimuovi dalla Seduta"></asp:Button></td>
					</tr>
					<tr>
						<td>Aggiungi nuova seduta
							<asp:TextBox id="txtNuovaSeduta" runat="server"></asp:TextBox></td>
						<td>
							<asp:Button id="btnNuovaSeduta" runat="server" Text="Nuova Seduta"></asp:Button></td>
					</tr>
				</table>
			</asp:Panel>
		</form>
	</body>
</html>
