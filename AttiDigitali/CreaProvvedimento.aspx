<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreaProvvedimento.aspx.vb" Inherits="AttiDigitali.CreaProvvedimento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>Sistema di Gestione Atti Amministrativi</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <script src="ext/ToolBarMenu.js" type="text/javascript"></script>
    <script src="ext/GroupSummary.js" type="text/javascript"></script>  
    <link href="Determine.css" type="text/css" rel="stylesheet" />
	<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
    <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/LinkButton.js" type="text/javascript"></script>
    <script src="ext/ToolBarMenu.js" type="text/javascript"></script>
    <script src="ext/GroupSummary.js" type="text/javascript"></script>
     <script src="ext/Attributi20150420.js" type="text/javascript"></script>
     <script src="ext/ext-lang-it.js" type="text/javascript"></script>
     <script src="ext/CheckColumn.js" type="text/javascript"></script>
     
     <script src="ext/BeneficiariSearchPanel_20170913.js" type="text/javascript"></script>     
     <script src="ext/CRUDAnagraficaBeneficiari_20150325.js?scriptversion=20200331" type="text/javascript"></script>     
 <%--   <script src="ext/CreaProvvedimento.js" type="text/javascript"></script>   --%>
   <script src="ext/PanelComponentiUfficio.js" type="text/javascript"></script>
   <script src="ext/PanelUfficiCompetenzaDettaglioDocumento_20161220.js?scriptversion=20200331" type="text/javascript"></script>
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
			
			<asp:HiddenField ID="lblEtichetta" runat="server" />
			    <asp:HiddenField ID="controlloCheck" runat="server" />
			    <asp:HiddenField ID="valuePub" runat="server" />
			    <asp:HiddenField ID="valueOggetto" runat="server" />
			    <asp:HiddenField ID="valueOpContabile" runat="server" />
			    <asp:HiddenField ID="tipo" runat="server" />
			    <asp:HiddenField ID="codDocumento" runat="server" />
			    <asp:HiddenField ID="flagModificato" runat="server" />
			    <asp:HiddenField ID="flagRegistra" Value="1" runat="server" />
			     <asp:HiddenField ID="flagLivello" Value="0" runat="server" />
			    <asp:HiddenField ID="flagAbilitaOpContabili" Value="1" runat="server" />
			   
			    <asp:HiddenField ID="flagAbilitaPubblBur" Value="1" runat="server" />
			    <asp:HiddenField ID="flagCodificaAltriUff" Value="1" runat="server" />
			    <asp:HiddenField ID="flagCodiciCupCig" Value="1" runat="server" />
			    <asp:HiddenField ID="flagAbilitaOggettoDoc" Value="1" runat="server" />
			     <asp:HiddenField ID="flagAbilitaSchedaLeggeTrasparenza" Value="1" runat="server" />
			     <asp:HiddenField ID="flagAbilitaTipologiaProvvedimento" Value="1" runat="server" />
			    
			    <asp:HiddenField ID="valueDataCreazione" runat="server" />
			    <asp:HiddenField ID="valueUffProp" runat="server" />
			    <asp:HiddenField ID="descrizioneUffProp" runat="server" />
			    <asp:HiddenField ID="responsabileUffProp" runat="server" />
			    <asp:HiddenField ID="codFiscOperatore" runat="server" />
			    <asp:HiddenField ID="uffPubblicoOperatore" runat="server" />
			    <asp:HiddenField ID="schedaLeggeTrasparenzaInfo" runat="server" />
			    <asp:HiddenField ID="schedaContrattiFattureInfo" runat="server" />
			    <asp:HiddenField ID="schedaTipologiaProvvedimentoInfo" runat="server" />
                <asp:HiddenField ID="abilitatoCreazioneAttiSiOpCont" runat="server" />
                <asp:HiddenField ID="msgBloccoCreazioneAtti" runat="server" />
                
			</form>
       
       
              
</body>
</html>
