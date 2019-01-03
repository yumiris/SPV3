# Savegame

The `savegame.bin` contains - amongst other data - the last campaign mission and
difficulty the player has played on. This binary is parsed by the loader for the
campaign resume capability, by "converting" the data from the `savegame` binary
to the `initc` text file.

## Difficulty

The difficulty value is stored at offset `0x1E2`.

Refer to the [difficulty](difficulty.md) document for the values, as both the
[initc document](initc.md) and savegame binary store the difficulties with the
same numeric values.

## Missions

The mission value is stored at offset `0x1E8`.

The mission values are effectively the map names. Refer to the
(mission document)[mission.md] for the file names.