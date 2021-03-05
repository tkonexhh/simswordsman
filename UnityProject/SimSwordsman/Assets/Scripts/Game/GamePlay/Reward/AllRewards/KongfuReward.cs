using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuReward : RewardBase
    {
        public KongfuReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(new KongfuReward(RewardItemType.Kongfu, m_KeyID, Count));
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KungfuType)m_KeyID), Count);
        }

        public override string RewardName()
        {
            return TDKongfuConfigTable.GetData(m_KeyID).kongfuName;
        }
        public override string SpriteName()
        {
            return TDKongfuConfigTable.GetIconName((KungfuType)m_KeyID);
        }
    }
}