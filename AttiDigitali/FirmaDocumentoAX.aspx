<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FirmaDocumentoAX.aspx.vb" Inherits="AttiDigitali.FirmaDocumentoAX"%>
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
		<script language="JavaScript" src="FirmaDocumenti.js"></script>
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<form id="Form1" onsubmit="firmaDocumento()" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
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
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<P>&nbsp;</P>
			<P><asp:panel id="pnlFirma" runat="server">
					<table class="griglia" id="Table2" width="100%" align="center">
						<tr>
							<td>
								<P>
									<asp:HyperLink id="lnkAnteprima" runat="server" Visible="False">Anteprima</asp:HyperLink></P>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Button id="btnFirma" runat="server" Visible="False" Text="Firma" cssclass="btn"></asp:Button></td>
						</tr>
					</table>
				</asp:panel></P>
			<P> <INPUT id="hContenutoFileFirmato" type="hidden" value="-" name="hContenutoFileFirmato">
				<INPUT id="TxtTitolareNome" runat="server" name="TxtTitolareNome" type="hidden">
				<INPUT id="TxtTitolareCognome" runat="server" name="TxtTitolareCognome" type="hidden">
			</P>
		</form>
	</body>
</html>
