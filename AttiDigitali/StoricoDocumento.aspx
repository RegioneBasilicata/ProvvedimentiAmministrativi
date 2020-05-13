<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StoricoDocumento.aspx.vb" Inherits="AttiDigitali.StoricoDocumento"%>
<%@ OutputCache Duration="1" VaryByParam="none" %>

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
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" />
		
		<link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
        <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<script src="ext/GroupSummary.js" type="text/javascript"></script>  
		<script src="ext/PopupDettaglioStorico.js" type="text/javascript"></script>
		 
		      
		
</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table1" class="pagina" cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina">
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
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
		</form>
	</body>
</html>
