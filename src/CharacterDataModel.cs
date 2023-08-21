using Bloodlines.src.json;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Bloodlines.src
{
    // This is meant to be a json serializable copy of Vampire Survivors's Character data.
    // VampireSurvivors CharacterData has nullables, which make it hard to do anything. Copy the class into this.

    public class CharacterDataModelWrapper
    {
        [JsonIgnore]
        public CharacterType characterType { get; set; }

        public List<CharacterDataModel> CharacterSettings { get; set; } = new();

        [JsonIgnore]
        public string BaseDirectory { get; set; }

        public string SpritePath
        {
            get
            {
                return Path.Combine(BaseDirectory, Character.SpriteName);
            }
        }

        public string PortraitPath
        {
            get
            {
                return Path.Combine(BaseDirectory, Character.PortraitName);
            }
        }

        public string SkinPath(int skinId) => Path.Combine(BaseDirectory, Character.Skins[skinId].SpriteName);

        public SkinObjectModelv0_2 Skin(int skinId) => Character.Skins[skinId];

        [JsonIgnore]
        public CharacterDataModel Character { get
            {
                if (CharacterSettings.Any())
                {
                    return CharacterSettings[0];
                } else
                {
                    throw new System.Exception("Characters hasn't been set yet.");
                }
            }
        }
    }

    public class CharacterDataModel
    {
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }


        [JsonProperty("level")]
        public float Level { get; set; }


        [JsonProperty("startingWeapon")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponType StartingWeapon { get; set; }


        [JsonProperty("cooldown")]
        public float Cooldown { get; set; }


        [JsonProperty("prefix")]
        public string Prefix { get; set; }


        [JsonProperty("charName")]
        public string CharName { get; set; }


        [JsonProperty("surname")]
        public string Surname { get; set; }


        [JsonProperty("textureName")]
        public string TextureName { get; set; }


        [JsonProperty("spriteName")]
        public string SpriteName { get; set; }


        [JsonProperty("portraitName")]
        public string PortraitName { get; set; }


        [JsonProperty("walkingFrames")]
        public int WalkingFrames { get; set; }


        [JsonProperty("skins")]
        public List<SkinObjectModelv0_2> Skins { get; set; }


        [JsonProperty("spriteAnims")]
        public SpriteAnims SpriteAnims { get; set; }


        [JsonProperty("walkFrameRate")]
        public int WalkFrameRate { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("isBought")]
        public bool IsBought { get; set; }


        [JsonProperty("price")]
        public float Price { get; set; }


        [JsonProperty("maxHp")]
        public float MaxHp { get; set; }


        [JsonProperty("armor")]
        public float Armor { get; set; }


        [JsonProperty("regen")]
        public float Regen { get; set; }


        [JsonProperty("moveSpeed")]
        public float MoveSpeed { get; set; }


        [JsonProperty("power")]
        public double Power { get; set; }


        [JsonProperty("area")]
        public float Area { get; set; }


        [JsonProperty("speed")]
        public float Speed { get; set; }


        [JsonProperty("duration")]
        public float Duration { get; set; }


        [JsonProperty("amount")]
        public float Amount { get; set; }


        [JsonProperty("luck")]
        public float Luck { get; set; }


        [JsonProperty("growth")]
        public float Growth { get; set; }


        [JsonProperty("greed")]
        public float Greed { get; set; }


        [JsonProperty("magnet")]
        public float Magnet { get; set; }


        [JsonProperty("revivals")]
        public float Revivals { get; set; }


        [JsonProperty("curse")]
        public float Curse { get; set; }


        [JsonProperty("shields")]
        public float Shields { get; set; }


        [JsonProperty("rerolls")]
        public float Rerolls { get; set; }


        [JsonProperty("skips")]
        public float Skips { get; set; }


        [JsonProperty("banish")]
        public float Banish { get; set; }

        [JsonProperty("charm")]
        public float Charm { get; set; }

        [JsonProperty("shroud")]
        public float Shroud { get; set; }


        [JsonProperty("showcase", ItemConverterType = typeof(StringEnumConverter))]
        public List<WeaponType> Showcase { get; set; }


        [JsonProperty("debugTime")]
        public float DebugTime { get; set; }


        [JsonProperty("debugEnemies")]
        public float DebugEnemies { get; set; }


        [JsonProperty("bgm")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BgmType BGM { get; set; }


        [JsonProperty("startFrameCount")]
        public int StartFrameCount { get; set; }


        [JsonProperty("zeroPad")]
        public int ZeroPad { get; set; }


        [JsonProperty("suffix")]
        public string Suffix { get; set; }


        [JsonProperty("frameRate")]
        public int FrameRate { get; set; }


        [JsonProperty("sineSpeed")]
        public SineBonusData SineSpeed { get; set; }


        [JsonProperty("sineCooldown")]
        public SineBonusData SineCooldown { get; set; }


        [JsonProperty("sineArea")]
        public SineBonusData SineArea { get; set; }


        [JsonProperty("sineDuration")]
        public SineBonusData SineDuration { get; set; }


        [JsonProperty("sineMight")]
        public SineBonusData SineMight { get; set; }


        [JsonProperty("noHurt")]
        public bool NoHurt { get; set; }


        [JsonProperty("exLevels")]
        public int ExLevels { get; set; }


        [JsonProperty("exWeapons")]
        public List<string> ExWeapons { get; set; }


        [JsonProperty("hiddenWeapons")]
        public List<string> HiddenWeapons { get; set; }


        [JsonProperty("onEveryLevelUp")]
        public ModifierStats OnEveryLevelUp { get; set; }


        [JsonProperty("bodyOffset")]
        //[JsonConverter(typeof(Vector2JsonConverter))] I doubt Vampire survivors reads in like this, so don't do this.
        public Vector2 BodyOffset { get; set; }


        [JsonProperty("nameIndex")]
        public int NameIndex { get; set; }


        [JsonProperty("currentSkinIndex")]
        public int CurrentSkinIndex { get; set; }

        [JsonIgnore]
        public CharacterType CharacterType { get; set; }
    }
}
