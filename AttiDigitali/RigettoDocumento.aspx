<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RigettoDocumento.aspx.vb"  Inherits="AttiDigitali.RigettoDocumento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
		<script language="javascript">
		    function disabilita() {	       
		    if (window.document.getElementById('btnRigetta') != null) {
		        window.document.getElementById('btnRigetta').style.visibility = 'hidden'
		    }
		    if (window.document.getElementById('btnFirma') != null) {
		        window.document.getElementById('btnFirma').style.visibility = 'hidden'
		    }
		
		} </script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server" onsubmit="disabilita()">
			<table id="Table1" class="pagina" cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%"><asp:placeholder id="Contenuto" runat="server"></asp:placeholder></td>
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
			<asp:panel id="pnlRigetto" runat="server" Height="56px" Width="576px">
				<table width="100%">
					<tr>
						<td width="75%">
							<asp:Label id="Label2" Runat="server" text="Premi rigetta per inviare indietro il provvedimento"
								CssClass="lbl">Premi rigetta per inviare indietro il provvedimento informalmente</asp:Label></td>
						<td align="right" width="25%">
							<asp:Button id="btnRigetta" runat="server" Text="Rigetta senza Firma" cssclass="btn"></asp:Button></td>
					</tr>
					<tr>
						<td width="75%">
							<asp:Label id="Label1" Runat="server" text="Premi rigetta per inviare indietro il provvedimento"
								CssClass="lbl">Premi Firma per rigettare il provvedimento formalmente</asp:Label></td>
						<td align="right" width="25%">
					<asp:button id="btnFirma"    runat="server"  Text="Firma e Rigetta" cssclass="btn"></asp:button>
					</tr>
				</table>
			</asp:panel>
			
			<asp:Panel id="pnlNote" runat="server">
				<p>Note</p>
				<p>
					<asp:TextBox id="txtNote" runat="server" Rows="5" Columns="65" TextMode="MultiLine"></asp:TextBox></P>
			</asp:Panel>
			<asp:panel id="pnlAggiungiAllegato" runat="server" Height="24px" HorizontalAlign="Center" Width="560px">Aggiungi Note di Rigetto 
(file firmato) <input id="fileUpLoadAllegato" type="file" name="fileUpLoadAllegato"
					runat="server" /> 
			</asp:panel>
			<br />
			<table align="center">
			    <tr>
			        <td>
                    <asp:Label id="LblErrore" CssClass="lblWarning" Runat="server" visible="False"></asp:Label>
			        </td>
			    </tr>
			</table>
		
		</form>
	</body>
</html>
