using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class ArmorReward : RewardBase
    {
        public ArmorReward(int id, int count) : base(RewardItemType.Armor, id, count)
        {

        }

        public override void AcceptReward()
        {
            if (m_KeyID.HasValue)
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)m_KeyID.Value, Step.One), Count);
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