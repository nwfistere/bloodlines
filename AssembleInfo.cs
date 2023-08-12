using EasyAddCharacter;
using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using System;

[assembly: AssemblyTitle(ModInfo.Description)]
[assembly: AssemblyDescription(ModInfo.Description)]
[assembly: AssemblyCompany(ModInfo.Company)]
[assembly: AssemblyProduct(ModInfo.Name)]
[assembly: AssemblyCopyright("Copyright " + ModInfo.Author + " 2023")]
[assembly: AssemblyTrademark(ModInfo.Company)]
[assembly: AssemblyVersion(ModInfo.Version)]
[assembly: AssemblyFileVersion(ModInfo.Version)]
[assembly: MelonInfo(typeof(Mod), ModInfo.Name, ModInfo.Version, ModInfo.Author, ModInfo.Download)]
[assembly: MelonGame("poncle", "Vampire Survivors")]

[assembly: ComVisible(false)]

[assembly: Guid("11dc165c-253d-489c-9d97-5b624208faf5")]

//[assembly: HarmonyDontPatchAll]