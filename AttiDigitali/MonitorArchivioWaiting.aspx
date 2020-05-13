<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MonitorArchivioWaiting.aspx.vb" Inherits="AttiDigitali.MonitorArchivioWaiting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sistema di Gestione Atti Amministrativi</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
    <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="ext/ext-all-debug.js" type="text/javascript"></script>
    <link href="Determine.css" type="text/css" rel="stylesheet" />
    <script src="ext/WaitingPanel.js" type="text/javascript"></script>           
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
					       <div id="myPanelPrincipale"></div>
                     </td>
				</tr>
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
			</table>	
			<form id="Form1"  runat="server">			
			    <asp:HiddenField ID="waitingPanelTitle" runat="server" />		                            
			    <asp:HiddenField ID="waitingPanelMsg" runat="server" />		                            
			    <asp:HiddenField ID="waitingPanelActionUrl" runat="server" />		                            
			</form>
</body>
</html>
