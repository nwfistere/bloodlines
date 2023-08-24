# Examples

In this directory will be example character packs

## Gus.zip
This is a basic example of a character pack. A json file with image(s).
The json data is a copy of Antonio's settings with the starting weapon changed to an AXE. Check the Vampire Survivors wiki for the names to use for starting weapon (the ID tag). 

## Example json:
With version 0.1, the character json is a copy of what vampire survivors use as serialized data of their characters. This character "Gus" is a copy of Antonio's data.

The format of this json body will be changing in future releases, but I'm hoping to keep it somewhat backwards compatible after the first release.

## Changes from v0.1 to v0.2
Character json file format v0.2 is out!
	- version "0.1" -> "0.2"
	- Renamed character -> characters
	- characters.skins.frames - Names of each individual walking frame
	- characters.onEveryLevelUp - A statModifier object that effects the character every level.
	- characters.statModifiers - Moved initial stats from the base character object to this array. (Should have level: 1)
	- Multiple characters in one pack is probably supported but untested :)

## v0.2:

<details>

<summary>Copyable json v0.2</summary>

```json
{
	"version": "0.2",
	"characters": [
		{
			"level": 1,
			"startingWeapon": "JUBILEE",
			"cooldown": 1,
			"prefix": "Lord",
			"charName": "Gus",
			"surname": "the cat",
			"textureName": "characters",
			"spriteName": "gus_01.png",
			"portraitName": "gus_portrait.png",
			"skins": [
				{
					"id": 0,
					"name": "Default",
					"textureName": "characters",
					"spriteName": "gus_01.png",
					"walkingFrames": 4,
					"unlocked": true
				},
				{
					"id": 1,
					"name": "Legacy",
					"textureName": "characters",
					"spriteName": "gus_02.png",
					"walkingFrames": 4,
					"unlocked": true
				}
			],
			"currentSkinIndex": 0,
			"walkingFrames": 4,
			"description": "Gus the cat loves particles.",
			"isBought": true,
			"price": 0,
			"completedStages": [],
			"survivedMinutes": 0,
			"enemiesKilled": 0,
			"stageData": [],
			"showcase": [
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"AREA",
				"AREA",
				"AREA",
				"AREA",
				"AREA",
				"SPEED",
				"SPEED",
				"SPEED",
				"SPEED",
				"SPEED",
				"DURATION",
				"DURATION",
				"DURATION",
				"DURATION",
				"DURATION",
				"LUCK",
				"LUCK",
				"LUCK",
				"LUCK",
				"LUCK",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"AMOUNT",
				"AMOUNT"
			],
			"statModifiers": [
				{
					"level": 1,
					"maxHp": 120,
					"armor": 1,
					"regen": 0,
					"moveSpeed": 1,
					"power": 1,
					"area": 1,
					"speed": 1,
					"duration": 1,
					"amount": 0,
					"luck": 1,
					"growth": 1,
					"greed": 1,
					"curse": 1,
					"magnet": 0,
					"revivals": 0,
					"rerolls": 0,
					"skips": 0,
					"banish": 0,
					"shields": 0
				},
				{
					"power": 0.1,
					"level": 10
				},
				{
					"power": 0.1,
					"level": 20,
					"growth": 1
				},
				{
					"power": 0.1,
					"level": 30
				},
				{
					"power": 0.1,
					"level": 40,
					"growth": 1
				},
				{
					"power": 0.1,
					"level": 50
				},
				{
					"level": 21,
					"growth": -1
				},
				{
					"level": 41,
					"growth": -1
				}
			]
		}
	]
}
```
</details>

<details>

<summary>Json with comments</summary>

```jsonc
{
	"version": "0.2",
	"characters": [
		{
			"level": 1,
			"startingWeapon": "JUBILEE",
			"cooldown": 1,
			"prefix": "Lord",
			"charName": "Gus",
			"surname": "the cat",
			"textureName": "characters",
			"spriteName": "gus_01.png",
			"portraitName": "gus_portrait.png", // The image used in selection screen
			"skins": [
				{
					"id": 0,
					"name": "Default",
					"textureName": "characters",
					"spriteName": "gus_01.png",
					"walkingFrames": 4,
					"unlocked": true
				},
				{
					"id": 1,
					"name": "Legacy",
					"textureName": "characters",
					"spriteName": "gus_02.png",
					"walkingFrames": 4,
					"unlocked": true
				}
			],
			"currentSkinIndex": 0,
			"walkingFrames": 4,
			"description": "Gus the cat loves particles.",
			"isBought": true,
			"price": 0,
			"completedStages": [],
			"survivedMinutes": 0,
			"enemiesKilled": 0,
			"stageData": [],
			"showcase": [
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"SHADOWSERVANT",
				"AREA",
				"AREA",
				"AREA",
				"AREA",
				"AREA",
				"SPEED",
				"SPEED",
				"SPEED",
				"SPEED",
				"SPEED",
				"DURATION",
				"DURATION",
				"DURATION",
				"DURATION",
				"DURATION",
				"LUCK",
				"LUCK",
				"LUCK",
				"LUCK",
				"LUCK",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"COOLDOWN",
				"AMOUNT",
				"AMOUNT"
			],
			"statModifiers": [
				{ // Initial (level 1) stats are here.
					"level": 1,
					"maxHp": 120,
					"armor": 1,
					"regen": 0,
					"moveSpeed": 1,
					"power": 1,
					"area": 1,
					"speed": 1,
					"duration": 1,
					"amount": 0,
					"luck": 1,
					"growth": 1,
					"greed": 1,
					"curse": 1,
					"magnet": 0,
					"revivals": 0,
					"rerolls": 0,
					"skips": 0,
					"banish": 0,
					"shields": 0
				},
				{ // Stat effect after level 1 go next:
					"power": 0.1,
					"level": 10
				},
				{
					"power": 0.1,
					"level": 20,
					"growth": 1
				},
				{
					"power": 0.1,
					"level": 30
				},
				{
					"power": 0.1,
					"level": 40,
					"growth": 1
				},
				{
					"power": 0.1,
					"level": 50
				},
				{
					"level": 21,
					"growth": -1
				},
				{
					"level": 41,
					"growth": -1
				}
			]
		}
	]
}
```


</details>
