using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class PatrolRoomSlot : BaseSlot
	{
        public PatrolRoomSlot(PatrolRoomSoltDBData soltDBData,FacilityView facilityView) : base(soltDBData, facilityView)
        {
      
        }
        public PatrolRoomSlot(PatrolRoomInfo item, int index, int unLock) : base(index, unLock)
        {
            FacilityType = FacilityType.KongfuLibrary;
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddPatrolRoomData(this);
        }
        public PatrolRoomSlot()
        {
        }
        public void Warp(PatrolRoomInfo patrolRoomInfo)
        {
            slotState = SlotState.Free;
            UnlockLevel = patrolRoomInfo.level;
        }

        public PatrolRoomSlot(KongfuLibraryLevelInfo item, int index, int unLock) : base(index, unLock)
        {
            FacilityType = FacilityType.KongfuLibrary;
            InitSlotState(item);
            //GameDataMgr.S.GetClanData().AddKungfuLibraryData(this);
        }
        public PatrolRoomSlot(PatrolRoomInfo patrolRoomInfo)
        {
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
                case SlotState.Patrol:
                    ReturnDisciple();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Patrol, targetFacility);
                    break;
            }
            base.slotState = slotState;
            GameDataMgr.S.GetClanData().RefresPatrolRoomDBData(this);
        }

        /// <summary>
        /// 归还上一个弟子
        /// </summary>
        private void ReturnDisciple()
        {
            if (CharacterItem!=null && slotState== SlotState.Patrol)
            {
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(CharacterItem.id);
                characterController.SetState(CharacterStateID.Wander);
            }
        }
    }
}