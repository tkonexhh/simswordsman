using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class CoinRewardHandler : RewardHandlerBase<double>
	{
        public override void OnRewardClaimed()
        {
            base.OnRewardClaimed();

            GameDataMgr.S.GetPlayerData().AddCoinNum(m_Value);
        }
    }
	
}