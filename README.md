<html>
    <p align="center">
        <img src="./Atarashii.png"/>
    </p>
    <h1 align="center">
        HCE Atarashii
    </h1>
    <p align="center">
        A reliable and versatile HCE/SPV3 loader.
    </p>
</html>

# Introduction

Atarashii's goal is to cater to both end-users and developers. It accomplishes a beautiful and versatile command line
front-end (CLI). It is the result of learning from the mistakes of the SPV3 loader and installer.

# About

Atarashii's design consists of one library that contains the logic for all of the project's features and abilities.
At the moment, the project provides the following features:

- secure loading of the HCE executable, by verifying the executable and loading it if it passes the validation checks;
- detection of a legally installed HCE executable on the filesystem using various fallback detection mechanisms;
- determining the currently used HCE profile, thereby allowing automatic integration with the HCE profile instead;
- installation and configuration of the OpenSauce mod, with a balance between safety and flexibility.

## CLI

All of the library's major components have a Command Line Interface (CLI) front-end. It is cross-platform,
script-friendly and versatile program for developers and calling processes to use.

Interaction is carried out using start-up arguments, with detailed instructions & logs being written to the CLI.
Appropriate exit codes and error messages are used for communication with calling processes and developers.

# Repository

The main repository and issue tracker are hosted privately. A read-only mirror of the Git repository is available on
GitHub, and copies of the latest binaries are available on the SPV3 network.

Code is written mainly in C#, targeting .NET 4.5.2 for a balance between security and compatibility. The library's
features are covered with automated tests to ensure that the core logic fulfils the expected requirements and proposed
specifications.