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

td.paginaa4 {
	height:1100px;
	vertical-align:top;
	border-width:0px;
}

td.logo
{
    background-position: center; 
	height:200px;
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
    horizontal-align:center
}

td
{
	vertical-align:bottom;
	border-width:0px;
	border-style:solid;    
	font-size:14px;	
}

td.tab
{
	vertical-align:middle;
	align:center;
	border-width:1px;
	border-style:solid;    
	font-size:14px;	
}


</style>
				<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" /></HEAD>
			<body>
				<table Align="center" Border="0" cellpadding="5" cellspacing="5" width="100%">
					<tr>
						<td class="paginaa4">
							<table Border="1" width="100%" cellpadding="3" cellspacing="5">
								<tr>
									<td class="logo" align="center" ><img src="D:\Documenti\Sviluppo.Net\progetti\AttiDigitali\risorse\immagini\logoRegione.gif" width="180" height="90"/></td>
								</tr>
								<tr>
									<td height="20px" align="center">
										<xsl:value-of select="/datilettera/dipartimento"/>
									</td>
								</tr>
								<tr>
									<td height="20px" align="center">
										<xsl:value-of select="/datilettera/ufficio"/>
									</td>
								</tr>
								<tr>
									<td></td>
								</tr>
								<tr>
									<td height="20px" align="left" >Prot. n° _____________
									 </td>
								</tr>
								<tr>
									<td  align="right">del _____________</td>
								</tr>
								<tr>
									<td height="20px" ></td>
								</tr>
								<tr>
								
									<td align="right">Al Dirigente del</td>
								</tr>
								<tr>
									<td align="right"><xsl:value-of select="/datilettera/uffdestinatario"/></td>
								</tr>
								<tr>
									<td align="right"/>
								</tr>
								<tr>
									<td height="50px">Oggetto: <xsl:value-of select="/datilettera/oggetto"/>
									</td>
								</tr>
								<xsl:for-each select="datilettera/documento">
									<tr>
										<td>Numero: <xsl:value-of select="numero"/></td>
									</tr>
								</xsl:for-each>
								<tr>
									<td align="right">Il Dirigente                   </td>
								</tr>
								<tr>
									<td height="50px">
										<hr/>
									</td>
								</tr>
								<tr>
									<td>
										<xsl:value-of select="/datilettera/dipartimento"/>
									</td>
								</tr>
								<tr>
									<td/>
								</tr>
								<tr>
									<td align="left">Per ricevuta</td>
								</tr>
								<tr>
									<td align="left">Data </td>
								</tr>
								<tr>
									<td align="right">Firma </td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="paginaa4">
							<table Border="0" width="100%" cellpadding="5" cellspacing="5">
							<tr>
								<td colspan="3" align="center" class="logo"><img src="D:\Documenti\Sviluppo.Net\progetti\AttiDigitali\risorse\immagini\logoRegione.gif" width="180" height="90"/></td>
							</tr>
							<tr>
								<td colspan="3" align="center">
									<xsl:value-of select="/datilettera/dipartimento" />
								</td>
							</tr>
							
							<tr>
								<td colspan="3" align="center">Allegato alla lettera di Trasmissione delle Determinazioni Dirigenziali</td>
							</tr>
							<tr>
								<td colspan="3" align="center">
									<xsl:value-of select="/datilettera/uffdestinatario" />
								</td>
							</tr>
							<tr><td>
							<table Border="1" width="100%" cellpadding="0" cellspacing="0">
								<tr>
									<td height="40"  width="7%" class="tab"><div align="center">N. Prog</div></td>
									<td height="40" width="23%" class="tab"><div align="center">Codice Atto e Struttura Proponente</div></td>
									<td height="40" width="50%" class="tab"><div align="center"> Oggetto </div></td>
									<td height="40" width="20%" class="tab"><div align="center">Riservato alla Strutt. Proponente</div></td>
								</tr>
								</table>
								
								<table Border="0" width="100%" cellpadding="0" cellspacing="0">
								<xsl:for-each select="datilettera/documento">
								<tr>
									<td width="7%"  align="center"><xsl:value-of select="ord"/></td>
									<td width="23%" >N° <xsl:value-of select="numero"/><br/><xsl:value-of select="uffProp"/></td>
									<td width="50%" ><xsl:value-of select="oggetto"/></td>
									<td width="20%"></td>
									
								</tr>
								<tr>
									<td colspan="4"><hr/></hr></td>
								</tr>
								</xsl:for-each>
							</table></td></tr>
								
							</table>
						</td>
					</tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>