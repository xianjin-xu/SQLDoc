<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match = "/" >
    <html>
      <head>
        <title>DataSourceInformation</title>
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
                        <font style="font-weight:bold" size="2.5px">DataSource Information</font>
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
            <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="DataSourceInformation" >
              <thead>
                <tr class="GridRow_Office2007">
                  <th class="GridHeader_Office2007" width="05%">&nbsp;</th>
                  <th class="GridHeader_Office2007" width="15%">Name</th>
                  <th class="GridHeader_Office2007" width="40%">Value</th>
                  <th class="GridHeader_Office2007" width="40%">Description</th>
                </tr>
              </thead>
              <tbody>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="10%">CompositeIdentifierSeparatorPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/CompositeIdentifierSeparatorPattern"/>
                  </td>
                  <td>Separator for multipart names, (Example, the dot in pubs.dbo.authors)</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="10%">DataSourceProductName</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/DataSourceProductName"/>
                  </td>
                  <td>Database name</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="34%">DataSourceProductVersion</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/DataSourceProductVersion"/>
                  </td>
                  <td>Database version. [This is the version of the database instance currently being accessed.]</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="05%">DataSourceProductVersionNormalized</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/DataSourceProductVersionNormalized"/>
                  </td>
                  <td>A normalized DataSource version for easier comparison between different versions.</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="06%">GroupByBehavior</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/GroupByBehavior"/>
                  </td>
                  <td>Enumeration, System.Data.Common.GroupByBehavior</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="10%">IdentifierPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/IdentifierPattern"/>
                  </td>
                  <td>Regular expression string</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="34%">IdentifierCase</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/IdentifierCase"/>
                  </td>
                  <td>Enumeration, System.Data.Common.IdentifierCase</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="05%">OrderByColumnsInSelect</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/OrderByColumnsInSelect"/>
                  </td>
                  <td>Boolean, should you ORDER BY the columns in a SELECT statement by default</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="06%">ParameterMarkerFormat</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/ParameterMarkerFormat"/>
                  </td>
                  <td>Indicates whether parameter markers begin with a special character (Example, @ for T-SQL)</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="10%">ParameterMarkerPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/ParameterMarkerPattern"/>
                  </td>
                  <td>Regular expression string, used to create parameters</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="34%">ParameterNameMaxLength</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/ParameterNameMaxLength"/>
                  </td>
                  <td>Maximum length of a parameter</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="05%">ParameterNamePattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/ParameterNamePattern"/>
                  </td>
                  <td>Regular expression string, used to create parameters</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="06%">QuotedIdentifierPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/QuotedIdentifierPattern"/>
                  </td>
                  <td>Regular expression string, used to quote identifiers</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="10%">QuotedIdentifierCase</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/QuotedIdentifierCase"/>
                  </td>
                  <td>Enumeration, System.Data.Common.IdentifierCase</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="34%">StatementSeparatorPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/StatementSeparatorPattern"/>
                  </td>
                  <td>Regular expression string</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="05%">StringLiteralPattern</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/StringLiteralPattern"/>
                  </td>
                  <td>Regular expression string</td>
                </tr>
                <tr class="GridRow_Office2007">
                  <td>
                    <img class="vmiddle" border="0" src="../images/table.gif" />
                  </td>
                  <td width="06%">SupportedJoinOperators</td>
                  <td>
                    <xsl:value-of select="//DataSourceInformation/DataSourceInformation/SupportedJoinOperators"/>
                  </td>
                  <td>Enumeration, System.Data.Common.SupportedJoinOperators</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>