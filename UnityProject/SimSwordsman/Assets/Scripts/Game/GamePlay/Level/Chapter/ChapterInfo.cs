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
        public long recommendAtkValue;
        public List<RewardBase> levelRewardList = new List<RewardBase>();
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
                if (item.RewardItem == RewardItemType.Exp_Role)
                {
                    return item.Count;
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
            this.recommendAtkValue = long.Parse(tDLevelConfig.recommendAtkValue);
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
                // Debug.LogError(rewards.Length);
                for (int i = 0; i < rewards.Length; i++)
                {
                    // Debug.LogError(rewards[i]);
                    // string[] reward = rewards[i].Split('|');
                    // RewardItemType rewardType = EnumUtil.ConvertStringToEnum<RewardItemType>(reward[0]);
                    RewardBase levelReward = RewardMgr.S.GetRewardBase(rewards[i]);//LevelRewardFactory.SpawnLevelReward(rewardType, reward);
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

        public void PrepareReward()
        {
            for (int i = levelRewardList.Count - 1; i >= 0; i--)
            {
                if (levelRewardList[i].RewardItem == RewardItemType.Exp_Role || levelRewardList[i].RewardItem == RewardItemType.Exp_Kongfu)
                {
                    var reward = levelRewardList[i];
                    levelRewardList.RemoveAt(i);
                    // Debug.LogError("AddEXP:" + i);
                    for (int r = 0; r < MainGameMgr.S.BattleFieldMgr.OurCharacterList.Count; r++)
                    {
                        if (reward is Exp_RoleReward)
                            levelRewardList.Add(new Exp_RoleReward(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, reward.Count));
                        else if (reward is Exp_KongfuRweard)
                            levelRewardList.Add(new Exp_KongfuRweard(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, reward.Count));
                    }
                }


            }
        }
    }

}