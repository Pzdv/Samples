<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="html" indent="yes"/>
	<xsl:key name="unique-Autor" match="Autor" use="."/>
	
	<xsl:template match="/">
		<html>
			<body>
				<h1>Autors</h1>
				<table>
					<tr>
						<th>Autor</th>
						<th>Books</th>
						<th>Total</th>
					</tr>
					<xsl:for-each select="//Autor[count(. | key('unique-Autor', .)[1]) = 1]">
						<tr>
							<td>
								<xsl:value-of select="current()"/>
							</td>
							<td>
								<xsl:for-each select="//Book/Autors[Autor = current()]/parent::*" >
									<xsl:value-of select="Name"/>
								</xsl:for-each>
							</td>
							<td>
								<xsl:value-of select="count(//*[Autor = current()]/parent::*)"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
				<h2>Total Autors: <xsl:value-of select="count(//Autor[count(. | key('unique-Autor', .)[1])=1])"/>
			</h2>
			</body>
		</html>
	</xsl:template>



</xsl:stylesheet>
