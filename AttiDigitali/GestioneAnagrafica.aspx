<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestioneAnagrafica.aspx.vb" Inherits="AttiDigitali.GestioneAnagrafica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <link href="Determine.css" type="text/css" rel="stylesheet" />
	<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/utilityNumber.js" type="text/javascript"></script>
    <script src="ext/CheckColumn.js" type="text/javascript"></script>
   <script src="ext/LinkButton.js" type="text/javascript"></script>
   <script src="ext/CRUDAnagraficaBeneficiari_20150325.js?scriptversion=20200331" type="text/javascript"></script>
   <script src="ext/BeneficiariSearchPanel_20170913.js" type="text/javascript"></script>
   <script src="ext/GestioneAnagrafica_20150325.js" type="text/javascript"></script>
   <script src="ext/OnReadyGestioneAnagrafica.js" type="text/javascript"></script>
   <script src="ext/IBANChecker.js" type="text/javascript"></script>
</head>
<body>
      
       <table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="20%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="80%">
					<asp:placeholder id="Contenuto" runat="server"></asp:placeholder>
					       <div id="myPanelPrincipale"></div>
                     </td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td width="20%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<form  runat="server">
			    <asp:HiddenField ID="valueUffProp" runat="server" />
			    
			</form>
       
       
              
</body>
</html>
