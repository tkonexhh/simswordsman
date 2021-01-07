using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
	public class PracticeFieldData
	{

		public List<PracticeFieldDBData> practiceFieldDBDatas = new List<PracticeFieldDBData>();
		public PracticeFieldData() { }

		public List<PracticeFieldDBData> GetPracticeFieldData()
		{
			return practiceFieldDBDatas;
		}

		public void AddPracticeFieldData(PracticeField practiceField)
		{
			PracticeFieldDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.pitPositionID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB == null)
				practiceFieldDBDatas.Add(new PracticeFieldDBData(practiceField));
		}

		public void RefreshPracticeState(PracticeField practiceField)
		{
			PracticeFieldDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.pitPositionID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.practiceFieldState = practiceField.PracticeFieldState;
		}

		public void RefresDBData(PracticeField practiceField)
		{
			PracticeFieldDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.pitPositionID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.RefresDBData(practiceField);
		}

		public void TrainingIsOver(PracticeField practiceField)
		{
			PracticeFieldDBData practiceFieldDB = practiceFieldDBDatas.Where(i => i.facilityType == practiceField.FacilityType && i.pitPositionID == practiceField.Index).FirstOrDefault();
			if (practiceFieldDB != null)
				practiceFieldDB.TrainingIsOver();
		}
	}


	public class PracticeFieldDBData
	{
		public FacilityType facilityType;
		public PracticeFieldState practiceFieldState;
		public int pitPositionID;
		public int characterID = -1;
		public int unlockLevel = -1;
		public string startTime;

		public PracticeFieldDBData() { }
		public PracticeFieldDBData(PracticeField practiceField)
		{
			RefresDBData(practiceField);
		}

		public void RefresDBData(PracticeField practiceField)
		{
			unlockLevel = practiceField.UnlockLevel;
			facilityType = practiceField.FacilityType;
			practiceFieldState = practiceField.PracticeFieldState;
			pitPositionID = practiceField.Index;
			if (practiceField.CharacterItem != null)
				characterID = practiceField.CharacterItem.id;
			startTime = practiceField.StartTime;
		}

		public void TrainingIsOver()
		{
			startTime = string.Empty;
			characterID = -1;
			practiceFieldState = PracticeFieldState.Free;
		}
	}
}