using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum SlotState
    {
        None,
        /// <summary>
        /// ������
        /// </summary>
        Free,
        /// <summary>
        /// δ����
        /// </summary>
        NotUnlocked,
        /// <summary>
        /// ������
        /// </summary>
        CopyScriptures,
        /// <summary>
        /// Ѳ����
        /// </summary>
        Patrol,
        /// <summary>
        /// ������
        /// </summary>
        Practice,
    }


    public class BaseSlot
    {
        public int Index { set; get; }
        public int UnlockLevel { set; get; }
        public CharacterItem CharacterItem { set; get; }
        public SlotState slotState { set; get; }
        public FacilityType FacilityType { set; get; }
        public string StartTime { set; get; }

        // private Vector3 m_SlotPos;
        private CharacterController m_Character;
        private FacilityView m_FacilityView;

        public BaseSlot() { }

        public BaseSlot(SoltDBDataBase soltDBData, FacilityView facilityView)
        {
            m_FacilityView = facilityView;
            FacilityType = soltDBData.facilityType;
            Index = soltDBData.soltID;
            UnlockLevel = soltDBData.unlockLevel;
            slotState = soltDBData.practiceFieldState;
            if (soltDBData.characterID != -1)
                CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(soltDBData.characterID);
            StartTime = soltDBData.startTime;
        }

        public BaseSlot(int index, int unlock)
        {
            Index = index;
            UnlockLevel = unlock;

            CharacterItem = null;
            StartTime = string.Empty;
        }

        public bool IsFree()
        {
            if (slotState == SlotState.Free)
                return true;
            return false;
        }

        public bool IsHaveSameCharacterItem(int id)
        {
            if (CharacterItem != null && CharacterItem.id == id)
                return true;
            return false;
        }

        public void InitSlotState(FacilityLevelInfo item)
        {
            int Level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            if (Level >= item.level)
                slotState = SlotState.Free;
            else
                slotState = SlotState.NotUnlocked;
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
            StartTime = string.Empty;
            m_Character = null;
        }

        public Vector3 GetPosition()
        {
            return m_FacilityView.GetSlotPos(Index);
        }

        public virtual float GetProgress()
        {
            return 0;
        }
    }
}