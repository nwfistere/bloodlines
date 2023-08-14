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

        static DataManager dataManager;
        static GameManager gameManager;

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
            static void Initialize_Prefix(GameManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                gameManager = __instance;
            }

            [HarmonyPatch(nameof(GameManager.Construct))]
            [HarmonyPostfix]
            static void Initialize_Postfix(GameManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        [HarmonyPatch(typeof(DataManager))]
        class DataManager_Patch
        {
            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPrefix]
            static void Initialize_Prefix(DataManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                dataManager = __instance;
            }

            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPostfix]
            static void Initialize_Postfix(DataManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPostfix]
            static void LoadBaseJObjects_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
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

            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPrefix]
            static void LoadBaseJObjects_Prefix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPostfix]
            static void MergeInJsonData_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPrefix]
            static void MergeInJsonData_Prefix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadAndMergeIn))]
            [HarmonyPostfix]
            static void LoadAndMergeIn_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadAndMergeIn))]
            [HarmonyPrefix]
            static void LoadAndMergeIn_Prefix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.GetConvertedCharacterData))]
            [HarmonyPostfix]
            static void GetConvertedCharacterData_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod,
                ref Il2Generic.Dictionary<CharacterType, Il2Generic.List<CharacterData>> __result)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name} for {__result.Count} characters");
            }

            [HarmonyPatch(nameof(DataManager.GetConvertedCharacterData))]
            [HarmonyPrefix]
            static void GetConvertedCharacterData_Prefix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadDataFromJson))]
            [HarmonyPostfix]
            static void LoadDataFromJson_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadDataFromJson))]
            [HarmonyPrefix]
            static void LoadDataFromJson_Prefix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        [HarmonyPatch(typeof(CharacterSelectionPage))]
        class CharacterSelectionPage_Patch
        {
            [HarmonyPatch(nameof(CharacterSelectionPage.Start))]
            [HarmonyPrefix]
            static void Start_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.Construct))]
            [HarmonyPrefix]
            static void Construct_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.SetWeaponIconSprite))]
            [HarmonyPrefix]
            static void SetWeaponIconSprite_Prefix(CharacterSelectionPage __instance, CharacterData characterData, MethodBase __originalMethod)
            {
                if (characterData.charName == "Nick")
                    Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.SetWeaponIconSprite))]
            [HarmonyPostfix]
            static void SetWeaponIconSprite_Postfix(CharacterSelectionPage __instance, CharacterData characterData, MethodBase __originalMethod)
            {
                if (characterData.charName == "Nick")
                    Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.CharCodeToString))]
            [HarmonyPrefix]
            static void CharCodeToString_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.AddCharacter))]
            [HarmonyPrefix]
            static void AddCharacter_Prefix(CharacterSelectionPage __instance, CharacterData dat, CharacterType cType, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
            [HarmonyPrefix]
            static void NextSkin_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.Start))]
            [HarmonyPostfix]
            static void Start_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.Construct))]
            [HarmonyPostfix]
            static void Construct_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.CharCodeToString))]
            [HarmonyPostfix]
            static void CharCodeToString_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.AddCharacter))]
            [HarmonyPostfix]
            static void AddCharacter_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
            [HarmonyPostfix]
            static void NextSkin_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
            [HarmonyPrefix]
            static bool ShowCharacterInfo_Prefix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
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
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    Melon<Mod>.Logger.Msg($"Setting the icon for {cType}");
                    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;
                    __instance.Icon.sprite = SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename));
                    __instance._Name.text = charData.GetFullNameUntranslated();
                    __instance.Description.text = charData.description;
                    //__instance.StatsPanel.Populate();
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
            //SetInfoPanel
            [HarmonyPatch(nameof(CharacterItemUI.SetInfoPanel))]
            [HarmonyPrefix]
            static void SetInfoPanel_Prefix(CharacterItemUI __instance)
            {
                Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterItemUI.SetInfoPanel))]
            [HarmonyPostfix]
            static void SetInfoPanel_Postfix(CharacterItemUI __instance)
            {
                Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterItemUI.SetData))]
            [HarmonyPrefix]
            static bool SetData_Prefix(CharacterItemUI __instance, MethodBase __originalMethod, CharacterSelectionPage page, CharacterData dat, CharacterType cType, DataManager dataManager, PlayerOptions playerOptions)
            {
                if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

                    //__instance._voidWeapon = true;
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

        //[HarmonyPatch(typeof(Resources))]
        //class Resources_Patch
        //{
        //    [HarmonyPatch(nameof(Resources.LoadAll), new Type[] { typeof(string), typeof(Il2CppSystem.Type) })]
        //    [HarmonyPostfix]
        //    static void LoadAll_Postfix(Resources __instance, object[] __args, string path, MethodBase __originalMethod, Il2CppReferenceArray<UnityEngine.Object> __result)
        //    {
        //        Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
        //        if (path == "SpriteSheets")
        //        {
        //            FloatingCat.name = "floating-cat";
        //            FloatingCat.texture.name = "floating-cat";
        //            UnityEngine.Object.DontDestroyOnLoad(FloatingCat);

        //            _ = __result.AddItem(FloatingCat);
        //            //List<UnityEngine.Object> list = __result.Where(m => m.name.Contains("antonio")).ToList();
        //            //List<UnityEngine.Object> list2 = __result.Where(m => m.name.Contains("Antonio")).ToList();
        //            List<UnityEngine.Object> list2 = __result.Where(m => m.name.IndexOf("antonio", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //        }
        //    }
        //}
    }
}