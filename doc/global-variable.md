# Global Variable Value

- [Global Variable Value](#global-variable-value)
    - [Introduction](#introduction)
    - [Glossary](#glossary)
    - [Shader States Values](#shader-states-values)
    - [Global Variable Specification](#global-variable-specification)
        - [Encoding Method](#encoding-method)
        - [Decoding Method](#decoding-method)

## Introduction

This document serves as the specification for the global variable that SPV3.2 will use to deduce shader configurations.
Additionally, it also outlines the official values for the various shader configuration states, which means the option
an end-user has chosen for a particular post-processing effect.

## Glossary

- **Global Variable**: An int variable defined in `initc.txt` and loaded by OpenSauce for HCE/SPV3 to access its value. 
  In this context, the global variable's value stores the user's configuration for each post-processing effect.
- **Shader State**: The respective option chosen for a particular shader. For example, MXAO on low settings is a shader
  state. In this specification, each state is represented by a number that is a power of two to combine shader states.

---

## Shader States Values

The table below outlines the integer representations for each shader state:

| Sequence | Shader State           | Integer Representation | Base-16 Equivalent |
| -------- | ---------------------- | ---------------------- | ------------------ |
| 1        | `MXAO - Off`           | `1`                    | `0x1`              |
| 2        | `MXAO - Low`           | `2`                    | `0x2`              |
| 3        | `MXAO - High`          | `4`                    | `0x4`              |
| 4        | `DOF - Off`            | `8`                    | `0x8`              |
| 5        | `DOF - Low`            | `16`                   | `0x10`             |
| 6        | `DOF - High`           | `32`                   | `0x20`             |
| 7        | `Dynamic Flare - Off`  | `64`                   | `0x40`             |
| 8        | `Dynamic Flare - On`   | `128`                  | `0x80`             |
| 9        | `Lens Dirt - Off`      | `256`                  | `0x100`            |
| 10       | `Lens Dirt - On`       | `512`                  | `0x200`            |
| 11       | `Eye Adaptation - Off` | `1024`                 | `0x400`            |
| 12       | `Eye Adaptation - On`  | `2048`                 | `0x800`            |
| 13       | `SMAA - Off`           | `4096`                 | `0x1000`           |
| 14       | `SMAA - On`            | `8192`                 | `0x2000`           |
| 15       | `Debanding - Off`      | `16384`                | `0x4000`           |
| 16       | `Debanding - Low`      | `32768`                | `0x8000`           |
| 17       | `Debanding - High`     | `65536`                | `0x10000`          |

---

## Global Variable Specification

The global variable value is an integer that must represent the following user choices:

- the enabled & disabled post-processing effects;
- the quality levels for PPEs that require them (e.g. off/low/high for MXAO).

In essence, it serves as an encoded configuration. The sections below outline the methods used for encoding and decoding
an end-user's post-processing configuration.

### Encoding Method

> **Method**: Sum up the numbers that correspond to the chosen shader states, which are numbers to the power of two, and
> store the total in the global variable.

1. Create a list of the shader effects with the order specified in the [Render Stack Sorting](stack-sort.md) document.
2. Replace each shader effect with its possible states as specified in the [Quality Levels](quality-levels.md) document.
   - for example, replace list element `MXAO` with elements `Off`, `Low`, `High`; SMAA with `Off`, `On`;
   - the states' orders should be from off/lowest to on/highest (e.g. `Off`, `Low`, `High`, NOT `High`, `Off`, `Low`).
3. Each subsequent element's integer representation should be the previous one's to the power of two.
   - e.g. `(MXAO OFF) = 1` -> `(MXAO LOW) = 2` -> `(MXAO HIGH) = 4` -> `(DOF HIGH) = 32` -> `(Dynamic Flare ON) = 128`.
   - the first element is represented by 1; 
4. The global variable's value represents the sum of the integer representation for each chosen shader state.

The following ASCII should hopefully make the method specified above a tad clearer:

```
      MXAO        DOF        Dynamic Flare
       |           |               |
       |           |               +--------+
       |           |                        |
       |           +--------+               |
       |                    |               |
 +-----+-----+        +-----+----+        +-+-+
 |     |     |        |     |    |        |   |
Off, Low, High        Off, Low, High     Off, On
 |     |     |        |     |    |        |   |
 1     2     4        8    16   32       64  128
```

As a practical example, consider the following configuration:

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

---

### Decoding Method

> **Method**: Break down the global variable value into a sum that corresponds of numbers which are powers of two, then
> determine what shader state each number in the sum corresponds to.

To get the variables back, we would rely on the value and an array/list to store the numbers we will get back.

First, let's prepare by:

- declaring `x` as the value stored in the global variable;
- defining the variable `i` to a value of 1.

Now, while `i` is lower or equal to `x`, we:
  1. Compute the logical bitwise AND of operands `x` & `i`, and assert that the result equates to TRUE in Boolean form.
  2. If the value is TRUE, the current value of `i` is considered as one of the numbers in the sum that totals to `x`.
  3. Regardless of the result, `i` should be left-shifted (or multiplied by 2) to prepare it for the next iteration.

The snippet below shows how this is accomplished in C#. Note that the example writes to the standard output, rather than
adding to an array or list.

```csharp
int x = 39508; // value we encoded earlier on
var i = 1;

while (i <= x)
{
    if (Convert.ToBoolean(i & x))
        Console.WriteLine($"{i}"); // final output: 4 16 64 512 2048 4096 32768

    i <<= 1;
}
```