<html>
    <h1 align="center">
        SPV3.Installer
    </h1>
    <p align="center">
        Installation source code & specification for SPV3.2
    </p>
</html>

# Introduction

This repository contains the source code and specification for the SPV3.2 installer, which aims to serve as both a
flexible installer, and also a robust one.

Flexibility is provided through the ability to easily compile the SPV3.2 data into a redistributable installer.
Robustness is achieved through pre-installation backup routines and verification of the end-user's environment
fulfilling the installation requirements.

# Documentation

This section serves as an index for the major documentation:

| Documentation                                 | Description                                                      |
| --------------------------------------------- | ---------------------------------------------------------------- |
| [Implementation](doc/implementation.md)       | High-level overview of the library's logic and entities.         |
| [Installation Procedure](doc/installation.md) | Detailed overview of the backup & installation procedure.        |
| [Compilation Procedure](doc/compilation.md)   | Detailed overview of creating the installer packages & metadata. |
