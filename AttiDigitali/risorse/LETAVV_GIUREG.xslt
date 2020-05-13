<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
			<head>
				<style type="text/css">
				body,tr,td
{
	font-family:Verdana;
	font-size:14px;
	
}


td.logo
{
    background:url(./risorse/immagini/logoRegioneSD.gif) no-repeat
}
table
{
	border-width:0px;
	border-style:solid;
	padding:2px;	
    margin:4px;
    vertical-align:top;
    font-size:14px;
}

td
{
	vertical-align:bottom;
	border-width:0px;
	border-style:solid;    
	font-size:14px;
}</style>
				<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
			<body>
				<table Border="0" width="100%" cellpadding="3" cellspacing="0">
					<tr>
						<td align="center" class="logo"></td>
					</tr>
					<tr>
						<td>
							<xsl:value-of select="/datilettera/dipartimento" />
						</td>
					</tr>
					<tr>
						<td>
							<xsl:value-of select="/datilettera/ufficio" />
						</td>
					</tr>
					<tr>
						<td align="left">Prot. n° </td>
					</tr>
					<tr>
						<td align="right">del </td>
					</tr>
					<tr>
						<td align="center">Al Giunta Regionale di Basilicata</td>
					</tr>
					<tr>
						<td>Oggetto <xsl:value-of select="/datilettera/oggetto" /></td>
					</tr>
					<xsl:for-each select="datiDocumento/documento">
						<tr>
							<td>Numero<xsl:value-of select="numero"></td>
							<td>Numero<xsl:value-of select="oggetto"></td>
						</tr>
					</xsl:for-each>
					<tr>
						<td align="right">Il Dirigente Generale </td>
					</tr>
					
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>