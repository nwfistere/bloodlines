using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAddCharacter
{
    // The Deserialized version of the Character json files.
    public abstract class BaseCharacterFile
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(VersionConverter))]
        abstract public Version Version { get; set; }
    }

    // Mark with [Obsolete("CharacterFileV* is deprecated, use CharacterFileV* instead.")] when new version is here.
    public class CharacterFileV0_1 : BaseCharacterFile
    {
        [JsonIgnore]
        public const string _version = "0.1";

        public override Version Version { get; set; } = new Version("0.1");

        [JsonProperty("character")]
        public List<CharacterJson> Character { get; set; }

        public CharacterFileV0_1() : base() {}
    }
}
