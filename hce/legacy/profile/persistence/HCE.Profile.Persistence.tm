<TeXmacs|1.99.4>

<style|tmdoc>

<\body>
  <\doc-data|<doc-title|Profile Persistence in HCE>|<doc-subtitle|Serialising
  the Blam.sav Binary>|<doc-author|<\author-data|<author-name|Emilian
  Roman>|<author-homepage|<em|https://github.com/the-empress>>>
    \;

    \;
  </author-data>>|<\doc-date>
    <date>
  </doc-date>|<doc-misc|DRAFT>>
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

  <\table-of-contents|toc>
    <vspace*|1fn><with|font-series|bold|math-font-series|bold|1.<space|2spc>Video
    Configuration> <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-1><vspace|0.5fn>

    <with|par-left|1tab|1.1.<space|2spc>Resolution
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-2>>

    <with|par-left|2tab|1.1.1.<space|2spc>Calculation
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-3>>

    <with|par-left|2tab|1.1.2.<space|2spc>Retrieval
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-4>>

    <with|par-left|2tab|1.1.3.<space|2spc>Offsets
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-5>>

    <with|par-left|1tab|1.2.<space|2spc>Toggles
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-6>>

    <with|par-left|2tab|1.2.1.<space|2spc>Introduction
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-7>>

    <with|par-left|2tab|1.2.2.<space|2spc>Offsets
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-8>>

    <with|par-left|1tab|1.3.<space|2spc>Levels
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-9>>

    <with|par-left|2tab|1.3.1.<space|2spc>Introduction
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-10>>

    <with|par-left|2tab|1.3.2.<space|2spc>States
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-11>>

    <with|par-left|2tab|1.3.3.<space|2spc>Offsets
    <datoms|<macro|x|<repeat|<arg|x>|<with|font-series|medium|<with|font-size|1|<space|0.2fn>.<space|0.2fn>>>>>|<htab|5mm>>
    <no-break><pageref|auto-12>>
  </table-of-contents>

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

  <subsection|Toggles>

  <subsubsection|Introduction>

  This section covers video options that can be turned on and off; hence,
  they'd be stored as boolean values in the binary file. As far as toggles
  go, only the effects are toggles. Effects, in this context, refer to the
  following graphical options:

  <\itemize>
    <item>Specular - Effects and luster to objects

    <item>Shadows - Dynamic shadows\ 

    <item>Decals - Bullet holes and blood
  </itemize>

  <subsubsection|Offsets>

  <block*|<tformat|<twith|table-width|1par>|<twith|table-hmode|exact>|<table|<row|<cell|Option>|<cell|Address>>|<row|<cell|Specular>|<cell|0x00000A70>>|<row|<cell|Shadows>|<cell|0x00000A71>>|<row|<cell|Decals>|<cell|0x00000A72>>>>>

  <subsection|Levels>

  <subsubsection|Introduction>

  This section covers video options that can have different values to signify
  their current state. In the binary files, they're stored as unsigned bytes.
  Options that are \Plevels\Q include the following:

  <\itemize>
    <item>Frame Rate

    <item>Particles

    <item>Texture Quality
  </itemize>

  <subsubsection|States>

  <tabular*|<tformat|<twith|table-width|1par>|<twith|table-hmode|exact>|<table|<row|<cell|Frame
  Rate>|<cell|Particles>|<cell|Texture Quality>>|<row|<cell|<block*|<tformat|<table|<row|<cell|State>|<cell|Value>>|<row|<cell|VSync
  Off>|<cell|0x0>>|<row|<cell|VSync On>|<cell|0x1>>|<row|<cell|30
  FPS>|<cell|0x2>>>>>>|<cell|<block*|<tformat|<cwith|2|4|1|1|cell-width|100px>|<cwith|2|4|1|1|cell-hmode|exact>|<cwith|2|4|2|2|cell-width|100px>|<cwith|2|4|2|2|cell-hmode|exact>|<table|<row|<cell|State>|<cell|Value>>|<row|<cell|None>|<cell|0x0>>|<row|<cell|Low>|<cell|0x1>>|<row|<cell|Full>|<cell|0x2>>>>>>|<cell|<block*|<tformat|<cwith|2|4|1|1|cell-width|100px>|<cwith|2|4|1|1|cell-hmode|exact>|<cwith|2|4|2|2|cell-width|100px>|<cwith|2|4|2|2|cell-hmode|exact>|<table|<row|<cell|State>|<cell|Value>>|<row|<cell|Low>|<cell|0x0>>|<row|<cell|Medium>|<cell|0x1>>|<row|<cell|High>|<cell|0x2>>>>>>>>>>

  Enumerators could be used for representing these states:

  <\cpp-code>
    // enumerators

    enum framerate_t { vsync_on, no_vsync, locked };

    enum particles_t { none, low, full };

    enum qualities_t { low, medium, high }

    \;

    // for example, handling the chosen framerate

    framerate_t chosen_framerate;

    chosen_framerate = vsync_off;

    \;

    if (chosen_framerate == vsync_off)

    {

    \ \ \ \ unsigned int chosen_framerate = 1; // 0x1

    \ \ \ \ // write the chosen framerate to the file

    }
  </cpp-code>

  <subsubsection|Offsets>

  <block*|<tformat|<twith|table-width|1par>|<twith|table-hmode|exact>|<table|<row|<cell|Option>|<cell|Address>>|<row|<cell|Frame
  Rate>|<cell|0x00000A6F>>|<row|<cell|Particles>|<cell|0x00000A73>>|<row|<cell|Texture
  Quality>|<cell|0x00000A74>>>>>
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