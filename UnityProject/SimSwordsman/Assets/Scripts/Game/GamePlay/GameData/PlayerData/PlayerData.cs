using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PlayerData : DataDirtyHandler, IResetHandler, IDailyResetData
    {
        public string coinNumStr;

        public int coinNum;
        public int foodNum;

        public string firstPlayTime;
        public string lastPlayTime;

        public bool isGuideStart;

        public List<HerbModel> herbModels = new List<HerbModel>();
        public RecruitData recruitData = new RecruitData();
        public List<ChapterDbItem> chapterDataList = new List<ChapterDbItem>();

        public List<int> unlockFoodItemIDs = new List<int>();//已解锁的伙房食物id
        public List<int> rewardCollectItemIDs = new List<int>();//可以收集最大值的收集物

        private long m_CoinNum = 0;
        public int signInCount;
        //是否是第一次使用金牌招募
        public bool firstGoldRecruit;
        //是否是第一次使用银牌牌招募
        public bool firstSilverRecruit;

        public bool UnlockVisitor;
        public int visitorCount;
        public bool UnlockWorkSystem;

        #region 食物倒计时
        public string FoodCoundDownTime = string.Empty;
        #endregion

        /// <summary>
        /// 辅助记录刷新了食物的次数
        /// </summary>
        public int FoodRefreshCount;
        /// <summary>
        /// 今日食物刷新总次数
        /// </summary>
        public int FoodRefreshTimesToday;
        public string FoodRefreshRecordingTime;

        public void SetDefaultValue()
        {
            m_CoinNum = Define.DEFAULT_COIN_NUM;
            coinNumStr = m_CoinNum.ToString();

            coinNum = Define.DEFAULT_COIN_NUM;
            foodNum = Define.DEFAULT_FOOD_NUM;

            lastPlayTime = "0";
            firstPlayTime = string.Empty;

            isGuideStart = false;

            UnlockWorkSystem = false;
            UnlockVisitor = false;
            visitorCount = 0;
            firstGoldRecruit = false;
            firstSilverRecruit = false;
            FoodRefreshRecordingTime = DateTime.Now.ToString().Substring(0, 8) + ' ' + "06:00:00";
            FoodRefreshTimesToday = 5;
            FoodRefreshCount = 0;
            SetDataDirty();
        }

        public void ResetDailyData()
        {
            visitorCount = 0;
            SetDataDirty();
        }

        public void Init()
        {
            m_CoinNum = long.Parse(coinNumStr);
        }
        public RecruitData GetRecruitData()
        {
            return recruitData;
        }

        #region work system
        public bool IsUnlockWorkSystem()
        {
            return UnlockWorkSystem;
        }
        public void SetWorkSystem(bool isUnlock)
        {
            UnlockWorkSystem = isUnlock;
            SetDataDirty();
        }
        #endregion

        #region 草药相关
        /// <summary>
        /// 获取存档中的草药
        /// </summary>
        /// <returns></returns>
        public List<HerbModel> GetArchiveHerb()
        {
            return herbModels;
        }

        /// <summary>
        /// 增加药草
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        public void AddArchiveHerb(int id, int number)
        {
            HerbModel herbModel = herbModels.Where(i => i.ID == id).FirstOrDefault();
            if (herbModel != null)
                herbModel.Number += number;
            else
                herbModels.Add(new HerbModel(id, number));
        }

        #endregion

        #region 角色章节任务相关
        public void AddNewCheckpoint(int chapterId)
        {
            if (!chapterDataList.Any(i => i.chapter == chapterId))
                chapterDataList.Add(new ChapterDbItem(chapterId));
        }

        public ChapterDbItem GetChapterDbItem(int chapterId)
        {
            ChapterDbItem chapter = chapterDataList.Where(i => i.chapter == chapterId).FirstOrDefault();
            return chapter;
        }
        //public ChapterDbItem FindChapterDbItem(int chapterId)
        //{
        //    ChapterDbItem chapter = chapterDataList.Where(i => i.chapter == chapterId).FirstOrDefault();
        //    if (chapter != null)
        //        return chapter;
        //    return null;
        //}

        /// <summary>
        /// 根据ID判断是否存在该章节任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public bool ContainsForChapterDbItemId(int id)
        //{
        //    return chapterDataList.Any(i => i.chapter == id);
        //}
        /// <summary>
        /// 添加该章节任务
        /// </summary>
        /// <param name="chapterDbItem"></par am>
        //public void AddChapterDbItem(int chapterId, int levelId)
        //{
        //    ChapterDbItem chapter = chapterDataList.Where(i => i.chapter == chapterId).FirstOrDefault();
        //    if (chapter != null)
        //        chapter.OnLevelPassed(levelId);
        //    else
        //        chapterDataList.Add(new ChapterDbItem(chapterId, levelId));
        //}
        #endregion

        #region 食物刷新次数
        public int GetFoodRefreshCount()
        {
            return FoodRefreshCount;
        }

        public void SetFoodRefreshCount(int count)
        {
            FoodRefreshCount = count;
        }
        public int GetFoodRefreshTimesToday()
        {
            return FoodRefreshTimesToday;
        }

        public void SetFoodRefreshTimesToday()
        {
            FoodRefreshTimesToday -= 1;
            if (FoodRefreshTimesToday < 0)
                FoodRefreshTimesToday = 0;
        }
        public void ResetFoodRefreshTimesToday()
        {
            FoodRefreshTimesToday = 5;
        }
        public string GetFoodRefreshRecordingTime()
        {
            return FoodRefreshRecordingTime;
        }

        public void SetFoodRefreshRecordingTime(string recordingTime)
        {
            FoodRefreshRecordingTime = recordingTime;
        }
        #endregion

        #region 食物倒计时
        public string GetFoodCoundDownTime()
        {
            return FoodCoundDownTime;
        }

        public void SetFoodCoundDownTime(string str)
        {
            FoodCoundDownTime = str;
        }
        #endregion

        public void SetCoinNum(long num)
        {
            m_CoinNum = num;

            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);

            SetDataDirty();
        }
        /// <summary>
        /// 检查钱是否足够
        /// </summary>
        /// <param name="coinNumber"></param>
        /// <returns></returns>
        public bool CheckHaveCoin(int coinNumber)
        {
            if (m_CoinNum >= coinNumber)
                return true;
            return false;
        }

        /// <summary>
        /// 设置讲武堂建造时间
        /// </summary>
        public void SetLobbyBuildeTime()
        {
            recruitData.SetLobbyBuildTime();
        }
        /// <summary>
        /// 获取讲武堂建造时间
        /// </summary>
        /// <returns></returns>
        public string GetLobbyBuildTime()
        {
            return recruitData.GetLobbyBuildTime();
        }

        /// <summary>
        /// 设置招募时间次数
        /// </summary>
        /// <param name="recruitTimeType"></param>
        /// <param name="delta"></param>
        public void SetRecruitTimeType(RecruitType recruitType, RecruitTimeType recruitTimeType, int value)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    recruitData.SetGoldRecruitTimeType(recruitTimeType, value);
                    break;
                case RecruitType.SilverMedal:
                    recruitData.SetSilverRecruitTimeType(recruitTimeType, value);
                    break;
            }

        }
        /// <summary>
        /// 设置招募次数
        /// </summary>
        /// <param name="recruitType"></param>
        /// <param name="delta"></param>
        public void IncreaseCurRecruitCount(RecruitType recruitType, int delta)
        {
            recruitData.IncreaseCurRecruitCount(recruitType, delta);
        }
        public int GetRecruitTimeType(RecruitType m_CurRecruitType, RecruitTimeType recruitTimeType)
        {
            switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
                    return recruitData.GetGoldRecruitTimeType(recruitTimeType);
                case RecruitType.SilverMedal:
                    return recruitData.GetSilverRecruitTimeType(recruitTimeType);
                default:
                    break;
            }
            return 0;
        }

        public long GetCoinNum()
        {
            return m_CoinNum;
        }

        public int GetFoodNum()
        {
            return foodNum;
        }
        public void SetFoodNum(int food)
        {
            foodNum = food;
        }

        public void AddCoinNum(long delta)
        {
            //if (m_CoinNum < -delta)
            //{
            //    Log.e(m_CoinNum + "/" + delta + "/");
            //}
            if (delta > 0)
                m_CoinNum += FoodBuffSystem.S.Coin(delta);

            if (m_CoinNum < 0)
            {
                m_CoinNum = 0;
            }

            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
            EventSystem.S.Send(EventID.OnRawMaterialChangeEvent);
            SetDataDirty();
        }

        public bool ReduceCoinNum(long delta)
        {
            m_CoinNum = m_CoinNum - delta;
            if (m_CoinNum < 0)
            {
                FloatMessage.S.ShowMsg("您的金钱不够，升级失败");
                //UIMgr.S.OpenPanel(UIID.LogPanel, "升级提示", "您的金钱不够，升级失败");
                m_CoinNum += delta;
                return false;
            }
            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);

            SetDataDirty();
            return true;
        }

        public void AddFoodNum(int delta)
        {
            foodNum += delta;


            EventSystem.S.Send(EventID.OnAddFood);
            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);

            SetDataDirty();
        }
        public void ReduceFoodNum(int delta)
        {
            if (delta == 0)
                return;
            foodNum -= delta;
            if (foodNum < 0)
            {
                foodNum = 0;
            }

            EventSystem.S.Send(EventID.OnReduceFood, foodNum + delta);
            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
            SetDataDirty();
        }

        public void SetLastPlayTime(string time)
        {
            if (long.Parse(time) > long.Parse(lastPlayTime))
            {
                //判断是否过一天
                var lastTime = GameExtensions.GetTimeFromTimestamp(lastPlayTime);
                var now = GameExtensions.GetTimeFromTimestamp(time);
                if (lastTime.DayOfYear != now.DayOfYear)
                {
                    GameDataMgr.S.ResetDailyData();
                }
                lastPlayTime = time;
                SetDataDirty();
            }
        }

        public DateTime GetFirstLoginTime()
        {
            return DateTime.Parse(firstPlayTime);
        }
        public void AddSignInCount(int delta)
        {
            signInCount++;
            SetDataDirty();
        }


        #region Chapter




        #endregion


        #region Visitor
        public void AddVisitorCount(int count = 1)
        {
            visitorCount += count;
            SetDataDirty();
        }

        public void ResetVisitorCount()
        {
            visitorCount = 0;
            SetDataDirty();
        }
        #endregion
        public void OnReset()
        {
            m_CoinNum = 0;
            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);

            SetDataDirty();
        }
    }

    /// <summary>
    /// 招募时间的类型
    /// </summary>
    public enum RecruitTimeType
    {
        /// <summary>
        /// 免费招募
        /// </summary>
        Free,
        /// <summary>
        /// 广告招募
        /// </summary>
        Advertisement,

    }

    [Serializable]
    public class RecruitData
    {
        public string lobbyBuildTime = string.Empty;

        public int silverFree = 1;
        public int silverAdvertisement = 1;
        public int goldFree = 1;
        public int goldAdvertisement = 1;

        public int goldAdvertisementCount = 1;
        public int silverAdvertisementCount = 1;

        public int goldRecruitCount = 0;
        public int silverRecruitCount = 0;

        public bool goldIsFirst = true;
        public bool silverIsFirst = true;

        public int goldMedalGood = 7;
        public int goldMedalPerfect = 3;

        public int silverMedalNormal = 13;
        public int silverMedalGood = 6;
        public int silverMedalPerfect = 1;

        public RecruitData() { }


        public void IncreaseCurRecruitCount(RecruitType recruitType, int delta)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    goldRecruitCount += delta;
                    break;
                case RecruitType.SilverMedal:
                    silverRecruitCount += delta;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置招募/时间次数
        /// </summary>
        /// <param name="recruitTimeType"></param>
        /// <param name="delta"></param>
        public void SetGoldRecruitTimeType(RecruitTimeType recruitTimeType, int value)
        {
            switch (recruitTimeType)
            {
                case RecruitTimeType.Free:
                    goldFree = value;
                    break;
                case RecruitTimeType.Advertisement:
                    goldAdvertisement = value;
                    break;
                default:
                    break;
            }
        }
        public void SetSilverRecruitTimeType(RecruitTimeType recruitTimeType, int value)
        {
            switch (recruitTimeType)
            {
                case RecruitTimeType.Free:
                    silverFree = value;
                    break;
                case RecruitTimeType.Advertisement:
                    silverAdvertisement = value;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 获取招募/时间次数
        /// </summary>
        /// <param name="recruitTimeType"></param>
        /// <returns></returns>
        public int GetSilverRecruitTimeType(RecruitTimeType recruitTimeType)
        {
            switch (recruitTimeType)
            {
                case RecruitTimeType.Free:
                    return silverFree;
                case RecruitTimeType.Advertisement:
                    return silverAdvertisement;
            }
            return 0;

        }

        public int GetGoldRecruitTimeType(RecruitTimeType recruitTimeType)
        {
            switch (recruitTimeType)
            {
                case RecruitTimeType.Free:
                    return goldFree;
                case RecruitTimeType.Advertisement:
                    return goldAdvertisement;
            }
            return 0;
        }

        public void SetLobbyBuildTime()
        {
            lobbyBuildTime = DateTime.Now.ToString();
        }

        public string GetLobbyBuildTime()
        {
            return lobbyBuildTime;
        }

        public void SetIsFirstValue(RecruitType recruitType)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    goldIsFirst = false;
                    break;
                case RecruitType.SilverMedal:
                    silverIsFirst = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 重新设置内部数据
        /// </summary>
        /// <param name="recruitData"></param>
        public void SetRecruitGoldData(RecruitModel recruitModel)
        {
            goldRecruitCount = recruitModel.GetCurRecruitCount();
            goldAdvertisementCount = recruitModel.GetAdvertisementCount();
            goldMedalGood = recruitModel.goldMedalGood;
            goldMedalPerfect = recruitModel.goldMedalPerfect;
        }

        public void SetRecruitSilverData(RecruitModel recruitModel)
        {
            silverRecruitCount = recruitModel.GetCurRecruitCount();
            silverAdvertisementCount = recruitModel.GetAdvertisementCount();
            silverRecruitCount = recruitModel.GetCurRecruitCount();
            silverMedalNormal = recruitModel.silverMedalNormal;
            silverMedalGood = recruitModel.silverMedalGood;
            silverMedalPerfect = recruitModel.silverMedalPerfect;
        }
    }

    [Serializable]
    public class ChapterDbItem
    {
        public int chapter;
        public int level;
        public int number;


        public ChapterDbItem()
        {
        }

        public ChapterDbItem(int chapter)
        {
            this.chapter = chapter;
            number = 0;
            this.level = MainGameMgr.S.ChapterMgr.GetChapterFirstLevelInfo(chapter).level;
        }
        public ChapterDbItem(int chapter, int level)
        {
            this.chapter = chapter;
            this.level = level;
        }

        public void OnLevelPassed(int level)
        {
            if (this.level == level)
            {
                number++;
                this.level += 1;
                this.level = Mathf.Clamp(this.level, 1, Define.LEVEL_COUNT_PER_CHAPTER);
                GameDataMgr.S.GetPlayerData().SetDataDirty();

                EventSystem.S.Send(EventID.OnChanllengeSuccess, level);

                GameDataMgr.S.Save();
            }
        }
    }
}