# Bloodlines - A custom character importer for Vampire Survivors
A mod for Vampire Survivors to help facilitate creating custom characters.


## Installing for the first time
1. Install MelonLoader - [Installation Instructions](https://melonwiki.xyz/#/?id=requirements)		
	> [!WARNING]
	> **Important** The game must run at least once after installing melon loader before going onto the next steps.
2. Download bloodlines.dll from newest release - [Latest Release](https://github.com/nwfistere/bloodlines/releases/latest)
3. Add bloodlines.dll the mods folder in Vampire Survivors
   - Question: Where's my game installed?
	 - Answer: Go to your steam library, in the list of your games: right-click on Vampire Survivors, go to properties -> Installed Files -> Browse. That is the base folder of Vampire Survivors. If you installed Melonloader correctly you should see a mods folder.
4. Run Vampire Survivors again and the mod will create the required directories.
5. Add your custom character pack (zip file) into `..\Vampire Survivors\UserData\Bloodlines`. The zip file should be next to config.cfg. Do NOT unzip, the mod will do it for you.
6. Restart Vampire Survivors.

## Adding a character
Custom characters can be added to the game being downloading and copying a zip file to the Bloodlines directory ("Vampire Survivors/UserData/Bloodlines"). Once the game starts the character will be imported and the zip file will be deleted.

## Creating your own characters
**TODO:** More info needed

Zip files are used to import characters into the game.
The zip files contain two things:
1. character.json
 - This is a json document that holds all the info on the character. Name, starting weapon, strength, etc. Currently it mimics the Vampire Survivors character json data.
2. png files
 - The sprite(s) you want to use for the character. The json document must reference them.


## Json Schema
**TODO:** Need a wiki/readme for this.

## Sharing skins
**TODO:** Make a discord for this or something.

## Issues
See issues tab

## License
[See LICENSE](./LICENSE)
