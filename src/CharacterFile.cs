using Bloodlines.src;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bloodlines
{
    // The Deserialized version of the Character json files.
    public abstract class BaseCharacterFileModel
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(VersionConverter))]
        abstract public Version Version { get; set; }

        public abstract Type CharacterFileVersion();

        public abstract List<CharacterDataModelWrapper> GetCharacterList();
    }

    [Obsolete("CharacterFileModelV0_1 is deprecated, useCharacterFileModelV0_2 instead.")]
    public class CharacterFileModelV0_1 : BaseCharacterFileModel
    {
        [JsonIgnore]
        public const string _version = "0.1";

        public override Version Version { get; set; } = new Version("0.1");

        [JsonProperty("character")]
        public List<CharacterJsonModelv0_1> Character { get; set; }

        public CharacterFileModelV0_1() : base() { }

        public override Type CharacterFileVersion() => typeof(CharacterFileModelV0_1);

        public override List<CharacterDataModelWrapper> GetCharacterList()
        {
            List<CharacterDataModelWrapper> characterDatas = new();
            bool first = true;

            CharacterDataModelWrapper wrapper = new();

            foreach (CharacterJsonModelv0_1 old in Character)
            {
                CharacterDataModel model = new();

                PropertyInfo[] myProps = old.GetType().GetProperties();

                foreach (PropertyInfo prop in myProps)
                {
                    if (model.GetType().GetProperty(prop.Name) == null)
                    {
                        Melon<BloodlinesMod>.Logger.Msg($"No match for {prop.Name}");
                        continue;
                    }

                    try
                    {
                        var value = prop.GetValue(old, null);

                        if (prop.Name == "Skins" && old.Skins != null)
                        {
                            model.Skins = new();

                            foreach (SkinObjectModel os in old.Skins)
                            {
                                SkinObjectModelv0_2 ns = new()
                                {
                                    Id = (Il2CppVampireSurvivors.Data.SkinType)os.Id,
                                    Name = os.Name,
                                    SpriteName = os.SpriteName,
                                    TextureName = os.TextureName,
                                    Unlocked = os.Unlocked,
                                    frames = new()
                                };
                            }
                        }
                        else
                        {
                            model.GetType().GetProperty(prop.Name).SetValue(model, value);
                        }
                    }
                    catch (Exception e)
                    {
                        Melon<BloodlinesMod>.Logger
                            .Msg($"Failed to convert: {prop.Name} From {prop.PropertyType.FullName} to {model.GetType().GetProperty(prop.Name).PropertyType.FullName} on character: {old.CharName}");

                        Melon<BloodlinesMod>.Logger.Msg($"{e}");
                    }
                }

                if (first)
                {
                    model.PortraitName ??= model.SpriteName;
                    model.WalkingFrames = 1;

                    if (!model.Skins.Any())
                    {
                        SkinObjectModelv0_2 skin = new();
                        skin.Id = 0;
                        skin.Name = "Default";
                        skin.SpriteName = model.SpriteName;
                        skin.TextureName = "characters";
                        skin.Unlocked = true;
                        skin.frames = new();

                        model.Skins.Add(skin);
                    }

                    first = false;
                }

                wrapper.CharacterSettings.Add(model);
            }

            characterDatas.Add(wrapper);

            return characterDatas;
        }
    }

    // Mark with [Obsolete("CharacterFileModelV0_2 is deprecated, use CharacterFileV* instead.")] when I add a new version.
    public class CharacterFileModelV0_2 : BaseCharacterFileModel
    {
        [JsonIgnore]
        public const string _version = "0.2";

        public override Version Version { get; set; } = new Version("0.2");

        [JsonProperty("characters")]
        public List<CharacterJsonModelv0_2> Characters { get; set; }

        [JsonIgnore]
        public string CharacterBaseDir { get; set; }

        public CharacterFileModelV0_2() : base() { }

        public override Type CharacterFileVersion() => typeof(CharacterFileModelV0_2);

        public override List<CharacterDataModelWrapper> GetCharacterList()
        {
            List<CharacterDataModelWrapper> characterDatas = new();

            Characters.ForEach((c) => characterDatas.Add(c.toCharacterDataModel()));

            return characterDatas;
        }
    }
}