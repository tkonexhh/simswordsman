using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerRewardConfig
    {
        private List<String> m_RewardLst;
        public void Reset()
        {
            m_RewardLst = Helper.String2ListString(reward, ";");
        }

        public string GetRandomRewardStr()
        {
            return m_RewardLst[RandomHelper.Range(0, m_RewardLst.Count)]; ;
        }

        public RewardBase GetRandomReward()
        {
            var rewardStr = m_RewardLst[RandomHelper.Range(0, m_RewardLst.Count)];
            return RewardMgr.S.GetRewardBase(rewardStr);
        }
    }
}