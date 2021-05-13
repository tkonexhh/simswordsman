using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PracticeField : CDBaseSlot
    {
        public PracticeField(PracticeFieldLevelInfo item, int index, FacilityView facilityView) : base(index, facilityView)
        {
            FacilityType = item.GetHouseID();
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            try
            {
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
                CharacterItem = characterItem;
                characterController.SetState(CharacterStateID.Practice, targetFacility, System.DateTime.Now.ToString(), Index);
                base.slotState = SlotState.Busy;
                GameDataMgr.S.GetPlayerData().recordData.AddPractice();
            }
            catch (Exception e)
            {
                Debug.LogError("PracticeField" + e.ToString());
            }
        }

        protected override void OnCDOver()
        {
            EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, this);
        }
    }

}