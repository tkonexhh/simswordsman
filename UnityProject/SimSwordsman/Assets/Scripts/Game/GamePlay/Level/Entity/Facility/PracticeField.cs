using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PracticeField : CDBaseSlot
    {

        public PracticeField(PracticeFieldLevelInfo item, int index, int unlock) : base(index, unlock)
        {
            FacilityType = item.GetHouseID();
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddPracticeFieldData(this);
        }

        public PracticeField(PracticeSoltDBData item, FacilityView facilityView) : base(item, facilityView)
        {
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);

            StartTime = DateTime.Now.ToString();
            CharacterItem = characterItem;
            characterController.SetState(CharacterStateID.Practice, targetFacility);

            base.slotState = SlotState.Practice;
            GameDataMgr.S.GetClanData().RefresPracticeDBData(this);
        }

        protected override void OnCDOver()
        {
            GameDataMgr.S.GetClanData().PraceTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, this);
        }
    }

}