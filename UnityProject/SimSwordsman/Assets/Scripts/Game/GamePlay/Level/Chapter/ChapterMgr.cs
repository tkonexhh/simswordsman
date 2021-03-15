using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class ChapterMgr : MonoBehaviour, IMgr
    {
        public Dictionary<int, ChapterDbItem> ChapterInfoDic = new Dictionary<int, ChapterDbItem>();

        #region IMgr
        public void OnInit()
        {
            InitData();
        }

        private void InitData()
        {
            foreach (var item in GameDataMgr.S.GetPlayerData().chapterDataList)
                if (!ChapterInfoDic.ContainsKey(item.chapter))
                    ChapterInfoDic.Add(item.chapter, item);
        }

        public void OnUpdate()
        {
        }

        public void OnDestroyed()
        {
        }
        #endregion

        #region Private

        #endregion

        #region Public
        /// <summary>
        /// 增加新的章节
        /// </summary>
        /// <param name="chapterId"></param>
        public void AddNewCheckpoint(int chapterId)
        {
            GameDataMgr.S.GetPlayerData().AddNewCheckpoint(chapterId);
            if (!ChapterInfoDic.ContainsKey(chapterId))
            {
                ChapterDbItem item = GameDataMgr.S.GetPlayerData().GetChapterDbItem(chapterId);
                if (item != null)
                {
                    ChapterInfoDic.Add(chapterId, item);
                }
                else
                {
                    Log.e("Chapter item not found");
                }
            }
        }

        /// <summary>
        /// 通过某一关卡
        /// </summary>
        /// <param name="chaptId"></param>
        /// <param name="levelId"></param>
        public void PassCheckpoint(int chapterId, int levelId)
        {
            if (ChapterInfoDic.ContainsKey(chapterId))
                ChapterInfoDic[chapterId].OnLevelPassed(levelId);
        }

        /// <summary>
        /// 判断章节是否解锁
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public bool JudgeChapterIsUnlock(int chapterId)
        {
            ChapterUnlockPrecondition unlockInfo = GetChapterInfo(chapterId).unlockPrecondition;
            if (unlockInfo.chapter == -1)
                return true;
            int unlockLevel = GetLevelProgressNumber(unlockInfo.chapter);
            if ((unlockLevel)>= unlockInfo.level)
                return true;
            return false;
        }

        /// <summary>
        /// 获取某一关卡的进度
        /// </summary>
        /// <param name="clanType"></param>
        /// <returns></returns>
        public int GetLevelProgressNumber(int clanId)
        {
            if (ChapterInfoDic.ContainsKey(clanId))
                return ChapterInfoDic[clanId].number;
            else
                return -1;
        }

        public int GetLevelProgressLevelID(int clanId)
        {
            if (ChapterInfoDic.ContainsKey(clanId))
                return ChapterInfoDic[clanId].level;
            else
                return -1;
        }

        /// <summary>
        /// 获取所有chapter的信息
        /// </summary>
        public List<ChapterConfigInfo> GetAllChapterInfo()
        {
            List<ChapterConfigInfo> chapterList = new List<ChapterConfigInfo>();
            chapterList.AddRange(TDChapterConfigTable.chapterConfigInfoDic.Values);
            return chapterList;
        }

        public LevelConfigInfo GetChapterFirstLevelInfo(int chapter)
        {
            return TDLevelConfigTable.GetFirstLevelInfo(chapter);
        }

        public int GetChapterNumber(int chapter)
        {
            return TDLevelConfigTable.GetChapterNumber(chapter);
        }

        /// <summary>
        /// 获取某一关的信息
        /// </summary>
        public ChapterConfigInfo GetChapterInfo(int chapterId)
        {
            ChapterConfigInfo info = TDChapterConfigTable.GetChapterConfigInfo(chapterId);
            return info;
        }

        /// <summary>
        /// 获取某一关的详细信息
        /// </summary>
        public LevelConfigInfo GetLevelInfo(int chapterId, int levelId)
        {
            return TDLevelConfigTable.GetLevelConfigInfo(chapterId,levelId);
        }
        /// <summary>
        /// 获取某一章节所有关卡的信息
        /// </summary>
        /// <param name="chapterID"></param>
        /// <returns></returns>
        public  Dictionary<int, LevelConfigInfo> GetAllLevelConfigInfo(int chapterID)
        {
            return TDLevelConfigTable.GetAllLevelConfigInfo(chapterID);
        }
        #endregion

    }

}