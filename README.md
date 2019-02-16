# SPV3.Bbkpify

Safely replace Sapien texture bitmaps with placeholders.

![screenshot](https://user-images.githubusercontent.com/10241434/48938717-ff06f880-ef4c-11e8-927c-ba9dce6b69fb.png)

## Introduction

Sapien occasionally runs out of memory when large texture bitmaps are used. To avoid that, one can use placeholder
bitmaps which are insignificant in size. This tool replaces bitmaps in a provided directory with a given placeholder.
The original bitmaps are backed up, thus making the process completely safe and reversible.

## Documentation

- [Domain](doc/domain.md)