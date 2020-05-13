<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Worklist.aspx.vb" Inherits="AttiDigitali.Worklist"%>
<%@ Register TagPrefix="search" TagName="search" src="search.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="False" name="vs_snapToGrid" />
		<meta content="False" name="vs_showGrid" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<link href="Determine.css" type="text/css" rel="stylesheet" />  
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" /></head>
	<%--Test per selezionare tutte le checkbox griglia--%>
	<%--<script type="text/javascript" language=javascript src="GridSelezionaTutti.js" />--%>
	 <script  type="text/javascript" language="javascript"  >
	 
	 function SelezionaConNoteNegative(){
	 var i;
	     for(i = 1 ; i<document.getElementById("tblDati").rows.length ; i++){
	     if (document.getElementById("tblDati").rows[i].cells[7] != undefined ){
	        if(document.getElementById("tblDati").rows[i].cells[7].firstChild.innerHTML.indexOf('SuggNegativo.gif')>-1){
	          document.getElementById("tblDati").rows[i].cells[0].firstChild.checked=true
	         }else{
	          document.getElementById("tblDati").rows[i].cells[0].firstChild.checked=false
	         }
	      }
	      }
	      if(document.getElementById("btnRigettaInBlocco")!= null){
	        document.getElementById("btnRigettaInBlocco").style.borderColor= "red"
	      }
    	  document.getElementById("btnInoltroInBlocco").style.borderColor = "black"
    	  document.getElementById("btnInoltroInBlocco").onclick = function(){return confirm('Stai cercando di inoltrare atti segnalati come da rigettare, continuare?');}

	
	 }
	  function SelezionaConNotePositive(){
	  var i;
	     for(i = 1 ; i<document.getElementById("tblDati").rows.length ; i++){
	     if (document.getElementById("tblDati").rows[i].cells[7] != undefined ){
	        if(document.getElementById("tblDati").rows[i].cells[7].firstChild.innerHTML.indexOf('SuggNegativo.gif')>-1){
	          document.getElementById("tblDati").rows[i].cells[0].firstChild.checked=false
	         }else{
	          document.getElementById("tblDati").rows[i].cells[0].firstChild.checked=true
	         }
	      }
	      }
	      document.getElementById("btnInoltroInBlocco").style.borderColor = "red"
          if(document.getElementById("btnRigettaInBlocco")!= null){
	        document.getElementById("btnRigettaInBlocco").style.borderColor= "black"
	         document.getElementById("btnRigettaInBlocco").onclick = function(){return confirm('Stai cercando di rigettare atti segnalati come da inoltrare, continuare?');}
	      }
    	 
	
	 }
	 
	 
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
	 </script>   
	  <script  type="text/javascript" language="javascript"  >
	      function VerificaSelezioneEConsenso(idcontrol) {
	          var lists = document.getElementsByName(idcontrol);
	          var consenso = window.document.getElementById("consensoPin").value;
	          var contaSelezionati =0 ;
	          for (var i = 0; i < lists.length; i++) {
	             if (lists[i].checked) {
	                 contaSelezionati++;
	             }
	          }
	          if( ( contaSelezionati== 1) || (consenso=="TRUE")) {
                return true
	          }
              if (contaSelezionati< 1) {
                alert('Attenzione è necessario selezionare almeno un elemento dalla lista')  
	            return false
	          }
	           if( ( contaSelezionati>0) && (consenso!="True")) {
	                var ret = confirm('Hai scelto di firmare più provvedimenti con una sola immissione del Pin,(art. 35 del D.Lgs n. 82 del 2005). Questa scelta sarà memorizzata (potrai modificarla dalla maschera del profilo). Vuoi procedere?')  
	                return ret  
	            }
	      }
	 </script>
	<body>
		<form id="Form1" method="post" runat="server">
		 <input id="consensoPin" name="consensoPin" runat="server"  type="hidden" />
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
						<asp:placeholder id="Contenuto" runat="server">
						</asp:placeholder>
						<asp:placeholder id="PannelloRicerca" runat="server">
							<search:search id="ricerca" runat="server"></search:search>
						</asp:placeholder>
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
			<asp:panel id="pnlInoltroInBlocco" runat="server" Height="31px" Width="580px" HorizontalAlign="Center"
				Visible="False">Seleziona i documenti che 
vuoi <b>inoltrare </b> e clicca sul pulsante Prosegui 
<asp:button id="btnInoltroInBlocco" runat="server" Width="83px" Text="Prosegui" cssclass="btn"></asp:button></asp:panel>
<asp:panel id="pnlRigettaInBlocco" runat="server" Width="580px" HorizontalAlign="Center"
				Visible="False">Seleziona i documenti che 
vuoi&nbsp;<b> rigettare</b> &nbsp;e clicca sul pulsante "Rigetta"&nbsp;
<asp:button id="btnRigettaInBlocco"    runat="server" Visible="False" Width="83px" Text="Rigetta" cssclass="btn"></asp:button></asp:panel>

		</form>
	</body>
</html>
