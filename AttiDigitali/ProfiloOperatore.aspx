<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ProfiloOperatore.aspx.vb" Inherits="AttiDigitali.ProfiloOperatore"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" /></head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
						<asp:placeholder id="Contenuto" runat="server"></asp:placeholder></td>
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
			<asp:Panel id="pnlModificaPassword" runat="server" CssClass="griglia">
			    <fieldset style="margin-top:8px;margin-bottom:8px;padding-left:20px;border:1px solid green; font-family:Verdana; font-size:13px" >
	                <legend style="padding: 0.2em 0.5em; margin-bottom:15px;
                      border:1px solid green;
                      color:green;
                      font-size:90%;
                      text-align:right;">Modifica password: </legend>
				    <table class="griglia" id="Table2" style="font-family:Verdana; font-size:12px">
					    <tr>
						    <td>Vecchia password
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:TextBox id="oldPwd" Runat="server" TextMode="Password"></asp:TextBox></td>
					    </tr>
					    <tr>
						    <td>Nuova password
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:TextBox id="newPwd" Runat="server" TextMode="Password"></asp:TextBox></td>
					    </tr>
					    <tr>
						    <td>Conferma nuova password
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:TextBox id="newPwdConfirm" Runat="server" TextMode="Password"></asp:TextBox></td>
					    </tr>
					    <tr>
						    <td>
							    <div align="left">&nbsp;</div>
						    </td>
					    </tr>
					    <tr>
						    <td align="center" colspan="2">
							    <asp:Button id="btnModificaPassword" runat="server" Width="140px" Text="Modifica"></asp:Button></td>
					    </tr>
					    
					    </table>
			    </fieldset>
			</asp:Panel>
			<asp:Panel id="pnlConfEmail" runat="server" CssClass="griglia">
			    <fieldset style="margin-top:8px;margin-bottom:8px;padding-left:20px;border:1px solid green; font-family:Verdana; font-size:13px" >
	                <legend style="padding: 0.2em 0.5em; margin-bottom:15px;
                      border:1px solid green;
                      color:green;
                      font-size:90%;
                      text-align:right;">Dati e notifiche: </legend>
					<table class="griglia" id="Table3" style="font-family:Verdana; font-size:12px">
					    <tr>
						    <td>Codice Fiscale
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:TextBox id="txtCodiceFiscale" Runat="server" TextMode="SingleLine" 
                                    CausesValidation="True" Width="271px"></asp:TextBox>
                           
                            </td>
					    </tr>
					    <tr>
						    <td>Email &nbsp;  <asp:RegularExpressionValidator ID="valEmailAddress" ControlToValidate="txtEmail"	ValidationExpression=".*@.*\..*" 
						    ErrorMessage="Indirizzo Email non valido." EnableClientScript="true" Runat="server" />
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:TextBox id="txtEmail" Runat="server" TextMode="SingleLine" CausesValidation="True" Width="271px"></asp:TextBox>
                            </td>
					    </tr>
					    <tr>
						    <td>Opzioni Ricezione Email
							    <div align="left">&nbsp;</div>
						    </td>
						    <td align="center">
							    <asp:CheckBoxList RepeatDirection="Vertical" RepeatColumns="2"  id="chkOpzioni" Runat="server" BorderWidth="0" CssClass="chkbx">
                                    <asp:ListItem Text="Inoltro" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Assegna" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Prelazione" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Rigetto" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Archivio" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Inoltro Per Conoscenza" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Rigetto Per Conoscenza" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Esecutività" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Notifica Per Competenza" Enabled="False" Selected="True"  Value="11"></asp:ListItem>
                                </asp:CheckBoxList>
							</td>
					    </tr>
					    <tr>
						    <td>
							    <div align="left">&nbsp;</div>
						    </td>
					    </tr>
		                <tr>
						    <td align="center" colspan="2">
							    <asp:Button id="btnOpzioniMessaggi" runat="server" Width="140px" Text="Modifica"></asp:Button></td>
					    </tr>					
				    </table>
                </fieldset>
			</asp:Panel>
			<asp:Panel ID="pnlUtilityFirmaMultipla" Runat="server" Visible="false" CssClass="griglia">
			    <fieldset style="margin-top:8px;margin-bottom:8px;padding-left:20px;border:1px solid green; font-family:Verdana; font-size:13px" >
	                <legend style="padding: 0.2em 0.5em; margin-bottom:15px;
                      border:1px solid green;
                      color:green;
                      font-size:90%;
                      text-align:right;">Memorizza PIN: </legend>
				    <table width="100%" class="griglia" style="font-family:Verdana; font-size:12px">
					    <tr>
					        <td>Consenso alla memorizzazione del PIN per sessione nella firma multipla
							        <div align="left">&nbsp;</div>
					        </td>
						    <td>
                                <asp:CheckBox   id="CheckBoxPINCACHE" Runat="server" />
                            </td>
                        </tr>
                        <tr>
						    <td>
							    <div align="left">&nbsp;</div>
						    </td>
					    </tr>
					    <tr>
						    <td align="center" colspan="2">
							    <asp:Button id="btnSaveCachePin" runat="server" Width="140px" Text="Salva"></asp:Button></td>
					    </tr>	
				    </table>
				</fieldset>
			</asp:Panel>
			
		</form>
	</body>
</html>
