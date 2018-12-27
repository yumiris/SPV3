# Compilation Procedure

The SPV3 Compiler GUI's objective is to facilitate the compression of the SPV3 data into packages. These packages are
used by the SPV3 Installer to install the SPV3 files to the end-user's computer.

## Implementation

### Packages

To comply with the [Specification](specification.md) document, the compiler produces DEFLATE binary packages with the
following convention:

- for all of the root files in the source directory, a single package is created: `0x00.bin`;
- for each subdirectory in the source directory, a package is created; starting at `0x01.bin`.

![packages](https://user-images.githubusercontent.com/10241434/50490514-6fbb9c00-0a48-11e9-9396-b6b0144fd6bb.png)

### Compression

For the sake of simplicity, the compiler invokes the 7-Zip executable to compress the packages. The library verifies the
checksum of the 7-Zip executable prior to executing it, to avoid the execution of unsafe executables.

By default, the library expects the 64-bit Windows version of [7-Zip 18.05 (2018-04-30)](https://www.7-zip.org/). This
can be overridden by instantiating the `SPV3.Compiler.Compression` type with a new compression executable path & hash.

## Usage

![gui](https://user-images.githubusercontent.com/10241434/50489850-fff7e200-0a44-11e9-861d-da4bea45e3b7.png)

In the GUI, specify your source & target directory, then click the button and wait for the compilation to finish. During
this time, you might see brief command lines popping up. Rest assured that this is expected, and nothing tragic should
happen!

## Directories

The GUI expects you to specify two directories:

**Source**: In a nutshell, the directory you choose will end up being what the SPV3 Installer will end up installing to
the end-user's system.
  
This directory should contain the HCE & SPV3 data. In this context, data refers to the HCE executable, OS libraries,
SPV3 maps and equivalent files.
          
**Target**: The installation packages will be created in this directory. No special requirements here!

Ideally, it should be a directory that will be distributed as an ISO/ZIP. With this in mind, it should only contain
SPV3-related installation files (the installer & packages), and documents such as a changelog and readme.