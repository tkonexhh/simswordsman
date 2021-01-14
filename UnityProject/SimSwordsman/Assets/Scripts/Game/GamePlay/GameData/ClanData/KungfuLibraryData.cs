using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
	public class KungfuLibraryData  
	{
		public List<kungfuSoltDBData> kungfuLibraryDBDatas = new List<kungfuSoltDBData>();
		public KungfuLibraryData() { }

		public List<kungfuSoltDBData> GetKungfuLibrayData()
		{
			return kungfuLibraryDBDatas;
		}
		/// <summary>
		/// 添加藏经阁坑位信息
		/// </summary>
		/// <param name="kungfuLibraySlot"></param>
		public void AddKungfuLibrayData(KungfuLibraySlot kungfuLibraySlot)
		{
			kungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
			if (kungfuLibraySlotDB == null)
				kungfuLibraryDBDatas.Add(new kungfuSoltDBData(kungfuLibraySlot));
		}

		public void RefreshPracticeState(PracticeField kungfuLibraySlot)
		{
			//kungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
			//if (kungfuLibraySlotDB != null)
			//	kungfuLibraySlotDB.kungfuLibraySlotState = kungfuLibraySlot.PracticeFieldState;
		}

		/// <summary>
		/// 刷新藏经阁数据信息
		/// </summary>
		/// <param name="kungfuLibraySlot"></param>
		public void RefresDBData(KungfuLibraySlot kungfuLibraySlot)
		{
			kungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
			if (kungfuLibraySlotDB != null)
				kungfuLibraySlotDB.RefresDBData(kungfuLibraySlot);
		}

		public void TrainingIsOver(KungfuLibraySlot kungfuLibraySlot)
		{
			kungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
			if (kungfuLibraySlotDB != null)
				kungfuLibraySlotDB.TrainingIsOver();
		}
	}

	[Serializable]
	public class kungfuSoltDBData: SoltDBDataBase
	{
		public kungfuSoltDBData() { }
		public kungfuSoltDBData(KungfuLibraySlot kungfuLibraySlot) : base(kungfuLibraySlot)
		{
		}
	}
}