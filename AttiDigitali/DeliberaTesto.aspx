<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DeliberaTesto.aspx.vb" Inherits="AttiDigitali.DeliberaTesto" ValidateRequest="False"%>
<%@ Register tagPrefix="EditorTesto" tagName="EditorTesto" src="EditorTesto.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
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
			<P><asp:button id="btnSalvaTesto" runat="server" Visible="False" Text="Registra" cssclass="btn"></asp:button></P>
			<P><asp:label id="Label1" runat="server" ForeColor="Red"></asp:label>
				<asp:Label id="Label2" runat="server" ForeColor="Red" Width="56px"></asp:Label></P>
			<P>&nbsp;</P>
			<asp:panel id="pnlAggiungiAllegato" runat="server" Height="24px" HorizontalAlign="Center" Width="560px">
				<hr/>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P>&nbsp;</P>
				<P><STRONG>Allega testo&nbsp;Delibera</STRONG>:</P>
				<P>seleziona il file RTF&nbsp; <INPUT id="fileUpLoadAllegato" type="file" name="fileUpLoadAllegato" runat="server"></P>
				<P align="center"></P>
			</asp:panel>
			<P>&nbsp;</P>
		</form>
	</body>
</html>
