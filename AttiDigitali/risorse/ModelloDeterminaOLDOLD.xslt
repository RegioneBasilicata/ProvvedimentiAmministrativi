<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
			<head>
				<style type="text/css">
body
{
	font-family:Arial;
	font-size:10pt;
	margin:11pt ;
	padding:0px;
	color:#000000
}
.bold
{
	font-weight: bold;
}
.grande
{
	font-size:11pt;
}
.piccolo
{
	font-size:9pt;
}
.piccoloBold
{
	font-weight: bold;
	font-size:9pt;
}
td.paginaa4 {
	height:1120px;
	vertical-align:top;
	border-width:0px;
}
p.testatadoc
{
color:#FFFFFF;
height:30px;
vertical-align:center;
}

td.logo
{
	background-position: center;
	height:180px;
    background:url(./risorse/immagini/logoRegionetondo.gif) no-repeat
}
table.determina
{
	border-width: 0px 0px 0px 0px;
	border-style:solid;
	padding:3px;	
    margin:3px;
    vertical-align:top;
    font-size:11pt;
    border-color: #000000;
}
table.bordo
{
	border-width: 1px 1px 1px 1px;
	border-style:solid;
	padding:5px;	
    margin:5px;
    vertical-align:top;
    font-size:11pt;
    border-color: #000000;
}


td.determina
{
	vertical-align:top;
	border-width: 0px 0px 0px 0px;
	border-style:solid; 
	border-color: #000000;   
	font-size:11pt;
}

