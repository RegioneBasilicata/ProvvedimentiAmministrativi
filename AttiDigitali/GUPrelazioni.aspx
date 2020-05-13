<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GUPrelazioni.aspx.vb" Inherits="AttiDigitali.GUPrelazioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
  	<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table id="Table1" class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td colspan="2" class="pagina"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%"><asp:placeholder id="Contenuto" runat="server"></asp:placeholder></td>
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
			<asp:Button cssclass="btn" id="btnPrelaziona" runat="server" Text="Prelaziona" Width="104px"></asp:Button>
			<asp:Panel id="pnlNote" runat="server">
<P>Messaggio per l'utente che ha in carico il&nbsp;documento</P>
<P>
<asp:TextBox id=txtNote runat="server" TextMode="MultiLine" Rows="10" Columns="65"></asp:TextBox></P>
			</asp:Panel></form>
	</body>
</html>
