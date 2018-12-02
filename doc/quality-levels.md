# Estimated Quality Levels

The table below outlines the shader effects that are configurable by the user with the following characteristics:

- their configuration being a toggle (e.g. on/off), or a level (e.g. off, medium, high);
- their resource requirements at the time of this document's latest revision.

Specification of which shader effects are configurable by the end-user can be found in the
[Render Stack Sorting](stack-sort.md) documentation.

| Shader         | Configuration Type (Level/Toggle) | Available Levels | Resource Requirement |
| -------------- | --------------------------------- | ---------------- | -------------------- |
| MXAO           | Level                             | Off, Low, High   | High                 |
| DOF            | Level                             | Off, Low, High   | Ultra                |
| Dynamic Flare  | Toggle                            | Off, On          | Low                  |
| Lens Dirt      | Toggle                            | Off, On          | Ultra[1]             |
| Eye Adaptation | Toggle                            | Off, On          | Low                  |
| SMAA           | Toggle                            | Off, On          | Medium               |
| Debanding      | Level                             | Off, Low, High   | High                 |

The resource requirements are in the order `Low < Medium < High < Ultra`.
[1] - Currently ultra, will be made much faster, expected to be `Medium`.

For definitions on each shader, please refer to the [Shader Definitions](shader-definitions.md) documentation.