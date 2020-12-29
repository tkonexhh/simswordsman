using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PlayerData : DataDirtyHandler, IResetHandler
    {
        public string coinNumStr;

        public int coinNum;
        public int foodNum;

        public string firstPlayTime;
        public string lastPlayTime;

        public List<HerbModel> herbModels = new List<HerbModel>();
        public RecruitData recruitData = new RecruitData();
        public List<ChapterDbItem> chapterDataList = new List<ChapterDbItem>();

        private double m_CoinNum = 0;
        public int signInCount;

        public void SetDefaultValue()
        {
            m_CoinNum = Define.DEFAULT_COIN_NUM;
            coinNumStr = m_CoinNum.ToString();

            coinNum = Define.DEFAULT_COIN_NUM;
            foodNum = Define.DEFAULT_FOOD_NUM;

            lastPlayTime = "0";
            firstPlayTime = string.Empty;

            InitChapterDataList();

            SetDataDirty();
        }

        public void Init()
        {
            m_CoinNum = double.Parse(coinNumStr);
        }
        public RecruitData GetRecruitData()
        {
            return recruitData;
        }

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
            if (herbModel!=null)
                herbModel.Number += number;
            else
                herbModels.Add(new HerbModel(id,number));
        }

        #endregion

        #region 角色章节任务相关
        public void AddNewCheckpoint(int chapterId)
        {
            if (!chapterDataList.Any(i => i.chapter == chapterId))
                chapterDataList.Add(new ChapterDbItem(chapterId));
        }

        public ChapterDbItem FindChapterDbItem(int chapterId)
        {
            ChapterDbItem chapter= chapterDataList.Where(i => i.chapter == chapterId).FirstOrDefault();
            if (chapter != null)
                return chapter;
            return null;
        }

        /// <summary>
        /// 初始化默认第一个挑战任务
        /// </summary>
        public void InitChapterDataList()
        {
            chapterDataList.Add(new ChapterDbItem(1));
        }

        /// <summary>
        /// 根据ID判断是否存在该章节任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsForChapterDbItemId(int id)
        {
            return chapterDataList.Any(i => i.chapter == id);
        }
        /// <summary>
        /// 添加该章节任务
        /// </summary>
        /// <param name="chapterDbItem"></par am>
        public void AddChapterDbItem(int chapterId, int levelId)
        {
            ChapterDbItem chapter = chapterDataList.Where(i => i.chapter == chapterId).FirstOrDefault();
            if (chapter!=null)
                chapter.OnLevelPassed(levelId);
            else
                chapterDataList.Add(new ChapterDbItem(chapterId, levelId));
        }
        #endregion

        public void SetCoinNum(double num)
        {
            m_CoinNum = num;

            EventSystem.S.Send(EventID.OnAddCoinNum);

            SetDataDirty();
        }
        public void AddCoin(int num)
        {
            m_CoinNum += num;
            EventSystem.S.Send(EventID.OnAddCoinNum);
            SetDataDirty();
        }

        public double GetCoinNum()
        {
            return m_CoinNum;
        }

        public int GetFoodNum()
        {
            return foodNum;
        }

        public void AddCoinNum(double delta)
        {
            //if (m_CoinNum < -delta)
            //{
            //    Log.e(m_CoinNum + "/" + delta + "/");
            //}

            m_CoinNum = m_CoinNum + delta;

            if (m_CoinNum < 0)
            {
                m_CoinNum = 0;

            }

            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnAddCoinNum);

            SetDataDirty();
        }

        public bool ReduceCoinNum(double delta)
        {
            m_CoinNum = m_CoinNum - delta;
            if (m_CoinNum < 0)
            {
                UIMgr.S.OpenPanel(UIID.LogPanel, "升级提示", "您的金钱不够，升级失败");
                m_CoinNum += delta;
                return false;
            }
            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnReduceCoinNum);

            SetDataDirty();
            return true;
        }

        public void AddFoodNum(int delta)
        {
            foodNum += delta;
            if (foodNum < 0)
            {
                foodNum = 0;
            }

            EventSystem.S.Send(EventID.OnAddDiamondNum);

            SetDataDirty();
        }

        public void SetLastPlayTime(string time)
        {
            if (long.Parse(time) > long.Parse(lastPlayTime))
            {
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
        public void OnReset()
        {
            m_CoinNum = 0;
            coinNumStr = m_CoinNum.ToString();

            EventSystem.S.Send(EventID.OnAddCoinNum);

            SetDataDirty();
        }
    }
  

    public class RecruitData
    {
        public int goldRecruitCount = 3;
        public int silverRecruitCount = 3;

        public bool goldIsFirst = true;
        public bool silverIsFirst = true;

        public int goldMedalGood = 7;
        public int goldMedalPerfect = 3;

        public int silverMedalNormal = 13;
        public int silverMedalGood = 6;
        public int silverMedalPerfect = 1;

        public RecruitData() { }

        public RecruitData(int _goldMedalGood, int _goldMedalPerfect,
            int _silverMedalNormal, int _silverMedalGood, int _silverMedalPerfect)
        {
            goldMedalGood = _goldMedalGood;
            goldMedalPerfect = _goldMedalPerfect;

            silverMedalNormal = _silverMedalNormal;
            silverMedalGood = _silverMedalGood;
            silverMedalPerfect = _silverMedalPerfect;
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
            goldMedalGood = recruitModel.GoldMedalGood;
            goldMedalPerfect = recruitModel.GoldMedalPerfect;
        }

        public void SetRecruitSilverData(RecruitModel recruitModel)
        {
            silverRecruitCount = recruitModel.GetCurRecruitCount();
            silverMedalNormal = recruitModel.SilverMedalNormal;
            silverMedalGood = recruitModel.SilverMedalGood;
            silverMedalPerfect = recruitModel.SilverMedalPerfect;
        }
        public void SetFirstValue(RecruitType recruitType)
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
    }

    public class ChapterDbItem
    {
        public int chapter;
        public int level;

        public ChapterDbItem()
        {
        }

        public ChapterDbItem(int chapter, int level = 1)
        {
            this.chapter = chapter;
            this.level = level;
        }

        public void OnLevelPassed(int level)
        {
            if (this.level == level)
            {
                this.level += 1;
                this.level = Mathf.Clamp(this.level, 1, Define.LEVEL_COUNT_PER_CHAPTER);
                EventSystem.S.Send(EventID.OnChanllengeSuccess, level);
            }
        }
    }
}