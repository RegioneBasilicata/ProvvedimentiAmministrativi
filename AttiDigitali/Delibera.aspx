<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Delibera.aspx.vb" Inherits="AttiDigitali.Delibera"%>
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
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
						<P>
							<asp:placeholder id="Contenuto" runat="server"></asp:placeholder></P>
						<P>&nbsp;</P>
					</td>
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
			<asp:Panel id="pnlDatiDelibera" runat="server" Width="550px">
				<P>Oggetto</P>
				<P>
					<asp:TextBox id="txtOggetto" runat="server" Width="536px" Rows="5" TextMode="MultiLine"></asp:TextBox>
					<asp:RequiredFieldValidator id="ReqTxtOggetto" Runat="server" CssClass="lblErrore" ErrorMessage="Campo Oggetto Obbligatorio"
						ControlToValidate="txtOggetto"></asp:RequiredFieldValidator></P>
				<P>
					<table class="determina" id="Table3" style="WIDTH: 550px" cellspacing="0" cellpadding="0"
						border="0">
						<tr>
							<td colspan="3">La presente deliberazione comporta impegno contabile sul bilancio
								<asp:TextBox id="txt_Dbi_Bilancio1" runat="server" Width="96px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
						<tr>
							<td>UPB
								<asp:TextBox id="txt_Dbi_UPB1" runat="server" Width="94px" CssClass="sololinea"></asp:TextBox></td>
							<td>Cap.
								<asp:TextBox id="txt_Dbi_Cap1" runat="server" Width="127px" CssClass="sololinea"></asp:TextBox></td>
							<td>per €
								<asp:TextBox id="txt_Dbi_Costo1" runat="server" Width="128px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
					</table>
					<table class="determina" id="Table2" style="WIDTH: 550px" cellspacing="0" cellpadding="0"
						border="0">
						<tr>
							<td colspan="4">UFFICIO RAGIONERIA GENERALE</td>
						</tr>
						<tr>
							<td>Prenotazione di impegno N°
								<asp:TextBox id="txt_DRP_NImpegno1" runat="server" Width="52px" CssClass="sololinea"></asp:TextBox></td>
							<td>UPB
								<asp:TextBox id="txt_DRP_UPB1" runat="server" Width="56px" CssClass="sololinea"></asp:TextBox></td>
							<td>Cap.
								<asp:TextBox id="txt_DRP_Cap1" runat="server" Width="57px" CssClass="sololinea"></asp:TextBox></td>
							<td>per €
								<asp:TextBox id="txt_DRP_Costo1" runat="server" Width="103px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
						<tr>
							<td colspan="2">Assunto impegno contabile N°
								<asp:TextBox id="txt_DRA_NContabile1" runat="server" Width="99px" CssClass="sololinea"></asp:TextBox></td>
							<td>UPB
								<asp:TextBox id="txt_DRA_UPB1" runat="server" Width="58px" CssClass="sololinea"></asp:TextBox></td>
							<td>Cap.
								<asp:TextBox id="txt_DRA_Cap1" runat="server" Width="99px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
						<tr>
							<td>Esercizio
								<asp:TextBox id="txt_DRA_Esercizio1" runat="server" Width="104px" CssClass="sololinea"></asp:TextBox></td>
							<td colspan="3">per €
								<asp:TextBox id="txt_DRA_Costo1" runat="server" Width="96px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
						<tr>
							<td>La liquidazione di €
								<asp:TextBox id="txt_DRL_Costo1" runat="server" Width="88px" CssClass="sololinea"></asp:TextBox></td>
							<td>UPB
								<asp:TextBox id="txt_DRL_UPB1" runat="server" Width="58px" CssClass="sololinea"></asp:TextBox></td>
							<td>Cap.
								<asp:TextBox id="txt_DRL_Cap1" runat="server" Width="59px" CssClass="sololinea"></asp:TextBox></td>
							<td>Esercizio
								<asp:TextBox id="txt_DRL_Esercizio1" runat="server" Width="79px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
						<tr>
							<td>rientra nell'ambito dell'imp N°
								<asp:TextBox id="txt_DRL_NContabile1" runat="server" Width="42px" CssClass="sololinea"></asp:TextBox></td>
							<td colspan="2">assunto con delibera N°
								<asp:TextBox id="txt_DRL_NAssunzione1" runat="server" Width="48px" CssClass="sololinea"></asp:TextBox></td>
							<td>del
								<asp:TextBox id="txt_DRL_Data1" runat="server" Width="82px" CssClass="sololinea"></asp:TextBox></td>
						</tr>
					</table>
				<P align="center">
					<asp:Panel id="pnlPubblicazione" runat="server" Width="536px">Atto 
soggetto a pubblicazione 
<asp:DropDownList id="ddlTipoPubblicazione" runat="server"></asp:DropDownList><br/></asp:Panel>
					<asp:Button id="btnRegistra" runat="server" Text="Registra"></asp:Button></P>
			</asp:Panel>
		</form>
	</body>
</html>
