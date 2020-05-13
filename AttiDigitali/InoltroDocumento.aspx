<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InoltroDocumento.aspx.vb"  Inherits="AttiDigitali.InoltroDocumento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon"/>
		<script language="javascript">
		      function disabilita() {
		       
		    if (window.document.getElementById('btnInoltra')!=null) {
		        window.document.getElementById('btnInoltra').style.visibility = 'hidden';
		      
		    }
		    if (window.document.getElementById('btnFirma') != null) {

		        window.document.getElementById('btnFirma').style.visibility = 'hidden';
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
			<asp:panel id="pnlInoltro" runat="server" Width="680px">			  
				<table style="width: 680px;">		
						<tr>
                           <td colspan="2"><div style="height:20px"></div></td> 
                        </tr>       
				        <tr>
                            <td colspan="2" style="text-align:center">
                                <asp:Label ID="lblPriorita" Runat="server" CssClass="lbl" 
                                    text="<i style='color: #3dad8b; font-size: 10px'><b>Novità</b></i>&nbsp;&nbsp;&nbsp;Seleziona la priorità&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Label>
                                <asp:RadioButton ID="radioIsNotUrgent" runat="server" Checked="True" CssClass="radio" 
                                    GroupName="radioPriorita" Text="Normale" />
                                <asp:RadioButton ID="radioIsUrgent" runat="server" GroupName="radioPriorita"  CssClass="radio"
                                    Text="Urgente" />
                            </td>
                        </tr>
                        <tr>
                           <td colspan="2"><div style="height:20px"></div></td> 
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDestinatarioInoltro" Runat="server" CssClass="lbl" 
                                    text="Seleziona l'ufficio a cui inoltrare"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbDestinatarioInoltro" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="75%">
                                <asp:Label ID="Label3" Runat="server" CssClass="lbl" 
                                    text="Premi Firma e Inoltra per firmare ed inoltrare il provvedimento al successivo responsabile">Premi &#39;<span 
                                    class="textBold">Firma e Inoltra</span>&#39; per firmare ed inoltrare il provvedimento al successivo responsabile</asp:Label>
                            </td>
                            <td align="right" width="25%">
                                <asp:Button ID="btnFirma" runat="server" cssclass="btn" 
                                    Text="Firma e Inoltra" />
                            </td>
                        </tr>
                        <tr>
                            <td width="75%">
                                <asp:Label ID="Label1" Runat="server" CssClass="lbl" 
                                    text="Premi inoltra per inviare il provvedimento al successivo responsabile senza firma" 
                                    Width="500px">Premi &#39;Inoltra senza firma&#39; per inviare il provvedimento al successivo responsabile senza firma</asp:Label>
                                <asp:DropDownList ID="ddlSupervisore" runat="server" Width="312px">
                                </asp:DropDownList>
                            </td>
                            <td align="right" width="25%">
                                <asp:Button ID="btnInoltra" runat="server" cssclass="btn" 
                                    Text="Inoltra senza firma" />
                            </td>
                        </tr>
                   

					
				</table>
			</asp:panel>
			
			<asp:Panel id="pnlNote" runat="server"  Width="680px">
				<p>Note</p>
				<p>
					<asp:TextBox id="txtNote" runat="server" Rows="5" Columns="80" TextMode="MultiLine"></asp:TextBox></p>
			</asp:Panel>
			<br />
			<asp:panel id="pnlSuggerimento" runat="server" Width="680px">
			<asp:Label id="lblSuggerimento" CssClass="lbl" text="Seleziona un suggerimento per il prossimo responsabile "
								Runat="server" Visible="false"></asp:Label>
			<asp:DropDownList id="ddlSuggerimenti" runat="server" Visible="false"></asp:DropDownList>
			</asp:panel>
			<table align="center" style="width: 680px;">
			    <tr>
			        <td>
                    <asp:Label id="LblErrore" CssClass="lblWarning" Runat="server" visible="False"></asp:Label>
			        </td>
			    </tr>
			</table>
		
		</form>
	</body>
</html>
