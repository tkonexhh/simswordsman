using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
	public class PracticeFieldData 
	{

		public List<PracticeSoltDBData> practiceFieldDBDatas = new List<PracticeSoltDBData>();
		public PracticeFieldData() { }

		public List<PracticeSoltDBData> GetPracticeFieldData()
		{
			return practiceFieldDBDatas;
		}

		public void AddPracticeFieldData(PracticeField practiceField)
		{
			PracticeSoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
            if (practiceFieldDB==null)
				practiceFieldDBDatas.Add(new PracticeSoltDBData(practiceField));
		}

		public void RefreshPracticeState(PracticeField practiceField)
		{
			PracticeSoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.practiceFieldState = practiceField.slotState;
		}

		public void RefresDBData(PracticeField practiceField)
		{
			PracticeSoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.RefresDBData(practiceField);
		}

		public void TrainingIsOver(PracticeField practiceField)
		{
			PracticeSoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.TrainingIsOver();
		}
	}


	public class PracticeSoltDBData: SoltDBDataBase
	{
		public PracticeSoltDBData() { }
		public PracticeSoltDBData(PracticeField practiceField) :base(practiceField)
		{
		}
	}
	[Serializable]
	public class SoltDBDataBase
	{

		public FacilityType facilityType;
		public SlotState practiceFieldState;
		public int soltID;
		public int characterID = -1;
		public int unlockLevel = -1;
		public string startTime;
		public SoltDBDataBase() { }

		public SoltDBDataBase(BaseSlot baseSlot)
		{
			RefresDBData(baseSlot);
		}
		public void RefresDBData(BaseSlot baseSlot)
		{
			unlockLevel = baseSlot.UnlockLevel;
			facilityType = baseSlot.FacilityType;
			practiceFieldState = baseSlot.slotState;
			soltID = baseSlot.Index;
			if (baseSlot.CharacterItem != null)
				characterID = baseSlot.CharacterItem.id;
			startTime = baseSlot.StartTime;
		}

		public void TrainingIsOver()
		{
			startTime = string.Empty;
			characterID = -1;
			practiceFieldState = SlotState.Free;
		}
	}
}