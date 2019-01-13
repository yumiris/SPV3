# Implementation

- [Implementation](#implementation)
    - [Logic](#logic)
        - [Diagram](#diagram)
    - [Entities](#entities)
        - [Data Entities](#data-entities)
        - [Logic Entities](#logic-entities)

This document formally specifies how SPV3.Shaders and SPV3 will work together to fulfil the user's shader preferences.

## Logic

Before reading this section, please familiarise yourself with the [Entities](#entities) section, as this document will
identify each entity throughout the routine.

1. SPV3.Settings encodes the user's preferences into a single number, as specified in the
   [Global Variable](global-variable.md) documentation. This allows the value to be stored in the `initc.txt` file.

2. When SPV3 is loaded, OpenSauce will load the variables specified in the `initc.txt` file and load them into memory.
   The [Global Variable](global-variable.md) is one of the said variables.
   
   Its value will be accessible by the SPV3 scripts for decoding the user's preferences and set up the post-processing
   effects, accordingly.

3. The SPV3 script -- either in Lua or Lisp -- will decode the global variable back into the individual values that
   represent the user's preferences for each post-processing effect.

### Diagram

![diagram](https://user-images.githubusercontent.com/10241434/49340191-6fadc380-f677-11e8-94f7-8d841ea83773.png)

## Entities

The entities in this implementation are categorised into [Data Entities](#data-entities) and
[Logic Entities](#logic-entities).

### Data Entities

Data entities focus on storing the user's configuration in a particular format. This document distinguishes three kinds:

| Entity          | Description                                                                                 |
| --------------- | ------------------------------------------------------------------------------------------- |
| `initc.txt`     | Persistent storage on the filesystem for the [Global Variable](global-variable.md)'s value. |
| Global Variable | Value in memory, representing the user preferences as specified [here](global-variable.md). |
| Level Variables | Values in memory that have been decoded from the [Global Variable](global-variable.md).     |                                                                                            |

### Logic Entities

Logic entities marshal and parse the user's preferences. They rely on the values in the [Data Entities](#data-entities).

| Entity          | Description                                                                                      |
| --------------- | ------------------------------------------------------------------------------------------------ |
| SPV3.Settings   | Encodes the user's choices from an object to a single value that is stored in the `initc.txt`.   |
| OpenSauce       | Loads the [Global Variable](global-variable.md)'s value from the `initc.txt` into memory.        |
| Lua/Lisp Script | This script decodes the [Global Variable](global-variable.md) and sets up the PPEs, accordingly. |