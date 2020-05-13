<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="vb" debug="False" CodeBehind="tree.ascx.vb" AutoEventWireup="false" Inherits="AttiDigitali.tree" %>
<%@ Register TagPrefix="iewc1" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
<P>
	<iewc1:TreeView  id="TreeView1" runat="server" Indent="10"  ShowLines="False" ShowPlus="False" ExpandLevel="2"
		Width="180px"  DefaultStyle="color:#0000a0;font-family:Verdana,Geneva,Arial,Helvetica,sans-serif;font-size:10px;text-decoration:none;"></iewc1:TreeView></P>
