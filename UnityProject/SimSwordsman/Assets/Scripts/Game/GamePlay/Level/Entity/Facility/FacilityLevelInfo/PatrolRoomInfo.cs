using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class PatrolRoomInfo : FacilityLevelInfo
	{
		public int UnlockPatrolSeat { set; get; }

		public string UnlockTower { set; get; }


		public void InitPatrolRoomInfo(TDFacilityPatrolRoom tdData)
		{
			UnlockPatrolSeat = tdData.unlockDefendant;
			UnlockTower = tdData.unlockTower;
		}

		public int GetCurCapacity()
        {
			return UnlockPatrolSeat;

		}
    }
}