# HCE Profile Settings

| Property                    | Type           | Offset | Length | Constraints                        |
| --------------------------- | -------------- | ------ | ------ | ---------------------------------- |
| Name                        | UTF-16 string  | 0x002  | 22     | Max. 0xB chars                     |
| Colour                      | Unsigned byte  | 0x11A  | 1      | Range 0x00 - 0x12                  |
| Mouse.Sentivitiy.Horizontal | Unsigned byte  | 0x954  | 1      | Range 0x00 - 0x0A                  |
| Mouse.Sentivitiy.Vertical   | Unsigned byte  | 0x955  | 1      | Range 0x00 - 0x0A                  |
| Mouse.InvertVerticalAxis    | Boolean        | 0x955  | 1      | 1/0                                |
| Audio.Volume.Master         | Unsigned byte  | 0xB78  | 1      | Range 0x00 - 0x0A                  |
| Audio.Volume.Effects        | Unsigned byte  | 0xB79  | 1      | Range 0x00 - 0x0A                  |
| Audio.Volume.Music          | Unsigned byte  | 0xB7A  | 1      | Range 0x00 - 0x0A                  |
| Audio.Quality               | Unsigned byte  | 0xB7D  | 1      | Range 0x00 - 0x02                  |
| Audio.Variety               | Unsigned byte  | 0xB7F  | 1      | Range 0x00 - 0x02                  |
| Video.Resolution.Width      | Unsigned short | 0xA68  | 2      | Library imposes range 0x1 - 0x7FFF |
| Video.Resolution.Height     | Unsigned short | 0xA6A  | 2      | Library imposes range 0x1 - 0x7FFF |
| Video.FrameRate             | Unsigned short | 0xA6F  | 1      | Range 0x00 - 0x02                  |
| Video.Effects.Specular      | Boolean        | 0xA70  | 1      | 1/0                                |
| Video.Effects.Shadows       | Boolean        | 0xA71  | 1      | 1/0                                |
| Video.Effects.Decals        | Boolean        | 0xA72  | 1      | 1/0                                |
| Video.Particles             | Unsigned byte  | 0xA73  | 1      | Range 0x00 - 0x02                  |
| Video.Quality               | Unsigned byte  | 0xA74  | 1      | Range 0x00 - 0x02                  |