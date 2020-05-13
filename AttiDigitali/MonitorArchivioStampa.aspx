<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MonitorArchivioStampa.aspx.vb" Inherits="AttiDigitali.MonitorArchivioStampa"%>
<%@ Register TagPrefix="search" TagName="search" src="~/searchStampa.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="False" name="vs_snapToGrid">
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" /></head>
	<%--Test per selezionare tutte le checkbox griglia--%>
	<%--<script type="text/javascript" language=javascript src="GridSelezionaTutti.js" />--%>
	 <script  type="text/javascript" language="javascript"  >
	     function SelezionaTutti(idcontrol) {
	         var valueCheck = document.getElementById('ALL_' + idcontrol).checked
	         var lists = document.getElementsByName(idcontrol);

	         for (var i = 0; i < lists.length; i++) {
	             //lists[i].checked=true
	             lists[i].checked = valueCheck

	         }
	     }
	 </script>   
	  <script  type="text/javascript" language="javascript"  >
	      function VerificaSelezione(idcontrol) {
	            var lists = document.getElementsByName(idcontrol);
	            for (var i = 0; i < lists.length; i++) {	            
	                if (lists[i].checked) {
	                    return true
	                }
	            }
	         alert('Attenzione è necessario selezionare almeno un elemento dalla lista')  
	         return false
	        }
	 function AsseggnaClasseNonSelezione(idcontrol) {
	     var lists = document.getElementsByName(idcontrol);
	     for (var i = 0; i < lists.length; i++) {
	         if (!lists[i].checked) {
	             //  lists[i].parentNode.parentNode.setAttribute("oldClassName", lists[i].parentNode.className)
	             lists[i].parentNode.parentNode.className = "PrintClass"
	         } else { lists[i].parentNode.parentNode.className = ''}

	     }

	  


	 }
	
	 </script>   
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><div  class="PrintClass">
					<asp:placeholder id="Testata" runat="server"></asp:placeholder>
					</div>
					</td>
				</tr>
				<tr>
					<td id="tdAlbero" class="pagina" width="25%">
					<div  class="PrintClass">
					<asp:placeholder id="Albero" runat="server"></asp:placeholder>
					</div>
					</td>
					<td  id="tdContenuto" class="pagina" width="75%">
						<asp:placeholder id="Contenuto" runat="server">
						</asp:placeholder>
							<div  class="PrintClass">
						<asp:placeholder id="PannelloRicerca" runat="server">
							<search:search id="ricerca" runat="server"></search:search>
						</asp:placeholder>
						</div>
					</td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr class="PrintClass">
					<td width="25%">
						<div  class="PrintClass">
						    <div align="center" class="x-panel-header PrintClass"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					    </div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<asp:panel id="pnlStampaBlocco" runat="server" Height="31px" Width="580px" HorizontalAlign="Center"
				Visible="False">	<div  class="PrintClass">Seleziona i documenti che vuoi 
indicare come <b>Stampato</b> e clicca su Stampa
<asp:button id="btnStampaBlocco" runat="server" Width="81px" Text="Stampa" cssclass="btn"></asp:button>
</div>
</asp:panel>
<asp:panel id="pnlResetStato" runat="server" Height="31px" Width="580px" HorizontalAlign="Center"
				Visible="False">	<div  class="PrintClass">Seleziona i documenti che vuoi 
indicare come <b>da Stampare</b> e clicca su Reset
<asp:button id="btnResetStato" runat="server" Width="81px" Text="Reset" cssclass="btn"></asp:button>
</div>
</asp:panel>

		</form>
	</body>
</html>
