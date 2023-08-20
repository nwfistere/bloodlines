using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text.Json;

namespace Bloodlines
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CharacterJsonModelv0_2
    {
        [JsonProperty("startingWeapon")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponType StartingWeapon { get; set; }

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

        [JsonProperty("skins")]
        public List<SkinObjectModel> Skins { get; set; }

        [JsonProperty("currentSkinIndex")]
        public int CurrentSkinIndex { get; set; }

        [JsonProperty("walkingFrames")]
        public int WalkingFrames { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isBought")]
        public bool IsBought { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("showcase", ItemConverterType = typeof(StringEnumConverter))]
        public List<WeaponType> Showcase { get; set; }

        [JsonProperty("statModifiers")]
        public List<StatModifierJsonModelv0_2> StatModifiers { get; set; }

        [JsonProperty("onEveryLevelUp")]
        public StatModifierJsonModelv0_2 OnEveryLevelUp { get; set; }

        [JsonProperty("bodyOffset")]
        public Vector2 BodyOffset { get; set; }

        [JsonProperty("portraitName")]
        public string PortraitName { get; set; }

        [JsonProperty("bgm")]
        public BgmType BGM { get; set; }

        public string toCharacterDataJson()
        {
            CharacterData c = new();

            StatModifierJsonModelv0_2 stats = StatModifiers[0];

            PropertyInfo[] statsProps = stats.GetType().GetProperties();
            foreach (PropertyInfo prop in statsProps)
            {
                if (c.GetType().GetProperty(prop.Name) == null)
                    continue;
                var value = prop.GetValue(stats, null);
                c.GetType().GetProperty(prop.Name).SetValue(c, value);
            }

            PropertyInfo[] myProps = GetType().GetProperties();
            foreach (PropertyInfo prop in myProps)
            {
                if (c.GetType().GetProperty(prop.Name) == null)
                    continue;
                var value = prop.GetValue(this, null);
                c.GetType().GetProperty(prop.Name).SetValue(c, value);
            }


            return JsonConvert.SerializeObject(c);
        }
    }

    public class StatModifierJsonModelv0_2
    {
        [JsonProperty("level")]
        public float Level { get; set; }

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

        [JsonProperty("curse")]
        public float Curse { get; set; }

        [JsonProperty("magnet")]
        public float Magnet { get; set; }

        [JsonProperty("revivals")]
        public float Revivals { get; set; }

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

        [JsonProperty("shields")]
        public float Shields { get; set; }

        [JsonProperty("cooldown")]
        public float Cooldown { get; set; }

        public ModifierStats toModifierStat()
        {
            ModifierStats m = new();

            PropertyInfo[] myProps = GetType().GetProperties();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (m.GetType().GetProperty(prop.Name) == null)
                    continue;
                var value = prop.GetValue(this, null);
                m.GetType().GetProperty(prop.Name).SetValue(m, value);
            }

            return m;
        }
    }

    public class SkinObjectModelv0_2
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("textureName")]
        public string TextureName { get; set; }

        [JsonProperty("spriteName")]
        public string SpriteName { get; set; }

        [JsonProperty("walkingFrames")]
        public int WalkingFrames { get; set; }

        [JsonProperty("unlocked")]
        public bool Unlocked { get; set; }

        public static explicit operator Skin(SkinObjectModelv0_2 model)
        {
            Skin skin = new Skin();

            skin.id = (SkinType)model.Id;
            skin.name = model.Name;
            skin.textureName = model.TextureName;
            skin.spriteName = model.SpriteName;
            skin.walkingFrames = model.WalkingFrames;
            skin.unlocked = model.Unlocked;

            return skin;
        }
    }
}
