using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using UnityEngine;

namespace Bloodlines.src
{
    // This is meant to be a json serializable copy of Vampire Survivors's Character data.
    // VampireSurvivors CharacterData has nullables, which make it hard to do anything. Copy the class into this.
    public class CharacterDataModel
    {
        [JsonProperty("hidden")]
        public bool hidden;


        [JsonProperty("level")]
        public int level;


        [JsonProperty("startingWeapon")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponType startingWeapon;


        [JsonProperty("cooldown")]
        public float cooldown;


        [JsonProperty("prefix")]
        public string prefix;


        [JsonProperty("charName")]
        public string charName;


        [JsonProperty("surname")]
        public string surname;


        [JsonProperty("textureName")]
        public string textureName;


        [JsonProperty("spriteName")]
        public string spriteName;


        [JsonProperty("portraitName")]
        public string portraitName;


        [JsonProperty("walkingFrames")]
        public int walkingFrames;


        [JsonProperty("skins")]
        public List<Skin> skins;


        [JsonProperty("spriteAnims")]
        public SpriteAnims spriteAnims;


        [JsonProperty("walkFrameRate")]
        public int walkFrameRate;


        [JsonProperty("description")]
        public string description;


        [JsonProperty("isBought")]
        public bool isBought;


        [JsonProperty("price")]
        public float price;


        [JsonProperty("maxHp")]
        public float maxHp;


        [JsonProperty("armor")]
        public float armor;


        [JsonProperty("regen")]
        public float regen;


        [JsonProperty("moveSpeed")]
        public float moveSpeed;


        [JsonProperty("power")]
        public double power;


        [JsonProperty("area")]
        public float area;


        [JsonProperty("speed")]
        public float speed;


        [JsonProperty("duration")]
        public float duration;


        [JsonProperty("amount")]
        public float amount;


        [JsonProperty("luck")]
        public float luck;


        [JsonProperty("growth")]
        public float growth;


        [JsonProperty("greed")]
        public float greed;


        [JsonProperty("magnet")]
        public float magnet;


        [JsonProperty("revivals")]
        public float revivals;


        [JsonProperty("curse")]
        public float curse;


        [JsonProperty("shields")]
        public float shields;


        [JsonProperty("reRolls")]
        public float reRolls;


        [JsonProperty("skips")]
        public float skips;


        [JsonProperty("banish")]
        public float banish;


        [JsonProperty("showcase")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<WeaponType> showcase;


        [JsonProperty("debugTime")]
        public float debugTime;


        [JsonProperty("debugEnemies")]
        public float debugEnemies;


        [JsonProperty("bgm")]
        public string bgm;


        [JsonProperty("startFrameCount")]
        public int startFrameCount;


        [JsonProperty("zeroPad")]
        public int zeroPad;


        [JsonProperty("suffix")]
        public string suffix;


        [JsonProperty("frameRate")]
        public int frameRate;


        [JsonProperty("sineSpeed")]
        public SineBonusData sineSpeed;


        [JsonProperty("sineCooldown")]
        public SineBonusData sineCooldown;


        [JsonProperty("sineArea")]
        public SineBonusData sineArea;


        [JsonProperty("sineDuration")]
        public SineBonusData sineDuration;


        [JsonProperty("sineMight")]
        public SineBonusData sineMight;


        [JsonProperty("noHurt")]
        public bool noHurt;


        [JsonProperty("exLevels")]
        public int exLevels;


        [JsonProperty("exWeapons")]
        public List<string> exWeapons;


        [JsonProperty("hiddenWeapons")]
        public List<string> hiddenWeapons;


        [JsonProperty("onEveryLevelUp")]
        public ModifierStats onEveryLevelUp;


        [JsonProperty("bodyOffset")]
        public Vector2 bodyOffset;


        [JsonProperty("nameIndex")]
        public int nameIndex;


        [JsonProperty("currentSkinIndex")]
        public int currentSkinIndex;
    }
}
