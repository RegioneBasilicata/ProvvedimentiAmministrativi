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
						<td colspan="3" align="center" class="logo"></td>
					</tr>
					<tr>
						<td colspan="3">
							<xsl:value-of select="/datilettera/dipartimento" />
						</td>
					</tr>
					<tr>
						<td colspan="3">Allegato alla lettera di Trasmissione delle Determinazioni Dirigenziali</td>
					</tr>
					<tr>
						<td colspan="3"></td>
					</tr>
					<xsl:for-each select="datiDocumento/documento">
						<tr>
							<td>N. prog.<xsl:value-of select="ord"></td>
							<td>Numero<xsl:value-of select="numero"></br><xsl:value-of select="uffProp"></td>
							<td>Numero<xsl:value-of select="oggetto"></td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>