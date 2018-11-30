# Global Variable

This document serves as the specification for the global variable that SPV3.2 will use to deduce shader configurations.

The global variable value's is an integer that must represent the following user choices:

- the enabled & disabled post-processing effects;
- the quality levels for PPEs that require them (e.g. low/med/high for MXAO).

It is quite challenging to rely on a single integer value for representing an entire user configuration scheme; however,
in this circumstance it's quite doable.

# Method

1. Create a list of the shader effects with the order specified in the [Render Stack Sorting](stack-sort.md) document.
2. Replace each shader effect with its possible states as specified in the [Quality Levels](quality-levels.md) document.
   - for example, replace list element `MXAO` with elements `Off`, `Low`, `High`; SMAA with `Off`, `On`;
   - the states' orders should be from off/lowest to on/highest (e.g. `Off`, `Low`, `High`, NOT `High`, `Off`, `Low`).
3. Each subsequent element's integer representation should be the previous one's to the power of two.
   - e.g. `(MXAO OFF) = 1` -> `(MXAO LOW) = 2` -> `(MXAO HIGH) = 4` -> `(DOF HIGH) = 32` -> `(Dynamic Flare ON) = 128`.
   - the first element is represented by 1; 
4. The global variable's value represents the sum of the integer representation for each chosen shader state.

# Values

The table below outlines the integer representations for each shader state:

| Shader w/ State        | Integer Representation | Base-16 Equivalent |
| ---------------------- | ---------------------- | ------------------ |
| `MXAO - Off`           | `1`                    | `0x1`              |
| `MXAO - Low`           | `2`                    | `0x2`              |
| `MXAO - High`          | `4`                    | `0x4`              |
| `DOF - Off`            | `8`                    | `0x8`              |
| `DOF - Low`            | `16`                   | `0x10`             |
| `DOF - High`           | `32`                   | `0x20`             |
| `Dynamic Flare - Off`  | `64`                   | `0x40`             |
| `Dynamic Flare - On`   | `128`                  | `0x80`             |
| `Lens Dirt - Off`      | `256`                  | `0x100`            |
| `Lens Dirt - On`       | `512`                  | `0x200`            |
| `Eye Adaptation - Off` | `1024`                 | `0x400`            |
| `Eye Adaptation - On`  | `2048`                 | `0x800`            |
| `SMAA - Off`           | `4096`                 | `0x1000`           |
| `SMAA - On`            | `8192`                 | `0x2000`           |
| `Debanding - Off`      | `16384`                | `0x4000`           |
| `Debanding - Low`      | `32768`                | `0x8000`           |
| `Debanding - High`     | `65536`                | `0x10000`          |

What this allows us is to come up with unique values for any configuration.
For example, consider the following configuration:

| Respective Shader | Chosen State | Integer Representation |
| ----------------- | ------------ | ---------------------- |
| MXAO              | High         | `4`                    |
| DOF               | Low          | `16`                   |
| Dynamic Flare     | Off          | `64`                   |
| Lens Dirt         | On           | `512`                  |
| Eye Adaptation    | On           | `2048`                 |
| SMAA              | Off          | `4096`                 |
| Debanding         | Low          | `32768`                |

It can be represented in the global variable with the value of `39508`, which is the sum of the integers in the
`Integer Representation` column.