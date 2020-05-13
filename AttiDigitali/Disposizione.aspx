<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Disposizione.aspx.vb" Inherits="AttiDigitali.Disposizione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server">
					    	<asp:Panel   runat="server" ID="PannelloTipoContabile">
		        	 <table>
		           <tr><td><asp:CheckBox ID="chkAccertamento" CssClass="lbl"  Text="Accertamento"   onclick="javascript:gestioneCheck(this)"   runat="server" /></td></tr>
              </table>
                  </asp:Panel>
					</asp:placeholder></td>
					<td class="pagina" width="75%">
						<P><asp:placeholder id="Contenuto" runat="server"></asp:placeholder></P>
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
			
			<P>
				<asp:Button cssclass="btn" id="ButtonContinua" runat="server" Text="Registra"></asp:Button></P>
		</form>
	</body>
</html>
