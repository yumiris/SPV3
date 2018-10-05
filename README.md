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

Atarashii's goals are to cater to both end-users and developers. It accomplishes this goal by offering a set of modular tools that are sleek and scriptable for users and developers, respectively. It is the result of learning from the mistakes of the SPV3 loader and installer.

# About

Atarashii's design consists of one library that contains the logic for all of the project's features and abilities. At the moment, the project provides the following features:

- secure loading of the HCE executable, by verifying the provided executable and loading it if it passes the validation checks.

## CLIs

All of the library's major features have CLI front-ends to them. All of the CLIs are cross-platform, scriptable and informative programs for developers and calling processes to use.

Interaction is carried out using start-up arguments, with detailed instructions & logs being provided for each interaction. Appropriate exit codes and error messages are used for communication with calling processes and developers.

## GUIs

The GUIs serve as straightforward, simple and intuitive programs for users (and developers!) to use. There are two kinds of GUIs:

- feature GUIs, which serve as basic graphical front-ends to each major feature in the library. Due to their simplistic nature, they should be cross-platform compatible, at least on WINE. Both developers and users can rely on these GUIs for quick tasks.
- the principal GUI, which is akin to the SPV3 loader's GUI. The GUI serves to be a beautiful, minimalist and intuitive interface that allows easy yet powerful interaction with the library's features. It also contains its own features, such as MOTD and branding options.

# Repository

The main repository and issue tracker are hosted privately. A read-only mirror of the Git repository is available on GitHub, and a copy of the source code's latest revision is available on the SPV3 network.

Code is written mainly in C#, targetting .NET 4.5.2 for a balance between security and compatibility. The library's features are covered with automated tests to ensure that the core logic fulfils the expected requirements and proposed specifications.