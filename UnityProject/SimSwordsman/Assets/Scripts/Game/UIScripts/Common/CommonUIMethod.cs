using DG.Tweening;
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
        /// 进行Text上翻
        /// </summary>
        /// <param name="currentScoreText"></param>
        /// <param name="curValue"></param>
        /// <param name="targetValue"></param>
        public static void TextFlipUpEffect(Text currentScoreText, float curValue, float targetValue)
        {
            Sequence mScoreSequence = DOTween.Sequence();

            mScoreSequence.SetAutoKill(false);

            mScoreSequence.Append(DOTween.To(delegate (float value) {
                //向下取整
                var temp = Math.Floor(value);
                //向Text组件赋值
                currentScoreText.text = CommonUIMethod.GetTenThousandOrMillion((long)temp)/* temp + ""*/;
            }, curValue, targetValue, 1.0f));
            //将更新后的值记录下来, 用于下一次滚动动画
            curValue = targetValue;
        }


        /// <summary>
        /// 获取挑战名称
        /// </summary>
        /// <returns></returns>
        public static string GetChallengeTitle(ChapterConfigInfo chapterConfig, int level)
        {
            return TDLanguageTable.Get(Define.CHALLENGE_NAME) + ":"
                + chapterConfig.chapterId.ToString() + "-"
                + GetChallengeSubTitle(level.ToString());
        }
        /// <summary>
        /// 拼接00：00：00
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
        private static string GetChallengeSubTitle(string subTitle)
        {
            if (subTitle.Length == 3)
            {
                string str = subTitle.Substring(subTitle.Length - 2, subTitle.Length - 1);
                if (str[0] == '0')
                {
                    return str[1].ToString();
                }
                else
                {
                    return str;
                }
            }
            else
            {
                Log.w("挑战标题ID长度不对 = " + subTitle);
                return "0";
            }
        }

        public enum SortType
        {
            AtkValue,
            Level,
        }
        /// <summary>
        /// 顺序类型
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// 从大到小
            /// </summary>
            FromBigToSmall,
            /// <summary>
            /// 从小到达
            /// </summary>
            FromSmallToBig,
        }
        /// <summary>
        /// 对弟子排序按照武力值
        /// </summary>
        /// <param name="characterItems"></param>
        /// <returns></returns>
        public static List<CharacterItem> BubbleSortForType(List<CharacterItem> characterItems, SortType sortType, OrderType orderType)
        {
            var len = characterItems.Count;
            for (var i = 0; i < len - 1; i++)
            {
                for (var j = 0; j < len - 1 - i; j++)
                {
                    switch (sortType)
                    {
                        case SortType.AtkValue:
                            if (orderType == OrderType.FromBigToSmall)
                            {
                                if (characterItems[j].atkValue < characterItems[j + 1].atkValue)
                                {
                                    var temp = characterItems[j + 1];
                                    characterItems[j + 1] = characterItems[j];
                                    characterItems[j] = temp;
                                }
                            }
                            else
                            {
                                if (characterItems[j].atkValue > characterItems[j + 1].atkValue)
                                {
                                    var temp = characterItems[j + 1];
                                    characterItems[j + 1] = characterItems[j];
                                    characterItems[j] = temp;
                                }
                            }

                            break;
                        case SortType.Level:
                            if (orderType == OrderType.FromBigToSmall)
                            {
                                if (characterItems[j].level < characterItems[j + 1].level)
                                {
                                    var temp = characterItems[j + 1];
                                    characterItems[j + 1] = characterItems[j];
                                    characterItems[j] = temp;
                                }
                            }
                            else
                            {
                                if (characterItems[j].level > characterItems[j + 1].level)
                                {
                                    var temp = characterItems[j + 1];
                                    characterItems[j + 1] = characterItems[j];
                                    characterItems[j] = temp;
                                }
                            }
                            break;
                    }
                }
            }
            return characterItems;
        }


        /// <summary>
        /// 刷新讲武堂招募令红点
        /// </summary>
        public static bool CheackRecruitmentOrder()
        {
            //int GoldAdvCount = GameDataMgr.S.GetPlayerData().GetRecruitTimeType(RecruitType.GoldMedal, RecruitTimeType.Advertisement);
            //int SilverAdvCount = GameDataMgr.S.GetPlayerData().GetRecruitTimeType(RecruitType.SilverMedal, RecruitTimeType.Advertisement);
            bool SilverAdvCount = GameDataMgr.S.GetPlayerData().IsCanLookADRecruit( RecruitType.SilverMedal, 24);
            bool GoldAdvCount = GameDataMgr.S.GetPlayerData().IsCanLookADRecruit( RecruitType.GoldMedal, 48);
            int GoldFreeCount = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.GoldenToken);
            int SilverFreeCount = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.SilverToken);
            int allCount = GoldFreeCount + SilverFreeCount;

            if (allCount > 0 || GoldFreeCount > 0 || SilverFreeCount > 0 || SilverAdvCount  || GoldAdvCount )
            {
                EventSystem.S.Send(EventID.OnSendRecruitable, true);
                return true;
            }
            else
            {
                EventSystem.S.Send(EventID.OnSendRecruitable, false);
                return false;
            }
        }
        /// <summary>
        /// 计算小时差
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetDeltaTime(string time)
        {
            DateTime dateTime;
            DateTime.TryParse(time, out dateTime);
            if (dateTime != null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalHours;
            }
            return 0;
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
        public static bool CheckResFontColor(CostItem costItem, Text text)
        {
            bool res1Hava = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)costItem.itemId, costItem.value);
            if (res1Hava)
            {
                text.color = Color.black;
                return true;
            }
            else
            {
                //text.color = new Color(0.5647f, 0.2431f, 0.2666f, 1);
                text.color = Color.black;
                return false;
            }
        }

        public static void RefreshUpgradeResInfo(List<CostItem> costItems,Transform transform,GameObject obj , FacilityLevelInfo facilityLevelInfo = null, List<UpgradeResItem> list = null)
        {
            if (costItems == null || obj == null)
                return;
            if (costItems.Count == 0)
            {
                //res1Image.gameObject.SetActive(false);
                //res2Image.gameObject.SetActive(false);
                //res3Image.gameObject.SetActive(false);
            }
            else if (costItems.Count == 1)
            {
                int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);

                UpgradeResItem upgradeResItem1 = GameObject.Instantiate(obj,transform).GetComponent<UpgradeResItem>();
                upgradeResItem1.OnInit(costItems[0],transform);
                upgradeResItem1.ShowResItem(CommonUIMethod.GetTenThousandOrMillion(GetCurItem(havaItem, costItems[0].value)) /*+ Define.SLASH + CommonUIMethod.GetTenThousandOrMillion(costItems[0].value)*/,
                    SpriteHandler.S.GetSprite(AtlasDefine.ItemIconItemIcon, GetIconName(costItems[0].itemId)));

                list?.Add(upgradeResItem1);
                if (facilityLevelInfo!=null)
                {
                    UpgradeResItem upgradeResItem2 = GameObject.Instantiate(obj, transform).GetComponent<UpgradeResItem>();
                    upgradeResItem2.OnInit(facilityLevelInfo, transform);
                    upgradeResItem2.ShowResItem(GetCurCoin(facilityLevelInfo) /*+ Define.SLASH + CommonUIMethod.GetTenThousandOrMillion(facilityLevelInfo.upgradeCoinCost)*/,
                       SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonPanelCommon, "Coin"));
                }
            }
            else if (costItems.Count == 2)
            {
                int havaItemFirst = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);
                int havaItemSec = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[1].itemId);

                UpgradeResItem upgradeResItem1 = GameObject.Instantiate(obj, transform).GetComponent<UpgradeResItem>();
                upgradeResItem1.OnInit(costItems[0], transform);
                upgradeResItem1.ShowResItem(CommonUIMethod.GetTenThousandOrMillion(GetCurItem(havaItemFirst, costItems[0].value))/* + Define.SLASH + CommonUIMethod.GetTenThousandOrMillion(costItems[0].value)*/,
                    SpriteHandler.S.GetSprite(AtlasDefine.ItemIconItemIcon, GetIconName(costItems[0].itemId)));

                UpgradeResItem upgradeResItem2 = GameObject.Instantiate(obj, transform).GetComponent<UpgradeResItem>();
                upgradeResItem2.OnInit(costItems[1], transform);
                upgradeResItem2.ShowResItem(CommonUIMethod.GetTenThousandOrMillion(GetCurItem(havaItemSec, costItems[1].value)) /*+ Define.SLASH + CommonUIMethod.GetTenThousandOrMillion(costItems[1].value)*/,
                    SpriteHandler.S.GetSprite(AtlasDefine.ItemIconItemIcon, GetIconName(costItems[1].itemId)));

                list?.Add(upgradeResItem1);
                list?.Add(upgradeResItem2);
                if (facilityLevelInfo != null)
                {
                    UpgradeResItem upgradeResItem3 = GameObject.Instantiate(obj, transform).GetComponent<UpgradeResItem>();
                    upgradeResItem3.OnInit(facilityLevelInfo, transform);
                    upgradeResItem3.ShowResItem(GetCurCoin(facilityLevelInfo)/* + Define.SLASH + CommonUIMethod.GetTenThousandOrMillion(facilityLevelInfo.upgradeCoinCost)*/,
                       SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonPanelCommon, "Coin"));
                }
            }
        }
        private static string GetCurCoin(FacilityLevelInfo facilityLevelInfo)
        {
            //long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            //if (coin >= facilityLevelInfo.upgradeCoinCost)
            //    return GetTenThousandOrMillion(facilityLevelInfo.upgradeCoinCost);
            //return GetTenThousandOrMillion((int)coin);
            return GetTenThousandOrMillion(facilityLevelInfo.upgradeCoinCost);

        }

        private static int GetCurItem(int hava, int number)
        {
            //if (hava >= number)
            //    return number;
            //return hava;
            return number;

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
        public static bool CheckCoinFontColor(FacilityLevelInfo facilityLevelInfo, Text text)
        {
            //facilityLevelInfo.upgradeCoinCost
            long coinNum = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coinNum < facilityLevelInfo.upgradeCoinCost)
            {
                //text.color = new Color(0.5647f, 0.2431f, 0.2666f, 1);
                text.color = Color.black;
                return false;
            }
            else
            {
                text.color = Color.black;
                return true;
            }
        }
        public static string GetStrForColor(string color, string cont,bool table = false)
        {
            if (!table)
            {
                return "<color=" + color + ">" + cont + "</color>";
            }
            else
            {
                return "<color=" + color + ">" + GetStringForTableKey(cont) + "</color>";

            }
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
        /// <summary>
        /// 空格
        /// </summary>
        /// <returns></returns>
        public static string TextIndent()
        {
            return "<color=#FFFFFF00>----</color>";
        }
        /// <summary>
        /// 空格
        /// </summary>
        /// <returns></returns>
        public static string TextEmptyOne()
        {
            return "<color=#FFFFFF00>--</color>";
        }
        /// <summary>
        /// 获取万or亿or万亿
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetTenThousandOrMillion(long number)
        {
            if (number.ToString().Length > 12)
            {
                long MainNumber = number / 1000000000000;
                long remainder = number % 1000000000000;

                return MainNumber.ToString() + "." + remainder.ToString()[0] + "万亿";
            }
            else if (number.ToString().Length > 8)
            {   
                long MainNumber = number / 100000000;
                long remainder = number % 100000000;

                return MainNumber.ToString() + "." + remainder.ToString()[0] + "亿";
            }
            else if(number.ToString().Length > 4)
            {
                long MainNumber = number / 10000;
                long remainder = number % 10000;
                return MainNumber.ToString() + "." + remainder.ToString()[0] + "万";
            }
            else
            {
                return number.ToString();
            }
        }

        private static long GetThousand(long number)
        {
            string numStr = number.ToString();
            return long.Parse(numStr.Substring(numStr.Length - 4, 1));
        }

    }
}