using System;
using UnityEngine;


namespace GameWish.Game
{
    public class MedicineReward : RewardBase
    {
        public MedicineReward(int id, int count) : base(RewardItemType.Medicine, id, count) { }


        public override void AcceptReward()
        {
            //Log.e("???" + m_Info.Name + m_Count);
            //MainGameMgr.S.MedicinalPowderMgr.AddHerb(m_KeyID, Count);
            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)m_KeyID, Count));
        }

        public override string RewardName()
        {
            if (m_KeyID.HasValue)
                return TDHerbConfigTable.GetData(m_KeyID.Value).name;

            throw new NullReferenceException("m_KeyID");
        }

        public override string SpriteName()
        {
            if (m_KeyID.HasValue)
                return TDHerbConfigTable.GetData(m_KeyID.Value).icon;

            throw new NullReferenceException("m_KeyID");
        }
    }
}