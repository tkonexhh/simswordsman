using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraySlot : CDBaseSlot
    {
        public KungfuLibraySlot(KongfuLibraryLevelInfo item, int index, int unLock, FacilityView facilityView) : base(index, unLock, facilityView)
        {
            FacilityType = FacilityType.KongfuLibrary;
            UnlockLevel = CalcUnlockLvl();
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            CharacterItem = characterItem;
            characterController.SetState(CharacterStateID.Reading, targetFacility, System.DateTime.Now.ToString(), Index);

            base.slotState = SlotState.Busy;
        }

        protected override void OnCDOver()
        {
            EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, this);
        }

        private int CalcUnlockLvl()
        {
            return 1;
        }
    }
}