using Il2CppVampireSurvivors.Data;
using System;

namespace EasyAddCharacter
{
    public class Character
    {
        public CharacterJson CharacterJson { get; private set; }

        public CharacterType CharacterType { get; private set; }


        Character(CharacterType CharacterType, CharacterJson CharacterJson)
        {
            this.CharacterType = CharacterType;
            this.CharacterJson = CharacterJson;
        }
	}
}
