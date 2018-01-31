<h1 align="center">Eaxir</h1>
<p align="center">
  Library & GUI for enabling EAX on HCE
</p>

# Introduction

> The Environmental Audio Extensions (or EAX) are a number of digital signal processing presets for audio,
present in Creative Technology Sound Blaster sound cards starting with the Sound Blaster Live and the
Creative NOMAD/Creative ZEN product lines. After the release of Windows Vista, which deprecated the
DirectSound3D API EAX was based on, Creative discouraged EAX implementation in favor of its OpenAL-based EFX equivalent.

This project facilitates the installation and enabling of EAX on HCE by:

* adding the `dsound.dll` and `dsound.ini` library files to the specified HCE directory;
* patching the player's `blam.sav` to enable the Hardware Acceleration and Environmental Sound audio options
