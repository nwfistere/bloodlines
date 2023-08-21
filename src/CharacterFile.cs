using Bloodlines.src;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

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
            throw new Exception("Unimplemented...");
        }
    }

    // Mark with [Obsolete("CharacterFileV* is deprecated, use CharacterFileV* instead.")] when I add a new version.
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