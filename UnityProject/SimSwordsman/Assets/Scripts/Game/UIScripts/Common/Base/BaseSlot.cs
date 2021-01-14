using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class BaseSlot 
	{
		public int Index { set; get; }
		public int UnlockLevel { set; get; }
		public CharacterItem CharacterItem { set; get; }
		public SlotState slotState { set; get; }
		public FacilityType FacilityType { set; get; }
		public string StartTime { set; get; }
		public BaseSlot()
		{
		}
		public BaseSlot(SoltDBDataBase soltDBData)
		{
			FacilityType = soltDBData.facilityType;
			Index = soltDBData.soltID;
			UnlockLevel = soltDBData.unlockLevel;
			slotState = soltDBData.practiceFieldState;
			if (soltDBData.characterID != -1)
				CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(soltDBData.characterID);
			StartTime = soltDBData.startTime;
		}

		public BaseSlot(FacilityLevelInfo item, int index,int unlock)
		{
			FacilityType = FacilityType.KongfuLibrary;
			Index = index;
			UnlockLevel = unlock;
			int practiceFieldLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
			if (practiceFieldLevel >= item.level)
				slotState = SlotState.Free;
			else
				slotState = SlotState.NotUnlocked;
			CharacterItem = null;
			StartTime = string.Empty;
		}

		public void AddExperience(CharacterItem characterItem)
		{
			int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
			int exp = MainGameMgr.S.FacilityMgr.GetExpValue(FacilityType, level);
			characterItem.AddCharacterExp(exp);
		}

	}
}