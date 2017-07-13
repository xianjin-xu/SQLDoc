<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#160;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match = "/" >
    <html>
      <head>
        <title>Function Details</title>
        <LINK HREF="RadGrid_Office2007.css" REL="stylesheet" TYPE="text/css" />
        <LINK HREF="RadTabStrip.css" REL="stylesheet" TYPE="text/css" />
        <LINK HREF="RadTabStrip_Office2007.css" REL="stylesheet" TYPE="text/css" />
      </head>
      <body class="RadGrid RadGrid_Office2007">

        <table cellspacing="0" cellpadding="0" style="width:100%">
          <tr>
            <td>
              <div id="RadTabStrip1" class="RadTabStrip RadTabStrip_Office2007 RadTabStripTop_Office2007">
                <div class="rtsLevel rtsLevel1">
                  <ul class="rtsUL">
                    <li class="rtsLI rtsFirst rtsLast">
                      <a class="rtsLink rtsSelected" href="#">
                        <span class="rtsOut">
                          <span class="rtsIn">
                            <span class="rtsTxt">
                              <font style="font-weight:bold" size="2.5px">Function Details</font>
                            </span>
                          </span>
                        </span>
                      </a>
                    </li>
                  </ul>
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td style="border-top:solid 0px #8db2e3; border-right:solid 3px #8db2e3; border-bottom:solid 1px #8db2e3; border-left:solid 1px #8db2e3; padding:10px;">

              <font style="font-weight:bold" size="2px">Description</font>
                <hr class="RadGrid RadGrid_Office2007" />
                <font style="font-weight:normal" size="2px">
                  <xsl:if test="//NewDataSet/TableProperties/NAME='Description'">
                    <xsl:value-of select="//NewDataSet/TableProperties/VALUE"/>
                  </xsl:if>
                </font>
                <br/>
                <br/>

                <font style="font-weight:bold" size="2px">Function Properties</font>
                <hr class="RadGrid RadGrid_Office2007" />
                <div class="RadGrid RadGrid_Office2007" style="width:30%;">
                  <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfTables" >
                    <thead>
                      <tr class="GridRow_Office2007">
                        <th class="GridHeader_Office2007" width="20%">Name</th>
                        <th class="GridHeader_Office2007" width="80%">Value</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="//NewDataSet/FunctionProperties">
                        <tr class="GridRow_Office2007">
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(NAME) > 0">
                                <xsl:value-of select="NAME"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(VALUE) > 0">
                                <xsl:value-of select="VALUE"/>
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
                </div>
                <br/>
                <font style="font-weight:bold" size="2px">Columns</font>
                <hr class="RadGrid RadGrid_Office2007" />
                <div class="RadGrid RadGrid_Office2007" style="width:50%;">
                  <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfTables" >
                    <thead>
                      <tr class="GridRow_Office2007">
                        <th class="GridHeader_Office2007" width="10%">Name</th>
                        <th class="GridHeader_Office2007" width="10%">Data Type</th>
                        <th class="GridHeader_Office2007" width="10%">Max Length</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="//NewDataSet/FunctionINParameters">
                        <tr class="GridRow_Office2007">
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(PARAMETER_NAME) > 0">
                                <xsl:value-of select="PARAMETER_NAME"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(DATA_TYPE) > 0">
                                <xsl:value-of select="DATA_TYPE"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(CHARACTER_MAXIMUM_LENGTH) > 0">
                                <xsl:value-of select="CHARACTER_MAXIMUM_LENGTH"/>
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
                </div>
                <br/>
                <font style="font-weight:bold" size="2px">Recordset(s)</font>
                <hr class="RadGrid RadGrid_Office2007" />
                <div class="RadGrid RadGrid_Office2007" style="width:50%;">
                  <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfTables" >
                    <thead>
                      <tr class="GridRow_Office2007">
                        <th class="GridHeader_Office2007" width="10%">Name</th>
                        <th class="GridHeader_Office2007" width="10%">Data Type</th>
                        <th class="GridHeader_Office2007" width="10%">Max Length</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="//NewDataSet/FunctionINParameters">
                        <tr class="GridRow_Office2007">
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(PARAMETER_NAME) > 0">
                                <xsl:value-of select="PARAMETER_NAME"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(DATA_TYPE) > 0">
                                <xsl:value-of select="DATA_TYPE"/>
                              </xsl:when>
                              <xsl:otherwise>
                                &nbsp;
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td>
                            <xsl:choose>
                              <xsl:when test="string-length(CHARACTER_MAXIMUM_LENGTH) > 0">
                                <xsl:value-of select="CHARACTER_MAXIMUM_LENGTH"/>
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
                </div>
                <br/>
                <font style="font-weight:bold" size="2px">SQL</font>
                <hr class="RadGrid RadGrid_Office2007" />
                <div class="RadGrid RadGrid_Office2007" style="width:100%;">
                  <table class="MasterTable_Office2007" cellspacing="0"  style="width:100%;border-collapse:collapse;table-layout:auto;empty-cells:show;" id="ListOfTables" >
                    <thead>
                      <tr class="GridRow_Office2007">
                        <th class="GridHeader_Office2007" width="100%">Creation Script</th>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:for-each select="//NewDataSet/SQL">
                        <tr class="GridRow_Office2007">
                          <td>
                            <pre>
                              <xsl:value-of select="SQL"/>
                            </pre>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </div>
              </td>
            </tr>
          </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>