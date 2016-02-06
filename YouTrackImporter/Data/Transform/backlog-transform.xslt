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
            <value>1454688000</value>
          </field>
          <field name="reporter">
            <value>Maria_Börman</value>
          </field>
          <field name="subsystem">
            <value>
              <xsl:value-of select="area"/>
            </value>
          </field>
          <field name="priority">
            <value>
              <xsl:value-of select="priority"/>
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
          <!--Assigned should be here-->
          <field name="Roll">
            <value>
              <xsl:value-of select="roles"/>
            </value>
          </field>
          <xsl:if test="string-length(internal-comment) &gt; 0">
            <comment author="ChristerH" text="{internal-comment}" created="1454688000"/>
          </xsl:if>
          <xsl:if test="string-length(external-comment) &gt; 0">
            <comment author="Maria_Börman" text="{external-comment}" created="1454688000"/>
          </xsl:if>
        </issue>
      </xsl:for-each>
    </issues>
  </xsl:template>
</xsl:stylesheet>
