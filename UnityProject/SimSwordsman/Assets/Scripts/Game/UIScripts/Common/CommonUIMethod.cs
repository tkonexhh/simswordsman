using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        /// 刷新讲武堂招募令红点
        /// </summary>
        public static void CheackRecruitmentOrder()
        {
            int allCount = MainGameMgr.S.InventoryMgr.GetAllRecruitmentOrderCount();
            if (allCount > 0)
                EventSystem.S.Send(EventID.OnSendRecruitable, true);
            else
                EventSystem.S.Send(EventID.OnSendRecruitable, false);
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

        #region 设置面板资源

        /// <summary>
        /// 检查资源是否足够
        /// </summary>
        /// <param name="costItem"></param>
        /// <param name="text"></param>
        public static void CheckResFontColor(CostItem costItem, Text text)
        {
            bool res1Hava = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)costItem.itemId, costItem.value);
            if (res1Hava)
                text.color = Color.black;
            else
                text.color = new Color(0.5647f, 0.2431f, 0.2666f, 1);
        }
        public static void RefreshUpgradeResInfo(List<CostItem> costItems,Text res1Value,Image res1Image, Text res2Value, Image res2Image, Text res3Value, Image res3Image
            , FacilityLevelInfo facilityLevelInfo,AbstractAnimPanel abstractAnim)
        {
            if (costItems == null || facilityLevelInfo==null)
                return;
            if (costItems.Count==0)
            {
                res1Image.gameObject.SetActive(false);
                res2Image.gameObject.SetActive(false);
                res3Image.gameObject.SetActive(false);
            }
            else if (costItems.Count == 1)  
            {
               
                CommonUIMethod.CheckResFontColor(costItems[0], res1Value);
                CommonUIMethod.CheckCoinFontColor(facilityLevelInfo, res2Value);

                int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);
                res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItem, costItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[0].value);
                res1Image.sprite = abstractAnim.FindSprite(GetIconName(costItems[0].itemId));
                res2Value.text = GetCurCoin(facilityLevelInfo) + Define.SLASH + CommonUIMethod.GetTenThousand(facilityLevelInfo.upgradeCoinCost);
                res2Image.sprite = abstractAnim.FindSprite("Coin");
                res3Image.gameObject.SetActive(false);
                res1Image.gameObject.SetActive(true);
                res2Image.gameObject.SetActive(true);
            }
            else if (costItems.Count == 2)
            {
                CommonUIMethod.CheckResFontColor(costItems[0], res1Value);
                CommonUIMethod.CheckResFontColor(costItems[1], res2Value);
                CommonUIMethod.CheckCoinFontColor(facilityLevelInfo, res3Value);
                int havaItemFirst = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);
                int havaItemSec = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[1].itemId);
                res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemFirst, costItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[0].value);
                res1Image.sprite = abstractAnim.FindSprite(GetIconName(costItems[0].itemId));
                res2Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemSec, costItems[1].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[1].value);
                res2Image.sprite = abstractAnim.FindSprite(GetIconName(costItems[1].itemId));
                res3Value.text = GetCurCoin(facilityLevelInfo) + Define.SLASH + CommonUIMethod.GetTenThousand(facilityLevelInfo.upgradeCoinCost);
                res3Image.sprite = abstractAnim.FindSprite("Coin");
                res3Image.gameObject.SetActive(true);
                res1Image.gameObject.SetActive(true);
                res2Image.gameObject.SetActive(true);
            }
        }
        private static string GetCurCoin(FacilityLevelInfo facilityLevelInfo)
        {
            long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coin >= facilityLevelInfo.upgradeCoinCost)
                return GetTenThousand(facilityLevelInfo.upgradeCoinCost);
            return GetTenThousand((int)coin);
        }

        private static int GetCurItem(int hava, int number)
        {
            if (hava >= number)
                return number;
            return hava;
        }
        private static string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }
        #endregion


        #region 检查资源是否足够
        public static bool CheackIsBuild(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems, bool floatMessage = true)
        {
            if (facilityLevelInfo == null)
                return false;
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (facilityLevelInfo.GetUpgradeCondition() > lobbyLevel)
            {
                if (floatMessage)
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_NEEDLOBBY));
                return false;
            }

            if (CheckPropIsEnough(facilityLevelInfo, costItems, floatMessage))
                return true;
            return false;
        }

        private static bool CheckPropIsEnough(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems, bool floatMessage = true)
        {
            for (int i = 0; i < costItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)costItems[i].itemId, costItems[i].value);
                if (!isHave)
                {
                    if (floatMessage)
                        FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return false;
                }
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(facilityLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
            {
                if (floatMessage)
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_COIN));
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 检查钱是否足够
        /// </summary>
        /// <param name="facilityLevelInfo"></param>
        /// <param name="text"></param>
        public static void CheckCoinFontColor(FacilityLevelInfo facilityLevelInfo, Text text)
        {
            //facilityLevelInfo.upgradeCoinCost
            long coinNum = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coinNum < facilityLevelInfo.upgradeCoinCost)
                text.color = new Color(0.5647f, 0.2431f, 0.2666f,1);
            else
                text.color = Color.black;
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
        public static string TextEmptyOne()
        {
            return "<color=#FFFFFF00>--</color>";
        }
        /// <summary>
        /// 获取万
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetTenThousand(int number)
        {
            int fifth = number / 10000;
            if (fifth == 0)
                return number.ToString();
            else
            {
                int fourth = GetThousand(number);
                return fifth + "." + fourth + TDLanguageTable.Get(Define.COMMON_UNIT_TENTHOUSAND);
            }
        }

        private static int GetThousand(int number)
        {
            string numStr = number.ToString();
            return int.Parse(numStr.Substring(numStr.Length-4,1));
        }

    }
}