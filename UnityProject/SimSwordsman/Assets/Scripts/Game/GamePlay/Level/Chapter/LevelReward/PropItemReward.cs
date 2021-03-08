using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PropItemReward : LevelReward
    {
        private int m_ItemId;
        private int m_Number;

        public PropItemReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ItemId = int.Parse(paramStrs[1]);
            m_Number = int.Parse(paramStrs[2]);
        }

        public PropItemReward(RewardItemType rewardType, int itemID,int itemNumber) 
        {
            this.rewardType = rewardType;
            m_ItemId = itemID;
            m_Number = itemNumber;
        }
        public override void ApplyReward(int par)
        {
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)m_ItemId), m_Number);
            EventSystem.S.Send(EventID.OnChallengeReward, rewardType, m_ItemId, m_Number);
        }

        public override int GetRewardValue()
        {
            return m_Number;
        }
    }

}