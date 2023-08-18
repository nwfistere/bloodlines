using HarmonyLib;
using Il2Cpp;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

#if DEBUG
        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            //if (gameManager != null)
            //{

            //}
        }
        [HarmonyPatch("Il2CppInterop.HarmonySupport.Il2CppDetourMethodPatcher", "ReportException")]
        public static class Patch_Il2CppDetourMethodPatcher
        {
            public static bool Prefix(Exception ex)
            {
                MelonLogger.Error("During invoking native->managed trampoline", ex);
                return false;
            }
        }
#endif // DEBUG

        internal static JsonSerializerSettings serializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

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

                    Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name} - {characterType}");
                    if (Melon<BloodlinesMod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(characterType))
                    {
                        Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == characterType);
                        string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

                        c.Rend.sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(spriteFilename));

                    }

                }
            }

            [HarmonyPatch(nameof(GameManager.InitializeGameSession))]
            [HarmonyPrefix]
            static void InitializeGameSession_Prefix(GameManager __instance)
            {
                Melon<BloodlinesMod>.Logger.Msg($"GameManager.{MethodBase.GetCurrentMethod()?.Name}");
            }
        }


        [HarmonyPatch(typeof(CharacterController))]
        class CharacterController_Patch
        {
            [HarmonyPatch(nameof(CharacterController.InitCharacter))]
            [HarmonyPostfix]
            static void InitCharacter_Postfix(CharacterController __instance, CharacterType characterType, int playerIndex)
            {
                if (Melon<BloodlinesMod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(characterType))
                {
                    Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == characterType);
                    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

                    __instance.IsInvul = true;
                    //__instance.Rend.sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(spriteFilename)); // SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename), new Rect(0, 0, 50, 50), new Vector2(0.5f, 0.5f));
                    //__instance.Rend.sprite.name = ch.CharacterInfo.CharName;
                    ////__instance.Rend.size = new Vector2(50, 50);
                    ////__instance.Rend.sprite.texture.Resize(50, 50, __instance.Rend.sprite.texture.graphicsFormat, false);
                    //__instance.WeaponsManager.AddEquipment(__instance.WeaponsManager.GetWeaponByType(ch.CharacterInfo.StartingWeapon));
                    ////if (__instance.weaponSelection == null)
                    ////    __instance.weaponSelection = new();
                    ////__instance.weaponSelection.Add(ch.CharacterInfo.StartingWeapon);
                    ////__instance.WeaponsManager.
                }
            }

            //[HarmonyPatch(nameof(CharacterController.SetCharacterSprite))]
            //[HarmonyPostfix]
            //static void InitCharacter_Postfix(CharacterController __instance)
            //{
            //    //CharacterType characterType = __instance._characterType;

            //    //Melon<Mod>.Logger.Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name}");
            //    //if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(characterType))
            //    //{
            //    //    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == characterType);
            //    //    string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;
            //    //    //__instance._spriteRenderer.sprite.texture = SpriteImporter.LoadTexture(ch.FullSpritePath(spriteFilename));
            //    //    Sprite sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(spriteFilename));
            //    //    sprite.name = "Gus";
            //    //    __instance._spriteRenderer.sprite = sprite;
            //    //    Vector2 size = new Vector2(50, 50);
            //    //    __instance._spriteRenderer.size = size;
            //    //}
            //}

            //[HarmonyPatch(nameof(CharacterController.AfterFullInitialization))]
            //[HarmonyPostfix]
            //static void AfterFullInitialization_Postfix(CharacterController __instance)
            //{
            //    Melon<Mod>.Logger.Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name}");
            //}

            //[HarmonyPatch(nameof(CharacterController.AfterFullInitialization))]
            //[HarmonyPrefix]
            //static void AfterFullInitialization_Prefix(CharacterController __instance)
            //{
            //    Melon<Mod>.Logger.Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name}");
            //}

            [HarmonyPatch(nameof(CharacterController.SetCharacterSprite))]
            [HarmonyPrefix]
            static void InitCharacter_Prefix(CharacterController __instance)
            {
                Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterController).FullName}.{MethodBase.GetCurrentMethod().Name}");
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

                foreach (Character character in Melon<BloodlinesMod>.Instance.manager.characters)
                {
                    CharacterType characterType = iter++;
                    Melon<BloodlinesMod>.Logger.Msg($"Adding character... {characterType} {(character.CharacterFileJson as CharacterFileV0_1).Character[0].CharName}");
                    character.CharacterType = characterType;
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(character.CharacterFileJson.GetCharacterJson());
                    JArray json = JArray.Parse(jsonString);
                    __instance.AllCharactersJson.Add(characterType.ToString(), json);
                }
            }
        }

        //[HarmonyPatch(typeof(CharacterSelectionPage))]
        //class CharacterSelectionPage_Patch
        //{
        //    [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
        //    [HarmonyPrefix]
        //    static bool ShowCharacterInfo_Prefix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
        //    {
        //        //if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
        //        //{
        //        //    return false;
        //        //}
        //        return true;
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.CharMarkup))]
        //    [HarmonyPrefix]
        //    static void CharMarkup_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Construct))]
        //    [HarmonyPrefix]
        //    static void Construct_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Start))]
        //    [HarmonyPrefix]
        //    static void Start_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ClearPlayersAndGoBack))]
        //    [HarmonyPrefix]
        //    static void ClearPlayersAndGoBack_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnDestroy))]
        //    [HarmonyPrefix]
        //    static void OnDestroy_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.RefreshCharacters))]
        //    [HarmonyPrefix]
        //    static void RefreshCharacters_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SelectAfterFrameDelay))]
        //    [HarmonyPrefix]
        //    static void SelectAfterFrameDelay_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ConfirmCharacter))]
        //    [HarmonyPrefix]
        //    static void ConfirmCharacter_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetEggs))]
        //    [HarmonyPrefix]
        //    static void SetEggs_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SelectCharacter))]
        //    [HarmonyPrefix]
        //    static void SelectCharacter_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.BuyCharacter))]
        //    [HarmonyPrefix]
        //    static void BuyCharacter_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.IncreaseMaxWeapons))]
        //    [HarmonyPrefix]
        //    static void IncreaseMaxWeapons_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
        //    [HarmonyPrefix]
        //    static void NextSkin_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.WrapNavigation))]
        //    [HarmonyPrefix]
        //    static void WrapNavigation_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
        //    [HarmonyPrefix]
        //    static void ShowCharacterInfo_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetIconSizes))]
        //    [HarmonyPrefix]
        //    static void SetIconSizes_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ResetDisplay))]
        //    [HarmonyPrefix]
        //    static void ResetDisplay_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetSkinSlots))]
        //    [HarmonyPrefix]
        //    static void SetSkinSlots_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnShowStart))]
        //    [HarmonyPrefix]
        //    static void OnShowStart_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnPlayerRemoved))]
        //    [HarmonyPrefix]
        //    static void OnPlayerRemoved_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ResetPlayerSelections))]
        //    [HarmonyPrefix]
        //    static void ResetPlayerSelections_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnEnterPressed))]
        //    [HarmonyPrefix]
        //    static void OnEnterPressed_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnHideFinish))]
        //    [HarmonyPrefix]
        //    static void OnHideFinish_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnHideStart))]
        //    [HarmonyPrefix]
        //    static void OnHideStart_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Detune))]
        //    [HarmonyPrefix]
        //    static void Detune_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.setupRNJ))]
        //    [HarmonyPrefix]
        //    static void setupRNJ_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.setupMIS))]
        //    [HarmonyPrefix]
        //    static void setupMIS_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.CharCodeToString))]
        //    [HarmonyPrefix]
        //    static void CharCodeToString_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Populate))]
        //    [HarmonyPrefix]
        //    static void Populate_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.AddCharacter))]
        //    [HarmonyPrefix]
        //    static void AddCharacter_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.WaitAndDo))]
        //    [HarmonyPrefix]
        //    static void WaitAndDo_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ActivateAndSelectConfirmButton))]
        //    [HarmonyPrefix]
        //    static void ActivateAndSelectConfirmButton_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetWeaponIconSprite))]
        //    [HarmonyPrefix]
        //    static void SetWeaponIconSprite_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.MakeDisplayMultiplayer))]
        //    [HarmonyPrefix]
        //    static void MakeDisplayMultiplayer_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.MakeDisplaySingleplayer))]
        //    [HarmonyPrefix]
        //    static void MakeDisplaySingleplayer_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.RefreshMaxWeaponsAndEggsDisplay))]
        //    [HarmonyPrefix]
        //    static void RefreshMaxWeaponsAndEggsDisplay_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SpawnPlayerItem))]
        //    [HarmonyPrefix]
        //    static void SpawnPlayerItem_Prefix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.CharMarkup))]
        //    [HarmonyPostfix]
        //    static void CharMarkup_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Construct))]
        //    [HarmonyPostfix]
        //    static void Construct_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Start))]
        //    [HarmonyPostfix]
        //    static void Start_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ClearPlayersAndGoBack))]
        //    [HarmonyPostfix]
        //    static void ClearPlayersAndGoBack_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnDestroy))]
        //    [HarmonyPostfix]
        //    static void OnDestroy_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.RefreshCharacters))]
        //    [HarmonyPostfix]
        //    static void RefreshCharacters_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SelectAfterFrameDelay))]
        //    [HarmonyPostfix]
        //    static void SelectAfterFrameDelay_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ConfirmCharacter))]
        //    [HarmonyPostfix]
        //    static void ConfirmCharacter_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetEggs))]
        //    [HarmonyPostfix]
        //    static void SetEggs_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SelectCharacter))]
        //    [HarmonyPostfix]
        //    static void SelectCharacter_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.BuyCharacter))]
        //    [HarmonyPostfix]
        //    static void BuyCharacter_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.IncreaseMaxWeapons))]
        //    [HarmonyPostfix]
        //    static void IncreaseMaxWeapons_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
        //    [HarmonyPostfix]
        //    static void NextSkin_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.WrapNavigation))]
        //    [HarmonyPostfix]
        //    static void WrapNavigation_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
        //    [HarmonyPostfix]
        //    static void ShowCharacterInfo_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetIconSizes))]
        //    [HarmonyPostfix]
        //    static void SetIconSizes_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ResetDisplay))]
        //    [HarmonyPostfix]
        //    static void ResetDisplay_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetSkinSlots))]
        //    [HarmonyPostfix]
        //    static void SetSkinSlots_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnShowStart))]
        //    [HarmonyPostfix]
        //    static void OnShowStart_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnPlayerRemoved))]
        //    [HarmonyPostfix]
        //    static void OnPlayerRemoved_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ResetPlayerSelections))]
        //    [HarmonyPostfix]
        //    static void ResetPlayerSelections_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnEnterPressed))]
        //    [HarmonyPostfix]
        //    static void OnEnterPressed_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnHideFinish))]
        //    [HarmonyPostfix]
        //    static void OnHideFinish_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.OnHideStart))]
        //    [HarmonyPostfix]
        //    static void OnHideStart_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Detune))]
        //    [HarmonyPostfix]
        //    static void Detune_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.setupRNJ))]
        //    [HarmonyPostfix]
        //    static void setupRNJ_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.setupMIS))]
        //    [HarmonyPostfix]
        //    static void setupMIS_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.CharCodeToString))]
        //    [HarmonyPostfix]
        //    static void CharCodeToString_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.Populate))]
        //    [HarmonyPostfix]
        //    static void Populate_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.AddCharacter))]
        //    [HarmonyPostfix]
        //    static void AddCharacter_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.WaitAndDo))]
        //    [HarmonyPostfix]
        //    static void WaitAndDo_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.ActivateAndSelectConfirmButton))]
        //    [HarmonyPostfix]
        //    static void ActivateAndSelectConfirmButton_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetWeaponIconSprite))]
        //    [HarmonyPostfix]
        //    static void SetWeaponIconSprite_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.MakeDisplayMultiplayer))]
        //    [HarmonyPostfix]
        //    static void MakeDisplayMultiplayer_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.MakeDisplaySingleplayer))]
        //    [HarmonyPostfix]
        //    static void MakeDisplaySingleplayer_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.RefreshMaxWeaponsAndEggsDisplay))]
        //    [HarmonyPostfix]
        //    static void RefreshMaxWeaponsAndEggsDisplay_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SpawnPlayerItem))]
        //    [HarmonyPostfix]
        //    static void SpawnPlayerItem_Postfix(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }


        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetSkinSlots))]
        //    [HarmonyPrefix]
        //    static void SetSkinSlots_Prefix(CharacterSelectionPage __instance, MethodBase __originalMethod)
        //    {
        //        //__instance.cha
        //        //if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
        //        //{
        //        //    return false;
        //        //}
        //        //return true;
        //        Melon<BloodlinesMod>.Logger.Msg($"SetSkinSlots_Prefix");
        //        //if (Melon<Mod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(__instance._currentType))
        //        //{
        //        //    //Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == __instance._currentType);
        //        //    //string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;

        //        //    //GameObject character = GameObject.Instantiate(__instance.CharacterPrefab);
        //        //    //Image image = character.AddComponent<Image>();
        //        //    //image.sprite = SpriteImporter.LoadSprite(Path.Combine(Path.GetDirectoryName(ch.CharacterFilePath), spriteFilename));

        //        //    //Melon<Mod>.Logger.Msg($"skin for {__instance._currentType}");
        //        //    //__instance._skinSlots.Add(image);

        //        //    Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == __instance._currentType);
        //        //    int nSkins = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].Skins.Count;

        //        //    //for (int i = 0; i < nSkins; i++)
        //        //    //{
        //        //    //    __instance._SkinOffIcon
        //        //    //    __instance._skinSlots.Add()
        //        //    //}
        //        //}
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetSkinSlots))]
        //    [HarmonyPostfix]
        //    static void SetSkinSlots_Postfix(CharacterSelectionPage __instance, MethodBase __originalMethod)
        //    {
        //        //SetSkinSlots creates the skin slots, not the skins!

        //        //Character ch = Melon<Mod>.Instance.manager.characters.Find(c => c.CharacterType == __instance._currentType);
        //        ////string spriteFilename = ch.CharacterInfo.Skins;
        //        //Melon<Mod>.Logger.Msg($"SetSkinSlots_Postfix");
        //        //for (int i = 0; i < __instance._skinSlots.Count; i++)
        //        //    __instance._skinSlots[i].sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(ch.CharacterInfo.Skins[i].SpriteName));
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
        //    [HarmonyPrefix]
        //    static void NextSkin_Prefix2(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //    }

        //    [HarmonyPatch(nameof(CharacterSelectionPage.NextSkin))]
        //    [HarmonyPostfix]
        //    static void NextSkin_Postfix2(CharacterSelectionPage __instance)
        //    {
        //        Melon<BloodlinesMod>.Logger.Msg($"{typeof(CharacterSelectionPage).FullName}.{MethodBase.GetCurrentMethod().Name}");
        //        Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == __instance._currentType);

        //        int activeSkinIndex = __instance._skinSlots.FindIndex(new Func<Image, bool>((s) => s.sprite.name == "weaponLevelFull"));
        //        Sprite sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(ch.CharacterInfo.Skins[activeSkinIndex].SpriteName));
        //        __instance.Icon.sprite = sprite;
        //        __instance._selectedCharacter._CharacterIcon.sprite = sprite;
        //    }


        //    [HarmonyPatch(nameof(CharacterSelectionPage.SetIconSizes))]
        //    [HarmonyPrefix]
        //    static bool SetIconSizes_Prefix(CharacterSelectionPage __instance, MethodBase __originalMethod)
        //    {

        //        Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == __instance._currentType);
        //        Melon<BloodlinesMod>.Logger.Msg($"SetIconSizes_Prefix");
        //        if (Melon<BloodlinesMod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(__instance._currentType))
        //        {
        //            return false;
        //        }
        //        return true;
        //    }



        //    [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
        //    [HarmonyPostfix]
        //    static void ShowCharacterInfo_Postfix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
        //    {
        //        if (Melon<BloodlinesMod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
        //        {
        //            Melon<BloodlinesMod>.Logger.Msg($"Setting the icon for {cType}");
        //            Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
        //            string spriteFilename = (ch.CharacterFileJson as CharacterFileV0_1).Character[0].SpriteName;
        //            int activeSkinIndex = __instance._skinSlots.FindIndex(new Func<Image, bool>((s) => s.sprite.name == "weaponLevelFull"));
        //            Sprite sprite = SpriteImporter.LoadSprite(ch.FullSpritePath(ch.CharacterInfo.Skins[activeSkinIndex].SpriteName));
        //            __instance.Icon.sprite = sprite;
        //            __instance._Name.text = charData.GetFullNameUntranslated();
        //            __instance.Description.text = charData.description;
        //            __instance.StatsPanel.SetCharacter(charData, cType);
        //            __instance._EggCount.text = charData.exLevels.ToString();
        //            __instance.SetWeaponIconSprite(charData);
        //            __instance._selectedCharacter = character;
        //            //__instance.skin
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(CharacterItemUI))]
        class CharacterItemUI_Patch
        {
            [HarmonyPatch(nameof(CharacterItemUI.SetData))]
            [HarmonyPrefix]
            static bool SetData_Prefix(CharacterItemUI __instance, MethodBase __originalMethod, CharacterSelectionPage page, CharacterData dat, CharacterType cType, DataManager dataManager, PlayerOptions playerOptions)
            {
                if (Melon<BloodlinesMod>.Instance.manager.characters.Select((c) => c.CharacterType).Contains(cType))
                {
                    Character ch = Melon<BloodlinesMod>.Instance.manager.characters.Find(c => c.CharacterType == cType);
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
        }
    }
}