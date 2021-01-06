using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FacilitySlot
    {
        private Vector3 m_SlotPos;
        private CharacterController m_Character;

        public FacilitySlot(Vector3 pos)
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