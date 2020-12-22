using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class EquipReward : LevelReward
    {
        private int m_ItemId;
        private int m_Number;

        public EquipReward(LevelRewardType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ItemId = int.Parse(paramStrs[1]);
            m_Number = int.Parse(paramStrs[2]);
        }

        public override void ApplyReward()
        {
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem()); //TODO
        }
    }

}