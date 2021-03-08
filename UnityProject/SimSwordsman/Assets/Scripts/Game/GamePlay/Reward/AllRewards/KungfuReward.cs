using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuReward : RewardBase
    {
        public KongfuReward(KungfuType id, int count = 1) : base(RewardItemType.Kongfu, (int)id, count) { }
        public KongfuReward(int id, int count = 1) : base(RewardItemType.Kongfu, id, count) { }

        public override void AcceptReward(int bonus = 1)
        {
            if (m_KeyID.HasValue)
            {
                // List<RewardBase> rewards = new List<RewardBase>();
                // rewards.Add(new KongfuReward(RewardItemType.Kongfu, m_KeyID.Value, Count));
                // UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
                MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KungfuType)m_KeyID.Value), Count * bonus);
            }
        }

        public override string RewardName()
        {
            if (m_KeyID.HasValue)
                return TDKongfuConfigTable.GetData(m_KeyID.Value).kongfuName;

            throw new NullReferenceException("m_KeyID");
        }
        public override string SpriteName()
        {
            if (m_KeyID.HasValue)
                return TDKongfuConfigTable.GetIconName((KungfuType)m_KeyID.Value);

            throw new NullReferenceException("m_KeyID");
        }
    }
}