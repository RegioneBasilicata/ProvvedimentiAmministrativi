<%@ Register TagPrefix="search" TagName="search" src="search.ascx"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MonitorArchivio.aspx.vb" Inherits="AttiDigitali.MonitorArchivio" %>
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
		
		
	<body>
		<form id="form1" method="post" runat="server">
			<table id="Table1" class="pagina" cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina">
					<div  class="PrintClass">
						<asp:PlaceHolder id="Testata" runat="server"></asp:PlaceHolder>
					</div>
					</td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<div  class="PrintClass">
						<asp:PlaceHolder id="Albero" runat="server"></asp:PlaceHolder>
					</div>
					</td>
					<td class="pagina" width="75%">
						<asp:PlaceHolder id="Contenuto" runat="server"></asp:PlaceHolder>
						<div  class="PrintClass">
						<asp:placeholder  id="PannelloRicerca" runat="server">
							<search:search id="ricerca" runat="server"></search:search>
						</asp:placeholder>
						</div>
					</td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr >
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
		</form>
	</body>
</html>
