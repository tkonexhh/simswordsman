using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

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
        /// ��ȡ����ĳ�ؿ������е�����Ϣ
        /// </summary>dd
        public static List<EnemyConfig> GetCheckpointAllEnemies(int chapterId, int levelId)
        {
            if (m_AllChapter.ContainsKey(chapterId))
                return m_AllChapter[chapterId].GetAllEnemies(levelId);
            return null;
        }

        /// <summary>
        /// ��ȡĳ���ؿ�����Ϣ
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static LevelConfigInfo GetLevelConfigInfo(int chapterId, int levelId)
        {
            chapterId = Mathf.Clamp(chapterId, 1, Define.MAX_CHAPTER_ID);

            if (m_AllChapter.ContainsKey(chapterId))
                return m_AllChapter[chapterId].GetLevelConfigInfo(levelId);
            return null;
        }

        /// <summary>
        /// ��ȡĳһ�½ڵĵ�һ���ؿ�
        /// </summary>
        /// <param name="chapterID"></param>
        /// <returns></returns>
        public static LevelConfigInfo GetFirstLevelInfo(int chapterID)
        {
            chapterID = Mathf.Clamp(chapterID, 1, Define.MAX_CHAPTER_ID);

            if (m_AllChapter.ContainsKey(chapterID))
                return m_AllChapter[chapterID].GetLevelConfigInfo(0);
            return null;
        }

        public static int GetChapterNumber(int chapterID)
        {
            chapterID = Mathf.Clamp(chapterID, 1, Define.MAX_CHAPTER_ID);

            if (m_AllChapter.ContainsKey(chapterID))
                return m_AllChapter[chapterID].GetNumber();
            return 1;
        }

        public static Dictionary<int, LevelConfigInfo> GetAllLevelConfigInfo(int chapterID)
        {
            chapterID = Mathf.Clamp(chapterID, 1, Define.MAX_CHAPTER_ID);

            if (m_AllChapter.ContainsKey(chapterID))
                return m_AllChapter[chapterID].GetAllLevelConfigInfo();
            return null;
        }

        public static int GetLevelId(int chapter, int index)
        {
            if (m_AllChapter.ContainsKey(chapter))
                return m_AllChapter[chapter].GetLevelId(index);
            return -1;
        }

        public static bool IsBossLevel(int key)
        {
            TDLevelConfig data = GetData(key);
            if (data != null && data.type.ToLower().Equals("boss"))
            {
                return true;
            }

            return false;
        }
    }

    public class EnemyConfig
    {
        public int ConfigId { set; get; }
        public int Number { set; get; }
        public long Atk { set; get; }

        public EnemyConfig(int id, int number, int skill)
        {
            ConfigId = id;
            Number = number;
            Atk = skill;
        }

        public EnemyConfig(string enemisStr)
        {
            string[] enemies = enemisStr.Split(';');
            for (int i = 0; i < enemies.Length; i++)
            {
                string[] enemie = enemies[i].Split('|');
                ConfigId = int.Parse(enemie[0]);
                Number = int.Parse(enemie[1]);
                Atk = long.Parse(enemie[2]);
            }
        }
        public EnemyConfig(string[] enemyStr)
        {
            ConfigId = int.Parse(enemyStr[0]);
            Number = int.Parse(enemyStr[1]);
            Atk = long.Parse(enemyStr[2]);
        }
    }

    public class CharacterEnemyConfig : EnemyConfig
    {
        public CharacterQuality Quality { private set; get; }
        public int HeadID { private set; get; }
        public int BodyID { private set; get; }
        public CharacterEnemyConfig(CharacterQuality quality, int headID, int bodyID, int atk) : base(ArenaDefine.ArenaEnemyID, 1, atk)
        {
            this.Quality = quality;
            this.HeadID = headID;
            this.BodyID = bodyID;
        }
    }

    public class ChapterInfo
    {
        private Dictionary<int, LevelConfigInfo> m_AllCheckpoint = new Dictionary<int, LevelConfigInfo>();

        public ChapterInfo(TDLevelConfig tDLevelConfig)
        {
            if (!m_AllCheckpoint.ContainsKey(tDLevelConfig.level))
                m_AllCheckpoint.Add(tDLevelConfig.level, new LevelConfigInfo(tDLevelConfig));
        }

        public Dictionary<int, LevelConfigInfo> GetAllLevelConfigInfo()
        {
            return m_AllCheckpoint;
        }

        public int GetNumber()
        {
            return m_AllCheckpoint.Count;
        }

        /// <summary>
        /// ��ȡһ���ؿ�����Ϣ
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public LevelConfigInfo GetLevelConfigInfo(int index)
        {
            if (m_AllCheckpoint.Count > 0)
            {
                if (index == 0)
                {
                    return m_AllCheckpoint.FirstOrDefault().Value;
                }

                if (m_AllCheckpoint.ContainsKey(index))
                    return m_AllCheckpoint[index];
                else
                    return null;
            }
            return null;
        }

        /// <summary>
        /// ���ӹؿ���Ϣ
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