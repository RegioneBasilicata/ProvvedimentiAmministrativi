<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Determina.aspx.vb" Inherits="AttiDigitali.Determina" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body><script language=javascript>
	          function verificaTipoOperazione() {
	              alert();}
	          function gestioneCheck(obj) {

	              switch (obj.id) {
	              
	              
	                 case "chkNessuna":
	                     window.document.getElementById("chkImpegno").checked = false;
	                     window.document.getElementById("chkImpegnoSuPerenti").checked = false;
	                      window.document.getElementById("chkAccertamento").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      window.document.getElementById("chkLiquidazione").checked = false;
	                      break;


	                  case "chkRiduzione":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkImpegno").checked = false;
	                      window.document.getElementById("chkImpegnoSuPerenti").checked = false;
	                      if (window.document.getElementById("chkLiquidazione").checked == false) {
	                          window.document.getElementById("chkAccertamento").checked = false;
	                      }
	                      //' window.document.getElementById("chkLiquidazione").checked = false;
	                      break;

	                  case "chkImpegno":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      break;
	                      
	                  case "chkImpegnoSuPerenti":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      break;



	                  case "chkAccertamento":
	                  if  ( window.document.getElementById("chkImpegno").checked  ||
	                        window.document.getElementById("chkLiquidazione").checked) {
	                        window.document.getElementById("chkNessuna").checked = false;
	                        window.document.getElementById("chkRiduzione").checked = false;
	                       }else{
	                       window.document.getElementById("chkAccertamento").checked=false;
	                       }

	                       break;

	                   case "chkLiquidazione":
	                       window.document.getElementById("chkNessuna").checked = false;
	                     //  window.document.getElementById("chkRiduzione").checked = false;
	                       break;

	               }
	                     
	                       if ((window.document.getElementById("chkNessuna").checked) || (window.document.getElementById("chkRiduzione").checked)
|| (window.document.getElementById("chkAccertamento").checked) || (window.document.getElementById("chkImpegno").checked) || (window.document.getElementById("chkImpegnoSuPerenti").checked)
|| (window.document.getElementById("chkLiquidazione").checked)) {
	                           window.document.getElementById("controlloCheck").value='1'

	                       } else {
	                     window.document.getElementById("controlloCheck").value=''
	                     }
	              
	             
	          
	          }</script>
		<form id="Form1" enctype="multipart/form-data" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					
					<td class="pagina" width="75%">
						<P><asp:placeholder id="Contenuto" runat="server">
						<asp:Panel   runat="server" ID="PannelloTipoContabile">
		        	 <table>
		        	  <tr><td>
                   <asp:RequiredFieldValidator  ID="rqvControllo" runat="server" ControlToValidate="controlloCheck" ErrorMessage="Selezionare l'operazione contabile"></asp:RequiredFieldValidator>
                   <asp:TextBox   runat="server"   ID="controlloCheck" ></asp:TextBox>
		            </td></tr>
		           <tr><td><asp:CheckBox ID="chkNessuna" CssClass="lbl" Text="Nessuna Operazione"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkImpegno" CssClass="lbl" Text="Impegni + eventuali liquidazioni contestuali"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkImpegnoSuPerenti" CssClass="lbl" Text="Impegno Su Perenti + liquidazioni contestuali"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkLiquidazione"  CssClass="lbl" Text="Liquidazione"   onclick="javascript:gestioneCheck(this)"     runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkAccertamento" CssClass="lbl"  Text="Accertamento"   onclick="javascript:gestioneCheck(this)"   runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkRiduzione" CssClass="lbl" Text="Riduzione/Economia Impegno"   onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   
            </table>
                  </asp:Panel>
						
						</asp:placeholder></P>
						<P>&nbsp;</P>
					</td>
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
			
			<P>&nbsp;</P>
			<asp:Button cssclass="btn" id="ButtonContinua" runat="server" Text="Registra"></asp:Button>
		</form>
		<P>&nbsp;</P>
	</body>
</html>
