using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public enum CharacterQuality
	{
        Normal = 1,
        Good = 2,
        Perfect = 3,
        //Excellent = 4,
	}

    public enum UnlockContent
    {
        None = 0,
        LearnKongfu,
        EquipArmor,
        EquipWeapon,
    }

    public enum CharacterCamp
    {
        OurCamp,
        EnemyCamp,
    }
    public enum CharacterBehavior
    {
        Free,
        Working,
    }
}