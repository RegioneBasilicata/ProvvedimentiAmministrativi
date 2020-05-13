<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContabileDisp.aspx.vb" Inherits="AttiDigitali.ContabileDisp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  >
<head id="Head1" runat="server">
    <title>Sistema di Gestione Atti Amministrativi</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
	<meta name="vs_defaultClientScript" content="JavaScript" />
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	<link href="Determine.css" type="text/css" rel="stylesheet" />
	<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <script src="ext/Ext.ux.YearPicker.js" type="text/javascript"></script>  
    <script src="ext/ToolBarMenu.js" type="text/javascript"></script>
    <script src="ext/utilityNumber.js" type="text/javascript"></script>
    <script src="ext/GroupSummary.js" type="text/javascript"></script>  
    <script src="ext/LinkButton.js" type="text/javascript"></script>
    <script src="ext/OnReadyDisposizione_20150325.js" type="text/javascript"></script>
    <script src="ext/GridAggiungiLiquidazione_20180122.js?scriptversion=20200331" type="text/javascript"></script>
    <script src="ext/GridAggiungiBeneficiario_20170913.js?scriptversion=20200331" type="text/javascript"></script>
    <script src="ext/GridAccertamenti.js" type="text/javascript"></script>
    <script src="ext/PanelVisualizzaRettifica.js" type="text/javascript"></script>
    <script src="ext/RowAction.js" type="text/javascript"></script>
    <script src="ext/CRUDAnagraficaBeneficiari_20150325.js?scriptversion=20200331" type="text/javascript"></script>
    <script src="ext/BeneficiariSearchPanel_20170913.js" type="text/javascript"></script>
    <script src="ext/BeneficiarioLiquidazione_20170913.js" type="text/javascript"></script>
    <script src="ext/IBANChecker.js" type="text/javascript"></script>
    <script src="ext/CheckColumn.js" type="text/javascript"></script>    
    
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
						<div id="ToolBarMenu"></div>
						<div id="Rettifica"></div>
						<div id="ScegliCap"></div>
                        <div id="ListaCap"></div>
                        <div id="ListaLiq"></div>
                        <div id="ListaBen"></div>
                        <div id="Accertamento"></div>
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
			<form id="Form1" runat="server">
			    <asp:HiddenField ID="NPreimpReg" runat="server" />
			    <asp:HiddenField ID="NimpReg" runat="server" />
			    <asp:HiddenField ID="NliqReg" runat="server" />
			    <asp:HiddenField ID="NFattureDocumento" runat="server" />
			    <asp:HiddenField ID="isUffProp" runat="server" />
			    <asp:HiddenField ID="chkAccertamento" runat="server" />
			    <asp:HiddenField ID="chkLiquidazione" runat="server" />
			    <asp:HiddenField ID="TipoRettifiche" runat="server" />
			    <asp:HiddenField ID="LiquidazioniDaConfermare" runat="server" />
			    <asp:HiddenField ID="TestoCaricato" runat="server" />
			    <asp:HiddenField ID="Cod_uff_Prop" runat="server" />
			    <asp:HiddenField ID="codFiscOperatore" runat="server" />
                <asp:HiddenField ID="uffPubblicoOperatore" runat="server" />
			</form>
	</body>
</html>