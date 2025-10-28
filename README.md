# Stationeers Community Patches

This mod fixes some issues in Stationeers.

Currently it fixes two issues:

- Reading `StackSize` (number of devices) from a network:
```asm
l r0 db:0 StackSize
```

- LogicDisplay text is shown correctly when mounted upside down

## Installation

This mode requires [BepInEx](https://github.com/BepInEx/BepInEx).
Download the latest release from the [releases page](https://github.com/aproposmath/stationeers-community-patches/releases) and unzip it into your `BepInEx/plugins` folder.
