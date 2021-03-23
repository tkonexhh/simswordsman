using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PatrolRoomSlot : BaseSlot
    {
        public PatrolRoomSlot(PatrolRoomInfo item, int index, int unLock, FacilityView facilityView) : base(index, unLock, facilityView)
        {
            FacilityType = FacilityType.KongfuLibrary;
        }
        public void Warp(PatrolRoomInfo patrolRoomInfo)
        {
            slotState = SlotState.Free;
            UnlockLevel = patrolRoomInfo.level;
        }

        public void SetCharacterItem(CharacterItem characterItem, SlotState slotState, FacilityType targetFacility)
        {
            //StartTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(curFacilityType, curLevel);
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            switch (slotState)
            {
                case SlotState.Free:
                    characterController.SetState(CharacterStateID.Wander);
                    break;
                case SlotState.Busy:
                    ReturnDisciple();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Patrol, targetFacility);
                    break;
            }
            base.slotState = slotState;
        }

        /// <summary>
        /// 归还上一个弟子
        /// </summary>
        private void ReturnDisciple()
        {
            if (CharacterItem != null && slotState == SlotState.Busy)
            {
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(CharacterItem.id);
                characterController.SetState(CharacterStateID.Wander);
            }
        }
    }
}