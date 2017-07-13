<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match = "/" >

    <html>
      <head>
        <title>List of Views</title>
        <LINK HREF="RadGrid_Office2007.css" REL="stylesheet" TYPE="text/css" />
        <LINK HREF="RadTabStrip.css" REL="stylesheet" TYPE="text/css" />
        <LINK HREF="RadTabStrip_Office2007.css" REL="stylesheet" TYPE="text/css" />
        <script type="text/javascript" src="sorttable.js"></script>
      </head>
      <body class="RadGrid RadGrid_Office2007">
        <div id="RadTabStrip1" class="RadTabStrip RadTabStrip_Office2007 RadTabStripTop_Office2007 ">
          <div class="rtsLevel rtsLevel1">
            <ul class="rtsUL">
              <li class="rtsLI rtsFirst rtsLast">
                <a class="rtsLink rtsSelected" href="#">
                  <span class="rtsOut">
                    <span class="rtsIn">
                      <span class="rtsTxt">
                        <font style="font-weight:bold" size="2.5px">List Of View(s)</font>
                      </span>
                    </span>
                  </span>
                </a>
              </li>
            </ul>
          </div>
        </div>
        <div style="padding:10px;border-right:solid 1px #8db2e3;border-bottom:solid 1px #8db2e3;border-left:solid 1px #8db2e3;">
          <div class="RadGrid RadGrid_Office2007" style="width:100%;">
            <table cellspacing="0" cellpadding="0" style="width:100%">
              <tr>
                <td>
                  <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfViews" >
                    <thead>
                      <tr class="GridRow_Office2007">
                        <th class="GridHeader_Office2007" width="03%">&nbsp;</th>
                        <th class="GridHeader_Office2007" width="15%">Name</th>
                        <th class="GridHeader_Office2007" width="12%">Schema</th>
                        <th class="GridHeader_Office2007" width="50%">Description</th>
                        <th class="GridHeader_Office2007" width="10%">Check Option</th>
                        <th class="GridHeader_Office2007" width="10%">Is Updatable</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="//ViewList/Views">
                        <tr class="GridRow_Office2007">
                          <td>
                            <img class="vmiddle" border="0" src="../images/table.gif" />
                          </td>
                          <td>
                            <a>
                              <xsl:attribute name="href">
                                <xsl:value-of select="TABLE_NAME" disable-output-escaping="yes"  />.htm
                              </xsl:attribute>
                              <xsl:value-of select="TABLE_NAME"/>
                            </a>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(TABLE_SCHEMA) > 0">
                                <xsl:value-of select="TABLE_SCHEMA"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(TABLE_DESCRIPTION) > 0">
                                <xsl:value-of select="TABLE_DESCRIPTION"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td align="right">
                            <xsl:choose>
                              <xsl:when test="string-length(CHECK_OPTION) > 0">
                                <xsl:value-of select="CHECK_OPTION"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td align="right">
                            <xsl:choose>
                              <xsl:when test="string-length(IS_UPDATABLE) > 0">
                                <xsl:value-of select="IS_UPDATABLE"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>