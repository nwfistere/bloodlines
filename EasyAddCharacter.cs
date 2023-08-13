using EasyAddCharacter.Textures;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System.Reflection;
using WNP78;
using Il2Generic = Il2CppSystem.Collections.Generic;

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
        static string CharacterJson = "[{\"level\":1,\"startingWeapon\":\"WHIP\",\"cooldown\":1,\"charName\":\"Antonio\",\"surname\":\"Belpaese\",\"textureName\":\"characters\",\"spriteName\":\"newAntonio_01.png\",\"skins\":[{\"id\":0,\"name\":\"Default\",\"textureName\":\"characters\",\"spriteName\":\"newAntonio_01.png\",\"walkingFrames\":4,\"unlocked\":true},{\"id\":1,\"name\":\"Legacy\",\"textureName\":\"characters\",\"spriteName\":\"Antonio_01.png\",\"walkingFrames\":4,\"unlocked\":true}],\"currentSkinIndex\":0,\"walkingFrames\":4,\"description\":\"Gains 10% more Damage every 10 levels (max +50%).\",\"isBought\":true,\"price\":0,\"completedStages\":[],\"survivedMinutes\":0,\"enemiesKilled\":0,\"stageData\":[],\"maxHp\":120,\"armor\":1,\"regen\":0,\"moveSpeed\":1,\"power\":1,\"area\":1,\"speed\":1,\"duration\":1,\"amount\":0,\"luck\":1,\"growth\":1,\"greed\":1,\"curse\":1,\"magnet\":0,\"revivals\":0,\"rerolls\":0,\"skips\":0,\"banish\":0,\"showcase\":[\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"SHADOWSERVANT\",\"AREA\",\"AREA\",\"AREA\",\"AREA\",\"AREA\",\"SPEED\",\"SPEED\",\"SPEED\",\"SPEED\",\"SPEED\",\"DURATION\",\"DURATION\",\"DURATION\",\"DURATION\",\"DURATION\",\"LUCK\",\"LUCK\",\"LUCK\",\"LUCK\",\"LUCK\",\"COOLDOWN\",\"COOLDOWN\",\"COOLDOWN\",\"COOLDOWN\",\"COOLDOWN\",\"AMOUNT\",\"AMOUNT\"]},{\"power\":0.1,\"level\":10},{\"power\":0.1,\"level\":20,\"growth\":1},{\"power\":0.1,\"level\":30},{\"power\":0.1,\"level\":40,\"growth\":1},{\"power\":0.1,\"level\":50},{\"level\":21,\"growth\":-1},{\"level\":41,\"growth\":-1}]";

        private static Skin createSkin()
        {
            return new()
            {
                id = 0,
                name = "Default",
                textureName = "characters",
                walkingFrames = 1,
                unlocked = true,
                spriteName = "floating-cat.png",
                walkFrameRate = new Il2CppSystem.Nullable<int>(0),
            };
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

        public static unsafe Il2CppSystem.Nullable<T> ToCpp<T>(T? value)
            where T : struct
        {
            IntPtr classPtr = Il2CppClassPointerStore<Il2CppSystem.Nullable<T>>.NativeClassPtr;
            var obj = IL2CPP.il2cpp_object_new(classPtr);
            var res = new Il2CppSystem.Nullable<T>(obj);
            res.hasValue = value.HasValue;
            if (value.HasValue)
            {
                res.value = value.Value;
            }

            return res;
        }

        static NullableStruct<int> zeroPadNullable = new(1);

        private static CharacterData createCharacterData()
        {
            CharacterData c = new()
            {
                skins = new(),
                level = 1,
                cooldown = 1,
                charName = "Nick",
                surname = "F",
                textureName = "characters",
                spriteName = "floating-cat.png",
                portraitName = "floating-cat.png",
                currentSkinIndex = 0,
                walkingFrames = 1,
                description = "Testing out adding a new character.",
                isBought = true,
                showcase = new(),
                price = 0,
                speed = 1,
                skips = 1,
                startingWeapon = new NullableStruct<WeaponType>(WeaponType.WHIP)
            };
            c.skins.Add(createSkin());
            c.walkFrameRate = c.skins.ToArray().First().walkFrameRate;
            return c;
        }

        internal static JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        internal static JsonSerializerSettings serializerSettingsSerialize = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        static DataManager dataManager;
        static GameManager gameManager;

        static Il2Generic.List<CharacterData> char_list = new();
        static CharacterType my_character_type;

        private static JArray ListToJArray(Il2Generic.List<CharacterData> list)
        {
            string result = JsonConvert.SerializeObject(list, serializerSettings);
            return JArray.Parse(result);
        }

        [HarmonyPatch(typeof(GameManager))]
        class DataManager_Patch
        {
            [HarmonyPatch(nameof(GameManager.Construct))]
            [HarmonyPrefix]
            static void Initialize_Prefix(GameManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                gameManager = __instance;
            }
        }

        [HarmonyPatch(typeof(DataManager))]
        class GameManager_Patch
        {
            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPrefix]
            static void Initialize_Prefix(DataManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                dataManager = __instance;
                my_character_type = (CharacterType)Enum.GetValues<CharacterType>().Max() + 1;
            }

            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPostfix]
            static void Initialize_Postfix(DataManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");

                //Il2Generic.List<CharacterData> char_list = new();
                
                // char_list.Add(Mod.createCharacterData());
                //__instance.AllCharacters.Add(my_character_type, ListToJArray(char_list));
                //__instance.AllCharactersJson.Add("NICK", JsonConvert.SerializeObject(char_list, serializerSettings));
            }

            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPostfix]
            static void LoadBaseJObjects_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                JArray json = JArray.Parse(CharacterJson);
                __instance.AllCharactersJson.Add(my_character_type.ToString() ,json);
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
                //foreach (Il2Generic.KeyValuePair<CharacterType, Il2Generic.List<CharacterData>> item in __result)
                //    Melon<Mod>.Logger.Msg($"{item.Key} - {item.Value}");

                if (CharacterSelectionPageConstructed && !__result.ContainsKey(my_character_type))
                {
                    __result.Add(my_character_type, char_list);
                    Melon<Mod>.Logger.Msg($"Added {my_character_type}");
                }
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


        static private bool CharacterSelectionPageConstructed = false;

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
                // TODO: Move this to dataManager object.
                if (!dataManager.AllCharacters.ContainsKey(my_character_type))
                {
                    dataManager.AllCharacters.Add(my_character_type, ListToJArray(char_list));
                    dataManager.AllCharactersJson.Add(my_character_type.ToString(), JsonConvert.SerializeObject(char_list, serializerSettings));
                }
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
                if (cType.ToString() == my_character_type.ToString())
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
                CharacterSelectionPageConstructed = true;
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
            static void ShowCharacterInfo_Prefix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
                if (cType == my_character_type)
                {
                    __instance.Icon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                }
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.ShowCharacterInfo))]
            [HarmonyPostfix]
            static void ShowCharacterInfo_Postfix(CharacterSelectionPage __instance, CharacterData charData, CharacterType cType, CharacterItemUI character, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
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
                if (__instance._CharacterName.text == "Nick")
                {
                    Melon<Mod>.Logger.Msg($"Setting info panel for NICK");
                    //__instance._CharacterIcon
                }
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
                if (cType == my_character_type /*|| cType == my_character_type - 1*/)
                {
                    //Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
                    //GameObject character = page._spawned[^1].Cast<GameObject>();
                    //character.name = dat.GetFullNameUntranslated();

                    //__instance.name = character.name;
                    //Button button = character.GetComponent<Button>();
                    //// Set sprite
                    //Image characterIcon = character.transform.GetComponentsInChildren<Image>().Where(i => i.name == "CharacterIcon").First();
                    //characterIcon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                    //characterIcon.sprite.texture.ReinitializeWithFormatImpl(characterIcon.sprite.texture.width, characterIcon.sprite.texture.height, GraphicsFormat.R32G32B32A32_UInt, false);
                    //characterIcon.sprite.texture.filterMode = FilterMode.Point;

                    //character.transform.GetComponentInChildren<TextMeshProUGUI>().text = dat.charName;

                    //__instance._page = page;
                    //__instance._playerOptions = dataManager._playerOptions;
                    //__instance._data = dat;
                    //__instance._dataManager = dataManager;
                    //__instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                    //__instance._CharacterIcon.sprite.name = __instance.name;
                    //__instance._CharacterIcon.sprite.texture.name = __instance.name;
                    //__instance._defaultTextColor = new Color(1, 1, 1, 1);
                    //__instance._nameText = __instance._CharacterName;
                    //__instance.Type = my_character_type;
                }
                return true;
            }

            [HarmonyPatch(nameof(CharacterItemUI.SetData))]
            [HarmonyPostfix]
            static void SetData_Postfix(CharacterItemUI __instance, object[] __args, MethodBase __originalMethod)
            {
                if (__args[2] != null && __args[2].ToString() == my_character_type.ToString())
                    Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
            }
        }
    }
}