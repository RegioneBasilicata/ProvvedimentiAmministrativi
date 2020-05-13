<%@ Page Language="vb" AutoEventWireup="false" Codebehind="login.aspx.vb" Inherits="AttiDigitali.login"%>
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
		<link href="Determine.css" type="text/css" rel="stylesheet" />
	<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <script src="ext/LoginCFIMS.js" type="text/javascript"></script>
    
    </head>
<body >

			<table id="Table1"  style="height:500px" class="pagina"  cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina" width="1024px">
						<asp:placeholder  id="Testata" runat="server">
						
						
						</asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder Visible="false" id="Albero" runat="server">
						</asp:placeholder>
					</td>
					<td class="pagina" style="height:100%"   width="75%">
					    <div id="login" style="vertical-align:middle"></div>
						<asp:placeholder Visible=false id="Contenuto" runat="server">
						
						</asp:placeholder>
					</td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header" ><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td  class="x-panel-header" width="75%"><div></div></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>


<%--<form id="myPanel"  name="myPanel" >
<input id="TextBox1" name="codUtente"   />
</form>--%>
<form id="Form1"   runat="server">
<asp:HiddenField ID="hiddenCF" Value="1" runat="server" />
<%--<asp:TextBox id="codUtente" name="codUtente"   runat="server"/>
--%><asp:HiddenField ID="flagNascondiCF" Value="1" runat="server" />
<asp:Label id="lblEsitoAutenticazione"  width="330" runat="server" Height="64px" CssClass="lbl"></asp:Label>
</form>
	</body></html>