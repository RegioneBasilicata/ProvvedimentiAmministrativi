<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatiSeduta.aspx.vb" Inherits="AttiDigitali.DatiSeduta"%>

<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI, Version=1.9.0.0, Culture=neutral, PublicKeyToken=24d65337282035f2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  
		<link rel="stylesheet" href="table.css" type="text/css"/>	
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
	    <link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
	     <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
	    <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
        <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
        <script src="ext/GestioneDatiSeduta.js" type="text/javascript"></script>
        <script src="ext/OnReadyDatiSeduta.js" type="text/javascript"></script>
        <script src="ext/CheckColumn.js" type="text/javascript"></script>
		
</head>
	<body>
		<%--<form id="Form1" method="post" runat="server">
			<table class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
         
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
					  <asp:placeholder id="Contenuto" runat="server">
					    <div id="myPanelPrincipale"></div>
					</asp:placeholder></td>
                    
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
			
			<p><asp:label id="Label1" runat="server" ForeColor="Red"></asp:label></p>
			<p><asp:Label id="Label2" runat="server" ForeColor="Red"></asp:Label></p>--%>
			
		
		<%--	<asp:panel id="pnlSeduta" runat="server" HorizontalAlign="Center">
			    
			    <table style="margin-top: 30px; margin-bottom: 40px ">
			        <tr>
			            <td>
			            <asp:Label ID="Label4" runat="server" Text="Il: "/> 
			            </td>
			            <td style="padding-right: 20px">
			                <ew:CalendarPopup id="CalendarPopup1" runat="server" CalendarWidth="300" ImageUrl="~/risorse/immagini/Calendar.gif" ControlDisplay="TextBoxImage"
		                        DisableTextboxEntry="False">				
			                </ew:CalendarPopup> 
			            </td>
			            <td>
			            <asp:Label ID="lblOra" runat="server" Text="Alle ore:"/>
			            </td>			           
			            <td>
			                <asp:TextBox id="txtOra" runat="server" Width="23px" CssClass="sololinea"/>
	                        <asp:Label ID="Label3" runat="server" Text=":"/>
	                        <asp:TextBox id="txtMin" runat="server" Width="23px" CssClass="sololinea"/>
			            </td>
			        </tr>
    	        
	            </table>
	            <asp:Panel id="pnlRelatori" runat="server" CssClass="CSS_Table_Example">
	            </asp:Panel> 
			</asp:panel>	--%>	
		<%--</form>--%>
		
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
			<form id="Form1"  runat="server">
			    <asp:HiddenField ID="valueUffProp" runat="server" />
			    <asp:HiddenField ID="codDocumento" runat="server" />
			    <asp:HiddenField ID="itemDatiSedutaInfo" runat="server" />
			</form>
       
	</body>
</html>
