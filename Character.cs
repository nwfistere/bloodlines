using Il2CppVampireSurvivors.Data;
using System;

namespace EasyAddCharacter
{
    internal class Character
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
