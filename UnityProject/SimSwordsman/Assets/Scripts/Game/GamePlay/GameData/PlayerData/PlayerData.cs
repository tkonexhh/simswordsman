using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PlayerData : DataDirtyHandler, IResetHandler, IDailyResetData
    {
        public bool messagePush;

        public string coinNumStr;

        public int foodNum;

        public string firstPlayTime;
        public string lastPlayTime;

        public bool isGuideStart;

        public RecruitData recruitData = new RecruitData();
        public RecordData recordData = new RecordData();
        public TaskData taskData = new TaskData();
        public List<ChapterDbItem> chapterDataList = new List<ChapterDbItem>();
        public bool IsLookADInLastChallengeBossLevel = true;
        public bool LastChallengeIsBossLevel = false;

        public List<int> unlockFoodItemIDs = new List<int>();//已解锁的伙房食物id

        private long m_CoinNum = 0;
        public int signInCount;
        //是否是第一次使用金牌招募
        public bool firstGoldRecruit;
        //是否是第一次使用银牌牌招募
        public bool firstSilverRecruit;

        public bool UnlockVisitor;
        public bool UnlockWorkSystem;

        /// <summary>
        /// 点击金牌招募的时间
        /// </summary>
        public DateTime m_LookADClickGoldRecruitDate;
        /// <summary>
        /// 点击银牌招募的时间
        /// </summary>
        public DateTime m_LookADClickSilverRecruitDate;

        #region 食物倒计时
        public string FoodCoundDownTime = string.Empty;
        /// <summary>
        /// 辅助记录刷新了食物的次数
        /// </summary>
        public int FoodRefreshCount;
        /// <summary>
        /// 今日食物刷新总次数
        /// </summary>
        public int FoodRefreshTimesToday;
        public string FoodRefreshRecordingTime;
        #endregion

        #region 插屏广告
        /// <summary>
        /// 是否是新用户啊
        /// </summary>
        public  bool IsNewUser;
        /// <summary>
        /// 免播广告次数
        /// </summary>
        public int NoBroadcastTimes;
        /// <summary>
        /// 战斗次数
        /// </summary>
        public int BattleTimes;
        /// <summary>
        /// 播放了插屏广告的次数
        /// </summary>
        public int PlayInterADTimes;
        /// <summary>
        ///  插屏广告刷新时间
        /// </summary>
        public string NoBroadcastTimesTime;
        /// <summary>
        /// 插屏广告刷新的次数
        /// </summary>
        public int RefreshInterTimes;

        #endregion

        #region 挑战 广告相关
        public void UpdateIsLookADInLastChallengeBossLevel(bool value) 
        {
            IsLookADInLastChallengeBossLevel = value;
            SetDataDirty();
        }
        public void UpdateLastChallengeIsBossLevel(bool value) {
            LastChallengeIsBossLevel = value;
            SetDataDirty();
        }
        public bool CurrentChallengeLevelIsPlayInterAD() 
        {
            if (LastChallengeIsBossLevel && IsLookADInLastChallengeBossLevel == false) {
                return true;
            }
            return false;
        }
        #endregion

        public void SetDefaultValue()
        {
            messagePush = true;
            m_CoinNum = Define.DEFAULT_COIN_NUM;
            coinNumStr = m_CoinNum.ToString();

            foodNum = Define.DEFAULT_FOOD_NUM;

            lastPlayTime = "0";
            firstPlayTime = string.Empty;

            isGuideStart = false;

            IsNewUser = true;
            UnlockWorkSystem = false;
            UnlockVisitor = false;
            firstGoldRecruit = false;
            firstSilverRecruit = false;

            FoodRefreshRecordingTime = DateTime.Now.ToString().Substring(0, 9) + ' ' + "06:00:00";
            NoBroadcastTimesTime = DateTime.Now.ToString().Substring(0, 9) + ' ' + "06:00:00";
            FoodRefreshTimesToday = 5;
            FoodRefreshCount = 0;
            SetDataDirty();
        }

        public void ResetDailyData()
        {
            recordData.ResetDailyData();
            taskData.ResetDailyData();
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

        #region 插屏广告

        public void RefreshInterAdData()
        {
            NoBroadcastTimes = 0;
            PlayInterADTimes = 0;
        }

        public bool isPlayMaxTimes()
        {
            return PlayInterADTimes >= 20;
        }

        public void SetPlayInterADTimes()
        {
            PlayInterADTimes++;
        }

        public bool GetIsNewUser()
        {
            return IsNewUser;
        } 
        public void SetRefreshInterTimes(int count)
        {
            RefreshInterTimes = count;
        }

        public int GetRefreshInterTimes()
        {
            return RefreshInterTimes;
        }
        public string GetNoBroadcastTimesTime()
        {
            if (NoBroadcastTimesTime==null)
                NoBroadcastTimesTime = DateTime.Now.ToString().Substring(0, 9) + ' ' + "06:00:00"; ;
            return NoBroadcastTimesTime;
        }
        public void SetIsNewUser()
        {
            IsNewUser = false; ;
        }
        public int GetBattleTimes()
        {
            return BattleTimes;
        }
        public void SetBattleTimes()
        {
             BattleTimes++;
        }
        public int GetNoBroadcastTimes()
        {
            return NoBroadcastTimes;
        }
        public void SetNoBroadcastTimes(int delta=1)
        {
            NoBroadcastTimes += delta;
            if (NoBroadcastTimes < 0)
                NoBroadcastTimes = 0;
        }
        public void SetBattleTimes(int delta = 1)
        {
            BattleTimes+=delta;
            if (BattleTimes < 0)
                BattleTimes = 0;
        }
        #endregion

        #region 消息推送
        public bool GetMessagePush()
        {
            return messagePush;
        }

        public void SetMessagePush(bool msg)
        {
            messagePush = msg;
        }
        #endregion


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

        #region 招募相关
        public DateTime GetRecruitLastClickTime(RecruitType rt) 
        {
            return recruitData.GetLastRecruitDateTime(rt);
        }
        public void UpdateRecruitLastClickTime(RecruitType rt) {
            recruitData.UpdateLastLookADRecruitDateTime(rt);
            SetDataDirty();
        }
        public bool IsCanLookADRecruit(RecruitType rt,int intervalTimeHours) {
            return recruitData.IsCanLookADRecruit(rt, intervalTimeHours);
        }
        #endregion

        public long GetCoinNum()
        {
            return m_CoinNum;
        }

        public int GetFoodNum()
        {
            if (foodNum<0)
                foodNum = 0;
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
        public int GetSignInCount()
        {
            return signInCount;
        }

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

        public bool IsRecodeGoldRecruitDate = false;
        public bool IsRecodeSilverRecruitDate = false;
        public DateTime GoldRecruitDateTime;
        public DateTime SilverRecruitDateTime;

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
        public bool IsCanLookADRecruit(RecruitType rt, int intervalTimeHours) 
        {
            if (rt == RecruitType.GoldMedal)
            {
                if (IsRecodeGoldRecruitDate == false) 
                {
                    return true;
                }else if (GoldRecruitDateTime.AddSeconds(intervalTimeHours) <= DateTime.Now)
                {
                    return true;
                }
            }
            else {
                if (IsRecodeSilverRecruitDate == false)
                {
                    return true;
                }
                else if (SilverRecruitDateTime.AddSeconds(intervalTimeHours) <= DateTime.Now) {
                    return true;
                }
            }

            return false;
        }
        public DateTime GetLastRecruitDateTime(RecruitType rt) 
        {
            if (rt == RecruitType.GoldMedal)
            {
                if (IsRecodeGoldRecruitDate == false) 
                {
                    GoldRecruitDateTime = DateTime.Now;
                    IsRecodeGoldRecruitDate = true;
                }                
                return GoldRecruitDateTime;
            }
            else {
                if (IsRecodeSilverRecruitDate == false) 
                {
                    SilverRecruitDateTime = DateTime.Now;
                    IsRecodeSilverRecruitDate = true;
                }
                
                return SilverRecruitDateTime;
            }
        }
        public void UpdateLastLookADRecruitDateTime(RecruitType rt) 
        {
            if (rt == RecruitType.GoldMedal)
            {
                GoldRecruitDateTime = DateTime.Now;
                IsRecodeGoldRecruitDate = true;
            }
            else {
                SilverRecruitDateTime = DateTime.Now;
                IsRecodeSilverRecruitDate = true;
            }
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