# No longer actively developing.
I will no longer be actively developing this mod. Feel free to fork this repo and continue development as long as you follow the [LICENSE](./LICENSE).
  
<br />
<br /><br />
<br /><br />
<br />


# <img src="./docs/images/00051-1768801398.ico" alt="Bloodlines Icon" width="30"/> Bloodlines - A custom character importer for Vampire Survivors
A mod for Vampire Survivors to help facilitate creating custom characters.

## Join the official bloodlines discord [Here!](https://discord.gg/ScfxCepsRb)

## Installing for the first time
1. Install MelonLoader - [Installation Instructions](https://melonwiki.xyz/#/?id=requirements) **IMPORTANT:** Start the game at least once prior to going to the next step.
3. Download bloodlines.dll from newest release - [Latest Release](https://github.com/nwfistere/bloodlines/releases/latest)
4. Add bloodlines.dll the mods folder in Vampire Survivors
   - Question: Where's my game installed?
	 - Answer: Go to your steam library, in the list of your games: right-click on Vampire Survivors, go to properties -> Installed Files -> Browse. That is the base folder of Vampire Survivors. If you installed Melonloader correctly you should see a mods folder.
5. Run Vampire Survivors again and the mod will create the required directories.
6. Add your custom character pack (zip file) into `..\Vampire Survivors\UserData\Bloodlines`. The zip file should be next to config.cfg. Do NOT unzip, the mod will do it for you.
7. Restart Vampire Survivors.

## Character Installation
Did your friend make a super cool custom character and they want to share it with you? All they have to do is share the character pack with you.
1. Download the zip file they send you (shared somewhere like discord)
2. copy the zip file to the  `<Vampire Survivors Location>/UserData/Bloodlines` folder. No need to unzip
4. start the game.

## Creating your own character packs!
### **New!** - Easily create a custom character using the new tool - [HERE](https://nwfistere.github.io/bloodlines/)
### Character packs contain two types of items:
1. A json file - This contains the character data: Starting weapon, which sprites to use, power level, etc.
2. png files - The character sprites. (These must be referenced by filename in the json file)

### How to zip files together for a character pack
1. select the files you want to zip together - press `ctrl` + `left-click` each file you want, they should all be highlighted.
2. right click and select Send to > Compressed (zipped) folder
3. Rename the pack to whatever you want.
5. View the contents of the zip by opening it with windows explorer or a zip utility like 7zip.
**Note: There should be no folders in the zip file, just the json file and png files.**

### See [examples](./examples) for info on how to format json file and example character pack.

## Known bugs/limitations
This mod is in very early in development stages, but I wanted to get testers as quickly as possible so I know what works and what doesn't with the packs and the mod.
1. Sprite scaling doesn't work in game. Keep your sprites generally around 50x50 pixels no bigger than 200x200px or they will be obnoxious in game.
2. Custom character progress isn't saved.
3. Plenty of visual bugs
    - Legionnaire's sprites don't spawn correctly


## Json Schema
See [examples](./examples)


## Known bugs/limitations
See: https://github.com/nwfistere/bloodlines/labels/bug Feel free to open any bugs.

## Future plans
See: https://github.com/nwfistere/bloodlines/labels/enhancement

## License
[See LICENSE](./LICENSE)
