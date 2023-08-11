using EasyAddCharacter.Textures;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppTMPro;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using WNP78;
using static Il2CppSystem.Linq.Expressions.Interpreter.NullableMethodCallInstruction;
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
                //startingWeapon = new Il2CppSystem.Nullable<WeaponType>(WeaponType.WHIP),
                startingWeapon = new NullableStruct<WeaponType>(WeaponType.WHIP),
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
                //zeroPad = zeroPadNullable,
                bodyOffset = new NullableStruct<Vector2>(new Vector2(0.5f, 0.5f)),
                showcase = new(),
                price = 0,
                speed = 1,
                skips = 1,
            };
            //c.startFrameCount = new NullableStruct<int>(0);
            c.zeroPad = zeroPadNullable;
            NullableStruct<WeaponType> nullable = new NullableStruct<WeaponType>(WeaponType.WHIP);
            c.skins.Add(createSkin());
            c.walkFrameRate = c.skins.ToArray().First().walkFrameRate;

            //c.startingWeapon = ToCpp<WeaponType>(weaponType);
            ////c.startingWeapon = nullable;
            //c.startingWeapon = Il2CppSystem.Nullable<WeaponType>.UnboxExact(Il2CppSystem.Enum.ToObject(Il2CppType.Of<WeaponType>(), (int)WeaponType.WHIP));
            //c.startingWeapon = nullable;
            //WeaponType someWeaponType = (WeaponType)Il2CppSystem.Enum.ToUInt64(Il2CppSystem.Nullable<WeaponType>.Box(c.startingWeapon)); 

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

        //[HarmonyPatch(typeof(Il2CppSystem.Nullable))]
        //class Nullable_Patch
        //{
        //    [HarmonyPatch(nameof(Il2CppSystem.Nullable<WeaponType>.Value), MethodType.Getter)]
        //    [HarmonyPostfix]
        //    public static WeaponType Value_Postfix(Il2CppSystem.Nullable<WeaponType> __instance) => __instance.Value;
        //}

        [HarmonyPatch]
        static class Nullable_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                return typeof(Il2CppSystem.Nullable<WeaponType>).GetMethods()
                    .Where(m => m.Name == "get_value").Cast<MethodBase>().ToList();
            }

            static void Prefix(Resources __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"Resources.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
            }

            static void Postfix(Resources __instance, object[] __args, MethodBase __originalMethod, dynamic __result)
            {
                Melon<Mod>.Logger.Msg($"Resources.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
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
            }



            [HarmonyPatch(nameof(DataManager.Initialize))]
            [HarmonyPostfix]
            static void Initialize_Postfix(DataManager __instance)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");

                //Il2Generic.List<CharacterData> char_list = new();
                my_character_type = (CharacterType)Enum.GetValues<CharacterType>().Max() + 1;
                char_list.Add(Mod.createCharacterData());
                //__instance.AllCharacters.Add(my_character_type, ListToJArray(char_list));
                //__instance.AllCharactersJson.Add("NICK", JsonConvert.SerializeObject(char_list, serializerSettings));
            }

            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPostfix]
            static void MergeInJsonData_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPrefix]
            static void MergeInJsonData_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadAndMergeIn))]
            [HarmonyPostfix]
            static void LoadAndMergeIn_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(DataManager.LoadAndMergeIn))]
            [HarmonyPrefix]
            static void LoadAndMergeIn_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            // Add our characters to the result of this I believe!
            [HarmonyPatch(nameof(DataManager.GetConvertedCharacterData))]
            [HarmonyPostfix]
            static void GetConvertedCharacterData_Postfix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod,
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
            static void GetConvertedCharacterData_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
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

            [HarmonyPatch(nameof(CharacterSelectionPage.CharCodeToString))]
            [HarmonyPrefix]
            static void CharCodeToString_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"{MethodBase.GetCurrentMethod()?.Name}");
            }

            [HarmonyPatch(nameof(CharacterSelectionPage.AddCharacter))]
            [HarmonyPrefix]
            static void AddCharacter_Prefix(CharacterSelectionPage __instance, object[] __args, MethodBase __originalMethod)
            {
                if (__args[1] != null && __args[1].ToString() == my_character_type.ToString())
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
                // Looks like we need a on click as well on the button?
             
                // Default sprite antonio
                // Default name Pasqualina
                // Default weapon garlic
                if (cType == my_character_type /*|| cType == my_character_type - 1*/)
                {
                    Melon<Mod>.Logger.Msg($"CharacterItemUI.{MethodBase.GetCurrentMethod()?.Name}");
                    GameObject character = page._spawned[^1].Cast<GameObject>();
                    //RectTransform rectTransform = character.GetComponent<RectTransform>();
                    //CanvasRenderer canvasRenderer = character.GetComponent<CanvasRenderer>();
                    //Image image = character.GetComponent<Image>();
                    //CharacterItemUI characterItem = character.GetComponent<CharacterItemUI>(); // this object
                    __instance.name = character.name;
                    Button button = character.GetComponent<Button>();
                    character.name = dat.GetFullNameUntranslated();
                    Image characterIcon = character.transform.GetComponentsInChildren<Image>().Where(i => i.name == "CharacterIcon").First();
                    characterIcon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                    characterIcon.sprite.texture.ReinitializeWithFormatImpl(characterIcon.sprite.texture.width, characterIcon.sprite.texture.height, GraphicsFormat.R32G32B32A32_UInt, false);
                    characterIcon.sprite.texture.filterMode = FilterMode.Point;

                    //page._skinSlots.Add(characterIcon);
                    //button.onClick = page._spawned[^2].Cast<GameObject>().GetComponent<Button>().onClick;

                    character.transform.GetComponentInChildren<TextMeshProUGUI>().text = dat.charName;
                    //RectTransform infoPanel = page.transform.GetComponentsInChildren<Component>().ToList().Find(c => c.name == "InfoPanel").Cast<RectTransform>();
                    //Il2CppArrayBase<Component> comps = infoPanel.transform.GetComponentsInChildren<Component>();
                    //TextMeshProUGUI name = infoPanel.transform.GetComponentsInChildren<TextMeshProUGUI>().Where(text => text.name == "Name").First();

                    __instance._page = page;
                    __instance._playerOptions = dataManager._playerOptions;
                    __instance._data = dat;
                    __instance._dataManager = dataManager;
                    //page.Icon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                    __instance._CharacterIcon.sprite = SpriteImporter.LoadSprite(Path.Combine(Directory.GetCurrentDirectory(), "resources", "floating-cat.png"));
                    __instance._CharacterIcon.sprite.name = __instance.name;
                    __instance._CharacterIcon.sprite.texture.name = __instance.name;

                    __instance._defaultTextColor = new Color(1, 1, 1, 1);
                    __instance._nameText = __instance._CharacterName;

                    //button.onClick.AddListener(new Action(() => page.ShowCharacterInfo(dat, cType, __instance)));

                    __instance.Type = my_character_type;


                    return false;
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

        [HarmonyPatch]
        static class AssetBundle_Patch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                List<string> DoNotGrab = new() { "GetType", "ToString", "Equals", "GetHashCode", "GetCachedPtr" };
                //return AccessTools.GetTypesFromAssembly(AppDomain.CurrentDomain.GetAssemblies()
                //    .SingleOrDefault(assembly => assembly.GetName().Name == "UnityEngine.AssetBundleModule"))
                //    .Where(type => type == typeof(AssetBundle))
                //    .SelectMany(type => type.GetMethods())
                //    .Where(method => method.IsGenericMethod == false && method.DeclaringType == typeof(AssetBundle))
                //    .Cast<MethodBase>();
                List<MethodBase> list =  typeof(AssetBundle)
                    .GetMethods()
                    .Where(m => m.DeclaringType == typeof(AssetBundle))
                    .Where(method => method.IsGenericMethod == false).Cast<MethodBase>().ToList();

                //list.Add(typeof(AssetBundle).GetMethod(nameof(AssetBundle.LoadAsset).MakeGenericMethod()))
                //list.Add(AccessTools.Method(typeof(AssetBundle), nameof(AssetBundle.Load), new Type[] { typeof(string) }, new Type[] { typeof(Texture2D) }).MakeGenericMethod(typeof(Texture2D)));

                return list;
            }

            static void Prefix(AssetBundle __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"AssetBundle.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
            }

            static void Postfix(AssetBundle __instance, object[] __args, MethodBase __originalMethod)
            {
                Melon<Mod>.Logger.Msg($"AssetBundle.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
            }
        }

        //[HarmonyPatch]
        //static class Resources_Patch
        //{
        //    static IEnumerable<MethodBase> TargetMethods()
        //    {
        //        List<MethodBase> list = new() { typeof(Resources).GetMethods().Where(m => m.Name == nameof(Resources.Load) && m.IsGenericMethod).First().MakeGenericMethod(typeof(UnityEngine.Object)) };

        //        list.Add(typeof(Resources).GetMethods().Where(m => m.Name == nameof(Resources.LoadAll) && !m.IsGenericMethod).First());
        //        list.Add(typeof(Resources).GetMethods().Where(m => m.Name == nameof(Resources.FindObjectsOfTypeAll) && !m.IsGenericMethod).First());

        //        return list;
        //    }

        //    static void Prefix(Resources __instance, object[] __args, MethodBase __originalMethod)
        //    {
        //        Melon<Mod>.Logger.Msg($"Resources.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
        //    }

        //    static void Postfix(Resources __instance, object[] __args, MethodBase __originalMethod, dynamic __result)
        //    {
        //        Melon<Mod>.Logger.Msg($"Resources.{MethodBase.GetCurrentMethod()?.Name} - {__originalMethod.Name}");
        //    }
        //}

    }
}