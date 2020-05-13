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
	margin:10pt ;
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
    font-size:10pt;
    border-color: #000000;
}
table.bordo
{
	border-width: 1px 1px 1px 1px;
	border-style:solid;
	padding:5px;	
    margin:5px;
    vertical-align:top;
    font-size:10pt;
    border-color: #000000;
}


td.determina
{
	vertical-align:top;
	border-width: 0px 0px 0px 0px;
	border-style:solid; 
	border-color: #000000;   
	font-size:10pt;
}

td.bordo 
{
	vertical-align:top;
	border-width: 1px 1px 1px 1px;
	border-style: solid;
	border-color: #000000;
	font-size:10pt;
}

				</style>
			</head>
			<body>
				<table Align="center" Border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr><td height="50"></td></tr>
					<tr>
						<td class="paginaa4">
							<table Align="center" Border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td class="determina" Align="center">
										<table class="determina" Align="center" Border="0" cellpadding="0" cellspacing="3" width="100%">
											<tr>
												<td Align="center" width="20%">
													<img src="http://localhost/AttiDigitali/risorse/immagini/logoRegione.gif" width="170" height="70"/>
												</td>
												<td valign="bottom" width="30%">
													<div class="bold" valign="bottom">
														<xsl:value-of select="/datiDocumento/descrdipartimento"/>
													</div>
												</td>
												<td width="50%">
													<table class="bordo" width="100%" cellpadding="3" cellspacing="3">
														<tr>
															<td class="determina" colspan="2" valign="top"><xsl:value-of select="/datiDocumento/descrufficio"/>
															</td>
														</tr>
														<tr>
															<td class="determina" width="55%" valign="bottom"><div class="bold">STRUTTURA PROPONENTE </div></td>
															<td class="determina" width="40%" valign="bottom"><div class="bold">COD. </div><xsl:value-of select="/datiDocumento/codufficio"/>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td/>
												<td/>
												<td>
													<table class="bordo" width="100%" align="left" cellpadding="3" cellspacing="3">
														<tr> 
															<td  class="determina" valign="bottom"><div class="bold">N° ORDINE DEL GIORNO</div></td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td/>
												<td align="center" bgcolor="#000000" valign="bottom">
													<p class="testatadoc">DELIBERAZIONE</p>
												</td>
												<td>
													<table  class="bordo"  width="100%" cellpadding="3" cellspacing="3">
														<tbody>
															<tr>
																<td class="determina" width="25%"><div class="bold">N° </div></td>
																<td class="determina" width="25%"></td>
																<td class="determina" width="25%"><div class="bold">DEL </div> </td>
																<td class="determina" width="25%"></td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr><td height="50"></td></tr>
								<tr>
									<td>
										<table width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td class="determina" colspan="4">
														<div class="grande">Si sottopone all'approvazione della Giunta Regionale l'allegato schema di deliberazione avente per oggetto:</div>
														
													</td>
													<td/>
													<td/>
												</tr>
												<tr><td height="10"></td></tr>
												<tr>
													<td class="bordo"  colspan="4" height="100">
															<xsl:value-of select="/datiDocumento/oggetto" />
													</td>
													<td/>
													<td/>
												</tr>
											</tbody>
											<tr><td height="20"></td></tr>
											<tr>
												<td colspan="4" width="100%" class="determina" style="border-width: 0px 0px 0px 0px"><div class="grande">( ) La presente deliberazione non comporta impegno contabile sul bilancio </div></td>
												<td/>
												<td/>
											</tr>
											<tr><td height="5"></td></tr>
											<tr class="sololinea">
												<td colspan="4" width="100%" class="determina" style="border-width: 0px 0px 0px 0px"><div class="grande">( ) La presente deliberazione comporta impegno contabile sul bilancio</div>
												 <xsl:value-of select="//Dbi_Bilancio" /></td>
												<td/>
												<td/>
											</tr>
											<tr><td height="20"></td></tr>
											<tr class="sololinea">
												<td class="determina" width="20%"><div class="grande">UPB </div> <xsl:value-of select="//Dbi_UPB" />
												</td>
												<td class="determina" width="20%"><div class="grande">Cap.</div> <xsl:value-of select="//Dbi_Cap" />
												</td>
												<td class="determina" width="20%"><div class="grande">per € </div> <xsl:value-of select="//Dbi_Costo" />
												</td>
											</tr>
											<tr> <td height="20"></td></tr>
											<tr>
												<td colspan="3" width="100%" class="determina" align="left">IL DIRIGENTE</td>
												<td/>
												<td/>
											</tr>
											<tr> <td height="20"></td></tr>
										</table>
									</td>
								</tr>
								<tr>
									<td >
										<table  width="100%" cellpadding="0" cellspacing="0">
											<tbody>
												<tr>
													<td >
														<table  class="bordo" width="100%" cellpadding="0" cellspacing="0">
															<tbody>
																<tr>
																	<td class="determina"><div class="bold">OSSERVAZIONI</div>
																		<xsl:for-each select="/datiDocumento/tabella[@nome_tabella='Documento_noteosservazioni' and Dno_prog=1]">
                                                						<xsl:value-of select="Dno_testo" /></xsl:for-each>
																	</td>
																</tr>
																<tr><td height="60"></td></tr>
																<tr>
																	<td class="determina">IL DIRIGENTE GENERALE</td>
																</tr>
															</tbody>
														</table>
													</td>
												</tr>
												<tr><td height="5"></td></tr>
												<tr>
													<td>
														<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
															<tbody>
																<tr>
																	<td>
																		<table class="determina" width="100%" cellpadding="5" cellspacing="0">
																			<tbody>
																				<tr>
																					<td class="determina" colspan="4"><div class="bold">UFFICIO RAGIONERIA GENERALE</div></td>
																				</tr>
																				<tr class="sololinea">
																					<td class="determina" width="40%">Prenotazione di impegno N.  <xsl:value-of select="//Drp_NImpegno"/>
																					</td>
																					<td class="determina" width="20%">UPB  <xsl:value-of select="//Drp_UPB"/>
																					</td>
																					<td class="determina" width="20%">Cap.  <xsl:value-of select="//Drp_Cap"/>
																					</td>
																				</tr>
																				<tr class="sololinea">
																					<td class="determina" width="60%" colspan="2">Assunto impegno contabile N.  <xsl:value-of select="//Dra_NContabile"/>
																					</td>
																					<td class="determina" width="20%">UPB <xsl:value-of select="//Dra_UPB"/>
																					</td>
																					<td class="determina" width="20%">Cap. <xsl:value-of select="//Dra_Cap"/>
																					</td>
																				</tr>
																				<tr class="sololinea">
																					<td class="determina" width="40%">Esercizio <xsl:value-of select="//Dra_Esercizio"/>
																					</td>
																					<td class="determina" width="20%">per € <xsl:value-of select="//Dra_Costo"/>
																					</td>
																					<td colspan="2"/>
																				</tr>
																			</tbody>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td>
																		<table class="determina" Border="0" width="100%" cellpadding="5" cellspacing="0">
																			<tbody>
																				<tr class="sololinea">
																					<td class="determina" width="40%">La liquidazione di € <xsl:value-of select="//Dli_Costo" />
																					</td>
																					<td class="determina" width="20%">UPB  <xsl:value-of select="//Dli_UPB" />
																					</td>
																					<td class="determina" width="20%">Cap.  <xsl:value-of select="//Dli_Cap" />
																					</td>
																					<td class="determina" width="20%">Esercizio  <xsl:value-of select="//Dli_Esercizio" />
																					</td>
																				</tr>
																				<tr class="sololinea">
																					<td class="determina" width="60%" colspan="2">rientra nell'ambito dell'impegno N° <xsl:value-of select="//Dli_NContabile" />
																					</td>
																					<td class="determina" width="20%">assunto con delibera N°<xsl:value-of select="//Dli_Num_assunzione" />
																					</td>
																					<td class="determina" width="20%">del <xsl:value-of select="//Dli_Data_Assunzione" />
																					</td>
																				</tr>
																			</tbody>
																		</table>
																	</td>
																</tr>
																<tr> <td height="15"></td></tr>
																<tr>
																	<td class="determina" colspan="4">IL DIRIGENTE DELL'UFFICIO</td>
																</tr>
															</tbody>
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
						<td class="paginaa4">
							<table Align="center" Border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td class="determina" Align="center">
										<table class="determina" Align="center" Border="0" cellpadding="0" cellspacing="3" width="100%">
											<tr>
												<td width="100%" Align="center">
													<img src="http://localhost/AttiDigitali/risorse/immagini/logoRegionetondo.gif"/>
												</td>
											</tr>
											<tr>
												<td>
													<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td width="70%">
																	<table width="70%" Border="0">
																		<tbody>
																			<tr>
																				<td class="determina">DELIBERAZIONE N. <xsl:value-of select="/datiDocumento/docnumeroutente"/>
																				</td>
																			</tr>
																			<tr>
																				<td class="determina">SEDUTA DEL <xsl:value-of select="/datiDocumento/dataSeduta"/>
																				</td>
																			</tr>
																		</tbody>
																	</table>
																</td>
																<td width="30%">
																	<table class="determina" style="border-width: 1px 1px 1px 1px " width="100%">
																		<tbody>
																			<tr>
																				<td class="determina">
																					<!-- erroneamente viene utilizzato l'ufficio <xsl:value-of select="/datiDocumento/descrdipartimento"/>-->
																					 <xsl:value-of select="/datiDocumento/descrdipartimento"/><br/>
																					 <xsl:value-of select="/datiDocumento/descrufficio"/>&#160;<xsl:value-of select="/datiDocumento/codufficio"/>
																				</td>
																			</tr>
																			<tr class="determina">
																				<td>Dipartimento</td>
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
													<table class="bordo" width="100%" cellpadding="3" cellspacing="0" height="100">
														<tbody>
															<tr>
																<td width="20%" class="determina">Oggetto<br/>
																</td>
																<td width="80%" class="determina">
																	<xsl:value-of select="/datiDocumento/oggetto"/>
																</td>
															</tr>
														</tbody>
													</table>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td class="determina">Relatore  <xsl:value-of select="/datiDocumento/nomeRelatore"/>
																</td>
															</tr>
															<tr>
																<td class="determina">	La Giunta, riunitasi il giorno __________<xsl:value-of select="/datiDocumento/dataSeduta"/> alle ore ________ <xsl:value-of select="/datiDocumento/oraSeduta"/> nella sede dell'Ente,	</td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
											<tr>
												<td height="100px"/>
											</tr>
											<tr>
												<td align="center" class="determina">
													<table align="center" width="80%" border="0" cellpadding="2" cellspacing="0">
														<tbody>
															<tr>
																<td width="80%" colspan="3" border="0"/>
																<td width="10%" class="determina">Presente</td>
																<td width="10%" class="determina">Assente</td>
															</tr>
															<xsl:for-each select="datiDocumento/componentegiunta">
																<xsl:sort select="ordine"/>
																<tr>
																	<td class="determina" style="border-width: 1px 0px 1px 1px ">
																		<xsl:value-of select="ordine"/>.</td>
																	<td class="determina" style="border-width: 1px 0px 1px 0px ">
																		<xsl:value-of select="nome"/>&#160;<b>
																			<xsl:value-of select="cognome"/>
																		</b>
																	</td>
																	<td class="determina" style="border-width: 1px 1px 1px 0px ">
																		<xsl:value-of select="carica"/>
																	</td>
																	<td class="determina" style="border-width: 1px 1px 1px 1px ">&#160;</td>
																	<td class="determina" style="border-width: 1px 1px 1px 1px ">&#160;</td>
																</tr>
															</xsl:for-each>
															<tr><td colspan="5">Segretario: _______________________</td></tr>
														</tbody>
													</table>
												</td>
											</tr>
											<tr>
												<td height="100px"/>
											</tr>
											<tr>
												<td>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td width="45%" class="determina">Ha deciso in merito all'argomento in oggetto, <br /> secondo quanto riportato nelle pagine successive.</td>
																<td width="5"></td>
																<td width="45%" class="determina" style="border-width: 1px 1px 1px 1px ">L'atto si compone di N.___________ <xsl:value-of select="/datiDocumento/numeroPagine"/> pagine compreso il frontespizio e di N._________ <xsl:value-of select="/datiDocumento/numeroAllegati"/>  Allegati </td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
											<tr>
												<td>
													<table class="bordo" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td>
																	<table class="determina" Border="0" width="100%" cellpadding="3" cellspacing="0">
																		<tbody>
																			<tr>
																				<td class="determina" colspan="4">UFFICIO RAGIONERIA GENERALE</td>
																			</tr>
																			<tr>
																				<td class="determina" width="40%">Prenotazione di impegno N.  <xsl:value-of select="//Drp_NImpegno"/>
																				</td>
																				<td class="determina" width="20%">UPB  <xsl:value-of select="//Drp_UPB"/>
																				</td>
																				<td class="determina" width="20%">Cap.  <xsl:value-of select="//Drp_Cap"/>
																				</td>
																				<td class="determina" width="20%">per €  <xsl:value-of select="//Drp_Costo"/>
																				</td>
																			</tr>
																			<tr>
																				<td class="determina" width="60%" colspan="2">Assunto impegno contabile N.  <xsl:value-of select="//Dra_NContabile"/>
																				</td>
																				<td class="determina" width="20%">UPB <xsl:value-of select="//Dra_UPB"/>
																				</td>
																				<td class="determina" width="20%">Cap. <xsl:value-of select="//Dra_Cap"/>
																				</td>
																			</tr>
																			<tr>
																				<td class="determina" width="40%">Esercizio <xsl:value-of select="//Dra_Esercizio"/>
																				</td>
																				<td class="determina" width="20%">per € <xsl:value-of select="//Dra_Costo"/>
																				</td>
																				<td colspan="2"/>
																			</tr>
																			<tr>
																				<td class="determina" colspan="4">IL DIRIGENTE </td>
																			</tr>
																		</tbody>
																	</table>
																</td>
															</tr>
															<tr>
																<td>
																	<table width="100%" cellpadding="3" cellspacing="0">
																		<tbody>
																			<tr>
																				<td class="determina" style="border-width: 0px 0px 0px 0px">Atto soggetto a pubblicazione  
																					<xsl:choose>
																						<xsl:when test="/datiDocumento/pubIntegrale = 1">( ) integrale (X) per estratto
																						</xsl:when>
																						<xsl:when test="/datiDocumento/pubIntegrale = 0">(X)integrale ( ) per estratto
																						</xsl:when>
																						<xsl:otherwise> ( )integrale ( ) per estratto
																						</xsl:otherwise>
																					</xsl:choose>	
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
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="paginaa4">
							<table>
								<tr>
									<td>
										<table class="determina" Border="0" width="100%" cellpadding="10" cellspacing="10">
											<tbody>
												<tr>
													<td stesto="1"/>
												</tr>
											</tbody>
										</table>
										<table class="determina" border="0" width="100%">
											<tbody>
												<tr>
													<td class="determina" colspan="2" height="50px">L'ISTRUTTORE  </td>
												</tr>
												<tr>
													<td class="determina" width="50%" height="50px">Il RESPONSABILE P.O. </td>
													<td class="determina" width="50%" height="50px">Il DIRIGENTE </td>
												</tr>
											</tbody>
										</table>
										<table border="1" width="100%" class="determina" valign="bottom">
											<tbody>
												<tr>
													<td class="determina">Tutti gli atti ai quali è fatto riferimento nella premessa e nel dispositivo della deliberazione sono depositati presso la struttura proponente, che ne curerà  la conservazione nei termini di legge.
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
						<td class="paginaa4">
							<table>
								<tr>
									<td>
										<table border="0" width="100%" class="determina">
											<tbody>
												<tr>
													<td colspan="2" class="determina">
														<br/>Del che è redatto il presente verbale che, letto e confermato, viene sottoscritto come segue:<br/>
														<br/>
													</td>
												</tr>
												<tr>
													<td class="determina">IL SEGRETARIO  <xsl:value-of select="/datiDocumento/nomePresidente"/>
														<xsl:value-of select="/datiDocumento/cognomePresidente"/>
													</td>
													<td class="determina">IL PRESIDENTE  <xsl:value-of select="/datiDocumento/nomeSegretario"/>
														<xsl:value-of select="/datiDocumento/cognomeSegretario"/>
													</td>
												</tr>
											</tbody>
										</table>
										<table border="0" width="100%" class="determina">
											<tbody>
												<tr>
													<td class="determina">Si attesta che copia conforme della presente deliberazione è stata trasmessa in data ___________ al Dipartimento interessato () al Consiglio Regionale () .<br/>
														<br/>
												L'IMPIEGATO ADDETTO
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
