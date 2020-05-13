<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" CodeBehind="Editor.aspx.vb" Inherits="AttiDigitali.editor" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" />
        <script type="text/javascript" src="ckeditor/ckeditor.js"></script>
    </head>
    <body>
		<form id="Form1" method="post" runat="server">
			<table class="pagina" id="Table1" cellspacing="0" cellpadding="0">
				<tr>
					<td class="pagina" colspan="2">
						<asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:placeholder id="Albero" runat="server"></asp:placeholder>
					</td>
					
					<td class="pagina" width="75%" style="height: 100px;" ><asp:placeholder id="Contenuto" runat="server"></asp:placeholder>

					<textarea cols="80" id="editor1" name="editor1" rows="10" runat="server"></textarea>
					<input type="hidden" id="hiddentoolbarId"  runat="server"/>
                <script type="text/javascript" src="ckeditor/config.js"></script>
                <script type="text/javascript">

			//<![CDATA[
			var nometoolbar ='';
			nometoolbar = document.getElementById('hiddentoolbarId').value;
				 CKEDITOR.replace( 'editor1',
	             {
	                 skin: 'office2003',
	                 height: '700px',
		            toolbar : nometoolbar

	            });
				 
			//]]>
			</script>
					</td>
				</tr>
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
			</table>

		</form>
    
	</body>

</html>
