using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ArmsReward : RewardBase
    {
        public ArmsReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)m_KeyID, Step.One), Count);
        }

        public override string RewardName()
        {
            return TDEquipmentConfigTable.GetData(m_KeyID).name;
        }

        public override string SpriteName()
        {
            return TDEquipmentConfigTable.GetData(m_KeyID).iconName;
        }
    }

}