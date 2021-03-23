using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum CharacterQuality
    {
        /// <summary>
        /// 平民级
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 精英级
        /// </summary>
        Good = 2,
        /// <summary>
        /// 天才级
        /// </summary>
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
}