td.bordo 
{
	vertical-align:top;
	border-width: 1px 1px 1px 1px;
	border-style: solid;
	border-color: #000000;
	font-size:11pt;
}
				</style>
			</head>
			<body>
				<table Align="center" Border="0" cellpadding="0" cellspacing="0" width="100%">
					<!-- Prima pagina -->
					<tr>
						<td class="paginaa4">
							<table Align="center" Border="0" cellpadding="0" cellspacing="10" width="100%">
								<!-- Determinazione dirigenziale -->
								<tr>
									<td align="center" bgcolor="#FFFFFF" valign="center">
										<p class="testatadoc"/>
									</td>
								</tr>
								<tr>
									<td align="center" bgcolor="#000000" valign="bottom">
										<p class="testatadoc">
											<br/>
											<br/>DETERMINAZIONE DIRIGENZIALE  </p>
									</td>
								</tr>
								<!-- Logo + dati -->
								<tr>
									<td>
										<table border="0" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td Align="center" width="20%">
														<img src="D:\Documenti\Sviluppo.Net\progetti\AttiDigitali\risorse\immagini\logoRegione.gif" width="180" height="90"/>
													</td>
													<td valign="bottom" width="40%">
														<span class="piccoloBold">
															<xsl:value-of select="/datiDocumento/descrdipartimento"/>
														</span>
													</td>
													<td width="40%">
														<table border="0" width="100%" cellpadding="0" cellspacing="0">
															<tbody>
																<tr>
																	<td colspan="2" class="determina" valign="bottom">
																		<table border="1" width="100%" cellpadding="0" cellspacing="3">
																			<tbody>
																				<tr>
																					<td class="determina" valign="bottom" colspan="2">
																						<xsl:value-of select="/datiDocumento/descrufficio"/>
																					</td>
																				</tr>
																				<tr>
																					<td class="determina" width="55%">
																						<span class="piccoloBold">STRUTTURA PROPONENTE</span>
																					</td>
																					<td class="determina" width="40%">
																						<span class="piccoloBold">COD.</span>
																						<span class="piccolo">
																							<xsl:value-of select="/datiDocumento/codufficio"/>
																						</span>
																					</td>
																				</tr>
																			</tbody>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td class="determina" width="60%">
																		<table border="1" width="100%" cellpadding="0" cellspacing="3">
																			<tbody>
																				<tr>
																					<td class="determina" with="50%">
																						<span class="piccoloBold">N° </span>
																					</td>
																					<td class="determina" with="50%">
																						<span class="piccoloBold">
																							<xsl:value-of select="/datiDocumento/docnumeroutente"/>
																						</span>
																					</td>
																				</tr>
																			</tbody>
																		</table>
																	</td>
																	<td class="determina" width="40%">
																		<table border="1" width="100%" cellpadding="0" cellspacing="3">
																			<tbody>
																				<tr>
																					<td class="determina" with="50%">
																						<span class="piccoloBold">DEL  </span>
																					</td>
																					<td class="determina" with="50%">
																						<span class="piccoloBold">
																							<xsl:value-of select="/datiDocumento/docdata"/>
																						</span>
																					</td>
																				</tr>
																			</tbody>
																		</table>
																	</td>
																</tr>
															</tbody>
														</table>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<!-- Oggetto -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td height="130" class="determina">
														<div class="bold">OGGETTO</div>
														<div class="piccolo">
															<xsl:value-of select="/datiDocumento/oggetto"/>
														</div>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<!-- Ufficio controllo interno di regolarità amministrativa -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td colspan="2" class="determina">
														<div class="bold">UFFICIO CONTROLLO INTERNO DI REGOLARITA' AMMINISTRATIVA<br/>
														</div>
													</td>
												</tr>
												<tr>
													<td height="150" class="determina">Note<br/>
														<div class="piccolo">
															<xsl:for-each select="/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni' and Dno_prog=3]">
																<xsl:value-of select="Dno_testo"/>
															</xsl:for-each>
														</div>
													</td>
												</tr>
												<tr>
													<td width="40%" class="determina">Visto di regolarita' amministrativa
													</td>
													<td width="40%" class="determina">IL DIRIGENTE <xsl:value-of select="//compitidocumento/dir_controlloamm"/>
													</td>
													<td width="20%" class="determina">Data <xsl:value-of select="/datiDocumento/docdata"/>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<!-- Ufficio ragioneria generale -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tr>
												<td>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td class="determina" colspan="5">
																	<div class="bold">UFFICIO RAGIONERIA GENERALE</div>
																</td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_assunzione']">
																<tr>
																	<td height="80" width="30%" class="determina">Assunto impegno contabile N <xsl:value-of select="DRA_NContabile"/>
																	</td>
																	<td width="15%" class="determina">UPB:  <xsl:value-of select="DRA_UPB"/>
																	</td>
																	<td width="15%" class="determina">Cap.  <xsl:value-of select="DRA_Cap"/>
																	</td>
																	<td width="15%" class="determina">Esercizio  <xsl:value-of select="DRA_Esercizio"/>
																	</td>
																	<td width="25%" class="determina"> per € <xsl:value-of select="DRA_Costo"/>
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
												</td>
											</tr>
											<tr>
												<td>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_liquidazione']">
																<tr>
																	<td width="30%" class="determina">Liquidazione N. <xsl:value-of select="DRL_NLiquidazione"/>
																	</td>
																	<td width="15%" class="determina">UPB:  <xsl:value-of select="DRL_UPB"/>
																	</td>
																	<td width="15%" class="determina">Cap.  <xsl:value-of select="DRL_Cap"/>
																	</td>
																	<td width="15%" class="determina">Esercizio  <xsl:value-of select="DRL_Esercizio"/>
																	</td>
																	<td width="25%" class="determina"> per € <xsl:value-of select="DRL_Costo"/>
																	</td>
																</tr>
																<tr>
																	<td height="120" width="30%" class="determina">In base all'impegno contabile N.: <xsl:value-of select="DRL_NContabile"/>
																	</td>
																	<td width="15%" class="determina">Assunto con:  
													<xsl:choose>
																			<xsl:when test="DRL_TipoAssunzione = 0">Determinazione
														</xsl:when>
																			<xsl:when test="DRL_TipoAssunzione = 1">Deliberazione
														</xsl:when>
																			<xsl:otherwise> 
														</xsl:otherwise>
																		</xsl:choose>
																	</td>
																	<td width="15%" class="determina">N.  <xsl:value-of select="DRL_NAssunzione"/>
																	</td>
																	<td width="15%" class="determina"> del <xsl:value-of select="DRL_Data"/>
																	</td>
																	<td width="25%" class="determina"/>
																</tr>
															</xsl:for-each>
															<tr>
																<td height="110" colspan="5" class="determina">Note<br/>
																	<br/>
																	<br/>
																</td>
															</tr>
															<tr>
																<td width="30%" colspan="2" class="determina">Visto di regolarita' contabile
													</td>
																<td width="15%" colspan="2" class="determina">IL DIRIGENTE <xsl:value-of select="//compitidocumento/dir_ragioneria"/>
																</td>
																<td width="15%" class="determina">Data <xsl:value-of select="/datiDocumento/docdata"/>
																</td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<!-- Atto soggetto a pubblicazione -->
								<tr>
									<td class="determina">
										<table Border="0" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina">
														<table Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tr class="determina">
																<td class="determina">Atto soggetto a pubblicazione	 <xsl:value-of select="/datiDocumento/tipoPubblicazione"/>   ()Integrale  <xsl:value-of select="/datiDocumento/tipoPubblicazione"/>    ()Per estratto</td>
															</tr>
														</table>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<!-- Seconda pagina -->
					<tr>
						<td class="paginaa4">
							<table border="0" width="100%">
								<tbody>
									<tr>
										<td colspan="2">
											<table Border="0" width="100%" cellpadding="10" cellspacing="10">
												<tbody>
													<tr>
														<td stesto="1">testo determina</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" class="determina">L'istruttore  <xsl:value-of select="//compitidocumento/istruttore"/>
											<br/>
											<br/>
											<br/>
										</td>
									</tr>
									<tr>
										<td width="50%" class="determina">Il responsabile P.O.  <xsl:value-of select="//compitidocumento/poc_istruttore"/>
										</td>
										<td width="50%" class="determina">Il Dirigente  <xsl:value-of select="//compitidocumento/dirigenteup"/>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<!-- Terza pagina -->
					<tr>
						<td class="paginaa4">
							<table Align="center" Border="0" cellpadding="0" cellspacing="10" width="100%">
								<tr>
									<td align="center" bgcolor="#FFFFFF" valign="center">
										<p class="testatadoc"/>
									</td>
								</tr>
								<tr>
									<td height="80" align="center" bgcolor="#000000" valign="center">
										<p class="testatadoc"/>
									</td>
								</tr>
								<tr>
									<td align="center" bgcolor="#FFFFFF" valign="center">
										<p class="testatadoc"/>
									</td>
								</tr>
								<!-- Oggetto -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td height="130" width="20%" class="determina">
														<div class="bold">OGGETTO</div>
														<br/>
													</td>
													<td width="80%" class="determina">
														<xsl:value-of select="/datiDocumento/oggetto"/>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<!-- La presente determinazione comporta -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tr>
												<td class="determina">
													<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr class="determina">
																<td colspan="4">La presente determinazione comporta impegno contabile sul: </td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_bilancio']">
																<tr class="sololinea">
																	<td width="25%" class="determina">Bilancio: <xsl:value-of select="Dbi_Bilancio"/>
																	</td>
																	<td width="25%" class="determina">UPB:  <xsl:value-of select="Dbi_UPB"/>
																	</td>
																	<td width="25%" class="determina">Cap.  <xsl:value-of select="Dbi_Cap"/>
																	</td>
																	<td width="25%" class="determina"> per € <xsl:value-of select="Dbi_Costo"/>
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td width="35%" class="determina">Come da prenotazione d'impegno N. <xsl:value-of select="/datiDocumento/numeroPrenotazione"/>	 Anno: <xsl:value-of select="/datiDocumento/annoPrenotazione"/>
																</td>
															</tr>
														</tbody>
													</table>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td colspan="4" class="determina">Con la presente determinazione si procede alla liquidazione della somma</td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_liquidazione']">
																<tr>
																	<td width="35%" class="determina">di € 
																		<xsl:value-of select="Dli_Costo"/>
																	</td>
																	<td width="30%" class="determina">sul Cap.
																		<xsl:value-of select="Dli_Cap"/>
																	</td>
																	<td width="20%" class="determina">UPB
																		<xsl:value-of select="Dli_UPB"/>
																	</td>
																	<td width="20%" class="determina">Esercizio
																		<xsl:value-of select="Dli_Esercizio"/>
																	</td>
																</tr>
																<tr>
																	<td width="35%" class="determina">in base all'impegno contabile N. <xsl:value-of select="Dli_NContabile"/>
																	</td>
																	<td width="30%" class="determina">assunto con       
																		<xsl:choose>
																			<xsl:when test="Dli_TipoAssunzione = 0">Determinazione
																			</xsl:when>
																			<xsl:when test="Dli_TipoAssunzione = 1">Deliberazione
																			</xsl:when>
																			<xsl:otherwise> 
																			</xsl:otherwise>
																		</xsl:choose>
																	</td>
																	<td width="20%" class="determina">N.
																		<xsl:value-of select="Dli_Num_assunzione"/>
																	</td>
																	<td width="20%" class="determina">del
																		<xsl:value-of select="Dli_Data_Assunzione"/>
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
												</td>
											</tr>
											<tr>
												<td class="determina">
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td width="35%" class="determina">Allegati N. <xsl:value-of select="/datiDocumento/numeroAllegati"/>
																</td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<!-- Osservazioni -->
								<tr>
									<td>
										<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina">OSSERVAZIONI</td>
												</tr>
												<tr>
													<td class="determina" height="80px">
														<xsl:for-each select="/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni' and Dno_prog=1]">
															<xsl:value-of select="Dno_testo"/>
														</xsl:for-each>
													</td>
												</tr>
												<tr>
													<td class="determina">IL DIRIGENTE GENERALE <xsl:value-of select="//compitidocumento/rev_dirigentegen"/>
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<!-- Copia della presente determinazione -->
								<tr>
									<td>
										<table border="0" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina" border="1">Copia della presente determinazione viene trasmessa in data <xsl:value-of select="/datiDocumento/dataTrasmissione"/> al Presidente della Giunta regionale, alla Giunta, al Consiglio regionale, al Responsabile politico del Dipartimento proponente e al Bollettino Ufficiale della Regione. <br/>
														<br/> L'IMPIEGATO ADDETTO
							</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
