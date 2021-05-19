using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class ArmorReward : RewardBase
    {
        public ArmorReward(ArmorType id, int count) : base(RewardItemType.Armor, (int)id, count) { }
        public ArmorReward(int id, int count) : base(RewardItemType.Armor, id, count) { }

        public override void AcceptReward(int bonus = 1)
        {
            if (m_KeyID.HasValue)
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)m_KeyID.Value, Step.One), Count * bonus);
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

        public override string RewardTips()
        {
            Equipment info = TDEquipmentConfigTable.GetEquipmentInfo(KeyID.Value);

            if (info != null)
                return info.Desc;

            return "";
        }
    }

}