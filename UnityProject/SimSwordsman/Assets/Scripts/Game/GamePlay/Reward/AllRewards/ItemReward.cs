using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ItemReward : RewardBase
    {
        public ItemReward(int id, int count) : base(RewardItemType.Item, id, count) { }

        public override void AcceptReward()
        {
            //Log.e("???" + m_TDItem.name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)m_KeyID), Count);
        }

        public override string RewardName()
        {
            if (m_KeyID.HasValue)
                return TDItemConfigTable.GetData(m_KeyID.Value).name;

            throw new NullReferenceException("m_KeyID");
        }

        public override string SpriteName()
        {
            if (m_KeyID.HasValue)
                return TDItemConfigTable.GetData(m_KeyID.Value).iconName;

            throw new NullReferenceException("m_KeyID");
        }
    }
}