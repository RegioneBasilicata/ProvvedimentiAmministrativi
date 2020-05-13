<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SceltaOperatore.aspx.vb" Inherits="AttiDigitali.SceltaOperatore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
   <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
       <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
        <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
   
        <script src="ext/onReadySceltaOperatore.js?scriptversion=20200331" type="text/javascript"></script>
       
</head>
<body>
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server" Visible=false></asp:placeholder></td>
					<td class="pagina" width="75%">
					<asp:placeholder id="Contenuto" runat="server"></asp:placeholder>
					   
					    <div id="FiltroStruttura"></div>
					    <div id="Lista"></div>
					        <div id="MyPanel"></div>
					   
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
			<form runat="server">
			    <asp:HiddenField ID="scelte" runat="server" />
			    
			        <asp:HiddenField ID="hiddenCF" runat="server" />
			</form>
	</body>
</html>
