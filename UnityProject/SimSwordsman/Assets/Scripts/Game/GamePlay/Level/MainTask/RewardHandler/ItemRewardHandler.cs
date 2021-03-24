using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class ItemRewardHandler : RewardHandlerBase
	{
        public override void OnRewardClaimed()
        {
            //base.OnRewardClaimed();

            //MainTaskItemInfo item = TDMainTaskTable.GetMainTaskItemInfo(m_TaskId);
            //int itemId = item.GetRewardId(0);
            //int count = item.GetRewardValue(0);
            //MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
        }
    }
	
}