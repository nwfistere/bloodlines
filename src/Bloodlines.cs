using Bloodlines.src;
using HarmonyLib;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.NumberTypes;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Il2Generic = Il2CppSystem.Collections.Generic;

namespace Bloodlines
{
    public static class ModInfo
    {
        public const string Name = "Bloodlines";
        public const string Description = "Easily add custom characters!";
        public const string Author = "Nick";
        public const string Company = "Nick";
        public const string Version = "0.1.0";
        public const string Download = "https://github.com/nwfistere/bloodlines";
    }

    public class BloodlinesMod : MelonMod
    {
        public static readonly string ModDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UserData", "Bloodlines");
        public static readonly string DataDirectory = Path.Combine(ModDirectory, "data");
        public Config Config { get; private set; }
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

            Config = new Config(Path.Combine(ModDirectory, "config.cfg"), "Bloodlines");
            manager = new(ModDirectory, Path.Combine(DataDirectory, "characters"));
        }

        public static CharacterManager getCharacterManager() => Melon<BloodlinesMod>.Instance.manager;

        public static bool isCustomCharacter(CharacterType characterType)
        {
            return getCharacterManager().characterDict.ContainsKey(characterType);
        }

#if DEBUG
        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            if (gameManager != null)
            {
            }
        }
#endif // DEBUG
        [HarmonyPatch("Il2CppInterop.HarmonySupport.Il2CppDetourMethodPatcher", "ReportException")]
        public static class Patch_Il2CppDetourMethodPatcher
        {
            public static bool Prefix(Exception ex)
            {
                MelonLogger.Error("During invoking native->managed trampoline", ex);
                return false;
            }
        }

        internal static JsonSerializerSettings serializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        static JArray ListToJArray(Il2Generic.List<CharacterData> list)
        {
            string result = JsonConvert.SerializeObject(list, serializerSettings);
            return JArray.Parse(result);
        }

        public static Timer _Timer;

        public static void TimerCallback(object stateInfo)
        {
            if (gameManager != null && gameManager.PlayerOne != null && gameManager.PlayerOne.PlayerStats != null)
            {
                PlayerModifierStats stats = gameManager.PlayerOne.PlayerStats;
                PropertyInfo[] statsProps = stats.GetType().GetProperties();
                List<string> ignoreFileds = new() { "ObjectClass", "Pointer", "WasCollected" };

                Melon<BloodlinesMod>.Logger.Msg("\n==============================\n");

                foreach (PropertyInfo prop in statsProps)
                {
                    if (prop.Name.Contains("BackingField") || ignoreFileds.Contains(prop.Name))
                        continue;

                    if (prop.PropertyType == typeof(EggFloat))
                        Melon<BloodlinesMod>.Logger
                            .Msg($"{prop.Name} = Value: <{(prop.GetValue(stats) as EggFloat).GetValue()}> EggValue: <{(prop.GetValue(stats) as EggFloat).GetEggValue()}>");
                    else if (prop.PropertyType == typeof(EggDouble))
                        Melon<BloodlinesMod>.Logger
                            .Msg($"{prop.Name} = Value: <{(prop.GetValue(stats) as EggDouble).GetValue()}> EggValue: <{(prop.GetValue(stats) as EggDouble).GetEggValue()}>");
                    else
                        Melon<BloodlinesMod>.Logger.Msg($"{prop.Name} = <{prop.GetValue(stats)}>");
                }
            }
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
                Melon<BloodlinesMod>.Logger.Msg($"GameManager.{MethodBase.GetCurrentMethod()?.Name}");
            }

            // InitializeGameSession

            [HarmonyPatch(nameof(GameManager.InitializeGameSession))]
            [HarmonyPostfix]
            static void InitializeGameSession_Postfix(GameManager __instance)
            {
                Melon<BloodlinesMod>.Logger.Msg($"GameManager.{MethodBase.GetCurrentMethod()?.Name}");

                foreach (CharacterController c in __instance._characters)
                {
                    CharacterType characterType = c.CharacterType;

                    Melon<BloodlinesMod>.Logger
                        .Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name} - {characterType}");

                    if (BloodlinesMod.isCustomCharacter(characterType))
                    {
                        CharacterDataModelWrapper character = getCharacterManager().characterDict[characterType];
                        int skinNum = c.CurrentSkinData.currentSkinIndex;
                        SkinObjectModelv0_2 skin = character.Skin(skinNum);

                        if (skin != null)
                        {
                            c.Rend.sprite = SpriteImporter.LoadCharacterSprite(character.SkinPath(skinNum));
                        }
                        else
                        {
                            c.Rend.sprite = SpriteImporter.LoadCharacterSprite(character.SpritePath);
                        }

                        foreach (string frame in skin.frames)
                        {
                            string framePath = System.IO.Path.Join(character.BaseDirectory, frame);
                            c.SpriteAnimation._animations["walk"]._frames.Add(SpriteImporter.LoadCharacterSprite(framePath));
                        }

                        c.SpriteAnimation.Play("walk");
                        // c.gameObject.transform.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(0, -0.2f);
                        // c.gameObject.transform.Find("WickedSeason").localPosition = new Vector3(0, -0.2f, 0);
                    }
                }

