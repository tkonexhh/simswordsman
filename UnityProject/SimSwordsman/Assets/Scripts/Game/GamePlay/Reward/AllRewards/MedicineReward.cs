using System;
using UnityEngine;


namespace GameWish.Game
{
    public class MedicineReward : RewardBase
    {
        public MedicineReward(HerbType id, int count) : base(RewardItemType.Medicine, (int)id, count) { }
        public MedicineReward(int id, int count) : base(RewardItemType.Medicine, id, count) { }

        public override void AcceptReward(int bonus = 1)
        {
            //Log.e("???" + m_Info.Name + m_Count);
            if (m_KeyID.HasValue)
                MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)m_KeyID.Value, Count * bonus));
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

        public override string RewardTips()
        {
            if (m_KeyID.HasValue)
                return TDHerbConfigTable.GetData(m_KeyID.Value).desc;
            return "";
        }
    }
}