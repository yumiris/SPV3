# Render Stack Sorting

The table below outlines the external shader effects' orders on the stack and whether they are configurable by the user
or not. As such, any shader effects which are NOT (✗) configurable should NOT be handled by SPV3.Shaders at all!

| Order | Shader Effect  | Configurable |
| ----- | -------------  | ------------ |
| 1     | MXAO           | ✓            |
| 2     | DOF            | ✓            |
| 3     | Dynamic Flare  | ✓            |
| 4     | Lensdirt       | ✓            |
| 5     | Vignette       | ✗            |
| 6     | Eye Adaptation | ✓            |
| 7     | ACES HDR       | ✗            |
| 8     | LUT            | ✗            |
| 9     | SMAA           | ✓            |
| 10    | Debanding      | ✓            |