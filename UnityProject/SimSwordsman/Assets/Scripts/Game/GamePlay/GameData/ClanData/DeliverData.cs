using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	[System.Serializable]
	public class DeliverData
	{
		public List<SingleDeliverDetailData> DaliverDetailDataList = new List<SingleDeliverDetailData>();

		public void RemoveDeliverData(int daliverID) 
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DaliverID == daliverID);

			if (data != null) 
			{
				DaliverDetailDataList.Remove(data);
			}
		}

		public SingleDeliverDetailData AddOrUpdateDeliverData(int daliverID,DeliverState state,List<DeliverRewadData> rewardDataList,List<int> characterIDList) 
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DaliverID == daliverID);
			if (data != null)
			{
				data.DaliverState = state;
				data.RewadDataList = rewardDataList;
				data.StartTime = DateTime.Now;
				data.CharacterIDList = characterIDList;
			}
			else {
				data = new SingleDeliverDetailData();
				data.DaliverID = daliverID;
				data.DaliverState = state;
				data.RewadDataList = rewardDataList;
				data.StartTime = DateTime.Now;
				data.CharacterIDList = characterIDList;

				DaliverDetailDataList.Add(data);
			}

			return data;
		}
	}
	public class SingleDeliverDetailData 
	{
		public int DaliverID;
		public DeliverState DaliverState;
		public List<DeliverRewadData> RewadDataList;
		public List<int> CharacterIDList;
		public DateTime StartTime;
		/// <summary>
		/// 加速倍数
		/// </summary>
		public int SpeedUpMultiple = 1;

		private int m_CountDownID = -1;

		private DateTime EndTime;

		public int GetTotalTimeSeconds() 
		{
			TDDeliver data = TDDeliverTable.GetData(DaliverID);
            //DateTime endTime = StartTime.AddMinutes(data.duration);
            DateTime endTime = StartTime.AddSeconds(30);
			EndTime = endTime;

			return (int)((endTime - StartTime).TotalSeconds / SpeedUpMultiple);
		}

		public int GetRemainTimeSeconds() 
		{
			return (int)(EndTime - DateTime.Now).TotalSeconds;
		}

		public void UpdateSpeedUpMultiple(int speedUpMultiple) 
		{
			this.SpeedUpMultiple = speedUpMultiple;
			if (m_CountDownID != -1) 
			{
				CountDownItemTest item = CountDowntMgr.S.GetCountDownItemByID(m_CountDownID);
				if (item != null) 
				{
					item.SetSpeedUpMultiply(this.SpeedUpMultiple);
				}
			}
		}

		public void SetCountDownID(int countDownID) 
		{
			this.m_CountDownID = countDownID;
		}
		public int GetCountDownID() 
		{
			return m_CountDownID;
		}
    }
	public class DeliverRewadData 
	{
		public RewardItemType RewardType;
		public int RewardID;
		public int RewardCount;

		public DeliverRewadData() { }

		public DeliverRewadData(RewardItemType rewardType,int rewardID,int rewardCount) {
			this.RewardType = rewardType;
			this.RewardID = rewardID;
			this.RewardCount = rewardCount;
		}
	}
	public enum DeliverState 
	{
		/// <summary>
		/// 未解锁
		/// </summary>
		Unlock,         
		/// <summary>
		/// 已出发
		/// </summary>
		HasBeenSetOut,  
		/// <summary>
		/// 未出发
		/// </summary>
		DidNotSetOut,	
	}
}