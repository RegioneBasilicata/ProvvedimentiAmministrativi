<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HomePage.aspx.vb" Inherits="AttiDigitali.HomePage"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="Determine.css" type="text/css" rel="stylesheet" /> 
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
        <link href="ext/display/stile.css" rel="stylesheet" type="text/css" />
		<script language="javascript"></script>
</head>

	<body>
		<form id="Form1" method="post" runat="server">
		
			<asp:Label id="lblBenvenuto" runat="server">Benvenuto a</asp:Label>
			
			
			<table id="Table1" class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<%--<tr >--%>
				<%--<td class="pagina" colspan="2" 
                        style="padding: 5px 50px 5px 50px;  text-align:justify; line-height: 1.5em; font-weight: bold; font-family:Trebuchet MS;  font-size: 14px; color: #F85118;">Da martedì 08 Ottobre 2013
                    è in linea la nuova scheda &quot;Tipologia Provvedimento/Destinatari&quot; per la gestione
                    degli obblighi di pubblicita', trasparenza e diffusione di informazioni da parte della Pubblica Amministrazione (legge <a href="http://www.normattiva.it/uri-res/N2Ls?urn:nir:stato:decreto.legislativo:2013-03-14;33!vig=" target=”_blank">n. 33/2013</a>) attraverso il sito istituzionale.</td>--%>
                    <%--<td class="pagina" colspan="2" 
                        style="padding: 5px 50px 5px 50px;  text-align:justify; line-height: 1.5em; font-weight: bold; font-family:Trebuchet MS;  font-size: 14px; color: #04B404">
                        SU RICHIESTA DELL&#39; UFFICIO RAGIONERIA GENERALE E FISCALITA&#39; REGIONALE, LA 
                        CREAZIONE DEGLI ATTI RIMANE BLOCCATA FINO AL GIORNO  <a style="font-family:Trebuchet MS;  font-size: 15px; color: blue">15 LUGLIO 2014</a></td>--%>
                        
                       <%-- <td class="pagina" colspan="2" 
                        style="border-style: inset; border-color: #008080; padding: 15px 0px 15px 30px;  text-align:left ; line-height: 1.5em; font-weight: bold; font-family:Trebuchet MS;  font-size: 14px; color: #FF8000">
                            <p style="margin-left: 200px; font-weight: bold; font-family:Trebuchet MS;  font-size: 14px; color: #FF5050">**** ATTENZIONE: ****</p>
                            <p style="font-weight: bold; font-family:Trebuchet MS;  font-size: 14px; color: #993366; background-color: #FFFFFF;">
                                Provvedimenti Amministrativi è stato aggiornato alla nuova versione per 
                                permettere la gestione della FATTURAZIONE ELETTRONICA.</p></td>--%>
                    <%-- </tr>--%>
                <%--<tr><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 14px; COLOR: Red; padding:10px;"><img src="risorse/immagini/information.png" alt="Avviso">&nbsp;Per informazioni sul caricamento dei beneficiari delle liquidazioni chiedere all'ufficio Ragioneria Generale</td></tr>--%>
                 <%--<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 16px; padding:10px; line-height:1.5em"><img src="risorse/immagini/information.png" alt="Avviso">&nbsp;Da Lunedì 15 Ottobre sarà in linea la nuova funzionalità che riguarda il bilancio pluriennale; per maggiori dettagli consultare il <a href="risorse/helpBilancioPluriennale.htm">documento</a>. </td></tr>--%>
				
				<%--<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red"> &nbsp; &nbsp;Per richiedere assistenza è necessario inviare una mail a</td></tr>
				
				<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red">&nbsp; &nbsp;<a href='mailto:provvedimenti@assistenza.regione.basilicata.it'"><b>provvedimenti@assistenza.regione.basilicata.it</b></a> </td></tr>
				
				<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Fuchsia">&nbsp; &nbsp; DA TALE DATA GLI ATTI CHE PERVERRANNO IN DIREZIONE GENERALE REDATTI </td></tr>
				<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Fuchsia">&nbsp; &nbsp; CON MODELLI DIVERSI, SARANNO RESTITUITI AI RELATIVI UFFICI PROPONENTI</td></tr>
				
				<tr ><td class="pagina" colspan=2 style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red"> &nbsp; &nbsp;Attenzione! Scaricare il nuovo modello della propria Area da <b><a href="ProfiloOperatoreAction.aspx">Profilo</a></b></td></tr>
				
					<tr ><td class="pagina" colspan="2" style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red"> &nbsp; &nbsp;Attenzione la funzione di firma digitale è stata aggiornata per maggiori informazioni:<br /> <b><a href="risorse/HELPFIRMASINGOLA/help_firmasingola.htm" target="_blank">Help Firma Singola</a></b> e <b><a href="risorse/HELPFIRMAMULTIPLA/help_firmamultipla.htm" target="_blank">Help Firma Multipla</a></b></td></tr>
			--%>
				<tr>
					<td valign="top" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td width="75%" valign="top">
						<asp:placeholder id="Contenuto" runat="server"></asp:placeholder></td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td  width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx" >Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%" ></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			<asp:Label id="lblAlert" runat="server" CssClass="lblErrore" />
			<asp:PlaceHolder ID=PlaceTotaleAtti runat=server></asp:PlaceHolder>
		</form>
	</body>
</html>
