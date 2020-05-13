<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DownloadModelli.aspx.vb" Inherits="AttiDigitali.DownloadModelli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon"/>
		<script src="ext/adapter/ext/ext-base.js" type="text/javascript"></script>
        <script src="ext/ext-all-debug.js" type="text/javascript"></script>
        <script src="ext/Ext.ux.YearPicker.js" type="text/javascript"></script>        
</head>
<body>
    <form id="form1" runat="server">
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
		<asp:Panel id="pnlDownload" runat="server" Width="700px">
		<table style="width: 400px; text-align: center; margin-left: 150px; margin-top: 50px  " >
		    <tr >
		        <td style="text-align : center; color : Green; font-size: x-large  " colspan = "2">
		            <asp:Label  id="Label1" runat="server">MODELLI DETERMINE</asp:Label> 
		        </td>
		        <td style="text-align : center; color : Green; font-size: x-large; padding-left : 30px;" colspan = "2">
		            <asp:Label id="Label2" runat="server">MODELLI DISPOSIZIONI</asp:Label> 
		        </td>
                <%--<td style="text-align : center; color : Green; font-size: x-large; padding-left : 30px;" colspan = "2">
		            <asp:Label id="Label3" runat="server">MODELLO DELIBERA</asp:Label> 
		        </td>--%>
                <td style="text-align : center; color : Green; font-size: x-large; padding-left : 30px;" colspan = "2">
		            <asp:Label id="Label4" runat="server">TRACCIATO EXCEL</asp:Label> 
		        </td>
		    </tr>
		    <tr>
		        <td style="padding-top : 15px; text-align:right ">
		            <asp:Label id="lblDetermina" runat="server"> </asp:Label> 
		        </td>			   
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkDetermina" runat="server"></asp:HyperLink>				        
			    </td>
			    <td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblDisposizione" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkDisposizione" runat="server"></asp:HyperLink>				        
			    </td>
                
                <%--<td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblDelibera" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkDelibera" runat="server"></asp:HyperLink>				        
			    </td>--%>
                <td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblTracciatoBeneficiari" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkTracciatoBeneficiari" runat="server"></asp:HyperLink>				        
			    </td>
               

			</tr>
			<tr>
		       <td style="padding-top : 15px; text-align:right ">
		            <asp:Label id="lblDetermina_dg" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkDetermina_dg" runat="server"></asp:HyperLink>				        
			    </td>
			    <td style="padding-top : 15px;padding-left : 15px;  text-align:right ">
		            <asp:Label id="lblDisposizione_dg" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px; padding-left : 15px;text-align:left">
		            <asp:HyperLink ID="hyperLinkDisposizione_dg" runat="server"></asp:HyperLink>				        
			    </td>
			</tr>
			<tr>
		       <td style="padding-top : 15px; text-align:right ">
		            <asp:Label id="lblDetermina_CICO" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left">
		            <asp:HyperLink ID="hyperLinkDetermina_CICO" runat="server"></asp:HyperLink>				        
			    </td>
			    <td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblDisposizione_CICO" runat="server"> </asp:Label> 
		        </td>
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left">
		            <asp:HyperLink ID="hyperLinkDisposizione_CICO" runat="server"></asp:HyperLink>				        
			    </td>
			</tr>
			<tr>
		        <td style="padding-top : 15px; text-align:right ">
		            <asp:Label id="lblDetermina_CICO_pres" runat="server"> </asp:Label> 
		        </td>			  
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left ">
		            <asp:HyperLink ID="hyperLinkDetermina_CICO_pres" runat="server"></asp:HyperLink>				        
			    </td>
			     <td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblDisposizione_CICO_pres" runat="server"> </asp:Label> 
		        </td>			  
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left ">
		            <asp:HyperLink ID="hyperLinkDisposizione_CICO_pres" runat="server"></asp:HyperLink>				        
			    </td>
			</tr>
			
			<tr>
		       <td style="padding-top : 15px; text-align:right ">
		            <asp:Label id="lblDetermina_StrutturePresidente" runat="server"> </asp:Label> 
		        </td>			   
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left">
		            <asp:HyperLink ID="hyperLinkDetermina_StrutturePresidente" runat="server"></asp:HyperLink>				        
			    </td>
			    <td style="padding-top : 15px; padding-left : 15px; text-align:right ">
		            <asp:Label id="lblDisposizione_StrutturePres" runat="server"> </asp:Label> 
		        </td>
		   
		        <td style="padding-top : 15px;padding-left : 15px; text-align:left">
		            <asp:HyperLink ID="hyperLinkDisposizione_StrutturePres" runat="server"></asp:HyperLink>				        
			    </td>
			</tr>
			
			
		</table>
		</asp:Panel>
    </form>
</body>
</html>
