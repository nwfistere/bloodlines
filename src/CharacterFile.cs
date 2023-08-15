using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bloodlines
{
    // The Deserialized version of the Character json files.
    public abstract class BaseCharacterFile
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(VersionConverter))]
        abstract public Version Version { get; set; }

        // Defined in case we move the character json somewhere.
        abstract public List<CharacterJsonModel> GetCharacterJson();
    }

    // Mark with [Obsolete("CharacterFileV* is deprecated, use CharacterFileV* instead.")] when I add a new version.
    public class CharacterFileV0_1 : BaseCharacterFile
    {
        [JsonIgnore]
        public const string _version = "0.1";

        public override Version Version { get; set; } = new Version("0.1");

        [JsonProperty("character")]
        public List<CharacterJsonModel> Character { get; set; }

        public CharacterFileV0_1() : base() { }

        public override List<CharacterJsonModel> GetCharacterJson()
        {
            return Character;
        }
    }
}
