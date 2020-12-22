using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class ChapterMgr : MonoBehaviour, IMgr
    {
        private Dictionary<int, ChapterDbItem> ChapterInfoDic = new Dictionary<int, ChapterDbItem>();

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
        /// �����µ��½�
        /// </summary>
        /// <param name="chapterId"></param>
        public void AddNewCheckpoint(int chapterId)
        {
            GameDataMgr.S.GetPlayerData().AddNewCheckpoint(chapterId);
            if (!ChapterInfoDic.ContainsKey(chapterId))
                ChapterInfoDic.Add(chapterId, GameDataMgr.S.GetPlayerData().FindChapterDbItem(chapterId));
        }

        /// <summary>
        /// ͨ��ĳһ�ؿ�
        /// </summary>
        /// <param name="chaptId"></param>
        /// <param name="levelId"></param>
        public void PassCheckpoint(int chapterId, int levelId)
        {
            if (ChapterInfoDic.ContainsKey(chapterId))
            {
                ChapterInfoDic[chapterId].OnLevelPassed(levelId);
            }
        }

        public bool JudgeChapterIsUnlock(int chapterId)
        {
            int CurlevelId = GetLevelProgress(chapterId);
            ChapterUnlockPrecondition unlockInfo = GetChapterInfo(chapterId).unlockPrecondition;
            int unlockLevel = GetLevelProgress(unlockInfo.chapter);
            if ((unlockLevel-1)== unlockInfo.level)
                return true;
            return false;
        }

        /// <summary>
        /// ��ȡĳһ�ؿ��Ľ���
        /// </summary>
        /// <param name="clanType"></param>
        /// <returns></returns>
        public int GetLevelProgress(int clanId)
        {
            if (ChapterInfoDic.ContainsKey(clanId))
                return ChapterInfoDic[clanId].level;
            else
                return 0;
        }

        /// <summary>
        /// ��ȡ����chapter����Ϣ
        /// </summary>
        public List<ChapterConfigInfo> GetAllChapterInfo()
        {
            List<ChapterConfigInfo> chapterList = new List<ChapterConfigInfo>();
            chapterList.AddRange(TDChapterConfigTable.chapterConfigInfoDic.Values);
            return chapterList;
        }

        /// <summary>
        /// ��ȡĳһ�ص���Ϣ
        /// </summary>
        public ChapterConfigInfo GetChapterInfo(int chapterId)
        {
            ChapterConfigInfo info = TDChapterConfigTable.GetChapterConfigInfo(chapterId);
            return info;
        }

        /// <summary>
        /// ��ȡĳһ�ص���ϸ��Ϣ
        /// </summary>
        public LevelConfigInfo GetLevelInfo(int chapterId, int levelId)
        {
            return TDLevelConfigTable.GetLevelConfigInfo(chapterId,levelId);
        }

        /// <summary>
        /// ��ȡĳһ�ؿ����ܹؿ���
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public int GetTotalLevels(int chapterId)
        {
            return TDChapterConfigTable.GetChapterConfigInfo(chapterId).chapterCount;
        }

        /// <summary>
        /// ��ѡ��ĳ��ʱˢ���½�����
        /// </summary>
        public void OnSelectChapter(int chapter)
        {
            //m_CurChapter = chapter;

        }
        #endregion

    }

}