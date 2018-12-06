# Startup Arguments

This document outlines the startup parameters that the HCE executable supports. This document classifies the parameters by two types:

- **toggle**: parameters whose mere presence enable a certain functionality (e.g. `-screenshot`, `-window`);
- **argument**: parameters that require arguments for modifying an existing functionality (e.g. `width x`).

## Overview

| Parameter        | Type     | Arguments                               | Description                                                                  |
| ---------------- | -------- | --------------------------------------- | ---------------------------------------------------------------------------- |
| `-nosound`       | toggle   | ~                                       | Disable all sound.                                                           |
| `-novideo`       | toggle   | ~                                       | Disable video playback.                                                      |
| `-nojoystick`    | toggle   | ~                                       | Disable joysticks and gamepads.                                              |
| `-nogamma`       | toggle   | ~                                       | Disables adjustment of gamma.                                                |
| `-useff`         | toggle   | ~                                       | Forces the game to run as a fixed function card.                             |
| `-use11`         | toggle   | ~                                       | Forces the game to run as a shader 1.1 card.                                 |
| `-use14`         | toggle   | ~                                       | Forces the game to run as a shader 1.4 card.                                 |
| `-use20`         | toggle   | ~                                       | Forces the game to run as a shader 2.0 card.                                 |
| `-safemode`      | toggle   | ~                                       | Disables as much as possible when running the game.                          |
| `-window`        | toggle   | ~                                       | Run the game in windowed mode.                                               |
| `-width x`       | argument | int width                               | Forces the game to run at a specified resolution.                            |
| `-vidmode w,h,r` | argument | int width, int height, int refresh rate | Forces the game to run at the width(w), height(h), and refresh(r) specified. |
| `-adapter x`     | argument | int monitor index                       | Forces the game to run fullscreen on a multimon (multiple monitors) adapter. |
| `-port x`        | argument | int server port                         | Server port address used when hosting multiplayer games.                     |
| `-cport x`       | argument | int client port                         | Client port address used when joining multiplayer games.                     |
| `-ip x.x.x.x`    | argument | string ip address                       | Server IP address used when you have multiple IP addresses.                  |
| `-screenshot`    | toggle   | ~                                       | Enables the Print screen key to generate screenshots.                        |
| `-console`       | toggle   | ~                                       | Enables the debugging console.                                               |
| `-devmode`       | toggle   | ~                                       | Enables the Halo Developer Mode.                                             |