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
        POROVNÁNÍ SOURCE v. TARGET/>
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
            <xsl:value-of select="source/@database"/>           
          </TD>
          <TD>
            <xsl:value-of select="target/@database"/>
          </TD>
        </TR>
        <xsl:call-template name="TABLES_DIFF"></xsl:call-template>
        <xsl:call-template name="TABLES_COLL_DIFF"></xsl:call-template>
        <xsl:call-template name="VIEWS_DIFF"></xsl:call-template>
        <xsl:call-template name="VIEWS_TABLE_DIFF"></xsl:call-template>
        <xsl:call-template name="VIEWS_COLL_DIFF"></xsl:call-template>
        <xsl:call-template name="CONSTR_TABL_DIFF"></xsl:call-template>
        <xsl:call-template name="CONSTR_REFS_DIFF"></xsl:call-template>
        <xsl:call-template name="CONSTR_KEYS_DIFF"></xsl:call-template>
        <xsl:call-template name="CONSTR_CHECK_DIFF"></xsl:call-template>
      </TABLE>
    </DIV>
    <xsl:call-template name="TABLES_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_COLL_DETAIL"></xsl:call-template>
    <xsl:call-template name="VIEWS_DETAIL"></xsl:call-template>
    <xsl:call-template name="VIEWS_TABLE_DETAIL"></xsl:call-template>
    <xsl:call-template name="VIEWS_COLL_DETAIL"></xsl:call-template>
    <xsl:call-template name="CONSTR_TABL_DETAIL"></xsl:call-template>
    <xsl:call-template name="CONSTR_REFS_DETAIL"></xsl:call-template>
    <xsl:call-template name="CONSTR_KEYS_DETAIL"></xsl:call-template>
    <xsl:call-template name="CONSTR_CHECK_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_DATA_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_XROWS_DETAIL"></xsl:call-template>
    <xsl:call-template name="TABLES_ROWS_DETAIL"></xsl:call-template>
  </xsl:template>

  <xsl:template name="TABLES_DIFF" match="tables">
    <xsl:choose>
      <xsl:when test="tables">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="tables/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="tables/@diff_count"/></H3>
          </TD>
          <TD>
            <xsl:value-of select="tables/source/@count"/>
          </TD>
          <TD>
           <xsl:value-of select="tables/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="TABLES_DETAIL" match="tables">
    <xsl:if test="count(tables/tables_diff/add)!=0 or count(tables/tables_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY ROZDÍLY (<xsl:value-of select="tables/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="tables/tables_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="tables/tables_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="TABLES_COLL_DIFF" match="columns">
    <xsl:choose>
      <xsl:when test="columns">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="columns/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY SLOUPCE</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="columns/@diff_count"/></H3>
          </TD>
          <TD>
            <xsl:value-of select="columns/source/@count"/>
          </TD>
          <TD>
            <xsl:value-of select="columns/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="TABLES_COLL_DETAIL" match="columns">
    <xsl:if test="count(columns/table_columns_diff/add)!=0 or count(columns/table_columns_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY SLOUPCE ROZDÍLY (<xsl:value-of select="columns/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="columns/table_columns_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="columns/table_columns_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="VIEWS_DIFF" match="views">
    <xsl:choose>
      <xsl:when test="views">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="views/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>VIEW</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="views/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="views/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="views/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="VIEWS_DETAIL" match="views">
    <xsl:if test="count(views/views_diff/add)!=0 or count(views/views_diff/drop)!=0">
      <DIV>
        <H2>
          VIEW ROZDÍLY (<xsl:value-of select="views/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="views/views_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="views/views_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="VIEWS_TABLE_DIFF" match="view_tables">
    <xsl:choose>
      <xsl:when test="view_tables">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="view_tables/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>VIEW TABULKY</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="view_tables/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="view_tables/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="view_tables/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="VIEWS_TABLE_DETAIL" match="view_tables">
    <xsl:if test="count(view_tables/view_tables_diff/add)!=0 or count(view_tables/view_tables_diff/drop)!=0">
      <DIV>
        <H2>
          VIEW TABULKY ROZDÍLY (<xsl:value-of select="view_tables/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="view_tables/view_tables_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="view_tables/view_tables_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="VIEWS_COLL_DIFF" match="view_columns">
    <xsl:choose>
      <xsl:when test="view_columns">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="view_columns/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>VIEW SLOUPCE</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="view_columns/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="view_columns/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="view_columns/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="VIEWS_COLL_DETAIL" match="view_columns">
    <xsl:if test="count(view_columns/view_columns_diff/add)!=0 or count(view_columns/view_columns_diff/drop)!=0">
      <DIV>
        <H2>
          VIEW SLOUPCE ROZDÍLY (<xsl:value-of select="view_columns/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="view_columns/view_columns_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="view_columns/view_columns_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="CONSTR_TABL_DIFF" match="table_constraints">
    <xsl:choose>
      <xsl:when test="table_constraints">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="table_constraints/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY CONSTRAINS</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="table_constraints/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="table_constraints/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="table_constraints/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="CONSTR_TABL_DETAIL" match="table_constraints">
    <xsl:if test="count(table_constraints/table_constraints_diff/add)!=0 or count(table_constraints/table_constraints_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY CONSTRAINS ROZDÍLY (<xsl:value-of select="table_constraints/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="table_constraints/table_constraints_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="table_constraints/table_constraints_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="CONSTR_REFS_DIFF" match="refer_constraints">
    <xsl:choose>
      <xsl:when test="refer_constraints">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="refer_constraints/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY REFERENCES</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="refer_constraints/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="refer_constraints/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="refer_constraints/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="CONSTR_REFS_DETAIL" match="refer_constraints">
    <xsl:if test="count(refer_constraints/refer_constraints_diff/add)!=0 or count(refer_constraints/refer_constraints_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY REFERENCES ROZDÍLY (<xsl:value-of select="refer_constraints/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="refer_constraints/refer_constraints_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="refer_constraints/refer_constraints_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="CONSTR_KEYS_DIFF" match="keys_constraints">
    <xsl:choose>
      <xsl:when test="keys_constraints">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="keys_constraints/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY KEYS</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="keys_constraints/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="keys_constraints/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="keys_constraints/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="CONSTR_KEYS_DETAIL" match="keys_constraints">
    <xsl:if test="count(keys_constraints/key_constraints_diff/add)!=0 or count(keys_constraints/key_constraints_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY KEYS ROZDÍLY (<xsl:value-of select="keys_constraints/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="keys_constraints/key_constraints_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="keys_constraints/key_constraints_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
  </xsl:template>
  <xsl:template name="CONSTR_CHECK_DIFF" match="check_constraints">
    <xsl:choose>
      <xsl:when test="check_constraints">
        <!-- Found the node(s) -->
        <TR>
          <xsl:if test="check_constraints/@diff_count!=0">
            <xsl:attribute name="class">red_flag</xsl:attribute>
          </xsl:if>
          <TD>
            <H3>TABULKY CHECKS</H3>
          </TD>
          <TD>
            <H3><xsl:value-of select="check_constraints/@diff_count"/></H3>
          </TD>
          <TD>
                <xsl:value-of select="check_constraints/source/@count"/>
          </TD>
          <TD>
                <xsl:value-of select="check_constraints/target/@count"/>
          </TD>
        </TR>
      </xsl:when>
      <!-- more xsl:when here, if needed -->
      <xsl:otherwise>
           <!-- No node exists -->
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>
  <xsl:template name="CONSTR_CHECK_DETAIL" match="check_constraints">
    <xsl:if test="count(check_constraints/check_constraints_diff/add)!=0 or count(check_constraints/check_constraints_diff/drop)!=0">
      <DIV>
        <H2>
          TABULKY CHECKS ROZDÍLY (<xsl:value-of select="check_constraints/@diff_count"/>)
        </H2>
        <TABLE cellspacing="0">
          <COL width="200px"/>
          <COL width="250px"/>
          <COL width="250px"/>
          <TR>
            <TD>__TYPE__</TD>
            <TD>__NAME__</TD>
            <TD>__CHANGE__</TD>
          </TR>
          <xsl:for-each select="check_constraints/check_constraints_diff/add">
            <TR>
              <TD>
                <H3>__ADD__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
          <xsl:for-each select="check_constraints/check_constraints_diff/drop">
            <TR>
              <TD>
                <H3>__DROP__</H3>
              </TD>
              <TD>
                <xsl:value-of select="@type"/>
              </TD>
              <TD>
                <xsl:value-of select="@name"/>
              </TD>
            </TR>
          </xsl:for-each>
        </TABLE>
      </DIV>
    </xsl:if>
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
            <TD>COUNT OF <xsl:value-of select="/results/source/@database"/></TD>
            <TD>COUNT OF <xsl:value-of select="/results/target/@database"/></TD>
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
                <xsl:value-of select="source/@count"/>
              </TD>
              <TD>
                <xsl:value-of select="target/@count"/>
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
          <xsl:if test="count(xpk_diff_rows/diff_data/data_rows/*)!=0">
            <H3>
              <xsl:attribute name="class">red_flag</xsl:attribute>
              <xsl:value-of select="local-name()"/>
            </H3>
            <TABLE cellspacing="0">
              <TR>
                <xsl:for-each select="xpk_diff_rows/diff_data/data_cols/TD">
                  <TD>
                    <xsl:value-of select="."/>
                  </TD>
                </xsl:for-each>
              </TR>
              <xsl:for-each select="xpk_diff_rows/diff_data/data_rows/data_row">
                <TR>
                  <xsl:for-each select="TD">
                    <TD>
                      <xsl:value-of select="."/>
                    </TD>
                  </xsl:for-each>
                </TR>
                <xsl:if test="count(diff_data_colls/*)!=0">
                  <TR>
                    <TD>
                      <H3>
                        <xsl:attribute name="class">red_flag</xsl:attribute>
                        ROZDILY VE SLOUPCICH
                      </H3>
                    </TD>
                  </TR>
                  <TR>
                    <TD>SLOUPEC</TD>
                    <TD>NOVA HODNOTA</TD>
                    <TD>PUVODNI HODNOTA</TD>
                  </TR>
                </xsl:if>
                <xsl:for-each select="diff_data_colls/diff_data_coll">
                  <TR>
                    <xsl:for-each select="TD">
                      <TD>
                        <xsl:value-of select="."/>
                      </TD>
                    </xsl:for-each>
                  </TR>
                </xsl:for-each>
                <xsl:if test="count(diff_data_colls/*)!=0">
                  <TR>
                    <TD>
                      <H3>
                        <xsl:attribute name="class">red_flag</xsl:attribute>
                      <br/>
                      </H3>
                    </TD>
                  </TR>
                  <TR>
                    <TD>SLOUPEC</TD>
                    <TD>NOVA HODNOTA</TD>
                    <TD>PUVODNI HODNOTA</TD>
                  </TR>
                </xsl:if>
              </xsl:for-each>
            </TABLE>
          </xsl:if>
        </xsl:for-each>
      </DIV>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
