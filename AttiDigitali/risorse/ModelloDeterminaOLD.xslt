<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
			<head>
				<style type="text/css">
body
{
	font-family:Verdana;
	font-size:10px;
	margin:0px ;
	padding:0px;
	color:#000000
}

td.paginaa4 {
	height:1120px;
	vertical-align:top;
	border-width:0px;
}
p.testatadoc
{
color:#FFFFFF;
height:50px;
vertical-align:center;
}

td.logo
{
	height:110px;
    background:url(./risorse/immagini/logoRegioneSD.gif) no-repeat
}
table.determina
{
	border-width:0px;
	border-style:solid;
	padding:2px;	
    margin:4px;
    vertical-align:top;
    font-size:14px;
}

td.determina
{
	vertical-align:top;
	border-width:0px;
	border-style:solid;    
	font-size:14px;
}
				</style>
			</head>
			<body>
				<table Align="center" Border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td class="paginaa4">
							<table Align="center" Border="0" cellpadding="0" cellspacing="10" width="100%">
								<tr>
									<td align="center" bgcolor="#000000" valign="center">
										<p class="testatadoc">DETERMINAZIONE DIRIGENZIALE</p>
									</td>
								</tr>
								<tr>
									<td>
										<table Border="0" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td width="33%" class="logo"></td>
													<td width="33%"><xsl:value-of select="/datiDocumento/descrdipartimento" />
													</td>
													<td width="33%">
														<table Border="0" width="100%" cellpadding="0" cellspacing="0">
															<tbody>
																<tr>
																	<td colspan="2" class="determina" valign="bottom">
																		<table border="1" width="100%" cellpadding="0" cellspacing="3">
																			<tbody>
																			<tr>
																				<td class="determina" valign="bottom" colspan="2">
																					<xsl:value-of select="/datiDocumento/descrufficio" />
																				</td>																				
																			</tr>
																			<tr>		
																					<td class="determina" width="55%">Struttura Proponente</td>
																					<td class="determina" width="40%">COD. <xsl:value-of select="/datiDocumento/codufficio" /></td>																				
																			</tr>
																			</tbody>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td class="determina" width="40%"></td>
																	<td class="determina" width="60%">
																		<table border="1" width="100%" cellpadding="0" cellspacing="3">
																			<tbody>
																				<tr>
																					<td class="determina" with="50%">N° </td>
																					<td class="determina" with="50%"> <xsl:value-of select="/datiDocumento/docnumeroutente" /></td>
																				</tr>
																				<tr>
																					<td class="determina" with="50%">DEL  </td>
																					<td class="determina" with="50%"><xsl:value-of select="/datiDocumento/docdata" /></td>
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
								<tr>
									<td>
										<table Border="1" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina">Oggetto<br />
														<xsl:value-of select="/datiDocumento/oggetto" />
													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<table Border="1" width="100%" cellpadding="3" cellspacing="0">
											<tr>
												<td class="determina">
													<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr class="determina">
																<td colspan="4">La presente determinazione comporta impegno contabile sul: </td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_bilancio']">
																<tr class="sololinea">
																	<td width="25%" class="determina">Bilancio: <xsl:value-of select="Dbi_Bilancio" />
																	</td>
																	<td width="25%" class="determina">UPB:  <xsl:value-of select="Dbi_UPB" />
																	</td>
																	<td width="25%" class="determina">Cap.  <xsl:value-of select="Dbi_Cap" />
																	</td>
																	<td width="25%" class="determina"> per € <xsl:value-of select="Dbi_Costo" />
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td width="35%" class="determina">Come da prenotazione d'impegno N. <xsl:value-of select="/datiDocumento/numeroPrenotazione" />	 Anno: <xsl:value-of select="/datiDocumento/annoPrenotazione" /></td>
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
																		<xsl:value-of select="Dli_Costo" />
																	</td>
																	<td width="30%" class="determina">sul Cap.
																		<xsl:value-of select="Dli_Cap" />
																	</td>
																	<td width="20%" class="determina">UPB
																		<xsl:value-of select="Dli_UPB" />
																	</td>
																	<td width="20%" class="determina">Esercizio
																		<xsl:value-of select="Dli_Esercizio" />
																	</td>
																</tr>
																<tr>
																	<td width="35%" class="determina" >in base all'impegno contabile N. <xsl:value-of select="Dli_NContabile" />
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
																		<xsl:value-of select="Dli_Num_assunzione" />
																	</td>
																	<td width="20%" class="determina">del
																		<xsl:value-of select="Dli_Data_Assunzione" />
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
																<td width="35%" class="determina">Allegati N. <xsl:value-of select="/datiDocumento/numeroAllegati" />
														</td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td class="determina">
										<table Border="1" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina">
														<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tbody>
																<tr>
																	<td class="determina" colspan="4">UFFICIO RAGIONERIA GENERALE</td>
																</tr>
																<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_assunzione']">
																	<tr>
																		<td width="30%" class="determina">Assunto impegno contabile N <xsl:value-of select="DRA_NContabile" />
																		</td>
																		<td width="15%" class="determina">UPB:  <xsl:value-of select="DRA_UPB" />
																		</td>
																		<td width="15%" class="determina">Cap.  <xsl:value-of select="DRA_Cap" />
																		</td>
																		<td width="15%" class="determina">Esercizio  <xsl:value-of select="DRA_Esercizio" />
																		</td>
																		<td width="25%" class="determina"> per € <xsl:value-of select="DRA_Costo" />
																		</td>
																	</tr>
																</xsl:for-each>
															</tbody>
														</table>
														<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tbody>
																<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_liquidazione']">
																	<tr>
																		<td width="25%" class="determina">Liquidazione N. <xsl:value-of select="DRL_NLiquidazione" />
																		</td>
																		<td width="20%" class="determina">UPB:  <xsl:value-of select="DRL_UPB" />
																		</td>
																		<td width="20%" class="determina">Cap.  <xsl:value-of select="DRL_Cap" />
																		</td>
																		<td width="15%" class="determina">Esercizio  <xsl:value-of select="DRL_Esercizio" />
																		</td>
																		<td width="20%" class="determina"> per € <xsl:value-of select="DRL_Costo" />
																		</td>
																	</tr>
																	<tr>
																		<td width="25%" class="determina">In base all'impegno contabile N.: <xsl:value-of select="DRL_NContabile" />
																		</td>
																		<td width="40%" class="determina" colspan="2">Assunto con:  
																		<xsl:choose>
																			<xsl:when test="DRL_TipoAssunzione = 0">Determinazione
																			</xsl:when>
																			<xsl:when test="DRL_TipoAssunzione = 1">Deliberazione
																			</xsl:when>
																			<xsl:otherwise> 
																			</xsl:otherwise>
																		</xsl:choose>	
																		</td>
																		<td width="15%" class="determina">N.  <xsl:value-of select="DRL_NAssunzione" />
																		</td>
																		<td width="20%" class="determina"> del <xsl:value-of select="DRL_Data" />
																		</td>
																	</tr>
																</xsl:for-each>
																<tr>
																	<td class="determina">Note<br /><br /><br /></td>
																</tr>
																<tr>
																	<td colspan="5">
																		<table class="determina">
																			<tbody>
																				<tr>
																					<td width="60%" class="determina">Visto di regolarita' amministrativa e contabile - IL DIRIGENTE <xsl:value-of select="/datiDocumento/nomeDirigente" />
																			</td>
																					<td width="40%" class="determina">Data <xsl:value-of select="/datiDocumento/dataVisto" />
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
												<tr>
													<td class="determina">
														<table Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tr class="determina">
																<td class="determina">Atto soggetto a pubblicazione	 <xsl:value-of select="/datiDocumento/tipoPubblicazione" />  )Integrale  <xsl:value-of select="/datiDocumento/tipoPubblicazione" />   )Per estratto</td>
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
					<tr>
						<td class="paginaa4" >
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
										<td colspan="2" class="determina">L'istruttore  <xsl:value-of select="//compitidocumento/istruttore" /><br /><br /><br/></td>
									</tr>
									<tr>
										<td width="50%" class="determina">Il responsabile P.O.  <xsl:value-of select="//compitidocumento/responsabilepo" /> </td>
										<td width="50%" class="determina">Il Dirigente Generale  <xsl:value-of select="//compitidocumento/dirigentegen" /> </td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td class="paginaa4">
							<table border="0" width="100%" cellpadding="10" cellspacing="10" >
		<tbody>
			<tr>
				<td width="20%" class="determina">Oggetto</td>
				<td width="80%" class="determina">
					<xsl:value-of select="/datiDocumento/oggetto"/>
				</td>
			</tr>
		</tbody>
	</table>
	<table border="1" width="100%" cellpadding="10" cellspacing="10">
		<tbody>
			<tr>
				<td class="determina" >Osservazioni</td>
				</tr>
				<tr>
					<td class="determina" height="200px">
						<xsl:for-each select="/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni' and Dno_prog=1]">
						<xsl:value-of select="Dno_testo" /></xsl:for-each>
					</td>
				</tr>
				<tr>
					<td class="determina">Il dirigente Generale</td>
				</tr>			
		</tbody>
	</table>
	<table border="1" width="100%" cellpadding="10" cellspacing="10">
		<tbody>
			<tr>
				<td class="determina">Osservazioni</td>
			</tr>
			<tr>
				<td class="determina" height="200px">
					<xsl:for-each select="/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni' and Dno_prog=2]">
						<xsl:value-of select="Dno_testo" /></xsl:for-each>
				</td>
			</tr>
			<tr>
				<td class="determina" >Il dirigente d'ufficio</td>
			</tr>
		</tbody>
	</table>
	<table border="0" width="100%" cellpadding="10" cellspacing="10">
		<tbody>
			<tr>
				<td class="determina" border="1">Copia della presente determinazione viene trasmessa in data <xsl:value-of select="/datiDocumento/dataTrasmissione"/> al Presidente della Giunta regionale, alla Giunta, al Consiglio regionale, al Responsabile politico del Dipartimento proponente e al Bollettino Ufficiale della Regione. <br/>
					<br/> L'impiegato addetto
							</td>
			</tr>
		</tbody>
	</table>
						</td>
					</tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
