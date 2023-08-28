using Bloodlines.src;
using HarmonyLib;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.NumberTypes;
using Il2CppVampireSurvivors.Graphics;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.Objects.Items;
using Il2CppVampireSurvivors.Objects.Pickups;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System;
using System.Collections;
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
        public const string Version = "0.2.0";
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
            manager = new(ModDirectory, DataDirectory, Path.Combine(DataDirectory, "characters"));
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

                if (ex.Data.Count > 0)
                {
                    MelonLogger.Error("Extra Data:");

                    foreach (DictionaryEntry de in ex.Data)
                        MelonLogger.Error("    Key: {0,-20}      Value: {1}",
                                          "'" + de.Key.ToString() + "'", de.Value);
                }

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
#if DEBUG
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
# endif // DEBUG
        }

        // SpriteAnimationController

        [HarmonyPatch(typeof(SpriteAnimationController))]
        static class SpriteAnimationController_Patch
        {
            [HarmonyPatch(nameof(SpriteAnimationController.OnUpdate), new Type[] { })]
            [HarmonyPrefix]
            static void OnUpdate_Prefix(SpriteAnimationController __instance)
            {
                // Melon<BloodlinesMod>.Logger.Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(SpriteAnimationController.Add), new Type[] { typeof(BaseSpriteAnimation) })]
            [HarmonyPrefix]
            static void Add_Prefix(SpriteAnimationController __instance, BaseSpriteAnimation baseSpriteAnimation)
            {
                Melon<BloodlinesMod>.Logger.Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(SpriteAnimationController.Remove), new Type[] { typeof(BaseSpriteAnimation) })]
            [HarmonyPrefix]
            static void Remove_Prefix(SpriteAnimationController __instance, BaseSpriteAnimation baseSpriteAnimation)
            {
                Melon<BloodlinesMod>.Logger.Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        [HarmonyPatch(typeof(Pickup))]
        class Pickup_Patch
        {
            // Bugfix: Allows gems to be picked up, even if the character is a big boy.
            [HarmonyPatch(nameof(Pickup.GoToThePlayer))]
            [HarmonyPostfix]
            static void GoToThePlayer_Postix(Gem __instance, MethodBase __originalMethod)
            {
                if (BloodlinesMod.isCustomCharacter(__instance.TargetPlayer._characterType))
                {
                    float distance = Vector2.Distance(__instance.position, __instance.TargetPlayer.position);
                    float closeEnough = 0.09f;

                    if (distance < closeEnough || float.IsInfinity(distance))
                    {
                        __instance.GetTaken();
                    }
                }
            }
        }

        // "idle" is an animation
        // "meleeA" is one for Maruto
        [HarmonyPatch(typeof(BaseSpriteAnimation))]
        [HarmonyPatch(nameof(BaseSpriteAnimation.Play), new Type[] { typeof(string), typeof(int) })]
        static class SpriteAnimation_Patch
        {
            [HarmonyPrefix]
            static void prefix(SpriteAnimation __instance, MethodBase __originalMethod, string animName, int frameRate)
            {
                Melon<BloodlinesMod>.Logger.Msg($"SpriteAnimation.{__originalMethod?.Name} - prefix");
            }

            [HarmonyPostfix]
            static void postfix(SpriteAnimation __instance, MethodBase __originalMethod, string animName, int frameRate)
            {
                Melon<BloodlinesMod>.Logger.Msg($"SpriteAnimation.{__originalMethod?.Name} - postfix");
            }
        }

        // SpriteAnimation
        [HarmonyPatch(typeof(BaseSpriteAnimation))]
        class BaseSpriteAnimation_Patch
        {
            [HarmonyPatch(nameof(BaseSpriteAnimation.Play), new Type[] { typeof(string) })]
            [HarmonyPrefix]
            static void Play_Prefix(BaseSpriteAnimation __instance, string animName)
            {
                Melon<BloodlinesMod>.Logger.Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}({animName})");
            }

            [HarmonyPatch(nameof(BaseSpriteAnimation.Play), new Type[] { typeof(string), typeof(int) })]
            [HarmonyPrefix]
            static void Play2_Prefix(BaseSpriteAnimation __instance, string animName, int frameRate)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}({animName}, {frameRate})");
            }

            [HarmonyPatch(nameof(BaseSpriteAnimation.Play), new Type[] { typeof(string) })]
            [HarmonyPostfix]
            static void Play_Postfix(BaseSpriteAnimation __instance, string animName)
            {
                Melon<BloodlinesMod>.Logger.Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}({animName})");
            }

            [HarmonyPatch(nameof(BaseSpriteAnimation.Play), new Type[] { typeof(string), typeof(int) })]
            [HarmonyPostfix]
            static void Play2_Postfix(BaseSpriteAnimation __instance, string animName, int frameRate)
            {
                Melon<BloodlinesMod>.Logger
                    .Msg($"BaseSpriteAnimation.{MethodBase.GetCurrentMethod()?.Name}({animName}, {frameRate})");
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
                        int skinNum = __instance.PlayerOptions.GetSkinIndexForCharacter(characterType);
                        SkinObjectModelv0_2 skin = character.Skin(skinNum);

                        if (skin != null)
                        {
                            c.Rend.sprite = SpriteImporter.LoadCharacterSprite(character.SkinPath(skinNum));
                        }
                        else
                        {
                            c.Rend.sprite = SpriteImporter.LoadCharacterSprite(character.SpritePath);
                        }

                        c.SpriteAnimation._animations["walk"]._frames.Clear();

                        foreach (string frame in skin.frames)
                        {
                            string framePath = System.IO.Path.Join(character.BaseDirectory, frame);
                            c.SpriteAnimation._animations["walk"]._frames.Add(SpriteImporter.LoadCharacterSprite(framePath));
                        }

                        if (skin.frames.Any())
                        {
                            c.SpriteAnimation.Play("walk");
                            c.IsAnimForced = skin.AlwaysAnimated;
                        }
                        else
                        {
                            c._hasWalkingAnimation = false;
                        }
                    }
                }

