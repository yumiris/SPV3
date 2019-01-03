# Initc File

The `initc.txt` file is used by SPV3 - through OpenSauce - to load persistently
stored global variables. These include the last played campaign mission &
difficulty, which is inferred by the loader from the
[savegame binary](savegame.md).

## Difficulty

Refer to the [difficulty](difficulty.md) document for the values, as both the
`initc` and [savegame binary](savegame.md) store the difficulties with the same
numeric values.

## Campaigns

The table below declares the global variable value (ID) in the `initc` for each
SPV3 map name.

| Map Name          | ID   |
| ----------------- | ---- |
| `spv3a10`         | `1`  |
| `spv3a30`         | `2`  |
| `spv3a50`         | `3`  |
| `spv3b30`         | `4`  |
| `spv3b40`         | `5`  |
| `spv3c10`         | `6`  |
| `spv3c20`         | `7`  |
| `spv3c40`         | `8`  |
| `spv3d20`         | `9`  |
| `spv3d30_evolved` | `10` |
| `spv3d40`         | `11` |