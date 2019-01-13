<html>
    <h1 align="center">
        SPV3.Shaders
    </h1>
    <h3 align="center">
        Shaders settings specification & program for SPV3
    </h3>
    <p align="center">
        <img src="https://user-images.githubusercontent.com/10241434/49329147-ceab0400-f5b5-11e8-8fb1-62c6b24c672b.png">
    <p>
</html>

# Introduction

SPV3.Shaders allows the configuration of various post-processing effects in SVP3.2. This repository contains the source
code revisions and documentation on the program and the shaders it handles.

## Shaders

- **MXAO**, for ambient obscurance;
- **DOF**, for depth of field;
- **Dynamic** Flare, for lens flares;
- **Lens Dirt**, for dirt/scratch effects;
- **Vignette**, for darkening edge ports;
- **Eye Adaptation**, for adjusting scene exposure;
- **Debanding**, for fixing banding at low bit depths;

For documentation on the shader effects, please refer to the following documents:

- [Estimated Quality Levels](doc/quality-levels.md)
- [Global Variable Value](doc/global-variable.md)
- [Implementation](doc/implementation.md)
- [Render Stack Sorting](doc/stack-sort.md)
- [Shader Definitions](doc/shader-definitions.md)

## Attributions

Although this project does not directly interact with the respective shader effects, we believe it is fair to include
the licenses for the shader effects that are distributed with the official copy of SPV3.2.

The [Shader Licences](doc/shader-licenses.md) document handles the shader effects' attributions and licences.

Note that the licence used for SPV3.Shaders (or its main implementor - SPV3.Settings) does not extend to any shader
effect that is utilised by the official copy of SPV3.2. As such, the source code and/or binaries for the shader effects
are not included in the official repositories for SPV3.Shaders and SPV3.Settings.