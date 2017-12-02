<TeXmacs|1.99.4>

<style|tmdoc>

<\body>
  <\doc-data|<doc-title|Profile Persistence in HCE>|<doc-subtitle|Serialising
  the Blam.sav Binary>|<doc-author|<\author-data|<author-name|Emilian
  Roman>|<author-homepage|<em|https://github.com/the-empress>>>
    \;

    \;
  </author-data>>|<doc-date|<date>>>
    \;
  </doc-data>

  <abstract-data|<\abstract>
    This paper covers the profile properties that are stored in the blam.sav
    binary, which is generated by HCE in the savegames directory, for the
    purpose of allowing Tiara CE to implement 1:1 compatibility with a user's
    HCE profile.

    The methodology, offsets and attributes of the properties are showcased
    with descriptions and code examples.
  </abstract>>

  <section|Video Configuration>

  <subsection|Resolution>

  <subsubsection|Calculation>

  The width and height will each be represented by two unsigned short
  variables, A & B, whose values are calculated using the following formula:

  <\cpp-code>
    unsigned int a = x / (2 ^ 8); // 2 ^ 8 = 256 or 0x100

    unsigned int b = x % (2 ^ 8); // x = width or height!
  </cpp-code>

  <\itemize>
    <item>The dividend is the width or height value, which can be an unsigned
    short of a value up to 2^15 (32768 or 0x8000).

    <item>The divisor can be represented with the magic unsigned integer of
    2^8.

    <item>Quotient \PA\Q is the result <em|without> a remainder when dividing
    by 2^8.
  </itemize>

  For example, 1920x1080 is represented in the blam.sav with the following
  values:

  <\cpp-code>
    unsigned int x = 1920; // width

    unsigned int y = 1080; // height

    \;

    unsigned int a = x / (2 ^ 8); // = 7

    unsigned int b = x % (2 ^ 8); // = 128

    \;

    unsigned int c = y / (2 ^ 8); // = 4

    unsigned int d = y % (2 ^ 8); // = 56
  </cpp-code>

  <new-page>

  <subsubsection|Retrieval>

  To get back the values from the blam.sav with the two variables, the
  following formula can be used: <cpp|unsigned int x = (a * (2 ^ 8)) + b>

  To get 1920 and 1080 back, we should use 128 & 7 and 56 & 4, respectively:

  <\cpp-code>
    unsigned int x = (7 * (2 ^ 8)) + 128 = 1920; // width

    unsigned int y = (4 * (2 ^ 8)) + 56 \ = 1080; // height
  </cpp-code>

  <subsubsection|Offsets>

  <tabular*|<tformat|<twith|table-width|1par>|<twith|table-hmode|exact>|<table|<row|<cell|Width>|<cell|Height>>|<row|<cell|<block*|<tformat|<table|<row|<cell|Value>|<cell|Address>>|<row|<cell|X
  / (2 ^ 8)>|<cell|0x00000A69>>|<row|<cell|X % (2 ^
  8)>|<cell|0x00000A68>>>>>>|<cell|<block*|<tformat|<table|<row|<cell|Value>|<cell|Address>>|<row|<cell|Y
  / (2 ^ 8)>|<cell|0x00000A6B>>|<row|<cell|Y % (2 ^
  8)>|<cell|0x00000A6A>>>>>>>>>>

  <subsection|Effects>

  <subsubsection|Summary>

  Effects, in this context, refer to the following graphical options:

  <\itemize>
    <item>Specular - Effects and luster to objects

    <item>Shadows - Dynamic shadows\ 

    <item>Decals - Bullet holes and blood
  </itemize>

  All three options are stored as booleans in the blam.sav, with 1 and 0
  representing on and off, respectively.

  <subsubsection|Offsets>

  <block*|<tformat|<twith|table-width|1par>|<twith|table-hmode|exact>|<table|<row|<cell|Option>|<cell|Address>>|<row|<cell|Specular>|<cell|0x00000A70>>|<row|<cell|Shadows>|<cell|0x00000A71>>|<row|<cell|Decals>|<cell|0x00000A72>>>>>
</body>

<\initial>
  <\collection>
    <associate|font-family|tt>
    <associate|page-even-footer|>
    <associate|page-even-header|>
    <associate|page-medium|paper>
    <associate|page-odd-footer|>
    <associate|page-odd-header|>
  </collection>
</initial>