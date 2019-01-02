# Executable Detection

This document outlines the routine which attempts to detect the HCE executable
on the filesystem. The attempts are carried out in the following order.
Subsequent methods are carried out if the HCE path cannot be inferred from the
previous methods.

- [Executable Detection](#executable-detection)
  - [Current Path](#current-path)
  - [Default Path](#default-path)
  - [Registry Key](#registry-key)

All methods rely on checking if the HCE executable exists from the inferred
paths, which officially named `haloce.exe`.

## Current Path

The first routine is by checking the current directory - that is, the directory
where SPV3.Loader is loaded from - for the HCE executable. This method covers
cases such as:

- installations of SPV3 that install SPV3.Loader to the same path as the rest of
  SPV3 (including the HCE core data);
- portable/non-standard installations of HCE.

Previously, this would've been the last-resort fall-back method, but it's now
prioritised because SPV3.2's installation won't be officially compatible with
the subsequent methods. 

## Default Path

The second attempt is by checking for well-known default locations which HCE
_might_ be installed to, including:

- **64-bit systems**: `C:\Program Files (x86)\Microsoft Games\Halo Custom
  Edition`
- **32-bit systems**: `C:\Program Files\Microsoft Games\Halo Custom Edition`

This attempt is a fall-back that often works if HCE was installed either by a
custom installer that respects the paths used by the official installer, or the
end-user manually created the directories.

## Registry Key

The last attempt is by retrieving the path from the registry. This strict method
returns an accurate path, assuming that the following two conditions are met:

- the HCE installation was carried out using its official installer (therefore,
  a legal installation);
- the installation directory's path remains the same since installation.

The value is retrieved from the `EXE Path` registry sub-key, located in
`SOFTWARE\Microsoft\Microsoft Games\Halo CE`.

Both the 64-bit & 32-it equivalents of the `HKEY_LOCAL_MACHINE` registry key are
checked by the SPV3.Loader, to cover both system types.