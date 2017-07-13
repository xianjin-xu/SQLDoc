<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match = "/" >
    <html>
      <head>
        <title>List of Databases</title>
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
                        <font style="font-weight:bold" size="2.5px">List of Database(s)</font>
                      </span>
                    </span>
                  </span>
                </a>
              </li>
            </ul>
          </div>
        </div>
        <div style="padding:10px;border-right:solid 1px #8db2e3;border-bottom:solid 1px #8db2e3;border-left:solid 1px #8db2e3;">
          <div class="RadGrid RadGrid_Office2007" style="width:50%;">
            <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfDatabases" >
              <thead>
                <tr class="GridRow_Office2007">
                  <th class="GridHeader_Office2007" width="10%">&nbsp;</th>
                  <th class="GridHeader_Office2007" width="30%">Database ID</th>
                  <th class="GridHeader_Office2007" width="30%">Database Name</th>
                  <th class="GridHeader_Office2007" width="30%">Creation Date</th>
                </tr>
              </thead>
              <tbody>
                <xsl:for-each select="//Databases/Databases">
                  <tr class="GridRow_Office2007">
                    <td>
                      <img class="vmiddle" border="0" src="../images/table.gif" />
                    </td>
                    <td>
                      <xsl:value-of select="dbid"/>
                    </td>
                    <td>
                      <xsl:value-of select="database_name"/>
                    </td>
                    <td>
                      <xsl:value-of select="create_date"/>
                    </td>
                  </tr>
                </xsl:for-each>
              </tbody>
            </table>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>