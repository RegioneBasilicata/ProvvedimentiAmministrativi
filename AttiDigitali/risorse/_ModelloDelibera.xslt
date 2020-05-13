<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
			<head/>
			<body>
				<table Align="center" Border="1" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td>
							<table Align="center" Border="0" cellpadding="0" cellspacing="3" width="100%">
								<tr>
									<td align="center"> DELIBERE DIRIGENZIALI</td>
								</tr>
								<tr>
									<td>
										<table Border="0" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td></td>
													<td>Dipartimento</td>
													<td>Struttura<xsl:value-of select="/datiDocumento/descrStruttura"/></td>
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
													<td>Oggetto<br/>
														<xsl:value-of select="/datiDocumento/oggetto"/>
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
												<td>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td colspan="4">La presente determinazione comporta impegno contabile sul: </td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_bilancio']">
																<tr>
																	<td width="25%">Bilancio: <xsl:value-of select="Dbi_Bilancio"/>
																	</td>
																	<td width="25%">UPB:  <xsl:value-of select="Dbi_UPB"/>
																	</td>
																	<td width="25%">Cap.  <xsl:value-of select="Dbi_Cap"/>
																	</td>
																	<td width="25%"> per € <xsl:value-of select="Dbi_Costo"/>
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
													<table Border="0" width="100%" cellpadding="3" cellspacing="0">
														<tbody>
															<tr>
																<td colspan="4">Come da presente determinazione di procede alla liquidazione della somma</td>
															</tr>
															<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_liquidazione']">
																<tr>
																	<td width="25%">di € 
																		<xsl:value-of select="Dli_Costo"/>
																	</td>
																	<td width="25%">sul Cap.
																		<xsl:value-of select="Dli_Cap"/>
																	</td>
																	<td width="25%">UPB
																		<xsl:value-of select="Dli_UPB"/>
																	</td>
																	<td width="25%">Esercizio
																		<xsl:value-of select="Dli_Esercizio"/>
																	</td>
																</tr>
																<tr>
																	<td width="25%">in base all'impegno contabile N° <xsl:value-of select="Dli_NContabile"/>
																	</td>
																	<td width="25%">assunto con
																		<xsl:value-of select="Dli_TipoAssunzione"/>
																	</td>
																	<td width="25%">N°
																		<xsl:value-of select="Dli_Num_assunzione"/>
																	</td>
																	<td width="25%">del
																		<xsl:value-of select="Dli_Data_Assunzione"/>
																	</td>
																</tr>
															</xsl:for-each>
														</tbody>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<table Border="1" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td>
														<table Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tbody>
																<tr>
																	<td>UFFICIO RAGIONERIA</td>
																</tr>
																	<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_assunzione']">
																<tr>
																	<td width="25%">Assunto impegno contabile N° <xsl:value-of select="Dbi_Bilancio"/>
																	</td>
																	<td width="25%">UPB:  <xsl:value-of select="Dbi_UPB"/>
																	</td>
																	<td width="25%">Cap.  <xsl:value-of select="Dbi_Cap"/>
																	</td>
																	<td width="25%">Esercizio  <xsl:value-of select="Dbi_Cap"/>
																	</td>
																	<td width="25%"> per € <xsl:value-of select="Dbi_Costo"/>
																	</td>
																</tr>
															</xsl:for-each>
															</tbody>
														</table>
														<table Border="0" width="100%" cellpadding="3" cellspacing="0">
															<tbody>
																						<xsl:for-each select="datiDocumento/tabella[@nome_tabella='Documento_rag_liquidazione']">
																<tr>
																	<td width="25%">Bilancio: <xsl:value-of select="Dbi_Bilancio"/>
																	</td>
																	<td width="25%">UPB:  <xsl:value-of select="Dbi_UPB"/>
																	</td>
																	<td width="25%">Cap.  <xsl:value-of select="Dbi_Cap"/>
																	</td>
																	<td width="25%"> per € <xsl:value-of select="Dbi_Costo"/>
																	</td>
																</tr>
															</xsl:for-each>
																<tr>
																	<td>note</td>
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
									<td>atto soggetto</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr><td>										<table Border="1" width="100%" cellpadding="3" cellspacing="0">
											<tbody>
												<tr>
													<td>Testo<br/>
														<xsl:value-of select="/datiDocumento/testo"/>
													</td>
												</tr>
											</tbody>
										</table>
</td></tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
