using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class CopyScripturesItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_CopyScripturesPos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurCopyScriptures;
        [SerializeField]
        private Text m_ArrangeDisciple;
        [SerializeField]
        private Text m_Free;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Button m_CopyScripturesBtn;

        private int m_CountDown = 0;
        private int m_CurLevel;
        private FacilityType m_CurFacility;
        private KungfuLibraySlot m_KungfuLibraySlot = null;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            m_CurFacility = (FacilityType)obj[0];
            m_KungfuLibraySlot = t as KungfuLibraySlot;
            m_CopyScripturesPos.text = "练功位:" + m_KungfuLibraySlot.Index;
            RefreshPracticeFieldState();
        }

        public SlotState GetPracticeFieldState()
        {
            return m_KungfuLibraySlot.slotState;
        }
        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }
        private void BindAddListenerEvent()
        {
            m_CopyScripturesBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.KungfuChooseDisciplePanel, m_KungfuLibraySlot, m_CurFacility);            
            });
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        public void RefreshPracticeFieldState()
        {

            switch (m_KungfuLibraySlot.slotState)
            {
                case SlotState.None:
                    break;
                case SlotState.Free:
                    m_CopyScripturesBtn.enabled = true;
                    m_ArrangeDisciple.text = "安排弟子";
                    m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR;
                    m_Time.text = Define.COMMON_DEFAULT_STR;
                    m_Free.text = "空闲";
                    break;
                case SlotState.NotUnlocked:
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    m_CopyScripturesBtn.enabled = false;
                    m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR; 
                    m_Free.text = "抄经位" + m_KungfuLibraySlot.UnlockLevel + "级后解锁";
                    m_Time.text = Define.COMMON_DEFAULT_STR;
                    break;
                case SlotState.CopyScriptures:
                    RefreshFixedInfo();
                    m_CurCopyScriptures.text = "当前训练:" + m_KungfuLibraySlot.CharacterItem.name;
                    m_Time.text = SplicingTime(GetDuration());
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    m_Free.text = Define.COMMON_DEFAULT_STR;
                    CreateCountDown();
                    //(m_PracticeFieldInfo.StartTime);
                    m_CopyScripturesBtn.enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CreateCountDown()
        {
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index, m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = refresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_KungfuLibraySlot.overAction;
        }
        public void refresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }
        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacility);
        }
        private int GetDuration()
        {
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacility, m_CurLevel);
            int takeTime = ComputingTime(m_KungfuLibraySlot.StartTime);
            if (duration - takeTime <= 0)
                return 0;
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
        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
    }
	
}