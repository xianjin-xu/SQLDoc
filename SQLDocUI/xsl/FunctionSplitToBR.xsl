<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:template name="SplitToBR">
    <xsl:param name="InputData" />
    <xsl:variable name="spliter">|</xsl:variable>
    <xsl:variable name="NormalizedData" select="concat(normalize-space($InputData), $spliter)" />
    <xsl:variable name="leftString" select="substring-before($NormalizedData, $spliter)" />
    <xsl:variable name="remainedString" select="substring-after($NormalizedData, $spliter)" />
    <xsl:value-of select="$leftString" />
    <br/>
    <xsl:if test="substring-before($remainedString, $spliter) != ''">
      <xsl:call-template name="SplitToBR">
        <xsl:with-param name="InputData" select="$remainedString" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>


