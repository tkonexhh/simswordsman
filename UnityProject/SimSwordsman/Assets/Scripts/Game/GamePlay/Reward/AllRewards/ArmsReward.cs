using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ArmsReward : RewardBase
    {
        public ArmsReward(int id, int count) : base(RewardItemType.Arms, id, count) { }

        public override void AcceptReward()
        {
            if (m_KeyID.HasValue)
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)m_KeyID.Value, Step.One), Count);
        }

        public override string RewardName()
        {
            if (m_KeyID.HasValue)
                return TDEquipmentConfigTable.GetData(m_KeyID.Value).name;
            throw new NullReferenceException("m_KeyID");
        }

        public override string SpriteName()
        {
            if (m_KeyID.HasValue)
                return TDEquipmentConfigTable.GetData(m_KeyID.Value).iconName;
            throw new NullReferenceException("m_KeyID");
        }
    }

}