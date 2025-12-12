# EntityFlags
EntityFlags is a flag system for entities in Broke Protocol to allow advanced configuration of entities in the game.

## Available flags
| Flag | Param | Type | Effect | Note |
| ---- | ----- | ---- | ------ | ---- |
| nosteal | | ShEntity | Viewing the inventory is disabled | |
| anim | animName | ShEntity | Force to play the animation animName | |
| godmode | | ShDamageable | Damages are disabled | |
| nomount | | ShMountable | Mounting this entity is disabled | |
| norotate | | ShPlayer | Auto-rotation is disabled | |
| noai | | ShPlayer | AI is disabled | |
| norestrain | | ShPlayer | Cuffing the NPC is disabled | |
| mount | seatIndex (optional) | ShPlayer | Mounts the closest available mountable at seatIndex | Without seatIndex the NPC will mount any seat of the closest available mountable |
| nocollect | | ShItem | Collecting this item is disabled | |

## How to use
To use this plugin, you have to use the `data` field of entities. The field must be filled in the following format: `flags[flag1,flag2:param,flag3]`.

The following flags on an npc: `flags[godmode,noai,nosteal,norestrain,mount:0]` will do:
* Godmode
* AI disabled
* Impossible to view the inventory
* Impossible to restrain with handcuffs
* Will mount the first seat (0) of the closest available mountable

## Build the plugin
1. Set the environment variable `BPDIR` to your Broke Protocol directory (or change every `$(BPDIR)` instances to your BP directory path in the csproj file).
2. Make sure to have `0Harmony.dll` in your BP plugins folder
3. Build the project using any IDE.
4. The plugin should be automatically copied to your BP Plugins directory.
