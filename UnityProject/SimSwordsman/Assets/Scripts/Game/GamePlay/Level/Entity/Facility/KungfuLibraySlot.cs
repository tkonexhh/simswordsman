using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraySlot : CDBaseSlot
    {
        // public KungfuLibraySlot()
        // {
        // }

        public KungfuLibraySlot(KongfuLibraryLevelInfo item, int index, int unLock, FacilityView facilityView) : base(index, unLock, facilityView)
        {
            FacilityType = FacilityType.KongfuLibrary;
            InitSlotState(item);
            // GameDataMgr.S.GetClanData().AddKungfuLibraryData(this);
        }


        public void Warp(KongfuLibraryLevelInfo kongfuLibrary)
        {
            slotState = SlotState.Free;
            UnlockLevel = kongfuLibrary.level;
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            CharacterItem = characterItem;
            characterController.SetState(CharacterStateID.Reading, targetFacility, System.DateTime.Now.ToString(), Index);

            base.slotState = SlotState.CopyScriptures;
            // GameDataMgr.S.GetClanData().RefresKungfuDBData(this);
        }

        protected override void OnCDOver()
        {
            return;
            // GameDataMgr.S.GetClanData().KungfuTrainingIsOver(this);
            //根据SoltID 和Type 来挑选Character
            var character = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Find(c => c.GetTargetFacilityType() == FacilityType.KongfuLibrary && c.GetTargetFacilityIndex() == Index);
            if (character != null)
            {
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(character.id);
                characterController.SetState(CharacterStateID.Wander);
            }
            // CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, this);
        }
    }
}