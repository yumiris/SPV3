# blam.sav - input

| Property                     | Type           | Offset | Length     | Constraints/Notes                       |
| ---------------------------- | -------------- | ------ | ---------- | --------------------------------------- |
| Mouse.Sensitivity.Horizontal | Unsigned byte  | `0x0954` | `1`      | Range `0x00` - `0x0A`                   |
| Mouse.Sensitivity.Vertical   | Unsigned byte  | `0x0955` | `1`      | Range `0x00` - `0x0A`                   |
| Mouse.InvertVerticalAxis     | Boolean        | `0x0955` | `1`      | Represented using `1` and `0`           |
