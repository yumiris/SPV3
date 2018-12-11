# Compilation Procedure

This document covers the procedure of compiling the SPV3 into a redistributable installer.

## GUI

The SPV3.Compiler.GUI serves as a graphical front-end to the `Installer` type, and allows the end-user/developer to
create and persist a metadata binary that is subsequently used by the SPV3 installer for installing SPV3.2 to the
filesystem.

![gui](https://user-images.githubusercontent.com/10241434/49721154-8e761080-fc9c-11e8-9247-c0e10a4f1b2a.png)

The main window is split into three columns. Each column focuses on each entity specified in the
[Implementation](implementation.md) documentation.

### File Column

The file column serves as a simple form for assigning a name & description to a package's file. Hierarchically, it is
the lowest-level column just like the entity it represents.

### Package Column

The package column allows a package's information and files to be assigned. It renders a list of files belonging to the
specific package, and allows the end-user/developer to add new files to the package.

Addition of new files can be done either manually or using batch mode. Batch mode allows multiple files on the
filesystem to be assigned at once to the respective package, through the use of a file picking dialogue.

Additionally, the package column also allows the assignment of package information, such as its name, description, and
an optional subdirectory for the package's files when they are installed.

### Installation Column

The installer column serves as an outline of the installer metadata. A list of packages belonging to the installer is
visible, with buttons that allow invocation of either saving the metadata to an `installer.bin` binary in the working
directory, or creating/editing/deleting packages belonging to the installer.