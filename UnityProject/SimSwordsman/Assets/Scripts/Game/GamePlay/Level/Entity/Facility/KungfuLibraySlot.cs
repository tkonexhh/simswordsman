using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraySlot: BaseSlot
    {
        public KungfuLibraySlot(KungfuSoltDBData soltDBData):base(soltDBData)
        {
            if (slotState == SlotState.CopyScriptures)
                InitTimerUpdate();
        }
        public override float GetProgress() 
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(FacilityType, level);

            int remainingTime = duration - ComputingTime(StartTime);

            return  1 - (remainingTime * 1.0f) / duration;
        }
        public int GetDurationTime()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(FacilityType, level);
            int takeTime = ComputingTime(StartTime);
            return duration - takeTime;
        }
        private int ComputingTime(string time)
        {
            DateTime dateTime;
            DateTime.TryParse(time, out dateTime);
            if (dateTime != null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalSeconds;
            }
            return 0;
        }

        private void InitTimerUpdate()
        {
            CountDownItem countDownMgr = new CountDownItem(FacilityType.ToString() + Index, GetDurationTime());
            countDownMgr.OnCountDownOverEvent = overAction;

            TimeUpdateMgr.S.AddObserver(countDownMgr);
        }

        public void Warp(KongfuLibraryLevelInfo kongfuLibrary)
        {
            slotState = SlotState.Free;
            UnlockLevel = kongfuLibrary.level;
        }

        public KungfuLibraySlot(KongfuLibraryLevelInfo item, int index,int unLock):base( index, unLock)
        {
            FacilityType = FacilityType.KongfuLibrary;
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddKungfuLibraryData(this);
        }
        public KungfuLibraySlot()
        {
        }
 
        public void SetCharacterItem(CharacterItem characterItem, SlotState slotState, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            switch (slotState)
            {
                case SlotState.Free:
                    characterController.SetState(CharacterStateID.Wander);
                    break;
                case SlotState.CopyScriptures:
                    StartTime = DateTime.Now.ToString();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Reading, targetFacility);
                    break;
            }
            base.slotState = slotState;
            GameDataMgr.S.GetClanData().RefresKungfuDBData(this);
        }
     
        public void overAction()
        {
            if (CharacterItem != null)
            {
                RewardKungfu(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType));
                TrainingIsOver();
            }
        }
        public void TrainingIsOver()
        {
            SetCharacterItem(CharacterItem, SlotState.Free, FacilityType.None);
            CharacterItem = null;
            StartTime = string.Empty;
            GameDataMgr.S.GetClanData().KungfuTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, this);
        }
    }
}