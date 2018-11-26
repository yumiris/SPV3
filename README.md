# Table of Contents

- [Table of Contents](#table-of-contents)
- [Disclaimer](#disclaimer)
- [Introduction](#introduction)
- [Procedure](#procedure)
    - [Distribution](#distribution)
    - [Installation](#installation)
    - [Statistics](#statistics)
- [Contribution](#contribution)

# Disclaimer

This is a proposal document, with nothing being carved in stone! This document keeps a summary of what we're currently
considering. Any of the proposed changes may be subject to change. 

# Introduction

This document covers the release procedure for SPV3.2. The purpose of this document is to plan out a release that caters
to both the community's and developers' needs.

To keep things simple, the release needs to fulfil the following
requirements:
                                               
- distribution and downloading should fast, easy and resilient;
- installation should be straightforward, robust and professional;
- statistics should be possible, without the need for personal information;

# Procedure

This section covers our planned approach to fulfilling the requirements specified above. This section will be updated
regularly based on community input. It will also refer to the old release procedure for clarification purposes.

## Distribution

The old approach was to use MediaFire for distributing the SPV3 installation package. This caused problems for a lot of
people, with bandwidth throttling and corrupted downloads being the main culprits. The primary reason for using MF was
to track the number of downloads. Because we had to resort to recommending the use of different browsers or download
managers, there's a high chance the statistics have been skewed quite significantly.

To mitigate this, SPV3.2 will be distributed using two methods:

- The primary method will still be a typical download, but rather than rely only on MediaFire, we'll also use MEGA
  Google Drive, Dropbox and other hosts for redundancy and mirroring.

- BitTorrent as a fallback. Only the torrent file will be released on MediaFire, and a magnet link will be provided as
  well. This effectively mitigates the problems with slow & corrupted downloads, due to the protocol's resilient and
  decentralised nature.
  
  With BitTorrent, the launcher can also be used for automatically initiating and downloading the installation files,
  without the need for a dedicated BitTorrent client and any manual setup.

## Installation

SPV3.2's installation data will be stored in an ISO file. The ISO file will contain the packages and the installer
itself. The advantage is that an ISO file not require to be extracted like a 7-Zip/ZIP/RAR file would; hence, the space
requirements will be much lower. To access the contents of the ISO file, one would simply double click on it. Of course,
extraction is also an option.

The installation procedure won't change much. A legal copy of HCE will still be required, and SPV3 will still be kept
separate from HCE. We will, however, warn the user when they try to install SPV3 to a restricted location such as the
Program Files folder.

## Statistics

We relied on MediaFire to determine the number of SPV3 downloads. As discussed in the Distribution section, this isn't
a viable option. To ensure more accurate statistics without compromising the user experience and privacy, the installer
will send an unique ID and installation status. The unique ID is a randomly generated string, to ensure that no personal
information is tracked or required for our download/install statistics.

# Contribution

Any input is highly appreciated! This release procedure strives to satisfy the community by taking its feedback in
consideration. Don't hesitate to propose changes or new ideas.