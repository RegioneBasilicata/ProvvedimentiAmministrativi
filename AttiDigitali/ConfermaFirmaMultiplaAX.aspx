<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfermaFirmaMultiplaAX.aspx.vb" Inherits="AttiDigitali.ConfermaFirmaMultiplaAX"%>
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
		<script language="JavaScript" src="FirmaDocumentiMultipli.js"></script>
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<FORM id="form1" onsubmit="firmaDocumentiMultipli()" runat="server">
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
							<td>L'utente è a conoscenza chiaramente e senza ambiguità dei dati contenuti<br/>
								nei presenti documenti e dichiara espressamente di voler firmare.</td>
						</tr>
					</table>
				</asp:panel></P>
			<P><asp:panel id="pnlBottoni" runat="server" Visible="False">
					<P>&nbsp;</P>
					<P>
						<asp:button id="btnContinua2" runat="server" Text="Firma"></asp:button>
						<asp:button id="btnAnnulla2" runat="server" Text="Abbandona"></asp:button></P>
				</asp:panel>
			<P></P>
			<P>
				<INPUT id="TxtTitolareNome" runat="server" name="TxtTitolareNome" type="hidden">
				<INPUT id="TxtTitolareCognome" runat="server" name="TxtTitolareCognome" type="hidden"></P>
			<P>&nbsp;</P>
			<P></P>
			<table align="left">
			    <tr>
			        <td align="left">
                    <asp:Label id="LabelErrore" CssClass="lblWarning" Runat="server" visible="False"></asp:Label>
			        </td>
			    </tr>
			</table>
		</FORM>
	</body>
</html>
