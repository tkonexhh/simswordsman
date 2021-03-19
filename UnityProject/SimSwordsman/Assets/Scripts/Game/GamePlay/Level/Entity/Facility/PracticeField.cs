using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PracticeField : BaseSlot
    {

        public PracticeField(PracticeFieldLevelInfo item, int index, int unlock) : base(index, unlock)
        {
            FacilityType = item.GetHouseID();
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddPracticeFieldData(this);
        }

        public PracticeField(PracticeSoltDBData item, FacilityView facilityView) : base(item, facilityView)
        {
            if (slotState == SlotState.Practice)
                InitTimerUpdate();
        }

        private void InitTimerUpdate()
        {
            CountDownItem countDownMgr = new CountDownItem(FacilityType.ToString() + Index, GetDurationTime());
            countDownMgr.OnCountDownOverEvent = overAction;

            TimeUpdateMgr.S.AddObserver(countDownMgr);
        }

        public void overAction()
        {
            if (CharacterItem == null)
                return;

            TrainingIsOver();
        }

        public override float GetProgress()
        {
            int duration = GetDuration();
            int remainingTime = duration - ComputingTime(StartTime);
            return 1 - (remainingTime * 1.0f) / duration;
        }

        public int GetDurationTime()
        {
            int duration = GetDuration();
            int takeTime = ComputingTime(StartTime);
            return duration - takeTime;
        }

        private int GetDuration()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            //TODO Test
            int duration = 10;//MainGameMgr.S.FacilityMgr.GetDurationForLevel(FacilityType, level);
            return duration;
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

        public void TrainingIsOver()
        {
            this.slotState = SlotState.Free;
            OnCharacterLeave();
            GameDataMgr.S.GetClanData().PraceTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, this);
        }

        public void SelectCharacterItem(CharacterItem characterItem, FacilityType targetFacility)
        {
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);

            StartTime = DateTime.Now.ToString();
            CharacterItem = characterItem;
            characterController.SetState(CharacterStateID.Practice, targetFacility);

            base.slotState = SlotState.Practice;
            // GameDataMgr.S.GetClanData().RefresPracticeDBData(this);
        }
    }

}