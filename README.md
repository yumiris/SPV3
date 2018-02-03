<h1 align="center">Eaxir</h1>
<p align="center">
  Library & GUI for enabling EAX on HCE
</p>

# Introduction

The Environmental Audio Extensions (or EAX) are a number of digital signal processing presets for audio,
present in Creative Technology Sound Blaster sound cards starting with the Sound Blaster Live and the
Creative NOMAD/Creative ZEN product lines. After the release of Windows Vista, which deprecated the
DirectSound3D API EAX was based on, Creative discouraged EAX implementation in favor of its OpenAL-based EFX equivalent.

This project facilitates the installation and enabling of EAX on HCE by:

* adding the `dsound.dll` and `dsound.ini` library files to the specified HCE directory;
* patching the player's `blam.sav` to enable the Hardware Acceleration and Environmental Sound audio options

# Configuration Details

## Default Values

| Variable             | Default Value |
|----------------------|---------------|
| Buffers              | 4             |
| Duration             | 25            |
| Maximum Voice Count  | 128           |
| Disable Direct Music | 0             |

## Values' Description

_The information below has been paraphrased from the [Creative ALchemy Quick Start Guide](https://community.pcgamingwiki.com/files/file/284-creative-alchemy-quick-start-guide/). Cheers to /u/Vencen-Hudder for providing the resources._

**Buffers**

Used to set the number of audio buffers used internally. The default value of 4
should be fine for most applications.

**Duration**

Used to set the length in milliseconds of each of the audio buffers. The
default value is 25ms.

The total duration of the audio queue used internally is equal to Buffers * Duration (i.e.
100ms by default). Experimenting with Duration values may be necessary in order to
find the best performance vs. quality trade-off for each game. In addition, some games
require smaller values than the default of 25ms because they use very small DirectSound
Buffers for streaming, or they require faster playback position updates. Reducing the Duration value can prevent audio glitches, pops and clicks. However, lower values
mean that there is more chance of the audio breaking up during CPU intensive moments
(e.g. lots of disc access during level loading). The recommended approach is to try the
default settings, and if audio artifacts are regularly heard then try lowering Duration by
5ms and trying again. If the problem still occurs try dropping the value by another 5ms
and so on (minimum allowed value is 5ms).

**Maximum Voice Count**

Used to set the maximum number of hardware voices that
will be used by ALchemy. The number of voices used will be the lesser of, the hardware
voice count limit and this setting. The default is 128 which is the highest number of
voices available on SB X-Fi cards. By lowering this value, hardware voices can be
reserved for another application to use, or, to improve performance by streaming less
audio channels.

**Disable Direct Music**

Used to disable DirectMusic support. The default is false
(unchecked), meaning DirectMusic support is enabled. At this time no known problems
have been caused by combining ALchemy with games, such as TRON 2.0, that use
DirectMusic.
