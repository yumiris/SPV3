# Implementation

- [Implementation](#implementation)
    - [Logic](#logic)
        - [Diagram](#diagram)
    - [Entities](#entities)
        - [Data Entities](#data-entities)
        - [Logic Entities](#logic-entities)

This document formally specifies how SPV3.Shaders and SPV3 will communicate together to accomplish the following
objective: **rely on the user's preferences for the post-processing decisions**.

The implementation is an entire chain of entities that conduct memory and filesystem I/O, with encoding & decoding
routines for the purpose of translating the end-user's choices into values that SPV3/HCE scripts can parse and utilise.

## Logic

Before reading this section, please familiarise yourself with the [Entities](#entities) section, as this document will
identify each entity throughout the routine.

1. SPV3.Settings encodes its object representation of the user's preferences into a number that can be assigned to a
   single variable, as specified in the [Global Variable](global-variable.md) documentation.
   
   This allows the value to be written to the `initc.txt` file, which SPV3.Settings handles when the post-processing
   preferences are saved.

2. When SPV3 is loaded, OpenSauce will load the variables specified in the `initc.txt` file and load them into memory.
   The [Global Variable](global-variable.md) will be among the other unrelated global variables that will be declared in
   memory.
   
   Its value will be accessible by the SPV3 scripts for decoding the user's preferences and set up the post-processing
   effects, accordingly.

3. The SPV3 script -- either in Lua or Lisp -- will decode the global variable back into the individual values that
   represent the user's preferences for each post-processing effect.
   
   This script will also assign the decoded values into dedicated variables that will be used in the SPV3 levels. With
   the variables being declared, the SPV3 code can handle setting the post-processing effects to match the preferences
   specified by the user in SPV3.Settings.

### Diagram

![diagram](https://user-images.githubusercontent.com/10241434/49340191-6fadc380-f677-11e8-94f7-8d841ea83773.png)

## Entities

The entities in this implementation are categorised into **data** and **logic**.

### Data Entities

Data entities focus on storing the user's configuration in a particular format. This document distinguishes three kinds:

| Entity          | Description                                                                                 |
| --------------- | ------------------------------------------------------------------------------------------- |
| `initc.txt`     | Persistent storage on the filesystem for the [Global Variable](global-variable.md)'s value. |
| Global Variable | Value in memory, representing the user preferences as specified [here](global-variable.md). |
| Level Variables | Values in memory that have been decoded from the [Global Variable](global-variable.md).     |                                                                                            |

### Logic Entities

Logic entities focus on encoding/decoding the user's preferences to eventually reach the objective specified in the
introduction of this document. They conduct I/O operations on the [Data Entities](#data-entities) for settings & getting
the value(s) used to represent the user's preferences.

| Entity          | Description                                                                                      |
| --------------- | ------------------------------------------------------------------------------------------------ |
| SPV3.Settings   | Encodes the user's choices from an object to a single value that is stored in the `initc.txt`.   |
| OpenSauce       | Loads the [Global Variable](global-variable.md)'s value from the `initc.txt` into memory.        |
| Lua/Lisp Script | This script decodes the [Global Variable](global-variable.md) and sets up the PPEs, accordingly. |