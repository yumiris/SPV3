# Specification

The SPV3.Installer project is designed to be flexible in terms of what files are installed and where. The library has
no prior knowledge of which files are installed. The `Installer` class is the top-level type that encompasses the
[installation information and logic](#installation).

The reliance on auto-properties instead of constructors is done for the sake of convenient XML serialisation. It is
expected that the properties would be populated through the deserialisation of a persisted Installer state. Refer to
[Persistence](#persistence) for further details.

## Entities

This section covers the major entities: [Metadata](#metadata), [Package](#package) and [File](#file).
Smaller types - such as `Description` and `Name`, are not covered here.

The following diagram visualises the hierarchy of the entities in SPV3.Installer:

![hierarchy](https://user-images.githubusercontent.com/10241434/49706761-cc564300-fc62-11e8-8ab5-9585ca932c79.png)

### Metadata

A metadata is the top-level specification of an SPV3 installer. It outlines which packages should be installed and what
files belong to each package. The metadata also specifies the backup directory, which is used by the installer for
backing up files on the system that may belong to any of the installation packages.

Technically, the metadata is an instance of the `Installer` type, which may then be serialised to a persistent format.
Please refer to the [Persistence](#persistence) section for further details on how persistence is conducted.

The SPV3.Compiler.GUI serves as a graphical front-end to the `Installer` type, and allows the end-user/developer to
create and persist a metadata binary that is subsequently used by the SPV3 installer for installing SPV3.2 to the
filesystem.

### Package

A package is a container of files. On the filesystem, it is a DEFLATE archive with the `.bin` extension.
All packages contain SPV3.2 installation data, which SPV3.Installer takes care of installing to the directory specified
by the end-user.

Conventionally, each package represents a top-level directory, relative to the installation directory. To handle both
scenarios where a package may ...

a) install files directly within the installation directory (e.g. `spv3.exe`, `dinput.dll`); or
b) install files within a subdirectory in the installation directory (e.g. `maps\loc.map`, `maps\sounds.map`)

... the Package object exposes a Directory property. It can be optionally assigned with the value that represents the
subdirectory for the package.

### File

A file, in this context, is a member of the [Package](#package) entity. On the filesystem, it can be imagined as an
entry in an archive file. The `File` type is used by the library to determine which files are contained within a package
for the purpose of conducting a pre-installation backup routine.

## Logic

This section covers the installation, backup and persistence implementations.

- [**Installation**](#installation): the routine of extracting the SPV3.2 files from the packages to the filesystem.
- [**Backup**](./specification.md): pre-installation backup of SPV3.2 files which may already exist on the filesystem.
- [**Persistence**](#persistence): storing the installation metadata, including what files to install & where.

### Installation

Installation is, fundamentally, the extraction of the distributed SPV3.2 packages to the filesystem. The installation
routine is comprised of the backup routine and the installation routine.

The installation concept has been sectioned into two documents:

- [**Installation Procedure**](./specification.md): The backup & installation procedures the installer will carry out.
- [**Compilation Procedure**](./compilation.md): The procedure used to compile the packages and installation metadata.

### Persistence

Persistence of the installation metadata - such as the list of packages to install and their target destinations - are
stored in a binary file, named `metadata.bin` on the filesystem. The Persistence static class exposes methods for
encoding & decoding an Installer object into the binary representation.

The binary is the result of:

1. Serialising an Installer instance into an XML equivalent.
2. Computing the UTF8 bytes representation of the XML string.
3. Compressing said UTF8 bytes using the DEFLATE algorithm.

The `metadata.bin` does not (at the moment) have any error correction or header information, for the sake of keeping it
small and purposed for its sole task of persisting the Installer state. It is expected that either the container file
(ZIP or ISO ) for the transfer protocol (BitTorrent) will handle any potential corruption during transmission.