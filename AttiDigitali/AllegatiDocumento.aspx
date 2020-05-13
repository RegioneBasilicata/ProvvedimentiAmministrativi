<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AllegatiDocumento.aspx.vb" Inherits="AttiDigitali.AllegatiDocumento"%>
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
        <script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
		<link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<script src="ext/GroupSummary.js" type="text/javascript"></script>  
		<script src="ext/PopupDettaglioStorico.js" type="text/javascript"></script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table1" class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td colspan="2" class="pagina">
						<asp:PlaceHolder id="Testata" runat="server"></asp:PlaceHolder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:PlaceHolder id="Albero" runat="server"></asp:PlaceHolder></td>
					<td class="pagina" width="75%">
						<asp:PlaceHolder id="Contenuto" runat="server"></asp:PlaceHolder></td>
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
			<asp:Panel HorizontalAlign="Center" id="pnlTotalePagineAllegati" runat="server" Width="750px" >
			    <table style="margin-left: 70px; margin-top: 10px; margin-bottom:20px">
                    <tr>
                        <td><asp:Label id="lblTotalePagineAllegati" runat="server">Pagine totali allegati: </asp:Label> </td>
                        <td><asp:TextBox id="txtTotalePagineAllegati" runat="server" Width="52px"></asp:TextBox></td>
                        <td><asp:Button id="btnRegistraTotPagineAllegati" runat="server" Width="83px" Text="Registra" cssclass="btn"></asp:Button><asp:Button id="btnResetTotPagineAllegati" runat="server" Width="83px" Text="Pulisci" cssclass="btn"></asp:Button></td>
                    </tr>
                </table>
			</asp:Panel>               
            <asp:Panel HorizontalAlign="Center" id="pnlAggiungiAllegato" runat="server" Width="750px" >
            <hr /> 
            <table style="margin-left: 70px; margin-top: 10px; margin-bottom:20px">
                <tr>
                    <td colspan="2"><div style="height:10px"></div> </td>
                </tr>
                <tr align="center" style="margin-bottom:10px; margin-top :10px; ">
                    <td colspan="2"><b>Aggiungi allegati</b></td>
                </tr>
                <tr>
                    <td>Seleziona il file: </td>
                    <td><input id="fileUpLoadAllegato1" type="file" name="File1" runat="server"/></td>
                </tr>
                <tr>
                    <td>Nome allegato: </td>
                    <td><asp:TextBox id="txtNomeAllegato" runat="server" Width="258px"></asp:TextBox></td>
                </tr>
                <tr align="center">
                    <td colspan="2"><asp:Button id="btnRegistraAllegato" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button></td>
                </tr>
            </table>
	           <%-- <p>Pagine totali allegati: <asp:TextBox id="txtNumeroPagine" runat="server" Width="258px"></asp:TextBox></p>
	            <p style="margin-left: 50px; margin-top: 20px; margin-bottom:10px"><b>Aggiungi allegati</b>:</p>
	            <p>seleziona il file&nbsp; <input id="fileUpLoadAllegato1" type="file" name="File1" runat="server"></p>
	            <p>nome dell'allegato&nbsp;
		        <asp:TextBox id="txtNomeAllegato" runat="server" Width="258px"></asp:TextBox>
		        <asp:Button id="btnRegistraAllegato" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button></p>--%>
		       <%-- </div> --%>
            </asp:Panel>
                    
            <asp:Panel id="pnlAggiungiDocumenti" runat="server" Height="70px" Width="750px" HorizontalAlign="Center">
            <hr /> 
	            <table style="margin-left: 70px; margin-top: 10px; margin-bottom:20px">
                    <tr align="center" style="margin-bottom:10px; margin-top :10px; ">
                        <td colspan="2"><b>Aggiungi Documentazione visibile solo nel tuo Ufficio</b></td>
                    </tr>
                    <tr>
                        <td>Seleziona il file: </td>
                        <td><input id="fileUpLoadDocumento" type="file" name="File1" runat="server"/></td>
                    </tr>
                    <tr>
                        <td>Nome documento: </td>
                        <td><asp:TextBox id="txtNomeDocumento" runat="server" Width="258px"></asp:TextBox></td>
                    </tr>
                    <tr align="center">
                        <td colspan="2"><asp:Button id="btnRegistraDocumento" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button></td>
                    </tr>
                </table>
	        <%--<P><STRONG>Aggiungi Documentazione visibile solo nel tuo Ufficio</strong>:</P>
	        <P>seleziona il file&nbsp; <INPUT id="fileUpLoadDocumento" type="file" name="File1" runat="server"></p>
	        <P>nome del documento&nbsp;
		        <asp:TextBox id="txtNomeDocumento" runat="server" Width="258px"></asp:TextBox>
		        <asp:Button id="btnRegistraDocumento" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button></p>--%>
            </asp:Panel>
            
	        <asp:Panel HorizontalAlign="Center" id="pnlAggiungiAllegatoVuoto" runat="server" Width="750px" Height="140px">			        
	        <hr /> 
		     <table style="margin-left: 70px; margin-top: 10px; margin-bottom:20px">
                <tr align="center" style="margin-bottom:10px; margin-top :10px; ">
                    <td colspan="2"><b>Aggiungi note allegati cartacei</b></td>
                </tr>
                <tr>
                    <td>Nome: </td>
                    <td><asp:TextBox id="txtNomeAllegatoVuoto" runat="server" Width="258px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Rintracciabilità: </td>
                    <td> <asp:TextBox id="txtModalita" runat="server" Width="258px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Referente:</td>
                    <td><asp:TextBox id="txtDestinatari" runat="server" Width="258px"></asp:TextBox></td>
                </tr>
                <tr align="center"> 
                    <td colspan="2"><asp:Button id="btnRegistraAllegatoVuoto" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button></td>
                </tr>
            </table>
		    <%--<p><strong>Aggiungi note allegati cartacei</strong>:</p>
		    <p></p>
		    <p>nome dell'allegato&nbsp;
			    <asp:TextBox id="txtNomeAllegatoVuoto" runat="server" Width="258px"></asp:TextBox></p>
		    <p>Rintracciabilità&nbsp;
			    <asp:TextBox id="txtModalita" runat="server" Width="258px"></asp:TextBox></p>
		    <p>Referente&nbsp;
			    <asp:TextBox id="txtDestinatari" runat="server" Width="258px"></asp:TextBox></p>
		    <asp:Button id="btnRegistraAllegatoVuoto" runat="server" Width="83px" Text="Allega" cssclass="btn"></asp:Button>
		    <p></p>--%>
	        </asp:Panel>

			            <asp:Panel id="PnlCancella" runat="server" Width="750px" HorizontalAlign="Center" Visible="False">
				        <hr/>
				        <hr/>
				        <asp:Button id="btnCancella" runat="server" Text="Cancella"></asp:Button>
			            </asp:Panel>
			
		</form>
	</body>
</html>
