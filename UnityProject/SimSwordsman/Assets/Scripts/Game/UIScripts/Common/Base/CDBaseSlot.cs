using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CDBaseSlot : BaseSlot
    {
        public CDBaseSlot(int index, FacilityView facilityView) : base(index, facilityView) { }

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
                CDIsOver();
            }
        }

        public void CDIsOver()
        {
            this.slotState = SlotState.Free;
            OnCharacterLeave();
            OnCDOver();
        }

        protected virtual void OnCDOver()
        {

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
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(FacilityType, level);
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
    }

}