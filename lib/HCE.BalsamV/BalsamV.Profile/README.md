# blam.sav - profile

| Property                     | Type           | Offset | Length     | Constraints/Notes                       |
| ---------------------------- | -------------- | ------ | ---------- | --------------------------------------- |
| Profile Name                 | UTF-16 string  | `0x0002` | `22`     | Max. `0xB` chars                        |
| Player Colour                | Unsigned byte  | `0x011A` | `1`      | Range `0x00` - `0x12` (white is `0xFF`) |
