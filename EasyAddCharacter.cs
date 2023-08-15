using EasyAddCharacter.Textures;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using WNP78;
using Il2Generic = Il2CppSystem.Collections.Generic;
using EasyAddCharacter.Config;
using MelonLoader.TinyJSON;
using Il2CppVampireSurvivors.Objects.Characters;

namespace EasyAddCharacter
{
    public static class ModInfo
    {
        public const string Name = "VSEasyAddCharacter";
        public const string Description = "Easily add new characters!";
        public const string Author = "Nick";
        public const string Company = "Nick";
        public const string Version = "0.1.0";
        public const string Download = "https://github.com/nwfistere/VSEasyAddCharacter";
    }

    public class Mod : MelonMod
    {
        public static readonly string ModDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UserData", "EasyAddCharacter");
        public static readonly string DataDirectory = Path.Combine(ModDirectory, "data");
        public Config.Config Config { get; private set; }
        public CharacterManager manager { get; private set; }

        static DataManager dataManager;
        static GameManager gameManager;

        public override void OnInitializeMelon()
        {
            if (!Directory.Exists(ModDirectory))
            {
                Directory.CreateDirectory(ModDirectory);
                Directory.CreateDirectory(DataDirectory);
            }
            Config = new Config.Config(Path.Combine(ModDirectory, "config.cfg"), "EasyAddCharacter");
            manager = new(ModDirectory, Path.Combine(DataDirectory, "characters"));
        }

        [HarmonyPatch("Il2CppInterop.HarmonySupport.Il2CppDetourMethodPatcher", "ReportException")]
        public static class Patch_Il2CppDetourMethodPatcher
        {
            public static bool Prefix(System.Exception ex)
            {
                MelonLogger.Error("During invoking native->managed trampoline", ex);
                return false;
            }
        }

        internal static JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        static Il2Generic.List<CharacterData> char_list = new();

        private static JArray ListToJArray(Il2Generic.List<CharacterData> list)
        {
            string result = JsonConvert.SerializeObject(list, serializerSettings);
            return JArray.Parse(result);
        }

        [HarmonyPatch(typeof(GameManager))]
        class GameManager_Patch
        {
            [HarmonyPatch(nameof(GameManager.Construct))]
            [HarmonyPrefix]
            static void Construct_Prefix(GameManager __instance)
            {
                gameManager = __instance;
            }

            [HarmonyPatch(nameof(GameManager.Construct))]
            [HarmonyPostfix]
            static void Construct_Postfix(GameManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }
        }


        [HarmonyPatch(typeof(CharacterController))]
        class CharacterController_Patch
        {
            [HarmonyPatch(nameof(CharacterController.InitCharacter))]
            [HarmonyPrefix]
            static void InitCharacter_Prefix(CharacterController __instance, CharacterType characterType, int playerIndex)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterController.InitCharacter))]
            [HarmonyPostfix]
            static void InitCharacter_Postfix(CharacterController __instance, CharacterType characterType, int playerIndex)
            {
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(characterType))
                {
                    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == characterType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

                    __instance.Rend.sprite = SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename));
                }
            }
        }

        [HarmonyPatch(typeof(DataManager))]
        class DataManager_Patch
        {
            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPrefix]
            static void Initialize_Prefix(DataManager __instance)
            {
                dataManager = __instance;
            }

            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPostfix]
            static void LoadBaseJObjects_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                CharacterType iter = Enum.GetValues<CharacterType>().Max() + 1;

                foreach (Character character in Melon<Mod>.Instance.manager.characters)
                {
                    CharacterType characterType = iter++;
                    Melon<Mod>.Logger.Msg($"Adding character... {characterType} {(character.CharacterFileJson as CharacterFileV0_1).Character[0].CharName}");
                    character.CharacterType = characterType;
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(character.CharacterFileJson.GetCharacterJson());
                    JArray json = JArray.Parse(jsonString);
                    __instance.AllCharactersJson.Add(characterType.ToString(), json);
                }
            }

            [HarmonyPatch(nameof(DataManager.GetConvertedCharacterData))]
            [HarmonyPostfix]
            static void GetConvertedCharacterData_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod,
                ref Il2Generic.Dictionary<CharacterType, Il2Generic.List<CharacterData>> __result)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name} for {__result.Count} characters");
            }
        }

        [HarmonyPatch(typeof(CharacterSelectionPage))]
        class CharacterSelectionPage_Patch
        {
            [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
            [HarmonyPrefix]
            static bool ShowCharacterInfo_Prefix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    return false;
                }
                return true;
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
            [HarmonyPostfix]
            static void ShowCharacterInfo_Postfix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    Melon<Mod>.Logger.Msg($"Setting the icon for {cType}");
                    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;
                    __instance.Icon.sprite = SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename));
                    __instance._Name.text = charData.GetFullNameUntranslated();
                    __instance.Description.text = charData.description;
                    __instance.StatsPanel.SetCharacter(charData, cType);
                    __instance._EggCount.text = charData.exLevels.ToString();
                    __instance.SetWeaponIconSprite(charData);
                    __instance._selectedCharacter = character;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterItemUI))]
        class CharacterItemUI_Patch
        {
            [HarmonyPatch(nameof(CharacterItemUI.SetData))]
            [HarmonyPrefix]
            static bool SetData_Prefix(CharacterItemUI __instance, MethodBase __originalMethod, CharacterSelectionPage page, CharacterData dat, CharacterType cType, DataManager dataManager, PlayerOptions playerOptions)
            {
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

                    __instance.name = dat.charName;
                    __instance._CharacterName.text = dat.charName;
                    __instance._page = page;
                    __instance._playerOptions = dataManager._playerOptions;
                    __instance._data = dat;
                    __instance._dataManager = dataManager;
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename));
                    __instance._CharacterIcon.sprite.name = __instance.name;
                    __instance._CharacterIcon.sprite.texture.name = __instance.name;
                    __instance._defaultTextColor = new Color(1, 1, 1, 1);
                    __instance._nameText = __instance._CharacterName;
                    __instance.Type = cType;
                    __instance._Background.name = __instance.name;
                    __instance.SetWeaponIconSprite(dat);

                    // TODO: put this somewhere...
                    dat.portraitName = dat.spriteName;
                    dat.onEveryLevelUp = new ModifierStats() { Amount = 1 };
                    dat.bgm = "NONE"; // TODO: what is this? A: Background Modifier? There's a type for it. BGMType
                    dat.isBought = true;

                    playerOptions._config.BoughtCharacters.Add(cType);

                    return false;
                }
                return true;
            }

            [HarmonyPatch(nameof(CharacterItemUI.SetData))]
            [HarmonyPostfix]
            static void SetData_Postfix(CharacterItemUI __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
            }
        }
    }
}