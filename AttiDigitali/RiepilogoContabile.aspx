<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RiepilogoContabile.aspx.vb" Inherits="AttiDigitali.RiepilogoContabile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  >
<head id="Head1" runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
   <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
     <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <script src="ext/Ext.ux.YearPicker.js" type="text/javascript"></script> 
      <script src="ext/CheckColumn.js" type="text/javascript"></script> 
  
    <script src="ext/GridAccertamenti.js" type="text/javascript"></script>
    <script src="ext/GridImpegniRag_20170913.js?scriptversion=20200331" type="text/javascript"></script>
    <script src="ext/GridLiquidazioniRag_20170913.js?scriptversion=20200331" type="text/javascript"></script>
    <script src="ext/GridRiduzioneRag.js?scriptversion=20200331" type="text/javascript"></script>
    
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
					
					    <div id="ListaCapPerenti"></div>
					     <div id="ListaLiqContestualiPerenti"></div>
                        <div id="ListaCap"></div>
                        <div id="ListaLiqContestuali"></div>
                        <div id="ListaLiq"></div>
                        <div id="Accertamento"></div>
                        <div id="ListaCapRiduzione"></div>
                        <div id="ListaCapRiduzioneContestuali"></div>
                           <div id="ListaMandati"></div>
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
			  
			  
			    <asp:HiddenField ID="chkImpegno" runat="server" />
	                      <asp:HiddenField ID="chkImpegnoSuPerenti" runat="server" />
	                       <asp:HiddenField ID="chkAccertamento" runat="server" />
	                       <asp:HiddenField ID="chkRiduzione" runat="server" />
	                       <asp:HiddenField ID="chkLiquidazione" runat="server" />
                
			
			
			
			    <asp:HiddenField ID="NimpReg" runat="server" />
			    <asp:HiddenField ID="NliqReg" runat="server" />
			    <asp:HiddenField ID="NMandati" runat="server" />
			      <asp:HiddenField ID="isUffProp"
			       runat="server" />
			       <asp:HiddenField ID="IsMandato" Value="1" runat="server" />
			       
			     <asp:HiddenField ID="NridReg" runat="server" />
			 
			</form>
	</body>
</html>
