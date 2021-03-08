using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class EquipReward : LevelReward
    {
        private int m_ItemId;
        private int m_Number;

        public EquipReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ItemId = int.Parse(paramStrs[1]);
            m_Number = int.Parse(paramStrs[2]);
        }

        public override void ApplyReward(int par)
        {
            if (rewardType == RewardItemType.Armor)
            {
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType) m_ItemId, Step.One), m_Number);
            }
            else if (rewardType == RewardItemType.Arms)
            {
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)m_ItemId, Step.One), m_Number);
            }

            EventSystem.S.Send(EventID.OnChallengeReward, rewardType, m_ItemId, m_Number);
        }

        public override int GetRewardValue()
        {
            return m_Number;
        }
    }

}