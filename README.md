<h1 align="center">Eaxir: HCE EAX Installer</h1>
<p align="center">
	A graphical installer for HCE that permits the installation of DSOAL & enabling of EAX.
	<br><br>
	<img src="https://user-images.githubusercontent.com/10241434/56606168-341c9180-6638-11e9-911c-a0e109a74fa0.png">
	<br><br>
	<a href="https://github.com/yumiris/HCE.Eaxir/releases/latest">Download</a>
</p>

# Introduction

The Environmental Audio Extensions (or EAX) are a number of digital signal
processing presets for audio, present in Creative Technology Sound Blaster sound
cards starting with the Sound Blaster Live and the Creative NOMAD/Creative ZEN
product lines. After the release of Windows Vista, which deprecated the
DirectSound3D API EAX was based on, Creative discouraged EAX implementation in
favor of its OpenAL-based EFX equivalent.

This project facilitates the installation and enabling of EAX on HCE by:

- adding the DSOAL files to the specified HCE directory;
- patching the player's profile to enable HWA  and EAX.

# Resources

- https://repo.or.cz/dsound-openal.git
- https://repo.or.cz/w/openal-soft.git

# Licence

All files under the src directory - minus the `DSOAL.zip` and unless otherwise
specified - are licenced under the licence specified in the COPYRIGHT file.