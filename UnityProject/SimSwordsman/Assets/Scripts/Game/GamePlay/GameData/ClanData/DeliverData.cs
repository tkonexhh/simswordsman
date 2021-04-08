using Qarth;
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
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == daliverID);

			if (data != null) 
			{
				DaliverDetailDataList.Remove(data);
			}
		}

		public List<SingleDeliverDetailData> GetSingleDeliverDetailDataList()
		{
			return DaliverDetailDataList;
		}

		public SingleDeliverDetailData GetSingleDeliverDetailData(int daliverID)
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == daliverID);
			if (data != null)
			{
				return data;
			}
            else
            {
				Log.e("车队ID未找到，ID = " + daliverID);
				return null;
            }
		}

		public void AddDeliverDisciple(int daliverID, List<int> characterIDList)
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == daliverID);
            if (data!=null)
				data.CharacterIDList = characterIDList;
		}
		public List<int> GetDeliverDisciple(int daliverID)
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == daliverID);
			if (data != null)
				return data.CharacterIDList;
			return null;
		}

		public bool IsGoOutSide(int deliverID) 
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == deliverID);
			if (data != null && data.DaliverState == DeliverState.HasBeenSetOut) 
			{
				return true;
			}
			return false;
		}
		public SingleDeliverDetailData AddDeliverData(int deliverID,DeliverState state,List<DeliverRewadData> rewardDataList,List<int> characterIDList) 
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == deliverID);

			if (data != null) 
			{
				Debug.LogError("已经有相同镖物ID出发了：" + deliverID);
				return null;
			}

			data = new SingleDeliverDetailData(deliverID, state, rewardDataList, characterIDList);

			DaliverDetailDataList.Add(data);			

			return data;
		}
		public SingleDeliverDetailData GetDeliverDataByID(int deliverID) 
		{
			SingleDeliverDetailData data = DaliverDetailDataList.Find(x => x.DeliverID == deliverID);
			return data;
		}
	}
	public class SingleDeliverDetailData
	{
		public int DeliverID;
		public DeliverState DaliverState;
		public List<DeliverRewadData> RewadDataList;
		public List<int> CharacterIDList;
		public DateTime StartTimeWithReally;
		/// <summary>
		/// 加速后结束时间
		/// </summary>
		public DateTime StartTimeWithScal;
		/// <summary>
		/// 实际结束时间
		/// </summary>
		public DateTime EndTimeWithReally;
		/// <summary>
		/// 加速倍数
		/// </summary>
		public int SpeedUpMultiple = 1;

		private int m_CountDownID = -1;

		public SingleDeliverDetailData() { }

		public SingleDeliverDetailData(int deliverID,DeliverState state,List<DeliverRewadData> rewatdDataList,List<int> characterList) 
		{
			this.DeliverID = deliverID;
			this.DaliverState = state;
			this.RewadDataList = rewatdDataList;
			this.CharacterIDList = characterList;

			TDDeliver data = TDDeliverTable.GetData(DeliverID);
			StartTimeWithReally = DateTime.Now;
			//EndTimeWithReally = StartTime.AddMinutes(data.duration);
			EndTimeWithReally = StartTimeWithReally.AddSeconds(50);
			StartTimeWithScal = StartTimeWithReally;

			//Debug.LogError("start time:" + StartTimeWithReally + "     endTime really:" + EndTimeWithReally);
		}
		public bool IsDeliverFinished() 
		{
			return StartTimeWithScal <= DateTime.Now;
		}
		public int GetRemainTimeSeconds() 
		{
			if (SpeedUpMultiple == 1)
			{
				return (int)(EndTimeWithReally - DateTime.Now).TotalSeconds;
			}
			else {
				int passTime = (int)(StartTimeWithScal - StartTimeWithReally).TotalSeconds;
				int totalTime = (int)(EndTimeWithReally - StartTimeWithReally).TotalSeconds;
				int speedUpMultipleTime = (int)(DateTime.Now - StartTimeWithScal).TotalSeconds;

				return totalTime - passTime - speedUpMultipleTime * SpeedUpMultiple;
			}
		}

		public void UpdateSpeedUpMultiple(int speedUpMultiple) 
		{
			this.SpeedUpMultiple = speedUpMultiple;
		}

		public void UpdateSpeedUpMultipleStartTime() 
		{
			StartTimeWithScal = DateTime.Now;

			//int passTime = (int)(StartTimeWithScal - StartTimeWithReally).TotalSeconds;
			//int totalTime = (int)(EndTimeWithReally - StartTimeWithReally).TotalSeconds;
			//DateTime endTime = DateTime.Now.AddSeconds((totalTime - passTime) * .5f);
			//Debug.LogError("start time scal:" + StartTimeWithScal + "     endTime:" + endTime);
		}
		public void UpdateCountDownSpeedUpMultiple() 
		{
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

        public DeliverRewadData(RewardBase rewardBase)
        {
			RewardType = rewardBase.RewardItem;
            if (rewardBase.KeyID==null)
				RewardID = 0;
            else
				RewardID = (int)rewardBase.KeyID;
			RewardCount = rewardBase.Count;
		}

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