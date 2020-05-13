<%@ Control Language="vb" CodeBehind="header.ascx.vb" AutoEventWireup="false" Inherits="AttiDigitali.header" %>

<table width="1000" cellspacing="0" cellpadding="0">
	<tr >
		<td class="x-panel-header" valign="middle" align="center"  >
			<asp:Image ID="logo" AlternateText="Banner Struttura" Runat="server" CssClass="logo"></asp:Image>
			<asp:Label Runat="server" id="Label1">Sistema di Gestione Provvedimenti Amministrativi</asp:Label>
		</td>
	</tr>
	<tr>
		<td>
			<div class="x-panel-headerwhite" align="right">
				&nbsp;&nbsp;
				<asp:Label id="lblOperatore" runat="server" ></asp:Label></div>
		</td>
	</tr>
</table>
