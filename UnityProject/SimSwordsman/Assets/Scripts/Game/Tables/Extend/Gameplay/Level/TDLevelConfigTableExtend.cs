using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDLevelConfigTable
    {
        private static Dictionary<int, ChapterInfo> m_AllChapter = new Dictionary<int, ChapterInfo>();

        static void CompleteRowAdd(TDLevelConfig tdData)
        {
            if (m_AllChapter.ContainsKey(tdData.chapter))
                m_AllChapter[tdData.chapter].AddLevelConfig(tdData);
            else
                m_AllChapter.Add(tdData.chapter, new ChapterInfo(tdData));
        }
        /// <summary>
        /// 获取具体某关卡的所有敌人信息
        /// </summary>dd
        public static List<EnemyConfig> GetCheckpointAllEnemies(int chapterId, int levelId)
        {
            if (m_AllChapter.ContainsKey(chapterId))
                return m_AllChapter[chapterId].GetAllEnemies(levelId);
            return null;
        }

        /// <summary>
        /// 获取某个关卡的信息
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static LevelConfigInfo GetLevelConfigInfo(int chapterId, int levelId)
        {
            if (m_AllChapter.ContainsKey(chapterId))
                return m_AllChapter[chapterId].GetLevelConfigInfo(levelId);
            return null;
        }

        public static int GetLevelId(int chapter, int index)
        {
            if (m_AllChapter.ContainsKey(chapter))
                return m_AllChapter[chapter].GetLevelId(index);
            return -1;
        }
    }

    public class EnemyConfig
    {
        public int ID { set; get; }
        public int Number { set; get; }
        public int Atk { set; get; }

        public EnemyConfig(int id,int number,int skill)
        {
            ID = id;
            Number = number;
            Atk = skill;
        }

        public EnemyConfig(string enemisStr)
        {
            string[] enemies = enemisStr.Split(';');
            for (int i = 0; i < enemies.Length; i++)
            {
                string[] enemie = enemies[i].Split('|');
                ID = int.Parse(enemie[0]);
                Number = int.Parse(enemie[1]);
                Atk = int.Parse(enemie[2]);
            }
        }
        public EnemyConfig(string[] enemyStr)
        {
            ID = int.Parse(enemyStr[0]);
            Number = int.Parse(enemyStr[1]);
            Atk = int.Parse(enemyStr[2]);
        }
    }

    public class ChapterInfo
    {
        private Dictionary<int, LevelConfigInfo> m_AllCheckpoint = new Dictionary<int, LevelConfigInfo>();

        public ChapterInfo(TDLevelConfig tDLevelConfig)
        {
            if (!m_AllCheckpoint.ContainsKey(tDLevelConfig.level))
                m_AllCheckpoint.Add(tDLevelConfig.level,new LevelConfigInfo(tDLevelConfig));
        }

        /// <summary>
        /// 获取一个关卡的信息
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public LevelConfigInfo GetLevelConfigInfo(int levelId)
        {
            if (m_AllCheckpoint.ContainsKey(levelId))
                return m_AllCheckpoint[levelId];
            return null;
        }

        /// <summary>
        /// 添加关卡信息
        /// </summary>
        /// <param name="tDLevelConfig"></param>
        public void AddLevelConfig(TDLevelConfig tDLevelConfig)
        {
            if (!m_AllCheckpoint.ContainsKey(tDLevelConfig.level))
                m_AllCheckpoint.Add(tDLevelConfig.level, new LevelConfigInfo(tDLevelConfig));
        }

        public List<EnemyConfig> GetAllEnemies(int levelId)
        {
            if (m_AllCheckpoint.ContainsKey(levelId))
                return m_AllCheckpoint[levelId].enemiesList;
            return null;
        }

        public int GetLevelId(int index)
        {
            if (index < 0 || index >= m_AllCheckpoint.Values.Count)
            {
                Log.e("GetLevelId index out of range: " + index);
                return -1;
            }

            LevelConfigInfo[] configInfoArray = new LevelConfigInfo[m_AllCheckpoint.Values.Count];
            m_AllCheckpoint.Values.CopyTo(configInfoArray, 0);

            return configInfoArray[index].level;
        }
    }
}