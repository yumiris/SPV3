# Compilation Procedure

This document covers the procedure of compiling the SPV3 into a redistributable installer.

## GUI

The SPV3.Compiler.GUI serves as a graphical front-end to the `Installer` type, and allows the end-user/developer to
create and persist a metadata binary that is subsequently used by the SPV3 installer for installing SPV3.2 to the
filesystem.

The main window is split into three columns. Each column focuses on each entity specified in the
[Implementation](implementation.md) documentation.

### Usage

Use this
[GIF](https://user-images.githubusercontent.com/10241434/49774713-9aabad80-fd30-11e8-9ebc-a9d1d26aa18e.gif)
as a quick tutorial! Otherwise, please read the following steps carefully:

1. Add files to the package. This can be done by either:
   - manually filling out the form on the left column and clicking `Save File`,
   - clicking on `Batch Add Files` and selecting the files in the pop-up dialogue,
   - dropping the files from another window into the list of files in the middle column.

2. Specify the package information in the middle column's form:
   - **Package Name**: Name of the package that the installer will extract the files from.
   - **Description**: Description for the package which will be used in the installer's GUI.
   - **Subdirectory**: Optional directory which the installer will extract the package's files to.
     - e.g. `maps` for the maps data, `content`, `shaders`, etc.
     - leave blank for packages dealing with root files! (e.g. `haloce.exe`, `dinput8.dll`, `strings.dll`)

3. Click on `Save Package`, and repeat the steps above for each subsequent package. Conventionally, each package should
   focus on one folder. For example the `maps` folder contents would be stored in something like `maps.pkg`.

4. When all packages are created, click on `Create Installer Metadata` button on the right column, then choose the place
   to save the `metadata.bin` file to. You should save it to the directory where the SPV3 Installer GUI and the packages
   are located. The installer uses the saved file to determine which packages to install and where.

### Columns

#### File Column

The file column serves as a simple form for assigning a name & description to a package's file. Hierarchically, it is
the lowest-level column just like the entity it represents.

#### Package Column

The package column allows a package's information and files to be assigned. It renders a list of files belonging to the
specific package, and allows the end-user/developer to add new files to the package.

Addition of new files can be done either manually or using batch mode. Batch mode allows multiple files on the
filesystem to be assigned at once to the respective package, through the use of a file picking dialogue.

Additionally, the package column also allows the assignment of package information, such as its name, description, and
an optional subdirectory for the package's files when they are installed.

#### Installation Column

The installer column serves as an outline of the installer metadata. A list of packages belonging to the installer is
visible, with buttons that allow invocation of either saving the metadata to an `installer.bin` binary in the working
directory, or creating/editing/deleting packages belonging to the installer.