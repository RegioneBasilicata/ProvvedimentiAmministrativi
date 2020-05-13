<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AvvisoRedirect.aspx.vb" Inherits="AttiDigitali.AvvisoRedirect"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
	<body>
		<FORM id="Form1" method="post" runat="server">
	

		<asp:HiddenField Value="HomePage.aspx" id="hdest"  runat="server" />

			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
						
								
						
				</tr>
					<tr ><td   class="pagina" colspan="2" style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red"> <b><a href="HomePage.aspx"  style="color:Red"> La delega legata all'utente è scaduta verrai rediretto per il login tra <input type="text" size="1"    name="counter">   secondi</a></b></td></tr>
		
				<tr>
					<td class="pagina"  width="25%">
						<asp:placeholder id="Albero" Visible=false runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%" >
						<asp:placeholder id="Contenuto" Visible=false runat="server">
										
						</asp:placeholder></td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr style="height:50px;>
					
					<td colspan=2  width="100%" >
					</td>
				</tr>
				<tr>
					
					<td colspan=2 class="x-panel-header" width="100%" >
					</td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
		</FORM>
		
			<script language="Javascript">
<!--
// Imposto il sito di destinazione
var destinazione =document.Form1.hdest.value ;
//alert(document.Form1.hdest.value)
// Imposto il numero di secondi per il conto alla rovescia
var secondi = 10;

// Creo la variabile conteggio e contestualmente
// imposto il valore di partenza al numero di secondi + 1
var conteggio = document.Form1.counter.value = secondi + 1;


 

// Creo la funzione che gestisce il conto alla rovescia
function contoallarovescia()
{

  // Se la variabile conteggio è maggiore di 1...
  if (conteggio > 1)
  {
    // decremento il valore della variabile conteggio
    conteggio = conteggio - 1;
    // e contestualmente aggiorno il numero mostrato a video
    document.Form1.counter.value = conteggio;
  // ...se conteggio è uguale o minore di 1 eseguo il redirect
  }else{
    window.location = destinazione;
    return
  }
  // La funzione ri-esegue se stessa ogni secondo fino alla
  // esecuzione del redirect
  setTimeout("contoallarovescia()", 1000);
}

// Lancio per la prima volta la funzione (che poi, come sappiamo
// provvederà autonomamente ad auto eseguirsi ogni secondo)
contoallarovescia()
//-->
</script>
	</body>
</html>
