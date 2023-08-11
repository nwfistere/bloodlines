using Il2CppVampireSurvivors.Data.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using Il2CppVampireSurvivors.Data;

namespace EasyAddCharacter
{
    [RegisterTypeInIl2Cpp]

    public class CustomCharacterData : CharacterData
    {
        public CustomCharacterData(IntPtr ptr) : base(ptr) { }

        public CustomCharacterData() : base(ClassInjector.DerivedConstructorPointer<CustomCharacterData>()) => ClassInjector.DerivedConstructorBody(this);

        public WeaponType __starting_weapon;

        public new WeaponType? startingWeapon
        {
            get
            {
                return __starting_weapon;
            }
            set
            {
                __starting_weapon = value ?? 0;
            }
        }

    }
}
