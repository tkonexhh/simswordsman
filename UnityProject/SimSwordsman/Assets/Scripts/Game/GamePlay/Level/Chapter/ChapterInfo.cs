using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class ChapterConfigInfo
    {
        public int chapterId;
        public string desc;
        public string battleName;
        public ChapterUnlockPrecondition unlockPrecondition;
        public ClanType clanType;
    }

    public class ChapterUnlockPrecondition
    {
        public int chapter;
        public int level;

        public ChapterUnlockPrecondition()
        {
            chapter = -1;
            level = -1;
        }

        public ChapterUnlockPrecondition(int chapter, int level)
        {
            this.chapter = chapter;
            this.level = level;
        }
    }

    public class LevelConfigInfo
    {
        public int chapterId;
        public int level;
        public string desc;
        public string battleName;
        public string enemyHeadIcon;
        public int recommendAtkValue;
        public List<LevelReward> levelRewardList = new List<LevelReward>();
        public List<EnemyConfig> enemiesList = new List<EnemyConfig>();
        public LevelConfigInfo(int chapterId, int level, string desc, int recommendAtk)
        {
            this.chapterId = chapterId;
            this.level = level;
            this.desc = desc;
            this.recommendAtkValue = recommendAtk;
        }

        public int GetExpRoleReward()
        {
            foreach (var item in levelRewardList)
            {
                if (item.rewardType== RewardItemType.Exp_Role)
                {
                    return int.Parse(item.paramStrs[1]);
                }
            }
            return 0;
        }

        public LevelConfigInfo(TDLevelConfig tDLevelConfig)
        {
            this.chapterId = tDLevelConfig.chapter;
            this.level = tDLevelConfig.level;
            this.desc = tDLevelConfig.desc;
            this.battleName = tDLevelConfig.battleName;
            this.enemyHeadIcon = tDLevelConfig.enemyHeadIcon;
            this.recommendAtkValue = tDLevelConfig.recommendAtkValue;
            AnalysisRewards(tDLevelConfig.reward);
            AnalysisEnemies(tDLevelConfig.enemies);
        }
        /// <summary>
        /// ½âÎö½±Àø
        /// </summary>
        /// <param name="reward"></param>
        private void AnalysisRewards(string rewardStr)
        {
            try
            {
                string[] rewards = rewardStr.Split(';');
                for (int i = 0; i < rewards.Length; i++)
                {
                    string[] reward = rewards[i].Split('|');
                    RewardItemType rewardType = EnumUtil.ConvertStringToEnum<RewardItemType>(reward[0]);
                    LevelReward levelReward = LevelRewardFactory.SpawnLevelReward(rewardType, reward);
                    levelRewardList.Add(levelReward);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("AnalysisRewards error: " + e.Message);
            }
        }
        /// <summary>
        /// ½âÎöµÐÈË
        /// </summary>
        /// <param name="enemisStr"></param>
        private void AnalysisEnemies(string enemisStr)
        {
            string[] enemies = enemisStr.Split(';');
            for (int i = 0; i < enemies.Length; i++)
            {
                string[] enemie = enemies[i].Split('|');
                enemiesList.Add(new EnemyConfig(enemie));
            }
        }
    }

}