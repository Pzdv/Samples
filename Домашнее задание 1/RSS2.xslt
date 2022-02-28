<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
				exclude-result-prefixes="msxsl">

	<xsl:output method="xml" />

	<xsl:template match="/">
		<rss version="2.0">
			<channel>
				<title>LibraryStorage</title>
				<link>Required</link>
				<description>Required</description>
				<xsl:for-each select="/LibraryStorage/Items/child::*">
					<item>
						<title>
							<xsl:value-of select="Name"/>
						</title>
						<pubDate>
							<xsl:value-of select="PublishYear"/>
						</pubDate>
						<xsl:choose>
							<xsl:when test="name(.) = 'Book'">
								<Autor>
									<xsl:for-each select="Autors">
										<xsl:value-of select="Autor"/>
									</xsl:for-each>
								</Autor>
							</xsl:when>
							<xsl:when test="name(.) = 'Patent'">
								<Inventor>
									<xsl:for-each select="Inventors">
										<xsl:value-of select="Inventor"/>
									</xsl:for-each>
								</Inventor>
							</xsl:when>
							<xsl:otherwise>
								<PublishingHouse>
									<xsl:value-of select="PublishingHouse"/>
								</PublishingHouse>
							</xsl:otherwise>
						</xsl:choose>
					</item>
				</xsl:for-each>
			</channel>
		</rss>
	</xsl:template>

</xsl:stylesheet>
