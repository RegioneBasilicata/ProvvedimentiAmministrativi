<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FirmaDocumento.aspx.vb" Inherits="AttiDigitali.FirmaDocumento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" />
		</head>
	<body >
	    
		<form id="Form1"  method="post" runat="server">
		    <input id="session_id" runat="server" type="hidden" name="session_id" />
		    <input id="key" runat="server" type="hidden" name="key" />
		    <input id="numDef" runat="server" type="hidden" name="numDef" />
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
					<tr ><td class="pagina" colspan="2" style="FONT-WEIGHT: bold; FONT-SIZE: 18px; COLOR: Red"> &nbsp; &nbsp;Attenzione la funzione di firma digitale è stata aggiornata per maggiori informazioni:  <b><a href="risorse/HELPFIRMASINGOLA/help_firmasingola.htm" target="_blank">Help Firma</a></b></td></tr>
                <tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%">
					    <table class="pagina" cellspacing="0" cellpadding="0">
					        <tr>
					            <td class="pagina" width="75%"><asp:placeholder id="Contenuto" runat="server"></asp:placeholder></td>
					        </tr>
					        <tr><td style="width:550" >
						       <script src="ext/deployJava.js" type="text/javascript"></script> 
						        <%--<script src="http://www.java.com/js/deployJava.js" type="text/javascript"></script> --%>
			                    <script type="text/javascript">
		                            function getDocuments() {
		                            
		                          
		                                   return [
				                            [window.document.getElementById("key").value, window.document.getElementById("lnkAnteprima").value.replace("&pdf=1","").replace("&prew=1","")+'&pdf=1', 'pdf', window.document.getElementById("lnkUpload").value, window.document.getElementById("numDef").value]
				                            ];
		                               }

		                               function documentUploaded(fileUploaded) {
		                                    //TODO		                               
		                               }
			                            </script>
							    <script type="text/javascript">
				                    var attributes = {
					                id:'dSigApplet',
					                code : 'com.intemaweb.security.dsig.DSigApplet',
					                archive: ['risorse/DSApplet-0.7.3.jar', 'risorse/bcprov-jdk16-1.45.jar', 'risorse/bcmail-jdk16-1.45.jar'],
					                width: 550,
					                height: 400
				                };
				                    var parameters = {
				                        sessionid: document.getElementById('session_id').value
				                    };
				                   
				                    deployJava.runApplet(attributes, parameters, '1.6');
			                </script>
			                </td></tr>
					    </table>
					</td>
				</tr>
				<tr align="right">
				    <td></td>
				    <td class="pagina">      
				        <asp:label id="label1" CssClass="lbl" runat="server" Visible="True" Text="Per continuare premi ->" ></asp:label>
			            <asp:Button id="btnProcedi" runat="server" Visible="True" Text="Continua" cssclass="btn"></asp:Button></td>
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
			<p>&nbsp;</p>
			<p><asp:panel id="pnlFirma" runat="server">
					<table class="griglia" id="Table2" width="100%" align="center">
						<tr>
							<td>
									<input id="lnkAnteprima" name="lnkAnteprima" runat="server"  type="hidden" />
									<input id="lnkUpload" name="lnkUpload" runat="server"  type="hidden" />
							</td>
						</tr>
					</table>
				</asp:panel></p>
		</form>
	</body>
</html>
