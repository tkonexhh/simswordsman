using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraySlot : CDBaseSlot
    {
        public KungfuLibraySlot()
        {
        }

        public KungfuLibraySlot(KongfuLibraryLevelInfo item, int index, int unLock, FacilityView facilityView) : base(index, unLock, facilityView)
        {
            FacilityType = FacilityType.KongfuLibrary;
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddKungfuLibraryData(this);
        }

        public KungfuLibraySlot(KungfuSoltDBData soltDBData, FacilityView facilityView) : base(soltDBData, facilityView)
        {
        }

        public void Warp(KongfuLibraryLevelInfo kongfuLibrary)
        {
            slotState = SlotState.Free;
            UnlockLevel = kongfuLibrary.level;
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);

            StartTime = DateTime.Now.ToString();
            CharacterItem = characterItem;
            characterController.SetState(CharacterStateID.Reading, targetFacility);

            base.slotState = SlotState.CopyScriptures;
            GameDataMgr.S.GetClanData().RefresKungfuDBData(this);
        }

        protected override void OnCDOver()
        {
            GameDataMgr.S.GetClanData().KungfuTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, this);
        }
    }
}