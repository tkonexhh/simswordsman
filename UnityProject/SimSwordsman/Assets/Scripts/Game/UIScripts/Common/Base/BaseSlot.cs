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

		private Vector3 m_SlotPos;
		private CharacterController m_Character;
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

		/// <summary>
		/// ½±Àø¾­Ñé
		/// </summary>
		/// <param name="characterItem"></param>
		public void AddExperience(CharacterItem characterItem)
		{
			int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
			int exp = MainGameMgr.S.FacilityMgr.GetExpValue(FacilityType, level);
			characterItem.AddCharacterExp(exp);
		}

		public void RewardKungfu(int kungfuLibraryLevel)
		{
			int kungfuID = (int)MainGameMgr.S.FacilityMgr.GetKungfuForWeightAndLevel(kungfuLibraryLevel);
			RewardBase reward = RewardMgr.S.GetRewardBase(RewardItemType.Kongfu, kungfuID,1);
			reward.AcceptReward();
		}

		public BaseSlot(Vector3 pos)
		{
			m_SlotPos = pos;
		}

		public void SetSlotPos(Vector3 pos)
		{
			m_SlotPos = pos;
		}

		public bool IsEmpty()
		{
			return m_Character == null;
		}

		public void OnCharacterEnter(CharacterController character)
		{
			m_Character = character;
		}

		public void OnCharacterLeave()
		{
			m_Character = null;
		}

		public Vector3 GetPosition()
		{
			return m_SlotPos;
		}
	}
}