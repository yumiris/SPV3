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

DOMAIN DOCUMENTATION
====================

This section serves as documentation for the fundamental domain entities
& logic of SPV3. For documentation on the implementations & source code,
please review the readme.txt file in the src directory.

MISCELLANEOUS
-------------

The following table outlines the documentation that's not related to the
SPV3 domain or filesystem files.

  ---------------------------------------------------------------------
  Documentation                         Description
  ------------------------------------- -------------------------------
  developer.txt                         Documentation for developers
                                        who would use HXE in their
                                        non-SPV3.2 projects.

  V3 INFORMATION                        
  ---------------------------------------------------------------------

The following table outlines the documentation focusing on information
specific to the SPV3 programs:

  ---------------------------------------------------------------------
  Documentation                         Description
  ------------------------------------- -------------------------------
  kernel.txt                            Outlines the loading procedure,
                                        including asset verification,
                                        campaign resuming,
                                        configuration overrides and
                                        loading SPV3.

  haloce.txt                            Rudimentary information on the
                                        HCE executable.

  shaders.txt                           Specification on the
                                        user-configurable
                                        post-processing effects, and
                                        saving + loading settings to
                                        and from the initc.txt

  release.txt                           Documentation on the public
                                        release procedure of SPV3.2,
                                        including distribution &
                                        installation decisions.

  distributing.txt                      Documentation & instructions on
                                        compiling, distributing, and
                                        installing SPV3.2.

  exit.txt                              Documentation on the exit codes
                                        that HXE returns when its
                                        process ends.
  ---------------------------------------------------------------------

FILESYSTEM FILES
----------------

The following table outlines the documentation focusing on files on the
filesystem that SPV3.2 deals with:

  ---------------------------------------------------------------------
  Documentation                         Description
  ------------------------------------- -------------------------------
  initc.txt                             Documentation on the initc.txt
                                        file, which is used for
                                        declaring global variables that
                                        SPV3.2 must load.

  savegame.txt                          Documentation on the
                                        savegame.bin file, which is
                                        used for resuming the campaign.

  lastprof.txt                          Documentation on the
                                        lastprof.txt, which is used for
                                        inferring the last used profile
                                        name.

  opensauce.txt                         Documentation on OpenSauce,
                                        which SPV3 depends on for
                                        post-processing and other
                                        goodies.

  blamsav.txt                           Documentation on the blam.sav
                                        file, which is used for storing
                                        HCE profile information &
                                        configurations.
  ---------------------------------------------------------------------
