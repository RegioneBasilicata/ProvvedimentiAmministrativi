<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisibilitaSic.aspx.vb" Inherits="AttiDigitali.VisibilitaSic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
   <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="Determine.css" type="text/css" rel="stylesheet" />  
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
        <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
        <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
        <script src="ext/Ext.ux.YearPicker.js" type="text/javascript"></script>  
        <script src="ext/onReadyDisponibilita.js" type="text/javascript"></script>
        <script src="ext/utilityNumber.js" type="text/javascript"></script>
        <script src="ext/PanelFiltroStruttura.js" type="text/javascript"></script> 
        <script src="ext/DisponibilitaPreImpegni.js?scriptversion=20200331" type="text/javascript"></script>
        <script src="ext/DisponibilitaImpegni_20170913.js?scriptversion=20200331" type="text/javascript"></script>
        <script src="ext/DisponibilitaLiquidazioni.js?scriptversion=20200331" type="text/javascript"></script>
        <script src="ext/ListaCapitoli.js?scriptversion=20200331" type="text/javascript"></script>
</head>
<body>

			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
					<asp:placeholder id="Contenuto" runat="server"></asp:placeholder>
					   
					    <div id="FiltroStruttura"></div>
					    <div id="Lista"></div>
					   
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
			    <asp:HiddenField ID="Dipartimento" runat="server" />
			    <asp:HiddenField ID="Ufficio" runat="server" />
			    <asp:HiddenField ID="Visualizza" runat="server" />
			    <asp:HiddenField ID="Cod_uff_Prop" runat="server" />
			</form>
	</body>
</html>
