# Examples

In this directory will be example character packs

## Gus.zip
This is a basic example of a character pack. A json file with image(s).
The json data is a copy of Antonio's settings with the starting weapon changed to an AXE. Check the Vampire Survivors wiki for the names to use for starting weapon (the ID tag). 

## Example json:
With version 0.1, the character json is a copy of what vampire survivors use as serialized data of their characters. This character "Gus" is a copy of Antonio's data.

The format of this json body will be changing in future releases, but I'm hoping to keep it somewhat backwards compatible after the first release.

v0.1:

<details>

<summary>Copyable json v0.1</summary>

```json
{
	"version": "0.1",
	"character": [
		{
			"level": 1,
			"startingWeapon": "AXE",
			"cooldown": 1,
			"charName": "Gus",
			"surname": "the cat",
			"textureName": "characters",
			"spriteName": "gus_01.png",
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
			"showcase": [
				"AXE"
			]
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
```
</details>

<details>

<summary>Json with comments</summary>

```jsonc

{
	"version": "0.1",
	"character": [
		{
			"level": 1,
			"startingWeapon": "AXE",
			"cooldown": 1,
			"charName": "Gus",
			"surname": "the cat",
			// textureName is unused, leave as characters.
			"textureName": "characters",
			"spriteName": "gus_01.png",
			"skins": [
				{
					// ids should stay incremental from 0 to 4
					"id": 0,
					"name": "Default",
					"textureName": "characters",
					"spriteName": "gus_01.png",
					// walkingFrames are ignored, animation isn't working yet.
					"walkingFrames": 4,
					"unlocked": true
				},
				{
					// ids should stay incremental from 0 to 4
					"id": 1,
					"name": "Legacy",
					"textureName": "characters",
					"spriteName": "gus_02.png",
					// walkingFrames are ignored, animation isn't working yet.
					"walkingFrames": 4,
					"unlocked": true
				}
			],
			"currentSkinIndex": 0,
			// walkingFrames are ignored, animation isn't working yet. This one will likely be removed
			"walkingFrames": 4,
			"description": "Gus the cat loves particles.",
			"isBought": true,
			"price": 0,
			// Progress isn't saved on custom characters yet!
			"completedStages": [],
			"survivedMinutes": 0,
			"enemiesKilled": 0,
			"stageData": [],
			// From here down to showcase are acceptable modifiers for the json objects below this one.
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
			// Not exactly sure what showcase does, but it has several copies of weapon ids in it. Removed for readability.
			// Has something to do with either chests or level up item chance..
			"showcase": [
				"AXE"
			]
		},
		// These objects are modifiers, they change a certain attribute at the given level.
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
```


</details>
