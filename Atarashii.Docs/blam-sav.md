# HCE Profile Settings

| Property                    | Type          | Offset | Length | Constraints        |
| --------------------------- | ------------- | ------ | ------ | ------------------ |
| Name                        | UTF-16 string | 0x002  | 22     | Max. 11 characters |
| Colour                      | Unsigned byte | 0x11A  | 1      | Range 0x00 - 0xFF  |
| Mouse.Sentivitiy.Horizontal | Unsigned byte | 0x954  | 1      | Range 0 - 10       |
| Mouse.Sentivitiy.Vertical   | Unsigned byte | 0x955  | 1      | Range 0 - 10       |
| Mouse.InvertVerticalAxis    | Boolean       | 0x955  | 1      | ~                  |
| Audio.Volume.Master         | Unsigned byte | 0xB78  | 1      | Range 0 - 10       |
| Audio.Volume.Effects        | Unsigned byte | 0xB79  | 1      | Range 0 - 10       |
| Audio.Volume.Music          | Unsigned byte | 0xB7A  | 1      | Range 0 - 10       |