# Estimated Quality Levels

The table below outlines the configuration type and resource requirement for each shader. The configuration type can be
either a toggle (e.g. off/on), or a level (e.g. off/low/high).

| Shader         | Configuration Type (Level/Toggle) | Available Levels | Resource Requirement |
| -------------- | --------------------------------- | ---------------- | -------------------- |
| MXAO           | Level                             | Off, Low, High   | High                 |
| DOF            | Level                             | Off, Low, High   | Ultra                |
| Dynamic Flare  | Toggle                            | Off, On          | Low                  |
| Lens Dirt      | Toggle                            | Off, On          | Ultra[1]             |
| Eye Adaptation | Toggle                            | Off, On          | Low                  |
| Debanding      | Level                             | Off, Low, High   | High                 |

The resource requirements are in the order `Low < Medium < High < Ultra`.
[1] - Currently ultra, will be made much faster, expected to be `Medium`.

For definitions on each shader, please refer to the [Shader Definitions](shader-definitions.md) documentation.