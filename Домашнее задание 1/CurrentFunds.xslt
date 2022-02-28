<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="html" indent="yes"/>

	<xsl:template match="/LibraryStorage/Items">
		<html>
			<body>
				<h1>
					<CurrentDate>
						<xsl:value-of select="current-date()"/>
					</CurrentDate>
				</h1>
				<h2>Books</h2>
				<table>
					<tr>
						<th>Id</th>
						<th>Title</th>
						<th>Autor</th>
						<th>ISBN</th>
					</tr>
					<xsl:for-each select="//Book">
						<tr>
							<td>
								<xsl:value-of select="Id"/>
							</td>
							<td>
								<xsl:value-of select="Name"/>
							</td>
							<td>
								<xsl:for-each select="Autors">
									<xsl:value-of select="Autor"/>
								</xsl:for-each>
							</td>
							<td>
								<xsl:value-of select="ISBN"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
				<h3>Total books: <xsl:value-of select="count(//Book)"/></h3>
				<h2>Newspapers</h2>
				<table>
					<tr>
						<th>Id</th>
						<th>Title</th>
						<th>PublishingHouse</th>
						<th>ISSN</th>
					</tr>
					<xsl:for-each select="//Newspaper">
						<tr>
							<td>
								<xsl:value-of select="Id"/>
							</td>
							<td>
								<xsl:value-of select="Name"/>
							</td>
							<td>
								<xsl:value-of select="PublishingHouse"/>
							</td>
							<td>
								<xsl:value-of select="ISSN"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
				<h3>Total newspapers: <xsl:value-of select="count(//Newspaper)"/></h3>
				<h2>Patents</h2>
				<table>
					<tr>
						<th>Id</th>
						<th>Title</th>
						<th>Inventor</th>
						<th>RegistrationNumber</th>
					</tr>
					<xsl:for-each select="//Patent">
						<tr>
							<td>
								<xsl:value-of select="Id"/>
							</td>
							<td>
								<xsl:value-of select="Name"/>
							</td>
							<td>
								<xsl:for-each select="Inventors">
									<xsl:value-of select="Inventor"/>
								</xsl:for-each>
							</td>
							<td>
								<xsl:value-of select="RegistrationNumber"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
				<h3>Total patents: <xsl:value-of select="count(//Patent)"/></h3>
			</body>
		</html>
		<h3>Total: <xsl:value-of select="count(//Items/child::*)"/>
	</h3>
	</xsl:template>

</xsl:stylesheet>
