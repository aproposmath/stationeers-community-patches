# Stationeers Community Patches

This mod fixes some issues related to IC10 in Stationeers.

Currently it fixes two issues:

- Reading `StackSize` (number of devices) from a network:
```asm
l r0 db:0 StackSize
```

- Indirect adressing of devices
```asm
# sets Setting of d3 to 99
move r0 3
s dr0 Setting 99
```

## Installation

This mode requires [BepInEx](https://github.com/BepInEx/BepInEx).
Download the latest release from the [releases page](https://github.com/aproposmath/stationeers-community-patches/releases) and unzip it into your `BepInEx/plugins` folder.
