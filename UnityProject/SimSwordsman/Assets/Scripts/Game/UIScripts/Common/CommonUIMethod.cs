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
        /// ���ݰ������ͻ�ȡ��������
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
        /// ��ȡ��ս����
        /// </summary>
        /// <returns></returns>
        public static string GetChallengeTitle(ChapterConfigInfo chapterConfig, int level)
        {
            return TDLanguageTable.Get(Define.CHALLENGE_NAME) + ":"
                + chapterConfig.chapterId.ToString() + "-"
                + level.ToString();
        }

        /// <summary>
        /// ��������ǰ׺
        /// </summary>
        /// <param name="clanType">��������</param>
        /// <returns></returns>
        public static string SetClanPrefix(ClanType clanType)
        {
            return "ClanType_" + clanType.ToString();
        }

        /// <summary>
        /// ���ݱ��е�Key����ȡ��Ӧ������
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringForTableKey(string key)
        {
            return TDLanguageTable.Get(key);
        }

        /// <summary>
        /// ���ݵ���Ʒ�ʻ�ȡ��Ӧ��str
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
        /// ��ȡ��Ʒ������
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetItemNumber(int number)
        {
            return number.ToString()+TDLanguageTable.Get(Define.FACILITY_WAREHOUSE_INDIVIDUAL);
        }
        /// <summary>
        /// ��ȡװ���Ľ׼�
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