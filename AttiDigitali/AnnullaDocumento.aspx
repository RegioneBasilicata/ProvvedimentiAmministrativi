<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AnnullaDocumento.aspx.vb" Inherits="AttiDigitali.AnnullaDocumento"%>
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
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon">
			<script language=javascript>
		    function disabilita() {
		        //obj.disabled = true;

		   if (window.document.getElementById('btnAnnulla') != null) {
		       window.document.getElementById('btnAnnulla').style.visibility = 'hidden'
		      
		    }
		   
		} </script>
	</head>
	<body>
		<form onsubmit="disabilita()" id="Form1" method="post" runat="server">
			<table id="Table1" class="pagina" cellpadding="0" cellspacing="0">
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
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<asp:panel id="pnlInoltro" runat="server" Height="48px" Width="576px">
				<table style="WIDTH: 576px; HEIGHT: 65px" width="576">
					<tr>
						<td width="75%">
							<P>
								<asp:Label id="LblErrore" CssClass="lblErrore" Runat="server" visible="False"></asp:Label>
								</P>
							<P>
								&nbsp;</P>
						</td>
						<td width="25%">
							&nbsp;</td>
					</tr>
				</table>
			</asp:panel>
			<asp:panel id="pnlAnnullo" runat="server" Height="48px" Width="576px">
				<table width="100%">
					<tr>
						<td width="75%">
							<asp:label id="lblAnnulla" CssClass="lbl" text="Premi annulla per annullare il documento" Runat="server">Premi annulla per annullare la </asp:label></td>
						<td width="25%">
							<asp:Button id="btnAnnulla" runat="server" cssclass="btn" Text="Annulla atto" Enabled="False"></asp:Button></td>
					</tr>
				</table>
			</asp:panel>
			<asp:Panel Visible=False id="pnlNote" runat="server">
				<P>Note</P>
				<P>
					<asp:TextBox id="txtNote" runat="server" Rows="10" Columns="65" TextMode="MultiLine"></asp:TextBox></P>
			</asp:Panel>
		</form>
	</body>
</html>
