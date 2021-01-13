using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
	public class PracticeFieldData 
	{

		public List<SoltDBData> practiceFieldDBDatas = new List<SoltDBData>();
		public PracticeFieldData() { }

		public List<SoltDBData> GetPracticeFieldData()
		{
			return practiceFieldDBDatas;
		}

		public void AddPracticeFieldData(PracticeField practiceField)
		{
			SoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
            if (practiceFieldDB==null)
				practiceFieldDBDatas.Add(new SoltDBData(practiceField));
		}

		public void RefreshPracticeState(PracticeField practiceField)
		{
			SoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.practiceFieldState = practiceField.PracticeFieldState;
		}

		public void RefresDBData(PracticeField practiceField)
		{
			SoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.RefresDBData(practiceField);
		}

		public void TrainingIsOver(PracticeField practiceField)
		{
			SoltDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.soltID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.TrainingIsOver();
		}
	}


	public class SoltDBData
	{
		public FacilityType facilityType;
		public SlotState practiceFieldState;
		public int soltID;
		public int characterID = -1;
		public int unlockLevel = -1;
		public string startTime;

		public SoltDBData() { }
		public SoltDBData(PracticeField practiceField) 
		{
			RefresDBData(practiceField);
		}

		public void RefresDBData(PracticeField practiceField)
		{
			unlockLevel = practiceField.UnlockLevel;
			facilityType = practiceField.FacilityType;
			practiceFieldState = practiceField.PracticeFieldState;
			soltID = practiceField.Index;
			if (practiceField.CharacterItem != null)
				characterID = practiceField.CharacterItem.id;
			startTime = practiceField.StartTime;
		}

		public void TrainingIsOver()
		{
			startTime = string.Empty;
			characterID = -1;
			practiceFieldState = SlotState.Free;
		}
	}
}