<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:OKsystem="urn:my-scripts" version="1.0">
  <xsl:output method="html" indent="yes" encoding="UTF-8"/>
  <xsl:template match="/results">
    <html>
      <head>
        <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
        <title>Porovnání databáze</title>
        <LINK rel="stylesheet" type="text/css" href="html_export_result.css"/>
      </head>
        <xsl:call-template name="RESULTS"></xsl:call-template>
      <body>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="RESULTS" match="results">
    <DIV>
      <H3>
        NOVA HODNOTA v. PUVODNI HODNOTA/>
      </H3>
      <TABLE cellspacing="0">
        <COL width="230px"/>
        <COL width="50px"/>
        <COL width="250px"/>
        <COL width="250px"/>
        <TR>
          <TD>__POROVNANI__</TD>
          <TD></TD>
          <TD>
            <xsl:value-of select="target/@database"/>
          </TD>
          <TD>
            <xsl:value-of select="source/@database"/>           
          </TD>
        </TR>
      </TABLE>
    </DIV>
    <xsl:call-template name="TABLES_DATA_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_XROWS_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_ROWS_DETAIL"></xsl:call-template>
  </xsl:template>

  <xsl:template name="TABLES_DATA_DETAIL" match="table_data">
   <xsl:if test="count(table_data/*)!=0">
     <DIV>
        <H2>TABULKY ROZDÍLY DATA</H2>
        <TABLE cellspacing="0">
          <COL width="250px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TABLE__</TD>
            <TD>__DIFF COUNT__</TD>
            <TD>COUNT OF <xsl:value-of select="/results/target/@database"/></TD>
            <TD>COUNT OF <xsl:value-of select="/results/source/@database"/></TD>
            <TD>__DIFF ROWS__</TD>
            <TD>__DIFF XPKS__</TD>
          </TR>
          <xsl:for-each select="table_data/*">
            <TR>
              <xsl:if test="@diff_count!=0 or diff_rows/@count!=0 or xpk_diff_rows/@count!=0">
                <xsl:attribute name="class">red_flag</xsl:attribute>
              </xsl:if>
              <TD>
                <H3><xsl:value-of select="local-name()"/></H3>
              </TD>
              <TD>
                <xsl:value-of select="@diff_count"/>
              </TD>
              <TD>
                <xsl:value-of select="target/@count"/>
              </TD>
              <TD>
                <xsl:value-of select="source/@count"/>
              </TD>
              <TD>
                <xsl:value-of select="diff_rows/@count"/>
              </TD>
              <TD>
                <xsl:value-of select="xpk_diff_rows/@count"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="TABLES_ROWS_DETAIL" match="table_data">
    <xsl:if test="count(table_data/*/diff_rows)!=0">
      <DIV>
        <H2>
          <xsl:if test="count(table_data/*)!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          PRIDANE RADKY S NOVYM XPK
        </H2>
        <xsl:for-each select="table_data/*">
          <xsl:if test="count(diff_rows/diff_data/data_rows/*)!=0">
            <H3>
              <xsl:attribute name="class">red_flag</xsl:attribute>
              <xsl:value-of select="local-name()"/>
            </H3>
            <TABLE cellspacing="0">
              <TR>
                <xsl:for-each select="diff_rows/diff_data/data_cols/TD">
                  <TD>
                    <xsl:value-of select="."/>
                  </TD>
                </xsl:for-each>
              </TR>
              <xsl:for-each select="diff_rows/diff_data/data_rows/data_row">
                <TR>
                  <xsl:for-each select="TD">
                    <TD>
                      <xsl:value-of select="."/>
                    </TD>
                  </xsl:for-each>
                </TR>
              </xsl:for-each>
            </TABLE>
         </xsl:if>
       </xsl:for-each>
       </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="TABLES_XROWS_DETAIL" match="table_data">
    <xsl:if test="count(table_data/*/xpk_diff_rows)!=0">
      <DIV>
        <H2>
          <xsl:if test="count(table_data/*)!=0">
           <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          ZMENENE RADKY S EXISTUJICIM XPK
        </H2>
        <xsl:for-each select="table_data/*">
          <xsl:if test="count(xpk_diff_rows/diff_data/*)!=0">
            <H3>
              <xsl:attribute name="class">red_flag</xsl:attribute>
              <xsl:value-of select="local-name()"/>
            </H3>
            <TABLE cellspacing="0">
              <TR>
                <TD>SLOUPEC</TD>
                <TD>
                  <xsl:value-of select="/results/target/@database"/>
                </TD>
                <TD>
                  <xsl:value-of select="/results/source/@database"/>
                </TD>
              </TR>
              <TR>
                <TD></TD>
                <TD>NOVA HODNOTA</TD>
                <TD>PUVODNI HODNOTA</TD>
              </TR>
              <xsl:for-each select="xpk_diff_rows/diff_data/data_rows/data_row/TR">
               <TR>
                 <xsl:if test="./@diff_colls!=0">
                   <xsl:attribute name="class">red_flag</xsl:attribute>
                 </xsl:if>
                 <xsl:for-each select="TD">
                   <TD>
                     <xsl:value-of select="."/> 
                   </TD>
                 </xsl:for-each>
               </TR>
             </xsl:for-each>
            </TABLE>
          </xsl:if>
        </xsl:for-each>
      </DIV>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
