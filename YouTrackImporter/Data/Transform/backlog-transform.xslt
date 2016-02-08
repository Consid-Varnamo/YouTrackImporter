<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@* | node()">
    <issues>
      <xsl:for-each select="issue">
        <issue>
          <field name="numberInProject">
            <value>
              <xsl:value-of select="id" />
            </value>
          </field>
          <field name="summary">
            <value>
              <xsl:if test="string-length(description) &lt; 129">
                <xsl:value-of select="description"/>
              </xsl:if>
              <xsl:if test="string-length(description) &gt; 128">
                <xsl:value-of select="substring(description, 0, 255)"/>...
              </xsl:if>
            </value>
          </field>
          <field name="description">
            <value>
              <xsl:value-of select="description"/>
            </value>
          </field>
          <field name="created">
            <value>1454932784000</value>
          </field>
          <!--Must be a valid YouTrack username-->
          <field name="reporterName">
            <value>maria.borman</value>
          </field>
          <field name="subsystem">
            <value>
              <xsl:value-of select="area"/>
            </value>
          </field>
          <!--Should not be empty-->
          <field name="priority">
            <value>
              <xsl:choose>
                <xsl:when test="priority = ''">
                  <xsl:text>4 - Medium</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="priority"/>
                </xsl:otherwise>
              </xsl:choose>
            </value>
          </field>
          <!-- *** Skipped
          <field name="sprint">
            <value>
              <xsl:value-of select="sprint"/>
            </value>
          </field>-->
          <field name="assembly file version">
            <value>
              <xsl:value-of select="release"/>
            </value>
          </field>
          <field name="estimation hrs">
            <value>
              <xsl:value-of select="estimation-hrs"/>
            </value>
          </field>
          <!-- *** MISSING ***
          <field name="">
            <value>
              <xsl:value-of select="worked-hrs"/>
            </value>
          </field>-->
          <field name="Assignee">
            <value>
              <xsl:choose>
                <xsl:when test="assigned = 'Christer Heinbäck'">
                  <xsl:text>ChristerH</xsl:text>
                </xsl:when>
                <xsl:when test="assigned = 'Daniel Heinmert'">
                  <xsl:text>daniel</xsl:text>
                </xsl:when>
              </xsl:choose>
            </value>
          </field>
          <xsl:if test="roles != ''">
            <field name="Roll">
              <value>
                <xsl:value-of select="roles"/>
              </value>
            </field>
          </xsl:if>
          <xsl:if test="string-length(internal-comment) &gt; 0">
            <comment author="ChristerH" text="{internal-comment}" created="1454932784000"/>
          </xsl:if>
          <xsl:if test="string-length(external-comment) &gt; 0">
            <comment author="maria.borman" text="{external-comment}" created="1454932784000"/>
          </xsl:if>
        </issue>
      </xsl:for-each>
    </issues>
  </xsl:template>
</xsl:stylesheet>
