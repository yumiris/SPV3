# Render Stack Sorting

The table below outlines the post-processing effects' orders on the stack. The table covers only the shaders that are
configured by SPV3.Shaders.

| Order | Shader Effect  |
| ----- | -------------  |
| 1     | MXAO           |
| 2     | DOF            |
| 3     | Dynamic Flare  |
| 4     | Lens Dirt      |
| 6     | Eye Adaptation |
| 9     | Debanding      |

For information on the configurability for each shader, please refer to [Estimated Quality Levels](quality-levels.md).

For definitions on each shader, please refer to the [Shader Definitions](shader-definitions.md) documentation.