#if DEBUG
                BloodlinesMod._Timer = new Timer(TimerCallback, null, 0, 10000); // List stats every 2 seconds.
#endif
            }
        }

        [HarmonyPatch(typeof(RecapPage))]
        class RecapPage_Patch
        {
            // Get's ran start of game.
            [HarmonyPatch(nameof(RecapPage.Construct))]
            [HarmonyPrefix]
            static void Construct_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.NextCharacter))]
            [HarmonyPrefix]
            static void NextCharacter_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.PreviousCharacter))]
            [HarmonyPrefix]
            static void PreviousCharacter_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            // Called from OnShowStart
            [HarmonyPatch(nameof(RecapPage.RefreshCharacterSpecificStats))]
            [HarmonyPrefix]
            static void RefreshCharacterSpecificStats_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            // Runs first on gameover
            [HarmonyPatch(nameof(RecapPage.OnShowStart))]
            [HarmonyPrefix]
            static void OnShowStart_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.SetInfo))]
            [HarmonyPrefix]
            static void SetInfo_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            // Called from RefreshCharacterSpecificStats
            [HarmonyPatch(nameof(RecapPage.SetCharacter))]
            [HarmonyPrefix]
            static void SetCharacter_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.SpawnDestructible))]
            [HarmonyPrefix]
            static void SpawnDestructible_Prefix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.Construct))]
            [HarmonyPostfix]
            static void Construct_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.NextCharacter))]
            [HarmonyPostfix]
            static void NextCharacter_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.PreviousCharacter))]
            [HarmonyPostfix]
            static void PreviousCharacter_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.RefreshCharacterSpecificStats))]
            [HarmonyPostfix]
            static void RefreshCharacterSpecificStats_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.OnShowStart))]
            [HarmonyPostfix]
            static void OnShowStart_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
                if (isCustomCharacter(__instance._currentCharacter.CharacterType))
                {
                    CharacterDataModelWrapper character = getCharacterManager().characterDict[__instance._currentCharacter.CharacterType];
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(character.SpritePath);
                }
            }

            [HarmonyPatch(nameof(RecapPage.SetInfo))]
            [HarmonyPostfix]
            static void SetInfo_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.SetCharacter))]
            [HarmonyPostfix]
            static void SetCharacter_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }

            [HarmonyPatch(nameof(RecapPage.SpawnDestructible))]
            [HarmonyPostfix]
            static void SpawnDestructible_Postfix(RecapPage __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
            }
        }

        [HarmonyPatch(typeof(CharacterController))]
        class CharacterController_Patch
        {
            [HarmonyPatch(nameof(CharacterController.SetCharacterSprite))]
            [HarmonyPrefix]
            static void InitCharacter_Prefix(CharacterController __instance)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name}");
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

                foreach (CharacterDataModelWrapper characterWrapper in Melon<BloodlinesMod>.Instance.manager.characters)
                {
                    CharacterDataModel character = characterWrapper.Character;
                    CharacterType characterType = iter++;
                    characterWrapper.characterType = characterType;
                    Melon<BloodlinesMod>.Logger.Msg($"Adding character... {characterType} {character.CharName}");
                    character.CharacterType = characterType;

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(characterWrapper.CharacterSettings, Newtonsoft.Json.Formatting.Indented,
                        new Newtonsoft.Json.JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

                    JArray json = JArray.Parse(jsonString);
                    __instance.AllCharactersJson.Add(characterType.ToString(), json);
                    Melon<BloodlinesMod>.Instance.manager.characterDict.Add(characterType, characterWrapper);
                }
            }
        }

        [HarmonyPatch(typeof(CharacterSelectionPage))]
        class CharacterSelectionPage_Patch
        {
            [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
            [HarmonyPostfix]
            static void NextSkin_Postfix(CharacterSelectionPage __instance)
            {
                CharacterType characterType = __instance._currentType;

                if (isCustomCharacter(characterType))
                {
                    CharacterDataModelWrapper character = getCharacterManager().characterDict[characterType];

                    int activeSkinIndex = __instance._skinSlots.FindIndex(new Func<Image, bool>((s) => s.sprite.name == "weaponLevelFull"));

                    if (activeSkinIndex == -1)
                        activeSkinIndex = 0;

                    Sprite sprite = SpriteImporter.LoadSprite(character.SkinPath(activeSkinIndex));
                    __instance.Icon.sprite = sprite;
                    __instance._selectedCharacter._CharacterIcon.sprite = sprite;
                }
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.SetIconSizes))]
            [HarmonyPrefix]
            static bool SetIconSizes_Prefix(CharacterSelectionPage __instance, MethodBase __originalMethod)
            {
                if (isCustomCharacter(__instance._currentType))
                {
                    return false;
                }

                return true;
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
            [HarmonyPostfix]
            static void ShowCharacterInfo_Postfix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                if (isCustomCharacter(cType))
                {
                    Melon<BloodlinesMod>.Logger.Msg($"Setting the icon for {cType}");
                    CharacterDataModelWrapper ch = getCharacterManager().characterDict[cType];
                    int activeSkinIndex = __instance._skinSlots.FindIndex(new Func<Image, bool>((s) => s.sprite.name == "weaponLevelFull"));

                    if (activeSkinIndex == -1)
                    {
                        activeSkinIndex = 0;
                    }

                    Sprite sprite = SpriteImporter.LoadSprite(ch.SkinPath(activeSkinIndex));

                    __instance.Icon.sprite = sprite;
                    __instance._Name.text = charData.GetFullNameUntranslated();
                    __instance.Description.text = charData.description;
                    __instance.StatsPanel.SetCharacter(charData, cType);
                    __instance._EggCount.text = charData.exLevels.ToString();
                    __instance.SetWeaponIconSprite(charData);
                    __instance._selectedCharacter = character;
                    // UI/Canvas - App/Safe Area/View - CharacterSelection/Panel/InfoPanel/Background/CharacterImage
                    RectTransform CharacterInfoIconRectTransform = __instance.transform.FindChild("Panel/InfoPanel/Background/CharacterImage").GetComponent<RectTransform>();

                    int width = sprite.texture.width;
                    int height = sprite.texture.height;

                    // Resize to fit the info box better.
                    int long_side = width > height ? width : height;
                    int delta = 150 - long_side;

                    CharacterInfoIconRectTransform.sizeDelta = new Vector2(width + delta, height + delta);
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
                if (isCustomCharacter(cType))
                {
                    CharacterDataModelWrapper ch = getCharacterManager().characterDict[cType];

                    __instance.name = dat.charName;
                    __instance._CharacterName.text = dat.charName;
                    __instance._page = page;
                    __instance._playerOptions = dataManager._playerOptions;
                    __instance._data = dat;
                    __instance._dataManager = dataManager;
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(ch.PortraitPath);
                    __instance._CharacterIcon.sprite.name = __instance.name;
                    __instance._CharacterIcon.sprite.texture.name = __instance.name;
                    __instance._defaultTextColor = new Color(1, 1, 1, 1);
                    __instance._nameText = __instance._CharacterName;
                    __instance.Type = cType;
                    __instance._Background.name = __instance.name;
                    __instance.SetWeaponIconSprite(dat);

                    // TODO: I'm pretty sure this isn't needed anymore. TBD
                    // TODO: put this somewhere...
                    // dat.portraitName = dat.spriteName;
                    // dat.onEveryLevelUp = new ModifierStats() { Amount = 1 };
                    // dat.bgm = "NONE"; // TODO: what is this? A: Background Modifier? There's a type for it. BGMType A: background music!
                    // dat.isBought = true;

                    playerOptions._config.BoughtCharacters.Add(cType);

                    return false;
                }

                return true;
            }
        }
    }
}