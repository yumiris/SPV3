# Introduction

SPV3.Settings is the configuration program used for tweaking SPV3. The program serves as a front-end to configuring:

- SPV3's video/audio/input settings;
- environmental sound & hardware acceleration;
- OpenSauce post-processing options/effects.

Configuration of post-processing effects is done using SPV3.Shaders, which is included in this repository. SPV3.Settings
launches the SPV3.Shaders GUI and instructs it to load the provided `initc.txt` file which SPV3.Settings takes care of
detecting.

This program is designed to deal with options within the scope of SPV3. It abstracts the generalised routines of the
various libraries away from the end-user, and exposes only the necessary options that are relevant to SPV3.