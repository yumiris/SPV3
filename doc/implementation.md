# Implementation

The SPV3.Installer project is designed to be flexible in terms of what files are installed and where. The library has
no prior knowledge of which files are installed. The `Installer` class is the top-level type that encompasses the
[installation information and logic](#installation).

The reliance on auto-properties instead of constructors is done for the sake of convenient XML serialisation. It is
expected that the properties would be populated through the deserialisation of a persisted Installer state. Refer to
[Persistence](#persistence) for further details.

## Entities

This section covers the major entities: [Package](#package) and [File](#file). Smaller types - such as `Description` and
`Name`, are not covered here.

### Package

A package is a container of files. On the filesystem, it is a DEFLATE archive with the `.pkg` extension.
All packages contain SPV3.2 installation data, which SPV3.Installer takes care of installing to the directory specified
by the end-user.

Conventionally, each package represents a top-level directory, relative to the installation directory. To handle both
scenarios where a package may ...

a) install files directly within the installation directory (e.g. `spv3.exe`, `dinput.dll`)
b) install files within a subdirectory in the installation directory (e.g. `maps\loc.map`, `maps\sounds.map`)

... the Package object exposes a Directory property. It can be optionally assigned with the value that represents the
subdirectory for the package.

### File

A file, in this context, is a member of the [Package](#package) entity. On the filesystem, it can be imagined as an
entry in an archive file. The `File` type is used by the library to determine which files are contained within a package
for the purpose of conducting a pre-installation [Backup](#backup) routine.

## Logic

This section covers the installation, backup and persistence implementations.

- [**Installation**](#installation): the routine of extracting the SPV3.2 files from the packages to the filesystem.
- [**Backup**](./implementation.md): pre-installation backup of SPV3.2 files which may already exist on the filesystem.
- [**Persistence**](#persistence): storing the installation metadata, including what files to install & where.

### Installation

Installation is, fundamentally, the extraction of the distributed SPV3.2 packages to the filesystem. The installation
routine is comprised of the backup routine and the installation routine.

The installation concept has been sectioned into two documents:

- [**Installation Procedure**](./implementation.md): The backup & installation procedures the installer will carry out.
- [**Compilation Procedure**](./compilation.md): The procedure used to compile the packages and installation metadata.

### Persistence

Persistence of the installation metadata - such as the list of packages to install and their target destinations - are
stored in a binary file, named `installer.bin` on the filesystem. The Persistence static class exposes methods for
encoding & decoding an Installer object into the binary representation.

The binary is the result of:

1. Serialising an Installer instance into an XML equivalent.
2. Computing the UTF8 bytes representation of the XML string.
3. Compressing said UTF8 bytes using the DEFLATE algorithm.

The `installr.bin` does not (at the moment) have any error correction or header information, for the sake of keeping it
small and purposed for its sole task of persisting the Installer state. It is expected that either the container file
(ZIP or ISO ) for the transfer protocol (BitTorrent) will handle any potential corruption during transmission.