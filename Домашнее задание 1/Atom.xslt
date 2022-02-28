<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		<xsl:for-each select="LibraryStorage/Items/child::*">
			<feed xmlns="http://www.w3.org/2005/Atom">
				<title>
					<xsl:value-of select="name(.)"/>
				</title>
				<subtitle>
					<xsl:value-of select="Name"/>
				</subtitle>
				<author>
					<xsl:apply-templates select="."/>
				</author>
			</feed>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="Book">
		<Autor>
			<xsl:for-each select="./Autors">
				<xsl:value-of select="./Autor"/>
			</xsl:for-each>
		</Autor>
	</xsl:template>

	<xsl:template match="Patent">
		<Autor>
			<xsl:for-each select="./Inventors">
				<xsl:value-of select="./Inventor"/>
			</xsl:for-each>
		</Autor>
	</xsl:template>

	<xsl:template match="Newspaper">
		<Autor>
			<xsl:value-of select="PublishingHouse"/>
		</Autor>
	</xsl:template>
	
</xsl:stylesheet>
