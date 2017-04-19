![Main UI Screenshot](http://i64.tinypic.com/kcnxgg.png)

[![Build status](https://ci.appveyor.com/api/projects/status/f57ignemhal4foga?svg=true)](https://ci.appveyor.com/project/yumiris/spv3-promise)
[![Code Climate](https://codeclimate.com/github/yumiris/spv3-promise/badges/gpa.svg)](https://codeclimate.com/github/yumiris/spv3-promise)
[![Test Coverage](https://codeclimate.com/github/yumiris/spv3-promise/badges/coverage.svg)](https://codeclimate.com/github/yumiris/spv3-promise/coverage)
[![Issue Count](https://codeclimate.com/github/codeclimate/codeclimate/badges/issue_count.svg)](https://codeclimate.com/github/yumiris/spv3-promise)

A stunning and functional loader for the acclaimed [Halo CE: SV3](https://reddit.com/r/halospv3) campaign mod!

![Main UI Screenshot](http://i67.tinypic.com/whmb1s.png)

In a nutshell, it saves you the time of manually adding launch parameters to the game, fixes some issues with the SPV3 mod, and makes up for a lack of in-game settings UI when using the portable version of [OpenSauce](https://bitbucket.org/KornnerStudios/opensauce-release), by providing its own configuration settings.

The project is still undergoing heavy development. Heavy refactoring, restructuring and even revamping is bound to occur. Currently in public beta at version 0.1.2.

# So, what's it got?
Quite a handful of features, some including:
- Configure various video settings for Halo CE, including resolution, adapter, refresh rate, and window/safe/low-end modes.
- Tweak OpenSauce settings including Field of View, Shader Extensions and Post Processing.
- Automatically fix SPV3-specific issues such as the mod not launching on freshly installed systems.
- Full support for generating compatible OpenSauce XML configuartion files.

# Setting it up
1. Download the latest release. .NET Framework 4.5 will be required for the loader!
2. Place the files (promise.dll and loader.exe) in your Halo CE (or SPV3) installation directory.
3. Use it as you please, preferably without breaking your system. For starters, you may want to go into Configuration before launching the game.

# Development
Visual Studio 2015 has been used for developing the project. In a nutshell, clone the repository, and open the solution. Make sure NuGet works, so it can restore the missing packages which have been used.

Note, this project will not run in Mono due to WPF being used for the UI foundation.

Good luck!

### Contribution
Please use the Issues feature of GitHub to contribute in the following ways:
- Report bugs, issues, errors, crashes and equivalent catastrophies. No generic errors, please!
- Suggest features and improvements, both for the UI and the library. I will perhaps unify the configuration windows into the main UI in the future. No transition requests for now, please!
- ~~Call me out on the disastrous code I've managed to produce at 2AM in some part of the library.~~
