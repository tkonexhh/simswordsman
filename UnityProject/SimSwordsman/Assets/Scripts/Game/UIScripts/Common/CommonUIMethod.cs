using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CommonUIMethod
    {
        /// <summary>
        /// 根据帮派类型获取帮派名称
        /// </summary>
        /// <param name="clanType"></param>
        /// <returns></returns>
        public static string GetClanName(ClanType clanType)
        {
            switch (clanType)
            {
                case ClanType.GaiBang:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.GaiBang));
                case ClanType.ShaoLin:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.ShaoLin));
                case ClanType.WuDang:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.WuDang));
                case ClanType.EMei:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.EMei));
                case ClanType.HuaShan:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.HuaShan));
                case ClanType.WuDu:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.WuDu));
                case ClanType.MoJiao:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.MoJiao));
                case ClanType.XiaoYao:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.XiaoYao));
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取挑战名称
        /// </summary>
        /// <returns></returns>
        public static string GetChallengeTitle(ChapterConfigInfo chapterConfig, int level)
        {
            return TDLanguageTable.Get(Define.CHALLENGE_NAME) + ":"
                + chapterConfig.chapterId.ToString() + "-"
                + level.ToString();
        }

        /// <summary>
        /// 设置门派前缀
        /// </summary>
        /// <param name="clanType">门派类型</param>
        /// <returns></returns>
        public static string SetClanPrefix(ClanType clanType)
        {
            return "ClanType_" + clanType.ToString();
        }

        /// <summary>
        /// 根据表中的Key，获取相应的内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringForTableKey(string key)
        {
            return TDLanguageTable.Get(key);
        }

        /// <summary>
        /// 根据弟子品质获取相应的str
        /// </summary>
        /// <param name="characterQuality"></param>
        /// <returns></returns>
        public static string GetStrQualityForChaQua(CharacterQuality characterQuality)
        {
            switch (characterQuality)
            {
                case CharacterQuality.Normal:
                     return TDLanguageTable.Get(Define.DISCIPLE_QUALITY_NORMAL);     
                case CharacterQuality.Good:
                    return TDLanguageTable.Get(Define.DISCIPLE_NAME_GOOD);
                case CharacterQuality.Perfect:
                    return TDLanguageTable.Get(Define.DISCIPLE_NAME_PREFECT);
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// 获取物品的数量
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetItemNumber(int number)
        {
            return number.ToString()+TDLanguageTable.Get(Define.FACILITY_WAREHOUSE_INDIVIDUAL);
        }
        /// <summary>
        /// 获取装备的阶级
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetItemClass(int number)
        {
            return number.ToString()+TDLanguageTable.Get(Define.FACILITY_WAREHOUSE_CLASS);
        }

        public static string GetBonus(float bonus)
        {
            return (bonus * 100).ToString()+"%";
        }
    }
}