#if DEBUG
                BloodlinesMod._Timer = new Timer(TimerCallback, null, 0, 10000); // List stats every 10 seconds.
                // gameManager.Player.Debug_ToggleInvulnerability();
#endif
            }
        }

        [HarmonyPatch(typeof(RecapPage))]
        class RecapPage_Patch
        {
            [HarmonyPatch(nameof(RecapPage.OnShowStart))]
            [HarmonyPostfix]
            static void OnShowStart_Postfix(RecapPage __instance)
            {
#if DEBUG
                if (BloodlinesMod._Timer != null)
                {
                    BloodlinesMod._Timer.Dispose();
                    BloodlinesMod._Timer = null;
                }

#endif
                Melon<BloodlinesMod>.Logger
                    .Msg($"{typeof(RecapPage).FullName}.{MethodBase.GetCurrentMethod().Name}");

                if (isCustomCharacter(__instance._currentCharacter.CharacterType))
                {
                    CharacterDataModelWrapper character = getCharacterManager().characterDict[__instance._currentCharacter.CharacterType];
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(character.SpritePath);
                }
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
                    RectTransform CharacterInfoIconRectTransform = __instance.transform.FindChild("Panel/InfoPanel/Background/CharacterImage").GetComponent<RectTransform>();

                    int width = sprite.texture.width;
                    int height = sprite.texture.height;

                    // Resize to fit the info box better.
                    int long_side = width > height ? width : height;
                    int delta = 100 - long_side;

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
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(ch.SkinPath(playerOptions.GetSkinIndexForCharacter(cType)));
                    __instance._defaultTextColor = new Color(1, 1, 1, 1);
                    __instance._nameText = __instance._CharacterName;
                    __instance.Type = cType;
                    __instance._Background.name = __instance.name;
                    __instance.SetWeaponIconSprite(dat);

                    playerOptions._config.BoughtCharacters.Add(cType);

                    return false;
                }

                return true;
            }
        }
    }
}