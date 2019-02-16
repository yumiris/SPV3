# Domain

This document covers the domain logic & entities for SPV3.Bbkpify.

## Entities

This section specifies the main entities of the SPV3.Bbkpify domain.

### Bitmap

A bitmap file is the kind of SPV3 file which Bbkpify replaces with a
placeholder. A lot of SPV3 bitmaps are too large for Sapien to handle, thus
they're replaced with lightweight placeholders. This allows Sapien to load heavy
maps into memory without the risk of performance issues or crashes.

In the context of SPV3 development, there are three kinds of bitmaps: `nrml`,
`multi`, and `diff`. Bbkpify allows bitmaps of these sets to be backed up and
replaced with a placeholder.

Bbkpify also distinguishes normal bitmaps from placeholder bitmaps, by keeping a
record of all the bitmaps that have been replaced with placeholder. This is to
prevent circumstances where placeholders are unnecessarily replaced again.

### Placeholder

The placeholder file is a bitmap file with minimum data. Conventionally, it's a
1-pixel image that requires the lowest resources; however, any bitmap can
technically be used as a placeholder.

Bbkpify declares the maximum size of the placeholder as 8KiB (8192 bytes), to
ensure that bitmaps which are chosen as placeholders fulfil their purpose of
being lightweight on resources for Sapien.

### Configuration

The configuration file is used by Bbkpify to store a snapshot of the latest
session. On the filesystem, it is a deflated XML string that's stored in the
Application Data directory.

Values which are stored in the configuration file include...

| Value       | Description                                   |
| ----------- | --------------------------------------------- |
| Placeholder | Absolute path to the placeholder bitmap.      |
| Directory   | Absolute path to the bitmaps directory.       |
| Sapien      | Absolute path to the Sapien executable.       |
| Bitmap Type | Chosen bitmap type (`nrml`, `multi`, `diff`). |

### History

The history file stores a record of all the bitmaps that have been replaced with
placeholders. Like the configuration file, it is also a DEFLATE-compressed XML
string that's stored in the Application Data directory.

## Logic

This section specifies the main logic of the SPV3.Bbkpify domain. All logic
should be carried out asynchronously, to prevent blocking of the UI thread. When
any routine is carried out, Bbkpify defensively checks if the current conditions
are suitable. For all files (i.e. configuration, history, placeholder and
bitmaps), their existence and properties will be checked. If any anomaly occurs,
no routine will be executed and the end-user will be informed with an error.

### Configuration

At runtime, the program checks for the existence of the configuration file. If
it exists, it will inflate it and then deserialise the XML string to the
respective objects. The values will be bound to the UI controls, accordingly.

### Replacement

When the routine for replacing bitmaps with placeholders is invoked, the program
will conduct the same steps it does for the configuration file: finding,
inflating and deserialising to a list of bitmaps. Once done, it will loop
through each bitmap file in the specified target directory. If the bitmap
already exists in the history list, then it will be skipped from being replaced.
Any other bitmaps will be replaced with the respective placeholder.