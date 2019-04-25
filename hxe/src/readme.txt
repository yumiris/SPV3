<!--
 Copyright (c) 2019 Emilian Roman
 
 This software is provided 'as-is', without any express or implied
 warranty. In no event will the authors be held liable for any damages
 arising from the use of this software.
 
 Permission is granted to anyone to use this software for any purpose,
 including commercial applications, and to alter it and redistribute it
 freely, subject to the following restrictions:
 
 1. The origin of this software must not be misrepresented; you must not
    claim that you wrote the original software. If you use this software
    in a product, an acknowledgment in the product documentation would be
    appreciated but is not required.
 2. Altered source versions must be plainly marked as such, and must not be
    misrepresented as being the original software.
 3. This notice may not be removed or altered from any source distribution.
-->

HXE SOURCE CODE
===============

This section contains the implementation source code for HXE.

This document serves as a high-level documentation for the source.
Specific documentation can be found within the source code itself.

LOADER INFORMATION
------------------

The loading procedure, in a nutshell, is as follows:

1.  verify the main SPV3 assets on the filesystem, by comparing their
    file lengths against the lengths specified in the manifest; and

2.  resume of the SPV3 campaign, through heuristic detection of the
    player's profile and inference of the mission & difficulty; and

3.  find the post-processing settings on the filesystem, infer the
    integer that represents them, and save it to the initc file; and

4.  invoke of the HCE executable within the working directory, with
    arguments being passed onto the invoked executable process.

All of the steps above are conducted by the Kernel class, which serves
as the highest-level abstraction of the aforementioned procedure.

The Kernel also filters out which files to be verified. It strives to
focus only on the assets that are not prone to changing, such as map and
game data. Implicitly, configurations and other files which are prone to
changing will be skipped.

Additionally, the Kernel will try to analyse if the directory contains
installation data (manifest & packages), but not game data. If it
heuristically infers that the directory contains only installation data,
then it will proceed to first install SPV3, and then load SPV3.

Configuration of the kernel can be done by editing the relevant binary
file. Please refer to doc/loader.txt for further information.

COMPILER INFORMATION
--------------------

The compiler handles the preparation of SPV3 data for distribution and
installation. It accomplishes this by DEFLATE-compressing all of the
subdirectories in the source directory to individual packages in the
target directory.

It also adds an entry for the subdirectory in the manifest: this entry
declares the relative path of the subdirectory to the source directory,
the name of the package, and the list of files the subdirectory
contains.

The compilation procedure, in a nutshell, is as follows:

1.  create a list containing:

    -   the specified source directory, and
    -   any subdirectories in the source directory.

2.  for each subdirectory, create a DEFLATE package in the specified
    target directory, and an entry in the manifest with:

    -   the package's name on the filesystem (hex.bin convention) and;
    -   the path the subdirectory resides in, relative to the source.

3.  for each file within the respective subdirectory, add an entry to
    the package on the filesystem, add an entry for it in the manifest
    containing:

    -   the filename (no paths) on the filesystem; and
    -   the file size (byte length) on the filesystem.

4.  save of the manifest data in DEFLATE format to the target directory,
    for the aforementioned distribution and installation.

For further information on the anatomy of a manifest file, please refer
to the manifest.txt documentation in the doc directory.

INSTALLER INFORMATION
---------------------

The installer handles the extraction of data from the DEFLATE packages -
which have been created by the compiler - to the filesystem. It relies
on the generated manifest for inferring which packages are installable,
and loops through each package to install it to the filesystem.

Additionally, it also verifies if files within packages already exist at
the target destination, and if it finds the file, then it will delete it
before installing the package. This is done to practically handle
reinstall scenarios.

To permit other programs to determine the path which SPV3 is installed
to, the installer also creates a text file which contains the absolute
path of the directory which SPV3 is installed to.

The installation procedure, in a nutshell, is as follows:

1.  load the manifest file from the provided source directory; and

2.  start iterating through each package in the loaded manifest; and

3.  for each file within the iteration's package, check if the file
    exists at the target destination, and delete it upon confirming its
    existence; and

4.  infer the destination path for the extracted files, then extract the
    package's data to it; and

5.  check if a manifest already exists at the target destination, and
    delete it upon confirming its existence; and

6.  copy the manifest file from the source directory the target
    directory, to allow the loader to verify the assets at runtime; and

7.  create a text file containing the target directory's absolute path,
    to permit other applications to infer SPV3's installation path.

For further information on the aforementioned installation text file,
please refer to the installer.txt documentation in the doc directory.

FILESYSTEM OBJECTS
------------------

A significant portion of source code focuses on representing files as
objects. This section seeks to outline each object and the respective
file it represents.

All of the types listed in the following table are inheritors of the
File type:

  ---------------------------------------------------------------------
  Object           Description
  ---------------- ----------------------------------------------------
  Executable       Represents the haloce.exe executable. Permits
                   invocation of the executable on the filesystem.
                   Common HCE executable arguments are exposed as
                   properties.

  Initiation       Represents the OpenSauce initc.txt file. For more
                   information on the file itself, please refer to
                   doc/initc.txt. This object permits the saving of its
                   properties to initc.txt values, including the
                   mission, difficulty, post-processing settings, and
                   some miscellaneous toggles.

  LastProfile      Represents the lastprof.txt file, which is used in
                   the kernel's auto-detection mechanism. This object
                   permits the retrieval of the last played profile
                   name.

  Progress         Represents the savegame.bin file, which contains the
                   player's checkpoint data. This object infers and
                   exposes the mission & difficulty, which in turn can
                   be saved using the Initiation object for campaign
                   resuming.

  OpenSauce        Represents the OS\_Settings.User.xml, which contains
                   the OpenSauce user configuration data. The loader
                   exposes all of the available options as object
                   properties, for programmable editing of the
                   OpenSauce configuration.

  Profile          Represents the blam.sav binary, which contains HCE
                   profile information and configuration data. The
                   object permits loading, editing, and saving this
                   data. Additionally, it also automatically forges the
                   hash of the blam.sav upon saving, to ensure that HCE
                   accepts the edited binary.

  Configuration    Represents the loader.bin binary, which SPV3 uses to
                   store the user preferences for its post-processing
                   features, and for kernel options.
  ---------------------------------------------------------------------

Documentation on any of the aforementioned files can be found in the doc
directory within this repository.
