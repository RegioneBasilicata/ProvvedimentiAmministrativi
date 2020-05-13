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
				<table Align="center" Border="0" cellpadding="5" cellspacing="5" width="100%">
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo"/>
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
									<td align="left">Prot. n°_______________ </td>
								</tr>
								<tr>
									<td align="right">del _________________</td>
								</tr>
								<tr>
									<td align="right">
										<p>Al Presidente</p>
										<p>della Giunta</p>
									</td>
								</tr>
								<tr>
									<td align="right">
										<p>All'assessore</p>
										<p>Dipartimento</p>
									</td>
								</tr>
								<tr>
									<td>OGGETTO: <xsl:value-of select="/datilettera/oggetto" /></td>
								</tr>
								<ol>
								<xsl:for-each select="/datilettera/documento">
								<tr>
									<td><li>o<xsl:value-of select="numero"></li></td>
									<td><xsl:value-of select="oggetto"></td>
								</tr>
								</xsl:for-each>
								</ol>
								<tr>
									<td align="right">Il Dirigente Generale </td>
								</tr>
					<tr></hr></tr>
					
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo"/>
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
									<td align="left">Prot. n°_______________ </td>
								</tr>
								<tr>
									<td align="right">del _________________</td>
								</tr>
								<tr>
									<td align="right">
										<p>Al Consiglio</p>
										<p>Regionale di</p>
										<p>Basilicata</p>
									</td>
								</tr>
								<tr>
									<td align="right">
										<p>All'assessore</p>
										<p>Dipartimento</p>
									</td>
								</tr>
								<tr>
									<td>OGGETTO: <xsl:value-of select="/datilettera/oggetto" /></td>
								</tr>
								<ol>
								<xsl:for-each select="/datilettera/documento">
								<tr>
									<td><li><xsl:value-of select="numero"></li></td>
									<td><xsl:value-of select="oggetto"></td>
								</tr>
								</xsl:for-each>
								</ol>
								<tr>
									<td align="right">Il Dirigente Generale </td>
								</tr>
					<tr></hr></tr>
					
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo"/>
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
									<td align="left">Prot. n°_______________ </td>
								</tr>
								<tr>
									<td align="right">del _________________</td>
								</tr>
								<tr>
									<td align="right">
										<p>Alla Giunta</p>
										<p>Regionale di</p>
										<p>Basilicata</p>
									</td>
								</tr>
								<tr>
									<td>OGGETTO: <xsl:value-of select="/datilettera/oggetto" /></td>
								</tr>
								<ol>
								<xsl:for-each select="/datilettera/documento">
								<tr>
									<td><li><xsl:value-of select="numero"></li></td>
									<td><xsl:value-of select="oggetto"></td>
								</tr>
								</xsl:for-each>
								</ol>
								<tr>
									<td align="right">Il Dirigente Generale </td>
								</tr>
					<tr></hr></tr>
					
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo"/>
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
									<td align="left">Prot. n°_______________ </td>
								</tr>
								<tr>
									<td align="right">del _________________</td>
								</tr>
								<tr>
									<td align="right">
										<p>Al Dirigente Responsabile</p>
										<p>Bollettino Ufficiale</p>
									</td>
								</tr>
								<tr>
									<td>OGGETTO: <xsl:value-of select="/datilettera/oggetto" /></td>
								</tr>
								<ol>
								<xsl:for-each select="/datilettera/documento">
								<tr>
									<td><li><xsl:value-of select="numero"></li></td>
									<td><xsl:value-of select="oggetto"></td>
								</tr>
								</xsl:for-each>
								</ol>
								<tr>
									<td align="right">Il Dirigente Generale </td>
								</tr>
					<tr></hr></tr>
					
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo"/>
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
									<td align="left">Prot. n°_______________ </td>
								</tr>
								<tr>
									<td align="right">del _________________</td>
								</tr>
								<tr>
									<td align="right">
										<p>Alla Struttura Dir.</p>
										<p>Generale</p>
										<p>Presidenza della </p>
										<p>Giunta Regionale</p>
									</td>
								</tr>
								<tr>
									<td>OGGETTO: <xsl:value-of select="/datilettera/oggetto" /></td>
								</tr>
								<ol>
								<xsl:for-each select="/datilettera/documento">
								<tr>
									<td><li><xsl:value-of select="numero"></li></td>
									<td><xsl:value-of select="oggetto"></td>
								</tr>
								</xsl:for-each>
								</ol>
								<tr>
									<td align="right">Il Dirigente Generale </td>
								</tr>
					<tr></hr></tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
