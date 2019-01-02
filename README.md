<html>
    <h1 align="center">
        SPV3.Loader
    </h1>
    <h3 align="center">
        Source for the SPV3 loader
    </h3>
</html>

# Introduction

This repository contains the source & documentation for SPV3.Loader.

The loader is a wrapper for the HCE executable. It extends the loading
procedure whilst accepting the same startup parameters as the HCE
executable.

The additional functionality, at the moment, includes:

- detection of the HCE executable's path;
- verification of the HCE executable's validity;

## CLI Usage

```powershell
# Implicit detection of HCE executable
.\SPV3.Loader.CLI.exe -window -screenshot

# Explicit HCE path & parameters declarations
.\SPV3.Loader.CLI.exe C:\haloce.exe -nosound -console
```

## Documentation

- [Startup Parameters](doc/parameters.md)
- [Executable Detection](doc/detection.md)