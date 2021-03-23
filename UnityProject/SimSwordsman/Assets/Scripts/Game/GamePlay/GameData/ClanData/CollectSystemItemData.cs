using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class CollectSystemItemData
	{
		public int ID;
		public DateTime StartTime;

		private TDCollectConfig m_ConfigData
        {
			get {
				return TDCollectConfigTable.GetData(ID);
			}
		}

		public CollectSystemItemData() { }

		public CollectSystemItemData(int id,DateTime StartTime) 
		{
			ID = id;
			this.StartTime = StartTime;
		}

		public int GetRewardCount() 
		{
			int rewardCount = 0;

			if (m_ConfigData != null) 
			{
				int totalMinutes = (int)(DateTime.Now - StartTime).TotalSeconds;

				rewardCount = totalMinutes / m_ConfigData.productTime;

				rewardCount = Mathf.Clamp(rewardCount, 0, m_ConfigData.maxStore);
			}

			return rewardCount;
		}

		public bool IsCanShowBubbleIcon() 
		{
			return GetRewardCount() > 0;
		}

		public int GetRemainTimeWhenBubbleShow() 
		{
			int time = (int)(StartTime.AddSeconds(m_ConfigData.productTime) - StartTime).TotalSeconds;
			return time;
		}

		public void Clear() 
		{
			int rewardCount = GetRewardCount();
			if (rewardCount < m_ConfigData.maxStore)
			{
				StartTime = StartTime.AddSeconds(rewardCount * m_ConfigData.productTime);
			}
			else 
			{
				StartTime = DateTime.Now;
			}
		}
	}	
}