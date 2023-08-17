using Il2CppSystem.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Objects;

namespace Bloodlines
{
    public class Character
    {
        //public CharacterJson CharacterJson { get; private set; }

        public CharacterType CharacterType { get; set; }

        public Type CharacterFileVersion { get; private set; }

        public BaseCharacterFile CharacterFileJson { get; private set; }

        public CharacterJsonModel CharacterInfo { get; private set; }

        public List<ModifierStats> ModifierStats { get; private set; }

        public string CharacterFilePath { get; private set; }

        public string CharacterBaseDir { get; private set; }


        public Character(string CharacterFilePath, BaseCharacterFile CharacterFileJson, Type CharacterFileVersion)
        {
            this.CharacterFilePath = CharacterFilePath;
            this.CharacterBaseDir = Path.GetDirectoryName(CharacterFilePath);
            this.CharacterFileVersion = CharacterFileVersion;
            this.CharacterFileJson = CharacterFileJson;
            CharacterInfo = (CharacterFileJson as CharacterFileV0_1).Character[0];
            ModifierStats = (CharacterFileJson as CharacterFileV0_1).Character.Skip(1).ToList().Select(c => c.toModifierStat()).ToList();
        }

        public string FullSpritePath(string spriteName)
        {
            return Path.Combine(CharacterBaseDir, spriteName);
        }
    }
}
