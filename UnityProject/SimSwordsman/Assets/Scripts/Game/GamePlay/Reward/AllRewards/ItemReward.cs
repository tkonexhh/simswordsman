using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ItemReward : RewardBase
    {
        public ItemReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            //Log.e("???" + m_TDItem.name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)m_KeyID), Count);
        }

        public override string RewardName()
        {
            return TDItemConfigTable.GetData(m_KeyID).name;
        }

        public override string SpriteName()
        {
            return TDItemConfigTable.GetData(m_KeyID).iconName;
        }
    }
}