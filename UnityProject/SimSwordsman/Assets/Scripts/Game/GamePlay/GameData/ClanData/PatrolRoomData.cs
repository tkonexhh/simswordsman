using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
	public class PatrolRoomData 
	{
		public List<PatrolRoomSoltDBData> patrolRoomSoltDBData = new List<PatrolRoomSoltDBData>();
		public PatrolRoomData() { }

		public List<PatrolRoomSoltDBData> GetPatrolRoomData()
		{
			return patrolRoomSoltDBData;
		}
		/// <summary>
		/// 添加练兵场坑位信息
		/// </summary>
		/// <param name="patrolRoomSlot"></param>
		public void AddPatrolRoomData(PatrolRoomSlot patrolRoomSlot)
		{
			PatrolRoomSoltDBData patrolRoom = patrolRoomSoltDBData.Where(i => i.facilityType == patrolRoomSlot.FacilityType && i.soltID == patrolRoomSlot.Index).FirstOrDefault();
			if (patrolRoom == null)
				patrolRoomSoltDBData.Add(new PatrolRoomSoltDBData(patrolRoomSlot));
		}

		/// <summary>
		/// 刷新练兵场数据信息
		/// </summary>
		/// <param name="patrolRoomSlot"></param>
		public void RefresDBData(PatrolRoomSlot patrolRoomSlot)
		{
			PatrolRoomSoltDBData patrolRoom = patrolRoomSoltDBData.Where(i => i.facilityType == patrolRoomSlot.FacilityType && i.soltID == patrolRoomSlot.Index).FirstOrDefault();
			if (patrolRoom != null)
				patrolRoom.RefresDBData(patrolRoomSlot);
		}

		public void TrainingIsOver(PatrolRoomSlot patrolRoomSlot)
		{
			PatrolRoomSoltDBData patrolRoom = patrolRoomSoltDBData.Where(i => i.facilityType == patrolRoomSlot.FacilityType && i.soltID == patrolRoomSlot.Index).FirstOrDefault();
			if (patrolRoom != null)
				patrolRoom.TrainingIsOver();
		}
	}
	[Serializable]
	public class PatrolRoomSoltDBData : SoltDBDataBase
	{
		public PatrolRoomSoltDBData() { }
		public PatrolRoomSoltDBData(PatrolRoomSlot patrolRoomSlot) : base(patrolRoomSlot)
		{
		}
	}
}