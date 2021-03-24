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
        Busy,
    }


    public class BaseSlot
    {
        public int Index { set; get; }
        public int UnlockLevel { set; get; }
        public CharacterItem CharacterItem { set; get; }
        public SlotState slotState { set; get; }
        public FacilityType FacilityType { set; get; }
        public string StartTime
        {
            get
            {
                if (m_Character != null)
                {
                    return m_Character.CharacterModel.CharacterItem.GetTargetFacilityStartTime();
                }
                return string.Empty;
            }
        }

        private CharacterController m_Character;
        private FacilityView m_FacilityView;

        public BaseSlot(int index, int unlock, FacilityView facilityView)
        {
            Index = index;
            UnlockLevel = unlock;
            m_FacilityView = facilityView;

            CharacterItem = null;
        }

        public bool IsFree()
        {
            return slotState == SlotState.Free;
        }

        public bool IsHaveSameCharacterItem(int id)
        {
            if (CharacterItem != null && CharacterItem.id == id)
                return true;
            return false;
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
            return m_FacilityView.GetSlotPos(Index);
        }

        public virtual float GetProgress()
        {
            return 0;
        }
    }
}