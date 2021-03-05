using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class ArmorReward : RewardBase
    {
        public ArmorReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            //Log.e("???" + m_Equip.Name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)m_KeyID, Step.One), Count);
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