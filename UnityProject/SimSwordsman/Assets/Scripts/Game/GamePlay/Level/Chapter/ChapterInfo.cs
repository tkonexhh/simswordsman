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

        private List<RewardBase> m_AcceptRewardList = new List<RewardBase>();
        private RewardBase m_TempRewardBase = null;

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
                    RewardBase levelReward = RewardMgr.S.GetRewardBase(rewards[i]);
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
        public void AcceptReward() 
        {
            if (m_AcceptRewardList != null && m_AcceptRewardList.Count > 0) 
            {
                m_AcceptRewardList.ForEach(i => i.AcceptReward());
            }
        }
        public void PrepareReward()
        {
            m_AcceptRewardList.Clear();
            for (int i = levelRewardList.Count - 1; i >= 0; i--)
            {
                m_TempRewardBase = levelRewardList[i];

                if (m_TempRewardBase.RewardItem == RewardItemType.Exp_Role || m_TempRewardBase.RewardItem == RewardItemType.Exp_Kongfu)
                {
                    for (int r = 0; r < MainGameMgr.S.BattleFieldMgr.OurCharacterList.Count; r++)
                    {
                        if (m_TempRewardBase is Exp_RoleReward)
                            m_AcceptRewardList.Add(new Exp_RoleReward(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, m_TempRewardBase.Count));
                        else if (m_TempRewardBase is Exp_KongfuRweard)
                            m_AcceptRewardList.Add(new Exp_KongfuRweard(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, m_TempRewardBase.Count));
                    }
                }
                else
                {
                    m_AcceptRewardList.Add(m_TempRewardBase);
                }





                //// Debug.LogError(levelRewardList[i].ToString());
                //if (m_TempRewardBase.RewardItem == RewardItemType.Exp_Role || m_TempRewardBase.RewardItem == RewardItemType.Exp_Kongfu)
                //{
                //    levelRewardList.RemoveAt(i);
                //    // Debug.LogError(reward.RewardItem + ":AddEXP:" + reward.Count);
                //    for (int r = 0; r < MainGameMgr.S.BattleFieldMgr.OurCharacterList.Count; r++)
                //    {
                //        if (m_TempRewardBase is Exp_RoleReward)
                //            levelRewardList.Add(new Exp_RoleReward(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, m_TempRewardBase.Count));
                //        else if (m_TempRewardBase is Exp_KongfuRweard)
                //            levelRewardList.Add(new Exp_KongfuRweard(MainGameMgr.S.BattleFieldMgr.OurCharacterList[r].CharacterModel.Id, m_TempRewardBase.Count));
                //    }
                //}
                //else {
                //    m_AcceptRewardList.Add(m_TempRewardBase);
                //}
            }
        }

        public void OnClear() {
            levelRewardList.Clear();
        }
    }

}