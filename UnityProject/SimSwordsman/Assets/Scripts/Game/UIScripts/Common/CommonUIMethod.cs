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
                case ClanType.Gaibang:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Gaibang));
                case ClanType.Shaolin:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Shaolin));
                case ClanType.Wudang:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Wudang));
                case ClanType.Emei:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Emei));
                case ClanType.Huashan:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Huashan));
                case ClanType.Wudu:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Wudu));
                case ClanType.Mojiao:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Mojiao));
                case ClanType.Xiaoyao:
                    return TDLanguageTable.Get(SetClanPrefix(ClanType.Xiaoyao));
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

        public static string GetStrForColor(string color, string cont)
        {
            return "<color=" + color + ">" + GetStringForTableKey(cont) + "</color>";
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
            return number.ToString() + TDLanguageTable.Get(Define.FACILITY_WAREHOUSE_INDIVIDUAL);
        }
        /// <summary>
        /// 获取装备的阶级
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetItemClass(int number)
        {
            return number.ToString() + TDLanguageTable.Get(Define.COMMON_UNIT_CLASS);
        }
        /// <summary>
        /// 获取等级
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetGrade(int number)
        {
            return number.ToString() + TDLanguageTable.Get(Define.COMMON_UNIT_GRADE);
        }
        /// <summary>
        /// 获取天数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetDay(int number)
        {
            return number.ToString() + TDLanguageTable.Get(Define.COMMON_UNIT_DAY);
        }
        /// <summary>
        /// 获得人
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetPeople(int number)
        {
            return number.ToString() + TDLanguageTable.Get(Define.COMMON_UNIT_PEOPLE);
        }
        /// <summary>
        /// 获得段
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetPart(int number)
        {
            return GetTextNumber(number) + TDLanguageTable.Get(Define.COMMON_UNIT_PAET);
        }
        /// <summary>
        /// 获得阶
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetClass(int number)
        {
            return GetTextNumber(number) + TDLanguageTable.Get(Define.COMMON_UNIT_CLASS);
        }
        public static string GetTextNumber(int number)
        {
            switch (number)
            {
                case 0:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_ZERO);
                case 1:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_ONE);
                case 2:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_TWO);
                case 3:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_THREE);
                case 4:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_FOUR);
                case 5:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_FIVE);
                case 6:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_SIX);
                case 7:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_SEVENT);
                case 8:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_EIGHT);
                case 9:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_NINE);
                case 10:
                    return TDLanguageTable.Get(Define.COMMON_NUMBER_TEN);
                default:
                    return null;
            }
        }

        public static string GetBonus(float bonus)
        {
            return (bonus * 100).ToString() + "%";
        }

        /// <summary>
        /// 升级需要讲武堂达到
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string GetUpgradeCondition(int level)
        {
            return GetStringForTableKey(Define.COMMON_UPGRADEINFODESC) + GetGrade(level);
        }

        public static string TextIndent()
        {
            return "<color=#FFFFFF00>----</color>";
        }
    }
}