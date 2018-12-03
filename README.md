# Introduction

SPV3.Settings is the configuration program used for tweaking SPV3. The program serves as a front-end to configuring:

- SPV3's video/audio/input settings;
- environmental sound & hardware acceleration;
- OpenSauce post-processing options/effects.

## Development

This section outlines the SPV3 Settings system. SPV3.Settings leverages HCE.BalsamV & SPV3.Shaders for configuring
SPV3's profile settings & shader preferences, respectively.

![diagram](https://user-images.githubusercontent.com/10241434/49387977-05238300-f75e-11e8-8eba-b8381039861a.png)

This program is designed to deal with options within the scope of SPV3. It abstracts the generalised routines of the
various libraries away from the end-user, and exposes only the necessary options that are relevant to SPV3.

### Profile Settings

Configuration of SPV3 profile settings is done using HCE.BalsamV, which is included in this repository in the `lib`
directory. Rather than launching the BalsamV GUI, SPV3.Settings relies on the library directly for configuring the
SPV3 profile settings. This is due to the fact that BalsamV is separate from SPV3 in terms of design philosophy.

### Shader Preferences

Configuration of post-processing effects is done using SPV3.Shaders, which is also included in this repository.
SPV3.Settings launches the SPV3.Shaders GUI and instructs it to load the provided `initc.txt` file which SPV3.Settings
takes care of detecting. 