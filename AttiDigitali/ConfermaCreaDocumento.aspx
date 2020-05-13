<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfermaCreaDocumento.aspx.vb" Inherits="AttiDigitali.ConfermaCreaDocumento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
       <script language="javascript">
	          function verificaTipoOperazione() {
	              alert();}
	          function gestioneCheck(obj) {

	              switch (obj.id) {

	                  case "chkNessuna":
	                      window.document.getElementById("chkPreimpegni").checked = false;
	                     window.document.getElementById("chkImpegno").checked = false;
	                     window.document.getElementById("chkImpegnoSuPerenti").checked = false;
	                      window.document.getElementById("chkAccertamento").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      window.document.getElementById("chkLiquidazione").checked = false;
	                      break;

	                  case "chkPreimpegni":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkImpegno").checked = false;
	                      window.document.getElementById("chkImpegnoSuPerenti").checked = false;
	                      window.document.getElementById("chkAccertamento").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      window.document.getElementById("chkLiquidazione").checked = false;
	                      break;

	                  case "chkRiduzione":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkPreimpegni").checked = false;
	                      window.document.getElementById("chkImpegno").checked = false;
	                      window.document.getElementById("chkImpegnoSuPerenti").checked = false;
	                      if (window.document.getElementById("chkLiquidazione").checked == false) {
	                          window.document.getElementById("chkAccertamento").checked = false;
	                      }
	                      //' window.document.getElementById("chkLiquidazione").checked = false;
	                      break;

	                  case "chkImpegno":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkPreimpegni").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      break;
	                      
	                  case "chkImpegnoSuPerenti":
	                      window.document.getElementById("chkNessuna").checked = false;
	                      window.document.getElementById("chkPreimpegni").checked = false;
	                      window.document.getElementById("chkRiduzione").checked = false;
	                      break;



	                  case "chkAccertamento":
	                  if  ( window.document.getElementById("chkImpegno").checked  ||
	                        window.document.getElementById("chkLiquidazione").checked) {
	                            window.document.getElementById("chkNessuna").checked = false;
	                            window.document.getElementById("chkPreimpegni").checked = false;
	                            window.document.getElementById("chkRiduzione").checked = false;
	                       }else{
	                            window.document.getElementById("chkAccertamento").checked=false;
	                       }

	                       break;

	                   case "chkLiquidazione":
	                       window.document.getElementById("chkNessuna").checked = false;
	                       window.document.getElementById("chkPreimpegni").checked = false;
	                     //  window.document.getElementById("chkRiduzione").checked = false;
	                       break;

	               }

	               if ((window.document.getElementById("chkNessuna").checked) 
	                            || (window.document.getElementById("chkRiduzione").checked)
                                || (window.document.getElementById("chkAccertamento").checked) 
                                || (window.document.getElementById("chkPreimpegni").checked)
                                || (window.document.getElementById("chkImpegno").checked) 
                                || (window.document.getElementById("chkImpegnoSuPerenti").checked)
                                || (window.document.getElementById("chkLiquidazione").checked)) {
	                           window.document.getElementById("controlloCheck").value='1'

	                       } else {
	                        window.document.getElementById("controlloCheck").value=''
	                     }
	             }
	              function verificaEsistenzaOggetto(obj) {
	              alert(obj);}
	             </script>
		<form id="form1" runat="server">
			<table class="pagina" id="table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
		   
					<asp:placeholder id="Contenuto" runat="server">
				     <asp:Panel   runat="server" ID="PannelloTipoContabile">
				      <asp:RequiredFieldValidator  ID="rqvControllo" runat="server" ControlToValidate="controlloCheck" ErrorMessage="Selezionare l'operazione contabile"></asp:RequiredFieldValidator>
                   <asp:TextBox   runat="server"   ID="controlloCheck" ></asp:TextBox>
		        	 <table>	        	
                   <tr><td><asp:CheckBox ID="chkNessuna" CssClass="lbl" Text="Nessuna Operazione"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkPreimpegni" CssClass="lbl" Text="Prenotazio di impegno"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   <tr><td><asp:CheckBox ID="chkImpegno" CssClass="lbl" Text="Impegno ed eventuale liquidazione contestuale"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>                   
                   <tr><td><asp:CheckBox ID="chkImpegnoSuPerenti" CssClass="lbl" Text="Impegno Su Perenti e liquidazione contestuale"  onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                      
             <tr><td><asp:CheckBox ID="chkLiquidazione"  CssClass="lbl" Text="Liquidazione su Impegno precedentemente assunto con altro provvedimento"   onclick="javascript:gestioneCheck(this)"     runat="server" /></td></tr>
             <tr><td><asp:CheckBox ID="chkAccertamento" CssClass="lbl"  Text="Accertamento"   onclick="javascript:gestioneCheck(this)"   runat="server" /></td></tr>
             <tr><td><asp:CheckBox ID="chkRiduzione" CssClass="lbl" Text="Riduzione/Economia di Impegno"   onclick="javascript:gestioneCheck(this)"    runat="server" /></td></tr>
                   
            </table>
         </asp:Panel>
             
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
			<asp:button id="btnContinua2" runat="server" Text="Continua" Visible="False" cssclass="btn"></asp:button><asp:button id="btnAnnulla2" runat="server" Text="Annulla" Visible="False"></asp:button>
		</FORM>
	</body>
</html>
