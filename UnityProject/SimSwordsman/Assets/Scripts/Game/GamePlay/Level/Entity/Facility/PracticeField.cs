using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PracticeField : BaseSlot
    {
        private PracticeFieldLevelInfo practiceFieldLevelInfo;

        public PracticeField(PracticeFieldLevelInfo item, int index, int unlock) : base(index, unlock)
        {
            FacilityType = item.GetHouseID();
            InitSlotState(item);
            GameDataMgr.S.GetClanData().AddPracticeFieldData(this);
        }
        public PracticeField(PracticeSoltDBData item) : base(item)
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
            if (CharacterItem != null)
            {
                AddExperience(CharacterItem);
                TrainingIsOver();
            }
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
        public void TrainingIsOver()
        {
            SetCharacterItem(CharacterItem, SlotState.Free, FacilityType.None);
            CharacterItem = null;
            StartTime = string.Empty;
            GameDataMgr.S.GetClanData().PraceTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnDisciplePracticeOver, this);
        }

        public void SetCharacterItem(CharacterItem characterItem, SlotState practiceFieldState, FacilityType targetFacility)
        {

            //StartTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(curFacilityType, curLevel);

            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            switch (practiceFieldState)
            {
                case SlotState.Free:
                    characterController.SetState(CharacterStateID.Wander);
                    break;
                case SlotState.CopyScriptures:
                    break;
                case SlotState.Practice:
                    StartTime = DateTime.Now.ToString();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Practice, targetFacility);
                    break;
                default:
                    break;
            }
            slotState = practiceFieldState;
            GameDataMgr.S.GetClanData().RefresPracticeDBData(this);
        }
    }

}