using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum CharacterQuality
    {
        /// <summary>
        /// ƽ��
        /// </summary>
        Normal = 1,
        /// <summary>
        /// ��Ӣ��
        /// </summary>
        Good = 2,
        /// <summary>
        /// ��ż�
        /// </summary>
        Perfect = 3,
        /// <summary>
        /// Ӣ�ۼ�
        /// </summary>
        Hero = 4,
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

    public class CharacterDefine
    {
        public const int Max_Head = 6;
        public const int Max_Body = 5;
    }
}