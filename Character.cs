using Il2CppSystem.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Objects;
using System;

namespace EasyAddCharacter
{
    public class Character
    {
        //public CharacterJson CharacterJson { get; private set; }

        public CharacterType CharacterType { get; set; }

        public Type CharacterFileVersion { get; private set; }

        public BaseCharacterFile CharacterFileJson { get; private set; }

        public CharacterJson CharacterInfo { get; private set; }

        public List<ModifierStats> ModifierStats { get; private set; }

        public string CharacterFilePath { get; private set; }


        public Character(string CharacterFilePath, BaseCharacterFile CharacterFileJson, Type CharacterFileVersion)
        {
            this.CharacterFilePath = CharacterFilePath;
            this.CharacterFileVersion = CharacterFileVersion;
            this.CharacterFileJson = CharacterFileJson;
            this.CharacterInfo = (CharacterFileJson as CharacterFileV0_1).Character[0];
            this.ModifierStats = (CharacterFileJson as CharacterFileV0_1).Character.Skip(1).ToList().Select(c => c.toModifierStat()).ToList();
        }
	}
}
